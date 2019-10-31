
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_dialog_box : MonoBehaviour {

	public GameObject m_name;
	public GameObject m_des;
	public GameObject m_num;
	public GameObject m_icon;
	public GameObject m_equip_panel;
	public GameObject m_sp_panel;
	public GameObject m_title;
	public GameObject m_jb_att;
	public GameObject m_enhance_att;
	public List<GameObject> m_jl_atts = new List<GameObject>();
	public List<GameObject> m_xicon = new List<GameObject>();
	public GameObject[] m_tz_name;
	public GameObject m_xname;
	public GameObject m_tz_sprite;
	public GameObject m_equip_skill_sprite;
	public dhc.equip_t m_equip = null;
	public GameObject m_shengqi_desc;
	public GameObject m_effect1;
	public GameObject m_effect2;
	public GameObject m_effect3;
	public GameObject m_scro;
	public GameObject m_sp_toggle;
	public GameObject m_equip_toggle;
	public int item_id;
	public int type;
	// Use this for initialization
	void Start () {
	
	}
	
	public void OnEnable()
	{
		reset(type);
	}

	public void reset(int select)
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		if(select == 0)
		{
			m_equip_toggle.SetActive(false);
			m_sp_toggle.SetActive(true);
			int sp_id = item_id;
			s_t_equip t_equip = game_data._instance.get_t_equip (item_id);
			if(t_equip != null)
			{
				for(int i =0; i < game_data._instance.m_dbc_item.get_y();++i)
				{
					int id = int.Parse(game_data._instance.m_dbc_item.get(0,i));
					s_t_item _t_item = game_data._instance.get_item(id);
					if(_t_item.type == 7001 && _t_item.def_1 == t_equip.id)
					{
						sp_id = id;
						break;
					}
				}
			}
			m_equip_panel.SetActive(false);
			m_sp_panel.SetActive(true);
			m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_dialog_box.cs_71_42");//碎片
			s_t_item t_item = game_data._instance.get_item (sp_id);
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2,sp_id,0,0);
			m_des.GetComponent<UILabel>().text = "[fff300]" + t_item.desc;
			int num = sys._instance.m_self.get_item_num ((uint)sp_id);
			m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + num.ToString();//当前拥有：
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_item_icon(sp_id);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
		}
		else if(select == 1)
		{
			m_equip_toggle.SetActive(true);
			m_sp_toggle.SetActive(false);
			int equip_id = item_id;
			m_equip_panel.SetActive(true);
			m_sp_panel.SetActive(false);
			m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2956_35");//装备
			s_t_item t_item = game_data._instance.get_item(item_id);
			if(t_item != null)
			{
				equip_id = t_item.def_1;
			}
			s_t_equip t_equip = game_data._instance.get_t_equip (equip_id);
			bool flag = false;
			if(t_equip != null)
			{
				for(int i =0; i < game_data._instance.m_dbc_item.get_y();++i)
				{
					int id = int.Parse(game_data._instance.m_dbc_item.get(0,i));
					s_t_item _t_item = game_data._instance.get_item(id);
					if(_t_item.type == 7001 && _t_item.def_1 == t_equip.id)
					{
						flag = true;
						break;
					}
				}
			}
			if(flag)
			{
				m_sp_toggle.transform.parent.gameObject.SetActive(true);
			}
			else
			{
				m_sp_toggle.transform.parent.gameObject.SetActive(false);
			}
			m_num.GetComponent<UILabel>().text = equip.get_equip_part_e (equip_id);
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(4,equip_id,0,0);
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_equip_icon(equip_id);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
			m_jb_att.GetComponent<UILabel>().text = game_data._instance.get_value_string(t_equip.eattr.attr,t_equip.eattr.value);
			s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx (t_equip.font_color, 0);
			m_enhance_att.GetComponent<UILabel>().text =game_data._instance.get_value_string (t_equip.eattr.attr, ((int)(t_equip.eattr.value * t_equip_sx.enhance_rate * 1)));
			for(int i = 0; i < m_jl_atts.Count;++i)
			{
				int num = t_equip.ejlattr[i].value * 1;
				m_jl_atts[i].GetComponent<UILabel>().text = game_data._instance.get_value_string( t_equip.ejlattr[i].attr,num);
			}
			s_t_equip_tz t_equip_tz = game_data._instance.get_t_equip_tz (equip_id);
			for(int i =0 ;i < m_xicon.Count; ++i)
			{
				sys._instance.remove_child (m_xicon[i]);
				GameObject iicon1 = icon_manager._instance.create_equip_icon(t_equip_tz.equip_ids[i]);
				iicon1.transform.parent = m_xicon[i].transform;
				iicon1.transform.localPosition = new Vector3(0,0,0);
				iicon1.transform.localScale = new Vector3(1.15f,1.15f,1.15f);
				iicon1.transform.GetComponent<BoxCollider>().enabled = false;
				m_tz_name[i].GetComponent<UILabel>().text = equip.get_equip_real_name(t_equip_tz.equip_ids[i]);
			}
			m_xname.GetComponent<UILabel>().text = equip.get_equip_color(equip_id) + t_equip_tz.name;
			if(t_equip.font_color == 5)
			{
				m_shengqi_desc.GetComponent<UILabel>().text = equip.equip_skill(t_equip.type,0,1);
				m_equip_skill_sprite.SetActive(true);
				m_equip_skill_sprite.transform.localPosition = new Vector3(0,-227,0);
				m_tz_sprite.transform.localPosition = new Vector3(0,-307 - m_shengqi_desc.GetComponent<UILabel>().height,0);
			}
			else
			{
				m_equip_skill_sprite.SetActive(false);
				m_tz_sprite.transform.localPosition = new Vector3(0,-227,0);
			}
			m_effect1.GetComponent<UILabel>().text ="[FFFF00]"+game_data._instance.get_t_language ("equip_detail.cs_169_55") + "：" + game_data._instance.get_value_string(t_equip_tz.attr1,t_equip_tz.value1);//两件效果
			m_effect2.GetComponent<UILabel>().text ="[FFFF00]" +game_data._instance.get_t_language ("equip_detail.cs_170_56") + "："+ game_data._instance.get_value_string(t_equip_tz.attr2,t_equip_tz.value2);//三件效果
			m_effect3.GetComponent<UILabel>().text ="[FFFF00]"  +game_data._instance.get_t_language ("equip_detail.cs_171_56") + "："+ game_data._instance.get_value_string(t_equip_tz.attr3,t_equip_tz.value3) //四件效果
				+ "  " + game_data._instance.get_value_string(t_equip_tz.attr4,t_equip_tz.value4);
		}
	}
	

	public void click(GameObject obj)
	{
		if(obj.transform.name == "ok")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "equip")
		{
			type = 1;
			reset(type);
		}
		if(obj.transform.name == "sp")
		{
			type = 0;
			reset(type);
		}
	}
}
