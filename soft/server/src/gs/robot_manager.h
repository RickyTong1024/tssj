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


	void robot_login(); // ��¼

	void rob_timer();   // �����ʱ��

	void rob_behavior(dhc::player_t *player, const std::string &serverid, int type);	// ��������Ϊ


private:


};