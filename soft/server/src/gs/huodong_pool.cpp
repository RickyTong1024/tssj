#include "huodong_pool.h"
#include "rpc.pb.h"
#include "utils.h"
#include "item_operation.h"
#include "rank_operation.h"
#include "post_operation.h"
#include "huodong_config.h"
#include "huodong_operation.h"


#define ADD_REWARD(sub, entry)	\
for (int rindex = 0; rindex < entry->types_size(); ++rindex) \
{ \
	sub->add_types(entry->types(rindex)); \
	sub->add_value1s(entry->value1s(rindex)); \
	sub->add_value2s(entry->value2s(rindex)); \
	sub->add_value3s(entry->value3s(rindex)); \
}


int HuodongPool::init()
{
	has_jieri_ = true;
	has_jieri_flag_ = true;
	jieri_count_ = 0;
	load_complete_ = false;
	has_xingheqingdian_ = 0;
	query_mofang_count_ = 0;
	huodong_list_load();
	return 0;
}

int HuodongPool::fini()
{
	dhc::huodong_t *huodong = 0;
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end(); ++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (huodong)
		{
			save_huodong(huodong, true);
		}
	}
	return 0;
}

int HuodongPool::update()
{
	uint64_t now_time = game::timer()->now();

	dhc::huodong_t *huodong = 0;
	bool is_jieri = false;
	bool is_jieri_flag = false;

	/// 检查进行中活动
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end();)
	{
		if (is_quering(it->second))
		{
			++it;
			continue;
		}

		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			now_huodong_.erase(it++);
			continue;
		}

		if (now_time > huodong->end())
		{
			remove_huodong(huodong);
			now_huodong_.erase(it++);
			continue;
		}

		if (huodong->type() == HUODONG_JRHD_TYPE)
		{
			is_jieri_flag = true;
			if (huodong->jieri_time() > now_time)
			{
				is_jieri = true;
			}
		}
		if (huodong->type() == HUODONG_XHQD_TYPE)
		{
			has_xingheqingdian_ = 1;

			if (huodong->subtype() == HUODONG_TBHD_TYPE)
			{
				tanbao_rank_reward(huodong);
			}
			else if (huodong->subtype() == HUODONG_SZZP_TYPE)
			{
				zhuanpan_rank_reward(huodong);
			}
			else if (huodong->subtype() == HUODONG_TKMY_TYPE)
			{
				tansuo_rank_reward(huodong);
			}
			else if (huodong->subtype() == HUODONG_JGMF_TYPE)
			{
				mofang_reward(huodong);
			}
		}
		
		save_huodong(huodong, false);
		++it;
	}

	if (load_complete_)
	{
		has_jieri_ = is_jieri;
		has_jieri_flag_ = is_jieri_flag;
	}
	else
	{
		jieri_count_ += 1;
		if (jieri_count_ >= 13)
		{
			load_complete_ = true;
		}
	}
	

	/// 检查未开始活动
	huodong = 0;
	for (HuodongMap::iterator it = delay_huodong_.begin();
		it != delay_huodong_.end();)
	{
		if (is_quering(it->second))
		{
			++it;
			continue;
		}

		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			delay_huodong_.erase(it++);
			continue;
		}

		if (now_time > huodong->start())
		{
			add_huodong(huodong, true);
			delay_huodong_.erase(it++);
			continue;
		}

		++it;
	}

	return 0;
}

int  HuodongPool::huodong_add(const rpcproto::tmsg_activity_group &msg)
{
	/// 检查活动有效性
	for (int i = 0; i < msg.activitys_size(); ++i)
	{
		const rpcproto::tmsg_activity &activity = msg.activitys(i);
		HuodongError okerror = check_huodong(activity);
		if (okerror != HUODONG_SUCESS)
		{
			return okerror;
		}

		for (int j = 0; j < activity.rewards_size(); ++j)
		{
			if (check_huodong_entry(activity.rewards(j)) == false)
			{
				return HUODONG_ERROR_INVALID_ARG;
			}
		}
	}

	/// 创建活动
	dhc::huodong_t *huodong = 0;
	for (int i = 0; i < msg.activitys_size(); ++i)
	{
		const rpcproto::tmsg_activity &activity = msg.activitys(i);
		huodong = create_huodong(activity, msg);
		if (huodong)
		{
			add_huodong(huodong, true);
		}
	}

	return 0;
}

void HuodongPool::huodong_list_view(dhc::player_t *player, protocol::game::smsg_huodong_view& msg, HuodongListViewType type) const
{
	if (now_huodong_.empty())
	{
		return;
	}
 
	dhc::huodong_t *huodong = 0;
	int level = 0;
	int day = game::timer()->run_day(game::gtool()->start_time());
	for (HuodongMap::const_iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		level = get_huodong_level(player, huodong);
		if (day >= huodong->kaikai_start() &&
			level >= huodong->kai_start() && 
			level <= huodong->kai_end())
		{
			if (type == HUODONG_HUODONG_VIEW)
			{

				if (huodong->type() != HUODONG_JRHD_TYPE &&
					huodong->type() != HUODONG_XHQD_TYPE &&
					huodong->type() != HUODONG_YKJJ_TYPE)
				{
					msg.add_huodong_ids(huodong->id());
					msg.add_huodong_times(huodong->end());
					msg.add_huodong_names(huodong->name());
					msg.add_huodong_types(huodong->type());
				}
				if (huodong->type() == HUODONG_YKJJ_TYPE &&
					huodong->subtype() == HUODONG_YKJJ_TYPE 
					)
				{	
					dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_YKJJ_TYPE, HUODONG_YKJJ_TYPE);
					if (huodong_player)
					{
						if (game::timer()->run_day(huodong->start()) < 5)
						{
							msg.add_huodong_ids(huodong->id());
							msg.add_huodong_times(huodong->start() + 5 * 24 * 60 * 60 * 1000);
							msg.add_huodong_names(huodong->name());
							msg.add_huodong_types(huodong->type());
						}

						if (game::timer()->run_day(huodong->start()) >= 5 && huodong_player->arg1() != 0)
						{
							msg.add_huodong_ids(huodong->id());
							msg.add_huodong_times(huodong->start() + 5 * 24 * 60 * 60 * 1000);
							msg.add_huodong_names(huodong->name());
							msg.add_huodong_types(huodong->type());
						}
					}
				}
			}
			else if (type == HUODONG_XINGHEQIDIAN_VIEW)
			{
				if (huodong->type() == HUODONG_XHQD_TYPE)
				{
					msg.add_huodong_ids(huodong->id());
					msg.add_huodong_times(huodong->end());
					msg.add_huodong_names(huodong->name());
					msg.add_huodong_types(huodong->type());
				}
			}
			
		}
	}
}

bool HuodongPool::huodong_view(dhc::player_t* player,
	int id,
	HuodongType type,
	protocol::game::smsg_huodong_reward_view& smsg) const
{
	switch (type)
	{
	case HUODONG_CZFL_TYPE:
		return get_ljcz_view(player, id, type, smsg);
		break;
	case HUODONG_HYLX_TYPE:
		return get_hyhd_view(player, id, type, smsg);
		break;
	case HUODONG_DBCZ_TYPE:
		return get_dbcz_view(player, id, type, smsg);
		break;
	case HUODONG_DLSL_TYPE:
		return get_dlsl_view(player, id, type, smsg);
		break;
	case HUODONG_ZKFS_TYPE:
		return get_zkfs_view(player, id, type, smsg);
		break;
	case HUODONG_DHDJ_TYPE:
		return get_dhhd_view(player, id, type, smsg);
		break;
	case HUODONG_RQDL_TYPE:
		return get_rqdl_view(player, id, type, smsg);
		break;
	case HUODONG_ZC_TYPE:
		return get_zchd_view(player, id, type, smsg);
		break;
	default:
		break;
	}
	return false;
}

bool HuodongPool::huodong_view(dhc::player_t* player,
	HuodongType type,
	protocol::game::smsg_huodong_jiri_view& smsg) const
{
	const dhc::huodong_t* huodong = 0;
	protocol::game::huodong_jiri* jiri = 0;
	protocol::game::smsg_huodong_reward_view* rewards = 0;
	int level = 0;
	int day = game::timer()->run_day(game::gtool()->start_time());
	uint64_t now_time = game::timer()->now();

	for (HuodongMap::const_iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		level = get_huodong_level(player, huodong);
		if (huodong->type() == type &&
			now_time >= huodong->show_time() &&
			now_time < huodong->noshow_time() &&
			level >= huodong->kai_start() &&
			level <= huodong->kai_end() &&
			day >= huodong->kaikai_start())
		{
			jiri = smsg.add_huodongs();
			if (jiri)
			{
				jiri->set_id(huodong->id());
				jiri->set_time(huodong->noshow_time());
				if (huodong->noshow_time() > huodong->jieri_time())
				{
					if (huodong->subtype() != HUODONG_DHDJ_TYPE)
					{
						jiri->set_time(huodong->jieri_time());
					}
					
				}
				jiri->set_type(huodong->subtype());
				huodong_view(player,
					huodong->id(),
					static_cast<HuodongType>(huodong->subtype()),
					*(jiri->mutable_reward()));
			}
		}
	}

	return true;
}

