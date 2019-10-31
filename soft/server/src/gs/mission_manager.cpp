#include "mission_manager.h"
#include "mission_config.h"
#include "player_operation.h"
#include "utils.h"
#include "item_operation.h"
#include "gs_message.h"
#include "role_operation.h"
#include "equip_operation.h"
#include "mission_pool.h"
#include "item_config.h"
#include "player_config.h"
#include "role_config.h"
#include "mission_operation.h"
#include "social_operation.h"
#include "huodong_pool.h"
#include "player_config.h"
#include "mission_fight.h"

#define MISSION_PERIOD 1000
#define YB_ZH 200
#define YB_NUM 3
#define YBQ_NUM 5
#define YBQ_TIME 600000
#define ORE_NUM 3
#define ORE_TIME 600000

MissionManager::MissionManager()
: timer_(0)
{

}

MissionManager::~MissionManager()
{

}

int MissionManager::init()
{
	if (-1 == sMissionConfig->parse())
	{
		return -1;
	}
	sMissionPool->init();
	timer_ = game::timer()->schedule(boost::bind(&MissionManager::update, this, _1), MISSION_PERIOD, "mission");
	return 0;
}

int MissionManager::fini()
{
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = 0;
	}
	return 0;
}

int MissionManager::update(ACE_Time_Value tv)
{
	sMissionPool->update();
	return 0;
}

void MissionManager::terminal_mission_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_mission_fight_end msg;
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

	if (!PlayerOperation::check_fight_time(player))
	{
		PERROR(ERROR_FIGHT_TIME);
		return;
	}	
	PlayerOperation::player_do_tili(player);

	s_t_mission *t_mission = sMissionConfig->get_mission(msg.mission_id());
	if (!t_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	if (EquipOperation::is_equip_full(player))
	{
		PERROR(ERROR_EQUIP_FULL);
		return;
	}
	if (t_mission->tili > player->tili())
	{
		PERROR(ERROR_TILI);
		return;
	}
	if (t_mission->jslc > player->mission())
	{
		PERROR(ERROR_MISSION_NOPEN);
		return;
	}
	if (t_mission->jyjslc > player->mission_jy())
	{
		PERROR(ERROR_MISSION_NOPEN);
		return;
	}

	if (t_mission->cishu)
	{
		int cishu = MissionOperation::get_mission_cishu(player, t_mission->id);
		if (cishu >= t_mission->cishu)
		{
			PERROR(ERROR_MISSION_CISHU);
			return;
		}
	}

	std::string text;
	int star;
	s_t_rewards rds;
	bool has_jieri = sHuodongPool->has_jieri_huodong(player);
	int result = MissionFight::mission_gq(player, t_mission->id, star, MissionOperation::mission_can_jump(player, t_mission->id), text);
	if (result == 1)
	{
		PlayerOperation::player_dec_resource(player, resource::TILI, t_mission->tili, LOGWAY_MISSION_END);

		rds.add_reward(1, resource::EXP, t_mission->tili * 5);
		rds.add_reward(1, resource::GOLD, t_mission->tili * player->level() * 10 + 50);
		rds.add_reward(1, resource::YUANLI, t_mission->tili * 250);

		for (int i = 0; i < t_mission->tmi.size(); ++i)
		{
			if (Utils::get_int32(0, 99) < t_mission->tmi[i].rate)
			{
				if (!has_jieri && (t_mission->tmi[i].reward.value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
					t_mission->tmi[i].reward.value1 == s_t_rewards::HUODONG_ITEM_ID2))
				{
					continue;
				}
				rds.add_reward(t_mission->tmi[i].reward);
			}
		}
		if (t_mission->id == 2006 && t_mission->lch > player->mission())
		{
			rds.add_reward(3, 203, 1);
		}
		
		if (t_mission->lch > player->mission())
		{
			if ((t_mission->jslc / 1000) != (t_mission->lch / 1000))
			{
				rds.add_reward(1, resource::DRESS_TUZHI, 1);
			}
			player->set_mission(t_mission->lch);
			if (t_mission->lch % 1000 == 0)
			{
				/*std::string text;
				game::scheme()->get_server_str(text, "mission", player->name().c_str(), t_mission->name.c_str());*/
				//SocialOperation::gundong(text);
				SocialOperation::gundong_server("t_server_language_text_mission", player->name(), t_mission->name,"");
			}
		}
		if (t_mission->jylch > player->mission_jy())
		{
			player->set_mission_jy(t_mission->jylch);
			if (t_mission->jylch % 1000 == 0)
			{
				/*	std::string text;
					game::scheme()->get_server_str(text, "mission", player->name().c_str(), t_mission->name.c_str());
					SocialOperation::gundong(text);*/
				SocialOperation::gundong_server("t_server_language_text_mission", player->name(), t_mission->name, "");
			}
		}

		// 副本次数
		if (t_mission->cishu)
		{
			MissionOperation::add_mission_cishu(player, t_mission->id, 1);
		}
		// 副本星星
		int star_add = MissionOperation::add_mission_star(player, t_mission->id, star);
		// 地图星星
		if (star_add && t_mission->type != 3)
		{
			MissionOperation::add_map_star(player, t_mission->map, star_add);
		}

		if (t_mission->type == 1)
		{
			player->set_pt_task_num(player->pt_task_num() + 1);
			PlayerOperation::player_add_active(player, 400, 1);
			sHuodongPool->huodong_active(player, HUODONG_COND_NORMAL_COUNT, 1);
			RoleOperation::check_bskill_count(player, BSKILL_TYPE1, 1);
		}
		else if (t_mission->type == 2 || t_mission->type == 3)
		{
			player->set_jy_task_num(player->jy_task_num() + 1);
			PlayerOperation::player_add_active(player, 500, 1);
			sHuodongPool->huodong_active(player, HUODONG_COND_JY_COUNT, 1);
			RoleOperation::check_bskill_count(player, BSKILL_TYPE2, 1);
		}
		PlayerOperation::player_add_reward(player, rds, LOGWAY_MISSION_END);

		
	}

	ResMessage::res_mission_fight_end(player, result, text, star, rds, name, id);
}

