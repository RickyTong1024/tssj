#include "guild_operation.h"
#include "guild_pool.h"
#include "guild_config.h"
#include "mission_config.h"
#include "item_config.h"
#include "player_operation.h"
#include "post_operation.h"

#define MAX_GUILD_EVENT_COUNT 20
#define GUILD_MISSION_FIGHT_TIME 7200000
#define GUILD_MISSION_GUAISIZE 5
#define GUILD_MISSION_REWARD_BASE 10

void GuildOperation::guild_refresh_check(dhc::guild_t * guild)
{
	uint64_t now = game::timer()->now();
	if (game::timer()->trigger_time(guild->last_daily_time(), 0, 0))
	{
		guild->set_last_daily_time(now);
		GuildOperation::guild_refresh(guild);
	}
	if (game::timer()->trigger_week_time(guild->last_week_time()))
	{
		guild->set_last_week_time(now);
		GuildOperation::guild_week_refresh(guild);
	}
}

void GuildOperation::guild_refresh(dhc::guild_t * guild)
{
	guild->set_honor(0);
	guild->set_mobai_num(0);
	guild->set_last_level(guild->level());

	// 刷新公会战
	dhc::guild_mission_t *guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
	if (guild_mission)
	{
		GuildOperation::create_guild_mission(guild_mission, true);
		sGuildPool->update_guild_mission_compare(guild, guild_mission);
	}

	uint64_t now_time = game::timer()->now();
	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		dhc::guild_member_t *member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (member)
		{
			member->clear_honors();
			member->set_mnum(0);
			member->set_mbnum(0);
			member->set_last_mbtime(now_time);
			member->clear_mission_rewards();
			member->set_sign_flag(0);
		}
	}
}

void GuildOperation::guild_week_refresh(dhc::guild_t * guild)
{
	// 刷新本周贡献
	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		dhc::guild_member_t *member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (member)
		{
			member->set_contribution(0);
		}
	}
}

operror_t GuildOperation::check_guild_name(const std::string &name)
{
	if (name.empty())
	{
		return ERROR_NAME_EMPTY;
	}
	if (name.size() > NAME_COUNT_MAX)
	{
		return ERROR_NAME_LONG;
	}
	if (game::scheme()->search_illword(name, false,false) == -1)		// 是否有不合法的字符
	{
		return ERROR_NAME_ILL;
	}
	if (!sGuildPool->check_name(name))	// 是否重名
	{
		return ERROR_NAME_HAS;
	}
	return operror_t(0);
}

operror_t GuildOperation::check_bulletin(const std::string &name)
{
	if (name.empty())
	{
		return ERROR_NAME_EMPTY;
	}
	if (name.size() > BULLETIN_COUNT_MAX)
	{
		return ERROR_NAME_LONG;
	}
	if (game::scheme()->search_illword(name,true, true) == -1)		// 是否有不合法的字符
	{
		return ERROR_NAME_ILL;
	}

	return operror_t(0);
}

uint32_t GuildOperation::get_guild_member_count(dhc::guild_t *guild, int type)
{
	int count = 0;
	for (int i = 0; i != guild->member_guids_size(); ++i)
	{
		dhc::guild_member_t *guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (!guild_member)
		{
			continue;
		}
		if (guild_member->zhiwu() == type)
		{
			count++;
		}
	}
	return count;
}

dhc::guild_member_t* GuildOperation::get_guild_member(dhc::guild_t *guild, uint64_t player_guid)
{
	for (int i = 0; i != guild->member_guids_size(); ++i)
	{
		dhc::guild_member_t *guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (!guild_member)
		{
			continue;
		}
		if (guild_member->player_guid() == player_guid)
		{
			return guild_member;
		}
	}
	return 0;
}

dhc::guild_member_t* GuildOperation::create_guild_member(dhc::guild_t *guild, dhc::player_t *player, int duty)
{
	if (duty < e_member_type_leader || duty > e_member_type_common)
	{
		return 0;
	}

	dhc::guild_member_t *guild_member = new dhc::guild_member_t;
	uint64_t guild_member_guid = game::gtool()->assign(et_guild_member);
	guild_member->set_guid(guild_member_guid);
	guild_member->set_guild_guid(guild->guid());
	guild_member->set_player_guid(player->guid());
	guild_member->set_player_iocn_id(player->template_id());
	guild_member->set_player_level(player->level());
	guild_member->set_player_name(player->name());
	guild_member->set_bf(player->bf());
	guild_member->set_zhiwu(duty);
	guild_member->set_join_time(game::timer()->now());
	guild_member->set_last_sign_time(game::timer()->now() - 86400000);
	guild_member->set_last_mbtime(game::timer()->now());
	guild_member->set_player_vip(player->vip());
	guild_member->set_player_achieve(player->dress_achieves_size());
	guild_member->set_offline_time(game::timer()->now());
	guild_member->set_nalflag(player->nalflag());
	for (int i = 0; i < player->map_star_size(); ++i)
	{
		guild_member->set_map_star(guild_member->map_star() + player->map_star(i));
	}

	player->set_guild(guild->guid());
	player->set_last_leave_guild_time(game::timer()->now());

	POOL_ADD_NEW(guild_member_guid, guild_member);

	guild->add_member_guids(guild_member_guid);

	return guild_member;
}

