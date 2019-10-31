
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chat_gui2 : MonoBehaviour , IMessage{

	public GameObject m_panel;
	public List<List<protocol.game.smsg_chat>> m_chat_list;
	private List<string> m_color = new List<string>();
	public GameObject m_view_1;

	// Use this for initialization
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
		show_chat2 ();
	}

	string  insert(string s)
	{

		if(s.Length > 26)
		{
			s =  s.Insert(26,"\n");
		}

		if(s.Length > 45)
		{
			s =  s.Insert(45,"\n");
		}
		if(s.Length > 64)
		{
			s =  s.Insert(64,"\n");
		}
		return s;
	}
	void IMessage.message (s_message message)
	{
		if(message.m_type == "fresh_chat")
		{
			show_chat2();
		}
		
	}
	
	void IMessage.net_message (s_net_message message)
	{
	}
	void show_chat2()
	{
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
		}
		for (int i = 0; i < sys._instance.m_chat_msgs1.Count; ++i)
		{
			m_chat_list[1].Add(sys._instance.m_chat_msgs1[i]);
		}
		sys._instance.remove_child (m_view_1);
		m_view_1.transform.parent.localPosition = Vector3.zero;
		m_view_1.transform.parent.GetComponent<UIPanel>().clipOffset = Vector2.zero;
		int y = -149;
		if(m_chat_list[0].Count > 0)
		{

            for (int i = m_chat_list[0].Count - 1; i >= 0; i--)
			{
                protocol.game.smsg_chat scs = m_chat_list[0][i];
				GameObject chat = game_data._instance.ins_object_res("ui/sjijie_c");
				chat.transform.parent = m_view_1.transform;
				chat.transform.localScale = Vector3.one;

                string s2 = "           [00ffff]" + scs.player_name + get_color() +  "：" + scs.text;
                chat.GetComponent<UILabel>().text = s2;

				chat.transform.localPosition = new Vector3(-150,y + chat.GetComponent<UILabel>().height + 5);
				y += chat.GetComponent<UILabel>().height + 5;
			}

		}

	}
	string get_color()
	{
		if(m_color.Count == 0)
		{
			m_color.Add ("[ff0000]");
			m_color.Add ("[ffff00]");
			m_color.Add ("[ff00ff]");
			m_color.Add ("[00ffff]");
			m_color.Add ("[00ff00]");
		}

		return m_color[Random.Range(0,m_color.Count)];
	}
}