bool HuodongPool::huodong_view_tanbao(dhc::player_t* player,
	protocol::game::smsg_huodong_tanbao_view& smsg)
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_tanbao_normal));
	if (!rank)
	{
		return false;
	}

	dhc::huodong_t* huodong = get_huodong_by_type(HUODONG_XHQD_TYPE, HUODONG_TBHD_TYPE);
	if (huodong)
	{
		dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
		if (huodong_player)
		{
			smsg.set_buy_num(huodong_player->arg1());
			smsg.set_dice_num(huodong_player->arg2());
			smsg.set_final_num(huodong_player->arg3());
			smsg.set_gezi(huodong_player->arg4());
			smsg.set_point(huodong_player->arg5());
			smsg.set_luck_num(huodong_player->arg6());
			smsg.set_shop_type(huodong_player->arg7());
			smsg.set_time(huodong->rank_time());
			smsg.set_rank(0);
			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				if (rank->player_guid(i) == player->guid())
				{
					smsg.set_rank(i + 1);
					break;
				}
			}
			for (int i = 0; i < huodong_player->args3_size(); ++i)
			{
				smsg.add_rewards(huodong_player->args3(i));
			}
			for (int i = 0; i < huodong_player->args1_size(); ++i)
			{
				smsg.add_shop_ids(huodong_player->args1(i));
				smsg.add_shop_nums(huodong_player->args2(i));
			}
			for (int i = 0; i < huodong_player->args4_size(); ++i)
			{
				smsg.add_shop_types(huodong_player->args4(i));
				smsg.add_shop_type_nums(huodong_player->args5(i));
			}

			if (game::timer()->now() > huodong->rank_time())
			{
				smsg.set_gezi(0);
				for (int i = 0; i < rank->player_name_size(); ++i)
				{
					if (i >= 3)
					{
						break;
					}
					smsg.add_names(rank->player_name(i));
					smsg.add_points(rank->value(i));
					smsg.add_ids(rank->player_template(i));
					smsg.add_vips(rank->player_vip(i));
					smsg.add_achieves(rank->player_achieve(i));
					smsg.add_chenghaos(rank->player_chenghao(i));
					smsg.add_nalflag(rank->player_nalflag(i));
				}
			}

			return true;
		}
	}
	return false;
}

bool HuodongPool::huodong_view_fanpai(dhc::player_t *player, protocol::game::smsg_huodong_fanpai_view& smsg)
{
	dhc::huodong_t* huodong = get_huodong_by_type(HUODONG_XHQD_TYPE, HUODONG_CZFP_TYPE);
	if (huodong)
	{
		dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
		if (huodong_player)
		{
			smsg.set_point(huodong_player->arg1());
			smsg.set_num1(huodong_player->arg2());
			smsg.set_num2(huodong_player->arg3());
			smsg.set_time(huodong->end());
			smsg.set_use(huodong_player->arg6());

			for (int i = 0; i < huodong_player->args1_size(); ++i)
			{
				smsg.add_ids1(huodong_player->args1(i));
			}

			for (int i = 0; i < huodong_player->args2_size(); ++i)
			{
				smsg.add_ids2(huodong_player->args2(i));
			}

			return true;
		}
	}
	return false;
}


bool HuodongPool::huodong_view_zhuanpan(dhc::player_t *player, protocol::game::smsg_huodong_zhuanpan_view& smsg)
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_zhuanpan_normal));
	if (!rank)
	{
		return false;
	}

	dhc::huodong_t* huodong = get_huodong_by_type(HUODONG_XHQD_TYPE, HUODONG_SZZP_TYPE);
	if (huodong)
	{
		dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
		if (huodong_player)
		{
			smsg.set_num1(huodong_player->arg1());
			smsg.set_jewel_pool1(huodong->extra_data());
			smsg.set_num2(huodong_player->arg3());
			smsg.set_jewel_pool2(huodong->extra_data1());
			smsg.set_point(huodong_player->arg5());
			smsg.set_rank(0);
			smsg.set_time(huodong->rank_time());

			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				if (rank->player_guid(i) == player->guid())
				{
					smsg.set_rank(i + 1);
					break;
				}
			}

			if (game::timer()->now() > huodong->rank_time())
			{
				for (int i = 0; i < rank->player_name_size(); ++i)
				{
					if (i >= 3)
					{
						break;
					}

					smsg.add_names(rank->player_name(i));
					smsg.add_points(rank->value(i));
					smsg.add_ids(rank->player_template(i));
					smsg.add_vips(rank->player_vip(i));
					smsg.add_achieves(rank->player_achieve(i));
					smsg.add_chenghaos(rank->player_chenghao(i));
					smsg.add_nalfalgs(rank->player_nalflag(i));
				}
			}

			return true;
		}
	}
	return false;
}

bool HuodongPool::huodong_view_tansuo(dhc::player_t *player, protocol::game::smsg_huodong_tansuo_view& smsg)
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_tansuo_normal));
	if (!rank)
	{
		return false;
	}

	dhc::huodong_t* huodong = get_huodong_by_type(HUODONG_XHQD_TYPE, HUODONG_TKMY_TYPE);
	if (huodong)
	{
		dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
		if (huodong_player)
		{
			// check past
			uint64_t now_time = game::timer()->now();
			for (int km = 0; km < huodong_player->times_size();)
			{
				if (now_time > huodong_player->times(km))
				{
					huodong_player->mutable_args1()->SwapElements(km, huodong_player->args1_size() - 1);
					huodong_player->mutable_args1()->RemoveLast();
					huodong_player->mutable_args2()->SwapElements(km, huodong_player->args2_size() - 1);
					huodong_player->mutable_args2()->RemoveLast();
					huodong_player->mutable_args3()->SwapElements(km, huodong_player->args3_size() - 1);
					huodong_player->mutable_args3()->RemoveLast();
					huodong_player->mutable_args4()->SwapElements(km, huodong_player->args4_size() - 1);
					huodong_player->mutable_args4()->RemoveLast();
					huodong_player->mutable_args5()->SwapElements(km, huodong_player->args5_size() - 1);
					huodong_player->mutable_args5()->RemoveLast();
					huodong_player->mutable_args6()->SwapElements(km, huodong_player->args6_size() - 1);
					huodong_player->mutable_args6()->RemoveLast();
					huodong_player->mutable_args7()->SwapElements(km, huodong_player->args7_size() - 1);
					huodong_player->mutable_args7()->RemoveLast();
					huodong_player->mutable_args8()->SwapElements(km, huodong_player->args8_size() - 1);
					huodong_player->mutable_args8()->RemoveLast();
					huodong_player->mutable_times()->SwapElements(km, huodong_player->times_size() - 1);
					huodong_player->mutable_times()->RemoveLast();
					huodong_player->mutable_extra_data1()->SwapElements(km, huodong_player->extra_data1_size() - 1);
					huodong_player->mutable_extra_data1()->RemoveLast();
					huodong_player->mutable_extra_data2()->SwapElements(km, huodong_player->extra_data2_size() - 1);
					huodong_player->mutable_extra_data2()->RemoveLast();
					huodong_player->mutable_extra_data3()->SwapElements(km, huodong_player->extra_data3_size() - 1);
					huodong_player->mutable_extra_data3()->RemoveLast();
					huodong_player->mutable_extra_data4()->SwapElements(km, huodong_player->extra_data4_size() - 1);
					huodong_player->mutable_extra_data4()->RemoveLast();
					huodong_player->mutable_extra_data5()->SwapElements(km, huodong_player->extra_data5_size() - 1);
					huodong_player->mutable_extra_data5()->RemoveLast();
					huodong_player->mutable_extra_data6()->SwapElements(km, huodong_player->extra_data6_size() - 1);
					huodong_player->mutable_extra_data6()->RemoveLast();
				}
				else
				{
					++km;
				}
			}
			smsg.set_point(huodong_player->arg2());
			smsg.set_num(huodong_player->arg1());
			smsg.set_count(huodong_player->arg3());
			smsg.set_rank(0);
			smsg.set_time(huodong->rank_time());

			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				if (rank->player_guid(i) == player->guid())
				{
					smsg.set_rank(i + 1);
					break;
				}
			}

			for (int i = 0; i < huodong_player->args9_size(); ++i)
			{
				smsg.add_rewards(huodong_player->args9(i));
			}

			if (game::timer()->now() > huodong->rank_time())
			{
				for (int i = 0; i < rank->player_name_size(); ++i)
				{
					if (i >= 3)
					{
						break;
					}

					smsg.add_names(rank->player_name(i));
					smsg.add_points(rank->value(i));
					smsg.add_ids(rank->player_template(i));
					smsg.add_vips(rank->player_vip(i));
					smsg.add_achieves(rank->player_achieve(i));
					smsg.add_chenghaos(rank->player_chenghao(i));
					smsg.add_nalflags(rank->player_nalflag(i));
				}
			}
			else
			{
				for (int i = 0; i < huodong_player->args1_size(); ++i)
				{
					protocol::game::tansuo_event *te = smsg.add_events();
					if (te)
					{
						te->set_id(huodong_player->args1(i));
						te->set_ts_id(huodong_player->args2(i));
						te->set_qiyu_id(huodong_player->args3(i));
						te->set_qiyu_time(huodong_player->times(i));
						te->set_qiyu_arg1(huodong_player->args4(i));
						te->set_qiyu_arg2(huodong_player->args5(i));
						te->set_qiyu_arg3(huodong_player->args6(i));
						te->set_qiyu_arg4(huodong_player->args7(i));
					}
				}
			}

			return true;
		}
	}
	return false;
}

