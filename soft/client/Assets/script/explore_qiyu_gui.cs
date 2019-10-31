
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explore_qiyu_gui : MonoBehaviour ,IMessage{	


	public int qiyu_id = 0;

	public GameObject m_scro;
	public GameObject m_chonzghi_panel;
	public GameObject m_question_panel;
	public GameObject m_shop_panel;
	public GameObject m_baoxiang_panel;
	public GameObject m_boss_panel;

	public GameObject m_shop_icon;
	public GameObject m_discount;
	public GameObject m_old_price;
	public GameObject m_now_price;
	public GameObject m_fresh_price;
	public GameObject m_shop_name;
	public GameObject m_shop_time;
	public GameObject m_jifen;

	public GameObject m_bx_free;
	public GameObject m_bx_recharge;
	public GameObject m_bx_free_button;
	public GameObject m_bx_recharge_button;
	public GameObject m_bx_price;
	public GameObject m_bx_time;

	public GameObject m_boss_bar;
	public GameObject m_boss_text;
	public GameObject m_boss_tili;
	public UILabel m_boss_name;
	public UILabel m_boss_reward_name;
	public UILabel m_boss_reward_num;
	public UISprite m_boss_sprite;
	public GameObject m_boss_icon;
	public UISprite m_boss_reward_small;
	public GameObject m_boss_time;

	public UILabel m_question;
	public List<GameObject> m_question_items = new List<GameObject>();
	public GameObject m_correct_icon;
	public UILabel m_correct_name;
	public GameObject m_error_icon;
	public UILabel m_error_name;
	public GameObject m_question_time;
	public GameObject m_question_role;

	public GameObject m_chongzhi_time;
	public UILabel m_chongzhi_title;
	public List<GameObject> m_icons = new List<GameObject>();
	public List<GameObject> m_names = new List<GameObject>();
	public UILabel m_chongzhi_tishi;
	public GameObject m_recharge_button;
	public GameObject m_liqu_button;

	private GameObject m_huodong_time = null;
	private s_t_manyou_qiyu t_manyou_qiyu;
	private int bx_seclect = 0;
	private ulong m_end_time = 0;
	private protocol.game.tansuo_event m_event;
	private int m_select = 0;
	private int score = 0;
	private int dati_id = 0;
	private List<GameObject> m_items = new List<GameObject>();
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
		/*if(qiyu_id != 0)
		{	
			m_select = qiyu_id;
			reset_ex();
			qiyu_id = 0;
		}
		else
		{
			reset ();
		}*/
	}

	void OnDisable()
	{	
		CancelInvoke ("time");
	}
	
	void time()
	{	
		long _time = (long)(m_end_time - timer.now());
		m_huodong_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("explore_qiyu_gui.cs_101_49") + timer.get_time_show(_time);//剩余时间 - 
		if(_time <= 0)
		{	
			m_huodong_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
			CancelInvoke ("time");
		}
	}

	public void reset_ex()
	{	
		int _num = 0;
		m_select = qiyu_id;
		qiyu_id = 0;
		if(m_scro.GetComponent<SpringPanel>() != null)
		{	
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0,0);
		m_scro.transform.localPosition = new Vector3 (0, 0, 0);
		sys._instance.remove_child (m_scro);
		for(int i = 0; i < explore_gui._instance._msg_event.Count;++i)
		{	
			_num++;
			if(explore_gui._instance._msg_event[i].id == m_select)
			{	
				break;
			}
		}
		float length = 82* _num + _num-1;
		m_items.Clear ();
		for(int i = 0; i < explore_gui._instance._msg_event.Count;++i)
		{	
			int id = explore_gui._instance._msg_event[i].ts_id;
			s_t_manyou t_manyou = game_data._instance.get_t_manyou(id);
			GameObject temp = game_data._instance.ins_object_res ("ui/explore_qiyu_item");
			temp.transform.parent = m_scro.transform;
			temp.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			temp.transform.name = explore_gui._instance._msg_event[i].id.ToString();
			temp.transform.localScale = new Vector3(1,1,1);
			temp.transform.localPosition = new Vector3(-289,- i * 83 + 235,0);
			temp.transform.GetComponent<explore_qiyu_item>().m_event = explore_gui._instance._msg_event[i];
			temp.GetComponent<explore_qiyu_item>().reset();
			m_items.Add(temp);
		}
		if(length > m_scro.GetComponent<UIPanel>().GetViewSize().y)
		{	
			float offset = (float)(82*_num+(_num-1) - m_scro.GetComponent<UIPanel>().GetViewSize().y);
			m_scro.transform.localPosition = new Vector3 (0,offset, 0);
			m_scro.GetComponent<UIPanel>().clipOffset = new Vector2 (0,-offset);
		}
		if(m_items.Count > 0)
		{	
			for(int i = 0; i < m_items.Count;++i)
			{	
				m_items[i].transform.Find("select").gameObject.SetActive(false);
			}
			m_items[_num-1].transform.Find("select").gameObject.SetActive(true);
			qiyu();
		}
	}
	
	public void reset()
	{	
		if(m_scro.GetComponent<SpringPanel>() != null)
		{	
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_scro);
		m_items.Clear ();
		for(int i = 0; i < explore_gui._instance._msg_event.Count;++i)
		{	
			int id = explore_gui._instance._msg_event[i].ts_id;
			s_t_manyou t_manyou = game_data._instance.get_t_manyou(id);
			GameObject temp = game_data._instance.ins_object_res ("ui/explore_qiyu_item");
			temp.transform.parent = m_scro.transform;
			temp.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			temp.transform.name = explore_gui._instance._msg_event[i].id.ToString();
			temp.transform.localScale = new Vector3(1,1,1);
			temp.transform.localPosition = new Vector3(-289,- i * 83 + 235,0);
			temp.transform.GetComponent<explore_qiyu_item>().m_event = explore_gui._instance._msg_event[i];
			temp.GetComponent<explore_qiyu_item>().reset();
			m_items.Add(temp);
		}
		if(m_items.Count > 0)
		{	
			for(int i = 0; i < m_items.Count;++i)
			{	
				m_items[i].transform.Find("select").gameObject.SetActive(false);
			}
			m_select = int.Parse(m_items[0].name);
			m_items[0].transform.Find("select").gameObject.SetActive(true);
			qiyu();
		}
	}

	void qiyu()
	{	
		m_chonzghi_panel.SetActive(false);
		m_question_panel.SetActive(false);
		m_shop_panel.SetActive(false);
		m_baoxiang_panel.SetActive(false);
		m_boss_panel.SetActive(false);
		for(int i = 0; i < explore_gui._instance._msg_event.Count;++i)
		{	
			if(m_select == explore_gui._instance._msg_event[i].id)
			{	
				m_event = explore_gui._instance._msg_event[i];
				break;
			}
		}
		long _time = (long)(m_event.qiyu_time - timer.now());
		if(_time <= 0)
		{
            root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("explore_qiyu_gui.cs_216_45"));
			return;
		}
		s_t_manyou t_manyou = game_data._instance.get_t_manyou (m_event.ts_id);
		if(t_manyou.id == 3001)
		{	
			shop();
			m_shop_panel.SetActive(true);
		}
		else if(t_manyou.id == 3002)
		{	
			baoxiang();
			m_baoxiang_panel.SetActive(true);
		}
		else if(t_manyou.id == 3003)
		{	
			boss();
			m_boss_panel.SetActive(true);
		}
		else if(t_manyou.id == 3004)
		{	
			queston();
			m_question_panel.SetActive(true);
		}
		else if(t_manyou.id == 3005)
		{	
			chongzhi();
			m_chonzghi_panel.SetActive(true);
		}
		InvokeRepeating ("time", 0.0f, 1.0f);
	}

	void shop()
	{	
		m_huodong_time = m_shop_time;
		m_end_time = m_event.qiyu_time;
		string color = "[ff0000]";
		sys._instance.remove_child (m_shop_icon);
		t_manyou_qiyu = game_data._instance.get_t_manyou_qiyu (m_event.qiyu_id);
		GameObject obj = icon_manager._instance.create_reward_icon (t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1
		                                                            ,t_manyou_qiyu.rewards[0].value2,t_manyou_qiyu.rewards[0].value3);
		obj.transform.parent = m_shop_icon.transform;
		obj.transform.localPosition = new Vector3 (0, 0, 0);
		obj.transform.localScale = new Vector3 (1, 1, 1);
		if(t_manyou_qiyu.def2 <= sys._instance.m_self.m_t_player.jewel)
		{	
			color = sys._instance.get_res_color(2);
		}
		m_old_price.GetComponent<UILabel>().text = t_manyou_qiyu.def1.ToString ();
		m_now_price.GetComponent<UILabel>().text = color + t_manyou_qiyu.def2.ToString ();
		float discount = (float)t_manyou_qiyu.def3 / 10.0f;
		discount = (float)decimal.Round ((decimal)discount, 2);
		if(discount >= 10.0f || discount <= 0)
		{	
			m_discount.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			m_discount.GetComponent<UILabel>().text = sys._instance.discountChange(discount) + game_data._instance.get_t_language ("dsign_week_sub.cs_52_80");//折
			m_discount.transform.parent.gameObject.SetActive(true);
		}
		color = "[ff0000]";
		if(sys._instance.m_self.m_t_player.jewel >= 10)
		{	
			color = sys._instance.get_res_color(2);
		}
		m_fresh_price.GetComponent<UILabel>().text = color + "x10";
		m_shop_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1
		                                                                         ,t_manyou_qiyu.rewards[0].value2,t_manyou_qiyu.rewards[0].value3);
		m_jifen.GetComponent<UILabel>().text = game_data._instance.get_t_language ("explore_qiyu_gui.cs_285_42") + t_manyou_qiyu.def2.ToString ();//获得积分:
	}

	void baoxiang()
	{	
		m_huodong_time = m_bx_time;
		m_end_time = m_event.qiyu_time;
		t_manyou_qiyu = game_data._instance.get_t_manyou_qiyu (m_event.qiyu_id);
		if(sys._instance.m_self.m_t_player.jewel < 200)
		{	
			m_bx_price.GetComponent<UILabel>().text = "[ff0000]200";
		}
		else
		{
			m_bx_price.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + "200";
		}
		m_bx_free.GetComponent<UISprite>().spriteName = "purple_box_b1";
		m_bx_free_button.GetComponent<UISprite>().set_enable (true);
		m_bx_free_button.GetComponent<BoxCollider>().enabled = true;
		m_bx_recharge.GetComponent<UISprite>().spriteName = "gold_box_b1";
		m_bx_recharge_button.GetComponent<UISprite>().set_enable (true);
		m_bx_recharge_button.GetComponent<BoxCollider>().enabled = true;
		if(m_event.qiyu_arg1 == 1)
		{	
			m_bx_free.GetComponent<UISprite>().spriteName = "purple_box_b5";
			m_bx_free_button.GetComponent<UISprite>().set_enable (false);
			m_bx_free_button.GetComponent<BoxCollider>().enabled = false;
		}
		if(m_event.qiyu_arg2 == 1)
		{	
			m_bx_recharge.GetComponent<UISprite>().spriteName = "gold_box_b5";
			m_bx_recharge_button.GetComponent<UISprite>().set_enable (false);
			m_bx_recharge_button.GetComponent<BoxCollider>().enabled = false;
		}
	}

	void boss()
	{	
		sys._instance.remove_child (m_boss_icon);
		m_huodong_time = m_boss_time;
		m_end_time = m_event.qiyu_time;
		t_manyou_qiyu = game_data._instance.get_t_manyou_qiyu (m_event.qiyu_id);
		if(sys._instance.m_self.m_t_player.tili < t_manyou_qiyu.def1)
		{	
			m_boss_tili.GetComponent<UILabel>().text = "[ff0000]x" + t_manyou_qiyu.def1;
		}
		else
		{
			m_boss_tili.GetComponent<UILabel>().text = sys._instance.get_res_color(3) + "x" + t_manyou_qiyu.def1;
		}
		m_boss_name.text = game_data._instance.get_t_language ("explore_qiyu_gui.cs_335_21");//水电费
		m_boss_bar.GetComponent<UIProgressBar>().value = (float)m_event.qiyu_arg3/(float)m_event.qiyu_arg4;
		m_boss_text.GetComponent<UILabel>().text = m_event.qiyu_arg3.ToString () + "/" + m_event.qiyu_arg4.ToString ();
		m_boss_reward_name.text = sys._instance.get_res_color (t_manyou_qiyu.rewards[0].value1) +  game_data._instance.get_t_resource(t_manyou_qiyu.rewards[0].value1).name;
		m_boss_reward_num.text = sys._instance.get_res_color (t_manyou_qiyu.rewards [0].value1) + "x" + t_manyou_qiyu.rewards [0].value2;
		m_boss_reward_small.spriteName = game_data._instance.get_t_resource(t_manyou_qiyu.rewards[0].value1).smallicon;
		m_boss_sprite.spriteName = game_data._instance.get_t_class (m_event.qiyu_arg1).icon;
		GameObject obj = icon_manager._instance.create_reward_icon(t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1,t_manyou_qiyu.rewards[0].value2,t_manyou_qiyu.rewards[0].value3);
		obj.transform.parent = m_boss_icon.transform;
		obj.transform.localPosition = new Vector3(0,0,0);
		obj.transform.localScale = new Vector3(1,1,1);
	}

	void queston()
	{	
		sys._instance.remove_child (m_correct_icon);
		sys._instance.remove_child (m_error_icon);
		m_huodong_time = m_question_time;
		m_end_time = m_event.qiyu_time;
		t_manyou_qiyu = game_data._instance.get_t_manyou_qiyu (m_event.qiyu_id);
		GameObject obj = icon_manager._instance.create_reward_icon(t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1,t_manyou_qiyu.rewards[0].value2,t_manyou_qiyu.rewards[0].value3);
		obj.transform.parent = m_correct_icon.transform;
		obj.transform.localPosition = new Vector3(0,0,0);
		obj.transform.localScale = new Vector3(1,1,1);

		GameObject obj1 = icon_manager._instance.create_reward_icon(t_manyou_qiyu.rewards[0].type,t_manyou_qiyu.rewards[0].value1,t_manyou_qiyu.rewards[0].value2/2,t_manyou_qiyu.rewards[0].value3);
		obj1.transform.parent = m_error_icon.transform;
		obj1.transform.localPosition = new Vector3(0,0,0);
		obj1.transform.localScale = new Vector3(1,1,1);

		m_correct_name.text = sys._instance.get_res_color (t_manyou_qiyu.rewards [0].value1) + game_data._instance.get_t_resource (t_manyou_qiyu.rewards [0].value1).name;
		m_error_name.text = sys._instance.get_res_color (t_manyou_qiyu.rewards [0].value1) + game_data._instance.get_t_resource (t_manyou_qiyu.rewards [0].value1).name;
		s_t_manyou_dati t_manyou_dati = game_data._instance.get_t_manyou_dati (m_event.qiyu_id);
		List<s_t_question> dati_questions = new List<s_t_question>();
		for(int i = 0; i <t_manyou_dati.questions.Count;++i)
		{	
			dati_questions.Add(t_manyou_dati.questions[i]);
		}
		m_question.text = t_manyou_dati.question;
		m_question_items[0].transform.name = t_manyou_dati.questions[(int)m_event.qiyu_arg1 -1].id.ToString();
		m_question_items[0].transform.Find("question").GetComponent<UILabel>().text = t_manyou_dati.questions[(int)m_event.qiyu_arg1 -1].answer;
		m_question_items[1].transform.name = t_manyou_dati.questions[(int)m_event.qiyu_arg2 -1].id.ToString();
		m_question_items[1].transform.Find("question").GetComponent<UILabel>().text = t_manyou_dati.questions[(int)m_event.qiyu_arg2 -1].answer;
		m_question_items[2].transform.name = t_manyou_dati.questions[(int)m_event.qiyu_arg3 -1].id.ToString();
		m_question_items[2].transform.Find("question").GetComponent<UILabel>().text = t_manyou_dati.questions[(int)m_event.qiyu_arg3 -1].answer;
		sys._instance.remove_child (m_question_role);
		GameObject _head = game_data._instance.ins_object_res("ui/npc_half/" + t_manyou_dati.half_image);
		if (_head)
		{	
			_head.transform.parent = m_question_role.transform;
			_head.transform.localPosition = new Vector3 (0, 0, 0);
			_head.transform.localScale = new Vector3 (1, 1, 1);
			_head.GetComponent<UITexture>().width = 512;
			_head.GetComponent<UITexture>().height = 512;
			_head.GetComponent<UITexture>().depth = 2;
		}
	}

	void chongzhi()
	{	
		for(int i = 0; i < m_icons.Count;++i)
		{	
			sys._instance.remove_child(m_icons[i]);
			m_icons[i].SetActive(false);
			m_names[i].SetActive(false);
		}
		m_huodong_time = m_chongzhi_time;
		m_end_time = m_event.qiyu_time;
		t_manyou_qiyu = game_data._instance.get_t_manyou_qiyu (m_event.qiyu_id);
		for (int i = 0; i < t_manyou_qiyu.rewards.Count; ++i) 
		{	
			GameObject obj = icon_manager._instance.create_reward_icon (t_manyou_qiyu.rewards[i].type, t_manyou_qiyu.rewards[i].value1, t_manyou_qiyu.rewards[i].value2, t_manyou_qiyu.rewards[i].value3);
			obj.transform.parent = m_icons[i].transform;
			obj.transform.localPosition = new Vector3 (0, 0, 0);
			obj.transform.localScale = new Vector3 (1, 1, 1);
			m_icons[i].SetActive(true);
			m_names[i].transform.GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_manyou_qiyu.rewards[i].type, t_manyou_qiyu.rewards[i].value1, t_manyou_qiyu.rewards[i].value2, t_manyou_qiyu.rewards[i].value3);
			m_names[i].SetActive(true);
		}
        s_t_recharge t_recharge = null;
        for (int i = 0; i < game_data._instance.m_dbc_recharge.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_recharge.get(0, i));
            s_t_recharge _t_recharge = game_data._instance.get_t_recharge(id);
            if (_t_recharge.id == t_manyou_qiyu.def1)
            {
                t_recharge = _t_recharge;
                break;
            }
        }
        if (t_recharge != null)
        {
            m_chongzhi_title.text = string.Format(game_data._instance.get_t_language("explore_qiyu_gui.cs_414_40"), sys._instance.get_czbl(t_recharge.vippt) + "");//单笔充值额度为 - {0}元
            m_chongzhi_tishi.text = string.Format(game_data._instance.get_t_language("explore_qiyu_gui.cs_415_40"), sys._instance.get_czbl(t_recharge.vippt) + "");
        }
    
        if(m_event.qiyu_arg1 == 1)
		{	
			m_recharge_button.SetActive(false);
			m_liqu_button.SetActive(true);
		}
		else
		{
			m_liqu_button.SetActive(false);
			m_recharge_button.SetActive(true);
		}
	}

	void select(GameObject obj)
	{	
		m_select = int.Parse (obj.transform.name);
		for(int i = 0; i < m_items.Count;++i)
		{	
			m_items[i].transform.Find("select").gameObject.SetActive(false);
		}
		for(int i = 0; i < m_items.Count;++i)
		{	
			int _id = int.Parse(m_items[i].transform.name);
			if(_id == m_select)
			{	
				m_items[i].transform.Find("select").gameObject.SetActive(true);
				break;
			}
		}
		qiyu ();
	}

	void answer(GameObject obj)
	{	
		long _time = (long)(m_event.qiyu_time - timer.now());
		if(_time <= 0)
		{	
			root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_qiyu_gui.cs_216_45"));
			return;
		}
		score = 0;
		dati_id = int.Parse (obj.transform.name);
		protocol.game.cmsg_huodong_tansuo_event _msg = new protocol.game.cmsg_huodong_tansuo_event ();
		_msg.id = m_select;
		_msg.type = dati_id;
		net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_event> (opclient_t.CMSG_HUODONG_MANYOU_EVENT, _msg);
	}

	public void click(GameObject obj)
	{	
		if(obj.transform.name == "refresh")
		{	
			score = 0;
			long _time = (long)(m_event.qiyu_time - timer.now());
			if(_time <= 0)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_qiyu_gui.cs_216_45"));
				return;
			}
			if(sys._instance.m_self.m_t_player.jewel < 10)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			protocol.game.cmsg_huodong_tansuo_event _msg = new protocol.game.cmsg_huodong_tansuo_event ();
			_msg.id = m_select;
			_msg.type = 0;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_event> (opclient_t.CMSG_HUODONG_MANYOU_EVENT_REFRESH, _msg);
		}
		else if(obj.transform.name == "get")
		{	
			long _time = (long)(m_event.qiyu_time - timer.now());
			if(_time <= 0)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_qiyu_gui.cs_216_45"));
				return;
			}
			if(sys._instance.m_self.m_t_player.jewel < t_manyou_qiyu.def2)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			score = t_manyou_qiyu.def2;
			protocol.game.cmsg_huodong_tansuo_event _msg = new protocol.game.cmsg_huodong_tansuo_event ();
			_msg.id = m_select;
			_msg.type = 0;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_event> (opclient_t.CMSG_HUODONG_MANYOU_EVENT, _msg);
		}
		else if(obj.transform.name == "ignore")
		{	
			score = 0;
			protocol.game.cmsg_huodong_tansuo_event _msg = new protocol.game.cmsg_huodong_tansuo_event ();
			_msg.id = m_select;
			_msg.type = 0;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_event> (opclient_t.CMSG_HUODONG_MANYOU_DEL, _msg);
		}
		else if(obj.transform.name == "box1")
		{	
			s_message mess = new s_message();
			mess.m_type = "show_expolre_reward_gui";
			mess.m_ints.Add(1);
			cmessage_center._instance.add_message(mess);
		}
		else if(obj.transform.name == "box2")
		{	
			s_message mess = new s_message();
			mess.m_type = "show_expolre_reward_gui";
			mess.m_ints.Add(2);
			cmessage_center._instance.add_message(mess);
		}
		else if(obj.transform.name == "kaiqi_recharge")
		{	
			long _time = (long)(m_event.qiyu_time - timer.now());
			if(_time <= 0)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_qiyu_gui.cs_216_45"));
				return;
			}
			score = 200;
			if(sys._instance.m_self.m_t_player.jewel < 200)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			bx_seclect = 1;
			protocol.game.cmsg_huodong_tansuo_event _msg = new protocol.game.cmsg_huodong_tansuo_event ();
			_msg.id = m_select;
			_msg.type = 2;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_event> (opclient_t.CMSG_HUODONG_MANYOU_EVENT, _msg);
		}
		else if(obj.transform.name == "kaiqi_free")
		{	
			long _time = (long)(m_event.qiyu_time - timer.now());
			if(_time <= 0)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_qiyu_gui.cs_216_45"));
				return;
			}
			bx_seclect = 0;
			score = 0;
			protocol.game.cmsg_huodong_tansuo_event _msg = new protocol.game.cmsg_huodong_tansuo_event ();
			_msg.id = m_select;
			_msg.type = 1;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_event> (opclient_t.CMSG_HUODONG_MANYOU_EVENT, _msg);
		}
		else if(obj.transform.name == "duixing")
		{	
			s_message _message = new s_message();
			_message.m_type = "show_duixing_gui";
			_message.m_bools.Add(true);
			cmessage_center._instance.add_message(_message);
		}
		else if(obj.transform.name == "zhan")
		{	
			long _time = (long)(m_event.qiyu_time - timer.now());
			if(_time <= 0)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_qiyu_gui.cs_216_45"));
				return;
			}
			score = 0;
			protocol.game.cmsg_huodong_tansuo_event _msg = new protocol.game.cmsg_huodong_tansuo_event ();
			_msg.id = m_select;
			_msg.type = 0;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_event> (opclient_t.CMSG_HUODONG_MANYOU_EVENT, _msg);
		}
		else if(obj.transform.name == "recharge")
		{	
			long _time = (long)(m_event.qiyu_time - timer.now());
			if(_time <= 0)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_qiyu_gui.cs_216_45"));
				return;
			}
			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);
		}
		else if(obj.transform.name == "liqu")
		{	
			long _time = (long)(m_event.qiyu_time - timer.now());
			if(_time <= 0)
			{	
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_qiyu_gui.cs_216_45"));
				return;
			}
			score = 0;
			protocol.game.cmsg_huodong_tansuo_event _msg = new protocol.game.cmsg_huodong_tansuo_event ();
			_msg.id = m_select;
			_msg.type = 0;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_event> (opclient_t.CMSG_HUODONG_MANYOU_EVENT, _msg);
		}
		else if(obj.transform.name == "qiyu_close")
		{	
			explore_gui._instance.is_press = true;
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	void remove_qiyu()
	{	
		for(int i = 0; i < explore_gui._instance._msg_event.Count;++i)
		{	
			if(m_select == explore_gui._instance._msg_event[i].id)
			{	
				explore_gui._instance._msg_event.RemoveAt(i);
				break;
			}
		}
		if(explore_gui._instance._msg_event.Count == 0)
		{	
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _mes = new s_message();
			_mes.m_type = "update_explore_gui";
			cmessage_center._instance.add_message(_mes);
		}
	}
	
	void IMessage.message (s_message message)
	{	
		if(message.m_type == "refresh_explore_chongzhi")
		{	
			int rmb = (int)message.m_ints[0];
			if(rmb == t_manyou_qiyu.def1)
			{	
				m_event.qiyu_arg1 = 1;
			}
			chongzhi();
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{	
		if (message.m_opcode == opclient_t.CMSG_HUODONG_MANYOU_EVENT)
		{	
			protocol.game.smsg_huodong_tansuo_event _msg1 = net_http._instance.parse_packet<protocol.game.smsg_huodong_tansuo_event>(message.m_byte);
			for (int i = 0; i < _msg1.equips.Count; i++)
			{	
				sys._instance.m_self.add_equip(_msg1.equips[i], true);
			}
			for (int i = 0; i < _msg1.treasures.Count; i++)
			{	
				sys._instance.m_self.add_treasure(_msg1.treasures[i], true);
			}
			for(int i = 0; i < _msg1.types.Count;++i)
			{	
				if(t_manyou_qiyu.type == 3001 || t_manyou_qiyu.type == 3005)
				{	
					sys._instance.m_self.add_reward(_msg1.types[i],_msg1.value1s[i],_msg1.value2s[i],_msg1.value3s[i],true,game_data._instance.get_t_language ("explore_qiyu_gui.cs_663_108"));//太空漫游事件
				}
				else
				{
                    sys._instance.m_self.add_reward(_msg1.types[i], _msg1.value1s[i], _msg1.value2s[i], _msg1.value3s[i], false, game_data._instance.get_t_language ("explore_qiyu_gui.cs_663_108"));//太空漫游事件
				}
			}
			explore_gui._instance.score += score;
			if(t_manyou_qiyu.type == 3003)
			{	
				sys._instance.m_self.sub_att(e_player_attr.player_tili,t_manyou_qiyu.def1);
				battle_logic_ex._instance.set_explore_fight_end(_msg1);
				sys._instance.m_game_state = "buttle";
				sys._instance.load_scene("ts_chapter01");
			}
			if(t_manyou_qiyu.type == 3002)
			{	
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = _msg1.types[0];
				t_reward.value1 = _msg1.value1s[0];
				t_reward.value2 = _msg1.value2s[0];
				t_reward.value3 = _msg1.value3s[0];
				string desc = "";
				bool flag = false;
				if(bx_seclect == 0)
				{	
					m_event.qiyu_arg1 = 1;
					desc = game_data._instance.get_t_language ("explore_qiyu_gui.cs_690_12");//恭喜主人，获得如下奖励
				}
				if(bx_seclect == 1)
				{	
					m_event.qiyu_arg2 = 1;
					sys._instance.m_self.sub_att(e_player_attr.player_jewel,200,game_data._instance.get_t_language ("explore_gui.cs_610_76"));//太空漫游消耗
					desc = game_data._instance.get_t_language ("explore_qiyu_gui.cs_696_12");//恭喜主人，获得了200积分，以及如下奖励
				}
				if(t_manyou_qiyu.def2 == 1)
				{	
					flag = true;
				}
				s_message _message = new s_message();
				_message.m_type = "show_explore_wuping";
				_message.m_object.Add(t_reward);
				_message.m_string.Add(desc);
				_message.m_bools.Add(flag);
				cmessage_center._instance.add_message(_message);
				baoxiang();
			}
			else if(t_manyou_qiyu.type == 3003 && _msg1.result != 1)
			{	
				m_event.qiyu_arg3 = _msg1.hp;
				boss();
			}
			else if(t_manyou_qiyu.type == 3004)
			{	
				string desc = "";
				if(dati_id == t_manyou_qiyu.def1)
				{	
					desc = game_data._instance.get_t_language ("explore_qiyu_gui.cs_720_12");
				}
				else
				{
                    desc = game_data._instance.get_t_language("explore_qiyu_gui.cs_724_12");
				}
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = _msg1.types[0];
				t_reward.value1 = _msg1.value1s[0];
				t_reward.value2 = _msg1.value2s[0];
				t_reward.value3 = _msg1.value3s[0];
				s_message _message = new s_message();
				_message.m_type = "show_explore_wuping";
				_message.m_object.Add(t_reward);
				_message.m_string.Add(desc);
				cmessage_center._instance.add_message(_message);
			}
			if(t_manyou_qiyu.type == 3001)
			{	
				sys._instance.m_self.sub_att(e_player_attr.player_jewel,t_manyou_qiyu.def2,game_data._instance.get_t_language ("explore_gui.cs_610_76"));//太空漫游消耗
				remove_qiyu();
				reset();
			}
			else if(t_manyou_qiyu.type == 3002 && m_event.qiyu_arg1 == 1 && m_event.qiyu_arg2 == 1)
			{	
				remove_qiyu();
				reset();
			}
			else if(t_manyou_qiyu.type == 3003&&_msg1.result == 1)
			{	
				remove_qiyu();
				reset();
			}
			else if(t_manyou_qiyu.type == 3005 || t_manyou_qiyu.type == 3004)
			{	
				remove_qiyu();
				reset();
			}

			if(score > 0)
			{	
				s_message mess = new s_message();
				mess.m_type = "update_explore_gui";
				cmessage_center._instance.add_message(mess);
			}
		}
		if (message.m_opcode == opclient_t.CMSG_HUODONG_MANYOU_EVENT_REFRESH)
		{	
			protocol.game.smsg_huodong_tansuo_event_refresh _msg1 = net_http._instance.parse_packet<protocol.game.smsg_huodong_tansuo_event_refresh>(message.m_byte);
			m_event.qiyu_id = _msg1.qiyu_id;
			sys._instance.m_self.sub_att(e_player_attr.player_jewel,10,game_data._instance.get_t_language ("explore_qiyu_gui.cs_770_62"));//太空漫游刷新消耗
			shop();
		}
		if (message.m_opcode == opclient_t.CMSG_HUODONG_MANYOU_DEL)
		{	
			protocol.game.smsg_success _msg1 = net_http._instance.parse_packet<protocol.game.smsg_success>(message.m_byte);
			if(_msg1.success)
			{	
				remove_qiyu();
				reset();
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
