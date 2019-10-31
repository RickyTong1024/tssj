#include "player_operation.h"
#include "player_config.h"
#include "player_def.h"
#include "item_operation.h"
#include "sport_operation.h"
#include "equip_operation.h"
#include "role_operation.h"
#include "role_config.h"
#include "equip_config.h"
#include "mission_pool.h"
#include "post_operation.h"
#include "social_operation.h"
#include "guild_operation.h"
#include "huodong_operation.h"
#include "huodong_pool.h"
#include "rank_operation.h"
#include "item_config.h"
#include "mission_operation.h"
#include "treasure_operation.h"
#include "treasure_config.h"
#include "mission_config.h"
#include "mission_pool.h"
#include "pvp_operation.h"
#include "pvp_config.h"

std::set<uint64_t> player_shops_;
std::set<uint64_t> player_shop_refresh_;
static std::map<uint64_t, s_t_rewards> player_zhichong_;
static std::map<uint64_t, std::pair<int, int> > player_chongzhi_;

s_t_rewards *PlayerOperation::get_player_zhichong(uint64_t player_guid)
{
	std::map<uint64_t, s_t_rewards>::iterator it = player_zhichong_.find(player_guid);
	if (it != player_zhichong_.end())
	{
		return &(*it).second;
	}
	else
	{
		return 0;
	}
}

 void PlayerOperation::remove_player_zhichong(uint64_t player_guid)
{
	player_zhichong_.erase(player_guid);
}

 void PlayerOperation::add_player_zhichong(uint64_t player_guid, const s_t_rewards &rds)
{
	std::map<uint64_t, s_t_rewards >::iterator it = player_zhichong_.find(player_guid);
	if (it == player_zhichong_.end())
	{
		player_zhichong_[player_guid] = rds;
	}
	else
	{
		player_zhichong_[player_guid] = rds;
	}
}

 std::pair<int,int> *PlayerOperation::get_player_chongzhi(uint64_t player_guid)
{
	std::map<uint64_t,std::pair<int, int> >::iterator it = player_chongzhi_.find(player_guid);
	if (it != player_chongzhi_.end())
	{
		return &(*it).second;
	}
	else
	{
		return 0;
	}
}

void PlayerOperation::add_player_chongzhi(uint64_t player_guid, int pid, int count)
{
	std::map<uint64_t,std::pair<int, int> >::iterator it = player_chongzhi_.find(player_guid);
	if (it == player_chongzhi_.end())
	{
		player_chongzhi_[player_guid] =std::make_pair(pid, count);
	}
	else
	{
		it->second = std::make_pair(pid, count);
	}
}
void PlayerOperation::remove_player_chongzhi(uint64_t player_guid)
{
	player_chongzhi_.erase(player_guid);
}

