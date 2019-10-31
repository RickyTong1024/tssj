#include "pvp_operation.h"
#include "rpc.pb.h"
#include "rank_operation.h"
#include "pvp_config.h"
#include "post_operation.h"
#include "player_load.h"

#define PVP_LEVEL 60


static void __copy_pet(
	rpcproto::tmsg_fight_pet* fpet,
	const dhc::pet_t* pet)
{
	fpet->set_guid(pet->guid());
	fpet->set_player_guid(pet->player_guid());
	fpet->set_template_id(pet->template_id());
	fpet->set_level(pet->level());
	fpet->set_jinjie(pet->jinjie());
	fpet->set_star(pet->star());
	fpet->set_role_guid(pet->role_guid());
	for (int i = 0; i < pet->jinjie_slot_size(); ++i)
	{
		fpet->add_jinjie_slot(pet->jinjie_slot(i));
	}
}

static void __copy_equip(
	rpcproto::tmsg_fight_equip* fequip,
	const dhc::equip_t* equip)
{
	fequip->set_guid(equip->guid());
	fequip->set_player_guid(equip->player_guid());
	fequip->set_template_id(equip->template_id());
	fequip->set_role_guid(equip->role_guid());
	fequip->set_enhance(equip->enhance());
	fequip->set_jilian(equip->jilian());
	fequip->set_star(equip->star());
	for (int m = 0; m < equip->rand_ids_size(); ++m)
	{
		fequip->add_rand_ids(equip->rand_ids(m));
		fequip->add_rand_values(equip->rand_values(m));
	}
}

static void __copy_treasures(
	rpcproto::tmsg_fight_treasure* ftreasure,
	const dhc::treasure_t* treasure)
{
	ftreasure->set_guid(treasure->guid());
	ftreasure->set_player_guid(treasure->player_guid());
	ftreasure->set_template_id(treasure->template_id());
	ftreasure->set_role_guid(treasure->role_guid());
	ftreasure->set_enhance(treasure->enhance());
	ftreasure->set_jilian(treasure->jilian());
	ftreasure->set_star(treasure->star());
	ftreasure->set_star_exp(treasure->star_exp());
}

static void __copy_roles(
	rpcproto::tmsg_player_fight_player* fplayer,
	rpcproto::tmsg_fight_role* frole,
	const dhc::role_t* role)
{
	frole->set_guid(role->guid());
	frole->set_player_guid(role->player_guid());
	frole->set_template_id(role->template_id());
	frole->set_level(role->level());
	frole->set_jlevel(role->jlevel());
	frole->set_glevel(role->glevel());
	frole->set_pinzhi(role->pinzhi());
	frole->set_dress_on_id(role->dress_on_id());
	frole->set_bskill_level(role->bskill_level());
	frole->set_pet(role->pet());
	for (int m = 0; m < role->jskill_level_size(); ++m)
	{
		frole->add_jskill_level(role->jskill_level(m));
	}
	for (int m = 0; m < role->dress_ids_size(); ++m)
	{
		frole->add_dress_ids(role->dress_ids(m));
	}

	const dhc::equip_t* equip = 0;
	rpcproto::tmsg_fight_equip* fequip = 0;
	for (int m = 0; m < role->zhuangbeis_size(); ++m)
	{
		frole->add_zhuangbeis(role->zhuangbeis(m));
		if (role->zhuangbeis(m) > 0)
		{
			equip = POOL_GET_EQUIP(role->zhuangbeis(m));
			if (equip)
			{
				fequip = fplayer->add_equips();
				if (fequip)
				{
					__copy_equip(fequip, equip);
				}
			}
		}
	}

	const dhc::treasure_t* treasure = 0;
	rpcproto::tmsg_fight_treasure* ftreasure = 0;
	for (int m = 0; m < role->treasures_size(); ++m)
	{
		frole->add_treasures(role->treasures(m));
		if (role->treasures(m) > 0)
		{
			treasure = POOL_GET_TREASURE(role->treasures(m));
			if (treasure)
			{
				ftreasure = fplayer->add_treasures();
				if (ftreasure)
				{
					__copy_treasures(ftreasure, treasure);
				}
			}
		}
	}

	if (role->pet() != 0)
	{
		const dhc::pet_t *pet = POOL_GET_PET(role->pet());
		if (pet)
		{
			rpcproto::tmsg_fight_pet *fpet = 0;
			fpet = fplayer->add_pets();
			if (fpet)
			{
				__copy_pet(fpet, pet);
			}
		}
	}
}

