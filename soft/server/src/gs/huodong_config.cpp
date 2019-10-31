#include "huodong_config.h"
#include "dbc.h"
#include "utils.h"

int HuodongConfig::parse()
{
	if (parse_pttq() == -1)
	{
		return -1;
	}

	if (parse_czjh() == -1)
	{
		return -1;
	}

	if (parse_kf() == -1)
	{
		return -1;
	}

	if (parse_vip_libao() == -1)
	{
		return -1;
	}

	if (parse_week_libao() == -1)
	{
		return -1;
	}

	if (parse_tanbao() == -1)
	{
		return -1;
	}

	if (parse_czfp() == -1)
	{
		return -1;
	}

	if (parse_szzp() == -1)
	{
		return -1;
	}

	if (parse_tansuo() == -1)
	{
		return -1;
	}

	if (parse_mofang() == -1)
	{
		return -1;
	}

	if (parse_yueka() == -1)
	{
		return -1;
	}

	if (parse_huigui() == -1)
	{
		return -1;
	}
	return 0;
}

int HuodongConfig::parse_pttq()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_huodong_pttq.txt");
	if (!dbfile)
	{
		return -1;
	}

	int vip = 0;
	pttq_min_vip_ = 100;
	pttq_min_id_ = 1;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huodong_pttq pt;
		pt.id = dbfile->Get(i, 0)->iValue;
		pt.gvip = dbfile->Get(i, 1)->iValue;
		if (pt.gvip < pttq_min_vip_)
		{
			pttq_min_vip_ = pt.gvip;
			pttq_min_id_ = pt.id;
		}

		for (int j = 0; j < 4; ++j)
		{
			s_t_reward reward;
			vip = dbfile->Get(i, 2 + j * 5)->iValue;
			reward.type = dbfile->Get(i, 3 + j * 5)->iValue;
			reward.value1 = dbfile->Get(i, 4 + j * 5)->iValue;
			reward.value2 = dbfile->Get(i, 5 + j * 5)->iValue;
			reward.value3 = dbfile->Get(i, 6 + j * 5)->iValue;
			pt.rewards[vip].push_back(reward);
		}

		t_huodong_pttq_[pt.id] = pt;
	}

	return 0;
}

int HuodongConfig::parse_czjh()
{
	DBCFile *dbfile = game::scheme()->get_dbc("t_huodong_czjh.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huodong_czjh t_huodong_czjh;

		t_huodong_czjh.index = dbfile->Get(i, 0)->iValue;
		t_huodong_czjh.level = dbfile->Get(i, 1)->iValue;
		t_huodong_czjh.jewel = dbfile->Get(i, 2)->iValue;

		t_huodong_czjh_[t_huodong_czjh.index] = t_huodong_czjh;
	}

	dbfile = game::scheme()->get_dbc("t_huodong_czjhrs.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huodong_czjhrs t_huodong_czjh;

		t_huodong_czjh.index = dbfile->Get(i, 0)->iValue;;
		t_huodong_czjh.count = dbfile->Get(i, 1)->iValue;
		t_huodong_czjh.rd.type = dbfile->Get(i, 2)->iValue;
		t_huodong_czjh.rd.value1 = dbfile->Get(i, 3)->iValue;
		t_huodong_czjh.rd.value2 = dbfile->Get(i, 4)->iValue;
		t_huodong_czjh.rd.value3 = dbfile->Get(i, 5)->iValue;

		t_huodong_czjh_count_[t_huodong_czjh.index] = t_huodong_czjh;
	}

	return 0;
}

