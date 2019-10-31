#include "rank_manager.h"
#include "gs_message.h"
#include "rank_operation.h"

#define RANK_PERIOD 300000

RankManager::RankManager()
: timer_(-1)
{

}

RankManager::~RankManager()
{
}

int RankManager::init()
{
	sRankPool->init();

	timer_ = game::timer()->schedule(boost::bind(&RankPool::update, sRankPool, _1), RANK_PERIOD, "rank");

	return 0;
}

int RankManager::fini()
{
	if (timer_ != -1)
	{
		game::timer()->cancel(timer_);
		timer_ = -1;
	}
	sRankPool->fini();
	return 0;
}

void RankManager::terminal_view_rank(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_rank_view msg;
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

	int type = msg.type();
	if (type < e_rank_type_level || type >= e_rank_type_end)
	{
		GLOBAL_ERROR;
		return;
	}
	uint64_t guid = MAKE_GUID(et_rank, type);
	dhc::rank_t *rank = POOL_GET_RANK(guid);
	if (!rank)
	{
		GLOBAL_ERROR;
		return;
	}

	ResMessage::res_rank_data(player, rank, name, id);
}

void RankManager::terminal_view_huiyi_rank(const std::string &data, const std::string &name, int id)
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

	dhc::rank_t *huiyi_rank = POOL_GET_RANK(MAKE_GUID(et_rank, e_rank_huiyi));
	if (!huiyi_rank)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_role_huiyi_rank smsg;
	int rank = RankOperation::get_huiyi_rank(player, huiyi_rank, smsg.mutable_rank_list());
	smsg.set_rank(rank);

	ResMessage::res_role_huiyi_rank(player, smsg, name, id);
}
