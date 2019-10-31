#include "equip_operation.h"
#include "equip_config.h"
#include "item_config.h"
#include "utils.h"

dhc::equip_t *  EquipOperation::equip_create(dhc::player_t *player, uint32_t equip_id, int enhance, int jl, int mode)
{
	s_t_equip *t_equip = sEquipConfig->get_equip(equip_id);
	if (!t_equip)
	{
		return 0;
	}
	uint64_t equip_guid = game::gtool()->assign(et_equip);
	dhc::equip_t *equip = new dhc::equip_t();
	equip->set_guid(equip_guid);
	equip->set_player_guid(player->guid());
	equip->set_template_id(equip_id);
	equip->set_enhance(enhance);
	equip->set_jilian(jl);
	for (int i = 0; i < t_equip->slot_num; ++i)
	{
		equip->add_stone(0);
	}
	POOL_ADD_NEW(equip->guid(), equip);
	player->add_equips(equip_guid);

	LOG_OUTPUT(player, LOG_EQUIP, equip_id, 1, mode, LOG_ADD);
	return equip;
}

void EquipOperation::equip_delete(dhc::player_t *player, uint64_t equip_guid, int mode)
{
	dhc::equip_t *equip = POOL_GET_EQUIP(equip_guid);
	if (!equip)
	{
		return;
	}
	int equip_id = equip->template_id();
	POOL_REMOVE(equip_guid, player->guid());
	for (int i = 0; i < player->equips_size(); ++i)
	{
		if (player->equips(i) == equip_guid)
		{
			for (int j = i; j < player->equips_size() - 1; ++j)
			{
				player->set_equips(j, player->equips(j + 1));
			}
			player->mutable_equips()->RemoveLast();
			break;
		}
	}

	LOG_OUTPUT(player, LOG_EQUIP, equip_id, 1, mode, LOG_DEC);
}

bool EquipOperation::is_equip_full(dhc::player_t *player)
{
	int num = 0;
	for (int i = 0; i < player->equips_size(); ++i)
	{
		dhc::equip_t *equip = POOL_GET_EQUIP(player->equips(i));
		if (!equip)
		{
			continue;
		}
		if (equip->role_guid() == 0)
		{
			num++;
		}
	}
	if (num >= EquipOperation::get_equip_slot(player))
	{
		return true;
	}
	return false;
}

int EquipOperation::get_equip_slot(dhc::player_t *player)
{
	//return EQUIP_SLOT + player->equip_kc_num() * 10;

	return EQUIP_SLOT;
}

bool EquipOperation::player_has_dress(dhc::player_t *player, int dress_id)
{
	for (int i = 0; i < player->dress_ids_size(); ++i)
	{
		if (dress_id == player->dress_ids(i))
		{
			return true;
		}
	}
	return false;
}

void EquipOperation::player_dress_check_all(dhc::player_t *player)
{
	/// 返还图纸
	if (player->dress_flag() == 0)
	{
		player->set_dress_flag(1);

		int num = player->mission() / 1000;
		if (num > 0)
		{
			player->set_dress_tuzhi(player->dress_tuzhi() + num);
		}
	}
}

int EquipOperation::get_equip_color_count(dhc::player_t *player, int color)
{
	int count = player->zhenxing_size();
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (equip)
				{
					s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
					/// 没有达到条件
					if (t_equip && t_equip->font_color < color)
					{
						count--;
						break;
					}
				}
				/// 没有装备
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

int EquipOperation::get_equip_enhance_count(dhc::player_t *player, int enhance)
{
	int count = player->zhenxing_size();
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (equip)
				{
					/// 没有达到条件
					if (equip->enhance() < enhance)
					{
						count--;
						break;
					}
				}
				/// 没有装备
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

int EquipOperation::get_equip_jinlian_count(dhc::player_t *player, int jilian)
{
	int count = player->zhenxing_size();
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (equip)
				{
					/// 没有达到条件
					if (equip->jilian() < jilian)
					{
						count--;
						break;
					}
				}
				/// 没有装备
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

int EquipOperation::get_equip_jinlian_max(dhc::player_t *player, int jilian)
{
	int jinlian_max = 0;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (equip && equip->jilian() > jinlian_max)
				{
					jinlian_max = equip->jilian();
				}
			}
		}
	}
	return jinlian_max;
}

int EquipOperation::get_equip_gaizao_count(dhc::player_t *player, int count)
{
	int role_count = player->zhenxing_size();
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (equip)
				{
					/// 不满足条件
					if (equip->rand_ids_size() < count)
					{
						role_count--;
						break;
					}
				}
				/// 没有装备
				else
				{
					role_count--;
					break;
				}
			}
		}
		/// 没有角色
		else
		{
			role_count--;
		}
	}
	return role_count;
}

