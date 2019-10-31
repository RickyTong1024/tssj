
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_detail : MonoBehaviour, IMessage {
	
	private dhc.equip_t m_equip;
	private int m_type;
	public GameObject m_name;
	public GameObject m_Lv;
	public GameObject m_sx;
	public GameObject m_jl;
	public GameObject m_part;
	public List<GameObject>m_jp_atts = new List<GameObject>();
	public List<GameObject> m_atts = new List<GameObject>();
	public List<GameObject> m_sx_atts = new List<GameObject>();
	public List<GameObject> m_jl_atts = new List<GameObject>();
	public GameObject m_icon;
	public GameObject m_tihuan;
	public GameObject m_gaizao;
	public GameObject[] m_xicon;
	public GameObject[] m_tz_name;
	public GameObject m_effect1;
	public GameObject m_effect2;
	public GameObject m_effect3;
	public GameObject m_xname;
	public GameObject effect;
	public GameObject m_jl_effect;
	public GameObject m_scro;
	public GameObject m_bj;
	public GameObject m_back;
	public GameObject m_wight;
	public GameObject m_buttom;
	public bool m_flag = false;
	public GameObject m_remove;
	public List<dhc.equip_t> m_t_equips = new List<dhc.equip_t>();
	public GameObject m_tz_sprite;
	public GameObject m_shenxing_sprite;
	public GameObject m_equip_skill_sprite;
	public GameObject m_gaizao_sprite;
	public GameObject m_jl_sprite;
	public GameObject m_desc;
	public GameObject m_star1;
	public GameObject m_star2;
	private ulong _gui;

	public UILabel m_jibenshuxing;
	public UILabel m_gaizaoshuxing;
	public UILabel m_gaizao_label;
	public UILabel m_xiangqian_label;
	public UILabel m_shengxing_label;
	public UILabel m_qianghua_label;
	public UILabel m_suoding_label;
	public UILabel m_tihuan_label;
	public UILabel m_tishi1;
	public UILabel m_qianghuadengji;
	public UILabel m_xiexia;
	public UILabel m_sx_Label;
	public UILabel m_jl_Label;
	public UILabel m_title;
	// Use this for initialization
	void Start () 
	{
		
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void reset(dhc.equip_t _equip, int type)
	{ 
		m_type = type;
		m_equip = _equip;
		m_scro.SetActive(true);
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		if(m_flag)
		{
			m_remove.SetActive(true);
		}
		else
		{
			m_remove.SetActive(false);
		}

		s_t_equip t_equip = game_data._instance.get_t_equip(m_equip.template_id);
		s_t_equip_tz t_equip_tz = game_data._instance.get_t_equip_tz (m_equip.template_id);
		m_name.GetComponent<UILabel>().text = equip.get_equip_real_name (m_equip.template_id);
		m_xname.GetComponent<UILabel>().text = equip.get_equip_color(m_equip.template_id) + t_equip_tz.name;
		int enhance_up = sys._instance.m_self.m_t_player.level;
		if(sys._instance.m_self.m_t_player.level > (game_data._instance.m_dbc_enhance.get_y() -1))
		{
			enhance_up = game_data._instance.m_dbc_enhance.get_y() - 1;
		}
		int jl_up = game_data._instance.m_dbc_equip_jl.get_y ();
		m_Lv.GetComponent<UILabel>().text =  m_equip.enhance.ToString() + "/" + enhance_up;
		s_t_equip_sx _equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color,m_equip.star);
		if(_equip_sx != null)
		{
			int sx_num = 0;
			for(int j = 0;j < game_data._instance.m_dbc_equip_sx.get_y();++j)
			{
				if(int.Parse (game_data._instance.m_dbc_equip_sx.get (1, j)) == t_equip.font_color)
				{
					sx_num++;
				}
			}
			sx_num = sx_num -1;
			m_star1.GetComponent<UISprite>().width = 37*sx_num;
			m_star2.GetComponent<UISprite>().width = 18*m_equip.star;
		}
		m_jl.GetComponent<UILabel>().text = m_equip.jilian.ToString() + "/" + jl_up;
		m_part.GetComponent<UILabel>().text = equip.get_equip_part_e (m_equip.template_id);
		for(int i = 0; i < m_jp_atts.Count;++i)
		{
			m_jp_atts[i].SetActive(true);
			m_jp_atts[i].GetComponent<UILabel>().text = equip.get_equip_value_text(m_equip.template_id,m_equip.enhance,m_equip.star,i);
			if(m_jp_atts[i].GetComponent<UILabel>().text == "0")
			{
				m_jp_atts[i].SetActive(false);
			}
		}
		for(int i = 0; i < m_atts.Count;++i)
		{
			if(i < m_equip.rand_ids.Count)
			{
				m_atts[i].GetComponent<UILabel>().text = equip.get_equip_random_value2(m_equip.rand_values[i], m_equip.rand_ids[i],t_equip,m_equip);
			}
			else
			{
				m_atts[i].GetComponent<UILabel>().text = "[2299ff]????????";
			}
		}

		for(int i = 0; i < m_sx_atts.Count;++i)
		{
			if(equip.get_equip_sx_text(m_equip,m_equip.star,i) == "")
			{
				m_sx_atts[i].SetActive(false);
			}
			else
			{
				m_sx_atts[i].SetActive(true);
				m_sx_atts[i].GetComponent<UILabel>().text = equip.get_equip_sx_text(m_equip,m_equip.star,i);
			}
		}
		for(int i = 0; i < m_jl_atts.Count;++i)
		{
			m_jl_atts[i].GetComponent<UILabel>().text = equip.get_equip_jl_text(m_equip,m_equip.jilian,i);
		}
		
		sys._instance.remove_child (m_icon);
		GameObject iicon = icon_manager._instance.create_equip_icon(m_equip.guid);
		iicon.transform.parent = m_icon.transform;
		iicon.transform.localPosition = new Vector3(0,0,0);
		iicon.transform.localScale = new Vector3(1.15f,1.15f,1.15f);
		iicon.transform.GetComponent<BoxCollider>().enabled = false;
		int tz_num = get_tz_num (t_equip_tz);
		m_effect1.GetComponent<UILabel>().text ="[777777]" +game_data._instance.get_t_language ("equip_detail.cs_169_55") + "："+ game_data._instance.get_value_string(t_equip_tz.attr1,t_equip_tz.value1);//两件效果
		m_effect2.GetComponent<UILabel>().text = "[777777]" +game_data._instance.get_t_language ("equip_detail.cs_170_56") + "："+ game_data._instance.get_value_string(t_equip_tz.attr2,t_equip_tz.value2);//三件效果
		m_effect3.GetComponent<UILabel>().text = "[777777]" +game_data._instance.get_t_language ("equip_detail.cs_171_56") + "："+game_data._instance.get_value_string(t_equip_tz.attr3,t_equip_tz.value3) 	+ "  " + game_data._instance.get_value_string(t_equip_tz.attr4,t_equip_tz.value4);//四件效果
		if(tz_num == 2)
		{
			m_effect1.GetComponent<UILabel>().text ="[FFFF00]"+game_data._instance.get_t_language ("equip_detail.cs_169_55") + "：" + game_data._instance.get_value_string(t_equip_tz.attr1,t_equip_tz.value1);//两件效果
		}
		if(tz_num == 3)
		{
			m_effect1.GetComponent<UILabel>().text ="[FFFF00]"+game_data._instance.get_t_language ("equip_detail.cs_169_55") + "：" + game_data._instance.get_value_string(t_equip_tz.attr1,t_equip_tz.value1);//两件效果
			m_effect2.GetComponent<UILabel>().text ="[FFFF00]" +game_data._instance.get_t_language ("equip_detail.cs_170_56") + "："+ game_data._instance.get_value_string(t_equip_tz.attr2,t_equip_tz.value2);//三件效果
		}
		if(tz_num == 4)	{
			m_effect1.GetComponent<UILabel>().text ="[FFFF00]"+game_data._instance.get_t_language ("equip_detail.cs_169_55") + "：" + game_data._instance.get_value_string(t_equip_tz.attr1,t_equip_tz.value1);//两件效果
			m_effect2.GetComponent<UILabel>().text ="[FFFF00]" +game_data._instance.get_t_language ("equip_detail.cs_170_56") + "："+ game_data._instance.get_value_string(t_equip_tz.attr2,t_equip_tz.value2);//三件效果
			m_effect3.GetComponent<UILabel>().text ="[FFFF00]"  +game_data._instance.get_t_language ("equip_detail.cs_171_56") + "："+ game_data._instance.get_value_string(t_equip_tz.attr3,t_equip_tz.value3) //四件效果
				+ "  " + game_data._instance.get_value_string(t_equip_tz.attr4,t_equip_tz.value4);
		}
		for(int i =0 ;i < 4; ++i)
		{
			sys._instance.remove_child (m_xicon[i]);
			GameObject iicon1 = icon_manager._instance.create_equip_icon(t_equip_tz.equip_ids[i]);
			iicon1.transform.parent = m_xicon[i].transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1.15f,1.15f,1.15f);
			iicon1.transform.GetComponent<BoxCollider>().enabled = true;
			m_tz_name[i].GetComponent<UILabel>().text = equip.get_equip_real_name(t_equip_tz.equip_ids[i]);
		}

		if (m_type == 0)
		{
			m_tihuan.gameObject.SetActive(false);
		}
		else if(m_type == 3)
		{
			m_tihuan.gameObject.SetActive(false);
			m_scro.GetComponent<UIPanel>().SetRect(0,-80, 492.0f,382.0f);
			m_scro.transform.localPosition = new Vector3(0, 0, 0);
			m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			m_bj.GetComponent<UISprite>().height = 380;
			m_buttom.transform.localPosition = new Vector3(0,-275,0);
			m_back.GetComponent<UISprite>().height = 550;
			m_wight.GetComponent<UIWidget>().height = 380;
			m_wight.GetComponent<BoxCollider>().center = new Vector3(0,-190,0);
			m_wight.GetComponent<BoxCollider>().size = new Vector3(440,380,0);
		}
		else
		{
			m_tihuan.gameObject.SetActive(true);
			m_scro.GetComponent<UIPanel>().SetRect(0,-51, 464.0f,320.0f);
			m_scro.transform.localPosition = new Vector3(0, 0, 0);
			m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			m_bj.GetComponent<UISprite>().height = 323;
			m_buttom.transform.localPosition = new Vector3(0,-220,0);
			m_back.GetComponent<UISprite>().height = 497;
			m_wight.GetComponent<UIWidget>().height = 318;
			m_wight.GetComponent<BoxCollider>().center = new Vector3(0,-159,0);
			m_wight.GetComponent<BoxCollider>().size = new Vector3(440,318,0);

		}
		if(equip.is_enhance(m_equip))
		{
			effect.SetActive(true);
		}
		else
		{
			effect.SetActive(false);
		}
		if(equip.is_jinglian(m_equip))
		{
			m_jl_effect.SetActive(true);
		}
		else
		{
			m_jl_effect.SetActive(false);
		}
		if(t_equip.font_color == 5)
		{
			m_desc.GetComponent<UILabel>().text = equip.equip_skill(t_equip.type,m_equip.jilian);
		}
		reset_position (t_equip);
	}

	void reset_position(s_t_equip t_equip)
	{
		if(sys._instance.m_self.m_t_player.level < (int)e_open_see.es_equip_jinlian)
		{
			m_shenxing_sprite.SetActive(false);
			m_gaizao_sprite.SetActive(false);
			m_jl_sprite.SetActive(false);
			m_equip_skill_sprite.SetActive(false);
			m_tz_sprite.transform.localPosition = new Vector3(0,-28,0);
			if(t_equip.font_color == 5)
			{
				m_equip_skill_sprite.SetActive(true);
				m_equip_skill_sprite.transform.localPosition = new Vector3(0,-28,0);
				m_tz_sprite.transform.localPosition = new Vector3(0,-108-m_desc.GetComponent<UILabel>().height,0);
			}
		}
		else if(sys._instance.m_self.m_t_player.level < (int)e_open_see.es_equip_gaizao)
		{
			m_shenxing_sprite.SetActive(false);
			m_gaizao_sprite.SetActive(false);
			m_jl_sprite.SetActive(true);
			m_equip_skill_sprite.SetActive(false);
			m_tz_sprite.transform.localPosition = new Vector3(0,-168,0);
			if(t_equip.font_color == 5)
			{
				m_equip_skill_sprite.SetActive(true);
				m_equip_skill_sprite.transform.localPosition = new Vector3(0,-168,0);
				m_tz_sprite.transform.localPosition = new Vector3(0,-248-m_desc.GetComponent<UILabel>().height,0);
			}
		}
		else if(sys._instance.m_self.m_t_player.level < (int)e_open_see.es_equip_shengxing)
		{
			m_shenxing_sprite.SetActive(false);
			m_equip_skill_sprite.SetActive(false);
			m_gaizao_sprite.SetActive(true);
			m_jl_sprite.SetActive(true);
			m_tz_sprite.transform.localPosition = new Vector3(0,-367,0);
			if(t_equip.font_color >= 4)
			{
				if(t_equip.font_color == 5)
				{
					m_equip_skill_sprite.SetActive(true);
					m_equip_skill_sprite.transform.localPosition = new Vector3(0,-367,0);
					m_tz_sprite.transform.localPosition = new Vector3(0,-447-m_desc.GetComponent<UILabel>().height,0);
				}
			}
		}
		else
		{
			m_shenxing_sprite.SetActive(false);
			m_equip_skill_sprite.SetActive(false);
			m_gaizao_sprite.SetActive(true);
			m_jl_sprite.SetActive(true);
			m_tz_sprite.transform.localPosition = new Vector3(0,-367,0);
			if(t_equip.font_color >= 4)
			{
				m_shenxing_sprite.SetActive(true);
				m_shenxing_sprite.transform.localPosition = new Vector3(0,-367,0);
				m_tz_sprite.transform.localPosition = new Vector3(0,-508,0);
				if(t_equip.font_color == 5)
				{
					m_equip_skill_sprite.SetActive(true);
					m_equip_skill_sprite.transform.localPosition = new Vector3(0,-508,0);
					m_tz_sprite.transform.localPosition = new Vector3(0,-588-m_desc.GetComponent<UILabel>().height,0);
				}
			}
		}
	}

	public int get_tz_num(s_t_equip_tz t_equip_tz)
	{
		int num = 0;
		for(int i = 0; i < 4;++i)
		{
			m_xicon[i].transform.GetComponent<UISprite>().alpha = 0.5f;
		}
		if(m_flag)
		{
			for (int i = 0; i < m_t_equips.Count; ++i) 
			{
				for(int j =0; j < t_equip_tz.equip_ids.Count;++j)
				{
					if(m_t_equips[i].template_id == t_equip_tz.equip_ids[j])
					{
						m_xicon[j].transform.GetComponent<UISprite>().alpha = 1.0f;
						num ++;
						break;
					}
				}
			}
		}
		else
		{
			for(int j =0; j < t_equip_tz.equip_ids.Count;++j)
			{
				if(m_equip.template_id == t_equip_tz.equip_ids[j])
				{
					m_xicon[j].transform.GetComponent<UISprite>().alpha = 1.0f;
					break;
				}
			}
		}
		return num;
	}
	// Update is called once per frame
	void Update () 
	{
		
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_EQUIP_LOCK)
		{
			m_equip.locked = 1 - m_equip.locked;
			reset(m_equip, m_type);

			s_message msg = new s_message();
			msg.m_type = "show_equip_gui2";
			msg.m_long.Add(m_equip.guid);
			cmessage_center._instance.add_message(msg);
		}
	}
	void IMessage.message(s_message message)
	{

	}

	public void click(GameObject obj)
	{
		if (obj.name == "locked")
		{
			protocol.game.cmsg_equip_lock _msg = new protocol.game.cmsg_equip_lock ();
			_msg.equip_guid = m_equip.guid;
			net_http._instance.send_msg<protocol.game.cmsg_equip_lock> (opclient_t.CMSG_EQUIP_LOCK, _msg);
		}
		else if (obj.name == "ok")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "remove")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _msg = new s_message();
			_msg.m_type = "common_select_equip";
			_msg.m_long.Add ((ulong)0);
			cmessage_center._instance.add_message(_msg);
		}
		else if (obj.name == "qianghua")
		{
			s_message msg = new s_message();
			msg.m_type = "show_qianghua_gui";
			msg.m_long.Add(m_equip.guid);
			for(int i =0 ; i < m_t_equips.Count;++i)
			{
				msg.m_long.Add(m_t_equips[i].guid);
			}
			if (m_type == 0)
			{
				msg.m_string.Add("show_equip_gui2");
			}
			else if (m_type == 1)
			{
				msg.m_string.Add("show_buzheng2");
			}
			else if(m_type == 3)
			{
                msg.m_string.Add("show_bag1");
			}
			cmessage_center._instance.add_message(msg);

			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_equip_gui";
				cmessage_center._instance.add_message(msg1);
			}
			else if (m_type == 1)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_buzheng";
				cmessage_center._instance.add_message(msg1);
			}
			else if (m_type == 3)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_bag";
				cmessage_center._instance.add_message(msg1);
			}

			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "gaizao")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_gaizao)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("equip_detail.cs_448_73"), (int)e_open_level.el_gaizao));//装备改造{0}级开启
				return;
			}
			s_message msg = new s_message();
			msg.m_type = "show_gaizao_gui";
			msg.m_long.Add(m_equip.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_equip_gui2");
			}
			else if (m_type == 1)
			{
				msg.m_string.Add("show_buzheng2");
			}
			else if(m_type == 3)
			{
                msg.m_string.Add("show_bag1");
			}
			cmessage_center._instance.add_message(msg);
			
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_equip_gui";
				cmessage_center._instance.add_message(msg1);
			}
			else if (m_type == 3)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_bag";
				cmessage_center._instance.add_message(msg1);
			}
			else if (m_type == 1)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_buzheng";
				cmessage_center._instance.add_message(msg1);
			}
			
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "shengxing")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shengxing)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("equip_detail.cs_493_73") , (int)e_open_level.el_shengxing ));//装备升星{0}级开启
				return;
			}
			s_message msg = new s_message();
			msg.m_type = "show_equip_sx_gui";
			msg.m_long.Add(m_equip.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_equip_gui2");
			}
			else if (m_type == 1)
			{
				msg.m_string.Add("show_buzheng2");
			}
			else if(m_type == 3)
			{
                msg.m_string.Add("show_bag1");
			}
			cmessage_center._instance.add_message(msg);
			
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_equip_gui";
				cmessage_center._instance.add_message(msg1);
			}
			else if (m_type == 1)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_buzheng";
				cmessage_center._instance.add_message(msg1);
			}
			else if (m_type == 3)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_bag";
				cmessage_center._instance.add_message(msg1);
			}
			
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "jinglian")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_equip_jl)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("equip_detail.cs_538_73"), (int)e_open_level.el_equip_jl));//装备精炼{0}级开启
				return;
			}
			s_t_equip_jl _equip_jl_next = game_data._instance.get_t_equip_jl (m_equip.jilian +1);
			if(_equip_jl_next == null)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_detail.cs_544_59"));//该装备已精炼至满阶
				return;
			}
			s_message msg = new s_message();
			msg.m_type = "show_equip_jinglian_gui";
			msg.m_long.Add(m_equip.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_equip_gui2");
			}
			else if (m_type == 1)
			{
				msg.m_string.Add("show_buzheng2");
			}
			else if(m_type == 3)
			{
                msg.m_string.Add("show_bag1");
			}
			cmessage_center._instance.add_message(msg);
			
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_equip_gui";
				cmessage_center._instance.add_message(msg1);
			}
			else if (m_type == 1)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_buzheng";
				cmessage_center._instance.add_message(msg1);
			}
			else if (m_type == 3)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_bag";
				cmessage_center._instance.add_message(msg1);
			}
			
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "tihuan")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();

			s_message msg1 = new s_message();
			msg1.m_type = "common_equip_tihuan";
			cmessage_center._instance.add_message(msg1);
		}
	}
}
