#include "sport_config.h"
#include "dbc.h"
#include "utils.h"

SportConfig::SportConfig()
{

}

SportConfig::~SportConfig()
{

}

int SportConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_sport_rank.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_sport_rank sr;
		sr.rank1 = dbfile->Get(i, 0)->iValue;
		sr.rank2 = dbfile->Get(i, 1)->iValue;
		sr.jjcpoint = dbfile->Get(i, 2)->iValue;
		sr.jewel = dbfile->Get(i, 3)->iValue;
		sr.pm_jewel = dbfile->Get(i, 4)->dValue;
		t_sport_rank_.push_back(sr);
	}

	dbfile = game::scheme()->get_dbc("t_name.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		std::string s = dbfile->Get(i, 0)->pString;
		if (s != "")
		{
			t_first_name_.push_back(s);
		}
		s = dbfile->Get(i, 1)->pString;
		if (s != "")
		{
			t_secend_name_.push_back(s);
		}
	}

	dbfile = game::scheme()->get_dbc("t_sport_npc.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_sport_npc npc;
		npc.rank1 = dbfile->Get(i, 0)->iValue;
		npc.rank2 = dbfile->Get(i, 1)->iValue;
		npc.level1 = dbfile->Get(i, 2)->iValue;
		npc.level2 = dbfile->Get(i, 3)->iValue;
		npc.bf1 = dbfile->Get(i, 4)->iValue;
		npc.bf2 = dbfile->Get(i, 5)->iValue;
		for (int j = 0; j < 6; ++j)
		{
			s_t_sport_npc_sub npc_sub;
			npc_sub.type = dbfile->Get(i, j * 4 + 6)->iValue;
			npc_sub.id = dbfile->Get(i, j * 4 + 7)->iValue;
			npc_sub.level = dbfile->Get(i, j * 4 + 8)->iValue;
			npc_sub.glevel = dbfile->Get(i, j * 4 + 9)->iValue;
			npc.npc_subs.push_back(npc_sub);
		}
		for (int j = 0; j < 24; ++j)
		{
			s_t_sport_npc_sub1 npc_sub1;
			npc_sub1.id = dbfile->Get(i, j * 2 + 30)->iValue;
			npc_sub1.enhance = dbfile->Get(i, j * 2 + 31)->iValue;
			npc.npc_subs1.push_back(npc_sub1);
		}
		t_sport_npc_.push_back(npc);
	}

	if (parse_sport_card() == -1)
	{
		return -1;
	}

	return 0;
}

int SportConfig::parse_sport_card()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_sport_card.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_sport_card card;
		card.id = dbfile->Get(i, 0)->iValue;
		card.level1 = dbfile->Get(i, 1)->iValue;
		card.level2 = dbfile->Get(i, 2)->iValue;
		card.reward.type = dbfile->Get(i, 4)->iValue;
		card.reward.value1 = dbfile->Get(i, 5)->iValue;
		card.reward.value2 = dbfile->Get(i, 6)->iValue;
		card.reward.value3 = dbfile->Get(i, 7)->iValue;
		card.weight = dbfile->Get(i, 8)->iValue;
		
		sport_cards_[card.id] = card;
	}

	return 0;
}

s_t_sport_rank * SportConfig::get_sport_rank(int rank)
{
	for (int i = 0; i < t_sport_rank_.size(); ++i)
	{
		if (rank >= t_sport_rank_[i].rank1 && rank <= t_sport_rank_[i].rank2)
		{
			return &t_sport_rank_[i];
		}
	}
	return 0;
}

std::string SportConfig::get_random_name()
{
	return t_first_name_[Utils::get_int32(0, t_first_name_.size() - 1)] + t_secend_name_[Utils::get_int32(0, t_secend_name_.size() - 1)];
}

s_t_sport_npc * SportConfig::get_sport_npc(int rank)
{
	for (int i = 0; i < t_sport_npc_.size(); ++i)
	{
		if (t_sport_npc_[i].rank1 <= rank && t_sport_npc_[i].rank2 >= rank)
		{
			return &t_sport_npc_[i];
		}
	}
	return 0;
}

int SportConfig::get_pm_jewel(int rank1, int rank2)
{
	int rr1 = rank1;
	int rr2 = rank2;
	double jewel = 0;
	for (int i = 0; i < t_sport_rank_.size(); ++i)
	{
		s_t_sport_rank &t_sport_rank = t_sport_rank_[i];
		int r1 = 0;
		int r2 = 0;
		bool flag = false;
		if (rr1 >= t_sport_rank_[i].rank1 && rr1 <= t_sport_rank_[i].rank2)
		{
			r1 = rr1;
			rr1 = t_sport_rank_[i].rank2 + 1;
		}
		else
		{
			continue;
		}
		if (rr2 >= t_sport_rank_[i].rank1 && rr2 <= t_sport_rank_[i].rank2)
		{
			r2 = rr2;
			flag = true;
		}
		else
		{
			r2 = t_sport_rank_[i].rank2;
		}
		jewel += (r2 - r1 + 1) * t_sport_rank_[i].pm_jewel;
		if (flag)
		{
			break;
		}
	}
	return int(jewel);
}

const s_t_sport_card *SportConfig::get_sport_shop_card(int id) const
{
	std::map<int, s_t_sport_card>::const_iterator it = sport_cards_.find(id);
	if (it == sport_cards_.end())
	{
		return 0;
	}

	return &(it->second);
}

int SportConfig::refresh_sport_card(int level, bool has_jieri)
{
	int sum = 0;
	for (std::map<int, s_t_sport_card>::const_iterator it = sport_cards_.begin();
		it != sport_cards_.end();
		++it)
	{
		const s_t_sport_card &card = it->second;
		if (level >= card.level1 && level <= card.level2)
		{
			if (!has_jieri &&
				(card.reward.value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
				card.reward.value1 == s_t_rewards::HUODONG_ITEM_ID2))
			{
				continue;
			}
			sum += card.weight;
		}
	}
	if (sum == 0)
	{
		return 0;
	}
	int gl = 0;
	int rate = Utils::get_int32(0, sum - 1);
	for (std::map<int, s_t_sport_card>::const_iterator it = sport_cards_.begin();
		it != sport_cards_.end();
		++it)
	{
		const s_t_sport_card &card = it->second;
		if (level >= card.level1 && level <= card.level2)
		{
			if (!has_jieri &&
				(card.reward.value1 == s_t_rewards::HUODONG_ITEM_ID1 ||
				card.reward.value1 == s_t_rewards::HUODONG_ITEM_ID2))
			{
				continue;
			}
			gl += card.weight;
			if (gl > rate)
			{
				return card.id;
			}
		}
	}
	return 0;
}
