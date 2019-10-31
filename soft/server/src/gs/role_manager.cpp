#include "role_manager.h"
#include "role_config.h"
#include "gs_message.h"
#include "item_operation.h"
#include "role_operation.h"
#include "utils.h"
#include "player_config.h"
#include "item_config.h"
#include "item_def.h"
#include "player_operation.h"
#include "equip_operation.h"
#include "equip_config.h"
#include "pvp_operation.h"
#include "rank_operation.h"
#include "social_operation.h"
#include "huodong_pool.h"

RoleManager::RoleManager()
{

}

RoleManager::~RoleManager()
{

}

int RoleManager::init()
{
	if (-1 == sRoleConfig->parse())
	{
		return -1;
	}
	return 0;
}

int RoleManager::fini()
{
	return 0;
}

void RoleManager::terminal_zhenxing(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_zhenxing msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;


	if (msg.index() == -1)
	{
		for (int i = 0; i < player->houyuan_size(); ++i)
		{
			if (player->houyuan(i) == msg.role_guid())
			{
				GLOBAL_ERROR;
				return;
			}
		}

		int zindex = -1;
		int zzcount = 0;
		for (int i = 0; i < player->zhenxing_size(); ++i)
		{
			if (player->zhenxing(i) == msg.role_guid())
			{
				zindex = i;
			}
			if (player->zhenxing(i) > 0)
			{
				zzcount += 1;
			}
		}
		if (zindex == -1)
		{
			GLOBAL_ERROR;
			return;
		}
		if (zzcount <= 3)
		{
			GLOBAL_ERROR;
			return;
		}

		dhc::role_t *src_role = POOL_GET_ROLE(msg.role_guid());
		if (!src_role)
		{
			GLOBAL_ERROR;
			return;
		}

		dhc::equip_t* equip = 0;
		for (int i = 0; i < src_role->zhuangbeis_size(); ++i)
		{
			equip = POOL_GET_EQUIP(src_role->zhuangbeis(i));
			if (equip)
			{
				equip->set_role_guid(0);
			}
			src_role->set_zhuangbeis(i, 0);
		}
		dhc::treasure_t* treasure = 0;
		for (int i = 0; i < src_role->treasures_size(); ++i)
		{
			treasure = POOL_GET_TREASURE(src_role->treasures(i));
			if (treasure)
			{
				treasure->set_role_guid(0);
			}
			src_role->set_treasures(i, 0);
		}
		dhc::pet_t *pet = POOL_GET_PET(src_role->pet());
		if (pet)
		{
			pet->set_role_guid(0);
		}
		src_role->set_pet(0);
		player->set_zhenxing(zindex, 0);
		ResMessage::res_success(player, true, name, id);
		return;
	}

	if (msg.index() < 0 || msg.index() >= player->zhenxing_size())
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::role_t *target_role = POOL_GET_ROLE(player->zhenxing(msg.index()));
	dhc::role_t *src_role = POOL_GET_ROLE(msg.role_guid());
	if (!src_role)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < player->houyuan_size(); ++i)
	{
		if (player->houyuan(i) == msg.role_guid())
		{
			GLOBAL_ERROR;
			return;
		}
	}
	for (int i = 0; i < src_role->zhuangbeis_size(); ++i)
	{
		if (src_role->zhuangbeis(i) != 0)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	for (int i = 0; i < src_role->treasures_size(); ++i)
	{
		if (src_role->treasures(i) != 0)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	if (src_role->pet() != 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (target_role)
	{
		if (src_role->zhuangbeis_size() != target_role->zhuangbeis_size())
		{
			GLOBAL_ERROR;
			return;
		}
		if (src_role->treasures_size() != target_role->treasures_size())
		{
			GLOBAL_ERROR;
		}
		dhc::equip_t* equip = 0;
		for (int i = 0; i < target_role->zhuangbeis_size(); ++i)
		{
			equip = POOL_GET_EQUIP(target_role->zhuangbeis(i));
			if (equip)
			{
				equip->set_role_guid(msg.role_guid());
			}
			src_role->set_zhuangbeis(i, target_role->zhuangbeis(i));
			target_role->set_zhuangbeis(i, 0);
		}
		dhc::treasure_t* treasure = 0;
		for (int i = 0; i < target_role->treasures_size(); ++i)
		{
			treasure = POOL_GET_TREASURE(target_role->treasures(i));
			if (treasure)
			{
				treasure->set_role_guid(msg.role_guid());
			}
			src_role->set_treasures(i, target_role->treasures(i));
			target_role->set_treasures(i, 0);
		}
		src_role->set_pet(target_role->pet());
		target_role->set_pet(0);
		dhc::pet_t *pet = POOL_GET_PET(src_role->pet());
		if (pet)
		{
			pet->set_role_guid(msg.role_guid());
		}
	}
	player->set_zhenxing(msg.index(), msg.role_guid());

	sPvpList->set_sync(player);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_duixing(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_duixing msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.src_site() < 0 || msg.src_site() >= player->duixing_size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.dest_site() < 0 || msg.dest_site() >= player->duixing_size())
	{
		GLOBAL_ERROR;
		return;
	}

	int src_pos = player->duixing(msg.src_site());
	int dest_pos = player->duixing(msg.dest_site());
	
	player->set_duixing(msg.src_site(), dest_pos);
	player->set_duixing(msg.dest_site(), src_pos);

	sPvpList->set_sync(player);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_duixing_up(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	const s_t_duixing_up* t_duixing_up = sRoleConfig->get_duixing_up(player->duixing_level() + 1);
	if (!t_duixing_up)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->level() < t_duixing_up->player_level)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->gold() < t_duixing_up->gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}
	if (ItemOperation::item_num_templete(player, 50140001) < t_duixing_up->zhuangzhi)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::GOLD, t_duixing_up->gold, LOGWAY_ROLE_DUIXING_UP);
	ItemOperation::item_destory_templete(player, 50140001, t_duixing_up->zhuangzhi, LOGWAY_ROLE_DUIXING_UP);
	player->set_duixing_level(player->duixing_level() + 1);
	
	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_duixing_on(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_duixing_on msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->duixing_id() == msg.id())
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_duixing* t_duixing = sRoleConfig->get_duixing(msg.id());
	if (!t_duixing)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->level() < t_duixing->level)
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_duixing_id(msg.id());

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_houyuan(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_zhenxing msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.index() == -1)
	{
		int index = -1;
		for (int i = 0; i < player->houyuan_size(); ++i)
		{
			if (player->houyuan(i) == msg.role_guid())
			{
				index = i;
				break;
			}
		}
		if (index == -1)
		{
			GLOBAL_ERROR;
			return;
		}
		dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
		if (!role)
		{
			GLOBAL_ERROR;
			return;
		}
		player->set_houyuan(index, 0);
	}
	else
	{
		if (msg.index() < 0 || msg.index() >= player->houyuan_size())
		{
			GLOBAL_ERROR;
			return;
		}

		dhc::role_t *src_role = POOL_GET_ROLE(msg.role_guid());
		if (!src_role)
		{
			GLOBAL_ERROR;
			return;
		}
		for (int i = 0; i < player->zhenxing_size(); ++i)
		{
			if (player->zhenxing(i) == msg.role_guid())
			{
				GLOBAL_ERROR;
				return;
			}
		}
		for (int i = 0; i < src_role->zhuangbeis_size(); ++i)
		{
			if (src_role->zhuangbeis(i) != 0)
			{
				GLOBAL_ERROR;
				return;
			}
		}
		for (int i = 0; i < src_role->treasures_size(); ++i)
		{
			if (src_role->treasures(i) != 0)
			{
				GLOBAL_ERROR;
				return;
			}
		}
		if (src_role->pet() != 0)
		{
			GLOBAL_ERROR;
			return;
		}
		player->set_houyuan(msg.index(), msg.role_guid());
	}
	sPvpList->set_sync(player);
	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_guanghuan(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guanghuan_on msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	const s_t_guanghuan *t_guanghuan = sRoleConfig->get_guanghuan(msg.id());
	if (!t_guanghuan)
	{
		GLOBAL_ERROR;
		return;
	}

	bool has = false;
	for (int i = 0; i < player->guanghuan_size(); ++i)
	{
		if (player->guanghuan(i) == msg.id())
		{
			has = true;
			break;
		}
	}
	if (!has)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->guanghuan_id() == msg.id())
	{
		player->set_guanghuan_id(0);
	}
	else
	{
		player->set_guanghuan_id(msg.id());
	}

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_guanghuan_up(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guanghuan_level msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	int index = -1;
	for (int i = 0; i < player->guanghuan_size(); ++i)
	{
		if (player->guanghuan(i) == msg.id())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guanghuan *t_guanhuan = sRoleConfig->get_guanghuan(msg.id());
	if (!t_guanhuan)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guanghuan_level *t_guanhuan_level = sRoleConfig->get_guanghuan_level(player->guanghuan_level(index) + 1);
	if (!t_guanhuan_level)
	{
		GLOBAL_ERROR;
		return;
	}

	int color = t_guanhuan->color - 2;
	if (color < 0 || color >= t_guanhuan_level->cailiaos_.size())
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->gold() < t_guanhuan_level->cailiaos_[color].first)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	if (ItemOperation::item_num_templete(player, 50150001) < t_guanhuan_level->cailiaos_[color].second)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	player->set_guanghuan_level(index, player->guanghuan_level(index) + 1);
	PlayerOperation::player_dec_resource(player, resource::GOLD, t_guanhuan_level->cailiaos_[color].first, LOGWAY_ROLE_GUANGHUAN_LEVEL);
	ItemOperation::item_destory_templete(player, 50150001, t_guanhuan_level->cailiaos_[color].second, LOGWAY_ROLE_GUANGHUAN_LEVEL);
	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_guanghun_init(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_guanghuan_init msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->jewel() < 200)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	if (player->guanghuan_id() == msg.id())
	{
		GLOBAL_ERROR;
		return;
	}

	int index = -1;
	for (int i = 0; i < player->guanghuan_size(); ++i)
	{
		if (player->guanghuan(i) == msg.id())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->guanghuan_level(index) <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_guanghuan *t_guanhuan = sRoleConfig->get_guanghuan(msg.id());
	if (!t_guanhuan)
	{
		GLOBAL_ERROR;
		return;
	}

	int color = t_guanhuan->color - 2;
	int gold = 0;
	int num = 0;
	const s_t_guanghuan_level *t_level = 0;
	for (int i = 1; i <= player->guanghuan_level(index); ++i)
	{
		t_level = sRoleConfig->get_guanghuan_level(i);
		if (!t_level)
		{
			GLOBAL_ERROR;
			return;
		}
		
		if (color < 0 || color >= t_level->cailiaos_.size())
		{
			GLOBAL_ERROR;
			return;
		}
		
		gold += t_level->cailiaos_[color].first;
		num += t_level->cailiaos_[color].second;
	}

	s_t_rewards rds;
	if (gold > 0)
	{
		rds.add_reward(1, resource::GOLD, gold);
	}
	if (num > 0)
	{
		rds.add_reward(2, 50150001, num);
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, 200, LOGWAY_ROLE_GUANGHUAN_INIT);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ROLE_GUANGHUAN_INIT);
	player->set_guanghuan_level(index, 0);
	protocol::game::smsg_equip_init smsg;
	ADD_MSG_REWARD(smsg, rds);
	ResMessage::res_equip_init(player, smsg, name, id);
}

