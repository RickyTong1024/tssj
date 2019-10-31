#include "social_pool.h"
#include "utils.h"
#include "post_operation.h"
#include "rpc.pb.h"

SocialPool::SocialPool()
{

}

SocialPool::~SocialPool()
{

}

void SocialPool::social_list_load()
{
	uint64_t social_list_guid = MAKE_GUID(et_social_list, 0);
	Request *req = new Request();
	req->add(opc_query, social_list_guid, new protocol::game::social_list_t);
	game::pool()->upcall(req, boost::bind(&SocialPool::social_list_load_callback, this, _1));
}

int SocialPool::social_list_load_callback(Request *req)
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
				if (social->template_id() != -1) 
				{
					add_social(social);
				}
				else
				{
					add_social_code(social);
				}
			}
		}
	}
	return 0;
}

void SocialPool::add_update(uint64_t social_guid)
{
	if (update_set_.find(social_guid) == update_set_.end())
	{
		update_list_.push_back(social_guid);
		update_set_.insert(social_guid);
	}
	
}

void SocialPool::save_all()
{
	for (std::list<uint64_t>::iterator it = update_list_.begin(); it != update_list_.end(); ++it)
	{
		uint64_t guid = *it;
		dhc::social_t *social = POOL_GET_SOCIAL(guid);
		if (social)
		{
			POOL_SAVE(dhc::social_t, social, false);
		}
	}
	update_list_.clear();
}

void SocialPool::update()
{
	int save_num = 0;
	while (save_num < 100)
	{
		if (update_list_.empty())
		{
			break;
		}
		uint64_t social_guid = update_list_.front();
		dhc::social_t *social = POOL_GET_SOCIAL(social_guid);
		if (social)
		{
			POOL_SAVE(dhc::social_t, social, false);
			save_num++;
			update_list_.pop_front();
		}
		else
		{
			update_list_.pop_front();
		}
		update_set_.erase(social_guid);
	}

	int check_num = 0;
	int tnum = 20;
	if (apply_update_list_.size() < tnum)
	{
		tnum = apply_update_list_.size();
	}
	while (check_num < tnum)
	{
		uint64_t player_guid = apply_update_list_.front();
		apply_update_list_.pop_front();
		if (player_apply_list_.find(player_guid) != player_apply_list_.end())
		{
			std::vector<uint64_t> target_guids;
			std::set< std::pair<uint64_t, uint64_t> > &sp = player_apply_list_[player_guid];
			for (std::set< std::pair<uint64_t, uint64_t> >::iterator jt = sp.begin(); jt != sp.end(); ++jt)
			{
				if ((*jt).second + 36000000 < game::timer()->now())
				{
					target_guids.push_back((*jt).first);
				}
			}
			for (int i = 0; i < target_guids.size(); ++i)
			{
				delete_apply(player_guid, target_guids[i]);
			}
			if (player_apply_list_.find(player_guid) != player_apply_list_.end())
			{
				apply_update_list_.push_back(player_guid);
			}
			check_num++;
		}
	}
}

void SocialPool::add_social(dhc::social_t *social, bool is_new)
{
	player_social_list_[social->player_guid()].insert(social->guid());
	player_besocial_list_[social->target_guid()].insert(social->guid());
	if (is_new)
	{
		add_update(social->guid());
	}
	friends_[social->player_guid()].insert(social->target_guid());
	friends_[social->target_guid()].insert(social->player_guid());
}

void SocialPool::delete_social(uint64_t player_guid, uint64_t target_guid)
{
	if (friends_.find(player_guid) != friends_.end())
	{
		friends_[player_guid].erase(target_guid);
	}
	if (friends_.find(target_guid) != friends_.end())
	{
		friends_[target_guid].erase(player_guid);
	}
	if (player_social_list_.find(player_guid) != player_social_list_.end())
	{
		std::set<uint64_t> &socials = player_social_list_[player_guid];
		for (std::set<uint64_t>::iterator it = socials.begin(); it != socials.end();)
		{
			uint64_t social_guid = *it;
			dhc::social_t *social = POOL_GET_SOCIAL(social_guid);
			if (social)
			{
				if (social->target_guid() == target_guid)
				{
					player_besocial_list_[social->target_guid()].erase(social->guid());
					socials.erase(it++);
					POOL_REMOVE(social->guid(), 0);
					continue;
				}
			}
			++it;
		}
	}

	if (player_social_list_.find(target_guid) != player_social_list_.end())
	{
		std::set<uint64_t> &socials = player_social_list_[target_guid];
		for (std::set<uint64_t>::iterator it = socials.begin(); it != socials.end();)
		{
			uint64_t social_guid = *it;
			dhc::social_t *social = POOL_GET_SOCIAL(social_guid);
			if (social)
			{
				if (social->target_guid() == player_guid)
				{
					player_besocial_list_[social->target_guid()].erase(social->guid());
					socials.erase(it++);
					POOL_REMOVE(social->guid(), 0);
					continue;
				}
			}
			++it;
		}
	}
}

