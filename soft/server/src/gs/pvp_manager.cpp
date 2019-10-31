#include "pvp_manager.h"
#include "gs_message.h"
#include "player_operation.h"
#include "player_config.h"
#include "pvp_operation.h"
#include "pvp_config.h"
#include "rpc.pb.h"
#include "post_operation.h"
#include "social_operation.h"
#include "huodong_pool.h"
#include "rank_operation.h"
#include "item_operation.h"
#include "guild_config.h"



#define PVP_REFRESH_NUM 10

int PvpManager::init()
{
	if (sPvpList->init() == -1)
	{
		return -1;
	}

	if (sPvpConfig->parse() == -1)
	{
		return -1;
	}

	start_ = false;

	timer_ = game::timer()->schedule(boost::bind(&PvpManager::update, this, _1), 5000, "pvp");
	return 0;
}

int PvpManager::fini()
{
	sPvpList->fini();

	return 0;
}

int PvpManager::update(const ACE_Time_Value& tv)
{
	terminal_bingyuan_reward();
	terminal_pvp_reward();
	terminal_guild_reward();
	sPvpList->update();

	return 0;
}

void PvpManager::start_pvp(Packet* pck)
{
	start_ = true;
}

void PvpManager::terminal_pvp_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (!start_ && game::timer()->weekday() >= 5)
	{
		const protocol::game::smsg_pvp_view* empty_pvp_view = sPvpList->get_empty_pvp_list();
		ResMessage::res_pvp_refresh(player, *empty_pvp_view, name, id);
		return;
	}

	const protocol::game::smsg_pvp_view* pvp_view = sPvpList->get_pvp_list(player->guid());
	if (pvp_view)
	{
		bool should_refresh = true;
		for (int i = 0; i < pvp_view->player_wins_size(); ++i)
		{
			if (pvp_view->player_wins(i) == 0)
			{
				should_refresh = false;
				break;
			}
		}
		if (!should_refresh)
		{
			ResMessage::res_pvp_refresh(player, *pvp_view, name, id);
			return;
		}
	}

	rpcproto::tmsg_req_pvp_match rmsg;
	rmsg.set_guid(player->guid());
	rmsg.set_serverid(player->serverid());
	rmsg.set_bf(player->bf_max());
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_PVP_PULL, s,
		boost::bind(&PvpManager::terminal_pvp_refresh_callback, this, _1, player->guid(), name, id));
}

void PvpManager::terminal_pvp_refresh(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (!start_ && game::timer()->weekday() >= 5)
	{
		const protocol::game::smsg_pvp_view* empty_pvp_view = sPvpList->get_empty_pvp_list();
		ResMessage::res_pvp_refresh(player, *empty_pvp_view, name, id);
		return;
	}

	if (player->pvp_refresh_num() >= 10)
	{
		if (player->jewel() < 10)
		{
			PERROR(ERROR_JEWEL);
			return;
		}
		PlayerOperation::player_dec_resource(player, resource::JEWEL, 10, LOGWAY_PVP_REFRESH);
	}
	player->set_pvp_refresh_num(player->pvp_refresh_num() + 1);
	

	rpcproto::tmsg_req_pvp_match rmsg;
	rmsg.set_guid(player->guid());
	rmsg.set_serverid(player->serverid());
	rmsg.set_bf(player->bf_max());
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_PVP_PULL, s, 
		boost::bind(&PvpManager::terminal_pvp_refresh_callback, this, _1, player->guid(), name, id));
}