int HuodongConfig::parse_kf()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_kaifu_mubiao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huodong_kf_mubiao mb;
		mb.id = dbfile->Get(i, 0)->iValue;
		mb.type = dbfile->Get(i, 3)->iValue;
		mb.def1 = dbfile->Get(i, 4)->iValue;
		mb.def2 = dbfile->Get(i, 5)->iValue;
		mb.def3 = dbfile->Get(i, 6)->iValue;
		mb.def4 = dbfile->Get(i, 7)->iValue;
		mb.cankao = dbfile->Get(i, 8)->iValue;
		for (int j = 0; j < 4; ++j)
		{
			s_t_reward reward;
			reward.type = dbfile->Get(i, 9 + j * 4)->iValue;
			reward.value1 = dbfile->Get(i, 10 + j * 4)->iValue;
			reward.value2 = dbfile->Get(i, 11 + j * 4)->iValue;
			reward.value3 = dbfile->Get(i, 12 + j * 4)->iValue;

			if (reward.type != 0)
			{
				mb.rewards.push_back(reward);
			}
		}
		t_huodong_kf_target_[mb.id] = mb;
	}


	dbfile = game::scheme()->get_dbc("t_kaifu.txt");
	if (!dbfile)
	{
		return -1;
	}
	
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		int day = dbfile->Get(i, 0)->iValue;

		s_t_huodong_kf &kf = t_huodong_kf_[day];

		int renwu_id = dbfile->Get(i, 4)->iValue;
		const s_t_huodong_kf_mubiao *mubiao = get_t_huodong_kf_mubiao(renwu_id);
		if (mubiao && mubiao->type == 200)
		{
			kf.ids.push_back(renwu_id);
			kf.xg_id = renwu_id;
			kf.type = dbfile->Get(i, 5)->iValue;
			kf.value1 = dbfile->Get(i, 6)->iValue;
			kf.value2 = dbfile->Get(i, 7)->iValue;
			kf.value3 = dbfile->Get(i, 8)->iValue;
			kf.jewel = dbfile->Get(i, 9)->iValue;
			kf.count = dbfile->Get(i, 11)->iValue;
		}
		else
		{
			for (int j = 0; j < 15; ++j)
			{
				renwu_id = dbfile->Get(i, 4 + j)->iValue;
				if (renwu_id != 0)
				{
					kf.ids.push_back(renwu_id);
				}
			}
		}
	}

	return 0;
}

int HuodongConfig::parse_vip_libao()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_vip_libao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huodong_vip_libao libao;
		libao.vip = dbfile->Get(i, 0)->iValue;

		for (int j = 0; j < 6; ++j)
		{
			s_t_reward rd;
			rd.type = dbfile->Get(i, 1 + j * 4)->iValue;
			rd.value1 = dbfile->Get(i, 2 + j * 4)->iValue;
			rd.value2 = dbfile->Get(i, 3 + j * 4)->iValue;
			rd.value3 = dbfile->Get(i, 4 + j * 4)->iValue;
			if (rd.type != 0)
			{
				libao.rewards.push_back(rd);
			}
		}
		t_huodong_vip_libao_.push_back(libao);
	}

	return 0;
}

int HuodongConfig::parse_week_libao()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_week_libao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_huodong_week_libao libao;
		libao.id = dbfile->Get(i, 0)->iValue;
		libao.level1 = dbfile->Get(i, 3)->iValue;
		libao.level2 = dbfile->Get(i, 4)->iValue;
		libao.jewel = dbfile->Get(i, 21)->iValue;
		libao.num = dbfile->Get(i, 22)->iValue;
		libao.zhekou = dbfile->Get(i, 23)->iValue;

		for (int j = 0; j < 4; ++j)
		{
			s_t_reward rd;
			rd.type = dbfile->Get(i, 5 + j * 4)->iValue;
			rd.value1 = dbfile->Get(i, 6 + j * 4)->iValue;
			rd.value2 = dbfile->Get(i, 7 + j * 4)->iValue;
			rd.value3 = dbfile->Get(i, 8 + j * 4)->iValue;
			if (rd.type != 0)
			{
				libao.rds.push_back(rd);
			}
		}
		t_huodong_week_libao_[libao.id] = libao;
	}

	return 0;
}

