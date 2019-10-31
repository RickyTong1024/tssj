#ifndef __SOCIAL_MANAGER_H__
#define __SOCIAL_MANAGER_H__

#include "gameinc.h"

class SocialManager
{
public:
	SocialManager();

	~SocialManager();

	int init();

	int fini();

	int update(const ACE_Time_Value &curr);

	void terminal_chat(const std::string &data, const std::string &name, int id);

	void terminal_social_look(const std::string &data, const std::string &name, int id);

	void terminal_social_rand(const std::string &data, const std::string &name, int id);

	void terminal_social_add(const std::string &data, const std::string &name, int id);

	void terminal_social_look_new(const std::string &data, const std::string &name, int id);

	void terminal_social_agree(const std::string &data, const std::string &name, int id);

	void terminal_social_delete(const std::string &data, const std::string &name, int id);

	void terminal_social_song(const std::string &data, const std::string &name, int id);

	void terminal_social_shou(const std::string &data, const std::string &name, int id);

	void terminal_social_invite_look(const std::string &data, const std::string &name, int id);

	void terminal_social_code_look(const std::string &data, const std::string &name, int id);

	void terminal_social_code_look_callback(const std::string &data, uint64_t player_guid, const std::string &name, int id);

private:
	int timer_;
	std::map<uint64_t, uint64_t> chat_time_map_;
	std::list<uint64_t> chat_time_list_;
};

#endif
