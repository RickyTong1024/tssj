#include "pool.h"
#include "dhc.h"
#include "name.h"
#include <ace/OS_NS_unistd.h>
#include "game.h"

Pool::Pool()
: dhc_(0)
, name_(0)
, tick_(0)
, stop_(true)
, mysql_(0)
{

}

Pool::~Pool()
{
	
}

int Pool::init(const std::string &name)
{
	mysql_ = boost::lexical_cast<int>(game::env()->get_id_value(name, "mysql"));
	if (mysql_ != 1)
	{
		return 0;
	}
	tick_ = boost::lexical_cast<int>(game::env()->get_game_value("tick"));
	dhc_ = new Dhc();
	if (-1 == dhc_->init())
	{
		return -1;
	}
	name_ = new Name();
	if (-1 == name_->init())
	{
		return -1;
	}
	stop_ = false;
	thlist_.push_back(0);
	thlist_.push_back(1);
	this->activate(THR_NEW_LWP | THR_JOINABLE | THR_INHERIT_SCHED, 2);
	return 0;
}

int Pool::fini()
{
	if (mysql_ != 1)
	{
		return 0;
	}
	stop_ = true;
	this->wait();

	if (dhc_)
	{
		dhc_->fini();
		delete dhc_;
		dhc_ = 0;
	}

	if (name_)
	{
		name_->fini();
		delete name_;
		name_ = 0;
	}

	std::map< int, std::map<uint64_t, Object> >::iterator it = entity_map_.begin();
	while (it != entity_map_.end())
	{
		std::map<uint64_t, Object> &entity_type_map = (*it).second;
		std::map<uint64_t, Object>::iterator jt = entity_type_map.begin();
		while (jt != entity_type_map.end())
		{
			google::protobuf::Message *entity = (*jt).second.entity;
			delete entity;
			entity_type_map.erase(jt++);
		}
		entity_map_.erase(it++);
	}
	return 0;
}

int Pool::add (uint64_t guid, google::protobuf::Message *entity, estatus es)
{
	int type = type_of_guid(guid);
	std::map<uint64_t, Object> &entity_type_map = entity_map_[type];
	std::map<uint64_t, Object>::iterator it = entity_type_map.find(guid);
	if (it == entity_type_map.end())
	{
		if (es == state_none)
		{
			entity->clear_changed();
		}
		Object ob;
		ob.entity = entity;
		ob.es = es;
		entity_type_map[guid] = ob;
		if (type == et_player)
		{
			off_map_[guid] = 0;
		}
		return 0;
	}
	return -1;
}

int Pool::remove (uint64_t guid, uint64_t ref_guid)
{
	int type = type_of_guid(guid);
	std::map<uint64_t, Object> &entity_type_map = entity_map_[type];
	std::map<uint64_t, Object>::iterator it = entity_type_map.find(guid);
	if (it != entity_type_map.end())
	{
		if ((*it).second.es != mmg::Pool::state_new)
		{
			if (ref_guid)
			{
				ref_map_[ref_guid].push_back(guid);
			}
			else
			{
				Request *req = new Request();
				req->add(opc_remove, guid, 0);
				upcall(req, 0);
			}
		}

		google::protobuf::Message *entity = (*it).second.entity;
		delete entity;
		entity_type_map.erase(it);
		if (type == et_player)
		{
			off_map_.erase(guid);
		}
		return 0;
	}
	return -1;
}

int Pool::remove_ref (uint64_t ref_guid)
{
	if (ref_map_.find(ref_guid) != ref_map_.end())
	{
		for (int i = 0; i < ref_map_[ref_guid].size(); ++i)
		{
			uint64_t guid = ref_map_[ref_guid][i];
			Request *req = new Request();
			req->add(opc_remove, guid, 0);
			upcall(req, 0);
		}
		ref_map_.erase(ref_guid);
	}
	return 0;
}

google::protobuf::Message * Pool::release (uint64_t guid)
{
	int type = type_of_guid(guid);
	std::map<uint64_t, Object> &entity_type_map = entity_map_[type];
	std::map<uint64_t, Object>::iterator it = entity_type_map.find(guid);
	if (it != entity_type_map.end())
	{
		google::protobuf::Message *entity = (*it).second.entity;
		entity_type_map.erase(it);
		if (type == et_player)
		{
			off_map_.erase(guid);
		}
		return entity;
	}
	return 0;
}

google::protobuf::Message * Pool::get (uint64_t guid)
{
	int type = type_of_guid(guid);
	std::map<uint64_t, Object> &entity_type_map = entity_map_[type];
	std::map<uint64_t, Object>::iterator it = entity_type_map.find(guid);
	if (it != entity_type_map.end())
	{
		return (*it).second.entity;
	}
	return 0;
}

