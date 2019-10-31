#ifndef __HUODONG_POOL_H__
#define __HUODONG_POOL_H__

#include "gameinc.h"

namespace {
	class tmsg_activity_group;
	class tmsg_activity;
	class tmsg_activity_reward;
}

enum  HuodongError
{
	HUODONG_SUCESS				= 0,
	HUODONG_ERROR_INVALID_TIME	= -100,
	HUODONG_ERROR_INVALID_ARG	= -101,
	HUODONG_ERROR_REPEATE		= -102,
};

enum HuodongType
{
	HUODONG_CZFL_TYPE	= 3,	/// �ۼƳ�ֵ
	HUODONG_HYLX_TYPE	= 4,	/// ��Ծ�
	HUODONG_DBCZ_TYPE	= 5,	/// ���ʳ�ֵ
	HUODONG_DLSL_TYPE	= 6,	/// ��¼����
	HUODONG_ZKFS_TYPE	= 7,	/// �ۿ۷���
	HUODONG_DHDJ_TYPE	= 8,	/// ���߶һ�
	HUODONG_RQDL_TYPE   = 9,	/// ���ڵ�¼
	HUODONG_YKJJ_TYPE   = 10,   /// �¿�����
	HUODONG_ZC_TYPE		= 11,   ///ֱ��
	HUODONG_TBHD_TYPE   = 20,   /// ̽���
	HUODONG_CZFP_TYPE   = 21,   /// ��ֵ����
	HUODONG_SZZP_TYPE   = 22,   /// ʱװת��
	HUODONG_TKMY_TYPE	= 23,	/// ̫������
	HUODONG_JGMF_TYPE	= 24,	/// �Ź�ħ��
	

	HUODONG_JRHD_TYPE	= 100,	/// ���ջ

	HUODONG_XHQD_TYPE   = 200,   /// �Ǻ����
};

enum HuodongListViewType
{
	HUODONG_HUODONG_VIEW = 0,
	HUODONG_XINGHEQIDIAN_VIEW,
};

enum HuodongConditionType
{
	HUODONG_COND_NONE				= 0,
	HUODONG_COND_JEWEL				= 1,		/// ��ʯ
	HUODONG_COND_RMB				= 2,		/// RMB
	HUODONG_COND_DAY				= 3,		/// ��¼
	HUODONG_COND_ITEM				= 4,		/// ����
	HUODONG_COND_RIQI				= 5,		/// ����

	HUODONG_COND_NORMAL_COUNT			= 100,	/// ����ͨ�ش���
	HUODONG_COND_MW_COUNT,						/// ħ���ַ�����
	HUODONG_COND_JJC_COUNT,						/// ������ʤ������
	HUODONG_COND_ROB_COUNT,						/// �ᱦ����
	HUODONG_COND_FATE_COUNT,					/// ����ָ�����
	HUODONG_COND_TANBAO_COUNT,					/// ����̽������
	HUODONG_COND_JY_COUNT,						/// ��Ӣͨ�ش���
	HUODONG_COND_GUILD_COUNT,					/// ���Ÿ�����ս����
	HUODONG_COND_PVP_COUNT,						/// ���˴����ս����
	HUODONG_COND_BINGYUAN_REWARD_COUNT,			/// ��ս��ԭ��ý�������
	HUODONG_COND_ZHUANPAN_COUNT,				/// ʱװת�̻���
	HUODONG_COND_TANSUO_COUNT,					/// ̫�����λ���
	HUODONG_COND_JGMF_COUNT,					/// �Ź�ħ������
	HUODONG_COND_TKYS_LJ_COUNT,                 /// ̫���������طɴ�����
	HUODONG_COND_TKYS_HS_COUNT,                 /// ̫�����令����ҵ�Ŵ���
	HUODONG_COND_CSSP_COUNT,                    /// �ϳɳ�ɫ��Ʒ����
	HUODONG_COND_HSSP_COUNT,                    /// �ϳɺ�ɫ��Ʒ����
	HUODONG_COND_HBSD_COUNT,                    /// ����̵�ˢ�´���
	HUODONG_COND_FUBEN_COUNT,                   /// ��ͨ/��Ӣ�������ô���
};

