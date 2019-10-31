#include "treasure_operation.h"
#include "treasure_config.h"
#include "treasure_list.h"
#include "role_config.h"
#include "utils.h"


void TreasureOperation::treasure_refresh_luck(dhc::player_t *player)
{
	dhc::treasure_t *treasure = 0;
	for (int i = 0; i < player->treasures_size(); ++i)
	{
		treasure = POOL_GET_TREASURE(player->treasures(i));
		if (treasure)
		{
			treasure->set_star_luck(0);
		}
	}
}

void TreasureOperation::treasure_refresh(dhc::player_t *player, bool login)
{
	if (login)
	{
		sTreasureList->reset_rob_player_list(player);
		sTreasureList->clear_rob_player_list(player);
		sTreasureList->remove_rob_suipian_list(player->guid());
	}
	else
	{
		sTreasureList->update_rob_player_list(player);
	}
}

void TreasureOperation::treasure_clear(dhc::player_t *player)
{
	sTreasureList->clear_rob_player_list(player);
}

void TreasureOperation::treasure_refresh_rate(dhc::treasure_t *treasure,
	const s_t_treasure *treasure_template)
{
	if (treasure_template->color >= 4)
	{
		int trate = 100;
		while (treasure->star_rates_size() < 20)
		{
			treasure->add_star_rates(Utils::get_int32(1, 100));
			trate = Utils::get_int32(0, 99);
			if (trate < 5)
				treasure->add_star_bjs(300);
			else if (trate < 15)
				treasure->add_star_bjs(200);
			else if (trate < 40)
				treasure->add_star_bjs(150);
			else
				treasure->add_star_bjs(100);
		}
	}
}


bool TreasureOperation::treasure_is_full(dhc::player_t *player)
{
	return player->treasures_size() >= TREASURE_SLOT_SIZE;
	/*int num = 0;
	dhc::treasure_t *treasure = 0;
	for (int i = 0; i < player->treasures_size(); ++i)
	{
	if (player->treasures(i) > 0)
	{
	treasure = POOL_GET_TREASURE(player->treasures(i));
	if (treasure && treasure->role_guid() == 0)
	{
	num++;
	}
	}
	}
	return num >= (TREASURE_SLOT_SIZE + player->treasure_kc_num() * TREASURE_EXPAND_SIZE);*/
}

bool TreasureOperation::treasure_is_full(dhc::player_t *player, int num)
{
	int total_num = 0;
	for (int i = 0; i < player->treasures_size(); ++i)
	{
		if (player->treasures(i) == 0)
		{
			total_num++;
		}
	}
	return num > total_num;
}

bool TreasureOperation::treasure_is_rob(dhc::player_t *player)
{
	return sTreasureList->has_role_suipian_list(player);
}


dhc::treasure_t *TreasureOperation::treasure_create(dhc::player_t *player, 
	uint32_t template_id, 
	int enhance, 
	int jilian, 
	int mode)
{
	const s_t_treasure * t_treasure = sTreasureConfig->get_treasure(template_id);
	if (!t_treasure)
	{
		return 0;
	}

	return treasure_create(player, t_treasure, enhance, jilian, mode);
}

dhc::treasure_t * TreasureOperation::treasure_create(dhc::player_t *player,
	const s_t_treasure *treasure_template,
	int enhance,
	int jinlian,
	int mode)
{
	dhc::treasure_t *treasure = new dhc::treasure_t();
	treasure->set_guid(game::gtool()->assign(et_treasure));
	treasure->set_player_guid(player->guid());
	treasure->set_template_id(treasure_template->id);
	treasure->set_role_guid(0);
	treasure->set_enhance(enhance);
	treasure->set_jilian(jinlian);
	player->add_treasures(treasure->guid());

	treasure_refresh_rate(treasure, treasure_template);

	POOL_ADD_NEW(treasure->guid(), treasure);

	LOG_OUTPUT(player, LOG_TREASURE, treasure_template->id, 1, mode, LOG_ADD);

	return treasure;
}


