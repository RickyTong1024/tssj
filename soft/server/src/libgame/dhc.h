#ifndef __DHC_H__
#define __DHC_H__

#include <google/protobuf/message.h>
#include "typedefs.h"

class Request;

namespace mysqlpp
{
	class Connection;
}

struct Itstr
{
	int disc;
	std::string type;
	std::string name;
};

struct Indexstr
{
	std::string index_name;
	int index_len;
};

struct Files
{
	std::string cname;
	std::vector<Itstr> itstr_vec;
	bool use_datetime;
	bool use_auto_increment;
	std::vector<Indexstr> uniques;
	std::vector<Indexstr> indexes;

	bool operator == (const std::string &name)
	{
		return cname == name;
	}

	bool operator == (const Files & rhs)
	{
		return cname == rhs.cname;
	}

	bool operator != (const Files & rhs)
	{
		return !(operator == (rhs));
	}

	bool operator != (const std::string &name)
	{
		return !(operator == (name));
	}
};

class Dhc
{
public:
	Dhc();

	~Dhc();

	int init();

	int fini();

	int do_request(Request *req);

	int do_check();

protected:
	int do_insert(uint64_t guid, google::protobuf::Message *data);

	int do_query(uint64_t guid, google::protobuf::Message *data);

	int do_update(uint64_t guid, google::protobuf::Message *data);

	int do_remove(uint64_t guid, google::protobuf::Message *data);

private:
	mysqlpp::Connection *conn_;
};

#endif