int HuodongConfig::parse_tanbao()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_tanbao_event.txt");
	if (!dbfile)
	{
		return -1;
	}

	int gezi = 1;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tanbao tanbao;
		gezi = dbfile->Get(i, 0)->iValue;
		tanbao.type = dbfile->Get(i, 2)->iValue;
		tanbao.shop_type = dbfile->Get(i, 3)->iValue;
		if (tanbao.type == 1 || tanbao.type == 2 || tanbao.type == 5)
		{
			s_t_tanbao_event et;
			et.type = dbfile->Get(i, 5)->iValue;
			et.value1 = dbfile->Get(i, 6)->iValue;
			et.value2 = dbfile->Get(i, 7)->iValue;
			et.value3 = dbfile->Get(i, 8)->iValue;
			tanbao.events.push_back(et);
		}
		else if (tanbao.type == 4)
		{
			for (int j = 0; j < 5; ++j)
			{
				s_t_tanbao_event et;
				et.type = dbfile->Get(i, 11 + j * 7)->iValue;
				et.value1 = dbfile->Get(i, 12 + j * 7)->iValue;
				et.value2 = dbfile->Get(i, 13 + j * 7)->iValue;
				et.value3 = dbfile->Get(i, 14 + j * 7)->iValue;
				et.rate = dbfile->Get(i, 15 + j * 7)->iValue;
				if (et.type != 0)
				{
					tanbao.events.push_back(et);
				}
			}
		}

		t_huodong_tanbao_[gezi] = tanbao;
	}

	dbfile = game::scheme()->get_dbc("t_tanbao_mubiao.txt");
	if (!dbfile)
	{
		return -1;
	}


	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tanbao_mubiao mubiao;
		mubiao.id = dbfile->Get(i, 0)->iValue;
		mubiao.num = dbfile->Get(i, 1)->iValue;
		mubiao.type = dbfile->Get(i, 3)->iValue;
		mubiao.value1 = dbfile->Get(i, 4)->iValue;
		mubiao.value2 = dbfile->Get(i, 5)->iValue;
		mubiao.value3 = dbfile->Get(i, 6)->iValue;

		t_huodong_tanbao_mubiao_[mubiao.id] = mubiao;
	}

	dbfile = game::scheme()->get_dbc("t_tanbao_shop.txt");
	if (!dbfile)
	{
		return -1;
	}


	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tanbao_shop shop;
		shop.id = dbfile->Get(i, 0)->iValue;
		shop.shop_type = dbfile->Get(i, 2)->iValue;
		shop.type = dbfile->Get(i, 3)->iValue;
		shop.value1 = dbfile->Get(i, 4)->iValue;
		shop.value2 = dbfile->Get(i, 5)->iValue;
		shop.value3 = dbfile->Get(i, 6)->iValue;
		shop.price = dbfile->Get(i, 7)->iValue;
		shop.point = dbfile->Get(i, 9)->iValue;
		shop.num = dbfile->Get(i, 10)->iValue;

		t_huodong_tanbao_shop_[shop.id] = shop;
	}

	dbfile = game::scheme()->get_dbc("t_tanbao_reward.txt");
	if (!dbfile)
	{
		return -1;
	}


	int type;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tanbao_reward rd;
		rd.rank1 = dbfile->Get(i, 0)->iValue;
		rd.rank2 = dbfile->Get(i, 1)->iValue;
		type = dbfile->Get(i, 2)->iValue;

		for (int j = 0; j < 3; ++j)
		{
			s_t_reward reward;
			reward.type = dbfile->Get(i, 3 + j * 4)->iValue;
			reward.value1 = dbfile->Get(i, 4 + j * 4)->iValue;
			reward.value2 = dbfile->Get(i, 5 + j * 4)->iValue;
			reward.value3 = dbfile->Get(i, 6 + j * 4)->iValue;

			if (reward.type != 0)
			{
				rd.rewards.push_back(reward);
			}
		}

		t_huodong_tanbao_rewards[type].push_back(rd);
	}
	return 0;
}

int HuodongConfig::parse_czfp()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_chongzhifanpai.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_fanpai fanpai;
		fanpai.id = dbfile->Get(i, 0)->iValue;
		fanpai.jewel = dbfile->Get(i, 2)->iValue;
		fanpai.rate = dbfile->Get(i, 3)->iValue;

		t_huodong_fanpai_[dbfile->Get(i, 1)->iValue].push_back(fanpai);
	}

	return 0;
}

int HuodongConfig::parse_szzp()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_zhuanpan.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_zhuanpan zp;
		zp.id = dbfile->Get(i, 0)->iValue;
		zp.reward.type = dbfile->Get(i, 2)->iValue;
		zp.reward.value1 = dbfile->Get(i, 3)->iValue;
		zp.reward.value2 = dbfile->Get(i, 4)->iValue;
		zp.reward.value3 = dbfile->Get(i, 5)->iValue;

		for (int j = 0; j < 4; ++j)
		{
			int jewel = dbfile->Get(i, 6 + j * 2)->iValue;
			int rate = dbfile->Get(i, 7 + j * 2)->iValue;
			zp.rates.push_back(std::make_pair(jewel, rate));
		}

		t_huodong_zhuanpan_[dbfile->Get(i, 1)->iValue].push_back(zp);
	}


	dbfile = game::scheme()->get_dbc("t_zhuanpan_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_zhuanpan_reward zp;
		zp.rank1 = dbfile->Get(i, 0)->iValue;
		zp.rank2 = dbfile->Get(i, 1)->iValue;

		for (int j = 0; j < 3; ++j)
		{
			if (dbfile->Get(i, 3 + j * 4)->iValue)
			{
				s_t_reward rd;
				rd.type = dbfile->Get(i, 3 + j * 4)->iValue;
				rd.value1 = dbfile->Get(i, 4 + j * 4)->iValue;
				rd.value2 = dbfile->Get(i, 5 + j * 4)->iValue;
				rd.value3 = dbfile->Get(i, 6 + j * 4)->iValue;
				zp.rewards.push_back(rd);
			}
		}

		t_zhuanpan_rewards_[dbfile->Get(i, 2)->iValue].push_back(zp);
	}

	return 0;
}

