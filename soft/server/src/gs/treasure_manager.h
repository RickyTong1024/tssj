#ifndef __TREASURE_MANAGER_HPP
#define __TREASURE_MANAGER_HPP

#include "gameinc.h"

class TreasureManager
{
public:
	int init();

	void fini();

	void self_player_load_treasure(Packet *pck);

	void terminal_treasure_expand(const std::string &data, const std::string &name, int id);

	void terminal_treasure_enhance(const std::string &data, const std::string &name, int id);

	void terminal_treasure_jinlian(const std::string &data, const std::string &name, int id);

	void terminal_treasure_lock(const std::string &data, const std::string &name, int id);

	void terminal_treasure_hecheng(const std::string &data, const std::string &name, int id);

	void terminal_treasure_star(const std::string &data, const std::string &name, int id);

	void terminal_treasure_init(const std::string &data, const std::string &name, int id);

	void terminal_treasure_equip(const std::string &data, const std::string &name, int id);

	void terminal_treasure_ronglian(const std::string &data, const std::string &name, int id);

	void terminal_treasure_zhuzao(const std::string &data, const std::string &name, int id);

	void terminal_treasure_rob_view(const std::string &data, const std::string &name, int id);

	void terminal_treasure_rob_protect(const std::string &data, const std::string &name, int id);

	void terminal_treasure_rob_buy(const std::string &data, const std::string &name, int id);

	void terminal_treasure_rob_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_treasure_saodang(const std::string &data, const std::string &name, int id);

	void terminal_treasure_yi_saodang(const std::string &data, const std::string &name, int id);

	void terminal_treasure_point(const std::string &data, const std::string &name, int id);

	void terminal_treasure_report(const std::string &data, const std::string &name, int id);

	void terminal_treasure_report_ex(const std::string &data, const std::string &name, int id);

private:
	int terasure_timer_;
};
#endif //__TREASURE_MANAGER_HPP