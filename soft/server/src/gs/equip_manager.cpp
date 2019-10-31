#include "equip_manager.h"
#include "equip_config.h"
#include "gs_message.h"
#include "equip_operation.h"
#include "item_operation.h"
#include "player_operation.h"
#include "social_operation.h"
#include "player_config.h"
#include "utils.h"
#include "item_config.h"
#include "item_def.h"

EquipManager::EquipManager()
{

}

EquipManager::~EquipManager()
{

}

int EquipManager::init()
{
	if (-1 == sEquipConfig->parse())
	{
		return -1;
	}
	return 0;
}

int EquipManager::fini()
{
	return 0;
}

void EquipManager::terminal_equip_auto_enhance(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_auto_enhance msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	PCK_CHECK_NO_GOLD;


	if (msg.equip_guid_size() != msg.enhance_num_size())
	{
		GLOBAL_ERROR;
		return;
	}

	for (int c = 0; c < msg.equip_guid_size(); c++)
	{
		dhc::equip_t *equip = POOL_GET_EQUIP(msg.equip_guid(c));
		if (!equip)
		{
			GLOBAL_ERROR;
			return;
		}
		s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
		if (!t_equip)
		{
			GLOBAL_ERROR;
			return;
		}

		if (equip->enhance() + msg.enhance_num(c) > (player->level() + 10) / 10 * 10)
		{
			PERROR(ERROR_EQUIP_ENHANCEUP);
			return;
		}

		int gold = 0;
		for (int i = equip->enhance() + 1; i <= equip->enhance() + msg.enhance_num(c); ++i)
		{
			int g = sEquipConfig->get_enhance(t_equip->font_color, i);
			gold += g;
		}
		if (player->gold() < gold)
		{
			PERROR(ERROR_GOLD);
			return;
		}

		PlayerOperation::player_dec_resource(player, resource::GOLD, gold, LOGWAY_EQUIP_ENHANCE);
		equip->set_enhance(equip->enhance() + msg.enhance_num(c));
		player->set_qh_task_num(player->qh_task_num() + msg.enhance_num(c));
		PlayerOperation::player_add_active(player, 600, msg.enhance_num(c));
	}


	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_equip_enhance(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_enhance msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	PCK_CHECK_NO_GOLD;

	dhc::equip_t *equip = POOL_GET_EQUIP(msg.equip_guid());
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!t_equip)
	{
		GLOBAL_ERROR;
		return;
	}

	if (equip->enhance() + msg.enhance_num() > (player->level() + 10) / 10 * 10)
	{
		PERROR(ERROR_EQUIP_ENHANCEUP);
		return;
	}

	int gold = 0;
	for (int i = equip->enhance() + 1; i <= equip->enhance() + msg.enhance_num(); ++i)
	{
		int g = sEquipConfig->get_enhance(t_equip->font_color, i);
		gold += g;
	}
	if (player->gold() < gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::GOLD, gold, LOGWAY_EQUIP_ENHANCE);
	equip->set_enhance(equip->enhance() + msg.enhance_num());
	player->set_qh_task_num(player->qh_task_num() + msg.enhance_num());
	PlayerOperation::player_add_active(player, 600, msg.enhance_num());

	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_equip_sell(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_sell msg;
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
	std::set<uint64_t> eguids;
	int hejin = 0;
	for (int i = 0; i < msg.equip_guids_size(); ++i)
	{
		dhc::equip_t *equip = POOL_GET_EQUIP(msg.equip_guids(i));
		if (!equip)
		{
			GLOBAL_ERROR;
			return;
		}

		s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
		if (!t_equip)
		{
			GLOBAL_ERROR;
			return;
		}
		if (equip->locked())
		{
			PERROR(ERROR_EQUIP_LOCK);
			return;
		}
		if (equip->role_guid() > 0)
		{
			GLOBAL_ERROR;
			return;
		}

		const s_t_equip_suipian* t_suipian = sEquipConfig->get_equip_suipian(equip->template_id());
		if (!t_suipian)
		{
			GLOBAL_ERROR;
			return;
		}

		if (eguids.find(equip->guid()) != eguids.end())
		{
			GLOBAL_ERROR;
			return;
		}
		eguids.insert(equip->guid());

		/// 强化
		if (!EquipOperation::get_enhance_return(player, equip, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 精炼
		if (!EquipOperation::get_jinlian_return(player, equip, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 改造
		if (!EquipOperation::get_gaizao_return(player, equip, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 升星
		if (!EquipOperation::get_shengxin_return(player, equip, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 红色装备之力
		if (t_equip->sell_item_num > 0)
		{
			rds.add_reward(2, 50120001, t_equip->sell_item_num);
		}

		/// 合金
		hejin += t_equip->sell;
	}
	rds.add_reward(1, resource::HEJIN, hejin);
	rds.merge();

	for (int i = 0; i < msg.equip_guids_size(); ++i)
	{
		EquipOperation::equip_delete(player, msg.equip_guids(i), LOGWAY_EQUIP_SELL);
	}
	PlayerOperation::player_add_reward(player, rds, LOGWAY_EQUIP_SELL);

	protocol::game::smsg_equip_init msg1;
	ADD_MSG_REWARD(msg1, rds);
	ResMessage::res_equip_init(player, msg1, name, id);
}

void EquipManager::terminal_equip_gaizao(const std::string &data, const std::string &name, int id)
{
	equip_gaizao(data, name, id, 0);
}

void EquipManager::terminal_equip_gaizao_ten(const std::string &data, const std::string &name, int id)
{
	equip_gaizao(data, name, id, 1);
}

void EquipManager::terminal_equip_gaizao_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_gaizao_buy msg;
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

	if (msg.count() <= 0 || msg.count() >= 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < msg.count() * 10)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, msg.count() * 10, LOGWAY_GAIZAOSHI_BUY);
	ItemOperation::item_add_template(player, 50010001, msg.count(), LOGWAY_GAIZAOSHI_BUY);

	ResMessage::res_success(player, true, name, id);
}

void EquipManager::equip_gaizao(const std::string &data, const std::string &name, int id, int type)
{
	protocol::game::cmsg_equip_gaizao msg;
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

	dhc::equip_t *equip = POOL_GET_EQUIP(msg.equip_guid());
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!t_equip)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_equip_sx *t_equip_sx = sEquipConfig->get_equip_sx(t_equip->font_color, equip->star());
	if (!t_equip_sx)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_gaizao *t_gaizao = sEquipConfig->get_gaizao(t_equip->font_color);
	if (!t_gaizao)
	{
		GLOBAL_ERROR;
		return;
	}

	/// 锁定属性
	int max_num = 4;
	if (max_num > equip->rand_ids_size())
	{
		max_num = equip->rand_ids_size();
	}
	if (msg.suos_size() > max_num)
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.suos_size() >= t_gaizao->gaizaoshi.size())
	{
		GLOBAL_ERROR;
		return;
	}
	for (int i = 0; i < msg.suos_size(); ++i)
	{
		if (msg.suos(i) >= equip->rand_ids_size())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	int gaizaoshi = t_gaizao->gaizaoshi[msg.suos_size()];
	if (msg.suos_size() > 0)
	{
		gaizaoshi += t_gaizao->gaizaoshi[0];
	}
	int gaizao_for_num = 1;
	if (type == 1)
	{
		gaizao_for_num = 100;
	}
	if (ItemOperation::item_num_templete(player, t_gaizao->item_id) < gaizaoshi)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	
	bool should_stop = false;
	int gaizao_index = 0;
	int total_gaizao_shi = ItemOperation::item_num_templete(player, t_gaizao->item_id);
	int xiaohao_gaizao_shi = 0;
	int gaizao_count = 0;
	for (gaizao_index = 0; gaizao_index < gaizao_for_num; ++gaizao_index)
	{
		if (total_gaizao_shi < gaizaoshi)
		{
			break;
		}

		gaizao_count++;
		total_gaizao_shi -= gaizaoshi;
		xiaohao_gaizao_shi += gaizaoshi;
		equip->set_gaizao_counts(equip->gaizao_counts() + gaizaoshi);

		/// 新增属性
		if (equip->rand_ids_size() < t_gaizao->max_num)
		{
			int rate = t_gaizao->gaizao_rate[equip->rand_ids_size()];
			int r = Utils::get_int32(0, 99);
			if (rate > r)
			{
				equip->add_rand_ids(0);
				equip->add_rand_values(0);

				should_stop = true;
			}
		}

		std::set<int> attrs;
		int num = 0;
		for (int i = 0; i < equip->rand_ids_size(); ++i)
		{
			bool flag = false;
			for (int j = 0; j < msg.suos_size(); ++j)
			{
				if (i == msg.suos(j))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				attrs.insert(equip->rand_ids(i));
				continue;
			}
			equip->set_rand_ids(i, 0);
			equip->set_rand_values(i, 0);
			num++;
		}

		std::vector<int> vec;
		for (int i = 0; i < t_equip->eeattr.size(); ++i)
		{
			if (attrs.find(t_equip->eeattr[i].attr) != attrs.end())
			{
				continue;
			}
			vec.push_back(i);
		}
		std::vector<int> nvec;
		Utils::get_vector(vec, num, nvec);
		int j = 0;
		for (int i = 0; i < equip->rand_ids_size(); ++i)
		{
			if (equip->rand_ids(i) > 0)
			{
				continue;
			}
			s_t_equip_random_attr &ra = t_equip->eeattr[nvec[j]];
			equip->set_rand_ids(i, ra.attr);
			double min_v = ra.value1;
			double max_v = ra.value2 + ra.value2 * t_equip_sx->enhance_rate * equip->enhance();
			double max_v_b = max_v;
			int rate = Utils::get_int32(0, 99);
			if (rate <= 61)
			{
				max_v *= 0.4f;
			}
			else if (rate <= 88)
			{
				max_v *= 0.6f;
			}
			else if (rate <= 97)
			{
				max_v *= 0.8f;
			}
			int v = Utils::get_int32(min_v, max_v);
			equip->set_rand_values(i, v);
			j++;

			if (v > max_v_b * 0.7 && v <= max_v_b * 0.9)
			{
				should_stop = true;
			}
		}

		if (should_stop)
		{
			break;
		}
	}

	ItemOperation::item_destory_templete(player, t_gaizao->item_id, xiaohao_gaizao_shi, LOGWAY_EQUIP_GAIZAO);
	ResMessage::res_equip_gaizao(player, equip, gaizao_count, name, id);
}

void EquipManager::terminal_equip_lock(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_lock msg;
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

	dhc::equip_t *equip = POOL_GET_EQUIP(msg.equip_guid());
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}

	equip->set_locked(1 - equip->locked());

	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_equip_kc(const std::string &data, const std::string &name, int id)
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

	if (player->equip_kc_num() >= 5)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_price *t_price = sPlayerConfig->get_price(player->equip_kc_num() + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < t_price->kc)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_price->kc, LOGWAY_EQUIP_KC);
	player->set_equip_kc_num(player->equip_kc_num() + 1);

	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_equip_suipian(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_suipian msg;
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

	s_t_item *t_item = sItemConfig->get_item(msg.item_id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_item->type != IT_EQUIP_SUIPIAN)
	{
		GLOBAL_ERROR;
		return;
	}

	int num = ItemOperation::item_num_templete(player, t_item->id);
	if (num < t_item->def2)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}
	s_t_equip *t_equip = sEquipConfig->get_equip(t_item->def1);
	if (!t_equip)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	dhc::equip_t *equip = EquipOperation::equip_create(player, t_item->def1, 0, 0, LOGWAY_EQUIP_SUIPIAN);
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}
	ItemOperation::item_destory_templete(player, t_item->id, t_item->def2, LOGWAY_EQUIP_SUIPIAN);

	ResMessage::res_equip_suipian(player, equip, name, id);
}

void EquipManager::terminal_dress_on(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_dress_on msg;
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

	s_t_dress *t_dress = sEquipConfig->get_dress(msg.dress_id());
	if (!t_dress)
	{
		GLOBAL_ERROR;
		return;
	}
	if (!EquipOperation::player_has_dress(player, t_dress->id))
	{
		GLOBAL_ERROR;
		return;
	}
	bool flag = false;
	for (int i = 0; i < player->dress_on_ids_size(); ++i)
	{
		int dress_id = player->dress_on_ids(i);
		s_t_dress *t_p_dress = sEquipConfig->get_dress(dress_id);
		if (!t_p_dress)
		{
			continue;
		}
		if (t_p_dress->type == t_dress->type)
		{
			flag = true;
			player->set_dress_on_ids(i, t_dress->id);
			break;
		}
		if (t_p_dress->id == t_dress->id)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	if (!flag)
	{
		player->add_dress_on_ids(t_dress->id);
	}

	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_dress_off(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_dress_off msg;
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

	s_t_dress *t_dress = sEquipConfig->get_dress(msg.dress_id());
	if (!t_dress)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_dress->type < 2)
	{
		GLOBAL_ERROR;
		return;
	}
	if (!EquipOperation::player_has_dress(player, t_dress->id))
	{
		GLOBAL_ERROR;
		return;
	}
	int index = -1;
	for (int i = 0; i < player->dress_on_ids_size(); ++i)
	{
		int dress_id = player->dress_on_ids(i);
		if (t_dress->id == dress_id)
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}
	player->mutable_dress_on_ids()->SwapElements(index, player->dress_on_ids_size() - 1);
	player->mutable_dress_on_ids()->RemoveLast();

	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_dress_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_dress_buy msg;
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

	s_t_dress *t_dress = sEquipConfig->get_dress(msg.dress_id());
	if (!t_dress)
	{
		GLOBAL_ERROR;
		return;
	}
	if (EquipOperation::player_has_dress(player, t_dress->id))
	{
		GLOBAL_ERROR;
		return;
	}
	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_dress_unlock(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_dress_unlock msg;
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

	const s_t_dress* t_dress = sEquipConfig->get_dress(msg.dress_id());
	if (!t_dress)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_dress_unlock* t_dress_unlock = sEquipConfig->get_dress_unlock(msg.dress_id());
	if (!t_dress_unlock)
	{
		GLOBAL_ERROR;
		return;
	}

	if (EquipOperation::player_has_dress(player, msg.dress_id()))
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_dress_unlock->last_id != 0)
	{
		if (!EquipOperation::player_has_dress(player, t_dress_unlock->last_id))
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->dress_tuzhi() < t_dress_unlock->num)
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::DRESS_TUZHI, t_dress_unlock->num, LOGWAY_DRESS_UNLOCK);
	player->add_dress_ids(msg.dress_id());

	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_dress_unlock_achieve(const std::string& data, const std::string& name, int id)
{
	protocol::game::cmsg_dress_unlock_achieve msg;
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

	const s_t_dress_target* t_dress_target = sEquipConfig->get_dress_target(msg.achieve_id());
	if (!t_dress_target)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_dress_target->type != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_dress_target->defs.size() < 2)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->dress_achieves_size(); ++i)
	{
		if (player->dress_achieves(i) == msg.achieve_id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->dress_tuzhi() < t_dress_target->defs[0])
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->dress_ids_size() < t_dress_target->defs[1])
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::DRESS_TUZHI, t_dress_target->defs[0], LOGWAY_DRESS_UNLOCK_ACHIEVE);
	player->add_dress_achieves(msg.achieve_id());
	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_equip_star(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_star msg;
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

	dhc::equip_t *equip = POOL_GET_EQUIP(msg.star_guid());
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!t_equip)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_equip->font_color < 4)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_equip_suipian* t_suipian = sEquipConfig->get_equip_suipian(equip->template_id());
	if (!t_suipian)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_equip_sx *t_equip_sx = sEquipConfig->get_equip_sx(t_equip->font_color, equip->star() + 1);
	if (!t_equip_sx)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, t_suipian->suipian_id) < t_equip_sx->suipian)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	if (player->gold() < t_equip_sx->gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	ItemOperation::item_destory_templete(player, t_suipian->suipian_id, t_equip_sx->suipian, LOGWAY_EQUIP_SX);
	PlayerOperation::player_dec_resource(player, resource::GOLD, t_equip_sx->gold, LOGWAY_EQUIP_SX);
	equip->set_star(equip->star() + 1);
	ResMessage::res_success(player, true, name, id);
}