void PvpManager::terminal_pvp_refresh_callback(const std::string &data, uint64_t player_guid, const std::string&name, int id)
{
	rpcproto::tmsg_rep_pvp_match msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.player_guids_size() <= 0)
	{
		PERROR(ERROR_PVP_MATCH);
		return;
	}

	protocol::game::smsg_pvp_view smsg;
	for (int i = 0; i < msg.player_guids_size(); ++i)
	{
		smsg.add_player_guids(msg.player_guids(i));
		smsg.add_player_names(msg.player_names(i));
		smsg.add_player_templates(msg.player_templates(i));
		smsg.add_player_servers(msg.player_servers(i));
		smsg.add_player_bfs(msg.player_bfs(i));
		int point = std::pow((double)msg.player_bfs(i), 0.333);
		if (point < 120)
		{
			point = 120;
		}
		smsg.add_player_points(point);
		smsg.add_player_wins(0);
		smsg.add_player_vips(msg.player_vips(i));
		smsg.add_player_achieves(msg.player_achieves(i));
		smsg.add_player_guanghuans(msg.player_guanghuans(i));
		smsg.add_player_dress(msg.player_dress(i));
		smsg.add_player_chenghaos(0);
		smsg.add_player_nalflags(msg.player_nalflag(i));
		
	}
	sPvpList->refresh_pvp_list(player_guid, smsg);

	ResMessage::res_pvp_refresh(player, smsg, name, id);
}

void PvpManager::terminal_pvp_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (!start_ && game::timer()->weekday() >= 5)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.num() < 1)
	{
		GLOBAL_ERROR;
		return;
	}

	int jewel = 0;
	const s_t_price* t_price = 0;
	for (int i = 0; i < msg.num(); ++i)
	{
		t_price = sPlayerConfig->get_price(player->pvp_buy_num() + 1 + i);
		if (!t_price)
		{
			GLOBAL_ERROR;
			return;
		}
		jewel += t_price->pvp_num;
	}
	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	player->set_pvp_buy_num(player->pvp_buy_num() + msg.num());
	player->set_pvp_num(player->pvp_num() + msg.num());
	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_PVP_BUY);

	ResMessage::res_success(player, true, name, id);
}

void PvpManager::terminal_pvp_fight(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pvp_fight_end msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (!start_ && game::timer()->weekday() >= 5)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->pvp_num() < 1)
	{
		GLOBAL_ERROR;
		return;
	}

	const protocol::game::smsg_pvp_view* views = sPvpList->get_pvp_list(player_guid);
	if (!views)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.index() < 0 || msg.index() >= views->player_guids_size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (views->player_wins(msg.index()) == 1)
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_req_pvp_fight rmsg;
	rmsg.set_guid(views->player_guids(msg.index()));
	PvpOperation::copy(player, rmsg.mutable_player(), PT_LIEREN);
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_PVP_FIGHT, s,
		boost::bind(&PvpManager::terminal_pvp_fight_callback, this, _1, player->guid(), msg.index(), name, id));
}

void PvpManager::terminal_pvp_active(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_active_reward msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	const s_t_pvp_active* t_pvp_active = sPvpConfig->get_pvp_active(msg.id());
	if (!t_pvp_active)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->pvp_hit_ids_size(); ++i)
	{
		if (player->pvp_hit_ids(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->pvp_hit() < t_pvp_active->num)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_add_resource(player, resource::LIEREN_POINT, t_pvp_active->lieren_point, LOGWAY_PVP_ACTIVE);
	player->add_pvp_hit_ids(msg.id());

	ResMessage::res_success(player, true, name, id);
}

void PvpManager::terminal_team_enter(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	TermInfo* info = game::channel()->get_channel(player->guid());
	if (!info)
	{
		GLOBAL_ERROR;
		return;
	}


	rpcproto::tmsg_req_team_login rmsg;
	rmsg.set_sig(info->sig);
	PlayerOperation::player_calc_force(player);
	PvpOperation::copy(player, rmsg.mutable_players(), PT_BINGYUAN);

	std::set<uint64_t> friends;
	SocialOperation::get_friends(player_guid, friends);
	for (std::set<uint64_t>::const_iterator it = friends.begin(); it != friends.end(); ++it)
	{
		rmsg.add_friends(*it);
	}
	rmsg.set_by_reward_num(player->by_reward_num());

	rmsg.set_srank(RankOperation::get_rank(player, e_rank_ds));
	rmsg.set_ds_reward_num(player->ds_reward_num());

	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_TEAM_ENTER, s,
		boost::bind(&PvpManager::terminal_team_enter_callback, this, _1, player_guid, name, id));
}

