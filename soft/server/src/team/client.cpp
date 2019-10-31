#include "client.h"
#include "rpc.pb.h"

static void _check_player(dhc::player_t* player, const rpcproto::tmsg_player_fight_player* fplayer)
{
	std::set<uint64_t> roles;
	std::set<uint64_t> equips;
	std::set<uint64_t> treasures;
	std::set<uint64_t> pets;
	for (int i = 0; i < fplayer->roles_size(); ++i)
	{
		roles.insert(fplayer->roles(i).guid());
	}
	for (int i = 0; i < fplayer->equips_size(); ++i)
	{
		equips.insert(fplayer->equips(i).guid());
	}
	for (int i = 0; i < fplayer->treasures_size(); ++i)
	{
		treasures.insert(fplayer->treasures(i).guid());
	}
	for (int i = 0; i < fplayer->pets_size(); ++i)
	{
		pets.insert(fplayer->pets(i).guid());
	}

	for (int i = 0; i < player->roles_size(); ++i)
	{
		if (roles.find(player->roles(i)) == roles.end())
		{
			POOL_REMOVE(player->roles(i), 0);
		}
	}

	for (int i = 0; i < player->equips_size(); ++i)
	{
		if (equips.find(player->equips(i)) == equips.end())
		{
			POOL_REMOVE(player->equips(i), 0);
		}
	}

	for (int i = 0; i < player->treasures_size(); ++i)
	{
		if (treasures.find(player->treasures(i)) == treasures.end())
		{
			POOL_REMOVE(player->treasures(i), 0);
		}
	}

	for (int i = 0; i < player->pets_size(); ++i)
	{
		if (pets.find(player->pets(i)) == pets.end())
		{
			POOL_REMOVE(player->pets(i), 0);
		}
	}
}
template<typename T1, typename T2>
static void _copy_role(const T1* frole, T2* role)
{
	role->set_guid(frole->guid());
	role->set_player_guid(frole->player_guid());
	role->set_template_id(frole->template_id());
	role->set_level(frole->level());
	role->set_jlevel(frole->jlevel());
	role->set_glevel(frole->glevel());
	role->set_pinzhi(frole->pinzhi());
	role->set_dress_on_id(frole->dress_on_id());
	role->set_bskill_level(frole->bskill_level());
	role->set_pet(frole->pet());
	role->clear_jskill_level();
	for (int i = 0; i < frole->jskill_level_size(); ++i)
	{
		role->add_jskill_level(frole->jskill_level(i));
	}
	role->clear_dress_ids();
	for (int i = 0; i < frole->dress_ids_size(); ++i)
	{
		role->add_dress_ids(frole->dress_ids(i));
	}
	role->clear_zhuangbeis();
	for (int i = 0; i < frole->zhuangbeis_size(); ++i)
	{
		role->add_zhuangbeis(frole->zhuangbeis(i));
	}
	role->clear_treasures();
	for (int i = 0; i < frole->treasures_size(); ++i)
	{
		role->add_treasures(frole->treasures(i));
	}
}

template<typename T1, typename T2>
static void _copy_equip(const T1* fequip, T2* equip)
{
	equip->set_guid(fequip->guid());
	equip->set_player_guid(fequip->player_guid());
	equip->set_template_id(fequip->template_id());
	equip->set_role_guid(fequip->role_guid());
	equip->set_enhance(fequip->enhance());
	equip->clear_rand_ids();
	equip->clear_rand_values();
	for (int i = 0; i < fequip->rand_ids_size(); ++i)
	{
		equip->add_rand_ids(fequip->rand_ids(i));
		equip->add_rand_values(fequip->rand_values(i));
	}
	equip->set_jilian(fequip->jilian());
	equip->set_star(fequip->star());
}

template<typename T1, typename T2>
static void _copy_treasure(const T1* ftreasure, T2* treasure)
{
	treasure->set_guid(ftreasure->guid());
	treasure->set_player_guid(ftreasure->player_guid());
	treasure->set_template_id(ftreasure->template_id());
	treasure->set_role_guid(ftreasure->role_guid());
	treasure->set_enhance(ftreasure->enhance());
	treasure->set_jilian(ftreasure->jilian());
	treasure->set_star(ftreasure->star());
	treasure->set_star_exp(ftreasure->star_exp());
}

