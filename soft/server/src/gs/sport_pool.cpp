#include "sport_pool.h"
#include "sport_config.h"
#include "role_config.h"
#include "player_def.h"
#include "utils.h"
#include "role_operation.h"

#define SPORT_UPDATE_TIME 60000

SportPool::SportPool()
: time_(0)
{

}

SportPool::~SportPool()
{

}

void SportPool::init()
{
	sport_player_ = new dhc::player_t();
	for (int i = 0; i < 6; ++i)
	{
		dhc::role_t *role = RoleOperation::role_create_normal();
		sport_player_->add_roles(role->guid());
		POOL_ADD_NEW(role->guid(), role);
		sport_roles_.push_back(role);
		sport_player_->add_zhenxing(0);
		sport_player_->add_duixing(i);
		for (int j = 0; j < 4; ++j)
		{
			dhc::equip_t *equip = new dhc::equip_t();
			uint64_t equip_guid = game::gtool()->assign(et_equip);
			equip->set_guid(equip_guid);
			sport_player_->add_equips(equip->guid());
			POOL_ADD_NEW(equip->guid(), equip);
			sport_equips_.push_back(equip);
			role->set_zhuangbeis(j, equip->guid());
			equip->set_role_guid(role->guid());
		}
	}

	time_ = game::timer()->now();
	{
		uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
		Request *req = new Request();
		req->add(opc_query, sport_list_guid, new dhc::sport_list_t);
		game::pool()->upcall(req, boost::bind(&SportPool::load_sport_list_callback, this,  _1, sport_list_guid));
	}
	{
		uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 1);
		Request *req = new Request();
		req->add(opc_query, sport_list_guid, new dhc::sport_list_t);
		game::pool()->upcall(req, boost::bind(&SportPool::load_sport_list_callback, this, _1, sport_list_guid));
	}
	{
		uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 100);
		Request *req = new Request();
		req->add(opc_query, sport_list_guid, new dhc::sport_list_t);
		game::pool()->upcall(req, boost::bind(&SportPool::load_sport_list_callback1, this, _1, sport_list_guid));
	}
}

int SportPool::load_sport_list_callback(Request *req, uint64_t sport_list_guid)
{
	if (req->success())
	{
		POOL_ADD(req->guid(), req->release_data());
		if (sport_list_guid == MAKE_GUID(et_sport_list, 1))
		{
			reset();
		}
		else
		{
			init_sport();
		}
	}
	else
	{
		dhc::sport_list_t *sport_list = new dhc::sport_list_t;
		sport_list->set_guid(sport_list_guid);
		sport_list->set_last_time(game::timer()->now());
		for (int i = 0; i < 5000; ++i)
		{
			s_t_sport_npc *t_npc = sSportConfig->get_sport_npc(i + 1);
			if (t_npc)
			{
				uint64_t npc_guid = game::gtool()->assign(et_npc);
				sport_list->add_player_guid(npc_guid);
				std::vector<uint32_t> roles;
				sport_list->add_player_template(sRoleConfig->get_random_role_id(Utils::get_int32(3, 4), roles));
				sport_list->add_player_name(sSportConfig->get_random_name());
				sport_list->add_player_level(Utils::get_int32(t_npc->level1, t_npc->level2));
				sport_list->add_player_bat_eff(Utils::get_int32(t_npc->bf1, t_npc->bf2));
				sport_list->add_player_isnpc(1);
				sport_list->add_player_vip(0);
				sport_list->add_player_achieve(0);
				sport_list->add_player_chenghao(0);
				sport_list->add_nalflag(0);
			}
		}
		POOL_ADD_NEW(sport_list_guid, sport_list);
	}

	return 0;
}

int SportPool::load_sport_list_callback1(Request *req, uint64_t sport_list_guid)
{
	if (req->success())
	{
		POOL_ADD(req->guid(), req->release_data());
	}
	else
	{
		dhc::sport_list_t *sport_list = new dhc::sport_list_t;
		sport_list->set_guid(sport_list_guid);
		sport_list->set_last_time(0);
		POOL_ADD_NEW(sport_list_guid, sport_list);
		POOL_SAVE(dhc::sport_list_t, sport_list, false);
	}

	return 0;
}

void SportPool::fini()
{
	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (sport_list)
	{
		POOL_SAVE(dhc::sport_list_t, sport_list, false);
	}

	uint64_t sport_list_guid1 = MAKE_GUID(et_sport_list, 1);
	dhc::sport_list_t *sport_list1 = POOL_GET_SPORT_LIST(sport_list_guid1);
	if (sport_list1)
	{
		POOL_SAVE(dhc::sport_list_t, sport_list1, false);
	}

	uint64_t sport_list_guid100 = MAKE_GUID(et_sport_list, 100);
	dhc::sport_list_t *sport_list100 = POOL_GET_SPORT_LIST(sport_list_guid100);
	if (sport_list100)
	{
		POOL_SAVE(dhc::sport_list_t, sport_list100, false);
	}
}

