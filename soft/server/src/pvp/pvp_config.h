#ifndef PVP_CONFIG_H
#define PVP_CONFIG_H

#include "gameinc.h"

struct s_t_pvp_active
{
	int id;
	int num;
	int lieren_point;
};

struct s_t_pvp_reward
{
	int rank1;
	int rank2;
	std::vector<s_t_reward> rewards;
};

struct s_t_bingyuan_chenghao
{
	int id;
	int rank;
	int point;
	int cid;
};

struct s_t_bingyuan_rank_reward
{
	int rank1;
	int rank2;
	std::vector<s_t_reward> rds;
};

struct s_t_ds_duanwei
{
	int id;
	int attr1;
	int value1;
	int attr2;
	int value2;
};

struct s_t_ds_target
{
	int count;
	s_t_reward rd;
};

struct s_t_ds_reward
{
	int rank1;
	int rank2;
	std::vector<s_t_reward> rds;
};


class PvpConfig
{
public:
	int parse();

	const s_t_pvp_active* get_pvp_active(int id) const;

	const s_t_pvp_reward* get_pvp_reward(int rank) const;

	const s_t_bingyuan_chenghao* get_bingyuan_chenghao(int id) const;

	int get_bingyuan_chenghao_by_chenghao(int id) const;

	const s_t_bingyuan_rank_reward* get_bingyuan_reward(int rank) const;

	int get_chenghao(int rank, int point) const;

	int get_default_chenghao() const { return default_chenhao_; }

	const s_t_ds_duanwei* get_ds_duanwei(int id) const;

	const s_t_ds_target* get_ds_target(int id) const;

	const s_t_ds_reward* get_ds_reward(int rank) const;

private:
	int parse_lieren_active();

	int parse_lieren_reward();

	int parse_bingyuan_chenghao();

	int parse_bingyuan_reward();

	int parse_ds();

private:
	int default_chenhao_;

	std::map<int, s_t_pvp_active> t_pvp_actives_;
	std::vector<s_t_pvp_reward> t_pvp_rewards_;
	std::map<int, s_t_bingyuan_chenghao> t_bingyuan_chenghao_;
	std::map<int, int> t_bingyuan_to_chenghao_;
	std::vector<s_t_bingyuan_rank_reward> t_bingyuan_reward_;
	std::map<int, s_t_ds_duanwei> t_ds_duanwei_;
	std::map<int, s_t_ds_target> t_ds_target_;
	std::vector<s_t_ds_reward> t_ds_rewards_;
};

#define sPvpConfig (Singleton<PvpConfig>::instance())
#endif