using System.Collections.Generic;
using UnityEngine;

public class guild_boss_gui_ex : MonoBehaviour, IMessage
{
    public protocol.game.smsg_guild_mission_look m_msg;
    public UILabel m_progress;
    public UILabel m_lost_time;
    public List<GameObject> m_icons;
    public GameObject m_guan_scro;
    public GameObject m_attacknum;
    public UILabel m_huifutime;
    public GameObject m_hp_rate;
    public GameObject m_guild_rank_gui;
    public GameObject m_guild_rank_scrollow;
    public GameObject m_guild_item;
    public List<GameObject> m_units = new List<GameObject>();
    public GameObject m_guild_guan;
    public GameObject m_guild_guan_scro;
   // public UIScrollBar m_bar;
    public GameObject m_guild_sl;
    public GameObject m_buttle_tip;
    public GameObject m_info;
    int guan_reward_index;
    int sl_reward_index;
    int curren_ceng;
    private int num;
    int id = 1;
    public s_t_guild_mission m_mission;
    public GameObject m_reward_effect;
    public GameObject m_sl_effect;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
    void OnEnable()
    {
        protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_BOSS_LOOK, _msg);
    }
    string get_name(int m_id)
    {
        switch (m_id)
        {
            case 0:
                return game_data._instance.get_t_language ("guildboss_task_gui.cs_286_10");//坚苍守卫
            case 1:
                return game_data._instance.get_t_language ("guildboss_task_gui.cs_289_10");//韧战守卫
            case 2:
                return game_data._instance.get_t_language ("guildboss_task_gui.cs_292_10");//迅霆守卫
            case 3:
                return game_data._instance.get_t_language ("guildboss_task_gui.cs_295_10");//布洛守卫
        }
        return "";
    }
    string get_name1(int m_id)
    {
        switch (m_id)
        {
            case 0:
                return game_data._instance.get_t_language ("guild_boss_gui_ex.cs_71_23");//物免
            case 1:
                return game_data._instance.get_t_language ("guild_boss_gui_ex.cs_74_23");//魔免
            case 2:
                return game_data._instance.get_t_language ("guild_boss_gui_ex.cs_77_23");//闪避
            case 3:
                return game_data._instance.get_t_language ("guild_boss_gui_ex.cs_80_23");//格挡
        }
        return "";
    }
    public void reset(int ceng,int type = 0)
    {
        m_mission = game_data._instance.get_t_guild_mission(ceng);
        m_progress.text =  m_mission.name;
        long cur_hp = 0;
        long all_hp = 0;
        for (int i = 0; i < m_msg.mission.guild_cur_hps.Count; i++)
        {
            cur_hp += m_msg.mission.guild_cur_hps[i];
            all_hp += m_msg.mission.guild_max_hps[i];
        }
        if (ceng == m_msg.mission.guild_ceng)
        {
          //  m_hp_rate.GetComponent<UIProgressBar>().value = (float)cur_hp / all_hp;
           // m_hp_rate.transform.Find("value").GetComponent<UILabel>().text = (int)((float)cur_hp / all_hp * 100) + "%";
            m_attacknum.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_100_69"),  m_msg.num) ;//挑战次数：{0}

        }
        else
        {
           // m_hp_rate.GetComponent<UIProgressBar>().value = 0;
          //  m_hp_rate.transform.Find("value").GetComponent<UILabel>().text = 0 + "%";
            m_attacknum.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_100_69") , m_msg.num);//挑战次数：{0}
 
        }
        create_huoban();
        for (int i = 0; i < m_icons.Count; i++)
        {
            ccard _card = new ccard();
			_card.set_monster(m_mission.ids[i]);
            m_icons[i].transform.Find("Progress Bar").GetComponent<UISlider>().value = get_hp(i,ceng);
            m_icons[i].transform.parent.Find("sx").GetComponent<UILabel>().text = get_name1(i);
            m_icons[i].transform.Find("Label").GetComponent<UILabel>().text = get_name(i);
            if (get_hp(i,ceng) == 0)
            {
                m_units[i].GetComponent<unit>().set_bh(5);
                m_icons[i].transform.parent.Find("name").gameObject.SetActive(true);
                m_icons[i].transform.parent.Find("name").gameObject.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_124_118"), m_msg.mission.mission_names[get_index(i + 1,ceng)]);//击杀者：{0}
                
            }
            else
            {
                m_units[i].GetComponent<unit>().set_bh(0);
                m_icons[i].transform.parent.Find("name").gameObject.SetActive(false);
 
            }
        }
        m_lost_time.transform.parent = m_guan_scro.transform.parent;
        sys._instance.remove_child(m_guan_scro);
        if (m_guan_scro.GetComponent<SpringPanel>() != null)
        {
            m_guan_scro.GetComponent<SpringPanel>().enabled = false;
        }
      
        for (int i = 0; i < game_data._instance.m_dbc_guild_mission.get_y(); i++)
        {
            int a = int.Parse(game_data._instance.m_dbc_guild_mission.get(0, i));
            if (a == m_msg.mission.guild_ceng + 2)
                break;
            s_t_guild_mission mission = game_data._instance.get_t_guild_mission(int.Parse(game_data._instance.m_dbc_guild_mission.get(0, i)));
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_mission_sub");
            obj.transform.parent = m_guan_scro.transform;
            obj.transform.localPosition = new Vector3(0,-158 + 100 * i,0);
            obj.transform.localScale = Vector3.one;
            obj.name = (i + 1) + "";
            obj.transform.Find("name").GetComponent<UILabel>().text = mission.name;
            obj.transform.Find("guan").GetComponent<UILabel>().text = mission.index + "";
            if (mission.index == ceng)
            {
                obj.GetComponent<UISprite>().spriteName = "nnssl_xkp005";
            }
            else
            {
                obj.GetComponent<UISprite>().spriteName = "nnssl_xkp004";
 
            }
            if (mission.index < m_msg.mission.guild_ceng)
            {
                obj.transform.Find("over").gameObject.SetActive(true);
            }
            else
            {
                obj.transform.Find("over").gameObject.SetActive(false);
            }
            if (i + 1 <= m_msg.mission.guild_ceng - 1)
            {
                obj.transform.Find("effect").gameObject.SetActive(get_ceng_state(i + 1));
            }
            else
            {
                obj.transform.Find("effect").gameObject.SetActive(false);
 
            }
            if (m_msg.mission.guild_ceng == 1)
            {
                if (i == 0)
                {
                    m_lost_time.transform.parent = obj.transform;
                    m_lost_time.transform.localPosition = new Vector3(-18, 45.8f, 0);
 
                }

            }
            else
            {
                if (i + 1 == m_msg.mission.guild_ceng - 1)
                {
                    m_lost_time.transform.parent = obj.transform;
                    m_lost_time.transform.localPosition = new Vector3(-18, 45.8f, 0);
                }
            }
            
            UIEventListener.Get(obj).onClick = select_mission;
        }
        if (type == 0)
        {
            m_guan_scro.GetComponent<UIScrollView>().SetDragAmount(0,0,false) ;
        }
        if (is_reward_effect())
        {
            m_reward_effect.SetActive(true);

        }
        else
        {
            m_reward_effect.SetActive(false);
        }
        if (is_sl_effect())
        {
            m_sl_effect.SetActive(true);

        }
        else
        {
            m_sl_effect.SetActive(false);
 
        }
		s_message msg = new s_message();
		msg.m_type = "guild_fb_change_scene";
		msg.m_ints.Add(ceng - 1);
		cmessage_center._instance.add_message(msg);
        CancelInvoke();
        InvokeRepeating("time", 0,1);
        InvokeRepeating("time2",0,1);
    }
    int get_index(int index,int ceng)
    {
        for (int i = 0; i < m_msg.mission.mission_rewards.Count; i++)
        {
            if (m_msg.mission.mission_rewards[i] == index + 10 * ceng)
            {
                return i;
            }
        }
        return 0;
 
    }
    void remove_all()
    {
        for (int i = 0; i < m_units.Count; i++)
        {
            GameObject.Destroy(m_units[i]);
        }

        m_units.Clear();
    }
    void create_huoban()
    {
        remove_all();
        for (int i = 0; i < m_mission.ids.Count; i++)
        {
            ccard _card = new ccard();
            _card.set_monster(m_mission.ids[i]);
            GameObject _unit = sys._instance.create_class(_card, 0);
            _unit.transform.localPosition = get_tran(i);
            m_units.Add(_unit);
			guild_fb_scene._instance.m_s[i] = _unit;
        }
    }
    Vector3 get_tran(int id)
    {
        if (id == 0)
        {
            return new Vector3(1.6f, 0, -3.8f);
        }
        else if (id == 1)
        {
            return new Vector3(-1.6f, 0, -3.8f);
        }
        else if (id == 2)
        {
            return new Vector3(3.4f, 0, -1);
        }
        else if (id == 3)
        {
            return new Vector3(-3.4f, 0, -1);
        }
        return Vector3.zero;
    }
    void time()
    {
        
        ulong time = (ulong)timer.get_time_cuo(22) - timer.now();
        if (time > 0)
        {
            if (m_msg.mission.guild_ceng != 1)
            {
                m_lost_time.text = string.Format(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_294_49"), timer.get_time_show((long)time) ,(m_msg.mission.guild_ceng - 1) );//[6cff00]{0}[-][0affff]后{nn}重置至第{1}关

            }
            else
            {
                m_lost_time.text = string.Format(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_294_49"), timer.get_time_show((long)time) , m_msg.mission.guild_ceng );//[6cff00]{0}[-][0affff]后{nn}重置至第{1}关
            }
          //每日10:00-22:00可攻伐，5小时55分55秒后重置第1宫
         
        }
        else
        {
            m_lost_time.text = game_data._instance.get_t_language ("guild_boss_gui_ex.cs_306_31");//[0affff]每日[-][6cff00]10:00-22:00[-][0affff]可挑战冷却中
            CancelInvoke();
        }
    }
    void time2()
    {
        ulong now = timer.now ();
        long dtime = (int)((long)now - (long)m_msg.last_time);
    
        long _time = 120 * 60 * 1000 - (int)(timer.now() - m_msg.last_time);
        if (_time == 0)
        {
            m_msg.last_time = timer.now();
            m_msg.num++;
            reset(curren_ceng);
            if(m_buttle_tip.activeSelf)
            {
                m_buttle_tip.GetComponent<guild_buttle_gui>().reset();
            }
            
            
        }
        m_huifutime.gameObject.SetActive(true);

        m_huifutime.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("guild_boss_gui_ex.cs_330_65"), timer.get_time_show((long)_time)); //[0affff]在10:00-22:00内,每2小时恢复1次挑战次数,[-][6cff00]{0}[-][0affff]后恢复
        if (timer.dtnow().Hour >= 22 || timer.dtnow().Hour < 10)
        {
            m_huifutime.GetComponent<UILabel>().text = game_data._instance.get_t_language ("guild_boss_gui_ex.cs_333_55");//[0affff]未到恢复时间
        }
    }
    
    void select_mission(GameObject obj)
    {
        int id = int.Parse(obj.name);
        s_t_guild_mission _mission = game_data._instance.get_t_guild_mission(id);
        if (id <= m_msg.mission.guild_ceng && id >= m_msg.mission.guild_last_ceng)
        {
            if (_mission.level <= juntuan_gui._instance.m_guild_t.level)
            {
                curren_ceng = id;
                reset(curren_ceng, 1);
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_350_72"),_mission.level));//[ffc884]需达到军团{0}级
 
            }
           
            
        }
        else
        {
            if (id < m_msg.mission.guild_last_ceng)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_350_58"));//[ffc884]已通关
            }
            else
            {
               
                root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_354_72"), game_data._instance.get_t_guild_mission(id - 1).name));//[ffc884]需通关{0}
 
            }
            
 
        }
        
        
 
    }
    float get_hp(int id,int ceng)
    {
        if (m_msg.mission.guild_ceng == ceng)
        {
            long cur = 0;
            long max = 0;
            for (int i = 0; i < 5; i++)
            {
                cur += m_msg.mission.guild_cur_hps[id * 5 + i];
                max += m_msg.mission.guild_max_hps[id * 5 + i];
            }
            return (float)cur / max;

        }
        else
        {
            return 0;
        }
        
    }

    int get_rank()
    {
        for (int i = 0; i < m_msg.mission.player_guids.Count; i++)
        {
            if (m_msg.mission.player_guids[i] == sys._instance.m_self.m_t_player.guid)
            {
                return i;
            }
        }
        return -1;
 
    }
    void IMessage.message(s_message message)
    {
        
        if (message.m_type == "guild_boss_fight_ex_end")
        {
            protocol.game.cmsg_ttt_fight_end _msg = new protocol.game.cmsg_ttt_fight_end();
            net_http._instance.send_msg<protocol.game.cmsg_ttt_fight_end>(opclient_t.CMSG_TTT_FIGHT_END, _msg);
        }
        else if (message.m_type == "buy_guild_attack_num")
        {
            protocol.game.cmsg_shop_buy _msg = new protocol.game.cmsg_shop_buy();
            _msg.num = (int)message.m_ints[0];
            num = _msg.num;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_GUILD_MISSION_BUY, _msg);
        }
        else if (message.m_type == "hide_guild_guttle_gui")
        {
            m_buttle_tip.SetActive(false);
        }
        else if (message.m_type == "refresh_guild_boss")
        {
            reset(curren_ceng);
        }
		else if (message.m_type == "guild_fb_click")
		{
			string s = (string)message.m_string[0];
			if (s == "sj")
			{
				m_guild_sl.GetComponent<guildboss_task_gui>().m_mission = game_data._instance.get_t_guild_mission(curren_ceng);
				m_guild_sl.GetComponent<guildboss_task_gui>().m_msg = m_msg;
                m_guild_sl.GetComponent<guildboss_task_gui>().id = 0;
                m_guild_sl.SetActive(true);
			}
			else
			{
				if (timer.dtnow().Hour >= 22 || timer.dtnow().Hour < 10)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_boss_gui_ex.cs_434_60"));//主人，请在每天10:00-22:00内挑战军团副本
					return;
				}
				id = int.Parse(s.Substring(1)) - 1;
				if (get_hp(id, curren_ceng) == 0)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("guild_boss_gui_ex.cs_440_60"));//守卫已被击杀
					return;
					
				}
				m_buttle_tip.GetComponent<guild_buttle_gui>().m_mission = m_mission;
				m_buttle_tip.GetComponent<guild_buttle_gui>().m_id = id;
				m_buttle_tip.GetComponent<guild_buttle_gui>().m_msg = m_msg;
				m_buttle_tip.GetComponent<guild_buttle_gui>().reset();
				m_buttle_tip.SetActive(true);
			}
		}
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_GUILD_BOSS_LOOK)
        {
            m_msg = net_http._instance.parse_packet<protocol.game.smsg_guild_mission_look>(message.m_byte);
            s_t_guild_mission m_guild_mission = game_data._instance.get_t_guild_mission(m_msg.mission.guild_ceng);
            if (m_guild_mission.level <= juntuan_gui._instance.m_guild_t.level)
            {
                curren_ceng = m_msg.mission.guild_ceng;
            }
            else
            {
                curren_ceng = m_msg.mission.guild_ceng - 1;
            }
            m_buttle_tip.GetComponent<guild_buttle_gui>().m_mission = game_data._instance.get_t_guild_mission(curren_ceng);
            m_buttle_tip.GetComponent<guild_buttle_gui>().m_id = id;
            m_buttle_tip.GetComponent<guild_buttle_gui>().m_msg = m_msg;
            ulong time = timer.now();
            ulong time1 = time - m_msg.last_time; 
            m_buttle_tip.GetComponent<guild_buttle_gui>().reset();
            reset(curren_ceng);
        }
        else if (message.m_opcode == opclient_t.CMSG_GUILD_BOSS_FIGHT_END)
        {
            protocol.game.smsg_guild_mission_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_mission_fight_end>(message.m_byte); 
            battle_logic_ex._instance.set_guild_boss_fight_end(_msg);
            if (m_msg.num == 3)
            {
                m_msg.last_time = timer.now();
            }
            
            m_msg.num--;
            m_buttle_tip.GetComponent<guild_buttle_gui>().m_msg = m_msg;
            m_buttle_tip.GetComponent<guild_buttle_gui>().reset();
            sys._instance.m_game_state = "buttle";
			sys._instance.load_scene_ex("ts_chapter01");
            this.gameObject.SetActive(false);
            remove_all();
        }
        else if (message.m_opcode == opclient_t.CMSG_GUILD_MISSION_BUY)
        {

            int jewel = 0;
            for (int i = m_msg.buy_num + 1; i <= m_msg.buy_num + num; i++)
            {
                s_t_price t_price = game_data._instance.get_t_price(i);
                jewel += t_price.guild_attack_buy;

            }
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, jewel, game_data._instance.get_t_language ("guild_boss_gui_ex.cs_493_75"));//军团boss挑战次数购买
            m_msg.buy_num += num;
            m_msg.num += num;
            string s = game_data._instance.get_t_language ("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, "", "[ffffc0]" + game_data._instance.get_t_language ("guild_boss_gui_ex.cs_516_92") + "[ffd000] + " + num.ToString()); //挑战次数
           // root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_boss_gui_ex.cs_498_69") + num + game_data._instance.get_t_language ("guild_boss_gui_ex.cs_498_85"));//恭喜你获得//次挑战次数。
            reset(curren_ceng);
            m_buttle_tip.GetComponent<guild_buttle_gui>().m_mission = m_mission;
            m_buttle_tip.GetComponent<guild_buttle_gui>().m_msg = m_msg;
            m_buttle_tip.GetComponent<guild_buttle_gui>().reset();

        }
        else if (message.m_opcode == opclient_t.CMSG_GUILD_MISSION_CENG_REWARD)
        {
            s_t_guild_mission mission = game_data._instance.get_t_guild_mission(guan_reward_index);
            for (int i = 0; i < mission.firstRewards.Count; i++)
            {
                sys._instance.m_self.add_reward(mission.firstRewards[i].type,mission.firstRewards[i].value1,mission.firstRewards[i].value2,mission.firstRewards[i].value3,game_data._instance.get_t_language ("guild_boss_gui_ex.cs_510_170"));//军团每层奖励
 
            }
            m_msg.mission_rewards.Add(mission.index * 10);
            reset_guan_reward();
            reset(curren_ceng);
 
        }
    }
    public void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            sys._instance.m_game_state = "hall";
            sys._instance.load_scene(sys._instance.m_hall_name);
            s_message _mes = new s_message();
            _mes.m_type = "show_juntuan_gui";
            cmessage_center._instance.add_message(_mes);
            remove_all();
            this.gameObject.SetActive(false);
        }
        else if (obj.name == "buy")
        {
            s_t_price t_price = game_data._instance.get_t_price(m_msg.buy_num + 1);
            s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
            if (m_msg.buy_num >= t_vip.guild_attack_num)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_537_58"));//[ffc882]今日购买次数不足，提升VIP等级可增加每日购买次数
                return;
            }
              s_message _mes = new s_message();
            _mes.m_type = "buy_guild_attack_num";
            root_gui._instance.show_cishu_dialog_box(m_msg.num, t_vip.guild_attack_num,m_msg.buy_num,1, _mes);
            if (sys._instance.m_self.m_t_player.jewel < t_price.guild_attack_buy)
            {
                root_gui._instance.show_recharge_dialog_box(
                    delegate()
                    {
                        this.gameObject.SetActive(false);
                    }
                    );
                return;
            }
          

        }
        else if (obj.name == "progress_reward")
        {
            m_guild_rank_gui.SetActive(true);
            reset_rank_gui();
            
        }
        else if (obj.name == "member_attack")
        {
           
        }
        else if (obj.name == "guan_reward")
        {
			m_guild_guan.SetActive(true);
			reset_guan_reward();
        }
        else if (obj.name == "info")
        {
            m_info.SetActive(true);
 
        } 
    }
    int guan_compare(s_t_guild_mission x, s_t_guild_mission y)
    {
        if (is_linqu(x.index) != is_linqu(y.index))
        {
            return -(is_linqu(x.index) - is_linqu(y.index));
        }
        else
        {
            return x.index - y.index;
        }
       
    }
    void reset_rank_gui()
    {
        sys._instance.remove_child(m_guild_rank_scrollow);
        m_guild_rank_scrollow.transform.localPosition = new Vector3(0, 0, 0);
        m_guild_rank_scrollow.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (m_guild_rank_scrollow.GetComponent<SpringPanel>() != null)
        {
            m_guild_rank_scrollow.GetComponent<SpringPanel>().enabled = false;
        }
        for (int i = 0; i < m_msg.mission.player_damages.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guildboss_ph_item");
            obj.transform.parent = m_guild_rank_scrollow.transform;
            obj.transform.localPosition = new Vector3(0,132 - i * 82,0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("rank").GetComponent<UILabel>().text = (i + 1) + "";
            obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_msg.mission.player_achieves[i]) + m_msg.mission.player_names[i].ToString();
            obj.transform.Find("num").GetComponent<UILabel>().text = m_msg.mission.player_counts[i] + "";
            obj.transform.Find("hit").GetComponent<UILabel>().text = sys._instance.value_to_wan(m_msg.mission.player_damages[i]) + "";

            GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_msg.mission.player_templates[i], m_msg.mission.player_achieves[i], m_msg.mission.player_vips[i], m_msg.mission.nalflag[i]);
            _obj1.transform.parent =  obj.transform.Find("icon").transform;
            _obj1.transform.localScale = new Vector3(1, 1, 1);
            _obj1.transform.localPosition = new Vector3(0, 0, 0);
			GameObject chenghao = obj.transform.Find("chenghao").gameObject;
			sys._instance.get_chenghao((int)m_msg.mission.player_chenghaos[i],chenghao);

        }
        int rank = get_rank();
        if (rank != -1)
        {
            m_guild_item.transform.Find("rank").GetComponent<UILabel>().text = rank + 1 + "";
            m_guild_item.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count)
                + sys._instance.m_self.m_t_player.name;
            m_guild_item.transform.Find("num").GetComponent<UILabel>().text = m_msg.mission.player_counts[rank] + "";
            m_guild_item.transform.Find("hit").GetComponent<UILabel>().text = sys._instance.value_to_wan(m_msg.mission.player_damages[rank]) + "";
           
           

        }
        else
        {
            m_guild_item.transform.Find("rank").GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81");//未上榜
            m_guild_item.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count)
                + sys._instance.m_self.m_t_player.name;
            m_guild_item.transform.Find("num").GetComponent<UILabel>().text =  "0";
            m_guild_item.transform.Find("hit").GetComponent<UILabel>().text = "0";
 
        }
        GameObject _obj2 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count,
                                                                    sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);

        _obj2.transform.parent = m_guild_item.transform.Find("icon").transform;
        _obj2.transform.localScale = new Vector3(1, 1, 1);
        _obj2.transform.localPosition = new Vector3(0, 0, 0);
       
		GameObject _chenghao = m_guild_item.transform.Find("chenghao").gameObject;
		sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on,_chenghao);
    }
    void reset_guan_reward()
    {
        
        sys._instance.remove_child(m_guild_guan_scro);
        if (m_guild_guan_scro.GetComponent<SpringPanel>() != null)
        {
            m_guild_guan_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_guild_guan_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_guild_guan_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        List<s_t_guild_mission> _guildmissions = new List<s_t_guild_mission>();
        for (int i = 0; i < game_data._instance.m_dbc_guild_mission.get_y(); i++)
        {
            s_t_guild_mission mission = game_data._instance.get_t_guild_mission(int.Parse(game_data._instance.m_dbc_guild_mission.get(0, i)));
            _guildmissions.Add(mission);
        }
        _guildmissions.Sort(guan_compare);
        for(int i = 0;i < _guildmissions.Count;i ++)
        {
            s_t_guild_mission mission = _guildmissions[i];
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_guan_sub");
            obj.transform.parent = m_guild_guan_scro.transform;
            obj.transform.localPosition = new Vector3(0,184 - 124 * i,0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("boss.cs_234_75"), mission.index );//第{0}关
            for (int j = 0; j < mission.firstRewards.Count; j ++)
            {

                Transform par = obj.transform.Find("icon" + (j + 1));
                sys._instance.remove_child(par.gameObject);
                GameObject icon = icon_manager._instance.create_reward_icon_ex(mission.firstRewards[j].type, mission.firstRewards[j].value1, mission.firstRewards[j].value2, mission.firstRewards[j].value3);
                icon.transform.parent = par;
                icon.transform.localPosition = Vector3.zero;
                icon.transform.localScale = Vector3.one;
          
            }
            if (is_linqu(mission.index) == 2)
            {
                obj.transform.Find("yq").gameObject.SetActive(false);
                obj.transform.Find("wei").gameObject.SetActive(true);
                obj.transform.Find("yi").gameObject.SetActive(false);

 
            }
            else if (is_linqu(mission.index) == 1)
            {
                obj.transform.Find("yq").gameObject.SetActive(false);
                obj.transform.Find("wei").gameObject.SetActive(false);
                obj.transform.Find("yi").gameObject.SetActive(true);

            }
            else if (is_linqu(mission.index) == 3)
            {
                obj.transform.Find("yq").gameObject.SetActive(true);
                obj.transform.Find("wei").gameObject.SetActive(false);
                obj.transform.Find("yi").gameObject.SetActive(false);
            }
            obj.name = mission.index + "";
            UIEventListener.Get(obj.transform.Find("yq").gameObject).onClick = reward_linqu;
        }

 
    }
    void reward_linqu(GameObject obj)
    {
        if (obj.name == "yq")
        {
            protocol.game.cmsg_guild_mission_ceng_reward _msg = new protocol.game.cmsg_guild_mission_ceng_reward();
            _msg.ceng = int.Parse(obj.transform.parent.name);
            guan_reward_index = _msg.ceng;
            net_http._instance.send_msg<protocol.game.cmsg_guild_mission_ceng_reward>(opclient_t.CMSG_GUILD_MISSION_CENG_REWARD, _msg);
        }
        else if(obj.transform.name == "sl_reward")
        {
            m_guild_sl.SetActive(true);
            m_guild_sl.GetComponent<guildboss_task_gui>().m_msg = m_msg;
            m_guild_sl.GetComponent<guildboss_task_gui>().m_mission = m_mission;

        }
       
 
    }
    int is_linqu(int id)
    {
        //if (!m_msg.mission.mission_rewards.Contains(id))
        //{
        //    return 2;//未达成

        //}
        //else
        {
            if (m_msg.mission_rewards.Contains(id * 10))
            {
                return 1;//以领取
            }
            else if (m_msg.mission.mission_rewards.Contains(id * 10))
            {
                return 3;//未领取
            }
            else
            {
                return 2;////未达成 
            }
        }
 
    }
    bool is_reward_effect()
    {
        
        for (int i = 0; i < game_data._instance.m_dbc_guild_mission.get_y(); i++)
        {
            s_t_guild_mission _mission = game_data._instance.get_t_guild_mission(int.Parse(game_data._instance.m_dbc_guild_mission.get(0, i)));
            if (is_linqu(_mission.index) == 3)
            {
                return true;
            }

 
        }
        return false;
    }
    bool is_sl_effect()
    {
        for (int i = 1; i <= 4; i++)
        {
            if (get_state(i) == 3)
            {
                return true;
            }
        }
        return false;
 
    }
    int get_state(int id)
    {
        if (m_msg.mission_rewards.Contains(m_mission.index * 10 + id))
        {
            return 1;//已领取
        }
        else
        {
            if (m_msg.mission.mission_rewards.Contains(m_mission.index * 10 + id))
            {
                return 3;//已达成
            }
            else
            {
                return 2;//未达成
            }

        }

    }
   
    bool get_ceng_state(int ceng)
    {
        for (int i = 1; i <= 4; i++)
        {
            if (m_msg.mission.mission_rewards.Contains(ceng * 10 + i) && !m_msg.mission_rewards.Contains(ceng * 10 + i))
            {
                return true;
            }
            
        }
        return false;
    }
    void Update()
    {
        //if (flag)
        //{
        //    flag = false;
        //    m_bar.value = 0;
        //    m_guan_scro.GetComponent<UIScrollView>().OnScrollBar();
        //}
    }
}

