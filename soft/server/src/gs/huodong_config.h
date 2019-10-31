#ifndef __HUODONG_CONFIG_H__
#define __HUODONG_CONFIG_H__

#include "gameinc.h"

struct s_t_huodong_pttq
{
	int id;
	int gvip;
	std::map<int, std::vector<s_t_reward> > rewards;
};

struct s_t_huodong_kf_mubiao 
{
	int id;
	int type;
	int def1;
	int def2;
	int def3;
	int def4;
	int cankao;
	std::vector<s_t_reward> rewards;
};

struct s_t_huodong_kf 
{
	int day;
	std::vector<int> ids;

	int xg_id;
	int type;
	int value1;
	int value2;
	int value3;
	int jewel;
	int count;
};

struct s_t_huodong_czjh
{
	int index;
	int level;
	int jewel;
};

struct s_t_huodong_czjhrs
{
	int index;
	int count;
	s_t_reward rd;
};

struct s_t_huodong_vip_libao
{
	int vip;
	std::vector<s_t_reward> rewards;
};

struct s_t_huodong_week_libao
{
	int id;
	int jewel;
	int num;
	int zhekou;
	int level1;
	int level2;
	std::vector<s_t_reward> rds;
};


struct s_t_tanbao_event
{
	int type;
	int value1;
	int value2;
	int value3;
	int rate;
};

struct s_t_tanbao
{
	int type;
	int shop_type;
	std::vector<s_t_tanbao_event> events;
};

struct s_t_tanbao_shop
{
	int id;
	int shop_type;
	int type;
	int value1;
	int value2;
	int value3;
	int price;
	int point;
	int num;
};

struct s_t_tanbao_mubiao
{
	int id;
	int num;
	int type;
	int value1;
	int value2;
	int value3;
};

struct s_t_tanbao_reward
{
	int rank1;
	int rank2;
	std::vector<s_t_reward> rewards;

};

struct s_t_fanpai
{
	int id;
	int jewel;
	int rate;
};

struct s_t_zhuanpan
{
	int id;
	s_t_reward reward;
	std::vector<std::pair<int, int> > rates;
};

struct s_t_zhuanpan_reward
{
	int rank1;
	int rank2;
	std::vector<s_t_reward> rewards;
};

struct s_t_tansuo
{
	int id;
	int color;
	int type;
	int value1;
	int value2;
	int value3;
	int rate;
};

struct s_t_tansuo_event
{
	int id;
	int type_id;
	int def1;
	int def2;
	int def3;
	int rate;
	std::vector<s_t_reward> rds;
};

struct s_t_tansuo_mubiao
{
	int id;
	int point;
	s_t_reward rd;
};

struct s_t_tansuo_reward
{
	int rank1;
	int rank2;
	std::vector<s_t_reward> rds;
};

struct s_t_mofang
{
	int id;
	int mtype;
	int type;
	int value1;
	int value2;
	int value3;
	int rate;
};

struct s_t_mofang_target
{
	int id;
	int point;
	int type;
	int value1;
	int value2;
	int value3;
};

struct s_t_yueka
{
	int index;
	int type;
	int value1;
	int value2;
	int value3;
	int type2;               
	int value4;             
	int value5;             
	int value6;             
};

struct s_t_huigui_buzhu
{
	int id;
	int tap;
	int def;
	int type1;
	int value1;
	int value2;
	int value3;
	int type2;
	int value4;
	int value5;
	int value6;
	int type3;
	int value7;
	int value8;
	int value9;
};

struct s_t_huigui_fuli
{
	int id;
	int tap;
	int def1;
	int def2;
	int def3;
	int def4;
	int type;
	int value1;
	int value2;
	int value3;
};

struct s_t_huigui_haoli
{
	int id;
	int tap;
	int def;
	int type1;
	int value1;
	int value2;
	int value3;
	int type2;
	int value4;
	int value5;
	int value6;
};

class HuodongConfig
{
public:
	int parse();

	const s_t_huodong_pttq * get_t_huodong_pttq(int id) const;
	const s_t_huodong_pttq * get_t_huodong_pttq_by_vip(int vip) const;
	const std::map<int, s_t_huodong_pttq>& get_t_all_huodong_pttq() const;
	int get_t_huodong_pttq_min_vip() const { return pttq_min_vip_; }
	int get_t_huodong_pttq_min_id() const { return pttq_min_id_; }

	void get_t_huodong_all_kaifu_xg(std::vector <std::pair<int, int> > &xgs) const;
	const s_t_huodong_kf* get_t_huodong_kaifu_xg(int day) const;
	const s_t_huodong_kf * get_t_huodong_kf(int id) const;
	void get_t_huodong_kf(int day, std::vector<int> &ids) const;
	const s_t_huodong_kf_mubiao *get_t_huodong_kf_mubiao(int id) const;

