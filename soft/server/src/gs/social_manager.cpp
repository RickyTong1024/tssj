#include "social_manager.h"
#include "gs_message.h"
#include "social_pool.h"
#include "utils.h"
#include "social_operation.h"
#include "player_operation.h"
#include "player_pool.h"
#include "social_config.h"
#include "rpc.pb.h"
#include "player_config.h"


#define SOCIAL_PERIOD 1000
#define SOCIAL_MAX_ADD 40
#define SOCIAL_MAX_APPLY 40
#define SOCIAL_LEVEL 20



SocialManager::SocialManager()
: timer_(-1)
{
	
}

SocialManager::~SocialManager()
{

}

int SocialManager::init()
{
	if (-1 == sSocialConfig->parse())
	{
		return -1;
	}
	sSocialPool->social_list_load();
	timer_ = game::timer()->schedule(boost::bind(&SocialManager::update, this, _1), SOCIAL_PERIOD, "social");
	return 0;
}

int SocialManager::fini()
{
	sSocialPool->save_all();
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = 0;
	}
	return 0;
}

int SocialManager::update(const ACE_Time_Value &curr)
{
	sSocialPool->update();
	//SocialOperation::check_gundong();
	return 0;
}

std::wstring utf8_to_utf16(const std::string& utf8)
{
	std::vector<unsigned long> unicode;
	size_t i = 0;
	while (i < utf8.size())
	{
		unsigned long uni;
		size_t todo;
		bool error = false;
		unsigned char ch = utf8[i++];
		if (ch <= 0x7F)
		{
			uni = ch;
			todo = 0;
		}
		else if (ch <= 0xBF)
		{
			throw std::logic_error("not a UTF-8 string");
		}
		else if (ch <= 0xDF)
		{
			uni = ch & 0x1F;
			todo = 1;
		}
		else if (ch <= 0xEF)
		{
			uni = ch & 0x0F;
			todo = 2;
		}
		else if (ch <= 0xF7)
		{
			uni = ch & 0x07;
			todo = 3;
		}
		else
		{
			throw std::logic_error("not a UTF-8 string");
		}
		for (size_t j = 0; j < todo; ++j)
		{
			if (i == utf8.size())
				throw std::logic_error("not a UTF-8 string");
			unsigned char ch = utf8[i++];
			if (ch < 0x80 || ch > 0xBF)
				throw std::logic_error("not a UTF-8 string");
			uni <<= 6;
			uni += ch & 0x3F;
		}
		if (uni >= 0xD800 && uni <= 0xDFFF)
			throw std::logic_error("not a UTF-8 string");
		if (uni > 0x10FFFF)
			throw std::logic_error("not a UTF-8 string");
		unicode.push_back(uni);
	}
	std::wstring utf16;
	for (size_t i = 0; i < unicode.size(); ++i)
	{
		unsigned long uni = unicode[i];
		if (uni <= 0xFFFF)
		{
			utf16 += (wchar_t)uni;
		}
		else
		{
			uni -= 0x10000;
			utf16 += (wchar_t)((uni >> 10) + 0xD800);
			utf16 += (wchar_t)((uni & 0x3FF) + 0xDC00);
		}
	}
	return utf16;
}

