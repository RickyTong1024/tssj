#ifndef __POST_OPERATION_H__
#define __POST_OPERATION_H__

#include "gameinc.h"

class PostOperation
{
public:
	static dhc::post_t * post_create(uint64_t player_guid, const std::string &title, const std::string &text,
		const std::string &sender_name, const std::vector<s_t_reward> &rewards);
};

#endif
