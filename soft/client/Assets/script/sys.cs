
using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;

public class sys : MonoBehaviour,IMessage {

	public static sys _instance;
	[System.NonSerialized]
	public string m_game_state;
	public GameObject m_root_unit;
	[System.NonSerialized]
	public GameObject m_map_root;
	[System.NonSerialized]
	public GameObject m_map;
	[System.NonSerialized]
	public string m_load_name;
	[System.NonSerialized]
	public string m_load_text;
	[System.NonSerialized]
	public bool m_is_loading = false;
	[System.NonSerialized]
	public GameObject m_reward_show;
	public GameObject m_buttle_cam;
	[System.NonSerialized]
    public bool m_check_bf = false;
	[System.NonSerialized]
	public bool m_pause = false;
	public AudioSource m_play_mus;
	// Use this for initialization
	[System.NonSerialized]
	public player m_self;
	private string m_mus_name;
	private bool m_stop_mus = false;
	private WWW m_download_www;
	[System.NonSerialized]
	public List<string> m_message_type = new List<string>();
	[System.NonSerialized]
	public List<ulong> m_message_long = new List<ulong>();
	[System.NonSerialized]
	public List<int> m_gongzhengs = new List<int>();
	[System.NonSerialized]
	public List<int> m_houyuans = new List<int>();
	[System.NonSerialized]
	public List<double> m_houyuan_sxs = new List<double>();
	[System.NonSerialized]
	public Dictionary<int,double> m_sxs = new Dictionary<int,double>();
	[System.NonSerialized]
	public ccard  m_card = null;
	[System.NonSerialized]
	public int chenghao_effect = 0;
	[System.NonSerialized]
	public string m_sid = "";
	[System.NonSerialized]
	public string m_sname = "";
	[System.NonSerialized]
	public List<ulong> m_select_equips = new List<ulong>();
	[System.NonSerialized]
	public List<ulong> m_select_treasures = new List<ulong>();
	[System.NonSerialized]
	public List<int> m_select_items = new List<int>();
	[System.NonSerialized]
	public List<int> m_select_indexs = new List<int>();
	[System.NonSerialized]
	public float m_game_speed = 1.0f;
    [System.NonSerialized]
	public string m_hall_name = "ts_game_hall";
	[System.NonSerialized]
	public List<protocol.game.smsg_chat> m_chat_msgs;
	[System.NonSerialized]
	public List<protocol.game.smsg_chat> m_chat_msgs1;
	private AudioSource m_sound_source;
	private bool m_edit_mode = false;
	private List<GameObject> m_scene_exs = new List<GameObject>();
	[System.NonSerialized]
	public GameObject m_hall_root;
	[System.NonSerialized]
	public bool m_is_director = false;
	[System.NonSerialized]
	public bool m_need_check = false;
	[System.NonSerialized]
	public bool m_xq_check = false;
    public int m_state_flag = 0;

	public float get_game_speed()
	{
		if(game_data._instance.m_player_data.m_speed == 2)
		{
			return 1.5f;
			//return 10.0f;
		}
		else if(game_data._instance.m_player_data.m_speed == 3)
		{
			return 2.0f;
		}

		return 1.0f;
	}

    public Texture2D get_image_half(string name)
    {
        Texture2D _texture = game_data._instance.get_object_res("ui/half/" + name, typeof(Texture2D)) as Texture2D;

        return _texture;
    }

    void Awake()
	{
		_instance = this;
		Debug.Log ("game_main");

        platform_config_common.init();
        platform_config.init();

        if (platform_config_common.m_resolution == 1)
        {
            float _scale = 640f / (float)Screen.height;
            float _width = (float)Screen.width * _scale;
            float _height = (float)Screen.height * _scale;
            Screen.SetResolution((int)_width, (int)_height, true);
        }

        Application.targetFrameRate = 30;

		m_self = new player ();
		m_self.m_reserve = 3;
		protocall.key1 = Random.Range (10000, 65536);
		protocall.key2 = Random.Range (10000, 65536);

        if (Application.isEditor && platform_config_common.m_gonggao == null)
		{
            this.gameObject.AddComponent<platform_object>();
            this.gameObject.AddComponent<platform_recharge_object>();
            this.gameObject.AddComponent<cmessage_center>();
            this.gameObject.AddComponent<icon_manager>();
            this.gameObject.AddComponent<game_data>();
            game_data._instance.load ();
			game_data._instance.load_txt();
            platform_config_common.m_gonggao = "";
		}
	}
	
	public void clear_select_equips()
	{
		m_select_equips.Clear ();
	}
	
