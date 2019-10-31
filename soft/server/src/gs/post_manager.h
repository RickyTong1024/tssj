#ifndef __POST_MANAGER_H__
#define __POST_MANAGER_H__

#include "gameinc.h"

class PostManager
{
public:
	PostManager();

	~PostManager();

	int init();

	int fini();

	void terminal_post_look(const std::string &data, const std::string &name, int id);

	void terminal_post_get(const std::string &data, const std::string &name, int id);

private:
	int timer_;
};

#endif
