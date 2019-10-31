#include "player_manager.h"
#include "utils.h"
#include "gs_message.h"
#include "player_load.h"
#include "player_def.h"
#include "player_pool.h"
#include "player_config.h"
#include "player_operation.h"
#include <boost/algorithm/string.hpp>
#include "role_operation.h"
#include "role_config.h"
#include "equip_operation.h"
#include "equip_config.h"
#include "item_operation.h"
#include "item_config.h"
#include "social_operation.h"
#include "huodong_operation.h"
#include "treasure_operation.h"
#include "post_pool.h"
#include "rpc.pb.h"
#include "rank_operation.h"
#include "sport_operation.h"
#include "mission_pool.h"
#include "mission_operation.h"
#include "guild_operation.h"
#include "huodong_pool.h"
#include "pvp_operation.h"
#include "post_operation.h"


PlayerManager::PlayerManager()
: timer_(0),
pinbi_(true)
{

}

PlayerManager::~PlayerManager()
{

}

int PlayerManager::init()
{
	if (-1 == sPlayerConfig->parse())
	{
		return -1;
	}
	timer_ = game::timer()->schedule(boost::bind(&PlayerManager::update, this, _1), PLAYER_PERIOD, "player");
	return 0;
}

int PlayerManager::fini()
{
	sPlayerPool->save_all();
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = 0;
	}
	return 0;
}

int PlayerManager::update(ACE_Time_Value tv)
{
	sPlayerPool->update();
	return 0;
}

void PlayerManager::terminal_client_login(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_client_login msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}
	TermInfo ti;
	ti.username = msg.username();
	ti.password = msg.password();
	ti.serverid = msg.serverid();
	ti.extra = msg.extra();
	ti.device = msg.device();
	ti.version = msg.version();
	ti.platform = msg.platform();
	ti.lang_ver = msg.lang_ver();

	if (msg.username() == "yymoontest")
	{
		acc_login(ti, name, id);
	}
	else
	{
		std::string debug = game::env()->get_game_value("debug");
		if (debug == "0")
		{
			rpcproto::tmsg_req_login_heitao msg1;
			msg1.set_uid(msg.username());
			msg1.set_token(msg.password());
			msg1.set_pt(msg.platform());
			std::string s;
			msg1.SerializeToString(&s);
			game::rpc_service()->request("login1", PMSG_LOGIN_HEITAO, s, boost::bind(&PlayerManager::heitao_login_callback, this, _1, ti, name, id));
		}
		else
		{
			acc_login(ti, name, id);
		}
	}
}

void PlayerManager::heitao_login_callback(const std::string &data, TermInfo &ti, const std::string &name, int id)
{
	rpcproto::tmsg_rep_login_heitao msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.errres() != 0)
	{
		PERROR(ERROR_LOGIN);
		return;
	}
	acc_login(ti, name, id);
}

void PlayerManager::acc_login(TermInfo &ti, const std::string &name, int id)
{
	dhc::acc_t *acc = new dhc::acc_t;
	acc->set_username(ti.username);
	acc->set_serverid(ti.serverid);
	acc->set_device(ti.device);
	acc->set_version(ti.version);
	acc->set_last_device(ti.device);
	acc->set_extra(ti.extra);
	acc->set_last_time(0);
	uint64_t sid = boost::lexical_cast<uint64_t>(ti.serverid);
	acc->set_guid(MAKE_GUID_EX(et_player, sid, 0));

	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_acc, 0), acc);
	
	game::pool()->upcall(req, boost::bind(&PlayerManager::client_login_acc_callback, this, _1, ti, name, id));
}

int PlayerManager::client_login_acc_callback(Request *req, TermInfo &ti, const std::string &name, int id)
{
	if (!req->success())
	{
		GLOBAL_ERROR;
		return 0;
	}
	dhc::acc_t *acc = (dhc::acc_t *)req->data();
	ti.gm_level = acc->gm_level();
	if (acc->fenghao_time() > game::timer()->now())
	{
		PERROR(ERROR_FENGHAO);
		return 0;
	}

	if (ti.platform == "apple")
	{
		bool should_update = false;
		if (acc->device() == "" ||
			acc->version() < ti.version)
		{
			acc->set_device(ti.device);
			acc->set_version(ti.version);
			acc->set_last_device(ti.device);
			acc->set_last_time(0);
			should_update = true;
		}
		else
		{
			if (acc->device() != ti.device)
			{
				if (ti.device != acc->last_device())
				{
					acc->set_last_device(ti.device);
					acc->set_last_time(game::timer()->now() + 7 * 24 * 60 * 60 * 1000);
					should_update = true;
				}
			}
		}

		if (should_update)
		{
			Request *req = new Request(); 
			dhc::acc_t *cmsg = new dhc::acc_t;
			cmsg->CopyFrom(*acc);
			req->add(opc_update, MAKE_GUID(et_acc, 0), cmsg);
			game::pool()->upcall(req, 0);
		}
		ti.device = acc->device();
		ti.version = acc->version();
		ti.device_time = acc->last_time();
	}
	
	std::string s = "";
	for (int i = 0; i < 16; ++i)
	{
		int a = Utils::get_int32(0, 35);
		if (a < 10)
		{
			s += char('0' + a);
		}
		else
		{
			s += char('a' - 10 + a);
		}
	}
	ti.sig = s;
	ti.gag_time = acc->gag_time();
	uint64_t guid = acc->guid();
	game::channel()->add_channel(guid, ti, true);
	dhc::player_t *player = POOL_GET_PLAYER(guid);
	if (!player)
	{
		if (-1 == sPlayerLoad->load_player(guid, name, id))
		{
			GLOBAL_ERROR;
			return 0;
		}
	}
	else
	{
		player->set_gag_time(ti.gag_time);   //禁言时间
		PlayerOperation::player_login(player);
		PlayerOperation::client_login(player);
		ResMessage::res_client_login(player, 0, name, id);
	}

	return 0;
}

void PlayerManager::terminal_player_name(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_player_name msg;
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

	if (player->level() != 1)
	{
		GLOBAL_ERROR;
		return;
	}
	std::string player_name = msg.name();
	if (player_name.empty())
	{
		PERROR(ERROR_NAME_EMPTY);
		return;
	}
	if (player_name.size() > 24)
	{
		PERROR(ERROR_NAME_LONG);
		return;
	}
	if (player_name.find(" ") != std::string::npos)
	{
		PERROR(ERROR_NAME_ILL);
		return;
	}
	if (game::scheme()->search_illword(player_name, false, false) == -1)
	{
		PERROR(ERROR_NAME_ILL);
		return;
	}
	if (pinbi_)
	{
		if (game::scheme()->valid_name(player_name) == false)
		{
			PERROR(ERROR_NAME_ILL);
			return;
		}
	}
	if (names_.find(player_name) != names_.end())
	{
		PERROR(ERROR_NAME_HAS);
		return;
	}
	NameStr *ns = new NameStr;
	ns->name = player_name;
	game::pool()->namecall(ns, boost::bind(&PlayerManager::player_name_callback, this, _1, player_guid, player_name, name, id));
}

