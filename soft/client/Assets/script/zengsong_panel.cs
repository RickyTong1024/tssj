
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class zengsong_panel : MonoBehaviour,IMessage {

	public GameObject m_view;
	public GameObject m_haoyou_panel;
	public GameObject m_friend_gui;
	public GameObject m_ling;
	public GameObject m_num;
	public GameObject m_no;

	public UILabel m_yijianlingqu;
	public UILabel m_weishoudao;
	public UILabel m_jinrikeling;

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
		update_ui ();
	}

	public void click(GameObject obj)
	{
		if (obj.name == "ling")
		{
            if (sys._instance.m_self.m_t_player.social_shou_num >= 30)
			{
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("zengsong_panel.cs_40_71"));//[ffc882]每日只能领取30次
				return;
			}
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SOCIAL_SHOU, _msg);
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_SOCIAL_SHOU)
		{
			protocol.game.smsg_social_shou _msg = net_http._instance.parse_packet<protocol.game.smsg_social_shou> (message.m_byte);
			protocol.game.smsg_social_look m_msg = m_haoyou_panel.GetComponent<haoyou_panel>().m_msg;
			for (int i = 0; i < m_msg.social.Count; ++i)
			{
				for (int j = 0; j < _msg.social_guids.Count; ++j)
				{
					if (m_msg.social[i].guid == _msg.social_guids[j])
					{
						m_msg.social[i].can_shou = 0;
						break;
					}
				}
			}
            sys._instance.m_self.add_att(e_player_attr.player_friend_point,_msg.social_guids.Count);
			//sys._instance.m_self.add_att(e_player_attr.player_treasure_energy ,_msg.social_guids.Count);
			sys._instance.m_self.m_t_player.social_shou_num += _msg.social_guids.Count;

			update_ui();
		}
	}
	
	void IMessage.message(s_message message)
	{
	}

	void update_ui()
	{
		protocol.game.smsg_social_look _msg = m_haoyou_panel.GetComponent<haoyou_panel>().m_msg;
		sys._instance.remove_child (m_view);
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);

		int num = 0;
		for (int i = 0; i < _msg.social.Count; ++i)
		{
			if (_msg.social[i].player_guid == 0)
			{
				continue;
			}
			if (_msg.social[i].can_shou == 0)
			{
				continue;
			}
			GameObject target = game_data._instance.ins_object_res("ui/zengsong");
			target.transform.parent = m_view.transform;
			target.transform.localPosition = new Vector3(0, 126 - num * 129,0);
			target.transform.localScale = new Vector3(1,1,1);
			target.GetComponent<zengsong>().m_social = _msg.social[i];
			target.GetComponent<zengsong>().reset ();
			num++;
		}

		if (num > 0)
		{
			m_no.SetActive(false);
		}
		else
		{
			if (sys._instance.m_self.is_friend_tili == 1)
			{
				sys._instance.m_self.is_friend_tili = 0;
				m_friend_gui.GetComponent<friend_gui>().update_button();
			}
			m_no.SetActive(true);
		}

		int snum = 30 - sys._instance.m_self.m_t_player.social_shou_num;
		m_num.GetComponent<UILabel>().text = snum.ToString() + "/30";
		if (snum <= 0 || num <= 0)
		{
			m_ling.GetComponent<UISprite>().set_enable(false);
			m_ling.GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			m_ling.GetComponent<UISprite>().set_enable(true);
			m_ling.GetComponent<BoxCollider>().enabled = true;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
