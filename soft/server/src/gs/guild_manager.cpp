#include "guild_manager.h"
#include "guild_pool.h"
#include "guild_operation.h"
#include "guild_config.h"
#include "gs_message.h"
#include "item_operation.h"
#include "utils.h"
#include "player_operation.h"
#include "player_config.h"
#include "mission_fight.h"
#include "post_operation.h"
#include "social_operation.h"
#include "player_load.h"
#include "huodong_pool.h"
#include "item_config.h"
#include "pvp_operation.h"
#include "rpc.pb.h"

GuildManager::GuildManager()
: timer_(-1)
{
	
}

GuildManager::~GuildManager()
{
	
}

int GuildManager::init()
{
	if (-1 == sGuildConfig->parse())
	{
		return -1;
	}
	sGuildPool->init();
	timer_ = game::timer()->schedule(boost::bind(&GuildPool::update, sGuildPool, _1), GUILD_PERIOD, "guild");
	return 0;
}

int GuildManager::fini()
{
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = -1;
	}
	sGuildPool->fini();
	return 0;
}

void GuildManager::terminal_guild_create(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_create msg;
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

	uint64_t last_time = player->last_leave_guild_time();
	if (game::timer()->now() - last_time < JOIN_GUILD_TIME_LIMITED)
	{
		PERROR(ERROR_JOIN_GUILD_TIME_LIMITED);
		return;
	}

	if (player->guild() > 0)
	{
		PERROR(ERROR_GUILD_BELONG);
		return;
	}

	if (player->jewel() < CREATE_GUILD_CONSUME)		
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	if (player->level() < GUILD_ADD_LEVEL)
	{
		GLOBAL_ERROR;
		return;
	}

	operror_t err = GuildOperation::check_guild_name(msg.guild_name());	
	if (err > operror_t(0))
	{
		PERROR(err);
		return;
	}

	dhc::guild_list_t *guild_list = POOL_GET_GUILD_LIST(MAKE_GUID(et_guild_list, 0));
	if (!guild_list)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t *guild = new dhc::guild_t();
	uint64_t guild_guid = game::gtool()->assign(et_guild);
	guild->set_guid(guild_guid);
	guild->set_name(msg.guild_name());
	guild->set_icon(DEFAULT_GUILD_ICON_ID);
	guild->set_level(1);
	guild->set_last_level(1);
	guild->set_leader_name(player->name());
	guild->set_gonggao("");
	
	GuildOperation::create_guild_member(guild, player, e_member_type_leader);
	GuildOperation::create_guild_mission(guild);
	GuildOperation::remove_player_all_apply(player);
	ItemOperation::refresh_guild_shop(guild);
	ItemOperation::refresh_guild_shop(player);

	POOL_ADD_NEW(guild_guid, guild);
	sGuildPool->add_guild(guild);
	guild_list->add_guild_guids(guild_guid);
	sGuildPool->save_list(false);

	PlayerOperation::player_dec_resource(player, resource::JEWEL, CREATE_GUILD_CONSUME, LOGWAY_GUIILD_CREATE);
	SocialOperation::gundong_server("t_server_language_guild_create_danmu", player->name(), msg.guild_name(), "");
	ResMessage::res_guild_data(player, guild, e_member_type_leader, true, true, 0, false, false, false, name, id);	
}

void GuildManager::terminal_guild_apply(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_apply msg;
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

	if (player->guild() > 0)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->guild_ltime() > game::timer()->now())
	{
		PERROR(ERROR_GUILD_EXIT_SEVEN);
		return;
	}

	uint64_t last_time = player->last_leave_guild_time();
	if (game::timer()->now() - last_time < JOIN_GUILD_TIME_LIMITED)
	{
		PERROR(ERROR_JOIN_GUILD_TIME_LIMITED);
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(msg.guild_guid());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild *t_guild = sGuildConfig->get_guild(guild->level());
	if (!t_guild)
	{
		GLOBAL_ERROR;
		return;
	}
	if (guild->member_guids_size() >= t_guild->member_num)
	{
		GLOBAL_ERROR;
		return;
	}

	if (GuildOperation::get_guild_apply_index(guild, player->guid()) != -1)
	{
		GLOBAL_ERROR;
		return;
	}

	player->add_guild_applys(guild->guid());
	if (player->guild_applys_size() > 3)
	{
		dhc::guild_t* apply_guild = POOL_GET_GUILD(player->guild_applys(0));
		if (apply_guild)
		{
			GuildOperation::remove_player_apply(player, apply_guild);
		}
	}
	guild->add_apply_guids(player->guid());
	guild->add_apply_names(player->name());
	guild->add_apply_template(player->template_id());
	guild->add_apply_level(player->level());
	guild->add_apply_bf(player->bf());
	guild->add_apply_vip(player->vip());
	guild->add_apply_achieve(player->dress_achieves_size());
	guild->add_apply_nalfags(player->nalflag());

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_agree(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_agree msg;
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

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() == e_member_type_common)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild *t_guild = sGuildConfig->get_guild(guild->level());
	if (!t_guild)
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.argree() && guild->member_guids_size() >= t_guild->member_num)
	{
		GLOBAL_ERROR;
		return;
	}

	int apply_index = GuildOperation::get_guild_apply_index(guild, msg.player_guid());
	if (apply_index == -1)
	{
		ResMessage::res_success(player, true, name, id);
		return;
	}

	dhc::player_t* member_player = POOL_GET_PLAYER(msg.player_guid());
	if (!member_player)
	{
		protocol::self::self_player_load msg1;
		msg1.set_data(data);
		msg1.set_name(name);
		msg1.set_id(id);
		std::string s;
		msg1.SerializeToString(&s);
		sPlayerLoad->load_player(msg.player_guid(), SLEF_PLAYER_LOAD_GUILD_APPLY, s);
		return;
	}
	else
	{
		game::channel()->refresh_offline_time(msg.player_guid());
	}

	if (msg.argree())
	{
		if (member_player->guild() > 0)
		{
			GLOBAL_ERROR;
			return;
		}
		GuildOperation::create_guild_member(guild, member_player, e_member_type_common);
		GuildOperation::create_guild_event(guild, member_player->name(), e_guild_op_type_join);
		GuildOperation::remove_player_all_apply(member_player);
	}
	else
	{
		GuildOperation::remove_player_apply(member_player, guild);
	}

	sGuildPool->save_guild(player->guild(), false);
	

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_open(const std::string &data, const std::string &name, int id)
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

	uint64_t guild_guid = player->guild();
	if (guild_guid > 0)	
	{
		dhc::guild_t *guild = POOL_GET_GUILD(guild_guid);
		if (!guild)
		{
			GLOBAL_ERROR;
			return;
		}
		dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
		if (!guild_member)
		{
			GLOBAL_ERROR;
			return;
		}

		dhc::guild_mission_t *guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
		if (!guild_mission)
		{
			GLOBAL_ERROR;
			return;
		}

		bool can_mobai = GuildOperation::has_mobai_point(player);
		bool can_fight = GuildOperation::has_fight_point(guild, player);
		bool can_apply = GuildOperation::has_apply_point(guild, guild_member);
		if (!can_fight)
		{
			can_fight = GuildOperation::has_reward_point(player, guild_mission);
		}
		bool has_hongbao = GuildOperation::has_guild_red_point(guild, player);
		bool can_guildpvp = GuildOperation::has_guildpvp_point(guild, player, guild_member);
		ResMessage::res_guild_data(player, guild, guild_member->zhiwu(), can_mobai, can_fight, guild_member->msg_count(), can_apply, has_hongbao, can_guildpvp, name, id);
		guild_member->set_msg_count(0);
	}
	else
	{
		terminal_guild_list_recommend(data, name, id);
	}
}


void GuildManager::terminal_guild_list_recommend(const std::string &data, const std::string &name, int id)
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

	int power = player->bf();

	std::vector<uint64_t> vec_guid;
	std::vector<uint64_t> vec_sguid;
	game::pool()->get_entitys(et_guild, vec_guid);	
	for (std::vector<uint64_t>::iterator iter = vec_guid.begin(); iter != vec_guid.end(); ++iter)
	{
		dhc::guild_t *guild = POOL_GET_GUILD(*iter);
		if (!guild)
		{
			continue;
		}

		s_t_guild *t_guild_exp = sGuildConfig->get_guild(guild->level());
		if (!t_guild_exp)
		{
			continue;
		}

		if (guild->member_guids_size() < t_guild_exp->member_num && guild->bftj() < power)	
		{
			vec_sguid.push_back(guild->guid());
		}
	}
	std::vector<uint64_t> vec_gguid;
	if (vec_sguid.size() > RECOMMEND_GUILD_COUNT_MAX)
	{
		Utils::get_vector(vec_sguid, RECOMMEND_GUILD_COUNT_MAX, vec_gguid);
	}
	else
	{
		vec_gguid = vec_sguid;
	}
	std::vector<dhc::guild_t *> vec_guild;
	for (int i = 0; i < vec_gguid.size(); ++i)
	{
		dhc::guild_t *guild = POOL_GET_GUILD(vec_gguid[i]);
		if (!guild)
		{
			continue;
		}
		vec_guild.push_back(guild);
	}

	ResMessage::res_guild_list_recommend(player, vec_guild, name, id);
}


