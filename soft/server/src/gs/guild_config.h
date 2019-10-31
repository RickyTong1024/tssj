#ifndef __GUILD_CONFIG_H__
#define __GUILD_CONFIG_H__

#include "gameinc.h"

struct s_t_guild 
{
	int level;
	int exp;
	int member_num;
	std::vector<int> box_num;
};

struct s_t_guild_icon 
{
	uint32_t id;
	std::string icon;
};

struct s_t_guild_sign
{
	uint32_t id;
	int	gold;
	int jewel;
	int exp;
	int contrubution;
	int honor;
};

struct s_t_guild_mobai
{
	int id;
	int honor;
	s_t_reward rd;
	int jindu_jc;
	int value2_jc;
};

struct s_t_guild_guai
{
	int boss_id;
	int monster_id;
};

struct s_t_guild_mission
{
	int index;
	std::vector<s_t_guild_guai> guai;
	int min_contribution;
	int max_contribution;
	int hit_contribution;
	int guild_exp;
	std::vector<std::vector<s_t_reward> > rewards;
	std::vector<s_t_reward> first_rewards;
	int level;
};

struct s_t_guild_skill
{
	int id;
	int guild_level;
	int study_level;
	int att;
	int value;
	int exp;
	int exp_jiacheng;
	int contri;
	int contri_jiacheng;
};

struct s_t_guild_hongbao_target
{
	int id;
	int type;
	int count;
	std::vector<s_t_reward> rewards;
};

struct s_t_guildfight
{
	int id;
	int color;
	int def_num;
	int chengfang_point;
	int gongpuo_exp;
	int win_point;
	int lose_point;
	int win_gongxian;
	int lose_gongxian;
	int bf_rate;
	int defense_num;
};

struct s_t_guildfight_rankreward
{
	int rank1;
	int rank2;
	std::vector<s_t_reward> rewards;
};

struct s_t_guildfight_target
{
	int id;
	int gongpuo_type;
	int nums;
	int type;
	int value1;
	int value2;
	int value3;
};

class GuildConfig
{
public:
	int parse();

	s_t_guild *get_guild(int level);

	s_t_guild_icon *get_guild_icon(uint32_t id);

	s_t_guild_sign *get_guild_sign(uint32_t id);

	const s_t_guild_mobai* get_guild_mobai(int id) const;

	const s_t_guild_mission* get_guild_mission(int ceng) const;

	const s_t_guild_skill* get_guild_skill(int id) const;

	const s_t_guild_hongbao_target* get_guild_hongbao_target(int id) const;

	const s_t_guildfight* get_guildfight(int id) const;

	const std::map<int, s_t_guildfight>& get_t_guildfight() const;

	const s_t_guildfight_target* get_guildfight_target(int id) const;

	const s_t_guildfight_rankreward* get_guildfight_rank_reward(int type, int rank) const;

private:
	std::vector<s_t_guild> t_guild_;
	std::map<uint32_t, s_t_guild_icon> t_guild_icon_;
	std::map<uint32_t, s_t_guild_sign> t_guild_sign_;
	std::map<int, s_t_guild_mobai> t_guild_mobai_reward_;
	std::map<int, s_t_guild_mission> t_guild_mission_;
	std::map<int, s_t_guild_skill> t_guild_skill_;
	std::map<int, s_t_guild_hongbao_target> t_guild_hongbao_target_;
	std::map<int, s_t_guildfight> t_guildfight_;
	std::map<int, std::vector<s_t_guildfight_rankreward> > t_guildfight_rank_reward_;
	std::map<int, s_t_guildfight_target> t_guildfight_target_;
};

#define sGuildConfig (Singleton<GuildConfig>::instance ())

#endif
