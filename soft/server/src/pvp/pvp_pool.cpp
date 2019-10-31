#include "pvp_pool.h"
#include <math.h>
#include "rpc.pb.h"
#include "utils.h"
#include "sport_config.h"
#include "role_config.h"
#include "role_operation.h"
#include "guild_config.h"
#include "mission_fight.h"


static const int rank_range[erank_end] = { 100 };

static void _check_player(dhc::player_t* player, const rpcproto::tmsg_player_fight_player* fplayer)
{
	std::set<uint64_t> roles;
	std::set<uint64_t> equips;
	std::set<uint64_t> treasures;
	std::set<uint64_t> pets;
	for (int i = 0; i < fplayer->roles_size(); ++i)
	{
		roles.insert(fplayer->roles(i).guid());
	}
	for (int i = 0; i < fplayer->equips_size(); ++i)
	{
		equips.insert(fplayer->equips(i).guid());
	}
	for (int i = 0; i < fplayer->treasures_size(); ++i)
	{
		treasures.insert(fplayer->treasures(i).guid());
	}
	for (int i = 0; i < fplayer->pets_size(); ++i)
	{
		pets.insert(fplayer->pets(i).guid());
	}

	for (int i = 0; i < player->roles_size(); ++i)
	{
		if (roles.find(player->roles(i)) == roles.end())
		{
			POOL_REMOVE(player->roles(i), 0);
		}
	}

	for (int i = 0; i < player->equips_size(); ++i)
	{
		if (equips.find(player->equips(i)) == equips.end())
		{
			POOL_REMOVE(player->equips(i), 0);
		}
	}

	for (int i = 0; i < player->treasures_size(); ++i)
	{
		if (treasures.find(player->treasures(i)) == treasures.end())
		{
			POOL_REMOVE(player->treasures(i), 0);
		}
	}

	for (int i = 0; i < player->pets_size(); ++i)
	{
		if (pets.find(player->pets(i)) == pets.end())
		{
			POOL_REMOVE(player->pets(i), 0);
		}
	}
}
template<typename T1, typename T2>
static void _copy_role(const T1* frole, T2* role)
{
	role->set_guid(frole->guid());
	role->set_player_guid(frole->player_guid());
	role->set_template_id(frole->template_id());
	role->set_level(frole->level());
	role->set_jlevel(frole->jlevel());
	role->set_glevel(frole->glevel());
	role->set_pinzhi(frole->pinzhi());
	role->set_dress_on_id(frole->dress_on_id());
	role->set_bskill_level(frole->bskill_level());
	role->set_pet(frole->pet());
	role->clear_jskill_level();
	for (int i = 0; i < frole->jskill_level_size(); ++i)
	{
		role->add_jskill_level(frole->jskill_level(i));
	}
	role->clear_dress_ids();
	for (int i = 0; i < frole->dress_ids_size(); ++i)
	{
		role->add_dress_ids(frole->dress_ids(i));
	}
	role->clear_zhuangbeis();
	for (int i = 0; i < frole->zhuangbeis_size(); ++i)
	{
		role->add_zhuangbeis(frole->zhuangbeis(i));
	}
	role->clear_treasures();
	for (int i = 0; i < frole->treasures_size(); ++i)
	{
		role->add_treasures(frole->treasures(i));
	}
}

template<typename T1, typename T2>
static void _copy_equip(const T1* fequip, T2* equip)
{
	equip->set_guid(fequip->guid());
	equip->set_player_guid(fequip->player_guid());
	equip->set_template_id(fequip->template_id());
	equip->set_role_guid(fequip->role_guid());
	equip->set_enhance(fequip->enhance());
	equip->clear_rand_ids();
	equip->clear_rand_values();
	for (int i = 0; i < fequip->rand_ids_size(); ++i)
	{
		equip->add_rand_ids(fequip->rand_ids(i));
		equip->add_rand_values(fequip->rand_values(i));
	}
	equip->set_jilian(fequip->jilian());
	equip->set_star(fequip->star());
}

template<typename T1, typename T2>
static void _copy_treasure(const T1* ftreasure, T2* treasure)
{
	treasure->set_guid(ftreasure->guid());
	treasure->set_player_guid(ftreasure->player_guid());
	treasure->set_template_id(ftreasure->template_id());
	treasure->set_role_guid(ftreasure->role_guid());
	treasure->set_enhance(ftreasure->enhance());
	treasure->set_jilian(ftreasure->jilian());
	treasure->set_star(ftreasure->star());
	treasure->set_star_exp(ftreasure->star_exp());
}

template<typename T1, typename T2>
static void _copy_pet(const T1* fpet, T2* pet)
{
	pet->set_guid(fpet->guid());
	pet->set_player_guid(fpet->player_guid());
	pet->set_template_id(fpet->template_id());
	pet->set_level(fpet->level());
	pet->set_jinjie(fpet->jinjie());
	pet->set_star(fpet->star());
	pet->set_role_guid(fpet->role_guid());
	pet->clear_jinjie_slot();
	for (int i = 0; i < fpet->jinjie_slot_size(); ++i)
	{
		pet->add_jinjie_slot(fpet->jinjie_slot(i));
	}
}

static void _copy_player(const rpcproto::tmsg_player_fight_player* fplayer, dhc::player_t* player)
{
	player->set_guid(fplayer->guid());
	player->set_serverid(fplayer->serverid());
	player->set_name(fplayer->name());
	player->set_template_id(fplayer->template_id());
	player->set_level(fplayer->level());
	player->set_bf(fplayer->bf());
	player->set_zsname(fplayer->guild_name());
	player->set_guild(fplayer->guild());
	player->set_pvp_total(fplayer->pvp_total());
	player->set_duixing_id(fplayer->duixing_id());
	player->set_vip(fplayer->vip());
	player->set_duixing_level(fplayer->duixing_level());
	player->set_guanghuan_id(fplayer->guanghuan_id());
	player->set_mission(fplayer->template_dress());
	player->set_chenghao_on(fplayer->chenghao_on());
	player->set_huiyi_shoujidu(fplayer->huiyi_shoujidu());
	player->set_pet_on(fplayer->pet_on());
	player->set_nalflag(fplayer->nalflag());
	player->clear_guild_skill_ids();
	player->clear_guild_skill_levels();
	for (int i = 0; i < fplayer->guild_skill_ids_size(); ++i)
	{
		player->add_guild_skill_ids(fplayer->guild_skill_ids(i));
		player->add_guild_skill_levels(fplayer->guild_skill_levels(i));
	}
	player->clear_dress_ids();
	for (int i = 0; i < fplayer->dress_ids_size(); ++i)
	{
		player->add_dress_ids(fplayer->dress_ids(i));
	}
	player->clear_dress_achieves();
	for (int i = 0; i < fplayer->dress_achieves_size(); ++i)
	{
		player->add_dress_achieves(fplayer->dress_achieves(i));
	}

	player->clear_zhenxing();
	for (int i = 0; i < fplayer->zhenxing_size(); ++i)
	{
		player->add_zhenxing(fplayer->zhenxing(i));
	}

	player->clear_duixing();
	for (int i = 0; i < fplayer->duixing_size(); ++i)
	{
		player->add_duixing(fplayer->duixing(i));
	}

	player->clear_houyuan();
	for (int i = 0; i < fplayer->houyuan_size(); ++i)
	{
		player->add_houyuan(fplayer->houyuan(i));
	}

	player->clear_huiyi_jihuos();
	for (int i = 0; i < fplayer->huiyi_jihuos_size(); ++i)
	{
		player->add_huiyi_jihuos(fplayer->huiyi_jihuos(i));
	}

	player->clear_huiyi_jihuo_starts();
	for (int i = 0; i < fplayer->huiyi_jihuo_starts_size(); ++i)
	{
		player->add_huiyi_jihuo_starts(fplayer->huiyi_jihuo_starts(i));
	}

	player->clear_guanghuan();
	player->clear_guanghuan_level();
	for (int i = 0; i < fplayer->guanghuan_size(); ++i)
	{
		player->add_guanghuan(fplayer->guanghuan(i));
		player->add_guanghuan_level(fplayer->guanghuan_level(i));
	}

	player->clear_chenghao();
	for (int i = 0; i < fplayer->chenghao_size(); ++i)
	{
		player->add_chenghao(fplayer->chenghao(i));
	}

	player->clear_guild_applys();

	player->clear_roles();
	dhc::role_t* role = 0;
	for (int i = 0; i < fplayer->roles_size(); ++i)
	{
		const rpcproto::tmsg_fight_role& frole = fplayer->roles(i);
		player->add_roles(frole.guid());
		role = POOL_GET_ROLE(frole.guid());
		if (!role)
		{
			role = new dhc::role_t();
			POOL_ADD_NEW(frole.guid(), role);
		}
		_copy_role(&frole, role);
	}

	player->clear_equips();
	dhc::equip_t* equip = 0;
	for (int i = 0; i < fplayer->equips_size(); ++i)
	{
		const rpcproto::tmsg_fight_equip& fequip = fplayer->equips(i);
		player->add_equips(fequip.guid());
		equip = POOL_GET_EQUIP(fequip.guid());
		if (!equip)
		{
			equip = new dhc::equip_t();
			POOL_ADD_NEW(fequip.guid(), equip);
		}
		_copy_equip(&fequip, equip);
	}

	player->clear_treasures();
	dhc::treasure_t* treasure = 0;
	for (int i = 0; i < fplayer->treasures_size(); ++i)
	{
		const rpcproto::tmsg_fight_treasure& ftreasure = fplayer->treasures(i);
		player->add_treasures(ftreasure.guid());
		treasure = POOL_GET_TREASURE(ftreasure.guid());
		if (!treasure)
		{
			treasure = new dhc::treasure_t();
			POOL_ADD_NEW(ftreasure.guid(), treasure);
		}
		_copy_treasure(&ftreasure, treasure);
	}

	player->clear_pets();
	dhc::pet_t* pet = 0;
	for (int i = 0; i < fplayer->pets_size(); ++i)
	{
		const rpcproto::tmsg_fight_pet& fpet = fplayer->pets(i);
		player->add_pets(fpet.guid());
		pet = POOL_GET_PET(fpet.guid());
		if (!pet)
		{
			pet = new dhc::pet_t();
			POOL_ADD_NEW(fpet.guid(), pet);
		}
		_copy_pet(&fpet, pet);
	}
}


