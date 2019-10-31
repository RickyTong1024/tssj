using UnityEngine;
using UnityEngine.AI;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public struct s_part
{	

	public string type;
	public GameObject obj;
};

public class s_action
{	
	public float m_time;
	public string m_type;
	public ArrayList m_floats = new ArrayList();
	public ArrayList m_ints = new ArrayList();	
	public ArrayList m_string = new ArrayList();	
};

public class s_action_set
{	
	public string m_name;
	public float m_speed = 1.0f;
	public int m_num;
	public int m_step = 0;
	public List<Transform> m_target_unit = new List<Transform>();
	public ArrayList m_actions = new ArrayList();
};

public class unit : MonoBehaviour,IMessage {
	public GameObject m_show_name;
	public GameObject m_mini_hp_pro;
    public GameObject m_mini_hp_player;
	public GameObject m_mini_hp_pet;
	public TextAsset m_xml_asset;

	public string m_show_icon;
	public int m_site = 99999999;

	public GameObject m_cam = null;
	public List<int> m_aspects;
	public List<int> m_aspects_level;

	public ArrayList m_parts = new ArrayList();
	
	public string m_action_name = "ready";

	XDocument m_t_unit = null;

	public string m_config;
	private ArrayList m_action = null;
	private ArrayList m_actions = new ArrayList();
	private s_action_set m_action_set;
	private float m_action_time = 0;

	private int m_action_pos = 0;

	//private int m_skill_id = 0;
	public List<Transform> m_target_unit = new List<Transform>();
	public GameObject[] m_effect_objects;
	public GameObject m_accept;
	public Vector3 m_start_pos = new Vector3();
	public Vector3 m_target_pos = new Vector3(0,0,0);
	
	public float m_while = 0.0f;

	public float m_alpha = 0.0f;
	public float m_target_alpha = 1.0f;

	public float m_angle = 0.0f;
	public bool m_is_edit = false;
	

	public float m_destroy_time = 0.0f;

	public float m_name_height = 1.0f;

	public int m_camp = 0; // 阵营
	public int m_battle_id = 0;
	public bool m_start_battle = false;
	public bool m_end_battle = false;

	public float m_battle_end_delay = 0.0f;

	private skill_ex m_skill;
	public bool m_release_skill = false;
	//private int 
	public ArrayList m_renderer = new ArrayList();

	public float m_spell_time = 0;

	public GameObject m_spell_target;

	public float m_move_cur_time = 0.0f;
	public float m_move_end_time = 0.0f;
	public float m_move_speed = 2.0f;
	public bool pet_end = false;
	
	public bool m_revive = false;
	public bool m_double_attack = false;
	public double m_revive_hp = 0.0f;

	public int m_att_type = 0;
	public double m_att_value = 0;

	public List<s_message> m_attack_num = new List<s_message>();

	public float m_attack_num_time = 0.0f;

	public string m_tag_name = "-V-";

	public float m_scale = 1.0f;
	private float m_base_scale = 1.0f;

	public bool m_out_next = true;

	List<s_t_skill> m_halos = new List<s_t_skill>();

	public List<Vector2> m_buffer_outs = new List<Vector2>();
	
	public float m_die_wait = 0.0f;
	public float m_fuhuo_wait = 0.0f;
	public bool m_fuhuo = false;
	public bool m_die = false;
	public int show_pet = 0; // 0隐藏宠物 1 显示我方宠物 2 显示敌方宠物
	public int battle_start = 0; // 0第一次进入战斗 1不是第一次进入战斗
	public float m_first_wait = 0.0f;
	public bool m_first = false;
	public bool m_fuhuo1 = false;

	public List<GameObject> m_wings = new List<GameObject>();

	public List<string> m_default_parts = new List<string>();

	private float m_wait = 0;

	private float m_attack_end_time = 0.0f;
	private float m_skill_end_time = 0.0f;

	public float m_dance = 0.0f;

	public GameObject m_die_effect;
	public GameObject m_damaged_effect;
	public float m_die_time = 0.0f;

	public double m_max_hp = 0;
	public double m_cur_hp = 0;
    public double m_max_hd = 0;
    public double m_cur_hd = 0;
	public int m_max_mp = 4;
	public int m_cur_mp = 0;
	public int m_glevel = 0;
	public int m_jlevel = 0;
	public int m_pinzhi = 0;

	public s_t_class m_t_class;	
	public ccard m_card;
	public s_t_pet m_t_pet;
	public pet m_pet;
    public string m_restrainskill_effect;

	private List<s_t_skill> m_buffers = new List<s_t_skill>();
	private List<int> m_cols = new List<int>();

	public Dictionary<string, GameObject> m_add_effects = new Dictionary<string, GameObject>();
	public List<SmoothColliderClass> m_joint_colliders = new List<SmoothColliderClass> ();

	public void set_layer(int layer)
	{
		Transform[] hips = transform.GetComponentsInChildren<Transform>();
		
		foreach(Transform hip in hips)
		{		
			hip.gameObject.layer = layer;
		}
	}

	void add_joint(GameObject bone)
	{
		SmoothJoint[] _copy_hips = bone.GetComponentsInChildren<SmoothJoint>();
		
		foreach (SmoothJoint joint in _copy_hips)
		{
			joint.Centroid = this.gameObject;
			
			if (joint != null)
			{
				joint.CollList = new List<SmoothColliderClass>();
				
				for (int x = 0; x < m_joint_colliders.Count; x++)
				{
					SmoothColliderClass[] _colliders = m_joint_colliders[x].GetComponents<SmoothColliderClass>();
					
					for (int y = 0; y < _colliders.Length; y++)
					{
						joint.CollList.Add(_colliders[y]);
					}
				}
			}
		}

	}

	public void add_addeffect(string name, string name1)
	{	
		if (m_add_effects.ContainsKey(name))
		{	
			return;
		}
		GameObject _obj = game_data._instance.ins_object_res(name1);			
		_obj.transform.parent = transform;
		_obj.transform.localPosition = new Vector3 (0, 0, 0);
		_obj.transform.localEulerAngles = new Vector3 (0, 0, 0);
		_obj.transform.localScale = new Vector3 (1, 1, 1);

		m_add_effects.Add (name, _obj);
	}

	public void remove_addeffect(string name)
	{	
		if (m_add_effects.ContainsKey(name))
		{	
			Object.Destroy(m_add_effects[name]);
			m_add_effects.Remove(name);
		}
	}

