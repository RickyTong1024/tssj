
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// 定义角色属性
public enum e_player_attr
{
	player_null = 0,

	player_gold,
	player_jewel,
	player_tili,
	
	player_exp,
	player_jjc_point,
	player_hj,//合金
    player_yuanli,
    player_huodong_jifen,
    player_vip_exp,
    player_contribution,
    player_baoshi_yuanshi,
    player_tansuo_cishu,
    player_medal_point,//魔王勋章
    player_treasure_powder,
    player_treasure_energy = 15,
    player_luck_point = 20,
    player_huiyi_point = 21,
    player_pvp_jz = 22,
    player_bingyuan_point = 24,
    player_xinpian = 27,
    player_mofang_jifen = 28,
    player_friend_point = 29,
	player_mp,
	
	player_ts,
	
	player_score,
	
    player_guild_exp,
    player_guild_hor,
	player_baoshi,
	
	player_toupiao_jifen,
	
	player_skill_point,
	player_dress_tuzhi,
    player_boss_num,
    player_level,
   
    player_pvp_jf,
    player_bf = 1000,
	player_end, 
}

public class s_mission
{
	int m_id;
	int m_star;
	int m_num;
}

public class s_map
{
	int m_id;
	int m_star;
}
public class player
{

    public ulong m_guid = 0;
    public string m_sig;
    public int m_pck_id = 0;
    private int m_max_mp = 400;
    private encryption_value m_mp = new encryption_value();
    private List<ccard> m_cards = new List<ccard>();
    private List<pet> m_pets = new List<pet>();
    private List<dhc.equip_t> m_equips = new List<dhc.equip_t>();
    private List<dhc.post_t> m_posts = new List<dhc.post_t>();
    public List<int> m_items = new List<int>();
    private List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
    //public List<s_mission> m_mission = new List<s_mission>();
    //public List<s_map> m_map = new List<s_map>();
    public dhc.player_t m_t_player;
    public List<dhc.role_t> m_roles = new List<dhc.role_t>();
    public Dictionary<int, double> m_sx = new Dictionary<int, double>();
    //public dhc.role_t m_t_role;
    //private List<dhc.role_t> m_t_role;
    public ulong m_time = 0;
    public int m_reserve = 3;
    public double m_force_per = 1.0f;
    public int is_post;
    public bool is_postzd;
    public int is_bingyuan_effect;
    public int is_friend_apply;
    public int is_friend_tili;
    public int m_can_kaifu;
    public int m_can_pttq;
    public int m_can_guild;
    public int m_can_czjh;
    public int m_can_jieri_huodong = 0;
    public int m_item_shop_effect = 0;
    public int m_jc_huodong_effect = 0;
    public bool m_exist_huodong = false;
    public string m_jieri_huodong = "";
    public string m_jieri_icon_name = "";
    public string m_jieri_icon_name1 = "";
    public string m_jieri_icon_desc = "";
    public string m_jieri_icon_desc1 = "";
    public int[] m_kaifu_banjia = { 0, 0, 0, 0, 0, 0, 0, 0 };
    public List<ulong> m_end_time = new List<ulong>();
    public List<string> m_huodong_name = new List<string>();
    public List<int> m_huodong_ids = new List<int>();
    public int m_huodong_czjh_count;
    public int m_huodong_xhqd = 0;
    public double m_hp_pe = 0;
    public double m_attack_pe = 0;
    public List<int> m_xg_ids = new List<int>();
    public List<int> m_xg_nums = new List<int>();
    public ulong guild_shop_xs_time;
    public Dictionary<int, int> m_huiyi_xuyao = new Dictionary<int, int>();
    public Dictionary<uint, int> m_item_nums = new Dictionary<uint, int>();
    public bool m_is_chou = false;
    public bool m_need_check_huiyi = false;

    public int get_un_equip_num()
    {
        int num = 0;
        for (int i = 0; i < this.m_equips.Count; i++)
        {
            dhc.equip_t equip = this.m_equips[i];

            num++;

        }
        return num;
    }

    public bool bag_full()
    {
        if (get_un_equip_num() >= get_max_equip_num())
        {
            string tishi = game_data._instance.get_t_language("arena.cs_104_45");//提示
            string _des = game_data._instance.get_t_language("player.cs_141_18");   //您的装备携带数量已达上限，是否前往进行装备回收
            s_message _message = new s_message();
            _message.m_type = "show_bagfull_gui";
            root_gui._instance.show_select_dialog_box(tishi, _des, _message);
            return true;
        }

        return false;
    }

    public bool treasure_full()
    {
        if (get_un_treasure_num() >= get_max_treasure_num())
        {
            string tishi = game_data._instance.get_t_language("arena.cs_104_45");//提示
            string _des = game_data._instance.get_t_language("bag_gui.cs_2059_38");//您的饰品携带数量已达上限，是否前往进行饰品强化或者精炼
            s_message _message = new s_message();
            _message.m_type = "show_buzheng";
            root_gui._instance.show_select_dialog_box(tishi, _des, _message);
            return true;
        }

        return false;
    }

    public bool tili_buzu(int num)
    {
        int tili = get_att(e_player_attr.player_tili);
        if (tili < num)
        {
            /*int _tili_num = m_t_player.tili_num + 1;
			s_t_price _price = game_data._instance.get_t_price(_tili_num);
			s_t_vip _vip = game_data._instance.get_t_vip(m_t_player.vip);
			s_t_exp _exp = game_data._instance.get_t_exp(m_t_player.level);
			if(_tili_num > _vip.tili_num)
			{
				string s = game_data._instance.get_t_language ("player.cs_177_15");//体力不足
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
			}
			else
			{
				string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
				string s = game_data._instance.get_t_language ("player.cs_183_15");//您的体力不足
				string s1 = game_data._instance.get_t_language ("player.cs_184_16");//是否花费[00ffff]
				string s2 = game_data._instance.get_t_language ("czfp_gui.cs_360_108");//钻石
				string s3 = game_data._instance.get_t_language ("player.cs_186_16");//购买
				string s4 = game_data._instance.get_t_language ("player.cs_187_16");//点体力，您今日还可购买
				string s5 = game_data._instance.get_t_language ("guild_buttle_gui.cs_55_160");//次
				int mt = 50;
				string  _des = s+"，" +s1 + _price.tili + s2+"[-]"+s3 + mt.ToString() + s4 + ((int)_vip.tili_num - m_t_player.tili_num) + s5;
				s_message _message = new s_message();
				_message.m_type = "buy_tl_count";
				_message.m_ints.Add(_price.tili);
				root_gui._instance.show_select_dialog_box(tishi,_des,_message);
			}*/
            return true;
        }
        return false;
    }

    /*
	public ccard get_battle_card(int id)
	{
		return get_card_guid (get_battle_guid(id));
	}
	*/

    public ccard get_battle_card(int id)
    {
        int _id = id;
        /*
		int _duixing_id = get_t_player().duixing[id];
		ulong _guid = get_t_player().zhenxing[_duixing_id];

		if(_guid == 0)
		{
			_duixing_id = get_t_player().duixing[id + 3];
			_guid = get_t_player().zhenxing[_duixing_id];
		}
		*/

        for (int i = 0; i < get_t_player().zhenxing.Count; i++)
        {
            ulong _guid = get_t_player().zhenxing[i];

            ccard _card = get_card_guid(_guid);

            if (_card != null && _card.m_t_class.job == 1)
            {
                if (_id == 0)
                {
                    return _card;
                }
                _id--;
            }
        }

        for (int i = 0; i < get_t_player().zhenxing.Count; i++)
        {
            ulong _guid = get_t_player().zhenxing[i];

            ccard _card = get_card_guid(_guid);

            if (_card != null && _card.m_t_class.job != 1)
            {
                if (_id == 0)
                {
                    return _card;
                }
                _id--;
            }
        }


        //ulong _guid = get_t_player().zhenxing[id];

        return null;
    }

