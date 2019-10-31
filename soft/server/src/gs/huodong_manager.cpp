#include "huodong_manager.h"
#include "utils.h"
#include "huodong_config.h"
#include "player_operation.h"
#include "role_operation.h"
#include "role_config.h"
#include "equip_operation.h"
#include "item_operation.h"
#include "social_operation.h"
#include "rank_operation.h"
#include "post_operation.h"
#include "huodong_operation.h"
#include "global_pool.h"
#include "huodong_pool.h"
#include "sport_config.h"
#include "item_config.h"
#include "player_config.h"
#include "gs_message.h"
#include "player_load.h"
#include "mission_fight.h"
#include "player_pool.h"
#include "rpc.pb.h"

#define HUODONG_PERIOD 5000	
#define GLOBAL_PERIOD  60000

#define ADD_EXTRA_HP(huodong_player, index) \
	huodong_player->add_extra_data##index(-1)

#define SET_EXTRA_HP(huodong_player, index, suo, hp) \
	huodong_player->set_extra_data##index(suo, hp)

#define PLAYER_ADD_HUODONG_REWARD(player, huodong_entry, rds, type1) \
	for (int i = 0; i < huodong_entry->types_size(); ++i) \
	{ \
		rds.add_reward(huodong_entry->types(i), huodong_entry->value1s(i), huodong_entry->value2s(i), huodong_entry->value3s(i)); \
	} \
	PlayerOperation::player_add_reward(player, rds, type1);

HuodongManager::HuodongManager()
{

}

HuodongManager::~HuodongManager()
{

}

int HuodongManager::init()
{
	if (-1 == sHuodongConfig->parse())
	{
		return -1;
	}
	if (sGlobalPool->init() == -1)
	{
		return -1;
	}
	if (sHuodongPool->init() == -1)
	{
		return -1;
	}

	timer_ = game::timer()->schedule(boost::bind(&HuodongManager::update, this, _1), HUODONG_PERIOD, "huodong");

	global_timer_ = game::timer()->schedule(boost::bind(&GlobalPool::update, sGlobalPool, _1), GLOBAL_PERIOD, "global");

	old_time_ = game::timer()->now();

	return 0;
}

int HuodongManager::fini()
{
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = 0;
	}
	if (global_timer_)
	{
		game::timer()->cancel(global_timer_);
	}

	sHuodongPool->fini();
	sGlobalPool->fini();

	return 0;
}

int HuodongManager::update(const ACE_Time_Value& tv)
{
	/// 刷新普天同庆
	if (game::timer()->trigger_time(game::gtool()->start_time() + game::gtool()->random_start_day() * 86400000, 0, 0))
	{
		sGlobalPool->update_random_pttq(sHuodongConfig->get_t_huodong_pttq_min_id());
	}

	/// 刷新精彩活动
	sHuodongPool->update();

	return 0;
}

void HuodongManager::terminal_huodong_view(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_huodong_view smsg;
	sHuodongPool->huodong_list_view(player, smsg, HUODONG_HUODONG_VIEW);

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (global)
	{
		smsg.set_czjh_count(global->czjh_count());
	}
	ResMessage::res_huodong_view(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_jiri(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_huodong_jiri_view smsg;
	sHuodongPool->huodong_view(player, HUODONG_JRHD_TYPE, smsg);
	ResMessage::res_huodong_jiri(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_xingheqidian(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_huodong_view smsg;
	sHuodongPool->huodong_list_view(player, smsg, HUODONG_XINGHEQIDIAN_VIEW);
	ResMessage::res_huodong_view(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_add(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_activity_group msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	int error_code = sHuodongPool->huodong_add(msg);
	game::rpc_service()->response(name, id, "", error_code, "");
}

void HuodongManager::terminal_huodong_pttq(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_pttq msg;
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

	if (!sGlobalPool->is_enable_pttq(msg.id()))
	{
		GLOBAL_ERROR;
		return;
	}

	int index = msg.id() * 100  + msg.vip();
	if (HuodongOperation::get_huodong_pttq_index(player, index) != -1)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_huodong_pttq * t_huodong_pttq = sHuodongConfig->get_t_huodong_pttq(msg.id());
	if (!t_huodong_pttq)
	{
		GLOBAL_ERROR;
		return;
	}
	std::map<int, std::vector<s_t_reward> >::const_iterator rewardIter = t_huodong_pttq->rewards.find(msg.vip());
	if (rewardIter == t_huodong_pttq->rewards.end())
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->vip() < rewardIter->first)
	{
		GLOBAL_ERROR;
		return;
	}
	
	player->add_huodong_pttq_reward(index);

	s_t_rewards rds;
	rds.add_reward(rewardIter->second);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_PTTQ);
	ResMessage::res_huodong_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_PTTQ, msg.id());
}

void HuodongManager::terminal_huodong_pttq_view(const std::string &data, const std::string &name, int id)
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

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		GLOBAL_ERROR;
		return;
	}

	ResMessage::res_global_view(player, global, name, id);
}

void HuodongManager::terminal_huodong_kaifu_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_kaifu_look msg;
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

	int day = game::timer()->run_day(player->birth_time()) + 1;
	int start_day = 1;
	if (msg.stype() == 0)
	{
		if (day > 8)
		{
			GLOBAL_ERROR;
			return;
		}
		if (day == 8)
		{
			day = 7;
		}
	}
	else
	{
		if (day < 8 || day > 15)
		{
			GLOBAL_ERROR;
			return;
		}
		start_day = 8;
	}
	

	std::vector<int> huodong_ids;
	std::vector<int> huodong_counts;
	for (int i = start_day; i <= day; ++i)
	{
		std::vector<int> ids;
		sHuodongConfig->get_t_huodong_kf(i, ids);
		for (int j = 0; j < ids.size(); ++j)
		{
			huodong_ids.push_back(ids[j]);
			huodong_counts.push_back(HuodongOperation::get_kaifu_huodong_count(player, ids[j]));
		}
	}

	ResMessage::res_huodong_kaifu_look(player, huodong_ids, huodong_counts, name, id);
}

void HuodongManager::terminal_huodong_kaifu(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_kaifu_reward msg;
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

	const s_t_huodong_kf_mubiao *t_mubiao = sHuodongConfig->get_t_huodong_kf_mubiao(msg.id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_mubiao->type == 300)
	{
		if (msg.num() <= 0 || msg.num() > t_mubiao->def4)
		{
			GLOBAL_ERROR;
			return;
		}
		if (HuodongOperation::get_kaifu_huodong_id_count(player, msg.id()) + msg.num() > t_mubiao->def4)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else
	{
		if (HuodongOperation::get_kaifu_huodong_id(player, msg.id()) != -1)
		{
			GLOBAL_ERROR;
			return;
		}
	}

	int day = game::timer()->run_day(player->birth_time()) + 1;
	bool has_id = false;
	for (int i = 1; i <= day; ++i)
	{
		std::vector<int> ids;
		sHuodongConfig->get_t_huodong_kf(i, ids);
		for (int j = 0; j < ids.size(); ++j)
		{
			if (ids[j] == msg.id())
			{
				has_id = true;
				break;
			}
		}
		if (has_id)
		{
			break;
		}
	}
	if (!has_id)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	int count = HuodongOperation::get_kaifu_huodong_count(player, msg.id());

	/// 半价购买
	if (t_mubiao->type == 200)
	{
		const s_t_huodong_kf *t_kaifu = sHuodongConfig->get_t_huodong_kf(msg.id());
		if (!t_kaifu)
		{
			GLOBAL_ERROR;
			return;
		}
		//if (count >= t_kaifu->count)
		//{
		//	GLOBAL_ERROR;
		//	return;
		//}

		if (player->jewel() < t_kaifu->jewel)
		{
			PERROR(ERROR_JEWEL);
			return;
		}

		sGlobalPool->update_kaifu_xg_count(msg.id());
		PlayerOperation::player_dec_resource(player, resource::JEWEL, t_kaifu->jewel, LOGWAY_KAIFU_BUY);
		
		rds.add_reward(t_kaifu->type, t_kaifu->value1, t_kaifu->value2, t_kaifu->value3);
		PlayerOperation::player_add_reward(player, rds, LOGWAY_KAIFU_BUY);
	}
	/// 四选一
	else if (t_mubiao->type == 100)
	{
		if (t_mubiao->def3 == 1)
		{
			if (msg.index() < 0 || msg.index() >= t_mubiao->rewards.size())
			{
				GLOBAL_ERROR;
				return;
			}
			rds.add_reward(t_mubiao->rewards[msg.index()].type,
				t_mubiao->rewards[msg.index()].value1,
				t_mubiao->rewards[msg.index()].value2,
				t_mubiao->rewards[msg.index()].value3);
		}
		else
		{
			rds.add_reward(t_mubiao->rewards);
		}
		
		PlayerOperation::player_add_reward(player, rds, LOGWAY_KAIFU_REWARD);
	}
	/// 折扣贩售
	else if (t_mubiao->type == 300)
	{
		if (player->jewel() < t_mubiao->def1 * msg.num())
		{
			PERROR(ERROR_JEWEL);
			return;
		}
		if (t_mubiao->def3 == 1)
		{
			if (msg.index() < 0 || msg.index() >= t_mubiao->rewards.size())
			{
				GLOBAL_ERROR;
				return;
			}
			for (int kg = 0; kg < msg.num(); ++kg)
			{
				rds.add_reward(t_mubiao->rewards[msg.index()].type,
					t_mubiao->rewards[msg.index()].value1,
					t_mubiao->rewards[msg.index()].value2,
					t_mubiao->rewards[msg.index()].value3);
			}
			
		}
		else
		{
			for (int kg = 0; kg < msg.num(); ++kg)
			{
				rds.add_reward(t_mubiao->rewards);
			}
		}
		PlayerOperation::player_dec_resource(player, resource::JEWEL, t_mubiao->def1 * msg.num(), LOGWAY_KAIFU_REWARD);
		PlayerOperation::player_add_reward(player, rds, LOGWAY_KAIFU_REWARD);
	}
	else
	{
		if (count < t_mubiao->cankao)
		{
			GLOBAL_ERROR;
			return;
		}

		if (t_mubiao->def3 == 1)
		{
			if (msg.index() < 0 || msg.index() >= t_mubiao->rewards.size())
			{
				GLOBAL_ERROR;
				return;
			}
			rds.add_reward(t_mubiao->rewards[msg.index()].type,
				t_mubiao->rewards[msg.index()].value1,
				t_mubiao->rewards[msg.index()].value2,
				t_mubiao->rewards[msg.index()].value3);
		}
		else
		{
			rds.add_reward(t_mubiao->rewards);
		}
		PlayerOperation::player_add_reward(player, rds, LOGWAY_KAIFU_REWARD);
	}

	if (t_mubiao->type == 300)
	{
		for (int kg = 0; kg < msg.num(); ++kg)
		{
			player->add_huodong_kaifu_finish_ids(msg.id());
		}
	}
	else
	{
		player->add_huodong_kaifu_finish_ids(msg.id());
	}
	
	ResMessage::res_huodong_kaifu_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_KAIFU_REWARD, msg.id());
}

