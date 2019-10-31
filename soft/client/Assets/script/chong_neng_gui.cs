
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chong_neng_gui: MonoBehaviour,IMessage {

	
	public ulong m_guid;	
	public GameObject m_back;
	
	public GameObject m_tp_ok;
	public GameObject m_up;
	public GameObject m_down;

	public GameObject m_yl_text_1;
	public GameObject m_level;
	public GameObject m_gold_text;
	public List<GameObject> sx = new List<GameObject>();
	public static bool is_texiao = true;
	public bool flag = false;
	public double level = 1.0f;
	public float m_time = 0.03f;
	bool is_press = false;
	bool is_release = false;

	private ccard m_card;
	private string error = "";
	private string yl_error = "";
	int i = 0;
	float m_life = 0.1f;
	
	void Start () {

		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	void IMessage.message(s_message message)
	{
		if(message.m_type == "update_cn_gui")
		{
			update_ui ();
		}
		
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_ROLE_UPGRADE)
		{
			int exp = get_exp (m_card, m_card.get_level() + (int)level);
			m_card.get_role().level = m_card.get_level() + (int)level;
			level = 1.0f;
			sys._instance.m_self.add_active(700, 1);
			sys._instance.m_self.check_target_done();
			sys._instance.m_self.sub_att(e_player_attr.player_yuanli, exp);
			sys._instance.m_self.sub_att(e_player_attr.player_gold, exp,m_card.m_t_class.name + game_data._instance.get_t_language ("chong_neng_gui.cs_65_87"));//角色升级消耗
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "update_bu_zheng";
			_message.m_bools.Add(is_texiao);
			cmessage_center._instance.add_message(_message);

			s_message _message2 = new s_message();
			_message2.m_type = "update_houyuan";
			cmessage_center._instance.add_message(_message2);

			//root_gui._instance.do_mask(1.0f);
			s_message _message1 = new s_message();
			_message1.time = 0.5f;
			_message1.m_type = "show_cn_label";
			cmessage_center._instance.add_message(_message1);
			s_message _message3 = new s_message();
			_message3.m_type = "check_bf";
			cmessage_center._instance.add_message(_message3);
		}
	}

	public void click(GameObject obj)
	{
		if (obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
		}
		else if(obj.name == "cn_ok")
		{
			s_t_exp _t_exp = game_data._instance.get_t_exp (m_card.get_level() + 1);
			if(_t_exp == null)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("chong_neng_gui.cs_99_59"));//已充能至顶级
				return;
			}
			if (error != "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + error);
				return;
			}
			if(yl_error != "")
			{
				send_mess();
				return;
			}
			//m_add_exp += exp;
			protocol.game.cmsg_role_upgrade _msg = new protocol.game.cmsg_role_upgrade ();
			_msg.role_guid = m_card.get_guid();
			_msg.level = m_card.get_role().level + (int)level;
			net_http._instance.send_msg<protocol.game.cmsg_role_upgrade> (opclient_t.CMSG_ROLE_UPGRADE, _msg);
		}

	}

	public void send_mess()
	{
		s_message _message = new s_message();
		_message.m_type = "buy_num_gui";
		_message.m_ints.Add(100100);
		_message.m_ints.Add(2);
		cmessage_center._instance.add_message(_message);
	}
	
	public void reset(ulong guid)
	{
		flag = false;
		m_guid = guid;
		m_life = 0.1f;
		//m_out_message = out_message;
		m_card = sys._instance.m_self.get_card_guid (m_guid);
		if(sys._instance.m_self.is_houyuan(m_card.get_guid()))
		{
			is_texiao = false;
		}
		else
		{
			is_texiao = true;
		}
		update_ui ();
	}

	public void OnEnable()
	{
		flag = false;
		i = 0;
		m_time = 0.03f;
	}

	private void set_att(List<double> attrs, int level, int type)
	{
		string s = "";
		string s1 = "";
		if(type == 1)
		{
			s = "start";
			s1= "cur";
		}
		else
		{
			s = "end";
			s1 = "next";
		}

		m_back.transform.Find("level").Find(s).GetComponent<UILabel>().text =  level.ToString();
		m_back.transform.Find("hp").Find(s1).GetComponent<UILabel>().text = " " + (int)attrs[1];
		m_back.transform.Find("attack").Find(s1).GetComponent<UILabel>().text = " " + (int)attrs[2];
		m_back.transform.Find("pd").Find(s1).GetComponent<UILabel>().text = " "+(int)attrs[3];
		m_back.transform.Find("md").Find(s1).GetComponent<UILabel>().text = " " + (int)attrs[4];
		m_back.transform.Find("speed").Find(s1).GetComponent<UILabel>().text = " " + (int)attrs[5];

	}

	void add_scale_anim(GameObject obj)
	{
		TweenScale _scale = TweenScale.Begin (obj,0.25f,new Vector3(1.0f,1.0f,1.0f));
		
		_scale.method = UITweener.Method.EaseInOut;
		_scale.from = new Vector3 (1.5f, 1.5f, 1.5f);
		_scale.to = new Vector3(1.0f,1.0f,1.0f);
		_scale.delay = 0;
		update_ui ();
	}

	public static int get_exp(ccard card, int level)
	{
		int _need_exp = 0;

		for(int i = card.get_level() + 1;i <= level;i ++)
		{
			s_t_exp _exp = game_data._instance.get_t_exp(i);
			if (_exp != null)
			{
				_need_exp += card.get_role_pz_exp(_exp);
			}
		}

		return _need_exp;
	}

	public void update_ui()
	{
		error = "";
		yl_error = "";
		m_card = sys._instance.m_self.get_card_guid (m_guid);

		List<double> attrs = ccard.get_role_org_attr (m_card.get_template_id(), m_card.get_level(), m_card.get_jlevel(), m_card.get_glevel(), m_card.get_pinzhi());
		set_att (attrs, m_card.get_level(), 1);
		attrs = ccard.get_role_org_attr (m_card.get_template_id(), m_card.get_level() + (int)level, m_card.get_jlevel(), m_card.get_glevel(), m_card.get_pinzhi());
		set_att (attrs, m_card.get_level() + (int)level, 2);

		s_t_exp _t_exp = game_data._instance.get_t_exp (m_card.get_level() + 1);

		if (_t_exp == null)
		{
			m_yl_text_1.GetComponent<UILabel>().text = "----";
			m_level.GetComponent<UILabel>().text = "----";
			m_gold_text.GetComponent<UILabel>().text = "----";
			m_back.transform.Find("level").Find("end").GetComponent<UILabel>().text = "---- (" + game_data._instance.get_t_language ("chong_neng_gui.cs_99_59") + ")";//已充能至顶级
			m_back.transform.Find("hp").Find("next").GetComponent<UILabel>().text = "----";
			m_back.transform.Find("attack").Find("next").GetComponent<UILabel>().text = "----";
			m_back.transform.Find("pd").Find("next").GetComponent<UILabel>().text = "----";
			m_back.transform.Find("md").Find("next").GetComponent<UILabel>().text = "----";
			m_back.transform.Find("speed").Find("next").GetComponent<UILabel>().text = "----";

		}
		else
		{
			int _yl = get_exp (m_card, m_card.get_level() + (int)level);
			string _text;
			if (_yl <= sys._instance.m_self.m_t_player.yuanli)
			{
				_text =  sys._instance.get_res_color(7);
				_text += _yl.ToString();
			}
			else
			{
				_text = "[ff0000]";
				_text += _yl.ToString();
				yl_error = game_data._instance.get_t_language ("chong_neng_gui.cs_246_15");//原力不足
			}
			string _text1;
			if (m_card.get_level()+level  <= sys._instance.m_self.m_t_player.level)
			{
				_text1 = "[00ff00]";
				_text1 += (m_card.get_level()+level).ToString();
			}
			else
			{
				_text1 = "[ff0000]";
				_text1 += (m_card.get_level()+level).ToString();
				error = game_data._instance.get_t_language ("chong_neng_gui.cs_258_12");//玩家等级不足
			}
			string _text2;
			if (_yl  <= sys._instance.m_self.get_att(e_player_attr.player_gold))
			{
				_text2 = sys._instance.get_res_color(1);
				_text2 += _yl.ToString();
			}
			else
			{
				_text2 = "[ff0000]";
				_text2 += _yl.ToString();
				error = game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
			}
			m_yl_text_1.GetComponent<UILabel>().text = _text;
			m_level.GetComponent<UILabel>().text = _text1;
			m_gold_text.GetComponent<UILabel>().text = _text2;
		}
	}

	public static bool is_chongneng(ccard _card)
	{
		s_t_exp _t_exp = game_data._instance.get_t_exp (_card.get_level() + 1);
		int _yl = get_exp (_card, _card.get_level() + 1);
		if(_yl <= sys._instance.m_self.m_t_player.yuanli 
		   && _card.get_level() < sys._instance.m_self.m_t_player.level&& _yl <= sys._instance.m_self.get_att(e_player_attr.player_gold) && _t_exp!= null)
		{			
			return true;
		}
		return false;
	}

	public void press(GameObject obj)
	{
		if(obj.name == "up")
		{
			m_life = 0.0f;
			is_press = true;
		}
		if(obj.name == "down")
		{
			m_life = 0.0f;
			is_release = true;
		}
	}
	
	public void release(GameObject obj)
	{
		if(obj.name == "up")
		{
			is_press = false;
		}
		if(obj.name == "down")
		{
			is_release = false;
		}
	}

	// Update is called once per frame
	void Update () {
		if(flag)
		{
			m_time -= Time.deltaTime;
			if(m_time <= 0)
			{
				m_time = 0.03f;
				add_scale_anim(sx[i]);
				i++;
			}
			if(i >= 10)
			{
				i =0;
				flag = false;
			}
		}
		if(is_press)
		{
			bool _flag = false;
			Ray ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] _hits = Physics.RaycastAll(ray);
			for(int i = 0; i < _hits.Length;++i)
			{
				if(_hits[i].transform.name == "up")
				{
					_flag = true;
					break;
				}
			}
			if(!_flag)
			{
				is_press = false;
			}
			m_life -= Time.deltaTime;
			if(m_life < 0)
			{
				m_life = 0.1f;
				int _yl = get_exp (m_card, m_card.get_level() + (int)level + 1);
				s_t_exp _t_exp = game_data._instance.get_t_exp (m_card.get_level() + (int)level + 1);
				if(_t_exp == null)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("chong_neng_gui.cs_99_59"));//已充能至顶级
					is_press = false;
					return;
				}
				if (m_card.get_level()+ level + 1 > sys._instance.m_self.m_t_player.level)
				{
					string s = string.Format(game_data._instance.get_t_language ("chong_neng_gui.cs_364_30"),(m_card.get_level()+ level + 1).ToString());//下一级充能需要玩家{0}级
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
					is_press = false;
					return;
				}
				if(_yl > sys._instance.m_self.get_att(e_player_attr.player_gold))
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
					return;
				}
				if (_yl > sys._instance.m_self.m_t_player.yuanli)
				{
					send_mess();
					is_press = false;
					return;
				}
				level += 1;
				flag  = true;
				//update_ui();
			}
		}
		if(is_release)
		{
			bool _flag = false;
			Ray ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit[] _hits = Physics.RaycastAll(ray);
			for(int i = 0; i < _hits.Length;++i)
			{
				if(_hits[i].transform.name == "down")
				{
					_flag = true;
					break;
				}
			}
			if(!_flag)
			{
				is_press = false;
			}
			m_life -= Time.deltaTime;
			if(m_life < 0)
			{
				m_life = 0.1f;
				s_t_exp _t_exp = game_data._instance.get_t_exp (m_card.get_level() + (int)level);
				if(_t_exp == null)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("chong_neng_gui.cs_99_59"));//已充能至顶级
					is_press = false;
					return;
				}
				if(level <= 1)
				{
					is_release = false;
					return;
				}
				level -= 1;
				flag = true;
				//update_ui();
			}
		}
	}
}
