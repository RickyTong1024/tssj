#include "robot_manager.h"
#include "robot_operation.h"
#include "gs_message.h"
#include <boost/algorithm/string.hpp>
#include "player_manager.h"
#include "player_load.h"
#include "role_config.h"
#include "utils.h"
#include "player_operation.h"
#include "player_config.h"


RobotManager::RobotManager()
{

}

RobotManager::~RobotManager()
{

}

int RobotManager::init()
{
	
	return 0;
}

int RobotManager::fini()
{

	return 0;
}




void RobotManager::rob_timer()
{
	return;
}

void RobotManager::rob_behavior(dhc::player_t *player, const std::string &serverid, int type)
{
//	uint64_t guid = player->guid;
	uint64_t guid = 0;
	PlayerOperation::player_login(player);
	PlayerOperation::client_login(player);
	ResMessage::res_client_login(player, 0, "", 0);
	PlayerOperation::player_refresh_check(player);

	if (type == 1)		// ≥‰÷µ
	{
		PlayerOperation::player_recharge(player, 60, 0);
		PlayerOperation::add_player_chongzhi(guid, 60, 0);
	}
	else if (type == 2)
	{

	}
	else if (type == 3)
	{

	}



	PlayerOperation::player_logout(player);
}