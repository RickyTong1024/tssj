#include "item_config.h"
#include "dbc.h"
#include "item_def.h"
#include "utils.h"
#include "role_operation.h"

int ItemConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_item.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_item t_item;
		t_item.id = dbfile->Get(i, 0)->iValue;
		t_item.name = dbfile->Get(i, 2)->pString;
		t_item.font_color = dbfile->Get(i, 3)->iValue;
		t_item.type = dbfile->Get(i, 4)->iValue;
		t_item.icon = dbfile->Get(i, 5)->pString;
		t_item.level = dbfile->Get(i, 8)->iValue;
		t_item.def1 = dbfile->Get(i, 9)->iValue;
		t_item.def2 = dbfile->Get(i, 10)->iValue;
		t_item.def3 = dbfile->Get(i, 11)->iValue;
		t_item.def4 = dbfile->Get(i, 12)->iValue;
		t_item.sell = dbfile->Get(i, 13)->iValue;
		t_item.can_use = dbfile->Get(i, 14)->iValue;
		t_item.jewel = dbfile->Get(i, 16)->iValue;

		if (t_item.type == IT_ROLE_SUIPIAN)
		{
			suipians_[t_item.def1] = t_item.id;
		}
		else if (t_item.type == IT_ROLE_JIYIN)
		{
			jiyins_[t_item.def1] = t_item.id;
		}

		t_items_[t_item.id] = t_item;
	}

	dbfile = game::scheme()->get_dbc("t_role_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_shop t_shop;
		t_shop.id = dbfile->Get(i, 0)->iValue;
		t_shop.gezi = dbfile->Get(i, 2)->iValue;
		t_shop.level = dbfile->Get(i, 3)->iValue;
		t_shop.type = dbfile->Get(i, 4)->iValue;
		t_shop.value1 = dbfile->Get(i, 5)->iValue;
		t_shop.value2 = dbfile->Get(i, 6)->iValue;
		t_shop.value3 = dbfile->Get(i, 7)->iValue;
		t_shop.rate = dbfile->Get(i, 8)->iValue;
		t_shop.hb_type = dbfile->Get(i, 9)->iValue;
		t_shop.hb = dbfile->Get(i, 10)->iValue;

		t_shops_[t_shop.id] = t_shop;
	}

	dbfile = game::scheme()->get_dbc("t_itemstore.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_itemstore t_itemstore;
		t_itemstore.id = dbfile->Get(i, 0)->iValue;
		t_itemstore.type = dbfile->Get(i, 2)->iValue;
		for (int j = 0; j < 100; ++j)
		{
			s_t_reward t_reward;
			t_reward.type = dbfile->Get(i, 3 + j * 5)->iValue;
			t_reward.value1 = dbfile->Get(i, 4 + j * 5)->iValue;
			t_reward.value2 = dbfile->Get(i, 5 + j * 5)->iValue;
			t_reward.value3 = dbfile->Get(i, 6 + j * 5)->iValue;
			if (t_reward.type)
			{
				t_itemstore.rewards.push_back(t_reward);
				int rate = dbfile->Get(i, 7 + j * 5)->iValue;
				t_itemstore.rates.push_back(rate);
			}
		}

		t_itemstores_[t_itemstore.id] = t_itemstore;
	}

	dbfile = game::scheme()->get_dbc("t_shop_xg.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_shop_xg t_shop_xg;
		t_shop_xg.id = dbfile->Get(i, 0)->iValue;
		t_shop_xg.recharge = dbfile->Get(i, 2)->iValue;
		t_shop_xg.xg_item.type = dbfile->Get(i, 3)->iValue;
		t_shop_xg.xg_item.value1 = dbfile->Get(i, 4)->iValue;
		t_shop_xg.xg_item.value2 = dbfile->Get(i, 5)->iValue;
		t_shop_xg.xg_item.value3 = dbfile->Get(i, 6)->iValue;
		t_shop_xg.xg_level = dbfile->Get(i, 8)->iValue;
		t_shop_xg.price_type = dbfile->Get(i, 9)->iValue;
		t_shop_xg.price = dbfile->Get(i, 10)->iValue;
		t_shop_xg.xg_type = dbfile->Get(i, 11)->iValue;
		t_shop_xg.xg_num = dbfile->Get(i, 12)->iValue;

		for (int j = 0; j < dbfile->GetFieldsNum() - 12; ++j)
		{
			int xg_num = dbfile->Get(i, 12 + j)->iValue;
			t_shop_xg.vip_xg_num.push_back(xg_num);
		}

		t_shop_xg_[t_shop_xg.id] = t_shop_xg;
	}

	if (parse_boss_shop() == -1)
	{
		return -1;
	}

	if (parse_ttt_shop() == -1)
	{
		return -1;
	}

	if (parse_guild_shop() == -1)
	{
		return -1;
	}

	if (parse_sport_shop() == -1)
	{
		return -1;
	}

	if (parse_item_hecheng() == -1)
	{
		return -1;
	}

	if (parse_huiyi_shop() == -1)
	{
		return -1;
	}

	if (parse_lieren_shop() == -1)
	{
		return -1;
	}

	if (parse_bingyuan_shop() == -1)
	{
		return -1;
	}

	if (parse_chongzhifanpai_shop() == -1)
	{
		return -1;
	}

	if (parse_chongwu_shop() == -1)
	{
		return -1;
	}

	if (parse_mofang_shop() == -1)
	{
		return -1;
	}

	return 0;
}

