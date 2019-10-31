#ifndef __COMMUNICATION_H__
#define __COMMUNICATION_H__

#include "packet.h"
#include "game_interface.h"
#include <ace/Thread_Mutex.h>

class GameService : public mmg::GameService
{
public:
	GameService();

	~GameService();

	int init(mmg::GameInterface *gif);

	int fini();

	virtual int add_msg(Packet *pck);

protected:
	int update(const ACE_Time_Value &cur);

	int fetch(Packet*& pck);

	void on_filter(Packet* pck);

private:
	int timer_;
	int tick_;
	Packet *head_;
	Packet *tail_;
	Packet *header_;
	Packet *next_;
	ACE_Thread_Mutex chain_;
	mmg::GameInterface *gif_;
};

#endif
