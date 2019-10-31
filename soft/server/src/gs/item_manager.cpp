#include "item_manager.h"
#include "item_config.h"
#include "item_operation.h"
#include "item_config.h"
#include "gs_message.h"
#include "item_def.h"
#include "equip_operation.h"
#include "player_operation.h"
#include "role_operation.h"
#include "sport_operation.h"
#include "player_config.h"
#include "mission_config.h"
#include "huodong_pool.h"
#include "mission_operation.h"
#include "utils.h"

ItemManager::ItemManager()
: timer_(0)
{

}

ItemManager::~ItemManager()
{

}

int ItemManager::init()
{
	if (-1 == sItemConfig->parse())
	{
		return -1;
	}
	timer_ = game::timer()->schedule(boost::bind(&ItemManager::update, this, _1), ITEM_PERIOD, "item");
	last_time_ = game::timer()->now();
	return 0;
}

int ItemManager::fini()
{
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = 0;
	}
	return 0;
}

int ItemManager::update(ACE_Time_Value tv)
{
	dhc::global_t *global_t = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (global_t)
	{
		int last_index = -1;
		if (game::timer()->trigger_time(global_t->guild_refresh_time(), 9, 0))
		{
			last_index = 3;
		}
		else if (game::timer()->trigger_time(global_t->guild_refresh_time(), 12, 0))
		{
			last_index = 1;
		}
		else if (game::timer()->trigger_time(global_t->guild_refresh_time(), 18, 0))
		{
			last_index = 2;
		}
		else if (game::timer()->trigger_time(global_t->guild_refresh_time(), 21, 0))
		{
			last_index = 4;
		}
		else if (game::timer()->trigger_time(global_t->guild_refresh_time(), 6, 0))
		{
			last_index = 0;
		}

		if (last_index != -1)
		{
			global_t->set_guild_refresh_time(game::timer()->now());

			if (last_index != 0)
			{
				std::vector<uint64_t> guild_guids;
				game::pool()->get_entitys(et_guild, guild_guids);
				for (int i = 0; i < guild_guids.size(); ++i)
				{
					dhc::guild_t *guild = POOL_GET_GUILD(guild_guids[i]);
					if (guild)
					{
						ItemOperation::refresh_guild_shop(guild);
					}
				}
			}

			std::vector<uint64_t> player_guids;
			game::pool()->get_entitys(et_player, player_guids);
			uint64_t now_time = game::timer()->now();
			for (int i = 0; i < player_guids.size(); ++i)
			{
				dhc::player_t* player = POOL_GET_PLAYER(player_guids[i]);
				if (player)
				{
					if (last_index != 0)
					{
						ItemOperation::refresh_huiyi_shop(player);
						ItemOperation::refresh_guild_shop(player);
						player->set_huiyi_shop_last_time(now_time);
					}
					if (last_index >= 0 && last_index <= 2)
					{
						MissionOperation::refresh_qiyu(player, last_index);
						player->set_qiyu_last_time(now_time);
					}
				}
			}
		}
	}
	
	return 0;
}

void ItemManager::terminal_item_sell(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_item_sell msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	int num = ItemOperation::item_num_templete(player, msg.item_id());
	if (num <= 0)
	{
		PERROR(ERROR_ITEM_NULL);
		return;
	}
	if (num < msg.item_num())
	{
		GLOBAL_ERROR;
		return;
	}
	num = msg.item_num();

	s_t_item *t_item = sItemConfig->get_item(msg.item_id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_item->type == 3001)
	{
		PlayerOperation::player_add_resource(player, resource::ZHANHUN, num * t_item->sell, LOGWAY_ITEM_SELL);
	}
	else if (t_item->type == 7001)
	{
		PlayerOperation::player_add_resource(player, resource::HEJIN, num * t_item->sell, LOGWAY_ITEM_SELL);
	}
	else if (t_item->type == 9001)
	{
		PlayerOperation::player_add_resource(player, resource::HUIYI_POINT, num * t_item->sell, LOGWAY_ITEM_SELL);
	}
	else
	{
		PlayerOperation::player_add_resource(player, resource::GOLD, num * t_item->sell, LOGWAY_ITEM_SELL);
	}
	ItemOperation::item_destory_templete(player, msg.item_id(), num, LOGWAY_ITEM_SELL);
	ResMessage::res_success(player, true, name, id);
}

void ItemManager::terminal_item_sell_all(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_item_sell_all msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	s_t_rewards rds;
	const s_t_item* t_item = 0;
	int num = 0;
	for (int i = 0; i < msg.item_ids_size(); ++i)
	{
		t_item = sItemConfig->get_item(msg.item_ids(i));
		if (!t_item)
		{
			PERROR(ERROR_ITEM_NULL);
			return;
		}
		num = ItemOperation::item_num_templete(player, msg.item_ids(i));
		if (num <= 0)
		{
			PERROR(ERROR_ITEM_NULL);
			return;
		}

		if (t_item->type == 3001)
		{
			rds.add_reward(1, resource::ZHANHUN, num * t_item->sell);
		}
		else if (t_item->type == 7001)
		{
			rds.add_reward(1, resource::HEJIN, num * t_item->sell);
		}
		else if (t_item->type == 9001)
		{
			rds.add_reward(1, resource::HUIYI_POINT, num * t_item->sell);
		}
		else
		{
			rds.add_reward(1, resource::GOLD, num * t_item->sell);
		}
		ItemOperation::item_destory_templete(player, msg.item_ids(i), num, LOGWAY_ITEM_SELL);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ITEM_SELL);

	ResMessage::res_success(player, true, name, id);
}

