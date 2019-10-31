#ifndef __CHANNEL_H__
#define __CHANNEL_H__

#include "game_interface.h"
#include <map>

namespace game {

class Channel : public mmg::Channel
{
public:
	Channel();

	~Channel();

	int init();

	int fini();

	virtual void add_channel(uint64_t guid, TermInfo ti, bool is_login);

	virtual TermInfo * get_channel(uint64_t guid);
	virtual int get_channel_lang(uint64_t guid);

	//virtual void update_channel(uint64_t guid, uint64_t gag_time);

	virtual void del_channel(uint64_t guid);

	virtual void refresh_offline_time(uint64_t guid);

	virtual int check_sig(uint64_t guid, const std::string &sig);

	virtual int get_channel_num();

	virtual int get_real_channel_num();

	virtual bool online(uint64_t guid);

	virtual int check_pck(uint64_t guid, int pck_id);

	virtual void last_pck(uint64_t guid, std::string &pck);

	virtual void set_last_pck(uint64_t guid, const std::string &pck);

private:
	std::map<uint64_t, TermInfo> channels_;
};

}

#endif