bool HuodongPool::huodong_view_mofang(dhc::player_t *player, protocol::game::smsg_huodong_mofang_view& smsg)
{
	dhc::huodong_t* huodong = get_huodong_by_type(HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
	if (huodong)
	{
		dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
		if (huodong_player)
		{
			smsg.set_total_point(huodong_player->arg3());
			smsg.set_left_point(huodong_player->arg4());
			smsg.set_server_point(huodong->extra_data());
			smsg.set_num(huodong_player->arg2());
			smsg.set_free_num(huodong_player->arg5());
			smsg.set_stat(huodong_player->arg1());
			smsg.set_time(huodong->rank_time());
			if (huodong_player->arg1() == 0)
			{
				for (int i = 0; i < huodong_player->args1_size(); ++i)
				{
					smsg.add_ids(huodong_player->args1(i));
				}
			}
			else
			{
				for (int i = 0; i < huodong_player->args2_size(); ++i)
				{
					smsg.add_ids(huodong_player->args2(i));
				}
			}
			for (int i = 0; i < huodong_player->args3_size(); ++i)
			{
				smsg.add_rewards(huodong_player->args3(i));
			}
			for (int i = 0; i < huodong_player->args4_size(); ++i)
			{
				smsg.add_shop_ids(huodong_player->args4(i));
				smsg.add_shop_nums(huodong_player->args5(i));
			}
		}
	}

	return false;
}

bool HuodongPool::huodong_view_yueka(dhc::player_t *player, protocol::game::smsg_huodong_yueka_view& smsg)
{
	dhc::huodong_t* huodong = get_huodong_by_type(HUODONG_YKJJ_TYPE, HUODONG_YKJJ_TYPE);
	if (huodong)
	{
		dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
		if (huodong_player)
		{
			smsg.set_level(huodong_player->arg1());
			smsg.set_stat_time(huodong->start());
			smsg.set_rem_time(huodong->start() + 5 * 24 * 60 * 60 * 1000);
			smsg.set_end_time(huodong->end());
			if (huodong_player->arg1() == 1)
			{
				for (int i = 0;i < huodong_player->args1_size(); ++i)
				{
					smsg.add_reward1(huodong_player->args1(i));
				}
			}
			
			if (huodong_player->arg1() == 2)
			{
				for (int i = 0; i < huodong_player->args2_size();++i)
				{
					smsg.add_reward2(huodong_player->args2(i));
				}
			}

			if (huodong_player->arg1() == 3)
			{
				for (int i = 0; i < huodong_player->args1_size(); ++i)
				{
					smsg.add_reward1(huodong_player->args1(i));
				}
				for (int i = 0; i < huodong_player->args2_size(); ++i)
				{
					smsg.add_reward2(huodong_player->args2(i));
				}
			}
		}
		return true;
	}		
	return false;
}

const dhc::huodong_entry_t* HuodongPool::get_huodong_entry(int huodong_id, int id, HuodongType type) const
{
	const dhc::huodong_t *huodong = get_huodong(huodong_id);
	if (!huodong)
	{
		return 0;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return 0;
	}
	dhc::huodong_entry_t *entry = 0;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (entry && entry->id() == id)
		{
			return entry;
		}
	}
	return 0;
}

dhc::huodong_entry_t* HuodongPool::get_huodong_entry(int huodong_id, int id, HuodongType type)
{
	dhc::huodong_t *huodong = get_huodong(huodong_id);
	if (!huodong)
	{
		return 0;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return 0;
	}
	dhc::huodong_entry_t *entry = 0;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (entry && entry->id() == id)
		{
			return entry;
		}
	}
	return 0;
}

void HuodongPool::huodong_login(dhc::player_t *player)
{
	if (game::timer()->run_day(player->huodong_yxhg_time()) > 2 )
	{
		player->set_huodong_yxhg_tap(0);
		player->clear_huodong_yxhg_buzhu_id();
		player->clear_huodong_yxhg_fuli_id();
		player->clear_huodong_yxhg_fuli_num();
	}
	if (player->level() >= 60 && game::timer()->now() > player->last_login_time() + 3 * 24 * 60 * 60 * 1000)
	{
		player->set_huodong_yxhg_tap(1);                                                                             //获得资格
		player->set_huodong_yxhg_buzhu_day(1);
		player->set_huodong_yxhg_time(game::timer()->now());
	}
	if (game::timer()->run_day(player->last_login_time()) > 0 &&
		game::timer()->run_day(player->last_login_time()) < 3 &&
		player->huodong_yxhg_tap() == 1)
	{
		player->set_huodong_yxhg_buzhu_day(player->huodong_yxhg_buzhu_day() + 1);
	}

	if (now_huodong_.empty())
	{
		return;
	}

	uint64_t now_time = game::timer()->now();
	int day = game::timer()->run_day(game::gtool()->start_time());

	dhc::huodong_t *huodong = 0;
	dhc::huodong_entry_t* entry = 0;
	int level = 0;
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}

		level = get_huodong_level(player, huodong);
		if (level == -1)
		{
			level = player->level();
			set_huodong_level(player, huodong);
		}

		if (now_time >= huodong->show_time() &&
			now_time < huodong->noshow_time() &&
			now_time < huodong->jieri_time() &&
			level >= huodong->kai_start() &&
			level <= huodong->kai_end() &&
			day >= huodong->kaikai_start())
		{
			if (huodong->type() == HUODONG_DLSL_TYPE || 
				huodong->subtype() == HUODONG_DLSL_TYPE)
			{
				HuodongArgResult output;
				for (int i = 0; i < huodong->entrys_size(); ++i)
				{
					entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
					if (is_cond(entry, HUODONG_COND_DAY))
					{
						get_huodong_arg(player, entry, output);
						if ((output.first == 0 && output.second == 0) ||
							(game::timer()->run_day(player->last_login_time()) > 0))
						{
							set_huodong_arg(player, entry, HUODONG_ARG_COUNT, 1);
						}

					}
				}
			}
			else if (huodong->type() == HUODONG_RQDL_TYPE ||
				huodong->subtype() == HUODONG_RQDL_TYPE)
			{
				HuodongArgResult output;
				for (int i = 0; i < huodong->entrys_size(); ++i)
				{
					entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
					if (is_cond(entry, HUODONG_COND_RIQI))
					{
						if (entry->arg2() == game::timer()->month() &&
							entry->arg3() == game::timer()->day())
						{
							get_huodong_arg(player, entry, output);
							if (output.first == 0 && output.second == 0)
							{
								set_huodong_arg(player, entry, HUODONG_ARG_COUNT, 1);
							}
						}
					}
				}
			}
			else if (huodong->type() == HUODONG_YKJJ_TYPE ||
				huodong->subtype() == HUODONG_YKJJ_TYPE)
			{
				dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
				if (huodong_player)
				{
					if (game::timer()->run_day(player->last_login_time()) > 0)
					{
						huodong_player->set_arg2(huodong_player->arg2() + 1);
						if (huodong_player->arg1() == 1)
							huodong_player->add_args1(1);
						if (huodong_player->arg1() == 2)
							huodong_player->add_args2(1);
						if (huodong_player->arg1() == 3)
						{
							huodong_player->add_args1(1);
							huodong_player->add_args2(1);
						}
						save_huodong_player(huodong_player->guid());
					}
				}
			}
		}		
	}
}

void HuodongPool::huodong_refresh(dhc::player_t *player)
{
	player->set_huodong_yxhg_rmb(0);								  //刷新回归充值数
	player->clear_huodong_yxhg_haoli_id();

	if (player->huodong_yxhg_tap() == 1)
	{
		if (game::channel()->online(player->guid()))
		{
			player->set_last_login_time(game::timer()->now());
			player->set_huodong_yxhg_buzhu_day(player->huodong_yxhg_buzhu_day() + 1);
		}
	}
	
	/// 刷新vip每日礼包
	player->set_huodong_vip_libao(0);
	
	if (now_huodong_.empty())
	{
		return;
	}
	uint64_t now_time = game::timer()->now();
	int day = game::timer()->run_day(game::gtool()->start_time());
	/// 刷新登录
	dhc::huodong_t *huodong = 0;
	dhc::huodong_entry_t* entry = 0;
	int level = 0;
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		level = get_huodong_level(player, huodong);
		if (now_time >= huodong->show_time() &&
			now_time < huodong->noshow_time() &&
			now_time < huodong->jieri_time() &&
			level >= huodong->kai_start() &&
			level <= huodong->kai_end() &&
			day >= huodong->kaikai_start())
		{
			if (huodong->type() == HUODONG_DLSL_TYPE ||
				huodong->subtype() == HUODONG_DLSL_TYPE)
			{
				if (game::channel()->online(player->guid()))
				{
					player->set_last_login_time(game::timer()->now());
					for (int i = 0; i < huodong->entrys_size(); ++i)
					{
						entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
						if (is_cond(entry, HUODONG_COND_DAY))
						{
							set_huodong_arg(player, entry, HUODONG_ARG_COUNT, 1);
						}
					}
				}
			}
			else if (huodong->type() == HUODONG_RQDL_TYPE ||
				huodong->subtype() == HUODONG_RQDL_TYPE)
			{
				if (game::channel()->online(player->guid()))
				{
					for (int i = 0; i < huodong->entrys_size(); ++i)
					{
						entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
						if (is_cond(entry, HUODONG_COND_RIQI))
						{
							if (entry->arg2() == game::timer()->month() &&
								entry->arg3() == game::timer()->day())
							{
								set_huodong_arg(player, entry, HUODONG_ARG_COUNT, 1);
							}
						}
					}
				}
			}
			else if (huodong->type() == HUODONG_XHQD_TYPE &&
				huodong->subtype() == HUODONG_TBHD_TYPE)
			{
				dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
				if (huodong_player)
				{
					huodong_player->set_arg2(5);
					huodong_player->set_arg1(0);

					save_huodong_player(huodong_player->guid());
				}
			}
			else if (huodong->type() == HUODONG_XHQD_TYPE &&
				huodong->subtype() == HUODONG_CZFP_TYPE)
			{
				dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
				if (huodong_player)
				{
					huodong_player->set_arg2(0);
					huodong_player->set_arg3(0);
					huodong_player->set_arg6(0);

					save_huodong_player(huodong_player->guid());
				}
			}
			else if (huodong->type() == HUODONG_XHQD_TYPE &&
				huodong->subtype() == HUODONG_SZZP_TYPE)
			{
				dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
				if (huodong_player)
				{
					huodong_player->set_arg1(0);
					huodong_player->set_arg3(0);

					save_huodong_player(huodong_player->guid());
				}
			}
			else if (huodong->type() == HUODONG_XHQD_TYPE &&
				huodong->subtype() == HUODONG_TKMY_TYPE)
			{
				dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
				if (huodong_player)
				{
					huodong_player->set_arg1(0);

					save_huodong_player(huodong_player->guid());
				}
			}
			else if (huodong->type() == HUODONG_XHQD_TYPE &&
				huodong->subtype() == HUODONG_JGMF_TYPE)
			{
				dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
				if (huodong_player)
				{
					huodong_player->set_arg2(0);
					huodong_player->set_arg5(0);
					huodong_player->clear_args4();
					huodong_player->clear_args5();

					save_huodong_player(huodong_player->guid());
				}
			}
			else if (huodong->type() == HUODONG_YKJJ_TYPE &&
				huodong->subtype() == HUODONG_YKJJ_TYPE)
			{
				if (game::channel()->online(player->guid()))
				{
					player->set_last_login_time(game::timer()->now());
					dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
					if (huodong_player)
					{
						if (game::timer()->now() > huodong_player->start() +  5 * 24 * 60 * 60 * 1000)
						{
							huodong_player->set_arg2(huodong_player->arg2() + 1);
							if (huodong_player->arg1() == 1)
								huodong_player->add_args1(1);
							if (huodong_player->arg1() == 2)
								huodong_player->add_args2(1);
							if (huodong_player->arg1() == 3)
							{
								huodong_player->add_args1(1);
								huodong_player->add_args2(1);
							}
							save_huodong_player(huodong_player->guid());
						}
					}
				}
			}
		}
	}
}