void ItemManager::terminal_item_fenjie(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_item_fenjie msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	const s_t_item *t_item = 0;
	int num = 0;
	std::vector<s_t_reward> items;
	s_t_reward rd;
	std::set<uint32_t> ids;
	for (int i = 0; i < msg.item_id_size(); ++i)
	{
		t_item = sItemConfig->get_item(msg.item_id(i));
		if (!t_item)
		{
			GLOBAL_ERROR;
			return;
		}

		num = ItemOperation::item_num_templete(player, msg.item_id(i));
		if (!num)
		{
			GLOBAL_ERROR;
			return;
		}
		if (t_item->type != 3001)
		{
			GLOBAL_ERROR;
			return;
		}
		if (ids.find(msg.item_id(i)) != ids.end())
		{
			continue;
		}
		rd.type = (int)msg.item_id(i);
		rd.value1 = num;
		rd.value2 = t_item->sell;
		items.push_back(rd);
	}

	for (int i = 0; i < items.size(); ++i)
	{
		const s_t_reward &rdd = items[i];
		PlayerOperation::player_add_resource(player, resource::ZHANHUN, rdd.value1 * rdd.value2, LOGWAY_ROLE_FENJIE);
		ItemOperation::item_destory_templete(player, rdd.type, rdd.value1, LOGWAY_ROLE_FENJIE);
	}

	ResMessage::res_success(player, true, name, id);
}

void ItemManager::terminal_item_apply(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_item_apply msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.item_count() < 1 || msg.item_count() > 100)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_item *t_item = sItemConfig->get_item(msg.item_id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_item->type != 1001)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_item->level > player->level())
	{
		PERROR(ERROR_LEVEL);
		return;
	}

	s_t_itemstore *t_itemstore = sItemConfig->get_itemstore(t_item->def1);
	if (!t_itemstore)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, t_item->id) < msg.item_count())
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_item->id == 10010062)
	{
		if (player->treasures_size() + 5 * msg.item_count() > 100)
		{
			PERROR(ERROR_TREASURE_BAG_FULL);
			return;
		}
	}

	bool has_jieri_huodong = sHuodongPool->has_jieri_huodong(player);
	s_t_rewards rds;
	for (int item_index = 0; item_index < msg.item_count(); ++item_index)
	{
		if (t_itemstore->type == 1)
		{
			rds.add_reward(t_itemstore->rewards);
		}
		else if (t_itemstore->type == 3)
		{
			if (msg.item_index() < 0 || msg.item_index() >= t_itemstore->rewards.size())
			{
				GLOBAL_ERROR;
				return;
			}
			rds.add_reward(t_itemstore->rewards[msg.item_index()]);
		}
		else
		{
			int sum = 0;
			for (int i = 0; i < t_itemstore->rates.size(); ++i)
			{
				if (!has_jieri_huodong && (t_itemstore->rewards[i].value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
					t_itemstore->rewards[i].value1 == s_t_rewards::HUODONG_ITEM_ID2))
				{
					continue;
				}
				sum += t_itemstore->rates[i];
			}
			if (sum == 0)
			{
				GLOBAL_ERROR;
				return;
			}
			int r = Utils::get_int32(0, sum - 1);
			int gl = 0;
			for (int i = 0; i < t_itemstore->rates.size(); ++i)
			{
				if (!has_jieri_huodong && (t_itemstore->rewards[i].value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
					t_itemstore->rewards[i].value1 == s_t_rewards::HUODONG_ITEM_ID2))
				{
					continue;
				}
				gl += t_itemstore->rates[i];
				if (gl > r)
				{
					s_t_reward reward = t_itemstore->rewards[i];
					if (reward.type == 2 && reward.value3 > 0)
					{
						reward.value2 = Utils::get_int32(reward.value2, reward.value3);
						reward.value3 = 0;
					}
					rds.add_reward(reward);
					break;
				}
			}
		}
	}
	
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ITEM_APPLY);
	ItemOperation::item_destory_templete(player, t_item->id, msg.item_count(), LOGWAY_ITEM_APPLY);

	ResMessage::res_item_apply(player, rds, name, id);
}

void ItemManager::terminal_item_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_item_direct_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.item_count() < 1 || msg.item_count() > 100)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_item *t_item = sItemConfig->get_item(msg.item_id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < t_item->jewel * msg.item_count())
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	ItemOperation::item_add_template(player, msg.item_id(), msg.item_count(), LOGWAY_ITEM_APPLY);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_item->jewel * msg.item_count(), LOGWAY_ITEM_APPLY);

	ResMessage::res_success(player, true, name, id);
}

