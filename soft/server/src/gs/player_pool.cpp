#include "player_pool.h"
#include "player_def.h"
#include "player_operation.h"
#include "sport_operation.h"
#include "mission_pool.h"
#include "rank_operation.h"
#include "social_operation.h"
#include "guild_operation.h"
#include "pvp_operation.h"

PlayerPool::PlayerPool()
{
	last_time_ = game::timer()->now();
	add_time_ = 0;
}

PlayerPool::~PlayerPool()
{

}

void PlayerPool::add_player(uint64_t guid, bool is_login)
{
	UpdatePlayer up;
	up.guid = guid;
	up.update_time = game::timer()->now();
	update_list_.push_back(up);

	TermInfo *ti = game::channel()->get_channel(guid);
	if (!ti)
	{
		TermInfo ti;
		game::channel()->add_channel(guid, ti, is_login);
	}
	dhc::player_t *player = POOL_GET_PLAYER(guid);
	if (player)
	{
		SportOperation::refresh(player);
		add_name_player(player->name(), player->guid());
	}
}

void PlayerPool::update()
{
	/// 日常刷新
	uint64_t now = game::timer()->now();
	if (game::timer()->trigger_time(last_time_, 0, 0))
	{
		std::vector<uint64_t> player_guids;
		game::pool()->get_entitys(et_player, player_guids);
		for (int i = 0; i < player_guids.size(); ++i)
		{
			dhc::player_t *player = POOL_GET_PLAYER(player_guids[i]);
			if (player)
			{
				player->set_last_daily_time(now);
				PlayerOperation::player_refresh(player);
			}
		}
	}
	
	if (game::timer()->trigger_week_time(last_time_))
	{
		std::vector<uint64_t> player_guids;
		game::pool()->get_entitys(et_player, player_guids);
		for (int i = 0; i < player_guids.size(); ++i)
		{
			dhc::player_t *player = POOL_GET_PLAYER(player_guids[i]);
			if (player)
			{
				player->set_last_week_time(now);
				PlayerOperation::player_week_refresh(player);
			}
		}
	}

	if (game::timer()->trigger_month_time(last_time_))
	{
		std::vector<uint64_t> player_guids;
		game::pool()->get_entitys(et_player, player_guids);
		for (int i = 0; i < player_guids.size(); ++i)
		{
			dhc::player_t *player = POOL_GET_PLAYER(player_guids[i]);
			if (player)
			{
				player->set_last_month_time(now);
				PlayerOperation::player_month_refresh(player);
			}
		}
	}

	last_time_ = now;

	if (game::pool()->full())
	{
		return;
	}

	/// 数据保存&&玩家下线
	int player_num = game::channel()->get_channel_num();
	int normal_num = boost::lexical_cast<int>(game::env()->get_server_state("normal"));
	int busy_num = boost::lexical_cast<int>(game::env()->get_server_state("busy"));
	int upnum = 0;
	while (upnum < 10)
	{
		if (update_list_.empty())
		{
			break;
		}
		UpdatePlayer up = update_list_.front();
		if (now - up.update_time < PLAYER_UPDATE_TIME)
		{
			break;
		}
		dhc::player_t *player = POOL_GET_PLAYER(up.guid);
		TermInfo *ti = game::channel()->get_channel(up.guid);
		if (player && ti)
		{
			GuildOperation::refresh_guild_offline(player->guild(), up.guid);
			upnum++;
			update_list_.pop_front();
			if (player_num > normal_num && player_num <= busy_num)
			{
				if (now - ti->time > NORMAL_OFF_TIME)
				{
					save_player(player->guid(), true);
					game::channel()->del_channel(up.guid);
					continue;
				}
			}
			else if (player_num > busy_num)
			{
				if (now - ti->time > BUSY_OFF_TIME)
				{
					save_player(player->guid(), true);
					game::channel()->del_channel(up.guid);
					continue;
				}
			}
			else
			{
				game::channel()->refresh_offline_time(up.guid);
			}
			save_player(player->guid(), false);
			PlayerOperation::player_calc_force(player);
			SportOperation::refresh(player);
			RankOperation::refresh(player);
			SocialOperation::refresh(player);
			PvpOperation::sync(player);
			PlayerOperation::player_check_chenghao(player);
			up.update_time = now;
			update_list_.push_back(up);
		}
		else
		{
			update_list_.pop_front();
		}
	}
}

void PlayerPool::save_all()
{
	std::vector<uint64_t> guids;
	game::pool()->get_entitys(et_player, guids);
	for (int i = 0; i < guids.size(); ++i)
	{
		dhc::player_t *player = POOL_GET_PLAYER(guids[i]);
		if (player)
		{
			save_player(player->guid(), true);
		}
	}
}

void PlayerPool::save_player(uint64_t guid, bool release)
{
	dhc::player_t *player = POOL_GET_PLAYER(guid);
	if (player)
	{
		for (int i = 0; i < player->roles_size(); ++i)
		{
			uint64_t roles_guid = player->roles(i);
			dhc::role_t *role = POOL_GET_ROLE(roles_guid);
			if (role)
			{
				POOL_SAVE(dhc::role_t, role, release);
			}
		}

		for (int i = 0; i < player->equips_size(); ++i)
		{
			uint64_t equip_guid = player->equips(i);
			dhc::equip_t *equip = POOL_GET_EQUIP(equip_guid);
			if (equip)
			{
				POOL_SAVE(dhc::equip_t, equip, release);
			}
		}

		for (int i = 0; i < player->sports_size(); ++i)
		{
			uint64_t sport_guid = player->sports(i);
			dhc::sport_t *sport = POOL_GET_SPORT(sport_guid);
			if (sport)
			{
				POOL_SAVE(dhc::sport_t, sport, release);
			}
		}
		
		for (int i = 0; i < player->treasures_size(); ++i)
		{
			uint64_t treasure_guid = player->treasures(i);
			dhc::treasure_t *treasure = POOL_GET_TREASURE(treasure_guid);
			if (treasure)
			{
				POOL_SAVE(dhc::treasure_t, treasure, release);
			}
		}
		for (int i = 0; i < player->treasure_reports_size(); ++i)
		{
			uint64_t treasure_report_guid = player->treasure_reports(i);
			dhc::treasure_report_t *treasure_report = POOL_GET_TREASURE_REPORT(treasure_report_guid);
			if (treasure_report)
			{
				POOL_SAVE(dhc::treasure_report_t, treasure_report, release);
			}
		}
		for (int i = 0; i < player->pets_size(); ++i)
		{
			dhc::pet_t* pet = POOL_GET_PET(player->pets(i));
			if (pet)
			{
				POOL_SAVE(dhc::pet_t, pet, release);
			}
		}

		game::pool()->remove_ref(player->guid());
		if (release)
		{
			del_name_player(player->name());
			PlayerOperation::player_logout(player);
		}
		POOL_SAVE(dhc::player_t, player, release);
	}
}

void PlayerPool::add_name_player(const std::string &name, uint64_t player_guid)
{
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (player)
	{
		name_player_[player->name()] = player->guid();
	}
}

void PlayerPool::del_name_player(const std::string &name)
{
	name_player_.erase(name);
}

uint64_t PlayerPool::get_name_player(const std::string &name)
{
	if (name_player_.find(name) == name_player_.end())
	{
		return 0;
	}
	return name_player_[name];
}
