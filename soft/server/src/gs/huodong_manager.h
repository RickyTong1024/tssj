#ifndef __HUODONG_MANAGER_H__
#define __HUODONG_MANAGER_H__

#include "gameinc.h"

class HuodongManager
{
public:
	HuodongManager();

	~HuodongManager();

	int init();

	int fini();

	int update(const ACE_Time_Value& tv);

	void terminal_huodong_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_jiri(const std::string &data, const std::string &name, int id);
	void terminal_huodong_add(const std::string &data, const std::string &name, int id);
	void terminal_huodong_xingheqidian(const std::string &data, const std::string &name, int id);
	void terminal_huodong_modify(const std::string &data, const std::string &name, int id);
	void terminal_huodong_modify_callback(Packet* pck);
	void terminal_huodong_tansuo_event_callback(Packet *pck);
public:
	/// ����ͬ��
	void terminal_huodong_pttq_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_pttq(const std::string &data, const std::string &name, int id);
	
	/// ����Ŀ��
	void terminal_huodong_kaifu_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_kaifu(const std::string &data, const std::string &name, int id);

	/// ��ֵ�ƻ�
	void terminal_huodong_czjh_buy(const std::string &data, const std::string &name, int id);
	void terminal_huodong_czjh_get(const std::string &data, const std::string &name, int id);
	void terminal_huodong_czjhrs(const std::string &data, const std::string &name, int id);

	/// vipÿ�����
	void terminal_huodong_vip_libao(const std::string &data, const std::string &name, int id);

	/// ÿ�����
	void terminal_huodong_week_libao(const std::string &data, const std::string &name, int id);

	/// �ۼƳ�ֵ
	void terminal_huodong_ljcz_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_ljcz(const std::string &data, const std::string &name, int id);

	/// ���ʳ�ֵ
	void terminal_huodong_dbcz_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_dbcz(const std::string &data, const std::string &name, int id);

	/// ��¼����
	void terminal_huodong_dlsl_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_dlsl(const std::string &data, const std::string &name, int id);

	/// �ۿ۷���
	void terminal_huodong_zkfs_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_zkfs(const std::string &data, const std::string &name, int id);

	/// ���߶һ�
	void terminal_huodong_djdh_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_djdh(const std::string &data, const std::string &name, int id);

	/// ��Ծ�
	void terminal_huodong_hyhd_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_hyhd(const std::string &data, const std::string &name, int id);

	/// ���ڵ�¼
	void terminal_huodong_rqdl_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_rqdl(const std::string &data, const std::string &name, int id);

	/// ����̽��
	void terminal_huodong_tanbao_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_tanbao_dice(const std::string &data, const std::string &name, int id);
	void terminal_huodong_tanbao_shop(const std::string &data, const std::string &name, int id);
	void terminal_huodong_tanbao_mubiao(const std::string &data, const std::string &name, int id);

	/// ��ֵ����
	void terminal_huodong_fanpai_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_fanpai(const std::string &data, const std::string &name, int id);
	void terminal_huodong_fanpai_cz(const std::string &data, const std::string &name, int id);

	/// ʱװת��
	void terminal_huodong_zhuanpan_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_zhuanpan(const std::string &data, const std::string &name, int id);

	/// ̫������
	void terminal_huodong_tansuo_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_tansuo(const std::string &data, const std::string &name, int id);
	void terminal_huodong_tansuo_event(const std::string &data, const std::string &name, int id);
	void terminal_huodong_tansuo_mubiao(const std::string &data, const std::string &name, int id);
	void terminal_huodong_tansuo_event_refresh(const std::string &data, const std::string &name, int id);
	void terminal_huodong_tansuo_event_del(const std::string &data, const std::string &name, int id);

	/// �Ź�ħ��
	void terminal_huodong_mofang_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_mofang_chou(const std::string &data, const std::string &name, int id);
	void terminal_huodong_mofang_refresh(const std::string &data, const std::string &name, int id);
	void terminal_huodong_mofang_reset(const std::string &data, const std::string &name, int id);
	void terminal_huodong_mofang(const std::string &data, const std::string &name, int id);
	void terminal_huodong_mofangall(const std::string &data, const std::string &name, int id);
	void terminal_huodong_mofang_mubiao(const std::string &data, const std::string &name, int id);

	///�¿�����
	void terminal_huodong_yueka_view(const std::string &data, const std::string &name, int id);
	void terminal_huodong_yueka_reward(const std::string &data, const std::string &name, int id);
	///ֱ��
	void terminal_huodong_zhichong_view(const std::string &data, const std::string &name, int id);
	///Ӣ�ۻع�
	void terminal_huodong_huigui_reward(const std::string &data, const std::string &name, int id);
private:
	void get_tanbao(int move, bool luck, int &gezi, protocol::game::smsg_tanbao_dice &smsg);

private:
	int timer_;
	int global_timer_;
	int huodong_timer_;
	uint64_t old_time_;
};

#endif