void GuildManager::terminal_guild_query(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_query msg;
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


	std::vector<dhc::guild_t*> vec_guild;	
	std::vector<uint64_t> vec_guid;
	game::pool()->get_entitys(et_guild, vec_guid);	
	for (int i = 0; i < vec_guid.size(); ++i)
	{
		dhc::guild_t *guild = POOL_GET_GUILD(vec_guid[i]);
		if (!guild)
		{
			continue;
		}
		if ((guild->name().find(msg.guild_name())) != std::string::npos)
		{
			vec_guild.push_back(guild);
		}
	}

	ResMessage::res_guild_list_recommend(player, vec_guild, name, id);
}


void GuildManager::terminal_guild_modify_bulletin(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_modify_bulletin msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	operror_t err = GuildOperation::check_bulletin(msg.bulletin());	
	if (err > operror_t(0))
	{
		PERROR(err);
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}
	if (guild_member->zhiwu() == e_member_type_common)	
	{
		PERROR(ERROR_GUILD_AUTHORITY_REFUSE);
		return;
	}

	guild->set_gonggao(msg.bulletin().c_str());		

	ResMessage::res_success(player, true, name, id);
}


void GuildManager::terminal_guild_modify_name(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_modify_bulletin msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	operror_t err = GuildOperation::check_guild_name(msg.bulletin());
	if (err > operror_t(0))
	{
		PERROR(err);
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() == e_member_type_common)
	{
		PERROR(ERROR_GUILD_AUTHORITY_REFUSE);
		return;
	}

	const s_t_price* t_price = sPlayerConfig->get_price(guild->change_name() + 1);
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
	std::string src_name = guild->name();
	guild->set_name(msg.bulletin());
	guild->set_change_name(guild->change_name() + 1);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_price->change_name, LOGWAY_GUILD_CHANGE_NAME);
	sGuildPool->update_guild_mission_compare(guild, 0);

	std::string sender;
	std::string title;
	std::string text;
	std::vector<s_t_reward> rds;
	int lang_ver = game::channel()->get_channel_lang(player_guid);

	game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
	game::scheme()->get_server_str(lang_ver, title, "change_name_guildd_title");
	
	if (guild_member->zhiwu() == e_member_type_leader)
		game::scheme()->get_server_str(lang_ver, text, "change_name_guildd_title_text1", src_name.c_str(), player->name().c_str(), guild->name().c_str());
	else {
		game::scheme()->get_server_str(lang_ver, text, "change_name_guildd_title_text2", src_name.c_str(), player->name().c_str(), guild->name().c_str());
		}

	dhc::guild_member_t *bmember = 0;
	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		bmember = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (bmember && bmember->guid() != guild_member->guid())
		{
			PostOperation::post_create(guild_member->player_guid(), title, text, sender, rds);
		}
	}
	
	ResMessage::res_success(player, true, name, id);
}


void GuildManager::terminal_guild_modify_icon(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_modify_icon msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() == e_member_type_common)	
	{
		PERROR(ERROR_GUILD_AUTHORITY_REFUSE);
		return;
	}

	s_t_guild_icon *t_guild_icon = sGuildConfig->get_guild_icon(msg.icon());
	if (!t_guild_icon)
	{
		GLOBAL_ERROR;
		return;
	}

	guild->set_icon(msg.icon());
	sGuildPool->update_guild_mission_compare(guild, 0);

	ResMessage::res_success(player, true, name, id);
}

// 改变成员职位
void GuildManager::terminal_guild_change_member_duty(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_change_member_duty msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}
	if (guild_member->zhiwu() != e_member_type_leader)	
	{
		PERROR(ERROR_GUILD_AUTHORITY_REFUSE);
		return;
	}

	uint64_t member_guid = msg.member_guid();
	dhc::guild_member_t *guild_member1 = GuildOperation::get_guild_member(guild, member_guid);
	if (!guild_member1)		
	{
		PERROR(ERROR_MEMBER_NOT_EXIT);
		return;
	}

	if (guild_member->guid() == member_guid)
	{
		PERROR(ERROR_OP_SELF);
		return;
	}

	int duty = msg.new_duty();
	if (duty < e_member_type_leader || duty > e_member_type_common)
	{
		GLOBAL_ERROR;
		return;
	}
	if (duty == guild_member1->zhiwu())		
	{
		PERROR(ERROR_CHANGE_DUTY);
		return;
	}
	if (duty == e_member_type_leader)		
	{
		GuildOperation::create_guild_event(guild, guild_member1->player_name(), e_guild_op_type_zhuanrang);
		guild_member->set_zhiwu(e_member_type_senator);
		guild->set_leader_name(guild_member1->player_name());
	}
	else if (duty == e_member_type_senator)
	{
		if (GuildOperation::get_guild_member_count(guild, e_member_type_senator) == 3)
		{
			PERROR(ERROR_GUILD_SENATOR_LIMITED);
			return;
		}
		//GuildOperation::create_guild_event(guild, guild_member1->player_name(), e_guild_op_type_renming);
	}
	else
	{
		GuildOperation::create_guild_event(guild, guild_member1->player_name(), e_guild_op_type_jiechu);
	}

	guild_member1->set_zhiwu(duty);
	ResMessage::res_success(player, true, name, id);
}

// 设置加入公会条件
void GuildManager::terminal_guild_set_join_condition(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_set_join_condition msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());	
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}
	if (guild_member->zhiwu() == e_member_type_common)	
	{
		PERROR(ERROR_GUILD_AUTHORITY_REFUSE);
		return;
	}

	int bf = msg.min_bf();
	if (bf < 0 || bf > GUILD_BF_MAX)
	{
		PERROR(ERROR_GUILD_BF_INVALID);		
		return;
	}
	guild->set_bftj(bf);

	ResMessage::res_success(player, true, name, id);
}

//  剔除成员
void GuildManager::terminal_guild_kick_member(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_kick_member msg;
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
	
	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->juntuan_apply())
	{
		PERROR(ERROR_GUILD_PVP_BAOMING);
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t member_guid = msg.member_guid();
	dhc::guild_member_t *guild_member1 = GuildOperation::get_guild_member(guild, member_guid);
	if (!guild_member1)
	{
		PERROR(ERROR_MEMBER_NOT_EXIT);
		return;
	}
	const std::string kick_name = guild_member1->player_name();
	
	if (guild_member->zhiwu() >= guild_member1->zhiwu())	
	{
		PERROR(ERROR_GUILD_AUTHORITY_REFUSE);
		return;
	}

	if (guild_member->guid() == guild_member1->guid())
	{
		PERROR(ERROR_OP_SELF);
		return;
	}

	dhc::player_t *mem_player = POOL_GET_PLAYER(guild_member1->player_guid());
	if (!mem_player)
	{
		protocol::self::self_player_load smsg;
		smsg.set_data(data);
		smsg.set_name(name);
		smsg.set_id(id);
		std::string s;
		smsg.SerializeToString(&s);
		sPlayerLoad->load_player(guild_member1->player_guid(), SELF_PLAYER_LOAD_GUILD_KICK, s);
		return;
	}
	else
	{
		game::channel()->refresh_offline_time(guild_member1->player_guid());
	}

	if (mem_player)
	{
		mem_player->set_guild(0);
		mem_player->set_last_leave_guild_time(game::timer()->now());

		mem_player->set_guild_lnum(mem_player->guild_lnum() + 1);
		if (mem_player->guild_lnum() >= 3)
		{
			mem_player->set_guild_lnum(0);
			mem_player->set_guild_ltime(game::timer()->now() + 7 * 24 * 60 * 60 * 1000);
		}
	}
	GuildOperation::post_leave_guild(guild_member1->player_guid(), guild->name(), 0);

	POOL_REMOVE(guild_member1->guid(), guild->guid());		

	for (int i = 0; i != guild->member_guids_size();)	
	{
		dhc::guild_member_t *p_mem = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (!p_mem)
		{
			guild->mutable_member_guids()->SwapElements(i, guild->member_guids_size() - 1);
			guild->mutable_member_guids()->RemoveLast();
		}
		else
		{
			i++;
		}
	}

	dhc::guild_event_t *guild_event = GuildOperation::create_guild_event(guild, kick_name, e_guild_op_type_kick);
	if (!guild_event)
	{
		GLOBAL_ERROR;
		return;
	}
	ResMessage::res_success(player, true, name, id);
}