int HuodongConfig::parse_tansuo()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_manyou.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tansuo ts;
		ts.id = dbfile->Get(i, 0)->iValue;
		ts.color = dbfile->Get(i, 3)->iValue;
		ts.type = dbfile->Get(i, 4)->iValue;
		ts.value1 = dbfile->Get(i, 5)->iValue;
		ts.value2 = dbfile->Get(i, 6)->iValue;
		ts.value3 = dbfile->Get(i, 7)->iValue;
		ts.rate = dbfile->Get(i, 8)->iValue;

		t_tansuos_[ts.id] = ts;
	}

	dbfile = game::scheme()->get_dbc("t_manyou_qiyu.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tansuo_event te;
		te.id = dbfile->Get(i, 0)->iValue;
		te.type_id = dbfile->Get(i, 3)->iValue;
		te.def1 = dbfile->Get(i, 4)->iValue;
		te.def2 = dbfile->Get(i, 5)->iValue;
		te.def3 = dbfile->Get(i, 6)->iValue;
		if (te.type_id == 3002)
		{
			te.rate = te.def3;
		}
		else
		{
			te.rate = 100;
		}

		for (int j = 0; j < 3; ++j)
		{
			if (dbfile->Get(i, 7 + j * 4)->iValue)
			{
				s_t_reward rd;
				rd.type = dbfile->Get(i, 7 + j * 4)->iValue;
				rd.value1 = dbfile->Get(i, 8 + j * 4)->iValue;
				rd.value2 = dbfile->Get(i, 9 + j * 4)->iValue;
				rd.value3 = dbfile->Get(i, 10 + j * 4)->iValue;
				te.rds.push_back(rd);
			}
		}

		t_tansuo_events_[te.type_id][te.id] = te;
	}

	dbfile = game::scheme()->get_dbc("t_manyou_mubiao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tansuo_mubiao mu;
		mu.id = dbfile->Get(i, 0)->iValue;
		mu.point = dbfile->Get(i, 1)->iValue;
		mu.rd.type = dbfile->Get(i, 3)->iValue;
		mu.rd.value1 = dbfile->Get(i, 4)->iValue;
		mu.rd.value2 = dbfile->Get(i, 5)->iValue;
		mu.rd.value3 = dbfile->Get(i, 6)->iValue;

		t_tansuo_mubiaos_[mu.id] = mu;
	}

	dbfile = game::scheme()->get_dbc("t_manyou_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_tansuo_reward mu;
		mu.rank1 = dbfile->Get(i, 0)->iValue;
		mu.rank2 = dbfile->Get(i, 1)->iValue;

		for (int j = 0; j < 3; ++j)
		{
			if (dbfile->Get(i, 2 + j * 4)->iValue)
			{
				s_t_reward rd;
				rd.type = dbfile->Get(i, 2 + j * 4)->iValue;
				rd.value1 = dbfile->Get(i, 3 + j * 4)->iValue;
				rd.value2 = dbfile->Get(i, 4 + j * 4)->iValue;
				rd.value3 = dbfile->Get(i, 5 + j * 4)->iValue;
				mu.rds.push_back(rd);
			}
		}

		t_tansuo_rewards_[1].push_back(mu);
	}

	return 0;
}

int HuodongConfig::parse_mofang()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_mofang.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_mofang mf;
		mf.id = dbfile->Get(i, 0)->iValue;
		mf.mtype = dbfile->Get(i, 2)->iValue;
		mf.type = dbfile->Get(i, 3)->iValue;
		mf.value1 = dbfile->Get(i, 4)->iValue;
		mf.value2 = dbfile->Get(i, 5)->iValue;
		mf.value3 = dbfile->Get(i, 6)->iValue;
		mf.rate = dbfile->Get(i, 7)->iValue;

		t_mofang_reward_.push_back(mf);
	}

	dbfile = game::scheme()->get_dbc("t_mofang_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_mofang_target mt;
		mt.id = dbfile->Get(i, 0)->iValue;
		mt.point = dbfile->Get(i, 3)->iValue;
		mt.type = dbfile->Get(i, 4)->iValue;
		mt.value1 = dbfile->Get(i, 5)->iValue;
		mt.value2 = dbfile->Get(i, 6)->iValue;
		mt.value3 = dbfile->Get(i, 7)->iValue;

		t_mofang_target_[mt.id] = mt;
	}

	return 0;
}

