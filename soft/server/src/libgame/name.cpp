#include "name.h"
#include <mysql++.h>
#include "game_interface.h"

Name::Name()
: conn_(0)
{

}

Name::~Name()
{

}

int Name::init()
{
	conn_ = new mysqlpp::Connection;
	conn_->disable_exceptions();
	conn_->set_option(new mysqlpp::ReconnectOption(true));
	conn_->set_option(new mysqlpp::SetCharsetNameOption("utf8"));
	int port = boost::lexical_cast<int>(game::env()->get_db_value("port"));
	int ok = conn_->connect(game::env()->get_db_value("db").c_str(), game::env()->get_db_value("host").c_str(), game::env()->get_db_value("username").c_str(), game::env()->get_db_value("password").c_str(), port);
	if (!ok)
	{
		printf("Failed to connect database.\n");
		return -1;
	}

	return 0;
}

int Name::fini()
{
	conn_->disconnect();
	delete conn_;
	conn_ = 0;
	return 0;
}

int Name::do_name(NameStr *name)
{
	conn_->ping();
	if (name->guid.size() == 0)
	{
		mysqlpp::Query query = conn_->query();
		query << "SELECT guid FROM player_t WHERE "
			<< "name=" << mysqlpp::quote << name->name;

		mysqlpp::StoreQueryResult res = query.store();

		if (!res)
		{
			return -1;
		}

		for (int i = 0; i < res.num_rows(); ++i)
		{
			name->guid.push_back(res.at(i).at(0));
		}
		name->suc = true;
	}
	else if (name->name == "")
	{
		mysqlpp::Query query = conn_->query();
		query << "SELECT name FROM player_t WHERE "
			<< "guid=" << boost::lexical_cast<std::string>(name->guid[0]);

		mysqlpp::StoreQueryResult res = query.store();

		if (!res || res.num_rows() != 1)
		{
			return -1;
		}

		name->name = (std::string)res.at(0).at(0);
		name->suc = true;
	}

	return 0;
}