// 离开公会
void GuildManager::terminal_guild_leave(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->juntuan_apply())
	{
		PERROR(ERROR_GUILD_PVP_BAOMING);
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		PERROR(ERROR_MEMBER_LEAVE);
		return;
	}

	/// 军长要先将军长职位赋予其他成员后才可以离开公会
	if (guild_member->zhiwu() == e_member_type_leader)	
	{
		if (guild->member_guids_size() > 1)
		{
			ResMessage::res_success(player, false, name, id);
			return;
		}
		else
		{
			sGuildPool->delete_guild(guild);
			ResMessage::res_success(player, true, name, id);
			return;
		}
	}

	POOL_REMOVE(guild_member->guid(), guild->guid());		

	for (int i = 0; i != guild->member_guids_size();)	
	{
		dhc::guild_member_t *p_mem = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (!p_mem)
		{
			guild->mutable_member_guids()->SwapElements(i, guild->member_guids_size() - 1);
			guild->mutable_member_guids()->RemoveLast();
		}
		else
		{
			i++;
		}

	}

	player->set_guild(0);
	player->set_last_leave_guild_time(game::timer()->now());

	dhc::guild_event_t *guild_event = GuildOperation::create_guild_event(guild, player->name(), e_guild_op_type_exit);
	if (!guild_event)
	{
		GLOBAL_ERROR;
		return;
	}

	ResMessage::res_success(player, true, name, id);
}

// 解散公会
void GuildManager::terminal_guild_dismiss(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->juntuan_apply() > 0)
	{
		PERROR(ERROR_GUILD_PVP_BAOMING);
		return;
	}

	if (guild->member_guids_size() >= 10)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() != e_member_type_leader)
	{
		PERROR(ERROR_GUILD_AUTHORITY_REFUSE);
		return;
	}

	sGuildPool->delete_guild(guild);		

	player->set_guild(0);
	player->set_last_leave_guild_time(game::timer()->now());

	ResMessage::res_success(player, true, name, id);
}


void GuildManager::terminal_guild_tanhe(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_kick_member msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->juntuan_apply())
	{
		PERROR(ERROR_GUILD_PVP_BAOMING);
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member1 = GuildOperation::get_guild_member(guild, msg.member_guid());
	if (!guild_member1)
	{
		PERROR(ERROR_MEMBER_NOT_EXIT);
		return;
	}

	if (guild_member1->zhiwu() != e_member_type_leader)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() == e_member_type_leader)
	{
		GLOBAL_ERROR;
		return;
	}

	if (game::timer()->now() < guild_member1->offline_time() + JOIN_GUILD_TIME_LIMITED * 7)
	{
		GLOBAL_ERROR;
		return;
	}

	int need_jewel = 2000;
	if (guild_member->zhiwu() == e_member_type_senator)
	{
		need_jewel = 200;
	}
	if (player->jewel() < need_jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	GuildOperation::create_guild_event(guild, player->name(), e_guild_op_type_tanhe, guild_member->zhiwu());
	PlayerOperation::player_dec_resource(player, resource::JEWEL, need_jewel, LOGWAY_GUILD_TANHE);
	guild->set_leader_name(player->name());
	guild_member1->set_zhiwu(guild_member->zhiwu());
	guild_member->set_zhiwu(e_member_type_leader);

	ResMessage::res_success(player, true, name, id);
}

// 查看公会成员
void GuildManager::terminal_guild_member_view(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->guild() != guild_member->guild_guid())
	{
		GLOBAL_ERROR;
		return;
	}

	std::list<dhc::guild_member_t*> member_list;
	for (int i = 0; i != guild->member_guids_size(); ++i)
	{
		dhc::guild_member_t *member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (member)
		{
			member_list.push_back(member);
		}
	}

	member_list.sort(GuildOperation::member_compare);	

	ResMessage::res_guild_member_view(player, member_list, name, id);
}

// 公会排行榜
void GuildManager::terminal_guild_ranking(const std::string &data, const std::string &name, int id)
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

	std::list<dhc::guild_t*>  guild_list;
	sGuildPool->get_guild_rank_list(guild_list);

	ResMessage::res_guild_ranking(player, guild_list, name, id);
}

void GuildManager::terminal_guild_mission_ranking(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_guild_mission_ranking msg1;
	sGuildPool->get_guild_mission_compare(msg1);

	ResMessage::res_guild_mission_ranking(player, msg1, name, id);
}

// 打开活动
void GuildManager::terminal_guild_activity(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());	
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}
	protocol::game::smsg_guild_activity msg1;
	msg1.set_sign_flag(player->guild_sign_flag());
	for (int i = 0; i < player->guild_honors_size(); ++i)
	{
		msg1.add_mobai_ids(player->guild_honors(i));
	}
	ResMessage::res_guild_activity(player, msg1, name, id);
}

// 公会签到
void GuildManager::terminal_guild_sign_in(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_sign_in msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());;
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (game::timer()->run_day(guild_member->last_sign_time()) == 0)
	{
		PERROR(ERROR_HAS_SIGN_IN);
		return;
	}

	if (game::timer()->run_day(player->guild_sign_time()) == 0)
	{
		PERROR(ERROR_HAS_SIGN_IN);			
		return;
	}
	int sign_in_type = msg.sign_in_type();
	if (sign_in_type < e_sign_in_type_primary || sign_in_type > e_sign_in_type_advanced)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_guild_sign *t_sign = sGuildConfig->get_guild_sign(sign_in_type);
	if (!t_sign)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->gold() < t_sign->gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}
	if (player->jewel() < t_sign->jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	dhc::guild_event_t *guild_event = GuildOperation::create_guild_event(guild, player->name(), sign_in_type + e_guild_op_type_sign_in1, t_sign->exp);
	if (!guild_event)
	{
		GLOBAL_ERROR;
		return;
	}
	guild->set_mobai_num(guild->mobai_num() + 1);
	guild->set_honor(guild->honor() + t_sign->honor);
	player->set_guild_sign_time(game::timer()->now());
	guild_member->set_last_sign_time(game::timer()->now());
	guild_member->set_contribution(guild_member->contribution() + t_sign->contrubution);
	player->set_guild_sign_flag(sign_in_type + 1);
	guild_member->set_sign_flag(sign_in_type + 1);
	GuildOperation::mod_guild_exp(guild, t_sign->exp);
	PlayerOperation::player_add_resource(player, resource::CONTRIBUTION, t_sign->contrubution, LOGWAY_GUILD_SIGN_IN);
	if (t_sign->gold > 0)
	{
		PlayerOperation::player_dec_resource(player, resource::GOLD, t_sign->gold, LOGWAY_GUILD_SIGN_IN);
	}
	if (t_sign->jewel > 0)
	{
		PlayerOperation::player_dec_resource(player, resource::JEWEL, t_sign->jewel, LOGWAY_GUILD_SIGN_IN);
	}

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_sign_reward(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_sign_reward msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());;
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->guild_honors_size(); ++i)
	{
		if (player->guild_honors(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	const s_t_guild_mobai *t_mobai = sGuildConfig->get_guild_mobai(msg.id());
	if (!t_mobai)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->honor() < (t_mobai->honor + (guild->last_level() - 1) * t_mobai->jindu_jc))
	{
		GLOBAL_ERROR;
		return;
	}
	player->add_guild_honors(msg.id());

	s_t_rewards rds;
	s_t_reward rd = t_mobai->rd;
	rd.value2 += (guild->last_level() - 1) * t_mobai->value2_jc;
	rds.add_reward(rd);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_GUILD_SIGN_IN);

	ResMessage::res_success(player, true, name, id);

}

void GuildManager::terminal_guild_mission_look(const std::string &data, const std::string &name, int id)
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
	PCK_CHECK_NO_GOLD;

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_mission_t *guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
	if (!guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->mbnum() > 0)
	{
		player->set_guild_num(player->guild_num() + guild_member->mbnum());
		guild_member->set_mbnum(0);
	}

	protocol::game::smsg_guild_mission_look msg1;
	msg1.mutable_mission()->CopyFrom(*guild_mission);
	msg1.set_num(player->guild_num());
	msg1.set_buy_num(player->guild_buy_num());
	msg1.set_last_time(guild_member->last_mbtime());
	for (int i = 0; i < player->guild_shilians_size(); ++i)
	{
		msg1.add_mission_rewards(player->guild_shilians(i));
	}
	for (int i = 0; i < player->guild_rewards_size(); ++i)
	{
		msg1.add_mission_rewards(player->guild_rewards(i));
	}
	ResMessage::res_guild_boss_look(player, msg1, name, id);
}

void GuildManager::terminal_guild_mission_look_ex(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_mission_t *guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
	if (!guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->mbnum() > 0)
	{
		player->set_guild_num(player->guild_num() + guild_member->mbnum());
		guild_member->set_mbnum(0);
	}

	protocol::game::smsg_guild_mission_look msg1;
	msg1.mutable_mission()->CopyFrom(*guild_mission);
	msg1.set_num(player->guild_num());
	msg1.set_buy_num(player->guild_buy_num());
	msg1.set_last_time(guild_member->last_mbtime());
	for (int i = 0; i < player->guild_shilians_size(); ++i)
	{
		msg1.add_mission_rewards(player->guild_shilians(i));
	}
	for (int i = 0; i < player->guild_rewards_size(); ++i)
	{
		msg1.add_mission_rewards(player->guild_rewards(i));
	}
	ResMessage::res_guild_boss_look_ex(player, msg1, name, id);
}

