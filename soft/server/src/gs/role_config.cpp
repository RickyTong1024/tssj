#include "role_config.h"
#include "dbc.h"
#include "utils.h"

int RoleConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_chouka.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_chouka t_chouka;
		t_chouka.id = dbfile->Get(i, 0)->iValue;
		t_chouka.type = dbfile->Get(i, 2)->iValue;
		t_chouka.num = dbfile->Get(i, 3)->iValue;
		for (int j = 0; j < 3; ++j)
		{
			int rate = dbfile->Get(i, 4 + j)->iValue;
			t_chouka.rates.push_back(rate);
		}
		t_chouka.bcx = dbfile->Get(i, 7)->iValue;

		t_choukas_.push_back(t_chouka);
	}

	dbfile = game::scheme()->get_dbc("t_role.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role t_role;
		t_role.id = dbfile->Get(i, 0)->iValue;
		t_role.job = dbfile->Get(i, 1)->iValue;
		t_role.name = dbfile->Get(i, 4)->pString;
		//t_role.name = game::scheme()->get_lang_str(t_role.name);
		t_role.font_color = dbfile->Get(i, 10)->iValue;
		t_role.pinzhi = dbfile->Get(i, 11)->iValue;
		t_role.max_glevel = dbfile->Get(i, 12)->iValue;
		for (int j = 0; j < 5; ++j)
		{
			int cs = dbfile->Get(i, 13 + j * 4)->iValue;
			double cscz = dbfile->Get(i, 14 + j * 4)->dValue;
			double cz = dbfile->Get(i, 15 + j * 4)->dValue;
			double czcz = dbfile->Get(i, 16 + j * 4)->dValue;
			t_role.cs.push_back(cs);
			t_role.cscz.push_back(cscz);
			t_role.cz.push_back(cz);
			t_role.czcz.push_back(czcz);
		}
		t_role.skill0 = dbfile->Get(i, 33)->iValue;
		t_role.skill1 = dbfile->Get(i, 34)->iValue;
		t_role.skill2 = dbfile->Get(i, 35)->iValue;
		for (int j = 0; j < 5; ++j)
		{
			int skill_id = dbfile->Get(i, 36 + j)->iValue;
			t_role.jskills.push_back(skill_id);
		}
		for (int j = 0; j < 15; ++j)
		{
			int skill_id = dbfile->Get(i, 41 + j)->iValue;
			t_role.tskills.push_back(skill_id);
		}

		t_role.kzskills.push_back(dbfile->Get(i, 56)->iValue);
		t_role.kzskills.push_back(dbfile->Get(i, 56)->iValue);
		t_role.kzskills.push_back(dbfile->Get(i, 57)->iValue);
		t_role.kzskills.push_back(dbfile->Get(i, 57)->iValue);
		t_role.kzskills.push_back(dbfile->Get(i, 57)->iValue);
		t_role.kzskills.push_back(dbfile->Get(i, 58)->iValue);
		t_role.exp = dbfile->Get(i, 59)->iValue;
		t_role.jinghua = dbfile->Get(i, 60)->iValue;
		for (int j = 0; j < 8; ++j)
		{
			int jb = dbfile->Get(i, 61 + j)->iValue;
			if (jb)
			{
				t_role.jbs.push_back(jb);
			}
		}
		t_role.ycang = dbfile->Get(i, 69)->iValue;
		t_role.hbb = dbfile->Get(i, 70)->iValue;
		if (t_role.hbb > 0)
		{
			s_t_hbb t_hbb;
			t_hbb.id = t_role.id;
			t_hbb.rate = t_role.hbb;
			t_hbb.color = t_role.font_color;
			t_hbbs_.push_back(t_hbb);
		}

		t_roles_[t_role.id] = t_role;
	}

	dbfile = game::scheme()->get_dbc("t_role_jiban.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role_jiban t_role_jiban;
		t_role_jiban.id = dbfile->Get(i, 0)->iValue;
		t_role_jiban.type = dbfile->Get(i, 3)->iValue;
		for (int j = 0; j < 4; ++j)
		{
			int tid = dbfile->Get(i, 4 + j)->iValue;
			if (tid)
			{
				t_role_jiban.tids.push_back(tid);
			}
		}
		for (int j = 0; j < 2; ++j)
		{
			int attr = dbfile->Get(i, 8 + j * 2)->iValue;
			int value = dbfile->Get(i, 9 + j * 2)->iValue;
			t_role_jiban.attrs.push_back(std::pair<int, int>(attr, value));
		}

		t_role_jiban_[t_role_jiban.id] = t_role_jiban;
	}

	dbfile = game::scheme()->get_dbc("t_role_jibanex.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role_jibanex t_role_jibanex;
		t_role_jibanex.id = dbfile->Get(i, 0)->iValue;
		for (int j = 0; j < 4; ++j)
		{
			int tid = dbfile->Get(i, 3 + j)->iValue;
			if (tid)
			{
				t_role_jibanex.tids.push_back(tid);
			}
		}		
		t_role_jibanex.attr = dbfile->Get(i, 7)->iValue;
		t_role_jibanex.value = dbfile->Get(i, 8)->iValue;

		t_role_jibanex_[t_role_jibanex.id] = t_role_jibanex;
	}

	dbfile = game::scheme()->get_dbc("t_role_skill.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_skill t_skill;
		t_skill.id = dbfile->Get(i, 0)->iValue;
		t_skill.type = dbfile->Get(i, 7)->iValue;
		t_skill.attack_type = dbfile->Get(i, 8)->iValue;
		t_skill.target_type = dbfile->Get(i, 9)->iValue;
		t_skill.range = dbfile->Get(i, 10)->iValue;
		t_skill.attack_pe = dbfile->Get(i, 11)->dValue;
		t_skill.attack_pe_add = dbfile->Get(i, 12)->dValue;
		t_skill.mp_target_type = dbfile->Get(i, 13)->iValue;
		t_skill.mp = dbfile->Get(i, 14)->iValue;
		t_skill.mp_rate = dbfile->Get(i, 15)->iValue;
		for (int j = 0; j < 2; ++j)
		{
			int buffer_type = dbfile->Get(i, 16 + j * 9)->iValue;
			int buffer_target_type = dbfile->Get(i, 17 + j * 9)->iValue;
			int buffer_round = dbfile->Get(i, 18 + j * 9)->iValue;
			int buffer_attack_type = dbfile->Get(i, 19 + j * 9)->iValue;
			double buffer_attack_pe = dbfile->Get(i, 20 + j * 9)->dValue;
			double buffer_attack_pe_add = dbfile->Get(i, 21 + j * 9)->dValue;
			int buffer_modify_att_type = dbfile->Get(i, 22 + j * 9)->iValue;
			double buffer_modify_att_val = dbfile->Get(i, 23 + j * 9)->dValue;
			double buffer_modify_att_val_add = dbfile->Get(i, 24 + j * 9)->dValue;
			t_skill.buffer_types.push_back(buffer_type);
			t_skill.buffer_target_types.push_back(buffer_target_type);
			t_skill.buffer_rounds.push_back(buffer_round);
			t_skill.buffer_attack_types.push_back(buffer_attack_type);
			t_skill.buffer_attack_pes.push_back(buffer_attack_pe);
			t_skill.buffer_attack_pe_adds.push_back(buffer_attack_pe_add);
			t_skill.buffer_modify_att_types.push_back(buffer_modify_att_type);
			t_skill.buffer_modify_att_vals.push_back(buffer_modify_att_val);
			t_skill.buffer_modify_att_val_adds.push_back(buffer_modify_att_val_add);
		}
		t_skill.stun_rate = dbfile->Get(i, 34)->iValue;
		t_skill.stun_round = dbfile->Get(i, 35)->iValue;
		t_skill.passive_type = dbfile->Get(i, 36)->iValue;
		t_skill.passive_modify_att_type = dbfile->Get(i, 37)->iValue;
		t_skill.passive_modify_att_val = dbfile->Get(i, 38)->dValue;
		t_skill.passive_modify_att_val_add = dbfile->Get(i, 39)->dValue;
		t_skill.ex_type = dbfile->Get(i, 40)->iValue;
		t_skill.ex_type_val_0 = dbfile->Get(i, 41)->iValue;
		t_skill.ex_type_val_1 = dbfile->Get(i, 42)->dValue;
		t_skill.ex_type_val_2 = dbfile->Get(i, 43)->dValue;
		t_skill.ex_type_val_add_1 = dbfile->Get(i, 44)->dValue;
		t_skill.ex_type_val_add_2 = dbfile->Get(i, 45)->dValue;
		t_skill.release_rate = dbfile->Get(i, 46)->iValue;

		t_skill_[t_skill.id] = t_skill;
	}

	dbfile = game::scheme()->get_dbc("t_role_tupo.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tupo t_tupo;
		t_tupo.id = dbfile->Get(i, 0)->iValue;
		t_tupo.level = dbfile->Get(i, 1)->iValue;
		for (int j = 0; j < 4; ++j)
		{
			int num = dbfile->Get(i, 2 + j)->iValue;;
			t_tupo.suipian.push_back(num);
		}
		t_tupo.gold = dbfile->Get(i, 6)->iValue;

		t_tupo_[t_tupo.id] = t_tupo;
	}

	dbfile = game::scheme()->get_dbc("t_role_jinjie.txt");
	if (!dbfile)
	{
		return -1;
	}

	double jinjie_point = 0;
	int jinjie_shuxing = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_jinjie t_jinjie;
		t_jinjie.id = dbfile->Get(i, 0)->iValue;
		t_jinjie.level = dbfile->Get(i, 3)->iValue;
		t_jinjie.clty = dbfile->Get(i, 4)->iValue;
		t_jinjie.clty_num = dbfile->Get(i, 5)->iValue;
		t_jinjie.clfy = dbfile->Get(i, 6)->iValue;
		t_jinjie.clfy_num = dbfile->Get(i, 7)->iValue;
		t_jinjie.clfy_num1 = dbfile->Get(i, 8)->iValue;
		t_jinjie.clgj = dbfile->Get(i, 9)->iValue;
		t_jinjie.clgj_num = dbfile->Get(i, 10)->iValue;
		t_jinjie.clgj_num1 = dbfile->Get(i, 11)->iValue;
		t_jinjie.clmf = dbfile->Get(i, 12)->iValue;
		t_jinjie.clmf_num = dbfile->Get(i, 13)->iValue;
		t_jinjie.clmf_num1 = dbfile->Get(i, 14)->iValue;
		t_jinjie.sxper = dbfile->Get(i, 15)->dValue;
		t_jinjie.gold = dbfile->Get(i, 16)->iValue;
		t_jinjie.shuxing = dbfile->Get(i, 17)->iValue;

		t_jinjie_[t_jinjie.id] = t_jinjie;

		jinjie_point += t_jinjie.sxper;
		t_jinjie_point_[t_jinjie.id] = jinjie_point;

		jinjie_shuxing += t_jinjie.shuxing;
		t_jinjie_shuxing_[t_jinjie.id] = jinjie_shuxing;
	}

	dbfile = game::scheme()->get_dbc("t_role_skillup.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_skillup t_skillup;
		t_skillup.level = dbfile->Get(i, 0)->iValue;
		t_skillup.item_num = dbfile->Get(i, 1)->iValue;
		t_skillup.gold = dbfile->Get(i, 2)->iValue;

		t_skillup_.push_back(t_skillup);
	}

	dbfile = game::scheme()->get_dbc("t_role_dress.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role_dress t_dress;
		t_dress.id = dbfile->Get(i, 0)->iValue;
		t_dress.role = dbfile->Get(i, 3)->iValue;
		t_dress.glevel = dbfile->Get(i, 8)->iValue;
		t_role_dress_[t_dress.id] = t_dress;
	}

	dbfile = game::scheme()->get_dbc("t_xinqing.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_xinqing t_xinqing;
		t_xinqing.type = dbfile->Get(i, 0)->iValue;
		t_xinqing.dmg = dbfile->Get(i, 1)->iValue;

		t_xinqing_[t_xinqing.type] = t_xinqing;
	}

	dbfile = game::scheme()->get_dbc("t_gongzhen.txt");
	if (!dbfile)
	{
		return -1;
	}
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role_gongzheng gongzheng;
		gongzheng.index = dbfile->Get(i, 0)->iValue;
		gongzheng.condition = dbfile->Get(i, 1)->iValue;
		gongzheng.type = dbfile->Get(i, 2)->iValue;
		

		for (int j = 0; j < 6; ++j)
		{
			std::pair<int, float> att;
			att.first = dbfile->Get(i, 3 + j * 2)->iValue;
			att.second = dbfile->Get(i, 4 + j * 2)->dValue;
			if (att.first != 0)
			{
				gongzheng.attrs.push_back(att);
			}
		}

		t_role_gongzheng_[gongzheng.type].push_back(gongzheng);
	}	

	dbfile = game::scheme()->get_dbc("t_role_shengpin.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role_shengpin t_role_shengpin;
		t_role_shengpin.pinzhi = dbfile->Get(i, 0)->iValue;
		t_role_shengpin.next_pinzhi = dbfile->Get(i, 3)->iValue;
		t_role_shengpin.level = dbfile->Get(i, 4)->iValue;
		for (int j = 0; j < 5; ++j)
		{
			double cs = dbfile->Get(i, j + 5)->dValue;
			t_role_shengpin.cs.push_back(cs);
		}
		for (int j = 0; j < 5; ++j)
		{
			double cz = dbfile->Get(i, j + 10)->dValue;
			t_role_shengpin.cz.push_back(cz);
		}
		t_role_shengpin.color = dbfile->Get(i, 15)->iValue;
		t_role_shengpin.zdjnjc = dbfile->Get(i, 16)->iValue;
		t_role_shengpin.bdjnjc = dbfile->Get(i, 17)->iValue;
		t_role_shengpin.shengpinshi = dbfile->Get(i, 18)->iValue;
		t_role_shengpin.zhanhun = dbfile->Get(i, 19)->iValue;
		t_role_shengpin.hongsehuobanzhili = dbfile->Get(i, 20)->iValue;
		t_role_shengpin.suipian = dbfile->Get(i, 21)->iValue;
		t_role_shengpin.gold = dbfile->Get(i, 22)->iValue;

		t_role_shengpin_[t_role_shengpin.pinzhi] = t_role_shengpin;
	}

	if (parse_huiyi() == -1)
	{
		return -1;
	}

	if (parse_duixing() == -1)
	{
		return -1;
	}

	if (parse_guanghuan() == -1)
	{
		return -1;
	}

	if (parse_bskill() == -1)
	{
		return -1;
	}

	if (parse_pet() == -1)
	{
		return -1;
	}

	return 0;
}

