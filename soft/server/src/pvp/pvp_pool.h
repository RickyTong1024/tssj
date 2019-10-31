#ifndef PVP_POOL_H
#define PVP_POOL_H

#include "gameinc.h"

struct s_t_guildfight;

namespace rpcproto {
	class tmsg_player_fight_player;
	class tmsg_rep_pvp_match;
	class tmsg_rep_invite_level;
	class tmsg_req_invite_code;
	class tmsg_rep_guild_fight;
}

struct QueryMap
{
	int query_num;
	std::string name;
	int id;
	std::vector<int> params;
	std::vector<std::string> msgs;
};

enum rank_enum
{
	erank_point = 0,
	erank_end,

	erank_point_last = 100,
	erank_point_last_end,

	erank_guild_zhanji = 200,
	erank_guild_player_zhanji,
	erank_guild_zhanji_last,
	erank_guild_player_zhanji_last,
	erank_guild_end,
};

struct PvpMatch
{
	enum Type
	{
		TYPE_80,
		TYPE_80_TO_100,
		TYPE_100,
		TYPE_LESS80,
	};

	int64_t bf;
	uint64_t player_guid;

	bool operator == (const PvpMatch& rhs) const
	{
		return player_guid == rhs.player_guid;
	}

	bool matchless80(int64_t player_bf) const
	{
		if (player_bf > bf)
			return true;
		return false;
	}

	bool match80(int64_t player_bf, int64_t player_bf_min) const
	{
		if (player_bf > bf && bf >= player_bf_min)
			return true;
		return false;
	}

	bool match80to100(int64_t player_bf, int64_t player_bf_max) const
	{
		if (player_bf <= bf && bf <= player_bf_max)
			return true;
		return false;
	}

	bool match100(int64_t player_bf) const
	{
		if (player_bf <= bf)
			return true;
		return false;
	}

};


struct GuildZhanjiRank
{
	dhc::guild_arrange_t *ar;
	double zhanji;

	GuildZhanjiRank(dhc::guild_arrange_t *a,
		double z)
		:zhanji(z),
		ar(a)
	{

	}

	bool operator > (const GuildZhanjiRank &rhs) const
	{
		return zhanji > rhs.zhanji;
	}
};

class PvpPool
{
public:
	PvpPool();

	~PvpPool();

	void init();

	void fini();

	void update();

	void make_npc();

	void load_all_player();

	void load_all_player_callback(Request* req);

	void load_rank();

	void load_rank_callback(Request* req);

	void load_player(uint64_t player_guid, const std::string& name, int id);

	void load_player_callback(Request *req);

	void load_global();

	void load_global_callback(Request* req);

	void social_list_load();

	void social_list_load_callback(Request *req);

	void guild_list_load();

	void guild_list_load_callback(Request *req, int type);

	void load_player_check_end(dhc::player_t *player);

	void load_msg_callback(Request *req, dhc::player_t *player);

	void save_player(uint64_t guid, bool release);

	void save_all();

	void check_rank(dhc::player_t* player, rank_enum rank_type, int value);

	void clear_rank(dhc::rank_t *rank);

	void add_player(const rpcproto::tmsg_player_fight_player& fplayer);

	void get_player(rpcproto::tmsg_rep_pvp_match* match, uint64_t self_guid, int64_t bf);

	void copy_player_choose(dhc::player_t *player);

	int get_force(dhc::player_t *player) const;

	bool create_social_code(const rpcproto::tmsg_req_invite_code& msg);

	int input_social_code(uint64_t player_guid, int level, const std::string &code);

	int update_social_code(uint64_t player_guid, int level);

	bool get_social_code(uint64_t player_guid, rpcproto::tmsg_rep_invite_level* msg);

	void save_social_code(uint64_t social_code);

	//////////////////////////////////////////////////////////////////////////
	void guild_fight(uint64_t player_guild,
		uint64_t player_guid,
		uint64_t target_guild,
		int fight_index,
		rpcproto::tmsg_rep_guild_fight &fight);

	void guild_match(dhc::global_t *global, int cmd = 0);

	void guild_fight_end(dhc::global_t *glob, int cmd = 0);

	void guild_pvp_gm(int arg1, int arg2);

	void save_guild(dhc::guild_arrange_t *arrange, bool release = false);

	void guild_fight_gongpo(dhc::guild_arrange_t *guild_arrange, int &jundian, int &jidian, int &perfect);

private:
	uint64_t get_player(rpcproto::tmsg_rep_pvp_match* match,
		uint64_t self_guid,
		const std::set<uint64_t>& has_choose_guid,
		const std::set<uint64_t>& this_choose_guid,
		int64_t bf,
		PvpMatch::Type type);

	void add_pvp_match(uint64_t player_guid, int bf);

private:
	void check_guild_zhanji(dhc::guild_arrange_t *guild, bool add = false);

	void check_guild_player_zhanji(dhc::player_t *player, int value);

	int get_guild_zhanji(dhc::guild_arrange_t *guild);

	dhc::player_t*  get_guild_player(uint64_t self_guid, int64_t bf);

	void create_match(dhc::guild_arrange_t *arrange);

	void create_match(std::vector<dhc::guild_arrange_t*> &arranges);

	void create_match_new(dhc::guild_arrange_t *arrange, dhc::guild_arrange_t *arrange_match);

	dhc::guild_arrange_t* create_ai(dhc::guild_arrange_t *arrange);

	dhc::guild_arrange_t* create_random_ai();

	void reset_match(dhc::guild_arrange_t *arrange);

	int get_arrange_index(dhc::guild_arrange_t *arrange, uint64_t player_guid) const;

	dhc::guild_fight_t *get_guild_fight(dhc::guild_arrange_t *arrange, uint64_t target_guild);

	int get_fight_index(dhc::guild_fight_t *fight, uint64_t target_guid) const;

	int get_zhanji_index(dhc::guild_arrange_t *arrange, uint64_t player_guid) const;

	int can_fight(dhc::guild_fight_t *fight, int fight_index, int guard_index, const s_t_guildfight *t_guild_fight) const;

	bool has_gongpo(dhc::guild_fight_t *fight, int guard_index, const s_t_guildfight *t_guild_fight) const;

	bool all_gongpo(dhc::guild_fight_t *fight) const;

	double get_weight_point(dhc::guild_arrange_t *arrange);

	bool check_bushu(dhc::guild_arrange_t *arrange);

	void guild_fight_rank(dhc::global_t *global);

	void guild_pvp_clear(dhc::global_t *global);

	void guild_pvp_reward(dhc::global_t *global);

private:
	std::map<uint64_t, QueryMap> query_map_;
	std::vector<PvpMatch> player_matches_;
	std::map<uint64_t, std::set<uint64_t> > player_choose_;
	bool load_complete_;
	int load_count_;

	std::map<uint64_t, uint64_t> player_social_code_invites_;
	std::map<std::string, uint64_t> name_social_code_invites_;
	std::map<uint64_t, uint64_t> player_social_to_invites_;

	dhc::player_t *guild_npc_;
};
#endif //PVP_POOL_H