void PlayerManager::terminal_player_change_nalflag(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_player_nalflag msg;
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
	const s_t_price* t_price = sPlayerConfig->get_price(player->change_nalflag_num() + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	int before_nalflag = player->nalflag();
	player->set_change_nalflag_num(player->change_nalflag_num() + 1);
	player->set_nalflag(msg.nalflag());
	PlayerOperation::refresh_player_name_nalflag(player, player->name(),before_nalflag);

	ResMessage::res_success(player, true, name, id);
	
}
void PlayerManager::terminal_player_change_name(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_player_name msg;
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

	std::string player_name = msg.name();
	if (player_name.empty())
	{
		PERROR(ERROR_NAME_EMPTY);
		return;
	}
	if (player_name.size() > 24)
	{
		PERROR(ERROR_NAME_LONG);
		return;
	}
	if (player_name.find(" ") != std::string::npos)
	{
		PERROR(ERROR_NAME_ILL);
		return;
	}
	if (game::scheme()->search_illword(player_name, false, false) == -1)
	{
		PERROR(ERROR_NAME_ILL);
		return;
	}
	if (pinbi_)
	{
		if (game::scheme()->valid_name(player_name) == false)
		{
			PERROR(ERROR_NAME_ILL);
			return;
		}
	}
	if (names_.find(player_name) != names_.end())
	{
		PERROR(ERROR_NAME_HAS);
		return;
	}
	const s_t_price* t_price = sPlayerConfig->get_price(player->change_name_num() + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < t_price->change_name)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	NameStr *ns = new NameStr;
	ns->name = player_name;
	game::pool()->namecall(ns, boost::bind(&PlayerManager::player_change_name_callback, this, _1, player_guid, player_name, name, id));
}

void PlayerManager::player_name_callback(NameStr *ns, uint64_t player_guid, const std::string &player_name, const std::string &name, int id)
{
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	if (ns->success() && ns->guid.size() > 0)
	{
		PERROR(ERROR_NAME_HAS);
		return;
	}
	
	player->set_name(player_name);
	names_.insert(player_name);
	sPlayerPool->add_name_player(player->name(), player->guid());
	ResMessage::res_player_name(player, 0, name, id);
}

void PlayerManager::player_change_name_callback(NameStr *ns, uint64_t player_guid, const std::string &player_name, const std::string &name, int id)
{
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	const s_t_price* t_price = sPlayerConfig->get_price(player->change_name_num() + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	if (ns->success() && ns->guid.size() > 0)
	{
		PERROR(ERROR_NAME_HAS);
		return;
	}
	std::string before_name = player->name();

	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_price->change_name, LOGWAY_CHANGE_NAME);
	player->set_change_name_num(player->change_name_num() + 1);
	player->set_name(player_name);
	names_.insert(player_name);
	sPlayerPool->add_name_player(player->name(), player->guid());
	PlayerOperation::refresh_player_name_nalflag(player, before_name, -1);

	protocol::self::self_boss_change_name self_msg;
	self_msg.set_player_guid(player->guid());
	self_msg.set_player_name(player->name());
	std::string self_str;
	self_msg.SerializeToString(&self_str);
	SelfMessage::self_boss_change_name(player->guid(), self_str);

	ResMessage::res_success(player, true, name, id);
}

void PlayerManager::terminal_player_zsname(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_player_zsname msg;
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

	std::string zsname = msg.zsname();
	if (zsname.empty())
	{
		PERROR(ERROR_NAME_EMPTY);
		return;
	}
	if (zsname.size() > 24)
	{
		PERROR(ERROR_NAME_LONG);
		return;
	}
	if (game::scheme()->search_illword(zsname, false,false) == -1)
	{
		PERROR(ERROR_NAME_ILL);
		return;
	}
	player->set_zsname(zsname);

	ResMessage::res_success(player, true, name, id);
}

void PlayerManager::terminal_gm_command(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_gm_command msg;
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

	if (game::env()->get_game_value("debug") == "0")
	{
		TermInfo *ti = game::channel()->get_channel(player->guid());
		if (!ti)
		{
			GLOBAL_ERROR;
			return;
		}
		if (ti->gm_level == 0)
		{
			GLOBAL_ERROR;
			return;
		}
	}

	std::vector<std::string> texts;
	boost::split(texts, msg.text(), boost::is_any_of(" "));

	if (texts.size() == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	if (texts[0] == "addreward")
	{
		if (texts.size() < 5)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			s_t_reward t_reward;
			t_reward.type = boost::lexical_cast<int>(texts[1]);
			t_reward.value1 = boost::lexical_cast<int>(texts[2]);
			t_reward.value2 = boost::lexical_cast<int>(texts[3]);
			t_reward.value3 = boost::lexical_cast<int>(texts[4]);
			rds.add_reward(t_reward);
			PlayerOperation::player_add_reward(player, rds, LGOWAY_GM_COMMAND);

			ResMessage::res_gm_command(player, t_reward.type, t_reward.value1, t_reward.value2, t_reward.value3, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "addmofang")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
		if (!huodong_player)
		{
			GLOBAL_ERROR;
			return;
		}

		dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
		if (!huodong)
		{
			GLOBAL_ERROR;
			return;
		}

		try
		{
			int point = boost::lexical_cast<int>(texts[1]);
			huodong_player->set_arg3(huodong_player->arg3() + point);
			huodong_player->set_arg4(huodong_player->arg4() + point);
			huodong->set_extra_data1(huodong->extra_data1() + point);
			sHuodongPool->save_huodong_player(huodong_player->guid());
			ResMessage::res_gm_command(player, 0, 10, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "startpvp")
	{
		SelfMessage::self_start_guild_boss();
		ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
	}
	else if (texts[0] == "unlockmission")
	{
		if (texts.size() < 3)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int lock_id = boost::lexical_cast<int>(texts[1]);
			int jylock_id = boost::lexical_cast<int>(texts[2]);
			player->set_mission(lock_id);
			player->set_mission_jy(jylock_id);
			ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "randomevent")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int event_id = boost::lexical_cast<int>(texts[1]);
			player->set_random_event_id(event_id);
			player->set_random_event_time(0);
			player->set_random_event_num(0);
			ResMessage::res_gm_command(player, 0, 1, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "refresh_day")
	{
		PlayerOperation::player_refresh(player);
		ResMessage::res_gm_command(player, 0, 2, 0, 0, rds, name, id);
	}
	else if (texts[0] == "refresh_week")
	{
		PlayerOperation::player_week_refresh(player);
		ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
	}
	else if (texts[0] == "refresh_qiyu")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}

		try
		{
			int daytype = boost::lexical_cast<int>(texts[1]);
			if (daytype < 0 || daytype > 3)
			{
				GLOBAL_ERROR;
				return;
			}
			if (daytype == 3)
			{
				player->clear_qiyu_mission();
				player->clear_qiyu_hard();
				player->clear_qiyu_suc();
			}
			else
			{
				MissionOperation::refresh_qiyu(player, daytype);
			}
			ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "addguildexp")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int exp = boost::lexical_cast<int>(texts[1]);
			dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
			if (guild)
			{
				GuildOperation::mod_guild_exp(guild, exp);
			}
			ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "addguildhonor")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int honor = boost::lexical_cast<int>(texts[1]);
			dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
			if (guild)
			{
				guild->set_honor(guild->honor() + honor);
			}
			ResMessage::res_gm_command(player, 0, 6, honor, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "addguildmember")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int num = boost::lexical_cast<int>(texts[1]);
			for (int jk = 0; jk < num; ++jk)
			{
				gm_create_player(player, player->serverid());
			}
			ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "setguildfight")
	{
		if (texts.size() < 3)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int day = boost::lexical_cast<int>(texts[1]);
			int hour = boost::lexical_cast<int>(texts[2]);
			if (day < 0)
			{
				if (day == -2)
				{
					dhc::global_t *glob = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
					if (glob)
					{
						glob->set_guild_pvp_zhou(hour);
					}
				}
				else if (day == -5)
				{
					dhc::pvp_list_t *guild_rank_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_GUILD_REWARD));
					if (guild_rank_list)
					{
						guild_rank_list->set_sync_flag(1);
						guild_rank_list->set_refresh_time(game::timer()->now());
						guild_rank_list->clear_player_guids();
						std::vector<uint64_t> guids;
						std::vector<dhc::guild_t*> guilds;
						dhc::guild_t *guild;
						game::pool()->get_entitys(et_guild, guids);
						for (int i = 0; i < guids.size(); ++i)
						{
							guild = POOL_GET_GUILD(guids[i]);
							if (guild && guild->juntuan_apply())
							{
								guild->set_juntuan_apply(0);
								dhc::guild_member_t *guild_member = 0;
								for (int kmg = 0; kmg < guild->member_guids_size(); ++kmg)
								{
									guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(kmg));
									if (guild_member)
									{
										guild_rank_list->add_player_guids(guild_member->player_guid());
									}
								}
							}
						}
						POOL_SAVE(dhc::pvp_list_t, guild_rank_list, false);
					}

					dhc::global_t *glob = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
					if (glob)
					{
						glob->set_guild_pvp_zhou(glob->guild_pvp_zhou() + 1);
					}
				}
			}
			else
			{
				GuildOperation::set_guild_pvp_state(day, hour);
			}
			
			rpcproto::tmsg_push_guild_match rmsg;
			rmsg.set_week(day);
			rmsg.set_hour(hour);
			std::string s;
			rmsg.SerializeToString(&s);
			game::rpc_service()->push("remote1", PMSG_GUILDFIGHT_MATCH, s);
			ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "mission")
	{
		player->set_last_leave_guild_time(player->last_leave_guild_time() - 24 * 60 * 60 * 1000);
		ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
	}
	else if (texts[0] == "adddress")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int dress_id = boost::lexical_cast<int>(texts[1]);
			if (!EquipOperation::player_has_dress(player, dress_id))
			{
				player->add_dress_ids(dress_id);
			}
			ResMessage::res_gm_command(player, 0, 3, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "addday")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int day = boost::lexical_cast<int>(texts[1]);
			player->set_birth_time(game::timer()->now() - day * 86400000);
			ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "delete_rank")
	{
		RankOperation::del_player(player->guid());
		ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
	}
	else if (texts[0] == "yxhg")
	{
		player->set_last_login_time(player->last_login_time() - 4 * 24 * 60 * 60 * 1000);
		ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
	}
	else if (texts[0] == "dsls")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			rpcproto::tmsg_req_bingyuan_buy rmsg;
			int point = boost::lexical_cast<int>(texts[1]);
			rmsg.set_guid(player->guid());
			rmsg.set_num(point);
			std::string s;
			rmsg.SerializeToString(&s);
			game::rpc_service()->push("remote1", PMSG_DS_GM, s);
			ResMessage::res_gm_command(player, 0, 0, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "setczjh")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int day = boost::lexical_cast<int>(texts[1]);
			dhc::global_t *t_global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
			if (t_global)
			{
				t_global->set_czjh_count(day);
			}
			ResMessage::res_gm_command(player, 0, 9, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (texts[0] == "guanbi")
	{
		if (texts.size() < 2)
		{
			GLOBAL_ERROR;
			return;
		}
		try
		{
			int guanbi = boost::lexical_cast<int>(texts[1]);
			if (guanbi)
			{
				pinbi_ = false;
			}
			else
			{
				pinbi_ = true;
			}
			ResMessage::res_gm_command(player, 0, 18, 0, 0, rds, name, id);
		}
		catch (...)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else
	{
		GLOBAL_ERROR;
		return;
	}
}

void PlayerManager::terminal_client_reload(const std::string &data, const std::string &name, int id)
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

	ResMessage::res_client_login(player, 0, name, id);
}

