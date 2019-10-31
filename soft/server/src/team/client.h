#ifndef __PLAYER_H__
#define __PLAYER_H__

#include "gameinc.h"

namespace rpcproto {
	class tmsg_req_team_login;
}

class Client
{
public:
	Client()
		:guid_(0),
		hid_(-1)
	{

	}

	Client(uint64_t guid,
		const std::string& sig)
		:sig_(sig),
		guid_(guid),
		hid_(-1)
	{

	}

	dhc::player_t* get_player();
	const dhc::player_t * get_player() const;

	bool check(int hid, 
		uint64_t guid,
		const std::string& sig);

	void send_msg(uint16_t opcode,
		const google::protobuf::Message* msg = 0);

	int get_hid() const { return hid_; }
	void set_hid(int hid) { hid_ = hid; }

private:
	std::string sig_;
	uint64_t guid_;
	int hid_;
};

class ClientManager
{
public:
	int init();

	int fini();

	Client* get_client(uint64_t guid);

	void add_client(uint64_t guid,
		const rpcproto::tmsg_req_team_login &login);

	void add_client(uint64_t guid, const std::string &sig);

	void remove_client(uint64_t guid);

	void send_msg(uint64_t guid, uint16_t opcode,
		const google::protobuf::Message* msg = 0);

	const std::map<uint64_t, Client>& get_all_client() const { return client_map_; }

private:
	std::map<uint64_t, Client> client_map_;
};

#define sClientManager (Singleton<ClientManager>::instance())

#endif // __PLAYER_H__
