
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mijing_shop_gui : MonoBehaviour,IMessage {

	public GameObject m_shop;
	public GameObject m_zz;
	public GameObject m_jz;
	public GameObject m_reward;
	public GameObject m_scro;
	public GameObject m_buy_equip_gui;
	public GameObject m_effect;
	public int buy_equip_num = 0;
	public int buy_equip_totle_price = 0;
	private int m_select = 0;
	private bool m_need_update = false;
	private s_t_ttt_shop m_t_shop;
	private s_t_ttt_mubiao m_t_mubiao;

	public UILabel m_shop_Label;
	public UILabel m_shop1_Label;
	public UILabel m_zz_Label;
	public UILabel m_zz1_Label;
	public UILabel m_jz_Label;
	public UILabel m_jz1_Label;
	public UILabel m_reward_Label;
	public UILabel m_reward1_Label;
	public UILabel m_sm_Label;

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
		m_select = 0;
		m_need_update = true;
	}

	public void equip_shop()
	{
		m_select = 0;
		m_shop.GetComponent<UIToggle>().value = true;
		m_zz.GetComponent<UIToggle>().value = false;
		m_jz.GetComponent<UIToggle>().value = false;
		m_reward.GetComponent<UIToggle>().value = false;
		m_need_update = true;
	}
	public void reward_shop()
	{
		m_select = 3;
		m_shop.GetComponent<UIToggle>().value = false;
		m_zz.GetComponent<UIToggle>().value = false;
		m_jz.GetComponent<UIToggle>().value = false;
		m_reward.GetComponent<UIToggle>().value = true;
		m_need_update = true;
	}

	void update_ui()
	{
		if (m_select == 0)
		{
			m_shop.GetComponent<UIToggle>().value = true;
		}
		else if (m_select == 1)
		{
			m_zz.GetComponent<UIToggle>().value = true;
		}
		else if (m_select == 2)
		{
			m_jz.GetComponent<UIToggle>().value = true;
		}
		else if(m_select == 3)
		{
			m_reward.GetComponent<UIToggle>().value = true;
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

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "sm")
		{
			string s = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			string s1 = game_data._instance.get_t_language ("mijing_shop_gui.cs_101_15");//[00FF00]回收装备[-]或{nn}参加[00FF00]BOSS战[-]和[00FF00]星河秘境[-]可获得合金
			root_gui._instance.show_single_dialog_box(s,s1,new s_message());
		}
		if(obj.transform.name == "shop")
		{
			m_select = 0;
			m_need_update = true;
		}
		if(obj.transform.name == "zz")
		{
			m_select = 1;
			m_need_update = true;
		}
		if(obj.transform.name == "jz")
		{
			m_select = 2;
			m_need_update = true;
		}
		if(obj.transform.name == "reward")
		{
			m_select = 3;
			m_need_update = true;
		}
	}

	void refresh_gird(int id)
	{
		m_scro.SetActive(true);
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_scro);
		int _id = 0;
		if(id == 0 || id == 1 || id == 2)
		{
			List<int> shop_ids = new List<int>();
			for(int i = 0;i < game_data._instance.m_dbc_ttt_shop.get_y();++i )
			{
				int shop_id = int.Parse(game_data._instance.m_dbc_ttt_shop.get (0,i));
				s_t_ttt_shop _t_ttt_shop = game_data._instance.get_t_ttt_shop(shop_id);
				if(_t_ttt_shop.fen_ye == id+1)
				{
					if(_t_ttt_shop.level > sys._instance.m_self.m_t_player.level)
					{
						continue;
					}
					shop_ids.Add(_t_ttt_shop.id);
				}
			}
			for(int i = 0; i < shop_ids.Count ;i ++)
			{
				int row = i / 2;
				int lie =  i % 2;
				
				GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");
				
				_card.transform.parent = m_scro.transform;
				_card.transform.localPosition = new Vector3(401 * lie - 164, -138 * row + 106,0);
				_card.transform.localScale = new Vector3(1,1,1);
				_card.GetComponent<temaihui_card>().m_shop_id = shop_ids[_id];
				_card.GetComponent<temaihui_card>().type = 2;
				_card.GetComponent<temaihui_card>().updata_ui();
				_card.SetActive (true);
				sys._instance.add_pos_anim(_card,0.3f, new Vector3(0, 60, 0), _id * 0.05f);
				sys._instance.add_alpha_anim(_card,0.3f, 0, 1.0f, _id * 0.05f);
				_id ++;
			}
		}
		if(id == 3)
		{
			List<int> shop_ids = new List<int>();
			for(int i = 0;i < game_data._instance.m_dbc_ttt_mubiao.get_y();++i )
			{
				int shop_id = int.Parse(game_data._instance.m_dbc_ttt_mubiao.get (0,i));
				s_t_ttt_mubiao _t_ttt_mubiao = game_data._instance.get_t_ttt_mubiao(shop_id);
				shop_ids.Add(_t_ttt_mubiao.id);
			}
			shop_ids.Sort(comp);
			for(int i = 0; i < shop_ids.Count ;i ++)
			{
				int row = i / 2;
				int lie =  i % 2;
				
				GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");
				
				_card.transform.parent = m_scro.transform;
				_card.transform.localPosition = new Vector3(401 * lie - 164, -138 * row + 106,0);
				_card.transform.localScale = new Vector3(1,1,1);
				_card.GetComponent<temaihui_card>().m_shop_id = shop_ids[_id];
				_card.GetComponent<temaihui_card>().type = 4;
				_card.GetComponent<temaihui_card>().updata_ui();
				_card.SetActive (true);
				sys._instance.add_pos_anim(_card,0.3f, new Vector3(0, 60, 0), _id * 0.05f);
				sys._instance.add_alpha_anim(_card,0.3f, 0, 1.0f, _id * 0.05f);
				_id ++;
			}
		}
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
		int buy_qualification = 0;
		for (int i = 0; i < sys._instance.m_self.m_t_player.ttt_last_stars.Count; ++i)
		{
			buy_qualification += sys._instance.m_self.m_t_player.ttt_last_stars[i];
		}
		for(int i = 0;i < game_data._instance.m_dbc_ttt_mubiao.get_y();++i )
		{
			int shop_id = int.Parse(game_data._instance.m_dbc_ttt_mubiao.get (0,i));
			s_t_ttt_mubiao _t_ttt_mubiao = game_data._instance.get_t_ttt_mubiao(shop_id);
			if(!can_buy(shop_id) && buy_qualification >= _t_ttt_mubiao.star &&  sys._instance.m_self.get_att(e_player_attr.player_hj) >= _t_ttt_mubiao.price)
			{
				return true;
			}
		}
		return false;
	}

	public static bool can_buy(int id)
	{
		for(int i = 0; i < sys._instance.m_self.m_t_player.ttt_shop_rewards.Count ; ++i)
		{
			if(sys._instance.m_self.m_t_player.ttt_shop_rewards[i] == id)
			{
				return true;
			}
		}
		return false;
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "buy_equip")
		{
			m_t_shop = game_data._instance.get_t_ttt_shop((int)message.m_ints[0]);
			s_message _message = new s_message();
			_message.m_type = "buy_num_gui";
			_message.m_ints.Add((int)message.m_ints[0]);
			_message.m_ints.Add(6);
			cmessage_center._instance.add_message(_message);
		}
		if(message.m_type == "buy_mijing_equip")
		{
			int id = (int)message.m_ints[0];
			m_t_mubiao = game_data._instance.get_t_ttt_mubiao(id);
			protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy ();
			_net_msg.item_id = id;
			_net_msg.num = 1;
			net_http._instance.send_msg<protocol.game.cmsg_shop_buy> (opclient_t.CMSG_TTT_SHOP_MUBIAO, _net_msg);
		}
		if(message.m_type == "refresh_mj_shop_gui")
		{
			m_need_update = true;
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_TTT_SHOP_MUBIAO)
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
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("mijing_shop_gui.cs_298_101"));//秘境商店获得
			}
			sys._instance.m_self.sub_res(6, m_t_mubiao.price);
			sys._instance.m_self.m_t_player.ttt_shop_rewards.Add(m_t_mubiao.id);
			m_need_update = true;
			s_message _message = new s_message();
			_message.m_type = "refresh_mijing_gui";
			cmessage_center._instance.add_message(_message);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(m_need_update)
		{
			m_need_update = false;
			refresh_gird (m_select);
			update_ui ();
		}
	
	}
}