void PlayerManager::terminal_player_task(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_player_task msg;
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

	s_t_task *t_task = sPlayerConfig->get_task(msg.task_id());
	if (!t_task)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->finished_tasks_size(); ++i)
	{
		if (player->finished_tasks(i) == t_task->id)
		{
			PERROR(ERROR_TASK_FINIED);
			return;
		}
	}
	if (t_task->pid)
	{
		bool flag = false;
		for (int i = 0; i < player->finished_tasks_size(); ++i)
		{
			if (player->finished_tasks(i) == t_task->pid)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			PERROR(ERROR_TASK_PID);
			return;
		}
	}
	if (t_task->type == 1)
	{
		if (player->level() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 3)
	{
		int num = player->role_template_ids_size();
		if (num < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 5)
	{
		if (player->mission() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 6)
	{
		if (player->pt_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 7)
	{
		if (player->sj_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 8)
	{
		if (player->jjie_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 9)
	{
		if (player->qh_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 10)
	{
		if (player->jj_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 11)
	{
		if (player->boss_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 12)
	{
		if (player->jy_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 13)
	{
		if (player->hs_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 14)
	{
		if (player->ttt_task_num() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 15)
	{
		if (player->mission_jy() < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 16)
	{
		if (MissionOperation::get_mission_star(player, 0) < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 17)
	{
		if (MissionOperation::get_mission_star(player, 1) < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 18)
	{
		int bf = PlayerOperation::player_calc_force(player);
		if (bf < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 19)
	{
		int star_num = 0;
		for (int i = 0; i < player->ttt_last_stars_size(); ++i)
		{
			star_num += player->ttt_last_stars(i);
		}
		if (star_num < t_task->def1)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_task->type == 20)
	{
		if (SocialOperation::get_social_code_pcount(player, t_task->def1) < t_task->num)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
		SocialOperation::add_social_code_id(player, t_task->id);
	}
	else
	{
		GLOBAL_ERROR;
		return;
	}

	player->add_finished_tasks(t_task->id);

	s_t_rewards rds;
	rds.add_reward(t_task->reward);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TASK);

	ResMessage::res_success(player, true, name, id);
}

void PlayerManager::terminal_active_reward(const std::string &data, const std::string &name, int id)
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

	s_t_active *t_active = sPlayerConfig->get_active(msg.id());
	if (!t_active)
	{
		GLOBAL_ERROR;
		return;
	}

	int index = -1;
	for (int i = 0; i < player->active_id_size(); ++i)
	{
		if (t_active->id == player->active_id(i))
		{
			index = i;
		}
	}
	if (index == -1 || player->active_num(index) < t_active->num)
	{
		PERROR(ERROR_TASK_TJ);
		return;
	}

	if (player->active_reward(index) == 1)
	{
		PERROR(ERROR_TASK_FINIED);
		return;
	}

	if (t_active->id == 100)
	{
		int hour = game::timer()->hour();
		if (hour != 12 && hour != 13)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}
	else if (t_active->id == 101)
	{
		int hour = game::timer()->hour();
		if (hour != 18 && hour != 19)
		{
			PERROR(ERROR_TASK_TJ);
			return;
		}
	}

	s_t_rewards rds;
	rds.add_reward(t_active->reward);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ACTIVE);
	if (t_active->score > 0)
	{
		PlayerOperation::player_add_resource(player, resource::ACTIVE, t_active->score, LOGWAY_ACTIVE);
		if (player->active_score() > sPlayerConfig->get_max_score())
		{
			player->set_active_score(sPlayerConfig->get_max_score());
		}
	}

	player->set_active_reward(index, 1);

	ResMessage::res_active_reward(player, rds, name, id);
}

void PlayerManager::terminal_active_score_reward(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_active_score_reward msg;
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

	int reward_id = msg.reward_id();
	s_t_active_reward *t_reward = sPlayerConfig->get_active_reward(reward_id);
	if (!t_reward)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->active_score() < t_reward->score)
	{
		PERROR(ERROR_SCORE_NOT_ENOUTH);
		return;
	}

	for (int i = 0; i != player->active_score_id_size(); ++i)
	{
		if (reward_id == player->active_score_id(i))
		{
			PERROR(ERROR_HAS_FIRST_REWARD);
			return;
		}
	}
	
	player->add_active_score_id(reward_id);

	s_t_rewards rds;
	rds.add_reward(t_reward->rewards);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ACTIVE_REWARD);

	ResMessage::res_active_score_reward(player, rds, name, id);
}

void PlayerManager::terminal_recharge(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_recharge msg;
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

	if (game::env()->get_game_value("debug") == "0")
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.huodong_id() == 0 && msg.entry_id() == 0)
	{
		PlayerOperation::player_recharge(player, msg.id(), 0);
		PlayerOperation::add_player_chongzhi(player_guid, msg.id(), 0);
	}
	else
	{
		s_t_rewards rds;
		sHuodongPool->huodong_zhichong(player, msg.huodong_id(), msg.entry_id(), rds);
		PlayerOperation::player_add_reward(player, rds, LOGWAY_RECHARGE);
		PlayerOperation::add_player_zhichong(player_guid, rds);
		PlayerOperation::player_recharge_zc(player, msg.id(), 0);
	}
	ResMessage::res_success(player, true, name, id);
}

void PlayerManager::terminal_recharge_heitao(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_recharge_heitao msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}
	dhc::acc_t *acc = new dhc::acc_t;
	acc->set_username(msg.uid());
	acc->set_serverid(msg.sid());
	acc->set_guid(0);

	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_acc, 0), acc);
	game::pool()->upcall(req, boost::bind(&PlayerManager::recharge_heitao_acc_callback, this, _1, msg.pid(), msg.order(), msg.count(), name, id, msg.huodong_id(), msg.entry_id()));
}

void PlayerManager::self_player_load_recharge(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}
	std::string name = msg.name();
	int id = msg.id();
	protocol::self::self_player_load_recharge msg1;
	if (!msg1.ParseFromString(msg.data()))
	{
		GLOBAL_ERROR;
		return;
	}
	recharge_heitao_acc_callback1(msg1.player_guid(), msg1.pid(), msg1.order(), 0, name, id, msg1.huodong_id(), msg1.entry_id());
}

int PlayerManager::recharge_heitao_acc_callback(Request *req, int pid, const std::string &order, int count, const std::string &name, int id, int huodong_id, int entry_id)
{
	if (!req->success())
	{
		GLOBAL_ERROR;
		return 0;
	}
	dhc::acc_t *acc = (dhc::acc_t *)req->data();
	uint64_t player_guid = acc->guid();
	recharge_heitao_acc_callback1(player_guid, pid, order, count, name, id, huodong_id, entry_id);
	return 0;
}

void PlayerManager::recharge_heitao_acc_callback1(uint64_t player_guid, int pid, const std::string &order, int count, const std::string &name, int id, int huodong_id, int entry_id)
{
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		protocol::self::self_player_load_recharge msg1;
		msg1.set_player_guid(player_guid);
		msg1.set_pid(pid);
		msg1.set_order(order);
		msg1.set_count(count);
		msg1.set_huodong_id(huodong_id);
		msg1.set_entry_id(entry_id);
		std::string ss;
		msg1.SerializeToString(&ss);
		protocol::self::self_player_load msg;
		msg.set_data(ss);
		msg.set_name(name);
		msg.set_id(id);
		std::string s;
		msg.SerializeToString(&s);
		sPlayerLoad->load_player(player_guid, SELF_PLAYER_LOAD_RECHARGE, s);
		return;
	}

	if (order == "moni")
	{
		PlayerOperation::player_recharge(player, pid, count);
		if (huodong_id != 0 && entry_id != 0)
		{
			s_t_rewards rds;
			sHuodongPool->huodong_zhichong(player, huodong_id, entry_id, rds);
			PlayerOperation::player_add_reward(player, rds, LOGWAY_RECHARGE);
		}
		ResMessage::res_success_ex(true, name, id);
		return;
	}
	int rid = pid;
	s_t_recharge *t_recharge = sPlayerConfig->get_recharge(rid);
	if (!t_recharge)
	{
		rid = 999;
	}
	dhc::recharge_heitao_t *rh = new dhc::recharge_heitao_t;
	rh->set_orderno(order);
	rh->set_rid(rid);
	rh->set_player_guid(player_guid);

	Request *req1 = new Request();
	req1->add(opc_insert, MAKE_GUID(et_reharge_heitao, 0), rh);
	game::pool()->upcall(req1, boost::bind(&PlayerManager::recharge_heitao_order_callback, this, _1, player_guid, pid, count, name, id, huodong_id, entry_id));
}

int PlayerManager::recharge_heitao_order_callback(Request *req, uint64_t player_guid, int pid, int count, const std::string &name, int id, int huodong_id, int entry_id)
{
	if (!req->success())
	{
		GLOBAL_ERROR;
		return 0;
	}
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return 0;
	}
	if (huodong_id == 0 && entry_id == 0)
	{
		PlayerOperation::player_recharge(player, pid, count);
		PlayerOperation::add_player_chongzhi(player_guid, pid, count);
	}
	else
	{
		s_t_rewards rds;
		sHuodongPool->huodong_zhichong(player, huodong_id, entry_id, rds);
		PlayerOperation::player_add_reward(player, rds, LOGWAY_RECHARGE);
		PlayerOperation::add_player_zhichong(player_guid, rds);
		PlayerOperation::player_recharge_zc(player, pid, 0);
	}
	
	ResMessage::res_success_ex(true, name, id);
	return 0;
}

void PlayerManager::terminal_recharge_check_ex(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common_ex msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	PCK_CHECK_EX
		s_t_rewards rds;
	s_t_rewards *zhichong_pairs = PlayerOperation::get_player_zhichong(player_guid);
	if (zhichong_pairs)
	{
		ResMessage::res_recharge_check_ex(1, 0, 0, *zhichong_pairs, name, id);
		PlayerOperation::remove_player_zhichong(player_guid);
	}
	std::pair<int, int> *chongzhi_pairs = PlayerOperation::get_player_chongzhi(player_guid);
	if (chongzhi_pairs)
	{
		int pid;
		int cz_count;
		pid = chongzhi_pairs->first;
		cz_count = chongzhi_pairs->second;
		ResMessage::res_recharge_check_ex(0, pid, cz_count, rds, name, id);
		PlayerOperation::remove_player_chongzhi(player_guid);
	}
}

void PlayerManager::terminal_first_recharge(const std::string &data, const std::string &name, int id)
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

	if (player->first_reward() != 1)
	{
		GLOBAL_ERROR;
		return;
	}
	player->set_first_reward(2);
	s_t_rewards rds;
	sPlayerConfig->get_first_recharge(rds.rewards);
	
	PlayerOperation::player_add_reward(player, rds, LOGWAY_FIRST_RECHARGE);
	SocialOperation::gundong_server("t_server_language_text_shouchong", player->name(), "", "");

	ResMessage::res_first_recharge(player, rds, name, id);
}

void PlayerManager::terminal_vip_reward(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_vip_reward msg;
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

	if (msg.vip() > player->vip())
	{
		GLOBAL_ERROR;
		return;
	}

	bool flag = false;
	for (int i = 0; i < player->vip_reward_ids_size(); ++i)
	{
		if (msg.vip() == player->vip_reward_ids(i))
		{
			flag = true;
			break;
		}
	}
	if (flag)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_vip *t_vip = sPlayerConfig->get_vip(msg.vip());
	if (!t_vip)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < t_vip->jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}


	s_t_rewards rds;
	rds.add_reward(t_vip->reward);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_VIP_REWARD);
	player->add_vip_reward_ids(t_vip->level);

	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_vip->jewel, LOGWAY_VIP_REWARD);

	ResMessage::res_vip_reward(player, rds, name, id);
}

