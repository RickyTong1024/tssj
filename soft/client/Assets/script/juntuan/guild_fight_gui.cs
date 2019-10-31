using System.Collections.Generic;
using UnityEngine;

public class guild_fight_gui : MonoBehaviour ,IMessage{

    public UILabel m_guild_all_zhanji;
    public UILabel m_guild_zhanji;
    public UILabel m_day_zhanji;
    public List<GameObject> m_guild_fights;
    protocol.game.msg_guild_fight m_fight;
    public UILabel m_guild_pvp_num;
    public UILabel m_time;
    public GameObject m_judian;
    public GameObject m_rank_gui;
    public GameObject m_target_gui;
    public GameObject m_zhang_kuang_gui;
    public GameObject m_target_effect;
    bool m_chat;
    public int num;
	public int count;
	public int sum;

    public void reset()
    {
        m_fight = juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().look_msg.fight;
        m_guild_all_zhanji.text = m_fight.guild_zhanji + "";
        m_guild_zhanji.text = m_fight.total_zhanji + "";
        m_day_zhanji.text = m_fight.zhanji + "";
        m_guild_pvp_num.text = game_data._instance.get_t_language ("guild_pvp_gui.cs_197_28") + "：" + sys._instance.m_self.m_t_player.guild_pvp_num + "";//攻打次数
        CancelInvoke();
        InvokeRepeating("time",0,1);
        int count_vaule = 0;
        foreach (int id in game_data._instance.m_dbc_guildfight.m_index.Keys)
        {
            s_t_guildfight fight = game_data._instance.get_guild_fight(id);
            if(fight != null)
            {
                count_vaule += fight.chengfangvalue;

            }
 
        }
        for (int i = 0; i < m_fight.fights.Count; i++)
        {
            if (i < m_guild_fights.Count)
            {
                int temp = 0;
                for (int j = 0; j < m_fight.fights[i].guard_points.Count; j++)
                {
                    temp += m_fight.fights[i].guard_points[j];
                }
               m_guild_fights[i].transform.Find("name").GetComponent<UILabel>().text = m_fight.fights[i].guild_name;
               m_guild_fights[i].transform.Find("zone").GetComponent<UILabel>().text = sys._instance.get_server(m_fight.fights[i].guild_server).m_name;
               m_guild_fights[i].transform.Find("hp").GetComponent<UIProgressBar>().value = (float)(count_vaule - temp) / count_vaule;
               m_guild_fights[i].transform.Find("hp/hp").GetComponent<UILabel>().text = (count_vaule - temp) + "/" +  count_vaule;
 
            }
            
        }
        if (m_judian.activeSelf)
        {
            m_judian.GetComponent<guild_judian_gui>().reset();
        }
        m_target_effect.SetActive(guildfight_target_gui.can_effect());
 
     }

     void Start()
     {
        cmessage_center._instance.add_handle(this);

     }

	void OnEnable()
	{
		if (root_gui._instance.m_default_active == "show_rank")
		{
			m_rank_gui.SetActive(true);

		}

	}


	void OnDestroy()
	{
		cmessage_center._instance.remove_handle(this);
	}

	void time()
	{
        long time = timer.get_time_cuo(24) - (long)timer.now();
        m_time.text = string.Format(game_data._instance.get_t_language ("guild_fight_gui.cs_92_36"), (int)timer.dtnow().DayOfWeek - 1,timer.get_time_show_rob(time));//军团战第{0}轮进行中，{1}结束
	}

