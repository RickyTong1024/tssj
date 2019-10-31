using System.Collections.Generic;
using UnityEngine;

public class juntuan_gui : MonoBehaviour,IMessage
{
	public  List<dhc.guild_t> m_guild_list_t;
	public dhc.guild_t m_guild_t = new dhc.guild_t();
	public int m_zhiwu_t;
	public List<dhc.guild_event_t> m_guild_log;
	public GameObject m_juntuan_open;
	public GameObject m_juntuan_main;
	public List<dhc.guild_member_t> m_guild_member_t;
	public List<int> m_defend_type;
	public List<int> m_defend_type_temp;
	public int m_contribution;
	public UILabel m_gongxian;
    public GameObject m_guild_chat;
    public int m_ceng;
    public GameObject m_guild_main;
    public GameObject m_guild_mobai;
    public GameObject m_guild_shop;
    public GameObject m_gongao_label;
    public GameObject m_guild_skill;
    public GameObject m_guild_rank;
    public GameObject m_guild_hongbao;
	public GameObject m_guild_kuafu;
	public GameObject m_button_top_shuoming;




	public protocol.game.msg_guild_member_info m_member_guids;
	public GameObject m_guild_kuafu_set;
	public GameObject m_tishi_save;
    public UILabel m_level;
    public UILabel m_name;
    public UIProgressBar m_bar;
    public UILabel m_barvalue;
	public UILabel m_biaoti;
	public UILabel m_neirong;
    public protocol.game.smsg_guild_data m_msg;
	public protocol.game.smsg_guild_fight_pvp_look look_msg;  //军团战查看
	public string m_text;
    public GameObject m_skill_effect;
    public GameObject m_boss_effect;
    public GameObject m_mobai_effect;
    public GameObject m_main_effect;
    public GameObject m_shop_effect;
    public GameObject m_chat_effect;
    public GameObject m_hongbao_effect;
    public GameObject m_kuafu_effect;
	public static juntuan_gui _instance;

