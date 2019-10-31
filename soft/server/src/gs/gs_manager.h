#ifndef __GS_MANAGER_H__
#define __GS_MANAGER_H__

#include "gameinc.h"
#include "packet.h"

#include "player_manager.h"
#include "item_manager.h"
#include "mission_manager.h"
#include "role_manager.h"
#include "equip_manager.h"
#include "post_manager.h"
#include "sport_manager.h"
#include "boss_manager.h"
#include "social_manager.h"
#include "guild_manager.h"
#include "rank_manager.h"
#include "huodong_manager.h"
#include "treasure_manager.h"
#include "pvp_manager.h"

class GsManager : public mmg::GameInterface
{
public:
	GsManager();

	~GsManager();

	BEGIN_PACKET_MAP
		PACKET_HANDLER(SELF_PLAYER_LOAD_SPORT, sport_mgr_->self_player_load_sport)
		PACKET_HANDLER(SELF_PLAYER_LOAD_SPORT_SAODANG, sport_mgr_->self_player_load_sport_saodang)
		PACKET_HANDLER(SELF_PLAYER_LOAD_LOOK, player_mgr_->self_player_load_look)
		PACKET_HANDLER(SELF_PLAYER_LOAD_RECHARGE, player_mgr_->self_player_load_recharge)
		PACKET_HANDLER(SLEF_PLAYER_LOAD_GUILD_APPLY, guild_mgr_->self_guild_load_apply)
		PACKET_HANDLER(SELF_PLAYER_LOAD_GUILD_KICK, guild_mgr_->self_guild_load_kick)
		PACKET_HANDLER(SELF_PLAYER_LOAD_TREASURE, treasure_mgr_->self_player_load_treasure)	
		PACKET_HANDLER(SLEF_PLAYER_BOSS_CHANGE_NAME, boss_mgr_->terminal_boss_change_name)
		PACKET_HANDLER(SELF_START_GUILD_BOSS, pvp_mrg_->start_pvp)
		PACKET_HANDLER(SELF_PLAYER_LOAD_HUODONG_MODIFY, huodong_mgr_->terminal_huodong_modify_callback)
		PACKET_HANDLER(SELF_PLAYER_LOAD_LIBAO_EXCHANGE, player_mgr_->self_player_load_libao_exchange)
	END_PACKET_MAP

	BEGIN_PUSH_MAP
	END_PUSH_MAP

