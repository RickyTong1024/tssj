#ifndef PVP_MANAGER
#define PVP_MANAGER

#include "gameinc.h"

class PvpManager
{
public:
	int init();

	int fini();

	int  update(const ACE_Time_Value& tv);

	void start_pvp(Packet* pck);

	void terminal_pvp_view(const std::string &data, const std::string &name, int id);

	void terminal_pvp_refresh(const std::string &data, const std::string &name, int id);

	void terminal_pvp_buy(const std::string &data, const std::string &name, int id);

	void terminal_pvp_fight(const std::string &data, const std::string &name, int id);

	void terminal_pvp_active(const std::string &data, const std::string &name, int id);

	void terminal_team_enter(const std::string &data, const std::string &name, int id);

	void terminal_team_invite(const std::string &data, const std::string &name, int id);

	void terminal_bingyuan_fight(const std::string &data, const std::string &name, int id);

	void terminal_bingyuan_reward_buy(const std::string &data, const std::string &name, int id);

	void terminal_ds_num_buy(const std::string &data, const std::string &name, int id);

	void terminal_ds_fight(const std::string &data, const std::string &name, int id);

	void terminal_ds_target(const std::string &data, const std::string &name, int id);

	void terminal_ds_time_buy(const std::string &data, const std::string &name, int id);

private:
	void terminal_pvp_refresh_callback(const std::string &data, uint64_t player_guid, const std::string&name, int id);

	void terminal_pvp_fight_callback(const std::string &data, uint64_t player_guid, int index, const std::string&name, int id);

	void terminal_pvp_reward();

	void terminal_bingyuan_reward();

	void terminal_guild_reward();

	void terminal_pvp_reward_callback(const std::string &data);

	void terminal_bingyuan_reward_callback(const std::string &data);

	void terminal_guild_reward_callback(const std::string &data);

	void terminal_team_enter_callback(const std::string &data, uint64_t player_guid, const std::string& name, int id);

	void terminal_bingyuan_fight_callback(const std::string &data, uint64_t player_guid, const std::string&name, int id);

	void terminal_ds_fight_callback(const std::string &data, uint64_t player_guid, const std::string&name, int id);

	void terminal_ds_time_buy_callback(const std::string &data, uint64_t player_guid, const std::string&name, int id);

	
private:
	int timer_;
	bool start_;
	std::map<uint64_t, uint64_t> cd_time_;
};

#endif