#include "scheme.h"
#include "dfa_algorith.h"
#include <ace/OS_NS_stdio.h>
#include <boost/algorithm/string.hpp>
#include "game.h"
#include <fstream>
#include <boost/lexical_cast.hpp>

Scheme::Scheme()
: dfa_(0)
{

}

Scheme::~Scheme()
{

}

int Scheme::init()
{
	DBCFile *dbfile = get_dbc("t_lang.txt");
	if (!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); i++)
	{
		std::string key = dbfile->Get(i, 0)->pString;
		for (int j = 0; j < 3; ++j)
		{
			lang_strs_[key].push_back(dbfile->Get(i, j + 1)->pString);
		}
	}

	dfa_ = new DFA();
	if (-1 == dfa_->init_ex())
	{
		return -1;
	}

	dbfile = get_dbc("t_server_language.txt");
	if(!dbfile)
	{
		return -1;
	}

	for (int i = 0; i < dbfile->GetRecordsNum(); i++)
	{
		std::string key = dbfile->Get(i, 0)->pString;
		std::string value = dbfile->Get(i, 1)->pString;
		if (lang_strs_.find(value) != lang_strs_.end())
		{
			server_strs_[key] = lang_strs_[value];
		}
	}

	return 0;
}

int Scheme::fini()
{
	delete dfa_;
	dfa_ = 0;
	
	return 0;
}

DBCFile * Scheme::get_dbc(const std::string &name, bool reset)
{
	DBCFile *old_dbc = 0;
	if (dbcfiles_.find(name) != dbcfiles_.end())
	{
		old_dbc = dbcfiles_[name];
		if (!reset)
			return old_dbc;
	}

	std::string content;
	if (read_file(name, content, reset) == -1)
	{
		printf ("read resource error. name: <%s>\n", name.c_str());
		return old_dbc;
	}

	DBCFile *dbc = new DBCFile(0);

	if (dbc->OpenFromMemory (content.c_str (), content.c_str () + content.size () + 1))
	{
		dbcfiles_[name] = dbc;
		if (old_dbc)
			delete old_dbc;
		return dbc;
	}
	else
	{
		delete dbc;
		printf ("parse dbc file error. name: <%s>\n", name.c_str());
		return old_dbc;
	}
	return old_dbc;
}

int Scheme::read_file(const std::string &name, std::string &res, bool reset)
{
	std::string path = game::env()->get_game_value("conf_path");
	if (filep_.find(name) != filep_.end())
	{
		if (!reset)
		{
			res = filep_[name];
			return 0;
		}
	}

	std::string fname = path + "/" + name;
	std::ifstream ifs(fname.c_str());
	if (!ifs.is_open())
	{
		return -1;
	}
	res.assign((std::istreambuf_iterator<char>(ifs.rdbuf())), std::istreambuf_iterator<char>());
	ifs.close();
	filep_[name] = res;

	return 0;
}

int Scheme::search_illword(const std::string& text,bool can_space, bool can_return)
{
	if (dfa_->search(text) == -1)
	{
		return -1;
	}
	bool flag = false;
	for (int i = 0; i < text.size(); ++i)	// ÊÇ·ñÎª¿Õ×Ö·û
	{
		if (can_return && text.substr(i, 1) == "\n")
		{
			flag = true;
			break;
		}
		else if (i == 0 && text.substr(i, 1) == " " && !can_space)
		{
			flag = true;
			break;
		}
		else if (i == text.size() - 1 && text.substr(i, 1) == " " && !can_space)
		{
			flag = true;
			break;
		}
	}
	if (flag)
	{
		return -1;
	}
	return 0;
}

void Scheme::change_illword(std::string& text)
{
	dfa_->change(text);
}

int Scheme::get_server_str(int lang, std::string &str, const char * sstr, ...)
{
	if (lang == -1)
	{
		lang = 2;
	}

	std::string s(sstr);
	if (server_strs_.find(s) != server_strs_.end())
	{
		std::vector<std::string>& stringvec = server_strs_[s];
		if (lang < 0 || lang >= stringvec.size())
		{
			return -1;
		}

		str = stringvec[lang];

		char c[1000];
		va_list args;
		va_start(args, sstr);
		vsprintf(c, str.c_str(), args);
		va_end(args);

		str = std::string(c);
		return 0;
	}
	return -1;
}

std::string Scheme::get_lang_str(int lang, const std::string& str)
{
	if (lang_strs_.find(str) == lang_strs_.end())
	{
		return "";
	}
	std::vector<std::string>& stringvec = lang_strs_[str];
	if (lang < 0 || lang >= stringvec.size())
	{
		return "";
	}
	return lang_strs_[str][lang];
}

bool Scheme::valid_name(const std::string& text)
{
	return true;
}
