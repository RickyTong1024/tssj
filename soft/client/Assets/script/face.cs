
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;

public class s_t_face
{
	public int id;
	public int level;
	public int is_rand;
	public string name;
	public string action;
	public string texture;
	public string text;
	public string sound;
	public string reply;
};
public class s_mm_action_set
{
	public string m_type;
	public List<s_mm_action> m_actions = new List<s_mm_action>();
};
public class s_mm_action
{
	public string m_type;
	public List<int> m_ints = new List<int>();
	public List<float> m_floats = new List<float>();
	public List<string> m_strings = new List<string>();
};

public class face : MonoBehaviour,IMessage {
	
	// Use this for initialization
	public GameObject m_face;
	public GameObject m_talk;
	public GameObject m_talk_pos;
	public GameObject m_cam;

	float m_face_time = 0.0f;

	List<s_t_face> m_face_list = new List<s_t_face>();
	List<s_mm_action_set> m_actions = new List<s_mm_action_set>();
	List<s_mm_action> m_action = null;

	float m_wait_time = 0.0f;
	public float m_target_rot = 191.66f;
	int m_action_pos = 0;
	string m_action_name;
	string m_mm_xx = "xx";
	public bool m_free = false;

	private List<string> m_help_acitons = new List<string>();
	private List<string> m_reminds = new List<string>();
	private string m_mm_hello;
	private RaycastHit m_rayhit;

	Quaternion m_q = new Quaternion();

	void Start () {
		cmessage_center._instance.add_handle (this);

		if(m_face != null)
		{
			load_txt ();
		}

		if(root_gui._instance != null)
		{
			m_talk = root_gui._instance.m_talk;
		}


		int _hour = System.DateTime.Now.Hour;
		
		if(_hour >= 6 && _hour < 12)
		{
			m_mm_hello = "zaoshang";
		}
		else if(_hour >= 12 && _hour < 18)
		{
			m_mm_hello = "xiawu";
		}
		else if(_hour >= 18 && _hour < 24)
		{
			m_mm_hello = "wanshang";	
		}
		else if(_hour >= 0 && _hour < 6)
		{
			m_mm_hello = "lingchen";		
		}
		

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
		if(message.m_type == "show_main_icon_effect")
		{
			string _action = (string)message.m_string[0];

			if(GetComponent<unit>().has_atcion(_action) && m_help_acitons.IndexOf(_action) < 0)
			{
				m_help_acitons.Add((string)message.m_string[0]);
			}

		}
		if(message.m_type == "hide_main_icon_effect")
		{
			string _action = (string)message.m_string[0];
			
			if(GetComponent<unit>().has_atcion(_action) && m_help_acitons.IndexOf(_action) < 0)
			{
				m_help_acitons.Remove((string)message.m_string[0]);
			}
			
		}

		if(message.m_type == "help_acitons")
		{
			m_help_acitons.Add((string)message.m_string[0]);
		}

		if(message.m_type == "mm_action")
		{
			action((string)message.m_string[0]);
		}

		if(message.m_type == "touch_mm")
		{
			if(this.transform.position.magnitude > 4)
			{
				action("run");
			}
			else
			{
				/*s_message _out = new s_message();
				_out.m_type = "show_mm_show";
				cmessage_center._instance.add_message(_out);*/
			}
		}
		if(message.m_type == "face_id")
		{
			if(m_face_time < 3.0f)
			{
				set_face(int.Parse((string)message.m_string[0]));
			}
		}

		if(message.m_type == "face_name" && this.GetComponent<unit>().get_nav().hasPath == false)
		{
			unit_action((string)message.m_string[0]);
		}

		if(message.m_type == "mm_xx")
		{
			m_mm_xx = (string)message.m_string[0];
		}
	}

