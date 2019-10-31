#ifndef __SOCIAL_OPERATION_H__
#define __SOCIAL_OPERATION_H__

#include "gameinc.h"

#define MAX_SHOU_NUM 30

class SocialOperation
{
public:
	static void gundong(const std::string &text);
	static void gundong_server(const std::string &text, const std::string &arg1, const std::string &arg2, const std::string &arg3);

	static void gundong_ex(const std::string &text);

	static void check_gundong();

	static void refresh(dhc::player_t *player);

	static void refresh_name_nalflag(dhc::player_t *player, const std::string& name, const int& nalflag);

	static dhc::social_t * create_social(uint64_t player_guid, uint64_t target_guid, const std::string &name, int id, int level, int bf, int vip, int achieve, int nalfalg);

	static int get_is_friend_apply(dhc::player_t *player);

	static int get_is_friend_tili(dhc::player_t *player);

	static void add_chat(protocol::game::smsg_chat *smsg);

	static void get_chat(std::list<protocol::game::smsg_chat *> &smsg);

	static void add_chat(uint64_t guild_guid, protocol::game::smsg_chat *smsg);

	static void get_chat(uint64_t guild_guid, std::list<protocol::game::smsg_chat *> &smsg);

	static void  get_friends(uint64_t player_guid, std::set<uint64_t>& friends);

	static void update_social_code(dhc::player_t *player, int level, bool check = false);

	static int get_social_code_pcount(dhc::player_t *player, int level);

	static void add_social_code_id(dhc::player_t *player, int id);
};

#endif
