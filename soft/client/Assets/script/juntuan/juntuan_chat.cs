using UnityEngine;

public class juntuan_chat : MonoBehaviour , IMessage{

	public GameObject m_scro;
	public GameObject m_text;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		reset ();
	}

	public void reset()
	{
		sys._instance.remove_child (m_scro);
		if (chat_gui._instance.m_chat_list == null)
		{
			return;
		}
		for (int i = 0; i < chat_gui._instance.m_chat_list[1].Count; ++i)
		{
			GameObject chat = game_data._instance.ins_object_res("ui/juntuan/juntuan_chat");
			
			protocol.game.smsg_chat scs =  chat_gui._instance.m_chat_list[1][i];
			chat.transform.parent = m_scro.transform;
			chat.transform.localPosition = new Vector3(10, 169 - 95 * i, 0);
			chat.transform.localScale = new Vector3(1,1,1);
			chat.transform.GetComponent<juntuan_chat_date>().m_scs = scs;
			chat.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			chat.transform.GetComponent<juntuan_chat_date>().init();
			if(i % 2 == 0)
			{
				chat.transform.Find("main").GetComponent<UISprite>().spriteName = "jtlt_xdk001";
			}
			else
			{
				chat.transform.Find("main").GetComponent<UISprite>().spriteName = "jtlt_xdk002";
			}
			int y = 95 * i - 315;
			if (y < 0)
			{
				y = 0;
			}
			m_scro.transform.localPosition = new Vector3(0, y, 0);
			m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, -y);
		}
	}

	void click(GameObject obj)
	{
		if(obj.transform.name == "send")
		{
			string text = m_text.GetComponent<UILabel>().text;
			m_text.GetComponent<UILabel>().text = "";
			m_text.GetComponent<UIInput>().value = "";

			if (text == "")
			{
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
				protocol.game.cmsg_chat_ex _msg = new protocol.game.cmsg_chat_ex ();
				_msg.type = 1;
				_msg.text = text;
				_msg.color = "[ffc882]";
				_msg.target_name = "";
				net_http._instance.send_msg_ex<protocol.game.cmsg_chat_ex> (opclient_t.CMSG_CHAT, _msg);
			}
		}

		else if (obj.name == "close")
		{
			this.gameObject.SetActive(false);
			//guild_boss._instance.m_liao_tian.SetActive(true);
		}

	}

	void IMessage.message (s_message message)
	{
		if (message.m_type == "juntuan_chat" && this.gameObject.activeSelf)
		{
			reset();
		}
	}

	void IMessage.net_message (s_net_message message)
	{

	}

	// Update is called once per frame
	void Update () {
	
	}
}
