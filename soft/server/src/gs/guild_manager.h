#ifndef __GUILD_MANAGER_H__
#define __GUILD_MANAGER_H__

#include "gameinc.h"
#include "guild_operation.h"

#define GUILD_ADD_LEVEL 38
#define CREATE_GUILD_CONSUME 500	// 创建公会消耗的钻石
#define DEFAULT_GUILD_ICON_ID 1001
#define JOIN_GUILD_TIME_LIMITED 86400000	// 加入公会的时间间隔
#define NAME_COUNT_MAX 16		// 公会名字的最大长度
#define BULLETIN_COUNT_MAX 256	// 公告字符的最大长度
#define RECOMMEND_GUILD_COUNT_MAX 10	// 每次推荐的公会的数量
#define GUILD_RANKING_MAX 50		// 公会排行榜最大数量
#define GUILD_BF_MAX 1000000	// 公会需求最大战斗力
#define GUILD_MISSION_NUM 3  // 军团副本次数
#define GUILD_MISSION_CD  7200000

// 公会成员类型
enum e_guild_member_type
{
	e_member_type_leader	= 0,	// 军长
	e_member_type_senator	= 1,	// 副军长
	e_member_type_common	= 2,	// 普通成员
};

// 公会成员签到类型
enum e_sign_in_type
{
	e_sign_in_type_primary	= 0,	// 初级签到
	e_sign_in_type_middle	= 1,	// 中级签到
	e_sign_in_type_advanced	= 2,	// 高级签到
};


enum e_pvp_guild_stat
{
	e_pvp_guild_apply       = 0,       //报名
	e_pvp_guild_fight_quexi = 1,       //第一周并且缺席
	e_pvp_guild_quexi       = 2,	   //第二周并且缺席
	e_pvp_guild_xiuzhan     = 3,	   //第二周未缺席
	e_pvp_guild_look_bushu  = 4,
	e_pvp_guild_look_pipei  = 5,
	e_pvp_guild_look_zhanji = 6,
};

class GuildManager
{
public:
	GuildManager();

	~GuildManager();

	int init();

	int fini();
public:
	void terminal_guild_create(const std::string &data, const std::string &name, int id);

	void terminal_guild_apply(const std::string &data, const std::string &name, int id);

	void terminal_guild_agree(const std::string &data, const std::string &name, int id);

	void terminal_guild_open(const std::string &data, const std::string &name, int id);

	void terminal_guild_list_recommend(const std::string &data, const std::string &name, int id);

	void terminal_guild_query(const std::string &data, const std::string &name, int id);

	void terminal_guild_modify_icon(const std::string &data, const std::string &name, int id);

	void terminal_guild_modify_bulletin(const std::string &data, const std::string &name, int id);

	void terminal_guild_modify_name(const std::string &data, const std::string &name, int id);

	void terminal_guild_change_member_duty(const std::string &data, const std::string &name, int id);

	void terminal_guild_set_join_condition(const std::string &data, const std::string &name, int id);

	void terminal_guild_kick_member(const std::string &data, const std::string &name, int id);

	void terminal_guild_leave(const std::string &data, const std::string &name, int id);

	void terminal_guild_dismiss(const std::string &data, const std::string &name, int id);

	void terminal_guild_tanhe(const std::string &data, const std::string &name, int id);

	void terminal_guild_member_view(const std::string &data, const std::string &name, int id);

	void terminal_guild_ranking(const std::string &data, const std::string &name, int id);

	void terminal_guild_mission_ranking(const std::string &data, const std::string &name, int id);

	void terminal_guild_activity(const std::string &data, const std::string &name, int id);

	void terminal_guild_sign_in(const std::string &data, const std::string &name, int id);

	void terminal_guild_sign_reward(const std::string &data, const std::string &name, int id);

	void terminal_guild_mission_look(const std::string &data, const std::string &name, int id);

	void terminal_guild_mission_look_ex(const std::string &data, const std::string &name, int id);
	
	void terminal_guild_mission_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_guild__mission_complate_reward_view(const std::string &data, const std::string &name, int id);

	void terminal_guild_mission_complete_reward(const std::string &data, const std::string &name, int id);

	void terminal_guild_mission_shilian_reward(const std::string &data, const std::string &name, int id);

	void terminal_guild_buy_mission_num(const std::string &data, const std::string &name, int id);

	void terminal_guild_message_view(const std::string &data, const std::string &name, int id);

	void terminal_guild_message_add(const std::string &data, const std::string &name, int id);

	void terminal_guild_message_delete(const std::string &data, const std::string &name, int id);

	void terminal_guild_message_top(const std::string &data, const std::string &name, int id);

	void terminal_guild_keji_open(const std::string &data, const std::string &name, int id);

	void terminal_guild_keji_up(const std::string &data, const std::string &name, int id);

	void terminal_guild_keji_study(const std::string &data, const std::string &name, int id);

	void terminal_guild_keji_skillup(const std::string &data, const std::string &name, int id);

	void terminal_guild_red_deliver(const std::string &data, const std::string &name, int id);

	void terminal_guild_red_view(const std::string &data, const std::string &name, int id);

	void terminal_guild_red_rob(const std::string &data, const std::string &name, int id);

	void terminal_guild_red_target(const std::string &data, const std::string &name, int id);

	//////////////////////////////军团战///////////////////////////////////////////
	void terminal_guild_pvp_look(const std::string &data, const std::string &name, int id);

	void terminal_guild_pvp_baoming(const std::string &data, const std::string &name, int id);

	void terminal_guild_pvp_bushu(const std::string &data, const std::string &name, int id);

	void terminal_guild_pvp_fight(const std::string &data, const std::string& name, int id);

	void terminal_guild_pvp_buy(const std::string &data, const std::string& name, int id);

	void terminal_guild_pvp_reward(const std::string &data, const std::string& name, int id);

	void self_guild_load_apply(Packet *pck);

	void self_guild_load_kick(Packet *pck);

private:
	void terminal_guild_pvp_baoming_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id);

	void terminal_guild_pvp_look_bushu_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id);

	void terminal_guild_pvp_bushu_callback(const std::string &data, uint64_t player_guid, const std::string& name, int id);
	
	void terminal_guild_pvp_look_pipei_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id);

	void terminal_guild_pvp_look_zhanji_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id);
	
	void terminal_guild_pvp_fight_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id);

	void terminal_guild_pvp_target_callback(const std::string &data, uint64_t player_guid, int reward_id, const std::string &name, int id);

	void terminal_guild_pvp_xiuzhan_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id);

private:
	int timer_;
	

};

#endif