    public int get_mission_star(int id)
    {
        for (int i = 0; i < m_t_player.mission_ids.Count; i++)
        {
            if (m_t_player.mission_ids[i] == id)
            {
                return m_t_player.mission_star[i];
            }
        }

        return 0;
    }
    public int get_max_nl()
    {
        int _nl = 0;
        _nl = 30;
        return _nl;
    }
    public int get_hongbao_target_state(int id)
    {
        if (m_t_player.guild_red_rewards.Contains(id))
        {
            return 1;//已领取
        }
        else
        {
            s_t_hongbao_target target = game_data._instance.get_t_hongbao_target(id);
            if (target.type == 1)
            {
                if (m_t_player.guild_deliver_jewel >= target.tiaojian)
                {
                    return 3;
                }
                else
                {
                    return 2;
                }

            }
            else
            {
                if (m_t_player.guild_rob_jewel >= target.tiaojian)
                {
                    return 3;
                }
                else
                {
                    return 2;
                }

            }

        }
    }
    public int get_max_tili()
    {
        int _tili = 0;
        s_t_vip _vip = game_data._instance.get_t_vip(m_t_player.vip);
        s_t_exp _exp = game_data._instance.get_t_exp(m_t_player.level);
        if (m_t_player == null || _exp == null)
        {
            return 0;
        }

        _tili = _exp.tili + _vip.add_tili;
        return _tili;
    }
    public int get_vip()
    {
        return m_t_player.vip;
    }
    public int get_mission_cishu(int id)
    {
        s_t_mission t_mission = game_data._instance.get_t_mission(id);
        if (t_mission.type > 3)
        {
            id = t_mission.type;
        }
        for (int i = 0; i < m_t_player.mission_cishu_ids.Count; i++)
        {
            if (m_t_player.mission_cishu_ids[i] == id)
            {
                return m_t_player.mission_cishu[i];
            }
        }

        return 0;
    }
    public bool is_huiyi_shop_buy(int id)
    {
        for (int i = 0; i < m_t_player.shop4_sell.Count; i++)
        {
            if (id == m_t_player.shop4_sell[i])
            {
                if (m_t_player.shop4_sell[i] > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        return false;
    }
    public void add_mission_cishu(int id, int num)
    {
        s_t_mission t_mission = game_data._instance.get_t_mission(id);
        if (t_mission.type > 3)
        {
            id = t_mission.type;
        }
        bool flag = false;
        for (int i = 0; i < m_t_player.mission_cishu_ids.Count; ++i)
        {
            if (m_t_player.mission_cishu_ids[i] == id)
            {
                flag = true;
                m_t_player.mission_cishu[i] = m_t_player.mission_cishu[i] + num;
                break;
            }
        }
        if (!flag)
        {
            m_t_player.mission_cishu_ids.Add(id);
            m_t_player.mission_cishu.Add(num);
        }
    }

    public int get_map_star(int id)
    {
        for (int i = 0; i < m_t_player.map_ids.Count; i++)
        {
            if (m_t_player.map_ids[i] == id)
            {
                return m_t_player.map_star[i];
            }
        }

        return 0;
    }

    public int add_mission_star(int id, int star)
    {
        int add_star = 0;
        bool flag = false;
        for (int i = 0; i < m_t_player.mission_ids.Count; ++i)
        {
            if (m_t_player.mission_ids[i] == id)
            {
                flag = true;
                if (m_t_player.mission_star[i] < star)
                {
                    add_star = star - m_t_player.mission_star[i];
                    m_t_player.mission_star[i] = star;
                }
                break;
            }
        }
        if (!flag)
        {
            add_star = star;
            m_t_player.mission_ids.Add(id);
            m_t_player.mission_star.Add(star);
        }
        return add_star;
    }

    public void add_map_star(int id, int star)
    {
        bool flag = false;
        for (int i = 0; i < m_t_player.map_ids.Count; ++i)
        {
            if (m_t_player.map_ids[i] == id)
            {
                flag = true;
                m_t_player.map_star[i] = m_t_player.map_star[i] + star;
                break;
            }
        }
        if (!flag)
        {
            m_t_player.map_ids.Add(id);
            m_t_player.map_star.Add(star);
            m_t_player.map_reward_get.Add(0);
        }
    }

    public int get_mission_goumai(int id)
    {
        for (int i = 0; i < m_t_player.mission_goumai_ids.Count; i++)
        {
            if (m_t_player.mission_goumai_ids[i] == id)
            {
                return m_t_player.mission_goumai[i];
            }
        }

        return 0;
    }
    public int get_map_reward_get(int id)
    {
        for (int i = 0; i < m_t_player.map_ids.Count; i++)
        {
            if (m_t_player.map_ids[i] == id)
            {
                return m_t_player.map_reward_get[i];
            }
        }

        return 0;
    }
    public int get_catd_id_num(int id)
    {
        int _num = 0;
        for (int i = 0; i < this.m_cards.Count; i++)
        {
            ccard _card = this.m_cards[i];

            if (_card.m_t_class.id == id)
            {
                _num++;
            }
        }

        return _num;
    }
    public int get_card_num()
    {
        return m_cards.Count;
    }
    public ccard get_card_guid(ulong guid)
    {
        for (int i = 0; i < this.m_cards.Count; i++)
        {
            ccard _card = this.m_cards[i];

            if (_card.get_guid() == guid)
            {
                return _card;
            }
        }

        return null;
    }
    public ccard get_card_id(int id)
    {
        for (int i = 0; i < this.m_cards.Count; i++)
        {
            ccard _card = this.m_cards[i];

            if (_card.get_template_id() == id)
            {
                return _card;
            }
        }

        return null;
    }
    public ccard get_card_index(int id)
    {
        return m_cards[id];
    }
    public void clear_card()
    {
        m_cards.Clear();
    }

    public pet get_pet_guid(ulong guid)
    {
        for (int i = 0; i < this.m_pets.Count; i++)
        {
            pet _pet = this.m_pets[i];

            if (_pet.get_guid() == guid)
            {
                return _pet;
            }
        }

        return null;
    }

    public pet get_pet_id(int id)
    {
        for (int i = 0; i < this.m_pets.Count; i++)
        {
            pet _pet = this.m_pets[i];

            if (_pet.get_template_id() == id)
            {
                return _pet;
            }
        }

        return null;
    }

    public pet get_pet_index(int id)
    {
        return m_pets[id];
    }

    public void clear_pet()
    {
        m_pets.Clear();
    }

    public int get_pet_num()
    {
        return m_pets.Count;
    }

    public void add_card_login(dhc.role_t role)
    {
        ccard _card = new ccard();
        _card.set_role(role);
        _card.m_player = this;
        m_cards.Add(_card);
        check_tujian(role.template_id);
        _card.update_role_attr();
        _card.role_dress(_card);
    }

    public void add_pet_login(dhc.pet_t pet)
    {
        pet _pet = new pet();
        _pet.set_pet(pet);
        _pet.m_player = this;
        m_pets.Add(_pet);
    }

    public void update_attr()
    {
        for (int i = 0; i < m_cards.Count; ++i)
        {
            m_cards[i].update_role_attr();
        }
    }

    public void add_card(dhc.role_t role)
    {
        add_card(role, true);
    }
    public void add_card(dhc.role_t role, bool notify)
    {
        if (notify)
        {
            s_t_class _class = game_data._instance.get_t_class(role.template_id);
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 3, _class.id.ToString(), "[ffffc0]" + _class.name + " [ffd000]x1");
        }
        ccard _card = new ccard();
        _card.set_role(role);
        m_cards.Add(_card);
        _card.m_player = this;
        m_t_player.roles.Add(role.guid);
        check_tujian(role.template_id);
        _card.update_role_attr();
        check_target_done();
        _card.role_dress(_card);
    }
    public void add_card(List<dhc.role_t> role)
    {
        for (int i = 0; i < role.Count; i++)
        {
            add_card(role[i]);
        }
    }

    public bool has_t_card(int id)
    {
        for (int i = 0; i < m_t_player.role_template_ids.Count; i++)
        {
            if (m_t_player.role_template_ids[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool has_card(int id)
    {
        for (int i = 0; i < m_cards.Count; i++)
        {
            ccard _card = m_cards[i];

            if (_card.get_template_id() == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool has_pet(int id)
    {
        for (int i = 0; i < m_pets.Count; i++)
        {
            pet _pet = m_pets[i];

            if (_pet.get_template_id() == id)
            {
                return true;
            }
        }
        return false;
    }

    public void check_tujian(int id)
    {
        s_t_class _class = game_data._instance.get_t_class(id);
        if (_class.color > 1)
        {
            bool flag = false;
            for (int i = 0; i < m_t_player.role_template_ids.Count; ++i)
            {
                if (m_t_player.role_template_ids[i] == id)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                m_t_player.role_template_ids.Add((uint)id);
            }
        }
    }
    public void remove_card_guid(ulong guid)
    {
        for (int i = 0; i < m_cards.Count; i++)
        {
            ccard _card = m_cards[i];

            if (_card.get_guid() == guid)
            {
                remove_card(_card);
                return;
            }
        }
    }
    public void remove_card(ccard card)
    {
        for (int i = 0; i < get_equip_num(); i++)
        {
            if (m_equips[i].role_guid == card.get_guid())
            {
                m_equips[i].role_guid = 0;
            }
        }
        for (int i = 0; i < get_treasure_num(); i++)
        {
            if (m_treasures[i].role_guid == card.get_guid())
            {
                m_treasures[i].role_guid = 0;
            }
        }
        for (int i = 0; i < m_t_player.zhenxing.Count; i++)
        {
            if (m_t_player.zhenxing[i] == card.get_guid())
            {
                m_t_player.zhenxing[i] = 0;
            }
        }
        m_cards.Remove(card);
        m_t_player.roles.Remove(card.get_guid());
    }

    public void remove_pet(pet pet)
    {
        m_pets.Remove(pet);
        m_t_player.pets.Remove(pet.get_guid());
    }

    public double get_fighting()
    {
        double _value = 0;
        for (int i = 0; i < m_t_player.zhenxing.Count; i++)
        {
            ccard _card = get_card_guid(m_t_player.zhenxing[i]);
            if (_card != null)
            {
                _value += _card.get_fighting();
            }
        }

        return _value;
    }

    public void remove_item(uint id, int num, string info = "")
    {
        for (int i = 0; i < m_t_player.item_ids.Count; i++)
        {
            if (id == m_t_player.item_ids[i])
            {
                m_t_player.item_amount[i] -= num;

                if (m_t_player.item_amount[i] <= 0)
                {
                    m_t_player.item_ids.RemoveAt(i);
                    m_t_player.item_amount.RemoveAt(i);
                }
                if (!m_is_chou)
                {
                    s_t_item _item = game_data._instance.get_item((int)id);
                    if (_item.type == 9001)
                    {
                        m_need_check_huiyi = true;
                    }
                }
                return;
            }
        }
    }

    public void remove_guanghuan(int id)
    {
        for (int i = 0; i < m_t_player.guanghuan.Count; i++)
        {
            if (id == m_t_player.guanghuan[i])
            {
                m_t_player.guanghuan.RemoveAt(i);
                m_t_player.guanghuan_level.RemoveAt(i);
                return;
            }
        }
    }

    public void add_item(uint id, int num, string info = "")
    {
        add_item(id, num, true, info);
    }
    public void add_item(uint id, int num, bool notify, string info = "")
    {
        if (num <= 0)
        {
            return;
        }
        s_t_item _item = game_data._instance.get_item((int)id);

        if (_item == null)
        {
            return;
        }

        if (_item.tuse == 1)
        {
            s_message _message1 = new s_message();
            _message1.m_type = "catch_item";
            _message1.m_ints.Add((int)id);
            cmessage_center._instance.add_message(_message1);
        }

        if (notify)
        {
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 2, _item.id.ToString(), "[ffffc0]" + _item.name + " [ffd000]x" + num.ToString());
        }
        for (int i = 0; i < m_t_player.item_ids.Count; i++)
        {
            if (id == m_t_player.item_ids[i])
            {
                m_t_player.item_amount[i] += num;
                if (!m_is_chou && _item.type == 9001)
                {
                    m_need_check_huiyi = true;
                }
                return;
            }
        }

        m_t_player.item_ids.Add(id);
        m_t_player.item_amount.Add(num);
        if (!m_is_chou && _item.type == 9001)
        {
            m_need_check_huiyi = true;
        }

        s_message _message = new s_message();
        _message.m_type = "deal_main_gui";
        cmessage_center._instance.add_message(_message);
    }

    public void add_guanghuan(int id)
    {
        add_guanghuan(id, true);
    }
    public void add_guanghuan(int id, bool notify)
    {
        s_t_guanghuan _guanghuan = game_data._instance.get_t_guanghuan(id);

        if (_guanghuan == null)
        {
            return;
        }
        if (notify)
        {
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 7, _guanghuan.id.ToString(), "[ffffc0]" + _guanghuan.name);
        }
        for (int i = 0; i < m_t_player.guanghuan.Count; i++)
        {
            if (id == m_t_player.guanghuan[i])
            {
                return;
            }
        }
        m_t_player.guanghuan.Add(id);
        m_t_player.guanghuan_level.Add(0);
    }

    public void add_chenghao(int id, bool notify)
    {
        s_t_chenghao _chenghao = game_data._instance.get_t_chenhao(id);

        if (_chenghao == null)
        {
            return;
        }
        if (notify)
        {
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("", 0, _chenghao.id.ToString(), "[00ff00]" + s + "[-]" + _chenghao.name + "[-][00ff00]称号" + "[-]");
        }
        for (int i = 0; i < m_t_player.chenghao.Count; i++)
        {
            if (id == m_t_player.chenghao[i])
            {
                if (_chenghao.time > 0)
                {
                    ulong time = timer.now() + (ulong)_chenghao.time * 24 * 3600 * 1000;
                    m_t_player.chengchao_time[i] = time;
                }
                else
                {
                    m_t_player.chengchao_time[i] = 0;
                }
                return;
            }
        }
        sys._instance.chenghao_effect = _chenghao.is_show;
        m_t_player.chenghao.Add(id);
        if (_chenghao.time > 0)
        {
            ulong time = timer.now() + (ulong)_chenghao.time * 24 * 3600 * 1000;
            m_t_player.chengchao_time.Add(time);
        }
        else
        {
            m_t_player.chengchao_time.Add(0);
        }
    }
    public int get_resources_num(int value2)
    {
        int num = 0;
        switch (value2)
        {
            case 1:
                num = m_t_player.gold;
                break;
            case 2:
                num = m_t_player.jewel;
                break;
            /* case 3:
                 break;
             case 4:
                 break;
             case 5:
                 break;
             case 6:
                 break;
             case 7:
                 break;
             case 8:
                 break;
             case 9:
                 break;
             case 10:
                 break;
             case 11:
                 break;
             case 12:
                 break;
             case 13:
                 break;
             case 14:
                 break;
             case 15:
                 break;
             case 16:
                 break;
             case 17:
                 break;
             case 18:
                 break;
             case 19:
                 break;
             case 20:
                 break;
             case 21:
                 break;
             case 22:
                 break;
             case 23:
                 break;
             case 24:
                 break;
             case 25:
                 break;
             case 26:
                 break;
             case 27:
                 break;*/
            default:
                num = 0;
                break;


        }

        return num;
    }
    public int get_item_num(uint id)
    {
        for (int i = 0; i < m_t_player.item_ids.Count; i++)
        {
            if (id == m_t_player.item_ids[i])
            {
                return m_t_player.item_amount[i];
            }
        }

        return 0;
    }
    public void parse_itemnums()
    {
        for (int i = 0; i < m_t_player.item_ids.Count; i++)
        {
            m_item_nums[m_t_player.item_ids[i]] = m_t_player.item_amount[i];
        }
    }
    public int get_card_fragment_num()
    {
        int _num = 0;

        for (int i = 0; i < m_t_player.item_ids.Count; i++)
        {
            if (is_card_fragment(m_t_player.item_ids[i]))
            {
                _num++;
            }
        }

        return _num;
    }

    public int get_equip_fragment_num()
    {
        int _num = 0;

        for (int i = 0; i < m_t_player.item_ids.Count; i++)
        {
            if (is_equip_fragment(m_t_player.item_ids[i]))
            {
                _num++;
            }
        }

        return _num;
    }

    public bool is_zheng(int id)
    {
        if (id == 0)
        {
            return true;
        }

        for (int i = 0; i < m_t_player.zhenxing.Count; i++)
        {
            if (m_t_player.zhenxing[i] == 0)
            {
                continue;
            }
            ccard _card = get_card_guid(m_t_player.zhenxing[i]);

            if (_card != null && _card.m_t_class.id == id)
            {
                return true;
            }
        }

        return false;
    }
    public int zhen_count()
    {
        int count = 0;
        for (int i = 0; i < m_t_player.zhenxing.Count; i++)
        {
            if (m_t_player.zhenxing[i] != 0)
            {
                count++;
            }
        }
        return count;
    }
    public bool is_zheng(ulong guid)
    {
        for (int i = 0; i < m_t_player.zhenxing.Count; i++)
        {
            if (m_t_player.zhenxing[i] == guid)
            {
                return true;
            }
        }

        return false;
    }

    public bool is_gaurd(ulong guid)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            ccard _card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[i]);
            if (_card == null)
            {
                continue;
            }
            if (_card.get_role().pet == guid)
            {
                return true;
            }
        }

        return false;
    }

    public bool is_houyuan(int id)
    {
        if (id == 0)
        {
            return true;
        }

        for (int i = 0; i < m_t_player.houyuan.Count; i++)
        {
            if (m_t_player.houyuan[i] == 0)
            {
                continue;
            }
            ccard _card = get_card_guid(m_t_player.houyuan[i]);

            if (_card != null && _card.m_t_class.id == id)
            {
                return true;
            }
        }

        return false;
    }

    public bool is_houyuan(ulong guid)
    {
        for (int i = 0; i < m_t_player.houyuan.Count; i++)
        {
            if (m_t_player.houyuan[i] == guid)
            {
                return true;
            }
        }

        return false;
    }

    public bool is_card_fragment(uint id)
    {
        s_t_item t_item = game_data._instance.get_item((int)id);
        if (t_item == null)
        {
            return false;
        }
        if (t_item.type == 3001)
        {
            return true;
        }

        return false;
    }

    public bool is_card_jiyin(uint id)
    {
        s_t_item t_item = game_data._instance.get_item((int)id);
        if (t_item == null)
        {
            return false;
        }
        if (t_item.type == 3002)
        {
            return true;
        }

        return false;
    }

    public bool is_pet_fragment(uint id)
    {
        s_t_item t_item = game_data._instance.get_item((int)id);
        if (t_item == null)
        {
            return false;
        }
        if (t_item.type == 10001)
        {
            return true;
        }

        return false;
    }

    public int is_huiyi_finish(int id)
    {


        for (int i = 0; i < m_t_player.huiyi_jihuos.Count; i++)
        {

            if (id == m_t_player.huiyi_jihuos[i])
            {
                if (sys._instance.m_self.m_t_player.huiyi_jihuo_starts[i] == 5)//直接判断星星数  是不是满了
                {
                    return 5;
                }

                s_t_huiyi_sub _sub1 = game_data._instance.get_t_huiyi_sub(id);
                bool flag1 = true;

                for (int j = 0; j < _sub1.huiyis.Count; j++)
                {
                    if (!sys._instance.m_self.m_t_player.item_ids.Contains((uint)_sub1.huiyis[j]))
                    {
                        flag1 = false;
                    }
                }



                if (flag1 && sys._instance.m_self.m_t_player.huiyi_jihuo_starts[i] < 5)//判断星数
                {
                    return 2; //可以升星
                }
                //需要通过星数去判断是不是
                else
                {
                    return 4;//激活未满星,且不可以升星
                }
            }

        }
        s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(id);
        bool flag = true;

        for (int i = 0; i < _sub.huiyis.Count; i++)
        {
            if (!sys._instance.m_self.m_t_player.item_ids.Contains((uint)_sub.huiyis[i]))
            {
                flag = false;
            }
        }

        if (flag)
        {
            return 1;//可激活
        }
        else
        {
            return 3;//未激活
        }

    }

    public bool is_guanghuan_finish(int id)
    {
        int flag = 0;
        s_t_guanghuan_target t_guanghuan_target = game_data._instance.get_t_guanghuan_target(id);
        for (int i = 0; i < t_guanghuan_target.ids.Count; i++)
        {
            if (m_t_player.guanghuan.Contains(t_guanghuan_target.ids[i]))
            {
                flag++;
            }
        }
        if (flag == t_guanghuan_target.ids.Count)
        {
            return true;
        }
        return false;
    }

    public bool is_pet_finish(int id)
    {
        int flat = 0;
        s_t_pet_target t_target = game_data._instance.get_t_pet_target(id);
        for (int i = 0; i < t_target.target_ids.Count; i++)
        {
            for (int j = 0; j < sys._instance.m_self.m_t_player.pets.Count; ++j)
            {
                pet m_pet = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pets[j]);
                if (m_pet == null)
                {
                    continue;
                }
                if (m_pet.m_t_pet.id == t_target.target_ids[i])
                {
                    flat++;
                    break;
                }
            }
        }
        if (flat >= t_target.target_ids.Count)
        {
            return true;
        }
        return false;
    }
    public bool is_equip_fragment(uint id)
    {
        s_t_item t_item = game_data._instance.get_item((int)id);
        if (t_item == null)
        {
            return false;
        }
        if (t_item.type == 7001)
        {
            return true;
        }

        return false;
    }

    public dhc.player_t get_t_player()
    {
        return m_t_player;
    }

    public int get_equip_num()
    {
        return m_equips.Count;
    }

    public dhc.equip_t get_equip_index(int index)
    {
        return m_equips[index];
    }

    public dhc.equip_t get_equip_guid(ulong guid)
    {
        for (int i = 0; i < this.m_equips.Count; i++)
        {
            dhc.equip_t equip = this.m_equips[i];

            if (equip.guid == guid)
            {
                return equip;
            }
        }

        return null;
    }

    public bool has_equip_id(int id)
    {
        for (int i = 0; i < this.m_equips.Count; i++)
        {
            dhc.equip_t equip = this.m_equips[i];

            if (equip.template_id == id)
            {
                return true;
            }
        }

        return false;
    }

    public void add_equip(dhc.equip_t equip)
    {
        add_equip(equip, true);
    }
    public void add_equip(dhc.equip_t equip, bool notify)
    {
        m_equips.Add(equip);
        m_t_player.equips.Add(equip.guid);

        if (notify)
        {
            s_t_equip _equip = game_data._instance.get_t_equip(equip.template_id);
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 4, _equip.id.ToString(), "[ffffc0]" + _equip.name + " [ffd000]x1");
        }
    }

    public void add_dress(int id)
    {
        add_dress(id, true);
    }
    public void add_dress(int id, bool notify)
    {
        s_t_dress t_dress = game_data._instance.get_t_dress(id);
        if (t_dress == null)
        {
            return;
        }
        bool flag = false;
        for (int i = 0; i < m_t_player.dress_ids.Count; ++i)
        {
            if (m_t_player.dress_ids[i] == id)
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            m_t_player.dress_ids.Add(id);
        }
        if (notify)
        {
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 9, t_dress.id.ToString(), "[ffffc0]" + t_dress.name + " [ffd000]x1");
        }
    }

    public void add_pet(dhc.pet_t pet)
    {
        add_pet(pet, true);
    }
    public void add_pet(dhc.pet_t pet, bool notify)
    {
        if (notify)
        {
            s_t_pet _chongwu = game_data._instance.get_t_pet(pet.template_id);
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 11, _chongwu.id.ToString(), "[ffffc0]" + _chongwu.name + " [ffd000]x1");
        }
        pet _pet = new pet();
        _pet.set_pet(pet);
        m_pets.Add(_pet);
        _pet.m_player = this;
        m_t_player.pets.Add(pet.guid);
    }


