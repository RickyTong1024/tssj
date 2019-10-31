#include "team.h"
#include "client.h"
#include "pool.h"
#include "mission_fight.h"
#include "role_operation.h"
#include "player_config.h"
#include "sport_config.h"
#include "pvp_config.h"


TeamManager::TeamManager()
:team_id_(0),
invite_id_(0),
fight_id_(0)
{

}

TeamManager::~TeamManager()
{

}

int TeamManager::init()
{
	return 0;
}

int TeamManager::fini()
{
	return 0;
}

int TeamManager::update(const ACE_Time_Value & tv)
{
	update_team_match();
	update_team_prepare();
	update_player_match();
	update_player_invite();
	return 0;
}

void TeamManager::update_team_match()
{
	std::set<uint64_t> delete_match;

	for (TeamMatchMap::iterator it = team_matchs_.begin();
		it != team_matchs_.end();
		++it)
	{
		if (delete_match.find(it->first) != delete_match.end())
		{
			continue;
		}
		Team *mteam = get_team(it->first);
		if (!mteam)
		{
			delete_match.insert(it->first);
			continue;
		}
		/// 非匹配状态
		if (mteam->stat() != TS_MATCH)
		{
			delete_match.insert(it->first);
			continue;
		}
		TeamMatch& stm = it->second;
		stm.time++;

		/// NPC队伍
		if (stm.time >= 10)
		{
			Team *dead_team = get_dead_team(mteam);
			if (dead_team)
			{
				team_fight(mteam, dead_team);
			}
			delete_match.insert(it->first);
			continue;
		}

		/// 活人队伍
		for (TeamMatchMap::iterator jt = team_matchs_.begin();
			jt != team_matchs_.end();
			++jt)
		{
			if (it->first == jt->first)
			{
				continue;
			}
			if (delete_match.find(jt->first) != delete_match.end())
			{
				continue;
			}
			Team *oteam = get_team(jt->first);
			if (!oteam)
			{
				continue;
			}
			/// 非匹配状态
			if (oteam->stat() != TS_MATCH)
			{
				continue;
			}
			if (((mteam->level() >= oteam->level() - 5) &&
				(mteam->level() <= oteam->level() + 5)) ||
				((oteam->level() >= mteam->level() - 5) &&
				(oteam->level() <= mteam->level() - 5)))
			{
				team_fight(mteam, oteam);
				delete_match.insert(it->first);
				delete_match.insert(jt->first);
				break;
			}
		}
	}

	for (std::set<uint64_t>::iterator it = delete_match.begin();
		it != delete_match.end();
		++it)
	{
		team_matchs_.erase(*it);
	}
}

void TeamManager::update_team_prepare()
{
	for (TeamPrepareMap::iterator it = team_prepares_.begin();
		it != team_prepares_.end();)
	{
		Team *team = get_team(it->first);
		if (!team)
		{
			team_prepares_.erase(it++);
			continue;
		}

		if (it->second.time == 0)
		{
			/// 非创建状态
			if (team->stat() != TS_CREATE)
			{
				team_prepares_.erase(it++);
				continue;
			}

			team->set_stat(TS_PREPARE);
			protocol::team::smsg_change_team_stat smsg;
			smsg.set_stat(TS_PREPARE);
			for (int i = 0; i < team->players_size(); ++i)
			{
				if (team->players(i).guid() > 0 &&
					team->players(i).is_npc() == false)
				{
					sClientManager->send_msg(team->players(i).guid(), SMSG_CHANGE_TEAM_STAT, &smsg);
				}
			}
		}
		else
		{
			/// 非准备状态
			if (team->stat() != TS_PREPARE)
			{
				team_prepares_.erase(it++);
				continue;
			}

			if (it->second.time > 30)
			{
				team->set_stat(TS_CREATE);
				protocol::team::smsg_change_team_stat smsg;
				smsg.set_stat(TS_CREATE);
				for (int i = 0; i < team->players_size(); ++i)
				{
					if (team->players(i).guid() > 0 &&
						team->players(i).is_npc() == false)
					{
						sClientManager->send_msg(team->players(i).guid(), SMSG_CHANGE_TEAM_STAT, &smsg);
					}
				}

				TeamPlayer* leader_player = get_team_leader(team);
				if (leader_player)
				{
					dhc::player_t *leader = POOL_GET_PLAYER(leader_player->guid());
					if (leader)
					{
						leave_team(leader);
					}
				}
				team_prepares_.erase(it++);
				continue;;
			}
		}
		it->second.time++;
		++it;
	}
}

void TeamManager::update_player_match()
{
	for (PlayerMatchMap::iterator it = player_matchs_.begin();
		it != player_matchs_.end();)
	{
		Client *cl = sClientManager->get_client(it->first);
		if (!cl)
		{
			player_matchs_.erase(it++);
			continue;
		}

		dhc::player_t *player = cl->get_player();
		if (!player)
		{
			player_matchs_.erase(it++);
			continue;
		}

		PlayerMatch& match = it->second;
		match.time += 1;
		if (match.time >= 11)
		{
			cl->send_msg(SMSG_MATCH_TEAM_TIMEOUT);
			player_matchs_.erase(it++);
			continue;
		}

		TeamManager::Team* team = get_match_team(player);
		if (team)
		{
			enter_team(team->team_id(), player, false, false);
			player_matchs_.erase(it++);
			continue;
		}

		++it;
	}
}

void TeamManager::update_player_invite()
{
	std::vector<std::pair<uint64_t, uint64_t> > delete_invites;

	for (PlayerTeamInvite::iterator it = invite_list_.begin();
		it != invite_list_.end();
		++it)
	{
		std::list<protocol::team::team_invite> &inv = it->second;
		for (std::list<protocol::team::team_invite>::iterator jt = inv.begin();
			jt != inv.end();
			++jt)
		{
			jt->set_time(jt->time() + 1);
			if (jt->time() >= 300)
			{
				delete_invites.push_back(std::make_pair(it->first, jt->invite_id()));
			}
		}
	}

	for (int i = 0; i < delete_invites.size(); ++i)
	{
		remove_invite(delete_invites[i].first, delete_invites[i].second);
	}
}

