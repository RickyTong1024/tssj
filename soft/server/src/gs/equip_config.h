#ifndef __EQUIP_CONFIG_H__
#define __EQUIP_CONFIG_H__

#include "gameinc.h"

struct s_t_equip_attr
{
	int attr;
	int value;
};

struct s_t_equip_random_attr
{
	int attr;
	double value1;
	double value2;
};

struct s_t_equip
{
	uint32_t id;
	int font_color;
	int type;
	std::string icon;
	int slot_num;
	int sell;
	int sell_item_num;
	s_t_equip_attr eattr;
	std::vector<s_t_equip_attr> ejlattr;
	std::vector<s_t_equip_random_attr> eeattr;
};

struct s_t_gaizao
{
	int font_color;
	int item_id;
	int max_num;
	std::vector<int> gaizaoshi;
	std::vector<int> gaizao_rate;
};

struct s_t_equip_sx
{
	int level;
	int color;
	int mw_point;
	double enhance_rate;
	int suipian;
	int gold;
};

struct  s_t_equip_jl
{
	int level;
	std::vector<std::pair<int, int> > consume;
};

struct s_t_dress
{
	int id;
	int type;
	int color;
	int attr1;
	int value1;
	int attr2;
	int value2;
	int attr3;
	int value3;
	int attr4;
	int value4;
	int attr5;
	int value5;
};

struct s_t_dress_target
{
	int id;
	int type;
	std::vector<int> defs;
	int attr1;
	int value1;
	int attr2;
	int value2;
	int attr3;
	int value3;
	int attr4;
	int value4;
};

struct s_t_dress_unlock
{
	int id;
	int last_id;
	int num;
};

struct s_t_equip_tz
{
	int id;
	std::vector<int> equip_ids;
	int attr1;
	int value1;
	int attr2;
	int value2;
	int attr3;
	int value3;
	int attr4;
	int value4;

	bool has_equip_id(int eid)
	{
		for (int i = 0; i < equip_ids.size(); ++i)
		{
			if (equip_ids[i] == eid)
			{
				return true;
			}
		}
		return false;
	}
};

struct s_t_equip_suipian
{
	int equip_id;
	int suipian_id;
	int suipian_num;
	int suipian_price;
	bool suipian_specail;
};

struct s_t_equip_skill
{
	int id;
	int bw;
	int jl;
	int type;
	int def1;
	int def2;
};

class EquipConfig
{
public:
	int parse();

	s_t_equip * get_equip(uint32_t id);

	int get_enhance(int color, int level);

	int get_enhance_total(int color, int level);

	s_t_gaizao * get_gaizao(int font_color);

	s_t_dress * get_dress(int id);

	const s_t_dress_target* get_dress_target(int id) const;

	void get_player_dress_attrs(dhc::player_t *player, std::map<int, double> &attrs);

	s_t_equip_tz * get_equip_tz(int equip_id);

	s_t_equip_sx * get_equip_sx(int color, int level);

	const s_t_equip_jl* get_equip_jl(int level) const;

	const s_t_equip_suipian* get_equip_suipian(int equip_id) const;

	const s_t_dress_unlock* get_dress_unlock(int dress_id) const;

	const std::map<int, s_t_dress_unlock>& get_all_dress_unlock() const { return t_dress_unlock_; }

	const s_t_equip_skill * get_equip_skill(int id) const;

	void get_equip_skills(int bw, int jl, std::vector<int> &ids);

private:
	int parse_equip_suipian();

	int parse_dress_unlock();
private:
	std::map<uint32_t, s_t_equip> t_equips_;
	std::vector< std::vector<int> > t_enhances_;
	std::vector< std::vector<int> > t_enhance_totals_;
	std::vector<s_t_gaizao> t_gaizaos_;
	std::map<int, s_t_dress> t_dress_;
	std::map<int, s_t_dress_target> t_dress_target_;
	std::map<int, s_t_equip_tz> t_equip_tz_;
	std::map<int, int> t_equip_tz_map_;
	std::map<int, std::map<int, s_t_equip_sx> > t_equip_sx_;
	std::map<int, s_t_equip_jl> t_equip_jinlian_;
	std::map<int, s_t_equip_suipian> t_equip_suipian_;
	std::map<int, s_t_dress_unlock> t_dress_unlock_;
	s_t_equip_suipian t_suipian_special_;
	std::map<int, s_t_equip_skill> t_equip_skill_;
};

#define sEquipConfig (Singleton<EquipConfig>::instance ())

#endif