	BEGIN_REQ_MAP
		REQ_HANDLER(CMSG_CLIENT_LOGIN, player_mgr_->terminal_client_login)
		REQ_HANDLER(CMSG_PLAYER_NAME, player_mgr_->terminal_player_name)
		REQ_HANDLER(CMSG_CHANGE_NAME, player_mgr_->terminal_player_change_name)
		REQ_HANDLER(CMSG_CHANGE_NALFLAG, player_mgr_->terminal_player_change_nalflag)
		REQ_HANDLER(CMSG_PLAYER_ZSNAME, player_mgr_->terminal_player_zsname)
		REQ_HANDLER(CMSG_PLAYER_TEMPLATE, player_mgr_->terminal_player_template)
		REQ_HANDLER(CMSG_RECHARGE, player_mgr_->terminal_recharge)
		REQ_HANDLER(CMSG_FIRST_RECHARGE, player_mgr_->terminal_first_recharge)
		REQ_HANDLER(CMSG_GM_COMMAND, player_mgr_->terminal_gm_command)
		REQ_HANDLER(CMSG_CLIENT_RELOAD, player_mgr_->terminal_client_reload)
		REQ_HANDLER(CMSG_PLAYER_TASK, player_mgr_->terminal_player_task)
		REQ_HANDLER(CMSG_MISSION_FIGHT_END, mission_mgr_->terminal_mission_fight_end)
		REQ_HANDLER(CMSG_MISSION_REWARD, mission_mgr_->terminal_mission_reward)
		REQ_HANDLER(CMSG_MISSION_SAODANG, mission_mgr_->terminal_mission_saodang)
		REQ_HANDLER(CMSG_ZHENXING, role_mgr_->terminal_zhenxing)
		REQ_HANDLER(CMSG_ROLE_EQUIP, role_mgr_->terminal_role_equip)
		REQ_HANDLER(CMSG_CHOUKA, role_mgr_->terminal_chouka)
		REQ_HANDLER(CMSG_ROLE_UPGRADE, role_mgr_->terminal_role_upgrade)
		REQ_HANDLER(CMSG_ROLE_TUPO, role_mgr_->terminal_role_tupo)
		REQ_HANDLER(CMSG_ROLE_JINJIE, role_mgr_->terminal_role_jinjie)
		REQ_HANDLER(CMSG_ROLE_DUIHUAN, role_mgr_->terminal_role_duihuan)
		REQ_HANDLER(CMSG_ROLE_SUIPIAN, role_mgr_->terminal_role_suipian)
		REQ_HANDLER(CMSG_ROLE_SKILLUP, role_mgr_->terminal_role_skillup)
		REQ_HANDLER(CMSG_ROLE_DRESS_ON, role_mgr_->terminal_role_dress_on)
		REQ_HANDLER(CMSG_ROLE_CHONGSHENG, role_mgr_->terminal_role_init)
		REQ_HANDLER(CMSG_ROLE_FENJIE, role_mgr_->terminal_role_fenjie)
		REQ_HANDLER(CMSG_ROLE_XQ_LOOK, role_mgr_->terminal_role_xq_look)
		REQ_HANDLER(CMSG_ROLE_YH_LOOK, role_mgr_->terminal_role_yh_look)
		REQ_HANDLER(CMSG_ROLE_YH_SELECT, role_mgr_->terminal_role_yh_select)
		REQ_HANDLER(CMSG_ROLE_ALL_EQUIP, role_mgr_->terminal_role_all_equip)
		REQ_HANDLER(CMSG_ROLE_SHENGPIN, role_mgr_->terminal_role_shengpin)
		REQ_HANDLER(CMSG_ROLE_HUIYI_CHOU, role_mgr_->terminal_role_huiyi_chou)
		REQ_HANDLER(CMSG_ROLE_HUIYI_JIHUO, role_mgr_->terminal_role_huiyi_jihuo)
		REQ_HANDLER(CMSG_ROLE_HUIYI_STAR, role_mgr_->terminal_role_huiyi_starts)
		REQ_HANDLER(CMSG_ROLE_HUIYI_RESET,role_mgr_->termianl_role_huiyi_reset)
		REQ_HANDLER(CMSG_ROLE_HUIYI_ZHANPU, role_mgr_->terminal_role_huiyi_fate_zhanpu)
		REQ_HANDLER(CMSG_ROLE_HUIYI_GAIYUN, role_mgr_->terminal_role_huiyi_fate_gaiyun)
		REQ_HANDLER(CMSG_ROLE_HUIYI_FANPAI, role_mgr_->terminal_role_huiyi_fate_fanpai)
		REQ_HANDLER(CMSG_ROLE_SUIPIAN_GAIZAO, role_mgr_->terminal_role_suipian_gaizao)
		REQ_HANDLER(CMSG_ROLE_DUIXING, role_mgr_->terminal_duixing)
		REQ_HANDLER(CMSG_ROLE_DUIXING_UP, role_mgr_->terminal_duixing_up)
		REQ_HANDLER(CMSG_ROLE_DUIXING_ON, role_mgr_->terminal_duixing_on)
		REQ_HANDLER(CMSG_ROLE_HOUYUAN, role_mgr_->terminal_houyuan)
		REQ_HANDLER(CMSG_GUANGHUAN_ON, role_mgr_->terminal_guanghuan)
		REQ_HANDLER(CMSG_GUANGHUAN_LEVEL, role_mgr_->terminal_guanghuan_up)
		REQ_HANDLER(CMSG_GUANGHUAN_INIT, role_mgr_->terminal_guanghun_init)
		REQ_HANDLER(CMSG_CHENGHAO_ON, player_mgr_->terminal_chenghao_on)
		REQ_HANDLER(CMSG_ROLE_BSKILL_LEVELUP, role_mgr_->terminal_role_bskillup)
		REQ_HANDLER(CMSG_PET_ON, role_mgr_->terminal_pet_on)
		REQ_HANDLER(CMSG_PET_GUARD, role_mgr_->terminal_pet_guard)
		REQ_HANDLER(CMSG_PET_LEVEL, role_mgr_->terminal_pet_level)
		REQ_HANDLER(CMSG_PET_JINJIE, role_mgr_->terminal_pet_jinjie)
		REQ_HANDLER(CMSG_PET_STAR, role_mgr_->terminal_pet_star)
		REQ_HANDLER(CMSG_PET_DUIHUAN, role_mgr_->terminal_pet_suipian)
		REQ_HANDLER(CMSG_PET_INIT, role_mgr_->terminal_pet_init)
		REQ_HANDLER(CMSG_PET_FENJIE, role_mgr_->terminal_pet_fenjie)
		
		REQ_HANDLER(CMSG_EQUIP_ENHANCE, equip_mgr_->terminal_equip_enhance)
		REQ_HANDLER(CMSG_EQUIP_AUTO_ENHANCE, equip_mgr_->terminal_equip_auto_enhance)
		REQ_HANDLER(CMSG_EQUIP_JINLIAN, equip_mgr_->terminal_equip_jl)
		REQ_HANDLER(CMSG_EQUIP_SELL, equip_mgr_->terminal_equip_sell)
		REQ_HANDLER(CMSG_EQUIP_LOCK, equip_mgr_->terminal_equip_lock)
		REQ_HANDLER(CMSG_EQUIP_GAIZAO, equip_mgr_->terminal_equip_gaizao)
		REQ_HANDLER(CMSG_EQUIP_GAIZAO_TEN, equip_mgr_->terminal_equip_gaizao_ten)
		REQ_HANDLER(CMSG_EQUIP_GAIZAO1, equip_mgr_->terminal_equip_gaizao_buy)
		REQ_HANDLER(CMSG_EQUIP_INIT, equip_mgr_->terminal_equip_init)
		REQ_HANDLER(CMSG_EQUIP_STAR, equip_mgr_->terminal_equip_star)
		REQ_HANDLER(CMSG_EQUIP_RONGLIAN, equip_mgr_->terminal_equip_rongliang)