void TeamManager::create_team(dhc::player_t *player)
{
	if (get_player_team(player) != 0)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_EXIST);
		return;
	}
	++team_id_;

	protocol::team::team* team = new protocol::team::team();
	team->set_team_id(team_id_);
	team->set_level(player->level());
	team->set_open(true);
	team->set_stat(TS_CREATE);

	protocol::team::team_player* team_player = team->add_players();
	team_player->set_guid(player->guid());
	team_player->set_name(player->name());
	team_player->set_id(player->template_id());
	team_player->set_bf(player->bf());
	team_player->set_chenhao(sPoolManager->get_chenghao(player->guid()));
	team_player->set_level(player->level());
	team_player->set_leader(true);
	team_player->set_prepare(true);
	team_player->set_is_npc(false);
	team_player->set_reward_num(player->by_reward_num());
	team_player->set_vip(player->vip());
	team_player->set_achieve(player->dress_achieves_size());
	team_player->set_guanghuan(player->guanghuan_id());
	team_player->set_dress(player->mission());
	team_player->set_nalflag(player->nalflag());
	player->set_last_tili_time(0);
	player->set_last_check_time(0);

	for (int i = 0; i < 4; ++i)
	{
		protocol::team::team_player *dumy_player = team->add_players();
		dumy_player->set_guid(0);
	}

	player_team_id_[player->guid()] = team_id_;
	teams_[team_id_] = team;

	protocol::team::smsg_team_create msg;
	msg.mutable_team_info()->CopyFrom(*team);
	sClientManager->send_msg(player->guid(), SMSG_CREATE_TEAM, &msg);
}

int TeamManager::enter_team(uint64_t team_id, dhc::player_t *player, bool npc, bool invite)
{
	if (get_player_team(player) != 0)
	{
		bool send_error = true;
		if (invite)
		{
			Team *pteam = get_team(get_player_team(player));
			if (pteam &&
				pteam->team_id() != team_id &&
				pteam->stat() == TS_CREATE)
			{
				leave_team(player);
				send_error = false;
			}
		}
		if (send_error)
		{
			sClientManager->send_msg(player->guid(), SMSG_TEAM_EXIST);
			return SMSG_TEAM_EXIST;
		}
	}

	Team* team = get_team(team_id);
	if (!team)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return SMSG_TEAM_NOT_EXIST;
	}

	/// 非创建状态
	if (team->stat() != TS_CREATE)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_FULL);
		return SMSG_TEAM_FULL;
	}

	TeamPlayer *team_player = get_empty_team_player(team);
	if (!team_player)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_FULL);
		return SMSG_TEAM_FULL;
	}

	team_player->set_guid(player->guid());
	if (npc)
		team_player->set_bind_guid(player->ore_last_time());
	else
		team_player->set_bind_guid(0);
	team_player->set_name(player->name());
	team_player->set_id(player->template_id());
	team_player->set_bf(player->bf());
	team_player->set_chenhao(sPoolManager->get_chenghao(player->guid()));
	team_player->set_level(player->level());
	team_player->set_leader(false);
	if (npc)
		team_player->set_prepare(true);
	else
		team_player->set_prepare(false);
	team_player->set_is_npc(npc);
	if (npc)
		team_player->set_reward_num(0);
	else
		team_player->set_reward_num(player->by_reward_num());
	team_player->set_vip(player->vip());
	team_player->set_achieve(player->dress_achieves_size());
	team_player->set_guanghuan(player->guanghuan_id());
	team_player->set_dress(player->mission());
	team_player->set_nalflag(player->nalflag());

	if (!npc)
		player_team_id_[player->guid()] = team->team_id();

	/// 更新队伍状态
	update_team(team);

	protocol::team::smsg_team_create msg_create;
	msg_create.mutable_team_info()->CopyFrom(*team);
	sClientManager->send_msg(player->guid(), SMSG_CREATE_TEAM, &msg_create);

	protocol::team::smsg_team_enter msg_enter;
	msg_enter.mutable_member()->CopyFrom(*team_player);
	msg_enter.set_level(team->level());
	msg_enter.set_jiacheng(team->jiacheng());
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() != player->guid() &&
			team->players(i).guid() > 0 &&
			team->players(i).is_npc() == false)
		{
			sClientManager->send_msg(team->players(i).guid(), SMSG_ENTER_TEAM, &msg_enter);
		}
	}

	/// 加入准备状态
	add_prepare_team(team);

	return 0;
}

