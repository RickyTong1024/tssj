#include "pool.h"
#include "utils.h"
#include "pvp_config.h"
#include "sport_config.h"
#include "role_config.h"
#include "team_config.h"
#include "client.h"


int PoolManager::init()
{
	load_global();
	load_rank();
	load_all_player();
	make_npc();
	return 0;
}

int PoolManager::fini()
{
	return 0;
}

int PoolManager::update(const ACE_Time_Value & tv)
{
	if (game::timer()->weekday() == 2)
	{
		dhc::rank_t *last_rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_chenghao_last));
		if (last_rank && last_rank->reward_flag() == 2)
		{
			last_rank->set_reward_flag(0);
		}
	}
	dhc::global_t *gl = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (gl)
	{
		if (game::timer()->trigger_week_time(gl->guild_refresh_time()))
		{
			gl->set_guild_refresh_time(game::timer()->now());

			POOL_SAVE(dhc::global_t, gl, false);

			
			dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_chenghao));
			if (rank)
			{
				dhc::rank_t *last_rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_chenghao_last));
				if (last_rank)
				{
					last_rank->CopyFrom(*rank);
					last_rank->set_guid(MAKE_GUID(et_rank, erank_chenghao_last));
					last_rank->set_reward_flag(2);
				}
				clear_rank(rank);
			}

			dhc::rank_t *rank_duanwei = POOL_GET_RANK(MAKE_GUID(et_rank, erank_single_duanwei));
			if (rank_duanwei)
			{
				clear_rank(rank_duanwei);
			}

			dhc::player_t *uplayer = 0;
			uint64_t now_time = game::timer()->now();
			protocol::team::smsg_reset_ds smsg;
			const std::map<uint64_t, Client> &all_clients = sClientManager->get_all_client();
			for (std::map<uint64_t, Client>::const_iterator ci = all_clients.begin();
				ci != all_clients.end();
				++ci)
			{
				uplayer = POOL_GET_PLAYER(ci->first);
				if (uplayer)
				{
					smsg.set_oldduanwei(uplayer->ds_duanwei());
					uplayer->set_bwjl_task_num(0);
					uplayer->set_ds_hit(uplayer->ds_duanwei());
					uplayer->set_last_week_time(now_time);
					uplayer->set_ds_duanwei(uplayer->ds_duanwei() - 9);
					if (uplayer->ds_duanwei() < 1)
					{
						uplayer->set_ds_duanwei(1);
					}
					uplayer->set_ds_reward_buy(uplayer->ds_duanwei());
					smsg.set_newduanwei(uplayer->ds_duanwei());
					int point = sTeamConfig->get_duanwei_point(uplayer->ds_duanwei());
					if (point > 0)
					{
						check_rank(uplayer, erank_single_duanwei, point);
					}
					save_player(uplayer->guid(), false);
					sClientManager->send_msg(uplayer->guid(), SMSG_DS_RESET, &smsg);
				}
			}
		}
	}

	for (int i = erank_chenghao; i < erank_end; ++i)
	{
		dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, i));
		if (rank)
		{
			POOL_SAVE(dhc::rank_t, rank, false);
		}
	}

	return 0;
}

void PoolManager::load_all_player()
{
	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_player_guids_list, 0), new protocol::game::player_guids_list_t);
	game::pool()->upcall(req, boost::bind(&PoolManager::load_all_player_callback, this, _1));
}

void PoolManager::load_all_player_callback(Request* req)
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

void PoolManager::load_rank()
{
	for (int i = erank_chenghao; i < erank_end; ++i)
	{
		Request *req = new Request();
		req->add(opc_query, MAKE_GUID(et_rank, i), new dhc::rank_t);
		game::pool()->upcall(req, boost::bind(&PoolManager::load_rank_callback, this, _1));
	}
}

void PoolManager::load_rank_callback(Request* req)
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

void PoolManager::load_player(uint64_t player_guid, const std::string& name, int id)
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
	game::pool()->upcall(req, boost::bind(&PoolManager::load_player_callback, this, _1));
}

