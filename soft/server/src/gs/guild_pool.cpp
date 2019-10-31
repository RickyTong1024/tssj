#include "guild_pool.h"
#include "guild_operation.h"
#include "social_operation.h"
#include "item_operation.h"
#include "pvp_operation.h"
#include "player_load.h"


GuildPool::GuildPool()
{
	last_time_ = game::timer()->now();
	last_update_time_ = game::timer()->now();
	last_refresh_time_ = game::timer()->now();
	guild_pvp_hour_ = 0;
	guild_pvp_week_ = 0;
}

GuildPool::~GuildPool()
{

}

int GuildPool::init()
{
	guild_list_load();
	return 0;
}

int GuildPool::fini()
{
	save_all();
	return 0;
}

void GuildPool::guild_list_load()
{
	uint64_t guild_list_guid = MAKE_GUID(et_guild_list, 0);
	Request *req = new Request();
	req->add(opc_query, guild_list_guid, new dhc::guild_list_t);
	game::pool()->upcall(req, boost::bind(&GuildPool::guild_list_load_callback, this, _1, guild_list_guid));
}

void GuildPool::guild_list_load_callback(Request *req, uint64_t guild_list_guid)
{
	dhc::guild_list_t *guild_list = NULL;
	if (req && req->success())
	{
		guild_list = (dhc::guild_list_t*)req->release_data();
		if (!guild_list)
		{
			return;
		}
		POOL_ADD(req->guid(), guild_list);

		for (int i = 0; i != guild_list->guild_guids_size(); ++i)
		{
			guild_load(guild_list->guild_guids(i));
		}
	}
	else
	{
		guild_list = new dhc::guild_list_t;
		guild_list->set_guid(guild_list_guid);
		POOL_ADD_NEW(guild_list_guid, guild_list);
	}
}

int GuildPool::guild_load(uint64_t guild_guid)
{
	if (query_map_.find(guild_guid) != query_map_.end())
	{
		return -1;
	}
	query_map_[guild_guid] = 1;
	Request *req = new Request();
	req->add(opc_query, guild_guid, new dhc::guild_t);
	game::pool()->upcall(req, boost::bind(&GuildPool::guild_load_callback, this, _1));

	return 0;
}

void GuildPool::guild_load_callback(Request *req)
{
	if (req->success())
	{
		dhc::guild_t *guild = (dhc::guild_t*)req->release_data();
		if (!guild)
		{
			return;
		}
		int &num = query_map_[guild->guid()];
		for (int i = 0; i != guild->member_guids_size(); ++i)	// load member
		{
			num++;
			Request *req = new Request();
			req->add(opc_query, guild->member_guids(i), new dhc::guild_member_t);
			game::pool()->upcall(req, boost::bind(&GuildPool::guild_msg_callback, this, _1, guild));
		}
		for (int j = 0; j != guild->event_guids_size(); ++j)	// load event
		{
			num++;
			Request *req = new Request();
			req->add(opc_query, guild->event_guids(j), new dhc::guild_event_t);
			game::pool()->upcall(req, boost::bind(&GuildPool::guild_msg_callback, this, _1, guild));
		}
		for (int j = 0; j != guild->message_guids_size(); ++j)	// load message
		{
			num++;
			Request *req = new Request();
			req->add(opc_query, guild->message_guids(j), new dhc::guild_message_t);
			game::pool()->upcall(req, boost::bind(&GuildPool::guild_msg_callback, this, _1, guild));
		}
		if (guild->mission() != 0)
		{
			num++;
			Request *req = new Request();
			req->add(opc_query, guild->mission(), new dhc::guild_mission_t);
			game::pool()->upcall(req, boost::bind(&GuildPool::guild_msg_callback, this, _1, guild));
		}
		for (int j = 0; j != guild->red_guids_size(); ++j)	// load red
		{
			num++;
			Request *req = new Request();
			req->add(opc_query, guild->red_guids(j), new dhc::guild_red_t);
			game::pool()->upcall(req, boost::bind(&GuildPool::guild_msg_callback, this, _1, guild));
		}
		for (int j = 0; j != guild->box_guids_size(); ++j)	// load box
		{
			num++;
			Request *req = new Request();
			req->add(opc_query, guild->box_guids(j), new dhc::guild_box_t);
			game::pool()->upcall(req, boost::bind(&GuildPool::guild_msg_callback, this, _1, guild));
		}
		guild_load_check_end(guild);
	}
	else
	{
		
	}
}

