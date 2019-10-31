
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_sx_gui : MonoBehaviour ,IMessage{

	public GameObject m_name;
	public GameObject m_cur_attr;
	public List<GameObject> m_stars = new List<GameObject>();
	public GameObject m_star;
	public GameObject m_icon;
	public GameObject m_process;
	public GameObject m_num;
	public GameObject m_rate;
	public GameObject m_luck_num;
	public GameObject m_gold;
	public GameObject m_jewel;
	public GameObject m_next_attr;
	public GameObject m_next_star;
	public GameObject m_extra_attr;
	public GameObject m_effect;
	public GameObject m_sx_button;
	public GameObject m_sx_button_ex;
	public GameObject m_dacheng_des;
	public GameObject m_gold_button;
	public GameObject m_jewel_button;
	public GameObject m_one;
	public GameObject m_two;
	public GameObject m_ysx;
	public GameObject m_stop;
	public GameObject m_close;
	public string m_out_message;
	public dhc.treasure_t m_treasure;
	private int select = 0;
	private List<int> types = new List<int>();
	private int select_num = 0;
	private bool m_play = false;
	private bool is_close = false;
	private bool is_mess = false;
	private bool is_yj = false;
	private bool is_yj_ex = false;
	// Use this for initialization
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		select = 0;
		types.Clear ();
		select_num = 0;
		is_close = false;
		is_mess = false;
		is_yj = false;
		is_yj_ex = false;
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_yj_treasure_sx)
		{
			m_one.SetActive(true);
			m_two.SetActive(false);
		}
		else
		{
			m_one.SetActive(false);
			m_two.SetActive(true);
			m_ysx.SetActive(true);
			m_stop.SetActive(false);
		}
		reset ();
		m_dacheng_des.SetActive (false);
	}

	public void reset()
	{
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		int max_level = 0;
		for(int i = 0 ;i < game_data._instance.m_dbc_baowu_sx.get_y(); ++ i)
		{
			int level = int.Parse(game_data._instance.m_dbc_baowu_sx.get(0,i));
			s_t_baowu_sx t_baowu_sx = game_data._instance.get_t_baowu_sx(level,t_treasure.font_color);
			if(t_baowu_sx != null)
			{
				max_level = level;
			}
		}
		for(int i = 0; i < m_stars.Count;++i)
		{
			m_stars[i].SetActive(false);
		}
		m_stars[0].transform.localPosition = new Vector3(-20*(max_level-1),-2,0);
		for(int i = 0; i < max_level;++i)
		{
			m_stars[i].SetActive(true);
		}
		m_star.GetComponent<UISprite>().width = 41 * m_treasure.star;
		for(int i = 0; i < m_treasure.star;++i)
		{
			m_star.transform.localPosition = new Vector3 (m_stars[0].transform.localPosition.x +1+20*i,-4,0);
		}
		s_t_baowu_sx _baowu_sx = game_data._instance.get_t_baowu_sx(m_treasure.star +1,t_treasure.font_color);
		sys._instance.remove_child (m_icon);
		GameObject icon1 = icon_manager._instance.create_treasure_icon(m_treasure.guid,false);
		icon1.transform.parent =  m_icon.transform;
		icon1.transform.localPosition = new Vector3(0,0,0);
		icon1.transform.localScale = new Vector3(1,1,1);
		icon1.transform.GetComponent<BoxCollider>().enabled = false;
		m_name.GetComponent<UILabel>().text = treasure.get_treasure_real_name (m_treasure.template_id);
		if(_baowu_sx != null)
		{
			string color = "";
			m_cur_attr.GetComponent<UILabel>().text = treasure.get_baowu_sx_text(m_treasure,m_treasure.star);
			m_process.GetComponent<UIProgressBar>().value = (float)m_treasure.star_exp / (float)_baowu_sx.process;
			m_num.GetComponent<UILabel>().text = m_treasure.star_exp.ToString() + "/" + _baowu_sx.process;
			m_luck_num.GetComponent<UILabel>().text = m_treasure.star_luck.ToString();
			if(sys._instance.m_self.m_t_player.gold < _baowu_sx.gold)
			{
				color = "[ff0000]";
			}
			else
			{
				color = sys._instance.get_res_color(1);
			}
			m_gold.GetComponent<UILabel>().text = color + _baowu_sx.gold.ToString();
			if(sys._instance.m_self.m_t_player.jewel < _baowu_sx.jewel)
			{
				color = "[ff0000]";
			}
			else
			{
				color = sys._instance.get_res_color(2);
			}
			m_jewel.GetComponent<UILabel>().text = color + _baowu_sx.jewel.ToString();
			m_next_attr.GetComponent<UILabel>().text = treasure.get_next_baowu_sx_text(m_treasure,m_treasure.star,1);
			m_extra_attr.GetComponent<UILabel>().text = treasure.get_next_baowu_sx_text(m_treasure,m_treasure.star,2);
			m_rate.GetComponent<UILabel>().text = rate();
			m_next_star.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("treasure_sx_gui.cs_144_60"),(m_treasure.star+1).ToString());//{0}星属性
		}
		else
		{
			m_treasure.star_exp = 0;
			m_cur_attr.GetComponent<UILabel>().text = treasure.get_baowu_sx_text(m_treasure,m_treasure.star);
			m_process.GetComponent<UIProgressBar>().value = 1.0f;
			m_num.GetComponent<UILabel>().text = "--/--";
			m_luck_num.GetComponent<UILabel>().text = m_treasure.star_luck.ToString();
			m_gold.GetComponent<UILabel>().text = "----";
			m_jewel.GetComponent<UILabel>().text = "----";
			m_next_attr.SetActive(false);
			m_extra_attr.SetActive(false);
			m_rate.GetComponent<UILabel>().text = "--";
			m_next_star.GetComponent<UILabel>().text = game_data._instance.get_t_language ("treasure_sx_gui.cs_158_46");//下一星属性
		}
		//m_play = false;

	}

	string rate()
	{
		string s = "";
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		s_t_baowu_sx t_baowu_sx = game_data._instance.get_t_baowu_sx (m_treasure.star+1,t_treasure.font_color);
		int luck_num = t_baowu_sx.rate + m_treasure.star_luck / 10;
		if(luck_num <= 60)
		{
			s = game_data._instance.get_t_language ("treasure_sx_gui.cs_172_7");//低
		}
		else if(luck_num <= 65)
		{
			s = game_data._instance.get_name_color(1) + game_data._instance.get_t_language ("treasure_sx_gui.cs_176_47");//较低
		}
		else if(luck_num <= 70)
		{
			s = game_data._instance.get_name_color(2) + game_data._instance.get_t_language ("treasure_sx_gui.cs_180_47");//一般
		}
		else if(luck_num <= 80)
		{
			s = game_data._instance.get_name_color(3) + game_data._instance.get_t_language ("treasure_sx_gui.cs_184_47");//较高
		}
		else if(luck_num <= 90)
		{
			s = game_data._instance.get_name_color(4) + game_data._instance.get_t_language ("treasure_sx_gui.cs_188_47");//高
		}
		else
		{
			s = game_data._instance.get_name_color(5) + game_data._instance.get_t_language ("treasure_sx_gui.cs_192_47");//极高
		}
		return s;
	}

	public void click(GameObject obj)
	{
		if(obj.name == "sx")
		{
			s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
			s_t_baowu_sx _baowu_sx = game_data._instance.get_t_baowu_sx(m_treasure.star +1,t_treasure.font_color);
			if(_baowu_sx == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_detail.cs_427_46"));//[ffc882]该饰品已升至满星
				return;
			}
			if(sys._instance.m_self.m_t_player.gold < _baowu_sx.gold&&select == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_jingjie_gui.cs_346_46"));//[ffc882]金币不足
				return;
			}
			if(sys._instance.m_self.m_t_player.jewel < _baowu_sx.jewel && select == 1)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			select_num += 1;
			types.Add(select);
			if(m_treasure.star_rates.Count == select_num)
			{
				is_mess = true;
				protocol.game.cmsg_treasure_star _msg = new protocol.game.cmsg_treasure_star ();
				_msg.star_guid = m_treasure.guid;
				for(int i = 0; i < types.Count;++i)
				{
					_msg.types.Add(types[i]);
				}
				net_http._instance.send_msg<protocol.game.cmsg_treasure_star> (opclient_t.CMSG_TREASURE_STAR, _msg);
			}
			else
			{
				m_effect.GetComponent<Animator>().speed = 4.0f;
				m_effect.gameObject.SetActive (true);
				sys._instance.play_sound ("sound/gaizao");
				m_play = true;
				update_date();
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_yj_treasure_sx)
				{
					m_sx_button.GetComponent<UIButton>().isEnabled = false;
				}
				else
				{
					m_sx_button_ex.GetComponent<UIButton>().isEnabled = false;
				}
			}
			m_jewel_button.GetComponent<BoxCollider>().enabled = false;
			m_gold_button.GetComponent<BoxCollider>().enabled = false;
			m_close.GetComponent<BoxCollider>().enabled = false;
		}
		if(obj.name == "gold")
		{
			select = 0;
		}
		if(obj.name == "jewel")
		{
			select = 1;
		}
		if(obj.name == "close")
		{
			is_close = true;
			is_mess = true;
			protocol.game.cmsg_treasure_star _msg = new protocol.game.cmsg_treasure_star ();
			_msg.star_guid = m_treasure.guid;
			for(int i = 0; i < types.Count;++i)
			{
				_msg.types.Add(types[i]);
			}
			net_http._instance.send_msg<protocol.game.cmsg_treasure_star> (opclient_t.CMSG_TREASURE_STAR, _msg);
		}
		if(obj.name == "ysx")
		{
			s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
			s_t_baowu_sx _baowu_sx = game_data._instance.get_t_baowu_sx(m_treasure.star +1,t_treasure.font_color);
			if(_baowu_sx == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_detail.cs_427_46"));//[ffc882]该饰品已升至满星
				return;
			}
			if(sys._instance.m_self.m_t_player.gold < _baowu_sx.gold&&select == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_jingjie_gui.cs_346_46"));//[ffc882]金币不足
				return;
			}
			if(sys._instance.m_self.m_t_player.jewel < _baowu_sx.jewel && select == 1)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			m_ysx.SetActive(false);
			m_stop.SetActive(true);
			m_sx_button_ex.GetComponent<UIButton>().isEnabled = false;
			m_jewel_button.GetComponent<BoxCollider>().enabled = false;
			m_gold_button.GetComponent<BoxCollider>().enabled = false;
			m_close.GetComponent<BoxCollider>().enabled = false;
			is_yj = true;
			is_yj_ex = true;
		}
		if(obj.transform.name == "stop")
		{
			is_yj = false;
			is_yj_ex = false;
			m_stop.SetActive(false);
			m_ysx.SetActive(true);
		}
	}

	void update_date()
	{
		string text = "";
		bool flag = false;
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		s_t_baowu_sx _baowu_sx = game_data._instance.get_t_baowu_sx(m_treasure.star +1,t_treasure.font_color);
		if(m_treasure.star_rates[select_num -1] <= (_baowu_sx.rate + m_treasure.star_luck/10))
		{
			m_treasure.star_var += (5*_baowu_sx.value1*m_treasure.star_bjs[select_num -1])/100;
			int exp = m_treasure.star_exp;
			int star_var = m_treasure.star_var;
			float bj = (float)m_treasure.star_bjs[select_num -1]/100;
			m_treasure.star_exp += (5*m_treasure.star_bjs[select_num -1])/100;
			if(m_treasure.star_exp >= _baowu_sx.process)
			{
				flag = true;
				exp = _baowu_sx.process - exp;
				m_treasure.star_luck = 0;
				m_treasure.star_exp = 0;
				m_treasure.star += 1;
				text += game_data._instance.get_t_language ("treasure_sx_gui.cs_328_12");//成功{nn}
				if(bj == 1.5f)
				{
					text += "[f92020]" + string.Format(game_data._instance.get_t_language ("treasure_sx_gui.cs_331_40") ,bj ) +  "\n";//{0}倍暴击[-]
				}
				if(bj > 1.5f)
				{
					text += "[ffff00]" + string.Format(game_data._instance.get_t_language ("treasure_sx_gui.cs_331_40"),bj )+ "\n";//{0}倍暴击[-]
				}
				m_treasure.star_var += _baowu_sx.process*_baowu_sx.value2;
				if(m_treasure.star_var > _baowu_sx.valuemax)
				{
					m_treasure.star_var = _baowu_sx.valuemax;
				}
				string s = game_data._instance.get_value_string(t_treasure.attr1, (_baowu_sx.value1+_baowu_sx.value2)*_baowu_sx.process);
				if(m_treasure.star_var > _baowu_sx.valuemax)
				{
					s = game_data._instance.get_value_string(t_treasure.attr1, _baowu_sx.valuemax - star_var);
				}
				if(t_treasure.attr1 == 1)
				{
					s = s.Replace(game_data._instance.get_t_language ("treasure.cs_548_23"),game_data._instance.get_t_language ("treasure.cs_548_28"));//生命//防御
				}
				text +=  "[00ff00]" + s + "[-]\n";
				text += game_data._instance.get_t_language ("treasure_sx_gui.cs_352_12") + exp.ToString() + "[-]";//[0aff16]经验 +
			}
			else
			{
				text += text += game_data._instance.get_t_language ("treasure_sx_gui.cs_356_20") + "\n";//成功
				if(bj == 1.5f)
				{
					text += "[f92020]" + bj + game_data._instance.get_t_language ("treasure_sx_gui.cs_359_31") + "\n";//倍暴击[-]
				}
				if(bj > 1.5f)
				{
					text += "[ffff00]" + bj + game_data._instance.get_t_language ("treasure_sx_gui.cs_359_31") + "\n";//倍暴击[-]
				}
				string s = game_data._instance.get_value_string(t_treasure.attr1,(int)(_baowu_sx.value1*bj*5));
				if(t_treasure.attr1 == 1)
				{
					s = s.Replace(game_data._instance.get_t_language ("treasure.cs_548_23"),game_data._instance.get_t_language ("treasure.cs_548_28"));//生命//防御
				}
				text +=  "[00ff00]" + s+ "[-]\n";
				text += game_data._instance.get_t_language ("treasure_sx_gui.cs_371_12") + " +" + (int)(5*bj) + "[-]";//[0aff16]经验
			}
		}
		else
		{
			text += text += game_data._instance.get_t_language ("treasure_sx_gui.cs_376_19") + "\n";//失败
			text += game_data._instance.get_t_language ("treasure_sx_gui.cs_377_11");//幸运值 +10
			m_treasure.star_luck += 10;
		}
		if(select == 0)
		{
			sys._instance.m_self.sub_att(e_player_attr.player_gold,_baowu_sx.gold,game_data._instance.get_t_language ("treasure_sx_gui.cs_382_73"));//宝物升星消耗
			m_treasure.star_gold += _baowu_sx.gold;
		}
		if(select == 1)
		{
			sys._instance.m_self.sub_att(e_player_attr.player_jewel,_baowu_sx.jewel,game_data._instance.get_t_language ("treasure_sx_gui.cs_382_73"));//宝物升星消耗
			m_treasure.star_jewel += _baowu_sx.jewel;
		}
		reset();
		m_dacheng_des.SetActive(false);
		hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();
		if(_hide_Label != null)
		{
			Destroy(_hide_Label);
		}
		m_dacheng_des.GetComponent<UILabel>().text = text;
		dacheng ();
		if(flag)
		{
			if(is_yj)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_sx_gui.cs_403_46"));//[ffc882]饰品提升一星
				is_yj = false;
				is_yj_ex = false;
				m_sx_button_ex.GetComponent<UIButton>().isEnabled = true;
				m_jewel_button.GetComponent<BoxCollider>().enabled = true;
				m_gold_button.GetComponent<BoxCollider>().enabled = true;
				m_close.GetComponent<BoxCollider>().enabled = true;
				m_stop.SetActive(false);
				m_ysx.SetActive(true);
			}
		}
		if(is_mess)
		{
			select_num = 0;
			is_mess = false;
		}

	}

	void IMessage.message (s_message message)
	{

	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_TREASURE_STAR)
		{
			protocol.game.smsg_treasure_star _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_star> (message.m_byte);
			if(select_num == 20)
			{
				update_date();
			}
			m_treasure.star_bjs.Clear();
			m_treasure.star_rates.Clear();
			for(int i = 0; i < _msg.bjs.Count;++i)
			{
				m_treasure.star_bjs.Add(_msg.bjs[i]);
				m_treasure.star_rates.Add(_msg.rates[i]);
			}
			types.Clear();
			m_effect.GetComponent<Animator>().speed = 4.0f;
			m_effect.gameObject.SetActive (true);
			sys._instance.play_sound ("sound/gaizao");
			m_play = true;
			if(!is_yj)
			{
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_yj_treasure_sx)
				{
					m_sx_button.GetComponent<UIButton>().isEnabled = false;
				}
				else
				{
					m_sx_button_ex.GetComponent<UIButton>().isEnabled = false;
				}
				m_close.GetComponent<BoxCollider>().enabled = false;
			}
			if(is_close)
			{
				Object.Destroy(this.gameObject);
				
				s_message _msg1= new s_message();
				_msg1.m_type = m_out_message;
                _msg1.m_ints.Add(5);
				_msg1.m_long.Add(m_treasure.guid);
				cmessage_center._instance.add_message(_msg1);
				is_close = false;
			}
		}
	}

	bool isplay()
	{
		Animator  animator = m_effect.GetComponent<Animator>();
		AnimatorStateInfo animatorInfo; 
		animatorInfo =animator.GetCurrentAnimatorStateInfo(0); 
		if (animatorInfo.normalizedTime > 1.0f ) 
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	public void dacheng()
	{
		if(m_dacheng_des.GetComponent<UILabel>().text != "")
		{
			m_dacheng_des.SetActive(true);
			TweenScale _scale = sys._instance.add_scale_anim(m_dacheng_des.gameObject,0.2f,0.5f,1.2f,0);
			EventDelegate.Add(_scale.onFinished, delegate() 
			                  {
				hide();
			},true);
		}
	}
	
	public void hide()
	{
		hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();
		
		if(_hide_Label == null)
		{
			_hide_Label = m_dacheng_des.AddComponent<hide_Label>();
		}
		
		_hide_Label.m_time = 1.6f;
	}

	// Update is called once per frame
	void Update () {
		if (m_play && isplay() )
		{
			m_play = false;
			m_effect.gameObject.SetActive (false);
			if(!is_yj)
			{
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_yj_treasure_sx)
				{
					m_sx_button.GetComponent<UIButton>().isEnabled = true;
				}
				else
				{
					m_sx_button_ex.GetComponent<UIButton>().isEnabled = true;
				}
				m_close.GetComponent<BoxCollider>().enabled = true;
			}
			//update_date();
			if(is_yj)
			{
				is_yj_ex = true;
			}
			if(!is_yj)
			{
				m_jewel_button.GetComponent<BoxCollider>().enabled = true;
				m_gold_button.GetComponent<BoxCollider>().enabled = true;
			}
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
		if(is_yj && is_yj_ex && !is_mess)
		{
			is_yj_ex = false;
			s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
			s_t_baowu_sx _baowu_sx = game_data._instance.get_t_baowu_sx(m_treasure.star +1,t_treasure.font_color);
			if(_baowu_sx == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_detail.cs_427_46"));//[ffc882]该饰品已升至满星
				m_sx_button_ex.GetComponent<UIButton>().isEnabled = true;
				m_jewel_button.GetComponent<BoxCollider>().enabled = true;
				m_gold_button.GetComponent<BoxCollider>().enabled = true;
				m_close.GetComponent<BoxCollider>().enabled = true;
				m_stop.SetActive(false);
				m_ysx.SetActive(true);
				is_yj = false;
				is_yj_ex = false;
				return;
			}
			if(sys._instance.m_self.m_t_player.gold < _baowu_sx.gold&&select == 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_jingjie_gui.cs_346_46"));//[ffc882]金币不足
				m_sx_button_ex.GetComponent<UIButton>().isEnabled = true;
				m_jewel_button.GetComponent<BoxCollider>().enabled = true;
				m_gold_button.GetComponent<BoxCollider>().enabled = true;
				m_close.GetComponent<BoxCollider>().enabled = true;
				m_stop.SetActive(false);
				m_ysx.SetActive(true);
				is_yj = false;
				is_yj_ex = false;
				return;
			}
			if(sys._instance.m_self.m_t_player.jewel < _baowu_sx.jewel && select == 1)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				m_sx_button_ex.GetComponent<UIButton>().isEnabled = true;
				m_jewel_button.GetComponent<BoxCollider>().enabled = true;
				m_gold_button.GetComponent<BoxCollider>().enabled = true;
				m_stop.SetActive(false);
				m_ysx.SetActive(true);
				is_yj = false;
				is_yj_ex = false;
				return;
			}
			select_num += 1;
			types.Add(select);
			if(m_treasure.star_rates.Count == select_num)
			{
				is_mess = true;
				protocol.game.cmsg_treasure_star _msg = new protocol.game.cmsg_treasure_star ();
				_msg.star_guid = m_treasure.guid;
				for(int i = 0; i < types.Count;++i)
				{
					_msg.types.Add(types[i]);
				}
				net_http._instance.send_msg<protocol.game.cmsg_treasure_star> (opclient_t.CMSG_TREASURE_STAR, _msg);
			}
			else
			{
				m_effect.GetComponent<Animator>().speed = 4.0f;
				m_effect.gameObject.SetActive (true);
				sys._instance.play_sound ("sound/gaizao");
				m_play = true;
				update_date();
			}
		}
	}
}
