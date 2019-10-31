#ifndef __GLOBAL_POOL_H__
#define __GLOBAL_POOL_H__

#include "gameinc.h"

class GlobalPool
{
public:
	GlobalPool();

	~GlobalPool();

	int init();

	int fini();

	int update(const ACE_Time_Value& tv);

public:
	/// 普天同庆
	void update_pttq(const std::string &name, int id, int vip);
	void update_random_pttq(int id);
	bool is_enable_pttq(int id) const;

	/// 开服限购
	int get_kaifu_xg_count(int id) const;
	void update_kaifu_xg_count(int id);

private:
	void load_global();

	void load_global_callback(Request *req, uint64_t global_guid);

	void save_global(bool release);
};

#define sGlobalPool (Singleton<GlobalPool>::instance())

#endif