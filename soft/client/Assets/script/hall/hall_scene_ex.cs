using System.Collections.Generic;
using UnityEngine;

public class hall_scene_ex : MonoBehaviour,IMessage {

	public GameObject m_mm;

	public GameObject m_scene_show;

	public GameObject m_call_unit;
	public GameObject m_call_effect;
	private GameObject m_show_unit;
	public string m_message = "call_unit_des";
	public float m_wait_time = 0.0f;

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
	private Vector3 m_mm_pos = new Vector3 (1.6f,0,3.886f);

	float m_shake = 0.0f;

	public List<GameObject> m_cams = new List<GameObject>();
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);

		m_call_effect.SetActive (false);

		sys._instance.m_hall_root = this.gameObject;
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void OnEnable()
	{
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
		m_mm.transform.localEulerAngles = new Vector3 (0,0,0);
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
			call_cam();
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

	void hall_cam()
	{
		RenderSettings.fog = true;
		m_cams [0].SetActive (true);
		m_cams [1].SetActive (false);
	}
	void call_cam()
	{
		RenderSettings.fog = false;
		m_cams [0].SetActive (false);
		m_cams [1].SetActive (true);
	}
	void hide_mm_show()
	{
		m_mm.GetComponent<face>().m_cam.SetActive(false);
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "scene_show")
		{
			sys._instance.m_reward_show.SetActive(false);
			m_scene_show.SetActive(true);
			if(!m_mm.transform.parent.gameObject.activeSelf)
			{
				m_mm.transform.parent.gameObject.SetActive(true);
			}
		}

		if(message.m_type == "show_unit_show")
		{
			RenderSettings.fog = false;
			m_cams [0].SetActive (false);
			m_scene_show.SetActive(false);
		}

		if(message.m_type == "hide_unit_show")
		{
			m_scene_show.SetActive(true);
		}

		if(message.m_type == "show_mm_show")
		{
			sys._instance.get_cam().nearClipPlane = 1.5f;
			m_mm.GetComponent<face>().m_cam.SetActive(true);
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
			m_mm.transform.parent.gameObject.SetActive(false);
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
			call_cam();
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
			int _type = int.Parse((string)message.m_string[0]);

			if(_type == 0)
			{
				s_message _out = new s_message();

				_out.m_type = "mm_action";
				_out.m_string.Add("hui");

				cmessage_center._instance.add_message(_out);

				hall_cam();
			}
			 
			if(_type == 1)
			{
				call_cam();

				/*
				s_message _out = new s_message();
				
				_out.m_type = "mm_action";
				_out.m_string.Add("zhao_huan");
				
				cmessage_center._instance.add_message(_out);
				*/
			}

			if(_type == 4)
			{
				call_cam();

				/*
				s_message _out = new s_message();
				
				_out.m_type = "mm_action";
				_out.m_string.Add("zhao_huan");
				
				cmessage_center._instance.add_message(_out);
				*/
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
		

	}
}
