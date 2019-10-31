#ifndef __PLAYER_LOAD_H__
#define __PLAYER_LOAD_H__

#include "gameinc.h"

struct QueryMap
{
	int query_num;
	std::string name;
	int id;
	std::vector<int> params;
	std::vector<std::string> msgs;
};

class PlayerLoad
{
public:
	int load_player(uint64_t player_guid, const std::string &name, int id);

	int load_player(uint64_t player_guid, int param, const std::string &msg);

	dhc::player_t * create_player(uint64_t player_guid, const std::string &serverid);

protected:
	void load_player_callback(Request *req, bool is_create);

	void load_player_check_end(dhc::player_t *player);

	void load_msg_callback(Request *req, dhc::player_t *player);

private:
	std::map<uint64_t, QueryMap> query_map_;
};

#define sPlayerLoad (Singleton<PlayerLoad>::instance())

#endif