PvpPool::PvpPool()
{
	
}

PvpPool::~PvpPool()
{

}


void PvpPool::init()
{
	load_complete_ = false;
	load_count_ = 1;
	make_npc();
	load_global();
	load_rank();
	social_list_load();
	load_all_player();
	guild_list_load();
}

void PvpPool::fini()
{
	save_all();
}

void PvpPool::update()
{
	if (load_complete_ == false)
	{
		load_count_++;
		if (load_count_ >= 6)
		{
			load_complete_ = true;
		}
	}

	for (int i = erank_point; i < erank_end; ++i)
	{
		dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, i));
		if (rank)
		{
			POOL_SAVE(dhc::rank_t, rank, false);
		}
	}

	for (int i = erank_point_last; i < erank_point_last_end; ++i)
	{
		dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, i));
		if (rank)
		{
			POOL_SAVE(dhc::rank_t, rank, false);
		}
	}

	for (int i = erank_guild_zhanji; i < erank_guild_end; ++i)
	{
		dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, i));
		if (rank)
		{
			POOL_SAVE(dhc::rank_t, rank, false);
		}
	}

	dhc::global_t* glob = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (glob)
	{
		// 10点
		if (game::timer()->trigger_time(glob->ore_rank_time(), 10, 0))
		{
			glob->set_ore_rank_time(game::timer()->now());
			if (glob->guild_pvp_zhou() % 2 == 0 && game::timer()->weekday() >= 2)
			{
				guild_match(glob);
			}
		}

		// 0点
		if (game::timer()->trigger_time(glob->guild_refresh_time(), 0, 0))
		{
			glob->set_guild_refresh_time(game::timer()->now());
			guild_fight_end(glob);

			if (game::timer()->weekday() == 5)
			{
				dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_point));
				if (rank)
				{
					dhc::rank_t* rank1 = POOL_GET_RANK(MAKE_GUID(et_rank, erank_point_last));
					if (!rank1)
					{
						rank1 = new dhc::rank_t();
						rank1->set_guid(MAKE_GUID(et_rank, erank_point_last));
						POOL_ADD_NEW(rank1->guid(), rank1);
					}
					rank1->CopyFrom(*rank);
					rank1->set_guid(MAKE_GUID(et_rank, erank_point_last));
					rank1->set_reward_flag(2);
					clear_rank(rank);
				}
				player_choose_.clear();
			}
			if (game::timer()->weekday() == 6)
			{
				dhc::rank_t* rank1 = POOL_GET_RANK(MAKE_GUID(et_rank, erank_point_last));
				if (rank1)
				{
					rank1->set_reward_flag(0);
				}
			}
			if (game::timer()->weekday() == 2)
			{
				for (int kmg = erank_guild_zhanji_last; kmg <= erank_guild_player_zhanji_last; ++kmg)
				{
					dhc::rank_t* rank1 = POOL_GET_RANK(MAKE_GUID(et_rank, kmg));
					if (rank1)
					{
						rank1->set_reward_flag(0);
					}
				}
			}
		}
		POOL_SAVE(dhc::global_t, glob, false);
	}
}

void PvpPool::make_npc()
{
	for (int k = 0; k < 3; ++k)
	{
		uint64_t player_guid = MAKE_GUID(et_player, k);
		dhc::player_t *npc = new dhc::player_t();
		npc->set_guid(player_guid);
		npc->set_name(sSportConfig->get_random_name());
		npc->set_bf(Utils::get_int32(500000, 1000000));
		npc->set_template_id(100);
		POOL_ADD(npc->guid(), npc);
	
		for (int i = 0; i < 6; ++i)
		{
			int role_id = 201 + i;
			const s_t_role* t_role = sRoleConfig->get_role(role_id);
			if (t_role)
			{
				uint64_t role_guid = game::gtool()->assign(et_role);
				dhc::role_t *role = new dhc::role_t();
				role->set_guid(role_guid);
				role->set_template_id(role_id);
				role->set_level(60);
				role->set_pinzhi(t_role->pinzhi * 100);
				while (role->jskill_level_size() < 7)
				{
					role->add_jskill_level(1);
				}
				role->add_dress_ids(0);
				role->set_dress_on_id(0);
				for (int j = 0; j < 4; ++j)
				{
					role->add_zhuangbeis(0);
				}
				for (int j = 0; j < 2; ++j)
				{
					role->add_treasures(0);
				}
				npc->add_roles(role->guid());
				npc->add_zhenxing(role->guid());
				npc->add_duixing(i);
				POOL_ADD(role->guid(), role);
			}
		}
	}

	{
		uint64_t player_guid = MAKE_GUID(et_player, 1000);
		guild_npc_ = new dhc::player_t();
		guild_npc_->set_guid(player_guid);
		guild_npc_->set_name(sSportConfig->get_random_name());
		guild_npc_->set_bf(Utils::get_int32(500000, 1000000));
		guild_npc_->set_template_id(100);
		POOL_ADD(guild_npc_->guid(), guild_npc_);

		for (int i = 0; i < 6; ++i)
		{
			int role_id = 201 + i;
			const s_t_role* t_role = sRoleConfig->get_role(role_id);
			if (t_role)
			{
				uint64_t role_guid = game::gtool()->assign(et_role);
				dhc::role_t *role = new dhc::role_t();
				role->set_guid(role_guid);
				role->set_template_id(role_id);
				role->set_level(60);
				role->set_pinzhi(t_role->pinzhi * 100);
				while (role->jskill_level_size() < 7)
				{
					role->add_jskill_level(1);
				}
				role->add_dress_ids(0);
				role->set_dress_on_id(0);
				for (int j = 0; j < 4; ++j)
				{
					role->add_zhuangbeis(0);
				}
				for (int j = 0; j < 2; ++j)
				{
					role->add_treasures(0);
				}
				guild_npc_->add_roles(role->guid());
				guild_npc_->add_zhenxing(role->guid());
				guild_npc_->add_duixing(i);
				POOL_ADD(role->guid(), role);
			}
		}
	}
}

void PvpPool::load_all_player()
{
	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_player_guids_list, 0), new protocol::game::player_guids_list_t);
	game::pool()->upcall(req, boost::bind(&PvpPool::load_all_player_callback, this, _1));
}

void PvpPool::load_all_player_callback(Request* req)
{
	if (req->success())
	{
		protocol::game::player_guids_list_t* guid_lists = (protocol::game::player_guids_list_t*)req->data();
		for (int i = 0; i < guid_lists->player_guids_size(); ++i)
		{
			load_player(guid_lists->player_guids(i), "", 0);
		}
	}
}

void PvpPool::load_rank()
{
	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_rank, erank_point), new dhc::rank_t);
	game::pool()->upcall(req, boost::bind(&PvpPool::load_rank_callback, this, _1));

	Request *req1 = new Request();
	req1->add(opc_query, MAKE_GUID(et_rank, erank_point_last), new dhc::rank_t);
	game::pool()->upcall(req1, boost::bind(&PvpPool::load_rank_callback, this, _1));

	for (int i = erank_guild_zhanji; i < erank_guild_end; ++i)
	{
		Request *req1 = new Request();
		req1->add(opc_query, MAKE_GUID(et_rank, i), new dhc::rank_t);
		game::pool()->upcall(req1, boost::bind(&PvpPool::load_rank_callback, this, _1));
	}
}

void PvpPool::load_rank_callback(Request* req)
{
	if (req->success())
	{
		dhc::rank_t* rank = (dhc::rank_t*)req->release_data();
		POOL_ADD(req->guid(), rank);
	}
	else
	{
		dhc::rank_t *rank = new dhc::rank_t();
		rank->set_guid(req->guid());
		POOL_ADD_NEW(req->guid(), rank);
		POOL_SAVE(dhc::rank_t, rank, false);
	}
}

void PvpPool::load_player(uint64_t player_guid, const std::string& name, int id)
{
	if (query_map_.find(player_guid) != query_map_.end())
	{
		return;
	}
	QueryMap qm;
	qm.query_num = 1;
	qm.name = "";
	qm.id = 0;
	if (id != 0)
	{
		qm.params.push_back(id);
		qm.msgs.push_back(name);
	}
	query_map_[player_guid] = qm;
	Request *req = new Request();
	req->add(opc_query, player_guid, new dhc::player_t);
	game::pool()->upcall(req, boost::bind(&PvpPool::load_player_callback, this, _1));
}