void RoleManager::terminal_role_equip(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_equip msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (msg.index_size() != msg.equip_guid_size())
	{
		GLOBAL_ERROR;
		return;
	}
	int index = 0;
	uint64_t equip_guid = 0;
	for (int i = 0; i < msg.index_size(); ++i)
	{
		index = msg.index(i);
		equip_guid = msg.equip_guid(i);

		if (index < 0 || index >= role->zhuangbeis_size())
		{
			GLOBAL_ERROR;
			return;
		}
		if (role->zhuangbeis(index) == equip_guid)
		{
			GLOBAL_ERROR;
			return;
		}
		dhc::equip_t *equip = 0;
		if (equip_guid > 0)
		{
			equip = POOL_GET_EQUIP(equip_guid);
			if (!equip)
			{
				GLOBAL_ERROR;
				return;
			}
			if (equip->role_guid() > 0)
			{
				GLOBAL_ERROR;
				return;
			}
			s_t_equip *t_equip = sEquipConfig->get_equip(equip->template_id());
			if (!t_equip)
			{
				GLOBAL_ERROR;
				return;
			}
			if (t_equip->type - 1 != index)
			{
				GLOBAL_ERROR;
				return;
			}
		}
		if (role->zhuangbeis(index) > 0)
		{
			dhc::equip_t *equip1 = POOL_GET_EQUIP(role->zhuangbeis(index));
			if (!equip1)
			{
				GLOBAL_ERROR;
				return;
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

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_chouka(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_chouka msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.type() < 1 || msg.type() > 30)
	{
		GLOBAL_ERROR;
		return;
	}

	if (EquipOperation::is_equip_full(player))
	{
		PERROR(ERROR_EQUIP_FULL);
		return;
	}

	int num = 1;
	int jifen = 1;
	if (msg.type() == 1)
	{
		if (ItemOperation::item_num_templete(player, 80010001) < 1)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
		ItemOperation::item_destory_templete(player, 80010001, 1, LOGWAY_ROLE_CHOUKA);
	}
	else if (msg.type() == 10)
	{
		num = 10;
		if (ItemOperation::item_num_templete(player, 80010001) < 10)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
		ItemOperation::item_destory_templete(player, 80010001, 10, LOGWAY_ROLE_CHOUKA);
	
	}
	else if (msg.type() == 2)
	{
		uint64_t now = game::timer()->now();
		if (now > player->ck2_free_time())
		{
			player->set_ck2_free_time(now + 48 * 60 * 60000);
		}
		else if(ItemOperation::item_num_templete(player, 80010002) < 1 && player->jewel() < gCONST(CONST_CHOUKA))
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
		else if (ItemOperation::item_num_templete(player, 80010002) >= 1)
		{
			ItemOperation::item_destory_templete(player, 80010002, 1, LOGWAY_ROLE_CHOUKA);
		}
		else
		{
			if (player->jewel() < gCONST(CONST_CHOUKA))
			{
				PERROR(ERROR_JEWEL);
				return;
			}
			PlayerOperation::player_dec_resource(player, resource::JEWEL, gCONST(CONST_CHOUKA), LOGWAY_ROLE_CHOUKA);
		}
		PlayerOperation::player_add_active(player, 550, 1);
	}
	else if (msg.type() == 20)
	{
		num = 10;
		if (ItemOperation::item_num_templete(player, 80010002) < 10)
		{
			if (player->jewel() < gCONST(CONST_CHOUKA_TEN))
			{
				PERROR(ERROR_JEWEL);
				return;
			}
			PlayerOperation::player_dec_resource(player, resource::JEWEL, gCONST(CONST_CHOUKA_TEN), LOGWAY_ROLE_CHOUKA);
		}
		else
		{
			ItemOperation::item_destory_templete(player, 80010002, 10, LOGWAY_ROLE_CHOUKA);
		}
		PlayerOperation::player_add_active(player, 550, 1);
	}
	else if (msg.type() == 3)
	{
		num = 1;
		if (player->youqingdian() < 10)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
		PlayerOperation::player_dec_resource(player, resource::YOUQINGDIAN, 10, LOGWAY_ROLE_CHOUKA);
		PlayerOperation::player_add_active(player, 550, 1);
	}
	else if (msg.type() == 30)
	{
		num = 10;
		if (player->youqingdian() < 100)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
		PlayerOperation::player_dec_resource(player, resource::YOUQINGDIAN, 100, LOGWAY_ROLE_CHOUKA);
		PlayerOperation::player_add_active(player, 550, 1);
	}

	dhc::role_t *role = 0;
	const s_t_item* t_item = 0;
	const s_t_role* t_role = 0;
	s_t_chouka *t_chouka = 0;
	protocol::game::smsg_chouka smsg;
	std::string s = "";
	int choukas = 0;

	for (int i = 0; i < num; ++i)
	{
		int type = 2;
		if (msg.type() == 1 || msg.type() == 10)
		{
			type = 0;
		}
		else if(msg.type() == 2 || msg.type() == 20)
		{
			type = 1;
		}
		t_chouka = sRoleConfig->get_chouka(player, type);
		if (!t_chouka)
		{
			GLOBAL_ERROR;
			return;
		}
		t_item = sItemConfig->get_item(t_chouka->id);
		if (type == 1 && player->ck_num(1) == 0)
		{
			t_item = sItemConfig->get_item(30010101);
		}
		if (!t_item || t_item->type != IT_ROLE_SUIPIAN)
		{
			GLOBAL_ERROR;
			return;
		}
		t_role = sRoleConfig->get_role(t_item->def1);
		if (!t_role)
		{
			GLOBAL_ERROR;
			return;
		}
		if (t_role->font_color >= 4)
		{
			if (choukas > 0)
			{
				s += " ";
			}
			s += ItemOperation::get_color(t_item->font_color, t_role->name);
			choukas++;
		}
		if (t_chouka->type == 1)
		{
			if (RoleOperation::has_t_role(player, t_role->id))
			{
				ItemOperation::item_add_template(player, t_item->id, t_item->def2, LOGWAY_ROLE_CHOUKA);
				smsg.add_item_ids(t_item->id);
				smsg.add_item_nums(t_item->def2);
			}
			else
			{
				smsg.add_item_ids(t_item->id);
				smsg.add_item_nums(0);
				role = RoleOperation::role_create(player, t_role->id, 1, 0, LOGWAY_ROLE_CHOUKA);
				if (role)
				{
					smsg.add_roles()->CopyFrom(*role);
				}
			}
		}
		else
		{
			ItemOperation::item_add_template(player, t_item->id, t_chouka->num, LOGWAY_ROLE_CHOUKA);
			smsg.add_item_ids(t_item->id);
			smsg.add_item_nums(t_chouka->num);
		}
		player->set_ck_num(type, player->ck_num(type) + 1);
	}
	
	smsg.set_type(msg.type());
	smsg.set_ck2_free_time(player->ck2_free_time());
	smsg.set_jewel(player->jewel());

	if (choukas > 0)
	{
		SocialOperation::gundong_server("t_server_language_text_chouka", player->name(), s, "");
	}
	ResMessage::res_chouka(player, smsg, name, id);
}

void RoleManager::terminal_role_upgrade(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_upgrade msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK_NO_GOLD;

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_exp *t_tpexp = sPlayerConfig->get_exp(msg.level());
	if (!t_tpexp)
	{
		GLOBAL_ERROR;
		return;
	}

	int _yl = 0;
	int _end_level = msg.level();

	for (int i = role->level() + 1; i <= _end_level; i++)
	{
		s_t_exp *t_exp = sPlayerConfig->get_exp(i);

		if (!t_exp)
		{
			GLOBAL_ERROR;
			return;
		}

		_yl += RoleOperation::get_role_exp(t_role, t_exp);
	}

	if (player->yuanli() < _yl)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->gold() < _yl)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	role->set_level(_end_level);
	PlayerOperation::player_dec_resource(player, resource::YUANLI, _yl, LOGWAY_ROLE_CHONGNENG);
	PlayerOperation::player_dec_resource(player, resource::GOLD, _yl, LOGWAY_ROLE_CHONGNENG);
	PlayerOperation::player_add_active(player, 700, 1);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_tupo(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_tupo msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;
	
	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->player_guid() != player->guid())
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_tupo *t_tupo = sRoleConfig->get_tupo(role->glevel() + 1);
	if (!t_tupo)
	{
		GLOBAL_ERROR;
		return;
	}
	
	if (role->level() < t_tupo->level)
	{
		GLOBAL_ERROR;
		return;
	}

	//改为消耗碎片
	int _sp_id = sItemConfig->get_suipian(role->template_id());
	if (!_sp_id)
	{
		GLOBAL_ERROR;
		return;
	}

	int index = t_role->font_color - 2;
	if (index < 0 || index >= t_tupo->suipian.size())
	{
		GLOBAL_ERROR;
		return;
	}
	int num = ItemOperation::item_num_templete(player, _sp_id);
	if (num < t_tupo->suipian[index])
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->gold() < t_tupo->gold)
	{
		GLOBAL_ERROR;
		return;
	}

	ItemOperation::item_destory_templete(player, _sp_id, t_tupo->suipian[index], LOGWAY_ROLE_TUPO);
	PlayerOperation::player_dec_resource(player, resource::GOLD, t_tupo->gold, LOGWAY_ROLE_TUPO);

	role->set_glevel(role->glevel() + 1);
	player->set_sj_task_num(player->sj_task_num() + 1);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_jinjie(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_jinjie msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->player_guid() != player->guid())
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_jinjie * t_jinjie = sRoleConfig->get_jinjie(role->jlevel() + 1);
	if (!t_jinjie)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_role * t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->level() < t_jinjie->level)
	{
		GLOBAL_ERROR;
		return;
	}
	if (ItemOperation::item_num_templete(player, t_jinjie->clty) < t_jinjie->clty_num)
	{
		GLOBAL_ERROR;
		return;
	}
	int num1 = t_jinjie->clfy_num;
	if (t_role->job == 1)
	{
		num1 = t_jinjie->clfy_num1;
	}
	if (ItemOperation::item_num_templete(player, t_jinjie->clfy) < num1)
	{
		GLOBAL_ERROR;
		return;
	}
	int num2 = t_jinjie->clgj_num;
	if (t_role->job == 2)
	{
		num2 = t_jinjie->clgj_num1;
	}
	if (ItemOperation::item_num_templete(player, t_jinjie->clgj) < num2)
	{
		GLOBAL_ERROR;
		return;
	}
	int num3 = t_jinjie->clmf_num;
	if (t_role->job == 3)
	{
		num3 = t_jinjie->clmf_num1;
	}
	if (ItemOperation::item_num_templete(player, t_jinjie->clmf) < num3)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->gold() < t_jinjie->gold)
	{
		GLOBAL_ERROR;
		return;
	}

	ItemOperation::item_destory_templete(player, t_jinjie->clty, t_jinjie->clty_num, LOGWAY_ROLE_JINJIE);
	ItemOperation::item_destory_templete(player, t_jinjie->clfy, num1, LOGWAY_ROLE_JINJIE);
	ItemOperation::item_destory_templete(player, t_jinjie->clgj, num2, LOGWAY_ROLE_JINJIE);
	ItemOperation::item_destory_templete(player, t_jinjie->clmf, num3, LOGWAY_ROLE_JINJIE);
	PlayerOperation::player_dec_resource(player, resource::GOLD, t_jinjie->gold, LOGWAY_ROLE_JINJIE);
	role->set_jlevel(role->jlevel() + 1);

	player->set_jjie_task_num(player->jjie_task_num() + 1);
	PlayerOperation::player_add_active(player, 750, 1);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_duihuan(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_duihuan msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	s_t_item *t_item = sItemConfig->get_item(msg.item_id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_item->type != IT_ROLE_SUIPIAN)
	{
		GLOBAL_ERROR;
		return;
	}

	int _item_num =  ItemOperation::item_num_templete(player,t_item->id);

	if (_item_num < t_item->def2)
	{
		GLOBAL_ERROR;
		return;
	}

	if (RoleOperation::has_role(player, t_item->def1))
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_role *t_role = sRoleConfig->get_role(t_item->def1);
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::role_t *role = RoleOperation::role_create(player, t_item->def1, 1, 0, LOGWAY_ROLE_DUIHUAN);
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	ItemOperation::item_destory_templete(player, t_item->id, t_item->def2, LOGWAY_ROLE_DUIHUAN);

	ResMessage::res_role_duihuan(player, role, name, id);
}

