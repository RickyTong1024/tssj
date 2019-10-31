#ifndef __NAME_H__
#define __NAME_H__

#include "gameinc.h"

namespace mysqlpp
{
	class Connection;
}

struct NameStr;

class Name
{
public:
	Name();

	~Name();

	int init();

	int fini();

	int do_name(NameStr *name);

private:
	mysqlpp::Connection *conn_;
};

#endif
