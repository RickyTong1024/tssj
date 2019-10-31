#include "game.h"
#include "gs_manager.h"

int ACE_TMAIN(int argc, ACE_TCHAR *argv[])
{
	if (argc < 3)
	{
		return 0;
	}
	GsManager gm;
	if (-1 == game::init(argv[1], argv[2], &gm))
	{
		return -1;
	}
	gm.init();
	game::run();
	gm.fini();
	game::fini();
	return 0;
}
