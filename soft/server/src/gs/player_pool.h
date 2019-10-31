#ifndef __PLAYER_POOL_H__
#define __PLAYER_POOL_H__

#include "gameinc.h"
#include <deque>

struct SPlayer
{
	uint64_t guid;
	int off_time;
};

struct UpdatePlayer
{
	uint64_t guid;
	uint64_t update_time;
};

class PlayerPool
{
public:
	PlayerPool();

	~PlayerPool();

	void add_player(uint64_t guid, bool is_login);

	void update();

	void save_player(uint64_t guid, bool release);

	void save_all();

	void add_name_player(const std::string &name, uint64_t player_guid);

	void del_name_player(const std::string &name);

	uint64_t get_name_player(const std::string &name);

protected:
	template <typename T>
	void save_sub(T *msg, bool release);

private:
	std::list<UpdatePlayer> update_list_;
	uint64_t last_time_;
	uint64_t add_time_;
	std::map<std::string, uint64_t> name_player_;
};

#define sPlayerPool (Singleton<PlayerPool>::instance())

#endif
