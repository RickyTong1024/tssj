
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class jinjie_gui : MonoBehaviour ,IMessage{
	
	// Use this for initialization
	public GameObject m_name;
	public GameObject m_jb;
	public GameObject m_level;
	public GameObject m_gold;
	public GameObject m_scro;
	public List<GameObject> m_icons;
	public ulong m_guid;
	private ccard m_card;
	private int m_tj;
	public GameObject m_jj_show_label;
	public GameObject m_back;
	public GameObject m_max_jj;

	public UILabel m_special_Label;
	public UILabel m_base_Label;
	public UILabel m_open_jx_0;
	public UILabel m_open_jx_1;
	public UILabel m_open_jx_2;
	public UILabel m_open_jx_3;
	public UILabel m_open_jx_4;
	public UILabel m_open_jx_5;
	public UILabel m_skill_Label_0;
	public UILabel m_skill_Label_1;
	public UILabel m_skill_Label_2;
	public UILabel m_skill_Label_3;
	public UILabel m_skill_Label_4;
	public UILabel m_skill_Label_5;
	public UILabel m_jinjie_Label;
	public UILabel m_jx_Label;
	public UILabel m_hp_add_Label;
	public UILabel m_attack_add_Label;
	public UILabel m_wf_add_Label;
	public UILabel m_mf_add_Label;
	public UILabel m_speed_add_Label;
	public UILabel m_skill_point_Label;
	public UILabel m_nedd_role_Level_Label;
	public UILabel m_need_gold_Label;
	public UILabel m_jj_ok_Label;

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
	}

	public void show_jj()
	{
		Transform _obj_0 = m_back.transform.Find("0");

		List<double> attrs = ccard.get_role_org_attr (m_card.get_template_id(), m_card.get_level(), m_card.get_jlevel(), m_card.get_glevel(), m_card.get_pinzhi());
		_obj_0.Find("hp/cur").GetComponent<UILabel>().text = ((int)attrs[1]).ToString();
		_obj_0.Find("attack/cur").GetComponent<UILabel>().text = ((int)attrs[2]).ToString();
		_obj_0.Find("pd/cur").GetComponent<UILabel>().text = ((int)attrs[3]).ToString();
		_obj_0.Find("md/cur").GetComponent<UILabel>().text = ((int)attrs[4]).ToString();
		_obj_0.Find("speed/cur").GetComponent<UILabel>().text = ((int)attrs[5]).ToString();

		int jlevel = m_card.get_jlevel ();
		s_t_jinjie t_jinjie_o = game_data._instance.get_jinjie (jlevel);
		s_t_jinjie t_jinjie = game_data._instance.get_jinjie (jlevel + 1);
		if (t_jinjie == null)
		{
			m_name.GetComponent<UILabel>().text = t_jinjie_o.name;
			m_level.GetComponent<UILabel>().text = "--";
			m_gold.GetComponent<UILabel>().text = "----";
			m_jb.GetComponent<UISprite>().spriteName = t_jinjie_o.icon;
			m_jb.GetComponent<UISprite>().MakePixelPerfect();
			_obj_0.Find("hp/next").GetComponent<UILabel>().text = "----";
			_obj_0.Find("attack/next").GetComponent<UILabel>().text = "----";
			_obj_0.Find("pd/next").GetComponent<UILabel>().text = "----";
			_obj_0.Find("md/next").GetComponent<UILabel>().text = "----";
			_obj_0.Find("speed/next").GetComponent<UILabel>().text = "----";
			for (int i = 0; i < m_icons.Count; ++i)
			{
				sys._instance.remove_child(m_icons[i]);
				m_icons[i].SetActive(false);
			}
			m_max_jj.SetActive(true);
			m_tj = 4;
		}
		else
		{
			for (int i = 0; i < m_icons.Count; ++i)
			{
				sys._instance.remove_child(m_icons[i]);
				m_icons[i].SetActive(true);
			}
			m_max_jj.SetActive(false);
			m_name.GetComponent<UILabel>().text = t_jinjie.name;
			if (m_card.get_level() < t_jinjie.level)
			{
				m_level.GetComponent<UILabel>().text = "[ff0000]" + t_jinjie.level.ToString();
				m_tj = 3;
			}
			else
			{
				m_level.GetComponent<UILabel>().text = "[6cff00]" +t_jinjie.level.ToString();
			}
			if (sys._instance.m_self.m_t_player.gold < t_jinjie.gold)
			{
				m_gold.GetComponent<UILabel>().text = "[ff0000]" + t_jinjie.gold.ToString();
				m_tj = 2;
			}
			else
			{
				m_gold.GetComponent<UILabel>().text = sys._instance.get_res_color(1) + t_jinjie.gold.ToString();
			}
			m_jb.GetComponent<UISprite>().spriteName = t_jinjie.icon;
			m_jb.GetComponent<UISprite>().MakePixelPerfect();

			attrs = ccard.get_role_org_attr (m_card.get_template_id(), m_card.get_level(), m_card.get_jlevel() + 1, m_card.get_glevel(), m_card.get_pinzhi());
			_obj_0.Find("hp/next").GetComponent<UILabel>().text = ((int)attrs[1]).ToString();
			_obj_0.Find("attack/next").GetComponent<UILabel>().text = ((int)attrs[2]).ToString();
			_obj_0.Find("pd/next").GetComponent<UILabel>().text = ((int)attrs[3]).ToString();
			_obj_0.Find("md/next").GetComponent<UILabel>().text = ((int)attrs[4]).ToString();
			_obj_0.Find("speed/next").GetComponent<UILabel>().text = ((int)attrs[5]).ToString();

			{
				int num = sys._instance.m_self.get_item_num ((uint)t_jinjie.clty);
				GameObject _icon = icon_manager._instance.create_item_icon_ex (t_jinjie.clty, num, t_jinjie.clty_num);
				_icon.transform.parent = m_icons[0].transform;
				_icon.transform.localPosition = new Vector3 (0, 0, 0);
				_icon.transform.localScale = new Vector3 (1, 1, 1);
				if (num < t_jinjie.clty_num)
				{
					m_tj = 1;
				}
				UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
				meses[0].target = this.gameObject;
				meses[0].functionName = "tp_click";
				meses[1].target = null;
				meses[1].functionName = "";
				meses[2].target = null;
				meses[2].functionName = "";
			}
			{
				int num = sys._instance.m_self.get_item_num ((uint)t_jinjie.clfy);
				int num1 = t_jinjie.clfy_num;
				if (m_card.get_prof() == 1)
				{
					num1 = t_jinjie.clfy_num1;
				}
				GameObject _icon = icon_manager._instance.create_item_icon_ex (t_jinjie.clfy, num, num1);
				_icon.transform.parent = m_icons[1].transform;
				_icon.transform.localPosition = new Vector3 (0, 0, 0);
				_icon.transform.localScale = new Vector3 (1, 1, 1);
				if (num < num1)
				{
					m_tj = 1;
				}
				UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
				meses[0].target = this.gameObject;
				meses[0].functionName = "tp_click";
				meses[1].target = null;
				meses[1].functionName = "";
				meses[2].target = null;
				meses[2].functionName = "";
			}
			{
				int num = sys._instance.m_self.get_item_num ((uint)t_jinjie.clgj);
				int num1 = t_jinjie.clgj_num;
				if (m_card.get_prof() == 2)
				{
					num1 = t_jinjie.clgj_num1;
				}
				GameObject _icon = icon_manager._instance.create_item_icon_ex (t_jinjie.clgj, num, num1);
				_icon.transform.parent = m_icons[2].transform;
				_icon.transform.localPosition = new Vector3 (0, 0, 0);
				_icon.transform.localScale = new Vector3 (1, 1, 1);
				if (num < num1)
				{
					m_tj = 1;
				}
				UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
				meses[0].target = this.gameObject;
				meses[0].functionName = "tp_click";
				meses[1].target = null;
				meses[1].functionName = "";
				meses[2].target = null;
				meses[2].functionName = "";
			}
			{
				int num = sys._instance.m_self.get_item_num ((uint)t_jinjie.clmf);
				int num1 = t_jinjie.clmf_num;
				if (m_card.get_prof() == 3)
				{
					num1 = t_jinjie.clmf_num1;
				}
				GameObject _icon = icon_manager._instance.create_item_icon_ex (t_jinjie.clmf, num, num1);
				_icon.transform.parent = m_icons[3].transform;
				_icon.transform.localPosition = new Vector3 (0, 0, 0);
				_icon.transform.localScale = new Vector3 (1, 1, 1);
				if (num < num1)
				{
					m_tj = 1;
				}
				UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
				meses[0].target = this.gameObject;
				meses[0].functionName = "tp_click";
				meses[1].target = null;
				meses[1].functionName = "";
				meses[2].target = null;
				meses[2].functionName = "";
			}
		}
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_scro);
        
		int _num = 0;

		for(int i = (int)e_skill_type.skill_type_jlevel_1;i < (int)e_skill_type.skill_type_glevel_1;i ++)
		{
			role_skill _skill = m_card.m_skills[i];
			
			if(_skill != null)
			{
				int j_level = _num *3;
				if(_num == 0)
				{
					j_level = 1;
				}
				s_t_jinjie t_jinjie_ = game_data._instance.get_jinjie (j_level);
				GameObject _item = game_data._instance.ins_object_res("ui/jing_jie_item_ex");
				_item.transform.parent = m_scro.transform;
				_item.transform.name = _num.ToString();
				_item.transform.localPosition = new Vector3 (0, -83 * _num + -87,0);
				_item.transform.localScale = new Vector3(1,1,1);
				if(m_card.get_jlevel() >=  j_level)
				{
					_item.transform.Find("g").gameObject.SetActive(false);
					_item.transform.Find("lock").gameObject.SetActive(false);
				}
				else
				{
					_item.transform.Find("g").gameObject.SetActive(true);
					_item.transform.Find("lock").gameObject.SetActive(true);
				}
				_item.transform.Find("xj/xj").GetComponent<UISprite>().spriteName = t_jinjie_.icon;
                _item.transform.Find("xj/xj").GetComponent<UISprite>().MakePixelPerfect();
				_item.transform.Find("jn/jn").GetComponent<UILabel>().text = _skill.m_t_skill.name;
				GameObject _icon = icon_manager._instance.create_skill_icon(_skill);
				sys._instance.remove_child(_item.transform.Find("icon").gameObject);
				_icon.transform.parent = _item.transform.Find("icon");
				_icon.transform.localPosition = new Vector3(0,0,0);
				_icon.transform.localScale = new Vector3(1,1,1);

				_num ++;
			}
		}
		int value = 0;
		foreach(int id in game_data._instance.m_dbc_jinjie.m_index.Keys)
		{
			s_t_jinjie _t_jinjie = game_data._instance.get_jinjie(id);
			if(_t_jinjie.point != 0)
			{
				value = _t_jinjie.id;
				break;
			}
		}
		for(int i = value; i < game_data._instance.m_dbc_jinjie.get_y(); ++i)
		{
			s_t_jinjie _t_jinjie = game_data._instance.get_jinjie(i);
			GameObject obj = game_data._instance.ins_object_res("ui/jing_jie_item");
			obj.transform.parent = m_scro.transform;
			obj.transform.localPosition = new Vector3 (0, -83 * _num + -87 ,0);
			obj.transform.localScale = new Vector3(1,1,1);
			obj.transform.Find("xj").GetComponent<UISprite>().spriteName = _t_jinjie.icon;
            obj.transform.Find("xj").GetComponent<UISprite>().MakePixelPerfect();
            obj.transform.Find("xj").transform.localScale = new Vector3(0.8f, 0.8f, 1f);
			obj.transform.Find("desc").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("jinjie_gui.cs_288_80") , _t_jinjie.point );//全属性+{0}%
			if(m_card.get_jlevel() < i)
			{
				obj.transform.Find("lock").gameObject.SetActive(true);
			}
			else
			{
				obj.transform.Find("lock").gameObject.SetActive(false);
			}
			_num ++;
		}
		if(_num > 5)
		{
			m_back.transform.Find("2/down").gameObject.SetActive(true);
		}
		else
		{
			m_back.transform.Find("2/down").gameObject.SetActive(false);
		}
	}

	void tp_click(GameObject obj)
	{
		int id = obj.transform.GetComponent<item_icon>().m_item_id;
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add (id);
		cmessage_center._instance.add_message(message);
		sys._instance.m_message_type.Clear ();
		sys._instance.m_message_type.Add ("show_bu_zheng_gui");
		sys._instance.m_message_type.Add ("show_jj_gui");
		sys._instance.m_message_long.Clear ();
		sys._instance.m_message_long.Add (m_card.get_guid ());
	}

	public void click(GameObject obj)
	{
		if (obj.transform.name == "jj_ok") 
		{
			if (m_tj == 1)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_hecheng.cs_539_59"));//材料不足
				return;
			}
			else if (m_tj == 2)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
				return;
			}
			else if (m_tj == 3)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("jinjie_gui.cs_339_59"));//伙伴等级不足
				return;
			}
			else if (m_tj == 4)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("jinjie_gui.cs_344_59"));//伙伴已进阶至顶级
				return;
			}

			protocol.game.cmsg_role_jinjie _msg = new protocol.game.cmsg_role_jinjie ();
			_msg.role_guid = m_card.get_guid();
			net_http._instance.send_msg<protocol.game.cmsg_role_jinjie> (opclient_t.CMSG_ROLE_JINJIE, _msg);

		}
		else if (obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
		}
	}

	public void reset(ulong guid)
	{
		m_card = sys._instance.m_self.get_card_guid(guid);
		
		show_jj();
	}
	
	void IMessage.message (s_message message)
	{
		/*if(message.m_type == "py_show_jj")
		{
			m_card = sys._instance.m_self.get_card_guid((ulong)message.m_long[0]);
			int jlevel = m_card.get_jlevel ();
			s_t_jinjie t_jinjie = game_data._instance.get_jinjie (jlevel + 1);
			if (t_jinjie == null)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("jinjie_gui.cs_344_59"));//伙伴已进阶至顶级
				return;
			}

			show_jj();

			this.gameObject.SetActive(true);
		}*/
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ROLE_JINJIE)
		{
			int _jlevel = m_card.get_jlevel();
			s_t_jinjie t_jinjie = game_data._instance.get_jinjie (_jlevel + 1);
			sys._instance.m_self.sub_att(e_player_attr.player_gold, t_jinjie.gold,game_data._instance.get_t_language ("jinjie_gui.cs_392_73"));//角色进阶消耗

			sys._instance.m_self.remove_item((uint)t_jinjie.clty, t_jinjie.clty_num,game_data._instance.get_t_language ("jinjie_gui.cs_392_73"));//角色进阶消耗
			{
				int num1 = t_jinjie.clfy_num;
				if (m_card.get_prof() == 1)
				{
					num1 = t_jinjie.clfy_num1;
				}
                sys._instance.m_self.remove_item((uint)t_jinjie.clfy, num1, game_data._instance.get_t_language ("jinjie_gui.cs_392_73"));//角色进阶消耗
			}
			{
				int num1 = t_jinjie.clgj_num;
				if (m_card.get_prof() == 2)
				{
					num1 = t_jinjie.clgj_num1;
				}
                sys._instance.m_self.remove_item((uint)t_jinjie.clgj, num1, game_data._instance.get_t_language ("jinjie_gui.cs_392_73"));//角色进阶消耗
			}
			{
				int num1 = t_jinjie.clmf_num;
				if (m_card.get_prof() == 3)
				{
					num1 = t_jinjie.clmf_num1;
				}
                sys._instance.m_self.remove_item((uint)t_jinjie.clmf, num1, game_data._instance.get_t_language ("jinjie_gui.cs_392_73"));//角色进阶消耗
			}
			m_card.get_role().jlevel++;
			m_card.role_dress(m_card);
			sys._instance.m_self.m_t_player.jjie_task_num++;
			sys._instance.m_self.add_active(750, 1);
			sys._instance.m_self.check_target_done();
			int _num = 0;
			for(int i = (int)e_skill_type.skill_type_jlevel_1;i < (int)e_skill_type.skill_type_glevel_1;i ++)
			{
				role_skill _skill = m_card.m_skills[i];
				
				if(_skill != null)
				{
					int j_level = _num *3;
					if(_num == 0)
					{
						j_level = 1;
					}
					s_t_jinjie t_jinjie_ = game_data._instance.get_jinjie (j_level);
					
					if(m_card.get_jlevel() ==  j_level)
					{
						root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("card_dialog_box.cs_205_84") + "[00ff00]" + _skill.m_t_skill.name + "[-]" + game_data._instance.get_t_language ("jinjie_gui.cs_440_100"));//开启//技能
						break;
					}
					_num ++;
				}
			}

			this.transform.Find("frame_big").GetComponent<frame>().hide();
			//reset (m_guid, m_out_message);

			s_message _message = new s_message();
			_message.m_type = "update_bu_zheng";
			_message.m_bools.Add(true);
			cmessage_center._instance.add_message(_message);

			//root_gui._instance.do_mask(1.0f);
			s_message _message1 = new s_message();
			_message1.time = 0.5f;
			_message1.m_type = "show_jj_label";
			cmessage_center._instance.add_message(_message1);
		}
	}

	public static bool is_jinjie(ccard _card )
	{
		int jlevel = _card.get_jlevel ();
		//s_t_jinjie t_jinjie_o = game_data._instance.get_jinjie (jlevel);
		s_t_jinjie t_jinjie = game_data._instance.get_jinjie (jlevel + 1);
		if(t_jinjie == null)
		{
			return false;
		}
		if (_card.get_level() < t_jinjie.level)
		{
			return false;
		}
		if (sys._instance.m_self.m_t_player.gold < t_jinjie.gold)
		{
			return false;
		}
		int clty_num = sys._instance.m_self.get_item_num ((uint)t_jinjie.clty);
		if (clty_num < t_jinjie.clty_num)
		{
			return false;
		}
		int clfy_num = sys._instance.m_self.get_item_num ((uint)t_jinjie.clfy);
		int clfy_num1 = t_jinjie.clfy_num;
		if (_card.get_prof() == 1)
		{
			clfy_num1 = t_jinjie.clfy_num1;
		}
		if (clfy_num < clfy_num1)
		{
			return false;
		}
		int clgj_num = sys._instance.m_self.get_item_num ((uint)t_jinjie.clgj);
		int clgj_num1 = t_jinjie.clgj_num;
		if (_card.get_prof() == 2)
		{
			clgj_num1 = t_jinjie.clgj_num1;
		}

		if(clgj_num < clgj_num1)
		{
			return false;
		}
		int clmf_num = sys._instance.m_self.get_item_num ((uint)t_jinjie.clmf);
		int clmf_num1 = t_jinjie.clmf_num;
		if (_card.get_prof() == 3)
		{
			clmf_num1 = t_jinjie.clmf_num1;
		}
		if(clmf_num < clmf_num1)
		{
			return false;
		}
		return true;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
