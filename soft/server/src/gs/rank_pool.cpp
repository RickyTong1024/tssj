#include "rank_pool.h"
#include "rank_operation.h"

RankPool::RankPool()
{
	last_time_ = 0;
}

RankPool::~RankPool()
{

}

int RankPool::init()
{
	load_ranking_list();
	return 0;
}

int RankPool::fini()
{
	save_ranking_list(true);
	return 0;
}

void RankPool::load_ranking_list()
{
	for (int i = e_rank_type_level; i != e_rank_type_end; ++i)
	{
		uint64_t rank_guid = MAKE_GUID(et_rank, i);
		Request *req = new Request();
		req->add(opc_query, rank_guid, new dhc::rank_t);
		game::pool()->upcall(req, boost::bind(&RankPool::load_ranking_list_callback, this, _1, rank_guid));
	}
}

void RankPool::load_ranking_list_callback(Request *req, uint64_t rank_guid)
{
	dhc::rank_t *rank = NULL;
	if (req->success())
	{
		rank = (dhc::rank_t*)req->release_data();
		POOL_ADD(req->guid(), rank);
	}
	else
	{
		rank = new dhc::rank_t;
		rank->set_guid(rank_guid);
		POOL_ADD_NEW(req->guid(), rank);
	}
}

void RankPool::save_ranking_list(bool release)
{
	for (int i = e_rank_type_level; i != e_rank_type_end; ++i)
	{
		uint64_t rank_guid = MAKE_GUID(et_rank, i);
		dhc::rank_t *rank = POOL_GET_RANK(rank_guid);
		if (!rank)
		{
			continue;
		}
		POOL_SAVE(dhc::rank_t, rank, release);
	}
}

int RankPool::update(ACE_Time_Value tv)
{
	save_ranking_list(false);
	return 0;
}
