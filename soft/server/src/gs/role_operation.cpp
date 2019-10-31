#include "role_operation.h"
#include "player_config.h"
#include "role_config.h"
#include "item_config.h"
#include "item_operation.h"
#include "equip_operation.h"
#include "equip_config.h"
#include "utils.h"
#include "treasure_config.h"
#include "mission_operation.h"
#include "guild_config.h"
#include "pvp_config.h"

int RoleOperation::get_role_exp(const s_t_role *role, const s_t_exp* t_exp)
{
	switch (role->font_color)
	{
	case 2:
		return t_exp->role_lexp;
		break;
	case 3:
		return t_exp->role_zexp;
		break;
	case 4:
		return t_exp->role_jexp;
		break;
	case 5:
		return t_exp->role_rexp;
		break;
	default:
		break;
	}
	return 99999999;
}

dhc::role_t * RoleOperation::role_create(dhc::player_t *player, uint32_t id, int level, int glevel, int mode)
{
	s_t_role *t_role = sRoleConfig->get_role(id);
	if (!t_role)
	{
		return 0;
	}
	
	if (RoleOperation::has_role(player, id))
	{
		return 0;
	}

	uint64_t role_guid = game::gtool()->assign(et_role);
	dhc::role_t *role = new dhc::role_t();
	role->set_guid(role_guid);
	role->set_player_guid(player->guid());
	role->set_template_id(id);
	role->set_level(level);

	while (role->jskill_level_size() < 7)
	{
		role->add_jskill_level(1);
	}

	if (t_role->font_color > 3)
	{
		role->set_xq(3);
		role->set_xq_time(game::timer()->now());
	}

	role->set_glevel(glevel);
	role->set_pinzhi(t_role->pinzhi * 100);
	role->add_dress_ids(0);
	role->set_dress_on_id(0);
	for (int i = 0; i < 4; ++i)
	{
		role->add_zhuangbeis(0);
	}
	for (int i = 0; i < 2; ++i)
	{
		role->add_treasures(0);
	}
	for (int i = 0; i < 5; ++i)
	{
		role->add_bskill_counts(0);
	}
	POOL_ADD_NEW(role->guid(), role);

	player->add_roles(role->guid());

	if (t_role->font_color > 1)
	{
		bool flag = false;
		for (int i = 0; i < player->role_template_ids_size(); ++i)
		{
			if (player->role_template_ids(i) == id)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			player->add_role_template_ids(id);
		}
	}
	role_dress_check(player, role);

	if (mode != 0)
	{
		LOG_OUTPUT(player, LOG_ROLE, id, 1, mode, LOG_ADD);
	}

	return role;
}

dhc::role_t * RoleOperation::role_create_normal()
{
	uint64_t role_guid = game::gtool()->assign(et_role);
	dhc::role_t *role = new dhc::role_t();
	role->set_guid(role_guid);
	role->set_level(1);
	while (role->jskill_level_size() < 7)
	{
		role->add_jskill_level(1);
	}
	role->add_dress_ids(0);
	role->set_dress_on_id(0);
	for (int i = 0; i < 4; ++i)
	{
		role->add_zhuangbeis(0);
	}
	for (int i = 0; i < 2; ++i)
	{
		role->add_treasures(0);
	}
	for (int i = 0; i < 5; ++i)
	{
		role->add_bskill_counts(0);
	}

	return role;
}

void RoleOperation::role_delete(dhc::player_t *player, uint64_t role_guid, int mode)
{
	dhc::role_t *role = POOL_GET_ROLE(role_guid);
	if (!role)
	{
		return;
	}
	int role_id = role->template_id();

	for (int i = 0; i < role->zhuangbeis_size(); i++)
	{
		uint64_t equip_guid = role->zhuangbeis(i);

		if (equip_guid > 0)
		{
			dhc::equip_t *equip = POOL_GET_EQUIP(equip_guid);
			equip->set_role_guid(0);
		}
	}

	dhc::treasure_t* treasure = 0;
	for (int i = 0; i < role->treasures_size(); ++i)
	{
		if (role->treasures(i) > 0)
		{
			treasure = POOL_GET_TREASURE(role->treasures(i));
			if (treasure)
			{
				treasure->set_role_guid(0);
			}
		}
	}

	POOL_REMOVE(role_guid, player->guid());
	for (int i = 0; i < player->roles_size(); ++i)
	{
		if (player->roles(i) == role_guid)
		{
			for (int j = i; j < player->roles_size() - 1; ++j)
			{
				player->set_roles(j, player->roles(j + 1));
			}
			player->mutable_roles()->RemoveLast();
			break;
		}
	}

	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		if (player->zhenxing(i) == role_guid)
		{
			player->set_zhenxing(i, 0);
		}
	}

	for (int i = 0; i < player->houyuan_size(); ++i)
	{
		if (player->houyuan(i) == role_guid)
		{
			player->set_houyuan(i, 0);
		}
	}

	if (mode != 0)
	{
		LOG_OUTPUT(player, LOG_ROLE, role_id, 1, mode, LOG_DEC);
	}
}

bool RoleOperation::has_t_role(dhc::player_t *player, int id)
{
	for (int i = 0; i < player->role_template_ids_size(); ++i)
	{
		if (player->role_template_ids(i) == id)
		{
			return true;
		}
	}
	return false;
}

bool RoleOperation::has_role(dhc::player_t *player, int id)
{
	for (int i = 0; i < player->roles_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->roles(i));
		if (!role)
		{
			continue;
		}
		if (role->template_id() == id)
		{
			return true;
		}
	}
	return false;
}

bool RoleOperation::has_zheng(dhc::player_t *player, int id)
{
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (!role)
		{
			continue;
		}
		if (role->template_id() == id)
		{
			return true;
		}
	}
	return false;
}

bool RoleOperation::role_has_dress(dhc::role_t *role, int dress_id)
{
	for (int i = 0; i < role->dress_ids_size(); ++i)
	{
		if (role->dress_ids(i) == dress_id)
		{
			return true;
		}
	}

	return false;
}

void RoleOperation::role_dress_check(dhc::player_t *player, dhc::role_t *role)
{
	for (int i = 0; i < player->dress_id_bags_size();)
	{
		bool del = false;
		do 
		{
			const s_t_role_dress * t_dress = sRoleConfig->get_role_dress(player->dress_id_bags(i));
			if (!t_dress)
			{
				del = true;
				break;
			}

			if (t_dress->role == role->template_id())
			{
				del = true;
				if (!role_has_dress(role, t_dress->id))
				{
					role->add_dress_ids(t_dress->id);
				}
				break;
			}
		} while (0);

		if (del)
		{
			for (int j = i; j < player->dress_id_bags_size() - 1; ++j)
			{
				player->set_dress_id_bags(j, player->dress_id_bags(j + 1));
			}
			player->mutable_dress_id_bags()->RemoveLast();
		}
		else
		{
			++i;
		}
	}
}

void RoleOperation::role_dress_get(dhc::player_t *player, int dress_id)
{
	const s_t_role_dress * t_dress = sRoleConfig->get_role_dress(dress_id);
	if (!t_dress)
	{
		return;
	}

	bool has_role = false;
	dhc::role_t *role = 0;
	for (int i = 0; i < player->roles_size(); ++i)
	{
		role = POOL_GET_ROLE(player->roles(i));
		if (role)
		{
			if (role->template_id() == t_dress->role)
			{
				has_role = true;
				if (!role_has_dress(role, dress_id))
				{
					role->add_dress_ids(dress_id);
				}
				break;
			}
		}
	}

	/// 先获取了时装
	if (!has_role)
	{
		player->add_dress_id_bags(dress_id);
	}
}