void HuodongPool::huodong_week_refresh(dhc::player_t *player)
{
	/// 刷新每周礼包
	player->clear_huodong_week_libao();
}

void HuodongPool::huodong_recharge(dhc::player_t *player, int vippt, int iosid, int type)
{
	player->set_huodong_yxhg_rmb(player->huodong_yxhg_rmb() + vippt);             //回归每日累计充值
	int day_recharge = game::timer()->run_day(player->birth_time());
	if (day_recharge >= 7 && day_recharge <= 13)
	{
		player->add_huodong_kaifu_dbcz(vippt);
	}
	int day = game::timer()->run_day(game::gtool()->start_time());
	if (now_huodong_.empty())
	{
		return;
	}

	uint64_t now_time = game::timer()->now();
	

	dhc::huodong_t *huodong = 0;
	dhc::huodong_entry_t* entry = 0;
	int level = 0;
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		level = get_huodong_level(player, huodong);
		if (now_time >= huodong->show_time() &&
			now_time < huodong->noshow_time() &&
			now_time < huodong->jieri_time() &&
			level >= huodong->kai_start() &&
			level <= huodong->kai_end() &&
			day >= huodong->kaikai_start())
		{
			if (huodong->type() == HUODONG_CZFL_TYPE ||
				huodong->subtype() == HUODONG_CZFL_TYPE)
			{
				for (int i = 0; i < huodong->entrys_size(); ++i)
				{
					entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
					if (is_cond(entry, HUODONG_COND_RMB))
					{
						set_huodong_arg(player, entry, HUODONG_ARG_COUNT, vippt);
					}
				}
			}
			else if (huodong->type() == HUODONG_DBCZ_TYPE ||
				huodong->subtype() == HUODONG_DBCZ_TYPE)
			{
				for (int i = 0; i < huodong->entrys_size(); ++i)
				{
					if (type == 1)
					{
						break;
					}
					entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
					if (is_cond(entry, HUODONG_COND_RMB))
					{
						if (entry->arg1() == iosid)
						{
							set_huodong_arg(player, entry, HUODONG_ARG_COUNT, 1);
						}
					}
				}
			}
			else if (huodong->type() == HUODONG_XHQD_TYPE &&
				huodong->subtype() == HUODONG_CZFP_TYPE)
			{
				dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
				if (huodong_player)
				{
					huodong_player->set_arg1(huodong_player->arg1() + vippt);
					save_huodong_player(huodong_player->guid());
				}
			}
			else if (huodong->type() == HUODONG_YKJJ_TYPE &&
				huodong->subtype() == HUODONG_YKJJ_TYPE)
			{
				if (game::timer()->run_day(huodong->start()) < 5)
				{
					if (now_time < player->zhouka_time() && now_time < player->yueka_time())
					{
						dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
						if (huodong_player)
						{
							if (vippt == 980)
							{
								if (huodong_player->arg1() == 0)
									huodong_player->set_arg1(1);
								if (huodong_player->arg1() == 2)
									huodong_player->set_arg1(3);
							}
							else if (vippt == 3280)
							{
								if (huodong_player->arg1() == 0)
									huodong_player->set_arg1(2);
								if (huodong_player->arg1() == 1)
									huodong_player->set_arg1(3);
							}
							else if (vippt == 6480)
							{
								huodong_player->set_arg1(3);
							}
							save_huodong_player(huodong_player->guid());
						}
					}
				}
			}
			
			else if (huodong->type() == HUODONG_XHQD_TYPE &&
				huodong->subtype() == HUODONG_TKMY_TYPE)
			{
				dhc::huodong_player_t* huodong_player = get_huodong_player(player, huodong);
				if (huodong_player)
				{
					for (int i = 0; i < huodong_player->args2_size(); ++i)
					{
						if (huodong_player->args2(i) == 3005 &&
							huodong_player->args5(i) == iosid)
						{
							huodong_player->set_args4(i, 1);
						}
					}
					save_huodong_player(huodong_player->guid());
				}
			}
		}
	}
}

void HuodongPool::huodong_zhichong(dhc::player_t *player, int huodong_id, int entry_id, s_t_rewards& strewards)
{
	uint64_t now_time = game::timer()->now();
	int level = 0;
	int day = game::timer()->run_day(game::gtool()->start_time());
	dhc::huodong_t *huodong = 0;
	dhc::huodong_entry_t *entry = 0;
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		if (huodong->id() != huodong_id)
		{
			continue;
		}
		if (huodong->type() != HUODONG_ZC_TYPE)
		{
			continue;
		}
		level = get_huodong_level(player, huodong);
		if (now_time >= huodong->show_time() &&
			now_time < huodong->noshow_time() &&
			now_time < huodong->jieri_time() &&
			level >= huodong->kai_start() &&
			level <= huodong->kai_end() &&
			day >= huodong->kaikai_start())
		{
			for (int i = 0; i < huodong->entrys_size(); ++i)
			{
				entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
				if (is_cond(entry, HUODONG_COND_RMB))
				{
					if (entry->id() == entry_id)
					{
						HuodongArgResult result;
						get_huodong_arg(player, entry, result);
						if (result.first < entry->arg2())
						{
							for (int j = 0; j < entry->types_size(); ++j)
							{
								strewards.add_reward(entry->types(j), entry->value1s(j), entry->value2s(j), entry->value3s(j));
							}
							strewards.add_reward(1, resource::VIP_EXP, entry->arg3());
							set_huodong_arg(player, entry, HUODONG_ARG_COUNT, 1);
						}
					}
				}
			}
		}
	}
}


void HuodongPool::huodong_modify(dhc::player_t *player, int id, int rmb)
{
	if (now_huodong_.empty())
	{
		return;
	}

	uint64_t now_time = game::timer()->now();
	int day = game::timer()->run_day(game::gtool()->start_time());

	dhc::huodong_t *huodong = 0;
	dhc::huodong_entry_t* entry = 0;
	int level = 0;
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}

		if (huodong->id() != id)
		{
			continue;
		}

		level = get_huodong_level(player, huodong);
		if (level == -1)
		{
			level = player->level();
			set_huodong_level(player, huodong);
		}

		if (now_time >= huodong->show_time() &&
			now_time < huodong->noshow_time() &&
			now_time < huodong->jieri_time() &&
			level >= huodong->kai_start() &&
			level <= huodong->kai_end() &&
			day >= huodong->kaikai_start())
		{
			if (huodong->type() == HUODONG_CZFL_TYPE ||
				huodong->subtype() == HUODONG_CZFL_TYPE)
			{
				for (int i = 0; i < huodong->entrys_size(); ++i)
				{
					entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
					if (is_cond(entry, HUODONG_COND_RMB))
					{
						set_huodong_arg(player, entry, HUODONG_ARG_COUNT, rmb);
					}
				}
			}
		}
	}
}

void HuodongPool::huodong_active(dhc::player_t* player, HuodongConditionType cond, int val)
{
	int day_kaifu = game::timer()->run_day(player->birth_time());
	if (day_kaifu >= 7 && day_kaifu <= 13)
	{
		switch (cond)
		{
		case HUODONG_COND_NORMAL_COUNT:
			player->set_huodong_kaifu_ptfb(player->huodong_kaifu_ptfb() + val);
			break;
		case HUODONG_COND_JJC_COUNT:
			player->set_huodong_kaifu_jjc(player->huodong_kaifu_jjc() + val);
			break;
		case HUODONG_COND_ROB_COUNT:
			player->set_huodong_kaifu_dbfb(player->huodong_kaifu_dbfb() + val);
			break;
		case HUODONG_COND_JY_COUNT:
			player->set_huodong_kaifu_jyfb(player->huodong_kaifu_jyfb() + val);
			break;
		default:
			break;
		}
	}

	int day = game::timer()->run_day(game::gtool()->start_time());
	if (now_huodong_.empty())
	{
		return;
	}

	uint64_t now_time = game::timer()->now();
	

	dhc::huodong_t *huodong = 0;
	dhc::huodong_entry_t* entry = 0;
	int level = 0;
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		level = get_huodong_level(player, huodong);
		if (now_time >= huodong->show_time() &&
			now_time < huodong->noshow_time() &&
			now_time < huodong->jieri_time() &&
			level >= huodong->kai_start() &&
			level <= huodong->kai_end() &&
			day >= huodong->kaikai_start())
		{
			if (huodong->type() == HUODONG_HYLX_TYPE ||
				huodong->subtype() == HUODONG_HYLX_TYPE)
			{
				for (int i = 0; i < huodong->entrys_size(); ++i)
				{
					entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
					if (is_cond(entry, cond))
					{
						set_huodong_arg(player, entry, HUODONG_ARG_COUNT, val);
					}
				}
			}
		}
	}
}

