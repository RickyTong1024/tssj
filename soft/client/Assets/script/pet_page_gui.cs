
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pet_page_gui : MonoBehaviour {

	public List<GameObject> m_pet_items = new List<GameObject>();
	public List<ulong> m_hide_guids = new List<ulong>();
	public List<int> m_hide_sps = new List<int>();
	public List<pet> m_pets = new List<pet>();
	public int m_show = 0;
	
	private int m_cur_page = 0;
	private int m_max_page = 0;
	
	public int m_min_color = 0;
	
	public GameObject m_des_label;
	public string m_out_message = "click_pet";
	public GameObject m_left;
	public GameObject m_right;
	
	public GameObject m_page_index;
	public bool m_show_remove = false;
	
	public List<uint> m_sp_ids = new List<uint>();
	public  List<uint> m_jy_ids = new List<uint>();
	private int m_dir = 1;
	private int m_type = 1;
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
		m_out_message = "click_pet";
	}
	
	bool is_hide(ulong guid)
	{
		
		pet _pet = sys._instance.m_self.get_pet_guid (guid);		
		for(int i = 0;i < m_hide_guids.Count;i ++)
		{
			if(m_hide_guids[i] == guid)
			{
				return true;
			}
		}
		
		return false;
	}
	
	bool is_hide_sp(int id)
	{
		for(int i = 0;i < m_hide_sps.Count;i ++)
		{
			if(m_hide_sps[i] == id)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void init()
	{
		m_cur_page = 0;
		m_show_remove = false;
		m_min_color = 0;
		m_hide_guids.Clear();
		m_hide_sps.Clear ();
		m_pets.Clear ();
		m_sp_ids.Clear ();
		m_jy_ids.Clear ();
		m_show = 0;
		set_text ("");
		
		if(m_pet_items.Count == 0)
		{
			for(int y = 0;y < 2;y ++)
			{
				for(int x = 0;x < 4;x ++)
				{
					GameObject _item = game_data._instance.ins_object_res("ui/pet_page_item");
					_item.name = (y * 4 + x).ToString();
					_item.transform.parent = transform;
					_item.transform.localPosition = new Vector3(x * 210f - 310f,- y * 240f + 110f,0);
					_item.transform.localScale = new Vector3(1,1,1);
					_item.SetActive(true);
					
					m_pet_items.Add(_item);
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
					set_pet_page_index(m_cur_page);
				}
				else if (m_type == 2)
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
					set_pet_page_index(m_cur_page);
				}
				else if (m_type == 2)
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
			pet_page_item _item = m_pet_items[i].GetComponent<pet_page_item>();
			if(m_min_color == 0)
			{
				_item.set_sp(0,m_dir,true);
			}
			else
			{
				_item.set_sp(0,m_dir);
			}
			_item.m_out_message = m_out_message;
		}
		
		for (int k = 10; k >= 1; --k)
		{
			List<uint> _ids = new List<uint>();
			if(m_min_color == 0)
			{
				for(int i = 0;i <sys._instance.m_self.m_t_player.item_ids.Count;i ++)
				{
					if(sys._instance.m_self.is_pet_fragment(sys._instance.m_self.m_t_player.item_ids[i]))
					{
						uint _item_id = sys._instance.m_self.m_t_player.item_ids[i];
						s_t_item _t_item = game_data._instance.get_item ((int)_item_id);
						if (k != _t_item.def_3)
						{
							continue;
						}
						if(is_hide_sp((int)sys._instance.m_self.m_t_player.item_ids[i]))
						{
							continue;
						}
						_ids.Add(_item_id);
					}
				}
			}
			else
			{
				for(int i = 0;i < game_data._instance.m_dbc_item.get_y();i ++)
				{
					int _item_id = int.Parse(game_data._instance.m_dbc_item.get(0,i));
					if(sys._instance.m_self.is_pet_fragment((uint)_item_id))
					{
						s_t_item _t_item = game_data._instance.get_item ((int)_item_id);
						if (k != _t_item.def_3)
						{
							continue;
						}
						if(is_hide_sp(_item_id))
						{
							continue;
						}
						_ids.Add((uint)_item_id);
					}
				}
			}
			_ids.Sort ();
			m_sp_ids.AddRange (_ids);
		}
		
		
		m_sp_ids.Sort (sp_comp);
		m_cur_page = 0;
		m_max_page = (m_sp_ids.Count + 7) / 8;		
		if(m_max_page < 1)
		{
			m_max_page = 1;
		}
		
		set_sp_page_index (0);
		m_hide_sps.Clear ();
		m_min_color = 0;
	}
	
	public static int sp_comp(uint x, uint y)
	{
		s_t_item t_item = game_data._instance.get_item ((int)x);
		s_t_item t_item1 = game_data._instance.get_item ((int)y);
		s_t_class t_class = game_data._instance.get_t_class (t_item.def_1);
		s_t_class t_class1 = game_data._instance.get_t_class (t_item1.def_1);
		if(t_class.pz > t_class1.pz)
		{
			return -1;
		}
		else if(t_class.pz < t_class1.pz)
		{
			return 1;
		}
		else if(t_class.id > t_class1.id)
		{
			return -1;
		}
		else if(t_class.id < t_class1.id)
		{
			return 1;
		}
		return 0;
	}
	
	public void pet_reset()
	{
		m_type = 1;
		for(int i = 0;i < 8;i ++)
		{
			pet_page_item _item = m_pet_items[i].GetComponent<pet_page_item>();
			_item.set_pet(0,m_dir);
			_item.m_out_message = m_out_message;
		}
		m_pets.Clear ();
		for(int i = 0;i < sys._instance.m_self.get_pet_num();i ++)
		{
			pet _pet = sys._instance.m_self.get_pet_index(i);
			if(is_hide(_pet.get_guid()) || _pet.get_color() < m_min_color)
			{
				continue;
			}
			m_pets.Add(_pet);
		}
		m_pets.Sort (comp);

		
		m_max_page = (m_pets.Count + 7) / 8;		
		if(m_max_page < 1)
		{
			m_max_page = 1;
		}
		
		set_pet_page_index (0);
		
		m_hide_guids.Clear();
		m_min_color = 0;
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
			pet_page_item _item = m_pet_items[i].GetComponent<pet_page_item>();
			
			if(_id < m_sp_ids.Count)
			{
				if(m_min_color == 0)
				{
					_item.set_sp((int)m_sp_ids[_id],m_dir,true);
				}
				else
				{
					_item.set_sp((int)m_sp_ids[_id],m_dir);
				}
				_item.m_out_message = m_out_message;
			}
			else
			{
				if(m_min_color == 0)
				{
					_item.set_sp(0,m_dir,true);
				}
				else
				{
					_item.set_sp(0,m_dir);
				}
			}
		}
		
		m_cur_page = id;
		page_index ();
	}
	
	public void set_pet_page_index(int id)
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
			pet_page_item _item = m_pet_items[i].GetComponent<pet_page_item>();
			
			if(_id < m_pets.Count)
			{
				_item.set_pet(m_pets[_id].get_guid(),m_dir);
				_item.m_out_message = m_out_message;
			}
			else
			{
				_item.set_pet(0,m_dir);
			}
			if(m_show == 1)
			{
				_item.m_zhanli.SetActive(true);
			}
			else
			{
				_item.m_zhanli.SetActive(false);
			}
		}
		
		m_cur_page = id;
		page_index ();
	}
	
	public static int comp(pet x, pet y)
	{
		s_t_pet _pet_x = x.m_t_pet;
		s_t_pet _pet_y = y.m_t_pet;
		if (_pet_x.color > _pet_y.color)
		{
			return -1;
		}
		else if (_pet_x.color < _pet_y.color)
		{
			return 1;
		}
		else if (x.get_star() > y.get_star())
		{
			return -1;
		}
		else if (x.get_star() < y.get_star())
		{
			return 1;
		}
		else if (x.get_level() > y.get_level())
		{
			return -1;
		}
		else if (x.get_level() < y.get_level())
		{
			return 1;
		}
		else if (x.get_template_id() < y.get_template_id())
		{
			return -1;
		}
		else if (x.get_template_id() > y.get_template_id())
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
	
	void set_pet_index(int id,ulong guid)
	{
		pet_page_item _item = m_pet_items [id].GetComponent<pet_page_item>();
		_item.set_pet (guid,m_dir);
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
				set_pet_page_index(m_cur_page);
			}
			else if (m_type == 2)
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
				set_pet_page_index(m_cur_page);
			}
			else if (m_type == 2)
			{
				set_sp_page_index(m_cur_page);
			}
		}
	}
	
	public void check(ulong guid)
	{
		for(int i = 0; i < 8; i++)
		{
			int _id = m_cur_page * 8 + i;
			pet_page_item _item = m_pet_items[i].GetComponent<pet_page_item>();
			if(_id < m_pets.Count && m_pets[_id].get_guid() == guid)
			{
				_item.set_pet(m_pets[_id].get_guid(), 0);
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
