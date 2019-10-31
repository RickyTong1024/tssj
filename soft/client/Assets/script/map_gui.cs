
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class map_gui : MonoBehaviour,IMessage {
	
	public GameObject m_grid;
	public GameObject m_select_mission;
	public GameObject m_right;
	public GameObject m_left;
	public GameObject m_saodang;
	public GameObject m_tili;
	public GameObject m_jewel;
	public GameObject m_tishi;
	private s_t_map m_t_map;
	public GameObject m_num;
	public GameObject m_pt_button;
	public GameObject m_jy_button;
	public GameObject m_first_mission_gui;
	private int m_pt_cur = 0;
	private int m_jy_cur = 0;

	public GameObject m_buttle_tip;
	public GameObject m_map_name;
	public GameObject m_lock;
	public GameObject m_switch;
	public GameObject m_pt;
	public GameObject m_jy;
	public GameObject m_back;
	public GameObject m_bar;

	//public List<Texture> m_fimages = new List<Texture>();
	//public List<Texture> m_alpha = new List<Texture>();
	public GameObject m_left_bx;
	private int m_left_bx_map;
	public GameObject m_right_bx;
	private int m_right_bx_map;
	public GameObject m_pt_point;
	public GameObject m_jy_point;
	public GameObject m_image;
	public GameObject m_image1;
	public GameObject m_image_clip;
	public List<GameObject> m_rimage = new List<GameObject>();
	private List<s_t_mission> m_t_missions = new List<s_t_mission>();
	private Vector3 m_old_position;
	public GameObject m_left_move;
	public GameObject m_mission_panel;
	public GameObject m_root;
	public GameObject m_drag;
	bool m_need_update_ui = false;
	bool m_is_jy = false;
	bool m_is_click = false;

	private List<s_t_map> m_pt_list = new List<s_t_map>();
	private List<s_t_map> m_jy_list = new List<s_t_map>();
	//private int count=0;
	private Vector3 m_mouse_pos;
	private bool m_is_start = false;
	private int m_select = 0;
	private int m_max_pt_id = 0;
	private int m_max_jy_id = 0;
	private int m_level = 0;
	public GameObject m_jybg;
	int m_index = 0;
	int mission_id = 0;
	public GameObject m_map_end;
	public GameObject m_map_sz;
	public GameObject m_map_jiesuo;
	bool m_jiesuo = false;
	s_t_mission m_max_mission;
	s_t_mission m_last_mission;
	public GameObject m_image_text;
	public GameObject m_jl;
	public List<GameObject> m_icons;
	public List<GameObject> m_first_icons;
	public GameObject m_mystar;
	public GameObject m_get;
	public GameObject m_guanbi;
	public GameObject m_ylq;
	public GameObject m_first_ylq;
	public GameObject m_first_guanbi;
	public GameObject m_first_get;


    public GameObject m_qiyu_gui;
	// Use this for initialization
	void Start ()
	{
		cmessage_center._instance.add_handle (this);
		for(int i = 0;i < game_data._instance.m_dbc_map.get_y();i ++)
		{
			int _id = int.Parse(game_data._instance.m_dbc_map.get(0,i));
			s_t_map _map = game_data._instance.get_t_map(_id);
			if(_map.id > 10000)
			{
				m_jy_list.Add(_map);
				if(sys._instance.m_self.m_t_player.mission_jy >= _map.jypid && sys._instance.m_self.m_t_player.mission >= _map.pid && 
				   sys._instance.m_self.m_t_player.level >= _map.level)
				{
					m_jy_cur ++;
				}
			}
			else
			{
				m_pt_list.Add(_map);
				if(sys._instance.m_self.m_t_player.mission_jy >= _map.jypid && sys._instance.m_self.m_t_player.mission >= _map.pid && 
				   sys._instance.m_self.m_t_player.level >= _map.level)
				{
					m_pt_cur ++;
				}
			}
		}
		if(m_jy_cur > 0)
		{
			m_jy_cur --;
		}
		if(m_pt_cur > 0)
		{
			m_pt_cur --;
		}

		m_is_start = true;
		m_is_jy = false;
		m_pt.SetActive(false);
		m_pt_button.SetActive(true);
		m_jy.SetActive (true);
		m_jy_button.SetActive(false);
		OnEnable ();

		m_max_pt_id = sys._instance.m_self.m_t_player.mission;
		m_max_jy_id = sys._instance.m_self.m_t_player.mission_jy;
		m_level = sys._instance.m_self.m_t_player.level;
        m_image.SetActive(true);
        m_image_clip.SetActive(true);
        m_image_text.SetActive(true);

       
    }

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	bool is_lock()
	{
		if(sys._instance.m_pause == true 
		   || m_map_jiesuo.activeSelf
		   || m_map_end.activeSelf)
		{
			return true;
		}

		return false;
	}
	public void press()
	{
		m_mouse_pos.x = sys._instance.get_mouse_position ().x;
		m_mouse_pos.y = sys._instance.get_mouse_position ().y;
	}

	public void release()
	{
		if (is_lock())
		{
			return;
		}
		Vector3 _mouse_pos = sys._instance.get_mouse_position ();
		int _add = 0;
		float _x = m_mouse_pos.x - _mouse_pos.x;
		float _y = m_mouse_pos.y - _mouse_pos.y;

		if(Mathf.Abs(_x) > Mathf.Abs(_y) && Mathf.Abs(_x) > 50)
		{
			if(m_mouse_pos.x >  _mouse_pos.x)
			{
				_add = 1;
			}
			else if(m_mouse_pos.x <  _mouse_pos.x)
			{
				_add = -1;
			}

			if(get_cur() != add_cur(_add))
			{
				reset_map();
			}
		}
	}

	public void OnEnable()
	{
        
		if (!m_is_start)
		{
			return;
		}
		m_saodang.SetActive (false);
		m_buttle_tip.SetActive (false);
		reset_map ();

		s_message _message2 = new s_message();
		_message2.m_type = "check_bf";
		cmessage_center._instance.add_message(_message2);

		root_gui._instance.show_mask();
		sys._instance.m_hall_root.SetActive(false);
		sys._instance.m_map.SetActive (true);

    }

	void get_t_map()
	{
		if(m_is_jy == true)
		{

            if (sys._instance.m_self.m_t_player.qiyu_mission.Count != 0)
            {
                m_qiyu_gui.SetActive(true);
            }
            else
            {
                m_qiyu_gui.SetActive(false);

            }
			m_t_map = m_jy_list[get_cur()];
		}
		else
		{
            m_qiyu_gui.SetActive(false);
			m_t_map = m_pt_list[get_cur()];
		}
	}

	void set_t_map(int map_id)
	{
		m_t_map = game_data._instance.get_t_map(map_id);
		if (m_t_map.id > 10000)
		{
			m_is_jy = true;
		}
		else
		{
			m_is_jy = false;
		}
		if(m_is_jy)
		{
			m_pt.SetActive(true);
			m_pt_button.SetActive(false);
			m_jy.SetActive(false);
			m_jy_button.SetActive(true);
			for (int i = 0; i < m_jy_list.Count; ++i)
			{
				if (m_jy_list[i].id == map_id)
				{
					m_jy_cur = i;
					break;
				}
			}
		}
		else
		{
			m_pt.SetActive(false);
			m_pt_button.SetActive(true);
			m_jy.SetActive(true);
			m_jy_button.SetActive(false);
			for (int i = 0; i < m_pt_list.Count; ++i)
			{
				if (m_pt_list[i].id == map_id)
				{
					m_pt_cur = i;
					break;
				}
			}
		}
		reset_map ();
	}

	int get_cur()
	{
		if(m_is_jy == true)
		{
			return m_jy_cur;
		}
		return m_pt_cur;
	}

	int add_cur(int cur)
	{
		if (cur == 1 && !is_unlock())
		{
			if(m_is_jy)
			{
				return m_jy_cur;
			}
			else
			{
				return m_pt_cur;
			}
		}
		m_index = 0;

		int _min = 0;

		

		if(m_is_jy)
		{
			m_jy_cur += cur;

			if(m_jy_cur >= m_jy_list.Count - 1)
			{
				m_jy_cur = m_jy_list.Count - 1;
				m_right.SetActive(false);
			}
			else 
			{
				m_right.SetActive(true);
			}

			if(m_jy_cur <= _min)
			{
				m_jy_cur = _min;
				m_left.SetActive(false);
			}
			else
			{
				m_left.SetActive(true);
			}
			return m_jy_cur;
		}
		else
		{
			m_pt_cur += cur;
			if(m_pt_cur >= m_pt_list.Count - 1)
			{
				m_pt_cur = m_pt_list.Count - 1;
				m_right.SetActive(false);
			}
			else 
			{
				m_right.SetActive(true);
			}
			if(m_pt_cur <= _min)
			{
				m_pt_cur = _min;
				m_left.SetActive(false);
			}
			else
			{
				m_left.SetActive(true);
			}
			return m_pt_cur;
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name.Length >= 6 && obj.name.Substring(0, 6) == "rimage")
		{
			if(obj.name == "rimage1")
			{
				m_select = 0;
			}
			if(obj.name == "rimage2")
			{
				m_select = 1;
			}
			if(obj.name == "rimage3")
			{
				m_select = 2;
			}

			reset_jl(m_select);
			m_jl.SetActive(true);
		}

		if(obj.name == "add_tili")
		{
			int item_id = 10010002;
			int num = sys._instance.m_self.get_item_num((uint)item_id);
			if(num> 0)
			{
				root_gui._instance.show_tili_dialog_box(item_id);
				return;
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "buy_num_gui";
				_message.m_ints.Add(100200);
				_message.m_ints.Add(2);
				cmessage_center._instance.add_message(_message);
				
			}
			return;
		}

		if(obj.name == "lq")
		{
			protocol.game.cmsg_mission_reward _msg = new protocol.game.cmsg_mission_reward ();
			_msg.map_id = m_t_map.id;
			_msg.reward_id = m_select;
			net_http._instance.send_msg<protocol.game.cmsg_mission_reward> (opclient_t.CMSG_MISSION_REWARD, _msg);
		}
		if(obj.name == "jl_close" || obj.name == "guanbi")
		{
			m_jl.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.name == "close")
		{
            s_message _message = new s_message();
            _message.m_type = "show_main_gui";
            cmessage_center._instance.add_message(_message);
            m_jy_cur = 0;
			m_pt_cur = 0;
			for(int i = 0;i < game_data._instance.m_dbc_map.get_y();i ++)
			{
				int _id = int.Parse(game_data._instance.m_dbc_map.get(0,i));
				s_t_map _map = game_data._instance.get_t_map(_id);
				if(_map.id > 10000)
				{
					if(sys._instance.m_self.m_t_player.mission_jy >= _map.jypid && sys._instance.m_self.m_t_player.mission >= _map.pid && 
					   sys._instance.m_self.m_t_player.level >= _map.level)
					{
						m_jy_cur ++;
					}
				}
				else
				{
					if(sys._instance.m_self.m_t_player.mission_jy >= _map.jypid && sys._instance.m_self.m_t_player.mission >= _map.pid && 
					   sys._instance.m_self.m_t_player.level >= _map.level)
					{
						m_pt_cur ++;
					}
				}
			}
			if(m_jy_cur > 0)
			{
				m_jy_cur --;
			}
			if(m_pt_cur > 0)
			{
				m_pt_cur --;
			}
			root_gui._instance.show_mask();
			sys._instance.m_hall_root.SetActive(true);
			sys._instance.m_map.SetActive (false);
			Object.Destroy(this.gameObject);
		}
		if(obj.name == "right")
		{
			if (get_cur() != add_cur(1))
			{
				reset_map();
			}
		}
		if(obj.name == "left")
		{
			add_cur(-1);
			reset_map();
		}
		if(obj.name == "switch")
		{
			show_mission(m_last_mission.id);
		}
		if(obj.name == "jy" || obj.name == "pt")
		{
			if (obj.name == "jy" && sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jy)
			{
				root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("juntuan_panel.cs_367_72") ,(int)e_open_level.el_jy));//[ffc882]该功能{0}级开启
				return;
			}

			root_gui._instance.m_mask.GetComponent<UIPanel>().alpha = 1.0f;
			root_gui._instance.m_mask.SetActive (true);
			m_is_jy = !m_is_jy;
			get_t_map();
			set_t_map(m_t_map.id);
		}
		if(obj.name == "map_sz")
		{
			m_map_sz.SetActive(false);
			m_map_end.SetActive(true);
		}
		if (obj.name == "map_end")
		{
			m_map_end.SetActive(false);

			if(m_is_jy)
			{				
				if(m_jy_cur >= m_jy_list.Count - 1)
				{
					return;
				}
			}
			else
			{
				if(m_pt_cur >= m_pt_list.Count - 1)
				{
					return;
				}
			}

			if (get_cur() != add_cur(1))
			{
				reset_map();
			}
			if (!is_unlock())
			{
				return;
			}
			m_jiesuo = true;
		}
		if (obj.name == "mission_reward")
		{
			m_first_mission_gui.SetActive(true);
			int id = int.Parse(obj.transform.parent.name);
			mission_id = id;
			reset_first_reward(id);
		}
		if(obj.transform.name == "mission_close" || obj.transform.name == "first_guanbi")
		{
			m_first_mission_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "first_yq")
		{
			protocol.game.cmsg_mission_first _msg = new protocol.game.cmsg_mission_first ();
			_msg.mission_id = mission_id;
			net_http._instance.send_msg<protocol.game.cmsg_mission_first> (opclient_t.CMSG_MISSION_FIRST, _msg);
		}
		if(obj.transform.name == "left_bx")
		{
			set_t_map(m_left_bx_map);
		}
		if(obj.transform.name == "right_bx")
		{
			set_t_map(m_right_bx_map);
		}
	}

	void reset_first_reward(int id)
	{
		bool flag = false;
		s_t_mission_first_reward _t_mission = game_data._instance.get_t_mission_first_reward (id);
		for(int i = 0; i < sys._instance.m_self.m_t_player.mission_rewards.Count;++i)
		{
			if(id == sys._instance.m_self.m_t_player.mission_rewards[i])
			{
				flag = true;
				break;
			}
		}
		if(flag)
		{
			m_first_guanbi.SetActive(true);
			m_first_get.SetActive(false);
			m_first_ylq.SetActive(true);
		}
		else if(!flag)
		{
			if(sys._instance.m_self.get_mission_star(_t_mission.id) > 0)
			{
				m_first_guanbi.SetActive(false);
				m_first_get.SetActive(true);
				m_first_ylq.SetActive(false);
				m_first_get.GetComponent<BoxCollider>().enabled = true;
				m_first_get.GetComponent<UISprite>().set_enable(true);
			}
			else
			{
				m_first_guanbi.SetActive(false);
				m_first_get.SetActive(true);
				m_first_ylq.SetActive(false);
				m_first_get.GetComponent<BoxCollider>().enabled = false;
				m_first_get.GetComponent<UISprite>().set_enable(false);
			}
		}
		int num = 0;
		for(int i = 0 ;i < _t_mission.rewards.Count;++i)
		{
			num++;
			m_first_icons[i].SetActive(true);
			GameObject icon = m_first_icons[i];
			sys._instance.remove_child(icon);
			GameObject _icon = icon_manager._instance.create_reward_icon ( _t_mission.rewards[i].type,  _t_mission.rewards[i].value1,
			                                                              _t_mission.rewards[i].value2,_t_mission.rewards[i].value3);
			_icon.transform.parent = icon.transform;
			_icon.transform.localPosition = new Vector3 (0, 0, 0);
			_icon.transform.localScale = new Vector3 (1, 1, 1);
		}
		for(int i = num ; i < m_first_icons.Count;++i)
		{
			m_first_icons[i].SetActive(false);
		}
		if(num == 1)
		{
			m_first_icons[num - 1].transform.localPosition = new Vector3(0,-18,0); 
		}
		for(int i = 0; i < num;++i)
		{
			if(num == 2)
			{
				m_first_icons[i].transform.localPosition = new Vector3(-80+ i*160,-18,0); 
			}
			if(num == 3)
			{
				m_first_icons[i].transform.localPosition = new Vector3(-150+ i*150,-18,0); 
			}
			if(num == 4)
			{
				m_first_icons[i].transform.localPosition = new Vector3(-160+ i*107,-18,0); 
			}
		}
	}

	void reset_jl(int select)
	{
		int star_sum = 0;

		star_sum = m_t_map.stars[select].star_num;

		m_mystar.GetComponent<UILabel>().text = sys._instance.m_self.get_map_star (m_t_map.id).ToString ()+ "/" + star_sum.ToString();
		for(int i = 0; i < 3;++i)
		{
			if(m_t_map.stars[i].star_num == 0)
			{
				continue;
			}
			if(sys._instance.m_self.get_map_star (m_t_map.id) >= m_t_map.stars[i].star_num && !is_click(i) )
			{
				break;
			}
		}

		if(sys._instance.m_self.get_map_star (m_t_map.id) >= m_t_map.stars[select].star_num )
		{
			if(!is_click(select))
			{
				m_guanbi.SetActive(false);
				m_get.SetActive(true);
				m_ylq.SetActive(false);
				m_get.GetComponent<UISprite>().set_enable(true);
				m_get.GetComponent<BoxCollider>().enabled = true;
			}
			else
			{
				m_guanbi.SetActive(false);
				m_get.SetActive(true);
				m_ylq.SetActive(true);
				m_get.GetComponent<UISprite>().set_enable(false);
				m_get.GetComponent<BoxCollider>().enabled = false;
			}
		}
		else
		{
			m_guanbi.SetActive(true);
			m_get.SetActive(false);
			m_ylq.SetActive(false);
		}
		for(int i = 0; i < m_icons.Count;++i)
		{
			m_icons[i].SetActive(false);
		}
		int _id = 0;
		for ( int i = 0; i <  m_t_map.stars[select].rewards.Count; ++i)
		{
			GameObject icon = m_icons[_id];
			sys._instance.remove_child(icon);
			if(m_t_map.stars[select].rewards[i].type == 0)
			{
				continue;
			}
			m_icons[_id].SetActive(true);
			_id ++;
			GameObject _icon = icon_manager._instance.create_reward_icon (m_t_map.stars[select].rewards[i].type, m_t_map.stars[select].rewards[i].value1,
			                                                              m_t_map.stars[select].rewards[i].value2, m_t_map.stars[select].rewards[i].value3);
			_icon.transform.parent = icon.transform;
			_icon.transform.localPosition = new Vector3 (0, 0, 0);
			_icon.transform.localScale = new Vector3 (1, 1, 1);
		}
	}

	bool is_unlock()
	{
		if(sys._instance.m_self.m_t_player.mission_jy < m_t_map.jypid || sys._instance.m_self.m_t_player.mission < m_t_map.pid
		   || sys._instance.m_self.m_t_player.level < m_t_map.level)
		{
			return false;
		}
		return true;
	}

	void show_mission(int id)
	{
		s_t_mission _mission = game_data._instance.get_t_mission (id);
		show_buttle_id(id);
	}

	void reset_map()
	{

		get_t_map ();
		
		if(m_t_map == null)
		{
			return;
		}
		
		int _id = m_t_map.id;
		
		if(_id > 10000)
		{
			_id -= 10000;
		}
		
		s_message _message = new s_message();
		
		_message.m_type = "select_star_level";
		_message.m_ints.Add(_id - 1);
		cmessage_center._instance.add_message(_message);
		m_root.SetActive(false);
		m_map_name.GetComponent<UILabel>().text = m_t_map.name;
		m_lock.SetActive (!is_unlock());
		m_switch.SetActive (is_unlock());

		sys._instance.m_map.SetActive (true);

		if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jy)
		{
			m_jy.SetActive(true);
			m_jy_button.SetActive(false);
			m_jy.GetComponent<UISprite>().set_enable(false);
			m_jy.GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			m_jy.SetActive(true);
			m_jy_button.SetActive(false);
			m_jy.GetComponent<UISprite>().set_enable(true);
			m_jy.GetComponent<BoxCollider>().enabled = true;
			if(m_is_jy)
			{
				m_jy.SetActive(false);
				m_jy_button.SetActive(true);
			}
		}
		effect ();
		point_effect ();
		m_map_sz.GetComponent<UIButtonMessage>().enabled = false;
		init ();
	}

	bool map_exit()
	{
		for(int i = 0 ; i < sys._instance.m_self.m_t_player.map_ids.Count; ++i)
		{
			if(m_t_map.id ==  sys._instance.m_self.m_t_player.map_ids[i])
			{
				return true;
			}
		}
		return false;
	}

	string get_box_name(int id,int num)
	{
		if(id == 0)
		{
			if(num == 3)
			{
				return "green_box_b03";
			}
			return "green_box_b" + num;
		}
		if(id == 1)
		{
			if(num == 3)
			{
				return "blue_box_b03";
			}
			return "blue_box_b" + num;
		}
		if(id == 2)
		{
			if(num == 3)
			{
				return "purple_box_b03";
			}
			return "purple_box_b" +num;
		}
		if(id == 3)
		{
			if(num == 3)
			{
				return "gold__box_b03";
			}
			return "gold_box_b" + num;
		}
		if(id == 4)
		{
			if(num == 3)
			{
				return "orange_box_b03";
			}
			return "orange_box_b" + num;
		}
		return "";
	}

	void init()
	{
		int count = 0;
		count = chapter (m_t_map.id);
		for(int i = 0 ; i < 5; ++i)
		{
			m_rimage[i].SetActive(false);
			m_rimage[i].GetComponent<BoxCollider>().enabled = true;
			m_rimage[i].transform.GetComponent<UISpriteAnimation>().enabled = false;
			if(is_click(i))
			{
				m_rimage[i].transform.GetComponent<UISprite>().spriteName = get_box_name(i,5); 
			}
			else
			{
				m_rimage[i].transform.GetComponent<UISprite>().spriteName = get_box_name(i,1); 
			}
		}
		if(!map_exit() || !is_unlock())
		{
			for(int i = 0; i < m_t_map.stars.Count;++i)
			{
				if(m_t_map.stars[i].star_num == 0)
				{
					m_rimage[i].SetActive(false);
					continue;
				}
				m_rimage[i].SetActive(true);
				m_rimage[i].transform.GetComponent<UISprite>().spriteName = get_box_name(i,1); 
				num (i);
			}
		}
		else
		{
			for(int i = 0 ; i < m_t_map.stars.Count; ++i)
			{
				if(m_t_map.stars[i].star_num == 0)
				{
					m_rimage[i].SetActive(false);
					continue;
				}
				m_rimage[i].SetActive(true);
				if(sys._instance.m_self.get_map_star (m_t_map.id) >= m_t_map.stars[i].star_num && !is_click(i))
				{
					m_rimage[i].transform.GetComponent<UISprite>().spriteName = get_box_name(i,3); 
					m_rimage[i].GetComponent<BoxCollider>().enabled = true;
					m_rimage[i].transform.GetComponent<UISpriteAnimation>().enabled = true;
				}

				num (i);
			}
		}
		int num_star = sys._instance.m_self.get_map_star (m_t_map.id);
		int star_sum = 0;
		int star_count = 0;
		int num_ = 3;
		for(int i = 0; i < m_t_map.stars.Count;++i)
		{
			if(m_t_map.stars[i].star_num == 0)
			{
				continue;
			}
			star_count ++;
			num_ -= 1;
			star_sum = m_t_map.stars[i].star_num;
			m_num.GetComponent<UILabel>().text = num_star.ToString() + "/" + star_sum.ToString();
		}
		float m_width = (float)m_bar.GetComponent<UISprite>().width;
		for(int i = 0; i < m_t_map.stars.Count;++i)
		{
			if(m_t_map.stars[i].star_num == 0)
			{
				continue;
			}
			float value = (float)m_t_map.stars[i].star_num /star_sum;
			m_rimage[i].transform.localPosition = m_bar.transform.localPosition+new Vector3 ((value*m_width), 30 , 0);
		}
		m_bar.GetComponent<UIProgressBar>().value = (float)num_star / star_sum;

    }

	void num(int value)
	{
		int num = sys._instance.m_self.get_map_star (m_t_map.id);
		int star = m_t_map.stars[value].star_num;

		m_rimage[value].transform.Find("num").GetComponent<UILabel>().text = star.ToString();

	}

	void reset()
	{
		show_select_mission();
		m_need_update_ui = true;
        
        if (m_is_jy)
		{
			m_jybg.gameObject.SetActive(true);
			if (sys._instance.m_self.m_t_player.mission < m_t_map.pid)
			{
				
				m_tishi.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("map_gui.cs_928_67"), game_data._instance.get_t_map(m_t_map.id-10000).name);//解锁条件：通关{0}
			}
			else if(sys._instance.m_self.m_t_player.mission_jy < m_t_map.jypid)
			{
			
				m_tishi.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("map_gui.cs_928_67"), game_data._instance.get_t_map(m_t_map.id - 1).name);//解锁条件：通关{0}
			}
			else if (sys._instance.m_self.m_t_player.level < m_t_map.level)
			{
				m_tishi.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("map_gui.cs_939_67"), m_t_map.level.ToString());//解锁条件：达到{0}级
			}
		}
		else
		{
			m_jybg.gameObject.SetActive(false);
			if (sys._instance.m_self.m_t_player.mission < m_t_map.pid)
			{
				
				m_tishi.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("map_gui.cs_928_67"), game_data._instance.get_t_map(m_t_map.id - 1).name);//解锁条件：通关{0}
			}
			else if (sys._instance.m_self.m_t_player.level < m_t_map.level)
			{
				m_tishi.transform.GetComponent<UILabel>().text =string.Format(game_data._instance.get_t_language ("map_gui.cs_939_67"), m_t_map.level.ToString());//解锁条件：达到{0}级
			}
		}
	}

	void update_ui()
	{
		if(sys._instance.m_self.m_t_player.mission > m_max_pt_id)
		{
			m_max_pt_id = sys._instance.m_self.m_t_player.mission;
			root_gui._instance.action_guide("s_" + m_max_pt_id.ToString());
			root_gui._instance.action_guide("s2_" + m_max_pt_id.ToString());
		}
		if(sys._instance.m_self.m_t_player.mission_jy > m_max_jy_id)
		{
			m_max_jy_id = sys._instance.m_self.m_t_player.mission_jy;
			root_gui._instance.action_guide("j_" + m_max_jy_id.ToString());
			root_gui._instance.action_guide("j2_" + m_max_jy_id.ToString());
		}
		
		if(sys._instance.m_self.m_t_player.level > m_level)
		{
			m_level = sys._instance.m_self.m_t_player.level;
			
			root_gui._instance.action_guide("level_" + m_level);
		}

		if (m_t_map == null)
		{
			return;
		}
		int num = sys._instance.m_self.get_map_star (m_t_map.id);
		s_t_map t_map = game_data._instance.get_t_map (m_t_map.id);
        m_image.SetActive(false);
        m_image1.SetActive(false);
        if (platform_config_common.m_half > 0)
        {
            Texture2D _texture = sys._instance.get_image_half(t_map.res);
           

            int w = (int)((float)_texture.width * 1.1f);
            int h = (int)((float)_texture.height * 1.1f);
            m_image.transform.GetComponent<UITexture>().mainTexture = _texture;
            m_image.transform.GetComponent<UITexture>().width = w;
            m_image.transform.GetComponent<UITexture>().height = h;
            m_image.SetActive(true);
            
            m_image1.transform.GetComponent<UITexture>().mainTexture = _texture;
            m_image1.transform.GetComponent<UITexture>().width = w;
            m_image1.transform.GetComponent<UITexture>().height = h;
            m_image1.SetActive(true);
        }
        else
        {

            ccard _card = ccard.get_new_card(m_t_map.role_id);

            s_message mes = new s_message();
            mes.m_type = "show_map_unit";
            mes.m_object.Add(_card);
            cmessage_center._instance.add_message(mes);
        }
        

        int ms = m_t_map.stars [0].star_num;
		for (int i = 0; i < 3; ++i)
		{
			if (ms < m_t_map.stars [i].star_num)
			{
				ms = m_t_map.stars [i].star_num;
			}
		}
		float v = (float)num / ms;
		m_image_clip.transform.GetComponent<UIPanel>().clipOffset = new Vector2 (0, -750 + 600 * v);
		m_image_text.transform.Find("Label").GetComponent<UILabel>().text = m_t_map.image_text;
		string s = m_image_text.transform.Find("Label").GetComponent<UILabel>().processedText;
		m_image_text.GetComponent<UISprite>().width = m_image_text.transform.Find("Label").GetComponent<UILabel>().width + 96;
	
		m_left_move.transform.localPosition = new Vector3(0, 0, 0);
		sys._instance.add_pos_anim (m_left_move, 0.3f, new Vector3 (-500, 0, 0), 0);

        if (game_data._instance.m_guaji > 0)
        {
            s_message msg1 = new s_message();
            msg1.m_type = "map_guaji";
            cmessage_center._instance.add_message(msg1);
        }
    }

    public void map_guaji_jiesuo()
    {
        if (m_is_jy)
        {
            if (m_jy_cur >= m_jy_list.Count - 1)
            {
                game_data._instance.m_guaji = 0;
                return;
            }
        }
        else
        {
            if (m_pt_cur >= m_pt_list.Count - 1)
            {
                game_data._instance.m_guaji = 0;
                return;
            }
        }

        if (get_cur() != add_cur(1))
        {
            reset_map();
        }
        
    }

	public void map_jiesuo_end()
	{
		m_jiesuo = false;
		m_root.SetActive(true);
		reset ();
		m_need_update_ui = true;
		root_gui._instance.action_guide ("jiesuo_" + m_t_map.id.ToString());
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_MISSION_REWARD)
		{
			int count = 0;
			count = chapter (m_t_map.id);
			s_t_map t_map = game_data._instance.get_t_map(m_t_map.id);
			for (int i = 0; i < 4; ++i)
			{
				s_t_reward t_reward = m_t_map.stars[m_select].rewards[i];
				sys._instance.m_self.add_reward(t_reward.type, t_reward.value1, t_reward.value2, t_reward.value3,t_map.name + game_data._instance.get_t_language ("map_gui.cs_1037_114"));//关卡奖励
			}
			if(count >= 0 && count < sys._instance.m_self.m_t_player.map_reward_get.Count)
			{
				int num = 1 << m_select;
				sys._instance.m_self.m_t_player.map_reward_get[count]  =  sys._instance.m_self.m_t_player.map_reward_get[count] | num;

			}
			//set_reward_value(m_t_map.id, 1);
			m_jl.transform.Find("frame_big").GetComponent<frame>().hide();
			reset_map();
			m_need_update_ui = true;
		}

		if (message.m_opcode == opclient_t.CMSG_MISSION_SAODANG)
		{
			protocol.game.smsg_mission_saodang _msg = net_http._instance.parse_packet<protocol.game.smsg_mission_saodang> (message.m_byte);
			m_buttle_tip.transform.GetComponent<buttle_tip>().fini_saodang(_msg.saodangs);
			m_buttle_tip.transform.GetComponent<buttle_tip>().init();
			m_saodang.transform.GetComponent<saodang>().init(m_buttle_tip.transform.GetComponent<buttle_tip>().m_mission_id, _msg.saodangs);
		}
		
		if(message.m_opcode == opclient_t.CMSG_MISSION_FIRST)
		{
			protocol.game.smsg_mission_first _msg = net_http._instance.parse_packet<protocol.game.smsg_mission_first>(message.m_byte);
			for(int i = 0;i < _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure( _msg.treasures[i]);
			}
			for(int i = 0;i < _msg.roles.Count;i++)
			{
				sys._instance.m_self.add_card( _msg.roles[i]);
			}
			for(int i = 0;i < _msg.equips.Count;i++)
			{
				sys._instance.m_self.add_equip( _msg.equips[i]);
			}
			for(int i = 0; i < _msg.types.Count;++i)
			{
				sys._instance.m_self.add_reward(_msg.types[i],_msg.value1s[i]
				                                ,_msg.value2s[i], _msg.value3s[i],true,game_data._instance.get_t_language ("map_gui.cs_1077_75"));//关卡首次奖励获得
			}
			sys._instance.m_self.m_t_player.mission_rewards.Add(mission_id);
			m_first_mission_gui.transform.Find("frame_big").GetComponent<frame>().hide();
			reset();
		}
	}
	
	void IMessage.message(s_message message)
	{
		if(message.m_type == "show_mission")
		{
			int mission_id = (int)message.m_ints[0];
			show_mission(mission_id);
		}

		if(message.m_type == "select_map")
		{
			if (message.m_ints.Count > 1)
			{
				m_index = (int)message.m_ints[1];
			}
			if (message.m_ints.Count > 0)
			{
				int map_id = (int)message.m_ints[0];
				set_t_map(map_id);
			}
			else
			{
				reset ();
			}
			if (message.m_ints.Count > 1)
			{
				int mission_id = (int)message.m_ints[1];
				show_mission(mission_id);
			}
		}

		if(message.m_type == "hide_mission")
		{
			m_select_mission.GetComponent<ui_show_anim>().hide_ui();
		}

		if(message.m_type == "cam_finished")
		{
			if (m_jiesuo)
			{
				m_map_jiesuo.SetActive(true);
				return;
			}
			m_root.SetActive(true);
			reset ();
			m_need_update_ui = true;
		}
		if(message.m_type == "mission_lock")
		{
			m_index = (int)message.m_ints[0];
		}
		
		if(message.m_type == "map_end")
		{
			m_map_sz.SetActive(true);
			m_is_click = true;

		}

        if (message.m_type == "map_guaji")
        {
            automatic_battle();
        }
	}

    public void automatic_battle()
    {
        if (sys._instance.m_self.m_t_player.mission_ids.Contains(m_last_mission.id))
        {
            map_guaji_jiesuo();
        }
        else
        {
            do_automatic_battle();
        }
        
    }

    void do_automatic_battle()
    {
        m_t_missions.Clear();
        for (int i = 0; i < game_data._instance.m_dbc_mission.get_y(); i++)
        {
            if (int.Parse(game_data._instance.m_dbc_mission.get(5, i)) == m_t_map.id)
            {
                s_t_mission _t_mission = game_data._instance.get_t_mission(int.Parse(game_data._instance.m_dbc_mission.get(0, i)));
                m_t_missions.Add(_t_mission);
            }
        }

        for (int i = 0; i < m_t_missions.Count; i++)
        {
            if ((!m_is_jy && sys._instance.m_self.m_t_player.mission == m_t_missions[i].lock_id) || (m_is_jy && sys._instance.m_self.m_t_player.mission_jy == m_t_missions[i].jylock_id))
            {
                m_last_mission = m_t_missions[i];
            }
        }

        if (m_last_mission != null)
        {
            show_mission(m_last_mission.id);
        }
    }

    bool isplay()
	{
		Animator  animator = m_map_sz.GetComponent<Animator>();
		AnimatorStateInfo animatorInfo; 
		animatorInfo =animator.GetCurrentAnimatorStateInfo(0); 
		if (animatorInfo.normalizedTime > 1.0f ) 
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	static int chapter(int id)
	{
		int count = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.map_ids.Count ;++i )
		{
			//count = count + m_ids[i];
			if(id == sys._instance.m_self.m_t_player.map_ids[i])
			{
				break;
			}
			count++;
		}
		return count;
	}

	bool is_click(int select)
	{
		int count = 0;
		count = chapter (m_t_map.id);
		//count =count -(3-m_select);
		/*if(count >= 0 && count < sys._instance.m_self.m_t_player.map_reward_get.Count && select < sys._instance.m_self.m_t_player.map_reward_get[count])
		{
			return true;
		}*/
		int num = 1 << select;
		if(sys._instance.m_self.m_t_player.map_reward_get.Count == 0 || count >= sys._instance.m_self.m_t_player.map_reward_get.Count )
		{
			return true;
		}
		if((sys._instance.m_self.m_t_player.map_reward_get[count] & num )!= 0)
		{
			return true;
		}
		return false;
	}

	void show_buttle_id(int id)
	{
		s_t_mission _mission = game_data._instance.get_t_mission (id);
		
		if(sys._instance.m_self.m_t_player.mission < _mission.lock_id)
		{
			string s = game_data._instance.get_t_language ("map_gui.cs_1201_14");//[ffc882]关卡未解锁
			root_gui._instance.show_prompt_dialog_box(s);
			return;
		}
		if(sys._instance.m_self.m_t_player.mission_jy < _mission.jylock_id)
		{
			string s = game_data._instance.get_t_language ("map_gui.cs_1201_14");//[ffc882]关卡未解锁
			root_gui._instance.show_prompt_dialog_box(s);
			return;
		}

		m_buttle_tip.GetComponent<buttle_tip>().m_mission_id = id;
		m_buttle_tip.SetActive(true);
		m_buttle_tip.GetComponent<buttle_tip>().init();
	}

	void show_buttle_tip(GameObject obj)
	{
		int _id = int.Parse (obj.name);

		show_buttle_id (_id);
	}

	void set_reward_value(int id,int value)
	{
		for(int i = 0;i < sys._instance.m_self.m_t_player.map_ids.Count;i ++)
		{
			if(sys._instance.m_self.m_t_player.map_ids[i] == id)
			{
				sys._instance.m_self.m_t_player.map_reward_get[i] = value;
			}
		}
	}

	int map_reward_get(int id)
	{
		for(int i = 0;i < sys._instance.m_self.m_t_player.map_ids.Count;i ++)
		{
			if(sys._instance.m_self.m_t_player.map_ids[i] == id)
			{
				return sys._instance.m_self.m_t_player.map_reward_get[i];
			}
		}

		return 0;
	}

	int map_reward_num(int id)
	{
		for(int i = 0;i < sys._instance.m_self.m_t_player.map_ids.Count;i ++)
		{
			if(sys._instance.m_self.m_t_player.map_ids[i] == id)
			{
				return sys._instance.m_self.m_t_player.map_star[i];
			}
		}
		
		return 0;
	}

	bool has_baoxiang(int id)
	{
        if (game_data._instance.m_dbc_mission_first_reward.has_index(id))
        {
            return true;
        }
		return false;
	}

	void show_select_mission()
	{
		if (!is_unlock())
		{
			m_select_mission.SetActive (false);
			return;
		}
		m_select_mission.SetActive (true);

		m_t_missions.Clear ();
		sys._instance.remove_child (m_drag);

		for(int i = 0;i < game_data._instance.m_dbc_mission.get_y();i ++)
		{
			if(int.Parse(game_data._instance.m_dbc_mission.get(5,i)) == m_t_map.id)
			{
				s_t_mission _t_mission = game_data._instance.get_t_mission(int.Parse(game_data._instance.m_dbc_mission.get(0,i)));
				m_t_missions.Add(_t_mission);
				if(m_is_jy)
				{
					if(int.Parse(game_data._instance.m_dbc_mission.get(11,i)) != 0)
					{
						m_max_mission = _t_mission;
						m_last_mission = _t_mission;
					}
				}
				else
				{
					m_max_mission = _t_mission;
					m_last_mission = _t_mission;
				}
			}
		}
        m_image_text.SetActive(true);
		if (is_unlock())
		{
			if (m_is_jy && sys._instance.m_self.m_t_player.mission_jy >= m_max_mission.jyindex_id)
			{
				m_switch.SetActive(false);
				m_image_text.SetActive (false);
			}
			else if (!m_is_jy && sys._instance.m_self.m_t_player.mission >= m_max_mission.index_id)
			{
				m_switch.SetActive(false);
				m_image_text.SetActive (false);
			}
		}

		int mission_lock = 0;

		float _add_y = 270;
		int _y = 0;
		float _yy = -10;

		for(int i = 0;i < m_t_missions.Count;i ++)
		{
			int x = 65;
            if(m_t_missions[i].type == 3 && sys._instance.m_self.m_t_player.level < 70)
            {
                continue;
            }
			GameObject _sub = null;
			if (m_t_missions[i].jytype == 0)
			{
				_sub = game_data._instance.ins_object_res("ui/mission_sub2");
			}
			else if (m_t_missions[i].jytype == 1)
			{
                x = 38;
                _sub = game_data._instance.ins_object_res("ui/mission_sub1");	
			}
			else if (m_t_missions[i].jytype == 2)
			{     
                x = 38;
                _sub = game_data._instance.ins_object_res("ui/mission_sub");
			}
			if(has_baoxiang(m_t_missions[i].id))
			{
				_sub.transform.Find("mission_reward").GetComponent<UIButtonMessage>().target = this.gameObject;
				_sub.transform.Find("mission_reward").gameObject.SetActive(true);
				_sub.transform.Find("mission_reward").GetComponent<UISpriteAnimation>().enabled = false;
			}
			else
			{
				_sub.transform.Find("mission_reward").gameObject.SetActive(false);
			}
			if(i == 0)
			{
				_sub.transform.Find("mission/d2").gameObject.SetActive(false);
			}
			else
			{
				_sub.transform.Find("mission/d2").gameObject.SetActive(true);
			}
			_sub.transform.name = m_t_missions[i].id.ToString();
			_sub.transform.parent = m_drag.transform;
			_sub.transform.localPosition = new Vector3(x, _add_y, 0);
			_sub.transform.localScale = new Vector3(1, 1, 1);
			UIButtonMessage[] ums = _sub.GetComponents<UIButtonMessage>();
			for (int j = 0; j < ums.Length; ++j)
			{
				if (ums[j].trigger == UIButtonMessage.Trigger.OnPress || ums[j].trigger == UIButtonMessage.Trigger.OnRelease)
				{
					ums[j].target = this.gameObject;
				}
			}

			if (sys._instance.m_self.m_t_player.mission < m_t_missions[i].lock_id || sys._instance.m_self.m_t_player.mission_jy < m_t_missions[i].jylock_id)
			{
				_sub.transform.Find("mission/star").gameObject.SetActive(false);
				_sub.transform.Find("mission/name").gameObject.SetActive(false);
				_sub.transform.Find("mission/name1").gameObject.SetActive(false);
				_sub.transform.Find("mission/icon").gameObject.SetActive(false);
				_sub.transform.Find("mission/wh").gameObject.SetActive(true);

				_sub.GetComponent<BoxCollider>().enabled = false;
				_sub.transform.Find("mission").GetComponent<UIPanel>().alpha = 0.3f;
				_sub.transform.Find("mission/effect").gameObject.SetActive(false);
                if (has_baoxiang(m_t_missions[i].id))
				{
					_sub.transform.Find("mission_reward").GetComponent<UISpriteAnimation>().enabled = false;
					_sub.transform.Find("mission_reward").GetComponent<UISprite>().spriteName = "gold_box_b1"; 
				}
			}
			else
			{
				_sub.transform.Find("mission/star").gameObject.SetActive(true);
				_sub.transform.Find("mission/name").gameObject.SetActive(true);
				_sub.transform.Find("mission/name1").gameObject.SetActive(true);
				_sub.transform.Find("mission/icon").gameObject.SetActive(true);
				_sub.transform.Find("mission/wh").gameObject.SetActive(false);
				_sub.GetComponent<BoxCollider>().enabled = true;
				_sub.transform.Find("mission").GetComponent<UIPanel>().alpha = 1.0f;
				_sub.transform.Find("mission/effect").gameObject.SetActive(true);
				string[] map_name = m_t_missions[i].name.Split (' ');

				_sub.transform.Find("mission/star").GetComponent<UIProgressBar>().value = 0.333f * sys._instance.m_self.get_mission_star(m_t_missions[i].id);
                if (map_name.Length > 1)
                {
                    _sub.transform.Find("mission/name").GetComponent<UILabel>().text = map_name[1];
                }
                else
                {
                    _sub.transform.Find("mission/name").GetComponent<UILabel>().text = map_name[0];
                }
		
				_sub.transform.Find("mission/name1").GetComponent<UILabel>().text = "[" + map_name[0] + "]";

				int _id = buttle_tip.get_monster_id (m_t_missions[i]);
				if(_id > 2)
				{	ccard _card = new ccard();
					_card.set_monster(_id);
					_sub.transform.Find("mission/icon").GetComponent<UISprite>().spriteName = _card.m_t_class.icon;
				}
				if (m_t_missions[i].day_num > 0)
				{
					int num = m_t_missions[i].day_num - sys._instance.m_self.get_mission_cishu(m_t_missions[i].id);
					if (num <= 0)
					{
						num = 0;
					}
				}
				_sub.GetComponent<UIButtonMessage>().target = this.gameObject;
				bool flag = false;
                if (has_baoxiang(m_t_missions[i].id))
				{
					_sub.transform.Find("mission_reward").GetComponent<UISprite>().spriteName = "gold_box_b1"; 
					for(int j =0 ;j < sys._instance.m_self.m_t_player.mission_rewards.Count;++j)
					{
						if(m_t_missions[i].id == sys._instance.m_self.m_t_player.mission_rewards[j])
						{
							flag = true;
							_sub.transform.Find("mission_reward").GetComponent<UISpriteAnimation>().enabled = false;
							_sub.transform.Find("mission_reward").GetComponent<UISprite>().spriteName = "gold_box_b5"; 
							break;
						}
					}
					if(!flag && sys._instance.m_self.get_mission_star(m_t_missions[i].id) > 0)
					{
						_sub.transform.Find("mission_reward").GetComponent<UISprite>().spriteName = "gold_box_b03"; 
						_sub.transform.Find("mission_reward").GetComponent<UISpriteAnimation>().enabled = true;
						_sub.transform.Find("mission_reward").GetComponent<UISpriteAnimation>().ResetToBeginning();
					}
				}
              
			}

			if (m_index == m_t_missions[i].id)
			{
				mission_lock = (int)_add_y;
			}

			if ((!m_is_jy && sys._instance.m_self.m_t_player.mission == m_t_missions[i].lock_id) || (m_is_jy && sys._instance.m_self.m_t_player.mission_jy == m_t_missions[i].jylock_id))
			{
				m_last_mission = m_t_missions[i];
				_yy = _add_y - 270;
			}

			_y ++;
			_add_y += 168;
		}

		if (_yy < -5)
		{
			_yy = _add_y - 270 - 159;
		}
		if (mission_lock > 0)
		{
			_yy = mission_lock - 270;
		}
		_add_y += 120;

		m_grid.transform.localPosition = new Vector3 (0, -_yy, 0);
		m_grid.GetComponent<UIPanel>().clipOffset = new Vector2(0, _yy);

		m_drag.GetComponent<UISprite>().height = (int)_add_y;
		m_drag.transform.localPosition = new Vector3(0, -270, 0);

		m_select_mission.transform.localPosition = new Vector3(-190, -20, 0);
		sys._instance.add_pos_anim(m_select_mission,0.3f, new Vector3(500, 0, 0), 0);

	}

	void add_scale_anim(GameObject obj)
	{
		TweenScale _scale = TweenScale.Begin (obj,0.25f,new Vector3(1.0f,1.0f,1.0f));
		
		_scale.method = UITweener.Method.EaseInOut;
		_scale.from = new Vector3 (1.5f, 1.5f, 1.5f);
		_scale.to = new Vector3(1.0f,1.0f,1.0f);
		_scale.delay = 0;
	}

	public void point_effect()
	{
		s_t_map t_map = null;
		List<s_t_map> m_maps = new List<s_t_map>();
		bool jy_map = false;
		if(m_is_jy == true)
		{
			m_maps = m_pt_list;
			jy_map = false;
		}
		else
		{
			m_maps = m_jy_list;
			jy_map = true;
		}
		bool m_flag = false;
		for(int i = 0; i <  m_maps.Count;++i)
		{
			t_map = m_maps[i];
			if(charpter_box(t_map,jy_map) || first_mission_box(t_map,jy_map))
			{
				m_flag = true;
				break;
			}
		}
		if(m_is_jy)
		{
			m_jy_point.SetActive(false);
            if (m_flag )
			{
				m_pt_point.SetActive(true);
			}
			else
			{
				m_pt_point.SetActive(false);
			}
		}
		else
		{
			m_pt_point.SetActive(false);
            if (m_flag || qiyu_gui.qiyu_effect())
			{
				m_jy_point.SetActive(true);
			}
			else
			{
				m_jy_point.SetActive(false);
			}
		}
	}

	public void effect()
	{
		s_t_map t_map = null;
		int num = 0;
		List<s_t_map> m_maps = new List<s_t_map>();
		bool jy_map = false;
		if(m_is_jy == true)
		{
			m_maps = m_jy_list;
			jy_map = true;
		}
		else
		{
			m_maps = m_pt_list;
			jy_map = false;
		}
		bool m_flag = false;
		num = get_cur() -1;
		if(num <= 0)
		{
			num = 0;
		}
		for(int i = num; i >= 0; --i)
		{
			t_map = m_maps[i];
			if(charpter_box(t_map,jy_map) || first_mission_box(t_map,jy_map))
			{
				m_flag = true;
				m_left_bx_map = t_map.id;
				break;
			}
		}
		if(m_flag)
		{
			m_left_bx.SetActive(true);
		}
		else
		{
			m_left_bx.SetActive(false);
		}
		num = get_cur() + 1;
		if(num >= m_maps.Count)
		{
			num = m_maps.Count -1;
		}
		m_flag = false;
		for(int i = num; i <= m_maps.Count -1;++i)
		{
			t_map = m_maps[i];
			if(charpter_box(t_map,jy_map) || first_mission_box(t_map,jy_map))
			{
				m_flag = true;
				m_right_bx_map = t_map.id;
				break;
			}
		}
		if(m_flag)
		{
			m_right_bx.SetActive(true);
		}
		else
		{
			m_right_bx.SetActive(false);
		}
		if(get_cur() -1 < 0)
		{
			m_left_bx.SetActive(false);
		}
		else if(get_cur() +1 >= m_maps.Count)
		{
			m_right_bx.SetActive(false);
		}
	}

	public static bool charpter_box(s_t_map t_map,bool jy_map)
	{
		int count = 0;
		count = chapter (t_map.id);
		if(jy_map)
		{
			if(sys._instance.m_self.m_t_player.mission_jy < t_map.jypid
			   || sys._instance.m_self.m_t_player.level < t_map.level)
			{
				return false;
			}
		}
		else
		{
			if(sys._instance.m_self.m_t_player.mission < t_map.pid
			   || sys._instance.m_self.m_t_player.level < t_map.level)
			{
				return false;
			}
		}
		for(int i = 0; i < t_map.stars.Count; ++i)
		{
			int num1 = 1 << i;
			if(sys._instance.m_self.get_map_star (t_map.id) >= t_map.stars[i].star_num &&
			   (sys._instance.m_self.m_t_player.map_reward_get[count] & num1 ) == 0)
			{
				return true;
			}
		}
		return false;
	}

	public static bool first_mission_box(s_t_map t_map,bool jy_map)
	{
		if(jy_map)
		{
			if(sys._instance.m_self.m_t_player.mission_jy < t_map.jypid
			   || sys._instance.m_self.m_t_player.level < t_map.level)
			{
				return false;
			}
		}
		else
		{
			if(sys._instance.m_self.m_t_player.mission < t_map.pid
			   || sys._instance.m_self.m_t_player.level < t_map.level)
			{
				return false;
			}
		}
		List<s_t_mission> t_mission = new List<s_t_mission>();
        foreach (int id in game_data._instance.m_dbc_mission_first_reward.m_index.Keys)
		{
            s_t_mission _t_mission = game_data._instance.m_mission_s[id];
			if(_t_mission == null)
			{
				continue;
			}
			if(_t_mission.map_id == t_map.id)
			{
				t_mission.Add(_t_mission);
			}
		}
		for(int i = 0; i < t_mission.Count;++i)
		{
			bool flag = false;
            if (sys._instance.m_self.m_t_player.mission_rewards.Contains(t_mission[i].id))
            {
                flag = true;
            }
			if(!flag)
			{
				if(sys._instance.m_self.get_mission_star(t_mission[i].id) > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public static bool map_effect()
	{
		List<s_t_map> m_map_jys = new List<s_t_map>();
		List<s_t_map> m_map_pts = new List<s_t_map>();
        foreach (int id in game_data._instance.m_dbc_map.m_index.Keys)
		{
            int _id = id;
			s_t_map _map = game_data._instance.get_t_map(_id);
			if(_map.id > 10000)
			{
				m_map_jys.Add(_map);
			}
			else
			{
				m_map_pts.Add(_map);
			}
		}
		for(int i = 0; i < m_map_jys.Count;++i)
		{
			s_t_map t_map = m_map_jys[i];
			if(charpter_box(t_map,true) || first_mission_box(t_map,true))
			{
				return true;
			}
		}
		for(int i = 0; i < m_map_pts.Count;++i)
		{
			s_t_map t_map = m_map_pts[i];
			if(charpter_box(t_map,false) || first_mission_box(t_map,false))
			{
				return true;
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {

		if (m_need_update_ui)
		{
			m_need_update_ui = false;
			update_ui();
		}

		
		if(m_is_click)
		{
			if(isplay())
			{
				m_map_sz.GetComponent<UIButtonMessage>().enabled = true;
				m_is_click = false;
			}
		}

	}
}
