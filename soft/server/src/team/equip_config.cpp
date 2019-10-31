#include "equip_config.h"
#include "dbc.h"
#include "equip_operation.h"
#include "utils.h"

int EquipConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_equip.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_equip t_equip;
		t_equip.id = dbfile->Get(i, 0)->iValue;
		t_equip.font_color = dbfile->Get(i, 3)->iValue;
		t_equip.type = dbfile->Get(i, 4)->iValue;
		t_equip.icon = dbfile->Get(i, 5)->pString;
		t_equip.slot_num = dbfile->Get(i, 6)->iValue;
		t_equip.sell = dbfile->Get(i, 7)->iValue;
		t_equip.sell_item_num = dbfile->Get(i, 8)->iValue;
		t_equip.eattr.attr = dbfile->Get(i, 9)->iValue;
		t_equip.eattr.value = dbfile->Get(i, 10)->iValue;
		for (int j = 0; j < 2; ++j)
		{
			s_t_equip_attr ea;
			ea.attr = dbfile->Get(i, 11 + j * 2)->iValue;
			ea.value = dbfile->Get(i, 12 + j * 2)->iValue;
			if (ea.attr)
			{
				t_equip.ejlattr.push_back(ea);
			}
		}
		for (int j = 0; j < 5; ++j)
		{
			s_t_equip_random_attr eea;
			eea.attr = dbfile->Get(i, 15 + j * 3)->iValue;
			eea.value1 = dbfile->Get(i, 16 + j * 3)->dValue;
			eea.value2 = dbfile->Get(i, 17 + j * 3)->dValue;
			if (eea.attr)
			{
				t_equip.eeattr.push_back(eea);
			}
		}

		t_equips_[t_equip.id] = t_equip;
	}

	dbfile = game::scheme()->get_dbc("t_equip_enhance.txt");
	if (!dbfile)
	{
		return -1;
	}

	t_enhances_.resize(5);
	t_enhance_totals_.resize(5);
	int total[5] = { 0 };
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		int level = dbfile->Get(i, 0)->iValue;
		for (int j = 0; j < 5; ++j)
		{
			int gold = dbfile->Get(i, 1 + j)->iValue;
			total[j] += gold;

			t_enhances_[j].push_back(gold);
			t_enhance_totals_[j].push_back(total[j]);
		}
	}
	
	dbfile = game::scheme()->get_dbc("t_equip_gaizao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_gaizao t_gaizao;
		t_gaizao.font_color = dbfile->Get(i, 0)->iValue;
		t_gaizao.item_id = dbfile->Get(i, 1)->iValue;
		t_gaizao.max_num = dbfile->Get(i, 7)->iValue;

		for (int j = 0; j < 5; ++j)
		{
			t_gaizao.gaizaoshi.push_back(dbfile->Get(i, 2 + j * 1)->iValue);
		}

		for (int j = 0; j < 5; ++j)
		{
			t_gaizao.gaizao_rate.push_back(dbfile->Get(i, 8 + j * 1)->iValue);
		}

		t_gaizaos_.push_back(t_gaizao);
	}

	dbfile = game::scheme()->get_dbc("t_equip_sx.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_equip_sx t_equip_sx;
		t_equip_sx.level = dbfile->Get(i, 0)->iValue;
		t_equip_sx.color = dbfile->Get(i, 1)->iValue;
		t_equip_sx.mw_point = dbfile->Get(i, 2)->iValue;
		t_equip_sx.enhance_rate = dbfile->Get(i, 3)->dValue;
		t_equip_sx.suipian = dbfile->Get(i, 4)->iValue;
		t_equip_sx.gold = dbfile->Get(i, 5)->iValue;

		t_equip_sx_[t_equip_sx.color][t_equip_sx.level] = t_equip_sx;
	}

	dbfile = game::scheme()->get_dbc("t_dress.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_dress t_dress;
		t_dress.id = dbfile->Get(i, 0)->iValue;
		t_dress.type = dbfile->Get(i, 3)->iValue;
		t_dress.color = dbfile->Get(i, 4)->iValue;
		t_dress.attr1 = dbfile->Get(i, 7)->iValue;
		t_dress.value1 = dbfile->Get(i, 8)->iValue;
		t_dress.attr2 = dbfile->Get(i, 9)->iValue;
		t_dress.value2 = dbfile->Get(i, 10)->iValue;
		t_dress.attr3 = dbfile->Get(i, 11)->iValue;
		t_dress.value3 = dbfile->Get(i, 12)->iValue;
		t_dress.attr4 = dbfile->Get(i, 13)->iValue;
		t_dress.value4 = dbfile->Get(i, 14)->iValue;
		t_dress.attr5 = dbfile->Get(i, 15)->iValue;
		t_dress.value5 = dbfile->Get(i, 16)->iValue;

		t_dress_[t_dress.id] = t_dress;
	}

	dbfile = game::scheme()->get_dbc("t_dress_target.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_dress_target t_dress_target;
		t_dress_target.id = dbfile->Get(i, 0)->iValue;
		t_dress_target.type = dbfile->Get(i, 3)->iValue;
		for (int j = 0; j < 5; ++j)
		{
			int def = dbfile->Get(i, 5 + j)->iValue;
			if (def)
			{
				t_dress_target.defs.push_back(def);
			}
		}
		t_dress_target.attr1 = dbfile->Get(i, 10)->iValue;
		t_dress_target.value1 = dbfile->Get(i, 11)->iValue;
		t_dress_target.attr2 = dbfile->Get(i, 12)->iValue;
		t_dress_target.value2 = dbfile->Get(i, 13)->iValue;
		t_dress_target.attr3 = dbfile->Get(i, 14)->iValue;
		t_dress_target.value3 = dbfile->Get(i, 15)->iValue;
		t_dress_target.attr4 = dbfile->Get(i, 16)->iValue;
		t_dress_target.value4 = dbfile->Get(i, 17)->iValue;

		t_dress_target_[t_dress_target.id] = t_dress_target;
	}

	dbfile = game::scheme()->get_dbc("t_equip_tz.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_equip_tz t_equip_tz;
		t_equip_tz.id = dbfile->Get(i, 0)->iValue;
		for (int j = 0; j < 4; ++j)
		{
			int id = dbfile->Get(i, 3 + j)->iValue;
			t_equip_tz.equip_ids.push_back(id);
			t_equip_tz_map_[id] = t_equip_tz.id;
		}
		t_equip_tz.attr1 = dbfile->Get(i, 7)->iValue;
		t_equip_tz.value1 = dbfile->Get(i, 8)->iValue;
		t_equip_tz.attr2 = dbfile->Get(i, 9)->iValue;
		t_equip_tz.value2 = dbfile->Get(i, 10)->iValue;
		t_equip_tz.attr3 = dbfile->Get(i, 11)->iValue;
		t_equip_tz.value3 = dbfile->Get(i, 12)->iValue;
		t_equip_tz.attr4 = dbfile->Get(i, 13)->iValue;
		t_equip_tz.value4 = dbfile->Get(i, 14)->iValue;

		t_equip_tz_[t_equip_tz.id] = t_equip_tz;
	}

	dbfile = game::scheme()->get_dbc("t_equip_jl.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_equip_jl jl;
		jl.level = dbfile->Get(i, 0)->iValue;

		for (int j = 0; j < 5; ++j)
		{
			std::pair<int, int> con;
			con.first = dbfile->Get(i, 1 + j * 2)->iValue;
			con.second = dbfile->Get(i, 2 + j * 2)->iValue;
			jl.consume.push_back(con);
		}

		t_equip_jinlian_[jl.level] = jl;
	}

	if (parse_equip_suipian() == -1)
	{
		return -1;
	}

	if (parse_dress_unlock() == -1)
	{
		return -1;
	}

	dbfile = game::scheme()->get_dbc("t_equip_skill.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_equip_skill t_equip_skill;
		t_equip_skill.id = dbfile->Get(i, 0)->iValue;
		t_equip_skill.bw = dbfile->Get(i, 3)->iValue;
		t_equip_skill.jl = dbfile->Get(i, 4)->iValue;
		t_equip_skill.type = dbfile->Get(i, 5)->iValue;
		t_equip_skill.def1 = dbfile->Get(i, 6)->iValue;
		t_equip_skill.def2 = dbfile->Get(i, 7)->iValue;

		t_equip_skill_[t_equip_skill.id] = t_equip_skill;
	}

	return 0;
}