void ItemManager::terminal_item_hecheng(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_item_apply msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.item_count() <= 0 || msg.item_count() >= 5000)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_item_hecheng* t_hecheng = sItemConfig->get_item_hecheng(msg.item_id());
	if (!t_hecheng)
	{
		GLOBAL_ERROR;
		return;
	}

	/// 道具
	if (t_hecheng->hecheng.type == 2)
	{
		const s_t_item* t_item = sItemConfig->get_item(t_hecheng->hecheng.value1);
		if (!t_item)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	/// 光环
	else if (t_hecheng->hecheng.type == 7)
	{
		if (RoleOperation::player_has_guanghuan(player, t_hecheng->hecheng.value1))
		{
			GLOBAL_ERROR;
			return;
		}
	}

	for (std::vector<s_t_reward>::size_type i = 0;
		i < t_hecheng->cailiao.size();
		++i)
	{
		if (t_hecheng->cailiao[i].type == 2)
		{
			if (ItemOperation::item_num_templete(player, t_hecheng->cailiao[i].value1) < t_hecheng->cailiao[i].value2 * msg.item_count())
			{
				PERROR(ERROR_CAILIAO);
				return;
			}
		}
		else
		{
			GLOBAL_ERROR;
			return;
		}
	}
	for (std::vector<s_t_reward >::size_type i = 0;
		i < t_hecheng->cailiao.size();
		++i)
	{
		if (t_hecheng->cailiao[i].type == 2)
		{
			ItemOperation::item_destory_templete(player, t_hecheng->cailiao[i].value1, t_hecheng->cailiao[i].value2 * msg.item_count(), LOGWAY_ITEM_HECHENG);
		}	
	}

	/// 道具
	if (t_hecheng->hecheng.type == 2)
	{
		ItemOperation::item_add_template(player, t_hecheng->hecheng.value1, t_hecheng->hecheng.value2 * msg.item_count(), LOGWAY_ITEM_HECHENG);
	}
	/// 光环
	else if (t_hecheng->hecheng.type == 7)
	{
		RoleOperation::player_guanghuan_get(player, t_hecheng->hecheng.value1);
	}
	ResMessage::res_success(player, true, name, id);
}

void ItemManager::terminal_shop_check(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	ItemOperation::item_do_role_shop(player);
	ItemOperation::do_pet_shop(player);
	ResMessage::res_shop_refresh(player, name, id);
}

void ItemManager::terminal_shop_refresh(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_refresh msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.type() != 2)
	{
		PERROR(ERROR_SHOP_NULL);
		return;
	}

	ItemOperation::item_do_role_shop(player);

	if (player->shop2_refresh_num() > 0)
	{
		player->set_shop2_refresh_num(player->shop2_refresh_num() - 1);
	}
	else
	{
		const s_t_vip* t_vip = sPlayerConfig->get_vip(player->vip());
		if (!t_vip)
		{
			GLOBAL_ERROR;
			return;
		}

		if (player->shop4_refresh_num() >= t_vip->shop_refresh)
		{
			GLOBAL_ERROR;
			return;
		}
		bool has_stone = (ItemOperation::item_num_templete(player, SHOP_REFRESH_STONE) >= 1) ? true : false;
		if (has_stone)
		{
			ItemOperation::item_destory_templete(player, SHOP_REFRESH_STONE, 1, LOGWAY_SHOP_REFRESH);
		}
		else
		{
			if (player->jjc_point() < 20)
			{
				PERROR(ERROR_JJCPOINT);
				return;
			}
			PlayerOperation::player_dec_resource(player, resource::ZHANHUN, 20, LOGWAY_SHOP_REFRESH);
		}
		player->set_shop4_refresh_num(player->shop4_refresh_num() + 1);
	}

	ItemOperation::refresh_role_shop(player);
	player->set_shop_refresh_num(player->shop_refresh_num() + 1);
	sHuodongPool->huodong_active(player, HUODONG_COND_HBSD_COUNT, 1);
	ResMessage::res_shop_refresh(player, name, id);
}

