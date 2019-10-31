#ifndef __GAMEINC_H__
#define __GAMEINC_H__

#include "protos.h"
#include "typedefs.h"
#include "game.h"
#include "game_interface.h"
#include "singleton.h"
#include "opcodes.h"
#include "packet.h"
#include "rpc.pb.h"

#include <boost/lexical_cast.hpp>
#include <boost/bind.hpp>
#include <boost/scoped_array.hpp>

//////////////////////////////////////////////////////////////////////////

enum etypes
{
	et_null = 0x00,				/// 空类型
	et_player = 0x01,			/// 玩家类型
	et_acc = 0x02,				/// 账号类型
	et_gtool = 0x03,			/// guid产生器类型
	et_recharge = 0x04,			/// 充值类型
	et_recharge_list = 0x05,	/// 充值列表类型
	et_role = 0x06,				/// 角色类型
	et_equip = 0x07,			/// 装备类型
	et_post_list = 0x08,		/// 邮件列表类型
	et_post = 0x09,				/// 邮件类型
	et_sport = 0x0a,			/// 竞技场战报类型
	et_sport_list = 0x0b,		/// 竞技场列表类型
	et_guild = 0x0c,			/// 军团类型
	et_guild_list = 0x0d,		/// 军团列表类型
	et_guild_event = 0x0e,		/// 军团日志类型
	et_guild_member = 0x0f,		/// 军团成员类型
	et_npc = 0x10,				/// npc类型
	et_rank = 0x11,				/// 排行榜类型
	et_social = 0x12,			/// 社交类型
	et_social_list = 0x13,		/// 社交列表类型
	et_reharge_heitao = 0x14,	/// 黑桃充值类型
	et_global = 0x15,			/// 全局类型
	et_treasure = 0x16,			/// 宝物类型
	et_treasure_list = 0x17,	/// 宝物列表类型
	et_ore_list = 0x18,			/// 金矿列表类型
	et_treasure_report = 0x19,  /// 抢夺日志
	et_lottery_list = 0x1a,		/// pvp列表
	et_huodong = 0x1b,			/// 活动类型
	et_huodong_entry = 0x1c,	/// 活动entry类型
	et_huodong_list = 0x1d,		/// 活动列表类型
	et_guild_mission = 0x1e,    /// 家族公会战类型
	et_guild_message = 0x1f,    /// 家族留言板类型
	et_boss = 0x20,				/// boss战类型
	et_player_guids_list = 0x21, /// 玩家列表类型
	et_huodong_player = 0x22,    /// 活动player类型
	et_guild_red = 0x23,		 /// 军团红包类型
	et_guild_box = 0x24,		 /// 军团宝箱
	et_pet = 0x25,				 /// 宠物类型
	et_guild_pvp = 0x26,		 /// 军团战类型
	et_guild_pvp_list = 0x27,	 /// 军团战列表类型
	et_guild_fight = 0x28,
	et_guild_fight_list = 0x29,
	et_all
};

#define MAKE_GUID(T, I) (((uint64_t(T) << 56) & 0xFF00000000000000) | ((uint64_t(boost::lexical_cast<uint64_t>(game::env()->get_game_value("qid"))) << 44) & 0x00FFF00000000000) | (uint64_t(I) & 0x00000FFFFFFFFFFF))

#define MAKE_GUID_EX(T, Q, I) (((uint64_t(T) << 56) & 0xFF00000000000000) | ((uint64_t(Q) << 44) & 0x00FFF00000000000) | (uint64_t(I) & 0x00000FFFFFFFFFFF))

#define TRANS_GUID(T, G) (((uint64_t(T) << 56) & 0xFF00000000000000) | (uint64_t(G) & 0x00FFFFFFFFFFFFFF))

#define GUID_COUNTER(guid) (uint64_t)(guid & 0x00000FFFFFFFFFFF)

inline etypes type_of_guid(uint64_t guid)
{
	uint8_t et = (uint8_t)((uint64_t(guid) >> 56) & 0x00000000000000FF);
	return (et > et_null && et < et_all) ? etypes(et) : et_null;
}