void HuodongPool::huodong_jiri(dhc::player_t *player, protocol::game::smsg_player_check& smsg)
{
	smsg.set_huodong("");
	smsg.set_jieri_point(0);

	if (!has_jieri_flag_)
	{
		return;
	}

	smsg.set_jieri_point(has_jieri_point(player));

	uint64_t now_time = game::timer()->now();
	int day = game::timer()->run_day(game::gtool()->start_time());
	const dhc::huodong_t *huodong = 0;
	int level = -1;
	for (HuodongMap::const_iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (huodong && 
			huodong->type() == HUODONG_JRHD_TYPE)
		{
			level = get_huodong_level(player, huodong);
			if (now_time >= huodong->show_time() &&
				now_time < huodong->noshow_time() &&
				day >= huodong->kaikai_start() &&
				level >= huodong->kai_start() &&
				level <= huodong->kai_end())
			{
				smsg.set_huodong(huodong->group_name());
				smsg.set_huodong_item1(huodong->item_name1());
				smsg.set_huodong_item2(huodong->item_name2());
				smsg.set_huodong_des1(huodong->item_des1());
				smsg.set_huodong_des2(huodong->item_des2());
				smsg.set_jieri_chanchu(has_jieri_);
				break;
			}
		}
	}
}

void HuodongPool::huodong_xingheqingdian(dhc::player_t *player, protocol::game::smsg_player_check& smsg)
{
	smsg.set_xingheqidian(0);

	if (has_xingheqingdian_ != 1)
	{
		return;
	}

	uint64_t now_time = game::timer()->now();
	int day = game::timer()->run_day(game::gtool()->start_time());
	const dhc::huodong_t *huodong = 0;
	int level = -1;
	int ok = 0;
	for (HuodongMap::const_iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (huodong &&
			huodong->type() == HUODONG_XHQD_TYPE)
		{
			level = get_huodong_level(player, huodong);
			if (now_time >= huodong->show_time() &&
				now_time < huodong->noshow_time() &&
				day >= huodong->kaikai_start() &&
				level >= huodong->kai_start() &&
				level <= huodong->kai_end())
			{
				ok |= 1 << (huodong->subtype() - HUODONG_TBHD_TYPE);
			}
		}
	}

	smsg.set_xingheqidian(ok);
}

bool HuodongPool::has_jieri_huodong(dhc::player_t *player) const
{
	if (!has_jieri_)
	{
		return false;
	}

	const dhc::huodong_t *huodong = 0;
	uint64_t now_time = game::timer()->now();
	int day = game::timer()->run_day(game::gtool()->start_time());
	int level = -1;
	for (HuodongMap::const_iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		if (huodong->type() != HUODONG_JRHD_TYPE)
		{
			continue;
		}
		level = get_huodong_level(player, huodong);
		if (now_time < huodong->jieri_time() &&
			level >= huodong->kai_start() &&
			level <= huodong->kai_end() &&
			day >= huodong->kaikai_start())
		{
			return true;
		}
		else
		{
			return false;
		}
	}
	return false;
}

int HuodongPool::has_jieri_point(dhc::player_t *player) const
{
	if (!has_jieri_huodong_flag())
	{
		return 0;
	}

	dhc::huodong_t *huodong = 0;
	uint64_t now_time = game::timer()->now();
	int day = game::timer()->run_day(game::gtool()->start_time());
	int level = 0;
	for (HuodongMap::const_iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		if (huodong->type() == HUODONG_JRHD_TYPE &&
			now_time >= huodong->show_time() &&
			now_time < huodong->noshow_time() &&
			day >= huodong->kaikai_start())
		{
			level = get_huodong_level(player, huodong);
			if (level >= huodong->kai_start() &&
				level <= huodong->kai_end())
			{
				switch (huodong->subtype())
				{
				case HUODONG_RQDL_TYPE:
					if (has_rqdl_point(player, huodong))
						return 1;
					break;
				case HUODONG_DBCZ_TYPE:
					if (has_dbcz_point(player, huodong))
						return 1;
					break;
				case HUODONG_HYLX_TYPE:
					if (has_hyhd_point(player, huodong))
						return 1;
					break;
				case HUODONG_DHDJ_TYPE:
					if (has_dhhd_point(player, huodong))
						return 1;
					break;
				default:
					break;
				}
			}
		}
	}

	return 0;
}

void HuodongPool::huodong_list_load()
{
	uint64_t huodong_list_guid = MAKE_GUID(et_huodong_list, 0);
	Request *req = new Request();
	req->add(opc_query, huodong_list_guid, new protocol::game::huodong_list_t);
	game::pool()->upcall(req, boost::bind(&HuodongPool::huodong_list_load_callback, this, _1, huodong_list_guid));
}

void HuodongPool::huodong_list_load_callback(Request *req, uint64_t huodong_list_guid)
{
	dhc::huodong_t* new_huodong = 0;
	dhc::huodong_t* old_huodong = 0;
	if (req->success())
	{
		protocol::game::huodong_list_t *huodong_list = (protocol::game::huodong_list_t *)req->data();
		for (int i = 0; i < huodong_list->huodongs_size(); ++i)
		{
			old_huodong = huodong_list->mutable_huodongs(i);
			new_huodong = new dhc::huodong_t();
			new_huodong->CopyFrom(*old_huodong);
			add_query(new_huodong->guid());
			POOL_ADD(new_huodong->guid(), new_huodong);
			add_huodong(new_huodong, false);

			for (int m = 0; m < new_huodong->entrys_size(); ++m)
			{
				huodong_entry_load(new_huodong->guid(), new_huodong->entrys(m));
			}

			for (int m = 0; m < new_huodong->player_subs_size(); ++m)
			{
				huodong_sub_load(new_huodong->guid(), new_huodong->player_subs(m));
			}

			dec_query(new_huodong->guid());
		}
	}
}

void HuodongPool::huodong_entry_load(uint64_t huodong_guid, uint64_t huodong_entry_guid)
{
	inc_query(huodong_guid);
	Request *req = new Request();
	req->add(opc_query, huodong_entry_guid, new dhc::huodong_entry_t);
	game::pool()->upcall(req, boost::bind(&HuodongPool::huodong_entry_load_callback, this, _1, huodong_guid));
}

void HuodongPool::huodong_entry_load_callback(Request *req, uint64_t huodong_guid)
{
	dec_query(huodong_guid);
	if (req->success())
	{
		dhc::huodong_entry_t *huodong_entry = (dhc::huodong_entry_t*)req->release_data();
		if (huodong_entry)
		{
			POOL_ADD(huodong_entry->guid(), huodong_entry);
		}
	}
}

void HuodongPool::huodong_sub_load(uint64_t huodong_guid, uint64_t huodong_sub_guid)
{
	inc_query(huodong_guid);
	Request *req = new Request();
	req->add(opc_query, huodong_sub_guid, new dhc::huodong_player_t);
	game::pool()->upcall(req, boost::bind(&HuodongPool::huodong_sub_load_callback, this, _1, huodong_guid));
}

void HuodongPool::huodong_sub_load_callback(Request *req, uint64_t huodong_guid)
{
	dec_query(huodong_guid);
	if (req->success())
	{
		dhc::huodong_player_t *huodong_sub = (dhc::huodong_player_t*)req->release_data();
		if (huodong_sub)
		{
			POOL_ADD(huodong_sub->guid(), huodong_sub);
		}
	}
}

const dhc::huodong_t* HuodongPool::get_huodong(int id) const
{
	HuodongMap::const_iterator it = now_huodong_.find(id);
	if (it == now_huodong_.end())
	{
		return 0;
	}

	return POOL_GET_HUODONG(it->second);
}

dhc::huodong_t* HuodongPool::get_huodong(int id)
{
	HuodongMap::iterator it = now_huodong_.find(id);
	if (it == now_huodong_.end())
	{
		return 0;
	}

	return POOL_GET_HUODONG(it->second);
}

const dhc::huodong_t* HuodongPool::get_huodong_by_type(HuodongType type, HuodongType subtype) const
{
	const dhc::huodong_t *huodong = 0;
	for (HuodongMap::const_iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (huodong && huodong->type() == type && huodong->subtype() == subtype)
		{
			return huodong;
		}
	}
	return 0;
}

dhc::huodong_t* HuodongPool::get_huodong_by_type(HuodongType type, HuodongType subtype)
{
	dhc::huodong_t *huodong = 0;
	for (HuodongMap::iterator it = now_huodong_.begin();
		it != now_huodong_.end();
		++it)
	{
		huodong = POOL_GET_HUODONG(it->second);
		if (huodong && huodong->type() == type && huodong->subtype() == subtype)
		{
			return huodong;
		}
	}
	return 0;
}

void HuodongPool::add_huodong(dhc::huodong_t *huodong, bool is_new)
{
	if (game::timer()->now() < huodong->start())
	{
		delay_huodong_[huodong->id()] = huodong->guid();
	}
	else
	{
		if (is_new)
		{
			reset_huodong(huodong);
		}
		now_huodong_[huodong->id()] = huodong->guid();
	}
}

