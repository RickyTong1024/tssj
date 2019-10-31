#include "sport_operation.h"
#include "sport_pool.h"
#include "player_operation.h"
#include "utils.h"
#include "sport_config.h"
#include "post_operation.h"
#include "equip_operation.h"

#define SPORT_LEVEL 10

void SportOperation::refresh(dhc::player_t *player)
{
	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		return;
	}

	int rank = sSportPool->get_now_rank(player->guid());
	if (player->level() >= SPORT_LEVEL)
	{
		if (rank == 0)
		{
			sport_list->add_player_guid(player->guid());
			sport_list->add_player_template(player->template_id());
			sport_list->add_player_name(player->name());
			sport_list->add_player_level(player->level());
			sport_list->add_player_bat_eff(player->bf());
			sport_list->add_player_isnpc(0);
			sport_list->add_player_vip(player->vip());
			sport_list->add_player_achieve(player->dress_achieves_size());
			sport_list->add_player_chenghao(player->chenghao_on());
			sport_list->add_nalflag(player->nalflag());
			sSportPool->set_now_rank(player->guid(), sport_list->player_guid_size());
			player->set_max_rank(sport_list->player_guid_size());
		}
		else
		{
			sport_list->set_player_name(rank - 1, player->name());
			sport_list->set_player_level(rank - 1, player->level());
			sport_list->set_player_bat_eff(rank - 1, player->bf());
			sport_list->set_player_template(rank - 1, player->template_id());
			sport_list->set_player_vip(rank - 1, player->vip());
			sport_list->set_player_achieve(rank - 1, player->dress_achieves_size());
			sport_list->set_player_chenghao(rank - 1, player->chenghao_on());
			sport_list->set_nalflag(rank - 1, player->nalflag());
		}
	}
}

void SportOperation::get_rank(dhc::player_t *player, std::vector<int> &ranks)
{
	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		return;
	}
	int rank = sSportPool->get_now_rank(player->guid());
	if (rank <= 0)
	{
		return;
	}
	if (rank < 10)
	{
		for (int i = 0; i < rank - 1; ++i)
		{
			ranks.push_back(i + 1);
		}
		int count = 1;
		while (ranks.size() < 10)
		{
			ranks.push_back(rank + count);
			count++;
		}
	}
	else if (rank <= 20)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i);
		}
		for (int i = 1; i <= 2; ++i)
		{
			ranks.push_back(rank + 10 * i);
		}
	}
	else if (rank <= 100)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 2);
		}
		for (int i = 1; i <= 2; ++i)
		{
			ranks.push_back(rank + 10 * i);
		}
	}
	else if (rank <= 200)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 3);
		}
		for (int i = 1; i <= 2; ++i)
		{
			ranks.push_back(rank + 10 * i);
		}
	}
	else if (rank <= 300)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 5);
		}
		for (int i = 1; i <= 2; ++i)
		{
			ranks.push_back(rank + 10 * i);
		}
	}
	else if (rank <= 500)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 7);
		}
		for (int i = 1; i <= 2; ++i)
		{
			ranks.push_back(rank + 10 * i);
		}
	}
	else if (rank <= 1000)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 10);
		}
		for (int i = 1; i <= 2; ++i)
		{
			ranks.push_back(rank + 10 * i);
		}
	}
	else if (rank <= 2000)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 30);
		}
		for (int i = 1; i <= 2; ++i)
		{
			ranks.push_back(rank + 10 * i);
		}
	}
	else if (rank <= 3500)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 60);
		}
		for (int i = 1; i <= 2; ++i)
		{
			ranks.push_back(rank + 10 * i);
		}
	}
	else if (rank <= 5000)
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 80);
		}
		if ((rank + 20) <= sport_list->player_guid_size())
		{
			for (int i = 1; i <= 2; ++i)
			{
				ranks.push_back(rank + 10 * i);
			}
		}
		else if ((sport_list->player_guid_size() - rank) >= 2)
		{
			for (int i = 1; i <= 2; ++i)
			{
				ranks.push_back(rank + i);
			}
		}
		else
		{
			for (int i = 1; i <= 2; ++i)
			{
				ranks.push_back(rank - (3 - i));
			}
		}
	}
	else
	{
		for (int i = 8; i > 0; --i)
		{
			ranks.push_back(rank - i * 150);
		}
		if ((rank + 20) <= sport_list->player_guid_size())
		{
			for (int i = 1; i <= 2; ++i)
			{
				ranks.push_back(rank + 10 * i);
			}
		}
		else if ((sport_list->player_guid_size() - rank) >= 2)
		{
			for (int i = 1; i <= 2; ++i)
			{
				ranks.push_back(rank + i);
			}
		}
		else
		{
			for (int i = 1; i <= 2; ++i)
			{
				ranks.push_back(rank - (3 - i));
			}
		}
	}
}

dhc::sport_t * SportOperation::create_sport(dhc::player_t *player, uint64_t other_guid, std::string other_name, int type, int win, int rank)
{
	uint64_t sport_guid = game::gtool()->assign(et_sport);
	dhc::sport_t *sport = new dhc::sport_t;
	sport->set_guid(sport_guid);
	sport->set_player_guid(player->guid());
	sport->set_time(game::timer()->now());
	sport->set_type(type);
	sport->set_other_guid(other_guid);
	sport->set_other_name(other_name);
	sport->set_win(win);
	sport->set_rank(rank);

	POOL_ADD_NEW(sport->guid(), sport);

	if (player->sports_size() >= 3)
	{
		POOL_REMOVE(player->sports(0), player->guid());
		for (int i = 0; i < 2; ++i)
		{
			player->set_sports(i, player->sports(i + 1));
		}
		player->mutable_sports()->RemoveLast();
	}
	player->add_sports(sport_guid);

	return sport;
}

int SportOperation::player_max_rank(dhc::player_t *player, int rank)
{
	int prank = player->max_rank();
	player->set_max_rank(rank);
	return sSportConfig->get_pm_jewel(rank, prank);
}

int SportOperation::get_rank_type(dhc::player_t *player, int rank)
{
	if (rank <= 0)
	{
		return 5;
	}

	int type;
	if (rank >= 1 && rank <= 30)
	{
		type = 1;
	}
	else if (rank >= 31 && rank <= 100)
	{
		type = 2;
	}
	else if (rank >= 101 && rank <= 500)
	{
		type = 3;
	}
	else if (rank >= 501 && rank <= 1500)
	{
		type = 4;
	}
	else
	{
		type = 5;
	}

	return type;
}

int SportOperation::sport_can_get(dhc::player_t *player)
{
	int last_rank = sSportPool->get_last_rank(player->guid());
	uint64_t sport_list_guid1 = MAKE_GUID(et_sport_list, 1);
	dhc::sport_list_t *sport_list1 = POOL_GET_SPORT_LIST(sport_list_guid1);
	if (!sport_list1)
	{
		return 0;
	}
	int can_get = 1;
	if (last_rank == 0)
	{
		can_get = 0;
	}
	else if (sport_list1->player_level(last_rank - 1) == 0)
	{
		can_get = 0;
	}
	if (last_rank > 1000)
	{
		can_get = 0;
	}
	return can_get;
}
