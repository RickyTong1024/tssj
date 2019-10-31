#ifndef __MISSION_MANAGER_H__
#define __MISSION_MANAGER_H__

#include "gameinc.h"

class MissionManager
{
public:
	MissionManager();

	~MissionManager();

	int init();

	int fini();

	int update(ACE_Time_Value tv);

	void terminal_mission_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_mission_reward(const std::string &data, const std::string &name, int id);

	void terminal_mission_saodang(const std::string &data, const std::string &name, int id);

	void terminal_mission_first(const std::string &data, const std::string &name, int id);

	void terminal_mission_goumai(const std::string &data, const std::string &name, int id);

	void terminal_hbb_look(const std::string &data, const std::string &name, int id);

	void terminal_hbb_refresh(const std::string &data, const std::string &name, int id);

	void terminal_hbb_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_ttt_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_ttt_sanxing(const std::string &data, const std::string &name, int id);

	void terminal_ttt_saodang(const std::string &data, const std::string &name, int id);

	void terminal_ttt_value_look (const std::string &data, const std::string &name, int id);

	void terminal_ttt_value(const std::string &data, const std::string &name, int id);

	void terminal_ttt_reward(const std::string &data, const std::string &name, int id);

	void terminal_ttt_cz(const std::string &data, const std::string &name, int id);

	//void terminal_xjbz_get(const std::string &data, const std::string &name, int id);

	//void terminal_xjbz_buy(const std::string &data, const std::string &name, int id);

	void terminal_yb_refresh(const std::string &data, const std::string &name, int id);

	void terminal_yb_gw(const std::string &data, const std::string &name, int id);

	void terminal_yb_zh(const std::string &data, const std::string &name, int id);

	void terminal_yb(const std::string &data, const std::string &name, int id);

	void terminal_yb_jiasu(const std::string &data, const std::string &name, int id);

	void terminal_yb_finish(const std::string &data, const std::string &name, int id);

	void terminal_yb_look(const std::string &data, const std::string &name, int id);

	void terminal_yb_look_ex(const std::string &data, const std::string &name, int id);

	void terminal_yb_ybq_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_yb_reward(const std::string &data, const std::string &name, int id);

	void terminal_ore_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_qiyu_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_qiyu_check(const std::string &data, const std::string &name, int id);

private:
	int timer_;
};

#endif