void ItemManager::terminal_shop_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.type() != 2)
	{
		GLOBAL_ERROR;
		return;
	}

	int gezi = msg.gezi() - 1;
	if (gezi < 0 || gezi >= player->shop2_ids_size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->shop2_sell(gezi))
	{
		PERROR(ERROR_SHOP_SELLED);
		return;
	}

	s_t_shop *t_shop = sItemConfig->get_shop(player->shop2_ids(gezi));
	if (!t_shop)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_shop->hb_type == 1)
	{
		if (player->gold() < t_shop->hb)
		{
			PERROR(ERROR_GOLD);
			return;
		}
	}
	else if (t_shop->hb_type == 2)
	{
		if (player->jewel() < t_shop->hb)
		{
			PERROR(ERROR_JEWEL);
			return;
		}
	}
	else if (t_shop->hb_type == 5)
	{
		if (player->jjc_point() < t_shop->hb)
		{
			PERROR(ERROR_JJCPOINT);
			return;
		}
	}
	else if (t_shop->hb_type == 6)
	{
		if (player->mw_point() < t_shop->hb)
		{
			PERROR(ERROR_MWPOINT);
			return;
		}
	}

	s_t_rewards rds;
	rds.add_reward(t_shop->type, t_shop->value1, t_shop->value2, t_shop->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_SHOP_BUY);

	if (t_shop->hb_type == 1)
	{
		PlayerOperation::player_dec_resource(player, resource::GOLD, t_shop->hb, LOGWAY_SHOP_BUY);
	}
	else if (t_shop->hb_type == 2)
	{
		PlayerOperation::player_dec_resource(player, resource::JEWEL, t_shop->hb, LOGWAY_SHOP_BUY);
	}
	else if (t_shop->hb_type == 5)
	{
		PlayerOperation::player_dec_resource(player, resource::ZHANHUN, t_shop->hb, LOGWAY_SHOP_BUY);
	}
	else if (t_shop->hb_type == 6)
	{
		PlayerOperation::player_dec_resource(player, resource::HEJIN, t_shop->hb, LOGWAY_SHOP_BUY);
	}

	player->set_shop2_sell(gezi, 1);
	player->set_shop_buy_num(player->shop_buy_num() + 1);
	player->set_hs_task_num(player->hs_task_num() + 1);
	PlayerOperation::player_add_active(player, 1100, 1);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_xg(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_xg msg;
 	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.shop_num() <= 0 || msg.shop_num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_shop_xg *t_shop_xg = sItemConfig->get_shop_xg(msg.shop_id());
	if (!t_shop_xg)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->level() < t_shop_xg->xg_level)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->total_recharge() < t_shop_xg->recharge)
	{
		GLOBAL_ERROR;
		return;
	}

	int jewel = 0;
	switch (t_shop_xg->xg_type)
	{
	case 0:
		{
			  jewel = t_shop_xg->price * msg.shop_num();
		}
		break;
	case 1:
		{
			  for (int i = 0; i < player->shop_xg_ids_size(); ++i)
			  {
				  if (player->shop_xg_ids(i) == t_shop_xg->id)
				  {
					  if (player->shop_xg_nums(i) >= t_shop_xg->xg_num)
					  {
						  GLOBAL_ERROR;
						  return;
					  }
					  if (msg.shop_num() + player->shop_xg_nums(i) > t_shop_xg->xg_num)
					  {
						  GLOBAL_ERROR;
						  return;
					  }
					  break;
				  }
			  }
			  jewel = t_shop_xg->price * msg.shop_num();
		}
		break;
	case 2:
		{
			  int price_type = t_shop_xg->price_type - 1;
			  int buy_index = 0;
			  int xgg_num = 0;
			  if (player->vip() < 0 || player->vip() >= t_shop_xg->vip_xg_num.size())
			  {
				  GLOBAL_ERROR;
				  return;
			  }
			  xgg_num = t_shop_xg->vip_xg_num[player->vip()];
			  for (int i = 0; i < player->shop_xg_ids_size(); ++i)
			  {
				  if (player->shop_xg_ids(i) == t_shop_xg->id)
				  {
					  if (player->shop_xg_nums(i) >= xgg_num)
					  {
						  GLOBAL_ERROR;
						  return;
					  }
					  if (msg.shop_num() + player->shop_xg_nums(i) > xgg_num)
					  {
						  GLOBAL_ERROR;
						  return;
					  }
					  buy_index = player->shop_xg_nums(i);
					  break;
				  }
			  }
			  const s_t_price *t_price = 0;
			  for (int i = 0; i < msg.shop_num(); ++i)
			  {
				  t_price = sPlayerConfig->get_price(buy_index + i + 1);
				  if (!t_price || price_type < 0 || price_type >= t_price->xgs.size())
				  {
					  GLOBAL_ERROR;
					  return;
				  }
				  jewel += t_price->xgs[price_type];
			  }
		}
		break;
	default:
		{
			   GLOBAL_ERROR;
			   return;
		}
	}

	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	s_t_rewards rds;
	for (int i = 0; i < msg.shop_num(); ++i)
	{
		rds.add_reward(t_shop_xg->xg_item);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_SHOP_BUY);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_SHOP_BUY);

	if (t_shop_xg->xg_type)
	{
		bool flag = false;
		for (int i = 0; i < player->shop_xg_ids_size(); ++i)
		{
			if (player->shop_xg_ids(i) == t_shop_xg->id)
			{
				player->set_shop_xg_nums(i, player->shop_xg_nums(i) + msg.shop_num());
				flag = true;
			}
		}
		if (!flag)
		{
			player->add_shop_xg_ids(t_shop_xg->id);
			player->add_shop_xg_nums(msg.shop_num());
		}
	}
	player->set_hs_task_num(player->hs_task_num() + msg.shop_num());
	PlayerOperation::player_add_active(player, 1100, msg.shop_num());

	if (t_shop_xg->price_type == 2)
	{
		PlayerOperation::player_add_active(player, 1900, msg.shop_num());
	}
	else if (t_shop_xg->price_type == 3)
	{
		PlayerOperation::player_add_active(player, 2000, msg.shop_num());
	}

	ResMessage::res_shop_buy(player, rds, name, id);
}


