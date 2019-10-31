
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class xinzeng_panel : MonoBehaviour,IMessage {

	public GameObject m_view;
	public GameObject m_haoyou_panel;
	public GameObject m_friend_gui;
	public GameObject m_no;
	public bool m_flag = false;
	protocol.game.smsg_social_look_new m_msg;

	public UILabel m_xizeng;

	// Use this for initialization
	void Start () 
	{

		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void reset()
	{
		if (m_flag)
		{
			return;
		}
		m_flag = true;
		sys._instance.remove_child (m_view);

		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SOCIAL_LOOK_NEW, _msg);
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_SOCIAL_LOOK_NEW)
		{
			m_msg = net_http._instance.parse_packet<protocol.game.smsg_social_look_new> (message.m_byte);
			update_ui();
		}
		else if (message.m_opcode == opclient_t.CMSG_SOCIAL_AGREE)
		{
			protocol.game.smsg_social_agree _msg = net_http._instance.parse_packet<protocol.game.smsg_social_agree> (message.m_byte);
			ulong player_guid = _msg.player_guid;
			for (int i = 0; i < m_msg.social_player.Count; ++i)
			{
				if (m_msg.social_player[i].player_guid == player_guid)
				{
					m_msg.social_player[i].player_guid = 0;
					update_ui();
					break;
				}
			}
			if (_msg.agree == 1)
			{
				m_haoyou_panel.GetComponent<haoyou_panel>().m_flag = false;
			}
		}
	}

	void update_ui()
	{
		sys._instance.remove_child (m_view);
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);

		int num = 0;
		for (int i = 0; i < m_msg.social_player.Count; ++i)
		{
			if (m_msg.social_player[i].player_guid == 0)
			{
				continue;
			}
			GameObject target = game_data._instance.ins_object_res("ui/xinzeng");
			target.transform.parent = m_view.transform;
			target.transform.localPosition = new Vector3(0, 156 - num * 129,0);
			target.transform.localScale = new Vector3(1,1,1);
			target.GetComponent<xinzeng>().m_social_player = m_msg.social_player[i];
			target.GetComponent<xinzeng>().reset ();
			num++;
		}
		if (num > 0)
		{
			m_no.SetActive(false);
		}
		else
		{
			if (sys._instance.m_self.is_friend_apply == 1)
			{
				sys._instance.m_self.is_friend_apply = 0;
				m_friend_gui.GetComponent<friend_gui>().update_button();
			}
			m_no.SetActive(true);
		}
	}

	void IMessage.message(s_message message)
	{
	}

	// Update is called once per frame
	void Update () {
	
	}
}
