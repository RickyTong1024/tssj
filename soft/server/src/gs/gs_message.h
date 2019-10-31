#ifndef __GS_MESSAGE_H__
#define __GS_MESSAGE_H__

#include "gameinc.h"

struct GuildBossGuai;

class ResMessage
{
public:
	static void res_last(const std::string &pck, const std::string &name, int id);

	static void res_client_login(dhc::player_t *player, int is_new, const std::string &name, int id);

	static void res_success(dhc::player_t *player, bool suc, const std::string &name, int id);

	static void res_success_ex(bool suc, const std::string &name, int id);

	static void res_player_name(dhc::player_t *player, int suc, const std::string &name, int id);

	//static void res_success_googlebind(dhc::player_t *player, const protocol::game::smsg_player_name& msg, const std::string &name, int id);

	static void res_chat(const std::string &name, int id);

	static void res_mission_fight_end(dhc::player_t *player, int result, const std::string &text, int star, const s_t_rewards& rds, const std::string &name, int id);

	static void res_mission_saodang(dhc::player_t *player, const std::vector<s_t_rewards> &rewards, const std::string &name, int id);

	static void res_mission_first(dhc::player_t *player, const protocol::game::smsg_mission_first& msg, const std::string& name, int id);

	static void res_chouka(dhc::player_t *player, const protocol::game::smsg_chouka& msg, const std::string &name, int id);

	static void res_role_duihuan(dhc::player_t *player, dhc::role_t *role, const std::string &name, int id);

	static void res_role_zhanpu(dhc::player_t* player, const std::string &name, int id);

	static void res_pet_duihuan(dhc::player_t *player, dhc::pet_t *pet, const std::string &name, int id);

	static void res_role_xq_look(dhc::player_t *player, const std::vector<uint64_t> &guids, const std::vector<int> &xqs, const std::string &name, int id);
	static void res_role_yh_look(dhc::player_t *player, const std::string &name, int id);
	static void res_role_yh_select(dhc::player_t *player, uint64_t guid, int index, int jewel, const std::string &name, int id);

	static void res_role_huiyi_chou(dhc::player_t* player, const protocol::game::smsg_role_huiyi_chou& msg, const std::string& name, int id);

	static void res_role_huiyi_rank(dhc::player_t* player, const protocol::game::smsg_role_huiyi_rank& msg, const std::string& name, int id);

	static void res_role_init(dhc::player_t *player, const protocol::game::smsg_role_init& msg, const std::string &name, int id);

	static void res_pet_init(dhc::player_t *player, const protocol::game::smsg_pet_init& msg, const std::string &name, int id);

