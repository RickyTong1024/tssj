#ifndef __MISSION_OPERATION_H__
#define __MISSION_OPERATION_H__

#include "gameinc.h"

class MissionOperation
{
public:
	static int get_mission_cishu(dhc::player_t *player, int id);

	static void add_mission_cishu(dhc::player_t *player, int id, int num);

	static int add_mission_star(dhc::player_t *player, int id, int star);

	static bool mission_can_jump(dhc::player_t *player, int id);

	static void add_map_star(dhc::player_t *player, int id, int star);

	static int get_ttt_cur_mission_star(dhc::player_t *player);

	static bool can_jskill(int index, int jlevel);

	static std::pair<int, double> get_baoji();

	static int get_mission_star(dhc::player_t *player, int type);

	static void refresh_qiyu(dhc::player_t *player, int last);
};

#endif