void TeamManager::leave_team(dhc::player_t *player)
{
	/// 非NPC
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		return;
	}

	Team* team = get_team(team_id);
	if (!team)
	{
		return;
	}

	/// 移除准备状态,匹配状态
	if (team->stat() == TS_PREPARE ||
		team->stat() == TS_MATCH)
	{
		team->set_stat(TS_CREATE);
		protocol::team::smsg_change_team_stat smsg;
		smsg.set_stat(TS_CREATE);
		for (int i = 0; i < team->players_size(); ++i)
		{
			if (team->players(i).guid() > 0 &&
				team->players(i).is_npc() == false)
			{
				sClientManager->send_msg(team->players(i).guid(), SMSG_CHANGE_TEAM_STAT, &smsg);
			}
		}
	}

	/// 移除玩家队伍
	remove_player_team(player);
	protocol::team::smsg_team_leave msg;
	msg.set_guid(player->guid());
	sClientManager->send_msg(player->guid(), SMSG_LEAVE_TEAM, &msg);

	/// 删除队伍
	if (get_team_num(team) <= 1)
	{
		remove_team(team_id);
	}
	else
	{
		bool leader = false;
		for (int i = 0; i < team->players_size(); ++i)
		{
			if (team->players(i).guid() == player->guid())
			{
				leader = team->players(i).leader();
				team->mutable_players(i)->set_guid(0);
				break;
			}
		}

		/// 剩余是NPC队伍,删除队伍
		if (is_all_npc_team_num(team))
		{
			remove_team(team_id);
			return;
		}

		if (leader)
		{
			for (int i = 0; i < team->players_size(); ++i)
			{
				if (team->players(i).guid() > 0 &&
					team->players(i).is_npc() == false)
				{
					team->mutable_players(i)->set_leader(true);
					team->mutable_players(i)->set_prepare(true);
					msg.set_leader(team->players(i).guid());
					break;
				}
			}
			team->set_fight(false);
		}

		/// 更新队伍状态
		update_team(team);
		msg.set_level(team->level());
		msg.set_jiacheng(team->jiacheng());

		for (int i = 0; i < team->players_size(); ++i)
		{
			if (team->players(i).guid() > 0 &&
				team->players(i).is_npc() == false)
			{
				sClientManager->send_msg(team->players(i).guid(), SMSG_LEAVE_TEAM, &msg);
			}	
		}
	}
}

void TeamManager::kick_team(dhc::player_t *player, dhc::player_t *member)
{
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	if (team->stat() != TS_CREATE &&
		team->stat() != TS_PREPARE)
	{
		return;
	}

	TeamPlayer* team_player = get_team_player(team, player->guid());
	if (!team_player)
	{
		return;
	}
	if (!team_player->leader())
	{
		return;
	}

	TeamPlayer *team_member = get_team_player(team, member->guid());
	if (!team_member)
	{
		return;
	}

	/// 玩家
	if (team_member->is_npc() == false)
	{
		leave_team(member);
	}
	/// NPC
	else
	{
		/// 移除准备状态,匹配状态
		if (team->stat() == TS_PREPARE ||
			team->stat() == TS_MATCH)
		{
			team->set_stat(TS_CREATE);
			protocol::team::smsg_change_team_stat smsg;
			smsg.set_stat(TS_CREATE);
			for (int i = 0; i < team->players_size(); ++i)
			{
				if (team->players(i).guid() > 0 &&
					team->players(i).is_npc() == false)
				{
					sClientManager->send_msg(team->players(i).guid(), SMSG_CHANGE_TEAM_STAT, &smsg);
				}
				
			}
		}

		team_member->set_guid(0);

		/// 更新队伍状态
		update_team(team);

		/// 广播NPC离开消息
		protocol::team::smsg_team_leave msg;
		msg.set_guid(member->guid());
		msg.set_level(team->level());
		msg.set_jiacheng(team->jiacheng());

		for (int i = 0; i < team->players_size(); ++i)
		{
			if (team->players(i).guid() > 0 &&
				team->players(i).is_npc() == false)
			{
				sClientManager->send_msg(team->players(i).guid(), SMSG_LEAVE_TEAM, &msg);
			}
		}
	}
}

void TeamManager::move_team(dhc::player_t *player, dhc::player_t *member, int dest)
{
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	if (team->stat() != TS_CREATE &&
		team->stat() != TS_PREPARE)
	{
		return;
	}

	TeamPlayer * leader = get_team_player(team, player->guid());
	if (!leader)
	{
		return;
	}
	if (!leader->leader())
	{
		return;
	}

	TeamPlayer *team_member = get_team_player(team, member->guid());
	if (!team_member)
	{
		return;
	}

	if (dest < 0 || dest >= 5)
	{
		return;
	}

	int src_index = get_team_player_index(team, member->guid());
	if (src_index < 0 || src_index >= 5)
	{
		return;
	}
	team->mutable_players()->SwapElements(src_index, dest);

	protocol::team::smsg_team_move move_msg;
	move_msg.set_guid(member->guid());
	move_msg.set_index(dest);

	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0 &&
			team->players(i).is_npc() == false)
		{
			sClientManager->send_msg(team->players(i).guid(), SMSG_MOVE_TEAM, &move_msg);
		}	
	}
}

void TeamManager::prepare_team(dhc::player_t *player)
{
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	if (team->stat() != TS_CREATE &&
		team->stat() != TS_PREPARE)
	{
		return;
	}

	TeamPlayer * member = get_team_player(team, player->guid());
	if (!member)
	{
		return;
	}
	if (member->leader())
	{
		return;
	}

	member->set_prepare(!member->prepare());

	if (member->prepare() == false)
	{
		/// 移除准备状态,匹配状态
		if (team->stat() == TS_PREPARE ||
			team->stat() == TS_MATCH ||
			team_prepares_.find(team_id) != team_prepares_.end())
		{
			team_prepares_.erase(team_id);
			team->set_stat(TS_CREATE);
			protocol::team::smsg_change_team_stat smsg;
			smsg.set_stat(TS_CREATE);
			for (int i = 0; i < team->players_size(); ++i)
			{
				if (team->players(i).guid() > 0 &&
					team->players(i).is_npc() == false)
				{
					sClientManager->send_msg(team->players(i).guid(), SMSG_CHANGE_TEAM_STAT, &smsg);
				}
			}
		}
	}

	protocol::team::smsg_team_prepare prepare_msg;
	prepare_msg.set_guid(player->guid());
	prepare_msg.set_prepare(member->prepare());
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() >= 0 &&
			team->players(i).is_npc() == false)
		{
			sClientManager->send_msg(team->players(i).guid(), SMSG_PREPARE_TEAM, &prepare_msg);
		}	
	}

	/// 加入准备状态
	add_prepare_team(team);
}

