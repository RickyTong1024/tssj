#ifndef __TREASURE_CONFIG_H__
#define __TREASURE_CONFIG_H__

#include "gameinc.h"

struct s_t_treasure_att
{
	int att;
	double val;
};

struct s_t_treasure
{
	int id;
	int color;
	int type;
	int exp;
	int sell_price;
	s_t_treasure_att att1;
	s_t_treasure_att att2;
	s_t_treasure_att jl_att1;
	s_t_treasure_att jl_att2;
	s_t_treasure_att jl_att3;
	std::vector<int> suipian;
};

struct s_t_treasure_enhance
{
	int level;
	std::vector<int> attrs;
};

struct s_t_treasure_jinlian
{
	int level;
	int item_num;
	int gold;
	int baowu_num;
};

struct s_t_treasure_suipian
{
	int ordernum;
	int template_id;
	int player_rate;
	int npc_rate;
	int treasure_id;
};

struct s_t_treasure_sx
{
	int level;
	int color;
	int gold;
	int jewel;
	int jindu;
	int rate;
	int value1;
	int value2;
	int valuemax;
};

class TreasureConfig
{
public:
	int parse();

	const s_t_treasure* get_treasure(int id) const;

	const s_t_treasure_enhance * get_enhance(int level) const;

	const s_t_treasure_jinlian * get_jinlian(int level) const;

	const s_t_treasure_suipian * get_suipian(int id) const;

	const s_t_treasure_sx* get_sx(int level, int color) const;

	const std::map<int, s_t_treasure_suipian>& get_all_suipian() const;

	const std::map<int, s_t_treasure>& get_all_treasure() const;

	int get_enhance_max() const;

private:
	int parse_treasure();

	int parse_treasure_enhance();

	int parse_treasure_jinlian();

	int parse_treasure_suipian();

	int parse_treasure_sx();

private:
	std::map<int, s_t_treasure> t_treasures_;
	std::map<int, s_t_treasure_enhance> t_enhances_;
	std::map<int, s_t_treasure_jinlian> t_jinlians_;
	std::map<int, s_t_treasure_suipian> t_suipians_;
	std::map<int, int> t_treasure_tz_map_;
	std::map<int, std::map<int, s_t_treasure_sx> > t_treasure_sx_;
};

#define sTreasureConfig (Singleton<TreasureConfig>::instance())

#endif //__TREASURE_CONFIG_H__
