#ifndef __RANK_TEAM_H__
#define __RANK_TEAM_H__

#include "gameinc.h"

enum rank_enum
{
	erank_chenghao = 0,
	erank_chenghao_last,
	erank_namelist,
	erank_single_duanwei,
	erank_end,
};

struct QueryMap
{
	int query_num;
	std::string name;
	int id;
	std::vector<int> params;
	std::vector<std::string> msgs;
};

struct OfflinePlayer
{
	uint64_t guid;
	int level;
	int duanwei;
	int bf;
};

class PoolManager
{
public:
	int init();

	int fini();

	int update(const ACE_Time_Value & tv);

	void load_all_player();

	void load_all_player_callback(Request* req);

	void load_rank();

	void load_rank_callback(Request* req);

	void load_player(uint64_t player_guid, const std::string& name, int id);

	void load_player_callback(Request *req);

	void load_global();

	void load_global_callback(Request* req);

	void load_player_check_end(dhc::player_t *player);

	void load_msg_callback(Request *req, dhc::player_t *player);

	void save_player(uint64_t guid, bool release);

	int get_chenghao(uint64_t player_guid) const;

	void get_rank_info(uint64_t player_guid, int &chenghao, int &point, int &rank) const;

	void check_rank(dhc::player_t *player, rank_enum index, int value);

	void view_rank(dhc::player_t *player, rank_enum index, protocol::game::smsg_rank_view &smsg);

	void clear_rank(dhc::rank_t *rank);

	void get_dead_player(uint64_t guid,
		int level,
		int num,
		std::vector<dhc::player_t* > &deadd,
		const std::set<uint64_t> &has_invite);

	uint64_t get_dead_player(uint64_t guild,int duanwei, int64_t bf_max, int64_t bf_min) const;

	void add_dead_player(uint64_t guid, int level, int duanwei, int bf);

	void remove_dead_player(uint64_t guid);

	bool is_npc_guid(uint64_t guid) const;

	void get_duanwei_info(uint64_t player_guid, int &ranki, int &duanwei, int &point) const;

	int get_duanwei(uint64_t player_guid) const;

	int get_duanwei(dhc::player_t *player) const;

private:
	void make_npc();

private:
	std::map<uint64_t, QueryMap> query_map_;

	std::map<uint64_t, OfflinePlayer> players_;

	std::vector<uint64_t> npcs_;

	std::string default_talk_;
};

#define sPoolManager (Singleton<PoolManager>::instance())
#endif //__RANK_TEAM_H__