void TeamManager::open_team(dhc::player_t *player)
{
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	TeamPlayer * member = get_team_player(team, player->guid());
	if (!member)
	{
		return;
	}
	if (!member->leader())
	{
		return;
	}

	team->set_open(!team->open());

	sClientManager->send_msg(player->guid(), SMSG_OPEN_TEAM);
}

void TeamManager::chat_team(dhc::player_t *player, const std::string& color, const std::string& text)
{
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		sClientManager->send_msg(player->guid(), SMSG_TEAM_NOT_EXIST);
		return;
	}

	if (team->stat() != TS_CREATE &&
		team->stat() != TS_PREPARE)
	{
		return;
	}

	TeamPlayer * member = get_team_player(team, player->guid());
	if (!member)
	{
		return;
	}

	protocol::team::smsg_chat_team chat_msg;
	chat_msg.set_player_guid(player->guid());
	chat_msg.set_player_name(player->name());
	chat_msg.set_player_template(player->template_id());
	chat_msg.set_vip(player->vip());
	chat_msg.set_level(player->level());
	chat_msg.set_text(text);
	chat_msg.set_color(color);
	chat_msg.set_player_nalflag(player->nalflag());

	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0 &&
			team->players(i).is_npc() == false)
		{
			sClientManager->send_msg(team->players(i).guid(), SMSG_CHAT_TEAM, &chat_msg);
		}	
	}
}

void TeamManager::view_member(dhc::player_t *player, dhc::player_t *member)
{
	protocol::game::smsg_player_look msg;
	msg.set_template_id(member->template_id());
	msg.set_level(member->level());
	msg.set_name(member->name());
	msg.set_bf(member->bf());
	msg.set_vip(member->vip());
	msg.set_achieves(member->dress_achieves_size());
	msg.set_guid(member->guid());
	msg.set_serverid(member->serverid());
	msg.set_nalflag(member->nalflag());
	for (int i = 0; i < member->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(member->zhenxing(i));
		if (role)
		{
			msg.add_roles()->CopyFrom(*role);

			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (equip)
				{
					msg.add_equips()->CopyFrom(*equip);
				}
				else
				{
					msg.add_equips()->set_guid(0);
				}
			}

			for (int j = 0; j < role->treasures_size(); ++j)
			{
				dhc::treasure_t *treasure = POOL_GET_TREASURE(role->treasures(j));
				if (treasure)
				{
					msg.add_treasures()->CopyFrom(*treasure);
				}
				else
				{
					msg.add_treasures()->set_guid(0);
				}
			}

			dhc::pet_t *pet = POOL_GET_PET(role->pet());
			if (pet)
			{
				msg.add_pets()->CopyFrom(*pet);

				std::map<int, double> pet_attrs;
				RoleOperation::get_pet_attr(player, pet, pet_attrs, true, false, true);
				for (int j = 1; j <= 4; ++j)
				{
					msg.add_pets_sx(pet_attrs[j]);
				}
			}
			else
			{
				msg.add_pets()->set_guid(0);
				for (int j = 1; j <= 4; ++j)
				{
					msg.add_pets_sx(0);
				}
			}

			std::map<int, double> role_attrs;
			RoleOperation::get_role_attr(member, role, role_attrs);
			/// 百分比计算
			for (int j = 1; j <= 5; ++j)
			{
				role_attrs[j] = role_attrs[j] * (1 + role_attrs[j + 5] * 0.01f);
				msg.add_roles_sx(role_attrs[j]);
			}
		}
		else
		{
			msg.add_roles()->set_guid(0);
			for (int j = 1; j <= 5; ++j)
			{
				msg.add_roles_sx(0);
			}
			msg.add_pets()->set_guid(0);
			for (int j = 1; j <= 4; ++j)
			{
				msg.add_pets_sx(0);
			}
		}
	}

	dhc::pet_t *pet_on = POOL_GET_PET(member->pet_on());
	if (pet_on)
	{
		msg.add_pets()->CopyFrom(*pet_on);

		std::map<int, double> pet_attrs;
		RoleOperation::get_pet_attr(player, pet_on, pet_attrs, true, false, true);
		for (int j = 1; j <= 4; ++j)
		{
			msg.add_pets_sx(pet_attrs[j]);
		}
	}
	else
	{
		msg.add_pets()->set_guid(0);
		for (int j = 1; j <= 4; ++j)
		{
			msg.add_pets_sx(0);
		}
	}

	sClientManager->send_msg(player->guid(), SMSG_VIEW_TEAM_MEMBER, &msg);
}

void TeamManager::match_team(dhc::player_t *player)
{
	if (get_player_team(player) != 0)
	{
		return;
	}

	if (player_matchs_.find(player->guid()) != player_matchs_.end())
	{
		return;
	}
	
	PlayerMatch match;
	match.player_guid = player->guid();
	match.time = 0;
	player_matchs_[player->guid()] = match;
	sClientManager->send_msg(player->guid(), SMSG_MATCH_TEAM);
}

void TeamManager::end_match_team(dhc::player_t *player)
{
	if (player_matchs_.find(player->guid()) == player_matchs_.end())
	{
		return;
	}

	player_matchs_.erase(player->guid());
	sClientManager->send_msg(player->guid(), SMSG_END_MATCH_TEAM);
}

void TeamManager::fight_team(dhc::player_t *player)
{
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		return;
	}

	if (team->stat() == TS_MATCH)
	{
		return;
	}

	TeamPlayer *team_player = get_team_player(team, player->guid());
	if (!team_player)
	{
		return;
	}
	if (!team_player->leader())
	{
		return;
	}

	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() == 0)
		{
			return;
		}
		if (!team->players(i).prepare())
		{
			return;
		}
	}

	TeamMatch tm;
	tm.team_id = team_id;
	tm.time = 0;
	team_matchs_[tm.team_id] = tm;

	team->set_stat(TS_MATCH);
	protocol::team::smsg_change_team_stat smsg;
	smsg.set_stat(TS_MATCH);

	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0 &&
			team->players(i).is_npc() == false)
		{
			sClientManager->send_msg(team->players(i).guid(), SMSG_CHANGE_TEAM_STAT, &smsg);
		}
	}
}