	void Awake () 
	{
		cmessage_center._instance.add_handle (this);
		_instance = this;
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
       // m_guild_hongbao.GetComponent<guild_hongbao_gui>().Destroy();
	}
	public void reset()
	{
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_OPEN, _msg);
	}
	void IMessage.message (s_message message)
	{
        if (message.m_type == "close_guildchat")
        {
            m_guild_chat.SetActive(false);
        }

		if (message.m_type == "open_kuafu_reward")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_JT_LOOK, _msg);
		}


	}
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GUILD_OPEN)
		{
			{

				try
				{
					m_msg = net_http._instance.parse_packet<protocol.game.smsg_guild_data> (message.m_byte);

				} 
				catch
				{
					m_msg = null;
				}
                
				if (m_msg != null && m_msg.success == 200)
				{
					m_guild_t = m_msg.guild;
					m_zhiwu_t = m_msg.zhiwu;
					m_guild_log = m_msg.guild_event;
					//m_juntuan_main.GetComponent<juntuan_main>().refresh ("juntuan");
					m_guild_main.SetActive (true);
					updategui ();
				}
				else
				{
					protocol.game.smsg_guild_list_recommend _msg1 = net_http._instance.parse_packet<protocol.game.smsg_guild_list_recommend> (message.m_byte);
					sys._instance.m_self.m_t_player.guild_applys.Clear ();
					for (int i = 0; i < _msg1.apply_list.Count; i++)
					{
						sys._instance.m_self.m_t_player.guild_applys.Add (_msg1.apply_list [i]);

					}
					sys._instance.m_self.m_t_player.guild = 0;
					List<dhc.guild_t> guilds = new List<dhc.guild_t>();
					for (int i = 0; i < _msg1.guild_guids.Count; i ++)
					{
						dhc.guild_t guild = new dhc.guild_t ();
						guild.guid = _msg1.guild_guids [i];
						guild.icon = _msg1.guild_icons [i];
						guild.level = _msg1.guild_levels [i];
						guild.name = _msg1.guild_names [i];
						for (int j = 0; j < _msg1.guild_members[i]; j++)
						{
							guild.member_guids.Add (0);
						}
						guilds.Add (guild);
					}
					m_guild_list_t = guilds;
					m_juntuan_open.SetActive (true);
					m_juntuan_open.GetComponent<juntuan_panel>().m_button1.SetActive (true);
					m_juntuan_open.GetComponent<juntuan_panel>().m_button2.SetActive (false);
					m_juntuan_open.GetComponent<juntuan_panel>().refresh (m_guild_list_t, 0);
					m_guild_main.SetActive (false);
				}
			}

		}
		else if (message.m_opcode == opclient_t.CMSG_GUILD_MESSAGE_VIEW || message.m_opcode == opclient_t.CMSG_GUILD_MESSAGE_ADD || message.m_opcode == opclient_t.CMSG_GUILD_MESSAGE_DELETE || message.m_opcode == opclient_t.CMSG_GUILD_MESSAGE_TOP)
		{
            
			protocol.game.smsg_guild_message_view _msg = new protocol.game.smsg_guild_message_view ();
			_msg = net_http._instance.parse_packet<protocol.game.smsg_guild_message_view> (message.m_byte);
			m_guild_chat.SetActive (true);
			m_guild_chat.GetComponent<guild_chat>().m_msg = _msg;
			m_guild_chat.GetComponent<guild_chat>().m_msg.msgs.Sort (m_guild_chat.GetComponent<guild_chat>().comp); 
			m_guild_chat.GetComponent<guild_chat>().reset ();
		}
		else if (message.m_opcode == opclient_t.CMSG_GUILD_ACTIVITY)
		{
			m_guild_mobai.SetActive (true);
			m_guild_mobai.GetComponent<jt_mobai>().m_msg = net_http._instance.parse_packet<protocol.game.smsg_guild_activity> (message.m_byte);
			m_guild_mobai.GetComponent<jt_mobai>().updateui ();
			juntuan_gui._instance.updategui ();
		} 
		else if (message.m_opcode == opclient_t.CMSG_GUILD_MEMBER_VIEW)
		{
			protocol.game.smsg_guild_member_view _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_member_view> (message.m_byte);
			juntuan_gui._instance.m_guild_member_t = _msg.guild_members;
		} 
		
		else if(message.m_opcode == opclient_t.CMSG_GUILD_JT_LOOK)
		{
			//打开界面跨服军团战   look

			juntuan_gui._instance.m_guild_kuafu.SetActive(true);
			if(juntuan_gui._instance.m_guild_kuafu.activeSelf)
			{
				m_button_top_shuoming.SetActive(true);
			}
			m_guild_member_t.Clear();
			
			look_msg =net_http._instance.parse_packet<protocol.game.smsg_guild_fight_pvp_look>(message.m_byte);
			if(look_msg.stat == 5)
			{
				m_button_top_shuoming.SetActive(false);
			}
			m_guild_kuafu.GetComponent<juntuan_kuafu_control>().look_msg = look_msg;
			m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_gamestat = look_msg.stat;
			if(m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_gamestat == 4 )
			{
				if(look_msg.member != null)
				{
					
					for (int i = 0; i < look_msg.member.player_guids.Count; i++)
					{
						dhc.guild_member_t temp = new dhc.guild_member_t();
						temp.bf = look_msg.member.player_bat_eff[i];   //战斗力
						temp.player_guid = look_msg.member.player_guids[i];
						temp.player_iocn_id = look_msg.member.player_template[i];
						temp.player_vip = look_msg.member.player_vip[i];
						temp.player_name = look_msg.member.player_names[i];
						temp.player_level = look_msg.member.player_level[i];
						temp.join_time =look_msg.member.player_join_time[i];
						m_guild_member_t.Add(temp);
					}
					
					if(m_button_top_shuoming.activeSelf)
					{
						m_guild_member_t.Sort (compare);
						m_defend_type.Clear();
						for (int i = 0; i < m_guild_member_t.Count; i++)
						{
							
							if (m_guild_member_t [i].join_time + 24 * 60 * 60 * 1000   >= timer.now())
							{
								m_defend_type.Add (5);
							} 
							else
							{
								m_defend_type.Add (0); ////  type 0 为为布防，1  左据点 2右  3 主  4 司令。 5加入军团不满24小时
							}
						}
					}
				}
				else if(look_msg.member == null && m_zhiwu_t == 2 && look_msg.arrange != null)
				{
					for (int i = 0; i < look_msg.arrange.player_guids.Count; i++)
					{
						dhc.guild_member_t temp = new dhc.guild_member_t();
						temp.bf = look_msg.arrange.player_bat_eff[i];   //战斗力
						temp.player_guid = look_msg.arrange.player_guids[i];
						temp.player_iocn_id = look_msg.arrange.player_template[i];
						temp.player_vip = look_msg.arrange.player_vip[i];
						temp.player_name = look_msg.arrange.player_names[i];
						temp.player_level = look_msg.arrange.player_level[i];
//						temp.join_time =look_msg.member.player_join_time[i];
						m_guild_member_t.Add(temp);
					}
					
					if(m_button_top_shuoming.activeSelf)
					{
						m_guild_member_t.Sort (compare);
						m_defend_type.Clear();
						for (int i = 0; i < m_guild_member_t.Count; i++)
						{
							
							if (m_guild_member_t [i].join_time + 24 * 60 * 60 * 1000   >= timer.now())
							{
								m_defend_type.Add (5);
							} 
							else
							{
								m_defend_type.Add (0); ////  type 0 为为布防，1  左据点 2右  3 主  4 司令。 5加入军团不满24小时
							}
						}
					}
				}
				
				if(look_msg.arrange != null)
				{
					List<ulong> t =new List<ulong>();
					for (int j = 0; j < m_guild_member_t.Count; j++) 
					{	
						t.Add(m_guild_member_t[j].player_guid);
					}
					
					for (int i = 0; i < look_msg.arrange.player_guids.Count; i++) 
					{
						int temp =t.IndexOf(look_msg.arrange.player_guids[i]);
						
						if(temp != -1)
						{
							
							if((i / 7 == 0) )
							{
								m_defend_type[temp] = 1;
								
							}
							
							else if((i / 7 == 1))
							{
								m_defend_type[temp] = 2;
								
							}
							
							else if((i / 7 == 2) )
							{
								m_defend_type[temp] = 3;
								
							}
							
							else
							{
								m_defend_type[temp] = 4;
								
							}
						}
					}
				}
				
				m_defend_type_temp.Clear();
				for (int i = 0; i < m_defend_type.Count; i++) 
				{
					m_defend_type_temp.Add(m_defend_type[i]);
				}
			}
			else if(m_guild_kuafu.GetComponent<juntuan_kuafu_control>().m_gamestat == 6)
			{
				if(look_msg.member != null)
				{
					for (int i = 0; i < look_msg.member.player_guids.Count; i++)
					{
						dhc.guild_member_t temp = new dhc.guild_member_t();
						temp.bf = look_msg.member.player_bat_eff[i];   //战斗力
						temp.player_guid = look_msg.member.player_guids[i];
						temp.player_iocn_id = look_msg.member.player_template[i];
						temp.player_vip = look_msg.member.player_vip[i];
						temp.player_name = look_msg.member.player_names[i];
						temp.player_level = look_msg.member.player_level[i];
						temp.join_time =look_msg.member.player_join_time[i];
						m_guild_member_t.Add(temp);
					}
					
					if(m_button_top_shuoming.activeSelf)
					{
						m_guild_member_t.Sort (compare);
						m_defend_type.Clear();
						for (int i = 0; i < m_guild_member_t.Count; i++)
						{
							
							if (m_guild_member_t [i].join_time + 24 * 60 * 60 * 1000   >= timer.now())
							{
								m_defend_type.Add (5);
							} 
							else
							{
								m_defend_type.Add (0); ////  type 0 为为布防，1  左据点 2右  3 主  4 司令。 5加入军团不满24小时
							}
						}
					}
				}
				else if(look_msg.member == null && m_zhiwu_t == 2 && look_msg.zhanji.bushu != null)
				{
					for (int i = 0; i < look_msg.zhanji.bushu.player_guids.Count; i++)
					{
						dhc.guild_member_t temp = new dhc.guild_member_t();
						temp.bf = look_msg.zhanji.bushu.player_bat_eff[i];   //战斗力
						temp.player_guid = look_msg.zhanji.bushu.player_guids[i];
						temp.player_iocn_id = look_msg.zhanji.bushu.player_template[i];
						temp.player_vip = look_msg.zhanji.bushu.player_vip[i];
						temp.player_name = look_msg.zhanji.bushu.player_names[i];
						temp.player_level = look_msg.zhanji.bushu.player_level[i];
						//						temp.join_time =look_msg.member.player_join_time[i];
						m_guild_member_t.Add(temp);
					}
					
					if(m_button_top_shuoming.activeSelf)
					{
						m_guild_member_t.Sort (compare);
						m_defend_type.Clear();
						for (int i = 0; i < m_guild_member_t.Count; i++)
						{
							
							if (m_guild_member_t [i].join_time + 24 * 60 * 60 * 1000   >= timer.now())
							{
								m_defend_type.Add (5);
							} 
							else
							{
								m_defend_type.Add (0); ////  type 0 为为布防，1  左据点 2右  3 主  4 司令。 5加入军团不满24小时
							}
						}
					}
				}
				if(look_msg.zhanji.bushu != null)
				{
					List<ulong> t =new List<ulong>();
					for (int j = 0; j < m_guild_member_t.Count; j++) 
					{	
						t.Add(m_guild_member_t[j].player_guid);
					}
					
					for (int i = 0; i < look_msg.zhanji.bushu.player_guids.Count; i++) 
					{
						int temp =t.IndexOf(look_msg.zhanji.bushu.player_guids[i]);
						
						if(temp != -1)
						{
							
							if((i / 7 == 0) )
							{
								m_defend_type[temp] = 1;
								
							}
							
							else if((i / 7 == 1))
							{
								m_defend_type[temp] = 2;
								
							}
							
							else if((i / 7 == 2) )
							{
								m_defend_type[temp] = 3;
								
							}
							
							else
							{
								m_defend_type[temp] = 4;
								
							}
						}
					}
				}
				m_defend_type_temp.Clear();
				for (int i = 0; i < m_defend_type.Count; i++) 
				{
					m_defend_type_temp.Add(m_defend_type[i]);
				}
			}
			
			m_guild_kuafu.GetComponent<juntuan_kuafu_control>().reset();
		}


	}


	public void click(GameObject obj)
    {
        if (obj.transform.name == "close")
		{
			reset ();
			if (m_guild_shop.activeSelf)
			{
				m_guild_shop.GetComponent<guild_shop>().m_shop.value = true;
				m_guild_shop.transform.Find("frame_big").GetComponent<frame>().hide ();
				return;
			} 
			else if (m_guild_hongbao.activeSelf)
			{
				m_guild_hongbao.GetComponent<guild_hongbao_gui>().m_button_fa.value = true;
				m_guild_hongbao.transform.Find("frame_big").GetComponent<frame>().hide ();
			}
			else if (m_guild_mobai.activeSelf)
			{
				m_guild_mobai.SetActive (false);
				return;
			} 
			else if (m_guild_kuafu.activeSelf)
			{
				m_guild_kuafu.SetActive (false);
				m_button_top_shuoming.SetActive (false);
				return;
			} 
			else if (m_juntuan_main.activeSelf)
			{
				if (m_juntuan_main.GetComponent<juntuan_main>().m_apply_gui.activeSelf)
				{
					m_juntuan_main.GetComponent<juntuan_main>().m_apply_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
					return;
				}
				m_juntuan_main.GetComponent<juntuan_main>().m_juntuan_button.value = true;
				m_juntuan_main.transform.Find("frame_big").GetComponent<frame>().hide ();
				return;
			} 
			else if (m_guild_skill.activeSelf)
			{
				m_guild_skill.transform.Find("frame_big").GetComponent<frame>().hide ();
				m_guild_skill.GetComponent<guild_skill>().m_button_stu.value = true;
				return;
			} 
			else if (m_guild_rank.activeSelf)
			{
				m_guild_rank.transform.Find("frame_big").GetComponent<frame>().hide ();
				m_guild_rank.GetComponent<juntuan_paiming_info>().m_rank.value = true;
				return;

			} 
			else if (m_guild_chat.activeSelf)
			{
				m_guild_chat.SetActive (false);
				return;
			} 
			else if (m_guild_main.activeSelf || m_juntuan_open.activeSelf)
			{
				s_message _message = new s_message ();
				_message.m_type = "show_main_gui";
				cmessage_center._instance.add_message (_message);
				s_message _message1 = new s_message ();
				_message1.m_type = "close_main";
				cmessage_center._instance.add_message (_message1);
				m_juntuan_main.GetComponent<juntuan_main>().m_juntuan_button.GetComponent<UIToggle>().value = true;
				m_juntuan_open.GetComponent<juntuan_panel>().m_juntuan_button.GetComponent<UIToggle>().value = true;
				m_juntuan_main.SetActive (false);
				m_juntuan_open.SetActive (false);
				DestroyImmediate (this.gameObject);
			} 
			else
			{
				m_juntuan_main.transform.Find("frame_big").GetComponent<frame>().hide ();
			}
           		
		}
		else if (obj.transform.name == "guild_main")
		{
			m_juntuan_main.SetActive (true);
			m_juntuan_main.GetComponent<juntuan_main>().refresh ("juntuan");
		} 
		else if (obj.transform.name == "guild_rank") 
		{
			m_guild_rank.SetActive (true);
		} 
		else if (obj.name == "guild_mobai") 
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_ACTIVITY, _msg);
		} 
		else if (obj.name == "guild_boss") 
		{
			if (juntuan_gui._instance.m_guild_t.level >= (int)e_open_level.el_guild_boss_kaiqi) {
				this.gameObject.SetActive (false);
				sys._instance.m_game_state = "guild_boss_ex";

				sys._instance.load_scene ("ts_game_guildfb");
			} else {
				root_gui._instance.show_prompt_dialog_box ("[ffc882]" + string.Format (game_data._instance.get_t_language ("huo_yue_sub.cs_280_74"), (int)e_open_level.el_guild_boss_kaiqi));//军团副本需军团{0}级开启

			}


		} 
		else if (obj.name == "guild_shop") {
			m_guild_shop.SetActive (true);
		} 
		else if (obj.name == "guild_skill") 
		{
			if (juntuan_gui._instance.m_guild_t.level < 4) {
				root_gui._instance.show_prompt_dialog_box ("[ffc882]" + string.Format (game_data._instance.get_t_language ("juntuan_gui.cs_245_85"), (int)e_open_level.el_guild_keji));//军团科技需军团{0}级开启
				return;
			}
			m_guild_skill.SetActive (true);
			int index = int.Parse (game_data._instance.m_dbc_guild_keji.get (0, 0));
			m_guild_skill.GetComponent<guild_skill>().inde = index;
			m_guild_skill.GetComponent<guild_skill>().type = 0;
			m_guild_skill.GetComponent<guild_skill>().reset (0);
		}
		else if (obj.name == "notice") 
		{
			m_gongao_label.transform.parent.parent.gameObject.SetActive (!m_gongao_label.transform.parent.parent.gameObject.activeSelf);
            m_gongao_label.GetComponent<UILabel>().text = juntuan_gui._instance.set_juntuan_notice();
        }
		else if (obj.name == "guild_chat") 
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_MESSAGE_VIEW, _msg);

		} 
		else if (obj.name == "guild_hongbao") 
		{
			m_guild_hongbao.SetActive (true);
           
 
		} 
		else if (obj.name == "guild_main_kuafu") 
		{

			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_JT_LOOK, _msg);

		} 
    }

	public void juntuan_map()
	{
		if (juntuan_gui._instance.m_guild_t.level < (int)e_open_level.el_guild_boss_kaiqi)
		{
			root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_280_74") , (int)e_open_level.el_guild_boss_kaiqi ));//军团副本需军团{0}级开启
			return;
		}
		this.gameObject.SetActive(false);
		sys._instance.m_game_state = "guild_boss_ex";
		sys._instance.load_scene("ts_game_guildfb");
	}
	public string setKuang(int level)
	{
		string s = "";
		if (level == 1)
		{
			s = "ntx_kt001";
		}
		else if (level == 2)
		{
			s = "ntx_kt002";
		}
		else if (level == 3)
		{
			s = "ntx_kt003";
		}
		else if (level == 4)
		{
			s = "ntx_kt004";
		}
		else if (level == 5)
		{
			s = "ntx_kt005";
		}
		else if (level == 6)
		{
			s = "ntx_kt006";
		}
		else if (level == 7)
		{
			s = "ntx_kt007";
		}
		else if (level >= 8)
		{
			s = "ntx_kt008";
		}
		return s;
	}

	int compare(dhc.guild_member_t x, dhc.guild_member_t y)
	{
		int mX = x.bf;
		int mY = y.bf;
		if (mX > mY) 
		{
			return -1;
		}
		else
		{
			return 1;
		}
	}

    public string set_juntuan_notice()
    {
        if (platform_config_common.game_model == 2)
        {
            return game_data._instance.get_t_language("juntuan_gui.cs_549_85");
        }
        else
        {
            if (m_guild_t.gonggao != null && m_guild_t.gonggao == "")
            {
                return game_data._instance.get_t_language("juntuan_gui.cs_652_55");
            }
            else
            {
                return m_guild_t.gonggao;
            }
        }
    }

	public void updategui()
	{
		//m_gongxian.text = sys._instance.m_self.m_t_player.contribution + "";
		m_juntuan_main.GetComponent<juntuan_main>().m_juntuan.GetComponent<juntuan_info>().refresh ();
        if (platform_config_common.game_model == 2)
        {
            m_gongao_label.GetComponent<UILabel>().text = game_data._instance.get_t_language("juntuan_gui.cs_549_85");
        }
        else
        {
            m_gongao_label.GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_t.gonggao;
        }
        m_name.text = m_guild_t.name;
        m_level.text = "Lv" + m_guild_t.level;
        s_t_guild t_guild = game_data._instance.get_guild(juntuan_gui._instance.m_guild_t.level + 1);
        if (t_guild == null)
        {
            m_bar.value = 1.0f;
            m_barvalue.text = "--/--";
        }
        else
        {
            m_bar.value = (float)juntuan_gui._instance.m_guild_t.exp / t_guild.exp;//306 -257 229
            m_barvalue.text = juntuan_gui._instance.m_guild_t.exp.ToString() + "/" + t_guild.exp.ToString();
        }
        m_skill_effect.SetActive(false);
        m_boss_effect.SetActive(juntuan_gui._instance.m_msg.can_fight);
        if (juntuan_gui._instance.m_guild_t.level < 2)
        {
            m_boss_effect.SetActive(false);
 
        }
        m_mobai_effect.SetActive(juntuan_gui._instance.m_msg.can_mobai);
        m_main_effect.SetActive(juntuan_gui._instance.m_msg.has_apply);
        m_shop_effect.SetActive(guild_shop.can_shop());
        m_hongbao_effect.SetActive(guild_hongbao_gui.hongbao_effect());
        m_kuafu_effect.SetActive(juntuan_gui._instance.m_msg.guildpvp);
        if (juntuan_gui._instance.m_msg.msg_count > 0)
        {
            m_chat_effect.SetActive(true);
            if (juntuan_gui._instance.m_msg.msg_count >= 100)
            {
                m_chat_effect.transform.Find("Label").GetComponent<UILabel>().text = 99 + "";
            }
            else
            {
                m_chat_effect.transform.Find("Label").GetComponent<UILabel>().text = juntuan_gui._instance.m_msg.msg_count + "";
            }
        }
        else
        {
            m_chat_effect.SetActive(false);
        }
        
	}
    
}
