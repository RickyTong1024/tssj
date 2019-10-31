
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class common_player_panel : MonoBehaviour {

	public protocol.game.smsg_player_look m_msg;
	public List<GameObject> m_cards = new List<GameObject>();
	public List<dhc.role_t> roles = new List<dhc.role_t>();
	public int m_select_card_id = 0;
	public int m_info_id = 0;
	public GameObject[] m_wq;
	public GameObject m_name;
	public GameObject m_level;
	public GameObject m_bf;
	public GameObject m_equip_button;
	public GameObject m_vip_num;
	public GameObject m_jt;
	public GameObject m_scro;
	public GameObject m_item;
	public GameObject m_down;
	public UILabel m_title;
	public UILabel m_type;


	public GameObject m_role_level;
	public GameObject m_role_jpic;
	public GameObject m_role_jlevel;
	public List<GameObject> m_tps;
	public GameObject m_role_pin;
	public GameObject m_role_hp;
	public GameObject m_role_at;
	public GameObject m_role_pd;
	public GameObject m_role_md;
	public GameObject m_role_sp;

	private  List<GameObject> m_items = new List<GameObject>();

	void OnEnable()
	{
		init ();
		m_equip_button.SetActive (true);
		m_equip_button.GetComponent<UISprite>().set_enable (false);
		m_equip_button.GetComponent<BoxCollider>().enabled = false;
	}

	public void init()
	{
		sys._instance.remove_child(m_scro);
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		if (m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		roles.Clear ();
		m_items.Clear ();
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(m_msg.achieves) + m_msg.name;
		m_level.GetComponent<UILabel>().text = m_msg.level.ToString();
		m_bf.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)m_msg.bf);
		if(m_msg.guild != "")
		{
			m_jt.GetComponent<UILabel>().text = m_msg.guild;
		}
		else
		{
			m_jt.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_gui.cs_1203_90");//暂无
		}
		int c = 0;
		for (int i = 0; i < m_msg.roles.Count; ++i)
		{
			if (m_msg.roles[i].guid == 0)
			{
				continue;
			}
			GameObject obj = Instantiate(m_item) as GameObject;
			obj.transform.parent = m_scro.transform;
			obj.transform.localPosition = new Vector3(-13,80-108*c,0);
			obj.transform.localScale = Vector3.one;
			obj.transform.name = c.ToString();
			GameObject _icon = icon_manager._instance.create_card_icon_ex(m_msg.roles[i].template_id, m_msg.roles[i].glevel, m_msg.roles[i].jlevel, m_msg.roles[i].level);
			_icon.transform.name = c.ToString();
			_icon.transform.parent = obj.transform;
			_icon.transform.localPosition = new Vector3(0,0,0);
			_icon.transform.localScale = new Vector3(1,1,1);
			_icon.AddComponent<UIDragScrollView>();
			UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click_card_icon";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			roles.Add(m_msg.roles[i]);
			obj.SetActive(true);
			m_items.Add(obj);
			c++;
		}

		if (m_msg.pets[m_msg.pets.Count -1].guid != 0)
		{
			GameObject obj = Instantiate(m_item) as GameObject;
			obj.transform.parent = m_scro.transform;
			obj.transform.localPosition = new Vector3(-13,80-108*c,0);
			obj.transform.localScale = Vector3.one;
			obj.transform.name = "6";
			GameObject _icon = icon_manager._instance.create_pet_icon_ex(m_msg.pets[m_msg.pets.Count -1].template_id,
			                                                             m_msg.pets[m_msg.pets.Count -1].star,m_msg.pets[m_msg.pets.Count -1].jinjie,m_msg.pets[m_msg.pets.Count -1].level);
			_icon.transform.name = "6";
			_icon.transform.parent = obj.transform;
			_icon.transform.localPosition = new Vector3(0,0,0);
			_icon.transform.localScale = new Vector3(1,1,1);
			_icon.AddComponent<UIDragScrollView>();
			UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "click_pet_icon";
			meses[1].target = null;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			obj.SetActive(true);
			m_items.Add(obj);
			c++;
		}
		if(c >= 3)
		{
			m_down.SetActive(true);
		}
		else
		{
			m_down.SetActive(false);
		}

        if (platform_config_common.m_vip == 0)
        {
            m_vip_num.gameObject.SetActive(false);
        }
        else
        {
            m_vip_num.gameObject.SetActive(true);
        }
        s_t_vip t_vip = game_data._instance.get_t_vip(m_msg.vip);
        m_vip_num.GetComponent<UILabel>().text = t_vip.desc;
		select_info(0);
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void click_card_icon(GameObject obj)
	{
		int id = int.Parse (obj.transform.name);
		if (id == m_select_card_id)
		{
			return;
		}
		m_select_card_id = id;
		m_equip_button.SetActive(true);
		select_info (m_select_card_id);
	}

	public void click_pet_icon(GameObject obj)
	{
		int id = 6;
		if (id == m_select_card_id)
		{
			return;
		}
		m_select_card_id = 6;
		m_equip_button.SetActive(true);
		select_pet (m_select_card_id);
	}

	public void star (int glevel)
	{
		for(int i = 0 ; i < 5;++i)
		{
			m_tps[i].SetActive(false);
		}
		for(int i = 0; i < 5;i ++)
		{
			if(i < glevel)
			{
				m_tps[i].GetComponent<UISprite>().spriteName = "xstar_002d";
				m_tps[i].gameObject.SetActive(true);
			}
			else
			{
				m_tps[i].gameObject.SetActive(false);
			}
			
			if(i + 5 < glevel)
			{
				m_tps[i].GetComponent<UISprite>().spriteName = "xstar_003d";
			}
			
			if(i + 10 < glevel)
			{
				m_tps[i].GetComponent<UISprite>().spriteName = "xstar_001d";
			}
		}
	}

	public void select_info(int id)
	{
		m_title.text = game_data._instance.get_t_language ("common_player_panel.cs_223_17");//伙伴属性
		m_info_id = id;
		dhc.role_t role = roles[id];
		dhc.pet_t m_pet = m_msg.pets [id];
		if(m_msg.pets[id].guid == 0)
		{
			m_pet = null;
		}
		m_role_level.GetComponent<UILabel>().text = role.level.ToString();
		s_t_jinjie t_jinjie = game_data._instance.get_jinjie (role.jlevel);
		m_role_jlevel.GetComponent<UILabel>().text = t_jinjie.name;
		m_role_jpic.GetComponent<UISprite>().spriteName = t_jinjie.icon;
		m_role_jpic.GetComponent<UISprite>().MakePixelPerfect();
		star (role.glevel);
		m_role_pin.SetActive (true);
		m_type.text = game_data._instance.get_t_language ("common_player_panel.cs_238_16");//伙伴等级 lv
		s_t_role_shengpin t_shengpin = game_data._instance.get_t_role_shengpin (role.pinzhi);
		m_role_pin.GetComponent<UILabel>().text = game_data._instance.get_name_color (t_shengpin.color) + t_shengpin.name;
		m_role_hp.GetComponent<UILabel>().text = m_msg.roles_sx[id * 5].ToString();
		m_role_at.GetComponent<UILabel>().text = m_msg.roles_sx[id * 5 + 1].ToString();
		m_role_pd.GetComponent<UILabel>().text = m_msg.roles_sx[id * 5 + 2].ToString();
		m_role_md.GetComponent<UILabel>().text = m_msg.roles_sx[id * 5 + 3].ToString();
		m_role_sp.GetComponent<UILabel>().text = m_msg.roles_sx[id * 5 + 4].ToString();
		m_role_sp.transform.parent.gameObject.SetActive (true);
		for (int i = 0; i < 6; ++i)
		{
			m_wq[i].SetActive(false);
			sys._instance.remove_child(m_wq[i]);
		}
		int c = 0;

		for(int i = id *4 ; i < id*4 + 4; ++i)
		{
			if (m_msg.equips[i].guid == 0)
			{
				c++;
				continue;
			}
			m_wq[c].SetActive(true);
			GameObject _icon = icon_manager._instance.create_equip_icon(m_msg.equips[i]);
			_icon.transform.parent = m_wq[c].transform;
			_icon.transform.localPosition = new Vector3(0,0,0);
			_icon.transform.localScale = new Vector3(1,1,1);
			_icon.GetComponent<BoxCollider>().enabled = false;
			c++;
		}
		for(int i = id *2 ; i < id*2 + 2; ++i)
		{
			if (m_msg.treasures[i].guid == 0)
			{
				c++;
				continue;
			}
			m_wq[c].SetActive(true);
			GameObject _icon = icon_manager._instance.create_treasure_icon(m_msg.treasures[i]);
			_icon.transform.parent = m_wq[c].transform;
			_icon.transform.localPosition = new Vector3(0,0,0);
			_icon.transform.localScale = new Vector3(1,1,1);
			_icon.GetComponent<BoxCollider>().enabled = false;
			c++;
		}

		ccard _card = new ccard ();
		_card.set_role (role);
		pet _pet = new pet ();
		if(m_pet != null)
		{
			_pet.set_pet (m_pet);
		}
		else
		{
			_pet = null;
		}
        s_message mes = new s_message();
        mes.m_type = "show_ui_unit_cam";
        mes.m_object.Add(new Vector3(0, 0.8f, 8));
        mes.m_object.Add(new Vector3(0, 180, 0));
        mes.m_object.Add(_card);
		mes.m_object.Add(_pet);
        cmessage_center._instance.add_message(mes);
 
		for(int i = 0; i < m_items.Count;++i)
		{
			if(int.Parse(m_items[i].name) == id)
			{
				m_items[i].transform.Find("effect").gameObject.SetActive(true);
			}
			else
			{
				m_items[i].transform.Find("effect").gameObject.SetActive(false);
			}
		}
	}

	public void select_pet(int id)
	{
		m_title.text = game_data._instance.get_t_language ("common_player_panel.cs_321_17");//宠物属性
		m_type.text = game_data._instance.get_t_language ("common_player_panel.cs_322_16");//宠物等级 lv
		m_info_id = id;
		m_role_level.GetComponent<UILabel>().text = m_msg.pets[id].level.ToString();
		s_t_pet_jinjie t_jinjie = game_data._instance.get_t_pet_jinjie (m_msg.pets[id].jinjie);
		m_role_jlevel.GetComponent<UILabel>().text = t_jinjie.chenghao;
		m_role_jpic.GetComponent<UISprite>().spriteName = t_jinjie.icon;
		m_role_jpic.GetComponent<UISprite>().MakePixelPerfect();
		m_role_pin.SetActive (false);
		m_role_hp.GetComponent<UILabel>().text = m_msg.pets_sx[4*id].ToString();
		m_role_at.GetComponent<UILabel>().text = m_msg.pets_sx[4*id + 1].ToString();
		m_role_pd.GetComponent<UILabel>().text = m_msg.pets_sx[4*id + 2].ToString();
		m_role_md.GetComponent<UILabel>().text = m_msg.pets_sx[4*id +3].ToString();
		m_role_sp.transform.parent.gameObject.SetActive (false);
		for(int i = 0 ; i < 5;++i)
		{
			m_tps[i].SetActive(false);
		}
		for(int i = 0 ; i < m_msg.pets[id].star;++i)
		{
			m_tps[i].SetActive(true);
			m_tps[i].GetComponent<UISprite>().spriteName = "xstar_003d";
		}
		for (int i = 0; i < 6; ++i)
		{
			m_wq[i].SetActive(false);
			sys._instance.remove_child(m_wq[i]);
		}

		pet _pet = new pet ();
		_pet.set_pet (m_msg.pets[id]);

        s_message mes = new s_message();
		mes.m_type = "show_ui_pet_cam";
        mes.m_object.Add(new Vector3(0, 1.5f, 16));
        mes.m_object.Add(new Vector3(0, 180, 0));
        mes.m_object.Add(_pet);
		cmessage_center._instance.add_message(mes);
	
		for(int i = 0; i < m_items.Count;++i)
		{
			if(int.Parse(m_items[i].name) == id)
			{
				m_items[i].transform.Find("effect").gameObject.SetActive(true);
			}
			else
			{
				m_items[i].transform.Find("effect").gameObject.SetActive(false);
			}
		}
	}

    public void close()
    {
        s_message mes = new s_message();
        mes.m_type = "hide_ui_unit_cam";
        cmessage_center._instance.add_message(mes);

        m_select_card_id = 0;
   
        this.gameObject.SetActive(false);
        s_message _mes = new s_message();
        _mes.m_type = "bingyuan_canmove";
        cmessage_center._instance.add_message(_mes);
    }
	public void click(GameObject obj)
	{
		if(obj.name == "close")
		{
			s_message mes = new s_message ();
			mes.m_type = "hide_ui_unit_cam";
			cmessage_center._instance.add_message (mes);

			m_select_card_id = 0;
			this.gameObject.SetActive(false);
            s_message _mes = new s_message();
            _mes.m_type = "bingyuan_canmove";
            cmessage_center._instance.add_message(_mes);
		}
		if(obj.name == "role")
		{
			s_message mes = new s_message ();
			mes.m_type = "action_ui_unit_cam";
			cmessage_center._instance.add_message (mes);
		}
	}
}