typedef std::pair<int, int> HuodongArgResult;
enum HuodongArgType
{
	HUODONG_ARG_COUNT			= 1,
	HUODONG_ARG_COMPLETE		= 2,
};

class HuodongPool
{
public:
	int init();

	int fini();

	int update();

	int huodong_add(const rpcproto::tmsg_activity_group &msg);

	void huodong_list_view(dhc::player_t *player,
		protocol::game::smsg_huodong_view& msg, HuodongListViewType type) const;

	bool huodong_view(dhc::player_t* player,
		int id,
		HuodongType type,
		protocol::game::smsg_huodong_reward_view& smsg) const;

	bool huodong_view(dhc::player_t* player,
		HuodongType type,
		protocol::game::smsg_huodong_jiri_view& smsg) const;

	bool huodong_view_tanbao(dhc::player_t* player, 
		protocol::game::smsg_huodong_tanbao_view& smsg);

	bool huodong_view_fanpai(dhc::player_t *player,
		protocol::game::smsg_huodong_fanpai_view& smsg);

	bool huodong_view_zhuanpan(dhc::player_t *player,
		protocol::game::smsg_huodong_zhuanpan_view& smsg);

	bool huodong_view_tansuo(dhc::player_t *player,
		protocol::game::smsg_huodong_tansuo_view& smsg);

	bool huodong_view_mofang(dhc::player_t *player,
		protocol::game::smsg_huodong_mofang_view& smsg);

	bool huodong_view_yueka(dhc::player_t *player,
		protocol::game::smsg_huodong_yueka_view& smsg);

	const dhc::huodong_entry_t* get_huodong_entry(int huodong_id, int id, HuodongType type) const;
	dhc::huodong_entry_t* get_huodong_entry(int huodong_id, int id, HuodongType type);

	void set_huodong_arg(dhc::player_t* player,
		dhc::huodong_entry_t* entry,
		HuodongArgType arg,
		int value);

	void set_huodong_arg(uint64_t player_guid,
		dhc::huodong_entry_t* entry,
		HuodongArgType arg,
		int value);

	void get_huodong_arg(dhc::player_t* player,
		const dhc::huodong_entry_t* entry,
		HuodongArgResult& result) const;

	int get_huodong_level(dhc::player_t *player, const dhc::huodong_t* huodong) const;

	void set_huodong_level(dhc::player_t *player, dhc::huodong_t* huodong);

	dhc::huodong_player_t* get_huodong_player(dhc::player_t *player, HuodongType type, HuodongType subtype);
	dhc::huodong_player_t* get_huodong_player(dhc::player_t *player, dhc::huodong_t *huodong);
	void save_huodong_player(uint64_t guid);

	void huodong_login(dhc::player_t *player);

	void huodong_refresh(dhc::player_t *player);

	void huodong_week_refresh(dhc::player_t *player);

	void huodong_recharge(dhc::player_t *player, int vippt, int iosid, int type);

	void huodong_zhichong(dhc::player_t *player, int huodong_id, int entry_id, s_t_rewards& strewards);

	void huodong_modify(dhc::player_t *player, int id, int rmb);

	void huodong_active(dhc::player_t* player, HuodongConditionType cond, int val);

	void huodong_jiri(dhc::player_t *player, protocol::game::smsg_player_check& smsg);

	void huodong_xingheqingdian(dhc::player_t *player, protocol::game::smsg_player_check& smsg);

	bool has_jieri_huodong(dhc::player_t *player) const;

	bool has_jieri_huodong_flag() const { return has_jieri_flag_; }

	int has_jieri_point(dhc::player_t *player) const;

	int get_global_ts_id();

private:
	bool get_zkfs_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

	bool get_ljcz_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

	bool get_dbcz_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

	bool get_dlsl_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