void PvpPool::load_player_callback(Request *req)
{
	if (req->success())
	{
		dhc::player_t *player = (dhc::player_t *)req->release_data();
		QueryMap &qm = query_map_[player->guid()];

		for (int i = 0; i < player->roles_size(); ++i)
		{
			if (player->roles(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->roles(i), new dhc::role_t);
				game::pool()->upcall(req, boost::bind(&PvpPool::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->equips_size(); ++i)
		{
			if (player->equips(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->equips(i), new dhc::equip_t);
				game::pool()->upcall(req, boost::bind(&PvpPool::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->treasures_size(); ++i)
		{
			if (player->treasures(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->treasures(i), new dhc::treasure_t);
				game::pool()->upcall(req, boost::bind(&PvpPool::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->pets_size(); ++i)
		{
			if (player->pets(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->pets(i), new dhc::pet_t);
				game::pool()->upcall(req, boost::bind(&PvpPool::load_msg_callback, this, _1, player));
			}
		}

		load_player_check_end(player);
	}
}

void PvpPool::load_global()
{
	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_global, 0), new dhc::global_t);
	game::pool()->upcall(req, boost::bind(&PvpPool::load_global_callback, this, _1));
}

void PvpPool::load_global_callback(Request* req)
{
	if (req->success())
	{
		dhc::global_t* glob = (dhc::global_t*)req->release_data();
		POOL_ADD(req->guid(), glob);
	}
	else
	{
		dhc::global_t *glob = new dhc::global_t();
		glob->set_guid(req->guid());
		glob->set_guild_refresh_time(game::timer()->now());
		glob->set_ore_rank_time(game::timer()->now());
		POOL_ADD_NEW(req->guid(), glob);
		POOL_SAVE(dhc::global_t, glob, false);
	}
}

void PvpPool::social_list_load()
{
	uint64_t social_list_guid = MAKE_GUID(et_social_list, 0);
	Request *req = new Request();
	req->add(opc_query, social_list_guid, new protocol::game::social_list_t);
	game::pool()->upcall(req, boost::bind(&PvpPool::social_list_load_callback, this, _1));
}

void PvpPool::social_list_load_callback(Request *req)
{
	if (req->success())
	{
		protocol::game::social_list_t *social_list = (protocol::game::social_list_t *)req->data();
		for (int i = 0; i < social_list->socials_size(); ++i)
		{
			dhc::social_t* tsocial = social_list->mutable_socials(i);
			if (tsocial)
			{
				dhc::social_t* social = new dhc::social_t;
				social->CopyFrom(*tsocial);
				POOL_ADD(social->guid(), social);
				player_social_code_invites_[social->player_guid()] = social->guid();
				name_social_code_invites_[social->name()] = social->guid();

				for (int i = 0; i < social->invite_players_size(); ++i)
				{
					player_social_to_invites_[social->invite_players(i)] = social->guid();
				}
			}
		}
	}
}

void PvpPool::guild_list_load()
{
	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_guild_pvp_list, 0), new protocol::game::tmsg_guild_pvp_load);
	game::pool()->upcall(req, boost::bind(&PvpPool::guild_list_load_callback, this, _1, 0));

	Request *req1 = new Request();
	req1->add(opc_query, MAKE_GUID(et_guild_fight_list, 0), new protocol::game::tmsg_guild_pvp_load_fight);
	game::pool()->upcall(req1, boost::bind(&PvpPool::guild_list_load_callback, this, _1, 1));
}

void PvpPool::guild_list_load_callback(Request *req, int type)
{
	if (req->success())
	{
		if (type == 0)
		{
			protocol::game::tmsg_guild_pvp_load *msg = (protocol::game::tmsg_guild_pvp_load*)req->data();
			for (int i = 0; i < msg->ars_size(); ++i)
			{
				dhc::guild_arrange_t *gpvp = new dhc::guild_arrange_t;
				gpvp->CopyFrom(msg->ars(i));
				POOL_ADD(gpvp->guid(), gpvp);
			}
		}
		else
		{
			protocol::game::tmsg_guild_pvp_load_fight *msg = (protocol::game::tmsg_guild_pvp_load_fight*)req->data();
			for (int i = 0; i < msg->ars_size(); ++i)
			{
				dhc::guild_fight_t *gpvp = new dhc::guild_fight_t;
				gpvp->CopyFrom(msg->ars(i));
				POOL_ADD(gpvp->guid(), gpvp);
			}
		}
	}
}

void PvpPool::load_player_check_end(dhc::player_t *player)
{
	QueryMap &qm = query_map_[player->guid()];
	qm.query_num--;
	if (qm.query_num <= 0)
	{
		POOL_ADD(player->guid(), player);
		add_pvp_match(player->guid(), player->bf());
		check_rank(player, erank_point, player->pvp_total());

		std::set<uint64_t>& has_match_guids = player_choose_[player->guid()];
		for (int gl = 0; gl < player->guild_applys_size(); ++gl)
		{
			has_match_guids.insert(player->guild_applys(gl));
		}

		if (qm.params.size() > 0)
		{
			for (int i = 0; i < qm.params.size(); ++i)
			{
				Packet *pck = Packet::New(qm.params[i], 0, player->guid(), qm.msgs[i]);
				if (pck)
				{
					game::game_service()->add_msg(pck);
				}
			}
		}
		query_map_.erase(player->guid());
	}
}

void PvpPool::load_msg_callback(Request *req, dhc::player_t *player)
{
	if (req->success())
	{
		POOL_ADD(req->guid(), req->release_data());
	}
	else
	{

	}
	load_player_check_end(player);
}

void PvpPool::save_all()
{
	std::vector<uint64_t> guids;
	game::pool()->get_entitys(et_player, guids);
	for (int i = 0; i < guids.size(); ++i)
	{
		dhc::player_t *player = POOL_GET_PLAYER(guids[i]);
		if (player)
		{
			save_player(player->guid(), true);
		}
	}
}

void PvpPool::save_player(uint64_t guid, bool release)
{
	dhc::player_t *player = POOL_GET_PLAYER(guid);
	if (player)
	{
		for (int i = 0; i < player->roles_size(); ++i)
		{
			uint64_t roles_guid = player->roles(i);
			dhc::role_t *role = POOL_GET_ROLE(roles_guid);
			if (role)
			{
				POOL_SAVE(dhc::role_t, role, release);
			}
		}

		for (int i = 0; i < player->equips_size(); ++i)
		{
			uint64_t equip_guid = player->equips(i);
			dhc::equip_t *equip = POOL_GET_EQUIP(equip_guid);
			if (equip)
			{
				POOL_SAVE(dhc::equip_t, equip, release);
			}
		}


		for (int i = 0; i < player->treasures_size(); ++i)
		{
			uint64_t treasure_guid = player->treasures(i);
			dhc::treasure_t *treasure = POOL_GET_TREASURE(treasure_guid);
			if (treasure)
			{
				POOL_SAVE(dhc::treasure_t, treasure, release);
			}
		}

		for (int i = 0; i < player->pets_size(); ++i)
		{
			uint64_t pet_guid = player->pets(i);
			dhc::pet_t *pet = POOL_GET_PET(pet_guid);
			if (pet)
			{
				POOL_SAVE(dhc::pet_t, pet, release);
			}
		}

		game::pool()->remove_ref(player->guid());
		POOL_SAVE(dhc::player_t, player, release);
	}
}

void PvpPool::check_rank(dhc::player_t* player, rank_enum rank_type, int value)
{
	if (value <= 0)
	{
		return;
	}

	if (game::timer()->weekday() >= 5)
	{
		return;
	}

	int serverid = 0;
	try
	{
		serverid = boost::lexical_cast<int>(player->serverid());
	}
	catch (...)
	{
		return;
	}

	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, rank_type));
	if (!rank)
	{
		return;
	}
	int index = -1;
	for (int i = 0; i < rank->player_guid_size(); ++i)
	{
		if (rank->player_guid(i) == player->guid())
		{
			if (rank->value(i) == value)
			{
				rank->set_player_name(i, player->name());
				rank->set_player_level(i, serverid);
				rank->set_player_bf(i, player->bf());
				rank->set_player_template(i, player->template_id());
				rank->set_player_vip(i, player->vip());
				rank->set_player_achieve(i, player->dress_achieves_size());
				rank->set_player_huiyi(i, player->huiyi_jihuos_size());
				rank->set_player_chenghao(i, player->chenghao_on());
				rank->set_player_nalflag(i, player->nalflag());
				return;
			}
			index = i;
			break;
		}
	}
	if (index != -1)
	{
		for (int i = index; i < rank->player_guid_size() - 1; ++i)
		{
			rank->set_player_guid(i, rank->player_guid(i + 1));
			rank->set_player_name(i, rank->player_name(i + 1));
			rank->set_player_template(i, rank->player_template(i + 1));
			rank->set_player_level(i, rank->player_level(i + 1));
			rank->set_player_bf(i, rank->player_bf(i + 1));
			rank->set_value(i, rank->value(i + 1));
			rank->set_player_vip(i, rank->player_vip(i + 1));
			rank->set_player_achieve(i, rank->player_achieve(i + 1));
			rank->set_player_huiyi(i, rank->player_huiyi(i + 1));
			rank->set_player_chenghao(i, rank->player_chenghao(i + 1));
			rank->set_player_nalflag(i, rank->player_nalflag(i + 1));
		}
	}
	else
	{
		rank->add_player_guid(player->guid());
		rank->add_player_name(player->name());
		rank->add_player_template(player->template_id());
		rank->add_player_level(serverid);
		rank->add_player_bf(player->bf());
		rank->add_value(value);
		rank->add_player_vip(player->vip());
		rank->add_player_achieve(player->dress_achieves_size());
		rank->add_player_huiyi(player->huiyi_jihuos_size());
		rank->add_player_chenghao(player->chenghao_on());
		rank->add_player_nalflag(player->nalflag());
	}

	index = -1;
	for (int i = rank->player_guid_size() - 2; i >= 0; --i)
	{
		if (rank->value(i) < value)
		{
			rank->set_player_guid(i + 1, rank->player_guid(i));
			rank->set_player_name(i + 1, rank->player_name(i));
			rank->set_player_template(i + 1, rank->player_template(i));
			rank->set_player_level(i + 1, rank->player_level(i));
			rank->set_player_bf(i + 1, rank->player_bf(i));
			rank->set_value(i + 1, rank->value(i));
			rank->set_player_vip(i + 1, rank->player_vip(i));
			rank->set_player_achieve(i + 1, rank->player_achieve(i));
			rank->set_player_huiyi(i + 1, rank->player_huiyi(i));
			rank->set_player_chenghao(i + 1, rank->player_chenghao(i));
			rank->set_player_nalflag(i + 1, rank->player_nalflag(i));
		}
		else
		{
			index = i;
			break;
		}
	}
	rank->set_player_guid(index + 1, player->guid());
	rank->set_player_name(index + 1, player->name());
	rank->set_player_template(index + 1, player->template_id());
	rank->set_player_level(index + 1, serverid);
	rank->set_player_bf(index + 1, player->bf());
	rank->set_value(index + 1, value);
	rank->set_player_vip(index + 1, player->vip());
	rank->set_player_achieve(index + 1, player->dress_achieves_size());
	rank->set_player_huiyi(index + 1, player->huiyi_jihuos_size());
	rank->set_player_chenghao(index + 1, player->chenghao_on());
	rank->set_player_nalflag(index + 1, player->nalflag());

	if (rank->player_guid_size() > rank_range[rank_type])
	{
		rank->mutable_player_guid()->RemoveLast();
		rank->mutable_player_name()->RemoveLast();
		rank->mutable_player_template()->RemoveLast();
		rank->mutable_player_level()->RemoveLast();
		rank->mutable_player_bf()->RemoveLast();
		rank->mutable_value()->RemoveLast();
		rank->mutable_player_vip()->RemoveLast();
		rank->mutable_player_achieve()->RemoveLast();
		rank->mutable_player_huiyi()->RemoveLast();
		rank->mutable_player_chenghao()->RemoveLast();
		rank->mutable_player_nalflag()->RemoveLast();
	}
}

void PvpPool::clear_rank(dhc::rank_t *rank)
{
	if (rank)
	{
		rank->clear_player_guid();
		rank->clear_player_name();
		rank->clear_player_template();
		rank->clear_player_level();
		rank->clear_player_bf();
		rank->clear_value();
		rank->clear_player_vip();
		rank->clear_player_achieve();
		rank->clear_player_huiyi();
		rank->clear_player_chenghao();
		rank->clear_player_nalflag();

		rank->set_reward_flag(0);
	}
}

void PvpPool::add_player(const rpcproto::tmsg_player_fight_player& fplayer)
{
	if (load_complete_ == false)
	{
		return;
	}
	dhc::player_t* player = POOL_GET_PLAYER(fplayer.guid());
	if (player)
	{
		_check_player(player, &fplayer);
	}
	else
	{
		player = new dhc::player_t();
		POOL_ADD_NEW(fplayer.guid(), player);
	}
	_copy_player(&fplayer, player);
	copy_player_choose(player);
	check_rank(player, erank_point, fplayer.pvp_total());
	add_pvp_match(player->guid(), fplayer.bf());
	player->set_last_login_time(game::timer()->now());
	save_player(player->guid(), false);
}

void PvpPool::get_player(rpcproto::tmsg_rep_pvp_match* match, uint64_t self_guid, int64_t bf)
{
	std::set<uint64_t>& has_choose = player_choose_[self_guid];
	std::set<uint64_t>  this_choose;
	uint64_t choose_guid;

	for (int i = 0; i < 3; ++i)
	{
		choose_guid = 0;

		int rate = Utils::get_int32(1, 100);
		if (rate <= 80)
		{
			choose_guid = get_player(match, self_guid, has_choose,
				this_choose, bf, PvpMatch::TYPE_80);
		}
		if (choose_guid == 0 || rate > 80)
		{
			choose_guid = get_player(match, self_guid, has_choose,
				this_choose, bf, PvpMatch::TYPE_80_TO_100);
		}
			

		if (choose_guid == 0)
		{
			choose_guid = get_player(match, self_guid, has_choose,
				this_choose, bf, PvpMatch::TYPE_100);

			if (choose_guid == 0)
			{
				choose_guid = get_player(match, self_guid, has_choose,
					this_choose, bf, PvpMatch::TYPE_LESS80);
			}
		}
		
		if (choose_guid != 0)
		{
			has_choose.insert(choose_guid);
			this_choose.insert(choose_guid);
		}
	}
}

uint64_t PvpPool::get_player(rpcproto::tmsg_rep_pvp_match* match,
	uint64_t self_guid,
	const std::set<uint64_t>& has_choose_guid,
	const std::set<uint64_t>& this_choose_guid,
	int64_t bf,
	PvpMatch::Type type)
{
	std::set<uint64_t> guids = has_choose_guid;
	std::set<uint64_t> choose = this_choose_guid;
	int server_id = 0;
	int64_t bf_min = bf / 2;
	int64_t bf_max = bf * 120 / 100;
	std::vector<uint64_t> you_are_choosed;
	std::set<uint64_t>::const_iterator guid_iter;
	std::set<uint64_t>::const_iterator choose_iter;

	for (std::vector<PvpMatch>::size_type i = 0;
		i < player_matches_.size();
		++i)
	{
		const PvpMatch& pvp_match = player_matches_[i];
		if (pvp_match.player_guid == self_guid)
		{
			continue;
		}

		if (type == PvpMatch::TYPE_80)
		{
			if (pvp_match.match80(bf, bf_min) == false)
			{
				continue;
			}
		}
		else if (type == PvpMatch::TYPE_80_TO_100)
		{
			if (pvp_match.match80to100(bf, bf_max) == false)
			{
				continue;
			}
		}
		else if (type == PvpMatch::TYPE_100)
		{
			if (pvp_match.match100(bf) == false)
			{
				continue;
			}
		}
		else
		{
			if (pvp_match.matchless80(bf) == false)
			{
				continue;
			}
		}

		guid_iter = guids.find(pvp_match.player_guid);
		choose_iter = choose.find(pvp_match.player_guid);
		if (guid_iter != guids.end() && choose_iter != choose.end())
		{

		}
		else if (guid_iter == guids.end() && choose_iter == choose.end())
		{
			guids.insert(pvp_match.player_guid);
			choose.insert(pvp_match.player_guid);
			dhc::player_t *player = POOL_GET_PLAYER(pvp_match.player_guid);
			if (player)
			{
				match->add_player_guids(player->guid());
				match->add_player_names(player->name());
				match->add_player_templates(player->template_id());
				match->add_player_bfs(get_force(player));
				match->add_player_points(player->pvp_total());
				match->add_player_vips(player->vip());
				match->add_player_achieves(player->dress_achieves_size());
				match->add_player_nalflag(player->nalflag());
				try
				{
					server_id = boost::lexical_cast<int>(player->serverid());
				}
				catch (...)
				{
					server_id = 0;
				}
				match->add_player_servers(server_id);
				match->add_player_guanghuans(player->guanghuan_id());
				match->add_player_dress(player->mission());
				return player->guid();
			}
		}
		else if (guid_iter != guids.end() && choose_iter == choose.end())
		{
			choose.insert(pvp_match.player_guid);
			dhc::player_t *player = POOL_GET_PLAYER(pvp_match.player_guid);
			if (player)
			{
				you_are_choosed.push_back(pvp_match.player_guid);
			}
		}
		else
		{
			guids.insert(pvp_match.player_guid);
		}
	}

	if (you_are_choosed.empty())
	{
		return 0;
	}

	int rate = Utils::get_int32(0, you_are_choosed.size() - 1);
	dhc::player_t *player = POOL_GET_PLAYER(you_are_choosed[rate]);
	if (player)
	{
		match->add_player_guids(player->guid());
		match->add_player_names(player->name());
		match->add_player_templates(player->template_id());
		match->add_player_bfs(get_force(player));
		match->add_player_points(player->pvp_total());
		match->add_player_vips(player->vip());
		match->add_player_achieves(player->dress_achieves_size());
		match->add_player_nalflag(player->nalflag());
		try
		{
			server_id = boost::lexical_cast<int>(player->serverid());
		}
		catch (...)
		{
			server_id = 0;
		}
		match->add_player_servers(server_id);
		match->add_player_guanghuans(player->guanghuan_id());
		match->add_player_dress(player->mission());
		return player->guid();
	}
	return 0;
}

void PvpPool::add_pvp_match(uint64_t player_guid, int bf)
{
	PvpMatch match;
	match.player_guid = player_guid;
	match.bf = bf;

	std::vector<PvpMatch>::iterator it = std::find(player_matches_.begin(),
		player_matches_.end(), match);
	if (it == player_matches_.end())
	{
		player_matches_.push_back(match);
	}
	else
	{
		it->bf = bf;
	}
}

void PvpPool::copy_player_choose(dhc::player_t *player)
{
	std::map<uint64_t, std::set<uint64_t> >::const_iterator it = player_choose_.find(player->guid());
	if (it == player_choose_.end())
	{
		return;
	}

	const std::set<uint64_t>& has_match_guids = it->second;
	for (std::set<uint64_t>::const_iterator it = has_match_guids.begin();
		it != has_match_guids.end();
		++it)
	{
		player->add_guild_applys(*it);
	}
}

int PvpPool::get_force(dhc::player_t *player) const
{
	double force = 0;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (!role)
		{
			continue;
		}
		std::map<int, double> role_attrs;
		RoleOperation::get_role_attr(player, role, role_attrs);
		/// 百分比计算
		for (int j = 1; j <= 5; ++j)
		{
			role_attrs[j] = role_attrs[j] * (1 + role_attrs[j + 5] * 0.01f);
		}

		/// 总计
		int _skill_level = 0;

		for (int i = 0; i < role->jskill_level_size(); i++)
		{
			_skill_level += role->jskill_level(i);
		}

		force += (role_attrs[1] * 0.25 + role_attrs[2] * 2 + role_attrs[3] * 5 + role_attrs[4] * 5 + role_attrs[5] * 5)
			* (1 + role_attrs[11] * 0.003f + role_attrs[12] * 0.003f + role_attrs[14] * 0.003f + role_attrs[15] * 0.003f
			+ role_attrs[16] * 0.003f + role_attrs[17] * 0.003f + role_attrs[18] * 0.003f + role_attrs[19] * 0.003f
			+ role_attrs[20] * 0.003f + role_attrs[21] * 0.003f + role_attrs[22] * 0.05f + role_attrs[23] * 0.0003f
			+ role_attrs[24] * 0.0003f + role_attrs[25] * 0.0003f + role_attrs[26] * 0.0003f + role_attrs[27] * 0.0003f
			+ role_attrs[28] * 0.0003f) * (1 + _skill_level * 0.002f);
	}
	return static_cast<int>(force);
}

bool PvpPool::create_social_code(const rpcproto::tmsg_req_invite_code& msg)
{
	if (player_social_code_invites_.find(msg.player_guid()) != player_social_code_invites_.end())
	{
		return true;
	}

	dhc::social_t *social = new dhc::social_t();
	social->set_guid(msg.social_guid());
	social->set_player_guid(msg.player_guid());
	social->set_name(msg.code());
	for (int i = 0; i < msg.player_guids_size(); i++)
	{
		social->add_invite_players(msg.player_guids(i));
		social->add_invite_levels(msg.levels(i));
	}
	for (int i = 0; i < msg.ids_size(); ++i)
	{
		social->add_invite_ids(msg.ids(i));
	}
	POOL_ADD_NEW(social->guid(), social);
	save_social_code(social->guid());

	player_social_code_invites_[msg.player_guid()] = msg.social_guid();
	name_social_code_invites_[msg.code()] = msg.social_guid();

	return true;
}

int PvpPool::input_social_code(uint64_t player_guid, int level, const std::string &code)
{
	std::map<std::string, uint64_t>::const_iterator it = name_social_code_invites_.find(code);
	if (it == name_social_code_invites_.end())
	{
		return -1;
	}

	dhc::social_t *social = POOL_GET_SOCIAL(it->second);
	if (!social)
	{
		return -1;
	}

	// 已经被邀请
	for (int i = 0; i < social->invite_players_size(); ++i)
	{
		if (social->invite_players(i) == player_guid)
		{
			return 2;
		}
	}

	// 已经关联
	if (player_social_to_invites_.find(player_guid) != player_social_to_invites_.end())
	{
		return -1;
	}

	social->add_invite_players(player_guid);
	social->add_invite_levels(level);
	player_social_to_invites_[player_guid] = social->guid();
	save_social_code(social->guid());
	return 1;
}

int PvpPool::update_social_code(uint64_t player_guid, int level)
{
	std::map<uint64_t, uint64_t>::const_iterator it = player_social_to_invites_.find(player_guid);
	if (it != player_social_to_invites_.end())
	{
		dhc::social_t *social = POOL_GET_SOCIAL(it->second);
		if (social)
		{
			for (int i = 0; i < social->invite_players_size(); ++i)
			{
				if (social->invite_players(i) == player_guid)
				{
					social->set_invite_levels(i, level);
					save_social_code(social->guid());
					break;
				}
			}
		}
	}

	return 0;
}

bool PvpPool::get_social_code(uint64_t player_guid, rpcproto::tmsg_rep_invite_level* msg)
{
	std::map<uint64_t, uint64_t>::const_iterator it = player_social_code_invites_.find(player_guid);
	if (it == player_social_code_invites_.end())
	{
		return false;
	}

	dhc::social_t *social = POOL_GET_SOCIAL(it->second);
	if (!social)
	{
		return false;
	}

	for (int i = 0; i < social->invite_players_size(); ++i)
	{
		msg->add_player_guids(social->invite_players(i));
		msg->add_levels(social->invite_levels(i));
	}

	return true;
}

void PvpPool::save_social_code(uint64_t social_code)
{
	dhc::social_t *social = POOL_GET_SOCIAL(social_code);
	if (social)
	{
		POOL_SAVE(dhc::social_t, social, false);
	}
}

//////////////////////////////////////////////////////////////////////////

void PvpPool::guild_fight_gongpo(dhc::guild_arrange_t *guild_arrange, int &jundian, int &jidian, int &perfect)
{
	for (int i = 0; i < guild_arrange->guild_fights_size(); ++i)
	{
		dhc::guild_fight_t* guild_fight = POOL_GET_GUILD_FIGHT(guild_arrange->guild_fights(i));
		if (guild_fight)
		{
			int index = 0;
			for (int j = 0; j < guild_fight->guard_gongpuo_size(); ++j)
			{
				if (guild_fight->guard_gongpuo(j) != 1)
				{
					index = 1;
				}
				if (guild_fight->guard_gongpuo(j) == 1)
				{
					jundian += 1;
				}
			}
			if (guild_fight->guard_gongpuo(3) == 1)
			{
				jidian += 1;
			}
			if (index == 0)
			{
				perfect += 1;
			}
		}
	}
}

void PvpPool::check_guild_zhanji(dhc::guild_arrange_t *guild, bool add /* = false */)
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_guild_zhanji));
	if (!rank)
	{
		return;
	}

	int value = get_guild_zhanji(guild);
	guild->set_guild_zhanji(value);
	value += guild->guild_total_zhanji();
	if (add)
	{
		guild->set_guild_total_zhanji(value);
	}
	
	if (value <= 0)
	{
		return;
	}

	int index = -1;
	for (int i = 0; i < rank->player_guid_size(); ++i)
	{
		if (rank->player_guid(i) == guild->guid())
		{
			if (rank->value(i) == value)
			{
				rank->set_player_name(i, guild->guild_name());
				rank->set_player_level(i, guild->guild_server());
				rank->set_player_bf(i, 0);
				rank->set_player_template(i, guild->guild_icon());
				rank->set_player_vip(i,guild->guild_level());
				rank->set_player_achieve(i, 0);
				rank->set_player_huiyi(i, 0);
				rank->set_player_chenghao(i, 0);	
				rank->set_player_nalflag(i, 0);
				return;
			}
			index = i;
			break;
		}
	}
	if (index != -1)
	{
		for (int i = index; i < rank->player_guid_size() - 1; ++i)
		{
			rank->set_player_guid(i, rank->player_guid(i + 1));
			rank->set_player_name(i, rank->player_name(i + 1));
			rank->set_player_template(i, rank->player_template(i + 1));
			rank->set_player_level(i, rank->player_level(i + 1));
			rank->set_player_bf(i, rank->player_bf(i + 1));
			rank->set_value(i, rank->value(i + 1));
			rank->set_player_vip(i, rank->player_vip(i + 1));
			rank->set_player_achieve(i, rank->player_achieve(i + 1));
			rank->set_player_huiyi(i, rank->player_huiyi(i + 1));
			rank->set_player_chenghao(i, rank->player_chenghao(i + 1));
			rank->set_player_nalflag(i, rank->player_nalflag(i + 1));
		}
	}
	else
	{
		rank->add_player_guid(guild->guid());
		rank->add_player_name(guild->guild_name());
		rank->add_player_template(guild->guild_icon());
		rank->add_player_level(guild->guild_server());
		rank->add_player_bf(0);
		rank->add_value(value);
		rank->add_player_vip(0);
		rank->add_player_achieve(0);
		rank->add_player_huiyi(0);
		rank->add_player_chenghao(0);
		rank->add_player_nalflag(0);
	}

	index = -1;
	for (int i = rank->player_guid_size() - 2; i >= 0; --i)
	{
		if (rank->value(i) < value)
		{
			rank->set_player_guid(i + 1, rank->player_guid(i));
			rank->set_player_name(i + 1, rank->player_name(i));
			rank->set_player_template(i + 1, rank->player_template(i));
			rank->set_player_level(i + 1, rank->player_level(i));
			rank->set_player_bf(i + 1, rank->player_bf(i));
			rank->set_value(i + 1, rank->value(i));
			rank->set_player_vip(i + 1, rank->player_vip(i));
			rank->set_player_achieve(i + 1, rank->player_achieve(i));
			rank->set_player_huiyi(i + 1, rank->player_huiyi(i));
			rank->set_player_chenghao(i + 1, rank->player_chenghao(i));
			rank->set_player_nalflag(i + 1, rank->player_nalflag(i));
		}
		else
		{
			index = i;
			break;
		}
	}
	rank->set_player_guid(index + 1, guild->guid());
	rank->set_player_name(index + 1, guild->guild_name());
	rank->set_player_template(index + 1, guild->guild_icon());
	rank->set_player_level(index + 1, guild->guild_server());
	rank->set_player_bf(index + 1, 0);
	rank->set_value(index + 1, value);
	rank->set_player_vip(index + 1, 0);
	rank->set_player_achieve(index + 1, 0);
	rank->set_player_huiyi(index + 1, 0);
	rank->set_player_chenghao(index + 1, 0);
	rank->set_player_nalflag(index + 1, 0);

	if (rank->player_guid_size() > 20)
	{
		rank->mutable_player_guid()->RemoveLast();
		rank->mutable_player_name()->RemoveLast();
		rank->mutable_player_template()->RemoveLast();
		rank->mutable_player_level()->RemoveLast();
		rank->mutable_player_bf()->RemoveLast();
		rank->mutable_value()->RemoveLast();
		rank->mutable_player_vip()->RemoveLast();
		rank->mutable_player_achieve()->RemoveLast();
		rank->mutable_player_huiyi()->RemoveLast();
		rank->mutable_player_chenghao()->RemoveLast();
		rank->mutable_player_nalflag()->RemoveLast();
	}
}

