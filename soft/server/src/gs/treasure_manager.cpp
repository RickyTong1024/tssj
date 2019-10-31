#include "treasure_manager.h"
#include "gs_message.h"
#include "player_config.h"
#include "player_operation.h"
#include "treasure_operation.h"
#include "treasure_config.h"
#include "sport_config.h"
#include "item_config.h"
#include "item_operation.h"
#include "treasure_list.h"
#include "player_load.h"
#include "mission_fight.h"
#include "utils.h"
#include "huodong_pool.h"
#include "role_operation.h"

static int __get_treasure_suipian_count(dhc::player_t *player,
	const s_t_treasure *treasure)
{
	int count = 0;
	const s_t_treasure_suipian *suipian = 0;
	for (std::vector<int>::size_type i = 0; i < treasure->suipian.size(); ++i)
	{
		suipian = sTreasureConfig->get_suipian(treasure->suipian[i]);
		if (suipian)
		{
			count += ItemOperation::item_num_templete(player, suipian->template_id);
		}
	}
	return count;
}


int TreasureManager::init()
{
	if (sTreasureConfig->parse() == -1)
	{
		return -1;
	}

	if (sTreasureList->init() == -1)
	{
		return -1;
	}

	terasure_timer_ = game::timer()->schedule(boost::bind(&TreasureList::update, sTreasureList, _1), 240000, "treasure_list");

	return 0;
}

void TreasureManager::fini()
{
	if (terasure_timer_)
	{
		game::timer()->cancel(terasure_timer_);
	}
	sTreasureList->fini();
}

void TreasureManager::self_player_load_treasure(Packet *pck)
{
	protocol::self::self_player_load msg;
	if (!pck->parse_protocol(msg))
	{
		return;
	}

	terminal_treasure_rob_fight_end(msg.data(), msg.name(), msg.id());
}


void TreasureManager::terminal_treasure_expand(const std::string &data, const std::string &name, int id)
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

	if (player->treasure_kc_num() >= TreasureOperation::TREASURE_EXPAND_BASE)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_price *t_price = sPlayerConfig->get_price(player->treasure_kc_num() + 1);
	if (!t_price)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < t_price->kc)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_price->kc, LOGWAY_TREASURE_KC);
	player->set_treasure_kc_num(player->treasure_kc_num() + 1);

	ResMessage::res_success(player, true, name, id);
}

void TreasureManager::terminal_treasure_enhance(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_enhance msg;
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
	PCK_CHECK_NO_GOLD;

	dhc::treasure_t *enhance_treasure = POOL_GET_TREASURE(msg.enhance_guid());
	if (!enhance_treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure *t_enhance_treasure = sTreasureConfig->get_treasure(enhance_treasure->template_id());
	if (!t_enhance_treasure)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_enhance_treasure->type == 5)
	{
		GLOBAL_ERROR;
		return;
	}
	if (enhance_treasure->enhance() >= player->level())
	{
		GLOBAL_ERROR;
		return;
	}
	if (enhance_treasure->enhance() >= sTreasureConfig->get_enhance_max())
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::treasure_t *treasure = 0;
	const s_t_treasure *t_treasure = 0;
	int treasure_enhance_exp = enhance_treasure->enhance_exp();
	int temp_exp = -1;
	int need_exp = 0;
	int real_exp = 0;
	int first_exp = enhance_treasure->enhance_exp();
	/// 计算该宝物总共能吞噬的经验
	for (int i = 0; i < msg.treasure_guids_size(); ++i)
	{
		treasure = POOL_GET_TREASURE(msg.treasure_guids(i));
		if (!treasure)
		{
			GLOBAL_ERROR;
			return;
		}
		if (treasure->guid() == enhance_treasure->guid())
		{
			GLOBAL_ERROR;
			return;
		}
		if (treasure->locked() == 1)
		{
			GLOBAL_ERROR;
			return;
		}
		if (treasure->role_guid() != 0)
		{
			GLOBAL_ERROR;
			return;
		}
		if (treasure->jilian() > 0)
		{
			GLOBAL_ERROR;
			return;
		}
		if (treasure->star() > 0)
		{
			GLOBAL_ERROR;
			return;
		}
		temp_exp = TreasureOperation::get_treasure_enhance_exp(player, treasure);
		if (temp_exp == -1)
		{
			GLOBAL_ERROR;
			return;
		}
		treasure_enhance_exp += temp_exp;
		need_exp += temp_exp;
	}

	int treasure_enhance = enhance_treasure->enhance();
	int treasure_color = t_enhance_treasure->color - 2;
	const s_t_treasure_enhance *t_enhance = 0;
	do 
	{
		if (treasure_enhance >= sTreasureConfig->get_enhance_max())
		{
			treasure_enhance_exp = 0;
			need_exp = real_exp - first_exp;
			break;
		}

		if (treasure_enhance >= player->level())
		{
			treasure_enhance_exp = 0;
			need_exp = real_exp - first_exp;
			break;
		}

		t_enhance = sTreasureConfig->get_enhance(treasure_enhance + 1);
		if (!t_enhance)
		{
			GLOBAL_ERROR;
			return;
		}

		if (treasure_color < 0 || treasure_color > t_enhance->attrs.size() - 1)
		{
			GLOBAL_ERROR;
			return;
		}

		if (treasure_enhance_exp >= t_enhance->attrs[treasure_color])
		{
			treasure_enhance_exp -= t_enhance->attrs[treasure_color];
			real_exp += t_enhance->attrs[treasure_color];
			treasure_enhance += 1;	
		}
		else
		{
			break;
		}
	} while (true);

	if (player->gold() < need_exp)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	for (int i = 0; i < msg.treasure_guids_size(); ++i)
	{
		TreasureOperation::treasure_destroy(player, msg.treasure_guids(i), LOGWAY_TREASURE_ENHANCE);
	}

	enhance_treasure->set_enhance_exp(treasure_enhance_exp);
	enhance_treasure->set_enhance(treasure_enhance);
	enhance_treasure->set_enhance_counts(enhance_treasure->enhance_counts() + need_exp);
	PlayerOperation::player_dec_resource(player, resource::GOLD, need_exp, LOGWAY_TREASURE_ENHANCE);
	PlayerOperation::player_add_active(player, 1700, 1);
	ResMessage::res_success(player, true, name, id);
}

