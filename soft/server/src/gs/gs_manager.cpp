#include "gs_manager.h"

GsManager::GsManager()
{
	
}

GsManager::~GsManager()
{
	
}

int GsManager::init()
{
	player_mgr_ = new PlayerManager();
	if (-1 == player_mgr_->init())
	{
		return -1;
	}
	item_mgr_ = new ItemManager();
	if (-1 == item_mgr_->init())
	{
		return -1;
	}
	mission_mgr_ = new MissionManager();
	if (-1 == mission_mgr_->init())
	{
		return -1;
	}
	role_mgr_ = new RoleManager();
	if (-1 == role_mgr_->init())
	{
		return -1;
	}
	equip_mgr_ = new EquipManager();
	if (-1 == equip_mgr_->init())
	{
		return -1;
	}
	post_mgr_ = new PostManager();
	if (-1 == post_mgr_->init())
	{
		return -1;
	}
	sport_mgr_ = new SportManager();
	if (-1 == sport_mgr_->init())
	{
		return -1;
	}
	boss_mgr_ = new BossManager();
	if (-1 == boss_mgr_->init())
	{
		return -1;
	}
	social_mgr_ = new SocialManager();
	if (-1 == social_mgr_->init())
	{
		return -1;
	}
	guild_mgr_ = new GuildManager();
	if (-1 == guild_mgr_->init())
	{
		return -1;
	}
	rank_mgr_ = new RankManager();
	if (-1 == rank_mgr_->init())
	{
		return -1;
	}
	huodong_mgr_ = new HuodongManager();
	if (-1 == huodong_mgr_->init())
	{
		return -1;
	}
	treasure_mgr_ = new TreasureManager();
	if (-1 == treasure_mgr_->init())
	{
		return -1;
	}
	pvp_mrg_ = new PvpManager();
	if (-1 == pvp_mrg_->init())
	{
		return -1;
	}
	return 0;
}

int GsManager::fini()
{
	treasure_mgr_->fini();
	delete treasure_mgr_;
	huodong_mgr_->fini();
	delete huodong_mgr_;
	rank_mgr_->fini();
	delete rank_mgr_;
	guild_mgr_->fini();
	delete guild_mgr_;
	social_mgr_->fini();
	delete social_mgr_;
	boss_mgr_->fini();
	delete boss_mgr_;
	sport_mgr_->fini();
	delete sport_mgr_;
	post_mgr_->fini();
	delete post_mgr_;
	equip_mgr_->fini();
	delete equip_mgr_;
	role_mgr_->fini();
	delete role_mgr_;
	mission_mgr_->fini();
	delete mission_mgr_;
	item_mgr_->fini();
	delete item_mgr_;
	player_mgr_->fini();
	delete player_mgr_;
	pvp_mrg_->fini();
	delete pvp_mrg_;

	return 0;
}