void PvpManager::terminal_team_invite(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_team_invite_friend msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::player_t *target = POOL_GET_PLAYER(msg.friends());
	if (!target)
	{
		GLOBAL_ERROR;
		return;
	}

	sPvpList->add_social_invite(target->guid());

	rpcproto::tmsg_team_invite rmsg;
	rmsg.set_friends(player_guid);
	rmsg.set_whois(msg.friends());
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->push("remote1", PMSG_SOCIAL_TEAM_INVITE, s);

	ResMessage::res_success(player, true, name, id);
}

void PvpManager::terminal_team_enter_callback(const std::string &data, uint64_t player_guid, const std::string& name, int id)
{
	rpcproto::tmsg_rep_team_login msg;
	if (!msg.ParseFromString(data))
	{
		PERROR(ERROR_TEAM_ENTER_FAIL);
		return;
	}

	if (msg.res() != 0)
	{
		PERROR(ERROR_TEAM_ENTER_FAIL);
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_TEAM_ENTER_FAIL);
		return;
	}

	player->set_by_point(msg.point());

	const s_t_bingyuan_chenghao *t_bingyuan_chenghao = sPvpConfig->get_bingyuan_chenghao(msg.chenghao());
	if (t_bingyuan_chenghao)
	{
		PlayerOperation::player_add_chenghao(player, t_bingyuan_chenghao->cid);
	}
	
	player->set_ds_duanwei(msg.duanwei());
	player->set_ds_point(msg.dspoint());
	if (player->ds_point() < 0)
		player->set_ds_point(0);
	if (msg.ds_cd_time())
	{
		cd_time_[player->guid()] = msg.ds_cd_time();
	}
	RankOperation::check_value(player, e_rank_ds, player->ds_point());

	ResMessage::res_success(player, true, name, id);
}

void PvpManager::terminal_bingyuan_fight(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_bingyuan_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	sPvpList->update_bingyuan_list(player_guid);

	if (player->by_reward_num() <= 0)
	{
		protocol::game::smsg_bingyuan_fight_end smsg;
		smsg.set_bingjing(0);
		smsg.set_point(0);
		ResMessage::res_bingyuan_fight_end(player, smsg, name, id);
		return;
	}

	rpcproto::tmsg_req_bingyuan_fight rmsg;
	rmsg.set_id(msg.id());
	rmsg.set_guid(player->guid());
	rmsg.set_num(player->by_reward_num());
	rmsg.set_point(player->by_point());
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_BINGYUAN_FIGHT, s,
		boost::bind(&PvpManager::terminal_bingyuan_fight_callback, this, _1, player->guid(), name, id));
}

void PvpManager::terminal_bingyuan_reward_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}
	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() < 1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->by_reward_buy() + msg.num() > 9)
	{
		GLOBAL_ERROR;
		return;
	}

	int price = 0;
	const s_t_price* t_price = 0;
	for (int i = 0; i < msg.num(); ++i)
	{
		t_price = sPlayerConfig->get_price(player->by_reward_buy() + i + 1);
		if (!t_price)
		{
			GLOBAL_ERROR;
			return;
		}
		price += t_price->bingyuan;
	}
	if (player->jewel() < price)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	player->set_by_reward_num(player->by_reward_num() + msg.num());
	player->set_by_reward_buy(player->by_reward_buy() + msg.num());
	PlayerOperation::player_dec_resource(player, resource::JEWEL, price, LOGWAY_BINGYUAN_BUY);

	rpcproto::tmsg_req_bingyuan_buy rmsg;
	rmsg.set_guid(player->guid());
	rmsg.set_num(player->by_reward_num());
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->push("remote1", PMSG_BINGYUAN_BUY, s);

	ResMessage::res_success(player, true, name, id);
}

