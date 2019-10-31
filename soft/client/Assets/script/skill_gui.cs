
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class skill_gui : MonoBehaviour,IMessage {
	
	public UILabel m_skill_name;
	public UILabel m_now_level;
	public UILabel m_next_level;
	public UILabel m_now_des;
	public UILabel m_gold;
	public UILabel m_next_des;
	public UILabel m_level;
	public List<role_skill> role_skills = new List<role_skill>();
	public GameObject m_select;
	public GameObject[] m_skills;
	public GameObject m_name;
	public GameObject m_skill_up_panel;
	public GameObject m_zs_skill_up_panel;
	public GameObject m_item_icon;
	public GameObject m_skill;
	public GameObject m_skill_point;

	//专属技能
	public GameObject m_zs_skill_icon;
	public UILabel m_zs_skill_name;
	public UILabel m_zs_level;
	public UILabel m_zs_taici;
	public UILabel m_zs_skill_desc;
	public GameObject m_scro;
	public GameObject m_view;
	public GameObject m_cl_icon;
	public UILabel m_zs_gold;
	public GameObject m_tishi;
	public GameObject m_zs_skill;
	public GameObject m_zs_skill_point;
	public GameObject m_zs_skill_button_Label;

	private int m_id = 0;
	private string error = "";
	private int flat = 0;
    private const uint stoneid = 50090001;
	public List<int> m_gongzhens = new List<int>();
	private ccard m_card;
	public ulong m_guid;
	public GameObject m_sj;

	// Use this for initialization
	void Start () {
		
		cmessage_center._instance.add_handle (this);

	}

	void OnEnable()
	{
		sys._instance.m_card = m_card;
		sys._instance.m_gongzhengs.Clear();
		sys._instance.m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur (m_card);
	}


	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void click(GameObject obj)
	{
		if (obj.name == "close")
		{			
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			if(sys._instance.m_message_type.Count != 0)
			{
				s_message message1 = new s_message ();
				message1.m_type = sys._instance.m_message_type[0];
				cmessage_center._instance.add_message(message1);
				sys._instance.m_message_type.Clear();
			}
			s_message _message = new s_message();
			_message.m_type = "update_bu_zheng";
			_message.m_bools.Add(true);
			cmessage_center._instance.add_message(_message);

		}
		else if (obj.name == "levelup_max")
		{
			if (error != "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + error);
				return;
			}
			if (error == "" && flat == 1)
			{
				s_message message1 = new s_message ();
				message1.m_type = "show_cl_gui";
				message1.m_ints.Add ((int)stoneid);
				cmessage_center._instance.add_message(message1);
				sys._instance.m_message_type.Clear ();
				sys._instance.m_message_type.Add ("show_bu_zheng_gui");
				sys._instance.m_message_type.Add ("show_jn_gui");
				sys._instance.m_message_long.Clear ();
				sys._instance.m_message_long.Add (m_card.get_guid ());
				return;
			}
			int index = 0;
			protocol.game.cmsg_role_skillup _msg = new protocol.game.cmsg_role_skillup ();
			_msg.role_guid = m_card.get_guid();
			role_skill skill = m_card.m_skills[(int)e_skill_type.skill_type_active];
			if(role_skills[m_id] == skill)
			{
				index = (int)e_skill_type.skill_type_active -1;
			}
			else
			{
				for(int i = (int)e_skill_type_ex.skill_type_jlevel_1;i < (int)e_skill_type_ex.skill_end;i ++)
				{
					role_skill _skill = m_card.m_skills[i+2];
					if(role_skills[m_id] == _skill)
					{
						index = i;
						break;
					}
				}
			}
			_msg.index = index;
			net_http._instance.send_msg<protocol.game.cmsg_role_skillup> (opclient_t.CMSG_ROLE_SKILLUP, _msg);
		}
		else if(obj.transform.name == "skill")
		{
			m_skill_up_panel.SetActive(true);
			m_zs_skill_up_panel.SetActive(false);
			update_ui();
		}
		else if(obj.transform.name == "zs_skill")
		{
			if(m_card.m_t_class.color < 4)
			{
				m_zs_skill.GetComponent<UIToggle>().enabled = false;
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("skill_gui.cs_143_46"));//[ffc882]该伙伴没有专属天赋
				return;
			}
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_skill_zs)
			{
				m_zs_skill.GetComponent<UIToggle>().enabled = false;
			}
			else
			{
				m_zs_skill.GetComponent<UIToggle>().enabled = true;
			}
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_skill_zs)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("skill_gui.cs_156_73"),(int)e_open_level.el_skill_zs));//专属天赋{0}级开启，主人赶快提升等级吧
				return;
			}
			m_skill_up_panel.SetActive(false);
			m_zs_skill_up_panel.SetActive(true);
			update_ui_ex();
		}
		else if (obj.name == "levelup")
		{
			s_t_role_skillunlock t_role_skilunlock = game_data._instance.get_t_role_skillunlock (m_card.get_template_id(),m_card.get_role().bskill_level+1);
			if(t_role_skilunlock == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("skill_gui.cs_168_46"));//[ffc882]专属天赋已升至满级
				return;
			}
			if(!task_done (m_card))
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("skill_gui.cs_173_46"));//[ffc882]任务未全部完成
				return;
			}
			if (error != "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + error);
				return;
			}
			if (error == "" && flat == 1)
			{
				s_message message1 = new s_message ();
				message1.m_type = "show_cl_gui";
				message1.m_ints.Add ((int)stoneid);
				cmessage_center._instance.add_message(message1);
				sys._instance.m_message_type.Clear ();
				sys._instance.m_message_type.Add ("show_bu_zheng_gui");
				sys._instance.m_message_type.Add ("show_jn_gui_ex");
				sys._instance.m_message_long.Clear ();
				sys._instance.m_message_long.Add (m_card.get_guid ());
				return;
			}
			protocol.game.cmsg_role_bskillup _msg = new protocol.game.cmsg_role_bskillup ();
			_msg.role_guid = m_card.get_guid();
			net_http._instance.send_msg<protocol.game.cmsg_role_bskillup> (opclient_t.CMSG_ROLE_BSKILL_LEVELUP, _msg);
		}
	}
	
	public void select(GameObject obj)
	{
		int id = 0;
		id = int.Parse (obj.transform.name);
		int jlevel = (id -1) *3;
		if(id == 0)
		{
			jlevel = 0;
		}
		if(id - 1 == 0)
		{
			jlevel = 1;
		}
		if(m_card.get_jlevel() < jlevel)
		{
			s_t_jinjie _t_jinjie = game_data._instance.get_jinjie(jlevel); 
			string text = string.Format(game_data._instance.get_t_language ("skill_gui.cs_216_31") ,_t_jinjie.name);//达到{0}开启该技能
			root_gui._instance.show_prompt_dialog_box("[ffc882]" + text);
			return;
		}
		m_id = id;
		reset_skill ();
	}

	public void reset_skill()
	{
		error = "";
		flat = 0;
		m_gongzhens.Clear();
		m_gongzhens = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		m_select.transform.localPosition = m_skills [m_id].transform.localPosition;
		m_name.GetComponent<UILabel>().text = role_skills[m_id].m_t_skill.name;
		int level = 0;
		role_skill skill = m_card.m_skills[(int)e_skill_type.skill_type_active];
		if(role_skills[m_id] == skill)
		{
			level = m_card.get_role().jskill_level [0];
		}
		else
		{
			for(int i = (int)e_skill_type_ex.skill_type_jlevel_1;i < (int)e_skill_type_ex.skill_end;i ++)
			{
				int index = i + 2;
				role_skill _skill = m_card.m_skills[index];
				if(role_skills[m_id] == _skill)
				{
					level = m_card.get_role().jskill_level [i];
					break;
				}
			}
		}
		s_t_class t_class = m_card.m_t_class;
		s_t_skillup t_skillup = game_data._instance.get_skillup (level);
		s_t_skillup t_skillup1 = game_data._instance.get_skillup (level + 1);
		m_now_level.GetComponent<UILabel>().text = level.ToString ();
		m_now_des.GetComponent<UILabel>().text = role_skills[m_id].get_des(t_skillup.level);
        
		if (t_skillup1 == null)
		{
			m_next_level.GetComponent<UILabel>().text = "----";
			m_gold.text = "----";
			m_next_des.GetComponent<UILabel>().text = "[0aabff]" + game_data._instance.get_t_language ("skill_gui.cs_262_58");//该技能已升至顶级
			int num = sys._instance.m_self.get_item_num (stoneid);
			sys._instance.remove_child(m_item_icon);
			GameObject m_icon = icon_manager._instance.create_item_icon_ex ((int)stoneid, num, -1);
			m_icon.transform.parent = m_item_icon.transform;
			m_icon.transform.localPosition = new Vector3 (0, 0, 0);
			m_icon.transform.localScale = new Vector3 (1, 1, 1);
			UIButtonMessage[] meses = m_icon.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "skill_click";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			error = game_data._instance.get_t_language ("skill_gui.cs_262_58");//该技能已升至顶级
		}
		else
		{
			
            m_next_des.GetComponent<UILabel>().text = role_skills[m_id].get_des(t_skillup1.level);
            m_next_level.GetComponent<UILabel>().text = level + 1 + "";
			string _text3;
			if (sys._instance.m_self.get_att(e_player_attr.player_gold)  >= t_skillup1.gold)
			{
				_text3 = sys._instance.get_res_color(1);
				_text3 +=  t_skillup1.gold.ToString();
			}
			else
			{
				_text3 = "[ff0000]";
				_text3 +=  t_skillup1.gold.ToString();
				error = game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
				flat = 0;
			}
            if (sys._instance.m_self.get_item_num(stoneid) < t_skillup1.skillstone)
            {
				flat = 1;
            }
			int num = sys._instance.m_self.get_item_num (stoneid);
			sys._instance.remove_child(m_item_icon);
			GameObject m_icon = icon_manager._instance.create_item_icon_ex ((int)stoneid, num, t_skillup1.skillstone);
			m_icon.transform.parent = m_item_icon.transform;
			m_icon.transform.localPosition = new Vector3 (0, 0, 0);
			m_icon.transform.localScale = new Vector3 (1, 1, 1);
			UIButtonMessage[] meses = m_icon.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "skill_click";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			m_gold.text = _text3;
		}

	}

	public void skill_click(GameObject obj)
	{
		int id = obj.transform.GetComponent<item_icon>().m_item_id;
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add (id);
		cmessage_center._instance.add_message(message);
		sys._instance.m_message_type.Clear ();
		sys._instance.m_message_type.Add ("show_bu_zheng_gui");
		if(m_zs_skill_up_panel.activeSelf)
		{
			sys._instance.m_message_type.Add ("show_jn_gui_ex");
		}
		else
		{
			sys._instance.m_message_type.Add ("show_jn_gui");
		}
		sys._instance.m_message_long.Clear ();
		sys._instance.m_message_long.Add (m_card.get_guid ());
	}
	
	void IMessage.message(s_message message)
	{
		
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ROLE_SKILLUP)
		{
			protocol.game.smsg_role_skillup _msg = net_http._instance.parse_packet<protocol.game.smsg_role_skillup> (message.m_byte);
			role_skill skill = m_card.m_skills[(int)e_skill_type.skill_type_active];
			int level = 0 ;
			if(role_skills[m_id] == skill)
			{
				m_card.get_role().jskill_level [0] += 1;
				level = m_card.get_role().jskill_level [0];
			}
			else
			{
				for(int i = (int)e_skill_type_ex.skill_type_jlevel_1;i < (int)e_skill_type_ex.skill_end;i ++)
				{
					int index = i + 2;
					role_skill _skill = m_card.m_skills[index];
					if(role_skills[m_id] == _skill)
					{
						m_card.get_role().jskill_level [i] += 1;
						level = m_card.get_role().jskill_level [i];
						break;
					}
				}
			}
			s_t_skillup t_skillup = game_data._instance.get_skillup (level);
			sys._instance.m_self.sub_att(e_player_attr.player_gold,t_skillup.gold,game_data._instance.get_t_language ("skill_gui.cs_371_73"));//技能升级消耗
            sys._instance.m_self.remove_item(stoneid, t_skillup.skillstone, game_data._instance.get_t_language ("skill_gui.cs_371_73"));//技能升级消耗
			sys._instance.m_self.add_active(830, 1);
			sys._instance.m_card = m_card;
			string s = sys._instance.m_self.gongzhengs_ex(m_card,m_gongzhens);
			if(s != "")
			{
				root_gui._instance.show_prompt_dialog_box(s);
			}
			update_ui();
			m_skills[m_id].transform.Find("effect").gameObject.SetActive(true);
			m_skills[m_id].transform.Find("effect").gameObject.GetComponent<TweenAlpha>().enabled = true;
			m_skills[m_id].transform.Find("effect").gameObject.GetComponent<TweenAlpha>().ResetToBeginning();
			m_select.transform.GetComponent<TweenAlpha>().enabled = true;
			m_select.transform.GetComponent<TweenAlpha>().ResetToBeginning();
			m_select.transform.GetComponent<TweenAlpha>().AddOnFinished(reset_effect);
			sys._instance.play_sound_ex("sound/skill_up");
			s_message _message3 = new s_message();
			_message3.m_type = "check_bf";
			cmessage_center._instance.add_message(_message3);
			if(is_skill(m_card))
			{
				m_skill_point.SetActive(true);
			}
			else
			{
				m_skill_point.SetActive(false);
			}
			if(is_zs_skill(m_card))
			{
				m_zs_skill_point.SetActive(true);
			}
			else
			{
				m_zs_skill_point.SetActive(false);
			}
		}
		if(message.m_opcode == opclient_t.CMSG_ROLE_BSKILL_LEVELUP)
		{
			m_card.get_role().bskill_level += 1;
			s_t_role_spskillup t_role_spskillup = game_data._instance.get_t_role_spskillup (m_card.get_role().bskill_level);
			sys._instance.m_self.sub_att(e_player_attr.player_gold,t_role_spskillup.gold,game_data._instance.get_t_language ("skill_gui.cs_412_80"));//专属技能升级消耗
            sys._instance.m_self.remove_item(stoneid, t_role_spskillup.stone, game_data._instance.get_t_language ("skill_gui.cs_412_80"));//专属技能升级消耗
			update_ui_ex();
			s_message _message3 = new s_message();
			_message3.m_type = "check_bf";
			cmessage_center._instance.add_message(_message3);
			if(is_zs_skill(m_card))
			{
				m_zs_skill_point.SetActive(true);
			}
			else
			{
				m_zs_skill_point.SetActive(false);
			}
		}
	}

	public void reset_effect()
	{
		m_select.transform.GetComponent<TweenAlpha>().enabled = false;
		for(int i = 0; i < role_skills.Count;++i)
		{
			m_skills[i].transform.Find("effect").gameObject.SetActive(false);
			m_skills[i].transform.Find("effect").gameObject.GetComponent<TweenAlpha>().enabled = false;
		}
	}

	public void reset(ulong guid,int type = 0)
	{
		m_guid = guid;
		m_card = sys._instance.m_self.get_card_guid (m_guid);
		m_id = 0;
		m_skill_up_panel.SetActive (true);
		m_zs_skill_up_panel.SetActive (false);
		if(type == 0)
		{
			m_zs_skill_up_panel.SetActive(false);
			m_skill_up_panel.SetActive(true);
			update_ui ();
		}
		else
		{
			m_zs_skill_up_panel.SetActive(true);
			m_skill_up_panel.SetActive(false);
			update_ui_ex ();
		}
		reset_effect ();
		if(is_skill(m_card))
		{
			m_skill_point.SetActive(true);
		}
		else
		{
			m_skill_point.SetActive(false);
		}
		if(is_zs_skill(m_card))
		{
			m_zs_skill_point.SetActive(true);
		}
		else
		{
			m_zs_skill_point.SetActive(false);
		}

	}

	public void zs_skill()
	{
		m_zs_skill.GetComponent<UIToggle>().value = true;
		m_skill.GetComponent<UIToggle>().value = false;
	}

	public void update_ui()
	{
		role_skills.Clear ();
		Transform _obj_2 = m_skill_up_panel.transform.Find("1/Scroll View");
		
		int _id = 0;
		int _num = 0;

		GameObject item = _obj_2.Find(_id.ToString()).gameObject;
		
		GameObject _icon = item.transform.Find("icon").gameObject;
		sys._instance.remove_child(_icon);
		sys._instance.remove_child(m_item_icon);
		int skill_enhance_up = game_data._instance.m_dbc_skillup.get_y ();
		role_skill skill = m_card.m_skills[(int)e_skill_type.skill_type_active];
		if(skill != null)
		{
			role_skills.Add(skill);
			item.SetActive(true);
			item.transform.Find("g").gameObject.SetActive(false);
			item.transform.Find("lock").gameObject.SetActive(false);
			int level = m_card.get_role().jskill_level [0];
			item.transform.Find("jn/jn").GetComponent<UILabel>().text = skill.m_t_skill.name + " " + (level).ToString() + "/" + skill_enhance_up;
			sys._instance.remove_child(_icon);
			GameObject iicon1 = icon_manager._instance.create_skill_icon(skill);
			iicon1.transform.parent =  _icon.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			_id ++;
		}

		for(int i = (int)e_skill_type_ex.skill_type_jlevel_1;i < (int)e_skill_type_ex.skill_end;i ++)
		{
			GameObject _item = _obj_2.Find(_id.ToString()).gameObject;

			GameObject m_icon = _item.transform.Find("icon").gameObject;
			sys._instance.remove_child(m_icon);
			int index = i + 2;
			role_skill _skill = m_card.m_skills[index];
			if(_skill != null)
			{
				role_skills.Add(_skill);
				_item.SetActive(true);
				int jlevel = _num *3;
				if(_num  == 0)
				{
					jlevel = 1;
				}
				if(m_card.get_jlevel() >=  jlevel)
				{
					int level = m_card.get_role().jskill_level [i];
					_item.transform.Find("g").gameObject.SetActive(false);
					_item.transform.Find("lock").gameObject.SetActive(false);
					_item.transform.Find("jn/jn").GetComponent<UILabel>().text = _skill.m_t_skill.name + " " + (level).ToString() + "/" + skill_enhance_up;
				}
				else
				{
					_item.transform.Find("g").gameObject.SetActive(true);
					_item.transform.Find("lock").gameObject.SetActive(true);
					_item.transform.Find("jn/jn").GetComponent<UILabel>().text = _skill.m_t_skill.name;
				}
				sys._instance.remove_child(m_icon);
				GameObject iicon1 = icon_manager._instance.create_skill_icon(_skill);
				iicon1.transform.parent =  m_icon.transform;
				iicon1.transform.localPosition = new Vector3(0,0,0);
				iicon1.transform.localScale = new Vector3(1,1,1);
				_num ++;
			}
			else
			{
				_item.SetActive(false);
			}
			
			_id ++;
		}
		
		if(_num > 5)
		{
			m_skill_up_panel.transform.Find("1/down").gameObject.SetActive(true);
		}
		else
		{
			m_skill_up_panel.transform.Find("1/down").gameObject.SetActive(false);
		}
		reset_skill ();
	}

	public void update_ui_ex()//专属技能
	{
		flat = 0;
		error = "";
		sys._instance.remove_child (m_zs_skill_icon);
		m_scro.GetComponent<UIScrollView>().ResetPosition ();
		s_t_role_skillunlock t_role_skilunlock = game_data._instance.get_t_role_skillunlock (m_card.get_template_id(),m_card.get_role().bskill_level+1);
		if(t_role_skilunlock == null)
		{
			t_role_skilunlock = game_data._instance.get_t_role_skillunlock (m_card.get_template_id(),m_card.get_role().bskill_level);
		}
		s_t_role_spskillup t_role_spskillup = game_data._instance.get_t_role_spskillup (m_card.get_role().bskill_level+1);
		if( m_card.get_role ().bskill_level == 0)
		{
			m_zs_level.text = game_data._instance.get_t_language ("skill_gui.cs_586_21") + (m_card.get_role ().bskill_level + 1).ToString ();//等级:
			m_zs_skill_button_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language ("skill_gui.cs_587_58");//激活
		}
		else
		{
			m_zs_level.text = game_data._instance.get_t_language ("skill_gui.cs_586_21") + m_card.get_role ().bskill_level.ToString ();//等级:
			m_zs_skill_button_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_weiyang_gui.cs_170_15");//升级
		}
		int index = 0;
		for(int i = 0; i < m_card.m_skills.Count;++i)
		{
			if(m_card.m_skills[i].m_t_skill.id == t_role_skilunlock.id)
			{
				break;
			}
			index++;
		}
		GameObject m_icon = icon_manager._instance.create_skill_icon (m_card.m_skills [index]);
		m_icon.transform.parent = m_zs_skill_icon.transform;
		m_icon.transform.localPosition = new Vector3 (0, 0, 0);
		m_icon.transform.localScale = new Vector3 (1, 1, 1);
		m_zs_skill_name.text = m_card.m_skills[index]._t_skill.name;
		m_zs_taici.text = "[0aabff]" + t_role_skilunlock.taici + "[-]\n" + m_card.m_skills [index].get_des (0);
		string desc = "";
		for(int i = 2; i <= 5;++i)
		{
			if(i > m_card.get_role().bskill_level)
			{
				if(i == 5)
				{
					desc += game_data._instance.get_t_language ("skill_gui.cs_616_13") + i.ToString()+ "[-]  "+ "[777777]" +m_card.m_skills [index].get_des_ex (i-1) +"[-]";//[777777]等级
				}
				else
				{
					desc += game_data._instance.get_t_language ("skill_gui.cs_616_13") + i.ToString()+ "[-]  "+"[777777]" +m_card.m_skills [index].get_des_ex (i-1) + "[-]\n\n";//[777777]等级
				}
			}
			else
			{
				if(i == 5)
				{
					desc += game_data._instance.get_t_language ("skill_gui.cs_627_13") + i.ToString()+ "[-]  "+m_card.m_skills [index].get_des (i-1) +"[-]";//[0af6ff]等级
				}
				else
				{
					desc += game_data._instance.get_t_language ("skill_gui.cs_627_13") + i.ToString()+ "[-]  "+m_card.m_skills [index].get_des(i-1) + "[-]\n\n";//[0af6ff]等级
				}
			}
		}
		m_zs_skill_desc.text = desc;
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		if(t_role_spskillup != null)
		{
			for(int i = 0; i < t_role_skilunlock.role_skillunlock_tasks.Count;++i)
			{
				GameObject _card = game_data._instance.ins_object_res("ui/zs_skill_sub");
				
				_card.transform.parent = m_view.transform;
				_card.transform.name = t_role_skilunlock.role_skillunlock_tasks[i].task_type.ToString();
				_card.transform.localPosition = new Vector3(203,86- 86*i,0);
				_card.transform.localScale = new Vector3(1,1,1);
				_card.GetComponent<zs_skill_sub>().t_role_skillunlock_task = t_role_skilunlock.role_skillunlock_tasks[i];
				_card.GetComponent<zs_skill_sub>().m_card = m_card; 
				_card.GetComponent<zs_skill_sub>().reset();
				_card.SetActive (true);
			}
			m_tishi.SetActive(false);
			string color = "";
			if(sys._instance.m_self.m_t_player.gold < t_role_spskillup.gold)
			{
				color = "[ff0000]";
				error = game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
				flat = 0;
			}
			else
			{
				color = sys._instance.get_res_color(1);
			}
			m_zs_gold.text = color + t_role_spskillup.gold;
			int num = sys._instance.m_self.get_item_num (stoneid);
			if(num < t_role_spskillup.stone)
			{
				flat = 1;
			}
			sys._instance.remove_child(m_cl_icon);
			GameObject m_zs_icon = icon_manager._instance.create_item_icon_ex ((int)stoneid, num, t_role_spskillup.stone);
			m_zs_icon.transform.parent = m_cl_icon.transform;
			m_zs_icon.transform.localPosition = new Vector3 (0, 0, 0);
			m_zs_icon.transform.localScale = new Vector3 (1, 1, 1);
			UIButtonMessage[] meses = m_zs_icon.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "skill_click";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
		}
		else
		{
			m_tishi.SetActive(true);
			m_zs_gold.text = "----";
			sys._instance.remove_child(m_cl_icon);
			int num = sys._instance.m_self.get_item_num (stoneid);
			GameObject m_icon1 = icon_manager._instance.create_item_icon_ex ((int)stoneid, num, -1);
			m_icon1.transform.parent = m_cl_icon.transform;
			m_icon1.transform.localPosition = new Vector3 (0, 0, 0);
			m_icon1.transform.localScale = new Vector3 (1, 1, 1);
			UIButtonMessage[] meses = m_icon1.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "skill_click";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
		}
	}
	
	public static bool is_skill(ccard m_card)
	{
		bool flag = false;
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_skill)
		{
			return false;
		}
		int num = 0;
		for(int i = (int)e_skill_type_ex.skill_type_jlevel_1;i < (int)e_skill_type_ex.skill_end;i ++)
		{
			int index = i + 2;
			role_skill _skill = m_card.m_skills[index];
			if(_skill != null)
			{
				int jlevel = num *3;
				if(num  == 0)
				{
					jlevel = 1;
				}
				int level = m_card.get_role().jskill_level [i];
				s_t_skillup t_skillup = game_data._instance.get_skillup (level+1);
				num ++;
				if(t_skillup == null)
				{
					continue;
				}
                else if (sys._instance.m_self.get_item_num(stoneid) >= t_skillup.skillstone && m_card.get_jlevel() >= jlevel //m_card.get_level() >= t_skillup.role_level 
				        && sys._instance.m_self.get_att(e_player_attr.player_gold) >= t_skillup.gold)
				{
					flag = true;
					break;
				}
			}
		}
		if(flag)
		{
			return true;
		}
		role_skill skill = m_card.m_skills[(int)e_skill_type.skill_type_active];
		if(skill != null)
		{
			int level = m_card.get_role().jskill_level [0];
			s_t_skillup t_skillup = game_data._instance.get_skillup (level+1);
			if(t_skillup == null)
			{
				return false;
			}
            if (sys._instance.m_self.get_item_num(stoneid) >= t_skillup.skillstone && sys._instance.m_self.get_att(e_player_attr.player_gold) >= t_skillup.gold)
			{
				return true;
			}
		}
		return false;
	}

	static bool task_done(ccard m_card)
	{
		s_t_role_skillunlock t_role_skilunlock = game_data._instance.get_t_role_skillunlock (m_card.get_template_id(),m_card.get_role().bskill_level+1);
		if(t_role_skilunlock == null)
		{
			return false;
		}
		for(int i = 0; i < t_role_skilunlock.role_skillunlock_tasks.Count;++i)
		{
			s_t_role_skillunlock_task t_role_skillunlock_task = t_role_skilunlock.role_skillunlock_tasks[i];
			int num = 0;
			int achieve_num = 0;
			switch(t_role_skillunlock_task.task_type)
			{
			case 1:
				num = t_role_skillunlock_task.def1;
				break;
			case 2:
				num = t_role_skillunlock_task.def1;
				break;
			case 3:
				num = t_role_skillunlock_task.def1;
				break;
			case 4:
				num = t_role_skillunlock_task.def1;
				break;
			case 5:
				num = t_role_skillunlock_task.def1;
				break;
			case 6:
				num = t_role_skillunlock_task.def1;
				achieve_num = m_card.get_level();
				break;
			case 7:
				num = t_role_skillunlock_task.def1;
				achieve_num = m_card.get_jlevel();
				break;
			case 8:
				num = t_role_skillunlock_task.def1;
				achieve_num = m_card.get_glevel();
				break;
			case 9:
				num = 1;
				if(m_card.get_pinzhi() >= t_role_skillunlock_task.def1)
				{
					achieve_num = 1;
				}
				break;
			case 10:
				num = t_role_skillunlock_task.def2;
				achieve_num = m_card.get_role().jskill_level[t_role_skillunlock_task.def1];
				break;
			case 11:
				num = t_role_skillunlock_task.def1;
				int item_id = 0;
				foreach(int id in game_data._instance.m_dbc_item.m_index.Keys)
				{
					s_t_item t_item = game_data._instance.get_item(id);
					if(t_item.type == 9001 && t_item.def_1 == m_card.m_t_class.id)
					{
						item_id=t_item.id;
						break;
					}
				}
				for(int j = 0; j < sys._instance.m_self.m_t_player.huiyi_jihuos.Count;++j)
				{
					s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(sys._instance.m_self.m_t_player.huiyi_jihuos[j]);
					if(_sub.huiyis.Contains(item_id))
					{
						achieve_num++;
					}
				}
				break;
			case 12:
				s_t_role_dress t_role_dress = game_data._instance.get_t_role_dress(t_role_skillunlock_task.def1);
				num = 1;
				achieve_num = 0;
				for(int j = 0; j < m_card.get_role().dress_ids.Count;++j)
				{
					if(m_card.get_role().dress_ids[j] == t_role_skillunlock_task.def1)
					{
						achieve_num = 1;
						break;
					}
				}
				break;
			}
			if(t_role_skillunlock_task.task_type <= 5)
			{
				achieve_num = m_card.get_role().bskill_counts[t_role_skillunlock_task.task_type -1];
			}
			if(achieve_num < num)
			{

				return false;
			}
		}
		return true;
	}

	public static bool is_zs_skill(ccard m_card)
	{
		bool flag = true;
		int level = m_card.get_role ().bskill_level;
		s_t_role_spskillup t_role_spskillup = game_data._instance.get_t_role_spskillup (level+1);
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_skill_zs || m_card.m_t_class.color < 4 || t_role_spskillup == null)
		{
			return false;
		}
		flag = task_done (m_card);
		if (sys._instance.m_self.get_item_num(stoneid) >= t_role_spskillup.stone
		         && sys._instance.m_self.get_att(e_player_attr.player_gold) >= t_role_spskillup.gold && flag)
		{
			return true;
		}
		return false;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
