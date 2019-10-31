#ifndef __BOSS_MANAGER_H__
#define __BOSS_MANAGER_H__

#include "gameinc.h"


struct boss_hit
{
	uint64_t guid;
	std::string name;
	int template_id;
	int level;
	int64_t hit;
	int num;
	int64_t top;
	int vip;
	int achieve;
	int chenchao;
	int nalflag;
};

struct boss_pre
{
	uint64_t guid;
	std::string name;
	int template_id;
	int level;
	int64_t hit;
	int vip;
	int achieve;
	int chenghao;
	int nalflag;

	boss_pre(){}

	boss_pre(uint64_t g,
		const std::string& n,
		int t,
		int l,
		int64_t h,
		int vi,
		int ach,
		int chengh,
		int nalflag)
		:guid(g),
		name(n),
		template_id(t),
		level(l),
		hit(h),
		vip(vi),
		achieve(ach),
		chenghao(chengh),
		nalflag(nalflag)
	{}

	bool operator > (const boss_pre &m) const
	{
		return hit > m.hit;
	}

	bool operator == (const boss_pre& rhs) const
	{
		return guid == rhs.guid;
	}
};

class BossManager
{
public:
	BossManager();

	~BossManager();

	int init();

	int fini();

	int update(const ACE_Time_Value &curr);

	void terminal_boss_look(const std::string &data, const std::string &name, int id);

	void terminal_boss_look_ex(const std::string &data, const std::string &name, int id);

	void terminal_boss_rank(const std::string &data, const std::string &name, int id);

	void terminal_boss_fight_end(const std::string &data, const std::string &name, int id);

	void terminal_boss_active_look(const std::string &data, const std::string &name, int id);

	void terminal_boss_active(const std::string &data, const std::string &name, int id);

	void terminal_boss_change_name(Packet* pck);

	void terminal_boss_saodang(const std::string &data, const std::string &name, int id);

protected:
	void boss_load();

	void boss_load_callback(Request *req, uint64_t boss_guid);

	void boss_save(bool release);

	void create_boss();

	void end_boss();

	void reward();

	void calrank(const boss_hit &hb);

	void add_rank(dhc::player_t *player, int64_t hit);

	int get_rank(dhc::player_t *player, int type);

	void set_active(dhc::player_t *player, int type, int64_t count);

	bool has_active(dhc::player_t *player);

	std::pair<int, int> get_medal_reward() const;

	double get_boss_hp(int boss_level) const;

	int get_boss_player_num() const;

private:
	int timer_;
	int boss_id_;
	bool changed_;
	std::map<uint64_t, boss_hit> boss_hits_;

	boss_pre find_;
	std::vector<boss_pre>::iterator find_ite_;
	std::vector<boss_pre> total_rank_;
	std::vector<boss_pre> top_rank_;
};

#endif
