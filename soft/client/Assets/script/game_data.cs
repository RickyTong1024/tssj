
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;

public enum e_open_level
{
	el_second_role = 2,
    el_guild_boss_kaiqi = 2,
	el_post = 3,
    el_guild_keji = 4,
	el_auto_battle = 5,
	el_2x = 7,
	el_third_role = 7,
	el_shop = 8,
	el_fourth_role = 9,
	el_mx = 10,
	el_ph = 10,
	el_jjc = 10,
	el_yuehui = 12,
	el_richang = 13,
	el_treasue_qh = 15,
    el_treasure_qu = 15,
	el_saodang = 15,
	el_jy = 18,
	el_tupo = 18,
	el_shengji_duixing = 18,
	el_chat = 20,
	el_friend = 20,
	el_3x = 20,
    el_saodang10 = 25,
	el_mijing = 22,
	el_fifth_role = 23,
	el_treasure = 25,
	el_transport_ship = 28,
	el_baoshi = 28,
    el_chunjian = 38,
	el_equip_jl = 30,
	el_baozang = 32,
	el_sixth_role = 33,
	el_skill = 35,
	el_mowang = 35,
	el_juntuan = 38,
	el_first_houyuan = 38,
	el_chenghao = 38,
	el_treasure_qiangduo5 = 38,
	el_treasure_jl = 40,
	el_second_houyuan = 43,
	el_hhb = 45,
	el_third_houyuan = 48,
	el_mj_yj = 52,
	el_fourth_houyuan = 53,
	el_skill_zs = 55,//专属技能
	el_gaizao = 50,
    el_memory = 50,
	el_fifth_houyuan = 58,
	el_treasure_zhuzao = 60,
	el_treasure_shenglian = 60,
	el_equip_shenglian = 60,
    el_pvp = 60,
	el_sixth_houyuan = 58,
	el_sport_zhan5ci = 65,
    el_yijian_boss = 65,
	el_baowu_yj_duobao = 70,
	el_shengpin = 70,
	el_jiyingaizao = 70,
	el_bingyuan = 65,
    el_master = 70,
	el_pet = 70,//宠物
	el_guanghuan = 75,
	el_pet_guard = 75,//宠物护卫
	el_houyuan_ex = 75,//后援助威鼓舞
	el_shengxing = 80,
	el_baowu_sx = 80,
	el_mijing_sanxin_sao = 85,
	el_yj_treasure_sx = 85,
	el_yj_treasure_qh = 85,
	el_yj_equip_jl = 85,
	el_huiyi_shengxing =110, //回忆升星
}

public enum e_huodong_item_id
{
	ei_huodong_item1 = 80020001,
	ei_huodong_item2 = 80020002,
}

public enum e_open_see
{
	es_equip_jinlian = 20,
	es_equip_gaizao = 30,
	es_sport_zhan5ci = 50,
	es_equip_shengxing = 50,
	es_treasure_shenglian = 50,
	es_equip_shenglian = 50,
	es_baowu_yj_duobao = 60,
	es_treasure_zhuzao = 60,
	es_guanghuan = 65,
	es_jiyingaizao_sp1 = 70,
	es_baowu_sx = 70,
	es_mijing_sanxin_sao = 75,
	es_jiyingaizao_sp2 = 80,
	es_jiyingaizao_sp3 = 90,
	es_jiyingaizao_sp4 = 100,
}

public enum e_open_vip
{
	ev_jiasu = 1,
	ev_saodang10 = 2,
	ev_treasure_qiangduo5 = 3,
	ev_mj_yj = 4,
	ev_lianjin10 = 4,
	ev_sport_zhan5ci = 5,
	ev_baowu_yj_duobao = 6,
	ev_mijing_sanxin_sao = 7,
}

public enum e_language
{
	Simplified = 0,
	Tranditional = 1,
	English = 2,
}

public class save_data
{
    public string m_token = "null";
    public string m_user = "null";
    public string m_pass = "null";
	public int m_channel = -1;
	public int high_ver = 1;
	public int low_ver = 0;
	public int m_sound = 1;
    public int m_music = 1;
    public int m_guanghuan = 1;
    public int m_speed = 1;
	public int m_auto_skill = 0;
	public int m_danmu = 1;
	public int m_id = -1;
	public int m_is_register = 0;
}

public class server_list
{
	public string m_id;
	public string m_http;
	public string m_tcp;
	public int m_port;
	public string m_name;
	public int m_state;
}

public class post_zd
{
	public string title;
	public string text;
}

public enum e_skill_type
{
	skill_type_attack = 0,

	skill_type_active,
	skill_type_zhuanshu,
	
	skill_type_jlevel_1,
	skill_type_jlevel_2,
	skill_type_jlevel_3,
	skill_type_jlevel_4,
	skill_type_jlevel_5,

	skill_type_glevel_1,
	skill_type_glevel_2,
	skill_type_glevel_3,
	skill_type_glevel_4,
	skill_type_glevel_5,
	skill_type_glevel_6,
	skill_type_glevel_7,
	skill_type_glevel_8,
	skill_type_glevel_9,
	skill_type_glevel_10,
	skill_type_glevel_11,
	skill_type_glevel_12,
    skill_type_glevel_13,
    skill_type_glevel_14,
    skill_type_glevel_15,

	skill_end,
}

public enum e_skill_type_ex
{
	skill_type_active = 0,
	skill_type_jlevel_1 = 1,
	skill_type_jlevel_2 = 2,
	skill_type_jlevel_3 = 3,
	skill_type_jlevel_4 = 4,
	skill_type_jlevel_5 = 5,
	skill_end,
}

public class s_t_reward
{
	public int type;
	public int value1;
	public int value2;
	public int value3;
}

public class s_t_skill
{
	public int id;
	public string name;
	public string des;
	public string icon;
	public string action;
	public int type;	
	public int attack_type;
	public int target_type;
	
	public int range; //7
	
	public float attack_pe;
	public float attack_pe_add;
	
	public List<int> buffer_types = new List<int>();
	public List<int> buffer_target_types = new List<int>();
	public List<int> buffer_rounds = new List<int>();
	public List<int> buffer_attack_types = new List<int>();
	public List<float> buffer_attack_pes = new List<float>();
	public List<float> buffer_attack_pe_adds = new List<float>();
	public List<int> buffer_modify_att_types = new List<int>();
	public List<float> buffer_modify_att_vals = new List<float>();
	public List<float> buffer_modify_att_val_adds = new List<float>();

	public int passive_type;
	public int passive_modify_att_type;
	public float passive_modify_att_val;
	public float passive_modify_att_val_add;
	
	public int base_ex_type;

	public int base_ex_type_val_0;
	public float base_ex_type_val_1;
	public float base_ex_type_val_2;

	public float add_ex_type_val_1;
	public float add_ex_type_val_2;

}

public class s_t_yueka
{
	public int day;  //月卡基金  奖励日
	public int type1; //奖励类型 98档

	public int value_1_1; //奖励值
	public int value_1_2;
	public int value_1_3;

	public int type2; //奖励类型 328档
	
	public int value_2_1; //奖励值
	public int value_2_2;
	public int value_2_3;
	
}

public class s_t_monster
{
	public int id;
	public int class_id;
	public int level;						//等级
	public int jlevel;				//阶
	public int glevel;				//突破
	public int pinzhi_skill;
	public int is_boss;
	public int skill_level;
}

public class s_t_role_gaizao
{
    public int id;
    public int type;
    public int jewel;
    public int jjc_point;
}


public class s_t_class
{
	public int id;
	public int job;
	public int gender;
	public string name;
	public string show;
	public string dress;
	public string icon;
	public string sound;
	public string card;

	public int color;
	public int pz;
	public int max_glevel;
	public int ptattack;
	public List<int> cs = new List<int>();
	public List<float> cscz = new List<float>();
	public List<float> cz = new List<float>();
	public List<float> czcz = new List<float>();	
	public List<int> skills = new List<int>();
	public int exp;
    public int role_power;
	public List<int> jbs = new List<int>();
	public List<int> jbexs = new List<int>();
    public int ycang;
    public int tupo10;
    public int tupo12;
    public int tupo15;
}
public class s_t_resource
{
    public int type;
    public string name;
    public int color;
    public string icon;
    public string smallicon;
    public string desc;
    public int ch;
    public string namecolor;
}

public class s_t_mission_item
{
	public s_t_reward reward = new s_t_reward();
	public int rate;
}

public class s_t_mission
{
	public int id;
	public string name;
	public string des;
	public int type;
	public int map_id;
	public string map_name;
	public int jytype;
	public int lock_id;
	public int index_id;
	public int jylock_id;
	public int jyindex_id;
	public int tili;
	public int day_num;
	public int friend_id;
	public int friend_guan;
	public List<int> monsters = new List<int>();
	public List<s_t_mission_item> items = new List<s_t_mission_item>();
}

public class s_t_mission_first_reward
{
	public int id;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}


public class s_t_attr
{
	public int attr;
	public int value;
}

public class s_t_random_attr
{
	public int attr;
	public float value1;
	public float value2;
}

public class s_t_equip
{
	public int id;
	public string name;
	public int font_color;
	public int type;
	public string icon;
	public int slot_num;
	public int sell;
	public int sell_jh;
	public s_t_attr eattr = new s_t_attr();
	public List<s_t_attr> ejlattr = new List<s_t_attr>();
	public List<s_t_random_attr> eeattr = new List<s_t_random_attr>();
}

public class s_t_equip_sx
{
	public int level;
	public int color;
	public int price;
	public float enhance_rate;
	public int sp_num;
	public int gold;
}

public class s_t_equip_jl
{
	public int level;
	public List<int> stones = new List<int>();
	public List<int> golds = new List<int>();
}

public class s_t_item
{
	public int id;
	public string name;
	public int font_color;
	public int type;
	public string icon;
	public string desc;
	public int need_level;
	public int def_1;
	public int def_2;
	public int def_3;
	public int def_4;
	public int gold;
	public int use;
	public int tuse;
	public int jewel;
	public string out_put;
}
public class s_t_itemstore
{
    public int id;
    public string name;
    public int type;
    public List<s_t_reward> rewards = new List<s_t_reward>();

}
public class s_t_gaizao
{
	public int color;
	public int item_id;
	public int m_0;
	public int m_1;
	public int m_2;
	public int m_3;
	public int m_4;
	public int max_count;
	public int rate_1;
	public int rate_2;
	public int rate_3;
	public int rate_4;
	public int rate_5;
}

public class s_t_role_shop
{
	public int id;
	public string name;
	public int grid;
	public int level;
	public int sell_type;
	public int sell_value_0;
	public int sell_value_1;
	public int sell_value_2;
	public int weight;
	public int money_type;
	public int price;
	public int rec;
}

public class s_t_shop_xg
{
	public int id;
    public int shop_type;
    public int jewel;
	public int type;
	public int vlaue1;
	public int vlaue2;
	public int vlaue3;
	public string name;
	public int level;
	public int price_type;
	public int price;
	public int xg_type;
	public int xg_num;
	public List<int> vip_type = new List<int>();
}

public class s_t_target
{
	public int id;
	public string desc;
	public string icon;
	public int type;
	public int pid;
	public int tjtype;
	public int tjnum;
	public int tjdef1;
	public int tjdef2;
	public s_t_reward reward = new s_t_reward();
}
public class s_t_guild_horreward
{
    public  int id;
    public int value;
    public s_t_reward reward = new s_t_reward();
    public int speed_add;
    public int value2_add;
}
public class s_t_unlock
{
    public int level;
    public int dangwei;
    public string gos_name;
    public string bottom_name;

   
}
public class s_t_exp
{
	public int level;
	public int exp;
	public int tili;
	public int regain_tili;
	public int role_exp;
    public int role_zi_exp;
    public int role_jin_exp;
	public int role_hong_exp;
	public int yuanli;
	public int zhanhun;
	public int dxqhzz;
    public int suc_bingjin;
    public int fail_bingjin;
	public int pet_exp;
	public int pet_zi_exp;
	public int pet_jin_exp;
	public int pet_hong_exp;
	public string desc;
	public string icon;
	public string desc2;
	public string icon2;
	public string tg;
	public string tg2;
	public int type;
}

public class s_t_active
{
	public int id;
	public string name;
	public string desc;
	public string icon;
	public int level;
	public int num;
	public s_t_reward reward = new s_t_reward();
	public int score;
}
public class s_t_map_star
{
	public int star_num;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_map
{
	public int id;
	public string name;
	public string boss_name;
	public string res;
	public int pid;
	public int jypid;
	public int level;
	public int role_id;
	public string image_text;
	public List<s_t_map_star> stars = new List<s_t_map_star>();
}
public class s_t_qiyu_tiaozhan
{
    public int id;
    public int zhuangzhi;
    public int tili;
    public string desc;

}
public class s_t_ji_ban
{
	public int id;
	public string name;
	public int type;
	public List<int> tids = new List<int>();
	public int attr1;
	public int value1;
	public int attr2;
	public int value2;
}

public class s_t_ji_banex
{
	public int id;
	public string name;
	public List<int> tids = new List<int>();
	public int attr;
	public int value;
}

public class s_t_vip
{
	public int level;
	public int recharge;
	public int yjewel;
	public int jewel;
	public string desc;
	public string desc1;
	public List<s_t_reward> rewards = new List<s_t_reward>();
	public string half;
	public int add_tili;
	public int dj_num;
	public int jy_buy_num;
	public int ttt_cz_num;
    public int guild_attack_num;
	public int refresh_shop_num;
	public int hunter_assembly;
    public int huiyi_zhanbu_num;
    public int master_buy_num;
    public int guildpvpbuy_num;
}

public class s_t_recharge
{
	public int id;
	public string name;
	public string desc;
	public string icon;
	public int type;
	public int pid;
	public int vippt;
	public int jewel;
	public int jjc_num;
    public string rmb;
	public int ios_id;
   
}

public class s_t_price
{
	public int num;
	public int dj;
	public int kc;
	public int jy;
	public int ttt_cz;
	public int hbb_refresh;
    public int guild_attack_buy;
	public int yuanli_potion;
	public int tili_potion;
	public int energy_potion;
	public int jinxiang_equip;
	public int jinxiang_treasure;
	public int mowang_invitation;
	public int change_name;
	public int hunter_assembly;
    public int bingyuan_reward;
	public int tanbao_price;
	public int luck_zhuanpan;
	public int haohua_zhuanpan;
	public int manyou;
    public int master;
    public int mofang;
    public int guildpvpbuy;
}

public class s_t_sport_rank
{
	public int rank_1;
	public int rank_2;
	public int zs_point;
	public int jj_point;
	public float zs_pm;
}

public class s_t_online_reward
{
	public string name;
	public int time;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_huodong
{
	public int id;
	public string name;
	public int type;
	public string sdes;
	public List<int> times = new List<int>();

	public string des;
	public int icon;
}

public class s_t_huodong_sub
{
	public int id;
	public int pid;
	public string name;
	public int level;
	public int vip;
	public int mission_id;
	public List<int> values = new List<int>();
}

public class s_t_daily_sign
{
	public int index;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int vip;
    public int type1;
    public int value11;
    public int value21;
    public int value31;
    public int vip1;
}

public class s_t_ttt_guai
{
	public int id1;
	public int id2;
	public int id3;
	public int id4;
	public int id5;
	public int gold;
	public int hj;
	public string bf;
}

public class s_t_ttt_shop
{
	public int id;
	public string name;
	public int fen_ye;
	public int level;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int price;
}

public class s_t_ttt_mubiao
{
	public int id;
	public int star;
	public string name;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int price;
	public int discount;
}

public class s_t_ttt
{
	public int index;
	public List<s_t_ttt_guai> guais = new List<s_t_ttt_guai>();
	public int tjtype;
	public int tjvalue;
	public string tj;
	public string desc;
}
public class s_t_mision_reward
{
    public int type;
    public int value1;
    public List<int> value2s = new List<int>();
}
public class s_t_guild_mission
{
    public int index;
    public string name;
    public List<int> ids = new List<int>();
    public int basicCon;
    public int maxCon;
    public int jishaCon;
    public int exp;
    public List<s_t_mision_reward> slrewarxds = new List<s_t_mision_reward>();
    public List<s_t_reward> firstRewards = new List<s_t_reward>();
    public int level;
}
public class s_t_ttt_reward
{
	public int index;
	public List< List<s_t_reward> > rewardss = new List<List<s_t_reward>>();
}
//冰原类=================================
public class s_t_bingyuan_chenhao
{
    public int id;
    public string name;
    public int rank;
    public int jifen;
    public int chenhaoid;
}
public class s_t_chenghao
{
    public int id;
    public int type;
    public string name;
    public string color;
	public string color_effect;
	public string icon1;
	public string icon2;
	public string condition;
	public string desc;
	public List<s_t_attr> attr = new List<s_t_attr>();
	public int is_show;
	public int time;
}
public class s_t_bingyuan_reward
{
    public int rank1;
    public int rank2;
    public List<s_t_reward> rewards = new List<s_t_reward>();
}
public class s_t_bingyuan_shop
{
    public int id;
    public string name;
    public int type;
    public int value1;
    public int value2;
    public int value3;
    public int binjin;
    public int buy_count;
    public int level;
}
public class s_t_bingyuan_mubiao
{
    public int id;
    public int jifen;
    public string name;
    public int type;
    public int value1;
    public int value2;
    public int value3;
    public int price;
    public int discount;
}
//回忆类==================================
public class s_t_huiyi
{
    public  int id;
    public  string name;
    public int level;
    public int pre_num;
    public string icon;
    public string bg;

}
public class s_t_huiyi_chengjiu
{
    public  int id;
    public int mem_value;
    public int attr;
    public int value;
}
public class s_t_huiyi_luckshop
{
    public int id;
    public string name;
    public int page;
    public int day_num;
    public s_t_reward reward = new s_t_reward();
    public int luck_point;
}
public class s_t_huiyi_lunpan
{
    public int id;
    public string name;
    public int type;
    public int weight;
    public int luck_point;
}
public class s_t_huiyi_destiny
{
    public int id;
    public string name;
    public int weight;
    public int huiyi_point;
}
public class s_t_huiyi_shop
{
    public int id;
    public string name;
    public int gezi;
    public s_t_reward reward = new s_t_reward();
    public int weight;
    public int huobi_type;
    public int huobi_value;
}
public class s_t_huiyi_sub
{
    public int id;
    public string name;
    public int page;
    public string dialog;
    public List<int> huiyis = new List<int>();
    public int huiyi_value;
    public List<int> attrs = new List<int>();
    public List<float> values = new List<float>();
	public List<float> values2 = new List<float>();
}
public class s_t_ttt_value
{
	public int id;
	public int type;
	public int xh;
	public int sxtype;
	public int sxvalue;
}

public class s_t_tupo
{
	public int level;
	public int role_level;
	public List<int> sps = new List<int>();
	public int purple_sp;
	public int gold_sp;
	public int cl_gold;
}

public class s_t_jinjie
{
	public int id;
	public string name;
	public int level;
	public int clty;
	public int clty_num;
	public int clfy;
	public int clfy_num;
	public int clfy_num1;
	public int clgj;
	public int clgj_num;
	public int clgj_num1;
	public int clmf;
	public int clmf_num;
	public int clmf_num1;
	public float sxper;
	public int gold;
	public int point;
	public string icon;
}
public class s_t_skillup
{
	public int level;
    public int skillstone;
	public int gold;
}
public class s_t_guild
{
	public int level;
	public int exp;
	public int number_count;
    public List<int> reward_nums = new List<int>();
}
public class s_t_guildfight_reward
{
    public int rank1;
    public int rank2;
    public int type;
    public List<s_t_reward> rewards = new List<s_t_reward>();
}
public class s_t_guildfight
{
    public int id;
    public string name;
    public int color;
    public int defendrolenum;
    public int chengfangvalue;
    public int exp;
    public int suc_chengfangvalue;
    public int fail_chengfangvalue;
    public int suc_con;
    public int fail_con;
    public int zhanjizhanli;
    public int defendnum;
}
public class s_t_guildfight_target
{
    public int id;
    public string name;
    public int type;
    public int num;
    public s_t_reward reward = new s_t_reward();
}
public class s_t_guild_icon
{
	public int id;
	public string icon;
}

public class s_t_guild_shop
{
	public int id;
	public	string name;
	public s_t_reward reward = new s_t_reward();
	public int gx;
	public int hb_power;
	public int num;
	public int level;
}
public class s_t_guild_mubiao
{
	public int id;
	public int level;
	public string name;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int price;
	public int discount;
}
public class s_t_guild_shop_ex
{
    public int id;
    public string name;
    public int type;
    public int value1;
    public int value2;
    public int value3;
    public int gongxian;
    public int jewel;
    public int level;
}
public class s_t_master_duanwei
{
    public int id;
    public string duanwei;
    public string icon;
    public string staricon;
    public int starcount;
    public string kuang;
    public string topcolor;
    public string bottomcolor;
    public int need_jifen;
    public int need_rank;
    public int attr1;
    public double value1;
    public int attr2;
    public double value2;
}
public class s_t_master_target
{
    public int id;
    public string name;
    public int count;
    public int type;
    public int value1;
    public int value2;
    public int value3;
}
public class s_t_master_reward
{
    public int rank1;
    public int rank2;
    public List<s_t_reward> rewards = new List<s_t_reward>();
}
public class s_t_guild_sign
{
	public int id;
	public string name;
	public int coin;
	public int zuanshi;
    public int exp;
	public int gongxian;
	public int hor;
}

public class s_t_active_reward
{
	public int id;
	public int score;
	public List<s_t_reward> reward = new List<s_t_reward>();
}
public class s_t_dress
{
	public int id;
	public string name;
	public int type;
    public int color;
	public string icon;
	public string res;
	public List<s_t_attr> attrs = new List<s_t_attr>();
	public string action1;
	public string action2;

	public string get_des()
	{
        string text = game_data._instance.get_t_language ("game_data.cs_1079_16");//助手时装，可以给所有伙伴增加属性
		for(int i= 0; i < attrs.Count;++i)
		{
			if(attrs[i].attr > 0)
			{
				text += "\n" +game_data._instance.get_value_string (attrs[i].attr, attrs[i].value,1);
			}
		}
		return text;
	}
}

public class s_t_role_dress
{
	public int id;
	public string name;
	public int role;
	public string icon;
	public string icon1;
	public string res;
	public int hq_condition;
	public int hq_Level;
	public string mai_desc;
}

public class s_t_dress_target
{
	public int id;
	public string name;
	public int type;
	public string desc;
	public List<int> defs = new List<int>();
	public int attr1;
	public int value1;
	public int attr2;
	public int value2;
	public int attr3;
	public int value3;
	public int attr4;
	public int value4;
}

public class s_t_xjbz
{
	public int site;
	public int type;
	public string name;
	public int db;
	public int def1;
	public int def2;
}

public class s_t_xjbz_mission
{
	public int level1;
	public int level2;
	public List<int> missions = new List<int>();
}

public class s_t_value
{
	public int id;
	public string name;
	public int att;
	public string des;
	public float force;
}

public class s_t_danmu
{
	public int mission;
	public int type;
	public string text;
	public string biaoqing;
	public float start_time;
	public int x;
	public int y;
	public float speed;
	public float time;
	public int rand;
}
public class s_t_guild_keji
{
    public int id;
    public string name;
    public string icon;
    public int guildlevel;
    public int level;
    public int sx;
    public int sx_value;
    public int exp;
    public int expadd;
    public int con;
    public int conadd;
}
public class s_t_huodong_pttq_sub
{
	public int vip;
	public s_t_reward reward = new s_t_reward ();
}

public class s_t_huodong_pttq
{
	public int id;
	public int vip;
	public List<s_t_huodong_pttq_sub> sub = new List<s_t_huodong_pttq_sub>();
}

public class s_t_kaifu
{
	public int tian;
	public int site;
	public string name;
	public List<int> ints = new List<int>();
}

public class s_t_kaifu_mubiao
{
	public int id;
	public string desc;
	public int type;
	public int def1;
	public int def2;
	public int def3;
	public int def4;
	public int ck;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_jc_huodong
{
	public int id;
	public string name;
	public string name_color;
	public string name_mb;
	public string text;
	public string text_color;
	public string text_mb;
	public string time_color;
	public string time_mb;
	public string res;
	public string message;
}

public class s_t_boss_reward
{
	public int id1;
	public int id2;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}
public class s_t_pvp_reward
{
    public int id1;
    public int id2;
    public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_huodong_czjh
{
	public int id;
	public int level;
	public int jewel;
}
public class s_t_huodong_czjhrs
{
    public int index;
    public int buy_count;
    public int type;
    public int value1;
    public int value2;
    public int value3;
}
public class s_t_mofang
{
    public int id;
    public int leixing;
    public int type;
    public int value1;
    public int value2;
    public int value3;
    public int rate;
    
}
public class s_t_mofang_shop
{
    public int id;
    public int type;
    public int value1;
    public int value2;
    public int value3;
    public int price;
    public int buycount;

}
public class s_t_mofang_reward
{
    public int id;
    public string name;
    public int need_jifen;
    public int type;
    public int value1;
    public int value2;
    public int value3;
}
public class s_t_equip_tz
{
	public int id;
	public string name;
	public List<int> equip_ids = new List<int>();
	public int attr1;
	public int value1;
	public int attr2;
	public int value2;
	public int attr3;
	public int value3;
	public int attr4;
	public int value4;

