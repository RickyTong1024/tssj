#ifndef __SOCIAL_CONFIG_H__
#define __SOCIAL_CONFIG_H__

#include "gameinc.h"

struct s_t_gundong
{
	int start;
	int time;
	std::string text;
	int gtime;
};

class SocialConfig
{
public:
	SocialConfig();

	~SocialConfig();

	int parse();

	std::vector<s_t_gundong> & get_gundong();

	const std::map<int, std::vector<std::pair<int, int> > >& get_social_active() const { return t_social_active_; }

private:
	std::vector<s_t_gundong> t_gundong_;
	std::map<int, std::vector<std::pair<int,int> > > t_social_active_;
};

#define sSocialConfig (Singleton<SocialConfig>::instance())

#endif
