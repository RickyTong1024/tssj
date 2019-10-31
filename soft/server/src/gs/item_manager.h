#ifndef __ITEM_MANAGER_H__
#define __ITEM_MANAGER_H__

#include "gameinc.h"

class ItemManager
{
public:
	ItemManager();

	~ItemManager();

	int init();

	int fini();

	int update(ACE_Time_Value tv);

	void terminal_item_sell(const std::string &data, const std::string &name, int id);

	void terminal_item_sell_all(const std::string &data, const std::string &name, int id);

	void terminal_item_fenjie(const std::string &data, const std::string &name, int id);

	void terminal_item_apply(const std::string &data, const std::string &name, int id);

	void terminal_item_buy(const std::string &data, const std::string &name, int id);

	void terminal_item_hecheng(const std::string &data, const std::string &name, int id);

	void terminal_shop_check(const std::string &data, const std::string &name, int id);

	void terminal_shop_refresh(const std::string &data, const std::string &name, int id);

	void terminal_shop_buy(const std::string &data, const std::string &name, int id);

	void terminal_shop_xg(const std::string &data, const std::string &name, int id);

	void terminal_shop_boss(const std::string &data, const std::string &name, int id);

	void terminal_shop_ttt(const std::string &data, const std::string &name, int id);

	void terminal_shop_guild(const std::string &data, const std::string &name, int id);

	void terminal_shop_guild_ex(const std::string &data, const std::string &name, int id);

	void terminal_shop_sport(const std::string &data, const std::string &name, int id);

	void terminal_baozang_ttt(const std::string &data, const std::string &name, int id);

	void terminal_mubiao_ttt(const std::string &data, const std::string &name, int id);

	void terminal_mubiao_guild(const std::string &data, const std::string &name, int id);

	void terminal_mubiao_sport(const std::string &data, const std::string &name, int id);

	void terminal_time_guild(const std::string &data, const std::string &name, int id);

	void terminal_shop_huiyi(const std::string &data, const std::string &name, int id);

	void terminal_shop_huiyi_luck(const std::string &data, const std::string &name, int id);

	void terminal_shop_lieren(const std::string &data, const std::string &name, int id);

	void terminal_shop_bingyuan(const std::string &data, const std::string &name, int id);

	void terminal_reward_bingyuan(const std::string &data, const std::string &name, int id);

	void terminal_shop_chongzhifanpai(const std::string &data, const std::string &name, int id);

	void terminal_shop_chongwu(const std::string &data, const std::string &name, int id);

	void terminal_shop_chongwu_refresh(const std::string &data, const std::string &name, int id);

	void terminal_shop_mofang(const std::string &data, const std::string &name, int id);

private:
	int timer_;
	uint64_t last_time_;
};

#endif
