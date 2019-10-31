#ifndef __ROLE_CONFIG_H__
#define __ROLE_CONFIG_H__

#include "gameinc.h"

struct s_t_chouka
{
	uint32_t id;
	int type;
	int num;
	std::vector<int> rates;
	int bcx;
};

struct s_t_role
{
	uint32_t id;
	int job;
	std::string name;
	int font_color;
	int pinzhi;
	int max_glevel;
	std::vector<int> cs;
	std::vector<double> cscz;
	std::vector<double> cz;
	std::vector<double> czcz;
	int skill0;
	int skill1;
	int skill2;
	std::vector<int> jskills;
	std::vector<int> tskills;
	std::vector<int> kzskills;
	int exp;
	int jinghua;
	std::vector<int> jbs;
	int ycang;
	int hbb;
};

struct s_t_role_jiban
{
	int id;
	int type;
	std::vector<int> tids;
	std::vector<std::pair<int, float> > attrs;
};

struct s_t_role_jibanex
{
	int id;
	std::vector<int> tids;
	int attr;
	int value;
};

struct s_t_skill
{
	int id;
	int type;
	int attack_type;
	int target_type;
	int range;
	double attack_pe;
	double attack_pe_add;
	int mp_target_type;
	int mp;
	int mp_rate;
	std::vector<int> buffer_types;
	std::vector<int> buffer_target_types;
	std::vector<int> buffer_rounds;
	std::vector<int> buffer_attack_types;
	std::vector<double> buffer_attack_pes;
	std::vector<double> buffer_attack_pe_adds;
	std::vector<int> buffer_modify_att_types;
	std::vector<double> buffer_modify_att_vals;
	std::vector<double> buffer_modify_att_val_adds;
	int stun_rate;
	int stun_round;
	int passive_type;
	int passive_modify_att_type;
	double passive_modify_att_val;
	double passive_modify_att_val_add;
	int ex_type;
	int ex_type_val_0;
	double ex_type_val_1;
	double ex_type_val_2;
	double ex_type_val_add_1;
	double ex_type_val_add_2;
	int release_rate;

	double get_attack_pe(int level)
	{
		return attack_pe + attack_pe_add * (level - 1);
	}
};

struct s_t_tupo
{
	int id;
	int level;
	std::vector<int> suipian;
	int gold;
};

struct s_t_jinjie
{
	int id;
	int level;
	int clty;
	int clty_num;
	int clfy;
	int clfy_num;
	int clfy_num1;
	int clgj;
	int clgj_num;
	int clgj_num1;
	int clmf;
	int clmf_num;
	int clmf_num1;
	double sxper;
	int gold;
	int shuxing;
};

struct s_t_skillup
{
	int level;
	int item_num;
	int gold;
};

struct s_t_role_dress
{
	int id;
	int role;
	int glevel;
};

struct s_t_hbb
{
	int id;
	int rate;
	int color;
};

struct s_t_chongsheng
{
	int type;
	int jewel;
	int tupo;
	int jinjie;
};

struct s_t_xinqing
{
	int type;
	int dmg;
};

struct s_t_role_gongzheng
{
	int index;
	int type;
	int condition;
	std::vector<std::pair<int, float> > attrs;
};

struct s_t_role_shengpin
{
	int pinzhi;
	int next_pinzhi;
	int level;
	std::vector<double> cs;
	std::vector<double> cz;
	int color;
	int zdjnjc;
	int bdjnjc;
	int shengpinshi;
	int zhanhun;
	int hongsehuobanzhili;
	int suipian;
	int gold;
};

struct s_t_huiyin_chou
{
	int id;
	int type;
	int weight;
	int point;
};

struct s_t_huiyi_start_attr
{
	int attr;
	double val1;
	double val2;
};

struct s_t_huiyi_jihuo
{
	int id;
	std::vector<int> huiyi;
	std::vector<s_t_huiyi_start_attr> attrs;
};

struct s_t_huiyi_fate
{
	int id;
	int weight;
	int point;
};

struct s_t_huiyi_chengjiu
{
	int id;
	int val;
	int attr;
	int value;
};

struct s_t_duixing_add
{
	int attr;
	double value;
	double add;
};

