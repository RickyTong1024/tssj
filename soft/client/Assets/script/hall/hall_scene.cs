using System.Collections.Generic;
using UnityEngine;

public class hall_scene : MonoBehaviour,IMessage {

	public GameObject m_anim;
	public GameObject m_mm;
	public GameObject m_effect;

	public GameObject m_cam;
	public GameObject m_lookat;

	public Vector3 m_cam_pos;
	public Vector3 m_look_pos;

	public Vector3 m_cur_cam_pos;
	public Vector3 m_cur_look_pos;

	public GameObject m_scene_show;

	public float m_cam_speed = 1.0f;

	public List<Vector3> m_pos_lists = new List<Vector3>();

	public GameObject m_call_unit;
	public GameObject m_call_effect;
	private GameObject m_show_unit;
	public string m_message = "call_unit_des";
	public float m_wait_time = 0.0f;

	public Vector3 m_select_max_pos = new Vector3(-0.3521915f,2.663836f,6.463345f);
	public Vector3 m_select_max_lookat = new Vector3(2.523803f,2.762269f,15.74583f);

	public Vector3 m_min_select_pos = new Vector3(0.7131583f,2.065522f,9.991017f);
	public Vector3 m_min_select_lookat = new Vector3(1.998699f,2.006067f,16.12329f);

	private float m_finished = 0.0f;

	private ccard m_show_card = null;
	private bool m_has_card = false;

	private protocol.game.smsg_chouka m_msg;
	private float m_mouse_pos_x = 0;
	private bool m_mouse_button = false;

	private RaycastHit m_rayhit;

	private float m_show_time = 0;
	private s_message m_show_message;
	private Vector3 m_old_position = new Vector3();
	private float m_pe = 0;
	private int m_center = 1;
	private Vector3 m_mm_pos = new Vector3 (7,0,13);

	float m_shake = 0.0f;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
        platform._instance.deal_cam("ts_game_hall", m_cam.GetComponent<Camera>());

        m_pos_lists.Add (new Vector3(0.4637054f,1.500454f,0.02867391f));
		m_pos_lists.Add (new Vector3(0.8995714f,1.451681f,2.212451f));

		m_pos_lists.Add (new Vector3(0.806143f,2.517467f,7.120002f));
		m_pos_lists.Add (new Vector3(0.5733793f,2.639872f,16.11357f));

		m_pos_lists.Add (new Vector3(13.53347f,3.779832f,5.353086f));
		m_pos_lists.Add (new Vector3(16.42854f,0.94f,9.696849f));

		m_cam_pos = m_pos_lists[0];
		m_look_pos = m_pos_lists[1];
		m_cam_speed = 0.5f;
        
		m_call_effect.SetActive (false);

