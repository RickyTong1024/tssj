#include "treasure_list.h"
#include "treasure_config.h"
#include "sport_config.h"
#include "role_config.h"
#include "utils.h"
#include "item_operation.h"
#include "role_operation.h"

struct __treasure_list_id
{
	dhc::treasure_list_t *treasure_list;
	int player_index;

	__treasure_list_id()
		:treasure_list(0),
		player_index(-1)
	{
	}
};


struct __rob_pair
{
	int index;
	int level;
	int bf;

	bool operator > (const __rob_pair &rhs) const
	{
		if (this->level == rhs.level)
			return this->bf > rhs.bf;
		else
			return this->level > rhs.level;
	}
};


void RobPlayerList::get_rob_player_list(protocol::game::smsg_treasure_rob_view &view_list)
{
	for (std::vector<RobPlayer>::size_type i = 0;
		i < rob_player_list_.size();
		++i)
	{
		const RobPlayer &rp = rob_player_list_[i];
		view_list.add_player_guids(rp.guid_);
		view_list.add_player_names(rp.name_);
		view_list.add_player_templates(rp.template_id_);
		view_list.add_player_levels(rp.level_);
		view_list.add_player_bfs(rp.bf_);
		view_list.add_player_rates(rp.rate_);
		view_list.add_player_npcs(rp.is_npc_);
		view_list.add_player_vips(rp.vip_);
		view_list.add_player_achieves(rp.achieve_);
		view_list.add_player_chenghaos(rp.chenghao_);
		view_list.add_player_nalflags(rp.nalflag);
	}
}

RobPlayer* RobPlayerList::get_rob_player(uint64_t guid)
{
	for (std::vector<RobPlayer>::size_type i = 0;
		i < rob_player_list_.size();
		++i)
	{
		if (rob_player_list_[i].guid_ == guid)
		{
			return &(rob_player_list_[i]);
		}
	}
	return 0;
}


void RobPlayerList::modify_first_suipian(int level, int bf)
{
	if (rob_player_list_.empty())
	{
		return;
	}
	std::vector<uint32_t> roles;
	rob_player_list_[0].guid_ = 1;
	rob_player_list_[0].name_ = sSportConfig->get_random_name();
	rob_player_list_[0].template_id_ = sRoleConfig->get_random_role_id(Utils::get_int32(3, 4), roles);
	rob_player_list_[0].level_ = Utils::get_int32(level - 5, level + 5);
	rob_player_list_[0].bf_ = Utils::get_int32(bf * 0.7f, bf);
	rob_player_list_[0].rate_ = 100;
	rob_player_list_[0].is_npc_ = true;
	rob_player_list_[0].vip_ = 0;
	rob_player_list_[0].achieve_ = 0;
	rob_player_list_[0].chenghao_ = 0;
	rob_player_list_[0].nalflag = 0;
}

int TreasureList::init()
{
	load_treasure_list();
	new_npc();
	return 0;
}

void TreasureList::new_npc()
{
	npc_ = new dhc::player_t();
	fake_npc_ = new dhc::player_t();
	for (int i = 0; i < 6; ++i)
	{
		dhc::role_t *role = RoleOperation::role_create_normal();
		
		npc_->add_roles(role->guid());
		POOL_ADD_NEW(role->guid(), role);
		npc_->add_zhenxing(role->guid());
		npc_->add_duixing(i);
	}
}

int TreasureList::fini()
{
	const std::map<int, s_t_treasure_suipian> &all_suipian = sTreasureConfig->get_all_suipian();
	for (std::map<int, s_t_treasure_suipian>::const_iterator it = all_suipian.begin();
		it != all_suipian.end();
		++it)
	{
		save_treasure_list(it->second.ordernum, true);
	}

	if (npc_)
	{
		delete npc_;
	}
	if (fake_npc_)
	{
		delete fake_npc_;
	}

	return 0;
}