void MissionManager::terminal_mission_reward(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_mission_reward msg;
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

	int mapid = msg.map_id();
	s_t_map *t_map = sMissionConfig->get_map(mapid);
	if (!t_map)
	{
		GLOBAL_ERROR;
		return;
	}

	int reward_id = msg.reward_id();
	if (reward_id < 0 || reward_id > 2)
	{
		GLOBAL_ERROR;
		return;
	}
	bool flag = false;
	s_t_rewards rds;
	for (int i = 0; i < player->map_ids_size(); ++i)
	{
		if (player->map_ids(i) == mapid)
		{
			flag = true;

			if (player->map_reward_get(i) & (1 << reward_id))
			{
				GLOBAL_ERROR;
				return;
			}
			if (reward_id >= t_map->star_rewards.size())
			{
				GLOBAL_ERROR;
				return;
			}
			if (player->map_star(i) < t_map->star_rewards[reward_id].star_num)
			{
				PERROR(ERROR_MISSION_REWARD_STAR);
				return;
			}
			player->set_map_reward_get(i, player->map_reward_get(i) | (1 << reward_id));
			rds.add_reward(t_map->star_rewards[reward_id].rewards);
			PlayerOperation::player_add_reward(player, rds, LOGWAY_MISSION_REWARD);
			break;
		}
	}
	if (!flag)
	{
		GLOBAL_ERROR;
		return;
	}
	ResMessage::res_success(player, true, name, id);
}

void MissionManager::terminal_mission_saodang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_mission_saodang msg;
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
	
	PlayerOperation::player_do_tili(player);
	int num = msg.num();
	if (num <= 0 || num > 100)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_mission *t_mission = sMissionConfig->get_mission(msg.mission_id());
	if (!t_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	if (EquipOperation::is_equip_full(player))
	{
		PERROR(ERROR_EQUIP_FULL);
		return;
	}

	if (t_mission->tili * num > player->tili())
	{
		PERROR(ERROR_TILI);
		return;
	}
	if (t_mission->jslc > player->mission())
	{
		PERROR(ERROR_MISSION_NOPEN);
		return;
	}
	if (t_mission->jyjslc > player->mission_jy())
	{
		PERROR(ERROR_MISSION_NOPEN);
		return;
	}

	{
		bool flag = false;
		for (int i = 0; i < player->mission_ids_size(); ++i)
		{
			if (player->mission_ids(i) == t_mission->id)
			{
				flag = true;
				if (player->mission_star(i) < 3)
				{
					PERROR(ERROR_MISSION_SAODANG_STAR);
					return;
				}
				break;
			}
		}
		if (!flag)
		{
			PERROR(ERROR_MISSION_SAODANG_STAR);
			return;
		}
	}

	if (t_mission->cishu)
	{
		int cishu = MissionOperation::get_mission_cishu(player, t_mission->id);
		if (cishu + num > t_mission->cishu)
		{
			PERROR(ERROR_MISSION_CISHU);
			return;
		}
		if (num > t_mission->cishu)
		{
			PERROR(ERROR_MISSION_CISHU);
			return;
		}
	}

	PlayerOperation::player_dec_resource(player, resource::TILI, t_mission->tili * num, LOGWAY_MISSION_SAODANG);

	bool has_jieri = sHuodongPool->has_jieri_huodong(player);
	std::vector<s_t_rewards> vrds;
	for (int k = 0; k < num; ++k)
	{
		s_t_rewards rds;
		rds.add_reward(1, resource::EXP, t_mission->tili * 5);
		rds.add_reward(1, resource::GOLD, (t_mission->tili * player->level() * 10 + 50));
		rds.add_reward(1, resource::YUANLI, t_mission->tili * 250);
		for (int i = 0; i < t_mission->tmi.size(); ++i)
		{
			if (Utils::get_int32(0, 99) < t_mission->tmi[i].rate)
			{
				if (!has_jieri && (t_mission->tmi[i].reward.value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
					t_mission->tmi[i].reward.value1 == s_t_rewards::HUODONG_ITEM_ID2))
				{
					continue;
				}
				rds.add_reward(t_mission->tmi[i].reward);
			}
		}
		PlayerOperation::player_add_reward(player, rds, LOGWAY_MISSION_SAODANG);
		vrds.push_back(rds);
	}

	// 副本次数
	if (t_mission->cishu)
	{
		MissionOperation::add_mission_cishu(player, t_mission->id, msg.num());
	}

	if (t_mission->type == 1)
	{
		player->set_pt_task_num(player->pt_task_num() + msg.num());
		PlayerOperation::player_add_active(player, 400, msg.num());
		sHuodongPool->huodong_active(player, HUODONG_COND_NORMAL_COUNT, msg.num());
		RoleOperation::check_bskill_count(player, BSKILL_TYPE1, msg.num());
	}
	else if (t_mission->type == 2)
	{
		player->set_jy_task_num(player->jy_task_num() + msg.num());
		PlayerOperation::player_add_active(player, 500, msg.num());
		sHuodongPool->huodong_active(player, HUODONG_COND_JY_COUNT, msg.num());
		RoleOperation::check_bskill_count(player, BSKILL_TYPE2, msg.num());
	}
	else if (t_mission->type == 4)
	{
		PlayerOperation::player_add_active(player, 1400, 1);
	}
	else if (t_mission->type == 5)
	{
		PlayerOperation::player_add_active(player, 1500, 1);
	}

	

	ResMessage::res_mission_saodang(player, vrds, name, id);
}

void MissionManager::terminal_mission_first(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_mission_first msg;
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

	bool has_tongguan = false;
	for (int i = 0; i < player->mission_ids_size(); ++i)
	{
		if (player->mission_ids(i) == msg.mission_id())
		{
			has_tongguan = true;
			break;
		}
	}
	if (!has_tongguan)
	{
		GLOBAL_ERROR;
		return;
	}

	bool has_reward = false;
	for (int i = 0; i < player->mission_rewards_size(); ++i)
	{
		if (player->mission_rewards(i) == msg.mission_id())
		{
			has_reward = true;
			break;
		}
	}
	if (has_reward)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_mission_first* t_mission_first = sMissionConfig->get_mission_first(msg.mission_id());
	if (!t_mission_first)
	{
		GLOBAL_ERROR;
		return;
	}

	player->add_mission_rewards(msg.mission_id());

	
	s_t_rewards rds;
	rds.add_reward(t_mission_first->rewards);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_MISSION_FIRST);

	protocol::game::smsg_mission_first smsg;
	ADD_MSG_REWARD(smsg, rds);
	ResMessage::res_mission_first(player, smsg, name, id);
}

