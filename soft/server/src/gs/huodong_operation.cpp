#include "huodong_operation.h"
#include "huodong_config.h"
#include "equip_operation.h"
#include "role_operation.h"
#include "rank_operation.h"
#include "player_operation.h"
#include "treasure_operation.h"
#include "global_pool.h"
#include "utils.h"

enum KaiFuHuodongType
{
	KF_LOGIN = 1,					/// ����
	KF_RECHARE = 2,					/// �ۼƳ�ֵdef1
	KF_NORMAL_MISSION = 3,			/// ��ͨ�ؿ���def1
	KF_JINYIN_MISSION = 4,			/// ��Ӣ�ؿ�def1
	KF_LEVEL = 5,					/// �ȼ�def1
	KF_EQUIP_COLOR = 6,				/// 24��װ��ȫ���ﵽdef1Ʒ��
	KF_EQUIP_ENHANCE = 7,			/// 24��װ��ȫ��ǿ����def1
	KF_SPORT_RANK = 8,				/// ������������def1
	KF_ROLE_HAS = 9,				/// ӵ��def1��def1Ʒ�����ϵĻ��
	KF_ROLE_TUPO = 10,				/// 6�����ȫ��ͻ�Ƶ�def1��
	KF_ROLE_MAX_TUPO = 11,			/// ��߳ɳ�ͻ�Ƶ�def1��
	KF_BOSS_MAX_DAM = 12,			/// BOSSս����˺���def1
	KF_BOSS_RANK = 13,				/// BOSSս�ﵽ��def1��
	KF_XINHE_RESET = 14,			/// �Ǻ��ؾ�����def1�� 
	KF_XINHE_RANK = 15,				/// �Ǻ��ؾ���������def1
	KF_GAIZAO_COUNT = 16,			/// 24��װ��ȫ������def1������
	KF_GAIZAO_MAX = 17,				/// ��߸����def1Ʒ�ʵ�����
	KF_SHOP_REFRESH = 18,			/// �̵�ˢ��def1��
	KF_SHOP_BUY = 19,				/// �̵깺��def1�� 
	KF_XINHE_STAR = 20,				/// �Ǻ��ؾ��ﵽdef1��
	KF_BAOSHI = 21,					/// ȫ��24����def1����ʯ
	KF_BF_TUPO = 22,				/// ս����ͻ��def1
	KF_SPEND = 23,					/// �ۼ�����def1
	KF_ROLE_ALL_EQUIP = 24,			/// ������ȫ��װ��װ��
	KF_ZHANWEI1 = 25,
	KF_JJC_SUC_NUM = 26,			/// ������ʤ������
	
	KF_YB_LANJIE = 32,				/// Ѻ������
	KF_REFRESH_HBB = 34,			/// ˢ�¾�Ԯ���
	KF_TREASURE_HECHENG = 35,		/// �ϳ�def1������ 
	KF_TREASURE_HECHENG_COLOR,		/// �ϳ�def1��def2Ʒ�ʵı���
	KF_EQUIP_JINLIAN,				/// ����def1�����ȫ����������def���ϵ�װ��
	KF_EQUIP_JINLIAN_MAX,			/// ������װ������߾����ȼ�����def1��
	KF_ROLE_SKILL,					/// ����def1�����ȫ������ǿ�ȴﵽdef2
	KF_ROLE_SKILL_MAX,				/// ����6������������ǿ�ȴﵽdef1
	KF_BOSS_LEIJI_DAM,				/// ħ���ַ�ս�ۼ��˺��ﵽdef1
	KF_TREASURE_JINLIAN,			/// ����def1�����ȫ����������def���ϵ�װ������
	KF_TREASURE_JINLIAN_MAX,		/// �����ı�������߾����ȼ�����def1��

	KF_PTFB_SUC_COUNT = 51,			/// ���߸���ʤ��def1��
	KF_JYFB_SUC_COUNT,				/// ��Ӣ����ʤ��def2��
	KF_JRDB_SUC_COUNT,				/// �ᱦdef1��
	KF_JJC_SUC_COUNT, 				/// ������ʤ��def1��
	KF_DBCZ_SUC_COUNT,				/// ���ʳ�ֵdef1��


	KF_ROLE_SELECT = 100,			/// ���4ѡ1
	KF_XG		   = 200,			/// �޹�
	KF_ZKFS		   = 300,			/// �ۿ۷���
};

