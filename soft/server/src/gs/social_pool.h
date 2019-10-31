#ifndef __SOCIAL_POOL_HPP__
#define __SOCIAL_POOL_HPP__

#include "gameinc.h"

class SocialPool
{
public:
	SocialPool();

	~SocialPool();

	void social_list_load();

	int social_list_load_callback(Request *req);

	void add_update(uint64_t social_guid);

	void save_all();

	void update();

	void add_social(dhc::social_t *social, bool is_new = false);

	void delete_social(uint64_t player_guid, uint64_t target_guid);

	void get_player_social(uint64_t player_guid, std::set<uint64_t> &player_social);

	void get_player_besocial(uint64_t player_guid, std::set<uint64_t> &player_besocial);

	dhc::social_t *get_another(dhc::social_t *social);

	void add_apply(dhc::player_t *player, uint64_t target_guid);

	void delete_apply(uint64_t player_guid, uint64_t target_guid);

	void get_player_apply(uint64_t player_guid, std::set< std::pair<uint64_t, uint64_t> > &player_apply);

	void get_player_beapply(uint64_t player_guid, std::set<protocol::game::msg_social_player *> &player_beapply);

	const std::set<uint64_t>* get_friends(uint64_t player_guid) const;

	void update_social_code(dhc::player_t *player, int level, bool check);

	dhc::social_t *get_social_code(dhc::player_t *player);

private:
	void add_social_code(dhc::social_t *social);

	void update_social_code(dhc::player_t *player, dhc::social_t *social);

	void update_social_code_callback(const std::string &data, uint64_t social_guid);

private:
	std::list<uint64_t> update_list_;
	std::set<uint64_t> update_set_;
	std::map< uint64_t, std::set<uint64_t> > player_social_list_;
	std::map< uint64_t, std::set<uint64_t> > player_besocial_list_;
	std::map< uint64_t, std::set< std::pair<uint64_t, uint64_t> > > player_apply_list_;
	std::map< uint64_t, std::set<protocol::game::msg_social_player *> > player_beapply_list_;
	std::list<uint64_t> apply_update_list_;

	std::map<uint64_t, std::set<uint64_t> > friends_;

	std::map<uint64_t, uint64_t> player_social_code_invites_;
	std::map<std::string, uint64_t> name_social_code_invites_;
};

#define sSocialPool (Singleton<SocialPool>::instance())

#endif