void HuodongManager::terminal_huodong_czjh_buy(const std::string &data, const std::string &name, int id)
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

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->huodong_czjh_buy() == 1)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->vip() < 2)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < gCONST(CONST_PLAN))
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, gCONST(CONST_PLAN), LOGWAY_CZJH_BUY);
	player->set_huodong_czjh_buy(1);
	global->set_czjh_count(global->czjh_count() + 1);

	ResMessage::res_success(player, true, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_CZJH_BUY, 1);
}

void HuodongManager::terminal_huodong_czjh_get(const std::string &data, const std::string &name, int id)
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

	if (player->huodong_czjh_buy() == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_huodong_czjh * t_huodong_czjh = sHuodongConfig->get_t_huodong_czjh(player->huodong_czjh_index() + 1);
	if (!t_huodong_czjh)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->level() < t_huodong_czjh->level)
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_huodong_czjh_index(player->huodong_czjh_index() + 1);
	PlayerOperation::player_add_resource(player, resource::JEWEL, t_huodong_czjh->jewel, LOGWAY_CZJH);

	ResMessage::res_success(player, true, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_CZJH, player->huodong_czjh_index());
}

void HuodongManager::terminal_huodong_czjhrs(const std::string &data, const std::string &name, int id)
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

	dhc::global_t *global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
	if (!global)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_huodong_czjhrs * t_huodong_czjh = sHuodongConfig->get_t_huodong_czjhrs(player->huodong_czjh_index1() + 1);
	if (!t_huodong_czjh)
	{
		GLOBAL_ERROR;
		return;
	}
	if (global->czjh_count() < t_huodong_czjh->count)
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_huodong_czjh_index1(player->huodong_czjh_index1() + 1);
	s_t_rewards rds;
	rds.add_reward(t_huodong_czjh->rd);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_CZJHRS);

	ResMessage::res_huodong_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_CZJHRS, player->huodong_czjh_index1());
}

void HuodongManager::terminal_huodong_vip_libao(const std::string &data, const std::string &name, int id)
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

	if (player->huodong_vip_libao() != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_huodong_vip_libao* t_vip_libao = sHuodongConfig->get_t_huodong_vip_libao(player->vip());
	if (!t_vip_libao)
	{
		GLOBAL_ERROR;
		return;
	}

	player->set_huodong_vip_libao(1);
	s_t_rewards rds;
	rds.add_reward(t_vip_libao->rewards);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_VIP_LIBAO_DAILY);
	ResMessage::res_huodong_reward(player, rds, name, id);
}

void HuodongManager::terminal_huodong_week_libao(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward msg;
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

	const s_t_huodong_week_libao* t_week_libao = sHuodongConfig->get_t_huodong_week_libao(msg.id());
	if (!t_week_libao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < t_week_libao->jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	if (player->level() < t_week_libao->level1 ||
		player->level() > t_week_libao->level2)
	{
		GLOBAL_ERROR;
		return;
	}

	if (HuodongOperation::get_huodong_week_libao(player, msg.id()) >= t_week_libao->num)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_week_libao->jewel, LOGWAY_WEEK_LIBAO);
	player->add_huodong_week_libao(msg.id());
	s_t_rewards rds;
	rds.add_reward(t_week_libao->rds);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_WEEK_LIBAO);
	ResMessage::res_huodong_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_WEEK_LIBAO, msg.id());
}


void HuodongManager::terminal_huodong_ljcz_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward_view msg;
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

	protocol::game::smsg_huodong_reward_view msg1;
	sHuodongPool->huodong_view(player, msg.id(), HUODONG_CZFL_TYPE, msg1);
	ResMessage::res_huodong_reward_view(player, msg1, name, id);
}

void HuodongManager::terminal_huodong_ljcz(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward msg;
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

	dhc::huodong_entry_t *huodong_entry = sHuodongPool->get_huodong_entry(msg.huodong(), msg.id(), HUODONG_CZFL_TYPE);
	if (!huodong_entry)
	{
		GLOBAL_ERROR;
		return;
	}

	HuodongArgResult arg_result;
	sHuodongPool->get_huodong_arg(player, huodong_entry, arg_result);
	/// 已领过
	if (arg_result.second > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	/// 充值金额不足
	if (arg_result.first < huodong_entry->arg1())
	{
		GLOBAL_ERROR;
		return;
	}
	sHuodongPool->set_huodong_arg(player, huodong_entry, HUODONG_ARG_COMPLETE, 1);
	s_t_rewards rds;
	PLAYER_ADD_HUODONG_REWARD(player, huodong_entry, rds, LOGWAY_HUODONG_CZFL);
	ResMessage::res_huodong_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_HUODONG_CZFL, msg.id());
}


void HuodongManager::terminal_huodong_dbcz_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward_view msg;
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

	protocol::game::smsg_huodong_reward_view msg1;
	sHuodongPool->huodong_view(player, msg.id(), HUODONG_DBCZ_TYPE, msg1);
	ResMessage::res_huodong_reward_view(player, msg1, name, id);
}

void HuodongManager::terminal_huodong_dbcz(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward msg;
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

	dhc::huodong_entry_t *huodong_entry = sHuodongPool->get_huodong_entry(msg.huodong(), msg.id(), HUODONG_DBCZ_TYPE);
	if (!huodong_entry)
	{
		GLOBAL_ERROR;
		return;
	}
	HuodongArgResult arg_result;
	sHuodongPool->get_huodong_arg(player, huodong_entry, arg_result);
	if (arg_result.second >= arg_result.first)
	{
		GLOBAL_ERROR;
		return;
	}
	if (arg_result.second >= huodong_entry->arg2())
	{
		GLOBAL_ERROR;
		return;
	}

	sHuodongPool->set_huodong_arg(player, huodong_entry, HUODONG_ARG_COMPLETE, 1);
	s_t_rewards rds;
	PLAYER_ADD_HUODONG_REWARD(player, huodong_entry, rds, LOGWAY_HUODONG_DBCZ);
	ResMessage::res_huodong_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_HUODONG_DBCZ, msg.id());
}

void HuodongManager::terminal_huodong_dlsl_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward_view msg;
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

	protocol::game::smsg_huodong_reward_view msg1;
	sHuodongPool->huodong_view(player, msg.id(), HUODONG_DLSL_TYPE, msg1);
	ResMessage::res_huodong_reward_view(player, msg1, name, id);
}

void HuodongManager::terminal_huodong_dlsl(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward msg;
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

	dhc::huodong_entry_t *huodong_entry = sHuodongPool->get_huodong_entry(msg.huodong(), msg.id(), HUODONG_DLSL_TYPE);
	if (!huodong_entry)
	{
		GLOBAL_ERROR;
		return;
	}
	HuodongArgResult arg_result;
	sHuodongPool->get_huodong_arg(player, huodong_entry, arg_result);
	if (arg_result.second > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (arg_result.first < huodong_entry->arg1())
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	if (huodong_entry->arg2() == 1)
	{
		if (msg.index() < 0 || msg.index() >= huodong_entry->types_size())
		{
			GLOBAL_ERROR;
			return;
		}
		rds.add_reward(huodong_entry->types(msg.index()), huodong_entry->value1s(msg.index()),
			huodong_entry->value2s(msg.index()),
			huodong_entry->value3s(msg.index()));
		PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_DLSL);
	}
	else
	{
		PLAYER_ADD_HUODONG_REWARD(player, huodong_entry, rds, LOGWAY_HUODONG_DLSL);
	}
	sHuodongPool->set_huodong_arg(player, huodong_entry, HUODONG_ARG_COMPLETE, 1);
	ResMessage::res_huodong_reward(player, rds, name, id);
	PlayerOperation::player_huodong(player, LOGWAY_HUODONG_DLSL, msg.id());

	/*for (int i = 0; i < rds.rewards.size(); ++i)
	{
		if (rds.rewards[i].value1 == 50060001)
		{
			std::string sender;
			game::scheme()->get_server_str(sender, "sys_sender");
			std::string title;
			game::scheme()->get_server_str(title, "dengludongli_title_title");
			std::string text;
			game::scheme()->get_server_str(text, "dengludongli_title_text", player->name().c_str());
			std::vector<s_t_reward> pstds;
			PostOperation::post_create(player->guid(), title, text, sender, pstds);
			break;
		}
	}*/
}

void HuodongManager::terminal_huodong_zkfs_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward_view msg;
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

	protocol::game::smsg_huodong_reward_view msg1;
	sHuodongPool->huodong_view(player, msg.id(), HUODONG_ZKFS_TYPE, msg1);
	ResMessage::res_huodong_reward_view(player, msg1, name, id);
}

void HuodongManager::terminal_huodong_zkfs(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward msg;
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

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		return;
	}

	dhc::huodong_entry_t *huodong_entry = sHuodongPool->get_huodong_entry(msg.huodong(), msg.id(), HUODONG_ZKFS_TYPE);
	if (!huodong_entry)
	{
		GLOBAL_ERROR;
		return;
	}
	HuodongArgResult arg_result;
	sHuodongPool->get_huodong_arg(player, huodong_entry, arg_result);
	if (arg_result.second >= huodong_entry->arg2())
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.num() + arg_result.second > huodong_entry->arg2())
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < huodong_entry->arg1() * msg.num())
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	s_t_rewards rds;
	if (huodong_entry->arg3() == 1)
	{
		if (msg.index() < 0 || msg.index() >= huodong_entry->types_size())
		{
			GLOBAL_ERROR;
			return;
		}
		for (int i = 0; i < msg.num(); ++i)
		{
			rds.add_reward(huodong_entry->types(msg.index()), huodong_entry->value1s(msg.index()),
				huodong_entry->value2s(msg.index()),
				huodong_entry->value3s(msg.index()));
		}
		rds.merge();
		PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_ZKFS);
	}
	else
	{
		for (int j = 0; j < msg.num(); ++j)
		{
			for (int i = 0; i < huodong_entry->types_size(); ++i)
			{
				rds.add_reward(huodong_entry->types(i), huodong_entry->value1s(i), huodong_entry->value2s(i), huodong_entry->value3s(i));
			}
		}
		rds.merge();
		PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_ZKFS);
	}
	sHuodongPool->set_huodong_arg(player, huodong_entry, HUODONG_ARG_COMPLETE, msg.num());
	PlayerOperation::player_dec_resource(player, resource::JEWEL, huodong_entry->arg1() * msg.num(), LOGWAY_HUODONG_ZKFS);
	ResMessage::res_huodong_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_HUODONG_ZKFS, msg.id());
}

