#include "post_manager.h"
#include "post_pool.h"
#include "gs_message.h"
#include "player_operation.h"

#define POST_PERIOD 1000

PostManager::PostManager()
: timer_(0)
{

}

PostManager::~PostManager()
{

}

int PostManager::init()
{
	sPostPool->post_list_load();
	timer_ = game::timer()->schedule(boost::bind(&PostPool::update, sPostPool, _1), POST_PERIOD, "post");
	return 0;
}

int PostManager::fini()
{
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = 0;
	}
	return 0;
}

void PostManager::terminal_post_look(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_common msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	std::vector<dhc::post_t *> posts;
	sPostPool->get_player_post_list(player->guid(), posts);
	ResMessage::res_post_look(player, posts, name, id);
}

void PostManager::terminal_post_get(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_post_get msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		PERROR(ERROR_SIG);
		return;
	}
	PCK_CHECK;

	dhc::post_t *post = POOL_GET_POST(msg.post_guid());
	if (!post)
	{
		GLOBAL_ERROR;
		return;
	}

	if (post->receiver_guid() != player->guid())
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	for (int i = 0; i < post->type_size(); ++i)
	{
		rds.add_reward(post->type(i), post->value1(i), post->value2(i), post->value3(i));
	}
	PlayerOperation::player_add_reward(player, rds, LOGWAY_POST);

	sPostPool->delete_post(post);
	ResMessage::res_post_get(player, rds, name, id);
}