void TreasureManager::terminal_treasure_jinlian(const std::string &data, const std::string &name, int id)
 {
	protocol::game::cmsg_treasure_jinlian msg;
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

	dhc::treasure_t *treasure = POOL_GET_TREASURE(msg.jinlian_guid());
	if (!treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	if (t_treasure->type == 5)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure_jinlian *t_jinlian = sTreasureConfig->get_jinlian(treasure->jilian() + 1);
	if (!t_jinlian)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, 50100001) < t_jinlian->item_num)
	{
		PERROR(ERROR_CAILIAO);
		return;
	}

	if (player->gold() < t_jinlian->gold)
	{
		PERROR(ERROR_GOLD);
		return;
	}

	if (msg.treasure_guid_size() != t_jinlian->baowu_num)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure *t_tun_treasure = 0;
	for (int i = 0; i < msg.treasure_guid_size(); ++i)
	{
		dhc::treasure_t *tun_treasure = POOL_GET_TREASURE(msg.treasure_guid(i));
		if (!tun_treasure)
		{
			GLOBAL_ERROR;
			return;
		}

		if (tun_treasure->guid() == treasure->guid())
		{
			GLOBAL_ERROR;
			return;
		}

		if (tun_treasure->locked() != 0)
		{
			GLOBAL_ERROR;
			return;
		}

		if (tun_treasure->enhance() >= 1)
		{
			GLOBAL_ERROR;
			return;
		}

		if (tun_treasure->jilian() >= 1)
		{
			GLOBAL_ERROR;
			return;
		}

		if (tun_treasure->star() >= 1)
		{
			GLOBAL_ERROR;
			return;
		}

		if (tun_treasure->template_id() != treasure->template_id())
		{
			GLOBAL_ERROR;
			return;
		}

		if (tun_treasure->role_guid() != 0)
		{
			GLOBAL_ERROR;
			return;
		}

		t_tun_treasure = sTreasureConfig->get_treasure(tun_treasure->template_id());
		if (!t_tun_treasure)
		{
			GLOBAL_ERROR;
			return;
		}

		if (t_tun_treasure->type == 5)
		{
			GLOBAL_ERROR;
			return;
		}
	}

	PlayerOperation::player_dec_resource(player, resource::GOLD, t_jinlian->gold, LOGWAY_TERASURE_JINLIAN);
	ItemOperation::item_destory_templete(player, 50100001, t_jinlian->item_num, LOGWAY_TERASURE_JINLIAN);

	for (int i = 0; i < msg.treasure_guid_size(); ++i)
	{
		TreasureOperation::treasure_destroy(player, msg.treasure_guid(i), LOGWAY_TERASURE_JINLIAN);
	}
	treasure->set_jilian(treasure->jilian() + 1);
	player->set_bwjl_task_num(player->bwjl_task_num() + 1);
	std::vector<uint64_t> treasures;
	ResMessage::res_treasure_jinlian(player, treasures, name, id);
}