void PlayerOperation::player_login(dhc::player_t * player)
{
	while (player->ck_num_size() < 3)
	{
		player->add_ck_num(0);
	}
	while (player->zhenxing_size() < 6)
	{
		player->add_zhenxing(0);
	}
	while (player->duixing_size() < 6)
	{
		player->add_duixing(player->duixing_size());
	}
	while (player->houyuan_size() < 6)
	{
		player->add_houyuan(0);
	}

	while (player->huiyi_jihuo_starts_size() < player->huiyi_jihuos_size())
	{
		player->add_huiyi_jihuo_starts(0);
	}

	for (int i = 0; i < player->roles_size();)
	{
		uint64_t roles_guid = player->roles(i);
		dhc::role_t *role = POOL_GET_ROLE(roles_guid);
		if (!role)
		{
			player->mutable_roles()->SwapElements(i, player->roles_size() - 1);
			player->mutable_roles()->RemoveLast();
		}
		else
		{
			++i;

			while (role->zhuangbeis_size() < 4)
			{
				role->add_zhuangbeis(0);
			}
			while (role->treasures_size() < 2)
			{
				role->add_treasures(0);
			}
			while (role->treasures_size() > 2)
			{
				role->mutable_treasures()->RemoveLast();
			}
			while (role->jskill_level_size() < 7)
			{
				role->add_jskill_level(1);
			}
			if (!RoleOperation::role_has_dress(role, 0))
			{
				role->add_dress_ids(0);
			}
			while (role->bskill_counts_size() < 5)
			{
				role->add_bskill_counts(0);
			}

			bool is_zhengxing = false;
			for (int k = 0; k < player->zhenxing_size(); ++k)
			{
				if (player->zhenxing(k) == role->guid())
				{
					is_zhengxing = true;
					break;
				}
			}
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				uint64_t equip_guid = role->zhuangbeis(j);
				dhc::equip_t *equip = POOL_GET_EQUIP(equip_guid);
				if (!equip)
				{
					role->set_zhuangbeis(j, 0);
				}
				else if(equip->role_guid() != role->guid())
				{
					role->set_zhuangbeis(j, 0);
				}
				else if(!is_zhengxing)
				{
					equip->set_role_guid(0);
					role->set_zhuangbeis(j, 0);
				}
			}
			for (int m = 0; m < role->treasures_size(); ++m)
			{
				dhc::treasure_t *treasure = POOL_GET_TREASURE(role->treasures(m));
				if (!treasure)
				{
					role->set_treasures(m, 0);
				}
				else if (treasure->role_guid() != role->guid())
				{
					role->set_treasures(m, 0);
				}
				else if (!is_zhengxing)
				{
					treasure->set_role_guid(0);
					role->set_treasures(m, 0);
				}
			}
			dhc::pet_t *role_pet = POOL_GET_PET(role->pet());
			if (!role_pet)
			{
				role->set_pet(0);
			}
			else if (role_pet->role_guid() != role->guid())
			{
				role->set_pet(0);
			}
			else if (!is_zhengxing)
			{
				role->set_pet(0);
				role_pet->set_role_guid(0);
			}
			s_t_role *t_role = sRoleConfig->get_role(role->template_id());
			if (t_role)
			{
				if (role->pinzhi() == 0)
				{
					role->set_pinzhi(t_role->pinzhi * 100);
				}
			}
		}
	}

	for (int i = 0; i < player->equips_size();)
	{
		uint64_t equip_guid = player->equips(i);
		dhc::equip_t *equip = POOL_GET_EQUIP(equip_guid);
		s_t_equip *t_equip = 0;
		if (equip)
		{
			t_equip = sEquipConfig->get_equip(equip->template_id());
		}
		if (!equip || !t_equip)
		{
			player->mutable_equips()->SwapElements(i, player->equips_size() - 1);
			player->mutable_equips()->RemoveLast();
		}
		else
		{
			while (equip->stone_size() < t_equip->slot_num)
			{
				equip->add_stone(0);
			}
			++i;
		}
	}

	for (int i = 0; i < player->treasures_size();)
	{
		uint64_t treasure_guid = player->treasures(i);
		dhc::treasure_t *treasure = POOL_GET_TREASURE(treasure_guid);
		const s_t_treasure *t_treasure = 0;
		if (treasure)
		{
			t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
		}
		if (!treasure || !t_treasure)
		{
			player->mutable_treasures()->SwapElements(i, player->treasures_size() - 1);
			player->mutable_treasures()->RemoveLast();
		}
		else
		{
			int enhance_exp = 0;
			int color = t_treasure->color - 2;
			const s_t_treasure_enhance *t_enhance = 0;
			for (int mk = 1; mk <= treasure->enhance(); ++mk)
			{
				t_enhance = sTreasureConfig->get_enhance(mk);
				if (t_enhance)
				{
					if (color >= 0 && color < t_enhance->attrs.size())
					{
						enhance_exp += t_enhance->attrs[color];
					}
				}
			}
			if (treasure->enhance_counts() < enhance_exp)
			{
				treasure->set_enhance_counts(enhance_exp);
			}

			TreasureOperation::treasure_refresh_rate(treasure, t_treasure);
			++i; 
		}
	}

	for (int i = 0; i < player->sports_size();)
	{
		uint64_t sport_guid = player->sports(i);
		dhc::sport_t *sport = POOL_GET_SPORT(sport_guid);
		if (!sport)
		{
			player->mutable_sports()->SwapElements(i, player->sports_size() - 1);
			player->mutable_sports()->RemoveLast();
		}
		else
		{
			++i;
		}
	}

	for (int i = 0; i < player->treasure_reports_size();)
	{
		uint64_t treasure_report_guid = player->treasure_reports(i);
		dhc::treasure_report_t *treasure_report = POOL_GET_TREASURE_REPORT(treasure_report_guid);
		if (!treasure_report)
		{
			player->mutable_treasure_reports()->SwapElements(i, player->treasure_reports_size() - 1);
			player->mutable_treasure_reports()->RemoveLast();
		}
		else
		{
			++i;
		}
	}

	for (int i = 0; i < player->pets_size();)
	{
		dhc::pet_t *pet = POOL_GET_PET(player->pets(i));
		if (!pet)
		{
			if (player->pets(i) == player->pet_on())
			{
				player->set_pet_on(0);
			}
			player->mutable_pets()->SwapElements(i, player->pets_size() - 1);
			player->mutable_pets()->RemoveLast();
		}
		else
		{
			++i;
		}
	}

	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		uint64_t role_guid = player->zhenxing(i);
		dhc::role_t *role = POOL_GET_ROLE(role_guid);
		if (!role)
		{
			player->set_zhenxing(i, 0);
		}
	}

	std::map<int, int> map_stars;
	for (int i = 0; i < player->mission_ids_size();)
	{
		s_t_mission* t_mission = sMissionConfig->get_mission(player->mission_ids(i));
		if (!t_mission)
		{
			player->mutable_mission_ids()->SwapElements(i, player->mission_ids_size() - 1);
			player->mutable_mission_ids()->RemoveLast();

			player->mutable_mission_star()->SwapElements(i, player->mission_star_size() - 1);
			player->mutable_mission_star()->RemoveLast();
		}
		else
		{
			map_stars[t_mission->map] += player->mission_star(i);
			++i;
		}
	}

	for (int i = 0; i < player->map_ids_size();)
	{
		s_t_map * t_map = sMissionConfig->get_map(player->map_ids(i));
		if (t_map)
		{
			if (map_stars.find(t_map->id) != map_stars.end())
			{
				player->set_map_star(i, map_stars[t_map->id]);
				++i;
				continue;
			}
		}
		player->mutable_map_ids()->SwapElements(i, player->map_ids_size() - 1);
		player->mutable_map_ids()->RemoveLast();

		player->mutable_map_star()->SwapElements(i, player->map_star_size() - 1);
		player->mutable_map_star()->RemoveLast();

		player->mutable_map_reward_get()->SwapElements(i, player->map_reward_get_size() - 1);
		player->mutable_map_reward_get()->RemoveLast();
	}
	while (player->map_reward_get_size() > player->map_ids_size())
	{
		player->mutable_map_reward_get()->RemoveLast();
	}

	PlayerOperation::player_do_tili(player);
	PlayerOperation::player_do_energy(player);
	PlayerOperation::player_do_boss(player);
	ItemOperation::item_do_role_shop(player);
	ItemOperation::do_pet_shop(player);

	if (player->boss_player_level() <= 0)
	{
		player->set_boss_player_level(player->level());
	}

	uint64_t now = game::timer()->now();
	if (player_shops_.find(player->guid()) == player_shops_.end())
	{
		ItemOperation::refresh_role_shop(player);
		ItemOperation::refresh_huiyi_shop(player);
		ItemOperation::refresh_guild_shop(player);
		ItemOperation::refresh_pet_shop(player);
		player_shops_.insert(player->guid());
		player_shop_refresh_.insert(player->guid());
	}
	if (game::timer()->trigger_time(player->qiyu_last_time(), 6, 0))
	{
		MissionOperation::refresh_qiyu(player, 0);
		player->set_qiyu_last_time(now);
	}
	else if (game::timer()->trigger_time(player->qiyu_last_time(), 12, 0))
	{
		MissionOperation::refresh_qiyu(player, 1);
		player->set_qiyu_last_time(now);
	}
	else if (game::timer()->trigger_time(player->qiyu_last_time(), 18, 0))
	{
		MissionOperation::refresh_qiyu(player, 2);
		player->set_qiyu_last_time(now);
	}
	if (game::timer()->trigger_time(player->huiyi_shop_last_time(), 9, 0) ||
		game::timer()->trigger_time(player->huiyi_shop_last_time(), 12, 0) ||
		game::timer()->trigger_time(player->huiyi_shop_last_time(), 18, 0) ||
		game::timer()->trigger_time(player->huiyi_shop_last_time(), 21, 0))
	{
		ItemOperation::refresh_guild_shop(player);
		ItemOperation::refresh_huiyi_shop(player);
		player->set_huiyi_shop_last_time(now);
		player_shop_refresh_.insert(player->guid());
	}

	s_t_online_reward *t_online_reward = sPlayerConfig->get_online_reward(player->online_reward_index());
	if (t_online_reward)
	{
		if (now > player->online_reward_time())
		{
			if (player->last_check_time() != 0)
			{
				if (player->online_reward_time() > player->last_check_time())
				{
					player->set_online_reward_time(now + (player->online_reward_time() - player->last_check_time()) - 120000);
					if (player->online_reward_time() > (now + t_online_reward->time))
					{
						player->set_online_reward_time(now + t_online_reward->time);
					}
				}
			}
			else
			{
				player->set_online_reward_time(now + t_online_reward->time);
			}
		}
	}

	PlayerOperation::player_calc_force(player);

	if (player->dress_on_ids_size() == 0)
	{
		player->add_dress_on_ids(10001);
		player->add_dress_on_ids(20001);
	}
	if (player->dress_ids_size() == 0)
	{
		player->add_dress_ids(10001);
		player->add_dress_ids(20001);
	}
	EquipOperation::player_dress_check_all(player);

	/// 修改经验后问题修复
	s_t_exp *t_exp = sPlayerConfig->get_exp(player->level() + 1);
	if (!t_exp)
	{
		player->set_exp(0);
	}
	else if (player->exp() >= t_exp->exp)
	{
		player->set_exp(t_exp->exp - 1);
	}
	/// 修改经验后问题修复

	/// 修改VIP经验后的问题修复
	s_t_vip *t_vip = sPlayerConfig->get_vip(player->vip() + 1);
	if (t_vip)
	{
		if (player->vip_exp() > t_vip->recharge)
		{
			player_mod_vip_exp(player, 0);
		}
	}
	/// 修改VIP经验后的问题修复
	

	player_refresh_check(player);
	remove_player_chongzhi(player->guid());
	remove_player_zhichong(player->guid());
	GuildOperation::refresh_guild_offline(player->guild(), player->guid());
	sMissionPool->refresh_yb(player);
	TreasureOperation::treasure_refresh(player, true);
	SocialOperation::update_social_code(player, player->level(), true);
}