mmg::Pool::estatus Pool::get_state (uint64_t guid)
{
	int type = type_of_guid(guid);
	std::map<uint64_t, Object> &entity_type_map = entity_map_[type];
	std::map<uint64_t, Object>::iterator it = entity_type_map.find(guid);
	if (it != entity_type_map.end())
	{
		return (*it).second.es;
	}
	return mmg::Pool::state_none;
}

void Pool::set_state (uint64_t guid, estatus es)
{
	int type = type_of_guid(guid);
	std::map<uint64_t, Object> &entity_type_map = entity_map_[type];
	std::map<uint64_t, Object>::iterator it = entity_type_map.find(guid);
	if (it != entity_type_map.end())
	{
		(*it).second.es = es;
	}
}

void Pool::get_entitys (int type, std::vector<uint64_t> &guids)
{
	std::map<uint64_t, Object> &entity_type_map = entity_map_[type];
	for (std::map<uint64_t, Object>::iterator it = entity_type_map.begin(); it != entity_type_map.end(); ++it)
	{
		guids.push_back((*it).first);
	}
}

bool Pool::full()
{
	ACE_Guard<ACE_Thread_Mutex> t(chain_);
	return upcaller_.size() > 2000;
}

int Pool::upcall (Request *req, Upcaller caller)
{
	ACE_Guard<ACE_Thread_Mutex> t (chain_);
	upcaller_.push_back(std::pair<Request *, Upcaller>(req, caller));
	return 0;
}

int Pool::doupcall ()
{
	while (!stop_)
	{
		std::pair<Request *, Upcaller> ru;
		{
			ACE_Guard<ACE_Thread_Mutex> t (chain1_);
			if (docaller_.empty())
			{
				break;
			}
			ru = docaller_.front();
			docaller_.pop_front();
		}
		Upcaller &up = ru.second;
		if (up != 0)
		{
			up(ru.first);
		}
		delete ru.first;
	}
	return 0;
}

int Pool::upcall_svc()
{
	while (1)
	{
		bool sflag = false;
		{
			ACE_Guard<ACE_Thread_Mutex> t(chain_);
			if (upcaller_.empty())
			{
				sflag = true;
			}
		}
		if (stop_ && sflag)
		{
			break;
		}
		bool flag = true;
		while (flag)
		{
			std::pair<Request *, Upcaller> ru;
			flag = false;
			{
				ACE_Guard<ACE_Thread_Mutex> t (chain_);
				if (!upcaller_.empty())
				{
					ru = upcaller_.front();
					upcaller_.pop_front();
					flag = true;
				}
			}

			if (flag)
			{
				dhc_->do_request(ru.first);
				{
					ACE_Guard<ACE_Thread_Mutex> t (chain1_);
					docaller_.push_back(ru);
				}
			}
		}
		ACE_Time_Value tv(0, tick_ * 1000);
		ACE_OS::sleep(tv);
	}
	return 0;
}

int Pool::namecall (NameStr *name, Namecaller caller)
{
	ACE_Guard<ACE_Thread_Mutex> t (nchain_);
	namecaller_.push_back(std::pair<NameStr *, Namecaller>(name, caller));
	return 0;
}

int Pool::donamecall (void)
{
	while (!stop_)
	{
		std::pair<NameStr *, Namecaller> nn;
		{
			ACE_Guard<ACE_Thread_Mutex> t (nchain1_);
			if (donamecaller_.empty())
			{
				break;
			}
			nn = donamecaller_.front();
			donamecaller_.pop_front();
		}
		Namecaller &nc = nn.second;
		if (nc != 0)
		{
			nc(nn.first);
		}
		delete nn.first;
	}
	return 0;
}

int Pool::namecall_svc()
{
	while (1)
	{
		bool sflag = false;
		{
			ACE_Guard<ACE_Thread_Mutex> t(nchain_);
			if (namecaller_.empty())
			{
				sflag = true;
			}
		}
		if (stop_ && sflag)
		{
			break;
		}
		bool flag = true;
		while (flag)
		{
			std::pair<NameStr *, Namecaller> nc;
			flag = false;
			{
				ACE_Guard<ACE_Thread_Mutex> t (nchain_);
				if (!namecaller_.empty())
				{
					nc = namecaller_.front();
					namecaller_.pop_front();
					flag = true;
				}
			}

			if (flag)
			{
				name_->do_name(nc.first);
				{
					ACE_Guard<ACE_Thread_Mutex> t (nchain1_);
					donamecaller_.push_back(nc);
				}
			}
		}
		ACE_Time_Value tv(0, tick_ * 1000);
		ACE_OS::sleep(tv);
	}
	return 0;
}

int Pool::svc()
{
	int num = 0;
	{
		ACE_Guard<ACE_Thread_Mutex> t (listch_);
		num = thlist_.front();
		thlist_.pop_front();
	}
	if (num == 0)
	{
		upcall_svc();
	}
	else if (num == 1)
	{
		namecall_svc();
	}
	
	return 0;
}