int EquipConfig::parse_equip_suipian()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_item.txt");
	if (!dbfile)
	{
		return -1;
	}

	bool has_init = false;
	int ttype = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		if (dbfile->Get(i, 4)->iValue == 7001)
		{
			s_t_equip_suipian sp;
			sp.equip_id = dbfile->Get(i, 9)->iValue;
			sp.suipian_id = dbfile->Get(i, 0)->iValue;
			sp.suipian_num = dbfile->Get(i, 10)->iValue;
			sp.suipian_price = dbfile->Get(i, 13)->iValue;
			sp.suipian_specail = false;

			t_equip_suipian_[sp.equip_id] = sp;

			if (!has_init && dbfile->Get(i, 3)->iValue == 3)
			{
				has_init = true;
				t_suipian_special_ = sp;
				t_suipian_special_.suipian_specail = true;
			}
		}
	}

	return 0;
}

int EquipConfig::parse_dress_unlock()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_dress_unlock.txt");
	if (!dbfile)
	{
		return -1;
	}

	int last_id = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_dress_unlock un;
		un.id = dbfile->Get(i, 1)->iValue;
		un.last_id = last_id;
		un.num = dbfile->Get(i, 2)->iValue;
		last_id = un.id;
		t_dress_unlock_[un.id] = un;
	}

	return 0;
}