void PlayerOperation::client_login(dhc::player_t *player, bool login)
{
	// guild check
	if (player->guild() > 0)
	{
		dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
		if (!guild)
		{
			player->set_guild(0);
			player->set_last_leave_guild_time(game::timer()->now());
		}
		else
		{
			dhc::guild_member_t *member = GuildOperation::get_guild_member(guild, player->guid());
			if (!member)
			{
				player->set_guild(0);
				player->set_last_leave_guild_time(game::timer()->now());
			}
			else
			{
				member->set_bf(player->bf_max());
				member->set_player_level(player->level());	
				member->set_player_iocn_id(player->template_id());
				member->set_player_vip(player->vip());
				member->set_player_achieve(player->dress_achieves_size());
				for (int i = 0; i < player->map_star_size(); ++i)
				{
					member->set_map_star(member->map_star() + player->map_star(i));
				}
			}

		}
	}
	/// 移除无效的申请家族
	for (int i = 0; i < player->guild_applys_size();)
	{
		dhc::guild_t* guild = POOL_GET_GUILD(player->guild_applys(i));
		if (!guild)
		{
			player->mutable_guild_applys()->SwapElements(i, player->guild_applys_size() - 1);
			player->mutable_guild_applys()->RemoveLast();
		}
		else
		{
			++i;
		}
	}
	sHuodongPool->huodong_login(player);
	player->set_last_login_time(game::timer()->now());
	RankOperation::login(player);
	if (!sHuodongPool->has_jieri_huodong_flag())
	{
		int num = ItemOperation::item_num_templete(player, s_t_rewards::HUODONG_ITEM_ID1);
		if (num > 0)
			ItemOperation::item_destory_templete(player, s_t_rewards::HUODONG_ITEM_ID1, num, -1);
		num = ItemOperation::item_num_templete(player, s_t_rewards::HUODONG_ITEM_ID2);
		if (num > 0)
			ItemOperation::item_destory_templete(player, s_t_rewards::HUODONG_ITEM_ID2, num, -1);
	}

	if (login)
	{
		LOG_OUTPUT(player, LOG_LOGIN, 2, 0, 0, LOG_ADD);
	}
	else
	{
		LOG_OUTPUT(player, LOG_LOGIN, 1, 0, 0, LOG_ADD);
	}
}

void PlayerOperation::player_logout(dhc::player_t * player)
{
	sPvpList->remove_sync(player);
	sPvpList->remove_team_pull(player);
	TreasureOperation::treasure_clear(player);
}

void PlayerOperation::player_refresh_check(dhc::player_t * player)
{
	uint64_t now = game::timer()->now();
	if (game::timer()->trigger_time(player->last_daily_time(), 0, 0))
	{
		player->set_last_daily_time(now);
		player_refresh(player);
	}
	if (game::timer()->trigger_week_time(player->last_week_time()))
	{
		player->set_last_week_time(now);
		player_week_refresh(player);
	}
	if (game::timer()->trigger_month_time(player->last_month_time()))
	{
		player->set_last_month_time(now);
		player_month_refresh(player);
	}
}