void TreasureManager::terminal_treasure_lock(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_lock msg;
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

	dhc::treasure_t *treasure = POOL_GET_TREASURE(msg.treasure_guid());
	if (!treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	bool locked = (treasure->locked() == 0) ? false : true;
	if (locked == msg.locked())
	{
		GLOBAL_ERROR;
		return;
	}
	treasure->set_locked(msg.locked() ? 1 : 0);

	ResMessage::res_success(player, true, name, id);
}

void TreasureManager::terminal_treasure_hecheng(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_hecheng msg;
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

	if (TreasureOperation::treasure_is_full(player))
	{
		PERROR(ERROR_TREASURE_BAG_FULL);
		return;
	}

	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(msg.id());
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	for (std::vector<int>::size_type i = 0;
		i < t_treasure->suipian.size();
		++i)
	{
		if (ItemOperation::item_num_templete(player, t_treasure->suipian[i]) < 1)
		{
			PERROR(ERROR_SUIPIAN_NOT);
			return;
		}
	}

	dhc::treasure_t *treasure = TreasureOperation::treasure_create(player, t_treasure, 0, 0, LOGWAY_TREASURE_HECHENG);
	if (!treasure)
	{
		GLOBAL_ERROR;
		return;
	}
	if (game::timer()->run_day(player->birth_time()) <= 7)
	{
		player->add_treasure_hechengs(t_treasure->id);
	}

	for (std::vector<int>::size_type i = 0;
		i < t_treasure->suipian.size();
		++i)
	{
		ItemOperation::item_destory_templete(player, t_treasure->suipian[i], 1, LOGWAY_TREASURE_HECHENG);
	}
	if (t_treasure->color == 4)
	{
		sHuodongPool->huodong_active(player, HUODONG_COND_CSSP_COUNT, 1);
	}
	else if (t_treasure->color == 5)
	{
		sHuodongPool->huodong_active(player, HUODONG_COND_HSSP_COUNT, 1);
	}
	PlayerOperation::player_add_active(player, 1800, 1);
	ResMessage::res_treasure_hecheng(player, treasure, name, id);
}

void TreasureManager::terminal_treasure_star(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_star msg;
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
	PCK_CHECK_NO_GOLD;

	dhc::treasure_t *treasure = POOL_GET_TREASURE(msg.star_guid());
	if (!treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	if (msg.types_size() > treasure->star_rates_size())
	{
		GLOBAL_ERROR;
		return;
	}

	int star = treasure->star();
	int star_exp = treasure->star_exp();
	int var = treasure->star_var();
	int luck = treasure->star_luck();
	const s_t_treasure_sx *t_sx = 0;
	int gold = 0;
	int jewel = 0;
	for (int i = 0; i < msg.types_size(); ++i)
	{
		t_sx = sTreasureConfig->get_sx(star + 1, t_treasure->color);
		if (!t_sx)
		{
			GLOBAL_ERROR;
			return;
		}

		if (msg.types(i) == 1)
		{
			jewel += t_sx->jewel;
		}
		else
		{
			gold += t_sx->gold;
		}

		if (player->jewel() < jewel)
		{
			PERROR(ERROR_JEWEL);
			return;
		}
		if (player->gold() < gold)
		{
			PERROR(ERROR_GOLD);
			return;
		}

		if (treasure->star_rates(i) <= (t_sx->rate + luck / 10))
		{
			star_exp += 5 * treasure->star_bjs(i) / 100;
			var += 5 * t_sx->value1 * treasure->star_bjs(i) / 100;
		}
		else
		{
			luck += 10;
		}

		if (star_exp >= t_sx->jindu)
		{
			star_exp = 0;
			star++;
			luck = 0;
			var = t_sx->valuemax;

			/// 满级
			if (sTreasureConfig->get_sx(star + 1, t_treasure->color) == 0)
			{
				break;
			}
		}
	}

	PlayerOperation::player_dec_resource(player, resource::GOLD, gold, LOGWAY_TREASURE_STAR);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, jewel, LOGWAY_TREASURE_STAR);
	treasure->set_star(star);
	treasure->set_star_exp(star_exp);
	treasure->set_star_luck(luck);
	treasure->set_star_var(var);
	treasure->set_star_gold(treasure->star_gold() + gold);
	treasure->set_star_jewel(treasure->star_jewel() + jewel);

	int new_bj = 100;
	for (int i = 0; i < msg.types_size(); ++i)
	{
		treasure->set_star_rates(i, Utils::get_int32(1, 100));
		new_bj = Utils::get_int32(0, 99);
		if (new_bj < 5)
			treasure->set_star_bjs(i, 300);
		else if (new_bj < 15)
			treasure->set_star_bjs(i, 200);
		else if (new_bj < 40)
			treasure->set_star_bjs(i, 150);
		else
			treasure->set_star_bjs(i, 100);
	}

	protocol::game::smsg_treasure_star smsg;
	for (int i = 0; i < treasure->star_rates_size(); ++i)
	{
		smsg.add_rates(treasure->star_rates(i));
		smsg.add_bjs(treasure->star_bjs(i));
	}

	ResMessage::res_treasure_star(player, smsg, name, id);
}

void TreasureManager::terminal_treasure_init(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_init msg;
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

	dhc::treasure_t *treasure = POOL_GET_TREASURE(msg.treasure_guids());
	if (!treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	if (treasure->locked())
	{
		GLOBAL_ERROR;
		return;
	}

	if (treasure->role_guid() > 0)
	{
		GLOBAL_ERROR;
		return;
	}

	if (player->jewel() < 50)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	s_t_rewards rds;
	if (!TreasureOperation::get_enhance_return(player, treasure, rds))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!TreasureOperation::get_jilian_return(player, treasure, rds))
	{
		GLOBAL_ERROR;
		return;
	}

	if (!TreasureOperation::get_star_return(player, treasure, rds))
	{
		GLOBAL_ERROR;
		return;
	}
	rds.merge();

	PlayerOperation::player_add_reward(player, rds, LOGWAY_TREASURE_INIT);
	PlayerOperation::player_dec_resource(player, resource::JEWEL, 50, LOGWAY_TREASURE_INIT);

	treasure->set_enhance(0);
	treasure->set_enhance_exp(0);
	treasure->set_jilian(0);
	treasure->set_enhance_counts(0);
	treasure->set_star(0);
	treasure->set_star_exp(0);
	treasure->set_star_gold(0);
	treasure->set_star_jewel(0);
	treasure->set_star_var(0);
	treasure->set_star_luck(0);
	TreasureOperation::treasure_refresh_rate(treasure, t_treasure);

	protocol::game::smsg_treasure_init msg1;
	ADD_MSG_REWARD(msg1, rds);
	msg1.add_treasures()->CopyFrom(*treasure);

	ResMessage::res_treasure_init(player, msg1, name, id);
}

void TreasureManager::terminal_treasure_equip(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_equip msg;
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

	if (msg.index_size() != msg.treasure_guid_size())
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::role_t *role = POOL_GET_ROLE(msg.role_guid());
	if (!role)
	{
		GLOBAL_ERROR;
		return;
	}
	
	int index = 0;
	uint64_t treasure_guid = 0;
	dhc::treasure_t *on_treasure = 0;
	dhc::treasure_t *off_treasure = 0;
	const s_t_treasure *t_treasure = 0;

	for (int i = 0; i < msg.index_size(); ++i)
	{
		index = msg.index(i);
		if (index < 0 || index >= role->treasures_size())
		{
			GLOBAL_ERROR;
			return;
		}
		treasure_guid = msg.treasure_guid(i);
		if (role->treasures(index) == treasure_guid)
		{
			GLOBAL_ERROR;
			return;
		}
		
		/// 要装备的宝物
		if (treasure_guid > 0)
		{
			on_treasure = POOL_GET_TREASURE(treasure_guid);
			if (!on_treasure)
			{
				GLOBAL_ERROR;
				return;
			}
			if (on_treasure->role_guid() > 0)
			{
				GLOBAL_ERROR;
				return;
			}
			if (on_treasure->locked() != 0)
			{
				GLOBAL_ERROR;
				return;
			}
			t_treasure = sTreasureConfig->get_treasure(on_treasure->template_id());
			if (!t_treasure)
			{
				GLOBAL_ERROR;
				return;
			}
			if (t_treasure->type == 5)
			{
				GLOBAL_ERROR;
				return;
			}
			if (t_treasure->type - 1 != index)
			{
				GLOBAL_ERROR;
				return;
			}
		}

		/// 要卸下的宝物
		if (role->treasures(index) > 0)
		{
			off_treasure = POOL_GET_TREASURE(role->treasures(index));
			if (!off_treasure)
			{
				GLOBAL_ERROR;
				return;
			}
			off_treasure->set_role_guid(0);
			role->set_treasures(index, 0);
		}

		if (treasure_guid > 0)
		{
			on_treasure->set_role_guid(role->guid());
			role->set_treasures(index, on_treasure->guid());
		}
	}

	ResMessage::res_success(player, true, name, id);
}

void TreasureManager::terminal_treasure_ronglian(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_ronglian msg;
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

	if (player->jewel() < 250)
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	dhc::treasure_t* treasure = POOL_GET_TREASURE(msg.treasure_guid());
	if (!treasure)
	{
		GLOBAL_ERROR;
		return;
	}
	if (treasure->enhance() > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (treasure->jilian() > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (treasure->role_guid() != 0)
	{
		GLOBAL_ERROR;
		return;
	}
	if (treasure->locked())
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure* t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_treasure->color != 4)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure_suipian* t_suipian = sTreasureConfig->get_suipian(msg.suipian_id());
	if (!t_suipian)
	{
		GLOBAL_ERROR;
		return;
	}
	const s_t_treasure* t_treasure_suipian = sTreasureConfig->get_treasure(t_suipian->treasure_id);
	if (!t_treasure_suipian)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_treasure_suipian->color != 5)
	{
		GLOBAL_ERROR;
		return;
	}
	dhc::treasure_list_t* treasure_list = POOL_GET_TREASURE_LIST(MAKE_GUID(et_treasure_list, t_suipian->ordernum));
	if (!treasure_list)
	{
		GLOBAL_ERROR;
		return;
	}

	PlayerOperation::player_dec_resource(player, resource::JEWEL, 250, LOGWAY_TREASURE_RONGLIAN);
	ItemOperation::item_add_template(player, msg.suipian_id(), 1, LOGWAY_TREASURE_RONGLIAN);
	TreasureOperation::treasure_destroy(player, treasure, LOGWAY_TREASURE_RONGLIAN);

	ResMessage::res_success(player, true, name, id);
}

void TreasureManager::terminal_treasure_zhuzao(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_zhuzao msg;
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

	dhc::treasure_t* treasure = POOL_GET_TREASURE(msg.treasure_guid());
	if (!treasure)
	{
		GLOBAL_ERROR;
		return;
	}
	if (treasure->star() > 0)
	{
		GLOBAL_ERROR;
		return;
	}
	const s_t_treasure* t_treasure = sTreasureConfig->get_treasure(treasure->template_id());
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}
	if (t_treasure->color != 4)
	{
		GLOBAL_ERROR;
		return;
	}
	const s_t_treasure* t_treasure_zhuzao = sTreasureConfig->get_treasure(treasure->template_id() + 1000);
	if (!t_treasure_zhuzao)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure_jinlian* t_jinlian = 0;
	int baowu_num = 0;
	for (int i = 1; i <= treasure->jilian(); ++i)
	{
		t_jinlian = sTreasureConfig->get_jinlian(i);
		if (!t_jinlian)
		{
			GLOBAL_ERROR;
			return;
		}
		baowu_num += t_jinlian->baowu_num;
	}
	
	if (player->jewel() < (550 + 550 * baowu_num))
	{
		PERROR(ERROR_JEWEL);
		return;
	}

	int exp = TreasureOperation::get_treasure_enhance_pure_exp(player, treasure);
	if (exp == -1)
	{
		GLOBAL_ERROR;
		return;
	}

	int color = t_treasure_zhuzao->color - 2;
	int enhance_level = 0;
	const s_t_treasure_enhance* t_enhance = 0;
	do 
	{
		if (enhance_level >= sTreasureConfig->get_enhance_max())
		{
			exp = 0;
			break;
		}
		if (enhance_level >= player->level())
		{
			exp = 0;
			break;
		}

		t_enhance = sTreasureConfig->get_enhance(enhance_level + 1);
		if (!t_enhance)
		{
			GLOBAL_ERROR;
			return;
		}
		if (color < 0 || color > t_enhance->attrs.size() - 1)
		{
			GLOBAL_ERROR;
			return;
		}
		if (exp >= t_enhance->attrs[color])
		{
			enhance_level += 1;
			exp -= t_enhance->attrs[color];
		}
		else
		{
			break;
		}
	} while (true);
	
	PlayerOperation::player_dec_resource(player, resource::JEWEL, 550 + 550 * baowu_num, LOGWAY_TREASURE_ZHUZAO);
	treasure->set_enhance(enhance_level);
	treasure->set_enhance_exp(exp);
	treasure->set_template_id(treasure->template_id() + 1000);

	ResMessage::res_treasure_zhuzao(player, enhance_level, exp, name, id);
}