	static void res_item_apply(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_shop_refresh(dhc::player_t *player, const std::string &name, int id);

	static void res_shop_buy(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_post_look(dhc::player_t *player, const std::vector<dhc::post_t *> posts, const std::string &name, int id);

	static void res_post_get(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_active_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_active_score_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_post_view(dhc::player_t *player, uint64_t post_guid, const std::string &name, int id);

	static void res_equip_gaizao(dhc::player_t *player, dhc::equip_t *equip, int num, const std::string &name, int id);

	static void res_sport_look(dhc::player_t *player, const std::vector<protocol::game::msg_sport_player> &players
		, int rank, int last_rank, int can_get, const std::string &name, int id);

	static void res_sport_top(dhc::player_t *player, const std::vector<protocol::game::msg_sport_player> &players, const std::string &name, int id);

	static void res_sport_shop_list(dhc::player_t *player, const std::string &name, int id);

	static void res_sport_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, int cid, int pgold, const std::string &name, int id);

	static void res_sport_saodang(dhc::player_t*player, const protocol::game::smsg_sport_saodang& msg, const std::string& name, int id);

	static void res_sport_shop_buy(dhc::player_t *player, const std::vector<dhc::equip_t *> &equips, const std::vector<dhc::treasure_t*> &treasures, const std::string &name, int id);

	static void res_vip_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_gm_command(dhc::player_t *player, int type, int value1, int value2, int value3, const s_t_rewards& rds, const std::string &name, int id);

	static void res_dj_reward(dhc::player_t *player, int bs, int gold, const std::string &name, int id);

	static void res_boss_look(dhc::player_t *player, const protocol::game::smsg_boss_look& msg, const std::string &name, int id);

	static void res_boss_look_ex(dhc::player_t *player, const protocol::game::smsg_boss_look& msg, const std::string &name, int id);

	static void res_boss_rank(dhc::player_t *player, const protocol::game::smsg_boss_rank& msg, const std::string &name, int id);

	static void res_boss_active_look(dhc::player_t *player, const protocol::game::smsg_boss_active_look& msg, const std::string& name, int id);

	static void res_boss_fight_end(dhc::player_t *player, const protocol::game::smsg_boss_fight_end& msg, const std::string &name, int id);

	static void res_boss_fight_saodang(dhc::player_t *player, const protocol::game::smsg_boss_saodang& msg, const std::string &name, int id);

	static void res_first_recharge(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_online_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_daily_sign(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_hbb_look(dhc::player_t *player, const std::string &name, int id);

	static void res_hbb_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, const std::string &name, int id);

	static void res_player_look(dhc::player_t *player, dhc::player_t *target, const std::vector<dhc::role_t *> &roles, const std::vector<dhc::equip_t *> &equips, const std::vector<dhc::treasure_t*> &treasures, const std::vector<int>& role_sxs, const std::vector<dhc::pet_t*> &pets, const std::vector<int> &pet_sxs, const std::string &name, int id);

	static void res_random_event_look(dhc::player_t *player, int32_t event_id, const std::string &name, int id);

	static void res_random_event_get(dhc::player_t *player, uint64_t random_event_time, const std::string &name, int id);

	static void res_guild_data(dhc::player_t *player, dhc::guild_t *guild, int32_t zhiwu, bool mobai, bool fight, int msg_count, bool apply, bool hongbao, bool guildpvp, const std::string &name, int id);

	static void res_guild_list_recommend(dhc::player_t *player, std::vector<dhc::guild_t*> &guild_list, const std::string &name, int id);

	static void res_guild_member_view(dhc::player_t *player, std::list<dhc::guild_member_t*> &member_list, const std::string &name, int id);

	static void  res_guild_ranking(dhc::player_t *player, std::list<dhc::guild_t*> &guild_list, const std::string &name, int id);

	static void res_guild_mission_ranking(dhc::player_t *player, const protocol::game::smsg_guild_mission_ranking& msg, const std::string& name, int id);

	static void res_guild_activity(dhc::player_t *player, const protocol::game::smsg_guild_activity &msg, std::string name, int id);

	static void res_guild_boss_look(dhc::player_t *player, const protocol::game::smsg_guild_mission_look&msg, const std::string &name, int id);

	static void res_guild_boss_look_ex(dhc::player_t *player, const protocol::game::smsg_guild_mission_look&msg, const std::string &name, int id);

	static void res_guild_boss_fight_end(dhc::player_t *player, int result, const std::string& text, int contri, int hit_contri, int hit, const std::string &name, int id);

	static void res_guild_message_view(dhc::player_t *player, const protocol::game::smsg_guild_message_view& msg, const std::string &name, int id);

	static void res_guild_red_deliver(dhc::player_t *player, const protocol::game::smsg_guild_red_deliver& msg, const std::string &name, int id);

	static void res_guild_red_view(dhc::player_t *player, const protocol::game::smsg_guild_red_view& msg, const std::string &name, int id);

	static void res_guild_red_rob(dhc::player_t *player, int jewel, const std::string& name, int id);

	static void res_guild_mission_complete_reward(dhc::player_t *player, int reward_index, const std::string& name, int id);

	static void res_guild_mission_complete_reward_view(dhc::player_t *player, const protocol::game::smsg_guild_mission_complete_reward_view& msg, const std::string& name, int id);

	static void res_guild_fight_look(dhc::player_t *player, const protocol::game::smsg_guild_fight_pvp_look& msg, const std::string &name, int id);

	static void res_equip_suipian(dhc::player_t *player, dhc::equip_t *equip, const std::string &name, int id);

	static void res_equip_init(dhc::player_t *player, const protocol::game::smsg_equip_init &msg, const std::string &name, int id);

	static void res_rank_data(dhc::player_t *player, dhc::rank_t *rank, const std::string &name, int id);

	static void res_social_look(dhc::player_t *player, const std::vector<dhc::social_t *> &socials, const std::string &name, int id);

	static void res_social_rand(dhc::player_t *player, const std::vector<dhc::player_t *> &players, const std::string &name, int id);

	static void res_social_add(dhc::player_t *player, uint64_t target_guid, const std::string &name, int id);

	static void res_social_look_new(dhc::player_t *player, const std::vector<protocol::game::msg_social_player *> social_players, const std::string &name, int id);

	static void res_social_agree(dhc::player_t *player, uint64_t target_guid, int agree, const std::string &name, int id);

	static void res_social_delete(dhc::player_t *player, uint64_t social_guid, const std::string &name, int id);

	static void res_social_song(dhc::player_t *player, uint64_t social_guid, const std::string &name, int id);

	static void res_social_shou(dhc::player_t *player, const std::vector<uint64_t> &social_guid, const std::string &name, int id);

	static void res_player_check(dhc::player_t *player, const protocol::game::smsg_player_check& msg, const std::string &name, int id);

	static void res_role_skillup(dhc::player_t *player, int exp, int num, const std::string &name, int id);

	static void res_libao(dhc::player_t *player, const s_t_rewards& rds, int chongzhi, const std::string &name, int id);

	static void res_ttt_fight_end(dhc::player_t *player, int result, const std::string& text, int index, int nd, int mibao, int baoji1, int baoji2, const s_t_rewards& rds, const std::string &name, int id);

	static void res_ttt_sanxing(dhc::player_t *player, const protocol::game::smsg_ttt_sanxing& msg, const std::string& name, int id);

	static void res_ttt_value_look(dhc::player_t *player, const std::string &name, int id);

	static void res_xjbz_get(dhc::player_t *player, int gold, int baoshi, int zz, const std::string &name, int id);

	static void res_global_view(dhc::player_t *player, dhc::global_t *global, const std::string &name, int id);

	static void res_huodong_kaifu_look(dhc::player_t *player, const std::vector<int> &ids, const std::vector<int> &counts, const std::string &name, int id);

	static void res_huodong_kaifu_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_huodong_view(dhc::player_t *player, const protocol::game::smsg_huodong_view& msg, const std::string &name, int id);

	static void res_huodong_tanbao_view(dhc::player_t *player, const protocol::game::smsg_huodong_tanbao_view& msg, const std::string &name, int id);

	static void res_huodong_tanbao_dice(dhc::player_t *player, const protocol::game::smsg_tanbao_dice& msg, const std::string &name, int id);

	static void res_huodong_fanpai_view(dhc::player_t *player, const protocol::game::smsg_huodong_fanpai_view& msg, const std::string &name, int id);

	static void res_huodong_fanpai(dhc::player_t *player, int fan_id, const std::string &name, int id);

	static void res_huodong_tansuo_view(dhc::player_t *player, const protocol::game::smsg_huodong_tansuo_view& msg, const std::string &name, int id);

	static void res_huodong_tansuo(dhc::player_t *player, const protocol::game::smsg_huodong_tansuo &msg, const std::string &name, int id);

	static void res_huodong_tansuo_event(dhc::player_t *player, const protocol::game::smsg_huodong_tansuo_event &msg, const std::string &name, int id);

	static void res_huodong_tansuo_event_refresh(dhc::player_t *player, int qiyu_id, const std::string &name, int id);

	static void res_huodong_zhuanpan_view(dhc::player_t *player, const protocol::game::smsg_huodong_zhuanpan_view& msg, const std::string &name, int id);

	static void res_huodong_zhuanpan(dhc::player_t *player, const protocol::game::smsg_huodong_zhuan& msg, const std::string& name, int id);

	static void res_huodong_jiri(dhc::player_t* player, const protocol::game::smsg_huodong_jiri_view& msg, const std::string &name, int id);

	static void res_huodong_reward_view(dhc::player_t *player, const protocol::game::smsg_huodong_reward_view& msg, const std::string &name, int id);

	static void res_huodong_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id);

	static void res_huodong_zc_reward(dhc::player_t * player, const s_t_rewards & rds, int type, const std::string & name, int id);

	static void res_pvp_refresh(dhc::player_t* player, const protocol::game::smsg_pvp_view& msg, const std::string& name, int id);

	static void res_pvp_fight_end(dhc::player_t*player, const protocol::game::smsg_pvp_fight_end& msg, const std::string& name, int id);

	static void res_recharge_check_ex(int type, const int & rdz_ids, const int & rdz_counts, const s_t_rewards& rds, const std::string & name, int id);

	static void res_treasure_jinlian(dhc::player_t *player, const std::vector<uint64_t> &treasures, const std::string &name, int id);

	static void res_treasure_hecheng(dhc::player_t *player, dhc::treasure_t *treasure, const std::string &name, int id);

	static void res_treasure_star(dhc::player_t *player, const protocol::game::smsg_treasure_star &msg, const std::string &name, int id);

	static void res_treasure_init(dhc::player_t *player, const protocol::game::smsg_treasure_init &msg, const std::string &name, int id);

	static void res_treasure_view(dhc::player_t *player, const protocol::game::smsg_treasure_rob_view &msg, const std::string &name, int id);

	static void res_treasure_protect(dhc::player_t *player, const std::string &name, int id);

	static void res_treasure_fight(dhc::player_t *player, const protocol::game::smsg_treasure_fight &msg, const std::string &name, int id);

	static void res_treasure_fight_end(dhc::player_t *player, const s_t_rewards& rds, int suipian_id, int result, int card, int pgold, const std::string &text, const std::string &name, int id);

	static void res_treasure_saodang(dhc::player_t *player, const protocol::game::smsg_treasure_saodang &msg, const std::string &name, int id);

	static void res_treasure_yijian_saodang(dhc::player_t *player, const protocol::game::smsg_treasure_yijian_saodang &msg, const std::string &name, int id);

	static void res_treasure_point(dhc::player_t *player, int has, const std::string &name, int id);

	static void res_treasure_report(dhc::player_t *player, const std::vector<int> &suipian_ids, const std::string &name, int id);

	static void res_treasure_report_ex(dhc::player_t *player, const std::vector<int> &suipian_ids, const std::string &name, int id);

	static void res_treasure_zhuzao(dhc::player_t *player, int level, int exp, const std::string &name, int id);

	static void res_yb_refresh(dhc::player_t *player, int type, const std::string &name, int id);

	static void res_yb_look(dhc::player_t *player, const std::vector<protocol::game::msg_yb_player> &yb_players
		, const std::list<protocol::game::msg_yb_info> &yb_infos, const std::list<protocol::game::msg_ybq_info> &ybq_infos
		, int player_id, const std::string &name, int id);

	static void res_yb_look_ex(dhc::player_t *player, const std::vector<protocol::game::msg_yb_player> &yb_players
		, const std::list<protocol::game::msg_yb_info> &yb_infos, const std::list<protocol::game::msg_ybq_info> &ybq_infos
		, int player_id, const std::string &name, int id);

	static void res_yb_ybq_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, const std::string &name, int id);

	static void res_yb_reward(dhc::player_t *player, int yuanli, const std::string &name, int id);

	static void res_ore_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, int hp, const std::string &name, int id);

