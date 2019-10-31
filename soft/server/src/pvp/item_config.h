#ifndef __ITEM_CONFIG_H__
#define __ITEM_CONFIG_H__

#include "gameinc.h"

struct s_t_item
{
	uint32_t id;
	std::string name;
	int font_color;
	int type;
	std::string icon;
	int level;
	int def1;
	int def2;
	int def3;
	int def4;
	int sell;
	int can_use;
	int jewel;
};

struct s_t_itemstore
{
	uint32_t id;
	int type;
	std::vector<s_t_reward> rewards;
	std::vector<int> rates;
};

struct s_t_shop_xg
{
	int id;
	int recharge;
	s_t_reward xg_item;
	int xg_level;
	int price_type;
	int price;
	int xg_type;
	int xg_num;
	std::vector<int> vip_xg_num;
};

struct s_t_shop
{
	uint32_t id;
	int gezi;
	int vip;
	int level;
	int type;
	int value1;
	int value2;
	int value3;
	int rate;
	int hb_type;
	int hb;
};

struct s_t_boss_shop
{
	int id;
	int level;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
	int hongsehuoban;
};

struct s_t_ttt_shop
{
	int id;
	int level;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
	int red_force;
	int star;
};

struct s_t_guild_shop
{
	int id;
	int type;
	int value1;
	int value2;
	int value3;
	int contribution;
	int hongsehuoban;
	int num;
	int guild_level;
};

struct s_t_sport_shop
{
	int id;
	int type;
	int value1;
	int value2;
	int value3;
	int spower;
	int level;
	int hongsehuoban;
};

struct s_t_ttt_baozang
{
	int id;
	int star;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
	int rate;
};

struct s_t_ttt_mubiao
{
	int id;
	int star;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
};

struct s_t_sport_mubiao
{
	int id;
	int rank;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
};

struct s_t_guild_mubiao
{
	int id;
	int level;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
};

struct s_t_guild_shop_xs
{
	int id;
	int level;
	int jewel;
	int rate;
	int num;
	int type;
	int value1;
	int value2;
	int value3;
};

struct s_t_item_hecheng
{
	int id;
	s_t_reward hecheng;
	std::vector<s_t_reward> cailiao;
};

struct s_t_huiyi_shop
{
	int id;
	int gezi;
	int type;
	int value1;
	int value2;
	int value3;
	int weight;
	int huobi;
	int price;
};

struct s_t_huiyi_luck_shop
{
	int id;
	int num;
	int type;
	int value1;
	int value2;
	int value3;
	int luck_point;
};

struct s_t_lieren_shop
{
	int id;
	int type;
	int value1;
	int value2;
	int value3;
	int huobi;
	int hongsehuoban;
	int hongsezhuangbei;
};

struct s_t_bingyuan_shop
{
	int id;
	int type;
	int value1;
	int value2;
	int value3;
	int bingjing;
	int num;
};

struct s_t_bingyuan_mubiao
{
	int id;
	int point;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
};

struct s_t_guild_shop_ex
{
	int id;
	int type;
	int value1;
	int value2;
	int vlaue3;
	int point;
	int price;
	int level;
};

struct s_t_chongzhifanpai_shop
{
	int id;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
};

struct s_t_chongwu_shop
{
	int id;
	int level;
	int type;
	int value1;
	int value2;
	int value3;
	int weight;
	int huobi;
	int price;
};


struct s_t_mofang_shop
{
	int id;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
	int num;
};

class ItemConfig
{
public:
	int parse();

	s_t_item * get_item(uint32_t id);

	s_t_shop * get_ramdom_shop(dhc::player_t * player, int type, int gezi);

	s_t_shop * get_random_role_shop(dhc::player_t *player, int gezi, std::vector<int>& refreshs);

	s_t_shop * get_shop(uint32_t id);

	uint32_t get_suipian(int role_id);

	uint32_t get_jiyin(int role_id);

	s_t_itemstore * get_itemstore(uint32_t id);

	s_t_shop_xg * get_shop_xg(int id);

	const s_t_ttt_baozang* get_ttt_baozhang(int id) const;

	const s_t_ttt_baozang* get_ttt_random_baozhang(int star) const;

	const s_t_boss_shop* get_boss_shop(int id) const;

