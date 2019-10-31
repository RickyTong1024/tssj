#ifndef __EQUIP_OPERATION_H__
#define __EQUIP_OPERATION_H__

#include "gameinc.h"

#define EQUIP_SLOT 100

class EquipOperation
{
public:
	static dhc::equip_t * equip_create(dhc::player_t *player, uint32_t equip_id, int enhance, int jl, int mode);

	static void equip_delete(dhc::player_t *player, uint64_t equip_guid, int mode);

	static bool is_equip_full(dhc::player_t *player);

	static int get_equip_slot(dhc::player_t *player);

	static bool player_has_dress(dhc::player_t *player, int dress_id);

	static void player_dress_check_all(dhc::player_t *player);

	static int get_equip_color_count(dhc::player_t *player, int color);

	static int get_equip_enhance_count(dhc::player_t *player, int enhance);

	static int get_equip_jinlian_count(dhc::player_t *player, int jilian);

	static int get_equip_jinlian_max(dhc::player_t *player, int jilian);

	static int get_equip_gaizao_count(dhc::player_t *player, int count);

	static int get_equip_gaizao_color_max(dhc::player_t *player, int color);

	static int get_equip_baoshi_count(dhc::player_t *player, int baoshi_level);

	static bool get_enhance_return(dhc::player_t *player, dhc::equip_t *equip, s_t_rewards& rds);

	static bool get_jinlian_return(dhc::player_t *player, dhc::equip_t *equip, s_t_rewards& rds);

	static bool get_gaizao_return(dhc::player_t *player, dhc::equip_t *equip, s_t_rewards& rds);

	static bool get_shengxin_return(dhc::player_t *player, dhc::equip_t *equip, s_t_rewards& rds);
};

#endif