	static void res_qiyu_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, const std::string &name, int id);

	static void res_qiyu_check(dhc::player_t *player, const std::string &name, int id);

	static void res_social_invite_look(dhc::player_t *player, const protocol::game::smsg_team_friend_view& msg, const std::string &name, int id);

	static void res_bingyuan_fight_end(dhc::player_t *player, const protocol::game::smsg_bingyuan_fight_end &msg, const std::string &name, int id);

	static void res_qiechuo(dhc::player_t *player, const protocol::game::smsg_qiecuo &msg, const std::string &name, int id);

	static void res_ds_fight_end(dhc::player_t *player, const protocol::game::smsg_ds_fight_end &msg, const std::string &name, int id);

	static void res_ds_time_buy(dhc::player_t *player, const protocol::game::smsg_huodong_fanpai &msg, const std::string &name, int id);

	static void res_huodong_mofang_refresh(dhc::player_t *player, const protocol::game::smsg_huodong_mofang_refresh &msg, const std::string &name, int id);

	static void res_huodong_mofang_view(dhc::player_t *player, const protocol::game::smsg_huodong_mofang_view &msg, const std::string &name, int id);

	static void res_huodong_mofang(dhc::player_t *player, const protocol::game::smsg_huodong_mofang &msg, const std::string &name, int id);

	static void res_huodong_yueka_view(dhc::player_t *player, const protocol::game::smsg_huodong_yueka_view &msg, const std::string &name, int id);

	static void res_guild_pvp_look(dhc::player_t *player, const protocol::game::smsg_guild_fight_pvp_look &msg, const std::string &name, int id);

	static void res_guild_pvp_fight(dhc::player_t *player, const protocol::game::smsg_guild_fight &msg, const std::string &name, int id);
};

class SelfMessage
{
public:
	static void self_player_load_end(uint64_t player_guid, int opcode, const std::string &msg);

	static void self_start_boss();

	static void self_start_guild_boss();

	static void self_boss_change_name(uint64_t player_guid, const std::string&msg);
};

class PushMessage
{
public:
	static void push_chat(dhc::player_t *player, int type, const std::string &color, const std::string &text,
		int is_danmu, uint64_t target_guid, const std::string &target_name, const std::vector<uint64_t> &guids, const std::string &name);

	static void push_gundong(const std::string &text, const std::string &name);

	static void push_vote(dhc::player_t *player, int goddess, int renqi);
};

#endif
