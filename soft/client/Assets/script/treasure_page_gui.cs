
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_page_gui : MonoBehaviour {

	public List<GameObject> m_treasure_items = new List<GameObject>();
	public List<ulong> m_hide_guids = new List<ulong>();
	public List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
	
	private int m_cur_page = 0;
	private int m_max_page = 0;
	
	public int m_show_type = 0;
	public bool m_show_lock = true;
	public int m_show = 1;
	public int m_count = 0;
	public GameObject m_des_label;
	public string m_out_message = "click_treasure";
	
	public GameObject m_left;
	public GameObject m_right;
	
	public GameObject m_page_index;
	public bool m_show_remove = false;
	
	private List<uint> m_sp_ids = new List<uint>();
	private int m_dir = 1;
	private int m_type = 1;
	private Vector3 m_mouse_pos;
	private bool m_press = false;
	// Use this for initialization
	void Start () {
	
	}

	void OnEnable() {
		press ();
	}

	public void OnDisable()
	{
		m_out_message = "click_treasure";
	}

	bool is_hide(ulong guid)
	{
		dhc.treasure_t _treasure= sys._instance.m_self.get_treasure_guid (guid);
		
		if(_treasure.locked == 1 && m_show_lock == false)
		{
			return true;
		}
		
		for(int i = 0;i < m_hide_guids.Count;i ++)
		{
			if(m_hide_guids[i] == guid)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void init()
	{
		m_cur_page = 0;
		m_show_lock = true;
		m_show = 1;
		m_show_remove = false;
		m_show_type = 0;
		m_hide_guids.Clear();
		m_treasures.Clear ();
		m_sp_ids.Clear ();
		set_text ("");
		
		if(m_treasure_items.Count == 0)
		{
			for(int y = 0;y < 2;y ++)
			{
				for(int x = 0;x < 4;x ++)
				{
					GameObject _item = game_data._instance.ins_object_res("ui/treasure_page_item");
					_item.name = (y * 4 + x).ToString();
					_item.transform.parent = transform;
					_item.transform.localPosition = new Vector3(x * 210f - 310f,- y * 240f + 110f,0);
					_item.transform.localScale = new Vector3(1,1,1);
					_item.SetActive(true);
					
					m_treasure_items.Add(_item);
				}
			}
		}
	}
	
	public void press()
	{
		m_mouse_pos.x = sys._instance.get_mouse_position ().x;
		m_mouse_pos.y = sys._instance.get_mouse_position ().y;
	}
	
	public void release()
	{
		if(sys._instance.m_pause == true)
		{
			return;
		}
		
		Vector3 _mouse_pos = sys._instance.get_mouse_position ();
		float _x = m_mouse_pos.x - _mouse_pos.x;
		float _y = m_mouse_pos.y - _mouse_pos.y;
		
		if(Mathf.Abs(_x) > Mathf.Abs(_y) && Mathf.Abs(_x) > 50)
		{
			if(m_mouse_pos.x >  _mouse_pos.x && m_right.activeSelf)
			{
				m_cur_page ++;
				m_dir = 1;
				
				if (m_type == 1)
				{
					set_treasure_page_index(m_cur_page);
				}
				else
				{
					set_sp_page_index(m_cur_page);
				}
				
			}
			else if(m_mouse_pos.x <  _mouse_pos.x && m_left.activeSelf)
			{
				m_cur_page --;
				m_dir = -1;
				
				if (m_type == 1)
				{
					set_treasure_page_index(m_cur_page);
				}
				else
				{
					set_sp_page_index(m_cur_page);
				}
			}
		}
	}
	
	public void sp_reset()
	{
		m_type = 2;
		for(int i = 0;i < 8;i ++)
		{
			treasure_page_item _item = m_treasure_items[i].GetComponent<treasure_page_item>();
			_item.set_sp(0,m_dir);
		}
		
		int _frament_num = sys._instance.m_self.get_treasure_fragment_num ();
		
		m_cur_page = 0;
		m_max_page = (_frament_num + 7) / 8;		
		if(m_max_page < 1)
		{
			m_max_page = 1;
		}

		List<uint> _ids = new List<uint>();
		for(int i = 0;i <sys._instance.m_self.m_t_player.item_ids.Count;i ++)
		{
			if(sys._instance.m_self.is_treasure_fragment(sys._instance.m_self.m_t_player.item_ids[i]))
			{
				uint _item_id = sys._instance.m_self.m_t_player.item_ids[i];
				s_t_item _t_item = game_data._instance.get_item ((int)_item_id);
				int _num = sys._instance.m_self.get_item_num (_item_id);
					
				_ids.Add(_item_id);
			}
		}
		_ids.Sort (comp);
		m_sp_ids.AddRange (_ids);
		set_sp_page_index (0);
	}

	public int comp(uint id1, uint id2)
	{
		s_t_item _t_item_x = game_data._instance.get_item ((int)id1);
		s_t_item _t_item_y = game_data._instance.get_item ((int)id2);
		if (_t_item_x.font_color > _t_item_y.font_color)
		{
			return -1;
		}
		else if (_t_item_x.font_color <_t_item_y.font_color)
		{
			return 1;
		}
		else if (_t_item_x.id < _t_item_y.id)
		{
			return -1;
		}
		else if (_t_item_x.id > _t_item_y.id)
		{
			return 1;
		}
		
		return 0; 
	}

	public void treasure_reset()
	{
		for(int i = 0;i < 8;i ++)
		{
			treasure_page_item _item = m_treasure_items[i].GetComponent<treasure_page_item>();
			_item.set_treasure(0,m_dir);
		}
		
		sys._instance.clear_select_treasures (m_count);
		m_treasures.Clear ();
		for(int i = 0;i < sys._instance.m_self.get_treasure_num();i ++)
		{
			dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(i);
			s_t_baowu t_treasure = game_data._instance.get_t_baowu(_treasure.template_id);
			if(m_show == 1)
			{
				if(is_hide(_treasure.guid) || (t_treasure.type != m_show_type && m_show_type != 0) || _treasure.role_guid > 0)
				{
					continue;
				}
			}
			else if(m_show == 2)
			{
				if(is_hide(_treasure.guid) || (t_treasure.type != m_show_type && m_show_type != 0) || _treasure.role_guid > 0 
				   || _treasure.jilian > 0 || _treasure.star > 0 || _treasure.star_exp > 0 )
				{
					continue;
				}
			}
			else if(m_show == 3)
			{
				//dhc.treasure_t _baowu = sys._instance.m_self.get_treasure_guid(m_hide_guids[0]);
				if(is_hide(_treasure.guid) || t_treasure.font_color != 4 || _treasure.role_guid > 0 || _treasure.jilian > 0 || _treasure.enhance > 0
				   || _treasure.star > 0 || _treasure.star_exp > 0)
				{
					continue;
				}
			}
			m_treasures.Add(_treasure);
		}
		m_treasures.Sort (treasure.comp);
		
		m_max_page = (m_treasures.Count + 7) / 8;		
		if(m_max_page < 1)
		{
			m_max_page = 1;
		}
		
		set_treasure_page_index (0);
		
		m_hide_guids.Clear();
		m_type = 1;
	}
	
	public void set_sp_page_index(int id)
	{
		if(id < 0)
		{
			id = 0;
		}
		
		m_right.SetActive(true);
		m_left.SetActive (true);
		
		if(id + 1 < m_max_page)
		{
			m_cur_page = id;
			m_right.SetActive(true);
		}
		else
		{
			m_right.SetActive(false);
		}
		
		if(id == 0)
		{
			m_left.SetActive(false);
		}
		
		for(int i = 0;i < 8;i ++)
		{
			int _id = id * 8 + i;
			treasure_page_item _item = m_treasure_items[i].GetComponent<treasure_page_item>();
			
			if(_id < m_sp_ids.Count)
			{
				_item.set_sp((int)m_sp_ids[_id],m_dir);
				_item.m_out_message = m_out_message;
			}
			else
			{
				_item.set_sp(0,m_dir);
			}
		}
		
		m_cur_page = id;
		page_index ();
	}
	
	public void set_treasure_page_index(int id)
	{
		if(id < 0)
		{
			id = 0;
		}
		
		m_right.SetActive(true);
		m_left.SetActive (true);
		
		if(id + 1 < m_max_page)
		{
			m_cur_page = id;
			m_right.SetActive(true);
		}
		else
		{
			m_right.SetActive(false);
		}
		
		if(id == 0)
		{
			m_left.SetActive(false);
		}
		
		for(int i = 0;i < 8;i ++)
		{
			int _id = id * 8 + i;
			treasure_page_item _item = m_treasure_items[i].GetComponent<treasure_page_item>();
			
			if(_id < m_treasures.Count)
			{
				_item.set_treasure(m_treasures[_id].guid,m_dir);
				_item.m_out_message = m_out_message;
			}
			else
			{
				_item.set_treasure(0,m_dir);
			}
			if(m_show == 2)
			{
				_item.m_jinyan.SetActive(true);
			}
			else
			{
				_item.m_jinyan.SetActive(false);
			}
		}
		
		m_cur_page = id;
		page_index ();
	}
	
	void page_index()
	{
		UISprite _back = m_page_index.transform.Find("back").GetComponent<UISprite>();
		UISprite _bar = m_page_index.transform.Find("bar").GetComponent<UISprite>();
		
		_back.SetDimensions(m_max_page * 20,20);
		
		Vector3 _pos = _bar.transform.localPosition;
		
		_pos.x = m_cur_page * 20;
		_bar.transform.localPosition = _pos;
		
		_pos = m_page_index.transform.localPosition;
		
		_pos.x = - m_max_page * 10;
		
		m_page_index.transform.localPosition = _pos;
	}
	
	void set_treasure_index(int id,ulong guid)
	{
		treasure_page_item _item = m_treasure_items [id].GetComponent<treasure_page_item>();
		_item.set_treasure(guid,m_dir);
	}
	
	public void set_text(string des)
	{
		m_des_label.GetComponent<UILabel>().text = des;
	}
	
	public void click(GameObject obj)
	{
		if(obj.transform.name == "left")
		{
			m_cur_page --;
			m_dir = -1;
			if (m_type == 1)
			{
				set_treasure_page_index(m_cur_page);
			}
			else
			{
				set_sp_page_index(m_cur_page);
			}
		}
		
		if(obj.transform.name == "right")
		{
			m_cur_page ++;
			m_dir = 1;
			if (m_type == 1)
			{
				set_treasure_page_index(m_cur_page);
			}
			else
			{
				set_sp_page_index(m_cur_page);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if(this.gameObject.activeSelf == false)
		{
			return ;
		}
		
		if(sys._instance.get_mouse_button(0) == true)
		{
			if(m_press == false)
			{
				press();
				m_press = true;
			}
		}
		else
		{
			if(m_press == true)
			{
				release();
				m_press = false;
			}
		}
	
	}
}
