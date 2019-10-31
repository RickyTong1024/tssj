#include "game.h"
#include "game_interface.h"
#include "env.h"
#include "timer.h"
#include "game_service.h"
#include "rpc_service.h"
#include "sig.h"
#include "tcp_service.h"
#include "gtool.h"
#include "pool.h"
#include "channel.h"
#include "scheme.h"
#include "log.h"
#include <ace/Reactor.h>

#ifdef _WIN32
#include <ace/Select_Reactor.h>
#else
#include <ace/Dev_Poll_Reactor.h>
#endif

namespace game
{
	std::string name_;
	Env *env_;
	Log *log_;
	Timer *timer_;
	GameService *game_service_;
	RpcService *rs_;
	Sig *sig_;
	TcpService *ts_;
	GTool *gtool_;
	Pool *pool_;
	Channel *channel_;
	Scheme *scheme_;
	bool stop_ = false;

	int init(const std::string &name, const std::string &confpath, mmg::GameInterface *gif)
	{

#ifdef _WIN32
		ACE_Select_Reactor *select_reactor = new ACE_Select_Reactor();
		ACE_Reactor *reactor = new ACE_Reactor(select_reactor);
#else
		ACE_Dev_Poll_Reactor *dev_reactor = new ACE_Dev_Poll_Reactor();
		dev_reactor->restart(true);
		ACE_Reactor *reactor = new ACE_Reactor(dev_reactor);
#endif
		ACE_Reactor::instance(reactor, true);

		name_ = name;
		env_ = new Env();
		if (-1 == env_->init(confpath))
		{
			return -1;
		}

		log_ = new Log();
		if (-1 == log_->init(name))
		{
			return -1;
		}

		timer_ = new Timer();
		if (-1 == timer_->init())
		{
			return -1;
		}

		rs_ = new RpcService();
		if (-1 == rs_->init(name, gif))
		{
			return -1;
		}

		sig_ = new Sig();
		if (-1 == sig_->init())
		{
			return -1;
		}

		ts_ = new TcpService();
		if (-1 == ts_->init(name))
		{
			return -1;
		}

		gtool_ = new GTool();
		if (-1 == gtool_->init(name))
		{
			return -1;
		}

		pool_ = new Pool();
		if (-1 == pool_->init(name))
		{
			return -1;
		}

		channel_ = new Channel();
		if (-1 == channel_->init())
		{
			return -1;
		}

		scheme_ = new Scheme();
		if (-1 == scheme_->init())
		{
			return -1;
		}

		game_service_ = new GameService();
		if (-1 == game_service_->init(gif))
		{
			return -1;
		}

		printf("%s init\n", name_.c_str());

		return 0;
	}

	int run()
	{
		timer_->start();
		ACE_Reactor::instance()->run_reactor_event_loop();
		timer_->stop();
		return 0;
	}

	int fini()
	{
		stop_ = true;

		game_service_->fini();
		delete game_service_;

		scheme_->fini();
		delete scheme_;

		channel_->fini();
		delete channel_;

		pool_->fini();
		delete pool_;

		gtool_->fini();
		delete gtool_;		

		ts_->fini();
		delete ts_;

		sig_->fini();
		delete sig_;

		rs_->fini();
		delete rs_;

		timer_->fini();
		delete timer_;

		log_->fini();
		delete log_;

		env_->fini();
		delete env_;

		printf("%s fini\n", name_.c_str());
		return 0;
	}

	bool is_stop()
	{
		return stop_;
	}

	mmg::Env * env()
	{
		return env_;
	}

	mmg::Timer *timer()
	{
		return timer_;
	}

	mmg::GameService *game_service()
	{
		return game_service_;
	}

	mmg::RpcService * rpc_service()
	{
		return rs_;
	}

	mmg::TcpService * tcp_service()
	{
		return ts_;
	}

	mmg::GTool * gtool()
	{
		return gtool_;
	}

	mmg::Pool * pool()
	{
		return pool_;
	}

	mmg::Channel * channel()
	{
		return channel_;
	}

	mmg::Scheme *scheme()
	{
		return scheme_;
	}

	mmg::Log *log()
	{
		return log_;
	}
}