int HuodongOperation::get_huodong_pttq_index(dhc::player_t *player, int index)
{
	for (int i = 0; i < player->huodong_pttq_reward_size(); ++i)
	{
		if (player->huodong_pttq_reward(i) == index)
		{
			return i;
		}
	}
	return -1;
}

void HuodongOperation::check_pttq_vip(dhc::player_t *player)
{
	int min_vip = sHuodongConfig->get_t_huodong_pttq_min_vip();
	if (player->vip() < min_vip)
	{
		return;
	}

	for (int i = min_vip; i <= player->vip(); ++i)
	{
		const s_t_huodong_pttq *t_huodong_pttq = sHuodongConfig->get_t_huodong_pttq_by_vip(i);
		if (!t_huodong_pttq)
		{
			continue;
		}
		sGlobalPool->update_pttq(player->name(), t_huodong_pttq->id, i);
	}
}

int HuodongOperation::get_kaifu_huodong_count(dhc::player_t *player, int id)
{
	const s_t_huodong_kf_mubiao * t_mubiao = sHuodongConfig->get_t_huodong_kf_mubiao(id);
	if (t_mubiao)
	{
		switch (t_mubiao->type)
		{
		case KF_LOGIN:
		{
			return 1;
		}
		case KF_RECHARE:
		{
			return player->total_recharge();
		}
		break;
		case KF_NORMAL_MISSION:
		{
			return (player->mission() >= t_mubiao->def1) ? 1: 0;
		}
		break;
		case KF_JINYIN_MISSION:
		{
			return (player->mission_jy() >= t_mubiao->def1) ? 1 : 0;
		}
		break;
		case KF_LEVEL:
		{
			return player->level();
		}
		break;
		case KF_EQUIP_COLOR:
		{
			return EquipOperation::get_equip_color_count(player, t_mubiao->def1);
		}
		break;
		case KF_EQUIP_ENHANCE:
		{
			return EquipOperation::get_equip_enhance_count(player, t_mubiao->def1);
		}
		break;
		case KF_SPORT_RANK:
		{
			int rank = player->max_rank(); 
			if (rank <= 0)
			{
				rank = 99999999;
			}
			return (rank <= t_mubiao->def1) ? 1 : 0;
		}
		break;
		case KF_ROLE_HAS:
		{
			return RoleOperation::get_role_color_count(player, t_mubiao->def2);
		}
		break;
		case KF_ROLE_TUPO:
		{
			return RoleOperation::get_role_tupo_count(player, t_mubiao->def1);
		}
		break;
		case KF_ROLE_MAX_TUPO:
		{
			return RoleOperation::get_role_tupo_max(player, t_mubiao->def1);
		}
		break;
		case KF_BOSS_MAX_DAM:
		{
			return player->boss_max_damage();
		}
		break;
		case KF_BOSS_RANK:
		{
			return (player->boss_max_rank() <= t_mubiao->def1) ? 1 : 0;
		}
		break;
		case KF_XINHE_RESET:
		{
			return player->ttt_cz_num();
		}
		break;
		case KF_XINHE_RANK:
		{
			return (RankOperation::get_player_rank(player, e_rank_type_ttt) <= t_mubiao->def1) ? 1 : 0;
		}
		break;
		case KF_GAIZAO_COUNT:
		{
			return EquipOperation::get_equip_gaizao_count(player, t_mubiao->def1);
		}
		break;
		case KF_GAIZAO_MAX:
		{
			return EquipOperation::get_equip_gaizao_color_max(player, t_mubiao->def1);
		}
		break;
		case KF_SHOP_REFRESH:
		{
			return player->shop_refresh_num();
		}
		break;
		case KF_SHOP_BUY:
		{
			return player->shop_buy_num();
		}
		break;
		case KF_XINHE_STAR:
		{
			int num = 0;
			for (int i = 0; i < player->ttt_last_stars_size(); ++i)
			{
				num += player->ttt_last_stars(i);
			}
			return num;
		}
		break;
		case KF_BAOSHI:
		{
			return EquipOperation::get_equip_baoshi_count(player, t_mubiao->def1);
		}
		break;
		case KF_BF_TUPO:
		{
			return player->bf();
		}
		break;
		case KF_ROLE_SELECT:
		{
			return 1;
		}
		break;
		case KF_XG:
		{
			return sGlobalPool->get_kaifu_xg_count(id);
		}
		break;
		case KF_ZKFS:
		{
			return get_kaifu_huodong_id_count(player, id);
		}
		break;
		case KF_SPEND:
		{
			return player->total_spend();
		}
		case KF_ROLE_ALL_EQUIP:
		{
			return RoleOperation::get_role_equip_count(player, t_mubiao->def1);
		}
		break;
		case KF_JJC_SUC_NUM:
		{
			return player->jjcs_task_num();
		}
		break;
		case KF_YB_LANJIE:
		{
			return player->yb_task_num();
		}
		break;
		case KF_REFRESH_HBB:
		{
			return player->bweh_task_num();
		}
		break;
		case KF_TREASURE_HECHENG:
		{
			return player->treasure_hechengs_size();
		}
		break;
		case KF_TREASURE_HECHENG_COLOR:
		{
			return TreasureOperation::get_treasure_hecheng_count(player, t_mubiao->def2);
		}
		break;
		case KF_EQUIP_JINLIAN:
		{
			return EquipOperation::get_equip_jinlian_count(player, t_mubiao->def1);
		}
		break;
		case KF_EQUIP_JINLIAN_MAX:
		{
			return EquipOperation::get_equip_jinlian_max(player, t_mubiao->def1);
		}
		break;
		case KF_ROLE_SKILL:
		{
			return RoleOperation::get_role_skill_count(player, t_mubiao->def1);
		}
		break;
		case KF_ROLE_SKILL_MAX:
		{
			return RoleOperation::get_role_skill_max(player, t_mubiao->def1);
		}
		break;
		case KF_BOSS_LEIJI_DAM:
		{
			return (int)player->boss_leiji_damage();
		}
		break;
		case KF_TREASURE_JINLIAN:
		{
			return TreasureOperation::get_treasure_jinlian_count(player, t_mubiao->def1);
		}
		break;
		case KF_TREASURE_JINLIAN_MAX:
		{
			return TreasureOperation::get_treasure_jinlian_max(player, t_mubiao->def1);
		}
		break;
		case KF_PTFB_SUC_COUNT:
		{
			return player->huodong_kaifu_ptfb();
		}
		break;
		case KF_JYFB_SUC_COUNT:
		{
			return player->huodong_kaifu_jyfb();
		}
		break;
		case KF_JRDB_SUC_COUNT:
		{
			return player->huodong_kaifu_dbfb();
		}
		break;
		case KF_JJC_SUC_COUNT:
		{
			return player->huodong_kaifu_jjc();
		}
		break;
		case KF_DBCZ_SUC_COUNT:
		{
			for (int km = 0; km < player->huodong_kaifu_dbcz_size(); ++km)
			{
				if (player->huodong_kaifu_dbcz(km) == t_mubiao->def1)
				{
					return t_mubiao->def1;
				}
			}
			return 0;
		}
		break;
		default:
			break;
		}
	}

	return 0;
}

