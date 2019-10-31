#ifndef __SPORT_MANAGER_H__
#define __SPORT_MANAGER_H__

#include "gameinc.h"

class SportManager
{
public:
	SportManager();

	~SportManager();

	int init();

	int fini();

	int update(const ACE_Time_Value &curr);

	void terminal_sport_look(const std::string &data, const std::string &name, int id);

	void sport_look(dhc::player_t *player, const std::string &name, int id);

	void terminal_sport_top(const std::string &data, const std::string &name, int id);

	void self_player_load_sport(Packet *pck);

	void self_player_load_sport_saodang(Packet *pck);

	void terminal_sport_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_sport_saodang(const std::string &data, const std::string &name, int id);

	void terminal_sport_reward(const std::string &data, const std::string &name, int id);

	void terminal_sport_qicuo(const std::string &data, const std::string &name, int id);

private:
	int timer_;
};

#endif
