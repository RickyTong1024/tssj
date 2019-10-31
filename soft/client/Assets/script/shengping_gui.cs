
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class shengping_gui : MonoBehaviour,IMessage {

	public ccard m_card;
	public GameObject m_name;
	public GameObject m_sx;
	public GameObject m_cz;
	public GameObject m_jn;
	public GameObject m_next_sx;
	public GameObject m_next_cz;
	public GameObject m_next_jn;
	public GameObject m_next_name;
	public GameObject m_tishi;
	public GameObject m_level;
	public GameObject m_gold;
	public GameObject m_text;
	public GameObject m_text1;
	public GameObject m_jinjie;
	public List<GameObject> m_icons = new List<GameObject>();
	public GameObject m_pingji;
	public GameObject m_next_pingji;
	public GameObject m_last_pingji;
	public GameObject m_next;
	public GameObject m_last;
	public GameObject m_back;
	public GameObject m_back1;
	public GameObject m_cl_panel;
	public GameObject m_retrun;
	public GameObject m_scro;
	public GameObject m_view;
	public GameObject m_juexing;
	public GameObject m_show_Label;

	private int pingzhi = 0;
	uint shenping_stone = 50130001;
	uint role_red_power = 50110001;
	uint card_sp_id = 0;

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
		card_sp_id = (uint)m_card.get_fragment_id ();
		m_show_Label.SetActive (false);
		pingzhi = m_card.get_pinzhi ();
		reset (m_card.get_role().pinzhi);
	}

	public void reset(int pinzhi)
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		string text = "";
		List<role_skill> m_skills = new List<role_skill>();
		s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin (pinzhi);
		List<double> attrs = ccard.get_role_org_attr (m_card.get_template_id(), m_card.get_level(), m_card.get_jlevel(), m_card.get_glevel(), pinzhi);
		m_sx.transform.Find("hp").GetComponent<UILabel>().text =  ((int)attrs[1]).ToString();
		m_sx.transform.Find("attack").GetComponent<UILabel>().text =  ((int)attrs[2]).ToString();
		m_sx.transform.Find("wf").GetComponent<UILabel>().text =  ((int)attrs[3]).ToString();
		m_sx.transform.Find("mf").GetComponent<UILabel>().text =  ((int)attrs[4]).ToString();
		m_sx.transform.Find("speed").GetComponent<UILabel>().text =  ((int)attrs[5]).ToString();
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color (t_role_shengpin.color) + t_role_shengpin.name;
		m_pingji.GetComponent<UILabel>().text = game_data._instance.get_name_color (t_role_shengpin.color) + t_role_shengpin.name;
		s_t_role_shengpin t_role_shengpin_next = null;
		t_role_shengpin_next = game_data._instance.get_t_role_shengpin(t_role_shengpin.next_pinzhi);
		int _id = -1;
		if(t_role_shengpin_next != null)
		{
			m_next.SetActive(true);
			m_next_pingji.GetComponent<UILabel>().text =game_data._instance.get_name_color (t_role_shengpin_next.color) + t_role_shengpin_next.name;
			m_tishi.SetActive(false);
			m_jinjie.SetActive(true);
			m_level.GetComponent<UILabel>().text = "[0aff16]"+ t_role_shengpin_next.level;
			if(m_card.get_level() < t_role_shengpin_next.level)
			{
				m_level.GetComponent<UILabel>().text = "[ff0000]"+ t_role_shengpin_next.level;
			}
			m_level.transform.parent.gameObject.SetActive(true);
			m_gold.GetComponent<UILabel>().text = sys._instance.get_res_color(1) + t_role_shengpin_next.gold;
			if(sys._instance.m_self.m_t_player.gold < t_role_shengpin_next.gold)
			{
				m_gold.GetComponent<UILabel>().text = "[ff0000]"+ t_role_shengpin_next.gold;
			}
			m_gold.transform.parent.gameObject.SetActive(true);
			for(int i = 0;i < m_icons.Count;++i)
			{
				m_icons[i].SetActive(true);
				sys._instance.remove_child(m_icons[i]);
			}
			int num = sys._instance.m_self.get_item_num(shenping_stone);
			if(t_role_shengpin_next.shengpinshi > 0)
			{
				_id++;
				create_icon((int)shenping_stone,num,t_role_shengpin_next.shengpinshi,_id);
			}
			num = sys._instance.m_self.m_t_player.jjc_point;
			if(t_role_shengpin_next.zhanhun > 0)
			{
				_id++;
				GameObject _icon = icon_manager._instance.create_resource_icon (5,num,t_role_shengpin_next.zhanhun);
				_icon.transform.parent = m_icons[_id].transform;
				_icon.transform.localPosition = new Vector3(0,0,0);
				_icon.transform.localScale = new Vector3(1,1,1);
			}
			num = sys._instance.m_self.get_item_num(role_red_power);
			if(t_role_shengpin_next.hongsehuobanzhili > 0)
			{
				_id++;
				create_icon((int)role_red_power,num,t_role_shengpin_next.hongsehuobanzhili,_id);
			}
			num = sys._instance.m_self.get_item_num(card_sp_id);
			if(t_role_shengpin_next.suipian > 0)
			{
				_id++;
				create_icon((int)card_sp_id,num,t_role_shengpin_next.suipian,_id);
			}
		}
		else
		{	
			m_next.SetActive(false);
			for(int i = 0;i < m_icons.Count;++i)
			{
				m_icons[i].SetActive(false);
			}
			m_tishi.SetActive(true);
			m_jinjie.SetActive(false);
			m_level.transform.parent.gameObject.SetActive(false);
			m_gold.transform.parent.gameObject.SetActive(false);
		}
		for(int i = _id+1;i < m_icons.Count;++i)
		{
			m_icons[i].SetActive(false);
		}
		if(t_role_shengpin_next == null)
		{
			t_role_shengpin_next = game_data._instance.get_t_role_shengpin (t_role_shengpin.pinzhi);
		}
		double value = 0;
		if(t_role_shengpin_next.cz[0] > t_role_shengpin.cz[0])
		{
			m_cz.SetActive(true);
			value = m_card.m_t_class.cz [0] * t_role_shengpin.cz [0] +m_card.m_t_class.czcz [0] * t_role_shengpin.cz [0] *m_card .get_role().glevel;
			m_cz.transform.Find("hp").GetComponent<UILabel>().text = value.ToString("f2");
			value = m_card.m_t_class.cz [1] * t_role_shengpin.cz [1] +m_card.m_t_class.czcz [1] * t_role_shengpin.cz [1] *m_card .get_role().glevel;
			m_cz.transform.Find("attack").GetComponent<UILabel>().text =  value.ToString("f2");
			value = m_card.m_t_class.cz [2] * t_role_shengpin.cz [2] +m_card.m_t_class.czcz [2] * t_role_shengpin.cz [2] *m_card .get_role().glevel;
			m_cz.transform.Find("wf").GetComponent<UILabel>().text = value.ToString("f2");
			value = m_card.m_t_class.cz [3] * t_role_shengpin.cz [3] +m_card.m_t_class.czcz [3] * t_role_shengpin.cz [3] *m_card .get_role().glevel;
			m_cz.transform.Find("mf").GetComponent<UILabel>().text =  value.ToString("f2");
			value = m_card.m_t_class.cz [4] * t_role_shengpin.cz [4] +m_card.m_t_class.czcz [4] * t_role_shengpin.cz [4] *m_card .get_role().glevel;
			m_cz.transform.Find("speed").GetComponent<UILabel>().text =  value.ToString("f2");
		}
		else
		{
			m_cz.SetActive(false);
		}
		attrs = ccard.get_role_org_attr (m_card.get_template_id(), m_card.get_level(), m_card.get_jlevel(), m_card.get_glevel(), t_role_shengpin_next.pinzhi);
		m_next_sx.transform.Find("hp").GetComponent<UILabel>().text = "[0aabff]"  + ((int)attrs[1]).ToString();
		m_next_sx.transform.Find("hp_up").gameObject.SetActive(false);
		if(t_role_shengpin_next.cs[0] > t_role_shengpin.cs[0])
		{
			m_next_sx.transform.Find("hp").GetComponent<UILabel>().text = "[0aff16]"  + ((int)attrs[1]).ToString();
			m_next_sx.transform.Find("hp_up").gameObject.SetActive(true);
		}
		m_next_sx.transform.Find("attack").GetComponent<UILabel>().text = "[0aabff]" + ((int)attrs[2]).ToString();
		m_next_sx.transform.Find("attack_up").gameObject.SetActive(false);
		if(t_role_shengpin_next.cs[1] > t_role_shengpin.cs[1])
		{
			m_next_sx.transform.Find("attack").GetComponent<UILabel>().text = "[0aff16]"  + ((int)attrs[2]).ToString();
			m_next_sx.transform.Find("attack_up").gameObject.SetActive(true);
		}
		m_next_sx.transform.Find("wf").GetComponent<UILabel>().text =  "[0aabff]" + ((int)attrs[3]).ToString();
		m_next_sx.transform.Find("wf_up").gameObject.SetActive(false);
		if(t_role_shengpin_next.cs[2] > t_role_shengpin.cs[2])
		{
			m_next_sx.transform.Find("wf").GetComponent<UILabel>().text = "[0aff16]"  + ((int)attrs[3]).ToString();
			m_next_sx.transform.Find("wf_up").gameObject.SetActive(true);
		}
		m_next_sx.transform.Find("mf").GetComponent<UILabel>().text =  "[0aabff]" + ((int)attrs[4]).ToString();
		m_next_sx.transform.Find("mf_up").gameObject.SetActive(false);
		if(t_role_shengpin_next.cs[3] > t_role_shengpin.cs[3])
		{
			m_next_sx.transform.Find("mf").GetComponent<UILabel>().text = "[0aff16]"  + ((int)attrs[4]).ToString();
			m_next_sx.transform.Find("mf_up").gameObject.SetActive(true);
		}
		m_next_sx.transform.Find("speed").GetComponent<UILabel>().text = "[0aabff]" + ((int)attrs[5]).ToString();
		m_next_sx.transform.Find("speed_up").gameObject.SetActive(false);
		if(t_role_shengpin_next.cs[4] > t_role_shengpin.cs[4])
		{
			m_next_sx.transform.Find("speed").GetComponent<UILabel>().text = "[0aff16]"  + ((int)attrs[5]).ToString();
			m_next_sx.transform.Find("speed_up").gameObject.SetActive(true);
		}
		if(t_role_shengpin_next.cz[0] > t_role_shengpin.cz[0])
		{
			m_next_cz.SetActive(true);
			value = m_card.m_t_class.cz [0] * t_role_shengpin_next.cz [0] +m_card.m_t_class.czcz [0] * t_role_shengpin_next.cz [0] *m_card .get_role().glevel;
			m_next_cz.transform.Find("hp").GetComponent<UILabel>().text = value.ToString("f2");
			value = m_card.m_t_class.cz [1] * t_role_shengpin_next.cz [1] +m_card.m_t_class.czcz [1] * t_role_shengpin_next.cz [1] *m_card .get_role().glevel;
			m_next_cz.transform.Find("attack").GetComponent<UILabel>().text = value.ToString("f2");
			value = m_card.m_t_class.cz [2] * t_role_shengpin_next.cz [2] +m_card.m_t_class.czcz [2] * t_role_shengpin_next.cz [2] *m_card .get_role().glevel;
			m_next_cz.transform.Find("wf").GetComponent<UILabel>().text = value.ToString("f2");
			value = m_card.m_t_class.cz [3] * t_role_shengpin_next.cz [3] +m_card.m_t_class.czcz [3] * t_role_shengpin_next.cz [3] *m_card .get_role().glevel;
			m_next_cz.transform.Find("mf").GetComponent<UILabel>().text = value.ToString ("f2");
			value = m_card.m_t_class.cz [4] * t_role_shengpin_next.cz [4] +m_card.m_t_class.czcz [4] * t_role_shengpin_next.cz [4] *m_card .get_role().glevel;
			m_next_cz.transform.Find("speed").GetComponent<UILabel>().text = value.ToString("f2");
		}
		else
		{
			m_next_cz.SetActive(false);
		}
		if(t_role_shengpin_next.bdjnjc > t_role_shengpin.bdjnjc)
		{
			for(int i = (int)e_skill_type.skill_type_glevel_1;i < (int)e_skill_type.skill_end;i ++)
			{
				if(m_card.get_glevel() <= (i - (int)e_skill_type.skill_type_glevel_1))
				{
					continue;
				}
				role_skill _skill = m_card.m_skills[i];
				
				if(_skill != null)
				{
					m_skills.Add(_skill);
				}
			}
		}
		role_skill skill = m_card.m_skills[(int)e_skill_type.skill_type_active];
		s_t_skill t_skill = game_data._instance.get_t_skill (skill._t_skill.id + t_role_shengpin.zdjnjc);
		if(t_role_shengpin.zdjnjc  < t_role_shengpin_next.zdjnjc)
		{
			text += "[FFFF00]" + t_skill.name + "[-]" + "\n";
			if(m_skills.Count > 0)
			{
				text += skill_des(t_skill,skill.level())+"\n\n";
			}
			else
			{
				text +=  skill_des(t_skill,skill.level());
			}
		}
		for(int i = 0; i < m_skills.Count;++i)
		{
			text +=  "[FFFF00]" + m_skills[i].m_t_skill.name  + "[-]" +"\n";
			if(i != m_skills.Count -1)
			{
				text += m_skills[i].get_bd_des(pinzhi,1) +"\n\n";
			}
			else
			{
				text += m_skills[i].get_bd_des(pinzhi,1);
			}
		}
		m_text.GetComponent<UILabel>().text = text;
		m_next_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(t_role_shengpin_next.color) + t_role_shengpin_next.name;
		if(text == "")
		{
			m_jn.SetActive(false);
			m_jn.transform.localPosition = new Vector3(258,20,0);
		}
		else
		{
			m_jn.transform.localPosition = new Vector3(258,-69,0);
			m_jn.SetActive(true);
		}
		text = "";
		m_skills.Clear ();
		if(t_role_shengpin_next.bdjnjc > t_role_shengpin.bdjnjc)
		{
			for(int i = (int)e_skill_type.skill_type_glevel_1; i < (int)e_skill_type.skill_end;i ++)
			{
				if(m_card.get_glevel() <= (i - (int)e_skill_type.skill_type_glevel_1))
				{
					continue;
				}
				role_skill _skill = m_card.m_skills[i];
				
				if(_skill != null)
				{
					m_skills.Add(_skill);
				}
			}
		}
		skill = m_card.m_skills[(int)e_skill_type.skill_type_active];
		if(t_role_shengpin.zdjnjc  < t_role_shengpin_next.zdjnjc)
		{
			t_skill = game_data._instance.get_t_skill (skill._t_skill.id + t_role_shengpin_next.zdjnjc);
			text += "[FFFF00]" + t_skill.name  + "[-]" + "\n";
			if(m_skills.Count > 0)
			{
				text += skill_des(t_skill,skill.level(),1)+"\n\n";
			}
			else
			{
				text +=  skill_des(t_skill,skill.level(),1);
			}
		}
		for(int i = 0; i < m_skills.Count;++i)
		{
			text += "[FFFF00]" + m_skills[i].m_t_skill.name + "[-]" +"\n";
			if(i != m_skills.Count -1)
			{
				text += m_skills[i].get_bd_des(t_role_shengpin_next.pinzhi) +"\n\n";
			}
			else
			{
				text += m_skills[i].get_bd_des(t_role_shengpin_next.pinzhi);
			}
		}
		if(text == "")
		{
			m_next_jn.SetActive(false);
			m_next_jn.transform.localPosition = new Vector3(-258,20,0);
		}
		else
		{
			m_next_jn.transform.localPosition = new Vector3(-258,-69,0);
			m_next_jn.SetActive(true);
		}
		m_text1.GetComponent<UILabel>().text = text;
		s_t_role_shengpin t_role_shengpin_last = null;
		for(int i = game_data._instance.m_dbc_role_shengpin.get_y() -1; i > 0;i--)
		{
			int id = int.Parse(game_data._instance.m_dbc_role_shengpin.get(0,i));
			if(id < pinzhi && id >= m_card.get_role().pinzhi)
			{
				t_role_shengpin_last = game_data._instance.get_t_role_shengpin(id);
				break;
			}
		}
		if(t_role_shengpin_last == null)
		{
			m_last.SetActive(false);
		}
		else
		{
			m_last.SetActive(true);
			m_last_pingji.GetComponent<UILabel>().text = game_data._instance.get_name_color(t_role_shengpin_last.color) + t_role_shengpin_last.name;
		}
		m_back.transform.localPosition = new Vector3 (0,28,0);
		m_back1.transform.localPosition = new Vector3 (0,28,0);
		sys._instance.add_pos_anim(m_back,0.3f, new Vector3(-225, 0, 0), 0.05f);
		sys._instance.add_alpha_anim(m_back,0.3f, 0, 1.0f, 0.05f);
		sys._instance.add_pos_anim(m_back1,0.3f, new Vector3(225, 0, 0), 0.05f);
		sys._instance.add_alpha_anim(m_back1,0.3f, 0, 1.0f, 0.05f);
		if(pinzhi != m_card.get_role().pinzhi)
		{
			m_retrun.SetActive(true);
			m_cl_panel.SetActive(false);
		}
		else
		{
			m_retrun.SetActive(false);
			m_cl_panel.SetActive(true);
			m_cl_panel.transform.localPosition = new Vector3 (0,68,0);
			sys._instance.add_pos_anim(m_cl_panel,0.3f, new Vector3(0, -62, 0), 0.05f);
			sys._instance.add_alpha_anim(m_cl_panel,0.3f, 0, 1.0f, 0.05f);
		}
		chang_color ();
	}

	public void chang_color()
	{
		s_t_role_shengpin _t_role_shengpin = game_data._instance.get_t_role_shengpin (m_card.get_role ().pinzhi);
		m_juexing.GetComponent<UILabel>().text = "";
		for(int i = 0 ; i < game_data._instance.m_dbc_role_shengpin.get_y();i++)
		{
			int id = int.Parse(game_data._instance.m_dbc_role_shengpin.get(0,i));
			s_t_role_shengpin t_role_shengping = game_data._instance.get_t_role_shengpin(id);
			if(id > m_card.get_role().pinzhi && t_role_shengping.color > _t_role_shengpin.color)
			{
				m_juexing.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("shengping_gui.cs_392_59"),t_role_shengping.name,color(t_role_shengping.color));//觉醒到{0}可升至{1}
				break;
			}
			if(id > m_card.get_role().pinzhi && t_role_shengping.zdjnjc > _t_role_shengpin.zdjnjc)
			{
				m_juexing.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("shengping_gui.cs_397_59"),t_role_shengping.name);//觉醒到{0}可提升主动技能
				break;
			}
		}
	}

	string color(int type)
	{
		string s = "";
		if(type == 4)
		{
			return game_data._instance.get_t_language ("shengping_gui.cs_408_10");//橙色
		}
		if(type == 5)
		{
			return game_data._instance.get_t_language ("shengping_gui.cs_412_10");//红色
		}
		if(type == 6)
		{
			return game_data._instance.get_t_language ("shengping_gui.cs_416_10");//金色
		}
		return s;
	}

	public string skill_des(s_t_skill t_skill,int level, int type = 0)
	{
		string color = "[0aabff]";
		string s ="[0aabff]" +  t_skill.des;
		if(type == 1)
		{
			color = "[0aff16]";
		}
		float value = t_skill.attack_pe + t_skill.attack_pe_add * level;
		s = s.Replace("{{n1}}", "[-]" + color + (value * 100).ToString() + "[-][0aabff]");
		value = t_skill.buffer_attack_pes [0] + t_skill.buffer_attack_pe_adds [0] * level;
		s = s.Replace("{{n2}}", "[-]"+color + (value * 100).ToString() + "[-][0aabff]");
		value = t_skill.buffer_modify_att_vals [0] + t_skill.buffer_modify_att_val_adds [0] * level;
		s = s.Replace("{{n3}}", "[-]" + color + (Mathf.Abs(value)).ToString() + "[-][0aabff]");
		value = t_skill.buffer_attack_pes[1] + t_skill.buffer_attack_pe_adds[1] * level;
		s = s.Replace("{{n7}}", "[-]" + color + (value * 100).ToString() + "[-][0aabff]");
		value = t_skill.buffer_modify_att_vals [1] + t_skill.buffer_modify_att_val_adds [1] * level;
		s = s.Replace("{{n8}}", "[-]" + color + (Mathf.Abs(value)).ToString() + "[-][0aabff]");
		value = t_skill.base_ex_type_val_0;
		s = s.Replace("{{n4}}", "[-]" + color + (Mathf.Abs(value)).ToString() + "[-][0aabff]");
		value = t_skill.base_ex_type_val_1 + t_skill.add_ex_type_val_1 * level;
		s = s.Replace("{{n5}}", "[-]" + color + (Mathf.Abs(value)).ToString() + "[-][0aabff]");
		value = t_skill.base_ex_type_val_2 + t_skill.add_ex_type_val_2 * level;	
		s = s.Replace("{{n6}}", "[-]" + color + (Mathf.Abs(value)).ToString() + "[-][0aabff]");
		return s;
	}

	public void click(GameObject obj)
	{
		List<int> m_pingzhis = new List<int>();
		for(int i = 0 ; i < game_data._instance.m_dbc_role_shengpin.get_y();i++)
		{
			int id = int.Parse(game_data._instance.m_dbc_role_shengpin.get(0,i));
			if(id >= m_card.get_role().pinzhi)
			{
				m_pingzhis.Add(id);
			}
		}

		if(obj.transform.name == "next")
		{
			s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(pingzhi);
			if(t_role_shengpin.next_pinzhi == 0)
			{
				pingzhi = t_role_shengpin.pinzhi;
			}
			else
			{
				pingzhi = t_role_shengpin.next_pinzhi;
			}
			reset(pingzhi);
		}
		if(obj.transform.name == "last")
		{
			for(int i = m_pingzhis.Count -1;i >= 0;i--)
			{
				if(m_pingzhis[i] < pingzhi)
				{
					pingzhi = m_pingzhis[i];
					break;
				}
			}
			reset(pingzhi);
		}
		if(obj.transform.name == "back")
		{
			pingzhi = m_card.get_role().pinzhi;
			reset(pingzhi);
		}
		if(obj.transform.name == "jj")
		{
			s_t_role_shengpin t_role_shengpin_next = null;
			s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(m_card.get_role().pinzhi);
			t_role_shengpin_next = game_data._instance.get_t_role_shengpin(t_role_shengpin.next_pinzhi);
			if(t_role_shengpin_next == null)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("shengping_gui.cs_497_59"));//已升至最高品级
				return;
			}
			if(t_role_shengpin_next.level > m_card.get_level())
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("jinjie_gui.cs_339_59"));//伙伴等级不足
				return;
			}
			int num = sys._instance.m_self.get_item_num(shenping_stone);
			if(t_role_shengpin_next.shengpinshi > num &&  t_role_shengpin_next.shengpinshi >0)
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)shenping_stone);
				cmessage_center._instance.add_message(message);
				sys._instance.m_message_type.Add ("show_sheng_pin_gui");
				sys._instance.m_message_long.Add (m_card.get_guid ());
				return;
			}
			num = sys._instance.m_self.get_item_num(role_red_power);
			if(t_role_shengpin_next.hongsehuobanzhili > num && t_role_shengpin_next.hongsehuobanzhili >0)
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)role_red_power);
				cmessage_center._instance.add_message(message);
				sys._instance.m_message_type.Add ("show_sheng_pin_gui");
				sys._instance.m_message_long.Add (m_card.get_guid ());
				return;
			}
			num = sys._instance.m_self.get_item_num(card_sp_id);
			if(t_role_shengpin_next.suipian > num && t_role_shengpin_next.suipian > 0)
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)card_sp_id);
				cmessage_center._instance.add_message(message);
				sys._instance.m_message_type.Add ("show_sheng_pin_gui");
				sys._instance.m_message_long.Add (m_card.get_guid ());
				return;
			}
			num = sys._instance.m_self.m_t_player.jjc_point;
			if(t_role_shengpin_next.zhanhun > num &&  t_role_shengpin_next.zhanhun > 0)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("item_shop_card.cs_84_24"));//战魂不足
				return;
			}
			if(t_role_shengpin_next.gold > sys._instance.m_self.m_t_player.gold && t_role_shengpin_next.gold >0)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
				return;
			}
			protocol.game.cmsg_role_shengpin _msg = new protocol.game.cmsg_role_shengpin ();
			_msg.role_guid = m_card.get_guid();
			net_http._instance.send_msg<protocol.game.cmsg_role_shengpin> (opclient_t.CMSG_ROLE_SHENGPIN, _msg);
		}
	}

	public void click_item_icon(GameObject obj)
	{
		int id = obj.transform.GetComponent<item_icon>().m_item_id;
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add (id);
		cmessage_center._instance.add_message(message);
		cmessage_center._instance.add_message(message);
		sys._instance.message_clear ();
		sys._instance.m_message_type.Add ("show_sheng_pin_gui");
		sys._instance.m_message_long.Add (m_card.get_guid ());
	}

	void create_icon(int id,int num,int max,int i)
	{
		GameObject _icon = icon_manager._instance.create_item_icon_ex(id,num,max);
		_icon.transform.name = id.ToString();
		_icon.transform.parent = m_icons[i].transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
		meses[0].target = this.gameObject;
		meses[0].functionName = "click_item_icon";
		meses[1].target = null;
		meses[1].functionName = "";
		meses[2].target = null;
		meses[2].functionName = "";
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ROLE_SHENGPIN)
		{
			s_t_role_shengpin t_role_shengpin_next = null;
			s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(m_card.get_role().pinzhi);
			t_role_shengpin_next = game_data._instance.get_t_role_shengpin(t_role_shengpin.next_pinzhi);
			if(t_role_shengpin_next.shengpinshi > 0)
			{
				sys._instance.m_self.remove_item((uint)shenping_stone,t_role_shengpin_next.shengpinshi,game_data._instance.get_t_language ("shengping_gui.cs_593_91"));//角色升品消耗
			}
			if(t_role_shengpin_next.zhanhun > 0)
			{
				sys._instance.m_self.sub_att(e_player_attr.player_jjc_point,t_role_shengpin_next.zhanhun);
			}
			if(t_role_shengpin_next.hongsehuobanzhili > 0)
			{
                sys._instance.m_self.remove_item((uint)role_red_power, t_role_shengpin_next.hongsehuobanzhili, game_data._instance.get_t_language ("shengping_gui.cs_593_91"));//角色升品消耗
			}
			if(t_role_shengpin_next.suipian > 0)
			{
                sys._instance.m_self.remove_item((uint)card_sp_id, t_role_shengpin_next.suipian, game_data._instance.get_t_language ("shengping_gui.cs_593_91"));//角色升品消耗
			}
			if(t_role_shengpin_next.gold > 0)
			{
				sys._instance.m_self.sub_att(e_player_attr.player_gold,t_role_shengpin_next.gold,game_data._instance.get_t_language ("shengping_gui.cs_593_91"));//角色升品消耗
			}
			m_card.get_role().pinzhi = t_role_shengpin_next.pinzhi;
			pingzhi = t_role_shengpin_next.pinzhi;
			reset(pingzhi);
			m_show_Label.SetActive(true);
			Label_time();
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
	}

	public static bool is_shengping(ccard m_card)
	{
		if(m_card.m_t_class.color < 4)
		{
			return false;
		}
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shengpin)
		{
			return false;
		}
		s_t_role_shengpin t_role_shengpin_next = null;
		s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(m_card.get_role().pinzhi);
		t_role_shengpin_next = game_data._instance.get_t_role_shengpin(t_role_shengpin.next_pinzhi);
		if(t_role_shengpin_next == null)
		{
			return false;
		}
		if(t_role_shengpin_next.level > m_card.get_level())
		{
			return false;
		}
		int num = sys._instance.m_self.get_item_num((uint)50130001);
		if(t_role_shengpin_next.shengpinshi > num &&  t_role_shengpin_next.shengpinshi >0)
		{
			return false;
		}
		num = sys._instance.m_self.get_item_num((uint)50110001);
		if(t_role_shengpin_next.hongsehuobanzhili > num && t_role_shengpin_next.hongsehuobanzhili >0)
		{
			return false;
		}
		num = sys._instance.m_self.get_item_num((uint)m_card.get_fragment_id());
		if(t_role_shengpin_next.suipian > num && t_role_shengpin_next.suipian > 0)
		{
			return false;
		}
		num = sys._instance.m_self.m_t_player.jjc_point;
		if(t_role_shengpin_next.zhanhun > num &&  t_role_shengpin_next.zhanhun > 0)
		{
			return false;
		}
		if(t_role_shengpin_next.gold > sys._instance.m_self.m_t_player.gold && t_role_shengpin_next.gold >0)
		{
			return false;
		}
		return true;
	}

	void Label_time()
	{
		hide_time _hide = m_show_Label.GetComponent<hide_time>();
		
		if(_hide == null)
		{
			_hide = m_show_Label.AddComponent<hide_time>();
		}
		
		_hide.m_time = 0.3f;
	}

	void IMessage.message(s_message message)
	{

	}

	// Update is called once per frame
	void Update () {
	
	}
}
