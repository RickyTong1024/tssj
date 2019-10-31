
using UnityEngine;
using System.Collections;

public class pet_info : MonoBehaviour,IMessage {

	public GameObject m_name;
	public GameObject m_desc;
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
	public GameObject m_scroll_view;
	public int item_id;
	public int type;
	public pet m_pet = null;

	void OnEnable()
	{
		resert ();
	}
	void IMessage.message(s_message message)
	{
		
	}
	
	void IMessage.net_message(s_net_message message)
	{
		
	}
	public void click(GameObject obj)
	{
		if(obj.name == "close")
		{
			s_message mes = new s_message ();
			mes.m_type = "hide_ui_unit_cam";
			cmessage_center._instance.add_message (mes);
			this.gameObject.SetActive(false);
		}
		if(obj.name == "role")
		{
			s_message mes = new s_message ();
			mes.m_type = "action_ui_unit_cam";
			cmessage_center._instance.add_message (mes);
		}
	}
	public void resert()
	{
		m_scroll_view.GetComponent<UIScrollView>().ResetPosition ();
		
		//GameObject icon = icon_manager._instance.create_card_icon_ex(m_card);
		
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
		icon.GetComponent<BoxCollider>().enabled = false;
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
		
		m_desc.GetComponent<UILabel>().text = t_pet.desc;

		s_message mes = new s_message();
		mes.m_type = "show_ui_pet_cam";
        mes.m_object.Add(new Vector3(0, 2.3f, 16));
        mes.m_object.Add(new Vector3(0, 180, 0));
        mes.m_object.Add(m_pet);
		cmessage_center._instance.add_message(mes);
	}

	void Start () {
	
	}

	void Update () {
	
	}
}
