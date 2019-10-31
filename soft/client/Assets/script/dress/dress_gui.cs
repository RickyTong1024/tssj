using System.Collections.Generic;
using UnityEngine;

public class dress_gui : MonoBehaviour ,IMessage{

	public GameObject m_dress_page_gui;
	public GameObject m_dress_chengjiu_gui;
	private int m_item_id;
	public GameObject m_panel;
	public GameObject m_chengjiu_gui;
	public GameObject m_cj_icon1;
	public GameObject m_cj_icon2;
	public GameObject m_task_name1;
	public GameObject m_task_name2;
	public GameObject m_myname1;
	public GameObject m_myname2;
	public GameObject m_cur_attr;
	public GameObject m_wu;
	public GameObject m_next_attr;
	public GameObject m_tuzhi_num;
	public GameObject m_zg;
	public GameObject m_js;
	public GameObject m_effect;
	public GameObject m_sx;
	public GameObject m_num;
	public GameObject m_bar;
	public GameObject m_collect_num;
	public GameObject m_cj_effect;
	public string m_message = "";
	public string erro = "";
	s_t_dress_target _dress_target;
	private bool is_player;

	public UILabel m_shizhuang;
	public UILabel m_chengjiu;
	public UILabel m_taozhuang;
	public UILabel m_shizhuang1;
	public UILabel m_chengjiu1;
	public UILabel m_taozhuang1;