void HuodongManager::terminal_huodong_djdh_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward_view msg;
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

	protocol::game::smsg_huodong_reward_view msg1;
	sHuodongPool->huodong_view(player, msg.id(), HUODONG_DHDJ_TYPE, msg1);
	ResMessage::res_huodong_reward_view(player, msg1, name, id);
}

void HuodongManager::terminal_huodong_djdh(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward msg;
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

	if (msg.index() < 1 || msg.index() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::huodong_entry_t *huodong_entry = sHuodongPool->get_huodong_entry(msg.huodong(), msg.id(), HUODONG_DHDJ_TYPE);
	if (!huodong_entry)
	{
		GLOBAL_ERROR;
		return;
	}
	HuodongArgResult arg_result;
	sHuodongPool->get_huodong_arg(player, huodong_entry, arg_result);
	/// 兑换次数
	if (arg_result.second >= huodong_entry->arg1())
	{
		GLOBAL_ERROR;
		return;
	}
	/// 兑换道具个数
	for (int i = 0; i < huodong_entry->arg6_size(); ++i)
	{
		if (ItemOperation::item_num_templete(player, huodong_entry->arg7(i)) < huodong_entry->arg8(i) * msg.index())
		{
			PERROR(ERROR_CAILIAO);
			return;
		}
	}
	s_t_rewards rds;
	for (int i = 0; i < msg.index(); ++i)
	{
		for (int j = 0; j < huodong_entry->types_size(); ++j)
		{
			rds.add_reward(huodong_entry->types(j), huodong_entry->value1s(j), huodong_entry->value2s(j));
		}
	}
	rds.merge();
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_DJDH);
	sHuodongPool->set_huodong_arg(player, huodong_entry, HUODONG_ARG_COMPLETE, msg.index());
	for (int i = 0; i < huodong_entry->arg6_size(); ++i)
	{
		ItemOperation::item_destory_templete(player, huodong_entry->arg7(i), huodong_entry->arg8(i) * msg.index(), LOGWAY_HUODONG_DJDH);
	}
	ResMessage::res_huodong_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_HUODONG_DJDH, msg.id());
}

void HuodongManager::terminal_huodong_hyhd_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward_view msg;
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

	protocol::game::smsg_huodong_reward_view msg1;
	sHuodongPool->huodong_view(player, msg.id(), HUODONG_HYLX_TYPE, msg1);
	ResMessage::res_huodong_reward_view(player, msg1, name, id);
}

void HuodongManager::terminal_huodong_hyhd(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward msg;
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

	dhc::huodong_entry_t *huodong_entry = sHuodongPool->get_huodong_entry(msg.huodong(), msg.id(), HUODONG_HYLX_TYPE);
	if (!huodong_entry)
	{
		GLOBAL_ERROR;
		return;
	}

	HuodongArgResult arg_result;
	sHuodongPool->get_huodong_arg(player, huodong_entry, arg_result);
	/// 已领取
	if (arg_result.second > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	/// 条件未达成
	if (arg_result.first < huodong_entry->arg1())
	{
		PERROR(ERROR_SPORT_REWARD);
		return;
	}

	sHuodongPool->set_huodong_arg(player, huodong_entry, HUODONG_ARG_COMPLETE, 1);
	s_t_rewards rds;
	PLAYER_ADD_HUODONG_REWARD(player, huodong_entry, rds, LOGWAY_HUODONG_HYHD);
	ResMessage::res_huodong_reward(player, rds, name, id);

	PlayerOperation::player_huodong(player, LOGWAY_HUODONG_HYHD, msg.id());
}


void HuodongManager::terminal_huodong_rqdl_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward_view msg;
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

	protocol::game::smsg_huodong_reward_view msg1;
	sHuodongPool->huodong_view(player, msg.id(), HUODONG_RQDL_TYPE, msg1);
	ResMessage::res_huodong_reward_view(player, msg1, name, id);
}

void HuodongManager::terminal_huodong_rqdl(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward msg;
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

	dhc::huodong_entry_t *huodong_entry = sHuodongPool->get_huodong_entry(msg.huodong(), msg.id(), HUODONG_RQDL_TYPE);
	if (!huodong_entry)
	{
		GLOBAL_ERROR;
		return;
	}
	HuodongArgResult arg_result;
	sHuodongPool->get_huodong_arg(player, huodong_entry, arg_result);
	/// 已领取
	if (arg_result.second > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	/// 未登录
	if (arg_result.first <= 0)
	{
		GLOBAL_ERROR;
		return;
	}
	s_t_rewards rds;
	if (huodong_entry->arg4() == 1)
	{
		if (msg.index() < 0 || msg.index() >= huodong_entry->types_size())
		{
			GLOBAL_ERROR;
			return;
		}
		rds.add_reward(huodong_entry->types(msg.index()), huodong_entry->value1s(msg.index()),
			huodong_entry->value2s(msg.index()),
			huodong_entry->value3s(msg.index()));
		PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_RQDL);
	}
	else
	{
		PLAYER_ADD_HUODONG_REWARD(player, huodong_entry, rds, LOGWAY_HUODONG_RQDL);
	}
	sHuodongPool->set_huodong_arg(player, huodong_entry, HUODONG_ARG_COMPLETE, 1);
	ResMessage::res_huodong_reward(player, rds, name, id);
	PlayerOperation::player_huodong(player, LOGWAY_HUODONG_RQDL, msg.id());
}