void PvpManager::terminal_ds_num_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}
	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.num() < 1)
	{
		GLOBAL_ERROR;
		return;
	}

	int price = 0;
	const s_t_price* t_price = 0;
	for (int i = 0; i < msg.num(); ++i)
	{
		t_price = sPlayerConfig->get_price(player->ds_reward_buy() + i + 1);
		if (!t_price)
		{
			GLOBAL_ERROR;
			return;
		}
		price += t_price->ds;
	}
	if (player->jewel() < price)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	player->set_ds_reward_num(player->ds_reward_num() + msg.num());
	player->set_ds_reward_buy(player->ds_reward_buy() + msg.num());
	PlayerOperation::player_dec_resource(player, resource::JEWEL, price, LOGWAY_DS_BUY);

	ResMessage::res_success(player, true, name, id);
}

void PvpManager::terminal_ds_fight(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_ds_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	rpcproto::tmsg_req_ds_fight rmsg;
	rmsg.set_id(msg.id());
	rmsg.set_guid(player->guid());
	rmsg.set_reward_num(player->ds_reward_num());
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_DS_FIGHT, s,
		boost::bind(&PvpManager::terminal_ds_fight_callback, this, _1, player->guid(), name, id));
}

void PvpManager::terminal_ds_target(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_active_reward msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	const s_t_ds_target *t_target = sPvpConfig->get_ds_target(msg.id());
	if (!t_target)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->ds_hit_ids_size(); ++i)
	{
		if (player->ds_hit_ids(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->ds_hit() < t_target->count)
	{
		GLOBAL_ERROR;
		return;
	}

	player->add_ds_hit_ids(msg.id());
	s_t_rewards rds;
	rds.add_reward(t_target->rd);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_DS_TARGET);
	ResMessage::res_success(player, true, name, id);
}

void PvpManager::terminal_ds_time_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	std::map<uint64_t, uint64_t>::const_iterator it = cd_time_.find(player->guid());
	if (it == cd_time_.end())
	{
		GLOBAL_ERROR;
		return;
	}
	uint64_t time = it->second;
	int jewel = ((time - game::timer()->now()) / 1000) / 5;
	if (((time - game::timer()->now()) / 1000) % 5 > 0)
	{
		jewel = jewel + 1;
	}
	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	rpcproto::tmsg_req_team_pull rmsg;
	rmsg.set_guid(player->guid());
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_DS_TIME_BUY, s,
		boost::bind(&PvpManager::terminal_ds_time_buy_callback, this, _1, player->guid(), name, id));
}
	
void PvpManager::terminal_pvp_fight_callback(const std::string &data, uint64_t player_guid, int index, const std::string&name, int id)
{
	rpcproto::tmsg_rep_pvp_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	const protocol::game::smsg_pvp_view* views = sPvpList->get_pvp_list(player_guid);
	if (!views)
	{
		GLOBAL_ERROR;
		return;
	}

	if (index < 0 || index >= views->player_guids_size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (views->player_wins(index) == 1)
	{
		GLOBAL_ERROR;
		return;
	}
	
	int point = views->player_points(index);
	if (msg.result() == 1)
	{
		sPvpList->update_pvp_list(player_guid, static_cast<PvpListNum>(index));
		player->set_pvp_hit(player->pvp_hit() + 1);
	}
	else
	{
		point /= 2;
	}
	s_t_rewards rds;
	rds.add_reward(1, resource::LIEREN_POINT, point);

	protocol::game::smsg_pvp_fight_end smsg;
	smsg.set_result(msg.result());
	smsg.set_text(msg.text());
	smsg.set_pvp_point(point);

	player->set_pvp_num(player->pvp_num() - 1);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_PVP_FIGHT_END);
	player->set_pvp_total(player->pvp_total() + point);
	sHuodongPool->huodong_active(player, HUODONG_COND_PVP_COUNT, 1);

	ADD_MSG_REWARD(smsg, rds);
	ResMessage::res_pvp_fight_end(player, smsg, name, id);
}

