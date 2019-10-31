#include "log.h"
#include "game.h"
#include "ace/Log_Msg.h"
#include <boost/date_time/posix_time/posix_time.hpp>
#include <boost/format.hpp>

Log::Log()
: log_level_(0)
{

}

Log::~Log()
{

}

int Log::init(const std::string &name)
{
	name_ = name;
	std::string ll = game::env()->get_game_value("log");
	if (ll == "debug")
	{
		log_level_ = 0;
	}
	else
	{
		log_level_ = 1;
	}
	
	return 0;
}

int Log::fini()
{
	return 0;
}

int Log::debug(const char *text, ...)
{
	if (log_level_ > 0)
	{
		return -1;
	}

	char c[1000];
	va_list args;
	va_start(args, text);
	vsprintf(c, text, args);
	va_end(args);
	std::string t = name_ + " " + get_date() + " debug: " + std::string(c) + "\n";
	printf(t.c_str());
	return 0;
}

int Log::error(const char *text, ...)
{
	char c[1000];
	va_list args;
	va_start(args, text);
	vsprintf(c, text, args);
	va_end(args);
	std::string t = name_ + " " + get_date() + " error: " + std::string(c) + "\n";
	printf(t.c_str());
	return 0;
}

void Log::log(const std::string &username, const std::string &server_id, uint64_t player_guid, int type, int value1, int value2, int value3, int value4, const std::string &platform)
{
	//std::string str = (boost::format("%1%|%2%|%3%|%4%|%5%|%6%|%7%|%8%|%9%\n") % username % server_id % player_guid % type % value1 % value2 % value3 % value4 % platform).str();
	//ACE_DEBUG((LM_INFO, str.c_str()));
}

std::string Log::get_date()
{
	std::string tm = boost::posix_time::to_iso_extended_string(boost::posix_time::second_clock::local_time());

	int pos = tm.find('T');
	tm.replace(pos, 1, std::string("-"));

	return tm;
}
