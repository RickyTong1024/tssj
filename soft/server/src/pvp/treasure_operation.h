#ifndef __TREASURE_OPERATION_HPP__
#define __TREASURE_OPERATION_HPP__

#include "gameinc.h"

struct s_t_treasure;
struct RobPlayer;

class TreasureOperation
{
public:
	enum
	{
		TREASURE_SLOT_SIZE = 50,
		TREASURE_EXPAND_BASE = 5,
		TREASURE_EXPAND_SIZE = 10,
	};

	static void treasure_refresh(dhc::player_t *player, bool login);

	static bool treasure_is_full(dhc::player_t *player);

	static bool treasure_is_full(dhc::player_t *player, int num);

	static bool treasure_is_rob(dhc::player_t *player);

	static dhc::treasure_t *treasure_create(dhc::player_t *player,
		uint32_t template_id,
		int enhance,
		int jilian,
		int mode);

	static dhc::treasure_t *treasure_create(dhc::player_t *player,
		const s_t_treasure *treasure_template,
		int enhance,
		int jinlian,
		int mode);

	static void treasure_destroy(dhc::player_t *player,
		dhc::treasure_t *treasure,
		int mode);

	static void treasure_destroy(dhc::player_t *player,
		uint64_t treasure_guid,
		int mode);

	static int get_treasure_jinlian_count(dhc::player_t *player, int jinlian);

	static int get_treasure_jinlian_max(dhc::player_t *player, int jinlian);

	static int get_treasure_hecheng_count(dhc::player_t *player, int color);

	static int get_treasure_max_enhance_level(dhc::player_t *player);

	static int get_treasure_max_jinlian_level(dhc::player_t *player);

	static int get_treasure_enhance_exp(dhc::player_t *player,
		dhc::treasure_t *treasure);

	static int get_treasure_enhance_pure_exp(dhc::player_t *player,
		dhc::treasure_t *treasure);

	static void create_treasure_report(dhc::player_t *player,
		dhc::player_t *target, int suipian_id, int win);

	static bool get_enhance_return(dhc::player_t *player, dhc::treasure_t *treasure, s_t_rewards& rds);

	static bool get_jilian_return(dhc::player_t *player, dhc::treasure_t *treasure, s_t_rewards& rds);
};
#endif //__TREASURE_OPERATION_HPP__