	public void show_attack_num(int type, double value,string value_type,bool buffer = false)
	{	

		if (value <= 0)
		{

			value = 1;
		}

		if(type == 1)
		{	
			show_attack_num ((int)value,"red",value_type,buffer);
			m_while = 1.0f;
			m_cur_hp -= value;
		}
		else if (type == 2)
		{	
			show_attack_num ((int)value,"blue",value_type,buffer);
			m_while = 1.0f;
			m_cur_hp -= value;
		}
		else
		{
			show_attack_num ((int)value,"green",value_type,false);
			m_cur_hp += value;
		}
	}
	public void show_attack_num(int num,string type,string value_type,bool buffer)
	{	

		Vector3 _pos = get_accept_pos ();

		s_message _new_msg = new s_message();
		
		_new_msg.m_type = "attack_num";
		
		_new_msg.m_string.Add(type);
		_new_msg.m_string.Add(value_type);
		_new_msg.m_floats.Add(_pos.x);
		_new_msg.m_floats.Add(_pos.y);
		_new_msg.m_floats.Add(_pos.z);
		_new_msg.m_long.Add((long)num);
		_new_msg.m_bools.Add (buffer);
		
		m_attack_num.Add(_new_msg);
	}
	public void show_att_modify_effect(string type,float add,string ex = "null")
	{

		Vector3 _pos = get_accept_pos ();
		
		s_message _new_msg = new s_message();
		
		_new_msg.m_type = "modify_effect";
		
		_new_msg.m_string.Add(type);
		_new_msg.m_string.Add (ex);
		_new_msg.m_floats.Add(_pos.x);
		_new_msg.m_floats.Add(_pos.y);
		_new_msg.m_floats.Add(_pos.z);
		_new_msg.m_floats.Add (add);
		
		m_attack_num.Add(_new_msg);
	}
    public void show_trigger_restrainskill_effect()
    {	

            if (this.m_t_class.job == 1)
            {	


                m_restrainskill_effect = "effect/buff01_fy01";

                GameObject _effect = game_data._instance.ins_object_res("effect/buff01_fy01");
                _effect.transform.localScale = new Vector3(1, 1, 1);
                _effect.transform.parent = this.transform;
                _effect.transform.name = "buff01_fy01";
                _effect.transform.localPosition = new Vector3(0, m_name_height / transform.localScale.y, 0);
                Destroy(_effect,2f);
                Debug.Log(m_t_class.name + game_data._instance.get_t_language ("unit.cs_267_43"));//:释放坚如磐石
 
            }
            else if (this.m_t_class.job == 3)
            {	


                m_restrainskill_effect = "effect/buff01_mx01";


                GameObject _effect = game_data._instance.ins_object_res("effect/buff01_mx01");
                _effect.transform.localScale = new Vector3(1, 1, 1);
                _effect.transform.parent = this.transform;
                _effect.transform.localPosition = new Vector3(0, m_name_height / transform.localScale.y, 0);
                _effect.transform.name = "buff01_mx01";
                Debug.Log(m_t_class.name + game_data._instance.get_t_language ("unit.cs_281_43"));//:释放冥想境界
                Destroy(_effect, 2f);


 
            }
            else if (this.m_t_class.job == 2)
            {	


                m_restrainskill_effect = "effect/buff01_zj01";


                GameObject _effect = game_data._instance.ins_object_res("effect/buff01_zj01");
                _effect.transform.localScale = new Vector3(1, 1, 1);
                _effect.transform.parent = this.transform;
                _effect.transform.name = "buff01_zj01";
                _effect.transform.localPosition = new Vector3(0, m_name_height / transform.localScale.y, 0);
                Debug.Log(m_t_class.name + game_data._instance.get_t_language ("unit.cs_298_43"));//:释放御魔追击
                Destroy(_effect, 2f);



            }
          
        
 
    }
	public void buffer(int id, int col, bool add)
	{

		s_t_skill _t_skill = game_data._instance.get_t_skill(id);

		if(add)
		{

			buffer(id, col,false);

			m_buffers.Add (_t_skill);
			m_cols.Add (col);
			if(m_mini_hp_pro != null)
			{

				m_mini_hp_pro.GetComponent<mini_hp_pro>().set_buffer (m_buffers, m_cols);
			}
		}
		else
		{
			for(int i = 0;i < m_buffers.Count;i ++)
			{

				if(m_buffers[i].id == id && m_cols[i] == col)
				{

					m_buffers.RemoveAt(i);
					m_cols.RemoveAt (i);
					break;
				}
			}

			if(m_mini_hp_pro != null)
			{

				m_mini_hp_pro.GetComponent<mini_hp_pro>().set_buffer (m_buffers, m_cols);
			}

			if(_t_skill.buffer_modify_att_types[col] == 13)
			{

				remove_effect("effect_xuanyun");
			}

			return;
		}


		//int _att = game_data._instance.get_value(_t_skill.buffer_modify_att_type);
		int _att = _t_skill.buffer_modify_att_types[col];
		float _val = _t_skill.buffer_modify_att_vals[col];
		
		switch(_att)
		{
		case 13:
		{
			if(_val > 0)
			{

				if(this.transform.Find("effect_xuanyun") == null)
				{

					GameObject _effect = game_data._instance.ins_object_res("effect/npc/pt_ef_xy01");
					_effect.transform.localScale = new Vector3(1,1,1);
					_effect.transform.parent = this.transform;
					_effect.transform.name = "effect_xuanyun";
					_effect.transform.localPosition = new Vector3(0,m_name_height / transform.localScale.y,0);
					sys._instance.play_sound("sound/stun");
				}
			}
		}
			break;
		case 7:
		{
			if(_val > 0)
			{

				show_att_modify_effect("gj_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else 
			{
				show_att_modify_effect("gj_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
		case 8:
		{		
			if(_val > 0)
			{

				show_att_modify_effect("wf_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
				show_att_modify_effect("wf_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
		case 9:
		{		
			if(_val > 0)
			{

				show_att_modify_effect("mf_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
				show_att_modify_effect("mf_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
		case 14:
		{
			if(_val> 0)
			{

				show_att_modify_effect("wm_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
				show_att_modify_effect("wm_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
        case 15:
            {
                if (_val > 0)
                {

                    show_att_modify_effect("mm_word", 1);
                    sys._instance.play_sound("sound/attr_up");
                }
                else
                {
                    show_att_modify_effect("mm_word_01", -1);
                    sys._instance.play_sound("sound/attr_down");
                }
            }
            break;
		case 16:
		{
			if(_val > 0)
			{

                show_att_modify_effect("minz_word", 1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
                show_att_modify_effect("minz_word_01", -1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
        case 17:
            {
                if (_val > 0)
                {

                    show_att_modify_effect("shbi_word", 1);
                    sys._instance.play_sound("sound/attr_up");
                }
                else
                {
                    show_att_modify_effect("shbi_word_01", -1);
                    sys._instance.play_sound("sound/attr_down");
                }
            }
            break;
        case 18:
            {
                if (_val > 0)
                {

                    show_att_modify_effect("kab_word", 1);
                    sys._instance.play_sound("sound/attr_up");
                }
                else
                {
                    show_att_modify_effect("kab_word_01", -1);
                    sys._instance.play_sound("sound/attr_down");
                }
            }
            break;
        case 19:
            {
                if (_val > 0)
                {

                    show_att_modify_effect("chut_word", 1);
                    sys._instance.play_sound("sound/attr_up");
                }
                else
                {
                    show_att_modify_effect("chut_word_01", -1);
                    sys._instance.play_sound("sound/attr_down");
                }
            }
            break;
		case 11:
		{
			if(_val > 0)
			{

				show_att_modify_effect("bj_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
				show_att_modify_effect("bj_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
		case 12:
		{
			if(_val > 0)
			{

				show_att_modify_effect("gd_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
				show_att_modify_effect("gd_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
		case 10:
		{
			if(_val > 0)
			{

				show_att_modify_effect("sd_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
				show_att_modify_effect("sd_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
		case 20:
		{
			if(_val > 0)
			{

				show_att_modify_effect("sh_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
				show_att_modify_effect("sh_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
		case 21:
		{
			if(_val > 0)
			{

				show_att_modify_effect("ms_word",1);
				sys._instance.play_sound("sound/attr_up");
			}
			else
			{
				show_att_modify_effect("ms_word_01",- 1);
				sys._instance.play_sound("sound/attr_down");
			}
		}
			break;
		};
	}
	
	public Vector3 get_accept_pos()
	{	

		if(m_accept == null)
		{	
			return transform.position;
		}

		return m_accept.transform.position;
	}

	public void set_wing_level(int level)
	{

		for(int i = 0;i < m_wings.Count;i ++)
		{

			if(level == i)
			{

				m_wings[i].SetActive(true);
			}
			else
			{
				m_wings[i].SetActive(false);
			}
		}

	}
	void Start () {

		cmessage_center._instance.add_handle (this);

		if(m_config.Length == 0) m_config = this.transform.name;

		m_die = false;
		m_fuhuo = false;
		pet_end = false;
		m_first = false;
		m_fuhuo1 = false;
		m_alpha = 0.0f;
		m_target_alpha = 1.0f;
		show_pet = 0;
		battle_start = 0;
		m_first_wait = 0.0f;
		m_start_pos = transform.localPosition;
		if(m_site == 100 || m_site == 101)
		{

			m_first_wait = 2.0f;
			m_first = true;
			alpha_pet();
		}
		else
		{
			alpha ();
		}
		m_base_scale = transform.localScale.y;
		//出生特效

		if(sys._instance != null && gameObject.transform.name != "mm" && m_site != 100 && m_site != 101)
		{

			sys._instance.show_effect (transform.position + new Vector3(0,0.1f,0),"effect/npc/pt_ef_cx01",2.0f);
		}

		Transform _bound_box = transform.Find("xuankuang");
		
		if(_bound_box != null)
		{

			_bound_box.gameObject.SetActive(false);
			//m_name_height = (_bound_box.localScale.y + _bound_box.transform.localPosition.y - _bound_box.localScale.y * 0.5f) * this.transform.localScale.y;
		}

		m_die_wait = 0.0f;
		m_fuhuo_wait = 0.0f;

		for(int i = 0;i < m_effect_objects.Length;i ++)
		{

			Hello_MeleeWeaponTrail _trail = m_effect_objects[i].GetComponent<Hello_MeleeWeaponTrail>();
			
			if(_trail != null)
			{

				_trail.Emit = false;
			}
			else
			{
				m_effect_objects[i].SetActive(false);
			}
		}

		off_receive_shadows (transform);

		/*
		if(sys._instance != null 
		   && m_logic != null
		   && sys._instance.m_self.is_reserve(m_guid))
		{

			s_message _new_msg = new s_message();
			
			_new_msg.m_type = "modify_effect";
			
			_new_msg.m_string.Add("tbsz_word");
			_new_msg.m_floats.Add(m_start_pos.x);
			_new_msg.m_floats.Add(m_start_pos.y + 0.1f);
			_new_msg.m_floats.Add(m_start_pos.z);
			_new_msg.m_floats.Add (1.0f);
			
			cmessage_center._instance.add_message(_new_msg);
		}
		*/
	}

	private void off_receive_shadows(Transform transform)
	{

		for(int i = 0;i < transform.childCount;i ++)
		{

			Transform _unit = transform.GetChild(i);
			
			if(_unit.GetComponent<Renderer>())
			{

				_unit.GetComponent<Renderer>().material.SetFloat("_alpha",m_alpha);
				_unit.GetComponent<Renderer>().receiveShadows = false;
			}
			
			off_receive_shadows(_unit);

		}
	}
	public void load_xml()
	{

		if(m_t_unit != null || m_config == "")
		{

			return;
		}

		m_parts.Clear ();
		m_actions.Clear ();

		m_t_unit = new XDocument();

		string _config = "unit_config/" + m_config;
		m_xml_asset = game_data._instance.get_object_res(_config, typeof(TextAsset)) as TextAsset;
		if(m_xml_asset == null)
		{

			Debug.Log (game_data._instance.get_t_language ("face.cs_226_14") + _config);//角色脚本错误
			return;
		}
		StringReader reader = new StringReader(m_xml_asset.text);
		m_t_unit = XDocument.Load(reader);

		XElement unit_node = m_t_unit.Element("unit");
		IEnumerable<XElement> nodeList = unit_node.Elements("default_part");
		foreach (XElement xe in nodeList)
		{

			string _type = xe.Attribute("name").Value;
			change_part(_type,0);
		}

		if(m_default_parts.Count > 0)
		{

			for(int i = 0;i < m_default_parts.Count;i ++)
			{

				change_part(m_default_parts[i],0);
			}
			m_default_parts.Clear();
		}

		////////////////////////////////////

		if(unit_node.Attribute("name_height") != null)
		{

			string _name_height = unit_node.Attribute("name_height").Value;
			if(_name_height.Length > 0)
			{

				m_name_height = float.Parse(_name_height) * m_scale;
			}

			if(m_name_height < 2.3f)
			{

				m_name_height = 2.3f;
			}
		}

		if(unit_node.Attribute("accept") != null)
		{

			string _name = unit_node.Attribute("accept").Value;

			if(_name.Length > 0)
			{

				Transform _bone = get_bone(_name);

				if(_bone != null)
				{

					m_accept = get_bone(_name).gameObject;
				}
			}
		}
		else
		{
			Transform _bone = get_bone("Bip01 Spine1");

			if(_bone != null)
			{

				m_accept = _bone.gameObject;
			}
			else
			{
				m_accept = this.gameObject;
			}
		}

		if(unit_node.Attribute("speed") != null)
		{

			m_move_speed = int.Parse(unit_node.Attribute("speed").Value);
		}

		nodeList = unit_node.Elements("actions");
		foreach (XElement xe in nodeList)
		{

			string _type = xe.Attribute("name").Value;

			s_action_set _actions = new s_action_set();

			_actions.m_num = 1;
			_actions.m_name = _type;
		
			if(xe.Attribute("speed") != null)
			{

				_actions.m_speed = float.Parse(xe.Attribute("speed").Value);
			}
			//Debug.Log (game_data._instance.get_t_language ("unit.cs_740_16") + _type);//行为读取

			if(xe.Attribute("probability") != null)
			{

				_actions.m_num = int.Parse(xe.Attribute("probability").Value);
			}

			if(xe.Attribute("step") != null)
			{

				_actions.m_step = int.Parse(xe.Attribute("step").Value);
			}
			else
			{
				_actions.m_step = 0;
			}
			
			IEnumerable<XElement> _sub_xml = xe.Elements("atcion");
			float _time = 0;
			foreach (XElement _sub_xe in _sub_xml)
			{

				s_action _action = new s_action();

				_action.m_type = _sub_xe.Attribute("type").Value;
				_action.m_time = float.Parse(_sub_xe.Attribute("time").Value) + _time;

				if(_action.m_type == "anim")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
				}

				if(_action.m_type == "show_effect_object")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
				}

				if(_action.m_type == "show_effect_object_trail")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
				}

				if(_action.m_type == "hide_effect_object_trail")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
				}

				if(_action.m_type == "mm_talk")
				{

					if (_sub_xe.Attribute("v0") != null)
					{

						_action.m_string.Add(game_data._instance.get_t_language(_sub_xe.Attribute("v0").Value));
					}
					if (_sub_xe.Attribute("v1") != null)
					{

						_action.m_string.Add(game_data._instance.get_t_language(_sub_xe.Attribute("v1").Value));
					}
				}

				if(_action.m_type == "mm_face")
				{

					_action.m_string.Add(_sub_xe.Attribute("v0").Value);
				}

				if(_action.m_type == "game_speed")
				{

					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("speed").Value));
				}

				if(_action.m_type == "hide_effect_object")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
				}

				if(_action.m_type == "action")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
				}

				if(_action.m_type == "action_wait")
				{

					//_action.m_floats.Add(float.Parse(_sub_xe.GetAttribute("v0")));

					_time += float.Parse(_sub_xe.Attribute("v0").Value);
				}

				if(_action.m_type == "target_action")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
				}
				
				if(_action.m_type == "shake_cam")
				{

					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("shake").Value));
				}

				if(_action.m_type == "shake_ui")
				{

					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("shake").Value));
				}

				if(_action.m_type == "entity" || _action.m_type == "target_entity" || _action.m_type == "copy_effect_object"  || _action.m_type == "copy_effect_object_ex")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
					
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("remove_time").Value));
				}

				if(_action.m_type == "entity_bone" 
				   || _action.m_type == "target_entity_bone" 
				   || _action.m_type == "target_copy_effect_object")
				{
					_action.m_string.Add(_sub_xe.Attribute("name").Value);
					_action.m_string.Add(_sub_xe.Attribute("bone").Value);
			
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

				if(_action.m_type == "entity_link")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
					_action.m_string.Add(_sub_xe.Attribute("bone").Value);
					
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("remove_time").Value));
				}

				if(_action.m_type == "entity_cast")
				{

					_action.m_string.Add(_sub_xe.Attribute("name").Value);
					_action.m_string.Add(_sub_xe.Attribute("bone").Value);
					_action.m_string.Add(_sub_xe.Attribute("effect").Value);
	
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("speed").Value));
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("cast_remove_time").Value));
					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("remove_time").Value));
				}

				if(_action.m_type == "sound")
				{

					_action.m_string.Add(_sub_xe.Attribute("sound").Value);

					if(_sub_xe.Attribute("loop") != null)
					{

						_action.m_floats.Add(float.Parse(_sub_xe.Attribute("loop").Value));
					}
				}

				if(_action.m_type == "skill_export")
				{

					_action.m_floats.Add(float.Parse(_sub_xe.Attribute("export").Value));
				}

				if(_action.m_type == "skill_end")
				{

					if(_actions.m_name == "attack")
					{

						m_attack_end_time = (float)_action.m_time;
					}
					else
					{
						m_skill_end_time = (float)_action.m_time;
					}

				}
				_actions.m_actions.Add(_action);
			}

			m_actions.Add(_actions);
		}

		if(this.transform.Find("shadow") == null)
		{

			GameObject _shadow = game_data._instance.ins_object_res("ui/shadow");			
			_shadow.transform.parent = transform;
			_shadow.transform.localPosition = new Vector3 (0, 0, 0);
			_shadow.transform.localEulerAngles = new Vector3 (0, 0, 0);
			_shadow.transform.localScale = new Vector3 (1, 1, 1);
		}


		//Debug.Log (game_data._instance.get_t_language ("unit.cs_927_15")); //脚本读取完毕

	}
	private GameObject get_effect_object(string name)
	{

		for(int i = 0;i < m_effect_objects.Length;i ++)
		{

			if(m_effect_objects[i].transform.name == name)
			{

				return m_effect_objects[i];
			}
		}

		return null;
	}
	void show_effect_object(string name,bool show)
	{

		for(int i = 0;i < m_effect_objects.Length;i ++)
		{

			if(m_effect_objects[i].transform.name == name)
			{

				ParticleSystem _par = m_effect_objects[i].GetComponent<ParticleSystem>();

				if(_par != null)
				{

					if(show == true)
					{

						_par.Clear();
						_par.Play();
					}
					else
					{
						_par.Clear();
					}
				}

				m_effect_objects[i].SetActive(show);
			}
		}
	}
	public void set_bh(float bh)
	{

		for(int i = 0;i < transform.childCount;i ++)
		{

			Transform _unit = transform.GetChild(i);
			
			if(_unit.GetComponent<Renderer>())
			{

				_unit.GetComponent<Renderer>().material.SetFloat("_bh",bh);
			}
		}
	}

	public void set_back_color(float back,Color color)
	{

		for(int i = 0;i < transform.childCount;i ++)
		{

			Transform _unit = transform.GetChild(i);
			
			if(_unit.GetComponent<Renderer>())
			{

				_unit.GetComponent<Renderer>().material.SetFloat("_back",back);
				_unit.GetComponent<Renderer>().material.SetColor("_BackColor",color);
			}

			alpha(_unit);
		}
	}

	void alpha(Transform transform)
	{

		for(int i = 0;i < transform.childCount;i ++)
		{

			Transform _unit = transform.GetChild(i);
			if(_unit.GetComponent<Renderer>())
			{

				_unit.GetComponent<Renderer>().material.SetFloat("_alpha",m_alpha);
			}

			alpha(_unit);
		}
	}

	void alpha(bool scale = false)
	{

		float _delta_time = Time.deltaTime * 5;

		if(m_alpha != m_target_alpha)
		{

			if(Mathf.Abs(m_alpha - m_target_alpha) > _delta_time)
			{

				if(m_alpha > m_target_alpha)
				{

					m_alpha -= _delta_time;
				}
				else if(m_alpha < m_target_alpha)
				{

					m_alpha += _delta_time;
				}
			}
			else
			{
				m_alpha = m_target_alpha;
			}

			alpha(transform);

			if(scale == true)
			{

				Vector3 _scale = transform.localScale;

				_scale.x = m_alpha * m_base_scale;
				_scale.z = m_alpha * m_base_scale;

				transform.localScale = _scale;
			}
		}

		if(m_alpha <= 0)
		{

			for(int i = 0;i < transform.childCount;i ++)
			{

				Transform _unit = transform.GetChild(i);
				
				if(_unit.GetComponent<Renderer>() && _unit.GetComponent<Renderer>().enabled == true)
				{

					_unit.GetComponent<Renderer>().enabled = false;
				}
			}
		}
		else
		{
			for(int i = 0;i < transform.childCount;i ++)
			{

				Transform _unit = transform.GetChild(i);
				
				if(_unit.GetComponent<Renderer>() && _unit.GetComponent<Renderer>().enabled == false)
				{

					_unit.GetComponent<Renderer>().enabled = true;
				}
			}
		}
		if(m_alpha == 1 && m_fuhuo1)
		{

			m_fuhuo1 = false;
		}
	}

	void alpha_pet()
	{

		m_alpha = m_target_alpha;
		alpha(transform);
		Vector3 _scale = transform.localScale;

		_scale.x = m_alpha * m_base_scale;
		_scale.z = m_alpha * m_base_scale;

		transform.localScale = _scale;

		for(int i = 0;i < transform.childCount;i ++)
		{

			Transform _unit = transform.GetChild(i);
			
			if(_unit.GetComponent<Renderer>() && _unit.GetComponent<Renderer>().enabled == false)
			{

				_unit.GetComponent<Renderer>().enabled = true;
			}
		}
		if(m_camp == 0)
		{

			transform.transform.localEulerAngles = new Vector3(0,60,0);
			transform.localPosition = m_start_pos + new Vector3(-3.5f,0,3);
			if(sys._instance != null && gameObject.transform.name != "mm")
			{

			 	sys._instance.show_effect (transform.position + new Vector3(0,0.1f,0),"effect/cw_start",1.0f);
			}
		}
		else
		{
			transform.transform.localEulerAngles = new Vector3(0,-120,0);
			transform.localPosition = m_start_pos + new Vector3(3.5f,0,-3);
			if(sys._instance != null && gameObject.transform.name != "mm")
			{

				 sys._instance.show_effect (transform.position + new Vector3(0,0.1f,0),"effect/cw_start",1.0f);
			}
		}

	}

	void alpha_ex()
	{

		float _delta_time = Time.deltaTime*0.9f;
		if(m_target_alpha == 1)
		{

			this.transform.gameObject.SetActive(true);
			m_mini_hp_pro.GetComponent<UIPanel>().alpha = 1;
		}
		if(m_alpha != m_target_alpha)
		{

			if(Mathf.Abs(m_alpha - m_target_alpha) > _delta_time)
			{

				if(m_alpha > m_target_alpha)
				{

					m_alpha -= _delta_time;
				}
				else if(m_alpha < m_target_alpha)
				{

					m_alpha += _delta_time;
				}
			}
			else
			{
				m_alpha = m_target_alpha;
			}
			
			alpha(transform);
			m_mini_hp_pro.GetComponent<UIPanel>().alpha = m_alpha;
		}
		
		if(m_alpha <= 0)
		{

			for(int i = 0;i < transform.childCount;i ++)
			{

				Transform _unit = transform.GetChild(i);
				
				if(_unit.GetComponent<Renderer>() && _unit.GetComponent<Renderer>().enabled == true)
				{

					_unit.GetComponent<Renderer>().enabled = false;
				}
			}
			this.transform.gameObject.SetActive(false);
			m_mini_hp_pro.GetComponent<UIPanel>().alpha = 0;
		}
		else
		{
			for(int i = 0;i < transform.childCount;i ++)
			{

				Transform _unit = transform.GetChild(i);
				
				if(_unit.GetComponent<Renderer>() && _unit.GetComponent<Renderer>().enabled == false)
				{

					_unit.GetComponent<Renderer>().enabled = true;
				}
			}
			this.transform.gameObject.SetActive(true);
			m_mini_hp_pro.GetComponent<UIPanel>().alpha = 1;
		}
	}
	
	void white()
	{

		if(m_while > 0.0f)
		{

			m_while -= Time.deltaTime * 1.0f;
			if(m_while < 0.0f) m_while = 0.0f;

			for(int i = 0;i < transform.childCount;i ++)
			{

				Transform _unit = transform.GetChild(i);

				if(_unit.GetComponent<Renderer>())
				{

					_unit.GetComponent<Renderer>().material.SetFloat("_while",m_while);
				}
			}
		}
	}
	void OnDestroy()
	{

		if(battle._instance != null)
		{

			battle._instance.m_units.Remove (this.gameObject);
		}

		cmessage_center._instance.remove_handle (this);

		if(m_show_name != null)
		{

			Object.Destroy(m_show_name);
		}

		if(m_mini_hp_pro != null)
		{

			Object.Destroy(m_mini_hp_pro);
		}
		if(m_mini_hp_pet != null)
		{

			Object.Destroy(m_mini_hp_pet);
		}

	}

	public Transform get_bone(string bone_name)
	{

		if(bone_name == "accept" )
		{

			if (m_accept == null)
			{

				Debug.Log(game_data._instance.get_t_language ("unit.cs_1225_14"));//accept 不存在
				return transform;
			}
			return m_accept.transform;
		}

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
	public void remove_part(string type)
	{

		Transform[] _hips = transform.GetComponentsInChildren<Transform>();

		foreach(Transform hip in _hips)
		{

			if(hip.name.IndexOf(type) != -1)
			{

				GameObject.Destroy(hip.gameObject);
			}
		}

		for (int i = 0; i < m_parts.Count;)
		{

			s_part _part = (s_part)m_parts[i];
			if (_part.type == type)
			{

				Object.Destroy(_part.obj);
				m_parts.RemoveAt(i);
			}
			else
			{
				++i;
			}
		}
	}
	public void change_static_part(string type,string pack,string bone_name)
	{
		remove_part (type);

		Transform _bone = get_bone (bone_name);

		if(_bone == null)
		{

			Debug.Log(game_data._instance.get_t_language ("unit.cs_1283_13") + bone_name);//找到不到骨骼
			return ;
		}

		GameObject _ins = game_data._instance.ins_object_res(pack);
		_ins.transform.parent = _bone;
		_ins.transform.localPosition = new Vector3 (0, 0, 0);
		_ins.transform.localEulerAngles = new Vector3 (0, 0, 0);
        _ins.transform.localScale = new Vector3(1, 1, 1);

        add_joint (_ins);


		s_part _new_part = new s_part ();
		
		_new_part.type = type;
		_new_part.obj = _ins;

		m_parts.Add (_new_part);

		set_layer (this.gameObject.layer);
	}

	public void change_skin_part(string type,string part,string pack)
	{
		remove_part (type);

		s_part _new_part = new s_part ();

		_new_part.type = type;

		GameObject _copy = game_data._instance.ins_object_res(pack);
		GameObject _obj = null;

		if(part == pack)
		{

			_obj = _copy;
		}
		else
		{
			_obj = _copy.transform.Find(part).gameObject;
		}

		if (_obj == null)
						return;

		SkinnedMeshRenderer smr = _obj.transform.GetComponent<SkinnedMeshRenderer>();

		Transform[] _hips = transform.GetComponentsInChildren<Transform>();

		Transform[] _copy_hips = _copy.GetComponentsInChildren<Transform>();

		List<Transform> bones = new List<Transform>();
		
		foreach(Transform bone in smr.bones)
		{

			foreach(Transform hip in _hips)
			{
				if(hip.name != bone.name) continue;		
				bones.Add(hip);	
				/////////////////////
				//Debug.Log("Type + " + bone.gameObject.GetType());

				foreach(Transform copy_hip in _copy_hips)
				{

					if(copy_hip.name != hip.name) continue;
					//if(copy_hip.childCount == 0 &&　hip.childCount == 0) continue;
					for(int i = 0;i < copy_hip.childCount;i ++)
					{

						GameObject _copy_obj = copy_hip.GetChild(i).gameObject;

						for(int c = 0;c < hip.childCount;c ++)
						{

							if(copy_hip.name == hip.name)
							{

								_copy_obj = null;
								break;
							}
						}

						//if(_copy_obj && _copy_obj.name.IndexOf(type) != -1)
						if(_copy_obj)
						{

							GameObject _new_obj = (GameObject)Object.Instantiate(_copy_obj);

							_new_obj.name = type;
							_new_obj.transform.parent = hip;
							_new_obj.transform.localPosition = _copy_obj.transform.localPosition;
							_new_obj.transform.localRotation = _copy_obj.transform.localRotation;
							_new_obj.transform.localScale = _copy_obj.transform.localScale;
						}
					}

				}

				break;	
			}	
		}
		
		_new_part.obj = new GameObject();
		_new_part.obj.name = type;
		_new_part.obj.transform.parent = transform;


		SkinnedMeshRenderer _target = _new_part.obj.AddComponent<SkinnedMeshRenderer>();
		
		_target.sharedMesh = smr.sharedMesh;
		_target.bones = bones.ToArray();
		_target.materials = smr.materials;

		GameObject.Destroy (_copy);
		_target.GetComponent<Renderer>().material.SetFloat("_alpha",m_alpha);

		if(this.transform.name != "mm")
		{

			gameObject.SetActive (false);
			gameObject.SetActive (true);
		}

		set_layer (this.gameObject.layer);
	}

	public void change_part(string name,int level)
	{

		if (name == null)
		{

			return;
		}

		if(this.gameObject.activeInHierarchy == false)
		{

			return;
		}

		if(m_t_unit == null)
		{

			load_xml();
		}
		
		XElement unit_node = m_t_unit.Element("unit");		
		if(unit_node == null)
		{
			Debug.Log (game_data._instance.get_t_language ("unit.cs_1423_14"));//无角色脚本
		}		
		
		IEnumerable<XElement> nodeList = unit_node.Elements("part");
		foreach (XElement xe in nodeList)
		{

			if(xe.Attribute("name").Value == name)
			{

				IEnumerable<XElement> _sub_xml = xe.Elements("mesh");
				foreach (XElement _sub_xe in _sub_xml)
				{

					if(_sub_xe.Attribute("bone").Value == "")
					{

						change_skin_part(_sub_xe.Attribute("type").Value,_sub_xe.Attribute("part").Value,_sub_xe.Attribute("pack").Value);
					}
					else
					{
						change_static_part(_sub_xe.Attribute("type").Value,_sub_xe.Attribute("pack").Value,_sub_xe.Attribute("bone").Value);
					}
				}
			}
		}
	}

	public void remove_part_name(string name)
	{

		if(m_t_unit == null)
		{

			load_xml();
		}

		XElement unit_node = m_t_unit.Element("unit");		
		if(unit_node == null)
		{

			Debug.Log (game_data._instance.get_t_language ("unit.cs_1423_14"));//无角色脚本
		}
        
		IEnumerable<XElement> nodeList = unit_node.Elements("part");
		foreach (XElement xe in nodeList)
		{

			if(xe.Attribute("name").Value == name)
			{

				IEnumerable<XElement> _sub_xml = xe.Elements("mesh");
				foreach (XElement _sub_xe in _sub_xml)
				{

					remove_part(_sub_xe.Attribute("type").Value);
				}
			}
		}
	}

	public void create_name(string name,int star)
	{

		m_show_name = game_data._instance.ins_object_res("ui/name_label");
		m_show_name.transform.Find("Label").GetComponent<UILabel>().text = name;
		m_show_name.transform.position = new Vector3(0,0,0);
		m_show_name.transform.parent = root_gui._instance.m_ui_bottomleft.transform;
		m_show_name.transform.localScale = new Vector3(1,1,1);
	}
    public void create_mini_pro_director(string name)
    {
        if (sys._instance == null)
            return;

        m_mini_hp_pro = game_data._instance.ins_object_res("ui/mini_hp_pro");
        s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(m_pinzhi);

        m_mini_hp_pro.GetComponent<mini_hp_pro>().init_dir(this.gameObject, ccard.get_color_name(m_t_class.name, t_role_shengpin.color));
        m_mini_hp_pro.transform.position = new Vector3(0, 0, 0);
        m_mini_hp_pro.transform.parent = root_gui._instance.m_ui_bottomleft.transform;
        m_mini_hp_pro.transform.localScale = new Vector3(1, 1, 1);
        if (show_pet == 0)
        {
            m_mini_hp_pro.SetActive(true);
        }
        else
        {
            m_mini_hp_pro.SetActive(false);
        }
    }
	public void create_mini_pro()
	{

		if (sys._instance == null)
			return;

		m_mini_hp_pro = game_data._instance.ins_object_res("ui/mini_hp_pro");
		s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin (m_pinzhi);
		m_mini_hp_pro.GetComponent<mini_hp_pro>().init(this.gameObject, ccard.get_color_name(m_t_class.name, t_role_shengpin.color));
		m_mini_hp_pro.transform.position = new Vector3(0,0,0);
		m_mini_hp_pro.transform.parent = root_gui._instance.m_ui_bottomleft.transform;
		m_mini_hp_pro.transform.localScale = new Vector3(1,1,1);
		if(show_pet == 0)
		{

			m_mini_hp_pro.SetActive (true);
		}
		else
		{
			m_mini_hp_pro.SetActive (false);
		}
	}

	public void create_mini_pet_pro()
	{

		if (sys._instance == null)
			return;
		
		m_mini_hp_pet = game_data._instance.ins_object_res("ui/mini_hp_pro");
		m_mini_hp_pet.GetComponent<mini_hp_pro>().init(this.gameObject, pet.get_color_name(m_t_pet.name,m_t_pet.color));
		m_mini_hp_pet.transform.position = new Vector3(0,0,0);
		m_mini_hp_pet.transform.parent = root_gui._instance.m_ui_bottomleft.transform;
		m_mini_hp_pet.transform.localScale = new Vector3(1,1,1);
		if(show_pet != 0)
		{

			m_mini_hp_pet.SetActive (true);
		}
		else
		{
			m_mini_hp_pet.SetActive (false);
		}
	}

    public GameObject create_mini_pro_player(protocol.team.team_player player)
    {

        GameObject min_hp = game_data._instance.ins_object_res("ui/player_hp_pro");
        min_hp.transform.Find("main/name").GetComponent<UILabel>().text = game_data._instance.get_name_color(player.achieve) + player.name;
        min_hp.transform.Find("main/attack").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("unit.cs_1530_95"),  sys._instance.value_to_wan(player.bf));//战力 {0}
        s_t_bingyuan_chenhao _chenghao = game_data._instance.get_t_bingyuan_chenghao(player.chenhao);
        if (_chenghao != null)
        {

            min_hp.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
        }
        else
        {
            _chenghao = game_data._instance.get_t_bingyuan_chenghao(7);
            min_hp.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
        }
        min_hp.transform.Find("hp").GetComponent<UISlider>().value = 1;
        m_mini_hp_player = min_hp;
        m_mini_hp_player.transform.position = new Vector3(0, 0, 0);
        m_mini_hp_player.transform.parent = root_gui._instance.m_ui_bottomleft.transform;
        m_mini_hp_player.transform.localScale = new Vector3(1, 1, 1);
        m_mini_hp_player.SetActive(true);
        return min_hp;
    }
	public void show_mini_hp(float time)
	{	
		m_mini_hp_pro.SetActive (true);
		m_mini_hp_pro.GetComponent<mini_hp_pro>().m_show_time = time;
	}

	public void show_zskill(string name,string icon)
	{

		m_mini_hp_pro.GetComponent<mini_hp_pro>().set_zskill (name,icon);
	}

	void IMessage.net_message(s_net_message message)
	{	

	}

	void IMessage.message(s_message message)
	{

		if(message.m_type == "move" && m_cam != null)
		{

			destination((float)message.m_floats[0],(float)message.m_floats[1]);
		}

		if(message.m_type == "action")
		{

			action((string)message.m_string[0]);
		}
		else if(message.m_type == "action_ex")
		{

			if(m_card != null && m_card.get_guid() == (ulong)message.m_long[0])
			{

				action((string)message.m_string[0]);
			}
			if(m_pet != null && m_pet.get_guid() == (ulong)message.m_long[1])
			{

				action((string)message.m_string[0]);
			}
		}

		if(message.m_type == "part" && this.gameObject.activeSelf)
		{

			change_part((string)message.m_string[0],0);
		}

		if(message.m_type == "remove_part")
		{

			remove_part_name((string)message.m_string[0]);
		}
		if(message.m_type == "release_skill")
		{

			if(message.m_site == m_site)
			{

				remove_effect("effect_xuanyun");
				remove_effect("effect_jq");

				m_skill = (skill_ex)message.m_object[0];

				if(m_skill.m_t_skill.action != "attack")
				{

					m_battle_end_delay = m_skill_end_time;

					GameObject _ins = game_data._instance.ins_object_res("effect/AllUnit_Spell_LaserEnergy");
					{

						_ins.transform.parent = transform.parent;
						if(m_site == 100 || m_site == 101)
						{

							_ins.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + m_name_height * m_scale*2, transform.localPosition.z);
						}
						else
						{
							_ins.transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + m_name_height * m_scale, transform.localPosition.z);
						}
						_ins.transform.localEulerAngles = new Vector3(0,0,0);

						Transform _bound_box = transform.Find("xuankuang");

						float _scale = 1.0f;
						if(_bound_box != null)
						{

							_scale = _bound_box.transform.localScale.y * 0.5f;
						}
						_ins.gameObject.transform.localScale = new Vector3(_scale,_scale,_scale);

					}
					
					Object.Destroy(_ins,2);

					m_wait = 1.0f;
				}
				else
				{
					m_battle_end_delay = m_attack_end_time;
				}

				m_target_unit.Clear();

				for(int i = 0;i < m_skill.m_skill_target_exs.Count;i ++)
				{

					GameObject _object = battle._instance.get_unit_site(m_skill.m_skill_target_exs[i].m_site);

					if(_object != null)
					{

						if(m_skill.m_t_skill.action == "attack")
						{

							_object.GetComponent<unit>().m_die_wait = m_attack_end_time + 0.5f;
							_object.GetComponent<unit>().m_fuhuo_wait = m_attack_end_time + 0.5f;
						}
						else
						{
							_object.GetComponent<unit>().m_die_wait = m_skill_end_time + 0.5f;
							_object.GetComponent<unit>().m_fuhuo_wait = m_skill_end_time + 0.5f;
						}

						m_target_unit.Add(_object.transform);
					}
				}

				/*for (int i = 0; i < battle._instance.m_units.Count; i++) {
					Material temp = game_data._instance.get_object_res("effect/Materials/a02",typeof(Material)) as Material;
					battle._instance.m_units[i].gameObject.transform.Find("shadow(Clone)/Plane").GetComponent<MeshRenderer>().material = null;
				}*/

				target();

				if(m_target_unit.Count > 0)
				{

					if( m_target_unit[0].GetComponent<unit>().m_camp != m_camp)
					{

						lookat(m_target_pos);
					}
				}

				action(m_skill.m_t_skill.action);

				for(int i = 0;i < battle._instance.m_units.Count; ++i)
				{

					unit ut = battle._instance.m_units[i].GetComponent<unit>();
					if (ut != null && ut.m_site != m_site)
					{

						head_look hl = ut.GetComponent<head_look>();
						if (hl != null)
						{

							hl.look_obj(this.gameObject);
						}
					}
				}
			}
		}
	}
	public Vector3 get_bound()
	{

		Vector3 _bound = new Vector3 ();

		_bound.x = 2;
		_bound.y = 2;
		_bound.z = 2;

		for(int i = 0;i < this.gameObject.transform.childCount;i ++)
		{

			Transform _obj = this.gameObject.transform.GetChild(i);

			if(_obj.name == "xuankuang" && _obj.GetComponent<Renderer>() != null)
			{

				_bound = _obj.GetComponent<Renderer>().bounds.size;
			}
		}

		return _bound;
	}
	public void target_unit(Vector3 target,float bound)
	{	
		Vector3 _pos = target - m_start_pos;
		float _dis = _pos.magnitude - (bound + get_bound().z) * 0.5f;
		
		m_target_pos = _dis * _pos.normalized * 0.8f  + m_start_pos;
	}
	void target()
	{	

		if(m_target_unit.Count == 0)
		{	

			return;
		}

		if(m_target_unit.Count == 1)
		{	
			m_spell_target = m_target_unit[0].gameObject;
			//Material temp = game_data._instance.get_object_res("effect/Materials/a01",typeof(Material)) as Material;
			//m_spell_target.gameObject.transform.Find("shadow(Clone)/Plane").GetComponent<MeshRenderer>().material = temp;
			unit _unit = m_spell_target.GetComponent<unit>();
			target_unit (_unit.m_start_pos,_unit.get_bound().z);
		}
		else
		{
			/*
			Transform _target = null;

			for(int i = 0;i < m_target_unit.Count;i ++)
			{

				if(Mathf.Abs(m_target_unit[i].transform.position.x) < 1.0f)
				{

					_target = m_target_unit[i];
				}
			}

			if(_target != null)
			{

				m_spell_target = _target.gameObject;
				unit _unit = m_spell_target.GetComponent<unit>();
				target_unit (_unit.transform.position,_unit.get_bound().z);
			}
			else
			{
				Vector3 _pos = m_target_unit[0].position;
				_pos.x = 0;
				target_unit (_pos,2);
			}
			*/

			Vector3 _pos = m_target_unit[0].position;
			float _min = _pos.x;
			float _max = _pos.x;

			for(int i = 0;i < m_target_unit.Count;i ++)
			{
//				Material temp = game_data._instance.get_object_res("effect/Materials/a01",typeof(Material)) as Material;
//				m_target_unit[i].gameObject.transform.Find("shadow(Clone)/Plane").GetComponent<MeshRenderer>().material = temp;
				if(_min > m_target_unit[i].position.x)
				{	

					_min = m_target_unit[i].position.x;
				}

				if(_max < m_target_unit[i].position.x)
				{

					_max = m_target_unit[i].position.x;
				}
			}

			_pos.x = _min + (_max - _min) * 0.5f;

			target_unit (_pos,2);
		}
	}
	void lookat(Vector3 pos)
	{

		Vector3 _pos = new Vector3 (pos.x, this.transform.position.y, pos.z);
		this.transform.LookAt(_pos);
	}
	public bool is_self()
	{

		return true;
	}
	bool is_move()
	{

		if(m_start_pos != this.transform.position)
		{

			return true;
		}
		return false;
	}
	public NavMeshAgent get_nav()
	{

		return this.transform.GetComponent<NavMeshAgent>();
	}

	public bool has_atcion(string name)
	{

		for(int i = 0;i < m_actions.Count;i ++ )
		{

			s_action_set _set = (s_action_set)m_actions[i]; 
			
			if(_set.m_name == name)
			{

				return true;
			}
		}

		return false;
	}
	public float action(string name)
	{

		if (name == null || m_action_name == name || name.Length == 0) return 0.0f;
		m_action_name = name;

		for(int i = 0;i < m_effect_objects.Length;i ++)
		{

			Hello_MeleeWeaponTrail _trail = m_effect_objects[i].GetComponent<Hello_MeleeWeaponTrail>();

			if(_trail != null)
			{

				_trail.Emit = false;
			}
			else
			{
				m_effect_objects[i].SetActive(false);
			}
		}

		if(m_battle_end_delay <= 0.0f && m_move_cur_time == m_move_end_time && m_skill != null)
		{

			Vector3 _target = new Vector3();
			
			_target = m_start_pos;
			_target.z = 0;

			lookat(_target);
		}

		List<int> _aspects = new List<int>();

		for(int i = 0;i < m_actions.Count;i ++ )
		{

			s_action_set _set = (s_action_set)m_actions[i]; 

			if(_set.m_name == name)
			{

				for(int c = 0;c < _set.m_num;c ++)
				{

					_aspects.Add(i);
				}
			}
		}

		if(_aspects.Count == 0)
		{

			//Debug.Log(game_data._instance.get_t_language ("face.cs_464_13") + name);//单位行为无效
			return 0.0f;
		}

		int _index = Random.Range (0, _aspects.Count);

		m_action_set = (s_action_set)m_actions [(int)_aspects [_index]];
		m_action = m_action_set.m_actions;
		m_action_pos = 0;
		m_action_time = 0.0f;

		if(m_action_set.m_step == 1)
		{

			if(m_target_unit.Count > 0)
			{

				for(int i = 0;i < m_target_unit.Count;i ++)
				{

					m_action_set.m_target_unit.Add(m_target_unit[i]);
				}
				
				m_target_unit.Clear();
			}

			step_target();
		}
		else
		{
			m_action_set.m_target_unit.Clear();
		}

		float _lenght = 0.0f;
		for(int i = 0;i < m_action.Count;i ++)
		{

			s_action _act = (s_action)m_action[i];

			_lenght = _act.m_time;
		}

		return _lenght;
		//Debug.Log(game_data._instance.get_t_language ("unit.cs_1905_14") + name);//单位行为
	}

	bool step_target()
	{

		if(m_action_set.m_target_unit.Count == 0)
		{

			int _pos = 0;
			float _time = 0.0f;

			for(int i = 0;i < m_action.Count;i ++)
			{

				s_action _action = (s_action)m_action[i];

				if(_action.m_type == "skill_end")
				{

					_pos = i;
					_time = _action.m_time;
				}
			}

			if(m_action_pos < _pos)
			{

				m_action_pos = _pos + 1;
				m_action_time = _time;
			}

			return false;
		}

		m_target_unit.Clear ();
		m_target_unit.Add(m_action_set.m_target_unit[0]);
		
		lookat(m_action_set.m_target_unit[0].position);
		
		m_action_set.m_target_unit.RemoveAt(0);

		return true;
	}

	void stop()
	{

		if (!is_move ())
		{

				return;
		}

		get_nav ().ResetPath ();
	}

	void action_update()
	{

		if(m_action == null) return;

		/*
		if(m_action_wait > 0)
		{

			m_action_wait -= Time.deltaTime * m_action_set.m_speed;

			return;
		}
		*/

		m_action_time += Time.deltaTime * m_action_set.m_speed;

		while(m_action.Count > 0 && m_action_pos < m_action.Count)
		{

			s_action _action = (s_action)m_action[m_action_pos];

			if(m_action_time > _action.m_time)
			{

				m_action_pos ++;

				/*
				if(_action.m_type == "action_wait")
				{

					//m_move_cur_time = 0.001f;
					//m_move_end_time = 0.0f;		
					m_action_wait = (float)_action.m_floats[0];
					
					return;
				}
				*/


				//Debug.Log((string)_action.m_string[0]);

				if(_action.m_type == "anim")
				{

					Animation _animation = transform.GetComponent<Animation>();

					if(_animation != null)
					{

						if(_animation.GetClip( (string)_action.m_string[0]) != null)
						{

							if(transform.GetComponent<Animation>().name == (string)_action.m_string[0])
							{

								transform.GetComponent<Animation>().Stop();
							}
							transform.GetComponent<Animation>().CrossFade((string)_action.m_string[0]);
						}
					}

					Animator _animator = transform.GetComponent<Animator>();
					string _anim_name = (string)_action.m_string[0];

					if(_animator != null && _anim_name != "ready")
					{

						//_animator.get
						_animator.StopPlayback();
						_animator.SetTrigger(_anim_name);
					}
				}

				if(_action.m_type == "action")
				{	
					action((string)_action.m_string[0]);
				}

				if(_action.m_type == "goto_target")
				{

					m_move_end_time = 1.0f;
				}

				if(_action.m_type == "goto_home")
				{

					m_move_end_time = 0.0f;
					if(m_site == 100 || m_site == 101)
					{

						m_move_cur_time = 0.0f;
						m_wait = 0;
					}
					//m_move_cur_time = 0.001f;
				}

				if(_action.m_type == "mm_talk")
				{

					if(root_gui._instance.m_talk != null)
					{

						

						string _text = (string)_action.m_string[0];
						if(_text == null)
						{

							return;
						}
						if(_text == "")
						{

							return;
						}
						root_gui._instance.m_talk.transform.GetChild(0).Find("text").GetComponent<UILabel>().text = _text;

						s_message _mm_talk = new s_message();

						_mm_talk.m_type = "mm_talk";
						_mm_talk.m_string.Add((string)_action.m_string[0]);

						cmessage_center._instance.add_message(_mm_talk);
					}
					else
					{
						//Debug.Log((string)_action.m_string[0]);
						//Debug.Log((string)_action.m_string[1]);
					}
				}

				if(_action.m_type == "mm_face")
				{

					s_message _msg = new s_message();
					_msg.m_type = "face_ex_name";
					_msg.m_string.Add((string)_action.m_string[0]);
					cmessage_center._instance.add_message(_msg);
				}

				if(_action.m_type == "skill_export" && m_skill != null)
				{	

					float _pe = (float)_action.m_floats[0];

					for(int i = 0;i < m_skill.m_skill_target_exs.Count;i ++)
					{	

						skill_target_ex _target = m_skill.m_skill_target_exs[i];

						if(_target == null)
						{	
							continue;
						}

						GameObject _object = battle._instance.get_unit_site(_target.m_site);

						if(_object == null)
						{	
							continue;
						}

						unit _unit = _object.GetComponent<unit>();

						if(_target.m_type > 0)
						{	
							if(_target.m_mp_bh != 0 && m_skill.m_index == 0)
							{	

								if(_target.m_mp_bh == 1)
								{

									_unit.show_att_modify_effect("nl_word",1);
									sys._instance.play_sound("sound/attr_up");
								}
								else
								{
									_unit.show_att_modify_effect("nl_word_01",-1);
									sys._instance.play_sound("sound/attr_down");
								}

								_unit.m_cur_mp = _target.m_mp;
							}

							if(_target.m_type == 4)
							{	




							}
                            else if (_target.m_type == 25)
                            {	

                                if(m_skill.m_index == 0)
                                {	

                                    print(this.m_t_class.name + i + game_data._instance.get_t_language ("unit.cs_2185_68"));//：开始攻击
                                    _unit.GetComponent<unit>().show_trigger_restrainskill_effect(); ;
                                }
                            }
                            else if (_target.m_type == 26)
                            {	

 
                            }
                            else if (_target.m_type == 3)
                            {

                                _unit.show_attack_num(3, _target.m_value, "");
                            }
                            else
                            {
                                if (_target.m_jght > 0)
                                {

                                    sys._instance.show_effect(_unit.get_accept_pos(), "effect/ef_dsj01", 2.0f);
                                }

                                int _value = (int)(_target.m_value * _pe);

                                if (_target.m_mian_y == 0 && _target.m_san_b == 0)
                                {

                                    _unit.show_attack_num(_target.m_type, _value, "");

                                    if (_target.m_yl > 0)
                                    {

                                        if (m_skill.m_index == 0)
                                        {

                                            _unit.show_attack_num(3, _target.m_yl_value, game_data._instance.get_t_language ("yili_word"));
                                        }
                                    }
                                }

                                List<string> ss = new List<string>();
                                if (_target.m_mian_y == 1)
                                {

                                    if (m_skill.m_index == 0)
                                    {

                                        ss.Add("wlmy_word");
                                    }
                                }
                                else if (_target.m_mian_y == 2)
                                {

                                    if (m_skill.m_index == 0)
                                    {

                                        ss.Add("mfmy_word");
                                    }
                                }
                                else if (_target.m_san_b > 0)
                                {

                                    if (m_skill.m_index == 0)
                                    {

                                        ss.Add("shanbi_zdword");
                                    }
                                }
                                else
                                {
                                    if (_target.m_bao_j > 0)
                                    {

                                        if (m_skill.m_index == 0)
                                        {

                                            ss.Add("bj_zdword");
                                        }
                                    }
                                    if (_target.m_shuang_b > 0)
                                    {

                                        if (m_skill.m_index == 0)
                                        {

                                            ss.Add("shubei_word");
                                        }
                                    }
                                    if (_target.m_ge_d > 0)
                                    {

                                        if (m_skill.m_index == 0)
                                        {

                                            ss.Add("gd_zdword");
                                        }
                                    }
                                    if (_target.m_wshj > 0)
                                    {

                                        if (m_skill.m_index == 0)
                                        {

                                            ss.Add("pojia_word");
                                        }
                                    }
                                }

                                if (ss.Count > 0)
                                {

                                    sys._instance.show_skill_lable(_unit.get_accept_pos(), ss);
                                }
                            }
						}

					}

					if(m_skill.m_index == 0)
					{

						if(m_skill.m_xx > 0)
						{

                        show_attack_num(3, m_skill.m_xx_value,game_data._instance.get_t_language ("xixie_word"));//吸血
						}
					}

					m_skill.m_index ++;
				}

				if(_action.m_type == "skill_end")
				{

					if(m_action_set.m_step == 1)
					{

						if(step_target())
						{

							return;
						}
						else
						{
							m_battle_end_delay = 0.0f;
						}
					}

					if(m_skill != null)
					{

						for(int c = 0;c < m_skill.m_buffer_target_exs.Count;c ++)
						{

							GameObject _buffer_target = battle._instance.get_unit_site(m_skill.m_buffer_target_exs[c].m_site);
							_buffer_target.GetComponent<unit>().buffer(m_skill.m_buffer_target_exs[c].m_id, m_skill.m_buffer_target_exs[c].m_col,true);
						}

						for(int c = 0;c < m_skill.m_buffer_ends.Count;c ++)
						{

							GameObject _buffer_target = battle._instance.get_unit_site(m_skill.m_buffer_ends[c].m_site);
							_buffer_target.GetComponent<unit>().buffer(m_skill.m_buffer_ends[c].m_id, m_skill.m_buffer_ends[c].m_col,false);
						}

						if(m_skill.m_ft > 0)
						{

                            show_attack_num(m_skill.m_ft, m_skill.m_ft_value,  game_data._instance.get_t_language ("fantan_word"), false);//反弹
						}

						m_cur_hp = m_skill.m_hp;

						if(m_skill.m_mp_ch != 0)
						{

							if(m_skill.m_mp_ch == 1)
							{

								show_att_modify_effect("nl_word",1);
								sys._instance.play_sound("sound/attr_up");
							}
							else
							{
								show_att_modify_effect("nl_word_01",-1);
								sys._instance.play_sound("sound/attr_down");
							}
						}

						for(int i = 0;i < m_skill.m_skill_target_exs.Count;i ++)
						{

							skill_target_ex _target = m_skill.m_skill_target_exs[i];
							
							if(_target == null)
							{

								continue;
							}
							
							GameObject _object = battle._instance.get_unit_site(_target.m_site);
							
							if(_object == null)
							{

								continue;
							}
                            unit _unit = _object.GetComponent<unit>();

                            if (_target.m_type == 26)
                            {

                                _unit.m_max_hd = _target.m_yl_value;
                                _unit.m_mini_hp_pro.GetComponent<mini_hp_pro>().set_hudun(_target.m_yl,_target.m_yl_value);
 
                            }
                            else
                            {
                                _unit.m_cur_hp = _target.m_hp;
                                _unit.m_cur_hd = _unit.m_max_hd - _target.m_hd;

                            }
						}

						if (m_skill.do_zz > 0)
						{

							GameObject _object = battle._instance.get_unit_site(m_skill.do_zz_site);
							
							if(_object == null)
							{

								continue;
							}
							
							unit _unit = _object.GetComponent<unit>();
							if (_unit != null)
							{

								if (m_skill.do_zz == 1)
								{

									_unit.add_addeffect("wdhd", "effect/cj_a02");
								}
								else
								{
									_unit.remove_addeffect("wdhd");
								}
							}
						}

						for (int i = 0; i < m_skill.m_message.Count; ++i)
						{

							battle_logic_ex._instance.m_buttle_message.Add(m_skill.m_message[i]);
							m_skill.m_message.Clear();
						}

						for(int i = 0;i < battle._instance.m_units.Count; ++i)
						{

							unit ut = battle._instance.m_units[i].GetComponent<unit>();
							if (ut != null && ut.m_site != m_site)
							{

								head_look hl = ut.GetComponent<head_look>();
								if (hl != null)
								{

									hl.look_empty();
								}
							}
						}
					}
					else
					{
						Debug.Log(game_data._instance.get_t_language ("unit.cs_2336_16"));//无m_skill
					}
				}

				if(_action.m_type == "sound" && sys._instance != null)
				{

					if(_action.m_floats.Count > 0)
					{

						sys._instance.play_sound_loop((string)_action.m_string[0],(float)_action.m_floats[0]);
					}
					else
					{
						if(m_config == "mm")
						{

							sys._instance.play_sound_ex((string)_action.m_string[0]);
						}
						else
						{
							sys._instance.play_sound((string)_action.m_string[0]);
						}
					}
				}

				if(_action.m_type == "game_speed")
				{

					sys._instance.m_game_speed = (float)_action.m_floats[0];
				}


				if(_action.m_type == "show_effect_object")
				{

					show_effect_object((string)_action.m_string[0],true);
				}

				if(_action.m_type == "show_effect_object_trail")
				{

					GameObject _obj = get_effect_object((string)_action.m_string[0]);

					if(_obj != null)
					{

						_obj.GetComponent<Hello_MeleeWeaponTrail>().Emit = true;    
					}  
				}

				if(_action.m_type == "hide_effect_object_trail")
				{

					GameObject _obj = get_effect_object((string)_action.m_string[0]);

					if(_obj != null)
					{

						_obj.GetComponent<Hello_MeleeWeaponTrail>().Emit = false;    
					}
				}

				if(_action.m_type == "hide_effect_object")
				{

					show_effect_object((string)_action.m_string[0],false);
				}

				if(_action.m_type == "copy_effect_object_ex")
				{

					
					GameObject _res =  get_effect_object((string)_action.m_string[0]);
					GameObject _ins = (GameObject)Object.Instantiate(_res);
					
					_ins.transform.position = _res.transform.position;
					_ins.transform.localEulerAngles = _res.transform.eulerAngles;
					_ins.transform.localScale = new Vector3(m_scale,m_scale,m_scale);
					_ins.SetActive(true);
					
					Object.Destroy(_ins,(float)_action.m_floats[0]);
					
				}

				for(int c = 0;c < m_target_unit.Count;c ++)
				{

					if(m_target_unit[c] == null) 
					{

						continue;
					}

					if(_action.m_type == "skill_export")
					{

						m_target_unit[c].GetComponent<unit>().m_while = 1.0f;
						m_target_unit[c].GetComponent<unit>().m_dance = 0.2f;
					}

					if(_action.m_type == "target_action")
					{

						if(m_target_unit[c] != this.transform && m_target_unit[c].GetComponent<unit>().m_battle_end_delay <= 0.0f)
						{	
							m_target_unit[c].GetComponent<unit>().action((string)_action.m_string[0]);
						}
						else if(m_skill == null)
						{	
							m_target_unit[c].GetComponent<unit>().action((string)_action.m_string[0]);
						}
					}

					if(_action.m_type == "target_entity")
					{

						GameObject _ins = game_data._instance.ins_object_res((string)_action.m_string[0]);
						_ins.transform.position = m_target_unit[c].transform.position;
						_ins.transform.localEulerAngles = m_target_unit[c].transform.localEulerAngles;
						_ins.transform.localScale = new Vector3(m_scale,m_scale,m_scale);

						Object.Destroy(_ins,(float)_action.m_floats[0]);
					}

					if(_action.m_type == "copy_effect_object")
					{


						GameObject _res =  get_effect_object((string)_action.m_string[0]);
						GameObject _ins = (GameObject)Object.Instantiate(_res);
						
						_ins.transform.position = _res.transform.position;
						_ins.transform.localEulerAngles = _res.transform.eulerAngles;
						_ins.transform.localScale = new Vector3(m_scale,m_scale,m_scale);
						_ins.SetActive(true);

						Object.Destroy(_ins,(float)_action.m_floats[0]);

					}


					if(_action.m_type == "target_entity_bone")
					{

						GameObject _ins = game_data._instance.ins_object_res((string)_action.m_string[0]);
						if(_ins != null)
						{

							Transform _bone = m_target_unit[c].GetComponent<unit>().get_bone((string)_action.m_string[1]);

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
									_ins.transform.eulerAngles = this.transform.eulerAngles;
								}
							}

							_ins.transform.localScale = new Vector3(m_scale,m_scale,m_scale);
							Object.Destroy(_ins,(float)_action.m_floats[0]);
						}
					}

					if(_action.m_type == "target_copy_effect_object")
					{

						GameObject _res =  get_effect_object((string)_action.m_string[0]);
						
						if(_res != null)
						{

							GameObject _ins = (GameObject)Object.Instantiate(_res);
							
							//_ins.transform.localEulerAngles = transform.transform.localEulerAngles;
							Transform _bone = m_target_unit[c].GetComponent<unit>().get_bone((string)_action.m_string[1]);
							
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
									_ins.transform.eulerAngles = _res.transform.eulerAngles;
								}
							}
							_ins.SetActive(true);
							Object.Destroy(_ins,(float)_action.m_floats[0]);
						}
					}

					if(_action.m_type == "entity_link")
					{

						GameObject _ins = game_data._instance.ins_object_res((string)_action.m_string[0]);
						Transform _bone = get_bone((string)_action.m_string[1]);
						
						if(_bone)
						{

							_ins.transform.localPosition = new Vector3(0,0,0);
							_ins.transform.localRotation = _bone.localRotation;
							_ins.transform.position = _bone.position * m_scale;
							_ins.AddComponent<effect_link>().m_src = _bone.gameObject;
							_ins.GetComponent<effect_link>().m_target = m_target_unit[c].GetComponent<unit>().m_accept;
							Object.Destroy(_ins,(float)_action.m_floats[0]);
						}
					}
					
					if(_action.m_type == "entity_cast")
					{

						GameObject _ins = game_data._instance.ins_object_res((string)_action.m_string[0]);
						Transform _bone = get_bone((string)_action.m_string[1]);
						if(_bone)
						{

							_ins.transform.localPosition = new Vector3(0,0,0);
							_ins.transform.localRotation = _bone.localRotation;
							_ins.transform.position = _bone.position * m_scale;
							_ins.AddComponent<effect_cast>().m_effect = (string)_action.m_string[2];
							_ins.GetComponent<effect_cast>().m_target_unit = m_target_unit[c].gameObject;
							_ins.GetComponent<effect_cast>().m_target_pos = m_target_unit[c].GetComponent<unit>().m_accept;
							_ins.GetComponent<effect_cast>().m_speed = (float)_action.m_floats[0];
							_ins.GetComponent<effect_cast>().m_remove_time = (float)_action.m_floats[1]; 
							_ins.GetComponent<effect_cast>().m_remove_time2 = (float)_action.m_floats[2];
						}

					}
				}

				if(_action.m_type == "shake_cam" )
				{

					sys._instance.shake_cam((float)_action.m_floats[0]);
				}

				if(_action.m_type == "shake_ui" )
				{

					s_message _msg = new s_message();
					
					_msg.m_type = "shake_ui";
					_msg.m_floats.Add((float)_action.m_floats[0]);
					
					cmessage_center._instance.add_message(_msg);
				}

				if(_action.m_type == "entity")
				{

					GameObject _ins = game_data._instance.ins_object_res((string)_action.m_string[0]);
					_ins.transform.position = transform.transform.position;
					_ins.transform.localEulerAngles = transform.transform.localEulerAngles;
					_ins.transform.localScale = new Vector3(m_scale,m_scale,m_scale);

					Object.Destroy(_ins,(float)_action.m_floats[0]);
					//Debug.Log(game_data._instance.get_t_language ("unit.cs_2581_17") + _action.m_floats[0]);//销毁时间
				}

				if(_action.m_type == "entity_bone")
				{

					GameObject _ins = game_data._instance.ins_object_res((string)_action.m_string[0]);
					Transform _bone = get_bone((string)_action.m_string[1]);

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

					_ins.transform.localScale = new Vector3(m_scale,m_scale,m_scale);

					Object.Destroy(_ins,(float)_action.m_floats[0]);
				}

			}
			else
			{
				return;
			}
		}

		if(m_action_pos >= m_action.Count)
		{

			m_action = null;
		}
	}

	double unit_dis(Vector3 pos_0,Vector3 pos_1)
	{

		Vector3 _pos = pos_0 - pos_1;

		return _pos.magnitude;
	}
	void pos_warp(float x,float y)
	{

		Vector3 _pos = sys._instance.terrain_raycast(new Vector3(x,0,y),"terrain");
		this.transform.GetComponent<NavMeshAgent>().Warp(_pos);
	}
	public void destination(float x,float y)
	{

		Vector3 _pos = sys._instance.terrain_raycast(new Vector3(x,0,y),"terrain");
		this.transform.GetComponent<NavMeshAgent>().SetDestination(_pos);
	}

	public void release_skill()
	{

		m_release_skill = true;
	}

	void remove_effect(string name)
	{

		Transform _obj = this.transform.Find(name);

		if(_obj != null)
		{

			GameObject.Destroy(_obj.gameObject);
		}
	}

	void update_name_show()
	{

		if(m_show_name)
		{

			if(m_show_name.activeSelf == false)
			{

				m_show_name.SetActive(true);
			}

			Vector3 _pos = new Vector3(transform.position.x,transform.position.y + m_name_height,transform.position.z);
			m_show_name.transform.localPosition = sys._instance.WorldToScreenPoint(_pos);
		}

		if(m_mini_hp_pro)
		{

			Vector3 _pos = new Vector3(transform.position.x,transform.position.y + m_name_height + m_camp * 0.5f,transform.position.z);
            m_mini_hp_pro.transform.localPosition = sys._instance.WorldToScreenPoint(_pos);
			if(m_alpha > 0.8f)
			{

				m_mini_hp_pro.GetComponent<UIPanel>().alpha = 0.8f;
			}
			else
			{
				m_mini_hp_pro.GetComponent<UIPanel>().alpha = m_alpha;
			}
		}
        if (m_mini_hp_player)
        {

            Vector3 _pos = new Vector3(transform.position.x, transform.position.y + m_name_height * 1.5f + m_camp  * 0.5f, transform.position.z);
            m_mini_hp_player.transform.localPosition = sys._instance.WorldToScreenPoint(_pos);
        }
		if(m_mini_hp_pet)
		{

			Vector3 _pos = new Vector3(transform.position.x,transform.position.y + m_name_height + m_camp * 0.5f,transform.position.z);
			m_mini_hp_pet.transform.localPosition = sys._instance.WorldToScreenPoint(_pos);
			m_mini_hp_pet.GetComponent<UIPanel>().alpha = m_alpha;
		}
	}
	void move()
	{

		if (m_move_cur_time == m_move_end_time)
		{

			if(m_dance > 0)
			{

				m_dance -= Time.deltaTime;

				Vector3 _pos = transform.localPosition;

				_pos.x = m_start_pos.x + Random.Range(- m_dance,m_dance);
				_pos.z = m_start_pos.z + Random.Range(- m_dance,m_dance);

				/*
				if(m_camp == 0)
				{

					_pos.z = m_start_pos.z - m_dance;
				}
				else
				{
					_pos.z = m_start_pos.z + m_dance;
				}
				*/

				transform.localPosition = _pos;
			}
			return;
		}

		if(Mathf.Abs(m_move_cur_time - m_move_end_time) < Time.deltaTime  * m_move_speed)
		{

			m_move_cur_time = m_move_end_time;
		}
		else if(m_move_cur_time > m_move_end_time)
		{

			m_move_cur_time -= Time.deltaTime * m_move_speed;
		}
		else
		{
			m_move_cur_time += Time.deltaTime * m_move_speed;
		}
		Vector3 _start_pos = m_start_pos;
		Vector3 _end_pos = m_target_pos;
		transform.localPosition = Vector3.Lerp (m_start_pos,m_target_pos,m_move_cur_time);

	}

	void battle_ex()
	{

		if(show_pet == 0)
		{

			if(m_site == 100 || m_site == 101)
			{

				m_target_alpha = 0;
			}
			else
			{
				m_target_alpha = 1.0f;
			}
		}
		else if(show_pet == 1)
		{

			if(m_site == 100 || (m_site >= 6 && m_site != 101 && m_site != 100))
			{

				m_target_alpha = 1.0f;
			}
			else
			{
				m_target_alpha = 0;
			}
		}
		else if(show_pet == 2)
		{

			if(m_site == 101 || m_site < 6)
			{

				m_target_alpha = 1.0f;
			}
			else
			{
				m_target_alpha = 0;
			}
		}
		if(show_pet > 0 && (m_site == 100 || m_site == 101) && m_target_alpha >= 1.0f)
		{

			if(m_camp == 0)
			{

				transform.transform.localEulerAngles = new Vector3(0,0,0);
			}
			else
			{
				transform.transform.localEulerAngles = new Vector3(0,180,0);
			}
			transform.localPosition = m_start_pos;
			if(sys._instance != null && gameObject.transform.name != "mm")
			{

				sys._instance.show_effect (transform.position + new Vector3(0,0.1f,0),"effect/cw_start",1.0f);
			}
		}
		if(m_site != 100 && m_site != 101 && m_cur_hp > 0 && !m_fuhuo1)
		{

			alpha_ex();
		}
		else
		{
			alpha(true);
		}
	}

	// Update is called once per frame
	void Update () {

		if(m_cur_hp <= 0 &&  m_die_time > 0)
		{

			m_die_time -= Time.deltaTime;

			if(m_die_time <= 0)
			{

				Destroy(this.gameObject);
			}
		}

		if(m_wait > 0)
		{

			m_wait -= Time.deltaTime;
			return;
		}

		load_xml ();
		if(m_site != 99999999 && battle_start != 0)
		{

			battle_ex();
			show_pet = -1;
		}
		else
		{
			alpha(true);
		}

		if(m_first_wait > 0 && sys._instance.m_pause == false)
		{

			m_first_wait -= Time.deltaTime;
		}

		if(m_first == true && m_first_wait <= 0)
		{

			m_first_wait = 0.0f;
			m_first = false;
			if(m_site == 100 || m_site == 101)
			{

				GameObject _object = sys._instance.show_effect (this.transform.position,"effect/npc/pt_ef_die01",1.0f);
				m_target_alpha = 0.0f;
			}
		}

		white();
		action_update ();
		move ();

		update_name_show ();

		if(m_attack_num_time <= 0)
		{

			if(m_attack_num.Count > 0)
			{

				m_attack_num_time = 0.2f;

				cmessage_center._instance.add_message(m_attack_num[0]);

				m_attack_num.RemoveAt(0);
			}
		}
		else if(m_attack_num_time > 0)
		{

			m_attack_num_time -= Time.deltaTime;
		}


		if(m_fuhuo_wait > 0)
		{

			m_fuhuo_wait -= Time.deltaTime;
		}
		
		if(m_fuhuo == true
		   && m_fuhuo_wait <= 0)
		{
			m_cur_hp = 0;
			m_fuhuo = false;
			m_target_alpha = 0.0f;
			m_fuhuo_wait = 1.0f;

			GameObject _object = sys._instance.show_effect (this.transform.position,"effect/npc/pt_ef_die01",3.0f);
		}

		if(m_die_wait > 0)
		{

			m_die_wait -= Time.deltaTime;
		}

		if((m_die == true) 
		   && m_die_wait <= 0)
		{
			m_cur_hp = 0;
			m_die = false;
			GameObject _object = sys._instance.show_effect (this.transform.position,"effect/npc/pt_ef_die01",3.0f);
			GameObject.Destroy(this.gameObject,1.0f);

			m_target_alpha = 0.0f;
		}

		if(m_battle_end_delay > 0)
		{

			m_battle_end_delay -= Time.deltaTime;

			if(m_battle_end_delay <= 0.0f)
			{

				m_release_skill = false;
			}
		}
	}
}
