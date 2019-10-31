
using UnityEngine;
using System.Collections;

public class pet_dialog_box : MonoBehaviour {
	public GameObject m_title;
	public GameObject m_name;
	public GameObject m_desc;
	public GameObject m_profession;
	public GameObject m_icon;
	public UILabel m_hp;
	public UILabel m_attack;
	public UILabel m_wf;
	public UILabel m_mf;
	public UILabel m_skill_desc;
	public UILabel m_hp_up;
	public UILabel m_attack_up;
	public UILabel m_wf_up;
	public UILabel m_mf_up;
	public UILabel m_pet_desc;
	public GameObject m_sp_toggle;
	public GameObject m_pet_toggle;
	public GameObject m_scro;
	public GameObject m_pet_panel;
	public GameObject m_sp_panel;
	public int item_id;
	public int type;
	public pet m_pet = null;
	
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
			m_profession.SetActive(true);
			m_name.transform.localPosition = new Vector3 (-5,180,0);
			m_pet_toggle.SetActive(false);
			m_sp_toggle.SetActive(true);
			int sp_id = item_id;
			s_t_pet t_pet = game_data._instance.get_t_pet (item_id);
			if(t_pet != null)
			{
				for(int i =0; i < game_data._instance.m_dbc_item.get_y();++i)
				{
					int id = int.Parse(game_data._instance.m_dbc_item.get(0,i));
					s_t_item _t_item = game_data._instance.get_item(id);
					if(_t_item.type == 10001 && _t_item.def_1 == t_pet.id)
					{
						sp_id = id;
						break;
					}
				}
			}
			m_pet_panel.SetActive(false);
			m_sp_panel.SetActive(true);
			m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_dialog_box.cs_71_42");//碎片
			s_t_item t_item = game_data._instance.get_item (sp_id);
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2,sp_id,0,0);
			m_desc.GetComponent<UILabel>().text = "[fff300]" + t_item.desc;
			int num = sys._instance.m_self.get_item_num ((uint)sp_id);
			m_profession.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("card_dialog_box.cs_81_62") , " "+num.ToString());//当前拥有：{0}
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_item_icon(sp_id);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
		}
		if(select == 1)
		{
			m_pet_toggle.SetActive(true);
			m_sp_toggle.SetActive(false);
			m_pet_panel.SetActive(true);
			m_sp_panel.SetActive(false);
			m_name.transform.localPosition = new Vector3 (-5,167,0);
			m_profession.SetActive(false);
			m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_3132_76");//宠物
			int pet_id = item_id;
			s_t_item t_item = game_data._instance.get_item(item_id);
			if(t_item != null)
			{
				pet_id = t_item.def_1;
			}
			s_t_pet t_pet = game_data._instance.get_t_pet (pet_id);
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(11,pet_id,0,0);
			if(m_pet == null)
			{
				m_pet = pet.get_new_pet (t_pet.id);
			}
			sys._instance.remove_child (m_icon);
			GameObject icon = icon_manager._instance.create_pet_icon(pet_id,0,0,0);
			Transform iicon = m_icon.transform;
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
			icon.GetComponent<BoxCollider>().enabled = false;
			int value = (int)m_pet.get_attr(1);
			m_hp.text = value.ToString();
			value = (int)m_pet.get_attr(2);
			m_attack.text = value.ToString();
			value = (int)m_pet.get_attr(3);
			m_wf.text = value.ToString();
			value = (int)m_pet.get_attr(4);
			m_mf.text = value.ToString();
			double _f = t_pet.cscz[0] + t_pet.shengxing_cz[0] * m_pet.get_star();
			m_hp_up.text = _f.ToString ("f0");
			_f = t_pet.cscz[1] + t_pet.shengxing_cz[1] * m_pet.get_star();
			m_attack_up.text = _f.ToString ("f0");
			_f = t_pet.cscz[2] + t_pet.shengxing_cz[2] * m_pet.get_star();
			m_wf_up.text =  _f.ToString ("f0");
			_f = t_pet.cscz[3] + t_pet.shengxing_cz[3] * m_pet.get_star();
			m_mf_up.text =  _f.ToString ("f0");

			s_t_pet_skill t_pet_skill = game_data._instance.get_t_pet_skill(t_pet.skills[0]);
			string _skill_des = "[ffff00]" + t_pet_skill.name;
			if (t_pet_skill.attack_type == 1)
			{
				_skill_des += "[ff0000][" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2726_26") + "]\n";//物理
			}
			else if (t_pet_skill.attack_type == 2)
			{
				_skill_des += "[7fa0ff][" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
			}
			_skill_des += "[0aabff]" + t_pet_skill.des;
			int id = m_pet.m_t_pet.skills[1];
			if(id > 0)
			{
				t_pet_skill = game_data._instance.get_t_pet_skill(t_pet.skills[1]);
				_skill_des += "\n\n";
				_skill_des += "[ffff00]" + t_pet_skill.name;
				if (t_pet_skill.attack_type == 1)
				{
					_skill_des += "[ff0000][" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2726_26") + "]\n";//物理
				}
				else if (t_pet_skill.attack_type == 2)
				{
					_skill_des += "[7fa0ff][" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
				}
				else
				{
					_skill_des += "\n";
				}
				_skill_des += "[0aabff]" + t_pet_skill.des;
				_skill_des += "\n\n";
			}
			m_skill_desc.text = _skill_des;

			m_pet_desc.text = t_pet.desc;
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "ok")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "pet")
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
