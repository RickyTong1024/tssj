#ifndef __GAME_H__
#define __GAME_H__

#include <string>

namespace mmg {
	class Env;
	class Log;
	class RpcService;
	class Timer;
	class TcpService;
	class GTool;
	class Pool;
	class Channel;
	class GameService;
	class Scheme;
	class GameInterface;
}

namespace game {

	int init(const std::string &name, const std::string &confpath, mmg::GameInterface *gif);

	int run();

	int fini();

	bool is_stop();

	mmg::Env * env();

	mmg::Log *log();

	mmg::Timer * timer();

	mmg::GameService *game_service();

	mmg::RpcService * rpc_service();

	mmg::TcpService * tcp_service();

	mmg::GTool * gtool();

	mmg::Pool *pool();

	mmg::Channel *channel();

	mmg::Scheme *scheme();
}

#endif
