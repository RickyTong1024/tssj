#include "global_pool.h"
#include "rank_operation.h"
#include "social_operation.h"
#include "post_operation.h"
#include "sport_config.h"
#include "item_config.h"
#include "huodong_config.h"
#include "utils.h"
#include "role_operation.h"

GlobalPool::GlobalPool()
{

}


GlobalPool::~GlobalPool()
{

}


int GlobalPool::init()
{
	load_global();
	return 0;
}

int GlobalPool::fini()
{
	save_global(true);
	return 0;
}

int GlobalPool::update(const ACE_Time_Value& tv)
{
	save_global(false);
	return 0;
}

void GlobalPool::load_global()
{
	uint64_t global_guid = MAKE_GUID(et_global, 0);
	Request *req = new Request();
	req->add(opc_query, global_guid, new dhc::global_t());
	game::pool()->upcall(req, boost::bind(&GlobalPool::load_global_callback, this, _1, global_guid));
}

void GlobalPool::load_global_callback(Request *req, uint64_t global_guid)
{
	dhc::global_t *global = 0;
	if (req->success())
	{
		global = (dhc::global_t*)req->release_data();
		POOL_ADD(global->guid(), global);
	}
	else
	{
		global = new dhc::global_t();
		global->set_guid(global_guid);
		POOL_ADD_NEW(global_guid, global);
	}

	std::vector<std::pair<int, int> > xgs;
	sHuodongConfig->get_t_huodong_all_kaifu_xg(xgs);
	for (int i = 0; i < xgs.size(); ++i)
	{
		bool has = false;
		for (int j = 0; j < global->kaifu_xg_id_size(); ++j)
		{
			if (global->kaifu_xg_id(j) == xgs[i].first)
			{
				has = true;
				break;
			}
		}
		if (!has)
		{
			global->add_kaifu_xg_id(xgs[i].first);
			global->add_kaifu_xg_count(0);
		}
	}
}

void GlobalPool::save_global(bool release)
{
	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (global)
	{
		POOL_SAVE(dhc::global_t, global, release);
	}
}

void GlobalPool::update_pttq(const std::string &name, int id, int vip)
{
	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		return;
	}

	for (int i = 0; i < global->pttq_vip_id_size(); ++i)
	{
		if (global->pttq_vip_id(i) == id)
		{
			return;
		}
	}
	global->add_pttq_vip_id(id);
	global->add_pttq_player_name(name);
	save_global(false);
}

void GlobalPool::update_random_pttq(int id)
{
	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		return;
	}

	for (int i = 0; i < global->pttq_vip_id_size(); ++i)
	{
		if (global->pttq_vip_id(i) == id)
		{
			return;
		}
	}
	global->add_pttq_vip_id(id);
	global->add_pttq_player_name(sSportConfig->get_random_name());
	save_global(false);
}

bool GlobalPool::is_enable_pttq(int id) const
{
	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		return false;
	}

	for (int i = 0; i < global->pttq_vip_id_size(); ++i)
	{
		if (global->pttq_vip_id(i) == id)
		{
			return true;
		}
	}

	return false;
}


int GlobalPool::get_kaifu_xg_count(int id) const
{
	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		return 0;
	}

	for (int i = 0; i < global->kaifu_xg_id_size(); ++i)
	{
		if (global->kaifu_xg_id(i) == id)
		{
			return global->kaifu_xg_count(i);
		}
	}

	return 0;
}

void GlobalPool::update_kaifu_xg_count(int id)
{
	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		return;
	}

	for (int i = 0; i < global->kaifu_xg_id_size(); ++i)
	{
		if (global->kaifu_xg_id(i) == id)
		{
			global->set_kaifu_xg_count(i, global->kaifu_xg_count(i) + 1);
			break;
		}
	}
}