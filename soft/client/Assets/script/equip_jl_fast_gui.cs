
using UnityEngine;
using System.Collections;

public class equip_jl_fast_gui : MonoBehaviour {

	public dhc.equip_t m_equip;
	public GameObject m_icon;
	public UILabel m_name;
	public UILabel m_jinglian;
	public UILabel m_cl;
	public UILabel m_input;
	public UILabel m_gold;

	public int m_input_num = 1;
	private uint m_stone_id = 50070001;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_equip_icon(m_equip.guid);
		_icon.transform.GetComponent<BoxCollider>().enabled = false;
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		m_name.text = equip.get_equip_real_name (m_equip.template_id);
		m_jinglian.text = m_equip.jilian.ToString ();
		update_ui ();
	}

	public void update_ui()
	{
		m_input.text = m_input_num.ToString ();
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		int cl = 0;
		int gold = 0;
		for(int i = 1; i <= m_input_num;++i)
		{
			s_t_equip_jl _equip_jl = game_data._instance.get_t_equip_jl (m_equip.jilian +i);
			cl += _equip_jl.stones[t_equip.font_color -1];
			gold += _equip_jl.golds[t_equip.font_color -1];
		}
		string color = "[ff0000]";
		if( sys._instance.m_self.get_item_num(m_stone_id) >= cl)
		{
			color = "[fff05c]";
		}
		m_cl.text = color + cl.ToString ();
		color = "[ff0000]";
		if(sys._instance.m_self.m_t_player.gold >= gold)
		{
			color = sys._instance.get_res_color(1);
		}
		m_gold.text = color + gold.ToString ();
	}

	int get_max_num()
	{
		int enhance_up = game_data._instance.m_dbc_equip_jl.get_y();
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		if(enhance_up > m_equip.jilian)
		{
			int gold_max = enhance_up - m_equip.jilian;
			int gold = 0;
			for(int i = 1; i <= enhance_up - m_equip.jilian;i++)
			{
				s_t_equip_jl _equip_jl = game_data._instance.get_t_equip_jl (m_equip.jilian +i);
				gold += _equip_jl.golds[t_equip.font_color -1];
				if(sys._instance.m_self.m_t_player.gold < gold)
				{
					gold_max = i-1;
					break;
				}
			}
			int stone_num = sys._instance.m_self.get_item_num(m_stone_id);
			int cl_max = enhance_up - m_equip.jilian;
			int cl = 0;
			for(int i = 1; i <= enhance_up - m_equip.jilian;i++)
			{
				s_t_equip_jl _equip_jl = game_data._instance.get_t_equip_jl (m_equip.jilian +i);
				cl += _equip_jl.stones[t_equip.font_color -1];
				if(stone_num < cl)
				{
					cl_max = i-1;
					break;
				}
			}
			enhance_up = Mathf.Min(cl_max,gold_max);
		}
		else
		{
			enhance_up = 0;
		}
		return enhance_up;
	}

	public void click(GameObject obj)
	{
		if (obj.name == "add") 
		{
			if(m_input_num + 1 <= get_max_num())
			{
				m_input_num += 1;
				update_ui();
			}
		}
		else if(obj.name == "sub")
		{
			if (m_input_num > 1)
			{
				m_input_num--;
				update_ui();
			}
		}
		else if(obj.name == "add10")
		{
			if(m_input_num +10 > get_max_num())
			{
				m_input_num = get_max_num();
			}
			else
			{
				m_input_num += 10;
			}
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
			update_ui();
		}
		else if(obj.name == "queding")
		{
			s_message _msg = new s_message ();
			
			_msg.m_type = "equip_yj_jinglian";
			_msg.m_ints.Add (m_input_num);
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
