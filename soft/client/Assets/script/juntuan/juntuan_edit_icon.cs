using System;
using UnityEngine;

public class juntuan_edit_icon : MonoBehaviour ,IMessage {
	
	public GameObject m_scrollview;
	bool m_b = false;
    int icon;


	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		if (m_b)
		{
			return;
		}

		m_b = true;
		m_scrollview.transform.localPosition = new Vector3(0, 0, 0);
		m_scrollview.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_scrollview);
		int j = -1;
		for(int i = 0;i < game_data._instance.m_dbc_guild_icon.get_y ();i++)
		{
			if (i % 5 == 0)
			{
				j++;
			}
			GameObject target = game_data._instance.ins_object_res ("ui/juntuan/icon_juntuan");
			target.transform.parent = m_scrollview.transform;
			target.transform.localPosition = new Vector3(-230f + i % 5 * 115f,77f - j * 115f ,0);
			target.GetComponent<UIDragScrollView>().scrollView = m_scrollview.GetComponent<UIScrollView>();
			target.GetComponent<UISprite>().spriteName = game_data._instance.m_dbc_guild_icon.get (1,i);
			target.name = game_data._instance.m_dbc_guild_icon.get(0,i);
			target.GetComponent<UIButtonMessage>().target = this.gameObject;
			target.transform.localScale = Vector3.one;
		}
	}

	void click(GameObject obj)
	{
		if(obj.name == "Sprite" || obj.name == "juntuan_edit_icon" )
		{
			return;
		}
		try
		{
			protocol.game.cmsg_guild_modify_icon _msg = new protocol.game.cmsg_guild_modify_icon();
			_msg.icon = int.Parse (obj.name);
            icon = _msg.icon;
			net_http._instance.send_msg<protocol.game.cmsg_guild_modify_icon> (opclient_t.CMSG_GUILD_MODIFY_ICON, _msg);
		}
		catch(FormatException)
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();	
		}
	}

	void IMessage.message (s_message message)
	{
		
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GUILD_MODIFY_ICON) 
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if(_msg.success)
			{
                juntuan_gui._instance.m_guild_t.icon = icon;
				this.transform.Find("frame_big").GetComponent<frame>().hide ();
				juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().refresh ("juntuan");
			}
			else
			{
                this.transform.Find("frame_big").GetComponent<frame>().hide();
				root_gui._instance.show_prompt_dialog_box ("[ffc882]" + game_data._instance.get_t_language ("juntuan_edit_icon.cs_88_60"));//修改失败
			}
		}
	}
}