void EquipManager::terminal_equip_jl(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_jinlian msg;
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

	if (msg.level() < 1)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::equip_t *equip = POOL_GET_EQUIP(msg.equip_guid());
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}

	int gold = 0;
	int item_num = 0;
	for (int jlevel = 1; jlevel <= msg.level(); jlevel++)
	{
		const s_t_equip_jl *t_jinlian = sEquipConfig->get_equip_jl(equip->jilian() + jlevel);
		if (!t_jinlian)
		{
			GLOBAL_ERROR;
			return;
		}

		int color = t_equip->font_color - 1;
		if (color < 0 || color >= t_jinlian->consume.size())
		{
			GLOBAL_ERROR;
			return;
		}

		gold += t_jinlian->consume[color].second;

		if (player->gold() < gold)
		{
			PERROR(ERROR_GOLD);
			return;
		}

		item_num += t_jinlian->consume[color].first;
		if (ItemOperation::item_num_templete(player, 50070001) < item_num)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}

	if (gold <= 0 || item_num <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::GOLD, gold, LOGWAY_EQUIP_JINLIAN);
	ItemOperation::item_destory_templete(player, 50070001, item_num, LOGWAY_EQUIP_JINLIAN);
	equip->set_jilian(equip->jilian() + msg.level());

	ResMessage::res_success(player, true, name, id);
}

