#ifndef __EQUIP_MANAGER_H__
#define __EQUIP_MANAGER_H__

#include "gameinc.h"

class EquipManager
{
public:
	EquipManager();

	~EquipManager();

	int init();

	int fini();

	void terminal_equip_auto_enhance(const std::string &data, const std::string &name, int id);

	void terminal_equip_enhance(const std::string &data, const std::string &name, int id);

	void terminal_equip_sell(const std::string &data, const std::string &name, int id);

	void terminal_equip_gaizao(const std::string &data, const std::string &name, int id);

	void terminal_equip_gaizao_ten(const std::string &data, const std::string &name, int id);

	void terminal_equip_gaizao_buy(const std::string &data, const std::string &name, int id);

	void terminal_equip_lock(const std::string &data, const std::string &name, int id);

	void terminal_equip_kc(const std::string &data, const std::string &name, int id);

	void terminal_equip_suipian(const std::string &data, const std::string &name, int id);

	void terminal_dress_on(const std::string &data, const std::string &name, int id);

	void terminal_dress_off(const std::string &data, const std::string &name, int id);

	void terminal_dress_buy(const std::string &data, const std::string &name, int id);

	void terminal_dress_unlock(const std::string &data, const std::string &name, int id);

	void terminal_dress_unlock_achieve(const std::string& data, const std::string& name, int id);

	void terminal_equip_star(const std::string &data, const std::string &name, int id);

	void terminal_equip_jl(const std::string &data, const std::string &name, int id);

	void terminal_equip_init(const std::string &data, const std::string &name, int id);

	void terminal_equip_rongliang(const std::string &data, const std::string &name, int id);

private:
	void equip_gaizao(const std::string &data, const std::string &name, int id, int type);
};

#endif
