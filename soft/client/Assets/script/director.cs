
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;

public class director : MonoBehaviour {

	public GameObject m_center_label;
	public GameObject m_film_label;
	public List<GameObject> m_objects = new List<GameObject>();
	public List<GameObject> m_units = new List<GameObject>();
	public Texture2D[] m_textures;
	public GameObject m_root;
	public string m_xml_name;
	public GameObject m_back;
	public GameObject m_effect_lable;
    public GameObject m_left_lable;
	private string m_center_text;
	private float m_shake = 0.0f;
	private XDocument m_xml;
	private List<s_message> m_directors = new List<s_message>();
	private float m_wait = 0;

	private string m_mus_name;
	private bool m_stop_mus = false;
	public AudioSource m_play_mus;
	private int m_pos_id = 0;
	private UILabel m_tiaoguo;
    public GameObject m_skip2game;
    public GameObject m_skip2next;

    public GameObject top_mask;
    public GameObject bottom_mask;

	public GameObject lookat_obj;
	public GameObject campos_obj;
	private Vector3 m_lookat_target;
	private float m_lookat_speed;
	private Vector3 m_campos_target;
	private float m_campos_speed;
        // Use this for initialization
	void Start () {

		Transform _obj = this.transform.parent.transform.Find("ui/Camera/film/Anchor/Label");

		if(_obj != null)
		{
			m_tiaoguo = _obj.GetComponent<UILabel>();
			m_tiaoguo.text = game_data._instance.get_t_language ("director.cs_38_20") + ">>";//点击跳过
		}

		TextAsset _xml_data = game_data._instance.get_object_res(m_xml_name, typeof(TextAsset)) as TextAsset;
		
		if(_xml_data == null)
		{
			Debug.Log (game_data._instance.get_t_language ("director.cs_45_14") + m_xml_name);//导演脚本错误
			return;
		}

		StringReader reader = new StringReader(_xml_data.text);
		m_xml = XDocument.Load(reader);
		XElement directors = m_xml.Element("directors");
		IEnumerable<XElement> director = directors.Elements("director");
		foreach (XElement xe in director)
		{
			s_message _msg = new s_message();
			_msg.m_type = xe.Attribute("type").Value;
			int _id = 0;
			while(xe.Attribute("v" + _id.ToString()) != null)
			{
				_msg.m_string.Add(xe.Attribute("v" + _id.ToString()).Value);
				_id ++;
			}

			m_directors.Add(_msg);
		}
		  
 
		if(cmessage_center._instance == null)
		{
			this.gameObject.AddComponent<cmessage_center>();
			this.gameObject.AddComponent<sys>();
			this.gameObject.AddComponent<AudioListener>();
		}


		if(m_effect_lable != null)
		{
			m_effect_lable.GetComponent<UILabel>().text = "";
		}
	}

	public void OnEnable()
	{
		m_pos_id = 0;
		Time.timeScale = 1.0f;

		if(sys._instance != null)
		{
			root_gui._instance.show_mask();
		}
	}