	const s_t_ttt_shop* get_ttt_shop(int id) const;

	const s_t_guild_shop* get_guild_shop(int id) const;

	const s_t_sport_shop* get_sport_shop(int id) const;

	const s_t_ttt_mubiao* get_ttt_mubiao(int id) const;

	const s_t_guild_mubiao* get_guild_mubiao(int id) const;

	const s_t_sport_mubiao* get_sport_mubiao(int id) const;

	const std::map<int, s_t_guild_mubiao>& get_guild_mubiao() const { return t_guild_mubiao_; }

	const s_t_guild_shop_xs* get_guild_shop_xs(int id) const;
	int get_guild_shop_xs(int gezi, int level, const std::set<int>& ids) const;
	const s_t_guild_shop_ex* get_guild_shop_ex(int id) const;

	const s_t_item_hecheng* get_item_hecheng(int id) const;

	const s_t_huiyi_shop* get_huiyi_shop(int id) const;
	const s_t_huiyi_shop* get_huiyi_shop_random(int gezi, const std::set<int>& ids) const;
	const s_t_huiyi_luck_shop* get_huiyi_luckshop(int id) const;

	const s_t_lieren_shop* get_lieren_shop(int id) const;

	const s_t_bingyuan_shop* get_bingyuan_shop(int id) const;
	const s_t_bingyuan_mubiao* get_bingyuan_mubiao(int id) const;

	const s_t_chongzhifanpai_shop* get_chongzhifanpai_shop(int id) const;

	const s_t_chongwu_shop* get_chongwu_shop(int id) const;
	const s_t_chongwu_shop* get_chongwu_shop_random(int gezi, int level, const std::set<int>& ids) const;

	const s_t_mofang_shop* get_mofang_shop(int id) const;

private:
	int parse_boss_shop();

	int parse_ttt_shop();

	int parse_guild_shop();

	int parse_sport_shop();

	int parse_item_hecheng();

	int parse_huiyi_shop();

	int parse_lieren_shop();

	int parse_bingyuan_shop();

	int parse_chongzhifanpai_shop();

	int parse_chongwu_shop();

	int parse_mofang_shop();

private:
	std::map<uint32_t, s_t_item> t_items_;
	std::map<uint32_t, s_t_shop> t_shops_;
	std::map<int, uint32_t> suipians_;
	std::map<int, uint32_t> jiyins_;
	std::map<uint32_t, s_t_itemstore> t_itemstores_;
	std::map<int, s_t_shop_xg> t_shop_xg_;
	std::map<int, s_t_boss_shop> t_boss_shop_;
	std::map<int, s_t_ttt_shop> t_ttt_shop_;
	std::map<int, s_t_guild_shop> t_guild_shop_;
	std::map<int, s_t_sport_shop> t_sport_shop_;
	std::map<int, s_t_ttt_baozang> t_ttt_baozang_;
	std::map<int, s_t_ttt_mubiao> t_ttt_mubiao_;
	std::map<int, s_t_guild_mubiao> t_guild_mubiao_;
	std::map<int, s_t_sport_mubiao> t_sport_mubiao_;
	std::map<int, std::vector<s_t_guild_shop_xs> > t_guild_shop_xs_refresh_;
	std::map<int, s_t_guild_shop_xs> t_guild_shop_xs_;
	std::map<int, s_t_item_hecheng> t_item_hecheng_;
	std::map<int, s_t_huiyi_shop> t_huiyi_shop_;
	std::map<int, s_t_huiyi_luck_shop> t_huiyi_luck_shop_;
	std::map<int, s_t_lieren_shop> t_lieren_shop_;
	std::map<int, s_t_bingyuan_shop> t_bingyuan_shop_;
	std::map<int, s_t_bingyuan_mubiao> t_bingyuan_mubiao_;
	std::map<int, s_t_guild_shop_ex> t_guild_shop_ex_;
	std::map<int, s_t_chongzhifanpai_shop> t_chongzhifanpai_shop_;
	std::map<int, std::vector<s_t_chongwu_shop> > t_chongwu_shop_refresh_;
	std::map<int, s_t_chongwu_shop> t_chongwu_shop_;
	std::map<int, s_t_mofang_shop> t_mofang_shop_;
};

#define sItemConfig (Singleton<ItemConfig>::instance ())

#endif