void RoleOperation::player_guanghuan_get(dhc::player_t *player, int guanghuan)
{
	const s_t_guanghuan *t_guanghuan = sRoleConfig->get_guanghuan(guanghuan);
	if (!t_guanghuan)
	{
		return;
	}

	for (int i = 0; i < player->guanghuan_size(); ++i)
	{
		if (player->guanghuan(i) == guanghuan)
		{
			return;
		}
	}

	player->add_guanghuan(guanghuan);
	player->add_guanghuan_level(0);
}

bool RoleOperation::player_has_guanghuan(dhc::player_t *player, int guanghuan)
{
	for (int i = 0; i < player->guanghuan_size(); ++i)
	{
		if (player->guanghuan(i) == guanghuan)
		{
			return true;
		}
	}
	return false;
}

int RoleOperation::get_role_color_count(dhc::player_t *player, int color)
{
	int count = 0;
	for (int i = 0; i < player->roles_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->roles(i));
		if (role)
		{
			s_t_role *t_role = sRoleConfig->get_role(role->template_id());
			if (t_role && t_role->font_color >= color)
			{
				count++;
			}
		}
	}
	return count;
}

int RoleOperation::get_role_tupo_count(dhc::player_t *player, int level)
{
	int count = player->zhenxing_size();
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			if (role->glevel() < level)
			{
				count--;
			}
		}
		else
		{
			count--;
		}
	}
	return count;
}

int RoleOperation::get_role_equip_count(dhc::player_t *player, int cout)
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
				if (!equip)
				{
					count--;
					break;
				}
			}
		}
		else
		{
			count--;
		}
	}

	return count;
}

int RoleOperation::get_role_skill_count(dhc::player_t *player, int level)
{
	int count = player->zhenxing_size();
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			if (role->jskill_level(0) < level)
			{
				count--;
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

int RoleOperation::get_role_skill_max(dhc::player_t *player, int level)
{
	int max_level = 0;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (role)
		{
			if (role->jskill_level(0) > max_level)
			{
				max_level = role->jskill_level(0);
			}
		}
	}

	return max_level;
}

int RoleOperation::get_role_tupo_max(dhc::player_t *player, int level)
{
	int max_level = 0;
	for (int i = 0; i < player->roles_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->roles(i));
		if (role)
		{
			if (role->glevel() >= level)
			{
				return level;
			}

			if (role->glevel() > max_level)
			{
				max_level = role->glevel();
			}
		}
	}

	return max_level;
}

bool RoleOperation::check_bskill(dhc::player_t *player, dhc::role_t *role, const s_t_reward &defs)
{
	switch (defs.type)
	{
	case BSKILL_TYPE1:
	case BSKILL_TYPE2:
	case BSKILL_TYPE3:
	case BSKILL_TYPE4:
	case BSKILL_TYPE5:
		if (defs.type - 1 < 0 || defs.type - 1 >= role->bskill_counts_size())
			return false;
		return role->bskill_counts(defs.type - 1) >= defs.value1;
		break;
	case 6:
		return role->level() >= defs.value1;
		break;
	case 7:
		return role->jlevel() >= defs.value1;
		break;
	case 8:
		return role->glevel() >= defs.value1;
		break;
	case 9:
		return role->pinzhi() >= defs.value1;
		break;
	case 10:
		if (defs.value1 < 0 || defs.value1 >= role->jskill_level_size())
			return false;
		return role->jskill_level(defs.value1) >= defs.value2;
	case 11:
		return get_role_huiyi_count(player, role->template_id(), defs.value1) >= defs.value1;
	case 12:
		return role_has_dress(role, defs.value1);
	default:
		break;
	}
	return false;
}

void RoleOperation::check_bskill_count(dhc::player_t *player, int type, int count)
{
	if (player->level() < 55)
	{
		return;
	}
	const s_t_role_bskill *t_role_bskill = 0;
	switch (type)
	{
	case BSKILL_TYPE1:
	case BSKILL_TYPE2:
	case BSKILL_TYPE3:
	case BSKILL_TYPE4:
	case BSKILL_TYPE5:
		dhc::role_t *role = 0;
		for (int i = 0; i < player->zhenxing_size(); ++i)
		{
			if (player->zhenxing(i))
			{
				role = POOL_GET_ROLE(player->zhenxing(i));
				if (role)
				{
					t_role_bskill = sRoleConfig->get_role_bskill(role->template_id(), role->bskill_level() + 1);
					if (t_role_bskill)
					{
						for (int kmg = 0; kmg < t_role_bskill->defs.size(); ++kmg)
						{
							if (t_role_bskill->defs[kmg].type == type)
							{
								if (type - 1 >= 0 && type - 1 < role->bskill_counts_size())
									role->set_bskill_counts(type - 1, role->bskill_counts(type - 1) + count);
							}
						}
					}
				}
			}

		}
		break;
	}
}

int RoleOperation::get_role_huiyi_count(dhc::player_t *player, int role_id, int def)
{
	const s_t_item *t_item = 0;
	const s_t_huiyi_jihuo* t_jihuo = 0;
	int count = 0;
	for (int ki = 0; ki < player->huiyi_jihuos_size(); ++ki)
	{
		if (count >= def)
		{
			return count;
		}

		t_jihuo = sRoleConfig->get_huiyi_jihuo(player->huiyi_jihuos(ki));
		if (!t_jihuo)
		{
			return 0;
		}
		for (std::vector<int>::size_type i = 0; i < t_jihuo->huiyi.size(); ++i)
		{
			t_item = sItemConfig->get_item(t_jihuo->huiyi[i]);
			if (!t_item)
			{
				return 0;
			}
			if (t_item->def1 == role_id)
			{
				count++;
			}
		}
	}

	return count;
}

dhc::pet_t* RoleOperation::create_pet(dhc::player_t *player, int template_id, int mode)
{
	dhc::pet_t *pet = new dhc::pet_t();
	pet->set_guid(game::gtool()->assign(et_pet));
	pet->set_player_guid(player->guid());
	pet->set_template_id(template_id);
	pet->set_level(1);
	for (int i = 0; i < 4; ++i)
	{
		pet->add_jinjie_slot(0);
	}
	player->add_pets(pet->guid());
	POOL_ADD_NEW(pet->guid(), pet);

	if (mode != 0)
	{
		LOG_OUTPUT(player, LOG_ROLE, template_id, 1, mode, LOG_ADD);
	}

	return pet;
}

void RoleOperation::destroy_pet(dhc::player_t *player, dhc::pet_t *pet, int mode)
{
	int pet_id = pet->template_id();
	uint64_t pet_guid = pet->guid();
	uint64_t role_guid = pet->role_guid();

	POOL_REMOVE(pet_guid, player->guid());
	for (int i = 0; i < player->pets_size(); ++i)
	{
		if (player->pets(i) == pet_guid)
		{
			for (int j = i; j < player->pets_size() - 1; ++j)
			{
				player->set_pets(j, player->pets(j + 1));
			}
			player->mutable_pets()->RemoveLast();
			break;
		}
	}

	if (player->pet_on() == pet_guid)
	{
		player->set_pet_on(0);
	}
	if (role_guid != 0)
	{
		dhc::role_t *role = POOL_GET_ROLE(role_guid);
		if (role)
		{
			role->set_pet(0);
		}
	}

	if (mode != 0)
	{
		LOG_OUTPUT(player, LOG_ROLE, pet_id, 1, mode, LOG_DEC);
	}
}