int ItemConfig::parse_boss_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_boss_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_boss_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.level = dbfile->Get(i, 2)->iValue;
		shop.type = dbfile->Get(i, 3)->iValue;
		shop.value1 = dbfile->Get(i, 4)->iValue;
		shop.value2 = dbfile->Get(i, 5)->iValue;
		shop.value3 = dbfile->Get(i, 6)->iValue;
		shop.price = dbfile->Get(i, 7)->iValue;
		shop.hongsehuoban = dbfile->Get(i, 8)->iValue;

		t_boss_shop_[shop.id] = shop;
	}

	return 0;
}

int ItemConfig::parse_ttt_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_ttt_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ttt_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.level = dbfile->Get(i, 3)->iValue;
		shop.type = dbfile->Get(i, 4)->iValue;
		shop.value1 = dbfile->Get(i, 5)->iValue;
		shop.value2 = dbfile->Get(i, 6)->iValue;
		shop.value3 = dbfile->Get(i, 7)->iValue;
		shop.price = dbfile->Get(i, 8)->iValue;
		//shop.red_force = dbfile->Get(i, 9)->iValue;
		//shop.star = dbfile->Get(i, 10)->iValue;

		t_ttt_shop_[shop.id] = shop;
	}

	dbfile = game::scheme()->get_dbc("t_ttt_baozang.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ttt_baozang bz;
		bz.id = dbfile->Get(i, 0)->iValue;
		bz.star = dbfile->Get(i, 1)->iValue;
		bz.type = dbfile->Get(i, 3)->iValue;
		bz.value1 = dbfile->Get(i, 4)->iValue;
		bz.value2 = dbfile->Get(i, 5)->iValue;
		bz.value3 = dbfile->Get(i, 6)->iValue;
		bz.price = dbfile->Get(i, 7)->iValue;
		bz.rate = dbfile->Get(i, 8)->iValue;

		t_ttt_baozang_[bz.id] = bz;
	}

	dbfile = game::scheme()->get_dbc("t_ttt_mubiao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ttt_mubiao mubiao;
		mubiao.id = dbfile->Get(i, 0)->iValue;
		mubiao.star = dbfile->Get(i, 1)->iValue;
		mubiao.type = dbfile->Get(i, 3)->iValue;
		mubiao.value1 = dbfile->Get(i, 4)->iValue;
		mubiao.value2 = dbfile->Get(i, 5)->iValue;
		mubiao.value3 = dbfile->Get(i, 6)->cValue;
		mubiao.price = dbfile->Get(i, 7)->iValue;

		t_ttt_mubiao_[mubiao.id] = mubiao;
	}

	return 0;
}

