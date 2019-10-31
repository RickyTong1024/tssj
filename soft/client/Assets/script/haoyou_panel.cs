
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class haoyou_panel : MonoBehaviour,IMessage {

	public GameObject m_view;
	public GameObject m_num;
	public GameObject m_no;
	public protocol.game.smsg_social_look m_msg;
	List<GameObject> m_haoyous = new List<GameObject>();
	public bool m_flag = false;

	public UILabel m_weitianjia;
	// Use this for initialization
	public void add_handle () 
	{
        cmessage_center._instance.add_handle(this);
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
		m_num.GetComponent<UILabel>().text = "0/40";

		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SOCIAL_LOOK, _msg);
	}

	void IMessage.net_message(s_net_message message)
	{
        if (message.m_opcode == opclient_t.CMSG_SOCIAL_LOOK)
		{
			m_msg = net_http._instance.parse_packet<protocol.game.smsg_social_look> (message.m_byte);
            friend_gui._insatnce.m_msg = m_msg;
            friend_gui._insatnce.update_button();
			update_ui();
		}
		else if (message.m_opcode == opclient_t.CMSG_SOCIAL_DELETE)
		{
			protocol.game.smsg_social_delete _msg = net_http._instance.parse_packet<protocol.game.smsg_social_delete> (message.m_byte);
			for (int i = 0; i < m_msg.social.Count; ++i)
			{
				if (m_msg.social[i].guid == _msg.social_guid)
				{
					m_msg.social.RemoveAt(i);
				}
			}
			update_ui();
		}
		else if (message.m_opcode == opclient_t.CMSG_SOCIAL_SONG)
		{
			protocol.game.smsg_social_song _msg = net_http._instance.parse_packet<protocol.game.smsg_social_song> (message.m_byte);
			for (int i = 0; i < m_haoyous.Count; ++i)
			{
				if (m_haoyous[i].GetComponent<haoyou>().m_social.guid == _msg.social_guid)
				{
					m_haoyous[i].GetComponent<haoyou>().m_social.last_song_time = timer.now();
					m_haoyous[i].GetComponent<haoyou>().reset ();
					break;
				}
			}
			update_ui();
			sys._instance.m_self.add_active(2100, 1);
		}
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "delete_haoyou")
		{
			ulong guid = (ulong)message.m_long[0];
			protocol.game.cmsg_social_delete _msg = new protocol.game.cmsg_social_delete ();
			_msg.social_guid = guid;
			net_http._instance.send_msg<protocol.game.cmsg_social_delete> (opclient_t.CMSG_SOCIAL_DELETE, _msg);
		}
	}

	void update_ui()
	{
		sys._instance.remove_child (m_view);
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		m_haoyous = new List<GameObject>();
		List<dhc.social_t> m_socials = new List<dhc.social_t>();
		for (int i = 0; i < m_msg.social.Count; ++i)
		{
            if (m_msg.social[i].template_id != -1)
            {
                m_socials.Add(m_msg.social[i]);
 
            }
		}
		m_socials.Sort (comp);
		int tnum = 0;
		for (int i = 0; i < m_socials.Count; ++i)
		{
			GameObject target = game_data._instance.ins_object_res("ui/haoyou");
			target.transform.parent = m_view.transform;
			target.transform.localPosition = new Vector3(0, 142 - i * 129,0);
			target.transform.localScale = new Vector3(1,1,1);
			target.GetComponent<haoyou>().m_social = m_socials[i];
			target.GetComponent<haoyou>().reset ();
			tnum++;
			m_haoyous.Add(target);
		}
		if (tnum > 0)
		{
			m_no.SetActive(false);
		}
		else
		{
			m_no.SetActive(true);
		}
		m_num.GetComponent<UILabel>().text = tnum.ToString() + "/40";
	}

	public int comp(dhc.social_t m_social1 ,dhc.social_t m_social2)
	{
		if(timer.trigger_time(m_social1.last_song_time, 0, 0) && !timer.trigger_time(m_social2.last_song_time, 0, 0))
		{
			return -1;
		}
		else if(!timer.trigger_time(m_social1.last_song_time, 0, 0) && timer.trigger_time(m_social2.last_song_time, 0, 0))
		{
			return 1;
		}
		return 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
