
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pet_gui : MonoBehaviour {
	
	public GameObject m_pet_icon;
	public GameObject m_fight;
	public UILabel m_name;
	public GameObject m_pet_jj;
	public GameObject m_scro;
	public GameObject m_zuhe_view;
	public GameObject m_down;
	public GameObject m_xiangxi_panel;
	public GameObject m_peiyang_panel;
	public UILabel m_hp;
	public UILabel m_attack;
	public UILabel m_mf;
	public UILabel m_wf;
	public UILabel m_skill;
	public UILabel m_desc;
	public GameObject m_wy_effect;
	public GameObject m_jj_effect;
	public GameObject m_sx_effect;
	public GameObject m_peiyang_effect;
	public GameObject m_info_scro;
	public GameObject m_info_gui;
	
	private int m_select_pet_id = 0;
	private pet m_pet = null;
	private List<ulong> m_guids = new List<ulong>();
	private List<GameObject> m_pet_items = new List<GameObject>();
	private bool m_start = false;
	private ulong m_guid = 0;
	// Use this for initialization
	void Start () 
	{

	}

	public void OnEnable()
	{
		update_ui();
		int index = m_guids.IndexOf (m_guid);
		if(index != -1)
		{
			m_select_pet_id = index;
			m_guid = 0;
		}
		else
		{
			m_select_pet_id = 0;
			m_guid = 0;
		}
		if(m_start)
		{
			select_info (m_select_pet_id, false);
		}
		else
		{
			select_info (m_select_pet_id, true);
		}
		m_start = false;
	}

	public void update_ui()
	{
		m_guids.Clear ();
		m_pet_items.Clear ();
		for(int i = 0;i < sys._instance.m_self.m_t_player.pets.Count;i ++)
		{
			m_guids.Add(sys._instance.m_self.m_t_player.pets[i]);
		}
		m_guids.Sort (comp);
		sys._instance.remove_child(m_scro);
		for(int i = 0;i < m_guids.Count;i ++)
		{
			set_pet_guid(i);
		}
	}
	
	void set_pet_guid(int id)
	{
		ulong guid = m_guids [id];
		int row = id/ 2;
		int lie =  id % 2;
		GameObject obj = Instantiate (m_pet_icon) as GameObject;
		obj.transform.name = id.ToString ();
		obj.transform.parent = m_scro.transform;
		obj.transform.localPosition = new Vector3 (75 + lie * 100, 145 - row * 100, 0);
		obj.transform.localScale = new Vector3(1,1,1);
		obj.transform.Find("select").gameObject.SetActive (false);
		obj.transform.Find("effect").gameObject.SetActive (false);
		if(guid == sys._instance.m_self.m_t_player.pet_on)
		{
			obj.transform.Find("z").gameObject.SetActive (true);
			obj.transform.Find("z/z").GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_battle.cs_384_136");//出战中
		}
		else if(sys._instance.m_self.is_gaurd(guid))
		{
			obj.transform.Find("z").gameObject.SetActive (true);
			obj.transform.Find("z/z").GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_gui.cs_103_66");//护卫中
		}
		else
		{
			obj.transform.Find("z").gameObject.SetActive (false);
		}
		GameObject m_icon = obj.transform.Find("icon").gameObject;
		sys._instance.remove_child(m_icon);
		GameObject _icon = icon_manager._instance.create_pet_icon_ex (guid);
		_icon.transform.name = id.ToString();
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		if(m_guids.Count > 6)
		{
			_icon.AddComponent<UIDragScrollView>().scrollView = m_icon.transform.parent.GetComponent<UIScrollView>();
			m_scro.transform.parent.GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			m_scro.transform.parent.GetComponent<BoxCollider>().enabled = false;
		}
		UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
		meses[0].target = this.gameObject;
		meses[0].functionName = "click_pet_icon";
		meses[1].target = null;
		meses[1].functionName = "";
		meses[2].target = null;
		meses[2].functionName = "";
		obj.SetActive (true);
		m_pet_items.Add (obj);
	}

	public void select_info(int id, bool pz = false)
	{
		effect (id);
		pet_effect ();
		m_zuhe_view.GetComponent<UIScrollView>().ResetPosition ();
		ulong _guid = 0;
		_guid =  m_guids[id];
		m_guid = _guid;
		pet _pet = sys._instance.m_self.get_pet_guid (_guid);
		m_fight.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_3104_54") + sys._instance.value_to_wan((long)_pet.get_fight());//战斗力  
		s_t_pet_jinjie t_pet_jinjie = game_data._instance.get_t_pet_jinjie (_pet.get_jlevel());
		m_pet_jj.GetComponent<UISprite>().spriteName = t_pet_jinjie.icon;
		m_pet_jj.GetComponent<UISprite>().MakePixelPerfect();
		if (m_pet != _pet)
		{
			m_pet = _pet;
			pz = true;

		}
		if (pz) 
		{
			s_message _message = new s_message ();
			_message.m_type = "show_pet";
			_message.time = 0.1f;
			_message.m_object.Add (_pet);
			_message.m_ints.Add ((int)0);
			_message.m_string.Add ("select_show");
			cmessage_center._instance.add_message (_message);
		}
		m_name.text = m_pet.m_t_pet.name;
		m_hp.text = _pet.get_attr (1).ToString("f0");
		m_attack.text = _pet.get_attr (2).ToString("f0");
		m_wf.text = _pet.get_attr (3).ToString("f0");
		m_mf.text = _pet.get_attr (4).ToString("f0");
		string text = "";
		int skill_id = _pet.m_t_pet.skills[0];
		if(skill_id > 0)
		{
			s_t_pet_skill t_pet_skill = game_data._instance.get_t_pet_skill(skill_id);
			text += "[ffff00]" + t_pet_skill.name;
			if (t_pet_skill.attack_type == 1)
			{
				text += "[ff0000][" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2726_26") + "]\n";//物理
			}
			else if (t_pet_skill.attack_type == 2)
			{
				text += "[7fa0ff][" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
			}
			text += "[0aabff]" + t_pet_skill.des;
		}
		skill_id = _pet.m_t_pet.skills[1];
		if(skill_id > 0)
		{
			s_t_pet_skill t_pet_skill = game_data._instance.get_t_pet_skill(skill_id + _pet.get_star());
			text += "\n\n";
			text += "[ffff00]" + t_pet_skill.name;
			if (t_pet_skill.attack_type == 1)
			{
				text += "[ff0000][" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2726_26") + "]\n";//物理
			}
			else if (t_pet_skill.attack_type == 2)
			{
				text += "[7fa0ff][" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
			}
			else
			{
				text += "\n";
			}
			text += "[0aabff]" + t_pet_skill.des;
		}
		m_skill.text = text;
		m_desc.text = m_pet.m_t_pet.desc;
		m_pet_items [id].transform.Find("select").gameObject.SetActive (true);
	}

	int comp(ulong guid1,ulong guid2)
	{
		pet m_pet1 = sys._instance.m_self.get_pet_guid (guid1);
		pet m_pet2 = sys._instance.m_self.get_pet_guid (guid2);
		if(sys._instance.m_self.m_t_player.pet_on == guid1 && sys._instance.m_self.m_t_player.pet_on != guid2)
		{
			return -1;
		}
		else if(sys._instance.m_self.m_t_player.pet_on != guid1 && sys._instance.m_self.m_t_player.pet_on == guid2)
		{
			return 1;
		}
		else if(sys._instance.m_self.is_gaurd(guid1) &&!sys._instance.m_self.is_gaurd(guid2))
		{
			return -1;
		}
		else if(!sys._instance.m_self.is_gaurd(guid1) && sys._instance.m_self.is_gaurd(guid2))
		{
			return 1;
		}
		else if(m_pet1.get_fight() > m_pet2.get_fight())
		{
			return -1;
		}
		else if(m_pet1.get_fight() < m_pet2.get_fight())
		{
			return 1;
		}
		else if(m_pet1.m_t_pet.color >  m_pet2.m_t_pet.color)
		{
			return -1;
		}
		else if(m_pet1.m_t_pet.color <  m_pet2.m_t_pet.color)
		{
			return 1;
		}
		else if(m_pet1.m_t_pet.id >  m_pet2.m_t_pet.id)
		{
			return -1;
		}
		else if(m_pet1.m_t_pet.id <  m_pet2.m_t_pet.id)
		{
			return 1;
		}
		return 0;
	}

	public void click_pet_icon(GameObject obj)
	{
		m_pet_items [m_select_pet_id].transform.Find("select").gameObject.SetActive (false);
		m_select_pet_id = int.Parse (obj.transform.name);
		select_info (m_select_pet_id);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "pei_yang")
		{
			m_down.SetActive(false);
			m_peiyang_panel.SetActive(true);
			m_xiangxi_panel.SetActive(false);
		}
		else if(obj.transform.name == "xiang_xi")
		{
			m_peiyang_panel.SetActive(false);
			m_down.SetActive(true);
			m_xiangxi_panel.SetActive(true);
		}
		else if (obj.transform.name == "chong_neng")
		{
			s_message _message = new s_message();
			_message.m_type = "show_pet_cn_gui";
			_message.m_long.Add(m_pet.get_guid());
			_message.m_string.Add("show_pet_gui");
			cmessage_center._instance.add_message(_message);
			this.GetComponent<ui_show_anim>().hide_ui();
			this.GetComponent<gui_remove>().m_remove = false;
		}
		else if (obj.transform.name == "shengxing")
		{
			s_message _message = new s_message();
			_message.m_type = "show_pet_sx_gui";
			_message.m_long.Add(m_pet.get_guid());
			_message.m_string.Add("show_pet_gui");
			cmessage_center._instance.add_message(_message);
			this.GetComponent<ui_show_anim>().hide_ui();
			this.GetComponent<gui_remove>().m_remove = false;
		}
		else if (obj.transform.name == "jinjie")
		{
			s_message _message = new s_message();
			_message.m_type = "show_pet_jj_gui";
			_message.m_long.Add(m_pet.get_guid());
			_message.m_string.Add("show_pet_gui");
			cmessage_center._instance.add_message(_message);
			this.GetComponent<ui_show_anim>().hide_ui();
			this.GetComponent<gui_remove>().m_remove = false;
		}
		else if (obj.transform.name == "tj")
		{
			s_message _message = new s_message();
			_message.m_type = "show_pet_tj_gui";
			_message.m_long.Add(m_pet.get_guid());
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.transform.name == "close")
		{
			m_start = false;
			this.GetComponent<ui_show_anim>().hide_ui();
			m_pet = null;
			s_message _message = new s_message();
			_message.m_type = "hide_show_unit";
			cmessage_center._instance.add_message (_message);
			
			_message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
			
			_message = new s_message();
			_message.m_type = "hide_unit_show";
			cmessage_center._instance.add_message(_message);
			this.GetComponent<gui_remove>().m_remove = true;
		}
		else if(obj.transform.name == "sm")
		{
			if(m_info_scro.transform.GetComponent<SpringPanel>() != null)
			{
				m_info_scro.transform.GetComponent<SpringPanel>().enabled = false;
			}
			m_info_scro.transform.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
			m_info_scro.transform.localPosition = new Vector3 (0, 0, 0);
			m_info_gui.SetActive(true);
		}
		else if(obj.transform.name == "sm_close")
		{
			m_info_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	public void pet_effect()
	{
		for(int i = 0; i < m_guids.Count;++i)
		{
			if(m_guids[i] ==  0)
			{
				continue;
			}
			else
			{
				if(can_effect(m_guids[i]))
				{
					m_pet_items[i].transform.Find("effect").gameObject.SetActive(true);
				}
				else
				{
					m_pet_items[i].transform.Find("effect").gameObject.SetActive(false);
				}
			}
		}
	}

	public void effect(int id)
	{
		ulong _guid = 0;
		_guid = m_guids[id];
		bool flag = false;
		pet _pet = sys._instance.m_self.get_pet_guid (_guid);
		
		if (pet_weiyang_gui.is_weiyang(_pet))
		{
			flag = true;
			m_wy_effect.SetActive(true);
		}
		else
		{
			m_wy_effect.SetActive(false);
		}
		
		if(pet_jingjie_gui.is_jingjie(_pet))
		{
			flag = true;
			m_jj_effect.SetActive(true);
		}
		else
		{
			m_jj_effect.SetActive(false);
		}
		
		if(pet_shengxing_gui.is_shengxing(_pet))
		{
			flag = true;
			m_sx_effect.SetActive(true);
		}
		else
		{
			m_sx_effect.SetActive(false);
		}
		m_peiyang_effect.SetActive (flag);
	}

	public static bool can_effect(ulong guid)
	{
		pet _pet = sys._instance.m_self.get_pet_guid (guid);
		if(pet_weiyang_gui.is_weiyang(_pet))
		{
			return true;
		}
		if(pet_jingjie_gui.is_jingjie(_pet))
		{
			return true;
		}
		if (pet_shengxing_gui.is_shengxing(_pet))
		{
			return true;
		}
		return false;
	}

	public static bool is_effect()
	{
		for(int i = 0; i < sys._instance.m_self.m_t_player.pets.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.pets[i] ==  0)
			{
				continue;
			}
			else
			{
				if(can_effect(sys._instance.m_self.m_t_player.pets[i]))
				{
					return true;
				}
			}
		}
		return false;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