void MissionManager::terminal_mission_goumai(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_mission_goumai msg;
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

	s_t_mission *t_mission = sMissionConfig->get_mission(msg.mission_id());
	if (!t_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	int num = 0;
	for (int i = 0; i < player->mission_goumai_ids_size(); ++i)
	{
		if (player->mission_goumai_ids(i) == t_mission->id)
		{
			num = player->mission_goumai(i);
			break;
		}
	}
	s_t_vip *t_vip = sPlayerConfig->get_vip(player->vip());
	if (!t_vip)
	{
		GLOBAL_ERROR;
		return;
	}
	if (num >= t_vip->jy_buy_num)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_price *t_price = sPlayerConfig->get_price(num + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < t_price->jy)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_price->jy, LOGWAY_MISSION_BUY);
	bool flag = false;
	for (int i = 0; i < player->mission_goumai_ids_size(); ++i)
	{
		if (player->mission_goumai_ids(i) == t_mission->id)
		{
			flag = true;
			player->set_mission_goumai(i, player->mission_goumai(i) + 1);
			break;
		}
	}
	if (!flag)
	{
		player->add_mission_goumai_ids(t_mission->id);
		player->add_mission_goumai(1);
	}

	for (int i = 0; i < player->mission_cishu_ids_size(); ++i)
	{
		if (player->mission_cishu_ids(i) == t_mission->id)
		{
			player->set_mission_cishu(i, 0);
			break;
		}
	}
	sHuodongPool->huodong_active(player, HUODONG_COND_FUBEN_COUNT, 1);
	ResMessage::res_success(player, true, name, id);
}

void MissionManager::terminal_hbb_look(const std::string &data, const std::string &name, int id)
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

	ResMessage::res_hbb_look(player, name, id);
}

void MissionManager::terminal_hbb_refresh(const std::string &data, const std::string &name, int id)
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

	int num = player->hbb_refresh_num();
	s_t_price *t_price = sPlayerConfig->get_price(num + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < t_price->hbb_refresh)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_price->hbb_refresh, LOGWAY_HBB_REFRESH);
	player->set_hbb_refresh_num(player->hbb_refresh_num() + 1);
	player->set_bweh_task_num(player->bweh_task_num() + 1);
	sRoleConfig->refresh_hhb(player, false);

	const s_t_role *t_role = 0;
	int refresh_count = 0;
	for (int i = 0; i < player->hbb_class_ids_size(); ++i)
	{
		t_role = sRoleConfig->get_role(player->hbb_class_ids(i));
		if (t_role && t_role->font_color >= 4)
		{
			refresh_count += 1;
		}
	}
	
	ResMessage::res_hbb_look(player, name, id);
}

void MissionManager::terminal_hbb_fight_end(const std::string &data, const std::string &name, int id)
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

	const s_t_exp *t_exp = sPlayerConfig->get_exp(player->level());
	if (!t_exp)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->hbb_finish_num() >= player->hbb_num())
	{
		PERROR(ERROR_MISSION_CISHU);
		return;
	}

	std::vector<int> ids;
	for (int i = 0; i < player->hbb_class_ids_size(); ++i)
	{
		ids.push_back(player->hbb_class_ids(i));
	}
	std::string text;
	int result = MissionFight::mission_hbb(player, ids, text);

	int p = Utils::get_int32(0, player->hbb_class_ids_size() - 1);
	int cid = ids[p];
	std::vector<s_t_reward> rewards;
	int num = 0;
	int sp_id = sItemConfig->get_suipian(cid);
	if (!sp_id)
	{
		GLOBAL_ERROR;
		return;
	}
	if (result == 1)
	{
		int fgl = Utils::get_int32(0, 29);
		if (fgl == 0)
		{
			num = Utils::get_int32(1, 12);
		}
		else if (fgl < 5)
		{
			num = Utils::get_int32(1, 6);
		}
		else
		{
			num = Utils::get_int32(1, 3);
		}		
	}
	else
	{
		num = 1;
	}

	int zhanhun = Utils::get_int32(t_exp->zhanhun, t_exp->zhanhun * 2);
	const s_t_role* t_role = sRoleConfig->get_role(cid);
	if (t_role && t_role->font_color >= 4)
	{
		zhanhun = zhanhun * 2;
	}

	s_t_rewards rds;
	rds.add_reward(2, sp_id, num);
	rds.add_reward(1, resource::ZHANHUN, zhanhun);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HBB_END);

	player->set_hbb_finish_num(player->hbb_finish_num() + 1);
	player->set_hbb_refresh_num(0);
	sRoleConfig->refresh_hhb(player);
	PlayerOperation::player_add_active(player, 1600, 1);

	ResMessage::res_hbb_fight_end(player, result, text, rds, name, id);
}

