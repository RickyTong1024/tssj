#include "team_service.h"
#include "rpc.pb.h"
#include "client.h"
#include "team.h"
#include "pool.h"
#include "single.h"
#include "equip_config.h"
#include "guild_config.h"
#include "item_config.h"
#include "mission_config.h"
#include "player_config.h"
#include "role_config.h"
#include "sport_config.h"
#include "treasure_config.h"
#include "pvp_config.h"
#include "team_config.h"
#include "mission_fight.h"

enum ServiceType
{
	ST_BINGYUAN,
	ST_DS,
};

TeamService::TeamService()
{
	
}

TeamService::~TeamService()
{
	
}

int TeamService::init()
{
	if (sTeamConfig->parse() == -1)
	{
		return -1;
	}

	if (sEquipConfig->parse() == -1)
	{
		return -1;
	}
	if (sGuildConfig->parse() == -1)
	{
		return -1;
	}
	if (sItemConfig->parse() == -1)
	{
		return -1;
	}
	if (sMissionConfig->parse() == -1)
	{
		return -1;
	}
	if (sPlayerConfig->parse() == -1)
	{
		return -1;
	}
	if (sRoleConfig->parse() == -1)
	{
		return -1;
	}
	if (sSportConfig->parse() == -1)
	{
		return -1;
	}
	if (sTreasureConfig->parse() == -1)
	{
		return -1;
	}

	if (sPvpConfig->parse() == -1)
	{
		return -1;
	}

	if (sPoolManager->init() == -1)
	{
		return -1;
	}

	if (sTeamManager->init() == -1)
	{
		return -1;
	}

	if (sClientManager->init() == -1)
	{
		return -1;
	}

	team_timer_ = game::timer()->schedule(boost::bind(&TeamManager::update, sTeamManager, _1), 1000, "team");

	pool_timer_ = game::timer()->schedule(boost::bind(&PoolManager::update, sPoolManager, _1), 5000, "team_pool");

	single_timer_ = game::timer()->schedule(boost::bind(&SingleManager::update, sSingleManager, _1), 1000, "single");

	return 0;
}

int TeamService::fini()
{
	sClientManager->fini();
	sTeamManager->fini();
	sPoolManager->fini();

	if (team_timer_)
	{
		game::timer()->cancel(team_timer_);
	}

	if (pool_timer_)
	{
		game::timer()->cancel(pool_timer_);
	}

	return 0;
}

int TeamService::terminal_enter_world(Packet *pck)
{
	int hid = pck->hid();
	hids_.insert(hid);
	hid_guids_[hid] = 0;
	return 0;
}

int TeamService::terminal_login_world(Packet *pck)
{
	if (hids_.find(pck->hid()) == hids_.end())
	{
		return -1;
	}

	if (hid_guids_.find(pck->hid()) == hid_guids_.end())
	{
		return -1;
	}

	protocol::team::cmsg_enter_world msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	Client* client = sClientManager->get_client(msg.guid());
	if (!client)
	{
		dhc::player_t *old_player = POOL_GET_PLAYER(msg.guid());
		if (old_player)
		{
			sClientManager->add_client(msg.guid(), msg.sig());
			client = sClientManager->get_client(msg.guid());
			if (!client)
			{
				return -1;
			}
		}
		else
		{
			return -1;
		}
	}
	else
	{
		/// »¹Ã»ÍË³ö
		if (client->get_hid() != -1)
		{
			if (hids_.find(client->get_hid()) != hids_.end() &&
				hid_guids_.find(client->get_hid()) != hid_guids_.end())
			{
				hid_guids_[client->get_hid()] = 0;
			}
			client->set_hid(-1);
		}
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}
	sTeamManager->leave_team(player);
	sPoolManager->remove_dead_player(player->guid());
	sSingleManager->stop_match(player);

	if (!client->check(pck->hid(), msg.guid(), msg.sig()))
	{
		return -1;
	}
	hid_guids_[pck->hid()] = msg.guid();

	protocol::team::smsg_enter_world smsg;
	int chenghao = 7;
	int point = 0;
	int rank = -1;
	sPoolManager->get_rank_info(msg.guid(), chenghao, point, rank);
	smsg.set_chenghao(chenghao);
	smsg.set_point(point);
	smsg.set_rank(rank);
	smsg.set_next_invite_time(player->last_tili_time());
	sTeamManager->get_invite_list(msg.guid(), smsg);
	int sgrank = -1;
	int sduanwei = 0;
	int spoint = 0;
	sPoolManager->get_duanwei_info(msg.guid(), sgrank, sduanwei, spoint);
	smsg.set_sgrank(sgrank);
	smsg.set_spoint(spoint);
	smsg.set_sduanwei(sduanwei);
	smsg.set_srank(player->yuanli());
	smsg.set_sreset(player->bwjl_task_num());
	smsg.set_soduanwei(player->ds_hit());
	smsg.set_ds_cdtime(player->huodong_yxhg_time());
	client->send_msg(SMSG_ENTER_TEAM_SERVER, &smsg);

	if (player->bwjl_task_num() > 0 && msg.type() == ST_DS)
	{
		player->set_bwjl_task_num(0);
	}

	return 0;
}

