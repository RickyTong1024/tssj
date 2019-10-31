#include "gtool.h"
#include <ace/OS_NS_sys_time.h>
#include "dhc.h"
#include "game.h"
#include "gameinc.h"
#include "utils.h"
#include <mysql++.h>

GTool::GTool()
: dhc_(0)
, cur_guid_(10000)
, next_guid_(10000)
, start_time_(0)
, start_random_day_(0)
{
	
}

GTool::~GTool()
{

}

int GTool::init(const std::string &name)
{
	mysql_ = boost::lexical_cast<int>(game::env()->get_id_value(name, "mysql"));
	if (mysql_ != 1)
	{
		return 0;
	}

	dhc_ = new Dhc();
	if (-1 == dhc_->init())
	{
		return -1;
	}

	if (-1 == dhc_->do_check())
	{
		printf("Failed to check database.\n");
		return -1;
	}

	qid_ = boost::lexical_cast<int>(game::env()->get_game_value("qid"));
	one_add_ = boost::lexical_cast<int>(game::env()->get_game_value("one_add"));

	if (read_guid() == -1)
	{
		insert_guid();
	}

	next_guid_ = cur_guid_ + one_add_;
	write_guid();

	get_start_time();

	get_random_day();

	return 0;
}

int GTool::fini()
{
	if (mysql_ != 1)
	{
		return 0;
	}
	dhc_->fini();
	delete dhc_;
	dhc_ = 0;

	return 0;
}

uint64_t GTool::assign (int et)
{
	uint64_t guid = MAKE_GUID_EX(et, qid_, cur_guid_);
	cur_guid_++;
	if (cur_guid_ > next_guid_)
	{
		next_guid_ += one_add_;
		write_guid();
	}
	return guid;
}

uint64_t GTool::start_time()
{
	return start_time_;
}

int GTool::random_start_day()
{
	return start_random_day_;
}

int GTool::read_guid()
{
	dhc::gtool_t *gtool = new dhc::gtool_t;
	Request req;
	req.add(opc_query, MAKE_GUID_EX(et_gtool, qid_, 0), gtool);
	
	if (dhc_->do_request(&req) != -1)
	{
		cur_guid_ = gtool->num();
		return 0;
	}
	return -1;
}

int GTool::insert_guid()
{
	dhc::gtool_t *gtool = new dhc::gtool_t;
	gtool->set_guid(MAKE_GUID_EX(et_gtool, qid_, 0));
	gtool->set_num(cur_guid_);
	Request req;
	req.add(opc_insert, gtool->guid(), gtool);

	if (dhc_->do_request(&req) == -1)
	{
		return -1;
	}

	return 0;
}

int GTool::write_guid()
{
	dhc::gtool_t *gtool = new dhc::gtool_t;
	gtool->set_guid(MAKE_GUID_EX(et_gtool, qid_, 0));
	gtool->set_num(next_guid_);
	Request req;
	req.add(opc_update, gtool->guid(), gtool);

	if (dhc_->do_request(&req) == -1)
	{
		return -1;
	}

	return 0;
}

void GTool::get_start_time()
{
	dhc::gtool_t *gtool = new dhc::gtool_t;
	Request req;
	req.add(opc_query, MAKE_GUID_EX(et_gtool, qid_, 1), gtool);

	if (dhc_->do_request(&req) != -1)
	{
		start_time_ = gtool->num();
		return;
	}

	ACE_OS::gettimeofday().msec(start_time_);

	dhc::gtool_t *gtool1 = new dhc::gtool_t;
	gtool1->set_guid(MAKE_GUID_EX(et_gtool, qid_, 1));
	gtool1->set_num(start_time_);
	Request req1;
	req1.add(opc_insert, gtool1->guid(), gtool1);

	dhc_->do_request(&req1);
}

void GTool::get_random_day()
{
	dhc::gtool_t *gtool = new dhc::gtool_t;
	Request req;
	req.add(opc_query, MAKE_GUID_EX(et_gtool, qid_, 2), gtool);

	if (dhc_->do_request(&req) != -1)
	{
		start_random_day_ = gtool->num();
		return;
	}

	start_random_day_ = Utils::get_int32(2, 5);

	dhc::gtool_t *gtool1 = new dhc::gtool_t;
	gtool1->set_guid(MAKE_GUID_EX(et_gtool, qid_, 2));
	gtool1->set_num(start_random_day_);
	Request req1;
	req1.add(opc_insert, gtool1->guid(), gtool1);

	dhc_->do_request(&req1);
}
