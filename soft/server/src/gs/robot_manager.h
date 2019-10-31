#include "gameinc.h"


struct s_rebot
{
	uint64_t guid;
	int type;
};

class RobotManager
{
public:
	RobotManager();

	~RobotManager();

	int init();

	int fini();


	void robot_login(); // 登录

	void rob_timer();   // 随机定时器

	void rob_behavior(dhc::player_t *player, const std::string &serverid, int type);	// 机器人行为


private:


};