int TreasureList::update(const ACE_Time_Value& tv)
{
	const std::map<int, s_t_treasure_suipian> &all_suipian = sTreasureConfig->get_all_suipian();
	for (std::map<int, s_t_treasure_suipian>::const_iterator it = all_suipian.begin();
		it != all_suipian.end();
		++it)
	{
		save_treasure_list(it->second.ordernum, false);
	}

	return 0;
}

void TreasureList::get_rob_player_list(dhc::player_t *player, 
	int suipian_id,
	protocol::game::smsg_treasure_rob_view &view_list)
{
	const s_t_treasure_suipian *t_suipian = sTreasureConfig->get_suipian(suipian_id);
	if (!t_suipian)
	{
		return;
	}

	dhc::treasure_list_t *treasure_list = POOL_GET_TREASURE_LIST(MAKE_GUID(et_treasure_list, t_suipian->ordernum));
	if (!treasure_list)
	{
		return;
	}
	if (treasure_list->template_id() != suipian_id)
	{
		return;
	}

	RobPlayerList *rob_player_list = new_rob_player_list(player, suipian_id);
	if (!rob_player_list)
	{
		return;
	}

	/// 筛选玩家
	__rob_pair pair;
	std::vector<__rob_pair> pairs;

	int hour = game::timer()->hour();
	if (hour >= 1 && hour <= 9)
	{
	}
	else
	{
		int min_level = player->level() - 5;
		int max_level = player->level() + 5;
		uint64_t now_time = game::timer()->now();
		for (int i = 0; i < treasure_list->player_guid_size(); ++i)
		{
			if (treasure_list->player_guid(i) != player->guid() &&
				treasure_list->player_num(i) > 0 &&
				treasure_list->player_total(i) > 1 &&
				treasure_list->player_level(i) >= min_level &&
				treasure_list->player_level(i) <= max_level &&
				treasure_list->player_time(i) <= now_time &&
				treasure_list->player_first(i) > 0)
			{
				pair.index = i;
				pair.level = treasure_list->player_level(i);
				pair.bf = treasure_list->player_bt(i);
				pairs.push_back(pair);
			}

		}
		std::sort(pairs.begin(), pairs.end(), std::greater<__rob_pair>());
	}
	
	/// 取6到12位玩家
	int	max_size = pairs.size();
	int player_size = 0;
	int npc_size = 0;
	if (max_size <= 0)
	{
		npc_size = 6;
	}
	else
	{
		int base_size = 0;
		if ((max_size % 3) == 0)
		{
			base_size = (max_size / 3) * 3;
		}
		else
		{
			base_size = (max_size / 3 + 1) * 3;
		}
		if (base_size < 6)
		{
			base_size = 6;
		}
		if (base_size > 12)
		{
			base_size = 12;
		}
		npc_size = base_size - max_size;
		player_size = (npc_size > 0) ? base_size - npc_size : base_size;
	}

	/// 选取玩家
	for (int i = 0; i < player_size; ++i)
	{
		if (i < pairs.size())
		{
			const __rob_pair &rp_pari = pairs[i];

			RobPlayer rp(
				treasure_list->player_guid(rp_pari.index),
				treasure_list->player_name(rp_pari.index),
				treasure_list->player_template(rp_pari.index),
				treasure_list->player_level(rp_pari.index),
				treasure_list->player_bt(rp_pari.index),
				t_suipian->player_rate,
				false,
				treasure_list->player_vip(rp_pari.index),
				treasure_list->player_achieve(rp_pari.index),
				treasure_list->player_chenghao(rp_pari.index),
				treasure_list->nalflag(rp_pari.index)
				);	
			rob_player_list->add_rob_player(rp);
		}
	}

	/// 选取npc
	std::vector<uint32_t> roles;
	for (int i = 0; i < npc_size; ++i)
	{
		RobPlayer rp(i + 1,
			sSportConfig->get_random_name(),
			sRoleConfig->get_random_role_id(Utils::get_int32(3, 4), roles),
			Utils::get_int32(player->level() - 5, player->level() + 5),
			Utils::get_int32(player->bf() * 0.7, player->bf()),
			t_suipian->npc_rate,
			true,
			0,
			0,
			0,
			0
			);
		rob_player_list->add_rob_player(rp);
	}

