#include "mission_pool.h"
#include "mission_config.h"
#include "social_operation.h"
#include "player_load.h"
#include "rank_operation.h"
#include "global_pool.h"
#include "huodong_pool.h"

MissionPool::MissionPool()
: yb_info_id_(0)
, ybq_info_id_(0)
, time_(0)
, load_num_(0)
, yb_player_id_(0)
{
	
}

MissionPool::~MissionPool()
{
	
}

void MissionPool::start_yb(dhc::player_t *player)
{
	if (player->yb_type() > 0)
	{
		add_yb_info(0, player->name(), "", player->yb_type(), 0);
	}
	if (player->yb_type() >= 3)
	{
		s_t_yb *t_yb = sMissionConfig->get_yb(player->yb_type());
		if (!t_yb)
		{
			return;
		}
		//std::string text;
		//game::scheme()->get_server_str(text, "yb_ts", player->name().c_str(), t_yb->name.c_str());
		//SocialOperation::gundong(text);
		SocialOperation::gundong_server("t_server_language_text_yb_ts", player->name(), t_yb->name, "");

		if (player->yb_type() == 4)
		{
			sHuodongPool->huodong_active(player, HUODONG_COND_TKYS_HS_COUNT, 1);
		}
	}
	refresh_yb(player);
}

void MissionPool::refresh_yb(dhc::player_t *player)
{
	if (player->yb_start_time() == 0)
	{
		return;
	}
	if (get_yb_finish_time(player) <= game::timer()->now())
	{
		return;
	}

	for (std::list<yb_player_item>::iterator it = yb_players_.begin(); it != yb_players_.end(); ++it)
	{
		if ((*it).player_guid == player->guid())
		{
			yb_players_.erase(it);
			break;
		}
	}
	yb_player_id_++;
	yb_player_item yp;
	yp.player_guid = player->guid();
	yp.player_id = yb_player_id_;
	yb_players_.push_back(yp);
}

void MissionPool::remove_yb(dhc::player_t *player)
{
	for (std::list<yb_player_item>::iterator it = yb_players_.begin(); it != yb_players_.end(); ++it)
	{
		if ((*it).player_guid == player->guid())
		{
			player_ybq_infos_.erase((*it).player_guid);
			yb_players_.erase(it);
			break;
		}
	}
}

uint64_t MissionPool::get_yb_finish_time(dhc::player_t *player)
{
	int type = player->yb_type();
	s_t_yb *t_yb = sMissionConfig->get_yb(type);
	if (!t_yb)
	{
		return 0;
	}
	if (player->yb_jiasu_time() > 0)
	{
		return (t_yb->time - (player->yb_jiasu_time() - player->yb_start_time())) / 2 + player->yb_jiasu_time();
	}
	return player->yb_start_time() + t_yb->time;
}

bool MissionPool::is_start_yb(dhc::player_t *player)
{
	return player->yb_start_time() > 0;
}

bool MissionPool::is_finish_yb(dhc::player_t *player)
{
	return get_yb_finish_time(player) < game::timer()->now();
}

bool MissionPool::is_jiasu_yb(dhc::player_t *player)
{
	return player->yb_jiasu_time() > 0;
}

void MissionPool::update()
{
	for (std::list<yb_player_item>::iterator it = yb_players_.begin(); it != yb_players_.end();)
	{
		dhc::player_t *player = POOL_GET_PLAYER((*it).player_guid);
		if (!player)
		{
			player_ybq_infos_.erase((*it).player_guid);
			yb_players_.erase(it++);
		}
		else
		{
			if (player->yb_start_time() == 0)
			{
				player_ybq_infos_.erase((*it).player_guid);
				yb_players_.erase(it++);
			}
			else if (is_finish_yb(player))
			{
				player_ybq_infos_.erase((*it).player_guid);
				yb_players_.erase(it++);
			}
			else
			{
				++it;
			}
		}
	}

	uint64_t now = game::timer()->now();
	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (global)
	{
		if (game::timer()->trigger_time(global->ore_rank_time(), 0, 0))
		{
			RankOperation::clear_rank(e_rank_ore);
			global->set_ore_rank_time(now);
		}
	}
}

