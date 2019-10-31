
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class transport_gui : MonoBehaviour,IMessage {

	// Use this for initialization
	public GameObject m_ship_gui;
	public GameObject m_player_gui;
	private Dictionary<ulong, protocol.game.msg_yb_player> m_players = new Dictionary<ulong, protocol.game.msg_yb_player>();
	private Dictionary<ulong, GameObject> m_ships = new Dictionary<ulong, GameObject>();
	private List<protocol.game.msg_yb_info> m_yb_infos = new List<protocol.game.msg_yb_info>();
	private static List<protocol.game.msg_ybq_info> m_ybq_infos = new List<protocol.game.msg_ybq_info>();
	private List<ulong> m_enemys = new List<ulong>();
	public GameObject m_jewel;
	public GameObject m_yuanli;
	public GameObject m_ybq_obj;
	public GameObject m_ybq_num;
	public GameObject m_ybq_obj1;
	public GameObject m_ybq_next_time;
	public GameObject m_yb_num;
	public GameObject m_yb_time;
	public GameObject m_enemy_root;
	public GameObject m_reward;
	public List<GameObject> m_events;
	public GameObject m_text;
	public GameObject m_view;
	public GameObject m_jiasu;
	public GameObject m_finish;
	private int m_player_id;

	void Start () {
		cmessage_center._instance.add_handle (this);

	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		InvokeRepeating("time", 0.0f,1.0f);
		InvokeRepeating("time1", 10.0f,10.0f);
		m_players.Clear();
		m_player_id = 0;
		transport_look ();
	}

	void OnDisable()
	{
		CancelInvoke ("time");
		CancelInvoke ("time1");
	}
	
	void transport_look()
	{
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_YB_LOOK, _msg);
	}

	void transport_look_ex()
	{
		if(sys._instance.m_game_state == "buttle")
		{
			return;
		}

		protocol.game.cmsg_yb_look_ex _msg = new protocol.game.cmsg_yb_look_ex ();
		_msg.player_id = m_player_id;
		net_http._instance.send_msg_ex<protocol.game.cmsg_yb_look_ex> (opclient_t.CMSG_YB_LOOK_EX, _msg);
	}

	void add_player(protocol.game.msg_yb_player yb_player)
	{
		bool flag = false;
		foreach (KeyValuePair<ulong, protocol.game.msg_yb_player> pair in m_players)
		{
			if (yb_player.player_guid == pair.Key)
			{
				flag = true;
				break;
			}
		}
		m_players[yb_player.player_guid] = yb_player;
		if (!flag)
		{
			add_ship(yb_player.player_guid);
		}
		else
		{
			refresh_ship(yb_player.player_guid);
		}
	}

	void remove_all()
	{
		foreach (ulong guid in m_players.Keys)
		{
			remove_ship (guid);
		}
		m_players.Clear ();
	}

	void remove_player(ulong player_guid)
	{
		remove_ship (player_guid);
		m_players.Remove (player_guid);
	}

	void add_ship(ulong player_guid)
	{
		protocol.game.msg_yb_player yb_player = m_players [player_guid];
		s_t_yb _yb = game_data._instance.get_t_yb(yb_player.player_type);
		string _name = "ui/ship/ship_" + _yb.type;
		GameObject _ship = game_data._instance.ins_object_res(_name);
		m_ships[player_guid] = _ship;

		GameObject _plauer_name = game_data._instance.ins_object_res("ui/transport_name");

		_plauer_name.GetComponent<UILabel>().text = "[00ffff]" + yb_player.player_name;
		_plauer_name.GetComponent<ui_3dto2d>().obj = _ship;
		_plauer_name.GetComponent<ui_3dto2d>().m_pos = new Vector3 (-0.65f, 1.5f, 0);
		_plauer_name.GetComponent<transport_name>().m_guid = player_guid;

        if (sys._instance.m_self.m_t_player.nalflag != 0)
        {
            if (platform_config_common.m_nationality > 0)
            {

                _plauer_name.transform.Find("gq").gameObject.SetActive(true);
                _plauer_name.transform.Find("gq").GetComponent<UISprite>().spriteName = sys._instance.returnfirstSpritename(yb_player.player_nalflag);


            }
            else
            {
                _plauer_name.transform.Find("gq").gameObject.SetActive(false);
            }
        }

        if (player_guid == sys._instance.m_self.m_guid)
		{
			_plauer_name.GetComponent<UILabel>().text = "[ffff00]" + yb_player.player_name;
			_plauer_name.transform.GetChild(0).gameObject.SetActive(false);
			_plauer_name.GetComponent<BoxCollider>().enabled = false;
		}

		_plauer_name.transform.parent = root_gui._instance.m_ui_bottomleft_1.transform;
		_plauer_name.transform.localScale = new Vector3(1, 1, 1);
		_plauer_name.SetActive (true);

		if(_ship != null)
		{
			float _z = Random.Range(10,-10);

			_ship.transform.localPosition = new Vector3(0, 0, _z);
			_ship.transform.localScale = new Vector3(1, 1, 1);

			float _pro = 0.0f;
			if (yb_player.jiasu_time > 0)
			{
				_pro = (float)(yb_player.jiasu_time - yb_player.start_time + (timer.now () - yb_player.jiasu_time) * 2) / (float)_yb.time;
			}
			else
			{
				_pro = ((float)timer.now () - (float)yb_player.start_time) / (float)_yb.time;
			}

			float _time = (1 - _pro) * (float)_yb.time / 1000.0f;

			TweenPosition _effect = _ship.AddComponent<TweenPosition>();

			_effect.duration = _time;
			_effect.method = UITweener.Method.Linear;
			_effect.from = new Vector3(100.0f * _pro, 0, _z);
			_effect.to = new Vector3(100.0f, 0, _z);
		}
	}

	void refresh_ship(ulong player_guid)
	{
		if(m_ships.ContainsKey(player_guid) == false)
		{
			return;
		}

		protocol.game.msg_yb_player yb_player = m_players [player_guid];
		s_t_yb _yb = game_data._instance.get_t_yb(yb_player.player_type);
		GameObject _ship = m_ships[player_guid];

		if(_ship != null)
		{
			float _z = _ship.transform.localPosition.z;

			float _pro = 0.0f;
			if (yb_player.jiasu_time > 0)
			{
				_pro = (float)(yb_player.jiasu_time - yb_player.start_time + (timer.now () - yb_player.jiasu_time) * 2) / (float)_yb.time;
			}
			else
			{
				_pro = ((float)timer.now () - (float)yb_player.start_time) / (float)_yb.time;
			}
			
			float _time = (1 - _pro) * (float)_yb.time / 1000.0f;

			TweenPosition _effect = _ship.GetComponent<TweenPosition>();
			
			_effect.duration = _time;
			//_effect.method = UITweener.Method.Linear;
			//_effect.from = new Vector3(100.0f * _pro, 0, _z);
			//_effect.to = new Vector3(100.0f, 0, _z);
		}
	}

	void remove_ship(ulong player_guid)
	{
		if(m_ships.ContainsKey(player_guid) == false)
		{
			return;
		}

		GameObject _ship = m_ships[player_guid];

		GameObject.Destroy (_ship);

		m_ships.Remove (player_guid);
	}

	void reset()
	{
		for (int i = 0; i < m_ybq_infos.Count; ++i)
		{
			m_events[i].SetActive(true);
		}
		for (int i = m_ybq_infos.Count; i < m_events.Count; ++i)
		{
			m_events[i].SetActive(false);
		}
		
		string s = "";
		for (int i = 0; i < m_yb_infos.Count; ++i)
		{
			if (i != 0)
			{
				s += "\n";
			}
			protocol.game.msg_yb_info yb_info = m_yb_infos[i];
			s_t_yb t_yb = game_data._instance.get_t_yb(yb_info.yb_type);
			if (yb_info.type == 0)
			{
				string text = game_data._instance.get_t_language ("transport_gui.cs_238_18") + " ";//运气极佳，开始运输
				string text1 = " " + game_data._instance.get_t_language ("transport_gui.cs_239_19");//货船
				s += "[00ff00]" + yb_info.player_name + "[-]  " + text + t_yb.name + text1;
			}
			else if (yb_info.type == 1)
			{

				s += string.Format(game_data._instance.get_t_language ("[00ff00]{0}[-]  运输{1}货船成功") , yb_info.player_name , t_yb.name );
			}
			else if (yb_info.type == 2)
			{

				s += string.Format(game_data._instance.get_t_language ("{0}  成功拦截了{1}[00ff00]{2}货船[-]，抢到了[00ffff]原力x{3}[-]") , yb_info.player_name ,  yb_info.target_name , t_yb.name ,yb_info.yuanli);
				/*if (yb_info.num1 > 0)[]
				{
					_des += "、[5cf732]"+ game_data._instance.get_t_language ("transport_gui.cs_253_26") +"x" + yb_info.num1 + "[-]";//绿色技能钥匙
				}
				if (yb_info.num2 > 0)
				{
					_des += "、[32eef7]"+ game_data._instance.get_t_language ("transport_gui.cs_257_26") +"x" + yb_info.num2 + "[-]";//蓝色技能钥匙
				}
				if (yb_info.num3 > 0)
				{
					_des += "、[ff3fbf]"+game_data._instance.get_t_language ("transport_gui.cs_261_25") +"x" + yb_info.num3 + "[-]";//紫色技能钥匙
				}*/
				
			}
		}
		m_text.GetComponent<UILabel>().text = s;
		m_view.GetComponent<UIScrollView>().ResetPosition ();
		sys._instance.remove_child (m_enemy_root);
		int tnum = 0;
		for (int i = 0; i < m_enemys.Count; ++i)
		{
			ulong enemy = m_enemys[i];
			bool flag = false;
			string name = "";
			foreach (KeyValuePair<ulong, protocol.game.msg_yb_player> pair in m_players)
			{
				if (enemy == pair.Key)
				{
					name = pair.Value.player_name;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				GameObject target = game_data._instance.ins_object_res("ui/transport_enemy_item");
				target.transform.parent = m_enemy_root.transform;
				target.transform.localPosition = new Vector3(-15, -15 - tnum * 35,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.transform.GetComponent<transport_enemy_item>().m_guid = enemy;
				target.transform.GetComponent<transport_enemy_item>().m_name.GetComponent<UILabel>().text = name;
				tnum++;
			}
		}
		check_effect ();
	}

	void time()
	{
		
		m_yb_num.GetComponent<UILabel>().text = 3 - sys._instance.m_self.m_t_player.yb_finish_num + game_data._instance.get_t_language ("guild_buttle_gui.cs_55_160");//次
		long t = (long)sys._instance.m_self.m_t_player.ybq_last_time + 600000 - (long)timer.now();
		if (t > 0)
		{
			m_ybq_obj.SetActive(false);
			m_ybq_obj1.SetActive(true);
			m_ybq_next_time.GetComponent<UILabel>().text = timer.get_time_show(t);
		}
		else
		{
			m_ybq_obj1.SetActive(false);
			m_ybq_obj.SetActive(true);
			m_ybq_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),5 - sys._instance.m_self.m_t_player.ybq_finish_num);//{0}次
		}
		if (!is_start_yb())
		{
			m_yb_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("transport_gui.cs_317_45");//未出发
			m_jiasu.SetActive(false);
			m_finish.SetActive(false);
		}
		else
		{
			t = (long)get_yb_finish_time() - (long)timer.now();
			m_yb_time.GetComponent<UILabel>().text = timer.get_time_show(t);
			m_jiasu.SetActive(true);
			m_finish.SetActive(true);
		}
		if (!is_start_yb() || !is_finish_yb())
		{
			m_reward.SetActive(false);
		}
		else
		{
			m_reward.SetActive(true);
		}

		List<ulong> rk = new List<ulong>();
		foreach (KeyValuePair<ulong, protocol.game.msg_yb_player> pair in m_players)
		{
			protocol.game.msg_yb_player yp = pair.Value;
			long ltime = 0;
			s_t_yb t_yb = game_data._instance.get_t_yb (yp.player_type);
			if (yp.jiasu_time > 0)
			{
				ltime = ((long)t_yb.time - (long)(yp.jiasu_time - yp.start_time)) / 2 - (long)(timer.now() - yp.jiasu_time);
			}
			ltime = (long)t_yb.time - (long)(timer.now () - yp.start_time);
			if (ltime <= 0)
			{
				rk.Add(pair.Key);
			}
		}
		for (int i = 0; i < rk.Count; ++i)
		{
			remove_player(rk[i]);
		}
		check_effect ();
	}
	
	void time1()
	{
		transport_look_ex ();
	}

	void click(GameObject obj)
	{
		if(obj.transform.name == "escort")
		{
			m_ship_gui.SetActive(true);
		}
		if(obj.name == "close")
		{
			s_message _message = new s_message();
			_message.m_type = "show_huo_dong";
			_message.m_ints.Add(2);
			_message.m_bools.Add(false);
			cmessage_center._instance.add_message(_message);
			
			sys._instance.m_game_state = "hall";
			sys._instance.load_scene(sys._instance.m_hall_name);
			
			Object.Destroy(this.gameObject);
		}
		if(obj.name == "reward")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();		
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_YB_REWARD, _msg);
		}
		if(obj.name == "jiasu")
		{
			if (!is_start_yb())
			{
				string s = game_data._instance.get_t_language ("transport_gui.cs_393_15");//[ffc882]尚未开始运输
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (is_jiasu_yb())
			{
				string s = game_data._instance.get_t_language ("transport_gui.cs_399_15");//[ffc882]已经启用加速了
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (is_finish_yb())
			{
				string s = game_data._instance.get_t_language ("transport_gui.cs_405_15");//[ffc882]已经完成运输
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			string text = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			s_message _message = new s_message();
			_message.m_type = "transport_ship_jiasu";
			string  _des = game_data._instance.get_t_language ("transport_gui.cs_412_18");//是否花费[00ffff]40钻石[-]加速[00ff00](运输时间缩短一半)[-]
			root_gui._instance.show_select_dialog_box(text,_des,_message);
		}
		if(obj.name == "finish")
		{
			if (!is_start_yb())
			{
				string s = game_data._instance.get_t_language ("transport_gui.cs_393_15");//[ffc882]尚未开始运输
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (is_finish_yb())
			{
				string s = game_data._instance.get_t_language ("transport_gui.cs_405_15");//[ffc882]已经完成运输
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			string text = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			s_message _message = new s_message();
			_message.m_type = "transport_ship_finish";
			string  _des = game_data._instance.get_t_language ("transport_gui.cs_432_18");//是否花费[00ffff]250钻石[-]完成运输
			root_gui._instance.show_select_dialog_box(text,_des,_message);
		}
		if (obj.name.Length == 5 && obj.name.Substring(0, 3) == "tip")
		{
			int index = int.Parse(obj.name.Substring(4, 1));
			if (index < 0 || index >= m_ybq_infos.Count)
			{
				return;
			}
			protocol.game.msg_ybq_info myi = m_ybq_infos[index];
			string s = "";
			if (myi.yuanli == 0)
			{
				s = string.Format(game_data._instance.get_t_language ("transport_gui.cs_446_22") , myi.player_name );//[00ff00]{0}[-]拦截你的货船失败
			}
			else
			{
                s = string.Format(game_data._instance.get_t_language ("transport_gui.cs_450_34"), myi.player_name, myi.yuanli);//[00ff00]{0}[-]拦截你的货船成功，抢到了[00ffff]原力x{1}[-]
				
				/*if (myi.num1 > 0)
				{
					_des += "、[5cf732]"+ game_data._instance.get_t_language ("transport_gui.cs_253_26") + "x" + myi.num1 + "[-]";//绿色技能钥匙
				}
				if (myi.num2 > 0)
				{
					_des += "、[32eef7]" + game_data._instance.get_t_language ("transport_gui.cs_257_26") + "x" + myi.num2 + "[-]";//蓝色技能钥匙
				}
				if (myi.num3 > 0)
				{
					_des += "、[ff3fbf]" + game_data._instance.get_t_language ("transport_gui.cs_261_25") +"x" + myi.num3 + "[-]";//紫色技能钥匙
				}*/
				
			}
			string text = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			root_gui._instance.show_single_dialog_box(text, s, new s_message());

			m_ybq_infos.RemoveAt(index);
			reset();
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_YB_LOOK || message.m_opcode == opclient_t.CMSG_YB_LOOK_EX)
		{
			protocol.game.smsg_yb_look _msg = net_http._instance.parse_packet<protocol.game.smsg_yb_look> (message.m_byte);

			for(int i = 0; i < _msg.players.Count;i ++)
			{
				protocol.game.msg_yb_player _player = _msg.players[i];
				add_player(_player);
			}
			for(int i = 0; i < _msg.yb_infos.Count;i ++)
			{
				bool flag = true;
				for (int j = 0; j < m_yb_infos.Count; ++j)
				{
					if (m_yb_infos[j].info_id == _msg.yb_infos[i].info_id)
					{
						flag = false;
						break;
					}
				}
				if (!flag)
				{
					continue;
				}
				m_yb_infos.Add (_msg.yb_infos[i]);
				
				if (m_yb_infos.Count > 10)
				{
					m_yb_infos.RemoveAt(0);
				}
			}
			for(int i = 0; i < _msg.ybq_infos.Count;i ++)
			{
				m_ybq_infos.Add(_msg.ybq_infos[i]);
				if (m_ybq_infos.Count > 5)
				{
					m_ybq_infos.RemoveAt(0);
				}
			}
			m_enemys.Clear();
			for(int i = 0; i < _msg.ybq_guids.Count;i ++)
			{
				m_enemys.Add(_msg.ybq_guids[i]);
			}
			m_player_id = _msg.player_id;
			reset ();
			time ();
		}
		else if(message.m_opcode == opclient_t.CMSG_YB_REWARD)
		{
			protocol.game.smsg_yb_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_yb_reward> (message.m_byte);

			sys._instance.m_self.m_t_player.yb_type = 0;
			sys._instance.m_self.m_t_player.yb_level = 0;
			sys._instance.m_self.m_t_player.yb_start_time = 0;
			sys._instance.m_self.m_t_player.yb_jiasu_time = 0;
			sys._instance.m_self.m_t_player.yb_gw_type = 0;
			sys._instance.m_self.m_t_player.yb_byb_num = 0;
			sys._instance.m_self.m_t_player.yb_per = 0;

			sys._instance.m_self.add_att(e_player_attr.player_yuanli, _msg.yuanli);
			/*for (int i = 0; i < _msg.item_nums.Count; ++i)
			{
				sys._instance.m_self.add_item((uint)(60010001 + i), _msg.item_nums[i]);
			}*/
			sys._instance.m_self.add_active(1300, 1);
			time ();
		}
		else if(message.m_opcode == opclient_t.CMSG_YB_JIASU)
		{
			sys._instance.m_self.m_t_player.yb_jiasu_time = timer.now();
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, 40,game_data._instance.get_t_language ("transport_gui.cs_547_63"));//运输消耗

			if (m_players.ContainsKey(sys._instance.m_self.m_guid))
			{
				protocol.game.msg_yb_player _player = m_players[sys._instance.m_self.m_guid];
				_player.jiasu_time = timer.now();
				refresh_ship(sys._instance.m_self.m_guid);
			}
			time ();
		}
		else if(message.m_opcode == opclient_t.CMSG_YB_FINISH)
		{
			protocol.game.smsg_yb_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_yb_reward> (message.m_byte);
			
			sys._instance.m_self.m_t_player.yb_type = 0;
			sys._instance.m_self.m_t_player.yb_level = 0;
			sys._instance.m_self.m_t_player.yb_start_time = 0;
			sys._instance.m_self.m_t_player.yb_jiasu_time = 0;
			sys._instance.m_self.m_t_player.yb_gw_type = 0;
			sys._instance.m_self.m_t_player.yb_byb_num = 0;
			sys._instance.m_self.m_t_player.yb_per = 0;

			sys._instance.m_self.add_att(e_player_attr.player_yuanli, _msg.yuanli);

            sys._instance.m_self.sub_att(e_player_attr.player_jewel, 250, game_data._instance.get_t_language ("transport_gui.cs_571_74"));//运输结束消耗
			sys._instance.m_self.add_active(1300, 1);

			remove_player(sys._instance.m_self.m_guid);
			time ();
		}
		else if(message.m_opcode == opclient_t.CMSG_YB_YBQ_FIGHT_END)
		{
			remove_all();
			protocol.game.smsg_yb_ybq_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_yb_ybq_fight_end> (message.m_byte);

			sys._instance.m_game_state = "buttle";

			battle_logic_ex._instance.set_transport_end(_msg);
			
			sys._instance.load_scene_ex("ts_fight_mijing");
		}
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "transport_ship")
		{
			ulong _select_guid = (ulong)message.m_long[0];
			if (!m_players.ContainsKey(_select_guid))
			{
				return;
			}
			m_player_gui.GetComponent<transport_player_gui>().m_yb_player = m_players[_select_guid];
			m_player_gui.SetActive(true);
		}

		if (message.m_type == "transport_add_self")
		{
			protocol.game.msg_yb_player _player = new protocol.game.msg_yb_player();
			_player.player_guid = sys._instance.m_self.m_t_player.guid;
			_player.player_name = sys._instance.m_self.m_t_player.name;
			_player.player_level = sys._instance.m_self.m_t_player.level;
			_player.player_bf = sys._instance.m_self.m_t_player.bf;
			_player.player_type = sys._instance.m_self.m_t_player.yb_type;
			_player.start_time = sys._instance.m_self.m_t_player.yb_start_time;
			_player.jiasu_time = sys._instance.m_self.m_t_player.yb_jiasu_time;
			_player.player_ybq_num = 0;
			add_player(_player);
		}

		if (message.m_type == "transport_ship_jiasu")
		{
			if (sys._instance.m_self.m_t_player.jewel < 40)
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_YB_JIASU, _msg);
		}
		if (message.m_type == "transport_ship_finish")
		{
			if (sys._instance.m_self.m_t_player.jewel < 250)
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();		
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_YB_FINISH, _msg);
		}
		if (message.m_type == "transport_enemy_item")
		{
			ulong guid = (ulong)message.m_long[0];
			if (m_players.ContainsKey(guid))
			{
				m_player_gui.GetComponent<transport_player_gui>().m_yb_player = m_players[guid];
				m_player_gui.SetActive(true);
			}
		}
	}

	public static ulong get_yb_finish_time()
	{
		int type = sys._instance.m_self.m_t_player.yb_type;
		s_t_yb t_yb = game_data._instance.get_t_yb (type);
		if (sys._instance.m_self.m_t_player.yb_jiasu_time > 0)
		{
			return ((ulong)t_yb.time - (sys._instance.m_self.m_t_player.yb_jiasu_time - sys._instance.m_self.m_t_player.yb_start_time)) / 2 + sys._instance.m_self.m_t_player.yb_jiasu_time;
		}
		return sys._instance.m_self.m_t_player.yb_start_time + (ulong)t_yb.time;
	}

	public static bool is_start_yb()
	{
		return sys._instance.m_self.m_t_player.yb_start_time > 0;
	}
	
	public static bool is_finish_yb()
	{
		return get_yb_finish_time() < timer.now();
	}
	
	public static bool is_jiasu_yb()
	{
		return sys._instance.m_self.m_t_player.yb_jiasu_time > 0;
	}

	void check_effect()
	{
		if (huo_dong_gui.m_yb_effect == 1)
		{
			if (is_start_yb())
			{
				if (is_finish_yb())
				{
					return;
				}
			}
			if (m_ybq_infos.Count > 0)
			{
				return;
			}
			huo_dong_gui.m_yb_effect = 0;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