	/// 首次抢夺
	if (player->treasure_first() == 0)
	{
		rob_player_list->modify_first_suipian(player->level(), player->bf());
	}

	rob_player_list->get_rob_player_list(view_list);
}

void TreasureList::set_rob_player_list(dhc::player_t *player, int suipian_id)
{
	const s_t_treasure_suipian *t_suipian = sTreasureConfig->get_suipian(suipian_id);
	if (!t_suipian)
	{
		return;
	}
	reset_rob_treasure_list(player, t_suipian->treasure_id);
	save_treasure_list(t_suipian->ordernum, false);
}

void TreasureList::reset_rob_player_list(dhc::player_t *player)
{
	const std::map<int, s_t_treasure> &all_treasures = sTreasureConfig->get_all_treasure();
	for (std::map<int, s_t_treasure>::const_iterator it = all_treasures.begin();
		it != all_treasures.end();
		++it)
	{
		reset_rob_treasure_list(player, it->second.id);
	}
}

void TreasureList::update_rob_player_list(dhc::player_t *player)
{
	dhc::treasure_list_t *treasure_list = 0;
	const std::map<int, s_t_treasure_suipian> &all_suipian = sTreasureConfig->get_all_suipian();
	for (std::map<int, s_t_treasure_suipian>::const_iterator it = all_suipian.begin();
		it != all_suipian.end();
		++it)
	{
		const s_t_treasure_suipian &suipian = it->second;

		treasure_list = POOL_GET_TREASURE_LIST(MAKE_GUID(et_treasure_list, suipian.ordernum));
		if (treasure_list)
		{
			for (int i = 0; i < treasure_list->player_guid_size(); ++i)
			{
				if (treasure_list->player_guid(i) == player->guid())
				{
					treasure_list->set_player_name(i, player->name());
					treasure_list->set_player_template(i, player->template_id());
					treasure_list->set_player_level(i, player->level());
					treasure_list->set_player_bt(i, player->bf());
					treasure_list->set_player_time(i, player->treasure_protect_next_time());
					treasure_list->set_player_first(i, player->treasure_first());
					treasure_list->set_player_vip(i, player->vip());
					treasure_list->set_player_achieve(i, player->dress_achieves_size());
					treasure_list->set_player_chenghao(i, player->chenghao_on());
					treasure_list->set_nalflag(i, player->nalflag());
					break;
				}
			}
		}
	}
}

void TreasureList::clear_rob_player_list(dhc::player_t *player)
{
	player_list_.erase(player->guid());
}


RobPlayerList* TreasureList::get_rob_player_list(dhc::player_t *player)
{
	std::map<uint64_t, RobPlayerList>::iterator it = player_list_.find(player->guid());
	if (it == player_list_.end())
	{
		return 0;
	}
	return &(it->second);
}

RobPlayer *TreasureList::get_rob_player(dhc::player_t *player, uint64_t guid)
{
	RobPlayerList *rob_player_list = get_rob_player_list(player);
	if (!rob_player_list)
	{
		return 0;
	}

	return rob_player_list->get_rob_player(guid);
}

dhc::player_t *TreasureList::get_rob_fight_npc(dhc::player_t* player)
{
	fake_npc_->CopyFrom(*npc_);

	if (player->treasure_first() == 0)
	{
		for (int i = 0; i < player->zhenxing_size(); ++i)
		{
			if (player->zhenxing(i) == 0)
			{
				fake_npc_->set_zhenxing(i, 0);
			}
		}
	}
	else if (player->level() <= 23)
	{
		fake_npc_->set_zhenxing(4, 0);
		fake_npc_->set_zhenxing(5, 0);
	}
	else if (player->level() <= 33)
	{
		fake_npc_->set_zhenxing(5, 0);
	}

	dhc::role_t *role = 0;
	std::vector<uint32_t> rdroles;
	for (int i = 0; i < fake_npc_->zhenxing_size(); ++i)
	{
		role = POOL_GET_ROLE(fake_npc_->zhenxing(i));
		if (role)
		{
			uint32_t id = sRoleConfig->get_random_role_id(Utils::get_int32(3, 4), rdroles);
			role->set_template_id(id);
			s_t_role *t_role = sRoleConfig->get_role(id);
			if (!t_role)
			{
				continue;
			}
			role->set_pinzhi(t_role->pinzhi * 100);
			rdroles.push_back(id);
		}
	}
	return fake_npc_;
}

