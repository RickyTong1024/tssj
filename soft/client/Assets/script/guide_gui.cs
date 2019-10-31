
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;

public class guide_gui : MonoBehaviour {

    [System.NonSerialized]
    public string m_button = "";

	public GameObject m_center;

	public GameObject m_top;
	public GameObject m_bottom;
	public GameObject m_left;
	public GameObject m_right;

	public GameObject m_left_top;
	public GameObject m_left_bottom;

	public GameObject m_right_top;
	public GameObject m_right_bottom;

	public GameObject m_cursor;
	public GameObject m_des;

	public GameObject m_story;
	public GameObject m_story_back;
	private GameObject m_story_name;
	public GameObject m_story_cursor;
	public GameObject m_story_des;
	public GameObject m_story_left;
	public GameObject m_story_right;
	public GameObject m_story_center;

	public GameObject m_pointer;

	public static XDocument m_xml;
	public List<XElement> m_elements = new List<XElement>();

	private int m_guide_index = 0;

	private float m_story_wait = 0;
	private float m_pointer_wait = 0;
	private float m_action_wait = 0;
	private string m_name;

    private float m_icon_show_wait = 0;

	public GameObject m_skip_button;

	private float m_show_skip = 5.0f;
	private float m_shake = 0;

	private main_gui main_gui_got;
    public GameObject m_anchors;

	private s_message m_out_msg = new s_message();

	private int m_index = 0;
	public GameObject m_mask;
	public GameObject m_shell;
    public GameObject m_animask;
	// Use this for initialization
	void Awake()
	{
        if (m_xml == null)
		{
			string _config = "config/guide";
			TextAsset _xml_data = game_data._instance.get_object_res(_config, typeof(TextAsset)) as TextAsset;
			if(_xml_data == null)
			{
				Debug.Log (game_data._instance.get_t_language ("guide_gui.cs_71_15") + _config);//引导脚本错误
				return;
			}
			
			StringReader reader = new StringReader(_xml_data.text);
			m_xml = new XDocument();
			m_xml = XDocument.Load(reader);
		}

    }

	void Start () {
		m_skip_button.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guide_gui.cs_82_77");//跳过引导

    }
   public void got_main_gui()
    {
        main_gui_got = root_gui._instance.m_hall_gui.GetComponent<hall_gui>().m_main_gui.gameObject.GetComponent<main_gui>();
    }
	public bool is_button(string name)
	{
		if(name == m_button || name == "single_dialog_ok" || name == "skip_guide")
		{
			return true;
		}

		return false;
	}

	public void action_guide(string name)
	{
		if(m_elements.Count > 0)
		{
			return;
		}

		if(name == "game_start" )
		{
		}

		m_story_back.SetActive(true);
        if (m_xml == null)
        {
            print("the m_xml is null");
        }
		XElement roots = m_xml.Element("roots");
		IEnumerable<XElement> guides = roots.Elements("guides");
		foreach (XElement xe in guides)
		{	
			if(xe.Attribute("name").Value == name)
			{
				IEnumerable<XElement> _sub_xml = xe.Elements("guide");				
				foreach (XElement _sub_xe in _sub_xml)
				{
					m_elements.Add(_sub_xe);
				}
			}
		}

		if(m_elements.Count == 0)
		{
			return;
		}

		m_name = name;
		m_index = 0;
		m_guide_index = 0;

		sys._instance.m_pause = true;
		action ();
	}

