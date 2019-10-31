
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class main_gui : MonoBehaviour, IMessage { 
    public GameObject m_name;
    public GameObject m_level;
    public GameObject m_vip_level;

    public GameObject m_exp_bar;
    public GameObject m_exp_text;
    public GameObject m_tili_text;
    public GameObject m_nl_text;
    public GameObject m_wan_fight;

    public GameObject m_gold;
    public GameObject m_zuanshi;
    public GameObject m_fighting;
    public GameObject m_chong_zhi;
    public GameObject m_zs_name;
    public GameObject m_player_name;
    public GameObject m_touxiang;
    public GameObject m_kaifu;
    public GameObject m_online_reward;
    public GameObject m_random_event;
    public GameObject m_sign;
    public GameObject m_zhan_niang;
    public GameObject m_gonggao;
    public GameObject m_jieri;
    public GameObject m_jieri_Label;
    public GameObject m_tanbao;

    /// <summary>
    /// 图标
    /// </summary>
    public GameObject m_shou_chong;
    public GameObject m_huigui;
    public GameObject m_kaifu_ex;
    public GameObject m_vr;
    public GameObject m_zhaomu_;
    public UISprite m_shop_back;
    public GameObject m_huiyi_shop;
    public GameObject m_chongwu_shop;
    public GameObject m_infinite_shop;
    public GameObject m_sign_icon;
    public GameObject m_sign_icon1;
    public GameObject m_fight_word;
    public GameObject m_finger_pointer;

    public GameObject m_gua_ji;
    string[] m_icon_name = { "xzjm_zsjyszwan_cnj", "xzjm_zsjyszwan_cn", "xzjm_zsjyszwan" };
    public bool m_show = true;

    public bool m_need_update = false;

    private bool m_show_tili = true;
    private bool m_show_nl = true;

    private float m_shake = 0.0f;
    public UILabel m_youjian;
    public UILabel m_paihangbang;
    const float time = 300f;
    private string m_wan;
    public List<GameObject> tr_icons;
    public List<GameObject> tl_icons;
    // Use this for initialization
    void Start()
    {
        Vector3 temp =  m_youjian.transform.TransformDirection(m_youjian.transform.localPosition);
        m_wan = game_data._instance.get_t_language("main_gui.cs_60_16");//万        update_ui();
        update_ui();
        show();
        if (game_data._instance.m_language == e_language.English)
        {
            m_fight_word.GetComponent<UISprite>().spriteName = "xzjm_zsjxxl11";
        }
        else if (game_data._instance.m_language == e_language.Simplified)
        {
            m_fight_word.GetComponent<UISprite>().spriteName = "xzjm_zsjxxl11_cnj";
        }
        else
        {
            m_fight_word.GetComponent<UISprite>().spriteName = "xzjm_zsjxxl11_cn";
        }
        cmessage_center._instance.add_handle(this);
        m_random_event.SetActive(false);
        this.InvokeRepeating("yuehui_show", time, time);
        if (sys._instance.m_self.m_t_player.level >= 15 )
        {
            m_finger_pointer.SetActive(false);
        }
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void OnEnable()
    {
        this.InvokeRepeating("tili_show", 0.0f, 2.0f);
        if (m_show)
        {
            show();
        }
        else
        {
            hide();
        }
    }
    void yuehui_show()
    {
        List<uint> _ids = new List<uint>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.role_template_ids.Count; i++)
        {
            if (game_data._instance.get_t_class((int)sys._instance.m_self.m_t_player.role_template_ids[i]).color >= 4)
            {
                _ids.Add(sys._instance.m_self.m_t_player.role_template_ids[i]);
            }
        }
        if (_ids.Count == 0)
        {
            return;
        }
        m_random_event.SetActive(true);
    }
    void OnDisable()
    {
        if (root_gui._instance != null && root_gui._instance.m_talk != null)
        {
            root_gui._instance.m_talk.SetActive(false);
        }
        this.CancelInvoke("tili_show");
    }
    void show_gonggao()
    {
        if (m_gonggao.GetComponent<TweenWidth>() != null)
        {
            DestroyImmediate(m_gonggao.GetComponent<TweenWidth>());
        }
        Transform label = m_gonggao.transform.Find("Panel").Find("Label");
        if (label.GetComponent<TweenPosition>() != null)
        {
            DestroyImmediate(label.GetComponent<TweenPosition>());
        }
        label.localPosition = new Vector3(360, 0, 0);
        m_gonggao.SetActive(true);
        m_gonggao.GetComponent<UISprite>().width = 0;
        TweenWidth width = TweenWidth.Begin(m_gonggao.GetComponent<UISprite>(), 1f, 719);
        width.AddOnFinished(playgonggao);

    }
    void playgonggao()
    {

        Transform label = m_gonggao.transform.Find("Panel").Find("Label");
        label.localPosition = new Vector3(360, 0, 0);
        TweenPosition temp = TweenPosition.Begin(label.gameObject, 6f, new Vector3(360 - m_gonggao.GetComponent<UISprite>().width - label.GetComponent<UILabel>().width, 0, 0));
        temp.AddOnFinished(finsh);
    }
    void finsh()
    {
        m_gonggao.SetActive(false);
    }
    void tili_show()
    {
        last_time();
        s_t_resource res = game_data._instance.get_t_resource(3);
        if (m_show_tili == true)
        {
            int _tili = sys._instance.m_self.get_max_tili();
            m_tili_text.GetComponent<UILabel>().text = res.namecolor + sys._instance.value_to_wan((long)sys._instance.m_self.m_t_player.tili) + "/" + sys._instance.value_to_wan((long)_tili);

        }
        else
        {
            long _time = 360000 - (int)(timer.now() - sys._instance.m_self.m_t_player.last_tili_time);
            m_tili_text.GetComponent<UILabel>().text = res.namecolor + timer.get_time_show(_time);

        }
        res = game_data._instance.get_t_resource(15);
        if (m_show_nl == true)
        {
            int _max_nl = sys._instance.m_self.get_max_nl();
            m_nl_text.GetComponent<UILabel>().text = res.namecolor + sys._instance.m_self.m_t_player.energy + "/" + _max_nl;
        }
        else
        {
            long _time = 1800000 - (int)(timer.now() - sys._instance.m_self.m_t_player.last_energy_time);
            m_nl_text.GetComponent<UILabel>().text = res.namecolor + timer.get_time_show(_time);
        }
    }

    void IMessage.net_message(s_net_message message)
    {

    }
    void IMessage.message(s_message message)
    {
        if (message.m_type == "update_player_att")
        {
            m_need_update = true;
        }
        else if (message.m_type == "hide_main_guide")
        {
            m_finger_pointer.SetActive(false);
        }
        else if (message.m_type == "show_main_guide")
        {
            m_finger_pointer.SetActive(true);
        }
        else if (message.m_type == "zs_name")
        {
            m_zs_name.transform.GetComponent<zs_name>().reset(1);
            m_zs_name.SetActive(true);
        }
        else if (message.m_type == "player_name")
        {
            m_player_name.transform.GetComponent<zs_name>().reset(0);
            m_player_name.SetActive(true);
        }
        else if (message.m_type == "change_danmu")
        {
            update_ui();
        }
        else if (message.m_type == "shake_ui")
        {
            m_shake = (float)message.m_floats[0] * 50;
        }
        else if (message.m_type == "daily_refresh")
        {
            m_need_update = true;
        }
        else if (message.m_type == "show_talk")
        {
            m_random_event.SetActive(false);
            CancelInvoke("yuehui_show");
            this.InvokeRepeating("yuehui_show", time, time);
        }
        else if (message.m_type == "nationality_change")
        {
            update_ui();
        }
    }

    public void show()
    {
        m_shake = 0;
        update_ui();
        m_show = true;
        this.gameObject.SetActive(true);
    }

    public void hide(bool msg = true)
    {
        s_message _mes = new s_message();
        _mes.m_type = "hide_main_gui_0";
        cmessage_center._instance.add_message(_mes);
        this.gameObject.SetActive(false);
        m_show = false;
    }

    public void hide_obj()
    {
        this.gameObject.SetActive(false);
    }

	public void update_ui()
    {
        if (sys._instance.m_self.m_t_player == null)
        {
            return;
        }

        m_level.GetComponent<UILabel>().text = "Lv." + sys._instance.m_self.m_t_player.level.ToString();
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name.ToString();
		m_touxiang.GetComponent<UISprite>().spriteName = player.get_touxiang((int)sys._instance.m_self.m_t_player.template_id);
        if (sys._instance.m_self.m_t_player.nalflag != 0)
        {
            if (platform_config_common.m_nationality > 0)
            {
                m_touxiang.transform.Find("gq").gameObject.SetActive(true);
                m_touxiang.transform.Find("gq").GetComponent<UISprite>().spriteName = sys._instance.returnfirstSpritename(sys._instance.m_self.m_t_player.nalflag);
            }
            else
            {
                m_touxiang.transform.Find("gq").gameObject.SetActive(false);
            }
        }

		int _gold = sys._instance.m_self.m_t_player.gold;
        s_t_resource res = game_data._instance.get_t_resource(1);
        m_gold.GetComponent<UILabel>().text = res.namecolor + sys._instance.value_to_wan(_gold);
		
        res = game_data._instance.get_t_resource(2);
        int zuan = sys._instance.m_self.m_t_player.jewel;
        m_zuanshi.GetComponent<UILabel>().text = res.namecolor + sys._instance.value_to_wan(zuan);
       
		s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
        m_vip_level.GetComponent<UILabel>().text = t_vip.desc;
        res = game_data._instance.get_t_resource(3);
		int _tili =  sys._instance.m_self.get_max_tili ();
		m_tili_text.GetComponent<UILabel>().text = res.namecolor +  sys._instance.value_to_wan((long)sys._instance.m_self.m_t_player.tili) + "/" +sys._instance.value_to_wan((long)_tili);
		res = game_data._instance.get_t_resource(15);
		int _max_nl = sys._instance.m_self.get_max_nl ();
		m_nl_text.GetComponent<UILabel>().text = res.namecolor +  sys._instance.m_self.m_t_player.energy + "/" + _max_nl;

		s_t_exp t_exp = game_data._instance.get_t_exp (sys._instance.m_self.m_t_player.level + 1);
		if (t_exp == null)
		{
			m_exp_bar.GetComponent<UISprite>().fillAmount = 1.0f;
			m_exp_text.GetComponent<UILabel>().text = "--/--";
		}
		else
		{
			m_exp_text.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.exp.ToString() + "/" + t_exp.exp;
			m_exp_bar.GetComponent<UISprite>().fillAmount = (float)sys._instance.m_self.m_t_player.exp / (float)t_exp.exp;
		}

		m_fighting.GetComponent<UILabel>().text = ((int)sys._instance.m_self.get_fighting()).ToString(); 
		m_wan_fight.SetActive(false);
		if((long)sys._instance.m_self.get_fighting() >= 100000)
		{
			long value =(long)sys._instance.m_self.get_fighting();
			m_fighting.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)sys._instance.m_self.get_fighting());
            m_wan_fight.GetComponent<UISprite>().spriteName = m_icon_name[(int)game_data._instance.m_language];
			m_wan_fight.SetActive(true);
		}

        // 充值和首充
        m_shou_chong.SetActive(false);
        m_chong_zhi.SetActive(false);
        if (sys._instance.m_self.m_t_player.first_reward <= 1)
        {
            if(platform_config_common.game_model == 2)
            {
                m_shou_chong.transform.Find("icon").GetComponent<UISprite>().spriteName = "small_ksjlic";
            }
            m_shou_chong.SetActive(true);
        }
        else
        {
            m_chong_zhi.SetActive(true);         
        }

        // 回归
        if (sys._instance.m_self.m_t_player.huodong_yxhg_tap != 1 || sys._instance.m_self.m_t_player.level <= 5)
        {
            m_huigui.SetActive(false);
        }
        else
        {
            m_huigui.SetActive(true);
        }
        // 7日活动
        if (timer.run_day(sys._instance.m_self.m_t_player.birth_time) >= 8  || sys._instance.m_self.m_t_player.level <= 5)
		{
			m_kaifu.SetActive(false);
		}
		else
		{
			m_kaifu.SetActive(true);
		}
        // 14日活动
		if (timer.run_day(sys._instance.m_self.m_t_player.birth_time) >= 15 || timer.run_day(sys._instance.m_self.m_t_player.birth_time) < 7 || sys._instance.m_self.m_t_player.level <= 5)
		{
			m_kaifu_ex.SetActive(false);
		}
		else
		{
			m_kaifu_ex.SetActive(true);
		}
        // 节日活动
		if(sys._instance.m_self.m_jieri_huodong == "" || sys._instance.m_self.m_t_player.level <= 5)
		{
			m_jieri.SetActive(false);
		}
		else
		{
			m_jieri_Label.GetComponent<UILabel>().text = sys._instance.m_self.m_jieri_huodong;
            m_jieri.SetActive(true);
        }
        // 探宝显示规则
        if (sys._instance.m_self.m_huodong_xhqd == 0 || sys._instance.m_self.m_t_player.level <= 5)
        {
            m_tanbao.SetActive(false);
		}
		else
		{
            m_tanbao.SetActive(true);
            if ((sys._instance.m_self.m_huodong_xhqd & 1) != 0)
            {
                m_tanbao.name = "tanbao";
                m_tanbao.transform.Find("icon").GetComponent<UISprite>().spriteName = "small_xytb";
                m_tanbao.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language("main_gui.cs_441_72");//幸运探宝
            }
            else if ((sys._instance.m_self.m_huodong_xhqd & 2) != 0)
            {
                m_tanbao.name = "czfp";
                m_tanbao.transform.Find("icon").GetComponent<UISprite>().spriteName = "small_czfp";
                m_tanbao.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language("main_gui.cs_447_72");//充值翻牌
            }
            else if ((sys._instance.m_self.m_huodong_xhqd & 4) != 0)
            {
                m_tanbao.name = "zhuanpan";
                m_tanbao.transform.Find("icon").GetComponent<UISprite>().spriteName = "small_yhzp";
                m_tanbao.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language("main_gui.cs_453_72");//银河转盘
            }
            else if ((sys._instance.m_self.m_huodong_xhqd & 8) != 0)
            {
                m_tanbao.name = "tansuo";
                m_tanbao.transform.Find("icon").GetComponent<UISprite>().spriteName = "small_tkmy";
                m_tanbao.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language("main_gui.cs_459_72");//太空漫步
            }
            else if ((sys._instance.m_self.m_huodong_xhqd & 16) != 0)
            {
                m_tanbao.name = "mofang";
                m_tanbao.transform.Find("icon").GetComponent<UISprite>().spriteName = "small_jgmf";
                m_tanbao.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language("main_gui.cs_465_84");//九宫魔方
            }
        }
        // 在线领奖
		s_t_online_reward tor = game_data._instance.get_t_online_reward (sys._instance.m_self.m_t_player.online_reward_index);
        if (tor != null)
        {
            m_online_reward.SetActive(true);
        }
        else
        {
            m_online_reward.SetActive(false);
        }

        // 商店页高度
        if (sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_memory)
        {
            m_huiyi_shop.SetActive(true);
            if (sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_pet)
            {
                m_chongwu_shop.SetActive(true);
                m_shop_back.width = 480;
            }
            else
            {
                m_chongwu_shop.SetActive(false);
                m_shop_back.width = 380;
            }
        }
        else
        {
            m_huiyi_shop.SetActive(false);
            m_shop_back.width = 280;
            m_chongwu_shop.SetActive(false);
        }

        // 签到显示明日香还是普通签到
        m_sign_icon.SetActive(true);
        m_sign_icon1.SetActive(false);
        if(sys._instance.m_self.m_t_player.daily_sign_index == 1)
        {
            m_sign_icon.SetActive(false);
            m_sign_icon1.SetActive(true);
            m_sign_icon1.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("main_gui.cs_594_84");//明日获得
        }
        if (sys._instance.m_self.m_t_player.daily_sign_index == 2 && sys._instance.m_self.m_t_player.daily_sign_reward != 2)
        {
            m_sign_icon.SetActive(false);
            m_sign_icon1.SetActive(true);
            m_sign_icon1.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("main_gui.cs_601_84");//今日获得
        }

        //版本特殊显示
        if (platform_config_common.game_model == 2 && sys._instance.m_self.m_t_player.level >= 10)
        {
            m_gua_ji.SetActive(true);
        }
        else
        {
            m_gua_ji.SetActive(false);
        }
        if (platform_config_common.game_model == 2)
        {
            m_infinite_shop.SetActive(true);
        }
        else
        {
            m_infinite_shop.SetActive(false);
        }

        // 调整所有图标位置
        int ix = 0;
        int iy_index = 0;
        for (int i = 0; i < tr_icons.Count; ++i)
        {
            if (tr_icons[i].activeSelf)
            {
                tr_icons[i].transform.localPosition = new Vector3(-16 + ix, -85 - 115 * (iy_index / 6), 0);
                ++iy_index;
                ix = - 100 * (iy_index % 6);               
            }
        }

        int lr_ix = 150;
        for (int i = 0; i < tl_icons.Count; i++)
        {
            if (tl_icons[i].activeSelf)
            {
                tl_icons[i].transform.localPosition = new Vector3(lr_ix, -155, 0);
                lr_ix += 100 ;
            }
        }
    }

	void last_time()
	{
		if(sys._instance.m_self == null)
		{
			return;
		}

		if(sys._instance.m_self.m_t_player == null)
		{
			return;
		}

		if(m_show_tili == true)
		{
			if (sys._instance.m_self.m_t_player.tili < sys._instance.m_self.get_max_tili())
			{
				m_show_tili = false;
			}
		}
		else
		{
			m_show_tili = true;
		}

		if(m_show_nl == true)
		{
			if (sys._instance.m_self.m_t_player.energy < sys._instance.m_self.get_max_nl())
			{
				m_show_nl = false;
			}
		}
		else
		{
			m_show_nl = true;
		}
	}

	// Update is called once per frame
	void Update () {

		if(sys._instance.m_self.m_t_player == null)
		{
			return;
		}

		if (m_need_update)
		{
			m_need_update = false;
			update_ui();
		}

        
        if (m_shake > 0)
		{
			m_shake -= Time.deltaTime * 20.0f;

			if(m_shake <= 0)
			{
				this.transform.localPosition = new Vector3(0,0,0);
			}
			else
			{
				this.transform.localPosition = new Vector3(Random.Range(- m_shake,m_shake),Random.Range(- m_shake,m_shake),Random.Range(- m_shake,m_shake));
			}
		}
	}
}
