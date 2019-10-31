#include "mission_fight.h"
#include "mission_config.h"
#include "equip_config.h"
#include "role_config.h"
#include "role_operation.h"
#include "item_config.h"
#include "mission_operation.h"
#include "utils.h"
#include "equip_config.h"
#include "treasure_config.h"

FightPich g_fp1;
FightPich g_fp2;
std::map<int, FightRole *> g_yj;
int g_pich1 = 0;
int g_pich2 = 0;
int g_round = 0;
int g_die_role = 0;
double g_total_hp = 0;
std::map<int, double> cp_attrs0;
std::map<int, double> cp_attrs1;
std::map<int, double> ttt_attrs;
double g_gj1 = 1.0f;
double g_sm1 = 1.0f;
double g_gj2 = 1.0f;
double g_sm2 = 1.0f;
protocol::game::msg_fight_text g_text;
protocol::game::msg_fight_tick *g_tick;
int g_max_round = 20;
int g_min_hp = -1;
int g_max_die_role = 9999;
// 0是人 1是怪 2是组队跨服
int g_fight_type = 0;
std::vector<double> g_bingyuan_vec;
std::list<int> g_ewxd;

void FightRole::init(dhc::role_t *role)
{
	tp_skills_map.clear();
	for (int sk = 0; sk < TP_SKILL_NUM; ++sk)
	{
		s_t_skill *t_tp_skill = get_tp_skill(sk);
		if (t_tp_skill && t_tp_skill->type == 3)
		{
			FightSkill fs;
			fs.t_skill = t_tp_skill;
			fs.level = jskill_levels[sk];
			fs.eval0 = t_tp_skill->ex_type_val_0;
			fs.eval1 = t_tp_skill->ex_type_val_1 + t_tp_skill->ex_type_val_add_1 * fs.level;
			fs.eval2 = t_tp_skill->ex_type_val_2 + t_tp_skill->ex_type_val_add_2 * fs.level;
			tp_skills_map[t_tp_skill->ex_type].push_back(fs);
		}
	}

	s_t_role *t_role = sRoleConfig->get_role(id);
	if (t_role && t_role->skill2 && (!role || role->bskill_level()))
	{
		zs_skill.t_skill = sRoleConfig->get_skill(t_role->skill2);
		if (zs_skill.t_skill)
		{
			zs_skill.level = role ? role->bskill_level() : 1;
			zs_skill.eval0 = zs_skill.t_skill->ex_type_val_0;
			zs_skill.eval1 = zs_skill.t_skill->ex_type_val_1 + zs_skill.t_skill->ex_type_val_add_1 * (zs_skill.level - 1);
			zs_skill.eval2 = zs_skill.t_skill->ex_type_val_2 + zs_skill.t_skill->ex_type_val_add_2 * (zs_skill.level - 1);
		}
	}

	if (t_role)
	{
		int qlevel = glevel - 10;
		if (qlevel >= 0)
		{
			if (qlevel >= t_role->kzskills.size())
			{
				qlevel = t_role->kzskills.size() - 1;
			}
			kz_skill.t_skill = sRoleConfig->get_skill(t_role->kzskills[qlevel]);
			if (kz_skill.t_skill)
			{
				kz_skill.eval0 = kz_skill.t_skill->ex_type_val_1;
				kz_skill.eval1 = kz_skill.t_skill->ex_type_val_2;
			}
		}
	}

	if (!role)
	{
		return;
	}

	for (int j = 0; j < role->zhuangbeis_size(); ++j)
	{
		dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
		if (!equip)
		{
			continue;
		}
		s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
		if (!t_equip)
		{
			continue;
		}
		if (t_equip->font_color == 5)
		{
			std::vector<int> ids;
			sEquipConfig->get_equip_skills(t_equip->type, equip->jilian(), ids);
			const s_t_equip_skill * es = 0;
			for (int i = 0; i < ids.size(); ++i)
			{
				const s_t_equip_skill *t_equip_skill = sEquipConfig->get_equip_skill(ids[i]);
				if (t_equip_skill->type != 1)
				{
					es = t_equip_skill;
				}
			}
			if (es)
			{
				equip_skills.push_back(es);
			}
		}
	}

	for (int j = 0; j < role->treasures_size(); ++j)
	{
		dhc::treasure_t *treasure = POOL_GET_TREASURE(role->treasures(j));
		if (!treasure)
		{
			continue;
		}
		const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
		if (!t_treasure)
		{
			continue;
		}

		if (t_treasure->color == 5)
		{
			std::vector<int> ids;
			sEquipConfig->get_equip_skills(t_treasure->type + 4, treasure->jilian(), ids);
			const s_t_equip_skill * es = 0;
			for (int i = 0; i < ids.size(); ++i)
			{
				const s_t_equip_skill *t_equip_skill = sEquipConfig->get_equip_skill(ids[i]);
				if (t_equip_skill->type != 1)
				{
					es = t_equip_skill;
				}
			}
			if (es)
			{
				equip_skills.push_back(es);
			}
		}
	}
}

bool judge(const std::pair<double,int> a, const std::pair<double, int> b)
{
	return a.first < b.first;
}

double FightRole::get_attr(int id)
{
	double val = role_attrs[id];
	val += get_tp_skill1(id);
	val += get_tp_skill2(id);
	for (std::map<int, FightBuff>::iterator it = buffs.begin(); it != buffs.end(); ++it)
	{
		FightBuff &buff = (*it).second;
		s_t_skill *t_skill = sRoleConfig->get_skill(buff.id);
		if (t_skill && t_skill->buffer_types[buff.col] == 2 && t_skill->buffer_modify_att_types[buff.col] == id)
		{
			val += t_skill->buffer_modify_att_vals[buff.col] + t_skill->buffer_modify_att_val_adds[buff.col] * (buff.level - 1);
		}
	}
	if (cp == 0)
	{
		val += cp_attrs0[id];
		val += ttt_attrs[id];
		if (id == 6)
		{
			val *= g_sm1;
		}
		else if (id == 7)
		{
			val *= g_gj1;
		}
	}
	else
	{
		val += cp_attrs1[id];
		if (id == 6)
		{
			val *= g_sm2;
		}
		else if (id == 7)
		{
			val *= g_gj2;
		}
	}
	if (id >= 1 && id <= 5)
	{
		return val * (1 + get_attr(id + 5) * 0.01f);
	}
	return val;
}

void FightRole::set_cur_hp(double hp)
{
	cur_hp = hp;
	if (cur_hp > get_attr(1))
	{
		cur_hp = get_attr(1);
	}
}

s_t_skill * FightRole::get_skill(int type, int pinzhi_skill)
{
	s_t_role *t_role = sRoleConfig->get_role(id);
	if (!t_role)
	{
		return 0;
	}

	if (is_pet)
	{
		if (pinzhi_skill >= 0)
		{
			s_t_skill *t_skill = sRoleConfig->get_skill(t_role->skill1 + pinzhi_skill);
			if (t_skill && Utils::get_int32(0, 99) < t_skill->release_rate)
			{
				return t_skill;
			}
		}
	}
	if (type == 0)
	{
		return sRoleConfig->get_skill(t_role->skill0);
	}
	else
	{
		return sRoleConfig->get_skill(t_role->skill1 + pinzhi_skill);
	}
	return 0;
}

s_t_skill * FightRole::get_tp_skill(int index)
{
	s_t_role *t_role = sRoleConfig->get_role(id);
	if (!t_role)
	{
		return 0;
	}
	if (index < 0 || index >= t_role->jskills.size())
	{
		return 0;
	}
	if (!MissionOperation::can_jskill(index, jlevel))
	{
		return 0;
	}
	return sRoleConfig->get_skill(t_role->jskills[index]);
}

double FightRole::get_tp_skill1(int id)
{
	for (int i = 0; i < tp_skills_map[1].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[1][i];
		if (fs.eval0 == id)
		{
			if (Utils::get_int32(0, 99) < fs.eval1)
			{
				return fs.eval2;
			}
		}
	}
	return 0;
}

double FightRole::get_tp_skill2(int id)
{
	for (int i = 0; i < tp_skills_map[2].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[2][i];
		if (fs.eval0 == id)
		{
			double per = cur_hp / get_attr(1) * 100;
			if (per < fs.eval1)
			{
				return fs.eval2;
			}
		}
	}
	return 0;
}

void FightRole::get_tp_skill3(std::map<int, double> &attrs)
{
	for (int i = 0; i < tp_skills_map[3].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[3][i];
		attrs[fs.eval0] += fs.eval1;
	}
}

void FightRole::get_tp_skill4(std::map<int, double> &attrs)
{
	for (int i = 0; i < tp_skills_map[4].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[4][i];
		attrs[fs.eval0] += fs.eval1;
	}
}

bool FightRole::get_tp_skill5(double &xx)
{
	xx = 0;
	bool flag = false;
	for (int i = 0; i < tp_skills_map[5].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[5][i];
		xx += fs.eval1 / 100.0f;
		flag = true;
	}
	return flag;
}

bool FightRole::get_tp_skill6(double &fh)
{
	if (fuhuo > 0)
	{
		return false;
	}
	for (int i = 0; i < tp_skills_map[6].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[6][i];
		if (Utils::get_int32(0, 99) < fs.eval1)
		{
			fh = fs.eval2 / 100.0f;
			fuhuo++;
			return true;
		}
	}
	return false;
}

bool FightRole::get_tp_skill7()
{
	for (int i = 0; i < tp_skills_map[7].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[7][i];
		if (Utils::get_int32(0, 99) < fs.eval1)
		{
			return true;
		}
	}
	return false;
}

bool FightRole::get_tp_skill8(double &attack, double &ychu)
{
	double _val = 0;
	bool flag = false;
	for (int i = 0; i < tp_skills_map[8].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[8][i];
		flag = true;
		_val += (fs.eval1) / 100.0f;
	}

	_val = _val * get_attr(1);
	if (flag && attack > _val)
	{
		ychu = attack - _val;
		attack = _val;
		return true;
	}
	return false;
}

bool FightRole::get_tp_skill9()
{
	for (int i = 0; i < tp_skills_map[9].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[9][i];
		if (Utils::get_int32(0, 99) < fs.eval1)
		{
			return true;
		}
	}
	return false;
}

bool FightRole::get_tp_skill10(double &hp)
{
	hp = 0;
	bool flag = false;
	for (int i = 0; i < tp_skills_map[10].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[10][i];
		hp += get_attr(2) * fs.eval1 / 100.0f;
		flag = true;
	}
	return flag;
}

void FightRole::get_tp_skill11(double &attack, int per)
{
	double _val = 0;
	for (int i = 0; i < tp_skills_map[11].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[11][i];
		if (fs.eval0 == per)
		{
			_val += fs.eval1 / 100.0f;
		}
	}

	attack = (1 + _val) * attack;
}

void FightRole::get_tp_skill12(double &attack, int per)
{
	double _val = 0;
	for (int i = 0; i < tp_skills_map[12].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[12][i];
		if (fs.eval0 == per)
		{
			_val += fs.eval1 / 100.0f;
		}
	}

	attack = (1 - _val) * attack;
}

bool FightRole::get_tp_skill14()
{
	bool flag = false;
	for (int i = 0; i < tp_skills_map[14].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[14][i];
		role_attrs[fs.eval0] += fs.eval1;
		flag = true;
	}
	return flag;
}

bool FightRole::get_tp_skill15(double &attack)
{
	bool flag = false;
	double _val = 0;
	for (int i = 0; i < tp_skills_map[15].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[15][i];
		_val += fs.eval1 / 100.0f;
		flag = true;
	}

	attack = (1 - _val) * attack;
	return flag;
}

bool FightRole::get_tp_skill16()
{
	for (int i = 0; i < tp_skills_map[16].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[16][i];
		return true;
	}
	return false;
}

bool FightRole::get_tp_skill17(double & val)
{
	if (baozha > 0)
	{
		return false;
	}
	val = 0;
	for (int i = 0; i < tp_skills_map[17].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[17][i];
		val = fs.eval1 / 100.0f * get_attr(2);
		baozha++;
		return true;
	}

	return false;
}

bool FightRole::get_tp_skill18(double & val)
{
	if (fengxian > 0)
	{
		return false;
	}
	val = 0;
	for (int i = 0; i < tp_skills_map[18].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[18][i];
		val = fs.eval1 / 100.0f * get_attr(2);
		fengxian++;
		return true;
	}

	return false;
}