int RoleConfig::parse_huiyi()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_huiyi_lunpan.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huiyin_chou chou;
		chou.id = dbfile->Get(i, 0)->iValue;
		chou.type = dbfile->Get(i, 2)->iValue;
		chou.weight = dbfile->Get(i, 3)->iValue;
		chou.point = dbfile->Get(i, 4)->iValue;

		t_huiyi_chou_.push_back(chou);
	}

	dbfile = game::scheme()->get_dbc("t_huiyi_sub.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huiyi_jihuo jihuo;
		jihuo.id = dbfile->Get(i, 0)->iValue;
		
		for (int j = 0; j < 5; ++j)
		{
			if (dbfile->Get(i, 6 + j * 1)->iValue != 0)
			{
				jihuo.huiyi.push_back(dbfile->Get(i, 6 + j * 1)->iValue);
			}
		}

		for (int j = 0; j < 4; ++j)
		{
			s_t_huiyi_start_attr attrs;
			attrs.attr = dbfile->Get(i, 12 + j * 3)->iValue;
			attrs.val1 = dbfile->Get(i, 13 + j * 3)->dValue;
			attrs.val2 = dbfile->Get(i, 14 + j * 3)->dValue;
			if (attrs.attr != 0)
			{
				jihuo.attrs.push_back(attrs);
			}
		}

		t_huiji_jihuos_[jihuo.id] = jihuo;
	}

	dbfile = game::scheme()->get_dbc("t_huiyi_mingyun.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huiyi_fate fate;
		fate.id = dbfile->Get(i, 0)->iValue;
		fate.weight = dbfile->Get(i, 2)->iValue;
		fate.point = dbfile->Get(i, 3)->iValue;

		t_huiyi_fates_[fate.id] = fate;
	}

	dbfile = game::scheme()->get_dbc("t_huiyi_chengjiu.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huiyi_chengjiu chengjiu;
		chengjiu.id = dbfile->Get(i, 0)->iValue;
		chengjiu.val = dbfile->Get(i, 1)->iValue;
		chengjiu.attr = dbfile->Get(i, 2)->iValue;
		chengjiu.value = dbfile->Get(i, 3)->iValue;

		t_huiyi_chengjius_.push_back(chengjiu);
	}

	return 0;
}