void PlayerOperation::player_refresh(dhc::player_t * player)
{
	player->clear_mission_cishu_ids();
	player->clear_mission_cishu();
	player->clear_mission_goumai_ids();
	player->clear_mission_goumai();
	player->set_shop4_refresh_num(0);
	player->clear_active_id();
	player->clear_active_num();
	player->clear_active_reward();
	player->clear_shop_xg_ids();
	player->clear_shop_xg_nums();
	player->clear_shop3_ids();
	player->clear_shop3_sell();
	player->set_shoppet_num(0);

	player_add_active(player, 100, 1);
	player_add_active(player, 101, 1);
	if (player->zhouka_time() > game::timer()->now())
	{
		player_add_active(player, 200, 1);
	}
	if (player->yueka_time() > game::timer()->now())
	{
		player_add_active(player, 300, 1);
	}
	player->set_active_score(0);
	player->clear_active_score_id();

	player->set_dj_num(0);
	player->set_tili_reward(0);
	player->set_daily_sign_index(player->daily_sign_reward() + 1);
	player->set_hbb_finish_num(0);
	player->set_hbb_num(5);
	player->set_hbb_refresh_num(0);
	sRoleConfig->refresh_hhb(player);
	player->set_ttt_dead(0);
	player->set_ttt_star(0);
	player->set_ttt_can_reward(0);
	player->clear_ttt_reward_ids();
	player->clear_ttt_cur_stars();
	player->clear_ttt_cur_reward_ids();
	player->set_ttt_cz_num(0);
	player->set_random_event_num(0);
	player->set_ttt_mibao(0);
	player->set_boss_player_level(player->level());
	if (player->boss_player_level() <= 0)
	{
		player->set_boss_player_level(1);
	}
	s_t_online_reward *t_online_reward1 = sPlayerConfig->get_online_reward(0);
	if (t_online_reward1)
	{
		player->set_online_reward_time(game::timer()->now() + t_online_reward1->time);
	}
	else
	{
		player->set_online_reward_time(0);
	}
	player->set_online_reward_index(0);
	player->set_social_shou_num(0);
	player->set_yb_finish_num(0);
	player->set_yb_refresh_num(0);
	player->set_ybq_finish_num(0);
	player->set_ore_finish_num(0);
	player->set_huiyi_zhanpu_flag(0);
	player->clear_huiyi_zhanpus();
	player->set_huiyi_zhanpu_num(0);
	player->set_huiyi_gaiyun_num(0);
	player->set_pvp_refresh_num(0);
	player->set_pvp_num(15);
	player->set_pvp_buy_num(0);
	player->set_pvp_hit(0);
	player->clear_pvp_hit_ids();
	player->clear_huiyi_shop_ids();
	player->clear_huiyi_shop_nums();
	player->clear_qiyu_mission();
	player->clear_qiyu_hard();
	player->clear_qiyu_suc();
	player->set_huiyi_chou_num(0);

	player->clear_guild_honors();
	player->set_guild_buy_num(0);
	player->set_guild_num(3);
	player->clear_guild_shilians();
	player->set_guild_sign_flag(0);
	player->set_guild_red_num(0);
	player->set_guild_red_num1(0);
	player->set_guild_pvp_num(5);
	player->set_guild_pvp_buy_num(0);

	player->set_by_reward_num(3);
	player->set_by_reward_buy(0);
	player->clear_by_shops();
	player->clear_by_nums();
	player->set_ds_reward_num(15);
	player->set_ds_reward_buy(0);
	player->set_ds_hit(0);
	player->clear_ds_hit_ids();

	player->clear_huodong_kaifu_dbcz();

	TreasureOperation::treasure_refresh_luck(player);

	sHuodongPool->huodong_refresh(player);
}

void PlayerOperation::player_week_refresh(dhc::player_t * player)
{
	player->set_pvp_total(0);
	player->clear_by_rewards();
	player->set_by_point(0);
	player->set_ds_point(0);
	player->set_ds_duanwei(player->ds_duanwei() - 9);
	if (player->ds_duanwei() < 1)
	{
		player->set_ds_duanwei(1);
	}
	sPvpList->remove_team_pull(player);
	sHuodongPool->huodong_week_refresh(player);
}

void PlayerOperation::player_month_refresh(dhc::player_t * player)
{
	
}

void PlayerOperation::player_mod_exp(dhc::player_t * player, int exp)
{
	int last_level = player->level();
	int level = player->level();
	player->set_exp(player->exp() + exp);
	int nexp = player->exp();
	s_t_exp *t_exp = sPlayerConfig->get_exp(level + 1);
	if (!t_exp)
	{
		player->set_exp(0);
		return;
	}
	if (nexp < t_exp->exp)
	{
		/// 没升级
		return;
	}
	PlayerOperation::player_do_tili(player);
	while (nexp >= t_exp->exp)
	{
		level += 1;
		nexp -= t_exp->exp;
		player->set_level(level);
		int tili = get_level_tili(player);
		if (player->tili() < tili)
		{
			if (player->tili() + t_exp->tili_recover > tili)
			{
				player->set_tili(tili);
			}
			else
			{
				player->set_tili(player->tili() + t_exp->tili_recover);
			}
		}
		int energy = get_level_enegy(player);
		if (player->energy() < energy)
		{
			if (player->energy() + 10 > energy)
			{
				player->set_energy(energy);
			}
			else
			{
				player->set_energy(player->energy() + 10);
			}
		}

		/// 2级发邮件
		if (t_exp->type > 0)
		{
			std::vector<s_t_reward> rewards;
			s_t_reward reward;
			reward.type = t_exp->type;
			reward.value1 = t_exp->value1;
			reward.value2 = t_exp->value2;
			reward.value3 = t_exp->value3;
			rewards.push_back(reward);
			std::string sender;
			std::string t_exp_title;
			std::string t_exp_text;
			int lang_ver = game::channel()->get_channel_lang(player->guid());
			t_exp_title = game::scheme()->get_lang_str(lang_ver, t_exp->title);
			t_exp_text = game::scheme()->get_lang_str(lang_ver, t_exp->text);
			game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
			PostOperation::post_create(player->guid(), t_exp_title, t_exp_text, sender, rewards);
		}
		if (level == 10)
		{
			player_add_resource(player, resource::JJ_POINT, 200, LOGWAY_LEVEL);
		}
		/// 15级发宝物碎片
		if (level == 15)
		{
			ItemOperation::item_add_template(player, 60020012, 1, LOGWAY_LEVEL);
			ItemOperation::item_add_template(player, 60020013, 1, LOGWAY_LEVEL);
		}
		if (level == 70)
		{
			ItemOperation::item_add_template(player, 100011001, 25, LOGWAY_LEVEL);
		}
		if (level % 5 == 0)
		{
			//std::string text;
			//game::scheme()->get_server_str(text, "levelup", player->name().c_str(), level);
			//SocialOperation::gundong(text);			
			SocialOperation::gundong_server("t_server_language_text_levelup", player->name(), boost::lexical_cast<std::string>(level), "");
		}
		if (level > 25)
		{
			TreasureOperation::treasure_refresh(player, false);
		}
		if (level >= 30)
		{
			SocialOperation::update_social_code(player, level);
		}

		t_exp = sPlayerConfig->get_exp(level + 1);
		if (!t_exp)
		{
			nexp = 0;
			break;
		}
	}
	player->set_level(level);
	player->set_exp(nexp);
	if (level != last_level)
	{
		SportOperation::refresh(player);
	}
}

