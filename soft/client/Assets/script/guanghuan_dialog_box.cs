
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class guanghuan_dialog_box : MonoBehaviour {

	public GameObject m_name;
	public GameObject m_num;
	public GameObject m_icon;
	public GameObject m_title;
	public GameObject m_jb_att;
	public GameObject m_enhance_att;
	public List<GameObject> m_xicon = new List<GameObject>();
	public GameObject[] m_tz_name;
	public GameObject m_xname;
	public GameObject m_shengqi_desc;
	public GameObject m_effect1;
	public GameObject m_scro;
	public int item_id;
	public int type;
	// Use this for initialization
	void Start () {
	
	}

	public void OnEnable()
	{
		reset();
	}

	public void reset()
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("guanghuan_dialog_box.cs_39_41");//光环
		s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan (item_id);
		if(t_guanghuan.attr1 == 1)
		{
			m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2668_51");//生命型
		}
		else if(t_guanghuan.attr1 == 2)
		{
			m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2672_51");//攻击型
		}
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(t_guanghuan.color) + t_guanghuan.name;
		sys._instance.remove_child (m_icon);
		GameObject icon = icon_manager._instance.create_guanghuan_icon(item_id,0);
		Transform iicon = m_icon.transform;
		icon.transform.parent = iicon;
		icon.transform.localPosition = new Vector3(0,0,0);
		icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		icon.GetComponent<BoxCollider>().enabled = false;
		string text = "";
		text += game_data._instance.get_value_string (t_guanghuan.attr1, t_guanghuan.value1,1);
		if(t_guanghuan.attr2 > 0)
		{
			text += "\n" + game_data._instance.get_value_string (t_guanghuan.attr2, t_guanghuan.value2,1);
		}
		m_jb_att.GetComponent<UILabel>().text = text;
		int enhance = guanghuan_gui.guanghuan_level (t_guanghuan);
		text = "";
		text += game_data._instance.get_value_string (t_guanghuan.attr1,(float)(t_guanghuan.value1 *0.025),1);
		/*if(t_guanghuan.attr2 > 0)
		{
			text += "\n" + game_data._instance.get_value_string (t_guanghuan.attr2,(float)(t_guanghuan.value2 *0.2));
		}*/
		m_enhance_att.GetComponent<UILabel>().text = text;
		s_t_guanghuan_target t_guanghuan_target = null;
		foreach(int id in game_data._instance.m_dbc_guanghuan_target.m_index.Keys)
		{
			t_guanghuan_target = game_data._instance.get_t_guanghuan_target(id);
			if(t_guanghuan_target.ids.Contains(item_id))
			{
				break;
			}
		}
		for(int i = 0; i < m_xicon.Count ;++i)
		{
			m_xicon[i].SetActive(false);
			m_tz_name[i].SetActive(false);
		}
		if( t_guanghuan_target.ids.Count == 2)
		{
			m_xicon[0].transform.localPosition = new Vector3(-53,-68,0);
			m_tz_name[0].transform.localPosition = new Vector3(-53,-120,0);
			m_xicon[1].transform.localPosition = new Vector3(53,-68,0);
			m_tz_name[1].transform.localPosition = new Vector3(53,-120,0);
		}
		else
		{
			for(int i = 0; i < m_xicon.Count;++i)
			{
				m_xicon[i].transform.localPosition = new Vector3(-105+105*i,-68,0);
				m_tz_name[i].transform.localPosition = new Vector3(-105+105*i,-120,0);
			}
		}
		for(int i =0 ;i < m_xicon.Count && i < t_guanghuan_target.ids.Count; ++i)
		{
			m_xicon[i].SetActive(true);
			sys._instance.remove_child (m_xicon[i]);
			GameObject iicon1 = icon_manager._instance.create_guanghuan_icon(t_guanghuan_target.ids[i],0);
			iicon1.transform.parent = m_xicon[i].transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1.15f,1.15f,1.15f);
			iicon1.transform.GetComponent<BoxCollider>().enabled = false;
			s_t_guanghuan m_guanhuan = game_data._instance.get_t_guanghuan(t_guanghuan_target.ids[i]);
			m_tz_name[i].GetComponent<UILabel>().text = game_data._instance.get_name_color(m_guanhuan.color) + m_guanhuan.name;
			m_tz_name[i].SetActive(true);
		}
		m_xname.GetComponent<UILabel>().text =  t_guanghuan_target.name;
		text = "";
		for(int i = 0; i < t_guanghuan_target.attrs.Count;++i)
		{
			if(t_guanghuan_target.attrs.Count -1 == i)
			{
				text += game_data._instance.get_value_string(t_guanghuan_target.attrs[i].attr,t_guanghuan_target.attrs[i].value,1);
			}
			else
			{
				text += game_data._instance.get_value_string(t_guanghuan_target.attrs[i].attr,t_guanghuan_target.attrs[i].value,1) + "\n";
			}
		}
		m_effect1.GetComponent<UILabel>().text ="[FFFF00]"+ text;
		List <s_t_guanghuan_skill> t_guanghuan_skills = new List<s_t_guanghuan_skill>();
		foreach(int _id in game_data._instance.m_dbc_guanghuan_skill.m_index.Keys)
		{
			s_t_guanghuan_skill t_guanghuan_skill = game_data._instance.get_t_guanghuan_skill(_id);
			if(t_guanghuan_skill.wing_id == item_id)
			{
				t_guanghuan_skills.Add(t_guanghuan_skill);
			}
		}
		string zh = "";
		for(int i = 0; i < t_guanghuan_skills.Count;++i)
		{
			zh += get_skill_des(t_guanghuan_skills[i],zh == "");
		}
		if(zh == "")
		{
			zh = game_data._instance.get_t_language ("guanghuan_dialog_box.cs_144_8");//[FFFF00]无技能
		}
		m_shengqi_desc.GetComponent<UILabel>().text = zh;
	}

	public string get_skill_des(s_t_guanghuan_skill t_guanghuan_skill,bool first)
	{
		string text = "";
		string desc = "";
		if(t_guanghuan_skill.type == 1)
		{
			desc = game_data._instance.get_value_string(t_guanghuan_skill.def1,t_guanghuan_skill.def2,1);
			string[] s = desc.Split('+');
			desc = "[0aabff]" + s[0] + "[-]" + "[0aff16]+" +s[1] + "[-]";
		}
		if(t_guanghuan_skill.type == 2 || t_guanghuan_skill.type == 3)
		{
			desc = t_guanghuan_skill.desc.Replace("%","");
			desc = "[0aabff]" + desc.Replace("{{n1}}", "[-][0aff16]"+ t_guanghuan_skill.def1.ToString() + "%[-][0aabff]");
		}
		if(!first)
		{
			text += "\n";
		}
		text += "[-][" + t_guanghuan_skill.name + "][-] "+ desc + " [ffde00](" + string.Format(game_data._instance.get_t_language ("guanghuan_dialog_box.cs_168_89"), t_guanghuan_skill.enhance);//强化至[-][0aff16]{0}[-][ffde00]级开启)[-]
		return text;
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "ok")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}
}
