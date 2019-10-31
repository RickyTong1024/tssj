#ifndef __GTOOL_H__
#define __GTOOL_H__

#include "game_interface.h"

namespace mysqlpp
{
	class Connection;
}

class Dhc;

class GTool : public mmg::GTool
{
public:
	GTool();

	~GTool();

	int init(const std::string &name);

	int fini();

	virtual uint64_t assign (int et);

	virtual uint64_t start_time();

	virtual int random_start_day();

protected:
	int read_guid();

	int write_guid();

	int insert_guid();

	void get_start_time();

	void get_random_day();

private:
	mysqlpp::Connection *conn_;
	int qid_;
	int one_add_;
	uint64_t cur_guid_;
	uint64_t next_guid_;
	uint64_t start_time_;
	Dhc *dhc_;
	int mysql_;
	int start_random_day_;
};

#endif
