
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scene : MonoBehaviour,IMessage {
	
	/// <summary>
	/// 鼠标射线
	/// </summary>
	private Ray m_ray;
	/// <summary>
	/// 射线碰撞的结构
	/// </summary>
	private RaycastHit m_rayhit;
	/// <summary>
	/// 鼠标拾取的有效距离
	/// </summary>
	private float m_fDistance = 20f;

	private bool m_main_show = true;

	private bool m_mm_show_gui = false;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);

	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.net_message(s_net_message message)
	{

	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "hide_mm_show_gui")
		{
			m_mm_show_gui = false;
		}
		
		else if(message.m_type == "show_mm_show_gui")
		{
			m_mm_show_gui = false;
		}

		if(message.m_type == "hide_main_gui_0")
		{
			m_main_show = false;
		}
		else if(message.m_type == "show_main_gui_0")
		{
			m_main_show = true;
		}
		else if(message.m_type == "show_main_gui")
		{
			m_main_show = true;
		}

		if(message.m_type == "modify_effect")
		{
			Vector3 _v3_pos = new Vector3((float)message.m_floats[0],(float)message.m_floats[1],(float)message.m_floats[2]);
			Vector3 _pos = sys._instance.WorldToScreenPoint(_v3_pos);

			GameObject _label = game_data._instance.ins_object_res("ui/att_modify_ex");
			
			int _depth = 0;
			for(int i = 0;i < root_gui._instance.m_ui_bottomleft.transform.childCount;i ++)
			{
				Transform _object = root_gui._instance.m_ui_bottomleft.transform.GetChild(i);
				UIPanel _panel = _object.GetComponent<UIPanel>(); 
				
				if(_panel != null && _depth < _panel.depth)
				{
					_depth =  _panel.depth;
				}
			}

			_depth ++;

            string _child_name = (string)message.m_string[1];
			float _add = (float)message.m_floats [3] * 100;
				
            if (_child_name == "null")
            {
                if (_add > 0)
                {
                    _child_name = "up";
                }
                else if (_add < 0)
                {
                    _child_name = "down";
                }
            }
                
            Transform _child = _label.transform.Find(_child_name);

            if (_child == null)
            {
                Debug.Log("modify_effect_ex not find child");
                Object.Destroy(_label);
                return;
            }
            else
            {
                _child.gameObject.SetActive(true);
                _child.GetComponent<UILabel>().text = game_data._instance.get_t_language((string)message.m_string[0]);
                _child.GetComponent<UILabel>().MakePixelPerfect();
            }

			_label.transform.GetComponent<att_modify_effect>().m_position = _v3_pos;

			_label.transform.GetComponent<UIPanel>().depth = _depth + 10;
			_label.transform.parent = root_gui._instance.m_ui_bottomleft.transform;
;

			_label.transform.localPosition = new Vector3(_pos.x,_pos.y,0);
			_label.transform.localScale = new  Vector3(1.0f,1.0f,1.0f);

            sys._instance.add_scale_anim(_child.gameObject,0.1f,0,1,0);

            TweenPosition _effect = TweenPosition.Begin(_child.gameObject,1.0f,_child.transform.localPosition);

            _effect.to = _child.transform.localPosition + new Vector3(0,_add,0);
			_effect.method = UITweener.Method.EaseInOut;
			_effect.ignoreTimeScale = false;
			_effect.delay = 0.3f;
			Object.Destroy(_label,0.8f);
		}

		if(message.m_type == "attack_num")
		{ 
			
			Vector3 _v3_pos = new Vector3((float)message.m_floats[0],(float)message.m_floats[1],(float)message.m_floats[2]);
			Vector3 _pos = sys._instance.WorldToScreenPoint(_v3_pos);

			string _name = "ui/physical_attack_num_ex";

			if((string)message.m_string[0] == "blue")
			{
                _name = "ui/magic_attack_num_ex";
			}
			else if((string)message.m_string[0] == "green")
			{
                _name = "ui/add_hp_num_ex";
			}

			GameObject _label = game_data._instance.ins_object_res(_name);
			bool _buffer = message.m_bools[0];

			string temp = sys._instance.value_to_wan( System.Int64.Parse(message.m_long[0].ToString()));

			string _num = temp;

			if(_buffer == true)
			{
				//_num = "a" + _num;
			}

			if (_buffer == true)
			{
				sys._instance.play_sound("sound/blood");
			}
			else if((string)message.m_string[0] == "green")
			{
				sys._instance.play_sound("sound/heal");
			}

			_label.SetActive(true);
			string s = (string)message.m_string[1] + " " + _num;
			_label.transform.GetChild(0).GetComponent<UILabel>().text = s;

			int _depth = 0;
			for(int i = 0;i < root_gui._instance.m_ui_bottomleft.transform.childCount;i ++)
			{
				Transform _object = root_gui._instance.m_ui_bottomleft.transform.GetChild(i);
				UIPanel _panel = _object.GetComponent<UIPanel>(); 

				if(_panel != null && _depth < _panel.depth)
				{
					_depth =  _panel.depth;
				}
			}

			_depth ++;

			_label.transform.GetComponent<attack_num_show>().m_position = _v3_pos;
			_label.transform.GetComponent<UIPanel>().depth = _depth + 10;
			_label.transform.parent = root_gui._instance.m_ui_bottomleft.transform;
			_label.transform.position = new Vector3(0,0,0);
			_label.transform.localPosition = new Vector3(_pos.x,_pos.y,0);
			_label.transform.localScale = new  Vector3(0.8f,0.8f,0.8f);
			Object.Destroy(_label,0.8f);
		}

		if(message.m_type == "skill_name")
		{
			Vector3 _v3_pos = new Vector3((float)message.m_floats[0],(float)message.m_floats[1],(float)message.m_floats[2]);
			Vector3 _pos = sys._instance.WorldToScreenPoint(_v3_pos);

			GameObject _ins = game_data._instance.ins_object_res("ui/skill_name");
			
			UILabel _label = _ins.transform.GetChild(0).Find("Label").GetComponent<UILabel>();
			
			_label.text = (string)message.m_string[0];

			_ins.SetActive(true);
			_ins.transform.GetComponent<attack_num_show>().m_position = _v3_pos;
			_ins.transform.parent = root_gui._instance.m_ui_bottomleft_1.transform;
			_ins.transform.position = new Vector3(0,0,0);
			_ins.transform.localPosition = new Vector3(_pos.x,_pos.y,0);
			_ins.transform.localScale = new  Vector3(1.0f,1.0f,1.0f);

			Object.Destroy(_ins,1.2f);
		}

		if(message.m_type == "skill_lable")
		{
			Vector3 _v3_pos = new Vector3((float)message.m_floats[0],(float)message.m_floats[1],(float)message.m_floats[2]);
			Vector3 _pos = sys._instance.WorldToScreenPoint(_v3_pos);

			GameObject _label = game_data._instance.ins_object_res("ui/skill_label");
			_label.GetComponent<attack_num_show>().reset((List<string>)message.m_object[0]);			
			_label.transform.GetComponent<attack_num_show>().m_position = _v3_pos;
			_label.transform.GetComponent<attack_num_show>().m_y = 50;
			_label.transform.parent = root_gui._instance.m_ui_bottomleft_1.transform;
			_label.transform.position = new Vector3(0,0,0);
			_label.transform.localPosition = new Vector3(_pos.x,_pos.y + 50,0);
			_label.transform.localScale = new  Vector3(0.8f,0.8f,0.8f);
			Object.Destroy(_label,1.2f);
		}
	}
	// Update is called once per frame
	void Update () {
	
		if (sys._instance.m_is_loading)
			return;

		//检测鼠标左键的拾取
		if (Input.GetMouseButtonDown(0) 
		    && sys._instance.get_cam() != null 
		    && (m_main_show || m_mm_show_gui == true)) {
			//鼠标的屏幕坐标空间位置转射线

			Ray _ui_ray = root_gui._instance.get_ui_cam().ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(_ui_ray, out m_rayhit, m_fDistance))
			{
				return ;
			}

			m_ray = sys._instance.get_cam().ScreenPointToRay(Input.mousePosition);

			RaycastHit[] _hits = Physics.RaycastAll(m_ray);

			GameObject _face = null;
			GameObject _mm = null;
			GameObject _ts = null;

			float _face_dis = 1000000.0f;
			float _mm_dis = 10000000.0f;

			for(int i = 0;i < _hits.Length;i ++)
			{
				if(_hits[i].collider.gameObject.tag == "trigger_face")
				{
					if(_hits[i].distance < _face_dis)
					{
						_face = _hits[i].collider.gameObject;
						_face_dis = _hits[i].distance;
					}
				}

				if(_hits[i].collider.gameObject.tag == "mm" && sys._instance.m_pause == false)
				{
					if(_hits[i].distance < _mm_dis)
					{
						_mm = _hits[i].collider.gameObject;
						_mm_dis = _hits[i].distance;
					}
				}

				if(_hits[i].collider.gameObject.tag == "ts")
				{
					_ts = _hits[i].collider.gameObject;
				}
			}

			if(_face != null)
			{
				s_message _msg = new s_message();
				_msg.m_type = "face_name";
				_msg.m_string.Add(_face.name);
				cmessage_center._instance.add_message(_msg);
			}
			else if(_mm != null)
			{
				s_message _msg = new s_message();
				_msg = new s_message();
				_msg.m_type = "touch_mm";
				cmessage_center._instance.add_message(_msg);
			}

		}

	}
}