void GuildManager::terminal_guild_buy_mission_num(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_vip* t_vip = sPlayerConfig->get_vip(player->vip());
	if (!t_vip)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->guild_buy_num() >= t_vip->guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->guild_buy_num() + msg.num() > t_vip->guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}
	int price = 0;
	const s_t_price* t_price = 0;
	for (int i = 0; i < msg.num(); ++i)
	{
		t_price = sPlayerConfig->get_price(player->guild_buy_num() + i + 1);
		if (!t_price)
		{
			GLOBAL_ERROR;
			return;
		}
		price += t_price->guild_mission;
	}
	if (player->jewel() < price)
	{
		GLOBAL_ERROR;
		return;
	}
	player->set_guild_buy_num(player->guild_buy_num() + msg.num());
	player->set_guild_num(player->guild_num() + msg.num());
	PlayerOperation::player_dec_resource(player, resource::JEWEL, price, LOGWAY_GUILD_MISSION_BUY);
	
	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_message_view(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_guild_message_view msg_view;
	dhc::guild_message_t* guild_msg = 0;
	for (int i = 0; i < guild->message_guids_size(); ++i)
	{
		guild_msg = POOL_GET_GUILD_MESSAGE(guild->message_guids(i));
		if (guild_msg)
		{
			msg_view.add_msgs()->CopyFrom(*guild_msg);
		}
	}

	ResMessage::res_guild_message_view(player, msg_view, name, id);
}


void GuildManager::terminal_guild_message_add(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_message_add msg;
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

	operror_t ok = GuildOperation::check_bulletin(msg.text());
	if (ok != operror_t(0))
	{
		PERROR(ok);
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	GuildOperation::create_guild_message(player, guild, guild_member->zhiwu(), msg.text());

	protocol::game::smsg_guild_message_view msg_view;
	dhc::guild_message_t* guild_msg = 0;
	for (int i = 0; i < guild->message_guids_size(); ++i)
	{
		guild_msg = POOL_GET_GUILD_MESSAGE(guild->message_guids(i));
		if (guild_msg)
		{
			msg_view.add_msgs()->CopyFrom(*guild_msg);
		}
	}

	ResMessage::res_guild_message_view(player, msg_view, name, id);
}


void GuildManager::terminal_guild_message_delete(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_message_delete msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_message_t* guild_message = POOL_GET_GUILD_MESSAGE(msg.msg_guid());
	if (!guild_message)
	{
		GLOBAL_ERROR;
		return;
	}

	GuildOperation::delete_guild_message(guild, msg.msg_guid());

	protocol::game::smsg_guild_message_view msg_view;
	dhc::guild_message_t* guild_msg = 0;
	for (int i = 0; i < guild->message_guids_size(); ++i)
	{
		guild_msg = POOL_GET_GUILD_MESSAGE(guild->message_guids(i));
		if (guild_msg)
		{
			msg_view.add_msgs()->CopyFrom(*guild_msg);
		}
	}

	ResMessage::res_guild_message_view(player, msg_view, name, id);
}

void GuildManager::terminal_guild_message_top(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_message_top msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_message_t* guild_message = POOL_GET_GUILD_MESSAGE(msg.msg_guid());
	if (!guild_message)
	{
		GLOBAL_ERROR;
		return;
	}

	GuildOperation::top_guild_message(guild, msg.msg_guid());

	protocol::game::smsg_guild_message_view msg_view;
	dhc::guild_message_t* guild_msg = 0;
	for (int i = 0; i < guild->message_guids_size(); ++i)
	{
		guild_msg = POOL_GET_GUILD_MESSAGE(guild->message_guids(i));
		if (guild_msg)
		{
			msg_view.add_msgs()->CopyFrom(*guild_msg);
		}
	}

	ResMessage::res_guild_message_view(player, msg_view, name, id);
}

void GuildManager::terminal_guild_mission_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_mission_fight_end msg;
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

	if (game::timer()->run_day(player->last_leave_guild_time()) == 0)
	{
		PERROR(ERROR_GUILD_ATTACK_LIMIT);
		return;
	}

	if (!PlayerOperation::check_fight_time(player))
	{
		PERROR(ERROR_FIGHT_TIME);
		return;
	}
	int now_time = game::timer()->hour();
	if (now_time < 10 || now_time >= 22)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}
	if (guild->level() < 2)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->guild_num() < 1)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_mission_t* guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
	if (!guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.index() < 0 || msg.index() >= 4)
	{
		GLOBAL_ERROR;
		return;
	}

	int start = msg.index() * 5;
	int end = msg.index() * 5 + 5;
	double cur_hp = 0;
	double total_hp = 0.0;
	for (int i = start; i < end; ++i)
	{
		if (i >= guild_mission->guild_monsters_size())
		{
			GLOBAL_ERROR;
			return;
		}
		cur_hp += (double)guild_mission->guild_cur_hps(i);
		total_hp += (double)guild_mission->guild_max_hps(i);
	}
	if (cur_hp <= 0.0)
	{
		PERROR(ERROR_GUILD_BOSS_KILL);
		return;
	}
	if (total_hp <= 0.0)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_mission* t_guild_mission = sGuildConfig->get_guild_mission(guild_mission->guild_ceng());
	if (!t_guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->level() < t_guild_mission->level)
	{
		GLOBAL_ERROR;
		return;
	}
	
	int hit_contri = 0;
	double hp = 0;
	std::string text;
	int result = MissionFight::mission_guild(player, guild_mission, msg.index(), hp, text);
	GuildOperation::mod_guild_mission_damage(player, guild_mission, static_cast<int64_t>(hp));
	int stat = GuildOperation::mod_guild_mission(player, guild_mission, msg.index());
	sGuildPool->update_guild_mission_compare(guild, guild_mission);
	if (result == 1)
	{
		if (stat == 1)
		{
			hit_contri = t_guild_mission->hit_contribution;
		}
	}
	else
	{

	}
	int contri = t_guild_mission->max_contribution;
	double hit_rate = (double)hp / total_hp;
	if (hit_rate < 0.01)
	{
		contri = t_guild_mission->min_contribution + (t_guild_mission->max_contribution - t_guild_mission->min_contribution) * hp / (total_hp / 100);
	}
	player->set_guild_num(player->guild_num() - 1);
	PlayerOperation::player_add_resource(player, resource::CONTRIBUTION, contri + hit_contri, LOGWAY_GUILD_MISSION_END);
	sHuodongPool->huodong_active(player, HUODONG_COND_GUILD_COUNT, 1);
	
	ResMessage::res_guild_boss_fight_end(player, result, text, contri, hit_contri, hp, name, id);
}

void GuildManager::terminal_guild__mission_complate_reward_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_mission_complete_reward_view msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_mission_t* guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
	if (!guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild* t_guild = sGuildConfig->get_guild(guild->level());
	if (!t_guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.ceng() > guild_mission->guild_ceng())
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_guild_mission_complete_reward_view smsg;
	dhc::guild_box_t *guild_box = 0;
	for (int i = 0; i < guild->box_guids_size(); ++i)
	{
		guild_box = POOL_GET_GUILD_BOX(guild->box_guids(i));
		if (guild_box && guild_box->mceng() == msg.ceng())
		{
			smsg.add_boxes()->CopyFrom(*guild_box);
		}
	}

	if (smsg.boxes_size() <= 0)
	{
		int guild_box_num = (t_guild->member_num / 5 + 1) * 5;
		for (int i = 0; i < 4; ++i)
		{
			guild_box = new dhc::guild_box_t();
			guild_box->set_guid(game::gtool()->assign(et_guild_box));
			guild_box->set_guild_guid(guild->guid());
			guild_box->set_mceng(msg.ceng());
			guild_box->set_mindex(i + 1);
			
			for (int j = 0; j < guild_box_num; ++j)
			{
				guild_box->add_reward_ids(0);
				guild_box->add_reward_guids(0);
				guild_box->add_reward_names("");
				guild_box->add_reward_achieves(0);
			}
			smsg.add_boxes()->CopyFrom(*guild_box);

			POOL_ADD_NEW(guild_box->guid(), guild_box);
			guild->add_box_guids(guild_box->guid());
		}
	}

	ResMessage::res_guild_mission_complete_reward_view(player, smsg, name, id);
}

