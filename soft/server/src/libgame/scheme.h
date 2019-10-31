#ifndef __SCHEME_H__
#define __SCHEME_H__

#include "game_interface.h"
#include "dbc.h"

class DFA;

class Scheme : public mmg::Scheme
{
public:
	Scheme();

	~Scheme();

	int init();

	int fini();

	virtual DBCFile * get_dbc(const std::string &name, bool reset = false);

	virtual int read_file(const std::string &name, std::string &res, bool reset = false);

	virtual int search_illword(const std::string& text,bool can_space, bool can_return);

	virtual void change_illword(std::string& text);

	virtual int get_server_str(int lang, std::string &str, const char * sstr, ...);

	virtual std::string get_lang_str(int lang, const std::string& str);

	virtual bool valid_name(const std::string& text);

private:
	DFA *dfa_;
	std::map<std::string, DBCFile *> dbcfiles_;
	std::map<std::string, std::string> filep_;
	std::map<std::string, std::vector<std::string> > server_strs_;
	std::map<std::string, std::vector<std::string> > lang_strs_;
};

#endif