int ItemConfig::parse_guild_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_guild_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.type = dbfile->Get(i, 2)->iValue;
		shop.value1 = dbfile->Get(i, 3)->iValue;
		shop.value2 = dbfile->Get(i, 4)->iValue;
		shop.value3 = dbfile->Get(i, 5)->iValue;
		shop.contribution = dbfile->Get(i, 6)->iValue;
		shop.hongsehuoban = dbfile->Get(i, 7)->iValue;
		shop.num = dbfile->Get(i, 8)->iValue;
		shop.guild_level = dbfile->Get(i, 9)->iValue;

		t_guild_shop_[shop.id] = shop;
	}


	dbfile = game::scheme()->get_dbc("t_guild_mubiao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_mubiao mubiao;
		mubiao.id = dbfile->Get(i, 0)->iValue;
		mubiao.level = dbfile->Get(i, 1)->iValue;
		mubiao.type = dbfile->Get(i, 3)->iValue;
		mubiao.value1 = dbfile->Get(i, 4)->iValue;
		mubiao.value2 = dbfile->Get(i, 5)->iValue;
		mubiao.value3 = dbfile->Get(i, 6)->iValue;
		mubiao.price = dbfile->Get(i, 7)->iValue;

		t_guild_mubiao_[mubiao.id] = mubiao;
	}

	dbfile = game::scheme()->get_dbc("t_guild_shop_xs.txt");
	if (!dbfile)
	{
		return -1;
	}

	int gezi = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_shop_xs shop_xs;
		shop_xs.id = dbfile->Get(i, 0)->iValue;
		gezi = dbfile->Get(i, 2)->iValue;
		shop_xs.level = dbfile->Get(i, 3)->iValue;
		shop_xs.type = dbfile->Get(i, 4)->iValue;
		shop_xs.value1 = dbfile->Get(i, 5)->iValue;
		shop_xs.value2 = dbfile->Get(i, 6)->iValue;
		shop_xs.value3 = dbfile->Get(i, 7)->iValue;
		shop_xs.jewel = dbfile->Get(i, 8)->iValue;
		shop_xs.rate = dbfile->Get(i, 9)->iValue;
		shop_xs.num = dbfile->Get(i, 10)->iValue;
		t_guild_shop_xs_refresh_[gezi].push_back(shop_xs);
		t_guild_shop_xs_[shop_xs.id] = shop_xs;
	}

	dbfile = game::scheme()->get_dbc("t_guild_shop_ex.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_shop_ex shop_ex;
		shop_ex.id = dbfile->Get(i, 0)->iValue;
		shop_ex.type = dbfile->Get(i, 3)->iValue;
		shop_ex.value1 = dbfile->Get(i, 4)->iValue;
		shop_ex.value2 = dbfile->Get(i, 5)->iValue;
		shop_ex.vlaue3 = dbfile->Get(i, 6)->iValue;
		shop_ex.point = dbfile->Get(i, 7)->iValue;
		shop_ex.price = dbfile->Get(i, 8)->iValue;
		shop_ex.level = dbfile->Get(i, 9)->iValue;

		t_guild_shop_ex_[shop_ex.id] = shop_ex;
	}

	return 0;
}

