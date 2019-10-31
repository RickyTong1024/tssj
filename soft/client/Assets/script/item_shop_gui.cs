
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class item_shop_gui : MonoBehaviour,IMessage {

	// Use this for initialization
	public GameObject m_scro;
	
	public GameObject m_chongzhi;
	private s_t_shop_xg m_t_shop_xg;
	private s_t_item m_t_item;
	public GameObject m_buy_num_gui;
	public int sy_xg_num = 0;
	public int m_zprice = 0;
	public int buy_equip_num = 0;
	public int buy_equip_totle_price = 0;
	bool m_need_update = false;
	bool m_need_refersh = false;
	public GameObject m_mowang_have;
	public GameObject m_zhanhun_have;

	public UILabel m_chongzhi_label;
    public int m_shop_tyep = 1;

	void Start () 
	{


		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		m_need_update = true;
		m_need_refersh = false;
	}
	
	public void hide()
	{
		//this.gameObject.SetActive
	}
	public void ok()
	{
		hide ();
	}
	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_SHOP_CHECK)
		{
			protocol.game.smsg_shop_refresh _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_refresh> (message.m_byte);
			
			sys._instance.m_self.m_t_player.shop1_ids.Clear();
			sys._instance.m_self.m_t_player.shop1_sell.Clear();
			
			for(int i = 0;i < _msg.shop1_ids.Count;i ++)
			{
				sys._instance.m_self.m_t_player.shop1_ids.Add(_msg.shop1_ids[i]);
				sys._instance.m_self.m_t_player.shop1_sell.Add(_msg.shop1_sell[i]);
			}
			
			sys._instance.m_self.m_t_player.shop2_ids.Clear();
			sys._instance.m_self.m_t_player.shop2_sell.Clear();
			
			for(int i = 0;i < _msg.shop2_ids.Count;i ++)
			{
				sys._instance.m_self.m_t_player.shop2_ids.Add(_msg.shop2_ids[i]);
				sys._instance.m_self.m_t_player.shop2_sell.Add(_msg.shop2_sell[i]);
			}

			m_need_update = true;
			m_need_refersh = false;
		}
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "select_buy_xg_item")
		{
			int _count = (int)message.m_ints[0];
			
			if (_count > sys._instance.m_self.get_att(e_player_attr.player_jewel))
			{
				root_gui._instance.show_recharge_dialog_box(delegate() {
					m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide();
					this.transform.Find("frame_big").GetComponent<frame>().hide();
				});
				return;
			}
			protocol.game.cmsg_shop_xg _net_msg = new protocol.game.cmsg_shop_xg ();;
			_net_msg.shop_id = m_t_shop_xg.id;
			_net_msg.shop_num = sy_xg_num;
			net_http._instance.send_msg<protocol.game.cmsg_shop_xg> (opclient_t.CMSG_SHOP_XG,_net_msg);
		}
		if(message.m_type == "buy_xg_item")
		{
			m_t_shop_xg = game_data._instance.get_shop_xg((int)message.m_ints[0]);
			s_message _message = new s_message();
			_message.m_type = "buy_num_gui";
			_message.m_ints.Add((int)message.m_ints[0]);
			_message.m_ints.Add(2);
			cmessage_center._instance.add_message(_message);
		}
		if(message.m_type == "buy_temaihui_item")
		{
			sy_xg_num = (int)message.m_ints[0];
			m_zprice = (int)message.m_ints[1];
			string _des = string.Format(game_data._instance.get_t_language ("item_shop_gui.cs_117_31") , (int)message.m_ints[1]//是否花费[00ffff]{0}钻石[-]购买[00ff00][{1}][-]?
			,sys._instance.get_res_info(m_t_shop_xg.type,m_t_shop_xg.vlaue1,m_t_shop_xg.vlaue2*sy_xg_num,m_t_shop_xg.vlaue3) );
			s_message _msg = new s_message();
			_msg.m_ints.Add((int)message.m_ints[1]);
			_msg.m_type = "select_buy_xg_item";
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
		}
		if(message.m_type == "refresh_item_shop_gui")
		{
			m_need_update = true;
			m_need_refersh = false;
		}
	}

	public void click(GameObject obj)
	{

		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
		else if(obj.transform.name == "chongzhi")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);

		}

	}

	void refresh_gird(bool flag)
	{
		List<int> shop4_ids = new List<int>();
		dbc m_dbc_shop_xg = game_data._instance.get_dbc_shop_xg();
		for(int k = 0;k < m_dbc_shop_xg.get_y();++k )
		{
			int xg_id = int.Parse(m_dbc_shop_xg.get (0,k));
			s_t_shop_xg _t_shop_xg = game_data._instance.get_shop_xg(xg_id);
			if(_t_shop_xg.level > sys._instance.m_self.m_t_player.level || _t_shop_xg.shop_type != m_shop_tyep)
			{
				continue;
			}
			shop4_ids.Add(_t_shop_xg.id);
		}
        
		int i = 0;
		m_scro.SetActive(true);
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_scro);
		int _id = 0;
		for(i = 0;i < shop4_ids.Count ; i++)
		{
			int row = i / 2;
			int lie =  i % 2;
			int xg_id = int.Parse(m_dbc_shop_xg.get (0,i));
			s_t_shop_xg _t_shop_xg = game_data._instance.get_shop_xg(shop4_ids[ _id]);
			GameObject temp = game_data._instance.ins_object_res ("ui/temaihui_card");
			temp.transform.parent = m_scro.transform;
			temp.transform.localPosition = new Vector3 (401 * lie - 164, -138 * row + 106,0);
			temp.transform.localScale = new Vector3(1,1,1);
			temp.GetComponent<temaihui_card>().m_item_shop_gui = this.gameObject;
			temp.GetComponent<temaihui_card>().m_shop_id = _t_shop_xg.id;
			temp.GetComponent<temaihui_card>().type = 1;
			temp.GetComponent<temaihui_card>().updata_ui();
			sys._instance.add_pos_anim(temp,0.3f, new Vector3(0, 60, 0), _id * 0.05f);
			sys._instance.add_alpha_anim(temp,0.3f, 0, 1.0f, _id * 0.05f);
			_id++;
		}
	}

	// Update is called once per frame
	void Update () {
		if(m_need_update)
		{
			m_need_update = false;
			refresh_gird (m_need_refersh);
		}
     
	}
}