struct s_t_duixing
{
	int id;
	int type;
	int level;
	std::vector<int> duiweis;
	std::vector< std::vector<s_t_duixing_add> > sx;
};

struct s_t_duixing_skill
{
	int id;
	int level;
	std::vector<std::pair<int, int> > attrs;
};

struct s_t_duixing_up
{
	int level;
	int player_level;
	int zhuangzhi;
	int gold;
};

struct s_t_guanghuan
{
	int id;
	int color;
	int att1;
	double val1;
	int att2;
	double val2;
};

struct s_t_guanghuan_skill
{
	int level;
	int type;
	int def1;
	int def2;
};

struct s_t_guanghuan_target
{
	int id;
	std::vector<int> guanghuans;
	std::vector<std::pair<int, double> > attrs;
};

struct s_t_guanghuan_level
{
	int level;
	std::vector<std::pair<int, int> > cailiaos_;
};

struct s_t_role_bskill
{
	std::vector<s_t_reward> defs;
};

struct s_t_role_bskill_up
{
	int jls;
	int gold;
};

struct s_t_pet
{
	int class_id;
	int color;
	int cshp;
	double cshpcz;
	double sxhpcz;
	int csgj;
	double csgjcz;
	double sxgjcz;
	int cswf;
	double cswfcz;
	double sxwfcz;
	int csmf;
	double csmfcz;
	double sxmfcz;
	int ptjn;
	int zdjn;
	double jjhp;
	double jjgj;
	double jjwf;
	double jjmf;
	int jjattr1;
	int jjval1;
	int jjattr2;
	int jjval2;
	int sxhpadd;
	int sxgjadd;
	int sxwfadd;
	int sxmfadd;
	double sxjnadd;
	int shouhun;
	int suipian_id;
	double guard_jiacheng;
	double on_jiacheng;
};

struct s_t_pet_jinjie
{
	int level;
	std::vector<int> cailiao;
	int gold;
	int quan;
	int extra1;
	int extra2;
	std::map<int, int> attrs;
};

struct s_t_pet_jinjie_item
{
	int att1;
	int val1;
	int att2;
	int val2;
	int att3;
	int val3;
};

struct s_t_pet_shengxin_item
{
	int suipian;
	int shitou;
	int gold;
};

struct s_t_pet_shengxing
{
	int level;
	std::vector<s_t_pet_shengxin_item> shengxing;
};

struct s_t_pet_chengjiu
{
	int pet1;
	int pet2;
	std::vector<std::pair<int, int> > attrs;
};

struct s_t_role_gaizao
{
	int id;
	int type;
	int jewel;
	int hun;
};

class RoleConfig
{
public:
	int parse();

	s_t_chouka * get_chouka(dhc::player_t* player, int type);

	s_t_role * get_role(uint32_t id);

	s_t_role_jiban * get_role_jiban(int id);

	s_t_role_jibanex * get_role_jibanex(int id);

	s_t_skill *get_skill(int id);

	uint32_t get_random_role_id(int color, const std::vector<uint32_t> &rdroles);

	s_t_tupo * get_tupo(int id);

	s_t_jinjie * get_jinjie(int id);

	double get_jinjie_point(int id);

	int get_jinjie_shuxing(int level);

	s_t_skillup * get_skillup(int level);

	const s_t_role_dress * get_role_dress(int id) const;

	void refresh_hhb(dhc::player_t *player, bool reset = true);

	const s_t_hbb* get_hbb(int color, int add, const std::set<int>& ids) const;

	const s_t_chongsheng *get_role_chongsheng(int type) const;

	s_t_xinqing * get_xinqing(int type);

	const s_t_role_gongzheng* get_role_gongzheng(int type, int level) const;

	s_t_role_shengpin* get_role_shengpin(int pinzhi);

	const s_t_huiyin_chou* get_huiyi_chou(dhc::player_t* player, int type) const;

	const s_t_huiyi_jihuo* get_huiyi_jihuo(int id) const;

	const s_t_huiyi_fate* get_huiyi_fate(int id) const;
	int get_huiyi_fate_random(const std::set<int>& lasts) const;

	void get_huiyi_chengjiu_attr(dhc::player_t* player, std::map<int, double> &attrs) const;

