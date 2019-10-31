#include "player_load.h"
#include "player_pool.h"
#include "player_def.h"
#include "gs_message.h"
#include "player_operation.h"
#include "role_operation.h"
#include "player_config.h"

int PlayerLoad::load_player(uint64_t player_guid, const std::string &name, int id)
{
	if (query_map_.find(player_guid) != query_map_.end())
	{
		return -1;
	}
	QueryMap qm;
	qm.query_num = 1;
	qm.name = name;
	qm.id = id;
	query_map_[player_guid] = qm;
	Request *req = new Request();
	req->add(opc_query, player_guid, new dhc::player_t);
	game::pool()->upcall(req, boost::bind(&PlayerLoad::load_player_callback, this, _1, true));
	return 0;
}

int PlayerLoad::load_player(uint64_t player_guid, int param, const std::string &msg)
{
	if (query_map_.find(player_guid) != query_map_.end())
	{
		if (msg == "" && param == 0)
		{

		}
		else
		{
			query_map_[player_guid].params.push_back(param);
			query_map_[player_guid].msgs.push_back(msg);
		}
		return -1;
	}
	QueryMap qm;
	qm.query_num = 1;
	qm.name = "";
	qm.id = -1;
	if (msg == "" && param == 0)
	{

	}
	else
	{
		qm.params.push_back(param);
		qm.msgs.push_back(msg);
	}
	query_map_[player_guid] = qm;
	Request *req = new Request();
	req->add(opc_query, player_guid, new dhc::player_t);
	game::pool()->upcall(req, boost::bind(&PlayerLoad::load_player_callback, this, _1, false));
	return 0;
}

void PlayerLoad::load_player_callback(Request *req, bool is_create)
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
				game::pool()->upcall(req, boost::bind(&PlayerLoad::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->equips_size(); ++i)
		{
			if (player->equips(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->equips(i), new dhc::equip_t);
				game::pool()->upcall(req, boost::bind(&PlayerLoad::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->sports_size(); ++i)
		{
			if (player->sports(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->sports(i), new dhc::sport_t);
				game::pool()->upcall(req, boost::bind(&PlayerLoad::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->treasures_size(); ++i)
		{
			if (player->treasures(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->treasures(i), new dhc::treasure_t);
				game::pool()->upcall(req, boost::bind(&PlayerLoad::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->treasure_reports_size(); ++i)
		{
			if (player->treasure_reports(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->treasure_reports(i), new dhc::treasure_report_t);
				game::pool()->upcall(req, boost::bind(&PlayerLoad::load_msg_callback, this, _1, player));
			}
		}

		for (int i = 0; i < player->pets_size(); ++i)
		{
			if (player->pets(i) != 0)
			{
				qm.query_num++;
				Request *req = new Request();
				req->add(opc_query, player->pets(i), new dhc::pet_t);
				game::pool()->upcall(req, boost::bind(&PlayerLoad::load_msg_callback, this, _1, player));
			}
		}

		load_player_check_end(player);
	}
	else if (is_create)
	{
		TermInfo *ti = game::channel()->get_channel(req->guid());
		if (!ti)
		{
			return;
		}
		dhc::player_t *player = create_player(req->guid(), ti->serverid);
		QueryMap &qm = query_map_[player->guid()];
		PlayerOperation::client_login(player, false);
		ResMessage::res_client_login(player, 1, qm.name, qm.id);
		query_map_.erase(req->guid());
	}
}

void PlayerLoad::load_player_check_end(dhc::player_t *player)
{
	QueryMap &qm = query_map_[player->guid()];
	qm.query_num--;
	if (qm.query_num <= 0)
	{
		POOL_ADD(player->guid(), player);
		PlayerOperation::player_login(player);
		if (qm.id == -1)
		{
			sPlayerPool->add_player(player->guid(), false);
		}
		else
		{
			sPlayerPool->add_player(player->guid(), true);
		}

		if (qm.params.size() > 0)
		{
			for (int i = 0; i < qm.params.size(); ++i)
			{
				SelfMessage::self_player_load_end(player->guid(), qm.params[i], qm.msgs[i]);
			}
		}
		
		if (qm.name != "")
		{
			PlayerOperation::client_login(player);
			ResMessage::res_client_login(player, 0, qm.name, qm.id);
		}
		query_map_.erase(player->guid());
	}
}

void PlayerLoad::load_msg_callback(Request *req, dhc::player_t *player)
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

dhc::player_t * PlayerLoad::create_player(uint64_t player_guid, const std::string &serverid)
{
	dhc::player_t *player = new dhc::player_t;
	player->set_guid(player_guid);
	player->set_serverid(serverid);
	player->set_template_id(100);
	player->set_birth_time(game::timer()->now());
	player->set_last_tili_time(game::timer()->now());
	player->set_last_energy_time(game::timer()->now());
	player->set_last_daily_time(game::timer()->now());
	PlayerOperation::player_refresh(player);
	player->set_last_week_time(game::timer()->now());
	PlayerOperation::player_week_refresh(player);
	player->set_last_month_time(game::timer()->now());
	PlayerOperation::player_month_refresh(player);
	player->set_level(1);
	player->set_boss_max_rank(200);
	player->set_dress_flag(1);
	const std::vector<s_t_create> &t_creates = sPlayerConfig->get_create();
	for (int i = 0; i < t_creates.size(); ++i)
	{
		PlayerOperation::player_add_resource(player, (resource::resource_t)t_creates[i].resource, t_creates[i].value);
	}
	player->set_zsname("zsname");
	POOL_ADD_NEW(player_guid, player);

	player->set_ck2_free_time(game::timer()->now());

	PlayerOperation::player_login(player);

	dhc::role_t *role = RoleOperation::role_create(player, 100, 1, 0, 0);
	player->set_zhenxing(0, role->guid());

	sPlayerPool->save_player(player_guid, false);
	sPlayerPool->add_player(player->guid(), true);

	return player;
}
