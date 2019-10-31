#ifndef __POST_POOL_HPP__
#define __POST_POOL_HPP__

#include "gameinc.h"

struct UpdatePost
{
	uint64_t guid;
	uint64_t update_time;
};

class PostPool
{
public:
	PostPool();

	~PostPool();	

	void post_list_load();

	void post_list_load_new();

	int post_list_load_callback(Request *req, uint64_t post_list_guid);

	bool get_player_post_list(uint64_t player_guid, std::vector<dhc::post_t*>& posts);

	void add_post(dhc::post_t *post, bool is_new = false);

	void delete_post(dhc::post_t *post);

	bool check_post_delete(dhc::post_t * post);

	int update(ACE_Time_Value tv);

private:
	std::list<UpdatePost> update_list_;
	uint64_t last_reload_time_;
	std::map< uint64_t, std::set<uint64_t> > player_posts_set_;
};

#define sPostPool (Singleton<PostPool>::instance())

#endif