static void __copy_player(
	rpcproto::tmsg_player_fight_player* fplayer,
	const dhc::player_t* player,
	PvpType type)
{
	fplayer->set_guid(player->guid());
	fplayer->set_serverid(player->serverid());
	fplayer->set_name(player->name());
	fplayer->set_template_id(player->template_id());
	fplayer->set_level(player->level());
	if (type == PT_LIEREN)
		fplayer->set_bf(player->bf());
	else
		fplayer->set_bf(player->bf());
	fplayer->set_guild(player->guild());
	fplayer->set_pvp_total(player->pvp_total());
	fplayer->set_duixing_id(player->duixing_id());
	fplayer->set_vip(player->vip());
	fplayer->set_duixing_level(player->duixing_level());
	fplayer->set_guanghuan_id(player->guanghuan_id());
	fplayer->set_chenghao_on(player->chenghao_on());
	fplayer->set_huiyi_shoujidu(player->huiyi_shoujidu());
	fplayer->set_nalflag(player->nalflag());
	if (player->guild() > 0)
	{
		dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
		if (guild)
		{
			fplayer->set_guild_name(guild->name());
		}
	}
	dhc::role_t *template_role = 0;
	for (int i = 0; i < player->roles_size(); ++i)
	{
		template_role = POOL_GET_ROLE(player->roles(i));
		if (template_role && template_role->template_id() == player->template_id())
		{
			fplayer->set_template_dress(template_role->dress_on_id());
			break;
		}
	}
	for (int i = 0; i < player->guild_skill_ids_size(); ++i)
	{
		fplayer->add_guild_skill_ids(player->guild_skill_ids(i));
		fplayer->add_guild_skill_levels(player->guild_skill_levels(i));
	}
	for (int i = 0; i < player->dress_ids_size(); ++i)
	{
		fplayer->add_dress_ids(player->dress_ids(i));
	}
	for (int i = 0; i < player->dress_achieves_size(); ++i)
	{
		fplayer->add_dress_achieves(player->dress_achieves(i));
	}
	for (int i = 0; i < player->duixing_size(); ++i)
	{
		fplayer->add_duixing(player->duixing(i));
	}
	for (int i = 0; i < player->huiyi_jihuos_size(); ++i)
	{
		fplayer->add_huiyi_jihuos(player->huiyi_jihuos(i));
	}
	for (int i = 0; i < player->huiyi_jihuo_starts_size(); ++i)
	{
		fplayer->add_huiyi_jihuo_starts(player->huiyi_jihuo_starts(i));
	}
	for (int i = 0; i < player->guanghuan_size(); ++i)
	{
		fplayer->add_guanghuan(player->guanghuan(i));
		fplayer->add_guanghuan_level(player->guanghuan_level(i));
	}
	for (int i = 0; i < player->chenghao_size(); ++i)
	{
		fplayer->add_chenghao(player->chenghao(i));
	}
	
	const dhc::role_t *role = 0;
	rpcproto::tmsg_fight_role* frole = 0;
	for (int i = 0; i < player->zhenxing_size(); ++i)
	{
		fplayer->add_zhenxing(player->zhenxing(i));
		if (player->zhenxing(i) > 0)
		{
			role = POOL_GET_ROLE(player->zhenxing(i));
			if (role)
			{
				frole = fplayer->add_roles();
				if (frole)
				{
					__copy_roles(fplayer, frole, role);
				}
			}
		}
	}

	for (int i = 0; i < player->houyuan_size(); ++i)
	{
		fplayer->add_houyuan(player->houyuan(i));
		if (player->houyuan(i) > 0)
		{
			role = POOL_GET_ROLE(player->houyuan(i));
			if (role)
			{
				frole = fplayer->add_roles();
				if (frole)
				{
					__copy_roles(fplayer, frole, role);
				}
			}
		}
	}

	fplayer->set_pet_on(player->pet_on());
	if (fplayer->pet_on() > 0)
	{
		const dhc::pet_t *pet = POOL_GET_PET(fplayer->pet_on());
		if (pet)
		{
			rpcproto::tmsg_fight_pet *fpet = fplayer->add_pets();
			if (fpet)
			{
				__copy_pet(fpet, pet);
			}
		}
	}
}