		REQ_HANDLER(CMSG_ITEM_SELL, item_mgr_->terminal_item_sell)
		REQ_HANDLER(CMSG_ITEM_SELL_ALL, item_mgr_->terminal_item_sell_all)
		REQ_HANDLER(CMSG_ITEM_APPLY, item_mgr_->terminal_item_apply)
		REQ_HANDLER(CMSG_ITEM_QUICK_APPLY, item_mgr_->terminal_item_apply)
		REQ_HANDLER(CMSG_ITEM_FENJIE, item_mgr_->terminal_item_fenjie)
		REQ_HANDLER(CMSG_ITEM_DIRECT_BUY, item_mgr_->terminal_item_buy)
		REQ_HANDLER(CMSG_ITEM_HECHENG, item_mgr_->terminal_item_hecheng)

		REQ_HANDLER(CMSG_SHOP_CHECK, item_mgr_->terminal_shop_check)
		REQ_HANDLER(CMSG_SHOP_REFRESH, item_mgr_->terminal_shop_refresh)
		REQ_HANDLER(CMSG_SHOP_BUY, item_mgr_->terminal_shop_buy)
		REQ_HANDLER(CMSG_SHOP_XG, item_mgr_->terminal_shop_xg)
		REQ_HANDLER(CMSG_BOSS_SHOP_BUY, item_mgr_->terminal_shop_boss)
		REQ_HANDLER(CMSG_TTT_SHOP_BUY, item_mgr_->terminal_shop_ttt)
		REQ_HANDLER(CMSG_TTT_SHOP_MUBIAO, item_mgr_->terminal_mubiao_ttt)
		REQ_HANDLER(CMSG_TTT_BAOZANG, item_mgr_->terminal_baozang_ttt)
		REQ_HANDLER(CMSG_GUILD_SHOP_BUY, item_mgr_->terminal_shop_guild)
		REQ_HANDLER(CMSG_GUILD_SHOP_EX_BUY, item_mgr_->terminal_shop_guild_ex)
		REQ_HANDLER(CMSG_GUILD_MUBIAO, item_mgr_->terminal_mubiao_guild)
		REQ_HANDLER(CMSG_SPORT_SHOP_BUY, item_mgr_->terminal_shop_sport)
		REQ_HANDLER(CMSG_SPORT_MUBIAO, item_mgr_->terminal_mubiao_sport)
		REQ_HANDLER(CMSG_GUILD_TIME_BUY, item_mgr_->terminal_time_guild)
		REQ_HANDLER(CMSG_HUIYI_SHOP, item_mgr_->terminal_shop_huiyi)
		REQ_HANDLER(CMSG_LIEREN_SHOP, item_mgr_->terminal_shop_lieren)
		REQ_HANDLER(CMSG_HUIYI_LUCK_SHOP, item_mgr_->terminal_shop_huiyi_luck)
		REQ_HANDLER(CMSG_BY_SHOP_BUY, item_mgr_->terminal_shop_bingyuan)
		REQ_HANDLER(CMSG_BY_MUBIAO, item_mgr_->terminal_reward_bingyuan)
		REQ_HANDLER(CMSG_CHONGZHIFANPAI_BUY, item_mgr_->terminal_shop_chongzhifanpai)
		REQ_HANDLER(CMSG_PET_SHOP_BUY, item_mgr_->terminal_shop_chongwu)
		REQ_HANDLER(CMSG_PET_SHOP_REFRESH, item_mgr_->terminal_shop_chongwu_refresh)
		REQ_HANDLER(CMSG_MOFANG_BUY, item_mgr_->terminal_shop_mofang)

		REQ_HANDLER(CMSG_ACTIVE_REWARD, player_mgr_->terminal_active_reward)
		REQ_HANDLER(CMSG_ACTIVE_SCORE_REWARD, player_mgr_->terminal_active_score_reward)
		REQ_HANDLER(CMSG_POST_LOOK, post_mgr_->terminal_post_look)
		REQ_HANDLER(CMSG_POST_GET, post_mgr_->terminal_post_get)

		REQ_HANDLER(CMSG_SPORT_LOOK, sport_mgr_->terminal_sport_look)
		REQ_HANDLER(CMSG_SPORT_TOP, sport_mgr_->terminal_sport_top)
		REQ_HANDLER(CMSG_SPORT_FIGHT_END, sport_mgr_->terminal_sport_fight_end)
		REQ_HANDLER(CMSG_SPORT_REWARD, sport_mgr_->terminal_sport_reward)
		REQ_HANDLER(CMSG_SPORT_SAODANG, sport_mgr_->terminal_sport_saodang)
		REQ_HANDLER(CMSG_PLAYER_QIECUO, sport_mgr_->terminal_sport_qicuo)