void PoolManager::load_player_callback(Request *req)
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
				game::pool()->upcall(req, boost::bind(&PoolManager::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->equips_size(); ++i)
		{
			if (player->equips(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->equips(i), new dhc::equip_t);
				game::pool()->upcall(req, boost::bind(&PoolManager::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->treasures_size(); ++i)
		{
			if (player->treasures(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->treasures(i), new dhc::treasure_t);
				game::pool()->upcall(req, boost::bind(&PoolManager::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->pets_size(); ++i)
		{
			if (player->pets(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->pets(i), new dhc::pet_t);
				game::pool()->upcall(req, boost::bind(&PoolManager::load_msg_callback, this, _1, player));
			}
		}

		load_player_check_end(player);
	}
}

void PoolManager::load_global()
{
	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_global, 0), new dhc::global_t);
	game::pool()->upcall(req, boost::bind(&PoolManager::load_global_callback, this, _1));
}

void PoolManager::load_global_callback(Request* req)
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
		POOL_ADD_NEW(req->guid(), glob);
		POOL_SAVE(dhc::global_t, glob, false);
	}
}

void PoolManager::load_player_check_end(dhc::player_t *player)
{
	QueryMap &qm = query_map_[player->guid()];
	qm.query_num--;
	if (qm.query_num <= 0)
	{
		POOL_ADD(player->guid(), player);

		add_dead_player(player->guid(), player->level(), player->ds_duanwei(), player->bf());

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

void PoolManager::load_msg_callback(Request *req, dhc::player_t *player)
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

void PoolManager::save_player(uint64_t guid, bool release)
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

int PoolManager::get_chenghao(uint64_t player_guid) const
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_chenghao));
	if (rank)
	{
		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			if (rank->player_guid(i) == player_guid)
			{
				return sPvpConfig->get_chenghao(i + 1, rank->value(i));
			}
		}
	}
	
	return sPvpConfig->get_default_chenghao();
}

void PoolManager::get_rank_info(uint64_t player_guid, int &chenghao, int &point, int &rank) const
{
	chenghao = sPvpConfig->get_default_chenghao();
	point = 0;
	rank = -1;

	dhc::rank_t *ranks = POOL_GET_RANK(MAKE_GUID(et_rank, erank_chenghao));
	if (ranks)
	{
		for (int i = 0; i < ranks->player_guid_size(); ++i)
		{
			if (ranks->player_guid(i) == player_guid)
			{
				chenghao = sPvpConfig->get_chenghao(i + 1, ranks->value(i));
				point = ranks->value(i);
				rank = i + 1;
				if (rank > 100)
					rank = -1;
				break;
			}
		}
	}
	
}

void PoolManager::check_rank(dhc::player_t *player, rank_enum num, int value)
{
	int serverid = 0;
	try
	{
		serverid = boost::lexical_cast<int>(player->serverid());
	}
	catch (...)
	{
		return;
	}

	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, num));
	if (!rank)
	{
		return;
	}

	int index = -1;
	for (int i = 0; i < rank->player_guid_size(); ++i)
	{
		if (rank->player_guid(i) == player->guid())
		{
			rank->set_player_name(i, player->name());
			rank->set_player_level(i, serverid);
			rank->set_player_bf(i, player->bf());
			rank->set_player_template(i, player->template_id());
			rank->set_player_vip(i, player->vip());
			rank->set_player_achieve(i, player->dress_achieves_size());
			rank->set_player_huiyi(i, player->huiyi_jihuos_size());
			rank->set_value(i, rank->value(i) + value);
			if (rank->value(i) < 0)
				rank->set_value(i, 0);
			rank->set_player_chenghao(i, player->chenghao_on());
			rank->set_player_nalflag(i , player->nalflag());
			value = rank->value(i);
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
		if (value < 0)
			rank->add_value(0);
		else
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
	if (value < 0)
		rank->set_value(index + 1, 0);
	else
		rank->set_value(index + 1, value);
	rank->set_player_vip(index + 1, player->vip());
	rank->set_player_achieve(index + 1, player->dress_achieves_size());
	rank->set_player_huiyi(index + 1, player->huiyi_jihuos_size());
	rank->set_player_chenghao(index + 1, player->chenghao_on());
	rank->set_player_nalflag(index + 1, player->nalflag());
}

void PoolManager::view_rank(dhc::player_t *player, rank_enum index, protocol::game::smsg_rank_view &smsg)
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, index));
	if (!rank)
	{
		return;
	}

	dhc::rank_t *new_rank = smsg.mutable_rank_list();
	new_rank->set_guid(rank->guid());
	for (int i = 0; i < rank->player_guid_size(); ++i)
	{
		if (i >= 50)
		{
			break;
		}
		new_rank->add_player_guid(rank->player_guid(i));
		new_rank->add_player_name(rank->player_name(i));
		new_rank->add_player_template(rank->player_template(i));
		new_rank->add_player_level(rank->player_level(i));
		new_rank->add_player_bf(rank->player_bf(i));
		new_rank->add_value(rank->value(i));
		new_rank->add_player_vip(rank->player_vip(i));
		new_rank->add_player_achieve(rank->player_achieve(i));
		new_rank->add_player_huiyi(rank->player_huiyi(i));
		new_rank->add_player_chenghao(rank->player_chenghao(i));
		new_rank->add_player_nalflag(rank->player_nalflag(i));
	}
	
}

