#include "mission_operation.h"
#include "mission_config.h"
#include "utils.h"

std::map<uint64_t, uint64_t> player_random_;

int MissionOperation::get_mission_cishu(dhc::player_t *player, int id)
{
	s_t_mission *t_mission = sMissionConfig->get_mission(id);
	if (!t_mission)
	{
		return 0;
	}
	if (t_mission->type > 3)
	{
		id = t_mission->type;
	}
	for (int i = 0; i < player->mission_cishu_ids_size(); ++i)
	{
		if (player->mission_cishu_ids(i) == id)
		{
			return player->mission_cishu(i);
		}
	}
	return 0;
}

void MissionOperation::add_mission_cishu(dhc::player_t *player, int id, int num)
{
	s_t_mission *t_mission = sMissionConfig->get_mission(id);
	if (!t_mission)
	{
		return;
	}
	if (t_mission->type > 3)
	{
		id = t_mission->type;
	}
	bool flag = false;
	for (int i = 0; i < player->mission_cishu_ids_size(); ++i)
	{
		if (player->mission_cishu_ids(i) == id)
		{
			flag = true;
			player->set_mission_cishu(i, player->mission_cishu(i) + num);
			break;
		}
	}
	if (!flag)
	{
		player->add_mission_cishu_ids(id);
		player->add_mission_cishu(num);
	}
}

int MissionOperation::add_mission_star(dhc::player_t *player, int id, int star)
{
	int star_add = 0;
	bool flag = false;
	for (int i = 0; i < player->mission_ids_size(); ++i)
	{
		if (player->mission_ids(i) == id)
		{
			flag = true;
			if (player->mission_star(i) < star)
			{
				star_add = star - player->mission_star(i);
				player->set_mission_star(i, star);
			}
			break;
		}
	}
	if (!flag)
	{
		star_add = star;
		player->add_mission_ids(id);
		player->add_mission_star(star);
	}
	return star_add;
}

bool MissionOperation::mission_can_jump(dhc::player_t *player, int id)
{
	for (int i = 0; i < player->mission_ids_size(); ++i)
	{
		if (player->mission_ids(i) == id)
		{
			return true;
		}
	}
	return false;
}

void MissionOperation::add_map_star(dhc::player_t *player, int id, int star)
{
	bool flag = false;
	for (int i = 0; i < player->map_ids_size(); ++i)
	{
		if (player->map_ids(i) == id)
		{
			flag = true;
			player->set_map_star(i, player->map_star(i) + star);
			break;
		}
	}
	if (!flag)
	{
		player->add_map_ids(id);
		player->add_map_star(star);
		player->add_map_reward_get(0);
	}
}


int MissionOperation::get_ttt_cur_mission_star(dhc::player_t *player)
{
	int star = 0;
	for (int i = 0; i < player->ttt_cur_stars_size(); ++i)
	{
		star += player->ttt_cur_stars(i);
	}
	return star;
}

bool MissionOperation::can_jskill(int index, int jlevel)
{
	if (index == 0 && jlevel < 1)
	{
		return false;
	}
	else if (index == 1 && jlevel < 3)
	{
		return false;
	}
	else if (index == 2 && jlevel < 6)
	{
		return false;
	}
	else if (index == 3 && jlevel < 9)
	{
		return false;
	}
	else if (index == 4 && jlevel < 12)
	{
		return false;
	}
	else if (index == 5 && jlevel < 9999999)
	{
		return false;
	}
	return true;
}

std::pair<int, double> MissionOperation::get_baoji()
{
	static int baoji[4] = { 10, 20, 20, 50 };

	int rate = Utils::get_int32(0, 99);
	int gl = 0;
	for (int i = 0; i < 4; ++i)
	{
		gl += baoji[i];
		if (gl > rate)
		{
			switch (i)
			{
			case 0:
				return std::make_pair(3, 2.0);
				break;
			case 1:
				return std::make_pair(2, 1.5);
				break;
			case 2:
				return std::make_pair(1, 1.25);
				break;
			case 3:
				return std::make_pair(0, 1.0);
				break;
			default:
				break;
			}
		}
	}
	return std::make_pair(0, 1.0);
}

int MissionOperation::get_mission_star(dhc::player_t *player, int type)
{
	const s_t_map *t_map = 0;
	int star = 0;
	int gettype = 0;
	for (int i = 0; i < player->map_ids_size(); ++i)
	{
		t_map = sMissionConfig->get_map(player->map_ids(i));
		if (!t_map)
		{
			continue;
		}
		if (t_map->id < 10000)
		{
			gettype = 0;
		}
		else
		{
			gettype = 1;
		}
		if (gettype == type)
		{
			star += player->map_star(i);
		}
	}
	return star;
}

void MissionOperation::refresh_qiyu(dhc::player_t *player, int last)
{
	sMissionConfig->get_yiqu_boss(player, last);
}