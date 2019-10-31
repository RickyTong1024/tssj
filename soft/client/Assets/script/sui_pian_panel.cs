
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sui_pian_panel : MonoBehaviour,IMessage {

	public GameObject m_card_page_gui;
	private int m_item_id = 0;
	
	public GameObject m_par;
	// Use this for initialization
	void Start () {

		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	void OnFinished()
	{

	}

	void dui_huan()
	{
		s_t_item _item = game_data._instance.get_item (m_item_id);
		sys._instance.m_self.remove_item ((uint)m_item_id, _item.def_2,game_data._instance.get_t_language ("sui_pian_panel.cs_30_65"));//碎片兑换消耗
		m_par.transform.GetComponent<partner_gui>().update_button ();
		update_ui ();
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_ROLE_DUIHUAN)
		{
			protocol.game.smsg_role_duihuan _msg = net_http._instance.parse_packet<protocol.game.smsg_role_duihuan> (message.m_byte);

			if(sys._instance.m_self.has_card(_msg.role.template_id))
			{
				return;
			}

			dui_huan();
			sys._instance.m_self.add_card(_msg.role, false);
			
			update_ui();
			
			s_message _mes2 = new s_message();
			_mes2.m_type = "hide_huoban";
			cmessage_center._instance.add_message(_mes2);
			
			s_message _mes1 = new s_message();
			_mes1.m_type = "show_huoban2";
			
			ccard _card = sys._instance.m_self.get_card_guid(_msg.role.guid);
			s_message _message = new s_message();
			_message.m_type = "show_zhaomu_shuxing";
			_message.m_object.Add(_card);
			_message.m_object.Add(_mes1);
			cmessage_center._instance.add_message(_message);
		}

		/*
		if(message.m_opcode == opclient_t.CMSG_ROLE_SUIPIAN)
		{
			he_cheng();
			update_ui();
		}
		*/
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "sui_pian_he_cheng")
		{
			m_item_id = (int)message.m_ints[0];
			
			protocol.game.cmsg_role_suipian _msg = new protocol.game.cmsg_role_suipian();
			_msg.item_id = (uint)m_item_id;
			net_http._instance.send_msg<protocol.game.cmsg_role_suipian> (opclient_t.CMSG_ROLE_DUIHUAN, _msg);
		}
	}
	void he_cheng()
	{
		s_t_item _item = game_data._instance.get_item (m_item_id);
		sys._instance.m_self.add_item ((uint)_item.def_4,1,game_data._instance.get_t_language ("sui_pian_panel.cs_88_53"));//碎片合成
		sys._instance.m_self.remove_item ((uint)m_item_id,_item.def_2,game_data._instance.get_t_language ("sui_pian_panel.cs_89_64"));//碎片合成消耗
		m_par.transform.GetComponent<partner_gui>().update_button ();
		update_ui ();
	}
	public void update_ui()
	{
		if(sys._instance.m_self == null)
		{
			return ;
		}

		m_card_page_gui.GetComponent<card_page_gui>().init();
		m_card_page_gui.GetComponent<card_page_gui>().sp_reset();
	}

	public static bool can_hecheng()
	{
		for(int i = 0;i < sys._instance.m_self.m_t_player.item_ids.Count;i ++)
		{
			if(sys._instance.m_self.is_card_fragment(sys._instance.m_self.m_t_player.item_ids[i]))
			{
				uint _item_id = sys._instance.m_self.m_t_player.item_ids[i];
				s_t_item _t_item = game_data._instance.get_item ((int)_item_id);
				int _num = sys._instance.m_self.get_item_num (_item_id);
				if(_num >= _t_item.def_2)
				{
					return true;
				}
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
