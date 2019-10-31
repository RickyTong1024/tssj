#ifndef __MISSION_POOL_H__
#define __MISSION_POOL_H__

#include "gameinc.h"

struct yb_player_item
{
	uint64_t player_guid;
	int player_id;
};

class MissionPool
{
public:
	MissionPool();

	~MissionPool();

	void start_yb(dhc::player_t *player);

	void refresh_yb(dhc::player_t *player);

	void remove_yb(dhc::player_t *player);

	uint64_t get_yb_finish_time(dhc::player_t *player);

	bool is_start_yb(dhc::player_t *player);

	bool is_finish_yb(dhc::player_t *player);

	bool is_jiasu_yb(dhc::player_t *player);

	void update();

	int get_yb_players(dhc::player_t *player, std::vector<protocol::game::msg_yb_player> &yb_players, int player_id, int num = 9999);

	void add_yb_info(int type, const std::string &player_name, const std::string &target_name, int yb_type, int yuanli);

	void get_yb_info(std::list<protocol::game::msg_yb_info> &infos);

	void add_ybq_info(dhc::player_t *player, int type, const std::string &player_name, int yuanli);

	void get_ybq_info(uint64_t player_guid, std::list<protocol::game::msg_ybq_info> &infos);

	bool is_yb_effect(dhc::player_t *player);

	void init();

private:
	int yb_player_id_;
	std::list<yb_player_item> yb_players_;
	int yb_info_id_;
	std::list<protocol::game::msg_yb_info> yb_infos_;
	int ybq_info_id_;
	std::map<uint64_t, std::list<protocol::game::msg_ybq_info> > player_ybq_infos_;
	uint64_t time_;
	int load_num_;
};

#define sMissionPool (Singleton<MissionPool>::instance())

#endif
