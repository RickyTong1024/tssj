using System.Collections.Generic;
using UnityEngine;

public class juntuan_panel : MonoBehaviour ,IMessage{
	private List<dhc.guild_t> m_guild_list_t;
	public GameObject m_juntuan_items;
    protocol.game.smsg_guild_mission_ranking _msg;
    public ulong guid;
    public GameObject m_top1;
    public GameObject m_top2;
    public GameObject m_top3;
	public GameObject m_chuangjianjuntuan;
	public UIInput m_input_chuangjian;
    public GameObject m_button1;
    public GameObject m_button2;
	public GameObject m_sousuodialog;
	public GameObject m_input_sousuo;
	public GameObject m_queding_sousuo;

	
	public GameObject m_juntuan_button;
	public GameObject m_tip;
	public UIInput chuangjian_input;
	public UIInput souuo_input;

	public UILabel m_juntuan;
	public UILabel m_paiming;
	public UILabel m_juntuan1;
	public UILabel m_paiming1;
	public UILabel m_juntuanhuizhang;
	public UILabel m_juntuanming;
	public UILabel m_juntuandengji;
	public UILabel m_juntuanjifen;
	public UILabel m_renshu;
	public UILabel m_tuijianjuntuan;
	public UILabel m_sousuojuntuan;
	public UILabel m_sousuojuntuan1;
	public UILabel m_chuangjianjuntuan_label;
	public UILabel m_tip_label;
	public UILabel m_shurujuntuan;
	public UILabel m_shuruxianzhi;
	public UILabel m_shurujuntuan1;
	public UILabel m_shuruxianzhi1;
	public UILabel m_chuangjianjiage;
	public UILabel m_chuangjian;
	public UILabel m_sousuo;
    public GameObject m_skill_effect;
    public GameObject m_boss_effect;
    public GameObject m_mobai_effect;
    public GameObject m_main_effect;
    public GameObject m_shop_effect;
	public UILabel m_chuangjianjuntuan_label1;
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void IMessage.message (s_message message)
	{
		if (message.m_type == "chuangjiangonghui")
		{
			if (sys._instance.m_self.m_t_player.jewel < 500)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
				return;
			}
			string name = m_input_chuangjian.label.text;
			if(name.Trim() == "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("juntuan_info.cs_64_59"));//名字不能为空
			}
			else
			{
                if (game_data._instance.m_dfa.fei_fa(name) || name.IndexOf(" ") >= 0)
                {
                    root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("info.cs_277_58"));//[ffc882]非法名字
                    return;
                }
                protocol.game.cmsg_guild_create _msg = new protocol.game.cmsg_guild_create();
				_msg.guild_name = name;
				net_http._instance.send_msg<protocol.game.cmsg_guild_create> (opclient_t.CMSG_GUILD_CREATE, _msg);	

			}

		}
        else if (message.m_type == "guild_apply")
        {
            protocol.game.cmsg_guild_apply _msg = new protocol.game.cmsg_guild_apply();
            _msg.guild_guid = (ulong)message.m_object[0];
            guid = _msg.guild_guid;
            net_http._instance.send_msg<protocol.game.cmsg_guild_apply>(opclient_t.CMSG_GUILD_APPLY, _msg);
 
        }
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GUILD_CREATE) 
		{
			protocol.game.smsg_guild_data _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_data> (message.m_byte);
			sys._instance.m_self.m_t_player.guild = _msg.guild.guid;
			juntuan_gui._instance.m_guild_t = _msg.guild;
			juntuan_gui._instance.m_zhiwu_t = _msg.zhiwu;
			juntuan_gui._instance.m_guild_log = _msg.guild_event;
            juntuan_gui._instance.m_msg = _msg;
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, 500,game_data._instance.get_t_language ("juntuan_panel.cs_109_64"));//创建军团长消耗
            m_chuangjianjuntuan.transform.Find("frame_big").GetComponent<frame>().hide();
			m_chuangjianjuntuan.SetActive(false);
			juntuan_gui._instance.m_juntuan_open.SetActive (false);
		//	juntuan_gui._instance.m_juntuan_main.SetActive (true);
            juntuan_gui._instance.m_guild_main.SetActive(true);
            juntuan_gui._instance.updategui();
		//	juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().refresh("juntuan");
		

		}
		if (message.m_opcode == opclient_t.CMSG_GUILD_LIST_RECOMMEND) 
		{
			protocol.game.smsg_guild_list_recommend _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_list_recommend> (message.m_byte);
            List<dhc.guild_t> guilds = new List<dhc.guild_t>();
            for (int i = 0; i < _msg.guild_guids.Count; i++)
            {
                dhc.guild_t guild = new dhc.guild_t();
                guild.guid = _msg.guild_guids[i];

                guild.icon = _msg.guild_icons[i];
                guild.level = _msg.guild_levels[i];
                guild.name = _msg.guild_names[i];
                for (int j = 0; j < _msg.guild_members[i]; j++)
                {
                    guild.member_guids.Add(0);
                }
                guilds.Add(guild);
            }
            m_guild_list_t = guilds;
			juntuan_gui._instance.m_juntuan_open.GetComponent<juntuan_panel>().refresh (m_guild_list_t);
		}
		if (message.m_opcode == opclient_t.CMSG_GUILD_QUERY)
		{

			protocol.game.smsg_guild_list_recommend _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_list_recommend> (message.m_byte);
            List<dhc.guild_t> guilds = new List<dhc.guild_t>();
            for (int i = 0; i < _msg.guild_guids.Count; i++)
            {
                dhc.guild_t guild = new dhc.guild_t();
                guild.guid = _msg.guild_guids[i];
                guild.icon = _msg.guild_icons[i];
                guild.level = _msg.guild_levels[i];
                guild.name = _msg.guild_names[i];
                for (int j = 0; j < _msg.guild_members[i]; j++)
                {
                    guild.member_guids.Add(0);
                }
                guilds.Add(guild);
            }
            m_guild_list_t = guilds;
			juntuan_gui._instance.m_juntuan_open.GetComponent<juntuan_panel>().refresh (m_guild_list_t);
			m_juntuan_button.GetComponent<UIToggle>().value = true;
		}
		if (message.m_opcode == opclient_t.CMSG_GUILD_JOIN)
		{
			protocol.game.smsg_guild_data _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_data> (message.m_byte);
			sys._instance.m_self.m_t_player.guild =_msg.guild.guid;
			juntuan_gui._instance.m_guild_t = _msg.guild;
			juntuan_gui._instance.m_zhiwu_t = _msg.zhiwu;
			juntuan_gui._instance.m_guild_log = _msg.guild_event;
			juntuan_gui._instance.m_juntuan_open.SetActive(false);
			//juntuan_gui._instance.m_juntuan_main.SetActive(true);
			//juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().refresh("juntuan");
            juntuan_gui._instance.m_guild_main.SetActive(true);
            juntuan_gui._instance.updategui();
		}
		if (message.m_opcode == opclient_t.CMSG_GUILD_RANKING)
		{
			protocol.game.smsg_guild_ranking _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_ranking>(message.m_byte);
            List<dhc.guild_t> guilds = new List<dhc.guild_t>();
            for (int i = 0; i < _msg.guild_members.Count; i++)
            {
                dhc.guild_t guild = new dhc.guild_t();
                guild.icon = _msg.guild_icons[i];
                guild.level = _msg.guild_levels[i];
                guild.name = _msg.guild_names[i];
                for (int j = 0; j < _msg.guild_members[i]; j++)
                {
                    guild.member_guids.Add(0);
                }
                guilds.Add(guild);
            }

            juntuan_gui._instance.m_guild_list_t = guilds;
			juntuan_gui._instance.m_guild_list_t.Sort(juntuan_paiming_info.comp);
			refresh(juntuan_gui._instance.m_guild_list_t,1);
		}
        else if (message.m_opcode == opclient_t.CMSG_GUILD_MISSION_RANKING)
        {
            _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_mission_ranking>(message.m_byte);
            refresh_mission();

        }
        else if (message.m_opcode == opclient_t.CMSG_GUILD_APPLY)
        {
            sys._instance.m_self.m_t_player.guild_applys.Add(guid);
            if (sys._instance.m_self.m_t_player.guild_applys.Count >= 4)
            {
                for (int i = 1; i < sys._instance.m_self.m_t_player.guild_applys.Count; i++)
                {
                    sys._instance.m_self.m_t_player.guild_applys[i - 1] = sys._instance.m_self.m_t_player.guild_applys[i];
                }
                sys._instance.m_self.m_t_player.guild_applys.RemoveAt(3);
            }
            refresh(m_guild_list_t);
        }
	}
    public void refresh_mission()
    {
        m_top1.SetActive(false);
        m_top2.SetActive(true);
        m_top3.SetActive(false);
        m_button1.SetActive(false);
        m_button2.SetActive(true);
        m_juntuan_items.transform.localPosition = new Vector3(0, 0, 0);
        m_juntuan_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_juntuan_items);
        if (_msg.guild_name.Count == 0)
        {
            m_tip.SetActive(true);
        }
        else
        {
            m_tip.SetActive(false);
        }
        for (int i = 0; i < _msg.guild_name.Count; i++)
        {
            GameObject target = game_data._instance.ins_object_res("ui/juntuan/guilditem");

            target.transform.parent = m_juntuan_items.transform;
            target.transform.localPosition = new Vector3(0, 104 - i * 112, 0);
            target.transform.localScale = Vector3.one;
            target.transform.Find("name").GetComponent<UILabel>().text = _msg.guild_name[i];
            target.transform.Find("rank").GetComponent<UILabel>().text = i + 1 + "";
            target.transform.Find("icon").GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon(_msg.icon[i]).icon;
            target.transform.Find("num").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("boss.cs_234_75"), _msg.ceng[i]);//第{0}关
            target.transform.Find("blood").GetComponent<UILabel>().text = "[ff0000]" + ((float)_msg.hp[i] / _msg.max_hp[i] * 100) + "%";

        }

    }
	public void refresh(List<dhc.guild_t> t,int type=0)
	{

        m_guild_list_t = t;
		sys._instance.remove_child (m_juntuan_items);
		m_juntuan_items.transform.localPosition = new Vector3(0, 0, 0);
		m_juntuan_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		if(t.Count==0)
		{
			m_tip.SetActive(true);
		}
		else
		{
			m_tip.SetActive(false);

		}
        if (type == 0)
        {
            m_top1.SetActive(false);
            m_top2.SetActive(false);
            m_top3.SetActive(true);
            for (int i = 0; i < t.Count; i++)
            {
                GameObject target = game_data._instance.ins_object_res("ui/juntuan/juntuanitem");
                target.GetComponent<juntuan_item>().m_guilt = t[i];
                target.GetComponent<juntuan_item>().m_apply.SetActive(false);
                target.transform.parent = m_juntuan_items.transform;
                {
                    if (is_apply(t[i].guid))
                    { 
                        target.GetComponent<juntuan_item>().m_apply.SetActive(true);
                        target.GetComponent<juntuan_item>().m_join.SetActive(false);

                    }
                    else
                    {
                        target.GetComponent<juntuan_item>().m_join.SetActive(true);

                    }

                }
                target.transform.localPosition = new Vector3(0, 104 - i * 112, 0);
                target.transform.localScale = Vector3.one;
            }
            return;
        }
        m_top1.SetActive(true);
        m_top2.SetActive(false);
        m_top3.SetActive(false);
		for(int i = 0;i < t.Count;i ++)
		{
            GameObject target = game_data._instance.ins_object_res("ui/juntuan/guilditem");

            target.transform.parent = m_juntuan_items.transform;
            target.transform.localPosition = new Vector3(0, 104 - i * 112, 0);
            target.transform.localScale = Vector3.one;
            target.transform.Find("name").GetComponent<UILabel>().text = t[i].name;
            target.transform.Find("rank").GetComponent<UILabel>().text = i + 1 + "";
            target.transform.Find("icon").GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon(t[i].icon).icon;
            target.transform.Find("num").GetComponent<UILabel>().text = t[i].level + "";
            target.transform.Find("blood").GetComponent<UILabel>().text = "[00ff00]" + t[i].member_guids.Count.ToString() + "/" + game_data._instance.get_guild(t[i].level).number_count.ToString();
		}
	}
    bool is_apply(ulong guid)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.guild_applys.Count; i++)
        {
            if (sys._instance.m_self.m_t_player.guild_applys[i] == guid)
            {
                return true;
            }
        }
        return false;
 
    }
	public void click(GameObject obj)
	{
		if (obj.transform.name == "close")
		{
	        m_juntuan_button.GetComponent<UIToggle>().value = true;
		    this.gameObject.SetActive (false);
		    this.gameObject.transform.parent.gameObject.SetActive (false);
			m_chuangjianjuntuan.SetActive(false);
		    m_sousuodialog.SetActive(false);
			s_message _message = new s_message ();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message (_message);
		}
		else if (obj.transform.name == "chuangjian")
		{
            if(platform_config_common.game_model == 2)
            {
                if (sys._instance.m_self.m_t_player.total_recharge >= 50000)
                {
                    m_chuangjianjuntuan.SetActive(true);
                    chuangjian_input.value = "";
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("juntuan_panel.cs_349_111"));//充值100元才可创建军团
                    return;
                }
            }
            else
            {
                m_chuangjianjuntuan.SetActive(true);
                chuangjian_input.value = "";
            }
            
	        
		}
		else if (obj.transform.name == "sousuo") 
		{
		    m_sousuodialog.SetActive (true);
			souuo_input.value = "";
		}
		else if (obj.transform.name == "tuijian" || obj.transform.name == "juntuan")
		{
            m_button1.SetActive(true);
            m_button2.SetActive(false);
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_LIST_RECOMMEND, _msg,false,100);
		} 
		else if (obj.transform.name == "paiming")
		{
            m_button1.SetActive(false);
            m_button2.SetActive(true);
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_RANKING, _msg);
		}
		else if (obj.transform.name == "queding_chuangjian")
		{
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_chunjian)
            {
                root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("juntuan_panel.cs_367_72"),(int)e_open_level.el_chunjian));//[ffc882]该功能{0}级开启
                return;
            }
			s_message message = new s_message ();
			message.m_type = "chuangjiangonghui";
            cmessage_center._instance.add_message(message);
		} 
		else if (obj.transform.name == "queding_sousuo") 
		{    
			protocol.game.cmsg_guild_query _msg = new protocol.game.cmsg_guild_query ();
			_msg.guild_name = m_input_sousuo.GetComponent<UIInput>().label.text;
			net_http._instance.send_msg<protocol.game.cmsg_guild_query> (opclient_t.CMSG_GUILD_QUERY, _msg);
            m_sousuodialog.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.transform.name == "hide") 
		{
            m_sousuodialog.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.transform.name == "hide_create") 
		{
			m_chuangjianjuntuan.transform.Find("frame_big").GetComponent<frame>().hide();
		}
        else if (obj.name == "rank_gongxian")
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_RANKING, _msg);
           

        }
        else if (obj.name == "rank_fuben")
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_MISSION_RANKING, _msg);
           


        }
	}

}
