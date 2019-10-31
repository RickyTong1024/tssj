#include "chat_manager.h"

ChatManager::ChatManager()
{
	
}

ChatManager::~ChatManager()
{
	
}

int ChatManager::init()
{
	return 0;
}

int ChatManager::fini()
{
	return 0;
}

int ChatManager::terminal_enter_world(Packet *pck)
{
	int hid = pck->hid();
	hids_.insert(hid);
	return 0;
}

int ChatManager::terminal_leave_world(Packet *pck)
{
	int hid = pck->hid();
	hids_.erase(hid);
	if (hids_guids_.find(hid) != hids_guids_.end())
	{
		uint64_t guid = hids_guids_[hid];
		hids_guids_.erase(hid);
		guids_hids_.erase(guid);
	}
	return 0;
}

int ChatManager::terminal_chat_player(Packet *pck)
{
	int hid = pck->hid();
	uint64_t guid = pck->guid();
	guids_hids_[guid] = hid;
	hids_guids_[hid] = guid;
	return 0;
}

int ChatManager::push_chat(const std::string &data, const std::string &name)
{
	protocol::game::pmsg_chat msg;
	if (!msg.ParseFromString(data))
	{
		return -1;
	}
	const protocol::game::smsg_chat &smsg = msg.msg_chat();
	std::string s;
	smsg.SerializeToString(&s);

	if (smsg.type() == 0)
	{
		for (std::set<int>::iterator it = hids_.begin(); it != hids_.end(); ++it)
		{
			int hid = *it;
			Packet *pck = Packet::New(SMSG_CHAT, hid, 0, s);
			game::tcp_service()->send_msg(hid, pck);
		}
	}
	else
	{
		for (int i = 0; i < msg.player_guids_size(); ++i)
		{
			uint64_t guid = msg.player_guids(i);
			if (guids_hids_.find(guid) != guids_hids_.end())
			{
				int hid = guids_hids_[guid];
				Packet *pck = Packet::New(SMSG_CHAT, hid, 0, s);
				game::tcp_service()->send_msg(hid, pck);
			}
		}
	}
	return 0;
}

int ChatManager::push_gundong(const std::string &data, const std::string &name)
{
	for (std::set<int>::iterator it = hids_.begin(); it != hids_.end(); ++it)
	{
		int hid = *it;
		Packet *pck = Packet::New(SMSG_GUNDONG, hid, 0, data);
		game::tcp_service()->send_msg(hid, pck);
	}
	return 0;
}
int ChatManager::push_gundong_server(const std::string &data, const std::string &name)
{
	for (std::set<int>::iterator it = hids_.begin(); it != hids_.end(); ++it)
	{
		int hid = *it;
		Packet *pck = Packet::New(SMSG_GUNDOG_SERVER, hid, 0, data);
		game::tcp_service()->send_msg(hid, pck);
	}
	return 0;
}