	bool get_hyhd_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

	bool get_dhhd_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

	bool get_rqdl_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

	bool get_ykjj_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

	bool get_zchd_view(dhc::player_t* player, int id, HuodongType type, protocol::game::smsg_huodong_reward_view& smsg) const;

private:
	bool has_rqdl_point(dhc::player_t *player, dhc::huodong_t *huodong) const;

	bool has_dbcz_point(dhc::player_t *player, dhc::huodong_t *huodong) const;

	bool has_hyhd_point(dhc::player_t *player, dhc::huodong_t *huodong) const;

	bool has_dhhd_point(dhc::player_t *player, dhc::huodong_t *huodong) const;

private:
	void tanbao_rank_reward(dhc::huodong_t *huodong);

	void zhuanpan_rank_reward(dhc::huodong_t *huodong);

	void tansuo_rank_reward(dhc::huodong_t *huodong);

	void mofang_reward(dhc::huodong_t *huodong);

	void mofang_reward_callback(const std::string &data, uint64_t huodong_guid);

private:
	void huodong_list_load();
	void huodong_list_load_callback(Request *req, uint64_t huodong_list_guid);
	void huodong_entry_load(uint64_t huodong_guid, uint64_t huodong_entry_guid);
	void huodong_entry_load_callback(Request *req, uint64_t huodong_guid);
	void huodong_sub_load(uint64_t huodong_guid, uint64_t huodong_sub_guid);
	void huodong_sub_load_callback(Request *req, uint64_t huodong_guid);

	void add_query(uint64_t huodong_guid) { query_map_[huodong_guid] = 1; }
	void inc_query(uint64_t huodong_guid) { int &num = query_map_[huodong_guid]; num++; }
	void dec_query(uint64_t huodong_guid) { int &num = query_map_[huodong_guid]; num--; if (num <= 0) query_map_.erase(huodong_guid); }
	bool is_quering(uint64_t huodong_guid) { return query_map_.find(huodong_guid) != query_map_.end(); }

	void add_huodong(dhc::huodong_t *huodong, bool is_new);
	void remove_huodong(dhc::huodong_t *huodong);
	void save_huodong(dhc::huodong_t* huodong, bool release);
	void reset_huodong(dhc::huodong_t* huodong);
	const dhc::huodong_t* get_huodong(int id) const;
	dhc::huodong_t* get_huodong(int id);
	const dhc::huodong_t* get_huodong_by_type(HuodongType type, HuodongType subtype) const;
	dhc::huodong_t* get_huodong_by_type(HuodongType type, HuodongType subtype);
	bool is_cond(const dhc::huodong_entry_t *entry, HuodongConditionType type) const;
	bool is_cond_range(const dhc::huodong_entry_t *entry, HuodongConditionType min, HuodongConditionType max) const;

	HuodongError check_huodong(const rpcproto::tmsg_activity &msg) const;
	bool check_huodong_entry(const rpcproto::tmsg_activity_reward &msg) const;

	dhc::huodong_t* create_huodong(const rpcproto::tmsg_activity &msg, const rpcproto::tmsg_activity_group &gp);
	dhc::huodong_entry_t* create_huodong_entry(uint64_t huodong_guid, const rpcproto::tmsg_activity_reward &msg);

	void update_huodong(dhc::player_t* player,
		HuodongType type,
		HuodongArgType arg,
		HuodongConditionType cond,
		int value);
private:
	typedef std::map<int, uint64_t> HuodongMap;
	HuodongMap now_huodong_;
	HuodongMap delay_huodong_;
	bool has_jieri_;
	bool has_jieri_flag_;
	int jieri_count_;
	bool load_complete_;
	int has_xingheqingdian_;

	std::set<uint64_t> huodong_player_save_;

	std::map<uint64_t, int> query_map_;

	int global_ts_id_;

	int query_mofang_count_;
};

#define sHuodongPool (Singleton<HuodongPool>::instance())

#endif //__HUODONG_POOL_H__