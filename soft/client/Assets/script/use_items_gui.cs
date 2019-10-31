
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class use_items_gui : MonoBehaviour  {
	
	public List<int> types = new List<int>();
	public List<int> values1 = new List<int>();
	public List<int> values2 = new List<int>();
	public List<int> values3= new List<int>();
	public List<Vector3> m_v = new List<Vector3>();
	private List<int> m_rewards_pz;
	public GameObject m_icon_root;
	public bool flag = false;
	private int m_type;
	private int m_num;
	private int _id;
	private float m_time;
	private List<s_t_reward> m_rewards;
	public GameObject m_label;
	public GameObject m_shell;
	public GameObject m_icon;
	private List<GameObject> m_objs;
	private bool m_start;
	public GameObject m_close_panel;
	public GameObject m_next;
	public GameObject m_last;
	public GameObject m_stop_panel;
	private int m_dir = 1;
	private int m_cur_page = 0;
	private int m_max_page = 0;
	private bool m_press = false;
	private Vector3 m_mouse_pos;
	private bool m_stop = false;
	public GameObject m_page_index;
	public UILabel m_hide;
	void Start () 
	{

		
	}
	
	void OnEnable() {
		m_start = false;
		m_close_panel.SetActive (false);
		sys._instance.remove_child (m_icon_root);
		int num = types.Count;
		m_rewards = new List<s_t_reward>();
		m_rewards_pz = new List<int>();
		for (int i = 0; i < num; ++i)
		{
			int pz = 0;
			s_t_reward t_reward = new s_t_reward();
			if (types[i] == 2 && values1[i] != 1)
			{
				s_t_item t_item = game_data._instance.get_item(values1[i]);
				if(t_item.font_color >= 4)
				{
					pz = 1;
				}
			}
			if(types[i] == 4)
			{
				s_t_equip t_equip = game_data._instance.get_t_equip(values1[i]);
				if (t_equip.font_color >= 3)
				{
					pz =1;
				}
			}
			t_reward.type = types[i];
			t_reward.value1 = values1[i];
			t_reward.value2 = values2[i];
			t_reward.value3 = values3[i];
			m_rewards.Add(t_reward);
			m_rewards_pz.Add(pz);
		}
		if (num == 1)
		{
			m_type = 0;
			m_time = 0.4f;
		}
		else
		{
			m_type = 1;
			m_time = num * 0.3f + 0.1f;
		}
		m_num = 0;
		_id = 0;
		m_cur_page = 0;
		m_stop = false;
		m_max_page = (m_rewards.Count + 9) / 10;		
		if(m_max_page < 1)
		{
			m_max_page = 1;
		}
		m_next.SetActive (false);
		m_last.SetActive (false);
		m_objs = new List<GameObject>();
		m_start = true;
		press ();
		if(m_type == 0)
		{
			m_stop_panel.GetComponent<UIButtonMessage>().enabled = false;
		}
		else
		{
			m_stop_panel.GetComponent<UIButtonMessage>().enabled = true;
		}
		m_page_index.SetActive (false);
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
			if(m_mouse_pos.x >  _mouse_pos.x && m_next.activeSelf)
			{
				m_cur_page ++;
				m_dir = 1;
				set_page_index(m_cur_page);
				
			}
			else if(m_mouse_pos.x <  _mouse_pos.x && m_last.activeSelf)
			{
				m_cur_page --;
				m_dir = -1;
				set_page_index(m_cur_page);
			}
		}
	}

	public void init()
	{
		types.Clear();
		values1.Clear();
		values2.Clear();
		values3.Clear();
	}
    void OnDisable()
    {
        init();
    }
	public void click(GameObject obj)
	{
		if(obj.name == "hide")
		{
            s_message _mes = new s_message();
            _mes.m_type = "refresh_zhizhen";
            if (types.Count > 1)
            {
                _mes.m_ints.Add(2);
            }
            else
            {
                _mes.m_ints.Add(1);

            }
            cmessage_center._instance.add_message(_mes);
			if(root_gui._instance.m_tanbao_gui != null && root_gui._instance.m_tanbao_gui.activeSelf)
			{
				s_message _mess = new s_message();
				_mess.m_type = "tanbao_shop";
				cmessage_center._instance.add_message(_mess);
			}

			s_message _mes1 = new s_message();
			_mes1.m_type = "refresh_czfp_ex";
			cmessage_center._instance.add_message(_mes1);

			this.gameObject.SetActive(false);
			sys._instance.m_self.check_huiyi_xuyao ();
		}
		if(obj.transform.name == "last")
		{
			m_cur_page --;
			m_dir = -1;
			set_page_index(m_cur_page);
		}
		
		if(obj.transform.name == "next")
		{
			m_cur_page ++;
			m_dir = 1;
			set_page_index(m_cur_page);
		}
	}

	public void set_page_index(int id)
	{
		if(id < 0)
		{
			id = 0;
		}
		sys._instance.remove_child (m_icon_root);
		m_last.SetActive(true);
		m_next.SetActive (true);
		
		if(id + 1 < m_max_page)
		{
			m_cur_page = id;
			m_next.SetActive(true);
		}
		else
		{
			m_next.SetActive(false);
		}
		
		if(id == 0)
		{
			m_last.SetActive(false);
		}
	
		for(int i = 0;i < 10;i ++)
		{
			int m_id = id * 10 + i;
			if(m_id >= m_rewards.Count)
			{
				break;
			}
			set_card(m_rewards[m_id],m_dir,i);
		}
		
		m_cur_page = id;
		page_index ();
	}

	public void stop(GameObject obj)
	{
		m_stop = true;
	}

	public void set_card(s_t_reward t_reward,int dir,int id)
	{
	
		GameObject icon = icon_manager._instance.create_reward_icon_ex(t_reward.type, t_reward.value1, t_reward.value2,t_reward.value3);
		icon.transform.parent = m_icon_root.transform;
		icon.transform.localPosition = m_v[id];
		icon.transform.localScale = new Vector3(1, 1, 1);
		GameObject obj = (GameObject)Instantiate(m_label);
		obj.transform.parent = icon.transform;
		obj.transform.localPosition = new Vector3(0, 0, 0);
		obj.transform.localScale = new Vector3(1, 1, 1);
		if(t_reward.type == 4)
		{
			
			obj.transform.Find("Label").GetComponent<UILabel>().text  = equip.get_equip_real_name (t_reward.value1);
		}
		else if(t_reward.type == 6)
		{
			obj.transform.Find("Label").GetComponent<UILabel>().text  = treasure.get_treasure_real_name (t_reward.value1);
		}
		else if(t_reward.type == 1)
		{
			obj.transform.Find("Label").GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_reward.type, t_reward.value1, t_reward.value2, t_reward.value3);
		}
		else if(t_reward.type == 2 && t_reward.value1 == 1)
		{
			obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(game_data._instance.get_t_language ("rotary_reward.cs_240_88"), 4);//幸运骰子
		}
		else
		{
			s_t_item item = game_data._instance.get_item (t_reward.value1);
			obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(item.name, item.font_color);
		}
		obj.SetActive(true);
		int num = 0;
		for(int i = 0; i < m_rewards.Count;++i)
		{
			if(m_rewards[i] == t_reward)
			{
				num = i;
				break;
			}
		}
		if (m_rewards_pz[num] == 1)
		{
			obj = (GameObject)Instantiate(m_shell);
			obj.transform.parent = icon.transform;
			obj.transform.localPosition = new Vector3(0, 0, 0);
			obj.transform.localScale = new Vector3(1, 1, 1);
			obj.SetActive(true);
		}
		
		m_objs.Remove(icon);
		m_icon = icon;
		
		if (dir != 0)
		{
			sys._instance.add_alpha_anim (m_icon,0.5f,0f,1f,0);
			sys._instance.add_scale_anim (m_icon,0.5f,0.2f,1.0f,0);
			TweenPosition _ceffect = TweenPosition.Begin(m_icon,0.5f,m_v[id]);
			_ceffect.method = UITweener.Method.EaseInOut;
		}

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

	// Update is called once per frame
	void Update () {

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
		if(m_stop)
		{
			m_page_index.SetActive(true);
			m_close_panel.SetActive (true);
			m_cur_page = m_max_page -1;
			if(m_max_page > 1)
			{
				m_last.SetActive(true);
			}
			else
			{
				m_last.SetActive(false);
			}
			m_next.SetActive(false);
			m_stop_panel.GetComponent<UIButtonMessage>().enabled = false;
			set_page_index(m_cur_page);
			m_objs.Clear();
			m_stop = false;
			m_time = 0;
			return;
		}
		if (!m_start)
		{
			return;
		}
		for (int i = 0; i < m_objs.Count; ++i)
		{
			m_objs[i].transform.localEulerAngles = new Vector3(0, 0, m_objs[i].transform.localEulerAngles.z + Time.deltaTime * 720);
		}
		
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_type == 0)
			{
				if (m_num == 0 && m_time <= 0.2f)
				{
					GameObject icon = icon_manager._instance.create_reward_icon_ex(m_rewards[0].type, m_rewards[0].value1, m_rewards[0].value2, m_rewards[0].value3);
					icon.transform.parent = m_icon_root.transform;
					icon.transform.localPosition = new Vector3(0, -20, 0);
					icon.transform.localScale = new Vector3(1, 1, 1);
					
					m_objs.Add(icon);
					
					TweenPosition effect = sys._instance.add_pos_anim(icon,0.3f, new Vector3(0, 230, 0), 0);
					EventDelegate.Add(effect.onFinished, delegate() 
					                  {
						icon.transform.localEulerAngles = new Vector3(0, 0, 0);
						
						GameObject obj = (GameObject)Instantiate(m_label);
						obj.transform.parent = icon.transform;
						obj.transform.localPosition = new Vector3(0, 0, 0);
						obj.transform.localScale = new Vector3(1, 1, 1);
						if(m_rewards[0].type == 4)
						{
							obj.transform.Find("Label").GetComponent<UILabel>().text  = equip.get_equip_real_name (m_rewards[0].value1);
						}
						else if(m_rewards[0].type == 6)
						{
							obj.transform.Find("Label").GetComponent<UILabel>().text  = treasure.get_treasure_real_name (m_rewards[0].value1);
						}
						else if(m_rewards[0].type == 1)
						{
							obj.transform.Find("Label").GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_rewards[0].type, m_rewards[0].value1, m_rewards[0].value2, m_rewards[0].value3);
						}
						else if(m_rewards[0].type == 2 && m_rewards[0].value1 == 1)
						{
							obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(game_data._instance.get_t_language ("rotary_reward.cs_240_88"), 4);//幸运骰子
						}
						else
						{
							s_t_item item = game_data._instance.get_item (m_rewards[0].value1);
							obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(item.name, item.font_color);
						}
						obj.SetActive(true);
						
						if (m_rewards_pz[0] == 1)
						{
							obj = (GameObject)Instantiate(m_shell);
							obj.transform.parent = icon.transform;
							obj.transform.localPosition = new Vector3(0, 0, 0);
							obj.transform.localScale = new Vector3(1, 1, 1);
							obj.SetActive(true);
						}
						
						m_objs.Remove(icon);
					});
					m_num = 1;
				}
			}
			else
			{
				for (int i = 0; i < m_rewards.Count; ++i)
				{
					if (m_num == i && m_time <= m_rewards.Count * 0.3f - i * 0.3f)
					{
						_id = i;
						if(_id >= 10)
						{
							_id = _id % 10;
						}
						if(i % 10 == 0 && i != 0 && m_num < m_rewards.Count)
						{
							sys._instance.remove_child(m_icon_root);
							m_objs.Clear();
						}
						GameObject icon = icon_manager._instance.create_reward_icon_ex(m_rewards[i].type, m_rewards[i].value1, m_rewards[i].value2, m_rewards[i].value3);
						icon.transform.parent = m_icon_root.transform;
						icon.transform.localPosition = m_v[_id];
						icon.transform.localScale = new Vector3(1, 1, 1);

						m_objs.Add(icon);
						
						Vector3 v = new Vector3(-m_v[_id].x, -m_v[_id].y + 210, -m_v[_id].z);
						
						TweenPosition effect = sys._instance.add_pos_anim(icon,0.3f, v, 0);
						int num = i;
						EventDelegate.Add(effect.onFinished, delegate() 
						                  {
							icon.transform.localEulerAngles = new Vector3(0, 0, 0);
							
							GameObject obj = (GameObject)Instantiate(m_label);
							obj.transform.parent = icon.transform;
							obj.transform.localPosition = new Vector3(0, 0, 0);
							obj.transform.localScale = new Vector3(1, 1, 1);
							if(m_rewards[num].type == 4)
							{

								obj.transform.Find("Label").GetComponent<UILabel>().text  = equip.get_equip_real_name (m_rewards[num].value1);
							}
							else if(m_rewards[num].type == 6)
							{
								obj.transform.Find("Label").GetComponent<UILabel>().text  = treasure.get_treasure_real_name (m_rewards[num].value1);
							}
							else if(m_rewards[num].type == 1)
							{
								obj.transform.Find("Label").GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_rewards[num].type, m_rewards[num].value1, m_rewards[num].value2, m_rewards[num].value3);
							}
							else if(m_rewards[num].type == 2 && m_rewards[num].value1 == 1)
							{
								obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(game_data._instance.get_t_language ("rotary_reward.cs_240_88"), 4);//幸运骰子
							}
							else
							{
								s_t_item item = game_data._instance.get_item (m_rewards[num].value1);
								obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(item.name, item.font_color);
							}
							obj.SetActive(true);
							
							if (m_rewards_pz[num] == 1)
							{
								obj = (GameObject)Instantiate(m_shell);
								obj.transform.parent = icon.transform;
								obj.transform.localPosition = new Vector3(0, 0, 0);
								obj.transform.localScale = new Vector3(1, 1, 1);
								obj.SetActive(true);
							}
							
							m_objs.Remove(icon);
						});
						m_num = i + 1;
					}
				}
				if(m_num == m_rewards.Count)
				{
					m_stop_panel.GetComponent<UIButtonMessage>().enabled = false;
				}
			}
			if (m_time <= 0)
			{
				m_page_index.SetActive(true);
				m_close_panel.SetActive (true);
				m_cur_page = m_max_page -1;
				if(m_max_page > 1)
				{
					m_last.SetActive(true);
				}
				else
				{
					m_last.SetActive(false);
				}
				m_next.SetActive(false);
				page_index ();
			}
		}
	}
}
