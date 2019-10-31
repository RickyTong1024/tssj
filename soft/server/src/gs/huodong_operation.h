#ifndef __HUODONG_OPERATION_H__
#define __HUODONG_OPERATION_H__

#include "gameinc.h"

class HuodongOperation
{
public:
	static int get_huodong_pttq_index(dhc::player_t *player, int index);

	static void check_pttq_vip(dhc::player_t *player);

	static int get_kaifu_huodong_count(dhc::player_t *player, int id);

	static int get_kaifu_huodong_id(dhc::player_t *player, int id);

	static int get_kaifu_huodong_id_count(dhc::player_t *player, int id);

	static bool has_kaifu_huodong_complete(dhc::player_t *player);

	static bool has_pttq_huodong_complete(dhc::player_t *player);

	static int get_huodong_week_libao(dhc::player_t *player, int id);

	static dhc::player_t* get_tansuo_boss(dhc::player_t *player);

	static bool refresh_mofang(std::vector<int>& idss);
};

#endif