void FightRole::get_tp_skill19(double &attack, int fc1, int fc2)
{
	double _val = 0;
	for (int i = 0; i < tp_skills_map[19].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[19][i];
		if (fc1 > fc2)
		{
			_val += fs.eval1 / 100.0f;
		}
	}

	attack = (1 + _val) * attack;
}

void FightRole::get_tp_skill20(double &attack, int fc1, int fc2)
{
	double _val = 0;
	for (int i = 0; i < tp_skills_map[20].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[20][i];
		if (fc1 > fc2)
		{
			_val += (fs.eval1) / 100.0f;
		}
	}

	attack = (1 - _val) * attack;
}

bool FightRole::get_tp_skill21()
{
	bool flag = false;
	for (int i = 0; i < tp_skills_map[21].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[21][i];
		if (Utils::get_int32(0, 99) < fs.eval1)
		{
			flag = true;
			break;
		}
	}

	return flag;
}

bool FightRole::get_tp_skill22(double &ft)
{
	bool flag = false;
	for (int i = 0; i < tp_skills_map[22].size(); ++i)
	{
		FightSkill &fs = tp_skills_map[22][i];
		if (Utils::get_int32(0, 99) < fs.eval1)
		{
			ft += (fs.eval2) / 100.0f;
			flag = true;
		}
	}

	return flag;
}

bool FightRole::get_equip_skill1(double &attack)
{
	bool flag = false;
	double _val = 1;
	for (int i = 0; i < equip_skills.size(); ++i)
	{
		const s_t_equip_skill *es = equip_skills[i];
		if (es->type == 2)
		{
			int rate = es->def1;
			if (guanghuan_skills.find(2) != guanghuan_skills.end())
			{
				rate += guanghuan_skills[2];
			}
			if (Utils::get_int32(0, 99) < rate)
			{
				_val = 2;
				flag = true;
			}
			break;
		}
	}

	attack = _val * attack;
	return flag;
}

bool FightRole::get_equip_skill2(double &val)
{
	bool flag = false;
	for (int i = 0; i < equip_skills.size(); ++i)
	{
		const s_t_equip_skill *es = equip_skills[i];
		if (es->type == 3)
		{
			if (Utils::get_int32(0, 99) < es->def1)
			{
				int v = es->def2;
				if (guanghuan_skills.find(3) != guanghuan_skills.end())
				{
					v += guanghuan_skills[3];
				}
				val = v / 100.0f;
				flag = true;
			}
			break;
		}
	}

	return flag;
}

bool FightRole::get_equip_skill3(double &ft)
{
	bool flag = false;
	for (int i = 0; i < equip_skills.size(); ++i)
	{
		const s_t_equip_skill *es = equip_skills[i];
		if (es->type == 4)
		{
			if (Utils::get_int32(0, 99) < es->def1)
			{
				ft = es->def2 / 100.0f;
				flag = true;
			}
			break;
		}
	}

	return flag;
}

bool FightRole::get_equip_skill4(double &ft)
{
	bool flag = false;
	for (int i = 0; i < equip_skills.size(); ++i)
	{
		const s_t_equip_skill *es = equip_skills[i];
		if (es->type == 5)
		{
			if (Utils::get_int32(0, 99) < es->def1)
			{
				ft = es->def2 / 100.0f;
				flag = true;
			}
			break;
		}
	}

	return flag;
}

bool FightRole::get_equip_skill5(double &val)
{
	bool flag = false;
	for (int i = 0; i < equip_skills.size(); ++i)
	{
		const s_t_equip_skill *es = equip_skills[i];
		if (es->type == 6)
		{
			if (Utils::get_int32(0, 99) < es->def1)
			{
				val = es->def2 / 100.0f;
				flag = true;
			}
			break;
		}
	}

	return flag;
}

bool FightRole::get_equip_skill6(double &attack)
{
	bool flag = false;
	double _val = 0;
	for (int i = 0; i < equip_skills.size(); ++i)
	{
		const s_t_equip_skill *es = equip_skills[i];
		if (es->type == 7)
		{
			if (Utils::get_int32(0, 99) < es->def1)
			{
				_val = es->def2;
				flag = true;
			}
			break;
		}
	}

	attack = (1 - _val / 100.0f) * attack;
	return flag;
}

void FightRole::get_zs_skill201(double &attack, int type, int is_stun, int mp_ch)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 201)
	{
		if (type == 1 && is_stun && mp_ch == 2)
		{
			attack *= 1 + zs_skill.eval1 / 100.0f;
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
		}
	}
}

void FightRole::get_zs_skill202(double &attack, int type, int num)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 202)
	{
		if (type == 1 && num == 1)
		{
			attack *= 1 + zs_skill.eval1 / 100.0f;
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
		}
	}
}

bool FightRole::get_zs_skill203(double &xx)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 203)
	{
		xx = get_attr(1) * zs_skill.eval1 / 100.0f;
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
		return true;
	}
	return false;
}

bool FightRole::get_zs_skill204()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 204)
	{
		if (Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

bool FightRole::get_zs_skill205(double &ft, double sh)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 205)
	{
		ft = sh * zs_skill.eval1 / 100.0f;
		if (ft > get_attr(2) * zs_skill.eval2 / 100.0f)
		{
			ft = get_attr(2) * zs_skill.eval2 / 100.0f;
		}
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
		return true;
	}
	return false;
}

bool FightRole::get_zs_skill206(double wf1, double wf2)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 206)
	{
		if (wf1 > wf2 && Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

void FightRole::get_zs_skill207(double &attack, int type, int is_stun)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 207)
	{
		if (type == 1 && !is_stun)
		{
			attack *= 1 + zs_skill.eval1 / 100.0f;
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
		}
	}
}

int FightRole::get_zs_skill208(double &gj)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 208)
	{
		gj = zs_skill.eval1;
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
		return zs_skill.t_skill->id;
	}
	return 0;
}

void FightRole::get_zs_skill209()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 209)
	{
		add_nl(zs_skill.eval1);
		role_attrs[7] += zs_skill.eval2;
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
	}
}

void FightRole::get_zs_skill211(double &attack, int type, double sx)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 211)
	{
		if (type == 1)
		{
			attack += sx * zs_skill.eval1 / 100.0f;
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
		}
	}
}

void FightRole::get_zs_skill212(int type)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 212)
	{
		if (type == 1 && Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			mianyi = 1;
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			MissionFight::mission_make_tick(22);
			g_tick->add_values(site);
			g_tick->add_values(1);
		}
	}
}

bool FightRole::get_zs_skill213(int type)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 213)
	{
		if (type == 1 && Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

void FightRole::get_zs_skill214(double &attack, double hp, int is_bj)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 214)
	{
		if (is_bj)
		{
			double fj = hp * zs_skill.eval1 / 100.0f;
			if (fj > get_attr(2) * zs_skill.eval2 / 100.0f)
			{
				fj = get_attr(2) * zs_skill.eval2 / 100.0f;
			}
			attack += fj;
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
		}
	}
}

void FightRole::get_zs_skill215_1()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 215)
	{
		role_attrs[17] += zs_skill.eval1;
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
	}
}

bool FightRole::get_zs_skill215_2(double &hp)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 215)
	{
		hp = get_attr(2) * zs_skill.eval2 / 100.0f;
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
		return true;
	}
	return false;
}

void FightRole::get_zs_skill401(double sh)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 401)
	{
		role_attrs[2] += sh * zs_skill.eval1 / 100.0f;
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
	}
}

bool FightRole::get_zs_skill402()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 402)
	{
		if (Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

void FightRole::get_zs_skill403(double attack, std::vector<FightRole *> &range_frs)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 403)
	{
		FightRole *fr1 = 0;
		double bl = 1000;
		for (int i = 0; i < range_frs.size(); ++i)
		{
			FightRole *fr = range_frs[i];
			if (fr->cp == cp)
			{
				double tbl = fr->cur_hp / fr->get_attr(1);
				if (tbl < bl)
				{
					bl = tbl;
					fr1 = fr;
				}
			}
		}
		if (!fr1)
		{
			return;
		}
		double hf = attack * zs_skill.eval1 / 100.0f;
		MissionFight::mission_make_tick(12);
		g_tick->add_values(site);
		fr1->set_cur_hp(fr1->cur_hp + hf);
		MissionFight::mission_make_tick(13);
		g_tick->add_values(fr1->site);
		g_tick->add_dvalues(hf);
		g_tick->add_dvalues(fr1->cur_hp);

		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
	}
}

bool FightRole::get_zs_skill404()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 404)
	{
		bool flag = false;
		s_t_skill *t_skill = get_skill(1, pinzhi_skill);
		if (!t_skill)
		{
			return false;
		}
		for (std::map<int, FightBuff>::iterator it = buffs.begin(); it != buffs.end(); ++it)
		{
			int id = (*it).second.id;
			if (id == t_skill->id)
			{
				flag = true;
				break;
			}
		}
		if (flag && Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

void FightRole::get_zs_skill405()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 405)
	{
		role_attrs[20] += zs_skill.eval1;
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
	}
}

void FightRole::get_zs_skill406(double &attack, int is_hf)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 406)
	{
		if (is_hf)
		{
			attack *= 1 + zs_skill.eval1 / 100.0f;
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
		}
	}
}

void FightRole::get_zs_skill400(double &attack)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 400)
	{
		attack += get_attr(3) * zs_skill.eval1 / 100.0f;
		MissionFight::mission_make_tick(21);
		g_tick->add_values(site);
		g_tick->add_values(zs_skill.t_skill->id);
	}
}

bool FightRole::get_zs_skill500()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 500)
	{
		if (Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

bool FightRole::get_zs_skill501()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 501)
	{
		if (fuhuo == 0 && cur_hp <= 0 && Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			set_cur_hp(1);
			fuhuo = 1;
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

void FightRole::get_zs_skill502()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 502)
	{
		if (Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			add_nl(1);
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
		}
	}
}

bool FightRole::get_zs_skill503(int num1, int num2)
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 503)
	{
		if (num1 > num2 && Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

bool FightRole::get_zs_skill504()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 504)
	{
		if (Utils::get_int32(0, 99) < zs_skill.eval1)
		{
			MissionFight::mission_make_tick(21);
			g_tick->add_values(site);
			g_tick->add_values(zs_skill.t_skill->id);
			return true;
		}
	}
	return false;
}


void FightRole::get_zs_skill505()
{
	if (zs_skill.t_skill && zs_skill.t_skill->ex_type == 505)
	{
		role_attrs[7] += zs_skill.eval2;
		mianyi = 1;
	}
}

bool FightRole::get_kz_skill_fw(double &attack, int is_gd, int job)
{
	if (per == 1 && kz_skill.t_skill && is_gd && job == 2)
	{
		attack *= (1.0 - kz_skill.eval0 / 100.0);
		MissionFight::mission_make_tick(25);
		g_tick->add_values(site);
		g_tick->add_values(kz_skill.t_skill->id);
		return true;
	}
	return false;
}

bool FightRole::get_kz_skill_wl(double attack, int job)
{
	if (per == 2 && kz_skill.t_skill && job == 3 && cur_hp > 0.1)
	{
		if (attack >= kz_skill.eval0 / 100.0 * get_attr(1))
		{
			MissionFight::mission_make_tick(25);
			g_tick->add_values(site);
			g_tick->add_values(kz_skill.t_skill->id);
			return true;
		}
	}
	return false;
}

int FightRole::get_kz_skill_mf(int job)
{
	if (per == 3 && kz_skill.t_skill && job == 1)
	{
		if (Utils::get_int32(0, 99) < kz_skill.eval0)
		{
			MissionFight::mission_make_tick(25);
			g_tick->add_values(site);
			g_tick->add_values(kz_skill.t_skill->id);
			return Utils::get_int32(1, kz_skill.eval1);
		}
	}
	return 0;
}

void FightRole::add_helo()
{
	std::map<int, double> attrs1;
	get_tp_skill3(attrs1);
	std::map<int, double> attrs2;
	get_tp_skill4(attrs2);
	if (cp == 0)
	{
		for (std::map<int, double>::iterator it = attrs1.begin(); it != attrs1.end(); ++it)
		{
			cp_attrs0[(*it).first] += (*it).second;
		}
		for (std::map<int, double>::iterator it = attrs2.begin(); it != attrs2.end(); ++it)
		{
			cp_attrs1[(*it).first] += (*it).second;
		}
	}
	else
	{
		for (std::map<int, double>::iterator it = attrs1.begin(); it != attrs1.end(); ++it)
		{
			cp_attrs1[(*it).first] += (*it).second;
		}
		for (std::map<int, double>::iterator it = attrs2.begin(); it != attrs2.end(); ++it)
		{
			cp_attrs0[(*it).first] += (*it).second;
		}
	}
}

