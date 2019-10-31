#include "social_operation.h"
#include "gs_message.h"
#include "social_pool.h"
#include "social_config.h"
#include "post_operation.h"
#include "player_operation.h"

#define NEXT_TIME 4000

uint64_t last_time = 0;
std::list<protocol::game::smsg_chat *> msgs_;
std::map<uint64_t, std::list<protocol::game::smsg_chat *> > guild_msgs_;

void SocialOperation::gundong(const std::string &text)
{
	if (game::timer()->now() - last_time < NEXT_TIME)
	{
		return;
	}
	last_time = game::timer()->now();
	PushMessage::push_gundong(text, "chat1");
}

void SocialOperation::gundong_server(const std::string &text, const std::string &arg1, const std::string &arg2, const std::string &arg3)
{
	if (game::timer()->now() - last_time < NEXT_TIME)
	{
		return;
	}
	last_time = game::timer()->now();

	protocol::game::smsg_gundong_server msg;
	msg.set_text(text);
	if (arg1 !=  "")
	{
		msg.add_gundong_pars(arg1);
		if (arg2 != "")
		{
			msg.add_gundong_pars(arg2);
			if (arg3 != "" )
			{
				msg.add_gundong_pars(arg3);
			}
		}
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->push("chat1", PMSG_GUNDONG_SERVER, s);
}

void SocialOperation::gundong_ex(const std::string &text)
{
	PushMessage::push_gundong(text, "chat1");
}

void SocialOperation::check_gundong()
{
	std::vector<s_t_gundong> &t_gundong = sSocialConfig->get_gundong();
	for (int i = 0; i < t_gundong.size(); ++i)
	{
		bool flag = false;
		if (t_gundong[i].gtime <= t_gundong[i].start * 60)
		{
			flag = true;
		}
		t_gundong[i].gtime += 1;
		if (t_gundong[i].gtime > t_gundong[i].start * 60 && flag)
		{
			gundong_ex(t_gundong[i].text);
		}
		if (t_gundong[i].gtime > t_gundong[i].time * 60)
		{
			t_gundong[i].gtime -= t_gundong[i].time * 60;
		}
	}
}

void SocialOperation::refresh(dhc::player_t *player)
{
	if (!game::channel()->online(player->guid()))
	{
		return;
	}

	std::set<uint64_t> player_besocial;
	sSocialPool->get_player_besocial(player->guid(), player_besocial);
	if (player_besocial.size() > 0)
	{
		bool should_update = false;
		dhc::social_t *update_social = POOL_GET_SOCIAL(*(player_besocial.begin()));
		do 
		{
			if (!update_social)
			{
				break;
			}
			if (update_social->bf() != player->bf())
			{
				should_update = true;
				break;
			}
			if (update_social->level() != player->level())
			{
				should_update = true;
				break;
			}
			if (update_social->achieve() != player->dress_achieves_size())
			{
				should_update = true;
				break;
			}
			if (update_social->vip() != player->vip())
			{
				should_update = true;
				break;
			}
			if (update_social->template_id() != player->template_id())
			{
				should_update = true;
				break;
			}
			if (game::timer()->now() - update_social->offline_time() >= 240000)
			{
				should_update = true;
				break;
			}
			if (PlayerOperation::get_bingyuan_chenghao(player) != update_social->chengchao())
			{
				should_update = true;
				break;
			}
		} while (0);

		if (should_update)
		{
			uint64_t now_time = game::timer()->now();
			for (std::set<uint64_t>::iterator it = player_besocial.begin(); it != player_besocial.end(); ++it)
			{
				uint64_t social_guid = *it;
				dhc::social_t *social = POOL_GET_SOCIAL(social_guid);
				if (social)
				{
					social->set_name(player->name());
					social->set_template_id(player->template_id());
					social->set_level(player->level());
					social->set_bf(player->bf());
					social->set_vip(player->vip());
					social->set_achieve(player->dress_achieves_size());
					social->set_offline_time(now_time);
					social->set_chengchao(PlayerOperation::get_bingyuan_chenghao(player));
					sSocialPool->add_update(social_guid);
				}
			}
		}
	}
}

void SocialOperation::refresh_name_nalflag(dhc::player_t *player, const std::string& name ,const int& nalflag)
{
	std::vector<s_t_reward> rds;
	std::set<uint64_t> player_besocial;
	sSocialPool->get_player_besocial(player->guid(), player_besocial);
	for (std::set<uint64_t>::iterator it = player_besocial.begin(); it != player_besocial.end(); ++it)
	{
		std::string sender;
		std::string title;
		std::string text;
		uint64_t social_guid = *it;
		dhc::social_t *social = POOL_GET_SOCIAL(social_guid);
		if (social)
		{
			social->set_name(player->name());
			social->set_template_id(player->template_id());
			social->set_level(player->level());
			social->set_bf(player->bf());
			social->set_vip(player->vip());
			social->set_achieve(player->dress_achieves_size());
			if (nalflag != -1)
			{
				social->set_nalflag(nalflag);
			}
			sSocialPool->add_update(social_guid);
			if (nalflag != -1)
			{
				return;
			}
			int lang_ver = game::channel()->get_channel_lang(social->player_guid());
			game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
			game::scheme()->get_server_str(lang_ver, title, "change_name_social_title");
			game::scheme()->get_server_str(lang_ver, text, "change_name_social_text", name.c_str(), player->name().c_str());
			PostOperation::post_create(social->player_guid(), title, text, sender, rds);
		}
	}
}

dhc::social_t * SocialOperation::create_social(uint64_t player_guid, uint64_t target_guid, const std::string &name, int id, int level, int bf, int vip, int achieve, int nalflag)
{
	uint64_t social_guid = game::gtool()->assign(et_social);
	dhc::social_t *social = new dhc::social_t();
	social->set_guid(social_guid);
	social->set_player_guid(player_guid);
	social->set_target_guid(target_guid);
	social->set_name(name);
	social->set_template_id(id);
	social->set_level(level);
	social->set_bf(bf);
	social->set_vip(vip);
	social->set_achieve(achieve);
	social->set_nalflag(nalflag);
	social->set_offline_time(game::timer()->now());
	POOL_ADD_NEW(social->guid(), social);

	return social;
}

int SocialOperation::get_is_friend_apply(dhc::player_t *player)
{
	std::set<protocol::game::msg_social_player *> player_beapply;
	sSocialPool->get_player_beapply(player->guid(), player_beapply);

	int flag = 0;
	if (player_beapply.size() > 0)
	{
		flag += 1;
	}

	if (player->level() >= 30)
	{
		dhc::social_t *socialCode = sSocialPool->get_social_code(player);
		if (socialCode)
		{
			const std::map<int, std::vector<std::pair<int, int> > >& allactive = sSocialConfig->get_social_active();
			for (std::map<int, std::vector<std::pair<int, int> > >::const_iterator it = allactive.begin();
				it != allactive.end();
				++it)
			{
				if (socialCode->invite_players_size() >= it->first)
				{
					const std::vector<std::pair<int, int> >& allids = it->second;
					for (int km = 0; km < allids.size(); ++km)
					{
						bool full = true;

						const int& klevel = allids[km].second;
						for (int kj = 0; kj < socialCode->invite_levels_size(); ++kj)
						{
							if (socialCode->invite_levels(kj) < klevel)
							{
								full = false;
								break;
							}
						}
						if (!full)
						{
							break;
						}
						
						full = false;
						const int& kid = allids[km].first;
						for (int kj = 0; kj < socialCode->invite_ids_size(); ++kj)
						{
							if (socialCode->invite_ids(kj) == kid)
							{
								full = true;
								break;
							}
						}
						if (!full)
						{
							flag += 2;
							return flag;
						}
					}
				}
			}
		}
	}

	return flag;
}

int SocialOperation::get_is_friend_tili(dhc::player_t *player)
{
	if (player->social_shou_num() >= MAX_SHOU_NUM)
	{
		return 0;
	}
	std::set<uint64_t> player_social;
	sSocialPool->get_player_social(player->guid(), player_social);
	for (std::set<uint64_t>::iterator it = player_social.begin(); it != player_social.end(); ++it)
	{
		dhc::social_t *social = POOL_GET_SOCIAL(*it);
		if (social && social->can_shou())
		{
			return 1;
		}
	}
	return 0;
}

void SocialOperation::add_chat(protocol::game::smsg_chat *smsg)
{
	msgs_.push_back(smsg);
	if (msgs_.size() > 10)
	{
		protocol::game::smsg_chat *smsg1 = msgs_.front();
		delete smsg1;
		msgs_.pop_front();
	}
}

void SocialOperation::get_chat(std::list<protocol::game::smsg_chat *> &smsg)
{
	smsg = msgs_;
}

void SocialOperation::add_chat(uint64_t guild_guid, protocol::game::smsg_chat *smsg)
{
	std::list<protocol::game::smsg_chat *> &msgs = guild_msgs_[guild_guid];
	msgs.push_back(smsg);
	if (msgs.size() > 10)
	{
		protocol::game::smsg_chat *smsg1 = msgs.front();
		delete smsg1;
		msgs.pop_front();
	}
}

void SocialOperation::get_chat(uint64_t guild_guid, std::list<protocol::game::smsg_chat *> &smsg)
{
	if (guild_msgs_.find(guild_guid) != guild_msgs_.end())
	{
		smsg = guild_msgs_[guild_guid];
	}
}

void SocialOperation::get_friends(uint64_t player_guid, std::set<uint64_t>& friends)
{
	const std::set<uint64_t> *friends_list = sSocialPool->get_friends(player_guid);
	if (friends_list)
	{
		friends.insert(friends_list->begin(), friends_list->end());
	}
}

void SocialOperation::update_social_code(dhc::player_t *player, int level, bool check /* = false */)
{
	sSocialPool->update_social_code(player, level, check);
}

int SocialOperation::get_social_code_pcount(dhc::player_t *player, int level)
{
	dhc::social_t *social_code = sSocialPool->get_social_code(player);
	if (!social_code)
	{
		return 0;
	}

	int count = 0;
	for (int i = 0; i < social_code->invite_levels_size(); ++i)
	{
		if (social_code->invite_levels(i) >= level)
		{
			count++;
		}
	}
	
	return count;
}

void SocialOperation::add_social_code_id(dhc::player_t *player, int id)
{
	dhc::social_t *social_code = sSocialPool->get_social_code(player);
	if (!social_code)
	{
		return;
	}
	social_code->add_invite_ids(id);
	sSocialPool->add_update(social_code->guid());
}
