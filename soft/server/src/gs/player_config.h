#ifndef __PLAYER_CONFIG_H__
#define __PLAYER_CONFIG_H__

#include "gameinc.h"

struct s_t_exp
{
	int level;
	int exp;
	int tili;
	int tili_recover;
	int role_lexp;
	int role_zexp;
	int role_jexp;
	int role_rexp;
	int yuanli;
	int zhanhun;
	int dxqhzz;
	int bywin;
	int bylose;
	int pet_lexp;
	int pet_zexp;
	int pet_jexp;
	int pet_rexp;
	int type;
	int value1;
	int value2;
	int value3;
	std::string title;
	std::string text;
};

struct s_t_task
{
	uint32_t id;
	int pid;
	int type;
	int num;
	int def1;
	int def2;
	s_t_reward reward;
};

struct s_t_active
{
	int id;
	int num;
	s_t_reward reward;
	int score;
};

struct s_t_active_reward
{
	int id;
	int score;
	std::vector<s_t_reward> rewards;
};

struct s_t_vip
{
	int level;
	int recharge;
	int jewel;
	std::vector<s_t_reward> reward;
	int add_tili;
	int dj_num;
	int jy_buy_num;
	int ttt_cz_num;
	int guild_mission;
	int shop_refresh;
	int pvp;
	int huiyi;
	int guild_fight;
};


struct s_t_price
{
	int cishu;
	int dj;
	int kc;
	int jy;
	int ttt_cz;
	int hbb_refresh;
	int guild_mission;
	int change_name;
	int pvp_num;
	int bingyuan;
	int tanbao;
	int zhuanpan1;
	int zhuanpan2;
	int ts;
	int ds;
	int mofang;
	int guild_fight;
	int change_nalflag;

	std::vector<int> xgs;
};

struct s_t_recharge
{
	int id;
	int type;
	int pid;
	int vippt;
	int jewel;
	int ios_id;
};

struct s_t_first_recharge
{
	int type;
	int value1;
	int value2;
	int value3;
};

struct s_t_online_reward
{
	int time;
	std::vector<s_t_reward> rewards;
};

struct s_t_daily_sign
{
	int index;
	s_t_reward reward;
	int vip;
	s_t_reward reward1;
	int vip1;
};

struct s_t_random_event
{
	int id;
	int role_id;
	int rate;
	std::vector< std::vector<s_t_reward> > xuan_rewards;
	std::vector<int> xuan_haogan;
};


struct s_t_chenghao
{
	int id;
	int type;
	std::vector<std::pair<int, double> > attrs;
	int day;
};

struct s_t_taobao_recharge
{
	int id;
	int subid;
	int num;
};

struct s_t_create
{
	int resource;
	int value;
};

class PlayerConfig
{
public:
	int parse();

	s_t_exp * get_exp(int level);

	s_t_task * get_task(uint32_t task_id);

	s_t_active * get_active(int id);

	s_t_active_reward * get_active_reward(int id);

	int get_max_score(){ return max_score_; }

	s_t_vip * get_vip(int level);

	s_t_price * get_price(int cishu);

	s_t_recharge * get_recharge(int id);

	void get_first_recharge(std::vector<s_t_reward> & rewards);

	s_t_online_reward * get_online_reward(int index);

	s_t_daily_sign * get_daily_sign(int index);

	int get_random_event(dhc::player_t *player);

	s_t_random_event * get_random_event(int id);

	const s_t_chenghao* get_chenghao(int id) const;

	const s_t_taobao_recharge* get_taobao_recharge(int id) const;

	int get_const(int index);

	const std::vector<s_t_create> get_create() const { return t_creates_; }

private:
	std::map<int, s_t_exp> t_exps_;
	int max_level_;
	std::map<uint32_t, s_t_task> t_tasks_;
	std::map<int, s_t_active> t_active_;
	std::map<int, s_t_active_reward> t_active_reward_;
	int max_score_;
	int active_reward_num_;
	std::map<int, s_t_vip> t_vips_;
	int max_cishu_;
	std::map<int, s_t_price> t_prices_;
	std::map<int, s_t_recharge> t_recharge_;
	std::vector<s_t_reward> t_rewards_;
	std::vector<s_t_online_reward> t_online_reward_;
	std::map<int, s_t_daily_sign> t_daily_sign_;
	std::map<int, s_t_random_event> t_random_event_;
	std::map<int, s_t_chenghao> t_chenghao_;
	std::map<int, s_t_taobao_recharge> t_taobao_recharge_;
	std::vector<int> t_const_;
	std::vector<s_t_create> t_creates_;
};

#define sPlayerConfig (Singleton<PlayerConfig>::instance ())
#define gCONST(index) (sPlayerConfig->get_const(index))

#endif
