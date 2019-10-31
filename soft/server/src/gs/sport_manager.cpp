#include "sport_manager.h"
#include "sport_pool.h"
#include "sport_config.h"
#include "sport_operation.h"
#include "gs_message.h"
#include "player_config.h"
#include "player_load.h"
#include "player_operation.h"
#include "social_operation.h"
#include "mission_operation.h"
#include "huodong_pool.h"
#include "item_operation.h"
#include "mission_fight.h"
#include "role_operation.h"
#include "utils.h"
#include "item_config.h"

#define SPORT_TIME 300000
#define SPORT_PERIOD 1000

SportManager::SportManager()
: timer_(-1)
{
}

SportManager::~SportManager()
{
}

int SportManager::init()
{
	if (-1 == sSportConfig->parse())
	{
		return -1;
	}
	sSportPool->init();
	timer_ = game::timer()->schedule(boost::bind(&SportManager::update, this, _1), SPORT_PERIOD, "sport");

	return 0;
}

int SportManager::fini()
{
	sSportPool->fini();
	if (timer_)
	{
		game::timer()->cancel(timer_);
		timer_ = 0;
	}
	return 0;
}

int SportManager::update(const ACE_Time_Value &curr)
{
	sSportPool->update();
	return 0;
}

void SportManager::terminal_sport_look(const std::string &data, const std::string &name, int id)
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

	sport_look(player, name, id);
}

void SportManager::sport_look(dhc::player_t *player, const std::string &name, int id)
{
	int rank = sSportPool->get_now_rank(player->guid());
	if (rank == 0)
	{
		GLOBAL_ERROR;
		return;
	}
	std::vector<int> ranks;
	SportOperation::get_rank(player, ranks);

	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		GLOBAL_ERROR;
		return;
	}
	std::vector<protocol::game::msg_sport_player> msgs;
	for (int i = 0; i < ranks.size(); ++i)
	{
		protocol::game::msg_sport_player msg;
		msg.set_player_guid(sport_list->player_guid(ranks[i] - 1));
		msg.set_player_template(sport_list->player_template(ranks[i] - 1));
		msg.set_player_name(sport_list->player_name(ranks[i] - 1));
		msg.set_player_level(sport_list->player_level(ranks[i] - 1));
		msg.set_player_bat_eff(sport_list->player_bat_eff(ranks[i] - 1));
		msg.set_player_isnpc(sport_list->player_isnpc(ranks[i] - 1));
		msg.set_player_vip(sport_list->player_vip(ranks[i] - 1));
		msg.set_player_achieve(sport_list->player_achieve(ranks[i] - 1));
		msg.set_player_chenghao(sport_list->player_chenghao(ranks[i] - 1));
		msg.set_player_rank(ranks[i]);
		msg.set_player_nalflag(sport_list->nalflag(ranks[i] - 1));
		msgs.push_back(msg);
	}

	int last_rank = sSportPool->get_last_rank(player->guid());
	int can_get = SportOperation::sport_can_get(player);

	ResMessage::res_sport_look(player, msgs, rank, last_rank, can_get, name, id);
}

void SportManager::terminal_sport_top(const std::string &data, const std::string &name, int id)
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

	int rank = sSportPool->get_now_rank(player->guid());
	if (rank == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		GLOBAL_ERROR;
		return;
	}
	std::vector<protocol::game::msg_sport_player> msgs;
	int num = 10;
	if (num > sport_list->player_guid_size())
	{
		num = sport_list->player_guid_size();
	}
	for (int i = 0; i < num; ++i)
	{
		protocol::game::msg_sport_player msg;
		msg.set_player_guid(sport_list->player_guid(i));
		msg.set_player_template(sport_list->player_template(i));
		msg.set_player_name(sport_list->player_name(i));
		msg.set_player_level(sport_list->player_level(i));
		msg.set_player_bat_eff(sport_list->player_bat_eff(i));
		msg.set_player_isnpc(sport_list->player_isnpc(i));
		msg.set_player_vip(sport_list->player_vip(i));
		msg.set_player_achieve(sport_list->player_achieve(i));
		msg.set_player_chenghao(sport_list->player_chenghao(i));
		msg.set_player_rank(i + 1);
		msg.set_player_nalflag(sport_list->nalflag(i));
		msgs.push_back(msg);
	}

	ResMessage::res_sport_top(player, msgs, name, id);
}