void PlayerManager::terminal_dj(const std::string &data, const std::string &name, int id)
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

	s_t_price *t_price = sPlayerConfig->get_price(player->dj_num() + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_vip *t_vip = sPlayerConfig->get_vip(player->vip());
	if (!t_vip)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_vip->dj_num <= player->dj_num())
	{
		PERROR(ERROR_DJ_NUM);
		return;
	}
	if (player->jewel() < t_price->dj)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	int bs = 1;
	int rad = Utils::get_int32(0, 99);
	if (rad < 1)
	{
		bs = 10;
		SocialOperation::gundong_server("t_server_language_text_lianjin", player->name(), "", "");
	}
	else if (rad < 6)
	{
		bs = 5;
	}
	else if (rad < 26)
	{
		bs = 2;
	}
	int gold = 40000 * bs * (1 + player->level() * 0.008);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_price->dj, LOGWAY_DIANJING);
	player->set_dj_num(player->dj_num() + 1);
	PlayerOperation::player_add_resource(player, resource::GOLD, gold, LOGWAY_DIANJING);
	PlayerOperation::player_add_active(player, 850, 1);

	ResMessage::res_dj_reward(player, bs, gold, name, id);
}

void PlayerManager::terminal_djten(const std::string &data, const std::string &name, int id)
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

	s_t_vip *t_vip = sPlayerConfig->get_vip(player->vip());
	if (!t_vip)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_vip->dj_num <= player->dj_num())
	{
		PERROR(ERROR_DJ_NUM);
		return;
	}
	int num = 10;
	if (t_vip->dj_num - player->dj_num() < 10)
	{
		num = t_vip->dj_num - player->dj_num();
	}
	int jewel = 0;
	for (int i = player->dj_num() + 1; i <= player->dj_num() + num; ++i)
	{
		s_t_price *t_price = sPlayerConfig->get_price(i);
		if (!t_price)
		{
			GLOBAL_ERROR;
			return;
		}
		jewel += t_price->dj;
	}
	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	int gold = 0;
	for (int i = player->dj_num() + 1; i <= player->dj_num() + num; ++i)
	{
		int bs = 1;
		int rad = Utils::get_int32(0, 99);
		if (rad < 1)
		{
			bs = 10;
			//std::string text;
			//game::scheme()->get_server_str(text, "lianjin", player->name().c_str());
			//SocialOperation::gundong(text);
			SocialOperation::gundong_server("t_server_language_text_lianjin", player->name(), "", "");
		}
		else if (rad < 6)
		{
			bs = 5;
		}
		else if (rad < 26)
		{
			bs = 2;
		}
		gold += 40000 * bs * (1 + player->level() * 0.008);
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_DIANJING);
	player->set_dj_num(player->dj_num() + num);
	PlayerOperation::player_add_resource(player, resource::GOLD, gold, LOGWAY_DIANJING);
	PlayerOperation::player_add_active(player, 850, num);

	ResMessage::res_dj_reward(player, num, gold, name, id);
}