void PvpOperation::sync(dhc::player_t* player)
{
	if (sPvpList->should_sync(player))
	{
		sync_player(player);
	}
}

void PvpOperation::copy(const dhc::player_t*player, rpcproto::tmsg_player_fight_player* fplayer, PvpType type)
{
	__copy_player(fplayer, player, type);
}

void PvpOperation::sync_player(dhc::player_t* player)
{
	rpcproto::tmsg_player_fight_info infos;
	rpcproto::tmsg_player_fight_player* fplayer = infos.mutable_players();
	if (fplayer)
	{
		__copy_player(fplayer, player, PT_LIEREN);
		std::string s;
		infos.SerializeToString(&s);
		game::rpc_service()->push("remote1", PMSG_PVP_PUSH, s);
	}
	return;
}


int PvpList::init()
{
	load_pvp_list();
	return 0;
}

int PvpList::fini()
{
	dhc::pvp_list_t *pvp_list = 0;
	for (int i = PVP_LIST_0; i <= PVP_LIST_2; ++i)
	{
	   pvp_list	= POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, i));
	   if (pvp_list)
	   {
		   POOL_SAVE(dhc::pvp_list_t, pvp_list, true);
	   }
	}
	for (int i = PVP_LIST_REWARD; i <= PVP_LIST_RANK; ++i)
	{
		pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, i));
		if (pvp_list)
		{
			POOL_SAVE(dhc::pvp_list_t, pvp_list, true);
		}
	}
	for (int i = PVP_BINGYUAN_REWARD; i < PVP_LIST_END; ++i)
	{
		pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, i));
		if (pvp_list)
		{
			POOL_SAVE(dhc::pvp_list_t, pvp_list, true);
		}
	}
	for (int i = PVP_GUILD_REWARD; i < PVP_GUILD_LIST_END; ++i)
	{
		pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, i));
		if (pvp_list)
		{
			POOL_SAVE(dhc::pvp_list_t, pvp_list, true);
		}
	}
	return 0;
}