void SportManager::self_player_load_sport(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_sport_fight_end(msg.data(), msg.name(), msg.id());
}

void SportManager::self_player_load_sport_saodang(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_sport_saodang(msg.data(), msg.name(), msg.id());
}

void SportManager::terminal_sport_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_sport_fight_end msg;
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

	if (!PlayerOperation::check_fight_time(player))
	{
		PERROR(ERROR_FIGHT_TIME);
		return;
	}
	PlayerOperation::player_do_energy(player);
	if (player->energy() < 2)
	{
		PERROR(ERROR_ENERGY);
		return;
	}
	
	int rank = sSportPool->get_now_rank(player->guid());
	if (rank == 0)
	{
		GLOBAL_ERROR;
		return;
	}
	std::vector<int> ranks;
	SportOperation::get_rank(player, ranks);

	if (msg.index() < 0 || msg.index() >= ranks.size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (rank == ranks[msg.index()])
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		GLOBAL_ERROR;
		return;
	}

	int rank1 = ranks[msg.index()];
	uint64_t target_guid = sport_list->player_guid(rank1 - 1);
	std::string target_name = sport_list->player_name(rank1 - 1);
	int target_bf = sport_list->player_bat_eff(rank1 - 1);
	int target_tempalte = sport_list->player_template(rank1 - 1);
	int target_level = sport_list->player_level(rank1 - 1);
	int is_npc = sport_list->player_isnpc(rank1 - 1);
	int target_vip = sport_list->player_vip(rank1 - 1);
	int target_achieve = sport_list->player_achieve(rank1 - 1);
	int target_chenghao = sport_list->player_chenghao(rank1 - 1);
	int target_nalflag = sport_list->nalflag(rank1 - 1);
	dhc::player_t *target = 0;
	if (player->jj_task_num() == 0)
	{
		target = sSportPool->get_sport_player(rank1);
	}
	else if (is_npc)
	{
		target = sSportPool->get_sport_player(rank1);
	}
	else
	{
		target = POOL_GET_PLAYER(target_guid);
		if (!target)
		{
			protocol::self::self_player_load msg;
			msg.set_data(data);
			msg.set_name(name);
			msg.set_id(id);
			std::string s;
			msg.SerializeToString(&s);
			sPlayerLoad->load_player(target_guid, SELF_PLAYER_LOAD_SPORT, s);
			return;
		}
		else
		{
			game::channel()->refresh_offline_time(target_guid);
		}
	}
	if (!target)
	{
		GLOBAL_ERROR;
		return;
	}

	int rank_jewel = 0;
	std::string text;
	s_t_rewards rds;
	int result = MissionFight::mission_sport(player, target, text);
	if (result == 1)
	{
		sHuodongPool->huodong_active(player, HUODONG_COND_JJC_COUNT, 1);
		RoleOperation::check_bskill_count(player, BSKILL_TYPE3, 1);
		player->set_jjcs_task_num(player->jjcs_task_num() + 1);
		rds.add_reward(1, resource::JJ_POINT, 50);
		if (rank > rank1)
		{
			sport_list->set_player_guid(rank1 - 1, player->guid());
			sport_list->set_player_template(rank1 - 1, player->template_id());
			sport_list->set_player_name(rank1 - 1, player->name());
			sport_list->set_player_level(rank1 - 1, player->level());
			sport_list->set_player_bat_eff(rank1 - 1, player->bf());
			sport_list->set_player_isnpc(rank1 - 1, 0);
			sport_list->set_player_vip(rank1 - 1, player->vip());
			sport_list->set_player_achieve(rank1 - 1, player->dress_achieves_size());
			sport_list->set_player_chenghao(rank1 - 1, player->chenghao_on());
			sport_list->set_nalflag(rank1 - 1, player->nalflag());
			sSportPool->set_now_rank(player->guid(), rank1);

			sport_list->set_player_guid(rank - 1, target_guid);
			sport_list->set_player_template(rank - 1, target_tempalte);
			sport_list->set_player_name(rank - 1, target_name);
			sport_list->set_player_level(rank - 1, target_level);
			sport_list->set_player_bat_eff(rank - 1, target_bf);
			sport_list->set_player_isnpc(rank - 1, is_npc);
			sport_list->set_player_vip(rank - 1, target_vip);
			sport_list->set_player_achieve(rank - 1, target_achieve);
			sport_list->set_player_chenghao(rank - 1, target_chenghao);
			sport_list->set_nalflag(rank - 1, target_nalflag);
			sSportPool->set_now_rank(target_guid, rank);

			SportOperation::create_sport(player, target_guid, target_name, 0, 0, rank1);
			if (target &&
				is_npc == 0)
			{
				SportOperation::create_sport(target, player->guid(), player->name(), 1, 1, rank);
			}

			if (rank1 < player->max_rank())
			{
				rank_jewel = SportOperation::player_max_rank(player, rank1);
				rds.add_reward(1, resource::JEWEL, rank_jewel);
			}
			if (rank1 == 1)
			{
				SocialOperation::gundong_server("t_server_language_text_sport_first", player->name(), target_name, "");
			}
		}
		else
		{
			SportOperation::create_sport(player, target_guid, target_name, 0, 0, 0);
			if (target &&
				is_npc == 0)
			{
				SportOperation::create_sport(target, player->guid(), player->name(), 1, 1, 0);
			}
		}
	}
	else
	{
		rds.add_reward(1, resource::JJ_POINT, 30);
		SportOperation::create_sport(player, target_guid, target_name, 0, 1, 0);
		if (target &&
			is_npc == 0)
		{
			SportOperation::create_sport(target, player->guid(), player->name(), 1, 0, 0);
		}
	}

	PlayerOperation::player_dec_resource(player, resource::ENERGY, 2, LOGWAY_SPORT_FIGHT_END);

	rds.add_reward(1, resource::EXP, 12);
	rds.add_reward(1, resource::GOLD, 1000 + player->level() * 40);
	player->set_jj_task_num(player->jj_task_num() + 1);
	PlayerOperation::player_add_active(player, 900, 1);

	/// ·­ÅÆ
	int cid = 0;
	int pgold = 0;
	if (result == 1)
	{
		cid = sSportConfig->refresh_sport_card(player->level(), sHuodongPool->has_jieri_huodong(player));
		if (cid > 0)
		{
			const s_t_sport_card * t_card = sSportConfig->get_sport_shop_card(cid);
			if (t_card)
			{
				s_t_rewards prds;
				if (t_card->reward.type == 1 &&
					t_card->reward.value1 == 1)
				{
					pgold = Utils::get_int32(player->level() * 20, player->level() * 30);
					prds.add_reward(1, resource::GOLD, pgold);
				}
				else
				{
					prds.add_reward(t_card->reward);
				}
				PlayerOperation::player_add_reward(player, prds, LOGWAY_SPORT_FIGHT_END);
			}
		}
	}
	
	PlayerOperation::player_add_reward(player, rds, LOGWAY_SPORT_FIGHT_END);
	ResMessage::res_sport_fight_end(player, result, text, rds, cid, pgold, name, id);
}