		sys._instance.m_hall_root = this.gameObject;
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void OnEnable()
	{
        if (platform_config_common.m_shenhe == 1)
        {
            return;
        }
		if(m_mm != null)
		{
			for(int i = 0;i < sys._instance.m_self.m_t_player.dress_on_ids.Count;i ++)
			{
				s_t_dress _dress = game_data._instance.get_t_dress((int)sys._instance.m_self.m_t_player.dress_on_ids[i]);
				
				if(_dress.type == 2)
				{
					m_mm.GetComponent<unit>().change_part(_dress.res,0);
				}
			}

			m_mm.GetComponent<face>().clear_remind();
			return;
		}

        m_mm = sys._instance.create_mm(this.gameObject);
        m_mm.transform.position = m_mm_pos;
	}
	public void OnDisable()
	{
		if(m_mm != null)
		{
			m_mm.GetComponent<unit>().remove_part("hair");
		}
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_CHOUKA)
		{
			protocol.game.smsg_chouka _msg = net_http._instance.parse_packet<protocol.game.smsg_chouka> (message.m_byte);
			for (int i = 0; i < _msg.item_ids.Count; ++i)
			{
				if( _msg.item_ids[i] != 0 && _msg.item_nums[i] !=0)
				{
					sys._instance.m_self.add_item((uint)_msg.item_ids[i], _msg.item_nums[i], false,game_data._instance.get_t_language ("抽卡获得"));
				}
			}
			m_msg = _msg;
			m_show_card = null;
			show_effect();
			m_wait_time = 1.8f;
			zhao_huan();
			if(message.m_opcode == opclient_t.CMSG_CHOUKA)
			{
				m_message = "show_catch_gui";
			}
		}
	}
	void show_effect()
	{
		m_call_effect.SetActive(true);
		
		hide_time _hide = m_call_effect.GetComponent<hide_time>();
		
		if(_hide == null)
		{
			_hide = m_call_effect.AddComponent<hide_time>();
		}
		
		_hide.m_time = 4.0f;
	}
	void zhao_huan()
	{
		m_cam_pos =  m_pos_lists[2];
		m_look_pos = m_pos_lists[3];
		m_cam_speed = 1.0f;
	}
	void hide_mm_show()
	{
        if (m_mm!=null)
        {
            m_mm.GetComponent<face>().m_cam.SetActive(false);
        }		
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "scene_show")
		{
			sys._instance.m_reward_show.SetActive(false);
			m_scene_show.SetActive(true);
            if (m_mm!=null)
            {
                if (!m_mm.transform.parent.gameObject.activeSelf)
                {
                    m_mm.transform.parent.gameObject.SetActive(true);
                }
            }			
		}

		if(message.m_type == "show_unit_show")
		{
			m_scene_show.SetActive(false);
		}

		if(message.m_type == "hide_unit_show")
		{
			m_scene_show.SetActive(true);
		}

		if(message.m_type == "show_mm_show")
		{
            sys._instance.get_cam().nearClipPlane = 1.5f;
            if (m_mm!=null)
            {              
                m_mm.GetComponent<face>().m_cam.SetActive(true);
            }		
		}

		if(message.m_type == "hide_mm_show")
		{
			sys._instance.get_cam().nearClipPlane = 1f;
			hide_mm_show();
		}

		if(message.m_type == "shake_ui")
		{
			m_shake = (float)message.m_floats[0];
		}

		if(message.m_type == "reward_show")
		{
			sys._instance.m_reward_show.SetActive(true);
			m_scene_show.SetActive(false);
			m_call_effect.SetActive(false);
            if (m_mm!=null)
            {
                m_mm.transform.parent.gameObject.SetActive(false);
            }			
			s_message _message1 = new s_message();
			_message1 = new s_message();
			_message1.m_type = "reward_show_unit";
			_message1.m_long.Add((ulong)message.m_long[0]);
			cmessage_center._instance.add_message(_message1);
		}

		if(message.m_type == "show_shengjie_effect")
		{
			Vector3 _pos = m_call_unit.transform.GetChild(0).GetComponent<unit>().get_bone("accept").position;


			GameObject _object = sys._instance.show_effect (_pos,"effect/s_levelup_a02",3.0f);
			
			float _scale = 0.8f;
			
			Transform _bound_box = transform.Find("xuankuang");
			
			if(_bound_box != null)
			{
				_scale = _bound_box.transform.localScale.y * 0.8f;
			}
			_object.gameObject.transform.localScale = new Vector3(_scale,_scale,_scale);

			//m_call_unit.transform.GetChild(0).GetComponent<unit>().action("win");
			m_call_unit.transform.localEulerAngles = new Vector3(0,200,0);
		}


		if(message.m_type == "show_shengji_effect")
		{
			Vector3 _pos = m_call_unit.transform.GetChild(0).GetComponent<unit>().get_bone("accept").position;

			GameObject _object = sys._instance.show_effect (_pos,"effect/s_levelup_a01",3.0f);
			
			float _scale = 0.8f;
			
			Transform _bound_box = transform.Find("xuankuang");
			
			if(_bound_box != null)
			{
				_scale = _bound_box.transform.localScale.y * 0.8f;
			}
			_object.gameObject.transform.localScale = new Vector3(_scale,_scale,_scale);

		}
		if(message.m_type == "call_effect")
		{
			show_effect();
		}
		if(message.m_type == "hide_show_unit")
		{
			sys._instance.remove_child(m_call_unit);
		}
		if(message.m_type == "show_san")
		{
			m_show_unit.GetComponent<unit>().m_while = 1.0f;
		}
		if(message.m_type == "call_unit_des")
		{
			m_call_effect.SetActive(false);
			s_message _mes = new s_message();
			_mes.m_type = "show_catch_gui";
			cmessage_center._instance.add_message(_mes);
			zhao_huan();
		}

		if(message.m_type == "show_unit")
		{
			m_wait_time = 0.2f;

			if(message.m_long.Count > 0)
			{
				ulong guid = (ulong)message.m_long[0];
				m_show_card = sys._instance.m_self.get_card_guid(guid);
			}
			else if(message.m_object.Count > 0)
			{
				m_show_card = (ccard)message.m_object[0];
			}

			if(message.m_ints.Count > 0)
			{
				m_center = (int)message.m_ints[0];
			}
			else
			{
				m_center = 0;
			}

			m_message = (string)message.m_string[0];
		}
		if(message.m_type == "show_talk" && m_mm != null)
		{
			unit t = m_mm.GetComponent<unit>();
			List<int> _ids = new List<int>();
			for(int i = 0;i < sys._instance.m_self.get_card_num();i ++ )
			{
				if(game_data._instance.get_t_class(sys._instance.m_self.get_card_index(i).get_template_id()).color >= 4)
				{
					_ids.Add(sys._instance.m_self.get_card_index(i).get_template_id());
				}
			}
			if(_ids.Count == 0)
			{
				return;
			}
			int x = Random.Range(0,_ids.Count);
			int id  = (int)_ids[x];
			t.action("yh_" + id);

		}
		if(message.m_type == "hall_anim")
		{
			//m_star_root.SetActive(false);
			//m_anim.GetComponent<Animator>().SetInteger("anim",int.Parse((string)message.m_string[0])); 
			int _type = int.Parse((string)message.m_string[0]);

			if(_type == 0)
			{
				m_cam_pos = m_pos_lists[0];
				m_look_pos = m_pos_lists[1];
				m_cam_speed = 1.0f;
				//m_effect.SetActive(false);

				s_message _out = new s_message();

				_out.m_type = "mm_action";
				_out.m_string.Add("hui");

				cmessage_center._instance.add_message(_out);
			}
			 
			if(_type == 1)
			{
				zhao_huan();
				//m_effect.SetActive(true);

				s_message _out = new s_message();
				
				_out.m_type = "mm_action";
				_out.m_string.Add("zhao_huan");
				
				cmessage_center._instance.add_message(_out);
			}

			if(_type == 2)
			{
				m_cam_pos =  m_pos_lists[4];
				m_look_pos = m_pos_lists[5];
				m_cam_speed = 1.0f;

				s_message _out = new s_message();
				
				_out.m_type = "mm_action";
				_out.m_string.Add("fu_ben");
				
				cmessage_center._instance.add_message(_out);
			}

			if(_type == 4)
			{
				zhao_huan();

				s_message _out = new s_message();
				
				_out.m_type = "mm_action";
				_out.m_string.Add("zhao_huan");
				
				cmessage_center._instance.add_message(_out);
			}
		}
	}

	void show_object()
	{
		if (m_show_card != null)
		{			
			if(m_message != "select_show")
			{
				s_message _message = new s_message ();
				_message.m_type = m_message;

				s_message _message1 = new s_message ();
				_message1.m_type = "show_zhaomu_shuxing";
				_message1.m_object.Add(m_show_card);
				_message1.m_object.Add(_message);
				_message1.m_bools.Add(m_has_card);
				cmessage_center._instance.add_message(_message1);
			}
			m_show_card = null;
			
			if(m_show_unit != null)
			{
				float _pe = m_show_unit.GetComponent<unit>().m_name_height / 2.0f - 1.3f;
				if(_pe < 0)
				{
					_pe = 0;
				}
				else if(_pe > 1.0f)
				{
					_pe = 1.0f;
				}
				
				m_pe = _pe;
				
				//m_unit = _object;
				float _height =  m_show_unit.GetComponent<unit>().m_name_height * 0.6f + 0.6f;
				
				if(_height < 1.0f)
				{
					_height = 1.0f;
				}
				
				m_cam_pos.y = _height;
				m_look_pos.y = _height;
				
			}
		}
		else
		{
			/// 抽伙伴
			s_message _message = new s_message ();
			_message.m_type = "show_catch_card_show";
			_message.m_object.Add(m_msg);
			_message.m_string.Add(m_message);
			cmessage_center._instance.add_message(_message);
		}
	}
	// Update is called once per frame
	void Update () {
	
		if(m_show_time > 0)
		{
			m_show_time -= Time.deltaTime;
			
			if(m_show_time < 0)
			{
				m_look_pos = m_old_position;
				root_gui._instance.m_hall_gui.SetActive(true);
			}
		}
		if(m_wait_time > 0)
		{
			m_wait_time -= Time.deltaTime;
			if(m_wait_time < 0.0f)
			{
				show_object ();
			}
		}

		float _cam_speed = m_cam_speed;

		if(m_mm != null)
		{
			if(m_mm.GetComponent<face>().m_cam.activeSelf)
			{
				m_look_pos = m_mm.GetComponent<face>().m_cam.transform.position;
				m_cam_pos = m_mm.GetComponent<face>().m_cam.transform.GetChild(0).position;
				_cam_speed = 2.0f;
			}
		}

		float _speed =  Time.smoothDeltaTime * _cam_speed;

		if (_speed > 1)
		{
			_speed = 1;
		}

		m_cur_cam_pos = m_cur_cam_pos - (m_cur_cam_pos - m_cam_pos) * _speed;
		m_cur_look_pos = m_cur_look_pos - (m_cur_look_pos - m_look_pos) * _speed;

		Vector3 _out_pos = m_cur_cam_pos;
		Vector3 _out_lookat = m_cur_look_pos;

		if (m_shake > 0)
		{
			float _addx = Random.Range(- m_shake,m_shake);
			float _addy = Random.Range(- m_shake,m_shake);
			float _addz = Random.Range(- m_shake,m_shake);

			_out_pos = new Vector3(m_cur_cam_pos.x + _addx,m_cur_cam_pos.y + _addy,m_cur_cam_pos.z + _addz);
			_out_lookat = new Vector3(m_cur_look_pos.x + _addx,m_cur_look_pos.y + _addy,m_cur_look_pos.z + _addz);

			m_shake -= Time.deltaTime * 3.0f;
		}


		m_cam.transform.position = _out_pos;
		m_lookat.transform.position = _out_lookat;

		m_cam.transform.LookAt (m_lookat.transform.position);

		Ray _ui_ray = root_gui._instance.get_ui_cam().ScreenPointToRay(Input.mousePosition);

		if(sys._instance.get_mouse_button(0) && Physics.Raycast(_ui_ray, out m_rayhit) == false)
		{
			if(m_mouse_button == false)
			{
				m_mouse_pos_x = sys._instance.get_mouse_position ().x;
				m_mouse_button = true;
			}

			float _mouse_pos_x = sys._instance.get_mouse_position ().x;

			Vector3 _angle = m_call_unit.transform.localEulerAngles;
			
			_angle.y += m_mouse_pos_x - _mouse_pos_x;
			
			m_call_unit.transform.localEulerAngles = _angle;

			m_mouse_pos_x = sys._instance.get_mouse_position ().x;
		}
		else if(m_show_time <= 0.0f)
		{
			m_mouse_button = false;

			if (m_show_unit &&  m_show_unit.GetComponent<unit>().m_action_name == "win")
			{
				return;
			}

			Vector3 _angle = m_call_unit.transform.localEulerAngles;
			_angle.y -= Time.deltaTime * 30;
			
			m_call_unit.transform.localEulerAngles = _angle;
		}

		Vector3 _dis = m_lookat.transform.position - m_look_pos;

		if(_dis.magnitude < m_finished)
		{
			m_finished = 0.0f;
		}

	}
}