void RoleManager::terminal_role_suipian(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_suipian msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	s_t_item *t_item = sItemConfig->get_item(msg.item_id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_item->type != IT_ROLE_SUIPIAN)
	{
		GLOBAL_ERROR;
		return;
	}

	int num = ItemOperation::item_num_templete(player, t_item->id);
	if (num < t_item->def2)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	ItemOperation::item_add_template(player, t_item->def4, 1, LOGWAY_ROLE_SUIPIAN);
	ItemOperation::item_destory_templete(player, t_item->id, t_item->def2, LOGWAY_ROLE_SUIPIAN);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_skillup(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_skillup msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->player_guid() != player->guid())
	{
		GLOBAL_ERROR;
		return;
	}

	int index = msg.index();
	if (index < 0 || index >= role->jskill_level_size())
	{
		GLOBAL_ERROR;
		return;
	}

	int level = role->jskill_level(index);
	const s_t_skillup* t_skillup = sRoleConfig->get_skillup(level + 1);
	if (!t_skillup)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->gold() < t_skillup->gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	if (ItemOperation::item_num_templete(player, 50090001) < t_skillup->item_num)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	ItemOperation::item_destory_templete(player, 50090001, t_skillup->item_num, LOGWAY_ROLE_SKILL);
	PlayerOperation::player_dec_resource(player, resource::GOLD, t_skillup->gold, LOGWAY_ROLE_SKILL);
	role->set_jskill_level(index, level + 1);
	PlayerOperation::player_add_active(player, 830, 1);

	ResMessage::res_role_skillup(player, 0, 0, name, id);
}