void TreasureList::update_rob_suipian_list(uint64_t guid, int suipian_id)
{
	std::map<uint64_t, RobSuipianList>::iterator it = suipian_list_.find(guid);
	if (it != suipian_list_.end())
	{
		it->second.suipian_ids.push_back(suipian_id);
		return;
	}
	RobSuipianList lt;
	lt.suipian_ids.push_back(suipian_id);
	suipian_list_[guid] = lt;
}

void TreasureList::remove_rob_suipian_list(uint64_t guid)
{
	suipian_list_.erase(guid);
}

void TreasureList::get_rob_suipian_list(uint64_t guid, std::vector<int> &suipian_id) const
{
	std::map<uint64_t, RobSuipianList>::const_iterator it = suipian_list_.find(guid);
	if (it != suipian_list_.end())
	{
		const RobSuipianList &suipianlist = it->second;
		suipian_id.insert(suipian_id.begin(), suipianlist.suipian_ids.begin(), suipianlist.suipian_ids.end());
	}
}

bool TreasureList::has_role_suipian_list(dhc::player_t *player) const
{
	for (int i = 0; i < player->treasure_reports_size(); ++i)
	{
		dhc::treasure_report_t *treasure_report = POOL_GET_TREASURE_REPORT(player->treasure_reports(i));
		if (treasure_report && treasure_report->new_report() == 0)
		{
			return true;
		}
	}

	std::map<uint64_t, RobSuipianList>::const_iterator it = suipian_list_.find(player->guid());
	if (it != suipian_list_.end())
	{
		return true;
	}

	return false;
}

RobPlayerList* TreasureList::new_rob_player_list(dhc::player_t *player, int suipian_id)
{
	player_list_.erase(player->guid());

	RobPlayerList rpl(suipian_id);
	player_list_[player->guid()] = rpl;

	return get_rob_player_list(player);
}