int HuodongOperation::get_kaifu_huodong_id(dhc::player_t *player, int id)
{
	for (int i = 0; i < player->huodong_kaifu_finish_ids_size(); ++i)
	{
		if (player->huodong_kaifu_finish_ids(i) == id)
		{
			return i;
		}
	}
	return -1;
}

int HuodongOperation::get_kaifu_huodong_id_count(dhc::player_t *player, int id)
{
	int count = 0;
	for (int i = 0; i < player->huodong_kaifu_finish_ids_size(); ++i)
	{
		if (player->huodong_kaifu_finish_ids(i) == id)
		{
			count++;
		}
	}
	return count;
}

bool HuodongOperation::has_kaifu_huodong_complete(dhc::player_t *player)
{
	int day = game::timer()->run_day(player->birth_time()) + 1;

	if (day >= 14)
	{
		return false;
	}

	for (int i = 1; i <= day; ++i)
	{
		std::vector<int> ids;
		sHuodongConfig->get_t_huodong_kf(i, ids);
		for (int j = 0; j < ids.size(); ++j)
		{
			const s_t_huodong_kf_mubiao * t_mubiao = sHuodongConfig->get_t_huodong_kf_mubiao(ids[j]);
			if (t_mubiao)
			{
				if (get_kaifu_huodong_id(player, ids[j]) == -1)
				{
					int count = get_kaifu_huodong_count(player, ids[j]);
					if (t_mubiao->type != KF_XG && count >= t_mubiao->cankao)
					{
						return true;
					}
					else if (t_mubiao->type == KF_XG)
					{
						const s_t_huodong_kf *t_kaifu = sHuodongConfig->get_t_huodong_kf(ids[j]);
						if (t_kaifu && count < t_kaifu->count)
						{
							return true;
						}
					}
				}
			}
		}
	}

	return false;
}



