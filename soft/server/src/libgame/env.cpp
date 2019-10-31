#include "env.h"
#include <boost/property_tree/ptree.hpp>
#include <boost/property_tree/json_parser.hpp>

Env::Env()
{
	
}

Env::~Env()
{

}

int Env::init(const std::string &confpath)
{
	boost::property_tree::ptree pt;
	boost::property_tree::read_json(confpath + "/server.json", pt);

	{
		boost::property_tree::ptree pt_master = pt.get_child("master");
		std::string host = pt_master.get<std::string>("host");
		master_value_["host"] = host;
		std::string port = pt_master.get<std::string>("port");
		master_value_["port"] = port;
		master_value_["endport"] = "";
		master_value_["mysql"] = "0";
	}

	{
		boost::property_tree::ptree pt_servers = pt.get_child("servers");
		for (boost::property_tree::ptree::iterator it = pt_servers.begin(); it != pt_servers.end(); ++it)
		{
			std::string sname = (*it).first;
			server_kinds_.push_back(sname);
			boost::property_tree::ptree pt_ch = (*it).second;
			for (boost::property_tree::ptree::iterator jt = pt_ch.begin(); jt != pt_ch.end(); ++jt)
			{
				boost::property_tree::ptree pt_ch_ch = (*jt).second;
				std::string id = pt_ch_ch.get<std::string>("id");
				server_names_[sname].push_back(id);
				server_value_[id]["id"] = id;
				std::string host = pt_ch_ch.get<std::string>("host");
				server_value_[id]["host"] = host;
				std::string port = pt_ch_ch.get<std::string>("port");
				server_value_[id]["port"] = port;
				try
				{
					std::string endport = pt_ch_ch.get<std::string>("endport");
					server_value_[id]["endport"] = endport;
				}
				catch (...)
				{
					server_value_[id]["endport"] = "";
				}
				try
				{
					std::string mysql = pt_ch_ch.get<std::string>("mysql");
					server_value_[id]["mysql"] = mysql;
				}
				catch (...)
				{
					server_value_[id]["mysql"] = "0";
				}
			}
		}
	}

	{
		boost::property_tree::ptree pt_others = pt.get_child("other");
		for (boost::property_tree::ptree::iterator it = pt_others.begin(); it != pt_others.end(); ++it)
		{
			std::string sname = (*it).first;
			server_kinds_.push_back(sname);
			boost::property_tree::ptree pt_ch = (*it).second;
			for (boost::property_tree::ptree::iterator jt = pt_ch.begin(); jt != pt_ch.end(); ++jt)
			{
				boost::property_tree::ptree pt_ch_ch = (*jt).second;
				std::string id = pt_ch_ch.get<std::string>("id");
				other_value_[id]["id"] = id;
				std::string host = pt_ch_ch.get<std::string>("host");
				other_value_[id]["host"] = host;
				std::string port = pt_ch_ch.get<std::string>("port");
				other_value_[id]["port"] = port;
			}
		}
	}

	{
		boost::property_tree::ptree pt_db = pt.get_child("db");
		std::string host = pt_db.get<std::string>("host");
		db_value_["host"] = host;
		std::string username = pt_db.get<std::string>("username");
		db_value_["username"] = username;
		std::string password = pt_db.get<std::string>("password");
		db_value_["password"] = password;
		std::string db = pt_db.get<std::string>("db");
		db_value_["db"] = db;
		std::string port = pt_db.get<std::string>("port");
		db_value_["port"] = port;
	}

	boost::property_tree::ptree pt1;
	boost::property_tree::read_json(confpath + "/game.json", pt1);

	{
		std::string qid = pt1.get<std::string>("qid");
		game_value_["qid"] = qid;
		std::string one_add = pt1.get<std::string>("one_add");
		game_value_["one_add"] = one_add;
		std::string tick = pt1.get<std::string>("tick");
		game_value_["tick"] = tick;
		std::string conf_path = pt1.get<std::string>("conf_path");
		game_value_["conf_path"] = conf_path;
		std::string proto_path = pt1.get<std::string>("proto_path");
		game_value_["proto_path"] = proto_path;
		std::string debug = pt1.get<std::string>("debug");
		game_value_["debug"] = debug;

		boost::property_tree::ptree pt_server_state = pt1.get_child("state");
		server_state_["normal"] = pt_server_state.get<std::string>("normal");
		server_state_["busy"] = pt_server_state.get<std::string>("busy");

		std::string mlog = pt1.get<std::string>("log");
		game_value_["log"] = mlog;
		std::string lang = pt1.get<std::string>("lang");
		game_value_["lang"] = lang;
	}

	return 0;
}

int Env::fini()
{
	return 0;
}

std::string Env::get_master_value(const std::string &key)
{
	if (master_value_.find(key) != master_value_.end())
	{
		return master_value_[key];
	}
	return "";
}

std::string Env::get_id_value(const std::string &name, const std::string &key)
{
	if (name == "master")
	{
		if (master_value_.find(key) != master_value_.end())
		{
			return master_value_[key];
		}
	}
	else
	{
		if (server_value_.find(name) != server_value_.end())
		{
			if (server_value_[name].find(key) != server_value_[name].end())
			{
				return server_value_[name][key];
			}
		}

		if (other_value_.find(name) != other_value_.end())
		{
			if (other_value_[name].find(key) != other_value_[name].end())
			{
				return other_value_[name][key];
			}
		}
	}
	return "";
}

void Env::get_server_names(const std::string &kind, std::vector<std::string> &names)
{
	if (server_names_.find(kind) != server_names_.end())
	{
		names = server_names_[kind];
	}
}

void Env::get_server_kinds(std::vector<std::string> &kinds)
{
	kinds = server_kinds_;
}

std::string Env::get_db_value(const std::string &key)
{
	if (db_value_.find(key) != db_value_.end())
	{
		return db_value_[key];
	}
	return "";
}

std::string Env::get_game_value(const std::string &key)
{
	if (game_value_.find(key) != game_value_.end())
	{
		return game_value_[key];
	}
	return "";
}

std::string Env::get_server_state(const std::string &state)
{
	if (server_state_.find(state) != server_state_.end())
	{
		return server_state_[state];
	}

	return "";
}
