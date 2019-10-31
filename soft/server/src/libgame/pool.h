#ifndef __POOL_H__
#define __POOL_H__

#include <ace/Task.h>
#include "typedefs.h"
#include "game_interface.h"
#include <map>
#include <list>
#include <vector>
#include <ace/Thread_Mutex.h>
#include <google/protobuf/message.h>


class Name;
class Dhc;

class Pool : public ACE_Task<ACE_NULL_SYNCH>, public mmg::Pool
{
public:

	Pool();

	~Pool();

	int init(const std::string &name);

	int fini();

	virtual int add (uint64_t guid, google::protobuf::Message *entity, estatus es);

	virtual int remove (uint64_t guid, uint64_t ref_guid);

	virtual int remove_ref (uint64_t ref_guid);

	virtual google::protobuf::Message * release (uint64_t guid);

	virtual google::protobuf::Message * get (uint64_t guid);

	virtual estatus get_state (uint64_t guid);

	virtual void set_state (uint64_t guid, estatus es);

	virtual void get_entitys (int type, std::vector<uint64_t> &guids);

	virtual bool full ();

	virtual int upcall (Request *req, Upcaller caller);

	virtual int doupcall (void);

	virtual int namecall (NameStr *name, Namecaller caller);

	virtual int donamecall (void);

	virtual int svc();

protected:
	int upcall_svc();

	int namecall_svc();

private:
	struct Object
	{
		google::protobuf::Message *entity;
		mmg::Pool::estatus es;
	};

	std::map< int, std::map<uint64_t, Object> > entity_map_;

	std::map< uint64_t, std::vector<uint64_t> > ref_map_;

	std::map<uint64_t, int> off_map_;

	std::list<int> thlist_;

	ACE_Thread_Mutex listch_;

	std::list< std::pair<Request *, Upcaller> > upcaller_;

	ACE_Thread_Mutex chain_;

	std::list< std::pair<Request *, Upcaller> > docaller_;

	ACE_Thread_Mutex chain1_;

	std::list< std::pair<NameStr *, Namecaller> > namecaller_;

	ACE_Thread_Mutex nchain_;

	std::list< std::pair<NameStr *, Namecaller> > donamecaller_;

	ACE_Thread_Mutex nchain1_;

	Dhc *dhc_;

	Name *name_;

	int tick_;

	bool stop_;

	int mysql_;
};

#endif