int ItemConfig::parse_sport_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_sport_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_sport_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.level = dbfile->Get(i, 2)->iValue;
		shop.type = dbfile->Get(i, 3)->iValue;
		shop.value1 = dbfile->Get(i, 4)->iValue;
		shop.value2 = dbfile->Get(i, 5)->iValue;
		shop.value3 = dbfile->Get(i, 6)->iValue;
		shop.spower = dbfile->Get(i, 7)->iValue;
		shop.hongsehuoban = dbfile->Get(i, 8)->iValue;

		t_sport_shop_[shop.id] = shop;
	}

	dbfile = game::scheme()->get_dbc("t_sport_mubiao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_sport_mubiao mubiao;
		mubiao.id = dbfile->Get(i, 0)->iValue;
		mubiao.rank = dbfile->Get(i, 1)->iValue;
		mubiao.type = dbfile->Get(i, 3)->iValue;
		mubiao.value1 = dbfile->Get(i, 4)->iValue;
		mubiao.value2 = dbfile->Get(i, 5)->iValue;
		mubiao.value3 = dbfile->Get(i, 6)->iValue;
		mubiao.price = dbfile->Get(i, 7)->iValue;

		t_sport_mubiao_[mubiao.id] = mubiao;
	}


	return 0;
}

int ItemConfig::parse_item_hecheng()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_itemhecheng.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_item_hecheng hecheng;
		hecheng.id = dbfile->Get(i, 0)->iValue;
		hecheng.hecheng.type = dbfile->Get(i, 2)->iValue;
		hecheng.hecheng.value1 = dbfile->Get(i, 3)->iValue;
		hecheng.hecheng.value2 = dbfile->Get(i, 4)->iValue;

		for (int j = 0; j < 2; ++j)
		{
			s_t_reward rd;
			rd.type = dbfile->Get(i, 5 + j * 3)->iValue;
			rd.value1 = dbfile->Get(i, 6 + j * 3)->iValue;
			rd.value2 = dbfile->Get(i, 7 + j * 3)->iValue;
			if (rd.type != 0)
			{
				hecheng.cailiao.push_back(rd);
			}
		}
		t_item_hecheng_[hecheng.id] = hecheng;
	}

	return 0;
}

int ItemConfig::parse_huiyi_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_huiyi_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huiyi_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.gezi = dbfile->Get(i, 2)->iValue;
		shop.type = dbfile->Get(i, 3)->iValue;
		shop.value1 = dbfile->Get(i, 4)->iValue;
		shop.value2 = dbfile->Get(i, 5)->iValue;
		shop.value3 = dbfile->Get(i, 6)->iValue;
		shop.weight = dbfile->Get(i, 7)->iValue;
		shop.huobi = dbfile->Get(i, 8)->iValue;
		shop.price = dbfile->Get(i, 9)->iValue;

		t_huiyi_shop_[shop.id] = shop;
	}

	dbfile = game::scheme()->get_dbc("t_huiyi_luckshop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huiyi_luck_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.num = dbfile->Get(i, 3)->iValue;
		shop.type = dbfile->Get(i, 4)->iValue;
		shop.value1 = dbfile->Get(i, 5)->iValue;
		shop.value2 = dbfile->Get(i, 6)->iValue;
		shop.value3 = dbfile->Get(i, 7)->iValue;
		shop.luck_point = dbfile->Get(i, 8)->iValue;

		t_huiyi_luck_shop_[shop.id] = shop;
	}

	return 0;
}

int ItemConfig::parse_lieren_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_lieren_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_lieren_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.type = dbfile->Get(i, 2)->iValue;
		shop.value1 = dbfile->Get(i, 3)->iValue;
		shop.value2 = dbfile->Get(i, 4)->iValue;
		shop.value3 = dbfile->Get(i, 5)->iValue;
		shop.huobi = dbfile->Get(i, 6)->iValue;
		shop.hongsehuoban = dbfile->Get(i, 7)->iValue;
		shop.hongsezhuangbei = dbfile->Get(i, 8)->iValue;

		t_lieren_shop_[shop.id] = shop;
	}

	return 0;
}

