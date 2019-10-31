#ifndef _RANK_MANAGER_H_
#define _RANK_MANAGER_H_

#include "gameinc.h"
#include "rank_pool.h"

class RankManager
{
public:
	RankManager();

	~RankManager();

	int init();

	int fini();

public:
	void terminal_view_rank(const std::string &data, const std::string &name, int id);

	void terminal_view_huiyi_rank(const std::string &data, const std::string &name, int id);

private:
	int timer_;
};
#endif
