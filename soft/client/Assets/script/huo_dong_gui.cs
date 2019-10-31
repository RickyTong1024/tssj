
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class huo_dong_gui : MonoBehaviour,IMessage {
	
	public GameObject m_hd_list;
	public List<s_t_huodong_sub> m_huodong_subs = new List<s_t_huodong_sub>(); 
	public int m_select = 6;
	private List<GameObject> m_huodong_items = new List<GameObject>();
	public static int m_yb_effect = 0;
	public static int m_qd_effect = 0;
	public static int m_jjc_effect = 0;
	public float offset = 0;
	public bool flag = false;
	private string error = "";
	private Vector3 m_mouse_pos;
	public GameObject m_right;
	public GameObject m_left;
	public GameObject weight;
	s_t_huodong_sub _huodong_sub;
	private int m_vw;
	public List<Texture> m_texs;

	// Use this for initialization
	void Start ()
	{
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		float _height = root_gui._instance.m_ui_cam.transform.parent.GetComponent<UIRoot>().activeHeight;
		float bl = _height / Screen.height;
		m_vw = (int)(Screen.width * bl) - 220;
		m_hd_list.GetComponent<UIPanel>().baseClipRegion = new Vector4(0, 0, m_vw, 536);
		weight.GetComponent<UIWidget>().width = m_vw;
		weight.GetComponent<UIWidget>().height = 536;
		weight.GetComponent<BoxCollider>().size = new Vector3(m_vw,536,0);
		//select_map (m_select);
		update_ui ();
		refresh_items ();
		InvokeRepeating("time", 0.0f,1.0f);
	}
	
	void OnDisable()
	{
		CancelInvoke ("time");
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.GetComponent<ui_show_anim>().hide_ui();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);

			Object.Destroy(this.gameObject);
		}
		if(obj.transform.name == "add_nl")
		{
			int item_id = 10010007;
			int num = sys._instance.m_self.get_item_num((uint)item_id);
			if(num > 0)
			{
				root_gui._instance.show_tili_dialog_box(item_id);
				return;
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "buy_num_gui";
				_message.m_ints.Add(100300);
				_message.m_ints.Add(2);
				cmessage_center._instance.add_message(_message);
				return;
			}
		}
	}

	public void select_mission(GameObject obj)
	{
		int _id = int.Parse(obj.transform.name);
		m_select = _id;
		select_map (_id);
		if(error != "")
		{
			root_gui._instance.show_prompt_dialog_box(error);
			return;
		}
		if(_id == 4 || _id == 5)
		{
			flag = false;
		}
		for(int j = 0;j < game_data._instance.m_dbc_huodong_sub.get_y();j ++)
		{
			int sub_id = int.Parse(game_data._instance.m_dbc_huodong_sub.get(0,j));
			_huodong_sub = game_data._instance.get_t_huodong_sub(sub_id);
			if(_huodong_sub.pid == _id)
			{
				break;
			}
		}

		s_t_huodong _huodong = game_data._instance.get_t_huodong(_id);

		if(_huodong.id == 1)
		{
			sys._instance.m_game_state = "boss";
			sys._instance.load_scene("ts_game_boss");
		}
		else if(_huodong.id == 2)
		{
			sys._instance.m_game_state = "transport";
			sys._instance.load_scene("ts_game_transport");
		}
		else if(_huodong.id == 3)
		{
			sys._instance.m_game_state = "ying_jiu";
			sys._instance.load_scene("ts_game_hhb");
		}
		else if (_huodong.id == 4)
		{
			s_message _message = new s_message();
			_message.m_type = "show_ore_gui";
			cmessage_center._instance.add_message(_message);
		}
		else if(_huodong.id == 5)
		{
			sys._instance.m_game_state = "mi_jing";
			sys._instance.load_scene("ts_game_mijing");
		}
		else if(_huodong.id == 6)
		{
			this.transform.GetComponent<ui_show_anim>().hide_ui();
			s_message _mes = new s_message();
			_mes.m_type = "show_jing_ji_chang";
			cmessage_center._instance.add_message(_mes);
		}
		else if(_huodong.id == 9)
		{
            s_message _mes = new s_message();
            _mes.m_type = "show_baowu_hecheng_gui";
            cmessage_center._instance.add_message(_mes);
			
		}
        else if (_huodong.id == 10)
        {
            sys._instance.m_game_state = "pvp";
            sys._instance.load_scene("ts_game_lieren");
 
        }
        else if (_huodong.id == 11)
        {
            sys._instance.m_game_state = "bingyuan";
            sys._instance.load_scene("ts_game_bingyuan");
        }
        else if (_huodong.id == 12)
        {
            sys._instance.m_game_state = "master";
            sys._instance.load_scene("ts_game_master");
        }
	}

	public static bool can_effect()
	{
		if ((is_time_up(1) || boss_task_gui.effect()) && sys._instance.m_self.get_att(e_player_attr.player_level) >= (int)e_open_level.el_mowang)
		{
			return true;
		}
		if (m_yb_effect == 1 && sys._instance.m_self.get_att(e_player_attr.player_level) >= (int)e_open_level.el_transport_ship)
		{
			return true;
		}
		if ((m_qd_effect == 1 || baowu_hecheng.is_canhecheng()) && sys._instance.m_self.get_att(e_player_attr.player_level) >= (int)e_open_level.el_treasure_qu)
		{
			return true;
		}
		if ((m_jjc_effect == 1 || honor_shop_gui.is_effect())&& sys._instance.m_self.get_att(e_player_attr.player_level) >= (int)e_open_level.el_jjc)
		{
			return true;
		}
		if ((mijing_shop_gui.is_effect() || sys._instance.m_self.m_t_player.ttt_dead == 0) && sys._instance.m_self.get_att(e_player_attr.player_level) >= (int)e_open_level.el_mijing)
		{
			return true;
		}
		if(((sys._instance.m_self.m_t_player.hbb_num - sys._instance.m_self.m_t_player.hbb_finish_num) > 0) && sys._instance.m_self.get_att(e_player_attr.player_level) >= (int)e_open_level.el_hhb )
		{
			return true;
		}
        if (pvp_gui.is_effect())
        {
            return true;
        }
        if (bingyuan_gui.is_effect() && sys._instance.m_self.get_att(e_player_attr.player_level) >= (int)e_open_level.el_bingyuan)
        {
            return true;
        }
		int num = 3 - sys._instance.m_self.m_t_player.ore_finish_num;
		if (num > 0 && (sys._instance.m_self.get_att(e_player_attr.player_level) >= (int)e_open_level.el_baozang) && (sys._instance.m_self.m_t_player.ore_last_time + 600000 <= timer.now()))
		{
			return true;
		}
		return false;
	}
  
	private void refresh_items()
	{
		if (m_huodong_items.Count == 0)
		{
			return;
		}
		for(int i = 0;i < game_data._instance.m_dbc_huodong.get_y() && i < m_huodong_items.Count;i ++)
		{
			int level = 0;
			int _id = int.Parse(game_data._instance.m_dbc_huodong.get(0,i));
			GameObject _item = m_huodong_items[i];
			GameObject effect = _item.transform.Find("effect").gameObject;
			for(int j = 0;j < game_data._instance.m_dbc_huodong.get_y();j ++)
			{
				int sub_id = int.Parse(game_data._instance.m_dbc_huodong_sub.get(0,j));
				s_t_huodong_sub _sub = game_data._instance.get_t_huodong_sub(sub_id);
				if(_sub.pid == _id)
				{
					level = _sub.level;
					break;
				}
			}
            if (_id == 1)
            {
                if ((is_time_up(1) || boss_task_gui.effect()) && sys._instance.m_self.get_att(e_player_attr.player_level) >= level)
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 2)
            {
                if (m_yb_effect == 1 && sys._instance.m_self.get_att(e_player_attr.player_level) >= level)
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 3)
            {
                int num = sys._instance.m_self.m_t_player.hbb_num - sys._instance.m_self.m_t_player.hbb_finish_num;
                if (num > 0 && sys._instance.m_self.get_att(e_player_attr.player_level) >= level)
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 4)
            {
                int num = 3 - sys._instance.m_self.m_t_player.ore_finish_num;
                if (num > 0 && (sys._instance.m_self.get_att(e_player_attr.player_level) >= level) && (sys._instance.m_self.m_t_player.ore_last_time + 600000 <= timer.now()))
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 5)
            {
                if ((mijing_shop_gui.is_effect() || sys._instance.m_self.m_t_player.ttt_dead == 0) && sys._instance.m_self.get_att(e_player_attr.player_level) >= level)
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 6)
            {
                if ((m_jjc_effect == 1 || honor_shop_gui.is_effect()) && sys._instance.m_self.get_att(e_player_attr.player_level) >= level)
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 9)
            {
                if ((m_qd_effect == 1 || baowu_hecheng.is_canhecheng()) && sys._instance.m_self.get_att(e_player_attr.player_level) >= level)
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 10)
            {
                if (pvp_gui.is_effect())
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 11)
            {
                if (bingyuan_gui.is_effect() && sys._instance.m_self.get_att(e_player_attr.player_level) >= level)
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else if (_id == 12)
            {
                if (masterleague_gui.is_effect() && sys._instance.m_self.get_att(e_player_attr.player_level) >= level)
                {
                    effect.SetActive(true);
                }
                else
                {
                    effect.SetActive(false);
                }
            }
            else
            {
                effect.SetActive(false);
            }
		}
	}

	public static bool is_time_up(int id)
	{
		int _minute = timer.dtnow().Minute;
		int _hour = timer.dtnow().Hour;
		s_t_huodong _huodong = game_data._instance.get_t_huodong(id);

		for(int c = 0;c < _huodong.times.Count;c += 4)
		{
			if(
				((_hour == _huodong.times[c] && _minute >= _huodong.times[c + 1]) ||  _hour > _huodong.times[c])
				&& 
				((_hour == _huodong.times[c + 2] && _minute < _huodong.times[c + 3]) || _hour < _huodong.times[c + 2])
				)
			{
				return true;
			}
		}

		for(int c = 0;c < _huodong.times.Count;c ++)
		{
			if(_huodong.times[c] > 0)
			{
				return false;
			}
		}

		return true;
	}

	public static string get_time(int id)
	{
		for(int i = 0;i < game_data._instance.m_dbc_huodong.get_y();i ++)
		{
			int _id = int.Parse(game_data._instance.m_dbc_huodong.get(0,i));
			if (_id == id)
			{
				s_t_huodong _huodong = game_data._instance.get_t_huodong(_id);
				return get_time(_huodong);
			}
		}
		return "";
	}

	static string get_time(s_t_huodong _huodong)
	{
		int _minute = timer.dtnow().Minute;
		int _hour = timer.dtnow().Hour;
		string _time = "";
		bool flag = false;
		for(int c = 0;c < _huodong.times.Count && _huodong.times[0] > 0;c += 4)
		{
			int mini;
			if (_hour < _huodong.times[c] || (_hour == _huodong.times[c] && _minute < _huodong.times[c + 1]))
			{
				mini = (_huodong.times[c] - _hour) * 60 - _minute + _huodong.times[c + 1];
				if (mini >= 60)
				{
					_time = string.Format(game_data._instance.get_t_language ("huo_dong_gui.cs_419_27"),(mini / 60).ToString());	//{0}小时后开始
				}
				else
				{
					_time = string.Format(game_data._instance.get_t_language ("huo_dong_gui.cs_423_27"),mini.ToString());	//{0}分钟后开始
				}
				flag = true;
				break;
			}
			else if (_hour < _huodong.times[c + 2] || (_hour == _huodong.times[c + 2] && _minute < _huodong.times[c + 3]))
			{
				mini = (_huodong.times[c + 2] - _hour) * 60 - _minute + _huodong.times[c + 3];
				if (mini >= 60)
				{
					_time = string.Format(game_data._instance.get_t_language ("huo_dong_gui.cs_433_27"),(mini / 60).ToString());	//{0}小时后结束
				}
				else
				{
					_time = string.Format(game_data._instance.get_t_language ("huo_dong_gui.cs_437_27"),mini.ToString());	//{0}分钟后结束
				}
				flag = true;
				break;
			}
		}
		if (!flag && _huodong.times[0] > 0)
		{
			_time = game_data._instance.get_t_language ("huo_dong_gui.cs_445_11");//今天活动已结束
		}
		return _time;
	}

	void drag(GameObject obj,Vector2 dela)
	{
		if(dela.x > 0 )
		{
			m_right.SetActive(true);
			m_right.transform.Find("right").gameObject.SetActive(true);
			m_left.SetActive(false);
		}
		else if(dela.x < 0)
		{
			m_left.SetActive(true);
			m_left.transform.Find("left").gameObject.SetActive(true);
			m_right.SetActive(false);
		}
	}

	void Press (GameObject obj,bool press) 
	{
		if(!press)
		{
			m_left.SetActive(false);
			m_right.SetActive(false);
			m_left.transform.Find("left").gameObject.SetActive(false);
			m_right.transform.Find("right").gameObject.SetActive(false);
		}
	}
	public void update_ui()
	{
		sys._instance.remove_child(m_hd_list);
		m_huodong_items.Clear ();
		for(int i = 0;i < game_data._instance.m_dbc_huodong.get_y();i ++)
		{
			int _id = int.Parse(game_data._instance.m_dbc_huodong.get(0,i));
			s_t_huodong _huodong = game_data._instance.get_t_huodong(_id);

			GameObject _item = game_data._instance.ins_object_res("ui/huo_dong_item");
			UIEventListener.Get(_item).onDrag = drag;
			UIEventListener.Get(_item).onPress = Press;
			_item.transform.name = _id.ToString();
			_item.transform.parent = m_hd_list.transform;
		
			_item.transform.localPosition = new Vector3(-m_vw / 2 + 124 + 245*i, 55, 0);

			_item.transform.localScale = new Vector3(1,1,1);

			_item.transform.Find("name").GetComponent<UILabel>().text = _huodong.name;
			string s = get_time(_huodong);
			if (s == "")
			{
				s = _huodong.sdes;
			}
			_item.transform.Find("time").GetComponent<UILabel>().text = s;
			_item.transform.Find("dec").GetComponent<UILabel>().text = _huodong.des;
			_item.transform.GetComponent<UITexture>().mainTexture = m_texs[_huodong.icon];
			_item.GetComponent<UIButtonMessage>().target = this.gameObject;
			if(_id == m_select)
			{
				//_item.transform.GetComponent<UIToggle>().startsActive = true;
				_item.transform.Find("select").gameObject.SetActive(true);
			}
			else
			{
				_item.transform.Find("select").gameObject.SetActive(false);
			}

			information(_item,_id);

			m_huodong_items.Add(_item);
			bool c_flag = false;
			for(int j = 0;j < game_data._instance.m_dbc_huodong_sub.get_y();j ++)
			{
				int sub_id = int.Parse(game_data._instance.m_dbc_huodong_sub.get(0,j));
				s_t_huodong_sub _sub = game_data._instance.get_t_huodong_sub(sub_id);
				
				if(_sub.pid == _id)
				{
					if (sys._instance.m_self.get_att(e_player_attr.player_level) < _sub.level)
					{
						c_flag = true;
						break;
					}
				}
			}
			if(c_flag)
			{
				break;
			}
		}
		if (flag)
		{
			offset = 0;
			float length = (float)(226*get_pos()+(get_pos()-1)*19 - m_vw +24);
			if(length > 0)
			{
				offset = length;
			}
			m_hd_list.transform.localPosition = new Vector3 (-offset, 0, 0);
			m_hd_list.GetComponent<UIPanel>().clipOffset = new Vector2 (offset, 0);
		}
		m_left.SetActive(false);
		m_right.SetActive(false);
		refresh_items ();
	}

	public int get_pos()
	{
		int num = 0;
		for(int i = 0; i < m_huodong_items.Count;++i)
		{
			num++;
			if( int.Parse(m_huodong_items[i].name) == m_select)
			{
				break;
			}
		}
		return num;
	}

	public void information(GameObject _item, int _id)
	{
		for(int j = 0;j < game_data._instance.m_dbc_huodong_sub.get_y();j ++)
		{
			int sub_id = int.Parse(game_data._instance.m_dbc_huodong_sub.get(0,j));
			s_t_huodong_sub _sub = game_data._instance.get_t_huodong_sub(sub_id);
			
			if(_sub.pid == _id)
			{
				
				m_huodong_subs.Add(_sub);
				if (sys._instance.m_self.get_att(e_player_attr.player_level) < _sub.level)
				{
					_item.transform.Find("back").gameObject.SetActive(true);
					_item.transform.Find("ok").gameObject.SetActive(false);
					_item.transform.Find("suo").gameObject.SetActive(false);
					_item.transform.Find("level").gameObject.SetActive(true);
					_item.transform.Find("time").gameObject.SetActive(false);
					_item.transform.Find("level").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("duixing_gui.cs_431_71"),_sub.level.ToString() );//{0}级开启
				}
				else
				{
					_item.transform.Find("back").gameObject.SetActive(false);
					_item.transform.Find("ok").gameObject.SetActive(true);
					_item.transform.Find("suo").gameObject.SetActive(true);
					_item.transform.Find("level").gameObject.SetActive(false);
					_item.transform.Find("time").gameObject.SetActive(false);
				}
				_item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_596_75");//消耗体力
				_item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_597_78");//剩余次数
				if(_sub.mission_id == 0)
				{
					
					_item.transform.Find("ti_li").GetComponent<UILabel>().text = "";
					_item.transform.Find("count").GetComponent<UILabel>().text = "";
					if (_sub.pid == 3)
					{
						string s = "[00ffff]";
						_item.transform.Find("ok").gameObject.SetActive(true);
						_item.transform.Find("suo").gameObject.SetActive(false);
						int num = sys._instance.m_self.m_t_player.hbb_num - sys._instance.m_self.m_t_player.hbb_finish_num;
						if (num <= 0)
						{
							s = "[ff0000]";
						}
						_item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_596_75");//消耗体力
						_item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_597_78");//剩余次数
						_item.transform.Find("count").GetComponent<UILabel>().text = s+  string.Format(game_data._instance.get_t_language ("huo_dong_gui.cs_615_90") , num.ToString());//今日{0}次
						_item.transform.Find("ti_li").GetComponent<UILabel>().text = s +game_data._instance.get_t_language ("huo_dong_gui.cs_616_75");//无消耗
					}
					else if(_sub.pid == 1)
					{

						if(sys._instance.m_self.m_t_player.level < _sub.level)
						{
							_item.transform.Find("level").gameObject.SetActive(true);
							_item.transform.Find("time").gameObject.SetActive(false);
						}
						else
						{
							_item.transform.Find("level").gameObject.SetActive(false);
							_item.transform.Find("time").gameObject.SetActive(true);
						}
						_item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_596_75");//消耗体力
						_item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_632_80");//开启状态
						if(is_time_up(1))
						{
							_item.transform.Find("ok").gameObject.SetActive(true);
							_item.transform.Find("suo").gameObject.SetActive(false);
							_item.transform.Find("count").GetComponent<UILabel>().text = "[00ffff]" + game_data._instance.get_t_language ("huo_dong_gui.cs_637_86");//已开启
						}
						else
						{
							_item.transform.Find("ok").gameObject.SetActive(false);
							_item.transform.Find("suo").gameObject.SetActive(true);
							_item.transform.Find("count").GetComponent<UILabel>().text = "[ff0000]" + game_data._instance.get_t_language ("huo_dong_gui.cs_643_86");//未开启
						}
						_item.transform.Find("ti_li").GetComponent<UILabel>().text = "[00ffff]" + game_data._instance.get_t_language ("huo_dong_gui.cs_616_75");//无消耗
					}
					else if(_sub.pid == 2)
					{
						_item.transform.Find("ok").gameObject.SetActive(true);
						_item.transform.Find("suo").gameObject.SetActive(false);
						_item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_651_77");//今日还可拦截
						_item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_652_80");//今日还可运输
						_item.transform.Find("count").GetComponent<UILabel>().text = "[00ffff]" + string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),(3 - sys._instance.m_self.m_t_player.yb_finish_num));//{0}次
						_item.transform.Find("ti_li").GetComponent<UILabel>().text =  "[00ffff]" +  string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),(5 - sys._instance.m_self.m_t_player.ybq_finish_num));//{0}次
					}
					else if(_sub.pid == 4)
					{
						_item.transform.Find("ok").gameObject.SetActive(true);
						_item.transform.Find("suo").gameObject.SetActive(false);
						string s = "[00ffff]";
						int num = 3 - sys._instance.m_self.m_t_player.ore_finish_num;
						if (num <= 0)
						{
							s = "[ff0000]";
						}
						string ss = "[00ffff]";
						if (sys._instance.m_self.m_t_player.ore_last_time + 600000 > timer.now())
						{
							_item.transform.Find("ok").gameObject.SetActive(false);
							_item.transform.Find("suo").gameObject.SetActive(true);
							ss = "[ff0000]";
						}
						_item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_673_77");//冷却时间
						_item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_597_78");//剩余次数
						_item.transform.Find("count").GetComponent<UILabel>().text =  s +  string.Format(game_data._instance.get_t_language ("huo_dong_gui.cs_615_90") , (3 - sys._instance.m_self.m_t_player.ore_finish_num).ToString());//今日{0}次
						_item.transform.Find("ti_li").GetComponent<UILabel>().text =  ss + timer.get_time_show((long)(sys._instance.m_self.m_t_player.ore_last_time + 600000 - timer.now()));
					}
					else if(_sub.pid == 5)
					{
						_item.transform.Find("ok").gameObject.SetActive(true);
						_item.transform.Find("suo").gameObject.SetActive(false);
						_item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_596_75");//消耗体力
						_item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_683_80");//当前状态
						if(sys._instance.m_self.m_t_player.ttt_dead == 1)
						{
							_item.transform.Find("count").GetComponent<UILabel>().text = "[ff0000]" + game_data._instance.get_t_language ("huo_dong_gui.cs_686_86");//已死亡
						}
						else
						{
							_item.transform.Find("count").GetComponent<UILabel>().text =  "[00ffff]" + game_data._instance.get_t_language ("huo_dong_gui.cs_690_87");//可挑战
						}
						
						_item.transform.Find("ti_li").GetComponent<UILabel>().text = "[00ffff]" + game_data._instance.get_t_language ("huo_dong_gui.cs_616_75");//无消耗
					}
					else if(_sub.pid == 6 || _sub.pid == 9)
					{
						_item.transform.Find("ok").gameObject.SetActive(true);
						_item.transform.Find("suo").gameObject.SetActive(false);
						_item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_699_77");//消耗能量
						_item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_700_80");//剩余能量
						_item.transform.Find("count").GetComponent<UILabel>().text =  "[00ffff]" +sys._instance.m_self.m_t_player.energy + "";
						string s = "[00ffff]";
						if(sys._instance.m_self.m_t_player.energy  < 2)
						{
							s = "[ff0000]";
						}
						_item.transform.Find("ti_li").GetComponent<UILabel>().text =  s + "2";
					}
                    else if (_sub.pid == 10)
                    {
                        if (pvp_gui.is_lieren())
                        {
                            _item.transform.Find("ok").gameObject.SetActive(true);
                            _item.transform.Find("suo").gameObject.SetActive(false);
                            _item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text =
                                game_data._instance.get_t_language ("huo_dong_gui.cs_597_78");//剩余次数
                            _item.transform.Find("ti_li").GetComponent<UILabel>().text =
                               "[00ffff]" + sys._instance.m_self.m_t_player.pvp_num + "";
                            _item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text =
                               game_data._instance.get_t_language ("huo_dong_gui.cs_632_80");//开启状态
                            _item.transform.Find("count").GetComponent<UILabel>().text =
                               game_data._instance.get_t_language ("huo_dong_gui.cs_722_31");//[00ffff]已开启
                        }
                        else
                        {
                            _item.transform.Find("ok").gameObject.SetActive(true);
                            _item.transform.Find("suo").gameObject.SetActive(false);
                            _item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text =
                                game_data._instance.get_t_language ("huo_dong_gui.cs_597_78");//剩余次数
                            _item.transform.Find("ti_li").GetComponent<UILabel>().text =
                               "[00ffff]" + sys._instance.m_self.m_t_player.pvp_num + "";
                            _item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text =
                               game_data._instance.get_t_language ("huo_dong_gui.cs_632_80");//开启状态
                            _item.transform.Find("count").GetComponent<UILabel>().text =
                               game_data._instance.get_t_language ("huo_dong_gui.cs_735_31");//[ff0000]未开启
 
                        }
                       
 
                    }
                    else if (_sub.pid == 11)
                    {
                        _item.transform.Find("ok").gameObject.SetActive(true);
                        _item.transform.Find("suo").gameObject.SetActive(false);
                        _item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text =
                               game_data._instance.get_t_language ("huo_dong_gui.cs_596_75"); ;//消耗体力
                        _item.transform.Find("ti_li").GetComponent<UILabel>().text =
                           "[00ffff]" + game_data._instance.get_t_language ("huo_dong_gui.cs_616_75");//无消耗

                        _item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text =
                           game_data._instance.get_t_language ("huo_dong_gui.cs_751_27");//剩余奖励次数
                        _item.transform.Find("count").GetComponent<UILabel>().text =
                               "[00ffff]" + sys._instance.m_self.m_t_player.by_reward_num;

                    }
                    else if (_sub.pid == 12)
                    {
                        _item.transform.Find("ok").gameObject.SetActive(true);
                        _item.transform.Find("suo").gameObject.SetActive(false);
                        _item.transform.Find("xiaohao_ti").GetComponent<UILabel>().text =
                               game_data._instance.get_t_language ("huo_dong_gui.cs_596_75"); ;//消耗体力
                        _item.transform.Find("ti_li").GetComponent<UILabel>().text =
                           "[00ffff]" + game_data._instance.get_t_language ("huo_dong_gui.cs_616_75");//无消耗

                        _item.transform.Find("shengyu_cishu").GetComponent<UILabel>().text =
                           game_data._instance.get_t_language ("huo_dong_gui.cs_751_27");//剩余奖励次数
                        _item.transform.Find("count").GetComponent<UILabel>().text =
                               "[00ffff]" + sys._instance.m_self.m_t_player.ds_reward_num;
 
                    }
                    int reward_num = 0;
					for(int c = 0;c < 4;c ++)
					{	
						if(_sub.values[c * 4 + 1] != 0)
						{
							reward_num++;
						}

					}
					for(int c = 0;c < 4;c ++)
					{
						GameObject _icon = icon_manager._instance.create_reward_icon(_sub.values[c * 4],_sub.values[c * 4 + 1],_sub.values[c * 4  + 2],_sub.values[c * 4  + 3]);
						Transform _root =  _item.transform.Find("icon_" + c);
						sys._instance.remove_child(_root.gameObject);

						if(_icon != null)
						{
							if(reward_num == 1)
							{
								_root.localPosition = new Vector3 (0,-280,0);
							}
							else if(reward_num == 2)
							{
								_root.localPosition = new Vector3 (-28.5f + 57*c,-280,0);
							}
							else if(reward_num == 3)
							{
								_root.localPosition = new Vector3 (-57 + 57 *c,-280,0);
							}
							else if(reward_num == 4)
							{
								_root.localPosition = new Vector3 (-86 + 57*c ,-280,0);
							}
							_root.gameObject.SetActive(true);
							_icon.transform.parent = _root;
							_icon.transform.localPosition = new Vector3(0,0,0);
							_icon.transform.localScale = new Vector3(1,1,1);
						}
						else
						{
							_root.gameObject.SetActive(false);
						}
					}
				}
				else
				{
					s_t_mission _mission = game_data._instance.get_t_mission(_sub.mission_id);
					
					_item.transform.Find("ti_li").GetComponent<UILabel>().text = _mission.tili.ToString();
					int _count = _mission.day_num - sys._instance.m_self.get_mission_cishu(_sub.mission_id);
					_item.transform.Find("count").GetComponent<UILabel>().text =   string.Format(game_data._instance.get_t_language ("huo_dong_gui.cs_615_90"), _count.ToString());//今日{0}次

					int reward_num = 0;
					for(int c = 0;c < 4;c ++)
					{
						if(_sub.values[c * 4 + 1] != 0)
						{
							reward_num++;
						}
					}
	
					for(int c = 0;c < 4;c ++)
					{
						Transform _root =  _item.transform.Find("icon_" + c);
						sys._instance.remove_child(_root.gameObject);
					}
					for(int c = 0;c < _mission.items.Count;c ++)
					{
						GameObject _icon = icon_manager._instance.create_reward_icon(_mission.items[c].reward.type,_mission.items[c].reward.value1,_mission.items[c].reward.value2,_mission.items[c].reward.value3);
						
						Transform _root =  _item.transform.Find("icon_" + c);
						if(_icon != null)
						{
							if(reward_num == 1)
							{
								_root.localPosition = new Vector3 (0,-280,0);
							}
							else if(reward_num == 2)
							{
								_root.localPosition = new Vector3 (-28.5f + 57*c,-280,0);
							}
							else if(reward_num == 3)
							{
								_root.localPosition = new Vector3 (-57 + 57 *c,-280,0);
							}
							else if(reward_num == 4)
							{
								_root.localPosition = new Vector3 (-86 + 57*c ,-280,0);
							}
							_root.gameObject.SetActive(true);
							_icon.transform.parent = _root;
							_icon.transform.localPosition = new Vector3(0,0,0);
							_icon.transform.localScale = new Vector3(1,1,1);
						}
						else
						{
							_root.gameObject.SetActive(false);
						}
					}
					for(int c = _mission.items.Count;c < 4;c ++)
					{
						Transform _root =  _item.transform.Find("icon_" + c);
						_root.gameObject.SetActive(false);
					}
				}
			}
		}

	}

	public void select_map(int _id)
	{
		error = "";
		for(int j = 0;j < game_data._instance.m_dbc_huodong_sub.get_y();j ++)
		{
			int sub_id = int.Parse(game_data._instance.m_dbc_huodong_sub.get(0,j));
			s_t_huodong_sub _sub = game_data._instance.get_t_huodong_sub(sub_id);
			
			if(_sub.pid == _id)
			{
				if (sys._instance.m_self.get_att(e_player_attr.player_level) < _sub.level)
				{
                    error = "[ffc882]" + string.Format(game_data._instance.get_t_language("huo_dong_gui.cs_893_40"), _sub.level, _sub.name); //"主人,{0}级才开放{1},请努力提升等级吧"
					return;
				}

                if (sys._instance.m_self.zhen_count() < 6 && _sub.level > (int)e_open_level.el_sixth_role)
                {
                    error = "[ffc882]" + string.Format(game_data._instance.get_t_language("huo_dong_gui.cs_900_40")); //"危险,需要上阵6位伙伴才可前往"
                    return;
                }


				if(_sub.mission_id == 0)
				{
					if(_sub.pid == 4)
					{
						int num = 3 - sys._instance.m_self.m_t_player.ore_finish_num;
						if (num <= 0)
						{
							error = game_data._instance.get_t_language ("huo_dong_gui.cs_904_15");//[ffc882]今日次数已用完
							return;
						}
						if(sys._instance.m_self.m_t_player.ore_last_time + 600000 > timer.now())
						{
							error = "[ffc882]" + game_data._instance.get_t_language ("huo_dong_gui.cs_909_28");//正在冷却中
							return;
						}

					}

				}
				else
				{
					s_t_mission _mission = game_data._instance.get_t_mission(_sub.mission_id);
					int _count = _mission.day_num - sys._instance.m_self.get_mission_cishu(_sub.mission_id);
					if (_count <= 0)
					{
						error = game_data._instance.get_t_language ("huo_dong_gui.cs_904_15");//[ffc882]今日次数已用完
						return;
					}
				}
			}
		}
		
	}

   
	public int get_num()
	{
		int num = 0;
		for(int i = 0;i < sys._instance.m_self.m_t_player.zhenxing.Count;++i)
		{
			ccard _card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[i]);
			if(_card == null)
			{
				continue;
			}
			num++;
		}
		return num;
	}

	public int profession_num(int id1, int id2)
	{
		int num = 0;
		for(int i =0;i< sys._instance.m_self.m_t_player.zhenxing.Count;++i)
		{
			ccard _card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[i]);
			if(_card == null)
			{
				continue;
			}
			if(_card.m_skills[0].m_t_skill.id == id1 || _card.m_skills[1].m_t_skill.id == id2 )
			{
				num ++;
			}
		}
		return num;
	}

	void IMessage.net_message(s_net_message message)
	{
		
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "buy_nl_count")
		{
			int item_id = 10010007;
			int num = sys._instance.m_self.get_item_num((uint)item_id);
			if(num > 0)
			{
				root_gui._instance.show_tili_dialog_box(item_id);
				return;
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "buy_num_gui";
				_message.m_ints.Add(100300);
				_message.m_ints.Add(2);
				cmessage_center._instance.add_message(_message);
				return;
			}
		}
		if(message.m_type == "refresh_huo_dong_gui")
		{
			update_ui();
			flag = false;
		}
	}

	void time()
	{

		string ss = "[00ffff]";
		int num = 0;
		bool flag = false;
		for(int i = 0;i < game_data._instance.m_dbc_huodong.get_y() && i < m_huodong_items.Count;i ++)
		{
			int _id = int.Parse(game_data._instance.m_dbc_huodong.get(0,i));
			if(_id == 4)
			{
				num = i;
				flag = true;
				break;
			}
		}
		m_huodong_items[num].transform.Find("ok").gameObject.SetActive(true);
		m_huodong_items[num].transform.Find("suo").gameObject.SetActive(false);
		if (sys._instance.m_self.m_t_player.ore_last_time + 600000 > timer.now())
		{
			m_huodong_items[num].transform.Find("ok").gameObject.SetActive(false);
			m_huodong_items[num].transform.Find("suo").gameObject.SetActive(true);
			ss = "[ff0000]";
		}
		if(flag)
		{
			m_huodong_items[num].transform.Find("ti_li").GetComponent<UILabel>().text = ss + timer.get_time_show((long)(sys._instance.m_self.m_t_player.ore_last_time + 600000 - timer.now()));
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