void HuodongPool::remove_huodong(dhc::huodong_t *huodong)
{
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		POOL_REMOVE(huodong->entrys(i), 0);
	}
	for (int i = 0; i < huodong->player_subs_size(); ++i)
	{
		POOL_REMOVE(huodong->player_subs(i), 0);
	}
	POOL_REMOVE(huodong->guid(), 0);
}

void HuodongPool::save_huodong(dhc::huodong_t* huodong, bool release)
{
	POOL_SAVE(dhc::huodong_t, huodong, release);

	dhc::huodong_entry_t* entry = 0;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (entry)
		{
			POOL_SAVE(dhc::huodong_entry_t, entry, release);
		}
	}

	dhc::huodong_player_t* huodong_player = 0;
	for (std::set<uint64_t>::iterator it = huodong_player_save_.begin();
		it != huodong_player_save_.end();
		++it)
	{
		huodong_player = POOL_GET_HUODONG_PLAYER(*it);
		if (huodong_player)
		{
			POOL_SAVE(dhc::huodong_player_t, huodong_player, release);
		}
	}
	huodong_player_save_.clear();
}

void HuodongPool::save_huodong_player(uint64_t guid)
{
	huodong_player_save_.insert(guid);
}

void HuodongPool::reset_huodong(dhc::huodong_t* huodong)
{
	std::vector<uint64_t> guids;
	game::pool()->get_entitys(et_player, guids);

	dhc::player_t *player = 0;
	uint64_t now_time = game::timer()->now();
	dhc::huodong_entry_t* entry = 0;
	int mon = game::timer()->month();
	int day = game::timer()->day();
	int server_day = -1;

	for (int i = 0; i < guids.size(); ++i)
	{
		player = POOL_GET_PLAYER(guids[i]);
		if (player)
		{
			set_huodong_level(player, huodong);
		}
		if (player && game::channel()->online(player->guid()))
		{
			if (huodong->type() == HUODONG_DLSL_TYPE ||
				huodong->type() == HUODONG_RQDL_TYPE ||
				huodong->subtype() == HUODONG_DLSL_TYPE ||
				huodong->subtype() == HUODONG_RQDL_TYPE)
			{
				server_day = game::timer()->run_day(game::gtool()->start_time());

				if (now_time >= huodong->show_time() &&
					now_time < huodong->noshow_time() &&
					now_time < huodong->jieri_time() &&
					player->level() >= huodong->kai_start() &&
					player->level() <= huodong->kai_end() &&
					server_day >= huodong->kaikai_start())
				{
					player->set_last_login_time(game::timer()->now());
					for (int i = 0; i < huodong->entrys_size(); ++i)
					{
						entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
						if (is_cond(entry, HUODONG_COND_DAY))
						{
							set_huodong_arg(player->guid(), entry, HUODONG_ARG_COUNT, 1);
						}
						else if (is_cond(entry, HUODONG_COND_RIQI))
						{
							if (entry->arg2() == mon && entry->arg3() == day)
							{
								set_huodong_arg(player->guid(), entry, HUODONG_ARG_COUNT, 1);
							}
						}
					}
				}
			}
		}
	}

	if (huodong->type() == HUODONG_XHQD_TYPE &&
		huodong->subtype() == HUODONG_TBHD_TYPE)
	{
		RankOperation::clear_rank(e_rank_tanbao_normal);
		RankOperation::clear_rank(e_rank_tanbao_jy);
	}

	if (huodong->type() == HUODONG_XHQD_TYPE &&
		huodong->subtype() == HUODONG_SZZP_TYPE)
	{
		RankOperation::clear_rank(e_rank_zhuanpan_normal);
		RankOperation::clear_rank(e_rank_zhuanpan_jy);
	}

	if (huodong->type() == HUODONG_XHQD_TYPE &&
		huodong->subtype() == HUODONG_TKMY_TYPE)
	{
		RankOperation::clear_rank(e_rank_tansuo_normal);
		RankOperation::clear_rank(e_rank_tansuo_jy);
	}
}

bool HuodongPool::is_cond(const dhc::huodong_entry_t *entry, HuodongConditionType type) const
{
	if (!entry)
	{
		return false;
	}
	if (entry->cond() == type)
	{
		return true;
	}
	return false;
}

bool HuodongPool::is_cond_range(const dhc::huodong_entry_t *entry, HuodongConditionType min, HuodongConditionType max) const
{
	if (!entry)
	{
		return false;
	}
	if (entry->cond() >= min &&
		entry->cond() <= max)
	{
		return true;
	}
	return false;
}


HuodongError HuodongPool::check_huodong(const rpcproto::tmsg_activity &msg) const
{
	for (HuodongMap::const_iterator it = now_huodong_.begin();
		it != now_huodong_.end(); ++it)
	{
		dhc::huodong_t *huodong = 0;
		huodong = POOL_GET_HUODONG(it->second);
		if (!huodong)
		{
			continue;
		}
		if (huodong->type() == HUODONG_TBHD_TYPE ||
			huodong->type() == HUODONG_CZFP_TYPE ||
			huodong->type() == HUODONG_SZZP_TYPE ||
			huodong->type() == HUODONG_TKMY_TYPE ||
			huodong->type() == HUODONG_JGMF_TYPE ||
			huodong->type() == HUODONG_JRHD_TYPE)
		{
			if (msg.type() == HUODONG_TBHD_TYPE ||
				msg.type() == HUODONG_CZFP_TYPE ||
				msg.type() == HUODONG_SZZP_TYPE ||
				msg.type() == HUODONG_TKMY_TYPE ||
				msg.type() == HUODONG_JGMF_TYPE ||
				msg.type() == HUODONG_JRHD_TYPE)
			{
				return HUODONG_ERROR_REPEATE;
			}
		}
	}
	/// 无效活动
	if (msg.start() >= msg.end())
	{
		return HUODONG_ERROR_INVALID_TIME;
	}

	/// 活动已结束
	uint64_t now_time = game::timer()->now();
	if (now_time >= msg.end())
	{
		return HUODONG_ERROR_INVALID_TIME;
	}

	/// 重复的活动
	if (now_huodong_.find(msg.id()) != now_huodong_.end())
	{
		return HUODONG_ERROR_REPEATE;
	}

	/// 重复的活动
	if (delay_huodong_.find(msg.id()) != delay_huodong_.end())
	{
		return HUODONG_ERROR_REPEATE;
	}

	return HUODONG_SUCESS;
}

bool HuodongPool::check_huodong_entry(const rpcproto::tmsg_activity_reward &msg) const
{
	if (msg.types_size() == 0)
	{
		return false;
	}

	if (msg.types_size() != msg.value1s_size())
	{
		return false;
	}
	if (msg.types_size() != msg.value2s_size())
	{
		return false;
	}
	if (msg.types_size() != msg.value3s_size())
	{
		return false;
	}
	if (msg.arg6_size() != msg.arg7_size())
	{
		return false;
	}
	if (msg.arg6_size() != msg.arg8_size())
	{
		return false;
	}
	
	return true;
}

dhc::huodong_t* HuodongPool::create_huodong(const rpcproto::tmsg_activity &msg, const rpcproto::tmsg_activity_group &gp)
{
	dhc::huodong_t *huodong = new dhc::huodong_t();
	huodong->set_guid(game::gtool()->assign(et_huodong));
	huodong->set_id(msg.id());
	huodong->set_type(msg.type());
	huodong->set_subtype(msg.subtype());
	huodong->set_start(msg.start());
	huodong->set_end(msg.end());
	huodong->set_name(msg.name());
	huodong->set_kai_start(msg.kaifu_start());
	huodong->set_kai_end(msg.kaifu_end());
	huodong->set_show_time(msg.show());
	huodong->set_noshow_time(msg.noshow());
	huodong->set_group_name(gp.name());
	huodong->set_item_name1(gp.item_name1());
	huodong->set_item_des1(gp.item_des1());
	huodong->set_item_name2(gp.item_name2());
	huodong->set_item_des2(gp.item_des2());
	huodong->set_kaikai_start(msg.kaikai_start());
	huodong->set_extra_data(0);
	huodong->set_extra_data1(0);
	if (msg.type() == HUODONG_JRHD_TYPE)
	{
		huodong->set_jieri_time(msg.start() + 6 * 24 * 60 * 60 * 1000);
	}
	else
	{
		huodong->set_jieri_time(msg.end());
	}

	if (msg.type() == HUODONG_XHQD_TYPE &&
		(msg.subtype() == HUODONG_TBHD_TYPE || msg.subtype() == HUODONG_SZZP_TYPE ||
		msg.subtype() == HUODONG_TKMY_TYPE || msg.subtype() == HUODONG_JGMF_TYPE))
	{
		huodong->set_rank_time(msg.end() - 24 * 60 * 60 * 1000);
	}

	if (msg.type() == HUODONG_YKJJ_TYPE &&
		msg.subtype() == HUODONG_YKJJ_TYPE)
	{
		huodong->set_rank_time(msg.start() + 5 * 24 * 60 * 60 * 1000);
	}
	
	
	dhc::huodong_entry_t* huodong_entry = 0;
	for (int i = 0; i < msg.rewards_size(); ++i)
	{
		huodong_entry = create_huodong_entry(huodong->guid(), msg.rewards(i));
		if (huodong_entry)
		{
			huodong->add_entrys(huodong_entry->guid());
		}
	}
	POOL_ADD_NEW(huodong->guid(), huodong);
	POOL_SAVE(dhc::huodong_t, huodong, false);
	return huodong;
}