void PlayerOperation::player_mod_vip_exp(dhc::player_t * player, int exp)
{
	int vip = player->vip();
	player->set_vip_exp(player->vip_exp() + exp);
	int nexp = player->vip_exp();
	s_t_vip *t_nvip = sPlayerConfig->get_vip(vip);
	if (!t_nvip)
	{
		return;
	}
	s_t_vip *t_vip = sPlayerConfig->get_vip(vip + 1);
	if (!t_vip)
	{
		player->set_vip_exp(t_nvip->recharge);
		return;
	}
	if (nexp < t_vip->recharge)
	{
		/// 没升级
		return;
	}
	PlayerOperation::player_do_tili(player);
	while (nexp >= t_vip->recharge)
	{
		vip += 1;
		t_nvip = t_vip;
		t_vip = sPlayerConfig->get_vip(vip + 1);
		if (!t_vip)
		{
			nexp = t_nvip->recharge;
			break;
		}
	}
	player->set_vip(vip);
	player->set_vip_exp(nexp);
	player->set_huodong_vip_libao(0);

	/// 普天同庆活动
	HuodongOperation::check_pttq_vip(player);
}

void PlayerOperation::player_add_reward(dhc::player_t * player,
	s_t_rewards& strewards,
	int mode)
{
	bool has_jieri = sHuodongPool->has_jieri_huodong(player);
	std::vector<s_t_reward> role_suipian_reward;
	for (int i = 0; i < strewards.rewards.size(); ++i)
	{
		if (strewards.rewards[i].type == 1)
		{
			player_add_resource(player,
				static_cast<resource::resource_t>(strewards.rewards[i].value1),
				strewards.rewards[i].value2 + player->level() * strewards.rewards[i].value3,
				mode);
		}
		else if (strewards.rewards[i].type == 2)
		{
			if (!has_jieri && (strewards.rewards[i].value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
				strewards.rewards[i].value1 == s_t_rewards::HUODONG_ITEM_ID2))
			{
				// 仍可道具兑换
				if (mode == LOGWAY_HUODONG_DJDH)
				{
					ItemOperation::item_add_template(player, strewards.rewards[i].value1, strewards.rewards[i].value2, mode);
				}
			}
			else
			{
				ItemOperation::item_add_template(player, strewards.rewards[i].value1, strewards.rewards[i].value2, mode);
			}
		}
		else if (strewards.rewards[i].type == 3)
		{
			if (RoleOperation::has_t_role(player, strewards.rewards[i].value1))
			{
				s_t_item* _item = sItemConfig->get_item(sItemConfig->get_suipian(strewards.rewards[i].value1));
				if (_item != NULL)
				{
					ItemOperation::item_add_template(player, _item->id, _item->def2, mode);
					role_suipian_reward.push_back(s_t_reward(2, _item->id, _item->def2));
				}
			}
			else
			{
				dhc::role_t *role = RoleOperation::role_create(player,
					strewards.rewards[i].value1,
					strewards.rewards[i].value2,
					strewards.rewards[i].value3,
					mode);
				if (role)
				{
					strewards.roles.push_back(role);
					if (mode == LOGWAY_RECHARGE)
					{
						strewards.guids.push_back(role->guid());
					}
				}
			}

		}
		else if (strewards.rewards[i].type == 4)
		{
			dhc::equip_t *equip = EquipOperation::equip_create(player,
				strewards.rewards[i].value1,
				strewards.rewards[i].value2,
				strewards.rewards[i].value3,
				mode);
			if (equip)
			{
				strewards.equips.push_back(equip);
				if (mode == LOGWAY_RECHARGE)
				{
					strewards.guids.push_back(equip->guid());
				}
			}
		}
		else if (strewards.rewards[i].type == 5)
		{
			RoleOperation::role_dress_get(player, strewards.rewards[i].value1);
		}
		else if (strewards.rewards[i].type == 6)
		{
			dhc::treasure_t *treasure = TreasureOperation::treasure_create(player,
				strewards.rewards[i].value1,
				strewards.rewards[i].value2,
				strewards.rewards[i].value3,
				mode);
			if (treasure)
			{
				strewards.treasures.push_back(treasure);
				if (mode == LOGWAY_RECHARGE)
				{
					strewards.guids.push_back(treasure->guid());
				}
			}
		}
		else if (strewards.rewards[i].type == 7)
		{
			RoleOperation::player_guanghuan_get(player, strewards.rewards[i].value1);
		}
		else if (strewards.rewards[i].type == 8)
		{
			PlayerOperation::player_add_chenghao(player, strewards.rewards[i].value1);
		}
		else if (strewards.rewards[i].type == 9)
		{
			if (!EquipOperation::player_has_dress(player, strewards.rewards[i].value1))
			{
				player->add_dress_ids(strewards.rewards[i].value1);
			}
		}
		else if (strewards.rewards[i].type == 10)
		{

		}
		else if (strewards.rewards[i].type == 11)
		{
			dhc::pet_t *pet = RoleOperation::create_pet(player,
				strewards.rewards[i].value1,
				mode);
			if (pet)
			{
				strewards.pets.push_back(pet);
				if (mode == LOGWAY_RECHARGE)
				{
					strewards.guids.push_back(pet->guid());
				}
			}
			/*if (RoleOperation::has_pet(player, strewards.rewards[i].value1))
			{
				const s_t_pet* t_pet = sRoleConfig->get_pet(strewards.rewards[i].value1);
				if (t_pet)
				{
					const s_t_item *t_item = sItemConfig->get_item(t_pet->suipian_id);
					if (t_item)
					{
						ItemOperation::item_add_template(player, t_item->id, t_item->def2, mode);
						role_suipian_reward.push_back(s_t_reward(2, t_item->id, t_item->def2));
					}
				}
			}
			else
			{
				dhc::pet_t *pet = RoleOperation::create_pet(player,
					strewards.rewards[i].value1,
					mode);
				if (pet)
				{
					strewards.pets.push_back(pet);
				}
			}*/
		}
	}
	if (!role_suipian_reward.empty())
	{
		strewards.add_reward(role_suipian_reward);
	}
}

