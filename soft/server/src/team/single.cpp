#include "single.h"
#include "client.h"
#include "team_config.h"
#include "pool.h"
#include "mission_fight.h"
#include "utils.h"
#include <math.h>


bool SingleMatch::match_last(const SingleMatch &rhs) const
{
	if (std::abs(duanwei - rhs.duanwei) <= 1)
	{
		return true;
	}

	if (bf >= rhs.bf_min && bf <= rhs.bf_max &&
		rhs.bf >= bf_min && rhs.bf <= bf_max)
	{
		return true;
	}

	return false;
}

SingleManager::SingleManager()
:fight_id_(0)
{
}

SingleManager::~SingleManager()
{

}

int SingleManager::update(const ACE_Time_Value & tv)
{
	for (std::map<uint64_t, SingleMatch>::iterator it = matchs_.begin();
		it != matchs_.end();
		++it)
	{
		SingleMatch& sm1 = it->second;
		if (sm1.match)
		{
			continue;
		}
		sm1.count++;

		for (std::map<uint64_t, SingleMatch>::iterator jt = matchs_.begin();
			jt != matchs_.end();
			++jt)
		{
			SingleMatch &sm2 = jt->second;
			if (sm2.match)
			{
				continue;
			}
			if (sm2.guid == sm1.guid)
			{
				continue;
			}

			if (sm2.match_duanwei(sm1))
			{
				sm1.match = true;
				sm2.match = true;
				fight(sm1.guid, sm2.guid,false);
				break;
			}

			if (sm1.count >= 3 && 
				sm2.count >= 1 &&
				sm2.match_last(sm1))
			{
				sm1.match = true;
				sm2.match = true;
				fight(sm1.guid, sm2.guid,false);
				break;
			}
		}

		if (!sm1.match &&
			sm1.count >= 3)
		{
			sm1.match = true;
			uint64_t match_guid = sPoolManager->get_dead_player(sm1.guid, sm1.duanwei, sm1.bf_max, sm1.bf_min);
			if (match_guid == 0)
			{
				sClientManager->send_msg(sm1.guid, SMSG_DS_MATCH_TIMEOUT);
			}
			else
			{
				dhc::player_t *target_match = POOL_GET_PLAYER(match_guid);
				if (target_match)
				{
					fight(sm1.guid, match_guid, true);
				}
				else
				{
					sClientManager->send_msg(sm1.guid, SMSG_DS_MATCH_TIMEOUT);
				}
			}
			
		}
	}

	for (std::map<uint64_t, SingleMatch>::iterator it = matchs_.begin();
		it != matchs_.end();)
	{
		if (it->second.match)
		{
			matchs_.erase(it++);
			continue;
		}
		it++;
	}

	return 0;
}

void SingleManager::start_match(dhc::player_t *player)
{
	if (matchs_.find(player->guid()) != matchs_.end())
	{
		return;
	}

	if (player->last_leave_guild_time() > game::timer()->now())   
	{ 
		sClientManager->send_msg(player->guid(), SMSG_DS_MATCH_CD); 
		return; 
	} 

	if (player->huodong_yxhg_time() > game::timer()->now())
	{
		sClientManager->send_msg(player->guid(), SMSG_DS_MATCH_CD);
		return;
	}
	 

	SingleMatch match;
	match.guid = player->guid();
	match.bf_max = (int64_t)player->bf() * 120 / 100;
	match.bf_min = (int64_t)player->bf() * 50 / 100;
	match.bf = player->bf();
	match.duanwei = player->ds_duanwei();
	match.match = false;
	match.count = 0;

	matchs_[match.guid] = match;

	if (game::timer()->now() - player->shoppet_last_time() <= 60 * 60 * 1000)
	{
		player->set_guild_buy_num(player->guild_buy_num() + 1);
		if (player->guild_buy_num() >= 120)
		{
			player->set_last_leave_guild_time(game::timer()->now() + 61 * 60 * 1000);
			player->set_guild_buy_num(0);
		}
	}
	else
	{
		player->set_guild_buy_num(0);
	}
	player->set_shoppet_last_time(game::timer()->now());
	

	sClientManager->send_msg(player->guid(), SMSG_DS_MATCH);
}

bool SingleManager::stop_match(dhc::player_t *player)
{
	if (matchs_.find(player->guid()) == matchs_.end())
	{
		return false;
	}
	matchs_.erase(player->guid());
	return true;
}

