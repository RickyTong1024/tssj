
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class xq_gui : MonoBehaviour ,IMessage{

	public List<ulong> guids = new List<ulong>();
	public List<int> xqs = new List<int>();
	public List<GameObject> m_scens = new List<GameObject>();
	List<int> m_indexes;
	public GameObject m_event;
	public GameObject m_cs;
	int m_index;
	ulong guid = 0;
	int change = 0;
	private s_t_xinqing_event t_xinqing_event ;
	private float m_wait_time = 0;
	public GameObject m_tishi;
	public GameObject m_yh_time;
	public GameObject m_next_yh_time;
	public GameObject m_xq_down_gui;
	public GameObject m_xq_down_now;
	public GameObject m_xq_down_change;
	public GameObject m_xq_down_up;
	public GameObject m_xq_down_down;
	public GameObject m_xq_down_des;
	public GameObject m_xq_down_green;
	public GameObject m_xq_down_red;
	public GameObject m_xq_down_icon;
	public GameObject m_xq_down_des1;
	public GameObject m_reward;
	public GameObject m_xq;
	public GameObject m_panel;
	public List<GameObject> m_sxs = new List<GameObject>();
	public bool flag = false;
	private int jewel = 0;
	float m_font_time = 0;
	int x = -Screen.width;

	public UILabel m_yh_time_Label;
	public UILabel m_yh_next_time_Label;
	public UILabel m_yhing_Label;
	public UILabel m_xl_Label;
	public UILabel m_gc_Label;
	public UILabel m_ts_Label;
	public UILabel m_gy_Label;
	public UILabel m_ct_Label;
	public UILabel m_bf_done_Label;
	long m_showtime;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);

	}
	void OnDestroy()
	{
		CancelInvoke ("time");
		cmessage_center._instance.remove_handle (this);
	}
	void time()
	{
		if (m_showtime > (long)timer.now ()) 
		{
			m_next_yh_time.GetComponent<UILabel>().text = timer.get_time_show_ex (m_showtime - (long)timer.now ());
		} 
		else 
		{
			CancelInvoke ("time");
			reset();
		}
	}
	long reutenTime(int h)
	{
		long now_timer = timer.datetimeconvertcuo(timer.dtnow().Date);
		long star_fighttimer = now_timer + h * 60 * 60 * 1000;
		return star_fighttimer;
	}
	public void reset()
	{
		jewel = 0;
		m_cs.SetActive (true);
		m_event.SetActive (false);
		m_tishi.SetActive(false);
		m_wait_time = 0.0f;
		flag = false;
		int _minute = timer.dtnow().Minute;
		int _hour = timer.dtnow().Hour;

		if( _hour >= 4 &&_hour < 10)
		{
			m_showtime = reutenTime(10);
			InvokeRepeating ("time", 0.0f, 1.0f);
		}
		if(_hour >= 10 &&_hour < 15)
		{
			m_showtime = reutenTime(15);
			InvokeRepeating ("time", 0.0f, 1.0f);

		}
		if(_hour >= 15 && _hour < 19)
		{
			m_showtime = reutenTime(19);
			InvokeRepeating ("time", 0.0f, 1.0f);


		}
		if(_hour >= 19 && _hour < 23)
		{
			m_showtime = reutenTime(23);
			InvokeRepeating ("time", 0.0f, 1.0f);


		}
		if(_hour >= 23)
		{
			m_showtime = reutenTime(28);
			InvokeRepeating ("time", 0.0f, 1.0f);


		}
		if (_hour < 4) 
		{
			m_showtime = reutenTime(4);
			InvokeRepeating ("time", 0.0f, 1.0f);
		}
		if(guids.Count == 0 && is_card())
		{
			m_font_time = 0.05f ;
			x = -Screen.width;
			m_tishi.SetActive(true);
		}
		else
		{
			m_tishi.SetActive(false);
		}
		for(int i = 0; i < game_data._instance.m_dbc_xinqing.get_y() && i < m_sxs.Count;++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_xinqing.get(0, i));
			s_t_xinqing t_xinqing = game_data._instance.get_t_xinqing(id);
			if(t_xinqing.sx_per == 0)
			{
				m_sxs[i].GetComponent<UILabel>().text = game_data._instance.get_t_language ("xq_change_item.cs_92_8");//属性不变
			}
			else if(t_xinqing.sx_per > 0)
			{
				m_sxs[i].GetComponent<UILabel>().text = game_data._instance.get_t_language ("xq_change_item.cs_96_8") + "+" + t_xinqing.sx_per + "%";//最终伤害
			}
			else
			{
				m_sxs[i].GetComponent<UILabel>().text = game_data._instance.get_t_language ("xq_change_item.cs_96_8") + t_xinqing.sx_per + "%";//最终伤害
			}
		
		}
		for(int i = 0 ;i < m_scens.Count;++i)
		{
			m_scens[i].transform.Find("0").gameObject.SetActive(false);
			m_scens[i].transform.GetComponent<BoxCollider>().enabled = false;
		}
		for(int i = 0; i < guids.Count && i < m_scens.Count;++i)
		{
			m_scens[i].transform.GetComponent<BoxCollider>().enabled = true;
			m_scens[i].transform.Find("0").gameObject.SetActive(true);
			sys._instance.remove_child (m_scens[i].transform.Find("0/icon").gameObject);
			GameObject _icon = icon_manager._instance.create_card_icon_ex(guids[i]);
			
			_icon.transform.parent = m_scens[i].transform.Find("0/icon").transform;
			_icon.transform.name = m_scens[i].transform.name;
			_icon.transform.localScale = new Vector3 (1, 1, 1);
			_icon.transform.localPosition = new Vector3 (0, 0, 0);
			UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
		}
	}

	bool is_card()
	{
		for (int i = 0; i < game_data._instance.m_dbc_class.get_y(); ++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_class.get(0, i));
			s_t_class t_class = game_data._instance.get_t_class(id);
			if (t_class.color >= 4)
			{
				if(sys._instance.m_self.has_card(id))
				{
					return true;
				}
			}
			
		}
		return false;
	}

	public static bool effect()
	{
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_yuehui)
		{
			return false;
		}
		int _hour = timer.dtnow().Hour;
		int xq_huor = timer.time2dt (sys._instance.m_self.m_t_player.yh_time).Hour;
		List <ccard> cards = new List<ccard>();
		for (int i = 0; i < game_data._instance.m_dbc_class.get_y(); ++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_class.get(0, i));
			s_t_class t_class = game_data._instance.get_t_class(id);
			if (t_class.color >= 4)
			{
				ccard card = sys._instance.m_self.get_card_id(id);
				if(sys._instance.m_self.has_card(id))
				{
					cards.Add(card);
				}
			}
		}
		if(cards.Count == 0)
		{
			return false;
		}
		if( _hour >= 4 && _hour < 10)
		{
			if(sys._instance.m_self.m_t_player.yh_hour != 1)
			{
				return true;
			}
		}
		else if( _hour >= 10 && _hour < 15)
		{
			if(sys._instance.m_self.m_t_player.yh_hour != 2)
			{
				return true;
			}
		}
		else if( _hour >= 15 && _hour < 19)
		{
			if(sys._instance.m_self.m_t_player.yh_hour != 3)
			{
				return true;
			}
		}
		else if( _hour >= 19 && _hour < 23)
		{
			if(sys._instance.m_self.m_t_player.yh_hour != 4)
			{
				return true;
			}
		}
		else if( _hour >= 23 || _hour < 4)
		{
			if(sys._instance.m_self.m_t_player.yh_hour != 5)
			{
				return true;
			}
		}
		if(timer.time2dt (sys._instance.m_self.m_t_player.yh_time).Day != timer.dtnow().Day)
		{
			int yh_time = 24 - xq_huor + _hour;
			if(yh_time >= 12)
			{
				return true;
			}
		}
		if(sys._instance.m_self.m_t_player.yh_hour == 0 && cards.Count != 0)
		{
			return true;
		}
		return false;
	}


	public Texture2D get_image_half(string name)
	{
		Texture2D _texture = game_data._instance.get_object_res ("ui/yh_pic/" + name, typeof(Texture2D)) as Texture2D;
		
		return _texture;
	}

	public void show_event (string name)
	{
		string s = "";
		guid = 0;
		if(name == "gc")
		{
			s = "yhcj_gc001";
			guid = guids[0];
			m_event.transform.Find("back").GetComponent<UITexture>().mainTexture = get_image_half("yhcj_gc001");
		}
		else if(name == "ct")
		{
			s = "yhcj_fd001";
			guid = guids[1];
			m_event.transform.Find("back").GetComponent<UITexture>().mainTexture = get_image_half("yhcj_fd001");
		}
		else if(name == "gy")
		{
			s = "yhcj_gy001";
			guid = guids[2];
			m_event.transform.Find("back").GetComponent<UITexture>().mainTexture = get_image_half("yhcj_gy001");
		}
		else if(name == "ts")
		{
			s = "yhbj_tsg001";
			guid = guids[3];
			m_event.transform.Find("back").GetComponent<UITexture>().mainTexture = get_image_half("yhbj_tsg001");
		}
		else if(name == "xl")
		{
			s = "yhcj_xlc001";
			guid = guids[4];
			m_event.transform.Find("back").GetComponent<UITexture>().mainTexture = get_image_half("yhcj_xlc001");
		}
		ccard card = sys._instance.m_self.get_card_guid(guid);
		m_event.transform.Find("xuan").gameObject.SetActive (false);
		List<s_t_xinqing_event> xinqing_events = new List<s_t_xinqing_event>();
		for(int i = 0;i < game_data._instance.m_dbc_xinqing_event.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_xinqing_event.get(0, i));
			s_t_xinqing_event  _t_xinqing_event = game_data._instance.get_t_xinqing_event(id);
			if(_t_xinqing_event.scene == s && _t_xinqing_event.role_id == card.get_template_id())
			{
				xinqing_events.Add(_t_xinqing_event);
			}
		}
		List<int> random = new List<int>();
		for(int i = 0 ; i < xinqing_events.Count;++i)
		{
			for(int j = 0;j < xinqing_events[i].rate/10;++j)
			{
				random.Add(xinqing_events[i].id);
			}
		}
		int m_id = Random.Range (0, random.Count);

		t_xinqing_event = game_data._instance.get_t_xinqing_event (random[m_id]);
		root_gui._instance.action_guide (t_xinqing_event.start_scene);
		List<int> xinqing_random_ids = new List<int>();
		m_indexes = new List<int>();
		if(t_xinqing_event.type == 1)
		{
			for(int i = 0;i < game_data._instance.m_dbc_xinqing_random.get_y();++i)
			{
				int id = int.Parse(game_data._instance.m_dbc_xinqing_random.get(0, i));
				s_t_xinqing_random  _t_xinqing_random = game_data._instance.get_t_xinqing_random(id);
				xinqing_random_ids.Add(_t_xinqing_random.id);
			}
			for(int i = 0; i < 3;++i)
			{
				int _id = Random.Range (0, xinqing_random_ids.Count);
				m_indexes.Add(xinqing_random_ids[_id]);
				xinqing_random_ids.RemoveAt(_id);
				
			}
			for(int i =0;i < m_indexes.Count;++i)
			{
				s_t_xinqing_random  _t_xinqing_random = game_data._instance.get_t_xinqing_random(m_indexes[i]);
				GameObject xuan = m_event.transform.Find("xuan").Find("xuan" + (i + 1).ToString()).gameObject;
				xuan.transform.Find("Label").GetComponent<UILabel>().text = _t_xinqing_random.select;
				xuan.GetComponent<BoxCollider>().enabled = true;
			}
		}
		else if(t_xinqing_event.type == 2)
		{
			List<int> m_tmp = new List<int>();
			m_tmp.Add (1);
			m_tmp.Add (2);
			m_tmp.Add (3);
			m_indexes = new List<int>();
			for (int i = 0; i < 3; ++i)
			{
				int j = Random.Range(0, m_tmp.Count);
				m_indexes.Add(m_tmp[j]);
				m_tmp.RemoveAt(j);
			}
			for (int i = 0; i < 3; ++i)
			{
				int j = m_indexes[i];
				GameObject xuan = m_event.transform.Find("xuan").Find("xuan" + (i + 1).ToString()).gameObject;
				if(j == 1)
				{
					xuan.transform.Find("Label").GetComponent<UILabel>().text = t_xinqing_event.select1;
				}
				else if(j == 2)
				{
					xuan.transform.Find("Label").GetComponent<UILabel>().text = t_xinqing_event.select2;
				}
				else
				{
					xuan.transform.Find("Label").GetComponent<UILabel>().text = t_xinqing_event.select3;
				}
				xuan.GetComponent<BoxCollider>().enabled = true;
			}
		}
 	}

	public void click(GameObject obj)
	{
		m_event.SetActive (true);
		m_cs.SetActive (false);
		m_xq_down_gui.SetActive (false);
		show_event (obj.transform.name);

	}

	public void select(GameObject obj)
	{
		if (obj.name == "xuan1")
		{
			m_index = m_indexes[0];
		}
		if (obj.name == "xuan2")
		{
			m_index = m_indexes[1];
		}
		if (obj.name == "xuan3")
		{
			m_index = m_indexes[2];
		}
		GameObject xuan1 = m_event.transform.Find("xuan").Find("xuan1").gameObject;
		xuan1.transform.localPosition = new Vector3(300, 150, 0);
		xuan1.GetComponent<BoxCollider>().enabled = false;
		sys._instance.add_pos_anim(xuan1,0.3f, new Vector3(-300, 0, 0), 0);
		sys._instance.add_alpha_anim(xuan1,0.3f, 1.0f, 0, 0);

		GameObject xuan2 = m_event.transform.Find("xuan").Find("xuan2").gameObject;
		xuan2.transform.localPosition = new Vector3(300, 50, 0);
		xuan2.GetComponent<BoxCollider>().enabled = false;
		sys._instance.add_pos_anim(xuan2,0.3f, new Vector3(-300, 0, 0), 0.1f);
		sys._instance.add_alpha_anim(xuan2,0.3f, 1.0f, 0, 0.1f);
		
		GameObject xuan3 = m_event.transform.Find("xuan").Find("xuan3").gameObject;
		xuan3.transform.localPosition = new Vector3(300, -50, 0);
		xuan3.GetComponent<BoxCollider>().enabled = false;
		sys._instance.add_pos_anim(xuan3,0.3f, new Vector3(-300, 0, 0), 0.2f);
		sys._instance.add_alpha_anim(xuan3,0.3f, 1.0f, 0, 0.2f);
		m_wait_time = 0.5f;
	}

	public void close(GameObject obj)
	{
		this.gameObject.SetActive (false);
		s_message _mes = new s_message();
		_mes.m_type = "show_main_gui";
		cmessage_center._instance.add_message(_mes);
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "guide_end")
		{
			string name = (string)message.m_string[0];
			if (name.Length >= 6 && name.Substring(0, 6) == "yuehui")
			{
				if (name.Substring(name.Length - 1) == "0")
				{
					m_event.transform.Find("xuan").gameObject.SetActive (true);
					
					GameObject xuan1 = m_event.transform.Find("xuan").Find("xuan1").gameObject;
					xuan1.transform.localPosition = new Vector3(0, 150, 0);
					sys._instance.add_pos_anim(xuan1,0.3f, new Vector3(-300, 0, 0), 0);
					sys._instance.add_alpha_anim(xuan1,0.3f, 0, 1.0f, 0);
					
					GameObject xuan2 = m_event.transform.Find("xuan").Find("xuan2").gameObject;
					xuan2.transform.localPosition = new Vector3(0, 50, 0);
					sys._instance.add_pos_anim(xuan2,0.3f, new Vector3(-300, 0, 0), 0.1f);
					sys._instance.add_alpha_anim(xuan2,0.3f, 0, 1.0f, 0.1f);
					
					GameObject xuan3 = m_event.transform.Find("xuan").Find("xuan3").gameObject;
					xuan3.transform.localPosition = new Vector3(0, -50, 0);
					sys._instance.add_pos_anim(xuan3,0.3f, new Vector3(-300, 0, 0), 0.2f);
					sys._instance.add_alpha_anim(xuan3,0.3f, 0, 1.0f, 0.2f);
				}
				else
				{
					show_xinqing_done_gui();
				}
			}
		}
	}

	void show_xinqing_done_gui()
	{
		string s = "";
		m_xq_down_gui.SetActive(true);
		ccard card = sys._instance.m_self.get_card_guid(guid);
		sys._instance.remove_child (m_xq_down_icon);
		GameObject _icon = icon_manager._instance.create_card_icon_ex(guid);
		if(jewel > 0)
		{
			m_xq.transform.localPosition = new Vector3(0,51,0);
			m_reward.SetActive(true);
			m_reward.transform.Find("zs").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + "x" + jewel.ToString();
		}
		else
		{
			m_xq.transform.localPosition = new Vector3(0,32,0);
			m_reward.SetActive(false);
		}
		_icon.transform.parent = m_xq_down_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		_icon.transform.GetComponent<BoxCollider>().enabled = false;
		m_xq_down_now.GetComponent<UISprite>().spriteName = xq_change_item.xq_icon (card.get_role().xq - change);
		m_xq_down_change.GetComponent<UISprite>().spriteName = xq_change_item.xq_icon (card.get_role().xq);
		if(change >= 0)
		{
			m_xq_down_up.SetActive(true);
			m_xq_down_down.SetActive(false);
			if(change != 0 )
			{
				if(t_xinqing_event.type == 1)
				{
					m_panel.transform.localPosition = new Vector3(0,0,0);
					s_t_xinqing_random  _xinqing_random  = game_data._instance.get_t_xinqing_random(m_index);
					s = _xinqing_random.good_result.Replace("XX",card.m_t_class.name);
					m_xq_down_des1.SetActive(true);
					m_xq_down_des1.transform.Find("des1").GetComponent<UILabel>().text = "[00ff00]" + s;
				}
				else
				{
					m_xq_down_des1.SetActive(false);
					m_panel.transform.localPosition = new Vector3(0,-17,0);
				}
				m_xq_down_green.SetActive(true);
				m_xq_down_red.SetActive(false);
				m_xq_down_des.GetComponent<UILabel>().text ="[00ff00]" + game_data._instance.get_t_language ("心情上升,战斗力上升");
			}
			else
			{
				if(t_xinqing_event.type == 1)
				{
					m_panel.transform.localPosition = new Vector3(0,0,0);
					s_t_xinqing_random  _xinqing_random  = game_data._instance.get_t_xinqing_random(m_index);
					s = _xinqing_random.bad_result.Replace("XX",card.m_t_class.name);
					m_xq_down_des1.SetActive(true);
					m_xq_down_des1.transform.Find("des1").GetComponent<UILabel>().text =  s;
				}
				else
				{
					m_xq_down_des1.SetActive(false);
					m_panel.transform.localPosition = new Vector3(0,-17,0);
				}
				m_xq_down_green.SetActive(false);
				m_xq_down_red.SetActive(false);
				m_xq_down_des.GetComponent<UILabel>().text = game_data._instance.get_t_language ("xq_gui.cs_517_49");//伙伴的心情没有发生变化
			}
		}
		else
		{
			m_xq_down_des1.SetActive(false);
			m_panel.transform.localPosition = new Vector3(0,-17,0);
			m_xq_down_up.SetActive(false);
			m_xq_down_down.SetActive(true);
			m_xq_down_green.SetActive(false);
			m_xq_down_red.SetActive(true);
			m_xq_down_des.GetComponent<UILabel>().text = "[ff0000]" + game_data._instance.get_t_language ("心情下降,战斗力下降");
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ROLE_YH_SELECT)
		{
			protocol.game.smsg_role_yh_select _msg = net_http._instance.parse_packet<protocol.game.smsg_role_yh_select> (message.m_byte);
			guid = _msg.guid;
			ccard card = sys._instance.m_self.get_card_guid(_msg.guid);
			change = _msg.xq - card.get_role().xq;
			card.get_role().xq = _msg.xq;
			jewel = _msg.jewel;
			sys._instance.m_self.m_t_player.yh_time = timer.now();
			int _hour = timer.dtnow().Hour;
			if( _hour >= 4 && _hour < 10)
			{
				sys._instance.m_self.m_t_player.yh_hour = 1;
			}
			else if( _hour >= 10 && _hour < 15)
			{
				sys._instance.m_self.m_t_player.yh_hour = 2;
			}
			else if( _hour >= 15 && _hour < 19)
			{
				sys._instance.m_self.m_t_player.yh_hour = 3;
			}
			else if( _hour >= 19 && _hour < 23)
			{
				sys._instance.m_self.m_t_player.yh_hour = 4;
			}
			else if( _hour >= 23 || _hour < 4)
			{
				sys._instance.m_self.m_t_player.yh_hour = 5;
			}
			if(t_xinqing_event.type == 2)
			{
				if(m_index == 1)
				{
					root_gui._instance.action_guide (t_xinqing_event.end_scene1);
				}
				else if(m_index == 2)
				{
					root_gui._instance.action_guide (t_xinqing_event.end_scene2);
				}
				else
				{
					root_gui._instance.action_guide (t_xinqing_event.end_scene3);
				}
			}
			else
			{
				show_xinqing_done_gui();
			}
			sys._instance.m_self.add_att(e_player_attr.player_jewel,jewel,game_data._instance.get_t_language ("xq_gui.cs_583_65"));//心情获得
		}
	}

	// Update is called once per frame
	void Update () {
		m_font_time -= Time.deltaTime;
		if(m_font_time < 0)
		{
			m_font_time = 0.05f;
			m_tishi.transform.Find("tishi").Translate(Vector3.right * Time.deltaTime);
			if(m_tishi.transform.Find("tishi").localPosition.x >= Screen.width)
			{
				m_tishi.transform.Find("tishi").localPosition = new Vector3(-Screen.width,0,0);
			}
		}
		if (m_wait_time > 0)
		{
			m_wait_time -= Time.deltaTime;
			if (m_wait_time <= 0)
			{
				if(t_xinqing_event.type == 1)
				{
					for(int i = 0;i < game_data._instance.m_dbc_xinqing_random.get_y();++i)
					{
						int id = int.Parse(game_data._instance.m_dbc_xinqing_random.get(0, i));
						s_t_xinqing_random  _t_xinqing_random = game_data._instance.get_t_xinqing_random(id);
						if(m_index ==_t_xinqing_random.id)
						{
							m_event.transform.Find("back").GetComponent<UITexture>().mainTexture = get_image_half(_t_xinqing_random.scene);
						}
					}
				}
				m_event.transform.Find("xuan").gameObject.SetActive (false);
				
				protocol.game.cmsg_role_yh_select _msg = new protocol.game.cmsg_role_yh_select ();
				_msg.guid = guid;
				if(t_xinqing_event.type == 2)
				{
					_msg.index = m_index;
				}
				else
				{
					_msg.index = 0;
				}
				net_http._instance.send_msg<protocol.game.cmsg_role_yh_select> (opclient_t.CMSG_ROLE_YH_SELECT, _msg);
			}
		}
	}
}
