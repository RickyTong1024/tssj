
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explore_gui : MonoBehaviour,IMessage{

	public static explore_gui _instance;
	public GameObject m_rank_gui;
	public GameObject m_wuping_gui;
	public GameObject m_huodong_time;
	public GameObject m_free_num;
	public GameObject m_one_jewel;
	public GameObject m_ten_jewel;
	public GameObject m_norank;
	public GameObject m_rank;
	public GameObject m_wuping_icon;
	public GameObject m_ten_reward_gui;
	public GameObject m_task_gui;
	public GameObject m_ten_pt_item;
	public GameObject m_ten_pt_scro;
	public GameObject m_ten_qiyu_item;
	public GameObject m_ten_qiyu_scro;
	public GameObject m_qiyu_gui;
	public GameObject m_reward_gui;
	public GameObject m_score;
	public GameObject m_task_point;
	public GameObject m_info_gui;
	public GameObject m_info_scro;
	public GameObject m_one;
	public GameObject m_ten;
	public GameObject m_daren_rank_gui;

	public GameObject m_qiyu;
	public GameObject m_rank_button;
	public GameObject m_reward_button;
	public GameObject m_yulan_button;
	public GameObject m_close;
	public GameObject m_sm_button;

	public protocol.game.smsg_huodong_tansuo_view  _msg;
	public List<int> rewards = new List<int>();
	public int score = 0;
	public GameObject m_qiyu_effect;

	public List<GameObject> m_icons = new List<GameObject>();
	public List<GameObject> m_names = new List<GameObject>();
	public List<GameObject> m_scores = new List<GameObject>();
	public List<GameObject> m_chenghaos = new List<GameObject>();

	public UILabel m_wuping_name;
	public UILabel m_wuping_desc;
	public List<protocol.game.tansuo_event> _msg_event = new List<protocol.game.tansuo_event>();
	private List<protocol.game.tansuo_event> _pt_event = new List<protocol.game.tansuo_event>();
	private List<protocol.game.tansuo_event> _qiyu_event = new List<protocol.game.tansuo_event>();
	public int m_select = 0;
	private ulong m_end_time;
	private int rank = 0;
	public int m_num = 0;
	public bool have_huodong = true;
	public bool is_press = false;
	// Use this for initialization
	void Start () 
	{
		is_press = true;
		cmessage_center._instance.add_handle (this);
		m_qiyu_effect.SetActive (false);
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_MANYOU_VIEW, _msg);
	}

	void Awake()
	{
		_instance = this;
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	
	void OnDisable()
	{
		CancelInvoke ("time");
	}
	
	void time()
	{
		long _time = (long)(m_end_time - timer.now());
		m_huodong_time.GetComponent<UILabel>().text = timer.get_time_show_ex(_time);
		if(_time <= 0)
		{
			have_huodong = false;
			m_huodong_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
			reset();
			CancelInvoke ("time");
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
			sys._instance.m_game_state = "hall";
			sys._instance.load_scene(sys._instance.m_hall_name);
			this.gameObject.SetActive(false);
		}
		else if(obj.transform.name == "one")
		{
			if(m_num >= 5)
			{
				s_t_price t_price = game_data._instance.get_t_price((m_num -5) +1);
				if(sys._instance.m_self.m_t_player.jewel < t_price.manyou)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
					return;
				}
			}
			m_select = 0;
			int num = Random.Range(0,3);
			s_message _message = new s_message();
			_message.m_type = "tansuo_move";
			_message.m_ints.Add(num);
			cmessage_center._instance.add_message(_message);
		}
		else if(obj.transform.name == "ten")
		{
			int price = 0;
			int _num = 0;
			if(m_num <= 5)
			{
				_num = 5- m_num;
				for(int i = 1; i <= 10 - _num;++i)
				{
					s_t_price t_price = game_data._instance.get_t_price(i);
					price += t_price.manyou;
				}
			}
			else
			{
				_num = m_num - 5;
				for(int i = 1; i <= 10;++i)
				{
					s_t_price t_price = game_data._instance.get_t_price(i + _num);
					price += t_price.manyou;
				}
			}
			if(sys._instance.m_self.m_t_player.jewel < price)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			m_select = 1;
			int num = Random.Range(0,3);
			s_message _message = new s_message();
			_message.m_type = "tansuo_move";
			_message.m_ints.Add(num);
			cmessage_center._instance.add_message(_message);
		}
		else if(obj.transform.name == "rank")
		{
			is_press = false;
			m_rank_gui.SetActive(true);
		}
		else if(obj.transform.name == "yulan")
		{
			is_press = false;
			m_reward_gui.GetComponent<explore_reward_gui>().type = 0;
			m_reward_gui.SetActive(true);
		}
		else if(obj.transform.name == "yulan_close")
		{
			if(!m_qiyu_gui.activeSelf)
			{
				is_press = true;
			}
			m_reward_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.transform.name == "qiyu")
		{
			m_qiyu_effect.SetActive(false);
			end_qiyu();
			is_press = false;
			m_qiyu_gui.SetActive(true);
			m_qiyu_gui.GetComponent<explore_qiyu_gui>().reset();
		}
		else if(obj.transform.name == "hide")
		{
			is_press = true;
			m_ten_reward_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.transform.name == "reward")
		{
			is_press = false;
			m_task_gui.SetActive(true);
		}
		else if(obj.transform.name == "sm")
		{
			is_press = false;
			if(m_info_scro.transform.GetComponent<SpringPanel>() != null)
			{
				m_info_scro.transform.GetComponent<SpringPanel>().enabled = false;
			}
			m_info_scro.transform.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
			m_info_scro.transform.localPosition = new Vector3 (0, 0, 0);
			m_info_gui.SetActive(true);
		}
		else if(obj.transform.name == "sm_close")
		{
			is_press = true;
			m_info_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	public void wuping_close(GameObject obj)
	{
		if(!m_qiyu_gui.activeSelf)
		{
			is_press = true;
		}
		m_wuping_gui.GetComponent<ui_show_anim>().hide_ui();
	}

	void skip(GameObject obj)
	{
		end_qiyu();
		m_qiyu_effect.SetActive (false);
		is_press = false;
		m_qiyu_gui.GetComponent<explore_qiyu_gui>().qiyu_id = int.Parse (obj.transform.name);
		m_qiyu_gui.GetComponent<explore_qiyu_gui>().reset_ex ();
		m_qiyu_gui.SetActive(true);
		m_ten_reward_gui.transform.Find("frame_big").GetComponent<frame>().hide();
	}

	public void button(bool flag)
	{
		m_qiyu.GetComponent<BoxCollider>().enabled = flag;
		m_rank_button.GetComponent<BoxCollider>().enabled = flag;
		m_one.GetComponent<BoxCollider>().enabled = flag;
		m_ten.GetComponent<BoxCollider>().enabled = flag;
		m_yulan_button.GetComponent<BoxCollider>().enabled = flag;
		m_reward_button.GetComponent<BoxCollider>().enabled = flag;
		m_sm_button.GetComponent<BoxCollider>().enabled = flag;
		m_close.GetComponent<BoxCollider>().enabled = flag;
	}

	public void move_finish()
	{
		protocol.game.cmsg_huodong_tansuo _msg = new protocol.game.cmsg_huodong_tansuo ();
		_msg.type = m_select;
		net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo> (opclient_t.CMSG_HUODONG_MANYOU, _msg);
	}

	void daren_rank()
	{
		for(int i = 0; i < _msg.names.Count;++i)
		{
			sys._instance.remove_child(m_icons[i]);
			m_icons[i].transform.parent.gameObject.SetActive(true);
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)_msg.ids[i], _msg.achieves[i], _msg.vips[i],_msg.nalflags[i]);
			_obj1.transform.parent = m_icons[i].transform;
			_obj1.transform.localScale = new Vector3(1, 1, 1);
			_obj1.transform.localPosition = new Vector3(0, 0, 0);
			m_names[i].GetComponent<UILabel>().text = game_data._instance.get_name_color(_msg.achieves[i]) + _msg.names[i].ToString();
			m_scores[i].GetComponent<UILabel>().text = game_data._instance.get_t_language ("explore_gui.cs_268_46") + _msg.points[i];//积分:
			sys._instance.get_chenghao(_msg.chenghaos[i],m_chenghaos[i]);
		}
		for(int i = _msg.names.Count ; i < m_icons.Count;++i)
		{
			m_icons[i].transform.parent.gameObject.SetActive(false);
			m_names[i].GetComponent<UILabel>().text = "";
			m_scores[i].GetComponent<UILabel>().text = "";
			sys._instance.get_chenghao(0,m_chenghaos[i]);
		}
	}

	public void reset(bool flag = false)
	{
		m_score.GetComponent<UILabel>().text = score.ToString ();
		long _time = (long)(m_end_time - timer.now());
		if(_time <= 0)
		{
			have_huodong = false;
			m_one.SetActive (false);
			m_ten.SetActive (false);
			m_free_num.SetActive(false);
			m_one_jewel.SetActive(false);
			m_ten_jewel.SetActive(false);
			m_qiyu.SetActive(false);
			daren_rank();
			m_daren_rank_gui.SetActive(true);
		}
		else
		{
			m_one.SetActive (true);
			m_ten.SetActive (true);
			m_one_jewel.SetActive(true);
			m_ten_jewel.SetActive(true);
			m_daren_rank_gui.SetActive(false);
			have_huodong = true;
			if(flag)
			{
				s_message _message = new s_message();
				_message.m_type = "explore_create_floor";
				cmessage_center._instance.add_message(_message);
			}
		}
		if(_msg_event.Count > 0 && _time > 0)
		{
			m_qiyu.SetActive(true);
		}
		else
		{
			m_qiyu.SetActive(false);
		}
		if(m_num < 5)
		{
			m_free_num.SetActive(true);
			m_one_jewel.SetActive(false);
			m_free_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("explore_gui.cs_323_59"), (5 - m_num));//免费{0}次
		}
		else
		{
			string color = "[ff0000]";
			s_t_price t_price = game_data._instance.get_t_price((m_num -5) +1);
			if(sys._instance.m_self.m_t_player.jewel >= t_price.manyou)
			{
				color = sys._instance.get_res_color(2);
			}
			m_free_num.SetActive(false);
			m_one_jewel.SetActive(true);
			m_one_jewel.transform.Find("one").GetComponent<UILabel>().text = color + "x" + t_price.manyou;
		}
		if(_time <= 0)
		{
			m_free_num.SetActive(false);
			m_one_jewel.SetActive(false);
		}
		int price = 0;
		int _num = 0;
		if(m_num <= 5)
		{
			_num = 5- m_num;
			for(int i = 1; i <= 10 - _num;++i)
			{
				s_t_price t_price = game_data._instance.get_t_price(i);
				price += t_price.manyou;
			}
		}
		else
		{
			_num = m_num - 5;
			for(int i = 1; i <= 10;++i)
			{
				s_t_price t_price = game_data._instance.get_t_price(i + _num);
				price += t_price.manyou;
			}
		}
		string color1 = "[ff0000]";
		if(sys._instance.m_self.m_t_player.jewel >= price)
		{
			color1 = sys._instance.get_res_color(2);
		}
		m_ten_jewel.transform.Find("ten").GetComponent<UILabel>().text = color1 + "x" +price;
		if(rank == 0)
		{
			m_norank.SetActive(true);
			m_rank.SetActive(false);
		}
		else
		{
			m_norank.SetActive(false);
			m_rank.SetActive(true);
			m_rank.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), rank.ToString());//第{0}名
		}
		if(explore_task_gui.effect())
		{
			m_task_point.SetActive(true);
		}
		else
		{
			m_task_point.SetActive(false);
		}
	}

	void end_qiyu()
	{
		for(int i = 0; i < _msg_event.Count;)
		{
			long _time = (long)(_msg_event[i].qiyu_time - timer.now());
			if(_time <= 0)
			{
				_msg_event.RemoveAt(i);
				continue;
			}
			i++;
		}
		if(_msg_event.Count <= 0)
		{
			root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_403_45"));//[ffc882]奇遇中所有活动时间已到
			m_qiyu.SetActive(false);
			return;
		}
	}

	void wuping(s_t_reward t_reward,string desc,bool is_xy)
	{
		is_press = false;
		sys._instance.remove_child(m_wuping_icon);
		GameObject obj = icon_manager._instance.create_reward_icon(t_reward.type,t_reward.value1,t_reward.value2,t_reward.value3);
		obj.transform.parent = m_wuping_icon.transform;
		obj.transform.localPosition = new Vector3(0,0,0);
		obj.transform.localScale = new Vector3(1,1,1);
		if(is_xy)
		{
			m_wuping_gui.transform.Find("effect").gameObject.SetActive(true);
		}
		else
		{
			m_wuping_gui.transform.Find("effect").gameObject.SetActive(false);
		}
		m_wuping_name.text = sys._instance.m_self.get_name(t_reward.type,t_reward.value1,t_reward.value2,t_reward.value3);
		m_wuping_desc.text = desc;
		m_wuping_gui.SetActive(true);
	}
	
	void reward()
	{
		if(m_select == 0)
		{
			if(_pt_event.Count == 1)
			{
				bool flag = false;
				s_t_manyou t_manyou = game_data._instance.get_t_manyou(_pt_event[0].ts_id);
				if(t_manyou.type == 1)
				{
					flag = true;
				}
                wuping(t_manyou.reward, game_data._instance.get_t_language("explore_gui.cs_442_27"), flag);
			}
			else if(_qiyu_event.Count == 1)
			{
				end_qiyu();
				is_press = false;
				m_qiyu_gui.GetComponent<explore_qiyu_gui>().qiyu_id = _qiyu_event[0].id;
				m_qiyu_gui.GetComponent<explore_qiyu_gui>().reset_ex ();
				m_qiyu_gui.SetActive(true);
			}
		}
		if(m_select == 1)
		{
			if(m_ten_pt_scro.GetComponent<SpringPanel>() != null)
			{
				m_ten_pt_scro.GetComponent<SpringPanel>().enabled = false;
			}
			m_ten_pt_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0,0);
			m_ten_pt_scro.transform.localPosition = new Vector3(0,0,0);

			if(m_ten_qiyu_scro.GetComponent<SpringPanel>() != null)
			{
				m_ten_qiyu_scro.GetComponent<SpringPanel>().enabled = false;
			}
			m_ten_qiyu_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0,0);
			m_ten_qiyu_scro.transform.localPosition = new Vector3(0,0,0);

			sys._instance.remove_child(m_ten_pt_scro);
			sys._instance.remove_child(m_ten_qiyu_scro);
			if(_pt_event.Count > 0)
			{
				_pt_event.Sort(comp);
				for(int i = 0; i < _pt_event.Count;++i)
				{
					s_t_manyou t_manyou = game_data._instance.get_t_manyou(_pt_event[i].ts_id);
					GameObject item = (GameObject)Object.Instantiate(m_ten_pt_item);
					item.transform.parent = m_ten_pt_scro.transform;
					item.transform.localPosition = new Vector3(0,121- 83*i,0);
					item.transform.localScale = new Vector3(1,1,1);

					if(t_manyou.type == 1)
					{
						item.transform.Find("effect").gameObject.SetActive(true);
					}
					else
					{
						item.transform.Find("effect").gameObject.SetActive(false);
					}
					item.transform.Find("name").GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_manyou.reward.type,t_manyou.reward.value1,t_manyou.reward.value2,t_manyou.reward.value3);
					GameObject icon = item.transform.Find("icon").gameObject;
					sys._instance.remove_child(icon);
					GameObject obj = icon_manager._instance.create_reward_icon(t_manyou.reward.type,t_manyou.reward.value1,t_manyou.reward.value2,t_manyou.reward.value3);
					obj.transform.parent = icon.transform;
					obj.transform.localPosition = new Vector3(0,0,0);
					obj.transform.localScale = new Vector3(1,1,1);
					item.SetActive(true);
				}
			}
			if(_qiyu_event.Count > 0)
			{
				for(int i = 0 ; i < _qiyu_event.Count;++i)
				{
					s_t_manyou t_manyou = game_data._instance.get_t_manyou(_qiyu_event[i].ts_id);
					GameObject item = (GameObject)Object.Instantiate(m_ten_qiyu_item);
					item.transform.parent = m_ten_qiyu_scro.transform;
					item.transform.localPosition = new Vector3(0,121- 83*i,0);
					item.transform.localScale = new Vector3(1,1,1);
					item.transform.name = _qiyu_event[i].id.ToString();

					item.transform.GetComponent<UISprite>().spriteName = t_manyou.image;
					item.transform.Find("name").GetComponent<UILabel>().text = t_manyou.name;
					item.SetActive(true);
				}
				m_qiyu_effect.SetActive(true);
			}
			is_press = false;
			m_ten_reward_gui.SetActive(true);
		}
	}

	int comp(protocol.game.tansuo_event tansuo_event1, protocol.game.tansuo_event tansuo_event2)
	{
		s_t_manyou t_manyou1 = game_data._instance.get_t_manyou(tansuo_event1.ts_id);
		s_t_manyou t_manyou2 = game_data._instance.get_t_manyou(tansuo_event2.ts_id);
		if(t_manyou1.type == 1 && t_manyou2.type != 1)
		{
			return -1;
		}
		return t_manyou1.id - t_manyou2.id;
	}

	void IMessage.message (s_message message)
	{
		if(message.m_type == "show_expolre_reward_gui")
		{
			is_press = false;
			m_reward_gui.GetComponent<explore_reward_gui>().type = (int)message.m_ints[0];
			m_reward_gui.SetActive(true);
		}
		if(message.m_type == "show_explore_wuping")
		{
			bool flag = false;
			if(message.m_long.Count > 0)
			{
				flag = (bool)message.m_long[0];
			}
			s_t_reward t_reward = message.m_object[0] as s_t_reward;
			string desc = (string)message.m_string[0];
			wuping(t_reward,desc,flag);
		}
		if(message.m_type == "update_explore_gui")
		{
			reset();
		}
		if(message.m_type == "show_manyou_task_effect")
		{
			if(explore_task_gui.effect())
			{
				m_task_point.SetActive(true);
			}
			else
			{
				m_task_point.SetActive(false);
			}
		}

	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_MANYOU_VIEW)
		{
			_msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_tansuo_view>(message.m_byte);
			score = _msg.point;
			rank = _msg.rank;
			m_end_time = _msg.time;
			m_num = _msg.num;
			_msg_event.Clear();
			rewards.Clear();
			for(int i = 0; i < _msg.events.Count;++i)
			{
				_msg_event.Add(_msg.events[i]);
			}
			for(int i = 0 ; i < _msg.rewards.Count;++i)
			{
				rewards.Add(_msg.rewards[i]);
			}
			long _time = (long)(m_end_time - timer.now());
			if(_time > 0)
			{
				s_message _message = new s_message();
				_message.m_type = "explore_create_floor";
				cmessage_center._instance.add_message(_message);
			}
			InvokeRepeating ("time", 0.0f, 1.0f);
			reset();
		}
		if (message.m_opcode == opclient_t.CMSG_HUODONG_MANYOU)
		{
			protocol.game.smsg_huodong_tansuo _msg1 = net_http._instance.parse_packet<protocol.game.smsg_huodong_tansuo>(message.m_byte);
			_pt_event.Clear();
			_qiyu_event.Clear();
			if(m_select == 0)
			{
				score += 20;
				if(m_num >= 5)
				{
					s_t_price t_price = game_data._instance.get_t_price((m_num -5) +1);
					sys._instance.m_self.sub_att(e_player_attr.player_jewel,t_price.manyou,game_data._instance.get_t_language ("explore_gui.cs_610_76"));//太空漫游消耗
				}
				m_num += 1;
			}
			else if(m_select == 1)
			{
				score += 200;
				int price = 0;
				int _num = 0;
				if(m_num <= 5)
				{
					_num = 5- m_num;
					for(int i = 1; i <= 10 - _num;++i)
					{
						s_t_price t_price = game_data._instance.get_t_price(i);
						price += t_price.manyou;
					}
				}
				else
				{
					_num = m_num - 5;
					for(int i = 1; i <= 10;++i)
					{
						s_t_price t_price = game_data._instance.get_t_price(i + _num);
						price += t_price.manyou;
					}
				}
				sys._instance.m_self.sub_att(e_player_attr.player_jewel,price,game_data._instance.get_t_language ("explore_gui.cs_610_76"));//太空漫游消耗
				m_num += 10;
			}
			if(_msg1.events.Count > 0)
			{
				for(int i = 0; i < _msg1.events.Count;++i)
				{
					s_t_manyou t_manyou = game_data._instance.get_t_manyou(_msg1.events[i].ts_id);
					if(t_manyou.type == 3)
					{
						_qiyu_event.Add(_msg1.events[i]);
						_msg_event.Add(_msg1.events[i]);
					}
					else
					{
						sys._instance.m_self.add_reward(t_manyou.reward.type,t_manyou.reward.value1,t_manyou.reward.value2,t_manyou.reward.value3,false,game_data._instance.get_t_language ("explore_gui.cs_652_134"));//太空漫游探索
						_pt_event.Add(_msg1.events[i]);
					}
				}
			}
			button(true);
			reward();
			reset();
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
