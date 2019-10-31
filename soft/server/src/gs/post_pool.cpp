#include "post_pool.h"

#define RELOAD_PERIOD 60000
#define DELETE_TIME 3 * 86400 * 1000
#define POST_UPDATE_TIME 60000
#define POST_MAX_SIZE 15

PostPool::PostPool()
{
	last_reload_time_ = game::timer()->now();
}

PostPool::~PostPool()
{

}

void PostPool::post_list_load()
{
	uint64_t post_list_guid = MAKE_GUID(et_post_list, 0);
	Request *req = new Request();
	req->add(opc_query, post_list_guid, new protocol::game::post_list_t);
	game::pool()->upcall(req, boost::bind(&PostPool::post_list_load_callback, this, _1, post_list_guid));
}

void PostPool::post_list_load_new()
{
	uint64_t post_list_guid = MAKE_GUID(et_post_list, 1);
	Request *req = new Request();
	req->add(opc_query, post_list_guid, new protocol::game::post_list_t);
	game::pool()->upcall(req, boost::bind(&PostPool::post_list_load_callback, this, _1, post_list_guid));
}

int PostPool::post_list_load_callback(Request *req, uint64_t post_list_guid)
{
	if (req->success())
	{
		protocol::game::post_list_t *post_list  = (protocol::game::post_list_t *)req->data();
		bool is_load_new = (GUID_COUNTER(post_list_guid) == 1);
		for(int i = 0; i < post_list->post_record_size(); ++i)
		{
			dhc::post_t* tpost = post_list->mutable_post_record(i);
			if(tpost)
			{
				dhc::post_t* post = new dhc::post_t;
				post->CopyFrom(*tpost);
				bool flag = false;
				if(post->guid() == 0)
				{
					flag = true;
					uint64_t new_guid = game::gtool()->assign(et_post);
					post->set_guid(new_guid);
				}
				add_post(post);
				if (flag)
				{
					Request *req = new Request();
					dhc::post_t *post1 = new dhc::post_t();
					post1->CopyFrom(*post);
					req->add(opc_update, post->guid(), post1);
					game::pool()->upcall(req, 0);
				}
			}
		}
	}
	return 0;
}

bool PostPool::get_player_post_list(uint64_t player_guid, std::vector<dhc::post_t*>& posts)
{
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		return 0;
	}
	if(player_posts_set_.find(player_guid) != player_posts_set_.end())
	{
		posts.clear();
		std::set<uint64_t>& post_set = player_posts_set_[player_guid];
		for(std::set<uint64_t>::iterator it = post_set.begin(); it != post_set.end(); it++)
		{
			dhc::post_t* post = POOL_GET_POST(*it);
			if (post)
			{
				posts.push_back(post);
			}
		}
	}
	return 0;
}

void PostPool::add_post(dhc::post_t *post, bool is_new)
{
	/// check post
	std::set<uint64_t>& post_set = player_posts_set_[post->receiver_guid()];
	if (post_set.size() >= POST_MAX_SIZE)
	{
		uint64_t delete_post_guid = *(post_set.begin());
		post_set.erase(delete_post_guid);
		POOL_REMOVE(delete_post_guid, 0);
	}

	/// add post
	UpdatePost up;
	up.guid = post->guid();
	up.update_time = game::timer()->now();
	update_list_.push_back(up);
	post_set.insert(post->guid());
	POOL_ADD(post->guid(), post);
	if (is_new)
	{
		Request *req = new Request();
		dhc::post_t *post1 = new dhc::post_t();
		post1->CopyFrom(*post);
		req->add(opc_insert, post1->guid(), post1);
		game::pool()->upcall(req, 0);
	}
}

void PostPool::delete_post(dhc::post_t *post)
{
	std::set<uint64_t>& post_set = player_posts_set_[post->receiver_guid()];
	post_set.erase(post->guid());
	POOL_REMOVE(post->guid(), 0);
}

bool PostPool::check_post_delete(dhc::post_t * post)
{
	bool flag = false;
	uint64_t nt = game::timer()->now();
	if (nt > post->sender_date() + DELETE_TIME)
	{
		delete_post(post);
		return true;
	}
	return false;
}

int PostPool::update(ACE_Time_Value tv)
{
	/// Êý¾Ý±£´æ
	uint64_t time = game::timer()->now();
	if (time - last_reload_time_ >= RELOAD_PERIOD)
	{
		post_list_load_new();
		last_reload_time_ = time;
	}

	int upnum = 0;
	int save_num = 0;
	while (upnum < 1000 && save_num < 100)
	{
		if (update_list_.empty())
		{
			break;
		}
		UpdatePost up = update_list_.front();
		dhc::post_t *post = POOL_GET_POST(up.guid);
		if (post)
		{
			upnum++;
			if (time - up.update_time < POST_UPDATE_TIME)
			{
				break;
			}
			if (!check_post_delete(post))
			{
				update_list_.pop_front();
				up.update_time = time;
				update_list_.push_back(up);
			}
			else
			{
				save_num++;
			}
		}
		else
		{
			update_list_.pop_front();
		}
	}

	return 0;
}