void TreasureList::reset_rob_treasure_list(dhc::player_t *player, int treasure_id)
{
	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(treasure_id);
	if (!t_treasure)
	{
		return;
	}

	const s_t_treasure_suipian *suipian = 0;
	dhc::treasure_list_t *treasure_list = 0;
	int player_index = -1;
	int item_num = 0;
	int total = 0;
	std::vector<__treasure_list_id> treasure_list_vector;

	for (std::vector<int>::size_type i = 0;
		i < t_treasure->suipian.size();
		++i)
	{
		suipian = sTreasureConfig->get_suipian(t_treasure->suipian[i]);
		if (suipian)
		{
			treasure_list = POOL_GET_TREASURE_LIST(MAKE_GUID(et_treasure_list, suipian->ordernum));
			if (treasure_list)
			{
				player_index = -1;
				for (int j = 0; j < treasure_list->player_guid_size(); ++j)
				{
					if (treasure_list->player_guid(j) == player->guid())
					{
						player_index = j;
						break;
					}
				}

				if (player_index == -1)
				{
					item_num = ItemOperation::item_num_templete(player, suipian->template_id);
					if (item_num > 0)
					{
						treasure_list->add_player_guid(player->guid());
						treasure_list->add_player_name(player->name());
						treasure_list->add_player_template(player->template_id());
						treasure_list->add_player_level(player->level());
						treasure_list->add_player_bt(player->bf());
						treasure_list->add_player_time(player->treasure_protect_next_time());
						treasure_list->add_player_first(player->treasure_first());
						treasure_list->add_player_vip(player->vip());
						treasure_list->add_player_achieve(player->dress_achieves_size());
						treasure_list->add_player_num(item_num);
						treasure_list->add_player_total(0);
						treasure_list->add_player_chenghao(player->chenghao_on());
						treasure_list->add_nalflag(player->nalflag());
						total += item_num;
						player_index = treasure_list->player_guid_size() - 1;
					}
				}
				else
				{
					if (treasure_list->player_name_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_template_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_level_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_bt_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_time_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_total_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_num_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_first_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_vip_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_achieve_size() != treasure_list->player_guid_size())
					{
						return;
					}
					if (treasure_list->player_chenghao_size() != treasure_list->player_guid_size())
					{
						return;
					}

					/// 重新设置相关信息
					treasure_list->set_player_template(player_index, player->template_id());
					treasure_list->set_player_level(player_index, player->level());
					treasure_list->set_player_bt(player_index, player->bf());
					treasure_list->set_player_time(player_index, player->treasure_protect_next_time());
					treasure_list->set_player_first(player_index, player->treasure_first());
					treasure_list->set_player_vip(player_index, player->vip());
					treasure_list->set_player_achieve(player_index, player->dress_achieves_size());
					treasure_list->set_player_chenghao(player_index, player->chenghao_on());
					treasure_list->set_nalflag(player_index, player->nalflag());
					item_num = ItemOperation::item_num_templete(player, suipian->template_id);
					treasure_list->set_player_num(player_index, item_num);
					total += item_num;
				}
				__treasure_list_id tli;
				tli.treasure_list = treasure_list;
				tli.player_index = player_index;
				treasure_list_vector.push_back(tli);
			}
		}
	}

	for (std::vector<__treasure_list_id>::size_type i = 0;
		i < treasure_list_vector.size();
		++i)
	{
		if (treasure_list_vector[i].treasure_list != 0 &&
			treasure_list_vector[i].player_index >= 0 &&
			treasure_list_vector[i].player_index < treasure_list_vector[i].treasure_list->player_guid_size())
		{
			treasure_list_vector[i].treasure_list->set_player_total(treasure_list_vector[i].player_index, total);
		}
	}
}

void TreasureList::load_treasure_list()
{
	const std::map<int, s_t_treasure_suipian> &all_suipian = sTreasureConfig->get_all_suipian();
	for (std::map<int, s_t_treasure_suipian>::const_iterator it = all_suipian.begin();
		it != all_suipian.end();
		++it)
	{
		const s_t_treasure_suipian &suipian = it->second;
		uint64_t guid = MAKE_GUID(et_treasure_list, suipian.ordernum);
		Request *req = new Request();
		req->add(opc_query, guid, new dhc::treasure_list_t());
		game::pool()->upcall(req, boost::bind(&TreasureList::load_treasure_list_callback, this, _1, suipian.template_id, guid));
	}
}


void TreasureList::load_treasure_list_callback(Request *req, int id, uint64_t guid)
{
	dhc::treasure_list_t *treasure_list = 0;
	if (req->success())
	{
		treasure_list = (dhc::treasure_list_t*)req->release_data();
		POOL_ADD(treasure_list->guid(), treasure_list);
	}
	else
	{
		treasure_list = new dhc::treasure_list_t();
		treasure_list->set_guid(guid);
		treasure_list->set_template_id(id);
		treasure_list->set_amount(0);
		POOL_ADD_NEW(guid, treasure_list);
		POOL_SAVE(dhc::treasure_list_t, treasure_list, false);
	}
}

void TreasureList::save_treasure_list(int num, bool release)
{
	dhc::treasure_list_t *treasure_list = POOL_GET_TREASURE_LIST(MAKE_GUID(et_treasure_list, num));
	if (treasure_list)
	{
		POOL_SAVE(dhc::treasure_list_t, treasure_list, release);
	}
}