int ItemConfig::parse_bingyuan_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_bingyuan_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_bingyuan_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.type = dbfile->Get(i, 2)->iValue;
		shop.value1 = dbfile->Get(i, 3)->iValue;
		shop.value2 = dbfile->Get(i, 4)->iValue;
		shop.value3 = dbfile->Get(i, 5)->iValue;
		shop.bingjing = dbfile->Get(i, 6)->iValue;
		shop.num = dbfile->Get(i, 7)->iValue;

		t_bingyuan_shop_[shop.id] = shop;
	}


	dbfile = game::scheme()->get_dbc("t_bingyuan_mubiao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_bingyuan_mubiao bm;
		bm.id = dbfile->Get(i, 0)->iValue;
		bm.point = dbfile->Get(i, 1)->iValue;
		bm.type = dbfile->Get(i, 3)->iValue;
		bm.value1 = dbfile->Get(i, 4)->iValue;
		bm.value2 = dbfile->Get(i, 5)->iValue;
		bm.value3 = dbfile->Get(i, 6)->iValue;
		bm.price = dbfile->Get(i, 7)->iValue;

		t_bingyuan_mubiao_[bm.id] = bm;
	}

	return 0;
}

int ItemConfig::parse_chongzhifanpai_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_chongzhifanpai_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_chongzhifanpai_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.type = dbfile->Get(i, 1)->iValue;
		shop.value1 = dbfile->Get(i, 2)->iValue;
		shop.value2 = dbfile->Get(i, 3)->iValue;
		shop.value3 = dbfile->Get(i, 4)->iValue;
		shop.price = dbfile->Get(i, 6)->iValue;

		t_chongzhifanpai_shop_[shop.id] = shop;
	}
	return 0;
}

int ItemConfig::parse_chongwu_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_chongwu_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_chongwu_shop cs;
		cs.id = dbfile->Get(i, 0)->iValue;
		cs.level = dbfile->Get(i, 3)->iValue;
		cs.type = dbfile->Get(i, 4)->iValue;
		cs.value1 = dbfile->Get(i, 5)->iValue;
		cs.value2 = dbfile->Get(i, 6)->iValue;
		cs.value3 = dbfile->Get(i, 7)->iValue;
		cs.weight = dbfile->Get(i, 8)->iValue;
		cs.huobi = dbfile->Get(i, 9)->iValue;
		cs.price = dbfile->Get(i, 10)->iValue;

		t_chongwu_shop_[cs.id] = cs;
		t_chongwu_shop_refresh_[dbfile->Get(i, 2)->iValue].push_back(cs);
	}

	return 0;
}

int ItemConfig::parse_mofang_shop()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_mofang_shop.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_mofang_shop mf;
		mf.id = dbfile->Get(i, 0)->iValue;
		mf.type = dbfile->Get(i, 2)->iValue;
		mf.value1 = dbfile->Get(i, 3)->iValue;
		mf.value2 = dbfile->Get(i, 4)->iValue;
		mf.value3 = dbfile->Get(i, 5)->iValue;
		mf.price = dbfile->Get(i, 6)->iValue;
		mf.num = dbfile->Get(i, 7)->iValue;

		t_mofang_shop_[mf.id] = mf;
	}

	return 0;
}

