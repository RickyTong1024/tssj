#ifndef __PLAYER_MANAGER_H__
#define __PLAYER_MANAGER_H__

#include "gameinc.h"

class PlayerManager
{
public:
	PlayerManager();

	~PlayerManager();

	int init();

	int fini();

	int update(ACE_Time_Value tv);

	void terminal_client_login(const std::string &data, const std::string &name, int id);

	void heitao_login_callback(const std::string &data, TermInfo &ti, const std::string &name, int id);

	void acc_login(TermInfo &ti, const std::string &name, int id);

	int client_login_acc_callback(Request *req, TermInfo &ti, const std::string &name, int id);

	void terminal_player_name(const std::string &data, const std::string &name, int id);

	void terminal_player_change_name(const std::string &data, const std::string &name, int id);

	void terminal_player_change_nalflag(const std::string &data, const std::string &name, int id);

	void player_name_callback(NameStr *ns, uint64_t player_guid, const std::string &player_name, const std::string &name, int id);

	void player_change_name_callback(NameStr *ns, uint64_t player_guid, const std::string &player_name, const std::string &name, int id);

	void terminal_player_zsname(const std::string &data, const std::string &name, int id);

	void terminal_gm_command(const std::string &data, const std::string &name, int id);

	void terminal_client_reload(const std::string &data, const std::string &name, int id);

	void terminal_player_task(const std::string &data, const std::string &name, int id);

	void terminal_active_reward(const std::string &data, const std::string &name, int id);

	void terminal_active_score_reward(const std::string &data, const std::string &name, int id);

	void terminal_recharge(const std::string &data, const std::string &name, int id);

	void terminal_recharge_heitao(const std::string &data, const std::string &name, int id);

	void self_player_load_recharge(Packet *pck);

	int recharge_heitao_acc_callback(Request *req, int pid, const std::string &order, int count, const std::string &name, int id, int hudong_id, int entry_id);

	void recharge_heitao_acc_callback1(uint64_t player_guid, int pid, const std::string &order, int count, const std::string &name, int id, int hudong_id, int entry_id);

	int recharge_heitao_order_callback(Request *req, uint64_t player_guid, int pid, int count, const std::string &name, int id, int hudong_id, int entry_id);

	void terminal_recharge_check_ex(const std::string &data, const std::string &name, int id);

	void terminal_first_recharge(const std::string &data, const std::string &name, int id);

	void terminal_vip_reward(const std::string &data, const std::string &name, int id);

	void terminal_dj(const std::string &data, const std::string &name, int id);

	void terminal_djten(const std::string &data, const std::string &name, int id);

	void terminal_online_reward(const std::string &data, const std::string &name, int id);

	void terminal_daily_sign(const std::string &data, const std::string &name, int id);

	void terminal_daily_sign_reward(const std::string &data, const std::string &name, int id);

	void terminal_player_look(const std::string &data, const std::string &name, int id);

	void self_player_load_look(Packet *pck);

	void terminal_random_event_look(const std::string &data, const std::string &name, int id);

	void terminal_random_event_get(const std::string &data, const std::string &name, int id);

	void terminal_player_check(const std::string &data, const std::string &name, int id);

	void terminal_libao_exchange(const std::string &data, const std::string &name, int id);

	void libao_exchange_acc_callback(Request *req, const std::string &data, const std::string &code, const std::string &name, int id);

	void self_player_load_libao_exchange(Packet *pck);

	void libao(const std::string &data, uint64_t player_guid, const std::string &code, const std::string &name, int id);

	void libao1(const std::string &data, uint64_t player_guid, const std::string &code, const std::string &name, int id);

	void terminal_gonggao(const std::string &data, const std::string &name, int id);

	void terminal_player_template(const std::string &data, const std::string &name, int id);

	void terminal_kick(const std::string &data, const std::string &name, int id);

	void terminal_chenghao_on(const std::string &data, const std::string &name, int id);

	void terminal_server_stat(const std::string &data, const std::string &name, int id);

private:
	void gm_create_player(dhc::player_t *player, const std::string &serverid);

	int gm_create_player_callback(Request *req, const std::string &name, uint64_t guild);
private:
	int timer_;
	std::set<std::string> names_;
	bool pinbi_;
};

#endif
