
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class wuping_gui : MonoBehaviour ,IMessage{

	public GameObject m_icon;
	public GameObject m_hall_gui;
	public GameObject m_name;
	public int m_item_id;

	public UILabel m_use_Label;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);

	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void reset()
	{
		GameObject _icon = icon_manager._instance.create_item_icon(m_item_id, 1);

		sys._instance.remove_child (m_icon);
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);

		s_t_item _item = game_data._instance.get_item (m_item_id);
		m_name.GetComponent<UILabel>().text = _item.name;
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "tuse")
		{
			protocol.game.cmsg_item_apply _msg = new protocol.game.cmsg_item_apply ();
			_msg.item_id = (uint)m_item_id;
			_msg.item_count = 1;
			net_http._instance.send_msg<protocol.game.cmsg_item_apply> (opclient_t.CMSG_ITEM_QUICK_APPLY, _msg);
		}
	}

	void IMessage.message(s_message message)
	{
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_ITEM_QUICK_APPLY)
		{
			s_t_item _item = game_data._instance.get_item (m_item_id);
			protocol.game.smsg_item_apply _msg = net_http._instance.parse_packet<protocol.game.smsg_item_apply> (message.m_byte);
			for(int c = 0;c < _msg.types.Count;c ++)
			{
				sys._instance.m_self.add_reward(_msg.types[c], _msg.value1s[c], _msg.value2s[c], _msg.value3s[c],_item.name + game_data._instance.get_t_language ("wuping_gui.cs_60_114"));//物品开启
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			for (int i = 0; i < _msg.roles.Count; ++i)
			{
				sys._instance.m_self.add_card(_msg.roles[i], false);
			}
			sys._instance.m_self.remove_item((uint)m_item_id, 1,game_data._instance.get_t_language ("wuping_gui.cs_70_55"));//物品开启消耗
			if (_msg.role_ids.Count == 1)
			{
				int sid = _msg.role_ids[0];
				bool flag = false;
				if (_msg.roles.Count == 0)
				{
                    sys._instance.m_self.add_item((uint)ccard.get_fragment_id(sid), 10, false,_item.name + game_data._instance.get_t_language ("wuping_gui.cs_60_114"));//物品开启
					flag = true;
				}
				
				s_message _mes2 = new s_message();
				_mes2.m_type = "hide_hall";
				cmessage_center._instance.add_message(_mes2);
				
				s_message _mes1 = new s_message();
				_mes1.m_type = "show_hall";
				
				ccard _card = sys._instance.m_self.get_card_id(sid);
				s_message _message = new s_message();
				_message.m_type = "show_zhaomu_shuxing";
				_message.m_object.Add(_card);
				_message.m_object.Add(_mes1);
				_message.m_bools.Add(flag);
				cmessage_center._instance.add_message(_message);
			}

			this.GetComponent<ui_show_anim>().hide_ui();
			this.gameObject.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {

	}
}