		REQ_HANDLER(CMSG_VIP_REWARD, player_mgr_->terminal_vip_reward)
		REQ_HANDLER(CMSG_DJ, player_mgr_->terminal_dj)
		REQ_HANDLER(CMSG_DJTEN, player_mgr_->terminal_djten)
		REQ_HANDLER(CMSG_CHAT, social_mgr_->terminal_chat)
		REQ_HANDLER(CMSG_ONLINE_REWARD, player_mgr_->terminal_online_reward)
		REQ_HANDLER(CMSG_DAILY_SIGN, player_mgr_->terminal_daily_sign)
		REQ_HANDLER(CMSG_DAILY_SIGN_REWARD, player_mgr_->terminal_daily_sign_reward)
		REQ_HANDLER(CMSG_BOSS_LOOK, boss_mgr_->terminal_boss_look)
		REQ_HANDLER(CMSG_BOSS_LOOK_EX, boss_mgr_->terminal_boss_look_ex)
		REQ_HANDLER(CMSG_BOSS_FIGHT_END, boss_mgr_->terminal_boss_fight_end)
		REQ_HANDLER(CMSG_BOSS_RANK, boss_mgr_->terminal_boss_rank)
		REQ_HANDLER(CMSG_BOSS_ACTIVE_REWARD, boss_mgr_->terminal_boss_active)
		REQ_HANDLER(CMSG_BOSS_SAODANG, boss_mgr_->terminal_boss_saodang)
		REQ_HANDLER(CMSG_BOSS_ACTIVE_LOOK, boss_mgr_->terminal_boss_active_look)
		REQ_HANDLER(CMSG_BOSS_ITEM_APPLY, item_mgr_->terminal_item_apply)
		REQ_HANDLER(CMSG_HBB_LOOK, mission_mgr_->terminal_hbb_look)
		REQ_HANDLER(CMSG_HBB_REFRESH, mission_mgr_->terminal_hbb_refresh)
		REQ_HANDLER(CMSG_HBB_FIGHT_END, mission_mgr_->terminal_hbb_fight_end)
		REQ_HANDLER(CMSG_TTT_FIGHT_END, mission_mgr_->terminal_ttt_fight_end)
		REQ_HANDLER(CMSG_TTT_VALUE_LOOK, mission_mgr_->terminal_ttt_value_look)
		REQ_HANDLER(CMSG_TTT_VALUE, mission_mgr_->terminal_ttt_value)
		REQ_HANDLER(CMSG_TTT_REWARD, mission_mgr_->terminal_ttt_reward)
		REQ_HANDLER(CMSG_TTT_CZ, mission_mgr_->terminal_ttt_cz)
		REQ_HANDLER(CMSG_TTT_SANXING, mission_mgr_->terminal_ttt_sanxing)
		REQ_HANDLER(CMSG_TTT_SAODANG, mission_mgr_->terminal_ttt_saodang)
		REQ_HANDLER(CMSG_PLAYER_LOOK, player_mgr_->terminal_player_look)
		REQ_HANDLER(CMSG_RANDOM_EVENT_LOOK, player_mgr_->terminal_random_event_look)
		REQ_HANDLER(CMSG_RANDOM_EVENT_GET, player_mgr_->terminal_random_event_get)
		REQ_HANDLER(CMSG_EQUIP_KC, equip_mgr_->terminal_equip_kc)
		REQ_HANDLER(CMSG_MISSION_GOUMAI, mission_mgr_->terminal_mission_goumai)
		REQ_HANDLER(CMSG_EQUIP_SUIPIAN, equip_mgr_->terminal_equip_suipian)
		REQ_HANDLER(CMSG_SOCIAL_LOOK, social_mgr_->terminal_social_look)
		REQ_HANDLER(CMSG_SOCIAL_RAND, social_mgr_->terminal_social_rand)
		REQ_HANDLER(CMSG_SOCIAL_ADD, social_mgr_->terminal_social_add)
		REQ_HANDLER(CMSG_SOCIAL_LOOK_NEW, social_mgr_->terminal_social_look_new)
		REQ_HANDLER(CMSG_SOCIAL_AGREE, social_mgr_->terminal_social_agree)
		REQ_HANDLER(CMSG_SOCIAL_DELETE, social_mgr_->terminal_social_delete)
		REQ_HANDLER(CMSG_SOCIAL_SONG, social_mgr_->terminal_social_song)
		REQ_HANDLER(CMSG_SOCIAL_SHOU, social_mgr_->terminal_social_shou)
		REQ_HANDLER(CMSG_PLAYER_CHECK, player_mgr_->terminal_player_check)
		REQ_HANDLER(CMSG_DRESS_ON, equip_mgr_->terminal_dress_on);
		REQ_HANDLER(CMSG_DRESS_OFF, equip_mgr_->terminal_dress_off);
		REQ_HANDLER(CMSG_DRESS_BUY, equip_mgr_->terminal_dress_buy);
		REQ_HANDLER(CMSG_DRESS_UNLOCK, equip_mgr_->terminal_dress_unlock)
		REQ_HANDLER(CMSG_DRESS_UNLOCK_ACHIEVE, equip_mgr_->terminal_dress_unlock_achieve)
		//REQ_HANDLER(CMSG_LIBAO, player_mgr_->terminal_libao);
		REQ_HANDLER(CMSG_RECHARGE_CHECK_EX, player_mgr_->terminal_recharge_check_ex)
		REQ_HANDLER(CMSG_YB_REFRESH, mission_mgr_->terminal_yb_refresh)
		REQ_HANDLER(CMSG_YB_GW, mission_mgr_->terminal_yb_gw)
		REQ_HANDLER(CMSG_YB_ZH, mission_mgr_->terminal_yb_zh)
		REQ_HANDLER(CMSG_YB, mission_mgr_->terminal_yb)
		REQ_HANDLER(CMSG_YB_JIASU, mission_mgr_->terminal_yb_jiasu)
		REQ_HANDLER(CMSG_YB_FINISH, mission_mgr_->terminal_yb_finish)
		REQ_HANDLER(CMSG_YB_LOOK, mission_mgr_->terminal_yb_look)
		REQ_HANDLER(CMSG_YB_LOOK_EX, mission_mgr_->terminal_yb_look_ex)
		REQ_HANDLER(CMSG_YB_YBQ_FIGHT_END, mission_mgr_->terminal_yb_ybq_fight_end)
		REQ_HANDLER(CMSG_YB_REWARD, mission_mgr_->terminal_yb_reward)
		REQ_HANDLER(CMSG_ORE_FIGHT_END, mission_mgr_->terminal_ore_fight_end)
		REQ_HANDLER(CMSG_MISSION_FIRST, mission_mgr_->terminal_mission_first)
		REQ_HANDLER(CMSG_QIYU_FIGHT_END, mission_mgr_->terminal_qiyu_fight_end)
		REQ_HANDLER(CMSG_QIYU_CHECK, mission_mgr_->terminal_qiyu_check)
		REQ_HANDLER(CMSG_SOCIAL_CODE_LOOK, social_mgr_->terminal_social_code_look)

