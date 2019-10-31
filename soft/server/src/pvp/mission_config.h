#ifndef __MISSION_CONFIG_H__
#define __MISSION_CONFIG_H__

#include "gameinc.h"

struct s_t_mission_item
{
	s_t_reward reward;
	int rate;
};

struct s_t_mission
{
	uint32_t id;
	std::string name;
	int map;
	std::string cjname;
	int type;
	int jslc;
	int lch;
	int jyjslc;
	int jylch;
	int bf;
	int tili;
	int cishu;
	int yj;
	int yjguan;
	int qyboss;
	int tkezhi;
	std::vector<int> dxs;
	std::vector<int> monsters;
	std::vector<s_t_mission_item> tmi;
};

struct s_t_mission_first
{
	int id;
	std::vector<s_t_reward> rewards;
};

struct s_t_monster
{
	int id;
	int class_id;
	int level;
	int jlevel;
	int glevel;
	int pinzhi_skill;
	int jy_level;
	int skill_level;
	double hp;
	int gj;
	int wf;
	int mf;
	int sd;
	int bj;
	int gd;
	int wm;
	int mm;
	int mz;
	int sb;
	int kb;
	int ct;
	int zs;
	int js;
	int snl;
};

struct s_t_star_reward
{
	int star_num;
	std::vector<s_t_reward> rewards;
};
struct s_t_map
{
	uint32_t id;
	int qy_boss_guan;
	std::vector<s_t_star_reward> star_rewards;
};

struct s_t_ts
{
	int id;
	int ts_type;
	s_t_reward reward;
	int rate;
	int level1;
	int level2;
};

struct s_t_ttt
{
	int index;
	std::vector< std::vector<int> > monster_ids;
	std::vector<int> mw_point;
	std::vector<int> shi;
	int d_type;
	int d_param;
};

struct s_t_ttt_reward
{
	int index;
	std::vector< std::vector<s_t_reward> > rewards;
};

struct s_t_ttt_value
{
	int id;
	int type;
	int star;
	int sxtype;
	int sxvalue;
};

struct s_t_xjbz
{
	int site;
	int type;
};

struct s_t_yb
{
	int type;
	std::string name;
	int time;
	int rate;
	int yuanli_bs;
	int yuanli_ybq_per;
	int yuanli_ybq_min_per;
};

struct s_t_yb_gw
{
	int index;
	double gj;
	int jewel;
};

struct s_t_ore
{
	int index;
	int monster_id;
	int level;
	int tili;
	int bd_gold;
	int hx_gold;
	int jl_num;
	int zj_num;
	int sz_num;
};

struct s_t_mrt
{
	int id;
	int mission_id;
	int tian;
	int jiyin;
	int suipian;
};

struct s_t_qingyu
{
	int id;
	int zhuangzhi;
	int tili;
};

class MissionConfig
{
public:
	int parse();

	s_t_mission * get_mission(uint32_t id);

	s_t_monster * get_monster(int id);

	s_t_map *get_map(uint32_t id);

	s_t_ts * get_ts(int id);

	s_t_ts * get_random_ts(dhc::player_t *player);

	s_t_ttt * get_ttt(int index);

	s_t_ttt_reward * get_ttt_reward(int index);

	s_t_ttt_value * get_ttt_value(int id);

	int get_random_ttt_value(int type);

	s_t_xjbz * get_xjbz(int site);

	const s_t_ts * get_random_ts_by_type(dhc::player_t *player, int type) const;

	s_t_yb * get_yb(int type);

	s_t_yb_gw * get_yb_gw(int index);

	s_t_ore * get_t_ore(int index);

	s_t_mrt * get_t_mrt(int id);

	int get_ttt_max() const { return t_ttt_.size(); }

	const s_t_mission_first* get_mission_first(int id) const;

	void get_yiqu_boss(dhc::player_t *player, int last) ;
	const s_t_qingyu* get_qiyu(int id) const;

private:
	std::map<uint32_t, s_t_mission> t_missions_;
	std::map<int, s_t_monster> t_monsters_;
	std::map<uint32_t, s_t_map> t_maps_;
	std::map<int, s_t_ts> t_ts_;
	std::map<int, s_t_ttt> t_ttt_;
	std::map<int, s_t_ttt_reward> t_ttt_rewards_;
	std::map<int, s_t_ttt_value> t_ttt_values_;
	std::map<int, s_t_xjbz> t_xjbzs_;
	std::map<int, s_t_yb> t_yb_;
	std::map<int, s_t_yb_gw> t_yb_gw_;
	std::map<int, s_t_ore> t_ore_;
	std::map<int, s_t_mrt> t_mrt_;
	std::map<int, s_t_mission_first> t_mission_first_;
	std::map<int, s_t_qingyu> t_qiyu_;
};

#define sMissionConfig (Singleton<MissionConfig>::instance ())

#endif
