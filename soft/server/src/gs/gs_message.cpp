#include "gs_message.h"
#include "item_def.h"
#include "post_pool.h"
#include "global_pool.h"
#include "huodong_pool.h"
#include "social_operation.h"
#include "rpc.pb.h"

void ResMessage::res_last(const std::string &pck, const std::string &name, int id)
{
	game::rpc_service()->response(name, id, pck);
}

void ResMessage::res_client_login(dhc::player_t *player, int is_new, const std::string &name, int id)
{
	protocol::game::smsg_client_login msg;
	TermInfo *ti = game::channel()->get_channel(player->guid());
	if (ti)
	{
		msg.set_sig(ti->sig);
		msg.set_device(ti->device);
		msg.set_device_time(ti->device_time);
	}
	else
	{
		msg.set_sig("");
		msg.set_device("");
		msg.set_device_time(0);
	}

	msg.set_guid(player->guid());
	msg.mutable_player()->CopyFrom(*player);
	msg.set_is_new(is_new);

	for (int i = 0; i < player->roles_size(); ++i)
	{
		uint64_t role_guid = player->roles(i);
		dhc::role_t *role = POOL_GET_ROLE(role_guid);
		if (role)
		{
			msg.add_roles()->CopyFrom(*role);
		}
	}

	for (int i = 0; i < player->equips_size(); ++i)
	{
		uint64_t equip_guid = player->equips(i);
		dhc::equip_t *equip = POOL_GET_EQUIP(equip_guid);
		if (equip)
		{
			msg.add_equips()->CopyFrom(*equip);
		}
	}
	for (int i = 0; i < player->treasures_size(); ++i)
	{
		if (player->treasures(i) > 0)
		{
			dhc::treasure_t *treasure = POOL_GET_TREASURE(player->treasures(i));
			if (treasure)
			{
				msg.add_treasures()->CopyFrom(*treasure);
			}
		}
	}
	for (int i = 0; i < player->pets_size(); ++i)
	{
		dhc::pet_t *pet = POOL_GET_PET(player->pets(i));
		if (pet)
		{
			msg.add_pets()->CopyFrom(*pet);
		}
	}
	msg.set_server_time(game::timer()->now());
	msg.set_start_time(game::gtool()->start_time());

	std::list<protocol::game::smsg_chat *> msgs;
	SocialOperation::get_chat(msgs);
	for (std::list<protocol::game::smsg_chat *>::iterator it = msgs.begin(); it != msgs.end(); ++it)
	{
		protocol::game::smsg_chat *msg1 = *it;
		msg.add_chats()->CopyFrom(*msg1);
	}

	if (player->guild())
	{
		std::list<protocol::game::smsg_chat *> msgs1;
		SocialOperation::get_chat(player->guild(), msgs1);
		for (std::list<protocol::game::smsg_chat *>::iterator it = msgs1.begin(); it != msgs1.end(); ++it)
		{
			protocol::game::smsg_chat *msg1 = *it;
			msg.add_guild_chats()->CopyFrom(*msg1);
		}

		dhc::global_t* guild_global = POOL_GET_GLOBAL(MAKE_GUID(et_global, 0));
		if (guild_global)
		{
			msg.set_guild_time(guild_global->guild_refresh_time());
		}
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void ResMessage::res_chat(const std::string &name, int id)
{
	protocol::game::smsg_success msg;
	msg.set_success(true);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void ResMessage::res_success(dhc::player_t *player, bool suc, const std::string &name, int id)
{
	protocol::game::smsg_success msg;
	msg.set_success(suc);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_success_ex(bool suc, const std::string &name, int id)
{
	protocol::game::smsg_success msg;
	msg.set_success(suc);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void ResMessage::res_player_name(dhc::player_t *player, int suc, const std::string &name, int id)
{
	protocol::game::smsg_player_name msg;
	msg.set_code_stat(suc);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

//void ResMessage::res_success_googlebind(dhc::player_t *player, const protocol::game::smsg_player_name& msg, const std::string &name, int id)
//{
	//std::string s;
	//msg.SerializeToString(&s);
	//game::rpc_service()->response(name, id, s);
//	game::channel()->set_last_pck(player->guid(), s);
//}

void ResMessage::res_mission_fight_end(dhc::player_t *player, int result, const std::string &text, int star, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_mission_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	msg.set_star(star);
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_mission_saodang(dhc::player_t *player, const std::vector<s_t_rewards> &rewards, const std::string &name, int id)
{
	protocol::game::smsg_mission_saodang msg;
	for (std::vector<s_t_rewards>::size_type k = 0; k < rewards.size(); ++k)
	{
		protocol::game::smsg_mission_fight_end *msg1 = msg.add_saodangs();
		msg1->set_result(0);
		msg1->set_text("");
		msg1->set_star(0);

		const s_t_rewards &rds = rewards[k];
		for (std::vector<s_t_reward>::size_type i = 0; i < rds.rewards.size(); ++i)
		{
			msg1->add_types(rds.rewards[i].type);
			msg1->add_value1s(rds.rewards[i].value1);
			msg1->add_value2s(rds.rewards[i].value2);
			msg1->add_value3s(rds.rewards[i].value3);
		}
		for (std::vector<dhc::equip_t*>::size_type i = 0; i < rds.equips.size(); ++i)
		{
			msg1->add_equips()->CopyFrom(*(rds.equips[i]));
		}
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_mission_first(dhc::player_t *player, const protocol::game::smsg_mission_first& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_chouka(dhc::player_t *player, const protocol::game::smsg_chouka& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_duihuan(dhc::player_t *player, dhc::role_t *role, const std::string &name, int id)
{
	protocol::game::smsg_role_duihuan msg;
	msg.mutable_role()->CopyFrom(*role);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_zhanpu(dhc::player_t* player, const std::string &name, int id)
{
	protocol::game::smsg_role_huiyi_zhanpu msg;
	for (int i = 0; i < player->huiyi_zhanpus_size(); ++i)
	{
		msg.add_ids(player->huiyi_zhanpus(i));
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_pet_duihuan(dhc::player_t *player, dhc::pet_t *pet, const std::string &name, int id)
{
	protocol::game::smsg_pet_duihuan msg;
	msg.mutable_pet()->CopyFrom(*pet);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_xq_look(dhc::player_t *player, const std::vector<uint64_t> &guids, const std::vector<int> &xqs,const std::string &name, int id)
{
	protocol::game::smsg_role_xq_look msg;

	for (int i = 0; i < guids.size(); i++)
	{
		msg.add_guid(guids[i]);
		msg.add_xq(xqs[i]);
	}
	
	msg.set_jb(player->gold());
	msg.set_zs(player->jewel());

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_yh_look(dhc::player_t *player, const std::string &name, int id)
{
	protocol::game::smsg_role_yh_look msg;

	for (int i = 0; i < player->yh_roles_size(); i++)
	{
		msg.add_guid(player->yh_roles(i));
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_yh_select(dhc::player_t *player, uint64_t guid, int index, int jewel, const std::string &name, int id)
{
	protocol::game::smsg_role_yh_select msg;

	msg.set_guid(guid);
	msg.set_xq(index);
	msg.set_jewel(jewel);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_init(dhc::player_t *player, const protocol::game::smsg_role_init& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_pet_init(dhc::player_t *player, const protocol::game::smsg_pet_init& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_huiyi_chou(dhc::player_t* player, const protocol::game::smsg_role_huiyi_chou& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_huiyi_rank(dhc::player_t* player, const protocol::game::smsg_role_huiyi_rank& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_shop_refresh(dhc::player_t *player, const std::string &name, int id)
{
	protocol::game::smsg_shop_refresh msg;
	dhc::guild_t *guild = POOL_GET_GUILD(player->guild());
	if (guild)
	{
		for (int i = 0; i < player->shop1_ids_size(); ++i)
		{
			msg.add_shop1_ids(player->shop1_ids(i));
			msg.add_shop1_sell(player->shop1_sell(i));
		}
	}

	for (int i = 0; i < player->shop2_ids_size(); ++i)
	{
		msg.add_shop2_ids(player->shop2_ids(i));
		msg.add_shop2_sell(player->shop2_sell(i));
	}

	for (int i = 0; i < player->shop4_ids_size(); ++i)
	{
		msg.add_shop4_ids(player->shop4_ids(i));
		msg.add_shop4_sell(player->shop4_sell(i));
	}

	if (guild)
	{
		for (int i = 0; i < guild->shop_ids_size(); ++i)
		{
			msg.add_guild_shop_ids(guild->shop_ids(i));
			msg.add_guild_shop_sell(guild->shop_nums(i));
		}
	}

	for (int i = 0; i < player->shoppet_ids_size(); ++i)
	{
		msg.add_shoppet_ids(player->shoppet_ids(i));
		msg.add_shoppet_sell(player->shoppet_sell(i));
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_item_apply(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_item_apply msg;
	ADD_MSG_REWARD_WITH_PET(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_shop_buy(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_shop_buy msg;
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_post_look(dhc::player_t *player, const std::vector<dhc::post_t *> posts, const std::string &name, int id)
{
	protocol::game::smsg_post_look msg;
	for (int i = 0; i < posts.size(); ++i)
	{
		msg.add_posts()->CopyFrom(*posts[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_post_get(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_post_get msg;
	ADD_MSG_REWARD_WITH_PET(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_active_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_active_reward msg;
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_active_score_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_active_score_reward msg;
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_post_view(dhc::player_t *player, uint64_t post_guid, const std::string &name, int id)
{
	/*protocol::game::smsg_post_view msg;
	msg.set_post_guid(post_guid);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);*/
}

void ResMessage::res_equip_gaizao(dhc::player_t *player, dhc::equip_t *equip, int num, const std::string &name, int id)
{
	protocol::game::smsg_equip_gaizao msg;
	msg.mutable_equip()->CopyFrom(*equip);
	msg.set_num(num);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_sport_look(dhc::player_t *player, const std::vector<protocol::game::msg_sport_player> &players
	, int rank, int last_rank, int can_get, const std::string &name, int id)
{
	protocol::game::smsg_sport_look msg;
	for (int i = 0; i < players.size(); ++i)
	{
		msg.add_players()->CopyFrom(players[i]);
	}
	msg.set_rank(rank);
	msg.set_last_rank(last_rank);
	msg.set_can_get(can_get);
	msg.set_max_rank(player->max_rank());
	
	for (int i = 0; i < player->sports_size(); ++i)
	{
		uint64_t sport_guid = player->sports(i);
		dhc::sport_t *sport = POOL_GET_SPORT(sport_guid);
		if (sport)
		{
			msg.add_sports()->CopyFrom(*sport);
		}
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_sport_top(dhc::player_t *player, const std::vector<protocol::game::msg_sport_player> &players, const std::string &name, int id)
{
	protocol::game::smsg_sport_top msg;
	for (int i = 0; i < players.size(); ++i)
	{
		msg.add_players()->CopyFrom(players[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_sport_shop_list(dhc::player_t *player, const std::string &name, int id)
{
	protocol::game::smsg_sport_shop_list msg;
	for (int i = 0; i < player->shop4_ids_size(); ++i)
	{
		msg.add_shop_ids(player->shop4_ids(i));
		msg.add_shop_sells(player->shop4_sell(i));
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_sport_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, int cid, int pgold, const std::string &name, int id)
{
	protocol::game::smsg_sport_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	ADD_MSG_REWARD(msg, rds);
	msg.set_pgold(pgold);
	msg.set_cid(cid);
	msg.set_max_rank(player->max_rank());
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_sport_saodang(dhc::player_t*player, const protocol::game::smsg_sport_saodang& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_sport_shop_buy(dhc::player_t *player, const std::vector<dhc::equip_t *> &equips, const std::vector<dhc::treasure_t*> &treasures, const std::string &name, int id)
{
	protocol::game::smsg_sport_shop_buy msg;
	for (int i = 0; i < equips.size(); ++i)
	{
		msg.add_equips()->CopyFrom(*equips[i]);
	}
	for (int i = 0; i < treasures.size(); ++i)
	{
		msg.add_treasures()->CopyFrom(*treasures[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_vip_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_vip_reward msg;
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_gm_command(dhc::player_t *player, int type, int value1, int value2, int value3, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_gm_command msg;
	msg.set_type(type);
	msg.set_value1(value1);
	msg.set_value2(value2);
	msg.set_value3(value3);
	ADD_MSG_REWARD_WITH_PET(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_dj_reward(dhc::player_t *player, int bs, int gold, const std::string &name, int id)
{
	protocol::game::smsg_dj_reward msg;
	msg.set_bs(bs);
	msg.set_gold(gold);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_boss_look(dhc::player_t *player, const protocol::game::smsg_boss_look& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_boss_look_ex(dhc::player_t *player, const protocol::game::smsg_boss_look& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void ResMessage::res_boss_active_look(dhc::player_t *player, const protocol::game::smsg_boss_active_look& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_boss_rank(dhc::player_t *player, const protocol::game::smsg_boss_rank& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_boss_fight_end(dhc::player_t *player, const protocol::game::smsg_boss_fight_end& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_boss_fight_saodang(dhc::player_t *player, const protocol::game::smsg_boss_saodang& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_first_recharge(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_first_recharge msg;
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_online_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_online_reward msg;
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_daily_sign(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_daily_sign msg;
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_hbb_look(dhc::player_t *player, const std::string &name, int id)
{
	protocol::game::smsg_hbb_look msg;
	for (int i = 0; i < player->hbb_class_ids_size(); ++i)
	{
		msg.add_class_ids(player->hbb_class_ids(i));
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_hbb_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_hbb_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	for (int i = 0; i < player->hbb_class_ids_size(); ++i)
	{
		msg.add_class_ids(player->hbb_class_ids(i));
	}
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_player_look(dhc::player_t *player, dhc::player_t *target, const std::vector<dhc::role_t *> &roles, const std::vector<dhc::equip_t *> &equips, const std::vector<dhc::treasure_t*> &treasures, const std::vector<int>& role_sxs, const std::vector<dhc::pet_t*> &pets, const std::vector<int> &pet_sxs, const std::string &name, int id)
{
	protocol::game::smsg_player_look msg;
	msg.set_template_id(target->template_id());
	msg.set_level(target->level());
	msg.set_name(target->name());
	msg.set_bf(target->bf());
	msg.set_vip(target->vip());
	msg.set_achieves(target->dress_achieves_size());
	msg.set_chenghao(target->chenghao_on());
	msg.set_guid(target->guid());
	msg.set_serverid(player->serverid());
	msg.set_nalflag(target->nalflag());
	if (target->guild() > 0)
	{
		dhc::guild_t *guild = POOL_GET_GUILD(target->guild());
		if (guild)
		{
			msg.set_guild(guild->name());
		}
	}
	for (int i = 0; i < roles.size(); ++i)
	{
		if (roles[i])
		{
			msg.add_roles()->CopyFrom(*roles[i]);
		}
		else
		{
			msg.add_roles()->set_guid(0);
		}
	}
	for (int i = 0; i < role_sxs.size(); ++i)
	{
		msg.add_roles_sx(role_sxs[i]);
	}
	for (int i = 0; i < equips.size(); ++i)
	{
		if (equips[i])
		{
			msg.add_equips()->CopyFrom(*equips[i]);
		}
		else
		{
			msg.add_equips()->set_guid(0);
		}
	}
	for (int i = 0; i < treasures.size(); ++i)
	{
		if (treasures[i])
		{
			msg.add_treasures()->CopyFrom(*treasures[i]);
		}
		else
		{
			msg.add_treasures()->set_guid(0);
		}
	}
	for (int i = 0; i < pets.size(); ++i)
	{
		if (pets[i])
		{
			msg.add_pets()->CopyFrom(*pets[i]);
		}
		else
		{
			msg.add_pets()->set_guid(0);
		}
	}
	for (int i = 0; i < pet_sxs.size(); ++i)
	{
		msg.add_pets_sx(pet_sxs[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_random_event_look(dhc::player_t *player, int32_t event_id, const std::string &name, int id)
{
	protocol::game::smsg_random_event_look msg;
	msg.set_id(event_id);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_random_event_get(dhc::player_t *player, uint64_t random_event_time, const std::string &name, int id)
{
	protocol::game::smsg_random_event_get msg;
	msg.set_random_event_time(random_event_time);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_data(dhc::player_t *player, dhc::guild_t *guild, int32_t zhiwu, bool mobai, bool fight, int msg_count, bool apply, bool hongbao, bool guildpvp, const std::string &name, int id)
{
	protocol::game::smsg_guild_data msg;
	msg.mutable_guild()->CopyFrom(*guild);
	
	dhc::guild_t *copyGuild = msg.mutable_guild();
	copyGuild->clear_event_guids();
	copyGuild->clear_message_guids();
	copyGuild->clear_red_guids();
	copyGuild->clear_box_guids();

	msg.set_zhiwu(zhiwu);
	msg.set_can_mobai(mobai);
	msg.set_can_fight(fight);
	msg.set_msg_count(msg_count);
	msg.set_has_apply(apply);
	msg.set_has_hongbao(hongbao);
	msg.set_guildpvp(guildpvp);
	for (int i = 0; i != guild->event_guids_size(); ++i)
	{
		dhc::guild_event_t *event = POOL_GET_GUILD_EVENT(guild->event_guids(i));
		if (event)
		{
			msg.add_guild_event()->CopyFrom(*event);
		}
	}
	msg.set_success(200);
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_list_recommend(dhc::player_t *player, std::vector<dhc::guild_t*> &guild_list, const std::string &name, int id)
{
	protocol::game::smsg_guild_list_recommend msg;
	std::vector<dhc::guild_t*>::iterator iter = guild_list.begin();

	dhc::guild_t *copyGuild = 0;
	for (; iter != guild_list.end(); ++iter)
	{
		copyGuild = *iter;
		msg.add_guild_guids(copyGuild->guid());
		msg.add_guild_names(copyGuild->name());
		msg.add_guild_levels(copyGuild->level());
		msg.add_guild_members(copyGuild->member_guids_size());
		msg.add_guild_icons(copyGuild->icon());
	}
	for (int i = 0; i < player->guild_applys_size(); ++i)
	{
		msg.add_apply_list(player->guild_applys(i));
	}
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);	
}

void ResMessage::res_guild_member_view(dhc::player_t *player, std::list<dhc::guild_member_t*> &member_list, const std::string &name, int id)
{
	protocol::game::smsg_guild_member_view msg;
	std::list<dhc::guild_member_t*>::iterator iter = member_list.begin();
	for (; iter != member_list.end(); ++iter)
	{
		dhc::guild_member_t *guild_member = msg.add_guild_members();
		guild_member->CopyFrom(*(*iter));
		if (game::channel()->online(guild_member->player_guid()))
		{
			guild_member->set_offline_time(game::timer()->now());
		}
	}
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_ranking(dhc::player_t *player, std::list<dhc::guild_t*> &guild_list, const std::string &name, int id)
{
	protocol::game::smsg_guild_ranking msg;
	std::list<dhc::guild_t*>::iterator iter = guild_list.begin();

	dhc::guild_t *copyGuild = 0;
	for (; iter != guild_list.end(); ++iter)
	{
		copyGuild = *iter;
		msg.add_guild_names(copyGuild->name());
		msg.add_guild_levels(copyGuild->level());
		msg.add_guild_members(copyGuild->member_guids_size());
		msg.add_guild_icons(copyGuild->icon());
	}
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_mission_ranking(dhc::player_t *player, const protocol::game::smsg_guild_mission_ranking& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_activity(dhc::player_t *player, const protocol::game::smsg_guild_activity &msg, std::string name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_boss_look(dhc::player_t *player, const protocol::game::smsg_guild_mission_look&msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_boss_look_ex(dhc::player_t *player, const protocol::game::smsg_guild_mission_look&msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void ResMessage::res_guild_boss_fight_end(dhc::player_t *player, int result, const std::string& text, int contri, int hit_contri, int hit, const std::string &name, int id)
{
	protocol::game::smsg_guild_mission_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	msg.set_contri(contri);
	msg.set_hit_contri(hit_contri);
	msg.set_hit(hit);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_message_view(dhc::player_t *player, const protocol::game::smsg_guild_message_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_red_deliver(dhc::player_t *player, const protocol::game::smsg_guild_red_deliver& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_red_view(dhc::player_t *player, const protocol::game::smsg_guild_red_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_red_rob(dhc::player_t *player, int jewel, const std::string& name, int id)
{
	protocol::game::smsg_guild_red_rob msg;
	msg.set_jewel(jewel);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_mission_complete_reward(dhc::player_t *player, int reward_index, const std::string& name, int id)
{
	protocol::game::smsg_guild_mission_complete_reward msg;
	msg.set_reward_index(reward_index);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_mission_complete_reward_view(dhc::player_t *player, const protocol::game::smsg_guild_mission_complete_reward_view& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_fight_look(dhc::player_t *player, const protocol::game::smsg_guild_fight_pvp_look& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_equip_suipian(dhc::player_t *player, dhc::equip_t *equip, const std::string &name, int id)
{
	protocol::game::smsg_equip_suipian msg;
	msg.mutable_equip()->CopyFrom(*equip);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_equip_init(dhc::player_t *player, const protocol::game::smsg_equip_init &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_rank_data(dhc::player_t *player, dhc::rank_t *rank, const std::string &name, int id)
{
	protocol::game::smsg_rank_view msg;
	msg.mutable_rank_list()->CopyFrom(*rank);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_look(dhc::player_t *player, const std::vector<dhc::social_t *> &socials, const std::string &name, int id)
{
	protocol::game::smsg_social_look msg;
	for (int i = 0; i < socials.size(); ++i)
	{
		dhc::social_t *social = msg.add_social();
		social->CopyFrom(*socials[i]);
		if (game::channel()->online(social->target_guid()))
		{
			social->set_offline_time(game::timer()->now());
		}
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_rand(dhc::player_t *player, const std::vector<dhc::player_t *> &players, const std::string &name, int id)
{
	protocol::game::smsg_social_rand msg;
	uint64_t now_time = game::timer()->now();

	for (int i = 0; i < players.size(); ++i)
	{
		protocol::game::msg_social_player *sp = msg.add_social_player();
		sp->set_player_guid(players[i]->guid());
		sp->set_player_name(players[i]->name());
		sp->set_player_template(players[i]->template_id());
		sp->set_player_level(players[i]->level());
		sp->set_player_bf(players[i]->bf());
		sp->set_player_vip(players[i]->vip());
		sp->set_player_achieve(players[i]->dress_achieves_size());
		sp->set_nalflag(players[i]->nalflag());
		if (game::channel()->online(players[i]->guid()))
		{
			sp->set_offline_time(now_time);
		}
		else
		{
			sp->set_offline_time(players[i]->last_check_time());
		}
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_add(dhc::player_t *player, uint64_t target_guid, const std::string &name, int id)
{
	protocol::game::smsg_social_add msg;
	msg.set_player_guid(target_guid);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_look_new(dhc::player_t *player, const std::vector<protocol::game::msg_social_player *> social_players, const std::string &name, int id)
{
	protocol::game::smsg_social_look_new msg;
	for (int i = 0; i < social_players.size(); ++i)
	{
		msg.add_social_player()->CopyFrom(*social_players[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_agree(dhc::player_t *player, uint64_t target_guid, int agree, const std::string &name, int id)
{
	protocol::game::smsg_social_agree msg;
	msg.set_player_guid(target_guid);
	msg.set_agree(agree);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_delete(dhc::player_t *player, uint64_t social_guid, const std::string &name, int id)
{
	protocol::game::smsg_social_delete msg;
	msg.set_social_guid(social_guid);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_song(dhc::player_t *player, uint64_t social_guid, const std::string &name, int id)
{
	protocol::game::smsg_social_song msg;
	msg.set_social_guid(social_guid);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_shou(dhc::player_t *player, const std::vector<uint64_t> &social_guids, const std::string &name, int id)
{
	protocol::game::smsg_social_shou msg;
	for (int i = 0; i < social_guids.size(); ++i)
	{
		msg.add_social_guids(social_guids[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_player_check(dhc::player_t *player, const protocol::game::smsg_player_check& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_role_skillup(dhc::player_t *player, int exp, int num, const std::string &name, int id)
{
	protocol::game::smsg_role_skillup msg;
	msg.set_exp(exp);
	msg.set_num(num);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_libao(dhc::player_t *player, const s_t_rewards& rds, int chongzhi, const std::string &name, int id)
{
	protocol::game::smsg_libao msg;
	msg.set_chongzhi(chongzhi);
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_ttt_value_look(dhc::player_t *player, const std::string &name, int id)
{
	protocol::game::smsg_ttt_value_look msg;
	for (int i = 0; i < player->ttt_cur_reward_ids_size(); ++i)
	{
		msg.add_ids(player->ttt_cur_reward_ids(i));
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_ttt_fight_end(dhc::player_t *player, int result, const std::string& text, int index, int nd, int mibao, int baoji1, int baoji2, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_ttt_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	msg.set_index(index);
	msg.set_nd(nd);
	msg.set_mibao(mibao);
	msg.set_baoji1(baoji1);
	msg.set_baoji2(baoji2);
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_ttt_sanxing(dhc::player_t *player, const protocol::game::smsg_ttt_sanxing& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_xjbz_get(dhc::player_t *player, int gold, int baoshi, int zz, const std::string &name, int id)
{
	protocol::game::smsg_xjbz_get msg;
	msg.set_gold(gold);
	msg.set_baoshi(baoshi);
	msg.set_zz(zz);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_global_view(dhc::player_t *player, dhc::global_t *global, const std::string &name, int id)
{
	protocol::game::smsg_huodong_pttq_view msg;
	msg.mutable_data()->CopyFrom(*global);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_kaifu_look(dhc::player_t *player, const std::vector<int> &ids, const std::vector<int> &counts, const std::string &name, int id)
{
	protocol::game::smsg_huodong_kaifu_look msg;

	for (int i = 0; i < ids.size(); ++i)
	{
		msg.add_ids(ids[i]);
		msg.add_counts(counts[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_kaifu_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_huodong_kaifu_reward msg;
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_view(dhc::player_t *player, const protocol::game::smsg_huodong_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_tanbao_view(dhc::player_t *player, const protocol::game::smsg_huodong_tanbao_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_tanbao_dice(dhc::player_t *player, const protocol::game::smsg_tanbao_dice& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_fanpai_view(dhc::player_t *player, const protocol::game::smsg_huodong_fanpai_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_fanpai(dhc::player_t *player, int fan_id, const std::string &name, int id)
{
	protocol::game::smsg_huodong_fanpai msg;
	msg.set_id(fan_id);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_tansuo_view(dhc::player_t *player, const protocol::game::smsg_huodong_tansuo_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_tansuo(dhc::player_t *player, const protocol::game::smsg_huodong_tansuo &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_tansuo_event(dhc::player_t *player, const protocol::game::smsg_huodong_tansuo_event &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_tansuo_event_refresh(dhc::player_t *player, int qiyu_id, const std::string &name, int id)
{
	protocol::game::smsg_huodong_tansuo_event_refresh msg;
	msg.set_qiyu_id(qiyu_id);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_zhuanpan_view(dhc::player_t *player, const protocol::game::smsg_huodong_zhuanpan_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_zhuanpan(dhc::player_t *player, const protocol::game::smsg_huodong_zhuan& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_jiri(dhc::player_t* player, const protocol::game::smsg_huodong_jiri_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_reward_view(dhc::player_t *player, const protocol::game::smsg_huodong_reward_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_reward(dhc::player_t *player, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_huodong_reward msg;
	ADD_MSG_REWARD_WITH_PET(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}
void ResMessage::res_huodong_zc_reward(dhc::player_t *player, const s_t_rewards& rds,int type, const std::string &name, int id)
{
	protocol::game::smsg_huodong_reward msg;
	ADD_MSG_REWARD_WITH_PET(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_pvp_refresh(dhc::player_t* player, const protocol::game::smsg_pvp_view& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_pvp_fight_end(dhc::player_t*player, const protocol::game::smsg_pvp_fight_end& msg, const std::string& name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_recharge_check_ex(int type, const int &rdz_ids, const int &rdz_counts, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_recharge_check_ex msg;
	msg.add_type_judge(type);
	msg.add_rdz_ids(rdz_ids);
	msg.add_rdz_counts(rdz_counts);
	ADD_MSG_REWARD_WITH_PET(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void ResMessage::res_treasure_jinlian(dhc::player_t *player, const std::vector<uint64_t> &treasures, const std::string &name, int id)
{
	protocol::game::smsg_treasure_jinlian msg;
	for (int i = 0; i < treasures.size(); ++i)
	{
		msg.add_treasures(treasures[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_hecheng(dhc::player_t *player, dhc::treasure_t *treasure, const std::string &name, int id)
{
	protocol::game::smsg_treasure_hecheng msg;
	msg.mutable_treasure()->CopyFrom(*treasure);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_star(dhc::player_t *player, const protocol::game::smsg_treasure_star &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_init(dhc::player_t *player, const protocol::game::smsg_treasure_init &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_view(dhc::player_t *player, const protocol::game::smsg_treasure_rob_view &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_protect(dhc::player_t *player, const std::string &name, int id)
{
	protocol::game::smsg_treasure_protect msg;
	msg.set_next_time(player->treasure_protect_next_time());
	msg.set_cd_time(player->treasure_protect_cd_time());

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_fight(dhc::player_t *player, const protocol::game::smsg_treasure_fight &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_fight_end(dhc::player_t *player, const s_t_rewards& rds, int suipian_id, int result, int card, int pgold, const std::string &text, const std::string &name, int id)
{
	protocol::game::smsg_treasure_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	msg.set_suipian_id(suipian_id);
	msg.set_card(card);
	msg.set_pgold(pgold);
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_saodang(dhc::player_t *player, const protocol::game::smsg_treasure_saodang &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_yijian_saodang(dhc::player_t *player, const protocol::game::smsg_treasure_yijian_saodang &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_point(dhc::player_t *player, int has, const std::string &name, int id)
{
	protocol::game::smsg_treasure_point msg;
	msg.set_has_point(has);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_report(dhc::player_t *player, const std::vector<int> &suipian_ids, const std::string &name, int id)
{
	protocol::game::smsg_treasure_report msg;
	for (int i = 0; i < player->treasure_reports_size(); ++i)
	{
		dhc::treasure_report_t *treasure_report = POOL_GET_TREASURE_REPORT(player->treasure_reports(i));
		if (treasure_report)
		{
			msg.add_reports()->CopyFrom(*treasure_report);
			if (treasure_report->new_report() == 0)
			{
				treasure_report->set_new_report(1);
			}
		}
	}
	for (std::vector<int>::size_type i = 0;
		i < suipian_ids.size();
		++i)
	{
		msg.add_suipian_ids(suipian_ids[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_treasure_report_ex(dhc::player_t *player, const std::vector<int> &suipian_ids, const std::string &name, int id)
{
	protocol::game::smsg_treasure_report msg;
	for (int i = 0; i < player->treasure_reports_size(); ++i)
	{
		dhc::treasure_report_t *treasure_report = POOL_GET_TREASURE_REPORT(player->treasure_reports(i));
		if (treasure_report)
		{
			msg.add_reports()->CopyFrom(*treasure_report);
			if (treasure_report->new_report() == 0)
			{
				treasure_report->set_new_report(1);
			}
		}
	}
	for (std::vector<int>::size_type i = 0;
		i < suipian_ids.size();
		++i)
	{
		msg.add_suipian_ids(suipian_ids[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void ResMessage::res_treasure_zhuzao(dhc::player_t *player, int level, int exp, const std::string &name, int id)
{
	protocol::game::smsg_treasure_zhuzao msg;
	msg.set_level(level);
	msg.set_exp(exp);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_yb_refresh(dhc::player_t *player, int type, const std::string &name, int id)
{
	protocol::game::smsg_yb_refresh msg;
	msg.set_type(type);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_yb_look(dhc::player_t *player, const std::vector<protocol::game::msg_yb_player> &yb_players
	, const std::list<protocol::game::msg_yb_info> &yb_infos, const std::list<protocol::game::msg_ybq_info> &ybq_infos
	, int player_id, const std::string &name, int id)
{
	protocol::game::smsg_yb_look msg;
	for (int i = 0; i < yb_players.size(); ++i)
	{
		msg.add_players()->CopyFrom(yb_players[i]);
	}
	for (std::list<protocol::game::msg_yb_info>::const_iterator it = yb_infos.begin(); it != yb_infos.end(); ++it)
	{
		msg.add_yb_infos()->CopyFrom(*it);
	}
	for (std::list<protocol::game::msg_ybq_info>::const_iterator it = ybq_infos.begin(); it != ybq_infos.end(); ++it)
	{
		msg.add_ybq_infos()->CopyFrom(*it);
	}
	for (int i = 0; i < player->ybq_guids_size(); ++i)
	{
		msg.add_ybq_guids(player->ybq_guids(i));
	}
	msg.set_player_id(player_id);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_yb_look_ex(dhc::player_t *player, const std::vector<protocol::game::msg_yb_player> &yb_players
	, const std::list<protocol::game::msg_yb_info> &yb_infos, const std::list<protocol::game::msg_ybq_info> &ybq_infos
	, int player_id, const std::string &name, int id)
{
	protocol::game::smsg_yb_look msg;
	for (int i = 0; i < yb_players.size(); ++i)
	{
		msg.add_players()->CopyFrom(yb_players[i]);
	}
	for (std::list<protocol::game::msg_yb_info>::const_iterator it = yb_infos.begin(); it != yb_infos.end(); ++it)
	{
		msg.add_yb_infos()->CopyFrom(*it);
	}
	for (std::list<protocol::game::msg_ybq_info>::const_iterator it = ybq_infos.begin(); it != ybq_infos.end(); ++it)
	{
		msg.add_ybq_infos()->CopyFrom(*it);
	}
	for (int i = 0; i < player->ybq_guids_size(); ++i)
	{
		msg.add_ybq_guids(player->ybq_guids(i));
	}
	msg.set_player_id(player_id);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
}

void ResMessage::res_yb_ybq_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_yb_ybq_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_yb_reward(dhc::player_t *player, int yuanli, const std::string &name, int id)
{
	protocol::game::smsg_yb_reward msg;
	msg.set_yuanli(yuanli);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_ore_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, int hp, const std::string &name, int id)
{
	protocol::game::smsg_ore_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	msg.set_hp(hp);
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_qiyu_fight_end(dhc::player_t *player, int result, const std::string &text, const s_t_rewards& rds, const std::string &name, int id)
{
	protocol::game::smsg_qiyu_fight_end msg;
	msg.set_result(result);
	msg.set_text(text);
	ADD_MSG_REWARD(msg, rds);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_qiyu_check(dhc::player_t *player, const std::string &name, int id)
{
	protocol::game::smsg_qiyu_check msg;
	for (int i = 0; i < player->qiyu_mission_size(); ++i)
	{
		msg.add_qiyu_mission(player->qiyu_mission(i));
		msg.add_qiyu_hard(player->qiyu_hard(i));
		msg.add_qiyu_suc(player->qiyu_suc(i));
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_social_invite_look(dhc::player_t *player, const protocol::game::smsg_team_friend_view& msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_bingyuan_fight_end(dhc::player_t *player, const protocol::game::smsg_bingyuan_fight_end &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_qiechuo(dhc::player_t *player, const protocol::game::smsg_qiecuo &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_ds_fight_end(dhc::player_t *player, const protocol::game::smsg_ds_fight_end &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_ds_time_buy(dhc::player_t *player, const protocol::game::smsg_huodong_fanpai &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_mofang_refresh(dhc::player_t *player, const protocol::game::smsg_huodong_mofang_refresh &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_mofang_view(dhc::player_t *player, const protocol::game::smsg_huodong_mofang_view &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_mofang(dhc::player_t *player, const protocol::game::smsg_huodong_mofang &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_huodong_yueka_view(dhc::player_t *player, const protocol::game::smsg_huodong_yueka_view &msg, const std::string &name, int id)
{	
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}
/////////////////////////////////////////////////////////////////////////

void SelfMessage::self_player_load_end(uint64_t player_guid, int opcode, const std::string &msg)
{
	Packet *pck = Packet::New(opcode, 0, player_guid, msg);
	if (!pck)
	{
		return;
	}
	game::game_service()->add_msg(pck);
}

void SelfMessage::self_start_boss()
{
	Packet *pck = Packet::New(SELF_START_BOSS, 0, 0, 0);
	if (!pck)
	{
		return;
	}
	game::game_service()->add_msg(pck);
}

void SelfMessage::self_start_guild_boss()
{
	Packet *pck = Packet::New(SELF_START_GUILD_BOSS, 0, 0, 0);
	if (!pck)
	{
		return;
	}
	game::game_service()->add_msg(pck);
}

void SelfMessage::self_boss_change_name(uint64_t player_guid, const std::string&msg)
{
	Packet *pck = Packet::New(SLEF_PLAYER_BOSS_CHANGE_NAME, 0, player_guid, msg);
	if (!pck)
	{
		return;
	}
	game::game_service()->add_msg(pck);
}

//////////////////////////////////////////////////////////////////////////

void PushMessage::push_chat(dhc::player_t *player, int type, const std::string &color, const std::string &text,
	int is_danmu, uint64_t target_guid, const std::string &target_name, const std::vector<uint64_t> &guids, const std::string &name)
{
	protocol::game::pmsg_chat msg;
	protocol::game::smsg_chat *smsg = msg.mutable_msg_chat();
	smsg->set_player_guid(player->guid());
	smsg->set_player_name(player->name());
	smsg->set_player_template(player->template_id());
	smsg->set_level(player->level());
	smsg->set_vip(player->vip());
	smsg->set_chenghao(player->chenghao_on());
	smsg->set_type(type);
	smsg->set_color(color);
	smsg->set_text(text);
	smsg->set_is_danmu(is_danmu);
	smsg->set_time(game::timer()->now());
	smsg->set_target_guid(target_guid);
	smsg->set_target_name(target_name);
	smsg->set_nalflag(player->nalflag());
	for (int i = 0; i < guids.size(); ++i)
	{
		msg.add_player_guids(guids[i]);
	}

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->push(name, PMSG_CHAT, s);
}

void PushMessage::push_gundong(const std::string &text, const std::string &name)
{
	protocol::game::smsg_gundong msg;
	msg.set_text(text);

	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->push(name, PMSG_GUNDONG, s);
}

void PushMessage::push_vote(dhc::player_t *player, int goddess, int renqi)
{
	rpcproto::tmsg_player_vote msg;
	msg.set_guid(player->guid());
	msg.set_name(player->name());
	TermInfo * info = game::channel()->get_channel(player->guid());
	if (info)
	{
		try
		{
			msg.set_server(boost::lexical_cast<int>(info->serverid));
		}
		catch (...)
		{
			msg.set_server(0);
		}
		msg.set_acc(info->username);
	}
	msg.set_id(goddess);
	msg.set_renqi(renqi);
	
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->push("remote1", PMSG_VOTE, s);
}

void ResMessage::res_guild_pvp_look(dhc::player_t *player, const protocol::game::smsg_guild_fight_pvp_look &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}

void ResMessage::res_guild_pvp_fight(dhc::player_t *player, const protocol::game::smsg_guild_fight &msg, const std::string &name, int id)
{
	std::string s;
	msg.SerializeToString(&s);
	game::rpc_service()->response(name, id, s);
	game::channel()->set_last_pck(player->guid(), s);
}