void PvpList::update()
{
	dhc::pvp_list_t *pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_LIST_0));
	if (pvp_list)
	{
		if (game::timer()->trigger_time(pvp_list->refresh_time(), 0, 0))
		{
			uint64_t now_time = game::timer()->now();
			pvp_list->set_refresh_time(now_time);
			POOL_SAVE(dhc::pvp_list_t, pvp_list, false);

			if (game::timer()->weekday() == 5)
			{
				dhc::pvp_list_t *refresh_list = 0;
				for (int i = PVP_LIST_0;
					i <= PVP_LIST_2;
					++i)
				{
					refresh_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, i));
					if (refresh_list)
					{
						refresh_list->clear_player_guids();
						refresh_list->clear_player_targets();
						refresh_list->clear_player_names();
						refresh_list->clear_player_templates();
						refresh_list->clear_player_servers();
						refresh_list->clear_player_bfs();
						refresh_list->clear_player_vips();
						refresh_list->clear_player_wins();
						refresh_list->clear_player_pvps();
						refresh_list->clear_player_achieves();
						refresh_list->clear_player_guanghuans();
						refresh_list->clear_player_dress();
						refresh_list->clear_player_chenghaos();
						refresh_list->clear_player_nalflags();

						POOL_SAVE(dhc::pvp_list_t, refresh_list, false);
					}
				}

				dhc::pvp_list_t *reward_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_LIST_REWARD));
				if (reward_list)
				{
					reward_list->set_sync_flag(1);
					reward_list->set_refresh_time(now_time);
					reward_list->clear_player_guids();
					for (std::map<uint64_t, protocol::game::smsg_pvp_view>::const_iterator it = player_views_.begin();
						it != player_views_.end(); ++it)
					{
						reward_list->add_player_guids(it->first);
					}
					POOL_SAVE(dhc::pvp_list_t, reward_list, false);
				}
				player_views_.clear();

				dhc::pvp_list_t *rank_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_LIST_RANK));
				if (rank_list)
				{
					rank_list->clear_player_guids();
					rank_list->clear_player_targets();
					rank_list->clear_player_names();
					rank_list->clear_player_templates();
					rank_list->clear_player_servers();
					rank_list->clear_player_bfs();
					rank_list->clear_player_vips();
					rank_list->clear_player_wins();
					rank_list->clear_player_pvps();
					rank_list->clear_player_achieves();
					rank_list->clear_player_guanghuans();
					rank_list->clear_player_dress();
					rank_list->clear_player_chenghaos();
					rank_list->clear_player_nalflags();

					POOL_SAVE(dhc::pvp_list_t, rank_list, false);
				}
			}
		}
	}

	dhc::pvp_list_t *bingyuan_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_BINGYUAN_REWARD));
	if (bingyuan_list)
	{
		if (game::timer()->trigger_week_time(bingyuan_list->refresh_time()))
		{
			bingyuan_list->set_sync_flag(1);
			bingyuan_list->set_refresh_time(game::timer()->now());
			bingyuan_list->clear_player_guids();
			
			dhc::pvp_list_t *bingyuan_rank_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_BINGYUAN_LIST));
			if (bingyuan_rank_list)
			{
				for (int i = 0; i < bingyuan_rank_list->player_guids_size(); ++i)
				{
					bingyuan_list->add_player_guids(bingyuan_rank_list->player_guids(i));
				}
				bingyuan_rank_list->clear_player_guids();
				POOL_SAVE(dhc::pvp_list_t, bingyuan_rank_list, false);
			}

			POOL_SAVE(dhc::pvp_list_t, bingyuan_list, false);

			ds_reward();
		}
	}

	dhc::pvp_list_t *guild_rank_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_GUILD_REWARD));
	if (guild_rank_list)
	{
		if (game::timer()->trigger_week_time(guild_rank_list->refresh_time()))
		{
			dhc::global_t *glob = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
			if (glob && glob->guild_pvp_zhou() % 2 == 0)
			{
				guild_rank_list->set_sync_flag(1);
			}
			else
			{
				guild_rank_list->set_sync_flag(3);
			}
			guild_rank_list->set_refresh_time(game::timer()->now());
			guild_rank_list->clear_player_guids();
			std::vector<uint64_t> guids;
			std::vector<dhc::guild_t*> guilds;
			dhc::guild_t *guild;
			game::pool()->get_entitys(et_guild, guids);
			for (int i = 0; i < guids.size(); ++i)
			{
				guild = POOL_GET_GUILD(guids[i]);
				if (guild && guild->juntuan_apply())
				{
					guild->set_juntuan_apply(0);
					dhc::guild_member_t *guild_member = 0;
					for (int kmg = 0; kmg < guild->member_guids_size(); ++kmg)
					{
						guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(kmg));
						if (guild_member)
						{
							guild_rank_list->add_player_guids(guild_member->player_guid());
						}
					}
				}
			}
			POOL_SAVE(dhc::pvp_list_t, guild_rank_list, false);
		}
		
	}
}

