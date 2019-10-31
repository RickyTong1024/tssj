#ifndef _RANK_OPERATION_H_
#define _RANK_OPERATION_H_

#include "gameinc.h"

enum e_rank_type
{
	e_rank_type_level	= 0,
	e_rank_type_bf		= 1,
	e_rank_type_pt		= 2,
	e_rank_type_jy		= 3,
	e_rank_type_star	= 4,
	e_rank_type_ttt		= 5,
	e_rank_type_xssj	= 6,
	e_rank_type_exchange = 7,
	e_rank_type_misslike = 8,
	e_rank_ore			= 9,
	e_rank_huiyi		= 10,
	e_rank_tanbao_normal = 11,
	e_rank_tanbao_jy = 12,
	e_rank_zhuanpan_normal = 13,
	e_rank_zhuanpan_jy = 14,
	e_rank_tansuo_normal = 15,
	e_rank_tansuo_jy = 16,
	e_rank_ds = 17,
	e_rank_type_end
};

class RankOperation
{
public:
	static void refresh(dhc::player_t *p_player);

	static void del_player(uint64_t player_guid);

	static void check_value(dhc::player_t *player, int index, int value);

	static void login(dhc::player_t *player);

	static int get_player_rank(dhc::player_t *player, e_rank_type type);

	static void clear_rank(e_rank_type type);

	static int get_huiyi_rank(dhc::player_t *player, dhc::rank_t *src_rank, dhc::rank_t* dest_rank);

	static int get_rank(dhc::player_t *player, e_rank_type type);
};

#endif
