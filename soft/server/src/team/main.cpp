#include "game.h"
#include <ace/ACE.h>
#include "team_service.h"

int ACE_TMAIN(int argc, ACE_TCHAR *argv[])
{
	if (argc < 3)
	{
		return 0;
	}
	TeamService tm;
	if (-1 == game::init(argv[1], argv[2], &tm))
	{
		return -1;
	}
	tm.init();
	game::run();
	tm.fini();
	game::fini();

	return 0;
}
