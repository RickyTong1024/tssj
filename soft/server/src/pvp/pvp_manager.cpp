#include "pvp_manager.h"
#include "pvp_pool.h"
#include "rpc.pb.h"
#include "mission_fight.h"
#include "equip_config.h"
#include "guild_config.h"
#include "item_config.h"
#include "mission_config.h"
#include "player_config.h"
#include "role_config.h"
#include "sport_config.h"
#include "treasure_config.h"
#include "pvp_config.h"

PvpManager::PvpManager()
:pvp_pool_(0),
timer_id_(0)
{

}

PvpManager::~PvpManager()
{

}

int PvpManager::init()
{
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
	pvp_pool_ = new PvpPool();
	pvp_pool_->init();

	timer_id_ = game::timer()->schedule(boost::bind(&PvpManager::update, this, _1), 5000, "pvp");
	return 0;
}

int PvpManager::fini()
{
	if (timer_id_)
	{
		game::timer()->cancel(timer_id_);
		timer_id_ = 0;
	}

	if (pvp_pool_)
	{
		pvp_pool_->fini();
		delete pvp_pool_;
	}
	return 0;
}

int PvpManager::update(const ACE_Time_Value& rhs)
{
	pvp_pool_->update();
	return 0;
}


void PvpManager::terminal_player_look(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_player_look msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	uint64_t target_guid = msg.target_guid();

	dhc::player_t *target = POOL_GET_PLAYER(target_guid);
	if (!target)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_player_look smsg;
	smsg.set_template_id(target->template_id());
	smsg.set_level(target->level());
	smsg.set_name(target->name());
	smsg.set_bf(target->bf());
	smsg.set_vip(target->vip());
	smsg.set_achieves(target->dress_achieves_size());
	smsg.set_guild(target->zsname());
	smsg.set_guid(target->guid());
	smsg.set_serverid(target->serverid());
	smsg.set_nalflag(target->nalflag());
	for (int i = 0; i < target->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(target->zhenxing(i));
		if (!role)
		{
			smsg.add_roles()->set_guid(0);
		}
		else
		{
			smsg.add_roles()->CopyFrom(*role);
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (!equip)
				{
					smsg.add_equips()->set_guid(0);
				}
				else
				{
					smsg.add_equips()->CopyFrom(*equip);
				}
			}
			for (int j = 0; j < role->treasures_size(); ++j)
			{
				dhc::treasure_t *treasure = POOL_GET_TREASURE(role->treasures(j));
				if (!treasure)
				{
					smsg.add_treasures()->set_guid(0);
				}
				else
				{
					smsg.add_treasures()->CopyFrom(*treasure);
				}
			}
		}
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_player_rank(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_rank_view msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	int type = msg.type();
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, type));
	if (!rank)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_rank_view smsg;
	smsg.mutable_rank_list()->CopyFrom(*rank);
	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_player_push(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_player_fight_info msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	pvp_pool_->add_player(msg.players());
	game::rpc_service()->response(name, id, "");
}