void FightRole::del_helo()
{
	std::map<int, double> attrs1;
	get_tp_skill3(attrs1);
	std::map<int, double> attrs2;
	get_tp_skill4(attrs2);
	
	if (cp == 0)
	{
		for (std::map<int, double>::iterator it = attrs1.begin(); it != attrs1.end(); ++it)
		{
			cp_attrs0[(*it).first] -= (*it).second;
		}
		for (std::map<int, double>::iterator it = attrs2.begin(); it != attrs2.end(); ++it)
		{
			cp_attrs1[(*it).first] -= (*it).second;
		}
	}
	else
	{
		for (std::map<int, double>::iterator it = attrs1.begin(); it != attrs1.end(); ++it)
		{
			cp_attrs1[(*it).first] -= (*it).second;
		}
		for (std::map<int, double>::iterator it = attrs2.begin(); it != attrs2.end(); ++it)
		{
			cp_attrs0[(*it).first] -= (*it).second;
		}
	}
}

void FightRole::do_buff(int type)
{
	for (std::map<int, FightBuff>::iterator it = buffs.begin(); it != buffs.end();)
	{
		FightBuff &buff = (*it).second;
		s_t_skill *t_skill = sRoleConfig->get_skill(buff.id);
		if (!t_skill || t_skill->buffer_types[buff.col] != type)
		{
			it++;
			continue;
		}
		if (t_skill->buffer_types[buff.col] == 1)
		{
			double sh = buff.attack * t_skill->buffer_attack_pes[buff.col] + t_skill->buffer_attack_pe_adds[buff.col] * (buff.level - 1);
			if (t_skill->buffer_attack_types[buff.col] == 3)
			{
				set_cur_hp(cur_hp + sh);
				MissionFight::mission_make_tick(1);
				g_tick->add_values(site);
				g_tick->add_dvalues(sh);
				g_tick->add_dvalues(cur_hp);
			}
			else
			{
				set_cur_hp(cur_hp - sh);
				MissionFight::mission_make_tick(2);
				g_tick->add_values(site);
				g_tick->add_values(t_skill->buffer_attack_types[buff.col]);
				g_tick->add_dvalues(sh);
				g_tick->add_dvalues(cur_hp);
			}
		}
		buff.round--;
		if (buff.round <= 0)
		{
			if (t_skill->buffer_types[buff.col] == 1)
			{
				MissionFight::mission_make_tick(3);
			}
			else
			{
				MissionFight::mission_make_tick(18);
			}
			g_tick->add_values(site);
			g_tick->add_values(buff.id);
			g_tick->add_values(buff.col);
			buffs.erase(it++);
		}
		else
		{
			it++;
		}
	}
}

//////////////////////////////////////////////////////////////////////////

void MissionFight::mission_clear(int fight_type)
{
	g_fp1.clear();
	g_fp2.clear();
	for (std::map<int, FightRole *>::iterator it = g_yj.begin(); it != g_yj.end(); ++it)
	{
		delete (*it).second;
	}
	g_yj.clear();
	g_pich1 = 0;
	g_pich2 = 0;
	g_round = 0;
	g_die_role = 0;
	g_total_hp = 0;
	cp_attrs0.clear();
	cp_attrs1.clear();
	g_gj1 = 1.0f;
	g_sm1 = 1.0f;
	g_gj2 = 1.0f;
	g_sm2 = 1.0f;
	ttt_attrs.clear();
	g_text.Clear();
	g_max_round = 20;
	g_min_hp = -1;
	g_max_die_role = 99999;
	g_fight_type = fight_type;
	g_bingyuan_vec.clear();
	g_ewxd.clear();
	g_text.set_fight_type(fight_type);
}

FightRole * MissionFight::get_fight_role(dhc::player_t *player, dhc::role_t *role, int cp, int site, int duixing_id)
{
	FightRole *fr = new FightRole();
	RoleOperation::get_role_attr(player, role, fr->role_attrs);
	fr->id = role->template_id();
	fr->site = site;
	s_t_role *t_role = sRoleConfig->get_role(fr->id);
	if (!t_role)
	{
		return 0;
	}
	s_t_duixing *t_duixing = sRoleConfig->get_duixing(duixing_id);
	if (!t_duixing)
	{
		return 0;
	}
	fr->duiwei = t_duixing->duiweis[site % 6];
	fr->cp = cp;
	fr->per = t_role->job;
	fr->font_color = t_role->font_color;
	fr->pinzhi = role->pinzhi();
	s_t_role_shengpin *t_role_shengpin = sRoleConfig->get_role_shengpin(fr->pinzhi);
	if (!t_role_shengpin)
	{
		return 0;
	}
	fr->pinzhi_skill = t_role_shengpin->zdjnjc;
	fr->level = role->level();
	fr->glevel = role->glevel();
	fr->jlevel = role->jlevel();
	fr->dress_id = role->dress_on_id();
	fr->guanghuan_id = player->guanghuan_id();
	fr->skill_level = role->jskill_level(0);
	for (int i = 1; i < role->jskill_level_size(); ++i)
	{
		fr->jskill_levels.push_back(role->jskill_level(i));
	}
	fr->cur_hp = fr->get_attr(1);
	fr->nengliang = 1 + fr->get_attr(22);
	std::map<int, int> ids;
	sRoleConfig->get_guanghuan_skill_id(player, ids);
	fr->guanghuan_skills = ids;
	fr->init(role);
	return fr;
}

FightRole * MissionFight::get_fight_role_monster(int monster_id, int site, int duixing_id)
{
	s_t_monster *t_monster = sMissionConfig->get_monster(monster_id);
	if (!t_monster)
	{
		return 0;
	}
	FightRole *fr = new FightRole();
	fr->id = t_monster->class_id;
	fr->site = site;
	s_t_role *t_role = sRoleConfig->get_role(fr->id);
	if (!t_role)
	{
		return 0;
	}
	s_t_duixing *t_duixing = sRoleConfig->get_duixing(duixing_id);
	if (!t_duixing)
	{
		return 0;
	}
	fr->duiwei = t_duixing->duiweis[site % 6];
	fr->cp = 1;
	fr->per = t_role->job;
	fr->font_color = t_role->font_color;
	fr->pinzhi = 400;
	if (t_monster->jy_level == 1)
	{
		fr->pinzhi = 700;
	}
	else if (t_monster->jy_level == 2)
	{
		fr->pinzhi = 1000;
	}
	fr->pinzhi_skill = t_monster->pinzhi_skill;
	if (fr->pinzhi_skill == 1)
	{
		fr->pinzhi = 1300;
	}
	else if (fr->pinzhi_skill == 2)
	{
		fr->pinzhi = 1700;
	}
	fr->role_attrs[1] += t_monster->hp;
	fr->role_attrs[2] += t_monster->gj;
	fr->role_attrs[3] += t_monster->wf;
	fr->role_attrs[4] += t_monster->mf;
	fr->role_attrs[5] += t_monster->sd;;
	fr->role_attrs[11] += t_monster->bj;
	fr->role_attrs[12] += t_monster->gd;
	fr->role_attrs[14] += t_monster->wm;
	fr->role_attrs[15] += t_monster->mm;
	fr->role_attrs[16] += t_monster->mz;
	fr->role_attrs[17] += t_monster->sb;
	fr->role_attrs[18] += t_monster->kb;
	fr->role_attrs[19] += t_monster->ct;
	fr->role_attrs[20] += t_monster->zs;
	fr->role_attrs[21] += t_monster->js;
	fr->role_attrs[22] += t_monster->snl;
	fr->level = t_monster->level;
	fr->glevel = t_monster->glevel;
	fr->jlevel = t_monster->jlevel;
	fr->skill_level = t_monster->skill_level;
	for (int i = 0; i < TP_SKILL_NUM; ++i)
	{
		fr->jskill_levels.push_back(t_monster->skill_level);
	}
	fr->cur_hp = fr->get_attr(1);
	fr->nengliang = fr->get_attr(22);
	fr->init(0);
	return fr;
}

FightRole* MissionFight::get_fight_pet(dhc::player_t *player, dhc::pet_t *pet, int cp)
{
	const s_t_pet *t_pet = sRoleConfig->get_pet(pet->template_id());
	if (!t_pet)
	{
		return 0;
	}
	const s_t_role *t_role = sRoleConfig->get_role(t_pet->class_id);
	if (!t_role)
	{
		return 0;
	}
	FightRole *fr = new FightRole();
	RoleOperation::get_pet_attr(player, pet, fr->role_attrs, true, true, true);
	fr->is_pet = true;
	fr->id = t_role->id;
	fr->site = cp + 100;
	fr->duiwei = cp + 100;
	fr->cp = cp;
	fr->per = t_role->job;
	fr->font_color = t_pet->color;
	fr->pinzhi = 400;
	fr->pinzhi_skill = pet->star();
	fr->level = pet->level();
	fr->glevel = 0;
	fr->jlevel = 0;
	fr->dress_id = 0;
	fr->guanghuan_id = 0;
	fr->skill_level = 1;
	for (int i = 0; i < TP_SKILL_NUM; ++i)
	{
		fr->jskill_levels.push_back(1);
	}
	fr->cur_hp = fr->get_attr(1);
	fr->nengliang = 0;
	fr->init(0);
	return fr;
}

int MissionFight::make_fz(dhc::player_t *player, int cp, FightZhen &fz)
{
	int num = 0;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		int site = player->duixing(i);
		uint64_t guid = player->zhenxing(i);
		dhc::role_t *role = POOL_GET_ROLE(guid);
		if (role)
		{
			s_t_role *t_role = sRoleConfig->get_role(role->template_id());
			if (t_role)
			{
				FightRole * fr = get_fight_role(player, role, cp, site + cp * 6, player->duixing_id());
				fz.frs[site] = fr;
				g_total_hp += fr->cur_hp;
				num++;
			}
		}
	}
	if (player->pet_on() != 0)
	{
		dhc::pet_t *pet = POOL_GET_PET(player->pet_on());
		if (pet)
		{
			FightRole *fr = get_fight_pet(player, pet, cp);
			if (fr)
			{
				fr->bf = player->bf();
				fz.pet = fr;
			}
		}
	}
	return num;
}

int MissionFight::make_fz(dhc::player_t *player, int cp, FightZhen &fz, const std::vector<std::pair<bool, int> >& cur_hps)
{
	int num = 0;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		int site = player->duixing(i);
		uint64_t guid = player->zhenxing(i);
		dhc::role_t *role = POOL_GET_ROLE(guid);
		if (role)
		{
			s_t_role *t_role = sRoleConfig->get_role(role->template_id());
			if (t_role)
			{
				if (!cur_hps[site].first)
				{
					FightRole * fr = get_fight_role(player, role, cp, site + cp * 6, player->duixing_id());
					fz.frs[site] = fr;
					if (cur_hps[site].second > 0)
					{
						fr->set_cur_hp(cur_hps[site].second);
					}
					g_total_hp += fr->cur_hp;
					num++;
				}
			}
		}
	}
	return num;
}