void SocialManager::terminal_chat(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_chat_ex msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	uint64_t time_now = game::timer()->now();
	for (std::list<uint64_t>::iterator iter = chat_time_list_.begin(); iter != chat_time_list_.end();)
	{
		uint64_t guid = *iter;
		std::map<uint64_t, uint64_t>::iterator jt = chat_time_map_.find(guid);
		if (jt == chat_time_map_.end())
		{
			chat_time_list_.erase(iter++);
		}
		else if (time_now >= (*jt).second + gCONST(CONST_FYJG))
		{
			chat_time_map_.erase(guid);
			chat_time_list_.erase(iter++);
		}
		else
		{
			break;
		}
	}

	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	PCK_CHECK_EX 

	if (player->level() < 20)
	{
		PERROR(ERROR_LEVEL);
		return;
	}

	if (chat_time_map_.find(player_guid) != chat_time_map_.end())
	{
		if (time_now < chat_time_map_[player_guid] + gCONST(CONST_FYJG))
		{
			PERROR(ERROR_CHAT_TIME);
			return;
		}
	}

	if (msg.type() < 0 || msg.type() > 2)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.text().length() > 128)
	{
		PERROR(ERROR_CHAT_TEXT_LENGTH);
		return;
	}

	chat_time_map_[player_guid] = time_now;
	chat_time_list_.push_back(player_guid);

	int is_danmu = 0;
	if (player->vip() >= 2 && msg.type() == 0)
	{
		is_danmu = 1;
	}

	std::string text = msg.text();
	game::scheme()->change_illword(text);

	std::vector<uint64_t> guids;
	uint64_t target_guid = 0;
	std::string target_name = "";
	if (msg.type() == 0)
	{
		protocol::game::smsg_chat *smsg = new protocol::game::smsg_chat();
		smsg->set_player_guid(player->guid());
		smsg->set_player_name(player->name());
		smsg->set_player_template(player->template_id());
		smsg->set_level(player->level());
		smsg->set_vip(player->vip());
		smsg->set_type(msg.type());
		smsg->set_color(msg.color());
		smsg->set_text(text);
		smsg->set_is_danmu(is_danmu);
		smsg->set_time(game::timer()->now());
		smsg->set_target_guid(target_guid);
		smsg->set_target_name(target_name);
		smsg->set_nalflag(player->nalflag());

		SocialOperation::add_chat(smsg);
	}
	else if (msg.type() == 1)
	{
		if (player->guild() == 0)
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
		for (int i = 0; i < guild->member_guids_size(); ++i)
		{
			dhc::guild_member_t *guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
			if (!guild_member)
			{
				continue;
			}
			dhc::player_t * target = POOL_GET_PLAYER(guild_member->player_guid());
			if (!target)
			{
				continue;
			}
			guids.push_back(target->guid());
		}
		protocol::game::smsg_chat *smsg = new protocol::game::smsg_chat();
		smsg->set_player_guid(player->guid());
		smsg->set_player_name(player->name());
		smsg->set_player_template(player->template_id());
		smsg->set_level(player->level());
		smsg->set_vip(player->vip());
		smsg->set_type(msg.type());
		smsg->set_color(msg.color());
		smsg->set_text(text);
		smsg->set_is_danmu(is_danmu);
		smsg->set_time(game::timer()->now());
		smsg->set_target_guid(target_guid);
		smsg->set_target_name(target_name);
		smsg->set_nalflag(player->nalflag());

		SocialOperation::add_chat(guild->guid(), smsg);
	}
	else if(msg.type() == 2)
	{
		if (msg.target_name() == player->name())
		{
			GLOBAL_ERROR;
			return;
		}
		uint64_t target_guid = sPlayerPool->get_name_player(msg.target_name());
		dhc::player_t * target = POOL_GET_PLAYER(target_guid);
		if (!target)
		{
			PERROR(ERROR_CHAT_NOT_ONLINE);
			return;
		}
		target_guid = target->guid();
		target_name = target->name();
		guids.push_back(player->guid());
		guids.push_back(target->guid());
	}
	
	PushMessage::push_chat(player, msg.type(), msg.color(), text, is_danmu, target_guid, target_name, guids, "chat1");
	ResMessage::res_chat(name, id);
}

void SocialManager::terminal_social_look(const std::string &data, const std::string &name, int id)
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

	std::set<uint64_t> player_social;
	sSocialPool->get_player_social(player->guid(), player_social);
	std::vector<dhc::social_t *> socials;
	for (std::set<uint64_t>::iterator it = player_social.begin(); it != player_social.end(); ++it)
	{
		dhc::social_t *social = POOL_GET_SOCIAL(*it);
		if (social)
		{
			socials.push_back(social);
		}
	}
	ResMessage::res_social_look(player, socials, name, id);
}