void SocialPool::get_player_social(uint64_t player_guid, std::set<uint64_t> &player_social)
{
	if (player_social_list_.find(player_guid) != player_social_list_.end())
	{
		player_social = player_social_list_[player_guid];
	}
}

void SocialPool::get_player_besocial(uint64_t player_guid, std::set<uint64_t> &player_besocial)
{
	if (player_besocial_list_.find(player_guid) != player_besocial_list_.end())
	{
		player_besocial = player_besocial_list_[player_guid];
	}
}

dhc::social_t * SocialPool::get_another(dhc::social_t *social)
{
	if (player_social_list_.find(social->target_guid()) != player_social_list_.end())
	{
		std::set<uint64_t> &socials = player_social_list_[social->target_guid()];
		for (std::set<uint64_t>::iterator it = socials.begin(); it != socials.end(); ++it)
		{
			uint64_t social_guid = *it;
			dhc::social_t *tsocial = POOL_GET_SOCIAL(social_guid);
			if (tsocial)
			{
				if (social->player_guid() == tsocial->target_guid())
				{
					return tsocial;
				}
			}
		}
	}
	return 0;
}

void SocialPool::add_apply(dhc::player_t *player, uint64_t target_guid)
{
	if (player_apply_list_.find(player->guid()) == player_apply_list_.end())
	{
		apply_update_list_.push_back(player->guid());
	}
	player_apply_list_[player->guid()].insert(std::pair<uint64_t, uint64_t>(target_guid, game::timer()->now()));

	protocol::game::msg_social_player *sp = new protocol::game::msg_social_player;
	sp->set_player_guid(player->guid());
	sp->set_player_name(player->name());
	sp->set_player_template(player->template_id());
	sp->set_player_level(player->level());
	sp->set_player_bf(player->bf());
	sp->set_player_vip(player->vip());
	sp->set_player_achieve(player->dress_achieves_size());
	sp->set_nalflag(player->nalflag());
	if (game::channel()->online(player->guid()))
	{
		sp->set_offline_time(game::timer()->now());
	}
	else
	{
		sp->set_offline_time(player->last_check_time());
	}
	player_beapply_list_[target_guid].insert(sp);
}

void SocialPool::delete_apply(uint64_t player_guid, uint64_t target_guid)
{
	if (player_apply_list_.find(player_guid) != player_apply_list_.end())
	{
		std::set< std::pair<uint64_t, uint64_t> > &sp = player_apply_list_[player_guid];
		for (std::set< std::pair<uint64_t, uint64_t> >::iterator jt = sp.begin(); jt != sp.end(); ++jt)
		{
			if ((*jt).first == target_guid)
			{
				sp.erase(jt);
				break;
			}
		}
		if (player_apply_list_.empty())
		{
			player_apply_list_.erase(player_guid);
		}
	}
	if (player_beapply_list_.find(target_guid) != player_beapply_list_.end())
	{
		std::set<protocol::game::msg_social_player *> &sp_set = player_beapply_list_[target_guid];
		for (std::set<protocol::game::msg_social_player *>::iterator jt = sp_set.begin(); jt != sp_set.end(); ++jt)
		{
			protocol::game::msg_social_player *sp = *jt;
			if (sp->player_guid() == player_guid)
			{
				delete sp;
				sp_set.erase(jt);
				break;
			}
		}
		if (player_beapply_list_[target_guid].empty())
		{
			player_beapply_list_.erase(target_guid);
		}
	}
}

void SocialPool::get_player_apply(uint64_t player_guid, std::set< std::pair<uint64_t, uint64_t> > &player_apply)
{
	if (player_apply_list_.find(player_guid) != player_apply_list_.end())
	{
		player_apply = player_apply_list_[player_guid];
	}
}