s_t_item * ItemConfig::get_item(uint32_t id)
{
	std::map<uint32_t, s_t_item>::iterator it = t_items_.find(id);
	if (it == t_items_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_shop * ItemConfig::get_ramdom_shop(dhc::player_t * player, int type, int gezi)
{
	int sum = 0;
	for (std::map<uint32_t, s_t_shop>::iterator it = t_shops_.begin(); it != t_shops_.end(); ++it)
	{
		s_t_shop &t_shop = (*it).second;
		if (t_shop.gezi == gezi && player->level() >= t_shop.level)
		{
			int r = t_shop.rate;
			if (type == 2 && gezi == 2)
			{
				s_t_item *t_item = sItemConfig->get_item(t_shop.value1);
				if (t_item && t_item->type == 3001)
				{
					if (RoleOperation::has_role(player, t_item->def1))
					{
						r = r * 10;
					}
					if (RoleOperation::has_zheng(player, t_item->def1))
					{
						r = r * 3;
					}
				}
			}
			sum += r;
		}
	}
	if (sum == 0)
	{
		return 0;
	}
	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::map<uint32_t, s_t_shop>::iterator it = t_shops_.begin(); it != t_shops_.end(); ++it)
	{
		s_t_shop &t_shop = (*it).second;
		if (t_shop.gezi == gezi && player->level() >= t_shop.level)
		{
			int r = t_shop.rate;
			if (type == 2 && gezi == 2)
			{
				s_t_item *t_item = sItemConfig->get_item(t_shop.value1);
				if (t_item && t_item->type == 3001)
				{
					if (RoleOperation::has_role(player, t_item->def1))
					{
						r = r * 10;
					}
					if (RoleOperation::has_zheng(player, t_item->def1))
					{
						r = r * 3;
					}
				}
			}
			gl += r;
			if (gl > rate)
			{
				return &t_shop;
			}
		}
	}
	return 0;
}

s_t_shop * ItemConfig::get_random_role_shop(dhc::player_t *player, int gezi, std::vector<int>& refreshs)
{
	int sum = 0;
	for (std::map<uint32_t, s_t_shop>::iterator it = t_shops_.begin(); it != t_shops_.end(); ++it)
	{
		s_t_shop &t_shop = (*it).second;
		if (t_shop.gezi == gezi &&
			player->level() >= t_shop.level &&
			(std::find(refreshs.begin(), refreshs.end(), t_shop.value1) == refreshs.end()))
		{
			sum += t_shop.rate;
		}
	}
	if (sum == 0)
	{
		return 0;
	}
	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::map<uint32_t, s_t_shop>::iterator it = t_shops_.begin(); it != t_shops_.end(); ++it)
	{
		s_t_shop &t_shop = (*it).second;
		if (t_shop.gezi == gezi && 
			player->level() >= t_shop.level &&
			(std::find(refreshs.begin(), refreshs.end(), t_shop.value1) == refreshs.end()))
		{
			gl += t_shop.rate;
			if (gl > rate)
			{
				return &t_shop;
			}
		}
	}
	return 0;
}