int RoleOperation::get_pet_exp(const s_t_pet *pet, const s_t_exp* t_exp)
{
	switch (pet->color)
	{
	case 2:
		return t_exp->pet_lexp;
		break;
	case 3:
		return t_exp->pet_zexp;
		break;
	case 4:
		return t_exp->pet_jexp;
		break;
	case 5:
		return t_exp->pet_rexp;
		break;
	default:
		break;
	}
	return 99999999;
}

bool RoleOperation::has_pet(dhc::player_t *player, int template_id)
{
	dhc::pet_t *pet = 0;
	for (int i = 0; i < player->pets_size(); ++i)
	{
		pet = POOL_GET_PET(player->pets(i));
		if (pet)
		{
			if (pet->template_id() == template_id)
			{
				return true;
			}
		}
	}
	return false;
}

void RoleOperation::get_role_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
{
	s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		return;
	}
	/// 时装
	sEquipConfig->get_player_dress_attrs(player, attrs);
	/// 原本
	RoleOperation::get_role_org_attr(player, role, attrs);
	/// 进阶属性
	RoleOperation::get_role_jinjie_attr(player, role, attrs);
	/// 装备
	RoleOperation::get_role_equip_attr(player, role, attrs);
	/// 装备套装
	RoleOperation::get_role_equip_tz_attr(player, role, attrs);
	/// 宝物
	RoleOperation::get_role_treasure_attr(player, role, attrs);
	/// 被动技能
	RoleOperation::get_role_skill_attr(role, attrs);
	/// 伙伴光环技能
	RoleOperation::get_role_tdskill_attr(player, attrs);
	/// 羁绊
	RoleOperation::get_role_jb_attr(player, role, attrs);
	/// 共振属性
	RoleOperation::get_role_gongzheng_attr(player, role, attrs);
	/// 军团科技
	RoleOperation::get_role_guild_skill_attr(player, attrs);
	/// 回忆录
	RoleOperation::get_role_huiyi_attr(player, attrs);
	/// 队形
	RoleOperation::get_role_duixing_attr(player, role, attrs);
	/// 队形技能
	RoleOperation::get_role_duixing_skill_attr(player, attrs);
	/// 光环
	RoleOperation::get_role_guanghuan_attr(player, attrs);
	/// 光环技能
	RoleOperation::get_role_guanghuan_skill(player, attrs);
	/// 光环套装
	RoleOperation::get_role_guanghuan_target(player, attrs);
	/// 称号
	RoleOperation::get_role_chenghao(player, attrs);
	/// 段位
	RoleOperation::get_role_duanwei_attr(player, attrs);
	/// 护卫宠物属性
	RoleOperation::get_role_pet_guard_attr(player, role, attrs);
	/// 宠物上阵属性
	RoleOperation::get_role_pet_on_attr(player, attrs);
	/// 宠物图鉴
	RoleOperation::get_role_pet_target(player, attrs);

	attrs[3] += attrs[29];
	attrs[4] += attrs[29];
	attrs[1] += attrs[37];
	attrs[2] += attrs[37];
	attrs[3] += attrs[37];
	attrs[4] += attrs[37];
	attrs[5] += attrs[37];

	attrs[8] += attrs[30];
	attrs[9] += attrs[30];
	attrs[6] += attrs[38];
	attrs[7] += attrs[38];
	attrs[8] += attrs[38];
	attrs[9] += attrs[38];
	attrs[10] += attrs[38];

	if (t_role->skill0 <= 9)
	{
		attrs[2] += attrs[31];
		attrs[7] += attrs[33];
	}
	else
	{
		attrs[2] += attrs[32];
		attrs[7] += attrs[34];
	}
}

void RoleOperation::get_role_org_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
{
	s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		return;
	}
	s_t_role_shengpin *t_role_shengpin = sRoleConfig->get_role_shengpin(role->pinzhi());
	if (!t_role_shengpin)
	{
		return;
	}
	for (int j = 0; j < 5; ++j)
	{
		/// 伙伴基本属性
		
		double _sxper = 0;
		for (int i = 0; i <= role->jlevel(); i++)
		{
			s_t_jinjie* _jinjie = sRoleConfig->get_jinjie(i);

			if (_jinjie != NULL)
			{
				_sxper += _jinjie->sxper;
			}
		}

		double cz = t_role->cz[j] * t_role_shengpin->cz[j] + t_role->czcz[j] * t_role_shengpin->cz[j] * role->glevel();
		double cs = t_role->cs[j] * t_role_shengpin->cs[j] + t_role->cscz[j] * t_role_shengpin->cs[j] * _sxper;
		attrs[j + 1] += cs + cz * (role->level() - 1);
	}
	/// 伙伴原始属性
	if (t_role->job == 1)
	{
		attrs[12] += 10;
	}
	else if (t_role->job == 2)
	{
		attrs[11] += 10;
	}
	else if (t_role->job == 3)
	{
		attrs[17] += 10;
	}
}

void RoleOperation::get_role_jinjie_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
{
	int val = sRoleConfig->get_jinjie_shuxing(role->jlevel());
	attrs[6] += val;
	attrs[7] += val;
	attrs[8] += val;
	attrs[9] += val;
	attrs[10] += val;
}

void RoleOperation::get_role_equip_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
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
		const s_t_equip_sx *t_equip_sx = sEquipConfig->get_equip_sx(t_equip->font_color, equip->star());
		if (!t_equip_sx)
		{
			continue;
		}
		attrs[t_equip->eattr.attr] += t_equip->eattr.value + t_equip->eattr.value * t_equip_sx->enhance_rate * equip->enhance();
		for (int k = 0; k < t_equip->ejlattr.size(); ++k)
		{
			attrs[t_equip->ejlattr[k].attr] += t_equip->ejlattr[k].value * equip->jilian();
		}
		for (int k = 0; k < equip->rand_ids_size(); ++k)
		{
			attrs[equip->rand_ids(k)] += equip->rand_values(k);
		}
		for (int k = 0; k < equip->stone_size(); ++k)
		{
			s_t_item *t_item = sItemConfig->get_item(equip->stone(k));
			if (!t_item)
			{
				continue;
			}
			attrs[t_item->def1] += t_item->def2;
		}
		if (t_equip->font_color == 5)
		{
			std::vector<int> ids;
			sEquipConfig->get_equip_skills(t_equip->type, equip->jilian(), ids);
			for (int i = 0; i < ids.size(); ++i)
			{
				const s_t_equip_skill *t_equip_skill = sEquipConfig->get_equip_skill(ids[i]);
				if (t_equip_skill->type == 1)
				{
					attrs[t_equip_skill->def1] += t_equip_skill->def2;
				}
			}
		}
	}
}