void PoolManager::clear_rank(dhc::rank_t *rank)
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

void PoolManager::get_dead_player(uint64_t guid, int level, int num, std::vector<dhc::player_t* > &deadd, const std::set<uint64_t> &has_invite)
{
	std::vector<uint64_t> dead;
	for (std::map<uint64_t, OfflinePlayer>::const_iterator it = players_.begin();
		it != players_.end();
		++it)
	{
		if (dead.size() >= num)
		{
			break;
		}

		const OfflinePlayer &pp = it->second;
		if (guid != pp.guid &&
			pp.level <= level &&
			pp.level >= level - 5 &&
			has_invite.find(pp.guid) == has_invite.end())
		{
			dhc::player_t *player = POOL_GET_PLAYER(pp.guid);
			if (player)
			{
				dead.push_back(player->guid());
			}
		}
	}
	
	int npc_size = num - dead.size();

	if (npc_size > 0)
	{
		int npc_index = 0;
		for (int i = 0; i < npcs_.size(); ++i)
		{
			if (npc_index >= npc_size)
			{
				break;
			}
			if (has_invite.find(npcs_[i]) == has_invite.end())
			{
				npc_index++;
				dead.push_back(npcs_[i]);
			}
		}
	}

	for (int i = 0; i < dead.size(); ++i)
	{
		dhc::player_t *player = POOL_GET_PLAYER(dead[i]);
		if (player)
		{
			dhc::player_t *clone_player = new dhc::player_t();
			clone_player->CopyFrom(*player);
			clone_player->set_guid(game::gtool()->assign(et_player));
			clone_player->set_name(sSportConfig->get_random_name());
			if (is_npc_guid(player->guid()))
			{
				clone_player->set_level(Utils::get_int32(level - 5, level));
			}
			POOL_ADD(clone_player->guid(), clone_player);
			clone_player->set_ore_last_time(player->guid());
			deadd.push_back(clone_player);
		}
	}
}

uint64_t PoolManager::get_dead_player(uint64_t player_guid,  int duanwei, int64_t bf_max, int64_t bf_min) const
{
	std::vector<uint64_t> dead;
	std::vector<uint64_t> hit;
	std::vector<uint64_t> maxx;
	int count = 0;

	for (std::map<uint64_t, OfflinePlayer>::const_iterator it = players_.begin();
		it != players_.end();
		++it)
	{
		/// 最适合的5玩家
		if (hit.size() >= 5)
		{
			break;
		}

		/// 50玩家
		if (count >= 50 && dead.size() >= 5)
		{
			break;
		}

		/// 1000玩家
		if (count >= 1000 && maxx.size() >= 5)
		{
			break;
		}

		count++;
		const OfflinePlayer &op = it->second;
		if (op.duanwei <= 0)
		{
			continue;
		}

		/// 同段位战力相近
		if (op.duanwei == duanwei)
		{
			if (op.bf >= bf_min && op.bf <= bf_max)
			{
				hit.push_back(op.guid);
			}
			else
			{
				maxx.push_back(op.guid);
			}
		}
		/// 段位相差一战力相近
		else if (std::abs(op.duanwei - duanwei) <= 1)
		{
			if (op.bf >= bf_min && op.bf <= bf_max)
			{
				hit.push_back(op.guid);
			}
			else
			{
				maxx.push_back(op.guid);
			}
		}
		/// 战力相近的
		else if(op.bf >= bf_min && op.bf <= bf_max)
		{
			dead.push_back(op.guid);
		}
		/// 随机
		else if (op.bf >= bf_min)
		{
			maxx.push_back(op.guid);
		}
	}

	if (hit.size() > 0)
	{
		return hit[Utils::get_int32(0, hit.size() - 1)];
	}

	if (dead.size() > 0)
	{
		return dead[Utils::get_int32(0, dead.size() - 1)];
	}

	if (maxx.size() > 0)
	{
		return maxx[Utils::get_int32(0, maxx.size() - 1)];
	}


	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (player)
	{
		dhc::player_t *clone_player = new dhc::player_t();
		clone_player->CopyFrom(*player);
		clone_player->set_guid(game::gtool()->assign(et_player));
		clone_player->set_name(sSportConfig->get_random_name());
		clone_player->set_level(Utils::get_int32(player->level() - 5, player->level()));
		POOL_ADD(clone_player->guid(), clone_player);	
		return clone_player->guid();
	}
	return 0;
}