int HuodongConfig::parse_yueka()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_yueka.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_yueka yk;
		yk.index = dbfile->Get(i, 0)->iValue;
		yk.type = dbfile->Get(i, 1)->iValue;
		yk.value1 = dbfile->Get(i, 2)->iValue;
		yk.value2 = dbfile->Get(i, 3)->iValue;
		yk.value3 = dbfile->Get(i, 4)->iValue;
		yk.type2 = dbfile->Get(i, 5)->iValue;
		yk.value4 = dbfile->Get(i, 6)->iValue;
		yk.value5 = dbfile->Get(i, 7)->iValue;
		yk.value6 = dbfile->Get(i, 8)->iValue;

		t_yueka_.push_back(yk);
	}
	return 0;
}
	
int HuodongConfig::parse_huigui()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_comeback.txt");
	if (!dbfile)
	{
		return -1;
	}
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		if (dbfile->Get(i, 3)->iValue == 1)
		{
			s_t_huigui_buzhu buzhu;
			buzhu.id = dbfile->Get(i, 0)->iValue;
			buzhu.tap = dbfile->Get(i, 3)->iValue;
			buzhu.def = dbfile->Get(i, 4)->iValue;
			buzhu.type1 = dbfile->Get(i, 8)->iValue;
			buzhu.value1 = dbfile->Get(i, 9)->iValue;
			buzhu.value2 = dbfile->Get(i, 10)->iValue;
			buzhu.value3 = dbfile->Get(i, 11)->iValue;
			buzhu.type2 = dbfile->Get(i, 12)->iValue;
			buzhu.value4 = dbfile->Get(i, 13)->iValue;
			buzhu.value5 = dbfile->Get(i, 14)->iValue;
			buzhu.value6 = dbfile->Get(i, 15)->iValue;
			buzhu.type3 = dbfile->Get(i, 16)->iValue;
			buzhu.value7 = dbfile->Get(i, 17)->iValue;
			buzhu.value8 = dbfile->Get(i, 18)->iValue;
			buzhu.value9 = dbfile->Get(i, 19)->iValue;

			t_huigui_buzhu_[buzhu.id] = buzhu;
		}
		if (dbfile->Get(i, 3)->iValue == 2)
		{
			s_t_huigui_fuli fuli;
			fuli.id = dbfile->Get(i, 0)->iValue;
			fuli.tap = dbfile->Get(i, 3)->iValue;
			fuli.def1 = dbfile->Get(i, 4)->iValue;
			fuli.def2 = dbfile->Get(i, 5)->iValue;
			fuli.def3 = dbfile->Get(i, 6)->iValue;
			fuli.def4 = dbfile->Get(i, 7)->iValue;
			fuli.type = dbfile->Get(i, 8)->iValue;
			fuli.value1 = dbfile->Get(i, 9)->iValue;
			fuli.value2 = dbfile->Get(i, 10)->iValue;
			fuli.value3 = dbfile->Get(i, 11)->iValue;

			t_huigui_fuli_[fuli.id] = fuli;
		}
		if (dbfile->Get(i, 3)->iValue == 3)
		{
			s_t_huigui_haoli haoli;
			haoli.id = dbfile->Get(i, 0)->iValue;
			haoli.tap = dbfile->Get(i, 3)->iValue;
			haoli.def = dbfile->Get(i, 4)->iValue;
			haoli.type1 = dbfile->Get(i, 8)->iValue;
			haoli.value1 = dbfile->Get(i, 9)->iValue;
			haoli.value2 = dbfile->Get(i, 10)->iValue;
			haoli.value3 = dbfile->Get(i, 11)->iValue;
			haoli.type2 = dbfile->Get(i, 12)->iValue;
			haoli.value4 = dbfile->Get(i, 13)->iValue;
			haoli.value5 = dbfile->Get(i, 14)->iValue;
			haoli.value6 = dbfile->Get(i, 15)->iValue;

			t_huigui_haoli_[haoli.id] = haoli;
		}
	}
	return 0;
}		

const s_t_huodong_pttq * HuodongConfig::get_t_huodong_pttq(int id) const
{
	std::map<int, s_t_huodong_pttq>::const_iterator it = t_huodong_pttq_.find(id);
	if (it == t_huodong_pttq_.end())
	{
		return 0;
	}
	return &(it->second);
}


const s_t_huodong_pttq *HuodongConfig::get_t_huodong_pttq_by_vip(int vip) const
{
	if (vip < get_t_huodong_pttq_min_vip())
	{
		return 0;
	}

	for (std::map<int, s_t_huodong_pttq>::const_iterator it = t_huodong_pttq_.begin();
		it != t_huodong_pttq_.end();
		++it)
	{
		if (it->second.gvip == vip)
		{
			return &(it->second);
		}
	}

	return 0;
}

const std::map<int, s_t_huodong_pttq>& HuodongConfig::get_t_all_huodong_pttq() const
{
	return t_huodong_pttq_;
}

