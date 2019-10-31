#ifndef __GUILD_POOL_HPP__
#define __GUILD_POOL_HPP__

#include "gameinc.h"
#include "guild_manager.h"

#define GUILD_PERIOD 5 * 1000
#define GUILD_DELETE_TIME 7 * 86400 * 1000
#define GUILD_UPDATE_TIME 60000
#define GUILD_RANKING_UPDATE_TIME 3 * 60000

struct UpdateGuild
{
	uint64_t guid;
	uint64_t update_time;
};

struct GuildMissionCompare
{
	uint64_t guid;
	std::string guild_name;
	int icon;
	int guild_level;
	int64_t hp;
	int bot;
	int ceng;
	int64_t max_hp;

	bool operator > (const GuildMissionCompare& rhs) const
	{
		if (ceng == rhs.ceng)
		{
			if (bot == rhs.bot)
				return hp < rhs.hp;
			else
				return bot > rhs.bot;
		}
		else
		{
			return ceng > rhs.ceng;
		}	
	}

	bool operator == (const GuildMissionCompare& rhs) const
	{
		return guid == rhs.guid;
	}

	void update(dhc::guild_mission_t* mission);
};

class GuildPool
{
public:
	GuildPool();

	~GuildPool();

	int init();

	int fini();

	int update(ACE_Time_Value tv);

	// load
	void guild_list_load();

	void guild_list_load_callback(Request *req, uint64_t);

	int guild_load(uint64_t guild_guid);

	void guild_load_callback(Request *req);

	void guild_msg_callback(Request *req, dhc::guild_t *guild);

	void guild_load_check_end(dhc::guild_t *guild);

	// save
	void save_all();

	void save_guild(uint64_t guid, bool release);

	void save_list(bool release);

	//////////////////////////////////////////////////////////////////////////
	
	void add_guild(dhc::guild_t *guild);

	void remove_guild(dhc::guild_t *guild);

	void delete_guild(dhc::guild_t *guild);

	bool check_guild_delete(dhc::guild_t * guild);

	bool check_name(const std::string &name);

	//////////////////////////////////////////////////////////////////////////

	void get_guild_rank_list(std::list<dhc::guild_t*> &guild_list);

	void add_guild_mission_compare(dhc::guild_t* guild, dhc::guild_mission_t *mission);

	void update_guild_mission_compare(dhc::guild_t* guild, dhc::guild_mission_t *mission);

	void remove_guild_mission_compare(uint64_t guid);

	void get_guild_mission_compare(protocol::game::smsg_guild_mission_ranking& rank);

	void set_guild_pvp_week(int wk);
	int get_guild_pvp_week() const;

	void set_guild_pvp_hour(int hour);
	int get_guild_pvp_hour() const;

public:
	void add_guild_pvp_sync(uint64_t guild_guid);

	void update_guild_pvp_sync();
protected:
	template <typename T>
	void save_sub(T *msg, bool release);

private:
	uint64_t last_time_;
	uint64_t last_update_time_;	// 公会上次存盘时间
	uint64_t last_refresh_time_;	// 公会排行榜上次更新时间

	std::list<UpdateGuild> update_list_;	// 公会更新列表

	std::map<std::string, uint64_t> name_map_;

	std::map< uint64_t, std::map<uint64_t, int> > guild_member_index_;

	std::list<dhc::guild_t*> guild_rank_list_;	// 公会排行榜

	std::map<uint64_t, int> query_map_;

	std::vector<GuildMissionCompare> guild_mission_compare_;

	std::set<uint64_t> guild_pvp_sync_;

	int guild_pvp_week_;
	int guild_pvp_hour_;
};

#define sGuildPool (Singleton<GuildPool>::instance())

#endif