void PvpManager::terminal_pvp_reward()
{
	dhc::pvp_list_t *reward_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_LIST_REWARD));
	if (reward_list)
	{
		if (reward_list->sync_flag() == 1)
		{ 
			reward_list->set_sync_flag(2);
			reward_list->set_refresh_time(game::timer()->now() + 30000);

			rpcproto::tmsg_req_pvp_reward rmsg;
			rmsg.set_serverid("");
			std::string s;
			rmsg.SerializeToString(&s);
			game::rpc_service()->request("remote1", PMSG_PVP_REWARD, s,
				boost::bind(&PvpManager::terminal_pvp_reward_callback, this, _1));
		}
		else if (reward_list->sync_flag() == 2)
		{
			if (game::timer()->now() > reward_list->refresh_time())
			{
				reward_list->set_sync_flag(1);
			}
		}
	}
}

void PvpManager::terminal_bingyuan_reward()
{
	dhc::pvp_list_t *reward_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_BINGYUAN_REWARD));
	if (reward_list)
	{
		if (reward_list->sync_flag() == 1)
		{
			reward_list->set_sync_flag(2);
			reward_list->set_refresh_time(game::timer()->now() + 30000);

			rpcproto::tmsg_req_pvp_reward rmsg;
			rmsg.set_serverid("");
			std::string s;
			rmsg.SerializeToString(&s);
			game::rpc_service()->request("remote1", PMSG_BINGYUAN_REWARD, s,
				boost::bind(&PvpManager::terminal_bingyuan_reward_callback, this, _1));
		}
		else if (reward_list->sync_flag() == 2)
		{
			if (game::timer()->now() > reward_list->refresh_time())
			{
				reward_list->set_sync_flag(1);
			}
		}
	}
}

void PvpManager::terminal_guild_reward()
{
	dhc::pvp_list_t *reward_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_GUILD_REWARD));
	if (reward_list)
	{
		if (reward_list->sync_flag() == 1)
		{
			reward_list->set_sync_flag(2);
			reward_list->set_refresh_time(game::timer()->now() + 30000);

			rpcproto::tmsg_req_pvp_reward rmsg;
			rmsg.set_serverid("");
			std::string s;
			rmsg.SerializeToString(&s);
			game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_REWARD, s,
				boost::bind(&PvpManager::terminal_guild_reward_callback, this, _1));
		}
		else if (reward_list->sync_flag() == 2)
		{
			if (game::timer()->now() > reward_list->refresh_time())
			{
				reward_list->set_sync_flag(1);
			}
		}
		else if (reward_list->sync_flag() == 3)
		{
			reward_list->set_sync_flag(4);
			reward_list->set_refresh_time(game::timer()->now() + 60000);

			rpcproto::tmsg_req_pvp_reward rmsg;
			rmsg.set_serverid("sync");
			std::string s;
			rmsg.SerializeToString(&s);
			game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_REWARD, s,
				boost::bind(&PvpManager::terminal_guild_reward_callback, this, _1));
		}
		else if (reward_list->sync_flag() == 4)
		{
			if (game::timer()->now() > reward_list->refresh_time())
			{
				reward_list->set_sync_flag(3);
			}
		}
	}
}

