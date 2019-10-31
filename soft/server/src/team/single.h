#ifndef __SINGLE_H__
#define __SINGLE_H__

#include "gameinc.h"

struct SingleMatch
{
	uint64_t guid;
	int64_t bf_max;
	int64_t bf_min;
	int64_t bf;
	int duanwei;
	bool match;
	int count;

	bool match_duanwei(const SingleMatch &rhs) const
	{
		return duanwei == rhs.duanwei;
	}

	bool match_last(const SingleMatch &rhs) const;
};

struct FightResult
{
	uint64_t id;
	uint64_t win_guid;
	uint64_t win_bf;
	uint64_t lose_guid;
	uint64_t lose_bf;
	bool win;

	bool operator == (const FightResult &rhs) const
	{
		return id == rhs.id;
	}
};

class SingleManager
{
public:
	SingleManager();
	~SingleManager();

	int update(const ACE_Time_Value & tv);

	void start_match(dhc::player_t *player);

	bool stop_match(dhc::player_t *player);

	bool get_fight_result(dhc::player_t *player, uint64_t id, int &point, int &xinpian, int &ciliao);

	int get_basic_point(int player_ds_duwanwei, bool isgot);


private:
	void fight(uint64_t player_guid, uint64_t target_guid, bool isclone);

private:
	uint64_t fight_id_;
	std::map<uint64_t, SingleMatch> matchs_;
	std::list<FightResult> fight_results_;
};

#define sSingleManager (Singleton<SingleManager>::instance())

#endif