		// guild
		REQ_HANDLER(CMSG_GUILD_CREATE, guild_mgr_->terminal_guild_create)
		REQ_HANDLER(CMSG_GUILD_APPLY, guild_mgr_->terminal_guild_apply)
		REQ_HANDLER(CMSG_GUILD_AGREE, guild_mgr_->terminal_guild_agree)
		REQ_HANDLER(CMSG_GUILD_OPEN, guild_mgr_->terminal_guild_open)
		REQ_HANDLER(CMSG_GUILD_LIST_RECOMMEND, guild_mgr_->terminal_guild_list_recommend)
		REQ_HANDLER(CMSG_GUILD_QUERY, guild_mgr_->terminal_guild_query)
		REQ_HANDLER(CMSG_GUILD_MODIFY_ICON, guild_mgr_->terminal_guild_modify_icon)
		REQ_HANDLER(CMSG_GUILD_MODIFY_BULLETIN, guild_mgr_->terminal_guild_modify_bulletin)
		REQ_HANDLER(CMSG_GUILD_MODIFY_GUILD_NAME, guild_mgr_->terminal_guild_modify_name)
		REQ_HANDLER(CMSG_GUILD_CHANGE_MEMBER_DUTY, guild_mgr_->terminal_guild_change_member_duty)
		REQ_HANDLER(CMSG_GUILD_SET_JOIN_CONDITION, guild_mgr_->terminal_guild_set_join_condition)
		REQ_HANDLER(CMSG_GUILD_SIGN_IN, guild_mgr_->terminal_guild_sign_in)
		REQ_HANDLER(CMSG_GUILD_KICK_MEMBER, guild_mgr_->terminal_guild_kick_member)
		REQ_HANDLER(CMSG_GUILD_LEAVE, guild_mgr_->terminal_guild_leave)
		REQ_HANDLER(CMSG_GUILD_DISMISS, guild_mgr_->terminal_guild_dismiss)
		REQ_HANDLER(CMSG_GUILD_MEMBER_VIEW, guild_mgr_->terminal_guild_member_view)
		REQ_HANDLER(CMSG_GUILD_RANKING, guild_mgr_->terminal_guild_ranking)
		REQ_HANDLER(CMSG_GUILD_MISSION_RANKING, guild_mgr_->terminal_guild_mission_ranking)
		REQ_HANDLER(CMSG_GUILD_ACTIVITY, guild_mgr_->terminal_guild_activity)
		REQ_HANDLER(CMSG_GUILD_BOSS_LOOK, guild_mgr_->terminal_guild_mission_look)
		REQ_HANDLER(CMSG_GUILD_BOSS_LOOK_EX, guild_mgr_->terminal_guild_mission_look_ex)
		REQ_HANDLER(CMSG_GUILD_BOSS_FIGHT_END, guild_mgr_->terminal_guild_mission_fight_end)
		REQ_HANDLER(CMSG_GUILD_SIGN_REWARD, guild_mgr_->terminal_guild_sign_reward)
		REQ_HANDLER(CMSG_GUILD_MESSAGE_VIEW, guild_mgr_->terminal_guild_message_view)
		REQ_HANDLER(CMSG_GUILD_MESSAGE_ADD, guild_mgr_->terminal_guild_message_add)
		REQ_HANDLER(CMSG_GUILD_MESSAGE_DELETE, guild_mgr_->terminal_guild_message_delete)
		REQ_HANDLER(CMSG_GUILD_MESSAGE_TOP, guild_mgr_->terminal_guild_message_top)
		REQ_HANDLER(CMSG_GUILD_MISSION_BUY, guild_mgr_->terminal_guild_buy_mission_num)
		REQ_HANDLER(CMSG_GUILD_MISSION_COMPLETE_REWARD, guild_mgr_->terminal_guild_mission_complete_reward)
		REQ_HANDLER(CMSG_GUILD_MISSION_CENG_REWARD, guild_mgr_->terminal_guild_mission_shilian_reward)
		REQ_HANDLER(CMSG_GUILD_KEJI_OPEN, guild_mgr_->terminal_guild_keji_open)
		REQ_HANDLER(CMSG_GUILD_KEJI_UPLEVEL, guild_mgr_->terminal_guild_keji_up)
		REQ_HANDLER(CMSG_GUILD_KEJI_STUDY, guild_mgr_->terminal_guild_keji_study)
		REQ_HANDLER(CMSG_GUILD_KEJI_SKILLUP, guild_mgr_->terminal_guild_keji_skillup)
		REQ_HANDLER(CMSG_GUILD_TANHE, guild_mgr_->terminal_guild_tanhe)
		REQ_HANDLER(CMSG_GUILD_RED_VIEW, guild_mgr_->terminal_guild_red_view)
		REQ_HANDLER(CMSG_GUILD_RED_DELIVER, guild_mgr_->terminal_guild_red_deliver)
		REQ_HANDLER(CMSG_GUILD_RED_ROB, guild_mgr_->terminal_guild_red_rob)
		REQ_HANDLER(CMSG_GUILD_RED_TARGET, guild_mgr_->terminal_guild_red_target)
		REQ_HANDLER(CMSG_GUILD_MISSION_COMPLETE_REWARD_VIEW, guild_mgr_->terminal_guild__mission_complate_reward_view)
		REQ_HANDLER(CMSG_GIILD_JT_APPLY, guild_mgr_->terminal_guild_pvp_baoming)
		REQ_HANDLER(CMSG_GUILD_JT_LOOK, guild_mgr_->terminal_guild_pvp_look)
		REQ_HANDLER(CMSG_GUILD_JT_BUSHU, guild_mgr_->terminal_guild_pvp_bushu)
		REQ_HANDLER(CMSG_GUILD_JT_BUY,guild_mgr_->terminal_guild_pvp_buy)
		REQ_HANDLER(CMSG_GUILD_JT_REWARD,guild_mgr_->terminal_guild_pvp_reward)
		REQ_HANDLER(CMSG_PVP_GUILD_FIGHT, guild_mgr_->terminal_guild_pvp_fight)
		