void PvpManager::terminal_pvp_reward_callback(const std::string &data)
{
	rpcproto::tmsg_rep_pvp_reward msg;
	if (!msg.ParseFromString(data))
	{
		return;
	}

	if (msg.res() != 0)
	{
		return;
	}

	dhc::pvp_list_t *pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_LIST_REWARD));
	if (!pvp_list)
	{
		return;
	}
	if (pvp_list->sync_flag() != 2)
	{
		return;
	}
	pvp_list->set_sync_flag(0);
	const s_t_pvp_reward *t_rewards = 0;
	for (int i = 0; i < msg.guids_size(); ++i)
	{
		for (int j = 0; j < pvp_list->player_guids_size(); ++j)
		{
			std::string sender;
			std::string title;
			std::string text;
			if (pvp_list->player_guids(j) == msg.guids(i))
			{
				t_rewards = sPvpConfig->get_pvp_reward(i + 1);
				if (t_rewards)
				{
					int lang_ver = game::channel()->get_channel_lang(msg.guids(i));
					game::scheme()->get_server_str(lang_ver,sender, "sys_sender");
					game::scheme()->get_server_str(lang_ver, title, "pvp_rank_title");
					game::scheme()->get_server_str(lang_ver, text, "pvp_rank_text", i + 1);
					PostOperation::post_create(msg.guids(i), title, text, sender, t_rewards->rewards);
				}
				break;
			}
		}
	}
	POOL_SAVE(dhc::pvp_list_t, pvp_list, false);

	dhc::pvp_list_t *rank_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_LIST_RANK));
	if (!rank_list)
	{
		return;
	}
	
	const rpcproto::tmsg_rep_pvp_match& rank = msg.ranks();
	protocol::game::smsg_pvp_view smsg;
	for (int i = 0; i < rank.player_guids_size(); ++i)
	{
		smsg.add_player_guids(rank.player_guids(i));
		smsg.add_player_names(rank.player_names(i));
		smsg.add_player_templates(rank.player_templates(i));
		smsg.add_player_servers(rank.player_servers(i));
		smsg.add_player_bfs(rank.player_bfs(i));
		smsg.add_player_points(rank.player_points(i));
		smsg.add_player_wins(0);
		smsg.add_player_vips(rank.player_vips(i));
		smsg.add_player_achieves(rank.player_achieves(i));
		smsg.add_player_guanghuans(rank.player_guanghuans(i));
		smsg.add_player_dress(rank.player_dress(i));
		smsg.add_player_chenghaos(0);
		smsg.add_player_nalflags(rank.player_nalflag(i));
	}
	sPvpList->refresh_empty_view(smsg);
}

void PvpManager::terminal_bingyuan_reward_callback(const std::string &data)
{
	rpcproto::tmsg_rep_pvp_reward msg;
	if (!msg.ParseFromString(data))
	{
		return;
	}

	if (msg.res() != 0)
	{
		return;
	}

	dhc::pvp_list_t *pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_BINGYUAN_REWARD));
	if (!pvp_list)
	{
		return;
	}
	if (pvp_list->sync_flag() != 2)
	{
		return;
	}
	pvp_list->set_sync_flag(0);
	const s_t_bingyuan_rank_reward *t_rewards = 0;
	for (int i = 0; i < msg.guids_size(); ++i)
	{
		for (int j = 0; j < pvp_list->player_guids_size(); ++j)
		{
			if (pvp_list->player_guids(j) == msg.guids(i))
			{
				t_rewards = sPvpConfig->get_bingyuan_reward(i + 1);
				if (t_rewards)
				{
					std::string sender;
					std::string title;
					std::string text;
					int lang_ver = game::channel()->get_channel_lang(msg.guids(i));
					game::scheme()->get_server_str(lang_ver,sender, "sys_sender");
					game::scheme()->get_server_str(lang_ver, title, "bingyuan_rank_title");
					game::scheme()->get_server_str(lang_ver, text, "bingyuan_rank_text", i + 1);
					PostOperation::post_create(msg.guids(i), title, text, sender, t_rewards->rds);
				}
				break;
			}
		}
	}
	POOL_SAVE(dhc::pvp_list_t, pvp_list, false);
}

