#include "pvp_service.h"

PvpService::PvpService()
:pvp_manage_(0)
{
	
}

PvpService::~PvpService()
{
	
}

int PvpService::init()
{
	pvp_manage_ = new PvpManager();
	if (pvp_manage_->init() == -1)
	{
		return -1;
	}
	return 0;
}

int PvpService::fini()
{
	pvp_manage_->fini();
	delete pvp_manage_;

	return 0;
}
