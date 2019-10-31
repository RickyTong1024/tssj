#include "guild_config.h"
#include "dbc.h"
#include "item_def.h"
#include "utils.h"

int GuildConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_guild.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild t_item;
		t_item.level = dbfile->Get(i, 0)->iValue;
		t_item.exp = dbfile->Get(i, 1)->iValue;
		t_item.member_num = dbfile->Get(i, 2)->iValue;

		for (int j = 0; j < 5; ++j)
		{
			t_item.box_num.push_back(dbfile->Get(i, 3 + j * 1)->iValue);
		}

		t_guild_.push_back(t_item);
	}

	dbfile = game::scheme()->get_dbc("t_guild_icon.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_icon t_item;
		t_item.id = dbfile->Get(i, 0)->iValue;
		t_item.icon = dbfile->Get(i, 1)->pString;

		t_guild_icon_[t_item.id] = t_item;
	}

	dbfile = game::scheme()->get_dbc("t_guild_sign.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_sign t_item;
		t_item.id = dbfile->Get(i, 0)->iValue;
		t_item.gold = dbfile->Get(i, 2)->iValue;
		t_item.jewel = dbfile->Get(i, 3)->iValue;
		t_item.exp = dbfile->Get(i, 4)->iValue;
		t_item.contrubution = dbfile->Get(i, 5)->iValue;
		t_item.honor = dbfile->Get(i, 6)->iValue;

		t_guild_sign_[t_item.id] = t_item;
	}

	dbfile = game::scheme()->get_dbc("t_guild_mobai_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_mobai mb;
		mb.id = dbfile->Get(i, 0)->iValue;
		mb.honor = dbfile->Get(i, 1)->iValue;
		mb.rd.type = dbfile->Get(i, 2)->iValue;
		mb.rd.value1 = dbfile->Get(i, 3)->iValue;
		mb.rd.value2 = dbfile->Get(i, 4)->iValue;
		mb.rd.value3 = dbfile->Get(i, 5)->iValue;
		mb.jindu_jc = dbfile->Get(i, 6)->iValue;
		mb.value2_jc = dbfile->Get(i, 7)->iValue;

		t_guild_mobai_reward_[mb.id] = mb;
	}

	dbfile = game::scheme()->get_dbc("t_guild_mission.txt");
	if (!dbfile)
	{
		return -1;
	}
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_mission mission;
		mission.index = dbfile->Get(i, 0)->iValue;

		for (int j = 0; j < 4; ++j)
		{
			s_t_guild_guai guai;
			guai.boss_id = dbfile->Get(i, 3 + j * 2)->iValue;
			guai.monster_id = dbfile->Get(i, 4 + j * 2)->iValue;
			mission.guai.push_back(guai);
		}
		mission.min_contribution = dbfile->Get(i, 11)->iValue;
		mission.max_contribution = dbfile->Get(i, 12)->iValue;
		mission.hit_contribution = dbfile->Get(i, 13)->iValue;
		mission.guild_exp = dbfile->Get(i, 14)->iValue;
		mission.level = dbfile->Get(i, 55)->iValue;

		for (int j = 0; j < 4; ++j)
		{
			mission.rewards.push_back(std::vector<s_t_reward>());

			for (int k = 0; k < 5; ++k)
			{
				s_t_reward rd;
				rd.type = dbfile->Get(i, 15 + j * 7)->iValue;
				rd.value1 = dbfile->Get(i, 16 + j * 7)->iValue;
				rd.value2 = dbfile->Get(i, 17 + k + j * 7)->iValue;
				rd.value3 = 0;

				mission.rewards[j].push_back(rd);
			}
		}

		for (int j = 0; j < 3; ++j)
		{
			s_t_reward rd;
			rd.type = dbfile->Get(i, 43 + j * 4)->iValue;
			rd.value1 = dbfile->Get(i, 44 + j * 4)->iValue;
			rd.value2 = dbfile->Get(i, 45 + j * 4)->iValue;
			rd.value3 = dbfile->Get(i, 46 + j * 4)->iValue;
			if (rd.type != 0)
			{
				mission.first_rewards.push_back(rd);
			}
		}

		t_guild_mission_[mission.index] = mission;
	}

	dbfile = game::scheme()->get_dbc("t_guild_keji.txt");
	if (!dbfile)
	{
		return -1;
	}
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_skill keji;
		keji.id = dbfile->Get(i, 0)->iValue;
		keji.guild_level = dbfile->Get(i, 4)->iValue;
		keji.study_level = dbfile->Get(i, 5)->iValue;
		keji.att = dbfile->Get(i, 6)->iValue;
		keji.value = dbfile->Get(i, 7)->iValue;
		keji.exp = dbfile->Get(i, 8)->iValue;
		keji.exp_jiacheng = dbfile->Get(i, 9)->iValue;
		keji.contri = dbfile->Get(i, 10)->iValue;
		keji.contri_jiacheng = dbfile->Get(i, 11)->iValue;

		t_guild_skill_[keji.id] = keji;
	}

	dbfile = game::scheme()->get_dbc("t_hongbao_target.txt");
	if (!dbfile)
	{
		return -1;
	}
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guild_hongbao_target ht;
		ht.id = dbfile->Get(i, 0)->iValue;
		ht.type = dbfile->Get(i, 3)->iValue;
		ht.count = dbfile->Get(i, 4)->iValue;
		
		for (int j = 0; j < 3; ++j)
		{
			if (dbfile->Get(i, 5 + j * 4)->iValue)
			{
				s_t_reward rd;
				rd.type = dbfile->Get(i, 5 + j * 4)->iValue;
				rd.value1 = dbfile->Get(i, 6 + j * 4)->iValue;
				rd.value2 = dbfile->Get(i, 7 + j * 4)->iValue;
				rd.value3 = dbfile->Get(i, 8 + j * 4)->iValue;
				ht.rewards.push_back(rd);
			}
		}

		t_guild_hongbao_target_[ht.id] = ht;
	}

	dbfile = game::scheme()->get_dbc("t_guildfight.txt");
	if (!dbfile)
	{
		return -1;
	}
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guildfight gt;
		gt.id = dbfile->Get(i, 0)->iValue;
		gt.color = dbfile->Get(i, 3)->iValue;
		gt.def_num = dbfile->Get(i, 4)->iValue;
		gt.chengfang_point = dbfile->Get(i, 5)->iValue;
		gt.gongpuo_exp = dbfile->Get(i, 6)->iValue;
		gt.win_point = dbfile->Get(i, 7)->iValue;
		gt.lose_point = dbfile->Get(i, 8)->iValue;
		gt.win_gongxian = dbfile->Get(i, 9)->iValue;
		gt.lose_gongxian = dbfile->Get(i, 10)->iValue;
		gt.bf_rate = dbfile->Get(i, 11)->iValue;
		gt.defense_num = dbfile->Get(i, 12)->iValue;

		t_guildfight_[gt.id] = gt;
	}

	dbfile = game::scheme()->get_dbc("t_guildfight_reward.txt");
	if (!dbfile)
	{
		return -1;
	}
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guildfight_rankreward gt;
		gt.rank1 = dbfile->Get(i, 0)->iValue;
		gt.rank2 = dbfile->Get(i, 1)->iValue;
		
		for (int j = 0; j < 4; ++j)
		{
			s_t_reward rd;
			rd.type = dbfile->Get(i, 3 + j * 4)->iValue;
			rd.value1 = dbfile->Get(i, 4 + j * 4)->iValue;
			rd.value2 = dbfile->Get(i, 5 + j * 4)->iValue;
			rd.value3 = dbfile->Get(i, 6 + j * 4)->iValue;
			if (rd.type > 0)
			{
				gt.rewards.push_back(rd);
			}
		}
		
		t_guildfight_rank_reward_[dbfile->Get(i, 2)->iValue].push_back(gt);
	}

	dbfile = game::scheme()->get_dbc("t_guildfight_target.txt");
	if (!dbfile)
	{
		return -1;
	}
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_guildfight_target gt;
		gt.id = dbfile->Get(i, 0)->iValue;
		gt.gongpuo_type = dbfile->Get(i, 3)->iValue;
		gt.nums = dbfile->Get(i, 4)->iValue;
		gt.type = dbfile->Get(i, 5)->iValue;
		gt.value1 = dbfile->Get(i, 6)->iValue;
		gt.value2 = dbfile->Get(i, 7)->iValue;
		gt.value3 = dbfile->Get(i, 8)->iValue;

		t_guildfight_target_[gt.id] = gt;
	}

	return 0;
}