dhc::guild_event_t* GuildOperation::create_guild_event(dhc::guild_t *guild, const std::string &name, int type, int value, int value1)
{
	if (type < e_guild_op_type_join || type >= e_guild_op_type_end)
	{
		return 0;
	}

	dhc::guild_event_t *p_guild_event = new dhc::guild_event_t;
	uint64_t event_guid = game::gtool()->assign(et_guild_event);
	p_guild_event->set_guid(event_guid);
	p_guild_event->set_guild_guid(guild->guid());
	p_guild_event->set_create_name(guild->leader_name());
	p_guild_event->set_player_name(name);
	p_guild_event->set_type(type);
	p_guild_event->set_value(value);
	p_guild_event->set_value1(value1);
	p_guild_event->set_time(game::timer()->now());

	if (guild->event_guids_size() >= MAX_GUILD_EVENT_COUNT)
	{
		int i = 0;
		uint64_t temp_guid = guild->event_guids(0);
		for (; i < guild->event_guids_size() - 1; ++i)
		{
			guild->set_event_guids(i, guild->event_guids(i + 1));
		}
		guild->set_event_guids(i, event_guid);
		POOL_REMOVE(temp_guid, guild->guid());
	}
	else
	{
		guild->add_event_guids(event_guid);
	}
	
	POOL_ADD_NEW(event_guid, p_guild_event);

	return p_guild_event;
}


void GuildOperation::mod_guild_exp(dhc::guild_t * guild, int add_exp)
{
	guild->set_exp(guild->exp() + add_exp);	
	s_t_guild *t_guild = sGuildConfig->get_guild(guild->level() + 1);
	if (!t_guild)
	{
		return;
	}
	if (t_guild->exp <= 0)
	{
		return;
	}
	while (guild->exp() >= t_guild->exp)
	{
		guild->set_level(guild->level() + 1); 
		guild->set_exp(guild->exp() - t_guild->exp);
		create_guild_event(guild, "", e_guild_op_type_levelup, guild->level());

		t_guild = sGuildConfig->get_guild(guild->level() + 1);
		if (!t_guild)
		{
			break;
		}
		if (t_guild->exp <= 0)
		{
			break;
		}
	}
}

bool GuildOperation::member_compare(dhc::guild_member_t *member1, dhc::guild_member_t *member2)
{
	if (member1->zhiwu() < member2->zhiwu())
	{
		return true;
	}
	else if (member1->zhiwu() == member2->zhiwu())
	{
		if (member1->join_time() < member2->join_time())
		{
			return true;
		}
	}
	return false;
}

bool GuildOperation::guild_compare(dhc::guild_t *guild1, dhc::guild_t *guild2)
{
	if (guild1->level() != guild2->level())
	{
		return guild1->level() > guild2->level();
	}
	return guild1->exp() > guild2->exp();
}

dhc::guild_mission_t* GuildOperation::create_guild_mission(dhc::guild_t *guild)
{
	uint64_t guild_mission_guid = game::gtool()->assign(et_guild_mission);
	dhc::guild_mission_t *guild_mission = new dhc::guild_mission_t();
	guild_mission->set_guid(guild_mission_guid);
	guild_mission->set_guild_guid(guild->guid());
	guild_mission->set_guild_ceng(0);
	guild_mission->set_guild_max_ceng(0);
	guild_mission->set_guild_last_ceng(0);
	guild->set_mission(guild_mission_guid);
	create_guild_mission(guild_mission);

	POOL_ADD_NEW(guild_mission_guid, guild_mission);

	return guild_mission;
}