int RoleConfig::parse_duixing()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_duixing.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_duixing duixing;
		duixing.id = dbfile->Get(i, 0)->iValue;
		duixing.type = dbfile->Get(i, 3)->iValue;
		duixing.level = dbfile->Get(i, 4)->iValue;
		for (int j = 0; j < 6; ++j)
		{
			int dx = dbfile->Get(i, 5 + j)->iValue;
			duixing.duiweis.push_back(dx);
		}
		for (int j = 0; j < 3; ++j)
		{
			std::vector<s_t_duixing_add> adds;
			for (int k = 0; k < 2; ++k)
			{
				s_t_duixing_add t_duixing_add;
				t_duixing_add.attr = dbfile->Get(i, 11 + k * 3 + j * 6)->iValue;
				t_duixing_add.value = dbfile->Get(i, 12 + k * 3 + j * 6)->dValue;
				t_duixing_add.add = dbfile->Get(i, 13 + k * 3 + j * 6)->dValue;
				adds.push_back(t_duixing_add);
			}
			duixing.sx.push_back(adds);
		}

		t_duixings_[duixing.id] = duixing;
	}

	dbfile = game::scheme()->get_dbc("t_duixing_skill.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_duixing_skill skill;
		skill.id = dbfile->Get(i, 0)->iValue;
		skill.level = dbfile->Get(i, 5)->iValue;
		
		for (int j = 0; j < 4; ++j)
		{
			if (dbfile->Get(i, 6 + j * 2)->iValue)
			{
				std::pair<int, int> att;
				att.first = dbfile->Get(i, 6 + j * 2)->iValue;
				att.second = dbfile->Get(i, 7 + j * 2)->iValue;
				skill.attrs.push_back(att);
			}
		}

		t_duixing_skills_.push_back(skill);
	}

	dbfile = game::scheme()->get_dbc("t_duixing_up.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_duixing_up up;
		up.level = dbfile->Get(i, 0)->iValue;
		up.player_level = dbfile->Get(i, 1)->iValue;
		up.zhuangzhi = dbfile->Get(i, 2)->iValue;
		up.gold = dbfile->Get(i, 3)->iValue;
		t_duixing_ups_[up.level] = up;
	}

	dbfile = game::scheme()->get_dbc("t_role_gaizao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role_gaizao gaizao;
		gaizao.id = dbfile->Get(i, 0)->iValue;
		gaizao.type = dbfile->Get(i, 2)->iValue;
		gaizao.jewel = dbfile->Get(i, 3)->iValue;
		gaizao.hun = dbfile->Get(i, 4)->iValue;
		t_role_gaizao_[gaizao.id] = gaizao;
	}

	return 0;
}