void RoleOperation::get_role_equip_tz_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
{
	std::set<uint64_t> equip_set;
	for (int j = 0; j < role->zhuangbeis_size(); ++j)
	{
		uint64_t equip_guid = role->zhuangbeis(j);
		dhc::equip_t *equip = POOL_GET_EQUIP(equip_guid);
		if (!equip)
		{
			continue;
		}
		if (equip_set.find(equip_guid) != equip_set.end())
		{
			continue;
		}
		equip_set.insert(equip_guid);
		s_t_equip_tz *t_equip_tz = sEquipConfig->get_equip_tz(equip->template_id());
		if (!t_equip_tz)
		{
			continue;
		}
		int num = 1;
		for (int k = j + 1; k < role->zhuangbeis_size(); ++k)
		{
			uint64_t equip_guid1 = role->zhuangbeis(k);
			dhc::equip_t *equip1 = POOL_GET_EQUIP(equip_guid1);
			if (!equip1)
			{
				continue;
			}
			if (equip_set.find(equip_guid1) != equip_set.end())
			{
				continue;
			}
			if (!t_equip_tz->has_equip_id(equip1->template_id()))
			{
				continue;
			}
			num++;
			equip_set.insert(equip_guid1);
		}
		if (num >= 2)
		{
			attrs[t_equip_tz->attr1] += t_equip_tz->value1;
		}
		if (num >= 3)
		{
			attrs[t_equip_tz->attr2] += t_equip_tz->value2;
		}
		if (num >= 4)
		{
			attrs[t_equip_tz->attr3] += t_equip_tz->value3;
			attrs[t_equip_tz->attr4] += t_equip_tz->value4;
		}
	}
}

void RoleOperation::get_role_treasure_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
{
	for (int j = 0; j < role->treasures_size(); ++j)
	{
		dhc::treasure_t *treasure = POOL_GET_TREASURE(role->treasures(j));
		if (!treasure)
		{
			continue;
		}
		const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
		if (!t_treasure)
		{
			continue;
		}
		attrs[t_treasure->att1.att] += t_treasure->att1.val + t_treasure->att1.val * treasure->enhance();
		attrs[t_treasure->att2.att] += t_treasure->att2.val + t_treasure->att2.val * treasure->enhance();
		attrs[t_treasure->jl_att1.att] += t_treasure->jl_att1.val * treasure->jilian();
		attrs[t_treasure->jl_att2.att] += t_treasure->jl_att2.val * treasure->jilian();
		attrs[t_treasure->jl_att3.att] += t_treasure->jl_att3.val * treasure->jilian();

		if (t_treasure->color == 5)
		{
			std::vector<int> ids;
			sEquipConfig->get_equip_skills(t_treasure->type + 4, treasure->jilian(), ids);
			for (int i = 0; i < ids.size(); ++i)
			{
				const s_t_equip_skill *t_equip_skill = sEquipConfig->get_equip_skill(ids[i]);
				if (t_equip_skill->type == 1)
				{
					attrs[t_equip_skill->def1] += t_equip_skill->def2;
				}
			}
		}

		
		{
			double val = 0;
			const s_t_treasure_sx *t_treasure_sx = sTreasureConfig->get_sx(treasure->star(), t_treasure->color);
			if (t_treasure_sx)
			{
				val = t_treasure_sx->valuemax;
			}
			const s_t_treasure_sx *t_treasure_sx1 = sTreasureConfig->get_sx(treasure->star() + 1, t_treasure->color);
			if (t_treasure_sx1)
			{
				val += t_treasure_sx1->value1 * treasure->star_exp();
			}
			if (t_treasure->type == 1)
			{
				attrs[2] += val;
			}
			else
			{
				attrs[29] += val;
			}
		}
	}
}

void RoleOperation::get_role_skill_attr(dhc::role_t *role, std::map<int, double> &attrs)
{
	s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		return;
	}
	for (int j = 0; j < t_role->tskills.size(); ++j)
	{
		s_t_skill *t_skill = sRoleConfig->get_skill(t_role->tskills[j]);
		if (!t_skill)
		{
			continue;
		}
		s_t_role_shengpin *t_role_shengpin = sRoleConfig->get_role_shengpin(role->pinzhi());
		if (!t_role_shengpin)
		{
			continue;
		}
		if (t_skill->type != 2 || t_skill->passive_type != 1)
		{
			continue;
		}
		if (j >= role->glevel())
		{
			break;
		}
		attrs[t_skill->passive_modify_att_type] += t_skill->passive_modify_att_val + t_skill->passive_modify_att_val_add * t_role_shengpin->bdjnjc;
	}
}

void RoleOperation::get_role_tdskill_attr(dhc::player_t *player, std::map<int, double> &attrs)
{
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
		if (!role)
		{
			continue;
		}
		s_t_role *t_role = sRoleConfig->get_role(role->template_id());
		if (t_role)
		{
			for (int j = 0; j < t_role->tskills.size(); ++j)
			{
				s_t_skill *t_skill = sRoleConfig->get_skill(t_role->tskills[j]);
				if (!t_skill)
				{
					continue;
				}
				s_t_role_shengpin *t_role_shengpin = sRoleConfig->get_role_shengpin(role->pinzhi());
				if (!t_role_shengpin)
				{
					continue;
				}
				if (t_skill->type != 2 || t_skill->passive_type != 2)
				{
					continue;
				}
				if (j >= role->glevel())
				{
					break;
				}
				attrs[t_skill->passive_modify_att_type] += t_skill->passive_modify_att_val + t_skill->passive_modify_att_val_add * t_role_shengpin->bdjnjc;
			}
		}
	}
}

void RoleOperation::get_role_jb_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
{
	std::set<int> role_ids;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		dhc::role_t *role1 = POOL_GET_ROLE(player->zhenxing(i));
		if (!role1)
		{
			continue;
		}
		role_ids.insert(role1->template_id());
	}
	for (int i = 0; i < player->houyuan_size(); ++i)
	{
		dhc::role_t *role1 = POOL_GET_ROLE(player->houyuan(i));
		if (!role1)
		{
			continue;
		}
		role_ids.insert(role1->template_id());
	}
	std::set<int> equip_ids;
	for (int i = 0; i < role->zhuangbeis_size(); ++i)
	{
		dhc::equip_t *equip1 = POOL_GET_EQUIP(role->zhuangbeis(i));
		if (!equip1)
		{
			continue;
		}
		equip_ids.insert(equip1->template_id());
	}
	std::set<int> treasure_ids;
	for (int i = 0; i < role->treasures_size(); ++i)
	{
		dhc::treasure_t *treasure1 = POOL_GET_TREASURE(role->treasures(i));
		if (!treasure1)
		{
			continue;
		}
		treasure_ids.insert(treasure1->template_id());
	}

	s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		return;
	}
	for (int j = 0; j < t_role->jbs.size(); ++j)
	{
		s_t_role_jiban * t_role_jiban = sRoleConfig->get_role_jiban(t_role->jbs[j]);
		if (!t_role_jiban)
		{
			continue;
		}
		bool flag = true;
		if (t_role_jiban->type == 1)
		{
			for (int k = 0; k < t_role_jiban->tids.size(); ++k)
			{
				std::set<int>::iterator it = role_ids.find(t_role_jiban->tids[k]);
				if (it == role_ids.end())
				{
					flag = false;
					break;
				}
			}
		}
		else if (t_role_jiban->type == 2)
		{
			for (int k = 0; k < t_role_jiban->tids.size(); ++k)
			{
				std::set<int>::iterator it = equip_ids.find(t_role_jiban->tids[k]);
				if (it == equip_ids.end())
				{
					flag = false;
					break;
				}
			}
		}
		else if (t_role_jiban->type == 3)
		{
			for (int k = 0; k < t_role_jiban->tids.size(); ++k)
			{
				std::set<int>::iterator it = treasure_ids.find(t_role_jiban->tids[k]);
				if (it == treasure_ids.end())
				{
					flag = false;
					break;
				}
			}
		}
		if (flag)
		{
			for (int j = 0; j < t_role_jiban->attrs.size(); ++j)
			{
				attrs[t_role_jiban->attrs[j].first] += t_role_jiban->attrs[j].second;
			}
		}
	}
}