void GuildPool::guild_msg_callback(Request *req, dhc::guild_t *guild)
{
	if (req->success())
	{
		POOL_ADD(req->guid(), req->release_data());		// 添加到内存池实体
	}
	else
	{

	}
	guild_load_check_end(guild);
}

void GuildPool::guild_load_check_end(dhc::guild_t *guild)
{
	int &num = query_map_[guild->guid()];
	num--;
	if (num <= 0)
	{
		POOL_ADD(guild->guid(), guild);
		add_guild(guild);
		ItemOperation::refresh_guild_shop(guild);
		query_map_.erase(guild->guid());
	}
}

//////////////////////////////////////////////////////////////////////////

void GuildPool::add_guild(dhc::guild_t *guild)
{
	UpdateGuild up;
	up.guid = guild->guid();
	up.update_time = game::timer()->now();
	update_list_.push_back(up);
	guild_rank_list_.push_back(guild);
	name_map_[guild->name()] = guild->guid();

	if (guild->last_level() <= 0)
	{
		guild->set_last_level(guild->level());
	}

	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		dhc::guild_member_t *guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (!guild_member)
		{
			continue;
		}
	}

	dhc::guild_mission_t *mission = POOL_GET_GUILD_MISSION(guild->mission());
	if (mission)
	{
		add_guild_mission_compare(guild, mission);
	}

	GuildOperation::guild_refresh_check(guild);

	if (guild->pvp_guilds_size() > 0)
	{
		add_guild_pvp_sync(guild->guid());
	}

	if (guild->pvp_guild() == 0)
	{
		guild->set_pvp_guild(game::gtool()->assign(et_guild_pvp));
	}
}

// 清除一个公会
void GuildPool::remove_guild(dhc::guild_t *guild)
{
	name_map_.erase(guild->name());
	guild_rank_list_.remove(guild);
	remove_guild_mission_compare(guild->mission());
}

// 销毁公会所有信息
void GuildPool::delete_guild(dhc::guild_t *guild)
{
	remove_guild(guild);
	/// 清除成员
	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		uint64_t guild_member_guid = guild->member_guids(i);
		dhc::guild_member_t * guild_member = POOL_GET_GUILD_MEMBER(guild_member_guid);
		if (!guild_member)
		{
			continue;
		}
		dhc::player_t *tmpplayer = POOL_GET_PLAYER(guild_member->player_guid());
		if (tmpplayer)
		{
			tmpplayer->set_guild(0);
			tmpplayer->set_last_leave_guild_time(game::timer()->now());
		}
		GuildOperation::post_leave_guild(guild_member->player_guid(), guild->name(), 1);
		POOL_REMOVE(guild_member_guid, guild->guid());
	}
	/// 清除事件
	for (int i = 0; i < guild->event_guids_size(); ++i)
	{
		uint64_t guild_event_guid = guild->event_guids(i);
		dhc::guild_event_t * guild_event = POOL_GET_GUILD_EVENT(guild_event_guid);
		if (!guild_event)
		{
			continue;
		}
		POOL_REMOVE(guild_event_guid, guild->guid());
	}
	/// 清除留言板
	for (int i = 0; i < guild->message_guids_size(); ++i)
	{
		uint64_t guild_message_guid = guild->message_guids(i);
		dhc::guild_message_t * guild_message = POOL_GET_GUILD_MESSAGE(guild_message_guid);
		if (!guild_message)
		{
			continue;
		}
		POOL_REMOVE(guild_message_guid, guild->guid());
	}
	/// 清除公会活动boss
	if (guild->mission() != 0)
	{
		dhc::guild_mission_t *guild_boss = POOL_GET_GUILD_MISSION(guild->mission());
		if (guild_boss)
		{
			POOL_REMOVE(guild->mission(), guild->guid());
		}
	}
	/// 清除红包
	for (int i = 0; i < guild->red_guids_size(); ++i)
	{
		uint64_t guild_red_guid = guild->red_guids(i);
		dhc::guild_red_t * guild_red = POOL_GET_GUILD_RED(guild_red_guid);
		if (!guild_red)
		{
			continue;
		}
		POOL_REMOVE(guild_red_guid, guild->guid());
	}
	/// 清除宝箱
	for (int i = 0; i < guild->box_guids_size(); ++i)
	{
		uint64_t guild_box_guid = guild->box_guids(i);
		dhc::guild_box_t * guild_box = POOL_GET_GUILD_BOX(guild_box_guid);
		if (!guild_box)
		{
			continue;
		}
		POOL_REMOVE(guild_box_guid, guild->guid());
	}

	uint64_t guild_list_guid = MAKE_GUID(et_guild_list, 0);
	dhc::guild_list_t *guild_list = POOL_GET_GUILD_LIST(guild_list_guid);
	if (!guild_list)
	{
		return;
	}
	for (int i = 0; i < guild_list->guild_guids_size(); ++i)
	{
		if (guild_list->guild_guids(i) == guild->guid())
		{
			guild_list->mutable_guild_guids()->SwapElements(i, guild_list->guild_guids_size() - 1);
			guild_list->mutable_guild_guids()->RemoveLast();
			break;
		}
	}

	game::pool()->remove_ref(guild->guid());
	POOL_REMOVE(guild->guid(), 0);
}