void RoleManager::terminal_role_shengpin(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_shengpin msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t* role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->player_guid() != player_guid)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_role* t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_item* t_suipian = sItemConfig->get_item(sItemConfig->get_suipian(role->template_id()));
	if (!t_suipian)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_role_shengpin* t_current_shengpin = sRoleConfig->get_role_shengpin(role->pinzhi());
	if (!t_current_shengpin)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_role_shengpin* t_shenpin = sRoleConfig->get_role_shengpin(t_current_shengpin->next_pinzhi);
	if (!t_shenpin)
	{
		GLOBAL_ERROR;
		return;
	}


	if (role->level() < t_shenpin->level)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, 50130001) < t_shenpin->shengpinshi)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	if (player->gold() < t_shenpin->gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	if (ItemOperation::item_num_templete(player, 50110001) < t_shenpin->hongsehuobanzhili)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	if (ItemOperation::item_num_templete(player, t_suipian->id) < t_shenpin->suipian)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	if (player->jjc_point() < t_shenpin->zhanhun)
	{
		PERROR(ERROR_JJCPOINT);
		return;
	}

	role->set_pinzhi(t_shenpin->pinzhi);

	if (t_shenpin->shengpinshi > 0)
		ItemOperation::item_destory_templete(player, 50130001, t_shenpin->shengpinshi, LOGWAY_ROLE_SHENGPIN);
	if (t_shenpin->gold > 0)
		PlayerOperation::player_dec_resource(player, resource::GOLD, t_shenpin->gold, LOGWAY_ROLE_SHENGPIN);
	if (t_shenpin->hongsehuobanzhili > 0)
		ItemOperation::item_destory_templete(player, 50110001, t_shenpin->hongsehuobanzhili, LOGWAY_ROLE_SHENGPIN);
	if (t_shenpin->suipian > 0)
		ItemOperation::item_destory_templete(player, t_suipian->id, t_shenpin->suipian, LOGWAY_ROLE_SHENGPIN);
	if (t_shenpin->zhanhun > 0)
		PlayerOperation::player_dec_resource(player, resource::ZHANHUN, t_shenpin->zhanhun, LOGWAY_ROLE_SHENGPIN);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_bskillup(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_bskillup msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t* role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->player_guid() != player_guid)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_role* t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_role_bskill *t_role_bskill = sRoleConfig->get_role_bskill(role->template_id(), role->bskill_level() + 1);
	if (!t_role_bskill)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_role_bskill_up* t_role_bskill_up = sRoleConfig->get_role_bskill_up(role->bskill_level() + 1);
	if (!t_role_bskill_up)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, 50090001) < t_role_bskill_up->jls)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	if (player->gold() < t_role_bskill_up->gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	for (int i = 0; i < t_role_bskill->defs.size(); ++i)
	{
		if (!RoleOperation::check_bskill(player, role, t_role_bskill->defs[i]))
		{
			GLOBAL_ERROR;
			return;
		}
	}

	role->set_bskill_level(role->bskill_level() + 1);
	PlayerOperation::player_dec_resource(player, resource::GOLD, t_role_bskill_up->gold, LOGWAY_ROLE_BSKILLUP);
	ItemOperation::item_destory_templete(player, 50090001, t_role_bskill_up->jls, LOGWAY_ROLE_BSKILLUP);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_suipian_gaizao(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_suipian_gaizao msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	int num = msg.src_num();
	if (num <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_item* t_item = sItemConfig->get_item(msg.src_suipian());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_item->type != IT_ROLE_SUIPIAN)
	{
		GLOBAL_ERROR;
		return;
	}
	const s_t_role* t_role = sRoleConfig->get_role(t_item->def1);
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, msg.src_suipian()) < num)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	const s_t_item* t_item_target = sItemConfig->get_item(msg.target_suipian());
	if (!t_item_target)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_item_target->type != IT_ROLE_SUIPIAN)
	{
		GLOBAL_ERROR;
		return;
	}
	const s_t_role* t_role_target = sRoleConfig->get_role(t_item_target->def1);
	if (!t_role_target)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_role_gaizao *t_role_gaizao_src = sRoleConfig->get_role_gaizao(t_role->id);
	if (!t_role_gaizao_src)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_role_gaizao *t_role_gaizao_target = sRoleConfig->get_role_gaizao(t_role_target->id);
	if (!t_role_gaizao_target)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_role_gaizao_src->type != t_role_gaizao_target->type)
	{
		GLOBAL_ERROR;
		return;
	}
	int jewel = t_role_gaizao_target->jewel * num;
	int zhanhun = t_role_gaizao_target->hun * num;
	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	if (player->jjc_point() < zhanhun)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_ROLE_SUIPIAN_GAIZAO);
	PlayerOperation::player_dec_resource(player, resource::ZHANHUN, zhanhun, LOGWAY_ROLE_SUIPIAN_GAIZAO);
	ItemOperation::item_destory_templete(player, msg.src_suipian(), num, LOGWAY_ROLE_SUIPIAN_GAIZAO);
	ItemOperation::item_add_template(player, msg.target_suipian(), num, LOGWAY_ROLE_SUIPIAN_GAIZAO);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_dress_on(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_dress_on msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}

	if (role->player_guid() != player_guid)
	{
		GLOBAL_ERROR;
		return;
	}
	
	if (msg.dress_id() != 0)
	{
		const s_t_role_dress *t_dress = sRoleConfig->get_role_dress(msg.dress_id());
		if (!t_dress)
		{
			GLOBAL_ERROR;
			return;
		}

		if (role->template_id() != t_dress->role)
		{
			GLOBAL_ERROR;
			return;
		}

		if (role->glevel() < t_dress->glevel)
		{
			GLOBAL_ERROR;
			return;
		}
	}

	role->set_dress_on_id(msg.dress_id());
	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_init(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_init msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->player_guid() != player->guid())
	{
		GLOBAL_ERROR;
		return;
	}
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		if (player->zhenxing(i) == role->guid())
		{
			GLOBAL_ERROR;
			return;
		}
	}
	for (int i = 0; i < player->houyuan_size(); ++i)
	{
		if (player->houyuan(i) == role->guid())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	const s_t_role *t_role = sRoleConfig->get_role(role->template_id());
	if (!t_role)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < 50)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	s_t_rewards rds;

	/// 返还充能使用的原力金币
	if (!RoleOperation::get_role_init_exp(player, role, rds))
	{
		GLOBAL_ERROR;
		return;
	}

	/// 返还突破基因和金币
	if (!RoleOperation::get_role_init_tupo(player, role, rds))
	{
		GLOBAL_ERROR;
		return;
	}

	/// 返还进阶的材料和金币
	if (!RoleOperation::get_role_init_jinjie(player, role, rds))
	{
		GLOBAL_ERROR;
		return;
	}

	/// 返还技能的技能石和金币
	if (!RoleOperation::get_role_init_skill(player, role, rds))
	{
		GLOBAL_ERROR;
		return;
	}

	/// 返还升品消耗的材料
	if (!RoleOperation::get_role_init_shenpin(player, role, rds))
	{
		GLOBAL_ERROR;
		return;
	}

	/// 返还专属技能的材料
	if (!RoleOperation::get_role_init_bskill(player, role, rds))
	{
		GLOBAL_ERROR;
		return;
	}
	rds.merge();

	PlayerOperation::player_add_reward(player, rds, LOGWAY_ROLE_INIT);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, 50, LOGWAY_ROLE_INIT);
	role->set_level(1);
	role->set_dress_on_id(0);

	for (int i = 0; i < role->jskill_level_size(); i++)
	{
		role->set_jskill_level(i, 1);
	}

	const s_t_role_bskill *t_role_bskill = sRoleConfig->get_role_bskill(role->template_id(), role->bskill_level() + 1);
	if (t_role_bskill)
	{
		int bskill_type = -1;
		for (int kmg = 0; kmg < t_role_bskill->defs.size(); ++kmg)
		{
			bskill_type = t_role_bskill->defs[kmg].type;
			if (bskill_type - 1 >= 0 && bskill_type - 1 < role->bskill_counts_size())
				role->set_bskill_counts(bskill_type - 1, 0);
		}
	}
	role->set_bskill_level(0);

	role->set_glevel(0);
	role->set_jlevel(0);
	role->set_pinzhi(t_role->pinzhi * 100);

	protocol::game::smsg_role_init msg1;
	msg1.set_yuanli(0);
	ADD_MSG_REWARD(msg1, rds);

	ResMessage::res_role_init(player, msg1, name, id);
}