int RoleConfig::parse_guanghuan()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_guanghuan.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guanghuan gh;
		gh.id = dbfile->Get(i, 0)->iValue;
		gh.color = dbfile->Get(i, 3)->iValue;
		gh.att1 = dbfile->Get(i, 6)->iValue;
		gh.val1 = dbfile->Get(i, 7)->iValue;
		gh.att2 = dbfile->Get(i, 8)->iValue;
		gh.val2 = dbfile->Get(i, 9)->iValue;

		t_guanghuans_[gh.id] = gh;
	}

	dbfile = game::scheme()->get_dbc("t_guanghuan_target.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guanghuan_target gt;
		gt.id = dbfile->Get(i, 0)->iValue;
		
		for (int j = 0; j < 3; ++j)
		{
			if (dbfile->Get(i, 3 + j * 1)->iValue > 0)
			{
				gt.guanghuans.push_back(dbfile->Get(i, 3 + j * 1)->iValue);
			}
		}

		for (int j = 0; j < 4; ++j)
		{
			if (dbfile->Get(i, 6 + j * 2)->iValue > 0)
			{
				std::pair<int, double> att;
				att.first = dbfile->Get(i, 6 + j * 2)->iValue;
				att.second = dbfile->Get(i, 7 + j * 2)->iValue;
				gt.attrs.push_back(att);
			}
		}

		t_guanghuan_targets_[gt.id] = gt;
	}

	dbfile = game::scheme()->get_dbc("t_guanghuan_enhance.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guanghuan_level gl;
		gl.level = dbfile->Get(i, 0)->iValue;

		for (int j = 0; j < 4; ++j)
		{
			std::pair<int, int> cailiao;
			cailiao.first = dbfile->Get(i, 1 + j * 2)->iValue;
			cailiao.second = dbfile->Get(i, 2 + j * 2)->iValue;
			gl.cailiaos_.push_back(cailiao);
		}

		t_guanghuan_levels_.push_back(gl);
	}

	dbfile = game::scheme()->get_dbc("t_guanghuan_skill.txt");
	if (!dbfile)
	{
		return -1;
	}

	int id = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guanghuan_skill skill;
		id = dbfile->Get(i, 3)->iValue;
		skill.level = dbfile->Get(i, 4)->iValue;
		skill.type = dbfile->Get(i, 5)->iValue;
		skill.def1 = dbfile->Get(i, 6)->iValue;
		skill.def2 = dbfile->Get(i, 7)->iValue;

		t_guanghuan_skill_[id].push_back(skill);
	}
	return 0;
}

int RoleConfig::parse_bskill()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_role_skillunlock.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role_bskill sk;
		for (int j = 0; j < 5; ++j)
		{
			if (dbfile->Get(i, 5 + j * 4)->iValue)
			{
				s_t_reward rd;
				rd.type = dbfile->Get(i, 5 + j * 4)->iValue;
				rd.value1 = dbfile->Get(i, 6 + j * 4)->iValue;
				rd.value2 = dbfile->Get(i, 7 + j * 4)->iValue;
				rd.value3 = dbfile->Get(i, 8 + j * 4)->iValue;
				sk.defs.push_back(rd);
			}
		}

		t_role_bskill_[dbfile->Get(i, 3)->iValue].push_back(sk);
	}

	dbfile = game::scheme()->get_dbc("t_role_spskillup.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_role_bskill_up bu;
		bu.jls = dbfile->Get(i, 1)->iValue;
		bu.gold = dbfile->Get(i, 2)->iValue;
		t_role_bskill_up_.push_back(bu);
	}

	return 0;
}

