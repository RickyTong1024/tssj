#include "rank_operation.h"
#include "rank_pool.h"
#include "social_operation.h"

#define MAX_RANK_PLAYER 60
#define RANK_LEVEL 10

static const int rank_nums[e_rank_type_end] = {
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	MAX_RANK_PLAYER,
	200,
	100,
	100,
	100,
	100,
	100,
	100,
	100
};

void RankOperation::refresh(dhc::player_t *p_player)
{
	if (p_player->level() < RANK_LEVEL)
	{
		return;
	}
	check_value(p_player, e_rank_type_level, p_player->level());
	check_value(p_player, e_rank_type_bf, p_player->bf());
	if (p_player->mission() > 0)
	{
		check_value(p_player, e_rank_type_pt, p_player->mission());
	}
	if (p_player->mission_jy() > 0)
	{
		check_value(p_player, e_rank_type_jy, p_player->mission_jy());
	}
	int num = 0;
	for (int i = 0; i < p_player->map_star_size(); ++i)
	{
		num += p_player->map_star(i);
	}
	if (num > 0)
	{
		check_value(p_player, e_rank_type_star, num);
	}
	num = 0;
	for (int i = 0; i < p_player->ttt_last_stars_size(); ++i)
	{
		num += p_player->ttt_last_stars(i);
	}
	if (num > 0)
	{
		check_value(p_player, e_rank_type_ttt, num);
	}

	if (p_player->huiyi_shoujidu() > 0)
	{
		check_value(p_player, e_rank_huiyi, p_player->huiyi_shoujidu());
	}
}

void RankOperation::del_player(uint64_t player_guid)
{
	for (int k = e_rank_type_level; k < e_rank_type_end; ++k)
	{
		uint64_t rank_guid = MAKE_GUID(et_rank, k);
		dhc::rank_t *rank = POOL_GET_RANK(rank_guid);
		if (!rank)
		{
			continue;
		}
		int index = -1;
		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			if (rank->player_guid(i) == player_guid)
			{
				index = i;
				break;
			}
		}
		if (index != -1)
		{
			for (int i = index; i < rank->player_guid_size() - 1; ++i)
			{
				rank->set_player_guid(i, rank->player_guid(i + 1));
				rank->set_player_name(i, rank->player_name(i + 1));
				rank->set_player_template(i, rank->player_template(i + 1));
				rank->set_player_level(i, rank->player_level(i + 1));
				rank->set_player_bf(i, rank->player_bf(i + 1));
				rank->set_value(i, rank->value(i + 1));
				rank->set_player_vip(i, rank->player_vip(i + 1));
				rank->set_player_achieve(i, rank->player_achieve(i + 1));
				rank->set_player_huiyi(i, rank->player_huiyi(i + 1));
				rank->set_player_chenghao(i, rank->player_chenghao(i + 1));
				rank->set_player_nalflag(i, rank->player_nalflag(i + 1));
			}
			rank->mutable_player_guid()->RemoveLast();
			rank->mutable_player_name()->RemoveLast();
			rank->mutable_player_template()->RemoveLast();
			rank->mutable_player_level()->RemoveLast();
			rank->mutable_player_bf()->RemoveLast();
			rank->mutable_value()->RemoveLast();
			rank->mutable_player_vip()->RemoveLast();
			rank->mutable_player_achieve()->RemoveLast();
			rank->mutable_player_huiyi()->RemoveLast();
			rank->mutable_player_chenghao()->RemoveLast();
			rank->mutable_player_nalflag()->RemoveLast();
		}
	}
}