int GuildOperation::mod_guild_mission(dhc::player_t* player, dhc::guild_mission_t *guild_mission, int mission)
{
	int stat = 0;
	int count = 0;
	int bot = 0;
	int start = mission * GUILD_MISSION_GUAISIZE;
	int end = mission * GUILD_MISSION_GUAISIZE + GUILD_MISSION_GUAISIZE;

	for (int i = 0; i < guild_mission->guild_monsters_size(); ++i)
	{
		if (guild_mission->guild_cur_hps(i) <= 0)
		{
			if (i >= start && i < end)
			{
				bot++;
			}
			count++;
		}
	}

	if (bot == GUILD_MISSION_GUAISIZE)
	{
		add_guild_mission_reward(guild_mission, guild_mission->guild_ceng(), mission + 1, player->name());
		const s_t_guild_mission* t_mission = sGuildConfig->get_guild_mission(guild_mission->guild_ceng());
		if (t_mission)
		{
			dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
			if (guild)
			{
				mod_guild_exp(guild, t_mission->guild_exp);
				create_guild_event(guild, "", e_guild_op_type_mission, mission + 1, t_mission->guild_exp);
			}
		}
		stat = 1;
	}

	if (count == guild_mission->guild_monsters_size())
	{
		add_guild_mission_reward(guild_mission, guild_mission->guild_ceng(), 0, "");

		create_guild_mission(guild_mission);
	}

	return stat;
}

void GuildOperation::create_guild_mission(dhc::guild_mission_t *guild_mission, bool reset)
{
	if (reset)
	{
		dhc::guild_t *guild = POOL_GET_GUILD(guild_mission->guild_guid());
		if (guild)
		{
			for (int kg = 0; kg < guild->box_guids_size(); ++kg)
			{
				POOL_REMOVE(guild->box_guids(kg), guild->guid());
			}
		}

		if (guild_mission->guild_ceng() - 1 > guild_mission->guild_max_ceng())
		{
			guild_mission->set_guild_max_ceng(guild_mission->guild_ceng() - 1);
		}
		guild_mission->set_guild_ceng(guild_mission->guild_max_ceng() - 2);
		guild_mission->set_guild_last_ceng(guild_mission->guild_max_ceng() - 1);
		if (guild_mission->guild_ceng() < 0)
		{
			guild_mission->set_guild_ceng(0);
		}
		if (guild_mission->guild_last_ceng() < 0)
		{
			guild_mission->set_guild_last_ceng(0);
		}
		
		
		guild_mission->clear_player_guids();
		guild_mission->clear_player_templates();
		guild_mission->clear_player_names();
		guild_mission->clear_player_damages();
		guild_mission->clear_player_counts();
		guild_mission->clear_mission_rewards();
		guild_mission->clear_mission_names();
		guild_mission->clear_player_vips();
		guild_mission->clear_player_achieves();
		guild_mission->clear_player_chenghaos();
		guild_mission->clear_nalflag();

		for (int km = 0; km < guild_mission->guild_max_ceng() - 2; ++km)
		{
			add_guild_mission_reward(guild_mission, km + 1, 0, "");
		}
	}

	guild_mission->set_guild_ceng(guild_mission->guild_ceng() + 1);
	guild_mission->clear_guild_monsters();
	guild_mission->clear_guild_max_hps();
	guild_mission->clear_guild_cur_hps();

	const s_t_guild_mission* t_guild_mission = sGuildConfig->get_guild_mission(guild_mission->guild_ceng());
	if (t_guild_mission)
	{
		const s_t_monster* t_monster = 0;
		const s_t_monster* t_boss = 0;
		for (std::vector<s_t_guild_guai>::size_type i = 0;
			i < t_guild_mission->guai.size();
			++i)
		{
			const s_t_guild_guai &guai = t_guild_mission->guai[i];

			t_monster = sMissionConfig->get_monster(guai.monster_id);
			if (t_monster)
			{
				for (int mn = 0; mn < 2; ++mn)
				{
					guild_mission->add_guild_monsters(t_monster->id);
					guild_mission->add_guild_max_hps(t_monster->hp);
					guild_mission->add_guild_cur_hps(t_monster->hp);
				}
			}
			else
			{
				for (int mn = 0; mn < 2; ++mn)
				{
					guild_mission->add_guild_monsters(0);
					guild_mission->add_guild_max_hps(0);
					guild_mission->add_guild_cur_hps(0);
				}
			}
			t_boss = sMissionConfig->get_monster(guai.boss_id);
			if (t_boss)
			{
				guild_mission->add_guild_monsters(t_boss->id);
				guild_mission->add_guild_max_hps(t_boss->hp);
				guild_mission->add_guild_cur_hps(t_boss->hp);
			}
			else
			{
				guild_mission->add_guild_monsters(0);
				guild_mission->add_guild_max_hps(0);
				guild_mission->add_guild_cur_hps(0);
			}
			if (t_monster)
			{
				for (int mn = 0; mn < 2; ++mn)
				{
					guild_mission->add_guild_monsters(t_monster->id);
					guild_mission->add_guild_max_hps(t_monster->hp);
					guild_mission->add_guild_cur_hps(t_monster->hp);
				}
			}
			else
			{
				for (int mn = 0; mn < 2; ++mn)
				{
					guild_mission->add_guild_monsters(0);
					guild_mission->add_guild_max_hps(0);
					guild_mission->add_guild_cur_hps(0);
				}
			}
		}
	}
}