void PlayerManager::terminal_online_reward(const std::string &data, const std::string &name, int id)
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

	s_t_online_reward *t_online_reward = sPlayerConfig->get_online_reward(player->online_reward_index());
	if (!t_online_reward)
	{
		GLOBAL_ERROR;
		return;
	}

	if (game::timer()->now() < player->online_reward_time())
	{
		PERROR(ERROR_ONLINE_REWARD_TIME);
		return;
	}

	s_t_rewards rds;
	rds.add_reward(t_online_reward->rewards);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ONLINE_REWARD);

	player->set_online_reward_index(player->online_reward_index() + 1);
	s_t_online_reward *t_online_reward1 = sPlayerConfig->get_online_reward(player->online_reward_index());
	if (t_online_reward1)
	{
		player->set_online_reward_time(game::timer()->now() + t_online_reward1->time);
	}

	ResMessage::res_online_reward(player, rds, name, id);
}

void PlayerManager::terminal_daily_sign(const std::string &data, const std::string &name, int id)
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

	int index = player->daily_sign_reward();
	if (index >= player->daily_sign_index())
	{
		GLOBAL_ERROR;
		return;
	}
	int rindex = index % 30;
	s_t_daily_sign *t_daily_sign = sPlayerConfig->get_daily_sign(rindex + 1);
	if (!t_daily_sign)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	if (index >= 30)
	{
		rds.add_reward(t_daily_sign->reward1);
		if (t_daily_sign->vip1 > 0 && player->vip() >= t_daily_sign->vip1)
		{
			rds.add_reward(t_daily_sign->reward1);
		}
	}
	else
	{
		rds.add_reward(t_daily_sign->reward);
		if (t_daily_sign->vip > 0 && player->vip() >= t_daily_sign->vip)
		{
			rds.add_reward(t_daily_sign->reward);
		}
	}
	PlayerOperation::player_add_reward(player, rds, LOGWAY_DAILY_SIGN);
	player->set_daily_sign_reward(player->daily_sign_reward() + 1);
	player->set_daily_sign_num(player->daily_sign_num() + 1);

	ResMessage::res_daily_sign(player, rds, name, id);
}