int GuildPool::update(ACE_Time_Value tv)
{
	uint64_t now = game::timer()->now();
	if (game::timer()->trigger_time(last_time_, 0, 0))
	{
		std::vector<uint64_t> guild_guids;
		game::pool()->get_entitys(et_guild, guild_guids);
		for (int i = 0; i < guild_guids.size(); ++i)
		{
			dhc::guild_t *guild = POOL_GET_GUILD(guild_guids[i]);
			if (guild)
			{
				guild->set_last_daily_time(now);
				GuildOperation::guild_refresh(guild);
			}
		}
	}

	if (game::timer()->trigger_week_time(last_time_))
	{
		std::vector<uint64_t> guild_guids;
		game::pool()->get_entitys(et_guild, guild_guids);
		for (int i = 0; i < guild_guids.size(); ++i)
		{
			dhc::guild_t *guild = POOL_GET_GUILD(guild_guids[i]);
			if (guild)
			{
				guild->set_last_week_time(now);
				GuildOperation::guild_week_refresh(guild);
			}
		}
	}

	for (int t = 10; t < 22; t += 2)
	{
		if (game::timer()->trigger_time(last_time_, t, 0))
		{
			std::vector<uint64_t> guild_guids;
			game::pool()->get_entitys(et_guild, guild_guids);
			dhc::guild_t *guild = 0;
			dhc::guild_member_t *member = 0;
			for (int i = 0; i < guild_guids.size(); ++i)
			{
				guild = POOL_GET_GUILD(guild_guids[i]);
				if (guild)
				{
					for (int j = 0; j < guild->member_guids_size(); ++j)
					{
						member = POOL_GET_GUILD_MEMBER(guild->member_guids(j));
						if (member)
						{
							if (t != 10)
							{
								member->set_mbnum(member->mbnum() + 1);
							}
							else
							{
								member->clear_gongpo_rewards();
							}
							member->set_last_mbtime(now);
						}
					}
				}
			}
		}
	}
	last_time_ = now;

	/// 数据保存
	int update_time = GUILD_UPDATE_TIME;
	if (now - last_update_time_ >= update_time)
	{
		last_update_time_ = now;
		save_list(false);
	}

	int upnum = 0;
	while (upnum < 5)
	{
		if (update_list_.empty())
		{
			break;
		}
		UpdateGuild up = update_list_.front();
		dhc::guild_t *guild = POOL_GET_GUILD(up.guid);
		if (guild)
		{
			if (now - up.update_time < update_time)
			{
				break;
			}
			upnum++;
			update_list_.pop_front();
			if (!check_guild_delete(guild))
			{
				save_guild(guild->guid(), false);
				up.update_time = now;
				update_list_.push_back(up);
			}
		}
		else
		{
			update_list_.pop_front();
		}
	}

	update_guild_pvp_sync();

	// guild ranking update
	if (game::timer()->now() - last_refresh_time_ > GUILD_RANKING_UPDATE_TIME)
	{
		guild_rank_list_.sort(GuildOperation::guild_compare);
		std::sort(guild_mission_compare_.begin(), guild_mission_compare_.end(), std::greater<GuildMissionCompare>());
		last_refresh_time_ = now;
	}
	
	return 0;
}