void TeamManager::fight_end_team(dhc::player_t *player)
{
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		return;
	}

	if (team->stat() != TS_MATCH)
	{
		return;
	}

	TeamPlayer *team_player = get_team_player(team, player->guid());
	if (!team_player)
	{
		return;
	}
	if (!team_player->leader())
	{
		return;
	}
	
	TeamPrepare tp;
	tp.team_id = team->team_id();
	tp.time = 1;
	team_prepares_[tp.team_id] = tp;

	team->set_stat(TS_PREPARE);
	protocol::team::smsg_change_team_stat smsg;
	smsg.set_stat(TS_PREPARE);
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0 &&
			team->players(i).is_npc() == false)
		{
			sClientManager->send_msg(team->players(i).guid(), SMSG_CHANGE_TEAM_STAT, &smsg);
		}
	}
}

void TeamManager::invite_dead(dhc::player_t *player)
{
	if (game::timer()->now() < player->last_tili_time())
	{
		return;
	}

	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		return;
	}

	if (team->stat() != TS_CREATE)
	{
		return;
	}

	TeamPlayer *team_player = get_team_player(team, player->guid());
	if (!team_player)
	{
		return;
	}
	if (!team_player->leader())
	{
		return;
	}

	int num = get_team_empty_num(team);
	if (!num)
	{
		return;
	}

	std::set<uint64_t> has_invite;
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0 &&
			team->players(i).is_npc())
		{
			has_invite.insert(team->players(i).bind_guid());
		}
	}

	std::vector<dhc::player_t*> dead_vec;
	sPoolManager->get_dead_player(player->guid(), player->level(), num, dead_vec, has_invite);
	protocol::team::smsg_invite_all smsg;
	smsg.set_next_time(game::timer()->now() + 60 * 1000);
	player->set_last_tili_time(smsg.next_time());
	sClientManager->send_msg(player->guid(), SMSG_INVITE_ALL, &smsg);

	for (int i = 0; i < dead_vec.size(); ++i)
	{
		enter_team(team_id, dead_vec[i], true, false);
	}
}

void TeamManager::change_reward_num(dhc::player_t *player)
{
	int point, chenghao, rank;
	sPoolManager->get_rank_info(player->guid(), chenghao, point, rank);
	protocol::team::smsg_team_player_reward_change smsg;
	smsg.set_guid(player->guid());
	smsg.set_num(player->by_reward_num());
	smsg.set_point(point);
	smsg.set_rank(rank);
	smsg.set_chenghao(chenghao);
	sClientManager->send_msg(player->guid(), SMSG_CHANGE_REWARD_NUM, &smsg);

	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		return;
	}

	TeamPlayer *team_player = get_team_player(team, player->guid());
	if (!team_player)
	{
		return;
	}
	team_player->set_reward_num(player->by_reward_num());
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0 &&
			team->players(i).is_npc() == false &&
			team->players(i).guid() != player->guid())
		{
			sClientManager->send_msg(team->players(i).guid(), SMSG_CHANGE_REWARD_NUM, &smsg);
		}
	}
}

void TeamManager::press_team(dhc::player_t *player, int index)
{
	if (game::timer()->now() < player->last_check_time())
	{
		return;
	}

	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		return;
	}

	if (team->stat() != TS_CREATE)
	{
		return;
	}

	TeamPlayer *team_player = get_team_player(team, player->guid());
	if (!team_player)
	{
		return;
	}

	protocol::team::smsg_hanhua smsg;
	smsg.set_content(boost::lexical_cast<std::string>(index));
	smsg.set_guid(player->guid());
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0 &&
			team->players(i).is_npc() == false)
		{
			sClientManager->send_msg(team->players(i).guid(), SMSG_TEAM_URGE, &smsg);
		}
	}

	player->set_last_check_time(game::timer()->now() + 1000);
}

void TeamManager::leader_prepare(dhc::player_t *player)
{
	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		return;
	}

	if (team->stat() != TS_CREATE)
	{
		return;
	}

	TeamPlayer *team_player = get_team_player(team, player->guid());
	if (!team_player)
	{
		return;
	}

	if (!team_player->leader())
	{
		return;
	}

	bool has_member = false;
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() != player->guid() &&
			team->players(i).guid() > 0 &&
			team->players(i).is_npc() == false)
		{
			has_member = true;
			break;
		}
	}
	team->set_fight(false);
	add_prepare_team(team);
}

TeamManager::Team* TeamManager::get_team(uint64_t team_id)
{
	TeamMap::iterator it = teams_.find(team_id);
	if (it == teams_.end())
	{
		return 0;
	}
	return it->second;
}

void TeamManager::remove_team(uint64_t team_id)
{
	Team* team = get_team(team_id);
	if (team)
	{
		for (int i = 0; i < team->players_size(); ++i)
		{
			if (team->players(i).guid() > 0 &&
				team->players(i).is_npc())
			{
				dhc::player_t *player = (dhc::player_t*)POOL_RELEASE(team->players(i).guid());
				if (player)
				{
					delete player;
				}
			}
		}
		teams_.erase(team_id);
		delete team;
	}

	remove_dead_team(team_id);
}

TeamManager::Team* TeamManager::get_match_team(dhc::player_t *player)
{
	Team *team = 0;
	for (TeamMap::iterator it = teams_.begin();
		it != teams_.end();
		++it)
	{
		team = it->second;
		if (team && team->open())
		{
			if ((player->level() >= (team->level() - 5)) &&
				(player->level() <= (team->level() + 5)) &&
				(get_team_empty_num(team) > 0))
			{
				return team;
			}
		}
	}
	return 0;
}