#define IS_PLAYER_GUID(guid)  (type_of_guid(guid) == et_player)
#define IS_ACC_GUID(guid)    (type_of_guid(guid) == et_acc)
#define IS_GTOOL_GUID(guid)    (type_of_guid(guid) == et_gtool)
#define IS_RECHARGE_GUID(guid)    (type_of_guid(guid) == et_recharge)
#define IS_RECHARGE_LIST_GUID(guid)    (type_of_guid(guid) == et_recharge_list)
#define IS_ROLE_GUID(guid)    (type_of_guid(guid) == et_role)
#define IS_EQUIP_GUID(guid)    (type_of_guid(guid) == et_equip)
#define IS_POST_LIST_GUID(guid)  (type_of_guid(guid) == et_post_list)
#define IS_POST_GUID(guid)  (type_of_guid(guid) == et_post)
#define IS_SPORT_GUID(guid)  (type_of_guid(guid) == et_sport)
#define IS_SPORT_LIST_GUID(guid)  (type_of_guid(guid) == et_sport_list)
#define IS_GUILD_GUID(guid)  (type_of_guid(guid) == et_guild)
#define IS_GUILD_LIST_GUID(guid)  (type_of_guid(guid) == et_guild_list)
#define IS_GUILD_ARRANGE_GUID(guid)  (type_of_guid(guid) == et_guild_pvp)
#define IS_GUILD_ARRANGE_LIST_GUID(guid)  (type_of_guid(guid) == et_guild_pvp_list)
#define IS_GUILD_FIGHT_GUID(guid) (type_of_guid(guid) == et_guild_fight)
#define IS_GUILD_EVENT_GUID(guid)  (type_of_guid(guid) == et_guild_event)
#define IS_GUILD_MEMBER_GUID(guid)  (type_of_guid(guid) == et_guild_member)
#define IS_RANK_GUID(guid)  (type_of_guid(guid) == et_rank)
#define IS_SOCIAL_GUID(guid)  (type_of_guid(guid) == et_social)
#define IS_SOCIAL_LIST_GUID(guid)  (type_of_guid(guid) == et_social_list)
#define IS_RECHARGE_HEITAO_GUID(guid)  (type_of_guid(guid) == et_reharge_heitao)
#define IS_GLOBAL_GUID(guid)	(type_of_guid(guid) == et_global)
#define IS_TREASURE_GUID(guid)  (type_of_guid(guid) == et_treasure)
#define IS_TREASURE_LIST_GUID(guid) (type_of_guid(guid) == et_treasure_list)
#define IS_TREASURE_REPORT_GUID(guid) (type_of_guid(guid) == et_treasure_report)
#define IS_LOTTERY_LIST_GUID(guid) (type_of_guid(guid) == et_lottery_list)
#define IS_HUODONG_GUID(guid) (type_of_guid(guid) == et_huodong)
#define IS_HUODONG_ENTRY_GUID(guid) (type_of_guid(guid) == et_huodong_entry)
#define IS_HUODONG_LIST_GUID(guid) (type_of_guid(guid) == et_huodong_list)
#define IS_GUILD_MISSION_GUID(guid) (type_of_guid(guid) == et_guild_mission)
#define IS_GUILD_MESSAGE_GUID(guid) (type_of_guid(guid) == et_guild_message)
#define IS_BOSS_GUID(guid) (type_of_guid(guid) == et_boss)
#define IS_PLAYER_LIST_GUID(guid) (type_of_guid(guid) == et_player_guids_list)
#define IS_HUODONG_PLAYER_GUID(guid) (type_of_guid(guid) == et_huodong_player)
#define IS_GUILD_RED_GUID(guid) (type_of_guid(guid) == et_guild_red)
#define IS_GUILD_BOX_GUID(guid) (type_of_guid(guid) == et_guild_box)
#define IS_PET_GUID(guid) (type_of_guid(guid) == et_pet)
#define IS_GUILD_FIGHT_LIST_GUID(guid) (type_of_guid(guid) == et_guild_fight_list)

#define POOL_GET(_type, __guid__) \
	((_type*)game::pool()->get(__guid__))

#define POOL_GET_PLAYER(__guid__) \
	((type_of_guid((__guid__)) != et_player) ? NULL : (dhc::player_t*)game::pool()->get(__guid__))

#define POOL_GET_ROLE(__guid__) \
	((type_of_guid((__guid__)) != et_role) ? NULL : (dhc::role_t*)game::pool()->get(__guid__))

#define POOL_GET_EQUIP(__guid__) \
	((type_of_guid((__guid__)) != et_equip) ? NULL : (dhc::equip_t*)game::pool()->get(__guid__))

#define POOL_GET_POST(__guid__) \
	((type_of_guid((__guid__)) != et_post) ? NULL : (dhc::post_t*)game::pool()->get(__guid__))

