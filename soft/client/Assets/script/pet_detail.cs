
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pet_detail : MonoBehaviour {

	private dhc.pet_t m_pet;
	public GameObject m_name;
	public UILabel m_fight;
	public GameObject m_Lv;
	public GameObject m_jj;
	public List<GameObject>m_jp_atts = new List<GameObject>();
	public List<GameObject> m_jj_atts = new List<GameObject>();
	public List<GameObject> m_sx_atts = new List<GameObject>();
	public GameObject m_icon;
	public UILabel m_desc;
	public GameObject m_sj_effect;
	public GameObject m_jj_effect;
	public GameObject m_sx_effect;
	public GameObject m_scro;
	public bool m_flag = false;
	public GameObject m_skill_desc;
	public GameObject m_js_desc;
	public GameObject m_star1;
	public GameObject m_star2;
	private int m_type;
	private ulong _gui;
	// Use this for initialization
	void Start () {
	
	}

	public void reset(dhc.pet_t _pet,int type)
	{
		m_pet = _pet;
		m_type = type;
		pet m_t_pet = sys._instance.m_self.get_pet_guid (m_pet.guid);
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		
		s_t_pet t_pet = game_data._instance.get_t_pet(m_pet.template_id);
		m_name.GetComponent<UILabel>().text = pet.get_color_name (t_pet.name,t_pet.color);
		int enhance_up = game_data._instance.m_dbc_exp.get_y ();
		m_Lv.GetComponent<UILabel>().text =  m_pet.level.ToString() + "/" + enhance_up;
		int sx_num = game_data._instance.m_dbc_chongwu_shengxing.get_y ();
		m_star1.GetComponent<UISprite>().width = 37*sx_num;
		m_star2.GetComponent<UISprite>().width = 18*m_pet.star;
		m_star2.transform.localPosition = new Vector3(-sx_num*13,-35,0);
		m_jj.GetComponent<UILabel>().text = game_data._instance.get_t_pet_jinjie(m_pet.jinjie).chenghao;
		m_fight.text = game_data._instance.get_t_language ("pet_detail.cs_55_17") + sys._instance.value_to_wan ((long)m_t_pet.get_fight ());//战斗力 
		for(int i = 0; i < m_jp_atts.Count;++i)
		{
			m_jp_atts[i].SetActive(true);
			m_jp_atts[i].GetComponent<UILabel>().text = game_data._instance.get_t_value(i+1).name + "+" + m_t_pet.get_attr(i+1).ToString("f0");
		}
		for(int i = 0; i < m_jj_atts.Count;++i)
		{
			m_jj_atts[i].GetComponent<UILabel>().text =game_data._instance.get_t_value(i+1).name + "+"+ get_pet_jingjie_attr(i+1,m_pet.jinjie).ToString("f0");
		}
		
		for(int i = 0; i < m_sx_atts.Count;++i)
		{
			m_sx_atts[i].GetComponent<UILabel>().text = game_data._instance.get_t_value(i+1).name + "+"+m_t_pet.get_pet_shengxing_cz_attr(i+1,m_pet.star).ToString("f0");
		}
		m_js_desc.GetComponent<UILabel>().text = t_pet.desc;
		string text = "";
		int id = t_pet.skills[0];
		if(id > 0)
		{
			s_t_pet_skill t_pet_skill = game_data._instance.get_t_pet_skill(id);
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
		id = t_pet.skills[1];
		if(id > 0)
		{
			s_t_pet_skill t_pet_skill = game_data._instance.get_t_pet_skill(id + m_pet.star);
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
		m_skill_desc.GetComponent<UILabel>().text = text;
		sys._instance.remove_child (m_icon);
		GameObject iicon = icon_manager._instance.create_pet_icon(m_pet.guid);
		iicon.transform.parent = m_icon.transform;
		iicon.transform.localPosition = new Vector3(0,0,0);
		iicon.transform.localScale = new Vector3(1.15f,1.15f,1.15f);
		iicon.transform.GetComponent<BoxCollider>().enabled = false;
		if(pet_weiyang_gui.is_weiyang(m_t_pet))
		{
			m_sj_effect.SetActive(true);
		}
		else
		{
			m_sj_effect.SetActive(false);
		}
		if(pet_jingjie_gui.is_jingjie(m_t_pet))
		{
			m_jj_effect.SetActive(true);
		}
		else
		{
			m_jj_effect.SetActive(false);
		}
		if(pet_shengxing_gui.is_shengxing(m_t_pet))
		{
			m_sx_effect.SetActive(true);
		}
		else
		{
			m_sx_effect.SetActive(false);
		}
		m_desc.text = string.Format (game_data._instance.get_t_language ("pet_detail.cs_138_31"),t_pet.sx_add*100);//护卫时，提供基本属性*{0}%的属性加成
	}

	public double get_pet_jingjie_attr(int type,int jingjie)
	{
		s_t_pet m_t_pet = game_data._instance.get_t_pet(m_pet.template_id);
		List<double> attrs = new List<double>();
		for (int i = 0; i < 50; ++i)
		{
			attrs.Add(0);
		}
		float add_per = 0.0f ;
		for(int i = 0; i <= jingjie;++i)
		{
			s_t_pet_jinjie t_pet_jingjie = game_data._instance.get_t_pet_jinjie (i);
			if(t_pet_jingjie != null && t_pet_jingjie.qsx_add > 0)
			{
				add_per += (float)t_pet_jingjie.qsx_add/100;
			}
			if(t_pet_jingjie != null && t_pet_jingjie.esx_add > 0)
			{
				attrs[m_t_pet.jinjie_add_sx[t_pet_jingjie.esx_add-1].attr] += m_t_pet.jinjie_add_sx[t_pet_jingjie.esx_add-1].value;
			}
		}
		add_per += 1.0f;
		for(int i = 0; i < jingjie;++i)
		{
			s_t_pet_jinjie t_jingjie = game_data._instance.get_t_pet_jinjie(i);
			if(t_jingjie != null)
			{
				for(int j = 0; j < t_jingjie.cls.Count;++j)
				{
					s_t_pet_jinjieitem t_jinjieitem = game_data._instance.get_t_pet_jinjieitem(t_jingjie.cls[j]);
					if(t_jinjieitem != null)
					{
						for(int k= 0; k < t_jinjieitem.attrs.Count;++k)
						{
							attrs[t_jinjieitem.attrs[k].attr] += t_jinjieitem.attrs[k].value;
						}
					}
				}
			}
		}
		if(jingjie <= m_pet.jinjie)
		{
			for(int i = 0; i < m_pet.jinjie_slot.Count;++i)
			{
				int id = m_pet.jinjie_slot[i];
				if(id <= 0)
				{
					continue;
				}
				s_t_pet_jinjieitem t_pet_jingjieitem = game_data._instance.get_t_pet_jinjieitem(id);
				if(t_pet_jingjieitem != null)
				{
					for(int j= 0; j < t_pet_jingjieitem.attrs.Count;++j)
					{
						attrs[t_pet_jingjieitem.attrs[j].attr] += t_pet_jingjieitem.attrs[j].value;
					}
				}
			}
		}
		attrs[3] += attrs[29];
		attrs[4] += attrs[29];
		return attrs [type];
	}

	public void click(GameObject obj)
	{
		if (obj.name == "ok")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "remove")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "select_pet_guard";
			_message.m_long.Add(m_pet.guid);
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.name == "weiyang")
		{
			s_message msg = new s_message();
			msg.m_type = "show_pet_cn_gui";
			msg.m_long.Add(m_pet.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_buzheng");
			}
			cmessage_center._instance.add_message(msg);
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_pet_buzheng";
				cmessage_center._instance.add_message(msg1);
			}
			pet m_t_pet = sys._instance.m_self.get_pet_guid (m_pet.guid);
			s_message _message = new s_message ();
			_message.m_type = "show_pet";
			_message.time = 0.1f;
			_message.m_object.Add (m_t_pet);
			_message.m_ints.Add ((int)0);
			_message.m_bools.Add (true);
			_message.m_string.Add ("select_show");
			cmessage_center._instance.add_message (_message);
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "jinjie")
		{
			s_message msg = new s_message();
			msg.m_type = "show_pet_jj_gui";
			msg.m_long.Add(m_pet.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_buzheng");
			}
			cmessage_center._instance.add_message(msg);
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_pet_buzheng";
				cmessage_center._instance.add_message(msg1);
			}
			pet m_t_pet = sys._instance.m_self.get_pet_guid (m_pet.guid);
			s_message _message = new s_message ();
			_message.m_type = "show_pet";
			_message.time = 0.1f;
			_message.m_object.Add (m_t_pet);
			_message.m_ints.Add ((int)0);
			_message.m_bools.Add (true);
			_message.m_string.Add ("select_show");
			cmessage_center._instance.add_message (_message);
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "shengxing")
		{
			s_message msg = new s_message();
			msg.m_type = "show_pet_sx_gui";
			msg.m_long.Add(m_pet.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_buzheng");
			}
			cmessage_center._instance.add_message(msg);
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_pet_buzheng";
				cmessage_center._instance.add_message(msg1);
			}
			pet m_t_pet = sys._instance.m_self.get_pet_guid (m_pet.guid);
			s_message _message = new s_message ();
			_message.m_type = "show_pet";
			_message.time = 0.1f;
			_message.m_object.Add (m_t_pet);
			_message.m_ints.Add ((int)0);
			_message.m_bools.Add (true);
			_message.m_string.Add ("select_show");
			cmessage_center._instance.add_message (_message);
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "tihuan")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
			s_message msg1 = new s_message();
			msg1.m_type = "pet_guard_tihuan";
			cmessage_center._instance.add_message(msg1);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
