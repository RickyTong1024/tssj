
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_dialog_box : MonoBehaviour {

	public GameObject m_name;
	public GameObject m_des;
	public GameObject m_num;
	public GameObject m_icon;
	public GameObject m_treasure_panel;
	public GameObject m_sp_panel;
	public GameObject m_title;
	public GameObject m_jb_att;
	public GameObject m_jb_att1;
	public GameObject m_enhance_att;
	public GameObject m_enhance_att1;
	public List<GameObject> m_jl_atts = new List<GameObject>();
	public GameObject m_treausre_skill_sprite;
	public dhc.treasure_t m_treasure = null;
	public GameObject m_shengqi_desc;
	public GameObject m_scro;
	public GameObject m_sp_toggle;
	public GameObject m_treasure_toggle;
	public GameObject m_sp_button;
	public GameObject m_treasure_button;
	public int item_id;
	public int type;
	public bool flag = false;
	// Use this for initialization
	void Start () {
		
	}
	
	public void OnEnable()
	{
		if(type == 2)
		{
			flag = true;
		}
		else
		{
			flag= false;
		}
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
		m_sp_button.SetActive(true);
		m_treasure_button.GetComponent<BoxCollider>().enabled = true;
		if(select == 0)
		{
			m_treasure_toggle.SetActive(false);
			m_sp_toggle.SetActive(true);
			int sp_id = item_id;
			s_t_baowu t_treasure = game_data._instance.get_t_baowu (item_id);
			if(t_treasure != null)
			{
				for(int i =0; i < game_data._instance.m_dbc_item.get_y();++i)
				{
					int id = int.Parse(game_data._instance.m_dbc_item.get(0,i));
					s_t_item _t_item = game_data._instance.get_item(id);
					if(_t_item.type == 6001 && _t_item.def_1 == t_treasure.id)
					{
						sp_id = id;
						break;
					}
				}
			}
			m_treasure_panel.SetActive(false);
			m_sp_panel.SetActive(true);
			m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_dialog_box.cs_71_42");//碎片
			s_t_item t_item = game_data._instance.get_item (sp_id);
			s_t_baowu t_baowu = game_data._instance.get_t_baowu (t_item.def_1);
			if(t_baowu.type == 5)
			{
				flag = true;
			}
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2,sp_id,0,0);
			m_des.GetComponent<UILabel>().text = "[fff300]" + t_item.desc;
			int num = sys._instance.m_self.get_item_num ((uint)sp_id);
			m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ num.ToString();//当前拥有：
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
			m_treasure_toggle.SetActive(true);
			m_sp_toggle.SetActive(false);
			int treasure_id = item_id;
			m_treasure_panel.SetActive(true);
			m_sp_panel.SetActive(false);
			m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("treasure_dialog_box.cs_105_42");//饰品
			s_t_item t_item = game_data._instance.get_item(item_id);
			if(t_item != null)
			{
				treasure_id = t_item.def_1;
			}
			s_t_baowu t_treaure = game_data._instance.get_t_baowu (treasure_id);
			m_num.GetComponent<UILabel>().text = treasure.get_treasure_part (treasure_id);
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(6,treasure_id,0,0);
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_treasure_icon(treasure_id);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
			bool flag = false;
			m_jb_att.GetComponent<UILabel>().text = game_data._instance.get_float_string(t_treaure.attr1,t_treaure.value1);
			m_jb_att1.GetComponent<UILabel>().text = game_data._instance.get_float_string(t_treaure.attr2,t_treaure.value2);
			m_enhance_att.GetComponent<UILabel>().text = game_data._instance.get_value_string(t_treaure.attr1,t_treaure.value1 *1);
			m_enhance_att1.GetComponent<UILabel>().text = game_data._instance.get_value_string(t_treaure.attr2,t_treaure.value2 *1);
			for(int i = 0; i < m_jl_atts.Count;++i)
			{
				m_jl_atts[i].SetActive(true);
				m_jl_atts[i].GetComponent<UILabel>().text = treasure.get_treasure_jl_text(treasure_id,1,i);
				if(treasure.get_treasure_jl_text(treasure_id,1,i) == "")
				{
					flag = true;
					m_jl_atts[i].SetActive(false);
				}
			}
			if(t_treaure.font_color == 5)
			{
				m_shengqi_desc.GetComponent<UILabel>().text = equip.equip_skill(t_treaure.type+4,0,1);
				m_treausre_skill_sprite.SetActive(true);
				if(flag)
				{
					m_treausre_skill_sprite.transform.localPosition = new Vector3(0,-277,0);
				}
				else
				{
					m_treausre_skill_sprite.transform.localPosition = new Vector3(0,-313,0);
				}
			}
			else
			{
				m_treausre_skill_sprite.SetActive(false);
			}
		}
		else if(select == 2)
		{
			m_treasure_button.GetComponent<BoxCollider>().enabled = false;
			m_treasure_toggle.SetActive(true);
			m_sp_toggle.SetActive(false);
			int treasure_id = item_id;
			s_t_item t_item = game_data._instance.get_item(item_id);
			if(t_item != null)
			{
				treasure_id = t_item.def_1;
			}
			m_treasure_panel.SetActive(false);
			m_sp_panel.SetActive(true);
			m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("treasure_dialog_box.cs_105_42");//饰品
			s_t_baowu t_treaure = game_data._instance.get_t_baowu (treasure_id);
			int num = sys._instance.m_self.get_treasure_num (treasure_id);
			m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ num.ToString();//当前拥有：
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(6,treasure_id,0,0);
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_treasure_icon(treasure_id);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
			m_des.GetComponent<UILabel>().text = "[fff300]" + string.Format(game_data._instance.get_t_language ("treasure.cs_46_41"), + t_treaure.exp );//饰品强化时提供{0}点饰品经验
		}
	}
	
	public void click(GameObject obj)
	{
		if(obj.transform.name == "ok")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "treasure")
		{
			if(flag)
			{
				type = 2;
			}
			else
			{
				type = 1;
			}
			reset(type);
		}
		if(obj.transform.name == "sp")
		{
			type = 0;
			reset(type);
		}
	}
}
