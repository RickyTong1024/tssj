
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_ronglian_gui : MonoBehaviour,IMessage {

	public List<GameObject> m_icons = new List<GameObject>();
	List<s_t_item> items = new List<s_t_item>();
	public GameObject m_new_icon;
	public GameObject m_name;
	public GameObject m_attr;
	public GameObject m_ricon;
	public GameObject m_sp_icon;
	public GameObject m_item_icon;
	public GameObject m_hc_name;
	public GameObject m_select;
	public GameObject m_sp_show;
	public int m_id = 0;
	public ulong m_guid;
	private uint equip_power = 50120001;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	public void reset(int _id)
	{
		items.Clear ();
		for(int i = 0;i < game_data._instance.m_dbc_item.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_item.get (0,i));
			s_t_item t_item = game_data._instance.get_item(id);
			if(t_item.type == 7001 && t_item.font_color == 5)
			{
				items.Add(t_item);
			}
		}
		for(int i = 0 ;i < items.Count &&i < m_icons.Count;++i)
		{
			sys._instance.remove_child(m_icons[i]);
			GameObject _icon = icon_manager._instance.create_item_icon_ex(items[i].id,20);
			_icon.transform.name = i.ToString();
			_icon.transform.parent = m_icons[i].transform;
			_icon.transform.localScale = new Vector3 (1, 1, 1);
			_icon.transform.localPosition = new Vector3 (0, 0, 0);
			_icon.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = _icon.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_sp_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";
		}
		GameObject temp1 = Instantiate(m_select) as GameObject;
		temp1.transform.parent = m_icons [_id].transform;
		temp1.transform.localScale = Vector3.one;
		temp1.transform.localPosition = new Vector3 (0, 0, 0);
		temp1.SetActive (true);
		sys._instance.remove_child (m_new_icon);
		GameObject _icon1 = icon_manager._instance.create_equip_icon(items[_id].def_1);
		_icon1.transform.parent = m_new_icon.transform;
		_icon1.transform.localScale = new Vector3 (1, 1, 1);
		_icon1.transform.localPosition = new Vector3 (0, 0, 0);
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (2,items[_id].id,1,0);
		s_t_equip t_equip = game_data._instance.get_t_equip (items [_id].def_1);
		m_hc_name.GetComponent<UILabel>().text = game_data._instance.get_t_language ("baowu_ronglian_gui.cs_70_44") +t_equip.name;//可合成
		m_attr.GetComponent<UILabel>().text = equip.get_equip_value (t_equip.id,0,0);

		int item_num = sys._instance.m_self.get_item_num(equip_power);
		sys._instance.remove_child (m_item_icon);
		GameObject item_icon = icon_manager._instance.create_item_icon_ex((int)equip_power,item_num,40);
		item_icon.transform.parent = m_item_icon.transform;
		item_icon.transform.localScale = new Vector3 (1, 1, 1);
		item_icon.transform.localPosition = new Vector3 (0, 0, 0);
		UIButtonMessage[] _message = item_icon.transform.GetComponents<UIButtonMessage>();
		_message[0].target = this.gameObject;
		_message[0].functionName = "click_equip_power_icon";
		_message[1].target = null;
		_message[1].functionName = "";
		_message[2].target = null;
		_message[2].functionName = "";

		sys._instance.remove_child (m_sp_icon);
		GameObject _icon2 = icon_manager._instance.create_item_icon(items[_id].id,20);
		_icon2.transform.parent = m_sp_icon.transform;
		_icon2.transform.localScale = new Vector3 (1, 1, 1);
		_icon2.transform.localPosition = new Vector3 (0, 0, 0);
		sys._instance.remove_child(m_ricon);
		if(m_guid != 0)
		{
			GameObject _icon = icon_manager._instance.create_equip_icon(m_guid);
			_icon.transform.GetComponent<BoxCollider>().enabled = false;
			_icon.transform.parent = m_ricon.transform;
			_icon.transform.localPosition = new Vector3(0,0,0);
			_icon.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void click_equip_power_icon(GameObject obj)
	{
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)equip_power);
		cmessage_center._instance.add_message(message);
	}

	public void select_equip(GameObject obj)
	{
		if(m_guid == 0)
		{
			List<ulong > m_hide_guids = new List<ulong >();
			root_gui._instance.show_common_equip_panel (game_data._instance.get_t_language ("equip_ronglian_gui.cs_118_47"), false, false, 0, m_hide_guids, "common_ronglian_equip", false, 2,this.transform.parent.parent.gameObject);//请选择需要圣炼的装备
			this.transform.parent.parent.gameObject.SetActive(false);
		}
		else
		{
			sys._instance.remove_child(m_ricon);
			m_guid = 0;
		}
	}
	
	void IMessage.message(s_message message)
	{
		if(message.m_type == "common_ronglian1_equip")
		{
			m_guid = (ulong)message.m_long[0];
			reset (m_id);
		}
		
	}
	
	public void click_sp_icon(GameObject obj)
	{
		m_id = int.Parse (obj.transform.name);
		reset (m_id);
	}
	
	public void click(GameObject obj)
	{
		if(obj.transform.name == "sl")
		{
			if(m_guid == 0)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_ronglian_gui.cs_150_59"));//未选择未强化未精炼的橙色装备
				return;
			}
			int num = sys._instance.m_self.get_item_num (equip_power);
			if(num < 40)
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)equip_power);
				cmessage_center._instance.add_message(message);
				return;
			}
			if(m_guid != 0)
			{
				protocol.game.cmsg_equip_ronglian _msg = new protocol.game.cmsg_equip_ronglian ();
				_msg.suipian_id = items[m_id].id;
				_msg.equip_guid = m_guid;
				net_http._instance.send_msg<protocol.game.cmsg_equip_ronglian> (opclient_t.CMSG_EQUIP_RONGLIAN, _msg);
			}
		}
		else if(obj.transform.name == "auto")
		{
			if(m_guid != 0)
			{
				return;
			}
			auto_treasure();
			reset (m_id);
		}
		else if(obj.name == "close_sp")
		{
			m_sp_show.GetComponent<ui_show_anim>().hide_ui();
			reset(m_id);
		}
		else if(obj.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}
	
	void show_item()
	{
		m_sp_show.SetActive (true);
		
		UILabel _label = m_sp_show.transform.Find("info").GetComponent<UILabel>();
		GameObject _icon = m_sp_show.transform.Find("icon").gameObject;
		_label.text = sys._instance.get_res_info (2,items[m_id].id,0,0);
		sys._instance.remove_child (_icon);
		GameObject temp = icon_manager._instance.create_item_icon (items[m_id].id,20);
		temp.transform.parent = _icon.transform;
		temp.transform.localPosition = Vector3.zero;
		temp.transform.localScale = Vector3.one;
	}
	
	public void auto_treasure()
	{
		m_guid = 0;
		List<ulong> m_equips_guid = new List<ulong>();
		for(int i = 0; i < sys._instance.m_self.m_t_player.equips.Count;++i)
		{
			dhc.equip_t _equip = sys._instance.m_self.get_equip_guid(sys._instance.m_self.m_t_player.equips[i]);
			s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
			if(_equip.role_guid !=0 || _equip.locked == 1 || _equip.jilian != 0 || _equip.enhance != 0 || t_equip.font_color != 4)
			{
				continue;
			}
			m_equips_guid.Add(sys._instance.m_self.m_t_player.equips[i]);
		}
		if(m_equips_guid.Count <= 0)
		{
			root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("equip_ronglian_gui.cs_220_45"));//[ffc882]您没有闲置的未强化未精炼的橙色装备
			return;
		}
		m_equips_guid.Sort (comp);
		m_guid = m_equips_guid [0];
	}
	
	public int comp(ulong guid1, ulong guid2)
	{
		dhc.equip_t t_equip1 = sys._instance.m_self.get_equip_guid (guid1); 
		dhc.equip_t t_equip2 = sys._instance.m_self.get_equip_guid (guid2); 
		if(t_equip1.template_id < t_equip2.template_id)
		{
			return -1;
		}
		else if(t_equip2.template_id < t_equip1.template_id)
		{
			return 1;
		}
		return 0;
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_EQUIP_RONGLIAN)
		{
			sys._instance.m_self.remove_item(equip_power,40,game_data._instance.get_t_language ("equip_ronglian_gui.cs_246_51"));//装备熔炼消耗
			sys._instance.m_self.remove_equip(m_guid);
			sys._instance.m_self.add_item((uint)items[m_id].id,20,false,game_data._instance.get_t_language ("equip_ronglian_gui.cs_248_63"));//装备熔炼获得
			sys._instance.remove_child(m_ricon);
			m_guid = 0;
			show_item();
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
