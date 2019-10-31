
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class friend_gui : MonoBehaviour{

	public GameObject m_haoyou_panel;
	public GameObject m_sousuo_panel;
	public GameObject m_xinzeng_panel;
	public GameObject m_zengsong_panel;
	public GameObject m_haoyou_button;
	public GameObject m_xinzeng_eff;
	public GameObject m_zengsong_eff;
    public protocol.game.smsg_social_look m_msg;
    public protocol.game.smsg_social_look m_msgcode;
    public static friend_gui _insatnce;


	public UILabel m_haoyou;
	public UILabel m_sousuo;
	public UILabel m_xinzeng;
	public UILabel m_zengsong;
	public UILabel m_haoyou1;
	public UILabel m_sousuo1;
	public UILabel m_xinzeng1;
	public UILabel m_zengsong1;
	// Use this for initialization
	void Start () 
	{
        _insatnce = this;
	}

    public  void reset()
    {
        m_haoyou_panel.GetComponent<haoyou_panel>().m_flag = false;
        m_sousuo_panel.GetComponent<sousuo_panel>().m_flag = false;
        m_xinzeng_panel.GetComponent<xinzeng_panel>().m_flag = false;
        update_button();
        haoyou();
    }

	void hide_page()
	{
		m_haoyou_panel.SetActive(false);
		m_sousuo_panel.SetActive(false);
		m_xinzeng_panel.SetActive(false);
		m_zengsong_panel.SetActive(false);
		m_haoyou_button.GetComponent<UIToggle>().value = true;
	}

	public void update_button()
	{
		if (sys._instance.m_self.is_friend_apply == 1)
		{
			m_xinzeng_eff.SetActive(true);
		}
        else if (sys._instance.m_self.is_friend_apply == 2)
        {
            m_xinzeng_eff.SetActive(false);
        }
        else if (sys._instance.m_self.is_friend_apply == 3)
        {
            m_xinzeng_eff.SetActive(true);
        }
        else if (sys._instance.m_self.is_friend_apply == 0)
        {
            m_xinzeng_eff.SetActive(false);
        }
		if (sys._instance.m_self.is_friend_tili == 1)
		{
			m_zengsong_eff.SetActive(true);
		}
		else
		{
			m_zengsong_eff.SetActive(false);
		}        
	}
    
    int get_state(int id)
    {
        s_t_target t_target = game_data._instance.get_t_target(id);
        dhc.social_t temp = null;
        for (int i = 0; i < m_msgcode.social.Count; i++)
        {
            if (m_msgcode.social[i].template_id == -1)
            {
                temp = m_msgcode.social[i];
 
            }
        }
        int num = 0;
        if (temp != null)
        {
            for (int i = 0; i < temp.invite_players.Count; i++)
            {
                if ((int)temp.invite_levels[i] >= t_target.tjdef1)
                {
                    num++;
 
                }
            }
        }
        if (sys._instance.m_self.m_t_player.finished_tasks.Contains((uint)id))
        {
            return 3;//已领取
        }
        else
        {
            if (num >= t_target.tjnum)
            {
                return 2;//可领取

            }
            else
            {
                return 1;//未完成
            }
        }
 
    }
	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			hide_page();
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
            Destroy(this.gameObject);
		}
		
		if(obj.transform.name == "haoyou")
		{
			haoyou();
		}
		if(obj.transform.name == "sousuo")
		{
			sousuo();
		}
		if(obj.transform.name == "xinzeng")
		{
			xinzeng();
		}
		if(obj.transform.name == "zengsong")
		{
			zengsong();
		}
	}
    
	void haoyou()
	{
		hide_page ();
		m_haoyou_panel.SetActive(true);
        m_haoyou_panel.GetComponent<haoyou_panel>().add_handle();
		m_haoyou_panel.GetComponent<haoyou_panel>().reset ();
	}

	void sousuo()
	{
		hide_page ();
		m_sousuo_panel.SetActive(true);
		m_sousuo_panel.GetComponent<sousuo_panel>().reset ();
	}

	void xinzeng()
	{
		hide_page ();
		m_xinzeng_panel.SetActive(true);
		m_xinzeng_panel.GetComponent<xinzeng_panel>().reset ();
	}

	void zengsong()
	{
		hide_page ();
		m_zengsong_panel.SetActive(true);
		m_zengsong_panel.GetComponent<zengsong_panel>().reset ();
	}
}