void GuildManager::terminal_guild_mission_complete_reward(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_mission_complete_reward msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild *t_guild = sGuildConfig->get_guild(guild->level());
	if (!t_guild)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_guild->box_num.size() != 5)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_mission_t *guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
	if (!guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_mission *t_guild_mission = sGuildConfig->get_guild_mission(msg.ceng());
	if (!t_guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.index() < 0 || msg.index() >= t_guild_mission->rewards.size())
	{
		GLOBAL_ERROR;
		return;
	}

	if (!GuildOperation::has_guild_mission_reward(guild_mission, msg.ceng(), msg.index() + 1))
	{
		GLOBAL_ERROR;
		return;
	}

	if (GuildOperation::has_guild_member_reward(player, msg.ceng(), msg.index() + 1))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_box_t *guild_box = 0;
	for (int i = 0; i < guild->box_guids_size(); ++i)
	{
		dhc::guild_box_t *box = POOL_GET_GUILD_BOX(guild->box_guids(i));
		if (box && box->mceng() == msg.ceng() && box->mindex() == msg.index() + 1)
		{
			guild_box = box;
			break;
		}
	}

	if (!guild_box)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.box() < 0 || msg.box() >= guild_box->reward_guids_size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (guild_box->reward_guids(msg.box()) != 0)
	{
		PERROR(ERROR_GUILD_BOX);
		return;
	}

	std::vector<int> member_nums = t_guild->box_num;
	for (int i = 0; i < guild_box->reward_guids_size(); ++i)
	{
		if (guild_box->reward_guids(i) == player->guid())
		{
			GLOBAL_ERROR;
			return;
		}

		if (guild_box->reward_guids(i) != 0 &&
			guild_box->reward_ids(i) >= 0 &&
			guild_box->reward_ids(i) < member_nums.size())
		{
			member_nums[guild_box->reward_ids(i)] -= 1;
		}
	}
	std::vector<int> left_nums;
	for (int i = 0; i < member_nums.size(); ++i)
	{
		if (member_nums[i] > 0)
		{
			for (int kmg = 0; kmg < member_nums[i]; ++kmg)
			{
				left_nums.push_back(i);
			}
		}
	}
	if (left_nums.size() <= 0)
	{
		GLOBAL_ERROR;
		return;
	}
	int box_index = left_nums[Utils::get_int32(0, left_nums.size() - 1)];
	if (box_index < 0 || box_index >= t_guild_mission->rewards[msg.index()].size())
	{
		GLOBAL_ERROR;
		return;
	}
	
	s_t_rewards rds;
	rds.add_reward(t_guild_mission->rewards[msg.index()][box_index]);
	GuildOperation::add_guild_member_reward(player, msg.ceng(), msg.index() + 1);
	guild_box->set_reward_guids(msg.box(), player->guid());
	guild_box->set_reward_ids(msg.box(), box_index);
	guild_box->set_reward_names(msg.box(), player->name());
	guild_box->set_reward_achieves(msg.box(), player->dress_achieves_size());
	PlayerOperation::player_add_reward(player, rds, LOGWAY_GUILD_MISSION_COMPLETE_REWARD);

	ResMessage::res_guild_mission_complete_reward(player, box_index, name, id);
}

