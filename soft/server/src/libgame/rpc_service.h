#ifndef __RPC_SERVICE_H__
#define __RPC_SERVICE_H__

#include "typedefs.h"
#include "game_interface.h"

class RpcClient;
class RpcServer;

namespace google
{
	namespace protobuf
	{
		class Message;
	}
}

namespace rpcproto
{
	class rpc;
}

struct s_self_request
{
	int opcode;
	std::string msg;
	int request_id;
};

struct s_request_func_time
{
	ResponseFunc func;
	uint64_t time;
};

class RpcService : public mmg::RpcService
{
public:
	RpcService();

	~RpcService();

	int init(const std::string &name, mmg::GameInterface *gif);

	int fini();

	void request(const std::string &name, int opcode, const std::string &msg, ResponseFunc func);

	void push(const std::string &name, int opcode, const std::string &msg);

	void response(const std::string &name, int id, const std::string &msg, int error_code = 0, const std::string &error_text = "");

	void self_request(int opcode, const std::string &msg, ResponseFunc func);

protected:
	void send(const std::string &name, rpcproto::rpc *msg);

	int dispacth(const ACE_Time_Value &tv);

private:
	int request_id_;
	int request_deal_id_;
	std::string name_;
	std::map<std::string, RpcClient *> rpc_client_map_;
	RpcServer *rpc_server_;
	mmg::GameInterface *gif_;
	std::map<int, s_request_func_time> response_func_map_;
	int timer_;
	std::list<s_self_request> self_request_;
};

#endif // !__RPC_SERVICE_H__
