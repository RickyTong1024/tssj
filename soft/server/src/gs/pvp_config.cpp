#include "pvp_config.h"
#include "dbc.h"


int PvpConfig::parse()
{
	if (parse_lieren_active() == -1)
	{
		return -1;
	}
	if (parse_lieren_reward() == -1)
	{
		return -1;
	}
	if (parse_bingyuan_chenghao() == -1)
	{
		return -1;
	}
	if (parse_bingyuan_reward() == -1)
	{
		return -1;
	}
	if (parse_ds() == -1)
	{
		return -1;
	}
	return 0;
}

int PvpConfig::parse_lieren_active()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_lieren_active.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_pvp_active active;
		active.id = dbfile->Get(i, 0)->iValue;
		active.num = dbfile->Get(i, 3)->iValue;
		active.lieren_point = dbfile->Get(i, 4)->iValue;

		t_pvp_actives_[active.id] = active;
	}

	return 0;
}

int PvpConfig::parse_lieren_reward()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_lieren_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_pvp_reward rd;
		rd.rank1 = dbfile->Get(i, 0)->iValue;
		rd.rank2 = dbfile->Get(i, 1)->iValue;
		
		for (int j = 0; j < 5; ++j)
		{
			if (dbfile->Get(i, 2 + j * 4)->iValue > 0)
			{
				s_t_reward reward;
				reward.type = dbfile->Get(i, 2 + j * 4)->iValue;
				reward.value1 = dbfile->Get(i, 3 + j * 4)->iValue;
				reward.value2 = dbfile->Get(i, 4 + j * 4)->iValue;
				reward.value3 = dbfile->Get(i, 5 + j * 4)->iValue;
				rd.rewards.push_back(reward);
			}
		}
		t_pvp_rewards_.push_back(rd);
	}
	return 0;
}

int PvpConfig::parse_bingyuan_chenghao()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_bingyuan_chenghao.txt");
	if (!dbfile)
	{
		return -1;
	}

	default_chenhao_ = 7;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_bingyuan_chenghao ch;
		ch.id = dbfile->Get(i, 0)->iValue;
		ch.rank = dbfile->Get(i, 2)->iValue;
		ch.point = dbfile->Get(i, 3)->iValue;
		ch.cid = dbfile->Get(i, 4)->iValue;
		default_chenhao_ = ch.id;
		t_bingyuan_chenghao_[ch.id] = ch;
		t_bingyuan_to_chenghao_[ch.cid] = ch.id;
	}

	return 0;
}

int PvpConfig::parse_bingyuan_reward()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_bingyuan_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_bingyuan_rank_reward rd;
		rd.rank1 = dbfile->Get(i, 0)->iValue;
		rd.rank2 = dbfile->Get(i, 1)->iValue;

		for (int j = 0; j < 2; ++j)
		{
			s_t_reward srd;
			srd.type = dbfile->Get(i, 2 + j * 4)->iValue;
			srd.value1 = dbfile->Get(i, 3 + j * 4)->iValue;
			srd.value2 = dbfile->Get(i, 4 + j * 4)->iValue;
			srd.value3 = dbfile->Get(i, 5 + j * 4)->iValue;
			rd.rds.push_back(srd);
		}

		t_bingyuan_reward_.push_back(rd);
	}

	return 0;
}