bool GuildPool::check_guild_delete(dhc::guild_t * guild)
{
	if (guild->juntuan_apply() > 0)
	{
		return false;
	}

	bool flag = false;
	uint64_t nt = game::timer()->now();
	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		dhc::guild_member_t *guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (!guild_member)
		{
			continue;
		}
		uint64_t time = guild_member->last_sign_time();
		if (nt - time <= GUILD_DELETE_TIME)
		{
			flag = true;
			break;
		}
	}
	if (!flag)
	{
		delete_guild(guild);
		return true;
	}

	for (int i = 0; i < guild->red_guids_size(); ++i)
	{
		dhc::guild_red_t* guild_red = POOL_GET_GUILD_RED(guild->red_guids(i));
		if (guild_red)
		{
			if (nt > guild_red->time() + 3 * 24 * 60 * 60 * 1000)
			{
				POOL_REMOVE(guild->red_guids(i), guild->guid());
			}
		}
	}

	return false;
}

void GuildPool::save_all()
{
	for (std::list<UpdateGuild>::iterator it = update_list_.begin(); it != update_list_.end(); ++it)
	{
		save_guild((*it).guid, true);
	}
	save_list(true);
}

void GuildPool::save_guild(uint64_t guid, bool release)
{
	dhc::guild_t *guild = POOL_GET_GUILD(guid);
	if (guild)
	{
		for (int i = 0; i < guild->member_guids_size(); ++i)
		{
			uint64_t guild_member_guid = guild->member_guids(i);
			dhc::guild_member_t *guild_member = POOL_GET_GUILD_MEMBER(guild_member_guid);
			if (guild_member)
			{
				dhc::player_t *player = POOL_GET_PLAYER(guild_member->player_guid());
				if (player)
				{
					guild_member->set_player_iocn_id(player->template_id());
					guild_member->set_bf(player->bf());
					guild_member->set_player_level(player->level());
				}
				POOL_SAVE(dhc::guild_member_t, guild_member, release);
			}
		}

		for (int i = 0; i < guild->event_guids_size(); ++i)
		{
			uint64_t guild_event_guid = guild->event_guids(i);
			dhc::guild_event_t *guild_event = POOL_GET_GUILD_EVENT(guild_event_guid);
			if (guild_event)
			{
				POOL_SAVE(dhc::guild_event_t, guild_event, release);
			}
		}
		for (int i = 0; i < guild->message_guids_size(); ++i)
		{
			uint64_t guild_message_guid = guild->message_guids(i);
			dhc::guild_message_t *guild_message = POOL_GET_GUILD_MESSAGE(guild_message_guid);
			if (guild_message)
			{
				POOL_SAVE(dhc::guild_message_t, guild_message, release);
			}
		}
		if (guild->mission() != 0)
		{
			dhc::guild_mission_t *guild_boss = POOL_GET_GUILD_MISSION(guild->mission());
			if (guild_boss)
			{
				POOL_SAVE(dhc::guild_mission_t, guild_boss, release);
			}
		}
		for (int i = 0; i < guild->red_guids_size(); ++i)
		{
			uint64_t guild_red_guid = guild->red_guids(i);
			dhc::guild_red_t *guild_red = POOL_GET_GUILD_RED(guild_red_guid);
			if (guild_red)
			{
				POOL_SAVE(dhc::guild_red_t, guild_red, release);
			}
		}
		for (int i = 0; i < guild->box_guids_size(); ++i)
		{
			uint64_t guild_box_guid = guild->box_guids(i);
			dhc::guild_box_t *guild_box = POOL_GET_GUILD_BOX(guild_box_guid);
			if (guild_box)
			{
				POOL_SAVE(dhc::guild_box_t, guild_box, release);
			}
		}

		game::pool()->remove_ref(guild->guid());

		POOL_SAVE(dhc::guild_t, guild, release);
	}
}

void GuildPool::save_list(bool release)
{
	uint64_t guild_list_guid = MAKE_GUID(et_guild_list, 0);
	dhc::guild_list_t *guild_list = POOL_GET_GUILD_LIST(guild_list_guid);
	if (!guild_list)
	{
		return;
	}
	POOL_SAVE(dhc::guild_list_t, guild_list, release);
}

bool GuildPool::check_name(const std::string &name)
{
	return name_map_.find(name) == name_map_.end();
}

//////////////////////////////////////////////////////////////////////////

void GuildPool::get_guild_rank_list(std::list<dhc::guild_t*>& guild_list)
{
	guild_list.clear();
	int j = 0;
	for (std::list<dhc::guild_t*>::iterator iter = guild_rank_list_.begin(); iter != guild_rank_list_.end() && j != GUILD_RANKING_MAX; ++iter, ++j)
	{
		guild_list.push_back(*iter);
	}
}

