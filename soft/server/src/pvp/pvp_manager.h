#ifndef PVP_MANAGER_H
#define PVP_MANAGER_H

#include "gameinc.h"

class PvpPool;

class PvpManager
{
public:
	PvpManager();

	~PvpManager();

	int init();

	int fini();

	int update(const ACE_Time_Value& rhs);

	void terminal_player_look(const std::string &data, const std::string &name, int id);

	void terminal_player_rank(const std::string &data, const std::string &name, int id);

	void terminal_player_push(const std::string &data, const std::string &name, int id);

	void terminal_player_pull(const std::string &data, const std::string &name, int id);

	void terminal_player_fight(const std::string &data, const std::string &name, int id);

	void terminal_player_reward(const std::string &data, const std::string &name, int id);

	void terminal_player_mofang(const std::string &data, const std::string &name, int id);

	void terminal_invite_code_gen(const std::string &data, const std::string &name, int id);

	void terminal_invite_code_input(const std::string &data, const std::string &name, int id);

	void terminal_invite_code_level(const std::string &data, const std::string &name, int id);

	void terminal_invite_code_pull(const std::string &data, const std::string &name, int id);

	//////////////////////////////////////////////////////////////////////////
	void terminal_guild_baoming(const std::string &data, const std::string &name, int id);

	void terminal_guild_look_bushu(const std::string &data, const std::string &name, int id);

	void terminal_guild_look_pipei(const std::string &data, const std::string &name, int id);

	void terminal_guild_jinrizhanji(const std::string &data, const std::string &name, int id);

	void terminal_guild_look_xiuzhan(const std::string &data, const std::string &name, int id);

	void terminal_guild_bushu(const std::string &data, const std::string &name, int id);

	void terminal_guild_fight(const std::string &data, const std::string &name, int id);

	void terminal_guild_zhankuang(const std::string &data, const std::string &name, int id);

	void terminal_guild_zhanji(const std::string &data, const std::string &name, int id);

	void terminal_guild_pvp_match(const std::string &data, const std::string &name, int id);

	void terminal_guild_pvp_reward(const std::string &data, const std::string &name, int id);

	void terminal_guild_pvp_target(const std::string &data, const std::string &name, int id);

	void self_player_load_look(Packet *pck);

private:
	PvpPool* pvp_pool_;
	int timer_id_;
};
#endif