int MissionFight::mission_gq(dhc::player_t *player, int mission_id, int &star, bool can_jump, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(1);
	int num = 0;
	{
		FightZhen fz;
		num = make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}
	{
		s_t_mission *t_mission = sMissionConfig->get_mission(mission_id);
		if (!t_mission)
		{
			return false;
		}
		for (int i = 0; i < 2; ++i)
		{
			FightZhen fz;
			bool flag = false;
			int dx = t_mission->dxs[i];
			int num = 0;
			for (int j = 0; j < 5; ++j)
			{
				int id = t_mission->monsters[i * 5 + j];
				if (id != 0)
				{
					FightRole * fr = get_fight_role_monster(id, j + 6, dx);
					if (!t_mission->tkezhi)
						fr->kz_skill.t_skill = 0;
					fz.frs[j] = fr;
					flag = true;
				}
			}
			if (flag)
			{
				g_fp2.fzs.push_back(fz);
			}
		}
		if (t_mission->yjguan)
		{
			int index = -1;
			for (int i = 0; i < player->zhenxing_size(); ++i)
			{
				if (player->zhenxing(i) == 0)
				{
					index = i;
					break;
				}
			}
			if (index != -1)
			{
				FightRole * fr = get_fight_role_monster(t_mission->yj, index, player->duixing_id());
				fr->cp = 0;
				fr->is_yj = true;
				g_yj[t_mission->yjguan] = fr;
			}
		}
	}
	int res = mission_fight();
	if (res == 1)
	{
		int num1 = 0;
		for (int i = 0; i < g_fp1.fzs[0].frs.size(); ++i)
		{
			if (g_fp1.fzs[0].frs[i] != 0 && !g_fp1.fzs[0].frs[i]->is_yj)
			{
				num1++;
			}
		}
		if (num == num1)
		{
			star = 3;
		}
		else if (num - 1 == num1)
		{
			star = 2;
		}
		else
		{
			star = 1;
		}
	}
	else
	{
		star = 0;
		mission_make_fail();
	}
	g_text.set_can_jump(can_jump);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_sport(dhc::player_t *player, dhc::player_t *target, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(0);
	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}
	{
		FightZhen fz;
		make_fz(target, 1, fz);
		g_fp2.fzs.push_back(fz);
	}
	int res = mission_fight();
	if (res != 1)
	{
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_rob(dhc::player_t *player, dhc::player_t *target, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(0);
	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}
	{
		FightZhen fz;
		make_fz(target, 1, fz);
		g_fp2.fzs.push_back(fz);
	}
	int res = mission_fight();
	if (res != 1)
	{
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_ttt(dhc::player_t *player, int d_type, int d_param, const std::vector<int> &mids, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(1);
	for (int i = 0; i < player->ttt_reward_ids_size(); ++i)
	{
		int id = player->ttt_reward_ids(i);
		s_t_ttt_value *t_ttt_value = sMissionConfig->get_ttt_value(id);
		if (t_ttt_value)
		{
			ttt_attrs[t_ttt_value->sxtype] += t_ttt_value->sxvalue;
		}
	}

	if (d_type == 2)
	{
		g_max_round = d_param;
	}
	else if (d_type == 3)
	{
		g_max_die_role = d_param;
	}
	else if (d_type == 4)
	{
		g_min_hp = d_param;
	}

	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}
	{
		FightZhen fz;
		int dx = 101;
		bool flag = false;
		for (int j = 0; j < 5; ++j)
		{
			int id = mids[j];
			if (id != 0)
			{
				FightRole * fr = get_fight_role_monster(id, j + 6, dx);
				fz.frs[j] = fr;
				flag = true;
			}
		}
		if (flag)
		{
			g_fp2.fzs.push_back(fz);
		}
	}
	int res = mission_fight();
	if (res != 1)
	{
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_boss(dhc::player_t *player, int boss_id, double boss_hp, double boss_max_hp, double gj, double &hhp, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(1);
	g_gj1 = gj;

	if (boss_id == 0)
	{
		return true;
	}
	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}
	{
		FightZhen fz;
		int dx = 101;
		FightRole * fr = get_fight_role_monster(boss_id, 6, dx);
		fr->pinzhi = 2000;
		fr->role_attrs[1] = boss_max_hp;
		fr->set_cur_hp(boss_hp);
		fr->is_boss = true;
		fz.frs[0] = fr;
		g_fp2.fzs.push_back(fz);
	}
	int res = mission_fight();
	if (res == 1)
	{
		hhp = boss_hp;
	}
	else
	{
		if (g_fp2.fzs[0].frs.find(0) != g_fp2.fzs[0].frs.end())
		{
			hhp = boss_hp - g_fp2.fzs[0].frs[0]->cur_hp;
		}
		else
		{
			hhp = boss_hp;
		}
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_yb(dhc::player_t *player, dhc::player_t *target, double gw, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(0);
	g_gj2 = gw + 1.0f;
	g_sm2 = gw + 1.0f;

	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}
	{
		FightZhen fz;
		make_fz(target, 1, fz);
		g_fp2.fzs.push_back(fz);
	}
	int res = mission_fight();
	if (res != 1)
	{
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_guild(dhc::player_t *player, dhc::guild_mission_t* guild_mission, int index, double& hp, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(1);
	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}

	std::vector<std::pair<int, int> > monster_indexs;
	{
		FightZhen fz;
		int dx = 106;
		int pos_index = 0;
		for (int i = 0; i < 5; ++i)
		{
			int monster_index = index * 5 + i;
			if (guild_mission->guild_cur_hps(monster_index) > 0)
			{
				FightRole * fr = get_fight_role_monster(guild_mission->guild_monsters(monster_index), pos_index + 6, dx);
				fr->set_cur_hp(guild_mission->guild_cur_hps(monster_index));
				fr->fuhuo = 1;
				fz.frs[pos_index] = fr;
				monster_indexs.push_back(std::make_pair(pos_index, monster_index));
				++pos_index;
			}
			
		}
		g_fp2.fzs.push_back(fz);
	}

	int res = mission_fight();
	if (res == 1)
	{
		for (std::vector<std::pair<int, int> >::size_type i = 0;
			i < monster_indexs.size();
			++i)
		{
			const std::pair<int, int>& mindexs = monster_indexs[i];
			hp += guild_mission->guild_cur_hps(mindexs.second);
			guild_mission->set_guild_cur_hps(mindexs.second, 0);
		}
	}
	else
	{
		for (std::vector<std::pair<int, int> >::size_type i = 0;
			i < monster_indexs.size();
			++i)
		{
			const std::pair<int, int>& mindexs = monster_indexs[i];
			if (g_fp2.fzs[0].frs.find(mindexs.first) != g_fp2.fzs[0].frs.end())
			{
				FightRole * fr = g_fp2.fzs[0].frs[mindexs.first];
				hp += guild_mission->guild_cur_hps(mindexs.second) - fr->cur_hp;
				guild_mission->set_guild_cur_hps(mindexs.second, fr->cur_hp);
			}
			else
			{
				hp += guild_mission->guild_cur_hps(mindexs.second);
				guild_mission->set_guild_cur_hps(mindexs.second, 0);
			}
		}
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_ore(dhc::player_t *player, int monster_id, int& hp, double& hper, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(1);
	g_max_round = 5;

	int shp;
	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}
	{
		FightZhen fz;
		int dx = 101;
		FightRole * fr = get_fight_role_monster(monster_id, 6, dx);
		fr->pinzhi = 2000;
		fz.frs[0] = fr;
		shp = fr->get_attr(1);
		g_fp2.fzs.push_back(fz);
	}
	int res = mission_fight();
	if (res == 1)
	{
		hper = 1;
		hp = shp;
	}
	else
	{
		hper = 1 - g_fp2.fzs[0].frs[0]->cur_hp / shp;
		hp = shp - g_fp2.fzs[0].frs[0]->cur_hp;
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_hbb(dhc::player_t *player, const std::vector<int> ids, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(1);

	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}
	{
		FightZhen fz;
		int dx = 107;
		for (int i = 0; i < ids.size(); ++i)
		{
			s_t_role *t_role = sRoleConfig->get_role(ids[i]);
			if (!t_role)
			{
				continue;
			}
			FightRole * fr = get_fight_role_monster(70000001, 6 + i, dx);
			fr->id = ids[i];
			fr->pinzhi = t_role->pinzhi * 100;
			fr->init(0);
			fz.frs[i] = fr;
		}
		g_fp2.fzs.push_back(fz);
	}
	int res = mission_fight();
	if (res != 1)
	{
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

int MissionFight::mission_bingyuan(protocol::team::team *lteam, protocol::team::team *rteam, protocol::team::bingyuan_fight_text& fight_text)
{
	int index1 = 0;
	int index2 = 0;
	dhc::player_t *player1 = 0;
	dhc::player_t *player2 = 0;
	int res = -1;
	std::vector<std::pair<bool, int> > role_hp1s;
	int role_max_hp1 = -1;
	int role_max_hp2 = -1;
	std::vector<std::pair<bool, int> > role_hp2s;
	int houshou = 1;
	do
	{
		mission_clear(2);

		if (player1 == 0)
		{
			player1 = POOL_GET_PLAYER(lteam->players(index1).guid());
			role_hp1s.clear();
			role_max_hp1 = -1;
			for (int i = 0; i < 6; ++i)
			{
				role_hp1s.push_back(std::make_pair(false, 0));
			}
		}
		if (player2 == 0)
		{
			player2 = POOL_GET_PLAYER(rteam->players(index2).guid());
			role_hp2s.clear();
			role_max_hp2 = -1;
			for (int i = 0; i < 6; ++i)
			{
				role_hp2s.push_back(std::make_pair(false, 0));
			}
		}

		if (player1 && player2)
		{
			{
				FightZhen fz;
				make_fz(player1, 0, fz, role_hp1s);
				g_fp1.fzs.push_back(fz);
			}

			{
				FightZhen fz;
				make_fz(player2, 1, fz, role_hp2s);
				g_fp2.fzs.push_back(fz);
			}

			protocol::team::bingyuan_huihe *huihe = fight_text.add_huihes();

			res = MissionFight::mission_fight();
			huihe->add_team_ids(lteam->team_id());
			huihe->add_team_ids(rteam->team_id());
			huihe->add_team_guids(player1->guid());
			huihe->add_team_guids(player2->guid());
			for (int i = 0; i < g_bingyuan_vec.size(); ++i)
			{
				huihe->add_hps(g_bingyuan_vec[i]);
			}
			if (role_max_hp1 == -1)
			{
				role_max_hp1 = g_bingyuan_vec[0];
			}
			if (role_max_hp2 == -1)
			{
				role_max_hp2 = g_bingyuan_vec[1];
			}
			huihe->add_max_hps(role_max_hp1);
			huihe->add_max_hps(role_max_hp2);

			if (res == 1)
			{
				huihe->set_win(player1->guid());
				for (int i = 0; i < role_hp1s.size(); ++i)
				{
					std::map<int, FightRole*>::iterator jt = g_fp1.fzs[0].frs.find(i);
					if (jt == g_fp1.fzs[0].frs.end())
					{
						role_hp1s[i].first = true;
					}
					else
					{
						if (jt->second->cur_hp <= 0)
						{
							role_hp1s[i].first = true;
						}
						else
						{
							role_hp1s[i].second = jt->second->cur_hp;
						}
					}
				}

				lteam->mutable_players(index1)->set_kill(lteam->players(index1).kill() + 1);
				if (lteam->players(index1).kill() >= 3)
				{
					++index1;
					player1 = 0;
				}
				++index2;
				player2 = 0;
				houshou = 1;
			}
			else if (res == -1)
			{
				huihe->set_win(player2->guid());
				for (int i = 0; i < role_hp2s.size(); ++i)
				{
					std::map<int, FightRole*>::iterator jt = g_fp2.fzs[0].frs.find(i);
					if (jt == g_fp2.fzs[0].frs.end())
					{
						role_hp2s[i].first = true;
					}
					else
					{
						if (jt->second->cur_hp <= 0)
						{
							role_hp2s[i].first = true;
						}
						else
						{
							role_hp2s[i].second = jt->second->cur_hp;
						}
					}
				}

				rteam->mutable_players(index2)->set_kill(rteam->players(index2).kill() + 1);
				if (rteam->players(index2).kill() >= 3)
				{
					++index2;
					player2 = 0;
				}
				++index1;
				player1 = 0;
				houshou = 0;
			}
			else if (res == 0)
			{
				if (houshou == 0)
				{
					huihe->set_win(player1->guid());
					for (int i = 0; i < role_hp1s.size(); ++i)
					{
						std::map<int, FightRole*>::iterator jt = g_fp1.fzs[0].frs.find(i);
						if (jt == g_fp1.fzs[0].frs.end())
						{
							role_hp1s[i].first = true;
						}
						else
						{
							if (jt->second->cur_hp <= 0)
							{
								role_hp1s[i].first = true;
							}
							else
							{
								role_hp1s[i].second = jt->second->cur_hp;
							}
						}
					}

					lteam->mutable_players(index1)->set_kill(lteam->players(index1).kill() + 1);
					if (lteam->players(index1).kill() >= 3)
					{
						++index1;
						player1 = 0;
					}
					++index2;
					player2 = 0;
					houshou = 1;
				}
				else
				{
					huihe->set_win(player2->guid());
					for (int i = 0; i < role_hp2s.size(); ++i)
					{
						std::map<int, FightRole*>::iterator jt = g_fp2.fzs[0].frs.find(i);
						if (jt == g_fp2.fzs[0].frs.end())
						{
							role_hp2s[i].first = true;
						}
						else
						{
							if (jt->second->cur_hp <= 0)
							{
								role_hp2s[i].first = true;
							}
							else
							{
								role_hp2s[i].second = jt->second->cur_hp;
							}
						}
					}

					rteam->mutable_players(index2)->set_kill(rteam->players(index2).kill() + 1);
					if (rteam->players(index2).kill() >= 3)
					{
						++index2;
						player2 = 0;
					}
					++index1;
					player1 = 0;
					houshou = 0;
				}
			}
			else if (res == -2)
			{
				huihe->set_win(0);
				++index1;
				++index2;
				player1 = 0;
				player2 = 0;
				houshou = 1;
			}
		}
		else if (player1 && (!player2))
		{
			++index2;
			houshou = 1;
		}
		else if ((!player1) && player2)
		{
			++index1;
			houshou = 0;
		}
		else
		{
			++index1;
			++index2;
			houshou = 1;
		}

	} while (index1 < 5 && index2 < 5);

	/// 最后满三场
	if (index1 >= 5 &&
		index2 >= 5 &&
		res != -2)
	{
		if (lteam->players(4).kill() >= 3)
		{
			return 1;
		}
		else if (rteam->players(4).kill() >= 3)
		{
			return -1;
		}
	}


	if (index2 >= 5)
	{
		return 1;
	}
	else
	{
		return -1;
	}
}

int MissionFight::mission_ts(dhc::player_t *player, dhc::player_t *target, int index, dhc::huodong_player_t *huodong_player, std::string &text)
{
	player->set_last_fight_time(game::timer()->now());
	mission_clear(0);

	{
		FightZhen fz;
		make_fz(player, 0, fz);
		g_fp1.fzs.push_back(fz);
	}

	std::vector<std::pair<bool, int> > role_hps;
	{
		for (int i = 0; i < 6; ++i)
		{
			role_hps.push_back(std::make_pair(true, 0));
		}

#define TS_GET_ROLE_HP(i) if (huodong_player->extra_data##i(index) > 0) {role_hps[i - 1].first = false;role_hps[i - 1].second = huodong_player->extra_data##i(index);}
		TS_GET_ROLE_HP(1);
		TS_GET_ROLE_HP(2);
		TS_GET_ROLE_HP(3);
		TS_GET_ROLE_HP(4);
		TS_GET_ROLE_HP(5);
		TS_GET_ROLE_HP(6);

		FightZhen fz;
		make_fz(target, 1, fz, role_hps);
		g_fp2.fzs.push_back(fz);

	}
	int res = mission_fight();
	if (res == 1)
	{

	}
	else
	{
#define TS_SET_ROLE_HP(i) huodong_player->set_extra_data##i(index, role_hps[i - 1].second)
		for (int i = 0; i < role_hps.size(); ++i)
		{
			std::map<int, FightRole*>::iterator jt = g_fp2.fzs[0].frs.find(i);
			if (jt == g_fp2.fzs[0].frs.end())
			{
				role_hps[i].second = 0;
			}
			else
			{
				if (jt->second->cur_hp <= 0)
				{
					role_hps[i].second = 0;
				}
				else
				{
					role_hps[i].second = jt->second->cur_hp;
				}
			}
		}
		TS_SET_ROLE_HP(1);
		TS_SET_ROLE_HP(2);
		TS_SET_ROLE_HP(3);
		TS_SET_ROLE_HP(4);
		TS_SET_ROLE_HP(5);
		TS_SET_ROLE_HP(6);
		mission_make_fail();
	}
	g_text.set_can_jump(true);
	g_text.SerializeToString(&text);
	return res;
}

//////////////////////////////////////////////////////////////////////////

int MissionFight::mission_fight()
{
	g_round = 0;
	protocol::game::msg_fight_bo *bo = g_text.add_bos();
	protocol::game::msg_fight_state *state = bo->mutable_bo_state();
	missoin_make_state(state);
	mission_bingyuan_vec(false);
	/// 初始化光环
	if (g_fp1.fzs.size() > g_pich1)
	{
		for (std::map<int, FightRole *>::iterator it = g_fp1.fzs[g_pich1].frs.begin(); it != g_fp1.fzs[g_pich1].frs.end(); ++it)
		{
			(*it).second->add_helo();
		}
	}
	if (g_fp2.fzs.size() > g_pich2)
	{
		for (std::map<int, FightRole *>::iterator it = g_fp2.fzs[g_pich2].frs.begin(); it != g_fp2.fzs[g_pich2].frs.end(); ++it)
		{
			(*it).second->add_helo();
		}
	}
	/// 战斗
	while (g_pich1 < g_fp1.fzs.size() && g_pich2 < g_fp2.fzs.size())
	{
		g_round++;
		if (g_round > g_max_round)
		{
			break;
		}
		if (g_die_role > g_max_die_role)
		{
			break;
		}
		MissionFight::mission_make_tick(4);
		std::set<int> has_site;
		while (1)
		{
			/// 创建回合位置
			std::vector<FightRole *> range_frs;
			if (g_fp1.fzs[g_pich1].frs.size() == 0)
			{
				break;
			}
			for (std::map<int, FightRole *>::iterator it = g_fp1.fzs[g_pich1].frs.begin(); it != g_fp1.fzs[g_pich1].frs.end(); ++it)
			{
				FightRole *fr = (*it).second;
				range_frs.push_back(fr);
			}
			if (g_fp2.fzs[g_pich2].frs.size() == 0)
			{
				break;
			}
			for (std::map<int, FightRole *>::iterator it = g_fp2.fzs[g_pich2].frs.begin(); it != g_fp2.fzs[g_pich2].frs.end(); ++it)
			{
				FightRole *fr = (*it).second;
				range_frs.push_back(fr);
			}

			/// 选择行动者
			int gindex = -1;
			int gtype = 0;
			while (g_ewxd.size() > 0)
			{
				int tsite = g_ewxd.front();
				g_ewxd.pop_front();
				int ttype = g_ewxd.front();
				g_ewxd.pop_front();
				for (int i = 0; i < range_frs.size(); ++i)
				{
					FightRole *fr = range_frs[i];
					if (fr && fr->site == tsite)
					{
						gindex = i;
						gtype = ttype;
						break;
					}
				}
				if (gindex != -1)
				{
					break;
				}
			}
			if (gindex == -1)
			{
				int max_xg = -99999999;
				int hsite = 0;
				for (int i = 0; i < range_frs.size(); ++i)
				{
					FightRole *fr = range_frs[i];
					if (!fr)
					{
						continue;
					}
					if (has_site.find(fr->site) != has_site.end())
					{
						continue;
					}
					int xg = fr->get_attr(5);
					if (xg > max_xg)
					{
						max_xg = xg;
						gindex = i;
						hsite = fr->site;
					}
				}
				if (gindex == -1)
				{
					break;
				}
				has_site.insert(hsite);
			}
			mission_deal(gindex, range_frs, gtype);
		}

		// 宠物
		int pet_xg[2] = { 0, 0 };
		while (1)
		{
			if (g_fp1.fzs[g_pich1].frs.size() == 0)
			{
				break;
			}
			if (g_fp2.fzs[g_pich2].frs.size() == 0)
			{
				break;
			}
			int xg = -1;
			std::vector<FightRole *> range_frs;
			if (g_fp1.fzs[g_pich1].pet &&
				g_fp2.fzs[g_pich2].pet)
			{
				if (pet_xg[0] != 0 &&
					pet_xg[1] != 0)
				{
					break;
				}
				if (g_fp1.fzs[g_pich1].pet->bf >= g_fp2.fzs[g_pich2].pet->bf)
				{
					xg = 0;
					if (pet_xg[0] != 0)
					{
						xg = 1;
					}
				}
				else
				{
					xg = 1;
					if (pet_xg[1] != 0)
					{
						xg = 0;
					}
				}
			}
			else if (g_fp1.fzs[g_pich1].pet &&
				!g_fp2.fzs[g_pich2].pet)
			{
				xg = 0;
				if (pet_xg[0] != 0)
				{
					break;
				}
			}
			else if (!g_fp1.fzs[g_pich1].pet &&
				g_fp2.fzs[g_pich2].pet)
			{
				xg = 1;
				if (pet_xg[1] != 0)
				{
					break;
				}
			}
			else
			{
				break;
			}
			if (xg == -1)
			{
				break;
			}
			if (xg == 0)
			{
				pet_xg[0] = 1;
				range_frs.push_back(g_fp1.fzs[g_pich1].pet);
				for (std::map<int, FightRole *>::iterator it = g_fp2.fzs[g_pich2].frs.begin(); it != g_fp2.fzs[g_pich2].frs.end(); ++it)
				{
					FightRole *fr = (*it).second;
					range_frs.push_back(fr);
				}
			}
			else
			{
				pet_xg[1] = 1;
				range_frs.push_back(g_fp2.fzs[g_pich2].pet);
				for (std::map<int, FightRole *>::iterator it = g_fp1.fzs[g_pich1].frs.begin(); it != g_fp1.fzs[g_pich1].frs.end(); ++it)
				{
					FightRole *fr = (*it).second;
					range_frs.push_back(fr);
				}
			}
			mission_deal(0, range_frs, 0);
		}

		mission_bingyuan_vec(false);
		if (g_fp1.fzs[g_pich1].empty())
		{
			g_pich1++;
			g_round = 0;
			/// 初始化光环
			if (g_fp1.fzs.size() > g_pich1)
			{
				protocol::game::msg_fight_bo *bo = g_text.add_bos();
				protocol::game::msg_fight_state *state = bo->mutable_bo_state();
				missoin_make_state(state);
				for (std::map<int, FightRole *>::iterator it = g_fp1.fzs[g_pich1].frs.begin(); it != g_fp1.fzs[g_pich1].frs.end(); ++it)
				{
					(*it).second->add_helo();
				}
			}
		}
		else if (g_fp2.fzs[g_pich2].empty())
		{
			g_pich2++;
			for (std::map<int, FightRole *>::iterator it = g_yj.begin(); it != g_yj.end();)
			{
				if ((*it).first == g_pich2 + 1)
				{
					FightRole *fr = (*it).second;
					g_fp1.fzs[g_pich1].frs[fr->site] = fr;
					g_yj.erase(it);
					break;
				}
				else
				{
					++it;
				}
			}
			g_round = 0;
			/// 初始化光环
			if (g_fp2.fzs.size() > g_pich2)
			{
				protocol::game::msg_fight_bo *bo = g_text.add_bos();
				protocol::game::msg_fight_state *state = bo->mutable_bo_state();
				missoin_make_state(state);
				for (std::map<int, FightRole *>::iterator it = g_fp2.fzs[g_pich2].frs.begin(); it != g_fp2.fzs[g_pich2].frs.end(); ++it)
				{
					(*it).second->add_helo();
				}
			}
		}
	}
	mission_make_tick(99);
	state = g_text.mutable_end_state();
	missoin_make_state(state);
	mission_bingyuan_vec(true);
	int res = -2;
	if (g_pich1 < g_fp1.fzs.size() && g_pich2 < g_fp2.fzs.size())
	{
		res = 0;
	}
	else if (g_pich1 < g_fp1.fzs.size())
	{
		res = 1;
	}
	else if (g_pich2 < g_fp2.fzs.size())
	{
		res = -1;
	}
	return res;
}

int MissionFight::mission_deal(int gindex, std::vector<FightRole *> &range_frs, int gtype)
{
	int dr_num = 0;
	for (int i = 0; i < range_frs.size(); ++i)
	{
		if (range_frs[i] && range_frs[i]->cp != range_frs[gindex]->cp)
		{
			dr_num++;
		}
	}
	range_frs[gindex]->do_buff(1);
	while (1)
	{
		if (mission_die(range_frs) == -1)
		{
			break;
		}
	}
	if (range_frs[gindex] == 0)
	{
		return 0;
	}
	if (range_frs[gindex]->get_attr(13) > 0.1f)
	{
		range_frs[gindex]->do_buff(2);
		return 0;
	}
	int at_type = mission_attack(gindex, range_frs, false, gtype);
	while (1)
	{
		if (mission_die(range_frs) == -1)
		{
			break;
		}
	}
	if (range_frs[gindex] == 0)
	{
		return 0;
	}
	int jf_num = 0;
	while (at_type == 0)
	{
		if (jf_num == 0 && !range_frs[gindex]->get_tp_skill7())
		{
			break;
		}
		else if (jf_num > 0 && !range_frs[gindex]->get_zs_skill204())
		{
			break;
		}
		jf_num++;
		int num = 0;
		for (int i = 0; i < range_frs.size(); ++i)
		{
			if (range_frs[i] == 0)
			{
				continue;
			}
			if (range_frs[i]->cp != range_frs[gindex]->cp)
			{
				num++;
			}
		}
		if (num != 0)
		{
			MissionFight::mission_make_tick(17);
			g_tick->add_values(range_frs[gindex]->site);
			mission_attack(gindex, range_frs, true, gtype);
			while (1)
			{
				if (mission_die(range_frs) == -1)
				{
					break;
				}
			}
			if (range_frs[gindex] == 0)
			{
				return 0;
			}
		}
		else
		{
			break;
		}
	}
	double hp = 0;
	if (range_frs[gindex]->get_tp_skill10(hp))
	{
		mission_hx(range_frs[gindex]->site, range_frs[gindex]->cp, hp, range_frs);
	}
	if (range_frs[gindex]->get_tp_skill21())
	{
		mission_nl(range_frs[gindex]->site, range_frs[gindex]->cp, range_frs);
	}
	range_frs[gindex]->get_zs_skill405();
	int dr_num1 = 0;
	for (int i = 0; i < range_frs.size(); ++i)
	{
		if (range_frs[i] && range_frs[i]->cp != range_frs[gindex]->cp)
		{
			dr_num1++;
		}
	}
	if (range_frs[gindex]->get_zs_skill500())
	{
		g_ewxd.push_back(range_frs[gindex]->site);
		g_ewxd.push_back(0);
	}
	if (range_frs[gindex]->get_zs_skill503(dr_num, dr_num1))
	{
		g_ewxd.push_back(range_frs[gindex]->site);
		g_ewxd.push_back(0);
	}
	return 0;
}

int MissionFight::mission_die(std::vector<FightRole *> &range_frs)
{
	int res = -1;
	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *fr = range_frs[i];
		if (!fr)
		{
			continue;
		}
		if (fr->is_pet)
		{
			continue;
		}
		if (fr->cur_hp > 0)
		{
			continue;
		}
		double fh = 0;
		if (fr->get_tp_skill6(fh))
		{
			fr->get_zs_skill209();
			fr->get_zs_skill505();
			int hp = fr->get_attr(1) * fh;
			fr->set_cur_hp(hp);
			MissionFight::mission_make_tick(5);
			g_tick->add_values(fr->site);
			g_tick->add_values(fr->nengliang);
			g_tick->add_dvalues(hp);
			continue;
		}
		
		res = 0;
		MissionFight::mission_make_tick(6);
		g_tick->add_values(fr->site);
		/// 复仇
		for (int j = 0; j < range_frs.size(); ++j)
		{
			FightRole *tfr = range_frs[j];
			if (!tfr)
			{
				continue;
			}
			if (tfr->cp == fr->cp && tfr != fr)
			{
				if (tfr->get_tp_skill14())
				{
					MissionFight::mission_make_tick(7);
					g_tick->add_values(tfr->site);
				}
			}
		}
		double atk = fr->get_attr(1);
		double gj_208;
		int skill_id = fr->get_zs_skill208(gj_208);
		int site = fr->site;
		int cp = fr->cp;
		double s17 = 0;
		bool h17 = fr->get_tp_skill17(s17);
		double s18 = 0;
		bool h18 = fr->get_tp_skill18(s18);
		fr->del_helo();
		if (cp == 0)
		{
			g_fp1.die(fr);
			g_die_role++;
		}
		else
		{
			g_fp2.die(fr);
		}
		range_frs[i] = 0;
		/// 奉献
		if (h18)
		{
			mission_fx(site, cp, s18, range_frs, skill_id, gj_208);
		}
		/// 爆炸
		if (h17)
		{
			mission_bz(site, cp, s17, range_frs);
		}
	}
	
	return res;
}

int MissionFight::mission_attack(int gindex, std::vector<FightRole *> &range_frs, bool jf, int gtype)
{
	FightRole *fr = range_frs[gindex];
	if (fr->is_pet)
	{
		MissionFight::mission_make_tick(24);
	}
	else
	{
		MissionFight::mission_make_tick(23);
	}
	g_tick->add_values(fr->site);
	int type = 0;
	if (gtype == 1)
	{
		type = 1;
	}
	else if (gtype == 2)
	{
		type = 0;
	}
	else
	{
		if (fr->nengliang >= 4 && !jf)
		{
			type = 1;
			fr->nengliang = fr->nengliang - 4;
		}
		else
		{
			if (fr->is_pet)
			{

			}
			else if (fr->is_boss)
			{
				fr->nengliang += 1;
			}
			else
			{
				fr->nengliang += 2;
			}
		}
	}
	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tmpfr = range_frs[i];
		if (tmpfr && tmpfr->cp != fr->cp)
		{
			if (tmpfr->get_zs_skill213(type))
			{
				type = 0;
				break;
			}
		}
	}
	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tmpfr = range_frs[i];
		if (tmpfr && tmpfr != fr && tmpfr->cp == fr->cp)
		{
			if (tmpfr->get_zs_skill402())
			{
				g_ewxd.push_back(tmpfr->site);
				g_ewxd.push_back(2);
				break;
			}
		}
	}
	s_t_skill *t_skill = fr->get_skill(type, fr->pinzhi_skill);
	if (!t_skill)
	{
		return -1;
	}

	int is_xx = 0;
	double zgxx = 0;
	int is_ft = 0;
	double zgft = 0;
	std::vector<int> targets;
	mission_get_target(t_skill->target_type, t_skill->range, gindex, fr->cp, targets, range_frs);
	double atk = fr->get_attr(2);
	for (int i = 0; i < targets.size(); ++i)
	{
		int site2 = targets[i];
		FightRole *tfr = range_frs[site2];
		double _attack = atk;
		fr->get_zs_skill400(_attack);
		double _defense = 0.0f;
		int is_my = 0;
		int is_sb = 0;
		int is_bj = 0;
		int is_gd = 0;
		int is_ht = 0;
		int is_ws = 0;
		int is_dl = 0;
		int mp_ch = 0;
		int is_shbei = 0;
		int is_yl = 0;
		double zgyl = 0;
		int is_stun = 0;
		double hundun_attack = 0;

		/// 判断闪避
		if (t_skill->attack_type < 3 && !fr->is_pet && !fr->is_boss && !tfr->is_boss)
		{
			double sb = tfr->get_attr(17) - fr->get_attr(16) + 5;
			if (sb > 25)
			{
				sb = 25;
			}
			int r = Utils::get_int32(0, 99);
			if (sb > r)
			{
				is_sb = 1;
			}
		}
		if (fr->cp != tfr->cp && !fr->is_pet && !tfr->is_pet && !fr->is_boss)
		{
			int nnl = tfr->get_kz_skill_mf(fr->per);
			if (nnl > 0)
			{
				is_sb = 1;
				tfr->nengliang += nnl;
				MissionFight::mission_make_tick(19);
				g_tick->add_values(tfr->site);
				MissionFight::mission_make_tick(20);
				g_tick->add_values(tfr->site);
				g_tick->add_values(nnl);
				g_tick->add_values(tfr->nengliang);
			}
		}
		/// 减能量
		if (!is_sb && t_skill->mp_target_type == 2 && t_skill->mp && Utils::get_int32(0, 99) < t_skill->mp_rate && !tfr->is_boss)
		{
			tfr->add_nl(t_skill->mp);
			if (t_skill->mp > 0)
			{
				mp_ch = 1;
			}
			else
			{
				mp_ch = 2;
			}
		}
		/// 晕眩
		if (!is_sb && !tfr->is_boss)
		{
			if (t_skill->stun_rate > 0 && Utils::get_int32(0, 99) < t_skill->stun_rate)
			{
				is_stun = 1;
			}
			else if (fr->get_zs_skill206(fr->get_attr(3), tfr->get_attr(3)))
			{
				is_stun = 1;
			}
		}

		/// 物理魔法攻击
		if (t_skill->attack_type < 3)
		{
			if (fr->is_boss)
			{
				if (type != 0)
				{
					_attack = 99999999;
				}
				else
				{
					_attack = 1;
				}
				double ychu = 0;
				if (tfr->get_tp_skill8(_attack, ychu))
				{
					is_dl = 1;
				}
				tfr->set_cur_hp(tfr->cur_hp - _attack);
			}
			else
			{
				if (!is_sb)
				{
					if (t_skill->attack_type == 1)
					{
						/// 物理攻击
						_defense = tfr->get_attr(3);
						if (_defense < 0)
						{
							_defense = 0;
						}
					}
					else if (t_skill->attack_type == 2)
					{
						/// 魔法攻击
						_defense = tfr->get_attr(4);
						if (_defense < 0)
						{
							_defense = 0;
						}
					}
					/// EX技能3 第一回合无视防御
					if (fr->get_tp_skill9())
					{
						_defense = 0;
						is_ws = 1;
					}
					/// 装备技能 无视护甲
					double wushi = 0;
					if (fr->get_equip_skill5(wushi))
					{
						_defense = _defense * (1 - wushi);
						is_ws = 1;
					}
					double _tp_attack = _attack - _defense;
					if (_tp_attack < _attack * 0.1f)
					{
						_attack = _attack * 0.1f;
					}
					else
					{
						_attack = _tp_attack;
					}
					_attack = _attack * t_skill->get_attack_pe(fr->skill_level);

					int bj = fr->get_attr(11) - tfr->get_attr(18);
					int r = Utils::get_int32(0, 99);
					if (bj > r)
					{
						/// 暴击
						_attack = _attack * 1.5f;
						is_bj = 1;
						fr->get_zs_skill214(_attack, tfr->get_attr(1), is_bj);
					}

					if (fr->get_equip_skill1(_attack))
					{
						is_shbei = 1;
					}

					int gd = tfr->get_attr(12) - fr->get_attr(19);
					r = Utils::get_int32(0, 99);
					if (gd > r && !fr->is_pet)
					{
						/// 格挡
						_attack = _attack * 0.5f;
						is_gd = 1;
					}

					if (t_skill->attack_type == 1)
					{
						double _imm = tfr->get_attr(14);
						if (_imm > 70.0f)
						{
							_imm = 70;
						}
						if (_imm < 0)
						{
							_imm = 0;
						}
						_imm *= 0.01f;
						_attack *= (1.0f - _imm);
					}
					else if (t_skill->attack_type == 2)
					{
						double _imm = tfr->get_attr(15);
						if (_imm > 70.0f)
						{
							_imm = 70;
						}
						if (_imm < 0)
						{
							_imm = 0;
						}
						_imm *= 0.01f;
						_attack *= (1.0f - _imm);
					}
					/// 增减伤灭和抗
					int mxing = 22 + fr->per;
					int kxing = 25 + tfr->per;
					double zs = 100 + fr->get_attr(20) - tfr->get_attr(21) + fr->get_attr(mxing) - tfr->get_attr(kxing);
					if (g_fight_type == 0)
					{
						zs = zs + fr->get_attr(35) - tfr->get_attr(36);
					}
					if (g_fight_type == 0)
					{
						/// 人对怪最大减伤50%
						if (zs < 50)
						{
							zs = 50;
						}
					}
					else
					{
						if (zs < 20)
						{
							zs = 20;
						}
					}
					zs *= 0.01f;
					_attack *= zs;
					/// 职业克制
					fr->get_tp_skill11(_attack, tfr->per);
					tfr->get_tp_skill12(_attack, fr->per);
					/// 技能克制
					if (fr->cp != tfr->cp && !fr->is_pet && !tfr->is_pet && tfr->get_kz_skill_fw(_attack, is_gd, fr->per))
					{
						FightBuff fb;
						fb.col = 0;
						fb.id = tfr->kz_skill.t_skill->id;
						fb.round = tfr->kz_skill.t_skill->buffer_rounds[0];;
						fb.level = tfr->skill_level;
						tfr->add_buff(fb);
						MissionFight::mission_make_tick(16);
						g_tick->add_values(tfr->site);
						g_tick->add_values(tfr->kz_skill.t_skill->id);
						g_tick->add_values(fb.col);
					}
					/// 金刚
					if (type == 0)
					{
						if (tfr->get_tp_skill15(_attack))
						{
							is_ht = 1;
						}
					}
					/// 品质克制
					fr->get_tp_skill19(_attack, fr->pinzhi, tfr->pinzhi);
					tfr->get_tp_skill20(_attack, tfr->pinzhi, fr->pinzhi);
					/// 护体
					if (type == 0)
					{
						if (tfr->get_equip_skill6(_attack))
						{
							is_ht = 1;
						}
					}
					/// 专属
					fr->get_zs_skill201(_attack, type, is_stun, mp_ch);
					fr->get_zs_skill202(_attack, type, targets.size());
					fr->get_zs_skill207(_attack, type, is_stun);
					fr->get_zs_skill212(type);
					tfr->get_zs_skill215_1();
					if (tfr->get_zs_skill404())
					{
						fr->add_nl(-1);
					}
					fr->get_zs_skill406(_attack, tfr->fuhuo);
					tfr->get_zs_skill502();

					/// 伤害不小于10%
					if (_attack < atk * 0.1f)
					{
						_attack = atk * 0.1f;
					}

					if (tfr->mianyi)
					{
						tfr->mianyi = 0;
						is_my = t_skill->attack_type;
						MissionFight::mission_make_tick(22);
						g_tick->add_values(tfr->site);
						g_tick->add_values(2);
					}
					if (tfr->get_zs_skill504())
					{
						is_my = t_skill->attack_type;
					}

					if (is_my)
					{
						_attack = 0;
					}
					else
					{
						/// 克制技能护盾
						if (tfr->kzdy && tfr->kzdyhp > 0.1 && (!fr->is_pet) && (!tfr->is_pet) && tfr->kz_skill.t_skill && t_skill->attack_type == 2)
						{
							double kzdybe = tfr->kzdyhp;
							tfr->kzdyhp -= _attack;
							if (tfr->kzdyhp > 0.1)
							{
								hundun_attack = _attack;
								_attack = 0;
							}
							else
							{
								hundun_attack = kzdybe;
								_attack = _attack - kzdybe;
								tfr->kzdyhp = 0;
							}
						}
						/// 动力护甲
						double ychu = 0;
						double fantan = 0;
						if (tfr->get_tp_skill8(_attack, ychu))
						{
							is_dl = 1;
							fantan = 0;
							if (tfr->get_zs_skill205(fantan, ychu) && (!fr->is_pet))
							{
								is_ft = t_skill->attack_type;
								zgft += fantan;
								fr->set_cur_hp(fr->cur_hp - fantan);
							}
							tfr->get_zs_skill401(ychu);
						}
						tfr->set_cur_hp(tfr->cur_hp - _attack);

						fr->get_zs_skill403(_attack, range_frs);

						double yili = 0;
						if (tfr->get_equip_skill2(yili))
						{
							zgyl = yili * tfr->get_attr(1);
							tfr->set_cur_hp(tfr->cur_hp + zgyl);
							is_yl = 1;
						}

						fantan = 0;
						if (t_skill->attack_type == 1 && tfr->get_equip_skill3(fantan))
						{
							is_ft = 1;
							fantan = fantan * _attack;
							zgft += fantan;
							fr->set_cur_hp(fr->cur_hp - fantan);
						}
						else if (t_skill->attack_type == 2 && tfr->get_equip_skill4(fantan) && (!fr->is_pet))
						{
							is_ft = 2;
							fantan = fantan * _attack;
							zgft += fantan;
							fr->set_cur_hp(fr->cur_hp - fantan);
						}
						fantan = 0;
						if (tfr->get_tp_skill22(fantan) && (!fr->is_pet))
						{
							is_ft = t_skill->attack_type;
							fantan = fantan * _attack;
							zgft += fantan;
							fr->set_cur_hp(fr->cur_hp - fantan);
						}
						double gxx = 0;
						if (fr->get_tp_skill5(gxx))
						{
							gxx = _attack * gxx;
							fr->set_cur_hp(fr->cur_hp + gxx);
							is_xx = 1;
							zgxx += gxx;
						}
						gxx = 0;
						if (tfr->cur_hp < 0)
						{
							if (tfr->get_zs_skill501())
							{
								g_ewxd.push_back(tfr->site);
								g_ewxd.push_back(1);
							}
							else if (fr->get_zs_skill203(gxx))
							{
								fr->set_cur_hp(fr->cur_hp + gxx);
								is_xx = 1;
								zgxx += gxx;
							}
						}

						if (fr->cp != tfr->cp && !tfr->kzdy && !fr->is_pet && !tfr->is_pet && tfr->get_kz_skill_wl(_attack, fr->per))
						{
							tfr->kzdy = 1;
							tfr->kzmz = 1;
							tfr->kzdyhp = tfr->cur_hp * tfr->kz_skill.eval1 / 100.0;
							MissionFight::mission_make_tick(26);
							g_tick->add_values(tfr->site);
							g_tick->add_values(1);
							g_tick->add_dvalues(tfr->cur_hp * tfr->kz_skill.eval1 / 100.0);

							FightBuff fb;
							fb.col = 0;
							fb.id = tfr->kz_skill.t_skill->id;
							fb.round = tfr->kz_skill.t_skill->buffer_rounds[0];;
							fb.level = tfr->skill_level;
							tfr->add_buff(fb);
							MissionFight::mission_make_tick(16);
							g_tick->add_values(tfr->site);
							g_tick->add_values(tfr->kz_skill.t_skill->id);
							g_tick->add_values(fb.col);
						}

						if (tfr->kzdy && tfr->kzdyhp < 0.1)
						{
							tfr->kzdy = 0;
							tfr->kzdyhp = 0;
							tfr->kzmz = 0;
							MissionFight::mission_make_tick(26);
							g_tick->add_values(tfr->site);
							g_tick->add_values(2);
						}
					}
				}
				else
				{
					double hp = 0;
					if (tfr->get_zs_skill215_2(hp))
					{
						mission_hx(tfr->site, tfr->cp, hp, range_frs);
					}
				}
			}

			MissionFight::mission_make_tick(15);
			g_tick->add_values(tfr->site);
			g_tick->add_values(t_skill->attack_type);
			g_tick->add_values(tfr->nengliang);
			g_tick->add_values(is_my);
			g_tick->add_values(is_sb);
			g_tick->add_values(is_bj);
			g_tick->add_values(is_gd);
			g_tick->add_values(mp_ch);
			g_tick->add_values(is_ht);
			g_tick->add_values(is_ws);
			g_tick->add_values(is_dl);
			g_tick->add_values(is_shbei);
			g_tick->add_values(is_yl);
			g_tick->add_dvalues(_attack);
			g_tick->add_dvalues(tfr->cur_hp);
			g_tick->add_dvalues(zgyl);
			g_tick->add_dvalues(hundun_attack);
		}
		else if (t_skill->attack_type == 3)
		{
			/// 治疗
			_attack = _attack * t_skill->get_attack_pe(fr->skill_level);
			fr->get_zs_skill211(_attack, type, tfr->get_attr(1));
			tfr->set_cur_hp(tfr->cur_hp + _attack);

			MissionFight::mission_make_tick(15);
			g_tick->add_values(tfr->site);
			g_tick->add_values(t_skill->attack_type);
			g_tick->add_values(tfr->nengliang);
			g_tick->add_values(mp_ch);
			g_tick->add_dvalues(_attack);
			g_tick->add_dvalues(tfr->cur_hp);
		}
		else
		{
			/// buff
			MissionFight::mission_make_tick(15);
			g_tick->add_values(tfr->site);
			g_tick->add_values(t_skill->attack_type);
			g_tick->add_values(tfr->nengliang);
			g_tick->add_values(mp_ch);
			g_tick->add_dvalues(tfr->cur_hp);
		}

		if (is_stun)
		{
			FightBuff fb;
			fb.col = 0;
			fb.id = 999;
			fb.round = t_skill->stun_round;
			if (fr == tfr)
			{
				fb.round++;
			}
			fb.level = fr->skill_level;
			tfr->add_buff(fb);

			MissionFight::mission_make_tick(16);
			g_tick->add_values(tfr->site);
			g_tick->add_values(999);
			g_tick->add_values(fb.col);
		}
		for (int col = 0; col < 2; ++col)
		{
			if (t_skill->buffer_types[col] == 2)
			{
				if (!is_sb && t_skill->buffer_target_types[col] == 2)
				{
					FightBuff fb;
					fb.col = col;
					fb.id = t_skill->id;
					fb.round = t_skill->buffer_rounds[col];
					if (fr == tfr)
					{
						fb.round++;
					}
					fb.level = fr->skill_level;
					tfr->add_buff(fb);

					MissionFight::mission_make_tick(16);
					g_tick->add_values(tfr->site);
					g_tick->add_values(t_skill->id);
					g_tick->add_values(fb.col);
				}
			}
			else if (t_skill->buffer_types[col] == 1)
			{
				if (!is_sb && t_skill->buffer_target_types[col] == 2)
				{
					FightBuff fb;
					fb.col = col;
					fb.id = t_skill->id;
					fb.round = t_skill->buffer_rounds[col];
					fb.level = fr->skill_level;
					fb.attack = fr->get_attr(2);
					tfr->add_buff(fb);

					MissionFight::mission_make_tick(16);
					g_tick->add_values(tfr->site);
					g_tick->add_values(t_skill->id);
					g_tick->add_values(fb.col);
				}
				else if (t_skill->buffer_target_types[col] == 1)
				{
					FightBuff fb;
					fb.col = col;
					fb.id = t_skill->id;
					fb.round = t_skill->buffer_rounds[col];
					fb.level = fr->skill_level;
					fb.attack = fr->get_attr(2);
					fr->add_buff(fb);

					MissionFight::mission_make_tick(16);
					g_tick->add_values(fr->site);
					g_tick->add_values(t_skill->id);
					g_tick->add_values(fb.col);
				}
			}
		}
	}

	fr->do_buff(2);
	for (int col = 0; col < 2; ++col)
	{
		if (t_skill->buffer_types[col] == 2 && t_skill->buffer_target_types[col] == 1)
		{
			FightBuff fb;
			fb.col = col;
			fb.id = t_skill->id;
			fb.round = t_skill->buffer_rounds[col];
			fb.level = fr->skill_level;
			fr->add_buff(fb);

			MissionFight::mission_make_tick(16);
			g_tick->add_values(fr->site);
			g_tick->add_values(t_skill->id);
			g_tick->add_values(fb.col);
		}
	}

	int fmp_ch = 0;
	if (t_skill->mp_target_type == 1 && t_skill->mp && Utils::get_int32(0, 99) < t_skill->mp_rate)
	{
		fr->add_nl(t_skill->mp);
		if (t_skill->mp > 0)
		{
			fmp_ch = 1;
		}
		else
		{
			fmp_ch = 2;
		}
	}
	{
		MissionFight::mission_make_tick(14);
		g_tick->add_values(fr->site);
		g_tick->add_values(t_skill->id);
		g_tick->add_values(fr->nengliang);
		g_tick->add_values(is_xx);
		g_tick->add_values(is_ft);
		g_tick->add_values(fmp_ch);
		g_tick->add_dvalues(zgxx);
		g_tick->add_dvalues(fr->cur_hp);
		g_tick->add_dvalues(zgft);
	}
	return type;
}

int MissionFight::mission_hx(int site, int cp, double dmg, std::vector<FightRole *> &range_frs)
{
	MissionFight::mission_make_tick(12);
	g_tick->add_values(site);

	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tfr = range_frs[i];
		if (!tfr)
		{
			continue;
		}
		if (tfr->cp == cp)
		{
			tfr->set_cur_hp(tfr->cur_hp + dmg);
			MissionFight::mission_make_tick(13);
			g_tick->add_values(tfr->site);
			g_tick->add_dvalues(dmg);
			g_tick->add_dvalues(tfr->cur_hp);
		}
	}
	return 0;
}

int MissionFight::mission_nl(int site, int cp, std::vector<FightRole *> &range_frs)
{
	MissionFight::mission_make_tick(19);
	g_tick->add_values(site);

	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tfr = range_frs[i];
		if (!tfr)
		{
			continue;
		}
		if (tfr->cp == cp)
		{
			tfr->add_nl(1);
			MissionFight::mission_make_tick(20);
			g_tick->add_values(tfr->site);
			g_tick->add_values(1);
			g_tick->add_values(tfr->nengliang);
		}
	}
	return 0;
}

int MissionFight::mission_fx(int site, int cp, double dmg, std::vector<FightRole *> &range_frs, int skill_id, double gj_208)
{
	MissionFight::mission_make_tick(8);
	g_tick->add_values(site);
	g_tick->add_values(skill_id);

	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tfr = range_frs[i];
		if (!tfr)
		{
			continue;
		}
		if (tfr->cp == cp)
		{
			tfr->set_cur_hp(tfr->cur_hp + dmg);
			if (skill_id)
			{
				tfr->role_attrs[7] += gj_208;
			}
			MissionFight::mission_make_tick(9);
			g_tick->add_values(tfr->site);
			g_tick->add_dvalues(dmg);
			g_tick->add_dvalues(tfr->cur_hp);
		}
	}
	return 0;
}

int MissionFight::mission_bz(int site, int cp, double dmg, std::vector<FightRole *> &range_frs)
{
	MissionFight::mission_make_tick(10);
	g_tick->add_values(site);

	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tfr = range_frs[i];
		if (!tfr)
		{
			continue;
		}
		if (tfr->cp != cp && (!tfr->is_pet))
		{
			tfr->set_cur_hp(tfr->cur_hp - dmg);
			MissionFight::mission_make_tick(11);
			g_tick->add_values(tfr->site);
			g_tick->add_dvalues(dmg);
			g_tick->add_dvalues(tfr->cur_hp);
		}
	}
	return 0;
}

void MissionFight::mission_get_target(int type, int range, int gindex, int cp, std::vector<int> &targets, std::vector<FightRole *> &range_frs)
{
	if (type == 1)
	{
		if (range == 1)
		{
			targets.push_back(gindex);
		}
		else
		{
			for (int i = 0; i < range_frs.size(); ++i)
			{
				FightRole *tfr = range_frs[i];
				if (!tfr)
				{
					continue;
				}
				if (tfr->cp == cp)
				{
					targets.push_back(i);
				}
			}
		}
	}
	else if (type == 2)
	{
		int index = -1;
		double rate = 2;
		for (int i = 0; i < range_frs.size(); ++i)
		{
			FightRole *tfr = range_frs[i];
			if (!tfr)
			{
				continue;
			}
			if (tfr->cp == cp)
			{
				double r = tfr->cur_hp / tfr->get_attr(1);
				if (r < rate)
				{
					rate = r;
					index = i;
				}
			}
		}
		if (index == -1)
		{
			return;
		}
		targets.push_back(index);
	}
	else if (type == 3)
	{
		if (range == 10)
		{
			std::vector<std::pair<double, int> >atk;
			//std::vector<std::pair<double, int>>::iterator it;
			std::pair<double, int> par;
			for (int i = 0; i < range_frs.size(); ++i)
			{
				FightRole *tfr = range_frs[i];
				if (!tfr)
				{
					continue;
				}
				if (tfr->cp != cp)
				{
					double fit = tfr->get_attr(2);
					atk.push_back(std::make_pair(fit, i));
				}
			}
			sort(atk.begin(), atk.end(), judge);
			par = atk.back();

			targets.push_back(par.second);
		}
		else if (range == 9)
		{
			for (int i = 0; i < range_frs.size(); ++i)
			{
				FightRole *tfr = range_frs[i];
				if (!tfr)
				{
					continue;
				}
				if (tfr->cp != cp)
				{
					targets.push_back(i);
				}
			}
		}
		else
		{
			bool flag1 = false;
			bool flag2 = false;
			bool flag3 = false;
			for (int i = 0; i < range_frs.size(); ++i)
			{
				FightRole *tfr = range_frs[i];
				if (!tfr)
				{
					continue;
				}
				if (tfr->cp == cp)
				{
					continue;
				}
				if (tfr->duiwei < 6)
				{
					flag1 = true;
				}
				else if(tfr->duiwei < 12)
				{
					flag2 = true;
				}
				else
				{
					flag3 = true;
				}
				if (tfr->is_yj)
				{
					targets.push_back(i);
					return;
				}
			}
			if (range == 1)
			{
				if (flag1)
				{
					mission_get_single_target(0, cp, targets, range_frs);
				}
				else if (flag2)
				{
					mission_get_single_target(1, cp, targets, range_frs);
				}
				else
				{
					mission_get_single_target(2, cp, targets, range_frs);
				}
			}
			else if (range == 2)
			{
				if (flag2)
				{
					mission_get_single_target(1, cp, targets, range_frs);
				}
				else if (flag1)
				{
					mission_get_single_target(0, cp, targets, range_frs);
				}
				else
				{
					mission_get_single_target(2, cp, targets, range_frs);
				}
			}
			else if (range == 3)
			{
				if (flag3)
				{
					mission_get_single_target(2, cp, targets, range_frs);
				}
				else if (flag2)
				{
					mission_get_single_target(1, cp, targets, range_frs);
				}
				else
				{
					mission_get_single_target(0, cp, targets, range_frs);
				}
			}
			else if (range == 4)
			{
				if (flag1)
				{
					mission_get_row_target(0, cp, targets, range_frs);
				}
				else if (flag2)
				{
					mission_get_row_target(1, cp, targets, range_frs);
				}
				else
				{
					mission_get_row_target(2, cp, targets, range_frs);
				}
			}
			else if (range == 5)
			{
				if (flag2)
				{
					mission_get_row_target(1, cp, targets, range_frs);
				}
				else if (flag1)
				{
					mission_get_row_target(0, cp, targets, range_frs);
				}
				else
				{
					mission_get_row_target(2, cp, targets, range_frs);
				}
			}
			else if (range == 6)
			{
				if (flag3)
				{
					mission_get_row_target(2, cp, targets, range_frs);
				}
				else if (flag2)
				{
					mission_get_row_target(1, cp, targets, range_frs);
				}
				else
				{
					mission_get_row_target(0, cp, targets, range_frs);
				}
			}
			else if (range == 7)
			{
				mission_get_random_target(3, cp, targets, range_frs);
			}
			else if (range == 8)
			{
				if (flag1)
				{
					mission_get_single_target(0, cp, targets, range_frs);
				}
				else if (flag2)
				{
					mission_get_single_target(1, cp, targets, range_frs);
				}
				else
				{
					mission_get_single_target(2, cp, targets, range_frs);
				}
				int duiwei = range_frs[targets[0]]->duiwei;
				for (int i = 0; i < range_frs.size(); ++i)
				{
					FightRole *tfr = range_frs[i];
					if (!tfr)
					{
						continue;
					}
					if (tfr->cp != cp)
					{
						if (duiwei % 6 != 0 && tfr->duiwei % 6 == duiwei % 6 - 1)
						{
							targets.push_back(i);
						}
						else if (duiwei % 6 != 5 && tfr->duiwei % 6 == duiwei % 6 + 1)
						{
							targets.push_back(i);
						}
					}
				}
			}
		}
	}
}

void MissionFight::mission_get_single_target(int row, int cp, std::vector<int> &targets, std::vector<FightRole *> &range_frs)
{
	std::vector<int> ids;
	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tfr = range_frs[i];
		if (!tfr)
		{
			continue;
		}
		if (tfr->cp != cp && tfr->duiwei / 6 == row)
		{
			ids.push_back(i);
		}
	}
	int r = Utils::get_int32(0, ids.size() - 1);
	targets.push_back(ids[r]);
}

void MissionFight::mission_get_row_target(int row, int cp, std::vector<int> &targets, std::vector<FightRole *> &range_frs)
{
	std::vector<int> ids;
	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tfr = range_frs[i];
		if (!tfr)
		{
			continue;
		}
		if (tfr->cp != cp && tfr->duiwei / 6 == row)
		{
			targets.push_back(i);
		}
	}
}

