
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chenhao_gui : MonoBehaviour,IMessage{

	public GameObject m_down;
	public GameObject m_scro;
	public GameObject m_info_gui;
	public GameObject m_info_name;
	public GameObject m_info_desc;
	public GameObject m_info_time;
	public GameObject m_info_icon;
	public GameObject m_info_sx;
	public GameObject m_info_xiexia;
	public GameObject m_info_hq;
	public GameObject m_info_pd;
	public GameObject m_pvp_effect;
	public GameObject m_send_effect;
	public GameObject m_collect_effect;
	public GameObject m_sm_gui;
	public GameObject m_sm_scro;
	private int select = 1;
	private int m_id = 0;
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
		select = 1;
		reset ();
		effect ();
	}

	public void effect()
	{
		m_pvp_effect.SetActive (false);
		m_send_effect.SetActive (false);
		m_collect_effect.SetActive (false);
		if(sys._instance.chenghao_effect == 1)
		{
			m_pvp_effect.SetActive(false);
			sys._instance.chenghao_effect = 0;
		}
		else if(sys._instance.chenghao_effect == 2)
		{
			m_collect_effect.SetActive (true);
		}
		else if(sys._instance.chenghao_effect == 3)
		{
			m_send_effect.SetActive (true);
		}
	}
	void reset()
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_scro);
		List<int> ids = new List<int>();
		foreach(int id in game_data._instance.m_dbc_cheng_hao.m_index.Keys)
		{
			s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao(id);
			if(t_chenghao.is_show == select)
			{
				ids.Add(t_chenghao.id);
			}
		}
		for(int i = 0; i < ids.Count;++i)
		{
			s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao(ids[i]);
			GameObject obj = game_data._instance.ins_object_res("ui/chenhao_item");
			obj.transform.parent = m_scro.transform;
			obj.transform.localScale = new Vector3(1,1,1);
			obj.transform.localPosition = new Vector3(90,163 - i * 120,1);
			obj.GetComponent< chenhao_item>().m_id = ids[i];
			obj.GetComponent< chenhao_item>().reset();

			sys._instance.add_pos_anim(obj,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(obj,0.3f, 0, 1.0f, i * 0.05f);
		}
		if(ids.Count > 5)
		{
			m_down.SetActive(true);
		}
		else
		{
			m_down.SetActive(false);
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "send")
		{
			if(sys._instance.chenghao_effect == 3)
			{
				m_send_effect.SetActive(false);
				sys._instance.chenghao_effect = 0;
			}
			select = 3;
			reset();
		}
		else if(obj.transform.name == "collect")
		{
			if(sys._instance.chenghao_effect == 2)
			{
				m_collect_effect.SetActive(false);
				sys._instance.chenghao_effect = 0;
			}
			select = 2;
			reset();
		}
		else if(obj.transform.name == "pvp")
		{
			select = 1;
			reset();
		}
		else if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
		else if(obj.transform.name == "xiexia")
		{
			s_message _message = new s_message();
			_message.m_type = "refresh_chenghao";
			_message.m_ints.Add(m_id);
			cmessage_center._instance.add_message(_message);
		}
		else if(obj.transform.name == "pd")
		{
			s_message _message = new s_message();
			_message.m_type = "refresh_chenghao";
			_message.m_ints.Add(m_id);
			cmessage_center._instance.add_message(_message);
		}
		else if(obj.transform.name == "info_close")
		{
			m_info_gui.GetComponent<ui_show_anim>().hide_ui();
		}
		else if(obj.transform.name == "hq")
		{
			m_info_gui.GetComponent<ui_show_anim>().hide_ui();
			s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao (m_id);
			int type = t_chenghao.type;
			s_message _message = new s_message ();
			s_message _message1 = new s_message ();
			switch(type)
			{
                case 2:
                    _message1.m_type = "hide_chenghao_gui";
                    cmessage_center._instance.add_message(_message1);

                    _message.m_type = "show_guild_hongbao";
                    _message.m_ints.Add(1);
                    cmessage_center._instance.add_message(_message);
                    break;
                case 3:
                    _message1.m_type = "hide_chenghao_gui";
                    cmessage_center._instance.add_message(_message1);

                    _message.m_type = "show_guild_hongbao";
                    _message.m_ints.Add(0);
                    cmessage_center._instance.add_message(_message);
                    break;
			case 4:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pvp)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("chenhao_gui.cs_182_74") , (int)e_open_level.el_pvp  + ""));//猎人大会{0}级开启
					return;
				}
				_message1.m_type = "hide_chenghao_gui";
				cmessage_center._instance.add_message(_message1);

				_message.m_type = "show_huo_dong";
				_message.m_ints.Add(10);
				cmessage_center._instance.add_message(_message);
				break;
			}
		}
		else if(obj.transform.name == "sm")
		{
			if(m_sm_scro.transform.GetComponent<SpringPanel>() != null)
			{
				m_sm_scro.transform.GetComponent<SpringPanel>().enabled = false;
			}
			m_sm_scro.transform.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
			m_sm_scro.transform.localPosition = new Vector3 (0, 0, 0);
			m_sm_gui.SetActive(true);
		}
		else if(obj.transform.name == "sm_close")
		{
			m_sm_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}
	
	void IMessage.message (s_message message)
	{
		if(message.m_type == "refresh_chenghao")
		{
			m_id = (int)message.m_ints[0];
			bool flag = false;
			for(int i = 0; i < sys._instance.m_self.m_t_player.chenghao.Count;++i)
			{
				if(sys._instance.m_self.m_t_player.chenghao[i] == m_id)
				{
					flag = true;
					break;
				}
			}
			if(!flag)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("chenhao_gui.cs_226_59"));//该称号已过期
				if(m_info_gui.activeSelf)
				{
					m_info_gui.GetComponent<ui_show_anim>().hide_ui();
				}
				reset();
				return;
			}
			protocol.game.cmsg_chenghao_on _msg = new protocol.game.cmsg_chenghao_on ();
			_msg.id = m_id;
			net_http._instance.send_msg<protocol.game.cmsg_chenghao_on> (opclient_t.CMSG_CHENGHAO_ON, _msg);
		}
		if(message.m_type == "click_chenghao_item")
		{
			m_id = (int)message.m_ints[0];
			info();
			m_info_gui.SetActive(true);
		}
		if(message.m_type == "hide_chenghao_gui")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	void info()
	{
		s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao (m_id);
		m_info_name.GetComponent<UILabel>().text = t_chenghao.name;
		m_info_icon.GetComponent<UISprite>().spriteName = t_chenghao.icon1;
		m_info_name.GetComponent<UILabel>().effectColor = sys._instance.get_chenghao_color (t_chenghao.id);
		m_info_xiexia.SetActive (false);
		m_info_hq.SetActive (false);
		m_info_pd.SetActive (false);
		string text = "";
		for(int i = 0;i < t_chenghao.attr.Count;++i)
		{
			if(i == t_chenghao.attr.Count - 1)
			{
				text += game_data._instance.get_value_string(t_chenghao.attr[i].attr,t_chenghao.attr[i].value);
			}
			else
			{
				text += game_data._instance.get_value_string(t_chenghao.attr[i].attr,t_chenghao.attr[i].value) + " ";
			}
		}
		if(text == "")
		{
			text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_187_79");//无属性加成
		}
		m_info_desc.GetComponent<UILabel>().text = t_chenghao.desc;
		if(!sys._instance.m_self.m_t_player.chenghao.Contains(t_chenghao.id))
		{
			m_info_time.SetActive(false);
			m_info_hq.SetActive(true);
		}
		else
		{
			if(sys._instance.m_self.m_t_player.chenghao_on == t_chenghao.id)
			{
				m_info_xiexia.SetActive(true);
			}
			else
			{
				m_info_pd.SetActive(true);
			}
		}
		if(t_chenghao.time > 0)
		{
			m_info_time.SetActive(true);
			m_info_time.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("chenghao_dialog_box.cs_41_69"), t_chenghao.time );//可获得称号{0}天
		}
		else
		{
			m_info_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chenghao_dialog_box.cs_45_55");//可获得永久称号
		}
		m_info_sx.GetComponent<UILabel>().text = text;
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_CHENGHAO_ON)
		{
			if(sys._instance.m_self.m_t_player.chenghao_on == m_id)
			{
				sys._instance.m_self.m_t_player.chenghao_on = 0;
			}
			else
			{
				sys._instance.m_self.m_t_player.chenghao_on = m_id;
			}
			if(m_info_gui.activeSelf)
			{
				m_info_gui.GetComponent<ui_show_anim>().hide_ui();
			}
			s_message _message3 = new s_message();
			_message3.m_type = "check_bf";
			cmessage_center._instance.add_message(_message3);
			reset();
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