void EquipManager::terminal_equip_init(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_init msg;
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

	dhc::equip_t *equip = POOL_GET_EQUIP(msg.equip_guids());
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_equip_suipian* t_suipian = sEquipConfig->get_equip_suipian(equip->template_id());
	if (!t_suipian)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!t_equip)
	{
		GLOBAL_ERROR;
		return;
	}

	if (equip->locked())
	{
		GLOBAL_ERROR;
		return;
	}

	if (equip->role_guid() != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < 50)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	s_t_rewards rds;
	/// 强化
	if (!EquipOperation::get_enhance_return(player, equip, rds))
	{
		GLOBAL_ERROR;
		return;
	}
	/// 精炼
	if (!EquipOperation::get_jinlian_return(player, equip, rds))
	{
		GLOBAL_ERROR;
		return;
	}
	/// 改造
	if (!EquipOperation::get_gaizao_return(player, equip, rds))
	{
		GLOBAL_ERROR;
		return;
	}
	/// 升星
	if (!EquipOperation::get_shengxin_return(player, equip, rds))
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_suipian->suipian_specail)
	{
		equip->set_enhance(0);
		equip->set_jilian(0);
		equip->clear_rand_ids();
		equip->clear_rand_values();
		equip->set_star(0);
		equip->set_gaizao_counts(0);
	}
	else
	{
		rds.add_reward(2, t_suipian->suipian_id, t_suipian->suipian_num);
		EquipOperation::equip_delete(player, msg.equip_guids(), LOGWAY_EQUIP_INIT);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_EQUIP_INIT);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, 50, LOGWAY_EQUIP_INIT);

	protocol::game::smsg_equip_init msg1;
	ADD_MSG_REWARD(msg1, rds);

	ResMessage::res_equip_init(player, msg1, name, id);
}

void EquipManager::terminal_equip_rongliang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_equip_ronglian msg;
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

	dhc::equip_t* equip = POOL_GET_EQUIP(msg.equip_guid());
	if (!equip)
	{
		GLOBAL_ERROR;
		return;
	}
	if (equip->enhance() > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (equip->jilian() > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (equip->role_guid() != 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (equip->locked())
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_equip* t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!t_equip)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_equip->font_color != 4)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_item* t_item = sItemConfig->get_item(msg.suipian_id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, 50120001) < 40)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	ItemOperation::item_destory_templete(player, 50120001, 40, LOGWAY_EQUIP_RONGLIAN);
	ItemOperation::item_add_template(player, msg.suipian_id(), 20, LOGWAY_EQUIP_RONGLIAN);
	EquipOperation::equip_delete(player, msg.equip_guid(), LOGWAY_EQUIP_RONGLIAN);

	ResMessage::res_success(player, true, name, id);
}