void TreasureManager::terminal_treasure_rob_view(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_rob_view msg;
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

	if (ItemOperation::item_num_templete(player, msg.treasure_suipian()) > 0)
	{
		GLOBAL_ERROR;
		return;
	}

	protocol::game::smsg_treasure_rob_view msg1;
	sTreasureList->get_rob_player_list(player, msg.treasure_suipian(), msg1);
	ResMessage::res_treasure_view(player, msg1, name, id);
}

void TreasureManager::terminal_treasure_rob_protect(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_protect msg;
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

	if (msg.type() < 0 || msg.type() > 1)
	{
		GLOBAL_ERROR;
		return;
	}

	/// 免战中
	if (game::timer()->now() < player->treasure_protect_next_time())
	{
		GLOBAL_ERROR;
		return;
	}

	/// CD中
	if (game::timer()->now() < player->treasure_protect_cd_time())
	{
		GLOBAL_ERROR;
		return;
	}

	int item_id = (msg.type() == 1) ? 50050002 : 50050001;
	const s_t_item *t_item = sItemConfig->get_item(item_id);
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}

	if (ItemOperation::item_num_templete(player, item_id) < 1)
	{
		GLOBAL_ERROR;
		return;
	}

	ItemOperation::item_destory_templete(player, item_id, 1, LOGWAY_TREASURE_PROTEXT);
	player->set_treasure_protect_next_time(game::timer()->now() + t_item->def1 * 60 * 60 * 1000);
	player->set_treasure_protect_cd_time(game::timer()->now() + t_item->def2 * 60 * 60 * 1000);
	sTreasureList->update_rob_player_list(player);
	ResMessage::res_treasure_protect(player, name, id);
}