int RoleConfig::parse_pet()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_chongwu.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_pet pet;
		pet.class_id = dbfile->Get(i, 7)->iValue;
		pet.color = dbfile->Get(i, 8)->iValue;
		pet.cshp = dbfile->Get(i, 9)->iValue;
		pet.cshpcz = dbfile->Get(i, 10)->dValue;
		pet.sxhpcz = dbfile->Get(i, 11)->dValue;
		pet.csgj = dbfile->Get(i, 12)->iValue;
		pet.csgjcz = dbfile->Get(i, 13)->dValue;
		pet.sxgjcz = dbfile->Get(i, 14)->dValue;
		pet.cswf = dbfile->Get(i, 15)->iValue;
		pet.cswfcz = dbfile->Get(i, 16)->dValue;
		pet.sxwfcz = dbfile->Get(i, 17)->dValue;
		pet.csmf = dbfile->Get(i, 18)->iValue;
		pet.csmfcz = dbfile->Get(i, 19)->dValue;
		pet.sxmfcz = dbfile->Get(i, 20)->dValue;
		pet.ptjn = dbfile->Get(i, 21)->iValue;
		pet.zdjn = dbfile->Get(i, 22)->iValue;
		pet.jjhp = dbfile->Get(i, 23)->dValue;
		pet.jjgj = dbfile->Get(i, 24)->dValue;
		pet.jjwf = dbfile->Get(i, 25)->dValue;
		pet.jjmf = dbfile->Get(i, 26)->dValue;
		pet.jjattr1 = dbfile->Get(i, 27)->iValue;
		pet.jjval1 = dbfile->Get(i, 28)->iValue;
		pet.jjattr2 = dbfile->Get(i, 29)->iValue;
		pet.jjval2 = dbfile->Get(i, 30)->iValue;
		pet.sxhpadd = dbfile->Get(i, 31)->iValue;
		pet.sxgjadd = dbfile->Get(i, 32)->iValue;
		pet.sxwfadd = dbfile->Get(i, 33)->iValue;
		pet.sxmfadd = dbfile->Get(i, 34)->iValue;
		pet.sxjnadd = dbfile->Get(i, 35)->dValue;
		pet.shouhun = dbfile->Get(i, 36)->iValue;
		pet.guard_jiacheng = dbfile->Get(i, 37)->dValue;
		pet.on_jiacheng = dbfile->Get(i, 38)->dValue;

		t_pets_[dbfile->Get(i, 0)->iValue] = pet;
	}

	dbfile = game::scheme()->get_dbc("t_chongwu_jinjieitem.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_pet_jinjie_item ji;
		ji.att1 = dbfile->Get(i, 5)->iValue;
		ji.val1 = dbfile->Get(i, 6)->iValue;
		ji.att2 = dbfile->Get(i, 7)->iValue;
		ji.val2 = dbfile->Get(i, 8)->iValue;
		ji.att3 = dbfile->Get(i, 9)->iValue;
		ji.val3 = dbfile->Get(i, 10)->iValue;

		t_pet_jinjie_items_[dbfile->Get(i, 0)->iValue] = ji;
	}

	dbfile = game::scheme()->get_dbc("t_item.txt");
	if (!dbfile)
	{
		return -1;
	}

	std::map<int, s_t_pet>::iterator pet_iter;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		if (dbfile->Get(i, 4)->iValue == 10001)
		{
			pet_iter = t_pets_.find(dbfile->Get(i, 9)->iValue);
			if (pet_iter != t_pets_.end())
			{
				pet_iter->second.suipian_id = dbfile->Get(i, 0)->iValue;
			}
		}
	}

	dbfile = game::scheme()->get_dbc("t_chongwu_jinjie.txt");
	if (!dbfile)
	{
		return -1;
	}

	int quan = 0;
	int extra1 = 0;
	int extra2 = 0;
	std::map<int, int> attrs;
	std::map<int, s_t_pet_jinjie_item>::const_iterator jinjie_item_iter;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_pet_jinjie pj;
		pj.level = dbfile->Get(i, 4)->iValue;
		for (int j = 0; j < 4; ++j)
		{
			if (dbfile->Get(i, 5 + j * 1)->iValue != 0)
			{
				pj.cailiao.push_back(dbfile->Get(i, 5 + j * 1)->iValue);
				jinjie_item_iter = t_pet_jinjie_items_.find(dbfile->Get(i, 5 + j * 1)->iValue);
				if (jinjie_item_iter != t_pet_jinjie_items_.end())
				{
					const s_t_pet_jinjie_item& ji = jinjie_item_iter->second;
					attrs[ji.att1] += ji.val1;
					attrs[ji.att2] += ji.val2;
					attrs[ji.att3] += ji.val3;
				}
			}
		}
		pj.gold = dbfile->Get(i, 9)->iValue;
		quan += dbfile->Get(i, 10)->iValue;
		pj.quan = quan;
		if (dbfile->Get(i, 11)->iValue == 1)
		{
			extra1 += 1;
		}
		else if (dbfile->Get(i, 11)->iValue == 2)
		{
			extra2 += 1;
		}
		pj.extra1 = extra1;
		pj.extra2 = extra2;
		pj.attrs = attrs;
		t_pet_jinjies_.push_back(pj);
	}

	dbfile = game::scheme()->get_dbc("t_chongwu_shengxing.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_pet_shengxing sx;
		sx.level = dbfile->Get(i, 1)->iValue;
		for (int j = 0; j < 4; ++j)
		{
			s_t_pet_shengxin_item item;
			item.suipian = dbfile->Get(i, 2 + j * 1)->iValue;
			item.shitou = dbfile->Get(i, 6 + j * 1)->iValue;
			item.gold = dbfile->Get(i, 10 + j * 1)->iValue;
			sx.shengxing.push_back(item);
		}
		t_pet_shengxings_.push_back(sx);
	}

	dbfile = game::scheme()->get_dbc("t_chongwu_target.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_pet_chengjiu cj;
		cj.pet1 = dbfile->Get(i, 3)->iValue;
		cj.pet2 = dbfile->Get(i, 4)->iValue;

		for (int j = 0; j < 4; ++j)
		{
			if (dbfile->Get(i, 5 + j * 2)->iValue)
			{
				cj.attrs.push_back(std::make_pair(dbfile->Get(i, 5 + j * 2)->iValue,
					dbfile->Get(i, 6 + j * 2)->iValue));
			}
		}
		t_pet_chengjius_.push_back(cj);
	}

	return 0;
}