uint64_t TeamManager::get_player_team(dhc::player_t *player) const
{
	PlayerTeamID::const_iterator it = player_team_id_.find(player->guid());
	if (it == player_team_id_.end())
	{
		return 0;
	}
	return it->second;
}

void TeamManager::remove_player_team(dhc::player_t *player)
{
	player_team_id_.erase(player->guid());
}

TeamManager::TeamPlayer* TeamManager::get_team_player(Team *team, uint64_t player_guid)
{
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() == player_guid)
		{
			return team->mutable_players(i);
		}
	}
	return 0;
}

int TeamManager::get_team_player_index(Team *team, uint64_t player_guid) const
{
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() == player_guid)
		{
			return i;
		}
	}
	return -1;
}

int TeamManager::get_team_empty_num(Team *team) const
{
	int count = 0;
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() == 0)
		{
			count++;
		}
	}
	return count;
}

int TeamManager::get_team_num(Team *team) const
{
	int count = 0;
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0)
		{
			count++;
		}
	}
	return count;
}

bool TeamManager::is_all_npc_team_num(Team *team) const
{
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0  &&
			team->players(i).is_npc() == false)
		{
			return false;
		}
	}
	return true;
}

TeamManager::TeamPlayer* TeamManager::get_empty_team_player(Team *team)
{
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() == 0)
		{
			return team->mutable_players(i);
		}
	}
	return 0;
}

TeamManager::TeamPlayer* TeamManager::get_team_leader(Team *team)
{
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).leader())
		{
			return team->mutable_players(i);
		}
	}
	return 0;
}

void TeamManager::add_invite_list(uint64_t invite, uint64_t whois)
{
	Client *cl = sClientManager->get_client(invite);
	if (!cl)
	{
		return;
	}

	dhc::player_t * player = cl->get_player();
	if (!player)
	{
		return;
	}

	uint64_t team_id = get_player_team(player);
	if (team_id == 0)
	{
		return;
	}

	Team *team = get_team(team_id);
	if (!team)
	{
		return;
	}
	TeamPlayer* team_player = get_team_player(team, player->guid());
	if (!team_player)
	{
		return;
	}

	/// 同一个队伍
	if (get_team_player(team, whois))
	{
		return;
	}
	
	/// 已经邀请过
	for (int i = 0; i < team_player->invites_size(); ++i)
	{
		if (team_player->invites(i) == whois)
		{
			return;
		}
	}

	protocol::team::team_invite ti;
	ti.set_invite_id(++invite_id_);
	ti.set_guid(player->guid());
	ti.set_name(player->name());
	ti.set_id(player->template_id());
	ti.set_chenghao(sPoolManager->get_chenghao(player->guid()));
	ti.set_bf(player->bf());
	ti.set_guild(player->zsname());
	ti.set_level(player->level());
	ti.set_vip(player->vip());
	ti.set_achieve(player->dress_achieves_size());
	ti.set_team_id(team_id);
	ti.set_time(0);
	ti.set_nalflag(player->nalflag());

	team_player->add_invites(whois);
	invite_list_[whois].push_back(ti);

	protocol::team::smsg_invite_add smsg;
	smsg.mutable_invite()->CopyFrom(ti);
	sClientManager->send_msg(whois, SMSG_INVITE_ADD, &smsg);
}

void TeamManager::social_invite(dhc::player_t *player, uint64_t invite_id)
{
	std::map<uint64_t, std::list<protocol::team::team_invite> >::iterator it = invite_list_.find(player->guid());
	if (it == invite_list_.end())
	{
		return;
	}

	std::list<protocol::team::team_invite>& inv = it->second;
	for (std::list<protocol::team::team_invite>::iterator it = inv.begin();
		it != inv.end();
		++it)
	{
		if (it->invite_id() == invite_id)
		{
			if (enter_team(it->team_id(), player, false, true) != SMSG_TEAM_FULL)
			{
				protocol::team::smsg_invite_remove smsg;
				smsg.set_invite_id(invite_id);
				sClientManager->send_msg(player->guid(), SMSG_INVITE_REMOVE, &smsg);

				Team* team = get_team(it->team_id());
				if (team)
				{
					TeamPlayer *team_player = get_team_player(team, it->guid());
					if (team_player)
					{
						for (int i = 0; i < team_player->invites_size(); ++i)
						{
							if (team_player->invites(i) == player->guid())
							{
								protocol::team::smsg_invite_remove_social smsg_remove;
								smsg_remove.set_guid(player->guid());
								sClientManager->send_msg(it->guid(), SMSG_INVITE_REMOVE_SOCIAL, &smsg_remove);

								team_player->mutable_invites()->SwapElements(i, team_player->invites_size() - 1);
								team_player->mutable_invites()->RemoveLast();
								break;
							}
						}
					}
				}

				inv.erase(it);
			}
			break;
		}
	}

	if (inv.empty())
	{
		invite_list_.erase(it);
	}
}