s_t_shop * ItemConfig::get_shop(uint32_t id)
{
	std::map<uint32_t, s_t_shop>::iterator it = t_shops_.find(id);
	if (it == t_shops_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

uint32_t ItemConfig::get_suipian(int role_id)
{
	if (suipians_.find(role_id) == suipians_.end())
	{
		return 0;
	}
	return suipians_[role_id];
}

uint32_t ItemConfig::get_jiyin(int role_id)
{
	if (jiyins_.find(role_id) == jiyins_.end())
	{
		return 0;
	}
	return jiyins_[role_id];
}

s_t_itemstore * ItemConfig::get_itemstore(uint32_t id)
{
	std::map<uint32_t, s_t_itemstore>::iterator it = t_itemstores_.find(id);
	if (it == t_itemstores_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_shop_xg * ItemConfig::get_shop_xg(int id)
{
	std::map<int, s_t_shop_xg>::iterator it = t_shop_xg_.find(id);
	if (it == t_shop_xg_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

const s_t_ttt_baozang* ItemConfig::get_ttt_baozhang(int id) const
{
	std::map<int, s_t_ttt_baozang>::const_iterator it = t_ttt_baozang_.find(id);
	if (it == t_ttt_baozang_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_ttt_baozang* ItemConfig::get_ttt_random_baozhang(int star) const
{
	int sum = 0;
	for (std::map<int, s_t_ttt_baozang>::const_iterator it = t_ttt_baozang_.begin();
		it != t_ttt_baozang_.end();
		++it)
	{
		const s_t_ttt_baozang& mibao = it->second;
		if (star >= mibao.star)
		{
			sum += mibao.rate;
		}
	}

	if (sum == 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::map<int, s_t_ttt_baozang>::const_iterator it = t_ttt_baozang_.begin();
		it != t_ttt_baozang_.end();
		++it)
	{
		const s_t_ttt_baozang& mibao = it->second;
		if (star >= mibao.star)
		{
			gl += mibao.rate;
			if (gl > rate)
			{
				return &(it->second);
			}
		}
	}
	return 0;
}

const s_t_boss_shop* ItemConfig::get_boss_shop(int id) const
{
	std::map<int, s_t_boss_shop>::const_iterator it = t_boss_shop_.find(id);
	if (it == t_boss_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_ttt_shop* ItemConfig::get_ttt_shop(int id) const
{
	std::map<int, s_t_ttt_shop>::const_iterator it = t_ttt_shop_.find(id);
	if (it == t_ttt_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guild_shop* ItemConfig::get_guild_shop(int id) const
{
	std::map<int, s_t_guild_shop>::const_iterator it = t_guild_shop_.find(id);
	if (it == t_guild_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_sport_shop* ItemConfig::get_sport_shop(int id) const
{
	std::map<int, s_t_sport_shop>::const_iterator it = t_sport_shop_.find(id);
	if (it == t_sport_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_ttt_mubiao* ItemConfig::get_ttt_mubiao(int id) const
{
	std::map<int, s_t_ttt_mubiao>::const_iterator it = t_ttt_mubiao_.find(id);
	if (it == t_ttt_mubiao_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guild_mubiao* ItemConfig::get_guild_mubiao(int id) const
{
	std::map<int, s_t_guild_mubiao>::const_iterator it = t_guild_mubiao_.find(id);
	if (it == t_guild_mubiao_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_sport_mubiao* ItemConfig::get_sport_mubiao(int id) const
{
	std::map<int, s_t_sport_mubiao>::const_iterator it = t_sport_mubiao_.find(id);
	if (it == t_sport_mubiao_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guild_shop_xs* ItemConfig::get_guild_shop_xs(int id) const
{
	std::map<int, s_t_guild_shop_xs>::const_iterator it = t_guild_shop_xs_.find(id);
	if (it == t_guild_shop_xs_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guild_shop_ex* ItemConfig::get_guild_shop_ex(int id) const
{
	std::map<int, s_t_guild_shop_ex>::const_iterator it = t_guild_shop_ex_.find(id);
	if (it == t_guild_shop_ex_.end())
	{
		return 0;
	}
	return &(it->second);
}

int ItemConfig::get_guild_shop_xs(int gezi, int level, const std::set<int>& ids) const
{
	std::map<int, std::vector<s_t_guild_shop_xs> >::const_iterator it = t_guild_shop_xs_refresh_.find(gezi);
	if (it == t_guild_shop_xs_refresh_.end())
	{
		return 0;
	}

	int sum = 0;
	const std::vector<s_t_guild_shop_xs>& xgs = it->second;
	for (std::vector<s_t_guild_shop_xs>::size_type i = 0;
		i < xgs.size();
		++i)
	{
		if (level >= xgs[i].level &&
			ids.find(xgs[i].id) == ids.end())
		{
			sum += xgs[i].rate;
		}
		
	}
	if (sum == 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::vector<s_t_guild_shop_xs>::size_type i = 0;
		i < xgs.size();
		++i)
	{
		if (level >= xgs[i].level &&
			ids.find(xgs[i].id) == ids.end())
		{
			gl += xgs[i].rate;
			if (gl > rate)
			{
				return xgs[i].id;
			}
		}

	}
	return 0;
}

const s_t_item_hecheng* ItemConfig::get_item_hecheng(int id) const
{
	std::map<int, s_t_item_hecheng>::const_iterator it = t_item_hecheng_.find(id);
	if (it == t_item_hecheng_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_huiyi_shop* ItemConfig::get_huiyi_shop(int id) const
{
	std::map<int, s_t_huiyi_shop>::const_iterator it = t_huiyi_shop_.find(id);
	if (it == t_huiyi_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_huiyi_shop* ItemConfig::get_huiyi_shop_random(int gezi, const std::set<int>& ids) const
{
	int sum = 0;
	for (std::map<int, s_t_huiyi_shop>::const_iterator it = t_huiyi_shop_.begin();
		it != t_huiyi_shop_.end();
		++it)
	{
		if (it->second.gezi == gezi &&
			ids.find(it->second.id) == ids.end())
		{
			sum += it->second.weight;
		}
	}

	if (sum == 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::map<int, s_t_huiyi_shop>::const_iterator it = t_huiyi_shop_.begin();
		it != t_huiyi_shop_.end();
		++it)
	{
		if (it->second.gezi == gezi &&
			ids.find(it->second.id) == ids.end())
		{
			gl += it->second.weight;
			if (gl > rate)
			{
				return &(it->second);
			}
		}
	}

	return 0;
}

const s_t_huiyi_luck_shop* ItemConfig::get_huiyi_luckshop(int id) const
{
	std::map<int, s_t_huiyi_luck_shop>::const_iterator it = t_huiyi_luck_shop_.find(id);
	if (it == t_huiyi_luck_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_lieren_shop* ItemConfig::get_lieren_shop(int id) const
{
	std::map<int, s_t_lieren_shop>::const_iterator it = t_lieren_shop_.find(id);
	if (it == t_lieren_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_bingyuan_shop* ItemConfig::get_bingyuan_shop(int id) const
{
	std::map<int, s_t_bingyuan_shop>::const_iterator it = t_bingyuan_shop_.find(id);
	if (it == t_bingyuan_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_bingyuan_mubiao* ItemConfig::get_bingyuan_mubiao(int id) const
{
	std::map<int, s_t_bingyuan_mubiao>::const_iterator it = t_bingyuan_mubiao_.find(id);
	if (it == t_bingyuan_mubiao_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_chongzhifanpai_shop* ItemConfig::get_chongzhifanpai_shop(int id) const
{
	std::map<int, s_t_chongzhifanpai_shop>::const_iterator it = t_chongzhifanpai_shop_.find(id);
	if (it == t_chongzhifanpai_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_chongwu_shop* ItemConfig::get_chongwu_shop(int id) const
{
	std::map<int, s_t_chongwu_shop>::const_iterator it = t_chongwu_shop_.find(id);
	if (it == t_chongwu_shop_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_chongwu_shop* ItemConfig::get_chongwu_shop_random(int gezi, int level, const std::set<int>& ids) const
{
	std::map<int, std::vector<s_t_chongwu_shop> >::const_iterator it = t_chongwu_shop_refresh_.find(gezi);
	if (it == t_chongwu_shop_refresh_.end())
	{
		return 0;
	}

	int sum = 0;
	const std::vector<s_t_chongwu_shop> &gezis = it->second;
	for (int i = 0; i < gezis.size(); ++i)
	{
		if (level >= gezis[i].level &&
			ids.find(gezis[i].value1) == ids.end())
		{
			sum += gezis[i].weight;
		}
	}

	if (sum <= 0)
	{
		return 0;
	}

	int gl = 0;
	int rate = Utils::get_int32(0, sum - 1);
	for (int i = 0; i < gezis.size(); ++i)
	{
		if (level >= gezis[i].level &&
			ids.find(gezis[i].value1) == ids.end())
		{
			gl += gezis[i].weight;
			if (gl > rate)
			{
				return &(gezis[i]);
			}
		}
	}
	return 0;
}

const s_t_mofang_shop* ItemConfig::get_mofang_shop(int id) const
{
	std::map<int, s_t_mofang_shop>::const_iterator it = t_mofang_shop_.find(id);
	if (it == t_mofang_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}