void PvpPool::check_guild_player_zhanji(dhc::player_t *player, int value)
{
	if (value <= 0)
	{
		return;
	}

	int serverid = 0;
	try
	{
		serverid = boost::lexical_cast<int>(player->serverid());
	}
	catch (...)
	{
		return;
	}

	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_guild_player_zhanji));
	if (!rank)
	{
		return;
	}
	int index = -1;
	for (int i = 0; i < rank->player_guid_size(); ++i)
	{
		if (rank->player_guid(i) == player->guid())
		{
			if (rank->value(i) == value)
			{
				rank->set_player_name(i, player->name());
				rank->set_player_level(i, serverid);
				rank->set_player_bf(i, player->bf());
				rank->set_player_template(i, player->template_id());
				rank->set_player_vip(i, player->vip());
				rank->set_player_achieve(i, player->dress_achieves_size());
				rank->set_player_huiyi(i, player->huiyi_jihuos_size());
				rank->set_player_chenghao(i, player->chenghao_on());
				rank->set_player_nalflag(i, player->nalflag());
				return;
			}
			index = i;
			break;
		}
	}
	if (index != -1)
	{
		for (int i = index; i < rank->player_guid_size() - 1; ++i)
		{
			rank->set_player_guid(i, rank->player_guid(i + 1));
			rank->set_player_name(i, rank->player_name(i + 1));
			rank->set_player_template(i, rank->player_template(i + 1));
			rank->set_player_level(i, rank->player_level(i + 1));
			rank->set_player_bf(i, rank->player_bf(i + 1));
			rank->set_value(i, rank->value(i + 1));
			rank->set_player_vip(i, rank->player_vip(i + 1));
			rank->set_player_achieve(i, rank->player_achieve(i + 1));
			rank->set_player_huiyi(i, rank->player_huiyi(i + 1));
			rank->set_player_chenghao(i, rank->player_chenghao(i + 1));
			rank->set_player_nalflag(i, rank->player_nalflag(i + 1));
		}
	}
	else
	{
		rank->add_player_guid(player->guid());
		rank->add_player_name(player->name());
		rank->add_player_template(player->template_id());
		rank->add_player_level(serverid);
		rank->add_player_bf(player->bf());
		rank->add_value(value);
		rank->add_player_vip(player->vip());
		rank->add_player_achieve(player->dress_achieves_size());
		rank->add_player_huiyi(player->huiyi_jihuos_size());
		rank->add_player_chenghao(player->chenghao_on());
		rank->add_player_nalflag(player->nalflag());
	}

	index = -1;
	for (int i = rank->player_guid_size() - 2; i >= 0; --i)
	{
		if (rank->value(i) < value)
		{
			rank->set_player_guid(i + 1, rank->player_guid(i));
			rank->set_player_name(i + 1, rank->player_name(i));
			rank->set_player_template(i + 1, rank->player_template(i));
			rank->set_player_level(i + 1, rank->player_level(i));
			rank->set_player_bf(i + 1, rank->player_bf(i));
			rank->set_value(i + 1, rank->value(i));
			rank->set_player_vip(i + 1, rank->player_vip(i));
			rank->set_player_achieve(i + 1, rank->player_achieve(i));
			rank->set_player_huiyi(i + 1, rank->player_huiyi(i));
			rank->set_player_chenghao(i + 1, rank->player_chenghao(i));
			rank->set_player_nalflag( i + 1, rank->player_nalflag(i));
		}
		else
		{
			index = i;
			break;
		}
	}
	rank->set_player_guid(index + 1, player->guid());
	rank->set_player_name(index + 1, player->name());
	rank->set_player_template(index + 1, player->template_id());
	rank->set_player_level(index + 1, serverid);
	rank->set_player_bf(index + 1, player->bf());
	rank->set_value(index + 1, value);
	rank->set_player_vip(index + 1, player->vip());
	rank->set_player_achieve(index + 1, player->dress_achieves_size());
	rank->set_player_huiyi(index + 1, player->huiyi_jihuos_size());
	rank->set_player_chenghao(index + 1, player->chenghao_on());
	rank->set_player_nalflag(index + 1, player->nalflag());

	if (rank->player_guid_size() > 50)
	{
		rank->mutable_player_guid()->RemoveLast();
		rank->mutable_player_name()->RemoveLast();
		rank->mutable_player_template()->RemoveLast();
		rank->mutable_player_level()->RemoveLast();
		rank->mutable_player_bf()->RemoveLast();
		rank->mutable_value()->RemoveLast();
		rank->mutable_player_vip()->RemoveLast();
		rank->mutable_player_achieve()->RemoveLast();
		rank->mutable_player_huiyi()->RemoveLast();
		rank->mutable_player_chenghao()->RemoveLast();
		rank->mutable_player_nalflag()->RemoveLast();
	}
}