void MissionManager::terminal_ttt_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_ttt_fight_end msg;
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

	if (!PlayerOperation::check_fight_time(player))
	{
		PERROR(ERROR_FIGHT_TIME);
		return;
	}
	
	int nd = msg.nd();
	if (nd < 1 || nd > 3)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->ttt_dead())
	{
		GLOBAL_ERROR;
		return;
	}
	int index = player->ttt_cur_stars_size();
	if (index / 3 > player->ttt_reward_ids_size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->ttt_can_reward())
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_ttt *t_ttt = sMissionConfig->get_ttt(index + 1);
	if (!t_ttt)
	{
		GLOBAL_ERROR;
		return;
	}
	int result = 1;
	std::string text;
	result = MissionFight::mission_ttt(player, t_ttt->d_type, t_ttt->d_param, t_ttt->monster_ids[nd - 1], text);

	int mibao_id = 0;
	int baoji1 = 0;
	int baoji2 = 0;
	s_t_rewards rds;
	if (result == 1)
	{
		player->add_ttt_cur_stars(nd);
		player->set_ttt_star(player->ttt_star() + nd);
		if (index >= player->ttt_last_stars_size())
		{
			player->add_ttt_last_stars(nd);
		}
		else if (nd > player->ttt_last_stars(index))
		{
			player->set_ttt_last_stars(index, nd);
		}

		std::pair<int, double> baoji = MissionOperation::get_baoji();
		baoji1 = baoji.first;
		rds.add_reward(1, resource::GOLD, t_ttt->mw_point[nd - 1] * baoji.second);

		baoji = MissionOperation::get_baoji();
		baoji2 = baoji.first;
		rds.add_reward(1, resource::HEJIN, t_ttt->shi[nd - 1] * baoji.second);

		int reward_mod = (index + 1) % 3;
		if (reward_mod == 0)
		{
			player->set_ttt_can_reward(1);
		}

		RoleOperation::check_bskill_count(player, BSKILL_TYPE5, nd);
	}
	else
	{
		player->set_ttt_dead(1);
		const s_t_ttt_baozang *t_mibao = sItemConfig->get_ttt_random_baozhang(MissionOperation::get_ttt_cur_mission_star(player));
		if (t_mibao)
		{
			mibao_id = t_mibao->id;
			player->set_ttt_mibao(mibao_id);
		}
	}
	player->set_ttt_task_num(player->ttt_task_num() + 1);
	PlayerOperation::player_add_active(player, 1200, 1);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TTT_END);

	ResMessage::res_ttt_fight_end(player, result, text, index, msg.nd(), mibao_id, baoji1, baoji2, rds, name, id);
}

void MissionManager::terminal_ttt_sanxing(const std::string &data, const std::string &name, int id)
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

	if (player->ttt_dead())
	{
		GLOBAL_ERROR;
		return;
	}

	int index = player->ttt_cur_stars_size();
	int max = player->ttt_last_stars_size();
	if (index >= max)
	{
		GLOBAL_ERROR;
		return;
	}
	int saodang_num = (index / 3 + 1) * 3 - index;
	if (saodang_num <= 0 || saodang_num > 3)
	{
		GLOBAL_ERROR;
		return;
	}
	if (index + saodang_num > max)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < saodang_num; ++i)
	{
		if ((index + i) >= player->ttt_last_stars_size())
		{
			GLOBAL_ERROR;
			return;
		}
		if (player->ttt_last_stars(index + i) < 3)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	bool has_jieri = sHuodongPool->has_jieri_huodong(player);
	protocol::game::smsg_ttt_sanxing smsg;
	s_t_rewards rds;
	for (int i = 0; i < saodang_num; ++i)
	{
		s_t_ttt *t_ttt = sMissionConfig->get_ttt(index + i + 1);
		if (t_ttt)
		{
			player->add_ttt_cur_stars(3);
			player->set_ttt_star(player->ttt_star() + 3);

			std::pair<int, double> baoji;
			protocol::game::smsg_ttt_fight_end *sub = smsg.add_subs();
			sub->set_result(1);
			sub->set_text("");
			sub->set_index(player->ttt_cur_stars_size());
			sub->set_nd(3);
			sub->set_mibao(0);

			if (t_ttt->mw_point.size() >= 3)
			{
				baoji = MissionOperation::get_baoji();
				sub->set_baoji1(baoji.first);

				int gold = t_ttt->mw_point[2] * baoji.second;
				rds.add_reward(1, resource::GOLD, gold);

				sub->add_types(1);
				sub->add_value1s(resource::GOLD);
				sub->add_value2s(gold);
				sub->add_value3s(0);

			}
			if (t_ttt->shi.size() >= 3)
			{
				baoji = MissionOperation::get_baoji();
				sub->set_baoji2(baoji.first);

				int hejin = t_ttt->shi[2] * baoji.second;
				rds.add_reward(1, resource::HEJIN, hejin);

				sub->add_types(1);
				sub->add_value1s(resource::HEJIN);
				sub->add_value2s(hejin);
				sub->add_value3s(0);
			}

			if ((player->ttt_cur_stars_size() % 3) == 0)
			{
				s_t_ttt_reward *t_ttt_reward = sMissionConfig->get_ttt_reward(((player->ttt_cur_stars_size() - 1) / 3 + 1));
				if (t_ttt_reward && t_ttt_reward->rewards.size() >= 3)
				{
					rds.add_reward(t_ttt_reward->rewards[2]);

					protocol::game::smsg_ttt_fight_end *sub1 = smsg.add_subs();
					sub1->set_result(0);
					sub1->set_text("");
					sub1->set_index(0);
					sub1->set_nd(0);
					sub1->set_mibao(0);
					sub1->set_baoji1(0);
					sub1->set_baoji2(0);
					std::vector<s_t_reward> &ttt_rds = t_ttt_reward->rewards[2];
					for (std::vector<s_t_reward>::size_type m = 0; m < ttt_rds.size(); ++m)
					{
						if (!has_jieri && (ttt_rds[m].value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
							ttt_rds[m].value1 == s_t_rewards::HUODONG_ITEM_ID2))
						{
							continue;
						}
						sub1->add_types(ttt_rds[m].type);
						sub1->add_value1s(ttt_rds[m].value1);
						sub1->add_value2s(ttt_rds[m].value2);
						sub1->add_value3s(ttt_rds[m].value3);
					}
				}
			}

			player->set_ttt_task_num(player->ttt_task_num() + 1);
			PlayerOperation::player_add_active(player, 1200, 1);
		}
	}
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TTT_SANXING);
	RoleOperation::check_bskill_count(player, BSKILL_TYPE5, saodang_num * 3);
	ResMessage::res_ttt_sanxing(player, smsg, name, id);
}