void SocialManager::terminal_social_rand(const std::string &data, const std::string &name, int id)
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

	

	std::set<uint64_t> player_social;
	sSocialPool->get_player_social(player->guid(), player_social);

	std::set< std::pair<uint64_t, uint64_t> > player_apply;
	sSocialPool->get_player_apply(player->guid(), player_apply);

	std::vector<uint64_t> player_guids;
	game::pool()->get_entitys(et_player, player_guids);

	std::vector<uint64_t> player_guids1;
	for (int i = 0; i < player_guids.size(); ++i)
	{
		dhc::player_t *target = POOL_GET_PLAYER(player_guids[i]);
		if (!target)
		{
			continue;
		}
		if (target->level() < SOCIAL_LEVEL)
		{
			continue;
		}
		if (target->guid() == player->guid())
		{
			continue;
		}
		bool flag = false;
		for (std::set<uint64_t>::iterator jt = player_social.begin(); jt != player_social.end(); ++jt)
		{
			dhc::social_t *social = POOL_GET_SOCIAL(*jt);
			if (social)
			{
				if (social->target_guid() == target->guid())
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			continue;
		}
		for (std::set< std::pair<uint64_t, uint64_t> >::iterator it = player_apply.begin(); it != player_apply.end(); ++it)
		{
			if ((*it).first == target->guid())
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			continue;
		}
		player_guids1.push_back(player_guids[i]);
	}
	std::vector<uint64_t> player_guids2;
	if (player_guids1.size() <= 4)
	{
		player_guids2 = player_guids1;
	}
	else
	{
		Utils::get_vector(player_guids1, 4, player_guids2);
	}
	std::vector<dhc::player_t *> players;
	for (int i = 0; i < player_guids2.size(); ++i)
	{
		dhc::player_t *tplayer = POOL_GET_PLAYER(player_guids2[i]);
		if (tplayer)
		{
			players.push_back(tplayer);
		}
	}
	ResMessage::res_social_rand(player, players, name, id);
}

void SocialManager::terminal_social_add(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_social_add msg;
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

	uint64_t target_guid = msg.player_guid();

	if (!IS_PLAYER_GUID(target_guid))
	{
		GLOBAL_ERROR;
		return;
	}

	if (target_guid == player->guid())
	{
		GLOBAL_ERROR;
		return;
	}

	std::set<uint64_t> player_social;
	sSocialPool->get_player_social(player->guid(), player_social);
	if (player_social.size() >= SOCIAL_MAX_ADD)
	{
		PERROR(ERROR_SOCIAL_MAX_ADD);
		return;
	}

	std::set< std::pair<uint64_t, uint64_t> > player_apply;
	sSocialPool->get_player_apply(player->guid(), player_apply);
	if (player_social.size() > SOCIAL_MAX_APPLY)
	{
		PERROR(ERROR_SOCIAL_MAX_APPLY);
		return;
	}

	bool flag = false;
	for (std::set<uint64_t>::iterator it = player_social.begin(); it != player_social.end(); ++it)
	{
		dhc::social_t *social = POOL_GET_SOCIAL(*it);
		if (social)
		{
			if (social->target_guid() == target_guid)
			{
				flag = true;
				break;
			}
		}
	}
	if (flag)
	{
		PERROR(ERROR_SOCIAL_HAS_ADD);
		return;
	}
	for (std::set< std::pair<uint64_t, uint64_t> >::iterator it = player_apply.begin(); it != player_apply.end(); ++it)
	{
		if ((*it).first == target_guid)
		{
			flag = true;
			break;
		}
	}
	if (flag)
	{
		PERROR(ERROR_SOCIAL_HAS_APPLY);
		return;
	}
	sSocialPool->add_apply(player, target_guid);

	ResMessage::res_social_add(player, target_guid, name, id);
}

