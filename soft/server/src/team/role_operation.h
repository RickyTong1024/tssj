#ifndef __ROLE_OPERATION_H__
#define __ROLE_OPERATION_H__

#include "gameinc.h"

#define ROLE_SLOT 50

struct s_t_role;
struct s_t_exp;
struct s_t_pet;

enum RoleBskillType
{
	BSKILL_TYPE1 = 1,
	BSKILL_TYPE2,
	BSKILL_TYPE3,
	BSKILL_TYPE4,
	BSKILL_TYPE5,
	BSKILL_END,
};

class RoleOperation
{
public:
	static int get_role_exp(const s_t_role *role, const s_t_exp* t_exp);

	static void role_mod_exp(dhc::player_t *player, dhc::role_t * role, int exp);

	static int role_mod_skill_exp(dhc::role_t * role, int exp);

	static dhc::role_t * role_create(dhc::player_t *player, uint32_t id, int level, int glevel, int mode);

	static dhc::role_t * role_create_normal();

	static void role_delete(dhc::player_t *player, uint64_t role_guid, int mode);

	static bool has_t_role(dhc::player_t *player, int id);

	static bool has_role(dhc::player_t *player, int id);

	static bool has_zheng(dhc::player_t *player, int id);

	static bool role_has_dress(dhc::role_t *role, int dress_id);

	static void role_dress_check(dhc::player_t *player, dhc::role_t *role);

	static void role_dress_get(dhc::player_t *player, int dress_id);

	static bool player_has_guanghuan(dhc::player_t *player, int guanghuan);

	static void player_guanghuan_get(dhc::player_t *player, int guanghuan);

	static int get_role_color_count(dhc::player_t *player, int color);

	static int get_role_tupo_count(dhc::player_t *player, int level);

	static int get_role_equip_count(dhc::player_t *player, int cout);

	static int get_role_skill_count(dhc::player_t *player, int level);

	static int get_role_skill_max(dhc::player_t *player, int level);

	static int get_role_tupo_max(dhc::player_t *player, int level);

	static bool check_bskill(dhc::player_t *player, dhc::role_t *role, const s_t_reward &defs);

	static void check_bskill_count(dhc::player_t *player, int type, int count);

	static int get_role_huiyi_count(dhc::player_t *player, int role_id, int def);

	static dhc::pet_t *create_pet(dhc::player_t *player, int template_id, int mode);

	static void destroy_pet(dhc::player_t *player, dhc::pet_t *pet, int mode);

	static int get_pet_exp(const s_t_pet *pet, const s_t_exp* t_exp);

	static bool has_pet(dhc::player_t *player, int template_id);

	//////////////////////////////////////////////////////////////////////////

	static void get_role_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_org_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_jinjie_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_equip_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_equip_tz_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_treasure_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_skill_attr(dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_tdskill_attr(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_jb_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_gongzheng_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_guild_skill_attr(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_huiyi_attr(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_huiyi_chengjiu_attr(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_duixing_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs);

	static void get_role_duixing_skill_attr(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_guanghuan_attr(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_guanghuan_skill(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_guanghuan_target(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_chenghao(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_role_duanwei_attr(dhc::player_t *player, std::map<int, double> &attrs);

	static void get_pet_attr(dhc::player_t *player, dhc::pet_t *pet, std::map<int, double> &attrs, bool extra = false, bool fight = false, bool selff = false);

	static void get_role_pet_guard_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrss);

	static void get_role_pet_on_attr(dhc::player_t *player, std::map<int, double> &attrss);

	static void get_role_pet_target(dhc::player_t *player, std::map<int, double> &attrs);

	//////////////////////////////////////////////////////////////////////////

	static bool get_role_init_exp(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds);

	static bool get_role_init_tupo(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds);

	static bool get_role_init_jinjie(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds);

	static bool get_role_init_skill(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds);

	static bool get_role_init_shenpin(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds);

	static bool get_role_init_bskill(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds);

	static bool role_mod_equip(dhc::role_t *role, const std::vector<int> &indexs, const std::vector<uint64_t> &equip_guids);

	static bool role_mod_treasure(dhc::role_t *role, const std::vector<int> &indexs, const std::vector<uint64_t> &treasure_guids);

	static bool get_pet_init_level(dhc::player_t *player, dhc::pet_t *pet, s_t_rewards &rds);

	static bool get_pet_init_jinjie(dhc::player_t *player, dhc::pet_t *pet, s_t_rewards &rds);

	static bool get_pet_init_star(dhc::player_t *player, dhc::pet_t *pet, s_t_rewards &rds, bool fenjie);
};

#endif
