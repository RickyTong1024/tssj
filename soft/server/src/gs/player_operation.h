#ifndef __PLAYER_OPERATION_H__
#define __PLAYER_OPERATION_H__

#include "gameinc.h"

class PlayerOperation
{
public:
	static s_t_rewards* get_player_zhichong(uint64_t player_guid);
	
	static void remove_player_zhichong(uint64_t player_guid);
	
	static void add_player_zhichong(uint64_t player_guid, const s_t_rewards &rds);
	
	static std::pair<int, int>* get_player_chongzhi(uint64_t player_guid);

	static void add_player_chongzhi(uint64_t player_guid, int pid, int count);

	static void remove_player_chongzhi(uint64_t player_guid);

	static void player_login(dhc::player_t * player);

	static void client_login(dhc::player_t *player, bool login = true);

	static void player_logout(dhc::player_t * player);

	static void player_refresh_check(dhc::player_t * player);

	static void player_refresh(dhc::player_t * player);

	static void player_week_refresh(dhc::player_t * player);

	static void player_month_refresh(dhc::player_t * player);

	static void player_add_reward(dhc::player_t * player,
		s_t_rewards& strewards,
		int mode);

	static void player_add_resource(dhc::player_t *player, 
		resource::resource_t type, 
		int value, 
		int mode = -1);

	static void player_dec_resource(dhc::player_t *player,
		resource::resource_t type,
		int value,
		int mode = -1);

	static void player_recharge_log(dhc::player_t *player, int rmb, int rid, bool success);

	static int get_max_tili(dhc::player_t *player);

	static int get_level_tili(dhc::player_t *player);

	static int get_level_enegy(dhc::player_t *player);

	static void player_do_tili(dhc::player_t *player);

	static void player_do_energy(dhc::player_t *player);
	
	//static void player_do_youqingdian(dhc::player_t *player);

	static void player_do_boss(dhc::player_t *player);

	static void player_add_active(dhc::player_t *player, int type, int num);

	static int player_calc_force(dhc::player_t *player);

	static int player_recharge(dhc::player_t *player, int rid, int count);

	static int player_recharge_zc(dhc::player_t * player, int rid, int count);

	static void player_huodong(dhc::player_t *player, int id, int val);

	static void player_statistics(dhc::player_t *player, int id, int val);

	static int get_shop_refresh_flag(dhc::player_t *player);

	static void set_shop_refresh_flag(dhc::player_t *player);

	static bool check_fight_time(dhc::player_t *player);

	static void refresh_player_name_nalflag(dhc::player_t* player, const std::string& name, const int& nalflag);

	static void player_add_chenghao(dhc::player_t* player, int chenghao);

	static void player_check_chenghao(dhc::player_t *player);

	static int get_bingyuan_chenghao(dhc::player_t *player);

private:
	static void player_mod_exp(dhc::player_t * player, int exp);

	static void player_mod_vip_exp(dhc::player_t * player, int exp);
};

#endif
