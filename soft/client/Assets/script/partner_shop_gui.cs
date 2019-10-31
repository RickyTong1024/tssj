
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class partner_shop_gui : MonoBehaviour,IMessage {

	const uint m_shuaxin_id = 50020001;
	public GameObject m_root;
	
	public GameObject m_refresh_num;
	public GameObject m_free_num;
	public GameObject m_time;
	public GameObject m_stone_num;
	public GameObject m_refresh_shop;
	private s_t_role_shop m_t_shop;

	private int m_gezi = 0;
	bool m_need_update = false;
	bool m_need_refersh = false;
	public GameObject m_zhanhun_have;
	public GameObject m_effect;

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		this.InvokeRepeating ("time", 0.0f, 1.0f);
		m_need_update = true;
		m_need_refersh = false;
		check ();
	}

	public static bool effect()
	{
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shop)
		{
			return false;
		}
		else if(sys._instance.m_self.get_item_num(m_shuaxin_id) > 0 || sys._instance.m_self.m_t_player.shop2_refresh_num > 0)
		{
			return true;
		}
		return false;
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}

	void time()
	{
		if(sys._instance.m_self.m_t_player.shop2_refresh_num < 10)
		{
			long _time = 7200000 - (int)(timer.now() - sys._instance.m_self.m_t_player.shop_last_time);
			m_time.GetComponent<UILabel>().text =  timer.get_time_show(_time);
			m_time.SetActive(true);
			
		}
		else
		{
			m_time.SetActive(false);
			
		}
	
	}


	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_SHOP_BUY)
		{
			protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy> (message.m_byte);
			for(int i =0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i],true);
			}
			
			for(int i = 0;i < _msg.equips.Count;i ++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i],true);
			}
			for(int i = 0; i < _msg.types.Count;++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("partner_shop_gui.cs_94_101"));//伙伴商店购买
			}
			sys._instance.m_self.sub_res(m_t_shop.money_type, m_t_shop.price,game_data._instance.get_t_language ("partner_shop_gui.cs_96_68"));//伙伴商店消耗
			

			sys._instance.m_self.m_t_player.shop2_sell[m_gezi] = 1;
			sys._instance.m_self.m_t_player.hs_task_num++;//huoban
			sys._instance.m_self.check_target_done();
			
			m_need_update = true;
			m_need_refersh = false;
		}
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
		
		if(message.m_opcode == opclient_t.CMSG_SHOP_REFRESH)
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
			if(sys._instance.m_self.m_t_player.shop2_refresh_num > 0)
			{
				sys._instance.m_self.m_t_player.shop2_refresh_num -= 1;
			}
			else if(sys._instance.m_self.get_item_num(m_shuaxin_id) > 0)
			{
				sys._instance.m_self.remove_item(m_shuaxin_id,1,game_data._instance.get_t_language ("partner_shop_gui.cs_159_52"));//伙伴商店刷新消耗
				sys._instance.m_self.m_t_player.shop4_refresh_num ++;

			}
			else
			{
				sys._instance.m_self.sub_att(e_player_attr.player_jjc_point,get_refresh_cost());
				sys._instance.m_self.m_t_player.shop4_refresh_num ++;
			}
			
			m_need_update = true;
			m_need_refersh = false;
		}
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "buy_item")
		{
			//buy_item((int)message.m_ints[0]);
			m_t_shop = game_data._instance.get_role_shop((int)message.m_ints[0]);
			m_gezi = (int)message.m_ints[1];
			string item_num = sys._instance.m_self.get_item_num(m_t_shop.sell_type,m_t_shop.sell_value_0,m_t_shop.sell_value_1,m_t_shop.sell_value_2);
			if(m_t_shop.money_type == 2)
			{
				string _des = string.Format(game_data._instance.get_t_language ("guild_shop.cs_424_40") , m_t_shop.price ,//是否花费[00ffff]{0}钻石[-]购买{1}[-]?
					 sys._instance.get_res_info(m_t_shop.sell_type,m_t_shop.sell_value_0,m_t_shop.sell_value_1,m_t_shop.sell_value_2,1) );
				if(item_num != "")
				{
					_des += "\n"+ game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ item_num;//当前拥有：
				}
				s_message _msg = new s_message();
				
				_msg.m_type = "select_buy_item";
				
				root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
			}
			else
			{
				protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy ();
				_net_msg.type = 2;
				_net_msg.gezi = m_gezi+1;
				
				net_http._instance.send_msg<protocol.game.cmsg_shop_buy> (opclient_t.CMSG_SHOP_BUY, _net_msg);
			}
		
		}
		if(message.m_type == "select_buy_item")
		{
			protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy ();
			_net_msg.type = 2;
			_net_msg.gezi = m_gezi+1;
			
			net_http._instance.send_msg<protocol.game.cmsg_shop_buy> (opclient_t.CMSG_SHOP_BUY, _net_msg);
		}
	}
	int get_refresh_cost()
	{
		return 20;
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
		else if(obj.transform.name == "refresh_shop")
		{
			s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
			int re_num = t_vip.refresh_shop_num - sys._instance.m_self.m_t_player.shop4_refresh_num;
			if(re_num <= 0 &&  sys._instance.m_self.m_t_player.shop2_refresh_num <= 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("master_shop_gui.cs_212_58"));//[ffc882]提升VIP等级增加刷新次数
				return;
			}
			if(sys._instance.m_self.get_item_num(m_shuaxin_id) <= 0 && sys._instance.m_self.m_t_player.shop2_refresh_num <= 0)
			{
				
				if (sys._instance.m_self.get_att(e_player_attr.player_jjc_point) < get_refresh_cost())
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("partner_shop_gui.cs_246_47"));//[ffc882]战魂不足
					return;
				}
			}
			
			protocol.game.cmsg_shop_refresh _net_msg = new protocol.game.cmsg_shop_refresh ();;
			_net_msg.type = 2;
			net_http._instance.send_msg<protocol.game.cmsg_shop_refresh> (opclient_t.CMSG_SHOP_REFRESH, _net_msg);
			
		}
		update_ui ();
	}
	
	void check()
	{
		protocol.game.cmsg_common _net_msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SHOP_CHECK, _net_msg);
	}
	
	void refresh_gird(bool flag)
	{
		int _id = 0;
		m_root.SetActive(true);
		_id = 0;
		sys._instance.remove_child (m_root);
		for(int y = 0;y < 3;y ++)
		{
			for(int x = 0;x < 2;x ++)
			{				
				GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");
				
				_card.transform.parent = m_root.transform;
				_card.transform.name = _id.ToString();
				_card.transform.localPosition = new Vector3(401 * x - 200, -138 * y + 16,0);
				_card.transform.localScale = new Vector3(1,1,1);
				_card.GetComponent<temaihui_card>().m_item_shop_gui = this.gameObject;
				_card.GetComponent<temaihui_card>().m_shop_id = (int)sys._instance.m_self.m_t_player.shop2_ids[_id];
				_card.GetComponent<temaihui_card>().m_shell = (int)sys._instance.m_self.m_t_player.shop2_sell[_id];
				_card.GetComponent<temaihui_card>().type = 8; 
				_card.GetComponent<temaihui_card>().updata_ui();
				_card.SetActive (true);
				
				sys._instance.add_pos_anim(_card,0.3f, new Vector3(0, 60, 0), _id * 0.05f);
				sys._instance.add_alpha_anim(_card,0.3f, 0, 1.0f, _id * 0.05f);
				_id ++;
			}
		}
	}
	void update_ui()
	{

		m_refresh_shop.SetActive(true);
		int num = sys._instance.m_self.get_item_num(m_shuaxin_id);
		m_effect.SetActive(false);
		s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
		int re_num = t_vip.refresh_shop_num - sys._instance.m_self.m_t_player.shop4_refresh_num;
		if(num > 0 ||  sys._instance.m_self.m_t_player.shop2_refresh_num > 0)
		{
			m_effect.SetActive(true);
		}
		m_free_num.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.shop2_refresh_num + "/10";
		m_refresh_num.GetComponent<UILabel>().text = re_num.ToString();
		m_stone_num.GetComponent<UILabel>().text = num.ToString ();
		if(sys._instance.m_self.m_t_player.shop2_refresh_num < 10)
		{
			m_time.SetActive(true);
		}
		else
		{
			m_time.SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(m_need_update)
		{
			m_need_update = false;
			refresh_gird (m_need_refersh);
			update_ui ();
		}
		
	}
}
