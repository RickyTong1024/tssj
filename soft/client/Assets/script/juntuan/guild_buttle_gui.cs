using UnityEngine;

public class guild_buttle_gui : MonoBehaviour,IMessage{

    public GameObject m_icon;
    public UILabel m_name_label;
    public GameObject m_hp_slider;
    public UILabel m_jisha_label;
    public UILabel m_mondesc_label;
    public UILabel m_attack_label;
    public UILabel m_attack_num;
    public UILabel m_desc_label;
    public GameObject m_icon_guild;
    public s_t_guild_mission m_mission;
    public int m_id;
    public protocol.game.smsg_guild_mission_look m_msg;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	}
    void IMessage.message(s_message message)
    {
    }
    void IMessage.net_message(s_net_message message)
    {

    }
    void OnEnable()
    {
        reset();
    }

    void click(GameObject obj)
    {
        if (obj.name == "attack")
        {
            //if (m_msg.total - m_msg.num > 0)
            //{
            //    long _time = (long)(m_msg.next_time) - (long)(timer.now());
            //    if (_time < 0)
            //    {
            //        fight(1);

            //    }

            //}
            //else
            //{
            //    s_t_vip _vip = game_data._instance.get_t_vip(sys._instance.m_self.get_vip());
            //    s_t_price _price = game_data._instance.get_t_price(m_msg.num - 2);
                
            //    s_message _msg = new s_message();
            //    _msg.m_type = "buy_attack_num";
            //    root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),game_data._instance.get_t_language ("guild_buttle_gui.cs_55_65") + _price.guild_attack_buy + game_data._instance.get_t_language ("guild_buttle_gui.cs_55_102") + game_data._instance.get_t_language ("guild_buttle_gui.cs_55_121") + (m_msg.total - m_msg.num) + game_data._instance.get_t_language ("guild_buttle_gui.cs_55_160"),_msg);//提示//你确定要花费//钻石购买1guild_buttle_gui.cs_55_160挑战guild_buttle_gui.cs_55_160数吗？{nn}//今日还可购买//次
            //}
               
        }
        else if (obj.name == "start")
        {
            
            if (m_msg.num > 0)
            {
                fight();
 
            }
            else
            {
                s_t_price t_price = game_data._instance.get_t_price(m_msg.buy_num + 1);
                s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
                if (m_msg.buy_num >= t_vip.guild_attack_num)
                {
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_buttle_gui.cs_73_47"));//[ffc882]今日购买次数已经用完，提升vip等级可增加购买次数
                    return;
                }
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
                s_message _mes = new s_message();
                _mes.m_type = "buy_guild_attack_num";
                root_gui._instance.show_cishu_dialog_box(m_msg.num, t_vip.guild_attack_num, m_msg.buy_num,1, _mes);

            }
        }
		if(obj.transform.name == "duixing")
		{
			s_message _message = new s_message();
			_message.m_type = "show_duixing_gui";
			_message.m_bools.Add(true);
			cmessage_center._instance.add_message(_message);
		}
 
    }
    void fight()
    {
        protocol.game.cmsg_guild_mission_fight_end _msg = new protocol.game.cmsg_guild_mission_fight_end();
        _msg.index = m_id;
        net_http._instance.send_msg<protocol.game.cmsg_guild_mission_fight_end>(opclient_t.CMSG_GUILD_BOSS_FIGHT_END, _msg);
    }
   public void reset()
    {
        switch (m_id)
        {
            case 0:
                m_name_label.text = game_data._instance.get_t_language ("guildboss_task_gui.cs_286_10");//坚苍守卫
                break;
            case 1:
                m_name_label.text = game_data._instance.get_t_language ("guildboss_task_gui.cs_289_10");//韧战守卫
                break;
            case 2:
                m_name_label.text = game_data._instance.get_t_language ("guildboss_task_gui.cs_292_10");//迅霆守卫
                break;
            case 3:
                m_name_label.text = game_data._instance.get_t_language ("guildboss_task_gui.cs_295_10");//布洛守卫
                break;
        }
       
        sys._instance.remove_child(m_icon);
        ccard _card = new ccard();
		s_t_monster t_monster = game_data._instance.get_t_monster(m_mission.ids[m_id]);
        dhc.role_t _role = new dhc.role_t();
        _role.template_id = t_monster.class_id;
        _card.set_role(_role);
        GameObject obj = icon_manager._instance.create_card_icon_ex(_card);
        obj.transform.parent = m_icon.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
		obj.transform.GetComponent<BoxCollider>().enabled = false;
        m_jisha_label.text = string.Format(game_data._instance.get_t_language ("guild_buttle_gui.cs_136_43"), m_mission.exp );//军团经验+{0}
        m_hp_slider.GetComponent<UIProgressBar>().value = get_hp(m_id);
        m_attack_num.text = game_data._instance.get_t_language ("guild_buttle_gui.cs_138_28") + m_msg.num;//挑战次数：
        m_hp_slider.transform.Find("text").GetComponent<UILabel>().text = get_hp_text(m_id);
        sys._instance.remove_child(m_icon_guild);
        GameObject icon = icon_manager._instance.create_resource_icon(10,0);
        icon.transform.parent = m_icon_guild.transform;
        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.one;
        m_attack_label.text = game_data._instance.get_t_language ("guild_buttle_gui.cs_145_30") + "：" + m_mission.basicCon + "-" + m_mission.maxCon;//数量
        m_desc_label.text = m_mission.jishaCon + "";
       if(m_id == 0)
       {
           m_mondesc_label.text = game_data._instance.get_t_language ("guild_buttle_gui.cs_149_34");//物免属性
       }
       else if (m_id == 1)
       {
           m_mondesc_label.text = game_data._instance.get_t_language ("guild_buttle_gui.cs_153_34");//魔免属性
       }
       else if (m_id == 2)
       {
           m_mondesc_label.text = game_data._instance.get_t_language ("guild_buttle_gui.cs_157_34");//闪避属性
       }
       else if (m_id == 3)
       {
           m_mondesc_label.text = game_data._instance.get_t_language ("guild_buttle_gui.cs_161_34");//格挡属性
       }
       if (get_hp_num(m_id) == 0)
       {
           this.gameObject.SetActive(false);
       }
    }
   float get_hp(int id)
   {
       long cur = 0;
       long max = 0;
       for (int i = 0; i < 5; i++)
       {
           cur += m_msg.mission.guild_cur_hps[i + id * 5];
           max += m_msg.mission.guild_max_hps[i + id * 5];
       }
       return (float)cur / max;
   }
   string get_hp_text(int id)
   {
       long cur = 0;
       long max = 0;
       for (int i = 0; i < 5; i++)
       {
           cur += m_msg.mission.guild_cur_hps[i + id * 5];
           max += m_msg.mission.guild_max_hps[i + id * 5];
       }
       return sys._instance.value_to_wan(cur) + "/" + sys._instance.value_to_wan(max);
   }
   long get_hp_num(int id)
   {
       long cur = 0;
       for (int i = 0; i < 5; i++)
       {
           cur += m_msg.mission.guild_cur_hps[i + id * 5];
       }
       return cur;
 
   }

    void OnDestroy()
    {
 
    }
}
