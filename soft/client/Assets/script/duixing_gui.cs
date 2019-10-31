
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class duixing_gui : MonoBehaviour,IMessage {

	public GameObject m_scro;
	public GameObject m_view;
	public GameObject m_change_panel;
	public GameObject m_up_panel;
	public List<GameObject> m_icons = new List<GameObject>();
	public List<GameObject> m_objs = new List<GameObject>();
	public GameObject m_duixing_level;
	public GameObject m_q_attr;
	public GameObject m_q_attr1;
	public GameObject m_q_up;
	public GameObject m_q_up1;
	public GameObject m_z_attr;
	public GameObject m_z_attr1;
	public GameObject m_z_up;
	public GameObject m_z_up1;
	public GameObject m_h_attr;
	public GameObject m_h_attr1;
	public GameObject m_h_up;
	public GameObject m_h_up1;
	public GameObject m_use;
	public GameObject m_duixing_now_level;
	public GameObject m_duixing_next_level;
	public GameObject m_skill_name;
	public GameObject m_skill_desc;
	public GameObject m_player_level;
	public GameObject m_gold;
	public GameObject m_cl_icon;
	public GameObject m_change_toggle;
	public GameObject m_up_toggle;
	public GameObject m_effect;
	public bool up_flag = false;
	private int m_select = 0;
	private string error = "";
	private uint cl_id = 50140001;
	int duixing_id = 0;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		m_up_toggle.GetComponent<UIToggle>().Set(false);
		m_change_toggle.GetComponent<UIToggle>().Set(true);
	}

	public void reset(int select)
	{
		error = "";
		if(up_flag)
		{
			m_up_toggle.SetActive(false);
		}
		else
		{
			m_up_toggle.SetActive(true);
		}
		if(select == 0)
		{
			m_objs.Clear();
			for(int i = 0; i < m_icons.Count;++i)
			{
				sys._instance.remove_child (m_icons [i]);
				m_icons[i].SetActive(false);
			}
			m_change_panel.SetActive(true);
			m_up_panel.SetActive(false);
			if(m_scro.GetComponent<SpringPanel>() != null)
			{
				m_scro.GetComponent<SpringPanel>().enabled = false;
			}
			m_scro.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
			m_scro.transform.localPosition = new Vector2 (0, 0);
			sys._instance.remove_child(m_scro);
			List<s_t_duixng> duixings = new List<s_t_duixng>();
			bool flag = false;
			for(int i = 0;i < game_data._instance.m_dbc_duixing.get_y();++i)
			{
				int id = int.Parse(game_data._instance.m_dbc_duixing.get (0,i));
				s_t_duixng _t_duixing = game_data._instance.get_t_duixing(id);
				if(flag)
				{
					break;
				}
				if(_t_duixing.type == 1 && !flag)
				{
					duixings.Add(_t_duixing);
				}
				if(_t_duixing.level > sys._instance.m_self.m_t_player.level)
				{
					flag = true;
				}
			}
			bool s_flag = false;
			int num = get_num(duixings);
			int y = 0;
			int _id = 0;
			if(num < 1)
			{
				y = 86;
			}
			else if(num < duixings.Count -1)
			{
				y = -46;
			}
			else
			{
				y = -178;
			}
			_id = num;
			for(int i = 0 ;i < duixings.Count;++i)
			{
				GameObject obj = game_data._instance.ins_object_res("ui/duixing_item");
				obj.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
				obj.transform.parent = m_scro.transform;
				obj.transform.name = duixings[i].id.ToString();
				obj.transform.localPosition = new Vector3(-270, y + 132*_id,0);
				if(s_flag)
				{
					obj.transform.localPosition = new Vector3(-270, y - 132*_id,0);
				}
				obj.transform.localScale = new Vector3(1,1,1);
				obj.transform.GetComponent<duixing_item>().t_duixing = duixings[i];
				obj.transform.GetComponent<duixing_item>().reset();
				if(duixings[i].id == sys._instance.m_self.m_t_player.duixing_id)
				{
					s_flag = true;
					_id = 0;
					obj.transform.Find("select").gameObject.SetActive(true);
				}
				else
				{
					obj.transform.Find("select").gameObject.SetActive(false);
				}
				m_objs.Add(obj);
				if(s_flag)
				{
					_id ++;
				}
				else
				{
					_id--;
				}
			}
			get_attr(sys._instance.m_self.m_t_player.duixing_id);
			m_duixing_level.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.duixing_level.ToString();
			s_t_duixng t_duixing = game_data._instance.get_t_duixing (sys._instance.m_self.m_t_player.duixing_id);
			m_use.GetComponent<BoxCollider>().enabled = false;
			m_use.GetComponent<UISprite>().set_enable(false);
			for(int i = 0;i < (int)sys._instance.m_self.m_t_player.duixing.Count;i ++)
			{
				set_info_ex(i,t_duixing.zhenwei[(int)sys._instance.m_self.m_t_player.duixing[i]]);
			}
			for(int i = 0; i < t_duixing.zhenwei.Count;++i)
			{
				m_icons[t_duixing.zhenwei[i]].SetActive(true);
			}
		}
		if(select == 1)
		{
			m_change_panel.SetActive(false);
			m_up_panel.SetActive(true);
			m_duixing_level.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.duixing_level.ToString();
			if(m_view.GetComponent<SpringPanel>() != null)
			{
				m_view.GetComponent<SpringPanel>().enabled = false;
			}
			m_view.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
			m_view.transform.localPosition = new Vector2 (0, 0);
			sys._instance.remove_child(m_view);
			for(int i = 0;i < game_data._instance.m_dbc_duixing_skill.get_y();++i)
			{
				int id = int.Parse(game_data._instance.m_dbc_duixing_skill.get (0,i));
				s_t_duixng_skill t_duixing_skill = game_data._instance.get_t_duixing_skill(id);
				GameObject obj = game_data._instance.ins_object_res("ui/duixing_up_item");
				obj.transform.parent = m_view.transform;
				obj.transform.localPosition = new Vector3(-202, 112 - 130*i,0);
				obj.transform.localScale = new Vector3(1,1,1);
				obj.transform.GetComponent<duixing_up_item>().t_duixing_skill = t_duixing_skill;
				obj.transform.GetComponent<duixing_up_item>().reset();
			}
			m_duixing_now_level.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.duixing_level.ToString();
			m_duixing_next_level.GetComponent<UILabel>().text = (sys._instance.m_self.m_t_player.duixing_level +1).ToString();
			m_skill_name.GetComponent<UILabel>().text = "----";
			m_skill_desc.GetComponent<UILabel>().text = game_data._instance.get_t_language ("duixing_gui.cs_196_47");//已开启全部队形技能
			m_player_level.GetComponent<UILabel>().text = "--";
			m_gold.GetComponent<UILabel>().text = "--";
			sys._instance.remove_child(m_cl_icon);
			int num = sys._instance.m_self.get_item_num(cl_id);
			GameObject iicon1 = icon_manager._instance.create_item_icon_ex((int)cl_id,num,-1);
			iicon1.transform.parent =  m_cl_icon.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			UIButtonMessage[] message = iicon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_item_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";
			for(int i = 0;i < game_data._instance.m_dbc_duixing_skill.get_y();++i)
			{
				int id = int.Parse(game_data._instance.m_dbc_duixing_skill.get(0,i));
				s_t_duixng_skill _t_duixing_skill = game_data._instance.get_t_duixing_skill(id);
				if(sys._instance.m_self.m_t_player.duixing_level < _t_duixing_skill.level)
				{
					m_skill_name.GetComponent<UILabel>().text = _t_duixing_skill.name + "[0aabff](" + string.Format(game_data._instance.get_t_language ("duixing_gui.cs_218_101"),_t_duixing_skill.level);//{0}级开启)[-]
					m_skill_desc.GetComponent<UILabel>().text = _t_duixing_skill.desc;
					break;
				}
			}
			s_t_duixng_up t_duixing_up = game_data._instance.get_t_duixing_up(sys._instance.m_self.m_t_player.duixing_level+1);
			if(t_duixing_up != null)
			{
				m_player_level.GetComponent<UILabel>().text = "[0aff16]" + t_duixing_up.level2.ToString();
				if(sys._instance.m_self.m_t_player.level < t_duixing_up.level2)
				{
					m_player_level.GetComponent<UILabel>().text = "[ff0000]" + t_duixing_up.level2;
					error = game_data._instance.get_t_language ("chong_neng_gui.cs_258_12");//玩家等级不足
				}
				m_gold.GetComponent<UILabel>().text = sys._instance.get_res_color(1) + t_duixing_up.gold;
				if(sys._instance.m_self.m_t_player.gold < t_duixing_up.gold)
				{
					m_gold.GetComponent<UILabel>().text = "[ff0000]" + t_duixing_up.gold;
					error = game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
				}
				if(num < t_duixing_up.cl)
				{
					error = "0";
				}
				sys._instance.remove_child(m_cl_icon);
				GameObject iicon = icon_manager._instance.create_item_icon_ex((int)cl_id,num,t_duixing_up.cl);
				iicon.transform.parent =  m_cl_icon.transform;
				iicon.transform.localPosition = new Vector3(0,0,0);
				iicon.transform.localScale = new Vector3(1,1,1);
				UIButtonMessage[] message1 = iicon.transform.GetComponents<UIButtonMessage>();
				message1[0].target = this.gameObject;
				message1[0].functionName = "click_item_icon";
				message1[1].target = null;
				message1[1].functionName = "";
				message1[2].target = null;
				message1[2].functionName = "";
			}
			else
			{
				error = game_data._instance.get_t_language ("duixing_gui.cs_257_12");//队形已升至满级
			}
		}
		if(up_duixing())
		{
			m_effect.SetActive(true);
		}
		else
		{
			m_effect.SetActive(false);
		}
	}

	public static bool up_duixing()
	{
		s_t_duixng_up t_duixing_up = game_data._instance.get_t_duixing_up(sys._instance.m_self.m_t_player.duixing_level+1);
		if(t_duixing_up == null)
		{
			return false;
		}
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shengji_duixing)
		{
			return false;
		}
		if(sys._instance.m_self.m_t_player.level < t_duixing_up.level2)
		{
			return false;
		}
		if(sys._instance.m_self.m_t_player.gold < t_duixing_up.gold)
		{
			return false;
		}
		int num = sys._instance.m_self.get_item_num((uint)50140001);
		if(num < t_duixing_up.cl)
		{
			return false;
		}
		return true;
	}

	public int get_num(List<s_t_duixng> t_duixings)
	{
		int num = 0;
		for(int i = 0; i < t_duixings.Count;++i)
		{
			if(t_duixings[i].id == sys._instance.m_self.m_t_player.duixing_id)
			{
				num = i;
				break;
			}
		}
		return num;
	}

	public void get_attr(int id)
	{
		s_t_duixng t_duixing = game_data._instance.get_t_duixing (id);
		string text = "";
		if(t_duixing.q_attr > 0)
		{
			text = game_data._instance.get_value_string(t_duixing.q_attr,t_duixing.q_value + t_duixing.q_cz * sys._instance.m_self.m_t_player.duixing_level);
			m_q_attr.GetComponent<UILabel>().text = "[0aabff]" + text.Split('+')[0] +"[-][0aff16]+" + text.Split('+')[1] +"[-]" ;
		}
		else
		{
			m_q_attr.GetComponent<UILabel>().text = "[0aabff]" + game_data._instance.get_t_language ("duixing_gui.cs_322_56") + "[-]";//无属性
		}
		if(t_duixing.q_attr1 > 0)
		{
			text = game_data._instance.get_value_string(t_duixing.q_attr1,t_duixing.q_value1 + t_duixing.q_cz1 * sys._instance.m_self.m_t_player.duixing_level);
			m_q_attr1.GetComponent<UILabel>().text = "[0aabff]" + text.Split('+')[0] +"[-][0aff16]+" + text.Split('+')[1] +"[-]";
		}
		else
		{
			m_q_attr1.GetComponent<UILabel>().text = "";
		}
		if(t_duixing.z_attr > 0)
		{
			text = game_data._instance.get_value_string(t_duixing.z_attr,t_duixing.z_value + t_duixing.z_cz * sys._instance.m_self.m_t_player.duixing_level);
			m_z_attr.GetComponent<UILabel>().text = "[0aabff]" + text.Split('+')[0] +"[-][0aff16]+" + text.Split('+')[1] +"[-]";
		}
		else
		{
            m_z_attr.GetComponent<UILabel>().text = "[0aabff]" + game_data._instance.get_t_language ("duixing_gui.cs_322_56") + "[-]";//无属性
		}
		if(t_duixing.z_attr1 > 0)
		{
			text = game_data._instance.get_value_string(t_duixing.z_attr1,t_duixing.z_value1 + t_duixing.z_cz1 * sys._instance.m_self.m_t_player.duixing_level);
			m_z_attr1.GetComponent<UILabel>().text = "[0aabff]" + text.Split('+')[0] +"[-][0aff16]+" + text.Split('+')[1] +"[-]";
		}
		else
		{
			m_z_attr1.GetComponent<UILabel>().text = "";
		}
		if(t_duixing.h_attr > 0)
		{
			text = game_data._instance.get_value_string(t_duixing.h_attr,t_duixing.h_value + t_duixing.h_cz * sys._instance.m_self.m_t_player.duixing_level);
			m_h_attr.GetComponent<UILabel>().text = "[0aabff]" + text.Split('+')[0] +"[-][0aff16]+" + text.Split('+')[1] +"[-]";
		}
		else
		{
            m_h_attr.GetComponent<UILabel>().text = "[0aabff]" + game_data._instance.get_t_language ("duixing_gui.cs_322_56") + "[-]";//无属性
		}
		if(t_duixing.h_attr1 > 0)
		{
			text = game_data._instance.get_value_string(t_duixing.h_attr1,t_duixing.h_value1 + t_duixing.h_cz1 * sys._instance.m_self.m_t_player.duixing_level);
			m_h_attr1.GetComponent<UILabel>().text = "[0aabff]" + text.Split('+')[0] +"[-][0aff16]+" + text.Split('+')[1] +"[-]";
		}
		else
		{
			m_h_attr1.GetComponent<UILabel>().text = "";
		}
		float value = t_duixing.q_cz;
		if( t_duixing.q_attr <= 0)
		{
			m_q_up.GetComponent<UILabel>().text ="";
		}
		else
		{
			m_q_up.GetComponent<UILabel>().text =string.Format(game_data._instance.get_t_language ("duixing_gui.cs_376_54"), game_data._instance.get_value_string(t_duixing.q_attr,value).Split('+')[1]);//(升级+{0})
		}
		value = t_duixing.q_cz1;
		if(t_duixing.q_attr1 <= 0)
		{
			m_q_up1.GetComponent<UILabel>().text ="";
		}
		else
		{
			m_q_up1.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("duixing_gui.cs_376_54") , game_data._instance.get_value_string(t_duixing.q_attr1,value).Split('+')[1] );//(升级+{0})
		}
		value = t_duixing.z_cz;
		if(t_duixing.z_attr <= 0)
		{
			m_z_up.GetComponent<UILabel>().text ="";
		}
		else
		{
			m_z_up.GetComponent<UILabel>().text =string.Format(game_data._instance.get_t_language ("duixing_gui.cs_376_54") , game_data._instance.get_value_string(t_duixing.z_attr,value).Split('+')[1] );//(升级+{0})
		}
		value =  t_duixing.z_cz1;
		if(t_duixing.z_attr1 <= 0)
		{
			m_z_up1.GetComponent<UILabel>().text ="";
		}
		else
		{
			m_z_up1.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("duixing_gui.cs_376_54") , game_data._instance.get_value_string(t_duixing.z_attr1,value).Split('+')[1] );//(升级+{0})
		}
		value = t_duixing.h_cz;
		if(t_duixing.h_attr <= 0)
		{
			m_h_up.GetComponent<UILabel>().text ="";
		}
		else
		{
			m_h_up.GetComponent<UILabel>().text =string.Format(game_data._instance.get_t_language ("duixing_gui.cs_376_54") , game_data._instance.get_value_string(t_duixing.h_attr,value).Split('+')[1]);//(升级+{0})
		}
		value =  t_duixing.h_cz1;
		if(t_duixing.h_attr1 <= 0)
		{
			m_h_up1.GetComponent<UILabel>().text ="";
		}
		else
		{
			m_h_up1.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("duixing_gui.cs_376_54"), game_data._instance.get_value_string(t_duixing.h_attr1,value).Split('+')[1]);//(升级+{0})
		}
	}

	public void select(GameObject obj)
	{
		duixing_id = int.Parse (obj.transform.name);
		s_t_duixng t_duixing = game_data._instance.get_t_duixing (duixing_id);
		if(t_duixing.level > sys._instance.m_self.m_t_player.level)
		{
			root_gui._instance.show_prompt_dialog_box("[ffc882]"+ string.Format(game_data._instance.get_t_language ("duixing_gui.cs_431_71"),t_duixing.level ) + t_duixing.name);//{0}级开启
			return;
		}
		if(duixing_id == sys._instance.m_self.m_t_player.duixing_id)
		{
			m_use.GetComponent<BoxCollider>().enabled = false;
			m_use.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			m_use.GetComponent<BoxCollider>().enabled = true;
			m_use.GetComponent<UISprite>().set_enable(true);
		}
		get_attr(duixing_id);
		for(int i = 0; i < m_icons.Count;++i)
		{
			sys._instance.remove_child (m_icons [i]);
			m_icons [i].SetActive(false);
		}
		for(int i = 0;i < (int)sys._instance.m_self.m_t_player.duixing.Count;i ++)
		{
			set_info_ex(i,t_duixing.zhenwei[(int)sys._instance.m_self.m_t_player.duixing[i]]);
		}
		for(int i = 0;i < m_objs.Count;++i)
		{
			if(m_objs[i].name == obj.transform.name)
			{
				m_objs[i].transform.Find("select").gameObject.SetActive(true);
			}
			else
			{
				m_objs[i].transform.Find("select").gameObject.SetActive(false);
			}
		}
	}

	public void set_info_ex(int id,int _id)
	{
		//int _id = (int)sys._instance.m_self.m_t_player.duixing[id];
		sys._instance.remove_child (m_icons [_id]);
		m_icons [_id].SetActive (true);
		m_icons [_id].GetComponent<BoxCollider>().enabled = true;

		
		set_info (id,_id);
	}

	public void set_info(int id,int _id)
	{
		sys._instance.remove_child (m_icons [_id]);
		m_icons [_id].SetActive (true);
		ccard _card = sys._instance.m_self.get_card_guid (sys._instance.m_self.m_t_player.zhenxing[id]);

		GameObject _root = game_data._instance.ins_object_res("ui/duixing_icon");
		GameObject m_icon = _root.transform.Find("icon").gameObject;
		m_icon.transform.localScale = new Vector3(0.72f,0.72f,0.72f);
		_root.transform.parent = m_icons [_id].transform;
		_root.transform.name = (id).ToString();
		_root.transform.localPosition = new Vector3(0,0,0);
		_root.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		_root.SetActive(true);
		_root.GetComponent<dui_xing_icon>().m_parent = m_icons[_id].transform;
		_root.GetComponent<dui_xing_icon>().m_item = _root.GetComponent<dui_xing_icon>();
		_root.GetComponent<dui_xing_icon>().m_icons = m_icons;

		if(_card == null)
		{
			_root.SetActive(false);
			return;
		}

		GameObject _icon = icon_manager._instance.create_card_icon_ex (sys._instance.m_self.m_t_player.zhenxing[id]);
		sys._instance.remove_child (m_icon);
		_icon.transform.name =  ((int)id).ToString();
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		_icon.SetActive(true);
		_icon.GetComponent<BoxCollider>().enabled = false;
		
		sys._instance.add_scale_anim (_icon,0.5f,1.2f,1,0);
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_ROLE_DUIXING_ON)
		{
			sys._instance.m_self.m_t_player.duixing_id = duixing_id;
			reset(m_select);
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
		if(message.m_opcode == opclient_t.CMSG_ROLE_DUIXING_UP)
		{
			s_t_duixng_up t_duixing_up = game_data._instance.get_t_duixing_up(sys._instance.m_self.m_t_player.duixing_level+1);
            sys._instance.m_self.remove_item(cl_id, t_duixing_up.cl, game_data._instance.get_t_language ("duixing_gui.cs_527_69"));//队形升级消耗
			sys._instance.m_self.sub_att(e_player_attr.player_gold,t_duixing_up.gold,game_data._instance.get_t_language ("duixing_gui.cs_527_69"));//队形升级消耗
			sys._instance.m_self.m_t_player.duixing_level += 1;
			reset(m_select);
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);

			sys._instance.play_sound_ex("sound/skill_up");

			for(int i = 0;i < game_data._instance.m_dbc_duixing_skill.get_y();++i)
			{
				int id = int.Parse(game_data._instance.m_dbc_duixing_skill.get (0,i));
				s_t_duixng_skill t_duixing_skill = game_data._instance.get_t_duixing_skill(id);
				if(t_duixing_skill.level == sys._instance.m_self.m_t_player.duixing_level)
				{
					root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("duixing_gui.cs_543_61") , t_duixing_skill.name));//开启[00ff00]{0}[-]技能
					break;
				}
			}
			s_message _message = new s_message();
			_message.m_type = "update_duixing_effect";
			cmessage_center._instance.add_message(_message);
		}
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "dui_xing")
		{
			protocol.game.cmsg_duixing _msg = new protocol.game.cmsg_duixing ();
			_msg.dest_site = (int)message.m_ints[1];
			_msg.src_site = (int)message.m_ints[0];
			net_http._instance.send_msg<protocol.game.cmsg_duixing> (opclient_t.CMSG_ROLE_DUIXING, _msg);
			set_info_change((int)message.m_ints[0],(int)message.m_ints[1],(int)message.m_ints[2],(int)message.m_ints[3]);
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
	}

	public void set_info_change(int form,int to,int form_id,int to_id)
	{
		int d1 = (int)sys._instance.m_self.m_t_player.duixing [form];
		int d2 = (int)sys._instance.m_self.m_t_player.duixing [to];
	
		sys._instance.m_self.m_t_player.duixing[form] = d2;
		sys._instance.m_self.m_t_player.duixing[to] = d1;
		
		set_info_ex(form,to_id);
		set_info_ex(to,form_id);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			m_change_toggle.GetComponent<UIToggle>().value = true;
			m_select = 0;
            s_message _mes = new s_message();
            _mes.m_type = "bingyuan_canmove";
            cmessage_center._instance.add_message(_mes);
		}
		if(obj.transform.name == "use")
		{
			protocol.game.cmsg_duixing_on _msg = new protocol.game.cmsg_duixing_on ();
			_msg.id = duixing_id;
			net_http._instance.send_msg<protocol.game.cmsg_duixing_on> (opclient_t.CMSG_ROLE_DUIXING_ON, _msg);
		}
		if(obj.transform.name == "change")
		{
			m_select = 0;
			reset(m_select);
		}
		if(obj.transform.name == "up")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shengji_duixing)
			{
				m_up_toggle.GetComponent<UIToggle>().enabled = false;
			}
			else
			{
				m_up_toggle.GetComponent<UIToggle>().enabled = true;
			}
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shengji_duixing)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("duixing_gui.cs_614_59"));//升级队形18级开启，主人赶快提升等级吧
				return;
			}
			m_select = 1;
			reset(1);
		}
		if(obj.transform.name == "sj")
		{
			if(error != "" && error != "0")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + error);
				return;
			}
			if(error == "0")
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)cl_id);
				cmessage_center._instance.add_message(message);
				return;
			}
			
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_ROLE_DUIXING_UP, _msg);
		}
	}

	public void click_item_icon(GameObject obj)
	{
		//m_change_toggle.GetComponent<UIToggle>().value = true;
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)cl_id);
		cmessage_center._instance.add_message(message);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
