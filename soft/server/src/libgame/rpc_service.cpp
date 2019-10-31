#include "rpc_service.h"
#include "rpc_client.h"
#include "rpc_server.h"
#include <google/protobuf/message.h>
#include <boost/bind.hpp>
#include "rpc.pb.h"
#include "game.h"

RpcService::RpcService()
	: request_id_(0)
	, request_deal_id_(0)
	, rpc_server_(0)
	, gif_(0)
{

}

RpcService::~RpcService()
{
	
}

int RpcService::init(const std::string &name, mmg::GameInterface *gif)
{
	name_ = name;
	gif_ = gif;
	std::string host = game::env()->get_id_value(name, "host");
	std::string port = game::env()->get_id_value(name, "port");
	if (host == "" || port == "")
	{
		return -1;
	}
	std::string addr = "tcp://" + host + ":" + port;
	rpc_server_ = new RpcServer(name, addr);
	timer_ = game::timer()->schedule(boost::bind(&RpcService::dispacth, this, _1), 30, "rpc_service");

	return 0;
}

int RpcService::fini()
{
	if (timer_ != -1)
	{
		game::timer()->cancel(timer_);
		timer_ = -1;
	}
	if (rpc_server_)
	{
		delete rpc_server_;
	}
	for (std::map<std::string, RpcClient *>::iterator it = rpc_client_map_.begin(); it != rpc_client_map_.end(); ++it)
	{
		RpcClient *rpc_client_ = (*it).second;
		delete rpc_client_;
	}
	rpc_client_map_.clear();

	return 0;
}


void RpcService::request(const std::string &name, int opcode, const std::string &msg, ResponseFunc func)
{
	rpcproto::rpc rpc;
	rpc.set_type(rpcproto::REQUESST);
	rpcproto::request *req = rpc.mutable_req();
	req->set_name(name_);
	req->set_id(request_id_);
	req->set_opcode(opcode);
	if (msg != "")
	{
		req->set_msg(msg);
	}

	send(name, &rpc);

	s_request_func_time rft;
	rft.func = func;
	rft.time = game::timer()->now();
	response_func_map_[request_id_] = rft;
	request_id_++;
}

void RpcService::push(const std::string &name, int opcode, const std::string &msg)
{
	rpcproto::rpc rpc;
	rpc.set_type(rpcproto::PUSH);
	rpcproto::request *req = rpc.mutable_req();
	req->set_name(name_);
	req->set_id(0);
	req->set_opcode(opcode);
	if (msg != "")
	{
		req->set_msg(msg);
	};

	send(name, &rpc);
}

void RpcService::response(const std::string &name, int id, const std::string &msg, int error_code, const std::string &error_text)
{
	if (name == "self")
	{
		std::map<int, s_request_func_time>::iterator it = response_func_map_.find(id);
		if (it != response_func_map_.end())
		{
			ResponseFunc f = (*it).second.func;
			f(msg);
			response_func_map_.erase(it);
		}
		return;
	}
	rpcproto::rpc rpc;
	rpc.set_type(rpcproto::RESPONSE);
	rpcproto::response *rep = rpc.mutable_rep();
	rep->set_name(name_);
	rep->set_id(id);
	if (msg != "")
	{
		rep->set_msg(msg);
	}
	if (error_code)
	{
		rpcproto::error *err = rep->mutable_error();
		err->set_code(error_code);
		err->set_text(error_text);
	}

	send(name, &rpc);
}

void RpcService::send(const std::string &name, rpcproto::rpc *msg)
{
	RpcClient *rc = 0;
	std::map<std::string, RpcClient *>::iterator it = rpc_client_map_.find(name);
	if (it == rpc_client_map_.end())
	{
		std::string host = game::env()->get_id_value(name, "host");
		std::string port = game::env()->get_id_value(name, "port");
		if (host == "" || port == "")
		{
			return;
		}
		std::string addr = "tcp://" + host + ":" + port;
		rc = new RpcClient(name, addr);
		rpc_client_map_[name] = rc;
	}
	else
	{
		rc = (*it).second;
	}
	std::string s;
	msg->SerializeToString(&s);
	rc->putq(s);
}

void RpcService::self_request(int opcode, const std::string &msg, ResponseFunc func)
{
	s_self_request sr;
	sr.opcode = opcode;
	sr.msg = msg;
	sr.request_id = request_id_;
	self_request_.push_back(sr);

	s_request_func_time rft;
	rft.func = func;
	rft.time = game::timer()->now();
	response_func_map_[request_id_] = rft;
	request_id_++;
}

int RpcService::dispacth(const ACE_Time_Value &tv)
{
	std::list<std::string> msgs;
	rpc_server_->getq(msgs);

	if (!gif_)
	{
		return 0;
	}

	for (std::list<std::string>::iterator it = msgs.begin(); it != msgs.end(); ++it)
	{
		std::string data = *it;
		rpcproto::rpc rpc;
		rpc.ParseFromString(data);
		if (rpc.type() == rpcproto::REQUESST)
		{
			const rpcproto::request req = rpc.req();
			game::log()->debug("req op = <%d>", req.opcode());
			gif_->dispath_req_handle(req.opcode(), req.msg(), req.name(), req.id());
		}
		else if (rpc.type() == rpcproto::PUSH)
		{
			const rpcproto::request req = rpc.req();
			game::log()->debug("push op = <%d>", req.opcode());
			gif_->dispath_push_handle(req.opcode(), req.msg(), req.name());
		}
		else if (rpc.type() == rpcproto::RESPONSE)
		{
			const rpcproto::response rep = rpc.rep();
			std::map<int, s_request_func_time>::iterator it = response_func_map_.find(rep.id());
			if (it != response_func_map_.end())
			{
				ResponseFunc f = (*it).second.func;
				f(rep.msg());
				response_func_map_.erase(it);
			}
		}
	}

	for (std::list<s_self_request>::iterator it = self_request_.begin(); it != self_request_.end(); ++it)
	{
		s_self_request &sr = *it;
		game::log()->debug("self req op = <%d>", sr.opcode);
		gif_->dispath_req_handle(sr.opcode, sr.msg, "self", sr.request_id);
	}
	self_request_.clear();

	uint64_t now = game::timer()->now();
	while (request_deal_id_ < request_id_)
	{
		if (response_func_map_.find(request_deal_id_) == response_func_map_.end())
		{
			request_deal_id_++;
		}
		else if (now > response_func_map_[request_deal_id_].time + 30000)
		{
			response_func_map_.erase(request_deal_id_);
			request_deal_id_++;
		}
		else
		{
			break;
		}
	}
	return 0;
}