void PlayerManager::terminal_daily_sign_reward(const std::string &data, const std::string &name, int id)
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

	if (player->daily_sign_num() < 30)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->daily_sign_flag() != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_add_resource(player, resource::VIP_EXP, 1000, LOGWAY_DAILY_SIGN_REWARD);
	player->set_daily_sign_flag(1);

	ResMessage::res_success(player, true, name, id);
}

void PlayerManager::terminal_player_look(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_player_look msg;
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

	uint64_t target_guid = msg.target_guid();
	dhc::player_t *target = POOL_GET_PLAYER(target_guid);
	if (!target)
	{
		protocol::self::self_player_load msg;
		msg.set_data(data);
		msg.set_name(name);
		msg.set_id(id);
		std::string s;
		msg.SerializeToString(&s);
		sPlayerLoad->load_player(target_guid, SELF_PLAYER_LOAD_LOOK, s);
		return;
	}
	else
	{
		game::channel()->refresh_offline_time(target_guid);
	}

	std::vector<dhc::role_t *> roles;
	std::vector<dhc::equip_t *> equips;
	std::vector<dhc::treasure_t*> treasures;
	std::vector<int> role_sxs;
	std::vector<dhc::pet_t*> pets;
	std::vector<int> pet_sxs;
	for (int i = 0; i < target->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(target->zhenxing(i));
		if (!role)
		{
			roles.push_back(0);
			for (int j = 1; j <= 5; ++j)
			{
				role_sxs.push_back(0);
			}
			pets.push_back(0);
			for (int j = 1; j <= 4; ++j)
			{
				pet_sxs.push_back(0);
			}
		}
		else
		{
			roles.push_back(role);
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (!equip)
				{
					equips.push_back(0);
				}
				else
				{
					equips.push_back(equip);
				}
			}
			for (int j = 0; j < role->treasures_size(); ++j)
			{
				dhc::treasure_t *treasure = POOL_GET_TREASURE(role->treasures(j));
				if (!treasure)
				{
					treasures.push_back(0);
				}
				else
				{
					treasures.push_back(treasure);
				}
			}
			std::map<int, double> role_attrs;
			RoleOperation::get_role_attr(target, role, role_attrs);
			/// 百分比计算
			for (int j = 1; j <= 5; ++j)
			{
				role_attrs[j] = role_attrs[j] * (1 + role_attrs[j + 5] * 0.01f);
				role_sxs.push_back(role_attrs[j]);
			}
			dhc::pet_t *pet = POOL_GET_PET(role->pet());
			if (pet)
			{
				pets.push_back(pet);
				std::map<int, double> pet_attrs;
				RoleOperation::get_pet_attr(player, pet, pet_attrs, true, false, true);
				for (int j = 1; j <= 4; ++j)
				{
					pet_sxs.push_back(pet_attrs[j]);
				}

			}
			else
			{
				pets.push_back(0);
				for (int j = 1; j <= 4; ++j)
				{
					pet_sxs.push_back(0);
				}
			}
		}
	}
	dhc::pet_t *pet_on = POOL_GET_PET(target->pet_on());
	if (pet_on)
	{
		pets.push_back(pet_on);
		std::map<int, double> pet_attrs;
		RoleOperation::get_pet_attr(player, pet_on, pet_attrs, true, false, true);
		for (int j = 1; j <= 4; ++j)
		{
			pet_sxs.push_back(pet_attrs[j]);
		}
	}
	else
	{
		pets.push_back(0);
		for (int j = 1; j <= 4; ++j)
		{
			pet_sxs.push_back(0);
		}
	}
	
	ResMessage::res_player_look(player, target, roles, equips, treasures, role_sxs, pets, pet_sxs, name, id);
}

void PlayerManager::self_player_load_look(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_player_look(msg.data(), msg.name(), msg.id());
}