void TreasureManager::terminal_treasure_rob_buy(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_buy msg;
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

	if (msg.type() < 0 || msg.type() > 1)
	{
		GLOBAL_ERROR;
		return;
	}

	int item_id = (msg.type() == 1) ? 50050002 : 50050001;
	const s_t_item *t_item = sItemConfig->get_item(item_id);
	if (!t_item)
	{
		GLOBAL_ERROR;
		return;
	}
	if (player->jewel() < t_item->def3)
	{
		PERROR(ERROR_JEWEL);
		return;
	}
	PlayerOperation::player_dec_resource(player, resource::JEWEL, t_item->def3, LOGWAY_TREASURE_BUY);
	ItemOperation::item_add_template(player, item_id, 1, LOGWAY_TREASURE_BUY);

	ResMessage::res_success(player, true, name, id);
}

void TreasureManager::terminal_treasure_rob_fight_end(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_fight_end msg;
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

	const RobPlayerList *rplist = sTreasureList->get_rob_player_list(player);
	if (!rplist)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure_suipian *t_suipian = sTreasureConfig->get_suipian(rplist->get_suipian());
	if (!t_suipian)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(t_suipian->treasure_id);
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	/// 判断是否已有碎片
	if (ItemOperation::item_num_templete(player, rplist->get_suipian()) > 0)
	{
		PERROR(ERROR_TREASURE_SUIPIAN_FULL);
		return;
	}

	// 对方
	RobPlayer *rp_target = sTreasureList->get_rob_player(player, msg.player_guid());
	if (!rp_target)
	{
		GLOBAL_ERROR;
		return;
	}

	dhc::player_t *target = 0;
	/// NPC
	if (rp_target->is_npc_)
	{
		target = sTreasureList->get_rob_fight_npc(player);
		target->set_guid(rp_target->guid_);
		target->set_name(rp_target->name_);
		target->set_template_id(rp_target->template_id_);
		target->set_level(rp_target->level_);
		target->set_bf(rp_target->bf_);
	}
	else
	{
		target = POOL_GET_PLAYER(rp_target->guid_);
		if (!target)
		{
			protocol::self::self_player_load msg;
			msg.set_data(data);
			msg.set_name(name);
			msg.set_id(id);
			std::string s;
			msg.SerializeToString(&s);
			sPlayerLoad->load_player(rp_target->guid_, SELF_PLAYER_LOAD_TREASURE, s);
			return;
		}
		else
		{
			if (game::timer()->now() < target->treasure_protect_next_time())
			{
				PERROR(ERROR_TREASURE_PROTECT);
				return;
			}
			if (__get_treasure_suipian_count(target, t_treasure) > 1 &&
				ItemOperation::item_num_templete(target, rplist->get_suipian()) > 0)
			{

			}
			else
			{
				PERROR(ERROR_FIGHT_TREASURE);
				return;
			}
			game::channel()->refresh_offline_time(rp_target->guid_);
		}
	}

	if (game::timer()->now() < player->treasure_protect_next_time())
	{
		player->set_treasure_protect_next_time(0);
		sTreasureList->update_rob_player_list(player);
	}

	if (player->treasure_first() == 0)
	{
		player->set_treasure_first(1);
		sTreasureList->update_rob_player_list(player);
	}

	s_t_rewards rds;
	rds.add_reward(1, resource::EXP, 12);
	rds.add_reward(1, resource::GOLD, 1000 + player->level() * 40);
	int supian_id = 0;
	std::string text;
	int result = MissionFight::mission_rob(player, target, text);
	if (result == 1)
	{
		/// 抢到碎片
		if (Utils::get_int32(0, 99) < rp_target->rate_)
		{
			/// 玩家
			if (!rp_target->is_npc_ && target)
			{
				if (__get_treasure_suipian_count(target, t_treasure) > 1 &&
					ItemOperation::item_num_templete(target, rplist->get_suipian()) > 0)
				{
					supian_id = rplist->get_suipian();
					ItemOperation::item_destory_templete(target, supian_id, 1, LOGWAY_TREASURE_ROB);
					ItemOperation::item_add_template(player, supian_id, 1, LOGWAY_TREASURE_ROB);
					TreasureOperation::create_treasure_report(target, player, rplist->get_suipian(), 2);
					sTreasureList->update_rob_suipian_list(target->guid(), supian_id);
				}
				else
				{
					/// 没有碎片了
					PERROR(ERROR_FIGHT_TREASURE);
					return;
				}
			}
			/// NPC
			else
			{
				supian_id = rplist->get_suipian();
				ItemOperation::item_add_template(player, supian_id, 1, LOGWAY_TREASURE_ROB);
			}
		}
	}
	else
	{
		if (!rp_target->is_npc_ && target)
		{
			TreasureOperation::create_treasure_report(target, player, rplist->get_suipian(), 1);
		}
	}

	/// 翻牌
	int cid = sSportConfig->refresh_sport_card(player->level(), sHuodongPool->has_jieri_huodong(player));
	int pgold = 0;
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
			PlayerOperation::player_add_reward(player, prds, LOGWAY_TREASURE_ROB);
		}
	}
	PlayerOperation::player_dec_resource(player, resource::ENERGY, 2, LOGWAY_TREASURE_ROB);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TREASURE_ROB);
	sHuodongPool->huodong_active(player, HUODONG_COND_ROB_COUNT, 1);
	RoleOperation::check_bskill_count(player, BSKILL_TYPE4, 1);
	ResMessage::res_treasure_fight_end(player, rds, supian_id, result, cid, pgold, text, name, id);
}

