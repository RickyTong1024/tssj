#ifndef __MISSION_FIGHT_H__
#define __MISSION_FIGHT_H__

#include "gameinc.h"

#define TP_SKILL_NUM 12

struct s_t_skill;
struct s_t_equip_skill;

struct FightBuff
{
	int col;
	int id;
	int level;
	double attack;
	int round;
};

struct FightSkill
{
	s_t_skill *t_skill;
	int level;
	int eval0;
	double eval1;
	double eval2;

	FightSkill():
		t_skill(0)
	{

	}
};

struct FightRole
{
	int id;
	int site;
	int duiwei;
	int per;
	int font_color;
	int pinzhi;
	int pinzhi_skill;
	std::map<int, double> role_attrs;
	double cur_hp;
	int level;
	int glevel;
	int jlevel;
	int dress_id;
	int guanghuan_id;
	int skill_level;
	std::vector<int> jskill_levels;
	std::vector<const s_t_equip_skill *> equip_skills;
	std::map<int, int> guanghuan_skills;
	int cp;
	int fuhuo;
	int baozha;
	int fengxian;
	int nengliang;
	std::map<int, FightBuff> buffs;
	bool is_yj;
	std::map<int, std::vector<FightSkill> > tp_skills_map;
	bool is_boss;
	FightSkill zs_skill;
	FightSkill kz_skill;
	int mianyi;
	bool is_pet;
	int bf;
	int kzdy;
	int kzmz;
	double kzdyhp;

	FightRole()
	{
		dress_id = 0;
		guanghuan_id = 0;
		fuhuo = 0;
		baozha = 0;
		fengxian = 0;
		nengliang = 0;
		is_yj = false;
		is_boss = false;
		zs_skill.t_skill = 0;
		mianyi = 0;
		is_pet = false;
		kzdy = 0;
		kzmz = 0;
		kzdyhp = 0.0;
	}

	void init(dhc::role_t *role);

	double get_attr(int id);

	void set_cur_hp(double hp);

	s_t_skill * get_skill(int type, int pinzhi_skill);

	s_t_skill * get_tp_skill(int index);

	/// 每回合几率加属性
	double get_tp_skill1(int id);

	/// 血少于N%时加属性
	double get_tp_skill2(int id);

	/// 己方光环
	void get_tp_skill3(std::map<int, double> &attrs);

	/// 敌方光环
	void get_tp_skill4(std::map<int, double> &attrs);

	/// 攻击吸血
	bool get_tp_skill5(double &xx);

	/// 复活
	bool get_tp_skill6(double &fh);

	/// 两次攻击
	bool get_tp_skill7();

	/// 单次伤害无法超过血量的N%
	bool get_tp_skill8(double &attack, double &ychu);

	/// 每回合几率无视防御
	bool get_tp_skill9();

	/// 每回合回复全体生命
	bool get_tp_skill10(double &hp);

	/// 对特定职业伤害增加
	void get_tp_skill11(double &attack, int per);

	/// 受到特定职业伤害减少
	void get_tp_skill12(double &attack, int per);

	/// 每死一个队友增加属性
	bool get_tp_skill14();

	/// 普通攻击伤害减少
	bool get_tp_skill15(double &attack);

	/// 免疫晕眩
	bool get_tp_skill16();

	/// 死亡时造成敌方全体攻击N%的伤害
	bool get_tp_skill17(double &val);

	/// 死亡时回复全体攻击N%的生命
	bool get_tp_skill18(double &val);

	/// 对星级低于自己的伙伴增加N%的伤害
	void get_tp_skill19(double &attack, int fc1, int fc2);

	/// 受到星级低于自己的伙伴攻击时，伤害减少N%
	void get_tp_skill20(double &attack, int fc1, int fc2);

	/// 每回合概率回复全体友军1点能量
	bool get_tp_skill21();

	/// 概率反弹一定伤害
	bool get_tp_skill22(double &ft);

	/// 双倍攻击
	bool get_equip_skill1(double &attack);

	/// 毅力抵抗
	bool get_equip_skill2(double &val);

	/// 物理反弹
	bool get_equip_skill3(double &ft);

	/// 魔法反弹
	bool get_equip_skill4(double &ft);

	/// 破甲
	bool get_equip_skill5(double &val);

	/// 护甲
	bool get_equip_skill6(double &attack);

	void get_zs_skill201(double &attack, int type, int is_stun, int mp_ch);
	void get_zs_skill202(double &attack, int type, int num);
	bool get_zs_skill203(double &xx);
	bool get_zs_skill204();
	bool get_zs_skill205(double &ft, double sh);
	bool get_zs_skill206(double wf1, double wf2);
	void get_zs_skill207(double &attack, int type, int is_stun);
	int get_zs_skill208(double &gj);
	void get_zs_skill209();
	void get_zs_skill211(double &attack, int type, double sx);
	void get_zs_skill212(int type);
	bool get_zs_skill213(int type);
	void get_zs_skill214(double &attack, double hp, int is_bj);
	void get_zs_skill215_1();
	bool get_zs_skill215_2(double &hp);
	void get_zs_skill401(double sh);
	bool get_zs_skill402();
	void get_zs_skill403(double attack, std::vector<FightRole *> &range_frs);
	bool get_zs_skill404();
	void get_zs_skill405();
	void get_zs_skill406(double &attack, int is_hf);
	void get_zs_skill400(double &attack);
	bool get_zs_skill500();
	bool get_zs_skill501();
	void get_zs_skill502();
	bool get_zs_skill503(int num1, int num2);
	bool get_zs_skill504();
	void get_zs_skill505();

