#include "channel.h"
#include "game_interface.h"
#include "game.h"

#define CHANNEL_TIME 300000

namespace game {

Channel::Channel()
{

}

Channel::~Channel()
{

}

int Channel::init()
{
	return 0;
}

int Channel::fini()
{
	return 0;
}

void Channel::add_channel(uint64_t guid, TermInfo ti, bool is_login)
{
	ti.time = game::timer()->now();
	if (is_login)
	{
		ti.cztime = game::timer()->now();
	}
	else
	{
		ti.cztime = 0;
	}
	channels_[guid] = ti;
}

//void Channel::update_channel(uint64_t guid, uint64_t gag_time)
//{
//	if (channels_.find(guid) != channels_.end())
//	{
//		channels_[guid].gag_time = gag_time;
//	}
//}

TermInfo * Channel::get_channel(uint64_t guid)
{
	if (channels_.find(guid) != channels_.end())
	{
		TermInfo &ti = channels_[guid];
		return &ti;
	}
	return 0;
}

int Channel::get_channel_lang(uint64_t guid)
{
	if (channels_.find(guid) != channels_.end())
	{
		TermInfo& ti = channels_[guid];
		return ti.lang_ver;
	}
	return -1;
}

void Channel::del_channel(uint64_t guid)
{
	if (channels_.find(guid) != channels_.end())
	{
		channels_.erase(guid);
	}
}

void Channel::refresh_offline_time(uint64_t guid)
{
	if (channels_.find(guid) != channels_.end())
	{
		TermInfo &ti = channels_[guid];
		ti.time = game::timer()->now();
	}
}

int Channel::check_sig(uint64_t guid, const std::string &sig)
{
	if (channels_.find(guid) != channels_.end())
	{
		TermInfo &ti = channels_[guid];
		if (ti.sig == sig)
		{
			ti.time = game::timer()->now();
			ti.cztime = game::timer()->now();
			return 0;
		}
		else
		{
			return 1;
		}
	}
	return -1;
}

int Channel::get_channel_num()
{
	return channels_.size();
}

int Channel::get_real_channel_num()
{
	int num = 0;
	uint64_t t = game::timer()->now();
	for (std::map<uint64_t, TermInfo>::iterator it = channels_.begin(); it != channels_.end(); ++it)
	{
		TermInfo &ti = (*it).second;
		if (t - ti.cztime < CHANNEL_TIME)
		{
			num++;
		}
	}
	return num;
}

bool Channel::online(uint64_t guid)
{
	if (channels_.find(guid) != channels_.end())
	{
		TermInfo &ti = channels_[guid];
		uint64_t t = game::timer()->now();
		if (t - ti.cztime < CHANNEL_TIME)
		{
			return true;
		}
	}
	return false;
}

int Channel::check_pck(uint64_t guid, int pck_id)
{
	if (channels_.find(guid) != channels_.end())
	{
		TermInfo &ti = channels_[guid];
		if (ti.pck_id <= pck_id)
		{
			ti.pck_id = pck_id;
			return 0;
		}
		else if (ti.pck_id == pck_id + 1)
		{
			return 1;
		}
	}
	return -1;
}

void Channel::last_pck(uint64_t guid, std::string &pck)
{
	if (channels_.find(guid) != channels_.end())
	{
		TermInfo &ti = channels_[guid];
		pck = ti.last_pck;
	}
}

void Channel::set_last_pck(uint64_t guid, const std::string &pck)
{
	if (channels_.find(guid) != channels_.end())
	{
		TermInfo &ti = channels_[guid];
		ti.last_pck = pck;
		ti.pck_id++;
	}
}

}