	public void set_film_label(string text)
	{
		m_film_label.GetComponent<UILabel>().text = text;

		TweenAlpha _alpha = TweenAlpha.Begin (m_film_label, 0.2f, 1);
	}
	public void click(GameObject obj)
	{
        if (obj.transform.name == "start2game")
        {
            m_pos_id = m_directors.Count - 1;
            m_wait = 0.0f;
            Time.timeScale = 1.0f;
        }
        else if (obj.transform.name == "start2director")
        {
            m_pos_id++;
            m_wait = 0.0f;
            m_center_label.GetComponent<UILabel>().text = " ";
            m_film_label.GetComponent<UILabel>().text = " ";



            m_center_label.AddComponent<TypewriterEffect>();
            TypewriterEffect eeff = null;
            eeff = m_center_label.gameObject.GetComponent<TypewriterEffect>();
            if (eeff == null)
            {
                eeff = m_center_label.gameObject.AddComponent<TypewriterEffect>();

            }

            EventDelegate.Add(eeff.onFinished, delegate()
            {

                Destroy(m_center_label.gameObject.GetComponent<TypewriterEffect>());
            });
            eeff.charsPerSecond = 20;
            eeff.enabled = true;
            eeff.mFullText = " ";
        }
        else
        {
            m_pos_id = m_directors.Count - 1;
            m_wait = 0.0f;
        }
		
	}
	public void set_center_label(string text)
	{
		m_center_text = text;
		m_center_label.GetComponent<UILabel>().text = text;

		TweenAlpha _alpha = TweenAlpha.Begin (m_center_label, 0.2f, 1);
	}
    void set_center_label_ani(string text)
    {
        m_center_text = text;
        m_center_label.GetComponent<UILabel>().text = "";

        m_center_label.AddComponent<TypewriterEffect>();
        TypewriterEffect eeff = null;
        eeff = m_center_label.gameObject.GetComponent<TypewriterEffect>();
        if (eeff == null)
        {
            eeff = m_center_label.gameObject.AddComponent<TypewriterEffect>();

        }

        EventDelegate.Add(eeff.onFinished, delegate()
        {
 
            Destroy(m_center_label.gameObject.GetComponent<TypewriterEffect>());
        });
        if (game_data._instance.m_language == e_language.English)
        {
            eeff.charsPerSecond = 80;
        }
        else
        {
            eeff.charsPerSecond = 40;
        }
        eeff.enabled = true;
        eeff.mFullText = m_center_text;
    }
    
	public void set_shake(float value)
	{
		m_shake = value;
	}
	public GameObject get_object(string name)
	{
		for(int i = 0;i < m_objects.Count;i ++)
		{
			if(m_objects[i] != null && m_objects[i].transform.name == name)
			{
				return m_objects[i];
			}
		}

		return null;
	}
	public GameObject get_unit(string name)
	{
		for(int i = 0;i < m_units.Count;i ++)
		{
			if(m_units[i] != null && m_units[i].transform.name == name)
			{
				return m_units[i];
			}
		}
		return null;
	}
	public void show_object(string name)
	{
		GameObject _object = get_object (name);

		_object.SetActive (true);
	}
	public void hide_object(string name)
	{
		GameObject _object = get_object (name);
		
		_object.SetActive (false);
	}
	public void play_anim(string name,string anim)
	{
		GameObject _object = get_object (name);

		_object.GetComponent<Animator>().Play (anim);
	}
	public void play_mus(string name,bool loop = true)
	{
		if(m_play_mus == null)
		{
			m_play_mus = transform.gameObject.AddComponent<AudioSource>();
			m_play_mus.volume = 0.0f;
		}

		m_mus_name = name;
		m_stop_mus = true;
	}
	public Texture2D get_texture(string name)
	{
		for(int i = 0;i < m_textures.Length;i ++)
		{
			if(m_textures[i].name == name)
			{
				return m_textures[i];
			}
		}
		return null;
	}
    public void enddirector()
    {
        m_pos_id ++;
        m_left_lable.SetActive(false);
    }
	public void play_sound(string name)
	{
		if(name.Length == 0)	{
			return ;
		}
		
		AudioClip _clip = game_data._instance.get_object_res(name, typeof(AudioClip)) as AudioClip;
		
		if(_clip == null)
		{
			return;
		}
		
		AudioSource _source = transform.gameObject.AddComponent<AudioSource>();
		
		_source.clip = _clip;
		_source.Play();
		
		Object.Destroy(_source,_clip.length + 1.0f);
	}