void RoleManager::terminal_role_xq_look(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_xq_look msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	std::vector<uint64_t> _guids;
	std::vector<int> _xqs;

	for (int i = 0; i < player->roles_size(); i++)
	{
		dhc::role_t *role = POOL_GET_ROLE(player->roles(i));
		if (!role)
		{
			GLOBAL_ERROR;
			return;
		}

		const s_t_role *t_role = sRoleConfig->get_role(role->template_id());
		if (!t_role)
		{
			GLOBAL_ERROR;
			return;
		}

		uint64_t now = game::timer()->now();
		std::vector<int> _xs;

		for (int i = 0; i < 5; i++)
		{
			_xs.push_back(1);
			_xs.push_back(5);
		}
		for (int i = 0; i < 25; i++)
		{
			_xs.push_back(2);
			_xs.push_back(4);
		}
		for (int i = 0; i < 40; i++)
		{
			_xs.push_back(3);
		}
		if (t_role->font_color > 3 && now - role->xq_time() > 6 * 60 * 60000)
		{
			role->set_xq(_xs[Utils::get_int32(0, _xs.size() - 1)]);
			role->set_xq_time(now);

			_guids.push_back(role->guid());
			_xqs.push_back(role->xq());
		}
	}

	ResMessage::res_role_xq_look(player, _guids, _xqs, name, id);
}

void RoleManager::terminal_role_yh_select(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_yh_select msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->yh_roles_size() == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::role_t *role = POOL_GET_ROLE(msg.guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->player_guid() != player->guid())
	{
		GLOBAL_ERROR;
		return;
	}

	int _xq = role->xq();
	int _select = msg.index();
	int bxq = _xq;

	if (_select == 0)
	{
		_select = Utils::get_int32(0, 3);
	}
	if (_select == 1)
	{
		_xq ++ ;
	}
	else if (_select == 2)
	{
		_xq += 2;
	}
	else if (_select == 3)
	{
		//_xq -- ;
	}

	if (_xq > 5) _xq = 5;
	if (_xq < 1) _xq = 1;


	uint64_t now = game::timer()->now();
	role->set_xq_time(now);
	role->set_xq(_xq);

	int jewel = 0;
	if (_xq - bxq >= 2 || 
		_select == 2)
	{
		jewel = 40;
		PlayerOperation::player_add_resource(player, resource::JEWEL, 40, LOGWAY_ROLE_XINQING);
	}

	if (game::timer()->hour() >= 4 && game::timer()->hour() < 10)
	{
		player->set_yh_hour(1);
	}
	else if (game::timer()->hour() >= 10 && game::timer()->hour() < 15)
	{
		player->set_yh_hour(2);
	}
	else if (game::timer()->hour() >= 15 && game::timer()->hour() < 19)
	{
		player->set_yh_hour(3);
	}
	else if (game::timer()->hour() >= 19 && game::timer()->hour() < 23)
	{
		player->set_yh_hour(4);
	}
	else if (game::timer()->hour() >= 23 || game::timer()->hour() < 4)
	{
		player->set_yh_hour(5);
	}
	
	player->set_yh_time(now);
	player->clear_yh_roles();

	ResMessage::res_role_yh_select(player, role->guid(), _xq, jewel, name, id);
}

void RoleManager::terminal_role_huiyi_chou(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_huiyi_chou msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	int jewel = gCONST(CONST_MINGYUN);
	int num = 1;
	if (msg.type() == 1)
	{
		jewel = gCONST(CONST_MINGYUN_FIVE);
		num = 5;
	}
	else
	{
		if (player->huiyi_chou_num() == 0)
		{
			player->set_huiyi_chou_num(1);
			jewel = 0;
		}
	}

	bool bichu = false;
	if (num == 5)
	{
		bichu = true;
	}
	else
	{
		player->set_huiyi_fan_num(player->huiyi_fan_num() + 1);
		if (player->huiyi_fan_num() >= 5)
		{
			player->set_huiyi_fan_num(0);
			bichu = true;
		}
	}

	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	

	const s_t_huiyin_chou* t_chou = 0;
	const s_t_item* t_item = 0;
	s_t_rewards rds;
	protocol::game::smsg_role_huiyi_chou smsg;
	for (int i = 0; i < num; ++i)
	{
		if (bichu)
		{
			bichu = false;
			t_chou = sRoleConfig->get_huiyi_chou(player, 2);
		}
		else
		{
			t_chou = sRoleConfig->get_huiyi_chou(player, 1);
		}
		if (!t_chou)
		{
			GLOBAL_ERROR;
			return;
		}
		t_item = sItemConfig->get_item(t_chou->id);
		if (!t_item)
		{
			GLOBAL_ERROR;
			return;
		}

		rds.add_reward(2, t_chou->id, 1);
		rds.add_reward(1, resource::LUCK_POINT, t_chou->point);
		smsg.add_ids(t_chou->id);
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ROLE_HUIYI_CHOU);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_ROLE_HUIYI_CHOU);
	PlayerOperation::player_add_active(player, 2200, num);
	sHuodongPool->huodong_active(player, HUODONG_COND_FATE_COUNT, num);

	ResMessage::res_role_huiyi_chou(player, smsg, name, id);
}

