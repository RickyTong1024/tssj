#ifndef __GUILD_OPERATION_H__
#define __GUILD_OPERATION_H__

#include "gameinc.h"

// 公会操作类型
enum e_guild_op_type
{
	e_guild_op_type_join = 0,		// 加入军团
	e_guild_op_type_sign_in1 = 1,	// 初级签到
	e_guild_op_type_sign_in2 = 2,	// 中级签到
	e_guild_op_type_sign_in3 = 3,	// 高级签到
	e_guild_op_type_exit = 4,		// 退出军团
	e_guild_op_type_kick = 5,		// 踢出军团
	e_guild_op_type_renming = 6,	// 任命副军团长
	e_guild_op_type_jiechu = 7,		// 解除副军团长
	e_guild_op_type_zhuanrang = 8,	// 职务转让
	e_guild_op_type_levelup = 9,	// 军团升级
	e_guild_op_type_mission = 10,	// 击杀守卫
	e_guild_op_type_tanhe	= 11,   // 弹劾军团长

	e_guild_op_type_end,
};

class GuildOperation
{
public:
	static void guild_refresh_check(dhc::guild_t * guild);

	static void guild_load_check();

	static void guild_refresh(dhc::guild_t * guild);

	static void guild_week_refresh(dhc::guild_t * guild);

	static operror_t check_guild_name(const std::string &name);

	static operror_t check_bulletin(const std::string &name);

	static uint32_t get_guild_member_count(dhc::guild_t *guild, int type);

	static dhc::guild_member_t* get_guild_member(dhc::guild_t *guild, uint64_t player_guid);

	static dhc::guild_member_t* create_guild_member(dhc::guild_t *guild, dhc::player_t *player, int duty);

	static dhc::guild_event_t* create_guild_event(dhc::guild_t *guild, const std::string &name, int type, int value = 0, int value1 = 0);

	static void mod_guild_exp(dhc::guild_t * guild, int add_exp);

	static bool member_compare(dhc::guild_member_t *member1, dhc::guild_member_t *member2);

	static bool guild_compare(dhc::guild_t *guild1, dhc::guild_t *guild2);

	static dhc::guild_mission_t* create_guild_mission(dhc::guild_t *guild);

	static int mod_guild_mission(dhc::player_t* player, dhc::guild_mission_t *guild_mission, int mission);

	static void create_guild_mission(dhc::guild_mission_t *guild_mission, bool reset = false);

	static void mod_guild_mission_damage(dhc::player_t *player, dhc::guild_mission_t *guild_mission, int64_t hp);

	static void create_guild_message(dhc::player_t *player, dhc::guild_t* guild, int zhiwu, const std::string &text);

	static void delete_guild_message(dhc::guild_t* guild, uint64_t msg_guid);

	static void top_guild_message(dhc::guild_t* guild, uint64_t msg_guid);

	static void add_guild_mission_reward(dhc::guild_mission_t *guild_mission, int ceng, int index, const std::string& name);

	static void add_guild_member_reward(dhc::guild_member_t *guild_member, int ceng, int index);

	static void add_guild_member_reward(dhc::player_t *player, int ceng, int index);

	static bool has_guild_mission_reward(dhc::guild_mission_t *guild_mission, int ceng, int index);

	static bool has_guild_member_reward(dhc::guild_member_t *guild_member, int ceng, int index);

	static bool has_guild_member_reward(dhc::player_t *player, int ceng, int index);

	static int get_guild_apply_index(dhc::guild_t* guild, uint64_t player_guid);

	static void remove_player_apply(dhc::player_t *player, dhc::guild_t* guild, bool remove = true);

	static void remove_player_all_apply(dhc::player_t* player);

	static int has_red_point(dhc::player_t* player);

	static bool has_mobai_point(dhc::player_t *player);

	static bool has_fight_point(dhc::guild_t* guild, dhc::player_t *player);

	static bool has_reward_point(dhc::player_t* player, dhc::guild_mission_t* mission);

	static bool has_keji_point(dhc::guild_t* guild, dhc::player_t* player, dhc::guild_member_t* member);

	static bool has_apply_point(dhc::guild_t* guild, dhc::guild_member_t* member);

	static bool has_shop_point(dhc::guild_t* guild, dhc::player_t* player);

	static bool has_guild_red_point(dhc::guild_t *guild, dhc::player_t *player);

	static void refresh_guild_offline(uint64_t guild_guid, uint64_t player_guid);

	static void post_leave_guild(uint64_t player_guid, const std::string& guild_name, int type);

	static void refresh_guild_mission_name_nalflag(dhc::player_t* player, const std::string& name, const int& nalflag);

	static dhc::guild_red_t* create_guild_red(dhc::player_t *player, dhc::guild_t *guild, int id,  int jewel, const std::string& text);

	static int get_guild_pvp_state(dhc::global_t *global, dhc::guild_t *guild);

	static void set_guild_pvp_state(int day, int hour);

	static bool has_guildpvp_point(dhc::guild_t *guild, dhc::player_t *player, dhc::guild_member_t* member);
};

#endif
