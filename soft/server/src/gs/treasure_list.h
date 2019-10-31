#ifndef __TREASURE_LIST_HPP__
#define __TREASURE_LIST_HPP__

#include "gameinc.h"

struct RobPlayer
{
	uint64_t guid_;
	std::string name_;
	int template_id_;
	int level_;
	int bf_;
	int rate_;
	bool is_npc_;
	int vip_;
	int achieve_;
	int chenghao_;
	int nalflag;

	RobPlayer(uint64_t guid,
		const std::string &name,
		int template_id,
		int level,
		int bf,
		int rate,
		bool npc,
		int vip,
		int achieve,
		int chenghao,
		int nalflag)
		:guid_(guid),
		name_(name),
		template_id_(template_id),
		level_(level),
		bf_(bf),
		rate_(rate),
		is_npc_(npc),
		vip_(vip),
		achieve_(achieve),
		chenghao_(chenghao),
		nalflag(nalflag)
	{

	}
};

class RobPlayerList
{
public:
	RobPlayerList()
		:suipian_id_(0)
	{

	}

	RobPlayerList(int suipian_id)
		:suipian_id_(suipian_id)
	{

	}

	int get_suipian() const
	{
		return suipian_id_;
	}

	void add_rob_player(const RobPlayer &rp)
	{
		rob_player_list_.push_back(rp);
	}

	void get_rob_player_list(protocol::game::smsg_treasure_rob_view &view_list);

	RobPlayer* get_rob_player(uint64_t guid);

	void modify_first_suipian(int level, int bf);

private:
	int suipian_id_;
	std::vector<RobPlayer> rob_player_list_;
};

struct RobSuipianList
{
	std::list<int> suipian_ids;
};

class TreasureList
{
public:
	int init();

	int fini();

	int update(const ACE_Time_Value& tv);

public:
	void get_rob_player_list(dhc::player_t *player, 
		int suipian_id, 
		protocol::game::smsg_treasure_rob_view &view_list);

	void set_rob_player_list(dhc::player_t *player, int suipian_id);

	void reset_rob_player_list(dhc::player_t *player);

	void update_rob_player_list(dhc::player_t *player);

	void clear_rob_player_list(dhc::player_t *player);

	RobPlayerList* get_rob_player_list(dhc::player_t *player);

	RobPlayer *get_rob_player(dhc::player_t *player, uint64_t guid);

	dhc::player_t *get_rob_fight_npc(dhc::player_t* player);

	void update_rob_suipian_list(uint64_t guid, int suipian_id);

	void remove_rob_suipian_list(uint64_t guid);

	void get_rob_suipian_list(uint64_t guid, std::vector<int> &suipian_id) const;

	bool has_role_suipian_list(dhc::player_t *player) const;

private:
	RobPlayerList* new_rob_player_list(dhc::player_t *player, int suipian_id);

	void reset_rob_treasure_list(dhc::player_t *player, int treasure_id);
private:
	void load_treasure_list();

	void load_treasure_list_callback(Request *req, int id, uint64_t guid);

	void save_treasure_list(int num, bool release);

	void new_npc();

private:
	std::map<uint64_t, RobPlayerList> player_list_;

	dhc::player_t *npc_;
	dhc::player_t *fake_npc_;

	std::map<uint64_t, RobSuipianList> suipian_list_;
};

#define sTreasureList (Singleton<TreasureList>::instance())
#endif //__TREASURE_LIST_HPP__