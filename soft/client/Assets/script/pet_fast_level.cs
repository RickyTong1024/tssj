
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pet_fast_level : MonoBehaviour {

	public pet m_pet;
	public GameObject m_icon;
	public UILabel m_name;
	public UILabel m_level;
	public UILabel m_gold;
	public UILabel m_exp;
	public UILabel m_input;
	List<int> m_item_ids = new List<int >();
	public int m_input_num = 1;
	static int[] item_ids = new int[4]{110020001,110020002,110020003,110020004};
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		m_item_ids.Clear ();
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_pet_icon(m_pet.get_guid());
		_icon.transform.GetComponent<BoxCollider>().enabled = false;
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		m_name.text = pet.get_color_name (m_pet.m_t_pet.name,m_pet.m_t_pet.color);
		m_level.text = m_pet.get_level().ToString ();
		m_input_num = 1;
		consume_item();
		update_ui ();
	}

	public void update_ui()
	{
		m_input.text = m_input_num.ToString ();
		s_t_pet t_pet = game_data._instance.get_t_pet (m_pet.get_template_id());
		int enhance_up = game_data._instance.m_dbc_exp.get_y();
		int gold = 0;
		int max_exp = pet_weiyang_gui.get_exp(m_pet,m_pet.get_level(),enhance_up - m_pet.get_level()) - m_pet.get_exp();
		for(int i = 0; i < m_item_ids.Count;++i)
		{
			s_t_item t_item = game_data._instance.get_item (m_item_ids[i]);
			gold += t_item.def_1;
		}
		if(gold > max_exp)
		{
			gold = max_exp;
		}
		string color = "[ff0000]";
		if(sys._instance.m_self.m_t_player.gold >= gold)
		{
			color = sys._instance.get_res_color(1);
		}
		m_gold.text = color + gold.ToString ();
		m_exp.text = gold.ToString ();
	}

	public static int get_max_num(pet m_pet)
	{
		int enhance_up = game_data._instance.m_dbc_exp.get_y();
		s_t_pet t_pet = game_data._instance.get_t_pet (m_pet.get_template_id());
		if(enhance_up > m_pet.get_level() && m_pet.get_level() >= 1)
		{
			List<s_t_item> m_items = new List<s_t_item>();
			m_items = get_items();
			for(int i = 1; i <= enhance_up - m_pet.get_level();i++)
			{
				int exp = 0;
				int need_exp = pet_weiyang_gui.get_exp(m_pet,m_pet.get_level(),i) - m_pet.get_exp();
				if(need_exp > 0)
				{
					for(int j = 0; j < m_items.Count;++j)
					{
						exp += m_items[j].def_1;
						if(exp >= need_exp)
						{
							break;
						}
					}
				}
				if(exp > sys._instance.m_self.m_t_player.gold || exp < need_exp)
				{
					enhance_up = i-1;
					break;
				}
			}
		}
		else
		{
			enhance_up = 0;
		}
		enhance_up = Mathf.Min (enhance_up, game_data._instance.m_dbc_exp.get_y () - m_pet.get_level ());
		return enhance_up;
	}

	static List<s_t_item> get_items()
	{
		List<s_t_item> m_items = new List<s_t_item>();
		for(int j = 0; j < item_ids.Length;++j)
		{
			for(int i = 0;i < sys._instance.m_self.get_item_num((uint)item_ids[j])&& i < 1000;i ++)
			{
				s_t_item t_item = game_data._instance.get_item(item_ids[j]);
				m_items.Add(t_item);
			}
		}
		m_items.Sort (comp);
		if(m_items.Count > 1000)
		{
			m_items.RemoveRange(1000,m_items.Count-1000);
		}
		return m_items;
	}

	public static int comp(s_t_item x, s_t_item y)
	{
		if (x.font_color > y.font_color)
		{
			return -1;
		}
		else if (x.font_color < y.font_color)
		{
			return 1;
		}
		else if (x.id > y.id)
		{
			return -1;
		}
		else if (x.id < y.id)
		{
			return 1;
		}
		return 0; 
	}

	void consume_item()
	{
		m_item_ids.Clear ();
		List<s_t_item> m_items = new List<s_t_item>();
		m_items = get_items();
		int exp = 0;
		int enhance_up = game_data._instance.m_dbc_exp.get_y();
		s_t_pet t_pet = game_data._instance.get_t_pet (m_pet.get_template_id());
		int need_exp = pet_weiyang_gui.get_exp(m_pet,m_pet.get_level(),m_input_num) - m_pet.get_exp();
		if(need_exp > 0)
		{
			for(int i = 0; i < m_items.Count;++i)
			{
				exp += m_items[i].def_1;
				m_item_ids.Add(m_items[i].id);
				if(exp >= need_exp)
				{
					break;
				}
			}
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "add") 
		{
			int num = get_max_num (m_pet);
			if(num == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_fast_level.cs_171_46"));//[ffc882]拥有的粮食不足以提升一级
				return;
			}
			if(m_input_num + 1 <= num)
			{
				m_input_num += 1;
				consume_item();
				update_ui();
			}
		}
		else if(obj.name == "sub")
		{
			if (m_input_num > 1)
			{
				m_input_num--;
				consume_item();
				update_ui();
			}
		}
		else if(obj.name == "add10")
		{
			int num = get_max_num (m_pet);
			if(num == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_fast_level.cs_171_46"));//[ffc882]拥有的粮食不足以提升一级
				return;
			}
			if(m_input_num +10 > num)
			{
				m_input_num = num;
			}
			else
			{
				m_input_num += 10;
			}
			consume_item();
			update_ui();
		}
		else if(obj.name == "sub10")
		{
			if (m_input_num <= 10) 
			{
				m_input_num = 1;
			}
			else
			{
				m_input_num -= 10;
			}
			consume_item();
			update_ui();
		}
		else if(obj.name == "queding")
		{
			int num = get_max_num (m_pet);
			if(num == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_fast_level.cs_227_46"));//[ffc882]请选择需要升级的等级数
				return;
			}
			s_message _msg = new s_message ();
			
			_msg.m_type = "pet_yj_enhance";
			for(int i = 0; i < m_item_ids.Count;++i)
			{
				_msg.m_ints.Add (m_item_ids[i]);
			}
			cmessage_center._instance.add_message (_msg);
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.name == "hide")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.name == "cancle")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