void MissionFight::mission_get_random_target(int num, int cp, std::vector<int> &targets, std::vector<FightRole *> &range_frs)
{
	std::vector<int> ids;
	for (int i = 0; i < range_frs.size(); ++i)
	{
		FightRole *tfr = range_frs[i];
		if (!tfr)
		{
			continue;
		}
		if (tfr->cp != cp)
		{
			ids.push_back(i);
		}
	}
	if (ids.size() < num)
	{
		num = ids.size();
	}
	for (int i = 0; i < num; ++i)
	{
		int index = Utils::get_int32(0, ids.size() - 1);
		targets.push_back(ids[index]);
		ids[index] = ids[ids.size() - 1];
		ids.pop_back();
	}
}

//////////////////////////////////////////////////////////////////////////

void MissionFight::missoin_make_state(protocol::game::msg_fight_state *state)
{
	if (g_pich1 < g_fp1.fzs.size())
	{
		for (std::map<int, FightRole *>::iterator it = g_fp1.fzs[g_pich1].frs.begin(); it != g_fp1.fzs[g_pich1].frs.end(); ++it)
		{
			FightRole *fr = (*it).second;
			protocol::game::msg_fight_role *role = state->add_roles();
			missoin_make_role(fr, role);
		}
		if (g_fp1.fzs[g_pich1].pet)
		{
			protocol::game::msg_fight_role *role = state->add_roles();
			missoin_make_role(g_fp1.fzs[g_pich1].pet, role);
		}
	}
	if (g_pich2 < g_fp2.fzs.size())
	{
		for (std::map<int, FightRole *>::iterator it = g_fp2.fzs[g_pich2].frs.begin(); it != g_fp2.fzs[g_pich2].frs.end(); ++it)
		{
			FightRole *fr = (*it).second;
			protocol::game::msg_fight_role *role = state->add_roles();
			missoin_make_role(fr, role);
		}
		if (g_fp2.fzs[g_pich2].pet)
		{
			protocol::game::msg_fight_role *role = state->add_roles();
			missoin_make_role(g_fp2.fzs[g_pich2].pet, role);
		}
	}
}