s_t_chouka * RoleConfig::get_chouka(dhc::player_t* player, int type)
{
	int sum = 0;
	for (int i = 0; i < t_choukas_.size(); ++i)
	{
		if (player->ck_num(type) >= t_choukas_[i].bcx)
		{
			sum += t_choukas_[i].rates[type];
		}	
	}
	if (sum == 0)
	{
		return 0;
	}
	int rands = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (int i = 0; i < t_choukas_.size(); ++i)
	{
		if (player->ck_num(type) >= t_choukas_[i].bcx)
		{
			gl += t_choukas_[i].rates[type];
			if (gl > rands)
			{
				return &t_choukas_[i];
			}
		}
		
	}
	return 0;
}

s_t_role * RoleConfig::get_role(uint32_t id)
{
	std::map<uint32_t, s_t_role>::iterator it = t_roles_.find(id);
	if (it == t_roles_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_role_jiban * RoleConfig::get_role_jiban(int id)
{
	std::map<int, s_t_role_jiban>::iterator it = t_role_jiban_.find(id);
	if (it == t_role_jiban_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_role_jibanex * RoleConfig::get_role_jibanex(int id)
{
	std::map<int, s_t_role_jibanex>::iterator it = t_role_jibanex_.find(id);
	if (it == t_role_jibanex_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_skill * RoleConfig::get_skill(int id)
{
	std::map<int, s_t_skill>::iterator it = t_skill_.find(id);
	if (it == t_skill_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

uint32_t RoleConfig::get_random_role_id(int color, const std::vector<uint32_t> &rdroles)
{
	std::vector<uint32_t> roles;
	for (std::map<uint32_t, s_t_role>::iterator it = t_roles_.begin(); it != t_roles_.end(); ++it)
	{
		s_t_role &t_role = (*it).second;
		bool flag = true;
		for (int i = 0; i < rdroles.size(); ++i)
		{
			if (rdroles[i] == t_role.id)
			{
				flag = false;
				break;
			}
		}
		if (t_role.font_color == color && flag && !t_role.ycang)
		{
			roles.push_back(t_role.id);
		}
	}
	if (roles.size() == 0)
	{
		return 0;
	}
	return roles[Utils::get_int32(0, roles.size() - 1)];
}

s_t_tupo * RoleConfig::get_tupo(int id)
{
	std::map<int, s_t_tupo>::iterator it = t_tupo_.find(id);
	if (it == t_tupo_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}


s_t_jinjie * RoleConfig::get_jinjie(int id)
{
	std::map<int, s_t_jinjie>::iterator it = t_jinjie_.find(id);
	if (it == t_jinjie_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}


double RoleConfig::get_jinjie_point(int id)
{
	std::map<int, double>::iterator it = t_jinjie_point_.find(id);
	if (it == t_jinjie_point_.end())
	{
		return 0;
	}
	else
	{
		return (*it).second;
	}
}

int RoleConfig::get_jinjie_shuxing(int level)
{
	std::map<int, int>::iterator it = t_jinjie_shuxing_.find(level);
	if (it == t_jinjie_shuxing_.end())
	{
		return 0;
	}
	else
	{
		return (*it).second;
	}
}

s_t_skillup * RoleConfig::get_skillup(int level)
{
	if (level < 1 || level > t_skillup_.size())
	{
		return 0;
	}

	return &t_skillup_[level - 1];
}

const s_t_role_dress * RoleConfig::get_role_dress(int id) const
{
	std::map<int, s_t_role_dress>::const_iterator it = t_role_dress_.find(id);
	if (it == t_role_dress_.end())
	{
		return 0;
	}

	return &(it->second);
}

void RoleConfig::refresh_hhb(dhc::player_t *player, bool reset)
{
	int color_sum = 0;
	const s_t_role *t_role = 0;

	/*
	for (int i = 0; i < player->hbb_class_ids_size(); ++i)
	{
		t_role = get_role(player->hbb_class_ids(i));
		if (t_role)
		{
			color_sum += t_role->font_color;
		}
	}
	*/
	player->clear_hbb_class_ids();

	int base = color_sum / 3;
	int extra = color_sum % 3;
	if (base == 0 || reset)
	{
		base = 3;
		extra = 0;
	}

	std::set<int> ids;
	for (int i = 0; i < 3; ++i)
	{
		const s_t_hbb *t_hbb = get_hbb(base, extra, ids);

		if (!t_hbb)
		{
			continue;
		}
		ids.insert(t_hbb->id);
		if (extra >= 1)
		{
			extra -= 1;
		}
		player->add_hbb_class_ids(t_hbb->id);
	}
	
	/*int color_new = 0;
	player->clear_hbb_class_ids();
	std::vector<int> nums;
	for (int i = 0; i < t_hbbs_.size(); ++i)
	{
	nums.push_back(i);
	}
	for (int i = 0; i < 3;)
	{
	int sum = 0;
	for (int j = i; j < nums.size(); ++j)
	{
	sum += t_hbbs_[nums[j]].rate;
	}
	if (sum == 0)
	{
	++i;
	continue;
	}
	int r = Utils::get_int32(0, sum - 1);
	int gl = 0;
	int p = 0;
	for (int j = i; j < nums.size(); ++j)
	{
	gl += t_hbbs_[nums[j]].rate;
	if (gl > r)
	{
	p = j;
	break;
	}
	}
	int a = nums[i];
	nums[i] = nums[p];
	nums[p] = a;
	player->add_hbb_class_ids(t_hbbs_[nums[i]].id);
	}*/
}

const s_t_hbb *RoleConfig::get_hbb(int color, int add, const std::set<int>& ids) const
{
	if (add > 0)
	{
		color += 1;
	}
	int sum = 0;
	for (int i = 0; i < t_hbbs_.size(); ++i)
	{
		if (t_hbbs_[i].color >= color && ids.find(t_hbbs_[i].id) == ids.end())
		{
			sum += t_hbbs_[i].rate;
		}
	}

	if (sum == 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (int i = 0; i < t_hbbs_.size(); ++i)
	{
		if (t_hbbs_[i].color >= color && ids.find(t_hbbs_[i].id) == ids.end())
		{
			gl += t_hbbs_[i].rate;
			if (gl > rate)
			{
				return &(t_hbbs_[i]);
			}
		}
	}

	return 0;
}

const s_t_chongsheng *RoleConfig::get_role_chongsheng(int type) const
{
	if (type > 5)
	{
		type = 5;
	}
	std::map<int, s_t_chongsheng>::const_iterator it = t_chongsheng_.find(type);
	if (it == t_chongsheng_.end())
	{
		return 0;
	}
	return &(it->second);
}

s_t_xinqing * RoleConfig::get_xinqing(int type)
{
	std::map<int, s_t_xinqing>::iterator it = t_xinqing_.find(type);
	if (it == t_xinqing_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_role_gongzheng* RoleConfig::get_role_gongzheng(int type, int level) const
{
	std::map<int, std::vector<s_t_role_gongzheng> >::const_iterator it = t_role_gongzheng_.find(type);
	if (it == t_role_gongzheng_.end())
	{
		return 0;
	}

	const s_t_role_gongzheng *gongzheng = 0;
	const std::vector<s_t_role_gongzheng>& gongzhengs = it->second;
	for (std::vector<s_t_role_gongzheng>::size_type i = 0;
		i < gongzhengs.size();
		++i)
	{
		if (level >= gongzhengs[i].condition)
		{
			if (gongzheng)
			{
				if (gongzheng->condition < gongzhengs[i].condition)
				{
					gongzheng = &(gongzhengs[i]);
				}
			}
			else
			{
				gongzheng = &(gongzhengs[i]);
			}
		}
	}

	return gongzheng;
}

s_t_role_shengpin* RoleConfig::get_role_shengpin(int pinzhi)
{
	std::map<int, s_t_role_shengpin>::iterator it = t_role_shengpin_.find(pinzhi);
	if (it == t_role_shengpin_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_huiyin_chou* RoleConfig::get_huiyi_chou(dhc::player_t* player, int type) const
{
	int sum = 0;
	int rate = 0;
	int gl = 0;
	if (type == 2)
	{
		for (std::vector<s_t_huiyin_chou>::size_type i = 0;
			i < t_huiyi_chou_.size();
			++i)
		{
			if (t_huiyi_chou_[i].type == type)
			{
				sum += t_huiyi_chou_[i].weight;
			}
		}

		if (sum == 0)
		{
			return 0;
		}

		rate = Utils::get_int32(0, sum - 1);
		for (std::vector<s_t_huiyin_chou>::size_type i = 0;
			i < t_huiyi_chou_.size();
			++i)
		{
			if (t_huiyi_chou_[i].type == type)
			{
				gl += t_huiyi_chou_[i].weight;
				if (gl > rate)
				{
					return &t_huiyi_chou_[i];
				}
			}
		}
	}
	else
	{
		for (std::vector<s_t_huiyin_chou>::size_type i = 0;
			i < t_huiyi_chou_.size();
			++i)
		{
			sum += t_huiyi_chou_[i].weight;
		}

		if (sum == 0)
		{
			return 0;
		}

		rate = Utils::get_int32(0, sum - 1);
		for (std::vector<s_t_huiyin_chou>::size_type i = 0;
			i < t_huiyi_chou_.size();
			++i)
		{
			gl += t_huiyi_chou_[i].weight;
			if (gl > rate)
			{
				return &t_huiyi_chou_[i];
			}
		}
	}

	return 0;
}

const s_t_huiyi_jihuo* RoleConfig::get_huiyi_jihuo(int id) const
{
	std::map<int, s_t_huiyi_jihuo>::const_iterator it = t_huiji_jihuos_.find(id);
	if (it == t_huiji_jihuos_.end())
	{
		return 0;
	}
	return &(it->second);
}

//const s_t_huiyi_jihuo_starts* RoleConfig::get_huiyi_star(int id) const
//{
//	std::map<int, s_t_huiyi_jihuo_starts>::const_iterator it = t_huiyi_jihuo_starts.find(id);
//	if (it == t_huiyi_jihuo_starts.end())
//	{
//		return 0;
//	}
//	return &(it->second);


const s_t_huiyi_fate* RoleConfig::get_huiyi_fate(int id) const
{
	std::map<int, s_t_huiyi_fate>::const_iterator it = t_huiyi_fates_.find(id);
	if (it == t_huiyi_fates_.end())
	{
		return 0;
	}
	return &(it->second);
}

int RoleConfig::get_huiyi_fate_random(const std::set<int>& lasts) const
{
	int sum = 0;
	for (std::map<int, s_t_huiyi_fate>::const_iterator it = t_huiyi_fates_.begin();
		it != t_huiyi_fates_.end();
		++it)
	{
		if (lasts.find(it->second.id) == lasts.end())
		{
			sum += it->second.weight;
		}
	}
	if (sum == 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);;
	int gl = 0;
	for (std::map<int, s_t_huiyi_fate>::const_iterator it = t_huiyi_fates_.begin();
		it != t_huiyi_fates_.end();
		++it)
	{
		if (lasts.find(it->second.id) == lasts.end())
		{
			gl += it->second.weight;
			if (gl > rate)
			{
				return it->second.id;
			}
		}
	}
	return 0;
}

void RoleConfig::get_huiyi_chengjiu_attr(dhc::player_t* player, std::map<int, double> &attrs) const
{
	for (std::vector<s_t_huiyi_chengjiu>::size_type i = 0;
		i < t_huiyi_chengjius_.size();
		++i)
	{
		const s_t_huiyi_chengjiu& chengjiu = t_huiyi_chengjius_[i];
		if (player->huiyi_shoujidu() >= chengjiu.val)
		{
			attrs[chengjiu.attr] += chengjiu.value;
		}
		else
		{
			break;
		}
	}
}

s_t_duixing * RoleConfig::get_duixing(int id)
{
	std::map<int, s_t_duixing>::iterator it = t_duixings_.find(id);
	if (it == t_duixings_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_duixing_up* RoleConfig::get_duixing_up(int level) const
{
	std::map<int, s_t_duixing_up>::const_iterator it = t_duixing_ups_.find(level);
	if (it == t_duixing_ups_.end())
	{
		return 0;
	}
	return &(it->second);
}

void RoleConfig::get_duixing_skill_attr(dhc::player_t* player, std::map<int, double> &attrs) const
{
	for (std::vector<s_t_duixing_skill>::size_type i = 0;
		i < t_duixing_skills_.size();
		++i)
	{
		if (player->duixing_level() >= t_duixing_skills_[i].level)
		{
			for (std::vector<std::pair<int, int> >::size_type j = 0; j < t_duixing_skills_[i].attrs.size(); ++j)
			{
				const std::pair<int, int>& duixing_attr = t_duixing_skills_[i].attrs[j];
				attrs[duixing_attr.first] += duixing_attr.second;
			}
		}
		else
		{
			break;
		}
	}
}

const s_t_guanghuan* RoleConfig::get_guanghuan(int id) const
{
	std::map<int, s_t_guanghuan>::const_iterator it = t_guanghuans_.find(id);
	if (it == t_guanghuans_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guanghuan_level* RoleConfig::get_guanghuan_level(int level) const
{
	level = level - 1;

	if (level < 0 || level >= t_guanghuan_levels_.size())
	{
		return 0;
	}
	return &t_guanghuan_levels_[level];
}

const s_t_guanghuan_target* RoleConfig::get_guanghuan_target(int id) const
{
	std::map<int, s_t_guanghuan_target>::const_iterator it = t_guanghuan_targets_.find(id);
	if (it == t_guanghuan_targets_.end())
	{
		return 0;
	}
	return &(it->second);
}

void RoleConfig::get_guanghuan_skill_attr(dhc::player_t *player, std::map<int, double> &attrs) const
{
	std::map<int, std::vector<s_t_guanghuan_skill> >::const_iterator it;
	for (int i = 0; i < player->guanghuan_size(); ++i)
	{
		if (player->guanghuan(i) == player->guanghuan_id())
		{
			it = t_guanghuan_skill_.find(player->guanghuan(i));
			if (it != t_guanghuan_skill_.end())
			{
				const std::vector<s_t_guanghuan_skill>& skills = it->second;
				for (int j = 0; j < skills.size(); ++j)
				{
					if (player->guanghuan_level(i) >= skills[j].level)
					{
						if (skills[j].type == 1)
						{
							attrs[skills[j].def1] += skills[j].def2;
						}
					}
					else
					{
						break;
					}
				}
			}
		}
	}
}

void RoleConfig::get_guanghuan_skill_id(dhc::player_t *player, std::map<int, int> &ids) const
{
	std::map<int, std::vector<s_t_guanghuan_skill> >::const_iterator it;
	for (int i = 0; i < player->guanghuan_size(); ++i)
	{
		it = t_guanghuan_skill_.find(player->guanghuan(i));
		if (it != t_guanghuan_skill_.end())
		{
			const std::vector<s_t_guanghuan_skill>& skills = it->second;
			for (int j = 0; j < skills.size(); ++j)
			{
				if (player->guanghuan_level(i) >= skills[j].level)
				{
					if (skills[j].type > 1)
					{
						ids[skills[j].type] = skills[j].def1;
					}
				}
				else
				{
					break;
				}
			}
		}
	}
}

void RoleConfig::get_guanghuan_target_attr(dhc::player_t *player, std::map<int, double> &attrs) const
{
	for (std::map<int, s_t_guanghuan_target>::const_iterator it = t_guanghuan_targets_.begin();
		it != t_guanghuan_targets_.end();
		++it)
	{
		const s_t_guanghuan_target& tg = it->second;
		bool com = true;
		for (int i = 0; i < tg.guanghuans.size(); ++i)
		{
			bool has = false;
			for (int j = 0; j < player->guanghuan_size(); ++j)
			{
				if (player->guanghuan(j) == tg.guanghuans[i])
				{
					has = true;
					break;
				}
			}
			if (!has)
			{
				com = false;
				break;
			}
		}
		if (com)
		{
			for (int i = 0; i < tg.attrs.size(); ++i)
			{
				attrs[tg.attrs[i].first] += tg.attrs[i].second;
			}
		}
	}
}

const s_t_role_bskill *RoleConfig::get_role_bskill(int role_id, int level) const
{
	std::map<int, std::vector<s_t_role_bskill> >::const_iterator it = t_role_bskill_.find(role_id);
	if (it == t_role_bskill_.end())
	{
		return 0;
	}

	level = level - 1;
	const std::vector<s_t_role_bskill>& tbs = it->second;
	if (level < 0 || level >= tbs.size())
	{
		return 0;
	}
	return &(tbs[level]);
}

const s_t_role_bskill_up* RoleConfig::get_role_bskill_up(int level) const
{
	level = level - 1;
	if (level < 0 || level >= t_role_bskill_up_.size())
	{
		return 0;
	}
	return &(t_role_bskill_up_[level]);
}

const s_t_pet* RoleConfig::get_pet(int id) const
{
	std::map<int, s_t_pet>::const_iterator it = t_pets_.find(id);
	if (it == t_pets_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_pet_jinjie* RoleConfig::get_pet_jinjie(int level) const
{
	if (level < 0 || level >= t_pet_jinjies_.size())
	{
		return 0;
	}
	return &(t_pet_jinjies_[level]);
}

const s_t_pet_jinjie_item* RoleConfig::get_pet_jinjie_item(int id) const
{
	std::map<int, s_t_pet_jinjie_item>::const_iterator it = t_pet_jinjie_items_.find(id);
	if (it == t_pet_jinjie_items_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_pet_shengxing* RoleConfig::get_pet_shengxing(int level) const
{
	level = level - 1;
	if (level < 0 || level >= t_pet_shengxings_.size())
	{
		return 0;
	}
	return &(t_pet_shengxings_[level]);
}

const s_t_role_gaizao* RoleConfig::get_role_gaizao(int id) const
{
	std::map<int, s_t_role_gaizao>::const_iterator it = t_role_gaizao_.find(id);
	if (it == t_role_gaizao_.end())
	{
		return 0;
	}
	return &(it->second);
}