int MissionPool::get_yb_players(dhc::player_t *player, std::vector<protocol::game::msg_yb_player> &yb_players, int player_id, int num)
{
	int bnum = 0;
	std::set<uint64_t> guids;
	if (is_start_yb(player) && !is_finish_yb(player))
	{
		guids.insert(player->guid());
	}
	int mid = player_id;
	for (std::list<yb_player_item>::iterator it = yb_players_.begin(); it != yb_players_.end(); ++it)
	{
		if ((*it).player_guid != player->guid() && (*it).player_id > player_id)
		{
			guids.insert((*it).player_guid);
			bnum++;
			mid = (*it).player_id;
			if (bnum >= num)
			{
				break;
			}
		}
	}
	for (std::set<uint64_t>::iterator it = guids.begin(); it != guids.end(); ++it)
	{
		dhc::player_t *player = POOL_GET_PLAYER(*it);
		if (player)
		{
			protocol::game::msg_yb_player yb_player;
			yb_player.set_player_guid(player->guid());
			yb_player.set_player_name(player->name());
			yb_player.set_player_level(player->yb_level());
			yb_player.set_player_bf(player->bf());
			yb_player.set_player_type(player->yb_type());
			yb_player.set_player_per(player->yb_per());
			yb_player.set_start_time(player->yb_start_time());
			yb_player.set_jiasu_time(player->yb_jiasu_time());
			yb_player.set_player_ybq_num(player->yb_byb_num());
			yb_player.set_player_vip(player->vip());
			yb_player.set_player_achieve(player->dress_achieves_size());
			yb_player.set_player_template(player->template_id());
			yb_player.set_player_nalflag(player->nalflag());
			yb_players.push_back(yb_player);
		}
	}
	return mid;
}

void MissionPool::add_yb_info(int type, const std::string &player_name, const std::string &target_name, int yb_type, int yuanli)
{
	yb_info_id_++;
	protocol::game::msg_yb_info info;
	info.set_info_id(yb_info_id_);
	info.set_type(type);
	info.set_player_name(player_name);
	info.set_target_name(target_name);
	info.set_yb_type(yb_type);
	info.set_yuanli(yuanli);
	yb_infos_.push_back(info);
	if (yb_infos_.size() > 10)
	{
		yb_infos_.pop_front();
	}
}

void MissionPool::get_yb_info(std::list<protocol::game::msg_yb_info> &infos)
{
	infos = yb_infos_;
}

void MissionPool::add_ybq_info(dhc::player_t *player, int type, const std::string &player_name, int yuanli)
{
	ybq_info_id_++;
	protocol::game::msg_ybq_info info;
	info.set_info_id(ybq_info_id_);
	info.set_type(type);
	info.set_player_name(player_name);
	info.set_yuanli(yuanli);
	player_ybq_infos_[player->guid()].push_back(info);
	if (player_ybq_infos_[player->guid()].size() > 5)
	{
		player_ybq_infos_[player->guid()].pop_front();
	}
}

void MissionPool::get_ybq_info(uint64_t player_guid, std::list<protocol::game::msg_ybq_info> &infos)
{
	std::map<uint64_t, std::list<protocol::game::msg_ybq_info> >::iterator it = player_ybq_infos_.find(player_guid);
	if (it == player_ybq_infos_.end())
	{
		return;
	}
	infos = (*it).second;
	player_ybq_infos_.erase(player_guid);
}

bool MissionPool::is_yb_effect(dhc::player_t *player)
{
	if (player_ybq_infos_.find(player->guid()) != player_ybq_infos_.end())
	{
		return true;
	}
	if (is_start_yb(player))
	{
		if (is_finish_yb(player))
		{
			return true;
		}
	}
	return false;
}

void MissionPool::init()
{
	time_ = game::timer()->now();
}
