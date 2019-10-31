using System;
using UnityEngine;

public class juntuan_edit_jiarutiaojian : MonoBehaviour,IMessage {
	public UIInput  m_edit_input;


	public UILabel m_jiarutiaojian;
	public UILabel m_zuidizhanli;
	public UILabel m_queding;
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
		m_edit_input.label.text = juntuan_gui._instance.m_guild_t.bftj.ToString();
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.message (s_message message)
	{
		
	}
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GUILD_SET_JOIN_CONDITION) 
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if(_msg.success)
			{
				juntuan_gui._instance.m_guild_t.bftj = Int32.Parse(m_edit_input.label.text);
				this.gameObject.transform.Find("frame_big").GetComponent<frame>().hide();
				juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().refresh ("juntuan");
			}
		}
		
	}
	public void  ButtonClick(GameObject obj)
	{
		switch (obj.name) 
		{
		case "queding":
			protocol.game.cmsg_guild_set_join_condition _msg = new protocol.game.cmsg_guild_set_join_condition();
			try
			{
				_msg.min_bf = Int32.Parse(m_edit_input.label.text);
			}
			catch(FormatException)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("juntuan_edit_jiarutiaojian.cs_51_59"));//请输入数字
				return;
			}
			net_http._instance.send_msg<protocol.game.cmsg_guild_set_join_condition> (opclient_t.CMSG_GUILD_SET_JOIN_CONDITION, _msg);
			break;
		case "quxiao":
			this.gameObject.SetActive(false);
			break;
		case "add":
			m_edit_input.label.text = (Int32.Parse(m_edit_input.label.text) + 10000).ToString ();
			break;
		case "sub":
			if(Int32.Parse(m_edit_input.label.text) - 10000 < 0)
			{
				return;
			}
			m_edit_input.label.text = (Int32.Parse(m_edit_input.label.text) - 10000).ToString ();
			break;
		}

	}
	public void click(GameObject obj)
	{
		if(obj.transform.name == "hide")
		{
			transform.Find("frame_big").GetComponent<frame>().hide();
		}

	}
}