void RoleOperation::get_role_gongzheng_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
{
	const s_t_role_gongzheng* t_gongzheng = 0;
	int min_enhanc_level = 99999;
	int min_jl_level = 99999;

	bool has_equip = (role->zhuangbeis_size() == 4)  ? true : false;
	dhc::equip_t *equip = 0;
	for (int i = 0; i < role->zhuangbeis_size(); ++i)
	{
		equip = POOL_GET_EQUIP(role->zhuangbeis(i));
		if (!equip)
		{
			has_equip = false;
			break;
		}
		if (equip->enhance() < min_enhanc_level)
		{
			min_enhanc_level = equip->enhance();
		}
		if (equip->jilian() < min_jl_level)
		{
			min_jl_level = equip->jilian();
		}
	}
	if (has_equip)
	{
		t_gongzheng = sRoleConfig->get_role_gongzheng(1, min_enhanc_level);
		if (t_gongzheng)
		{
			for (int j = 0; j < t_gongzheng->attrs.size(); ++j)
			{
				attrs[t_gongzheng->attrs[j].first] += t_gongzheng->attrs[j].second;
			}
		}
		t_gongzheng = sRoleConfig->get_role_gongzheng(3, min_jl_level);
		if (t_gongzheng)
		{
			for (int j = 0; j < t_gongzheng->attrs.size(); ++j)
			{
				attrs[t_gongzheng->attrs[j].first] += t_gongzheng->attrs[j].second;
			}
		}
	}

	min_enhanc_level = 99999;
	min_jl_level = 99999;
	bool has_treasure = (role->treasures_size() == 2) ? true : false;
	dhc::treasure_t* treasure = 0;
	for (int i = 0; i < role->treasures_size(); ++i)
	{
		treasure = POOL_GET_TREASURE(role->treasures(i));
		if (!treasure)
		{
			has_treasure = false;
			break;
		}
		if (treasure->enhance() < min_enhanc_level)
		{
			min_enhanc_level = treasure->enhance();
		}
		if (treasure->jilian() < min_jl_level)
		{
			min_jl_level = treasure->jilian();
		}
	}
	if (has_treasure)
	{
		t_gongzheng = sRoleConfig->get_role_gongzheng(2, min_enhanc_level);
		if (t_gongzheng)
		{
			for (int j = 0; j < t_gongzheng->attrs.size(); ++j)
			{
				attrs[t_gongzheng->attrs[j].first] += t_gongzheng->attrs[j].second;
			}
		}
		t_gongzheng = sRoleConfig->get_role_gongzheng(4, min_jl_level);
		if (t_gongzheng)
		{
			for (int j = 0; j < t_gongzheng->attrs.size(); ++j)
			{
				attrs[t_gongzheng->attrs[j].first] += t_gongzheng->attrs[j].second;
			}
		}
	}

	int skill_level = role->jskill_level(0);
	for (int i = 1; i < role->jskill_level_size(); ++i)
	{
		if (!MissionOperation::can_jskill(i - 1, role->jlevel()))
		{
			continue;
		}
		skill_level += role->jskill_level(i);
	}
	t_gongzheng = sRoleConfig->get_role_gongzheng(5, skill_level);
	if (t_gongzheng)
	{
		for (int j = 0; j < t_gongzheng->attrs.size(); ++j)
		{
			attrs[t_gongzheng->attrs[j].first] += t_gongzheng->attrs[j].second;
		}
	}

	min_enhanc_level = 99999;
	min_jl_level = 99999;
	bool has_houyuan = (player->houyuan_size() == 6) ? true: false;
	dhc::role_t *houyuan = 0;
	for (int i = 0; i < player->houyuan_size(); ++i)
	{
		houyuan = POOL_GET_ROLE(player->houyuan(i));
		if (!houyuan)
		{
			has_houyuan = false;
			break;
		}
		if (houyuan->level() < min_enhanc_level)
		{
			min_enhanc_level = houyuan->level();
		}
		if (houyuan->glevel() < min_jl_level)
		{
			min_jl_level = houyuan->glevel();
		}
	}
	if (has_houyuan)
	{
		t_gongzheng = sRoleConfig->get_role_gongzheng(6, min_enhanc_level);
		if (t_gongzheng)
		{
			for (int j = 0; j < t_gongzheng->attrs.size(); ++j)
			{
				attrs[t_gongzheng->attrs[j].first] += t_gongzheng->attrs[j].second;
			}
		}
		t_gongzheng = sRoleConfig->get_role_gongzheng(7, min_jl_level);
		if (t_gongzheng)
		{
			for (int j = 0; j < t_gongzheng->attrs.size(); ++j)
			{
				attrs[t_gongzheng->attrs[j].first] += t_gongzheng->attrs[j].second;
			}
		}
	}
}

void RoleOperation::get_role_guild_skill_attr(dhc::player_t *player, std::map<int, double> &attrs)
{
	if (player->guild() != 0)
	{
		const s_t_guild_skill *t_skill = 0;
		for (int i = 0; i < player->guild_skill_ids_size(); ++i)
		{
			t_skill = sGuildConfig->get_guild_skill(player->guild_skill_ids(i));
			if (t_skill)
			{
				attrs[t_skill->att] += t_skill->value * player->guild_skill_levels(i);
			}
		}
	}
}

void RoleOperation::get_role_huiyi_attr(dhc::player_t *player, std::map<int, double> &attrs)
{
	const s_t_huiyi_jihuo* t_jihuo = 0;
	for (int i = 0; i < player->huiyi_jihuos_size(); ++i)
	{
		t_jihuo = sRoleConfig->get_huiyi_jihuo(player->huiyi_jihuos(i));
		if (t_jihuo)
		{	
			for (std::vector<s_t_huiyi_start_attr >::size_type j = 0; j < t_jihuo->attrs.size(); ++j)
			{
				const s_t_huiyi_start_attr& atthui = t_jihuo->attrs[j];
				if (i >= player->huiyi_jihuo_starts_size())
				{
					attrs[atthui.attr] += atthui.val1;
				}
				else
				{
					attrs[atthui.attr] += atthui.val1 + atthui.val2 * player->huiyi_jihuo_starts(i);
				}
			}
		}
	}

	get_role_huiyi_chengjiu_attr(player, attrs);
}

void RoleOperation::get_role_huiyi_chengjiu_attr(dhc::player_t *player, std::map<int, double> &attrs)
{
	sRoleConfig->get_huiyi_chengjiu_attr(player, attrs);
}

