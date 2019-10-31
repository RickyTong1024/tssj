#ifndef __BOSS_CONFIG_H__
#define __BOSS_CONFIG_H__

#include "gameinc.h"

struct s_t_boss_reward
{
	int rank1;
	int rank2;
	std::vector<s_t_reward> top_rewards;
	std::vector<s_t_reward> max_rewards;
};

struct s_t_boss_dw
{
	int level1;
	int level2;
	int dw;
	int shanghai;
};

struct s_t_boss_active
{
	int id;
	int type;
	int count;
	int jiacheng;
	s_t_reward reward;
};

struct s_t_boss_hp
{
	int level;
	double hp_base;
	double hp_inc;
};

class BossConfig
{
public:
	int parse();

	const s_t_boss_reward * get_boss_reward(int rank) const;

	const s_t_boss_active* get_boss_active(int id) const;

	const std::map<int, s_t_boss_active>& get_all_boss_active() const { return t_boss_active_;  }

	const s_t_boss_dw* get_boss_dw(int level) const;

	const s_t_boss_hp* get_boss_hp(int level) const;

private:
	std::vector<s_t_boss_reward> t_boss_reward_;
	std::map<int, s_t_boss_active> t_boss_active_;
	std::vector<s_t_boss_dw> t_boss_dw_;
	std::map<int, s_t_boss_hp> t_boss_hp_;
	std::pair<int, int> t_boss_level_;
};

#define sBossConfig (Singleton<BossConfig>::instance ())

#endif