void GuildOperation::mod_guild_mission_damage(dhc::player_t *player, dhc::guild_mission_t *guild_mission, int64_t hp)
{
	if (hp <= 0)
	{
		return;
	}

	int index = -1;
	for (int i = 0; i < guild_mission->player_guids_size(); ++i)
	{
		if (guild_mission->player_guids(i) == player->guid())
		{
			index = i;
			guild_mission->set_player_damages(i, guild_mission->player_damages(i) + hp);
			guild_mission->set_player_counts(i, guild_mission->player_counts(i) + 1);
			guild_mission->set_player_templates(i, player->template_id());
			guild_mission->set_player_vips(i, player->vip());
			guild_mission->set_player_achieves(i, player->dress_achieves_size());
			guild_mission->set_player_chenghaos(i, player->chenghao_on());
			guild_mission->set_nalflag(i, player->nalflag());
			break;
		}
	}
	if (index == -1)
	{
		guild_mission->add_player_guids(player->guid());
		guild_mission->add_player_templates(player->template_id());
		guild_mission->add_player_names(player->name());
		guild_mission->add_player_damages(hp);
		guild_mission->add_player_counts(1);
		guild_mission->add_player_vips(player->vip());
		guild_mission->add_player_achieves(player->dress_achieves_size());
		guild_mission->add_player_chenghaos(player->chenghao_on());
		guild_mission->add_nalflag(player->nalflag());
		index = guild_mission->player_guids_size() - 1;
	}
	for (int i = index; i >= 1; --i)
	{
		if (guild_mission->player_damages(i) > guild_mission->player_damages(i - 1))
		{
			guild_mission->mutable_player_guids()->SwapElements(i, i - 1);
			guild_mission->mutable_player_templates()->SwapElements(i, i - 1);
			guild_mission->mutable_player_names()->SwapElements(i, i - 1);
			guild_mission->mutable_player_damages()->SwapElements(i, i - 1);
			guild_mission->mutable_player_counts()->SwapElements(i, i - 1);
			guild_mission->mutable_player_vips()->SwapElements(i, i - 1);
			guild_mission->mutable_player_achieves()->SwapElements(i, i - 1);
			guild_mission->mutable_player_chenghaos()->SwapElements(i, i - 1);
			guild_mission->mutable_nalflag()->SwapElements(i, i - 1);
		}
		else
		{
			break;
		}
	}
}

void GuildOperation::create_guild_message(dhc::player_t *player, dhc::guild_t* guild, int zhiwu, const std::string &text)
{
	dhc::guild_member_t* guild_member = 0;
	for (int i = 0; i < guild->member_guids_size(); ++i)
	{
		guild_member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
		if (guild_member && guild_member->player_guid() != player->guid())
		{
			guild_member->set_msg_count(guild_member->msg_count() + 1);
		}
	}
	uint64_t msg_guid = game::gtool()->assign(et_guild_message);
	dhc::guild_message_t* guild_msg1 = new dhc::guild_message_t;
	guild_msg1->set_guid(msg_guid);
	guild_msg1->set_guild_guid(guild->guid());
	guild_msg1->set_name(player->name());
	guild_msg1->set_text(text);
	guild_msg1->set_zhiwu(zhiwu);
	guild_msg1->set_time(game::timer()->now());
	guild_msg1->set_otype(0);
	guild->add_message_guids(msg_guid);

	dhc::guild_message_t* guild_msg = 0;
	uint64_t delete_guid = 0;
	if (guild->message_guids_size() >= 10)
	{
		for (int i = 0; i < guild->message_guids_size(); ++i)
		{
			guild_msg = POOL_GET_GUILD_MESSAGE(guild->message_guids(i));
			if (guild_msg && guild_msg->otype() != 1)
			{
				delete_guid = guild_msg->guid();
				for (int j = i; j < guild->message_guids_size() - 1; ++j)
				{
					guild->set_message_guids(j, guild->message_guids(j + 1));
				}
				guild->mutable_message_guids()->RemoveLast();
				break;
			}
		}
	}
	if (delete_guid != 0)
	{
		POOL_REMOVE(delete_guid, guild->guid());
	}

	POOL_ADD_NEW(msg_guid, guild_msg1);
}

