
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class sign_gui : MonoBehaviour, IMessage {

	public GameObject m_view;
	private GameObject m_sign;
	public UILabel m_qiandaojiangli;
	public UIProgressBar m_bar;
	public UILabel m_text;
    public UISprite m_back;
    public GameObject m_recharge;
    public GameObject m_yilingqu;
	public GameObject m_dsign_point;
	public GameObject m_vip_point;
	public GameObject m_week_point;
    public UIToggle m_sign_button;
	private int m_select = 0;
	int m_id = 0;

	// Use this for initialization
	void Start () {
		m_qiandaojiangli.text = game_data._instance.get_t_language ("sign_gui.cs_30_28");//月签到奖励
		cmessage_center._instance.add_handle (this);
		reset (1);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		m_select = 1;
		reset (m_select);
	}

	public void reset(int id,bool flag = false)
	{
		if(!flag)
		{
			if(m_view.GetComponent<SpringPanel>() != null)
			{
				m_view.GetComponent<SpringPanel>().enabled = false;
			}
			m_view.transform.localPosition = new Vector3(0, 0, 0);
			m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		}
		sys._instance.remove_child (m_view);
		if(id == 1)
		{
			dbc m_dbc_daily_sign = game_data._instance.m_dbc_daily_sign;
	        GameObject gsign = null;
	        GameObject fsign = null;
			for (int i = 0; i < m_dbc_daily_sign.get_y(); ++i)
			{
	           
			    gsign = game_data._instance.ins_object_res("ui/dsign");
				gsign.transform.parent = m_view.transform;
				gsign.transform.localPosition = new Vector3(-240 + 120 * (i % 5), 174- 120 * (i / 5));
				gsign.transform.localScale = new Vector3(1,1,1);
				gsign.transform.GetComponent<sign>().m_index = i;
				gsign.transform.GetComponent<sign>().m_scro = m_view;
	            if (i == 0)
	            {
	                fsign = gsign;

	            }
				if (i  == sys._instance.m_self.m_t_player.daily_sign_index % 30 - 1)
				{
					m_sign = gsign;
				}
			}
	        if (sys._instance.m_self.m_t_player.daily_sign_index % 30 == 0)
	        {
	            m_sign = gsign;
	        }
		}
		if(id == 2)
		{
			List<s_t_vip_libao> t_vips = new List<s_t_vip_libao>();
            for (int i = sys._instance.m_self.m_t_player.vip; i < sys._instance.m_self.m_t_player.vip + 4; i++)
            {
                s_t_vip_libao t_vip_libao = game_data._instance.get_t_vip_libao(i);
                if (t_vip_libao != null)
                {
                    t_vips.Add(t_vip_libao);
                }
                
            }
			
			for (int i = 0; i < t_vips.Count; ++i)
			{
				GameObject _vip = game_data._instance.ins_object_res("ui/dsign_vip_sub");
				_vip.transform.parent = m_view.transform;
				_vip.transform.localPosition = new Vector3(0, 161-153*i,0);
				_vip.transform.localScale = new Vector3(1,1,1);
				_vip.transform.GetComponent<dsign_vip_sub>().m_scro = m_view;
				_vip.transform.GetComponent<dsign_vip_sub>().t_vip_libao = t_vips[i];
				_vip.transform.GetComponent<dsign_vip_sub>().reset();
				sys._instance.add_pos_anim(_vip,0.3f, new Vector3(-200, 0, 0), i * 0.05f);
				sys._instance.add_alpha_anim(_vip,0.3f, 0, 1.0f, i * 0.05f);
			}
		}
		if(id == 3)
		{
			int _id = 0;
			List<int> week_ids = new List<int>();
			for (int i = 0; i < game_data._instance.m_dbc_week_libao.get_y(); ++i)
			{
				int week_id = int.Parse(game_data._instance.m_dbc_week_libao.get(0,i));
				s_t_week_libao t_week_libao = game_data._instance.get_t_week_libao(week_id);
				if(sys._instance.m_self.m_t_player.level < t_week_libao.level1 || sys._instance.m_self.m_t_player.level > t_week_libao.level2)
				{
					continue;
				}
				week_ids.Add(week_id);
			}
			week_ids.Sort(comp);
			for(int i = 0; i < week_ids.Count;++i)
			{
				GameObject _vip = game_data._instance.ins_object_res("ui/dsign_week_sub");
				_vip.transform.parent = m_view.transform;
				_vip.transform.localPosition = new Vector3(0, 161-153*_id,0);
				_vip.transform.localScale = new Vector3(1,1,1);
				_vip.transform.GetComponent<dsign_week_sub>().m_scro = m_view;
				_vip.transform.GetComponent<dsign_week_sub>().id = week_ids[i];
				_vip.transform.GetComponent<dsign_week_sub>().reset();
				sys._instance.add_pos_anim(_vip,0.3f, new Vector3(-200, 0, 0), _id * 0.05f);
				sys._instance.add_alpha_anim(_vip,0.3f, 0, 1.0f, _id * 0.05f);
				_id++;
			}
		}
		int value = sys._instance.m_self.m_t_player.daily_sign_num;
		if (value > 30)
		{
			value = 30;
		}
        if (value < 30)
        {
            m_bar.gameObject.SetActive(true);            
            m_text.text = value.ToString() + "/30";
            float v = value / 30.0f;
            m_bar.value = v;
            m_recharge.SetActive(false);
            m_yilingqu.SetActive(false);

        }
        else
        {
            if (sys._instance.m_self.m_t_player.daily_sign_flag == 0)
            {
                m_back.height = 245;
                m_recharge.SetActive(true);
                m_yilingqu.SetActive(false);
            }
            else
            {
				m_recharge.SetActive(false);
                m_yilingqu.SetActive(true);              
                m_back.height = 326;
            }
            m_bar.gameObject.SetActive(false);
        }
		if(sign_effect())
		{
			m_dsign_point.SetActive(true);
		}
		else
		{
			m_dsign_point.SetActive(false);
		}
		s_t_vip_libao _vip_libao = game_data._instance.get_t_vip_libao (sys._instance.m_self.m_t_player.vip);
		if(sys._instance.m_self.m_t_player.huodong_vip_libao == 0 && _vip_libao!=null)
		{
			m_vip_point.SetActive(true);
		}
		else
		{
			m_vip_point.SetActive(false);
		}
		if(sys._instance.m_self.m_t_player.huodong_week_libao.Count <= 0)
		{
			m_week_point.SetActive(true);
		}
		else
		{
			m_week_point.SetActive(false);
		}
	}

	public int comp(int id1, int id2)
	{
		int num = 0;
		int num1 = 0;
		int jewel = sys._instance.m_self.m_t_player.jewel;
		s_t_week_libao t_week_libao = game_data._instance.get_t_week_libao(id1);
		s_t_week_libao t_week_libao1 = game_data._instance.get_t_week_libao(id2);
		for(int j = 0; j < sys._instance.m_self.m_t_player.huodong_week_libao.Count;++j)
		{
			if(sys._instance.m_self.m_t_player.huodong_week_libao[j] == id1)
			{
				num++;
			}
		}
		for(int j = 0; j < sys._instance.m_self.m_t_player.huodong_week_libao.Count;++j)
		{
			if(sys._instance.m_self.m_t_player.huodong_week_libao[j] == id2)
			{
				num1++;
			}
		}
		if(num < t_week_libao.num && num1 >= t_week_libao1.num)
		{
			return -1;
		}
		else if(num >= t_week_libao.num && num1 < t_week_libao1.num)
		{
			return 1;
		}
		else if(jewel >= t_week_libao.jewel && jewel < t_week_libao1.jewel)
		{
			return -1;
		}
		else if(jewel < t_week_libao.jewel && jewel >= t_week_libao1.jewel)
		{
			return 1;
		}
		else
		{
			return id1 - id2;
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			transform.Find("frame_big").GetComponent<frame>().hide();

			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.transform.name == "recharge")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_DAILY_SIGN_REWARD, _msg);
		}
		else if (obj.transform.name == "dsign")
		{
			m_select = 1;
			reset(1);
		}
		else if (obj.transform.name == "vip")
		{
			m_select = 2;
			reset(2);
		}
		else if (obj.transform.name == "week")
		{
			m_select = 3;
			reset(3);
		}
	}

	void IMessage.message (s_message message)
	{
		if (message.m_type == "daily_refresh")
		{
            m_select = 1;
            m_sign_button.value = true;
            reset(1);
			
		}
		if(message.m_type == "vip_libao")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_VIP_LIBAO, _msg);
		}
		if(message.m_type == "huodong_vip_liabo_recharge")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);
			
			s_message _mes1 = new s_message();
			_mes1.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_mes1);
		}
		if(message.m_type == "huodong_week_libao")
		{
			m_id = (int)message.m_ints[0];
			s_t_week_libao t_week_libao = game_data._instance.get_t_week_libao(m_id);
			string _des = string.Format(game_data._instance.get_t_language ("sign_gui.cs_347_31") , t_week_libao.jewel//是否花费[00ffff]{0}钻石购买[00ff00][{1}][-]?
				, t_week_libao.name);
			s_message _msg = new s_message();
			_msg.m_type = "select_buy_week_liabo";
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
		}

		if(message.m_type == "select_buy_week_liabo")
		{
			protocol.game.cmsg_huodong_reward _msg = new protocol.game.cmsg_huodong_reward ();
			_msg.id = m_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_WEEK_LIBAO, _msg);
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_DAILY_SIGN)
		{
			protocol.game.smsg_daily_sign _msg = net_http._instance.parse_packet<protocol.game.smsg_daily_sign> (message.m_byte);
			for(int i = 0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}

			s_t_daily_sign t_sign = game_data._instance.get_t_daily_sign(sys._instance.m_self.m_t_player.daily_sign_reward  % 30 + 1);
			for (int i = 0; i < _msg.types.Count; ++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg.value1s[i],_msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("sign_gui.cs_379_101"));//日常签到获得
			}

			for(int i = 0;i < _msg.roles.Count;i ++)
			{
				sys._instance.m_self.add_card(_msg.roles[i], true);
			}

			sys._instance.m_self.m_t_player.daily_sign_reward++;
			sys._instance.m_self.m_t_player.daily_sign_num++;
			m_sign.transform.Find("main").GetComponent<UISprite>().alpha = 0.5f;
			m_sign.transform.Find("gou").gameObject.SetActive(true);
			int value = sys._instance.m_self.m_t_player.daily_sign_num;
            if (value < 30)
            {
                m_bar.gameObject.SetActive(true);                
                m_text.text = value.ToString() + "/30";
                float v = value / 30.0f;
                m_bar.value = v;
                m_recharge.SetActive(false);
                m_yilingqu.SetActive(false);
                
            }
            else
            {
                if (sys._instance.m_self.m_t_player.daily_sign_flag == 0)
                {
                    m_back.height = 245;
                    m_recharge.SetActive(true);                    
                    m_yilingqu.SetActive(false);
                }
                else
				{
					m_recharge.SetActive(false);
                    m_yilingqu.SetActive(true);                    
                    m_back.height = 326;
                }
                m_bar.gameObject.SetActive(false);
            }
			if(sign_effect())
			{
				m_dsign_point.SetActive(true);
			}
			else
			{
				m_dsign_point.SetActive(false);
			}
			reset(1,true);
		}
		else if (message.m_opcode == opclient_t.CMSG_DAILY_SIGN_REWARD)
		{
			sys._instance.m_self.add_att(e_player_attr.player_vip_exp, 1000);
			sys._instance.m_self.m_t_player.daily_sign_flag = 1;
			m_recharge.SetActive(false);
			m_yilingqu.SetActive(true);
			m_back.height = 326;
		}
		else if (message.m_opcode == opclient_t.CMSG_HUODONG_VIP_LIBAO)
		{
			protocol.game.smsg_huodong_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_reward> (message.m_byte);
			for(int i = 0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			for (int i = 0; i < _msg.types.Count; ++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg.value1s[i],_msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("sign_gui.cs_449_101"));//签到vip礼包
			}
            for (int i = 0; i < _msg.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_msg.pets[i]);
            }
			
			for(int i = 0;i < _msg.roles.Count;i ++)
			{
				sys._instance.m_self.add_card(_msg.roles[i], true);
			}
			sys._instance.m_self.m_t_player.huodong_vip_libao = 1;
			reset(m_select);
		}
		if (message.m_opcode == opclient_t.CMSG_HUODONG_WEEK_LIBAO)
		{
			protocol.game.smsg_huodong_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_reward> (message.m_byte);
			for(int i = 0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			for (int i = 0; i < _msg.types.Count; ++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg.value1s[i],_msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("sign_gui.cs_476_101"));//签到周礼包
			}
            for (int i = 0; i < _msg.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_msg.pets[i]);
            }
			for(int i = 0;i < _msg.roles.Count;i ++)
			{
				sys._instance.m_self.add_card(_msg.roles[i], true);
			}
			sys._instance.m_self.m_t_player.huodong_week_libao.Add(m_id);
			s_t_week_libao t_week_libao = game_data._instance.get_t_week_libao(m_id);
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, t_week_libao.jewel, game_data._instance.get_t_language ("sign_gui.cs_488_89"));//签到周礼包消耗
			reset(m_select);
		}
	}

	public bool sign_effect()
	{
		if (sys._instance.m_self.m_t_player.daily_sign_index != sys._instance.m_self.m_t_player.daily_sign_reward)
		{
			return true;
		}
		if (sys._instance.m_self.m_t_player.daily_sign_num >= 30 && sys._instance.m_self.m_t_player.daily_sign_flag == 0)
		{
			return true;
		}
		return false;
	}
	

	public static bool effect()
	{
		if (sys._instance.m_self.m_t_player.daily_sign_index != sys._instance.m_self.m_t_player.daily_sign_reward)
		{
			return true;
		}
		if (sys._instance.m_self.m_t_player.daily_sign_num >= 30 && sys._instance.m_self.m_t_player.daily_sign_flag == 0)
		{
			return true;
		}
		s_t_vip_libao t_vip_libao = game_data._instance.get_t_vip_libao (sys._instance.m_self.m_t_player.vip);
		if(sys._instance.m_self.m_t_player.huodong_vip_libao == 0 && t_vip_libao!=null)
		{
			return true;
		}
		if(sys._instance.m_self.m_t_player.huodong_week_libao.Count <= 0)
		{
			return true;
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
	
	}

}