int PvpPool::get_guild_zhanji(dhc::guild_arrange_t *guild)
{
	int val = 0;
	dhc::guild_fight_t *guild_fight = 0;
	const s_t_guildfight *t_guild_fight = 0;
	dhc::guild_arrange_t *target_guild = 0;
	dhc::guild_fight_t *target_guild_fight = 0;
	std::vector<const s_t_guildfight *> t_guildfights;

	for (int i = 0; i < guild->guild_fights_size(); ++i)
	{
		guild_fight = POOL_GET_GUILD_FIGHT(guild->guild_fights(i));
		if (guild_fight)
		{
			for (int j = 0; j < guild_fight->win_nums_size(); ++j)
			{
				t_guild_fight = sGuildConfig->get_guildfight(j);
				if (t_guild_fight)
				{
					val += t_guild_fight->win_point * guild_fight->win_nums(j);
					val += t_guild_fight->lose_point * guild_fight->lose_nums(j);
				}
				else
				{
					return 0;
				}
				t_guildfights.push_back(t_guild_fight);
			}
		}

		target_guild = POOL_GET_GUILD_ARRANGE(guild_fight->target_guild_guid());
		if (target_guild)
		{
			for (int j = 0; j < target_guild->guild_fights_size(); ++j)
			{
				target_guild_fight = POOL_GET_GUILD_FIGHT(target_guild->guild_fights(j));
				if (target_guild_fight && target_guild_fight->target_guild_guid() == guild->guid())
				{
					int defense_num[4] = { 0, 0, 0, 0 };
					for (int k = 0; k < target_guild_fight->target_defense_nums_size(); ++k)
					{
						t_guild_fight = t_guildfights[k / 7];
						defense_num[k / 7] += t_guild_fight->defense_num - target_guild_fight->target_defense_nums(k);
					}
					for (int k = 0; k < 4; ++k)
					{
						t_guild_fight = t_guildfights[k];
						val += defense_num[k] * t_guild_fight->win_point;
					}
					break;
				}
			}
		}
	}

	return val;
}