void ItemManager::terminal_shop_boss(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_boss_shop *t_shop_other = sItemConfig->get_boss_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->level() < t_shop_other->level)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_shop_other->price > 0)
	{
		if (player->medal_point() < t_shop_other->price * msg.num())
		{
			GLOBAL_ERROR;
			return;
		}
	}
	
	if (t_shop_other->hongsehuoban > 0)
	{
		if (ItemOperation::item_num_templete(player, 50110001) < t_shop_other->hongsehuoban * msg.num())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (t_shop_other->price > 0)
	{
		PlayerOperation::player_dec_resource(player, resource::MW_MEDAL, t_shop_other->price * msg.num(), LOGWAY_SHOP_BUY);
	}

	if (t_shop_other->hongsehuoban > 0)
	{
		ItemOperation::item_destory_templete(player, 50110001, t_shop_other->hongsehuoban * msg.num(), LOGWAY_SHOP_BUY);
	}
	

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_SHOP_BUY);
	player->set_hs_task_num(player->hs_task_num() + 1);
	PlayerOperation::player_add_active(player, 1100, 1);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_ttt(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_ttt_shop *t_shop_other = sItemConfig->get_ttt_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->mw_point() < t_shop_other->price * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::HEJIN, t_shop_other->price * msg.num(), LOGWAY_SHOP_BUY);

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_SHOP_BUY);
	player->set_hs_task_num(player->hs_task_num() + 1);
	PlayerOperation::player_add_active(player, 1100, 1);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_guild(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_shop *t_shop_other = sItemConfig->get_guild_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_shop_other->guild_level > guild->level())
	{
		GLOBAL_ERROR;
		return;
	}
	
	int buy_num = ItemOperation::get_guild_shop_buy_num(player, msg.item_id());
	int avail_num = t_shop_other->num - buy_num;
	if (msg.num() > avail_num)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_shop_other->contribution > 0)
	{
		if (player->contribution() < t_shop_other->contribution * msg.num())
		{
			GLOBAL_ERROR;
			return;
		}
	}
	if (t_shop_other->hongsehuoban > 0)
	{
		if (ItemOperation::item_num_templete(player, 50110001) < t_shop_other->hongsehuoban * msg.num())
		{
			GLOBAL_ERROR;
			return;
		}
	}
	if (t_shop_other->contribution > 0)
	{
		PlayerOperation::player_dec_resource(player, resource::CONTRIBUTION, t_shop_other->contribution * msg.num(), LOGWAY_SHOP_BUY);
	}
	if (t_shop_other->hongsehuoban > 0)
	{
		ItemOperation::item_destory_templete(player, 50110001, t_shop_other->hongsehuoban * msg.num(), LOGWAY_SHOP_BUY);
	}
	
	ItemOperation::add_guild_shop_buy_num(player, msg.item_id(), msg.num());

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_SHOP_BUY);
	player->set_hs_task_num(player->hs_task_num() + 1);
	PlayerOperation::player_add_active(player, 1100, 1);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_guild_ex(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_shop_ex* t_mubiao = sItemConfig->get_guild_shop_ex(msg.item_id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_mubiao->type == 7)
	{
		if (RoleOperation::player_has_guanghuan(player, t_mubiao->value1))
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (guild->level() < t_mubiao->level)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->guild_shop_ex_rewards_size(); ++i)
	{
		if (player->guild_shop_ex_rewards(i) == msg.item_id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->contribution() < t_mubiao->point * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < t_mubiao->price * msg.num())
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::CONTRIBUTION, t_mubiao->point * msg.num(), LOGWAY_GUILD_SHOP_EX);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_mubiao->price * msg.num(), LOGWAY_GUILD_SHOP_EX);
	player->add_guild_shop_ex_rewards(msg.item_id());
	s_t_rewards rds;
	rds.add_reward(t_mubiao->type, t_mubiao->value1, t_mubiao->value2, t_mubiao->vlaue3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_GUILD_SHOP_EX);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_sport(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_sport_shop *t_shop_other = sItemConfig->get_sport_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->level() < t_shop_other->level)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_shop_other->spower > 0)
	{
		if (player->powder() < t_shop_other->spower * msg.num())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (t_shop_other->hongsehuoban > 0)
	{
		if (ItemOperation::item_num_templete(player, 50110001) < t_shop_other->hongsehuoban * msg.num())
		{
			GLOBAL_ERROR;
			return;
		}
	}
	
	if (t_shop_other->spower > 0)
	{
		PlayerOperation::player_dec_resource(player, resource::JJ_POINT, t_shop_other->spower * msg.num(), LOGWAY_SHOP_BUY);
	}
	
	if (t_shop_other->hongsehuoban > 0)
	{
		ItemOperation::item_destory_templete(player, 50110001, t_shop_other->hongsehuoban * msg.num(), LOGWAY_SHOP_BUY);
	}

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_SHOP_BUY);
	player->set_hs_task_num(player->hs_task_num() + 1);
	PlayerOperation::player_add_active(player, 1100, 1);

	ResMessage::res_shop_buy(player, rds, name, id);
}