void MissionFight::missoin_make_role(FightRole *fr, protocol::game::msg_fight_role *role)
{
	role->set_site(fr->site);
	role->set_duiwei(fr->duiwei);
	role->set_id(fr->id);
	role->set_max_hp(fr->get_attr(1));
	role->set_cur_hp(fr->cur_hp);
	role->set_nengliang(fr->nengliang);
	role->set_jlevel(fr->jlevel);
	role->set_glevel(fr->glevel);
	role->set_pinzhi(fr->pinzhi);
	role->set_dress_id(fr->dress_id);
	role->set_guanghuan_id(fr->guanghuan_id);
	for (std::map<int, FightBuff>::iterator it = fr->buffs.begin(); it != fr->buffs.end(); ++it)
	{
		int id = (*it).second.id;
		role->add_buff_ids(id);
	}
}

void MissionFight::mission_make_tick(int type)
{
	protocol::game::msg_fight_bo *bo = g_text.mutable_bos(g_text.bos_size() - 1);
	g_tick = bo->add_ticks();
	g_tick->set_type(type);
}

void MissionFight::mission_make_fail()
{
	double hp = 0;
	for (int i = 0; i < g_fp1.fzs[0].frs.size(); ++i)
	{
		FightRole *fr = g_fp1.fzs[0].frs[i];
		if (fr)
		{
			hp += fr->cur_hp;
		}
	}
	hp = hp / g_total_hp * 100;
	if (g_min_hp != -1 && hp < g_min_hp)
	{
		g_text.set_fail_type(3);
		g_text.set_fail_param(g_min_hp);
	}
	else if (g_die_role > g_max_die_role)
	{
		g_text.set_fail_type(2);
		g_text.set_fail_param(g_max_die_role);
	}
	else if (g_round > g_max_round)
	{
		g_text.set_fail_type(1);
		g_text.set_fail_param(g_max_round);
	}
	else
	{
		g_text.set_fail_type(0);
	}
}