void HuodongConfig::get_t_huodong_all_kaifu_xg(std::vector<std::pair<int, int> > &xgs) const
{
	for (std::map<int, s_t_huodong_kf>::const_iterator it = t_huodong_kf_.begin();
		it != t_huodong_kf_.end();
		++it)
	{
		xgs.push_back(std::make_pair(it->second.xg_id, it->second.count));
	}
}

const s_t_huodong_kf * HuodongConfig::get_t_huodong_kaifu_xg(int day) const
{
	std::map<int, s_t_huodong_kf>::const_iterator it = t_huodong_kf_.find(day);
	if (it == t_huodong_kf_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_huodong_kf * HuodongConfig::get_t_huodong_kf(int id) const
{
	for (std::map<int, s_t_huodong_kf>::const_iterator it = t_huodong_kf_.begin();
		it != t_huodong_kf_.end();
		++it)
	{
		if (it->second.xg_id == id)
		{
			return &(it->second);
		}
	}

	return 0;
}

void HuodongConfig::get_t_huodong_kf(int day, std::vector<int> &ids) const
{
	std::map<int, s_t_huodong_kf>::const_iterator it = t_huodong_kf_.find(day);
	if (it == t_huodong_kf_.end())
	{
		return;
	}

	ids.insert(ids.end(), it->second.ids.begin(), it->second.ids.end());
}


const s_t_huodong_kf_mubiao * HuodongConfig::get_t_huodong_kf_mubiao(int id) const
{
	std::map<int, s_t_huodong_kf_mubiao>::const_iterator it = t_huodong_kf_target_.find(id);
	if (it == t_huodong_kf_target_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_huodong_czjh *HuodongConfig::get_t_huodong_czjh(int index) const
{
	std::map<int, s_t_huodong_czjh>::const_iterator it = t_huodong_czjh_.find(index);
	if (it == t_huodong_czjh_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_huodong_czjhrs *HuodongConfig::get_t_huodong_czjhrs(int index) const
{
	std::map<int, s_t_huodong_czjhrs>::const_iterator it = t_huodong_czjh_count_.find(index);
	if (it == t_huodong_czjh_count_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_huodong_vip_libao *HuodongConfig::get_t_huodong_vip_libao(int vip) const
{
	if (vip < 0 || vip >= t_huodong_vip_libao_.size())
	{
		return 0;
	}
	return &(t_huodong_vip_libao_[vip]);
}

const s_t_huodong_week_libao* HuodongConfig::get_t_huodong_week_libao(int id) const
{
	std::map<int, s_t_huodong_week_libao>::const_iterator it = t_huodong_week_libao_.find(id);
	if (it == t_huodong_week_libao_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_tanbao* HuodongConfig::get_t_tanbao(int gezi) const
{
	std::map<int, s_t_tanbao>::const_iterator it = t_huodong_tanbao_.find(gezi);
	if (it == t_huodong_tanbao_.end())
	{
		return 0;
	}
	return &(it->second);
}

int HuodongConfig::get_t_tanbao_event(const s_t_tanbao* tanbao) const
{
	int sum = 0;
	for (int i = 0; i < tanbao->events.size(); ++i)
	{
		sum += tanbao->events[i].rate;
	}

	if (sum == 0)
	{
		return -1;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;

	for (int i = 0; i < tanbao->events.size(); ++i)
	{
		gl += tanbao->events[i].rate;
		if (gl > rate)
		{
			return i;
		}
	}

	return -1;
}

const s_t_tanbao_shop* HuodongConfig::get_t_tanbao_shop(int id) const
{
	std::map<int, s_t_tanbao_shop>::const_iterator it = t_huodong_tanbao_shop_.find(id);
	if (it == t_huodong_tanbao_shop_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_tanbao_mubiao* HuodongConfig::get_t_tanbao_mubiao(int id) const
{
	std::map<int, s_t_tanbao_mubiao>::const_iterator it = t_huodong_tanbao_mubiao_.find(id);
	if (it == t_huodong_tanbao_mubiao_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_tanbao_reward* HuodongConfig::get_t_tanbao_reward(int rank, int type) const
{
	std::map<int, std::vector<s_t_tanbao_reward> >::const_iterator it = t_huodong_tanbao_rewards.find(type);
	if (it == t_huodong_tanbao_rewards.end())
	{
		return 0;
	}

	const std::vector<s_t_tanbao_reward>& rds = it->second;
	for (int i = 0; i < rds.size(); ++i)
	{
		if (rank >= rds[i].rank1 &&
			rank <= rds[i].rank2)
		{
			return &(rds[i]);
		}
	}
	return 0;
}

const s_t_fanpai* HuodongConfig::get_t_fanpai(int type, const std::set<int>& ids) const
{
	std::map<int, std::vector<s_t_fanpai> >::const_iterator it = t_huodong_fanpai_.find(type);
	if (it == t_huodong_fanpai_.end())
	{
		return 0;
	}

	const std::vector<s_t_fanpai> &fs = it->second;
	int sum = 0;
	for (int i = 0; i < fs.size(); ++i)
	{
		if (ids.find(fs[i].id) == ids.end())
		{
			sum += fs[i].rate;
		}
	}
	if (sum == 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (int i = 0; i < fs.size(); ++i)
	{
		if (ids.find(fs[i].id) == ids.end())
		{
			gl += fs[i].rate;
			if (gl > rate)
			{
				return &(fs[i]);
			}
		}
	}
	return 0;
}

const s_t_zhuanpan * HuodongConfig::get_t_zhuanpan(int type, int jewel) const
{
	std::map<int, std::vector<s_t_zhuanpan> >::const_iterator it = t_huodong_zhuanpan_.find(type);
	if (it == t_huodong_zhuanpan_.end())
	{
		return 0;
	}

	const std::vector<s_t_zhuanpan>& zp = it->second;

	int index = -1;
	for (int i = zp.size() - 1; i >= 0; i--)
	{
		if (zp[i].reward.type == 1 &&
			zp[i].reward.value1 == 2)
		{
			for (int j = 0; j < zp[i].rates.size(); ++j)
			{
				if (jewel >= zp[i].rates[j].first)
				{
					index = j;
				}
			}
			break;
		}
	}

	if (index == -1)
	{
		return 0;
	}

	int sum = 0;
	for (int i = 0; i < zp.size(); ++i)
	{
		sum += zp[i].rates[index].second;
	}

	if (sum == 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (int i = 0; i < zp.size(); ++i)
	{
		gl += zp[i].rates[index].second;
		if (gl > rate)
		{
			return &(zp[i]);
		}
	}

	return 0;
}

const s_t_zhuanpan_reward* HuodongConfig::get_t_zhuanpan_reward(int rank, int type) const
{
	std::map<int, std::vector<s_t_zhuanpan_reward> >::const_iterator it = t_zhuanpan_rewards_.find(type);
	if (it == t_zhuanpan_rewards_.end())
	{
		return 0;
	}

	const std::vector<s_t_zhuanpan_reward>& zp = it->second;
	for (int i = 0; i < zp.size(); ++i)
	{
		if (rank >= zp[i].rank1 &&
			rank <= zp[i].rank2)
		{
			return &(zp[i]);
		}
	}

	return 0;
}

const s_t_tansuo* HuodongConfig::get_t_random_tansuo() const
{
	int sum = 0;
	for (std::map<int, s_t_tansuo>::const_iterator it = t_tansuos_.begin();
		it != t_tansuos_.end();
		++it)
	{
		sum += it->second.rate;
	}

	if (sum <= 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::map<int, s_t_tansuo>::const_iterator it = t_tansuos_.begin();
		it != t_tansuos_.end();
		++it)
	{
		gl += it->second.rate;
		if (gl > rate)
		{
			return &(it->second);
		}
	}

	return 0;
}

const s_t_tansuo_event* HuodongConfig::get_t_tansuo_random_event(int tid, int type /* = -1 */, int qid /* = -1 */) const
{
	std::map<int, std::map<int, s_t_tansuo_event> >::const_iterator it = t_tansuo_events_.find(tid);
	if (it == t_tansuo_events_.end())
	{
		return 0;
	}

	const std::map<int, s_t_tansuo_event>& te = it->second;
	int sum = 0;
	for (std::map<int, s_t_tansuo_event>::const_iterator jt = te.begin();
		jt != te.end();
		++jt)
	{
		if (qid == -1 ||
			qid != jt->second.id)
		{
			if (type == -1 ||
				type == jt->second.def1)
			{
				sum += jt->second.rate;
			}
		}

	}

	if (sum <= 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::map<int, s_t_tansuo_event>::const_iterator jt = te.begin();
		jt != te.end();
		++jt)
	{
		if (qid == -1 ||
			qid != jt->second.id)
		{
			if (type == -1 ||
				type == jt->second.def1)
			{
				gl += jt->second.rate;
				if (gl > rate)
				{
					return &(jt->second);
				}
			}
		}

	}

	return 0;
}

const s_t_tansuo_event* HuodongConfig::get_t_tansuo_event(int tid, int qid) const
{
	std::map<int, std::map<int, s_t_tansuo_event> >::const_iterator it = t_tansuo_events_.find(tid);
	if (it == t_tansuo_events_.end())
	{
		return 0;
	}

	const std::map<int, s_t_tansuo_event>& te = it->second;
	std::map<int, s_t_tansuo_event>::const_iterator jt = te.find(qid);
	if (jt == te.end())
	{
		return 0;
	}

	return &(jt->second);
}

const s_t_tansuo_mubiao* HuodongConfig::get_t_tansuo_mubiao(int id) const
{
	std::map<int, s_t_tansuo_mubiao>::const_iterator it = t_tansuo_mubiaos_.find(id);
	if (it == t_tansuo_mubiaos_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_tansuo_reward* HuodongConfig::get_t_tansuo_reward(int rank, int type) const
{
	std::map<int, std::vector<s_t_tansuo_reward> >::const_iterator it = t_tansuo_rewards_.find(type);
	if (it == t_tansuo_rewards_.end())
	{
		return 0;
	}

	const std::vector<s_t_tansuo_reward> &tr = it->second;
	for (int i = 0; i < tr.size(); ++i)
	{
		if (rank >= tr[i].rank1 &&
			rank <= tr[i].rank2)
		{
			return &(tr[i]);
		}
	}

	return 0;
}

const s_t_mofang* HuodongConfig::get_t_mofang_refresh(int type) const
{
	std::vector<int> indexs;
	for (int i = 0; i < t_mofang_reward_.size(); ++i)
	{
		if (t_mofang_reward_[i].mtype == type)
		{
			indexs.push_back(i);
		}
	}

	if (indexs.empty())
	{
		return 0;
	}
	return &t_mofang_reward_[indexs[Utils::get_int32(0, indexs.size() - 1)]];
}

const s_t_mofang* HuodongConfig::get_t_mofang_random(const std::list<int>& ids) const
{
	int sum = 0;
	std::vector<int> indexs;
	for (std::list<int>::const_iterator it = ids.begin();
		it != ids.end();
		++it)
	{
		for (int i = 0; i < t_mofang_reward_.size(); ++i)
		{
			if (t_mofang_reward_[i].id == (*it))
			{
				indexs.push_back(i);
				sum += t_mofang_reward_[i].rate;
				break;
			}
		}
	}

	if (sum <= 0)
	{
		return 0;
	}

	int rate = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (int i = 0; i < indexs.size(); ++i)
	{
		gl += t_mofang_reward_[indexs[i]].rate;
		if (gl > rate)
		{
			return &t_mofang_reward_[indexs[i]];
		}
	}

	return 0;
}

const s_t_mofang* HuodongConfig::get_t_mofang(int id) const
{
	for (int i = 0; i < t_mofang_reward_.size(); ++i)
	{
		if (t_mofang_reward_[i].id == id)
		{
			return &t_mofang_reward_[i];
		}
	}

	return 0;
}

const s_t_mofang_target* HuodongConfig::get_t_mofang_target(int id) const
{
	std::map<int, s_t_mofang_target>::const_iterator it = t_mofang_target_.find(id);
	if (it == t_mofang_target_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_yueka* HuodongConfig::get_t_yueka(int day) const
{
	for (std::vector<s_t_yueka>::const_iterator it = t_yueka_.begin(); it != t_yueka_.end(); it++)
	{
		if (it->index == day)
		{
			return &(*it);
	 	}
	}
	return 0;
}

const s_t_huigui_buzhu* HuodongConfig::get_t_huigui_buzhu(int id) const
{
	std::map<int, s_t_huigui_buzhu>::const_iterator it = t_huigui_buzhu_.find(id);
	if (it == t_huigui_buzhu_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_huigui_fuli* HuodongConfig::get_t_huigui_fuli(int id) const
{
	std::map<int, s_t_huigui_fuli>::const_iterator it = t_huigui_fuli_.find(id);
	if (it == t_huigui_fuli_.end())
	{
		return 0;
	}

	return &(it->second);
}

const s_t_huigui_haoli* HuodongConfig::get_t_huigui_haoli(int id) const
{
	std::map<int, s_t_huigui_haoli>::const_iterator it = t_huigui_haoli_.find(id);
	if (it == t_huigui_haoli_.end())
	{
		return 0;
	}

	return &(it->second);
}

const std::map<int,s_t_huigui_fuli>& HuodongConfig::get_t_fuli_num() const
{
	//for (std::map<int, s_t_huigui_fuli>::const_iterator it = t_huigui_fuli_.begin(); it != t_huigui_fuli_.end(); ++it)
	//{
	//	 s_t_fuli_num fuli;
	//	fuli.id = it->second.id;
	//	fuli.num = it->second.def3;
	//	//t_fuli_num_[fuli.id] = fuli;
	//	t_fuli_num_.push_back(fuli);
	//}
	return t_huigui_fuli_;
}