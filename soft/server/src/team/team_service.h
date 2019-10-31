#ifndef __TEAM_MANAGER_H__
#define __TEAM_MANAGER_H__

#include "gameinc.h"
#include "packet.h"


class TeamService : public mmg::GameInterface
{
public:
	TeamService();

	~TeamService();

	BEGIN_PACKET_MAP
		PACKET_HANDLER(CMSG_ENTER_WORLD, terminal_enter_world)
		PACKET_HANDLER(CMSG_ENTER_TEAM_SERVER, terminal_login_world)
		PACKET_HANDLER(CMSG_LEAVE_WORLD, terminal_leave_world)
		PACKET_HANDLER(CMSG_CREATE_TEAM, terminal_create_team)
		PACKET_HANDLER(CMSG_ENTER_TEAM, terminal_enter_team)
		PACKET_HANDLER(CMSG_LEAVE_TEAM, terminal_leave_team)
		PACKET_HANDLER(CMSG_KICK_TEAM, terminal_kick_team)
		PACKET_HANDLER(CMSG_PREPARE_TEAM, terminal_prepare_team)
		PACKET_HANDLER(CMSG_FIGHT_TEAM, terminal_fight_team)
		PACKET_HANDLER(CMSG_MATCH_TEAM, terminal_match_team)
		PACKET_HANDLER(CMSG_END_MATCH_TEAM, terminal_end_match_team)
		PACKET_HANDLER(CMSG_OPEN_TEAM, terminal_open_team)
		PACKET_HANDLER(CMSG_CHAT_TEAM, terminal_chat_team)
		PACKET_HANDLER(CMSG_MOVE_TEAM, terminal_move_team)
		PACKET_HANDLER(CMSG_VIEW_TEAM_MEMBER, terminal_view_team)
		PACKET_HANDLER(CMSG_SOCIAL_INVITE, terminal_invite_agree)
		PACKET_HANDLER(CMSG_INVITE_ALL, terminal_invite_all)
		PACKET_HANDLER(CMSG_STOP_FIGHT_TEAM, terminal_fight_end_team)
		PACKET_HANDLER(CMSG_VIEW_CHENGHAO_RANK, terminal_view_rank)
		PACKET_HANDLER(CMSG_TEAM_URGE, terminal_urge_team)
		PACKET_HANDLER(CMSG_LEADER_PREPARE, terminal_leader_prepare)
		PACKET_HANDLER(CMSG_DS_MATCH, terminal_ds_match)
		PACKET_HANDLER(CMSG_VIEW_DS_RANK, terminal_ds_rank)
		PACKET_HANDLER(CMSG_DS_MATCH_STOP, terminal_ds_stop)
		PACKET_HANDLER(CMSG_QIECUO, terminal_qiecuo)
	END_PACKET_MAP

	BEGIN_PUSH_MAP
	END_PUSH_MAP

	BEGIN_REQ_MAP
		REQ_HANDLER(TMSG_TEAM_ENTER, rpc_enter_world)
		REQ_HANDLER(TMSG_TEAM_INVITE, rpc_invite)
		REQ_HANDLER(TMSG_BINGYUAN_FIGHT, rpc_bingyuan)
		REQ_HANDLER(TMSG_BINGYUAN_BUY, rpc_bingyuan_buy)
		REQ_HANDLER(TMSG_BINGYUAN_REWARD, rpc_bingyuan_reward)
		REQ_HANDLER(TMSG_TEAM_PULL, rpc_team_pull)
		REQ_HANDLER(TMSG_DS_FIGHT, rpc_ds_fight)
		REQ_HANDLER(TMSG_DS_TIME_BUY,rpc_ds_time_buy)
		REQ_HANDLER(TMSG_DS_GM,rpc_ds_gm)
	END_REQ_MAP

	int init();

	int fini();

private:
	int terminal_enter_world(Packet *pck);

	int terminal_leave_world(Packet *pck);

	int terminal_login_world(Packet *pck);

	int terminal_create_team(Packet *pck);

	int terminal_enter_team(Packet *pck);

	int terminal_match_team(Packet *pck);

	int terminal_end_match_team(Packet *pck);

	int terminal_leave_team(Packet *pck);

	int terminal_kick_team(Packet *pck);

	int terminal_prepare_team(Packet *pck);

	int terminal_fight_team(Packet *pck);

	int terminal_fight_end_team(Packet *pck);

	int terminal_open_team(Packet *pck);

	int terminal_chat_team(Packet *pck);

	int terminal_move_team(Packet *pck);

	int terminal_view_team(Packet *pck);

	int terminal_invite_agree(Packet *pck);

	int terminal_invite_all(Packet *pck);

	int terminal_view_rank(Packet *pck);

	int terminal_urge_team(Packet *pck);

	int terminal_leader_prepare(Packet *pck);

	int terminal_ds_match(Packet *pck);

	int terminal_ds_rank(Packet *pck);

	int terminal_ds_stop(Packet *pck);

	int terminal_qiecuo(Packet *pck);

private:
	void rpc_enter_world(const std::string &data, const std::string &name, int id);

	void rpc_invite(const std::string &data, const std::string &name, int id);

	void rpc_bingyuan(const std::string &data, const std::string &name, int id);

	void rpc_bingyuan_buy(const std::string &data, const std::string &name, int id);

	void rpc_bingyuan_reward(const std::string &data, const std::string &name, int id);

	void rpc_team_pull(const std::string &data, const std::string &name, int id);

	void rpc_ds_fight(const std::string &data, const std::string &name, int id);
	
	void rpc_ds_time_buy(const std::string &data, const std::string &name, int id);
	
	void rpc_ds_gm(const std::string &data, const std::string &name, int id);

	uint64_t get_guid(int hid) const;

	void player_enter(uint64_t player_guid);

private:
	std::set<int> hids_;
	std::map<int, uint64_t> hid_guids_;

	int team_timer_;
	int pool_timer_;
	int single_timer_;
};

#endif