    bool story(XElement element)
	{
		XElement _element = element;

		m_story_wait = 0;
		if(element.Attribute("duration") != null)
		{
			m_story_wait = float.Parse(element.Attribute("duration").Value);
		}
		else
		{
			m_story.SetActive(true);
		}

		GameObject _root = null;

		if(_element.Attribute ("name").Value == "")
		{
			_root = m_story_center;

			m_story_center.SetActive(true);
			m_story_left.SetActive(false);
			m_story_right.SetActive(false);

			m_story_des = _root.transform.Find("story_back").Find("des").gameObject;
			m_story_des.GetComponent<UILabel>().text = game_data._instance.get_t_language(_element.Attribute("des").Value);
			TypewriterEffect _effect = m_story_des.AddComponent<TypewriterEffect>();
			_effect.charsPerSecond = 40;

			_effect.mFullText = game_data._instance.get_t_language(_element.Attribute("des").Value);
			m_story_des.GetComponent<UILabel>().alpha = 0.0f;

			TweenAlpha _alpha = TweenAlpha.Begin (m_story_des, 0.2f, 1);
			
			_alpha.method = UITweener.Method.EaseInOut;
			_alpha.from = 0;
			_alpha.to = 1;
			_alpha.delay = 0;
		}
		else
		{
			if(m_index % 2 == 0)
			{
				_root = m_story_left;

				m_story_left.SetActive(true);
				m_story_right.SetActive(false);
				m_story_center.SetActive(false);
			}
			else
			{
				_root = m_story_right;

				m_story_left.SetActive(false);
				m_story_right.SetActive(true);
				m_story_center.SetActive(false);
			}

			m_story_des = _root.transform.Find("story_back").Find("des").gameObject;
			m_story_name = _root.transform.Find("story_back").Find("name").gameObject;
			m_story_cursor = _root.transform.Find("cursor").gameObject;
			m_story_name.GetComponent<UILabel>().text = game_data._instance.get_t_language(_element.Attribute ("name").Value);

			GameObject _biao_qin = _root.transform.Find("story_back").Find("biao_qin").gameObject;
			if(_element.Attribute ("face") != null)
			{
				_biao_qin.SetActive(true);
				_biao_qin.GetComponent<UISprite>().spriteName = _element.Attribute ("face").Value;
			}
			else
			{
				_biao_qin.SetActive(false);
			}

			m_story_des.GetComponent<UILabel>().text = "[90fbf7]" + game_data._instance.get_t_language(_element.Attribute("des").Value);
			m_story_des.GetComponent<UILabel>().alpha = 0.0f;
			sys._instance.add_alpha_anim (m_story_des, 1, 0, 1, 0);

			m_index ++;
            if (platform_config_common.m_half > 0)
            {
                Transform _image = _root.transform.Find("image");

                if (_image != null)
                {
                    Object.Destroy(_image.gameObject);
                }

                GameObject _head = null;

                if (_element.Attribute("image").Value == "self")
                {
                    _head = game_data._instance.ins_object_res("ui/npc_half/self_1");
                }
                else
                {
                    _head = game_data._instance.ins_object_res("ui/npc_half/" + _element.Attribute("image").Value);
                }

                if (_head != null)
                {
                    _head.SetActive(true);
                    _head.transform.name = "image";
                    _head.transform.parent = _root.transform;
                    _head.transform.localScale = new Vector3(1, 1);
                    _head.GetComponent<UITexture>().width = 600;
                    _head.GetComponent<UITexture>().height = 600;
                    _head.GetComponent<UITexture>().depth = 1;

                    if (m_story_right.activeSelf == true)
                    {
                        _head.transform.localPosition = new Vector3(341, -100);
                        sys._instance.add_pos_anim(_head, 0.5f, new Vector3(200, 0), 0);
                    }
                    else
                    {
                        _head.transform.localPosition = new Vector3(-341, -100);
                        sys._instance.add_pos_anim(_head, 0.5f, new Vector3(-200, 0), 0);
                    }

                    sys._instance.add_alpha_anim(_head, 0.5f, 0, 1, 0);

                    if (_element.Attribute("mirror") != null)
                    {
                        string _mirror = _element.Attribute("mirror").Value;

                        if (_mirror == "1")
                        {
                            _head.transform.localEulerAngles = new Vector3(0, 180, 0);
                        }
                    }
                }
            }
            else
            {
                s_message mes = new s_message();
                mes.m_type = "hide_ui_unit_cam";
                cmessage_center._instance.add_message(mes);

                ccard _card = null;
                bool flag = true;
                if (_element.Attribute("image").Value != "zs")
                {
                    int id = 0;
                    if (_element.Attribute("image").Value == "self")
                    {
                        id = 100;
                    }
                    else if (_element.Attribute("image").Value == "")
                    {
                        flag = false;
                    }
                    else
                    {
                        id = int.Parse(_element.Attribute("image").Value);
                    }
                    if (flag)
                    {
                        _card = ccard.get_new_card(id);
                    }

                }
                if (flag)
                {
                    Vector3 v = new Vector3(0, 1.536f, 4.51f);
                    Vector3 r = new Vector3(4, -168.0f, 0);

                    if (m_story_right.activeSelf == true)
                    {
                        r = new Vector3(4, 168.0f, 0);
                    }

                    mes = new s_message();
                    mes.m_type = "show_ui_unit_cam_guide";
                    mes.m_object.Add(v);
                    mes.m_object.Add(r);
                    mes.m_object.Add(_card);
                    mes.m_bools.Add(false);
                    mes.time = m_story_wait;
                    cmessage_center._instance.add_message(mes);
                }
            }
            
            

            if (m_story_name.GetComponent<UILabel>().text == "self")
			{
				m_story_name.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.name;
			}
			
			if(m_story_name.GetComponent<UILabel>().text == "zs")
			{
				m_story_name.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.zsname;
			}
			
			
			GameObject _story_back = _root.transform.Find("story_back").gameObject;
			
			TweenPosition _pos_anim = _story_back.GetComponent<TweenPosition>();
			
			if(_pos_anim != null && _pos_anim.enabled == true)
			{
				_story_back.transform.localPosition = new Vector3(0,103,0);
			}
			else
			{
				sys._instance.add_pos_anim (_story_back, 0.5f,new Vector3 (0, -200), 0);
			}
			
			sys._instance.add_alpha_anim (_story_back, 0.5f, 0, 1,0);

		}


		if(element.Attribute("mm_action") != null)
		{
			s_message _msg = new s_message();
			
			_msg.m_type = "face_name";
			
			string _action = element.Attribute("mm_action").Value;
			
			_msg.m_string.Add(_action);
			
			cmessage_center._instance.add_message(_msg);
		}
		if(element.Attribute("back") != null)
		{
			int _show = int.Parse(element.Attribute("back").Value);

			if(_show == 0)
			{
				m_story_back.SetActive(false);
			}
			else
			{
				m_story_back.SetActive(true);
			}
		}

		string _sound = _element.Attribute ("sound").Value;

		sys._instance.play_sound (_sound);

		return true;
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "skip_guide")
		{
			skip_guide();
		}
	}

	void skip_guide()
	{
		m_story.SetActive (false);
		m_pointer.SetActive (false);
        m_anchors.SetActive(false);


        sys._instance.m_pause = false;
		m_button = "";
		s_message _message = new s_message();
		_message.m_type = "guide_end";
		_message.m_string.Add(m_name);
		cmessage_center._instance.add_message(_message);
		
		m_story_wait = 0;
		m_pointer_wait = 0;
		m_action_wait = 0;
		
		m_elements.Clear();
		this.gameObject.SetActive(false);
	}

	private GameObject find_anchor(string anchor)
	{
		if(anchor == "t")
		{
			return m_top;
		}
		else if(anchor == "b")
		{
			return m_bottom;
		}
		else if(anchor == "l")
		{
			return m_left;
		}
		else if(anchor == "r")
		{
			return m_right;
		}
		else if(anchor == "lt")
		{
			return m_left_top;
		}
		else if(anchor == "lb")
		{
			return m_left_bottom;
		}
		else if(anchor == "rt")
		{
			return m_right_top;
		}
		else if(anchor == "rb")
		{
			return m_right_bottom;
		}
		else if(anchor == "c")
		{
			return m_center;
		}
		return m_center;
	}

	public void pointer(XElement element)
	{
		m_skip_button.SetActive (false);
		m_show_skip = 6.0f;

		m_pointer_wait = 0;
		if(element.Attribute("duration") != null)
		{
			m_pointer_wait = float.Parse(element.Attribute("duration").Value);
		}
		else
		{
			m_pointer.SetActive(true);
            m_anchors.SetActive(true);
		}

		string _name = m_name + "_" + m_guide_index + "_(" + m_button + ")_" + Screen.width;

		float _cursor_pos_x = float.Parse(element.Attribute("cursor_pos_x").Value);
		float _cursor_pos_y = float.Parse(element.Attribute("cursor_pos_y").Value);
		float _des_pos_x = float.Parse(element.Attribute("des_pos_x").Value);
		float _des_pos_y = float.Parse(element.Attribute("des_pos_y").Value);
		float _rot = float.Parse(element.Attribute("rot").Value);
		string _anchor = element.Attribute("anchor").Value;
		string _button = element.Attribute("button").Value;
		string _action = element.Attribute("mm_action").Value;
		string _sound = element.Attribute("sound").Value;
		string _des = game_data._instance.get_t_language(element.Attribute("des").Value);
		if(element.Attribute("des").Value == "")
		{
			_des = "";
		}
		
		m_button = _button;
		
		GameObject _root = find_anchor(_anchor);
		if(element.Attribute("td") != null)
		{
			float _height = (float)Screen.height;
			float _width = (float)Screen.width;
			float _scale = _width / _height;

			if(_scale < 1.5f)
			{
				_scale = 1.5f / _scale;

				_cursor_pos_x *= _scale;
				_cursor_pos_y *= _scale;
			}
		}

		m_cursor.transform.parent = _root.transform;
		m_cursor.transform.localPosition = new Vector3(_cursor_pos_x,_cursor_pos_y);
		m_cursor.SetActive (true);

		if(_des.Length == 0)
		{
			m_des.SetActive(false);
		}
		else
		{
			m_des.SetActive(true);

			TypewriterEffect _effect = m_des.transform.Find("back").GetChild(0).gameObject.GetComponent<TypewriterEffect>();

			if(_effect == null)
			{
				_effect = m_des.transform.Find("back").GetChild(0).gameObject.AddComponent<TypewriterEffect>();
			}

			_effect.mFullText = _des;
			_effect.charsPerSecond = 40;
			m_des.transform.localPosition = new Vector3(_des_pos_x,_des_pos_y);

			sys._instance.add_pos_anim (m_des, 0.5f,new Vector3 (-100, 0), 0);
			sys._instance.add_alpha_anim(m_pointer,0.5f,0.1f,1,0);
		}
		
	}
    
    public bool is_action()
	{
		if(m_story_wait > 0 || m_pointer_wait > 0 || m_icon_show_wait > 0)
		{
			return false;
		}

		return true;
	}
	public void action()
	{
		if(is_action() == false)
		{
			return;
		}

		if(m_story_des != null)
		{
			if(m_story_des.GetComponent<TweenAlpha>() != null && m_story_des.GetComponent<TweenAlpha>().enabled == true)
			{
				m_story_des.GetComponent<UILabel>().alpha = 1.0f;
				Object.Destroy(m_story_des.GetComponent<TweenAlpha>());
			}
		}

		m_guide_index ++;

		m_story.SetActive (false);
		m_pointer.SetActive (false);
        m_anchors.SetActive(false);

        s_message _message1 = new s_message();
        _message1.m_type = "hide_ui_unit_cam";
        cmessage_center._instance.add_message(_message1);


        if (m_elements.Count == 0)
		{
			sys._instance.m_pause = false;
			m_button = "";
            s_message _message = new s_message();
			_message.m_type = "guide_end";
			_message.m_string.Add(m_name);
			cmessage_center._instance.add_message(_message);

			if(m_out_msg != null)
			{
				cmessage_center._instance.add_message(m_out_msg);
				m_out_msg = null;
			}

			this.gameObject.SetActive(false);
			return;
		}

		XElement _element = m_elements [0];
		bool _remove = true;
		bool _action = false;

		if(_element.Attribute("type").Value == "story")
		{
			m_button = "story_back";
			_remove = story(_element);
		}
		else if(_element.Attribute("type").Value == "pointer")
		{
			if(root_gui._instance.is_wait_active())
			{
				return;
			}

			pointer(_element);
		}
		else if(_element.Attribute("type").Value == "load_scene")
		{
			_remove = true;
			_action = true;
			m_story_back.SetActive(false);

			root_gui._instance.m_hall_gui.SetActive (false);
			root_gui._instance.hide_battle_gui();
			sys._instance.m_buttle_cam.SetActive(false);
			sys._instance.m_reward_show.SetActive(false);
			sys._instance.m_game_state = "director";
			sys._instance.load_scene (game_data._instance.get_t_language(_element.Attribute("name").Value));

			if(_element.Attribute("anim") != null)
			{
				s_message _msg = new s_message();

				_msg.time = 0.2f;
				_msg.m_type = "show_director";
				_msg.m_string.Add(_element.Attribute("anim").Value);

				cmessage_center._instance.add_message(_msg);
			}
		}
		else if(_element.Attribute("type").Value == "load_battle_scene")
		{
			battle._instance.remove_all();
			sys._instance.m_game_state = "buttle";
			sys._instance.load_scene (game_data._instance.get_t_language(_element.Attribute("name").Value));

			_remove = true;
			_action = true;

			if (root_gui._instance.m_battle_gui != null)
			{
				root_gui._instance.m_battle_gui.transform.Find("battle_gui").gameObject.SetActive(false);
			}
			m_story_back.SetActive(false);
		}
		else if(_element.Attribute("type").Value == "load_hall")
		{
			sys._instance.m_game_state = "hall";
			sys._instance.load_scene (sys._instance.m_hall_name);
			_remove = true;
			_action = true;

			if (root_gui._instance.m_battle_gui != null)
			{
				root_gui._instance.m_battle_gui.transform.Find("battle_gui").gameObject.SetActive(true);
			}
			m_story_back.SetActive(true);
		}
		else if(_element.Attribute("type").Value == "wait_action")
		{
			m_action_wait = float.Parse(_element.Attribute("duration").Value);
			_remove = true;
		}
		else if(_element.Attribute("type").Value == "message")
		{
			s_message _message = new s_message();

			_message.m_type = _element.Attribute("name").Value;

			cmessage_center._instance.add_message(_message);
			_remove = true;
			_action = true;
		}
        else if (_element.Attribute("type").Value == "mask_zhayan")
        {
            m_animask.transform.parent.gameObject.SetActive(true);
            m_action_wait = float.Parse(_element.Attribute("duration").Value);
            m_animask.GetComponent<Animator>().Play(_element.Attribute("ani_name").Value);
            _remove = true;
            
        }
        else if (_element.Attribute("type").Value == "spring_panel")
        {
            float _px = float.Parse(_element.Attribute("px").Value);
            float _py = float.Parse(_element.Attribute("py").Value);
            float _pz = float.Parse(_element.Attribute("pz").Value);

            string _name = _element.Attribute("name").Value;

            GameObject _object = GameObject.Find(_name);

            if (_object != null)
            {
                SpringPanel.Begin(_object, new Vector3(_px, _py, _pz), 13f);
            }
            _remove = true;
            _action = true;
        }
        else if (_element.Attribute("type").Value == "end_layout")
        {
            s_message _msg = new s_message();

            _msg.m_type = "end_layout";

            cmessage_center._instance.add_message(_msg);

            _remove = true;
            _action = true;
        }
        else if (_element.Attribute("type").Value == "show_director")
        {
            s_message _message = new s_message();

            _message.m_type = "show_director";
            _message.m_string.Add(_element.Attribute("v0").Value);

            cmessage_center._instance.add_message(_message);

            _remove = true;
            _action = true;
        }
        else if (_element.Attribute("type").Value == "show_skill_gui")
        {
            s_message _out_message = new s_message();

            _out_message.m_type = "show_skill_gui";

            cmessage_center._instance.add_message(_out_message);
        }
        else if (_element.Attribute("type").Value == "add_mp")
        {
            int _value = int.Parse(_element.Attribute("value").Value);

            int _val = sys._instance.m_self.get_att(e_player_attr.player_mp) + _value;
            sys._instance.m_self.set_att(e_player_attr.player_mp, _val);

            _remove = true;
            _action = true;
        }
        else if (_element.Attribute("type").Value == "shake")
        {
            m_shake = float.Parse(_element.Attribute("v0").Value);
            _remove = true;
            _action = true;
        }
        else if (_element.Attribute("type").Value == "cam_src")
        {
            float _x0 = float.Parse(_element.Attribute("x0").Value);
            float _y0 = float.Parse(_element.Attribute("y0").Value);
            float _z0 = float.Parse(_element.Attribute("z0").Value);

            float _x1 = float.Parse(_element.Attribute("x1").Value);
            float _y1 = float.Parse(_element.Attribute("y1").Value);
            float _z1 = float.Parse(_element.Attribute("z1").Value);
            int _init = int.Parse(_element.Attribute("init").Value);

            s_message _msg = new s_message();

            _msg.m_type = "cam_src";

            _msg.m_floats.Add(_x0);
            _msg.m_floats.Add(_y0);
            _msg.m_floats.Add(_z0);

            _msg.m_floats.Add(_x1);
            _msg.m_floats.Add(_y1);
            _msg.m_floats.Add(_z1);

            _msg.m_ints.Add(_init);

            cmessage_center._instance.add_message(_msg);

            _remove = true;
            _action = true;
        }
        else if (_element.Attribute("type").Value == "cam")
        {
            float _x = float.Parse(_element.Attribute("x").Value);
            float _y = float.Parse(_element.Attribute("y").Value);
            float _z = float.Parse(_element.Attribute("z").Value);

            s_message _msg = new s_message();

            _msg.m_type = "cam_pos";
            _msg.m_floats.Add(_y);
            _msg.m_floats.Add(_x);
            _msg.m_floats.Add(_z);

            cmessage_center._instance.add_message(_msg);

            _remove = true;
            _action = true;
        }

		if(m_elements.Count == 0)
		{
			return;
		}

		if(_remove == true)
		{
			m_elements.RemoveAt (0);
		}

		if(_action == true)
		{
			action();
		}
	}
	// Update is called once per frame
	void Update () {
	
		if(m_action_wait > 0)
		{
			m_action_wait -= Time.deltaTime;

			if(m_action_wait <= 0)
			{
				action();
			}
		}

		if(m_story_wait > 0)
		{
			m_story_wait -= Time.deltaTime;

			if(m_story_wait <= 0)
			{
				m_story.SetActive(true);
			}
		}

        if (m_icon_show_wait > 0)
        {
            m_icon_show_wait -= Time.deltaTime;
            if (m_icon_show_wait <= 0)
            {
                action();
            }
        }

		if(m_pointer_wait > 0)
		{
			m_pointer_wait -= Time.deltaTime;
			
			if(m_pointer_wait <= 0)
			{
				m_pointer.SetActive(true);
                m_anchors.SetActive(true);
			}
		}

		if(m_show_skip > 0)
		{
			m_show_skip -= Time.deltaTime;

			if(m_show_skip < 0)
			{
				m_skip_button.SetActive(true);
			}
		}

		if(m_shake > 0)
		{
			m_shake -= Time.deltaTime * 10;
			
			if(m_shake <= 0)
			{
				m_shake = 0;
			}
			
			Vector3 _pos = this.transform.localPosition;
			
			_pos.x = Random.Range(-m_shake,m_shake);
			_pos.y = Random.Range(-m_shake,m_shake);
			
			this.transform.localPosition = _pos;
		}
	}
}