	public bool has_equip_id(int eid)
	{
		for (int i = 0; i < equip_ids.Count; ++i)
		{
			if (equip_ids[i] == eid)
			{
				return true;
			}
		}
		return false;
	}
}

public class s_t_baowu
{
	public int id;
	public string name;
	public int font_color;
	public int type;
	public string icon;
	public int exp;
	public int sell;
	public int attr1;
	public float value1;
	public int attr2;
	public float value2;
	public int jl_type_0;
	public float jl_value_0;
	public int jl_type_1;
	public float jl_value_1;
	public int jl_type_2;
	public float jl_value_2;
	public int count;
	public List<int> fragments = new List<int>();
}

public class s_t_baowu_jl
{
	public int level;
	public int stone;
	public int cost;
	public int num;
}

public class s_t_sport_card
{
	public int id;
	public int level1;
	public int level2;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public string baodi;
	public int weight;

}
public class s_t_yb
{
	public int type;
	public string name;
	public int time;
	public int rate; 
	public int yuanli;
	public int per;
	public int min_per;	
}

public class s_t_yb_gw
{
	public int index;
	public float gj;
	public int jewel;
	public string desc;
}

public class s_t_ore
{
	public int index;
	public int monster_id;
	public int level;
	public int tili;
	public int bd_gold;
	public int hx_gold;
	public int jl_js;
	public int zjs;
	public int szjs;
	public string nd;
	public string ndsm;
	public string ndbs;
}

public class s_t_sport_shop
{
	public int id;
	public string name;
	public int level;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int price;
	public int hb_power;
}

public class s_t_sport_mubiao
{
	public int id;
	public int rank;
	public string name;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int price;
	public int discount;
}

public class s_t_xinqing_event
{
	public int id;
	public string name;
	public string scene;
	public string start_scene;
	public int role_id;
	public int rate;
	public int type;
	public string select1;
	public string end_scene1;
	public string result1;
	public string select2;
	public string end_scene2;
	public string result2;
	public string select3;
	public string end_scene3;
	public string result3;
}

public class s_t_xinqing_random
{
	public int id;
	public string scene;
	public string select;
	public string good_result;
	public string good_result_change;
	public string bad_result;
	public string bad_result_change;
}

public class s_t_xinqing
{
	public int level;
	public string xinqing;
	public string icon;
	public int sx_per;
}

public class s_t_boss_active
{
	public int id;
	public int task_type;
	public string desc;
	public int count;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int ex_add;
}
public class s_t_hongbao_target
{
    public int id;
    public string name;
    public string desc;
    public string icon;
    public int type;
    public int tiaojian;
    public List<s_t_reward> rewrds = new List<s_t_reward>();
}
public class s_t_pvp_active
{
    public int id;
    public string name;
    public int neednum;
    public int lieb;
}

public class s_t_boss_shop
{
	public int id;
	public string name;
	public int level;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int price;
	public int hb_power;
	public int discount;
}
public class s_t_pvp_shop
{
    public int id;
    public string name;
    public int  type;
    public int value1;
    public int value2;
    public int value3;
    public int liebi;
    public int redrolepower;
    public int redequippower;
}

public class s_t_boss_dw
{
	public int level1;
	public int level2;
	public int dw;
	public int base_hurt;
}

public class s_t_ttt_mibao
{
	public int id;
	public int star;
	public string name;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int now_price;
	public int old_price;
}

public class s_t_gongzhen
{
	public int gongzhen_level;
	public int condition;
	public int task_type;
	public List<int> attrs = new List<int>();
	public List<float> value1 = new List<float>();
}

public class s_t_dress_unlock
{
	public int id;
	public int sz_id;
	public int tz_num;
}

public class s_t_equip_skill
{
	public int id;
	public string name;
	public int part;
	public int jinglian;
	public int type;
	public int def1;
	public int def2;
	public string desc;
}

public class s_t_vip_libao
{
	public int id;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_week_libao
{
	public int id;
	public string name;
	public int level1;
	public int level2;
	public int jewel;
	public int num;
	public float discount;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_guild_shop_xs
{
	public int id;
	public string name;
	public int grid;
	public int level;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int jewel;
	public int num;
	public int discount;
}

public class s_t_itemhecheng
{
	public int id;
	public int fenye;
	public int type;
	public int item_id;
	public int item_num;
	public List<int> cl_type = new List<int>();
	public List<int> cl_id = new List<int>();
	public List<int> cl_num = new List<int>();
}

public class s_t_role_shengpin
{
	public int pinzhi;
	public string name;
	public int next_pinzhi;
	public int level;
	public List<float> cs = new List<float>();
	public List<float> cz = new List<float>();
	public int color;
	public int zdjnjc;
	public int bdjnjc;
	public int shengpinshi;
	public int zhanhun;
	public int hongsehuobanzhili;
	public int suipian;
	public int gold;
}

public class s_t_duixng
{
	public int id;
	public string name;
	public int type;
	public int level;
	public List<int> zhenwei = new List<int>();
	public int q_attr;
	public float q_value;
	public float q_cz;
	public int q_attr1;
	public float q_value1;
	public float q_cz1;
	public int z_attr;
	public float z_value;
	public float z_cz;
	public int z_attr1;
	public float z_value1;
	public float z_cz1;
	public int h_attr;
	public float h_value;
	public float h_cz;
	public int h_attr1;
	public float h_value1;
	public float h_cz1;
}

public class s_t_duixng_up
{
	public int level1;
	public int level2;
	public int cl;
	public int gold;
}

public class s_t_duixng_skill
{
	public int id;
	public string name;
	public string desc;
	public int level;
	public List<s_t_attr> attrs = new List<s_t_attr>();
}

public class s_t_tanbao_event
{
	public int site;
	public int type;
	public int shop_type;
	public int rtype;
	public int rvalue1;
	public int rvalue2;
	public int rvalue3;
	public List<string> juqings = new List<string>();
	public List<s_t_reward> rewards = new List<s_t_reward>();
	public string name;
}

public class s_t_tanbao_mubiao
{
	public int id;
	public int task_num;
	public string name;
	public int type;
	public int value1;
	public int value2;
	public int value3;
}

public class s_t_tanbao_shop
{
	public int id;
	public string name;
	public int shop_type;
	public int rtype;
	public int rvalue1;
	public int rvalue2;
	public int rvalue3;
	public int price;
	public int discount;
	public int score;
	public int buy_num;
}

public class s_t_tanbao_reward
{
	public int rank1;
	public int rank2;
	public int type;
	public int rtype;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_guanghuan
{
	public int id;
	public string name;
	public int color;
	public string icon;
	public string effect;
	public int attr1;
	public int value1;
	public int attr2;
	public int value2;
}

public class s_t_guanghuan_enhance
{
	public int level;
	public List<int> golds = new List<int>();
	public List<int> yuansus = new List<int>();
}

public class s_t_guanghuan_skill
{
	public int id;
	public string name;
	public int wing_id;
	public int enhance;
	public int type;
	public int def1;
	public int def2;
	public string desc;
}

public class s_t_guanghuan_target
{
	public int id;
	public string name;
	public List<int> ids = new List<int>();
	public List<s_t_attr> attrs = new List<s_t_attr>();
}

public class s_t_baowu_sx
{
	public int level;
	public int pz;
	public int gold;
	public int jewel;
	public int process;
	public int rate;
	public int value1;
	public int value2;
	public int valuemax;
}

public class s_t_role_dresstarget
{
	public int id;
	public string name;
	public string desc;
	public List<int> ids = new List<int>();
	public List<s_t_attr> attrs = new List<s_t_attr>();

}

public class s_t_chongzhifanpai
{
	public int id;
	public int type;
	public int jewel;
	public int rate;
}

public class s_t_chongzhifanpai_shop
{
	public int id;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public string name;
	public int price;
}

public class s_t_zhuanpan
{
	public int id;
	public int zhuanpan_type;
	public int type;
	public int value1;
	public int value2;
	public int value3;
	public int is_flash;
}

public class s_t_zhuanpan_reward
{
	public int rank1;
	public int rank2;
	public int type;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class ab_load_info
{
	public string name;
	public Object obj;
	public HashSet<GameObject> ins_objs = new HashSet<GameObject>();
}

public class s_t_manyou_qiyu
{
	public int id;
	public string name;
	public int type;
	public int def1;
	public int def2;
	public int def3;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_manyou_dati
{
	public int id;
	public string name;
	public string half_image;
	public string question;
	public List<s_t_question> questions = new List<s_t_question>();
}

public class s_t_manyou
{
	public int id;
	public string name;
	public int type;
	public s_t_reward reward = new s_t_reward();
	public int rate;
	public string image;
}

public class s_t_manyou_mubiao
{
	public int id;
	public int score;
	public string name;
	public s_t_reward reward = new s_t_reward();
}
public class s_t_reward_ex
{
    public int type;
    public int value1;
    public long value2;
    public int value3;
    public bool yichu;
    public long value4;
}

public class s_t_manyou_reward
{
	public int rank1;
	public int rank2;
	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class s_t_question
{
	public int id;
	public string answer;
}

public class s_t_role_skillunlock
{
	public int id;
	public string name;
	public string taici;
	public int role_id;
	public int level;
	public List<s_t_role_skillunlock_task> role_skillunlock_tasks = new List<s_t_role_skillunlock_task>();
}

public class s_t_role_skillunlock_task
{
	public int task_type;
	public int def1;
	public int def2;
	public int def3;
}

public class s_t_role_spskillup
{
	public int level;
	public int stone;
	public int gold;
}

public class s_t_pet
{
	public int id;
	public string name;
	public string show;
	public string small_show;
	public string icon;
	public string desc;
	public int color;

	public List<int> cs = new List<int>();
	public List<float> cscz = new List<float>();
	public List<float> shengxing_cz = new List<float>();	
	public List<int> skills = new List<int>();
	public List<float> jinjie_sxcz = new List<float>();
	public List<s_t_attr> jinjie_add_sx = new List<s_t_attr>();
	public List<int> shengxing_sxcz = new List<int>();
	public float shengxing_jncz;
	public int soul_beast ;
	public float sx_add ;
	public float sz_sx_add ;
}
public class s_t_chongwu_shop
{
    public int id;
    public string name;
    public int gezi;
    public int level;
    public int type;
    public int value1;
    public int value2;
    public int value3;
    public int weight;
    public int huobitype;
    public int huobi;
    public int tuijian;
}
public class s_t_pet_jinjie
{
	public int level;
	public string chenghao;
	public string icon;
	public int need_level;
	public List<int> cls = new List<int>();
	public int gold;
	public int qsx_add;
	public int esx_add;
}

public class s_t_pet_jinjieitem
{
	public int id;
	public string name;
	public int color;
	public List<s_t_attr> attrs = new List<s_t_attr>();
}

public class s_t_pet_shengxing
{
	public int level;
	public int need_level;
	public List<int> sp = new List<int>();
	public List<int> stone = new List<int>();
	public List<int> gold = new List<int>();
}

public class s_t_pet_skill
{
	public int id;
	public string name;
	public string des;
	public string action;
	public int attack_type;
	public int target_type;
	
	public int range; //7
	
	public float attack_pe;
	public float attack_rate;
	
	public List<int> buffer_types = new List<int>();
	public List<int> buffer_target_types = new List<int>();
	public List<int> buffer_rounds = new List<int>();
	public List<int> buffer_attack_types = new List<int>();
	public List<float> buffer_attack_pes = new List<float>();
	public List<int> buffer_modify_att_types = new List<int>();
	public List<float> buffer_modify_att_vals = new List<float>();
	
}

public class s_t_pet_target
{
	public int id;
	public string name;
	public List<int> target_ids = new List<int>();
	public List<s_t_attr> attrs = new List<s_t_attr>();
}

public class s_t_comeback
{
	public int id; 
	public string desc; //描述
	public string design; //代号

	public int type; //类型

	public int def1; //需求条件或者价格 
	public int def2; //折扣，40=4折
	public int def3; //限购次数
	public int def4; //等级达到才显示

	public List<s_t_reward> rewards = new List<s_t_reward>();
}

public class game_data  : MonoBehaviour
{
	public save_data m_player_data = new save_data(); //保留账户

	public List<string> m_users = new List<string>();

    public List<int> m_storage_sid = new List<int>();
    public List<int> m_storage_level = new List<int>();

    public e_language m_language = e_language.Simplified;
    public string m_id_price = "";
    public int m_guaji = 0;
	public List<server_list> m_server_list = new List<server_list>();
	public List<post_zd> m_postzd_list = new List<post_zd>();
    ///////////////////////////////////////////////////////////////////////////////////
	public dbc m_dbc_skill = new dbc();
	private Dictionary<int, s_t_skill> m_skill_s = new Dictionary<int, s_t_skill>();
	public dbc m_dbc_pet_skill = new dbc();
	private Dictionary<int, s_t_pet_skill> m_pet_skill_s = new Dictionary<int, s_t_pet_skill>();
	public dbc m_dbc_monster = new dbc();
	private Dictionary<int,s_t_monster> m_monster_s = new Dictionary<int,s_t_monster>();
	public dbc m_dbc_class = new dbc();
	private Dictionary<int,s_t_class> m_class_s = new Dictionary<int,s_t_class>();
    public dbc m_dbc_resource = new dbc();
	public dbc m_dbc_mission = new dbc();
	public Dictionary<int,s_t_mission> m_mission_s = new Dictionary<int,s_t_mission>();
	public dbc m_dbc_language = new dbc();
	public dbc m_dbc_language_prefab = new dbc();
	private Dictionary<string,string> m_languages_s = new Dictionary<string,string>();
	public dbc m_dbc_language_soft = new dbc();
	public dbc m_dbc_name = new dbc();
	public dbc m_dbc_prohibitword = new dbc();
	public dbc m_dbc_error = new dbc();
	public dbc m_dbc_exp = new dbc();
	private Dictionary<int,s_t_exp> m_exps = new Dictionary<int,s_t_exp>();
	public dbc m_dbc_item = new dbc();
	private Dictionary<int,s_t_item> m_item_s = new Dictionary<int,s_t_item>();
    public dbc m_dbc_itemstore = new dbc();
	public dbc m_dbc_equip = new dbc();
	private Dictionary<int,s_t_equip> m_equip_s = new Dictionary<int,s_t_equip>();
	public dbc m_dbc_baowu = new dbc();
	private Dictionary<int,s_t_baowu> m_baowus = new Dictionary<int,s_t_baowu>();
	public dbc m_dbc_enhance = new dbc();
	public dbc m_dbc_gaizao = new dbc();
	public dbc m_dbc_role_shop = new dbc();
	public dbc m_dbc_shop_xg = new dbc();
	public dbc m_dbc_target = new dbc();
	private Dictionary<int,s_t_target> m_target_s = new Dictionary<int,s_t_target>();
    public dbc m_dbc_horreward = new dbc();
    public dbc m_dbc_guild_guwu = new dbc();
	public dbc m_dbc_active = new dbc();
	private Dictionary<int,s_t_active> m_active_s = new Dictionary<int,s_t_active>();
	public dbc m_dbc_boss_active = new dbc();
	private Dictionary<int,s_t_boss_active> m_boss_active_s = new Dictionary<int,s_t_boss_active>();
	public dbc m_dbc_boss_shop = new dbc();
    public dbc m_dbc_pvp_active = new dbc();
    public dbc m_dbc_pvp_reward = new dbc();
    public dbc m_dbc_pvp_shop = new dbc();
    public dbc m_dbc_hongbao_target = new dbc();
    public Dictionary<int, s_t_hongbao_target> m_hongbao_s = new Dictionary<int, s_t_hongbao_target>();
	public dbc m_dbc_treasure_enhance = new dbc();
	public dbc m_dbc_sport_card = new dbc();
	public dbc m_dbc_jiban = new dbc();
	private Dictionary<int,s_t_ji_ban> m_ji_ban_s = new Dictionary<int,s_t_ji_ban>();
	public dbc m_dbc_jibanex = new dbc();
	private Dictionary<int,s_t_ji_banex> m_ji_banex_s = new Dictionary<int,s_t_ji_banex>();
	public dbc m_dbc_map = new dbc();
	private Dictionary<int,s_t_map> m_map_s = new Dictionary<int,s_t_map>();
    public dbc m_dbc_qiyu_tiaozhan = new dbc();
	public dbc m_dbc_vip = new dbc();
	private Dictionary<int,s_t_vip> m_vip_s = new Dictionary<int,s_t_vip>();
	public dbc m_dbc_show_update = new dbc();
	public dbc m_dbc_recharge = new dbc();
	public dbc m_dbc_price = new dbc();
	public dbc m_dbc_first_recharge = new dbc ();
	public dbc m_dbc_online_reward = new dbc ();
	private Dictionary<int,s_t_online_reward> m_online_rewards = new Dictionary<int,s_t_online_reward>();
	public dbc m_dbc_active_reward = new dbc ();
	public dbc m_dbc_daily_sign = new dbc ();
	public dbc m_t_value = new dbc();
	private Dictionary<int,s_t_value> m_value_s = new Dictionary<int,s_t_value>();
	public dbc m_t_face = new dbc();
	public dbc m_dbc_scene_music = new dbc();
	public dbc m_dbc_sport_reward = new dbc();
	public dbc m_dbc_sport_rank = new dbc();
	public dbc m_dbc_huodong = new dbc();
	private Dictionary<int,s_t_huodong> m_huodong_s = new Dictionary<int,s_t_huodong>();
	public dbc m_dbc_huodong_sub = new dbc();
	public dbc m_dbc_ttt = new dbc();
	public dbc m_dbc_ttt_reward = new dbc();
	public dbc m_dbc_ttt_value = new dbc();
	public dbc m_dbc_tip = new dbc();
	public dbc m_dbc_tupo = new dbc();
	public dbc m_dbc_guild_mission = new dbc();
    public Dictionary<int, s_t_guild_mission> m_guild_missions = new Dictionary<int, s_t_guild_mission>();
    public dbc m_dbc_cheng_hao = new dbc();
    private Dictionary<int, s_t_chenghao> m_chenhao_s = new Dictionary<int, s_t_chenghao>();
    //回忆表===================================
    public dbc m_dbc_huiyi = new dbc();
    public dbc m_dbc_huiyi_chengjiu = new dbc();
    public dbc m_dbc_huiyi_luckshop = new dbc();
    public dbc m_dbc_huiyi_lunpan = new dbc();
    public dbc m_dbc_huiyi_mingyun = new dbc();
    public dbc m_dbc_huiyi_shop = new dbc();
	public dbc m_dbc_huiyi_sub = new dbc();
	private Dictionary<int,s_t_huiyi_sub> m_huiyi_subs = new Dictionary<int,s_t_huiyi_sub>();
    //冰原表===================================
    public dbc m_dbc_bingyuan_chenhao = new dbc();
    private Dictionary<int, s_t_bingyuan_chenhao> m_bingyuan_chenhaos = new Dictionary<int, s_t_bingyuan_chenhao>();
    public dbc m_dbc_bingyuan_reward = new dbc();
    public dbc m_dbc_bingyuan_shop = new dbc();
    public dbc m_dbc_bingyuan_mubiao = new dbc();
    public dbc m_dbc_jinjie = new dbc();
	private Dictionary<int,s_t_jinjie> m_jinjies = new Dictionary<int,s_t_jinjie>();
	public dbc m_dbc_skillup = new dbc();
	public dbc m_dbc_guild = new dbc();
    private Dictionary<int, s_t_guild> m_guilds = new Dictionary<int, s_t_guild>();
    public dbc m_dbc_guildfight = new dbc();
    private Dictionary<int, s_t_guildfight> m_guildfights = new Dictionary<int, s_t_guildfight>();
    public dbc m_dbc_guildfight_target = new dbc();
    private Dictionary<int, s_t_guildfight_target> m_guildfight_targets = new Dictionary<int, s_t_guildfight_target>();
    public dbc m_dbc_guildfight_reward = new dbc();
	public dbc m_dbc_guild_icon = new dbc();
	public dbc m_dbc_guild_shop = new dbc();
	public dbc m_dbc_guild_sign = new dbc ();
    public dbc m_dbc_guild_keji = new dbc();
	public dbc m_dbc_dress = new dbc();
	private Dictionary<int,s_t_dress> m_dress_s = new Dictionary<int,s_t_dress>();
	public dbc m_dbc_dress_target = new dbc();
	private Dictionary<int,s_t_dress_target> m_dress_target_s = new Dictionary<int,s_t_dress_target>();
	public dbc m_dbc_role_dress = new dbc();
	public dbc m_dbc_xjbz = new dbc();
	public dbc m_dbc_xjbz_mission = new dbc();
	public dbc m_dbc_danmu = new dbc();
	public dbc m_dbc_huodong_pttq = new dbc();
	public dbc m_dbc_kaifu = new dbc();
	private List< List<s_t_kaifu> > m_kaifu_s = new List<List<s_t_kaifu>>();
	public dbc m_dbc_kaifu_mubiao = new dbc();
	public dbc m_dbc_jc_huodong = new dbc();
	public dbc m_dbc_zzk = new dbc();
	public dbc m_dbc_boss_reward = new dbc();
	public dbc m_dbc_huodong_czjh = new dbc();
    public dbc m_dbc_huodong_czjhrs = new dbc();
    public Dictionary<int, s_t_huodong_czjhrs> m_huo_dong_czjhrs = new Dictionary<int, s_t_huodong_czjhrs>();
    public dbc m_dbc_mofang = new dbc();
    public Dictionary<int, s_t_mofang> m_mofangs = new Dictionary<int, s_t_mofang>();
    public dbc m_dbc_mofang_shop = new dbc();
    public Dictionary<int, s_t_mofang_shop> m_mofang_shops = new Dictionary<int, s_t_mofang_shop>();
    public dbc m_dbc_mofang_reward = new dbc();
    public Dictionary<int, s_t_mofang_reward> m_mofang_rewards = new Dictionary<int, s_t_mofang_reward>();
	public dbc m_dbc_equip_tz = new dbc();
	private Dictionary<int, s_t_equip_tz> m_equip_tz_s = new Dictionary<int, s_t_equip_tz>();
	private Dictionary<int, int> m_equip_tz_map_ = new Dictionary<int, int>();
	public dbc m_dbc_baowu_jl = new dbc();
	private Dictionary< int, s_t_baowu_jl> m_baowu_jl = new  Dictionary<int, s_t_baowu_jl>();
	private Dictionary<int, int> m_treasure_tz_map_ = new Dictionary<int, int>();
	public DFA m_dfa = new DFA ();
	public dbc m_dbc_yb = new dbc();
	public dbc m_dbc_yb_gw = new dbc();
	public dbc m_dbc_ore = new dbc();
	public dbc m_dbc_sport_shop = new dbc();
	public dbc m_dbc_sport_mubiao = new dbc();
	public dbc m_dbc_equip_sx = new dbc();
	private Dictionary< int, s_t_equip_sx> m_equip_sx = new Dictionary<int, s_t_equip_sx>();
	public dbc m_dbc_xinqing_event = new dbc();
	public dbc m_dbc_xinqing_random = new dbc();
	public dbc m_dbc_xinqing = new dbc();
	public dbc m_dbc_boss_dw = new dbc();
    private Dictionary<int, s_t_boss_dw> m_boss_dw_s = new Dictionary<int, s_t_boss_dw>(); 
	public dbc m_dbc_gongzhen = new dbc();
	private Dictionary< int, s_t_gongzhen> m_gongzhens = new Dictionary<int, s_t_gongzhen>();
	public dbc m_dbc_ttt_mibao = new dbc();
	public dbc m_dbc_equip_jl = new dbc();
	public dbc m_dbc_ttt_shop = new dbc();
	public dbc m_dbc_ttt_mubiao = new dbc();
	public dbc m_dbc_guild_mubiao = new dbc();
	public dbc m_dbc_mission_first_reward = new dbc();
    public dbc m_dbc_guild_guanghuan = new dbc();
    private Dictionary<int, s_t_guild_shop_ex> m_guild_guangguans = new Dictionary<int, s_t_guild_shop_ex>();
    private dbc m_dbc_master_duanwei = new dbc();
    public Dictionary<int, s_t_master_duanwei> m_master_duanweis = new Dictionary<int, s_t_master_duanwei>();
    private dbc m_dbc_master_target = new dbc();
    public Dictionary<int, s_t_master_target> m_master_targets = new Dictionary<int, s_t_master_target>();
    private dbc m_dbc_master_reward = new dbc();
    public Dictionary<int, s_t_master_reward> m_master_rewards = new Dictionary<int, s_t_master_reward>();
    public dbc m_dbc_dress_unlock = new dbc();
	public dbc m_dbc_equip_skill = new dbc();
	public dbc m_dbc_vip_libao = new dbc();
	public dbc m_dbc_week_libao = new dbc();
	public dbc m_dbc_guild_shop_xs = new dbc();
	public dbc m_dbc_itemhecheng = new dbc();
	private Dictionary< int, s_t_itemhecheng> m_itemhechengs = new Dictionary<int, s_t_itemhecheng>();
	private Dictionary< int, s_t_itemhecheng> m_pet_itemhechengs = new Dictionary<int, s_t_itemhecheng>();
	public dbc m_dbc_role_shengpin = new dbc();
	public dbc m_dbc_duixing = new dbc();
	public dbc m_dbc_duixing_up = new dbc();
	public dbc m_dbc_duixing_skill = new dbc();
	public int m_self_template_id = 0;
	public dbc m_dbc_tanbao_event = new dbc();
	private Dictionary< int, s_t_tanbao_event> m_tanbaos = new Dictionary<int, s_t_tanbao_event>();
	public dbc m_dbc_tanbao_mubiao = new dbc();
	private Dictionary< int, s_t_tanbao_mubiao> m_tanbao_mubiaos = new Dictionary<int, s_t_tanbao_mubiao>();
	public dbc m_dbc_tanbao_shop = new dbc();
	private Dictionary< int, s_t_tanbao_shop> m_tanbao_shops = new Dictionary<int, s_t_tanbao_shop>();
	public dbc m_dbc_tanbao_reward = new dbc();
	private Dictionary< int, s_t_tanbao_reward> m_tanbao_rewards = new Dictionary<int, s_t_tanbao_reward>();
	public dbc m_dbc_guanghuan = new dbc();
	private Dictionary< int, s_t_guanghuan> m_guanghuans = new Dictionary<int, s_t_guanghuan>();
	public dbc m_dbc_guanghuan_enhance = new dbc();
	private Dictionary< int, s_t_guanghuan_enhance> m_guanghuan_enhances = new Dictionary<int, s_t_guanghuan_enhance>();
	public dbc m_dbc_guanghuan_skill = new dbc();
	private Dictionary< int, s_t_guanghuan_skill> m_guanghuan_skills = new Dictionary<int, s_t_guanghuan_skill>();
	public Dictionary<int, List<int> > m_guanghuan_skill_ids = new Dictionary<int, List<int> >();
	public dbc m_dbc_guanghuan_target = new dbc();
	public Dictionary< int, s_t_guanghuan_target> m_guanghuan_targets = new Dictionary<int, s_t_guanghuan_target>();
	public dbc m_dbc_baowu_sx = new dbc();
	private Dictionary< int, s_t_baowu_sx> m_baowu_sxs = new Dictionary<int, s_t_baowu_sx>();
	public dbc m_dbc_role_dresstarget = new dbc();
	private Dictionary< int, s_t_role_dresstarget> m_role_dresstargets = new Dictionary<int, s_t_role_dresstarget>();
	public dbc m_dbc_chongzhifanpai = new dbc ();
	private Dictionary< int, s_t_chongzhifanpai> m_chongzhifanpais = new Dictionary<int, s_t_chongzhifanpai>();
	public dbc m_dbc_chongzhifanpai_shop = new dbc();
	private Dictionary< int, s_t_chongzhifanpai_shop> m_chongzhifanpai_shops = new Dictionary<int, s_t_chongzhifanpai_shop>();
	public dbc m_dbc_zhuanpan = new dbc();
	private Dictionary< int, s_t_zhuanpan> m_zhuanpans = new Dictionary<int, s_t_zhuanpan>();
	public dbc m_dbc_zhuanpan_reward = new dbc();
	private Dictionary<int,s_t_zhuanpan_reward> m_zhuanpan_rewards = new Dictionary<int,s_t_zhuanpan_reward>();
	public dbc m_dbc_manyou_dati = new dbc();
	private Dictionary< int, s_t_manyou_dati> m_manyou_datis = new Dictionary<int, s_t_manyou_dati>();
	public dbc m_dbc_manyou_qiyu = new dbc();
	private Dictionary< int, s_t_manyou_qiyu> m_manyou_qiyus = new Dictionary<int, s_t_manyou_qiyu>();
	public dbc m_dbc_manyou = new dbc();
	private Dictionary< int, s_t_manyou> m_manyous = new Dictionary<int, s_t_manyou>();
	public dbc m_dbc_manyou_mubiao = new dbc();
	private Dictionary< int, s_t_manyou_mubiao> m_manyou_mubiaos = new Dictionary<int, s_t_manyou_mubiao>();
	public dbc m_dbc_manyou_reward = new dbc();
	private Dictionary<int,s_t_manyou_reward> m_manyou_rewards = new Dictionary<int,s_t_manyou_reward>();
	public dbc m_dbc_role_skillunlock = new dbc();
	private Dictionary<int,s_t_role_skillunlock> m_role_skillunlocks = new Dictionary<int,s_t_role_skillunlock>();
	public dbc m_dbc_role_spskillup = new dbc();
	private Dictionary<int,s_t_role_spskillup> m_role_spskilups = new Dictionary<int,s_t_role_spskillup>();
	public dbc m_dbc_chongwu_shengxing = new dbc();
	private Dictionary<int,s_t_pet_shengxing> m_pet_shengxings = new Dictionary<int,s_t_pet_shengxing>();
	public dbc m_dbc_chongwu_jinjie = new dbc();
	private Dictionary<int,s_t_pet_jinjie> m_pet_jinjies = new Dictionary<int,s_t_pet_jinjie>();
	public dbc m_dbc_chongwu_jinjieitem = new dbc();
	private Dictionary<int,s_t_pet_jinjieitem> m_pet_jinjie_items = new Dictionary<int,s_t_pet_jinjieitem>();
	public dbc m_dbc_chongwu = new dbc();
	private Dictionary<int,s_t_pet> m_pet_s = new Dictionary<int,s_t_pet>();
    public dbc m_dbc_chongwu_shop = new dbc();
    private Dictionary<int, s_t_chongwu_shop> m_chongwu_shop_s = new Dictionary<int, s_t_chongwu_shop>();
	public dbc m_dbc_chongwu_target = new dbc();
	private Dictionary<int, s_t_pet_target> m_chongwu_target_s = new Dictionary<int, s_t_pet_target>();
	public dbc m_dbc_yueka_jijin = new dbc();
	private Dictionary<int,s_t_yueka> m_yueka_s = new Dictionary<int, s_t_yueka>();
	public dbc m_dbc_comeback =new dbc();
	private Dictionary<int ,s_t_comeback> m_comeback = new Dictionary<int, s_t_comeback>();
    public dbc m_dbc_role_gaizao = new dbc();
    private Dictionary<int, s_t_role_gaizao> m_role_gaizao = new Dictionary<int, s_t_role_gaizao>();
    public dbc m_dbc_t_const = new dbc();
    private Dictionary<int, int> m_const = new Dictionary<int, int>();
    public static string[] m_sys_color = { "[ffffff]", "[28e958]", "[33fff5]", "[f908f8]", "[f98c20]", "[f92020]", "[ffff00]" };

    static public game_data _instance;

    void Awake()
    {
        _instance = this;
        m_language = (e_language)platform._instance.get_language();
    }
   
	public string get_name_color(int count)
	{
        if (count >= m_sys_color.Length)
        {
            count = m_sys_color.Length - 1;
        }
		return m_sys_color[count];
	}

    public int get_color_index(string color)
    {
        for (int i = 0; i < m_sys_color.Length; i++)
        {
            if (m_sys_color[i] == color)
            {
                return i;
            }
        }
        return -1;
    }

	public void load()
	{
        if (PlayerPrefs.HasKey("m_token"))
        {
            m_player_data.m_token = PlayerPrefs.GetString("m_token");
        }
        if (PlayerPrefs.HasKey("m_name"))
        {
            m_player_data.m_user = PlayerPrefs.GetString("m_name");
        }
        if (PlayerPrefs.HasKey("m_pass"))
		{
			m_player_data.m_pass = PlayerPrefs.GetString("m_pass");
		}
		if (PlayerPrefs.HasKey("m_channel"))
		{
			m_player_data.m_channel = PlayerPrefs.GetInt("m_channel");
		}
		if (PlayerPrefs.HasKey("high_ver"))
		{
			m_player_data.high_ver = PlayerPrefs.GetInt("high_ver");
		}
		if (PlayerPrefs.HasKey("low_ver"))
		{
			m_player_data.low_ver = PlayerPrefs.GetInt("low_ver");
		}
		if (PlayerPrefs.HasKey("m_sound"))
		{
			m_player_data.m_sound = PlayerPrefs.GetInt("m_sound");
		}
		if (PlayerPrefs.HasKey("m_music"))
		{
			m_player_data.m_music = PlayerPrefs.GetInt("m_music");
		}
        if (PlayerPrefs.HasKey("m_guanghuan"))
        {
            m_player_data.m_guanghuan = PlayerPrefs.GetInt("m_guanghuan");
        }
        if (PlayerPrefs.HasKey("m_speed"))
		{
			m_player_data.m_speed = PlayerPrefs.GetInt("m_speed");
		}
		if (PlayerPrefs.HasKey("m_auto_skill"))
		{
			m_player_data.m_auto_skill = PlayerPrefs.GetInt("m_auto_skill");
		}
		if (PlayerPrefs.HasKey("m_danmu"))
		{
			m_player_data.m_danmu = PlayerPrefs.GetInt("m_danmu");
		}
		if (PlayerPrefs.HasKey("m_id"))
		{
			m_player_data.m_id = PlayerPrefs.GetInt("m_id");
		}
		if (PlayerPrefs.HasKey("m_is_register"))
		{
            m_player_data.m_is_register = PlayerPrefs.GetInt("m_is_register");

            if ( m_player_data.m_is_register == 0 && !m_player_data.m_user.Equals("null"))
            {
                m_player_data.m_is_register = 1;
            }
        }
		save ();
	}
    
	public void save()
	{
        PlayerPrefs.SetString("m_token", m_player_data.m_token);
        PlayerPrefs.SetString("m_name", m_player_data.m_user);
        PlayerPrefs.SetString("m_pass", m_player_data.m_pass);
		PlayerPrefs.SetInt("m_channel", m_player_data.m_channel);
		PlayerPrefs.SetInt("high_ver", m_player_data.high_ver);
		PlayerPrefs.SetInt("low_ver", m_player_data.low_ver);
		PlayerPrefs.SetInt("m_sound", m_player_data.m_sound);
        PlayerPrefs.SetInt("m_music", m_player_data.m_music);
        PlayerPrefs.SetInt("m_guanghuan", m_player_data.m_guanghuan);
        PlayerPrefs.SetInt("m_speed", m_player_data.m_speed);
		PlayerPrefs.SetInt("m_auto_skill", m_player_data.m_auto_skill);
		PlayerPrefs.SetInt("m_danmu", m_player_data.m_danmu);
		PlayerPrefs.SetInt("m_id" ,m_player_data.m_id);
		PlayerPrefs.SetInt("m_is_register", m_player_data.m_is_register);
		PlayerPrefs.Save ();
	}
	
	public Object get_object_res(string name, System.Type type)
	{
		Object _object = Resources.Load(name);
		return _object;
	}

	public GameObject ins_object_res(string name)
	{
		GameObject _res = get_object_res(name, typeof(GameObject)) as GameObject;
		if (_res)
		{
			GameObject _object = MonoBehaviour.Instantiate(_res);
			return _object;
		}
		return null;
	}

	public string get_value_string(int id, float value,int type = 0)
	{
		string s = "";
		if(type == 0)
		{
			s = get_t_language(m_t_value.get_index (4, id));
		}
		else if(type == 1)
		{
            s = get_t_language ("dress_detail_gui_soft15") + get_t_language(m_t_value.get_index(4, id));//全队
			if(game_data._instance.m_language == e_language.English)
			{
                s = get_t_language ("dress_detail_gui_soft15") + " " + get_t_language(m_t_value.get_index(4, id));//全队
			}
		}

		s = s.Replace ("{n}", value.ToString());
		return s;
	}

	public string get_float_string(int id, float value)
	{
		if(id <= 0)
		{
			return "";
		}
		string s = get_t_language(m_t_value.get_index (4, id));
		value = (float)decimal.Round ((decimal)value, 2);
		s = s.Replace ("{n}", value.ToString());
		return s;
	}

	public s_t_value get_t_value(int id)
	{
		return m_value_s[id];
	}

	public string get_scene_music(string name)
	{
		for(int i = 0;i < m_dbc_scene_music.get_y();i ++)
		{
			if(m_dbc_scene_music.get(0,i) == name)
			{
				return m_dbc_scene_music.get(1,i);
			}
		}
		return "";
	}
   
    public s_t_guild_mission get_t_guild_mission(int index)
    {
        if (m_guild_missions.ContainsKey(index))
        {
            return m_guild_missions[index];
        }
        return null;
    }

    void parse_t_guild_mission()
    {
        m_dbc_guild_mission.load_txt("t_guild_mission", 0);
        foreach (int index in m_dbc_guild_mission.m_index.Keys)
        {
            s_t_guild_mission _t_guild_mission = new s_t_guild_mission();
        _t_guild_mission.index = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(0,index));
        _t_guild_mission.name = get_t_language(game_data._instance.m_dbc_guild_mission.get_index(2, index));
        for (int i = 0; i < 4; i++)
        {
            _t_guild_mission.ids.Add(int.Parse(game_data._instance.m_dbc_guild_mission.get_index(3 + i * 2,index)));
        }
        _t_guild_mission.basicCon = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(11, index));
        _t_guild_mission.maxCon = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(12, index));
        _t_guild_mission.jishaCon = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(13, index));
        _t_guild_mission.exp = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(14, index));
        for (int j = 0; j < 4; j++)
        {
            s_t_mision_reward er = new s_t_mision_reward();
            er.type = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(15 + j * 7, index));
            er.value1 = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(16 + j * 7, index));
            for (int k = 0; k < 5; k++)
            {
                int value2 = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(17 + j * 7 + k, index));
                er.value2s.Add(value2);
            }
            
            _t_guild_mission.slrewarxds.Add(er);
        }
            