void GuildManager::terminal_guild_mission_shilian_reward(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_mission_ceng_reward msg;
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	/*if (game::timer()->run_day(player->last_leave_guild_time()) <= 0)
	{
		GLOBAL_ERROR;
		return;
	}*/

	dhc::guild_mission_t *guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
	if (!guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_mission *t_guild_mission = sGuildConfig->get_guild_mission(msg.ceng());
	if (!t_guild_mission)
	{
		GLOBAL_ERROR;
		return;
	}

	if (!GuildOperation::has_guild_mission_reward(guild_mission, msg.ceng(), 0))
	{
		GLOBAL_ERROR;
		return;
	}

	/*if (GuildOperation::has_guild_member_reward(guild_member, msg.ceng(), 0))
	{
	GLOBAL_ERROR;
	return;
	}*/

	int reward_id = msg.ceng() * 10;
	for (int i = 0; i < player->guild_rewards_size(); ++i)
	{
		if (player->guild_rewards(i) == reward_id)
		{
			GLOBAL_ERROR;
			return;
		}
	}

	//GuildOperation::add_guild_member_reward(guild_member, msg.ceng(), 0);
	player->add_guild_rewards(reward_id);

	
	s_t_rewards rds;
	rds.add_reward(t_guild_mission->first_rewards);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_GUILD_MISSION_SHILIAN_REWARD);

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_keji_open(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_keji msg;
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

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() == e_member_type_common)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_skill* t_keji = sGuildConfig->get_guild_skill(msg.id());
	if (!t_keji)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->level() < t_keji->guild_level)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < guild->skill_ids_size(); ++i)
	{
		if (guild->skill_ids(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	int exp = t_keji->exp;
	if (guild->exp() < exp)
	{
		GLOBAL_ERROR;
		return;
	}
	guild->add_skill_ids(msg.id());
	guild->add_skill_levels(1);
	guild->set_exp(guild->exp() - exp);

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_keji_up(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_keji msg;
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

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() == e_member_type_common)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_skill* t_keji = sGuildConfig->get_guild_skill(msg.id());
	if (!t_keji)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->level() < t_keji->guild_level)
	{
		GLOBAL_ERROR;
		return;
	}

	int index = -1;
	for (int i = 0; i < guild->skill_ids_size(); ++i)
	{
		if (guild->skill_ids(i) == msg.id())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	int max_level = (guild->level() - t_keji->guild_level + 1) * t_keji->study_level;
	if (guild->skill_levels(index) >= max_level)
	{
		GLOBAL_ERROR;
		return;
	}
	int exp = guild->skill_levels(index) * t_keji->exp_jiacheng + t_keji->exp;
	if (guild->exp() < exp)
	{
		GLOBAL_ERROR;
		return;
	}
	guild->set_skill_levels(index, guild->skill_levels(index) + 1);
	guild->set_exp(guild->exp() - exp);

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_keji_study(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_keji msg;
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

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_skill* t_keji = sGuildConfig->get_guild_skill(msg.id());
	if (!t_keji)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->level() < t_keji->guild_level)
	{
		GLOBAL_ERROR;
		return;
	}

	bool has_keji = false;
	for (int i = 0; i < guild->skill_ids_size(); ++i)
	{
		if (guild->skill_ids(i) == msg.id())
		{
			has_keji = true;
			break;
		}
	}
	if (!has_keji)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->guild_skill_ids_size(); ++i)
	{
		if (player->guild_skill_ids(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	int contri = t_keji->contri;
	if (player->contribution() < contri)
	{
		GLOBAL_ERROR;
		return;
	}

	player->add_guild_skill_ids(msg.id());
	player->add_guild_skill_levels(1);
	PlayerOperation::player_dec_resource(player, resource::CONTRIBUTION, contri, LOGWAY_GUILD_KEJI_STUDY);

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_keji_skillup(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_keji msg;
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

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guild_skill* t_keji = sGuildConfig->get_guild_skill(msg.id());
	if (!t_keji)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->level() < t_keji->guild_level)
	{
		GLOBAL_ERROR;
		return;
	}

	int keji_level = -1;
	for (int i = 0; i < guild->skill_ids_size(); ++i)
	{
		if (guild->skill_ids(i) == msg.id())
		{
			keji_level = guild->skill_levels(i);
			break;
		}
	}
	if (keji_level == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	int index = -1;
	for (int i = 0; i < player->guild_skill_ids_size(); ++i)
	{
		if (player->guild_skill_ids(i) == msg.id())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->guild_skill_levels(index) >= keji_level)
	{
		GLOBAL_ERROR;
		return;
	}

	int contri = player->guild_skill_levels(index)  * t_keji->contri_jiacheng + t_keji->contri;
	if (player->contribution() < contri)
	{
		GLOBAL_ERROR;
		return;
	}
	player->set_guild_skill_levels(index, player->guild_skill_levels(index) + 1);
	PlayerOperation::player_dec_resource(player, resource::CONTRIBUTION, contri, LOGWAY_GUILD_KEJI_LEVELUP);

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::self_guild_load_apply(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_guild_agree(msg.data(), msg.name(), msg.id());
}

void GuildManager::self_guild_load_kick(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_guild_kick_member(msg.data(), msg.name(), msg.id());
}

void GuildManager::terminal_guild_red_deliver(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_red_deliver msg;
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

	if (player->guild_red_num() >= 1)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, msg.id()) < 1)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	const s_t_item* t_item = sItemConfig->get_item(msg.id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_red_t *guild_red = GuildOperation::create_guild_red(player, guild, msg.id(), t_item->jewel, msg.text());
	if (!guild_red)
	{
		GLOBAL_ERROR;
		return;
	}

	ItemOperation::item_destory_templete(player, msg.id(), 1, LOGWAY_GUILD_RED_DELIVER);
	player->set_guild_red_num(1);
	player->set_guild_deliver_jewel(player->guild_deliver_jewel() + t_item->jewel);

	protocol::game::smsg_guild_red_deliver smsg;
	smsg.mutable_guild_red()->CopyFrom(*guild_red);

	ResMessage::res_guild_red_deliver(player, smsg, name, id);
}

void GuildManager::terminal_guild_red_view(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_guild_red_view smsg;
	dhc::guild_red_t *guild_red = 0;
	for (int i = 0; i < guild->red_guids_size(); ++i)
	{
		guild_red = POOL_GET_GUILD_RED(guild->red_guids(i));
		if (guild_red)
		{
			smsg.add_guild_reds()->CopyFrom(*guild_red);
		}
	}

	ResMessage::res_guild_red_view(player, smsg, name, id);
}

void GuildManager::terminal_guild_red_rob(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_red_rob msg;
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

	if (player->guild_red_num1() >= 5)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	bool has = false;
	for (int i = 0; i < guild->red_guids_size(); ++i)
	{
		if (guild->red_guids(i) == msg.guid())
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

	dhc::guild_red_t *guild_red = POOL_GET_GUILD_RED(msg.guid());
	if (!guild_red)
	{
		GLOBAL_ERROR;
		return;
	}

	if (game::timer()->now() > guild_red->time() + 24 * 60 * 60 * 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_red->player_guid_size() >= 20)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < guild_red->player_guid_size(); ++i)
	{
		if (guild_red->player_guid(i) == player->guid())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	int jewel = 0;
	int remain_size = 20 - guild_red->player_guid_size();
	if (remain_size == 1)
	{
		jewel = guild_red->remain();
	}
	else
	{
		int min = 1;
		int max = guild_red->remain() / remain_size * 2;

		jewel = Utils::get_int32(1, 100) * max / 100;
		if (jewel < min)
		{
			jewel = min;
		}
	}

	guild_red->set_remain(guild_red->remain() - jewel);
	guild_red->add_player_guid(player->guid());
	guild_red->add_player_ids(player->template_id());
	guild_red->add_player_names(player->name());
	guild_red->add_player_vip(player->vip());
	guild_red->add_player_achieve(player->dress_achieves_size());
	guild_red->add_player_jewel(jewel);
	guild_red->add_player_nalflag(player->nalflag());

	PlayerOperation::player_add_resource(player, resource::JEWEL, jewel, LOGWAY_GUILD_RED_ROB);
	player->set_guild_red_num1(player->guild_red_num1() + 1);
	player->set_guild_rob_jewel(player->guild_rob_jewel() + jewel);
	ResMessage::res_guild_red_rob(player, jewel, name, id);

}

void GuildManager::terminal_guild_red_target(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guild_red_target msg;
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

	for (int i = 0; i < player->guild_red_rewards_size(); ++i)
	{
		if (player->guild_red_rewards(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	const s_t_guild_hongbao_target* t_guild_red = sGuildConfig->get_guild_hongbao_target(msg.id());
	if (!t_guild_red)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_guild_red->type == 1)
	{
		if (player->guild_deliver_jewel() < t_guild_red->count)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}
	else
	{
		if (player->guild_rob_jewel() < t_guild_red->count)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}

	player->add_guild_red_rewards(msg.id());
	s_t_rewards rds;
	rds.add_reward(t_guild_red->rewards);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_GUILD_RED_TARGET);

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_pvp_baoming(const std::string &data, const std::string &name, int id)
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

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		GLOBAL_ERROR;
		return;
	}

	if (global->guild_pvp_zhou() % 2 != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	int day = sGuildPool->get_guild_pvp_week();
	int now_time = sGuildPool->get_guild_pvp_hour();
	if (day != 1 && day != 2)
	{
		GLOBAL_ERROR;
		return;
	}
	if (day == 2 && now_time >= 10)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->pvp_guild() <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->juntuan_apply())
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->level() < 5)
	{
		PERROR(ERROR_LEVEL);
		return;
	}

	if (guild->member_guids_size() < 22)
	{
		PERROR(ERROR_GUILD_PVP_MEMBER);
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() == e_member_type_common)
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t now_join_time = game::timer()->now();
	std::set<uint64_t> member_guids;
	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (!guild_member)
		{
			continue;
		}
		if (now_join_time - guild_member->join_time() < JOIN_GUILD_TIME_LIMITED)
		{
			continue;
		}
		member_guids.insert(guild_member->player_guid());
	}

	if (member_guids.size() < 22)
	{
		PERROR(ERROR_GUILD_PVP_MEMBER);
		return;
	}

	rpcproto::tmsg_req_guild_pvp_baoming tmsg;
	tmsg.set_guild(guild->pvp_guild());
	tmsg.set_guild_server(boost::lexical_cast<int>(player->serverid()));
	tmsg.set_guild_name(guild->name());
	tmsg.set_guild_icon(guild->icon());
	tmsg.set_guild_level(guild->level());

	for (std::set<uint64_t>::const_iterator it = member_guids.begin();
		it != member_guids.end();
		++it)
	{
		tmsg.add_player_guids(*it);
	}

	std::string s;
	tmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_BAOMING, s,
		boost::bind(&GuildManager::terminal_guild_pvp_baoming_callback, this, _1, player->guid(), name, id));
}

void GuildManager::terminal_guild_pvp_baoming_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id)
{
	rpcproto::tmsg_rep_guild_pvp_baoming msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!msg.res())
	{
		PERROR(ERROR_GUILD_PVP_LOOK);
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	guild->set_juntuan_apply(1);

	for (int i = 0; i < msg.player_guids_size(); ++i)
	{
		guild->add_pvp_guilds(msg.player_guids(i));
	}

	if (guild->pvp_guilds_size() > 0)
	{
		sGuildPool->add_guild_pvp_sync(guild->guid());
	}

	protocol::game::smsg_guild_fight_pvp_look smsg;
	smsg.set_stat(e_pvp_guild_look_bushu);

	for (int i = 0; i < 22; ++i)
	{
		smsg.mutable_arrange()->add_player_guids(0);
		smsg.mutable_arrange()->add_player_names("");
		smsg.mutable_arrange()->add_player_template(0);
		smsg.mutable_arrange()->add_player_level(0);
		smsg.mutable_arrange()->add_player_bat_eff(0);
		smsg.mutable_arrange()->add_player_vip(0);
		smsg.mutable_arrange()->add_player_achieve(0);
	}

	dhc::guild_member_t *guild_member = 0;
	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (!guild_member)
		{
			continue;
		}
		smsg.mutable_member()->add_player_guids(guild_member->player_guid());
		smsg.mutable_member()->add_player_names(guild_member->player_name());
		smsg.mutable_member()->add_player_level(guild_member->player_level());
		smsg.mutable_member()->add_player_template(guild_member->player_iocn_id());
		smsg.mutable_member()->add_player_bat_eff(guild_member->bf());
		smsg.mutable_member()->add_player_vip(guild_member->player_vip());
		smsg.mutable_member()->add_player_achieve(guild_member->player_achieve());
		smsg.mutable_member()->add_player_join_time(guild_member->join_time());
	}

	ResMessage::res_guild_pvp_look(player, smsg, name, id);
}

void GuildManager::terminal_guild_pvp_look(const std::string &data, const std::string &name, int id)
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

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		GLOBAL_ERROR;
		return;
	}

	int state = GuildOperation::get_guild_pvp_state(global, guild);
	if (state == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (state == e_pvp_guild_look_bushu)
	{
		rpcproto::tmsg_req_guild_look tmsg;
		tmsg.set_player_guid(player_guid);
		tmsg.set_player_guild(guild->pvp_guild());
		std::string s;
		tmsg.SerializeToString(&s);
		game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_LOOK_BUSHU, s,
			boost::bind(&GuildManager::terminal_guild_pvp_look_bushu_callback, this, _1, player->guid(), name, id));
		return;
	}
	else if (state == e_pvp_guild_look_pipei)
	{
		rpcproto::tmsg_req_guild_look tmsg;
		tmsg.set_player_guid(player_guid);
		tmsg.set_player_guild(guild->pvp_guild());
		std::string s;
		tmsg.SerializeToString(&s);
		game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_LOOK_PIPEI, s,
			boost::bind(&GuildManager::terminal_guild_pvp_look_pipei_callback, this, _1, player->guid(), name, id));
		return;
	}
	else if (state == e_pvp_guild_look_zhanji)
	{
		rpcproto::tmsg_req_guild_look smsg;
		smsg.set_player_guid(player_guid);
		smsg.set_player_guild(guild->pvp_guild());
		std::string s;
		smsg.SerializeToString(&s);
		game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_LOOK_ZHANJI, s,
			boost::bind(&GuildManager::terminal_guild_pvp_look_zhanji_callback, this, _1, player->guid(), name, id));
		return;
	}
	else if (state == e_pvp_guild_xiuzhan)
	{
		rpcproto::tmsg_req_guild_look smsg;
		smsg.set_player_guid(player_guid);
		smsg.set_player_guild(guild->pvp_guild());
		std::string s;
		smsg.SerializeToString(&s);
		game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_LOOK_XIUZHAN, s,
			boost::bind(&GuildManager::terminal_guild_pvp_xiuzhan_callback, this, _1, player->guid(), name, id));
		return;
	}
	else
	{
		protocol::game::smsg_guild_fight_pvp_look smsg;
		smsg.set_stat(state);
		ResMessage::res_guild_fight_look(player, smsg, name, id);
	}
} 