void SingleManager::fight(uint64_t player_guid, uint64_t target_guid, bool isclone)
{
	protocol::team::smsg_fight_ds smsg;
	smsg.set_id(++fight_id_);
	smsg.set_result(0);
	smsg.set_text("");

	FightResult fr;
	fr.id = fight_id_;
	fr.win_guid = player_guid;
	fr.win_bf = 0;
	fr.lose_guid = target_guid;
	fr.lose_bf = 0;
	fr.win = false;

	
	int result = 0;
	uint64_t lose_time = game::timer()->now() + 5 * 60 * 1000;
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	dhc::player_t *target = POOL_GET_PLAYER(target_guid);
	Client* client_target = sClientManager->get_client(target_guid);
	Client* client_player = sClientManager->get_client(player_guid);
	if (player && target)
	{
		std::string text;
		result = MissionFight::mission_sport(player, target, text);
		smsg.set_text(text);
		if (result == 1)
		{
			fr.win_guid = player_guid;
			fr.win_bf = player->bf();
			fr.lose_guid = target_guid;
			fr.lose_bf = target->bf();
			fr.win = true;
			if (client_target && target->ds_duanwei() >= 25)
			{
				target->set_huodong_yxhg_time(lose_time);
			}
		}
		else if (result == -1) 
		{  
			fr.win_guid = target_guid; 
			fr.win_bf = target->bf();
			fr.lose_guid = player_guid; 
			fr.lose_bf = player->bf();
			fr.win = true;
			if (client_player && player->ds_duanwei() >= 25)
			{
				player->set_huodong_yxhg_time(lose_time);
			}
		} 
	} 

	if (fight_results_.size() >= 10)
	{
		fight_results_.pop_front();
	}
	fight_results_.push_back(fr);

	if (target)
	{
		smsg.set_name(target->name());
		smsg.set_duanwei(target->ds_duanwei());
		smsg.set_bf(target->bf());
		smsg.set_template_id(target->template_id());
		smsg.set_vip(target->vip());
		smsg.set_achieve(target->dress_achieves_size());
		smsg.set_guanghuan(target->guanghuan_id());
		smsg.set_dress(target->mission());
		
		smsg.set_result(result);
		if (result == -1)
		{
			if (client_target && target->ds_duanwei() >= 25)
			{
				smsg.set_cd_time(lose_time);
			}
		}
	}
	sClientManager->send_msg(player_guid, SMSG_DS_FIGHT_END, &smsg);

	if (player)
	{
		smsg.set_name(player->name());
		smsg.set_duanwei(player->ds_duanwei());
		smsg.set_bf(player->bf());
		smsg.set_template_id(player->template_id());
		smsg.set_vip(player->vip());
		smsg.set_achieve(player->dress_achieves_size());
		smsg.set_guanghuan(player->guanghuan_id());
		smsg.set_dress(player->mission());

		if (result == 1)
		{
			smsg.set_result(-1);
			if (client_player && player->ds_duanwei() >= 25)
			{
				smsg.set_cd_time(lose_time);
			}
		}
		else if (result == -1)
		{
			smsg.set_result(1);
		} 
		else 
		{
			smsg.set_result(result);
		}
	}
	sClientManager->send_msg(target_guid, SMSG_DS_FIGHT_END, &smsg);

	if (isclone)
	{
		POOL_RELEASE(target_guid);
	}
}
int SingleManager::get_basic_point(int player_ds_duanwei, bool isgot)
{
	if (player_ds_duanwei <= 8)
	{
		if (isgot)
		{
			return 100;
		}
		return 0;
	}
	else if (player_ds_duanwei >8 && player_ds_duanwei <= 12)
	{
		if (isgot)
		{
			return 60;
		}
		return  20;
	}
	else if (player_ds_duanwei > 12 && player_ds_duanwei <= 16)
	{
		if (isgot)
		{
			return 50;
		}
		return 25;
	}
	else if (player_ds_duanwei > 16 && player_ds_duanwei <= 20)
	{
		return 30;
	}
	else if (player_ds_duanwei > 20 && player_ds_duanwei <= 24)
	{
		if (isgot)
		{
			return 25;
		}
		return 50;
	}
	else if (player_ds_duanwei > 24 && player_ds_duanwei <= 28)
	{
		if (isgot)
		{
			return 15;
		}
		return 60;
	}
	return 0;
}

bool SingleManager::get_fight_result(dhc::player_t *player, uint64_t id, int &point, int &xinpian, int &ciliao)
{
	FightResult fr;
	fr.id = id;
	std::list<FightResult>::iterator it = std::find(fight_results_.begin(),
		fight_results_.end(),
		fr);
	if (it == fight_results_.end())
	{
		return false;
	}

	uint64_t win_guid = it->win_guid;
	uint64_t win_bf = it->win_bf;
	uint64_t lose_bf = it->lose_bf;
	uint64_t lose_guid = it->lose_guid;
	if (!it->win && win_guid == player->guid())
	{
		win_guid = lose_guid;
		lose_guid = player->guid();
		win_bf = lose_bf;
		lose_bf = player->bf();
	}

	if (win_guid == player->guid())
	{
		int bf_point = get_basic_point(player->ds_duanwei(), true);
		double target_bf = double(lose_bf);
		double player_bf = double(win_bf);
		point += static_cast<int>((1 + (target_bf - player_bf)/ player_bf) * bf_point);
		if (point < bf_point / 10)
		{
			point = bf_point / 10;
		}
		xinpian = 40 + Utils::get_int32(player->level() * 2, player->level() * 5);
		ciliao = 2;
	}
	else if (lose_guid == player->guid())
	{
		int bf_point = get_basic_point(player->ds_duanwei(), false);
		double target_bf = double(win_bf);
		double player_bf = double(lose_bf);
		if (point < bf_point / 10)
		{
			point = bf_point / 10;
		}
		point -= static_cast<int>((1 + (player_bf - target_bf)/ player_bf) * bf_point);
		xinpian = (40 + Utils::get_int32(player->level() * 2, player->level() * 5)) / 2;
		ciliao = 1;
	}
	else
	{
		return false;
	}

	return true;
}