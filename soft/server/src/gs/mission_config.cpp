#include "mission_config.h"
#include "dbc.h"
#include "utils.h"

int MissionConfig::parse()
{
	DBCFile * dbfile = game::scheme()->get_dbc("t_mission.txt");
	if (!dbfile)
	{
		return -1;
	}

	std::map<int, int> qiyu_ids;
	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_mission t_mission;
		t_mission.id = dbfile->Get(i, 0)->iValue;
		t_mission.name = dbfile->Get(i, 2)->pString;
		t_mission.type = dbfile->Get(i, 4)->iValue;
		t_mission.map = dbfile->Get(i, 5)->iValue;
		t_mission.cjname = dbfile->Get(i, 6)->pString;
		t_mission.jslc = dbfile->Get(i, 8)->iValue;
		t_mission.lch = dbfile->Get(i, 9)->iValue;
		t_mission.jyjslc = dbfile->Get(i, 10)->iValue;
		t_mission.jylch = dbfile->Get(i, 11)->iValue;
		t_mission.tili = dbfile->Get(i, 12)->iValue;
		t_mission.cishu = dbfile->Get(i, 13)->iValue;
		t_mission.yj = dbfile->Get(i, 14)->iValue;
		t_mission.yjguan = dbfile->Get(i, 15)->iValue;
		t_mission.qyboss = dbfile->Get(i, 17)->iValue;
		t_mission.tkezhi = dbfile->Get(i, 53)->iValue;
		for (int j = 0; j < 2; ++j)
		{
			int dx = dbfile->Get(i, 16 + j * 6)->iValue;
			t_mission.dxs.push_back(dx);
			for (int k = 0; k < 5; ++k)
			{
				int id = dbfile->Get(i, 17 + j * 6 + k)->iValue;
				t_mission.monsters.push_back(id);
			}
		}
		for (int j = 0; j < 3; ++j)
		{
			s_t_mission_item tmi;
			tmi.reward.type = dbfile->Get(i, 28 + j * 5)->iValue;
			tmi.reward.value1 = dbfile->Get(i, 29 + j * 5)->iValue;
			tmi.reward.value2 = dbfile->Get(i, 30 + j * 5)->iValue;
			tmi.reward.value3 = dbfile->Get(i, 31 + j * 5)->iValue;
			tmi.rate = dbfile->Get(i, 32 + j * 5)->iValue;
			if (tmi.reward.type)
			{
				t_mission.tmi.push_back(tmi);
			}
		}

		if (t_mission.type == 2 && ((t_mission.jyjslc / 1000) != (t_mission.jylch / 1000)))
		{
			qiyu_ids[t_mission.map] = t_mission.id;
		}
		
		t_missions_[t_mission.id] = t_mission;
	}

	dbfile = game::scheme()->get_dbc("t_monster.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_monster t_monster;
		t_monster.id = dbfile->Get(i, 0)->iValue;
		t_monster.class_id = dbfile->Get(i, 1)->iValue;
		t_monster.level = dbfile->Get(i, 2)->iValue;
		t_monster.jlevel = dbfile->Get(i, 3)->iValue;
		t_monster.glevel = dbfile->Get(i, 4)->iValue;
		t_monster.pinzhi_skill = dbfile->Get(i, 5)->iValue;
		t_monster.jy_level = dbfile->Get(i, 6)->iValue;
		t_monster.skill_level = dbfile->Get(i, 7)->iValue;
		t_monster.hp = dbfile->Get(i, 8)->dValue;
		t_monster.gj = dbfile->Get(i, 9)->iValue;
		t_monster.wf = dbfile->Get(i, 10)->iValue;
		t_monster.mf = dbfile->Get(i, 11)->iValue;
		t_monster.sd = dbfile->Get(i, 12)->iValue;
		t_monster.bj = dbfile->Get(i, 13)->iValue;
		t_monster.gd = dbfile->Get(i, 14)->iValue;
		t_monster.wm = dbfile->Get(i, 15)->iValue;
		t_monster.mm = dbfile->Get(i, 16)->iValue;
		t_monster.mz = dbfile->Get(i, 17)->iValue;
		t_monster.sb = dbfile->Get(i, 18)->iValue;
		t_monster.kb = dbfile->Get(i, 19)->iValue;
		t_monster.ct = dbfile->Get(i, 20)->iValue;
		t_monster.zs = dbfile->Get(i, 21)->iValue;
		t_monster.js = dbfile->Get(i, 22)->iValue;
		t_monster.snl = dbfile->Get(i, 23)->iValue;
		
		t_monsters_[t_monster.id] = t_monster;
	}

	dbfile = game::scheme()->get_dbc("t_map.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_map t_map;
		t_map.id = dbfile->Get(i, 0)->iValue;
		t_map.qy_boss_guan = 0;
		if (qiyu_ids.find(t_map.id) != qiyu_ids.end())
		{
			t_map.qy_boss_guan = qiyu_ids[t_map.id];
		}
		for (int k = 0; k < 3; ++k)
		{
			s_t_star_reward star_reward;
			star_reward.star_num = dbfile->Get(i, 10 + k * 17)->iValue;
			for (int j = 0; j < 4; ++j)
			{
				s_t_reward reward;
				reward.type = dbfile->Get(i, 11 + k * 17 + j * 4)->iValue;
				reward.value1 = dbfile->Get(i, 12 + k * 17 + j * 4)->iValue;
				reward.value2 = dbfile->Get(i, 13 + k * 17 + j * 4)->iValue;
				reward.value3 = dbfile->Get(i, 14 + k * 17 + j * 4)->iValue;
				if (reward.type)
				{
					star_reward.rewards.push_back(reward);
				}
			}
			if (star_reward.star_num > 0)
			{
				t_map.star_rewards.push_back(star_reward);
			}
		}
		t_maps_[t_map.id] = t_map;
	}

	dbfile = game::scheme()->get_dbc("t_ttt.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ttt t_ttt;
		t_ttt.index = dbfile->Get(i, 0)->iValue;
		for (int j = 0; j < 3; ++j)
		{
			std::vector<int> mids;
			for (int k = 0; k < 5; ++k)
			{
				int mid = dbfile->Get(i, 1 + j * 8 + k)->iValue;
				mids.push_back(mid);
			}
			t_ttt.monster_ids.push_back(mids);
			int gold = dbfile->Get(i, 6 + j * 8)->iValue;
			int shi = dbfile->Get(i, 7 + j * 8)->iValue;
			int bf = dbfile->Get(i, 8 + j * 8)->iValue;
			t_ttt.mw_point.push_back(gold);
			t_ttt.shi.push_back(shi);
		}
		t_ttt.d_type = dbfile->Get(i, 25)->iValue;
		t_ttt.d_param = dbfile->Get(i, 26)->iValue;

		t_ttt_[t_ttt.index] = t_ttt;
	}

	dbfile = game::scheme()->get_dbc("t_ttt_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ttt_reward t_ttt_reward;
		t_ttt_reward.index = dbfile->Get(i, 0)->iValue;
		t_ttt_reward.rewards.resize(3);
		for (int j = 0; j < 3; ++j)
		{
			for (int k = 0; k < 3; ++k)
			{
				s_t_reward t_reward;
				t_reward.type = dbfile->Get(i, 1 + j * 12 + k * 4)->iValue;
				t_reward.value1 = dbfile->Get(i, 2 + j * 12 + k * 4)->iValue;
				t_reward.value2 = dbfile->Get(i, 3 + j * 12 + k * 4)->iValue;
				t_reward.value3 = dbfile->Get(i, 4 + j * 12 + k * 4)->iValue;
				if (t_reward.type != 0)
				{
					t_ttt_reward.rewards[j].push_back(t_reward);
				}
				
			}
		}

		t_ttt_rewards_[t_ttt_reward.index] = t_ttt_reward;
	}

	dbfile = game::scheme()->get_dbc("t_ttt_value.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ttt_value t_ttt_value;
		t_ttt_value.id = dbfile->Get(i, 0)->iValue;
		t_ttt_value.type = dbfile->Get(i, 1)->iValue;
		t_ttt_value.star = dbfile->Get(i, 2)->iValue;
		t_ttt_value.sxtype = dbfile->Get(i, 3)->iValue;
		t_ttt_value.sxvalue = dbfile->Get(i, 4)->iValue;

		t_ttt_values_[t_ttt_value.id] = t_ttt_value;
	}

	dbfile = game::scheme()->get_dbc("t_xjbz.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_xjbz t_xjbz;
		t_xjbz.site = dbfile->Get(i, 0)->iValue;
		t_xjbz.type = dbfile->Get(i, 1)->iValue;

		t_xjbzs_[t_xjbz.site] = t_xjbz;
	}

	dbfile = game::scheme()->get_dbc("t_mission_first_reward.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_mission_first fr;
		fr.id = dbfile->Get(i, 0)->iValue;
		
		for (int j = 0; j < 4; ++j)
		{
			s_t_reward rd;
			rd.type = dbfile->Get(i, 1 + j * 4)->iValue;
			rd.value1 = dbfile->Get(i, 2 + j * 4)->iValue;
			rd.value2 = dbfile->Get(i, 3 + j * 4)->iValue;
			rd.value3 = dbfile->Get(i, 4 + j * 4)->iValue;
			if (rd.type != 0)
			{
				fr.rewards.push_back(rd);
			}
		}

		t_mission_first_[fr.id] = fr;
	}

	dbfile = game::scheme()->get_dbc("t_yb.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_yb t_yb;
		t_yb.type = dbfile->Get(i, 0)->iValue;
		t_yb.name = dbfile->Get(i, 1)->pString;
		t_yb.time = dbfile->Get(i, 2)->iValue;
		t_yb.rate = dbfile->Get(i, 3)->iValue;
		t_yb.yuanli_bs = dbfile->Get(i, 4)->iValue;
		t_yb.yuanli_ybq_per = dbfile->Get(i, 5)->iValue;
		t_yb.yuanli_ybq_min_per = dbfile->Get(i, 6)->iValue;
		t_yb_[t_yb.type] = t_yb;
	}

	dbfile = game::scheme()->get_dbc("t_yb_gw.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_yb_gw t_yb_gw;
		t_yb_gw.index = dbfile->Get(i, 0)->iValue;
		t_yb_gw.gj = dbfile->Get(i, 1)->dValue;
		t_yb_gw.jewel = dbfile->Get(i, 2)->iValue;
		t_yb_gw_[t_yb_gw.index] = t_yb_gw;
	}

	dbfile = game::scheme()->get_dbc("t_ore.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_ore t_ore;
		t_ore.index = dbfile->Get(i, 0)->iValue;
		t_ore.monster_id = dbfile->Get(i, 1)->iValue;
		t_ore.level = dbfile->Get(i, 2)->iValue;
		t_ore.tili = dbfile->Get(i, 3)->iValue;
		t_ore.bd_gold = dbfile->Get(i, 4)->iValue;
		t_ore.hx_gold = dbfile->Get(i, 5)->iValue;
		t_ore.jl_num = dbfile->Get(i, 6)->iValue;
		t_ore.zj_num = dbfile->Get(i, 7)->iValue;
		t_ore.sz_num = dbfile->Get(i, 8)->iValue;

		t_ore_[t_ore.index] = t_ore;
	}

	dbfile = game::scheme()->get_dbc("t_qiyutiaozhan.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); ++i)
	{
		s_t_qingyu qiyu;
		qiyu.id = dbfile->Get(i, 0)->iValue;
		qiyu.zhuangzhi = dbfile->Get(i, 1)->iValue;
		qiyu.tili = dbfile->Get(i, 2)->iValue;
		t_qiyu_[qiyu.id] = qiyu;
	}

	return 0;
}