bool HuodongOperation::has_pttq_huodong_complete(dhc::player_t *player)
{
	const std::map<int, s_t_huodong_pttq>& pttq = sHuodongConfig->get_t_all_huodong_pttq();
	int index = 0;
	for (std::map<int, s_t_huodong_pttq>::const_iterator it = pttq.begin();
		it != pttq.end();
		++it)
	{
		if (sGlobalPool->is_enable_pttq(it->first))
		{
			const s_t_huodong_pttq &pq = it->second;
			for (std::map<int, std::vector<s_t_reward> >::const_iterator jt = pq.rewards.begin();
				jt != pq.rewards.end();
				++jt)
			{
				index = it->first * 100 + jt->first;
				if (player->vip() >= jt->first &&
					HuodongOperation::get_huodong_pttq_index(player, index) == -1)
				{
					return true;
				}
			}
		}
	}

	return false;
}


int HuodongOperation::get_huodong_week_libao(dhc::player_t *player, int id)
{
	int count = 0;
	for (int i = 0; i < player->huodong_week_libao_size(); ++i)
	{
		if (player->huodong_week_libao(i) == id)
		{
			count++;
		}
	}
	return count;
}

dhc::player_t* HuodongOperation::get_tansuo_boss(dhc::player_t *player)
{
	std::vector<uint64_t> target_guids;
	game::pool()->get_entitys(et_player, target_guids);

	int64_t bf_max = int64_t(player->bf()) * 110 / 100;
	int64_t bf_min = int64_t(player->bf()) * 50 / 100;
	int64_t bf_me = int64_t(player->bf());
	int64_t bf = 0;
	std::vector<dhc::player_t*> fits;
	std::vector<dhc::player_t*> mins;
	std::vector<dhc::player_t*> maxs;

	dhc::player_t *target = 0;
	for (int i = 0; i < target_guids.size(); ++i)
	{
		target = POOL_GET_PLAYER(target_guids[i]);
		if (target && target->guid() != player->guid())
		{
			bf = int64_t(target->bf());
			if (bf >= bf_min &&
				bf <= bf_max)
			{
				fits.push_back(target);
			}
			else if (bf_me >= bf)
			{
				mins.push_back(target);
			}
			else if (bf_me < bf)
			{
				maxs.push_back(target);
			}
		}
	}

	if (fits.size() > 0)
	{
		target = fits[Utils::get_int32(0, fits.size() - 1)];
	}
	else if (mins.size() > 0)
	{
		target = mins[Utils::get_int32(0, mins.size() - 1)];
	}
	else if (maxs.size() > 0)
	{
		target = maxs[Utils::get_int32(0, maxs.size() - 1)];
	}
	else
	{
		target = player;
	}

	return target;
}

bool HuodongOperation::refresh_mofang(std::vector<int>& idss)
{
	std::vector<int> ids;
	bool wrong = false;

	const s_t_mofang *t_mofang = sHuodongConfig->get_t_mofang_refresh(1);
	if (!t_mofang)
	{
		ids.push_back(0);
		wrong = true;
	}
	else
	{
		ids.push_back(t_mofang->id);
	}

	for (int i = 0; i < 5; ++i)
	{
		t_mofang = sHuodongConfig->get_t_mofang_refresh(3);
		if (!t_mofang)
		{
			ids.push_back(0);
			wrong = true;
		}
		else
		{
			ids.push_back(t_mofang->id);
		}
	}
	
	if (Utils::get_int32(0, 99) < 30)
	{
		t_mofang = sHuodongConfig->get_t_mofang_refresh(1);
		if (!t_mofang)
		{
			ids.push_back(0);
			wrong = true;
		}
		else
		{
			ids.push_back(t_mofang->id);
		}

		for (int i = 0; i < 2; ++i)
		{
			t_mofang = sHuodongConfig->get_t_mofang_refresh(2);
			if (!t_mofang)
			{
				ids.push_back(0);
				wrong = true;
			}
			else
			{
				ids.push_back(t_mofang->id);
			}
		}
	}
	else
	{
		for (int i = 0; i < 3; ++i)
		{
			t_mofang = sHuodongConfig->get_t_mofang_refresh(2);
			if (!t_mofang)
			{
				ids.push_back(0);
				wrong = true;
			}
			else
			{
				ids.push_back(t_mofang->id);
			}
		}
	}
	

	Utils::get_vector(ids, 9, idss);

	return wrong;
}