s_t_equip * EquipConfig::get_equip(uint32_t id)
{
	std::map<uint32_t, s_t_equip>::iterator it = t_equips_.find(id);
	if (it == t_equips_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

int EquipConfig::get_enhance(int color, int level)
{
	if (color <= 0 || color > t_enhances_.size())
	{
		return 0;
	}
	if (level < 0 || level >= t_enhances_[color - 1].size())
	{
		return 0;
	}
	return t_enhances_[color - 1][level];
}

int EquipConfig::get_enhance_total(int color, int level)
{
	if (color <= 0 || color > t_enhance_totals_.size())
	{
		return 0;
	}
	if (level < 0 || level >= t_enhance_totals_[color - 1].size())
	{
		return 0;
	}
	return t_enhance_totals_[color - 1][level];
}

s_t_gaizao * EquipConfig::get_gaizao(int font_color)
{
	if (font_color < 1 || font_color > t_gaizaos_.size())
	{
		return 0;
	}
	return &t_gaizaos_[font_color - 1];
}

s_t_dress * EquipConfig::get_dress(int id)
{
	std::map<int, s_t_dress>::iterator it = t_dress_.find(id);
	if (it == t_dress_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

const s_t_dress_target* EquipConfig::get_dress_target(int id) const
{
	std::map<int, s_t_dress_target>::const_iterator it = t_dress_target_.find(id);
	if (it == t_dress_target_.end())
	{
		return 0;
	}
	return &(it->second);
}

void EquipConfig::get_player_dress_attrs(dhc::player_t *player, std::map<int, double> &attrs)
{
	for (int i = 0; i < player->dress_ids_size(); ++i)
	{
		int dress_id = player->dress_ids(i);
		s_t_dress *t_dress = get_dress(dress_id);
		if (!t_dress)
		{
			continue;
		}
		attrs[t_dress->attr1] += t_dress->value1;
		attrs[t_dress->attr2] += t_dress->value2;
		attrs[t_dress->attr3] += t_dress->value3;
		attrs[t_dress->attr4] += t_dress->value4;
		attrs[t_dress->attr5] += t_dress->value5;
	}
	for (int i = 0; i < player->dress_achieves_size(); ++i)
	{
		const s_t_dress_target* t_target = get_dress_target(player->dress_achieves(i));
		if (t_target)
		{
			attrs[t_target->attr1] += t_target->value1;
			attrs[t_target->attr2] += t_target->value2;
			attrs[t_target->attr3] += t_target->value3;
			attrs[t_target->attr4] += t_target->value4;
		}
	}
	for (std::map<int, s_t_dress_target>::iterator it = t_dress_target_.begin(); it != t_dress_target_.end(); ++it)
	{
		s_t_dress_target &t_dress_target = (*it).second;
		if (t_dress_target.type == 2)
		{
			bool flag = true;
			for (int i = 0; i < t_dress_target.defs.size(); ++i)
			{
				if (!EquipOperation::player_has_dress(player, t_dress_target.defs[i]))
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				attrs[t_dress_target.attr1] += t_dress_target.value1;
				attrs[t_dress_target.attr2] += t_dress_target.value2;
				attrs[t_dress_target.attr3] += t_dress_target.value3;
				attrs[t_dress_target.attr4] += t_dress_target.value4;
			}
		}
	}
}

s_t_equip_tz * EquipConfig::get_equip_tz(int equip_id)
{
	if (t_equip_tz_map_.find(equip_id) == t_equip_tz_map_.end())
	{
		return 0;
	}
	int id = t_equip_tz_map_[equip_id];
	if (t_equip_tz_.find(id) == t_equip_tz_.end())
	{
		return 0;
	}
	return &t_equip_tz_[id];
}

s_t_equip_sx *EquipConfig::get_equip_sx(int color, int level)
{
	std::map<int, std::map<int, s_t_equip_sx> >::iterator it = t_equip_sx_.find(color);
	if (it == t_equip_sx_.end())
	{
		return 0;
	}

	std::map<int, s_t_equip_sx> &sx = it->second;
	std::map<int, s_t_equip_sx>::iterator jt = sx.find(level);
	if (jt == sx.end())
	{
		return 0;
	}

	return &(jt->second);
}

const s_t_equip_jl* EquipConfig::get_equip_jl(int level) const
{
	std::map<int, s_t_equip_jl>::const_iterator it = t_equip_jinlian_.find(level);
	if (it == t_equip_jinlian_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_equip_suipian* EquipConfig::get_equip_suipian(int equip_id) const
{
	if (equip_id == 110300 ||
		equip_id == 120300 ||
		equip_id == 130300 ||
		equip_id == 140300)
	{
		return &t_suipian_special_;
	}

	std::map<int, s_t_equip_suipian>::const_iterator it = t_equip_suipian_.find(equip_id);
	if (it == t_equip_suipian_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_dress_unlock* EquipConfig::get_dress_unlock(int dress_id) const
{
	std::map<int, s_t_dress_unlock>::const_iterator it = t_dress_unlock_.find(dress_id);
	if (it == t_dress_unlock_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_equip_skill * EquipConfig::get_equip_skill(int id) const
{
	std::map<int, s_t_equip_skill>::const_iterator it = t_equip_skill_.find(id);
	if (it == t_equip_skill_.end())
	{
		return 0;
	}
	return &(it->second);
}

void EquipConfig::get_equip_skills(int bw, int jl, std::vector<int> &ids)
{
	for (std::map<int, s_t_equip_skill>::const_iterator it = t_equip_skill_.begin(); it != t_equip_skill_.end(); ++it)
	{
		const s_t_equip_skill *t_equip_skill = &(it->second);
		if (bw == t_equip_skill->bw && jl >= t_equip_skill->jl)
		{
			ids.push_back(t_equip_skill->id);
		}
	}
}
