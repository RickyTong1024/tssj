
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mowang_shop_gui : MonoBehaviour,IMessage {

	public GameObject m_shop;
	public GameObject m_reward;
	public GameObject m_scro;
	public GameObject m_hb_power;
	private int m_select = 0;
	private bool m_need_update = false;
	private s_t_boss_shop m_t_shop;
	private uint m_role_power = 50110001;

	public UILabel m_shop_Label;
	public UILabel m_shop1_Label;
	public UILabel m_reward_Label;
	public UILabel m_reward1_Label;
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
		sys._instance.m_self.m_item_shop_effect = 0;
		m_select = 0;
		m_need_update = true;
	}

	void update_ui()
	{
		if (m_select == 0)
		{
			m_shop.GetComponent<UIToggle>().value = true;
			if(sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_pvp)
			{
				int num = sys._instance.m_self.get_item_num(m_role_power);
				m_hb_power.transform.GetComponent<UILabel>().text = "[FF8585]" + num.ToString();
				m_hb_power.transform.parent.gameObject.SetActive(true);
			}
			else
			{
				m_hb_power.transform.parent.gameObject.SetActive(false);
			}
		}
		else if (m_select == 1)
		{
			m_reward.GetComponent<UIToggle>().value = true;
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			root_gui._instance.m_shop = 0;
			if(sys._instance.m_message_type.Count != 0)
			{
				s_message _message = new s_message();
				_message.m_type = "show_huo_dong";
				_message.m_ints.Add(1);
				_message.m_bools.Add(false);
				cmessage_center._instance.add_message(_message);
				sys._instance.m_game_state = "hall";
				sys._instance.load_scene(sys._instance.m_hall_name);
				s_message _message1 = new s_message();
				_message1.m_type = "show_main_gui";
				cmessage_center._instance.add_message(_message1);
			}
		}
		if (obj.transform.name == "shop")
		{
			m_select = 0;
			m_need_update = true;
		}
		if(obj.transform.name == "reward")
		{
			m_select = 1;
			m_need_update = true;
		}
	}

	void refresh_gird(int id)
	{
		List<int> shop_ids = new List<int>();
		for(int i = 0;i < game_data._instance.m_dbc_boss_shop.get_y();++i )
		{
			int shop_id = int.Parse(game_data._instance.m_dbc_boss_shop.get (0,i));
			s_t_boss_shop _t_boss_shop = game_data._instance.get_t_boss_shop(shop_id);
			if(sys._instance.m_self.m_t_player.level < _t_boss_shop.level)
			{
				continue;
			}
			shop_ids.Add(_t_boss_shop.id);
		}
		m_scro.SetActive(true);
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_scro);
		int _id = 0;
		if(id == 0)
		{
			for(int i = 0; i < shop_ids.Count ;i ++)
			{
				int row = i / 2;
				int lie =  i % 2;
				
				GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");
				
				_card.transform.parent = m_scro.transform;
				_card.transform.localPosition = new Vector3(401 * lie - 164, -138 * row + 106,0);
				_card.transform.localScale = new Vector3(1,1,1);
				_card.GetComponent<temaihui_card>().m_shop_id = shop_ids[_id];
				_card.GetComponent<temaihui_card>().type = 5;
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
	
	public static bool can_buy(int id)
	{
		for(int i = 0; i < sys._instance.m_self.m_t_player.mw_shop_rewards.Count ; ++i)
		{
			if(sys._instance.m_self.m_t_player.mw_shop_rewards[i] == id)
			{
				return true;
			}
		}
		return false;
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "buy_mowang_shop")
		{
			m_t_shop = game_data._instance.get_t_boss_shop((int)message.m_ints[0]);
			s_message _message = new s_message();
			_message.m_type = "buy_num_gui";
			_message.m_ints.Add((int)message.m_ints[0]);
			_message.m_ints.Add(4);
			cmessage_center._instance.add_message(_message);
		}
		if(message.m_type == "refresh_mowang_shop_gui")
		{
			m_need_update = true;
		}
	}

	void IMessage.net_message(s_net_message message)
	{

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
