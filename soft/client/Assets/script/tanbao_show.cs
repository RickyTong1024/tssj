
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tanbao_show : MonoBehaviour ,IMessage{
	private static float m_jg = 3.2f;
	public GameObject m_root;
	public Camera m_cam;
	public GameObject m_title;
	public GameObject m_scene_root;
	public GameObject[] m_effs;
	public List<Vector3> pos = new List<Vector3>();
	private GameObject m_unit;
	public List<GameObject> m_jians = new List<GameObject>();
	public GameObject m_jian;
	private List<GameObject> m_floors = new List<GameObject>();
	private Vector3[] m_fxs = {new Vector3(0, 270, 0), new Vector3(0, 0, 0), new Vector3(0, 90, 0), new Vector3(0, 180, 0)};
	private Vector3[] m_fx_poses = {new Vector3(-m_jg, 0, 0), new Vector3(0, 0, m_jg), new Vector3(m_jg, 0,0), new Vector3(0, 0, -m_jg)};
	private int m_site = 1;
	private int m_fx;
	private float m_cs_time1 = 0;
	private float m_cs_time = 0;
	private bool flag = false;
	private int final_site = 0;
	private float mFloat = 0;
	private Vector3 m_mouse_pos = new Vector3();
	public bool is_press = false;
	int row = 6;
	int line = 5;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_TANBAO_VIEW, _msg);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}


	public GameObject create_class()
	{	
		ccard m_card = sys._instance.m_self.get_card_id ((int)sys._instance.m_self.m_t_player.template_id);
		if(m_card == null)
		{
			m_card = ccard.get_new_card ((int)sys._instance.m_self.m_t_player.template_id);
		}
		s_t_class _class = game_data._instance.get_t_class(m_card.get_template_id());
		
		string _base_res = "unit" + "/" + _class.show + "/" + _class.show;
		string _dress_res = null;
		GameObject _ins = null;
		
		string _show = _class.show;
		
		string _dress = _class.dress;
		
		s_t_role_dress _t_dress =  game_data._instance.get_t_role_dress (m_card.get_role().dress_on_id);
		if(_t_dress != null)
		{
			_dress = _t_dress.res;
		}
		
		if(_dress != null && _dress != "" && _dress != "0")
		{
			_dress_res = "unit" + "/" + _dress + "/" + _dress;
			_ins = game_data._instance.ins_object_res(_dress_res);
			if(_ins != null)
			{
				_show =_dress;
			}
		}
		
		if(_ins == null)
		{
			_ins = game_data._instance.ins_object_res(_base_res);
		}
		
		if (_ins == null) {
			Debug.Log ("not find " + _class.show);
			
			_ins = game_data._instance.ins_object_res ("unit/ts_t01/ts_t01");
			_show = "ts_t01";
		}		
		_ins.transform.parent = m_root.transform;
		_ins.transform.GetComponent<unit>().m_config = _show;
		if(_dress_res != null)
		{
			_ins.transform.GetComponent<unit>().m_default_parts.Add(_dress);
		}
		_ins.AddComponent<CapsuleCollider>();
		_ins.GetComponent<CapsuleCollider>().center = new Vector3 (0,0.81f,0);
		_ins.GetComponent<CapsuleCollider>().radius = 0.68f;
		_ins.GetComponent<CapsuleCollider>().height = 3.04f;
		_ins.AddComponent<CapsuleCollider>().isTrigger = true;
		_ins.AddComponent<Rigidbody>();
		_ins.GetComponent<Rigidbody>().useGravity = false;
		_ins.GetComponent<Rigidbody>().isKinematic = true; 
		_ins.tag = "unit";
		_ins.GetComponent<unit>().action("ready");
		_ins.GetComponent<unit>().load_xml();
		_ins.GetComponent<unit>().m_start_pos = new Vector3 (0f, 0f, 0f);
		_ins.transform.localEulerAngles = new Vector3 (0f,0f,0f);
		_ins.transform.localPosition = new Vector3 (0f,0,0f);

		s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan (sys._instance.m_self.m_t_player.guanghuan_id);
		if (t_guanghuan != null)
		{
			GameObject _eff = game_data._instance.ins_object_res ("effect/" + t_guanghuan.effect);
			_eff.transform.parent = _ins.transform;
			_eff.transform.localEulerAngles = new Vector3 (0,0,0);
			_eff.transform.localPosition = new Vector3 (0,0,0);
			_eff.transform.localScale = Vector3.one;
		}
		return _ins;
	}

	public void create_site()
	{
		int flag = 0;
		int _id = 0;
		m_floors.Clear ();
		for (int i = 0; i < row; ++i)
		{
			for (int j = 0; j < row; ++j)
			{
				int site = i * 6 + j +1;
				s_t_tanbao_event t_tanbao = game_data._instance.get_t_tanbao(site);
				if (t_tanbao != null)
				{
					GameObject _ins = (GameObject)Object.Instantiate(m_title);
					_ins.transform.parent = m_scene_root.transform;
					m_floors.Add(_ins);
					if(flag == 0)
					{
						_ins.transform.localPosition = new Vector3(_id * (-m_jg), 0, 0);
						pos.Add(new Vector3(_id * (-m_jg), 0, 0));
					}
					else if(flag == 1)
					{
						_ins.transform.localPosition = new Vector3((row -1) * (-m_jg), 0, m_jg*_id);
						pos.Add(new Vector3((row -1) * (-m_jg), 0, m_jg*_id));
					}
					else if(flag == 2)
					{
						_ins.transform.localPosition = new Vector3(((row -1)-_id) * (-m_jg), 0, m_jg*(line -1));
						pos.Add(new Vector3(((row -1)-_id) * (-m_jg), 0, m_jg*(line -1)));
					}
					else
					{
						_ins.transform.localPosition = new Vector3(0 * (-m_jg), 0, m_jg*(line -1 -_id));
						pos.Add(new Vector3(0 * (-m_jg), 0, m_jg*(line -1 -_id)));
					}
					_id ++;
					if(site  == 1)
					{
						_id = 1;
						flag = 0;
					}
					else if(site  == row)
					{
						_id = 1;
						flag = 1;
					}
					else if(site  == row +line -1)
					{
						_id = 1;
						flag = 2;
					}
					else if(site  == row +line + row -2)
					{
						_id = 1;
						flag = 3;
					}
					_ins.transform.localEulerAngles = new Vector3(0, 0, 0);
					_ins.transform.localScale = new Vector3(1, 1, 1);
					_ins.SetActive (true);
					GameObject _ins1 = (GameObject)Object.Instantiate(m_effs[t_tanbao.type - 1]);
					_ins1.transform.parent = _ins.transform.GetChild(0);
					_ins1.transform.localPosition = new Vector3(0, 0, 0);
					_ins1.transform.localEulerAngles = new Vector3(-90, 90*(flag+1), 0);
					_ins1.transform.localScale = new Vector3(1, 1, 1);
					_ins1.SetActive (true);
				}
			}
		}
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "bz_move")
		{
			sys._instance.play_sound("effect/sound/chuangsun");
			sys._instance.show_effect(m_unit.transform.position,"effect/common/transport_start",3);
			m_cs_time = 1.0f;
			final_site = (int)message.m_ints[0];
		}
		if(message.m_type == "tanbao_toward")
		{
			int num = (int)message.m_ints[0];
			if(num < 0)
			{
				m_unit.transform.localEulerAngles = m_fxs[1];
				m_jian.transform.localPosition = new Vector3(m_unit.transform.localPosition.x,-1.5f,m_unit.transform.localPosition.z);
				for(int i = 0; i < m_jians.Count;++i)
				{
					m_jians[i].SetActive(false);
				}
				m_jians[1].SetActive(true);
			}
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_TANBAO_VIEW)
		{
			protocol.game.smsg_huodong_tanbao_view _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_tanbao_view>(message.m_byte);
			tanbao_gui._msg = _msg;
			root_gui._instance.show_tanbao_gui();
			m_site = _msg.gezi;
			if(m_site <= 0)
			{
				m_site = 1;
				return;
			}
			sys._instance.remove_child(m_root);
			create_class ();
			m_unit = m_root.transform.GetChild (0).gameObject;
			create_site ();
			m_unit.transform.localPosition = pos [m_site-1];
			int m_fx = 0;
			if(m_site  < row)
			{
				m_fx = 0;
			}
			else if(m_site  < row +line -1)
			{
				m_fx = 1;
			}
			else if(m_site  < row +line + row -2)
			{
				m_fx = 2;
			}
			else if(m_site >= row +line + row -2)
			{
				m_fx = 3;
			}
			m_unit.transform.localEulerAngles = m_fxs [m_fx];
			m_jian.transform.localPosition = new Vector3(m_unit.transform.localPosition.x,-1.5f,m_unit.transform.localPosition.z);
			for(int i = 0; i < m_jians.Count;++i)
			{
				m_jians[i].SetActive(false);
			}
			m_jians[m_fx].SetActive(true);
			flag = true;
		}
	}

	void show_eff()
	{
		//type 1起点 2道具格 3商店格 4事件格 5金币格 6骰子格 7移动格
		s_t_tanbao_event t_tanbao_event = game_data._instance.get_t_tanbao(m_site);
		int type = t_tanbao_event.type;
		if (t_tanbao_event.type == 1)
		{
			sys._instance.show_effect(m_unit.transform.position,"effect/common/jb_a01",3);
			sys._instance.play_sound("effect/sound/zhu_fu");
		}
		else if (type == 2)
		{
			sys._instance.show_effect(m_unit.transform.position,"effect/common/yl_a01",3);
			sys._instance.play_sound("effect/sound/zhu_fu");
		}
		else if (type == 3)
		{
			sys._instance.play_sound("effect/sound/zhu_fu");
		}
		else if (type == 4)
		{
			sys._instance.show_effect(m_unit.transform.position,"effect/common/wh_a01",3);
			sys._instance.play_sound("effect/sound/zhu_fu");
		}
		else if (type == 5)			    
		{
			sys._instance.show_effect(m_unit.transform.position,"effect/common/jb_a01",3);
			sys._instance.play_sound("effect/sound/zhu_fu");
		}
		else if (type == 6)
		{
			sys._instance.play_sound("effect/sound/zhu_fu");
		}
		else if (type == 7)
		{
			tanbao_gui._instance.m_one.transform.GetComponent<BoxCollider>().enabled = false;
			tanbao_gui._instance.m_ten.transform.GetComponent<BoxCollider>().enabled = false;
			sys._instance.show_effect(m_unit.transform.position,"effect/common/wh_a01",3);
			sys._instance.play_sound("effect/sound/zhu_fu");
			s_message mes = new s_message();
			mes.m_type = "bz_yidong";
			cmessage_center._instance.add_message(mes);
		}
	}

	public Vector3 get_mouse_position()
	{
		Vector3 pos = new Vector3 ();
		if(Input.touchCount > 0)
		{
			pos = Input.GetTouch(0).position;
		}
		else
		{
			pos = Input.mousePosition;
		}
		
		return pos;
	}


	// Update is called once per frame
	void Update () {
		if(!flag)
		{
			return;
		}
		if(sys._instance.get_mouse_button(0)&& !tanbao_gui.press)
		{
			mFloat += Time.deltaTime;
			Vector3 _pos = get_mouse_position ();
		
			if(is_press == false)
			{
				is_press = true;
				
				m_mouse_pos.x = _pos.x;
				m_mouse_pos.y = _pos.y;
				m_mouse_pos.z = _pos.z;	
			}
			Vector3 _cam_pos = m_cam.transform.localPosition;
			float x = Mathf.Abs(_pos.y - m_mouse_pos.y);
			float y = Mathf.Abs(_pos.x - m_mouse_pos.x);
			if( y > 30 || x > 30)
			{
				if(x > y)
				{
					_cam_pos.x -= (_pos.y - m_mouse_pos.y) * 0.2f;
				}
				else
				{
					_cam_pos.z -= (_pos.x - m_mouse_pos.x) * 0.2f;
				}
				if(_cam_pos.x < -1.9f)
				{
					_cam_pos.x = -1.9f;
				}
				else if(_cam_pos.x > 8.1f)
				{
					_cam_pos.x = 8.1f;
				}
				if(_cam_pos.z < 5)
				{
					_cam_pos.z = 5 ;
				}
				else if(_cam_pos.z > 8)
				{
					_cam_pos.z = 8;
				}
				m_cam.transform.localPosition = Vector3.Lerp(m_cam.transform.localPosition,_cam_pos,mFloat);
				//m_cam.transform.localPosition = _cam_pos;
				m_mouse_pos.x = _pos.x;
				m_mouse_pos.y = _pos.y;
				m_mouse_pos.z = _pos.z;
			}
		}
		else
		{
			mFloat =0;
			is_press = false;
		}
		if(!is_press)
		{
			m_cam.transform.localPosition = Vector3.Lerp(m_cam.transform.localPosition,new Vector3 (8.1f - Mathf.Abs((m_unit.transform.localPosition.x/3.2f)*2), 12, 6.4f),Time.time);
		}
		if (m_cs_time > 0)
		{
			m_cs_time -= Time.deltaTime;
			if (m_cs_time <= 0)
			{
				m_site = final_site;
				m_unit.transform.localPosition = pos[final_site-1];
				int m_fx = 0;
				if(final_site  < row)
				{
					m_fx = 0;
				}
				else if(final_site  < row +line -1)
				{
					m_fx = 1;
				}
				else if(final_site  < row +line + row -2)
				{
					m_fx = 2;
				}
				else if(final_site >= row +line + row -2)
				{
					m_fx = 3;
				}
				m_unit.transform.localEulerAngles = m_fxs[m_fx];
				m_jian.transform.localPosition = new Vector3(m_unit.transform.localPosition.x,-1.5f,m_unit.transform.localPosition.z);
				for(int i = 0; i < m_jians.Count;++i)
				{
					m_jians[i].SetActive(false);
				}
				m_jians[m_fx].SetActive(true);
				m_cs_time1 = 1.0f;
			}
			return;
		}
		if (m_cs_time1 > 0)
		{
			m_cs_time1 -= Time.deltaTime;
			if (m_cs_time1 <= 0)
			{
				s_message mes = new s_message();
				mes.m_type = "bz_move_finish";
				cmessage_center._instance.add_message(mes);
				show_eff();
			}
			return;
		}
	}
}
