using System.Collections.Generic;
using UnityEngine;

public class dress_page_gui : MonoBehaviour ,IMessage{

	public UILabel   shouji;
	public List<GameObject> m_dress_items = new List<GameObject>();
	public List<s_t_dress> m_dresss = new List<s_t_dress>();
	public GameObject m_sx;
	private int m_cur_page = 0;
	private int m_max_page = 0;
	
	public int m_show_type = 0;
	public bool m_show_lock = true;
	
	public GameObject m_des_label;
	
	public GameObject m_left;
	public GameObject m_right;
	
	public GameObject m_page_index;

	private int m_dir = 1;
	private Vector3 m_mouse_pos;
	private bool m_press = false;
	private s_t_dress dress_item;
	private int dress_type;

	public UILabel m_jiesuo;
	public UILabel m_shoujiwangcheng;
	// Use this for initialization
	void Start () 
	{

		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void OnEnable() {
		press ();
	}
	
	public void init()
	{
		m_cur_page = 0;
		m_show_lock = true;
		m_show_type = 0;
		m_dresss.Clear ();
		m_sx.SetActive (false);
		if(m_dress_items.Count == 0)
		{
			for(int y = 0;y < 2;y ++)
			{
				for(int x = 0;x < 4;x ++)
				{
					GameObject _item = game_data._instance.ins_object_res("ui/dress_page_item");
					_item.name = (y * 4 + x).ToString();
					_item.transform.parent = transform;
					_item.transform.localPosition = new Vector3(x * 210f - 310f,- y * 240f + 110f,0);
					_item.transform.localScale = new Vector3(1,1,1);
					_item.SetActive(true);
					
					m_dress_items.Add(_item);
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
				set_dress_page_index(m_cur_page);				
			}
			else if(m_mouse_pos.x <  _mouse_pos.x && m_left.activeSelf)
			{
				m_cur_page --;
				m_dir = -1;
				set_dress_page_index(m_cur_page);
			}
		}
	}

	public void dress_reset()
	{
		for(int i = 0;i < 8;i ++)
		{
			dress_page_item _item = m_dress_items[i].GetComponent<dress_page_item>();
			_item.set_dress (0,m_dir);
		}

		dress_get ();
		m_dresss.Sort (dress_page_gui.comp);
		
		m_max_page = (m_dresss.Count + 7) / 8;		
		if(m_max_page < 1)
		{
			m_max_page = 1;
		}
		
		set_dress_page_index (0);
	}

	public void dress_get()
	{
		m_dresss.Clear ();
		for(int j =0 ;j < sys._instance.m_self.m_t_player.dress_ids.Count;++j)
		{
			int id = sys._instance.m_self.m_t_player.dress_ids[j];
			s_t_dress t_dress = game_data._instance.get_t_dress(id);
			m_dresss.Add (t_dress);
		}
		
		for(int i = 0;i < game_data._instance.m_dbc_dress_unlock.get_y();i ++)
		{
			bool flag = false;
			int id = int.Parse( game_data._instance.m_dbc_dress_unlock.get(0,i));
			s_t_dress_unlock _t_dress_unlock = game_data._instance.get_t_dress_unlock(id);
			s_t_dress _dress = game_data._instance.get_t_dress(_t_dress_unlock.sz_id);
			for(int j =0 ;j < sys._instance.m_self.m_t_player.dress_ids.Count;++j)
			{
				if(_dress.id == sys._instance.m_self.m_t_player.dress_ids[j])
				{
					flag = true;
					break;
				}
			}
			if(!flag)
			{
				m_dresss.Add (_dress);
				break;
			}
		}

	}

	public static int comp(s_t_dress x,s_t_dress y)
	{
		int x_need = 0;
		int y_need = 0;
		if (sys._instance.m_self.has_dress (x.id)) 
		{	
			x_need = 1;
		}
		if (sys._instance.m_self.has_dress (y.id))
		{
			y_need = 1;		
		}
		if (sys._instance.m_self.has_dress_on (x.id))
		{
			x_need = 2;
		}	
		if (sys._instance.m_self.has_dress_on (y.id))
		{
			y_need = 2;		
		}
		if (x_need != y_need) 
		{
			return  y_need - x_need;
		}
						
		return x.id - y.id;
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_DRESS_ON)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if (_msg.success)
			{
				if(dress_type == 2)//game_data._instance.get_t_language ("dress_page_gui.cs_196_25")//穿上
				{

					bool flag = false;
					if(sys._instance.m_self.m_t_player.dress_on_ids.Count == 0)
					{
						sys._instance.m_self.m_t_player.dress_on_ids.Add (dress_item.id);
						flag = true;
					}
					else
					{
						for(int i = 0;i < sys._instance.m_self.m_t_player.dress_on_ids.Count;i++)
						{
							
							if (game_data._instance.get_t_dress ( sys._instance.m_self.m_t_player.dress_on_ids[i]).type == dress_item.type)
							{
								sys._instance.m_self.m_t_player.dress_on_ids.Remove (sys._instance.m_self.m_t_player.dress_on_ids[i]);
								sys._instance.m_self.m_t_player.dress_on_ids.Add (dress_item.id);
								flag = true;
								break;
							}
							
						}

					}
					if(!flag)
					{
						sys._instance.m_self.m_t_player.dress_on_ids.Add (dress_item.id);
					}
					s_message s_msg = new s_message();
					s_msg.m_type = "part";
					s_msg.m_string.Add(dress_item.res);
					cmessage_center._instance.add_message(s_msg);
					s_message s_msg1 = new s_message();
					s_msg1.m_type = "hide_dress";
					cmessage_center._instance.add_message(s_msg1);

					s_message _message = new s_message ();
					
					_message.time = 0.3f;
					_message.m_type = "action";

					if(dress_item.action2 != "")
					{
						_message.m_string.Add (dress_item.action2);
					}
					else
					{
						_message.m_string.Add ("huan_yi");
					}
					
					cmessage_center._instance.add_message (_message);

					_message = new s_message ();
					
					_message.time = 0.3f;
					_message.m_type = "mm_xx";
					
					if(dress_item.action1 != "")
					{
						_message.m_string.Add (dress_item.action2);
					}
					else
					{
						_message.m_string.Add ("huan_yi");
					}
					
					cmessage_center._instance.add_message (_message);

				}
		    	m_dresss.Sort(comp);
			    set_dress_page_index(m_cur_page);

			}
		}
		if (message.m_opcode == opclient_t.CMSG_DRESS_UNLOCK && dress_item != null)
		{
			protocol.game.smsg_success _msg1 = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if (_msg1.success)
			{
				sys._instance.m_self.m_t_player.dress_ids.Add (dress_item.id);
				s_t_dress_unlock _t_dress_unlock  = null;
				for(int i = 0; i < game_data._instance.m_dbc_dress_unlock.get_y();++i)
				{
					int id = int.Parse(game_data._instance.m_dbc_dress_unlock.get(0,i));
					_t_dress_unlock = game_data._instance.get_t_dress_unlock(id);
					if(dress_item.id == _t_dress_unlock.sz_id)
					{
						sys._instance.m_self.m_t_player.dress_tuzhi -= _t_dress_unlock.tz_num;
						break;
					}
				}
			}
			string text = "";
			for(int i = 0; i < sys._instance.m_self.player_dress_check(dress_item.id).Count;++i)
			{
				if(i ==0)
				{
					text += sys._instance.m_self.player_dress_check(dress_item.id)[i];
				}
				else
				{
					text += "\n"+sys._instance.m_self.player_dress_check(dress_item.id)[i];
				}
			}
			hide_Label _hide_Label = m_sx.GetComponent<hide_Label>();
			if(_hide_Label != null)
			{
				Destroy(_hide_Label);
			}
			m_sx.GetComponent<UILabel>().text = text;
			dacheng();
			dress_get();
			m_dresss.Sort(comp);
			if( m_dresss.Count - 1 != 0 && (m_dresss.Count - 1) % 8 == 0 
			   && (sys._instance.m_self.m_t_player.dress_ids.Count != game_data._instance.m_dbc_dress.get_y()))
			{
				int index = m_cur_page;
				dress_reset();
				set_dress_page_index(index + 1);
			}
			else
			{
				set_dress_page_index(m_cur_page);
			}
		}
		if (message.m_opcode == opclient_t.CMSG_DRESS_OFF)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if(_msg.success)
			{
				for(int i = 0;i < sys._instance.m_self.m_t_player.dress_on_ids.Count;i++)
				{
					if(dress_item.id == sys._instance.m_self.m_t_player.dress_on_ids[i])
					{
						sys._instance.m_self.m_t_player.dress_on_ids.Remove(dress_item.id);
					}
				}

				s_message s_msg = new s_message();
				s_msg.m_type = "remove_part";
				s_msg.m_string.Add(dress_item.res);
				cmessage_center._instance.add_message(s_msg);
				s_message s_msg1 = new s_message();
				s_msg1.m_type = "hide_dress";
				cmessage_center._instance.add_message(s_msg1);
				
				s_message _message = new s_message ();
				
				_message.time = 0.3f;
				_message.m_type = "action";
				_message.m_string.Add ("huan_yi");
				
				cmessage_center._instance.add_message (_message);

				m_dresss.Sort(comp);
				set_dress_page_index(m_cur_page);
			}

		}
	}

	public void dacheng()
	{
		if(m_sx.GetComponent<UILabel>().text != "")
		{
			m_sx.SetActive(true);
			TweenScale _scale = sys._instance.add_scale_anim(m_sx.gameObject,0.2f,0.5f,1.2f,0);
			EventDelegate.Add(_scale.onFinished, delegate() 
			                  {
				hide();
			},true);
		}
	}

	public void hide()
	{
		hide_Label _hide_Label = m_sx.GetComponent<hide_Label>();
		
		if(_hide_Label == null)
		{
			_hide_Label = m_sx.AddComponent<hide_Label>();
		}
		
		_hide_Label.m_time = 1.6f;
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "dress_buy") 
		{
			dress_item = message.m_object[0] as s_t_dress;
			dress_type = 1;
			protocol.game.cmsg_dress_unlock _msg = new protocol.game.cmsg_dress_unlock ();
			_msg.dress_id = dress_item.id;
			net_http._instance.send_msg<protocol.game.cmsg_dress_unlock> (opclient_t.CMSG_DRESS_UNLOCK, _msg);
		}
		if (message.m_type == "dress_on") 
		{
			dress_item = message.m_object[0] as s_t_dress;
			dress_type = 2;
			protocol.game.cmsg_dress_on _msg = new protocol.game.cmsg_dress_on ();
			_msg.dress_id = dress_item.id;
			net_http._instance.send_msg<protocol.game.cmsg_dress_on> (opclient_t.CMSG_DRESS_ON, _msg);

		}
		if(message.m_type == "dress_off")
		{
			dress_item = message.m_object[0] as s_t_dress;
			protocol.game.cmsg_dress_off _msg = new protocol.game.cmsg_dress_off ();
			_msg.dress_id = dress_item.id;
			net_http._instance.send_msg<protocol.game.cmsg_dress_off> (opclient_t.CMSG_DRESS_OFF, _msg);
		}

	}
	public void set_dress_page_index(int id)
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
			dress_page_item _item = m_dress_items[i].GetComponent<dress_page_item>();
			
			if(_id < m_dresss.Count)
			{
				_item.m_dress = m_dresss[_id];
				_item.set_dress(m_dresss[_id].id, m_dir);
			}
			else
			{
				_item.set_dress(0,m_dir);
			}
			
		}
		shouji.text = sys._instance.m_self.m_t_player.dress_ids.Count + "/" + game_data._instance.m_dbc_dress.get_y ();
		m_cur_page = id;
		page_index ();
		dress_item = null;
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

	public void click(GameObject obj)
	{
		if(obj.transform.name == "left")
		{
			m_cur_page --;
			m_dir = -1;
			set_dress_page_index(m_cur_page);
		}
		
		if(obj.transform.name == "right")
		{
			m_cur_page ++;
			m_dir = 1;
			set_dress_page_index(m_cur_page);
		}
	}

	bool player_hasdress(int id)
	{
		for (int i=0; i<sys._instance.m_self.m_t_player.dress_ids.Count; i++) 
		{
			if(id == sys._instance.m_self.m_t_player.dress_ids[i])
			{
				return true;
			}
				
		}
		return false;
	}
	void Update () {
		
		if(this.gameObject.activeSelf == false)
		{
			return ;
		}
		
		if(sys._instance.get_mouse_button(0) == true)
		{
			if(m_press == false)
			{
				press ();
				m_press = true;
			}
		}
		else
		{
			if(m_press == true)
			{
				release ();
				m_press = false;
			}
		}
	}
}