	// Update is called once per frame
	void Update () {
	
		if (m_shake > 0) {
			m_shake -= Time.deltaTime * 10;

			if (m_shake <= 0) {
				m_shake = 0;
			}

			Vector3 _pos = m_root.transform.localPosition;

			_pos.x = Random.Range (-m_shake, m_shake);
			_pos.y = Random.Range (-m_shake, m_shake);

			m_root.transform.localPosition = _pos;
		}

		if (m_play_mus != null) {
			if (m_stop_mus == true) {
				if (m_play_mus.volume > 0) {
					m_play_mus.volume -= Time.deltaTime;
				}
				
				if (m_play_mus.volume <= 0) {
					m_stop_mus = false;
					m_play_mus.volume = 0.0f;
					m_play_mus.Stop ();
					
					if (m_mus_name.Length > 0) {
						AudioClip _clip = game_data._instance.get_object_res (m_mus_name, typeof(AudioClip)) as AudioClip;
						
						if (_clip != null) {
							m_play_mus.clip = _clip;
							m_play_mus.loop = true;
							m_play_mus.Play ();
						}
					}
				}
			} else if (m_play_mus.isPlaying && m_play_mus.volume < 1.0f) {
				m_play_mus.volume += Time.deltaTime;
			}
		}
		
		if (lookat_obj!= null && m_lookat_target != null)
		{
			lookat_obj.transform.position = Vector3.MoveTowards(lookat_obj.transform.position, m_lookat_target, m_lookat_speed * Time.deltaTime);
		}
		if (campos_obj!= null && m_campos_target != null)
		{
			campos_obj.transform.position = Vector3.MoveTowards(campos_obj.transform.position, m_campos_target, m_campos_speed * Time.deltaTime);
		}

		if (m_wait > 0) {
			m_wait -= Time.deltaTime;
			return;
		}

		if (m_directors.Count > m_pos_id) {
			s_message _msg = m_directors [m_pos_id];

			if (_msg.m_type == "play_sound") {
				play_sound ((string)_msg.m_string [0]);
			} else if (_msg.m_type == "add_unit") {
				GameObject _ins = game_data._instance.ins_object_res ("unit" + "/" + (string)_msg.m_string [0] + "/" + (string)_msg.m_string [0]);
				_ins.transform.name = (string)_msg.m_string [1];
				_ins.transform.parent = this.transform;
				_ins.SetActive (false);

				Vector3 _pos = new Vector3 ();
				
				_pos.x = float.Parse ((string)_msg.m_string [2]);
				_pos.y = float.Parse ((string)_msg.m_string [3]);
				_pos.z = float.Parse ((string)_msg.m_string [4]);

				_ins.transform.position = _pos;

				float _angle = float.Parse ((string)_msg.m_string [5]);
				_ins.transform.localEulerAngles = new Vector3 (0, _angle, 0);

				m_units.Add (_ins);
			} else if (_msg.m_type == "add_objcet") {
				GameObject _ins = game_data._instance.ins_object_res ((string)_msg.m_string [0]);
				
				_ins.transform.name = (string)_msg.m_string [1];
				_ins.transform.parent = this.transform;
				_ins.SetActive (false);


				Vector3 _pos = new Vector3 ();
				
				_pos.x = float.Parse ((string)_msg.m_string [2]);
				_pos.y = float.Parse ((string)_msg.m_string [3]);
				_pos.z = float.Parse ((string)_msg.m_string [4]);

				_ins.transform.position = _pos;

				float _angle = float.Parse ((string)_msg.m_string [5]);
				_ins.transform.localEulerAngles = new Vector3 (0, _angle, 0);

				m_objects.Add (_ins);
			} else if (_msg.m_type == "play_music") {
				if (sys._instance != null) {
					sys._instance.play_mus ((string)_msg.m_string [0], false);
				} else {
					play_mus ((string)_msg.m_string [0]);
				}
			} else if (_msg.m_type == "effect_label") {
				TypewriterEffect _effect = m_effect_lable.AddComponent<TypewriterEffect>();
				_effect.charsPerSecond = 10;

				_effect.mFullText = game_data._instance.get_t_language ((string)_msg.m_string [0]);
				//m_effect_lable.GetComponent<UILabel>().text = (string)_msg.m_string[0];

			} else if (_msg.m_type == "set_center_label") {
				set_center_label (game_data._instance.get_t_language ((string)_msg.m_string [0]));
			} else if (_msg.m_type == "set_center_label_ani") {
				set_center_label_ani (game_data._instance.get_t_language ((string)_msg.m_string [0]));
			} else if (_msg.m_type == "set_film_label") {
				set_film_label (game_data._instance.get_t_language ((string)_msg.m_string [0]));
			} else if (_msg.m_type == "set_shake") {
				set_shake (float.Parse ((string)_msg.m_string [0]));
			} else if (_msg.m_type == "show_object") {
				show_object ((string)_msg.m_string [0]);
			} else if (_msg.m_type == "hide_object") {
				hide_object ((string)_msg.m_string [0]);
			} else if (_msg.m_type == "play_anim") {
				play_anim ((string)_msg.m_string [0], (string)_msg.m_string [1]);
			} else if (_msg.m_type == "wait") {
				m_wait = float.Parse ((string)_msg.m_string [0]);
			} else if (_msg.m_type == "hide_center_label") {
				TweenAlpha _alpha = TweenAlpha.Begin (m_center_label, 0.5f, 1);

				_alpha.method = UITweener.Method.EaseInOut;
				_alpha.from = 1;
				_alpha.to = 0;
				_alpha.delay = 0;
			} else if (_msg.m_type == "set_back") {
				m_back.GetComponent<UITexture>().mainTexture = get_texture ((string)_msg.m_string [0]);
			} else if (_msg.m_type == "guide" && sys._instance != null) {
				root_gui._instance.m_hall_guide = (string)_msg.m_string [0];
			} else if (_msg.m_type == "show_unit") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				_unit.SetActive (true);
			} else if (_msg.m_type == "unit_die") {
				Destroy (get_unit ((string)_msg.m_string [0]), 1.0f);
			} else if (_msg.m_type == "lookat_moveto") {
				m_lookat_target = new Vector3 (float.Parse ((string)_msg.m_string [0]), float.Parse ((string)_msg.m_string [1]), float.Parse ((string)_msg.m_string [2]));
				m_lookat_speed = float.Parse ((string)_msg.m_string [3]);                
			} else if (_msg.m_type == "campos_moveto") {
				m_campos_target = new Vector3 (float.Parse ((string)_msg.m_string [0]), float.Parse ((string)_msg.m_string [1]), float.Parse ((string)_msg.m_string [2]));
				m_campos_speed = float.Parse ((string)_msg.m_string [3]);
			} else if (_msg.m_type == "mask_open") {
                
				top_mask.GetComponent<UITexture>().color = new Color (float.Parse ((string)_msg.m_string [0]) / 255f, float.Parse ((string)_msg.m_string [1]) / 255f, float.Parse ((string)_msg.m_string [2]) / 255f);
				sys._instance.add_alpha_anim (top_mask, float.Parse ((string)_msg.m_string [3]), float.Parse ((string)_msg.m_string [4]), float.Parse ((string)_msg.m_string [5]), 0);
 
			} else if (_msg.m_type == "mask_off") {
				sys._instance.add_alpha_anim (top_mask, 0.5f, 1.0f, 0.0f, 0);

			} else if (_msg.m_type == "show_unit_kqjc") {
				GameObject _unit = game_data._instance.ins_object_res ("unit" + "/" + (string)_msg.m_string [0] + "/" + (string)_msg.m_string [1]);
				s_t_class t_class = game_data._instance.get_t_class (int.Parse ((string)_msg.m_string [3]));

				if (int.Parse ((string)_msg.m_string [9]) != 0) {
					s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan (int.Parse ((string)_msg.m_string [9]));
					if (t_guanghuan != null && game_data._instance.m_player_data.m_guanghuan == 1) {
						GameObject _res = game_data._instance.get_object_res ("effect/" + t_guanghuan.effect, typeof(GameObject)) as GameObject;
						GameObject _eff = (GameObject)Object.Instantiate (_res);
						_eff.transform.parent = _unit.transform;
						_eff.transform.localEulerAngles = new Vector3 (0, 0, 0);
						_eff.transform.localPosition = new Vector3 (0, 0, 0);
						_eff.transform.localScale = Vector3.one;
					}

				}
				_unit.GetComponent<unit>().m_die_wait = 0.5f;
				_unit.GetComponent<unit>().m_die = true;
				_unit.GetComponent<unit>().m_pinzhi = int.Parse ((string)_msg.m_string [2]);
				_unit.GetComponent<unit>().m_max_hp = int.Parse ((string)_msg.m_string [4]);
				_unit.GetComponent<unit>().m_cur_hp = int.Parse ((string)_msg.m_string [4]);
				_unit.GetComponent<unit>().m_t_class = t_class;

				_unit.transform.name = (string)_msg.m_string [1];
				//_ins.transform.parent = this.transform;            

				Vector3 _pos = new Vector3 ();

				_pos.x = float.Parse ((string)_msg.m_string [5]);
				_pos.y = float.Parse ((string)_msg.m_string [6]);
				_pos.z = float.Parse ((string)_msg.m_string [7]);

				_unit.transform.position = _pos;

				float _angle = float.Parse ((string)_msg.m_string [8]);
				_unit.transform.localEulerAngles = new Vector3 (0, _angle, 0);
				_unit.GetComponent<unit>().create_mini_pro_director (t_class.name);
				_unit.SetActive (true);
				m_units.Add (_unit);

			} else if (_msg.m_type == "hide_unit") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				_unit.SetActive (false);
			} else if (_msg.m_type == "set_game_speed") {
				Time.timeScale = float.Parse ((string)_msg.m_string [0]);
			} else if (_msg.m_type == "hide_unit_director") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);
				_unit.GetComponent<unit>().m_mini_hp_pro.SetActive (false);
				_unit.SetActive (false);
			} else if (_msg.m_type == "unit_action") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				_unit.GetComponent<unit>().action ((string)_msg.m_string [1]);
			} else if (_msg.m_type == "unit_action_atk") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				_unit.GetComponent<unit>().action ((string)_msg.m_string [1]);


				List<Transform> m_target_ = new List<Transform>();

				for (int i = 2; i < _msg.m_string.Count; i++) {
					m_target_.Add (get_unit ((string)_msg.m_string [i]).transform);
				}

				if (m_target_.Count > 0) {

					_unit.GetComponent<unit>().m_target_unit.Clear ();
					_unit.GetComponent<unit>().m_target_unit = m_target_;

					_unit.GetComponent<unit>().m_target_pos = new Vector3 (get_unit ((string)_msg.m_string [2]).transform.position.x, get_unit ((string)_msg.m_string [2]).transform.position.y, get_unit ((string)_msg.m_string [2]).transform.position.z + 2 * -_unit.transform.forward.z);
				}


			} else if (_msg.m_type == "skip2game") {

                if (int.Parse((string)_msg.m_string[0]) == 0)
                {
                    m_skip2game.SetActive(false);
                }
                else
                {
                    m_skip2game.SetActive(true);
                }
				
				

			} else if (_msg.m_type == "skip2next") {

                if (int.Parse((string)_msg.m_string[0]) == 0)
                {
                    m_skip2next.SetActive(false);
                }
                else
                {
                    m_skip2next.SetActive(true);
                }
				
			} else if (_msg.m_type == "show_attack_num_dir") {
				List<Transform> m_target_ = new List<Transform>();
				for (int i = 2; i < _msg.m_string.Count; i++) {
					m_target_.Add (get_unit ((string)_msg.m_string [i]).transform);
				}

				for (int i = 0; i < m_target_.Count; i++) {
					// _unit.GetComponent<unit>().m_accept = m_target_[i].gameObject;

					m_target_ [i].gameObject.GetComponent<unit>().show_attack_num (int.Parse ((string)_msg.m_string [0]), int.Parse ((string)_msg.m_string [1]), "");

				}

			} else if (_msg.m_type == "show_skill_name") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				Vector3 _v3_pos = new Vector3 (_unit.transform.position.x, _unit.transform.position.y, _unit.transform.position.z);
				Vector3 _pos = sys._instance.WorldToScreenPoint (_v3_pos);

				GameObject _ins = game_data._instance.ins_object_res ("ui/skill_name");

				UILabel _label = _ins.transform.GetChild (0).Find("Label").GetComponent<UILabel>();

				_label.text = game_data._instance.get_t_language ((string)_msg.m_string [1]);

				_ins.SetActive (true);
				_ins.transform.GetComponent<attack_num_show>().m_position = _v3_pos;
				//_label.transform.GetComponent<UIPanel>().depth = 100;
				_ins.transform.parent = root_gui._instance.m_ui_bottomleft_1.transform;
				_ins.transform.position = new Vector3 (0, 0, 0);
				_ins.transform.localPosition = new Vector3 (_pos.x, _pos.y, 0);
				_ins.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);

				Object.Destroy (_ins, 1.2f);
			} else if (_msg.m_type == "guide") {
				root_gui._instance.action_guide ((string)_msg.m_string [0]);
			} else if (_msg.m_type == "unit_moveto") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				Vector3 _target = new Vector3 ();

				_target.x = float.Parse ((string)_msg.m_string [1]);
				_target.y = float.Parse ((string)_msg.m_string [2]);
				_target.z = float.Parse ((string)_msg.m_string [3]);
				float _time = float.Parse ((string)_msg.m_string [4]);

				TweenPosition _effect = TweenPosition.Begin (_unit, _time, _unit.transform.position);
				_effect.to = _target;

				_unit.GetComponent<unit>().m_start_pos = _target;
			} else if (_msg.m_type == "unit_pos") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				Vector3 _pos = new Vector3 ();

				_pos.x = float.Parse ((string)_msg.m_string [1]);
				_pos.y = float.Parse ((string)_msg.m_string [2]);
				_pos.z = float.Parse ((string)_msg.m_string [3]);

				_unit.transform.position = _pos;

				_unit.GetComponent<unit>().m_start_pos = _pos;
			} else if (_msg.m_type == "unit_target") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				Vector3 _target = new Vector3 ();

				_target.x = float.Parse ((string)_msg.m_string [1]);
				_target.y = float.Parse ((string)_msg.m_string [2]);
				_target.z = float.Parse ((string)_msg.m_string [3]);
				float _time = float.Parse ((string)_msg.m_string [4]);

				_unit.GetComponent<unit>().m_target_pos = _target;
			} else if (_msg.m_type == "unit_target_ex") {
				GameObject _release = get_unit ((string)_msg.m_string [0]);
				GameObject _target = get_unit ((string)_msg.m_string [1]);

				_release.GetComponent<unit>().target_unit (_target.transform.position, _target.GetComponent<unit>().get_bound ().z);
				_release.GetComponent<unit>().m_target_unit.Clear ();
				_release.GetComponent<unit>().m_target_unit.Add (_target.transform);
			} else if (_msg.m_type == "film_speed") {
				float _scale = float.Parse ((string)_msg.m_string [0]);

				Time.timeScale = _scale;
			} else if (_msg.m_type == "unit_angle") {
				GameObject _unit = get_unit ((string)_msg.m_string [0]);

				float _angle = float.Parse ((string)_msg.m_string [1]);
				_unit.transform.localEulerAngles = new Vector3 (0, _angle, 0);
			} else if (_msg.m_type == "load_scene" && sys._instance != null) {
				if (sys._instance.m_is_director == true) {
					sys._instance.m_is_director = false;
					sys._instance.m_root_unit.SetActive (true);
					root_gui._instance.m_ui_bottomleft.SetActive (true);
					root_gui._instance.m_ui_bottomleft_1.SetActive (true);
				}

				sys._instance.load_scene ((string)_msg.m_string [0]);

				if (_msg.m_string.Count > 1) {
					s_message _out_msg = new s_message ();

					_out_msg.time = 0.2f;
					_out_msg.m_type = "show_director";
					_out_msg.m_string.Add ((string)_msg.m_string [1]);

					cmessage_center._instance.add_message (_out_msg);
				}
			} else if (_msg.m_type == "start_game") {
				if (sys._instance.m_is_director == true) {
					sys._instance.m_is_director = false;
					sys._instance.m_root_unit.SetActive (true);
					root_gui._instance.m_ui_bottomleft.SetActive (true);
					root_gui._instance.m_ui_bottomleft_1.SetActive (true);
				}

				sys._instance.m_game_state = "hall";
				sys._instance.load_scene (sys._instance.m_hall_name);
			} else if (_msg.m_type == "hide_director" && sys._instance.m_game_state == "buttle") {
				s_message _out_message = new s_message ();

				_out_message.m_type = "hide_director";

				cmessage_center._instance.add_message (_out_message);

				Time.timeScale = sys._instance.get_game_speed ();
			}

			m_pos_id++;
		}
	}
}
