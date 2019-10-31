
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class czfp_gui : MonoBehaviour,IMessage {

	public GameObject m_huodong_time;
	public GameObject m_luck;
	public GameObject m_haohua;
	public GameObject m_luck_tishi;
	public GameObject m_luck_xipai;
	public GameObject m_luck_chongzhi;
	public GameObject m_luck_jewel;
	public List<GameObject> m_luck_objs = new List<GameObject>();
	public GameObject m_haohua_tishi;
	public GameObject m_haohua_xipai;
	public GameObject m_haohua_chongzhi;
	public GameObject m_haohua_jewel;
	public GameObject m_close;
	public GameObject m_shop_button;
	public GameObject m_reward_button;
	public GameObject m_recharge_button;
	public List<GameObject> m_haohua_objs = new List<GameObject>();
	public GameObject m_left;
	public GameObject m_right;
	public GameObject m_info_scro;
	public GameObject m_info_gui;
	public GameObject m_reward_gui;
	public GameObject m_use_items_gui;
	public List<GameObject> m_reward_objs = new List<GameObject>();
	public GameObject m_shop_gui;
	public GameObject m_jifen;
	public ulong m_end_time;
	public GameObject m_top_right;
	public GameObject m_top_left;
	private int select = 0;
	private List<GameObject> m_objs = new List<GameObject>();
	private int luck_num = 0;
	private List<int> luck_ids = new List<int>();
	private int haohua_num = 0;
	private List<int> haohua_ids = new List<int>();
	private int point = 0;
	private int luck_index = 0;
	private int haohua_index = 0;
	private int fanpai_id = 0;
	private bool is_xipai = false;
	private int m_input_num = 0;
	private int m_max_score = 0;
	private s_t_chongzhifanpai_shop t_chongzhifanpai_shop;
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
		is_xipai = false;
		if(select == 0)
		{
			m_left.SetActive(false);
			m_right.SetActive(true);
			m_haohua.transform.localPosition = new Vector3(803,0,0);
			m_luck.transform.localPosition = new Vector3(0,0,0);
		}
		else if(select == 1)
		{
			m_left.SetActive(true);
			m_right.SetActive(false);
		}
		for(int i= 0; i < m_luck_objs.Count && i < m_haohua_objs.Count;++i)
		{
			m_luck_objs[i].SetActive(false);
			m_haohua_objs[i].SetActive(false);
		}
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_FANPAI_VIEW, _msg);
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
			m_huodong_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
			CancelInvoke ("time");
		}
	}

	void reset()
	{
		m_jifen.GetComponent<UILabel>().text = point.ToString ();
		bool flag = false;
		if(select == 0)
		{
			m_left.SetActive(false);
			m_right.SetActive(true);
			flag = false;
			for(int i = 0; i < luck_ids.Count;++i)
			{
				if(luck_ids[i] == -1)
				{
					flag = true;
					break;
				}
			}
		}
		else if(select == 1)
		{
			m_left.SetActive(true);
			m_right.SetActive(false);
			flag = false;
			for(int i = 0; i < haohua_ids.Count;++i)
			{
				if(haohua_ids[i] == -1)
				{
					flag = true;
					break;
				}
			}
		}
		if(!flag)
		{
			init_ex();
		}
		else
		{
			init ();
		}
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
			m_top_left.SetActive (false);
			m_top_right.SetActive (false);
			m_luck.GetComponent<TweenPosition>().onFinished.Clear ();
			m_luck.GetComponent<TweenPosition>().AddOnFinished(appear);
			reset();
		}
		else if(obj.transform.name == "right")
		{
			select = 1;
			m_top_left.SetActive (false);
			m_top_right.SetActive (false);
			m_haohua.GetComponent<TweenPosition>().onFinished.Clear ();
			m_haohua.GetComponent<TweenPosition>().AddOnFinished(appear);
			reset();
		}
		else if(obj.transform.name == "close")
		{
			if(m_shop_gui.activeSelf)
			{
				m_shop_gui.transform.Find("frame_big").GetComponent<frame>().hide();
				return;
			}
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
			this.gameObject.SetActive(false);
		}
		else if(obj.transform.name == "reward")
		{
			reward_gui();
			m_reward_gui.SetActive(true);
		}
		else if(obj.transform.name == "xipai")
		{
			is_xipai = true;
			protocol.game.cmsg_huodong_fanpai_cz _msg = new protocol.game.cmsg_huodong_fanpai_cz ();
			if(select == 0)
			{
				_msg.type = 2;
			}
			else if(select == 1)
			{
				_msg.type = 3;
			}
			net_http._instance.send_msg<protocol.game.cmsg_huodong_fanpai_cz> (opclient_t.CMSG_HUODONG_FANPAI_CZ, _msg);
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
		else if(obj.transform.name == "sm_close")
		{
			m_info_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.transform.name == "reward_close")
		{
			m_reward_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.transform.name == "chongzhi")
		{
			s_message _message = new s_message();
			_message.m_type = "show_recharge";
			cmessage_center._instance.add_message(_message);
		}
		else if(obj.transform.name == "shop")
		{
			m_shop_gui.GetComponent<czfp_shop_gui>().score = point;
			m_shop_gui.SetActive(true);
		}
		else if(obj.transform.name == "reset")
		{
			if(sys._instance.m_self.m_t_player.jewel < 5)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
				return;
			}

			string _des = game_data._instance.get_t_language ("czfp_gui.cs_237_17");//是否花费5钻石重置翻牌?
			s_message _msg = new s_message();
			_msg.m_type = "czfp_chongzhi";
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
		}
	}

	public void open(GameObject obj)
	{
		if(m_max_score > 1100)
		{
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("czfp_gui.cs_248_58"));
			return;
		}
		if(select == 0)
		{
			if(point < 10)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("czfp_gui.cs_248_58"));
				return;
			}
			if(luck_num >= 10)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("czfp_gui.cs_260_59"));
				return;
			}
			luck_index = int.Parse (obj.transform.name);
			protocol.game.cmsg_huodong_fanpai _msg = new protocol.game.cmsg_huodong_fanpai ();
			_msg.type = 0;
			_msg.index = luck_index;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_fanpai> (opclient_t.CMSG_HUODONG_FANPAI, _msg);
		}
		else if(select == 1)
		{
			if(point < 100)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("czfp_gui.cs_248_58"));//积分不足,充值可以获得积分
				return;
			}
			if(haohua_num >= 10)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("czfp_gui.cs_260_59"));//次数不足,次数在每日0点重置
				return;
			}
			haohua_index = int.Parse (obj.transform.name);
			protocol.game.cmsg_huodong_fanpai _msg = new protocol.game.cmsg_huodong_fanpai ();
			_msg.type = 1;
			_msg.index = haohua_index;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_fanpai> (opclient_t.CMSG_HUODONG_FANPAI, _msg);
		}
	}

	void fanpai()
	{
		m_objs.Clear ();
		if(select == 0)
		{
			for(int i = 0 ; i < m_luck_objs.Count;++i)
			{
				m_objs.Add(m_luck_objs[i]);
			}
			m_luck_chongzhi.GetComponent<BoxCollider>().enabled = false;
		}
		else if(select == 1)
		{
			for(int i = 0 ; i < m_luck_objs.Count;++i)
			{
				m_objs.Add(m_haohua_objs[i]);
			}
			m_haohua_chongzhi.GetComponent<BoxCollider>().enabled = false;
		}
		m_right.GetComponent<BoxCollider>().enabled = false;
		m_left.GetComponent<BoxCollider>().enabled = false;
		m_close.GetComponent<BoxCollider>().enabled = false;
		m_shop_button.GetComponent<BoxCollider>().enabled = false;
		m_reward_button.GetComponent<BoxCollider>().enabled = false;
		m_recharge_button.GetComponent<BoxCollider>().enabled = false;
		for(int i = 0 ; i < m_objs.Count;++i)
		{
			m_objs[i].transform.GetComponent<BoxCollider>().enabled = false;
		}
		int index = 0;
		if(select == 0)
		{
			index = luck_index;
		}
		if(select == 1)
		{
			index = haohua_index;
		}
		TweenRotation ro = m_objs[index].AddComponent<TweenRotation>();
		ro.from = m_objs[index].transform.localEulerAngles;
		ro.to = new Vector3(0,90,0);
		ro.duration = 0.2f;
		ro.enabled = true;
		ro.AddOnFinished(fanpai_finished1);
		if(m_objs[index].GetComponent<TweenPosition>() != null)
		{
			DestroyImmediate(m_objs[index].GetComponent<TweenPosition>());
		}
	}

	void fanpai_finished1()
	{
		s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai (fanpai_id);
		int index = 0;
		if(select == 0)
		{
			index = luck_index;
		}
		if(select == 1)
		{
			index = haohua_index;
		}
		GameObject icon = m_objs[index].transform.Find("icon").gameObject;
		sys._instance.remove_child(icon);
		icon.SetActive(true);
		GameObject obj = icon_manager._instance.create_resource_icon(2,t_chongzhifanpai.jewel);
		obj.transform.parent = icon.transform;
		obj.transform.localPosition = new Vector3(0,0,0);
		obj.transform.localEulerAngles = new Vector3(0,0,0);
		obj.transform.localScale = new Vector3(1,1,1);
		
		m_objs[index].transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + game_data._instance.get_t_language ("czfp_gui.cs_360_108");//钻石
		m_objs[index].transform.GetComponent<UISprite>().spriteName = "bwsp_ck002";
		TweenRotation ro = m_objs[index].AddComponent<TweenRotation>();
		ro.from = m_objs[index].transform.localEulerAngles;
		ro.to = new Vector3 (0, 0, 0);
		ro.duration = 0.5f;
		ro.enabled = true;
		ro.AddOnFinished (fanpai_end);
		if(m_objs[index].GetComponent<TweenRotation>() != null)
		{
			DestroyImmediate(m_objs[index].GetComponent<TweenRotation>());
		}
	}

	void fanpai_end()
	{
		s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai (fanpai_id);
		m_use_items_gui.GetComponent<use_items_gui>().init();
		m_use_items_gui.GetComponent<use_items_gui>().types.Add(1);
		m_use_items_gui.GetComponent<use_items_gui>().values1.Add(2);
		m_use_items_gui.GetComponent<use_items_gui>().values2.Add(t_chongzhifanpai.jewel);
		m_use_items_gui.GetComponent<use_items_gui>().values3.Add(0);
		m_use_items_gui.SetActive(true);
		m_right.GetComponent<BoxCollider>().enabled = true;
		m_left.GetComponent<BoxCollider>().enabled = true;
		m_close.GetComponent<BoxCollider>().enabled = true;
		m_shop_button.GetComponent<BoxCollider>().enabled = true;
		m_reward_button.GetComponent<BoxCollider>().enabled = true;
		m_recharge_button.GetComponent<BoxCollider>().enabled = true;
		if(select == 0)
		{
			m_luck_chongzhi.GetComponent<BoxCollider>().enabled = true;
		}
		else if(select == 1)
		{
			m_haohua_chongzhi.GetComponent<BoxCollider>().enabled = true;
		}
		reset ();
	}

	void kapai_reset()
	{
		m_objs.Clear ();
		if(select == 0)
		{
			for(int i = 0 ; i < m_luck_objs.Count;++i)
			{
				m_objs.Add(m_luck_objs[i]);
			}
			m_luck_xipai.SetActive(false);
			m_luck_chongzhi.GetComponent<BoxCollider>().enabled = false;
		}
		else if(select == 1)
		{
			for(int i = 0 ; i < m_luck_objs.Count;++i)
			{
				m_objs.Add(m_haohua_objs[i]);
			}
			m_haohua_xipai.SetActive(false);
			m_haohua_chongzhi.GetComponent<BoxCollider>().enabled = false;
		}
		m_right.GetComponent<BoxCollider>().enabled = false;
		m_left.GetComponent<BoxCollider>().enabled = false;
		m_close.GetComponent<BoxCollider>().enabled = false;
		m_shop_button.GetComponent<BoxCollider>().enabled = false;
		m_reward_button.GetComponent<BoxCollider>().enabled = false;
		m_recharge_button.GetComponent<BoxCollider>().enabled = false;
		for(int i = 0 ; i < m_objs.Count;++i)
		{
			m_objs[i].transform.GetComponent<BoxCollider>().enabled = false;
		}
		bool flag = false;
		for (int i = 0; i < m_objs.Count && i < luck_ids.Count && i < haohua_ids.Count; i++)
		{
			if(select == 0)
			{
				if(luck_ids[i] == 0)
				{
					continue;
				}
			}
			else if(select == 1)
			{
				if(haohua_ids[i] == 0)
				{
					continue;
				}
			}
			flag = true;
			TweenRotation ro = m_objs[i].AddComponent<TweenRotation>();
			ro.from = m_objs[i].transform.localEulerAngles;
			ro.to = new Vector3(0,90,0);
			ro.duration = 0.2f;
			ro.enabled = true;
			ro.AddOnFinished(finished1);
			if(m_objs[i].GetComponent<TweenPosition>() != null)
			{
				DestroyImmediate(m_objs[i].GetComponent<TweenPosition>());
			}
		}
		if(select == 0)
		{
			for(int i = 0; i < luck_ids.Count;++i)
			{
				luck_ids[i] = 0;
			}
		}
		else if(select == 1)
		{
			for(int i = 0; i < haohua_ids.Count;++i)
			{
				haohua_ids[i] = 0;
			}
		}
		if(!flag)
		{
			change();
		}
	}

	void reward_gui()
	{
		List<int> ids = new List<int>();
		foreach(int id in game_data._instance.m_dbc_chongzhifanpai.m_index.Keys)
		{
			s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai(id);
			if(t_chongzhifanpai.type == select +1)
			{
				ids.Add(id);
			}
		}
		for(int i = 0; i < m_reward_objs.Count && i < ids.Count;++i)
		{
			s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai(ids[i]);
			GameObject icon = m_reward_objs[i].transform.Find("icon").gameObject;
			sys._instance.remove_child(icon);
			GameObject obj = icon_manager._instance.create_resource_icon(2,t_chongzhifanpai.jewel);
			obj.transform.parent = icon.transform;
			obj.transform.localPosition = new Vector3(0,0,0);
			obj.transform.localScale = new Vector3(1,1,1);

			m_reward_objs[i].transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + game_data._instance.get_t_language ("czfp_gui.cs_360_108");//钻石
		}
	}

	void xipai()
	{
		if(select == 0)
		{
			for(int i = 0; i < luck_ids.Count;++i)
			{
				luck_ids[i] = 0;
			}
		}
		else if(select == 1)
		{
			for(int i = 0; i < haohua_ids.Count;++i)
			{
				haohua_ids[i] = 0;
			}
		}
		m_objs.Clear ();
		if(select == 0)
		{
			for(int i = 0 ; i < m_luck_objs.Count;++i)
			{
				m_objs.Add(m_luck_objs[i]);
			}
			m_luck_xipai.SetActive(false);
			m_luck_tishi.SetActive(false);
		}
		else if(select == 1)
		{
			for(int i = 0 ; i < m_luck_objs.Count;++i)
			{
				m_objs.Add(m_haohua_objs[i]);
			}
			m_haohua_xipai.SetActive(false);
			m_haohua_tishi.SetActive(false);
		}
		m_right.GetComponent<BoxCollider>().enabled = false;
		m_left.GetComponent<BoxCollider>().enabled = false;
		m_close.GetComponent<BoxCollider>().enabled = false;
		m_shop_button.GetComponent<BoxCollider>().enabled = false;
		m_reward_button.GetComponent<BoxCollider>().enabled = false;
		m_recharge_button.GetComponent<BoxCollider>().enabled = false;
		for(int i = 0 ; i < m_objs.Count;++i)
		{
			m_objs[i].transform.GetComponent<BoxCollider>().enabled = false;
		}
		for (int i = 0; i < m_objs.Count; i++)
		{
			TweenRotation ro = m_objs[i].AddComponent<TweenRotation>();
			ro.from = m_objs[i].transform.localEulerAngles;
			ro.to = new Vector3(0,90,0);
			ro.duration = 0.2f;
			ro.enabled = true;
			ro.AddOnFinished(finished1);
			if(m_objs[i].GetComponent<TweenPosition>() != null)
			{
				DestroyImmediate(m_objs[i].GetComponent<TweenPosition>());
			}
		}
	}

	void finished1()
	{
		for(int i = 0; i < m_objs.Count;++i)
		{
			if(select == 0)
			{
				m_objs[i].transform.GetComponent<UISprite>().spriteName = "bwsp_ck001";
			}
			else if(select == 1)
			{
				m_objs[i].transform.GetComponent<UISprite>().spriteName = "czfp_pai";
			}
			GameObject icon = m_objs[i].transform.Find("icon").gameObject;
			sys._instance.remove_child(icon);
			icon.SetActive(false);
			TweenRotation ro = m_objs[i].AddComponent<TweenRotation>();
			ro.from = m_objs[i].transform.localEulerAngles;
			ro.to = new Vector3 (0, 0, 0);
			ro.duration = 0.5f;
			ro.enabled = true;
			m_objs[i].transform.Find("text").GetComponent<UILabel>().text = "";
			if(m_objs[i].GetComponent<TweenRotation>() != null)
			{
				DestroyImmediate(m_objs[i].GetComponent<TweenRotation>());
			}
		}
		Invoke ("change",0.6f);
	}

	void finished2()
	{
		for(int i = 0; i < m_objs.Count;++i)
		{
			TweenPosition tp = m_objs[i].AddComponent<TweenPosition>();
			tp.from = m_objs[i].transform.localPosition;
			tp.to = new Vector3(0,0,0);
			tp.duration = 0.5f;
			tp.enabled = true;
			if(m_objs[i].GetComponent<TweenPosition>() != null)
			{
				DestroyImmediate(m_objs[i].GetComponent<TweenPosition>());
			}
			tp.AddOnFinished(end);
		}
	}

	void end()
	{
		for(int i = 0; i < 4;++i)
		{
			TweenPosition tp = m_objs[i].AddComponent<TweenPosition>();
			tp.from = m_objs[i].transform.localPosition;
			tp.to = new Vector3(-258+172*i,66,0);
			tp.duration = 0.5f;
			tp.enabled = true;

			TweenPosition tp1 = m_objs[i+4].AddComponent<TweenPosition>();
			tp1.from = m_objs[i+4].transform.localPosition;
			tp1.to = new Vector3(-258+172*i,-97,0);
			tp1.duration = 0.5f;
			tp1.enabled = true;
			if(m_objs[i].GetComponent<TweenPosition>() != null)
			{
				DestroyImmediate(m_objs[i].GetComponent<TweenPosition>());
			}
			if(m_objs[i+4].GetComponent<TweenPosition>() != null)
			{
				DestroyImmediate(m_objs[i+4].GetComponent<TweenPosition>());
			}
		}
		string color = "";
		if(select == 0)
		{
			m_luck_chongzhi.GetComponent<BoxCollider>().enabled = true;
			m_luck_jewel.SetActive(true);
			if(sys._instance.m_self.m_t_player.jewel < 5)
			{
				color = "[ff0000]";
			}
			else
			{
				color = sys._instance.get_res_color(2);
			}
			m_luck_jewel.GetComponent<UILabel>().text = color + "x5";
			m_luck_tishi.SetActive(true);
			m_luck_chongzhi.SetActive(true);
            m_luck_tishi.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("czfp_gui.cs_651_61"), (10 - luck_num)) + ")";
		}
		else if(select == 1)
		{
			m_haohua_chongzhi.GetComponent<BoxCollider>().enabled = true;
			m_haohua_jewel.SetActive(true);
			if(sys._instance.m_self.m_t_player.jewel < 5)
			{
				color = "[ff0000]";
			}
			else
			{
				color = sys._instance.get_res_color(2);
			}
			m_haohua_jewel.GetComponent<UILabel>().text = color + "x5";
			m_haohua_tishi.SetActive(true);
			m_haohua_chongzhi.SetActive(true);
            m_haohua_tishi.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("czfp_gui.cs_668_63"), (10 - haohua_num)) + ")";
		}
		m_right.GetComponent<BoxCollider>().enabled = true;
		m_left.GetComponent<BoxCollider>().enabled = true;
		m_close.GetComponent<BoxCollider>().enabled = true;
		m_shop_button.GetComponent<BoxCollider>().enabled = true;
		m_reward_button.GetComponent<BoxCollider>().enabled = true;
		m_recharge_button.GetComponent<BoxCollider>().enabled = true;
		for(int i = 0 ; i < m_objs.Count;++i)
		{
			m_objs[i].transform.GetComponent<BoxCollider>().enabled = true;
		}
		int num = 0;
		int num1 = 0;
		for(int i = 0; i < luck_ids.Count;++i)
		{
			if(luck_ids[i] > 0)
			{
				num ++;
			}
		}
		for(int i = 0; i < haohua_ids.Count;++i)
		{
			if(haohua_ids[i] > 0)
			{
				num1 ++;
			}
		}
		if(num == luck_ids.Count && num > 0)
		{
			for(int i = 0; i < luck_ids.Count;++i)
			{
				luck_ids[i] = 0;
			}
		}
		if(num1 == haohua_ids.Count && num1 > 0)
		{
			for(int i = 0; i < haohua_ids.Count;++i)
			{
				haohua_ids[i] = 0;
			}
		}
	}

	void change()
	{
		for(int i = 0; i < 4;++i)
		{
			TweenPosition tp = m_objs[i].AddComponent<TweenPosition>();
			tp.from = m_objs[i].transform.localPosition;
			tp.to = new Vector3(0,m_objs[i].transform.localPosition.y,0);
			tp.duration = 0.5f;
			tp.enabled = true;
			tp.AddOnFinished(finished2);
			
			TweenPosition tp1 = m_objs[i+4].AddComponent<TweenPosition>();
			tp1.from = m_objs[i+4].transform.localPosition;
			tp1.to = new Vector3(0,m_objs[i+4].transform.localPosition.y,0);
			tp1.duration = 0.5f;
			tp1.enabled = true;
			tp1.AddOnFinished(finished2);
			if(m_objs[i].GetComponent<TweenRotation>() != null)
			{
				DestroyImmediate(m_objs[i].GetComponent<TweenRotation>());
			}
			if(m_objs[i+4].GetComponent<TweenRotation>() != null)
			{
				DestroyImmediate(m_objs[i+4].GetComponent<TweenRotation>());
			}
		}
	}

	void init_ex()
	{
		if(select == 0)
		{
			for(int i = 0; i < m_luck_objs.Count && i < luck_ids.Count;++i)
			{
				s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai(luck_ids[i]);
				if(t_chongzhifanpai != null)
				{
					m_luck_objs[i].GetComponent<BoxCollider>().enabled = false;
					m_luck_objs[i].transform.GetComponent<UISprite>().spriteName = "bwsp_ck002";
					GameObject icon = m_luck_objs[i].transform.Find("icon").gameObject;
					sys._instance.remove_child(icon);
					icon.SetActive(true);
					GameObject _obj = icon_manager._instance.create_resource_icon(2,t_chongzhifanpai.jewel);
					_obj.transform.parent = icon.transform;
					_obj.transform.localPosition = new Vector3(0,0,0);
					_obj.transform.localScale = new Vector3(1, 1, 1);
					m_luck_objs[i].transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + game_data._instance.get_t_language ("czfp_gui.cs_360_108");//钻石
				}
				else
				{
					m_luck_objs[i].GetComponent<BoxCollider>().enabled = true;
					m_luck_objs[i].transform.GetComponent<UISprite>().spriteName = "bwsp_ck001";
				}
				m_luck_objs[i].SetActive(true);
			}
            m_luck_tishi.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("czfp_gui.cs_651_61"), (10 - luck_num)) + ")";
			m_luck_xipai.SetActive(false);
			m_luck_chongzhi.SetActive(true);
			m_luck_jewel.SetActive(true);
			string color= "";
			if(sys._instance.m_self.m_t_player.jewel < 5)
			{
				color = "[ff0000]";
			}
			else
			{
				color = sys._instance.get_res_color(2);
			}
			m_luck_jewel.GetComponent<UILabel>().text = color + "x5";
		}
		if(select == 1)
		{
			for(int i = 0; i < m_haohua_objs.Count && i < haohua_ids.Count;++i)
			{
				s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai(haohua_ids[i]);
				if(t_chongzhifanpai != null)
				{
					m_haohua_objs[i].GetComponent<BoxCollider>().enabled = false;
					m_haohua_objs[i].transform.GetComponent<UISprite>().spriteName = "bwsp_ck002";
					GameObject icon = m_haohua_objs[i].transform.Find("icon").gameObject;
					sys._instance.remove_child(icon);
					icon.SetActive(true);
					GameObject _obj = icon_manager._instance.create_resource_icon(2,t_chongzhifanpai.jewel);
					_obj.transform.parent = icon.transform;
					_obj.transform.localPosition = new Vector3(0,0,0);
					_obj.transform.localScale = new Vector3(1, 1, 1);
					m_haohua_objs[i].transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + game_data._instance.get_t_language ("czfp_gui.cs_360_108");//钻石
				}
				else
				{
					m_haohua_objs[i].GetComponent<BoxCollider>().enabled = true;
					m_haohua_objs[i].transform.GetComponent<UISprite>().spriteName = "czfp_pai";
				}
				m_haohua_objs[i].SetActive(true);
			}
            m_haohua_tishi.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("czfp_gui.cs_668_63"), (10 - haohua_num)) + ")";
			m_haohua_xipai.SetActive(false);
			m_haohua_chongzhi.SetActive(true);
			m_haohua_jewel.SetActive(true);
			string color= "";
			if(sys._instance.m_self.m_t_player.jewel < 5)
			{
				color = "[ff0000]";
			}
			else
			{
				color = sys._instance.get_res_color(2);
			}
			m_haohua_jewel.GetComponent<UILabel>().text = color + "x5";
		}
	}

	void init()
	{
		List<int> ids = new List<int>();
		foreach(int id in game_data._instance.m_dbc_chongzhifanpai.m_index.Keys)
		{
			s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai(id);
			if(t_chongzhifanpai.type == select +1)
			{
				ids.Add(id);
			}
		}
		if(select == 0)
		{
			for(int i = 0; i < m_luck_objs.Count && i < ids.Count;++i)
			{
				m_luck_objs[i].GetComponent<BoxCollider>().enabled = false;
				s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai(ids[i]);
				m_luck_objs[i].transform.GetComponent<UISprite>().spriteName = "bwsp_ck002";
				GameObject icon = m_luck_objs[i].transform.Find("icon").gameObject;
				sys._instance.remove_child(icon);
				icon.SetActive(true);
				GameObject _obj = icon_manager._instance.create_resource_icon(2,t_chongzhifanpai.jewel);
				_obj.transform.parent = icon.transform;
				_obj.transform.localPosition = new Vector3(0,0,0);
				_obj.transform.localScale = new Vector3(1, 1, 1);
				m_luck_objs[i].transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + game_data._instance.get_t_language ("czfp_gui.cs_360_108");//钻石
				m_luck_objs[i].SetActive(true);
			}
			m_luck_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("czfp_gui.cs_852_47");//翻牌可以获得以上奖励，请先点击洗牌
			m_luck_xipai.SetActive(true);
			m_luck_chongzhi.SetActive(false);
			m_luck_jewel.SetActive(false);
		}
		if(select == 1)
		{
			for(int i = 0; i < m_haohua_objs.Count && i < ids.Count;++i)
			{
				m_haohua_objs[i].GetComponent<BoxCollider>().enabled = false;
				s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai(ids[i]);
				m_haohua_objs[i].transform.GetComponent<UISprite>().spriteName = "bwsp_ck002";
				GameObject icon = m_haohua_objs[i].transform.Find("icon").gameObject;
				sys._instance.remove_child(icon);
				icon.SetActive(true);
				GameObject _obj = icon_manager._instance.create_resource_icon(2,t_chongzhifanpai.jewel);
				_obj.transform.parent = icon.transform;
				_obj.transform.localPosition = new Vector3(0,0,0);
				_obj.transform.localScale = new Vector3(1, 1, 1);
				m_haohua_objs[i].transform.Find("text").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + game_data._instance.get_t_language ("czfp_gui.cs_360_108");//钻石
				m_haohua_objs[i].SetActive(true);
			}
			m_haohua_tishi.GetComponent<UILabel>().text = game_data._instance.get_t_language ("czfp_gui.cs_852_47");//翻牌可以获得以上奖励，请先点击洗牌
			m_haohua_xipai.SetActive(true);
			m_haohua_chongzhi.SetActive(false);
			m_haohua_jewel.SetActive(false);
		}
	}

	void IMessage.message (s_message message)
	{
		if(message.m_type == "refresh_czfp" || message.m_type == "daily_refresh")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_FANPAI_VIEW, _msg);
		}
		if(message.m_type == "refresh_czfp_ex")
		{
			int num = 0;
			int num1 = 0;
			for(int i = 0; i < luck_ids.Count;++i)
			{
				if(luck_ids[i] > 0)
				{
					num ++;
				}
			}
			for(int i = 0; i < haohua_ids.Count;++i)
			{
				if(haohua_ids[i] > 0)
				{
					num1 ++;
				}
			}
			if((num == luck_ids.Count && num > 0) || (num1 == haohua_ids.Count && num1 > 0))
			{
				kapai_reset();
			}
		}
		if(message.m_type == "czfp_chongzhi")
		{
			protocol.game.cmsg_huodong_fanpai_cz _msg = new protocol.game.cmsg_huodong_fanpai_cz ();
			_msg.type = select;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_fanpai_cz> (opclient_t.CMSG_HUODONG_FANPAI_CZ, _msg);
		}
		if(message.m_type == "czfp_shop")
		{
			int id = (int)message.m_ints[0];
			t_chongzhifanpai_shop = game_data._instance.get_t_chongzhifanpai_shop(id);
			int buy_num = point/t_chongzhifanpai_shop.price;
			s_message _message = new s_message();
			_message.m_type = "buy_num_gui";
			_message.m_ints.Add((int)message.m_ints[0]);
			_message.m_ints.Add(19);
			_message.m_ints.Add(buy_num);
			cmessage_center._instance.add_message(_message);
		}
		if(message.m_type == "chongzhifanpai_shop_buy")
		{
			m_input_num = (int)message.m_ints[0];
			if(t_chongzhifanpai_shop.price*m_input_num > point)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("czfp_gui.cs_934_59"));//积分不足
				return;
			}
			protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
			_net_msg.item_id = t_chongzhifanpai_shop.id;
			_net_msg.num = m_input_num;
			net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_CHONGZHIFANPAI_BUY, _net_msg);
		}
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_FANPAI_VIEW)
		{
			protocol.game.smsg_huodong_fanpai_view _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_fanpai_view>(message.m_byte);
			luck_num = _msg.num1;
			haohua_num = _msg.num2;
			m_end_time = _msg.time;
			m_max_score = _msg.use;
			luck_ids.Clear();
			haohua_ids.Clear();
			for(int i = 0; i < _msg.ids1.Count;++i)
			{
				luck_ids.Add(_msg.ids1[i]);
			}
			for(int i = 0; i < _msg.ids2.Count;++i)
			{
				haohua_ids.Add(_msg.ids2[i]);
			}
			point = _msg.point;
			InvokeRepeating ("time", 0.0f, 1.0f);
			reset ();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_FANPAI)
		{
			protocol.game.smsg_huodong_fanpai _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_fanpai>(message.m_byte);
			fanpai_id = _msg.id;
			s_t_chongzhifanpai t_chongzhifanpai = game_data._instance.get_t_chongzhifanpai(fanpai_id);
			sys._instance.m_self.add_att(e_player_attr.player_jewel,t_chongzhifanpai.jewel,false,game_data._instance.get_t_language ("czfp_gui.cs_972_88"));//充值翻牌获得
			if(select == 0)
			{
				point -= 10;
				m_max_score += 10;
				luck_num ++;
				luck_ids[luck_index] = fanpai_id;
			}
			else if(select == 1)
			{
				point -= 100;
				m_max_score += 100;
				haohua_num ++;
				haohua_ids[haohua_index] = fanpai_id;
			}
			fanpai();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_FANPAI_CZ)
		{
			if(!is_xipai)
			{
				sys._instance.m_self.sub_att(e_player_attr.player_jewel,5,game_data._instance.get_t_language ("czfp_gui.cs_994_62"));//充值翻牌重置消耗
			}
			if(is_xipai)
			{
				is_xipai = false;
				xipai();
			}
			else
			{
				kapai_reset();
			}
		}
		if(message.m_opcode == opclient_t.CMSG_CHONGZHIFANPAI_BUY)
		{
			protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy>(message.m_byte);
			for (int i = 0; i < _msg.equips.Count; i++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i], true);
			}
			for (int i = 0; i < _msg.treasures.Count; i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i], true);
			}
			sys._instance.m_self.add_reward(t_chongzhifanpai_shop.type, t_chongzhifanpai_shop.value1, t_chongzhifanpai_shop.value2 * m_input_num, t_chongzhifanpai_shop.value3,game_data._instance.get_t_language ("czfp_gui.cs_972_88"));//充值翻牌获得
			point -= t_chongzhifanpai_shop.price*m_input_num;
			m_shop_gui.GetComponent<czfp_shop_gui>().score = point;
			m_shop_gui.GetComponent<czfp_shop_gui>().refresh_gird();
			root_gui._instance.m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide();
			reset();
		}
	}
	// Update is called once per frame
	void Update () {
	}
}