		// rank
		REQ_HANDLER(CMSG_RANK_VIEW, rank_mgr_->terminal_view_rank);
		REQ_HANDLER(CMSG_RANK_TTT_VIEW, rank_mgr_->terminal_view_rank);
		REQ_HANDLER(CMSG_RANK_XJBK_VIEW, rank_mgr_->terminal_view_rank);
		REQ_HANDLER(CMSG_RANK_ORE_VIEW, rank_mgr_->terminal_view_rank);
		REQ_HANDLER(CMSG_RANK_HUIYI_VIEW, rank_mgr_->terminal_view_huiyi_rank)

		// »î¶¯
		REQ_HANDLER(CMSG_HUODONG_VIEW, huodong_mgr_->terminal_huodong_view)
		REQ_HANDLER(CMSG_HUODONG4_PTTQ, huodong_mgr_->terminal_huodong_pttq)
		REQ_HANDLER(CMSG_HUODONG4_VIEW, huodong_mgr_->terminal_huodong_pttq_view)
		REQ_HANDLER(CMSG_HUODONG_KAIFU_LOOK, huodong_mgr_->terminal_huodong_kaifu_view)
		REQ_HANDLER(CMSG_HUODONG_KAIFU_REWARD, huodong_mgr_->terminal_huodong_kaifu)
		REQ_HANDLER(CMSG_HUODONG_CZJH_BUY, huodong_mgr_->terminal_huodong_czjh_buy)
		REQ_HANDLER(CMSG_HUODONG_CZJH_GET, huodong_mgr_->terminal_huodong_czjh_get)
		REQ_HANDLER(CMSG_HUODONG_CZJH_RS, huodong_mgr_->terminal_huodong_czjhrs)
		REQ_HANDLER(CMSG_HUODONG_VIP_LIBAO, huodong_mgr_->terminal_huodong_vip_libao)
		REQ_HANDLER(CMSG_HUODONG_WEEK_LIBAO, huodong_mgr_->terminal_huodong_week_libao)
		REQ_HANDLER(CMSG_HUODONG_LJCZ_VIEW, huodong_mgr_->terminal_huodong_ljcz_view)
		REQ_HANDLER(CMSG_HUODONG_LJCZ, huodong_mgr_->terminal_huodong_ljcz)
		REQ_HANDLER(CMSG_HUODONG_DBCZ_VIEW, huodong_mgr_->terminal_huodong_dbcz_view)
		REQ_HANDLER(CMSG_HUODONG_DBCZ, huodong_mgr_->terminal_huodong_dbcz)
		REQ_HANDLER(CMSG_HUODONG_DLSL_VIEW, huodong_mgr_->terminal_huodong_dlsl_view)
		REQ_HANDLER(CMSG_HUODONG_DLSL, huodong_mgr_->terminal_huodong_dlsl)
		REQ_HANDLER(CMSG_HUODONG_EXCHANGE_VIEW, huodong_mgr_->terminal_huodong_zkfs_view)
		REQ_HANDLER(CMSG_HUODONG_EXCHANGE, huodong_mgr_->terminal_huodong_zkfs)
		REQ_HANDLER(CMSG_HUODONG_HYHD_VIEW, huodong_mgr_->terminal_huodong_hyhd_view)
		REQ_HANDLER(CMSG_HUODONG_HYHD, huodong_mgr_->terminal_huodong_hyhd)
		REQ_HANDLER(CMSG_HUODONG_DJDH_VIEW, huodong_mgr_->terminal_huodong_djdh_view)
		REQ_HANDLER(CMSG_HUODONG_DJDH, huodong_mgr_->terminal_huodong_djdh)
		REQ_HANDLER(CMSG_HUODONG_RQDL_VIEW, huodong_mgr_->terminal_huodong_rqdl_view)
		REQ_HANDLER(CMSG_HUODONG_RQDL, huodong_mgr_->terminal_huodong_rqdl)
		REQ_HANDLER(CMSG_HUODONG_JIERI_VIEW, huodong_mgr_->terminal_huodong_jiri)
		REQ_HANDLER(CMSG_HUODONG_XHQD_VIEW, huodong_mgr_->terminal_huodong_xingheqidian)
		REQ_HANDLER(CMSG_HUODONG_TANBAO_VIEW, huodong_mgr_->terminal_huodong_tanbao_view)
		REQ_HANDLER(CMSG_HUODONG_TANBAO_DICE, huodong_mgr_->terminal_huodong_tanbao_dice)
		REQ_HANDLER(CMSG_HUODONG_TANBAO_MUBIAO, huodong_mgr_->terminal_huodong_tanbao_mubiao)
		REQ_HANDLER(CMSG_HUODONG_TANBAO_SHOP, huodong_mgr_->terminal_huodong_tanbao_shop)
		REQ_HANDLER(CMSG_HUODONG_FANPAI_VIEW, huodong_mgr_->terminal_huodong_fanpai_view)
		REQ_HANDLER(CMSG_HUODONG_FANPAI, huodong_mgr_->terminal_huodong_fanpai)
		REQ_HANDLER(CMSG_HUODONG_FANPAI_CZ, huodong_mgr_->terminal_huodong_fanpai_cz)
		REQ_HANDLER(CMSG_HUODONG_ZHUANPAN_VIEW, huodong_mgr_->terminal_huodong_zhuanpan_view)
		REQ_HANDLER(CMSG_HUODONG_ZHUANPAN, huodong_mgr_->terminal_huodong_zhuanpan)
		REQ_HANDLER(CMSG_HUODONG_MANYOU, huodong_mgr_->terminal_huodong_tansuo)
		REQ_HANDLER(CMSG_HUODONG_MANYOU_VIEW, huodong_mgr_->terminal_huodong_tansuo_view)
		REQ_HANDLER(CMSG_HUODONG_MANYOU_EVENT, huodong_mgr_->terminal_huodong_tansuo_event)
		REQ_HANDLER(CMSG_HUODONG_MANYOU_MUBIAO, huodong_mgr_->terminal_huodong_tansuo_mubiao)
		REQ_HANDLER(CMSG_HUODONG_MANYOU_EVENT_REFRESH, huodong_mgr_->terminal_huodong_tansuo_event_refresh)
		REQ_HANDLER(CMSG_HUODONG_MANYOU_DEL, huodong_mgr_->terminal_huodong_tansuo_event_del)
		REQ_HANDLER(CMSG_HUODONG_MOFANG_VIEW, huodong_mgr_->terminal_huodong_mofang_view)
		REQ_HANDLER(CMSG_HUODONG_MOFANG_START, huodong_mgr_->terminal_huodong_mofang_chou)
		REQ_HANDLER(CMSG_HUODONG_MOFANG_ALL, huodong_mgr_->terminal_huodong_mofangall)
		REQ_HANDLER(CMSG_HUODONG_MOFANG_REFRESH, huodong_mgr_->terminal_huodong_mofang_refresh)
		REQ_HANDLER(CMSG_HUODONG_MOFANG_RESET, huodong_mgr_->terminal_huodong_mofang_reset)
		REQ_HANDLER(CMSG_HUODONG_MOFANG_CHOU, huodong_mgr_->terminal_huodong_mofang)
		REQ_HANDLER(CMSG_HUODONG_MOFANG_TARGET, huodong_mgr_->terminal_huodong_mofang_mubiao)
		REQ_HANDLER(CMSG_HUODONG_YUEKA_VIEW,huodong_mgr_->terminal_huodong_yueka_view)
		REQ_HANDLER(CMSG_HUODONG_YUEKA_REWARD,huodong_mgr_->terminal_huodong_yueka_reward)
		REQ_HANDLER(CMSG_HUODONG_HUIGUI_REWARD,huodong_mgr_->terminal_huodong_huigui_reward)
		REQ_HANDLER(CMSG_HUODONG_ZHICHONG_VIEW, huodong_mgr_->terminal_huodong_zhichong_view)

