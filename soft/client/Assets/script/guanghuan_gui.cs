
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class guanghuan_gui : MonoBehaviour,IMessage{

	public ccard m_card;
	public GameObject m_scro;
	public GameObject m_view;
	public GameObject m_button_Label;
	public GameObject m_name;
	public GameObject m_type;
	public GameObject m_sx;
	public GameObject m_jn;
	public GameObject m_guanghuan;
	public GameObject m_up_gui;
	public GameObject m_tj_gui;
	public GameObject m_qh;
	public GameObject m_cs;
	public GameObject m_skill_attr1;
	public GameObject m_skill_attr1_up;
	public GameObject m_dacheng_des;
	private List<GameObject> m_objs = new List<GameObject>();
	private int index = 0;
	private uint cl_id = 50150001;
	private List<s_t_guanghuan> t_guanghuans = new List<s_t_guanghuan>();
	public static Dictionary<int,double> sxs = new Dictionary<int,double>();
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		index = 0;
		sxs = sx ();
		reset ();
	}

	public void reset(bool flag = false)
	{
		m_objs.Clear ();
		t_guanghuans.Clear ();
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3 (0,0,0);
		m_scro.transform.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
		sys._instance.remove_child (m_scro);
		foreach(int id in game_data._instance.m_dbc_guanghuan.m_index.Keys)
		{
			s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan(id);
			t_guanghuans.Add(t_guanghuan);
		}
		t_guanghuans.Sort (com);
		for(int i = 0; i < t_guanghuans.Count;++i)
		{
			GameObject icon = Instantiate(m_guanghuan) as GameObject;
			icon.transform.Find("bg").GetComponent<UISprite>().spriteName = kuang(t_guanghuans[i]);
			if(guanghuan_level(t_guanghuans[i]) == 0)
			{
				icon.transform.Find("level").gameObject.SetActive(false);
			}
			else
			{
				icon.transform.Find("level").gameObject.SetActive(true);
				icon.transform.Find("level").GetComponent<UILabel>().text = "+" + guanghuan_level(t_guanghuans[i]);
			}
			icon.transform.parent = m_scro.transform;
			icon.transform.localPosition = new Vector3(82,149 - 95*i,0);
			icon.transform.localScale = new Vector3(1,1,1);
			icon.transform.name = i.ToString();
			icon.transform.Find("icon").transform.GetComponent<UISprite>().spriteName = t_guanghuans[i].icon;
			if(on_guanghuan(t_guanghuans[i]))
			{
				if(!flag)
				{
					index = i;
				}
				icon.transform.Find("equip").gameObject.SetActive(true);
			}
			else
			{
				icon.transform.Find("equip").gameObject.SetActive(false);
			}
			if(has_guanghuan(t_guanghuans[i]))
			{
				icon.transform.Find("lock").gameObject.SetActive(false);
			}
			else
			{
				icon.transform.Find("lock").gameObject.SetActive(true);
			}
			icon.transform.Find("select").gameObject.SetActive(false);
			icon.SetActive(true);
			m_objs.Add(icon);
		}
		m_objs [index].transform.Find("select").gameObject.SetActive (true);
		update_ui (index);
	}

	string kuang(s_t_guanghuan m_guanghuan)
	{
		string s = "";
		if (m_guanghuan.color == 1)
		{
			s = "xtbk_lvpt001";
		}
		else if (m_guanghuan.color == 2)
		{
			s = "xtbk_lanpt001";
		}
		else if (m_guanghuan.color == 3)
		{
			s = "xtbk_zipt001";
		}
		else if (m_guanghuan.color == 4)
		{
			s = "xtbk_chpt001";
		}
		else if (m_guanghuan.color == 5)
		{
			s = "xtbk_hopt001";
		}
		else if (m_guanghuan.color == 6)
		{
			s = "xtbk_jinpt001";
		}
		return s;
	}

	public void update_ui(int id)
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		if(on_guanghuan(t_guanghuans[id]))
		{
			m_button_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language ("guanghuan_gui.cs_149_49");//卸下
			m_button_Label.transform.parent.GetComponent<BoxCollider>().enabled = true;
			m_button_Label.transform.parent.GetComponent<UISprite>().set_enable(true);
		}
		else if(!on_guanghuan(t_guanghuans[id]) && has_guanghuan(t_guanghuans[id]))
		{
			m_button_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2956_35");//装备
			m_button_Label.transform.parent.GetComponent<BoxCollider>().enabled = true;
			m_button_Label.transform.parent.GetComponent<UISprite>().set_enable(true);
		}
		else
		{
			m_button_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2956_35");//装备
			m_button_Label.transform.parent.GetComponent<BoxCollider>().enabled = false;
			m_button_Label.transform.parent.GetComponent<UISprite>().set_enable(false);
		}
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(t_guanghuans[id].color) + t_guanghuans [id].name;
		if(t_guanghuans[id].attr1 == 1)
		{
			m_type.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2668_51");//生命型
		}
		else if(t_guanghuans[id].attr1 == 2)
		{
			m_type.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2672_51");//攻击型
		}
		string text = "";
		string[] _text = get_attr (t_guanghuans [id].id, guanghuan_level (t_guanghuans [id]), 0).Split('+');
		text += (_text[0] + "   +" + _text[1]);
		if(t_guanghuans[id].attr2 > 0)
		{
			_text = get_attr (t_guanghuans [id].id, guanghuan_level (t_guanghuans [id]), 1).Split('+');
			text += "\n" + (_text[0] + "   +" + _text[1]);
		}
		m_sx.GetComponent<UILabel>().text = text;
		List <s_t_guanghuan_skill> t_guanghuan_skills = new List<s_t_guanghuan_skill>();
		foreach(int _id in game_data._instance.m_dbc_guanghuan_skill.m_index.Keys)
		{
			s_t_guanghuan_skill t_guanghuan_skill = game_data._instance.get_t_guanghuan_skill(_id);
			if(t_guanghuan_skill.wing_id == t_guanghuans[id].id)
			{
				t_guanghuan_skills.Add(t_guanghuan_skill);
			}
		}
		string zh = "";
		for(int i = 0; i < t_guanghuan_skills.Count;++i)
		{
			zh += get_skill_des(t_guanghuan_skills[i],zh == "");
		}
		m_jn.GetComponent<UILabel>().text = zh;
		if(zh == "")
		{
			m_jn.GetComponent<UILabel>().text = "[FFFF00]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_2702_63");//无技能
		}
		if(has_guanghuan(t_guanghuans[index]))
		{
			m_qh.GetComponent<UISprite>().set_enable(true);
			m_qh.GetComponent<BoxCollider>().enabled = true;
			m_cs.GetComponent<UISprite>().set_enable(true);
			m_cs.GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			m_qh.GetComponent<UISprite>().set_enable(false);
			m_qh.GetComponent<BoxCollider>().enabled = false;
			m_cs.GetComponent<UISprite>().set_enable(false);
			m_cs.GetComponent<BoxCollider>().enabled = false;
		}
	}

	public string get_skill_des(s_t_guanghuan_skill t_guanghuan_skill,bool first)
	{
		string text = "";
		string desc = "";
		if(t_guanghuan_skill.type == 1)
		{
			desc = game_data._instance.get_value_string(t_guanghuan_skill.def1,t_guanghuan_skill.def2,1);
		}
		if(t_guanghuan_skill.type == 2 || t_guanghuan_skill.type == 3)
		{
			desc = t_guanghuan_skill.desc;
			desc = desc.Replace("{{n1}}",t_guanghuan_skill.def1.ToString());
		}
		if(!first)
		{
			text += "\n\n";
		}
		if(guanghuan_skill_level(t_guanghuan_skill) >= t_guanghuan_skill.enhance)
		{
			text += "[FFFF00]";
		}
		else
		{
			text += "[777777]";
		}
		text += string.Format (game_data._instance.get_t_language ("guanghuan_gui.cs_243_25"), t_guanghuan_skill.name, t_guanghuan_skill.enhance, desc);//◆【{0}】强化至{1}级开启，{2}
		return text;
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "qh")
		{
			m_up_gui.GetComponent<guanghuan_up_gui>().t_guanghuan = t_guanghuans[index];
			m_up_gui.GetComponent<guanghuan_up_gui>().reset_skill();
			m_up_gui.SetActive(true);
		}
		if(obj.transform.name == "cs")
		{
			if(on_guanghuan(t_guanghuans[index]))
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_745_58"));//装备中的光环不可重生
				return;
			}
			if(guanghuan_level(t_guanghuans[index]) == 0)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guanghuan_gui.cs_264_59"));//未强化过的光环无法进行重生
				return;
			}
			s_message message = new s_message ();
			message.m_type = "show_guanghuan_cs";
			message.m_ints.Add(t_guanghuans[index].id);
			cmessage_center._instance.add_message(message);
		}
		if(obj.transform.name == "tujian")
		{
			m_tj_gui.GetComponent<guanghuan_tj_gui>().reset_tj();
			m_tj_gui.SetActive(true);
		}
		if(obj.transform.name == "equip")
		{
			protocol.game.cmsg_guanghuan_on _msg = new protocol.game.cmsg_guanghuan_on ();
			_msg.id = t_guanghuans[index].id;
			net_http._instance.send_msg<protocol.game.cmsg_guanghuan_on> (opclient_t.CMSG_GUANGHUAN_ON, _msg);
		}
	}

	public static string get_attr(int id,int enhance,int i)
	{
		s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan(id);
		List <string> text = new List<string>();
		string _text = "";
		float value = 0;
		value = (float)(t_guanghuan.value1 + enhance*  t_guanghuan.value1 *0.025);
		_text =  game_data._instance.get_value_string (t_guanghuan.attr1, value,1);
		text.Add (_text);
		if(t_guanghuan.attr2 > 0)
		{
			value = (float)(t_guanghuan.value2);
			_text =  game_data._instance.get_value_string (t_guanghuan.attr2, value,1);
			text.Add (_text);
		}
		
		return text[i];
	}

	public static int guanghuan_skill_level(s_t_guanghuan_skill t_guanghuan_skill)
	{
		int level = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.guanghuan.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.guanghuan[i] == t_guanghuan_skill.wing_id)
			{
				level = sys._instance.m_self.m_t_player.guanghuan_level[i];
				break;
			}
		}
		return level;
	}

	public static int guanghuan_level(s_t_guanghuan t_guanghuan)
	{
		int level = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.guanghuan.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.guanghuan[i] == t_guanghuan.id)
			{
				level = sys._instance.m_self.m_t_player.guanghuan_level[i];
				break;
			}
		}
		return level;
	}

	public void select(GameObject obj)
	{
		index = int.Parse (obj.transform.name);
		for(int i = 0; i < m_objs.Count;++i)
		{
			m_objs[i].transform.Find("select").gameObject.SetActive(false);
		}
		m_objs[index].transform.Find("select").gameObject.SetActive(true);
		update_ui (index);
		if(!has_guanghuan(t_guanghuans[index]))
		{
			s_message message = new s_message ();
			message.m_type = "show_cl_gui";
			message.m_ints.Add (t_guanghuans[index].id);
			cmessage_center._instance.add_message(message);
		}
	}

	public bool on_guanghuan(s_t_guanghuan t_guanghuan)
	{
		if(t_guanghuan.id == sys._instance.m_self.m_t_player.guanghuan_id)
		{
			return true;
		}
		return false;
	}

	public bool has_guanghuan(s_t_guanghuan t_guanghuan)
	{
		if(sys._instance.m_self.m_t_player.guanghuan.Contains(t_guanghuan.id))
		{
			return true;
		}
		return false;
	}

	int com(s_t_guanghuan t_guanghuan1,s_t_guanghuan t_guanghuan2)
	{
		if(on_guanghuan(t_guanghuan1) && !on_guanghuan(t_guanghuan2))
		{
			return -1;
		}
		else if(!on_guanghuan(t_guanghuan1) && on_guanghuan(t_guanghuan2))
		{
			return 1;
		}
		if(has_guanghuan(t_guanghuan1) && !has_guanghuan(t_guanghuan2))
		{
			return -1;
		}
		else if(!has_guanghuan(t_guanghuan1) && has_guanghuan(t_guanghuan2))
		{
			return 1;
		}
		else if(t_guanghuan1.color > t_guanghuan2.color)
		{
			return -1;
		}
		else if(t_guanghuan1.color < t_guanghuan2.color)
		{
			return 1;
		}
		else if(t_guanghuan1.attr1 > t_guanghuan2.attr1)
		{
			return -1;
		}
		else if(t_guanghuan1.attr1 < t_guanghuan2.attr1)
		{
			return 1;
		}
		else if(t_guanghuan1.id > t_guanghuan2.id)
		{
			return -1;
		}
		else if(t_guanghuan1.id < t_guanghuan2.id)
		{
			return 1;
		}
		return 0;
	}

	public void click_item_icon(GameObject obj)
	{
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)cl_id);
		cmessage_center._instance.add_message(message);
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_GUANGHUAN_INIT)
		{
			protocol.game.smsg_equip_init _msg = net_http._instance.parse_packet<protocol.game.smsg_equip_init>(message.m_byte);
			for (int i = 0; i < _msg.equips.Count; i++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i],true);
				
			}
			for (int i = 0; i < _msg.roles.Count; i++)
			{
				sys._instance.m_self.add_card(_msg.roles[i]);
				
			}
			for (int i = 0; i < _msg.treasures.Count; i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.types.Count; i++)
			{
				sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("guanghuan_gui.cs_443_101"));//光环重生获得
			}
			sys._instance.m_self.add_reward(7,t_guanghuans[index].id, 1, 0);
			for(int i = 0; i < sys._instance.m_self.m_t_player.guanghuan.Count;++i)
			{
				if(sys._instance.m_self.m_t_player.guanghuan[i] == t_guanghuans[index].id)
				{
					sys._instance.m_self.m_t_player.guanghuan_level[i] = 0;
					break;
				}
			}
			sys._instance.m_self.sub_att(e_player_attr.player_jewel,200,game_data._instance.get_t_language ("guanghuan_gui.cs_454_63"));//光环重生消耗
			reset();
		}
		if(message.m_opcode == opclient_t.CMSG_GUANGHUAN_ON)
		{
			if(sys._instance.m_self.m_t_player.guanghuan_id == t_guanghuans[index].id)
			{
				sys._instance.m_self.m_t_player.guanghuan_id = 0;
			}
			else
			{
				sys._instance.m_self.m_t_player.guanghuan_id = t_guanghuans[index].id;
			}
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
			Dictionary<int,double> sx1s = new Dictionary<int,double>();
			string text = "";
			sx1s = sx();
			foreach(int i  in sxs.Keys)
			{
				if(sx1s.ContainsKey(i))
				{
					continue;
				}
				sx1s.Add(i,0);
			}
			foreach(int i  in sx1s.Keys)
			{
				if(sxs.ContainsKey(i))
				{
					continue;
				}
				sxs.Add(i,0);
			}
			foreach(int i in sxs.Keys)
			{
				float value = (float)(sx1s[i] - sxs[i]);
				if(i > game_data._instance.m_t_value.get_y())
				{
					continue;
				}
				if(value > 0)
				{
					text += "[00ff00]" + game_data._instance.get_value_string(i,value,1) + "[-]\n";
				}
				else if(value < 0)
				{
					text += "[ff0000]" + game_data._instance.get_value_string(i,value,1).Replace("+","") + "[-]\n";
				}
			}
			m_dacheng_des.GetComponent<UILabel>().text = text;
			dacheng();
			sxs = sx ();
			reset();
			s_message _message = new s_message ();
			_message.m_type = "show_unit";
			_message.time = 0.1f;
			_message.m_object.Add (m_card);
			_message.m_ints.Add ((int)1);
			_message.m_string.Add ("select_show");
			_message.m_bools.Add (true);
			cmessage_center._instance.add_message (_message);
		}
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "guanghuan_chongsheng")
		{
			protocol.game.cmsg_guanghuan_init _msg = new protocol.game.cmsg_guanghuan_init();
			_msg.id = t_guanghuans[index].id;
			net_http._instance.send_msg<protocol.game.cmsg_guanghuan_init>(opclient_t.CMSG_GUANGHUAN_INIT, _msg);
		}
		if(message.m_type == "updte_guanghuan_gui")
		{
			reset(true);
		}
	}

	public static List<s_t_reward> get_guanghuan_chongsheng_reward(s_t_guanghuan t_guanghuan)
	{
		List<s_t_reward> rewards = get_guanghuan_chongsheng(t_guanghuan);
		List<s_t_reward> reward1s = new List<s_t_reward>();
		for (int i = 0; i < rewards.Count; i++)
		{
			reward1s.Add(rewards[i]);
		}
		s_t_reward reward = new s_t_reward();
		reward.type = 7;
		reward.value1 = t_guanghuan.id;
		reward.value2 = 0;
		reward.value3 = 0;
		reward1s.Add (reward);
		return reward1s;
	}

	public static  List<s_t_reward> get_guanghuan_chongsheng(s_t_guanghuan t_guanghuan)
	{
		List<s_t_reward> rewards = new List<s_t_reward>();
		int gold = 0;
		int cl = 0;
		int enhance = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.guanghuan.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.guanghuan[i] == t_guanghuan.id)
			{
				enhance = sys._instance.m_self.m_t_player.guanghuan_level[i];
				break;
			}
		}
		//强化
		for (int i = 1; i <= enhance; i++)
		{
			s_t_guanghuan_enhance _enhance = game_data._instance.get_t_guanghuan_enhance(i);
			gold += _enhance.golds[t_guanghuan.color -2];
			cl += _enhance.yuansus[t_guanghuan.color -2];
			
		}
		s_t_reward reward = new s_t_reward();
		reward.type = 1;
		reward.value1 = 1;
		reward.value2 = gold;
		reward.value3 = 0;
		if(gold > 0)
		{
			rewards.Add(reward);
		}
		s_t_reward t_reward = new s_t_reward();
		t_reward.type = 2;
		t_reward.value1 = 50150001;
		t_reward.value2 = cl;
		t_reward.value3 = 0;
		if(cl > 0)
		{
			rewards.Add(t_reward);
		}
		return rewards;
	}

	public void dacheng()
	{
		if(m_dacheng_des.GetComponent<UILabel>().text != "")
		{
			m_dacheng_des.SetActive(true);
			TweenScale _scale = sys._instance.add_scale_anim(m_dacheng_des.gameObject,0.2f,0.5f,1.2f,0);
			EventDelegate.Add(_scale.onFinished, delegate() 
			                  {
				hide();
			},true);
		}
	}

	public void hide()
	{
		hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();
		
		if(_hide_Label == null)
		{
			_hide_Label = m_dacheng_des.AddComponent<hide_Label>();
		}
		
		_hide_Label.m_time = 1.6f;
	}

	public static Dictionary<int,double> sx()
	{
		Dictionary<int,double> m_sxs = new Dictionary<int,double>();
		if (sys._instance.m_self.m_t_player.guanghuan_id > 0)
		{
			s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan(sys._instance.m_self.m_t_player.guanghuan_id);
			if (t_guanghuan != null)
			{
				int level = 0;
				for (int i = 0; i < sys._instance.m_self.m_t_player.guanghuan.Count; ++i)
				{
					if (sys._instance.m_self.m_t_player.guanghuan[i] == sys._instance.m_self.m_t_player.guanghuan_id)
					{
						level = sys._instance.m_self.m_t_player.guanghuan_level[i];
						break;
					}
				}
				if(!m_sxs.ContainsKey(t_guanghuan.attr1))
				{
					m_sxs.Add(t_guanghuan.attr1, t_guanghuan.value1 + t_guanghuan.value1 * 0.025f * level);
				}
				else
				{
					m_sxs[t_guanghuan.attr1] += (t_guanghuan.value1 + t_guanghuan.value1 * 0.025f * level);
				}
				if(!m_sxs.ContainsKey(t_guanghuan.attr2))
				{
					m_sxs.Add(t_guanghuan.attr2, t_guanghuan.value2);
				}
				else
				{
					m_sxs[t_guanghuan.attr2] += (t_guanghuan.value2);
				}
			}
		}
		return m_sxs;
	}
}
