#include "player_config.h"
#include "dbc.h"
#include "utils.h"
#include "role_operation.h"

int PlayerConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_exp.txt");
	if (!dbfile)
	{
		return -1;
	}

	int exp = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_exp t_exp;
		t_exp.level = dbfile->Get(i, 0)->iValue;
		t_exp.exp = dbfile->Get(i, 1)->iValue;
		t_exp.tili = dbfile->Get(i, 2)->iValue;
		t_exp.tili_recover = dbfile->Get(i, 3)->iValue;
		t_exp.role_lexp = dbfile->Get(i, 4)->iValue;
		t_exp.role_zexp = dbfile->Get(i, 5)->iValue;
		t_exp.role_jexp = dbfile->Get(i, 6)->iValue;
		t_exp.role_rexp = dbfile->Get(i, 7)->iValue;
		t_exp.yuanli = dbfile->Get(i, 8)->iValue;
		t_exp.zhanhun = dbfile->Get(i, 9)->iValue;
		t_exp.dxqhzz = dbfile->Get(i, 10)->iValue;
		t_exp.bywin = dbfile->Get(i, 11)->iValue;
		t_exp.bylose = dbfile->Get(i, 12)->iValue;
		t_exp.pet_lexp = dbfile->Get(i, 13)->iValue;
		t_exp.pet_zexp = dbfile->Get(i, 14)->iValue;
		t_exp.pet_jexp = dbfile->Get(i, 15)->iValue;
		t_exp.pet_rexp = dbfile->Get(i, 16)->iValue;
		t_exp.type = dbfile->Get(i, 25)->iValue;
		t_exp.value1 = dbfile->Get(i, 26)->iValue;
		t_exp.value2 = dbfile->Get(i, 27)->iValue;
		t_exp.value3 = dbfile->Get(i, 28)->iValue;
		t_exp.title = dbfile->Get(i, 29)->pString;
		t_exp.text = dbfile->Get(i, 30)->pString;

		t_exps_[t_exp.level] = t_exp;
	}

	max_level_ = dbfile->GetRecordsNum();

	dbfile = game::scheme()->get_dbc("t_target.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_task t_task;
		t_task.id = dbfile->Get(i, 0)->iValue;
		t_task.pid = dbfile->Get(i, 5)->iValue;
		t_task.type = dbfile->Get(i, 6)->iValue;
		t_task.num = dbfile->Get(i, 7)->iValue;
		t_task.def1 = dbfile->Get(i, 8)->iValue;
		t_task.def2 = dbfile->Get(i, 9)->iValue;
		t_task.reward.type = dbfile->Get(i, 10)->iValue;
		t_task.reward.value1 = dbfile->Get(i, 11)->iValue;
		t_task.reward.value2 = dbfile->Get(i, 12)->iValue;
		t_task.reward.value3 = dbfile->Get(i, 13)->iValue;

		t_tasks_[t_task.id] = t_task;
	}

	dbfile = game::scheme()->get_dbc("t_active.txt");
	if (!dbfile)
	{
		return -1;
	}

	max_score_ = 0;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_active t_active;
		t_active.id = dbfile->Get(i, 0)->iValue;
		t_active.num = dbfile->Get(i, 6)->iValue;
		t_active.reward.type = dbfile->Get(i, 7)->iValue;
		t_active.reward.value1 = dbfile->Get(i, 8)->iValue;
		t_active.reward.value2 = dbfile->Get(i, 9)->iValue;
		t_active.reward.value3 = dbfile->Get(i, 10)->iValue;
		t_active.score = dbfile->Get(i, 11)->iValue;
		t_active_[t_active.id] = t_active;

		max_score_ += t_active.score;
	}

	dbfile = game::scheme()->get_dbc("t_active_reward.txt");
	if (!dbfile)
	{
		return -1;
	}
	
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_active_reward t_active_reward;
		t_active_reward.id = dbfile->Get(i, 0)->iValue;
		t_active_reward.score = dbfile->Get(i, 1)->iValue;
		for (int j = 0; j < 4; ++j)
		{
			s_t_reward rd;
			rd.type = dbfile->Get(i, 2 + j * 4)->iValue;
			rd.value1 = dbfile->Get(i, 3 + j * 4)->iValue;
			rd.value2 = dbfile->Get(i, 4 + j * 4)->iValue;
			rd.value3 = dbfile->Get(i, 5 + j * 4)->iValue;
			if (rd.type != 0)
			{
				t_active_reward.rewards.push_back(rd);
			}
		}
		t_active_reward_[t_active_reward.id] = t_active_reward;
	}

	dbfile = game::scheme()->get_dbc("t_vip.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_vip t_vip;
		t_vip.level = dbfile->Get(i, 0)->iValue;
		t_vip.recharge = dbfile->Get(i, 1)->iValue;
		t_vip.jewel = dbfile->Get(i, 6)->iValue;
		for (int j = 0; j < 6; ++j)
		{
			s_t_reward t_reward;
			t_reward.type = dbfile->Get(i, 7 + j * 4)->iValue;
			t_reward.value1 = dbfile->Get(i, 8 + j * 4)->iValue;
			t_reward.value2 = dbfile->Get(i, 9 + j * 4)->iValue;
			t_reward.value3 = dbfile->Get(i, 10 + j * 4)->iValue;
			t_vip.reward.push_back(t_reward);
		}
		t_vip.add_tili = dbfile->Get(i, 32)->iValue;
		t_vip.dj_num = dbfile->Get(i, 33)->iValue;
		t_vip.jy_buy_num = dbfile->Get(i, 34)->iValue;
		t_vip.ttt_cz_num = dbfile->Get(i, 35)->iValue;
		t_vip.guild_mission = dbfile->Get(i, 36)->iValue;
		t_vip.shop_refresh = dbfile->Get(i, 37)->iValue;
		t_vip.pvp = dbfile->Get(i, 38)->iValue;
		t_vip.huiyi = dbfile->Get(i, 39)->iValue;
		t_vip.guild_fight = dbfile->Get(i, 41)->iValue;

		t_vips_[t_vip.level] = t_vip;
	}

	dbfile = game::scheme()->get_dbc("t_price.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_price t_price;
		t_price.cishu = dbfile->Get(i, 0)->iValue;
		t_price.dj = dbfile->Get(i, 1)->iValue;
		t_price.kc = dbfile->Get(i, 2)->iValue;
		t_price.jy = dbfile->Get(i, 3)->iValue;
		t_price.ttt_cz = dbfile->Get(i, 4)->iValue;
		t_price.hbb_refresh = dbfile->Get(i, 5)->iValue;
		t_price.guild_mission = dbfile->Get(i, 6)->iValue;
		t_price.change_name = dbfile->Get(i, 13)->iValue;
		t_price.pvp_num = dbfile->Get(i, 14)->iValue;
		t_price.bingyuan = dbfile->Get(i, 15)->iValue;
		t_price.tanbao = dbfile->Get(i, 16)->iValue;
		t_price.zhuanpan1 = dbfile->Get(i, 17)->iValue;
		t_price.zhuanpan2 = dbfile->Get(i, 18)->iValue;
		t_price.ts = dbfile->Get(i, 19)->iValue;
		t_price.ds = dbfile->Get(i, 20)->iValue;
		t_price.mofang = dbfile->Get(i, 21)->iValue;
		t_price.guild_fight = dbfile->Get(i, 22)->iValue;

		for (int j = 0; j < 6; ++j)
		{
			t_price.xgs.push_back(dbfile->Get(i, 7 + j * 1)->iValue);
		}

		t_prices_[t_price.cishu] = t_price;
	}
	max_cishu_ = dbfile->GetRecordsNum();

	dbfile = game::scheme()->get_dbc("t_recharge.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_recharge t_recharge;
		t_recharge.id = dbfile->Get(i, 0)->iValue;
		t_recharge.type = dbfile->Get(i, 5)->iValue;
		t_recharge.pid = dbfile->Get(i, 6)->iValue;
		t_recharge.vippt = dbfile->Get(i, 7)->iValue;
		t_recharge.jewel = dbfile->Get(i, 8)->iValue;
		t_recharge.ios_id = dbfile->Get(i, 10)->iValue;

		t_recharge_[t_recharge.id] = t_recharge;
	}

	dbfile = game::scheme()->get_dbc("t_first_recharge.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_reward t_reward;
		t_reward.type = dbfile->Get(i, 0)->iValue;
		t_reward.value1 = dbfile->Get(i, 1)->iValue;
		t_reward.value2 = dbfile->Get(i, 2)->iValue;
		t_reward.value3 = dbfile->Get(i, 3)->iValue;

		t_rewards_.push_back(t_reward);
	}

	dbfile = game::scheme()->get_dbc("t_online_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_online_reward t_online_reward;
		t_online_reward.time = dbfile->Get(i, 3)->iValue;
		for (int j = 0; j < 3; ++j)
		{
			s_t_reward t_reward;
			t_reward.type = dbfile->Get(i, 4 + 4 * j)->iValue;
			t_reward.value1 = dbfile->Get(i, 5 + 4 * j)->iValue;
			t_reward.value2 = dbfile->Get(i, 6 + 4 * j)->iValue;
			t_reward.value3 = dbfile->Get(i, 7 + 4 * j)->iValue;
			if (t_reward.type != 0)
			{
				t_online_reward.rewards.push_back(t_reward);
			}
		}

		t_online_reward_.push_back(t_online_reward);
	}

	dbfile = game::scheme()->get_dbc("t_daily_sign.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_daily_sign t_daily_sign;
		t_daily_sign.index = dbfile->Get(i, 0)->iValue;
		t_daily_sign.reward.type = dbfile->Get(i, 1)->iValue;
		t_daily_sign.reward.value1 = dbfile->Get(i, 2)->iValue;
		t_daily_sign.reward.value2 = dbfile->Get(i, 3)->iValue;
		t_daily_sign.reward.value3 = dbfile->Get(i, 4)->iValue;
		t_daily_sign.vip = dbfile->Get(i, 5)->iValue;
		t_daily_sign.reward1.type = dbfile->Get(i, 6)->iValue;
		t_daily_sign.reward1.value1 = dbfile->Get(i, 7)->iValue;
		t_daily_sign.reward1.value2 = dbfile->Get(i, 8)->iValue;
		t_daily_sign.reward1.value3 = dbfile->Get(i, 9)->iValue;
		t_daily_sign.vip1 = dbfile->Get(i, 10)->iValue;

		t_daily_sign_[t_daily_sign.index] = t_daily_sign;
	}

	dbfile = game::scheme()->get_dbc("t_chenghao.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_chenghao ch;
		ch.id = dbfile->Get(i, 0)->iValue;
		ch.type = dbfile->Get(i, 1)->iValue;
		
		for (int j = 0; j < 5; ++j)
		{
			if (dbfile->Get(i, 10 + j * 2)->iValue)
			{
				ch.attrs.push_back(std::make_pair(dbfile->Get(i, 10 + j * 2)->iValue,
					dbfile->Get(i, 11 + j * 2)->dValue));
			}
		}

		ch.day = dbfile->Get(i, 21)->iValue;

		t_chenghao_[ch.id] = ch;
	}

	dbfile = game::scheme()->get_dbc("t_taobao_recharge.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_taobao_recharge tr;
		tr.id = dbfile->Get(i, 0)->iValue;
		tr.subid = dbfile->Get(i, 1)->iValue;
		tr.num = dbfile->Get(i, 2)->iValue;

		t_taobao_recharge_[tr.id] = tr;
	}

	dbfile = game::scheme()->get_dbc("t_const.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		int value = dbfile->Get(i, 1)->iValue;
		t_const_.push_back(value);
	}

	dbfile = game::scheme()->get_dbc("t_create.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_create t_create;
		t_create.resource = dbfile->Get(i, 0)->iValue;
		t_create.value = dbfile->Get(i, 1)->iValue;
		t_creates_.push_back(t_create);
	}

	return 0;
}