void PvpList::load_pvp_list()
{
	for (int i = PVP_LIST_0; i <= PVP_LIST_2; ++i)
	{
		uint64_t pvp_list_guid = MAKE_GUID(et_lottery_list, i);
		Request *req = new Request();
		req->add(opc_query, pvp_list_guid, new dhc::pvp_list_t);
		game::pool()->upcall(req, boost::bind(&PvpList::load_pvp_list_callback, this, _1, pvp_list_guid));
	}

	for (int i = PVP_LIST_REWARD; i <= PVP_LIST_RANK; ++i)
	{
		uint64_t pvp_list_guid = MAKE_GUID(et_lottery_list, i);
		Request *req = new Request();
		req->add(opc_query, pvp_list_guid, new dhc::pvp_list_t);
		game::pool()->upcall(req, boost::bind(&PvpList::load_pvp_list_callback, this, _1, pvp_list_guid));
	}

	for (int i = PVP_BINGYUAN_REWARD; i < PVP_LIST_END; ++i)
	{
		uint64_t pvp_list_guid = MAKE_GUID(et_lottery_list, i);
		Request *req = new Request();
		req->add(opc_query, pvp_list_guid, new dhc::pvp_list_t);
		game::pool()->upcall(req, boost::bind(&PvpList::load_pvp_list_callback, this, _1, pvp_list_guid));
	}

	for (int i = PVP_GUILD_REWARD; i < PVP_GUILD_LIST_END; ++i)
	{
		uint64_t pvp_list_guid = MAKE_GUID(et_lottery_list, i);
		Request *req = new Request();
		req->add(opc_query, pvp_list_guid, new dhc::pvp_list_t);
		game::pool()->upcall(req, boost::bind(&PvpList::load_pvp_list_callback, this, _1, pvp_list_guid));
	}
}

void PvpList::load_pvp_list_callback(Request *req, uint64_t pvp_guid)
{
	if (req->success())
	{
		dhc::pvp_list_t* pvp_list = (dhc::pvp_list_t*)req->release_data();
		POOL_ADD(pvp_list->guid(), pvp_list);

		int list_index = GUID_COUNTER(pvp_guid);
		if (list_index >= PVP_LIST_0 && list_index <= PVP_LIST_2)
		{
			for (int i = 0; i < pvp_list->player_guids_size(); ++i)
			{
				add_pvp_view(pvp_list->player_guids(i), pvp_list->player_targets(i), pvp_list->player_names(i),
					pvp_list->player_templates(i), pvp_list->player_servers(i), pvp_list->player_bfs(i),
					pvp_list->player_pvps(i), pvp_list->player_wins(i), pvp_list->player_vips(i), pvp_list->player_achieves(i),
					pvp_list->player_guanghuans(i), pvp_list->player_dress(i), pvp_list->player_chenghaos(i),pvp_list->player_nalflags(i), false);
			}
		}
		if (list_index == PVP_LIST_RANK)
		{
			for (int i = 0; i < pvp_list->player_guids_size(); ++i)
			{
				add_pvp_view(pvp_list->player_guids(i), pvp_list->player_targets(i), pvp_list->player_names(i),
					pvp_list->player_templates(i), pvp_list->player_servers(i), pvp_list->player_bfs(i),
					pvp_list->player_pvps(i), pvp_list->player_wins(i), pvp_list->player_vips(i),
					pvp_list->player_achieves(i),
					pvp_list->player_guanghuans(i), pvp_list->player_dress(i),
					pvp_list->player_chenghaos(i),pvp_list->player_nalflags(i), true);
			}
		}
		if (list_index == PVP_GUILD_REWARD)
		{
			pvp_list->set_sync_flag(3);
		}
	}
	else
	{
		dhc::pvp_list_t* pvp_list = new dhc::pvp_list_t();
		pvp_list->set_guid(pvp_guid);
		pvp_list->set_refresh_time(game::timer()->now());
		pvp_list->set_sync_flag(0);
		if (GUID_COUNTER(pvp_guid) == PVP_GUILD_REWARD)
		{
			pvp_list->set_sync_flag(3);
		}
		POOL_ADD_NEW(pvp_guid, pvp_list);
		POOL_SAVE(dhc::pvp_list_t, pvp_list, false);
	}
}

