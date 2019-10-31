
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_xq_gui : MonoBehaviour, IMessage {

	public List<GameObject> m_attrs;
	public List<GameObject> m_icons;
	public GameObject m_icon1;
	public GameObject m_contain;
	public GameObject m_sm;
	private int m_select;
	private dhc.equip_t m_equip;
	private string m_out_message;
	private int m_id;


	public UILabel m_xiangqian;
	public UILabel m_baoshixiangqian;
	public UILabel m_shengming;
	public UILabel m_gongji;
	public UILabel m_wufang;
	public UILabel m_mofang;
	public UILabel m_xiangong;
	public UILabel m_shengming1;
	public UILabel m_gongji1;
	public UILabel m_wufang1;
	public UILabel m_mofang1;
	public UILabel m_xiangong1;
	public UILabel m_desc;
	public UILabel m_shandianhuode;
	public UILabel m_lijiqianwang;



	// Use this for initialization
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void reset(dhc.equip_t eequip, string out_message)
	{
		m_equip = eequip;
		m_out_message = out_message;
		m_select = 1;
		m_sm.GetComponent<UIToggle>().value = true;
		reset_equip ();
		reset_panel (m_select);
	}

	void reset_equip()
	{
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		sys._instance.remove_child (m_icon1);
		for (int i = 0; i < m_icons.Count; ++i)
		{
			sys._instance.remove_child (m_icons[i]);
			if (i < t_equip.slot_num)
			{
				m_icons[i].GetComponent<UISprite>().spriteName = "dj_frame";
			}
			else
			{
				m_icons[i].GetComponent<UISprite>().spriteName = "tb_suo001";
			}
		}

		GameObject icon1 = icon_manager._instance.create_equip_icon(m_equip.guid);
		icon1.transform.parent = m_icon1.transform;
		icon1.transform.localPosition = new Vector3(0, 0, 0);
		icon1.transform.localScale = new Vector3(1,1,1);

		for (int i = 0; i < m_equip.stone.Count; ++i)
		{
			if (m_equip.stone[i] > 0)
			{
				s_t_item t_item = game_data._instance.get_item(m_equip.stone[i]);
				string value = game_data._instance.get_value_string(t_item.def_1, t_item.def_2);
				string s = equip.get_baoshi_color(t_item.def_1);
				m_attrs[i].GetComponent<UILabel>().text = s + value;
				GameObject icon2 = icon_manager._instance.create_item_icon(m_equip.stone[i]);
				icon2.transform.parent = m_icons[i].transform;
				icon2.transform.localPosition = new Vector3(0, 0, 0);
				icon2.transform.localScale = new Vector3(1,1,1);
				icon2.GetComponent<item_icon>().m_out_message = "baoshi_xx_click";
			}
			else
			{
				m_attrs[i].GetComponent<UILabel>().text = "";
			}
		}
		for (int i = m_equip.stone.Count; i < 3; ++i)
		{
			m_attrs[i].GetComponent<UILabel>().text = "";
		}
	}

	void reset_panel(int type)
	{
		sys._instance.remove_child (m_contain);
		List<int> ids = new List<int>();
		for (int i = 0 ; i < sys._instance.m_self.m_t_player.item_ids.Count; ++i)
		{
			int id = (int)sys._instance.m_self.m_t_player.item_ids[i];
			s_t_item t_item = game_data._instance.get_item(id);
			if (t_item.type == 9001 && t_item.def_1 == type)
			{
				ids.Add(id);
			}
		}
		ids.Sort ();
		for (int i = 0; i < ids.Count; ++i)
		{
			s_t_item t_item = game_data._instance.get_item(ids[i]);
			int num = sys._instance.m_self.get_item_num((uint)t_item.id);
			GameObject icon1 = icon_manager._instance.create_item_icon(ids[i], num);
			icon1.transform.parent = m_contain.transform;
			icon1.transform.localPosition = new Vector3(i % 5 * 90 - 180, 75 - i / 5 * 90,0);
			icon1.transform.localScale = new Vector3(1,1,1);
			icon1.GetComponent<item_icon>().m_out_message = "baoshi_xq_click";
			icon1.GetComponent<item_icon>().flag = true;
		}
	}

	void IMessage.net_message(s_net_message message)
	{

	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "baoshi_xq_click")
		{
			m_id = (int)message.m_ints[0];
			s_t_item t_item = game_data._instance.get_item (m_id);
			int index1 = -1;
			for (int i = 0; i < m_equip.stone.Count; ++i)
			{
				int sid = m_equip.stone[i];
				if (sid == 0)
				{
					index1 = i;
					continue;
				}
				s_t_item t_item1 = game_data._instance.get_item (sid);
				if (t_item1.def_1 == t_item.def_1)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_xq_gui.cs_156_60"));//同种类型的宝石只能镶嵌一个
					return;
				}
			}
			if (index1 == -1)
			{
				return;
			}
			if (t_item.need_level > m_equip.enhance)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_xq_gui.cs_166_59"));//装备强化等级不足
				return;
			}

			/*protocol.game.cmsg_equip_stone_xq _msg = new protocol.game.cmsg_equip_stone_xq ();
			_msg.equip_guid = m_equip.guid;
			_msg.stone_id = m_id;
			net_http._instance.send_msg<protocol.game.cmsg_equip_stone_xq> (opclient_t.CMSG_EQUIP_STONE_XQ, _msg);*/
		}
		else if (message.m_type == "baoshi_xx_click")
		{
			m_id = (int)message.m_ints[0];

			/*protocol.game.cmsg_equip_stone_xq _msg = new protocol.game.cmsg_equip_stone_xq ();
			_msg.equip_guid = m_equip.guid;
			_msg.stone_id = m_id;
			net_http._instance.send_msg<protocol.game.cmsg_equip_stone_xq> (opclient_t.CMSG_EQUIP_STONE_XX, _msg);*/
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			m_sm.GetComponent<UIToggle>().value = true;
			this.transform.GetComponent<ui_show_anim>().hide_ui();
			
			s_message _msg = new s_message();
			_msg.m_type = m_out_message;
			_msg.m_long.Add(m_equip.guid);
			cmessage_center._instance.add_message(_msg);
		}
		if (obj.name == "t1")
		{
			m_select = 1;
			reset_panel(m_select);
		}
		else if (obj.name == "t2")
		{
			m_select = 2;
			reset_panel(m_select);
		}
		else if (obj.name == "t3")
		{
			m_select = 3;
			reset_panel(m_select);
		}
		else if (obj.name == "t4")
		{
			m_select = 4;
			reset_panel(m_select);
		}
		else if (obj.name == "t5")
		{
			m_select = 5;
			reset_panel(m_select);
		}
		else if (obj.transform.name == "qianwang")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shop)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_263_74"), (int)e_open_level.el_shop));//商店{0}级开启
				return;
			}

			m_sm.GetComponent<UIToggle>().value = true;
			transform.GetComponent<ui_show_anim>().hide_ui();

			s_message _message = new s_message();
			_message.m_type = "show_baoshi_shop";
			cmessage_center._instance.add_message(_message);

			s_message _mes = new s_message();
			_mes.m_type = "hall_anim";
			_mes.m_string.Add("0");
			cmessage_center._instance.add_message(_mes);
			
			s_message _message1 = new s_message();
			_message1.m_type = "hide_show_unit";
			cmessage_center._instance.add_message (_message1);
		}
	}
}