	void IMessage.message(s_message mes)
	{
        
            if (mes.m_type == "buy_guild_fight_num")
            {
                if (this.gameObject.activeSelf)
                {
                    protocol.game.cmsg_shop_buy _msg = new protocol.game.cmsg_shop_buy();
                    _msg.num = (int)mes.m_ints[0];
                    num = _msg.num;
                    net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_GUILD_JT_BUY, _msg);
                }
            }
        
    }
     void IMessage.net_message(s_net_message msg)
     {
         if (msg.m_opcode == opclient_t.CMSG_GUILD_MESSAGE_VIEW)
         {
             m_chat = true;
 
         }
         else if (msg.m_opcode == opclient_t.CMSG_GUILD_JT_BUY)
         {
             if (this.gameObject.activeSelf)
             {
                 int jewel = 0;
                 for (int i = sys._instance.m_self.m_t_player.guild_pvp_buy_num + 1; i <= sys._instance.m_self.m_t_player.guild_pvp_buy_num + num; i++)
                 {
                     s_t_price t_price = game_data._instance.get_t_price(i);
                     jewel += t_price.guildpvpbuy;

                 }
                 sys._instance.m_self.sub_att(e_player_attr.player_jewel, jewel, game_data._instance.get_t_language ("guild_pvp_gui.cs_91_80"));//跨服军团战攻打次数购买
                 sys._instance.m_self.m_t_player.guild_pvp_buy_num += num;
                 sys._instance.m_self.m_t_player.guild_pvp_num += num;
                 string s = game_data._instance.get_t_language(game_data._instance.get_t_language ("bingyuan_gui.cs_776_19"));//获得
                 root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, "", "[ffffc0]" + "攻打次数" + "[ffd000] + " + num.ToString());
                 m_guild_pvp_num.text = game_data._instance.get_t_language ("guild_pvp_gui.cs_197_28") + "：" + sys._instance.m_self.m_t_player.guild_pvp_num + "";//攻打次数
 
             }
            
        
         }
        
     }
    public void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            if (m_rank_gui.activeSelf)
            {
                m_rank_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                m_rank_gui.GetComponent<guildfight_rank_gui>().m_first_toggle.value = true;
            }
            else if (m_zhang_kuang_gui.activeSelf)
            {
                m_zhang_kuang_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                m_zhang_kuang_gui.GetComponent<guild_zhangkuang_gui>().m_first_toggle.value = true;
            

            }
            else if (m_target_gui.activeSelf)
            {
                m_target_gui.transform.Find("frame_big").GetComponent<frame>().hide();               

            }
            else if (m_chat)
            {
                s_message mes = new s_message();
                mes.m_type = "close_guildchat";
                cmessage_center._instance.add_message(mes);
                m_chat = false;
            }
            else if (m_judian.activeSelf)
            {
                m_judian.SetActive(false);
            }
            else
            {
                m_judian.SetActive(false);
                this.gameObject.SetActive(false);
            }
 
        }
        else if (obj.name == "duixing")
        {
            s_message _message = new s_message();
            _message.m_type = "show_duixing_gui";
            _message.m_bools.Add(true);
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.name == "gongporeward")
        {
            m_target_gui.SetActive(true);
            m_target_gui.GetComponent<guildfight_target_gui>().reset();
 
        }
        else if (obj.name == "rankreward")
        {
            m_rank_gui.SetActive(true);
           
        }
        else if (obj.name == "guild_chat")
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_MESSAGE_VIEW, _msg);
        }
        else if (obj.name == "zhangkuang")
        {
            m_zhang_kuang_gui.SetActive(true);

        }
        else if (obj.name == "add")
        {
            buy_guild_fight_num();
        }
        else
        {

            int index = int.Parse(obj.name);
            if (index == 3)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_fight_gui.cs_218_58"));//[ffc884]不能进入自己所在军团
                return;
            }

            m_judian.GetComponent<guild_judian_gui>().reset(m_fight.fights[index]);
            m_judian.SetActive(true);

        }

    }
    void buy_guild_fight_num()
    {
        s_t_price t_price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.guild_buy_num + 1);
        s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
        if (sys._instance.m_self.m_t_player.guild_pvp_buy_num >= t_vip.guildpvpbuy_num)
        {
            root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language(game_data._instance.get_t_language ("guild_buttle_gui.cs_73_47")));//[ffc882]今日购买次数已经用完，提升vip等级可增加购买次数
            return;
        }
        if (sys._instance.m_self.m_t_player.jewel < t_price.guildpvpbuy)
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
        _mes.m_type = "buy_guild_fight_num";
        root_gui._instance.show_cishu_dialog_box(sys._instance.m_self.m_t_player.guild_pvp_num, t_vip.guildpvpbuy_num, sys._instance.m_self.m_t_player.guild_pvp_buy_num, 5, _mes);
    }

	
}