template<typename T1, typename T2>
static void _copy_pet(const T1* fpet, T2* pet)
{
	pet->set_guid(fpet->guid());
	pet->set_player_guid(fpet->player_guid());
	pet->set_template_id(fpet->template_id());
	pet->set_level(fpet->level());
	pet->set_jinjie(fpet->jinjie());
	pet->set_star(fpet->star());
	pet->set_role_guid(fpet->role_guid());
	pet->clear_jinjie_slot();
	for (int i = 0; i < fpet->jinjie_slot_size(); ++i)
	{
		pet->add_jinjie_slot(fpet->jinjie_slot(i));
	}
}

static void _copy_player(const rpcproto::tmsg_player_fight_player* fplayer, dhc::player_t* player)
{
	player->set_guid(fplayer->guid());
	player->set_serverid(fplayer->serverid());
	player->set_name(fplayer->name());
	player->set_template_id(fplayer->template_id());
	player->set_level(fplayer->level());
	player->set_bf(fplayer->bf());
	player->set_zsname(fplayer->guild_name());
	player->set_guild(fplayer->guild());
	player->clear_guild_skill_ids();
	player->clear_guild_skill_levels();
	player->set_pvp_total(fplayer->pvp_total());
	player->set_duixing_id(fplayer->duixing_id());
	player->set_vip(fplayer->vip());
	player->set_duixing_level(fplayer->duixing_level());
	player->set_guanghuan_id(fplayer->guanghuan_id());
	player->set_mission(fplayer->template_dress());
	player->set_chenghao_on(fplayer->chenghao_on());
	player->set_huiyi_shoujidu(fplayer->huiyi_shoujidu());
	player->set_pet_on(fplayer->pet_on());
	player->set_nalflag(fplayer->nalflag());

	player->clear_guild_skill_ids();
	player->clear_guild_skill_levels();
	for (int i = 0; i < fplayer->guild_skill_ids_size(); ++i)
	{
		player->add_guild_skill_ids(fplayer->guild_skill_ids(i));
		player->add_guild_skill_levels(fplayer->guild_skill_levels(i));
	}
	player->clear_dress_ids();
	for (int i = 0; i < fplayer->dress_ids_size(); ++i)
	{
		player->add_dress_ids(fplayer->dress_ids(i));
	}
	player->clear_dress_achieves();
	for (int i = 0; i < fplayer->dress_achieves_size(); ++i)
	{
		player->add_dress_achieves(fplayer->dress_achieves(i));
	}

	player->clear_zhenxing();
	for (int i = 0; i < fplayer->zhenxing_size(); ++i)
	{
		player->add_zhenxing(fplayer->zhenxing(i));
	}

	player->clear_duixing();
	for (int i = 0; i < fplayer->duixing_size(); ++i)
	{
		player->add_duixing(fplayer->duixing(i));
	}

	player->clear_houyuan();
	for (int i = 0; i < fplayer->houyuan_size(); ++i)
	{
		player->add_houyuan(fplayer->houyuan(i));
	}

	player->clear_huiyi_jihuos();
	for (int i = 0; i < fplayer->huiyi_jihuos_size(); ++i)
	{
		player->add_huiyi_jihuos(fplayer->huiyi_jihuos(i));
	}

	player->clear_huiyi_jihuo_starts();
	for (int i = 0; i < fplayer->huiyi_jihuo_starts_size(); ++i)
	{
		player->add_huiyi_jihuo_starts(fplayer->huiyi_jihuo_starts(i));
	}

	player->clear_guanghuan();
	player->clear_guanghuan_level();
	for (int i = 0; i < fplayer->guanghuan_size(); ++i)
	{
		player->add_guanghuan(fplayer->guanghuan(i));
		player->add_guanghuan_level(fplayer->guanghuan_level(i));
	}

	player->clear_chenghao();
	for (int i = 0; i < fplayer->chenghao_size(); ++i)
	{
		player->add_chenghao(fplayer->chenghao(i));
	}

	player->clear_guild_applys();

	player->clear_roles();
	dhc::role_t* role = 0;
	for (int i = 0; i < fplayer->roles_size(); ++i)
	{
		const rpcproto::tmsg_fight_role& frole = fplayer->roles(i);
		player->add_roles(frole.guid());
		role = POOL_GET_ROLE(frole.guid());
		if (!role)
		{
			role = new dhc::role_t();
			POOL_ADD_NEW(frole.guid(), role);
		}
		_copy_role(&frole, role);
	}

	player->clear_equips();
	dhc::equip_t* equip = 0;
	for (int i = 0; i < fplayer->equips_size(); ++i)
	{
		const rpcproto::tmsg_fight_equip& fequip = fplayer->equips(i);
		player->add_equips(fequip.guid());
		equip = POOL_GET_EQUIP(fequip.guid());
		if (!equip)
		{
			equip = new dhc::equip_t();
			POOL_ADD_NEW(fequip.guid(), equip);
		}
		_copy_equip(&fequip, equip);
	}

	player->clear_treasures();
	dhc::treasure_t* treasure = 0;
	for (int i = 0; i < fplayer->treasures_size(); ++i)
	{
		const rpcproto::tmsg_fight_treasure& ftreasure = fplayer->treasures(i);
		player->add_treasures(ftreasure.guid());
		treasure = POOL_GET_TREASURE(ftreasure.guid());
		if (!treasure)
		{
			treasure = new dhc::treasure_t();
			POOL_ADD_NEW(ftreasure.guid(), treasure);
		}
		_copy_treasure(&ftreasure, treasure);
	}

	player->clear_pets();
	dhc::pet_t* pet = 0;
	for (int i = 0; i < fplayer->pets_size(); ++i)
	{
		const rpcproto::tmsg_fight_pet& fpet = fplayer->pets(i);
		player->add_pets(fpet.guid());
		pet = POOL_GET_PET(fpet.guid());
		if (!pet)
		{
			pet = new dhc::pet_t();
			POOL_ADD_NEW(fpet.guid(), pet);
		}
		_copy_pet(&fpet, pet);
	}
}


