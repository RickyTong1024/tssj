
using UnityEngine;
using System.Collections;

public class transport_player_gui : MonoBehaviour {

	public protocol.game.msg_yb_player m_yb_player;
	public GameObject m_name;
	public GameObject m_level;
	public GameObject m_bf;
	public GameObject m_ship;
	public GameObject m_qiang;
	public GameObject m_time;
	public GameObject m_reward;
	public GameObject m_reward1;
	public GameObject m_no_reward;
	public GameObject m_icon;
    public UILabel m_ship_kind1;
    public UILabel m_ship_kind2;
    public UILabel m_ship_kind3;
    public UILabel m_ship_kind4;
    public UILabel m_ship_kind5;

	public UILabel m_role_level;
	public UILabel m_role_zl;
	public UILabel m_role_ship;
	public UILabel m_rob_num;
	public UILabel m_ship_time;
	public UILabel m_goods;
	public UILabel m_rob_ship;
	public UILabel m_desc;
	public UILabel m_rob_reward;
	public UILabel m_get_Label;
	public UILabel m_ship_condition;

	// Use this for initialization
	void Start () {
	}

	void OnEnable()
	{
		InvokeRepeating("time", 0.0f,1.0f);
		reset();
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}

	void click(GameObject obj)
	{
		if(obj.transform.name == "get")
		{
			if (sys._instance.m_self.m_t_player.ybq_finish_num >= 5)
			{
				string s = game_data._instance.get_t_language ("transport_player_gui.cs_56_15");//[ffc882]今日拦截次数已用完
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (m_yb_player.player_ybq_num >= 2)
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("transport_player_gui.cs_62_28");//该玩家已被截光了
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (sys._instance.m_self.m_t_player.ybq_last_time + 600000 > timer.now())
			{
				string s = game_data._instance.get_t_language ("transport_player_gui.cs_68_15");//[ffc882]拦截冷却中
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			if (sys._instance.m_self.m_guid == m_yb_player.player_guid)
			{
				string s = game_data._instance.get_t_language ("transport_player_gui.cs_74_15");//[ffc882]无法拦截自己
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}

			protocol.game.cmsg_yb_ybq_fight_end _msg = new protocol.game.cmsg_yb_ybq_fight_end ();			
			_msg.target_guid = m_yb_player.player_guid;	
			net_http._instance.send_msg<protocol.game.cmsg_yb_ybq_fight_end> (opclient_t.CMSG_YB_YBQ_FIGHT_END, _msg);

			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "hide")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	void reset()
	{
		s_t_yb t_yb = game_data._instance.get_t_yb (m_yb_player.player_type);
        if (t_yb.type == 0)
        {
            m_ship.GetComponent<UILabel>().gradientTop = m_ship_kind1.gradientTop;
            m_ship.GetComponent<UILabel>().gradientBottom = m_ship_kind1.gradientBottom;
            m_ship.GetComponent<UILabel>().applyGradient = true;

        }
        else if (t_yb.type == 1)
        {
            m_ship.GetComponent<UILabel>().gradientTop = m_ship_kind2.gradientTop;
            m_ship.GetComponent<UILabel>().gradientBottom = m_ship_kind2.gradientBottom;
            m_ship.GetComponent<UILabel>().applyGradient = true;

        }
        else if (t_yb.type == 2)
        {
            m_ship.GetComponent<UILabel>().gradientTop = m_ship_kind3.gradientTop;
            m_ship.GetComponent<UILabel>().gradientBottom = m_ship_kind3.gradientBottom;
            m_ship.GetComponent<UILabel>().applyGradient = true;

        }
        else if (t_yb.type == 3)
        {
            m_ship.GetComponent<UILabel>().gradientTop = m_ship_kind4.gradientTop;
            m_ship.GetComponent<UILabel>().gradientBottom = m_ship_kind4.gradientBottom;
            m_ship.GetComponent<UILabel>().applyGradient = true;

        }
        else if (t_yb.type == 4)
        {
            m_ship.GetComponent<UILabel>().gradientTop = m_ship_kind5.gradientTop;
            m_ship.GetComponent<UILabel>().gradientBottom = m_ship_kind5.gradientBottom;
            m_ship.GetComponent<UILabel>().applyGradient = true;

        }
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(m_yb_player.player_achieve) + m_yb_player.player_name;
		sys._instance.remove_child (m_icon);
        GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_yb_player.player_template, m_yb_player.player_achieve, m_yb_player.player_vip, m_yb_player.player_nalflag);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		m_level.GetComponent<UILabel>().text = m_yb_player.player_level.ToString();
        m_bf.GetComponent<UILabel>().text = sys._instance.value_to_wan(m_yb_player.player_bf);
		m_ship.GetComponent<UILabel>().text = t_yb.name;
		m_qiang.GetComponent<UILabel>().text = m_yb_player.player_ybq_num.ToString() + "/2";
		s_t_exp t_exp = game_data._instance.get_t_exp (m_yb_player.player_level);

        int per = t_yb.per - (sys._instance.m_self.m_t_player.level - m_yb_player.player_level);
        if (per >= t_yb.per)
        {
            per = t_yb.per;
        }
        else if (per < t_yb.min_per)
        {
            per = t_yb.min_per;
        }

        m_reward1.GetComponent<UILabel>().text = (t_exp.yuanli * t_yb.yuanli).ToString();
        string _des = (t_exp.yuanli * t_yb.yuanli * per / 100).ToString();
		m_reward.GetComponent<UILabel>().text = _des;
		m_no_reward.SetActive(false);
		m_reward.transform.parent.gameObject.SetActive(true);
        if (m_yb_player.player_ybq_num >= 2)
        {
            string s = "[00ff00]" + game_data._instance.get_t_language ("transport_player_gui.cs_62_28");//该玩家已被截光了
            m_no_reward.GetComponent<UILabel>().text = s;
            m_reward.transform.parent.gameObject.SetActive(false);
            m_no_reward.SetActive(true);
        }
	}

	void time()
	{
		long ltime = 0;
		s_t_yb t_yb = game_data._instance.get_t_yb (m_yb_player.player_type);
		if (m_yb_player.jiasu_time > 0)
		{
			ltime = ((long)t_yb.time - (long)(m_yb_player.jiasu_time - m_yb_player.start_time)) / 2 - (long)(timer.now() - m_yb_player.jiasu_time);
		}
		ltime = (long)t_yb.time - (long)(timer.now () - m_yb_player.start_time);
		if (ltime < 0)
		{
			ltime = 0;
		}
		m_time.GetComponent<UILabel>().text = timer.get_time_show(ltime);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
