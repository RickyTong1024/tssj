
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rotary_table_gui : MonoBehaviour ,IMessage{

	public GameObject m_jifen;
	public GameObject m_rank;
	public GameObject m_time;
	public GameObject m_luck_select;
	public GameObject m_luck;
	public List<GameObject>m_luck_objs = new List<GameObject>();
	public GameObject m_luck_reward_jewel;
	public GameObject m_luck_free;
	public GameObject m_luck_jewel;
	public GameObject m_luck_jewel_5;
	public GameObject m_luck_turn;
	public GameObject m_luck_turn5;
	public GameObject m_haohua_select;
	public GameObject m_haohua;
	public List<GameObject> m_haohua_objs = new List<GameObject>();
	public GameObject m_haohua_reward_jewel;
	public GameObject m_haohua_free;
	public GameObject m_haohua_jewel;
	public GameObject m_haohua_jewel_5;
	public GameObject m_haohua_turn;
	public GameObject m_haohua_turn5;
	public GameObject m_reward_icon;
	public GameObject m_left;
	public GameObject m_right;
	public GameObject m_close;
	public GameObject m_right_button;
	public GameObject m_left_button;
	public GameObject m_info_scro;
	public GameObject m_info_gui;
	public GameObject m_sm_button;
	public GameObject m_use_items_gui;
	public GameObject m_reward_score;
	public GameObject m_rank_gui;
	public GameObject m_shell;
	public GameObject m_daren_rank_gui;
	public GameObject m_main;
	public GameObject m_top_left;
	public GameObject m_top_right;
	public List<GameObject> m_icons = new List<GameObject>();
	public List<GameObject> m_names = new List<GameObject>();
	public List<GameObject> m_scores = new List<GameObject>();
	public List<GameObject>m_chenghaos = new List<GameObject>();
	public ulong m_end_time;
	private int select = 0;
	private int luck_num = 0;
	private int luck_jewel_pool = 0;
	private int haohua_num = 0;
	private int haohua_jewel_pool = 0;
	private int point = 0;
	private int rank = 0;
	private List<string> names = new List<string>();
	private List<int> points = new List<int>();
	private List<int> ids = new List<int>();
    private List<int> gqs = new List<int>();
	private List<int> vips = new List<int>();
	private List<int> achieves = new List<int>();
	private List<int> zhuanpan_id = new List<int>();
	private List<int> zhuanpan_jewel = new List<int>();
	private List<int> chenghaos = new List<int>();
	private int consume_jewel = 0;
	private int turn = 0;
	private bool is_zhuang = false;
	private int reward_id = 0;
	private float life_time = 0;
	private float sin = 0;
	private GameObject m_select;
	private List<GameObject> m_objs = new List<GameObject>();
	private int round_num = 0;
	private int value = 0;
	private int value1 = 0;
	private float speed = 0;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		select = 0;
		round_num = 0;
		value = 0;
		value1 = 0;
		sin = 0;
		m_luck_select.SetActive (false);
		m_shell.SetActive (false);
		m_haohua_select.SetActive (false);
		m_main.SetActive (false);
		if(select == 0)
		{
			m_left.SetActive(false);
			m_right.SetActive(true);
			m_haohua.transform.localPosition = new Vector3(956,0,0);
			m_luck.transform.localPosition = new Vector3(0,0,0);
		}
		else if(select == 1)
		{
			m_left.SetActive(true);
			m_right.SetActive(false);
		}
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_ZHUANPAN_VIEW, _msg);
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}
	
	void time()
	{
		long _time = (long)(m_end_time - timer.now());
		m_time.GetComponent<UILabel>().text = timer.get_time_show_ex(_time);
		if(_time <= 0)
		{
			m_main.SetActive(false);
			m_left.SetActive(false);
			m_right.SetActive(false);
			daren_rank();
			m_daren_rank_gui.SetActive(true);
			m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
			CancelInvoke ("time");
		}
	}

	void daren_rank()
	{
		for(int i = 0; i < names.Count;++i)
		{
			sys._instance.remove_child(m_icons[i]);
			m_icons[i].transform.parent.gameObject.SetActive(true);
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)ids[i], achieves[i], vips[i],gqs[i]);
			_obj1.transform.parent = m_icons[i].transform;
			_obj1.transform.localScale = new Vector3(1, 1, 1);
			_obj1.transform.localPosition = new Vector3(0, 0, 0);
			m_names[i].GetComponent<UILabel>().text = game_data._instance.get_name_color(achieves[i]) + names[i].ToString();
			m_scores[i].GetComponent<UILabel>().text = game_data._instance.get_t_language ("explore_gui.cs_268_46") + points[i];//积分:
			sys._instance.get_chenghao(chenghaos[i],m_chenghaos[i]);
		}
		for(int i = names.Count ; i < m_icons.Count;++i)
		{
			m_icons[i].transform.parent.gameObject.SetActive(false);
			m_names[i].GetComponent<UILabel>().text = "";
			m_scores[i].GetComponent<UILabel>().text = "";
			sys._instance.get_chenghao(0,m_chenghaos[i]);
		}
	}

	void reset(int type = 0)
	{
		long _time = (long)(m_end_time - timer.now());
		if(_time <= 0)
		{
			m_main.SetActive(false);
			m_left.SetActive(false);
			m_right.SetActive(false);
			daren_rank();
			m_daren_rank_gui.SetActive(true);
			type = 1;
		}
		else
		{
			m_main.SetActive(true);
			if(select == 0)
			{
				m_left.SetActive(false);
				m_right.SetActive(true);
			}
			else if(select == 1)
			{
				m_left.SetActive(true);
				m_right.SetActive(false);
			}
		}
		List<int> zhuan_ids = new List<int>();
		if(type == 0)
		{
			foreach(int id in game_data._instance.m_dbc_zhuanpan.m_index.Keys)
			{
				s_t_zhuanpan t_zhuanpan = game_data._instance.get_t_zhuanpan(id);
				if(t_zhuanpan.zhuanpan_type == select +1)
				{
					zhuan_ids.Add(id);
				}
			}
		}
		if(select == 0)
		{
			m_luck_select.SetActive(true);
			m_luck_select.transform.localPosition = m_luck_objs[value].transform.localPosition;
			if(type == 0)
			{
				for(int i = 0 ;i < m_luck_objs.Count && i < zhuan_ids.Count;++i)
				{
					s_t_zhuanpan t_zhuanpan = game_data._instance.get_t_zhuanpan(zhuan_ids[i]);
					sys._instance.remove_child(m_luck_objs[i]);
					if(t_zhuanpan.type == 1 && t_zhuanpan.value1 == 2)
					{
						GameObject obj = Instantiate(m_reward_icon) as GameObject;
						obj.transform.parent = m_luck_objs[i].transform;
						obj.transform.localScale = new Vector3(1,1,1);
						obj.transform.localPosition = new Vector3(0,0,0);
						obj.SetActive(true);
					}
					else
					{
						GameObject _obj = icon_manager._instance.create_reward_icon(t_zhuanpan.type ,t_zhuanpan.value1 ,t_zhuanpan.value2 ,t_zhuanpan.value3);
						_obj.transform.parent = m_luck_objs[i].transform;
						_obj.transform.localScale = new Vector3(1,1,1);
						_obj.transform.localPosition = new Vector3(0,0,0);
					}
				}
			}
			m_luck_reward_jewel.GetComponent<UILabel>().text = luck_jewel_pool.ToString();
			string color = "";
			int jewel = 0;
			if(luck_num < 5)
			{
				m_luck_free.SetActive(true);
				m_luck_free.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("rotary_table_gui.cs_229_61"),(5- luck_num).ToString());//{0}次免费
				m_luck_jewel.SetActive(false);
				for(int i = 1;i <= 10 - (5-luck_num);++i)
				{
					s_t_price t_price = game_data._instance.get_t_price(i);
					jewel += t_price.luck_zhuanpan;
				}
				if(sys._instance.m_self.m_t_player.jewel < jewel)
				{
					color = "[ff0000]";
				}
				else
				{
					color = sys._instance.get_res_color(2);
				}
				m_luck_jewel_5.GetComponent<UILabel>().text = color+"x" + jewel;
			}
			else
			{
				jewel = 0;
				color = "";
				m_luck_free.SetActive(false);
				m_luck_jewel.SetActive(true);
				s_t_price t_price = game_data._instance.get_t_price(luck_num - 5 +1);
				if(sys._instance.m_self.m_t_player.jewel < t_price.luck_zhuanpan)
				{
					color = "[ff0000]";
				}
				else
				{
					color = sys._instance.get_res_color(2);
				}
				m_luck_jewel.GetComponent<UILabel>().text = color + "x"+t_price.luck_zhuanpan.ToString();
				color = "";
				for(int i = (luck_num - 5)+1;i <= 10 + (luck_num -5);++i)
				{
					s_t_price _price = game_data._instance.get_t_price(i);
					jewel += _price.luck_zhuanpan;
				}
				int num = (jewel *90/100) % 10;
				jewel = (jewel *90/100) - num;
				if(sys._instance.m_self.m_t_player.jewel < jewel)
				{
					color = "[ff0000]";
				}
				else
				{
					color = sys._instance.get_res_color(2);
				}
				m_luck_jewel_5.GetComponent<UILabel>().text = color + "x"+jewel;
			}
		}
		else if(select == 1)
		{
			m_haohua_select.SetActive(true);
			m_haohua_select.transform.localPosition = m_haohua_objs[value].transform.localPosition;
			if(type == 0)
			{
				for(int i = 0 ;i < m_haohua_objs.Count && i < zhuan_ids.Count;++i)
				{
					s_t_zhuanpan t_zhuanpan = game_data._instance.get_t_zhuanpan(zhuan_ids[i]);
					sys._instance.remove_child(m_haohua_objs[i]);
					if(t_zhuanpan.type == 1 && t_zhuanpan.value1 == 2)
					{
						GameObject obj = Instantiate(m_reward_icon) as GameObject;
						obj.transform.parent = m_haohua_objs[i].transform;
						obj.transform.localScale = new Vector3(1,1,1);
						obj.transform.localPosition = new Vector3(0,0,0);
						obj.SetActive(true);
					}
					else
					{
						GameObject _obj = icon_manager._instance.create_reward_icon(t_zhuanpan.type ,t_zhuanpan.value1 ,t_zhuanpan.value2 ,t_zhuanpan.value3);
						_obj.transform.parent = m_haohua_objs[i].transform;
						_obj.transform.localScale = new Vector3(1,1,1);
						_obj.transform.localPosition = new Vector3(0,0,0);
					}
				}
			}
			m_haohua_reward_jewel.GetComponent<UILabel>().text = haohua_jewel_pool.ToString();
			string color = "";
			int jewel = 0;
			if(haohua_num < 1)
			{
				m_haohua_free.SetActive(true);
				m_haohua_free.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("rotary_table_gui.cs_229_61"),(1- haohua_num).ToString() );//{0}次免费
				m_haohua_jewel.SetActive(false);
				for(int i = 1;i <= 10 - (1-haohua_num);++i)
				{
					s_t_price t_price = game_data._instance.get_t_price(i);
					jewel += t_price.haohua_zhuanpan;
				}
				if(sys._instance.m_self.m_t_player.jewel < jewel)
				{
					color = "[ff0000]";
				}
				else
				{
					color = sys._instance.get_res_color(2);
				}
				m_haohua_jewel_5.GetComponent<UILabel>().text = color + "x" + jewel;
			}
			else
			{
				jewel = 0;
				color = "";
				m_haohua_free.SetActive(false);
				m_haohua_jewel.SetActive(true);
				s_t_price t_price = game_data._instance.get_t_price(haohua_num - 1+1);
				if(sys._instance.m_self.m_t_player.jewel < t_price.haohua_zhuanpan)
				{
					color = "[ff0000]";
				}
				else
				{
					color = sys._instance.get_res_color(2);
				}
				m_haohua_jewel.GetComponent<UILabel>().text = color + "x"+ t_price.haohua_zhuanpan.ToString();
				color = "";
				for(int i = haohua_num;i <= 10 + (haohua_num-1);++i)
				{
					s_t_price _price = game_data._instance.get_t_price(i);
					jewel += _price.haohua_zhuanpan;
				}
				int num = (jewel *90/100) % 10;
				jewel = (jewel *90/100) - num;
				if(sys._instance.m_self.m_t_player.jewel < jewel)
				{
					color = "[ff0000]";
				}
				else
				{
					color = sys._instance.get_res_color(2);
				}
				m_haohua_jewel_5.GetComponent<UILabel>().text = color + "x" +jewel;
			}

		}
		if(rank <= 0)
		{
			m_rank.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81");//未上榜
		}
		else
		{
			m_rank.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81") , rank );//第{0}名
		}
		m_jifen.GetComponent<UILabel>().text = point.ToString ();
	}

	void appear()
	{
		m_top_left.SetActive (true);
		m_top_right.SetActive (true);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "left")
		{
			select = 0;
			value = 0;
			round_num = 0;
			value1 = 0;
			m_top_left.SetActive (false);
			m_top_right.SetActive (false);
			m_luck.GetComponent<TweenPosition>().onFinished.Clear ();
			m_luck.GetComponent<TweenPosition>().AddOnFinished(appear);
			reset();
		}
		else if(obj.transform.name == "right")
		{
			select = 1;
			value = 0;
			round_num = 0;
			value1 = 0;
			m_top_left.SetActive (false);
			m_top_right.SetActive (false);
			m_haohua.GetComponent<TweenPosition>().onFinished.Clear ();
			m_haohua.GetComponent<TweenPosition>().AddOnFinished(appear);
			reset();
		}
		else if(obj.transform.name == "sm")
		{
			if(m_info_scro.transform.GetComponent<SpringPanel>() != null)
			{
				m_info_scro.transform.GetComponent<SpringPanel>().enabled = false;
			}
			m_info_scro.transform.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
			m_info_scro.transform.localPosition = new Vector3 (0, 0, 0);
			m_info_gui.SetActive(true);
		}
		else if(obj.transform.name == "turn")
		{
			m_shell.SetActive(false);
			turn = 1;
			consume_jewel = 0;
			if(luck_num >= 5 && select == 0)
			{
				s_t_price t_price = game_data._instance.get_t_price(luck_num - 5 +1);
				consume_jewel = t_price.luck_zhuanpan;
				if(sys._instance.m_self.m_t_player.jewel < consume_jewel)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
					return;
				}
			}
			if(haohua_num >= 1 && select == 1)
			{
				s_t_price t_price = game_data._instance.get_t_price(haohua_num - 1 +1);
				consume_jewel = t_price.haohua_zhuanpan;
				if(sys._instance.m_self.m_t_player.jewel < consume_jewel)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
					return;
				}
			}
			protocol.game.cmsg_huodong_zhuanpan _msg = new protocol.game.cmsg_huodong_zhuanpan ();
			_msg.type = select;
			_msg.num = 1;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_zhuanpan> (opclient_t.CMSG_HUODONG_ZHUANPAN, _msg);
		}
		else if(obj.transform.name == "turn10")
		{
			m_shell.SetActive(false);
			turn = 2;
			consume_jewel = 0;
			if(select == 0)
			{
				if(luck_num < 5)
				{
					for(int i = 1;i <= 10 - (5-luck_num);++i)
					{
						s_t_price t_price = game_data._instance.get_t_price(i);
						consume_jewel += t_price.luck_zhuanpan;
					}
					int num = (consume_jewel *90/100) % 10;
					consume_jewel = (consume_jewel *90/100) - num;
					if(sys._instance.m_self.m_t_player.jewel < consume_jewel)
					{
						root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
						return;
					}
				}
				else
				{
					for(int i = (luck_num -5)+1;i <= 10 + (luck_num-5);++i)
					{
						s_t_price _price = game_data._instance.get_t_price(i);
						consume_jewel += _price.luck_zhuanpan;
					}
					int num = (consume_jewel *90 / 100) % 10;
					consume_jewel = (consume_jewel *90 / 100) - num;
					if(sys._instance.m_self.m_t_player.jewel < consume_jewel)
					{
						root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
						return;
					}
				}
			}
			else if(select == 1)
			{
				if(haohua_num < 1)
				{
					for(int i = 1;i <= 10 - (1-haohua_num);++i)
					{
						s_t_price t_price = game_data._instance.get_t_price(i);
						consume_jewel += t_price.haohua_zhuanpan;
					}
					int num = (consume_jewel *90/100) % 10;
					consume_jewel = (consume_jewel *90/100) - num;
					if(sys._instance.m_self.m_t_player.jewel < consume_jewel)
					{
						root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
						return;
					}
				}
				else
				{
					for(int i = haohua_num;i <= 10 + (haohua_num -1);++i)
					{
						s_t_price _price = game_data._instance.get_t_price(i);
						consume_jewel += _price.haohua_zhuanpan;
					}
					int num = (consume_jewel *90/100) % 10;
					consume_jewel = (consume_jewel *90/100) - num;
					if(sys._instance.m_self.m_t_player.jewel < consume_jewel)
					{
						root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
						return;
					}
				}
			}
			protocol.game.cmsg_huodong_zhuanpan _msg = new protocol.game.cmsg_huodong_zhuanpan ();
			_msg.type = select;
			_msg.num = 10;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_zhuanpan> (opclient_t.CMSG_HUODONG_ZHUANPAN, _msg);
		}
		else if(obj.transform.name == "rank")
		{
			m_rank_gui.GetComponent<rotary_table_rank_gui>().score = point;
			m_rank_gui.SetActive(true);
		}
		else if(obj.transform.name == "close")
		{
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
			this.gameObject.SetActive(false);
		}
		else if(obj.transform.name == "sm_close")
		{
			m_info_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	void IMessage.message (s_message message)
	{
		if(message.m_type == "daily_refresh")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_ZHUANPAN_VIEW, _msg);
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_ZHUANPAN_VIEW)
		{
			protocol.game.smsg_huodong_zhuanpan_view _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_zhuanpan_view>(message.m_byte);
			luck_num = _msg.num1;
			luck_jewel_pool = _msg.jewel_pool1;
			haohua_num = _msg.num2;
			haohua_jewel_pool =_msg.jewel_pool2;
			m_end_time = _msg.time;
			point = _msg.point;
			rank = _msg.rank;
			names.Clear();
			points.Clear();
			ids.Clear();
			vips.Clear();
			achieves.Clear();
			chenghaos.Clear();
			for(int i = 0; i < _msg.names.Count && i < _msg.points.Count && i< _msg.ids.Count && i <_msg.vips.Count && i < _msg.achieves.Count;++i)
			{
				names.Add(_msg.names[i]);
				points.Add(_msg.points[i]);
				ids.Add(_msg.ids[i]);
				vips.Add(_msg.vips[i]);
                gqs.Add(_msg.nalfalgs[i]);
				achieves.Add(_msg.achieves[i]);
				chenghaos.Add(_msg.chenghaos[i]);
			}
			InvokeRepeating ("time", 0.0f, 1.0f);
			reset ();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_ZHUANPAN)
		{
			protocol.game.smsg_huodong_zhuan _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_zhuan>(message.m_byte);
			zhuanpan_id = _msg.id;
			zhuanpan_jewel = _msg.jewel;
			sys._instance.m_self.sub_att(e_player_attr.player_jewel,consume_jewel,game_data._instance.get_t_language ("rotary_table_gui.cs_588_73"));//银河转盘消耗
			m_objs.Clear();
			if(select == 0)
			{
				if(turn == 1)
				{
					point += 10;
					luck_num += 1;
				}
				else
				{
					point += 100;
					luck_num += 10;
				}
				luck_jewel_pool = _msg.pool_jewel;
				m_select = m_luck_select;
				for(int i = 0; i < m_luck_objs.Count;++i)
				{
					m_objs.Add(m_luck_objs[i]);
				}
				m_luck_turn.GetComponent<BoxCollider>().enabled = false;
				m_luck_turn5.GetComponent<BoxCollider>().enabled = false;
				m_close.GetComponent<BoxCollider>().enabled = false;
				m_top_right.GetComponent<BoxCollider>().enabled = false;
				m_right_button.GetComponent<BoxCollider>().enabled = false;
				m_left_button.GetComponent<BoxCollider>().enabled = false;
				m_sm_button.GetComponent<BoxCollider>().enabled = false;
			}
			else if(select == 1)
			{
				if(turn == 1)
				{
					point += 100;
					haohua_num += 1;
				}
				else
				{
					point += 1000;
					haohua_num += 10;
				}
				haohua_jewel_pool = _msg.pool_jewel;
				m_select = m_haohua_select;
				for(int i = 0; i < m_haohua_objs.Count;++i)
				{
					m_objs.Add(m_haohua_objs[i]);
				}
				m_haohua_turn.GetComponent<BoxCollider>().enabled = false;
				m_haohua_turn5.GetComponent<BoxCollider>().enabled = false;
				m_close.GetComponent<BoxCollider>().enabled = false;
				m_top_right.GetComponent<BoxCollider>().enabled = false;
				m_right_button.GetComponent<BoxCollider>().enabled = false;
				m_left_button.GetComponent<BoxCollider>().enabled = false;
				m_sm_button.GetComponent<BoxCollider>().enabled = false;
			}
			for(int i = 0; i < _msg.id.Count;++i)
			{
				s_t_zhuanpan t_zhuanpan = game_data._instance.get_t_zhuanpan(_msg.id[i]);
				if(t_zhuanpan != null)
				{
					if(_msg.jewel[i] > 0)
					{
                        sys._instance.m_self.add_att(e_player_attr.player_jewel, _msg.jewel[i], false, game_data._instance.get_t_language ("rotary_table_gui.cs_649_103"));//银河转盘获得
					}
					else
					{
						sys._instance.m_self.add_reward(t_zhuanpan.type,t_zhuanpan.value1,t_zhuanpan.value2,t_zhuanpan.value3,false,game_data._instance.get_t_language ("rotary_table_gui.cs_649_103"));//银河转盘获得
					}
				}
			}
			reward_id = zhuanpan_id[0];
			speed = get_speed();
			sin = 0;
			is_zhuang = true;
			life_time = 0.5f;
		}
	}

	float get_speed()
	{
		float speed = 0;
		List<int> ids = new List<int>();
		foreach(int id in game_data._instance.m_dbc_zhuanpan.m_index.Keys)
		{
			s_t_zhuanpan t_zhuanpan = game_data._instance.get_t_zhuanpan(id);
			if(t_zhuanpan.zhuanpan_type == select +1)
			{
				ids.Add(id);
			}
		}
		int num = 0;
		for(int i = 0; i < ids.Count;++i)
		{
			if(ids[i] == reward_id)
			{
				break;
			}
			num++;
		}
		if(num >= value1)
		{
			num = (num - value1) + 12;
		}
		else
		{
			num = (11 - value1 + num) + 12;
		}
		sin += (float)(180 / num);
		if(sin > 180)
		{
			sin = 180;
		}
		speed = 1 + Mathf.Sin(Mathf.Deg2Rad * sin) *5.6f;
		return speed;
	}

	public bool is_stop(int _id)
	{
		List<int> ids = new List<int>();
		foreach(int id in game_data._instance.m_dbc_zhuanpan.m_index.Keys)
		{
			s_t_zhuanpan t_zhuanpan = game_data._instance.get_t_zhuanpan(id);
			if(t_zhuanpan.zhuanpan_type == select +1)
			{
				ids.Add(id);
			}
		}
		if(ids[_id] == reward_id)
		{
			return true;
		}
		return false;
	}

	void end()
	{
		if(select == 0)
		{
			m_luck_turn.GetComponent<BoxCollider>().enabled = true;
			m_luck_turn5.GetComponent<BoxCollider>().enabled = true;
			m_close.GetComponent<BoxCollider>().enabled = true;
			m_top_right.GetComponent<BoxCollider>().enabled = true;
			m_right_button.GetComponent<BoxCollider>().enabled = true;
			m_left_button.GetComponent<BoxCollider>().enabled = true;
			m_sm_button.GetComponent<BoxCollider>().enabled = true;
		}
		else if(select == 1)
		{
			m_haohua_turn.GetComponent<BoxCollider>().enabled = true;
			m_haohua_turn5.GetComponent<BoxCollider>().enabled = true;
			m_close.GetComponent<BoxCollider>().enabled = true;
			m_top_right.GetComponent<BoxCollider>().enabled = true;
			m_right_button.GetComponent<BoxCollider>().enabled = true;
			m_left_button.GetComponent<BoxCollider>().enabled = true;
			m_sm_button.GetComponent<BoxCollider>().enabled = true;
		}
		if(select == 0)
		{
			if(turn == 1)
			{
				m_reward_score.GetComponent<UILabel>().text = game_data._instance.get_t_language ("rotary_table_gui.cs_747_50");//获得10积分
			}
			else
			{
				m_reward_score.GetComponent<UILabel>().text = game_data._instance.get_t_language ("rotary_table_gui.cs_751_50");//获得100积分
			}
		}
		else if(select == 1)
		{
			if(turn == 1)
			{
				m_reward_score.GetComponent<UILabel>().text = game_data._instance.get_t_language ("rotary_table_gui.cs_751_50");//获得100积分
			}
			else
			{
				m_reward_score.GetComponent<UILabel>().text = game_data._instance.get_t_language ("rotary_table_gui.cs_762_50");//获得1000积分
			}
		}
		m_use_items_gui.GetComponent<rotary_reward>().init();
		for(int i = 0; i < zhuanpan_id.Count;++i)
		{
			s_t_zhuanpan t_zhuanpan = game_data._instance.get_t_zhuanpan(zhuanpan_id[i]);
			if(t_zhuanpan != null)
			{
				if(zhuanpan_jewel[i] > 0)
				{
					m_use_items_gui.GetComponent<rotary_reward>().types.Add(1);
					m_use_items_gui.GetComponent<rotary_reward>().values1.Add(2);
					m_use_items_gui.GetComponent<rotary_reward>().values2.Add(zhuanpan_jewel[i]);
					m_use_items_gui.GetComponent<rotary_reward>().values3.Add(0);
				}
				else
				{
					m_use_items_gui.GetComponent<rotary_reward>().types.Add(t_zhuanpan.type);
					m_use_items_gui.GetComponent<rotary_reward>().values1.Add(t_zhuanpan.value1);
					m_use_items_gui.GetComponent<rotary_reward>().values2.Add(t_zhuanpan.value2);
					m_use_items_gui.GetComponent<rotary_reward>().values3.Add(t_zhuanpan.value3);
				}
				m_use_items_gui.GetComponent<rotary_reward>().ids.Add(zhuanpan_id[i]);
			}
		}
		value1 = value;
		m_use_items_gui.SetActive(true);
		round_num = 0;
		sin = 0;
		reset(1);
	}
	
	// Update is called once per frame
	void Update () {
		if (is_zhuang) 
		{
			life_time -= Time.deltaTime*speed;
			if(life_time <= 0.0f)
			{
				speed = get_speed();
				if(round_num < 12)
				{
					life_time = 0.5f;
					m_select.transform.localPosition = m_objs[value].transform.localPosition;
					round_num++;
					value++;
					if(value > 11)
					{
						value = 0;
					}
				}
				else
				{
					life_time = 0.5f;
					m_select.transform.localPosition = m_objs[value].transform.localPosition;
					if(is_stop(value))
					{
						s_t_zhuanpan t_zhuanpan = game_data._instance.get_t_zhuanpan(reward_id);
						m_select.transform.localPosition = m_objs[value].transform.localPosition;
						if(t_zhuanpan.is_flash == 1)
						{
							m_shell.transform.localPosition = m_objs[value].transform.localPosition + new Vector3(115,-11,0);
							m_shell.SetActive (true);
						}
						life_time = 0.5f;
						is_zhuang = false;
						Invoke("end",0.5f);
					}
					else
					{
						round_num++;
						value++;
						if(value > 11)
						{
							value = 0;
						}
					}
				}
			}
		}
	}
}
