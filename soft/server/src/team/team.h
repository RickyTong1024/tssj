#ifndef __TEAM_H__
#define __TEAM_H__

#include "gameinc.h"

struct PlayerMatch
{
	uint64_t player_guid;
	int time;
};

struct TeamPrepare
{
	uint64_t team_id;
	int time;
};

struct TeamMatch
{
	uint64_t team_id;
	int time;
};

struct TeamFightResult
{
	uint64_t id;
	protocol::team::team* team1;
	protocol::team::team* team2;

	bool operator == (const TeamFightResult &rhs) const
	{
		return id == rhs.id;
	}
};

enum TeamStat
{
	TS_CREATE,
	TS_PREPARE,
	TS_MATCH,
};

class TeamManager
{
public:
	typedef protocol::team::team Team;
	typedef protocol::team::team_player TeamPlayer;

public:
	TeamManager();

	~TeamManager();

	int init();

	int fini();

	int update(const ACE_Time_Value & tv);

	void create_team(dhc::player_t*player);
	int enter_team(uint64_t team_id, dhc::player_t *player, bool npc, bool invite);
	void leave_team(dhc::player_t *player);
	void kick_team(dhc::player_t *player, dhc::player_t *member);
	void move_team(dhc::player_t *player, dhc::player_t *member, int dest);
	void prepare_team(dhc::player_t *player);
	void open_team(dhc::player_t *player);
	void chat_team(dhc::player_t *player, const std::string& color, const std::string& text);
	void view_member(dhc::player_t *player, dhc::player_t *member);
	void match_team(dhc::player_t *player);
	void end_match_team(dhc::player_t *player);
	void remove_team(uint64_t team_id);
	void fight_team(dhc::player_t *player);
	void fight_end_team(dhc::player_t *player);
	void invite_dead(dhc::player_t *player);
	void change_reward_num(dhc::player_t *player);
	void press_team(dhc::player_t *player, int index);
	void leader_prepare(dhc::player_t *player);

	Team* get_team(uint64_t team_id);
	Team* get_match_team(dhc::player_t *player);

	uint64_t get_player_team(dhc::player_t *player) const;
	void remove_player_team(dhc::player_t *player);

	TeamPlayer* get_team_player(Team *team, uint64_t player_guid);
	int get_team_player_index(Team *team, uint64_t player_guid) const;
	int get_team_empty_num(Team *team) const;
	int get_team_num(Team *team) const;
	bool is_all_npc_team_num(Team *team) const;
	TeamPlayer* get_empty_team_player(Team *team);
	TeamPlayer* get_team_leader(Team *team);

	void add_invite_list(uint64_t invite, uint64_t whois);
	void social_invite(dhc::player_t *player, uint64_t invite_id);
	void remove_invite(dhc::player_t *player, uint64_t invite_id);
	void remove_invite(uint64_t player_guid, uint64_t invite_id);

	void add_prepare_team(Team *team);

	void get_invite_list(uint64_t player_guid, protocol::team::smsg_enter_world &smsg) const;

	bool get_bingyuan_result(dhc::player_t *player, uint64_t id, int num, int &point, int &bingjin);

private:
	int team_fight(Team *steam, Team *oteam);
	Team* get_dead_team(Team *team);
	void remove_dead_team(uint64_t team_id);

	int get_jiacheng(dhc::player_t *player, dhc::player_t *target) const;

	void update_team(Team *team);

	void update_team_match();
	void update_team_prepare();
	void update_player_match();
	void update_player_invite();

private:
	uint64_t team_id_;
	uint64_t invite_id_;
	uint64_t fight_id_;

	typedef std::map<uint64_t, protocol::team::team*> TeamMap;
	TeamMap teams_;

	typedef std::map<uint64_t, uint64_t> PlayerTeamID;
	PlayerTeamID player_team_id_;

	typedef std::map<uint64_t, PlayerMatch> PlayerMatchMap;
	PlayerMatchMap player_matchs_;

	typedef std::map<uint64_t, std::list<protocol::team::team_invite> > PlayerTeamInvite;
	PlayerTeamInvite invite_list_;

	typedef std::map<uint64_t, TeamPrepare> TeamPrepareMap;
	TeamPrepareMap team_prepares_;

	typedef std::map<uint64_t, TeamMatch>  TeamMatchMap;
	TeamMatchMap team_matchs_;

	std::map<uint64_t, Team*> dead_team_;
	typedef std::list<TeamFightResult> TeamFightResultList;
	TeamFightResultList	team_fight_result_;
};

#define sTeamManager (Singleton<TeamManager>::instance())

#endif