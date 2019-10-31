
using UnityEngine;
using System.Collections;

public class transport_ship_gui : MonoBehaviour,IMessage {

	public GameObject m_name;
	public GameObject m_select;
	public GameObject m_free_num;
	public GameObject m_refresh_jewel;
	public GameObject m_gw_des1;
	public GameObject m_gw_des2;
	public GameObject m_gw_jewel;
	public GameObject m_time_des;
	public GameObject m_hw_des;

	public UILabel m_name_Label;
	public UILabel m_cur_ship;
	public UILabel m_ship_kind1;
	public UILabel m_ship_kind2;
	public UILabel m_ship_kind3;
	public UILabel m_ship_kind4;
	public UILabel m_ship_kind5;
	public UILabel m_refresh_ship;
	public UILabel m_refresh_Label;
	public UILabel m_cur_gw;
	public UILabel m_next_gw;
	public UILabel m_price;
	public UILabel m_gw_Label;
	public UILabel m_time_Label;
	public UILabel m_reward_Label;
	public UILabel m_start_Label;
	public UILabel m_zuyong_Label;
	public GameObject[] m_ship;

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);

	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		reset();
	}

	void click(GameObject obj)
	{
		if(obj.transform.name == "refresh")
		{
			if (sys._instance.m_self.m_t_player.yb_finish_num >= 3)
			{
				string s = game_data._instance.get_t_language ("transport_ship_gui.cs_57_15");//[ffc882]今日运输次数已用完
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (transport_gui.is_start_yb())
			{
				if (transport_gui.is_finish_yb())
				{
					string s = game_data._instance.get_t_language ("transport_ship_gui.cs_65_16");//[ffc882]请先领取上次奖励
					root_gui._instance.show_prompt_dialog_box(s);
					return;
				}
				string text = game_data._instance.get_t_language ("transport_ship_gui.cs_69_18");//[ffc882]已经开始运输
				root_gui._instance.show_prompt_dialog_box(text);
				return;
			}
			if (sys._instance.m_self.m_t_player.yb_type == 4)
			{
				string s = game_data._instance.get_t_language ("transport_ship_gui.cs_75_15");//[ffc882]已刷新出企业号货船
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (sys._instance.m_self.m_t_player.yb_refresh_num >= 3 && sys._instance.m_self.m_t_player.jewel < 20)
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();	
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_YB_REFRESH, _msg);
		}
		if(obj.transform.name == "max")
		{
			if (sys._instance.m_self.m_t_player.yb_finish_num >= 3)
			{
				string s = game_data._instance.get_t_language ("transport_ship_gui.cs_57_15");//[ffc882]今日运输次数已用完
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (transport_gui.is_start_yb())
			{
				if (transport_gui.is_finish_yb())
				{
					string s = game_data._instance.get_t_language ("transport_ship_gui.cs_65_16");//[ffc882]请先领取上次奖励
					root_gui._instance.show_prompt_dialog_box(s);
					return;
				}
				string text = game_data._instance.get_t_language ("transport_ship_gui.cs_69_18");//[ffc882]已经开始运输
				root_gui._instance.show_prompt_dialog_box(text);
				return;
			}
			if (sys._instance.m_self.m_t_player.yb_type == 4)
			{
				string s = game_data._instance.get_t_language ("transport_ship_gui.cs_75_15");//[ffc882]已刷新出企业号货船
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (sys._instance.m_self.m_t_player.jewel < 200)
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			s_message _message = new s_message();
			_message.m_type = "transport_ship_zh";
			string  _des = game_data._instance.get_t_language ("transport_ship_gui.cs_123_18");//是否花费[00ffff]200钻石[-]租用企业号货船
			root_gui._instance.show_select_dialog_box(tishi,_des,_message);
		}
		if(obj.transform.name == "gw")
		{
			if (sys._instance.m_self.m_t_player.yb_finish_num >= 3)
			{
				string s = game_data._instance.get_t_language ("transport_ship_gui.cs_57_15");//[ffc882]今日运输次数已用完
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (transport_gui.is_start_yb())
			{
				if (transport_gui.is_finish_yb())
				{
					string s = game_data._instance.get_t_language ("transport_ship_gui.cs_65_16");//[ffc882]请先领取上次奖励
					root_gui._instance.show_prompt_dialog_box(s);
					return;
				}
				string text = game_data._instance.get_t_language ("transport_ship_gui.cs_69_18");//[ffc882]已经开始运输
				root_gui._instance.show_prompt_dialog_box(text);
				return;
			}
			if (sys._instance.m_self.m_t_player.yb_gw_type == 5)
			{
				string s = game_data._instance.get_t_language ("transport_ship_gui.cs_148_15");//[ffc882]已鼓舞到最大值
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			s_t_yb_gw t_yb_gw = game_data._instance.get_t_yb_gw(sys._instance.m_self.m_t_player.yb_gw_type + 1);
			if (sys._instance.m_self.m_t_player.jewel < t_yb_gw.jewel)
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			s_message _message = new s_message();
			_message.m_type = "transport_ship_gw";
			string  _des = string.Format(game_data._instance.get_t_language ("transport_ship_gui.cs_162_32"),t_yb_gw.jewel );//是否花费[00ffff]{0}钻石[-]鼓舞
			root_gui._instance.show_select_dialog_box(tishi,_des,_message);
		}
		if(obj.transform.name == "start")
		{
			if (sys._instance.m_self.m_t_player.yb_finish_num >= 3)
			{
				string s = game_data._instance.get_t_language ("transport_ship_gui.cs_57_15");//[ffc882]今日运输次数已用完
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (transport_gui.is_start_yb())
			{
				if (transport_gui.is_finish_yb())
				{
					string s = game_data._instance.get_t_language ("transport_ship_gui.cs_65_16");//[ffc882]请先领取上次奖励
					root_gui._instance.show_prompt_dialog_box(s);
					return;
				}
				string text = game_data._instance.get_t_language ("transport_ship_gui.cs_69_18");//[ffc882]已经开始运输
				root_gui._instance.show_prompt_dialog_box(text);
				return;
			}
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();		
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_YB, _msg);
		}
		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	void reset()
	{
		int type = sys._instance.m_self.m_t_player.yb_type;
		m_select.transform.localPosition = m_ship [type].transform.localPosition;
		s_t_yb t_yb = game_data._instance.get_t_yb(type);
		s_t_exp t_exp = game_data._instance.get_t_exp (sys._instance.m_self.m_t_player.level);
        int per = t_yb.yuanli;
        string _des = (t_exp.yuanli * per ).ToString();
		m_hw_des.GetComponent<UILabel>().text = _des;
		
        if (t_yb.type == 0)
        {
            m_name.GetComponent<UILabel>().gradientTop = m_ship_kind1.gradientTop;
            m_name.GetComponent<UILabel>().gradientBottom = m_ship_kind1.gradientBottom;
            m_name.GetComponent<UILabel>().applyGradient = true;
 
        }
        else if (t_yb.type == 1)
        {
            m_name.GetComponent<UILabel>().gradientTop = m_ship_kind2.gradientTop;
            m_name.GetComponent<UILabel>().gradientBottom = m_ship_kind2.gradientBottom;
            m_name.GetComponent<UILabel>().applyGradient = true;
 
        }
        else if (t_yb.type == 2)
        {
            m_name.GetComponent<UILabel>().gradientTop = m_ship_kind3.gradientTop;
            m_name.GetComponent<UILabel>().gradientBottom = m_ship_kind3.gradientBottom;
            m_name.GetComponent<UILabel>().applyGradient = true;
 
        }
        else if (t_yb.type == 3)
        {
            m_name.GetComponent<UILabel>().gradientTop = m_ship_kind4.gradientTop;
            m_name.GetComponent<UILabel>().gradientBottom = m_ship_kind4.gradientBottom;
            m_name.GetComponent<UILabel>().applyGradient = true;
 
        }
        else if (t_yb.type == 4)
        {
            m_name.GetComponent<UILabel>().gradientTop = m_ship_kind5.gradientTop;
            m_name.GetComponent<UILabel>().gradientBottom = m_ship_kind5.gradientBottom;
            m_name.GetComponent<UILabel>().applyGradient = true;
 
        }
        m_name.GetComponent<UILabel>().text = t_yb.name;
		m_time_des.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("transport_ship_gui.cs_240_59"),(t_yb.time / 60000).ToString() );//{0}分钟
		if (sys._instance.m_self.m_t_player.yb_refresh_num < 3)
		{
			m_free_num.SetActive(true);
			m_refresh_jewel.SetActive(false);
		}
		else
		{
			m_free_num.SetActive(false);
			m_refresh_jewel.SetActive(true);
		}

		int index = sys._instance.m_self.m_t_player.yb_gw_type;
		s_t_yb_gw t_yb_gw = game_data._instance.get_t_yb_gw(index);
		s_t_yb_gw t_yb_gw1 = game_data._instance.get_t_yb_gw(index + 1);
		m_gw_des1.GetComponent<UILabel>().text = t_yb_gw.desc;
		if (t_yb_gw1 != null)
		{
			m_gw_des2.GetComponent<UILabel>().text = t_yb_gw1.desc;
			m_gw_jewel.GetComponent<UILabel>().text = t_yb_gw1.jewel.ToString();
		}
		else
		{
			m_gw_des2.GetComponent<UILabel>().text = "----";
			m_gw_jewel.GetComponent<UILabel>().text = "--";
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_YB_REFRESH)
		{
			protocol.game.smsg_yb_refresh _msg = net_http._instance.parse_packet<protocol.game.smsg_yb_refresh> (message.m_byte);
			sys._instance.m_self.m_t_player.yb_type = _msg.type;
			if (sys._instance.m_self.m_t_player.yb_refresh_num >= 3)
			{
				sys._instance.m_self.sub_att(e_player_attr.player_jewel, 20,game_data._instance.get_t_language ("transport_ship_gui.cs_276_64"));//运输刷新消耗
			}

			sys._instance.m_self.m_t_player.yb_refresh_num++;
			s_t_yb t_yb = game_data._instance.get_t_yb(_msg.type);
			root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("transport_ship_gui.cs_281_59") , t_yb.name ));//[ffc882]你刷新到了{0}货船
			reset();
		}

		if(message.m_opcode == opclient_t.CMSG_YB_GW)
		{
			s_t_yb_gw t_yb_gw = game_data._instance.get_t_yb_gw(sys._instance.m_self.m_t_player.yb_gw_type + 1);
			sys._instance.m_self.m_t_player.yb_gw_type++;
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, t_yb_gw.jewel,game_data._instance.get_t_language ("transport_ship_gui.cs_289_74"));//运输鼓舞消耗
			reset ();
		}

		if(message.m_opcode == opclient_t.CMSG_YB_ZH)
		{
			sys._instance.m_self.m_t_player.yb_type = 4;
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, 200,game_data._instance.get_t_language ("transport_gui.cs_547_63"));//运输消耗
			reset ();
		}

		if(message.m_opcode == opclient_t.CMSG_YB)
		{
			sys._instance.m_self.m_t_player.yb_start_time = timer.now();
			sys._instance.m_self.m_t_player.yb_jiasu_time = 0;
			sys._instance.m_self.m_t_player.yb_finish_num++;
			sys._instance.m_self.m_t_player.yb_per = 100;
			sys._instance.m_self.m_t_player.yb_level = sys._instance.m_self.m_t_player.level;

			this.transform.Find("frame_big").GetComponent<frame>().hide();
		
			s_message _message = new s_message();
			_message.m_type = "transport_add_self";
			cmessage_center._instance.add_message(_message);
		}
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "transport_ship_gw")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_YB_GW, _msg);
		}
		else if (message.m_type == "transport_ship_zh")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_YB_ZH, _msg);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