#define POOL_GET_SPORT(__guid__) \
	((type_of_guid((__guid__)) != et_sport) ? NULL : (dhc::sport_t*)game::pool()->get(__guid__))

#define POOL_GET_SPORT_LIST(__guid__) \
	((type_of_guid((__guid__)) != et_sport_list) ? NULL : (dhc::sport_list_t*)game::pool()->get(__guid__))

#define POOL_GET_GUILD(__guid__) \
	((type_of_guid((__guid__)) != et_guild) ? NULL : (dhc::guild_t*)game::pool()->get(__guid__))

#define POOL_GET_GUILD_LIST(__guid__) \
	((type_of_guid((__guid__)) != et_guild_list) ? NULL : (dhc::guild_list_t*)game::pool()->get(__guid__))

#define POOL_GET_GUILD_ARRANGE(__guid__) \
	((type_of_guid((__guid__)) != et_guild_pvp) ? NULL : (dhc::guild_arrange_t*)game::pool()->get(__guid__))

#define POOL_GET_GUILD_FIGHT(__guid__) \
	((type_of_guid((__guid__)) != et_guild_fight) ? NULL : (dhc::guild_fight_t*)game::pool()->get(__guid__))

#define POOL_GET_GUILD_EVENT(__guid__) \
	((type_of_guid((__guid__)) != et_guild_event) ? NULL : (dhc::guild_event_t*)game::pool()->get(__guid__))

#define POOL_GET_GUILD_MEMBER(__guid__) \
	((type_of_guid((__guid__)) != et_guild_member) ? NULL : (dhc::guild_member_t*)game::pool()->get(__guid__))

#define  POOL_GET_RANK(__guid__) \
	((type_of_guid((__guid__)) != et_rank) ? NULL : (dhc::rank_t*)game::pool()->get(__guid__))

#define  POOL_GET_SOCIAL(__guid__) \
	((type_of_guid((__guid__)) != et_social) ? NULL : (dhc::social_t*)game::pool()->get(__guid__))

#define  POOL_GET_GLOBAL(__guid__) \
	((type_of_guid((__guid__)) != et_global) ? NULL : (dhc::global_t*)game::pool()->get(__guid__))

#define  POOL_GET_TREASURE(__guid__) \
	((type_of_guid((__guid__)) != et_treasure) ? NULL : (dhc::treasure_t*)game::pool()->get(__guid__))

#define  POOL_GET_TREASURE_LIST(__guid__) \
	((type_of_guid((__guid__)) != et_treasure_list) ? NULL : (dhc::treasure_list_t*)game::pool()->get(__guid__))

#define  POOL_GET_TREASURE_REPORT(__guid__) \
	((type_of_guid((__guid__)) != et_treasure_report) ? NULL : (dhc::treasure_report_t*)game::pool()->get(__guid__))

#define  POOL_GET_LOTTERY_LIST(__guid__) \
	((type_of_guid((__guid__)) != et_lottery_list) ? NULL : (dhc::pvp_list_t*)game::pool()->get(__guid__))

#define  POOL_GET_HUODONG(__guid__) \
	((type_of_guid((__guid__)) != et_huodong) ? NULL : (dhc::huodong_t*)game::pool()->get(__guid__))

#define  POOL_GET_HUODONG_ENTRY(__guid__) \
	((type_of_guid((__guid__)) != et_huodong_entry) ? NULL : (dhc::huodong_entry_t*)game::pool()->get(__guid__))

#define  POOL_GET_GUILD_MISSION(__guid__) \
	((type_of_guid((__guid__)) != et_guild_mission) ? NULL : (dhc::guild_mission_t*)game::pool()->get(__guid__))

#define  POOL_GET_GUILD_MESSAGE(__guid__) \
	((type_of_guid((__guid__)) != et_guild_message) ? NULL : (dhc::guild_message_t*)game::pool()->get(__guid__))

#define  POOL_GET_HUODONG_PLAYER(__guid__) \
	((type_of_guid((__guid__)) != et_huodong_player) ? NULL : (dhc::huodong_player_t*)game::pool()->get(__guid__))

#define  POOL_GET_GUILD_RED(__guid__) \
	((type_of_guid((__guid__)) != et_guild_red) ? NULL : (dhc::guild_red_t*)game::pool()->get(__guid__))

#define  POOL_GET_GUILD_BOX(__guid__) \
	((type_of_guid((__guid__)) != et_guild_box) ? NULL : (dhc::guild_box_t*)game::pool()->get(__guid__))

