#include "game.h"
#include <vector>
#include "game_interface.h"
#include <ace/Process_Manager.h>
#include <ace/Signal.h>

int ACE_TMAIN(int argc, ACE_TCHAR *argv[])
{
	if (argc < 2)
	{
		return 0;
	}
	std::string name = "";
	if (argc >= 3)
	{
		name = argv[2];
	}
	if (-1 == game::init("master", argv[1], 0))
	{
		return -1;
	}

	std::vector<ACE_Process *> processes;
	std::vector<std::string> kinds;
	game::env()->get_server_kinds(kinds);
	for (int i = 0; i < kinds.size(); ++i)
	{
		std::vector<std::string> names;
		game::env()->get_server_names(kinds[i], names);
		for (int j = 0; j < names.size(); ++j)
		{
			ACE_Process_Options options;
			std::string cmd = "./" + kinds[i] + " " + names[j] + " " + argv[1] + " " + name;
			options.command_line(cmd.c_str());
			ACE_Process *process = new ACE_Process();
			process->spawn(options);
			processes.push_back(process);
		}
	}

	game::run();

	for (int i = 0; i < processes.size(); ++i)
	{
		processes[i]->kill();
	}

	for (int i = 0; i < processes.size(); ++i)
	{
		processes[i]->wait();
		delete processes[i];
	}

	game::fini();

	return 0;
}