		// treasure
		REQ_HANDLER(CMSG_TREASURE_EXPAND, treasure_mgr_->terminal_treasure_expand)
		REQ_HANDLER(CMSG_TREASURE_ENHANCE, treasure_mgr_->terminal_treasure_enhance)
		REQ_HANDLER(CMSG_TREASURE_JINLIAN, treasure_mgr_->terminal_treasure_jinlian)
		REQ_HANDLER(CMSG_TREASURE_LOCK, treasure_mgr_->terminal_treasure_lock)
		REQ_HANDLER(CMSG_TREASURE_HECHENG, treasure_mgr_->terminal_treasure_hecheng)
		REQ_HANDLER(CMSG_TREASURE_STAR, treasure_mgr_->terminal_treasure_star)
		REQ_HANDLER(CMSG_TREASURE_INIT, treasure_mgr_->terminal_treasure_init)
		REQ_HANDLER(CMSG_TREASURE_EQUICP, treasure_mgr_->terminal_treasure_equip)
		REQ_HANDLER(CMSG_TREASURE_VIEW, treasure_mgr_->terminal_treasure_rob_view)
		REQ_HANDLER(CMSG_TREASURE_PROTECT, treasure_mgr_->terminal_treasure_rob_protect)
		REQ_HANDLER(CMSG_TREASURE_BUY, treasure_mgr_->terminal_treasure_rob_buy)
		REQ_HANDLER(CMSG_TREASURE_FIGHT_END, treasure_mgr_->terminal_treasure_rob_fight_end)
		REQ_HANDLER(CMSG_TREASURE_SAODANG, treasure_mgr_->terminal_treasure_saodang)
		REQ_HANDLER(CMSG_TREASURE_YIJIAN_SAODANG, treasure_mgr_->terminal_treasure_yi_saodang)
		REQ_HANDLER(CMSG_TREASURE_REPORT, treasure_mgr_->terminal_treasure_report)
		REQ_HANDLER(CMSG_TREASURE_POINT, treasure_mgr_->terminal_treasure_point)
		REQ_HANDLER(CMSG_TREASURE_REPORT_EX, treasure_mgr_->terminal_treasure_report_ex)
		REQ_HANDLER(CMSG_TREASURE_RONGLIAN, treasure_mgr_->terminal_treasure_ronglian)
		REQ_HANDLER(CMSG_TREASURE_ZHUZAO, treasure_mgr_->terminal_treasure_zhuzao)