dhc::player_t* Client::get_player()
{
	return POOL_GET_PLAYER(guid_);
}

const dhc::player_t *Client::get_player() const
{
	return POOL_GET_PLAYER(guid_);
}

bool Client::check(int hid, uint64_t guid, const std::string& sig)
{
	if (hid_ != -1)
		return false;
	if (guid_ != guid)
		return false;
	if (sig_ != sig)
		return false;

	hid_ = hid;
	return true;
}

void Client::send_msg(uint16_t opcode, const google::protobuf::Message* msg /* = 0 */)
{
	Packet* pck = Packet::New(opcode, hid_, guid_, msg);
	if (pck)
	{
		game::tcp_service()->send_msg(hid_, pck);
	}
}

int ClientManager::init()
{
	return 0;
}

int ClientManager::fini()
{
	return 0;
}

Client* ClientManager::get_client(uint64_t guid)
{
	std::map<uint64_t, Client>::iterator it = client_map_.find(guid);
	if (it == client_map_.end())
	{
		return 0;
	}
	return &(it->second);
}

void ClientManager::add_client(uint64_t guid, const rpcproto::tmsg_req_team_login &login)
{
	remove_client(guid);

	dhc::player_t* player = POOL_GET_PLAYER(guid);
	if (player)
	{
		_check_player(player, &(login.players()));
	}
	else
	{
		player = new dhc::player_t();
		POOL_ADD_NEW(guid, player);
	}
	_copy_player(&(login.players()), player);

	player->clear_sports();
	for (int i = 0; i < login.friends_size(); ++i)
	{
		player->add_sports(login.friends(i));
	}
	player->set_by_reward_num(login.by_reward_num());

	player->set_yuanli(login.srank());
	player->set_ds_reward_num(login.ds_reward_num());
	player->set_last_login_time(game::timer()->now());

	client_map_[guid] = Client(guid, login.sig());
}

void ClientManager::add_client(uint64_t guid, const std::string &sig)
{
	remove_client(guid);
	client_map_[guid] = Client(guid, sig);
}

void ClientManager::remove_client(uint64_t guid)
{
	client_map_.erase(guid);
}

void ClientManager::send_msg(uint64_t guid, uint16_t opcode, const google::protobuf::Message* msg /* = 0 */)
{
	Client *cl = get_client(guid);
	if (cl)
	{
		cl->send_msg(opcode, msg);
	}
}