void PoolManager::add_dead_player(uint64_t guid, int level, int duanwei, int bf)
{
	OfflinePlayer pp;
	pp.guid = guid;
	pp.level = level;
	pp.bf = bf;
	pp.duanwei = duanwei;
	players_[guid] = pp;
}

void PoolManager::remove_dead_player(uint64_t guid)
{
	players_.erase(guid);
}

void PoolManager::make_npc()
{
	for (int j = 0; j < 10; ++j)
	{
		uint64_t player_guid = game::gtool()->assign(et_player);
		dhc::player_t *npc = new dhc::player_t();
		npc->set_guid(player_guid);
		npc->set_name(sSportConfig->get_random_name());
		npc->set_bf(Utils::get_int32(5000000, 6000000));
		npc->set_level(Utils::get_int32(70, 80));
		npc->set_template_id(Utils::get_int32(201, 209));
		POOL_ADD(npc->guid(), npc);
		npcs_.push_back(player_guid);

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
				role->set_level(Utils::get_int32(70, 100));
				role->set_pinzhi(t_role->pinzhi * 100);
				while (role->jskill_level_size() < 7)
				{
					role->add_jskill_level(1);
				}
				role->add_dress_ids(0);
				role->set_dress_on_id(0);
				for (int k = 0; k < 4; ++k)
				{
					uint64_t zhuangbei_guid = game::gtool()->assign(et_equip);
					dhc::equip_t *equip = new dhc::equip_t();
					equip->set_guid(zhuangbei_guid);
					equip->set_role_guid(role_guid);
					equip->set_player_guid(player_guid);
					equip->set_template_id(10401 + 10000 * k);
					equip->set_enhance(Utils::get_int32(70, 80));
					POOL_ADD(equip->guid(), equip);
					role->add_zhuangbeis(zhuangbei_guid);
				}
				for (int k = 0; k < 2; ++k)
				{
					uint64_t treasule_guid = game::gtool()->assign(et_treasure);
					dhc::treasure_t *treasure = new dhc::treasure_t();
					treasure->set_guid(treasule_guid);
					treasure->set_role_guid(role_guid);
					treasure->set_player_guid(player_guid);
					treasure->set_template_id(4001 + k * 1);
					treasure->set_enhance(Utils::get_int32(30, 50));
					POOL_ADD(treasure->guid(), treasure);
					role->add_treasures(treasule_guid);
				}
				npc->add_roles(role->guid());
				npc->add_zhenxing(role->guid());
				npc->add_duixing(i);
				POOL_ADD(role->guid(), role);
			}
		}
	}
}

bool PoolManager::is_npc_guid(uint64_t guid) const
{
	for (int i = 0; i < npcs_.size(); ++i)
	{
		if (npcs_[i] == guid)
		{
			return true;
		}
	}
	return false;
}

void PoolManager::get_duanwei_info(uint64_t player_guid, int &ranki, int &duanwei, int &point) const
{
	ranki = -1;
	duanwei = 1;
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (player)
	{
		duanwei = player->ds_reward_buy();
	}
	point = 0;

	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_single_duanwei));
	if (rank)
	{
		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			if (rank->player_guid(i) == player_guid)
			{
				if (rank->value(i) > 0)
				{
					ranki = i + 1;
				}
				point = rank->value(i);
				duanwei = sTeamConfig->get_duanwei(rank->value(i), i + 1, duanwei);
				break;
			}
		}
	}
}

int PoolManager::get_duanwei(uint64_t player_guid) const
{
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		return 1;
	}

	return get_duanwei(player);
}

int PoolManager::get_duanwei(dhc::player_t *player) const
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_single_duanwei));
	if (rank)
	{
		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			if (rank->player_guid(i) == player->guid())
			{
				return sTeamConfig->get_duanwei(rank->value(i), i + 1, player->ds_reward_buy());
			}
		}
	}
	return player->ds_reward_buy();
}