void PlayerOperation::player_add_resource(dhc::player_t *player,
	resource::resource_t type,
	int value,
	int mode)
{
	if (value == 0)
	{
		return;
	}

	switch (type)
	{
	case resource::GOLD:
		player->set_gold(player->gold() + value);
		if (player->gold() < 0)
			player->set_gold(0);
		break;
	case resource::JEWEL:
		player->set_jewel(player->jewel() + value);
		if (player->jewel() < 0)
			player->set_jewel(0);
		break;
	case resource::TILI:
		PlayerOperation::player_do_tili(player);
		player->set_tili(player->tili() + value);
		if (player->tili() < 0)
			player->set_tili(0);
		break;
	case resource::EXP:
		player_mod_exp(player, value);
		break;
	case resource::ZHANHUN:
		player->set_jjc_point(player->jjc_point() + value);
		if (player->jjc_point() < 0)
			player->set_jjc_point(0);
		break;
	case resource::HEJIN:
		player->set_mw_point(player->mw_point() + value);
		if (player->mw_point() < 0)
			player->set_mw_point(0);
		break;
	case resource::YUANLI:
		player->set_yuanli(player->yuanli() + value);
		if (player->yuanli() < 0)
			player->set_yuanli(0);
		break;
	case resource::ACTIVE:
		player->set_active_score(player->active_score() + value);
		if (player->active_score() < 0)
			player->set_active_score(0);
		break;
	case resource::VIP_EXP:
		player_mod_vip_exp(player, value);
		break;
	case resource::CONTRIBUTION:
		player->set_contribution(player->contribution() + value);
		if (player->contribution() < 0)
			player->set_contribution(0);
		break;
	case resource::MW_MEDAL:
		player->set_medal_point(player->medal_point() + value);
		if (player->medal_point() < 0)
			player->set_medal_point(0);
		break;
	case resource::JJ_POINT:
		player->set_powder(player->powder() + value);
		if (player->powder() < 0)
			player->set_powder(0);
		break;
	case resource::ENERGY:
		PlayerOperation::player_do_energy(player);
		player->set_energy(player->energy() + value);
		if (player->energy() < 0)
			player->set_energy(0);
		break;
	case resource::MW_COUNT:
		PlayerOperation::player_do_boss(player);
		player->set_boss_num(player->boss_num() + value);
		if (player->boss_num() < 0)
			player->set_boss_num(0);
		break;
	case resource::DRESS_TUZHI:
		player->set_dress_tuzhi(player->dress_tuzhi() + value);
		if (player->dress_tuzhi() < 0)
			player->set_dress_tuzhi(0);
		break;
	case resource::LUCK_POINT:
		player->set_luck_point(player->luck_point() + value);
		if (player->luck_point() < 0)
			player->set_luck_point(0);
		break;
	case resource::HUIYI_POINT:
		player->set_huiyi_point(player->huiyi_point() + value);
		if (player->huiyi_point() < 0)
			player->set_huiyi_point(0);
		break;
	case resource::LIEREN_POINT:
		player->set_lieren_point(player->lieren_point() + value);
		if (player->lieren_point() < 0)
			player->set_lieren_point(0);
		break;
	case resource::BINGJING:
		player->set_bingjing(player->bingjing() + value);
		if (player->bingjing() < 0)
			player->set_bingjing(0);
		break;
	case resource::XINPIAN:
		player->set_xinpian(player->xinpian() + value);
		if (player->xinpian() < 0)
			player->set_xinpian(0);
		break;
	case resource::YOUQINGDIAN:
		player->set_youqingdian(player->youqingdian() + value);
		if (player->youqingdian() < 0)
		{
			player->set_youqingdian(0);
		}
		break;
	default:
		break;
	}

	if (mode != -1)
	{
		if (type == resource::GOLD ||
			type == resource::JEWEL ||
			type == resource::ZHANHUN ||
			type == resource::HEJIN ||
			type == resource::YUANLI ||
			type == resource::CONTRIBUTION ||
			type == resource::MW_MEDAL ||
			type == resource::JJ_POINT ||
			type == resource::LUCK_POINT ||
			type == resource::HUIYI_POINT ||
			type == resource::LIEREN_POINT ||
			type == resource::BINGJING ||
			type == resource::XINPIAN)
		{
			if (value > 0)
			{
				LOG_OUTPUT(player, LOG_RESOURCE, type, value, mode, LOG_ADD);
			}
			else
			{
				LOG_OUTPUT(player, LOG_RESOURCE, type, -value, mode, LOG_DEC);
			}
		}
	}
}

void PlayerOperation::player_dec_resource(dhc::player_t *player, 
	resource::resource_t type, 
	int value, 
	int mode)
{
	if (type == resource::EXP ||
		type == resource::VIP_EXP)
	{
		return;
	}

	PlayerOperation::player_add_resource(player, type, -value, mode);

	if (type == resource::JEWEL)
	{
		player->set_total_spend(player->total_spend() + value);
	}
}

void PlayerOperation::player_recharge_log(dhc::player_t *player, int rmb, int rid, bool success)
{
	if (success)
	{
		LOG_OUTPUT(player, LOG_LOGIN, 100, rmb, rid, LOG_ADD);
	}
	else
	{
		LOG_OUTPUT(player, LOG_LOGIN, 101, rmb, rid, LOG_ADD);
	}
		
}