        for (int i = 0; i < 3; i++)
        {
            s_t_reward _reward = new s_t_reward();
                _reward.type = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(43 + i * 4, index));
                _reward.value1 = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(44 + i * 4, index));
                _reward.value2 = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(45 + i * 4, index));
                _reward.value3 = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(46 + i * 4, index));
            _t_guild_mission.firstRewards.Add(_reward);
        }
        _t_guild_mission.level = int.Parse(game_data._instance.m_dbc_guild_mission.get_index(46 + 2 * 4 + 1, index));
        m_guild_missions[index] = _t_guild_mission;
    }
   
 
    }
    public s_t_bingyuan_reward get_t_bingyuan_reward(int id)
    {
        s_t_bingyuan_reward _reward = new s_t_bingyuan_reward();
        if (!m_dbc_bingyuan_reward.has_index(id))
        {
            return null;
        }
        _reward.rank1 = int.Parse(m_dbc_bingyuan_reward.get_index(0, id));
        _reward.rank2 = int.Parse(m_dbc_bingyuan_reward.get_index(1, id));
        for (int i = 0; i < 2; i++)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = int.Parse(m_dbc_bingyuan_reward.get_index(4 * i + 2, id));
            reward.value1 = int.Parse(m_dbc_bingyuan_reward.get_index(4 * i + 3, id));
            reward.value2 = int.Parse(m_dbc_bingyuan_reward.get_index(4 * i + 4, id));
            reward.value3 = int.Parse(m_dbc_bingyuan_reward.get_index(4 * i + 5, id));
            _reward.rewards.Add(reward);
        }
        return _reward;
    }
    public s_t_guildfight_reward get_t_guildfight_reward(int id, int type)
    {

        for (int i = 0; i < m_dbc_guildfight_reward.get_y(); i++)
        {
            int _rank1 = int.Parse(m_dbc_guildfight_reward.get(0,i));
            int _type = int.Parse(m_dbc_guildfight_reward.get(2, i));
            if (id == _rank1 && type == _type)
            {
                 s_t_guildfight_reward _reward = new s_t_guildfight_reward();
                 _reward.rank1 = _rank1;
                 _reward.type = _type;
                 _reward.rank2 = int.Parse(m_dbc_guildfight_reward.get(1, i));
                 for (int j = 0; j < 4; j++)
                 {
                     s_t_reward reward = new s_t_reward();
                     reward.type = int.Parse(m_dbc_guildfight_reward.get(3 + j * 4, i));
                     reward.value1 = int.Parse(m_dbc_guildfight_reward.get(4 + j * 4, i));
                     reward.value2 = int.Parse(m_dbc_guildfight_reward.get(5 + j * 4, i));
                     reward.value3 = int.Parse(m_dbc_guildfight_reward.get(6 + j * 4, i));
                     if (reward.type > 0)
                     {
                         _reward.rewards.Add(reward);
                     }
                 }
                 return _reward;
            }
        }
        return null;
    }
    public s_t_bingyuan_mubiao get_t_bingyuan_mubiao(int id)
    {
        s_t_bingyuan_mubiao _mubiao = new s_t_bingyuan_mubiao();
        _mubiao.id = id;
        _mubiao.jifen = int.Parse(game_data._instance.m_dbc_bingyuan_mubiao.get_index(1, id));
        _mubiao.type = int.Parse(game_data._instance.m_dbc_bingyuan_mubiao.get_index(3, id));
        _mubiao.value1 = int.Parse(game_data._instance.m_dbc_bingyuan_mubiao.get_index(4, id));
        _mubiao.value2 = int.Parse(game_data._instance.m_dbc_bingyuan_mubiao.get_index(5, id));
        _mubiao.value3 = int.Parse(game_data._instance.m_dbc_bingyuan_mubiao.get_index(6, id));
        _mubiao.price = int.Parse(game_data._instance.m_dbc_bingyuan_mubiao.get_index(7, id));
        _mubiao.discount = int.Parse(game_data._instance.m_dbc_bingyuan_mubiao.get_index(8, id));
        return _mubiao;
    }
    public s_t_bingyuan_shop get_t_bingyuan_shop(int id)
    {
        s_t_bingyuan_shop _shop = new s_t_bingyuan_shop();
        _shop.id = id;
        _shop.name = m_dbc_bingyuan_shop.get_index(1, id);
        _shop.type = int.Parse(m_dbc_bingyuan_shop.get_index(2, id));
        _shop.value1 = int.Parse(m_dbc_bingyuan_shop.get_index(3, id));
        _shop.value2 = int.Parse(m_dbc_bingyuan_shop.get_index(4, id));
        _shop.value3 = int.Parse(m_dbc_bingyuan_shop.get_index(5, id));
        _shop.binjin = int.Parse(m_dbc_bingyuan_shop.get_index(6, id));
        _shop.buy_count = int.Parse(m_dbc_bingyuan_shop.get_index(7, id));
        _shop.level = int.Parse(m_dbc_bingyuan_shop.get_index(8,id));
        return _shop;
    }
    public s_t_huiyi get_t_huiyi(int id)
    {
        s_t_huiyi _huiyi = new s_t_huiyi();
        if (!m_dbc_huiyi.has_index(id))
        {
            return null;
        }
        _huiyi.id = id;
        _huiyi.name = get_t_language(m_dbc_huiyi.get_index(2, id));
        _huiyi.level = int.Parse(m_dbc_huiyi.get_index(3,id));
        _huiyi.pre_num = int.Parse(m_dbc_huiyi.get_index(4, id));
        _huiyi.icon = m_dbc_huiyi.get_index(5,id);
        _huiyi.bg = m_dbc_huiyi.get_index(6,id);
        return _huiyi;
    }
    public s_t_huiyi_chengjiu get_t_huiyi_chengjiu(int id)
    {
        s_t_huiyi_chengjiu _chengjiu = new s_t_huiyi_chengjiu();
        if (!m_dbc_huiyi_chengjiu.has_index(id))
        {
            return null;
        }
        _chengjiu.id = id;
        _chengjiu.mem_value = int.Parse(m_dbc_huiyi_chengjiu.get_index(1,id));
        _chengjiu.attr = int.Parse(m_dbc_huiyi_chengjiu.get_index(2, id));
        _chengjiu.value = int.Parse(m_dbc_huiyi_chengjiu.get_index(3, id));
        return _chengjiu;
    }
    public s_t_huiyi_luckshop get_t_huiyi_luckshop(int id)
    {
        s_t_huiyi_luckshop _luckshop = new s_t_huiyi_luckshop();
        if(!m_dbc_huiyi_luckshop.has_index(id))
        {
            return null;
        }
        _luckshop.id = id;
        _luckshop.name = m_dbc_huiyi_luckshop.get_index(1,id);
        _luckshop.page = int.Parse(m_dbc_huiyi_luckshop.get_index(2, id));
        _luckshop.day_num = int.Parse(m_dbc_huiyi_luckshop.get_index(3, id));
        _luckshop.reward.type = int.Parse(m_dbc_huiyi_luckshop.get_index(4,id));
        _luckshop.reward.value1 = int.Parse(m_dbc_huiyi_luckshop.get_index(5, id));
        _luckshop.reward.value2 = int.Parse(m_dbc_huiyi_luckshop.get_index(6, id));
        _luckshop.reward.value3 = int.Parse(m_dbc_huiyi_luckshop.get_index(7, id));
        _luckshop.luck_point = int.Parse(m_dbc_huiyi_luckshop.get_index(8, id));
        return _luckshop;
    }
    public s_t_huiyi_lunpan get_t_huiyi_lunpan(int id)
    {
        s_t_huiyi_lunpan _lunpan = new s_t_huiyi_lunpan();
        if (!m_dbc_huiyi_lunpan.has_index(id))
        {
			return null;
        }
        _lunpan.id = id;
        _lunpan.name = m_dbc_huiyi_lunpan.get_index(1,id);
        _lunpan.type = int.Parse(m_dbc_huiyi_lunpan.get_index(2,id));
        _lunpan.weight = int.Parse(m_dbc_huiyi_lunpan.get_index(3, id));
        _lunpan.luck_point = int.Parse(m_dbc_huiyi_lunpan.get_index(4,id));
        return _lunpan;
    }
    public s_t_huiyi_destiny get_t_huiyi_destiny(int id)
    {
        s_t_huiyi_destiny _destiny = new s_t_huiyi_destiny();
        if (!m_dbc_huiyi_mingyun.has_index(id))
        {
            return null;
        }
        _destiny.id = id;
        _destiny.name = m_dbc_huiyi_mingyun.get_index(1, id);
        _destiny.weight = int.Parse(m_dbc_huiyi_mingyun.get_index(2, id));
        _destiny.huiyi_point = int.Parse(m_dbc_huiyi_mingyun.get_index(3,id));
        return _destiny;
    }
    public s_t_huiyi_shop get_t_huiyi_shop(int id)
    {
        s_t_huiyi_shop _shop = new s_t_huiyi_shop();
        if (!m_dbc_huiyi_shop.has_index(id))
        {
            return null;
        }
        _shop.id = id;
        _shop.name = m_dbc_huiyi_shop.get_index(1, id);
        _shop.gezi = int.Parse(m_dbc_huiyi_shop.get_index(2, id));
        _shop.reward.type = int.Parse(m_dbc_huiyi_shop.get_index(3, id));
        _shop.reward.value1 = int.Parse(m_dbc_huiyi_shop.get_index(4, id));
        _shop.reward.value2 = int.Parse(m_dbc_huiyi_shop.get_index(5, id));
        _shop.reward.value3 = int.Parse(m_dbc_huiyi_shop.get_index(6, id));
        _shop.weight = int.Parse(m_dbc_huiyi_shop.get_index(7, id));
        _shop.huobi_type = int.Parse(m_dbc_huiyi_shop.get_index(8, id));
        _shop.huobi_value = int.Parse(m_dbc_huiyi_shop.get_index(9, id));
        return _shop;
    }
    void parse_t_huiyi_sub()
    {
        m_dbc_huiyi_sub.load_txt("t_huiyi_sub", 0);
        foreach (int id in m_dbc_huiyi_sub.m_index.Keys)
        {
            s_t_huiyi_sub _sub = new s_t_huiyi_sub();
            _sub.id = id;
            _sub.name = get_t_language(m_dbc_huiyi_sub.get_index(2, id));
            _sub.page = int.Parse(m_dbc_huiyi_sub.get_index(3, id));
            _sub.dialog = get_t_language(m_dbc_huiyi_sub.get_index(5, id));
            for (int i = 0; i < 5; i++)
            {
                int temp = int.Parse(m_dbc_huiyi_sub.get_index(6 + i, id));
                if (temp != 0)
                {
                    _sub.huiyis.Add(temp);
                }
            }
            _sub.huiyi_value = int.Parse(m_dbc_huiyi_sub.get_index(11, id));
            for (int i = 0; i < 4; i++)
            {
				int temp = int.Parse(m_dbc_huiyi_sub.get_index(12 + i * 3, id));
                float temp1 = float.Parse(m_dbc_huiyi_sub.get_index(13 + i * 3, id));
				float temp2 = float.Parse(m_dbc_huiyi_sub.get_index(14 + i * 3, id));
                if (temp != 0)
                {
                    _sub.attrs.Add(temp);
                    _sub.values.Add(temp1);
					_sub.values2.Add(temp2);// 成长属性值
                }
            }
            m_huiyi_subs[id] = _sub;
        }
    }
    public s_t_huiyi_sub get_t_huiyi_sub(int id)
    {
		if(m_huiyi_subs.ContainsKey(id))
		{
			return m_huiyi_subs[id];
		}
        return null;
    }
	public s_t_ttt get_t_ttt(int index)
	{
		s_t_ttt _ttt = new s_t_ttt ();
		
		if (!game_data._instance.m_dbc_ttt.has_index(index))
		{
			return null;
		}
		
		_ttt.index = int.Parse (game_data._instance.m_dbc_ttt.get_index(0,index));
		for (int i = 0; i < 3; ++i)
		{
			s_t_ttt_guai guai = new s_t_ttt_guai();
			guai.id1 = int.Parse (game_data._instance.m_dbc_ttt.get_index(1 + i * 8,index));
			guai.id2 = int.Parse (game_data._instance.m_dbc_ttt.get_index(2 + i * 8,index));
			guai.id3 = int.Parse (game_data._instance.m_dbc_ttt.get_index(3 + i * 8,index));
			guai.id4 = int.Parse (game_data._instance.m_dbc_ttt.get_index(4 + i * 8,index));
			guai.id5 = int.Parse (game_data._instance.m_dbc_ttt.get_index(5 + i * 8,index));

			guai.gold = int.Parse (game_data._instance.m_dbc_ttt.get_index(6 + i * 8,index));
			guai.hj = int.Parse (game_data._instance.m_dbc_ttt.get_index(7 + i * 8,index));
			guai.bf = game_data._instance.m_dbc_ttt.get_index(8 + i * 8,index);

			_ttt.guais.Add(guai);
		}
		_ttt.tjtype = int.Parse (game_data._instance.m_dbc_ttt.get_index(25,index));
		_ttt.tjvalue = int.Parse (game_data._instance.m_dbc_ttt.get_index(26,index));
		_ttt.tj = get_t_language(game_data._instance.m_dbc_ttt.get_index(28,index));
		_ttt.desc = get_t_language(game_data._instance.m_dbc_ttt.get_index(29,index));
		
		return _ttt;
	}

	public s_t_ttt_reward get_t_ttt_reward(int index)
	{
		s_t_ttt_reward _ttt_reward = new s_t_ttt_reward ();
		
		if (!game_data._instance.m_dbc_ttt_reward.has_index(index))
		{
			return null;
		}
		
		_ttt_reward.index = int.Parse (game_data._instance.m_dbc_ttt_reward.get_index(0,index));
		for (int i = 0; i < 3; ++i)
		{
			List<s_t_reward> rewards = new List<s_t_reward>();
			for (int j = 0; j < 3; ++j)
			{
				s_t_reward reward = new s_t_reward();
				reward.type = int.Parse (game_data._instance.m_dbc_ttt_reward.get_index(1 + i * 12 + j * 4,index));
				reward.value1 = int.Parse (game_data._instance.m_dbc_ttt_reward.get_index(2 + i * 12 + j * 4,index));
				reward.value2 = int.Parse (game_data._instance.m_dbc_ttt_reward.get_index(3 + i * 12 + j * 4,index));
				reward.value3 = int.Parse (game_data._instance.m_dbc_ttt_reward.get_index(4 + i * 12 + j * 4,index));
				rewards.Add(reward);
			}
			_ttt_reward.rewardss.Add(rewards);
		}
		
		return _ttt_reward;
	}

	public s_t_ttt_value get_t_ttt_value(int id)
	{
		s_t_ttt_value _ttt_value = new s_t_ttt_value ();
		
		if (!game_data._instance.m_dbc_ttt_value.has_index(id))
		{
			return null;
		}
		
		_ttt_value.id = int.Parse (game_data._instance.m_dbc_ttt_value.get_index(0,id));
		_ttt_value.type = int.Parse (game_data._instance.m_dbc_ttt_value.get_index(1,id));
		_ttt_value.xh = int.Parse (game_data._instance.m_dbc_ttt_value.get_index(2,id));
		_ttt_value.sxtype = int.Parse (game_data._instance.m_dbc_ttt_value.get_index(3,id));
		_ttt_value.sxvalue = int.Parse (game_data._instance.m_dbc_ttt_value.get_index(4,id));
		
		return _ttt_value;
	}

	public s_t_ttt_shop get_t_ttt_shop(int id)
	{
		s_t_ttt_shop _ttt_shop = new s_t_ttt_shop ();
		
		if (!game_data._instance.m_dbc_ttt_shop.has_index(id))
		{
			return null;
		}
		
		_ttt_shop.id = int.Parse (game_data._instance.m_dbc_ttt_shop.get_index(0,id));
		_ttt_shop.name = game_data._instance.m_dbc_ttt_shop.get_index(1,id);
		_ttt_shop.fen_ye = int.Parse (game_data._instance.m_dbc_ttt_shop.get_index(2,id));
		_ttt_shop.level = int.Parse (game_data._instance.m_dbc_ttt_shop.get_index(3,id));
		_ttt_shop.type = int.Parse (game_data._instance.m_dbc_ttt_shop.get_index(4,id));
		_ttt_shop.value1 = int.Parse (game_data._instance.m_dbc_ttt_shop.get_index(5,id));
		_ttt_shop.value2 = int.Parse (game_data._instance.m_dbc_ttt_shop.get_index(6,id));
		_ttt_shop.value3 = int.Parse (game_data._instance.m_dbc_ttt_shop.get_index(7,id));
		_ttt_shop.price = int.Parse (game_data._instance.m_dbc_ttt_shop.get_index(8,id));

		return _ttt_shop;
	}

	public s_t_ttt_mubiao get_t_ttt_mubiao(int id)
	{
		s_t_ttt_mubiao _ttt_shop_mubiao = new s_t_ttt_mubiao ();
		
		if (!game_data._instance.m_dbc_ttt_mubiao.has_index(id))
		{
			return null;
		}
		
		_ttt_shop_mubiao.id = int.Parse (game_data._instance.m_dbc_ttt_mubiao.get_index(0,id));
		_ttt_shop_mubiao.star = int.Parse (game_data._instance.m_dbc_ttt_mubiao.get_index(1,id));
		_ttt_shop_mubiao.name = game_data._instance.m_dbc_ttt_mubiao.get_index(2,id);
		_ttt_shop_mubiao.type = int.Parse (game_data._instance.m_dbc_ttt_mubiao.get_index(3,id));
		_ttt_shop_mubiao.value1 = int.Parse (game_data._instance.m_dbc_ttt_mubiao.get_index(4,id));
		_ttt_shop_mubiao.value2 = int.Parse (game_data._instance.m_dbc_ttt_mubiao.get_index(5,id));
		_ttt_shop_mubiao.value3 = int.Parse (game_data._instance.m_dbc_ttt_mubiao.get_index(6,id));
		_ttt_shop_mubiao.price = int.Parse (game_data._instance.m_dbc_ttt_mubiao.get_index(7,id));
		_ttt_shop_mubiao.discount = int.Parse (game_data._instance.m_dbc_ttt_mubiao.get_index(8,id));
		
		return _ttt_shop_mubiao;
	}

	public s_t_huodong_sub get_t_huodong_sub(int id)
	{
		s_t_huodong_sub _huodong = new s_t_huodong_sub ();
		if (!game_data._instance.m_dbc_huodong_sub.has_index(id))
		{
			return null;
		}
		_huodong.id = int.Parse (game_data._instance.m_dbc_huodong_sub.get_index(0,id));
		_huodong.pid = int.Parse (game_data._instance.m_dbc_huodong_sub.get_index(1,id));
		_huodong.name = get_t_language(game_data._instance.m_dbc_huodong_sub.get_index(3,id));
		_huodong.level = int.Parse (game_data._instance.m_dbc_huodong_sub.get_index(4,id));
		_huodong.vip = int.Parse (game_data._instance.m_dbc_huodong_sub.get_index(5,id));
		_huodong.mission_id = int.Parse (game_data._instance.m_dbc_huodong_sub.get_index(6,id));
	
		for(int i = 7;i < 23;i ++)
		{
			_huodong.values.Add(int.Parse (game_data._instance.m_dbc_huodong_sub.get_index(i,id)));
		}
		return _huodong;
	}

	public s_t_huodong get_t_huodong(int id)
	{
		if(m_huodong_s.ContainsKey(id))
		{
			return m_huodong_s[id];
		}
		s_t_huodong _huodong = new s_t_huodong ();
		if (!game_data._instance.m_dbc_huodong.has_index(id))
		{
			return null;
		}
		_huodong.id = int.Parse (game_data._instance.m_dbc_huodong.get_index(0,id));
		_huodong.name = get_t_language(game_data._instance.m_dbc_huodong.get_index(2,id));
		_huodong.type = int.Parse (game_data._instance.m_dbc_huodong.get_index(3,id));
		_huodong.sdes = get_t_language(game_data._instance.m_dbc_huodong.get_index(4,id));

		for(int i = 5;i < 13;i ++)
		{
			_huodong.times.Add(int.Parse (game_data._instance.m_dbc_huodong.get_index(i,id)));
		}
		_huodong.des = get_t_language(game_data._instance.m_dbc_huodong.get_index(13,id));
		_huodong.icon = int.Parse (game_data._instance.m_dbc_huodong.get_index(14,id));
		m_huodong_s [id] = _huodong;
		return _huodong;
	}

	public s_t_exp get_t_exp(int level)
	{	
		if(m_exps.ContainsKey(level))
		{	
			return m_exps[level];
		}
		s_t_exp t_exp = new s_t_exp();
		if (!game_data._instance.m_dbc_exp.has_index(level))
		{	
			return null;
		}
		t_exp.level = int.Parse(game_data._instance.m_dbc_exp.get_index(0,level));
		t_exp.exp = int.Parse(game_data._instance.m_dbc_exp.get_index(1,level));
		t_exp.tili = int.Parse(game_data._instance.m_dbc_exp.get_index(2,level));
		t_exp.regain_tili = int.Parse(game_data._instance.m_dbc_exp.get_index(3,level));
        t_exp.role_exp = int.Parse(game_data._instance.m_dbc_exp.get_index(4, level));
        t_exp.role_zi_exp = int.Parse(game_data._instance.m_dbc_exp.get_index(5, level));
        t_exp.role_jin_exp = int.Parse(game_data._instance.m_dbc_exp.get_index(6, level));
		t_exp.role_hong_exp = int.Parse(game_data._instance.m_dbc_exp.get_index(7, level));
		t_exp.yuanli = int.Parse (game_data._instance.m_dbc_exp.get_index (8, level));
		t_exp.zhanhun = int.Parse (game_data._instance.m_dbc_exp.get_index (9, level));
		t_exp.dxqhzz = int.Parse (game_data._instance.m_dbc_exp.get_index (10, level));
        t_exp.suc_bingjin = int.Parse(game_data._instance.m_dbc_exp.get_index(11, level));
        t_exp.fail_bingjin = int.Parse(game_data._instance.m_dbc_exp.get_index(12, level));
		t_exp.pet_exp = int.Parse(game_data._instance.m_dbc_exp.get_index(13, level));
		t_exp.pet_zi_exp = int.Parse(game_data._instance.m_dbc_exp.get_index(14, level));
		t_exp.pet_jin_exp = int.Parse(game_data._instance.m_dbc_exp.get_index(15, level));
		t_exp.pet_hong_exp = int.Parse(game_data._instance.m_dbc_exp.get_index(16, level));
        t_exp.desc = get_t_language(game_data._instance.m_dbc_exp.get_index(18,level));
		t_exp.icon = game_data._instance.m_dbc_exp.get_index(19,level);
		t_exp.tg = game_data._instance.m_dbc_exp.get_index(20,level);
		t_exp.desc2 = get_t_language(game_data._instance.m_dbc_exp.get_index(22,level));
		t_exp.icon2 = game_data._instance.m_dbc_exp.get_index(23,level);
		t_exp.tg2 = game_data._instance.m_dbc_exp.get_index(24,level);
		t_exp.type = int.Parse (game_data._instance.m_dbc_exp.get_index(25,level));
		m_exps [level] = t_exp;
		return t_exp;
	}
	public s_t_sport_card get_t_sport_card(int id)
	{
		s_t_sport_card _sport_card = new s_t_sport_card ();
		if(!m_dbc_sport_card.has_index(id))
		{
			return null;
		}
		_sport_card.id = int.Parse (m_dbc_sport_card.get_index (0, id));
		_sport_card.level1 = int.Parse (m_dbc_sport_card.get_index (1, id));
		_sport_card.level2 = int.Parse (m_dbc_sport_card.get_index (2, id));
		_sport_card.type = int.Parse (m_dbc_sport_card.get_index (4, id));
		_sport_card.value1 = int.Parse (m_dbc_sport_card.get_index (5, id));
		_sport_card.value2 = int.Parse (m_dbc_sport_card.get_index (6, id));
		_sport_card.value3 = int.Parse (m_dbc_sport_card.get_index (7, id));
		_sport_card.baodi = m_dbc_sport_card.get_index (3, id);
		_sport_card.weight = int.Parse (m_dbc_sport_card.get_index (8, id));
		return _sport_card;
	}
	public s_t_ji_ban get_t_ji_ban(int id)
	{
		if(m_ji_ban_s.ContainsKey(id))
		{
			return m_ji_ban_s[id];
		}
		s_t_ji_ban _ji_ban = new s_t_ji_ban ();
		if (!game_data._instance.m_dbc_jiban.has_index(id))
		{
			return null;
		}
		_ji_ban.id = int.Parse(game_data._instance.m_dbc_jiban.get_index(0,id));
		_ji_ban.name =get_t_language(game_data._instance.m_dbc_jiban.get_index(2,id));
		_ji_ban.type = int.Parse(game_data._instance.m_dbc_jiban.get_index(3,id));
		for (int i = 0; i < 4; ++i)
		{
			int tid = int.Parse(game_data._instance.m_dbc_jiban.get_index(4 + i,id));
			if (tid > 0)
			{
				_ji_ban.tids.Add(tid);
			}
		}
		_ji_ban.attr1 = int.Parse(game_data._instance.m_dbc_jiban.get_index(8,id));
		_ji_ban.value1 = int.Parse(game_data._instance.m_dbc_jiban.get_index(9,id));
		_ji_ban.attr2 = int.Parse(game_data._instance.m_dbc_jiban.get_index(10,id));
		_ji_ban.value2 = int.Parse(game_data._instance.m_dbc_jiban.get_index(11,id));
		m_ji_ban_s [id] = _ji_ban;
		return _ji_ban;
	}

	public s_t_ji_banex get_t_ji_banex(int id)
	{
		if(m_ji_banex_s.ContainsKey(id))
		{
			return m_ji_banex_s[id];
		}
		s_t_ji_banex _ji_banex = new s_t_ji_banex ();
		if (!game_data._instance.m_dbc_jibanex.has_index(id))
		{
			return null;
		}
		_ji_banex.id = int.Parse(game_data._instance.m_dbc_jibanex.get_index(0,id));
		_ji_banex.name = get_t_language(game_data._instance.m_dbc_jibanex.get_index(2,id));
		for (int i = 0; i < 4; ++i)
		{
			int tid = int.Parse(game_data._instance.m_dbc_jibanex.get_index(3 + i,id));
			if (tid > 0)
			{
				_ji_banex.tids.Add(tid);
			}
		}
		_ji_banex.attr = int.Parse(game_data._instance.m_dbc_jibanex.get_index(7,id));
		_ji_banex.value = int.Parse(game_data._instance.m_dbc_jibanex.get_index(8,id));
		m_ji_banex_s [id] = _ji_banex;
		return _ji_banex;
	}

	private void parse_t_skill()
	{
		m_dbc_skill.load_txt("t_role_skill",0);
		foreach (int id in m_dbc_skill.m_index.Keys)
		{
			s_t_skill _skill = new s_t_skill();
			_skill.id = int.Parse(game_data._instance.m_dbc_skill.get_index(0,id));
			_skill.name = get_t_language(game_data._instance.m_dbc_skill.get_index(3,id));
			_skill.des = get_t_language(game_data._instance.m_dbc_skill.get_index(4,id));
			_skill.icon = game_data._instance.m_dbc_skill.get_index(5,id);
			_skill.action = game_data._instance.m_dbc_skill.get_index(6,id);
			_skill.type = int.Parse(game_data._instance.m_dbc_skill.get_index(7,id));
			_skill.attack_type = int.Parse(game_data._instance.m_dbc_skill.get_index(8,id));
			_skill.target_type = int.Parse(game_data._instance.m_dbc_skill.get_index(9,id));
			_skill.range = int.Parse(game_data._instance.m_dbc_skill.get_index(10,id)); //7
			_skill.attack_pe = float.Parse(game_data._instance.m_dbc_skill.get_index(11,id));
			_skill.attack_pe_add = float.Parse(game_data._instance.m_dbc_skill.get_index(12,id));
			for (int i = 0; i < 2; ++i)
			{
				int buffer_type = int.Parse(game_data._instance.m_dbc_skill.get_index(16 + i * 9,id));
				int buffer_target_type = int.Parse(game_data._instance.m_dbc_skill.get_index(17 + i * 9,id));
				int buffer_round = int.Parse(game_data._instance.m_dbc_skill.get_index(18 + i * 9,id));
				int buffer_attack_type = int.Parse(game_data._instance.m_dbc_skill.get_index(19 + i * 9,id));
				float buffer_attack_pe = float.Parse(game_data._instance.m_dbc_skill.get_index(20 + i * 9,id));
				float buffer_attack_pe_add = float.Parse(game_data._instance.m_dbc_skill.get_index(21 + i * 9,id));
				int buffer_modify_att_type = int.Parse(game_data._instance.m_dbc_skill.get_index(22 + i * 9,id));
				float buffer_modify_att_val = float.Parse(game_data._instance.m_dbc_skill.get_index(23 + i * 9,id));
				float buffer_modify_att_val_add = float.Parse(game_data._instance.m_dbc_skill.get_index(24 + i * 9,id));
				_skill.buffer_types.Add(buffer_type);
				_skill.buffer_target_types.Add(buffer_target_type);
				_skill.buffer_rounds.Add(buffer_round);
				_skill.buffer_attack_types.Add(buffer_attack_type);
				_skill.buffer_attack_pes.Add(buffer_attack_pe);
				_skill.buffer_attack_pe_adds.Add(buffer_attack_pe_add);
				_skill.buffer_modify_att_types.Add(buffer_modify_att_type);
				_skill.buffer_modify_att_vals.Add(buffer_modify_att_val);
				_skill.buffer_modify_att_val_adds.Add(buffer_modify_att_val_add);
			}
			_skill.passive_type = int.Parse(game_data._instance.m_dbc_skill.get_index(36,id));
			_skill.passive_modify_att_type = int.Parse(game_data._instance.m_dbc_skill.get_index(37,id));
			_skill.passive_modify_att_val = float.Parse(game_data._instance.m_dbc_skill.get_index(38,id));
			_skill.passive_modify_att_val_add = float.Parse(game_data._instance.m_dbc_skill.get_index(39,id));
			_skill.base_ex_type = int.Parse(game_data._instance.m_dbc_skill.get_index(40,id));
			_skill.base_ex_type_val_0 = int.Parse(game_data._instance.m_dbc_skill.get_index(41,id));
			_skill.base_ex_type_val_1 = float.Parse(game_data._instance.m_dbc_skill.get_index(42,id));
			_skill.base_ex_type_val_2 = float.Parse(game_data._instance.m_dbc_skill.get_index(43,id));
			_skill.add_ex_type_val_1 = float.Parse(game_data._instance.m_dbc_skill.get_index(44,id));
			_skill.add_ex_type_val_2 = float.Parse(game_data._instance.m_dbc_skill.get_index(45,id));
			m_skill_s.Add(id, _skill);
		}
	}

	public s_t_skill get_t_skill(int id)
	{
		if(m_skill_s.ContainsKey(id))
		{
			return m_skill_s[id];
		}
		return null;
	}

	public s_t_monster get_t_monster(int id)
	{
		if(m_monster_s.ContainsKey(id))
		{
			return m_monster_s[id];
		}
		s_t_monster _t_monster = new s_t_monster ();
		if (!game_data._instance.m_dbc_monster.has_index(id))
		{
			return null;
		}
		_t_monster.id = int.Parse(game_data._instance.m_dbc_monster.get_index(0,id));
		_t_monster.class_id = int.Parse(game_data._instance.m_dbc_monster.get_index(1,id));
		_t_monster.level = int.Parse(game_data._instance.m_dbc_monster.get_index(2,id));
		_t_monster.jlevel = int.Parse(game_data._instance.m_dbc_monster.get_index(3,id));
		_t_monster.glevel = int.Parse(game_data._instance.m_dbc_monster.get_index(4,id));
		_t_monster.pinzhi_skill = int.Parse(game_data._instance.m_dbc_monster.get_index(5,id));
		_t_monster.is_boss = int.Parse(game_data._instance.m_dbc_monster.get_index(6,id));
		_t_monster.skill_level = int.Parse(game_data._instance.m_dbc_monster.get_index(7,id));
		m_monster_s [id] = _t_monster;
		return  _t_monster;
	}
    public s_t_resource get_t_resource(int type)
    {
        s_t_resource _resource = new s_t_resource();
        if (!game_data._instance.m_dbc_resource.has_index(type))
        {
            return null;
        }
        _resource.type = type;
        _resource.name = get_t_language(game_data._instance.m_dbc_resource.get_index(2, type));
        _resource.color = int.Parse(game_data._instance.m_dbc_resource.get_index(3, type));
        _resource.namecolor = game_data._instance.m_dbc_resource.get_index(4, type);
        _resource.icon = game_data._instance.m_dbc_resource.get_index(5, type);
        _resource.smallicon = game_data._instance.m_dbc_resource.get_index(6, type);
        _resource.desc = get_t_language(game_data._instance.m_dbc_resource.get_index(8, type));
        _resource.ch = int.Parse(game_data._instance.m_dbc_resource.get_index(9, type));
        return _resource;
    }

	public s_t_class get_t_class(int id)
	{	
		if(m_class_s.ContainsKey(id))
		{	
			return m_class_s[id];
		}

		s_t_class _t_class = new s_t_class ();
		if (!game_data._instance.m_dbc_class.has_index(id))
		{
			return null;
		}
		
		_t_class.id = int.Parse(game_data._instance.m_dbc_class.get_index(0, id));
		_t_class.job = int.Parse(game_data._instance.m_dbc_class.get_index(1, id));
		_t_class.gender = int.Parse(game_data._instance.m_dbc_class.get_index(2, id));
		_t_class.name = get_t_language( game_data._instance.m_dbc_class.get_index(4,id));
		_t_class.show = game_data._instance.m_dbc_class.get_index(5, id);
		_t_class.dress = game_data._instance.m_dbc_class.get_index(6, id);
		_t_class.icon = game_data._instance.m_dbc_class.get_index(7, id);
		_t_class.sound = game_data._instance.m_dbc_class.get_index(8, id);
		_t_class.card = get_t_language(game_data._instance.m_dbc_class.get_index(9, id));
		_t_class.color = int.Parse(game_data._instance.m_dbc_class.get_index(10, id));
		_t_class.pz =  int.Parse(game_data._instance.m_dbc_class.get_index(11, id));
		_t_class.max_glevel = int.Parse(game_data._instance.m_dbc_class.get_index(12, id));
		for (int j = 0; j < 5; ++j)
		{
			int cs = int.Parse(game_data._instance.m_dbc_class.get_index(13 + j * 4, id));
			float cscz = float.Parse(game_data._instance.m_dbc_class.get_index(14 + j * 4, id));
			float cz = float.Parse(game_data._instance.m_dbc_class.get_index(15 + j * 4, id));
			float czcz = float.Parse(game_data._instance.m_dbc_class.get_index(16 + j * 4, id));
			_t_class.cs.Add(cs);
			_t_class.cscz.Add(cscz);
			_t_class.cz.Add(cz);
			_t_class.czcz.Add(czcz);
		}
		for (int i = 0; i < 23; ++i)
		{
			int skill = int.Parse(game_data._instance.m_dbc_class.get_index(33 + i,id));
			_t_class.skills.Add(skill);
		}
        _t_class.tupo10 = int.Parse(game_data._instance.m_dbc_class.get_index(56, id));
        _t_class.tupo12 = int.Parse(game_data._instance.m_dbc_class.get_index(57, id));
        _t_class.tupo15 = int.Parse(game_data._instance.m_dbc_class.get_index(58, id));
        _t_class.exp = int.Parse(game_data._instance.m_dbc_class.get_index(59, id));
        _t_class.role_power = int.Parse(game_data._instance.m_dbc_class.get_index(60, id));
		for (int i = 0; i < 8; ++i)
		{
			int jb = int.Parse(game_data._instance.m_dbc_class.get_index(61 + i, id));
			_t_class.jbs.Add(jb);
		}
		_t_class.ycang =  int.Parse(game_data._instance.m_dbc_class.get_index(69, id));
		m_class_s [id] = _t_class;
		return _t_class;
	}

	public void parse_t_language_soft()
	{	
		m_dbc_language_soft.load_txt("t_lang_soft", -1);
		m_dbc_language.load_txt ("t_lang",-1);
		m_dbc_language_prefab.load_txt ("t_lang_prefab",-1);
        for (int i = 0; i < m_dbc_language.get_y(); ++i)
        {
            string key = m_dbc_language.get(0, i);
            string value = "";
            value = m_dbc_language.get((int)m_language + 1, i);
            if (!m_languages_s.ContainsKey(m_dbc_language.get(0, i)))
            {	
                m_languages_s.Add(key, value);
            }
        }
		for (int i = 0; i < m_dbc_language_soft.get_y(); ++i)
		{	
			string key = m_dbc_language_soft.get(0,i);
			string value = "";
			value = m_dbc_language_soft.get((int)m_language + 1,i);
			if(!m_languages_s.ContainsKey(m_dbc_language_soft.get(0,i)))			
			{	
				m_languages_s.Add(key,value);
			}
		}
		for (int i = 0; i < m_dbc_language_prefab.get_y(); ++i)
		{	
			string key = m_dbc_language_prefab.get(0,i);
			string value = "";
			value = m_dbc_language_prefab.get((int)m_language + 1,i);
			if(!m_languages_s.ContainsKey(m_dbc_language_prefab.get(0,i)))
			{	
				m_languages_s.Add(key,value);
			}
		}
	}
   
	public string get_t_language(string id)
	{	
       
        if (id.Contains("{nn}"))
        {	
            id = id.Replace("{nn}","\n");
        }
        id =  id.Trim('"');
        if (m_languages_s.Count == 0)
        {	
            parse_t_language_soft();
        }
        if (m_languages_s.ContainsKey(id))
        {	
            string value = m_languages_s[id];
            return value.Trim('"');
        }
        
        return "";
	}

    public void parse_t_mission()
    {
        m_dbc_mission.load_txt("t_mission", 0);
        foreach (int id in game_data._instance.m_dbc_mission.m_index.Keys)
        {
            s_t_mission _t_mission = new s_t_mission();
            _t_mission.id = int.Parse(game_data._instance.m_dbc_mission.get_index(0, id));
            _t_mission.name = get_t_language(game_data._instance.m_dbc_mission.get_index(2, id));
            _t_mission.des = get_t_language(game_data._instance.m_dbc_mission.get_index(3, id));
            _t_mission.type = int.Parse(game_data._instance.m_dbc_mission.get_index(4, id));
            _t_mission.map_id = int.Parse(game_data._instance.m_dbc_mission.get_index(5, id));
            _t_mission.map_name = game_data._instance.m_dbc_mission.get_index(6, id);
            _t_mission.jytype = int.Parse(game_data._instance.m_dbc_mission.get_index(7, id));

            _t_mission.lock_id = int.Parse(game_data._instance.m_dbc_mission.get_index(8, id));
            _t_mission.index_id = int.Parse(game_data._instance.m_dbc_mission.get_index(9, id));
            _t_mission.jylock_id = int.Parse(game_data._instance.m_dbc_mission.get_index(10, id));
            _t_mission.jyindex_id = int.Parse(game_data._instance.m_dbc_mission.get_index(11, id));
            _t_mission.tili = int.Parse(game_data._instance.m_dbc_mission.get_index(12, id));
            _t_mission.day_num = int.Parse(game_data._instance.m_dbc_mission.get_index(13, id));
            _t_mission.friend_id = int.Parse(game_data._instance.m_dbc_mission.get_index(14, id));
            _t_mission.friend_guan = int.Parse(game_data._instance.m_dbc_mission.get_index(15, id));

            for (int i = 0; i < 5; ++i)
            {
                int mid = int.Parse(game_data._instance.m_dbc_mission.get_index(17 + i, id));
                _t_mission.monsters.Add(mid);
            }
            for (int i = 0; i < 5; ++i)
            {
                int mid = int.Parse(game_data._instance.m_dbc_mission.get_index(23 + i, id));
                _t_mission.monsters.Add(mid);
            }

            for (int i = 0; i < 5; ++i)
            {
                s_t_mission_item t_item = new s_t_mission_item();
                t_item.reward.type = int.Parse(game_data._instance.m_dbc_mission.get_index(i * 5 + 28, id));
                t_item.reward.value1 = int.Parse(game_data._instance.m_dbc_mission.get_index(i * 5 + 29, id));
                t_item.reward.value2 = int.Parse(game_data._instance.m_dbc_mission.get_index(i * 5 + 30, id));
                t_item.reward.value3 = int.Parse(game_data._instance.m_dbc_mission.get_index(i * 5 + 31, id));
                t_item.rate = int.Parse(game_data._instance.m_dbc_mission.get_index(i * 5 + 32, id));
                if (t_item.reward.type > 0)
                {
                    _t_mission.items.Add(t_item);
                }
            }
            m_mission_s[id] = _t_mission;
        }
    }
	public s_t_mission get_t_mission(int id)
	{
		if(m_mission_s.ContainsKey(id))
		{
			return m_mission_s[id];
		}
        return null;
	}

	public s_t_mission_first_reward get_t_mission_first_reward(int id)
	{
		s_t_mission_first_reward _t_mission_first_reward = new s_t_mission_first_reward ();
		if (!game_data._instance.m_dbc_mission_first_reward.has_index(id))
		{
			return null;
		}
		_t_mission_first_reward.id = int.Parse(game_data._instance.m_dbc_mission_first_reward.get_index(0,id));
		for (int i = 0; i < 4; ++i)
		{
			s_t_reward reward = new s_t_reward();
			reward.type = int.Parse(game_data._instance.m_dbc_mission_first_reward.get_index(1 + i * 4,id));
			reward.value1 = int.Parse (game_data._instance.m_dbc_mission_first_reward.get_index (2 + i * 4, id));
			reward.value2 = int.Parse (game_data._instance.m_dbc_mission_first_reward.get_index (3 + i * 4, id));
			reward.value3 = int.Parse (game_data._instance.m_dbc_mission_first_reward.get_index (4 + i * 4, id));
			if (reward.type > 0)
			{
				_t_mission_first_reward.rewards.Add(reward);
			}
		}
		return _t_mission_first_reward;
	}

	public s_t_equip get_t_equip(int id)
	{
		if(m_equip_s.ContainsKey(id))
		{
			return m_equip_s[id];
		}
		s_t_equip _t_equip = new s_t_equip ();
		if (!game_data._instance.m_dbc_equip.has_index(id))
		{
			return null;
		}
		_t_equip.id = int.Parse(game_data._instance.m_dbc_equip.get_index(0,id));
		_t_equip.name = get_t_language(game_data._instance.m_dbc_equip.get_index(2,id));
		_t_equip.font_color = int.Parse(game_data._instance.m_dbc_equip.get_index(3,id));
		_t_equip.type = int.Parse(game_data._instance.m_dbc_equip.get_index(4,id));
		_t_equip.icon = game_data._instance.m_dbc_equip.get_index(5,id);
		_t_equip.slot_num = int.Parse(game_data._instance.m_dbc_equip.get_index(6,id));
		_t_equip.sell = int.Parse(game_data._instance.m_dbc_equip.get_index(7,id));
		_t_equip.sell_jh = int.Parse(game_data._instance.m_dbc_equip.get_index(8,id));
		_t_equip.eattr.attr = int.Parse(game_data._instance.m_dbc_equip.get_index(9,id));
		_t_equip.eattr.value = int.Parse(game_data._instance.m_dbc_equip.get_index(10,id));
		for (int j = 0; j < 2; ++j)
		{
			s_t_attr sta = new s_t_attr();
			sta.attr = int.Parse(game_data._instance.m_dbc_equip.get_index(11 + j * 2,id));
			sta.value = int.Parse(game_data._instance.m_dbc_equip.get_index(12 + j * 2,id));
			if (sta.attr > 0)
			{
				_t_equip.ejlattr.Add(sta);
			}
		}
		for (int i = 0; i < 5; ++i)
		{
			s_t_random_attr stra = new s_t_random_attr();
			stra.attr = int.Parse(game_data._instance.m_dbc_equip.get_index(15 + i * 3,id));
			stra.value1 = float.Parse(game_data._instance.m_dbc_equip.get_index(16 + i * 3,id));
			stra.value2 = float.Parse(game_data._instance.m_dbc_equip.get_index(17 + i * 3,id));
			if (stra.attr > 0)
			{
				_t_equip.eeattr.Add(stra);
			}
		}
		m_equip_s [id] = _t_equip;
		return _t_equip;
	}

	public int get_enhance(int level, int color)
	{
		if (!game_data._instance.m_dbc_enhance.has_index(level))
		{
			return 0;
		}
		return int.Parse(game_data._instance.m_dbc_enhance.get_index(color, level));
	}

	public int get_total_enhance(int level, int color)
	{
		int gold = 0;
		for (int i = 0; i <= level; ++i)
		{
			gold += int.Parse(game_data._instance.m_dbc_enhance.get_index(color, i));
		}
		return gold;
	}

	public s_t_item get_item(int id)
	{
		if(m_item_s.ContainsKey(id))
		{
			return m_item_s[id];
		}

		s_t_item _item = new s_t_item ();
		if (!game_data._instance.m_dbc_item.has_index(id))
		{
			return null;
		}
		
		_item.id = int.Parse(game_data._instance.m_dbc_item.get_index(0,id));
		_item.name = get_t_language(game_data._instance.m_dbc_item.get_index(2,id));
		_item.font_color = int.Parse(game_data._instance.m_dbc_item.get_index(3,id));
		_item.type = int.Parse(game_data._instance.m_dbc_item.get_index(4,id));
		_item.icon = game_data._instance.m_dbc_item.get_index(5,id);
		_item.desc = get_t_language(game_data._instance.m_dbc_item.get_index(7,id));
		_item.need_level = int.Parse(game_data._instance.m_dbc_item.get_index(8,id));
		_item.def_1 = int.Parse(game_data._instance.m_dbc_item.get_index(9,id));
		_item.def_2 = int.Parse(game_data._instance.m_dbc_item.get_index(10,id));
		_item.def_3 = int.Parse(game_data._instance.m_dbc_item.get_index(11,id));
		_item.def_4 = int.Parse(game_data._instance.m_dbc_item.get_index(12,id));
		_item.gold = int.Parse(game_data._instance.m_dbc_item.get_index(13,id));
		_item.use =  int.Parse(game_data._instance.m_dbc_item.get_index(14,id));
		_item.tuse =  int.Parse(game_data._instance.m_dbc_item.get_index(15,id));
		_item.jewel = int.Parse(game_data._instance.m_dbc_item.get_index(16,id));
		_item.out_put = game_data._instance.m_dbc_item.get_index (17,id);
		m_item_s [id] = _item;
		return _item;
	}
    public s_t_itemstore get_t_itemstore(int id)
    {
        s_t_itemstore _itemstore = new s_t_itemstore();
        if (!game_data._instance.m_dbc_itemstore.has_index(id))
        {
            return null;
        }
        _itemstore.id = id;
        _itemstore.type = int.Parse(m_dbc_itemstore.get_index(2,id));
        int k = 0;
        while (true)
        {
            if (int.Parse(game_data._instance.m_dbc_itemstore.get_index(3 + 5 * k,id)) != 0)
            {
                s_t_reward _reward = new s_t_reward();
                _reward.type = int.Parse(game_data._instance.m_dbc_itemstore.get_index(3 + 5 * k, id));
                _reward.value1 = int.Parse(game_data._instance.m_dbc_itemstore.get_index(4 + 5 * k, id));
                _reward.value2 = int.Parse(game_data._instance.m_dbc_itemstore.get_index(5 + 5 * k, id));
                _reward.value3 = int.Parse(game_data._instance.m_dbc_itemstore.get_index(6 + 5 * k, id));
                _itemstore.rewards.Add(_reward);
                k++;
            }
            else
            {
                break;
            }
        }
        return _itemstore;
    }
	public s_t_gaizao get_gaizao(int id)
	{
		s_t_gaizao _t_gaizao = new s_t_gaizao ();
		if (!game_data._instance.m_dbc_gaizao.has_index(id))
		{
			return null;
		}
		_t_gaizao.color = int.Parse(game_data._instance.m_dbc_gaizao.get_index(0,id));
		_t_gaizao.item_id = int.Parse(game_data._instance.m_dbc_gaizao.get_index(1,id));
		_t_gaizao.m_0 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(2,id));
		_t_gaizao.m_1 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(3,id));
		_t_gaizao.m_2 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(4,id));
		_t_gaizao.m_3 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(5,id));
		_t_gaizao.m_4 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(6,id));
		_t_gaizao.max_count = int.Parse(game_data._instance.m_dbc_gaizao.get_index(7,id));
		_t_gaizao.rate_1 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(8,id));
		_t_gaizao.rate_2 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(9,id));
		_t_gaizao.rate_3 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(10,id));
		_t_gaizao.rate_4 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(11,id));
		_t_gaizao.rate_5 = int.Parse(game_data._instance.m_dbc_gaizao.get_index(12,id));
		return _t_gaizao;
	}

	public s_t_role_shop get_role_shop(int id)
	{
		s_t_role_shop _t_role_shop = new s_t_role_shop ();
		if (!game_data._instance.m_dbc_role_shop.has_index(id))
		{
			return null;
		}
		_t_role_shop.id = int.Parse(m_dbc_role_shop.get_index(0,id));
		_t_role_shop.name = m_dbc_role_shop.get_index(1,id);
		_t_role_shop.grid = int.Parse(m_dbc_role_shop.get_index(2,id));
		_t_role_shop.level = int.Parse(m_dbc_role_shop.get_index(3,id));
		_t_role_shop.sell_type = int.Parse(m_dbc_role_shop.get_index(4,id));
		_t_role_shop.sell_value_0 = int.Parse(m_dbc_role_shop.get_index(5,id));
		_t_role_shop.sell_value_1 = int.Parse(m_dbc_role_shop.get_index(6,id));
		_t_role_shop.sell_value_2 = int.Parse(m_dbc_role_shop.get_index(7,id));
		_t_role_shop.weight = int.Parse(m_dbc_role_shop.get_index(8,id));
		_t_role_shop.money_type = int.Parse(m_dbc_role_shop.get_index(9,id));
		_t_role_shop.price = int.Parse(m_dbc_role_shop.get_index(10,id));
		_t_role_shop.rec = int.Parse(m_dbc_role_shop.get_index(11,id));
		return _t_role_shop;
	}

	public s_t_shop_xg get_shop_xg(int id)
	{
		s_t_shop_xg _t_shop_xg = new s_t_shop_xg ();
		if (!game_data._instance.m_dbc_shop_xg.has_index(id))
		{
			return null;
		}
		_t_shop_xg.id = int.Parse(m_dbc_shop_xg.get_index(0, id));
        _t_shop_xg.shop_type = int.Parse(m_dbc_shop_xg.get_index(1, id));
        _t_shop_xg.jewel = int.Parse(m_dbc_shop_xg.get_index(2, id));
        _t_shop_xg.type = int.Parse(m_dbc_shop_xg.get_index(3, id));
		_t_shop_xg.vlaue1 = int.Parse(m_dbc_shop_xg.get_index(4, id));
		_t_shop_xg.vlaue2 = int.Parse(m_dbc_shop_xg.get_index(5, id));
		_t_shop_xg.vlaue3 = int.Parse(m_dbc_shop_xg.get_index(6, id));
		_t_shop_xg.name = m_dbc_shop_xg.get_index(7, id);
		_t_shop_xg.level = int.Parse(m_dbc_shop_xg.get_index(8, id));
		_t_shop_xg.price_type = int.Parse(m_dbc_shop_xg.get_index(9, id));
		_t_shop_xg.price = int.Parse(m_dbc_shop_xg.get_index(10, id));
		_t_shop_xg.xg_type = int.Parse(m_dbc_shop_xg.get_index(11, id));
		_t_shop_xg.xg_num = int.Parse(m_dbc_shop_xg.get_index(12, id));
        for (int i = 0; i < 17; ++i)
        {
			int vip = int.Parse (m_dbc_shop_xg.get_index (12 + i, id));
			_t_shop_xg.vip_type.Add(vip);
		}
		return _t_shop_xg;
	}
    void parse_t_target()
    {
        m_dbc_target.load_txt("t_target", 0);
        foreach (int id in m_dbc_target.m_index.Keys)
        {
            s_t_target _t_target = new s_t_target();
            _t_target.id = int.Parse(game_data._instance.m_dbc_target.get_index(0, id));
            _t_target.desc = get_t_language(game_data._instance.m_dbc_target.get_index(2, id));
            _t_target.icon = game_data._instance.m_dbc_target.get_index(3, id);
            _t_target.type = int.Parse(game_data._instance.m_dbc_target.get_index(4, id));
            _t_target.pid = int.Parse(game_data._instance.m_dbc_target.get_index(5, id));
            _t_target.tjtype = int.Parse(game_data._instance.m_dbc_target.get_index(6, id));
            _t_target.tjnum = int.Parse(game_data._instance.m_dbc_target.get_index(7, id));
            _t_target.tjdef1 = int.Parse(game_data._instance.m_dbc_target.get_index(8, id));
            _t_target.tjdef2 = int.Parse(game_data._instance.m_dbc_target.get_index(9, id));
            _t_target.reward.type = int.Parse(game_data._instance.m_dbc_target.get_index(10, id));
            _t_target.reward.value1 = int.Parse(game_data._instance.m_dbc_target.get_index(11, id));
            _t_target.reward.value2 = int.Parse(game_data._instance.m_dbc_target.get_index(12, id));
            _t_target.reward.value3 = int.Parse(game_data._instance.m_dbc_target.get_index(13, id));
            m_target_s[id] = _t_target;
        }
    }
	public s_t_target get_t_target(int id)
	{
		if(m_target_s.ContainsKey(id))
		{
			return m_target_s[id];
		}
		return null;
	}

    public s_t_guild_horreward get_t_guild_horreward(int id)
    {
        s_t_guild_horreward _t_guild_horreward = new s_t_guild_horreward();
        if (!game_data._instance.m_dbc_horreward.has_index(id))
        {
            return null;
        }
        _t_guild_horreward.id = int.Parse(game_data._instance.m_dbc_horreward.get_index(0, id));
        _t_guild_horreward.value = int.Parse(game_data._instance.m_dbc_horreward.get_index(1, id));
        _t_guild_horreward.reward = new s_t_reward();
        _t_guild_horreward.reward.type = int.Parse(game_data._instance.m_dbc_horreward.get_index(2 , id));
        _t_guild_horreward.reward.value1 = int.Parse(game_data._instance.m_dbc_horreward.get_index(3, id));
        _t_guild_horreward.reward.value2 = int.Parse(game_data._instance.m_dbc_horreward.get_index(4, id));
        _t_guild_horreward.reward.value3 = int.Parse(game_data._instance.m_dbc_horreward.get_index(5, id));
        _t_guild_horreward.speed_add = int.Parse(game_data._instance.m_dbc_horreward.get_index(6, id));
        _t_guild_horreward.value2_add = int.Parse(game_data._instance.m_dbc_horreward.get_index(7, id));
        return _t_guild_horreward;
    }
	public dbc get_dbc_active()
	{
		return m_dbc_active;
	}
	public dbc get_dbc_active_reward()
	{
		return m_dbc_active_reward;
	}
	public dbc get_dbc_map()
	{
		return m_dbc_map;
	}
	public dbc get_dbc_shop_xg()
	{
		return m_dbc_shop_xg;
	}
	public dbc get_dbc_role_dress()
	{
		return m_dbc_role_dress;
	}
	public s_t_map get_t_map(int id)
	{
		if(m_map_s.ContainsKey(id))
		{
			return m_map_s[id];
		}
		s_t_map _t_map = new s_t_map ();
		if (!game_data._instance.m_dbc_map.has_index(id))
		{
			return null;
		}
		_t_map.id = int.Parse(game_data._instance.m_dbc_map.get_index(0,id));
		_t_map.name = get_t_language(game_data._instance.m_dbc_map.get_index(2,id));
		_t_map.boss_name = get_t_language(game_data._instance.m_dbc_map.get_index(4,id));
		_t_map.res = game_data._instance.m_dbc_map.get_index(5,id);
		_t_map.pid = int.Parse(game_data._instance.m_dbc_map.get_index(6,id));
		_t_map.jypid = int.Parse(game_data._instance.m_dbc_map.get_index(7,id));
		_t_map.role_id = int.Parse(game_data._instance.m_dbc_map.get_index(8,id));
		_t_map.image_text = get_t_language(game_data._instance.m_dbc_map.get_index(9,id));
		for (int i = 0; i < 3; ++i)
		{
			s_t_map_star t_map_star = new s_t_map_star();
			t_map_star.star_num = int.Parse(game_data._instance.m_dbc_map.get_index(10 + i * 17,id));
			for (int j = 0; j < 4; ++j)
			{
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = int.Parse(game_data._instance.m_dbc_map.get_index(11 + i * 17 + j * 4,id));
				t_reward.value1 = int.Parse(game_data._instance.m_dbc_map.get_index(12 + i * 17 + j * 4,id));
				t_reward.value2 = int.Parse(game_data._instance.m_dbc_map.get_index(13 + i * 17 + j * 4,id));
				t_reward.value3 = int.Parse(game_data._instance.m_dbc_map.get_index(14 + i * 17 + j * 4,id));
				t_map_star.rewards.Add(t_reward);
			}
			_t_map.stars.Add(t_map_star);
		}
		m_map_s [id] = _t_map;
		return _t_map;
	}
    public s_t_qiyu_tiaozhan get_t_qiyu_tiaozhan(int id)
    {
        if (!m_dbc_qiyu_tiaozhan.has_index(id))
        {
            return null;
        }
        s_t_qiyu_tiaozhan _qiyu_tiaozhan = new s_t_qiyu_tiaozhan();
        _qiyu_tiaozhan.id = id;
        _qiyu_tiaozhan.zhuangzhi = int.Parse(m_dbc_qiyu_tiaozhan.get_index(1, id));
        _qiyu_tiaozhan.tili = int.Parse(m_dbc_qiyu_tiaozhan.get_index(2, id));
        _qiyu_tiaozhan.desc = get_t_language(m_dbc_qiyu_tiaozhan.get_index(4,id));
        return _qiyu_tiaozhan;
    }
	
	public s_t_active get_t_active(int id)
	{
		if(m_active_s.ContainsKey(id))
		{
			return m_active_s[id];
		}
		s_t_active _t_active = new s_t_active ();
		if (!game_data._instance.m_dbc_active.has_index(id))
		{
			return null;
		}
		_t_active.id = int.Parse(game_data._instance.m_dbc_active.get_index(0,id));
		_t_active.name = get_t_language(game_data._instance.m_dbc_active.get_index(2,id));
		_t_active.desc = get_t_language(game_data._instance.m_dbc_active.get_index(3,id));
		_t_active.icon = game_data._instance.m_dbc_active.get_index(4,id);
		_t_active.level = int.Parse(game_data._instance.m_dbc_active.get_index(5,id));
		_t_active.num = int.Parse(game_data._instance.m_dbc_active.get_index(6,id));
		_t_active.reward.type = int.Parse(game_data._instance.m_dbc_active.get_index(7,id));
		_t_active.reward.value1 = int.Parse(game_data._instance.m_dbc_active.get_index(8,id));
		_t_active.reward.value2 = int.Parse(game_data._instance.m_dbc_active.get_index(9,id));
		_t_active.reward.value3 = int.Parse(game_data._instance.m_dbc_active.get_index(10,id));
		_t_active.score = int.Parse(game_data._instance.m_dbc_active.get_index(11,id));
		m_active_s [id] = _t_active;
		return _t_active;
	}
	public s_t_vip get_t_vip(int level)
	{
		if(m_vip_s.ContainsKey(level))
		{
			return m_vip_s[level];
		}
		s_t_vip _t_vip = new s_t_vip ();
		if (!game_data._instance.m_dbc_vip.has_index(level))
		{
			return null;
		}
		_t_vip.level = int.Parse(game_data._instance.m_dbc_vip.get_index(0,level));
		_t_vip.recharge = int.Parse(game_data._instance.m_dbc_vip.get_index(1,level));
		_t_vip.desc = game_data._instance.m_dbc_vip.get_index(2,level);
		_t_vip.desc1 = get_t_language(game_data._instance.m_dbc_vip.get_index(4,level));
		_t_vip.yjewel = int.Parse(game_data._instance.m_dbc_vip.get_index(5,level));
		_t_vip.jewel = int.Parse(game_data._instance.m_dbc_vip.get_index(6,level));
		for (int i = 0; i < 6; ++i)
		{
			s_t_reward reward = new s_t_reward();
			reward.type = int.Parse(game_data._instance.m_dbc_vip.get_index(7 + i * 4,level));
			reward.value1 = int.Parse (game_data._instance.m_dbc_vip.get_index (8 + i * 4, level));
			reward.value2 = int.Parse (game_data._instance.m_dbc_vip.get_index (9 + i * 4, level));
			reward.value3 = int.Parse (game_data._instance.m_dbc_vip.get_index (10 + i * 4, level));
			if (reward.type > 0)
			{
				_t_vip.rewards.Add(reward);
			}
		}
		_t_vip.half = game_data._instance.m_dbc_vip.get_index (31, level);
		_t_vip.add_tili = int.Parse(game_data._instance.m_dbc_vip.get_index(32,level));
		_t_vip.dj_num = int.Parse(game_data._instance.m_dbc_vip.get_index(33,level));
		_t_vip.jy_buy_num = int.Parse(game_data._instance.m_dbc_vip.get_index(34,level));
		_t_vip.ttt_cz_num = int.Parse(game_data._instance.m_dbc_vip.get_index(35,level));
        _t_vip.guild_attack_num = int.Parse(game_data._instance.m_dbc_vip.get_index(36,level));
		_t_vip.refresh_shop_num = int.Parse(game_data._instance.m_dbc_vip.get_index(37,level));
		_t_vip.hunter_assembly = int.Parse(game_data._instance.m_dbc_vip.get_index(38,level));
        _t_vip.huiyi_zhanbu_num = int.Parse(game_data._instance.m_dbc_vip.get_index(39, level));
        _t_vip.master_buy_num = int.Parse(game_data._instance.m_dbc_vip.get_index(40, level));
        _t_vip.guildpvpbuy_num = int.Parse(game_data._instance.m_dbc_vip.get_index(41, level));
		m_vip_s [level] = _t_vip;
		return _t_vip;
	}

	public s_t_recharge get_t_recharge(int id)
	{	
		s_t_recharge _t_recharge = new s_t_recharge ();
		if (!game_data._instance.m_dbc_recharge.has_index(id))
		{	
			return null;
		}
		_t_recharge.id = int.Parse(game_data._instance.m_dbc_recharge.get_index(0,id));
		_t_recharge.name = get_t_language(game_data._instance.m_dbc_recharge.get_index(2,id));
		_t_recharge.desc = get_t_language(game_data._instance.m_dbc_recharge.get_index(3,id));
		_t_recharge.icon = game_data._instance.m_dbc_recharge.get_index(4,id);
		_t_recharge.type = int.Parse(game_data._instance.m_dbc_recharge.get_index(5,id));
		_t_recharge.pid = int.Parse(game_data._instance.m_dbc_recharge.get_index(6,id));
		_t_recharge.vippt = int.Parse(game_data._instance.m_dbc_recharge.get_index(7,id));
		_t_recharge.jewel = int.Parse(game_data._instance.m_dbc_recharge.get_index(8,id));
        _t_recharge.rmb = get_t_language(game_data._instance.m_dbc_recharge.get_index(9, id));
        _t_recharge.ios_id = int.Parse(game_data._instance.m_dbc_recharge.get_index(10, id));
        return _t_recharge;
	}

	public s_t_price get_t_price(int num)
	{
		if (num > game_data._instance.m_dbc_price.get_y())
		{
			num = game_data._instance.m_dbc_price.get_y();
		}
		s_t_price _t_price = new s_t_price ();
		if (!game_data._instance.m_dbc_price.has_index(num))
		{
			return null;
		}
		
		_t_price.num = int.Parse(game_data._instance.m_dbc_price.get_index(0,num));
		_t_price.dj = int.Parse(game_data._instance.m_dbc_price.get_index(1,num));
		_t_price.kc = int.Parse(game_data._instance.m_dbc_price.get_index(2,num));
		_t_price.jy = int.Parse(game_data._instance.m_dbc_price.get_index(3,num));
		_t_price.ttt_cz = int.Parse(game_data._instance.m_dbc_price.get_index(4,num));
		_t_price.hbb_refresh = int.Parse(game_data._instance.m_dbc_price.get_index(5,num));
		_t_price.guild_attack_buy = int.Parse(game_data._instance.m_dbc_price.get_index(6,num));
		_t_price.yuanli_potion = int.Parse(game_data._instance.m_dbc_price.get_index(7,num));
		_t_price.tili_potion = int.Parse(game_data._instance.m_dbc_price.get_index(8,num));
		_t_price.energy_potion = int.Parse(game_data._instance.m_dbc_price.get_index(9,num));
		_t_price.jinxiang_equip = int.Parse(game_data._instance.m_dbc_price.get_index(10,num));
		_t_price.jinxiang_treasure = int.Parse (game_data._instance.m_dbc_price.get_index(11,num));
		_t_price.mowang_invitation = int.Parse (game_data._instance.m_dbc_price.get_index(12,num));
		_t_price.change_name = int.Parse (game_data._instance.m_dbc_price.get_index(13,num));
		_t_price.hunter_assembly = int.Parse (game_data._instance.m_dbc_price.get_index(14,num));
        _t_price.bingyuan_reward = int.Parse(game_data._instance.m_dbc_price.get_index(15, num));
        _t_price.tanbao_price = int.Parse (game_data._instance.m_dbc_price.get_index(16,num));
		_t_price.luck_zhuanpan = int.Parse (game_data._instance.m_dbc_price.get_index(17,num));
		_t_price.haohua_zhuanpan = int.Parse (game_data._instance.m_dbc_price.get_index(18,num));
		_t_price.manyou = int.Parse (game_data._instance.m_dbc_price.get_index(19,num));
        _t_price.master = int.Parse(game_data._instance.m_dbc_price.get_index(20, num));
        _t_price.mofang = int.Parse(m_dbc_price.get_index(21, num));
        _t_price.guildpvpbuy = int.Parse(m_dbc_price.get_index(22, num));
		return _t_price;
	}

	public s_t_online_reward get_t_online_reward(int index)
	{
		if(m_online_rewards.ContainsKey(index))
		{
			return m_online_rewards[index];
		}
		s_t_online_reward _t_online_rewrad = new s_t_online_reward ();
		if (index < 0 || index >= m_dbc_online_reward.get_y())
		{
			return null;
		}
		_t_online_rewrad.name = get_t_language(game_data._instance.m_dbc_online_reward.get(2,index));
		_t_online_rewrad.time = int.Parse(game_data._instance.m_dbc_online_reward.get(3,index));
		for (int i = 0; i < 3; ++i)
		{
			s_t_reward t_reward = new s_t_reward();
			t_reward.type = int.Parse(game_data._instance.m_dbc_online_reward.get(4 + i * 4,index));
			t_reward.value1 = int.Parse(game_data._instance.m_dbc_online_reward.get(5 + i * 4,index));
			t_reward.value2 = int.Parse(game_data._instance.m_dbc_online_reward.get(6 + i * 4,index));
			t_reward.value3 = int.Parse(game_data._instance.m_dbc_online_reward.get(7 + i * 4,index));
			if (t_reward.type != 0)
			{
				_t_online_rewrad.rewards.Add(t_reward);
			}
		}
		m_online_rewards[index] = _t_online_rewrad;
		return _t_online_rewrad;
	}

	public s_t_active_reward get_t_active_reward(int id)
	{
		s_t_active_reward _t_active_reward = new s_t_active_reward ();
		if (!game_data._instance.m_dbc_active_reward.has_index(id))
		{
			return null;
		}
		_t_active_reward.id= int.Parse(game_data._instance.m_dbc_active_reward.get_index(0,id));
		_t_active_reward.score= int.Parse(game_data._instance.m_dbc_active_reward.get_index(1,id));
		for(int i = 0;i < 4;i++)
		{
			s_t_reward _reward = new s_t_reward();
			_reward.type = int.Parse(game_data._instance.m_dbc_active_reward.get_index(2 + i * 4,id));
			_reward.value1 = int.Parse(game_data._instance.m_dbc_active_reward.get_index(3 + i * 4,id));
			_reward.value2 = int.Parse(game_data._instance.m_dbc_active_reward.get_index(4 + i * 4,id));
			_reward.value3 = int.Parse(game_data._instance.m_dbc_active_reward.get_index(5 + i * 4,id));
			if(_reward.type != 0)
			{
				_t_active_reward.reward.Add(_reward);
			}
		}
		return _t_active_reward;
	}
	public s_t_daily_sign get_t_daily_sign(int index)
	{
		s_t_daily_sign _t_daily_sign = new s_t_daily_sign ();
		if (!game_data._instance.m_dbc_daily_sign.has_index(index))
		{
			return null;
		}
		_t_daily_sign.index = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(0,index));
		_t_daily_sign.type = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(1,index));
		_t_daily_sign.value1 = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(2,index));
		_t_daily_sign.value2 = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(3,index));
		_t_daily_sign.value3 = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(4,index));
		_t_daily_sign.vip = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(5,index));
        _t_daily_sign.type1 = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(6, index));
        _t_daily_sign.value11 = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(7, index));
        _t_daily_sign.value21 = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(8, index));
        _t_daily_sign.value31 = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(9, index));
        _t_daily_sign.vip1 = int.Parse(game_data._instance.m_dbc_daily_sign.get_index(10, index));
		return _t_daily_sign;
	}

	public s_t_tupo get_tupo(int id)
	{
		s_t_tupo _t_tupo = new s_t_tupo ();
		if (!game_data._instance.m_dbc_tupo.has_index(id))
		{
			return null;
		}
		_t_tupo.level = int.Parse(game_data._instance.m_dbc_tupo.get_index(0,id));
		_t_tupo.role_level = int.Parse(game_data._instance.m_dbc_tupo.get_index(1,id));
		for(int i = 0; i < 4;++i)
		{
			int sp = int.Parse(game_data._instance.m_dbc_tupo.get_index(2 + i,id));
			_t_tupo.sps.Add(sp);
		}
		_t_tupo.cl_gold = int.Parse(game_data._instance.m_dbc_tupo.get_index(6,id));
		return _t_tupo;
	}

	public s_t_jinjie get_jinjie(int id)
	{
		if(m_jinjies.ContainsKey(id))
		{
			return m_jinjies[id];
		}
		s_t_jinjie _t_jinjie = new s_t_jinjie ();
		if (!game_data._instance.m_dbc_jinjie.has_index(id))
		{
			return null;
		}
		_t_jinjie.id = int.Parse(game_data._instance.m_dbc_jinjie.get_index(0,id));
		_t_jinjie.name = get_t_language(game_data._instance.m_dbc_jinjie.get_index(2,id));
		_t_jinjie.level = int.Parse(game_data._instance.m_dbc_jinjie.get_index(3,id));
		_t_jinjie.clty = int.Parse(game_data._instance.m_dbc_jinjie.get_index(4,id));
		_t_jinjie.clty_num = int.Parse(game_data._instance.m_dbc_jinjie.get_index(5,id));
		_t_jinjie.clfy = int.Parse(game_data._instance.m_dbc_jinjie.get_index(6,id));
		_t_jinjie.clfy_num = int.Parse(game_data._instance.m_dbc_jinjie.get_index(7,id));
		_t_jinjie.clfy_num1 = int.Parse(game_data._instance.m_dbc_jinjie.get_index(8,id));
		_t_jinjie.clgj = int.Parse(game_data._instance.m_dbc_jinjie.get_index(9,id));
		_t_jinjie.clgj_num = int.Parse(game_data._instance.m_dbc_jinjie.get_index(10,id));
		_t_jinjie.clgj_num1 = int.Parse(game_data._instance.m_dbc_jinjie.get_index(11,id));
		_t_jinjie.clmf = int.Parse(game_data._instance.m_dbc_jinjie.get_index(12,id));
		_t_jinjie.clmf_num = int.Parse(game_data._instance.m_dbc_jinjie.get_index(13,id));
		_t_jinjie.clmf_num1 = int.Parse(game_data._instance.m_dbc_jinjie.get_index(14,id));
		_t_jinjie.sxper = float.Parse(game_data._instance.m_dbc_jinjie.get_index(15,id));
		_t_jinjie.gold = int.Parse(game_data._instance.m_dbc_jinjie.get_index(16,id));
		_t_jinjie.point = int.Parse(game_data._instance.m_dbc_jinjie.get_index(17,id));
		_t_jinjie.icon = game_data._instance.m_dbc_jinjie.get_index(18,id);
		m_jinjies [id] = _t_jinjie;
		return _t_jinjie;
	}

	public float get_jinjie_point(int id)
	{
		float val = 0;
		for (int i = 0; i <= id; ++i)
		{
			s_t_jinjie t_jinjie = get_jinjie(i);
			if (t_jinjie != null)
			{
				val += t_jinjie.point;
			}
		}
		return val;
	}
	public s_t_skillup get_skillup(int level)
	{
		s_t_skillup _t_skillup = new s_t_skillup ();
		if (!game_data._instance.m_dbc_skillup.has_index(level))
		{
			return null;
		}
		_t_skillup.level = int.Parse(game_data._instance.m_dbc_skillup.get_index(0,level));
		_t_skillup.gold = int.Parse(game_data._instance.m_dbc_skillup.get_index(2,level));
		_t_skillup.skillstone = int.Parse(game_data._instance.m_dbc_skillup.get_index(1,level));
		return _t_skillup;
	}
	public s_t_guild get_guild(int level)
	{
        if (m_guilds.ContainsKey(level))
		{
            return m_guilds[level];
        }
		return null;
	}
    void parse_guild()
    {
        m_dbc_guild.load_txt("t_guild", 0);
        foreach (int level in m_dbc_guild.m_index.Keys)
        {
            s_t_guild _t_guild = new s_t_guild();
		    _t_guild.level = int .Parse (game_data._instance.m_dbc_guild.get_index(0,level));
		    _t_guild.exp = int .Parse (game_data._instance.m_dbc_guild.get_index (1, level));
		    _t_guild.number_count = int.Parse (game_data._instance.m_dbc_guild.get_index (2, level));
            for (int i = 0; i < 5; i++)
            {
                _t_guild.reward_nums.Add(int.Parse(game_data._instance.m_dbc_guild.get_index(3 + i, level)));
            }
            m_guilds[level] = _t_guild;
	    }
    }
    void parse_guildfight_target()
    {
        m_dbc_guildfight_target.load_txt("t_guildfight_target",0);
        foreach (int id in m_dbc_guildfight_target.m_index.Keys)
        {
            s_t_guildfight_target target = new s_t_guildfight_target();
            target.id = id;
            target.name = game_data._instance.get_t_language(m_dbc_guildfight_target.get_index(2, id));
            target.type = int.Parse(m_dbc_guildfight_target.get_index(3, id));
            target.num = int.Parse(m_dbc_guildfight_target.get_index(4, id));
            target.reward.type  = int.Parse(m_dbc_guildfight_target.get_index(5, id));
            target.reward.value1 = int.Parse(m_dbc_guildfight_target.get_index(6, id));
            target.reward.value2 = int.Parse(m_dbc_guildfight_target.get_index(7, id));
            target.reward.value3 = int.Parse(m_dbc_guildfight_target.get_index(8, id));
            m_guildfight_targets.Add(id,target);
        }
    }
    public s_t_guildfight_target get_t_guildfight_target(int id)
    {
        if (m_guildfight_targets.ContainsKey(id))
        {
            return m_guildfight_targets[id];
        }
        return null;
    }
    void parse_guildfight()
    {
        m_dbc_guildfight.load_txt("t_guildfight", 0);
        foreach (int id in m_dbc_guildfight.m_index.Keys)
        {
            s_t_guildfight _t_fight = new s_t_guildfight();
            _t_fight.id = id;
            _t_fight.color = int.Parse(m_dbc_guildfight.get_index(3,id));
            _t_fight.name = game_data._instance.get_name_color(_t_fight.color) + game_data._instance.get_t_language(m_dbc_guildfight.get_index(2,id));
            _t_fight.defendrolenum = int.Parse(m_dbc_guildfight.get_index(4,id));
            _t_fight.chengfangvalue = int.Parse(m_dbc_guildfight.get_index(5, id));
            _t_fight.exp = int.Parse(m_dbc_guildfight.get_index(6, id));
            _t_fight.suc_chengfangvalue = int.Parse(m_dbc_guildfight.get_index(7, id));
            _t_fight.fail_chengfangvalue = int.Parse(m_dbc_guildfight.get_index(8, id));
            _t_fight.suc_con = int.Parse(m_dbc_guildfight.get_index(9, id));
            _t_fight.fail_con = int.Parse(m_dbc_guildfight.get_index(10, id));
            _t_fight.zhanjizhanli = int.Parse(m_dbc_guildfight.get_index(11, id));
            _t_fight.defendnum = int.Parse(m_dbc_guildfight.get_index(12, id));
            m_guildfights[id] = _t_fight;
        }
    }
    public s_t_guildfight get_guild_fight(int id)
    {
        if (m_guildfights.ContainsKey(id))
        {
            return m_guildfights[id];
 
        }
        return null;
    }
	public s_t_guild_icon get_guild_icon(int id)
	{
		s_t_guild_icon _t_guild_icon = new s_t_guild_icon ();
		if (!game_data._instance.m_dbc_guild_icon.has_index (id)) {
			return null;
		}

		_t_guild_icon.id = int.Parse (game_data._instance.m_dbc_guild_icon.get_index (0, id));
		_t_guild_icon.icon = game_data._instance.m_dbc_guild_icon.get_index (1, id);
		return _t_guild_icon;
	}
	public s_t_guild_shop get_guild_shop(int id)
	{
		s_t_guild_shop _t_guild_shop = new s_t_guild_shop ();
		if (!game_data._instance.m_dbc_guild_shop.has_index(id))
		{
			return null;
		}
		_t_guild_shop.id = int.Parse(game_data._instance.m_dbc_guild_shop.get_index(0,id));
		_t_guild_shop.name = game_data._instance.m_dbc_guild_shop.get_index (1, id);
		_t_guild_shop.reward.type = int.Parse(game_data._instance.m_dbc_guild_shop.get_index(2,id));
		_t_guild_shop.reward.value1 = int.Parse(game_data._instance.m_dbc_guild_shop.get_index(3,id));
		_t_guild_shop.reward.value2 = int.Parse(game_data._instance.m_dbc_guild_shop.get_index(4,id));
		_t_guild_shop.reward.value3 = int.Parse(game_data._instance.m_dbc_guild_shop.get_index(5,id));
		_t_guild_shop.gx = int.Parse(game_data._instance.m_dbc_guild_shop.get_index(6,id));
		_t_guild_shop.hb_power = int.Parse(game_data._instance.m_dbc_guild_shop.get_index(7,id));
		_t_guild_shop.num = int.Parse(game_data._instance.m_dbc_guild_shop.get_index(8,id));
		_t_guild_shop.level = int.Parse(game_data._instance. m_dbc_guild_shop.get_index(9,id));
		return _t_guild_shop;
	}
    void parse_t_guild_shop_ex()
    {
        m_dbc_guild_guanghuan.load_txt("t_guild_shop_ex", 0);
        foreach (int id in m_dbc_guild_guanghuan.m_index.Keys)
        {
            s_t_guild_shop_ex _shop = new s_t_guild_shop_ex();
            _shop.id = id;
            _shop.name = get_t_language(game_data._instance.m_dbc_guild_guanghuan.get_index(2, id));
            _shop.type = int.Parse(game_data._instance.m_dbc_guild_guanghuan.get_index(3, id));
            _shop.value1 = int.Parse(game_data._instance.m_dbc_guild_guanghuan.get_index(4, id));
            _shop.value2 = int.Parse(game_data._instance.m_dbc_guild_guanghuan.get_index(5, id));
            _shop.value3 = int.Parse(game_data._instance.m_dbc_guild_guanghuan.get_index(6, id));
            _shop.gongxian = int.Parse(game_data._instance.m_dbc_guild_guanghuan.get_index(7, id));
            _shop.jewel = int.Parse(game_data._instance.m_dbc_guild_guanghuan.get_index(8, id));
            _shop.level = int.Parse(game_data._instance.m_dbc_guild_guanghuan.get_index(9, id));
            m_guild_guangguans.Add(id, _shop);
        }
    }
   
    public s_t_guild_shop_ex get_guild_shop_ex(int id)
    {
        if (m_guild_guangguans.ContainsKey(id))
        {
            return m_guild_guangguans[id];
        }
        return null;
    }
    void parse_t_master_duanwei()
    {
        m_dbc_master_duanwei.load_txt("t_master_duanwei", 0);
        foreach (int id in m_dbc_master_duanwei.m_index.Keys)
        {
            s_t_master_duanwei _duanwei = new s_t_master_duanwei();
            _duanwei.id = id;
            _duanwei.duanwei = get_t_language(m_dbc_master_duanwei.get_index(2, id));
            _duanwei.icon = m_dbc_master_duanwei.get_index(3,id);
            _duanwei.staricon = m_dbc_master_duanwei.get_index(4, id);
            _duanwei.starcount = int.Parse(m_dbc_master_duanwei.get_index(5, id));
            _duanwei.kuang = m_dbc_master_duanwei.get_index(6, id);
            _duanwei.topcolor = m_dbc_master_duanwei.get_index(7, id);
            _duanwei.bottomcolor = m_dbc_master_duanwei.get_index(8, id);
            _duanwei.need_rank = int.Parse(m_dbc_master_duanwei.get_index(10, id));
            _duanwei.need_jifen = int.Parse(m_dbc_master_duanwei.get_index(9, id));
            _duanwei.attr1 = int.Parse(m_dbc_master_duanwei.get_index(11, id));
            _duanwei.value1 = double.Parse(m_dbc_master_duanwei.get_index(12, id));
            _duanwei.attr2 = int.Parse(m_dbc_master_duanwei.get_index(13, id));
            _duanwei.value2 = double.Parse(m_dbc_master_duanwei.get_index(14, id));
            m_master_duanweis.Add(id, _duanwei);
        }
    }
    public s_t_master_duanwei get_t_master_duanwei(int id)
    {
        if (m_master_duanweis.ContainsKey(id))
        {
            return m_master_duanweis[id];
        }
        return null;
    }
	void parse_t_yueka_jijin()
	{
		m_dbc_yueka_jijin.load_txt ("t_yueka", 0);
		foreach (int day in m_dbc_yueka_jijin.m_index.Keys) 
		{
			s_t_yueka yuekajj =new s_t_yueka();
			yuekajj.day = int.Parse(m_dbc_yueka_jijin.get_index(0,day));
			yuekajj.type1 =int.Parse(m_dbc_yueka_jijin.get_index(1,day));
			yuekajj.value_1_1 =int.Parse(m_dbc_yueka_jijin.get_index(2,day));
			yuekajj.value_1_2 =int.Parse(m_dbc_yueka_jijin.get_index(3,day));
			yuekajj.value_1_3 =int.Parse(m_dbc_yueka_jijin.get_index(4,day));
			yuekajj.type2 =int.Parse(m_dbc_yueka_jijin.get_index(5,day));
			yuekajj.value_2_1 =int.Parse(m_dbc_yueka_jijin.get_index(6,day));
			yuekajj.value_2_2 =int.Parse(m_dbc_yueka_jijin.get_index(7,day));
			yuekajj.value_2_3 =int.Parse(m_dbc_yueka_jijin.get_index(8,day));
			m_yueka_s.Add(day,yuekajj);
		}
	}
	public s_t_yueka get_t_yueka_jijin(int id)
	{
		if(m_yueka_s.ContainsKey(id))
		{
			return m_yueka_s[id];
		}
		return null;
	}
	void parse_t_master_target()
	{
		m_dbc_master_target.load_txt("t_master_target", 0);
        foreach (int id in m_dbc_master_target.m_index.Keys)
        {
            s_t_master_target target = new s_t_master_target();
            target.id = id;
            target.name = get_t_language(m_dbc_master_target.get_index(2, id));
            target.count = int.Parse(m_dbc_master_target.get_index(3,id));
            target.type = int.Parse(m_dbc_master_target.get_index(4, id));
            target.value1 = int.Parse(m_dbc_master_target.get_index(5, id));
            target.value2 = int.Parse(m_dbc_master_target.get_index(6, id));
            target.value3 = int.Parse(m_dbc_master_target.get_index(7, id));
            m_master_targets.Add(id, target);
        }
    }
    public s_t_master_target get_t_master_target(int id)
    {
        if(m_master_targets.ContainsKey(id))
        {
            return m_master_targets[id];
        }
        return null;
    }
    void parse_t_master_reward()
    {
        m_dbc_master_reward.load_txt("t_master_reward", 0);
        foreach(int id in m_dbc_master_reward.m_index.Keys)
        {
            s_t_master_reward reward = new s_t_master_reward();
            reward.rank1 = id;
            reward.rank2 = int.Parse(m_dbc_master_reward.get_index(1,id));
            for(int i = 0; i < 2;i++)
            {
                s_t_reward re = new s_t_reward();
                re.type = int.Parse(m_dbc_master_reward.get_index(2 + 4 * i, id));
                re.value1 = int.Parse(m_dbc_master_reward.get_index(3 + 4 * i, id));
                re.value2 = int.Parse(m_dbc_master_reward.get_index(4 + 4 * i, id));
                re.value3 = int.Parse(m_dbc_master_reward.get_index(5 + 4 * i, id));
                reward.rewards.Add(re);
            }
            m_master_rewards.Add(id, reward);
        }
    }
    public s_t_master_reward get_t_master_reward(int id)
    {
        if (m_master_rewards.ContainsKey(id))
        {
            return m_master_rewards[id];
        }
        return null;
    }
    public s_t_guild_mubiao get_guild_mubiao(int id)
	{
		s_t_guild_mubiao _t_guild_mubiao = new s_t_guild_mubiao ();
		if (!game_data._instance.m_dbc_guild_mubiao.has_index(id))
		{
			return null;
		}
		_t_guild_mubiao.id = int.Parse(game_data._instance.m_dbc_guild_mubiao.get_index(0,id));
		_t_guild_mubiao.level = int.Parse(game_data._instance.m_dbc_guild_mubiao.get_index(1,id));
		_t_guild_mubiao.name = game_data._instance.m_dbc_guild_mubiao.get_index (2, id);
		_t_guild_mubiao.type = int.Parse(game_data._instance.m_dbc_guild_mubiao.get_index(3,id));
		_t_guild_mubiao.value1 = int.Parse(game_data._instance.m_dbc_guild_mubiao.get_index(4,id));
		_t_guild_mubiao.value2 = int.Parse(game_data._instance.m_dbc_guild_mubiao.get_index(5,id));
		_t_guild_mubiao.value3 = int.Parse(game_data._instance.m_dbc_guild_mubiao.get_index(6,id));
		_t_guild_mubiao.price = int.Parse(game_data._instance.m_dbc_guild_mubiao.get_index(7,id));
		_t_guild_mubiao.discount = int.Parse(game_data._instance.m_dbc_guild_mubiao.get_index(8,id));
		return _t_guild_mubiao;
	}

	public s_t_guild_sign get_guild_sign(int id)
	{
		s_t_guild_sign _t_guild_sign = new s_t_guild_sign ();
		if (!game_data._instance.m_dbc_guild_sign.has_index(id))
		{
			return null;
		}
		_t_guild_sign.id = int.Parse(game_data._instance.m_dbc_guild_sign.get_index(0,id));
		_t_guild_sign.name = get_t_language(game_data._instance.m_dbc_guild_sign.get_index(1,id));
		_t_guild_sign.coin = int.Parse(game_data._instance.m_dbc_guild_sign.get_index(2,id));
		_t_guild_sign.zuanshi = int.Parse(game_data._instance.m_dbc_guild_sign.get_index(3,id));
        _t_guild_sign.exp = int.Parse(game_data._instance.m_dbc_guild_sign.get_index(4, id));
		_t_guild_sign.gongxian = int.Parse(game_data._instance.m_dbc_guild_sign.get_index(5,id));
		_t_guild_sign.hor = int.Parse(game_data._instance.m_dbc_guild_sign.get_index(6,id));
		return _t_guild_sign;
	}
    public s_t_guild_keji get_t_guild_keji(int id)
    {
        s_t_guild_keji _guild_keji = new s_t_guild_keji();
        _guild_keji.id = id;
        _guild_keji.name = get_t_language(m_dbc_guild_keji.get_index(2,id));
        _guild_keji.icon = m_dbc_guild_keji.get_index(3, id);
        _guild_keji.guildlevel = int.Parse(m_dbc_guild_keji.get_index(4, id));
        _guild_keji.level = int.Parse(m_dbc_guild_keji.get_index(5, id));
        _guild_keji.sx = int.Parse(m_dbc_guild_keji.get_index(6, id));
        _guild_keji.sx_value = int.Parse(m_dbc_guild_keji.get_index(7, id));
        _guild_keji.exp = int.Parse(m_dbc_guild_keji.get_index(8, id));
        _guild_keji.expadd = int.Parse(m_dbc_guild_keji.get_index(9, id));
        _guild_keji.con = int.Parse(m_dbc_guild_keji.get_index(10, id));
        _guild_keji.conadd = int.Parse(m_dbc_guild_keji.get_index(11, id));
        return _guild_keji;
    }
	public s_t_dress get_t_dress(int id)
	{
		if(m_dress_s.ContainsKey(id))
		{
			return m_dress_s[id];
		}
		s_t_dress _t_dress = new s_t_dress ();
		if (!game_data._instance.m_dbc_dress.has_index(id))
		{
			return null;
		}
		_t_dress.id = int.Parse (game_data._instance.m_dbc_dress.get_index(0,id));
		_t_dress.name = get_t_language(game_data._instance.m_dbc_dress.get_index(2,id));
		_t_dress.type = int.Parse (game_data._instance.m_dbc_dress.get_index(3,id));
        _t_dress.color = int.Parse(game_data._instance.m_dbc_dress.get_index(4, id));
		_t_dress.icon = game_data._instance.m_dbc_dress.get_index(5,id);
		_t_dress.res = game_data._instance.m_dbc_dress.get_index(6,id);
		for (int i = 0; i < 5; ++i)
		{
			s_t_attr t_attr = new s_t_attr();
			t_attr.attr = int.Parse (game_data._instance.m_dbc_dress.get_index(7 + i * 2,id));
			t_attr.value = int.Parse (game_data._instance.m_dbc_dress.get_index(8 + i * 2,id));
			_t_dress.attrs.Add(t_attr);
		}
		_t_dress.action1 = game_data._instance.m_dbc_dress.get_index(17,id);
		_t_dress.action2 = game_data._instance.m_dbc_dress.get_index(18,id);
		m_dress_s[id] = _t_dress;
		return _t_dress;
	}

	public s_t_dress_target get_t_dress_target(int id)
	{
		if(m_dress_target_s.ContainsKey(id))
		{
			return m_dress_target_s[id];
		}
		s_t_dress_target _t_dress_target = new s_t_dress_target ();
		if (!game_data._instance.m_dbc_dress_target.has_index(id))
		{
			return null;
		}
		_t_dress_target.id = int.Parse (game_data._instance.m_dbc_dress_target.get_index(0,id));
		_t_dress_target.name =get_t_language( game_data._instance.m_dbc_dress_target.get_index(2,id));
		_t_dress_target.type = int.Parse (game_data._instance.m_dbc_dress_target.get_index(3,id));
		_t_dress_target.desc = get_t_language(game_data._instance.m_dbc_dress_target.get_index(4,id));
		for (int i = 0; i < 5; ++i)
		{
			int def = int.Parse(game_data._instance.m_dbc_dress_target.get_index(5 + i, id));
			if (def > 0)
			{
				_t_dress_target.defs.Add(def);
			}
		}
		_t_dress_target.attr1 = int.Parse(game_data._instance.m_dbc_dress_target.get_index(10,id));
		_t_dress_target.value1 = int.Parse(game_data._instance.m_dbc_dress_target.get_index(11,id));
		_t_dress_target.attr2 = int.Parse(game_data._instance.m_dbc_dress_target.get_index(12,id));
		_t_dress_target.value2 = int.Parse(game_data._instance.m_dbc_dress_target.get_index(13,id));
		_t_dress_target.attr3 = int.Parse(game_data._instance.m_dbc_dress_target.get_index(14,id));
		_t_dress_target.value3 = int.Parse(game_data._instance.m_dbc_dress_target.get_index(15,id));
		_t_dress_target.attr4 = int.Parse(game_data._instance.m_dbc_dress_target.get_index(16,id));
		_t_dress_target.value4 = int.Parse(game_data._instance.m_dbc_dress_target.get_index(17,id));
		m_dress_target_s[id] = _t_dress_target;
		return _t_dress_target;
	}

	public s_t_role_dress get_t_role_dress(int id)
	{
		s_t_role_dress _t_role_dress = new s_t_role_dress ();
		if (!game_data._instance.m_dbc_role_dress.has_index(id))
		{
			return null;
		}
		_t_role_dress.id = int.Parse (game_data._instance.m_dbc_role_dress.get_index(0,id));
		_t_role_dress.name = get_t_language(game_data._instance.m_dbc_role_dress.get_index(2,id));
		_t_role_dress.role = int.Parse (game_data._instance.m_dbc_role_dress.get_index(3,id));
		_t_role_dress.icon = game_data._instance.m_dbc_role_dress.get_index(4,id);
		_t_role_dress.icon1 = game_data._instance.m_dbc_role_dress.get_index(5,id);
		_t_role_dress.res = game_data._instance.m_dbc_role_dress.get_index(6,id);
		_t_role_dress.hq_condition = int.Parse (game_data._instance.m_dbc_role_dress.get_index(7,id));
		_t_role_dress.hq_Level = int.Parse (game_data._instance.m_dbc_role_dress.get_index(8,id));
		_t_role_dress.mai_desc = get_t_language(game_data._instance.m_dbc_role_dress.get_index(10,id));
		return _t_role_dress;
	}

	public s_t_xjbz get_t_xjbz(int site)
	{
		s_t_xjbz _t_xjbz = new s_t_xjbz ();
		if (!game_data._instance.m_dbc_xjbz.has_index(site))
		{
			return null;
		}
		
		_t_xjbz.site = int.Parse (game_data._instance.m_dbc_xjbz.get_index(0,site));
		_t_xjbz.type = int.Parse (game_data._instance.m_dbc_xjbz.get_index(1,site));
		_t_xjbz.name = get_t_language(game_data._instance.m_dbc_xjbz.get_index(2,site));
		_t_xjbz.db = int.Parse (game_data._instance.m_dbc_xjbz.get_index(3,site));
		_t_xjbz.def1 = int.Parse (game_data._instance.m_dbc_xjbz.get_index(4,site));
		_t_xjbz.def2 = int.Parse (game_data._instance.m_dbc_xjbz.get_index(5,site));
		return _t_xjbz;
	}

	public s_t_xjbz_mission get_t_xjbz_mission(int level)
	{
		s_t_xjbz_mission _t_xjbz_mission = new s_t_xjbz_mission ();
		for (int i = 0; i < m_dbc_xjbz_mission.get_y(); i++) 
		{
			int level1 = int.Parse (game_data._instance.m_dbc_xjbz_mission.get(0, i));
			int level2 = int.Parse (game_data._instance.m_dbc_xjbz_mission.get(1, i));
			if (level >= level1 && level <= level2)
			{
				_t_xjbz_mission.level1 = level1;
				_t_xjbz_mission.level2 = level2;
				for (int j = 0; j < 3; ++j)
				{
					int id = int.Parse (game_data._instance.m_dbc_xjbz_mission.get(2 + j, i));
					_t_xjbz_mission.missions.Add(id);
				}
				break;
			}
		}
		return _t_xjbz_mission;
	}

	public s_t_huodong_pttq get_t_huodong_pttq(int id)
	{
		s_t_huodong_pttq _t_huodong_pttq = new s_t_huodong_pttq ();
		if (!game_data._instance.m_dbc_huodong_pttq.has_index(id))
		{
			return null;
		}
		_t_huodong_pttq.id = int.Parse (game_data._instance.m_dbc_huodong_pttq.get_index (0, id));
		_t_huodong_pttq.vip = int.Parse (game_data._instance.m_dbc_huodong_pttq.get_index(1, id));
		for (int j = 0; j < 4; ++j)
		{
			s_t_huodong_pttq_sub sub = new s_t_huodong_pttq_sub();
			sub.vip = int.Parse (game_data._instance.m_dbc_huodong_pttq.get_index(2 + j * 5, id));
			sub.reward.type = int.Parse (game_data._instance.m_dbc_huodong_pttq.get_index(3 + j * 5, id));
			sub.reward.value1 = int.Parse (game_data._instance.m_dbc_huodong_pttq.get_index(4 + j * 5, id));
			sub.reward.value2 = int.Parse (game_data._instance.m_dbc_huodong_pttq.get_index(5 + j * 5, id));
			sub.reward.value3 = int.Parse (game_data._instance.m_dbc_huodong_pttq.get_index(6 + j * 5, id));
			_t_huodong_pttq.sub.Add(sub);
		}
		return _t_huodong_pttq;
	}

	public s_t_kaifu get_t_kaifu(int tian, int site)
	{
		if (m_kaifu_s.Count == 0)
		{
			for (int i = 0; i < m_dbc_kaifu.get_y(); ++i) 
			{
				s_t_kaifu t_kaifu = new s_t_kaifu();
				t_kaifu.tian = int.Parse (game_data._instance.m_dbc_kaifu.get(0, i));
				if (t_kaifu.tian > m_kaifu_s.Count)
				{
					m_kaifu_s.Add(new List<s_t_kaifu>());
				}
				t_kaifu.site = int.Parse (game_data._instance.m_dbc_kaifu.get(1, i));
				t_kaifu.name = get_t_language(game_data._instance.m_dbc_kaifu.get(3, i));
				for (int j = 0; j < 15; ++j)
				{
					int m = int.Parse (game_data._instance.m_dbc_kaifu.get(4 + j, i));
					if (t_kaifu.site != 4 && m > 0)
					{
						t_kaifu.ints.Add(m);
					}
					if (t_kaifu.site == 4)
					{
						t_kaifu.ints.Add(m);
					}
				}
				m_kaifu_s[t_kaifu.tian - 1].Add(t_kaifu);
			}
		}
		return m_kaifu_s [tian - 1] [site - 1];
	}

	public s_t_kaifu_mubiao get_t_kaifu_mubiao(int id)
	{
		s_t_kaifu_mubiao _t_kaifu_mubiao = new s_t_kaifu_mubiao ();
		if (!game_data._instance.m_dbc_kaifu_mubiao.has_index(id))
		{
			return null;
		}
		
		_t_kaifu_mubiao.id = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(0,id));
		_t_kaifu_mubiao.desc = get_t_language(game_data._instance.m_dbc_kaifu_mubiao.get_index(2,id));
		_t_kaifu_mubiao.type = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(3,id));
		_t_kaifu_mubiao.def1 = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(4,id));
		_t_kaifu_mubiao.def2 = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(5,id));
		_t_kaifu_mubiao.def3 = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(6,id));
		_t_kaifu_mubiao.def4 = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(7,id));
		_t_kaifu_mubiao.ck = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(8,id));
		for (int i = 0; i < 4; ++i)
		{
			s_t_reward t_reward = new s_t_reward();
			t_reward.type = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(9 + i * 4,id));
			t_reward.value1 = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(10 + i * 4,id));
			t_reward.value2 = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(11 + i * 4,id));
			t_reward.value3 = int.Parse (game_data._instance.m_dbc_kaifu_mubiao.get_index(12 + i * 4,id));
			if (t_reward.type > 0)
			{
				_t_kaifu_mubiao.rewards.Add(t_reward);
			}
		}
		
		return _t_kaifu_mubiao;
	}

	public s_t_jc_huodong get_t_jc_huodong(int id)
	{
		s_t_jc_huodong _t_jc_huodong = new s_t_jc_huodong ();
		if (!game_data._instance.m_dbc_jc_huodong.has_index(id))
		{
			return null;
		}
		
		_t_jc_huodong.id = int.Parse (game_data._instance.m_dbc_jc_huodong.get_index(0,id));
		_t_jc_huodong.name = get_t_language(game_data._instance.m_dbc_jc_huodong.get_index(2,id));
		_t_jc_huodong.name_color = game_data._instance.m_dbc_jc_huodong.get_index(3,id);
		_t_jc_huodong.name_mb = game_data._instance.m_dbc_jc_huodong.get_index(4,id);
		_t_jc_huodong.text = get_t_language(game_data._instance.m_dbc_jc_huodong.get_index(5,id));
		_t_jc_huodong.text_color = game_data._instance.m_dbc_jc_huodong.get_index(6,id);
		_t_jc_huodong.text_mb = game_data._instance.m_dbc_jc_huodong.get_index(7,id);
		_t_jc_huodong.time_color = game_data._instance.m_dbc_jc_huodong.get_index(8,id);
		_t_jc_huodong.time_mb = game_data._instance.m_dbc_jc_huodong.get_index(9,id);
		_t_jc_huodong.res = game_data._instance.m_dbc_jc_huodong.get_index(10,id);
		_t_jc_huodong.message = game_data._instance.m_dbc_jc_huodong.get_index(11,id);
		
		return _t_jc_huodong;
	}

	public s_t_boss_reward get_t_boss_reward(int index)
	{
		s_t_boss_reward _t_boss_rewrad = new s_t_boss_reward ();
		if (!game_data._instance.m_dbc_boss_reward.has_index(index))
		{
			return null;
		}
		_t_boss_rewrad.id1 = int.Parse(game_data._instance.m_dbc_boss_reward.get_index(0,index));
        _t_boss_rewrad.id2 = int.Parse(game_data._instance.m_dbc_boss_reward.get_index(1, index));
		for (int i = 0; i < 2; ++i)
		{
			s_t_reward t_reward = new s_t_reward();
			t_reward.type = int.Parse(game_data._instance.m_dbc_boss_reward.get_index(2 + i * 4,index));
			t_reward.value1 = int.Parse(game_data._instance.m_dbc_boss_reward.get_index(3 + i * 4,index));
			t_reward.value2 = int.Parse(game_data._instance.m_dbc_boss_reward.get_index(4 + i * 4,index));
			t_reward.value3 = int.Parse(game_data._instance.m_dbc_boss_reward.get_index(5 + i * 4,index));
			if (t_reward.type != 0)
			{
				_t_boss_rewrad.rewards.Add(t_reward);
			}
		}
		
		return _t_boss_rewrad;
	}

	public s_t_huodong_czjh get_t_huodong_czjh(int id)
	{
		s_t_huodong_czjh _t_huodong_czjh = new s_t_huodong_czjh ();
		if (!game_data._instance.m_dbc_huodong_czjh.has_index(id))
		{
			return null;
		}
		
		_t_huodong_czjh.id = int.Parse (game_data._instance.m_dbc_huodong_czjh.get_index (0, id));
		_t_huodong_czjh.level = int.Parse (game_data._instance.m_dbc_huodong_czjh.get_index(1, id));
		_t_huodong_czjh.jewel = int.Parse (game_data._instance.m_dbc_huodong_czjh.get_index(2, id));

		return _t_huodong_czjh;
	}

	public s_t_equip_tz get_t_equip_tz(int equip_id)
	{
		if (m_equip_tz_map_.Count == 0)
		{
			for (int i = 0; i < m_dbc_equip_tz.get_y(); ++i)
			{
				int id = int.Parse (m_dbc_equip_tz.get (0, i));
				int eid = int.Parse (m_dbc_equip_tz.get (3, i));
				m_equip_tz_map_.Add(eid, id);
				eid = int.Parse (m_dbc_equip_tz.get (4, i));
				m_equip_tz_map_.Add(eid, id);
				eid = int.Parse (m_dbc_equip_tz.get (5, i));
				m_equip_tz_map_.Add(eid, id);
				eid = int.Parse (m_dbc_equip_tz.get (6, i));
				m_equip_tz_map_.Add(eid, id);
			}
		}
		if (!m_equip_tz_map_.ContainsKey(equip_id))
		{
			return null;
		}
		int tz_id = m_equip_tz_map_ [equip_id];


		if(m_equip_tz_s.ContainsKey(tz_id))
		{
			return m_equip_tz_s[tz_id];
		}

		if (!game_data._instance.m_dbc_equip_tz.has_index(tz_id))
		{
			return null;
		}

		s_t_equip_tz t_equip_tz = new s_t_equip_tz();
		t_equip_tz.id = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (0, tz_id));
		t_equip_tz.name = get_t_language(game_data._instance.m_dbc_equip_tz.get_index (2, tz_id));
		for (int i = 0; i < 4; ++i)
		{
			int id = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (3 + i, tz_id));
			t_equip_tz.equip_ids.Add(id);
		}
		t_equip_tz.attr1 = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (7, tz_id));
		t_equip_tz.value1 = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (8, tz_id));
		t_equip_tz.attr2 = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (9, tz_id));
		t_equip_tz.value2 = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (10, tz_id));
		t_equip_tz.attr3 = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (11, tz_id));
		t_equip_tz.value3 = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (12, tz_id));
		t_equip_tz.attr4 = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (13, tz_id));
		t_equip_tz.value4 = int.Parse (game_data._instance.m_dbc_equip_tz.get_index (14, tz_id));

		m_equip_tz_s [tz_id] = t_equip_tz;

		return t_equip_tz;
	}

	public s_t_equip_sx get_t_equip_sx(int color,int star)
	{
		int tmp = color * 1000 + star;
		if (m_equip_sx.ContainsKey(tmp))
		{
			return m_equip_sx[tmp];
		}
		s_t_equip_sx _equip_sx = new s_t_equip_sx ();
		for(int j = 0;j < game_data._instance.m_dbc_equip_sx.get_y();++j)
		{
			if(int.Parse (m_dbc_equip_sx.get (0, j))== star && int.Parse (m_dbc_equip_sx.get (1, j)) == color)
			{
				_equip_sx.level = int.Parse (m_dbc_equip_sx.get (0, j));
				_equip_sx.color = int.Parse (m_dbc_equip_sx.get (1, j));
				_equip_sx.price = int.Parse (m_dbc_equip_sx.get (2, j));
				_equip_sx.enhance_rate = float.Parse (m_dbc_equip_sx.get (3, j));
				_equip_sx.sp_num = int.Parse (m_dbc_equip_sx.get (4, j));
				_equip_sx.gold = int.Parse (m_dbc_equip_sx.get (5, j));
				m_equip_sx[tmp] = _equip_sx;
				return _equip_sx;		
			}
		}
		return null;
	}

	public s_t_equip_jl get_t_equip_jl(int id)
	{
		s_t_equip_jl _t_equip_jl = new s_t_equip_jl ();
		if (!game_data._instance.m_dbc_equip_jl.has_index(id))
		{
			return null;
		}
		
		_t_equip_jl.level  = int.Parse(game_data._instance.m_dbc_equip_jl.get_index(0,id));
		for(int i = 0; i < 5;++i)
		{
			int stone = int.Parse(game_data._instance.m_dbc_equip_jl.get_index(1 + 2*i,id));
			int gold =  int.Parse(game_data._instance.m_dbc_equip_jl.get_index(2 + 2*i,id));
			_t_equip_jl.stones.Add(stone);
			_t_equip_jl.golds.Add(gold);
		}
		return _t_equip_jl;
	}

	public s_t_baowu get_t_baowu(int id)
	{
		if(m_baowus.ContainsKey(id))
		{
			return m_baowus[id];
		}
		
		s_t_baowu _t_baowu = new s_t_baowu ();
		if (!game_data._instance.m_dbc_baowu.has_index(id))
		{
			return null;
		}
		
		_t_baowu.id = int.Parse(game_data._instance.m_dbc_baowu.get_index(0,id));
		_t_baowu.name = get_t_language(game_data._instance.m_dbc_baowu.get_index(2,id));
		_t_baowu.font_color = int.Parse(game_data._instance.m_dbc_baowu.get_index(3,id));
		_t_baowu.type = int.Parse(game_data._instance.m_dbc_baowu.get_index(4,id));
		_t_baowu.icon = game_data._instance.m_dbc_baowu.get_index(5,id);
		_t_baowu.exp = int.Parse(game_data._instance.m_dbc_baowu.get_index(6,id));
		_t_baowu.sell = int.Parse(game_data._instance.m_dbc_baowu.get_index(7,id));
		_t_baowu.attr1 = int.Parse(game_data._instance.m_dbc_baowu.get_index(8,id));
		_t_baowu.value1 = float.Parse(game_data._instance.m_dbc_baowu.get_index(9,id));
		_t_baowu.attr2 = int.Parse(game_data._instance.m_dbc_baowu.get_index(10,id));
		_t_baowu.value2 = float.Parse(game_data._instance.m_dbc_baowu.get_index(11,id));
		_t_baowu.jl_type_0 = int.Parse(game_data._instance.m_dbc_baowu.get_index(12,id));
		_t_baowu.jl_value_0 = float.Parse(game_data._instance.m_dbc_baowu.get_index(13,id));
		_t_baowu.jl_type_1 = int.Parse(game_data._instance.m_dbc_baowu.get_index(14,id));
		_t_baowu.jl_value_1 = float.Parse(game_data._instance.m_dbc_baowu.get_index(15,id));
		_t_baowu.jl_type_2 = int.Parse(game_data._instance.m_dbc_baowu.get_index(16,id));
		_t_baowu.jl_value_2 = float.Parse(game_data._instance.m_dbc_baowu.get_index(17,id));

		for(int i = 0;i < 6;i ++ )
		{

			if(int.Parse(game_data._instance.m_dbc_baowu.get_index(i + 18,id)) != 0)
			{
				_t_baowu.fragments.Add(int.Parse(game_data._instance.m_dbc_baowu.get_index(i + 18,id)));
				_t_baowu.count++;
			}
		}
		
		m_baowus [id] = _t_baowu;
		
		return _t_baowu;
	}

	public s_t_baowu_jl get_t_baowu_jl(int level)
	{
		s_t_baowu_jl t_treasure_jl = new s_t_baowu_jl();
		if (!game_data._instance.m_dbc_baowu_jl.has_index(level))
		{
			return null;
		}
		t_treasure_jl.level = int.Parse (game_data._instance.m_dbc_baowu_jl.get_index (0, level));
		t_treasure_jl.stone = int.Parse (game_data._instance.m_dbc_baowu_jl.get_index (1, level));
		t_treasure_jl.cost = int.Parse (game_data._instance.m_dbc_baowu_jl.get_index (2, level));
		t_treasure_jl.num = int.Parse (game_data._instance.m_dbc_baowu_jl.get_index (3, level));

		return t_treasure_jl;
	}

	public s_t_yb get_t_yb(int type)
	{
		s_t_yb t_yb = new s_t_yb ();
		if (!game_data._instance.m_dbc_yb.has_index(type))
		{
			return null;
		}
		
		t_yb.type  = int.Parse(game_data._instance.m_dbc_yb.get_index(0,type));
		t_yb.name = get_t_language(game_data._instance.m_dbc_yb.get_index(1,type));
		t_yb.time = int.Parse(game_data._instance.m_dbc_yb.get_index(2,type));
		t_yb.rate = int.Parse(game_data._instance.m_dbc_yb.get_index(3,type));
		t_yb.yuanli = int.Parse(game_data._instance.m_dbc_yb.get_index(4,type));
		t_yb.per = int.Parse(game_data._instance.m_dbc_yb.get_index(5,type));
		t_yb.min_per = int.Parse(game_data._instance.m_dbc_yb.get_index(6,type));
		
		return t_yb;
	}

	public s_t_yb_gw get_t_yb_gw(int index)
	{
		s_t_yb_gw _t_yb_gw = new s_t_yb_gw ();
		if (!game_data._instance.m_dbc_yb_gw.has_index(index))
		{
			return null;
		}
		
		_t_yb_gw.index  = int.Parse(game_data._instance.m_dbc_yb_gw.get_index(0,index));
		_t_yb_gw.gj = float.Parse(game_data._instance.m_dbc_yb_gw.get_index(1,index));
		_t_yb_gw.jewel = int.Parse(game_data._instance.m_dbc_yb_gw.get_index(2,index));
		_t_yb_gw.desc = get_t_language(game_data._instance.m_dbc_yb_gw.get_index(3,index));
		
		return _t_yb_gw;
	}

	public s_t_ore get_t_ore(int index)
	{
		s_t_ore _t_ore = new s_t_ore ();
		if (!game_data._instance.m_dbc_ore.has_index(index))
		{
			return null;
		}
		
		_t_ore.index  = int.Parse(game_data._instance.m_dbc_ore.get_index(0,index));
		_t_ore.monster_id = int.Parse(game_data._instance.m_dbc_ore.get_index(1,index));
		_t_ore.level = int.Parse(game_data._instance.m_dbc_ore.get_index(2,index));
		_t_ore.tili = int.Parse(game_data._instance.m_dbc_ore.get_index(3,index));
		_t_ore.bd_gold = int.Parse(game_data._instance.m_dbc_ore.get_index(4,index));
		_t_ore.hx_gold = int.Parse(game_data._instance.m_dbc_ore.get_index(5,index));
		_t_ore.jl_js = int.Parse(game_data._instance.m_dbc_ore.get_index(6,index));
		_t_ore.zjs = int.Parse(game_data._instance.m_dbc_ore.get_index(7,index));
		_t_ore.szjs = int.Parse(game_data._instance.m_dbc_ore.get_index(8,index));
		_t_ore.nd = get_t_language(game_data._instance.m_dbc_ore.get_index(9,index));
		_t_ore.ndsm = get_t_language(game_data._instance.m_dbc_ore.get_index(10,index));
		_t_ore.ndbs = game_data._instance.m_dbc_ore.get_index(11,index);
		return _t_ore;
	}

	public int get_treasure_enhance(int level, int color)
	{
		if (!game_data._instance.m_dbc_treasure_enhance.has_index(level))
		{
			return 0;
		}
		return int.Parse(game_data._instance.m_dbc_treasure_enhance.get_index(color-1, level));
	}
	
	public int get_total_treasure_enhance(int level, int color)
	{
		int gold = 0;
		for (int i = 0; i <= level; ++i)
		{
			gold += int.Parse(game_data._instance.m_dbc_treasure_enhance.get_index(color-1, i));
		}
		return gold;
	}

	public s_t_sport_shop get_t_sport_shop(int id)
	{
		s_t_sport_shop _t_sport_shop = new s_t_sport_shop ();
		if (!game_data._instance.m_dbc_sport_shop.has_index(id))
		{
			return null;
		}
		
		_t_sport_shop.id = int.Parse(m_dbc_sport_shop.get_index(0,id));
		_t_sport_shop.name = m_dbc_sport_shop.get_index(1,id);
		_t_sport_shop.level = int.Parse(m_dbc_sport_shop.get_index(2,id));
		_t_sport_shop.type = int.Parse(m_dbc_sport_shop.get_index(3,id));
		_t_sport_shop.value1 = int.Parse(m_dbc_sport_shop.get_index(4,id));
		_t_sport_shop.value2 = int.Parse(m_dbc_sport_shop.get_index(5,id));
		_t_sport_shop.value3 = int.Parse(m_dbc_sport_shop.get_index(6,id));
		_t_sport_shop.price = int.Parse(m_dbc_sport_shop.get_index(7,id));
		_t_sport_shop.hb_power = int.Parse(m_dbc_sport_shop.get_index(8,id));
		
		return _t_sport_shop;
	}

	public s_t_sport_mubiao get_t_sport_mubiao(int id)
	{
		s_t_sport_mubiao _t_sport_mubiao = new s_t_sport_mubiao ();
		if (!game_data._instance.m_dbc_sport_mubiao.has_index(id))
		{
			return null;
		}
		
		_t_sport_mubiao.id = int.Parse(m_dbc_sport_mubiao.get_index(0,id));
		_t_sport_mubiao.rank = int.Parse(m_dbc_sport_mubiao.get_index(1,id));
		_t_sport_mubiao.name = m_dbc_sport_mubiao.get_index(2,id);
		_t_sport_mubiao.type = int.Parse(m_dbc_sport_mubiao.get_index(3,id));
		_t_sport_mubiao.value1 = int.Parse(m_dbc_sport_mubiao.get_index(4,id));
		_t_sport_mubiao.value2 = int.Parse(m_dbc_sport_mubiao.get_index(5,id));
		_t_sport_mubiao.value3 = int.Parse(m_dbc_sport_mubiao.get_index(6,id));
		_t_sport_mubiao.price = int.Parse(m_dbc_sport_mubiao.get_index(7,id));
		_t_sport_mubiao.discount = int.Parse(m_dbc_sport_mubiao.get_index(8,id));
		
		return _t_sport_mubiao;
	}

	public s_t_xinqing_event get_t_xinqing_event(int id)
	{
		s_t_xinqing_event _t_xinqing_event = new s_t_xinqing_event ();
		if (!game_data._instance.m_dbc_xinqing_event.has_index(id))
		{
			return null;
		}
		
		_t_xinqing_event.id = int.Parse(game_data._instance.m_dbc_xinqing_event.get_index(0,id));
		_t_xinqing_event.name = game_data._instance.m_dbc_xinqing_event.get_index(1,id);
		_t_xinqing_event.scene = game_data._instance.m_dbc_xinqing_event.get_index(2,id);
		_t_xinqing_event.start_scene = game_data._instance.m_dbc_xinqing_event.get_index(3,id);
		_t_xinqing_event.role_id = int.Parse(game_data._instance.m_dbc_xinqing_event.get_index(4,id));
		_t_xinqing_event.rate = int.Parse(game_data._instance.m_dbc_xinqing_event.get_index(5,id));
		_t_xinqing_event.type = int.Parse(game_data._instance.m_dbc_xinqing_event.get_index(6,id));
		_t_xinqing_event.select1 = get_t_language(game_data._instance.m_dbc_xinqing_event.get_index(8,id));
		_t_xinqing_event.end_scene1 = game_data._instance.m_dbc_xinqing_event.get_index(9,id);
		_t_xinqing_event.result1 = game_data._instance.m_dbc_xinqing_event.get_index(10,id);
		_t_xinqing_event.select2 = get_t_language(game_data._instance.m_dbc_xinqing_event.get_index(12,id));
		_t_xinqing_event.end_scene2 = game_data._instance.m_dbc_xinqing_event.get_index(13,id);
		_t_xinqing_event.result2 = game_data._instance.m_dbc_xinqing_event.get_index(14,id);
		_t_xinqing_event.select3 = get_t_language(game_data._instance.m_dbc_xinqing_event.get_index(16,id));
		_t_xinqing_event.end_scene3 = game_data._instance.m_dbc_xinqing_event.get_index(17,id);
		_t_xinqing_event.result3 = game_data._instance.m_dbc_xinqing_event.get_index(18,id);
		
		return _t_xinqing_event;
	}

	public s_t_xinqing_random get_t_xinqing_random(int id)
	{
		s_t_xinqing_random _t_xinqing_random = new s_t_xinqing_random ();
		if (!game_data._instance.m_dbc_xinqing_random.has_index(id))
		{
			return null;
		}
		
		_t_xinqing_random.id = int.Parse(game_data._instance.m_dbc_xinqing_random.get_index(0,id));
		_t_xinqing_random.scene = game_data._instance.m_dbc_xinqing_random.get_index(1,id);
		_t_xinqing_random.select = get_t_language(game_data._instance.m_dbc_xinqing_random.get_index(3,id));
		_t_xinqing_random.good_result = get_t_language(game_data._instance.m_dbc_xinqing_random.get_index (5,id));
		_t_xinqing_random.good_result_change = game_data._instance.m_dbc_xinqing_random.get_index (6,id);
		_t_xinqing_random.bad_result = get_t_language(game_data._instance.m_dbc_xinqing_random.get_index (8,id));
		_t_xinqing_random.bad_result_change = game_data._instance.m_dbc_xinqing_random.get_index (9,id);

		return _t_xinqing_random;
	}
	
	public s_t_xinqing get_t_xinqing(int level)
	{
		s_t_xinqing _t_xinqing = new s_t_xinqing ();
		if (!game_data._instance.m_dbc_xinqing.has_index(level))
		{
			return null;
		}
		
		_t_xinqing.level = int.Parse(game_data._instance.m_dbc_xinqing.get_index(0,level));
		_t_xinqing.xinqing = game_data._instance.m_dbc_xinqing.get_index(1,level);
		_t_xinqing.icon = game_data._instance.m_dbc_xinqing.get_index(2,level);
		_t_xinqing.sx_per = int.Parse(game_data._instance.m_dbc_xinqing.get_index (3, level));
		
		return _t_xinqing;
	}

    public s_t_pvp_active get_t_pvp_active(int id)
    {
        s_t_pvp_active _t_pvp_active = new s_t_pvp_active();
        if (!game_data._instance.m_dbc_pvp_active.has_index(id))
        {
            return null;
        }
        _t_pvp_active.id = int.Parse(game_data._instance.m_dbc_pvp_active.get_index(0, id));
        _t_pvp_active.name = get_t_language(game_data._instance.m_dbc_pvp_active.get_index(2, id));
        _t_pvp_active.neednum = int.Parse(game_data._instance.m_dbc_pvp_active.get_index(3, id));
        _t_pvp_active.lieb = int.Parse(game_data._instance.m_dbc_pvp_active.get_index(4, id));
        return _t_pvp_active;
    }
    public s_t_pvp_shop get_t_pvp_shop(int id)
    {
        s_t_pvp_shop _t_pvp_shop = new s_t_pvp_shop();
        if (!m_dbc_pvp_shop.has_index(id))
        {
            return null;
        }
        _t_pvp_shop.id = int.Parse(game_data._instance.m_dbc_pvp_shop.get_index(0, id));
        _t_pvp_shop.type = int.Parse(game_data._instance.m_dbc_pvp_shop.get_index(2, id));
        _t_pvp_shop.value1 = int.Parse(game_data._instance.m_dbc_pvp_shop.get_index(3, id));
        _t_pvp_shop.value2 = int.Parse(game_data._instance.m_dbc_pvp_shop.get_index(4, id));
        _t_pvp_shop.value3 = int.Parse(game_data._instance.m_dbc_pvp_shop.get_index(5, id));
        _t_pvp_shop.liebi = int.Parse(game_data._instance.m_dbc_pvp_shop.get_index(6, id));
        _t_pvp_shop.redrolepower = int.Parse(game_data._instance.m_dbc_pvp_shop.get_index(7, id));
        _t_pvp_shop.redequippower = int.Parse(game_data._instance.m_dbc_pvp_shop.get_index(8, id));
        return _t_pvp_shop;
    }
    public s_t_pvp_reward get_t_pvp_reward(int id)
    {
        s_t_pvp_reward _t_pvp_reward = new s_t_pvp_reward();
        if (!m_dbc_pvp_reward.has_index(id))
        {
            return null;
        }
        _t_pvp_reward.id1 = int.Parse(game_data._instance.m_dbc_pvp_reward.get_index(0, id));
        _t_pvp_reward.id2 = int.Parse(game_data._instance.m_dbc_pvp_reward.get_index(1, id));

        for (int i = 0; i < 5; ++i)
		{
			s_t_reward t_reward = new s_t_reward();
			t_reward.type = int.Parse(game_data._instance.m_dbc_pvp_reward.get_index(2 + i * 4,id));
            t_reward.value1 = int.Parse(game_data._instance.m_dbc_pvp_reward.get_index(3 + i * 4, id));
            t_reward.value2 = int.Parse(game_data._instance.m_dbc_pvp_reward.get_index(4 + i * 4, id));
            t_reward.value3 = int.Parse(game_data._instance.m_dbc_pvp_reward.get_index(5 + i * 4, id));
			if (t_reward.type != 0)
			{
				_t_pvp_reward.rewards.Add(t_reward);
			}
		}
        return _t_pvp_reward;
        
    }
    void parse_t_boss_active()
    {
        m_dbc_boss_active.load_txt("t_boss_active", 0);
        foreach (int id in m_dbc_boss_active.m_index.Keys)
        {
            s_t_boss_active _t_boss_active = new s_t_boss_active();
            _t_boss_active.id = int.Parse(game_data._instance.m_dbc_boss_active.get_index(0, id));
            _t_boss_active.task_type = int.Parse(game_data._instance.m_dbc_boss_active.get_index(1, id));
            _t_boss_active.desc = get_t_language(game_data._instance.m_dbc_boss_active.get_index(3, id));
            _t_boss_active.count = int.Parse(game_data._instance.m_dbc_boss_active.get_index(4, id));
            _t_boss_active.type = int.Parse(game_data._instance.m_dbc_boss_active.get_index(5, id));
            _t_boss_active.value1 = int.Parse(game_data._instance.m_dbc_boss_active.get_index(6, id));
            _t_boss_active.value2 = int.Parse(game_data._instance.m_dbc_boss_active.get_index(7, id));
            _t_boss_active.value3 = int.Parse(game_data._instance.m_dbc_boss_active.get_index(8, id));
            _t_boss_active.ex_add = int.Parse(game_data._instance.m_dbc_boss_active.get_index(9, id));

            m_boss_active_s[id] = _t_boss_active;
 
        }
   
    }
   
	public s_t_boss_active get_t_boss_active(int id)
	{
		if(m_boss_active_s.ContainsKey(id))
		{
			return m_boss_active_s[id];
		}
        return null;
	}
    void parse_t_hongbao_target()
    {
        m_dbc_hongbao_target.load_txt("t_hongbao_target", 0);
        foreach (int id in m_dbc_hongbao_target.m_index.Keys)
        {
            s_t_hongbao_target _t_hongbao_target = new s_t_hongbao_target();
            _t_hongbao_target.id = int.Parse(m_dbc_hongbao_target.get_index(0, id));
            _t_hongbao_target.name = m_dbc_hongbao_target.get_index(1, id);
            _t_hongbao_target.desc = get_t_language(m_dbc_hongbao_target.get_index(2, id));
            _t_hongbao_target.type = int.Parse(m_dbc_hongbao_target.get_index(3, id));
            _t_hongbao_target.tiaojian = int.Parse(m_dbc_hongbao_target.get_index(4, id));
            for (int i = 0; i < 3; i++)
            {
                int type = int.Parse(m_dbc_hongbao_target.get_index(4 * i + 5, id));
                if (type > 0)
                {
                    s_t_reward reward = new s_t_reward();
                    reward.type = type;
                    reward.value1 = int.Parse(m_dbc_hongbao_target.get_index(4 * i + 6, id));
                    reward.value2 = int.Parse(m_dbc_hongbao_target.get_index(4 * i  + 7, id));
                    reward.value3 = int.Parse(m_dbc_hongbao_target.get_index(4 * i + 8, id));
                    _t_hongbao_target.rewrds.Add(reward);
                }
            }
            m_hongbao_s[id] = _t_hongbao_target;

        }


    }
    public s_t_hongbao_target get_t_hongbao_target(int id)
    {
        if (m_hongbao_s.ContainsKey(id))
        {
            return m_hongbao_s[id];
        }
        return null;
    }
	public s_t_boss_shop get_t_boss_shop(int id)
	{
		s_t_boss_shop _t_boss_shop = new s_t_boss_shop ();
		if (!game_data._instance.m_dbc_boss_shop.has_index(id))
		{
			return null;
		}
		
		_t_boss_shop.id = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(0,id));
		_t_boss_shop.name = game_data._instance.m_dbc_boss_shop.get_index(1,id);
		_t_boss_shop.level = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(2,id));
		_t_boss_shop.type = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(3,id));
		_t_boss_shop.value1 = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(4,id));
		_t_boss_shop.value2 = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(5,id));
		_t_boss_shop.value3 = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(6,id));
		_t_boss_shop.price = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(7,id));
		_t_boss_shop.hb_power = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(8,id));
		_t_boss_shop.discount = int.Parse(game_data._instance.m_dbc_boss_shop.get_index(9,id));
		
		return _t_boss_shop;
	}
    void parse_t_boss_dw()
    {
        m_dbc_boss_dw.load_txt("t_boss_dw", 0);
        foreach (int id in game_data._instance.m_dbc_boss_dw.m_index.Keys)
        {
            s_t_boss_dw _t_boss_dw = new s_t_boss_dw();
            _t_boss_dw.level1 = int.Parse(m_dbc_boss_dw.get_index(0, id));
            _t_boss_dw.level2 = int.Parse(m_dbc_boss_dw.get_index(1, id));
            _t_boss_dw.dw = int.Parse(m_dbc_boss_dw.get_index(2, id));
            _t_boss_dw.base_hurt = int.Parse(m_dbc_boss_dw.get_index(3, id));
            m_boss_dw_s[id] = _t_boss_dw;
        }
       
 
    }
	public s_t_boss_dw get_t_boss_dw(int id)
	{
        if (m_boss_dw_s.ContainsKey(id))
        {
            return m_boss_dw_s[id];
        }
		
		return null;
	}
	public s_t_gongzhen get_t_gongzhen (int type, int level)
	{
		int tmp = type * 1000 + level;
		if (m_gongzhens.ContainsKey(tmp))
		{
			return m_gongzhens[tmp];
		}
		s_t_gongzhen _t_gongzhen = new s_t_gongzhen ();
		for(int j = game_data._instance.m_dbc_gongzhen.get_y() - 1; j >=0; --j)
		{
			if(int.Parse (m_dbc_gongzhen.get (1, j)) <= level && int.Parse (m_dbc_gongzhen.get (2, j)) == type)
			{
				_t_gongzhen.gongzhen_level = int.Parse(game_data._instance.m_dbc_gongzhen.get(0,j));
				_t_gongzhen.condition = int.Parse(game_data._instance.m_dbc_gongzhen.get(1,j));
				_t_gongzhen.task_type = int.Parse(game_data._instance.m_dbc_gongzhen.get(2,j));
				for(int i = 0; i < 6;++i)
				{
					int attr = int.Parse(game_data._instance.m_dbc_gongzhen.get(3 + i*2,j));
					float value1 = float.Parse(game_data._instance.m_dbc_gongzhen.get(4 + i*2,j));
					if(attr  > 0)
					{
						_t_gongzhen.attrs.Add(attr);
						_t_gongzhen.value1.Add(value1);
					}
				}
				m_gongzhens[tmp] = _t_gongzhen;
				return _t_gongzhen;
			}
		}
		return null;
	}

	public s_t_ttt_mibao get_t_ttt_mibao(int id)
	{
		s_t_ttt_mibao _t_ttt_mibao = new s_t_ttt_mibao ();
		if (!game_data._instance.m_dbc_ttt_mibao.has_index(id))
		{
			return null;
		}
		_t_ttt_mibao.id = int.Parse (m_dbc_ttt_mibao.get_index(0,id));
		_t_ttt_mibao.star = int.Parse (m_dbc_ttt_mibao.get_index(1,id));
		_t_ttt_mibao.name = m_dbc_ttt_mibao.get_index(2,id);
		_t_ttt_mibao.type = int.Parse (m_dbc_ttt_mibao.get_index(3,id));
		_t_ttt_mibao.value1 = int.Parse (m_dbc_ttt_mibao.get_index(4,id));
		_t_ttt_mibao.value2 = int.Parse (m_dbc_ttt_mibao.get_index(5,id));
		_t_ttt_mibao.value3 = int.Parse (m_dbc_ttt_mibao.get_index(6,id));
		_t_ttt_mibao.now_price = int.Parse (m_dbc_ttt_mibao.get_index(7,id));
		_t_ttt_mibao.old_price = int.Parse (m_dbc_ttt_mibao.get_index(8,id));

		return _t_ttt_mibao;
	}

	public s_t_dress_unlock get_t_dress_unlock(int id)
	{
		s_t_dress_unlock _t_dress_unlock = new s_t_dress_unlock ();
		if (!game_data._instance.m_dbc_dress_unlock.has_index(id))
		{
			return null;
		}
		_t_dress_unlock.id = int.Parse (m_dbc_dress_unlock.get_index(0,id));
		_t_dress_unlock.sz_id = int.Parse (m_dbc_dress_unlock.get_index(1,id));
		_t_dress_unlock.tz_num = int.Parse (m_dbc_dress_unlock.get_index(2,id));
		
		return _t_dress_unlock;
	}

	public s_t_equip_skill get_t_equip_skill(int id)
	{
		s_t_equip_skill _t_equip_skill = new s_t_equip_skill ();
		if (!game_data._instance.m_dbc_equip_skill.has_index(id))
		{
			return null;
		}
		_t_equip_skill.id = int.Parse (m_dbc_equip_skill.get_index(0,id));
		_t_equip_skill.name = get_t_language(m_dbc_equip_skill.get_index(2,id));
		_t_equip_skill.part = int.Parse (m_dbc_equip_skill.get_index(3,id));
		_t_equip_skill.jinglian = int.Parse (m_dbc_equip_skill.get_index(4,id));
		_t_equip_skill.type = int.Parse (m_dbc_equip_skill.get_index(5,id));
		_t_equip_skill.def1 = int.Parse (m_dbc_equip_skill.get_index(6,id));
		_t_equip_skill.def2 = int.Parse (m_dbc_equip_skill.get_index(7,id));
		_t_equip_skill.desc = get_t_language(m_dbc_equip_skill.get_index(9,id));
		return _t_equip_skill;
	}

	public s_t_vip_libao get_t_vip_libao(int id)
	{
		s_t_vip_libao _t_vip_libao = new s_t_vip_libao();
		if (!game_data._instance.m_dbc_vip_libao.has_index(id))
		{
			return null;
		}
		_t_vip_libao.id = int.Parse(game_data._instance.m_dbc_vip_libao.get_index(0,id));
		for (int i = 0; i < 5; i++)
		{
			s_t_reward _reward = new s_t_reward();
			_reward.type = int.Parse(game_data._instance.m_dbc_vip_libao.get_index(1 + i * 4, id));
			_reward.value1 = int.Parse(game_data._instance.m_dbc_vip_libao.get_index(2 + i * 4, id));
			_reward.value2 = int.Parse(game_data._instance.m_dbc_vip_libao.get_index(3 + i * 4, id));
			_reward.value3 = int.Parse(game_data._instance.m_dbc_vip_libao.get_index(4 + i * 4, id));
			if(_reward.type > 0)
			{
				_t_vip_libao.rewards.Add(_reward);
			}
		}
		return _t_vip_libao;
	}

	public s_t_week_libao get_t_week_libao(int id)
	{
		s_t_week_libao _t_week_libao = new s_t_week_libao();
		if (!game_data._instance.m_dbc_week_libao.has_index(id))
		{
			return null;
		}
		_t_week_libao.id = int.Parse(game_data._instance.m_dbc_week_libao.get_index(0,id));
		_t_week_libao.name = get_t_language(game_data._instance.m_dbc_week_libao.get_index(2,id));
		_t_week_libao.level1 = int.Parse(game_data._instance.m_dbc_week_libao.get_index(3,id));
		_t_week_libao.level2 = int.Parse(game_data._instance.m_dbc_week_libao.get_index(4,id));
		for (int i = 0; i < 4; i++)
		{
			s_t_reward _reward = new s_t_reward();
			_reward.type = int.Parse(game_data._instance.m_dbc_week_libao.get_index(5 + i * 4, id));
			_reward.value1 = int.Parse(game_data._instance.m_dbc_week_libao.get_index(6 + i * 4, id));
			_reward.value2 = int.Parse(game_data._instance.m_dbc_week_libao.get_index(7 + i * 4, id));
			_reward.value3 = int.Parse(game_data._instance.m_dbc_week_libao.get_index(8 + i * 4, id));
			if(_reward.type > 0)
			{
				_t_week_libao.rewards.Add(_reward);
			}
		}
		_t_week_libao.jewel = int.Parse(game_data._instance.m_dbc_week_libao.get_index(21,id));
		_t_week_libao.num = int.Parse(game_data._instance.m_dbc_week_libao.get_index(22,id));
		_t_week_libao.discount = float.Parse(game_data._instance.m_dbc_week_libao.get_index(23,id));

		return _t_week_libao;
	}

	public s_t_guild_shop_xs get_t_guild_shop_xs(int id)
	{
		s_t_guild_shop_xs _t_guild_shop_xg = new s_t_guild_shop_xs();
		if (!game_data._instance.m_dbc_guild_shop_xs.has_index(id))
		{
			return null;
		}
		_t_guild_shop_xg.id = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(0,id));
		_t_guild_shop_xg.name = game_data._instance.m_dbc_guild_shop_xs.get_index(1,id);
		_t_guild_shop_xg.grid = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(2,id));
		_t_guild_shop_xg.level = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(3,id));
		_t_guild_shop_xg.type = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(4,id));
		_t_guild_shop_xg.value1 = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(5,id));
		_t_guild_shop_xg.value2 = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(6,id));
		_t_guild_shop_xg.value3 = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(7,id));
		_t_guild_shop_xg.jewel = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(8,id));
		_t_guild_shop_xg.num = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(10,id));
		_t_guild_shop_xg.discount = int.Parse(game_data._instance.m_dbc_guild_shop_xs.get_index(11,id));
		
		return _t_guild_shop_xg;
	}

	private void parse_t_itemhecheng()
	{
		m_dbc_itemhecheng.load_txt("t_itemhecheng",0);
		foreach (int id in m_dbc_itemhecheng.m_index.Keys)
		{
			s_t_itemhecheng _t_itemhecheng = new s_t_itemhecheng ();
			_t_itemhecheng.id = int.Parse (m_dbc_itemhecheng.get_index (0, id));
			_t_itemhecheng.fenye = int.Parse (m_dbc_itemhecheng.get_index (1, id));
			_t_itemhecheng.type = int.Parse (m_dbc_itemhecheng.get_index (2, id));
			_t_itemhecheng.item_id = int.Parse (m_dbc_itemhecheng.get_index (3, id));
			_t_itemhecheng.item_num = int.Parse (m_dbc_itemhecheng.get_index (4, id));
			for(int i = 0;i < 3;++i)
			{
				int type = int.Parse (m_dbc_itemhecheng.get_index (5 + 3*i, id));
				int cl_id = int.Parse (m_dbc_itemhecheng.get_index (6 + 3*i, id));
				int num = int.Parse (m_dbc_itemhecheng.get_index (7 + 3*i, id));
				if(type > 0)
				{
					_t_itemhecheng.cl_type.Add(type);
					_t_itemhecheng.cl_id.Add(cl_id);
					_t_itemhecheng.cl_num.Add(num);
				}
			}
			m_itemhechengs.Add(id, _t_itemhecheng);
			if(_t_itemhecheng.fenye == 3)
			{
				m_pet_itemhechengs.Add(_t_itemhecheng.item_id,_t_itemhecheng);
			}
		}
	}

	public s_t_itemhecheng get_t_itemhecheng(int id)
	{
		if(m_itemhechengs.ContainsKey(id))
		{
			return m_itemhechengs[id];
		}
		return null;
	}

	public s_t_itemhecheng get_t_pet_itemhecheng(int id)
	{
		if(m_pet_itemhechengs.ContainsKey(id))
		{
			return m_pet_itemhechengs[id];
		}
		return null;
	}

	public s_t_role_shengpin get_t_role_shengpin(int id)
	{
		s_t_role_shengpin _t_role_shengpin = new s_t_role_shengpin ();
		if (!game_data._instance.m_dbc_role_shengpin.has_index(id))
		{
			return null;
		}

		_t_role_shengpin.pinzhi = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(0,id));
		_t_role_shengpin.name = get_t_language(game_data._instance.m_dbc_role_shengpin.get_index(2,id));
		_t_role_shengpin.next_pinzhi = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(3,id));
		_t_role_shengpin.level = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(4,id));
		for (int j = 0; j < 5; ++j)
		{
			float cs = float.Parse(game_data._instance.m_dbc_role_shengpin.get_index(j + 5,id));
			_t_role_shengpin.cs.Add(cs);
		}
		for (int j = 0; j < 5; ++j)
		{
			float cz = float.Parse(game_data._instance.m_dbc_role_shengpin.get_index(j + 10,id));
			_t_role_shengpin.cz.Add(cz);
		}
		_t_role_shengpin.color = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(15,id));
		_t_role_shengpin.zdjnjc = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(16,id));
		_t_role_shengpin.bdjnjc = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(17,id));
		_t_role_shengpin.shengpinshi = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(18,id));
		_t_role_shengpin.zhanhun = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(19,id));
		_t_role_shengpin.hongsehuobanzhili = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(20,id));
		_t_role_shengpin.suipian = int.Parse(game_data._instance.m_dbc_role_shengpin.get_index(21,id));
		_t_role_shengpin.gold = int.Parse (game_data._instance.m_dbc_role_shengpin.get_index (22, id));
		return _t_role_shengpin;
	}

	public s_t_duixng get_t_duixing(int id)
	{
		s_t_duixng _t_duixing = new s_t_duixng ();
		if (!game_data._instance.m_dbc_duixing.has_index(id))
		{
			return null;
		}
		
		_t_duixing.id = int.Parse(game_data._instance.m_dbc_duixing.get_index(0,id));
		_t_duixing.name = get_t_language(game_data._instance.m_dbc_duixing.get_index(2,id));
		_t_duixing.type = int.Parse(game_data._instance.m_dbc_duixing.get_index(3,id));
		_t_duixing.level = int.Parse(game_data._instance.m_dbc_duixing.get_index(4,id));
		for (int j = 0; j < 6; ++j)
		{
			int zhenwei = int.Parse(game_data._instance.m_dbc_duixing.get_index(j + 5,id));
			_t_duixing.zhenwei.Add(zhenwei);
		}
		_t_duixing.q_attr = int.Parse(game_data._instance.m_dbc_duixing.get_index(11,id));
		_t_duixing.q_value = float.Parse(game_data._instance.m_dbc_duixing.get_index(12,id));
		_t_duixing.q_cz = float.Parse(game_data._instance.m_dbc_duixing.get_index(13,id));
		_t_duixing.q_attr1 = int.Parse(game_data._instance.m_dbc_duixing.get_index(14,id));
		_t_duixing.q_value1 = float.Parse(game_data._instance.m_dbc_duixing.get_index(15,id));
		_t_duixing.q_cz1 = float.Parse(game_data._instance.m_dbc_duixing.get_index(16,id));
		_t_duixing.z_attr = int.Parse(game_data._instance.m_dbc_duixing.get_index(17,id));
		_t_duixing.z_value = float.Parse(game_data._instance.m_dbc_duixing.get_index(18,id));
		_t_duixing.z_cz = float.Parse(game_data._instance.m_dbc_duixing.get_index(19,id));
		_t_duixing.z_attr1 = int.Parse(game_data._instance.m_dbc_duixing.get_index(20,id));
		_t_duixing.z_value1 = float.Parse(game_data._instance.m_dbc_duixing.get_index(21,id));
		_t_duixing.z_cz1 = float.Parse(game_data._instance.m_dbc_duixing.get_index(22,id));
		_t_duixing.h_attr = int.Parse(game_data._instance.m_dbc_duixing.get_index(23,id));
		_t_duixing.h_value = float.Parse(game_data._instance.m_dbc_duixing.get_index(24,id));
		_t_duixing.h_cz = float.Parse(game_data._instance.m_dbc_duixing.get_index(25,id));
		_t_duixing.h_attr1 = int.Parse(game_data._instance.m_dbc_duixing.get_index(26,id));
		_t_duixing.h_value1 = float.Parse(game_data._instance.m_dbc_duixing.get_index(27,id));
		_t_duixing.h_cz1 = float.Parse(game_data._instance.m_dbc_duixing.get_index(28,id));

		return _t_duixing;
	}

	public s_t_duixng_up get_t_duixing_up(int level)
	{
		s_t_duixng_up _t_duixing_up = new s_t_duixng_up ();
		if (!game_data._instance.m_dbc_duixing_up.has_index(level))
		{
			return null;
		}
		
		_t_duixing_up.level1 = int.Parse(game_data._instance.m_dbc_duixing_up.get_index(0,level));
		_t_duixing_up.level2 = int.Parse(game_data._instance.m_dbc_duixing_up.get_index(1,level));
		_t_duixing_up.cl = int.Parse(game_data._instance.m_dbc_duixing_up.get_index(2,level));
		_t_duixing_up.gold = int.Parse(game_data._instance.m_dbc_duixing_up.get_index(3,level));

		return _t_duixing_up;
	}

	public s_t_duixng_skill get_t_duixing_skill(int id)
	{
		s_t_duixng_skill _t_duixing_skill = new s_t_duixng_skill ();
		if (!game_data._instance.m_dbc_duixing_skill.has_index(id))
		{
			return null;
		}
		
		_t_duixing_skill.id = int.Parse(game_data._instance.m_dbc_duixing_skill.get_index(0,id));
		_t_duixing_skill.name = get_t_language(game_data._instance.m_dbc_duixing_skill.get_index(2,id));
		_t_duixing_skill.desc = get_t_language(game_data._instance.m_dbc_duixing_skill.get_index(4,id));
		_t_duixing_skill.level = int.Parse(game_data._instance.m_dbc_duixing_skill.get_index(5,id));
		for(int i = 0; i < 4;++i)
		{
			s_t_attr sx = new s_t_attr();
			sx.attr = int.Parse(game_data._instance.m_dbc_duixing_skill.get_index(6 + i * 2,id));
			sx.value = int.Parse(game_data._instance.m_dbc_duixing_skill.get_index(7 + i * 2,id));
			if (sx.attr > 0)
			{
				_t_duixing_skill.attrs.Add(sx);
			}
		}
		
		return _t_duixing_skill;
	}

	private void parse_t_tanbao_event()
	{
		m_dbc_tanbao_event.load_txt("t_tanbao_event",0);
		foreach (int id in m_dbc_tanbao_event.m_index.Keys)
		{
			s_t_tanbao_event t_tanbao = new s_t_tanbao_event();
			t_tanbao.site = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(0,id));
			t_tanbao.type =  int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(2,id));
			t_tanbao.shop_type = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(3,id));
			t_tanbao.name = get_t_language(game_data._instance.m_dbc_tanbao_event.get_index(4,id));
			t_tanbao.rtype = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(5,id));
			t_tanbao.rvalue1 = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(6,id));
			t_tanbao.rvalue2 = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(7,id));
			t_tanbao.rvalue3 = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(8,id));
			for(int i = 0;i < 5;++i)
			{
				string juqing =  game_data._instance.m_dbc_tanbao_event.get_index(9 + 7*i,id);
				if(juqing != "")
				{
					t_tanbao.juqings.Add(juqing);
				}
			}
			for(int i = 0;i < 5;++i)
			{
				s_t_reward reward = new s_t_reward();
				reward.type = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(11 + 7*i,id));
				reward.value1 = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(12 + 7*i,id));
				reward.value2 = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(13 + 7*i,id));
				reward.value3 = int.Parse(game_data._instance.m_dbc_tanbao_event.get_index(14 + 7*i,id));
				t_tanbao.rewards.Add(reward);
			}

			m_tanbaos.Add(id, t_tanbao);
		}
	}

	public s_t_tanbao_event get_t_tanbao(int site)
	{
		if(m_tanbaos.ContainsKey(site))
		{
			return m_tanbaos[site];
		}
		return null;
	}

	private void parse_t_tanbao_mubiao()
	{
		m_dbc_tanbao_mubiao.load_txt("t_tanbao_mubiao",0);
		foreach (int id in m_dbc_tanbao_mubiao.m_index.Keys)
		{
			s_t_tanbao_mubiao t_tanbao_mubiao = new s_t_tanbao_mubiao();
			t_tanbao_mubiao.id = int.Parse(game_data._instance.m_dbc_tanbao_mubiao.get_index(0,id));
			t_tanbao_mubiao.task_num =  int.Parse(game_data._instance.m_dbc_tanbao_mubiao.get_index(1,id));
			t_tanbao_mubiao.name = get_t_language(game_data._instance.m_dbc_tanbao_mubiao.get_index(2,id));
			t_tanbao_mubiao.type = int.Parse(game_data._instance.m_dbc_tanbao_mubiao.get_index(3,id));
			t_tanbao_mubiao.value1 = int.Parse(game_data._instance.m_dbc_tanbao_mubiao.get_index(4,id));
			t_tanbao_mubiao.value2 = int.Parse(game_data._instance.m_dbc_tanbao_mubiao.get_index(5,id));
			t_tanbao_mubiao.value3 = int.Parse(game_data._instance.m_dbc_tanbao_mubiao.get_index(6,id));
			
			m_tanbao_mubiaos.Add(id, t_tanbao_mubiao);
		}
	}

	public s_t_tanbao_mubiao get_t_tanbao_mubiao(int id)
	{
		if(m_tanbao_mubiaos.ContainsKey(id))
		{
			return m_tanbao_mubiaos[id];
		}
		return null;
	}

	private void parse_t_tanbao_shop()
	{
		m_dbc_tanbao_shop.load_txt("t_tanbao_shop",0);
		foreach (int id in m_dbc_tanbao_shop.m_index.Keys)
		{
			s_t_tanbao_shop t_tanbao_shop = new s_t_tanbao_shop();
			t_tanbao_shop.id = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(0,id));
			t_tanbao_shop.name = get_t_language(game_data._instance.m_dbc_tanbao_shop.get_index(1,id));
			t_tanbao_shop.shop_type = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(2,id));
			t_tanbao_shop.rtype = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(3,id));
			t_tanbao_shop.rvalue1 = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(4,id));
			t_tanbao_shop.rvalue2 = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(5,id));
			t_tanbao_shop.rvalue3 = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(6,id));
			t_tanbao_shop.price = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(7,id));
			t_tanbao_shop.discount = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(8,id));
			t_tanbao_shop.score = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(9,id));
			t_tanbao_shop.buy_num = int.Parse(game_data._instance.m_dbc_tanbao_shop.get_index(10,id));
			m_tanbao_shops.Add(id, t_tanbao_shop);
		}
	}

	public s_t_tanbao_shop get_t_tanbao_shop(int id)
	{
		if(m_tanbao_shops.ContainsKey(id))
		{
			return m_tanbao_shops[id];
		}
		return null;
	}

	public s_t_tanbao_reward get_t_tanbao_reward(int rank,int type)
	{
		int tmp = rank * 1000 + type;
		if (m_tanbao_rewards.ContainsKey(tmp))
		{
			return m_tanbao_rewards[tmp];
		}
		s_t_tanbao_reward _tanbao_reward = new s_t_tanbao_reward ();
		for(int j = 0;j < game_data._instance.m_dbc_tanbao_reward.get_y();++j)
		{
			if(int.Parse (m_dbc_tanbao_reward.get (0, j))== rank && int.Parse (m_dbc_tanbao_reward.get (2, j)) == type)
			{
				_tanbao_reward.rank1 = int.Parse (m_dbc_tanbao_reward.get (0, j));
				_tanbao_reward.rank2 = int.Parse (m_dbc_tanbao_reward.get (1, j));
				_tanbao_reward.type = int.Parse (m_dbc_tanbao_reward.get (2, j));
				for(int i = 0 ;i < 3;++i)
				{
					s_t_reward reward = new s_t_reward();
					reward.type = int.Parse (m_dbc_tanbao_reward.get (3+4*i, j));
					reward.value1 = int.Parse (m_dbc_tanbao_reward.get (4+4*i, j));
					reward.value2 = int.Parse (m_dbc_tanbao_reward.get (5+4*i, j));
					reward.value3 = int.Parse (m_dbc_tanbao_reward.get (6+4*i, j));
					if(reward.type > 0)
					{
						_tanbao_reward.rewards.Add(reward);
					}
				}
				m_tanbao_rewards[tmp] = _tanbao_reward;
				return _tanbao_reward;		
			}
		}
		return null;
	}
    private void parse_t_bingyuan_chenghao()
    {
        m_dbc_bingyuan_chenhao.load_txt("t_bingyuan_chenghao",0);
        foreach (int id in m_dbc_bingyuan_chenhao.m_index.Keys)
        {
            s_t_bingyuan_chenhao _chenghao = new s_t_bingyuan_chenhao();
            _chenghao.id = id;
            _chenghao.rank = int.Parse(m_dbc_bingyuan_chenhao.get_index(2, id));
            _chenghao.jifen = int.Parse(m_dbc_bingyuan_chenhao.get_index(3, id));
            _chenghao.chenhaoid = int.Parse(m_dbc_bingyuan_chenhao.get_index(4, id));
            m_bingyuan_chenhaos.Add(id,_chenghao); 
        }
    }
    public s_t_bingyuan_chenhao get_t_bingyuan_chenghao(int id)
    {
        if (m_bingyuan_chenhaos.ContainsKey(id))
        {
            return m_bingyuan_chenhaos[id];
        }
        return null;

    }
    private void parse_t_chenghao()
    {
        m_dbc_cheng_hao.load_txt("t_chenghao", 0);
        foreach (int id in m_dbc_cheng_hao.m_index.Keys)
        {
            s_t_chenghao _chenghao = new s_t_chenghao();
            _chenghao.id = id;
            _chenghao.type = int.Parse(m_dbc_cheng_hao.get_index(1, id));
            _chenghao.color = m_dbc_cheng_hao.get_index(3, id);
			_chenghao.color_effect = m_dbc_cheng_hao.get_index(4, id);
			_chenghao.icon1 = m_dbc_cheng_hao.get_index(5, id);
			_chenghao.icon2 = m_dbc_cheng_hao.get_index(6, id);
            string temp = m_dbc_cheng_hao.get_index(7, id);
            _chenghao.name = _chenghao.color + get_t_language(temp);
			_chenghao.condition = get_t_language(m_dbc_cheng_hao.get_index(8, id));
			_chenghao.desc = get_t_language(m_dbc_cheng_hao.get_index(9, id));
			for(int i = 0; i < 5; ++i)
			{
				s_t_attr attr = new s_t_attr();
				attr.attr = int.Parse(m_dbc_cheng_hao.get_index(10 + 2*i, id));
				attr.value = int.Parse(m_dbc_cheng_hao.get_index(11 + 2*i,id));
				if(attr.attr > 0)
				{
					_chenghao.attr.Add(attr);
				}
			}
			_chenghao.is_show = int.Parse(m_dbc_cheng_hao.get_index(20, id));
			_chenghao.time = int.Parse(m_dbc_cheng_hao.get_index(21, id));
            m_chenhao_s.Add(id,_chenghao);
        }
    }
    public s_t_chenghao get_t_chenhao(int id)
    {
        if (m_chenhao_s.ContainsKey(id))
        {
            return m_chenhao_s[id];
        }
        return null;
    }
    private void parse_t_guanghuan()
	{
		m_dbc_guanghuan.load_txt("t_guanghuan",0);
		foreach (int id in m_dbc_guanghuan.m_index.Keys)
		{
			s_t_guanghuan t_guanghuan = new s_t_guanghuan();
			t_guanghuan.id = int.Parse(game_data._instance.m_dbc_guanghuan.get_index(0,id));
			t_guanghuan.name = get_t_language(game_data._instance.m_dbc_guanghuan.get_index(2,id));
			t_guanghuan.color = int.Parse(game_data._instance.m_dbc_guanghuan.get_index(3,id));
			t_guanghuan.icon = game_data._instance.m_dbc_guanghuan.get_index(4,id);
			t_guanghuan.effect = game_data._instance.m_dbc_guanghuan.get_index(5,id);
			t_guanghuan.attr1 = int.Parse(game_data._instance.m_dbc_guanghuan.get_index(6,id));
			t_guanghuan.value1 = int.Parse(game_data._instance.m_dbc_guanghuan.get_index(7,id));
			t_guanghuan.attr2 = int.Parse(game_data._instance.m_dbc_guanghuan.get_index(8,id));
			t_guanghuan.value2 = int.Parse(game_data._instance.m_dbc_guanghuan.get_index(9,id));
			m_guanghuans.Add(id, t_guanghuan);
		}
	}

	public s_t_guanghuan get_t_guanghuan(int id)
	{
		if(m_guanghuans.ContainsKey(id))
		{
			return m_guanghuans[id];
		}
		return null;
	}
    void parse_t_mofang()
    {
        m_dbc_mofang.load_txt("t_mofang",0);
        foreach (int id in m_dbc_mofang.m_index.Keys)
        {
            s_t_mofang mofang = new s_t_mofang();
            mofang.id = id;
            mofang.leixing = int.Parse(m_dbc_mofang.get_index(2, id));
            mofang.type = int.Parse(m_dbc_mofang.get_index(3, id));
            mofang.value1 = int.Parse(m_dbc_mofang.get_index(4, id));
            mofang.value2 = int.Parse(m_dbc_mofang.get_index(5, id));
            mofang.value3 = int.Parse(m_dbc_mofang.get_index(6, id));
            m_mofangs.Add(id,mofang);
        }
    }
    public s_t_mofang get_t_mofang(int id)
    {
        if (m_mofangs.ContainsKey(id))
        {
            return m_mofangs[id];
 
        }
        return null;

    }
    void parse_t_mofang_shop()
    {
        m_dbc_mofang_shop.load_txt("t_mofang_shop",0);
        foreach (int id in m_dbc_mofang_shop.m_index.Keys)
        {
            s_t_mofang_shop shop = new s_t_mofang_shop();
            shop.id = id;
            shop.type = int.Parse(m_dbc_mofang_shop.get_index(2, id));
            shop.value1 = int.Parse(m_dbc_mofang_shop.get_index(3, id));
            shop.value2 = int.Parse(m_dbc_mofang_shop.get_index(4, id));
            shop.value3 = int.Parse(m_dbc_mofang_shop.get_index(5, id));
            shop.price = int.Parse(m_dbc_mofang_shop.get_index(6, id));
            shop.buycount = int.Parse(m_dbc_mofang_shop.get_index(7, id));
            m_mofang_shops.Add(id,shop);
        }
    }
    public s_t_mofang_shop get_t_mofang_shop(int id)
    {
        if (m_mofang_shops.ContainsKey(id))
        {
            return m_mofang_shops[id];
        }
        return null;
    }
    void parse_t_mofang_reward()
    {
        m_dbc_mofang_reward.load_txt("t_mofang_reward", 0);
        foreach (int id in m_dbc_mofang_reward.m_index.Keys)
        {
            s_t_mofang_reward re = new s_t_mofang_reward();
            re.id = id;
            re.name = get_t_language(m_dbc_mofang_reward.get_index(2, id));
            re.need_jifen = int.Parse(m_dbc_mofang_reward.get_index(3, id));
            re.type = int.Parse(m_dbc_mofang_reward.get_index(4, id));
            re.value1 = int.Parse(m_dbc_mofang_reward.get_index(5, id));
            re.value2 = int.Parse(m_dbc_mofang_reward.get_index(6, id));
            re.value3 = int.Parse(m_dbc_mofang_reward.get_index(7, id));
            m_mofang_rewards.Add(id, re);
        }
    }
    public s_t_mofang_reward get_t_mofang_reward(int id)
    {
        if (m_mofang_rewards.ContainsKey(id))
        {
            return m_mofang_rewards[id];
        }
        return null;
    }
	private void parse_t_guanghuan_enhance()
	{
		m_dbc_guanghuan_enhance.load_txt("t_guanghuan_enhance",0);
		foreach (int id in m_dbc_guanghuan_enhance.m_index.Keys)
		{
			s_t_guanghuan_enhance t_guanghuan_enhance = new s_t_guanghuan_enhance();
			t_guanghuan_enhance.level = int.Parse(game_data._instance.m_dbc_guanghuan_enhance.get_index(0,id));
			for(int i = 0 ;i < 4;++i)
			{
				int gold = int.Parse(game_data._instance.m_dbc_guanghuan_enhance.get_index(1 + 2*i,id));
				t_guanghuan_enhance.golds.Add(gold);
				int yuansu = int.Parse(game_data._instance.m_dbc_guanghuan_enhance.get_index(2 + 2*i,id));
				t_guanghuan_enhance.yuansus.Add(yuansu);
			}
			m_guanghuan_enhances.Add(id, t_guanghuan_enhance);
		}
	}

	public s_t_guanghuan_enhance get_t_guanghuan_enhance(int id)
	{
		if(m_guanghuan_enhances.ContainsKey(id))
		{
			return m_guanghuan_enhances[id];
		}
		return null;
	}

	private void parse_t_guanghuan_skill()
	{
		m_dbc_guanghuan_skill.load_txt("t_guanghuan_skill",0);
		foreach (int id in m_dbc_guanghuan_skill.m_index.Keys)
		{
			s_t_guanghuan_skill t_guanghuan_skill = new s_t_guanghuan_skill();
			t_guanghuan_skill.id = int.Parse(game_data._instance.m_dbc_guanghuan_skill.get_index(0,id));
			t_guanghuan_skill.name = get_t_language(game_data._instance.m_dbc_guanghuan_skill.get_index(2,id));
			t_guanghuan_skill.wing_id = int.Parse(game_data._instance.m_dbc_guanghuan_skill.get_index(3,id));
			t_guanghuan_skill.enhance = int.Parse(game_data._instance.m_dbc_guanghuan_skill.get_index(4,id));
			t_guanghuan_skill.type = int.Parse(game_data._instance.m_dbc_guanghuan_skill.get_index(5,id));
			t_guanghuan_skill.def1 = int.Parse(game_data._instance.m_dbc_guanghuan_skill.get_index(6,id));
			t_guanghuan_skill.def2 = int.Parse(game_data._instance.m_dbc_guanghuan_skill.get_index(7,id));
			t_guanghuan_skill.desc = get_t_language(game_data._instance.m_dbc_guanghuan_skill.get_index(9,id));
			m_guanghuan_skills.Add(id, t_guanghuan_skill);
			if (!m_guanghuan_skill_ids.ContainsKey(t_guanghuan_skill.wing_id))
			{
				m_guanghuan_skill_ids.Add(t_guanghuan_skill.wing_id, new List<int>());
			}
			m_guanghuan_skill_ids[t_guanghuan_skill.wing_id].Add(id);
		}
	}

	public s_t_guanghuan_skill get_t_guanghuan_skill(int id)
	{
		if(m_guanghuan_skills.ContainsKey(id))
		{
			return m_guanghuan_skills[id];
		}
		return null;
	}

	private void parse_t_guanghuan_target()
	{
		m_dbc_guanghuan_target.load_txt("t_guanghuan_target",0);
		foreach (int id in m_dbc_guanghuan_target.m_index.Keys)
		{
			s_t_guanghuan_target t_guanghuan_target = new s_t_guanghuan_target();
			t_guanghuan_target.id = int.Parse(game_data._instance.m_dbc_guanghuan_target.get_index(0,id));
			t_guanghuan_target.name = get_t_language(m_dbc_guanghuan_target.get_index(2,id));
			for(int i = 0; i < 3;++i)
			{
				int _id = int.Parse(m_dbc_guanghuan_target.get_index(3 + i,id));
				if(_id > 0)
				{
					t_guanghuan_target.ids.Add(_id);
				}
			}
			for(int i = 0; i < 4;++i)
			{
				s_t_attr attr = new s_t_attr();
				attr.attr = int.Parse(m_dbc_guanghuan_target.get_index(6 + 2*i,id));
				attr.value = int.Parse(m_dbc_guanghuan_target.get_index(7 + 2*i,id));
				if(attr.attr > 0)
				{
					t_guanghuan_target.attrs.Add(attr);
				}
			}
			m_guanghuan_targets.Add(id, t_guanghuan_target);
		}
	}

	public s_t_guanghuan_target get_t_guanghuan_target(int id)
	{
		if(m_guanghuan_targets.ContainsKey(id))
		{
			return m_guanghuan_targets[id];
		}
		return null;
	}

	public s_t_baowu_sx get_t_baowu_sx(int level,int pz)
	{
		int tmp = pz * 1000 + level;
		if (m_baowu_sxs.ContainsKey(tmp))
		{
			return m_baowu_sxs[tmp];
		}
		s_t_baowu_sx _baowu_sx = new s_t_baowu_sx ();
		for(int j = 0;j < game_data._instance.m_dbc_baowu_sx.get_y();++j)
		{
			if(int.Parse (m_dbc_baowu_sx.get (0, j))== level && int.Parse (m_dbc_baowu_sx.get (1, j)) == pz)
			{
				_baowu_sx.level = int.Parse (m_dbc_baowu_sx.get (0, j));
				_baowu_sx.pz = int.Parse (m_dbc_baowu_sx.get (1, j));
				_baowu_sx.gold = int.Parse (m_dbc_baowu_sx.get (2, j));
				_baowu_sx.jewel = int.Parse (m_dbc_baowu_sx.get (3, j));
				_baowu_sx.process = int.Parse (m_dbc_baowu_sx.get (4, j));
				_baowu_sx.rate = int.Parse (m_dbc_baowu_sx.get (5, j));
				_baowu_sx.value1 = int.Parse (m_dbc_baowu_sx.get (6, j));
				_baowu_sx.value2 = int.Parse (m_dbc_baowu_sx.get (7, j));
				_baowu_sx.valuemax = int.Parse (m_dbc_baowu_sx.get (8, j));
				m_baowu_sxs[tmp] = _baowu_sx;
				return _baowu_sx;		
			}
		}
		return null;
	}

	private void parse_t_role_dresstarget()
	{
		m_dbc_role_dresstarget.load_txt("t_role_dresstarget",0);
		foreach (int id in m_dbc_role_dresstarget.m_index.Keys)
		{
			s_t_role_dresstarget t_role_dresstarget = new s_t_role_dresstarget();
			t_role_dresstarget.id = int.Parse(game_data._instance.m_dbc_role_dresstarget.get_index(0,id));
			t_role_dresstarget.name = get_t_language(m_dbc_role_dresstarget.get_index(2,id));
			t_role_dresstarget.desc = get_t_language(m_dbc_role_dresstarget.get_index(3,id));
			for(int i = 0; i < 5;++i)
			{
				int _id = int.Parse(m_dbc_role_dresstarget.get_index(4 + i,id));
				if(_id > 0)
				{
					t_role_dresstarget.ids.Add(_id);
				}
			}
			for(int i = 0; i < 4;++i)
			{
				s_t_attr attr = new s_t_attr();
				attr.attr = int.Parse(m_dbc_role_dresstarget.get_index(9 + 2*i,id));
				attr.value = int.Parse(m_dbc_role_dresstarget.get_index(10 + 2*i,id));
				if(attr.attr > 0)
				{
					t_role_dresstarget.attrs.Add(attr);
				}
			}
			m_role_dresstargets.Add(id, t_role_dresstarget);
		}
	}

	public s_t_role_dresstarget get_t_role_dresstarget(int id)
	{
		if(m_role_dresstargets.ContainsKey(id))
		{
			return m_role_dresstargets[id];
		}
		return null;
	}

	private void parse_t_chongzhifanpai()
	{
		m_dbc_chongzhifanpai.load_txt("t_chongzhifanpai",0);
		foreach (int id in m_dbc_chongzhifanpai.m_index.Keys)
		{
			s_t_chongzhifanpai t_chongzhifanpai = new s_t_chongzhifanpai();
			t_chongzhifanpai.id = int.Parse(game_data._instance.m_dbc_chongzhifanpai.get_index(0,id));
			t_chongzhifanpai.type = int.Parse(m_dbc_chongzhifanpai.get_index(1,id));
			t_chongzhifanpai.jewel = int.Parse(m_dbc_chongzhifanpai.get_index(2,id));
			t_chongzhifanpai.rate = int.Parse(m_dbc_chongzhifanpai.get_index(3,id));
			m_chongzhifanpais.Add(id, t_chongzhifanpai);
		}
	}

	public s_t_chongzhifanpai get_t_chongzhifanpai(int id)
	{
		if(m_chongzhifanpais.ContainsKey(id))
		{
			return m_chongzhifanpais[id];
		}
		return null;
	}

	private void parse_t_chongzhifanpai_shop()
	{
		m_dbc_chongzhifanpai_shop.load_txt("t_chongzhifanpai_shop",0);
		foreach (int id in m_dbc_chongzhifanpai_shop.m_index.Keys)
		{
			s_t_chongzhifanpai_shop t_chongzhifanpai_shop = new s_t_chongzhifanpai_shop();
			t_chongzhifanpai_shop.id = int.Parse(game_data._instance.m_dbc_chongzhifanpai_shop.get_index(0,id));
			t_chongzhifanpai_shop.type = int.Parse(m_dbc_chongzhifanpai_shop.get_index(1,id));
			t_chongzhifanpai_shop.value1 = int.Parse(m_dbc_chongzhifanpai_shop.get_index(2,id));
			t_chongzhifanpai_shop.value2 = int.Parse(m_dbc_chongzhifanpai_shop.get_index(3,id));
			t_chongzhifanpai_shop.value3 = int.Parse(m_dbc_chongzhifanpai_shop.get_index(4,id));
			t_chongzhifanpai_shop.name = m_dbc_chongzhifanpai_shop.get_index(5,id);
			t_chongzhifanpai_shop.price = int.Parse(m_dbc_chongzhifanpai_shop.get_index(6,id));
			m_chongzhifanpai_shops.Add(id, t_chongzhifanpai_shop);
		}
	}

	public s_t_chongzhifanpai_shop get_t_chongzhifanpai_shop(int id)
	{
		if(m_chongzhifanpai_shops.ContainsKey(id))
		{
			return m_chongzhifanpai_shops[id];
		}
		return null;
	}

	private void parse_t_zhuanpan()
	{
		m_dbc_zhuanpan.load_txt("t_zhuanpan",0);
		foreach (int id in m_dbc_zhuanpan.m_index.Keys)
		{
			s_t_zhuanpan t_zhuanpan = new s_t_zhuanpan();
			t_zhuanpan.id = int.Parse(game_data._instance.m_dbc_zhuanpan.get_index(0,id));
			t_zhuanpan.zhuanpan_type = int.Parse(m_dbc_zhuanpan.get_index(1,id));
			t_zhuanpan.type = int.Parse(m_dbc_zhuanpan.get_index(2,id));
			t_zhuanpan.value1 = int.Parse(m_dbc_zhuanpan.get_index(3,id));
			t_zhuanpan.value2 = int.Parse(m_dbc_zhuanpan.get_index(4,id));
			t_zhuanpan.value3 = int.Parse(m_dbc_zhuanpan.get_index(5,id));
			t_zhuanpan.is_flash = int.Parse(m_dbc_zhuanpan.get_index(14,id));
			m_zhuanpans.Add(id, t_zhuanpan);
		}
	}

	public s_t_zhuanpan get_t_zhuanpan(int id)
	{
		if(m_zhuanpans.ContainsKey(id))
		{
			return m_zhuanpans[id];
		}
		return null;
	}

	public s_t_zhuanpan_reward get_t_zhuanpan_reward(int rank,int type)
	{
		int tmp = rank * 1000 + type;
		if (m_zhuanpan_rewards.ContainsKey(tmp))
		{
			return m_zhuanpan_rewards[tmp];
		}
		s_t_zhuanpan_reward t_zhuanpan_reward = new s_t_zhuanpan_reward ();
		for(int j = 0;j < game_data._instance.m_dbc_zhuanpan_reward.get_y();++j)
		{
			if(int.Parse (m_dbc_zhuanpan_reward.get (0, j))== rank && int.Parse (m_dbc_zhuanpan_reward.get (2, j)) == type)
			{
				t_zhuanpan_reward.rank1 = int.Parse (m_dbc_zhuanpan_reward.get (0, j));
				t_zhuanpan_reward.rank2 = int.Parse (m_dbc_zhuanpan_reward.get (1, j));
				t_zhuanpan_reward.type = int.Parse (m_dbc_zhuanpan_reward.get (2, j));
				for(int i = 0 ;i < 3;++i)
				{
					s_t_reward reward = new s_t_reward();
					reward.type = int.Parse (m_dbc_zhuanpan_reward.get (3+4*i, j));
					reward.value1 = int.Parse (m_dbc_zhuanpan_reward.get (4+4*i, j));
					reward.value2 = int.Parse (m_dbc_zhuanpan_reward.get (5+4*i, j));
					reward.value3 = int.Parse (m_dbc_zhuanpan_reward.get (6+4*i, j));
					if(reward.type > 0)
					{
						t_zhuanpan_reward.rewards.Add(reward);
					}
				}
				m_zhuanpan_rewards[tmp] = t_zhuanpan_reward;
				return t_zhuanpan_reward;		
			}
		}
		return null;
	}

	private void parse_t_manyou_dati()
	{
		m_dbc_manyou_dati.load_txt("t_manyou_dati",0);
		foreach (int id in m_dbc_manyou_dati.m_index.Keys)
		{
			s_t_manyou_dati t_manyou_dati = new s_t_manyou_dati();
			t_manyou_dati.id = int.Parse(game_data._instance.m_dbc_manyou_dati.get_index(0,id));
			t_manyou_dati.name = m_dbc_manyou_dati.get_index(1,id);
			t_manyou_dati.half_image = m_dbc_manyou_dati.get_index(2,id);
			t_manyou_dati.question = get_t_language(m_dbc_manyou_dati.get_index(4,id));
			for(int i = 0; i < 3;++i)
			{
				s_t_question t_quewstion = new s_t_question();
				t_quewstion.id = int.Parse(game_data._instance.m_dbc_manyou_dati.get_index(5+i*3,id));
                t_quewstion.answer = get_t_language (game_data._instance.m_dbc_manyou_dati.get_index(7 + i * 3, id));
				t_manyou_dati.questions.Add(t_quewstion);
			}
			m_manyou_datis.Add(id, t_manyou_dati);
		}
	}

	public s_t_manyou_dati get_t_manyou_dati(int id)
	{
		if(m_manyou_datis.ContainsKey(id))
		{
			return m_manyou_datis[id];
		}
		return null;
	}

	private void parse_t_manyou_qiyu()
	{
		m_dbc_manyou_qiyu.load_txt("t_manyou_qiyu",0);
		foreach (int id in m_dbc_manyou_qiyu.m_index.Keys)
		{
			s_t_manyou_qiyu t_manyou_qiyu = new s_t_manyou_qiyu();
			t_manyou_qiyu.id = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(0,id));
			t_manyou_qiyu.name = get_t_language(m_dbc_manyou_qiyu.get_index(2,id));
			t_manyou_qiyu.type = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(3,id));
			t_manyou_qiyu.def1 = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(4,id));
			t_manyou_qiyu.def2 = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(5,id));
			t_manyou_qiyu.def3 = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(6,id));
			for(int i = 0; i < 3;++i)
			{
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(7+i*4,id));
				t_reward.value1 = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(8+i*4,id));
				t_reward.value2 = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(9+i*4,id));
				t_reward.value3 = int.Parse(game_data._instance.m_dbc_manyou_qiyu.get_index(10+i*4,id));
				if(t_reward.type > 0)
				{
					t_manyou_qiyu.rewards.Add(t_reward);
				}
			}
			m_manyou_qiyus.Add(id, t_manyou_qiyu);
		}
	}

	public s_t_manyou_qiyu get_t_manyou_qiyu(int id)
	{
		if(m_manyou_qiyus.ContainsKey(id))
		{
			return m_manyou_qiyus[id];
		}
		return null;
	}

	private void parse_t_manyou()
	{
		m_dbc_manyou.load_txt("t_manyou",0);
		foreach (int id in m_dbc_manyou.m_index.Keys)
		{
			s_t_manyou t_manyou = new s_t_manyou();
			t_manyou.id = int.Parse(game_data._instance.m_dbc_manyou.get_index(0,id));
			t_manyou.name = get_t_language(m_dbc_manyou.get_index(2,id));
			t_manyou.type = int.Parse(game_data._instance.m_dbc_manyou.get_index(3,id));
			t_manyou.reward.type = int.Parse(game_data._instance.m_dbc_manyou.get_index(4,id));
			t_manyou.reward.value1 = int.Parse(game_data._instance.m_dbc_manyou.get_index(5,id));
			t_manyou.reward.value2 = int.Parse(game_data._instance.m_dbc_manyou.get_index(6,id));
			t_manyou.reward.value3 = int.Parse(game_data._instance.m_dbc_manyou.get_index(7,id));
			t_manyou.image = m_dbc_manyou.get_index(9,id);
			m_manyous.Add(id, t_manyou);
		}
	}

	public s_t_manyou get_t_manyou(int id)
	{
		if(m_manyous.ContainsKey(id))
		{
			return m_manyous[id];
		}
		return null;
	}

	private void parse_t_manyou_mubiao()
	{
		m_dbc_manyou_mubiao.load_txt("t_manyou_mubiao",0);
		foreach (int id in m_dbc_manyou_mubiao.m_index.Keys)
		{
			s_t_manyou_mubiao t_manyou_mubiao = new s_t_manyou_mubiao();
			t_manyou_mubiao.id = int.Parse(game_data._instance.m_dbc_manyou_mubiao.get_index(0,id));
			t_manyou_mubiao.score = int.Parse(game_data._instance.m_dbc_manyou_mubiao.get_index(1,id));
			t_manyou_mubiao.name = game_data._instance.m_dbc_manyou_mubiao.get_index(2,id);
			t_manyou_mubiao.reward.type = int.Parse(game_data._instance.m_dbc_manyou_mubiao.get_index(3,id));
			t_manyou_mubiao.reward.value1 = int.Parse(game_data._instance.m_dbc_manyou_mubiao.get_index(4,id));
			t_manyou_mubiao.reward.value2 = int.Parse(game_data._instance.m_dbc_manyou_mubiao.get_index(5,id));
			t_manyou_mubiao.reward.value3 = int.Parse(game_data._instance.m_dbc_manyou_mubiao.get_index(6,id));
			m_manyou_mubiaos.Add(id, t_manyou_mubiao);
		}
	}

	public s_t_manyou_mubiao get_t_manyou_mubiao(int id)
	{
		if(m_manyou_mubiaos.ContainsKey(id))
		{
			return m_manyou_mubiaos[id];
		}
		return null;
	}

	private void parse_t_manyou_reward()
	{
		m_dbc_manyou_reward.load_txt("t_manyou_reward",0);
		foreach (int id in m_dbc_manyou_reward.m_index.Keys)
		{
			s_t_manyou_reward t_manyou_reward = new s_t_manyou_reward ();
			t_manyou_reward.rank1 = int.Parse (m_dbc_manyou_reward.get_index (0, id));
			t_manyou_reward.rank2 = int.Parse (m_dbc_manyou_reward.get_index (1, id));
			for(int i = 0 ;i < 3;++i)
			{
				s_t_reward reward = new s_t_reward();
				reward.type = int.Parse (m_dbc_manyou_reward.get_index (2+4*i, id));
				reward.value1 = int.Parse (m_dbc_manyou_reward.get_index (3+4*i, id));
				reward.value2 = int.Parse (m_dbc_manyou_reward.get_index (4+4*i, id));
				reward.value3 = int.Parse (m_dbc_manyou_reward.get_index (5+4*i, id));
				if(reward.type > 0)
				{
					t_manyou_reward.rewards.Add(reward);
				}
			}
			m_manyou_rewards.Add(id, t_manyou_reward);
		}
	}

	public s_t_manyou_reward get_t_manyou_reward(int rank)
	{
		if(m_manyou_rewards.ContainsKey(rank))
		{
			return m_manyou_rewards[rank];
		}
		return null;
	}

	public s_t_role_skillunlock get_t_role_skillunlock(int role_id,int level)
	{
		int tmp = role_id * 1000 + level;
		if (m_role_skillunlocks.ContainsKey(tmp))
		{
			return m_role_skillunlocks[tmp];
		}
		s_t_role_skillunlock t_role_skillunlock = new s_t_role_skillunlock ();
		for(int j = 0;j < game_data._instance.m_dbc_role_skillunlock.get_y();++j)
		{
			if(int.Parse (m_dbc_role_skillunlock.get (3, j))== role_id && int.Parse (m_dbc_role_skillunlock.get (4, j)) == level)
			{
				t_role_skillunlock.id = int.Parse (m_dbc_role_skillunlock.get (0, j));
				t_role_skillunlock.name = get_t_language(m_dbc_role_skillunlock.get (2, j));
				t_role_skillunlock.taici = get_t_language(m_dbc_role_skillunlock.get (2, j));
				t_role_skillunlock.role_id = int.Parse (m_dbc_role_skillunlock.get (3, j));
				t_role_skillunlock.level = int.Parse (m_dbc_role_skillunlock.get (4, j));
				for(int i = 0 ;i < 5;++i)
				{
					s_t_role_skillunlock_task reward = new s_t_role_skillunlock_task();
					reward.task_type = int.Parse (m_dbc_role_skillunlock.get (5+4*i, j));
					reward.def1 = int.Parse (m_dbc_role_skillunlock.get (6+4*i, j));
					reward.def2 = int.Parse (m_dbc_role_skillunlock.get (7+4*i, j));
					reward.def3 = int.Parse (m_dbc_role_skillunlock.get (8+4*i, j));
					if(reward.task_type > 0)
					{
						t_role_skillunlock.role_skillunlock_tasks.Add(reward);
					}
				}
				m_role_skillunlocks[tmp] = t_role_skillunlock;
				return t_role_skillunlock;		
			}
		}
		return null;
	}

	private void parse_t_role_spskillup()
	{
		m_dbc_role_spskillup.load_txt("t_role_spskillup",0);
		foreach (int id in m_dbc_role_spskillup.m_index.Keys)
		{
			s_t_role_spskillup t_role_spskillup = new s_t_role_spskillup();
			t_role_spskillup.level = int.Parse(game_data._instance.m_dbc_role_spskillup.get_index(0,id));
			t_role_spskillup.stone = int.Parse(game_data._instance.m_dbc_role_spskillup.get_index(1,id));
			t_role_spskillup.gold = int.Parse(game_data._instance.m_dbc_role_spskillup.get_index(2,id));
			m_role_spskilups.Add(id, t_role_spskillup);
		}
	}
    void parse_t_chongwu_shop()
    {
        m_dbc_chongwu_shop.load_txt("t_chongwu_shop", 0);
        foreach (int id in m_dbc_chongwu_shop.m_index.Keys)
        {
            s_t_chongwu_shop t_chongwu_shop = new s_t_chongwu_shop();
            t_chongwu_shop.id = id;
            t_chongwu_shop.name = m_dbc_chongwu_shop.get_index(1, id);
            t_chongwu_shop.gezi = int.Parse(m_dbc_chongwu_shop.get_index(2, id));
            t_chongwu_shop.level = int.Parse(m_dbc_chongwu_shop.get_index(3, id));
            t_chongwu_shop.type = int.Parse(m_dbc_chongwu_shop.get_index(4, id));
            t_chongwu_shop.value1 = int.Parse(m_dbc_chongwu_shop.get_index(5, id));
            t_chongwu_shop.value2 = int.Parse(m_dbc_chongwu_shop.get_index(6, id));
            t_chongwu_shop.value3 = int.Parse(m_dbc_chongwu_shop.get_index(7, id));
            t_chongwu_shop.weight = int.Parse(m_dbc_chongwu_shop.get_index(8, id));
            t_chongwu_shop.huobitype = int.Parse(m_dbc_chongwu_shop.get_index(9, id));
            t_chongwu_shop.huobi = int.Parse(m_dbc_chongwu_shop.get_index(10, id));
            t_chongwu_shop.tuijian = int.Parse(m_dbc_chongwu_shop.get_index(11, id));
            m_chongwu_shop_s.Add(id, t_chongwu_shop);
        }
    }
    public s_t_chongwu_shop get_t_chongwu_shop(int id)
    {
        if (m_chongwu_shop_s.ContainsKey(id))
        {
            return m_chongwu_shop_s[id];
        }
        return null;
    }
	public s_t_role_spskillup get_t_role_spskillup(int level)
	{
		if(m_role_spskilups.ContainsKey(level))
		{
			return m_role_spskilups[level];
		}
		return null;
	}

	private void parse_t_pet()
	{
		m_dbc_chongwu.load_txt("t_chongwu",0);
		foreach (int id in m_dbc_chongwu.m_index.Keys)
		{
			s_t_pet t_chongwu = new s_t_pet();
			t_chongwu.id = int.Parse(game_data._instance.m_dbc_chongwu.get_index(0,id));
			t_chongwu.name = get_t_language( game_data._instance.m_dbc_chongwu.get_index(2,id));
			t_chongwu.show = game_data._instance.m_dbc_chongwu.get_index(3,id);
			t_chongwu.small_show = game_data._instance.m_dbc_chongwu.get_index(4,id);
			t_chongwu.icon = game_data._instance.m_dbc_chongwu.get_index(5,id);
			t_chongwu.desc = get_t_language(game_data._instance.m_dbc_chongwu.get_index(6,id));
			t_chongwu.color =  int.Parse(game_data._instance.m_dbc_chongwu.get_index(8,id));
			for (int j = 0; j < 4; ++j)
			{
				int cs = int.Parse(game_data._instance.m_dbc_chongwu.get_index(9 + j * 3,id));
				float cscz = float.Parse(game_data._instance.m_dbc_chongwu.get_index(10 + j * 3,id));
				float shengxing_cz = float.Parse(game_data._instance.m_dbc_chongwu.get_index(11 + j * 3,id));
				float jinjie_sxcz = float.Parse(game_data._instance.m_dbc_chongwu.get_index(23 + j,id));
				int shengxing_sxcz = int.Parse(game_data._instance.m_dbc_chongwu.get_index(31 + j,id));
				t_chongwu.cs.Add(cs);
				t_chongwu.cscz.Add(cscz);
				t_chongwu.shengxing_cz.Add(shengxing_cz);
				t_chongwu.jinjie_sxcz.Add(jinjie_sxcz);
				t_chongwu.shengxing_sxcz.Add(shengxing_sxcz);
			}
			for (int i = 0; i < 2; ++i)
			{
				int skill = int.Parse(game_data._instance.m_dbc_chongwu.get_index(21 + i,id));
				t_chongwu.skills.Add(skill);
			}
			for (int i = 0; i < 2; ++i)
			{
				s_t_attr t_attr = new s_t_attr();
				t_attr.attr = int.Parse(game_data._instance.m_dbc_chongwu.get_index(27 + i*2,id));
				t_attr.value = int.Parse(game_data._instance.m_dbc_chongwu.get_index(28 + i*2,id));
				t_chongwu.jinjie_add_sx.Add(t_attr);
			}
			t_chongwu.shengxing_jncz = float.Parse(game_data._instance.m_dbc_chongwu.get_index(35,id));
			t_chongwu.soul_beast = int.Parse(game_data._instance.m_dbc_chongwu.get_index(36,id));
			t_chongwu.sx_add = float.Parse(game_data._instance.m_dbc_chongwu.get_index(37,id));
			t_chongwu.sz_sx_add = float.Parse(game_data._instance.m_dbc_chongwu.get_index(38,id));
			m_pet_s.Add(id, t_chongwu);
		}
	}

	public s_t_pet get_t_pet(int id)
	{
		if(m_pet_s.ContainsKey(id))
		{
			return m_pet_s[id];
		}
		return null;
	}

	private void parse_t_pet_shengxing()
	{
		m_dbc_chongwu_shengxing.load_txt("t_chongwu_shengxing",0);
		foreach (int id in m_dbc_chongwu_shengxing.m_index.Keys)
		{
			s_t_pet_shengxing t_chongwu_shengxing = new s_t_pet_shengxing();
			t_chongwu_shengxing.level = int.Parse(game_data._instance.m_dbc_chongwu_shengxing.get_index(0,id));
			t_chongwu_shengxing.need_level = int.Parse(game_data._instance.m_dbc_chongwu_shengxing.get_index(1,id));
			for (int j = 0; j < 4; ++j)
			{
				int sp = int.Parse(game_data._instance.m_dbc_chongwu_shengxing.get_index(2 + j,id));
				t_chongwu_shengxing.sp.Add(sp);
				int stone = int.Parse(game_data._instance.m_dbc_chongwu_shengxing.get_index(6 + j,id));
				t_chongwu_shengxing.stone.Add(stone);
				int gold = int.Parse(game_data._instance.m_dbc_chongwu_shengxing.get_index(10 + j,id));
				t_chongwu_shengxing.gold.Add(gold);
			}
			m_pet_shengxings.Add(id, t_chongwu_shengxing);
		}
	}
	
	public s_t_pet_shengxing get_t_pet_shengxing(int level)
	{
		if(m_pet_shengxings.ContainsKey(level))
		{
			return m_pet_shengxings[level];
		}
		return null;
	}

	private void parse_t_pet_skill()
	{
		m_dbc_pet_skill.load_txt("t_chongwu_skill",0);
		foreach (int id in m_dbc_pet_skill.m_index.Keys)
		{
			s_t_pet_skill _skill = new s_t_pet_skill();
			_skill.id = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(0,id));
			_skill.name = get_t_language(game_data._instance.m_dbc_pet_skill.get_index(3,id));
			_skill.des = get_t_language(game_data._instance.m_dbc_pet_skill.get_index(4,id));
			_skill.action = game_data._instance.m_dbc_pet_skill.get_index(5,id);
			_skill.attack_type = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(6,id));
			_skill.target_type = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(7,id));
			
			_skill.range = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(8,id)); 
			
			_skill.attack_pe = float.Parse(game_data._instance.m_dbc_pet_skill.get_index(9,id));
			_skill.attack_rate = float.Parse(game_data._instance.m_dbc_pet_skill.get_index(10,id));
			for (int i = 0; i < 2; ++i)
			{
				int buffer_type = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(14 + i * 7,id));
				int buffer_target_type = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(15 + i * 7,id));
				int buffer_round = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(16 + i * 7,id));
				int buffer_attack_type = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(17 + i * 7,id));
				float buffer_attack_pe = float.Parse(game_data._instance.m_dbc_pet_skill.get_index(18 + i * 7,id));
				int buffer_modify_att_type = int.Parse(game_data._instance.m_dbc_pet_skill.get_index(19 + i * 7,id));
				float buffer_modify_att_val = float.Parse(game_data._instance.m_dbc_pet_skill.get_index(20 + i * 7,id));
				
				_skill.buffer_types.Add(buffer_type);
				_skill.buffer_target_types.Add(buffer_target_type);
				_skill.buffer_rounds.Add(buffer_round);
				_skill.buffer_attack_types.Add(buffer_attack_type);
				_skill.buffer_attack_pes.Add(buffer_attack_pe);
				_skill.buffer_modify_att_types.Add(buffer_modify_att_type);
				_skill.buffer_modify_att_vals.Add(buffer_modify_att_val);
			}

			
			m_pet_skill_s.Add(id, _skill);
		}
	}
	
	public s_t_pet_skill get_t_pet_skill(int id)
	{
		if(m_pet_skill_s.ContainsKey(id))
		{
			return m_pet_skill_s[id];
		}
		return null;
	}

	private void parse_t_pet_jinjie()
	{
		m_dbc_chongwu_jinjie.load_txt("t_chongwu_jinjie",0);
		foreach (int id in m_dbc_chongwu_jinjie.m_index.Keys)
		{
			s_t_pet_jinjie t_chongwu_jinjie = new s_t_pet_jinjie();
			t_chongwu_jinjie.level = int.Parse(game_data._instance.m_dbc_chongwu_jinjie.get_index(0,id));
			t_chongwu_jinjie.chenghao = get_t_language(m_dbc_chongwu_jinjie.get_index(2,id));
			t_chongwu_jinjie.icon = m_dbc_chongwu_jinjie.get_index(3,id);
			t_chongwu_jinjie.need_level = int.Parse(game_data._instance.m_dbc_chongwu_jinjie.get_index(4,id));
			for (int j = 0; j < 4; ++j)
			{
				int cl = int.Parse(game_data._instance.m_dbc_chongwu_jinjie.get_index(5 + j,id));
				if(cl > 0)
				{
					t_chongwu_jinjie.cls.Add(cl);
				}
			}
			t_chongwu_jinjie.gold = int.Parse(game_data._instance.m_dbc_chongwu_jinjie.get_index(9,id));
			t_chongwu_jinjie.qsx_add = int.Parse(game_data._instance.m_dbc_chongwu_jinjie.get_index(10,id));
			t_chongwu_jinjie.esx_add = int.Parse(game_data._instance.m_dbc_chongwu_jinjie.get_index(11,id));
			m_pet_jinjies.Add(id, t_chongwu_jinjie);
		}
	}
	
	public s_t_pet_jinjie get_t_pet_jinjie(int level)
	{
		if(m_pet_jinjies.ContainsKey(level))
		{
			return m_pet_jinjies[level];
		}
		return null;
	}

	private void parse_t_pet_jinjieitem()
	{
		m_dbc_chongwu_jinjieitem.load_txt("t_chongwu_jinjieitem",0);
		foreach (int id in m_dbc_chongwu_jinjieitem.m_index.Keys)
		{
			s_t_pet_jinjieitem t_chongwu_jinjieitem = new s_t_pet_jinjieitem();
			t_chongwu_jinjieitem.id = int.Parse(game_data._instance.m_dbc_chongwu_jinjieitem.get_index(0,id));
			t_chongwu_jinjieitem.name = get_t_language(m_dbc_chongwu_jinjieitem.get_index(3,id));
			for (int j = 0; j < 3; ++j)
			{
				s_t_attr t_attr = new s_t_attr();
				t_attr.attr = int.Parse(m_dbc_chongwu_jinjieitem.get_index(5+2*j,id));
				t_attr.value = int.Parse(m_dbc_chongwu_jinjieitem.get_index(6+2*j,id));
				if(t_attr.attr > 0)
				{
					t_chongwu_jinjieitem.attrs.Add(t_attr);
				}
			}
			m_pet_jinjie_items.Add(id, t_chongwu_jinjieitem);
		}
	}
	
	public s_t_pet_jinjieitem get_t_pet_jinjieitem(int id)
	{
		if(m_pet_jinjie_items.ContainsKey(id))
		{
			return m_pet_jinjie_items[id];
		}
		return null;
	}
    public void parse_t_huo_dong_czjhrs()
    {
        m_dbc_huodong_czjhrs.load_txt("t_huodong_czjhrs",0);
        foreach (int id in m_dbc_huodong_czjhrs.m_index.Keys)
        {
            s_t_huodong_czjhrs cz = new s_t_huodong_czjhrs();
            cz.index = id;
            cz.buy_count = int.Parse(m_dbc_huodong_czjhrs.get_index(1, id));
            cz.type = int.Parse(m_dbc_huodong_czjhrs.get_index(2, id));
            cz.value1 = int.Parse(m_dbc_huodong_czjhrs.get_index(3, id));
            cz.value2 = int.Parse(m_dbc_huodong_czjhrs.get_index(4, id));
            cz.value3 = int.Parse(m_dbc_huodong_czjhrs.get_index(5, id));
            m_huo_dong_czjhrs.Add(id,cz);
 
        }
    }

    public s_t_huodong_czjhrs get_t_huodong_czjhrs(int id)
    {
        if (m_huo_dong_czjhrs.ContainsKey(id))
        {
           return m_huo_dong_czjhrs[id];
        }
        return null;
    }
	private void parse_t_pet_target()
	{
		m_dbc_chongwu_target.load_txt("t_chongwu_target",0);
		foreach (int id in m_dbc_chongwu_target.m_index.Keys)
		{
			s_t_pet_target t_chongwu_target = new s_t_pet_target();
			t_chongwu_target.id = int.Parse(game_data._instance.m_dbc_chongwu_target.get_index(0,id));
			t_chongwu_target.name = get_t_language(m_dbc_chongwu_target.get_index(2,id));
			for (int j = 0; j < 2; ++j)
			{
				int _id = int.Parse(game_data._instance.m_dbc_chongwu_target.get_index(3+j,id));
				if(_id > 0)
				{
					t_chongwu_target.target_ids.Add(_id);
				}
			}
			for (int j = 0; j < 4; ++j)
			{
				s_t_attr t_attr = new s_t_attr();
				t_attr.attr = int.Parse(m_dbc_chongwu_target.get_index(5+2*j,id));
				t_attr.value = int.Parse(m_dbc_chongwu_target.get_index(6+2*j,id));
				if(t_attr.attr > 0)
				{
					t_chongwu_target.attrs.Add(t_attr);
				}
			}
			m_chongwu_target_s.Add(id, t_chongwu_target);
		}
	}
	
	public s_t_pet_target get_t_pet_target(int id)
	{
		if(m_chongwu_target_s.ContainsKey(id))
		{
			return m_chongwu_target_s[id];
		}
		return null;
	}

	private void parse_t_comeback()
	{
		m_dbc_comeback.load_txt ("t_comeback", 0);
		foreach (int id in m_dbc_comeback.m_index.Keys) 
		{
			s_t_comeback t_comeback =new s_t_comeback();
			
			t_comeback.id =int.Parse(game_data._instance.m_dbc_comeback.get_index(0,id));
			t_comeback.desc =game_data._instance.m_dbc_comeback.get_index(1,id);
			t_comeback.design =game_data._instance.m_dbc_comeback.get_index(2,id);

			t_comeback.type =int.Parse(game_data._instance.m_dbc_comeback.get_index(3,id));

			t_comeback.def1 =int.Parse(game_data._instance.m_dbc_comeback.get_index(4,id));
			t_comeback.def2 =int.Parse(game_data._instance.m_dbc_comeback.get_index(5,id));

			t_comeback.def3 =int.Parse(game_data._instance.m_dbc_comeback.get_index(6,id));

			t_comeback.def4 =int.Parse(game_data._instance.m_dbc_comeback.get_index(7,id));

			for (int i = 0; i < 3; i++) {

				s_t_reward reward =new s_t_reward();
				reward.type =int.Parse(game_data._instance.m_dbc_comeback.get_index(8 + 4 * i , id));
				reward.value1 =int.Parse(game_data._instance.m_dbc_comeback.get_index(9 + 4 * i , id));
				reward.value2 =int.Parse(game_data._instance.m_dbc_comeback.get_index(10 + 4 * i , id));
				reward.value3 =int.Parse(game_data._instance.m_dbc_comeback.get_index(11 + 4 * i , id));	
				if(reward.type > 0)
				{
				t_comeback.rewards.Add(reward);
				}
			}
			m_comeback.Add(id, t_comeback);
		}
	}
	public s_t_comeback get_t_comeback(int id)
	{
		if(m_comeback.ContainsKey(id))
		{
			return m_comeback[id];
		}
		return null;
	}

    private void parse_t_role_gaizao()
    {
        m_dbc_role_gaizao.load_txt("t_role_gaizao", 0);
        foreach (int id in m_dbc_role_gaizao.m_index.Keys)
        {
            s_t_role_gaizao t_role_gaizao = new s_t_role_gaizao();
            t_role_gaizao.id = int.Parse(game_data._instance.m_dbc_role_gaizao.get_index(0, id));
            t_role_gaizao.type = int.Parse(game_data._instance.m_dbc_role_gaizao.get_index(2, id));
            t_role_gaizao.jewel = int.Parse(game_data._instance.m_dbc_role_gaizao.get_index(3, id));
            t_role_gaizao.jjc_point = int.Parse(game_data._instance.m_dbc_role_gaizao.get_index(4, id));
            m_role_gaizao.Add(id, t_role_gaizao);
        }
    }

    public s_t_role_gaizao get_t_role_gaizao(int id)
    {
        if (m_role_gaizao.ContainsKey(id))
        {
            return m_role_gaizao[id];
        }
        return null;
    }

    private void parse_t_const()
    {
        m_dbc_t_const.load_txt("t_const", -1);
        for (int j = 0; j < game_data._instance.m_dbc_t_const.get_y(); ++j)
        {
            int const_vale = int.Parse(m_dbc_t_const.get(1, j));
            m_const.Add(j, const_vale);
        }
    }

    public int get_const_vale(int id)
    {
        if (m_const.ContainsKey(id))
        {
            return m_const[id];
        }
        return 1;
    }

    public void load_txt()
	{
		//float _ctime = Time.smoothDeltaTime;
		
		parse_t_role_spskillup ();
		parse_t_skill ();
		m_dbc_monster.load_txt("t_monster",0);
		m_dbc_role_skillunlock.load_txt("t_role_skillunlock",0);
		m_dbc_class.load_txt ("t_role",0);
        m_dbc_resource.load_txt("t_resoures",0);
        parse_t_mission();
		m_dbc_sport_card.load_txt ("t_sport_card",0);
		m_dbc_name.load_txt ("t_name", -1);
		m_dbc_prohibitword.load_txt ("t_prohibitword", -1);
		m_dbc_error.load_txt ("t_error", 0);
		m_dbc_exp.load_txt ("t_exp", 0);
		m_dbc_item.load_txt ("t_item", 0);
        m_dbc_itemstore.load_txt("t_itemstore",0);
		m_dbc_equip.load_txt ("t_equip", 0);
		m_dbc_enhance.load_txt ("t_equip_enhance", 0);
		m_dbc_gaizao.load_txt ("t_equip_gaizao", 0);
		m_dbc_role_shop.load_txt ("t_role_shop", 0);
		m_dbc_shop_xg.load_txt ("t_shop_xg", 0);
        parse_t_target();
		m_dbc_active.load_txt ("t_active", 0);
		m_dbc_active_reward.load_txt ("t_active_reward", 0);
		m_dbc_jiban.load_txt ("t_role_jiban", 0);
		m_dbc_jibanex.load_txt ("t_role_jibanex", 0);
		m_dbc_map.load_txt ("t_map",0);
        m_dbc_qiyu_tiaozhan.load_txt("t_qiyutiaozhan",0);
		m_dbc_vip.load_txt ("t_vip",0);
		m_dbc_recharge.load_txt ("t_recharge",0);
		m_dbc_price.load_txt ("t_price",0);
		m_dbc_first_recharge.load_txt ("t_first_recharge",-1);
		m_dbc_online_reward.load_txt ("t_online_reward",-1);
		m_dbc_daily_sign.load_txt ("t_daily_sign",0);
		m_dbc_huodong.load_txt("t_huodong",0);
		m_dbc_huodong_sub.load_txt("t_huodong_sub",0);
		parse_t_tanbao_event ();
		m_t_value.load_txt ("t_value",0);
		s_t_value _value = new s_t_value();
		m_value_s [0] = _value;
		for(int i = 0;i < m_t_value.get_y();i ++)
		{	
			_value = new s_t_value();

			_value.id = int.Parse(m_t_value.get(0,i));
			_value.name = get_t_language(m_t_value.get(1,i));
			_value.att = int.Parse(m_t_value.get(2,i));
			_value.des = get_t_language(m_t_value.get(4,i));
			_value.force = float.Parse(m_t_value.get(5,i));

			m_value_s[_value.id] = _value;
		}
        
		m_t_face.load_txt ("t_face",0);
		m_dbc_sport_rank.load_txt ("t_sport_rank", 0);
		m_dbc_scene_music.load_txt ("t_scene_music",-1);
		m_dbc_ttt.load_txt ("t_ttt", 0);
		m_dbc_ttt_reward.load_txt ("t_ttt_reward", 0);
		m_dbc_ttt_value.load_txt ("t_ttt_value", 0);
		m_dbc_tip.load_txt ("t_tip", -1);
		m_dbc_tupo.load_txt ("t_role_tupo", 0);
		m_dbc_jinjie.load_txt ("t_role_jinjie", 0);
		m_dbc_skillup.load_txt ("t_role_skillup", 0);
		m_dbc_guild_icon.load_txt ("t_guild_icon",0);
		m_dbc_guild_shop.load_txt ("t_guild_shop", 0);
		m_dbc_guild_sign.load_txt ("t_guild_sign", 0);
        m_dbc_guildfight_reward.load_txt("t_guildfight_reward", 0);
		m_dbc_dress.load_txt("t_dress",0);
		m_dbc_dress_target.load_txt("t_dress_target",0);
		m_dbc_role_dress.load_txt("t_role_dress",0);
		m_dbc_xjbz.load_txt("t_xjbz",0);
		m_dbc_xjbz_mission.load_txt("t_xjbz_mission",-1);
		m_dbc_danmu.load_txt("t_danmu",-1);
		m_dbc_huodong_pttq.load_txt("t_huodong_pttq", 0);
		m_dbc_kaifu.load_txt("t_kaifu", -1);
		m_dbc_kaifu_mubiao.load_txt("t_kaifu_mubiao", 0);
		m_dbc_jc_huodong.load_txt("t_jc_huodong", 0);
		m_dbc_boss_reward.load_txt ("t_boss_reward", 0);
		m_dbc_huodong_czjh.load_txt("t_huodong_czjh", 0);
		m_dbc_equip_tz.load_txt("t_equip_tz", 0);
		m_dbc_baowu.load_txt ("t_baowu", 0);
		m_dbc_treasure_enhance.load_txt ("t_baowu_enhance", 0);
		m_dbc_baowu_jl.load_txt("t_baowu_jl", 0);
		m_dbc_sport_shop.load_txt ("t_sport_shop", 0);
		m_dfa.init ();
		m_dbc_yb.load_txt("t_yb", 0);
		m_dbc_yb_gw.load_txt("t_yb_gw", 0);
		m_dbc_ore.load_txt("t_ore", 0);
        
        m_dbc_pvp_active.load_txt("t_lieren_active",0);
        m_dbc_pvp_reward.load_txt("t_lieren_reward", 0);
        m_dbc_pvp_shop.load_txt("t_lieren_shop", 0);
        //回忆相关表=============
        m_dbc_huiyi_chengjiu.load_txt("t_huiyi_chengjiu", 0);
        m_dbc_huiyi_luckshop.load_txt("t_huiyi_luckshop", 0);
        m_dbc_huiyi_lunpan.load_txt("t_huiyi_lunpan", 0);
        m_dbc_huiyi_mingyun.load_txt("t_huiyi_mingyun", 0);
        m_dbc_huiyi_shop.load_txt("t_huiyi_shop", 0);
        m_dbc_huiyi.load_txt("t_huiyi",0);
        parse_t_huiyi_sub();
        //========================
        //冰原相关表==============
        m_dbc_bingyuan_shop.load_txt("t_bingyuan_shop", 0);
        m_dbc_bingyuan_reward.load_txt("t_bingyuan_reward", 0);
        m_dbc_bingyuan_mubiao.load_txt("t_bingyuan_mubiao", 0);
        //========================
        m_dbc_equip_sx.load_txt("t_equip_sx", 0);
		m_dbc_xinqing_event.load_txt("t_xinqing_event", 0);
		m_dbc_xinqing.load_txt ("t_xinqing", 0);
        m_dbc_horreward.load_txt("t_guild_mobai_reward", 0);
        parse_t_boss_active();
        parse_t_hongbao_target();
		m_dbc_gongzhen.load_txt ("t_gongzhen", 0);
		m_dbc_ttt_mibao.load_txt ("t_ttt_baozang", 0);
		m_dbc_equip_jl.load_txt ("t_equip_jl", 0);
        parse_t_boss_dw();
		m_dbc_boss_shop.load_txt ("t_boss_shop", 0);
		m_dbc_sport_mubiao.load_txt ("t_sport_mubiao", 0);
		m_dbc_ttt_shop.load_txt ("t_ttt_shop", 0);
		m_dbc_ttt_mubiao.load_txt ("t_ttt_mubiao", 0);
		m_dbc_guild_mubiao.load_txt ("t_guild_mubiao", 0);
        m_dbc_guild_keji.load_txt("t_guild_keji", 0);
		m_dbc_mission_first_reward.load_txt ("t_mission_first_reward",0);
		m_dbc_dress_unlock.load_txt ("t_dress_unlock",0);
		m_dbc_xinqing_random.load_txt ("t_xinqing_random",0);
		m_dbc_equip_skill.load_txt ("t_equip_skill",0);
		m_dbc_vip_libao.load_txt ("t_vip_libao",0);
		m_dbc_week_libao.load_txt ("t_week_libao",0);
		m_dbc_guild_shop_xs.load_txt ("t_guild_shop_xs",0);
		m_dbc_itemhecheng.load_txt ("t_itemhecheng",0);
		m_dbc_role_shengpin.load_txt ("t_role_shengpin",0);
		m_dbc_duixing.load_txt ("t_duixing",0);
		m_dbc_duixing_up.load_txt ("t_duixing_up",0);
		m_dbc_duixing_skill.load_txt ("t_duixing_skill",0);
		parse_t_tanbao_mubiao();
		parse_t_tanbao_shop();
		m_dbc_tanbao_reward.load_txt ("t_tanbao_reward",0);
        parse_guild();
        parse_guildfight();
        parse_guildfight_target();
        parse_t_guild_mission();
		parse_t_guanghuan ();
        parse_t_master_duanwei();
		parse_t_guanghuan_enhance ();
		parse_t_guanghuan_skill ();
		parse_t_guanghuan_target ();
		parse_t_itemhecheng ();
        parse_t_chenghao();
        parse_t_bingyuan_chenghao();
        parse_t_guild_shop_ex();
		m_dbc_baowu_sx.load_txt ("t_baowu_sx", 0);
		parse_t_role_dresstarget ();
		parse_t_chongzhifanpai ();
		parse_t_chongzhifanpai_shop ();
		parse_t_zhuanpan ();
		m_dbc_zhuanpan_reward.load_txt ("t_zhuanpan_reward", 0);
		parse_t_manyou_dati ();
		parse_t_manyou_qiyu ();
		parse_t_manyou ();
		parse_t_manyou_mubiao ();
		parse_t_manyou_reward ();
		parse_t_pet ();
		parse_t_pet_shengxing();
		parse_t_pet_skill ();
        parse_t_chongwu_shop();
		parse_t_pet_jinjie ();
        parse_t_master_target();
		parse_t_pet_jinjieitem();
        parse_t_master_reward();
		parse_t_pet_target ();
        parse_t_huo_dong_czjhrs();
        parse_t_mofang();
        parse_t_mofang_shop();
        parse_t_mofang_reward();
		parse_t_yueka_jijin ();
		parse_t_comeback ();
        parse_t_role_gaizao();
        parse_t_const();
    }
}