		REQ_HANDLER(CMSG_PVP_VIEW, pvp_mrg_->terminal_pvp_view)
		REQ_HANDLER(CMSG_PVP_REFRESH, pvp_mrg_->terminal_pvp_refresh)
		REQ_HANDLER(CMSG_PVP_BUY, pvp_mrg_->terminal_pvp_buy)
		REQ_HANDLER(CMSG_PVP_FIGHT_END, pvp_mrg_->terminal_pvp_fight)
		REQ_HANDLER(CMSG_PVP_ACTIVE, pvp_mrg_->terminal_pvp_active)
		REQ_HANDLER(CMSG_TEAM_ENTER, pvp_mrg_->terminal_team_enter)
		REQ_HANDLER(CMSG_TEAM_INVITE_LOOK, social_mgr_->terminal_social_invite_look)
		REQ_HANDLER(CMSG_BINGYUAN_FIGHT_END, pvp_mrg_->terminal_bingyuan_fight)
		REQ_HANDLER(CMSG_TEAM_INVITE, pvp_mrg_->terminal_team_invite)
		REQ_HANDLER(CMSG_BINGYUAN_BUY_REWARD, pvp_mrg_->terminal_bingyuan_reward_buy)
		REQ_HANDLER(CMSG_DS_FIGHT_BUY, pvp_mrg_->terminal_ds_num_buy)
		REQ_HANDLER(CMSG_DS_FIGHT_END, pvp_mrg_->terminal_ds_fight)
		REQ_HANDLER(CMSG_DS_ACTIVE, pvp_mrg_->terminal_ds_target)
		REQ_HANDLER(CMSG_DS_TIME_BUY, pvp_mrg_->terminal_ds_time_buy)

		// tmsg
		REQ_HANDLER(TMSG_GONGGAO, player_mgr_->terminal_gonggao);
		REQ_HANDLER(TMSG_RECHARGE_HEITAO, player_mgr_->terminal_recharge_heitao);
		REQ_HANDLER(TMSG_KICK, player_mgr_->terminal_kick);
		//REQ_HANDLER(TMSG_GAG, player_mgr_->terminal_gag);
		REQ_HANDLER(TMSG_HUODONG, huodong_mgr_->terminal_huodong_add);
		REQ_HANDLER(TMSG_HUODONG_MODIFY, huodong_mgr_->terminal_huodong_modify)
		REQ_HANDLER(TMSG_SERVER_STAT, player_mgr_->terminal_server_stat)
		REQ_HANDLER(TMSG_LIBAO_EXCHANGE, player_mgr_->terminal_libao_exchange)

	END_REQ_MAP

	int init();

	int fini();

private:
	PlayerManager *player_mgr_;
	ItemManager *item_mgr_;
	MissionManager *mission_mgr_;
	RoleManager *role_mgr_;
	EquipManager *equip_mgr_;
	PostManager *post_mgr_;
	SportManager *sport_mgr_;
	BossManager *boss_mgr_;
	SocialManager *social_mgr_;
	GuildManager *guild_mgr_;
	RankManager *rank_mgr_;
	HuodongManager *huodong_mgr_;
	TreasureManager *treasure_mgr_;
	PvpManager* pvp_mrg_;
};

#endif