    public void add_equip_login(dhc.equip_t equip)
    {
        m_equips.Add(equip);
    }

    public void remove_equip(ulong guid)
    {
        for (int i = 0; i < m_equips.Count; ++i)
        {
            if (m_equips[i].guid == guid)
            {
                m_equips.Remove(m_equips[i]);
                break;
            }
        }
        m_t_player.equips.Remove(guid);
    }

    public void add_reward(int type, int value1, int value2, int value3, string info = "")
    {
        add_reward(type, value1, value2, value3, true, info);
    }

    public void add_reward(int type, int value1, int value2, int value3, bool notify, string info = "")
    {
        s_message _msg = new s_message();
        _msg.m_type = "action";
        _msg.m_string.Add("kai_xing");
        cmessage_center._instance.add_message(_msg);

        if (type == 1)
        {
            if (value1 == 1)
            {
                add_att(e_player_attr.player_gold, value2 + value3 * m_t_player.level, notify, info);
                if (m_t_player.gold < 0)
                {
                    m_t_player.gold = 0;
                }
            }
            else if (value1 == 2)
            {
                add_att(e_player_attr.player_jewel, value2 + value3 * m_t_player.level, notify, info);
                if (m_t_player.jewel < 0)
                {
                    m_t_player.jewel = 0;
                }
            }
            else if (value1 == 3)
            {
                add_att(e_player_attr.player_tili, value2 + value3 * m_t_player.level, notify);
                if (m_t_player.tili < 0)
                {
                    m_t_player.tili = 0;
                }
            }
            else if (value1 == 4)
            {
                add_att(e_player_attr.player_exp, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 5)
            {
                add_att(e_player_attr.player_jjc_point, value2 + value3 * m_t_player.level, notify);
                if (m_t_player.jjc_point < 0)
                {
                    m_t_player.jjc_point = 0;
                }
            }
            else if (value1 == 6)
            {
                add_att(e_player_attr.player_hj, value2 + value3 * m_t_player.level, notify);
                if (m_t_player.mw_point < 0)
                {
                    m_t_player.mw_point = 0;
                }
            }
            else if (value1 == 7)
            {
                add_att(e_player_attr.player_yuanli, value2 + value3 * m_t_player.level, notify);
                if (m_t_player.yuanli < 0)
                {
                    m_t_player.yuanli = 0;
                }
            }
            else if (value1 == 8)
            {
                add_att(e_player_attr.player_score, value2 + value3 * m_t_player.level, notify);
                if (m_t_player.active_score < 0)
                {
                    m_t_player.active_score = 0;
                }
            }
            else if (value1 == 9)
            {
                add_att(e_player_attr.player_vip_exp, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 10)
            {
                add_att(e_player_attr.player_contribution, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 11)
            {
                add_att(e_player_attr.player_baoshi, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 12)
            {
                add_att(e_player_attr.player_ts, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 13)
            {
                add_att(e_player_attr.player_medal_point, value2 + value3 * m_t_player.level, notify);
                if (m_t_player.medal_point < 0)
                {
                    m_t_player.medal_point = 0;
                }
            }
            else if (value1 == 14)
            {
                add_att(e_player_attr.player_treasure_powder, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 15)
            {
                add_att(e_player_attr.player_treasure_energy, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 16)
            {

                add_att(e_player_attr.player_guild_hor, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 17)
            {
                add_att(e_player_attr.player_skill_point, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 18)
            {
                add_att(e_player_attr.player_boss_num, value2, notify);
            }
            else if (value1 == 19)
            {
                add_att(e_player_attr.player_dress_tuzhi, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 20)
            {
                add_att(e_player_attr.player_luck_point, value2 + value3 * m_t_player.level, notify);

            }
            else if (value1 == 21)
            {
                add_att(e_player_attr.player_huiyi_point, value2 + value3 * m_t_player.level, notify);

            }
            else if (value1 == 22)
            {
                add_att(e_player_attr.player_pvp_jz, value2 + value3 * m_t_player.level, notify);
            }
            else if (value1 == 24)
            {
                add_att(e_player_attr.player_bingyuan_point, value2 + value3 * m_t_player.level, notify);

            }
            else if (value1 == 27)
            {
                add_att(e_player_attr.player_xinpian, value2 + value3 * m_t_player.level, notify);

            }
            else if (value1 == 29)
            {
                add_att(e_player_attr.player_friend_point, value2 + value3 * m_t_player.level, notify, info);
                if (m_t_player.youqingdian < 0)
                {
                    m_t_player.youqingdian = 0;
                }

            }
        }
        else if (type == 2)
        {
            add_item((uint)value1, value2, notify, info);
        }
        else if (type == 5)
        {
            add_role_dress(value1, notify);
        }
        else if (type == 7)
        {
            add_guanghuan(value1, notify);
        }
        else if (type == 8)
        {
            add_chenghao(value1, notify);
        }
        else if (type == 9)
        {
            add_dress(value1, notify);
        }
    }

    public void add_att(e_player_attr att, int val, string info = "")
    {

        add_att(att, val, true, info);

    }
    public void add_att(e_player_attr att, int val, bool notify, string info = "")
    {
        int _att = this.get_att(att) + val;

        this.set_att(att, _att, notify, info);
    }

    public void sub_att(e_player_attr att, int val, string info = "")
    {
        int _att = this.get_att(att) - val;
        this.set_att(att, _att, false, info);
    }

    public void set_att(e_player_attr att, int val, string info = "")
    {
        set_att(att, val, true);
    }
    public void set_att(e_player_attr att, int val, bool notify, string info = "")
    {
        s_message _message = new s_message();
        _message.m_type = "update_player_att";
        cmessage_center._instance.add_message(_message);
        s_t_resource res = game_data._instance.get_t_resource((int)att);
        if (val < 0)
        {
            val = 0;
        }
        switch (att)
        {
            case e_player_attr.player_gold:
                {
                    if (val > m_t_player.gold && notify)
                    {
                        sys._instance.play_sound("sound/ef_coin");
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("kaifu_gui.cs_242_19");//金币
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.gold).ToString());

                    }
                    m_t_player.gold = val;
                }
                break;
            case e_player_attr.player_jewel:
                {
                    if (val > m_t_player.jewel && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("czfp_gui.cs_360_108");//钻石
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.jewel).ToString());
                    }
                    m_t_player.jewel = val;
                }
                break;
            case e_player_attr.player_score:
                m_t_player.active_score = val;
                break;
            case e_player_attr.player_tili:
                {
                    if (val > m_t_player.tili && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1590_17");//体力
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.tili).ToString());
                    }

                    m_t_player.tili = val;
                }
                break;
            case e_player_attr.player_level:
                m_t_player.level = val;
                break;
            case e_player_attr.player_exp:
                {
                    if (val > m_t_player.exp && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1606_16");//经验
                        sys._instance.play_sound("sound/ef_coin");
                        string icon = res.smallicon;
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, icon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.exp).ToString());
                    }
                    m_t_player.exp = val;
                }
                bool need_check = false;
                bool flag = false;
                s_t_exp t_exp = game_data._instance.get_t_exp(m_t_player.level + 1);
                if (t_exp == null)
                {
                    m_t_player.exp = 0;
                    break;
                }
                int ptili = m_t_player.tili;
                int pnl = m_t_player.energy;
                while (m_t_player.exp >= t_exp.exp)
                {
                    flag = true;
                    m_t_player.exp -= t_exp.exp;
                    m_t_player.level++;

                    int tili = 0;
                    int energy = 0;
                    if (m_t_player.level < 30)
                    {
                        tili = get_max_tili();
                        energy = get_max_nl();
                    }
                    else if (m_t_player.level < 50)
                    {
                        tili = 200;
                        energy = 60;
                    }
                    else
                    {
                        tili = 500;
                        energy = 150;
                    }
                    if (m_t_player.tili < tili)
                    {
                        if (m_t_player.tili + t_exp.regain_tili > tili)
                        {
                            m_t_player.tili = tili;
                        }
                        else
                        {
                            m_t_player.tili += t_exp.regain_tili;
                        }
                    }
                    if (m_t_player.energy < energy)
                    {
                        if (m_t_player.energy + 10 > energy)
                        {
                            m_t_player.energy = energy;
                        }
                        else
                        {
                            m_t_player.energy += 10;
                        }
                    }
                    if (t_exp.type > 0)
                    {
                        need_check = true;
                    }

                    t_exp = game_data._instance.get_t_exp(m_t_player.level + 1);
                    if (t_exp == null)
                    {
                        m_t_player.exp = 0;
                        break;
                    }
                    if (m_t_player.level == (int)e_open_level.el_treasure_qu)
                    {
                        add_item(60020012, 1, false, game_data._instance.get_t_language("player.cs_1685_48"));//升级获得
                        add_item(60020013, 1, false, game_data._instance.get_t_language("player.cs_1685_48"));//升级获得
                    }
                    if (m_t_player.level == 7)
                    {
                        game_data._instance.m_player_data.m_speed = 2;
                        game_data._instance.save();
                    }
                    if (m_t_player.level == 20)
                    {
                        game_data._instance.m_player_data.m_speed = 3;
                        game_data._instance.save();
                    }
                    if (m_t_player.level == 10)
                    {
                        sys._instance.m_self.add_att(e_player_attr.player_treasure_powder, 200, false);
                    }
                    if (m_t_player.level == 70)
                    {
                        add_item(100011001, 25, false, game_data._instance.get_t_language("player.cs_1685_48"));//升级获得
                    }
                }
                if (flag)
                {

                    s_message _msg = new s_message();
                    _msg.m_type = "show_levelup";
                    _msg.m_ints.Add(ptili);
                    _msg.m_ints.Add(pnl);
                    cmessage_center._instance.add_message(_msg);
                    check_target_done();
                    check_huiyi_xuyao();
                    platform._instance.on_game_user_upgrade(m_t_player.level);

                    _msg = new s_message();
                    _msg.m_type = "do_set_storage";
                    cmessage_center._instance.add_message(_msg);
                }
                if (need_check)
                {
                    sys._instance.player_need_check();
                }
                break;
            case e_player_attr.player_jjc_point:
                {
                    if (val > m_t_player.jjc_point && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("kaifu_gui.cs_247_19");//战魂
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.jjc_point).ToString());
                    }
                    m_t_player.jjc_point = val;
                }
                break;
            case e_player_attr.player_hj:
                {
                    if (val > m_t_player.mw_point && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1740_17");//合金
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.mw_point).ToString());
                    }
                    m_t_player.mw_point = val;
                }
                break;
            case e_player_attr.player_mp:
                {
                    int _max = m_max_mp;

                    if (val > _max)
                    {
                        val = _max;
                    }

                    if (val < 0)
                    {
                        val = 0;
                    }

                    m_mp.set_int(val);
                }
                break;
            case e_player_attr.player_vip_exp:
                {
                    if (val > m_t_player.vip_exp && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1769_17");//VIP经验
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s + "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.vip_exp).ToString());
                    }
                    m_t_player.vip_exp = val;

                    s_t_vip t_nvip = game_data._instance.get_t_vip(m_t_player.vip);
                    s_t_vip t_vip = game_data._instance.get_t_vip(m_t_player.vip + 1);
                    if (t_vip == null)
                    {
                        m_t_player.vip_exp = t_nvip.recharge;
                        break;
                    }
                    while (m_t_player.vip_exp >= t_vip.recharge)
                    {
                        m_t_player.vip++;
                        t_nvip = t_vip;
                        t_vip = game_data._instance.get_t_vip(m_t_player.vip + 1);
                        m_t_player.huodong_vip_libao = 0;
                        if (t_vip == null)
                        {
                            m_t_player.vip_exp = t_nvip.recharge;
                            break;
                        }
                    }
                }
                break;
            case e_player_attr.player_yuanli:
                {
                    if (val > m_t_player.yuanli && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("kaifu_gui.cs_252_19");//原力
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.yuanli).ToString());
                        s_t_exp t_expp1 = game_data._instance.get_t_exp(m_t_player.level);
                    }
                    m_t_player.yuanli = val;
                }
                int pyuanli = m_t_player.yuanli;
                if (pyuanli < 0)
                {
                    pyuanli = 0;
                }
                /*int level = m_t_player.level;
                s_t_exp t_expp = game_data._instance.get_t_exp(m_t_player.level);
                if (pyuanli > t_expp.yuanli)
                {
                    pyuanli = t_expp.yuanli;
                }*/
                m_t_player.yuanli = pyuanli;
                break;
            case e_player_attr.player_contribution:
                {
                    if (val > m_t_player.contribution && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1826_17");//贡献
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.contribution).ToString());
                    }
                    m_t_player.contribution = val;
                }
                break;
            case e_player_attr.player_guild_exp:
                if (juntuan_gui._instance != null)
                {
                    int exp = val;
                    s_t_guild t_guild = game_data._instance.get_guild(juntuan_gui._instance.m_guild_t.level + 1);
                    if (t_guild.level == 11)
                    {
                        juntuan_gui._instance.m_guild_t.exp = exp;
                        break;
                    }
                    if (t_guild != null && exp >= t_guild.exp)
                    {
                        exp = exp - t_guild.exp;
                        juntuan_gui._instance.m_guild_t.level++;
                    }
                    juntuan_gui._instance.m_guild_t.exp = exp;
                }
                break;
            case e_player_attr.player_treasure_powder:
                {
                    if (val > m_t_player.powder && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1856_16");//荣誉
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.powder).ToString());//
                    }
                    m_t_player.powder = val;
                }
                break;
            case e_player_attr.player_treasure_energy:
                {
                    if (val > m_t_player.energy && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1868_16");//能量
                        sys._instance.play_sound("sound/ef_coin");
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[32eef7]" + s1 + "[ffd000] + " + (val - m_t_player.energy).ToString());
                    }
                    m_t_player.energy = val;
                }
                break;
            case e_player_attr.player_guild_hor:
                {
                    if (val > juntuan_gui._instance.m_guild_t.honor && notify)
                    {
                        sys._instance.play_sound("sound/ef_coin");
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("jt_mobai.cs_424_36");//军团荣誉
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, "", "[ffffc0]" + s1 + "[ffd000] + " + (val - juntuan_gui._instance.m_guild_t.honor).ToString());
                        // juntuan_gui._instance.m_guild_t.honor += val - juntuan_gui._instance.m_guild_t.honor;
                    }
                    juntuan_gui._instance.m_guild_t.honor = val;
                }
                break;
            case e_player_attr.player_medal_point:
                {
                    if (val > m_t_player.medal_point && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1893_16");//魔王勋章
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.medal_point).ToString());

                    }
                    m_t_player.medal_point = val;
                }
                break;
            case e_player_attr.player_boss_num:
                {
                    if (val > m_t_player.boss_num && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1905_32");//魔王挑战次数
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, "", "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.boss_num).ToString());

                    }
                    m_t_player.boss_num = val;
                }
                break;
            case e_player_attr.player_dress_tuzhi:
                {
                    if (val > m_t_player.dress_tuzhi && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_language("player.cs_1917_16");//时装设计图
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, "", "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.dress_tuzhi).ToString());

                    }
                    m_t_player.dress_tuzhi = val;
                }
                break;
            case e_player_attr.player_pvp_jz:
                {
                    if (val > m_t_player.lieren_point && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_resource(22).name;
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.lieren_point).ToString());

                    }
                    m_t_player.lieren_point = val;
                }
                break;
            case e_player_attr.player_luck_point:
                {
                    if (val > m_t_player.luck_point && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_resource(20).name;
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.luck_point).ToString());

                    }
                    m_t_player.luck_point = val;
                }
                break;
            case e_player_attr.player_huiyi_point:
                {
                    if (val > m_t_player.huiyi_point && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_resource(21).name;
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.huiyi_point).ToString());

                    }
                    m_t_player.huiyi_point = val;
                }
                break;
            case e_player_attr.player_bingyuan_point:
                {
                    if (val > m_t_player.bingjing && notify)
                    {
                        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                        string s1 = game_data._instance.get_t_resource(24).name;
                        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.bingjing).ToString());

                    }
                    m_t_player.bingjing = val;
                }
                break;
            case e_player_attr.player_xinpian:
                if (val > m_t_player.xinpian && notify)
                {
                    string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                    string s1 = game_data._instance.get_t_resource(27).name;
                    root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - m_t_player.xinpian).ToString());
                }
                m_t_player.xinpian = val;
                break;
            case e_player_attr.player_mofang_jifen:
                if (val > mofang_gui._instance.m_view_data.left_point && notify)
                {
                    string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                    string s1 = game_data._instance.get_t_resource(28).name;
                    root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[ffffc0]" + s1 + "[ffd000] + " + (val - mofang_gui._instance.m_view_data.left_point).ToString());
                }
                mofang_gui._instance.m_view_data.left_point = val;
                break;
            case e_player_attr.player_friend_point:
                if (val > m_t_player.youqingdian && notify)
                {
                    string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
                    string s1 = game_data._instance.get_t_language("player.cs_2092_16");//友情点
                    sys._instance.play_sound("sound/ef_coin");
                    root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, res.smallicon, "[32eef7]" + s1 + "[ffd000] + " + (val - m_t_player.youqingdian).ToString());
                }
                m_t_player.youqingdian = val;
                break;
        };
    }
    public int get_att(e_player_attr att)
    {
        int _val = 0;

        switch (att)
        {
            case e_player_attr.player_gold:
                _val = m_t_player.gold;
                break;
            case e_player_attr.player_jewel:
                _val = m_t_player.jewel;
                break;
            case e_player_attr.player_tili:
                _val = m_t_player.tili;
                break;
            case e_player_attr.player_level:
                _val = m_t_player.level;
                break;
            case e_player_attr.player_exp:
                _val = m_t_player.exp;
                break;
            case e_player_attr.player_jjc_point:
                _val = m_t_player.jjc_point;
                break;
            case e_player_attr.player_hj:
                _val = m_t_player.mw_point;
                break;
            case e_player_attr.player_mp:
                _val = m_mp.get_int();
                break;
            case e_player_attr.player_vip_exp:
                _val = m_t_player.vip_exp;
                break;
            case e_player_attr.player_yuanli:
                _val = m_t_player.yuanli;
                break;
            case e_player_attr.player_score:
                _val = m_t_player.active_score;
                break;
            case e_player_attr.player_contribution:
                _val = m_t_player.contribution;
                break;
            /*case e_player_attr.player_baoshi:
                _val = m_t_player.baoshi;
                break;*/
            case e_player_attr.player_treasure_powder:
                _val = m_t_player.powder;
                break;
            case e_player_attr.player_treasure_energy:
                _val = m_t_player.energy;
                break;
            case e_player_attr.player_guild_hor:
                _val = juntuan_gui._instance.m_guild_t.honor;
                break;
            case e_player_attr.player_medal_point:
                _val = m_t_player.medal_point;
                break;
            case e_player_attr.player_dress_tuzhi:
                _val = m_t_player.dress_tuzhi;
                break;
            case e_player_attr.player_boss_num:
                _val = m_t_player.boss_num;
                break;
            case e_player_attr.player_bf:
                _val = m_t_player.bf;
                break;
            case e_player_attr.player_luck_point:
                _val = m_t_player.luck_point;
                break;
            case e_player_attr.player_huiyi_point:
                _val = m_t_player.huiyi_point;
                break;
            case e_player_attr.player_pvp_jz:
                _val = m_t_player.lieren_point;
                break;
            case e_player_attr.player_bingyuan_point:
                _val = m_t_player.bingjing;
                break;
            case e_player_attr.player_xinpian:
                _val = m_t_player.xinpian;

                break;
            case e_player_attr.player_mofang_jifen:
                if (mofang_gui._instance != null)
                {
                    _val = mofang_gui._instance.m_view_data.left_point;
                }
                break;
            case e_player_attr.player_friend_point:
                _val = m_t_player.youqingdian;
                break;

        };

        return _val;
    }

    public void add_active(int id, int num)
    {
        s_t_active t_active = game_data._instance.get_t_active(id);
        bool flag = false;
        for (int i = 0; i < m_t_player.active_id.Count; ++i)
        {
            if (m_t_player.active_id[i] == t_active.id)
            {
                flag = true;
                m_t_player.active_num[i] = m_t_player.active_num[i] + num;
                if (m_t_player.active_num[i] > t_active.num)
                {
                    m_t_player.active_num[i] = t_active.num;
                    break;
                }
            }
        }
        if (!flag)
        {
            m_t_player.active_id.Add(t_active.id);
            m_t_player.active_reward.Add(0);
            if (num > t_active.num)
            {
                m_t_player.active_num.Add(t_active.num);
            }
            else
            {
                m_t_player.active_num.Add(num);
            }
        }
    }

    public void check_target_done()
    {
        s_message _mes = new s_message();
        _mes.m_type = "check_target_done";
        cmessage_center._instance.add_message(_mes);
    }

    public void check_dress_target_done()
    {
        s_message _mes = new s_message();
        _mes.m_type = "check_dress_target_done";
        cmessage_center._instance.add_message(_mes);
    }
    public void refresh()
    {
        m_t_player.mission_cishu_ids.Clear();
        m_t_player.mission_cishu.Clear();
        m_t_player.mission_goumai_ids.Clear();
        m_t_player.mission_goumai.Clear();
        m_t_player.shop1_refresh_num = 0;
        m_t_player.shop3_refresh_num = 0;
        m_t_player.shop4_refresh_num = 0;
        m_t_player.shoppet_num = 0;
        sys._instance.m_self.m_t_player.guild_red_num = 0;
        sys._instance.m_self.m_t_player.guild_red_num1 = 0;
        int run_day = timer.run_day(sys._instance.m_self.m_t_player.birth_time);
        if (run_day < 14 && run_day >= 7)
        {
            for (int i = 8; i <= 14; ++i)
            {
                if (run_day >= i)
                {
                    m_kaifu_banjia[i - 8] = 1;
                }
                else
                {
                    m_kaifu_banjia[i - 8] = 0;
                }
            }
        }
        m_t_player.shop3_ids.Clear();
        m_t_player.shop3_sell.Clear();
        sys._instance.m_self.m_t_player.huodong_vip_libao = 0;
        if (juntuan_gui._instance != null && juntuan_gui._instance.m_guild_t != null)
        {
            juntuan_gui._instance.m_guild_t.honor = 0;

        }
        m_t_player.shop_xg_ids.Clear();
        m_t_player.shop_xg_nums.Clear();
        m_t_player.active_id.Clear();
        m_t_player.active_num.Clear();
        m_t_player.boss_active_ids.Clear();
        m_t_player.pvp_hit = 0;
        m_t_player.pvp_hit_ids.Clear();
        m_t_player.ds_hit = 0;
        m_t_player.ds_hit_ids.Clear();
        m_t_player.boss_active_nums.Clear();
        m_t_player.boss_active_rewards.Clear();
        m_t_player.active_reward.Clear();
        m_t_player.huiyi_shop_ids.Clear();
        m_t_player.by_shops.Clear();
        m_t_player.by_nums.Clear();
        m_t_player.huiyi_shop_nums.Clear();
        m_t_player.huiyi_chou_num = 0;
        m_t_player.huiyi_gaiyun_num = 0;
        m_t_player.huiyi_zhanpu_num = 0;
        m_t_player.by_reward_num = 3;
        m_t_player.by_reward_buy = 0;
        m_t_player.guild_pvp_num = 5;
        m_t_player.guild_pvp_buy_num = 0;
        for (int i = 0; i < m_t_player.treasures.Count; ++i)
        {
            ulong guid = m_t_player.treasures[i];
            dhc.treasure_t treasure = sys._instance.m_self.get_treasure_guid(guid);
            if (treasure != null)
            {
                treasure.star_luck = 0;
            }
        }

        add_active(100, 1);
        add_active(101, 1);
        if (m_t_player.zhouka_time > timer.now())
        {
            add_active(200, 1);
        }
        if (m_t_player.yueka_time > timer.now())
        {
            add_active(300, 1);
        }
        m_t_player.active_score = 0;
        m_t_player.active_score_id.Clear();
        m_t_player.dj_num = 0;
        m_t_player.tili_reward = 0;
        m_t_player.daily_sign_index = m_t_player.daily_sign_reward + 1;
        m_t_player.hbb_finish_num = 0;
        m_t_player.hbb_num = 5;
        m_t_player.hbb_refresh_num = 0;
        m_t_player.ore_finish_num = 0;
        sys._instance.m_self.m_t_player.pvp_num = 15;
        m_t_player.pvp_refresh_num = 0;
        m_t_player.pvp_buy_num = 0;
        m_t_player.ttt_dead = 0;
        m_t_player.ttt_star = 0;
        m_t_player.ttt_reward_ids.Clear();
        m_t_player.ttt_cur_stars.Clear();
        m_t_player.ttt_cur_reward_ids.Clear();
        m_t_player.ttt_cz_num = 0;
        m_t_player.random_event_num = 0;
        s_t_online_reward t_online_reward = game_data._instance.get_t_online_reward(0);
        m_t_player.online_reward_time = timer.now() + (ulong)t_online_reward.time;
        m_t_player.online_reward_index = 0;
        m_t_player.social_shou_num = 0;
        m_t_player.yb_finish_num = 0;
        m_t_player.yb_refresh_num = 0;
        m_t_player.ybq_finish_num = 0;
        m_t_player.ds_reward_num = 15;
        m_t_player.ds_reward_buy = 0;
        m_t_player.ttt_mibao = 0;
        m_t_player.qiyu_mission.Clear();
        m_t_player.qiyu_hard.Clear();
        m_t_player.qiyu_suc.Clear();
        s_message _msg = new s_message();
        _msg.m_type = "deal_main_gui";
        cmessage_center._instance.add_message(_msg);

        _msg = new s_message();
        _msg.m_type = "daily_refresh";
        cmessage_center._instance.add_message(_msg);
        string s = game_data._instance.get_t_language("player.cs_2252_13");//零点已过，每日活动已刷新
        root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
    }

    private void week_refresh()
    {
        m_t_player.shop_xg_ids.Clear();
        m_t_player.shop_xg_nums.Clear();
        m_t_player.by_rewards.Clear();
        m_t_player.huodong_week_libao.Clear();
        sys._instance.m_self.m_t_player.by_point = 0;
        sys._instance.m_self.m_t_player.pvp_total = 0;

    }

    private void month_refresh()
    {

    }

    public void sub_res(int value1, int value2, string info = "")
    {
        if (value1 == 1)
        {
            sub_att(e_player_attr.player_gold, value2, info);
        }
        else if (value1 == 2)
        {
            sub_att(e_player_attr.player_jewel, value2, info);
        }
        else if (value1 == 5)
        {
            sub_att(e_player_attr.player_jjc_point, value2);
        }
        else if (value1 == 6)
        {
            sub_att(e_player_attr.player_hj, value2);
        }
        else if (value1 == 13)
        {
            sub_att(e_player_attr.player_medal_point, value2);
        }

        else if (value1 == 14)
        {
            sub_att(e_player_attr.player_contribution, value2);
        }
        else if (value1 == 17)
        {
            sub_att(e_player_attr.player_skill_point, value2);
        }
        else if (value1 == 19)
        {
            sub_att(e_player_attr.player_dress_tuzhi, value2);
        }
        else if (value1 == 21)
        {
            sub_att(e_player_attr.player_huiyi_point, value2);

        }
        else
        {
            sub_att((e_player_attr)value1, value2);

        }
    }

    public void update()
    {
        if (m_t_player == null)
        {
            return;
        }

        if (timer.trigger_time(m_time, 0, 0))
        {
            refresh();
        }
        if (timer.trigger_qiyu_time(m_time))
        {
            protocol.game.cmsg_common msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_QIYU_CHECK, msg);
        }
        if (timer.trigger_huiyi_shop_time(m_time))
        {
            protocol.game.cmsg_common msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_SHOP_CHECK, msg);
        }
        if (timer.trigger_week_time(m_time))
        {
            week_refresh();
        }
        if (timer.trigger_month_time(m_time))
        {
            month_refresh();
        }
        m_time = timer.now();

        ulong now = timer.now();
        if (m_t_player.tili >= get_max_tili())
        {
            m_t_player.last_tili_time = now;
        }
        else
        {
            int tili_time = 6 * 60 * 1000;
            int dtime = (int)((long)now - (long)m_t_player.last_tili_time);
            if (dtime > tili_time)
            {
                int num = (int)(dtime / tili_time);
                if (num > get_max_tili() - m_t_player.tili)
                {
                    m_t_player.tili = get_max_tili();
                    m_t_player.last_tili_time = now;
                }
                else
                {
                    m_t_player.tili = m_t_player.tili + num;
                    m_t_player.last_tili_time = m_t_player.last_tili_time + (ulong)(num * tili_time);
                }
            }
        }
        if (m_t_player.boss_num >= 10)
        {
            m_t_player.boss_last_time = now;
        }
        else
        {
            int boss_time = 60 * 60 * 1000;
            int dtime = (int)((long)now - (long)m_t_player.boss_last_time);
            if (dtime > boss_time)
            {
                int num = (int)(dtime / boss_time);
                if (num > 10 - m_t_player.boss_num)
                {
                    m_t_player.boss_num = 10;
                    m_t_player.boss_last_time = now;
                }
                else
                {
                    m_t_player.boss_num = m_t_player.boss_num + num;
                    m_t_player.boss_last_time = m_t_player.boss_last_time + (ulong)(num * boss_time);
                }
            }
        }
        if (m_t_player.energy >= 30)
        {
            m_t_player.last_energy_time = now;
        }
        else
        {
            int energy_time = 30 * 60 * 1000;
            int dtime = (int)((long)now - (long)m_t_player.last_energy_time);
            if (dtime > energy_time)
            {
                int num = (int)(dtime / energy_time);
                if (num > 30 - m_t_player.energy)
                {
                    m_t_player.energy = 30;
                    m_t_player.last_energy_time = now;
                }
                else
                {
                    m_t_player.energy = m_t_player.energy + num;
                    m_t_player.last_energy_time = m_t_player.last_energy_time + (ulong)(num * energy_time);
                }
            }
        }
        if (m_t_player.shop2_refresh_num >= 10)
        {
            m_t_player.shop_last_time = now;
        }
        else
        {
            int shop_time = 2 * 60 * 60 * 1000;
            int dtime = (int)((long)now - (long)m_t_player.shop_last_time);
            if (dtime > shop_time)
            {
                int num = (int)(dtime / shop_time);
                if (num > 10 - m_t_player.shop2_refresh_num)
                {
                    m_t_player.shop2_refresh_num = 10;
                    m_t_player.shop_last_time = now;
                }
                else
                {
                    m_t_player.shop2_refresh_num = m_t_player.shop2_refresh_num + num;
                    m_t_player.shop_last_time = m_t_player.shop_last_time + (ulong)(num * shop_time);
                }
            }
        }
        if (m_t_player.shoppet_refresh_num >= 10)
        {
            m_t_player.shoppet_last_time = now;
        }
        else
        {
            int shop_time = 2 * 60 * 60 * 1000;
            int dtime = (int)((long)now - (long)m_t_player.shoppet_last_time);
            if (dtime > shop_time)
            {
                int num = (int)(dtime / shop_time);
                if (num > 10 - m_t_player.shoppet_refresh_num)
                {
                    m_t_player.shoppet_refresh_num = 10;
                    m_t_player.shoppet_last_time = now;
                }
                else
                {
                    m_t_player.shoppet_refresh_num = m_t_player.shop2_refresh_num + num;
                    m_t_player.shoppet_last_time = m_t_player.shop_last_time + (ulong)(num * shop_time);
                }
            }
        }
        for (int i = 0; i < m_t_player.chenghao.Count; ++i)
        {
            if (m_t_player.chengchao_time[i] > 0)
            {
                if (timer.now() > m_t_player.chengchao_time[i])
                {
                    if (m_t_player.chenghao[i] == m_t_player.chenghao_on)
                    {
                        m_t_player.chenghao_on = 0;
                    }
                    m_t_player.chenghao.RemoveAt(i);
                    m_t_player.chengchao_time.RemoveAt(i);
                }
            }
        }
    }

    public static string get_touxiang(int id)
    {
        s_t_class t_class = game_data._instance.get_t_class(id);
        if (t_class == null)
        {
            return "";
        }
        return t_class.icon;
    }

    public string get_touxiang()
    {
        return get_touxiang((int)m_t_player.template_id);
    }

    public int get_max_equip_num()
    {
        return 100;
    }

    public bool has_dress(int id)
    {
        for (int i = 0; i < m_t_player.dress_ids.Count; ++i)
        {
            if (m_t_player.dress_ids[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool has_dress_on(int id)
    {
        for (int i = 0; i < m_t_player.dress_on_ids.Count; ++i)
        {
            if (m_t_player.dress_on_ids[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public List<string> player_dress_check(int id)
    {
        check_dress_target_done();
        List<string> sxs = new List<string>();
        int dress_id = id;
        s_t_dress _dress = game_data._instance.get_t_dress(dress_id);
        string text = "";
        text += string.Format(game_data._instance.get_t_language("player.cs_2534_24"), _dress.name);//[00ff00]获得[-][f9d420]{0}[-] [ffd000]x1[-]
        sxs.Add(text);
        for (int i = 0; i < _dress.attrs.Count; ++i)
        {
            string s = "[00ff00]" + game_data._instance.get_value_string(_dress.attrs[i].attr, _dress.attrs[i].value, 1) + "[-]";
            sxs.Add(s);
        }
        List<s_t_dress_target> temp_dress = new List<s_t_dress_target>();
        List<s_t_dress_target> temp_dress1 = new List<s_t_dress_target>();
        List<int> dress_ids = new List<int>();
        Dictionary<int, int> attrs = new Dictionary<int, int>();
        dbc m_dress_target = game_data._instance.m_dbc_dress_target;
        for (int i = 0; i < m_dress_target.get_y(); ++i)
        {
            s_t_dress_target temp = game_data._instance.get_t_dress_target(int.Parse(m_dress_target.get(0, i)));
            if (check_dress_target_done(temp, sys._instance.m_self.m_t_player.dress_ids))
            {
                if (attrs.ContainsKey(temp.attr1))
                {
                    attrs[temp.attr1] += temp.value1;
                }
                else
                {
                    attrs.Add(temp.attr1, temp.value1);
                }
                if (attrs.ContainsKey(temp.attr2))
                {
                    attrs[temp.attr2] += temp.value2;
                }
                else
                {
                    attrs.Add(temp.attr2, temp.value2);
                }
                if (attrs.ContainsKey(temp.attr3))
                {
                    attrs[temp.attr3] += temp.value3;
                }
                else
                {
                    attrs.Add(temp.attr3, temp.value3);
                }
                if (attrs.ContainsKey(temp.attr4))
                {
                    attrs[temp.attr4] += temp.value4;
                }
                else
                {
                    attrs.Add(temp.attr4, temp.value4);
                }
                temp_dress.Add(temp);
            }

        }
        for (int i = 0; i < sys._instance.m_self.m_t_player.dress_ids.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.dress_ids[i] != id)
            {
                dress_ids.Add(sys._instance.m_self.m_t_player.dress_ids[i]);
            }
        }
        for (int i = 0; i < m_dress_target.get_y(); ++i)
        {
            s_t_dress_target temp = game_data._instance.get_t_dress_target(int.Parse(m_dress_target.get(0, i)));
            if (check_dress_target_done(temp, dress_ids))
            {
                if (attrs.ContainsKey(temp.attr1))
                {
                    attrs[temp.attr1] -= temp.value1;
                }
                else
                {
                    attrs.Add(temp.attr1, temp.value1);
                }
                if (attrs.ContainsKey(temp.attr2))
                {
                    attrs[temp.attr2] -= temp.value2;
                }
                else
                {
                    attrs.Add(temp.attr2, temp.value2);
                }
                temp_dress1.Add(temp);
            }
        }
        foreach (int key in attrs.Keys)
        {
            if (attrs[key] != 0)
            {
                string s1 = "[00ff00]" + game_data._instance.get_value_string(key, attrs[key], 1) + "[-]";
                sxs.Add(s1);
            }
        }
        s_message _message2 = new s_message();
        _message2.m_type = "check_bf";
        cmessage_center._instance.add_message(_message2);
        mm_show_gui.m_new = true;
        return sxs;
    }

    public List<string> player_dress_chenjiu_check(int id)
    {
        check_dress_target_done();
        List<string> sxs = new List<string>();
        dbc m_dress_target = game_data._instance.m_dbc_dress_target;
        for (int i = 0; i < m_dress_target.get_y(); ++i)
        {
            s_t_dress_target temp = game_data._instance.get_t_dress_target(int.Parse(m_dress_target.get(0, i)));
            if (temp.id == id)
            {
                if (temp.attr1 > 0)
                {
                    string s = "[00ff00]" + game_data._instance.get_value_string(temp.attr1, temp.value1) + "[-]";
                    sxs.Add(s);
                }
                if (temp.attr2 > 0)
                {
                    string s = "[00ff00]" + game_data._instance.get_value_string(temp.attr2, temp.value2) + "[-]";
                    sxs.Add(s);
                }
                if (temp.attr3 > 0)
                {
                    string s = "[00ff00]" + game_data._instance.get_value_string(temp.attr3, temp.value3) + "[-]";
                    sxs.Add(s);
                }
                if (temp.attr4 > 0)
                {
                    string s = "[00ff00]" + game_data._instance.get_value_string(temp.attr4, temp.value4) + "[-]";
                    sxs.Add(s);
                }
                break;
            }
        }
        s_message _message2 = new s_message();
        _message2.m_type = "check_bf";
        cmessage_center._instance.add_message(_message2);
        mm_show_gui.m_new = true;
        return sxs;
    }

    public bool check_dress_target_done(s_t_dress_target _dress_target, List<int> dress_ids)
    {
        if (_dress_target.type == 1)
        {
            for (int i = 0; i < m_t_player.dress_achieves.Count; ++i)
            {
                if (_dress_target.id == m_t_player.dress_achieves[i])
                {
                    return true;
                }
            }
        }
        if (_dress_target.type == 2)
        {
            int count = 0;
            List<int> temp = dress_ids;
            for (int i = 0; i < temp.Count; i++)
            {
                for (int j = 0; j < _dress_target.defs.Count; j++)
                {
                    if (temp[i] == _dress_target.defs[j])
                        count++;
                }

            }
            if (count == _dress_target.defs.Count)
            {
                return true;
            }
        }
        return false;

    }

    public void add_role_dress(int id, bool notify)
    {
        s_t_role_dress t_role_dress = game_data._instance.get_t_role_dress(id);
        if (t_role_dress == null)
        {
            return;
        }
        if (has_card(t_role_dress.role))
        {
            ccard card = get_card_id(t_role_dress.role);
            if (!has_role_dress(card.get_guid(), t_role_dress.id))
            {
                card.get_role().dress_ids.Add(t_role_dress.id);
            }
        }
        else
        {
            m_t_player.dress_id_bags.Add(id);
        }
        if (notify)
        {
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 5, t_role_dress.id.ToString(), "[ffffc0]" + t_role_dress.name + " [ffd000]x1");
        }
    }

    public bool has_role_dress(int id)
    {
        for (int i = 0; i < m_t_player.dress_id_bags.Count; ++i)
        {
            if (m_t_player.dress_id_bags[i] == id)
            {
                return true;
            }
        }
        for (int i = 0; i < m_t_player.roles.Count; ++i)
        {
            ccard card = get_card_guid(m_t_player.roles[i]);
            dhc.role_t role = card.get_role();
            for (int j = 0; j < role.dress_ids.Count; ++j)
            {
                if (role.dress_ids[j] == id)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public bool has_role_dress(ulong guid, int id)
    {
        ccard _card = get_card_guid(guid);
        dhc.role_t m_role = _card.get_role();
        for (int i = 0; i < m_role.dress_ids.Count; ++i)
        {
            if (m_role.dress_ids[i] == id)
            {
                return true;
            }
        }
        return false;
    }

    public bool has_role_dress_on(ulong guid, int id)
    {
        ccard _card = get_card_guid(guid);
        dhc.role_t m_role = _card.get_role();
        if (m_role.dress_on_id == id)
        {
            return true;
        }
        return false;
    }

    public int get_un_treasure_num()
    {
        int num = 0;
        for (int i = 0; i < this.m_treasures.Count; i++)
        {
            dhc.treasure_t treasure = this.m_treasures[i];
            num++;
        }
        return num;
    }

    public int get_treasure_num()
    {
        return m_treasures.Count;
    }

    public int get_treasure_num(int id)
    {
        int num = 0;
        for (int i = 0; i < this.m_treasures.Count; i++)
        {
            dhc.treasure_t treasure = this.m_treasures[i];
            if (id == treasure.template_id)
            {
                num++;
            }
        }
        return num;
    }

    public dhc.treasure_t get_treasure_index(int index)
    {
        return m_treasures[index];
    }

    public dhc.treasure_t get_treasure_guid(ulong guid)
    {
        for (int i = 0; i < this.m_treasures.Count; i++)
        {
            dhc.treasure_t treasure = this.m_treasures[i];

            if (treasure.guid == guid)
            {
                return treasure;
            }
        }

        return null;
    }

    public bool has_treasure_id(int id)
    {
        for (int i = 0; i < this.m_treasures.Count; i++)
        {
            dhc.treasure_t treasure = this.m_treasures[i];

            if (treasure.template_id == id)
            {
                return true;
            }
        }

        return false;
    }

    public void add_treasure(dhc.treasure_t treasure)
    {
        add_treasure(treasure, true);
    }

    public void add_treasure(dhc.treasure_t treasure, bool notify)
    {
        m_treasures.Add(treasure);
        m_t_player.treasures.Add(treasure.guid);

        if (notify)
        {
            s_t_baowu _treasure = game_data._instance.get_t_baowu(treasure.template_id);
            string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 6, _treasure.id.ToString(), "[ffffc0]" + _treasure.name + " [ffd000]x1");
        }
    }

    public void add_treasure_login(dhc.treasure_t treasure)
    {
        m_treasures.Add(treasure);
    }

    public void remove_treasure(ulong guid)
    {
        for (int i = 0; i < m_treasures.Count; ++i)
        {
            if (m_treasures[i].guid == guid)
            {
                m_treasures.Remove(m_treasures[i]);
                break;
            }
        }
        m_t_player.treasures.Remove(guid);
    }


    public int get_max_treasure_num()
    {
        return 100 + m_t_player.treasure_kc_num * 10;
    }

    public int get_treasure_fragment_num()
    {
        int _num = 0;

        for (int i = 0; i < m_t_player.item_ids.Count; i++)
        {
            if (is_treasure_fragment(m_t_player.item_ids[i]))
            {
                _num++;
            }
        }

        return _num;
    }

    public bool is_treasure_fragment(uint id)
    {
        s_t_item t_item = game_data._instance.get_item((int)id);
        if (t_item == null)
        {
            return false;
        }
        if (t_item.type == 10001)
        {
            return true;
        }

        return false;
    }

    public string get_name(int type, int rvalue1, int rvalue2, int rvalue3)
    {
        string name = "";
        if (type == 1)
        {
            s_t_resource t_res = game_data._instance.get_t_resource(rvalue1);
            name = t_res.namecolor + t_res.name;
            return name;
        }
        if (type == 2)
        {
            s_t_item t_item = game_data._instance.get_item(rvalue1);
            name = ccard.get_color_name(t_item.name, t_item.font_color);
            return name;
        }
        if (type == 3)
        {
            s_t_class t_class = game_data._instance.get_t_class(rvalue1);
            name = ccard.get_color_name(t_class.name, t_class.color);
            return name;
        }
        if (type == 4)
        {
            name = equip.get_equip_real_name(rvalue1);
            return name;
        }
        if (type == 6)
        {
            name = treasure.get_treasure_real_name(rvalue1);
            return name;
        }
        if (type == 7)
        {
            s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan(rvalue1);
            name = game_data._instance.get_name_color(t_guanghuan.color) + t_guanghuan.name;
            return name;
        }
        if (type == 11)
        {
            s_t_pet t_pet = game_data._instance.get_t_pet(rvalue1);
            name = game_data._instance.get_name_color(t_pet.color) + t_pet.name;
            return name;
        }
        return name;
    }

    public int get_res_num(int type)
    {
        if (type == 1)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_gold);
        }
        if (type == 2)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_jewel);
        }
        if (type == 5)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_jjc_point);
        }
        if (type == 6)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_hj);
        }
        if (type == 7)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_yuanli);
        }
        if (type == 10)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_contribution);
        }
        if (type == 13)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_medal_point);
        }
        if (type == 14)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_treasure_powder);
        }
        if (type == 16)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_guild_hor);
        }
        if (type == 17)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_skill_point);
        }
        if (type == 18)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_boss_num);
        }
        if (type == 19)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_dress_tuzhi);
        }
        if (type == 20)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_luck_point);
        }
        if (type == 21)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_huiyi_point);
        }
        if (type == 22)
        {
            return sys._instance.m_self.get_att(e_player_attr.player_pvp_jz);
        }
        else
        {
            return sys._instance.m_self.get_att((e_player_attr)type);
        }
    }

    public string get_item_num(int type, int rvalue1, int rvalue2, int rvalue3)
    {
        string num = "";
        string name = "";
        string s = "";
        string value = "";
        if (type == 1)
        {
            if (rvalue1 == 5)
            {
                s = game_data._instance.get_t_language("kaifu_gui.cs_247_19");//战魂
                if (get_att(e_player_attr.player_jjc_point) != 0)
                {
                    value = sys._instance.value_to_wan((long)get_att(e_player_attr.player_jjc_point));
                    num = sys._instance.get_res_color(5) + s + "X" + value + "[-]";
                }
            }
            else if (rvalue1 == 7)
            {
                s = game_data._instance.get_t_language("kaifu_gui.cs_252_19");//原力
                if (get_att(e_player_attr.player_yuanli) != 0)
                {
                    value = sys._instance.value_to_wan((long)get_att(e_player_attr.player_yuanli));
                    num = sys._instance.get_res_color(7) + s + "X" + value + "[-]";
                }
            }
            else if (rvalue1 == 1)
            {
                s = game_data._instance.get_t_language("kaifu_gui.cs_242_19");//金币
                if (get_att(e_player_attr.player_gold) != 0)
                {
                    value = sys._instance.value_to_wan((long)get_att(e_player_attr.player_gold));
                    num = sys._instance.get_res_color(1) + s + "X" + value + "[-]";
                }
            }
            else if (rvalue1 == 2)
            {
                s = game_data._instance.get_t_language("czfp_gui.cs_360_108");//钻石
                if (get_att(e_player_attr.player_jewel) != 0)
                {
                    value = sys._instance.value_to_wan((long)get_att(e_player_attr.player_jewel));
                    num = sys._instance.get_res_color(2) + s + "X" + value + "[-]";
                }
            }
            else if (rvalue1 == 6)
            {
                s = game_data._instance.get_t_language("player.cs_1740_17");//合金
                if (get_att(e_player_attr.player_hj) != 0)
                {
                    value = sys._instance.value_to_wan((long)get_att(e_player_attr.player_hj));
                    num = sys._instance.get_res_color(6) + s + "X" + value + "[-]";
                }
            }
            else if (rvalue1 == 13)
            {
                s = game_data._instance.get_t_language("player.cs_1893_16");//魔王勋章
                if (get_att(e_player_attr.player_medal_point) != 0)
                {
                    value = sys._instance.value_to_wan((long)get_att(e_player_attr.player_medal_point));
                    num = sys._instance.get_res_color(13) + s + "X" + value + "[-]";
                }
            }
            else if (rvalue1 == 19)
            {
                s = game_data._instance.get_t_language("player.cs_1917_16");//时装设计图
                if (get_att(e_player_attr.player_dress_tuzhi) != 0)
                {
                    num = "[5cf732]" + s + "X" + get_att(e_player_attr.player_dress_tuzhi) + "[-]";
                }
            }
        }
        else if (type == 2)
        {
            s_t_item t_item = game_data._instance.get_item(rvalue1);
            name = t_item.name + "X" + get_item_num((uint)rvalue1).ToString();
            if (get_item_num((uint)rvalue1) != 0)
            {
                num = ccard.get_color_name(name, t_item.font_color);
            }
        }
        return num;
    }

    public string check_gongzhen(ccard m_card, List<int> old_contions, List<dhc.equip_t> m_old_equips, List<dhc.treasure_t> m_old_treasures, Dictionary<int, double> _value, ulong card_id = 0)
    {
        //if(!is_zheng(m_card.get_guid()))
        //{
        //    return "";
        //}
        string _zh = "";
        _zh += gongzhengs(m_card, old_contions);
        if (m_card.m_t_class.jbs[0] > 0)
        {
            _zh += get_jb_text(m_card.m_t_class.jbs[0], m_card, m_old_equips, m_old_treasures);
        }
        if (m_card.m_t_class.jbs[1] > 0)
        {
            _zh += get_jb_text(m_card.m_t_class.jbs[1], m_card, m_old_equips, m_old_treasures);
        }
        if (m_card.m_t_class.jbs[2] > 0)
        {
            _zh += get_jb_text(m_card.m_t_class.jbs[2], m_card, m_old_equips, m_old_treasures);
        }
        if (m_card.m_t_class.jbs[3] > 0)
        {
            _zh += get_jb_text(m_card.m_t_class.jbs[3], m_card, m_old_equips, m_old_treasures);
        }
        if (m_card.m_t_class.jbs[4] > 0)
        {
            _zh += get_jb_text(m_card.m_t_class.jbs[4], m_card, m_old_equips, m_old_treasures);
        }
        if (m_card.m_t_class.jbs[5] > 0)
        {
            _zh += get_jb_text(m_card.m_t_class.jbs[5], m_card, m_old_equips, m_old_treasures);
        }
        if (m_card.m_t_class.jbs[6] > 0)
        {
            _zh += get_jb_text(m_card.m_t_class.jbs[6], m_card, m_old_equips, m_old_treasures);
        }
        if (m_card.m_t_class.jbs[7] > 0)
        {
            _zh += get_jb_text(m_card.m_t_class.jbs[7], m_card, m_old_equips, m_old_treasures);
        }
        List<int> jbs = new List<int>();
        for (int j = 0; j < m_card.m_t_class.jbs.Count; ++j)
        {
            if (m_card.m_t_class.jbs[j] > 0)
            {
                s_t_ji_ban _jbex = game_data._instance.get_t_ji_ban(m_card.m_t_class.jbs[j]);
                if (_jbex != null && _jbex.type == 1)
                {
                    jbs.Add(m_card.m_t_class.jbs[j]);
                }
            }
        }

        for (int i = 0; i < jbs.Count; ++i)
        {
            _zh += get_jb_text_ex(jbs[i], card_id, 1);
        }
        Dictionary<int, double> sxs = new Dictionary<int, double>();
        sxs = get_gongzhen(m_card);
        foreach (int i in sxs.Keys)
        {
            int value = (int)(sxs[i] - _value[i]);
            int num = 0;
            num = value;
            if (value < 0)
            {
                num = -value;
            }
            if (i == 0 || i > game_data._instance.m_t_value.get_y())
            {
                continue;
            }
            string attr = game_data._instance.get_value_string(i, num);
            if (value != 0)
            {
                if (value < 0)
                {
                    attr = attr.Replace("+", "-");
                }
                if (value < 0)
                {
                    _zh += "\n" + "[ff0000]" + attr;
                }
                else
                {
                    _zh += "\n" + "[00ff00]" + attr;
                }
            }
        }
        if (_zh != "")
        {
            _zh.Remove(0);
        }
        return _zh;
    }

    public string check_houyuan(List<int> old_contions, List<double> _value, ulong card_id)
    {
        string _zh = "";
        List<int> jbs = new List<int>();
        List<int> conditions_now = new List<int>();
        conditions_now = houyuan_gui.gongzhen_cur();
        string s = "[ffffff]";
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[i] == 0)
            {
                continue;
            }
            ulong guid = sys._instance.m_self.m_t_player.zhenxing[i];
            ccard _card = sys._instance.m_self.get_card_guid(guid);
            for (int j = 0; j < _card.m_t_class.jbs.Count; ++j)
            {
                if (_card.m_t_class.jbs[j] > 0)
                {
                    s_t_ji_ban _jbex = game_data._instance.get_t_ji_ban(_card.m_t_class.jbs[j]);
                    if (_jbex != null && _jbex.type == 1)
                    {
                        jbs.Add(_card.m_t_class.jbs[j]);
                    }
                }
            }
        }
        for (int i = 0; i < jbs.Count; ++i)
        {
            _zh += get_jb_text_ex(jbs[i], card_id, 2);
        }
        for (int i = 5; i < 7; ++i)
        {
            int min_enhace = houyuan_gui.min_enhance(i);
            if (conditions_now[i - 5] > old_contions[i - 5])
            {
                s_t_gongzhen _t_gongzheng = null;
                for (int j = 0; j < game_data._instance.m_dbc_gongzhen.get_y(); ++j)
                {
                    int level = int.Parse(game_data._instance.m_dbc_gongzhen.get(1, j));
                    _t_gongzheng = game_data._instance.get_t_gongzhen(i + 1, level);
                    if (_t_gongzheng == null)
                    {
                        continue;
                    }
                    if (_t_gongzheng.task_type == i + 1 && _t_gongzheng.condition == conditions_now[i - 5])
                    {
                        break;
                    }
                }
                if (i == 5)
                {
                    _zh += "\n" + game_data._instance.get_t_language("player.cs_3265_19") + "[-]" + "[0aff16]" //[f9d420]后援鼓舞
                        + _t_gongzheng.gongzhen_level + "[-]" + s + game_data._instance.get_t_language("player.cs_3266_48") + "[-]";//级达成
                }
                if (i == 6)
                {
                    _zh += "\n" + game_data._instance.get_t_language("player.cs_3270_19") + "[-]" + "[0aff16]" //[f9d420]后援助威
                        + _t_gongzheng.gongzhen_level + "[-]" + s + game_data._instance.get_t_language("player.cs_3266_48") + "[-]";//级达成
                }
            }
        }
        List<double> sxs = new List<double>();
        sxs = get_houyuan_sxs();
        for (int i = 0; i < sxs.Count; ++i)
        {
            int value = (int)(sxs[i] - _value[i]);
            int num = 0;
            num = value;
            if (value < 0)
            {
                num = -value;
            }
            if (i == 0 || i > game_data._instance.m_t_value.get_y())
            {
                continue;
            }
            string attr = game_data._instance.get_value_string(i, num);
            if (value != 0)
            {
                if (value < 0)
                {
                    attr = attr.Replace("+", "-");
                }
                if (value < 0)
                {
                    _zh += "\n" + "[ff0000]" + attr;
                }
                else
                {
                    _zh += "\n" + "[00ff00]" + attr;
                }
            }
        }
        if (_zh != "")
        {
            _zh.Remove(0);
        }
        return _zh;
    }

    public string gongzhengs(ccard m_card, List<int> old_contions)
    {
        string text = "";
        List<int> conditions_now = new List<int>();
        conditions_now = gongzheng_mubiao_gui.gongzhen_cur(m_card);
        for (int i = 0; i < 5; ++i)
        {
            int min_enhace = gongzheng_mubiao_gui.min_enhance(i, m_card);
            if (conditions_now[i] > old_contions[i])
            {
                s_t_gongzhen _t_gongzheng = null;
                for (int j = 0; j < game_data._instance.m_dbc_gongzhen.get_y(); ++j)
                {
                    int level = int.Parse(game_data._instance.m_dbc_gongzhen.get(1, j));
                    _t_gongzheng = game_data._instance.get_t_gongzhen(i + 1, level);
                    if (_t_gongzheng == null)
                    {
                        continue;
                    }
                    if (_t_gongzheng.task_type == i + 1 && _t_gongzheng.condition == conditions_now[i])
                    {
                        break;
                    }
                }
                if (i == 0)
                {
                    text += "\n" + "[f9d420]" + game_data._instance.get_t_language("player.cs_3341_33") + "[-]" + "[0aff16]" //装备强化共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
                if (i == 1)
                {
                    text += "\n" + "[f9d420]" + game_data._instance.get_t_language("player.cs_3346_32") + "[-]" + "[0aff16]"//饰品强化共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
                if (i == 2)
                {
                    text += "\n" + "[f9d420]" + game_data._instance.get_t_language("player.cs_3351_33") + "[-]" + "[0aff16]"//装备精炼共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
                if (i == 3)
                {
                    text += "\n" + "[f9d420]" + game_data._instance.get_t_language("player.cs_3356_33") + "[-]" + "[0aff16]"//饰品精炼共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
                if (i == 4)
                {
                    text += "\n" + "[f9d420]" + game_data._instance.get_t_language("player.cs_3361_32") + "[-]" + "[0aff16]"//技能共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
            }
        }
        return text;
    }

    public string gongzhengs_ex(ccard m_card, List<int> old_contions)
    {
        if (!is_zheng(m_card.get_guid()))
        {
            return "";
        }
        string text = "";
        Dictionary<int, int> m_attrs = new Dictionary<int, int>();
        List<int> conditions_now = new List<int>();
        conditions_now = gongzheng_mubiao_gui.gongzhen_cur(m_card);
        for (int i = 0; i < 5; ++i)
        {
            int min_enhace = gongzheng_mubiao_gui.min_enhance(i, m_card);
            if (conditions_now[i] > old_contions[i])
            {
                s_t_gongzhen _t_gongzheng = null;
                for (int j = 0; j < game_data._instance.m_dbc_gongzhen.get_y(); ++j)
                {
                    int level = int.Parse(game_data._instance.m_dbc_gongzhen.get(1, j));
                    _t_gongzheng = game_data._instance.get_t_gongzhen(i + 1, level);
                    if (_t_gongzheng == null)
                    {
                        continue;
                    }
                    if (_t_gongzheng.task_type == i + 1 && _t_gongzheng.condition == conditions_now[i])
                    {
                        break;
                    }
                }
                if (i == 0)
                {
                    text = "[f9d420]" + game_data._instance.get_t_language("player.cs_3341_33") + "[-]" + "[0aff16]"//装备强化共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
                if (i == 1)
                {
                    text = "[f9d420]" + game_data._instance.get_t_language("player.cs_3346_32") + "[-]" + "[0aff16]"//饰品强化共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
                if (i == 2)
                {
                    text = "[f9d420]" + game_data._instance.get_t_language("player.cs_3351_33") + "[-]" + "[0aff16]"//装备精炼共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
                if (i == 3)
                {
                    text = "[f9d420]" + game_data._instance.get_t_language("player.cs_3356_33") + "[-]" + "[0aff16]"//饰品精炼共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
                if (i == 4)
                {
                    text = "[f9d420]" + game_data._instance.get_t_language("player.cs_3361_32") + "[-]" + "[0aff16]"//技能共振
                        + string.Format(game_data._instance.get_t_language("player.cs_3342_22"), _t_gongzheng.gongzhen_level);//{0}[-]级达成[-]
                }
            }
        }
        return text;
    }

    public string get_jb_text(int jb_id, ccard m_card, List<dhc.equip_t> m_old_equips, List<dhc.treasure_t> m_old_treasures)
    {
        string text = "";
        List<dhc.equip_t> m_equips = new List<dhc.equip_t>();
        List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
        List<ulong> m_card_guids = new List<ulong>();
        bool flag = false;
        for (int i = 0; i < m_card.m_equip.Count; ++i)
        {
            flag = false;
            if (m_card.m_equip[i] == null)
            {
                continue;
            }
            for (int j = 0; j < m_old_equips.Count; ++j)
            {
                if (m_old_equips[j] == null)
                {
                    continue;
                }
                if (m_old_equips[j].guid == m_card.m_equip[i].guid)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                m_equips.Add(m_card.m_equip[i]);
            }
        }
        for (int i = 0; i < m_card.m_treasure.Count; ++i)
        {
            flag = false;
            if (m_card.m_treasure[i] == null)
            {
                continue;
            }
            for (int j = 0; j < m_old_treasures.Count; ++j)
            {
                if (m_old_treasures[j] != null && m_old_treasures[j].guid == m_card.m_treasure[i].guid)
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                m_treasures.Add(m_card.m_treasure[i]);
            }
        }
        string s = "[ffffff]";
        s_t_ji_ban _jb = game_data._instance.get_t_ji_ban(jb_id);
        if (_jb.type == 2)
        {
            flag = false;
            for (int j = 0; j < _jb.tids.Count; ++j)
            {
                flag = false;
                for (int i = 0; i < m_equips.Count; i++)
                {
                    if (_jb.tids[j] == m_equips[i].template_id)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag)
            {
                text += "\n" + s + game_data._instance.get_t_language("player.cs_3499_21") + "[-]" + "[f9d420]" //组合
                    + _jb.name + "[-]" + s + game_data._instance.get_t_language("player.cs_3500_31") + "[-]";//达成
            }
        }
        else if (_jb.type == 3)
        {
            flag = false;
            for (int j = 0; j < _jb.tids.Count; ++j)
            {
                flag = false;
                for (int i = 0; i < m_treasures.Count; i++)
                {
                    if (_jb.tids[j] == m_treasures[i].template_id)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (flag)
            {
                text += "\n" + s + game_data._instance.get_t_language("player.cs_3499_21") + "[-]" + "[f9d420]" //组合
                    + _jb.name + "[-]" + s + game_data._instance.get_t_language("player.cs_3500_31") + "[-]";//达成
            }
        }
        return text;
    }

    public string get_jb_text_ex(int jb_id, ulong card_id, int flag)
    {
        string text = "";
        string s = "[ffffff]";
        s_t_ji_ban _jb = game_data._instance.get_t_ji_ban(jb_id);
        bool is_jb = false;
        ccard card = sys._instance.m_self.get_card_guid(card_id);
        if (card == null)
        {
            return text;
        }
        if (flag != 1)
        {
            if (_jb.tids.Contains(card.m_t_class.id))
            {
                for (int i = 0; i < _jb.tids.Count; ++i)
                {
                    if (!sys._instance.m_self.is_zheng(_jb.tids[i]) && !sys._instance.m_self.is_houyuan(_jb.tids[i]))
                    {
                        is_jb = true;
                    }
                }
                if (!is_jb)
                {
                    text += "\n" + s + game_data._instance.get_t_language("player.cs_3499_21") + "[-]" + "[f9d420]"//组合
                        + _jb.name + "[-]" + s + game_data._instance.get_t_language("player.cs_3500_31") + "[-]";//达成
                }
            }
        }
        else
        {
            for (int i = 0; i < _jb.tids.Count; ++i)
            {
                if (!sys._instance.m_self.is_zheng(_jb.tids[i]) && !sys._instance.m_self.is_houyuan(_jb.tids[i]))
                {
                    is_jb = true;
                }
            }
            if (!is_jb)
            {
                text += "\n" + card.get_color_name() + s + "" + game_data._instance.get_t_language("player.cs_3499_21") + "[-]" + "[f9d420]"
                    + _jb.name + "[-]" + s + game_data._instance.get_t_language("player.cs_3500_31") + "[-]";//组合//达成
            }
        }
        return text;
    }

    public Dictionary<int, double> get_gongzhen(ccard m_card)
    {
        Dictionary<int, double> _value = new Dictionary<int, double>();
        for (int i = 0; i < ccard.m_attr_num; ++i)
        {
            if (m_card != null)
            {
                if (!_value.ContainsKey(i))
                {
                    _value.Add(i, m_card.get_attr(i));
                }
                else
                {
                    _value[i] += m_card.get_attr(i);
                }

            }
            else
            {
                _value.Add(i, 0);
            }

        }
        return _value;
    }

    public List<double> get_houyuan_sxs()
    {
        int m_attr_num = 50;
        List<double> m_attrs = new List<double>();
        Dictionary<int, double> _value = new Dictionary<int, double>();
        for (int i = 0; i < m_attr_num; ++i)
        {
            m_attrs.Add(0);
        }
        s_t_gongzhen t_gongzheng = null;
        int min_enhanc_level = 99999;
        int min_jl_level = 99999;
        bool has_role = true;
        ccard card = null;
        for (int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count; ++i)
        {
            card = sys._instance.m_self.get_card_guid((ulong)sys._instance.m_self.m_t_player.houyuan[i]);
            if (card == null)
            {
                has_role = false;
                break;
            }
            if (card.get_level() < min_enhanc_level)
            {
                min_enhanc_level = card.get_level();
            }
            if (card.get_glevel() < min_jl_level)
            {
                min_jl_level = card.get_glevel();
            }
        }
        if (has_role)
        {
            t_gongzheng = game_data._instance.get_t_gongzhen(6, min_enhanc_level);
            if (t_gongzheng != null)
            {
                for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
                {
                    m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
                }
            }
            t_gongzheng = game_data._instance.get_t_gongzhen(7, min_jl_level);
            if (t_gongzheng != null)
            {
                for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
                {
                    m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
                }
            }
        }
        return m_attrs;
    }

    public int get_xg_shop_price(int id, int m_input_num = 1)
    {
        s_t_shop_xg m_t_shop = game_data._instance.get_shop_xg(id);
        int m_input_price = m_input_num * m_t_shop.price;
        if (m_t_shop.xg_type == 2)
        {
            m_input_price = 0;
            int xg_buy_num = 0;

            for (int i = 0; i < sys._instance.m_self.m_t_player.shop_xg_ids.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.shop_xg_ids[i] == id)
                {
                    xg_buy_num = sys._instance.m_self.m_t_player.shop_xg_nums[i];
                    break;
                }
            }

            for (int j = 0; j < m_input_num; ++j)
            {
                int xg_num = xg_buy_num + j + 1;
                m_input_price += temaihui_card.get_price(xg_num, m_t_shop.price_type);
            }
        }
        return m_input_price;
    }

    public int get_huiyi_xuyao(int id)
    {
        if (m_huiyi_xuyao.ContainsKey(id))
        {
            return m_huiyi_xuyao[id];
        }
        return 0;
    }

    public void check_huiyi_xuyao()
    {
        sys._instance.m_self.m_is_chou = false;
        m_huiyi_xuyao.Clear();
        Dictionary<int, int> xuyao = new Dictionary<int, int>();
        HashSet<int> jixu = new HashSet<int>();
        Dictionary<int, int> inum = new Dictionary<int, int>();

        for (int i = 0; i < m_t_player.item_ids.Count; i++)
        {
            inum[(int)m_t_player.item_ids[i]] = m_t_player.item_amount[i];
        }

        for (int i = 0; i < game_data._instance.m_dbc_huiyi_sub.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_huiyi_sub.get(0, i));
            s_t_huiyi_sub sub = game_data._instance.get_t_huiyi_sub(id);
            if (sys._instance.m_self.m_t_player.huiyi_jihuos.Contains(id))
            {
                continue;
            }
            if (!huiyilu_gui.is_jiesuo(sub.page))
            {
                continue;
            }
            int iid = 0;
            int num = 0;
            for (int j = 0; j < sub.huiyis.Count; j++)
            {
                if (!inum.ContainsKey(sub.huiyis[j]))
                {
                    num++;
                    iid = sub.huiyis[j];
                }
                if (xuyao.ContainsKey(sub.huiyis[j]))
                {
                    xuyao[sub.huiyis[j]]++;
                }
                else
                {
                    xuyao[sub.huiyis[j]] = 0;
                }
            }
            if (num == 1)
            {
                jixu.Add(iid);
            }
        }

        foreach (int iid in xuyao.Keys)
        {
            int xy = 1;
            if (jixu.Contains(iid))
            {
                xy = 2;
            }
            else
            {
                int num = 0;
                if (inum.ContainsKey(iid))
                {
                    num = inum[iid];
                }
                if (num >= xuyao[iid])
                {
                    xy = 0;
                }
            }
            m_huiyi_xuyao[iid] = xy;
        }
    }

    public string player_role_dress_check(ccard m_card)
    {
        dbc m_dbc_role_dress = game_data._instance.get_dbc_role_dress();
        s_t_role_dress _dress = new s_t_role_dress();
        bool flag = false;
        for (int i = 0; i < m_dbc_role_dress.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_role_dress.get(0, i));
            _dress = game_data._instance.get_t_role_dress(id);
            if (_dress.role == m_card.get_template_id() && _dress.hq_condition == 1 && m_card.get_role().glevel == _dress.hq_Level)
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            return "";
        }
        string text = "[00ff00]" + game_data._instance.get_t_language("bingyuan_gui.cs_776_19") + "[-]";//获得
        text += "[f9d420]" + _dress.name + "[-]" + " [ffd000]x1[-]\n";
        Dictionary<int, int> attrs = new Dictionary<int, int>();
        foreach (int id in game_data._instance.m_dbc_role_dresstarget.m_index.Keys)
        {
            s_t_role_dresstarget temp = game_data._instance.get_t_role_dresstarget(id);
            if (check_role_dress_target_done(_dress, temp))
            {
                text += game_data._instance.get_t_language("player.cs_3779_12") + "[00ff00]" + temp.name + game_data._instance.get_t_language("player.cs_3779_44");//时装组合//[-]达成{nn}
                for (int i = 0; i < temp.attrs.Count; ++i)
                {
                    if (attrs.ContainsKey(temp.attrs[i].attr))
                    {
                        attrs[temp.attrs[i].attr] += temp.attrs[i].value;
                    }
                    else
                    {
                        attrs.Add(temp.attrs[i].attr, temp.attrs[i].value);
                    }
                }
            }

        }
        foreach (int key in attrs.Keys)
        {
            if (attrs[key] != 0)
            {
                string s1 = "[00ff00]" + game_data._instance.get_value_string(key, attrs[key], 1) + "[-]";
                text += s1 + "\n";
            }
        }
        return text;
    }

    bool check_role_dress_target_done(s_t_role_dress role_dress, s_t_role_dresstarget t_role_drsstarget)
    {
        if (t_role_drsstarget == null)
        {
            return false;
        }
        int count = 0;
        for (int i = 0; i < m_t_player.roles.Count; ++i)
        {
            ccard m_card = get_card_guid(m_t_player.roles[i]);
            for (int j = 0; j < m_card.get_role().dress_ids.Count; ++j)
            {
                if (t_role_drsstarget.ids.Contains(m_card.get_role().dress_ids[j]))
                {
                    count++;
                }
            }
        }
        for (int i = 0; i < t_role_drsstarget.ids.Count; ++i)
        {
            s_t_role_dress t_role_dress = game_data._instance.get_t_role_dress(t_role_drsstarget.ids[i]);
            if (t_role_dress.hq_condition == 3)
            {
                count++;
            }
        }
        if (count == t_role_drsstarget.ids.Count)
        {
            return true;
        }
        return false;
    }

    public void zs_skill_target(int type, int num)
    {
        if (sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_skill_zs)
        {
            for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
            {
                ulong guid = sys._instance.m_self.m_t_player.zhenxing[i];
                ccard card = sys._instance.m_self.get_card_guid(guid);
                if (card != null)
                {
                    if (card.get_color() >= 4 && is_zs_level(card, type))
                    {
                        card.get_role().bskill_counts[type - 1] += num;
                    }
                }
            }
        }
    }

    public int is_finish_huigui_buzhu(int id)
    {
        s_t_comeback t_comeback = game_data._instance.get_t_comeback(id);

        if (t_comeback.def1 <= sys._instance.m_self.m_t_player.huodong_yxhg_buzhu_day)
        {
            if (sys._instance.m_self.m_t_player.huodong_yxhg_buzhu_id.Contains(id))
            {
                return 2;
            }
            return 0;
        }
        else
        {
            return 1;
        }
    }




    public int is_finish_huigui_haoli(int id)
    {
        s_t_comeback t_comeback = game_data._instance.get_t_comeback(id);

        if (t_comeback.def1 <= sys._instance.m_self.m_t_player.huodong_yxhg_rmb)
        {
            if (sys._instance.m_self.m_t_player.huodong_yxhg_haoli_id.Contains(id))
            {
                return 2;
            }
            return 0;
        }
        else
        {
            return 1;
        }

    }
    public bool is_zs_level(ccard m_card, int type)
    {
        int bsk_level = m_card.get_role().bskill_level + 1;
        s_t_role_skillunlock t_skillunlock = game_data._instance.get_t_role_skillunlock(m_card.get_template_id(), bsk_level);
        if (t_skillunlock == null)
        {
            return false;
        }
        for (int i = 0; i < t_skillunlock.role_skillunlock_tasks.Count; ++i)
        {
            if (t_skillunlock.role_skillunlock_tasks[i].task_type == type)
            {
                return true;
            }
        }
        return false;
    }
}