	public void clear_remind()
	{
		m_reminds.Clear ();
	}
	bool is_remind(string action)
	{
		if(m_reminds.IndexOf(action) >= 0)
		{
			return true;
		}

		m_reminds.Add (action);

		return false;
	}
	void unit_action(string action)
	{
		if(this.GetComponent<unit>().m_action_name.IndexOf("stand") >= 0
		   || this.GetComponent<unit>().m_action_name == "walk"
		   || this.GetComponent<unit>().m_action_name == "run")
		{
			this.GetComponent<unit>().action(action);
		}
	}
	public void click(GameObject obj)
	{
		Debug.Log (obj.name);
	}
	void load_txt()
	{
		for(int i = 0;i < game_data._instance.m_t_face.get_y();i ++)
		{
			s_t_face _face = new s_t_face();
			int _id = i;

			_face.id = int.Parse(game_data._instance.m_t_face.get(0,_id));
			_face.level = int.Parse(game_data._instance.m_t_face.get(1,_id));
			_face.is_rand = int.Parse(game_data._instance.m_t_face.get(2,_id));

			_face.name = game_data._instance.m_t_face.get(3,_id);
			_face.action = game_data._instance.m_t_face.get(4,_id);
			_face.texture = game_data._instance.m_t_face.get(5,_id);
			_face.text = game_data._instance.m_t_face.get(6,_id);
			_face.sound = game_data._instance.m_t_face.get(7,_id);
			_face.reply = game_data._instance.m_t_face.get(8,_id);

			m_face_list.Add(_face);
		}
	}
	public void load_xml()
	{
		string _config = "config/" + this.transform.name;
		TextAsset _xml_data = game_data._instance.get_object_res(_config, typeof(TextAsset)) as TextAsset;
		if(_xml_data == null)
		{
			Debug.Log (game_data._instance.get_t_language ("face.cs_226_14") + _config);//角色脚本错误
			return;
		}
		//Debug.Log (game_data._instance.get_t_language ("face.cs_229_15") + m_config);//角色脚本
		StringReader reader = new StringReader(_xml_data.text);
		XDocument _xml = XDocument.Load(reader);
		XElement mms = _xml.Element("mms");
		IEnumerable<XElement> actions = mms.Elements("actions");		
		foreach (XElement xe in actions)
		{
			string _type = xe.Attribute("name").Value;
			s_mm_action_set _actions = new s_mm_action_set();

			_actions.m_type = _type;
			
			IEnumerable<XElement> action = xe.Elements("action");
			foreach (XElement _sub_xe in action)
			{
				s_mm_action _action = new s_mm_action();
				
				_action.m_type = _sub_xe.Attribute("type").Value;

				if(_action.m_type == "moveto" )
				{
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("x").Value));
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("y").Value));
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("z").Value));

					if(_sub_xe.Attribute("speed") != null)
					{
						_action.m_floats.Add(float.Parse(_sub_xe.Attribute("speed").Value));
					}
				}

				if(_action.m_type == "rot" )
				{
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("rot").Value));
				}
				
				if(_action.m_type == "wait" )
				{
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("time").Value));
				}
				
				if(_action.m_type == "lookat")
				{
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("x").Value));
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("z").Value));
				}

				if(_action.m_type == "anim")
				{
					_action.m_strings.Add(_sub_xe.Attribute("name").Value);
				}
				
				if(_action.m_type == "mm_action")
				{
					_action.m_strings.Add(_sub_xe.Attribute("name").Value);
				}
				if(_action.m_type == "speak")
				{
					_action.m_strings.Add(_sub_xe.Attribute("text").Value);
				}
				if(_action.m_type == "action")
				{
					_action.m_strings.Add(_sub_xe.Attribute("name").Value);
				}

				if(_action.m_type == "target_action")
				{
					_action.m_strings.Add(_sub_xe.Attribute("name").Value);
				}
				
				if(_action.m_type == "shake_cam")
				{
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("shake").Value));
				}
				
				if(_action.m_type == "entity" || _action.m_type == "target_entity")
				{
					_action.m_strings.Add(_sub_xe.Attribute("name").Value);
					
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("remove_time").Value));
				}
				
				if(_action.m_type == "entity_bone" || _action.m_type == "target_entity_bone")
				{
					_action.m_strings.Add(_sub_xe.Attribute("name").Value);
					_action.m_strings.Add(_sub_xe.Attribute("bone").Value);
					
					if(_sub_xe.Attribute("follow") != null)
					{
						_action.m_ints.Add(int.Parse(_sub_xe.Attribute("follow").Value));
					}
					else
					{
						_action.m_ints.Add((int)1);
					}
					
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("remove_time").Value));
				}

				if(_action.m_type == "sound")
				{
					_action.m_strings.Add(_sub_xe.Attribute("sound").Value);
				}

				if(_action.m_type == "mus")
				{
					_action.m_strings.Add(_sub_xe.Attribute("mus").Value);
				}

				_actions.m_actions.Add(_action);
			}
			
			m_actions.Add(_actions);
		}

	}
	void set_face(string name)
	{
		if(m_free == false)
		{
			return;
		}

		List<s_t_face> _face_list = new List<s_t_face>();

		for(int i = 0;i < m_face_list.Count;i ++)
		{
			if(m_face_list[i].name == name)
			{
				_face_list.Add(m_face_list[i]);
			}
		}

		if(_face_list.Count > 0)
		{
			s_t_face _face = _face_list [Random.Range (0, _face_list.Count)];
			set_face(_face);
		}
	}
	void set_face(int id)
	{
		for(int i = 0;i < m_face_list.Count;i ++)
		{
			if(m_face_list[i].id == id)
			{
				set_face(m_face_list[i]);
				break;
			}
		}
	}
	void set_face(s_t_face face)
	{
		if(m_face_time > 0.0f)
		{
			return ;
		}

		set_action (face.action);

		s_message _msg = new s_message();
		_msg.m_type = "face_ex_name";
		_msg.m_string.Add(face.texture);
		cmessage_center._instance.add_message(_msg);


		if(m_talk == null)
		{
			return;
		}

		sys._instance.play_sound_ex ("sound/" + face.sound);
		m_talk.SetActive(true);
		m_talk.transform.GetChild(0).Find("text").GetComponent<UILabel>().text = face.text;

		if(face.reply == null || face.reply == "0")
		{
			m_talk.transform.GetChild(0).Find("reply").GetComponent<UILabel>().text = "...";
		}
		else
		{
			m_talk.transform.GetChild(0).Find("reply").GetComponent<UILabel>().text = face.reply;
		}

		//m_text = 
	}
	public Transform get_bone(string bone_name)
	{	
		Transform[] hips = transform.GetComponentsInChildren<Transform>();
		
		foreach(Transform hip in hips)
		{		
			if(hip.name == bone_name)
			{
				return hip;
			}
		}
		
		return null;
	}
	void set_action(string name)
	{
		this.GetComponent<unit>().action (name);
	}
	void action(string name)
	{

		if(this.gameObject.activeInHierarchy == false)
		{
			return ;
		}

	//	if (m_action_name == name) return;
		m_action_name = name;

		m_wait_time = 0;

		if(get_nav ().enabled == true)
		{
			get_nav ().ResetPath ();
		}

		List<int> _aspects = new List<int>();
		
		for(int i = 0;i < m_actions.Count;i ++ )
		{
			s_mm_action_set _set = (s_mm_action_set)m_actions[i]; 
			
			if(_set.m_type == name)
			{
				_aspects.Add(i);
			}
		}
		
		if(_aspects.Count == 0)
		{
			Debug.Log(game_data._instance.get_t_language ("face.cs_464_13") + name);//单位行为无效
			return;
		}
		
		int _index = Random.Range (0, _aspects.Count);
		
		m_action = ((s_mm_action_set)m_actions [(int)_aspects[_index]]).m_actions;
		m_action_pos = 0;
		m_free = false;
		m_talk.SetActive(false);
	}
	public unit get_unit()
	{
		return this.GetComponent<unit>();
	}
	void action_update()
	{
		if(is_move() || m_action == null)
		{
			return;
		}
		if(m_wait_time > 0)
		{
			m_wait_time -= Time.deltaTime;
			return;
		}

		if(m_action.Count > 0 && m_action_pos < m_action.Count)
		{
			s_mm_action _action = (s_mm_action)m_action[m_action_pos];

			m_action_pos ++;
			
			if(_action.m_type == "anim" && transform.GetComponent<Animation>().GetClip( (string)_action.m_strings[0]) != null)
			{
				if(transform.GetComponent<Animation>().name == (string)_action.m_strings[0])
				{
					transform.GetComponent<Animation>().Stop();
				}
				
				transform.GetComponent<Animation>().CrossFade((string)_action.m_strings[0]);
			}

			if(_action.m_type == "unlock")
			{
				m_free = true;
			}

			if(_action.m_type == "moveto" )
			{
				//Vector3 _target = new Vector3(_action.m_floats[0],_action.m_floats[1],_action.m_floats[2]) - get_nav().destination;

				//if(_target.magnitude > 0.2f)
				//{ 
					get_nav().SetDestination(new Vector3(_action.m_floats[0],_action.m_floats[1],_action.m_floats[2]));
				//}

				if(_action.m_floats.Count > 3)
				{
					get_nav().speed = _action.m_floats[3];
				}
				else
				{
					get_nav().speed = 1.5f;
				}
			}
			if(_action.m_type == "speak")
			{
				m_talk.SetActive(true);
				m_talk.transform.GetChild(0).Find("text").GetComponent<UILabel>().text = (string)_action.m_strings[0];
				m_talk.transform.GetChild(0).Find("reply").GetComponent<UILabel>().text = "";
			}
			if(_action.m_type == "mm_hint")
			{
				m_face_time = 2;
			}

			if(_action.m_type == "wait" )
			{
				m_wait_time = _action.m_floats[0];
			}

			if(_action.m_type == "rot" )
			{
                m_target_rot = _action.m_floats[0];
            }

			if(_action.m_type == "lookat")
			{
				Vector3 _look_at = this.transform.position;
				_look_at.x = _action.m_floats[0];
				_look_at.z = _action.m_floats[1];
				this.transform.LookAt(_look_at);
			}

			if(_action.m_type == "mm_action")
			{
				action((string)_action.m_strings[0]);
			}

			if(_action.m_type == "action")
			{
				unit_action((string)_action.m_strings[0]);
			}

			if(_action.m_type == "sound")
			{
				sys._instance.play_sound_ex((string)_action.m_strings[0]);
			}

			if(_action.m_type == "mus")
			{
				sys._instance.play_mus((string)_action.m_strings[0]);
			}

			if(_action.m_type == "shake_cam")
			{
				sys._instance.shake_cam((float)_action.m_floats[0]);
			}
			
			if(_action.m_type == "entity")
			{
				GameObject _ins = game_data._instance.ins_object_res((string)_action.m_strings[0]);				
				_ins.transform.position = transform.transform.position;
				_ins.transform.localEulerAngles = transform.transform.localEulerAngles;
				
				Object.Destroy(_ins,(float)_action.m_floats[0]);
			}
			
			if(_action.m_type == "entity_bone")
			{
				GameObject _ins = game_data._instance.ins_object_res((string)_action.m_strings[0]);
				Transform _bone = get_bone((string)_action.m_strings[1]);
				
				int _follow =  (int)_action.m_ints[0];
				
				if(_bone)
				{
					if(_follow == 1)
					{
						_ins.transform.parent = _bone;
						_ins.transform.localPosition = new Vector3(0,0,0);
						_ins.transform.localEulerAngles = new Vector3(0,0,0);
					}
					else
					{
						_ins.transform.position = _bone.position;
						_ins.transform.localEulerAngles = new Vector3(0,0,0);
					}
				}
				
				Object.Destroy(_ins,(float)_action.m_floats[0]);
			}

		}
		
		if(m_action_pos >= m_action.Count)
		{
			m_action = null;
		}
	}
	bool is_move()
	{
		if(get_nav() == null) return false;
		
		return get_nav ().hasPath;
	}
	NavMeshAgent get_nav()
	{
		return this.transform.GetComponent<NavMeshAgent>();
	}

	// Update is called once per frame
	void Update () { 

		if(m_talk != null && m_talk.activeSelf == true)
		{
			m_talk.transform.localPosition = sys._instance.WorldToScreenPoint(m_talk_pos.transform.position);
		}
		if(is_move())  
		{
			if(get_nav().speed > 2.0f)
			{
				if(get_unit().m_action_name != "run")
				{
					get_unit().action("run");
				}
			}
			else
			{
				if(get_unit().m_action_name != "walk")
				{
					get_unit().action("walk");
				}
			}


			return;
		}
		else if(!is_move())
		{
            float _add_rot = Time.deltaTime * 360;
            Vector3 _rot = this.transform.localEulerAngles;
            if (Mathf.Abs(_rot.y - m_target_rot) < _add_rot)
            {
                _rot.y = m_target_rot;
            }
            else if (Mathf.Abs(_rot.y - m_target_rot) > 180)
            {
                if (_rot.y > m_target_rot)
                {
                    _rot.y += _add_rot;
                }
                else
                {
                    _rot.y -= _add_rot;
                }
            }
            else
            {
                if (_rot.y > m_target_rot)
                {
                    _rot.y -= _add_rot;
                }
                else
                {
                    _rot.y += _add_rot;
                }
            }
            this.transform.localEulerAngles = _rot;
        }

		if(m_face_list.Count == 0)
		{
			load_txt();
		}

		if(m_actions.Count == 0)
		{
			load_xml();
		}

		if(m_action == null && m_free == true)
		{
			if(m_face_time >= 0.0f)
			{
				m_face_time -= Time.deltaTime;

				if(m_face_time < 0)
				{
					if(m_mm_hello.Length > 0)
					{
						if(is_remind(m_mm_hello) == false)
						{
							unit_action(m_mm_hello);
						}


						m_mm_hello = "";
						m_face_time = 8.0f;
					}
					else if(m_help_acitons.Count > 0)
					{
						int _rand = Random.Range(0,m_help_acitons.Count);
						string _action = (string)m_help_acitons[_rand];

						if(m_help_acitons.Count > 0 && is_remind(_action) == false)
						{
							unit_action(_action);

							m_help_acitons.Clear();

							m_face_time = Random.Range(60.0f,90.0f);

						}
						else
						{
							unit_action(m_mm_xx);
							m_face_time = Random.Range(60.0f,90.0f);
						}
					}
				}
			}
		}
		else
		{
			action_update();
		}



	}

}
