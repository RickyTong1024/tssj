
using UnityEngine;
using System.Collections;

public class item_dialog_box : MonoBehaviour {
	
	public GameObject m_name_obj;
	public GameObject m_des_obj;
	public GameObject m_level;
	public GameObject m_icon;
	public GameObject m_num;

	public int item_id;
	public int m_type;
	public string desc;
	// Use this for initialization
	void Start () {
		
	}
	public void OnEnable()
	{
		if(m_type == 2)
		{
			m_icon.transform.GetComponent<UISprite>().height = 88;
			m_icon.transform.GetComponent<UISprite>().width = 88;
			m_num.SetActive(true);
			s_t_item t_item = game_data._instance.get_item (item_id);
			m_name_obj.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2,item_id,0,0);
			string desc = "";
			desc =  t_item.desc;
			if(item_id == (int)e_huodong_item_id.ei_huodong_item1)
			{
				desc = sys._instance.m_self.m_jieri_icon_desc;
			}
			if(item_id == (int)e_huodong_item_id.ei_huodong_item2)
			{
				desc = sys._instance.m_self.m_jieri_icon_desc1;
			}
			m_des_obj.GetComponent<UILabel>().text = "[fff300]" + desc;
			int nlevel = t_item.need_level;
			if(nlevel > 0)
			{
				m_name_obj.transform.localPosition = new Vector3(-40,94,0);
				m_level.transform.localPosition = new Vector3(-40,64,0);
				m_num.transform.localPosition = new Vector3(-40,34,0);
				m_level.SetActive(true);
				if (nlevel > sys._instance.m_self.m_t_player.level)
				{
					m_level.GetComponent<UILabel>().text = "[ff0000]" + string.Format(game_data._instance.get_t_language ("bag_gui.cs_1633_70"),nlevel.ToString());//{0}级可使用
				}
				else
				{
                    m_level.GetComponent<UILabel>().text = "[ff0000]" + string.Format(game_data._instance.get_t_language ("bag_gui.cs_1633_70"), nlevel.ToString());//{0}级可使用
				}
			}
			else
			{
				m_level.SetActive(false);
				m_name_obj.transform.localPosition = new Vector3(-40,84,0);
				m_num.transform.localPosition = new Vector3(-40,46,0);
			}
			int num = sys._instance.m_self.get_item_num ((uint)item_id);
			m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ num.ToString();//当前拥有：
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_item_icon(item_id);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
		}
		else if(m_type == 1)
		{
			m_icon.transform.GetComponent<UISprite>().height = 88;
			m_icon.transform.GetComponent<UISprite>().width = 88;
			m_num.SetActive(true);
			m_level.SetActive(false);
			m_name_obj.transform.localPosition = new Vector3(-40,84,0);
			m_num.transform.localPosition = new Vector3(-40,46,0);
			m_name_obj.GetComponent<UILabel>().text = sys._instance.m_self.get_name(1,item_id,0,0);
			m_des_obj.GetComponent<UILabel>().text = "[fff300]" + desc;
			int num = sys._instance.m_self.get_res_num(item_id);
			m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ sys._instance.value_to_wan(num);//当前拥有：
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_resource_icon(item_id,0);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
		}
		else if(m_type == 3)
		{
			m_icon.transform.GetComponent<UISprite>().height = 88;
			m_icon.transform.GetComponent<UISprite>().width = 88;
			m_level.SetActive(false);
			m_name_obj.transform.localPosition = new Vector3(-40,58,0);
			m_num.SetActive(false);
			s_t_dress t_dress = game_data._instance.get_t_dress(item_id);
			m_name_obj.GetComponent<UILabel>().text = game_data._instance.get_name_color(t_dress.color) + t_dress.name;
			m_des_obj.GetComponent<UILabel>().text = "[fff300]" + t_dress.get_des();
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_dress_icon_ex(item_id,1);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
		}
		else if(m_type == 4)
		{
			m_level.SetActive(false);
			m_name_obj.transform.localPosition = new Vector3(-40,64,0);
			m_icon.transform.GetComponent<UISprite>().height = 64;
			m_icon.transform.GetComponent<UISprite>().width = 64;
			m_num.SetActive(false);
			s_t_skill t_skill = game_data._instance.get_t_skill(item_id);
			m_name_obj.GetComponent<UILabel>().text = t_skill.name;
			m_des_obj.GetComponent<UILabel>().text = "[fff300]" +desc;
			sys._instance.remove_child (m_icon);
			GameObject icon =icon_manager._instance.create_skill_icon(item_id);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
		}
		else if(m_type == 5)
		{
			m_icon.transform.GetComponent<UISprite>().height = 88;
			m_icon.transform.GetComponent<UISprite>().width = 88;
			m_level.SetActive(false);
			m_name_obj.transform.localPosition = new Vector3(-40,58,0);
			m_num.SetActive(false);
			s_t_role_dress t_dress = game_data._instance.get_t_role_dress(item_id);
			m_name_obj.GetComponent<UILabel>().text = t_dress.name;
			m_des_obj.GetComponent<UILabel>().text = "[fff300]" + t_dress.mai_desc;
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_dressrole_icon_ex(item_id);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
		}
	}

	public void click(GameObject obj)
	{
		this.transform.GetComponent<ui_show_anim>().hide_ui ();
	}
}
