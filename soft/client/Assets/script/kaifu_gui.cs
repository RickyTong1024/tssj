
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class kaifu_gui : MonoBehaviour, IMessage {
	// Use this for initialization
	public GameObject m_view;
	private int m_select1 = 1;
	private int m_select2 = 1;
	public GameObject m_first;
	public GameObject m_first1;
	public List<GameObject> m_texts;
	public GameObject m_panel1;
	public GameObject m_panel2;
	public GameObject m_name;
	public GameObject m_buy;
	public GameObject m_yj;
	public GameObject m_xj;
	public GameObject m_pl;
	public GameObject m_icon;
	public GameObject m_time;
	public GameObject m_t1_Label1;
	public GameObject m_t1_Label2;
	public GameObject m_t2_Label1;
	public GameObject m_t2_Label2;
	public GameObject m_t3_Label1;
	public GameObject m_t3_Label2;
	public GameObject m_t4_Label1;
	public GameObject m_t4_Label2;
	public GameObject m_gift;
	public List<GameObject> m_effect1;
	public List<GameObject> m_effect2;

	private protocol.game.smsg_huodong_kaifu_look m_msg;
	private int m_id;

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

		m_select1 = 1;
		m_select2 = 1;

		protocol.game.cmsg_huodong_kaifu_look _msg = new protocol.game.cmsg_huodong_kaifu_look ();
		_msg.stype = 0;
		net_http._instance.send_msg<protocol.game.cmsg_huodong_kaifu_look> (opclient_t.CMSG_HUODONG_KAIFU_LOOK, _msg);
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}

	public void reset()
	{
		for (int i = 1; i <= 4; ++i)
		{
			s_t_kaifu t_kaifu1 = game_data._instance.get_t_kaifu (m_select1, i);
			m_texts[i * 2 - 2].GetComponent<UILabel>().text = t_kaifu1.name;
			m_texts[i * 2 - 1].GetComponent<UILabel>().text = t_kaifu1.name;
		}

		if (m_select2 != 4)
		{
			m_panel1.SetActive(true);
			m_panel2.SetActive(false);
			if(m_view.GetComponent<SpringPanel>() != null)
			{
				m_view.GetComponent<SpringPanel>().enabled = false;
			}
			m_view.transform.localPosition = new Vector3(0, 0, 0);
			m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			sys._instance.remove_child (m_view);

			s_t_kaifu t_kaifu = game_data._instance.get_t_kaifu (m_select1, m_select2);
			int p = 0;
			for(int i = 0;i < t_kaifu.ints.Count;i ++)
			{
				int num = 0;
				for (int j = 0; j < m_msg.ids.Count; ++j)
				{
					if (m_msg.ids[j] == t_kaifu.ints[i])
					{
						num = m_msg.counts[j];
						break;
					}
				}
				int id = t_kaifu.ints[i];
				bool has = false;
				for (int j = 0; j < sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids.Count; ++j)
				{
					if (id == sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids[j])
					{
						has = true;
						break;
					}
				}
				s_t_kaifu_mubiao t_kaifu_mubiao = game_data._instance.get_t_kaifu_mubiao(id);
				if (num < t_kaifu_mubiao.ck || has)
				{
					continue;
				}

				GameObject _obj = game_data._instance.ins_object_res("ui/kaifu_sub");
				
				_obj.transform.parent = m_view.transform;
				_obj.transform.localScale = new Vector3(1,1,1);
				_obj.transform.localPosition = new Vector3(0,121 - p * 146,1);
				_obj.GetComponent<kaifu_sub>().m_id = id;
				_obj.GetComponent<kaifu_sub>().m_num = num;
				_obj.GetComponent<kaifu_sub>().reset();

				sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), p * 0.05f);
				sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, p * 0.05f);
				p++;
			}
			for(int i = 0;i < t_kaifu.ints.Count;i ++)
			{
				int num = 0;
				for (int j = 0; j < m_msg.ids.Count; ++j)
				{
					if (m_msg.ids[j] == t_kaifu.ints[i])
					{
						num = m_msg.counts[j];
						break;
					}
				}
				int id = t_kaifu.ints[i];
				bool has = false;
				for (int j = 0; j < sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids.Count; ++j)
				{
					if (id == sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids[j])
					{
						has = true;
						break;
					}
				}
				s_t_kaifu_mubiao t_kaifu_mubiao = game_data._instance.get_t_kaifu_mubiao(id);
				if (num >= t_kaifu_mubiao.ck && !has)
				{
					continue;
				}
				if(has)
				{
					continue;
				}
				GameObject _obj = game_data._instance.ins_object_res("ui/kaifu_sub");
				
				_obj.transform.parent = m_view.transform;
				_obj.transform.localScale = new Vector3(1,1,1);
				_obj.transform.localPosition = new Vector3(0,121 - p * 146,1);
				_obj.GetComponent<kaifu_sub>().m_id = id;
				_obj.GetComponent<kaifu_sub>().m_num = num;
				_obj.GetComponent<kaifu_sub>().reset();
				
				sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), p * 0.05f);
				sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, p * 0.05f);
				p++;
			}
			for(int i = 0;i < t_kaifu.ints.Count;i ++)
			{
				int num = 0;
				for (int j = 0; j < m_msg.ids.Count; ++j)
				{
					if (m_msg.ids[j] == t_kaifu.ints[i])
					{
						num = m_msg.counts[j];
						break;
					}
				}
				int id = t_kaifu.ints[i];
				bool has = false;
				for (int j = 0; j < sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids.Count; ++j)
				{
					if (id == sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids[j])
					{
						has = true;
						break;
					}
				}
				s_t_kaifu_mubiao t_kaifu_mubiao = game_data._instance.get_t_kaifu_mubiao(id);
				if (num >= t_kaifu_mubiao.ck && !has)
				{
					continue;
				}
				if(num < t_kaifu_mubiao.ck)
				{
					continue;
				}
				GameObject _obj = game_data._instance.ins_object_res("ui/kaifu_sub");
				
				_obj.transform.parent = m_view.transform;
				_obj.transform.localScale = new Vector3(1,1,1);
				_obj.transform.localPosition = new Vector3(0,121 - p * 146,1);
				_obj.GetComponent<kaifu_sub>().m_id = id;
				_obj.GetComponent<kaifu_sub>().m_num = num;
				_obj.GetComponent<kaifu_sub>().reset();
				
				sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), p * 0.05f);
				sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, p * 0.05f);
				p++;
			}
		}
		else
		{
			m_panel1.SetActive(false);
			m_panel2.SetActive(true);

			s_t_kaifu t_kaifu = game_data._instance.get_t_kaifu (m_select1, m_select2);
			sys._instance.remove_child(m_icon);
			GameObject _obj = icon_manager._instance.create_reward_icon(t_kaifu.ints[1], t_kaifu.ints[2], t_kaifu.ints[3], t_kaifu.ints[4]);
			_obj.transform.parent = m_icon.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,0,0);

			int num = 0;
			for (int j = 0; j < m_msg.ids.Count; ++j)
			{
				if (m_msg.ids[j] == t_kaifu.ints[0])
				{
					num = t_kaifu.ints[7] - m_msg.counts[j];
					break;
				}
			}

			if (t_kaifu.ints[1] == 2)
			{
				s_t_item t_item = game_data._instance.get_item(t_kaifu.ints[2]);
				m_name.GetComponent<UILabel>().text = t_item.name;
			}
			else if (t_kaifu.ints[1] == 1)
			{
				if (t_kaifu.ints[2] == 1)
				{
					string text = game_data._instance.get_t_language ("kaifu_gui.cs_242_19");//金币
					m_name.GetComponent<UILabel>().text = text;
				}
				else if (t_kaifu.ints[2] == 5)
				{
					string text = game_data._instance.get_t_language ("kaifu_gui.cs_247_19");//战魂
					m_name.GetComponent<UILabel>().text = text;
				}
				else if (t_kaifu.ints[2] == 7)
				{
					string text = game_data._instance.get_t_language ("kaifu_gui.cs_252_19");//原力
					m_name.GetComponent<UILabel>().text = text;
				}
			}
			m_xj.GetComponent<UILabel>().text = t_kaifu.ints[5].ToString();
			m_yj.GetComponent<UILabel>().text = t_kaifu.ints[6].ToString();
			m_pl.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_gui.cs_260_53"), t_kaifu.ints[7].ToString() , num.ToString());//仅限前{0}人购买([ff0000]剩余{1}[-])

			bool has = false;
			for (int i = 0; i < sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids.Count; ++i)
			{
				if (t_kaifu.ints[0] == sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids[i])
				{
					has = true;
					break;
				}
			}

			if (has || num == 0)
			{
				m_buy.GetComponent<UISprite>().set_enable(false);
				m_buy.GetComponent<BoxCollider>().enabled = false;
			}
			else
			{
				m_buy.GetComponent<UISprite>().set_enable(true);
				m_buy.GetComponent<BoxCollider>().enabled = true;
			}
		}
		reset_buttom ();
	}

	void reset_buttom()
	{
		int kflag = 0;
		int run_day = timer.run_day(sys._instance.m_self.m_t_player.birth_time) + 1;
		if(run_day >= 8)
		{
			run_day = 7;
		}
		for (int i = 1; i <= run_day; ++i)
		{
			bool flag = false;
			for (int j = 1; j <= 3 ;++j)
			{
				bool bflag = false;
				s_t_kaifu t_kaifu = game_data._instance.get_t_kaifu (i, j);
				for(int k = 0;k < t_kaifu.ints.Count;k ++)
				{
					int id = t_kaifu.ints[k];
					bool lflag = false;
					for (int l = 0; l < sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids.Count; ++l)
					{
						if (id == sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids[l])
						{
							lflag = true;
							break;
						}
					}
					if (lflag)
					{
						continue;
					}
					s_t_kaifu_mubiao t_kaifu_mubiao = game_data._instance.get_t_kaifu_mubiao(id);
					int num = 0;
					for (int l = 0; l < m_msg.ids.Count; ++l)
					{
						if (m_msg.ids[l] == id)
						{
							num = m_msg.counts[l];
							break;
						}
					}
					if (num >= t_kaifu_mubiao.ck)
					{
						bflag = true;
						break;
					}
				}
				if (bflag == true)
				{
					if (i == m_select1)
					{
						m_effect2[j - 1].SetActive(true);
					}
					flag = true;
				}
				else
				{
					if (i == m_select1)
					{
						m_effect2[j - 1].SetActive(false);
					}
				}
			}
			{
				s_t_kaifu t_kaifu = game_data._instance.get_t_kaifu (i, 4);
				int id = t_kaifu.ints[0];
				bool bflag = true;
				for (int l = 0; l < sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids.Count; ++l)
				{
					if (id == sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids[l])
					{
						bflag = false;
						break;
					}
				}
				if (bflag == true)
				{
					if (i == m_select1)
					{
						m_effect2[3].SetActive(true);
					}
					flag = true;
				}
				else
				{
					if (i == m_select1)
					{
						m_effect2[3].SetActive(false);
					}
				}
			}
			if (flag)
			{
				kflag = 1;
				m_effect1[i - 1].SetActive(true);
			}
			else
			{
				m_effect1[i - 1].SetActive(false);
			}
		}
		for (int i = run_day + 1; i <= 7; ++i)
		{
			m_effect1[i - 1].SetActive(false);
		}
		sys._instance.m_self.m_can_kaifu = kflag;
	}

	public void click(GameObject obj)
	{
		if(obj.name == "close")
		{
			m_first.GetComponent<UIToggle>().value = true;
			m_first1.GetComponent<UIToggle>().value = true;

			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.name == "buy")
		{
			s_t_kaifu t_kaifu = game_data._instance.get_t_kaifu (m_select1, m_select2);
			m_id = t_kaifu.ints[0];
			string  _des = string.Format(game_data._instance.get_t_language ("kaifu_gui.cs_412_32"), t_kaifu.ints[5]);//是否花费[00ffff]{0}钻石[-]购买半价商品
			s_message _message = new s_message();
			_message.m_type = "buy_banjia";
			_message.m_ints.Add(t_kaifu.ints[5]);
			string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			root_gui._instance.show_select_dialog_box(tishi,_des,_message);
		}
		else
		{
			int run_day = timer.run_day(sys._instance.m_self.m_t_player.birth_time) + 1;
			if (obj.name == "1")
			{
				m_select1 = 1;
			}
			else if (obj.name == "2")
			{
				if (run_day < 2)
				{
					string text = game_data._instance.get_t_language ("kaifu_gui.cs_430_19");//[ffc882]第二天才能查看哦~
					root_gui._instance.show_prompt_dialog_box(text);
					return;
				}
				m_select1 = 2;
			}
			else if (obj.name == "3")
			{
				if (run_day < 3)
				{
					string text1 = game_data._instance.get_t_language ("kaifu_gui.cs_440_20");//[ffc882]第三天才能查看哦~
					root_gui._instance.show_prompt_dialog_box(text1);
					return;
				}
				m_select1 = 3;
			}
			else if (obj.name == "4")
			{
				if (run_day < 4)
				{
					string text2 = game_data._instance.get_t_language ("kaifu_gui.cs_450_20");//[ffc882]第四天才能查看哦~
					root_gui._instance.show_prompt_dialog_box(text2);
					return;
				}
				m_select1 = 4;
			}
			else if (obj.name == "5")
			{
				if (run_day < 5)
				{
					string text3 = game_data._instance.get_t_language ("kaifu_gui.cs_460_20");//[ffc882]第五天才能查看哦~
					root_gui._instance.show_prompt_dialog_box(text3);
					return;
				}
				m_select1 = 5;
			}
			else if (obj.name == "6")
			{
				if (run_day < 6)
				{
					string text4 = game_data._instance.get_t_language ("kaifu_gui.cs_470_20");//[ffc882]第六天才能查看哦~
					root_gui._instance.show_prompt_dialog_box(text4);
					return;
				}
				m_select1 = 6;
			}
			else if (obj.name == "7")
			{
				if (run_day < 7)
				{
					string text5 = game_data._instance.get_t_language ("kaifu_gui.cs_480_20");//[ffc882]第七天才能查看哦~
					root_gui._instance.show_prompt_dialog_box(text5);
					return;
				}
				m_select1 = 7;
			}
			else if (obj.name == "t1")
			{
				m_select2 = 1;
			}
			else if (obj.name == "t2")
			{
				m_select2 = 2;
			}
			else if (obj.name == "t3")
			{
				m_select2 = 3;
			}
			else if (obj.name == "t4")
			{
				m_select2 = 4;
			}
			reset();
		}
	}

	void IMessage.message (s_message message)
	{
		if (message.m_type == "buy_banjia")
		{
			int jewel = (int)message.m_ints[0];
			if (sys._instance.m_self.m_t_player.jewel < jewel)
			{
				root_gui._instance.show_recharge_dialog_box(delegate() {
					m_first.GetComponent<UIToggle>().value = true;
					m_first1.GetComponent<UIToggle>().value = true;
					
					this.transform.Find("frame_big").GetComponent<frame>().hide();
				});
				return ;
			}

			protocol.game.cmsg_huodong_kaifu_reward _msg = new protocol.game.cmsg_huodong_kaifu_reward ();
			_msg.id = m_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_kaifu_reward> (opclient_t.CMSG_HUODONG_KAIFU_REWARD, _msg);
		}
		else if (message.m_type == "kaifu_lj")
		{
			m_id = (int)message.m_ints[0];
			protocol.game.cmsg_huodong_kaifu_reward _msg = new protocol.game.cmsg_huodong_kaifu_reward ();
			_msg.id = m_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_kaifu_reward> (opclient_t.CMSG_HUODONG_KAIFU_REWARD, _msg);
		}
		else if (message.m_type == "kaifu_recharge")
		{
			m_first.GetComponent<UIToggle>().value = true;
			m_first1.GetComponent<UIToggle>().value = true;
			transform.Find("frame_big").GetComponent<frame>().hide();

			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);

			s_message _mes1 = new s_message();
			_mes1.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_mes1);
		}
		if (message.m_type == "daily_refresh") 
		{
			protocol.game.cmsg_huodong_kaifu_look _msg = new protocol.game.cmsg_huodong_kaifu_look ();
			_msg.stype = 0;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_kaifu_look> (opclient_t.CMSG_HUODONG_KAIFU_LOOK, _msg);
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_KAIFU_LOOK && this.gameObject.activeSelf)
		{
			protocol.game.smsg_huodong_kaifu_look _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_kaifu_look> (message.m_byte);
			m_msg = _msg;
			reset ();
		}
		if (message.m_opcode == opclient_t.CMSG_HUODONG_KAIFU_REWARD && this.gameObject.activeSelf)
		{
			protocol.game.smsg_huodong_kaifu_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_kaifu_reward> (message.m_byte);
			sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids.Add(m_id);
			for(int i = 0;i < _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}

			s_t_kaifu_mubiao t_kaifu_mubiao = game_data._instance.get_t_kaifu_mubiao (m_id);
			if (t_kaifu_mubiao.type == 200)
			{
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, t_kaifu_mubiao.def1, game_data._instance.get_t_language ("kaifu_gui.cs_569_94"));//开服活动奖励消耗
				for (int l = 0; l < m_msg.ids.Count; ++l)
				{
					if (m_msg.ids[l] == t_kaifu_mubiao.id)
					{
						m_msg.counts[l]++;
						break;
					}
				}
			}
			for(int i = 0;i < _msg.types.Count;i ++)
			{
				sys._instance.m_self.add_reward(_msg.types[i],_msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("kaifu_gui.cs_581_100"));//开服活动获得
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			for (int i = 0; i < _msg.roles.Count; ++i)
			{
				sys._instance.m_self.add_card(_msg.roles[i]);
			}
			reset ();
		}
	}

	void time()
	{
		long _time = 0;
		int run_day = timer.run_day (sys._instance.m_self.m_t_player.birth_time) + 1;
		if (run_day <= 7)
		{
			_time = (7 - run_day) * 86400000 + timer.last_time_today();
		}
		string _text = timer.get_time_show_ex(_time);
		m_time.GetComponent<UILabel>().text = _text;
		if(_time <= 0)
		{
			m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

}