dhc::player_t *PvpPool::get_guild_player(uint64_t self_guid, int64_t bf)
{
	std::vector<uint64_t> match_guids;
	for (int i = 0; i < player_matches_.size(); ++i)
	{
		if (match_guids.size() >= 30)
		{
			break;
		}
		if (player_matches_[i].matchless80(bf))
		{
			match_guids.push_back(player_matches_[i].player_guid);
		}
	}

	dhc::player_t *player = 0;
	int count = 0;
	do 
	{
		count++;
		if (match_guids.empty())
		{
			break;
		}
		player = POOL_GET_PLAYER(match_guids[Utils::get_int32(0, match_guids.size() - 1)]);
		if (player)
		{
			return player;
		}

	} while (count < 3);

	player = POOL_GET_PLAYER(self_guid);
	if (!player)
	{
		player = guild_npc_;
	}
	return player;
}

void PvpPool::guild_fight(uint64_t player_guild, 
	uint64_t player_guid, 
	uint64_t target_guild, 
	int fight_index, 
	rpcproto::tmsg_rep_guild_fight &fight)
{
	dhc::guild_arrange_t *guild_arragne = POOL_GET_GUILD_ARRANGE(player_guild);
	if (!guild_arragne)
	{
		fight.set_res(-1);
		return;
	}

	dhc::guild_fight_t *guild_fight = get_guild_fight(guild_arragne, target_guild);
	if (!guild_fight)
	{
		fight.set_res(-1);
		return;
	}

	if (fight_index < 0 || fight_index >= guild_fight->target_guids_size())
	{
		fight.set_res(-1);
		return;
	}

	int guard_index = fight_index / 7;
	if (guard_index < 0 || guard_index >= guild_fight->guard_points_size())
	{
		fight.set_res(-1);
		return;
	}

	const s_t_guildfight* t_guild_fight = sGuildConfig->get_guildfight(guard_index);
	if (!t_guild_fight)
	{
		fight.set_res(-1);
		return;
	}

	int fight_code = can_fight(guild_fight, fight_index, guard_index, t_guild_fight);
	if (fight_code != 0)
	{
		fight.set_res(fight_code);
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	dhc::player_t *target = POOL_GET_PLAYER(guild_fight->target_guids(fight_index));
	if (player && target)
	{
		int zhanji = t_guild_fight->win_point + (target->bf() / t_guild_fight->bf_rate);
		int guard_point = t_guild_fight->win_point;
		int gongxian = t_guild_fight->win_gongxian;
		int exp = 0;

		std::string text;
		int result = MissionFight::mission_sport(player, target, text);
		if (result == 1)
		{
			guild_fight->set_target_defense_nums(fight_index, guild_fight->target_defense_nums(fight_index) + 1);
			guild_fight->set_win_nums(guard_index, guild_fight->win_nums(guard_index) + 1);
		}
		else
		{
			gongxian = t_guild_fight->lose_gongxian;
			zhanji = zhanji * 0.7;
			guard_point = t_guild_fight->lose_point;

			guild_fight->set_lose_nums(guard_index, guild_fight->lose_nums(guard_index) + 1);
		}

		int zhanji_index = get_zhanji_index(guild_arragne, player_guid);
		if (zhanji_index == -1)
		{
			guild_arragne->add_player_zguids(player_guid);
			guild_arragne->add_player_zhanjis(zhanji);
			guild_arragne->add_player_total_zhanjis(zhanji);
			guild_arragne->add_player_znames(player->name());
			guild_arragne->add_player_ztemplate(player->template_id());
			guild_arragne->add_player_zlevel(player->level());
			guild_arragne->add_player_zbat_eff(player->bf());
			guild_arragne->add_player_zvip(player->vip());
			guild_arragne->add_player_zachieve(player->dress_achieves_size());
			guild_arragne->add_player_znalflags(player->nalflag());
			zhanji_index = guild_arragne->player_zguids_size() - 1;
		}
		else
		{
			guild_arragne->set_player_zhanjis(zhanji_index, guild_arragne->player_zhanjis(zhanji_index) + zhanji);
			guild_arragne->set_player_total_zhanjis(zhanji_index, guild_arragne->player_total_zhanjis(zhanji_index) + zhanji);
		}

		guild_fight->set_guard_points(guard_index, guild_fight->guard_points(guard_index) + guard_point);
		if (guild_fight->guard_points(guard_index) >= t_guild_fight->chengfang_point)
		{
			guild_fight->set_guard_points(guard_index, t_guild_fight->chengfang_point);
		}

		check_guild_zhanji(guild_arragne);
		check_guild_player_zhanji(player, guild_arragne->player_total_zhanjis(zhanji_index));

		if (has_gongpo(guild_fight, guard_index, t_guild_fight))
		{
			guild_fight->set_guard_gongpuo(guard_index, 1);
			result = 2;
			exp = t_guild_fight->gongpuo_exp;
			guild_arragne->set_guild_exp(guild_arragne->guild_exp() + exp);
		}
		if (all_gongpo(guild_fight))
		{
			result = 3;
		}
		
		int jundian = 0;
		int jidian = 0;
		int perfect = 0;
		guild_fight_gongpo(guild_arragne, jundian, jidian, perfect);


		fight.set_res(0);
		fight.set_result(result);
		fight.set_text(text);
		fight.set_guard_point(guard_point);
		fight.set_zhanji(zhanji);
		fight.set_gongxian(gongxian);
		fight.set_exp(exp);
		fight.set_judian(jundian);
		fight.set_jidi(jidian);
		fight.set_perfect(perfect);

		save_guild(guild_arragne);
	}
}

void PvpPool::guild_match(dhc::global_t *global, int cmd /* = 0 */)
{
	// 周二选取报名家族
	if (game::timer()->weekday() == 2 || cmd == 1)
	{
		std::vector<uint64_t> guids;
		game::pool()->get_entitys(et_guild_pvp, guids);
		dhc::guild_arrange_t *temp_arrange = 0;

		std::vector<GuildZhanjiRank> zhanji_rank;
		for (int i = 0; i < guids.size(); ++i)
		{
			temp_arrange = POOL_GET_GUILD_ARRANGE(guids[i]);
			if (check_bushu(temp_arrange))
			{
				zhanji_rank.push_back(GuildZhanjiRank(temp_arrange, get_weight_point(temp_arrange)));
			}
		}
		std::sort(zhanji_rank.begin(), zhanji_rank.end(), std::greater<GuildZhanjiRank>());
		if (zhanji_rank.empty())
		{
			global->set_guild_pvp_suc(1);
			return;
		}

		for (int i = 0; i < zhanji_rank.size(); ++i)
		{
			global->add_guild_pvp_ranks(zhanji_rank[i].ar->guid());
		}
	}

	if (global->guild_pvp_ranks_size() <= 0)
	{
		return;
	}

	// clear AI
	std::vector<uint64_t> guids;
	game::pool()->get_entitys(et_guild_pvp, guids);
	dhc::guild_arrange_t *temp_arrange = 0;
	
	for (int i = 0; i < guids.size(); ++i)
	{
		temp_arrange = POOL_GET_GUILD_ARRANGE(guids[i]);
		if (temp_arrange && temp_arrange->guild_ai())
		{
			for (int i = 0; i < temp_arrange->guild_fights_size(); ++i)
			{
				POOL_REMOVE(temp_arrange->guild_fights(i), 0);
			}
			temp_arrange->clear_guild_fights();

			POOL_REMOVE(temp_arrange->guid(), 0);
		}
	}

	std::vector<uint64_t> choose_arrange;
	std::vector<uint64_t> random_arrange;
	dhc::guild_arrange_t *fight_arrange = 0;
	for (int i = 0; i < global->guild_pvp_ranks_size(); ++i)
	{
		fight_arrange = POOL_GET_GUILD_ARRANGE(global->guild_pvp_ranks(i));
		if (!fight_arrange)
		{
			global->set_guild_pvp_suc(3);
			return;
		}

		choose_arrange.clear();
		random_arrange.clear();

		for (int j = i - 1; j >= i - 2 && j >= 0; j--)
		{
			choose_arrange.push_back(global->guild_pvp_ranks(j));
		}
		for (int j = i + 1; j <= i + 5 && j < global->guild_pvp_ranks_size() - 1; ++j)
		{
			choose_arrange.push_back(global->guild_pvp_ranks(j));
		}

		while (choose_arrange.size() > 5)
		{
			choose_arrange.pop_back();
		}

		if (choose_arrange.size() <= 2)
		{
			random_arrange.insert(random_arrange.end(), choose_arrange.begin(), choose_arrange.end());
		}
		else
		{
			if (game::timer()->weekday() < 4 )
			{
				Utils::get_vector(choose_arrange, 2, random_arrange);
			}
			else
			{
				Utils::get_vector(choose_arrange, 3, random_arrange);
			}

		}

		reset_match(fight_arrange);

		for (int j = 0; j < random_arrange.size(); ++j)
		{
			temp_arrange = POOL_GET_GUILD_ARRANGE(random_arrange[j]);
			if (!temp_arrange)
			{
				global->set_guild_pvp_suc(3);
				return;
			}
			//match(fight_arrange, temp_arrange);
			create_match_new(fight_arrange, temp_arrange);
		}

			// AI
			int left_ai = 3 - random_arrange.size();
			for (int i = 0; i < left_ai; ++i)
			{
				dhc::guild_arrange_t *ai_arrange = create_ai(fight_arrange);
				if (!ai_arrange)
				{
					global->set_guild_pvp_suc(3);
					return;
				}
				//match(fight_arrange, ai_arrange);
				create_match_new(fight_arrange, ai_arrange);
			}					
	}
}

void PvpPool::guild_fight_end(dhc::global_t *glob, int cmd /* = 0 */)
{
	if (cmd != 0)
	{
		if (cmd == 1)
		{
			guild_fight_rank(glob);
		}
		else
		{
			guild_pvp_reward(glob);
		}
		return;
	}


	int week_day = game::timer()->weekday();
	switch (week_day)
	{
	case 1:
		guild_pvp_reward(glob);
		break;
	case 2:
		break;
	case 3:
	case 4:
	case 5:
	case 6:
	case 7:
		guild_fight_rank(glob);
		break;
	default:
		break;
	}
}

void PvpPool::guild_fight_rank(dhc::global_t *global)
{
	if (global->guild_pvp_zhou() % 2 != 0)
	{
		return;
	}

	/// 计算战绩
	std::vector<uint64_t> guids;
	game::pool()->get_entitys(et_guild_pvp, guids);
	dhc::guild_arrange_t *arrange = 0;
	for (int i = 0; i < guids.size(); ++i)
	{
		arrange = POOL_GET_GUILD_ARRANGE(guids[i]);
		if (arrange &&
			!arrange->guild_ai())
		{

			check_guild_zhanji(arrange, true);
		}
	}

	/// 重新排位
	/*std::vector<GuildZhanjiRank> zhanjis;
	int num = global->guild_pvp_ranks_size() / 4;
	for (int i = 0; i < num; ++i)
	{
		std::vector<GuildZhanjiRank> inter_zhanjis;
		dhc::guild_arrange_t *arrange = 0;
		for (int j = i * 4; j < i * 4 + 4; j++)
		{
			if (j < global->guild_pvp_ranks_size())
			{
				arrange = POOL_GET_GUILD_ARRANGE(global->guild_pvp_ranks(j));
				if (arrange)
				{
					inter_zhanjis.push_back(GuildZhanjiRank(arrange, arrange->guild_zhanji()));
				}
			}
		}
		std::sort(inter_zhanjis.begin(), inter_zhanjis.end(), std::greater<GuildZhanjiRank>());
		zhanjis.insert(zhanjis.end(), inter_zhanjis.begin(), inter_zhanjis.end());
	}*/

	/*global->clear_guild_pvp_ranks();
	for (int i = 0; i < zhanjis.size(); ++i) 
	{
		global->add_guild_pvp_ranks(zhanjis[i].ar->guid());
	}*/
}

void PvpPool::guild_pvp_clear(dhc::global_t *global)
{
	std::vector<uint64_t> guids;
	game::pool()->get_entitys(et_guild_pvp, guids);
	dhc::guild_arrange_t *arrange = 0;
	for (int i = 0; i < guids.size(); ++i)
	{
		arrange = POOL_GET_GUILD_ARRANGE(guids[i]);
		if (arrange)
		{
			for (int i = 0; i < arrange->guild_fights_size(); ++i)
			{
				POOL_REMOVE(arrange->guild_fights(i), 0);
			}
			arrange->clear_guild_fights();

			POOL_REMOVE(arrange->guid(), 0);
		}
	}
	global->clear_guild_pvp_ranks();
}

void PvpPool::guild_pvp_reward(dhc::global_t *global)
{
	// 作战周
	if (global->guild_pvp_zhou() % 2 == 0)
	{
		for (int kmg = erank_guild_zhanji; kmg <= erank_guild_player_zhanji; ++kmg)
		{
			dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, kmg));
			if (rank)
			{
				dhc::rank_t* rank1 = POOL_GET_RANK(MAKE_GUID(et_rank, kmg + 2));
				if (!rank1)
				{
					rank1 = new dhc::rank_t();
					rank1->set_guid(MAKE_GUID(et_rank, kmg + 2));
					POOL_ADD_NEW(rank1->guid(), rank1);
				}
				rank1->CopyFrom(*rank);
				rank1->set_guid(MAKE_GUID(et_rank, kmg + 2));
				rank1->set_reward_flag(2);
				clear_rank(rank);
			}
		}
	}
	// 休战周
	else
	{
		//清空所有战斗数据
		guild_pvp_clear(global);
	}
	global->set_guild_pvp_zhou(global->guild_pvp_zhou() + 1);
}

