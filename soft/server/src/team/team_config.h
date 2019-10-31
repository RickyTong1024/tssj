#ifndef TEAM_CONFIG_H
#define TEAM_CONFIG_H

#include "gameinc.h"

struct s_t_duanwei
{
	int id;
	int rank;
	int point;
};

class TeamConfig
{
public:
	int parse();

	int get_duanwei(int point, int rank, int old_duanwei) const;

	int get_duanwei_point(int duanwei) const;

private:
	std::vector<s_t_duanwei> t_duanweis_;
};

#define sTeamConfig (Singleton<TeamConfig>::instance())

#endif