s_t_mission * MissionConfig::get_mission(uint32_t id)
{
	std::map<uint32_t, s_t_mission>::iterator it = t_missions_.find(id);
	if (it == t_missions_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_monster * MissionConfig::get_monster(int id)
{
	std::map<int, s_t_monster>::iterator it = t_monsters_.find(id);
	if (it == t_monsters_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_map * MissionConfig::get_map(uint32_t id)
{
	std::map<uint32_t, s_t_map>::iterator it = t_maps_.find(id);
	if (it == t_maps_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_ts * MissionConfig::get_ts(int id)
{
	std::map<int, s_t_ts>::iterator it = t_ts_.find(id);
	if (it == t_ts_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_ts * MissionConfig::get_random_ts(dhc::player_t *player)
{
	int sum = 0;
	for (std::map<int, s_t_ts>::iterator it = t_ts_.begin(); it != t_ts_.end(); ++it)
	{
		if ((*it).second.level1 <= player->level() && (*it).second.level2 >= player->level())
		{
			sum += (*it).second.rate;
		}
	}
	if (sum == 0)
	{
		return 0;
	}
	int r = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::map<int, s_t_ts>::iterator it = t_ts_.begin(); it != t_ts_.end(); ++it)
	{
		if ((*it).second.level1 <= player->level() && (*it).second.level2 >= player->level())
		{
			gl += (*it).second.rate;
			if (gl > r)
			{
				return &(*it).second;
			}
		}
	}
	return 0;
}

const s_t_ts * MissionConfig::get_random_ts_by_type(dhc::player_t *player, int type) const
{
	int sum = 0;
	for (std::map<int, s_t_ts>::const_iterator it = t_ts_.begin(); it != t_ts_.end(); ++it)
	{
		if ((*it).second.ts_type == type &&
			(*it).second.level1 <= player->level() && 
			(*it).second.level2 >= player->level())
		{
			sum += (*it).second.rate;
		}
	}
	if (sum == 0)
	{
		return 0;
	}

	int r = Utils::get_int32(0, sum - 1);
	int gl = 0;
	for (std::map<int, s_t_ts>::const_iterator it = t_ts_.begin(); it != t_ts_.end(); ++it)
	{
		if ((*it).second.ts_type == type &&
			(*it).second.level1 <= player->level() && 
			(*it).second.level2 >= player->level())
		{
			gl += (*it).second.rate;
			if (gl > r)
			{
				return &(*it).second;
			}
		}
	}
	return 0;
}

s_t_ttt * MissionConfig::get_ttt(int index)
{
	std::map<int, s_t_ttt>::iterator it = t_ttt_.find(index);
	if (it == t_ttt_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_ttt_reward * MissionConfig::get_ttt_reward(int index)
{
	std::map<int, s_t_ttt_reward>::iterator it = t_ttt_rewards_.find(index);
	if (it == t_ttt_rewards_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_ttt_value * MissionConfig::get_ttt_value(int id)
{
	std::map<int, s_t_ttt_value>::iterator it = t_ttt_values_.find(id);
	if (it == t_ttt_values_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

int MissionConfig::get_random_ttt_value(int type)
{
	std::vector<int> ids;
	for (std::map<int, s_t_ttt_value>::iterator it = t_ttt_values_.begin(); it != t_ttt_values_.end(); ++it)
	{
		s_t_ttt_value &t_ttt_value = (*it).second;
		if (t_ttt_value.type == type)
		{
			ids.push_back(t_ttt_value.id);
		}
	}
	if (ids.size() == 0)
	{
		return 0;
	}
	return ids[Utils::get_int32(0, ids.size() - 1)];
}

s_t_xjbz * MissionConfig::get_xjbz(int site)
{
	std::map<int, s_t_xjbz>::iterator it = t_xjbzs_.find(site);
	if (it == t_xjbzs_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_yb * MissionConfig::get_yb(int type)
{
	std::map<int, s_t_yb>::iterator it = t_yb_.find(type);
	if (it == t_yb_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_yb_gw * MissionConfig::get_yb_gw(int index)
{
	std::map<int, s_t_yb_gw>::iterator it = t_yb_gw_.find(index);
	if (it == t_yb_gw_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_ore * MissionConfig::get_t_ore(int index)
{
	std::map<int, s_t_ore>::iterator it = t_ore_.find(index);
	if (it == t_ore_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

s_t_mrt * MissionConfig::get_t_mrt(int id)
{
	std::map<int, s_t_mrt>::iterator it = t_mrt_.find(id);
	if (it == t_mrt_.end())
	{
		return 0;
	}
	else
	{
		return &(*it).second;
	}
}

const s_t_mission_first* MissionConfig::get_mission_first(int id) const
{
	std::map<int, s_t_mission_first>::const_iterator it = t_mission_first_.find(id);
	if (it == t_mission_first_.end())
	{
		return 0;
	}
	return &(it->second);
}

const s_t_qingyu* MissionConfig::get_qiyu(int id) const
{
	std::map<int, s_t_qingyu>::const_iterator it = t_qiyu_.find(id);
	if (it != t_qiyu_.end())
	{
		return &(it->second);
	}
	return 0;
}

void MissionConfig::get_yiqu_boss(dhc::player_t *player, int last)
{
	if (player->level() < 18)
	{
		return;
	}
	int count = 0;
	const s_t_map *t_map = 0;
	const s_t_mission *t_mission = 0;
	const s_t_monster *t_monster = 0;
	std::map<int, int > monster_missions;

	/// 已选中
	std::set<int> role_ids;
	for (int i = 0; i < player->qiyu_mission_size(); ++i)
	{
		t_mission = get_mission(player->qiyu_mission(i));
		if (t_mission)
		{
			t_monster = get_monster(t_mission->qyboss);
			if (t_monster)
			{
				role_ids.insert(t_monster->class_id);
			}
		}
	}

	/// 取boss最大关卡
	for (int i = player->map_ids_size() - 1; i >= 0; --i)
	{
		if (count >= 15)
		{
			break;
		}

		if (player->map_ids(i) < 10000)
		{
			continue;
		}

		t_map = get_map(player->map_ids(i));
		if (!t_map)
		{
			continue;
		}

		int star = 0;
		for (int k = 0; k < t_map->star_rewards.size(); ++k)
		{
			if (t_map->star_rewards[k].star_num > star)
			{
				star = t_map->star_rewards[k].star_num;
			}
		}
		if (player->map_star(i) >= star)
		{
			t_mission = get_mission(t_map->qy_boss_guan);
			if (t_mission)
			{
				t_monster = get_monster(t_mission->qyboss);
				if (t_monster)
				{
					count++;
					if (role_ids.find(t_monster->class_id) == role_ids.end())
					{
						std::map<int, int >::iterator it = monster_missions.find(t_monster->class_id);
						if (it == monster_missions.end())
						{
							monster_missions[t_monster->class_id] = t_mission->id;
						}
						else
						{
							if (t_mission->id > it->second)
							{
								it->second = t_mission->id;
							}
						}
					}
					
				}
			}
		}
	}

	int boss_num = 0;
	if (count <= 0)
	{
		boss_num = 0;
	}
	else if (count <= 3)
	{
		boss_num = 1;
	}
	else if (count <= 5)
	{
		boss_num = 2;
	}
	else if (count <= 7)
	{
		boss_num = 3;
	}
	else if (count <= 11)
	{
		boss_num = 4;
	}
	else
	{
		boss_num = 5;
	}

	int left_num = boss_num - player->qiyu_mission_size();
	if (left_num > monster_missions.size())
	{
		left_num = monster_missions.size();
	}
	if (left_num >= 1)
	{
		int chuxian_num = Utils::get_int32(1, 2);
		if (chuxian_num > left_num)
		{
			chuxian_num = left_num;
		}
		if (last == 2 && chuxian_num < left_num)
		{
			chuxian_num = left_num;
		}

		std::vector<int> mission_ids;
		std::vector<int> choose_ids;
		for (std::map<int, int >::iterator jt = monster_missions.begin();
			jt != monster_missions.end();
			++jt)
		{
			mission_ids.push_back(jt->second);
		}
		Utils::get_vector(mission_ids, chuxian_num, choose_ids);

		for (int i = 0; i < choose_ids.size(); ++i)
		{
			int rate = Utils::get_int32(0, 99);
			if (rate < 20)
			{
				player->add_qiyu_hard(2);
			}
			else if (rate < 50)
			{
				player->add_qiyu_hard(1);
			}
			else
			{
				player->add_qiyu_hard(0);
			}
			player->add_qiyu_mission(choose_ids[i]);
			player->add_qiyu_suc(0);
		}
	}
}