int EquipOperation::get_equip_gaizao_color_max(dhc::player_t *player, int color)
{
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (!equip)
				{
					continue;
				}
				s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
				if (!t_equip)
				{
					continue;
				}
				s_t_equip_sx *t_equip_sx = sEquipConfig->get_equip_sx(t_equip->font_color, equip->star());
				if (!t_equip_sx)
				{
					continue;
				}
				for (int k = 0; k < equip->rand_ids_size(); ++k)
				{
					if (k >= equip->rand_values_size())
					{
						continue;
					}
					int index = equip->rand_ids_size() - 1;
					double max = t_equip->eeattr[index].value2 + t_equip->eeattr[index].value2 * t_equip_sx->enhance_rate * equip->enhance();
					int ecolor = 4;
					if (equip->rand_values(k) / max <= 0.4f)
					{
						ecolor = 1;
					}
					else if (equip->rand_values(k) / max <= 0.7f)
					{
						ecolor = 2;
					}
					else if (equip->rand_values(k) / max <= 0.9f)
					{
						ecolor = 3;
					}

					if (ecolor >= color)
					{
						return 1;
					}
				}
			}
		}
	}

	return 0;
}

int EquipOperation::get_equip_baoshi_count(dhc::player_t *player, int baoshi_level)
{
	int count = player->zhenxing_size();
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			for (int j = 0; j < role->zhuangbeis_size(); ++j)
			{
				dhc::equip_t *equip = POOL_GET_EQUIP(role->zhuangbeis(j));
				if (equip)
				{
					bool has_baoshi = false;
					for (int k = 0; k < equip->stone_size(); ++k)
					{
						s_t_item *t_item = sItemConfig->get_item(equip->stone(k));
						if (t_item)
						{
							has_baoshi = true;
							/// 不满足条件
							if (t_item->def3 < baoshi_level)
							{
								has_baoshi = false;
								break;
							}
						}
						else
						{
							has_baoshi = false;
							break;
						}
					}
					/// 没有宝石
					if (!has_baoshi)
					{
						count--;
						break;
					}
				}
				/// 没有装备
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

bool EquipOperation::get_enhance_return(dhc::player_t *player, dhc::equip_t *equip, s_t_rewards& rds)
{
	const s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!t_equip)
	{
		return false;
	}
	
	int gold = 0;
	for (int i = 1; i <= equip->enhance(); ++i)
	{
		gold += sEquipConfig->get_enhance(t_equip->font_color, i);
	}

	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}
	return true;
}

bool EquipOperation::get_jinlian_return(dhc::player_t *player, dhc::equip_t *equip, s_t_rewards& rds)
{
	const s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!equip)
	{
		return false;
	}

	int gold = 0;
	int item_num = 0;
	const s_t_equip_jl *t_jinlian = 0;
	for (int i = 1; i <= equip->jilian(); ++i)
	{
		t_jinlian = sEquipConfig->get_equip_jl(i);
		if (!t_jinlian)
		{
			return false;
		}

		int color = t_equip->font_color - 1;
		if (color < 0 || color >= t_jinlian->consume.size())
		{
			return false;
		}

		gold += t_jinlian->consume[color].second;
		item_num += t_jinlian->consume[color].first;
	}
	
	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}
	if (item_num > 0)
	{
		rds.add_reward(2, 50070001, item_num);
	}

	return true;
}

bool EquipOperation::get_gaizao_return(dhc::player_t *player, dhc::equip_t *equip, s_t_rewards& rds)
{
	if (equip->gaizao_counts() <= 0)
	{
		return true;
	}
	rds.add_reward(2, 50010001, equip->gaizao_counts());
	
	return true;
}

bool EquipOperation::get_shengxin_return(dhc::player_t *player, dhc::equip_t *equip, s_t_rewards& rds)
{
	s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
	if (!t_equip)
	{
		return false;
	}

	const s_t_equip_suipian* t_equip_suipian = sEquipConfig->get_equip_suipian(equip->template_id());
	if (!t_equip_suipian)
	{
		return false;
	}

	int gold = 0;
	int item_num = 0;
	s_t_equip_sx *t_equip_sx = 0;
	for (int j = 1; j <= equip->star(); ++j)
	{
		t_equip_sx = sEquipConfig->get_equip_sx(t_equip->font_color, j);
		if (!t_equip_sx)
		{
			return false;
		}
		gold += t_equip_sx->gold;
		item_num += t_equip_sx->suipian;
	}

	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}
	if (item_num > 0)
	{
		rds.add_reward(2, t_equip_suipian->suipian_id, item_num);
	}

	return true;
}