void GuildManager::terminal_guild_pvp_bushu(const std::string &data, const std::string& name, int id)
{
	protocol::game::cmsg_guild_pvp_bushu msg;
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

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		GLOBAL_ERROR;
		return;
	}

	if (global->guild_pvp_zhou() % 2 != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->juntuan_apply() <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->pvp_guild() < 0)
	{
		GLOBAL_ERROR;
		return;
	}

	int week = sGuildPool->get_guild_pvp_week();
	if (sGuildPool->get_guild_pvp_hour() >= 10 && week != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild_member->zhiwu() == e_member_type_common)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.player_guids_size() != 22)
	{
		GLOBAL_ERROR;
		return;
	}

	std::set<uint64_t> member_guids;
	uint64_t now_time = game::timer()->now();

	rpcproto::tmsg_req_guild_pvp_bushu tmsg;
	tmsg.set_guild(guild->pvp_guild());
	tmsg.set_guild_name(guild->name());
	tmsg.set_guild_icon(guild->icon());
	tmsg.set_guild_level(guild->level());

	for (int i = 0; i < msg.player_guids_size(); ++i)
	{
		if (msg.player_guids(i) == 0)
		{
			GLOBAL_ERROR;
			return;
		}
		guild_member = GuildOperation::get_guild_member(guild, msg.player_guids(i));
		if (!guild_member)
		{
			GLOBAL_ERROR;
			return;
		}
		if (now_time - guild_member->join_time() < JOIN_GUILD_TIME_LIMITED)
		{
			GLOBAL_ERROR;
			return;
		}
		if (member_guids.find(msg.player_guids(i)) != member_guids.end())
		{
			GLOBAL_ERROR;
			return;
		}
		member_guids.insert(msg.player_guids(i));

		tmsg.add_player_guids(msg.player_guids(i));
		tmsg.add_player_names(guild_member->player_name());
		tmsg.add_player_template(guild_member->player_iocn_id());
		tmsg.add_player_level(guild_member->player_level());
		tmsg.add_player_bat_eff(guild_member->bf());
		tmsg.add_player_vips(guild_member->player_vip());
		tmsg.add_player_achieves(guild_member->player_achieve());
		tmsg.add_player_map_star(guild_member->map_star());
	}

	std::string s;
	tmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_BUSHU, s,
		boost::bind(&GuildManager::terminal_guild_pvp_bushu_callback, this, _1, player->guid(), name, id));

}

void GuildManager::terminal_guild_pvp_fight(const std::string &data, const std::string& name, int id)
{
	protocol::game::cmsg_guild_fight msg;
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

	if (!PlayerOperation::check_fight_time(player))
	{
		PERROR(ERROR_FIGHT_TIME);
		return;
	}

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		GLOBAL_ERROR;
		return;
	}

	if (global->guild_pvp_zhou() % 2 != 0)
	{
		PERROR(ERROR_GUILD_PVP_FIGHT_END);
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	if (guild->juntuan_apply() <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	int now_time = sGuildPool->get_guild_pvp_hour();
	if (now_time < 10)
	{
		PERROR(ERROR_GUILD_PVP_FIGHT_END);
		return;
	}
	
	if (player->guild_pvp_num() <= 0)
	{
		PERROR(ERROR_MISSION_CISHU);
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	if (game::timer()->now() - guild_member->join_time() < JOIN_GUILD_TIME_LIMITED)
	{
		PERROR(ERROR_GUILD_PVP_ATTACK_TIME);
		return;
	}

	rpcproto::tmsg_req_guild_fight tmsg;
	tmsg.set_player_guid(player->guid());
	tmsg.set_player_guild(guild->pvp_guild());
	tmsg.set_target_guild(msg.target_guild());
	tmsg.set_target_index(msg.target_index());
	PvpOperation::copy(player, tmsg.mutable_player(), PT_LIEREN);

	std::string s;
	tmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_FIGHT, s,
		boost::bind(&GuildManager::terminal_guild_pvp_fight_callback, this, _1, player->guid(), name, id));

}

void GuildManager::terminal_guild_pvp_buy(const std::string &data, const std::string& name, int id)
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

	const s_t_vip* t_vip = sPlayerConfig->get_vip(player->vip());
	if (!t_vip)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->guild_pvp_buy_num() >= t_vip->guild_fight)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->guild_pvp_buy_num() + msg.num() > t_vip->guild_fight)
	{
		GLOBAL_ERROR;
		return;
	}
	
	int price = 0;
	const s_t_price* t_price = 0;
	for (int i = 0; i < msg.num(); ++i)
	{
		t_price = sPlayerConfig->get_price(player->guild_pvp_buy_num() + i + 1);
		if (!t_price)
		{
			GLOBAL_ERROR;
			return;
		}
		price += t_price->guild_fight;
	}
	if (player->jewel() < price)
	{
		GLOBAL_ERROR;
		return;
	}
	player->set_guild_pvp_buy_num(player->guild_pvp_buy_num() + msg.num());
	player->set_guild_pvp_num(player->guild_pvp_num() + msg.num());
	PlayerOperation::player_dec_resource(player, resource::JEWEL, price, LOGWAY_GUILD_PVP_BUY);

	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_pvp_reward(const std::string &data, const std::string& name, int id)
{
	protocol::game::cmsg_guild_pvp_reward msg;
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
	
	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < guild_member->gongpo_rewards_size(); ++i)
	{
		if (msg.id() == guild_member->gongpo_rewards(i))
		{
			GLOBAL_ERROR;
			return;
		}
	}

	rpcproto::tmsg_req_guild_target_reward smsg;
	smsg.set_pvp_guid(guild->pvp_guild());

	std::string s;
	smsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_GUILDFIGHT_BUY, s,
		boost::bind(&GuildManager::terminal_guild_pvp_target_callback, this, _1, player->guid(), msg.id(), name, id));
}

void GuildManager::terminal_guild_pvp_bushu_callback(const std::string &data, uint64_t player_guid, const std::string& name, int id)
{
	rpcproto::tmsg_rep_guild_pvp_bushu msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!msg.res())
	{
		PERROR(ERROR_GUILD_PVP_LOOK);
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < msg.player_guids_size(); ++i)
	{
		guild->add_pvp_guilds(msg.player_guids(i));
	}

	if (guild->pvp_guilds_size() > 0)
	{
		sGuildPool->add_guild_pvp_sync(guild->guid());
	}

	ResMessage::res_success(player, true, name, id);	
}

void GuildManager::terminal_guild_pvp_look_bushu_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id)
{
	rpcproto::tmsg_rep_guild_look_bushu msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!msg.res())
	{
		PERROR(ERROR_GUILD_PVP_LOOK);
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_guild_fight_pvp_look smsg;
	smsg.set_stat(e_pvp_guild_look_bushu);
	for (int i = 0; i < msg.player_guids_size(); ++i)
	{
		smsg.mutable_arrange()->add_player_guids(msg.player_guids(i));
		smsg.mutable_arrange()->add_player_names(msg.player_names(i));
		smsg.mutable_arrange()->add_player_template(msg.player_template(i));
		smsg.mutable_arrange()->add_player_level(msg.player_level(i));
		smsg.mutable_arrange()->add_player_bat_eff(msg.player_bat_eff(i));
		smsg.mutable_arrange()->add_player_vip(msg.player_vips(i));
		smsg.mutable_arrange()->add_player_achieve(msg.player_achieves(i));
	}
	if (guild_member->zhiwu() != e_member_type_common)
	{
		dhc::guild_member_t *guild_member = 0;
		for (int i = 0; i < guild->member_guids_size(); ++i)
		{
			guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
			if (guild_member)
			{
				smsg.mutable_member()->add_player_guids(guild_member->player_guid());
				smsg.mutable_member()->add_player_names(guild_member->player_name());
				smsg.mutable_member()->add_player_level(guild_member->player_level());
				smsg.mutable_member()->add_player_template(guild_member->player_iocn_id());
				smsg.mutable_member()->add_player_bat_eff(guild_member->bf());
				smsg.mutable_member()->add_player_vip(guild_member->player_vip());
				smsg.mutable_member()->add_player_achieve(guild_member->player_achieve());
				smsg.mutable_member()->add_player_join_time(guild_member->join_time());
			}
		}
	}
	
	ResMessage::res_guild_pvp_look(player, smsg, name, id);
}