void GuildOperation::delete_guild_message(dhc::guild_t* guild, uint64_t msg_guid)
{
	for (int i = 0; i < guild->message_guids_size(); ++i)
	{
		if (guild->message_guids(i) == msg_guid)
		{
			for (int j = i; j < guild->message_guids_size() - 1; ++j)
			{
				guild->set_message_guids(j, guild->message_guids(j + 1));
			}
			guild->mutable_message_guids()->RemoveLast();
			POOL_REMOVE(msg_guid, guild->guid());
			break;
		}
	}
}

void GuildOperation::top_guild_message(dhc::guild_t* guild, uint64_t msg_guid)
{
	dhc::guild_message_t* guild_msg = 0;
	dhc::guild_message_t *top_guild_msg = 0;
	dhc::guild_message_t* oth_guild_msg = 0;

	for (int i = 0; i < guild->message_guids_size(); ++i)
	{
		if (guild->message_guids(i) == msg_guid)
		{
			top_guild_msg = POOL_GET_GUILD_MESSAGE(msg_guid);
		}
		else
		{
			guild_msg = POOL_GET_GUILD_MESSAGE(guild->message_guids(i));
			if (guild_msg && guild_msg->otype() == 1)
			{
				oth_guild_msg = guild_msg;
			}
		}
	}

	if (top_guild_msg)
	{
		top_guild_msg->set_otype(1);
	}
	if (oth_guild_msg)
	{
		oth_guild_msg->set_otype(0);
	}
}

void GuildOperation::add_guild_mission_reward(dhc::guild_mission_t *guild_mission, int ceng, int index, const std::string& name)
{
	int reward = ceng * GUILD_MISSION_REWARD_BASE + index;
	for (int i = 0; i < guild_mission->mission_rewards_size(); ++i)
	{
		if (guild_mission->mission_rewards(i) == reward)
		{
			return;
		}
	}
	guild_mission->add_mission_rewards(reward);
	guild_mission->add_mission_names(name);
}

void GuildOperation::add_guild_member_reward(dhc::guild_member_t *guild_member, int ceng, int index)
{
	int reward = ceng * GUILD_MISSION_REWARD_BASE + index;
	for (int i = 0; i < guild_member->mission_rewards_size(); ++i)
	{
		if (guild_member->mission_rewards(i) == reward)
		{
			return;
		}
	}
	guild_member->add_mission_rewards(reward);
}

void GuildOperation::add_guild_member_reward(dhc::player_t *player, int ceng, int index)
{
	int reward = ceng * GUILD_MISSION_REWARD_BASE + index;
	for (int i = 0; i < player->guild_shilians_size(); ++i)
	{
		if (player->guild_shilians(i) == reward)
		{
			return;
		}
	}
	player->add_guild_shilians(reward);
}

bool GuildOperation::has_guild_mission_reward(dhc::guild_mission_t *guild_mission, int ceng, int index)
{
	int reward = ceng * GUILD_MISSION_REWARD_BASE + index;
	for (int i = 0; i < guild_mission->mission_rewards_size(); ++i)
	{
		if (guild_mission->mission_rewards(i) == reward)
		{
			return true;
		}
	}

	return false;
}

bool GuildOperation::has_guild_member_reward(dhc::guild_member_t *guild_member, int ceng, int index)
{
	int reward = ceng * GUILD_MISSION_REWARD_BASE + index;
	for (int i = 0; i < guild_member->mission_rewards_size(); ++i)
	{
		if (guild_member->mission_rewards(i) == reward)
		{
			return true;
		}
	}

	return false;
}

bool GuildOperation::has_guild_member_reward(dhc::player_t *player, int ceng, int index)
{
	int reward = ceng * GUILD_MISSION_REWARD_BASE + index;
	for (int i = 0; i < player->guild_shilians_size(); ++i)
	{
		if (player->guild_shilians(i) == reward)
		{
			return true;
		}
	}

	return false;
}

int GuildOperation::get_guild_apply_index(dhc::guild_t* guild, uint64_t player_guid)
{
	for (int i = 0; i < guild->apply_guids_size(); ++i)
	{
		if (guild->apply_guids(i) == player_guid)
		{
			return i;
		}
	}
	return -1;
}