	const s_t_huodong_czjh * get_t_huodong_czjh(int index) const;
	const s_t_huodong_czjhrs * get_t_huodong_czjhrs(int index) const;

	const s_t_huodong_vip_libao* get_t_huodong_vip_libao(int vip) const;

	const s_t_huodong_week_libao* get_t_huodong_week_libao(int id) const;

	const s_t_tanbao* get_t_tanbao(int gezi) const;
	int get_t_tanbao_event(const s_t_tanbao* tanbao) const;

	const s_t_tanbao_shop* get_t_tanbao_shop(int id) const;
	const s_t_tanbao_mubiao* get_t_tanbao_mubiao(int id) const;

	const s_t_tanbao_reward* get_t_tanbao_reward(int rank, int type) const;

	const s_t_fanpai* get_t_fanpai(int type, const std::set<int>& ids) const;

	const s_t_zhuanpan* get_t_zhuanpan(int type, int jewel) const;

	const s_t_zhuanpan_reward* get_t_zhuanpan_reward(int rank, int type) const;

	const s_t_tansuo* get_t_random_tansuo() const;

	const s_t_tansuo_event* get_t_tansuo_random_event(int tid, int type = -1, int qid = -1) const;

	const s_t_tansuo_event* get_t_tansuo_event(int tid, int qid) const;

	const s_t_tansuo_mubiao* get_t_tansuo_mubiao(int id) const;

	const s_t_tansuo_reward* get_t_tansuo_reward(int rank, int type) const;

	const s_t_mofang* get_t_mofang_refresh(int type) const;

	const s_t_mofang* get_t_mofang_random(const std::list<int>& ids) const;

	const s_t_mofang* get_t_mofang(int id) const;

	const s_t_mofang_target* get_t_mofang_target(int id) const;

	const s_t_yueka* get_t_yueka(int day) const;

	const s_t_huigui_buzhu* get_t_huigui_buzhu(int id) const;

	const s_t_huigui_fuli* get_t_huigui_fuli(int id) const;

	const s_t_huigui_haoli* get_t_huigui_haoli(int id) const;

	const std::map<int,s_t_huigui_fuli>& get_t_fuli_num() const;
private:
	int parse_pttq();

	int parse_czjh();

	int parse_kf();

	int parse_vip_libao();

	int parse_week_libao();

	int parse_tanbao();

	int parse_czfp();

	int parse_szzp();

	int parse_tansuo();

	int parse_mofang();

	int parse_yueka();

	int parse_huigui();
private:
	std::map<int, s_t_huodong_pttq> t_huodong_pttq_;
	int pttq_min_vip_;
	int pttq_min_id_;
	std::map<int, s_t_huodong_kf> t_huodong_kf_;
	std::map<int, s_t_huodong_kf_mubiao> t_huodong_kf_target_;
	std::map<int, s_t_huodong_czjh> t_huodong_czjh_;
	std::map<int, s_t_huodong_czjhrs> t_huodong_czjh_count_;
	std::vector<s_t_huodong_vip_libao> t_huodong_vip_libao_;
	std::map<int, s_t_huodong_week_libao> t_huodong_week_libao_;
	std::map<int, s_t_tanbao> t_huodong_tanbao_;
	std::map<int, s_t_tanbao_mubiao> t_huodong_tanbao_mubiao_;
	std::map<int, s_t_tanbao_shop> t_huodong_tanbao_shop_;
	std::map<int, std::vector<s_t_tanbao_reward> > t_huodong_tanbao_rewards;
	std::map<int, std::vector<s_t_fanpai> > t_huodong_fanpai_;
	std::map<int, std::vector<s_t_zhuanpan> > t_huodong_zhuanpan_;
	std::map<int, std::vector<s_t_zhuanpan_reward> > t_zhuanpan_rewards_;
	std::map<int, s_t_tansuo> t_tansuos_;
	std::map<int, std::map<int, s_t_tansuo_event> > t_tansuo_events_;
	std::map<int, s_t_tansuo_mubiao> t_tansuo_mubiaos_;
	std::map<int, std::vector<s_t_tansuo_reward> > t_tansuo_rewards_;
	std::vector<s_t_mofang> t_mofang_reward_;
	std::map<int, s_t_mofang_target> t_mofang_target_;
	std::vector<s_t_yueka>t_yueka_;
	std::map<int, s_t_huigui_buzhu> t_huigui_buzhu_;
	std::map<int, s_t_huigui_fuli> t_huigui_fuli_;
	std::map<int, s_t_huigui_haoli> t_huigui_haoli_;
};

#define sHuodongConfig (Singleton<HuodongConfig>::instance ())

#endif