void PvpManager::terminal_guild_reward_callback(const std::string &data)
{
	rpcproto::tmsg_rep_guild_fight_reward msg;
	if (!msg.ParseFromString(data))
	{
		return;
	}

	if (msg.stype() < 0)
	{
		return;
	}

	dhc::pvp_list_t *pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_GUILD_REWARD));
	if (!pvp_list)
	{
		return;
	}
	if (pvp_list->sync_flag() != 2 && pvp_list->sync_flag() != 4)
	{
		return;
	}
	pvp_list->set_sync_flag(0);
	POOL_SAVE(dhc::pvp_list_t, pvp_list, false);

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (global)
	{
		global->set_guild_pvp_zhou(msg.zhou());
	}

	if (msg.stype() == 0)
	{
		return;
	}

	const s_t_guildfight_rankreward *t_rewards = 0;
	{		
		for (int i = 0; i < msg.player_rewards_size(); ++i)
		{
			for (int j = 0; j < pvp_list->player_guids_size(); ++j)
			{
				if (pvp_list->player_guids(j) == msg.player_rewards(i))
				{
					t_rewards = sGuildConfig->get_guildfight_rank_reward(2, i + 1);
					if (t_rewards)
					{
						std::string sender;
						std::string title;
						std::string text;
						int lang_ver = game::channel()->get_channel_lang(msg.player_rewards(i));
						game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
						game::scheme()->get_server_str(lang_ver, title, "guild_pvp_fight_player_title");
						game::scheme()->get_server_str(lang_ver, text, "guild_pvp_fight_player_text", i + 1);
						PostOperation::post_create(msg.player_rewards(i), title, text, sender, t_rewards->rewards);
					}
					break;
				}
			}
		}
	}
	{
		//game::scheme()->get_server_str(title, "guild_pvp_fight_guild_title");

		std::vector<uint64_t> guids;
		std::vector<dhc::guild_t*> guilds;
		dhc::guild_t *guild;
		game::pool()->get_entitys(et_guild, guids);
		for (int i = 0; i < guids.size(); ++i)
		{
			guild = POOL_GET_GUILD(guids[i]);
			if (guild)
			{
				guilds.push_back(guild);
			}
		}
		dhc::guild_member_t *guild_member = 0;
		for (int i = 0; i < msg.guild_rewards_size(); ++i)
		{
			for (int j = 0; j < guilds.size(); ++j)
			{
				if (guilds[j]->pvp_guild() == msg.guild_rewards(i))
				{
					t_rewards = sGuildConfig->get_guildfight_rank_reward(1, i + 1);
					if (t_rewards)
					{
						//game::scheme()->get_server_str(text, "guild_pvp_fight_guild_text", i + 1);
						for (int k = 0; k < guilds[j]->member_guids_size(); ++k)
						{
							guild_member = POOL_GET_GUILD_MEMBER(guilds[j]->member_guids(k));
							if (guild_member)
							{
								std::string sender;
								std::string title;
								std::string text;
								int lang_ver = game::channel()->get_channel_lang(guild_member->player_guid());
								game::scheme()->get_server_str(lang_ver, text, "guild_pvp_fight_guild_text", i + 1);
								game::scheme()->get_server_str(lang_ver, title, "guild_pvp_fight_guild_title");
								game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
								PostOperation::post_create(guild_member->player_guid(), title, text, sender, t_rewards->rewards);
							}
						}
					}
					break;
				}
			}
		}
	}
}