protocol::game::smsg_pvp_view* PvpList::get_pvp_list(uint64_t player_guid)
{
	std::map<uint64_t, protocol::game::smsg_pvp_view>::iterator it = player_views_.find(player_guid);
	if (it == player_views_.end())
	{
		return 0;
	}
	return &(it->second);
}

void PvpList::refresh_pvp_list(uint64_t player_guid, const protocol::game::smsg_pvp_view& view)
{
	protocol::game::smsg_pvp_view* pvp_view = get_pvp_list(player_guid);
	if (pvp_view == 0)
	{
		player_views_[player_guid] = view;
	}
	else
	{
		pvp_view->CopyFrom(view);
	}

	changed_pvp_list(player_guid, &view);
}

void PvpList::refresh_empty_view(const protocol::game::smsg_pvp_view& view)
{
	empty_view_.CopyFrom(view);

	changed_empty_list(&view);
}

void PvpList::update_pvp_list(uint64_t player_guid, PvpListNum index)
{
	protocol::game::smsg_pvp_view* pvp_view = get_pvp_list(player_guid);
	if (pvp_view == 0)
	{
		return;
	}
	if (index >= pvp_view->player_guids_size())
	{
		return;
	}
	pvp_view->set_player_wins(index, 1);

	changed_pvp_list(player_guid, pvp_view);
}

bool PvpList::should_sync(dhc::player_t* player)
{
	if (player->level() < PVP_LEVEL)
	{
		return false;
	}
	std::map<uint64_t, SyncPolicy>::iterator it = player_sync_policy_.find(player->guid());
	if (it == player_sync_policy_.end())
	{
		/// 首次同步
		SyncPolicy policy;
		policy.enforce = false;
		policy.last_bf = player->bf_max();
		policy.count = 1;
		policy.total = 0;
		policy.pvp_point = player->pvp_total();
		player_sync_policy_[player->guid()] = policy;
		return true;
	}
	/// 战力同步
	if (it->second.last_bf != player->bf_max())
	{
		it->second.last_bf = player->bf_max();
		it->second.total = 0;
		return true;
	}
	/// 强制同步
	if (it->second.enforce)
	{
		it->second.enforce = false;
		it->second.total = 0;
		return true;
	}
	/// 分数同步
	if (it->second.pvp_point != player->pvp_total())
	{
		it->second.pvp_point = player->pvp_total();
		it->second.total = 0;
		return true;
	}

	/// 已经不再变化，无需同步
	if (it->second.total >= 10)
	{
		return false;
	}
	/// 计数同步
	if ((it->second.count % 11) == 0)
	{
		it->second.count = 1;
		it->second.total++;
		return true;
	}
	it->second.count++;
	return false;
}

void PvpList::set_sync(dhc::player_t* player)
{
	if (player->level() < PVP_LEVEL)
	{
		return;
	}
	std::map<uint64_t, SyncPolicy>::iterator it = player_sync_policy_.find(player->guid());
	if (it != player_sync_policy_.end())
	{
		it->second.enforce = true;
	}
}

void PvpList::remove_sync(dhc::player_t* player)
{
	if (player->level() < PVP_LEVEL)
	{
		return;
	}
	player_sync_policy_.erase(player->guid());
}