void PvpManager::terminal_player_pull(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_pvp_match msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_pvp_match smsg;
	pvp_pool_->get_player(&smsg, msg.guid(), msg.bf());

	int left = 3 - smsg.player_guids_size();
	for (int i = 0; i < left; ++i)
	{
		dhc::player_t *npc = POOL_GET_PLAYER(MAKE_GUID(et_player, i));
		if (npc)
		{
			smsg.add_player_guids(npc->guid());
			smsg.add_player_names(npc->name());
			smsg.add_player_templates(npc->template_id());
			smsg.add_player_bfs(npc->bf());
			smsg.add_player_points(npc->pvp_total());
			smsg.add_player_vips(npc->vip());
			smsg.add_player_achieves(npc->dress_achieves_size());
			smsg.add_player_guanghuans(0);
			smsg.add_player_dress(0);
			smsg.add_player_servers(boost::lexical_cast<int>(msg.serverid()));
			smsg.add_player_nalflag(npc->nalflag());
		}
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_player_fight(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_pvp_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t* target = POOL_GET_PLAYER(msg.guid());
	if (!target)
	{
		GLOBAL_ERROR;
		return;
	}

	pvp_pool_->add_player(msg.player());

	dhc::player_t* player = POOL_GET_PLAYER(msg.player().guid());
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	std::string text;
	int res = MissionFight::mission_sport(player, target, text);

	rpcproto::tmsg_rep_pvp_fight smsg;
	smsg.set_text(text);
	smsg.set_result(res);

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_player_reward(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_pvp_reward msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_point_last));
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
	if (rank->player_guid_size() <= 0)
	{
		smsg.mutable_ranks();
	}
	for (int i = 0; i < rank->player_guid_size(); ++i)
	{
		if (i >= 100)
		{
			break;
		}
		smsg.add_guids(rank->player_guid(i));

		if (i < 3)
		{
			rpcproto::tmsg_rep_pvp_match *emtpy_rank = smsg.mutable_ranks();
			if (emtpy_rank)
			{
				emtpy_rank->add_player_guids(rank->player_guid(i));
				emtpy_rank->add_player_names(rank->player_name(i));
				emtpy_rank->add_player_templates(rank->player_template(i));
				emtpy_rank->add_player_servers(rank->player_level(i));
				emtpy_rank->add_player_bfs(rank->player_bf(i));
				emtpy_rank->add_player_points(rank->value(i));
				emtpy_rank->add_player_vips(rank->player_vip(i));
				emtpy_rank->add_player_achieves(rank->player_achieve(i));
				emtpy_rank->add_player_guanghuans(0);
				emtpy_rank->add_player_dress(0);
				emtpy_rank->add_player_nalflag(rank->player_nalflag(i));
			}
		}
	}
	smsg.set_res(0);

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_player_mofang(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_mofang_point msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::global_t *glob = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!glob)
	{
		GLOBAL_ERROR;
		return;
	}

	int index = -1;
	for (int i = 0; i < glob->huodong_end_times_size(); ++i)
	{
		if (glob->huodong_end_times(i) == msg.huodong_time())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		if (glob->huodong_end_times_size() >= 20)
		{
			glob->mutable_huodong_end_times()->SwapElements(0, glob->huodong_end_times_size() - 1);
			glob->mutable_huodong_end_times()->RemoveLast();

			glob->mutable_huodong_ids()->SwapElements(0, glob->huodong_ids_size() - 1);
			glob->mutable_huodong_ids()->RemoveLast();
		}
		glob->add_huodong_end_times(msg.huodong_time());
		glob->add_huodong_ids(0);
		index = glob->huodong_end_times_size() - 1;
	}

	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.point() > 0)
	{
		glob->set_huodong_ids(index, glob->huodong_ids(index) + msg.point());
	}

	rpcproto::tmsg_rep_mofang_point smsg;
	smsg.set_point(glob->huodong_ids(index));
	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::self_player_load_look(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_player_look(msg.data(), msg.name(), msg.id());
}


void PvpManager::terminal_invite_code_gen(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_invite_code msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	bool res = pvp_pool_->create_social_code(msg);

	rpcproto::tmsg_rep_invite_code smsg;
	smsg.set_res(res);
	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_invite_code_input(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_invite_code_input msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	int res = pvp_pool_->input_social_code(msg.player_guid(), msg.player_level(), msg.code());

	rpcproto::tmsg_rep_invite_code smsg;
	smsg.set_res(res);
	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_invite_code_level(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_invite_code_input msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	pvp_pool_->update_social_code(msg.player_guid(), msg.player_level());
	game::rpc_service()->response(name, id, "");
}

void PvpManager::terminal_invite_code_pull(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_invite_code_input msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_invite_level smsg;
	smsg.set_res(false);
	bool res = pvp_pool_->get_social_code(msg.player_guid(), &smsg);
	smsg.set_res(res);
	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_look_bushu(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_guild_look msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_guild_look_bushu smsg;
	smsg.set_res(true);

	dhc::guild_arrange_t *arrange = POOL_GET_GUILD_ARRANGE(msg.player_guild());
	if (arrange)
	{
		for (int i = 0; i < arrange->player_guids_size(); ++i)
		{
			smsg.add_player_guids(arrange->player_guids(i));
			smsg.add_player_names(arrange->player_names(i));
			smsg.add_player_template(arrange->player_template(i));
			smsg.add_player_level(arrange->player_level(i));
			smsg.add_player_bat_eff(arrange->player_bat_eff(i));
			smsg.add_player_vips(arrange->player_vip(i));
			smsg.add_player_achieves(arrange->player_achieve(i));
		}
	}
	else
	{
		for (int i = 0; i < 22; ++i)
		{
			smsg.add_player_guids(0);
			smsg.add_player_names("");
			smsg.add_player_template(0);
			smsg.add_player_level(0);
			smsg.add_player_bat_eff(0);
			smsg.add_player_vips(0);
			smsg.add_player_achieves(0);
		}
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_look_pipei(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_guild_look msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_arrange_t *guild_arrage = POOL_GET_GUILD_ARRANGE(msg.player_guild());
	if (!guild_arrage)
	{
		GLOBAL_ERROR;
		return;
	}

	int jundian = 0;
	int jidian = 0;
	int perfect = 0;
	pvp_pool_->guild_fight_gongpo(guild_arrage, jundian, jidian, perfect);

	rpcproto::tmsg_rep_guild_fight_info tmsg;
	dhc::guild_fight_t *guild_fight = 0;
	rpcproto::tmsg_guild_fight_info *fight_info = 0;

	tmsg.set_res(true);
	tmsg.set_guild_zhanji(guild_arrage->guild_zhanji());
	tmsg.set_judian(jundian);
	tmsg.set_jidi(jidian);
	tmsg.set_perfect(perfect);

	for (int i = 0; i < guild_arrage->player_zguids_size(); ++i)
	{
		if (guild_arrage->player_zguids(i) == msg.player_guid())
		{
			tmsg.set_zhanji(guild_arrage->player_zhanjis(i));
			tmsg.set_total_zhanji(guild_arrage->player_total_zhanjis(i));
			break;
		}
	}
	
	for (int i = 0; i < guild_arrage->guild_fights_size(); ++i)
	{
		guild_fight = POOL_GET_GUILD_FIGHT(guild_arrage->guild_fights(i));
		if (guild_fight)
		{
			fight_info = tmsg.add_guild_fight();
			if (fight_info)
			{
				fight_info->set_guild(guild_fight->guild_guid());
				fight_info->set_guild_name(guild_fight->guild_name());
				fight_info->set_guild_server(guild_fight->guild_server());
				fight_info->set_guild_icon(guild_fight->guild_icon());
				fight_info->set_guild_level(guild_fight->guild_level());

				for (int j = 0; j < guild_fight->target_guids_size(); ++j)
				{
					fight_info->add_target_guids(guild_fight->target_guids(j));
					fight_info->add_target_names(guild_fight->target_names(j));
					fight_info->add_target_levels(guild_fight->target_levels(j));
					fight_info->add_target_templates(guild_fight->target_templates(j));
					fight_info->add_target_bat_effs(guild_fight->target_bat_effs(j));
					fight_info->add_target_vips(guild_fight->target_vips(j));
					fight_info->add_target_achieves(guild_fight->target_achieves(j));
					fight_info->add_target_defense_nums(guild_fight->target_defense_nums(j));
				}

				for (int j = 0; j < guild_fight->guard_points_size(); ++j)
				{
					fight_info->add_guard_points(guild_fight->guard_points(j));
					fight_info->add_guard_gongpo(guild_fight->guard_gongpuo(j));
				}
			}
		}
	}
	
	std::string s;
	tmsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_jinrizhanji(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_guild_look msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_arrange_t *guild_arrange = POOL_GET_GUILD_ARRANGE(msg.player_guild());
	if (!guild_arrange)
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_guild_jinrizhanji smsg;
	smsg.set_res(true);
	smsg.set_guild_zhanji(guild_arrange->guild_zhanji());
	smsg.set_guild_total_zhanji(guild_arrange->guild_total_zhanji());
	smsg.set_guild_exp(guild_arrange->guild_exp());

	for (int i = 0; i < guild_arrange->player_zguids_size(); ++i)
	{
		if (msg.player_guid() == guild_arrange->player_zguids(i))
		{
			smsg.set_player_zhanji(guild_arrange->player_zhanjis(i));
			smsg.set_player_total_zhanji(guild_arrange->player_total_zhanjis(i));
			break;
		}
	}
	
	for (int i = 0; i < guild_arrange->player_guids_size(); ++i)
	{
		smsg.mutable_bushu()->add_player_guids(guild_arrange->player_guids(i));
		smsg.mutable_bushu()->add_player_names(guild_arrange->player_names(i));
		smsg.mutable_bushu()->add_player_template(guild_arrange->player_template(i));
		smsg.mutable_bushu()->add_player_level(guild_arrange->player_level(i));
		smsg.mutable_bushu()->add_player_bat_eff(guild_arrange->player_bat_eff(i));
		smsg.mutable_bushu()->add_player_vips(guild_arrange->player_vip(i));
		smsg.mutable_bushu()->add_player_achieves(guild_arrange->player_achieve(i));
	}

	int jundian = 0;
	int jidian = 0;
	int perfect = 0;
	pvp_pool_->guild_fight_gongpo(guild_arrange, jundian, jidian, perfect);

	smsg.set_judian(jundian);
	smsg.set_jidi(jidian);
	smsg.set_perfect(perfect);

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_look_xiuzhan(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_guild_look msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_guild_jinrizhanji smsg;
	smsg.set_res(true);

	dhc::guild_arrange_t *guild_arrange = POOL_GET_GUILD_ARRANGE(msg.player_guild());
	if (!guild_arrange)
	{
		smsg.set_guild_zhanji(0);
		smsg.set_guild_total_zhanji(0);
		smsg.set_guild_exp(0);
		smsg.set_player_zhanji(0);
		smsg.set_player_total_zhanji(0);
	}
	else
	{
		smsg.set_guild_zhanji(guild_arrange->guild_zhanji());
		smsg.set_guild_total_zhanji(guild_arrange->guild_total_zhanji());
		smsg.set_guild_exp(guild_arrange->guild_exp());

		for (int i = 0; i < guild_arrange->player_zguids_size(); ++i)
		{
			if (msg.player_guid() == guild_arrange->player_zguids(i))
			{
				smsg.set_player_zhanji(guild_arrange->player_zhanjis(i));
				smsg.set_player_total_zhanji(guild_arrange->player_total_zhanjis(i));
				break;
			}
		}
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_baoming(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_guild_pvp_baoming msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_guild_pvp_baoming smsg;
	smsg.set_res(true);
	dhc::guild_arrange_t *guild_arrange = POOL_GET_GUILD_ARRANGE(msg.guild());
	if (!guild_arrange)
	{
		guild_arrange = new dhc::guild_arrange_t();
		guild_arrange->set_guid(msg.guild());
		guild_arrange->set_guild_server(msg.guild_server());
		guild_arrange->set_guild_name(msg.guild_name());
		guild_arrange->set_guild_icon(msg.guild_icon());
		guild_arrange->set_guild_level(msg.guild_level());
		for (int i = 0; i < 22; ++i)
		{
			guild_arrange->add_player_guids(0);
			guild_arrange->add_player_names("");
			guild_arrange->add_player_level(0);
			guild_arrange->add_player_template(0);
			guild_arrange->add_player_bat_eff(0);
			guild_arrange->add_player_vip(0);
			guild_arrange->add_player_achieve(0);
			guild_arrange->add_player_map_star(0);

		}
		POOL_ADD_NEW(guild_arrange->guid(), guild_arrange);
		POOL_SAVE(dhc::guild_arrange_t, guild_arrange, false);

		dhc::player_t *player = 0;
		for (int i = 0; i < msg.player_guids_size(); ++i)
		{
			player = POOL_GET_PLAYER(msg.player_guids(i));
			if (!player)
			{
				smsg.add_player_guids(msg.player_guids(i));
			}
		}
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_bushu(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_guild_pvp_bushu msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_arrange_t *guild_arrange = POOL_GET_GUILD_ARRANGE(msg.guild());
	if (!guild_arrange)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.player_guids_size() != guild_arrange->player_guids_size())
	{
		GLOBAL_ERROR;
		return;
	}

	rpcproto::tmsg_rep_guild_pvp_bushu smsg;
	smsg.set_res(true);
	guild_arrange->set_guild_name(msg.guild_name());
	guild_arrange->set_guild_icon(msg.guild_icon());
	guild_arrange->set_guild_level(msg.guild_level());

	dhc::player_t *player = 0;
	for (int i = 0; i < msg.player_guids_size(); ++i)
	{
		player = POOL_GET_PLAYER(msg.player_guids(i));
		if (!player)
		{
			smsg.add_player_guids(msg.player_guids(i));
		}

		guild_arrange->set_player_guids(i, msg.player_guids(i));
		guild_arrange->set_player_names(i, msg.player_names(i));
		guild_arrange->set_player_level(i, msg.player_level(i));
		guild_arrange->set_player_template(i, msg.player_template(i));
		guild_arrange->set_player_bat_eff(i, msg.player_bat_eff(i));
		guild_arrange->set_player_vip(i, msg.player_vips(i));
		guild_arrange->set_player_achieve(i, msg.player_achieves(i));
		guild_arrange->set_player_map_star(i, msg.player_map_star(i));
	}
	pvp_pool_->save_guild(guild_arrange);

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_fight(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_guild_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	pvp_pool_->add_player(msg.player());

	dhc::player_t *player = POOL_GET_PLAYER(msg.player_guid());
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	
	rpcproto::tmsg_rep_guild_fight smsg;
	smsg.set_result(-1);
	smsg.set_text("");
	smsg.set_res(-1);

	pvp_pool_->guild_fight(msg.player_guild(),
		msg.player_guid(), 
		msg.target_guild(), 
		msg.target_index(),
		smsg);

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_zhankuang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_look_zhankuang msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_arrange_t *arrange = POOL_GET_GUILD_ARRANGE(msg.guild());
	if (!arrange)
	{
		GLOBAL_ERROR;
		return;
	}

	std::vector<const s_t_guildfight *> t_guildfights;
	const s_t_guildfight *t_guildfight = 0;
	for (int i = 0; i < 4; ++i)
	{
		t_guildfight = sGuildConfig->get_guildfight(i);
		if (!t_guildfight)
		{
			GLOBAL_ERROR;
			return;
		}
		t_guildfights.push_back(t_guildfight);
	}

	protocol::game::smsg_guild_look_zhankuang smsg;
	if (msg.type() == 1)
	{
		dhc::guild_fight_t *guild_fight = 0;
		const s_t_guildfight *t_guildfight = 0;
		int point = 0;

		for (int i = 0; i < arrange->guild_fights_size(); ++i)
		{
			guild_fight = POOL_GET_GUILD_FIGHT(arrange->guild_fights(i));
			if (guild_fight)
			{
				smsg.add_guild_name(guild_fight->guild_name());
				smsg.add_guild_icon(guild_fight->guild_icon());
				smsg.add_guild_level(guild_fight->guild_level());
				
				int defense_num[4] = { 0, 0, 0, 0 };
				for (int j = 0; j < guild_fight->target_defense_nums_size(); ++j)
				{
					t_guildfight = t_guildfights[j / 7];
					if (guild_fight->target_defense_nums(j) < t_guildfight->defense_num)
					{
						defense_num[j / 7] += 1;
					}
				}
				for (int j = 0; j < 4; ++j)
				{
					point += guild_fight->win_nums(j) * t_guildfights[j]->win_point + guild_fight->lose_nums(j) * t_guildfights[j]->lose_point;
					smsg.add_guild_player_nums(defense_num[j]);
					smsg.add_guild_val_nums(t_guildfights[j]->chengfang_point - guild_fight->guard_points(j));
				}
				smsg.set_guild_zhanji(point);
			}
		}
	}
	else
	{
		dhc::guild_fight_t *guild_fight = 0;
		dhc::guild_arrange_t *target_guild = 0;
		dhc::guild_fight_t *target_guild_fight = 0;
		int point = 0;

		for (int i = 0; i < arrange->guild_fights_size(); ++i)
		{
			guild_fight = POOL_GET_GUILD_FIGHT(arrange->guild_fights(i));
			if (guild_fight)
			{
				target_guild = POOL_GET_GUILD_ARRANGE(guild_fight->target_guild_guid());
				if (target_guild)
				{
					for (int j = 0; j < target_guild->guild_fights_size(); ++j)
					{
						target_guild_fight = POOL_GET_GUILD_FIGHT(target_guild->guild_fights(j));
						if (target_guild_fight && target_guild_fight->target_guild_guid() == arrange->guid())
						{
							smsg.add_guild_name(guild_fight->guild_name());
							smsg.add_guild_icon(guild_fight->guild_icon());
							smsg.add_guild_level(guild_fight->guild_level());

							int defense_num[4] = { 0, 0, 0, 0 };
							int defense_player[4] = { 0, 0, 0, 0 };
							for (int k = 0; k < target_guild_fight->target_defense_nums_size(); ++k)
							{
								t_guildfight = t_guildfights[k / 7];
								point += t_guildfight->win_point * (t_guildfight->defense_num - target_guild_fight->target_defense_nums(k));
								if (target_guild_fight->target_defense_nums(k) < t_guildfight->defense_num)
								{
									defense_num[k / 7] += 1;
									defense_player[k / 7] += t_guildfight->defense_num - target_guild_fight->target_defense_nums(k);
								}
							}
							for (int k = 0; k < 4; ++k)
							{
								smsg.add_guild_player_nums(defense_num[k]);
								smsg.add_guild_val_nums(defense_player[k]);
							}
							break;
						}
					}
				}
			}
		}
		smsg.set_guild_zhanji(point);
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_zhanji(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_look_zhanji msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_arrange_t *arrange = POOL_GET_GUILD_ARRANGE(msg.guild());
	if (!arrange)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_guild_look_zhanji smsg;

	for (int i = 0; i < arrange->player_zguids_size(); ++i)
	{
		smsg.add_player_guids(arrange->player_zguids(i));
		smsg.add_player_names(arrange->player_znames(i));
		smsg.add_player_template(arrange->player_ztemplate(i));
		smsg.add_player_level(arrange->player_zlevel(i));
		smsg.add_player_bat_eff(arrange->player_zbat_eff(i));
		smsg.add_player_vip(arrange->player_zvip(i));
		smsg.add_player_achieve(arrange->player_zachieve(i));
		smsg.add_player_zhanjis(arrange->player_zhanjis(i));
		smsg.add_player_total_zhanjis(arrange->player_total_zhanjis(i));
		smsg.add_player_nalflags(arrange->player_znalflags(i));
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}


void PvpManager::terminal_guild_pvp_match(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_push_guild_match msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	pvp_pool_->guild_pvp_gm(msg.week(), msg.hour());
	
	game::rpc_service()->response(name, id, "");
}

void PvpManager::terminal_guild_pvp_reward(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_pvp_reward msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::global_t *glob = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!glob)
	{
		GLOBAL_ERROR;
		return;
	}
	

	rpcproto::tmsg_rep_guild_fight_reward smsg;
	smsg.set_stype(-1);
	smsg.set_zhou(0);

	if (msg.serverid() == "sync")
	{
		smsg.set_stype(0);
		smsg.set_zhou(glob->guild_pvp_zhou());
	}
	else
	{
		dhc::rank_t* rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_guild_zhanji_last));
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

		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			smsg.add_guild_rewards(rank->player_guid(i));
		}

		rank = POOL_GET_RANK(MAKE_GUID(et_rank, erank_guild_player_zhanji_last));
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
		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			smsg.add_player_rewards(rank->player_guid(i));
		}

		smsg.set_stype(1);
		smsg.set_zhou(glob->guild_pvp_zhou());
	}

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void PvpManager::terminal_guild_pvp_target(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_req_guild_target_reward msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_arrange_t *guild_arrange = POOL_GET_GUILD_ARRANGE(msg.pvp_guid());
	if (!guild_arrange)
	{
		GLOBAL_ERROR;
		return;
	}

	int jundian = 0;
	int jidian = 0;
	int perfect = 0;
	pvp_pool_->guild_fight_gongpo(guild_arrange, jundian, jidian, perfect);

	rpcproto::tmsg_rep_guild_target_reward smsg;
	smsg.set_res(true);
	smsg.set_judian(jundian);
	smsg.set_jidi(jidian);
	smsg.set_perfect(perfect);

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}