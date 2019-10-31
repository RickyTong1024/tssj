
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pet_jingjie_gui : MonoBehaviour,IMessage{

	public ulong m_guid;
	public UILabel m_cur_level;
	public UILabel m_next_level;
	public UILabel m_cur_hp;
	public UILabel m_cur_attack;
	public UILabel m_cur_wf;
	public UILabel m_cur_mf;
	public GameObject m_cur_reward;
	public UILabel m_next_hp;
	public UILabel m_next_attack;
	public UILabel m_next_wf;
	public UILabel m_next_mf;
	public GameObject m_next_reward;
	public GameObject m_item_icon;
	public List<GameObject> m_icons = new List<GameObject>();
	public List<UILabel> m_tishi = new List<UILabel>();
	public List<GameObject> m_nulls = new List<GameObject>();
	public List<GameObject> m_ups = new List<GameObject>();
	public UILabel m_sx_add;
	public UILabel m_gold;
	public UILabel m_level;
	public string m_message = "";

	public GameObject m_equip_gui;
	public GameObject m_equip_icon;
	public UILabel m_equip_name;
	public UILabel m_equip_num;
	public UILabel m_equip_Label;
	public GameObject m_button;
	public GameObject m_show_Label;
	public List<UILabel> attrs = new List<UILabel>();
	public List<UILabel> values = new List<UILabel>();

	private int select_id = 0;
	private List<int> m_ids = new List<int>();
	private dhc.pet_t m_t_pet;
	private pet m_pet;
	public static bool is_jj = false;
	// Use this for initialization
	void Start () {
		
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void reset(ulong guid)
	{
		m_guid = guid;
		m_pet = sys._instance.m_self.get_pet_guid (m_guid);
		m_t_pet = m_pet.get_pet ();
		update_ui ();
	}

	public void update_ui()
	{
		m_ids.Clear ();
		m_cur_hp.text = m_pet.get_pet_jingjie_attr(1,m_t_pet.jinjie).ToString("f0");
		m_cur_attack.text = m_pet.get_pet_jingjie_attr(2,m_t_pet.jinjie).ToString("f0");
		m_cur_wf.text = m_pet.get_pet_jingjie_attr(3,m_t_pet.jinjie).ToString("f0");
		m_cur_mf.text = m_pet.get_pet_jingjie_attr(4,m_t_pet.jinjie).ToString("f0");
		s_t_pet_jinjie cur_jinjie = game_data._instance.get_t_pet_jinjie (m_t_pet.jinjie);
		s_t_pet_jinjie next_jinjie = game_data._instance.get_t_pet_jinjie (m_t_pet.jinjie+1);
		m_cur_level.text = cur_jinjie.chenghao;
		if(cur_jinjie.qsx_add > 0)
		{
			m_cur_reward.transform.parent.gameObject.SetActive(true);
			m_cur_reward.transform.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_77_57") + cur_jinjie.qsx_add + "%";//全属性 +
		}
		else if(cur_jinjie.esx_add > 0)
		{
			m_cur_reward.transform.parent.gameObject.SetActive(true);
			m_cur_reward.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("pet_jingjie_gui.cs_82_71"),game_data._instance.get_t_value(m_pet.m_t_pet.jinjie_add_sx[cur_jinjie.esx_add-1].attr).name,//{0}额外增加{1}
			                                                                    m_pet.m_t_pet.jinjie_add_sx[cur_jinjie.esx_add-1].value);
		}
		else
		{
			m_cur_reward.transform.parent.gameObject.SetActive(false);
		}
		if(next_jinjie == null)
		{
			m_gold.text = "----";
			m_next_hp.text = m_pet.get_pet_jingjie_attr(1,m_t_pet.jinjie).ToString("f0");
			m_next_attack.text = m_pet.get_pet_jingjie_attr(2,m_t_pet.jinjie).ToString("f0");
			m_next_wf.text = m_pet.get_pet_jingjie_attr(3,m_t_pet.jinjie).ToString("f0");
			m_next_mf.text = m_pet.get_pet_jingjie_attr(4,m_t_pet.jinjie).ToString("f0");
			m_next_level.text = cur_jinjie.chenghao;
			for(int i = 0; i < m_ups.Count;++i)
			{
				m_ups[i].SetActive(false);
			}
			m_level.GetComponent<UILabel>().text = "----";
			m_sx_add.text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_102_19");//[b3fe13]该宠物已进阶满级
			for(int i = 0;i <m_icons.Count;++i)
			{
				sys._instance.remove_child(m_icons[i]);
				m_icons[i].transform.GetComponent<BoxCollider>().enabled = false;
				m_nulls[i].SetActive(false);
				m_tishi[i].text = "";
			}
			if(cur_jinjie.qsx_add > 0)
			{
				m_next_reward.transform.parent.gameObject.SetActive(true);
				m_next_reward.transform.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_77_57") + cur_jinjie.qsx_add + "%";//全属性 +
			}
			else if(cur_jinjie.esx_add > 0)
			{
				m_next_reward.transform.parent.gameObject.SetActive(true);
				m_next_reward.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("pet_jingjie_gui.cs_82_71"),game_data._instance.get_t_value( m_pet.m_t_pet.jinjie_add_sx[cur_jinjie.esx_add-1].attr).name,//{0}额外增加{1}
				                                                                     m_pet.m_t_pet.jinjie_add_sx[cur_jinjie.esx_add-1].value);
			}
			else
			{
				m_next_reward.transform.parent.gameObject.SetActive(false);
			}
		}
		else
		{
			int gold = cur_jinjie.gold;
			m_next_hp.text = m_pet.get_pet_jingjie_attr(1,m_t_pet.jinjie+1).ToString("f0");
			m_next_attack.text = m_pet.get_pet_jingjie_attr(2,m_t_pet.jinjie+1).ToString("f0");
			m_next_wf.text = m_pet.get_pet_jingjie_attr(3,m_t_pet.jinjie+1).ToString("f0");
			m_next_mf.text = m_pet.get_pet_jingjie_attr(4,m_t_pet.jinjie+1).ToString("f0");
			m_next_level.text = next_jinjie.chenghao;
			for(int i = 0; i < m_ups.Count;++i)
			{
				m_ups[i].SetActive(true);
			}
			if(sys._instance.m_self.m_t_player.gold > gold)
			{
				m_gold.text = sys._instance.get_res_color(1) +"x"+gold;
			}
			else
			{
				m_gold.text = "[ff0000]x" + gold;
			}
			if(m_t_pet.level < cur_jinjie.need_level)
			{
				m_level.GetComponent<UILabel>().text = "[ff0000]" +cur_jinjie.need_level.ToString();
			}
			else
			{
				m_level.GetComponent<UILabel>().text = "[0aff16]" + cur_jinjie.need_level.ToString();
			}
			if(next_jinjie.qsx_add > 0)
			{
				m_next_reward.transform.parent.gameObject.SetActive(true);
				m_next_reward.transform.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_77_57") + next_jinjie.qsx_add + "%";//全属性 +
			}
			else if(next_jinjie.esx_add > 0)
			{
				m_next_reward.transform.parent.gameObject.SetActive(true);
				m_next_reward.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("pet_jingjie_gui.cs_82_71"),game_data._instance.get_t_value(m_pet.m_t_pet.jinjie_add_sx[next_jinjie.esx_add-1].attr).name//{0}额外增加{1}
				                                                                     ,m_pet.m_t_pet.jinjie_add_sx[next_jinjie.esx_add-1].value);
			}
			else
			{
				m_next_reward.transform.parent.gameObject.SetActive(false);
			}
			for(int i = 0;i <m_icons.Count;++i)
			{
				sys._instance.remove_child(m_icons[i]);
				m_icons[i].transform.GetComponent<BoxCollider>().enabled = false;
				m_nulls[i].SetActive(false);
				m_tishi[i].text = "";
			}
			for(int i = 0; i < cur_jinjie.cls.Count;++i)
			{
				m_ids.Add(0);
			}
			for(int i = 0 ; i < m_t_pet.jinjie_slot.Count && i < m_ids.Count;++i)
			{
				m_ids[i] = m_t_pet.jinjie_slot[i];
			}
			for(int i = m_t_pet.jinjie +1;i < game_data._instance.m_dbc_chongwu_jinjie.get_y();++i)
			{
				s_t_pet_jinjie t_pet_jinjie = game_data._instance.get_t_pet_jinjie(i); 
				if(t_pet_jinjie.esx_add > 0)
				{
					m_sx_add.text = string.Format(game_data._instance.get_t_language ("pet_jingjie_gui.cs_189_35"),t_pet_jinjie.chenghao, //[0aabff]进阶至{0}{1}额外增加[-][0aff16]{2}[-]
					                              game_data._instance.get_t_value(m_pet.m_t_pet.jinjie_add_sx[t_pet_jinjie.esx_add-1].attr).name,m_pet.m_t_pet.jinjie_add_sx[t_pet_jinjie.esx_add-1].value);
					break;
				}
				if(t_pet_jinjie.qsx_add > 0)
				{
					m_sx_add.text = string.Format(game_data._instance.get_t_language ("pet_jingjie_gui.cs_195_35"),t_pet_jinjie.chenghao, t_pet_jinjie.qsx_add);//[0aabff]进阶至{0}全属性 [0aff16]+{1}%[-]
					break;
				}
			}
			update_icon();
		}
	}

	void update_icon()
	{
		s_t_pet_jinjie next_jinjie = game_data._instance.get_t_pet_jinjie (m_t_pet.jinjie);
		for(int i = 0; i < m_ids.Count && i < m_icons.Count;++i)
		{
			sys._instance.remove_child(m_icons[i]);
			m_icons[i].transform.GetComponent<BoxCollider>().enabled = true;
			if(m_ids[i] > 0)
			{
				m_tishi[i].text = "";
				m_nulls[i].SetActive(false);
				sys._instance.remove_child(m_icons[i]);
				m_nulls[i].SetActive(false);
				GameObject _icon = icon_manager._instance.create_item_icon_ex(m_ids[i]);
				_icon.transform.parent = m_icons[i].transform;
				_icon.transform.localPosition = new Vector3(0,0,0);
				_icon.transform.localScale = new Vector3(1,1,1);
				_icon.transform.GetComponent<BoxCollider>().enabled = true;
				UIButtonMessage[] message = _icon.transform.GetComponents<UIButtonMessage>();
				message[0].target = this.gameObject;
				message[0].functionName = "click_icon";
				message[1].target = null;
				message[1].functionName = "";
				message[2].target = null;
				message[2].functionName = "";
			}
			else
			{
				s_t_item t_item = game_data._instance.get_item(next_jinjie.cls[i]);
				m_nulls[i].SetActive(true);
				sys._instance.remove_child(m_icons[i]);
				GameObject obj = Instantiate(m_item_icon) as GameObject;
				string name = t_item.icon;
				obj.transform.GetComponent<UISprite>().spriteName = name.Split('_')[0] + "_001";
				obj.transform.parent = m_icons[i].transform;
				obj.transform.localPosition = new Vector3(0,0,0);
				obj.transform.localScale = new Vector3(1,1,1);
				obj.SetActive(true);

			}
		}
		for(int i = 0; i < next_jinjie.cls.Count;++i)
		{
			if(m_ids[i] > 0)
			{
				continue;
			}
			s_t_itemhecheng t_itemhecheng = game_data._instance.get_t_pet_itemhecheng(next_jinjie.cls[i]);
			int num = sys._instance.m_self.get_item_num((uint)next_jinjie.cls[i]);
			if(num > 0)
			{
				m_tishi[i].text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_254_22");//[0aff16]可装备
			}
			else
			{
				if(t_itemhecheng == null)
				{
					if(num <= 0)
					{
						m_tishi[i].text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_262_24");//[9d7e51]无道具
						m_icons[i].transform.GetComponent<BoxCollider>().enabled = true;
						m_nulls[i].SetActive(false);
					}
				}
				else
				{
					if(is_hecheng(next_jinjie.cls[i]))
					{
						m_tishi[i].text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_271_24");//[0aff16]可合成
						m_icons[i].transform.GetComponent<BoxCollider>().enabled = true;
						m_nulls[i].SetActive(true);
					}
					else
					{
						m_tishi[i].text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_262_24");//[9d7e51]无道具
						m_icons[i].transform.GetComponent<BoxCollider>().enabled = true;
						m_nulls[i].SetActive(false);
					}
				}
			}
		}
	}

	public bool is_hecheng(int id)
	{
		s_t_itemhecheng t_itemhecheng = game_data._instance.get_t_pet_itemhecheng(id);
		if(t_itemhecheng == null)
		{
			return false;
		}
		for(int i = 0; i < t_itemhecheng.cl_type.Count;++i)
		{
			int cl_num = sys._instance.m_self.get_item_num((uint)t_itemhecheng.cl_id[i]);
			if(cl_num < t_itemhecheng.cl_num[i])
			{
				return false;
			}
		}
		return true;
	}

	public void click_icon(GameObject obj)
	{
		int id = obj.transform.GetComponent<item_icon>().m_item_id;
		equip(id);
		m_equip_gui.SetActive(true);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "equip")
		{
			is_jj = false;
			s_t_pet_jinjie next_jinjie = game_data._instance.get_t_pet_jinjie (m_t_pet.jinjie);
			protocol.game.cmsg_pet_jinjie _msg = new protocol.game.cmsg_pet_jinjie ();
			_msg.guid = m_pet.get_guid();
			_msg.index = select_id;
			_msg.item_id = next_jinjie.cls[select_id];
			net_http._instance.send_msg<protocol.game.cmsg_pet_jinjie> (opclient_t.CMSG_PET_JINJIE, _msg);
		}
		else if(obj.transform.name == "guanbi")
		{
			m_equip_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.transform.name == "sj")
		{
			s_t_pet_jinjie next_jinjie = game_data._instance.get_t_pet_jinjie (m_t_pet.jinjie);
			s_t_pet_jinjie _jinjie = game_data._instance.get_t_pet_jinjie (m_t_pet.jinjie+1);
			if(_jinjie == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_jingjie_gui.cs_333_46"));//[ffc882]该宠物已进阶满级
				return;
			}
			for(int i = 0; i < next_jinjie.cls.Count;++i)
			{
				if(m_t_pet.jinjie_slot[i] <= 0)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_jingjie_gui.cs_340_47"));//[ffc882]未装备全部进阶材料
					return;
				}
			}
			if(next_jinjie.gold > sys._instance.m_self.m_t_player.gold)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_jingjie_gui.cs_346_46"));//[ffc882]金币不足
				return;
			}
			if(next_jinjie.need_level > m_pet.get_level())
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pet_jingjie_gui.cs_351_46"));//[ffc882]等级不足
				return;
			}
			is_jj = true;
			protocol.game.cmsg_pet_jinjie _msg = new protocol.game.cmsg_pet_jinjie ();
			_msg.guid = m_pet.get_guid();
			_msg.index = 0;
			_msg.item_id = 0;
			net_http._instance.send_msg<protocol.game.cmsg_pet_jinjie> (opclient_t.CMSG_PET_JINJIE, _msg);
		}
		else if(obj.transform.name == "close")
		{
			s_message _message = new s_message ();
			_message.m_type = m_message;
			cmessage_center._instance.add_message (_message);
			this.transform.GetComponent<ui_show_anim>().hide_ui();
		}
	}

	public void select(GameObject obj)
	{
		select_id = int.Parse (obj.transform.name);
		if(m_ids[select_id] > 0)
		{
			equip(m_ids[select_id]);
			m_equip_gui.SetActive(true);
		}
		else
		{
			s_t_pet_jinjie next_jinjie = game_data._instance.get_t_pet_jinjie (m_t_pet.jinjie);
			s_t_itemhecheng t_itemhecheng = game_data._instance.get_t_pet_itemhecheng(next_jinjie.cls[select_id]);
			int num = sys._instance.m_self.get_item_num((uint)next_jinjie.cls[select_id]);
			if(num > 0)
			{
				equip(next_jinjie.cls[select_id]);
				m_equip_gui.SetActive(true);
			}
			else
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)next_jinjie.cls[select_id]);
				cmessage_center._instance.add_message(message);
			}
		}
	}
	
	void equip(int id)
	{
		m_equip_name.text = sys._instance.m_self.get_name (2,id,0,0);
		m_equip_num.text = game_data._instance.get_t_language ("pet_jingjie_gui.cs_401_21") +" "+ sys._instance.m_self.get_item_num ((uint)id);//当前拥有:
		s_t_pet_jinjieitem t_jinjieitem = game_data._instance.get_t_pet_jinjieitem (id);
		for(int i = 0; i < t_jinjieitem.attrs.Count && i < attrs.Count;++i)
		{
			attrs[i].text = game_data._instance.get_t_value(t_jinjieitem.attrs[i].attr).name;
			values[i].text = t_jinjieitem.attrs[i].value.ToString();
		}
		sys._instance.remove_child (m_equip_icon);
		GameObject icon = icon_manager._instance.create_item_icon(id);
		Transform iicon = m_equip_icon.transform;
		icon.transform.parent = iicon;
		icon.transform.localPosition = new Vector3(0,0,0);
		icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		icon.GetComponent<BoxCollider>().enabled = false;
		if(m_ids.Contains(id))
		{
			m_button.name = "guanbi";
			m_equip_Label.text = game_data._instance.get_t_language ("jt_mobai.cs_217_99");//关闭
		}
		else
		{
			m_button.name = "equip";
			m_equip_Label.text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2956_35");//装备
		}
	}
	
	void IMessage.message(s_message message)
	{

	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_PET_JINJIE)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if (_msg.success)
			{
				s_t_pet_jinjie next_jinjie = game_data._instance.get_t_pet_jinjie (m_t_pet.jinjie);
				if(!is_jj)
				{
					sys._instance.m_self.remove_item((uint)next_jinjie.cls[select_id],1,game_data._instance.get_t_language ("pet_jingjie_gui.cs_442_73"));//宠物进阶消耗
					m_t_pet.jinjie_slot[select_id] = next_jinjie.cls[select_id];
					m_equip_gui.transform.Find("frame_big").GetComponent<frame>().hide();
				}
				else
				{
					m_t_pet.jinjie += 1;
					for(int i = 0; i < m_t_pet.jinjie_slot.Count;++i)
					{
						m_t_pet.jinjie_slot[i] = 0;
					}
					sys._instance.m_self.sub_att(e_player_attr.player_gold,next_jinjie.gold,game_data._instance.get_t_language ("pet_jingjie_gui.cs_442_73"));//宠物进阶消耗
					m_show_Label.SetActive(true);
					Label_time();
				}
				s_message _message3 = new s_message();
				_message3.m_type = "check_bf";
				cmessage_center._instance.add_message(_message3);
				update_ui();
			}
		}
	}

	void Label_time()
	{
		hide_time _hide = m_show_Label.GetComponent<hide_time>();
		
		if(_hide == null)
		{
			_hide = m_show_Label.AddComponent<hide_time>();
		}
		
		_hide.m_time = 0.3f;
	}

	public static bool is_jingjie(pet m_pet)
	{
		if(m_pet == null)
		{
			return false;
		}
		s_t_pet_jinjie t_jinjie = game_data._instance.get_t_pet_jinjie (m_pet.get_jlevel ()+1);
		if(t_jinjie == null)
		{
			return false;
		}
		t_jinjie = game_data._instance.get_t_pet_jinjie (m_pet.get_jlevel ());
		bool flag = true;
		for(int i = 0; i < t_jinjie.cls.Count;++i)
		{
			if(m_pet.get_pet().jinjie_slot.Contains(t_jinjie.cls[i]))
			{
				continue;
			}
			flag = false;
			int num = sys._instance.m_self.get_item_num((uint)t_jinjie.cls[i]);
			if(num > 0)
			{
				return true;
			}
		}
		if(flag)
		{
			if(m_pet.get_level()>= t_jinjie.need_level && sys._instance.m_self.m_t_player.gold >= t_jinjie.gold)
			{
				return true;
			}
		}
		return false;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