void PvpList::changed_pvp_list(uint64_t player_guid, const protocol::game::smsg_pvp_view* view)
{
	dhc::pvp_list_t *pvp_list = 0;
	bool has = false;
	for (int i = 0; i < view->player_guids_size(); ++i)
	{
		pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, i));
		if (pvp_list)
		{
			has = false;
			for (int j = 0; j < pvp_list->player_guids_size(); ++j)
			{
				if (pvp_list->player_guids(j) == player_guid)
				{
					pvp_list->set_player_guids(j, player_guid);
					pvp_list->set_player_targets(j, view->player_guids(i));
					pvp_list->set_player_names(j, view->player_names(i));
					pvp_list->set_player_templates(j, view->player_templates(i));
					pvp_list->set_player_servers(j, view->player_servers(i));
					pvp_list->set_player_bfs(j, view->player_bfs(i));
					pvp_list->set_player_pvps(j, view->player_points(i));
					pvp_list->set_player_wins(j, view->player_wins(i));
					pvp_list->set_player_vips(j, view->player_vips(i));
					pvp_list->set_player_achieves(j, view->player_achieves(i));
					pvp_list->set_player_guanghuans(j, view->player_guanghuans(i));
					pvp_list->set_player_dress(j, view->player_dress(i));
					pvp_list->set_player_chenghaos(j, view->player_chenghaos(i));
					pvp_list->set_player_nalflags(j, view->player_nalflags(i));
					has = true;
					break;
				}
			}
			if (!has)
			{
				pvp_list->add_player_guids(player_guid);
				pvp_list->add_player_targets(view->player_guids(i));
				pvp_list->add_player_names(view->player_names(i));
				pvp_list->add_player_templates(view->player_templates(i));
				pvp_list->add_player_servers(view->player_servers(i));
				pvp_list->add_player_bfs(view->player_bfs(i));
				pvp_list->add_player_pvps(view->player_points(i));
				pvp_list->add_player_wins(view->player_wins(i));
				pvp_list->add_player_vips(view->player_vips(i));
				pvp_list->add_player_achieves(view->player_achieves(i));
				pvp_list->add_player_guanghuans(view->player_guanghuans(i));
				pvp_list->add_player_dress(view->player_dress(i));
				pvp_list->add_player_chenghaos(view->player_chenghaos(i));
				pvp_list->add_player_nalflags(view->player_nalflags(i));
			}

			POOL_SAVE(dhc::pvp_list_t, pvp_list, false);
		}
	}
}

void PvpList::changed_empty_list(const protocol::game::smsg_pvp_view* view)
{
	dhc::pvp_list_t *pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_LIST_RANK));
	if (!pvp_list)
	{
		return;
	}

	for (int i = 0; i < view->player_guids_size(); ++i)
	{
		pvp_list->add_player_guids(view->player_guids(i));
		pvp_list->add_player_targets(view->player_guids(i));
		pvp_list->add_player_names(view->player_names(i));
		pvp_list->add_player_templates(view->player_templates(i));
		pvp_list->add_player_servers(view->player_servers(i));
		pvp_list->add_player_bfs(view->player_bfs(i));
		pvp_list->add_player_pvps(view->player_points(i));
		pvp_list->add_player_wins(view->player_wins(i));
		pvp_list->add_player_vips(view->player_vips(i));
		pvp_list->add_player_achieves(view->player_achieves(i));
		pvp_list->add_player_guanghuans(view->player_guanghuans(i));
		pvp_list->add_player_dress(view->player_dress(i));
		pvp_list->add_player_chenghaos(view->player_chenghaos(i));
		pvp_list->add_player_nalflags(view->player_nalflags(i));
	}
	POOL_SAVE(dhc::pvp_list_t, pvp_list, false);
}