dhc::huodong_entry_t* HuodongPool::create_huodong_entry(uint64_t huodong_guid, const rpcproto::tmsg_activity_reward &msg)
{
	dhc::huodong_entry_t *huodong_entry = new dhc::huodong_entry_t();
	huodong_entry->set_guid(game::gtool()->assign(et_huodong_entry));
	huodong_entry->set_huodong_guid(huodong_guid);
	huodong_entry->set_id(msg.id());
	huodong_entry->set_cond(msg.condition());
	huodong_entry->set_arg1(msg.arg1());
	huodong_entry->set_arg2(msg.arg2());
	huodong_entry->set_arg3(msg.arg3());
	huodong_entry->set_arg4(msg.arg4());
	huodong_entry->set_arg5(msg.arg5());
	huodong_entry->set_show_time(msg.show_time());
	for (int i = 0; i < msg.arg6_size(); ++i)
	{
		huodong_entry->add_arg6(msg.arg6(i));
		huodong_entry->add_arg7(msg.arg7(i));
		huodong_entry->add_arg8(msg.arg8(i));
	}
	for (int i = 0; i < msg.types_size(); ++i)
	{
		huodong_entry->add_types(msg.types(i));
		huodong_entry->add_value1s(msg.value1s(i));
		huodong_entry->add_value2s(msg.value2s(i));
		huodong_entry->add_value3s(msg.value3s(i));
	}
	POOL_ADD_NEW(huodong_entry->guid(), huodong_entry);
	POOL_SAVE(dhc::huodong_entry_t, huodong_entry, false);
	return huodong_entry;
}



bool HuodongPool::get_ljcz_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond(entry, HUODONG_COND_RMB))
		{
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(entry->arg1());
				sub->set_arg2(output.second);
				sub->set_arg3(output.first);
				ADD_REWARD(sub, entry);
			}
		}
	}
	
	return true;
}

bool HuodongPool::get_dbcz_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond(entry, HUODONG_COND_RMB))
		{
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(entry->arg1());
				sub->set_arg2(output.first);
				sub->set_arg3(output.second);
				sub->set_arg4(entry->arg2());
				sub->set_arg5(entry->arg3());
				ADD_REWARD(sub, entry);
			}
		}
	}

	return true;
}

bool HuodongPool::get_dlsl_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond(entry, HUODONG_COND_DAY))
		{
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(entry->arg1());
				sub->set_arg2(output.second);
				sub->set_arg3(output.first);
				sub->set_arg4(entry->arg2());
				ADD_REWARD(sub, entry);
			}
		}
	}

	return true;
}

bool HuodongPool::get_hyhd_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond_range(entry, HUODONG_COND_NORMAL_COUNT, HUODONG_COND_FUBEN_COUNT))
		{
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(entry->cond());
				sub->set_arg2(output.first);
				sub->set_arg3(output.second);
				sub->set_arg4(entry->arg1());
				ADD_REWARD(sub, entry);
			}
		}
	}

	return true;
}

bool HuodongPool::get_zkfs_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond(entry, HUODONG_COND_JEWEL))
		{
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(entry->arg1());
				sub->set_arg2(output.second);
				sub->set_arg3(entry->arg2());
				sub->set_arg4(entry->arg3());
				sub->set_arg5(entry->arg4());
				ADD_REWARD(sub, entry);
			}
		}
	}

	return true;
}

bool HuodongPool::get_dhhd_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	uint64_t now_time = game::timer()->now();
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond(entry, HUODONG_COND_ITEM) &&
			now_time > entry->show_time())
		{
			
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(output.second);
				sub->set_arg2(entry->arg1());
				sub->set_arg3(entry->arg2());
				for (int m = 0; m < entry->arg6_size(); ++m)
				{
					sub->add_arg6(entry->arg6(m));
					sub->add_arg7(entry->arg7(m));
					sub->add_arg8(entry->arg8(m));
				}
				ADD_REWARD(sub, entry);
			}
		}
	}

	return true;
}

bool HuodongPool::get_rqdl_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond(entry, HUODONG_COND_RIQI))
		{
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(entry->arg2());
				sub->set_arg2(entry->arg3());
				sub->set_arg3(output.first);
				sub->set_arg4(output.second);
				sub->set_arg5(entry->arg4());
				ADD_REWARD(sub, entry);
			}
		}
	}

	return true;
}

bool HuodongPool::get_ykjj_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond(entry, HUODONG_COND_DAY))
		{
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(entry->arg1());
				sub->set_arg2(entry->arg2());
				sub->set_arg3(output.first);
				sub->set_arg4(output.second);
				ADD_REWARD(sub, entry);
			}
		}
	}
	return true;
}

bool HuodongPool::get_zchd_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const
{	
	const dhc::huodong_t *huodong = get_huodong(id);
	if (!huodong)
	{
		return false;
	}
	if (huodong->type() != type && huodong->subtype() != type)
	{
		return false;
	}
	smsg.set_name(huodong->name());

	const dhc::huodong_entry_t *entry = 0;
	protocol::game::huodong_reward_sub *sub = 0;
	HuodongArgResult output;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (is_cond(entry, HUODONG_COND_RMB))
		{
			sub = smsg.add_subs();
			if (sub)
			{
				get_huodong_arg(player, entry, output);
				sub->set_id(entry->id());
				sub->set_arg1(entry->arg1());
				sub->set_arg2(entry->arg2());
				sub->set_arg3(entry->arg3());
				sub->set_arg4(output.first);
				ADD_REWARD(sub, entry);
			}
		}
	}
	return true;
}

void HuodongPool::set_huodong_arg(dhc::player_t* player,
	dhc::huodong_entry_t* entry,
	HuodongArgType arg,
	int value)
{
	int index = -1;
	for (int i = 0; i < entry->player_guids_size(); ++i)
	{
		if (entry->player_guids(i) == player->guid())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		entry->add_player_guids(player->guid());
		entry->add_player_arg1s(0);
		entry->add_player_arg2s(0);
		index = entry->player_guids_size() - 1;
	}

	switch (arg)
	{
	case HUODONG_ARG_COUNT:
		entry->set_player_arg1s(index, entry->player_arg1s(index) + value);
		break;
	case HUODONG_ARG_COMPLETE:
		entry->set_player_arg2s(index, entry->player_arg2s(index) + value);
		break;
	default:
		break;
	}
}

void HuodongPool::set_huodong_arg(uint64_t player_guid, dhc::huodong_entry_t* entry, HuodongArgType arg, int value)
{
	int index = -1;
	for (int i = 0; i < entry->player_guids_size(); ++i)
	{
		if (entry->player_guids(i) == player_guid)
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		entry->add_player_guids(player_guid);
		entry->add_player_arg1s(0);
		entry->add_player_arg2s(0);
		index = entry->player_guids_size() - 1;
	}

	switch (arg)
	{
	case HUODONG_ARG_COUNT:
		entry->set_player_arg1s(index, entry->player_arg1s(index) + value);
		break;
	case HUODONG_ARG_COMPLETE:
		entry->set_player_arg2s(index, entry->player_arg2s(index) + value);
		break;
	default:
		break;
	}
}

void HuodongPool::get_huodong_arg(dhc::player_t* player, const dhc::huodong_entry_t* entry, HuodongArgResult& result) const
{
	result.first = 0;
	result.second = 0;

	for (int i = 0; i < entry->player_guids_size(); ++i)
	{
		if (entry->player_guids(i) == player->guid())
		{
			result.first = entry->player_arg1s(i);
			result.second = entry->player_arg2s(i);
			break;
		}
	}
}

int HuodongPool::get_huodong_level(dhc::player_t *player, const dhc::huodong_t* huodong) const
{
	for (int i = 0; i < huodong->player_guids_size(); ++i)
	{
		if (huodong->player_guids(i) == player->guid())
		{
			return huodong->player_levels(i);
		}
	}
	return -1;
}

void HuodongPool::set_huodong_level(dhc::player_t *player, dhc::huodong_t* huodong)
{
	for (int i = 0; i < huodong->player_guids_size(); ++i)
	{
		if (huodong->player_guids(i) == player->guid())
		{
			return;
		}
	}
	huodong->add_player_levels(player->level());
	huodong->add_player_guids(player->guid());
}

dhc::huodong_player_t* HuodongPool::get_huodong_player(dhc::player_t *player, HuodongType type, HuodongType subtype)
{
	dhc::huodong_t* huodong = get_huodong_by_type(type, subtype);
	if (!huodong)
	{
		return 0;
	}

	for (int i = 0; i < huodong->player_players_size(); ++i)
	{
		if (huodong->player_players(i) == player->guid())
		{
			return POOL_GET_HUODONG_PLAYER(huodong->player_subs(i));
		}
	}
	return 0;
}

dhc::huodong_player_t* HuodongPool::get_huodong_player(dhc::player_t *player, dhc::huodong_t *huodong)
{
	for (int i = 0; i < huodong->player_players_size(); ++i)
	{
		if (huodong->player_players(i) == player->guid())
		{
			return POOL_GET_HUODONG_PLAYER(huodong->player_subs(i));
		}
	}


	uint64_t huodong_player_guid = game::gtool()->assign(et_huodong_player);
	dhc::huodong_player_t *huodong_player = new dhc::huodong_player_t();
	huodong_player->set_guid(huodong_player_guid);
	huodong_player->set_huodong_guid(huodong->guid());
	huodong_player->set_player_guid(player->guid());
	huodong->add_player_players(player->guid());
	huodong->add_player_subs(huodong_player_guid);

	if (huodong->type() == HUODONG_XHQD_TYPE &&
		huodong->subtype() == HUODONG_TBHD_TYPE)
	{
		huodong_player->set_arg2(5);
		huodong_player->set_arg4(1);
	}
	else if (huodong->type() == HUODONG_XHQD_TYPE &&
		huodong->subtype() == HUODONG_CZFP_TYPE)
	{
		for (int i = 0; i < 8; ++i)
		{
			huodong_player->add_args1(-1);
			huodong_player->add_args2(-1);
		}
	}
	else if (huodong->type() == HUODONG_XHQD_TYPE &&
		huodong->subtype() == HUODONG_JGMF_TYPE)
	{
		std::vector<int> ids;
		HuodongOperation::refresh_mofang(ids);
		for (int i = 0; i < ids.size(); ++i)
		{
			huodong_player->add_args1(ids[i]);
			huodong_player->add_args2(0);
		}
	}
	
	POOL_ADD_NEW(huodong_player_guid, huodong_player);
	POOL_SAVE(dhc::huodong_player_t, huodong_player, false);

	return huodong_player;
}