int TeamService::terminal_leave_world(Packet *pck)
{
	int hid = pck->hid();
	if (hids_.find(hid) == hids_.end())
	{
		return -1;
	}
	if (hid_guids_.find(hid) == hid_guids_.end())
	{
		return -1;
	}

	uint64_t guid = hid_guids_[hid];
	hids_.erase(hid);
	hid_guids_.erase(hid);
	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->leave_team(player);
	sClientManager->remove_client(guid);
	sPoolManager->add_dead_player(player->guid(), player->level(), player->ds_duanwei(), player->bf());
	sSingleManager->stop_match(player);

	return 0;
}

int TeamService::terminal_create_team(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->create_team(player);

	return 0;
}

int TeamService::terminal_enter_team(Packet *pck)
{
	protocol::team::cmsg_team_enter msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->enter_team(msg.team_id(), player, false, false);
	return 0;
}

int TeamService::terminal_match_team(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->match_team(player);

	return 0;
}

int TeamService::terminal_leave_team(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->leave_team(player);
	
	return 0;
}

int TeamService::terminal_kick_team(Packet *pck)
{
	protocol::team::cmsg_team_kick msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	dhc::player_t *target = POOL_GET_PLAYER(msg.guid());
	if (!target)
	{
		return -1;
	}

	sTeamManager->kick_team(player, target);

	return 0;
}

int TeamService::terminal_prepare_team(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->prepare_team(player);

	return 0;
}

int TeamService::terminal_fight_team(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->fight_team(player);

	return 0;
}

int TeamService::terminal_fight_end_team(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->fight_end_team(player);

	return 0;
}

int TeamService::terminal_view_rank(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	protocol::game::smsg_rank_view smsg;
	sPoolManager->view_rank(player, erank_chenghao, smsg);
	sClientManager->send_msg(player->guid(), SMSG_VIEW_CHENGHAO_RANK, &smsg);

	return 0;
}

int TeamService::terminal_urge_team(Packet *pck)
{
	protocol::team::cmsg_hanhua msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->press_team(player, msg.index());
	return 0;
}

int TeamService::terminal_leader_prepare(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->leader_prepare(player);
	return 0;
}

int TeamService::terminal_end_match_team(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->end_match_team(player);

	return 0;
}

int TeamService::terminal_open_team(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->open_team(player);

	return 0;
}

int TeamService::terminal_chat_team(Packet *pck)
{
	protocol::team::cmsg_chat_team msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->chat_team(player, msg.color(), msg.text());

	return 0;
}

int TeamService::terminal_move_team(Packet *pck)
{
	protocol::team::cmsg_team_move msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	dhc::player_t *target = POOL_GET_PLAYER(msg.guid());
	if (!target)
	{
		return -1;
	}

	sTeamManager->move_team(player, target, msg.index());

	return 0;
}

int TeamService::terminal_view_team(Packet *pck)
{
	protocol::team::cmsg_player_look msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	dhc::player_t *target = POOL_GET_PLAYER(msg.guid());
	if (!target)
	{
		return -1;
	}

	sTeamManager->view_member(player, target);

	return 0;
}

