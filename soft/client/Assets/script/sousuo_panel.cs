
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sousuo_panel : MonoBehaviour,IMessage {

	public GameObject m_view;
	public GameObject m_no;
	List<GameObject> m_sousuos = new List<GameObject>();
	public bool m_flag = false;
	public UILabel m_meisoudao;
	public UILabel m_huanyihuan;
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
		m_sousuos = new List<GameObject>();

		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SOCIAL_RAND, _msg);
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_SOCIAL_RAND)
		{
			m_view.transform.localPosition = new Vector3(0, 0, 0);
			m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);

			int num = 0;
			protocol.game.smsg_social_rand _msg = net_http._instance.parse_packet<protocol.game.smsg_social_rand> (message.m_byte);
			for (int i = 0; i < _msg.social_player.Count; ++i)
			{
				GameObject target = game_data._instance.ins_object_res("ui/sousuo");
				target.transform.parent = m_view.transform;
				target.transform.localPosition = new Vector3(0, 125 - i * 129,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.GetComponent<sousuo>().m_social_player = _msg.social_player[i];
				target.GetComponent<sousuo>().reset ();
				num++;
				m_sousuos.Add(target);
			}
			if (num > 0)
			{
				m_no.SetActive(false);
			}
			else
			{
				m_no.SetActive(true);
			}
		}
		else if (message.m_opcode == opclient_t.CMSG_SOCIAL_ADD)
		{
			protocol.game.smsg_social_add _msg = net_http._instance.parse_packet<protocol.game.smsg_social_add> (message.m_byte);
			ulong player_guid = _msg.player_guid;
			for (int i = 0; i < m_sousuos.Count; ++i)
			{
				if (m_sousuos[i].GetComponent<sousuo>().m_social_player.player_guid == player_guid)
				{
					m_sousuos[i].GetComponent<sousuo>().is_add = true;
					m_sousuos[i].GetComponent<sousuo>().reset();
					break;
				}
			}
		}
	}

	void IMessage.message(s_message message)
	{
	}

	public void click(GameObject obj)
	{
		if (obj.name == "huan")
		{
			m_flag = false;
			reset();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