void MissionFight::mission_bingyuan_vec(bool end)
{
	if (g_fight_type == 2 && (g_round <= 2 || end))
	{
		if (g_bingyuan_vec.size() == 0)
		{
			for (int i = 0; i < 8; ++i)
			{
				g_bingyuan_vec.push_back(-1);
			}
		}
		int index = g_round * 2;
		if (end)
		{
			index = 6;
		}
		double hp = 0;
		if (g_fp1.fzs.size() > g_pich1)
		{
			for (std::map<int, FightRole *>::iterator it = g_fp1.fzs[g_pich1].frs.begin(); it != g_fp1.fzs[g_pich1].frs.end(); ++it)
			{
				hp += (*it).second->cur_hp;
			}
			if (hp < 0)
			{
				hp = 0;
			}
		}
		g_bingyuan_vec[index] = hp;

		hp = 0;
		if (g_fp1.fzs.size() > g_pich2)
		{
			for (std::map<int, FightRole *>::iterator it = g_fp2.fzs[g_pich2].frs.begin(); it != g_fp2.fzs[g_pich2].frs.end(); ++it)
			{
				hp += (*it).second->cur_hp;
			}
			if (hp < 0)
			{
				hp = 0;
			}
		}
		g_bingyuan_vec[index + 1] = hp;
	}
}
