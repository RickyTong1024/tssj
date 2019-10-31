#ifndef __ITEM_OPERATION_H__
#define __ITEM_OPERATION_H__

#include "gameinc.h"

class ItemOperation
{
public:
	static void item_add_template(dhc::player_t *player, uint32_t item_id, int32_t item_amount, int mode);

	static int item_num_templete(dhc::player_t *player, uint32_t item_id);

	static void item_destory_templete(dhc::player_t *player, uint32_t item_id, uint32_t item_amount, int mode);

	static void refresh_role_shop(dhc::player_t *player);

	static void refresh_huiyi_shop(dhc::player_t *player);

	static std::string get_color(int color, const std::string &name);

	static void item_do_role_shop(dhc::player_t *player);

	static void refresh_guild_shop(dhc::guild_t* guild);

	static void refresh_guild_shop(dhc::player_t* player);

	static int get_guild_shop_buy_num(dhc::player_t* player, int id);

	static void add_guild_shop_buy_num(dhc::player_t* player, int id, int num);

	static int get_huiyi_luckshop_buy_num(dhc::player_t* player, int id);

	static void add_huiyi_luckshop_buy_num(dhc::player_t* player, int id, int num);

	static void do_pet_shop(dhc::player_t *player);

	static void refresh_pet_shop(dhc::player_t *player);
};

#endif