void RoleManager::terminal_role_huiyi_jihuo(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_huiyi_jihuo msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	for (int i = 0; i < player->huiyi_jihuos_size(); ++i)
	{
		if (player->huiyi_jihuos(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	const s_t_huiyi_jihuo* t_jihuo = sRoleConfig->get_huiyi_jihuo(msg.id());
	if (!t_jihuo)
	{
		GLOBAL_ERROR;
		return;
	}

	for (std::vector<int>::size_type i = 0; i < t_jihuo->huiyi.size(); ++i)
	{
		if (ItemOperation::item_num_templete(player, t_jihuo->huiyi[i]) < 1)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}

	player->add_huiyi_jihuos(msg.id());
	player->add_huiyi_jihuo_starts(0);
	player->set_huiyi_shoujidu(player->huiyi_shoujidu() + t_jihuo->huiyi.size());
	
	if (player->huiyi_shoujidu() > player->huiyi_shoujidu_top())
	{
		player->set_huiyi_shoujidu_top(player->huiyi_shoujidu());
	}

	RankOperation::check_value(player, e_rank_huiyi, player->huiyi_shoujidu());
	for (std::vector<int>::size_type i = 0; i < t_jihuo->huiyi.size(); ++i)
	{
		ItemOperation::item_destory_templete(player, t_jihuo->huiyi[i], 1, LOGWAY_ROLE_HUIYI_JIHUO);
	}

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_huiyi_starts(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_huiyi_jihuo msg;  
	if (!msg.ParseFromString(data))             
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;
	
	int index = -1;
	for (int i = 0; i < player->huiyi_jihuos_size(); ++i)  
	{
		if (player->huiyi_jihuos(i) == msg.id())
		{
			index = i;
			break;
		}
	}

	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->huiyi_jihuo_starts(index) == 5) 
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_huiyi_jihuo* t_jihuo = sRoleConfig->get_huiyi_jihuo(msg.id());
	if (!t_jihuo)
	{
		GLOBAL_ERROR;
		return;
	}

	for (std::vector<int>::size_type i = 0; i < t_jihuo->huiyi.size(); ++i)
	{
		if (ItemOperation::item_num_templete(player, t_jihuo->huiyi[i]) < 1)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}
	player->set_huiyi_jihuo_starts(index, player->huiyi_jihuo_starts(index) + 1);
	player->set_huiyi_shoujidu(player->huiyi_shoujidu() + t_jihuo->huiyi.size());
	RankOperation::check_value(player, e_rank_huiyi, player->huiyi_shoujidu());

	if (player->huiyi_shoujidu() > player->huiyi_shoujidu_top())
	{
		player->set_huiyi_shoujidu_top(player->huiyi_shoujidu());
	}

	for (std::vector<int>::size_type i = 0; i < t_jihuo->huiyi.size(); ++i)
	{
		ItemOperation::item_destory_templete(player, t_jihuo->huiyi[i], 1, LOGWAY_ROLE_HUIYI_STAR);
	}
	ResMessage::res_success(player, true, name, id);
}	

void  RoleManager::termianl_role_huiyi_reset(const std::string &data, const std::string &name, int id)    //回忆星级重置
{
	protocol::game::cmsg_role_huiyi_jihuo msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->jewel() < 50)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	int index = -1;
	for (int i = 0; i < player->huiyi_jihuos_size(); ++i)
	{
		if (player->huiyi_jihuos(i) == msg.id())
		{
			index = i;
			break;
		}
	}

	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->huiyi_jihuo_starts(index) == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_huiyi_jihuo* t_jihuo = sRoleConfig->get_huiyi_jihuo(msg.id());
	if (!t_jihuo)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, 50, LOGWAY_ROLE_HUIYI_RESET);

	for (std::vector<int>::size_type i = 0; i < t_jihuo->huiyi.size(); ++i)
	{
		ItemOperation::item_add_template(player, t_jihuo->huiyi[i], player->huiyi_jihuo_starts(index), LOGWAY_ROLE_HUIYI_RESET);
	}
	player->set_huiyi_shoujidu(player->huiyi_shoujidu() - t_jihuo->huiyi.size() * player->huiyi_jihuo_starts(index));

	player->set_huiyi_jihuo_starts(index, 0);
	RankOperation::check_value(player, e_rank_huiyi, player->huiyi_shoujidu());
	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_huiyi_fate_zhanpu(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->huiyi_zhanpu_flag() == 1)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_vip* t_vip = sPlayerConfig->get_vip(player->vip());
	if (!t_vip)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->huiyi_zhanpu_num() >= t_vip->huiyi)
	{
		GLOBAL_ERROR;
		return;
	}

	std::set<int> ids;
	for (int i = 0; i < player->huiyi_zhanpus_size(); ++i)
	{
		ids.insert(player->huiyi_zhanpus(i));
	}
	player->clear_huiyi_zhanpus();

	int fid = 0;
	for (int i = 0; i < 3; ++i)
	{
		fid = sRoleConfig->get_huiyi_fate_random(ids);
		if (fid == 0)
		{
			GLOBAL_ERROR;
			return;
		}
		ids.insert(fid);
		player->add_huiyi_zhanpus(fid);
	}
	player->set_huiyi_zhanpu_flag(1);
	player->set_huiyi_zhanpu_num(player->huiyi_zhanpu_num() + 1);
	PlayerOperation::player_add_active(player, 2300, 1);

	ResMessage::res_role_zhanpu(player, name, id);
}

void RoleManager::terminal_role_huiyi_fate_gaiyun(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->huiyi_zhanpu_flag() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	int jewel = 0;
	if (player->huiyi_gaiyun_num() >= 3)
	{
		jewel = 100;
	}
	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	std::set<int> ids;
	for (int i = 0; i < player->huiyi_zhanpus_size(); ++i)
	{
		ids.insert(player->huiyi_zhanpus(i));
	}
	player->clear_huiyi_zhanpus();

	int fid = 0;
	for (int i = 0; i < 3; ++i)
	{
		fid = sRoleConfig->get_huiyi_fate_random(ids);
		if (fid == 0)
		{
			GLOBAL_ERROR;
			return;
		}
		ids.insert(fid);
		player->add_huiyi_zhanpus(fid);
	}
	if (jewel > 0)
	{
		PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_ROLE_FATE_GAIYUN);
	}
	else
	{
		player->set_huiyi_gaiyun_num(player->huiyi_gaiyun_num() + 1);
	}

	ResMessage::res_role_zhanpu(player, name, id);
}

void RoleManager::terminal_role_huiyi_fate_fanpai(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_huiyi_fanpai msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->huiyi_zhanpu_flag() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->huiyi_zhanpus_size() != 3)
	{
		GLOBAL_ERROR;
		return;
	}

	bool has_id = false;
	for (int i = 0; i < player->huiyi_zhanpus_size(); ++i)
	{
		if (player->huiyi_zhanpus(i) == msg.id())
		{
			has_id = true;
			break;
		}
	}
	if (!has_id)
	{
		GLOBAL_ERROR;
		return;
	}
	const s_t_huiyi_fate* t_fate = sRoleConfig->get_huiyi_fate(msg.id());
	if (!t_fate)
	{
		GLOBAL_ERROR;
		return;
	}
	ItemOperation::item_add_template(player, t_fate->id, 1, LOGWAY_ROLE_FATE_FANPAI);
	PlayerOperation::player_add_resource(player, resource::HUIYI_POINT, t_fate->point, LOGWAY_ROLE_FATE_FANPAI);
	player->set_huiyi_zhanpu_flag(0);

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_role_yh_look(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_xq_look msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	int _type = 6;

	if (game::timer()->hour() >= 4 && game::timer()->hour() < 10)
	{
		_type = 1;
	}
	else if (game::timer()->hour() >= 10 && game::timer()->hour() < 15)
	{
		_type = 2;
	}
	else if (game::timer()->hour() >= 15 && game::timer()->hour() < 19)
	{
		_type = 3;
	}
	else if (game::timer()->hour() >= 19 && game::timer()->hour() < 23)
	{
		_type = 4;
	}
	else if (game::timer()->hour() >= 23 || game::timer()->hour() < 4)
	{
		_type = 5;
	}

	uint64_t now = game::timer()->now();

	if (now - player->yh_time() > 3600000 * 12 
		|| (player->yh_roles_size() == 0 && player->yh_hour() != _type)
		|| (now - player->yh_update_time() > 1000 * 1800 && player->yh_hour() != _type))
	{
		player->set_yh_update_time(now);
		player->clear_yh_roles();

		std::vector<uint64_t> _guids;

		for (int i = 0; i < player->roles_size(); i++)
		{
			dhc::role_t *role = POOL_GET_ROLE(player->roles(i));
			if (!role)
			{
				GLOBAL_ERROR;
				return;
			}

			const s_t_role *t_role = sRoleConfig->get_role(role->template_id());
			if (!t_role)
			{
				GLOBAL_ERROR;
				return;
			}
			if (t_role->id == 505)
			{
				continue;
			}

			if (t_role->font_color > 3)
			{
				_guids.push_back(role->guid());
			}
		}

		while (player->yh_roles_size() < 5 && _guids.size() > 0)
		{
			int _index = Utils::get_int32(0, _guids.size() - 1);
			player->add_yh_roles(_guids[_index]);
			_guids.erase(_guids.begin() + _index);
		}
	}

	ResMessage::res_role_yh_look(player, name, id);
}

