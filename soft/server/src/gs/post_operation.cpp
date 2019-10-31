#include "post_operation.h"
#include "post_pool.h"

dhc::post_t * PostOperation::post_create(uint64_t player_guid, const std::string &title, const std::string &text,
	const std::string &sender_name, const std::vector<s_t_reward> &rewards)
{
	uint64_t post_guid = game::gtool()->assign(et_post);
	dhc::post_t *post = new dhc::post_t();
	post->set_guid(post_guid);
	post->set_receiver_guid(player_guid);
	post->set_sender_date(game::timer()->now());
	post->set_title(title);
	post->set_text(text);
	post->set_sender_name(sender_name);
	for (int i = 0; i < rewards.size(); ++i)
	{
		post->add_type(rewards[i].type);
		post->add_value1(rewards[i].value1);
		post->add_value2(rewards[i].value2);
		post->add_value3(rewards[i].value3);
	}
	sPostPool->add_post(post, true);

	return post;
}