void PvpPool::guild_pvp_gm(int arg1, int arg2)
{
	dhc::global_t* glob = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!glob)
	{
		return;
	}

	if (arg1 == -1)
	{
		guild_match(glob, arg2);
	}
	else if (arg1 == -2)
	{
		glob->set_guild_pvp_zhou(arg2);
	}
	else if (arg1 == -3)
	{
		guild_fight_end(glob, arg2);
	}
	else if (arg1 == -4)
	{
		guild_pvp_clear(glob);
	}
	else if (arg1 == -5)
	{
		guild_pvp_reward(glob);
	}
}


void PvpPool::save_guild(dhc::guild_arrange_t *arrange, bool release)
{
	dhc::guild_fight_t *guild_fight = 0;
	for (int i = 0; i < arrange->guild_fights_size(); ++i)
	{
		guild_fight = POOL_GET_GUILD_FIGHT(arrange->guild_fights(i));
		if (guild_fight)
		{
			POOL_SAVE(dhc::guild_fight_t, guild_fight, release);
		}
	}
	POOL_SAVE(dhc::guild_arrange_t, arrange, release);
}

template<typename T1, typename T2>
void match(T1 *t1, T2 *t2)
{
	dhc::guild_fight_t *one_fight = new dhc::guild_fight_t();
	one_fight->set_guid(game::gtool()->assign(et_guild_fight));
	one_fight->set_guild_server(t2->guild_server());
	one_fight->set_guild_name(t2->guild_name());
	one_fight->set_guild_guid(t2->guid());
	one_fight->set_guild_icon(t2->guild_icon());
	one_fight->set_target_guild_guid(t2->guid());
	one_fight->set_guild_level(t2->guild_level());

	for (int i = 0; i < t2->player_guids_size(); ++i)
	{
		one_fight->add_target_guids(t2->player_guids(i));
		one_fight->add_target_names(t2->player_names(i));
		one_fight->add_target_templates(t2->player_template(i));
		one_fight->add_target_levels(t2->player_level(i));
		one_fight->add_target_bat_effs(t2->player_bat_eff(i));
		one_fight->add_target_vips(t2->player_vip(i));
		one_fight->add_target_achieves(t2->player_achieve(i));
		one_fight->add_target_defense_nums(0);
	}

	for (int i = 0; i < 4; ++i)
	{
		one_fight->add_guard_points(0);
		one_fight->add_guard_gongpuo(0);
		one_fight->add_win_nums(0);
		one_fight->add_lose_nums(0);
	}
	t1->add_guild_fights(one_fight->guid());
	POOL_ADD_NEW(one_fight->guid(), one_fight);
	POOL_SAVE(dhc::guild_fight_t, one_fight, false);
	POOL_SAVE(dhc::guild_arrange_t, t1, false);
}