#define  POOL_GET_BOSS(__guid__) \
	((type_of_guid((__guid__)) != et_boss) ? NULL : (dhc::boss_t*)game::pool()->get(__guid__))

#define  POOL_GET_PET(__guid__) \
	((type_of_guid((__guid__)) != et_pet) ? NULL : (dhc::pet_t*)game::pool()->get(__guid__))

#define POOL_ADD(__guid__, __entity__) \
	game::pool()->add((__guid__), (__entity__), mmg::Pool::state_none)

#define POOL_ADD_NEW(__guid__, __entity__) \
	game::pool()->add((__guid__), (__entity__), mmg::Pool::state_new)

#define POOL_REMOVE(__guid__, __ref_guid__) \
	game::pool()->remove((__guid__), (__ref_guid__))

#define POOL_RELEASE(__guid__) \
	game::pool()->release((__guid__))

#define POOL_SAVE(__type__, __entity__, __release__) \
	{ \
		if (__entity__->changed()) \
		{ \
			__entity__->clear_changed(); \
			mmg::Pool::estatus es = game::pool()->get_state(__entity__->guid()); \
			opcmd_t opt = opc_update; \
			if (es == mmg::Pool::state_new) \
			{ \
				opt = opc_insert; \
				game::pool()->set_state(__entity__->guid(), mmg::Pool::state_none); \
			} \
			Request *req = new Request(); \
			__type__ *cmsg = new __type__; \
			cmsg->CopyFrom(*__entity__); \
			req->add(opt, __entity__->guid(), cmsg); \
			game::pool()->upcall(req, 0); \
		} \
		if (__release__) \
		{ \
			POOL_RELEASE(__entity__->guid()); \
			delete __entity__; \
		} \
	}

inline std::string make_ertext(const char *s, int d, const char *ss)
{
	char c[256];
	sprintf(c, "%s(%d)-<%s>: ", s, d, ss);
	return std::string(c);
}

#define GLOBAL_ERROR game::rpc_service()->response(name, id, "", -10, make_ertext(__FILE__, __LINE__, __FUNCTION__))

#define PERROR(code) game::rpc_service()->response(name, id, "", code, "")

#define PCK_CHECK \
	std::string sig = msg.comm().sig(); \
	int res = game::channel()->check_sig(player_guid, sig); \
	if (res == -1) \
	{ \
		PERROR(ERROR_SIG); \
		return; \
	} \
	else if (res == 1) \
	{ \
		PERROR(ERROR_BEKILL); \
		return; \
	} \
	int pck_id = msg.comm().pck_id(); \
	int pres = game::channel()->check_pck(player_guid, pck_id); \
	if (pres == 1) \
	{ \
		std::string s; \
		game::channel()->last_pck(player_guid, s); \
		ResMessage::res_last(s, name, id); \
		return; \
	} \
	else if (pres == -1) \
	{ \
		PERROR(ERROR_PCK); \
		return; \
	}\
	if (player->gold() != msg.comm().pck_gold()) \
	{	\
		PERROR(ERROR_PCK_GOLD); \
		return; \
	} \
	if (player->jewel() != msg.comm().pck_jewel()) \
	{ \
		PERROR(ERROR_PCK_JEWEL); \
		return; \
	}
	

#define PCK_CHECK_EX \
	std::string sig = msg.comm().sig(); \
	int res = game::channel()->check_sig(player_guid, sig); \
	if (res == -1) \
	{ \
		PERROR(ERROR_SIG); \
		return; \
	} \
	else if (res == 1) \
	{ \
		PERROR(ERROR_BEKILL); \
		return; \
	}

#define PCK_CHECK_NO_GOLD \
	std::string sig = msg.comm().sig(); \
	int res = game::channel()->check_sig(player_guid, sig); \
	if (res == -1) \
	{ \
		PERROR(ERROR_SIG); \
		return; \
	} \
	else if (res == 1) \
	{ \
		PERROR(ERROR_BEKILL); \
		return; \
	} \
	int pck_id = msg.comm().pck_id(); \
	int pres = game::channel()->check_pck(player_guid, pck_id); \
	if (pres == 1) \
	{ \
		std::string s; \
		game::channel()->last_pck(player_guid, s); \
		ResMessage::res_last(s, name, id); \
		return; \
	} \
	else if (pres == -1) \
	{ \
		PERROR(ERROR_PCK); \
		return; \
	}

struct s_t_reward
{
	int type;
	int value1;
	int value2;
	int value3;