void RoleOperation::get_role_duixing_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrs)
{
	int index = -1;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		if (player->zhenxing(i) == role->guid())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		return;
	}
	int site = player->duixing(index);
	s_t_duixing *t_duixing = sRoleConfig->get_duixing(player->duixing_id());
	if (!t_duixing)
	{
		return;
	}
	int duiwei = t_duixing->duiweis[site];
	attrs[t_duixing->sx[duiwei / 6][0].attr] += t_duixing->sx[duiwei / 6][0].value + t_duixing->sx[duiwei / 6][0].add * player->duixing_level();
	attrs[t_duixing->sx[duiwei / 6][1].attr] += t_duixing->sx[duiwei / 6][1].value + t_duixing->sx[duiwei / 6][1].add * player->duixing_level();
}

void RoleOperation::get_role_duixing_skill_attr(dhc::player_t *player, std::map<int, double> &attrs)
{
	sRoleConfig->get_duixing_skill_attr(player, attrs);
}

void RoleOperation::get_role_guanghuan_attr(dhc::player_t *player, std::map<int, double> &attrs)
{
	if (player->guanghuan_id() > 0)
	{
		const s_t_guanghuan * t_guanghuan = sRoleConfig->get_guanghuan(player->guanghuan_id());
		if (t_guanghuan)
		{
			int level = 0;
			for (int i = 0; i < player->guanghuan_size(); ++i)
			{
				if (player->guanghuan(i) == player->guanghuan_id())
				{
					level = player->guanghuan_level(i);
					break;
				}
			}
			if (t_guanghuan->att1)
			{
				attrs[t_guanghuan->att1] += t_guanghuan->val1 + t_guanghuan->val1 * 0.025f * level;
			}
			if (t_guanghuan->att2)
			{
				attrs[t_guanghuan->att2] += t_guanghuan->val2;
			}
		}
	}
}

void RoleOperation::get_role_guanghuan_skill(dhc::player_t *player, std::map<int, double> &attrs)
{
	sRoleConfig->get_guanghuan_skill_attr(player, attrs);
}

void RoleOperation::get_role_guanghuan_target(dhc::player_t *player, std::map<int, double> &attrs)
{
	sRoleConfig->get_guanghuan_target_attr(player, attrs);
}

void RoleOperation::get_role_chenghao(dhc::player_t *player, std::map<int, double> &attrs)
{
	std::map<int, int> chs;
	for (int i = 0; i < player->chenghao_size(); i++)
	{
		const s_t_chenghao *t_chenhao = sPlayerConfig->get_chenghao(player->chenghao(i));
		if (t_chenhao)
		{
			int type = t_chenhao->type;
			if (chs.find(type) != chs.end())
			{
				if (chs[type] > t_chenhao->id)
				{
					chs[type] = t_chenhao->id;
				}
			}
			else
			{
				chs[type] = t_chenhao->id;
			}
		}
	}
	for (std::map<int, int>::iterator it = chs.begin(); it != chs.end(); ++it)
	{
		const s_t_chenghao *t_chenghao = sPlayerConfig->get_chenghao((*it).second);
		if (t_chenghao)
		{
			for (int i = 0; i < t_chenghao->attrs.size(); ++i)
			{
				attrs[t_chenghao->attrs[i].first] += t_chenghao->attrs[i].second;
			}
		}
	}
}

void RoleOperation::get_role_duanwei_attr(dhc::player_t *player, std::map<int, double> &attrs)
{
	const s_t_ds_duanwei *t_duanwei = sPvpConfig->get_ds_duanwei(player->ds_duanwei());
	if (t_duanwei)
	{
		attrs[t_duanwei->attr1] += t_duanwei->value1;
		attrs[t_duanwei->attr2] += t_duanwei->value2;
	}
}

void RoleOperation::get_pet_attr(dhc::player_t *player, dhc::pet_t *pet, std::map<int, double> &attrs, bool extra /* = false */, bool fight /* = false */, bool selff /* = false */)
{
	const s_t_pet *t_pet = sRoleConfig->get_pet(pet->template_id());
	if (!t_pet)
	{
		return;
	}

	attrs[1] += t_pet->cshp + (t_pet->cshpcz + t_pet->sxhpcz * pet->star()) * pet->level() + t_pet->jjhp * pet->jinjie() + t_pet->sxhpadd * pet->star();
	attrs[2] += t_pet->csgj + (t_pet->csgjcz + t_pet->sxgjcz * pet->star()) * pet->level() + t_pet->jjgj * pet->jinjie() + t_pet->sxgjadd * pet->star();
	attrs[3] += t_pet->cswf + (t_pet->cswfcz + t_pet->sxwfcz * pet->star()) * pet->level() + t_pet->jjwf * pet->jinjie() + t_pet->sxwfadd * pet->star();
	attrs[4] += t_pet->csmf + (t_pet->csmfcz + t_pet->sxmfcz * pet->star()) * pet->level() + t_pet->jjmf * pet->jinjie() + t_pet->sxmfadd * pet->star();

	const s_t_pet_jinjie_item *t_jinjie_item = 0;
	for (int i = 0; i < pet->jinjie_slot_size(); ++i)
	{
		if (pet->jinjie_slot(i) != 0)
		{
			t_jinjie_item = sRoleConfig->get_pet_jinjie_item(pet->jinjie_slot(i));
			if (t_jinjie_item)
			{
				attrs[t_jinjie_item->att1] += t_jinjie_item->val1;
				attrs[t_jinjie_item->att2] += t_jinjie_item->val2;
				attrs[t_jinjie_item->att3] += t_jinjie_item->val3;
			}
		}
	}
	const s_t_pet_jinjie *t_jinjie = sRoleConfig->get_pet_jinjie(pet->jinjie() - 1);
	if (t_jinjie)
	{
		for (std::map<int, int>::const_iterator it = t_jinjie->attrs.begin();
			it != t_jinjie->attrs.end();
			++it)
		{
			attrs[it->first] += it->second;
		}
	}

	t_jinjie = sRoleConfig->get_pet_jinjie(pet->jinjie());
	if (t_jinjie)
	{
		attrs[t_pet->jjattr1] += t_pet->jjval1 * t_jinjie->extra1;
		if (extra)
			attrs[t_pet->jjattr2] += t_pet->jjval2 * t_jinjie->extra2;

		if (selff)
		{
			if (attrs.find(29) != attrs.end())
			{
				attrs[3] += attrs[29];
				attrs[4] += attrs[29];
			}
		}
		

		attrs[1] *= (1.0 + (double)t_jinjie->quan / 100.0);
		attrs[2] *= (1.0 + (double)t_jinjie->quan / 100.0);
		attrs[3] *= (1.0 + (double)t_jinjie->quan / 100.0);
		attrs[4] *= (1.0 + (double)t_jinjie->quan / 100.0);
	}
	else
	{
		if (selff)
		{
			if (attrs.find(29) != attrs.end())
			{
				attrs[3] += attrs[29];
				attrs[4] += attrs[29];
			}
		}
	}

	if (fight)
	{
		double gj = 0.0;
		for (int i = 0; i < player->zhenxing_size(); ++i)
		{
			dhc::role_t *role = POOL_GET_ROLE(player->zhenxing(i));
			if (!role)
			{
				continue;
			}
			std::map<int, double> role_attrs;
			RoleOperation::get_role_attr(player, role, role_attrs);
			gj += role_attrs[2] * (1 + role_attrs[7] * 0.01f);
		}
		gj = gj / 6.0;
		if (gj > attrs[2])
		{
			attrs[2] = gj;
		}
		if (t_pet->color == 3)
		{
			attrs[2] *= 1.5;
		}
		else if (t_pet->color == 4)
		{
			attrs[2] *= 2;
		}
	}
}

