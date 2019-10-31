
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mubiao_gui : MonoBehaviour,IMessage {

	public GameObject mubiao_panel_;
	public GameObject active_panel_;
	public GameObject m_richang;
	public GameObject m_effect1;
	public GameObject m_effect2;
	public GameObject m_effect3;
	public GameObject m_ji_fen;
	public GameObject m_ji_fen_panel;
	public List<GameObject> m_icons = new List<GameObject>();
	public int m_id;
	public UILabel m_name;
	public UILabel m_mubiao_Label1;
	public UILabel m_mubiao_Label2;
	public UILabel m_tiaozhan_Label1;
	public UILabel m_tiaozhan_Label2;
	public UILabel m_richang_label;
	public UILabel m_richang_label2;
	// Use this for initialization
	void Start () {
		
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void OnEnable()
	{
		hide_all ();
		reset_button ();
		active_panel_.GetComponent<mubiao_active_panel>().richang();
		active_panel_.SetActive (true);
		m_ji_fen.SetActive (true);
		active_panel_.GetComponent<mubiao_active_panel>().reset (1);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);

			m_richang.GetComponent<UIToggle>().value = true;

		}
		else if (obj.transform.name == "richang")
		{
			hide_all();
			active_panel_.GetComponent<mubiao_active_panel>().richang();
			active_panel_.SetActive (true);
			m_ji_fen.SetActive (true);
			active_panel_.GetComponent<mubiao_active_panel>().reset (1);
		}
		else if (obj.transform.name == "mubiao")
		{
			hide_all();
			mubiao_panel_.SetActive (true);
			mubiao_panel_.GetComponent<mubiao_mubiao_panel>().reset (1, 1);
		}
		else if (obj.transform.name == "tiaozhan")
		{
			hide_all();
			mubiao_panel_.SetActive (true);
			mubiao_panel_.GetComponent<mubiao_mubiao_panel>().reset (2, 1);
		}
		else if(obj.transform.name == "hide")
		{
			m_ji_fen_panel.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		
		else if (obj.transform.name == "get")
		{
			protocol.game.cmsg_active_score_reward _msg = new protocol.game.cmsg_active_score_reward ();
			_msg.reward_id = m_id ;
			net_http._instance.send_msg<protocol.game.cmsg_active_score_reward> (opclient_t.CMSG_ACTIVE_SCORE_REWARD, _msg);
		}
	}
	
	public void hide_all()
	{
		active_panel_.SetActive (false);
		mubiao_panel_.SetActive (false);
		m_ji_fen.SetActive (false);
	}

	public void reset_button()
	{
		if (mubiao_active_panel.is_effect())
		{
			m_effect1.SetActive(true);
		}
		else
		{
			m_effect1.SetActive(false);
		}
		if (mubiao_mubiao_panel.is_effect(1))
		{
			m_effect2.SetActive(true);
		}
		else
		{
			m_effect2.SetActive(false);
		}
		if (mubiao_mubiao_panel.is_effect(2))
		{
			m_effect3.SetActive(true);
		}
		else
		{
			m_effect3.SetActive(false);
		}
	}

	public static bool is_effect()
	{
		if ((mubiao_mubiao_panel.is_effect()
		    || mubiao_active_panel.is_effect()) && sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_richang)
		{
			return true;
		}
		return false;
	}
	

	void IMessage.message (s_message message)
	{
		if(message.m_type=="show_jifen_gui")
		{
			m_ji_fen_panel.SetActive(true);
			m_id = (int)message.m_ints[0];
			GameObject back = m_ji_fen_panel.transform.Find("back").gameObject;
			s_t_active_reward t_active_reward = game_data._instance.get_t_active_reward (m_id);
			if (t_active_reward.score > sys._instance.m_self.m_t_player.active_score || mubiao_active_panel.is_click(m_id) )
			{
				back.transform.Find("get").GetComponent<BoxCollider>().enabled = false;
				back.transform.Find("get").GetComponent<UISprite>().set_enable(false);
			}
			else 
			{
				back.transform.Find("get").GetComponent<BoxCollider>().enabled = true ;
				back.transform.Find("get").GetComponent<UISprite>().set_enable(true);
			}
			for(int i = 0;i < m_icons.Count;++i)
			{
				m_icons[i].SetActive(false);
			}

			for(int i = 0; i < t_active_reward.reward.Count && i < m_icons.Count ;++i)
			{
				m_icons[i].SetActive(true);
				sys._instance.remove_child(m_icons[i]);
				GameObject icon1 = icon_manager._instance.create_reward_icon(t_active_reward.reward[i].type, t_active_reward.reward[i].value1, t_active_reward.reward[i].value2, t_active_reward.reward[i].value3);
				icon1.transform.parent = m_icons[i].transform;
				icon1.transform.localPosition = new Vector3(0,0,0);
				icon1.transform.localScale = new Vector3(1,1,1);
			}
		}
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ACTIVE_SCORE_REWARD)
		{
			int id = m_id;
			s_t_active_reward t_active_reward = game_data._instance.get_t_active_reward(id);
			for(int i = 0; i < t_active_reward.reward.Count ;++i)
			{
				sys._instance.m_self.add_reward(t_active_reward.reward[i].type, t_active_reward.reward[i].value1, t_active_reward.reward[i].value2, t_active_reward.reward[i].value3,game_data._instance.get_t_language ("mubiao_gui.cs_179_169"));//日常目标奖励获得
			}
			
			sys._instance.m_self.m_t_player.active_score_id.Add(id);
			GameObject back = m_ji_fen_panel.transform.Find("back").gameObject;
			m_ji_fen_panel.transform.Find("frame_big").GetComponent<frame>().hide();
			back.transform.Find("get").GetComponent<BoxCollider>().enabled = false;
			back.transform.Find("get").GetComponent<UISprite>().set_enable(false);
			active_panel_.GetComponent<mubiao_active_panel>().update_ui();
			reset_button ();
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