void SocialManager::terminal_social_look_new(const std::string &data, const std::string &name, int id)
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

	std::set<protocol::game::msg_social_player *> player_beapply;
	sSocialPool->get_player_beapply(player->guid(), player_beapply);

	std::vector<protocol::game::msg_social_player *> msps;
	for (std::set<protocol::game::msg_social_player *>::iterator jt = player_beapply.begin(); jt != player_beapply.end(); ++jt)
	{
		protocol::game::msg_social_player *sp = *jt;
		msps.push_back(sp);
	}

	ResMessage::res_social_look_new(player, msps, name, id);
}

void SocialManager::terminal_social_agree(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_social_agree msg;
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

	uint64_t target_guid = msg.player_guid();

	std::set<protocol::game::msg_social_player *> player_beapply;
	sSocialPool->get_player_beapply(player->guid(), player_beapply);

	bool flag = false;
	protocol::game::msg_social_player *msp;
	for (std::set<protocol::game::msg_social_player *>::iterator jt = player_beapply.begin(); jt != player_beapply.end(); ++jt)
	{
		protocol::game::msg_social_player *sp = *jt;
		if (sp->player_guid() == target_guid)
		{
			msp = sp;
			flag = true;
			break;
		}
	}

	if (!flag)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.agree() == 1)
	{
		std::set<uint64_t> player_social;
		sSocialPool->get_player_social(player->guid(), player_social);
		if (player_social.size() >= SOCIAL_MAX_ADD)
		{
			PERROR(ERROR_SOCIAL_MAX_ADD);
			return;
		}

		std::set<uint64_t> target_social;
		sSocialPool->get_player_social(target_guid, target_social);
		if (target_social.size() >= SOCIAL_MAX_ADD)
		{
			PERROR(ERROR_SOCIAL_MAX_ADD_TARGET);
			return;
		}

		bool flag = false;
		for (std::set<uint64_t>::iterator it = player_social.begin(); it != player_social.end(); ++it)
		{
			dhc::social_t *social = POOL_GET_SOCIAL(*it);
			if (social)
			{
				if (social->target_guid() == target_guid)
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			PERROR(ERROR_SOCIAL_HAS_ADD);
			return;
		}

		dhc::social_t *social = SocialOperation::create_social(player->guid(), msp->player_guid(), msp->player_name(), msp->player_template(), msp->player_level(), msp->player_bf(), msp->player_vip(), msp->player_achieve(), msp->nalflag());
		sSocialPool->add_social(social, true);

		social = SocialOperation::create_social(msp->player_guid(), player->guid(), player->name(), player->template_id(), player->level(), player->bf(), player->vip(), player->dress_achieves_size(), player->nalflag());
		sSocialPool->add_social(social, true);

		sSocialPool->delete_apply(target_guid, player->guid());
	}
	else
	{
		sSocialPool->delete_apply(target_guid, player->guid());
	}

	ResMessage::res_social_agree(player, target_guid, msg.agree(), name, id);
}

void SocialManager::terminal_social_delete(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_social_delete msg;
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

	uint64_t social_guid = msg.social_guid();
	dhc::social_t *social = POOL_GET_SOCIAL(social_guid);
	if (!social)
	{
		GLOBAL_ERROR;
		return;
	}
	if (social->player_guid() != player->guid())
	{
		GLOBAL_ERROR;
		return;
	}
	if (social->template_id() == -1)
	{
		GLOBAL_ERROR;
		return;
	}
	sSocialPool->delete_social(social->player_guid(), social->target_guid());

	ResMessage::res_social_delete(player, social_guid, name, id);
}

void SocialManager::terminal_social_song(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_social_song msg;
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

	dhc::social_t *social = POOL_GET_SOCIAL(msg.social_guid());
	if (!social)
	{
		GLOBAL_ERROR;
		return;
	}
	if (social->player_guid() != player->guid())
	{
		GLOBAL_ERROR;
		return;
	}
	if (!game::timer()->trigger_time(social->last_song_time(), 0, 0))
	{
		GLOBAL_ERROR;
		return;
	}
	social->set_last_song_time(game::timer()->now());
	sSocialPool->add_update(social->guid());

	dhc::social_t *asocial = sSocialPool->get_another(social);
	if (asocial)
	{
		asocial->set_can_shou(1);
		sSocialPool->add_update(asocial->guid());
	}
	PlayerOperation::player_add_active(player, 2100, 1);

	ResMessage::res_social_song(player, social->guid(), name, id);
}