void RoleManager::terminal_role_fenjie(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_fj msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	int zhanhun = 0;
	s_t_rewards rds;
	for (int i = 0; i < msg.role_guid_size(); i++)
	{
		dhc::role_t *role = POOL_GET_ROLE(msg.role_guid(i));
		if (!role)
		{
			GLOBAL_ERROR;
			return;
		}
		if (role->player_guid() != player->guid())
		{
			GLOBAL_ERROR;
			return;
		}

		for (int mn = 0; mn < player->zhenxing_size(); ++mn)
		{
			if (player->zhenxing(mn) == role->guid())
			{
				GLOBAL_ERROR;
				return;
			}
		}

		for (int mn = 0; mn < player->houyuan_size(); ++mn)
		{
			if (player->houyuan(mn) == role->guid())
			{
				GLOBAL_ERROR;
				return;
			}
		}

		const s_t_role *t_role = sRoleConfig->get_role(role->template_id());
		if (!t_role)
		{
			GLOBAL_ERROR;
			return;
		}

		/// 充能
		if (!RoleOperation::get_role_init_exp(player, role, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 进阶
		if (!RoleOperation::get_role_init_jinjie(player, role, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 突破
		if (!RoleOperation::get_role_init_tupo(player, role, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 技能
		if (!RoleOperation::get_role_init_skill(player, role, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 升品
		if (!RoleOperation::get_role_init_shenpin(player, role, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 专属技能
		if (!RoleOperation::get_role_init_bskill(player, role, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		/// 红色伙伴之力
		if (t_role->jinghua > 0)
		{
			rds.add_reward(2, 50110001, t_role->jinghua);
		}

		/// 战魂
		zhanhun += t_role->exp;
	}
	rds.add_reward(1, resource::ZHANHUN, zhanhun);
	rds.merge();

	for (int i = 0; i < msg.role_guid_size(); i++)
	{
		RoleOperation::role_delete(player, msg.role_guid(i), LOGWAY_ROLE_FENJIE);
	}
	PlayerOperation::player_add_reward(player, rds, LOGWAY_ROLE_FENJIE);

	protocol::game::smsg_role_init msg1;
	msg1.set_yuanli(0);
	ADD_MSG_REWARD(msg1, rds);

	ResMessage::res_role_init(player, msg1, name, id);
}

void RoleManager::terminal_role_all_equip(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_role_all_equip msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	if (role->player_guid() != player_guid)
	{
		GLOBAL_ERROR;
		return;
	}

	std::vector<int> equip_indexs;
	std::vector<int> treasure_indexs;
	std::vector<uint64_t> equip_guids;
	std::vector<uint64_t> treasure_guids;
	for (int i = 0; i < msg.equip_index_size(); ++i)
	{
		equip_indexs.push_back(msg.equip_index(i));
	}
	for (int i = 0; i < msg.equip_guid_size(); ++i)
	{
		equip_guids.push_back(msg.equip_guid(i));
	}
	for (int i = 0; i < msg.treasure_index_size(); ++i)
	{
		treasure_indexs.push_back(msg.treasure_index(i));
	}
	for (int i = 0; i < msg.treasure_guid_size(); ++i)
	{
		treasure_guids.push_back(msg.treasure_guid(i));
	}

	if (!RoleOperation::role_mod_equip(role, equip_indexs, equip_guids))
	{
		GLOBAL_ERROR;
		return;
	}
	if (!RoleOperation::role_mod_treasure(role, treasure_indexs, treasure_guids))
	{
		GLOBAL_ERROR;
		return;
	}
	
	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_pet_on(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pet_on msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::pet_t *pet = POOL_GET_PET(msg.guid());
	if (!pet)
	{
		GLOBAL_ERROR;
		return;
	}

	if (pet->role_guid() != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	if (pet->guid() == player->pet_on())
	{
		player->set_pet_on(0);
	}
	else
	{
		player->set_pet_on(pet->guid());
	}

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_pet_guard(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pet_guard msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::pet_t *pet = POOL_GET_PET(msg.pet_guid());
	if (!pet)
	{
		GLOBAL_ERROR;
		return;
	}

	// 已上阵
	if (pet->guid() == player->pet_on())
	{
		GLOBAL_ERROR;
		return;
	}

	// 已守卫
	if (pet->role_guid() != 0)
	{
		if (pet->role_guid() != role->guid())
		{
			GLOBAL_ERROR;
			return;
		}
		role->set_pet(0);
		pet->set_role_guid(0);
	}
	else
	{
		dhc::pet_t *src_pet = POOL_GET_PET(role->pet());
		if (src_pet)
		{
			src_pet->set_role_guid(0);
		}

		pet->set_role_guid(role->guid());
		role->set_pet(pet->guid());
	}

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_pet_level(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pet_level msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (msg.ciliao_size() != msg.count_size())
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.ciliao_size() > 5)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::pet_t *pet = POOL_GET_PET(msg.guid());
	if (!pet)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_pet* t_pet = sRoleConfig->get_pet(pet->template_id());
	if (!t_pet)
	{
		GLOBAL_ERROR;
		return;
	}

	int exp = pet->exp();
	int level = pet->level();

	const s_t_exp *t_exp = sPlayerConfig->get_exp(level + 1);
	if (!t_exp)
	{
		GLOBAL_ERROR;
		return;
	}
	const s_t_item *t_item = 0;
	int gold = 0;
	bool full = false;
	for (int i = 0; i < msg.ciliao_size(); ++i)
	{
		t_item = sItemConfig->get_item(msg.ciliao(i));
		if (!t_item)
		{
			GLOBAL_ERROR;
			return;
		}
		if (ItemOperation::item_num_templete(player, msg.ciliao(i)) < msg.count(i))
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
		exp += t_item->def1 * msg.count(i);
		gold += t_item->def1 * msg.count(i);
		while (exp >= RoleOperation::get_pet_exp(t_pet, t_exp))
		{
			exp -= RoleOperation::get_pet_exp(t_pet, t_exp);
			level += 1;
			t_exp = sPlayerConfig->get_exp(level + 1);
			if (!t_exp)
			{
				exp = 0;
				full = true;
				break;
			}
		}
		if (!t_exp)
		{
			break;
		}
	}

	if (full)
	{
		gold = 0;
		for (int i = pet->level(); i < level; i++)
		{
			t_exp = sPlayerConfig->get_exp(i + 1);
			if (!t_exp)
			{
				GLOBAL_ERROR;
				return;
			}
			gold += RoleOperation::get_pet_exp(t_pet, t_exp);
		}
		gold -= pet->exp();
	}

	if (player->gold() < gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	pet->set_exp(exp);
	pet->set_level(level);

	PlayerOperation::player_dec_resource(player, resource::GOLD, gold, LOGWAY_PET_SKILLUP);
	for (int i = 0; i < msg.ciliao_size(); ++i)
	{
		ItemOperation::item_destory_templete(player, msg.ciliao(i), msg.count(i), LOGWAY_PET_SKILLUP);
	}
	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_pet_jinjie(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pet_jinjie msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::pet_t *pet = POOL_GET_PET(msg.guid());
	if (!pet)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_pet* t_pet = sRoleConfig->get_pet(pet->template_id());
	if (!t_pet)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.item_id() != 0)
	{
		if (msg.index() < 0 || msg.index() >= pet->jinjie_slot_size())
		{
			GLOBAL_ERROR;
			return;
		}
		if (pet->jinjie_slot(msg.index()) != 0)
		{
			GLOBAL_ERROR;
			return;
		}
		const s_t_pet_jinjie *t_jinjie = sRoleConfig->get_pet_jinjie(pet->jinjie());
		if (!t_jinjie)
		{
			GLOBAL_ERROR;
			return;
		}
		if (msg.index() >= t_jinjie->cailiao.size())
		{
			GLOBAL_ERROR;
			return;
		}
		if (t_jinjie->cailiao[msg.index()] != msg.item_id())
		{
			GLOBAL_ERROR;
			return;
		}
		const s_t_item *t_item = sItemConfig->get_item(msg.item_id());
		if (!t_item)
		{
			GLOBAL_ERROR;
			return;
		}
		const s_t_pet_jinjie_item *t_item_jinjie = sRoleConfig->get_pet_jinjie_item(msg.item_id());
		if (!t_item_jinjie)
		{
			GLOBAL_ERROR;
			return;
		}
		if (ItemOperation::item_num_templete(player, msg.item_id()) < 1)
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
		pet->set_jinjie_slot(msg.index(), msg.item_id());
		ItemOperation::item_destory_templete(player, msg.item_id(), 1, LOGWAY_PET_JINJIE);
	}
	else
	{
		const s_t_pet_jinjie *t_jinjie = sRoleConfig->get_pet_jinjie(pet->jinjie());
		if (!t_jinjie)
		{
			GLOBAL_ERROR;
			return;
		}

		if (pet->level() < t_jinjie->level)
		{
			PERROR(ERROR_LEVEL);
			return;
		}

		if (player->gold() < t_jinjie->gold)
		{
			PERROR(ERROR_GOLD);
			return;
		}

		bool has_cailiao = false;
		for (int i = 0; i < t_jinjie->cailiao.size(); ++i)
		{
			has_cailiao = false;
			for (int j = 0; j < pet->jinjie_slot_size(); ++j)
			{
				if (t_jinjie->cailiao[i] == pet->jinjie_slot(j))
				{
					has_cailiao = true;
					break;
				}
			}
			if (!has_cailiao)
			{
				PERROR(ERROR_CAILIAO);
				return;
			}
		}
		pet->set_jinjie(pet->jinjie() + 1);
		PlayerOperation::player_dec_resource(player, resource::GOLD, t_jinjie->gold, LOGWAY_PET_JINJIE);
		for (int i = 0; i < pet->jinjie_slot_size(); ++i)
		{
			pet->set_jinjie_slot(i, 0);
		}
	}

	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_pet_star(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pet_star msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::pet_t *pet = POOL_GET_PET(msg.guid());
	if (!pet)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_pet* t_pet = sRoleConfig->get_pet(pet->template_id());
	if (!t_pet)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_pet_shengxing *t_shengxing = sRoleConfig->get_pet_shengxing(pet->star() + 1);
	if (!t_shengxing)
	{
		GLOBAL_ERROR;
		return;
	}

	if (pet->level() < t_shengxing->level)
	{
		PERROR(ERROR_LEVEL);
		return;
	}

	int index = t_pet->color - 2;
	if (index < 0 || index >= t_shengxing->shengxing.size())
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_pet_shengxin_item& si = t_shengxing->shengxing[index];

	if (ItemOperation::item_num_templete(player, t_pet->suipian_id) < si.suipian)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	if (ItemOperation::item_num_templete(player, 110010001) < si.shitou)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	if (player->gold() < si.gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	ItemOperation::item_destory_templete(player, t_pet->suipian_id, si.suipian, LOGWAY_PET_STAR);
	ItemOperation::item_destory_templete(player, 110010001, si.shitou, LOGWAY_PET_STAR);
	PlayerOperation::player_dec_resource(player, resource::GOLD, si.gold, LOGWAY_PET_STAR);

	pet->set_star(pet->star() + 1);
	ResMessage::res_success(player, true, name, id);
}

void RoleManager::terminal_pet_suipian(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pet_duihuan msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->pets_size() >= 50)
	{
		PERROR(ERROR_PET_FULL);
		return;
	}

	const s_t_item *t_item = sItemConfig->get_item(msg.item_id());
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_item->type != IT_PET_SUIPIAN)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, t_item->id) < t_item->def2)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_pet *t_pet = sRoleConfig->get_pet(t_item->def1);
	if (!t_pet)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::pet_t *pet = RoleOperation::create_pet(player, t_item->def1, LOGWAY_PET_DUIHUAN);
	if (!pet)
	{
		GLOBAL_ERROR;
		return;
	}
	ItemOperation::item_destory_templete(player, t_item->id, t_item->def2, LOGWAY_PET_DUIHUAN);

	ResMessage::res_pet_duihuan(player, pet, name, id);
}

void RoleManager::terminal_pet_fenjie(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pet_init msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->jewel() < 200)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	if (msg.guid_size() <= 0)
	{
		GLOBAL_ERROR;
		return;
	}


	dhc::pet_t *pet = 0;
	const s_t_pet *t_pet = 0;
	const s_t_item *t_item = 0;
	s_t_rewards rds;
	int xinpain = 0;
	for (int i = 0; i < msg.guid_size(); ++i)
	{
		pet = POOL_GET_PET(msg.guid(i));
		if (!pet)
		{
			GLOBAL_ERROR;
			return;
		}

		if (pet->guid() == player->pet_on())
		{
			GLOBAL_ERROR;
			return;
		}

		if (pet->role_guid() != 0)
		{
			GLOBAL_ERROR;
			return;
		}

		t_pet = sRoleConfig->get_pet(pet->template_id());
		if (!t_pet)
		{
			GLOBAL_ERROR;
			return;
		}

		t_item = sItemConfig->get_item(t_pet->suipian_id);
		if (!t_item)
		{
			GLOBAL_ERROR;
			return;
		}

		if (!RoleOperation::get_pet_init_level(player, pet, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		if (!RoleOperation::get_pet_init_jinjie(player, pet, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		if (!RoleOperation::get_pet_init_star(player, pet, rds, true))
		{
			GLOBAL_ERROR;
			return;
		}
		xinpain += t_pet->shouhun * t_item->def2;
	}
	rds.add_reward(1, resource::XINPIAN, xinpain / 2);
	rds.merge();

	PlayerOperation::player_dec_resource(player, resource::JEWEL, 200, LOGWAY_PET_FENJIE);
	for (int i = 0; i < msg.guid_size(); ++i)
	{
		pet = POOL_GET_PET(msg.guid(i));
		if (pet)
		{
			RoleOperation::destroy_pet(player, pet, LOGWAY_PET_FENJIE);
		}
	}
	PlayerOperation::player_add_reward(player, rds, LOGWAY_PET_FENJIE);

	protocol::game::smsg_pet_init msg1;
	ADD_MSG_REWARD(msg1, rds);

	ResMessage::res_pet_init(player, msg1, name, id);
}

void RoleManager::terminal_pet_init(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_pet_init msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	if (player->jewel() < 200)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	if (msg.guid_size() <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	dhc::pet_t *pet = 0;
	const s_t_pet *t_pet = 0;
	const s_t_item *t_suipian = 0;
	for (int i = 0; i < msg.guid_size(); ++i)
	{
		pet = POOL_GET_PET(msg.guid(i));
		if (!pet)
		{
			GLOBAL_ERROR;
			return;
		}

		t_pet = sRoleConfig->get_pet(pet->template_id());
		if (!t_pet)
		{
			GLOBAL_ERROR;
			return;
		}

		if (pet->guid() == player->pet_on())
		{
			GLOBAL_ERROR;
			return;
		}

		if (pet->role_guid() != 0)
		{
			GLOBAL_ERROR;
			return;
		}

		t_suipian = sItemConfig->get_item(t_pet->suipian_id);
		if (!t_suipian)
		{
			GLOBAL_ERROR;
			return;
		}

		if (!RoleOperation::get_pet_init_level(player, pet, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		if (!RoleOperation::get_pet_init_jinjie(player, pet, rds))
		{
			GLOBAL_ERROR;
			return;
		}

		if (!RoleOperation::get_pet_init_star(player, pet, rds, false))
		{
			GLOBAL_ERROR;
			return;
		}

		rds.add_reward(2, t_suipian->id, t_suipian->def2);
	}
	rds.merge();

	for (int i = 0; i < msg.guid_size(); ++i)
	{
		pet = POOL_GET_PET(msg.guid(i));
		if (pet)
		{
			RoleOperation::destroy_pet(player, pet, LOGWAY_PET_INIT);
		}
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, 200, LOGWAY_PET_INIT);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_PET_INIT);

	protocol::game::smsg_pet_init msg1;
	ADD_MSG_REWARD(msg1, rds);

	ResMessage::res_pet_init(player, msg1, name, id);
}