void GuildMissionCompare::update(dhc::guild_mission_t* mission)
{
	if (!mission)
	{
		return;
	}
	dhc::guild_t* guild = POOL_GET_GUILD(mission->guild_guid());
	if (!guild)
	{
		return;
	}
	ceng = mission->guild_ceng();
	bot = 0;
	hp = 0;
	max_hp = 0;
	guild_level = guild->level();
	int count = 0;
	for (int i = 0; i < mission->guild_cur_hps_size(); ++i)
	{
		hp += mission->guild_cur_hps(i);
		max_hp += mission->guild_max_hps(i);
		if (mission->guild_cur_hps(i) <= 0)
		{
			count++;
		}
		if (((i + 1) % 5) == 0)
		{
			if (count == 5)
			{
				bot++;
			}
			count = 0;
		}
	}
}
void GuildPool::add_guild_mission_compare(dhc::guild_t* guild, dhc::guild_mission_t *mission)
{
	GuildMissionCompare cmp;
	cmp.guid = mission->guid();
	cmp.guild_name = guild->name();
	cmp.icon = guild->icon();
	cmp.guild_level = guild->level();
	cmp.update(mission);
	guild_mission_compare_.push_back(cmp);
}

void GuildPool::update_guild_mission_compare(dhc::guild_t* guild, dhc::guild_mission_t *mission)
{
	GuildMissionCompare cmp;
	if (mission)
	{
		cmp.guid = mission->guid();
	}
	else
	{
		cmp.guid = guild->mission();
	}
	std::vector<GuildMissionCompare>::iterator it = std::find(guild_mission_compare_.begin(),
		guild_mission_compare_.end(), 
		cmp);
	if (it != guild_mission_compare_.end())
	{
		it->guild_name = guild->name();
		it->icon = guild->icon();
		it->guild_level = guild->level();
		it->update(mission);
	}
}

void GuildPool::remove_guild_mission_compare(uint64_t guid)
{
	GuildMissionCompare cmp;
	cmp.guid = guid;
	std::vector<GuildMissionCompare>::iterator it = std::find(guild_mission_compare_.begin(),
		guild_mission_compare_.end(),
		cmp);
	if (it != guild_mission_compare_.end())
	{
		guild_mission_compare_.erase(it);
	}
}

void GuildPool::get_guild_mission_compare(protocol::game::smsg_guild_mission_ranking& rank)
{
	int count = 0;
	for (std::vector<GuildMissionCompare>::const_iterator it = guild_mission_compare_.begin();
		it != guild_mission_compare_.end();
		++it)
	{
		if (count >= GUILD_RANKING_MAX)
		{
			break;
		}
		rank.add_guild_name(it->guild_name);
		rank.add_icon(it->icon);
		rank.add_ceng(it->ceng);
		rank.add_bot(it->bot);
		rank.add_hp(it->hp);
		rank.add_max_hp(it->max_hp);
		rank.add_guild_level(it->guild_level);
	}
}

void GuildPool::add_guild_pvp_sync(uint64_t guild_guid)
{
	guild_pvp_sync_.insert(guild_guid);
	return;
}

void GuildPool::update_guild_pvp_sync()
{
	if (guild_pvp_sync_.empty())
	{
		return;
	}

	std::set<uint64_t>::iterator it = guild_pvp_sync_.begin();
	if (it == guild_pvp_sync_.end())
	{
		return;
	}

	dhc::guild_t *guild = POOL_GET_GUILD(*it);
	if (!guild)
	{
		guild_pvp_sync_.erase(it);
		return;
	}

	if (guild->pvp_guilds_size() <= 0)
	{
		guild_pvp_sync_.erase(it);
		return;
	}

	uint64_t player_guid = guild->pvp_guilds(guild->pvp_guilds_size() - 1);
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		sPlayerLoad->load_player(player_guid, 0, "");
		return;
	}

	guild->mutable_pvp_guilds()->RemoveLast();
	if (guild->pvp_guilds_size() <= 0)
	{
		guild_pvp_sync_.erase(it);
	}

	PvpOperation::sync_player(player);
	
}

void GuildPool::set_guild_pvp_week(int wk)
{
	guild_pvp_week_ = wk;
}

void GuildPool::set_guild_pvp_hour(int hour)
{
	guild_pvp_hour_ = hour;
}

int GuildPool::get_guild_pvp_week() const
{
	if (guild_pvp_week_ == 0)
		return game::timer()->weekday();
	return guild_pvp_week_;
}

int GuildPool::get_guild_pvp_hour() const
{
	if (guild_pvp_hour_ == 0)
		return game::timer()->hour();
	return guild_pvp_hour_;
}