void RankOperation::check_value(dhc::player_t *player, int num, int value)
{
	uint64_t rank_guid = MAKE_GUID(et_rank, num);
	dhc::rank_t *rank = POOL_GET_RANK(rank_guid);
	if (!rank)
	{
		return;
	}
	int index = -1;
	for (int i = 0; i < rank->player_guid_size(); ++i)
	{
		if (rank->player_guid(i) == player->guid())
		{
			if (rank->value(i) == value)
			{
				rank->set_player_name(i, player->name());
				rank->set_player_level(i, player->level());
				rank->set_player_bf(i, player->bf());
				rank->set_player_template(i, player->template_id());
				rank->set_player_vip(i, player->vip());
				rank->set_player_achieve(i, player->dress_achieves_size());
				rank->set_player_huiyi(i, player->huiyi_jihuos_size());
				rank->set_player_chenghao(i, player->chenghao_on());
				rank->set_player_nalflag(i,player->nalflag());
				return;
			}
			index = i;
			break;
		}
	}
	if (index != -1)
	{
		for (int i = index; i < rank->player_guid_size() - 1; ++i)
		{
			rank->set_player_guid(i, rank->player_guid(i + 1));
			rank->set_player_name(i, rank->player_name(i + 1));
			rank->set_player_template(i, rank->player_template(i + 1));
			rank->set_player_level(i, rank->player_level(i + 1));
			rank->set_player_bf(i, rank->player_bf(i + 1));
			rank->set_value(i, rank->value(i + 1));
			rank->set_player_vip(i, rank->player_vip(i + 1));
			rank->set_player_achieve(i, rank->player_achieve(i + 1));
			rank->set_player_huiyi(i, rank->player_huiyi(i + 1));
			rank->set_player_chenghao(i, rank->player_chenghao(i + 1));
			rank->set_player_nalflag(i, rank->player_nalflag(i + 1));
		}
	}
	else
	{
		rank->add_player_guid(player->guid());
		rank->add_player_name(player->name());
		rank->add_player_template(player->template_id());
		rank->add_player_level(player->level());
		rank->add_player_bf(player->bf());
		rank->add_value(value);
		rank->add_player_vip(player->vip());
		rank->add_player_achieve(player->dress_achieves_size());
		rank->add_player_huiyi(player->huiyi_jihuos_size());
		rank->add_player_chenghao(player->chenghao_on());
		rank->add_player_nalflag(player->nalflag());
	}

	index = -1;
	for (int i = rank->player_guid_size() - 2; i >= 0; --i)
	{
		if (rank->value(i) < value)
		{
			rank->set_player_guid(i + 1, rank->player_guid(i));
			rank->set_player_name(i + 1, rank->player_name(i));
			rank->set_player_template(i + 1, rank->player_template(i));
			rank->set_player_level(i + 1, rank->player_level(i));
			rank->set_player_bf(i + 1, rank->player_bf(i));
			rank->set_value(i + 1, rank->value(i));
			rank->set_player_vip(i + 1, rank->player_vip(i));
			rank->set_player_achieve(i + 1, rank->player_achieve(i));
			rank->set_player_huiyi(i + 1, rank->player_huiyi(i));
			rank->set_player_chenghao(i + 1, rank->player_chenghao(i));
			rank->set_player_nalflag(i + 1, rank->player_nalflag(i));
		}
		else
		{
			index = i;
			break;
		}
	}
	rank->set_player_guid(index  + 1, player->guid());
	rank->set_player_name(index + 1, player->name());
	rank->set_player_template(index + 1, player->template_id());
	rank->set_player_level(index + 1, player->level());
	rank->set_player_bf(index + 1, player->bf());
	rank->set_value(index + 1, value);
	rank->set_player_vip(index + 1, player->vip());
	rank->set_player_achieve(index + 1, player->dress_achieves_size());
	rank->set_player_huiyi(index + 1, player->huiyi_jihuos_size());
	rank->set_player_chenghao(index + 1, player->chenghao_on());
	rank->set_player_nalflag(index + 1, player->nalflag());

	if (rank->player_guid_size() > rank_nums[num])
	{
		rank->mutable_player_guid()->RemoveLast();
		rank->mutable_player_name()->RemoveLast();
		rank->mutable_player_template()->RemoveLast();
		rank->mutable_player_level()->RemoveLast();
		rank->mutable_player_bf()->RemoveLast();
		rank->mutable_value()->RemoveLast();
		rank->mutable_player_vip()->RemoveLast();
		rank->mutable_player_achieve()->RemoveLast();
		rank->mutable_player_huiyi()->RemoveLast();
		rank->mutable_player_chenghao()->RemoveLast();
		rank->mutable_player_nalflag()->RemoveLast();
	}
}


