#include "game.h"
#include <ace/ACE.h>
#include "chat_manager.h"

int ACE_TMAIN(int argc, ACE_TCHAR *argv[])
{
	if (argc < 3)
	{
		return 0;
	}
	ChatManager chat;
	if (-1 == game::init(argv[1], argv[2], &chat))
	{
		return -1;
	}
	chat.init();
	game::run();
	chat.fini();
	game::fini();

	return 0;
}