void SportManager::terminal_sport_saodang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_sport_saodang msg;
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

	int rank = sSportPool->get_now_rank(player->guid());
	if (rank == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_do_energy(player);

	int saodang_num = player->energy() / 2;
	int total_num = msg.num();
	if (total_num > 100)
	{
		total_num = 100;
	}
	if (saodang_num < total_num)
	{
		if (!msg.use_item())
		{
			PERROR(ERROR_ENERGY);
			return;
		}
		else
		{
			int item_num = ItemOperation::item_num_templete(player, 10010007);
			if (item_num <= 0)
			{
				PERROR(ERROR_ENERGY);
				return;
			}
			const s_t_item *t_item = sItemConfig->get_item(10010007);
			if (!t_item)
			{
				GLOBAL_ERROR;
				return;
			}
			const s_t_itemstore *t_itemstore = sItemConfig->get_itemstore(t_item->def1);
			if (!t_itemstore)
			{
				GLOBAL_ERROR;
				return;
			}
			if (t_itemstore->type != 1)
			{
				GLOBAL_ERROR;
				return;
			}
			if (t_itemstore->rewards.empty())
			{
				GLOBAL_ERROR;
				return;
			}
			item_num = item_num * t_itemstore->rewards[0].value2 / 2;
			if (saodang_num + item_num < total_num)
			{
				PERROR(ERROR_ENERGY);
				return;
			}
		}
	}

	std::vector<int> ranks;
	SportOperation::get_rank(player, ranks);
	if (msg.index() < 0 || msg.index() >= ranks.size())
	{
		GLOBAL_ERROR;
		return;
	}
	if (rank >= ranks[msg.index()])
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t sport_list_guid = MAKE_GUID(et_sport_list, 0);
	dhc::sport_list_t *sport_list = POOL_GET_SPORT_LIST(sport_list_guid);
	if (!sport_list)
	{
		GLOBAL_ERROR;
		return;
	}
	dhc::player_t *target = 0;
	if (sport_list->player_isnpc(ranks[msg.index()] - 1))
	{
		target = sSportPool->get_sport_player(ranks[msg.index()]);
	}
	else
	{
		uint64_t target_guid = sport_list->player_guid(ranks[msg.index()] - 1);
		target = POOL_GET_PLAYER(target_guid);
		if (!target)
		{
			protocol::self::self_player_load msg;
			msg.set_data(data);
			msg.set_name(name);
			msg.set_id(id);
			std::string s;
			msg.SerializeToString(&s);
			sPlayerLoad->load_player(target_guid, SELF_PLAYER_LOAD_SPORT_SAODANG, s);
			return;
		}
		else
		{
			game::channel()->refresh_offline_time(target_guid);
		}
	}
	if (!target)
	{
		GLOBAL_ERROR;
		return;
	}

	int cid = 0;
	s_t_rewards rds;
	protocol::game::smsg_sport_saodang smsg;
	protocol::game::smsg_sport_fight_end* sub = 0;
	const s_t_sport_card * t_card = 0;
	int result = -1;
	std::string text;
	int saodang_num_i = 0, saodang_num_j = 0;
	for (int i = 0; i < total_num; ++i)
	{
		sub = smsg.add_saodangs();
		if (sub)
		{
			if (player->energy() < 2)
			{
				if (msg.use_item() && ItemOperation::item_num_templete(player, 10010007) > 0)
				{
					const s_t_item *t_item = sItemConfig->get_item(10010007);
					if (!t_item)
					{
						GLOBAL_ERROR;
						return;
					}
					const s_t_itemstore *t_itemstore = sItemConfig->get_itemstore(t_item->def1);
					if (!t_itemstore)
					{
						GLOBAL_ERROR;
						return;
					}
					if (t_itemstore->type != 1)
					{
						GLOBAL_ERROR;
						return;
					}
					s_t_rewards rdapply;
					rdapply.add_reward(t_itemstore->rewards);
					PlayerOperation::player_add_reward(player, rdapply, LOGWAY_SPORT_SAODANG);
					smsg.set_item_num(smsg.item_num() + 1);
					ItemOperation::item_destory_templete(player, 10010007, 1, LOGWAY_SPORT_SAODANG);
				}
			}

			if (player->energy() < 2)
			{
				break;
			}

			saodang_num_i += 1;
			PlayerOperation::player_dec_resource(player, resource::ENERGY, 2);
			result = MissionFight::mission_sport(player, target, text);

			sub->set_result(result);
			sub->set_text("");
			sub->set_max_rank(0);

			if (result == 1)
			{
				saodang_num_j += 1;
				

				sub->add_types(1);
				sub->add_value1s(resource::JJ_POINT);
				sub->add_value2s(50);
				sub->add_value3s(0);
				rds.add_reward(1, resource::JJ_POINT, 50);
			}
			else
			{
				sub->add_types(1);
				sub->add_value1s(resource::JJ_POINT);
				sub->add_value2s(30);
				sub->add_value3s(0);
				rds.add_reward(1, resource::JJ_POINT, 30);
			}

			

			sub->add_types(1);
			sub->add_value1s(resource::GOLD);
			sub->add_value2s(1000 + player->level() * 40);
			sub->add_value3s(0);
			rds.add_reward(1, resource::GOLD, 1000 + player->level() * 40);

			sub->add_types(1);
			sub->add_value1s(resource::EXP);
			sub->add_value2s(12);
			sub->add_value3s(0);
			rds.add_reward(1, resource::EXP, 12);

			sub->set_pgold(0);
			sub->set_cid(0);

			if (result == 1)
			{
				cid = sSportConfig->refresh_sport_card(player->level(), sHuodongPool->has_jieri_huodong(player));
				if (cid > 0)
				{
					t_card = sSportConfig->get_sport_shop_card(cid);
					if (t_card)
					{
						if (t_card->reward.type == 1 &&
							t_card->reward.value1 == 1)
						{
							sub->set_pgold(Utils::get_int32(player->level() * 20, player->level() * 30));
							rds.add_reward(1, resource::GOLD, sub->pgold());
						}
						else
						{
							sub->set_cid(cid);
							rds.add_reward(t_card->reward);
						}
					}
				}
			}
		}
	}

	if (saodang_num_j > 0)
	{
		sHuodongPool->huodong_active(player, HUODONG_COND_JJC_COUNT, saodang_num_j);
		RoleOperation::check_bskill_count(player, BSKILL_TYPE3, saodang_num_j);
		player->set_jjcs_task_num(player->jjcs_task_num() + saodang_num_j);
	}
	if (saodang_num_i > 0)
	{
		player->set_jj_task_num(player->jj_task_num() + saodang_num_i);
		PlayerOperation::player_add_active(player, 900, saodang_num_i);
	}

	PlayerOperation::player_add_reward(player, rds, LOGWAY_SPORT_SAODANG);
	ResMessage::res_sport_saodang(player, smsg, name, id);
}