void TreasureManager::terminal_treasure_saodang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_saodang msg;
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

	PlayerOperation::player_do_energy(player);

	RobPlayerList *rplist = sTreasureList->get_rob_player_list(player);
	if (!rplist)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure_suipian *t_suipian = sTreasureConfig->get_suipian(rplist->get_suipian());
	if (!t_suipian)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(t_suipian->treasure_id);
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	/// 判断是否已有碎片
	if (ItemOperation::item_num_templete(player, rplist->get_suipian()) > 0)
	{
		PERROR(ERROR_TREASURE_SUIPIAN_FULL);
		return;
	}

	RobPlayer *rp = sTreasureList->get_rob_player(player, msg.player_guid());
	if (!rp)
	{
		GLOBAL_ERROR;
		return;
	}

	int saodang_num = player->energy() / 2;
	if (saodang_num <= 0)
	{
		PERROR(ERROR_ENERGY);
		return;
	}
	if (saodang_num > 5)
	{
		saodang_num = 5;
	}

	if (!rp->is_npc_)
	{
		PERROR(ERROR_TREASURE_SAODANG);
		return;
	}

	/// 扫荡
	protocol::game::smsg_treasure_saodang msg1;
	protocol::game::smsg_treasure_fight_end *reward = 0;
	int cid = 0;
	s_t_rewards rds;
	for (int i = 0; i < saodang_num; ++i)
	{
		sHuodongPool->huodong_active(player, HUODONG_COND_ROB_COUNT, 1);
		RoleOperation::check_bskill_count(player, BSKILL_TYPE4, 1);
		PlayerOperation::player_dec_resource(player, resource::ENERGY, 2, LOGWAY_TREASURE_ROB_SAODANG);

		reward = msg1.add_rewards();
		reward->set_result(0);
		reward->set_text("");
		reward->add_types(1);
		reward->add_value1s(resource::GOLD);
		reward->add_value2s(player->level() * 40);
		reward->add_value3s(0);
		rds.add_reward(1, resource::GOLD, player->level() * 40);
		reward->add_types(1);
		reward->add_value1s(resource::EXP);
		reward->add_value2s(12);
		reward->add_value3s(0);
		rds.add_reward(1, resource::EXP, 12);

		cid = sSportConfig->refresh_sport_card(player->level(), sHuodongPool->has_jieri_huodong(player));
		if (cid > 0)
		{
			const s_t_sport_card * t_card = sSportConfig->get_sport_shop_card(cid);
			if (t_card)
			{
				if (t_card->reward.type == 1 &&
					t_card->reward.value1 == resource::GOLD)
				{
					reward->set_pgold(Utils::get_int32(player->level() * 20, player->level() * 30));
					rds.add_reward(1, resource::GOLD, reward->pgold());
				}
				else
				{
					reward->set_card(cid);
					rds.add_reward(t_card->reward);
				}
			}
		}

		if (Utils::get_int32(0, 99) < rp->rate_)
		{
			reward->set_suipian_id(rplist->get_suipian());
			rds.add_reward(2, rplist->get_suipian(), 1);
			break;
		}
	}

	if (player->treasure_first() == 0)
	{
		player->set_treasure_first(1);
		sTreasureList->update_rob_player_list(player);
	}

	PlayerOperation::player_add_reward(player, rds, LOGWAY_TREASURE_ROB_SAODANG);
	ResMessage::res_treasure_saodang(player, msg1, name, id);
}

