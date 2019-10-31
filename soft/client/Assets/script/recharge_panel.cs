using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class recharge_panel : MonoBehaviour, IMessage {

	public GameObject m_view;
	public GameObject m_recharge_gui;
	int m_click_id = 0;
	public static bool m_is_wait = false;
	string order = "";

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	public static bool recharge_vis(s_t_recharge t_recharge)
	{
        for (int j = 0; j < sys._instance.m_self.m_t_player.recharge_ids.Count; ++j)
		{
			if (sys._instance.m_self.m_t_player.recharge_ids[j] == t_recharge.id)
			{
				return false;
			}
		}
		if (t_recharge.type == 3)
		{
			for (int j = 0; j < sys._instance.m_self.m_t_player.recharge_ids.Count; ++j)
			{
				if (sys._instance.m_self.m_t_player.recharge_ids[j] == t_recharge.id)
				{
					return false;
				}
			}
		}
		if (t_recharge.type == 4)
		{
			bool flag = false;
			for (int j = 0; j < sys._instance.m_self.m_t_player.recharge_ids.Count; ++j)
			{
				if (sys._instance.m_self.m_t_player.recharge_ids[j] == t_recharge.pid)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return false;
			}
		}
		if (t_recharge.type == 1)
		{
			if (sys._instance.m_self.m_t_player.zhouka_time > timer.now())
			{
				return false;
			}
		}
		if (t_recharge.type == 2)
		{
			if (sys._instance.m_self.m_t_player.yueka_time > timer.now())
			{
				return false;
			}
		}
		return true;
	}

	public void reset()
	{
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		dbc m_dbc_recharge = game_data._instance.m_dbc_recharge;
		int tnum = 0;
		for (int i = 0; i < m_dbc_recharge.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_recharge.get(0, i));
			s_t_recharge t_recharge = game_data._instance.get_t_recharge(id);
			if (!recharge_panel.recharge_vis(t_recharge))
			{
				continue;
			}

			GameObject recharge = game_data._instance.ins_object_res("ui/recharge");
			recharge.transform.parent = m_view.transform;
			recharge.transform.localPosition = new Vector3(-211 + tnum % 2 * 422, 125 - tnum / 2 * 118,0);
			recharge.transform.localScale = new Vector3(1,1,1);
			recharge.transform.GetComponent<recharge>().m_recharge_id = t_recharge.id;
			recharge.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			tnum++;
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "recharge(Clone)")
		{
			m_click_id = obj.transform.GetComponent<recharge>().m_recharge_id;
			s_t_recharge t_recharge1 = game_data._instance.get_t_recharge(m_click_id);
			if(t_recharge1 == null)
			{
				return;
			}
            platform_recharge._instance.do_buy(m_click_id);
        }
	}

	void IMessage.message (s_message message)
	{

	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_RECHARGE_CHECK_EX)
		{
            m_recharge_gui.GetComponent<recharge_gui>().reset(0);
		}
	}
}