void SportManager::terminal_sport_reward(const std::string &data, const std::string &name, int id)
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
	
	int rank = sSportPool->get_now_rank(player->guid());
	if (rank == 0)
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t sport_list_guid1 = MAKE_GUID(et_sport_list, 1);
	dhc::sport_list_t *sport_list1 = POOL_GET_SPORT_LIST(sport_list_guid1);
	if (!sport_list1)
	{
		GLOBAL_ERROR;
		return;
	}
	int last_rank = sSportPool->get_last_rank(player->guid());
	if (last_rank == 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (sport_list1->player_level(last_rank - 1) == 0)
	{
		PERROR(ERROR_SPORT_HAD_REWARD);
		return;
	}

	sport_list1->set_player_level(last_rank - 1, 0);
	s_t_sport_rank *t_sport_rank = sSportConfig->get_sport_rank(last_rank);
	if (!t_sport_rank)
	{
		GLOBAL_ERROR;
		return;
	}
	PlayerOperation::player_add_resource(player, resource::JEWEL, t_sport_rank->jewel, LOGWAY_SPORT_REWARD);
	PlayerOperation::player_add_resource(player, resource::JJ_POINT, t_sport_rank->jjcpoint, LOGWAY_SPORT_REWARD);

	ResMessage::res_success(player, true, name, id);
}

void SportManager::terminal_sport_qicuo(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_qiecuo msg;
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

	dhc::player_t *target = POOL_GET_PLAYER(msg.target());
	if (!target)
	{
		GLOBAL_ERROR;
		return;
	}

	std::string text;
	int result = MissionFight::mission_sport(player, target, text);

	protocol::game::smsg_qiecuo smsg;
	smsg.set_result(result);
	smsg.set_text(text);

	ResMessage::res_qiechuo(player, smsg, name, id);
}