	void Start () {


		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void init ()
	{
		sys._instance.remove_child (m_panel);
		m_dress_page_gui = game_data._instance.ins_object_res ("ui/dress_page_gui");
		m_dress_page_gui.transform.parent = m_panel.transform;
		m_dress_page_gui.transform.localPosition = new Vector3 (0,0,0);
		m_dress_page_gui.transform.localScale = new Vector3 (1,1,1);
		m_dress_page_gui.SetActive (true);
	}
	
	public void reset()
	{
		shizhuang ();
	}

	void hide_page()
	{
		Transform _obj = this.transform.Find("main_button");
		_obj.Find("shi_zhuang").GetComponent<UIToggle>().value = true;
	}
	
	public void click (GameObject obj)
	{
		if(obj.transform.name == "hide_dress")
		{
			hide_page ();
			transform.GetComponent<ui_show_anim>().hide_ui();
			m_dress_chengjiu_gui.GetComponent<chengjiu_gui>().flag=false;
			s_message _message = new s_message ();
			_message.m_type = m_message;
			cmessage_center._instance.add_message (_message);
		}
		else if (obj.transform.name == "shi_zhuang")
		{
			shizhuang ();
		}
		else if (obj.transform.name == "cheng_jiu")
		{
			chengjiu ();
		}
		else if (obj.transform.name == "tao_zhuang")
		{
			taozhuang ();
		}
		else if(obj.transform.name == "js")
		{
			if(sys._instance.m_self.m_t_player.dress_ids.Count < _dress_target.defs[1])
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("dress_gui.cs_99_46"));//[ffc882]收集时装数量不足
				return;
			}
			if(erro != "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + erro);
				return;
			}
			is_player = true;
			m_effect.SetActive(true);
			m_effect.GetComponent<UISpriteAnimation>().ResetToBeginning();
			protocol.game.cmsg_dress_unlock_achieve _msg = new protocol.game.cmsg_dress_unlock_achieve ();
			_msg.achieve_id = _dress_target.id;
			net_http._instance.send_msg<protocol.game.cmsg_dress_unlock_achieve> (opclient_t.CMSG_DRESS_UNLOCK_ACHIEVE, _msg);
		}
	}
	
	public void shizhuang()
	{		
		hide_page ();
		m_dress_page_gui.SetActive (true);
		m_dress_chengjiu_gui.SetActive (false);
		m_chengjiu_gui.SetActive (false);
		m_dress_page_gui.GetComponent<dress_page_gui>().init ();
		setProgress ();
		m_dress_page_gui.GetComponent<dress_page_gui>().dress_reset ();
		if(is_effect())
		{
			m_cj_effect.SetActive(true);
		}
		else
		{
			m_cj_effect.SetActive(false);
		}
	}
	public void setProgress()
	{
		//m_dress_page_gui.GetComponent<dress_page_gui>().progress.fillAmount = (float) sys._instance.m_self.m_t_player.dress_ids.Count/game_data._instance.m_dbc_dress.get_y();
		m_dress_page_gui.GetComponent<dress_page_gui>().shouji.text = sys._instance.m_self.m_t_player.dress_ids.Count + "/" + game_data._instance.m_dbc_dress.get_y ();
	}

	public void chengjiu()
	{		
		hide_page ();
		m_sx.SetActive (false);
		m_dress_page_gui.SetActive (false);
		//m_dress_chengjiu_gui.GetComponent<chengjiu_gui>().reset (1);
		m_effect.SetActive (false);
		m_dress_chengjiu_gui.SetActive (false);
		chengjiu_gui ();
		m_chengjiu_gui.SetActive (true);
	}

	public void taozhuang()
	{
		hide_page ();
		m_dress_chengjiu_gui.SetActive (true);
		m_dress_page_gui.SetActive (false);
		m_chengjiu_gui.SetActive (false);
		m_dress_chengjiu_gui.GetComponent<chengjiu_gui>().reset (2);
	}

	public void chengjiu_gui()
	{
		erro = "";
		_dress_target = null;
		for(int i = 0; i < game_data._instance.m_dbc_dress_target.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_dress_target.get(0,i));
			s_t_dress_target t_dress_target = game_data._instance.get_t_dress_target(id);
			if(t_dress_target.type == 1)
			{
				bool flag = m_dress_chengjiu_gui.GetComponent<chengjiu_gui>().check_dress_target_done(t_dress_target);
				if(!flag)
				{
					_dress_target = game_data._instance.get_t_dress_target(id);
					break;
				}
			}
		}
		if(_dress_target != null)
		{
			m_js.SetActive(true);
			m_zg.SetActive(false);
			if(sys._instance.m_self.m_t_player.dress_achieves.Count == 0)
			{
				m_wu.SetActive(true);
				m_cur_attr.SetActive(false);
				m_task_name1.GetComponent<UILabel>().text = "";
			}
			else
			{
				int id = sys._instance.m_self.m_t_player.dress_achieves[sys._instance.m_self.m_t_player.dress_achieves.Count-1];
				s_t_dress_target t_cur_dress_target = game_data._instance.get_t_dress_target(id);
				m_cur_attr.SetActive(true);
				m_task_name1.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) + t_cur_dress_target.name;
				m_wu.SetActive(false);
				m_cur_attr.GetComponent<UILabel>().text = "[0aabff]" + get_attr(t_cur_dress_target.id);
			}
			m_task_name2.GetComponent<UILabel>().text =  game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count +1) + _dress_target.name;
			m_num.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.dress_ids.Count.ToString() + "/" + _dress_target.defs[1];
			m_collect_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_gui.cs_200_48") + _dress_target.defs[1] + game_data._instance.get_t_language ("dress_gui.cs_200_79");//收集//件时装
			m_bar.GetComponent<UIProgressBar>().value = (float)sys._instance.m_self.m_t_player.dress_ids.Count/(float)_dress_target.defs[1];
			m_next_attr.GetComponent<UILabel>().text = "[0aabff]" + get_attr(_dress_target.id);
			m_tuzhi_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_gui.cs_203_46") + "[0aff16]" + " " +sys._instance.m_self.m_t_player.dress_tuzhi + "/" + _dress_target.defs[0];//[0aabff]消耗图纸 [-]
			if(sys._instance.m_self.m_t_player.dress_tuzhi < _dress_target.defs[0])
			{
				erro = game_data._instance.get_t_language ("dress_gui.cs_206_11");//设计图数量不足
				m_tuzhi_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_gui.cs_203_46") + "[ff0000]" + " "  + sys._instance.m_self.m_t_player.dress_tuzhi + "/" + _dress_target.defs[0];//[0aabff]消耗图纸 [-]
			}
			sys._instance.remove_child (m_cj_icon1);
			GameObject _obj = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count
			                                                            ,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
			
			_obj.transform.parent = m_cj_icon1.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,0,0);

			sys._instance.remove_child (m_cj_icon2);
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count +1
			                                                            ,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
			
			_obj1.transform.parent = m_cj_icon2.transform;
			_obj1.transform.localScale = new Vector3(1,1,1);
			_obj1.transform.localPosition = new Vector3(0,0,0);
			m_myname1.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name;
			m_myname2.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count+1)+ sys._instance.m_self.m_t_player.name;
		}
		else
		{
			m_js.SetActive(false);
			m_zg.SetActive(true);
			m_wu.SetActive(false);
			m_tuzhi_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_gui.cs_203_46") + "[0aff16]" + " "  +  sys._instance.m_self.m_t_player.dress_tuzhi + "/--";//[0aabff]消耗图纸 [-]
			int id = sys._instance.m_self.m_t_player.dress_achieves[sys._instance.m_self.m_t_player.dress_achieves.Count-1];
			s_t_dress_target t_cur_dress_target = game_data._instance.get_t_dress_target(id);
			m_task_name1.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) + t_cur_dress_target.name;
			m_task_name2.GetComponent<UILabel>().text =  game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) + t_cur_dress_target.name;
			m_num.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.dress_ids.Count.ToString() + "/--";
			m_collect_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_gui.cs_200_48") + t_cur_dress_target.defs[1] + game_data._instance.get_t_language ("dress_gui.cs_238_84");//收集//套时装
			m_bar.GetComponent<UIProgressBar>().value = 1;
			m_next_attr.GetComponent<UILabel>().text = "[0aabff]" + get_attr(t_cur_dress_target.id);
			m_cur_attr.GetComponent<UILabel>().text = "[0aabff]" + get_attr(t_cur_dress_target.id);
			sys._instance.remove_child (m_cj_icon1);
			GameObject _obj = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count
			                                                            ,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
			
			_obj.transform.parent = m_cj_icon1.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,0,0);
			
			sys._instance.remove_child (m_cj_icon2);
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count
			                                                             ,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
			
			_obj1.transform.parent = m_cj_icon2.transform;
			_obj1.transform.localScale = new Vector3(1,1,1);
			_obj1.transform.localPosition = new Vector3(0,0,0);
			m_myname1.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count)+ sys._instance.m_self.m_t_player.name;;
			m_myname2.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count)+ sys._instance.m_self.m_t_player.name;
		}

	}

	public string get_attr(int _id)
	{
		Dictionary<int,int> attrs = new Dictionary<int, int>();
		string s = "";
		for(int i = 0; i < game_data._instance.m_dbc_dress_target.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_dress_target.get(0,i));
			s_t_dress_target _t_dress_target = game_data._instance.get_t_dress_target(id);
			if(attrs.ContainsKey(_t_dress_target.attr1))
			{
				attrs[_t_dress_target.attr1] += _t_dress_target.value1;
			}
			else
			{
				attrs.Add(_t_dress_target.attr1,_t_dress_target.value1);
			}
			if(attrs.ContainsKey(_t_dress_target.attr2))
			{
				attrs[_t_dress_target.attr2] += _t_dress_target.value2;
			}
			else
			{
				attrs.Add(_t_dress_target.attr2,_t_dress_target.value2);
			}
			if(attrs.ContainsKey(_t_dress_target.attr3))
			{
				attrs[_t_dress_target.attr3] += _t_dress_target.value3;
			}
			else
			{
				attrs.Add(_t_dress_target.attr3,_t_dress_target.value3);
			}
			if(attrs.ContainsKey(_t_dress_target.attr4))
			{
				attrs[_t_dress_target.attr4] += _t_dress_target.value4;
			}
			else
			{
				attrs.Add(_t_dress_target.attr4,_t_dress_target.value4);
			}
			if(_id == id)
			{
				break;
			}
		}
		bool flag = false;
		foreach(int key in attrs.Keys)
		{
			if(attrs[key] > 0)
			{
				if(!flag)
				{
					flag = true;
					s += game_data._instance.get_value_string(key,attrs[key],1);
				}
				else
				{
					s += "\n\n" + game_data._instance.get_value_string(key,attrs[key],1);
				}
			}
		}
		return s;
	}

	public static bool is_effect()
	{
		s_t_dress_target _t_dress_target = null;
		for(int i = 0; i < game_data._instance.m_dbc_dress_target.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_dress_target.get(0,i));
			s_t_dress_target t_dress_target = game_data._instance.get_t_dress_target(id);
			if(t_dress_target.type == 1)
			{
				bool flag = false;
				for( int j = 0; j <sys._instance.m_self.m_t_player.dress_achieves.Count; ++j)
				{
					if(t_dress_target.id == sys._instance.m_self.m_t_player.dress_achieves[j] )
					{
						flag = true;
						break;
					}
				}
				if(!flag)
				{
					_t_dress_target = game_data._instance.get_t_dress_target(id);
					break;
				}
			}
		}
		if(_t_dress_target == null)
		{
			return false;
		}
		else
		{
			if(sys._instance.m_self.m_t_player.dress_ids.Count >= _t_dress_target.defs[1]
			   && sys._instance.m_self.m_t_player.dress_tuzhi >= _t_dress_target.defs[0])
			{
				return true;
			}
		}
		return false;
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_DRESS_UNLOCK_ACHIEVE)
		{
			protocol.game.smsg_success  _msg = net_http._instance.parse_packet<protocol.game.smsg_success>(message.m_byte);
			if (_msg.success)
			{
				sys._instance.m_self.m_t_player.dress_achieves.Add(_dress_target.id);
				sys._instance.m_self.sub_att(e_player_attr.player_dress_tuzhi,_dress_target.defs[0]);
				string text = "";
				for(int i = 0; i < sys._instance.m_self.player_dress_chenjiu_check(_dress_target.id).Count;++i)
				{
					if(i ==0)
					{
						text += sys._instance.m_self.player_dress_chenjiu_check(_dress_target.id)[i];
					}
					else
					{
						text += "\n"+sys._instance.m_self.player_dress_chenjiu_check(_dress_target.id)[i];
					}
				}
				hide_Label _hide_Label = m_sx.GetComponent<hide_Label>();
				if(_hide_Label != null)
				{
					Destroy(_hide_Label);
				}
				m_sx.GetComponent<UILabel>().text = text;
				dacheng();
				if(is_effect())
				{
					m_cj_effect.SetActive(true);
				}
				else
				{
					m_cj_effect.SetActive(false);
				}
				chengjiu_gui();
			}
		}
	}

	public void dacheng()
	{
		if(m_sx.GetComponent<UILabel>().text != "")
		{
			m_sx.SetActive(true);
			TweenScale _scale = sys._instance.add_scale_anim(m_sx.gameObject,0.2f,0.5f,1.2f,0);
			EventDelegate.Add(_scale.onFinished, delegate() 
			                  {
				hide();
			},true);
		}
	}
	
	public void hide()
	{
		hide_Label _hide_Label = m_sx.GetComponent<hide_Label>();
		
		if(_hide_Label == null)
		{
			_hide_Label = m_sx.AddComponent<hide_Label>();
		}
		
		_hide_Label.m_time = 1.6f;
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "hide_dress")
		{
			hide_page ();
			transform.GetComponent<ui_show_anim>().hide_ui ();
			m_dress_chengjiu_gui.GetComponent<chengjiu_gui>().flag = false;
			s_message _message = new s_message ();
			_message.m_type = m_message;
			cmessage_center._instance.add_message (_message);
		}
		if (message.m_type == "update_dress_gui") 
		{
			setProgress();
		}
	}
	// Update is called once per frame
	void Update () {
		if(is_player)
		{
			if(!m_effect.GetComponent<UISpriteAnimation>().isPlaying)
			{
				m_effect.SetActive(false);
			}
		}
	}

}
