#ifndef __SPORT_POOL_H__
#define __SPORT_POOL_H__

#include "gameinc.h"

class SportPool
{
public:
	SportPool();

	~SportPool();

	void init();

	void fini();

	int get_now_rank(uint64_t player_guid);

	void set_now_rank(uint64_t player_guid, int rank);

	void update();

	int get_last_rank(uint64_t player_guid);

	dhc::player_t * get_sport_player(int rank);

protected:
	void init_sport();

	void reset();

	int load_sport_list_callback(Request *req, uint64_t sport_list_guid);

	int load_sport_list_callback1(Request *req, uint64_t sport_list_guid);

private:
	std::map<uint64_t, int> ranks_;
	std::map<uint64_t, int> now_ranks_;
	uint64_t time_;
	dhc::player_t *sport_player_;
	std::vector<dhc::role_t *> sport_roles_;
	std::vector<dhc::equip_t *> sport_equips_;
};

#define sSportPool (Singleton<SportPool>::instance())

#endif