void MissionManager::terminal_ttt_saodang(const std::string &data, const std::string &name, int id)
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

	if (player->ttt_dead())
	{
		GLOBAL_ERROR;
		return;
	}

	int index = player->ttt_cur_stars_size();
	int max = player->ttt_last_stars_size();
	int saodang_num = 0;
	for (int i = index; i < max; ++i)
	{
		if (player->ttt_last_stars(i) < 3)
		{
			break;
		}
		else
		{
			++saodang_num;
		}
	}
	if (saodang_num <= 0)
	{
		GLOBAL_ERROR;
		return;
	}
	bool has_jieri = sHuodongPool->has_jieri_huodong(player);
	protocol::game::smsg_ttt_sanxing smsg;
	s_t_rewards rds;
	for (int i = 0; i < saodang_num; ++i)
	{
		s_t_ttt *t_ttt = sMissionConfig->get_ttt(index + i + 1);
		if (t_ttt)
		{
			player->add_ttt_cur_stars(3);
			player->set_ttt_star(player->ttt_star() + 3);

			std::pair<int, double> baoji;
			protocol::game::smsg_ttt_fight_end *sub = smsg.add_subs();
			sub->set_result(1);
			sub->set_text("");
			sub->set_index(player->ttt_cur_stars_size());
			sub->set_nd(3);
			sub->set_mibao(0);

			if (t_ttt->mw_point.size() >= 3)
			{
				baoji = MissionOperation::get_baoji();
				sub->set_baoji1(baoji.first);

				int gold = t_ttt->mw_point[2] * baoji.second;
				rds.add_reward(1, resource::GOLD, gold);

				sub->add_types(1);
				sub->add_value1s(resource::GOLD);
				sub->add_value2s(gold);
				sub->add_value3s(0);

			}
			if (t_ttt->shi.size() >= 3)
			{
				baoji = MissionOperation::get_baoji();
				sub->set_baoji2(baoji.first);

				int hejin = t_ttt->shi[2] * baoji.second;
				rds.add_reward(1, resource::HEJIN, hejin);

				sub->add_types(1);
				sub->add_value1s(resource::HEJIN);
				sub->add_value2s(hejin);
				sub->add_value3s(0);
			}

			if ((player->ttt_cur_stars_size() % 3) == 0)
			{
				s_t_ttt_reward *t_ttt_reward = sMissionConfig->get_ttt_reward(((player->ttt_cur_stars_size() - 1) / 3 + 1));
				if (t_ttt_reward && t_ttt_reward->rewards.size() >= 3)
				{
					rds.add_reward(t_ttt_reward->rewards[2]);

					int mtotal_star = 0;
					int value_id = 0;
					for (int mindex = player->ttt_cur_stars_size() - 3; mindex < player->ttt_cur_stars_size(); ++mindex)
					{
						if (mindex >= 0)
						{
							mtotal_star += player->ttt_cur_stars(mindex);
						}
						
					}
					mtotal_star = mtotal_star / 3 * 3;
					if (mtotal_star > 0 && player->ttt_star() >= mtotal_star)
					{
						player->set_ttt_star(player->ttt_star() - mtotal_star);
						value_id = sMissionConfig->get_random_ttt_value(mtotal_star / 3);
						player->add_ttt_reward_ids(value_id);
					}

					protocol::game::smsg_ttt_fight_end *sub1 = smsg.add_subs();
					sub1->set_result(0);
					sub1->set_text("");
					sub1->set_index(0);
					sub1->set_nd(value_id);
					sub1->set_mibao(0);
					sub1->set_baoji1(0);
					sub1->set_baoji2(0);
					std::vector<s_t_reward> &ttt_rds = t_ttt_reward->rewards[2];
					for (std::vector<s_t_reward>::size_type m = 0; m < ttt_rds.size(); ++m)
					{
						if (!has_jieri && (ttt_rds[m].value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
							ttt_rds[m].value1 == s_t_rewards::HUODONG_ITEM_ID2))
						{
							continue;
						}
						sub1->add_types(ttt_rds[m].type);
						sub1->add_value1s(ttt_rds[m].value1);
						sub1->add_value2s(ttt_rds[m].value2);
						sub1->add_value3s(ttt_rds[m].value3);
					}
				}
			}

			player->set_ttt_task_num(player->ttt_task_num() + 1);
			PlayerOperation::player_add_active(player, 1200, 1);
		}
	}
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TTT_SAODANG);
	RoleOperation::check_bskill_count(player, BSKILL_TYPE5, saodang_num * 3);
	ResMessage::res_ttt_sanxing(player, smsg, name, id);
}

void MissionManager::terminal_ttt_value_look(const std::string &data, const std::string &name, int id)
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

	int index = player->ttt_cur_stars_size();
	if (index / 3 <= player->ttt_reward_ids_size())
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->ttt_cur_reward_ids_size() == 0)
	{
		for (int i = 1; i <= 3; ++i)
		{
			int vid = sMissionConfig->get_random_ttt_value(i);
			player->add_ttt_cur_reward_ids(vid);
		}
	}

	ResMessage::res_ttt_value_look(player, name, id);
}

void MissionManager::terminal_ttt_value(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_ttt_value msg;
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

	int index = player->ttt_cur_stars_size();
	if (index / 3 <= player->ttt_reward_ids_size())
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->ttt_cur_reward_ids_size() == 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.index() < 0 || msg.index() >= 3)
	{
		GLOBAL_ERROR;
		return;
	}
	int star = (msg.index() + 1) * 3;
	if (player->ttt_star() < star)
	{
		GLOBAL_ERROR;
		return;
	}
	player->set_ttt_star(player->ttt_star() - star);
	player->add_ttt_reward_ids(player->ttt_cur_reward_ids(msg.index()));
	player->clear_ttt_cur_reward_ids();

	ResMessage::res_success(player, true, name, id);
}

void MissionManager::terminal_ttt_reward(const std::string &data, const std::string &name, int id)
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

	if (player->ttt_can_reward() == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	int index = player->ttt_cur_stars_size();
	int reward_mod = index % 3;
	if (reward_mod != 0)
	{
		GLOBAL_ERROR;
		return;
	}
	int reward_index = (index - 1) / 3 + 1;
	s_t_ttt_reward *t_ttt_reward = sMissionConfig->get_ttt_reward(reward_index);
	if (!t_ttt_reward)
	{
		GLOBAL_ERROR;
		return;
	}
	int star = 0;
	for (int i = 1; i <= 3; ++i)
	{
		star += player->ttt_cur_stars(index - i);
	}
	star = star / 3 - 1;
	s_t_rewards rds;
	rds.add_reward(t_ttt_reward->rewards[star]);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TTT_REWARD);
	player->set_ttt_can_reward(0);

	ResMessage::res_success(player, true, name, id);
}

