
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class honor_shop_gui : MonoBehaviour ,IMessage{
	
	public GameObject m_scro;
	public GameObject m_shop;
	public GameObject m_reward;
	public GameObject m_effect;
	public GameObject m_hb_power;
	private List<GameObject> m_cards = new List<GameObject>();
	private s_t_sport_shop m_t_sport_shop;
	private s_t_sport_mubiao m_t_sport_mubiao;
	public bool m_need_update = false;
	private uint m_role_power = 50110001;
	int m_select = 0;


	//public UILabel m_jingjichang;
	// Use this for initialization
	void Start () 
	{
	
		cmessage_center._instance.add_handle (this);
	}

	public void OnEnable()
	{
		m_select = 0;
		m_need_update = true;
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	

	void IMessage.net_message(s_net_message message)
	{
		if ( message.m_opcode == opclient_t.CMSG_SPORT_SHOP_VIEW ) 
		{
			protocol.game.smsg_sport_shop_list _msg = net_http._instance.parse_packet<protocol.game.smsg_sport_shop_list> (message.m_byte);
			
			sys._instance.m_self.m_t_player.shop4_ids.Clear ();
			sys._instance.m_self.m_t_player.shop4_sell.Clear ();
			
			for (int i = 0; i < _msg.shop_ids.Count; i ++) 
			{
				sys._instance.m_self.m_t_player.shop4_ids.Add (_msg.shop_ids [i]);
				sys._instance.m_self.m_t_player.shop4_sell.Add (_msg.shop_sells [i]);
			}
			m_need_update = true;
		}
		if(message.m_opcode == opclient_t.CMSG_SPORT_MUBIAO)
		{
			protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy> (message.m_byte);
			
			for(int i = 0;i < _msg.equips.Count;i ++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i],true);
			}
			for(int i = 0;i < _msg.treasures.Count;i ++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i],true);
			}
			for(int i = 0; i < _msg.types.Count;++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("honor_shop_gui.cs_71_101"));//荣誉商店获得
			}
			sys._instance.m_self.sub_att(e_player_attr.player_treasure_powder,m_t_sport_mubiao.price);
			sys._instance.m_self.m_t_player.sport_shop_rewards.Add(m_t_sport_mubiao.id);
			m_need_update = true;
		}


	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "buy_honor_item")
		{
			m_t_sport_shop = game_data._instance.get_t_sport_shop((int)message.m_ints[0]);
			s_message _message = new s_message();
			_message.m_type = "buy_num_gui";
			_message.m_ints.Add((int)message.m_ints[0]);
			_message.m_ints.Add(3);
			cmessage_center._instance.add_message(_message);
		}
		if(message.m_type == "buy_honor_shop")
		{
			int id = (int)message.m_ints[0];
			m_t_sport_mubiao = game_data._instance.get_t_sport_mubiao(id);
			protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy ();
			_net_msg.item_id = id;
			_net_msg.num = 1;
			net_http._instance.send_msg<protocol.game.cmsg_shop_buy> (opclient_t.CMSG_SPORT_MUBIAO, _net_msg);
		}
		if(message.m_type == "refresh_honor_shop_gui")
		{
			m_need_update = true;
		}
	}


	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			if(sys._instance.m_message_type.Count != 0)
			{
				s_message _message = new s_message();
				_message.m_type = "show_main_gui";
				cmessage_center._instance.add_message(_message);
			}
			else
			{
				s_message _out_msg = new s_message ();
				
				_out_msg.m_type = "show_jing_ji_chang";
				
				cmessage_center._instance.add_message (_out_msg);
			}
		}
		else if(obj.transform.name == "shop")
		{
			m_select = 0;
			m_need_update = true;
		}

		else if(obj.transform.name == "reward")
		{
			m_select = 1;
			m_need_update = true;
		}
	}


	void refresh_gird()
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_scro);
		if(m_select == 0)
		{
			m_cards.Clear ();
			int _id = 0;
			List<int> shop4_ids = new List<int>();
			for(int i = 0;i < game_data._instance.m_dbc_sport_shop.get_y();++i )
			{
				int shop_id = int.Parse(game_data._instance.m_dbc_sport_shop.get (0,i));
				s_t_sport_shop _t_sport_shop = game_data._instance.get_t_sport_shop(shop_id);
				if(sys._instance.m_self.m_t_player.level < _t_sport_shop.level)
				{
					continue;
				}
				shop4_ids.Add(_t_sport_shop.id);
			}
			for(int i = 0; i < shop4_ids.Count ;i ++)
			{
				int row = i / 2;
				int lie =  i % 2;
				GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");
						
				_card.transform.parent = m_scro.transform;
				_card.transform.localPosition = new Vector3(401 * lie - 164, -138 * row + 106,0);
				_card.transform.localScale = new Vector3(1,1,1);
				_card.GetComponent<temaihui_card>().m_item_shop_gui = this.gameObject;
				_card.GetComponent<temaihui_card>().m_shop_id = shop4_ids[_id];
				_card.GetComponent<temaihui_card>().type = 9;
				_card.GetComponent<temaihui_card>().updata_ui();
				_card.SetActive (true);
						
				sys._instance.add_pos_anim(_card,0.3f, new Vector3(0, 60, 0), _id * 0.05f);
				sys._instance.add_alpha_anim(_card,0.3f, 0, 1.0f, _id * 0.05f);
				_id ++;
				m_cards.Add(_card);
				
			}
		}
		if(m_select == 1)
		{
			int _id = 0;
			List<int> shop4_ids = new List<int>();
			for(int i = 0;i < game_data._instance.m_dbc_sport_mubiao.get_y();++i )
			{
				int shop_id = int.Parse(game_data._instance.m_dbc_sport_mubiao.get (0,i));
				s_t_sport_mubiao _t_sport_mubiao = game_data._instance.get_t_sport_mubiao(shop_id);
				shop4_ids.Add(_t_sport_mubiao.id);
			}
			shop4_ids.Sort(comp);
			for(int i = 0; i < shop4_ids.Count ;i ++)
			{
				int row = i / 2;
				int lie =  i % 2;
				
				GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");
				
				_card.transform.parent = m_scro.transform;
				_card.transform.localPosition = new Vector3(401 * lie - 164, -138 * row + 106,0);
				_card.transform.localScale = new Vector3(1,1,1);
				_card.GetComponent<temaihui_card>().m_shop_id = shop4_ids[_id];
				_card.GetComponent<temaihui_card>().type = 3;
				_card.GetComponent<temaihui_card>().updata_ui();
				_card.SetActive (true);
				_id ++;
			}
		}
	}

	void update_ui()
	{
		if (m_select == 0)
		{
			m_shop.GetComponent<UIToggle>().value = true;
			if(sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_pvp)
			{
				m_hb_power.transform.parent.gameObject.SetActive(true);
				int num = sys._instance.m_self.get_item_num(m_role_power);
				m_hb_power.transform.GetComponent<UILabel>().text = "[FF8585]" + num.ToString();
			}
			else
			{
				m_hb_power.transform.parent.gameObject.SetActive(false);
			}
		}
		else if (m_select == 1)
		{
			m_reward.GetComponent<UIToggle>().value = true;
			m_hb_power.transform.parent.gameObject.SetActive(false);
		}
		if(is_effect())
		{
			m_effect.SetActive(true);
		}
		else
		{
			m_effect.SetActive(false);
		}
	}

	public void rewrd_shop()
	{
		m_select = 1;
		m_need_update = true;
	}

	public int comp(int x, int y)
	{
		if(!can_buy(x) && can_buy(y))
		{
			return -1;
		}
		else if(can_buy(x) && !can_buy(y))
		{
			return 1;
		}
		else if(x < y)
		{
			return -1 ;
		}
		else if(x >= y)
		{
			return 1 ;
		}
		return 0;
	}

	public static bool is_effect()
	{
		int buy_qualification = sys._instance.m_self.m_t_player.max_rank;
		for(int i = 0;i < game_data._instance.m_dbc_sport_mubiao.get_y();++i )
		{
			int shop_id = int.Parse(game_data._instance.m_dbc_sport_mubiao.get (0,i));
			s_t_sport_mubiao _t_sport_mubiao = game_data._instance.get_t_sport_mubiao(shop_id);
			if(!can_buy(shop_id) && buy_qualification <= _t_sport_mubiao.rank &&  sys._instance.m_self.get_att(e_player_attr.player_treasure_powder) >= _t_sport_mubiao.price)
			{
				return true;
			}
		}
		return false;
	}

	public static bool can_buy(int id)
	{
		for(int i = 0; i < sys._instance.m_self.m_t_player.sport_shop_rewards.Count ; ++i)
		{
			if(sys._instance.m_self.m_t_player.sport_shop_rewards[i] == id)
			{
				return true;
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
		if(m_need_update)
		{
			m_need_update = false;
			refresh_gird ();
			update_ui ();
		}
	
	}
}
