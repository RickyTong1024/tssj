#ifndef __CHAT_MANAGER_H__
#define __CHAT_MANAGER_H__

#include "gameinc.h"
#include "packet.h"

class ChatManager : public mmg::GameInterface
{
public:
	ChatManager();

	~ChatManager();

	BEGIN_PACKET_MAP
		PACKET_HANDLER(CMSG_ENTER_WORLD, terminal_enter_world)
		PACKET_HANDLER(CMSG_LEAVE_WORLD, terminal_leave_world)
		PACKET_HANDLER(CMSG_CHAT_PLAYER, terminal_chat_player)
	END_PACKET_MAP

	BEGIN_PUSH_MAP
		PUSH_HANDLER(PMSG_CHAT, push_chat)
		PUSH_HANDLER(PMSG_GUNDONG, push_gundong)
		PUSH_HANDLER(PMSG_GUNDONG_SERVER, push_gundong_server)
	END_PUSH_MAP

	BEGIN_REQ_MAP
	END_REQ_MAP

	int init();

	int fini();

private:
	int terminal_enter_world(Packet *pck);

	int terminal_leave_world(Packet *pck);

	int terminal_chat_player(Packet *pck);

	int push_chat(const std::string &data, const std::string &name);

	int push_gundong(const std::string &data, const std::string &name);

	int push_gundong_server(const std::string &data, const std::string &name);

private:
	std::set<int> hids_;
	std::map<uint64_t, int> guids_hids_;
	std::map<int, uint64_t> hids_guids_;
};

#endif