void SportPool::init_sport()
{
	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		return;
	}
	for (int i = 0; i < sport_list->player_guid_size(); ++i)
	{
		now_ranks_[sport_list->player_guid(i)] = i + 1;
	}
}

void SportPool::reset()
{
	uint64_t sport_list_guid1 = MAKE_GUID(et_sport_list, 1);
	dhc::sport_list_t *sport_list1 = POOL_GET_SPORT_LIST(sport_list_guid1);
	if (!sport_list1)
	{
		return;
	}

	ranks_.clear();
	for (int i = 0; i < sport_list1->player_guid_size(); ++i)
	{
		ranks_[sport_list1->player_guid(i)] = i + 1;
	}
}

int SportPool::get_now_rank(uint64_t player_guid)
{
	if (now_ranks_.find(player_guid) == now_ranks_.end())
	{
		return 0;
	}
	return now_ranks_[player_guid];
}

void SportPool::set_now_rank(uint64_t player_guid, int rank)
{
	now_ranks_[player_guid] = rank;
}

void SportPool::update()
{
	uint64_t now = game::timer()->now();

	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		return;
	}

	uint64_t sport_list_guid1 = MAKE_GUID(et_sport_list, 1);
	dhc::sport_list_t *sport_list1 = POOL_GET_SPORT_LIST(sport_list_guid1);
	if (!sport_list1)
	{
		return;
	}
	
	if (now - time_ >= SPORT_UPDATE_TIME)
	{
		POOL_SAVE(dhc::sport_list_t, sport_list, false);
		POOL_SAVE(dhc::sport_list_t, sport_list1, false);
		time_ = now;
	}

	if (game::timer()->trigger_time(sport_list->last_time(), 21, 0))
	{
		sport_list->set_last_time(game::timer()->now());
		sport_list1->CopyFrom(*sport_list);
		sport_list1->set_guid(sport_list_guid1);

		POOL_SAVE(dhc::sport_list_t, sport_list, false);
		POOL_SAVE(dhc::sport_list_t, sport_list1, false);
		this->reset();
	}
}

int SportPool::get_last_rank(uint64_t player_guid)
{
	if (ranks_.find(player_guid) == ranks_.end())
	{
		return 0;
	}
	return ranks_[player_guid];
}

dhc::player_t * SportPool::get_sport_player(int rank)
{
	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		return 0;
	}
	sport_player_->set_guid(sport_list->player_guid(rank - 1));
	sport_player_->set_name(sport_list->player_name(rank - 1));
	sport_player_->set_level(sport_list->player_level(rank - 1));
	sport_player_->set_template_id(sport_list->player_template(rank - 1));
	for (int i = 0; i < 6; ++i)
	{
		sport_player_->set_zhenxing(i, 0);
	}

	s_t_sport_npc *npc = sSportConfig->get_sport_npc(rank);
	if (!npc)
	{
		return 0;
	}

	std::vector<uint32_t> rdroles;
	for (int i = 0; i < npc->npc_subs.size(); ++i)
	{
		s_t_sport_npc_sub &npc_sub = npc->npc_subs[i];
		if (npc_sub.type == 0)
		{
			continue;
		}
		dhc::role_t *role = sport_roles_[i];
		if (npc_sub.type == 1)
		{
			role->set_template_id(npc_sub.id);
		}
		else if (npc_sub.type == 2)
		{
			role->set_template_id(sRoleConfig->get_random_role_id(3, rdroles));
		}
		else
		{
			role->set_template_id(sRoleConfig->get_random_role_id(4, rdroles));
		}
		rdroles.push_back(role->template_id());
		role->set_level(npc_sub.level);
		role->set_glevel(npc_sub.glevel);
		role->set_jlevel(npc_sub.glevel);
		s_t_role *t_role = sRoleConfig->get_role(npc_sub.id);
		if (!t_role)
		{
			continue;
		}
		role->set_pinzhi(t_role->pinzhi * 100);
		sport_player_->set_zhenxing(i, role->guid());

		for (int km = 0; km < role->zhuangbeis_size(); ++km)
		{
			dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(km));
			if (equip)
			{
				equip->set_template_id(0);
				equip->set_enhance(0);
			}
		}
	}
	for (int i = 0; i < npc->npc_subs1.size(); ++i)
	{
		s_t_sport_npc_sub1 &npc_sub1 = npc->npc_subs1[i];
		if (npc_sub1.id == 0)
		{
			continue;
		}
		dhc::equip_t *equip = sport_equips_[i];
		equip->set_template_id(npc_sub1.id);
		equip->set_enhance(npc_sub1.enhance);
	}
	return sport_player_;
}