int PvpConfig::parse_ds()
{
	DBCFile* dbfile = game::scheme()->get_dbc("t_master_duanwei.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ds_duanwei dw;
		dw.id = dbfile->Get(i, 0)->iValue;
		dw.attr1 = dbfile->Get(i, 11)->iValue;
		dw.value1 = dbfile->Get(i, 12)->iValue;
		dw.attr2 = dbfile->Get(i,13)->iValue;
		dw.value2 = dbfile->Get(i, 14)->iValue;

		t_ds_duanwei_[dw.id] = dw;
	}

	dbfile = game::scheme()->get_dbc("t_master_target.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ds_target tg;
		tg.count = dbfile->Get(i, 3)->iValue;
		tg.rd.type = dbfile->Get(i, 4)->iValue;
		tg.rd.value1 = dbfile->Get(i, 5)->iValue;
		tg.rd.value2 = dbfile->Get(i, 6)->iValue;
		tg.rd.value3 = dbfile->Get(i, 7)->iValue;
		t_ds_target_[dbfile->Get(i, 0)->iValue] = tg;
	}

	dbfile = game::scheme()->get_dbc("t_master_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ds_reward rd;
		rd.rank1 = dbfile->Get(i, 0)->iValue;
		rd.rank2 = dbfile->Get(i, 1)->iValue;

		for (int j = 0; j < 2; ++j)
		{
			s_t_reward rds;
			rds.type = dbfile->Get(i, 2 + j * 4)->iValue;
			rds.value1 = dbfile->Get(i, 3 + j * 4)->iValue;
			rds.value2 = dbfile->Get(i, 4 + j * 4)->iValue;
			rds.value3 = dbfile->Get(i, 5 + j * 4)->iValue;
			if (rds.type != 0)
			{
				rd.rds.push_back(rds);
			}
		}

		t_ds_rewards_.push_back(rd);
	}

	return 0;
}

const s_t_pvp_active* PvpConfig::get_pvp_active(int id) const
{
	std::map<int, s_t_pvp_active>::const_iterator it = t_pvp_actives_.find(id);
	if (it == t_pvp_actives_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_pvp_reward* PvpConfig::get_pvp_reward(int rank) const
{
	for (int i = 0; i < t_pvp_rewards_.size(); ++i)
	{
		if (rank >= t_pvp_rewards_[i].rank1 &&
			rank <= t_pvp_rewards_[i].rank2)
		{
			return &(t_pvp_rewards_[i]);
		}
	}
	return 0;
}

const s_t_bingyuan_chenghao* PvpConfig::get_bingyuan_chenghao(int id) const
{
	std::map<int, s_t_bingyuan_chenghao>::const_iterator it = t_bingyuan_chenghao_.find(id);
	if (it == t_bingyuan_chenghao_.end())
	{
		return 0;
	}

	return &(it->second);
}

int PvpConfig::get_bingyuan_chenghao_by_chenghao(int id) const
{
	std::map<int, int>::const_iterator it = t_bingyuan_to_chenghao_.find(id);
	if (it == t_bingyuan_to_chenghao_.end())
	{
		return default_chenhao_;
	}
	return it->second;
}

const s_t_bingyuan_rank_reward* PvpConfig::get_bingyuan_reward(int rank) const
{
	for (int i = 0; i < t_bingyuan_reward_.size(); ++i)
	{
		if (rank >= t_bingyuan_reward_[i].rank1 &&
			rank <= t_bingyuan_reward_[i].rank2)
		{
			return &(t_bingyuan_reward_[i]);
		}
	}
	return 0;
}

int PvpConfig::get_chenghao(int rank, int point) const
{
	for (std::map<int, s_t_bingyuan_chenghao>::const_iterator it = t_bingyuan_chenghao_.begin();
		it != t_bingyuan_chenghao_.end();
		++it)
	{
		const s_t_bingyuan_chenghao& ch = it->second;
		if (point >= ch.point)
		{
			if (ch.rank > 0 && rank <= ch.rank)
			{
				return ch.id;
			}
			else if (ch.rank == 0)
			{
				return ch.id;
			}
		}
	}
	return default_chenhao_;
}

const s_t_ds_duanwei* PvpConfig::get_ds_duanwei(int id) const
{
	std::map<int, s_t_ds_duanwei>::const_iterator it = t_ds_duanwei_.find(id);
	if (it == t_ds_duanwei_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_ds_target* PvpConfig::get_ds_target(int id) const
{
	std::map<int, s_t_ds_target>::const_iterator it = t_ds_target_.find(id);
	if (it == t_ds_target_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_ds_reward* PvpConfig::get_ds_reward(int rank) const
{
	for (int i = 0; i < t_ds_rewards_.size(); i++)
	{
		if (rank >= t_ds_rewards_[i].rank1 &&
			rank <= t_ds_rewards_[i].rank2)
		{
			return &(t_ds_rewards_[i]);
		}
	}
	return 0;
}