void TreasureManager::terminal_treasure_yi_saodang(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_yijian_saodang msg;
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

	PlayerOperation::player_do_energy(player);

	if (!msg.use_yaosui())
	{
		if (player->energy() < 2)
		{
			PERROR(ERROR_ENERGY);
			return;
		}
	}

	const s_t_treasure *t_treasure = sTreasureConfig->get_treasure(msg.treasure_id());
	if (!t_treasure)
	{
		GLOBAL_ERROR;
		return;
	}

	const s_t_treasure_suipian *t_suipian = 0;
	std::list<const s_t_treasure_suipian*> suipians;
	for (std::vector<int>::size_type i = 0;
		i < t_treasure->suipian.size();
		++i)
	{
		if (ItemOperation::item_num_templete(player, t_treasure->suipian[i]) <= 0)
		{
			t_suipian = sTreasureConfig->get_suipian(t_treasure->suipian[i]);
			if (!t_suipian)
			{
				GLOBAL_ERROR;
				return;
			}
			suipians.push_back(t_suipian);
		}
	}
	if (suipians.empty())
	{
		GLOBAL_ERROR;
		return;
	}

	/// 扫荡
	protocol::game::smsg_treasure_yijian_saodang msg1;
	msg1.set_success(true);
	protocol::game::smsg_treasure_fight_end *reward = 0;
	int cid = 0;
	s_t_rewards rds;
	int saodang_count = 0;
	do 
	{
		if (suipians.empty())
		{
			break;
		}

		t_suipian = suipians.back();

		if (player->energy() < 2)
		{
			if (msg.use_yaosui() && ItemOperation::item_num_templete(player, 10010007) > 0)
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
				PlayerOperation::player_add_reward(player, rdapply, LOGWAY_TREASURE_ROB_YIJIANSAODANG);
				msg1.set_yaosui_num(msg1.yaosui_num() + 1);
				ItemOperation::item_destory_templete(player, 10010007, 1, LOGWAY_TREASURE_ROB_YIJIANSAODANG);
			}
		}

		if (player->energy() < 2)
		{
			msg1.set_success(false);
			break;
		}
		PlayerOperation::player_dec_resource(player, resource::ENERGY, 2, LOGWAY_TREASURE_ROB_YIJIANSAODANG);
		saodang_count += 1;

		reward = msg1.add_rewards();
		reward->set_result(0);
		reward->set_text("");
		reward->add_types(1);
		reward->add_value1s(resource::GOLD);
		reward->add_value2s(player->level() * 40);
		reward->add_value3s(0);
		rds.add_reward(1, resource::GOLD, player->level() * 40);
		reward->add_types(1);
		reward->add_value1s(resource::EXP);
		reward->add_value2s(12);
		reward->add_value3s(0);
		rds.add_reward(1, resource::EXP, 12);

		cid = sSportConfig->refresh_sport_card(player->level(), sHuodongPool->has_jieri_huodong(player));
		if (cid > 0)
		{
			const s_t_sport_card * t_card = sSportConfig->get_sport_shop_card(cid);
			if (t_card)
			{
				if (t_card->reward.type == 1 &&
					t_card->reward.value1 == resource::GOLD)
				{
					reward->set_pgold(Utils::get_int32(player->level() * 20, player->level() * 30));
					rds.add_reward(1, resource::GOLD, reward->pgold());
				}
				else
				{
					reward->set_card(cid);
					rds.add_reward(t_card->reward);
				}
			}
		}

		if (Utils::get_int32(0, 99) < t_suipian->npc_rate)
		{
			reward->set_suipian_id(t_suipian->template_id);
			rds.add_reward(2, t_suipian->template_id, 1);
			suipians.pop_back();
		}
	} while (true);

	if (player->treasure_first() == 0)
	{
		player->set_treasure_first(1);
		sTreasureList->update_rob_player_list(player);
	}

	sHuodongPool->huodong_active(player, HUODONG_COND_ROB_COUNT, saodang_count);
	RoleOperation::check_bskill_count(player, BSKILL_TYPE4, saodang_count);
	PlayerOperation::player_add_reward(player, rds, LOGWAY_TREASURE_ROB_YIJIANSAODANG);
	ResMessage::res_treasure_yijian_saodang(player, msg1, name, id);
}

