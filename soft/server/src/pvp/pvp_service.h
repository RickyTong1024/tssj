#ifndef __GS_MANAGER_H__
#define __GS_MANAGER_H__

#include "gameinc.h"
#include "packet.h"
#include "pvp_manager.h"

class PvpService : public mmg::GameInterface
{
public:
	PvpService();

	~PvpService();

	BEGIN_PACKET_MAP
		PACKET_HANDLER(SELF_PLAYER_LOAD_LOOK, pvp_manage_->self_player_load_look)
	END_PACKET_MAP

	BEGIN_PUSH_MAP
	END_PUSH_MAP

	BEGIN_REQ_MAP
		REQ_HANDLER(CMSG_PVP_LOOK, pvp_manage_->terminal_player_look)
		REQ_HANDLER(CMSG_PVP_RANK, pvp_manage_->terminal_player_rank)
		REQ_HANDLER(TMSG_PVP_PUSH, pvp_manage_->terminal_player_push)
		REQ_HANDLER(TMSG_PVP_PULL, pvp_manage_->terminal_player_pull)
		REQ_HANDLER(TMSG_PVP_FIGHT, pvp_manage_->terminal_player_fight)
		REQ_HANDLER(TMSG_PVP_REWARD, pvp_manage_->terminal_player_reward)
		REQ_HANDLER(TMSG_MOFANG_POINT, pvp_manage_->terminal_player_mofang)
		REQ_HANDLER(TMSG_INVITE_CODE_CREATE, pvp_manage_->terminal_invite_code_gen)
		REQ_HANDLER(TMSG_INVITE_CODE_INPUT, pvp_manage_->terminal_invite_code_input)
		REQ_HANDLER(TMSG_INVITE_CODE_LEVEL, pvp_manage_->terminal_invite_code_level)
		REQ_HANDLER(TMSG_INVITE_CODE_PULL, pvp_manage_->terminal_invite_code_pull)
		REQ_HANDLER(CMSG_PVP_GUILD_ZHANGKUANG_LOOK, pvp_manage_->terminal_guild_zhankuang)
		REQ_HANDLER(CMSG_PVP_GUILD_ZHANJI_LOOK, pvp_manage_->terminal_guild_zhanji)
		REQ_HANDLER(TMSG_PVP_GUILD_LOOK_BUSHU,pvp_manage_->terminal_guild_look_bushu)
		REQ_HANDLER(TMSG_PVP_GUILD_LOOK_PIPEI,pvp_manage_->terminal_guild_look_pipei)
		REQ_HANDLER(TMSG_PVP_GUILD_LOOK_JINRIZHANJI,pvp_manage_->terminal_guild_jinrizhanji)
		REQ_HANDLER(TMSG_PVP_GUILD_LOOK_XIUZHAN, pvp_manage_->terminal_guild_look_xiuzhan)
		REQ_HANDLER(TMSG_PVP_GUILD_BUSHU,pvp_manage_->terminal_guild_bushu)
		REQ_HANDLER(TMSG_PVP_GUILD_FIGHT, pvp_manage_->terminal_guild_fight)
		REQ_HANDLER(TMSG_PVP_GUILD_MATCH, pvp_manage_->terminal_guild_pvp_match)
		REQ_HANDLER(TMSG_PVP_GUILD_BAOMING, pvp_manage_->terminal_guild_baoming)
		REQ_HANDLER(TMSG_PVP_GUILD_REWARD, pvp_manage_->terminal_guild_pvp_reward)
		REQ_HANDLER(TMSG_PVP_GUILD_TARGET, pvp_manage_->terminal_guild_pvp_target)
	END_REQ_MAP

	int init();

	int fini();

private:
	PvpManager* pvp_manage_;
};

#endif