void MissionManager::terminal_ttt_cz(const std::string &data, const std::string &name, int id)
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

	s_t_vip *t_vip = sPlayerConfig->get_vip(player->vip());
	if (!t_vip)
	{
		GLOBAL_ERROR;
		return;
	}
	int num = player->ttt_cz_num();
	if (num >= t_vip->ttt_cz_num)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_price *t_price = sPlayerConfig->get_price(num + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_price->ttt_cz > player->jewel())
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_price->ttt_cz, LOGWAY_TTT_CZ);
	player->set_ttt_dead(0);
	player->set_ttt_star(0);
	player->set_ttt_can_reward(0);
	player->clear_ttt_reward_ids();
	player->clear_ttt_cur_stars();
	player->clear_ttt_cur_reward_ids();
	player->set_ttt_cz_num(player->ttt_cz_num() + 1);
	player->set_ttt_mibao(0);

	ResMessage::res_success(player, true, name, id);
}

/*
void MissionManager::terminal_xjbz_get(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_xjbz_get msg;
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

	if (player->xjbz_state() != 0)
	{
		GLOBAL_ERROR;
		return;
	}
	int site = msg.site();
	s_t_xjbz *t_xjbz = sMissionConfig->get_xjbz(site);
	if (!t_xjbz)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_xjbz->type == 1 || t_xjbz->type == 2)
	{
		GLOBAL_ERROR;
		return;
	}
	for (int i = 0; i < player->xjbz_sites_size(); ++i)
	{
		if (player->xjbz_sites(i) == site)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	s_t_xjbz_sub *t_xjbz_sub = sMissionConfig->get_xjbz_sub(t_xjbz->type);
	if (!t_xjbz_sub)
	{
		GLOBAL_ERROR;
		return;
	}
	int gold1 = Utils::get_int32(t_xjbz_sub->gold1, t_xjbz_sub->gold3);
	int gold2 = Utils::get_int32(t_xjbz_sub->gold2, t_xjbz_sub->gold4);
	int gold = gold1 + gold2 * player->level();
	int baoshi1 = Utils::get_int32(t_xjbz_sub->baoshi1, t_xjbz_sub->baoshi3);
	int baoshi2 = Utils::get_int32(t_xjbz_sub->baoshi2, t_xjbz_sub->baoshi4);
	int baoshi = baoshi1 + baoshi2 * player->level();
	int zz = Utils::get_int32(t_xjbz_sub->zz1, t_xjbz_sub->zz2);
	if (t_xjbz->type == 11)
	{
		gold = gold * (player->xjbz_zz() + 100) / 100;
		player->set_xjbz_state(1);
		PlayerOperation::player_add_active(player, 1500, 1);
	}
	player->add_xjbz_sites(site);

	PlayerOperation::player_add_gold(player, gold, LOGWAY_XJBZ);
	player->set_xjbz_gold(player->xjbz_gold() + gold);
	player->set_baoshi(player->baoshi() + baoshi);
	player->set_xjbz_baoshi(player->xjbz_baoshi() + baoshi);
	player->set_xjbz_zz(player->xjbz_zz() + zz);

	ResMessage::res_xjbz_get(player, gold, baoshi, zz, name, id);
}

void MissionManager::terminal_xjbz_buy(const std::string &data, const std::string &name, int id)
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

	if (player->jewel() < 20)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	//player->set_jewel(player->jewel() - 20);
	PlayerOperation::player_consume(player, 20, LOGWAY_XJBZ_BUY);

	ResMessage::res_success(player, true, name, id);
}
*/

void MissionManager::terminal_yb_refresh(const std::string &data, const std::string &name, int id)
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

	if (player->yb_finish_num() >= YB_NUM)
	{
		GLOBAL_ERROR;
		return;
	}
	if (sMissionPool->is_start_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->yb_type() == 4)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->yb_refresh_num() >= 3 && player->jewel() < 20)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	int sum = 0;
	for (int i = player->yb_type(); i < 5; ++i)
	{
		s_t_yb *t_yb = sMissionConfig->get_yb(i);
		if (!t_yb)
		{
			GLOBAL_ERROR;
			return;
		}
		sum += t_yb->rate;
	}
	if (sum == 0)
	{
		GLOBAL_ERROR;
		return;
	}
	
	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	int index = 0;
	for (int i = player->yb_type(); i < 5; ++i)
	{
		s_t_yb *t_yb = sMissionConfig->get_yb(i);
		if (!t_yb)
		{
			GLOBAL_ERROR;
			return;
		}
		gl += t_yb->rate;
		if (gl > rate)
		{
			index = i;
			break;
		}
	}
	player->set_yb_type(index);
	if (player->yb_refresh_num() >= 3)
	{
		PlayerOperation::player_dec_resource(player, resource::JEWEL, 20, LOGWAY_YB_REFRESH);
	}
	player->set_yb_refresh_num(player->yb_refresh_num() + 1);

	ResMessage::res_yb_refresh(player, index, name, id);
}

void MissionManager::terminal_yb_gw(const std::string &data, const std::string &name, int id)
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

	if (player->yb_finish_num() >= YB_NUM)
	{
		GLOBAL_ERROR;
		return;
	}
	if (sMissionPool->is_start_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->yb_gw_type() == 5)
	{
		GLOBAL_ERROR;
		return;
	}
	int type = player->yb_gw_type();
	s_t_yb_gw *t_yb_gw = sMissionConfig->get_yb_gw(type + 1);
	if (!t_yb_gw)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < t_yb_gw->jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	player->set_yb_gw_type(type + 1);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_yb_gw->jewel, LOGWAY_YB_GW);

	ResMessage::res_success(player, true, name, id);
}

void MissionManager::terminal_yb_zh(const std::string &data, const std::string &name, int id)
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

	if (player->yb_finish_num() >= YB_NUM)
	{
		GLOBAL_ERROR;
		return;
	}
	if (sMissionPool->is_start_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->yb_type() == 4)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < YB_ZH)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	player->set_yb_type(4);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, YB_ZH, LOGWAY_YB_ZH);

	ResMessage::res_success(player, true, name, id);
}

void MissionManager::terminal_yb(const std::string &data, const std::string &name, int id)
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

	if (player->yb_finish_num() >= YB_NUM)
	{
		GLOBAL_ERROR;
		return;
	}
	if (sMissionPool->is_start_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_yb_start_time(game::timer()->now());
	player->set_yb_finish_num(player->yb_finish_num() + 1);
	player->set_yb_per(100);
	player->set_yb_level(player->level());
	sMissionPool->start_yb(player);

	ResMessage::res_success(player, true, name, id);
}