s_t_guild* GuildConfig::get_guild(int level)
{
	if (level < 1 || level > t_guild_.size())
	{
		return 0;
	}

	return &t_guild_[level - 1];
}

s_t_guild_icon* GuildConfig::get_guild_icon(uint32_t id)
{
	std::map<uint32_t, s_t_guild_icon>::iterator iter = t_guild_icon_.find(id);
	if (iter == t_guild_icon_.end())
	{
		return 0;
	}

	return &(iter->second);
}

s_t_guild_sign* GuildConfig::get_guild_sign(uint32_t id)
{
	std::map<uint32_t, s_t_guild_sign>::iterator iter = t_guild_sign_.find(id);
	if (iter == t_guild_sign_.end())
	{
		return 0;
	}

	return &(iter->second);
}

const s_t_guild_mobai* GuildConfig::get_guild_mobai(int id) const
{
	std::map<int, s_t_guild_mobai>::const_iterator it = t_guild_mobai_reward_.find(id);
	if (it == t_guild_mobai_reward_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guild_mission* GuildConfig::get_guild_mission(int ceng) const
{
	std::map<int, s_t_guild_mission>::const_iterator it = t_guild_mission_.find(ceng);
	if (it == t_guild_mission_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guild_skill* GuildConfig::get_guild_skill(int id) const
{
	std::map<int, s_t_guild_skill>::const_iterator it = t_guild_skill_.find(id);
	if (it == t_guild_skill_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guild_hongbao_target* GuildConfig::get_guild_hongbao_target(int id) const
{
	std::map<int, s_t_guild_hongbao_target>::const_iterator it = t_guild_hongbao_target_.find(id);
	if (it == t_guild_hongbao_target_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guildfight* GuildConfig::get_guildfight(int id) const
{
	std::map<int, s_t_guildfight>::const_iterator it = t_guildfight_.find(id);
	if (it == t_guildfight_.end())
	{
		return 0;
	}
	return &(it->second);
}

const std::map<int, s_t_guildfight>& GuildConfig::get_t_guildfight() const
{
	return t_guildfight_;
}

const s_t_guildfight_target* GuildConfig::get_guildfight_target(int id) const
{
	std::map<int, s_t_guildfight_target>::const_iterator it = t_guildfight_target_.find(id);
	if (it == t_guildfight_target_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_guildfight_rankreward* GuildConfig::get_guildfight_rank_reward(int type, int rank) const
{
	std::map<int, std::vector<s_t_guildfight_rankreward> >::const_iterator it = t_guildfight_rank_reward_.find(type);
	if (it == t_guildfight_rank_reward_.end())
	{
		return 0;
	}

	const std::vector<s_t_guildfight_rankreward>& rewards = it->second;
	for (int i = 0; i < rewards.size(); ++i)
	{
		if (rank >= rewards[i].rank1 &&
			rank <= rewards[i].rank2)
		{
			return &(rewards[i]);
		}
	}
	return 0;
}
