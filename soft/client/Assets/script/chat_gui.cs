
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chat_gui : MonoBehaviour, IMessage {	

	// Use this for initialization
	public static chat_gui _instance;
	public GameObject m_view;
	public List<List<protocol.game.smsg_chat>> m_chat_list;
	public GameObject m_text;
	public GameObject m_htext;
	public GameObject m_tname;
	public GameObject m_htname;
	public GameObject m_shijie;
	public GameObject m_siliao;
	public GameObject m_oppanel;
	public int m_type = 0;
	private GameObject m_select;
	public static bool m_show_effect;
	public GameObject m_effect;
	private bool m_bdanmu;
	public GameObject m_danmu;
	public GameObject m_chcolor;
	public GameObject m_color;
	public List<string> m_colors = new List<string>();
	public List<Color> m_colors1 = new List<Color>();

	private int m_index = 0;

	void Start () {


		if(game_data._instance.m_player_data.m_danmu > 0)
		{	
			m_bdanmu = true;
		}
		else
		{
			m_bdanmu = false;
		}
		do_danmu ();
	}
	void OnDestroy()
	{	
		cmessage_center._instance.remove_handle (this);
	}
	public void init()
	{	
		_instance = this;
		cmessage_center._instance.add_handle (this);

		m_chat_list = new List<List<protocol.game.smsg_chat>>();
		for (int i = 0 ; i < 3; ++i)
		{	
			List<protocol.game.smsg_chat> l = new List<protocol.game.smsg_chat>();
			m_chat_list.Add(l);
		}
		
		for (int i = 0; i < sys._instance.m_chat_msgs.Count; ++i)
		{	
            if (sys._instance.m_chat_msgs[i].type == 0)
            {	
                m_chat_list[0].Add(sys._instance.m_chat_msgs[i]);
            }
            else
            {
                m_chat_list[2].Add(sys._instance.m_chat_msgs[i]);
            }
		}
		for (int i = 0; i < sys._instance.m_chat_msgs1.Count; ++i)
		{	
			m_chat_list[1].Add(sys._instance.m_chat_msgs1[i]);
		}
	}
	
	public void OnEnable()
	{	
		reset ();
	}

	void hide()
	{			
		m_shijie.GetComponent<UIToggle>().value = true;
		m_type = 0;
	}

	public void reset()
	{	
		m_oppanel.SetActive(false);
        init();
		if (m_type == 2)
		{	
			m_htext.transform.localPosition = new Vector3(272, 514, 0);
			m_htext.GetComponent<UISprite>().width = 442;
			m_htname.SetActive(true);
		}
		else
		{
			m_htext.transform.localPosition = new Vector3(207, 514, 0);
			m_htext.GetComponent<UISprite>().width = 573;

			m_htname.SetActive(false);
		}

		sys._instance.remove_child (m_view);
		m_view.transform.localPosition = Vector3.zero;
		m_view.GetComponent<UIPanel>().clipOffset = Vector2.zero;
		for (int i =  m_chat_list[m_type].Count - 1, num = 0; i >= 0; i--,num ++)
		{	
			GameObject chat = game_data._instance.ins_object_res("ui/chat");

			protocol.game.smsg_chat scs = m_chat_list[m_type][i];
			chat.transform.parent = m_view.transform;
			chat.transform.localPosition = new Vector3(80, 197 - 130 * num, 0);
			chat.transform.localScale = new Vector3(1,1,1);
			chat.transform.GetComponent<chat>().m_scs = scs;
			chat.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			chat.transform.GetComponent<chat>().init();

		}
		//m_view.GetComponent<UIScrollView>().ResetPosition ();
		s_message _mes = new s_message ();
		_mes.m_type = "fresh_chat";
		cmessage_center._instance.add_message (_mes);
		m_text.GetComponent<UILabel>().text = "";
		reset_button ();
	}

	void reset_button()
	{	
		if (m_show_effect)
		{	
			m_effect.SetActive(true);
		}
		else
		{
			m_effect.SetActive(false);
		}
	}
	public void elegate()
	{	

		this.gameObject.SetActive (false);
		this.transform.parent.gameObject.SetActive (false);

	}
	public void click(GameObject obj)
	{	
		if(obj.name == "close" || obj.name == "close1")
		{	
			hide();
			TweenPosition.Begin(this.gameObject,0.3f,new Vector3(-800,0,0));
			Invoke("elegate",0.3f);

		}
		else if (obj.name == "danmu")
		{	
			m_bdanmu = !m_bdanmu;
			do_danmu();
			
			if(m_bdanmu)
			{	
				game_data._instance.m_player_data.m_danmu = 1;
			}
			else
			{
				game_data._instance.m_player_data.m_danmu = 0;
				root_gui._instance.m_danmu_panel.GetComponent<danmu_panel>().remove_all();
			}
			game_data._instance.save ();

			s_message _mes = new s_message();
			_mes.m_type = "change_danmu";
			cmessage_center._instance.add_message(_mes);
		}
		else if(obj.name == "send")
		{	
			try
			{
				m_text.GetComponent<UILabel>().trueTypeFont.RequestCharactersInTexture(m_text.GetComponent<UILabel>().text, NGUIText.finalSize, NGUIText.fontStyle);
			}
			catch(System.Exception)
			{	
				return;
			}
			string text = m_text.GetComponent<UILabel>().text.Replace("\n","");
			text = game_data._instance.m_dfa.search(text);

			m_text.GetComponent<UILabel>().text = "";
			m_text.GetComponent<UIInput>().value = "";
			string target_name = m_tname.GetComponent<UILabel>().text;

			if(sys._instance.m_self.m_t_player.gag_time > timer.now())
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("chat_gui.cs_shoudong") + timer.get_time_show_color_ex((long)(sys._instance.m_self.m_t_player.gag_time - timer.now()), "[ffc882]", "[ffc882]"));//禁言剩余时间:
				return;
			}

			if (text == "")
			{	
				return;
			}
			if (m_type == 1 && sys._instance.m_self.m_t_player.guild == 0)
			{	
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("chat_gui.cs_213_59"));//加入军团后才可以使用军团聊天
				return;
			}
			if (m_type == 2 && target_name == "")
			{	
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("chat_gui.cs_218_59"));//请输入有效的用户名
				return;
			}
			if (m_type == 2 && target_name == sys._instance.m_self.m_t_player.name)
			{	
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("chat_gui.cs_223_59"));//不能和自己聊天
				return;
			}

			if (text[0] == '#')
			{	
				text = text.Substring(1);

				s_message _msg = new s_message();

				_msg.m_type = "part";
				_msg.m_string.Add(text);

				cmessage_center._instance.add_message(_msg);
			}
			else if (text[0] == '$')
			{	
				text = text.Substring(1);
				
				root_gui._instance.action_guide(text);
			}
			else if (text[0] == '@')
			{	
				text = text.Substring(1);
				protocol.game.cmsg_gm_command _msg = new protocol.game.cmsg_gm_command ();
				_msg.text = text;
				net_http._instance.send_msg<protocol.game.cmsg_gm_command> (opclient_t.CMSG_GM_COMMAND, _msg);
			}
			else
			{
                if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_chat)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("chat_gui.cs_255_74"), (int)e_open_level.el_chat));//聊天{0}级开启
                    return;
                }

                if (platform_config_common.game_model == 2 && m_type == 0)
                {
                    if (sys._instance.m_self.m_t_player.total_recharge < 50000)
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("chat_gui.cs_259_93"));//充值100元可以发送聊天消息
                        return;
                    }
                }
                protocol.game.cmsg_chat_ex _msg = new protocol.game.cmsg_chat_ex ();
				_msg.type = m_type;
				_msg.text = text;
				_msg.color = m_colors[m_index];
				_msg.target_name = target_name;
				net_http._instance.send_msg_ex<protocol.game.cmsg_chat_ex> (opclient_t.CMSG_CHAT, _msg);
			}
		}
		else if (obj.name == "shijie")
		{	
			m_type = 0;
			reset();
			m_tname.GetComponent<UIInput>().value = "";
		}
		else if (obj.name == "juntuan")
		{	
			m_type = 1;
			reset();
			m_tname.GetComponent<UIInput>().value = "";
		}
		else if (obj.name == "siliao")
		{	
			m_type = 2;
			m_show_effect = false;
			reset();
		}
		else if (obj.name == "oppanel")
		{	
			m_oppanel.SetActive(false);
		}
		else if (obj.name == "chat(Clone)")
		{	
			if (obj.GetComponent<chat>().m_scs.player_guid == sys._instance.m_self.m_t_player.guid)
			{	
				return;
			}
			m_select = obj;
            protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look();
            _msg.target_guid = m_select.GetComponent<chat>().m_scs.player_guid;
            net_http._instance.send_msg<protocol.game.cmsg_player_look>(opclient_t.CMSG_PLAYER_LOOK, _msg);
		}
		else if (obj.name == "look")
		{	
			m_oppanel.SetActive(false);
			protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look ();
			_msg.target_guid = m_select.GetComponent<chat>().m_scs.player_guid;
			net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);
		}
		else if (obj.name == "sl")
		{	
			m_oppanel.SetActive(false);
			m_tname.GetComponent<UIInput>().value = m_select.GetComponent<chat>().m_scs.player_name;
			m_siliao.GetComponent<UIToggle>().value = true;
			m_type = 2;
			reset ();
		}
		else if (obj.name == "yq")
		{	
			m_oppanel.SetActive(false);
			protocol.game.cmsg_social_add _msg = new protocol.game.cmsg_social_add ();
			_msg.player_guid = m_select.GetComponent<chat>().m_scs.player_guid;
			net_http._instance.send_msg<protocol.game.cmsg_social_add> (opclient_t.CMSG_SOCIAL_ADD, _msg);
		}
		else if (obj.name == "color")
		{	
			m_chcolor.SetActive(true);
		}
		else if (obj.name == "chclose")
		{	
			m_chcolor.GetComponent<ui_bloom_anim>().hide_ui();
		}
		else if (obj.name.Substring(0, 6) == "ccolor")
		{	
			m_chcolor.GetComponent<ui_bloom_anim>().hide_ui();
			m_index = int.Parse(obj.name.Substring(6, 1)) - 1;
			m_color.GetComponent<UISprite>().color = m_colors1[m_index];
		}
	}

	void IMessage.message (s_message message)
	{	

	}
	
	void IMessage.net_message (s_net_message message)
	{	
		if (message.m_opcode == opclient_t.SMSG_CHAT)
		{	
			protocol.game.smsg_chat _msg = net_http._instance.parse_packet<protocol.game.smsg_chat> (message.m_byte);
            if (_msg.type == 1)
            {	
                sys._instance.m_chat_msgs1.Add(_msg);
            }
            else
            {
                sys._instance.m_chat_msgs.Add(_msg);
            }
			m_chat_list[_msg.type].Add(_msg);
			if (m_chat_list[_msg.type].Count > 30)
			{	
				m_chat_list[_msg.type].RemoveAt(0);
			}
			if (m_type == _msg.type)
			{	
				reset();
			}
			else if (m_type != 2 && _msg.type == 2)
			{	
				m_show_effect = true;
				reset_button();
			}

			if (_msg.type == 1)
			{	
				s_message _message = new s_message();
				_message.m_type = "juntuan_chat";
				cmessage_center._instance.add_message(_message);
			}
		}
	}

	void do_danmu()
	{	
		if (m_bdanmu)
		{	
			m_danmu.transform.Find("open").gameObject.SetActive(true);
			m_danmu.transform.Find("close").gameObject.SetActive(false);
		}
		else
		{
			m_danmu.transform.Find("open").gameObject.SetActive(false);
			m_danmu.transform.Find("close").gameObject.SetActive(true);
		}
	}

	// Update is called once per frame
	void Update () {

	}

}