void MissionManager::terminal_yb_jiasu(const std::string &data, const std::string &name, int id)
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

	if (!sMissionPool->is_start_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (sMissionPool->is_jiasu_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (sMissionPool->is_finish_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < 40)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, 40, LOGWAY_YB_JIASU);
	player->set_yb_jiasu_time(game::timer()->now());
	sMissionPool->refresh_yb(player);

	ResMessage::res_success(player, true, name, id);
}

void MissionManager::terminal_yb_finish(const std::string &data, const std::string &name, int id)
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

	if (!sMissionPool->is_start_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (sMissionPool->is_finish_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < 250)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	s_t_yb *t_yb = sMissionConfig->get_yb(player->yb_type());
	if (!t_yb)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_exp *t_exp = sPlayerConfig->get_exp(player->yb_level());
	if (!t_exp)
	{
		GLOBAL_ERROR;
		return;
	}
	int yuanli = t_exp->yuanli * t_yb->yuanli_bs * player->yb_per() / 100;
	if (player->yb_type() > 0)
	{
		sMissionPool->add_yb_info(1, player->name(), "", player->yb_type(), yuanli);
	}

	PlayerOperation::player_add_resource(player, resource::YUANLI, yuanli, LOGWAY_YB_FINISH);

	player->set_yb_type(0);
	player->set_yb_level(0);
	player->set_yb_start_time(0);
	player->set_yb_jiasu_time(0);
	player->set_yb_gw_type(0);
	player->set_yb_byb_num(0);
	player->set_yb_per(0);

	sMissionPool->remove_yb(player);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, 250, LOGWAY_YB_FINISH);
	PlayerOperation::player_add_active(player, 1300, 1);
	
	ResMessage::res_yb_reward(player, yuanli, name, id);
}

void MissionManager::terminal_yb_look(const std::string &data, const std::string &name, int id)
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

	std::vector<protocol::game::msg_yb_player> yb_players;
	int player_id = sMissionPool->get_yb_players(player, yb_players, 0, 300);
	std::list<protocol::game::msg_yb_info> yb_infos;
	sMissionPool->get_yb_info(yb_infos);
	std::list<protocol::game::msg_ybq_info> ybq_infos;
	sMissionPool->get_ybq_info(player->guid(), ybq_infos);

	ResMessage::res_yb_look(player, yb_players, yb_infos, ybq_infos, player_id, name, id);
}

void MissionManager::terminal_yb_look_ex(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_yb_look_ex msg;
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
	PCK_CHECK_EX;

	std::vector<protocol::game::msg_yb_player> yb_players;
	int player_id = sMissionPool->get_yb_players(player, yb_players, msg.player_id(), 10);
	std::list<protocol::game::msg_yb_info> yb_infos;
	sMissionPool->get_yb_info(yb_infos);
	std::list<protocol::game::msg_ybq_info> ybq_infos;
	sMissionPool->get_ybq_info(player->guid(), ybq_infos);

	ResMessage::res_yb_look_ex(player, yb_players, yb_infos, ybq_infos, player_id, name, id);
}

void MissionManager::terminal_yb_ybq_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_yb_ybq_fight_end msg;
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

	if (!PlayerOperation::check_fight_time(player))
	{
		PERROR(ERROR_FIGHT_TIME);
		return;
	}
	if (player->ybq_finish_num() >= 5)
	{
		GLOBAL_ERROR;
		return;
	}
	dhc::player_t *target = POOL_GET_PLAYER(msg.target_guid());
	if (!target)
	{
		GLOBAL_ERROR;
		return;
	}
	if (!sMissionPool->is_start_yb(target))
	{
		PERROR(ERROR_YB_FINISH);
		return;
	}
	if (sMissionPool->is_finish_yb(target))
	{
		PERROR(ERROR_YB_FINISH);
		return;
	}
	if (target->yb_byb_num() >= 2)
	{
		PERROR(ERROR_YB_MAX);
		return;
	}
	if (player->ybq_last_time() + YBQ_TIME > game::timer()->now())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->guid() == target->guid())
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_yb_gw *t_yb_gw = sMissionConfig->get_yb_gw(target->yb_gw_type());
	if (!t_yb_gw)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_rewards rds;
	std::string text;
	int result = MissionFight::mission_yb(player, target, t_yb_gw->gj, text);
	if (result == 1)
	{
		s_t_yb *t_yb = sMissionConfig->get_yb(target->yb_type());
		if (!t_yb)
		{
			GLOBAL_ERROR;
			return;
		}
		int per = t_yb->yuanli_ybq_per;
		if (player->level() > target->level())
		{
			per = per + target->level() - player->level();
		}
		if (per < t_yb->yuanli_ybq_min_per)
		{
			per = t_yb->yuanli_ybq_min_per;
		}
		target->set_yb_per(target->yb_per() - per);
		s_t_exp *t_exp = sPlayerConfig->get_exp(target->yb_level());
		if (!t_exp)
		{
			GLOBAL_ERROR;
			return;
		}
		int yuanli = t_exp->yuanli * t_yb->yuanli_bs * per / 100;
		rds.add_reward(1, resource::YUANLI, yuanli);

		if (target->yb_type() > 0)
		{
			sMissionPool->add_yb_info(2, player->name(), target->name(), target->yb_type(), yuanli);
			sMissionPool->add_ybq_info(target, target->yb_type(), player->name(), yuanli);
		}
		target->set_yb_byb_num(target->yb_byb_num() + 1);
		for (int i = 0; i < target->ybq_guids_size(); ++i)
		{
			if (target->ybq_guids(i) == player->guid())
			{
				for (int j = i; j < target->ybq_guids_size() - 1; ++j)
				{
					target->set_ybq_guids(j, target->ybq_guids(j + 1));
				}
				target->mutable_ybq_guids()->RemoveLast();
				break;
			}
		}
		target->add_ybq_guids(player->guid());
		sMissionPool->refresh_yb(target);
		player->set_ybq_finish_num(player->ybq_finish_num() + 1);
		player->set_ybq_last_time(game::timer()->now());
		player->set_yb_task_num(player->yb_task_num() + 1);
	}
	else
	{
		sMissionPool->add_ybq_info(target, target->yb_type(), player->name(), 0);
		player->set_ybq_finish_num(player->ybq_finish_num() + 1);
		player->set_ybq_last_time(game::timer()->now());
	}
	sHuodongPool->huodong_active(player, HUODONG_COND_TKYS_LJ_COUNT, 1);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_YBQ);
	ResMessage::res_yb_ybq_fight_end(player, result, text, rds, name, id);
}