void ItemManager::terminal_baozang_ttt(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	const s_t_ttt_baozang* t_mibao = sItemConfig->get_ttt_baozhang(player->ttt_mibao());
	if (!t_mibao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < t_mibao->price)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	s_t_rewards rds;
	rds.add_reward(t_mibao->type, t_mibao->value1, t_mibao->value2, t_mibao->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TTT_MIBAO);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_mibao->price, LOGWAY_TTT_MIBAO);
	player->set_ttt_mibao(0);

	ResMessage::res_huodong_reward(player, rds, name, id);
}

void ItemManager::terminal_mubiao_ttt(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_ttt_mubiao* t_mubiao = sItemConfig->get_ttt_mubiao(msg.item_id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	int star = 0;
	for (int i = 0; i < player->ttt_last_stars_size(); ++i)
	{
		star += player->ttt_last_stars(i);
	}
	if (star < t_mubiao->star)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->ttt_shop_rewards_size(); ++i)
	{
		if (player->ttt_shop_rewards(i) == msg.item_id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->mw_point() < t_mubiao->price * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::HEJIN, t_mubiao->price * msg.num(), LOGWAY_MUBIAO_TTT_BUY);
	player->add_ttt_shop_rewards(msg.item_id());


	s_t_rewards rds;
	rds.add_reward(t_mubiao->type, t_mubiao->value1, t_mubiao->value2, t_mubiao->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_MUBIAO_TTT_BUY);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_mubiao_guild(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_mubiao* t_mubiao = sItemConfig->get_guild_mubiao(msg.item_id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->level() < t_mubiao->level)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->guild_shop_rewards_size(); ++i)
	{
		if (player->guild_shop_rewards(i) == msg.item_id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->contribution() < t_mubiao->price * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::CONTRIBUTION, t_mubiao->price * msg.num(), LOGWAY_MUBIAO_GUILD_BUY);
	player->add_guild_shop_rewards(msg.item_id());


	s_t_rewards rds;
	rds.add_reward(t_mubiao->type, t_mubiao->value1, t_mubiao->value2, t_mubiao->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_MUBIAO_GUILD_BUY);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_mubiao_sport(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_sport_mubiao* t_mubiao = sItemConfig->get_sport_mubiao(msg.item_id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->max_rank() > t_mubiao->rank)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->sport_shop_rewards_size(); ++i)
	{
		if (player->sport_shop_rewards(i) == msg.item_id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->powder() < t_mubiao->price * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::JJ_POINT, t_mubiao->price * msg.num(), LOGWAY_MUBIAO_SPORT_BUY);
	player->add_sport_shop_rewards(msg.item_id());


	s_t_rewards rds;
	rds.add_reward(t_mubiao->type, t_mubiao->value1, t_mubiao->value2, t_mubiao->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_MUBIAO_SPORT_BUY);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_time_guild(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	
	int gezi = msg.gezi() - 1;
	if (gezi < 0 || gezi >= guild->shop_ids_size())
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_shop_xs* t_shop = sItemConfig->get_guild_shop_xs(guild->shop_ids(gezi));
	if (!t_shop)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->shop1_ids_size(); ++i)
	{
		if (player->shop1_ids(i) == t_shop->id)
		{
			PERROR(ERROR_SHOP_SELLED);
			return;
		}
	}
	if (guild->shop_nums(gezi) >= t_shop->num)
	{
		PERROR(ERROR_SHOP_SELLED);
		return;
	}
	if (player->level() < t_shop->level)
	{
		PERROR(ERROR_LEVEL);
		return;
	}
	if (player->jewel() < t_shop->jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	
	player->add_shop1_ids(t_shop->id);
	player->add_shop1_sell(1);
	guild->set_shop_nums(gezi, guild->shop_nums(gezi) + 1);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_shop->jewel, LOGWAY_TIME_GUILD_SHOP);
	s_t_rewards rds;
	rds.add_reward(t_shop->type, t_shop->value1, t_shop->value2, t_shop->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TIME_GUILD_SHOP);
	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_huiyi(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.type() != 4)
	{
		GLOBAL_ERROR;
		return;
	}

	int gezi = msg.gezi() - 1;
	if (gezi < 0 || gezi >= player->shop4_ids_size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->shop4_sell(gezi))
	{
		PERROR(ERROR_SHOP_SELLED);
		return;
	}

	const s_t_huiyi_shop *t_shop = sItemConfig->get_huiyi_shop(player->shop4_ids(gezi));
	if (!t_shop)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_shop->huobi == 2)
	{
		if (player->jewel() < t_shop->price)
		{
			PERROR(ERROR_JEWEL);
			return;
		}
	}
	else if (t_shop->huobi == 21)
	{
		if (player->huiyi_point() < t_shop->price)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else
	{
		GLOBAL_ERROR;
		return;
	}


	s_t_rewards rds;
	rds.add_reward(t_shop->type, t_shop->value1, t_shop->value2, t_shop->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUIYI_SHOP);

	if (t_shop->huobi == 2)
	{
		PlayerOperation::player_dec_resource(player, resource::JEWEL, t_shop->price, LOGWAY_HUIYI_SHOP);
	}
	else if (t_shop->huobi == 21)
	{
		PlayerOperation::player_dec_resource(player, resource::HUIYI_POINT, t_shop->price, LOGWAY_HUIYI_SHOP);
	}

	player->set_shop4_sell(gezi, 1);
	player->set_shop_buy_num(player->shop_buy_num() + 1);
	player->set_hs_task_num(player->hs_task_num() + 1);
	PlayerOperation::player_add_active(player, 1100, 1);
	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_huiyi_luck(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	const s_t_huiyi_luck_shop* t_shop_other = sItemConfig->get_huiyi_luckshop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	int buy_num = ItemOperation::get_huiyi_luckshop_buy_num(player, msg.item_id());
	int avail_num = t_shop_other->num - buy_num;
	if (msg.num() > avail_num)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->luck_point() < t_shop_other->luck_point * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::LUCK_POINT, t_shop_other->luck_point * msg.num(), LOGWAY_LUCK_SHOP);
	ItemOperation::add_huiyi_luckshop_buy_num(player, msg.item_id(), msg.num());

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_LUCK_SHOP);
	player->set_hs_task_num(player->hs_task_num() + 1);
	PlayerOperation::player_add_active(player, 1100, 1);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_lieren(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_lieren_shop *t_shop_other = sItemConfig->get_lieren_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_shop_other->huobi > 0)
	{
		if (player->lieren_point() < t_shop_other->huobi * msg.num())
		{
			GLOBAL_ERROR;
			return;
		}
	}
	if (t_shop_other->hongsezhuangbei > 0)
	{
		if (ItemOperation::item_num_templete(player, 50120001) < t_shop_other->hongsezhuangbei * msg.num())
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}
	if (t_shop_other->hongsehuoban > 0)
	{
		if (ItemOperation::item_num_templete(player, 50110001) < t_shop_other->hongsehuoban * msg.num())
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}
	if (t_shop_other->huobi > 0)
		PlayerOperation::player_dec_resource(player, resource::LIEREN_POINT, t_shop_other->huobi * msg.num(), LOGWAY_LIEREN_SHOP);
	if (t_shop_other->hongsehuoban)
		ItemOperation::item_destory_templete(player, 50110001, t_shop_other->hongsehuoban * msg.num(), LOGWAY_LIEREN_SHOP);
	if (t_shop_other->hongsezhuangbei)
		ItemOperation::item_destory_templete(player, 50120001, t_shop_other->hongsezhuangbei * msg.num(), LOGWAY_LIEREN_SHOP);

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_LIEREN_SHOP);
	player->set_hs_task_num(player->hs_task_num() + msg.num());
	PlayerOperation::player_add_active(player, 1100, msg.num());

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_bingyuan(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_bingyuan_shop *t_shop_other = sItemConfig->get_bingyuan_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	int buy_num = 0;
	int buy_index = -1;
	for (int i = 0; i < player->by_shops_size(); ++i)
	{
		if (player->by_shops(i) == msg.item_id())
		{
			buy_num = player->by_nums(i);
			buy_index = i;
			break;
		}
	}

	if (t_shop_other->num > 0)
	{
		if (buy_num + msg.num() > t_shop_other->num)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	

	if (player->bingjing() < t_shop_other->bingjing * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::BINGJING, t_shop_other->bingjing * msg.num(), LOGWAY_BINGYUAN_SHOP);
	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_BINGYUAN_SHOP);
	player->set_hs_task_num(player->hs_task_num() + msg.num());
	PlayerOperation::player_add_active(player, 1100, msg.num());

	if (buy_index == -1)
	{
		player->add_by_shops(msg.item_id());
		player->add_by_nums(msg.num());
	}
	else
	{
		player->set_by_nums(buy_index, player->by_nums(buy_index) + msg.num());
	}

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_reward_bingyuan(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_bingyuan_mubiao* t_mubiao = sItemConfig->get_bingyuan_mubiao(msg.item_id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->by_rewards_size(); ++i)
	{
		if (player->by_rewards(i) == msg.item_id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->bingjing() < t_mubiao->price * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::BINGJING, t_mubiao->price * msg.num(), LOGWAY_BINGYUAN_MUBIAO);
	player->add_by_rewards(msg.item_id());
	s_t_rewards rds;
	rds.add_reward(t_mubiao->type, t_mubiao->value1, t_mubiao->value2, t_mubiao->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_BINGYUAN_MUBIAO);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_chongzhifanpai(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_CZFP_TYPE);
	if (!huodong_player)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_chongzhifanpai_shop *t_shop_other = sItemConfig->get_chongzhifanpai_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	if (huodong_player->arg1() < t_shop_other->price * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_CHONGZHIFANPAI_SHOP);
	huodong_player->set_arg1(huodong_player->arg1() - t_shop_other->price * msg.num());
	sHuodongPool->save_huodong_player(huodong_player->guid());

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_chongwu(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	int gezi = msg.gezi() - 1;
	if (gezi < 0 || gezi >= player->shoppet_ids_size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->shoppet_sell(gezi))
	{
		PERROR(ERROR_SHOP_SELLED);
		return;
	}

	const s_t_chongwu_shop* t_shop = sItemConfig->get_chongwu_shop(player->shoppet_ids(gezi));
	if (!t_shop)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_shop->huobi == 1)
	{
		if (player->gold() < t_shop->price)
		{
			PERROR(ERROR_GOLD);
			return;
		}
	}
	else if (t_shop->huobi == 2)
	{
		if (player->jewel() < t_shop->price)
		{
			PERROR(ERROR_JEWEL);
			return;
		}
	}
	else if (t_shop->huobi == 3)
	{
		if (player->jjc_point() < t_shop->price)
		{
			PERROR(ERROR_JJCPOINT);
			return;
		}
	}
	else if (t_shop->huobi == 4)
	{
		if (player->mw_point() < t_shop->price)
		{
			PERROR(ERROR_MWPOINT);
			return;
		}
	}
	else if (t_shop->huobi == 27)
	{
		if (player->xinpian() < t_shop->price)
		{
			PERROR(ERROR_XINPIAN);
			return;
		}
	}
	else
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	rds.add_reward(t_shop->type, t_shop->value1, t_shop->value2, t_shop->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_CHONGWU_SHOP);

	if (t_shop->huobi == 1)
	{
		PlayerOperation::player_dec_resource(player, resource::GOLD, t_shop->price, LOGWAY_CHONGWU_SHOP);
	}
	else if (t_shop->huobi == 2)
	{
		PlayerOperation::player_dec_resource(player, resource::JEWEL, t_shop->price, LOGWAY_CHONGWU_SHOP);
	}
	else if (t_shop->huobi == 3)
	{
		PlayerOperation::player_dec_resource(player, resource::ZHANHUN, t_shop->price, LOGWAY_CHONGWU_SHOP);
	}
	else if (t_shop->huobi == 4)
	{
		PlayerOperation::player_dec_resource(player, resource::HEJIN, t_shop->price, LOGWAY_CHONGWU_SHOP);
	}
	else if (t_shop->huobi == 27)
	{
		PlayerOperation::player_dec_resource(player, resource::XINPIAN, t_shop->price, LOGWAY_CHONGWU_SHOP);
	}

	player->set_shoppet_sell(gezi, 1);
	player->set_shop_buy_num(player->shop_buy_num() + 1);
	player->set_hs_task_num(player->hs_task_num() + 1);
	PlayerOperation::player_add_active(player, 1100, 1);

	ResMessage::res_shop_buy(player, rds, name, id);
}

void ItemManager::terminal_shop_chongwu_refresh(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_refresh msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;


	ItemOperation::do_pet_shop(player);

	if (player->shoppet_refresh_num() > 0)
	{
		player->set_shoppet_refresh_num(player->shoppet_refresh_num() - 1);
	}
	else
	{
		const s_t_vip* t_vip = sPlayerConfig->get_vip(player->vip());
		if (!t_vip)
		{
			GLOBAL_ERROR;
			return;
		}

		if (player->shoppet_num() >= t_vip->shop_refresh)
		{
			GLOBAL_ERROR;
			return;
		}
		bool has_stone = (ItemOperation::item_num_templete(player, SHOP_REFRESH_STONE) >= 1) ? true : false;
		if (has_stone)
		{
			ItemOperation::item_destory_templete(player, SHOP_REFRESH_STONE, 1, LOGWAY_CHONGWU_SHOP_REFRESH);
		}
		else
		{
			if (player->jewel() < 20)
			{
				PERROR(ERROR_JEWEL);
				return;
			}
			PlayerOperation::player_dec_resource(player, resource::JEWEL, 20, LOGWAY_CHONGWU_SHOP_REFRESH);
		}
		player->set_shoppet_num(player->shoppet_num() + 1);
	}

	ItemOperation::refresh_pet_shop(player);
	ResMessage::res_shop_refresh(player, name, id);
}

void ItemManager::terminal_shop_mofang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
	if (!huodong_player)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_mofang_shop *t_shop_other = sItemConfig->get_mofang_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	if (huodong_player->arg4() < t_shop_other->price * msg.num())
	{
		GLOBAL_ERROR;
		return;
	}

	int buy_index = -1;
	if (t_shop_other->num > 0)
	{
		int buy_num = 0;
		for (int i = 0; i < huodong_player->args4_size(); ++i)
		{
			if (huodong_player->args4(i) == t_shop_other->id)
			{
				buy_num = huodong_player->args5(i);
				buy_index = i;
				break;
			}
		}
		if (buy_num + msg.num() > t_shop_other->num)
		{
			GLOBAL_ERROR;
			return;
		}
	}

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_MOFANG_SHOP);
	huodong_player->set_arg4(huodong_player->arg4() - t_shop_other->price * msg.num());
	if (t_shop_other->num > 0)
	{
		if (buy_index == -1)
		{
			huodong_player->add_args4(t_shop_other->id);
			huodong_player->add_args5(msg.num());
		}
		else
		{
			huodong_player->set_args5(buy_index, huodong_player->args5(buy_index) + msg.num());
		}
	}
	sHuodongPool->save_huodong_player(huodong_player->guid());

	ResMessage::res_shop_buy(player, rds, name, id);
}