#include "item_operation.h"
#include "item_config.h"
#include "item_def.h"
#include "treasure_list.h"

#define SHOP_REFRES_TIME 7200000

void ItemOperation::item_add_template(dhc::player_t *player, uint32_t item_id, int32_t item_amount, int mode)
{
	s_t_item *t_item = sItemConfig->get_item(item_id);
	if (!t_item)
	{
		return;
	}
	bool flag = false;
	for (int i = 0; i < player->item_ids_size(); ++i)
	{
		if (player->item_ids(i) == item_id)
		{
			player->set_item_amount(i, player->item_amount(i) + item_amount);
			flag = true;
			break;
		}
	}
	if (!flag)
	{
		player->add_item_ids(item_id);
		player->add_item_amount(item_amount);
	}

	if (t_item->type == 6001)
	{
		sTreasureList->set_rob_player_list(player, item_id);
	}

	LOG_OUTPUT(player, LOG_ITEM, item_id, item_amount, mode, LOG_ADD);
}

int ItemOperation::item_num_templete(dhc::player_t *player, uint32_t item_id)
{
	for (int i = 0; i < player->item_ids_size(); ++i)
	{
		if (player->item_ids(i) == item_id)
		{
			return player->item_amount(i);
		}
	}
	return 0;
}

void ItemOperation::item_destory_templete(dhc::player_t *player, uint32_t item_id, uint32_t item_amount, int mode)
{
	int pos = -1;
	for (int i = 0; i < player->item_ids_size(); ++i)
	{
		if (player->item_ids(i) == item_id)
		{
			if (player->item_amount(i) > item_amount)
			{
				player->set_item_amount(i, player->item_amount(i) - item_amount);
			}
			else
			{
				pos = i;
			}
			break;
		}
	}
	if (pos != -1)
	{
		for (int i = pos; i < player->item_ids_size() - 1; ++i)
		{
			player->set_item_ids(i, player->item_ids(i + 1));
			player->set_item_amount(i, player->item_amount(i + 1));
		}
		player->mutable_item_ids()->RemoveLast();
		player->mutable_item_amount()->RemoveLast();
	}

	s_t_item *t_item = sItemConfig->get_item(item_id);
	if (t_item && t_item->type == 6001)
	{
		sTreasureList->set_rob_player_list(player, item_id);
	}

	if (mode != -1)
	{
		LOG_OUTPUT(player, LOG_ITEM, item_id, item_amount, mode, LOG_DEC);
	}
	
}

void ItemOperation::refresh_role_shop(dhc::player_t *player)
{
	while (player->shop2_ids_size() < SHOP_NUM)
	{
		player->add_shop2_ids(0);
		player->add_shop2_sell(0);
	}
	std::vector<int> has_refreshs;
	for (int i = 0; i < SHOP_NUM; ++i)
	{
		s_t_shop *t_shop = 0;
		if (i == 0 ||
			i == 1)
		{
			t_shop = sItemConfig->get_random_role_shop(player, 1, has_refreshs);
		}
		else
		{
			t_shop = sItemConfig->get_random_role_shop(player, 2, has_refreshs);
		}
		if (t_shop)
		{
			has_refreshs.push_back(t_shop->value1);
			player->set_shop2_ids(i, t_shop->id);
			player->set_shop2_sell(i, 0);
		}
	}
}

void ItemOperation::refresh_huiyi_shop(dhc::player_t *player)
{
	player->clear_shop4_ids();
	player->clear_shop4_sell();

	const s_t_huiyi_shop* huiyi_shop = 0;
	std::set<int> has_refreshs;
	int geze = 0;
	for (int i = 0; i < SHOP_NUM; ++i)
	{
		geze = i + 1;
		if (geze > 4)
		{
			geze = 4;
		}
		huiyi_shop = sItemConfig->get_huiyi_shop_random(geze, has_refreshs);
		if (huiyi_shop)
		{
			player->add_shop4_ids(huiyi_shop->id);
		}
		else
		{
			player->add_shop4_ids(0);
		}
		player->add_shop4_sell(0);
	}
}

std::string ItemOperation::get_color(int color, const std::string &name)
{
	if (color == 0)
	{
		return "[ffffff]" + name ;
	}
	else if (color == 1)
	{
		return "[5cf732]" + name ;
	}
	else if (color == 2)
	{
		return "[32eef7]" + name ;
	}
	else if (color == 3)
	{
		return "[ff3fbf]" + name ;
	}
	else if (color == 4)
	{
		return "[ee9900]" + name ;
	}
	else if (color == 5)
	{
		return "[ff0000]" + name ;
	}
	return name;
}

