#ifndef __ENV_H__
#define __ENV_H__

#include "game_interface.h"
#include <map>
#include <set>

class Env : public mmg::Env
{
public:
	Env();

	~Env();

	int init(const std::string &confpath);

	int fini();

	virtual std::string get_master_value(const std::string &key);

	virtual std::string get_id_value(const std::string &name, const std::string &key);

	virtual void get_server_names(const std::string &kind, std::vector<std::string> &names);

	virtual void get_server_kinds(std::vector<std::string> &kinds);

	virtual std::string get_db_value(const std::string &key);

	virtual std::string get_game_value(const std::string &key);

	virtual std::string get_server_state(const std::string &state);

private:
	std::map<std::string, std::string> master_value_;
	std::map< std::string, std::map<std::string, std::string> > server_value_;
	std::map< std::string, std::vector<std::string> > server_names_;
	std::vector<std::string> server_kinds_;
	std::map< std::string, std::map<std::string, std::string> > other_value_;
	std::map<std::string, std::string> db_value_;
	std::map<std::string, std::string> game_value_;
	std::map<std::string, std::string> server_state_;
};

#endif
