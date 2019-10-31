
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_jl_gui : MonoBehaviour,IMessage {

	dhc.equip_t m_equip;
	
	public GameObject m_effect;
	public GameObject m_center;
	public bool m_play = false;
	private string m_out_message;
	public List<int> m_gongzhengs = new List<int>();
	public List<GameObject> m_old_attr = new List<GameObject>();
	public List<GameObject> m_new_attr = new List<GameObject>() ;
	public GameObject m_equip_name;
	public GameObject m_equip_jinlian;
	public GameObject m_need_stone;
	public GameObject m_need_gold;
	public GameObject m_tishi1;
	public GameObject m_icon;
	public GameObject m_icon1;
	public GameObject m_equip_skill_gui;
	public GameObject m_view;
	public GameObject m_jinglian;
	public GameObject m_skill_yl;
	public GameObject m_dacheng_des;
	public GameObject m_one;
	public GameObject m_jinglian_one;
	public GameObject m_ten;
	public GameObject m_jinglian_ten;
	public GameObject m_jinglian_ex;
	public GameObject m_num_gui;
	public int flag = 0;

	private GameObject m_jinglian_button;
	private ulong m_guid;
	private uint m_stone_id = 50070001;
	private int jinglian = 0;
	public UILabel m_cur_sx;
	public UILabel m_next_sx;
	public UILabel m_desc;
	public UILabel m_xiaohao;
	
	// Use this for initialization
	void Start ()
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		if (m_equip.role_guid != 0) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_equip.role_guid);
			sys._instance.m_card = m_card;
			sys._instance.m_gongzhengs.Clear();
			sys._instance.m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		}
		m_dacheng_des.SetActive (false);
	}

	public void reset(dhc.equip_t _equip, string out_message)
	{
		flag = 0;
        m_equip = _equip;
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_yj_equip_jl)
		{
			m_one.SetActive(true);
			m_ten.SetActive(false);
			m_jinglian_button = m_jinglian_one;
		}
		else
		{
			m_one.SetActive(false);
			m_ten.SetActive(true);
			m_jinglian_button = m_jinglian_ten;
		}
		m_out_message = out_message;
		if (m_equip.role_guid != 0) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_equip.role_guid);
			m_gongzhengs.Clear();
			m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		}		
		m_equip_name.GetComponent<UILabel>().text = equip.get_equip_real_name (m_equip.template_id);
		m_equip_jinlian.GetComponent<UILabel>().text = "[" + m_equip.jilian + game_data._instance.get_t_language ("equip_jl_gui.cs_94_72")+"]";//阶
		sys._instance.remove_child (m_icon);
		sys._instance.remove_child (m_icon1);

		GameObject iicon2 = icon_manager._instance.create_equip_icon(m_equip.template_id, m_equip.enhance, m_equip.star);
		iicon2.transform.parent =  m_icon.transform;
		iicon2.transform.localPosition = new Vector3(0,0,0);
		iicon2.transform.localScale = new Vector3(1,1,1);
		iicon2.transform.GetComponent<BoxCollider>().enabled = true;
		iicon2.transform.GetComponent<BoxCollider>().enabled = false;
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		s_t_equip_jl _equip_jl = game_data._instance.get_t_equip_jl (m_equip.jilian);
		int stone_num = sys._instance.m_self.get_item_num(m_stone_id);
		for(int i = 0; i < m_old_attr.Count;++i)
		{
			m_old_attr[i].SetActive(true);
			m_old_attr[i].GetComponent<UILabel>().text = equip.get_equip_jl_text(m_equip,m_equip.jilian,i);
		}
		s_t_equip_jl _equip_jl_next = game_data._instance.get_t_equip_jl (m_equip.jilian +1);
		if(_equip_jl_next == null)
		{
			m_tishi1.SetActive(true);
			for(int i = 0; i < m_new_attr.Count;++i)
			{
				m_new_attr[i].SetActive(false);
			}
			flag = 1;
			m_need_stone.GetComponent<UILabel>().text = "--";
			m_need_gold.GetComponent<UILabel>().text = "--";
			GameObject iicon1 = icon_manager._instance.create_item_icon_ex((int)m_stone_id,stone_num);
			iicon1.transform.parent =  m_icon1.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			iicon1.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = iicon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_item_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";
		}
		else
		{
			GameObject iicon1 = icon_manager._instance.create_item_icon_ex((int)m_stone_id,stone_num, _equip_jl_next.stones[t_equip.font_color -1]);
			iicon1.transform.parent =  m_icon1.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			iicon1.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = iicon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_item_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";

			string cl = "";
			for(int i = 0; i < m_new_attr.Count;++i)
			{
				m_new_attr[i].SetActive(true);
				m_new_attr[i].GetComponent<UILabel>().text = equip.get_equip_jl_text(m_equip,m_equip.jilian+1,i);
			}
			if(stone_num < _equip_jl_next.stones[t_equip.font_color -1])
			{
				flag = 3;
				cl = "[ff0000]";
			}
			m_need_stone.GetComponent<UILabel>().text = cl + _equip_jl_next.stones[t_equip.font_color -1];
			cl = sys._instance.get_res_color(1);
			if(sys._instance.m_self.get_att(e_player_attr.player_gold) < _equip_jl_next.golds[t_equip.font_color -1])
			{
				flag = 2;
				cl = "[ff0000]";
			}
			m_need_gold.GetComponent<UILabel>().text = cl +  _equip_jl_next.golds[t_equip.font_color -1];
		}
		m_jinglian_button.GetComponent<UIButton>().isEnabled = true;
		m_jinglian_ex.GetComponent<UIButton>().isEnabled = true;
		m_play = false;
		m_effect.gameObject.SetActive (false);
		if(t_equip.font_color == 5)
		{
			m_skill_yl.SetActive(true);
		}
		else
		{
			m_skill_yl.SetActive(false);
		}
	}

	bool isplay()
	{
		Animator  animator = m_effect.GetComponent<Animator>();
		AnimatorStateInfo animatorInfo; 
		animatorInfo =animator.GetCurrentAnimatorStateInfo(0); 
		if (animatorInfo.normalizedTime > 1.0f ) 
		{
			return true;
		}
		else 
		{
			return false;
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			Object.Destroy(this.gameObject);
			
			s_message _msg = new s_message();
			_msg.m_type = m_out_message;
            _msg.m_ints.Add(2);
			_msg.m_long.Add(m_equip.guid);
			cmessage_center._instance.add_message(_msg);
		}
		if(obj.name == "jinglian")
		{
			if(flag == 1)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_detail.cs_544_59"));//该装备已精炼至满阶
				return;
			}
			if(flag == 2)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
				return;
			}
			if(flag == 3)
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)m_stone_id);
				cmessage_center._instance.add_message(message);
				return;
			}
			jinglian =1;
			protocol.game.cmsg_equip_jinlian _msg = new protocol.game.cmsg_equip_jinlian ();
			_msg.equip_guid = m_equip.guid;
			_msg.level = jinglian;
			net_http._instance.send_msg<protocol.game.cmsg_equip_jinlian> (opclient_t.CMSG_EQUIP_JINLIAN, _msg);
		}
		if (obj.name == "jinglian_ex")
		{
			s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
			s_t_equip_jl _equip_jl = game_data._instance.get_t_equip_jl (m_equip.jilian +1);
			int stone_num = sys._instance.m_self.get_item_num(m_stone_id);
			if(_equip_jl ==null)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_detail.cs_544_59"));//该装备已精炼至满阶
				return;
			}
			if(sys._instance.m_self.m_t_player.gold  <  _equip_jl.golds[t_equip.font_color -1])
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
				return;
			}
			if(stone_num <   _equip_jl.stones[t_equip.font_color -1])
			{
				s_message message = new s_message ();
				message.m_type = "show_cl_gui";
				message.m_ints.Add ((int)m_stone_id);
				cmessage_center._instance.add_message(message);
				return;
			}
			m_num_gui.GetComponent<equip_jl_fast_gui>().m_equip = m_equip;
			m_num_gui.GetComponent<equip_jl_fast_gui>().m_input_num = 1;
			m_num_gui.GetComponent<equip_jl_fast_gui>().reset();
			m_num_gui.SetActive(true);
		}
		if(obj.name == "add_gold")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_jd_gui";
			cmessage_center._instance.add_message(_mes);
		}
		if (obj.name == "skill")
		{
			m_equip_skill_gui.SetActive(true);
			reset_skill();
		}
		if(obj.name == "skill_close")
		{
			m_equip_skill_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
		}
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


	public void reset_skill()
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		List<s_t_equip_skill> equip_skills = new List<s_t_equip_skill>();
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		for(int i = 0 ; i < game_data._instance.m_dbc_equip_skill.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_equip_skill.get(0,i));
			s_t_equip_skill t_equip_skill = game_data._instance.get_t_equip_skill (id);
			if(t_equip_skill.part == t_equip.type)
			{
				equip_skills.Add(t_equip_skill);
			}
		}
		for(int i = 0; i < equip_skills.Count;++i)
		{
			GameObject active = game_data._instance.ins_object_res("ui/equip_skill_sub");
			active.transform.parent = m_view.transform;
			active.transform.localPosition = new Vector3(0, 151 - i * 109,0);
			active.transform.localScale = new Vector3(1,1,1);
			active.transform.GetComponent<equip_skill_sub>().t_equip_skill = equip_skills[i];
			active.transform.GetComponent<equip_skill_sub>().jinlian = m_equip.jilian;
			active.transform.GetComponent<equip_skill_sub>().reset();
			sys._instance.add_pos_anim(active,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(active,0.3f, 0, 1.0f, i * 0.05f);
		}
		m_jinglian.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_jl_gui.cs_345_59") , m_equip.jilian);//当前精炼阶段数:{0}阶
	}

	public void click_item_icon(GameObject obj)
	{
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)m_stone_id);
		cmessage_center._instance.add_message(message);
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_EQUIP_JINLIAN)
		{
			s_t_equip _equip = game_data._instance.get_t_equip(m_equip.template_id);
			int gold = 0;
			int cl =0;
			for(int i = 1; i <= jinglian; i++)
			{
				s_t_equip_jl _equip_jl = game_data._instance.get_t_equip_jl(m_equip.jilian +i);
				gold += _equip_jl.golds[_equip.font_color -1];
				cl += _equip_jl.stones[_equip.font_color -1];
			}
			sys._instance.m_self.sub_att(e_player_attr.player_gold, gold,game_data._instance.get_t_language ("equip_jl_gui.cs_369_64"));//装备精炼消耗
			sys._instance.m_self.remove_item(m_stone_id,cl,game_data._instance.get_t_language ("equip_jl_gui.cs_369_64"));//装备精炼消耗

			m_equip.jilian += jinglian;
			if(m_equip.role_guid != 0)
			{
				ccard m_card = sys._instance.m_self.get_card_guid(m_equip.role_guid);
				string s = sys._instance.m_self.gongzhengs_ex(m_card,m_gongzhengs);
				if(s != "")
				{
					root_gui._instance.show_prompt_dialog_box(s);
				}
			}
			m_effect.GetComponent<Animator>().speed = 4.0f;
			m_effect.gameObject.SetActive (true);
			sys._instance.play_sound ("sound/gaizao");
			m_play = true;
			
			GameObject _main = m_center.transform.Find("main").gameObject;
			m_jinglian_button.GetComponent<UIButton>().isEnabled = false;
			m_jinglian_ex.GetComponent<UIButton>().isEnabled = false;
			if(_equip.font_color == 5)
			{
				skill_Label();
			}
		}
	}

	public void skill_Label()
	{
		string text = "";
		bool flag = false;
		List<s_t_equip_skill> equip_skills = new List<s_t_equip_skill>();
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		for(int i = 0 ; i < game_data._instance.m_dbc_equip_skill.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_equip_skill.get(0,i));
			s_t_equip_skill t_equip_skill = game_data._instance.get_t_equip_skill (id);
			if(t_equip_skill.part == t_equip.type)
			{
				equip_skills.Add(t_equip_skill);
			}
		}
		for(int i = 0; i < equip_skills.Count;++i)
		{
			if(m_equip.jilian == equip_skills[i].jinglian)
			{ 
				flag = true;
				text = string.Format(game_data._instance.get_t_language ("equip_jl_gui.cs_417_25"), equip_skills[i].name );//[ffffff]圣器技能[-][f9d420]{0}[-][ffffff]开启[-]
				break;
			}
		}
		if(flag)
		{
			m_dacheng_des.SetActive(false);
			hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();
			if(_hide_Label != null)
			{
				Destroy(_hide_Label);
			}
			m_dacheng_des.GetComponent<UILabel>().text = text;
			dacheng();
		}
	}


	void IMessage.message(s_message message)
	{
		if(message.m_type == "refresh_equip_jl" )
		{
			reset(m_equip, m_out_message);
		}
		if(message.m_type == "equip_yj_jinglian")
		{
			jinglian = (int)message.m_ints[0];
			protocol.game.cmsg_equip_jinlian _msg = new protocol.game.cmsg_equip_jinlian ();
			_msg.equip_guid = m_equip.guid;
			_msg.level = jinglian;
			net_http._instance.send_msg<protocol.game.cmsg_equip_jinlian> (opclient_t.CMSG_EQUIP_JINLIAN, _msg);
		}
	}
	// Update is called once per frame
	void Update () {
		if (m_play && isplay() )
		{
			m_play = false;
			sys._instance.remove_child(m_icon);
			reset(m_equip, m_out_message);
			
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
	}
}