void ItemOperation::item_do_role_shop(dhc::player_t *player)
{
	uint64_t now = game::timer()->now();
	if (player->shop2_refresh_num() >= 10)
	{
		player->set_shop_last_time(now);
		return;
	}

	uint64_t dtime = now - player->shop_last_time();
	if (dtime > SHOP_REFRES_TIME)
	{
		uint64_t num = dtime / SHOP_REFRES_TIME;
		if (num + player->shop2_refresh_num() > 10)
		{
			player->set_shop2_refresh_num(10);
			player->set_shop_last_time(now);
		}
		else
		{
			player->set_shop2_refresh_num(player->shop2_refresh_num() + num);
			player->set_shop_last_time(player->shop_last_time() + num * SHOP_REFRES_TIME);
		}
	}
}

void ItemOperation::refresh_guild_shop(dhc::guild_t* guild)
{
	guild->clear_shop_ids();
	guild->clear_shop_nums();

	int id = 0;
	std::set<int> ids;
	for (int i = 0; i < SHOP_NUM; ++i)
	{
		if (i == 0 || i == 1)
		{
			id = sItemConfig->get_guild_shop_xs(1, guild->level(), ids);
		}
		else
		{
			id = sItemConfig->get_guild_shop_xs(2, guild->level(), ids);
		}
		ids.insert(id);
		guild->add_shop_ids(id);
		guild->add_shop_nums(0);
	}
}

void ItemOperation::refresh_guild_shop(dhc::player_t* player)
{
	player->clear_shop1_ids();
	player->clear_shop1_sell();
}

int ItemOperation::get_guild_shop_buy_num(dhc::player_t* player, int id)
{
	for (int i = 0; i < player->shop3_ids_size(); ++i)
	{
		if (player->shop3_ids(i) == id)
		{
			return player->shop3_sell(i);
		}
	}
	return 0;
}

void ItemOperation::add_guild_shop_buy_num(dhc::player_t* player, int id, int num)
{
	bool has = false;
	for (int i = 0; i < player->shop3_ids_size(); ++i)
	{
		if (player->shop3_ids(i) == id)
		{
			player->set_shop3_sell(i, player->shop3_sell(i) + num);
			has = true;
			break;
		}
	}
	if (!has)
	{
		player->add_shop3_ids(id);
		player->add_shop3_sell(num);
	}
}

int ItemOperation::get_huiyi_luckshop_buy_num(dhc::player_t* player, int id)
{
	for (int i = 0; i < player->huiyi_shop_ids_size(); ++i)
	{
		if (player->huiyi_shop_ids(i) == id)
		{
			return player->huiyi_shop_nums(i);
		}
	}
	return 0;
}

void ItemOperation::add_huiyi_luckshop_buy_num(dhc::player_t* player, int id, int num)
{
	bool has = false;
	for (int i = 0; i < player->huiyi_shop_ids_size(); ++i)
	{
		if (player->huiyi_shop_ids(i) == id)
		{
			player->set_huiyi_shop_nums(i, player->huiyi_shop_nums(i) + num);
			has = true;
			break;
		}
	}
	if (!has)
	{
		player->add_huiyi_shop_ids(id);
		player->add_huiyi_shop_nums(num);
	}
}

void ItemOperation::do_pet_shop(dhc::player_t *player)
{
	uint64_t now = game::timer()->now();
	if (player->shoppet_refresh_num() >= 10)
	{
		player->set_shoppet_last_time(now);
		return;
	}

	uint64_t dtime = now - player->shoppet_last_time();
	if (dtime > SHOP_REFRES_TIME)
	{
		uint64_t num = dtime / SHOP_REFRES_TIME;
		if (num + player->shoppet_refresh_num() > 10)
		{
			player->set_shoppet_refresh_num(10);
			player->set_shoppet_last_time(now);
		}
		else
		{
			player->set_shoppet_refresh_num(player->shoppet_refresh_num() + num);
			player->set_shoppet_last_time(player->shoppet_last_time() + num * SHOP_REFRES_TIME);
		}
	}
}

void ItemOperation::refresh_pet_shop(dhc::player_t *player)
{
	while (player->shoppet_ids_size() < SHOP_NUM)
	{
		player->add_shoppet_ids(0);
		player->add_shoppet_sell(0);
	}
	std::set<int> has_refreshs;
	const s_t_chongwu_shop *t_shop = 0;
	for (int i = 0; i < SHOP_NUM; ++i)
	{
		t_shop = sItemConfig->get_chongwu_shop_random(i + 1, player->level(), has_refreshs);
		if (t_shop)
		{
			has_refreshs.insert(t_shop->value1);
			player->set_shoppet_ids(i, t_shop->id);
			player->set_shoppet_sell(i, 0);
		}
	}
}