void TreasureManager::terminal_treasure_point(const std::string &data, const std::string &name, int id)
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

	int has_new = 0;
	for (int i = 0; i < player->treasure_reports_size(); ++i)
	{
		dhc::treasure_report_t *treasure_report = POOL_GET_TREASURE_REPORT(player->treasure_reports(i));
		if (treasure_report)
		{
			if (treasure_report->new_report() == 0)
			{
				has_new = 1;
				break;
			}
		}
	}

	ResMessage::res_treasure_point(player, has_new, name, id);
}

void TreasureManager::terminal_treasure_report(const std::string &data, const std::string &name, int id)
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

	std::vector<int> suipian_ids;
	sTreasureList->get_rob_suipian_list(player->guid(), suipian_ids);
	sTreasureList->remove_rob_suipian_list(player->guid());

	ResMessage::res_treasure_report(player, suipian_ids, name, id);
}


void TreasureManager::terminal_treasure_report_ex(const std::string &data, const std::string &name, int id)
{
	protocol::game::cmsg_treasure_report_ex msg;
	if (!msg.ParseFromString(data))
	{
		GLOBAL_ERROR;
		return;
	}

	uint64_t player_guid = msg.comm().guid();
	dhc::player_t *player = POOL_GET_PLAYER(player_guid);
	if (!player)
	{
		GLOBAL_ERROR;
		return;
	}
	PCK_CHECK_EX;

	std::vector<int> suipian_ids;
	sTreasureList->get_rob_suipian_list(player->guid(), suipian_ids);
	sTreasureList->remove_rob_suipian_list(player->guid());

	ResMessage::res_treasure_report_ex(player, suipian_ids, name, id);
}