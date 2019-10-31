#ifndef __SPORT_CONFIG_H__
#define __SPORT_CONFIG_H__

#include "gameinc.h"

struct s_t_sport_rank
{
	int rank1;
	int rank2;
	int jjcpoint;
	int jewel;
	double pm_jewel;
};

struct s_t_sport_npc_sub
{
	int type;
	int id;
	int level;
	int glevel;
};

struct s_t_sport_npc_sub1
{
	int id;
	int enhance;
};

struct s_t_sport_npc
{
	int rank1;
	int rank2;
	int level1;
	int level2;
	int bf1;
	int bf2;
	std::vector<s_t_sport_npc_sub> npc_subs;
	std::vector<s_t_sport_npc_sub1> npc_subs1;
};


struct s_t_sport_card
{
	int id;
	int level1;
	int level2;
	int weight;
	s_t_reward reward;
};

class SportConfig
{
public:
	SportConfig();

	~SportConfig();

	int parse();

	s_t_sport_rank * get_sport_rank(int rank);

	std::string get_random_name();

	s_t_sport_npc * get_sport_npc(int rank);

	int get_pm_jewel(int rank1, int rank2);

	const s_t_sport_card *get_sport_shop_card(int id) const;

	int refresh_sport_card(int level, bool has_jieri);

private:
	int parse_sport_card();

private:
	std::vector<s_t_sport_rank> t_sport_rank_;
	std::vector<std::string> t_first_name_;
	std::vector<std::string> t_secend_name_;
	std::vector<s_t_sport_npc> t_sport_npc_;
	std::map<int, s_t_sport_card> sport_cards_;
};

#define sSportConfig (Singleton<SportConfig>::instance())

#endif