s_t_exp * PlayerConfig::get_exp(int level)
{
	std::map<int, s_t_exp>::iterator it = t_exps_.find(level);
	if (it == t_exps_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_task * PlayerConfig::get_task(uint32_t task_id)
{
	std::map<uint32_t, s_t_task>::iterator it = t_tasks_.find(task_id);
	if (it == t_tasks_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_active * PlayerConfig::get_active(int id)
{
	std::map<int, s_t_active>::iterator it = t_active_.find(id);
	if (it == t_active_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_active_reward * PlayerConfig::get_active_reward(int id)
{
	std::map<int, s_t_active_reward>::iterator it = t_active_reward_.find(id);
	if (it != t_active_reward_.end())
	{
		return &(*it).second;
	}
	return 0;
}

s_t_vip * PlayerConfig::get_vip(int level)
{
	std::map<int, s_t_vip>::iterator it = t_vips_.find(level);
	if (it == t_vips_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_price * PlayerConfig::get_price(int cishu)
{
	std::map<int, s_t_price>::iterator it = t_prices_.find(cishu);
	if (it == t_prices_.end())
	{
		return &t_prices_[max_cishu_];
	}
	else
	{
		return &(*it).second;
	}
}

s_t_recharge * PlayerConfig::get_recharge(int id)
{
	std::map<int, s_t_recharge>::iterator it = t_recharge_.find(id);
	if (it == t_recharge_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

void PlayerConfig::get_first_recharge(std::vector<s_t_reward> & rewards)
{
	rewards = t_rewards_;
}

s_t_online_reward * PlayerConfig::get_online_reward(int index)
{
	if (index < 0 || index >= t_online_reward_.size())
	{
		return 0;
	}

	return &t_online_reward_[index];
}

s_t_daily_sign * PlayerConfig::get_daily_sign(int index)
{
	std::map<int, s_t_daily_sign>::iterator it = t_daily_sign_.find(index);
	if (it == t_daily_sign_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

int PlayerConfig::get_random_event(dhc::player_t *player)
{
	int sum = 0;
	for (std::map<int, s_t_random_event>::iterator it = t_random_event_.begin(); it != t_random_event_.end(); ++it)
	{
		s_t_random_event *t_random_event = &(*it).second;
		if (t_random_event->role_id != 0 && !RoleOperation::has_role(player, t_random_event->role_id))
		{
			continue;
		}
		sum += t_random_event->rate;
	}
	if (sum == 0)
	{
		return 0;
	}
	int gl = 0;
	int rands = Utils::get_int32(0, sum - 1);
	for (std::map<int, s_t_random_event>::iterator it = t_random_event_.begin(); it != t_random_event_.end(); ++it)
	{
		s_t_random_event *t_random_event = &(*it).second;
		if (t_random_event->role_id != 0 && !RoleOperation::has_role(player, t_random_event->role_id))
		{
			continue;
		}
		gl += t_random_event->rate;
		if (gl > rands)
		{
			return t_random_event->id;
		}
	}
	return 0;
}

s_t_random_event * PlayerConfig::get_random_event(int id)
{
	std::map<int, s_t_random_event>::iterator it = t_random_event_.find(id);
	if (it == t_random_event_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

const s_t_chenghao* PlayerConfig::get_chenghao(int id) const
{
	std::map<int, s_t_chenghao>::const_iterator it = t_chenghao_.find(id);
	if (it == t_chenghao_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_taobao_recharge* PlayerConfig::get_taobao_recharge(int id) const
{
	std::map<int, s_t_taobao_recharge>::const_iterator it = t_taobao_recharge_.find(id);
	if (it == t_taobao_recharge_.end())
	{
		return 0;
	}
	return &(it->second);
}

int PlayerConfig::get_const(int index)
{
	if (index < 0 || index >= t_const_.size())
	{
		return 0;
	}

	return t_const_[index];
}