int PlayerOperation::get_max_tili(dhc::player_t *player)
{
	s_t_exp *t_exp = sPlayerConfig->get_exp(player->level());
	if (!t_exp)
	{
		return 0;
	}
	s_t_vip *t_vip = sPlayerConfig->get_vip(player->vip());
	if (!t_vip)
	{
		return 0;
	}
	return t_exp->tili + t_vip->add_tili;
}

int PlayerOperation::get_level_tili(dhc::player_t *player)
{
	if (player->level() < 30)
	{
		return get_max_tili(player);
	}
	else if (player->level() < 50)
	{
		return 200;
	}
	else
	{
		return 500;
	}
}

int PlayerOperation::get_level_enegy(dhc::player_t *player)
{
	if (player->level() < 30)
	{
		return 30;
	}
	else if (player->level() < 50)
	{
		return 60;
	}
	else
	{
		return 150;
	}
}

void PlayerOperation::player_do_tili(dhc::player_t *player)
{
	uint64_t now = game::timer()->now();
	if (player->tili() >= get_max_tili(player))
	{
		player->set_last_tili_time(now);
		return;
	}
	uint64_t dtime = now - player->last_tili_time();
	if (dtime > TILI_TIME)
	{
		uint64_t num = dtime / TILI_TIME;
		if (num > get_max_tili(player) - player->tili())
		{
			player->set_tili(get_max_tili(player));
			player->set_last_tili_time(now);
		}
		else
		{
			player->set_tili(player->tili() + num);
			player->set_last_tili_time(player->last_tili_time() + num * TILI_TIME);
		}
	}
}

void PlayerOperation::player_do_energy(dhc::player_t *player)
{
	uint64_t now = game::timer()->now();
	if (player->energy() >= 30)
	{
		player->set_last_energy_time(now);
		return;
	}

	uint64_t dtime = now - player->last_energy_time();
	if (dtime > ENERGY_TIME)
	{
		uint64_t num = dtime / ENERGY_TIME;
		if (num + player->energy() > 30)
		{
			player->set_energy(30);
			player->set_last_energy_time(now);
		}
		else
		{
			player->set_energy(player->energy() + num);
			player->set_last_energy_time(player->last_energy_time() + num * ENERGY_TIME);
		}
	}
}


void PlayerOperation::player_do_boss(dhc::player_t *player)
{
	uint64_t now = game::timer()->now();
	if (player->boss_num() >= 10)
	{
		player->set_boss_last_time(now);
		return;
	}

	uint64_t dtime = now - player->boss_last_time();
	if (dtime > BOSS_RETIME)
	{
		uint64_t num = dtime / BOSS_RETIME;
		if (num + player->boss_num() > 10)
		{
			player->set_boss_num(10);
			player->set_boss_last_time(now);
		}
		else
		{
			player->set_boss_num(player->boss_num() + num);
			player->set_boss_last_time(player->boss_last_time() + num * BOSS_RETIME);
		}
	}
}

void PlayerOperation::player_add_active(dhc::player_t *player, int id, int num)
{
	s_t_active *t_active = sPlayerConfig->get_active(id);
	if (!t_active)
	{
		return;
	}
	bool flag = false;
	for (int i = 0; i < player->active_id_size(); ++i)
	{
		if (player->active_id(i) == t_active->id)
		{
			flag = true;
			player->set_active_num(i, player->active_num(i) + num);
			if (player->active_num(i) > t_active->num)
			{
				player->set_active_num(i, t_active->num);
				break;
			}
		}
	}
	if (!flag)
	{
		player->add_active_id(t_active->id);
		player->add_active_reward(0);
		if (num > t_active->num)
		{
			player->add_active_num(t_active->num);
		}
		else
		{
			player->add_active_num(num);
		}
	}
}

int PlayerOperation::player_calc_force(dhc::player_t *player)
{
	double force = 0;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (!role)
		{
			continue;
		}
		std::map<int, double> role_attrs;
		RoleOperation::get_role_attr(player, role, role_attrs);
		/// 百分比计算
		for (int j = 1; j <= 5; ++j)
		{
			role_attrs[j] = role_attrs[j] * (1 + role_attrs[j + 5] * 0.01f);
		}

		/// 总计
		int _skill_level = 0;

		for (int i = 0; i < role->jskill_level_size(); i++)
		{
			_skill_level += role->jskill_level(i);
		}

		force += (role_attrs[1] * 0.25 + role_attrs[2] * 2 + role_attrs[3] * 5 + role_attrs[4] * 5 + role_attrs[5] * 5)
			* (1 + role_attrs[11] * 0.003f + role_attrs[12] * 0.003f + role_attrs[14] * 0.003f + role_attrs[15] * 0.003f
			+ role_attrs[16] * 0.003f + role_attrs[17] * 0.003f	+ role_attrs[18] * 0.003f + role_attrs[19] * 0.003f 
			+ role_attrs[20] * 0.003f + role_attrs[21] * 0.003f + role_attrs[22] * 0.05f + role_attrs[23] * 0.0003f
			+ role_attrs[24] * 0.0003f + role_attrs[25] * 0.0003f + role_attrs[26] * 0.0003f + role_attrs[27] * 0.0003f
			+ role_attrs[28] * 0.0003f) * (1 + _skill_level * 0.002f);
	}
	player->set_bf(force);
	if (player->bf() <= 0)
	{
		player->set_bf(2100000000);
	}
	if (player->bf() > player->bf_max())
	{
		player->set_bf_max(player->bf());
	}
	return force;
}