void MissionManager::terminal_yb_reward(const std::string &data, const std::string &name, int id)
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

	if (!sMissionPool->is_start_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}
	if (!sMissionPool->is_finish_yb(player))
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_yb *t_yb = sMissionConfig->get_yb(player->yb_type());
	if (!t_yb)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_exp *t_exp = sPlayerConfig->get_exp(player->yb_level());
	if (!t_exp)
	{
		GLOBAL_ERROR;
		return;
	}

	int yuanli = t_exp->yuanli * t_yb->yuanli_bs * player->yb_per() / 100;

	if (player->yb_type() > 0)
	{
		sMissionPool->add_yb_info(1, player->name(), "", player->yb_type(), yuanli);
	}

	PlayerOperation::player_add_resource(player, resource::YUANLI, yuanli, LOGWAY_YB_REWARD);

	player->set_yb_type(0);
	player->set_yb_level(0);
	player->set_yb_start_time(0);
	player->set_yb_jiasu_time(0);
	player->set_yb_gw_type(0);
	player->set_yb_byb_num(0);
	player->set_yb_per(0);

	sMissionPool->remove_yb(player);
	PlayerOperation::player_add_active(player, 1300, 1);

	ResMessage::res_yb_reward(player, yuanli, name, id);
}

void MissionManager::terminal_ore_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_ore_fight_end msg;
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

	PlayerOperation::player_do_tili(player);

	int index = msg.index();
	if (index < 0 || index > player->ore_nindex())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->ore_finish_num() >= ORE_NUM)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->ore_last_time() + ORE_TIME > game::timer()->now())
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_ore *t_ore = sMissionConfig->get_t_ore(index);
	if (!t_ore)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->tili() < t_ore->tili)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->level() < t_ore->level)
	{
		GLOBAL_ERROR;
		return;
	}

	int gold = 0;
	double hper = 0;
	int hp;
	std::string text;
	int result = MissionFight::mission_ore(player, t_ore->monster_id, hp, hper, text);
	if (result == 1)
	{
		gold = t_ore->hx_gold + t_ore->bd_gold;
		if (index == player->ore_nindex())
		{
			player->set_ore_nindex(player->ore_nindex() + 1);
		}
	}
	else
	{
		gold = t_ore->hx_gold * hper + t_ore->bd_gold;
	}

	PlayerOperation::player_dec_resource(player, resource::TILI, t_ore->tili, LOGWAY_ORE);

	s_t_rewards rds;
	rds.add_reward(1, resource::GOLD, gold);
	if (t_ore->jl_num > 0)
		rds.add_reward(2, 50070001, t_ore->jl_num);
	for (int i = 0; i < t_ore->zj_num; ++i)
	{
		rds.add_reward(6, 13001, 0);
	}
	if (t_ore->sz_num > 0)
		rds.add_reward(2, 50100001, t_ore->sz_num);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ORE);
	player->set_ore_finish_num(player->ore_finish_num() + 1);
	player->set_ore_last_time(game::timer()->now());
	PlayerOperation::player_add_active(player, 1500, 1);

	ResMessage::res_ore_fight_end(player, result, text, rds, hp, name, id);
}

void MissionManager::terminal_qiyu_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_qiyu_fight_end msg;
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

	if (msg.index() < 0 || msg.index() >= player->qiyu_mission_size())
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->qiyu_suc(msg.index()))
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_mission* t_mission = sMissionConfig->get_mission(player->qiyu_mission(msg.index()));
	if (!t_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_qingyu* t_qiyu = sMissionConfig->get_qiyu(t_mission->map);
	if (!t_qiyu)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_exp *t_exp = sPlayerConfig->get_exp(player->level());
	if (!t_exp)
	{
		GLOBAL_ERROR;
		return;
	}

	int tili = t_qiyu->tili;
	int zhuangzhi_min = t_exp->dxqhzz + t_qiyu->zhuangzhi;
	int zhuangzhi_max = zhuangzhi_min * 150 / 100;
	if (player->qiyu_hard(msg.index()) == 1)
	{
		tili *= 2;
		zhuangzhi_min = zhuangzhi_min * 120 / 100;
		zhuangzhi_max = zhuangzhi_max * 120 / 100;
	}
	else if (player->qiyu_hard(msg.index()) == 2)
	{
		tili *= 3;
		zhuangzhi_min = zhuangzhi_min * 150 / 100;
		zhuangzhi_max = zhuangzhi_max * 150 / 100;
	}
	int exp = t_qiyu->tili * 5;
	int gold = t_qiyu->tili * player->level() * 10 + 50;
	

	PlayerOperation::player_do_tili(player);
	if (player->tili() < tili)
	{
		PERROR(ERROR_TILI);
		return;
	}

	std::string text;
	int star = 0;
	int result = MissionFight::mission_gq(player, t_mission->id, star, false, text);

	s_t_rewards rds;
	if (result == 1)
	{
		player->set_qiyu_suc(msg.index(), 1);
		PlayerOperation::player_dec_resource(player, resource::TILI, tili, LOGWAY_MISSION_QIYU);

		rds.add_reward(1, resource::EXP, exp);
		rds.add_reward(1, resource::GOLD, gold);
		rds.add_reward(2, 50140001, Utils::get_int32(zhuangzhi_min, zhuangzhi_max));

		PlayerOperation::player_add_active(player, 2400, 1);
	}

	PlayerOperation::player_add_reward(player, rds, LOGWAY_MISSION_QIYU);
	ResMessage::res_qiyu_fight_end(player, result, text, rds, name, id);
}

void MissionManager::terminal_qiyu_check(const std::string &data, const std::string &name, int id)
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

	ResMessage::res_qiyu_check(player, name, id);
}

