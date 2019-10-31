#ifndef __SPORT_OPERTION_H__
#define __SPORT_OPERTION_H__

#include "gameinc.h"

class SportOperation
{
public:
	static void refresh(dhc::player_t *player);

	static void get_rank(dhc::player_t *player, std::vector<int> &ranks);

	static dhc::sport_t * create_sport(dhc::player_t *player, uint64_t other_guid, std::string other_name, int type, int win, int rank);

	static int player_max_rank(dhc::player_t *player, int rank);

	static int get_rank_type(dhc::player_t *player, int rank);

	static int sport_can_get(dhc::player_t *player);
};

#endif