void GuildOperation::remove_player_apply(dhc::player_t *player, dhc::guild_t* guild, bool remove /* = true */)
{
	int apply_index = get_guild_apply_index(guild, player->guid());
	if (apply_index != -1)
	{
		int apply_swap = guild->apply_guids_size() - 1;
		guild->mutable_apply_guids()->SwapElements(apply_index, apply_swap);
		guild->mutable_apply_guids()->RemoveLast();
		guild->mutable_apply_names()->SwapElements(apply_index, apply_swap);
		guild->mutable_apply_names()->RemoveLast();
		guild->mutable_apply_template()->SwapElements(apply_index, apply_swap);
		guild->mutable_apply_template()->RemoveLast();
		guild->mutable_apply_level()->SwapElements(apply_index, apply_swap);
		guild->mutable_apply_level()->RemoveLast();
		guild->mutable_apply_bf()->SwapElements(apply_index, apply_swap);
		guild->mutable_apply_bf()->RemoveLast();
		guild->mutable_apply_vip()->SwapElements(apply_index, apply_swap);
		guild->mutable_apply_vip()->RemoveLast();
		guild->mutable_apply_achieve()->SwapElements(apply_index, apply_swap);
		guild->mutable_apply_achieve()->RemoveLast();
		guild->mutable_apply_nalfags()->SwapElements(apply_index,apply_swap);
		guild->mutable_apply_nalfags()->RemoveLast();
	}

	if (remove)
	{
		for (int i = 0; i < player->guild_applys_size(); ++i)
		{
			if (player->guild_applys(i) == guild->guid())
			{
				for (int j = i; j < player->guild_applys_size() - 1; ++j)
				{
					player->set_guild_applys(j, player->guild_applys(j + 1));
				}
				player->mutable_guild_applys()->RemoveLast();
				break;
			}
		}
	}
	
}

void GuildOperation::remove_player_all_apply(dhc::player_t* player)
{
	dhc::guild_t* guild = 0;
	for (int i = 0; i < player->guild_applys_size(); ++i)
	{
		guild = POOL_GET_GUILD(player->guild_applys(i));
		if (guild)
		{
			remove_player_apply(player, guild, false);
		}
	}
	player->clear_guild_applys();
}

int GuildOperation::has_red_point(dhc::player_t* player)
{
	if (player->guild() > 0)
	{
		dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
		if (guild)
		{
			dhc::guild_member_t* member = get_guild_member(guild, player->guid());
			if (member)
			{
				if (has_mobai_point(player))
				{
					return 1;
				}

				if (has_fight_point(guild, player))
				{
					return 1;
				}

				if (has_apply_point(guild, member))
				{
					return 1;
				}

				if (member->msg_count() > 0)
				{
					return 1;
				}

				if (has_shop_point(guild, player))
				{
					return 1;
				}

				if (has_guild_red_point(guild, player))
				{
					return 1;
				}

				dhc::guild_mission_t* guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
				if (guild_mission)
				{
					if (has_reward_point(player, guild_mission))
					{
						return 1;
					}
				}
			}
		}
	}
	return 0;
}

bool GuildOperation::has_mobai_point(dhc::player_t *player)
{
	if (game::timer()->run_day(player->guild_sign_time()) != 0)
	{
		return true;
	}
	return false;
}

bool GuildOperation::has_fight_point(dhc::guild_t* guild, dhc::player_t *player)
{
	if (guild->level() >= 2 &&
		player->guild_num() > 0)
	{
		int hour = game::timer()->hour();
		if (hour >= 10 && hour < 22)
		{
			return true;
		}
	}
	return false;
}

bool GuildOperation::has_reward_point(dhc::player_t* player, dhc::guild_mission_t* mission)
{
	bool has_reward = false;
	for (int i = 0; i < mission->mission_rewards_size(); ++i)
	{
		has_reward = false;
		for (int j = 0; j < player->guild_shilians_size(); ++j)
		{
			if (player->guild_shilians(j) == mission->mission_rewards(i))
			{
				has_reward = true;
				break;
			}
		}
		for (int j = 0; j < player->guild_rewards_size(); ++j)
		{
			if (player->guild_rewards(j) == mission->mission_rewards(i))
			{
				has_reward = true;
				break;
			}
		}
		if (!has_reward)
		{
			return true;
		}
	}
	return false;
}