int TeamService::terminal_invite_agree(Packet *pck)
{
	protocol::team::cmsg_invite_agree msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	if (msg.agree())
	{
		sTeamManager->social_invite(player, msg.invite_id());
	}
	else
	{
		sTeamManager->remove_invite(player, msg.invite_id());
	}
	return 0;
}

int TeamService::terminal_invite_all(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sTeamManager->invite_dead(player);

	return 0;
}

int TeamService::terminal_ds_match(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	sSingleManager->start_match(player);

	return 0;
}

int TeamService::terminal_ds_rank(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	protocol::game::smsg_rank_view smsg;
	sPoolManager->view_rank(player, erank_single_duanwei, smsg);
	sClientManager->send_msg(player->guid(), SMSG_VIEW_DS_RANK, &smsg);

	return 0;
}

int TeamService::terminal_ds_stop(Packet *pck)
{
	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	if (sSingleManager->stop_match(player))
	{
		sClientManager->send_msg(player->guid(), SMSG_DS_MATCH_STOP);
	}
	
	return 0;
}

int TeamService::terminal_qiecuo(Packet *pck)
{
	protocol::game::cmsg_qiecuo msg;
	if (!pck->parse_protocol(msg))
	{
		return -1;
	}

	uint64_t guid = get_guid(pck->hid());
	if (guid != pck->guid())
	{
		return -1;
	}

	Client* client = sClientManager->get_client(guid);
	if (!client)
	{
		return -1;
	}

	dhc::player_t *player = client->get_player();
	if (!player)
	{
		return -1;
	}

	uint64_t player_team = sTeamManager->get_player_team(player);
	if (player_team != 0)
	{
		TeamManager::Team* team = sTeamManager->get_team(player_team);
		if (team)
		{
			if (team->stat() != TS_CREATE)
			{
				return -1;
			}
		}
		else
		{
			return -1;
		}
	}

	dhc::player_t *target = POOL_GET_PLAYER(msg.target());
	if (!target)
	{
		return -1;
	}

	std::string text;
	int result = MissionFight::mission_sport(player, target, text);

	protocol::game::smsg_qiecuo smsg;
	smsg.set_result(result);
	smsg.set_text(text);
	sClientManager->send_msg(player->guid(), SMSG_QIECUO, &smsg);

	return 0;
}

void TeamService::rpc_enter_world(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_team_login msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}
	sClientManager->add_client(msg.players().guid(), msg);
	sPoolManager->remove_dead_player(msg.players().guid());
	player_enter(msg.players().guid());
	sPoolManager->save_player(msg.players().guid(), false);

	int chenghao, point, rank;
	sPoolManager->get_rank_info(msg.players().guid(), chenghao, point, rank);
	rpcproto::tmsg_rep_team_login smsg;
	smsg.set_res(0);
	smsg.set_chenghao(chenghao);
	smsg.set_point(point);

	int dspoint, dsduanwei, dsrank;
	sPoolManager->get_duanwei_info(msg.players().guid(), dsrank, dsduanwei, dspoint);
	if (dsduanwei >= 25)
	{
		dhc::player_t *player = POOL_GET_PLAYER(msg.players().guid());
	    if (player)
		{
			smsg.set_ds_cd_time(player->huodong_yxhg_time());
		}
	}
	smsg.set_duanwei(dsduanwei);
	smsg.set_dspoint(dspoint);

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void TeamService::rpc_bingyuan(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_bingyuan_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(msg.guid());
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_bingyuan_fight smsg;
	int point = 0;
	int bingbing = 0;
	bool res = false;

	res = sTeamManager->get_bingyuan_result(player, msg.id(), msg.num(), point, bingbing);
	if (res)
	{
		player->set_by_reward_num(msg.num() - 1);
		sPoolManager->check_rank(player, erank_chenghao, point);
	}
	smsg.set_bingjing(bingbing);
	smsg.set_point(point);
	smsg.set_chenghao(sPoolManager->get_chenghao(player->guid()));
	smsg.set_res(res);
	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);

	int new_point, new_chenghao, new_rank;
	sPoolManager->get_rank_info(player->guid(), new_chenghao, new_point, new_rank);
	protocol::team::smsg_team_player_reward_change rsmsg;
	rsmsg.set_guid(player->guid());
	rsmsg.set_num(player->by_reward_num());
	rsmsg.set_point(new_point);
	rsmsg.set_chenghao(new_chenghao);
	rsmsg.set_rank(new_rank);
	sClientManager->send_msg(player->guid(), SMSG_CHANGE_REWARD_NUM, &rsmsg);
}

