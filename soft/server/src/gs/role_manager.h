#ifndef __ROLE_MANAGER_H__
#define __ROLE_MANAGER_H__

#include "gameinc.h"

class RoleManager
{
public:
	RoleManager();

	~RoleManager();

	int init();

	int fini();

	void terminal_zhenxing(const std::string &data, const std::string &name, int id);

	void terminal_duixing(const std::string &data, const std::string &name, int id);

	void terminal_duixing_up(const std::string &data, const std::string &name, int id);

	void terminal_duixing_on(const std::string &data, const std::string &name, int id);

	void terminal_houyuan(const std::string &data, const std::string &name, int id);

	void terminal_guanghuan(const std::string &data, const std::string &name, int id);

	void terminal_guanghuan_up(const std::string &data, const std::string &name, int id);

	void terminal_guanghun_init(const std::string &data, const std::string &name, int id);

	void terminal_role_equip(const std::string &data, const std::string &name, int id);

	void terminal_chouka(const std::string &data, const std::string &name, int id);

	void terminal_role_upgrade(const std::string &data, const std::string &name, int id);

	void terminal_role_tupo(const std::string &data, const std::string &name, int id);

	void terminal_role_jinjie(const std::string &data, const std::string &name, int id);

	void terminal_role_duihuan(const std::string &data, const std::string &name, int id);

	void terminal_role_suipian(const std::string &data, const std::string &name, int id);

	void terminal_role_skillup(const std::string &data, const std::string &name, int id);

	void terminal_role_shengpin(const std::string &data, const std::string &name, int id);

	void terminal_role_bskillup(const std::string &data, const std::string &name, int id);

	void terminal_role_suipian_gaizao(const std::string &data, const std::string &name, int id);

	/// 伙伴时装装备
	void terminal_role_dress_on(const std::string &data, const std::string &name, int id);

	/// 伙伴重生
	void terminal_role_init(const std::string &data, const std::string &name, int id);
	/// 伙伴分解
	void terminal_role_fenjie(const std::string &data, const std::string &name, int id);
	//心情
	void terminal_role_xq_look(const std::string &data, const std::string &name, int id);
	// 约会
	void terminal_role_yh_look(const std::string &data, const std::string &name, int id);
	// 约会选择
	void terminal_role_yh_select(const std::string &data, const std::string &name, int id);

	void terminal_role_huiyi_chou(const std::string &data, const std::string &name, int id);

	void terminal_role_huiyi_jihuo(const std::string &data, const std::string &name, int id);

	void terminal_role_huiyi_starts(const std::string &data, const std::string &name, int id);

	void termianl_role_huiyi_reset(const std::string &data, const std::string &name, int id);
	
	void terminal_role_huiyi_fate_zhanpu(const std::string &data, const std::string &name, int id);

	void terminal_role_huiyi_fate_gaiyun(const std::string &data, const std::string &name, int id);

	void terminal_role_huiyi_fate_fanpai(const std::string &data, const std::string &name, int id);

	// 一键换装
	void terminal_role_all_equip(const std::string &data, const std::string &name, int id);

	// 宠物
	void terminal_pet_on(const std::string &data, const std::string &name, int id);

	void terminal_pet_guard(const std::string &data, const std::string &name, int id);

	void terminal_pet_level(const std::string &data, const std::string &name, int id);

	void terminal_pet_jinjie(const std::string &data, const std::string &name, int id);

	void terminal_pet_star(const std::string &data, const std::string &name, int id);

	void terminal_pet_suipian(const std::string &data, const std::string &name, int id);

	void terminal_pet_fenjie(const std::string &data, const std::string &name, int id);

	void terminal_pet_init(const std::string &data, const std::string &name, int id);
};

#endif