void RoleOperation::get_role_pet_guard_attr(dhc::player_t *player, dhc::role_t *role, std::map<int, double> &attrss)
{
	if (role->pet() != 0)
	{
		dhc::pet_t *pet = POOL_GET_PET(role->pet());
		if (!pet)
		{
			return;
		}
		const s_t_pet *t_pet = sRoleConfig->get_pet(pet->template_id());
		if (!t_pet)
		{
			return;
		}
		std::map<int, double> attrs;
		get_pet_attr(player, pet, attrs);

		for (std::map<int, double>::const_iterator itt = attrs.begin();
			itt != attrs.end();
			++itt)
		{
			attrss[itt->first] += itt->second * t_pet->guard_jiacheng;
		}
	}
}

void RoleOperation::get_role_pet_on_attr(dhc::player_t *player, std::map<int, double> &attrss)
{
	if (player->pet_on() != 0)
	{
		dhc::pet_t *pet = POOL_GET_PET(player->pet_on());
		if (!pet)
		{
			return;
		}
		const s_t_pet *t_pet = sRoleConfig->get_pet(pet->template_id());
		if (!t_pet)
		{
			return;
		}
		std::map<int, double> attrs;
		get_pet_attr(player, pet, attrs);

		for (std::map<int, double>::const_iterator itt = attrs.begin();
			itt != attrs.end();
			++itt)
		{
			attrss[itt->first] += itt->second * t_pet->on_jiacheng;
		}
	}
}

void RoleOperation::get_role_pet_target(dhc::player_t *player, std::map<int, double> &attrs)
{
	if (player->level() < 70)
	{
		return;
	}
	const std::vector<s_t_pet_chengjiu>& chengjius = sRoleConfig->get_pet_chengjiu();
	for (int i = 0; i < chengjius.size(); ++i)
	{
		if (has_pet(player, chengjius[i].pet1) && has_pet(player, chengjius[i].pet2))
		{
			for (int j = 0; j < chengjius[i].attrs.size(); ++j)
			{
				const std::pair<int, int>& catt = chengjius[i].attrs[j];
				attrs[catt.first] += catt.second;
			}
		}
	}
}

//////////////////////////////////////////////////////////////////////////

bool RoleOperation::get_role_init_exp(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds)
{
	const s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		return false;
	}

	const s_t_exp *t_exp = 0;
	int exp = 0;
	for (int level = 2; level <= role->level(); ++level)
	{
		t_exp = sPlayerConfig->get_exp(level);
		if (!t_exp)
		{
			return false;
		}
		exp += get_role_exp(t_role, t_exp);
	}

	if (exp > 0)
	{
		rds.add_reward(1, resource::GOLD, exp);
		rds.add_reward(1, resource::YUANLI, exp);
	}
	
	return true;
}

bool RoleOperation::get_role_init_tupo(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds)
{
	const s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		return false;
	}
	int index = t_role->font_color - 2;
	uint32_t id = sItemConfig->get_suipian(role->template_id());
	if (id == 0)
	{
		return false;
	}
	std::map<uint32_t, int> items;
	int gold = 0;

	const s_t_tupo *t_tupo = 0;
	for (int level = 0; level < role->glevel(); ++level)
	{
		t_tupo = sRoleConfig->get_tupo(level + 1);
		if (!t_tupo)
		{
			return false;
		}

		if (index < 0 || index >= t_tupo->suipian.size())
		{
			return false;
		}
		
		gold += t_tupo->gold;
		items[id] += t_tupo->suipian[index];
	}


	for (std::map<uint32_t, int>::const_iterator it = items.begin();
		it != items.end();
		++it)
	{
		rds.add_reward(2, it->first, it->second);
	}

	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}

	return true;
}

bool RoleOperation::get_role_init_jinjie(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds)
{
	const s_t_role * t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		return false;
	}

	const s_t_jinjie * t_jinjie = 0;
	std::map<uint32_t, int> items;
	int gold = 0;
	for (int level = 1; level <= role->jlevel(); ++level)
	{
		t_jinjie = sRoleConfig->get_jinjie(level);
		if (!t_jinjie)
		{
			return false;
		}
		items[t_jinjie->clty] += t_jinjie->clty_num;
		items[t_jinjie->clfy] += (t_role->job == 1) ? t_jinjie->clfy_num1 : t_jinjie->clfy_num;
		items[t_jinjie->clgj] += (t_role->job == 2) ? t_jinjie->clgj_num1 : t_jinjie->clgj_num;
		items[t_jinjie->clmf] += (t_role->job == 3) ? t_jinjie->clmf_num1 : t_jinjie->clmf_num;
		gold += t_jinjie->gold;
	}

	for (std::map<uint32_t, int>::const_iterator it = items.begin();
		it != items.end();
		++it)
	{
		rds.add_reward(2, it->first, it->second);
	}

	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}
	
	return true;
}

bool RoleOperation::get_role_init_skill(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds)
{
	const s_t_skillup *t_skill = 0;
	int level = 0;
	int item_num = 0;
	int gold = 0;
	for (int i = 0; i < role->jskill_level_size(); ++i)
	{
		level = role->jskill_level(i);
		for (int j = 1; j < level; ++j)
		{
			t_skill = sRoleConfig->get_skillup(j + 1);
			if (!t_skill)
			{
				return false;
			}
			gold += t_skill->gold;
			item_num += t_skill->item_num;
		}
	}

	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}
	
	if (item_num > 0)
	{
		rds.add_reward(2, 50090001, item_num);
	}

	return true;
}

bool RoleOperation::get_role_init_shenpin(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds)
{
	const s_t_role* t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		return false;
	}

	const s_t_item* t_suipian = sItemConfig->get_item(sItemConfig->get_suipian(role->template_id()));
	if (!t_suipian)
	{
		return false;
	}

	int start_pinzhi = t_role->pinzhi * 100;
	int end_pinzhi = role->pinzhi();

	int shengpinshi = 0;
	int gold = 0;
	int hongsezhuanbei = 0;
	int suipian = 0;
	int zhanhun = 0;
	const std::map<int, s_t_role_shengpin>& all_shengpin = sRoleConfig->get_role_all_shengpin();
	for (std::map<int, s_t_role_shengpin>::const_iterator it = all_shengpin.begin();
		it != all_shengpin.end();
		++it)
	{
		const s_t_role_shengpin& shengpin = it->second;
		if (shengpin.pinzhi > start_pinzhi && shengpin.pinzhi <= end_pinzhi)
		{
			shengpinshi += shengpin.shengpinshi;
			gold += shengpin.gold;
			hongsezhuanbei += shengpin.hongsehuobanzhili;
			suipian += shengpin.suipian;
			zhanhun += shengpin.zhanhun;
		}
	}

	if (shengpinshi > 0)
	{
		rds.add_reward(2, 50130001, shengpinshi);
	}
	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}
	if (hongsezhuanbei > 0)
	{
		rds.add_reward(2, 50110001, hongsezhuanbei);
	}
	if (suipian > 0)
	{
		rds.add_reward(2, t_suipian->id, suipian);
	}
	if (zhanhun > 0)
	{
		rds.add_reward(1, resource::ZHANHUN, zhanhun);
	}

	return true;
}