void HuodongManager::terminal_huodong_tanbao_view(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_huodong_tanbao_view smsg;
	sHuodongPool->huodong_view_tanbao(player, smsg);
	ResMessage::res_huodong_tanbao_view(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_tanbao_dice(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_tanbao_dice msg;
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


	dhc::huodong_player_t* huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TBHD_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	int jewel = 0;
	int num = 1;
	int init_move = 0;
	if (msg.type() == 1)
	{
		num = 10;
	}
	else if (msg.type() == 2)
	{
		if (huodong_player->arg6() > 0 && 
			msg.dice() > 0 &&
			msg.dice() < 7)
		{
			init_move = msg.dice();
		}
		else
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (msg.type() == 3)
	{
		if (huodong_player->arg6() > 0 &&
			msg.dice() > 0 &&
			msg.dice() < 7)
		{
			init_move = msg.dice();
			num = huodong_player->arg6();
			if (num > 10)
			{
				num = 10;
			}
		}
		else
		{
			GLOBAL_ERROR;
			return;
		}
	}

	int dnum = huodong_player->arg2();
	if (dnum > 0)
	{
		if (num == 1)
		{
			dnum = 1;
		}
	}
	int avail_num = num - dnum;
	if (init_move == 0)
	{
		if (avail_num > 0)
		{
			for (int i = 0; i < avail_num; ++i)
			{
				const s_t_price* t_price = sPlayerConfig->get_price(huodong_player->arg1() + 1 + i);
				if (!t_price)
				{
					GLOBAL_ERROR;
					return;
				}
				jewel += t_price->tanbao;
			}
		}
	}
	else
	{
		dnum = 0;
		avail_num = 0;
	}
	
	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	int gezi = huodong_player->arg4();
	int shop_type = 0;
	std::map<int, int> counts;
	protocol::game::smsg_tanbao_dice smsg;
	for (int i = 0; i < num; ++i)
	{
		get_tanbao(init_move, (init_move ? true : false), gezi, smsg);
		if (msg.type() != 3)
		{
			init_move = 0;
		}
	}
	
	const s_t_tanbao *t_tanbao = 0;
	s_t_rewards rds;
	int luck_num = 0;
	int point = 0;
	for (int i = 0; i < smsg.dianshu_size(); ++i)
	{
		if (smsg.yidong(i) != 1)
		{
			point += smsg.dianshu(i);
		}

		t_tanbao = sHuodongConfig->get_t_tanbao(smsg.gezi(i));
		if (t_tanbao)
		{
			if (t_tanbao->type == 1)
			{
				rds.add_reward(t_tanbao->events[0].type,
					t_tanbao->events[0].value1,
					t_tanbao->events[0].value2,
					t_tanbao->events[0].value3);
			}
			else if (t_tanbao->type == 2)
			{
				if (smsg.item(i) > 0)
				{
					rds.add_reward(2,
						t_tanbao->events[0].value1,
						smsg.item(i));
				}
			}
			else if (t_tanbao->type == 3)
			{
				shop_type = shop_type | (1 << t_tanbao->shop_type);
				if (counts.find(t_tanbao->shop_type) != counts.end())
				{
					counts[t_tanbao->shop_type] += 1;
				}
				else
				{
					counts[t_tanbao->shop_type] = 1;
				}

			}
			else if (t_tanbao->type == 4)
			{
				if (smsg.events(i) != -1)
				{
					rds.add_reward(t_tanbao->events[smsg.events(i)].type,
						t_tanbao->events[smsg.events(i)].value1,
						t_tanbao->events[smsg.events(i)].value2,
						t_tanbao->events[smsg.events(i)].value3);
				}
			}
			else if (t_tanbao->type == 5)
			{
				if (smsg.gold(i) > 0)
				{
					rds.add_reward(1,
						resource::GOLD,
						smsg.gold(i));
				}
			}
			else if (t_tanbao->type == 6)
			{
				luck_num++;
			}
		}
	}

	if (jewel > 0)
	{
		PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_HUODONG_TANBAO_DICE);
	}
	if (avail_num > 0)
	{
		huodong_player->set_arg1(huodong_player->arg1() + avail_num);
	}
	if (dnum > 0)
	{
		huodong_player->set_arg2(huodong_player->arg2() - dnum);
		if (huodong_player->arg2() < 0)
			huodong_player->set_arg2(0);
	}
	huodong_player->set_arg3(huodong_player->arg3() + smsg.pass_num());
	huodong_player->set_arg4(gezi);
	smsg.set_final_gezi(gezi);
	huodong_player->set_arg5(huodong_player->arg5() + point);
	huodong_player->set_arg6(huodong_player->arg6() + luck_num);
	if (msg.type() == 2)
	{
		huodong_player->set_arg6(huodong_player->arg6() - 1);
	}
	else if (msg.type() == 3)
	{
		huodong_player->set_arg6(huodong_player->arg6() - num);
	}
	huodong_player->set_arg7(shop_type);
	huodong_player->clear_args4();
	huodong_player->clear_args5();
	for (std::map<int, int>::const_iterator it = counts.begin();
		it != counts.end();
		++it)
	{
		huodong_player->add_args4(it->first);
		huodong_player->add_args5(it->second);
	}
	if (huodong_player->arg5() >= 5888)
	{
		RankOperation::check_value(player, e_rank_tanbao_jy, huodong_player->arg5());
	}
	RankOperation::check_value(player, e_rank_tanbao_normal, huodong_player->arg5());

	huodong_player->clear_args1();
	huodong_player->clear_args2();
	
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_TANBAO_DICE);
	sHuodongPool->save_huodong_player(huodong_player->guid());
	sHuodongPool->huodong_active(player, HUODONG_COND_TANBAO_COUNT, point);
	ResMessage::res_huodong_tanbao_dice(player, smsg, name, id);
	PlayerOperation::player_huodong(player, LOGWAY_HUODONG_TANBAO_DICE, msg.type());
}

void HuodongManager::get_tanbao(int move, bool luck, int &gezi, protocol::game::smsg_tanbao_dice &smsg)
{
	int dianshu = Utils::get_int32(1,6);
	if (move != 0)
	{
		dianshu = move;
	}
	gezi += dianshu;
	if (gezi > 18)
	{
		gezi = gezi % 18;
		smsg.set_pass_num(smsg.pass_num() + 1);
	}
	smsg.add_dianshu(dianshu);
	smsg.add_gezi(gezi);
	smsg.add_events(-1);
	smsg.add_gold(0);
	smsg.add_item(0);
	if (luck)
	{
		smsg.add_yidong(2);
	}
	else
	{
		smsg.add_yidong(((move != 0) ? 1 : 0));
	}
	
	int index = smsg.dianshu_size() - 1;
	const s_t_tanbao *t_tanbao = sHuodongConfig->get_t_tanbao(gezi);
	if (t_tanbao)
	{
		if (t_tanbao->type == 2)
		{
			if (t_tanbao->events[0].value3 == 0)
			{
				smsg.set_item(index, t_tanbao->events[0].value2);
			}
			else
			{
				smsg.set_item(index, Utils::get_int32(t_tanbao->events[0].value2, t_tanbao->events[0].value3));
			}
		}
		else if (t_tanbao->type == 4)
		{
			smsg.set_events(index, sHuodongConfig->get_t_tanbao_event(t_tanbao));
		}
		else if (t_tanbao->type == 5)
		{
			smsg.set_gold(index, Utils::get_int32(t_tanbao->events[0].value2, t_tanbao->events[0].value3));
		}
		else if (t_tanbao->type == 7)
		{
			int yidong = 0;
			do
			{
				yidong = Utils::get_int32(-3, 6);
				if (yidong != 0)
				{
					break;
				}
			} while (true);

			get_tanbao(yidong, false, gezi, smsg);
		}
	}
}

void HuodongManager::terminal_huodong_tanbao_mubiao(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_tanbao_active msg;
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

	dhc::huodong_player_t* huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TBHD_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	const s_t_tanbao_mubiao* t_mubiao = sHuodongConfig->get_t_tanbao_mubiao(msg.id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (huodong_player->arg3() < t_mubiao->num)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < huodong_player->args3_size(); ++i)
	{
		if (huodong_player->args3(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	huodong_player->add_args3(msg.id());
	s_t_rewards rds;
	rds.add_reward(t_mubiao->type, t_mubiao->value1, t_mubiao->value2, t_mubiao->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TANBAO_MUBIAO);
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_shop_buy(player, rds, name, id);
}

void HuodongManager::terminal_huodong_tanbao_shop(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_shop_buy msg;
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

	if (msg.num() <= 0 || msg.num() > 1000)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::huodong_player_t* huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TBHD_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	const s_t_tanbao_shop *t_shop_other = sHuodongConfig->get_t_tanbao_shop(msg.item_id());
	if (!t_shop_other)
	{
		GLOBAL_ERROR;
		return;
	}

	if (!(huodong_player->arg7() & (1 << t_shop_other->shop_type)))
	{
		GLOBAL_ERROR;
		return;
	}

	int num = 0;
	for (int i = 0; i < huodong_player->args4_size(); ++i)
	{
		if (huodong_player->args4(i) == t_shop_other->shop_type)
		{
			num = huodong_player->args5(i);
			break;
		}
	}
	if (num <= 0)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < t_shop_other->price * msg.num())
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	int index = -1;
	for (int i = 0; i < huodong_player->args1_size(); ++i)
	{
		if (huodong_player->args1(i) == msg.item_id())
		{
			if (huodong_player->args2(i) + msg.num() > t_shop_other->num * num)
			{
				GLOBAL_ERROR;
				return;
			}
			index = i;
			break;
		}
	}

	s_t_rewards rds;
	for (int i = 0; i < msg.num(); i++)
	{
		rds.add_reward(t_shop_other->type, t_shop_other->value1, t_shop_other->value2, t_shop_other->value3);
	}
	rds.merge();
	if (index == -1)
	{
		huodong_player->add_args1(msg.item_id());
		huodong_player->add_args2(msg.num());
	}
	else
	{
		huodong_player->set_args2(index, huodong_player->args2(index) + msg.num());
	}
	huodong_player->set_arg5(huodong_player->arg5() + msg.num() * t_shop_other->point);
	if (huodong_player->arg5() >= 5888)
	{
		RankOperation::check_value(player, e_rank_tanbao_jy, huodong_player->arg5());
	}
	RankOperation::check_value(player, e_rank_tanbao_normal, huodong_player->arg5());
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TANBAO_SHOP);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_shop_other->price * msg.num(), LOGWAY_TANBAO_SHOP);
	player->set_hs_task_num(player->hs_task_num() + msg.num());
	PlayerOperation::player_add_active(player, 1100, msg.num());
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_shop_buy(player, rds, name, id);
}

void HuodongManager::terminal_huodong_modify(const std::string &data, const std::string &name, int id)
{
	rpcproto::tmsg_fight_treasure msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t *player = POOL_GET_PLAYER(msg.guid());
	if (!player)
	{
		protocol::self::self_player_load smsg;
		smsg.set_data(data);
		smsg.set_name(name);
		smsg.set_id(id);
		std::string s;
		smsg.SerializeToString(&s);
		sPlayerLoad->load_player(msg.guid(), SELF_PLAYER_LOAD_HUODONG_MODIFY, s);
		return;
	}
	else
	{
		game::channel()->refresh_offline_time(msg.guid());
	}

	if (msg.template_id() == 1)
	{
		if (msg.enhance() >= resource::GOLD && msg.enhance() <= resource::BINGJING)
		{
			if (msg.star() > 0)
			{
				PlayerOperation::player_add_resource(player, static_cast<resource::resource_t>(msg.enhance()), msg.star(), LGOWAY_GM_COMMAND);
			}
			else
			{
				PlayerOperation::player_dec_resource(player, static_cast<resource::resource_t>(msg.enhance()), -msg.star(), LGOWAY_GM_COMMAND);
			}
		}
	}
	else if (msg.template_id() == 2)
	{
		const s_t_item *t_item = sItemConfig->get_item(msg.enhance());
		if (t_item)
		{
			int item_num = ItemOperation::item_num_templete(player, msg.enhance());
			if (msg.star() < 0)
			{
				int dec_num = -msg.star();
				if (item_num < dec_num)
				{
					ItemOperation::item_destory_templete(player, msg.enhance(), item_num, LGOWAY_GM_COMMAND);
				}
				else
				{
					ItemOperation::item_destory_templete(player, msg.enhance(), dec_num, LGOWAY_GM_COMMAND);
				}
			}
			else
			{
				ItemOperation::item_add_template(player, msg.enhance(), msg.star(), LGOWAY_GM_COMMAND);
			}
		}
	}
	else if (msg.template_id() == 500)
	{
		dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TKMY_TYPE);
		if (huodong_player)
		{
			huodong_player->set_arg2(msg.star());
			RankOperation::check_value(player, e_rank_tansuo_normal, huodong_player->arg2());
			sHuodongPool->save_huodong_player(huodong_player->guid());
		}
	}

	else if (msg.template_id() == 600)
	{
		dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_YKJJ_TYPE, HUODONG_YKJJ_TYPE);
		if (huodong_player)
		{
			huodong_player->set_arg1(msg.star());
			sHuodongPool->save_huodong_player(huodong_player->guid());
		}
	}

	if (msg.star_exp())
	{
		TermInfo *ti = game::channel()->get_channel(msg.guid());
		if (player && ti)
		{
			sPlayerPool->save_player(player->guid(), true);
			game::channel()->del_channel(msg.guid());
		}
	}
	
	game::rpc_service()->response(name, id, "");
}

void HuodongManager::terminal_huodong_modify_callback(Packet* pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_huodong_modify(msg.data(), msg.name(), msg.id());
}

void HuodongManager::terminal_huodong_tansuo_event_callback(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_huodong_tansuo_event(msg.data(), msg.name(), msg.id());
}