void RankOperation::login(dhc::player_t *player)
{
	uint64_t level_guid = MAKE_GUID(et_rank, e_rank_type_level);
	dhc::rank_t *level_rank = POOL_GET_RANK(level_guid);
	if (!level_rank)
	{
		return;
	}

	uint64_t bf_guid = MAKE_GUID(et_rank, e_rank_type_bf);
	dhc::rank_t *bf_rank = POOL_GET_RANK(bf_guid);
	if (!bf_rank)
	{
		return;
	}

	if (level_rank->player_guid_size() > 0 && bf_rank->player_guid_size() > 0)
	{
		std::string text;
		if (level_rank->player_guid(0) == player->guid() &&
			bf_rank->player_guid(0) == player->guid())
		{
			SocialOperation::gundong_server("t_server_language_text_first_rank_login", player->name(),"","");
		}
		else if (level_rank->player_guid(0) == player->guid() &&
			bf_rank->player_guid(0) != player->guid())
		{
			SocialOperation::gundong_server("t_server_language_text_level_rank_login", player->name(), "", "");
		}
		else if (level_rank->player_guid(0) != player->guid() &&
			bf_rank->player_guid(0) == player->guid())
		{
			SocialOperation::gundong_server("t_server_language_text_bf_rank_login", player->name(), "", "");
		}
	}
}

int RankOperation::get_player_rank(dhc::player_t *player, e_rank_type type)
{
	int level = MAX_RANK_PLAYER + 1;
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, type));
	if (rank)
	{
		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			if (rank->player_guid(i) == player->guid())
			{
				return i + 1;
			}
		}
	}

	return level;
}

void RankOperation::clear_rank(e_rank_type type)
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, type));
	if (rank)
	{
		rank->clear_player_guid();
		rank->clear_player_name();
		rank->clear_player_template();
		rank->clear_player_level();
		rank->clear_player_bf();
		rank->clear_value();
		rank->clear_player_vip();
		rank->clear_player_achieve();
		rank->clear_player_huiyi();
		rank->clear_player_chenghao();

		rank->set_reward_flag(0);
	}
}


struct huiyi_sort
{
	int index;
	int shoujidu;
	int huiyinum;

	bool operator > (const huiyi_sort& rhs) const
	{
		if (shoujidu == rhs.shoujidu)
			return huiyinum > rhs.huiyinum;
		return shoujidu > rhs.shoujidu;
	}
};

int RankOperation::get_huiyi_rank(dhc::player_t *player, dhc::rank_t *src_rank, dhc::rank_t* dest_rank)
{
	dest_rank->set_guid(0);
	huiyi_sort st;
	std::vector<huiyi_sort> st_vec;
	int rank = -1;
	for (int i = 0; i < src_rank->player_guid_size(); ++i)
	{
		if (rank != -1 && i >= 20)
		{
			break;
		}

		if (i < 20)
		{
			st.index = i;
			st.shoujidu = src_rank->value(i);
			st.huiyinum = src_rank->player_huiyi(i);
			st_vec.push_back(st);
		}

		if (src_rank->player_guid(i) == player->guid())
		{
			rank = i;
		}
	}

	std::sort(st_vec.begin(), st_vec.end(), std::greater<huiyi_sort>());

	int index = 0;
	for (int i = 0; i < st_vec.size(); i++)
	{
		index = st_vec[i].index;
		dest_rank->add_player_guid(src_rank->player_guid(index));
		dest_rank->add_player_name(src_rank->player_name(index));
		dest_rank->add_player_level(src_rank->player_level(index));
		dest_rank->add_player_bf(src_rank->player_bf(index));
		dest_rank->add_value(src_rank->value(index));
		dest_rank->add_player_template(src_rank->player_template(index));
		dest_rank->add_player_vip(src_rank->player_vip(index));
		dest_rank->add_player_achieve(src_rank->player_achieve(index));
		dest_rank->add_player_huiyi(src_rank->player_huiyi(index));
		dest_rank->add_player_chenghao(src_rank->player_chenghao(index));
		dest_rank->add_player_nalflag(src_rank->player_nalflag(index));
	}
	
	return rank;
}

int RankOperation::get_rank(dhc::player_t *player, e_rank_type type)
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, type));
	if (rank)
	{
		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			if (rank->player_guid(i) == player->guid())
			{
				if (rank->value(i) <= 0)
				{
					return -1;
				}
				return i + 1;
			}
		}
	}

	return -1;
}