	s_t_reward()
		:type(0),
		value1(0),
		value2(0),
		value3(0)
	{

	}

	s_t_reward(int t, int v1, int v2, int v3 = 0)
		:type(t),
		value1(v1),
		value2(v2),
		value3(v3)
	{

	}
};

struct s_t_rewards
{
	enum
	{
		HUODONG_ITEM_ID1 = 80020001,
		HUODONG_ITEM_ID2 = 80020002,
	};

	std::vector<s_t_reward> rewards;
	std::vector<dhc::role_t*> roles;
	std::vector<dhc::equip_t*> equips;
	std::vector<dhc::treasure_t*> treasures;
	std::vector<dhc::pet_t*> pets;
	std::vector<uint64_t> guids;

	void add_reward(int type, int value1, int value2, int value3 = 0)
	{
		rewards.push_back(s_t_reward(type, value1, value2, value3));
	}

	void add_reward(const s_t_reward& reward)
	{
		rewards.push_back(reward);
	}

	void add_reward(const std::vector<s_t_reward>& reward)
	{
		rewards.insert(rewards.end(), reward.begin(), reward.end());
	}

	void merge()
	{
		std::vector<s_t_reward> rds = rewards;
		rewards.clear();
		for (std::vector<s_t_reward>::size_type i = 0; i < rds.size(); ++i)
		{
			if (rds[i].type == 3 || rds[i].type == 4 || rds[i].type == 6 || rds[i].type == 11)
			{
				rewards.push_back(rds[i]);
				continue;
			}
			bool has = false;
			for (std::vector<s_t_reward>::size_type j = 0; j < rewards.size(); ++j)
			{
				if (rewards[j].type == rds[i].type && rewards[j].value1 == rds[i].value1)
				{
					rewards[j].value2 += rds[i].value2;
					has = true;
					break;
				}
			}
			if (!has)
			{
				rewards.push_back(rds[i]);
			}
		}
	}
};

#define ADD_MSG_REWARD(msg, rds) \
for (std::vector<s_t_reward>::size_type i = 0; i < rds.rewards.size(); ++i) \
{ \
	msg.add_types(rds.rewards[i].type); \
	msg.add_value1s(rds.rewards[i].value1); \
	msg.add_value2s(rds.rewards[i].value2); \
	msg.add_value3s(rds.rewards[i].value3); \
} \
for (std::vector<dhc::role_t*>::size_type i = 0; i < rds.roles.size(); ++i) \
{ \
	msg.add_roles()->CopyFrom(*(rds.roles[i])); \
} \
for (std::vector<dhc::equip_t*>::size_type i = 0; i < rds.equips.size(); ++i) \
{ \
	msg.add_equips()->CopyFrom(*(rds.equips[i])); \
} \
for (std::vector<dhc::treasure_t*>::size_type i = 0; i < rds.treasures.size(); ++i) \
{ \
	msg.add_treasures()->CopyFrom(*(rds.treasures[i])); \
}

#define ADD_MSG_REWARD_WITH_PET(msg, rds) \
for (std::vector<s_t_reward>::size_type i = 0; i < rds.rewards.size(); ++i) \
{ \
	msg.add_types(rds.rewards[i].type); \
	msg.add_value1s(rds.rewards[i].value1); \
	msg.add_value2s(rds.rewards[i].value2); \
	msg.add_value3s(rds.rewards[i].value3); \
} \
for (std::vector<dhc::role_t*>::size_type i = 0; i < rds.roles.size(); ++i) \
{ \
	msg.add_roles()->CopyFrom(*(rds.roles[i])); \
} \
for (std::vector<dhc::equip_t*>::size_type i = 0; i < rds.equips.size(); ++i) \
{ \
	msg.add_equips()->CopyFrom(*(rds.equips[i])); \
} \
for (std::vector<dhc::treasure_t*>::size_type i = 0; i < rds.treasures.size(); ++i) \
{ \
	msg.add_treasures()->CopyFrom(*(rds.treasures[i])); \
} \
for (std::vector<dhc::pet_t*>::size_type i = 0; i < rds.pets.size(); ++i) \
{ \
	msg.add_pets()->CopyFrom(*(rds.pets[i])); \
}

#define LOG_OUTPUT(player, type, value1, value2, value3, value4) \
	TermInfo* info = game::channel()->get_channel(player->guid()); \
	if (info) \
	{\
		game::log()->log(info->username, player->serverid(), player->guid(), type, value1, value2, value3, value4, info->platform); \
	}

#endif
