#ifndef PVP_OPERATION_H
#define PVP_OPERATION_H

#include "gameinc.h"

namespace rpcproto {
	class tmsg_player_fight_player;
}

enum PvpType
{
	PT_LIEREN,
	PT_BINGYUAN,
};

class PvpOperation
{
public:
	static void sync(dhc::player_t* player);

	static void copy(const dhc::player_t* fplayer, rpcproto::tmsg_player_fight_player* player, PvpType type);

	static void sync_player(dhc::player_t* player);
};

enum PvpListNum
{
	PVP_LIST_0 = 0,
	PVP_LIST_1,
	PVP_LIST_2,

	PVP_LIST_REWARD = 100,
	PVP_LIST_RANK	= 101,

	PVP_BINGYUAN_REWARD = 200,
	PVP_BINGYUAN_LIST,
	PVP_LIST_END,

	PVP_GUILD_REWARD = 300,
	PVP_GUILD_LIST_END,
};

struct SyncPolicy
{
	bool enforce;
	int  last_bf;
	int  count;
	int  total;
	int  pvp_point;
};

struct TeamPullPolicy
{
	int last_chenghao;
	uint64_t next_time;
};

class PvpList
{
public:
	int init();

	int fini();

	void update();

	protocol::game::smsg_pvp_view* get_pvp_list(uint64_t player_guid);

	const protocol::game::smsg_pvp_view* get_empty_pvp_list() const { return &empty_view_; }

	void refresh_pvp_list(uint64_t player_guid, const protocol::game::smsg_pvp_view& view);

	void refresh_empty_view(const protocol::game::smsg_pvp_view& view);

	void update_pvp_list(uint64_t player_guid, PvpListNum index);

	bool should_sync(dhc::player_t* player);

	void set_sync(dhc::player_t* player);

	void remove_sync(dhc::player_t* player);

	bool should_team_pull(dhc::player_t *player);

	void remove_team_pull(dhc::player_t *player);

	void update_bingyuan_list(uint64_t player_guid);

public:
	void add_social_invite(uint64_t player_guid);

	int get_social_invite(uint64_t player_guid);

private:
	void load_pvp_list();

	void load_pvp_list_callback(Request *req, uint64_t pvp_guid);

	void changed_pvp_list(uint64_t player_guid, const protocol::game::smsg_pvp_view* view);

	void changed_empty_list(const protocol::game::smsg_pvp_view* view);

	void add_pvp_view(uint64_t player_guid, uint64_t target_guid, const std::string& name, int id,
		int server, int bf, int point, int win, int vip, int achieve, int guanghuan, int dress, int chenghao,int nalflag, bool empty);

	void ds_reward();

private:
	std::map<uint64_t, protocol::game::smsg_pvp_view> player_views_;
	std::map<uint64_t, SyncPolicy> player_sync_policy_;

	protocol::game::smsg_pvp_view empty_view_;

	std::map<uint64_t, TeamPullPolicy> player_team_pull_;
	std::set<uint64_t> team_social_invite_;
};

#define sPvpList (Singleton<PvpList>::instance())

#endif