	public bool is_select_equip(ulong guid)
	{
		for(int i = 0;i < m_select_equips.Count;i ++)
		{
			if(m_select_equips[i] == guid)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void select_equips(ulong guid)
	{
		for(int i = 0;i < m_select_equips.Count;i ++)
		{
			if(m_select_equips[i] == guid)
			{
				m_select_equips.RemoveAt(i);
				return;
			}
		}
		
		m_select_equips.Add (guid);
	}

	public void clear_select_treasures(int count)
	{
		m_select_treasures.Clear ();
		for(int i = 0; i < count;++i )
		{
			m_select_treasures.Add(0);
		}
	}
	
	public bool is_select_treasure(ulong guid)
	{
		for(int i = 0;i < m_select_treasures.Count;i ++)
		{
			if(m_select_treasures[i] == guid)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public void select_treasures(ulong guid)
	{
		for(int i = 0;i < m_select_treasures.Count;i ++)
		{
			if(m_select_treasures[i] == guid)
			{
				m_select_treasures[i] = 0;
				return;
			}
		}
		
		for(int i = 0;i < m_select_treasures.Count;i ++)
		{
			if(m_select_treasures[i] == 0)
			{
				m_select_treasures[i] = guid;
				break;
			}
		}
	}

	public void clear_select_items(int count)
	{
		m_select_items.Clear ();
		for(int i = 0; i < count;++i )
		{
			m_select_items.Add(0);
		}
		m_select_indexs.Clear ();
	}
	
	public void select_items(int id,bool flag = false)
	{
		s_t_item t_item = game_data._instance.get_item (id);
		for(int i = 0;i < m_select_items.Count;i ++)
		{
			if(m_select_items[i] == 0 && !flag)
			{
				m_select_items[i] = id;
				break;
			}
			if(m_select_items[i] == id && flag)
			{
				m_select_items[i] = 0;
				break;
			}
		}
	}

	public bool is_select_item(int index)
	{
		for(int i = 0;i < m_select_indexs.Count;i ++)
		{
			if(m_select_indexs[i] == index)
			{
				return true;
			}
		}
		
		return false;
	}

	public void select_item_indexs(int id)
	{
		for(int i = 0;i < m_select_indexs.Count;i ++)
		{
			if(m_select_indexs[i] == id)
			{
				m_select_indexs.RemoveAt(i);
				return;
			}
		}
		m_select_indexs.Add(id);
	}

	public void restart_cfg()
	{
		if(game_data._instance.m_player_data.m_music == 0)
		{
			m_play_mus.enabled = false;
		}
		else
		{
			m_play_mus.enabled = true;
		}

	}

	void Start () {

		if(this.transform.name != "game_main")
		{
			m_edit_mode = true;
			return;
		}

		get_cam ().backgroundColor = Color.black;

		m_pause = false;

		cmessage_center._instance.add_handle (this);

        DontDestroyOnLoad(this.gameObject);

		restart_cfg ();

		s_message _msg = new s_message();
		_msg.time = 0.5f;
		_msg.m_type = "login_game";
		cmessage_center._instance.add_message(_msg);

		m_reward_show = game_data._instance.ins_object_res("ui/reward_show");
		m_reward_show.transform.parent = this.transform;
		m_reward_show.transform.localPosition = new Vector3(0, 0, 0);
		m_reward_show.transform.localScale = new Vector3(1, 1, 1);

		m_map_root = game_data._instance.ins_object_res("ui/map_root");
		m_map_root.transform.parent = this.transform;
		m_map_root.transform.localPosition = new Vector3(0, 0, 0);
		m_map_root.transform.localScale = new Vector3(1, 1, 1);
		m_map = m_map_root.transform.Find("map").gameObject;
	}
	
	public void add_scene_exs(GameObject obj)
	{
		m_scene_exs.Add (obj);
	}

	public void remove_all_scene_ex()
	{
		for(int i = 0;i < m_scene_exs.Count;i ++)
		{
			GameObject _obj = m_scene_exs[i];
			GameObject.Destroy(_obj);
		}

		m_scene_exs.Clear ();
	}

	public void load_scene_ex(string name)
	{
		if(m_scene_exs.Count > 3)
		{
			for(int i = 0;i < m_scene_exs.Count;i ++)
			{
				GameObject _obj = m_scene_exs[i];
				
				if(_obj.transform.name != m_hall_name)
				{
					GameObject.Destroy(_obj);
					m_scene_exs.RemoveAt(i);
					Resources.UnloadUnusedAssets ();
					break;
				}
			}
		}

		load_scene (name, true);
	}

	public void load_scene(string name, bool add = false)
	{
		if(m_is_loading == true)
		{
			return;
		}

		add = false;

		string _music = "music/" + game_data._instance.get_scene_music(name);
		play_mus(_music);

		remove_all_scene_ex ();

		bool _load = true;

		Resources.UnloadUnusedAssets ();
		System.GC.Collect();

		for(int i = 0;i < m_scene_exs.Count;i ++)
		{
			if(m_scene_exs[i].transform.name == name)
			{
				_load = false;

				root_gui._instance.show_loading_gui_ex();

				s_message _msg = new s_message();

				_msg.m_type = "load_scene_show_ex";
				_msg.m_object.Add(m_scene_exs[i]);

				cmessage_center._instance.add_message(_msg);
			}
			else
			{
				m_scene_exs[i].SetActive(false);
			}
		}

		if(_load == true)
		{
			m_is_loading = true;
			root_gui._instance.show_loading_gui();
			root_gui._instance.m_loading.GetComponent<loading>().m_add = add;
			m_load_name = name;
			dbc dbc_tip = game_data._instance.m_dbc_tip;
			int index = Random.Range(0, dbc_tip.get_y ());
			m_load_text = game_data._instance.get_t_language(dbc_tip.get (0, index));
			root_gui._instance.m_loading.SetActive (true);
		}
		else
		{
			m_load_name = name;

			s_message _msg = new s_message();
			_msg.m_type = "loaded";
			cmessage_center._instance.add_message(_msg);

			this.Invoke("load_end_ex",0.3f);
		}
	}

	public void load_end_ex()
	{
		root_gui._instance.hide_loading_gui_ex();
	}

	public void load_end(float time = 0.5f)
	{
		s_message _msg = new s_message();
		
		_msg.time = time;
		_msg.m_type = "loaded";
		cmessage_center._instance.add_message(_msg);
		
		m_is_loading = false;
		root_gui._instance.m_mask.SetActive(true);
		root_gui._instance.m_mask.GetComponent<UIPanel>().alpha = 1.0f;
	}

	public Camera get_cam()
	{
		Camera _cam = null;
		
		if(m_buttle_cam.activeSelf)
		{
			_cam = m_buttle_cam.transform.GetChild(0).GetComponent<Camera>();
		}


		if(_cam == null)
		{
			_cam = Camera.main;
		}

		return _cam;
	}

	public Vector3 WorldToScreenPoint(Vector3 position)
	{
		Camera _cam = get_cam();

		if(_cam == null)
		{
			return position;
		}

		Vector3 _pos = _cam.WorldToScreenPoint(position);
		float _height = _cam.pixelHeight;
		//if(_height < 640)
		{
			_height /= root_gui._instance.m_ui_cam.transform.parent.GetComponent<UIRoot>().activeHeight;
			_pos.x /= _height;
			_pos.y /= _height;
		}
		_pos.z = 0;
		return _pos;
	}

	public Vector3 terrain_raycast(Vector3 position,string name)
	{
		RaycastHit[] _hit = Physics.RaycastAll (new Vector3 (position.x, 1000, position.z), new Vector3 (0, -1, 0), 1200);

		float _height = 0.0f;

		for(int i = 0;i < _hit.Length;i ++)
		{
			if(_hit[i].transform.gameObject.tag == name && _hit[i].point.y > _height)
			{
				_height = _hit[i].point.y;
			}
		}
		return new Vector3 (position.x, _height, position.z);
	}

	public bool get_mouse_button(int id)
	{
		if(Input.touchCount == 1)
		{
			return true;
		}
		else 
		{
			return Input.GetMouseButton (0);
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

		float _height = root_gui._instance.m_ui_cam.transform.parent.GetComponent<UIRoot>().activeHeight;

		pos.x = pos.x * (_height / (float)Screen.height);
		pos.y = pos.y * (_height / (float)Screen.height);

		return pos;
	}

	public void remove_child(GameObject obj)
	{
		if(obj == null)
		{
			return;
		}

		List<GameObject> objs = new List<GameObject>();
		for(int i = 0;i < obj.transform.childCount;i ++)
		{
			objs.Add(obj.transform.GetChild(i).gameObject);
		}
		for(int i = 0;i < objs.Count;i ++)
		{
			Object.Destroy(objs[i]);
		}
		objs.Clear ();
	}

	public void remove_child(GameObject obj, string name)
	{
		List<GameObject> objs = new List<GameObject>();
		for(int i = 0;i < obj.transform.childCount;i ++)
		{
			if(obj.transform.GetChild(i).name == name)
			{
				objs.Add(obj.transform.GetChild(i).gameObject);
			}
		}
		for(int i = 0;i < objs.Count;i ++)
		{
			Object.Destroy(objs[i]);
		}
		objs.Clear ();
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_FIRST_RECHARGE)
		{
			protocol.game.smsg_first_recharge _msg = net_http._instance.parse_packet<protocol.game.smsg_first_recharge> (message.m_byte);
			for(int i =0 ;i< _msg.treasures.Count;i++)
			{
				m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				m_self.add_equip(_msg.equips[i]);
			}
			
			for (int i = 0; i < _msg.types.Count; ++i)
			{
				m_self.add_reward( _msg.types[i], _msg.value1s[i],_msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("sys.cs_620_101"));//首充获得
			}
			
			for(int i = 0;i < _msg.roles.Count;i ++)
			{
				m_self.add_card(_msg.roles[i], true);
			}
			
			m_self.m_t_player.first_reward = 2;
            s_message _mes = new s_message();
            _mes.m_type = "hide_first_recharge";
            cmessage_center._instance.add_message(_mes);
            s_message _message = new s_message();
            _message.m_type = "show_main_gui";
            cmessage_center._instance.add_message(_message);
		}
        if (message.m_opcode == opclient_t.SMSG_CHAT && chat_gui._instance == null)
        {
            protocol.game.smsg_chat _msg = net_http._instance.parse_packet<protocol.game.smsg_chat>(message.m_byte);
            if (_msg.type == 1)
            {
                m_chat_msgs1.Add(_msg);
            }
            else
            {
                m_chat_msgs.Add(_msg);
                s_message _mes = new s_message();
                _mes.m_type = "fresh_chat";
                cmessage_center._instance.add_message(_mes);
            }
           
        }
		if (message.m_opcode == opclient_t.CMSG_GM_COMMAND)
		{
			protocol.game.smsg_gm_command _msg = net_http._instance.parse_packet<protocol.game.smsg_gm_command> (message.m_byte);
			if (_msg.type == 0)
			{
				if (_msg.value1 == 1)
				{
					m_self.m_t_player.random_event_num = 0;
					m_self.m_t_player.random_event_time = 0;

					s_message _smsg = new s_message();
					_smsg.m_type = "deal_main_gui";
					cmessage_center._instance.add_message(_smsg);
				}
                else if (_msg.value1 == 2)
                {
                    m_self.refresh();
                }
                if (_msg.value1 == 6)
                {
                    if (juntuan_gui._instance != null)
                    {
                        juntuan_gui._instance.m_guild_t.honor += _msg.value2; 
                    }
                }
			}
           
			else
			{
				m_self.add_reward(_msg.type, _msg.value1, _msg.value2, _msg.value3,game_data._instance.get_t_language ("sys.cs_682_85"));//gm命令获得
				for (int i = 0; i < _msg.roles.Count; ++i)
				{
					m_self.add_card(_msg.roles[i]);
				}
				for (int i = 0; i < _msg.pets.Count; ++i)
				{
					m_self.add_pet(_msg.pets[i]);
				}
				for (int i = 0; i < _msg.equips.Count; ++i)
				{
					m_self.add_equip(_msg.equips[i]);
				}
				for (int i = 0; i < _msg.treasures.Count; ++i)
				{
					m_self.add_treasure(_msg.treasures[i]);
				}
			}
		}
        if (message.m_opcode == opclient_t.CMSG_QIYU_CHECK)
        {
            protocol.game.smsg_qiyu_check msg = net_http._instance.parse_packet<protocol.game.smsg_qiyu_check>(message.m_byte);
            m_self.m_t_player.qiyu_hard.Clear();
            m_self.m_t_player.qiyu_mission.Clear();

            m_self.m_t_player.qiyu_suc.Clear();

            for (int i = 0; i < msg.qiyu_mission.Count; i++)
            {
                m_self.m_t_player.qiyu_hard.Add(msg.qiyu_hard[i]);
                m_self.m_t_player.qiyu_mission.Add(msg.qiyu_mission[i]);
                m_self.m_t_player.qiyu_suc.Add(msg.qiyu_suc[i]);
            }
        }
        else if (message.m_opcode == opclient_t.CMSG_SHOP_CHECK)
        {
           
            {
                protocol.game.smsg_shop_refresh _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_refresh>(message.m_byte);
                m_self.m_t_player.shop4_ids.Clear();
                m_self.m_t_player.shop4_sell.Clear();

                for (int i = 0; i < _msg.shop4_ids.Count; i++)
                {
                    m_self.m_t_player.shop4_ids.Add(_msg.shop4_ids[i]);
                    m_self.m_t_player.shop4_sell.Add(_msg.shop4_sell[i]);

                }
                m_self.m_t_player.huiyi_shop_last_time = timer.now();
                s_message mes = new s_message();
                mes.m_type = "refresh_huiyi_shop";
                cmessage_center._instance.add_message(mes);

            }

        }
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "re_login")
		{
			game_logout();
		}
		else if(message.m_type == "load_scene_show_ex")
		{
			GameObject _object = (GameObject)message.m_object[0];

			_object.SetActive(true);
		}
		else if(message.m_type == "login_game")
		{
			m_game_state = "login";
            if (platform_config_common.m_half > 0)
            {
                load_scene("ts_game_start");
            }
            else
            {
                load_scene("ts_game_start2");
            }

			
		}
		else if(message.m_type == "load_scene")
		{
			load_scene_ex((string)message.m_string[0]);
			cmessage_center._instance.add_message(message.m_next);
		}
		else if(message.m_type == "load_scene2")
		{
			load_scene((string)message.m_string[0]);
			cmessage_center._instance.add_message(message.m_next);
		}
        
	}

	public string get_res_info(int type, int value1, int value2, int value3, int kh = 0)
	{
		string _info = "";
		string s = "";
        if (type == 1)
        {

            s = game_data._instance.get_t_resource(value1).name;
            _info = get_res_color(value1) + s;
            if (value2 >= 1)
            {
                _info = _info + " x " + value2 + "[-]";
            }
            if (kh == 1)
            {
                _info = get_res_color(value1) + "[" + s + " x " + value2 + "][-]";
            }
        }
        else if (type == 2)
        {
            s_t_item _item = game_data._instance.get_item(value1);
            _info = _item.name;
            if (value2 >= 1)
            {
                _info = _info + " x " + value2;
            }
            if (kh == 1)
            {
                _info = "[" + _info + "]";
            }
            _info = ccard.get_color_name(_info, _item.font_color);

        }
        else if (type == 3)
        {
            s_t_class _class = game_data._instance.get_t_class(value1);
            _info = _class.name;
            if (kh == 1)
            {
                _info = "[" + _info + "]";
            }
            _info = ccard.get_color_name(_class.name, _class.color);
        }
        else if (type == 4)
        {
            s_t_equip _equip = game_data._instance.get_t_equip(value1);
            _info = _equip.name;
            if (kh == 1)
            {
                _info = "[" + _info + "]";
            }
            _info = equip.get_equip_name(_equip.id, 0);
        }
        else if (type == 6)
        {
            s_t_baowu _treasure = game_data._instance.get_t_baowu(value1);
            _info = _treasure.name;
            if (kh == 1)
            {
                _info = "[" + _info + "]";
            }
            _info = treasure.get_treasure_name(_treasure.id, 0);
        }
        else if (type == 7)
        {
            s_t_guanghuan _guanghuan = game_data._instance.get_t_guanghuan(value1);
            _info = game_data._instance.get_name_color(_guanghuan.color)  + _guanghuan.name;

        }
		return _info;
	}

	public string get_res_samall_icon(int id)
	{
        if (id == 101 || id == 102)
        {
            return "nmwjz";
        }
        if (id == 105)
        {
            return game_data._instance.get_t_resource(10).smallicon;
        }
        s_t_resource res = game_data._instance.get_t_resource(id);
        if (res == null)
        {
            return "";
        }
        return res.smallicon;
	}

	public string get_res_color(int id)
	{
		s_t_resource t_resource = game_data._instance.get_t_resource (id);
		if(t_resource != null)
		{
			return t_resource.namecolor;
		}
		return "";
	}
	
    public string get_res_color_name(int id)
    {
        s_t_resource res = null;
        if (id == 1)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_886_35");//获得金币[-]
        }
        else if (id == 2)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_891_35");//获得钻石[-]
        }
        else if (id == 3)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_896_35");//获得体力[-]
        }
        else if (id == 4)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_901_35");//获得经验[-]
        }
        else if (id == 5)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_906_35");//获得战魂[-]
        }
        else if (id == 6)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_911_35");//获得合金[-]
        }
        else if (id == 7)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_916_35");//获得原力[-]
        }
        else if (id == 10)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_921_35");//获得贡献[-]
        }
        else if (id == 13)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_926_35");//获得勋章[-]
        }
        else if (id == 14)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_931_35");//获得荣誉[-]
        }
        else if (id == 19)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_936_35");//获得图纸[-]
        }
        else if (id == 22)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_941_35");//获得奖章[-]
        }
        else if (id == 23)
        {
            return game_data._instance.get_t_language ("sys.cs_945_19");//[ffb391]获得积分[-]
        }
        else if (id == 24)
        {
            res = game_data._instance.get_t_resource(id);
            return res.namecolor + game_data._instance.get_t_language ("sys.cs_950_35");//获得冰晶[-]
        }
        else if (id == 25)
        {
            return game_data._instance.get_t_language ("sys.cs_945_19");//[ffb391]获得积分[-]
        }
        else if (id == 27)
        {
            return game_data._instance.get_t_language ("sys.cs_958_19");//[ffb391]获得芯片[-]
        }
        else if (id == 100)
        {
            return game_data._instance.get_t_language ("sys.cs_962_19");//[ffb391]造成伤害[-]
        }
        else if (id == 101)
        {
            return game_data._instance.get_t_language ("sys.cs_966_19");//[ffb391]最后一击[-]
        }
        else if (id == 102)
        {
            return game_data._instance.get_t_language ("sys.cs_966_19");//[ffb391]最后一击[-]
        }
        else if (id == 105)
        {
            return game_data._instance.get_t_language ("sys.cs_966_19");//[ffb391]最后一击[-]
        }
        else if (id == 103)
        {
            return game_data._instance.get_t_language ("sys.cs_978_19");//[ffb391]累积伤害排名[-]
        }
        else if (id == 104)
        {
            return game_data._instance.get_t_language ("sys.cs_982_19");//[ffb391]最高伤害排名[-]
        }
        else if (id == 106)
        {
            return game_data._instance.get_t_language ("sys.cs_986_19");//[ffb391]当前积分[-]
        }
        else if (id == 107)
        {
            return game_data._instance.get_t_language ("sys.cs_990_19");//[ffb391]获得战绩[-]
        }
        else if (id == 108)
        {
            return game_data._instance.get_t_language ("sys.cs_994_19");//[ffb391]减少城防值[-]
        }
        else if (id == 200)
        {
            return game_data._instance.get_t_language(game_data._instance.get_t_language ("sys.cs_990_19"));//[ffb391]奖励次数[-]
        }
        return "";
    }

	public void play_sound_ex(string name)
	{
		if(name.Length == 0 || game_data._instance.m_player_data.m_sound == 0)	{
			return ;
		}

		AudioClip _clip = game_data._instance.get_object_res(name, typeof(AudioClip)) as AudioClip;
		
		if(_clip == null)
		{
			return;
		}

		if (m_sound_source == null)
		{		
			m_sound_source = transform.gameObject.AddComponent<AudioSource>();
		}
		
		m_sound_source.clip = _clip;
		m_sound_source.Play();
	}

	public void play_sound_ex(AudioClip clip)
	{
		if(game_data._instance.m_player_data.m_sound == 0)	{
			return ;
		}

		if(clip == null)
		{
			return;
		}
		
		if (m_sound_source == null)
		{		
			m_sound_source = transform.gameObject.AddComponent<AudioSource>();
		}
		
		m_sound_source.clip = clip;
		m_sound_source.Play();
	}

	public void play_sound(string name)
	{
		if(name.Length == 0 
		   || game_data._instance.m_player_data.m_sound == 0 
		   ||  transform.gameObject.GetComponents<AudioSource>().Length > 20)	
		{
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

		Object.Destroy(_source,_clip.length * get_game_speed() + 1.0f);
	}

	public void play_sound_loop(string name,float time = 1f)
	{
		if(name.Length == 0 || game_data._instance.m_player_data.m_sound == 0)	{
			return ;
		}
		
		AudioClip _clip = game_data._instance.get_object_res(name, typeof(AudioClip)) as AudioClip;
		
		if(_clip == null)
		{
			return;
		}
		
		AudioSource _source = transform.gameObject.AddComponent<AudioSource>();
		
		_source.clip = _clip;
		_source.loop = true;
		_source.Play();
		
		Object.Destroy(_source,time);
	}

	public void play_mus(string name,bool loop = true)
	{
		m_mus_name = name;
		m_stop_mus = true;
	}
	
	public void show_skill_lable(Vector3 pos, List<string> ss)
	{
		s_message _new_msg = new s_message();
		
		_new_msg.m_type = "skill_lable";

		_new_msg.time = cmessage_center._instance.get_message_num ("skill_lable") * 0.5f;
		_new_msg.m_object.Add(ss);
		_new_msg.m_floats.Add(pos.x);
		_new_msg.m_floats.Add(pos.y);
		_new_msg.m_floats.Add(pos.z);
		
		cmessage_center._instance.add_message(_new_msg);
	}
    public server_list get_server(int s)
    {
        List<server_list> serverlist = game_data._instance.m_server_list;
        for (int i = 0; i < serverlist.Count; i++)
        {
            if (serverlist[i].m_id == s.ToString())
            {
                return serverlist[i];
            }

        }
        return serverlist[0];
    }
	public GameObject show_effect(Vector3 pos,string type,float life)
	{
		GameObject _res = game_data._instance.get_object_res(type, typeof(GameObject)) as GameObject;
        GameObject _ins = null;
        try
        {
            _ins = (GameObject)Object.Instantiate(_res);

        }
        catch { Debug.Log(type); }
		
		_ins.transform.position = pos;

		GameObject.Destroy (_ins,life);

		return _ins;
	}
	public void shake_cam(float shake)
	{
		s_message _msg = new s_message();
		
		_msg.m_type = "shake_cam";
		_msg.m_floats.Add(shake);
		
		cmessage_center._instance.add_message(_msg);
	}
	
	public GameObject create_class(int id, int dress, int guanghuan, GameObject root = null)
	{
		s_t_class t_class = game_data._instance.get_t_class (id);
		string _base_res = "unit" + "/" + t_class.show + "/" + t_class.show;
		string _dress_res = null;
		GameObject _res = null;
		
		string _show = t_class.show;
		string _dress = t_class.dress;
		s_t_role_dress _t_dress =  game_data._instance.get_t_role_dress (dress);
		if(_t_dress != null)
		{
			_dress = _t_dress.res;
		}
		if(_dress != null && _dress != "" && _dress != "0")
		{
			_dress_res = "unit" + "/" + _dress + "/" + _dress;
			_res = game_data._instance.get_object_res(_dress_res, typeof(GameObject)) as GameObject;
			
			if(_res != null)
			{
				_show =_dress;
			}
		}
		
		if(_res == null)
		{
			_res = game_data._instance.get_object_res(_base_res, typeof(GameObject)) as GameObject;
		}
		
		if (_res == null) {
			Debug.Log ("not find " + t_class.show);
			
			_res = game_data._instance.get_object_res ("unit/ts_t01/ts_t01", typeof(GameObject)) as GameObject;
			_show = "ts_t01";
		}
		
		GameObject _ins = (GameObject)Object.Instantiate (_res);
		
		if (root == null)
		{
			_ins.transform.parent = m_root_unit.transform;
		}
		else
		{
			_ins.transform.parent = root.transform;
			_ins.transform.localPosition = Vector3.zero;
		}

		_ins.transform.GetComponent<unit>().m_t_class = t_class;
		_ins.transform.GetComponent<unit>().m_config = _show;
		
		if(_dress_res != null)
		{
			_ins.transform.GetComponent<unit>().m_default_parts.Add(_dress);
		}

		_ins.GetComponent<unit>().load_xml();

		_ins.transform.localEulerAngles = new Vector3 (0,0,0);
		_ins.transform.localPosition = new Vector3 (0,0,0);
		_ins.transform.localScale = Vector3.one;

		s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan (guanghuan);
		if (t_guanghuan != null && game_data._instance.m_player_data.m_guanghuan == 1)
		{
			_res = game_data._instance.get_object_res ("effect/" + t_guanghuan.effect, typeof(GameObject)) as GameObject;
			GameObject _eff = (GameObject)Object.Instantiate (_res);
			_eff.transform.parent = _ins.transform;
			_eff.transform.localEulerAngles = new Vector3 (0,0,0);
			_eff.transform.localPosition = new Vector3 (0,0,0);
			_eff.transform.localScale = Vector3.one;
		}
		
		return _ins;
	}

	public GameObject create_pet(int id, int big, GameObject root = null)//big 0为大模型 1为小模型
	{
		s_t_pet t_pet = game_data._instance.get_t_pet (id);
		string _base_res = "";
		if(big == 0)
		{
			_base_res = "unit" + "/" + t_pet.show + "/" + t_pet.show;
		}
		else if(big == 1)
		{
			_base_res = "unit" + "/" + t_pet.show + "/" + t_pet.small_show;
		}
		GameObject _res = null;
		
		string _show = t_pet.show;
		
		if(_res == null)
		{
			_res = game_data._instance.get_object_res(_base_res, typeof(GameObject)) as GameObject;
		}
		
		if (_res == null) {
			if(big == 0)
			{
				_res = game_data._instance.get_object_res ("unit/ts_cw004/ts_cw004", typeof(GameObject)) as GameObject;
			}
			else
			{
				_res = game_data._instance.get_object_res ("unit/ts_cw004/ts_cw004a", typeof(GameObject)) as GameObject;
			}
			Debug.Log ("not find " + t_pet.show);
		}
		
		GameObject _ins = (GameObject)Object.Instantiate (_res);
		
		if (root == null)
		{
			_ins.transform.parent = m_root_unit.transform;
		}
		else
		{
			_ins.transform.parent = root.transform;
			_ins.transform.localPosition = Vector3.zero;
		}
		
		_ins.transform.GetComponent<unit>().m_t_pet = t_pet;
		if(big == 0)
		{
			_ins.transform.GetComponent<unit>().m_config = _show;
		}
		else
		{
			_ins.transform.GetComponent<unit>().m_config = _show + "a";
		}
		
		_ins.GetComponent<unit>().load_xml();
		
		_ins.transform.localEulerAngles = new Vector3 (0,0,0);
		_ins.transform.localPosition = new Vector3 (0,0,0);
		_ins.transform.localScale = Vector3.one;
		
		return _ins;
	}

    public GameObject create_mm(GameObject root = null)//big 0为大模型 1为小模型
    {
        GameObject _ins = game_data._instance.ins_object_res("unit/mm/mm");
        if (root == null)
        {
            _ins.transform.parent = m_root_unit.transform;
        }
        else
        {
            _ins.transform.parent = root.transform;
        }
        _ins.transform.name = "mm";
        _ins.transform.localPosition = new Vector3(0, 0, 0);
        _ins.transform.localEulerAngles = new Vector3(0, 0, 0);
        _ins.transform.localScale = Vector3.one;
        _ins.GetComponent<unit>().m_config = "mm";
        _ins.GetComponent<unit>().load_xml();

        for (int i = 0; i < m_self.m_t_player.dress_on_ids.Count; i++)
        {
            s_t_dress _dress = game_data._instance.get_t_dress((int)m_self.m_t_player.dress_on_ids[i]);
            _ins.GetComponent<unit>().change_part(_dress.res, 0);
        }
        return _ins;
    }

        public GameObject create_class(ccard card, int guanghuan, GameObject root = null)
	{
		if (card == null) {
				return null;
		}

		int dress = 0;
		if(card.get_role() != null)
		{
			dress = card.get_role().dress_on_id;
		}
		GameObject _ins = create_class (card.get_template_id(), dress, guanghuan, root);
		_ins.transform.GetComponent<unit>().m_card = card;

		return _ins;
	}

	public GameObject create_pet(pet pet, int big, GameObject root = null)
	{
		if (pet == null) {
			return null;
		}

		GameObject _ins = create_pet (pet.get_template_id(), big, root);
		_ins.transform.GetComponent<unit>().m_pet = pet;
		
		return _ins;
	}

    public TweenVolume add_value_anim(GameObject obj,float speed,float dst)
	{
		return TweenVolume.Begin (obj, speed,dst);
	}
	public TweenAlpha add_alpha_anim(GameObject obj,float speed,float from,float to,float delay)
	{
		if(obj.GetComponent<UISprite>() != null)
		{
			obj.GetComponent<UISprite>().alpha = from;
		}
		else if(obj.GetComponent<UIPanel>() != null)
		{
			obj.GetComponent<UIPanel>().alpha = from;
		}

		TweenAlpha _alpha = TweenAlpha.Begin (obj, speed, to);

		_alpha.method = UITweener.Method.EaseInOut;
		_alpha.from = from;
		_alpha.to = to;
		_alpha.delay = delay;

		return _alpha;
	}
	public TweenScale add_scale_anim(GameObject obj,float speed, float from, float to, float delay)
	{
		obj.transform.localScale = new Vector3 (from, from, from);
		TweenScale _scale = TweenScale.Begin (obj,speed,new Vector3(to,to,to));
		
		//_scale.updateTable = true;
		_scale.method = UITweener.Method.EaseInOut;
		//_scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 1.05f), new Keyframe(0.8f, 0.97f), new Keyframe(1f, 1f));
		_scale.from = new Vector3 (from, from, from);
		_scale.to = new Vector3(to,to,to);
		_scale.delay = delay;
		return _scale;
	}

    public TweenScale add_scale_animtion(GameObject obj, float speed, float from, float to, float delay)
    {
        TweenScale _scale = TweenScale.Begin(obj, speed, new Vector3(to, to, to));

        //_scale.updateTable = true;
        _scale.method = UITweener.Method.EaseInOut;
        //_scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 1.05f), new Keyframe(0.8f, 0.97f), new Keyframe(1f, 1f));
        _scale.from = new Vector3(from, from, from);
        _scale.to = new Vector3(to, to, to);
        _scale.delay = delay;
        return _scale;
    }
	public TweenPosition add_pos_anim(GameObject obj,float speed,Vector3 pos,float delay)
	{
		Vector3 _from = obj.transform.localPosition + pos;
		Vector3 _to = obj.transform.localPosition;
		obj.transform.localPosition = _from;
		
		TweenPosition _effect = TweenPosition.Begin(obj,speed,obj.transform.localPosition);
		
		_effect.method = UITweener.Method.EaseInOut;
		_effect.from = _from;
		_effect.to = _to;
		_effect.delay = delay;

		return _effect;
	}

    public TweenPosition add_pos_animtion(GameObject obj, float speed, Vector3 start_pos, Vector3 end_pos, float delay)
    {
        Vector3 _from = start_pos;
        Vector3 _to = end_pos;
        obj.transform.localPosition = _from;
        TweenPosition _effect = TweenPosition.Begin(obj, speed, obj.transform.localPosition);
        _effect.method = UITweener.Method.EaseInOut;
        _effect.from = _from;
        _effect.to = _to;
        _effect.delay = delay;
        return _effect;
    }
	public TweenWidth add_width_anim(UIWidget obj, float speed, int from, int to, float delay)
	{
		obj.GetComponent<UIWidget>().width = from;
		
		TweenWidth _tw = TweenWidth.Begin (obj, speed, to);
		
		_tw.method = UITweener.Method.EaseInOut;
		_tw.from = from;
		_tw.to = to;
		_tw.delay = delay;

		return _tw;
	}

	public TweenHeight add_height_anim(UIWidget obj, float speed, int from, int to, float delay)
	{
		obj.GetComponent<UIWidget>().height = from;
		
		TweenHeight _th = TweenHeight.Begin (obj, speed, to);
		
		_th.method = UITweener.Method.EaseInOut;
		_th.from = from;
		_th.to = to;
		_th.delay = delay;

		return _th;
	}

	public void player_need_check()
	{
		m_need_check = true;
	}
   
	public void player_check()
	{
		m_need_check = false;
		if(m_self == null || m_self.m_t_player == null)
		{
			return ;
		}

		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_PLAYER_CHECK, _msg);
	}

	public void xq_check()
	{
		m_xq_check = false;
		if(m_self == null || m_self.m_t_player == null || m_self.m_t_player.level < (int)e_open_level.el_yuehui)
		{
			return ;
		}
		
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_ROLE_XQ_LOOK, _msg);
	}

	public void game_logout()
	{
        remove_all_scene_ex();
        GameObject.Destroy(this.gameObject);
        if (game_start._instance == null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("game_start");
        }
        else
        {
            game_start._instance.init();
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameLoad");
        }
    }

	public void message_clear()
	{
		m_message_type.Clear ();
		m_message_long.Clear ();
	}
    public string discountChange(float value)
    {
        string text = "";
        if (game_data._instance.m_language == e_language.English)
        {
            text = ((10 - value) * 10).ToString() + "%";
        }
        else
        {
            text = ((float)value).ToString();
        }

        return text;
        
    }
	public string value_to_wan(long value)
	{
		string text = "";
		text = ((long)value).ToString ();
		{
            if (game_data._instance.m_language != e_language.English)
            {
                if (value >= 100000)
                {
                    value = value / 10000;
                    text = string.Format(game_data._instance.get_t_language("arena_item.cs_39_58"), ((long)value).ToString());//{0}万
                }
            }
            else
            {
                if (value >= 10000000)
                {
                    value = value / 1000000;
                    text = string.Format("{0}M", ((long)value).ToString());
                }
                else if (value >= 10000)
                {
                    value = value / 1000;
                    text = string.Format("{0}K", ((long)value).ToString());
                }
            }

		}
		return text;
	}
    public Color string_to_color(string value)
    {
        int r = System.Convert.ToInt32(value.Substring(1,2),16);
        int g = System.Convert.ToInt32(value.Substring(3, 2),16);
        int b = System.Convert.ToInt32(value.Substring(5, 2),16);
        return new Color(r/256.0f,g/256.0f,b/256.0f);
    }

	public bool is_hide_reward(int type,int value1)
	{
		if(!m_self.m_exist_huodong)
		{
			if(type == 2 && value1 == (int)e_huodong_item_id.ei_huodong_item1)
			{
				return true;
			}
			if(type == 2 && value1 == (int)e_huodong_item_id.ei_huodong_item2)
			{
				return true;
			}
		}
		return false;
	}

	public void get_chenghao(int id,GameObject obj)
	{
		s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao (id);
        if (t_chenghao != null)
        {
            obj.GetComponent<UISprite>().spriteName = t_chenghao.icon1;
            obj.transform.Find("name").GetComponent<UILabel>().text = t_chenghao.color + t_chenghao.name;
            obj.transform.Find("name").GetComponent<UILabel>().effectColor = get_chenghao_color(id);

        }
        else
        {
            obj.GetComponent<UISprite>().spriteName = "";
            obj.transform.Find("name").GetComponent<UILabel>().text = "";
        }
       
	}

    public int get_czbl(int jewel)
    {
        if (game_data._instance.m_language == e_language.Simplified)
        {
            return jewel / game_data._instance.get_const_vale((int)opclient_t.CONST_CZJH);
        }
        return jewel;
    }

    public Color get_chenghao_color(int id)
	{
		s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao (id);
		Color color = new Color ();
		if(t_chenghao != null)
		{
			string[] s = t_chenghao.color_effect.Split (' ');
			color.r = int.Parse(s[0]) / 255.0f;
			color.g = int.Parse(s[1]) / 255.0f;
			color.b = int.Parse(s[1]) / 255.0f;
			color.a = 1.0f;
			return color;
		}
		color.r = 0;
		color.g = 0;
		color.b = 0;
		color.a = 1.0f;
		return color;
	}

    public string returnfirstSpritename(int id)
    {
        string gq_sprite_name = "";

        if (id >= 100)
        {
            gq_sprite_name = string.Format("gq_{0}", id);
        }
        else if (id >= 10)
        {
            gq_sprite_name = string.Format("gq_0{0}", id);
        }
        else if (id >= 0)
        {
            gq_sprite_name = string.Format("gq_00{0}", id);
        }
        else
        {
            gq_sprite_name = "gq_000";
        }

        return gq_sprite_name;
    }

    void Update () {
        if (m_check_bf == true)
        {
            m_check_bf = false;

            int lbf = m_self.m_t_player.bf;
            for (int i = 0; i < m_self.m_t_player.zhenxing.Count; i++)
            {
                ccard _card = m_self.get_card_guid(m_self.m_t_player.zhenxing[i]);
                if (_card != null)
                {
                    _card.update_role_attr();
                }
                
            }
            int bf = (int)m_self.get_fighting();
            if (bf > lbf)
            {
				root_gui._instance.do_bf(bf- lbf);
            }
            m_self.m_t_player.bf = bf;
        }

		if (m_self.m_need_check_huiyi)
		{
			m_self.m_need_check_huiyi = false;
			m_self.check_huiyi_xuyao();
		}

		if(m_edit_mode)
		{
			return;
		}
		if(m_game_speed < 1.0f)
		{
			m_game_speed += Time.deltaTime / m_game_speed;

			if(m_game_speed > 1.0f)
			{
				m_game_speed = 1.0f;
			}
		}

		if(m_stop_mus == true)
		{
			if(m_play_mus.volume > 0)
			{
				m_play_mus.volume -= Time.deltaTime;
			}

			if(m_play_mus.volume <= 0)
			{
				m_stop_mus = false;
				m_play_mus.volume = 0.0f;
				m_play_mus.Stop();

				if(m_mus_name.Length > 0)
				{
					AudioClip _clip = game_data._instance.get_object_res(m_mus_name, typeof(AudioClip)) as AudioClip;

					if(_clip != null)
					{
						m_play_mus.clip=_clip;
						m_play_mus.loop = true;
						m_play_mus.Play();
					}
				}
			}
		}
		else if(m_play_mus.isPlaying && m_play_mus.volume < 0.8f)
		{
			m_play_mus.volume += Time.deltaTime;
		}
	}
}