void PlayerManager::terminal_random_event_look(const std::string &data, const std::string &name, int id)
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

	if (player->level() < RANDOM_EVENT_LEVEL)
	{
		PERROR(ERROR_LEVEL);
		return;
	}
	if (player->random_event_time() > game::timer()->now())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->random_event_num() >= RANDOM_EVENT_NUM)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->random_event_id() == 0)
	{
		player->set_random_event_id(sPlayerConfig->get_random_event(player));
	}
	ResMessage::res_random_event_look(player, player->random_event_id(), name, id);
}

void PlayerManager::terminal_random_event_get(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_random_event_get msg;
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

	if (player->level() < RANDOM_EVENT_LEVEL)
	{
		PERROR(ERROR_LEVEL);
		return;
	}
	if (player->random_event_time() > game::timer()->now())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->random_event_id() == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_random_event *t_random_event = sPlayerConfig->get_random_event(player->random_event_id());
	if (!t_random_event)
	{
		GLOBAL_ERROR;
		return;
	}

	int index = msg.index();
	if (index < 0 || index >= t_random_event->xuan_rewards.size())
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_random_event_id(0);
	player->set_random_event_time(game::timer()->now() + Utils::get_int32(20, 60) * 60000);
	player->set_random_event_num(player->random_event_num() + 1);

	ResMessage::res_random_event_get(player, player->random_event_time(), name, id);
}

void PlayerManager::terminal_player_check(const std::string &data, const std::string &name, int id)
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

	player->set_last_check_time(game::timer()->now());

	protocol::game::smsg_player_check smsg;
	std::vector<dhc::post_t *> posts;
	sPostPool->get_player_post_list(player->guid(), posts);
	smsg.set_post(posts.size());
	smsg.set_friend_apply(SocialOperation::get_is_friend_apply(player));
	smsg.set_friend_tili(SocialOperation::get_is_friend_tili(player));
	smsg.set_kaifu((HuodongOperation::has_kaifu_huodong_complete(player) ? 1 : 0));
	smsg.set_pttq((HuodongOperation::has_pttq_huodong_complete(player) ? 1 : 0));
	smsg.set_yb((sMissionPool->is_yb_effect(player) ? 1 : 0));
	smsg.set_qd((TreasureOperation::treasure_is_rob(player) ? 1 : 0));
	smsg.set_jjc(SportOperation::sport_can_get(player));
	smsg.set_shop_refresh(PlayerOperation::get_shop_refresh_flag(player));
	smsg.set_guild(GuildOperation::has_red_point(player));
	sHuodongPool->huodong_jiri(player, smsg);
	sHuodongPool->huodong_xingheqingdian(player, smsg);
	smsg.set_bingyuan(sPvpList->get_social_invite(player->guid()));
	smsg.set_duanwei(player->ds_duanwei());
	
	ResMessage::res_player_check(player, smsg, name, id);
}

void PlayerManager::terminal_libao_exchange(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_libao_exchange msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::acc_t *acc = new dhc::acc_t;
	acc->set_username(msg.username());
	acc->set_serverid(msg.serverid());
	acc->set_guid(0);

	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_acc, 0), acc);
	game::pool()->upcall(req, boost::bind(&PlayerManager::libao_exchange_acc_callback, this, _1, data, msg.code(), name, id));
}

void PlayerManager::libao_exchange_acc_callback(Request *req, const std::string &data, const std::string &code, const std::string &name, int id)
{
	if (!req->success())
	{
		GLOBAL_ERROR;
		return;
	}
	dhc::acc_t *acc = (dhc::acc_t *)req->data();
	uint64_t player_guid = acc->guid();

	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		protocol::self::self_player_load msg;
		msg.set_data(data);
		msg.set_name(name);
		msg.set_id(id);
		std::string s;
		msg.SerializeToString(&s);
		sPlayerLoad->load_player(player_guid, SELF_PLAYER_LOAD_LIBAO_EXCHANGE, s);
		return;
	}
	rpcproto::tmsg_req_libao msg1;
	msg1.set_code(code);
	msg1.set_pt("");
	msg1.set_use("0");
	msg1.set_username(player->guid());
	msg1.set_serverid(player->serverid());
	std::string s;
	msg1.SerializeToString(&s);
	game::rpc_service()->request("libao1", PMSG_LIBAO, s, boost::bind(&PlayerManager::libao, this, _1, player_guid, code, name, id));
}

void PlayerManager::self_player_load_libao_exchange(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_libao_exchange(msg.data(), msg.name(), msg.id());
}

void PlayerManager::libao(const std::string &data, uint64_t player_guid, const std::string &code, const std::string &name, int id)
{
	rpcproto::tmsg_rep_libao msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.res() == -1)
	{
		PERROR(ERROR_LIBAO_NO);
		return;
	}
	else if (msg.res() == -2)
	{
		PERROR(ERROR_LIBAO_PT);
		return;
	}
	else if (msg.res() == -3)
	{
		PERROR(ERROR_LIBAO_USED);
		return;
	}
	
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	int pc = msg.pc();
	if (pc != 0)
	{
		for (int i = 0; i < player->libao_nums_size(); ++i)
		{
			if (player->libao_nums(i) == pc)
			{
				PERROR(ERROR_LIBAO_NUM);
				return;
			}
		}
	}

	rpcproto::tmsg_req_libao msg1;
	msg1.set_code(code);
	msg1.set_pt("");
	msg1.set_use("1");
	msg1.set_username(player->guid());
	msg1.set_serverid(player->serverid());
	std::string s;
	msg1.SerializeToString(&s);
	game::rpc_service()->request("libao1", PMSG_LIBAO, s, boost::bind(&PlayerManager::libao1, this, _1, player_guid, code, name, id));
}