void PvpList::add_pvp_view(uint64_t player_guid, uint64_t target_guid, const std::string& name, int id, int server, int bf, int point,
	int win, int vip, int achieve, int guanghuan, int dress, int chenghao,int nalflag, bool empty)
{
	if (empty)
	{
		empty_view_.add_player_guids(target_guid);
		empty_view_.add_player_names(name);
		empty_view_.add_player_templates(id);
		empty_view_.add_player_servers(server);
		empty_view_.add_player_bfs(bf);
		empty_view_.add_player_points(point);
		empty_view_.add_player_wins(win);
		empty_view_.add_player_vips(vip);
		empty_view_.add_player_achieves(achieve);
		empty_view_.add_player_guanghuans(guanghuan);
		empty_view_.add_player_dress(dress);
		empty_view_.add_player_chenghaos(chenghao);
		empty_view_.add_player_nalflags(nalflag);
	}
	else
	{
		std::map<uint64_t, protocol::game::smsg_pvp_view>::iterator it = player_views_.find(player_guid);
		if (it == player_views_.end())
		{
			protocol::game::smsg_pvp_view view;
			view.add_player_guids(target_guid);
			view.add_player_names(name);
			view.add_player_templates(id);
			view.add_player_servers(server);
			view.add_player_bfs(bf);
			view.add_player_points(point);
			view.add_player_wins(win);
			view.add_player_vips(vip);
			view.add_player_achieves(achieve);
			view.add_player_guanghuans(guanghuan);
			view.add_player_dress(dress);
			view.add_player_chenghaos(chenghao);
			view.add_player_nalflags(nalflag);
			player_views_[player_guid] = view;
		}
		else
		{
			it->second.add_player_guids(target_guid);
			it->second.add_player_names(name);
			it->second.add_player_templates(id);
			it->second.add_player_servers(server);
			it->second.add_player_bfs(bf);
			it->second.add_player_points(point);
			it->second.add_player_wins(win);
			it->second.add_player_vips(vip);
			it->second.add_player_achieves(achieve);
			it->second.add_player_guanghuans(guanghuan);
			it->second.add_player_dress(dress);
			it->second.add_player_chenghaos(chenghao);
			it->second.add_player_nalflags(nalflag);
		}
	}
	
}


void PvpList::add_social_invite(uint64_t player_guid)
{
	team_social_invite_.insert(player_guid);
}

int PvpList::get_social_invite(uint64_t player_guid)
{
	if (team_social_invite_.find(player_guid) != team_social_invite_.end())
	{
		team_social_invite_.erase(player_guid);
		return 1;
	}
	return 0;
}

void PvpList::update_bingyuan_list(uint64_t player_guid)
{
	dhc::pvp_list_t *pvp_list = POOL_GET_LOTTERY_LIST(MAKE_GUID(et_lottery_list, PVP_BINGYUAN_LIST));
	if (!pvp_list)
	{
		return;
	}

	for (int i = 0; i < pvp_list->player_guids_size(); ++i)
	{
		if (pvp_list->player_guids(i) == player_guid)
		{
			return;
		}
	}

	pvp_list->add_player_guids(player_guid);
	POOL_SAVE(dhc::pvp_list_t, pvp_list, false);
}

bool PvpList::should_team_pull(dhc::player_t *player)
{
	if (player->level() <= 70)
	{
		return false;
	}

	if (player->by_point() < 500)
	{
		return false;
	}

	std::map<uint64_t, TeamPullPolicy>::iterator it = player_team_pull_.find(player->guid());
	if (it == player_team_pull_.end())
	{
		TeamPullPolicy tpp;
		tpp.next_time = game::timer()->now() + 60 * 60 * 1000;
		player_team_pull_[player->guid()] = tpp;
		return true;
	}

	if (game::timer()->now() > it->second.next_time)
	{
		it->second.next_time = game::timer()->now() + 60 * 60 * 1000;
		return true;
	}
	return false;
}

void PvpList::remove_team_pull(dhc::player_t *player)
{
	player_team_pull_.erase(player->guid());
}

void PvpList::ds_reward()
{
	dhc::rank_t *rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_ds));
	if (rank)
	{
		const s_t_ds_reward *t_ds_reward = 0;
		for (int i = 0; i < rank->player_guid_size(); ++i)
		{
			if (i >= 50)
			{
				break;
			}

			if (rank->value(i) <= 0)
			{
				continue;
			}

			t_ds_reward = sPvpConfig->get_ds_reward(i + 1);
			if (t_ds_reward)
			{
				std::string sender;
				std::string title;
				std::string text;
				int lang_ver = game::channel()->get_channel_lang(rank->player_guid(i));
				game::scheme()->get_server_str(lang_ver, text, "ds_normal_reward_text", i + 1);
				game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
				game::scheme()->get_server_str(lang_ver, title, "ds_normal_reward_title");
				PostOperation::post_create(rank->player_guid(i), title, text, sender, t_ds_reward->rds);
			}
		}
		RankOperation::clear_rank(e_rank_ds);
	}
}