int PlayerOperation::player_recharge(dhc::player_t *player, int rid, int count)
{
	s_t_recharge *t_recharge = sPlayerConfig->get_recharge(rid);
	if (!t_recharge)
	{
		return -1;
	}

	uint64_t now = game::timer()->now();
	if (t_recharge->type == 3)
	{
		bool flag = false;
		for (int i = 0; i < player->recharge_ids_size(); ++i)
		{
			if (player->recharge_ids(i) == t_recharge->id)
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			t_recharge = sPlayerConfig->get_recharge(t_recharge->pid);
			if (!t_recharge)
			{
				return -1;
			}
		}
	}

	if (t_recharge->type == 1)
	{
		if (now < player->zhouka_time())
		{
			player->set_zhouka_time(player->zhouka_time() + (uint64_t)86400000 * 30);
		}
		else
		{
			player->set_zhouka_time(now + (uint64_t)86400000 * 30);
		}
		PlayerOperation::player_add_active(player, 200, 1);
	}
	else if (t_recharge->type == 2)
	{
		if (now < player->yueka_time())
		{
			player->set_yueka_time(player->yueka_time() + (uint64_t)86400000 * 30);
		}
		else
		{
			player->set_yueka_time(now + (uint64_t)86400000 * 30);
		}
		PlayerOperation::player_add_active(player, 300, 1);
	}
	else if (t_recharge->type == 3)
	{
		player->add_recharge_ids(t_recharge->id);
	}
	int jewel = t_recharge->jewel;
	int vippt = t_recharge->vippt;
	
	PlayerOperation::player_add_resource(player, resource::JEWEL, jewel, LOGWAY_RECHARGE);
	player->set_total_recharge(player->total_recharge() + vippt);
	sHuodongPool->huodong_recharge(player, vippt, t_recharge->ios_id, 0);
	PlayerOperation::player_mod_vip_exp(player, vippt);
	PlayerOperation::player_recharge_log(player, vippt, player->first_reward(), true);
	if (player->first_reward() == 0)
	{
		player->set_first_reward(1);
	}
	return 0;
}

int PlayerOperation::player_recharge_zc(dhc::player_t *player, int rid, int count)
{
	s_t_recharge *t_recharge = sPlayerConfig->get_recharge(rid);
	if (!t_recharge)
	{
		return -1;
	}

	int jewel = t_recharge->jewel;
	int vippt = t_recharge->vippt;
	player->set_total_recharge(player->total_recharge() + vippt);
	sHuodongPool->huodong_recharge(player, vippt, t_recharge->ios_id, 1);
	PlayerOperation::player_recharge_log(player, vippt, player->first_reward(), true);
	if (player->first_reward() == 0)
	{
		player->set_first_reward(1);
	}
	return 0;
}

void PlayerOperation::player_huodong(dhc::player_t *player, int id, int val)
{
	LOG_OUTPUT(player, LOG_HUODONG, id, val, 0, 0);
}

void PlayerOperation::player_statistics(dhc::player_t *player, int id, int val)
{
	LOG_OUTPUT(player, LOG_STATISTICS, id, val, 0, 0);
}

int PlayerOperation::get_shop_refresh_flag(dhc::player_t *player)
{
	if (player_shop_refresh_.find(player->guid()) != player_shop_refresh_.end())
	{
		player_shop_refresh_.erase(player->guid());
		return 1;
	}
	return 0;
}

void PlayerOperation::set_shop_refresh_flag(dhc::player_t *player)
{
	player_shop_refresh_.insert(player->guid());
}

bool PlayerOperation::check_fight_time(dhc::player_t *player)
{
	if (player->last_fight_time() + 5000 > game::timer()->now())
	{
		return false;
	}
	return true;
}

void PlayerOperation::refresh_player_name_nalflag(dhc::player_t* player, const std::string& name, const int& nalflag)
{
	/// 排行榜
	RankOperation::refresh(player);

	/// 竞技场
	SportOperation::refresh(player);

	/// 抢夺
	TreasureOperation::treasure_refresh(player, false);

	/// 好友
	SocialOperation::refresh_name_nalflag(player, name, nalflag);
	/// 军团副本排行榜
	GuildOperation::refresh_guild_mission_name_nalflag(player, name, nalflag);
}


void PlayerOperation::player_add_chenghao(dhc::player_t* player, int chenghao)
{
	const s_t_chenghao *old_chenghao = 0;
	for (int i = 0; i < player->chenghao_size(); ++i)
	{
		old_chenghao = sPlayerConfig->get_chenghao(player->chenghao(i));
		if (!old_chenghao)
		{
			continue;
		}

		if (player->chenghao(i) == chenghao)
		{
			if (old_chenghao->day > 0)
			{
				player->set_chengchao_time(i, game::timer()->now() + old_chenghao->day * 24 * 60 * 60 * 1000);
			}
			return;
		}
	}

	const s_t_chenghao *t_chenghao = sPlayerConfig->get_chenghao(chenghao);
	if (!t_chenghao)
	{
		return;
	}

	if (t_chenghao->type == 1)
	{
		const s_t_chenghao *t_bingyuan_chenghao = 0;
		for (int i = 0; i < player->chenghao_size(); ++i)
		{
			t_bingyuan_chenghao = sPlayerConfig->get_chenghao(player->chenghao(i));
			if (t_bingyuan_chenghao && t_bingyuan_chenghao->type == 1)
			{
				player->set_chenghao(i, chenghao);
				return;
			}
		}
	}

	player->add_chenghao(chenghao);
	if (t_chenghao->day != 0)
	{
		player->add_chengchao_time(game::timer()->now() + t_chenghao->day * 24 * 60 * 60 * 1000);
	}
	else
	{
		player->add_chengchao_time(0);
	}
}

void PlayerOperation::player_check_chenghao(dhc::player_t *player)
{
	for (int i = 0; i < player->chenghao_size();)
	{
		if (player->chengchao_time(i) > 0 &&
			game::timer()->now() > player->chengchao_time(i))
		{
			if (player->chenghao_on() == player->chenghao(i))
			{
				player->set_chenghao_on(0);
			}

			player->mutable_chenghao()->SwapElements(i, player->chenghao_size() - 1);
			player->mutable_chenghao()->RemoveLast();

			player->mutable_chengchao_time()->SwapElements(i, player->chengchao_time_size() - 1);
			player->mutable_chengchao_time()->RemoveLast();
			
			continue;
		}
		++i;
	}
}

int PlayerOperation::get_bingyuan_chenghao(dhc::player_t *player)
{
	if (player->level() <= 70)
	{
		return 0;
	}

	if (player->by_point() < 500)
	{
		return 0;
	}

	const s_t_chenghao *t_ch = 0;
	int index = -1;
	for (int i = 0; i < player->chenghao_size(); ++i)
	{
		t_ch = sPlayerConfig->get_chenghao(player->chenghao(i));
		if (t_ch && t_ch->type == 1)
		{
			return sPvpConfig->get_bingyuan_chenghao_by_chenghao(t_ch->id);
		}
	}
	return 0;
}