void TreasureOperation::treasure_destroy(dhc::player_t *player, 
	dhc::treasure_t *treasure, 
	int mode)
{
	int template_id = treasure->template_id();
	for (int i = 0; i < player->treasures_size(); ++i)
	{
		if (player->treasures(i) == treasure->guid())
		{
			for (int j = i; j < player->treasures_size() - 1; ++j)
			{
				player->set_treasures(j, player->treasures(j + 1));
			}
			player->mutable_treasures()->RemoveLast();
			break;
		}
	}
	POOL_REMOVE(treasure->guid(), player->guid());
	LOG_OUTPUT(player, LOG_TREASURE, template_id, 1, mode, LOG_DEC);
}

void TreasureOperation::treasure_destroy(dhc::player_t *player, uint64_t treasure_guid, int mode)
{
	dhc::treasure_t * treasure = POOL_GET_TREASURE(treasure_guid);
	if (!treasure)
	{
		return;
	}

	treasure_destroy(player, treasure, mode);
}


int TreasureOperation::get_treasure_jinlian_count(dhc::player_t *player, int jinlian)
{
	int count = player->zhenxing_size();
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->treasures_size(); ++j)
			{
				dhc::treasure_t *treasure = POOL_GET_TREASURE(role->treasures(j));
				if (treasure)
				{
					/// 没有达到条件
					if (treasure->jilian() < jinlian)
					{
						count--;
						break;
					}
				}
				/// 没有宝物
				else
				{
					count--;
					break;
				}
			}
		}
		/// 没有角色
		else
		{
			count--;
		}
	}
	return count;
}

int TreasureOperation::get_treasure_jinlian_max(dhc::player_t *player, int jinlian)
{
	int jinlian_max = 0;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->treasures_size(); ++j)
			{
				dhc::treasure_t *treasure = POOL_GET_TREASURE(role->treasures(j));
				if (treasure && treasure->jilian() > jinlian_max)
				{
					jinlian_max = treasure->jilian();
				}
			}
		}
	}
	return jinlian_max;
}

int TreasureOperation::get_treasure_hecheng_count(dhc::player_t *player, int color)
{
	int count = 0;
	const s_t_treasure *t_treasure = 0;
	for (int i = 0; i < player->treasure_hechengs_size(); ++i)
	{
		t_treasure = sTreasureConfig->get_treasure(player->treasure_hechengs(i));
		if (t_treasure &&
			t_treasure->color >= color)
		{
			count++;
		}
	}
	return count;
}

int TreasureOperation::get_treasure_max_enhance_level(dhc::player_t *player)
{
	int max_enhance_level = 0;
	dhc::treasure_t *treasure = 0;
	for (int i = 0; i < player->treasures_size(); ++i)
	{
		treasure = POOL_GET_TREASURE(player->treasures(i));
		if (treasure)
		{
			if (treasure->enhance() > max_enhance_level)
			{
				max_enhance_level = treasure->enhance();
			}
		}
	}

	return max_enhance_level;
}

int TreasureOperation::get_treasure_max_jinlian_level(dhc::player_t *player)
{
	int max_jinlian_level = 0;
	dhc::treasure_t *treasure = 0;
	for (int i = 0; i < player->treasures_size(); ++i)
	{
		treasure = POOL_GET_TREASURE(player->treasures(i));
		if (treasure)
		{
			if (treasure->jilian() > max_jinlian_level)
			{
				max_jinlian_level = treasure->jilian();
			}
		}
	}

	return max_jinlian_level + 1;
}

int TreasureOperation::get_treasure_enhance_exp(dhc::player_t *player, dhc::treasure_t *treasure)
{
	int exp = 0;
	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
	if (!treasure)
	{
		return -1;
	}
	exp += treasure->enhance_exp();
	exp += t_treasure->exp;

	int color = t_treasure->color - 2;
	const s_t_treasure_enhance *t_enhance = 0;
	for (int i = 1; i <= treasure->enhance(); ++i)
	{
		t_enhance = sTreasureConfig->get_enhance(i);
		if (!t_enhance)
		{
			return -1;
		}

		if (color < 0 || color > t_enhance->attrs.size() - 1)
		{
			return -1;
		}
		exp += t_enhance->attrs[color];
	}

	return exp;
}