void TeamManager::remove_invite(dhc::player_t *player, uint64_t invite_id)
{
	std::map<uint64_t, std::list<protocol::team::team_invite> >::iterator it = invite_list_.find(player->guid());
	if (it == invite_list_.end())
	{
		return;
	}

	std::list<protocol::team::team_invite>& inv = it->second;
	for (std::list<protocol::team::team_invite>::iterator it = inv.begin();
		it != inv.end();
		++it)
	{
		if (it->invite_id() == invite_id)
		{
			protocol::team::smsg_invite_remove smsg;
			smsg.set_invite_id(invite_id);
			sClientManager->send_msg(player->guid(), SMSG_INVITE_REMOVE, &smsg);
			
			Team* team = get_team(it->team_id());
			if (team)
			{
				TeamPlayer *team_player = get_team_player(team, it->guid());
				if (team_player)
				{
					for (int i = 0; i < team_player->invites_size(); ++i)
					{
						if (team_player->invites(i) == player->guid())
						{
							protocol::team::smsg_invite_remove_social smsg_remove;
							smsg_remove.set_guid(player->guid());
							sClientManager->send_msg(it->guid(), SMSG_INVITE_REMOVE_SOCIAL, &smsg_remove);

							team_player->mutable_invites()->SwapElements(i, team_player->invites_size() - 1);
							team_player->mutable_invites()->RemoveLast();
							break;
						}
					}
				}
			}

			inv.erase(it);
			break;
		}
	}

	if (inv.empty())
	{
		invite_list_.erase(it);
	}
}

void TeamManager::remove_invite(uint64_t player_guid, uint64_t invite_id)
{
	std::map<uint64_t, std::list<protocol::team::team_invite> >::iterator it = invite_list_.find(player_guid);
	if (it == invite_list_.end())
	{
		return;
	}

	std::list<protocol::team::team_invite>& inv = it->second;
	for (std::list<protocol::team::team_invite>::iterator it = inv.begin();
		it != inv.end();
		++it)
	{
		if (it->invite_id() == invite_id)
		{
			protocol::team::smsg_invite_remove smsg;
			smsg.set_invite_id(invite_id);
			sClientManager->send_msg(player_guid, SMSG_INVITE_REMOVE, &smsg);

			Team* team = get_team(it->team_id());
			if (team)
			{
				TeamPlayer *team_player = get_team_player(team, it->guid());
				if (team_player)
				{
					for (int i = 0; i < team_player->invites_size(); ++i)
					{
						if (team_player->invites(i) == player_guid)
						{
							protocol::team::smsg_invite_remove_social smsg_remove;
							smsg_remove.set_guid(player_guid);
							sClientManager->send_msg(it->guid(), SMSG_INVITE_REMOVE_SOCIAL, &smsg_remove);

							team_player->mutable_invites()->SwapElements(i, team_player->invites_size() - 1);
							team_player->mutable_invites()->RemoveLast();
							break;
						}
					}
				}
			}

			inv.erase(it);
			break;
		}
	}

	if (inv.empty())
	{
		invite_list_.erase(it);
	}
}

void TeamManager::add_prepare_team(Team *team)
{
	if (team->stat() == TS_PREPARE)
	{
		return;
	}

	if (team->fight())
	{
		return;
	}

	int team_num = 0;
	int prepare_num = 0;
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0)
		{
			team_num++;
		}
		if (team->players(i).prepare())
		{
			prepare_num++;
		}
	}

	if (team_num >= 5 && prepare_num >= 5)
	{
		TeamPrepare tp;
		tp.team_id = team->team_id();
		tp.time = 0;
		team_prepares_[tp.team_id] = tp;
	}
}

int TeamManager::get_jiacheng(dhc::player_t *player, dhc::player_t *target) const
{
	if (player->guild() == target->guild())
	{
		return 6;
	}

	bool is_friend = false;
	for (int i = 0; i < player->sports_size(); ++i)
	{
		if (player->sports(i) == target->guid())
		{
			return 4;
		}
	}

	return 2;
}

void TeamManager::get_invite_list(uint64_t player_guid, protocol::team::smsg_enter_world &smsg) const
{
	std::map<uint64_t, std::list<protocol::team::team_invite> >::const_iterator it = invite_list_.find(player_guid);
	if (it == invite_list_.end())
	{
		return;
	}

	const std::list<protocol::team::team_invite> &invs = it->second;
	for (std::list<protocol::team::team_invite>::const_iterator it = invs.begin();
		it != invs.end();
		++it)
	{
		smsg.add_invites()->CopyFrom(*it);
	}
}

bool TeamManager::get_bingyuan_result(dhc::player_t *player, uint64_t id, int num, int &point, int &bingjin)
{
	uint64_t my_team_id = get_player_team(player);
	if (my_team_id == 0)
	{
		return false;
	}

	if (num <= 0)
	{
		return false;
	}

	TeamFightResult findt;
	findt.id = id;
	std::list<TeamFightResult>::iterator it = std::find(team_fight_result_.begin(),
		team_fight_result_.end(), findt);
	if (it == team_fight_result_.end())
	{
		return false;
	}

	int jiacheng = 0;
	bool result = true;
	TeamFightResult &tr = *it;

	TeamPlayer *tp = 0;
	if (tr.team1->team_id() == my_team_id)
	{
		tp = get_team_player(tr.team1, player->guid());
		if (tp)
		{
			jiacheng = tr.team1->jiacheng();
			result = tr.team1->result();
		}
	}
	else if (tr.team2->team_id() == my_team_id)
	{
		tp = get_team_player(tr.team2, player->guid());
		if (tp)
		{
			jiacheng = tr.team2->jiacheng();
			result = tr.team2->result();
		}
	}
	if (!tp)
	{
		return false;
	}

	const s_t_exp *t_exp = sPlayerConfig->get_exp(player->level());
	if (!t_exp)
	{
		return false;
	}

	bool shuang = false;
	int hour = game::timer()->hour();
	if (hour >= 12 && hour < 14)
	{
		shuang = true;
	}
	if (hour >= 18 && hour < 22)
	{
		shuang = true;
	}

	tp->set_guid(0);
	if (result)
	{
		bingjin = t_exp->bywin + t_exp->bywin * jiacheng / 100;
		point = 100 + 100 * jiacheng / 100;
	}
	else
	{
		bingjin = t_exp->bylose + t_exp->bylose * jiacheng / 100;
		point = 50 + 50 * jiacheng / 100;
	}

	if (shuang)
	{
		point *= 2;
	}

	return true;
}

