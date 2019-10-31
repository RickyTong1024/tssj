
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class item_page_gui : MonoBehaviour {

	public List<GameObject> m_item_items = new List<GameObject>();
	public List<int> m_hide_ids = new List<int>();
	public List<int> m_ids = new List<int>();
	public List<s_t_item> m_items = new List<s_t_item>();
	
	private int m_cur_page = 0;
	private int m_max_page = 0;
	
	public int m_min_color = 0;
	public int m_count = 0;
	public GameObject m_des_label;
	public string m_out_message = "click_item";
	public GameObject m_left;
	public GameObject m_right;
	
	public GameObject m_page_index;
	public bool m_show_remove = false;

	private int m_dir = 1;
	private Vector3 m_mouse_pos;
	private bool m_press = false;
	
	
	// Use this for initialization
	void Start ()
	{
		
	}
	void OnEnable() {
		press ();
	}
	public void OnDisable()
	{
		m_out_message = "click_item";
	}
	
	int hide_num(int id)
	{
		int num = 0;
		s_t_item _item = game_data._instance.get_item(id);		
		for(int i = 0;i < m_hide_ids.Count;i ++)
		{
			if(m_hide_ids[i] == id)
			{
				num++;
			}
		}
		return num;
	}

	
	public void init()
	{
		m_cur_page = 0;
		m_show_remove = false;
		m_min_color = 0;
		m_hide_ids.Clear();
		set_text ("");
		
		if(m_item_items.Count == 0)
		{
			for(int y = 0;y < 2;y ++)
			{
				for(int x = 0;x < 4;x ++)
				{
					GameObject _item = game_data._instance.ins_object_res("ui/item_page_item");
					_item.name = (y * 4 + x).ToString();
					_item.transform.parent = transform;
					_item.transform.localPosition = new Vector3(x * 210f - 310f,- y * 240f + 110f,0);
					_item.transform.localScale = new Vector3(1,1,1);
					_item.SetActive(true);
					
					m_item_items.Add(_item);
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

				set_item_page_index(m_cur_page);
			}
			else if(m_mouse_pos.x <  _mouse_pos.x && m_left.activeSelf)
			{
				m_cur_page --;
				m_dir = -1;

				set_item_page_index(m_cur_page);
			}
		}
	}


	
	public void item_reset()
	{
		for(int i = 0;i < 8;i ++)
		{
			item_page_item _item = m_item_items[i].GetComponent<item_page_item>();
			_item.set_item(0,m_dir);
			_item.m_out_message = m_out_message;
		}
		sys._instance.clear_select_items (m_count);
		m_items.Clear ();
		for(int j = 0; j < m_ids.Count;++j)
		{
			int _num = hide_num(m_ids[j]);
			for(int i = 0;i < sys._instance.m_self.get_item_num((uint)m_ids[j])- _num && i < 100;i ++)
			{
				s_t_item _item = game_data._instance.get_item(m_ids[j]);
				m_items.Add(_item);
			}
		}
		m_items.Sort (comp);
		if(m_items.Count > 100)
		{
			m_items.RemoveRange(100,m_items.Count-100);
		}
		
		m_max_page = (m_items.Count + 7) / 8;		
		if(m_max_page < 1)
		{
			m_max_page = 1;
		}
		
		set_item_page_index (0);
		
		m_hide_ids.Clear();
		m_min_color = 0;
	}

	public void set_item_page_index(int id)
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
			item_page_item _item = m_item_items[i].GetComponent<item_page_item>();
			_item.m_page = m_cur_page;
			if(_id < m_items.Count)
			{
				_item.set_item(m_items[_id].id,m_dir);
				_item.m_out_message = m_out_message;
			}
			else
			{
				_item.set_item(0,m_dir);
			}
		}
		
		m_cur_page = id;
		page_index ();
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
	
	void set_item_index(int id,int guid)
	{
		item_page_item _item = m_item_items [id].GetComponent<item_page_item>();
		_item.set_item (guid,m_dir);
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
			set_item_page_index(m_cur_page);
		}
		
		if(obj.transform.name == "right")
		{
			m_cur_page ++;
			m_dir = 1;
			set_item_page_index(m_cur_page);
		}
	}
	
	public void check(int guid)
	{
		for(int i = 0; i < 8; i++)
		{
			int _id = m_cur_page * 8 + i;
			item_page_item _item = m_item_items[i].GetComponent<item_page_item>();
			if(_id < m_items.Count && m_items[_id].id == guid)
			{
				_item.set_item(m_items[_id].id, 0);
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