void TeamService::rpc_bingyuan_buy(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_bingyuan_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(msg.guid());
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_by_reward_num(msg.num());
	sTeamManager->change_reward_num(player);

	game::rpc_service()->response(name, id, "");
}

void TeamService::rpc_bingyuan_reward(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_pvp_reward msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_chenghao_last));
	if (!rank)
	{
		GLOBAL_ERROR;
		return;
	}
	if (rank->reward_flag() != 2)
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_pvp_reward smsg;
	smsg.set_res(0);
	smsg.mutable_ranks();
	for (int i = 0; i < rank->player_guid_size(); ++i)
	{
		if (i >= 300)
		{
			break;
		}
		smsg.add_guids(rank->player_guid(i));
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void TeamService::rpc_team_pull(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_team_pull msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	int point, chenghao, rank;
	sPoolManager->get_rank_info(msg.guid(), chenghao, point, rank);

	rpcproto::tmsg_rep_team_push smsg;
	smsg.set_chenghao(chenghao);

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void TeamService::rpc_invite(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_team_invite msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	sTeamManager->add_invite_list(msg.friends(), msg.whois());

	game::rpc_service()->response(name, id, "");
}

void TeamService::rpc_ds_fight(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_ds_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(msg.guid());
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_ds_fight smsg;
	int point = 0;
	int xinpian = 0;
	int ciliao = 0;
	int duanwei = player->ds_duanwei();
	int grank = -1;

	bool res = sSingleManager->get_fight_result(player, msg.id(), point, xinpian, ciliao);       
	if (res)
	{
		sPoolManager->check_rank(player, erank_single_duanwei, point);

		if (msg.reward_num() <= 0)
		{
			xinpian = 0;
			ciliao = 0;
		}
		else
		{
			player->set_ds_reward_num(msg.reward_num() - 1);
		}

		int temppoint = 0;
		sPoolManager->get_duanwei_info(player->guid(), grank, duanwei, temppoint);
		player->set_ds_duanwei(duanwei);
	}
	smsg.set_xinpian(xinpian);
	smsg.set_point(point);
	smsg.set_ciliao(ciliao);
	smsg.set_duanwei(duanwei);
	smsg.set_grank(grank);                   
	smsg.set_res(res);
	smsg.set_cd_time(player->huodong_yxhg_time());
	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void TeamService::rpc_ds_time_buy(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_team_pull msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(msg.guid());
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	
	if (player->huodong_yxhg_time() == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_huodong_yxhg_time(0);
	rpcproto::tmsg_rep_invite_code smsg;
	smsg.set_res(1);
	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void TeamService::rpc_ds_gm(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_bingyuan_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(msg.guid());
	if (!player)
	{
		GLOBAL_ERROR; 
		return;
	}
	sPoolManager->check_rank(player, erank_single_duanwei,msg.num());
	game::rpc_service()->response(name, id, "");

}

void TeamService::player_enter(uint64_t player_guid)
{
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (player)
	{
		if (game::timer()->trigger_week_time(player->last_week_time()))
		{
			if (player->last_week_time() != 0)
			{
				player->set_bwjl_task_num(1);
			}
			player->set_last_week_time(game::timer()->now());
			player->set_ds_hit(player->ds_duanwei());
			player->set_ds_duanwei(player->ds_duanwei() - 9); 
			if (player->ds_duanwei() < 1)
			{
				player->set_ds_duanwei(1);
			}
			player->set_ds_reward_buy(player->ds_duanwei());
			int point = sTeamConfig->get_duanwei_point(player->ds_duanwei());
			if (point > 0)
			{
				sPoolManager->check_rank(player, erank_single_duanwei, point);
			}
		}
		else
		{
			player->set_ds_duanwei(sPoolManager->get_duanwei(player));
		}
	}
}


uint64_t TeamService::get_guid(int hid) const
{
	std::map<int, uint64_t>::const_iterator it = hid_guids_.find(hid);
	if (it == hid_guids_.end())
	{
		return 0;
	}
	return it->second;
}