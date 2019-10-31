#include "treasure_config.h"
#include "dbc.h"

int TreasureConfig::parse()
{
	if (parse_treasure() == -1)
	{
		return -1;
	}

	if (parse_treasure_enhance() == -1)
	{
		return -1;
	}

	if (parse_treasure_jinlian() == -1)
	{
		return -1;
	}

	if (parse_treasure_suipian() == -1)
	{
		return -1;
	}

	if (parse_treasure_sx() == -1)
	{
		return -1;
	}

	return 0;
}


int TreasureConfig::parse_treasure()
{
	DBCFile *dbfile = game::scheme()->get_dbc("t_baowu.txt");
	if (!dbfile)
	{
		return -1;
	}


	int id = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_treasure treasure;
		treasure.id = dbfile->Get(i, 0)->iValue;
		treasure.color = dbfile->Get(i, 3)->iValue;
		treasure.type = dbfile->Get(i, 4)->iValue;
		treasure.exp = dbfile->Get(i, 6)->iValue;
		treasure.sell_price = dbfile->Get(i, 7)->iValue;
		treasure.att1.att = dbfile->Get(i, 8)->iValue;
		treasure.att1.val = dbfile->Get(i, 9)->dValue;
		treasure.att2.att = dbfile->Get(i, 10)->iValue;
		treasure.att2.val = dbfile->Get(i, 11)->dValue;
		treasure.jl_att1.att = dbfile->Get(i, 12)->iValue;
		treasure.jl_att1.val = dbfile->Get(i, 13)->dValue;
		treasure.jl_att2.att = dbfile->Get(i, 14)->iValue;
		treasure.jl_att2.val = dbfile->Get(i, 15)->dValue;
		treasure.jl_att3.att = dbfile->Get(i, 16)->iValue;
		treasure.jl_att3.val = dbfile->Get(i, 17)->dValue;

		for (int j = 0; j < 6; ++j)
		{
			id = dbfile->Get(i, 18 + j * 1)->iValue;
			if (id != 0)
			{
				treasure.suipian.push_back(id);
			}
		}

		t_treasures_[treasure.id] = treasure;
	}

	return 0;
}


int TreasureConfig::parse_treasure_enhance()
{
	DBCFile *dbfile = game::scheme()->get_dbc("t_baowu_enhance.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_treasure_enhance hance;
		hance.level = dbfile->Get(i, 0)->iValue;

		for (int j = 0; j < 4; ++j)
		{
			int val = dbfile->Get(i, 1 + j * 1)->iValue;
			hance.attrs.push_back(val);
		}

		t_enhances_[hance.level] = hance;
	}

	return 0;
}

int TreasureConfig::parse_treasure_jinlian()
{
	DBCFile *dbfile = game::scheme()->get_dbc("t_baowu_jl.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_treasure_jinlian jinlian;
		jinlian.level = dbfile->Get(i, 0)->iValue;
		jinlian.item_num = dbfile->Get(i, 1)->iValue;
		jinlian.gold = dbfile->Get(i, 2)->iValue;
		jinlian.baowu_num = dbfile->Get(i, 3)->iValue;

		t_jinlians_[jinlian.level] = jinlian;
	}

	return 0;
}

int TreasureConfig::parse_treasure_suipian()
{
	DBCFile *dbfile = game::scheme()->get_dbc("t_item.txt");
	if (!dbfile)
	{
		return -1;
	}

	int type = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		type = dbfile->Get(i, 4)->iValue;
		if (type == 6001)
		{
			s_t_treasure_suipian suipian;
			suipian.template_id = dbfile->Get(i, 0)->iValue;
			suipian.treasure_id = dbfile->Get(i, 9)->iValue;
			suipian.ordernum = dbfile->Get(i, 10)->iValue;
			suipian.player_rate = dbfile->Get(i, 11)->iValue;
			suipian.npc_rate = dbfile->Get(i, 12)->iValue;
			

			t_suipians_[suipian.template_id] = suipian;
		}
	}

	return 0;
}

int TreasureConfig::parse_treasure_sx()
{
	DBCFile *dbfile = game::scheme()->get_dbc("t_baowu_sx.txt");
	if (!dbfile)
	{
		return -1;
	}


	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_treasure_sx sx;
		sx.level = dbfile->Get(i, 0)->iValue;
		sx.color = dbfile->Get(i, 1)->iValue;
		sx.gold = dbfile->Get(i, 2)->iValue;
		sx.jewel = dbfile->Get(i, 3)->iValue;
		sx.jindu = dbfile->Get(i, 4)->iValue;
		sx.rate = dbfile->Get(i, 5)->iValue;
		sx.value1 = dbfile->Get(i, 6)->iValue;
		sx.value2 = dbfile->Get(i, 7)->iValue;
		sx.valuemax = dbfile->Get(i, 8)->iValue;

		t_treasure_sx_[sx.color][sx.level] = sx;
	}

	return 0;
}

const s_t_treasure* TreasureConfig::get_treasure(int id) const
{
	std::map<int, s_t_treasure>::const_iterator it = t_treasures_.find(id);
	if (it == t_treasures_.end())
	{
		return 0;
	}
	return &(it->second);
}


const s_t_treasure_enhance *TreasureConfig::get_enhance(int level) const
{
	std::map<int, s_t_treasure_enhance>::const_iterator it = t_enhances_.find(level);
	if (it == t_enhances_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_treasure_jinlian *TreasureConfig::get_jinlian(int level) const
{
	std::map<int, s_t_treasure_jinlian>::const_iterator it = t_jinlians_.find(level);
	if (it == t_jinlians_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_treasure_suipian *TreasureConfig::get_suipian(int id) const
{
	std::map<int, s_t_treasure_suipian>::const_iterator it = t_suipians_.find(id);
	if (it == t_suipians_.end())
	{
		return 0;
	}

	return &(it->second);
}

const std::map<int, s_t_treasure_suipian>& TreasureConfig::get_all_suipian() const
{
	return t_suipians_;
}

const std::map<int, s_t_treasure>& TreasureConfig::get_all_treasure() const
{
	return t_treasures_;
}

int TreasureConfig::get_enhance_max() const
{
	return t_enhances_.size() - 1;
}

const s_t_treasure_sx* TreasureConfig::get_sx(int level, int color) const
{
	std::map<int, std::map<int, s_t_treasure_sx> >::const_iterator it = t_treasure_sx_.find(color);
	if (it == t_treasure_sx_.end())
	{
		return 0;
	}

	const std::map<int, s_t_treasure_sx>& tsx = it->second;
	std::map<int, s_t_treasure_sx>::const_iterator jt = tsx.find(level);
	if (jt == tsx.end())
	{
		return 0;
	}
	return &(jt->second);
}