int TeamManager::team_fight(Team *steam, Team *oteam)
{
	steam->set_stat(TS_CREATE);
	oteam->set_stat(TS_CREATE);
	steam->set_fight(true);
	oteam->set_fight(true);

	for (int i = 0; i < steam->players_size(); ++i)
	{
		if (steam->players(i).leader() == false &&
			steam->players(i).is_npc() == false)
		{
			steam->mutable_players(i)->set_prepare(false);
		}
		
	}
	for (int i = 0; i < oteam->players_size(); ++i)
	{
		if (oteam->players(i).leader() == false &&
			oteam->players(i).is_npc() == false)
		{
			oteam->mutable_players(i)->set_prepare(false);
		}
	}

	++fight_id_;

	TeamFightResult teamResult;
	teamResult.team1 = new protocol::team::team();
	teamResult.team1->CopyFrom(*steam);
	teamResult.team2 = new protocol::team::team();
	teamResult.team2->CopyFrom(*oteam);
	teamResult.id = fight_id_;
	
	protocol::team::smsg_fight_team smsg;
	int res = MissionFight::mission_bingyuan(teamResult.team1, teamResult.team2, *smsg.mutable_text());
	smsg.set_id(fight_id_);

	uint64_t win_team = steam->team_id();
	if (res == -1)
	{
		win_team = oteam->team_id();
	}
	smsg.set_win(win_team);

	smsg.mutable_oteam()->CopyFrom(*teamResult.team2);
	teamResult.team1->set_result((res == 1 ? true : false));
	for (int i = 0; i < steam->players_size(); ++i)
	{
		if (steam->players(i).guid() > 0 &&
			steam->players(i).is_npc() == false)
		{
			sClientManager->send_msg(steam->players(i).guid(), SMSG_BINGYUAN_FIGHT_END, &smsg);
		}
	}

	smsg.mutable_oteam()->CopyFrom(*teamResult.team1);
	teamResult.team2->set_result((res == 1 ? false : true));
	for (int i = 0; i < oteam->players_size(); ++i)
	{
		if (oteam->players(i).guid() > 0 &&
			oteam->players(i).is_npc() == false)
		{
			sClientManager->send_msg(oteam->players(i).guid(), SMSG_BINGYUAN_FIGHT_END, &smsg);
		}
	}

	if (team_fight_result_.size() >= 10)
	{
		TeamFightResult first_tr = team_fight_result_.front();
		team_fight_result_.pop_front();
		if (first_tr.team1)
			delete first_tr.team1;
		if (first_tr.team2)
			delete first_tr.team2;
	}

	team_fight_result_.push_back(teamResult);

	return 0;
}

TeamManager::Team* TeamManager::get_dead_team(Team *team)
{
	TeamPlayer *leader = get_team_leader(team);
	if (!leader)
	{
		return 0;
	}

	std::vector<dhc::player_t*> dead_vec_;
	std::set<uint64_t> temp;
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).is_npc()&&
			team->players(i).guid() > 0)
		{
			temp.insert(team->players(i).bind_guid());
		}
	}

	sPoolManager->get_dead_player(leader->guid(), team->level(), 5, dead_vec_, temp);

	remove_dead_team(team->team_id());

	Team *dead_team = new Team();
	dead_team->set_team_id(++team_id_);
	for (int i = 0; i < dead_vec_.size(); ++i)
	{
		dhc::player_t *player = dead_vec_[i];
		if (player)
		{
			TeamPlayer *dead_team_player = dead_team->add_players();
			dead_team_player->set_guid(player->guid());
			dead_team_player->set_name(player->name());
			dead_team_player->set_id(player->template_id());
			dead_team_player->set_level(player->level());
			dead_team_player->set_vip(player->vip());
			dead_team_player->set_achieve(player->dress_achieves_size());
			dead_team_player->set_bf(player->bf());
			dead_team_player->set_is_npc(true);
			dead_team_player->set_chenhao(sPvpConfig->get_default_chenghao());
			dead_team_player->set_nalflag(player->nalflag());
		}
	}
	dead_team_[team->team_id()] = dead_team;
	return dead_team;
}

void TeamManager::remove_dead_team(uint64_t team_id)
{
	std::map<uint64_t, Team*>::iterator it = dead_team_.find(team_id);
	if (it != dead_team_.end())
	{
		Team *dead_team = it->second;
		if (dead_team)
		{
			for (int i = 0; i < dead_team->players_size(); ++i)
			{
				if (dead_team->players(i).guid() > 0 &&
					dead_team->players(i).is_npc())
				{
					dhc::player_t *player = (dhc::player_t*)POOL_RELEASE(dead_team->players(i).guid());
					if (player)
					{
						delete player;
					}
				}
			}

			delete dead_team;
		}
		dead_team_.erase(it);
	}
}

void TeamManager::update_team(Team *team)
{
	int team_num = 0;
	int level = 0;
	dhc::player_t *leader = 0;
	std::vector<dhc::player_t *> member;
	for (int i = 0; i < team->players_size(); ++i)
	{
		if (team->players(i).guid() > 0)
		{
			team_num++;
			level += team->players(i).level();

			if (team->players(i).leader())
			{
				leader = POOL_GET_PLAYER(team->players(i).guid());
			}
			else
			{
				if (team->players(i).is_npc() == false)
				{
					dhc::player_t *player = POOL_GET_PLAYER(team->players(i).guid());
					if (player)
					{
						member.push_back(player);
					}
				}
			}
		}
	}

	if (team_num > 0)
	{
		team->set_level(level / team_num);
	}
	team->set_jiacheng(0);
	if (leader)
	{
		for (int i = 0; i < member.size(); ++i)
		{
			team->set_jiacheng(team->jiacheng() + get_jiacheng(leader, member[i]));
		}
	}
}