int TreasureOperation::get_treasure_enhance_pure_exp(dhc::player_t *player, dhc::treasure_t *treasure)
{
	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
	if (!treasure)
	{
		return -1;
	}
	int exp = 0;
	exp += treasure->enhance_exp();

	int color = t_treasure->color - 2;
	const s_t_treasure_enhance *t_enhance = 0;
	for (int i = 1; i <= treasure->enhance(); ++i)
	{
		t_enhance = sTreasureConfig->get_enhance(i);
		if (!t_enhance)
		{
			return -1;
		}

		if (color < 0 || color > t_enhance->attrs.size() - 1)
		{
			return -1;
		}
		exp += t_enhance->attrs[color];
	}

	return exp;
}


void TreasureOperation::create_treasure_report(dhc::player_t *player, dhc::player_t *target, int suipian_id, int win)
{
	dhc::treasure_report_t *report = new dhc::treasure_report_t();
	report->set_guid(game::gtool()->assign(et_treasure_report));
	report->set_player_guid(player->guid());
	report->set_other_guid(target->guid());
	report->set_other_name(target->name());
	report->set_other_template(target->template_id());
	report->set_other_level(target->level());
	report->set_other_bf(target->bf());
	report->set_suipian_id(suipian_id);
	report->set_time(game::timer()->now());
	report->set_win(win);
	report->set_new_report(0);
	
	POOL_ADD_NEW(report->guid(), report);

	if (player->treasure_reports_size() >= 10)
	{
		POOL_REMOVE(player->treasure_reports(0), player->guid());
		for (int i = 0; i < 9; ++i)
		{
			player->set_treasure_reports(i, player->treasure_reports(i + 1));
		}
		player->mutable_treasure_reports()->RemoveLast();
	}
	player->add_treasure_reports(report->guid());
}

bool TreasureOperation::get_enhance_return(dhc::player_t *player, dhc::treasure_t *treasure, s_t_rewards& rds)
{
	rds.add_reward(1, resource::GOLD, treasure->enhance_counts());

	int zi_num = treasure->enhance_counts() / 10000;
	int item_num = zi_num / 5;
	zi_num = zi_num % 5;
	int lan_num = treasure->enhance_counts() % 10000 / 3000;
	
	if (zi_num > 0)
	{
		for (int i = 0; i < zi_num; ++i)
		{
			rds.add_reward(6, 13001, 0);
		}
	}
	if (lan_num > 0)
	{
		for (int i = 0; i < lan_num; ++i)
		{
			rds.add_reward(6, 12001, 0);
		}
	}
	if (item_num > 0)
	{
		rds.add_reward(2, 10010062, item_num);
	}

	return true;
}

bool TreasureOperation::get_jilian_return(dhc::player_t *player, dhc::treasure_t *treasure, s_t_rewards& rds)
{
	int bao_num = 0;
	int gold = 0;
	int item_num = 0;
	const s_t_treasure_jinlian *t_jinlian = 0;
	for (int i = 1; i <= treasure->jilian(); ++i)
	{
		t_jinlian = sTreasureConfig->get_jinlian(i);
		if (!t_jinlian)
		{
			return false;
		}
		gold += t_jinlian->gold;
		item_num += t_jinlian->item_num;
		bao_num += t_jinlian->baowu_num;
	}

	if (bao_num > 0)
	{
		for (int i = 0; i < bao_num; ++i)
		{
			rds.add_reward(6, treasure->template_id(), 0);
		}
	}

	if (gold> 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}

	if (item_num > 0)
	{
		rds.add_reward(2, 50100001, item_num);
	}

	return true;
}

bool TreasureOperation::get_star_return(dhc::player_t *player, dhc::treasure_t *treasure, s_t_rewards& rds)
{
	if (treasure->star_gold() > 0)
	{
		rds.add_reward(1, resource::GOLD, treasure->star_gold());
	}

	return true;
}