void PlayerManager::libao1(const std::string &data, uint64_t player_guid, const std::string &code, const std::string &name, int id)
{
	rpcproto::tmsg_rep_libao msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.res() == -1)
	{
		PERROR(ERROR_LIBAO_NO);
		return;
	}
	else if (msg.res() == -2)
	{
		PERROR(ERROR_LIBAO_PT);
		return;
	}
	else if (msg.res() == -3)
	{
		PERROR(ERROR_LIBAO_USED);
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	int pc = msg.pc();
	if (pc != 0)
	{
		for (int i = 0; i < player->libao_nums_size(); ++i)
		{
			if (player->libao_nums(i) == pc)
			{
				PERROR(ERROR_LIBAO_NUM);
				return;
			}
		}
	}

	std::vector<s_t_reward> rds;
	int is_chongzhi = 0;
	if (msg.chongzi() > 0)
	{
		const s_t_taobao_recharge *t_taobao_recharge = sPlayerConfig->get_taobao_recharge(msg.chongzi());
		if (!t_taobao_recharge)
		{
			PERROR(ERROR_LIBAO_NO);
			return;
		}

		const s_t_recharge *t_recharge = sPlayerConfig->get_recharge(t_taobao_recharge->subid);
		if (!t_recharge)
		{
			PERROR(ERROR_LIBAO_NO);
			return;
		}

		for (int km = 0; km < t_taobao_recharge->num; ++km)
		{
			player->add_rdz_ids(t_taobao_recharge->subid);
			player->add_rdz_counts(t_recharge->vippt);
		}
		is_chongzhi = 1;
		
		dhc::recharge_heitao_t *rh = new dhc::recharge_heitao_t;
		rh->set_orderno("libao" + code);
		rh->set_rid(msg.chongzi());
		rh->set_player_guid(player_guid);

		Request *req1 = new Request();
		req1->add(opc_insert, MAKE_GUID(et_reharge_heitao, 0), rh);
		game::pool()->upcall(req1, 0);
	}
	else
	{
		for (int i = 0; i < msg.types_size(); ++i)
		{
			rds.push_back(s_t_reward(msg.types(i), msg.value1(i), msg.value2(i), msg.value3(i)));
		}
		int lang_ver = game::channel()->get_channel_lang(player_guid);
		std::string sender;
		std::string title;
		std::string text;
		game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
		game::scheme()->get_server_str(lang_ver, title, "sys_sender");
		game::scheme()->get_server_str(lang_ver, text, "sys_sender");
		PostOperation::post_create(player_guid, sender, title, text, rds);
	}

	if (pc != 0)
	{
		player->add_libao_nums(pc);
	}

	ResMessage::res_success_ex(true, name, id);
}

void PlayerManager::terminal_gonggao(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_gonggao msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.serverid() == game::env()->get_game_value("qid"))
	{
		SocialOperation::gundong_ex(msg.gonggao());
		
	}
	GLOBAL_ERROR;
}

void PlayerManager::terminal_player_template(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_player_template msg;
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

	s_t_role *t_role = sRoleConfig->get_role(msg.template_id());
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_role->font_color < 3)
	{
		GLOBAL_ERROR;
		return;
	}
	if (!RoleOperation::has_t_role(player, msg.template_id()))
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_template_id(msg.template_id());
	ResMessage::res_success(player, true, name, id);
}

void PlayerManager::terminal_kick(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_kick msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	TermInfo *ti = game::channel()->get_channel(player_guid);
	if (player && ti)
	{
		uint64_t guild_guid = player->guild();
		sPlayerPool->save_player(player->guid(), true);
		game::channel()->del_channel(player_guid);
	}
	RankOperation::del_player(player_guid);

	ResMessage::res_success_ex(true, name, id);
}

void PlayerManager::terminal_chenghao_on(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_chenghao_on msg;
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

	const s_t_chenghao *t_chenghao = sPlayerConfig->get_chenghao(msg.id());
	if (!t_chenghao)
	{
		GLOBAL_ERROR;
		return;
	}

	bool has = false;
	for (int i = 0; i < player->chenghao_size(); ++i)
	{
		if (player->chenghao(i) == msg.id())
		{
			has = true;
			break;
		}
	}

	if (!has)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.id() == player->chenghao_on())
	{
		player->set_chenghao_on(0);
	}
	else
	{
		player->set_chenghao_on(msg.id());
	}

	ResMessage::res_success(player, true, name, id);
}

void PlayerManager::terminal_server_stat(const std::string &data, const std::string &name, int id)
{
	game::rpc_service()->response(name, id, "", 0, "");
}

void PlayerManager::gm_create_player(dhc::player_t *player, const std::string &serverid)
{
	std::string s = "";
	for (int i = 0; i < 32; ++i)
	{
		int a = Utils::get_int32(0, 35);
		if (a < 10)
		{
			s += char('0' + a);
		}
		else
		{
			s += char('a' - 10 + a);
		}
	}

	dhc::acc_t *acc = new dhc::acc_t;
	acc->set_username(s);
	acc->set_serverid(serverid);
	acc->set_device("");
	acc->set_version(0);
	acc->set_last_device("");
	acc->set_last_time(0);
	uint64_t sid = boost::lexical_cast<uint64_t>(serverid);
	acc->set_guid(MAKE_GUID_EX(et_player, sid, 0));

	Request *req = new Request();
	req->add(opc_query, MAKE_GUID(et_acc, 0), acc);
	game::pool()->upcall(req, boost::bind(&PlayerManager::gm_create_player_callback, this, _1, s, player->guild()));
}

int PlayerManager::gm_create_player_callback(Request *req, const std::string &name, uint64_t guild)
{
	if (!req->success())
	{
		return 0;
	}

	dhc::acc_t *acc = (dhc::acc_t*)req->data();
	dhc::player_t *player = sPlayerLoad->create_player(acc->guid(), acc->serverid());
	player->set_name(name);
	player->set_level(Utils::get_int32(50, 100));
	player->set_bf(Utils::get_int32(100000, 500000));
	player->set_vip(Utils::get_int32(1, 12));
	std::vector<uint32_t> roles;
	player->set_template_id(sRoleConfig->get_random_role_id(Utils::get_int32(3, 4), roles));
	if (guild)
	{
		dhc::guild_t *guildd = POOL_GET_GUILD(guild);
		if (!guildd)
		{
			return 0;
		}

		dhc::guild_member_t *guild_member = new dhc::guild_member_t;
		uint64_t guild_member_guid = game::gtool()->assign(et_guild_member);
		guild_member->set_guid(guild_member_guid);
		guild_member->set_guild_guid(guildd->guid());
		guild_member->set_player_guid(player->guid());
		guild_member->set_player_iocn_id(player->template_id());
		guild_member->set_player_level(player->level());
		guild_member->set_player_name(player->name());
		guild_member->set_bf(player->bf());
		guild_member->set_zhiwu(2);
		guild_member->set_join_time(game::timer()->now() - 86400000);
		guild_member->set_last_sign_time(game::timer()->now() - 86400000);
		guild_member->set_last_mbtime(game::timer()->now());
		guild_member->set_player_vip(player->vip());
		guild_member->set_player_achieve(player->dress_achieves_size());
		guild_member->set_map_star(0);
		guild_member->set_offline_time(game::timer()->now());

		player->set_guild(guildd->guid());
		player->set_last_leave_guild_time(game::timer()->now() - 86400000);

		POOL_ADD_NEW(guild_member_guid, guild_member);

		guildd->add_member_guids(guild_member_guid);
	}

	return 0;
}
