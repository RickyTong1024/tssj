
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ji_yin_panel : MonoBehaviour,IMessage {

	public GameObject m_card_page_gui;
	private int m_item_id = 0;
	List<uint> _ids;
	List<uint> _ids2;
	
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


	void IMessage.net_message(s_net_message message)
	{

		if (message.m_opcode == opclient_t.CMSG_ITEM_SELL && this.gameObject.activeSelf)
		{
			s_t_item _item = game_data._instance.get_item (m_item_id);
			int _num = sys._instance.m_self.get_item_num ((uint)m_item_id);
			sys._instance.m_self.remove_item((uint)m_item_id,_num,game_data._instance.get_t_language ("ji_yin_panel.cs_37_57"));//基因使用
			sys._instance.m_self.add_att(e_player_attr.player_jjc_point,_num * _item.gold);
			update_ui();
		}
		if(message.m_opcode == opclient_t.CMSG_ITEM_FENJIE)
		{
			int _zhanhun = 0;
			for(int i = 0; i < _ids2.Count;i++)
			{
				s_t_item _item = game_data._instance.get_item ((int)_ids2[i]);
				int _num = sys._instance.m_self.get_item_num (_ids2[i]);
				sys._instance.m_self.remove_item(_ids2[i],_num,game_data._instance.get_t_language ("ji_yin_panel.cs_48_51"));//基因分解消耗
				_zhanhun += _num * _item.gold;
			}
			sys._instance.m_self.add_att(e_player_attr.player_jjc_point,_zhanhun);
			update_ui();
		}
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "ji_yin_dui_huan")
		{
			m_item_id = (int)message.m_ints[0];
			
			protocol.game.cmsg_role_duihuan _msg = new protocol.game.cmsg_role_duihuan();
			_msg.item_id = (uint)m_item_id;
			net_http._instance.send_msg<protocol.game.cmsg_role_duihuan> (opclient_t.CMSG_ROLE_DUIHUAN, _msg);
		}
		if(message.m_type == "ji_yin_sell")
		{
			m_item_id = (int)message.m_ints[0];
			
			protocol.game.cmsg_item_sell _msg = new protocol.game.cmsg_item_sell();

			_msg.item_id = (uint)m_item_id;
			net_http._instance.send_msg<protocol.game.cmsg_item_sell> (opclient_t.CMSG_ITEM_SELL, _msg);
		}
		if(message.m_type == "ji_yin_yijianfenjie")
		{
			 _ids = m_card_page_gui.GetComponent<card_page_gui>().m_jy_ids;
			 _ids2 = new List<uint>();
			for(int i = 0; i < _ids.Count;i++)
			{
				s_t_item item = game_data._instance.get_item((int)_ids[i]);
				if (sys._instance.m_self.has_card(item.def_1)&&item.font_color <= 3)
				{
					_ids2.Add(_ids[i]);
				}
			}
			if(_ids2.Count > 0)
			{
				protocol.game.cmsg_item_fenjie _msg1 = new protocol.game.cmsg_item_fenjie();;
				for(int i = 0;i < _ids2.Count;i ++ )
				{
					_msg1.item_id.Add(_ids2[i]);
				}
				net_http._instance.send_msg<protocol.game.cmsg_item_fenjie> (opclient_t.CMSG_ITEM_FENJIE, _msg1);

			}
			else
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("ji_yin_panel.cs_98_59"));//没有可供回收的3星及以下基因体
			}


		}

	}



	public void update_ui()
	{
		if(sys._instance.m_self == null)
		{
			return ;
		}

		m_card_page_gui.GetComponent<card_page_gui>().init();
		m_card_page_gui.GetComponent<card_page_gui>().jy_reset();
	}

	public static bool can_duihuan()
	{
		for(int i = 0;i < sys._instance.m_self.m_t_player.item_ids.Count;i ++)
		{
			if(sys._instance.m_self.is_card_jiyin(sys._instance.m_self.m_t_player.item_ids[i]))
			{
				uint _item_id = sys._instance.m_self.m_t_player.item_ids[i];
				s_t_item _t_item = game_data._instance.get_item ((int)_item_id);
				if(!sys._instance.m_self.has_card(_t_item.def_1))
				{
					return true;
				}
			}
		}
		return false;
	}
	public void click(GameObject obj)
	{
		if(obj.name == "yijianfenjie")
		{
			s_message _mes = new s_message();
			_mes.m_type = "ji_yin_yijianfenjie";
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),game_data._instance.get_t_language ("ji_yin_panel.cs_141_50"),_mes);//提示//你确定要回收所有的[ffff00]3星[-]及以下基因体
		}

	}
	// Update is called once per frame
	void Update () {
		
	}
}