bool GuildOperation::has_keji_point(dhc::guild_t* guild, dhc::player_t* player, dhc::guild_member_t* member)
{
	const s_t_guild_skill* t_keji = 0;

	if (member->zhiwu() < 2)
	{
		for (int i = 0; i < guild->skill_ids_size(); ++i)
		{
			t_keji = sGuildConfig->get_guild_skill(guild->skill_ids(i));
			if (t_keji && guild->level() >= t_keji->guild_level)
			{
				if (guild->skill_levels(i) < ((guild->level() - t_keji->guild_level + 1) * t_keji->study_level))
				{
					if (guild->exp() >= (guild->skill_levels(i) * t_keji->exp_jiacheng + t_keji->exp))
					{
						return true;
					}
				}
			}
		}
	}

	t_keji = 0;
	for (int i = 0; i < player->guild_skill_ids_size(); ++i)
	{
		for (int j = 0; j < guild->skill_ids_size(); ++j)
		{
			if (player->guild_skill_ids(i) == guild->skill_ids(j))
			{
				if (player->guild_skill_levels(i) < guild->skill_levels(j))
				{
					t_keji = sGuildConfig->get_guild_skill(player->guild_skill_ids(i));
					if (t_keji && guild->level() >= t_keji->guild_level)
					{
						if (player->contribution() >= (player->guild_skill_levels(i)  * t_keji->contri_jiacheng + t_keji->contri))
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

bool GuildOperation::has_apply_point(dhc::guild_t* guild, dhc::guild_member_t* member)
{
	if (member->zhiwu() < 2 && guild->apply_guids_size() > 0)
	{
		return true;
	}
	return false;
}

bool GuildOperation::has_shop_point(dhc::guild_t* guild, dhc::player_t* player)
{
	const std::map<int, s_t_guild_mubiao>& guild_mubiao = sItemConfig->get_guild_mubiao();
	if (player->guild_shop_rewards_size() >= guild_mubiao.size())
	{
		return false;
	}

	bool has_buy = false;
	for (std::map<int, s_t_guild_mubiao>::const_iterator it = guild_mubiao.begin();
		it != guild_mubiao.end();
		++it)
	{
		const s_t_guild_mubiao& mubiao = it->second;
		if (guild->level() >= mubiao.level &&
			player->contribution() >= mubiao.price)
		{
			has_buy = false;
			for (int i = 0; i < player->guild_shop_rewards_size(); ++i)
			{
				if (player->guild_shop_rewards(i) == mubiao.id)
				{
					has_buy = true;
					break;
				}
			}
			if (!has_buy)
			{
				return true;
			}
		}
	}
	return false;
}

bool GuildOperation::has_guild_red_point(dhc::guild_t *guild, dhc::player_t *player)
{
	if (player->guild_red_num1() >= 5)
	{
		return false;
	}

	if (guild->red_guids_size() <= 0)
	{
		return false;
	}

	uint64_t now_time = game::timer()->now();
	dhc::guild_red_t *guild_red = 0;
	bool has_me = false;
	for (int i = 0; i < guild->red_guids_size(); ++i)
	{
		guild_red = POOL_GET_GUILD_RED(guild->red_guids(i));
		if (!guild_red)
		{
			continue;
		}

		if (guild_red->player_guid_size() >= 20)
		{
			continue;
		}

		if (now_time > guild_red->time() + 86400000)
		{
			continue;
		}

		has_me = false;
		for (int j = 0; j < guild_red->player_guid_size(); ++j)
		{
			if (guild_red->player_guid(j) == player->guid())
			{
				has_me = true;
				break;
			}
		}
		if (!has_me)
		{
			return true;
		}
	}

	return false;
}

void GuildOperation::refresh_guild_offline(uint64_t guild_guid, uint64_t player_guid)
{
	if (!game::channel()->online(player_guid))
	{
		return;
	}
	dhc::guild_t *guild = POOL_GET_GUILD(guild_guid);
	if (!guild)
	{
		return;
	}
	dhc::guild_member_t *guild_member = get_guild_member(guild, player_guid);
	if (!guild_member)
	{
		return;
	}
	guild_member->set_offline_time(game::timer()->now());
}

void GuildOperation::post_leave_guild(uint64_t player_guid, const std::string& guild_name, int type)
{
	std::string sender;
	std::string title;
	std::string text;
	std::vector<s_t_reward> rds;
	int lang_ver = game::channel()->get_channel_lang(player_guid);
	game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
	if (type == 0)
	{
		game::scheme()->get_server_str(lang_ver, title, "guild_leave_title");
		game::scheme()->get_server_str(lang_ver, text, "guild_leave_text", guild_name.c_str());
	}
	else
	{
		game::scheme()->get_server_str(lang_ver, title, "guild_jiesan_title");
		game::scheme()->get_server_str(lang_ver, text, "guild_jiesan_text", guild_name.c_str());
	}
	
	PostOperation::post_create(player_guid, title, text, sender, rds);
}

void GuildOperation::refresh_guild_mission_name_nalflag(dhc::player_t* player, const std::string& name,const int& nalflag)
{
	if (player->guild() > 0)
	{
		dhc::guild_t* guild = POOL_GET_GUILD(player->guild());
		if (guild)
		{
			dhc::guild_mission_t* guild_mission = POOL_GET_GUILD_MISSION(guild->mission());
			if (guild_mission)
			{
				for (int i = 0; i < guild_mission->player_guids_size(); ++i)
				{
					if (guild_mission->player_guids(i) == player->guid())
					{
						guild_mission->set_player_names(i, player->name());
						if (nalflag != -1)
						{
							guild_mission->set_nalflag(i,player->nalflag());
						}
						break;
					}
				}
			}			
			dhc::guild_member_t *member = 0;
			for (int i = 0; i < guild->member_guids_size(); ++i)
			{
				member = POOL_GET_GUILD_MEMBER(guild->member_guids(i));
				if (member)
				{
					if (member->player_guid() == player->guid())
					{
						member->set_player_name(player->name());
						if (nalflag != -1)
						{
							member->set_nalflag(player->nalflag());
						}

						if (member->zhiwu() == 0)
						{
							guild->set_leader_name(player->name());
						}
					}
					else
					{
						std::string sender;
						std::string title;
						std::string text;
						std::vector<s_t_reward> rds;
						int lang_ver = game::channel()->get_channel_lang(player->guid());
						game::scheme()->get_server_str(lang_ver, sender, "sys_sender");
						game::scheme()->get_server_str(lang_ver, title, "change_name_guild_title");
						game::scheme()->get_server_str(lang_ver, text, "change_name_guild_text", name.c_str(), player->name().c_str());
						PostOperation::post_create(member->player_guid(), title, text, sender, rds);
					}
				}
			}
		}
	}
}


dhc::guild_red_t* GuildOperation::create_guild_red(dhc::player_t *player, dhc::guild_t *guild, int id, int jewel, const std::string& text)
{
	dhc::guild_red_t *guild_red = new dhc::guild_red_t();
	guild_red->set_guid(game::gtool()->assign(et_guild_red));
	guild_red->set_guild_guid(guild->guid());
	guild_red->set_create_name(player->name());
	guild_red->set_create_id(player->template_id());
	guild_red->set_create_vip(player->vip());
	guild_red->set_create_achieve(player->dress_achieves_size());
	guild_red->set_template_id(id);
	guild_red->set_time(game::timer()->now());
	guild_red->set_text(text);
	guild_red->set_remain(jewel);
	guild_red->set_nalflag(player->nalflag());
	guild->add_red_guids(guild_red->guid());
	
	POOL_ADD_NEW(guild_red->guid(), guild_red);

	return guild_red;
}

int GuildOperation::get_guild_pvp_state(dhc::global_t *global, dhc::guild_t *guild)
{
	if (global->guild_pvp_zhou() % 2 != 0)
	{
		return e_pvp_guild_xiuzhan;
	}

	int day = sGuildPool->get_guild_pvp_week();
	int now_time = sGuildPool->get_guild_pvp_hour();

	if (day == 1 || (day == 2 && now_time < 10))
	{
		if (guild->juntuan_apply() == 1)
		{
			return e_pvp_guild_look_bushu;
		}
		return e_pvp_guild_apply;
	}

	if (day > 2 && now_time < 10)
	{
		if (guild->juntuan_apply() == 1)
		{
			return e_pvp_guild_look_zhanji;
		}
		return e_pvp_guild_fight_quexi;
	}

	if (day >= 2 && now_time >= 10)
	{
		if (guild->juntuan_apply() == 1)
		{
			return e_pvp_guild_look_pipei;
		}
		return e_pvp_guild_fight_quexi;
	}

	return -1;
}

void GuildOperation::set_guild_pvp_state(int day, int hour)
{
	sGuildPool->set_guild_pvp_week(day);
	sGuildPool->set_guild_pvp_hour(hour);
}

bool GuildOperation::has_guildpvp_point(dhc::guild_t *guild, dhc::player_t *player, dhc::guild_member_t* member)
{
	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (global && (global->guild_pvp_zhou() % 2 == 0))
	{
		int week = sGuildPool->get_guild_pvp_week();
		int hour = sGuildPool->get_guild_pvp_hour();
		if (guild->juntuan_apply() == 0)
		{
			if ((member->zhiwu() != 2) && (week == 1 || (week == 2 && hour < 10)))
			{
				return true;
			}
			return false;
		}
		else
		{
			if ((week == 1 || hour < 10) && member->zhiwu() != 2)
			{
				return true;
			}

			if ((week >= 2 && hour >= 10) && player->guild_pvp_num() > 0)
			{
				return true;
			}
		}
	}
	return false;
}