void HuodongManager::terminal_huodong_fanpai_view(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_huodong_fanpai_view smsg;
	sHuodongPool->huodong_view_fanpai(player, smsg);
	ResMessage::res_huodong_fanpai_view(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_fanpai(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_fanpai msg;
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_CZFP_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (huodong_player->arg6() >= 1100)
	{
		GLOBAL_ERROR;
		return;
	}

	int fanid = 0;
	if (msg.type() == 0)
	{
		if (msg.index() < 0 || msg.index() >= huodong_player->args1_size())
		{
			GLOBAL_ERROR;
			return;
		}
		if (huodong_player->args1(msg.index()) != 0)
		{
			GLOBAL_ERROR;
			return;
		}

		if (huodong_player->arg1() < 10 ||
			huodong_player->arg2() >= 10)
		{
			GLOBAL_ERROR;
			return;
		}

		std::set<int> ids;
		for (int i = 0; i < huodong_player->args1_size(); ++i)
		{
			if (huodong_player->args1(i) != 0)
			{
				ids.insert(huodong_player->args1(i));
			}
		}

		const s_t_fanpai *t_fanpai = sHuodongConfig->get_t_fanpai(1, ids);
		if (!t_fanpai)
		{
			GLOBAL_ERROR;
			return;
		}
		fanid = t_fanpai->id;

		PlayerOperation::player_add_resource(player, resource::JEWEL, t_fanpai->jewel, LOGWAY_HUODONG_CZFP);
		huodong_player->set_arg1(huodong_player->arg1() - 10);
		huodong_player->set_arg2(huodong_player->arg2() + 1);
		huodong_player->set_args1(msg.index(), t_fanpai->id);
		huodong_player->set_arg4(huodong_player->arg4() + 1);
		huodong_player->set_arg6(huodong_player->arg6() + 10);

		if (huodong_player->arg4() >= 8)
		{
			for (int i = 0; i < huodong_player->args1_size(); ++i)
			{
				huodong_player->set_args1(i, 0);
			}
			huodong_player->set_arg4(0);
		}
	}
	else
	{
		if (msg.index() < 0 || msg.index() >= huodong_player->args2_size())
		{
			GLOBAL_ERROR;
			return;
		}
		if (huodong_player->args2(msg.index()) != 0)
		{
			GLOBAL_ERROR;
			return;
		}
		if (huodong_player->arg1() < 100 ||
			huodong_player->arg3() >= 10)
		{
			GLOBAL_ERROR;
			return;
		}

		std::set<int> ids;
		for (int i = 0; i < huodong_player->args2_size(); ++i)
		{
			if (huodong_player->args2(i) != 0)
			{
				ids.insert(huodong_player->args2(i));
			}
		}

		const s_t_fanpai *t_fanpai = sHuodongConfig->get_t_fanpai(2, ids);
		if (!t_fanpai)
		{
			GLOBAL_ERROR;
			return;
		}
		fanid = t_fanpai->id;
		
		PlayerOperation::player_add_resource(player, resource::JEWEL, t_fanpai->jewel, LOGWAY_HUODONG_CZFP);
		huodong_player->set_arg1(huodong_player->arg1() - 100);
		huodong_player->set_arg3(huodong_player->arg3() + 1);
		huodong_player->set_args2(msg.index(), t_fanpai->id);
		huodong_player->set_arg5(huodong_player->arg5() + 1);
		huodong_player->set_arg6(huodong_player->arg6() + 100);

		if (huodong_player->arg5() >= 8)
		{
			for (int i = 0; i < huodong_player->args2_size(); ++i)
			{
				huodong_player->set_args2(i, 0);
			}
			huodong_player->set_arg5(0);
		}
	}
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_huodong_fanpai(player, fanid, name, id);
}

void HuodongManager::terminal_huodong_fanpai_cz(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_fanpai_cz msg;
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

	
	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_CZFP_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	int jewel = 5;
	if (msg.type() == 2)
	{
		for (int i = 0; i < huodong_player->args1_size(); ++i)
		{
			if (huodong_player->args1(i) == -1)
			{
				jewel = 0;
				break;
			}
		}
		if (jewel != 0)
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else if (msg.type() == 3)
	{
		for (int i = 0; i < huodong_player->args2_size(); ++i)
		{
			if (huodong_player->args2(i) == -1)
			{
				jewel = 0;
				break;
			}
		}
		if (jewel != 0)
		{
			GLOBAL_ERROR;
			return;
		}
	}

	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	if (msg.type() == 0 || msg.type() == 2)
	{
		for (int i = 0; i < huodong_player->args1_size(); ++i)
		{
			huodong_player->set_args1(i, 0);
		}
		huodong_player->set_arg4(0);
	}
	else if (msg.type() == 1 || msg.type() == 3)
	{
		for (int i = 0; i < huodong_player->args2_size(); ++i)
		{
			huodong_player->set_args2(i, 0);
		}
		huodong_player->set_arg5(0);
	}
	else
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_HUODONG_CZFP_CZ);
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_success(player, true, name, id);
}

void HuodongManager::terminal_huodong_zhuanpan_view(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_huodong_zhuanpan_view smsg;
	sHuodongPool->huodong_view_zhuanpan(player, smsg);
	ResMessage::res_huodong_zhuanpan_view(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_zhuanpan(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_zhuanpan msg;
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

	if (msg.num() != 1 &&
		msg.num() != 10)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_SZZP_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	int jewel = 0;
	int get_point = 0;
	const s_t_zhuanpan *t_zuanpan = 0;
	s_t_rewards rds;
	protocol::game::smsg_huodong_zhuan smsg;
	if (msg.type() == 0)
	{
		int free_num = 5 - huodong_player->arg1();
		if (free_num < 0)
		{
			free_num = 0;
		}
		int price_num = msg.num() - free_num;
		if (price_num > 0)
		{
			const s_t_price *t_price = 0;
			int get_num = huodong_player->arg1() - 5;
			if (get_num < 0)
			{
				get_num = 0;
			}
			for (int i = 0; i < price_num; ++i)
			{
				t_price = sPlayerConfig->get_price(get_num + 1 + i);
				if (!t_price)
				{
					GLOBAL_ERROR;
					return;
				}
				jewel += t_price->zhuanpan1;
			}
		}

		if (msg.num() == 10)
		{
			jewel = jewel * 90 / 100;
			jewel = jewel / 10 * 10;
		}

		if (player->jewel() < jewel)
		{
			PERROR(ERROR_JEWEL);
			return;
		}

		int eve_jewel = 0;
		for (int kc = 0; kc < msg.num(); ++kc)
		{
			t_zuanpan = sHuodongConfig->get_t_zhuanpan(1, huodong->extra_data());
			if (!t_zuanpan)
			{
				GLOBAL_ERROR;
				return;
			}
			smsg.add_id(t_zuanpan->id);

			eve_jewel = 0;
			if (t_zuanpan->reward.type == 1 &&
				t_zuanpan->reward.value1 == 2)
			{
				eve_jewel = huodong->extra_data() * t_zuanpan->reward.value3 / 100;
				rds.add_reward(1, resource::JEWEL, eve_jewel);
				smsg.add_jewel(eve_jewel);
			}
			else
			{
				rds.add_reward(t_zuanpan->reward);
				smsg.add_jewel(0);
			}
			huodong_player->set_arg1(huodong_player->arg1() + 1);
			huodong->set_extra_data(huodong->extra_data() - eve_jewel);
			if (huodong->extra_data() < 0)
				huodong->set_extra_data(0);
			huodong->set_extra_data(huodong->extra_data() + 1);
			huodong_player->set_arg5(huodong_player->arg5() + 10);
			get_point += 10;
		}
		smsg.set_pool_jewel(huodong->extra_data());
	}
	else
	{
		int free_num = 1 - huodong_player->arg3();
		if (free_num < 0)
		{
			free_num = 0;
		}
		int price_num = msg.num() - free_num;
		if (price_num > 0)
		{
			const s_t_price *t_price = 0;
			int get_num = huodong_player->arg3() - 1;
			if (get_num < 0)
			{
				get_num = 0;
			}
			for (int i = 0; i < price_num; ++i)
			{
				t_price = sPlayerConfig->get_price(get_num + 1 + i);
				if (!t_price)
				{
					GLOBAL_ERROR;
					return;
				}
				jewel += t_price->zhuanpan2;
			}
		}

		if (msg.num() == 10)
		{
			jewel = jewel * 90 / 100;
			jewel = jewel / 10 * 10;
		}

		if (player->jewel() < jewel)
		{
			PERROR(ERROR_JEWEL);
			return;
		}

		int eve_jewel = 0;
		for (int kc = 0; kc < msg.num(); ++kc)
		{
			t_zuanpan = sHuodongConfig->get_t_zhuanpan(2, huodong->extra_data1());
			if (!t_zuanpan)
			{
				GLOBAL_ERROR;
				return;
			}
			smsg.add_id(t_zuanpan->id);

			eve_jewel = 0;
			if (t_zuanpan->reward.type == 1 &&
				t_zuanpan->reward.value1 == 2)
			{
				eve_jewel = huodong->extra_data1() * t_zuanpan->reward.value3 / 100;
				rds.add_reward(1, resource::JEWEL, eve_jewel);
				smsg.add_jewel(eve_jewel);
			}
			else
			{
				rds.add_reward(t_zuanpan->reward);
				smsg.add_jewel(0);
			}
			
			huodong_player->set_arg3(huodong_player->arg3() + 1);
			huodong->set_extra_data1(huodong->extra_data1() - eve_jewel);
			if (huodong->extra_data1() < 0)
				huodong->set_extra_data1(0);
			huodong->set_extra_data1(huodong->extra_data1() + 10);
			huodong_player->set_arg5(huodong_player->arg5() + 100);
			get_point += 100;
		}
		smsg.set_pool_jewel(huodong->extra_data1());
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_HUODONG_ZHUANPAN);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_ZHUANPAN);
	RankOperation::check_value(player, e_rank_zhuanpan_normal, huodong_player->arg5());
	if (huodong_player->arg5() >= 13800)
	{
		RankOperation::check_value(player, e_rank_zhuanpan_jy, huodong_player->arg5());
	}
	sHuodongPool->huodong_active(player, HUODONG_COND_ZHUANPAN_COUNT, get_point);
	sHuodongPool->save_huodong_player(huodong_player->guid());

	ResMessage::res_huodong_zhuanpan(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_tansuo_view(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_huodong_tansuo_view smsg;
	sHuodongPool->huodong_view_tansuo(player, smsg);
	ResMessage::res_huodong_tansuo_view(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_tansuo(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_tansuo msg;
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TKMY_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	int jewel = 0;
	int num = 1;
	if (msg.type() == 1)
	{
		num = 10;
	}

	int free_num = 5 - huodong_player->arg1();
	if (free_num < 0)
	{
		free_num = 0;
	}

	int price_num = num - free_num;
	if (price_num > 0)
	{
		const s_t_price *t_price = 0;
		int get_num = huodong_player->arg1() - 5;
		if (get_num < 0)
		{
			get_num = 0;
		}
		for (int i = 0; i < price_num; ++i)
		{
			t_price = sPlayerConfig->get_price(get_num + 1 + i);
			if (!t_price)
			{
				GLOBAL_ERROR;
				return;
			}
			jewel += t_price->ts;
		}
	}

	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	const s_t_tansuo* t_tansuo = 0;
	const s_t_tansuo_event *t_event = 0;
	s_t_rewards rds;
	protocol::game::smsg_huodong_tansuo smsg;
	protocol::game::tansuo_event *tansuo_event = 0;
	dhc::role_t *role = 0;
	std::vector<int64_t> hp_sites(6, 0);
	int64_t hp = 0;
	int64_t total_hp = 0;
	uint64_t boss_guid = 0;
	int get_point = 0;
	int left_int = 5;

	for (int i = 0; i < num; ++i)
	{
		t_tansuo = sHuodongConfig->get_t_random_tansuo();
		if (!t_tansuo)
		{
			GLOBAL_ERROR;
			return;
		}
		if (t_tansuo->id == 3005)
		{
			bool has_recharge_event = false;
			for (int klg = 0; klg < huodong_player->args2_size(); ++klg)
			{
				if (huodong_player->args2(klg) == 3005)
				{
					has_recharge_event = true;
					break;
				}
			}
			if (has_recharge_event)
			{
				do 
				{
					t_tansuo = sHuodongConfig->get_t_random_tansuo();
					if (!t_tansuo)
					{
						GLOBAL_ERROR;
						return;
					}
					if (t_tansuo->id != 3005)
					{
						break;
					}
				} while (true);
			}
		}

		if (!t_tansuo)
		{
			GLOBAL_ERROR;
			return;
		}

		huodong->set_extra_data(huodong->extra_data() + 1);
		huodong_player->set_arg1(huodong_player->arg1() + 1);
		huodong_player->set_arg2(huodong_player->arg2() + 20);
		get_point += 20;

		tansuo_event = smsg.add_events();
		tansuo_event->set_id(huodong->extra_data());
		tansuo_event->set_ts_id(t_tansuo->id);

		if (t_tansuo->color == 1 ||
			t_tansuo->color == 2)
		{
			rds.add_reward(t_tansuo->type, t_tansuo->value1, t_tansuo->value2, t_tansuo->value3);
		}
		else
		{
			t_event = sHuodongConfig->get_t_tansuo_random_event(t_tansuo->id);
			if (!t_event)
			{
				GLOBAL_ERROR;
				return;
			}
			tansuo_event->set_qiyu_id(t_event->id);
			tansuo_event->set_qiyu_time(game::timer()->now() + 2 * 60 * 60 * 1000);

			/// 充值
			if (t_event->type_id == 3005)
			{
				tansuo_event->set_qiyu_arg2(t_event->def1);
			}
			else if (t_event->type_id == 3004)
			{
				tansuo_event->set_qiyu_arg1(Utils::get_int32(1, 3));
				left_int = 6 - tansuo_event->qiyu_arg1();
				if (left_int == 5)
				{
					tansuo_event->set_qiyu_arg2(Utils::get_int32(2, 3));
				}
				else if (left_int == 4)
				{
					tansuo_event->set_qiyu_arg2(Utils::get_int32(0, 1) ? 3 : 1);
				}
				else
				{
					tansuo_event->set_qiyu_arg2(Utils::get_int32(1, 2));
				}
				tansuo_event->set_qiyu_arg3(left_int - tansuo_event->qiyu_arg2());
			}
			/// 战斗
			else if (t_event->type_id == 3003)
			{
				dhc::player_t *boss = HuodongOperation::get_tansuo_boss(player);
				if (!boss)
				{
					boss = player;
				}
				boss_guid = boss->guid();
				for (int km = 0; km < 6; ++km)
				{
					hp_sites[km] = 0;
				}
				total_hp = 0;

				for (int j = 0; j < boss->zhenxing_size(); ++j)
				{
					std::map<int, double> role_attrs;
					if (boss->zhenxing(j))
					{
						role = POOL_GET_ROLE(boss->zhenxing(j));
						if (role)
						{
							RoleOperation::get_role_attr(boss, role, role_attrs);

							hp = role_attrs[1] * (1 + role_attrs[1 + 5] * 0.01f);
							if (boss->duixing(j) >= 0 && boss->duixing(j) < hp_sites.size())
							{
								hp_sites[boss->duixing(j)] = hp;
								total_hp += hp;
							}
						}
					}
				}
				tansuo_event->set_qiyu_arg1(boss->template_id());
				tansuo_event->set_qiyu_arg2(0);
				tansuo_event->set_qiyu_arg3(total_hp);
				tansuo_event->set_qiyu_arg4(total_hp);
			}

			huodong_player->add_args1(tansuo_event->id());
			huodong_player->add_args2(tansuo_event->ts_id());
			huodong_player->add_args3(tansuo_event->qiyu_id());
			huodong_player->add_times(tansuo_event->qiyu_time());
			huodong_player->add_args4(tansuo_event->qiyu_arg1());
			huodong_player->add_args5(tansuo_event->qiyu_arg2());
			huodong_player->add_args6(tansuo_event->qiyu_arg3());
			huodong_player->add_args7(tansuo_event->qiyu_arg4());
			huodong_player->add_args8(boss_guid);
			huodong_player->add_extra_data1(hp_sites[0]);
			huodong_player->add_extra_data2(hp_sites[1]);
			huodong_player->add_extra_data3(hp_sites[2]);
			huodong_player->add_extra_data4(hp_sites[3]);
			huodong_player->add_extra_data5(hp_sites[4]);
			huodong_player->add_extra_data6(hp_sites[5]);
		}
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_HUODONG_TS);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_TS);
	if (huodong_player->arg2() >= 12000)
	{
		RankOperation::check_value(player, e_rank_tansuo_normal, huodong_player->arg2());
	}
	sHuodongPool->huodong_active(player, HUODONG_COND_TANSUO_COUNT, get_point);
	sHuodongPool->save_huodong_player(huodong_player->guid());

	ResMessage::res_huodong_tansuo(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_tansuo_event(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_tansuo_event msg;
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TKMY_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	int index = -1;
	for (int i = 0; i < huodong_player->args1_size(); ++i)
	{
		if (huodong_player->args1(i) == msg.id())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}
	if (game::timer()->now() > huodong_player->times(index))
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_tansuo_event *t_event = sHuodongConfig->get_t_tansuo_event(huodong_player->args2(index),
		huodong_player->args3(index));
	if (!t_event)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_event->type_id == 3002)
	{
		t_event = sHuodongConfig->get_t_tansuo_random_event(huodong_player->args2(index), msg.type());
		if (!t_event)
		{
			GLOBAL_ERROR;
			return;
		}
	}

	bool complete = false;
	s_t_rewards rds;
	protocol::game::smsg_huodong_tansuo_event smsg;
	if (t_event->type_id == 3001)
	{
		if (player->jewel() < t_event->def2)
		{
			PERROR(ERROR_JEWEL);
			return;
		}
		PlayerOperation::player_dec_resource(player, resource::JEWEL, t_event->def2, LOGWAY_HUODONG_TS);
		huodong_player->set_arg2(huodong_player->arg2() + t_event->def2);
		sHuodongPool->huodong_active(player, HUODONG_COND_TANSUO_COUNT, t_event->def2);
		complete = true;
		rds.add_reward(t_event->rds);
	}
	else if (t_event->type_id == 3002)
	{
		if (msg.type() == 2)
		{
			if (huodong_player->args5(index))
			{
				GLOBAL_ERROR;
				return;
			}
			if (player->jewel() < 200)
			{
				PERROR(ERROR_JEWEL);
				return;
			}
			PlayerOperation::player_dec_resource(player, resource::JEWEL, 200, LOGWAY_HUODONG_TS);
			huodong_player->set_arg2(huodong_player->arg2() + 200);
			sHuodongPool->huodong_active(player, HUODONG_COND_TANSUO_COUNT, 200);
			huodong_player->set_args5(index, 1);
			rds.add_reward(t_event->rds);
		}
		else
		{
			if (huodong_player->args4(index))
			{
				GLOBAL_ERROR;
				return;
			}
			huodong_player->set_args4(index, 1);
			rds.add_reward(t_event->rds);
		}

		if (huodong_player->args4(index) &&
			huodong_player->args5(index))
		{
			complete = true;
		}
	}
	else if (t_event->type_id == 3003)
	{
		dhc::player_t *target = POOL_GET_PLAYER(huodong_player->args8(index));
		if (!target)
		{
			protocol::self::self_player_load msg;
			msg.set_data(data);
			msg.set_name(name);
			msg.set_id(id);
			std::string s;
			msg.SerializeToString(&s);
			sPlayerLoad->load_player(huodong_player->args8(index), SELF_PLAYER_LOAD_HUODONG_TANSUO, s);
			return;
		}
		else
		{
			game::channel()->refresh_offline_time(huodong_player->args8(index));
		}
		PlayerOperation::player_do_tili(player);
		if (player->tili() < t_event->def1)
		{
			PERROR(ERROR_TILI);
			return;
		}
		PlayerOperation::player_dec_resource(player, resource::TILI, t_event->def1);
		std::string text;
		int result = MissionFight::mission_ts(player, target, index, huodong_player, text);
		int64_t cur_hp = 0;
		if (result == 1)
		{
			rds.add_reward(t_event->rds);
			complete = true;
		}
		else
		{
			cur_hp += huodong_player->extra_data1(index);
			cur_hp += huodong_player->extra_data2(index);
			cur_hp += huodong_player->extra_data3(index);
			cur_hp += huodong_player->extra_data4(index);
			cur_hp += huodong_player->extra_data5(index);
			cur_hp += huodong_player->extra_data6(index);
			huodong_player->set_args6(index, cur_hp);
		}
		smsg.set_result(result);
		smsg.set_text(text);
		smsg.set_hp(cur_hp);
	}
	else if (t_event->type_id == 3004)
	{
		if (msg.type() == t_event->def1)
		{
			rds.add_reward(t_event->rds);
		}
		else
		{
			for (int i = 0; i < t_event->rds.size(); ++i)
			{
				rds.add_reward(t_event->rds[i].type, t_event->rds[i].value1, t_event->rds[i].value2 / 2);
			}
		}
		complete = true;
	}
	else if (t_event->type_id == 3005)
	{
		if (huodong_player->args4(index) == 1)
		{
			complete = true;
			rds.add_reward(t_event->rds);
		}
		else
		{
			GLOBAL_ERROR;
			return;
		}
	}
	else
	{
		GLOBAL_ERROR;
		return;
	}

	if (complete)
	{
		huodong_player->mutable_args1()->SwapElements(index, huodong_player->args1_size() - 1);
		huodong_player->mutable_args1()->RemoveLast();
		huodong_player->mutable_args2()->SwapElements(index, huodong_player->args2_size() - 1);
		huodong_player->mutable_args2()->RemoveLast();
		huodong_player->mutable_args3()->SwapElements(index, huodong_player->args3_size() - 1);
		huodong_player->mutable_args3()->RemoveLast();
		huodong_player->mutable_args4()->SwapElements(index, huodong_player->args4_size() - 1);
		huodong_player->mutable_args4()->RemoveLast();
		huodong_player->mutable_args5()->SwapElements(index, huodong_player->args5_size() - 1);
		huodong_player->mutable_args5()->RemoveLast();
		huodong_player->mutable_args6()->SwapElements(index, huodong_player->args6_size() - 1);
		huodong_player->mutable_args6()->RemoveLast();
		huodong_player->mutable_args7()->SwapElements(index, huodong_player->args7_size() - 1);
		huodong_player->mutable_args7()->RemoveLast();
		huodong_player->mutable_args8()->SwapElements(index, huodong_player->args8_size() - 1);
		huodong_player->mutable_args8()->RemoveLast();
		huodong_player->mutable_times()->SwapElements(index, huodong_player->times_size() - 1);
		huodong_player->mutable_times()->RemoveLast();
		huodong_player->mutable_extra_data1()->SwapElements(index, huodong_player->extra_data1_size() - 1);
		huodong_player->mutable_extra_data1()->RemoveLast();
		huodong_player->mutable_extra_data2()->SwapElements(index, huodong_player->extra_data2_size() - 1);
		huodong_player->mutable_extra_data2()->RemoveLast();
		huodong_player->mutable_extra_data3()->SwapElements(index, huodong_player->extra_data3_size() - 1);
		huodong_player->mutable_extra_data3()->RemoveLast();
		huodong_player->mutable_extra_data4()->SwapElements(index, huodong_player->extra_data4_size() - 1);
		huodong_player->mutable_extra_data4()->RemoveLast();
		huodong_player->mutable_extra_data5()->SwapElements(index, huodong_player->extra_data5_size() - 1);
		huodong_player->mutable_extra_data5()->RemoveLast();
		huodong_player->mutable_extra_data6()->SwapElements(index, huodong_player->extra_data6_size() - 1);
		huodong_player->mutable_extra_data6()->RemoveLast();
	}

	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_TS);
	if (huodong_player->arg2() >= 12000)
	{
		RankOperation::check_value(player, e_rank_tansuo_normal, huodong_player->arg2());
	}
	sHuodongPool->save_huodong_player(huodong_player->guid());

	ADD_MSG_REWARD(smsg, rds);
	ResMessage::res_huodong_tansuo_event(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_tansuo_mubiao(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_tansuo_mubiao msg;
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

	dhc::huodong_player_t* huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TKMY_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	const s_t_tansuo_mubiao* t_mubiao = sHuodongConfig->get_t_tansuo_mubiao(msg.id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (huodong_player->arg2() < t_mubiao->point)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < huodong_player->args9_size(); ++i)
	{
		if (huodong_player->args9(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	huodong_player->add_args9(msg.id());
	s_t_rewards rds;
	rds.add_reward(t_mubiao->rd);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_TS);
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_shop_buy(player, rds, name, id);
}

void HuodongManager::terminal_huodong_tansuo_event_refresh(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_tansuo_event msg;
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TKMY_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	int index = -1;
	for (int i = 0; i < huodong_player->args1_size(); ++i)
	{
		if (huodong_player->args1(i) == msg.id())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	if (game::timer()->now() > huodong_player->times(index))
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_tansuo_event *t_event = sHuodongConfig->get_t_tansuo_event(huodong_player->args2(index),
		huodong_player->args3(index));
	if (!t_event)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < 10)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	const s_t_tansuo_event *t_new_event = sHuodongConfig->get_t_tansuo_random_event(huodong_player->args2(index), -1, huodong_player->args3(index));
	if (!t_new_event)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, 10, LOGWAY_HUODONG_TS);
	huodong_player->set_args3(index, t_new_event->id);
	sHuodongPool->save_huodong_player(huodong_player->guid());

	ResMessage::res_huodong_tansuo_event_refresh(player, t_new_event->id, name, id);
}

void HuodongManager::terminal_huodong_tansuo_event_del(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_tansuo_event msg;
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_TKMY_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	int index = -1;
	for (int i = 0; i < huodong_player->args1_size(); ++i)
	{
		if (huodong_player->args1(i) == msg.id())
		{
			index = i;
			break;
		}
	}
	if (index == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	huodong_player->mutable_args1()->SwapElements(index, huodong_player->args1_size() - 1);
	huodong_player->mutable_args1()->RemoveLast();
	huodong_player->mutable_args2()->SwapElements(index, huodong_player->args2_size() - 1);
	huodong_player->mutable_args2()->RemoveLast();
	huodong_player->mutable_args3()->SwapElements(index, huodong_player->args3_size() - 1);
	huodong_player->mutable_args3()->RemoveLast();
	huodong_player->mutable_args4()->SwapElements(index, huodong_player->args4_size() - 1);
	huodong_player->mutable_args4()->RemoveLast();
	huodong_player->mutable_args5()->SwapElements(index, huodong_player->args5_size() - 1);
	huodong_player->mutable_args5()->RemoveLast();
	huodong_player->mutable_args6()->SwapElements(index, huodong_player->args6_size() - 1);
	huodong_player->mutable_args6()->RemoveLast();
	huodong_player->mutable_args7()->SwapElements(index, huodong_player->args7_size() - 1);
	huodong_player->mutable_args7()->RemoveLast();
	huodong_player->mutable_args8()->SwapElements(index, huodong_player->args8_size() - 1);
	huodong_player->mutable_args8()->RemoveLast();
	huodong_player->mutable_times()->SwapElements(index, huodong_player->times_size() - 1);
	huodong_player->mutable_times()->RemoveLast();
	huodong_player->mutable_extra_data1()->SwapElements(index, huodong_player->extra_data1_size() - 1);
	huodong_player->mutable_extra_data1()->RemoveLast();
	huodong_player->mutable_extra_data2()->SwapElements(index, huodong_player->extra_data2_size() - 1);
	huodong_player->mutable_extra_data2()->RemoveLast();
	huodong_player->mutable_extra_data3()->SwapElements(index, huodong_player->extra_data3_size() - 1);
	huodong_player->mutable_extra_data3()->RemoveLast();
	huodong_player->mutable_extra_data4()->SwapElements(index, huodong_player->extra_data4_size() - 1);
	huodong_player->mutable_extra_data4()->RemoveLast();
	huodong_player->mutable_extra_data5()->SwapElements(index, huodong_player->extra_data5_size() - 1);
	huodong_player->mutable_extra_data5()->RemoveLast();
	huodong_player->mutable_extra_data6()->SwapElements(index, huodong_player->extra_data6_size() - 1);
	huodong_player->mutable_extra_data6()->RemoveLast();

	sHuodongPool->save_huodong_player(huodong_player->guid());

	ResMessage::res_success(player, true, name, id);
}

void HuodongManager::terminal_huodong_mofang_view(const std::string &data, const std::string &name, int id)
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

	protocol::game::smsg_huodong_mofang_view smsg;
	sHuodongPool->huodong_view_mofang(player, smsg);
	ResMessage::res_huodong_mofang_view(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_mofang_chou(const std::string &data, const std::string &name, int id)
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (huodong_player->arg1() != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	huodong_player->set_arg1(1);
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_success(player, true, name, id);
}

void HuodongManager::terminal_huodong_mofang_refresh(const std::string &data, const std::string &name, int id)
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	
	if (huodong_player->arg1() != 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < 20)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	
	std::vector<int> ids;
	if (HuodongOperation::refresh_mofang(ids))
	{
		GLOBAL_ERROR;
		return;
	}

	huodong_player->set_arg1(0);
	huodong_player->clear_args1();
	huodong_player->clear_args2();

	protocol::game::smsg_huodong_mofang_refresh smsg;
	for (int i = 0; i < ids.size(); ++i)
	{
		huodong_player->add_args1(ids[i]);
		huodong_player->add_args2(0);
		smsg.add_ids(ids[i]);
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, 20, LOGWAY_HUODONG_MOFANG);
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_huodong_mofang_refresh(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_mofang_reset(const std::string &data, const std::string &name, int id)
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}


	if (huodong_player->arg1() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	bool empty = true;
	for (int i = 0; i < huodong_player->args2_size(); ++i)
	{
		if (huodong_player->args2(i) != 0)
		{
			empty = false;
			break;
		}
	}
	if (empty)
	{
		GLOBAL_ERROR;
		return;
	}

	
	std::vector<int> ids;
	if (HuodongOperation::refresh_mofang(ids))
	{
		GLOBAL_ERROR;
		return;
	}

	huodong_player->set_arg1(0);
	huodong_player->clear_args1();
	huodong_player->clear_args2();

	protocol::game::smsg_huodong_mofang_refresh smsg;
	for (int i = 0; i < ids.size(); ++i)
	{
		huodong_player->add_args1(ids[i]);
		huodong_player->add_args2(0);
		smsg.add_ids(ids[i]);
	}
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_huodong_mofang_refresh(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_mofang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_mofang msg;
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (msg.index() < 0 || msg.index() >= huodong_player->args2_size())
	{
		GLOBAL_ERROR;
		return;
	}

	if (huodong_player->args2(msg.index()) != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	if (huodong_player->arg1() != 1)
	{
		GLOBAL_ERROR;
		return;
	}

	int jewel = 0;
	if (huodong_player->arg5() >= 5)
	{
		const s_t_price * t_price = sPlayerConfig->get_price(huodong_player->arg2() + 1);
		if (!t_price)
		{
			GLOBAL_ERROR;
			return;
		}
		if (player->jewel() < t_price->mofang)
		{
			PERROR(ERROR_JEWEL);
			return;
		}
		jewel = t_price->mofang;
	}

	std::list<int> ids;
	std::list<int>::iterator iter;
	for (int i = 0; i < huodong_player->args1_size(); ++i)
	{
		ids.push_back(huodong_player->args1(i));
	}

	for (int i = 0; i < huodong_player->args2_size(); ++i)
	{
		if (huodong_player->args2(i) != 0)
		{
			iter = std::find(ids.begin(), ids.end(), huodong_player->args2(i));
			if (iter != ids.end())
			{
				ids.erase(iter);
			}
		}
	}

	const s_t_mofang *t_mofang = sHuodongConfig->get_t_mofang_random(ids);
	if (!t_mofang)
	{
		GLOBAL_ERROR;
		return;
	}
	int mofang_point = 1;
	if (t_mofang->mtype == 2)
	{
		mofang_point = 5;
	}
	else if (t_mofang->mtype == 1)
	{
		mofang_point = 15;
	}

	if (jewel > 0)
	{
		huodong_player->set_arg2(huodong_player->arg2() + 1);
		PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_HUODONG_MOFANG);
	}
	s_t_rewards rds;
	rds.add_reward(t_mofang->type, t_mofang->value1, t_mofang->value2, t_mofang->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_MOFANG);
	huodong_player->set_arg5(huodong_player->arg5() + 1);
	huodong_player->set_arg3(huodong_player->arg3() + mofang_point);
	huodong_player->set_arg4(huodong_player->arg4() + mofang_point);
	huodong_player->set_args2(msg.index(), t_mofang->id);
	huodong->set_extra_data1(huodong->extra_data1() + mofang_point);
	sHuodongPool->huodong_active(player, HUODONG_COND_JGMF_COUNT, mofang_point);
	

	bool full = true;
	for (int i = 0; i < huodong_player->args2_size(); ++i)
	{
		if (huodong_player->args2(i) == 0)
		{
			full = false;
			break;
		}
	}

	protocol::game::smsg_huodong_mofang smsg;
	smsg.set_id(t_mofang->id);
	if (full)
	{
		std::vector<int> idlist;
		HuodongOperation::refresh_mofang(idlist);
		huodong_player->set_arg1(0);
		huodong_player->clear_args1();
		huodong_player->clear_args2();
		for (int i = 0; i < idlist.size(); ++i)
		{
			huodong_player->add_args1(idlist[i]);
			huodong_player->add_args2(0);
			smsg.add_refresh_ids(idlist[i]);
		}
	}

	sHuodongPool->save_huodong_player(huodong_player->guid());

	ResMessage::res_huodong_mofang(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_mofangall(const std::string &data, const std::string &name, int id)
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (game::timer()->now() > huodong->rank_time())
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	if (huodong_player->arg1() != 0)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_price *t_price = 0;
	int jewel = 0;
	for (int i = 0; i < 9; ++i)
	{
		t_price = sPlayerConfig->get_price(huodong_player->arg2() + i + 1);
		if (!t_price)
		{
			GLOBAL_ERROR;
			return;
		}
		jewel += t_price->mofang;
	}
	jewel = jewel * 75 / 100;
	jewel = jewel / 10 * 10;

	if (player->jewel() < jewel)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	s_t_rewards rds;
	const s_t_mofang *t_mofang = 0;
	int mofang_point = 0;
	for (int i = 0; i < huodong_player->args1_size(); ++i)
	{
		t_mofang = sHuodongConfig->get_t_mofang(huodong_player->args1(i));
		if (!t_mofang)
		{
			GLOBAL_ERROR;
			return;
		}
		if (t_mofang->mtype == 2)
		{
			mofang_point += 5;
		}
		else if (t_mofang->mtype == 1)
		{
			mofang_point += 15;
		}
		else
		{
			mofang_point += 1;
		}
		rds.add_reward(t_mofang->type, t_mofang->value1, t_mofang->value2, t_mofang->value3);
	}

	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_MOFANG);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_HUODONG_MOFANG);
	sHuodongPool->huodong_active(player, HUODONG_COND_JGMF_COUNT, mofang_point);

	std::vector<int> idlist;
	HuodongOperation::refresh_mofang(idlist);
	huodong_player->set_arg1(0);
	huodong_player->set_arg2(huodong_player->arg2() + 9);
	huodong_player->set_arg3(huodong_player->arg3() + mofang_point);
	huodong_player->set_arg4(huodong_player->arg4() + mofang_point);
	huodong->set_extra_data1(huodong->extra_data1() + mofang_point);
	huodong_player->clear_args1();
	huodong_player->clear_args2();
	protocol::game::smsg_huodong_mofang_refresh smsg;
	for (int i = 0; i < idlist.size(); ++i)
	{
		huodong_player->add_args1(idlist[i]);
		huodong_player->add_args2(0);
		smsg.add_ids(idlist[i]);
	}
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_huodong_mofang_refresh(player, smsg, name, id);
}

void HuodongManager::terminal_huodong_mofang_mubiao(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_mofang_mubiao msg;
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

	dhc::huodong_player_t* huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_XHQD_TYPE, HUODONG_JGMF_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}

	const s_t_mofang_target* t_mubiao = sHuodongConfig->get_t_mofang_target(msg.id());
	if (!t_mubiao)
	{
		GLOBAL_ERROR;
		return;
	}

	if (huodong->extra_data() + huodong->extra_data1() < t_mubiao->point)
	{
		GLOBAL_ERROR;
		return;
	}

	for (int i = 0; i < huodong_player->args3_size(); ++i)
	{
		if (huodong_player->args3(i) == msg.id())
		{
			GLOBAL_ERROR;
			return;
		}
	}

	huodong_player->add_args3(msg.id());
	s_t_rewards rds;
	rds.add_reward(t_mubiao->type, t_mubiao->value1, t_mubiao->value2, t_mubiao->value3);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_MOFANG);
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_shop_buy(player, rds, name, id);
}

void HuodongManager::terminal_huodong_yueka_view(const std::string &data, const std::string &name, int id)
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
		
	protocol::game::smsg_huodong_yueka_view smsg;
	sHuodongPool->huodong_view_yueka(player, smsg);
	ResMessage::res_huodong_yueka_view(player, smsg, name, id);
}


void HuodongManager::terminal_huodong_yueka_reward(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_yueka_reward msg;
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

	dhc::huodong_player_t *huodong_player = sHuodongPool->get_huodong_player(player, HUODONG_YKJJ_TYPE, HUODONG_YKJJ_TYPE);
	if (!huodong_player)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}
	 
	dhc::huodong_t *huodong = POOL_GET_HUODONG(huodong_player->huodong_guid());
	if (!huodong)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}
	
	if (!huodong_player->arg1())
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.day() > huodong_player->arg2())
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_yueka* yueka = sHuodongConfig->get_t_yueka(msg.day());
	if (!yueka)
	{
		GLOBAL_ERROR;
		return;
	}

	s_t_rewards rds;
	if (huodong_player->arg1() == 1)
	{
		if (huodong_player->args1(msg.day() - 1) == 0)
		{
			PERROR(ERROR_HAS_FIRST_REWARD);
			return;
		}
		rds.add_reward(yueka->type, yueka->value1, yueka->value2, yueka->value3);
		huodong_player->set_args1(msg.day() - 1, 0);
	}

	if (huodong_player->arg1() == 2)
	{
		if (huodong_player->args2(msg.day() - 1) == 0)
		{
			PERROR(ERROR_HAS_FIRST_REWARD);
			return;
		}
		rds.add_reward(yueka->type2, yueka->value4, yueka->value5, yueka->value6);
		huodong_player->set_args2(msg.day() - 1, 0);
	}

	if (huodong_player->arg1() == 3)
	{
		if (msg.level() == 1)
		{	
			if (huodong_player->args1(msg.day() - 1) == 0)
			{
				PERROR(ERROR_HAS_FIRST_REWARD);
				return;
			}
			rds.add_reward(yueka->type, yueka->value1, yueka->value2, yueka->value3);
			huodong_player->set_args1(msg.day() - 1, 0);		
		}

		if (msg.level() == 2)
		{
			if (huodong_player->args2(msg.day() - 1) == 0)
			{
				PERROR(ERROR_HAS_FIRST_REWARD);
				return;
			}
			rds.add_reward(yueka->type2, yueka->value4, yueka->value5, yueka->value6);
			huodong_player->set_args2(msg.day() - 1, 0);
		}
	}
	
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_YKJJ);
	sHuodongPool->save_huodong_player(huodong_player->guid());
	ResMessage::res_huodong_reward(player, rds, name, id);
	
}
void HuodongManager::terminal_huodong_huigui_reward(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_huigui_reward msg;
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

	if (player->huodong_yxhg_tap() == 0)
	{
		PERROR(ERROR_HUODONG_TIME_OUT);
		return;
	}
	s_t_rewards rds;
	if (msg.type() == 1)
	{
		const s_t_huigui_buzhu* buzhu = sHuodongConfig->get_t_huigui_buzhu(msg.id());
		if (buzhu)
		{
			if (buzhu->def > player->huodong_yxhg_buzhu_day())
			{
				PERROR(ERROR_ONLINE_REWARD_TIME);
				return;
			}
			for (int i = 0; i < player->huodong_yxhg_buzhu_id_size(); ++i)
			{
				if (buzhu->id == player->huodong_yxhg_buzhu_id(i))
				{
					PERROR(ERROR_HAS_FIRST_REWARD);
					return;
				}
			}
			rds.add_reward(buzhu->type1, buzhu->value1, buzhu->value2, buzhu->value3);
			rds.add_reward(buzhu->type2, buzhu->value4, buzhu->value5, buzhu->value6);
			rds.add_reward(buzhu->type3, buzhu->value7, buzhu->value8, buzhu->value9);
			player->add_huodong_yxhg_buzhu_id(buzhu->id);
		}
	}

	if (msg.type() == 2)
	{
		const s_t_huigui_fuli* fuli = sHuodongConfig->get_t_huigui_fuli(msg.id());
		if (fuli)
		{
			if (player->level() < fuli->def4)
			{
				PERROR(ERROR_LEVEL);
				return;
			}
			if (msg.num() > fuli->def3)
			{
				PERROR(ERROR_TILI_ADD);
				return;
			}
			int jewel = fuli->def1 * msg.num();
			if (player->jewel() < jewel)
			{
				PERROR(ERROR_JEWEL);
				return;
			}
			int index = -1;
			for (int i = 0; i < player->huodong_yxhg_fuli_id_size(); ++i)
			{
				if (fuli->id == player->huodong_yxhg_fuli_id(i))
				{
					int num = fuli->def3 - player->huodong_yxhg_fuli_num(i);
					if (msg.num() > num)
					{
						PERROR(ERROR_MISSION_CISHU);
						return;
					}
					index = i;
					break;
				}	
			}
			if (index == -1)
			{
				player->add_huodong_yxhg_fuli_id(fuli->id);
				player->add_huodong_yxhg_fuli_num(msg.num());
			}
			else
			{
				player->set_huodong_yxhg_fuli_num(index, player->huodong_yxhg_fuli_num(index) + msg.num());
			}
			PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_HUODONG_YXHG);
			for (int i = 0; i < msg.num(); ++i)
			{
				rds.add_reward(fuli->type, fuli->value1, fuli->value2, fuli->value3);
			}
		}
	}

	if (msg.type() == 3)
	{
		const s_t_huigui_haoli* haoli = sHuodongConfig->get_t_huigui_haoli(msg.id());
		if (haoli)
		{
			if (player->huodong_yxhg_rmb() < haoli->def)
			{
				PERROR(ERROR_TASK_TJ);
				return;
			}
			for (int i = 0; i < player->huodong_yxhg_haoli_id_size(); ++i)
			{
				if (haoli->id == player->huodong_yxhg_haoli_id(i))
				{
					PERROR(ERROR_HAS_FIRST_REWARD);
					return;
				}
			}
			rds.add_reward(haoli->type1, haoli->value1, haoli->value2, haoli->value3);
			if (haoli->type2 != 0)
			{
				rds.add_reward(haoli->type2, haoli->value4, haoli->value5, haoli->value6);
			}
			player->add_huodong_yxhg_haoli_id(haoli->id);
			
		}
	}
	PlayerOperation::player_add_reward(player, rds, LOGWAY_HUODONG_YXHG);
	ResMessage::res_huodong_reward(player, rds, name, id);
}

void HuodongManager::terminal_huodong_zhichong_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_huodong_reward_view msg;
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

	protocol::game::smsg_huodong_reward_view msg1;
	sHuodongPool->huodong_view(player, msg.id(), HUODONG_ZC_TYPE, msg1);
	ResMessage::res_huodong_reward_view(player, msg1, name, id);

}