void PvpPool::create_match(dhc::guild_arrange_t *arrange)
{
	for (int i = 0; i < 3; ++i)
	{
		dhc::guild_arrange_t *ai = create_ai(arrange);
		match(arrange, ai);
		match(ai, arrange);
	}
}
void PvpPool::create_match_new(dhc::guild_arrange_t *arrange, dhc::guild_arrange_t *arrange_match)
{
	match(arrange, arrange_match);
}

void PvpPool::create_match(std::vector<dhc::guild_arrange_t*> &arranges)
{
	match(arranges[0], arranges[1]);
	match(arranges[1], arranges[0]);
	match(arranges[0], arranges[2]);
	match(arranges[2], arranges[0]);
	match(arranges[0], arranges[3]);
	match(arranges[3], arranges[0]);

	match(arranges[1], arranges[2]);
	match(arranges[2], arranges[1]);
	match(arranges[1], arranges[3]);
	match(arranges[3], arranges[1]);

	match(arranges[2], arranges[3]);
	match(arranges[3], arranges[2]);
}

dhc::guild_arrange_t *PvpPool::create_ai(dhc::guild_arrange_t *arrange)
{
	dhc::guild_arrange_t *ai = new dhc::guild_arrange_t();
	ai->set_guid(game::gtool()->assign(et_guild_pvp));
	ai->set_guild_server(arrange->guild_server());
	ai->set_guild_name(sSportConfig->get_random_name());
	ai->set_guild_icon(1001);
	ai->set_guild_ai(1);
	ai->set_guild_level(5);

	dhc::player_t *player = 0;
	for (int i = 0; i < arrange->player_guids_size(); ++i)
	{
		std::vector<uint32_t> roles;
		player = get_guild_player(arrange->player_guids(i), arrange->player_bat_eff(i));
		ai->add_player_guids(player->guid());
		ai->add_player_names(sSportConfig->get_random_name());
		ai->add_player_template(sRoleConfig->get_random_role_id(Utils::get_int32(3, 4), roles));
		ai->add_player_level(player->level());
		ai->add_player_bat_eff(player->bf());
		ai->add_player_vip(player->vip());
		ai->add_player_achieve(player->dress_achieves_size());
		ai->add_mplayer_nalflags(player->nalflag());
		ai->add_player_map_star(0);
		
	}

	POOL_ADD_NEW(ai->guid(), ai);
	POOL_SAVE(dhc::guild_arrange_t, ai, false);
	return ai;
}

dhc::guild_arrange_t* PvpPool::create_random_ai()
{
	dhc::guild_arrange_t *ai = new dhc::guild_arrange_t();
	ai->set_guid(game::gtool()->assign(et_guild_pvp));
	ai->set_guild_server(0);
	ai->set_guild_name(sSportConfig->get_random_name());
	ai->set_guild_icon(1001);
	ai->set_guild_ai(1);
	ai->set_guild_level(5);

	dhc::player_t *player = 0;
	for (int i = 0; i < 22; ++i)
	{
		std::vector<uint32_t> roles;
		player = get_guild_player(0, Utils::get_int32(500000, 1000000));
		ai->add_player_guids(player->guid());
		ai->add_player_names(sSportConfig->get_random_name());
		ai->add_player_template(sRoleConfig->get_random_role_id(Utils::get_int32(3, 4), roles));
		ai->add_player_level(player->level());
		ai->add_player_bat_eff(player->bf());
		ai->add_player_vip(player->vip());
		ai->add_player_achieve(player->dress_achieves_size());
		ai->add_player_map_star(0);
	}

	POOL_ADD_NEW(ai->guid(), ai);
	POOL_SAVE(dhc::guild_arrange_t, ai, false);
	return ai;
}

void PvpPool::reset_match(dhc::guild_arrange_t *arrange)
{
	for (int i = 0; i < arrange->guild_fights_size(); ++i)
	{
		POOL_REMOVE(arrange->guild_fights(i), 0);
	}
	arrange->clear_guild_fights();

	arrange->set_guild_exp(0);
	arrange->set_guild_zhanji(0);
	for (int i = 0; i < arrange->player_zhanjis_size(); ++i)
	{
		arrange->set_player_zhanjis(i, 0);
	}

	
}

int PvpPool::get_arrange_index(dhc::guild_arrange_t *arrange, uint64_t player_guid) const
{
	for (int i = 0; i < arrange->player_guids_size(); ++i)
	{
		if (arrange->player_guids(i) == player_guid)
		{
			return i;
		}
	}

	return -1;
}

dhc::guild_fight_t* PvpPool::get_guild_fight(dhc::guild_arrange_t *arrange, uint64_t target_guild)
{
	dhc::guild_fight_t *guild_fight = 0;
	for (int i = 0; i < arrange->guild_fights_size(); ++i)
	{
		guild_fight = POOL_GET_GUILD_FIGHT(arrange->guild_fights(i));
		if (guild_fight && guild_fight->guild_guid() == target_guild)
		{
			return guild_fight;
		}
	}
	return 0;
}

int PvpPool::get_fight_index(dhc::guild_fight_t *fight, uint64_t target_guid) const
{
	for (int i = 0; i < fight->target_guids_size(); ++i)
	{
		if (fight->target_guids(i) == target_guid)
		{
			return i;
		}
	}
	return -1;
}

int PvpPool::get_zhanji_index(dhc::guild_arrange_t *arrange, uint64_t player_guid) const
{
	for (int i = 0; i < arrange->player_zguids_size(); ++i)
	{
		if (arrange->player_zguids(i) == player_guid)
		{
			return i;
		}
	}
	return -1;
}

int PvpPool::can_fight(dhc::guild_fight_t *fight, int fight_index, int guard_index, const s_t_guildfight *t_guild_fight) const
{
	if (fight->guard_gongpuo(guard_index))
	{
		return ERROR_GUILD_PVP_FIGHT_GONGPO;
	}

	if (fight->target_defense_nums(fight_index) >= t_guild_fight->defense_num)
	{
		return ERROR_GUILD_PVP_FIGHT_NUM;
	}

	if (fight->guard_points(guard_index) >= t_guild_fight->chengfang_point)
	{
		return ERROR_GUILD_PVP_FIGHT_GONGPO;
	}

	if (guard_index == 3)
	{
		bool has_gongpo = false;
		for (int i = 0; i < 3; ++i)
		{
			if (fight->guard_gongpuo(i))
			{
				has_gongpo = true;
				break;
			}
		}
		if (!has_gongpo)
		{
			return -1;
		}
	}
	return 0;
}

bool PvpPool::has_gongpo(dhc::guild_fight_t *fight, int guard_index, const s_t_guildfight *t_guild_fight) const
{
	if (fight->guard_points(guard_index) >= t_guild_fight->chengfang_point)
	{
		return true;
	}

	int defense_num = 0;
	for (int i = 7 * guard_index; i < 7 * guard_index + 7; ++i)
	{
		if (i < fight->target_defense_nums_size())
		{
			defense_num += fight->target_defense_nums(i);
		}
	}

	return defense_num >= t_guild_fight->defense_num * t_guild_fight->def_num;
}

bool PvpPool::all_gongpo(dhc::guild_fight_t *fight) const
{
	for (int i = 0; i < fight->guard_gongpuo_size(); ++i)
	{
		if (fight->guard_gongpuo(i) == 0)
		{
			return false;
		}
	}
	return true;
}

double PvpPool::get_weight_point(dhc::guild_arrange_t *arrange)
{
	std::vector<int> bfs;
	std::vector<int> lvs;
	std::vector<int> svs;

	for (int i = 0; i < arrange->player_bat_eff_size(); ++i)
	{
		bfs.push_back(arrange->player_bat_eff(i));
		lvs.push_back(arrange->player_level(i));
		svs.push_back(arrange->player_map_star(i));
	}

	std::sort(bfs.begin(), bfs.end(), std::greater<int>());
	std::sort(lvs.begin(), lvs.end(), std::greater<int>());
	std::sort(svs.begin(), svs.end(), std::greater<int>());

	double bf = 0;
	double level = 0;
	double star = 0;
	for (int i = 0; i < 4; ++i)
	{
		bf += bfs[i];
		level += lvs[i];
		star += svs[i];
	}

	bf /= 4.0;
	level /= 4.0;
	star /= 4.0;
	if (star >= 4000.0)
	{
		star = 25.0;
	}
	else
	{
		star *= 0.00625;
	}

	return level * 0.5 + std::log(bf / 15091) / 0.0809 * 0.3 + star * 0.2;
}

bool PvpPool::check_bushu(dhc::guild_arrange_t *arrange)
{
	if (!arrange)
	{
		return false;
	}

	if (arrange->player_guids_size() != 22)
	{
		return false;
	}

	for (int i = 0; i < arrange->player_guids_size(); ++i)
	{
		if (arrange->player_guids(i) <= 0)
		{
			return false;
		}
	}

	return true;
}