void SocialManager::terminal_social_shou(const std::string &data, const std::string &name, int id)
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

	std::set<uint64_t> player_social;
	sSocialPool->get_player_social(player->guid(), player_social);

	std::vector<uint64_t> social_guids;
	for (std::set<uint64_t>::iterator it = player_social.begin(); it != player_social.end(); ++it)
	{
		dhc::social_t *social = POOL_GET_SOCIAL(*it);
		if (social && social->can_shou())
		{
			if (player->social_shou_num() < MAX_SHOU_NUM)
			{
				player->set_social_shou_num(player->social_shou_num() + 1);
				PlayerOperation::player_add_resource(player, resource::YOUQINGDIAN, 1, LOGWAY_SOCIAL_SHOU);
				social->set_can_shou(0);
				sSocialPool->add_update(social->guid());
				social_guids.push_back(social->guid());
			}
			else
			{
				break;
			}
		}
	}

	ResMessage::res_social_shou(player, social_guids, name, id);
}

void SocialManager::terminal_social_invite_look(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_team_friend_view smsg;
	std::set<uint64_t> player_social;
	sSocialPool->get_player_social(player->guid(), player_social);
	
	dhc::player_t *target = 0;
	dhc::social_t *social = 0;
	dhc::guild_t *guild = 0;
	for (std::set<uint64_t>::iterator it = player_social.begin(); it != player_social.end(); ++it)
	{
		social = POOL_GET_SOCIAL(*it);
		if (social &&
			game::channel()->online(social->target_guid()))
		{
			target = POOL_GET_PLAYER(social->target_guid());
			if (target && target->level() >= 65)
			{
				smsg.add_guid(target->guid());
				smsg.add_name(target->name());
				smsg.add_id(target->template_id());
				if (social->chengchao())
				{
					smsg.add_chenghao(social->chengchao());
				}
				else
				{
					smsg.add_chenghao(7);
				}
				smsg.add_bf(target->bf());
				if (target->guild() > 0)
				{
					guild = POOL_GET_GUILD(target->guild());
					if (guild)
					{
						smsg.add_guild(guild->name());
					}
					else
					{
						smsg.add_guild("");
					}
				}
				else
				{
					smsg.add_guild("");
				}
				smsg.add_level(target->level());
				smsg.add_vip(target->vip());
				smsg.add_achieve(target->dress_achieves_size());
				smsg.add_nalflag(target->nalflag());
			}
		}
	}
	ResMessage::res_social_invite_look(player, smsg, name, id);
}

void SocialManager::terminal_social_code_look(const std::string &data, const std::string &name, int id)
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


	rpcproto::tmsg_req_invite_code_input rmsg;
	rmsg.set_player_guid(player->guid());
	rmsg.set_player_level(player->level());
	rmsg.set_code("");
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_SOCIAL_INVITE_CODE_PULL, s,
		boost::bind(&SocialManager::terminal_social_code_look_callback, this, _1, player->guid(), name, id));
}

void SocialManager::terminal_social_code_look_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id)
{
	rpcproto::tmsg_rep_invite_level msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}

	std::vector<dhc::social_t *> socials;
	dhc::social_t *social_code = sSocialPool->get_social_code(player);
	if (social_code)
	{
		socials.push_back(social_code);
	}
	

	if (msg.res() && social_code)
	{
		social_code->clear_invite_players();
		social_code->clear_invite_levels();

		for (int i = 0; i < msg.player_guids_size(); ++i)
		{
			social_code->add_invite_players(msg.player_guids(i));
			social_code->add_invite_levels(msg.levels(i));
		}
		sSocialPool->add_update(social_code->guid());
	}

	ResMessage::res_social_look(player, socials, name, id);
}