bool RoleOperation::get_role_init_bskill(dhc::player_t *player, dhc::role_t *role, s_t_rewards& rds)
{
	int item_num = 0;
	int gold = 0;
	const s_t_role_bskill_up* t_role_bskill_up = 0;
	for (int i = 1; i <= role->bskill_level(); i++)
	{
		t_role_bskill_up = sRoleConfig->get_role_bskill_up(i);
		if (!t_role_bskill_up)
		{
			return false;
		}

		item_num += t_role_bskill_up->jls;
		gold += t_role_bskill_up->gold;
	}

	if (item_num > 0)
	{
		rds.add_reward(2, 50090001, item_num);
	}

	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}

	return true;
}

bool RoleOperation::role_mod_equip(dhc::role_t *role, 
	const std::vector<int> &indexs, 
	const std::vector<uint64_t> &equip_guids)
{
	if (indexs.size() != equip_guids.size())
	{
		return false;
	}

	int index = 0;
	uint64_t equip_guid = 0;
	for (int i = 0; i < indexs.size(); ++i)
	{
		index = indexs[i];
		equip_guid = equip_guids[i];

		if (index < 0 || index >= role->zhuangbeis_size())
		{
			return false;
		}
		if (role->zhuangbeis(index) == equip_guid)
		{
			return false;
		}

		dhc::equip_t *equip = 0;
		if (equip_guid > 0)
		{
			equip = POOL_GET_EQUIP(equip_guid);
			if (!equip)
			{
				return false;
			}
			if (equip->role_guid() > 0)
			{
				return false;
			}
			s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
			if (!t_equip)
			{
				return false;
			}
			if (t_equip->type - 1 != index)
			{
				return false;
			}
		}
		if (role->zhuangbeis(index) > 0)
		{
			dhc::equip_t *equip1 = POOL_GET_EQUIP(role->zhuangbeis(index));
			if (!equip1)
			{
				return false;
			}
			equip1->set_role_guid(0);
			role->set_zhuangbeis(index, 0);
		}
		if (equip_guid > 0)
		{
			equip->set_role_guid(role->guid());
			role->set_zhuangbeis(index, equip->guid());
		}
	}

	return true;
}

bool RoleOperation::role_mod_treasure(dhc::role_t *role,
	const std::vector<int> &indexs,
	const std::vector<uint64_t> &treasure_guids)
{
	if (indexs.size() != treasure_guids.size())
	{
		return false;
	}

	int index = 0;
	uint64_t treasure_guid = 0;
	dhc::treasure_t *on_treasure = 0;
	dhc::treasure_t *off_treasure = 0;
	const s_t_treasure *t_treasure = 0;

	for (int i = 0; i < indexs.size(); ++i)
	{
		index = indexs[i];
		if (index < 0 || index >= role->treasures_size())
		{
			return false;
		}
		treasure_guid = treasure_guids[i];
		if (role->treasures(index) == treasure_guid)
		{
			return false;
		}

		/// 要装备的宝物
		if (treasure_guid > 0)
		{
			on_treasure = POOL_GET_TREASURE(treasure_guid);
			if (!on_treasure)
			{
				return false;
			}
			if (on_treasure->role_guid() > 0)
			{
				return false;
			}
			if (on_treasure->locked() != 0)
			{
				return false;
			}
			t_treasure = sTreasureConfig->get_treasure(on_treasure->template_id());
			if (!t_treasure)
			{
				return false;
			}
			if (t_treasure->type == 5)
			{
				return false;
			}
			if (t_treasure->type - 1 != index)
			{
				return false;
			}
		}

		/// 要卸下的宝物
		if (role->treasures(index) > 0)
		{
			off_treasure = POOL_GET_TREASURE(role->treasures(index));
			if (!off_treasure)
			{
				return false;
			}
			off_treasure->set_role_guid(0);
			role->set_treasures(index, 0);
		}

		if (treasure_guid > 0)
		{
			on_treasure->set_role_guid(role->guid());
			role->set_treasures(index, on_treasure->guid());
		}
	}

	return true;
}

bool RoleOperation::get_pet_init_level(dhc::player_t *player, dhc::pet_t *pet, s_t_rewards &rds)
{
	const s_t_pet *t_pet = sRoleConfig->get_pet(pet->template_id());
	if (!t_pet)
	{
		return false;
	}
	int exp = 0;
	const s_t_exp *t_exp = 0;
	for (int i = 2; i <= pet->level(); ++i)
	{
		t_exp = sPlayerConfig->get_exp(i);
		if (!t_exp)
		{
			return false;
		}
		exp += get_pet_exp(t_pet, t_exp);
	}
	exp += pet->exp();

	if (exp > 0)
	{
		rds.add_reward(1, resource::GOLD, exp);

		int zi_num = exp / 100000;
		int lan_num = exp % 100000 / 50000;
		int lv_num = exp % 100000 % 50000 / 10000;

		if (zi_num > 0)
		{
			rds.add_reward(2, 110020003, zi_num);
		}
		if (lan_num > 0)
		{
			rds.add_reward(2, 110020002, lan_num);
		}
		if (lv_num > 0)
		{
			rds.add_reward(2, 110020001, lv_num);
		}
	}

	return true;
}

bool RoleOperation::get_pet_init_jinjie(dhc::player_t *player, dhc::pet_t *pet, s_t_rewards &rds)
{
	int gold = 0;
	const s_t_pet_jinjie *t_jinjie = 0;
	for (int i = 0; i < pet->jinjie(); i++)
	{
		t_jinjie = sRoleConfig->get_pet_jinjie(i);
		if (!t_jinjie)
		{
			return false;
		}
		gold += t_jinjie->gold;

		for (int j = 0; j < t_jinjie->cailiao.size(); ++j)
		{
			rds.add_reward(2, t_jinjie->cailiao[j], 1);
		}
	}

	for (int i = 0; i < pet->jinjie_slot_size(); ++i)
	{
		if (pet->jinjie_slot(i) != 0)
		{
			rds.add_reward(2, pet->jinjie_slot(i), 1);
		}
	}

	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}

	return true;
}

bool RoleOperation::get_pet_init_star(dhc::player_t *player, dhc::pet_t *pet, s_t_rewards &rds, bool fenjie)
{
	const s_t_pet *t_pet = sRoleConfig->get_pet(pet->template_id());
	if (!t_pet)
	{
		return false;
	}

	const s_t_pet_shengxing *t_shengxing = 0;
	int color = t_pet->color - 2;
	int suipian = 0;
	int shitou = 0;
	int gold = 0;
	for (int i = 1; i <= pet->star(); ++i)
	{
		t_shengxing = sRoleConfig->get_pet_shengxing(i);
		if (!t_shengxing)
		{
			return false;
		}
		if (color < 0 || color >= t_shengxing->shengxing.size())
		{
			return false;
		}
		const s_t_pet_shengxin_item& si = t_shengxing->shengxing[color];
		if (!fenjie)
		{
			suipian += si.suipian;
		}
		else
		{
			suipian += si.suipian * t_pet->shouhun;
		}
		shitou += si.shitou;
		gold += si.gold;
	}

	if (suipian > 0)
	{
		if (!fenjie)
		{
			rds.add_reward(2, t_pet->suipian_id, suipian);
		}
		else
		{
			rds.add_reward(1, resource::XINPIAN, suipian / 2);
		}
	}
	if (shitou > 0)
	{
		rds.add_reward(2, 110010001, shitou);
	}
	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}

	return true;
}
