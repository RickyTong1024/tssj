#ifndef _RANK_POOL_H_
#define _RANK_POOL_H_

#include "gameinc.h"

class RankPool
{
public:
	RankPool();

	~RankPool();

	int init();

	int fini();

	int update(ACE_Time_Value tv);

public:
	void load_ranking_list();

	void load_ranking_list_callback(Request *req, uint64_t rank_guid);

	void save_ranking_list(bool release);

private:
	uint64_t last_time_;
};

#define sRankPool (Singleton<RankPool>::instance())

#endif
