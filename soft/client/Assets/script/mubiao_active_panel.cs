
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mubiao_active_panel : MonoBehaviour, IMessage {
	
	public GameObject m_ji_fen;
	private float score = 0;
	private int m_id=0;
	private static List<int> bx_id = new List<int>();
	private static List<float> num = new List<float>();
	public GameObject[] bx;
	public GameObject[] jf;
	public GameObject[] bx1;
	public GameObject[] fbx;
	public GameObject m_mubiao_panel;
	public GameObject m_view;
	
	public UILabel m_cur_jifen;
	public UILabel m_tishi1;
	public UILabel m_tishi2;
	public UILabel m_jifen_name_Label;
	public UILabel m_get_Label;
	int m_num;
	int click_id = 0;
	//private int m_id=0;
	List<int> m_done_ids = new List<int>();

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public static bool active_vis(s_t_active t_active)
	{
		for (int i = 0; i < sys._instance.m_self.m_t_player.active_id.Count; ++i)
		{
			if (sys._instance.m_self.m_t_player.active_id[i] == t_active.id)
			{
				if (sys._instance.m_self.m_t_player.active_reward[i] == 0)
				{
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		return true;
	}

	static public bool active_done(s_t_active t_active)
	{
		for (int i = 0; i < sys._instance.m_self.m_t_player.active_id.Count; ++i)
		{
			if (sys._instance.m_self.m_t_player.active_id[i] == t_active.id)
			{
				if (sys._instance.m_self.m_t_player.active_num[i] >= t_active.num)
				{
					int hour = timer.dtnow().Hour;
					if (t_active.id == 100)
					{
						if (hour != 12 && hour != 13)
						{
							return false;
						}
						else
						{
							return true;
						}
					}
					else if (t_active.id == 101)
					{
						if (hour != 18 && hour != 19)
						{
							return false;
						}
						else
						{
							return true;
						}
					}
					return true;
				}
				else
				{
					return false;
				}
			}
		}
		return false;
	}

	public int get_active_num(s_t_active t_active)
	{
		for (int i = 0; i < sys._instance.m_self.m_t_player.active_id.Count; ++i)
		{
			if (sys._instance.m_self.m_t_player.active_id[i] == t_active.id)
			{
				return sys._instance.m_self.m_t_player.active_num[i];
			}
		}
		return 0;
	}
	

	public void reset(int anim)
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}

		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);

		dbc m_dbc_active = game_data._instance.get_dbc_active();
		int tnum = 0;
		for (int i = 0; i < m_dbc_active.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_active.get(0, i));
			s_t_active t_active = game_data._instance.get_t_active(id);
			if(sys._instance.m_self.m_t_player.level < t_active.level)
			{
				continue;
			}
			if (!active_vis(t_active))
			{
				continue;
			}
			if (!active_done(t_active))
			{
				continue;
			}
			int num = get_active_num(t_active);

			GameObject active = game_data._instance.ins_object_res("ui/active");
			active.transform.parent = m_view.transform;
			active.transform.localPosition = new Vector3(0, 109 - tnum * 109,0);
			active.transform.localScale = new Vector3(1,1,1);
			active.transform.GetComponent<active>().m_active_id = t_active.id;
			active.transform.GetComponent<active>().m_num = num;
			active.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			active.transform.GetComponent<active>().reset();

			if (anim == 1)
			{
				sys._instance.add_pos_anim(active,0.3f, new Vector3(-300, 0, 0), tnum * 0.05f);
				sys._instance.add_alpha_anim(active,0.3f, 0, 1.0f, tnum * 0.05f);
			}
			else if (anim == 2)
			{
				sys._instance.add_pos_anim(active,0.3f, new Vector3(-300, 0, 0), tnum * 0.05f + 0.45f);
				sys._instance.add_alpha_anim(active,0.3f, 0, 1.0f, tnum * 0.05f + 0.5f);
			}
			else
			{
				if (active.GetComponent<TweenPosition>() != null)
				{
					Object.Destroy(active.GetComponent<TweenPosition>());
				}
				if (active.GetComponent<TweenAlpha>() != null)
				{
					Object.Destroy(active.GetComponent<TweenAlpha>());
				}
				active.GetComponent<UIWidget>().alpha = 1.0f;
			}

			tnum++;
		}
		for (int i = 0; i < m_dbc_active.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_active.get(0, i));
			s_t_active t_active = game_data._instance.get_t_active(id);
			if(sys._instance.m_self.m_t_player.level < t_active.level)
			{
				continue;
			}
			if (!active_vis(t_active))
			{
				continue;
			}
			if (active_done(t_active))
			{
				continue;
			}
			int num = get_active_num(t_active);

			GameObject active = game_data._instance.ins_object_res("ui/active");
			active.transform.parent = m_view.transform;
			active.transform.localPosition = new Vector3(0, 115 - tnum * 109,0);
			active.transform.localScale = new Vector3(1,1,1);
			active.transform.GetComponent<active>().m_active_id = t_active.id;
			active.transform.GetComponent<active>().m_num = num;
			active.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			active.transform.GetComponent<active>().reset();

			if (anim == 1)
			{
				sys._instance.add_pos_anim(active,0.3f, new Vector3(-300, 0, 0), tnum * 0.05f);
				sys._instance.add_alpha_anim(active,0.3f, 0, 1.0f, tnum * 0.05f);
			}
			else if (anim == 2)
			{
				sys._instance.add_pos_anim(active,0.3f, new Vector3(-300, 0, 0), tnum * 0.05f + 0.45f);
				sys._instance.add_alpha_anim(active,0.3f, 0, 1.0f, tnum * 0.05f + 0.5f);
			}
			else
			{
				if (active.GetComponent<TweenPosition>() != null)
				{
					Object.Destroy(active.GetComponent<TweenPosition>());
				}
				if (active.GetComponent<TweenAlpha>() != null)
				{
					Object.Destroy(active.GetComponent<TweenAlpha>());
				}
				active.GetComponent<UIWidget>().alpha = 1.0f;
			}

			tnum++;
		}
		update_ui ();
		m_mubiao_panel.GetComponent<mubiao_gui>().reset_button ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void click(GameObject obj)
	{
		if (obj.name == "active(Clone)")
		{
			click_id = obj.transform.GetComponent<active>().m_active_id;
			int num = obj.transform.GetComponent<active>().m_num;
			s_t_active t_active = game_data._instance.get_t_active(click_id);
			if (active_done(t_active))
			{
				if (t_active.id == 100 || t_active.id == 101)
				{
					s_t_exp t_exp = game_data._instance.get_t_exp (sys._instance.m_self.m_t_player.level);
					s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
					if (sys._instance.m_self.m_t_player.tili >= t_exp.tili + t_vip.add_tili)
					{
						string s = game_data._instance.get_t_language ("mubiao_active_panel.cs_254_17");//体力已满
						string s1 = game_data._instance.get_t_language ("mubiao_active_panel.cs_255_18");//无法领取
						root_gui._instance.show_prompt_dialog_box("[ffc882]" + s + "，" +s1);
						return;
					}
				}
				protocol.game.cmsg_active_reward _msg = new protocol.game.cmsg_active_reward ();
				_msg.id = click_id;
				net_http._instance.send_msg<protocol.game.cmsg_active_reward> (opclient_t.CMSG_ACTIVE_REWARD, _msg);
			}
		}
		if(obj.name.Length == 3&& obj.name.Substring(0,2) == "bx")
		{
			int m_index =  int.Parse(obj.transform.name.Substring(2, 1)) - 1;
			m_id = bx_id[m_index];
			s_message _message = new s_message();
			_message.m_type = "show_jifen_gui";
			_message.m_ints.Add(m_id);
			cmessage_center._instance.add_message(_message);
		}
		if(obj.name.Length == 4&& obj.name.Substring(0,3) == "bx1")
		{
			int m_index =  int.Parse(obj.transform.name.Substring(3, 1)) - 1;
			m_id = bx_id[m_index];
			s_message _message = new s_message();
			_message.m_type = "show_jifen_gui";
			_message.m_ints.Add(m_id);
			cmessage_center._instance.add_message(_message);
		}
	}

	public void richang()
	{
		GameObject back = m_ji_fen.transform.Find("back").gameObject;
		dbc m_dbc_active_reward = game_data._instance.get_dbc_active_reward();
		dbc m_dbc_active = game_data._instance.get_dbc_active();
		
		score=0;
		
		for (int i = 0; i < m_dbc_active.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_active.get(0, i));
			s_t_active t_active = game_data._instance.get_t_active(id);
			score+=t_active.score;
		}
		
		m_ji_fen.transform.Find("bar").GetComponent<UIProgressBar>().value = (float)(sys._instance.m_self.m_t_player.active_score) /score;
		m_ji_fen.transform.Find("tishi").Find("num").GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.active_score.ToString();
		
		for(int i = 0; i < m_dbc_active_reward.get_y(); ++i)
		{
			num.Add(int.Parse(m_dbc_active_reward.get(1, i)));
			bx_id.Add(int.Parse(m_dbc_active_reward.get(0, i)));
		}
		
		jf[0].GetComponent<UILabel>().text = num[0].ToString() ;
		jf[1].GetComponent<UILabel>().text = num[1].ToString() ;
		jf[2].GetComponent<UILabel>().text = num[2].ToString() ;
		jf[3].GetComponent<UILabel>().text = num[3].ToString() ;
		
		
		float m_width = (float)back.GetComponent<UISprite>().width;
		for(int i = 0; i < m_dbc_active_reward.get_y(); ++i)
		{
			float data = num[i];
			bx[i].transform.localPosition = back.transform.localPosition+new Vector3 (((data*m_width) /score) - (6*i), 0 , 0);
			bx1[i].transform.localPosition = back.transform.localPosition+new Vector3 (((data*m_width) /score)- (6*i) , 0 , 0);
			jf[i].transform.localPosition = back.transform.localPosition+new Vector3 (((data*m_width) /score)- (6*i), -27 , 0);
			fbx[i].transform.localPosition = back.transform.localPosition+new Vector3 (((data*m_width) /score)- (6*i), 0 , 0);
		}
		
		
	}

	public static bool is_effect()
	{
		dbc m_dbc_active_reward = game_data._instance.get_dbc_active_reward();
		for(int i = 0; i < m_dbc_active_reward.get_y(); ++i)
		{
			num.Add(int.Parse(m_dbc_active_reward.get(1, i)));
			bx_id.Add(int.Parse(m_dbc_active_reward.get(0, i)));
		}
		for (int i = 0; i < m_dbc_active_reward.get_y(); ++i)
		{
			s_t_active_reward t_active_reward = game_data._instance.get_t_active_reward (bx_id[i]);
			if(t_active_reward.score <= sys._instance.m_self.m_t_player.active_score && !is_click(bx_id[i]))
			{
				return true;
			}
		}

		dbc m_dbc_active = game_data._instance.get_dbc_active();
		for (int i = 0; i < m_dbc_active.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_active.get(0, i));
			s_t_active t_active = game_data._instance.get_t_active(id);
			if (!active_vis(t_active))
			{
				continue;
			}
			if (active_done(t_active))
			{
				return true;
			}
		}
		return false;
	}

	public static bool is_click(int id)
	{
		for(int i = 0;i<sys._instance.m_self.m_t_player.active_score_id.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.active_score_id[i] == id)
			{
				return true;
			}
		}
		return false;
	}
	
	public void update_ui()
	{
		dbc m_dbc_active_reward = game_data._instance.get_dbc_active_reward();
		for(int i = 0; i < m_dbc_active_reward.get_y(); ++i)
		{
			num.Add(int.Parse(m_dbc_active_reward.get(1, i)));
			bx_id.Add(int.Parse(m_dbc_active_reward.get(0, i)));
		}
		for (int i = 0; i < m_dbc_active_reward.get_y(); ++i)
		{
			s_t_active_reward t_active_reward = game_data._instance.get_t_active_reward (bx_id[i]);
			if(t_active_reward.score <= sys._instance.m_self.m_t_player.active_score && !is_click(bx_id[i]))
			{
				bx[i].SetActive(false);
				bx1[i].SetActive(true);
				fbx[i].SetActive(true);
			}
			else if(t_active_reward.score <= sys._instance.m_self.m_t_player.active_score && is_click(bx_id[i]))
			{
				bx[i].SetActive(true);
				bx1[i].SetActive(false);
				fbx[i].SetActive(false);
			}
			else
			{
				bx[i].SetActive(false);
				bx1[i].SetActive(true);
				fbx[i].SetActive(false);
			}
		}
		m_ji_fen.transform .Find("tishi").Find("num").GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.active_score.ToString ();
		m_ji_fen.transform.Find("bar").GetComponent<UIProgressBar>().value = (float)(sys._instance.m_self.m_t_player.active_score) /score;
	}


	void IMessage.message (s_message message)
	{
		
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ACTIVE_REWARD)
		{
			protocol.game.smsg_active_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_active_reward> (message.m_byte);
			for(int i =0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i],true);
			}
			s_t_active t_active = game_data._instance.get_t_active(click_id);
			for (int i = 0; i < sys._instance.m_self.m_t_player.active_id.Count; ++i)
			{
				if (sys._instance.m_self.m_t_player.active_id[i] == click_id)
				{
					sys._instance.m_self.m_t_player.active_reward[i] = 1;
				}
			}
			for(int i = 0; i < _msg.types.Count;++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("mubiao_active_panel.cs_433_101"));//日常目标获得
			}
			if (t_active.score > 0)
			{
				sys._instance.m_self.add_att(e_player_attr.player_score, t_active.score);
			}
			reset(0);
		}
	}
}