void SocialPool::get_player_beapply(uint64_t player_guid, std::set<protocol::game::msg_social_player *> &player_beapply)
{
	if (player_beapply_list_.find(player_guid) != player_beapply_list_.end())
	{
		player_beapply = player_beapply_list_[player_guid];
	}
}

const std::set<uint64_t>* SocialPool::get_friends(uint64_t player_guid) const
{
	std::map<uint64_t, std::set<uint64_t> >::const_iterator it = friends_.find(player_guid);
	if (it != friends_.end())
	{
		return &(it->second);
	}
	return 0;
}

void SocialPool::update_social_code(dhc::player_t *player, int level, bool check)
{
	// 创建code
	if (level == 30 || (check && player->level() >= 30))
	{
		// 没有code
		if (player_social_code_invites_.find(player->guid()) == player_social_code_invites_.end())
		{
			std::string s;
			do
			{
				s = player->serverid();
				for (int i = 0; i < 6; ++i)
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

				if (name_social_code_invites_.find(s) == name_social_code_invites_.end())
				{
					break;
				}
			} while (true);

			dhc::social_t *social = new dhc::social_t();
			social->set_guid(game::gtool()->assign(et_social));
			social->set_player_guid(player->guid());
			social->set_target_guid(0);
			social->set_name(s);
			social->set_template_id(-1);
			social->set_offline_time(game::timer()->now());
			POOL_ADD_NEW(social->guid(), social);
			add_update(social->guid());

			player_social_code_invites_[player->guid()] = social->guid();
			name_social_code_invites_[s] = social->guid();

			update_social_code(player, social);
		}
		else
		{
			dhc::social_t *social = POOL_GET_SOCIAL(player_social_code_invites_[player->guid()]);
			if (social && social->can_shou() == 0)
			{
				update_social_code(player, social);
			}
		}
	}

	// 更新邀请人等级
	if (level >= 30)
	{
		std::map<uint64_t, uint64_t>::iterator  it = player_social_code_invites_.find(player->guid());
		if (it != player_social_code_invites_.end())
		{
			dhc::social_t *social = POOL_GET_SOCIAL(it->second);
			if (social)
			{
				if (social->level() != level)
				{
					social->set_level(level);
					add_update(social->guid());
					rpcproto::tmsg_req_invite_code_input rmsg;
					rmsg.set_player_guid(player->guid());
					rmsg.set_player_level(level);
					rmsg.set_code("");
					std::string s;
					rmsg.SerializeToString(&s);
					game::rpc_service()->push("remote1", PMSG_SOCIAL_INVITE_CODE_LEVEL, s);
				}
			}
		}
		
	}
}

void SocialPool::add_social_code(dhc::social_t *social)
{
	player_social_code_invites_[social->player_guid()] = social->guid();
	name_social_code_invites_[social->name()] = social->guid();
}

dhc::social_t* SocialPool::get_social_code(dhc::player_t *player)
{
	std::map<uint64_t, uint64_t>::const_iterator it = player_social_code_invites_.find(player->guid());
	if (it == player_social_code_invites_.end())
	{
		return 0;
	}

	return POOL_GET_SOCIAL(it->second);
}


void SocialPool::update_social_code(dhc::player_t *player, dhc::social_t *social)
{
	rpcproto::tmsg_req_invite_code rmsg;
	rmsg.set_player_guid(player->guid());
	rmsg.set_social_guid(social->guid());
	rmsg.set_code(social->name());
	for (int jkl = 0; jkl < social->invite_players_size(); ++jkl)
	{
		rmsg.add_player_guids(social->invite_players(jkl));
		rmsg.add_levels(social->invite_levels(jkl));
	}
	for (int jkl = 0; jkl < social->invite_ids_size(); ++jkl)
	{
		rmsg.add_ids(social->invite_ids(jkl));
	}
	std::string s;
	rmsg.SerializeToString(&s);
	game::rpc_service()->request("remote1", PMSG_SOCIAL_INVITE_CODE_CREATE, s,
		boost::bind(&SocialPool::update_social_code_callback, this, _1, social->guid()));
}

void SocialPool::update_social_code_callback(const std::string &data, uint64_t social_guid)
{
	rpcproto::tmsg_rep_invite_code msg;
	if (!msg.ParseFromString(data))
	{
		return;
	}

	if (msg.res() != -1)
	{
		dhc::social_t *social = POOL_GET_SOCIAL(social_guid);
		if (social)
		{
			social->set_can_shou(1);
			add_update(social_guid);
		}
	}
}