	bool get_kz_skill_fw(double &attack, int is_gd, int job);

	bool get_kz_skill_wl(double attack, int job);

	int get_kz_skill_mf(int job);

	void add_helo();

	void del_helo();

	void add_buff(FightBuff &buff)
	{
		buffs[buff.id] = buff;
	}

	void do_buff(int type);

	void add_nl(int num)
	{
		nengliang += num;
		if (nengliang < 0)
		{
			nengliang = 0;
		}
	}
};

struct FightZhen
{
	std::map<int, FightRole *> frs;
	FightRole * pet;

	FightZhen()
		:pet(0)
	{

	}

	void clear()
	{
		for (std::map<int, FightRole *>::iterator it = frs.begin(); it != frs.end(); ++it)
		{
			delete (*it).second;
		}
		if (pet)
		{
			delete pet;
		}
	}

	void die(int site)
	{
		if (frs.find(site) == frs.end())
		{
			return;
		}
		frs[site]->del_helo();
		delete frs[site];
		frs.erase(site);
	}

	void die(FightRole *fr)
	{
		for (std::map<int, FightRole *>::iterator it = frs.begin(); it != frs.end(); ++it)
		{
			if ((*it).second == fr)
			{
				die((*it).first);
				return;
			}
		}
	}

	bool empty()
	{
		return frs.size() == 0;
	}
};

struct FightPich
{
	std::vector<FightZhen> fzs;

	void clear()
	{
		for (int i = 0; i < fzs.size(); ++i)
		{
			fzs[i].clear();
		}
		fzs.clear();
	}

	void die(FightRole *fr)
	{
		for (int i = 0; i < fzs.size(); ++i)
		{
			fzs[i].die(fr);
		}
	}
};

struct FightSite
{
	int site;
	FightRole *fr;

	bool operator>(const FightSite &y) const
	{
		if (fr->get_attr(5) == y.fr->get_attr(5))
		{
			return site < y.site;
		}
		else
		{
			return fr->get_attr(5) > y.fr->get_attr(5);
		}
	}
};

class MissionFight
{
public:
	static void mission_clear(int fight_type);

	static FightRole * get_fight_role(dhc::player_t *player, dhc::role_t *role, int cp, int site, int duixing_id);

	static FightRole * get_fight_role_monster(int monster_id, int site, int duixing_id);

	static FightRole * get_fight_pet(dhc::player_t *player, dhc::pet_t *pet, int cp);

	static int make_fz(dhc::player_t *player, int cp, FightZhen &fz);

	static int make_fz(dhc::player_t *player, int cp, FightZhen &fz, const std::vector<std::pair<bool, int> >& cur_hps);

	static int mission_gq(dhc::player_t *player, int mission_id, int &star, bool can_jump, std::string &text);

	static int mission_sport(dhc::player_t *player, dhc::player_t *target, std::string &text);

	static int mission_rob(dhc::player_t *player, dhc::player_t *target, std::string &text);

	static int mission_ttt(dhc::player_t *player, int d_type, int d_param, const std::vector<int> &mids, std::string &text);

	static int mission_boss(dhc::player_t *player, int boss_id, double boss_hp, double boss_max_hp, double gj, double &hhp, std::string &text);

	static int mission_yb(dhc::player_t *player, dhc::player_t *target, double gw, std::string &text);

	static int mission_guild(dhc::player_t *player, dhc::guild_mission_t* guild_mission, int index, double& hp, std::string &text);

	static int mission_ore(dhc::player_t *player, int monster_id, int& hp, double& hper, std::string &text);

	static int mission_hbb(dhc::player_t *player, const std::vector<int> ids, std::string &text);
	static int mission_ts(dhc::player_t *player, dhc::player_t *target, int index, dhc::huodong_player_t *huodong_player, std::string &text);


	static int mission_bingyuan(protocol::team::team *lteam, protocol::team::team *rteam, protocol::team::bingyuan_fight_text &text);

	//////////////////////////////////////////////////////////////////////////

	static int mission_fight();

	static int mission_deal(int gindex, std::vector<FightRole *> &range_frs, int gtype);

	static int mission_die(std::vector<FightRole *> &range_frs);

	static int mission_attack(int gindex, std::vector<FightRole *> &range_frs, bool jf, int gtype);

	static int mission_hx(int site, int cp, double dmg, std::vector<FightRole *> &range_frs);

	static int mission_nl(int site, int cp, std::vector<FightRole *> &range_frs);

	static int mission_fx(int site, int cp, double dmg, std::vector<FightRole *> &range_frs, int skill_id, double gj_208);

	static int mission_bz(int site, int cp, double dmg, std::vector<FightRole *> &range_frs);

	static void mission_get_target(int type, int range, int gindex, int cp, std::vector<int> &targets, std::vector<FightRole *> &range_frs);

	static void mission_get_single_target(int row, int cp, std::vector<int> &targets, std::vector<FightRole *> &range_frs);

	static void mission_get_row_target(int row, int cp, std::vector<int> &targets, std::vector<FightRole *> &range_frs);

	static void mission_get_random_target(int num, int cp, std::vector<int> &targets, std::vector<FightRole *> &range_frs);

	//////////////////////////////////////////////////////////////////////////

	static void missoin_make_state(protocol::game::msg_fight_state *state);

	static void missoin_make_role(FightRole *fr, protocol::game::msg_fight_role *role);

	static void mission_make_tick(int type);

	static void mission_make_fail();

	static void mission_bingyuan_vec(bool end);
};

#endif