	s_t_duixing * get_duixing(int id);
	const s_t_duixing_up* get_duixing_up(int level) const;

	void get_duixing_skill_attr(dhc::player_t* player, std::map<int, double> &attrs) const;

	const std::map<int, s_t_role_shengpin>& get_role_all_shengpin() const { return t_role_shengpin_; }

	const s_t_guanghuan* get_guanghuan(int id) const;

	const s_t_guanghuan_level* get_guanghuan_level(int level) const;

	const s_t_guanghuan_target* get_guanghuan_target(int id) const;

	void get_guanghuan_skill_attr(dhc::player_t *player, std::map<int, double> &attrs) const;

	void get_guanghuan_skill_id(dhc::player_t *player, std::map<int, int> &ids) const;

	void get_guanghuan_target_attr(dhc::player_t *player, std::map<int, double> &attrs) const;

	const s_t_role_bskill *get_role_bskill(int role_id, int level) const;

	const s_t_role_bskill_up* get_role_bskill_up(int level) const;

	const s_t_pet* get_pet(int id) const;

	const s_t_pet_jinjie *get_pet_jinjie(int level) const;

	const s_t_pet_jinjie_item* get_pet_jinjie_item(int id) const;

	const s_t_pet_shengxing* get_pet_shengxing(int level) const;

	const std::vector<s_t_pet_chengjiu>& get_pet_chengjiu() const { return t_pet_chengjius_; }

	const s_t_role_gaizao* get_role_gaizao(int id) const;

private:
	int parse_huiyi();

	int parse_duixing();

	int parse_guanghuan();

	int parse_bskill();

	int parse_pet();
private:
	std::vector<s_t_chouka> t_choukas_;
	std::map<uint32_t, s_t_role> t_roles_;
	std::map<int, s_t_role_jiban> t_role_jiban_;
	std::map<int, s_t_role_jibanex> t_role_jibanex_;
	std::map<int, s_t_skill> t_skill_;
	std::map<int, s_t_tupo> t_tupo_;
	std::map<int, s_t_jinjie> t_jinjie_;
	std::map<int, double> t_jinjie_point_;
	std::map<int, int> t_jinjie_shuxing_;
	std::vector<s_t_skillup> t_skillup_;
	std::map<int, s_t_role_dress> t_role_dress_;
	std::vector<s_t_hbb> t_hbbs_;
	std::map<int, s_t_chongsheng> t_chongsheng_;
	std::map<int, s_t_xinqing> t_xinqing_;
	std::map<int, std::vector<s_t_role_gongzheng> > t_role_gongzheng_;
	std::map<int, s_t_role_shengpin> t_role_shengpin_;
	std::vector<s_t_huiyin_chou> t_huiyi_chou_;
	std::map<int, s_t_huiyi_jihuo> t_huiji_jihuos_;
	std::map<int, s_t_huiyi_fate> t_huiyi_fates_;
	std::vector<s_t_huiyi_chengjiu> t_huiyi_chengjius_;
	std::map<int, s_t_duixing> t_duixings_;
	std::map<int, s_t_duixing_up> t_duixing_ups_;
	std::vector<s_t_duixing_skill> t_duixing_skills_;
	std::map<int, s_t_guanghuan> t_guanghuans_;
	std::vector<s_t_guanghuan_level> t_guanghuan_levels_;
	std::map<int, s_t_guanghuan_target> t_guanghuan_targets_;
	std::map<int, std::vector<s_t_guanghuan_skill> > t_guanghuan_skill_;
	std::map<int, std::vector<s_t_role_bskill> > t_role_bskill_;
	std::vector<s_t_role_bskill_up> t_role_bskill_up_;
	std::map<int, s_t_pet> t_pets_;
	std::vector<s_t_pet_jinjie> t_pet_jinjies_;
	std::map<int, s_t_pet_jinjie_item> t_pet_jinjie_items_;
	std::vector<s_t_pet_shengxing> t_pet_shengxings_;
	std::vector<s_t_pet_chengjiu> t_pet_chengjius_;
	std::map<int, s_t_role_gaizao> t_role_gaizao_;
};

#define sRoleConfig (Singleton<RoleConfig>::instance ())

#endif