void GuildManager::terminal_guild_pvp_look_pipei_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id)
{
	rpcproto::tmsg_rep_guild_fight_info msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!msg.res())
	{
		PERROR(ERROR_GUILD_PVP_LOOK);
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_guild_fight_pvp_look smsg;
	smsg.set_stat(e_pvp_guild_look_pipei);
	smsg.mutable_fight()->set_guild_zhanji(msg.guild_zhanji());
	smsg.mutable_fight()->set_zhanji(msg.zhanji());
	smsg.mutable_fight()->set_total_zhanji(msg.total_zhanji());
	smsg.mutable_fight()->set_judian(msg.judian());
	smsg.mutable_fight()->set_jidi(msg.jidi());
	smsg.mutable_fight()->set_perfect(msg.perfect());
	for (int i = 0; i < guild_member->gongpo_rewards_size(); ++i)
	{
		smsg.mutable_fight()->add_reward_ids(guild_member->gongpo_rewards(i));
	}

	protocol::game::msg_guild_fight_info *fight = 0;
	for (int i = 0; i < msg.guild_fight_size(); ++i)
	{
		const rpcproto::tmsg_guild_fight_info &fight_info = msg.guild_fight(i);
		fight = smsg.mutable_fight()->add_fights();
		if (fight)
		{
			fight->set_guild(fight_info.guild());
			fight->set_guild_server(fight_info.guild_server());
			fight->set_guild_name(fight_info.guild_name());
			fight->set_guild_icon(fight_info.guild_icon());
			fight->set_guild_level(fight_info.guild_level());

			for (int j = 0; j < fight_info.target_guids_size(); ++j)
			{
				fight->add_target_guids(fight_info.target_guids(j));
				fight->add_target_names(fight_info.target_names(j));
				fight->add_target_templates(fight_info.target_templates(j));
				fight->add_target_levels(fight_info.target_levels(j));
				fight->add_target_bat_effs(fight_info.target_bat_effs(j));
				fight->add_target_vips(fight_info.target_vips(j));
				fight->add_target_achieves(fight_info.target_achieves(j));
				fight->add_target_defense_nums(fight_info.target_defense_nums(j));
			}

			for (int j = 0; j < fight_info.guard_points_size(); ++j)
			{
				fight->add_guard_points(fight_info.guard_points(j));
				fight->add_guard_gongpo(fight_info.guard_gongpo(j));
			}
		}
	}
	fight = smsg.mutable_fight()->add_fights();
	if (fight)
	{
		fight->set_guild(guild->pvp_guild());
		fight->set_guild_server(boost::lexical_cast<int>(player->serverid()));
		fight->set_guild_name(guild->name());
		fight->set_guild_icon(guild->icon());
		fight->set_guild_level(guild->level());
	}

	ResMessage::res_guild_pvp_look(player, smsg, name, id);
}

void GuildManager::terminal_guild_pvp_look_zhanji_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id)
{
	rpcproto::tmsg_rep_guild_jinrizhanji msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!msg.res())
	{
		PERROR(ERROR_GUILD_PVP_LOOK);
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_guild_fight_pvp_look  smsg;
	smsg.set_stat(e_pvp_guild_look_zhanji);

	smsg.mutable_zhanji()->set_guild_zhanji(msg.guild_zhanji());
	smsg.mutable_zhanji()->set_guild_total_zhanji(msg.guild_total_zhanji());
	smsg.mutable_zhanji()->set_guild_exp(msg.guild_exp());
	smsg.mutable_zhanji()->set_zhanji(msg.player_zhanji());
	smsg.mutable_zhanji()->set_total_zhanji(msg.player_total_zhanji());

	smsg.mutable_zhanji()->set_judian(msg.judian());
	smsg.mutable_zhanji()->set_jidi(msg.jidi());
	smsg.mutable_zhanji()->set_perfect(msg.perfect());

	smsg.mutable_fight()->set_judian(msg.judian());
	smsg.mutable_fight()->set_jidi(msg.jidi());
	smsg.mutable_fight()->set_perfect(msg.perfect());
	for (int i = 0; i < guild_member->gongpo_rewards_size(); ++i)
	{
		smsg.mutable_fight()->add_reward_ids(guild_member->gongpo_rewards(i));
	}

	const rpcproto::tmsg_rep_guild_look_bushu &bushu = msg.bushu();
	for (int i = 0; i < bushu.player_guids_size(); ++i)
	{
		smsg.mutable_zhanji()->mutable_bushu()->add_player_guids(bushu.player_guids(i));
		smsg.mutable_zhanji()->mutable_bushu()->add_player_names(bushu.player_names(i));
		smsg.mutable_zhanji()->mutable_bushu()->add_player_template(bushu.player_template(i));
		smsg.mutable_zhanji()->mutable_bushu()->add_player_level(bushu.player_level(i));
		smsg.mutable_zhanji()->mutable_bushu()->add_player_bat_eff(bushu.player_bat_eff(i));
		smsg.mutable_zhanji()->mutable_bushu()->add_player_vip(bushu.player_vips(i));
		smsg.mutable_zhanji()->mutable_bushu()->add_player_achieve(bushu.player_achieves(i));
	}

	if (guild_member->zhiwu() != e_member_type_common)
	{
		dhc::guild_member_t *guild_member = 0;
		for (int i = 0; i < guild->member_guids_size(); ++i)
		{
			guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
			if (guild_member)
			{
				smsg.mutable_member()->add_player_guids(guild_member->player_guid());
				smsg.mutable_member()->add_player_names(guild_member->player_name());
				smsg.mutable_member()->add_player_level(guild_member->player_level());
				smsg.mutable_member()->add_player_template(guild_member->player_iocn_id());
				smsg.mutable_member()->add_player_bat_eff(guild_member->bf());
				smsg.mutable_member()->add_player_vip(guild_member->player_vip());
				smsg.mutable_member()->add_player_achieve(guild_member->player_achieve());
				smsg.mutable_member()->add_player_join_time(guild_member->join_time());
			}
		}
	}
	
	ResMessage::res_guild_pvp_look(player, smsg, name, id);
}

void GuildManager::terminal_guild_pvp_fight_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id)
{
	rpcproto::tmsg_rep_guild_fight msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.res() < 0)
	{
		PERROR(ERROR_GUILD_PVP_LOOK);
		return;
	}

	if (msg.res() > 0)
	{
		PERROR(msg.res());
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}
	
	player->set_guild_pvp_num(player->guild_pvp_num() - 1);

	if (msg.exp() > 0)
	{
		GuildOperation::mod_guild_exp(guild, msg.exp());
	}
	if (msg.gongxian() > 0)
	{
		PlayerOperation::player_add_resource(player, resource::CONTRIBUTION, msg.gongxian(), LOGWAY_GUILD_PVP_FIGHT);
	}
	
	player->set_last_fight_time(game::timer()->now());

	protocol::game::smsg_guild_fight smsg;
	smsg.set_text(msg.text());
	smsg.set_result(msg.result());
	smsg.set_guard_point(msg.guard_point());
	smsg.set_gongxian(msg.gongxian());
	smsg.set_zhanji(msg.zhanji());
	smsg.set_judian(msg.judian());
	smsg.set_jidi(msg.jidi());
	smsg.set_perfect(msg.perfect());

	ResMessage::res_guild_pvp_fight(player, smsg, name, id);
}

void GuildManager::terminal_guild_pvp_target_callback(const std::string &data, uint64_t player_guid, int reward_id, const std::string &name, int id)
{
	rpcproto::tmsg_rep_guild_target_reward msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!msg.res())
	{
		PERROR(ERROR_GUILD_PVP_LOOK);
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}


	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < guild_member->gongpo_rewards_size(); ++i)
	{
		if (reward_id == guild_member->gongpo_rewards(i))
		{
			GLOBAL_ERROR;
			return;
		}
	}

	const s_t_guildfight_target* t_guildfight_target = sGuildConfig->get_guildfight_target(reward_id);
	if (!t_guildfight_target)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_guildfight_target->gongpuo_type == 1)
	{
		if (msg.judian() < t_guildfight_target->nums)
		{
			PERROR(ERROR_SPORT_REWARD);
			return;
		}
	}
	else if (t_guildfight_target->gongpuo_type == 2)
	{
		if (msg.jidi() < t_guildfight_target->nums)
		{
			PERROR(ERROR_SPORT_REWARD);
			return;
		}
	}
	else if (t_guildfight_target->gongpuo_type == 3)
	{
		if (msg.perfect() < t_guildfight_target->nums)
		{
			PERROR(ERROR_SPORT_REWARD);
			return;
		}
	}
	else
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	rds.add_reward(t_guildfight_target->type, t_guildfight_target->value1, t_guildfight_target->value2, t_guildfight_target->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_GUILD_PVP_GONGPO);

	guild_member->add_gongpo_rewards(reward_id);
	ResMessage::res_success(player, true, name, id);
}

void GuildManager::terminal_guild_pvp_xiuzhan_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id)
{
	rpcproto::tmsg_rep_guild_jinrizhanji msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!msg.res())
	{
		PERROR(ERROR_GUILD_PVP_LOOK);
		return;
	}

	dhc::player_t* player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
	if (!guild)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::guild_member_t *guild_member = GuildOperation::get_guild_member(guild, player->guid());
	if (!guild_member)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_guild_fight_pvp_look  smsg;
	smsg.set_stat(e_pvp_guild_xiuzhan);

	smsg.mutable_zhanji()->set_guild_zhanji(msg.guild_zhanji());
	smsg.mutable_zhanji()->set_guild_total_zhanji(msg.guild_total_zhanji());
	smsg.mutable_zhanji()->set_guild_exp(msg.guild_exp());
	smsg.mutable_zhanji()->set_zhanji(msg.player_zhanji());
	smsg.mutable_zhanji()->set_total_zhanji(msg.player_total_zhanji());

	smsg.mutable_fight()->set_judian(msg.judian());
	smsg.mutable_fight()->set_jidi(msg.jidi());
	smsg.mutable_fight()->set_perfect(msg.perfect());
	for (int i = 0; i < guild_member->gongpo_rewards_size(); ++i)
	{
		smsg.mutable_fight()->add_reward_ids(guild_member->gongpo_rewards(i));
	}

	ResMessage::res_guild_pvp_look(player, smsg, name, id);
}