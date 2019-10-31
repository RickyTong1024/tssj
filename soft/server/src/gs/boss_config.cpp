#include "boss_config.h"
#include "dbc.h"

int BossConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_boss_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_boss_reward t_boss_reward;
		t_boss_reward.rank1 = dbfile->Get(i, 0)->iValue;
		t_boss_reward.rank2 = dbfile->Get(i, 1)->iValue;
		for (int j = 0; j < 2; ++j)
		{
			s_t_reward t_reward;
			t_reward.type = dbfile->Get(i, 2 + 4 * j)->iValue;
			t_reward.value1 = dbfile->Get(i, 3 + 4 * j)->iValue;
			t_reward.value2 = dbfile->Get(i, 4 + 4 * j)->iValue;
			t_reward.value3 = dbfile->Get(i, 5 + 4 * j)->iValue;
			if (j == 0)
			{
				t_boss_reward.max_rewards.push_back(t_reward);
			}
			else
			{
				t_boss_reward.top_rewards.push_back(t_reward);
			}
		}
		t_boss_reward_.push_back(t_boss_reward);
	}

	dbfile = game::scheme()->get_dbc("t_boss_active.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_boss_active active;
		active.id = dbfile->Get(i, 0)->iValue;
		active.type = dbfile->Get(i, 1)->iValue;
		active.count = dbfile->Get(i, 4)->iValue;
		active.reward.type = dbfile->Get(i, 5)->iValue;
		active.reward.value1 = dbfile->Get(i, 6)->iValue;
		active.reward.value2 = dbfile->Get(i, 7)->iValue;
		active.reward.value3 = dbfile->Get(i, 8)->iValue;
		active.jiacheng = dbfile->Get(i, 9)->iValue;

		t_boss_active_[active.id] = active;
	}

	dbfile = game::scheme()->get_dbc("t_boss_dw.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_boss_dw dw;
		dw.level1 = dbfile->Get(i, 0)->iValue;
		dw.level2 = dbfile->Get(i, 1)->iValue;
		dw.dw = dbfile->Get(i, 2)->iValue;
		dw.shanghai = dbfile->Get(i, 3)->iValue;
		t_boss_dw_.push_back(dw);
	}

	dbfile = game::scheme()->get_dbc("t_boss_hp.txt");
	if (!dbfile)
	{
		return -1;
	}

	t_boss_level_.first = 99999999;
	t_boss_level_.second = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_boss_hp hp;
		hp.level = dbfile->Get(i, 0)->iValue;
		hp.hp_base = dbfile->Get(i, 1)->dValue;
		hp.hp_inc = dbfile->Get(i, 2)->dValue;
		if (hp.level < t_boss_level_.first)
		{
			t_boss_level_.first = hp.level;
		}
		if (hp.level > t_boss_level_.second)
		{
			t_boss_level_.second = hp.level;
		}
		t_boss_hp_[hp.level] = hp;
	}

	return 0;
}

const s_t_boss_reward * BossConfig::get_boss_reward(int rank) const
{
	for (std::vector<s_t_boss_reward>::size_type i = 0;
		i < t_boss_reward_.size();
		++i)
	{
		if (rank >= t_boss_reward_[i].rank1 && rank <= t_boss_reward_[i].rank2)
		{
			return &(t_boss_reward_[i]);
		}
	}
	return 0;
}

const s_t_boss_active* BossConfig::get_boss_active(int id) const
{
	std::map<int, s_t_boss_active>::const_iterator it = t_boss_active_.find(id);
	if (it == t_boss_active_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_boss_dw* BossConfig::get_boss_dw(int level) const
{
	for (std::vector<s_t_boss_dw>::size_type i = 0;
		i < t_boss_dw_.size();
		++i)
	{
		const s_t_boss_dw &dw = t_boss_dw_[i];
		if (level >= dw.level1 &&
			level <= dw.level2)
		{
			return &(t_boss_dw_[i]);
		}
	}

	return 0;
}

const s_t_boss_hp* BossConfig::get_boss_hp(int level) const
{
	if (level < t_boss_level_.first)
	{
		level = t_boss_level_.first;
	}
	if (level > t_boss_level_.second)
	{
		level = t_boss_level_.second;
	}

	std::map<int, s_t_boss_hp>::const_iterator it = t_boss_hp_.find(level);
	if (it == t_boss_hp_.end())
	{
		return 0;
	}
	return &(it->second);
}

