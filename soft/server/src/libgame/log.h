#ifndef __LOG_H__
#define __LOG_H__

#include "game_interface.h"

class Log : public mmg::Log
{
public:
	Log();

	~Log();

	int init(const std::string &name);

	int fini();

	virtual int debug(const char *text, ...);

	virtual int error(const char *text, ...);

	virtual void log(const std::string &username,
		const std::string &server_id,
		uint64_t player_guid,
		int type,
		int value1,
		int value2,
		int value3,
		int value4,
		const std::string &platform);

protected:
	std::string get_date();
	
private:
	int log_level_;
	std::string name_;
};

#endif