bool HuodongPool::has_rqdl_point(dhc::player_t *player, dhc::huodong_t *huodong) const
{
	dhc::huodong_entry_t *entry = 0;
	HuodongArgResult result;
	int monsth = game::timer()->month();
	int day = game::timer()->day();
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (entry &&
			entry->arg2() == monsth &&
			entry->arg3() == day)
		{
			get_huodong_arg(player, entry, result);
			if (result.second <= 0 && result.first > 0)
			{
				return true;
			}
			return false;
		}
	}
	return false;
}

bool HuodongPool::has_dbcz_point(dhc::player_t *player, dhc::huodong_t *huodong) const
{
	dhc::huodong_entry_t *entry = 0;
	HuodongArgResult result;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (entry)
		{
			get_huodong_arg(player, entry, result);
			if (result.second < result.first)
			{
				return true;
			}
		}
	}
	return false;
}

bool HuodongPool::has_hyhd_point(dhc::player_t *player, dhc::huodong_t *huodong) const
{
	dhc::huodong_entry_t *entry = 0;
	HuodongArgResult result;
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (entry)
		{
			get_huodong_arg(player, entry, result);
			if (result.second <= 0 &&
				result.first >= entry->arg1())
			{
				return true;
			}
		}
	}
	return false;
}

bool HuodongPool::has_dhhd_point(dhc::player_t *player, dhc::huodong_t *huodong) const
{
	dhc::huodong_entry_t *entry = 0;
	HuodongArgResult result;
	uint64_t now_time = game::timer()->now();
	for (int i = 0; i < huodong->entrys_size(); ++i)
	{
		entry = POOL_GET_HUODONG_ENTRY(huodong->entrys(i));
		if (entry && now_time > entry->show_time())
		{
			get_huodong_arg(player, entry, result);
			if (result.second < entry->arg1())
			{
				bool has = true;
				for (int i = 0; i < entry->arg6_size(); ++i)
				{
					if (ItemOperation::item_num_templete(player, entry->arg7(i)) < entry->arg8(i))
					{
						has = false;
						break;
					}
				}
				if (has)
				{
					return true;
				}
			}
		}
	}
	return false;
}

void HuodongPool::tanbao_rank_reward(dhc::huodong_t *huodong)
{
	if (game::timer()->now() > huodong->rank_time())
	{
		dhc::rank_t * rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_tanbao_normal));
		if (rank && rank->reward_flag() == 0)
		{
			rank->set_reward_flag(1);

			const s_t_tanbao_reward* t_reward = 0;
			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				std::string sender;
				std::string title;
				std::string text;
				t_reward = sHuodongConfig->get_t_tanbao_reward(i + 1, 1);
				if (!t_reward)
				{
					continue;
				}			
				int lang_ver = game::channel()->get_channel_lang(rank->player_achieve(i));
				game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
				game::scheme()->get_server_str(lang_ver, title, "tanbao_normal_reward_title");
				game::scheme()->get_server_str(lang_ver, text, "tanbao_normal_reward_text", i + 1);
				PostOperation::post_create(rank->player_guid(i), title, text, sender, t_reward->rewards);

			}
		}

		rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_tanbao_jy));
		if (rank && rank->reward_flag() == 0)
		{
			rank->set_reward_flag(1);

			const s_t_tanbao_reward* t_reward = 0;
			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				std::string sender;
				std::string title;
				std::string text;
				t_reward = sHuodongConfig->get_t_tanbao_reward(i + 1, 2);
				if (!t_reward)
				{
					continue;
				}
				int lang_ver = game::channel()->get_channel_lang(rank->player_guid(i));
				game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
				game::scheme()->get_server_str(lang_ver, title, "tanbao_jy_reward_title");
				game::scheme()->get_server_str(lang_ver, text, "tanbao_jy_reward_text", i + 1);
				PostOperation::post_create(rank->player_guid(i), title, text, sender, t_reward->rewards);

			}
		}
	}
	
}

void HuodongPool::zhuanpan_rank_reward(dhc::huodong_t *huodong)
{
	if (game::timer()->now() > huodong->rank_time())
	{
		dhc::rank_t * rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_zhuanpan_normal));
		if (rank && rank->reward_flag() == 0)
		{
			rank->set_reward_flag(1);

			std::string sender;
			std::string title;
			std::string text;

			const s_t_zhuanpan_reward* t_reward = 0;
			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				t_reward = sHuodongConfig->get_t_zhuanpan_reward(i + 1, 1);
				if (!t_reward)
				{
					continue;
				}
				int lang_ver = game::channel()->get_channel_lang(rank->player_guid(i));
				game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
				game::scheme()->get_server_str(lang_ver, title, "zhuanpan_normal_reward_title");
				game::scheme()->get_server_str(lang_ver, text, "zhuanpan_normal_reward_text", i + 1);
				PostOperation::post_create(rank->player_guid(i), title, text, sender, t_reward->rewards);

			}
		}

		rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_zhuanpan_jy));
		if (rank && rank->reward_flag() == 0)
		{
			rank->set_reward_flag(1);
			std::string sender;
			std::string title;
			std::string text;

			const s_t_zhuanpan_reward* t_reward = 0;;
			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				std::string sender;
				std::string title;
				std::string text;
				t_reward = sHuodongConfig->get_t_zhuanpan_reward(i + 1, 2);
				if (!t_reward)
				{
					continue;
				}
				int lang_ver = game::channel()->get_channel_lang(rank->player_guid(i));
				game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
				game::scheme()->get_server_str(lang_ver, title, "zhuanpan_jy_reward_title");
				game::scheme()->get_server_str(lang_ver, text, "zhuanpan_jy_reward_text", i + 1);
				PostOperation::post_create(rank->player_guid(i), title, text, sender, t_reward->rewards);
			}
		}
	}
}


void HuodongPool::tansuo_rank_reward(dhc::huodong_t *huodong)
{
	if (game::timer()->now() > huodong->rank_time())
	{
		dhc::rank_t * rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_tansuo_normal));
		if (rank && rank->reward_flag() == 0)
		{
			rank->set_reward_flag(1);

			const s_t_tansuo_reward* t_reward = 0;
			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				std::string sender;
				std::string title;
				std::string text;
				t_reward = sHuodongConfig->get_t_tansuo_reward(i + 1, 1);
				if (!t_reward)
				{
					continue;
				}
				int lang_ver = game::channel()->get_channel_lang(rank->player_guid(i));
				game::scheme()->get_server_str(lang_ver, title, "tansuo_normal_reward_title");
				game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
				game::scheme()->get_server_str(lang_ver, text, "tansuo_normal_reward_text", i + 1);
				PostOperation::post_create(rank->player_guid(i), title, text, sender, t_reward->rds);
			}
		}

		/*rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_tansuo_jy));
		if (rank && rank->reward_flag() == 0)
		{
			rank->set_reward_flag(1);
			std::string sender;
			game::scheme()->get_server_str(sender, "sys_sender");
			std::string title;
			std::string text;

			const s_t_tansuo_reward* t_reward = 0;
			game::scheme()->get_server_str(title, "tansuo_jy_reward_title");
			for (int i = 0; i < rank->player_guid_size(); ++i)
			{
				t_reward = sHuodongConfig->get_t_tansuo_reward(i + 1, 2);
				if (!t_reward)
				{
					continue;
				}

				game::scheme()->get_server_str(text, "tansuo_jy_reward_text", i + 1);
				PostOperation::post_create(rank->player_guid(i), title, text, sender, t_reward->rds);
			}
		}*/
	}
}

void HuodongPool::mofang_reward(dhc::huodong_t *huodong)
{
	if (game::timer()->now() < huodong->rank_time() + 300000)
	{
		query_mofang_count_ += 1;
		if (query_mofang_count_ >= 20)
		{
			query_mofang_count_ = 0;
			rpcproto::tmsg_req_mofang_point rmsg;
			rmsg.set_point(0);
			rmsg.set_huodong_time(huodong->end());

			if (huodong->extra_data1() > 0 && huodong->extra_data2() <= 0)
			{
				huodong->set_extra_data2(huodong->extra_data1());
				huodong->set_extra_data1(0);

			}
			if (huodong->extra_data2() > 0)
			{
				rmsg.set_point(huodong->extra_data2());
			}

			std::string s;
			rmsg.SerializeToString(&s);
			game::rpc_service()->request("remote1", PMSG_MOFANG_POINT, s,
				boost::bind(&HuodongPool::mofang_reward_callback, this, _1, huodong->guid()));
		}
	}
	
}

void HuodongPool::mofang_reward_callback(const std::string &data, uint64_t huodong_guid)
{
	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_guid);
	if (!huodong)
	{
		return;
	}

	rpcproto::tmsg_rep_mofang_point msg;
	if (!msg.ParseFromString(data))
	{
		if (huodong->extra_data2() > 0)
		{
			huodong->set_extra_data1(huodong->extra_data1() + huodong->extra_data2());
			huodong->set_extra_data2(0);
		}
		return;
	}

	if (msg.point() < 0)
	{
		if (huodong->extra_data2() > 0)
		{
			huodong->set_extra_data1(huodong->extra_data1() + huodong->extra_data2());
			huodong->set_extra_data2(0);
		}
		return;
	}

	huodong->set_extra_data(msg.point());
	huodong->set_extra_data2(0);
}











