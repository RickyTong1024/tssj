
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_fast_gui : MonoBehaviour {
	
	public dhc.treasure_t m_treasure;
	public GameObject m_icon;
	public GameObject m_zs_button;
	public GameObject m_js_button;
	public UILabel m_name;
	public UILabel m_level;
	public UILabel m_gold;
	public UILabel m_exp;
	public UILabel m_input;
	List<ulong> m_treasure_guids = new List<ulong >();
	public int m_select = 0;
	public int m_input_num = 0;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		m_treasure_guids.Clear ();
		m_zs_button.SetActive (false);
		m_js_button.SetActive (false);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_treasure_icon(m_treasure.guid);
		_icon.transform.GetComponent<BoxCollider>().enabled = false;
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		m_name.text = treasure.get_treasure_real_name (m_treasure.template_id);
		m_level.text = m_treasure.enhance.ToString ();
		update_ui ();
	}

	public void update_ui()
	{
		m_input.text = m_input_num.ToString ();
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		int enhance_up = sys._instance.m_self.m_t_player.level;
		if(enhance_up > game_data._instance.m_dbc_treasure_enhance.get_y() -1)
		{
			enhance_up = game_data._instance.m_dbc_treasure_enhance.get_y() -1;
		}
		int gold = 0;
		int max_exp = game_data._instance.get_total_treasure_enhance(enhance_up,t_treasure.font_color) 
			- game_data._instance.get_total_treasure_enhance(m_treasure.enhance,t_treasure.font_color) - m_treasure.enhance_exp;
		for(int i = 0; i < m_treasure_guids.Count;++i)
		{
			dhc.treasure_t _t_treasure = sys._instance.m_self.get_treasure_guid(m_treasure_guids[i]);
			s_t_baowu t_baowu = game_data._instance.get_t_baowu (_t_treasure.template_id);
			gold += t_baowu.exp;
			gold +=  _t_treasure.enhance_exp;
			gold += game_data._instance.get_total_treasure_enhance(_t_treasure.enhance, t_baowu.font_color);
			if(_t_treasure.jilian > 0)
			{
				gold += t_baowu.exp*_t_treasure.jilian;
			}
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

	int get_max_num()
	{
		int enhance_up = sys._instance.m_self.m_t_player.level;
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		if(enhance_up > game_data._instance.m_dbc_treasure_enhance.get_y() -1)
		{
			enhance_up = game_data._instance.m_dbc_treasure_enhance.get_y() -1;
		}
		if(enhance_up > m_treasure.enhance)
		{
			int gold_max = enhance_up - m_treasure.enhance;
			for(int i = 1; i <= enhance_up - m_treasure.enhance;i++)
			{
				int gold = game_data._instance.get_total_treasure_enhance(m_treasure.enhance+i,t_treasure.font_color) 
					- game_data._instance.get_total_treasure_enhance(m_treasure.enhance,t_treasure.font_color) - m_treasure.enhance_exp;
				if(sys._instance.m_self.m_t_player.gold < gold)
				{
					gold_max = i -1;
					break;
				}
			}

			int exp_max = enhance_up - m_treasure.enhance;
			int exp = 0;
			List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
			m_treasures = get_treasures();
			for(int i = 0; i < m_treasures.Count;++i)
			{
				dhc.treasure_t _t_treasure = sys._instance.m_self.get_treasure_guid(m_treasures[i].guid);
				s_t_baowu t_baowu = game_data._instance.get_t_baowu (_t_treasure.template_id);
				exp += t_baowu.exp;
				exp +=  _t_treasure.enhance_exp;
				exp += game_data._instance.get_total_treasure_enhance(_t_treasure.enhance, t_baowu.font_color);
				if(_t_treasure.jilian > 0)
				{
					exp += t_baowu.exp*_t_treasure.jilian;
				}
			}
			for(int i = 1; i <= enhance_up - m_treasure.enhance;i++)
			{
				int gold = game_data._instance.get_total_treasure_enhance(m_treasure.enhance+i,t_treasure.font_color) 
					- game_data._instance.get_total_treasure_enhance(m_treasure.enhance,t_treasure.font_color) - m_treasure.enhance_exp;
				if(exp < gold)
				{
					exp_max =  i -1;
					break;
				}
			}
			enhance_up = Mathf.Min(exp_max,gold_max);
		}
		else
		{
			enhance_up = 0;
		}
		return enhance_up;
	}

	List<dhc.treasure_t> get_treasures()
	{
		List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
		for(int i = 0;i < sys._instance.m_self.get_treasure_num();i ++)
		{
			dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(i);
			s_t_baowu t_treasure = game_data._instance.get_t_baowu(_treasure.template_id);
			if(_treasure.guid == m_treasure.guid || _treasure.role_guid > 0 || t_treasure.font_color >= 5 || _treasure.jilian > 0 || _treasure.star > 0 || _treasure.star_exp > 0)
			{
				continue;
			}
			if(m_select == 2)
			{
				if(t_treasure.font_color == 3 && t_treasure.type !=5 )
				{
					continue;
				}
			}
			else if(m_select == 1)
			{
				if(t_treasure.font_color == 4 && t_treasure.type !=5 )
				{
					continue;
				}
			}
			else if(m_select == 0)
			{
				if((t_treasure.font_color == 4 || t_treasure.font_color == 3)&& t_treasure.type != 5 )
				{
					continue;
				}
			}
			m_treasures.Add(_treasure);
		}
		m_treasures.Sort (comp);
		return m_treasures;
	}

	public static int comp(dhc.treasure_t x, dhc.treasure_t y)
	{
		if (x.template_id == 999999 && y.template_id == 999999)
		{
			return 0;
		}
		else if (x.template_id == 999999 && y.template_id != 999999)
		{
			return -1;
		}
		else if (x.template_id != 999999 && y.template_id == 999999)
		{
			return 1;
		}
		s_t_baowu t_treasure_x = game_data._instance.get_t_baowu(x.template_id);
		s_t_baowu t_treasure_y = game_data._instance.get_t_baowu(y.template_id);
		if (t_treasure_x.font_color < t_treasure_y.font_color)
		{
			return -1;
		}
		else if (t_treasure_x.font_color > t_treasure_y.font_color)
		{
			return 1;
		}
		else if (t_treasure_x.type < t_treasure_y.type)
		{
			return -1;
		}
		else if (t_treasure_x.type > t_treasure_y.type)
		{
			return 1;
		}
		else if (t_treasure_x.id < t_treasure_y.id)
		{
			return -1;
		}
		else if (t_treasure_x.id > t_treasure_y.id)
		{
			return 1;
		}
		else if (x.jilian > y.jilian)
		{
			return -1;
		}
		else if (x.jilian < y.jilian)
		{
			return 1;
		}
		else if (x.enhance > y.enhance)
		{
			return -1;
		}
		else if (x.enhance < y.enhance)
		{
			return 1;
		}
		return 0; 
	}

	void consume_treasue()
	{
		m_treasure_guids.Clear ();
		List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
		m_treasures = get_treasures();
		int exp = 0;
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		int need_exp = game_data._instance.get_total_treasure_enhance(m_treasure.enhance+m_input_num,t_treasure.font_color) 
			- game_data._instance.get_total_treasure_enhance(m_treasure.enhance,t_treasure.font_color) - m_treasure.enhance_exp;
		if(need_exp > 0)
		{
			for(int i = 0; i < m_treasures.Count;++i)
			{
				dhc.treasure_t _t_treasure = sys._instance.m_self.get_treasure_guid(m_treasures[i].guid);
				s_t_baowu t_baowu = game_data._instance.get_t_baowu (_t_treasure.template_id);
				exp += t_baowu.exp;
				exp +=  _t_treasure.enhance_exp;
				exp += game_data._instance.get_total_treasure_enhance(_t_treasure.enhance, t_baowu.font_color);
				if(_t_treasure.jilian > 0)
				{
					exp += t_baowu.exp*_t_treasure.jilian;
				}
				m_treasure_guids.Add(m_treasures[i].guid);
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
			int num = get_max_num ();
			if(num == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_fast_gui.cs_269_46"));//[ffc882]拥有的饰品不足以提升一级
				return;
			}
			if(m_input_num + 1 <= num)
			{
				m_input_num += 1;
				consume_treasue();
				update_ui();
			}
		}
		else if(obj.name == "sub")
		{
			if (m_input_num > 0)
			{
				m_input_num--;
				consume_treasue();
				update_ui();
			}
		}
		else if(obj.name == "add10")
		{
			int num = get_max_num ();
			if(num == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_fast_gui.cs_269_46"));//[ffc882]拥有的饰品不足以提升一级
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
			consume_treasue();
			update_ui();
		}
		else if(obj.name == "sub10")
		{
			if (m_input_num <= 10) 
			{
				m_input_num = 0;
			}
			else
			{
				m_input_num -= 10;
			}
			consume_treasue();
			update_ui();
		}
		else if(obj.name == "queding")
		{
			int num = get_max_num ();
			if(num == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_fast_gui.cs_325_46"));//[ffc882]请选择需要强化的等级数
				return;
			}
			s_message _msg = new s_message ();
			
			_msg.m_type = "treasure_yj_enhance";
			for(int i = 0; i < m_treasure_guids.Count;++i)
			{
				_msg.m_long.Add (m_treasure_guids[i]);
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
		else if(obj.name == "zs")
		{
			if((m_select & 1) != 0)
			{
			 	m_select = m_select & 2;
			}
			else
			{
				m_select = m_select | 1;
			}
			int num = get_max_num ();
			if(m_input_num > num)
			{
				m_input_num = num;
			}
			consume_treasue();
			update_ui();
			m_zs_button.SetActive(!m_zs_button.activeSelf);
		}
		else if(obj.name == "js")
		{
			if((m_select & 2) != 0)
			{
				m_select = m_select & 1;
			}
			else
			{
				m_select = m_select | 2;
			}
			int num = get_max_num ();
			if(m_input_num > num)
			{
				m_input_num = num;
			}
			consume_treasue();
			update_ui();
			m_js_button.SetActive(!m_js_button.activeSelf);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