void PvpManager::terminal_bingyuan_fight_callback(const std::string &data, uint64_t player_guid, const std::string&name, int id)
{
	rpcproto::tmsg_rep_bingyuan_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.res() == false)
	{
		protocol::game::smsg_bingyuan_fight_end smsg;
		smsg.set_bingjing(0);
		smsg.set_point(0);
		ResMessage::res_bingyuan_fight_end(player, smsg, name, id);
		return;
	}

	if (player->by_reward_num() > 0)
	{
		player->set_by_reward_num(player->by_reward_num() - 1);
		PlayerOperation::player_add_resource(player, resource::BINGJING, msg.bingjing(), LOGWAY_BINGYUAN_FIGHT);
		player->set_by_point(player->by_point() + msg.point());
		sHuodongPool->huodong_active(player, HUODONG_COND_BINGYUAN_REWARD_COUNT, 1);
	}

	const s_t_bingyuan_chenghao *t_bingyuan_chenghao = sPvpConfig->get_bingyuan_chenghao(msg.chenghao());
	if (t_bingyuan_chenghao)
	{
		PlayerOperation::player_add_chenghao(player, t_bingyuan_chenghao->cid);
	}
	
	protocol::game::smsg_bingyuan_fight_end smsg;
	smsg.set_bingjing(msg.bingjing());
	smsg.set_point(msg.point());
	ResMessage::res_bingyuan_fight_end(player, smsg, name, id);
}

void PvpManager::terminal_ds_fight_callback(const std::string &data, uint64_t player_guid, const std::string&name, int id)
{
	rpcproto::tmsg_rep_ds_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.res() == false)
	{
		protocol::game::smsg_ds_fight_end smsg;
		smsg.set_xinpian(0);
		smsg.set_point(0);
		smsg.set_ciliao(0);
		smsg.set_duanwei(player->ds_duanwei());
		smsg.set_srank(RankOperation::get_rank(player, e_rank_ds));
		smsg.set_grank(-1);
		ResMessage::res_ds_fight_end(player, smsg, name, id);
		return;
	}

	if (player->ds_reward_num() > 0)
	{
		player->set_ds_reward_num(player->ds_reward_num() - 1);
		PlayerOperation::player_add_resource(player, resource::XINPIAN, msg.xinpian(), LOGWAY_DS_FIGHT);
		ItemOperation::item_add_template(player, 110020001, msg.ciliao(), LOGWAY_DS_FIGHT);
	}
	player->set_ds_hit(player->ds_hit() + 1);
	player->set_ds_point(player->ds_point() + msg.point());
	if (player->ds_point() < 0)
		player->set_ds_point(0);
	RankOperation::check_value(player, e_rank_ds, player->ds_point());
	player->set_ds_duanwei(msg.duanwei());
	if (msg.cd_time() > 0)
		cd_time_[player->guid()] = msg.cd_time();
	else
		cd_time_.erase(player->guid());

	protocol::game::smsg_ds_fight_end smsg;
	smsg.set_xinpian(msg.xinpian());
	smsg.set_point(msg.point());
	smsg.set_ciliao(msg.ciliao());
	smsg.set_duanwei(msg.duanwei());
	smsg.set_srank(RankOperation::get_rank(player, e_rank_ds));
	smsg.set_grank(msg.grank());
	smsg.set_cd_time(msg.cd_time());

	ResMessage::res_ds_fight_end(player, smsg, name, id);
}

void PvpManager::terminal_ds_time_buy_callback(const std::string &data, uint64_t player_guid, const std::string&name, int id)
{
	rpcproto::tmsg_rep_invite_code msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.res() == -1)
	{
		GLOBAL_ERROR;
		return;
	}
	std::map<uint64_t, uint64_t>::const_iterator it = cd_time_.find(player->guid());
	if (it == cd_time_.end())
	{
		GLOBAL_ERROR;
		return;
	}
	uint64_t time = it->second;
	int jewel = ((time - game::timer()->now()) / 1000) / 5;
	if (((time - game::timer()->now()) / 1000) % 5 > 0)
	{
		jewel = jewel + 1;
	}
	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_DS_TIME_BUY);
	cd_time_.erase(player->guid());
	protocol::game::smsg_huodong_fanpai smsg;
	smsg.set_id(jewel);

	ResMessage::res_ds_time_buy(player, smsg, name, id);
}

