
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_jinglian_gui : MonoBehaviour,IMessage {

	dhc.treasure_t m_treasure;
	
	public GameObject m_effect;
	public GameObject m_center;
	public bool m_play = false;
	private string m_out_message;
	public List<GameObject> m_old_attr = new List<GameObject>();
	public List<GameObject> m_new_attr = new List<GameObject>();
	public GameObject m_icon1;
	public GameObject m_icon;
	public GameObject m_icon2;
	private ulong m_guid;
	public GameObject m_treasure_name;
	public GameObject m_treasure_jl;
	public GameObject m_need_gold;
	public List<ulong> m_guids = new List<ulong>();
	public List<int> m_gongzhens = new List<int>();
	public GameObject m_equip_skill_gui;
	public GameObject m_view;
	public GameObject m_jinglian;
	public GameObject m_skill_yl;
	public GameObject m_dacheng_des;
	uint m_stone_id = 50100001;
	public int flag = 0;
	
	public UILabel m_cur_sx;
	public UILabel m_next_sx;
	public UILabel m_desc;
	public UILabel m_xiaohao;

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
		if (m_treasure.role_guid != 0) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_treasure.role_guid);
			sys._instance.m_card = m_card;
			sys._instance.m_gongzhengs.Clear();
			sys._instance.m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		}
		m_dacheng_des.SetActive (false);
	}

	public void reset(dhc.treasure_t _treasure, string out_message)
	{
		flag = 0;
		bool sx = false;
		m_out_message = out_message;
		GameObject _main = m_center.transform.Find("main").gameObject;
		m_treasure = _treasure;
		if (m_treasure.role_guid != 0) 
		{
			ccard m_card = sys._instance.m_self.get_card_guid (m_treasure.role_guid);
			m_gongzhens.Clear();
			m_gongzhens = gongzheng_mubiao_gui.gongzhen_cur (m_card);
		}
		jl_guids ();
		sys._instance.remove_child (m_icon1);
		sys._instance.remove_child (m_icon);
		sys._instance.remove_child (m_icon2);

		GameObject icon1 = icon_manager._instance.create_treasure_icon(m_treasure.guid);
		icon1.transform.parent =  m_icon.transform;
		icon1.transform.localPosition = new Vector3(0,0,0);
		icon1.transform.localScale = new Vector3(1,1,1);
		icon1.transform.GetComponent<BoxCollider>().enabled = false;

		m_treasure_name.GetComponent<UILabel>().text = treasure.get_treasure_real_name (m_treasure.template_id);
		m_treasure_jl.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("treasure_jinglian_gui.cs_86_61"), m_treasure.jilian);//[{0}阶]
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		s_t_baowu_jl _treasure_jl = game_data._instance.get_t_baowu_jl (m_treasure.jilian);
		for(int i = 0; i < m_old_attr.Count;++i)
		{
			m_old_attr[i].SetActive(true);
			m_old_attr[i].GetComponent<UILabel>().text = treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian,i);
			if(treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian,i) == "")
			{
				sx = true;
				m_old_attr[i].SetActive(false);
			}
		}
		if(sx)
		{
			m_old_attr[0].transform.localPosition = new Vector3(-58,25,0);
		}
		else
		{
			m_old_attr[0].transform.localPosition = new Vector3(-58,46,0);
		}
		int stone_num = sys._instance.m_self.get_item_num(m_stone_id);
		s_t_baowu_jl _treasure_jl_next = game_data._instance.get_t_baowu_jl (m_treasure.jilian +1);
		if(_treasure_jl_next == null)
		{
			for(int i = 0; i < m_new_attr.Count;++i)
			{
				m_new_attr[i].SetActive(false);
			}
			m_need_gold.GetComponent<UILabel>().text = "--";
			m_icon2.transform.parent.localPosition = new Vector3(-75,-24,0);
			m_icon1.transform.parent.gameObject.SetActive(true);
			GameObject _icon1 = icon_manager._instance.create_item_icon_ex((int)m_stone_id,stone_num,-1);
			_icon1.transform.parent =  m_icon2.transform;
			_icon1.transform.localPosition = new Vector3(0,0,0);
			_icon1.transform.localScale = new Vector3(1,1,1);
			_icon1.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = _icon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_item_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";
			GameObject iicon1 = icon_manager._instance.create_treasure_icon(m_treasure.template_id,m_guids.Count,-1,false);
			iicon1.transform.parent =  m_icon1.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			iicon1.transform.GetComponent<BoxCollider>().enabled = false;
			flag = 1;
		}
		else
		{
			string cl = "";
			for(int i = 0; i < m_new_attr.Count;++i)
			{
				m_new_attr[i].SetActive(true);
				m_new_attr[i].GetComponent<UILabel>().text = treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian+1,i);
				if(treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian+1,i) == "")
				{
					sx = true;
					m_new_attr[i].SetActive(false);
				}
			}
			if(sx)
			{
                m_new_attr[0].transform.localPosition = new Vector3(98, 25, 0);
			}
			else
			{
                m_new_attr[0].transform.localPosition = new Vector3(98, 46, 0);
			}
			if(stone_num < _treasure_jl_next.stone)
			{
				cl = "[ff0000]";
				flag = 3;
			}
			cl = sys._instance.get_res_color(1);
			if(sys._instance.m_self.get_att(e_player_attr.player_gold) < _treasure_jl_next.cost)
			{
				cl = "[ff0000]";
				flag = 2;
			}
			m_need_gold.GetComponent<UILabel>().text = cl +  _treasure_jl_next.cost;
			cl = "";
			if(m_guids.Count < _treasure_jl_next.num)
			{
				cl = "[ff0000]";
				flag = 4;
			}
			if( _treasure_jl_next.num == 0)
			{
				m_icon1.transform.parent.gameObject.SetActive(false);
				m_icon2.transform.parent.localPosition = new Vector3(1,-24,0);
			}
			else
			{
				m_icon1.transform.parent.gameObject.SetActive(true);
				m_icon2.transform.parent.localPosition = new Vector3(-75,-24,0);
			}
			GameObject _icon1 = icon_manager._instance.create_item_icon_ex((int)m_stone_id,stone_num, _treasure_jl_next.stone);
			_icon1.transform.parent =  m_icon2.transform;
			_icon1.transform.localPosition = new Vector3(0,0,0);
			_icon1.transform.localScale = new Vector3(1,1,1);
			_icon1.transform.GetComponent<BoxCollider>().enabled = true;
			UIButtonMessage[] message = _icon1.transform.GetComponents<UIButtonMessage>();
			message[0].target = this.gameObject;
			message[0].functionName = "click_item_icon";
			message[1].target = null;
			message[1].functionName = "";
			message[2].target = null;
			message[2].functionName = "";

			GameObject iicon1 = icon_manager._instance.create_treasure_icon(m_treasure.template_id,m_guids.Count, _treasure_jl_next.num,false);
			iicon1.transform.parent =  m_icon1.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);
			iicon1.transform.GetComponent<BoxCollider>().enabled = false;
		}

		_main.transform.Find("jinglian").GetComponent<UIButton>().isEnabled = true;
		m_play = false;
		m_effect.gameObject.SetActive (false);
		if(t_treasure.font_color == 5)
		{
			m_skill_yl.SetActive(true);
		}
		else
		{
			m_skill_yl.SetActive(false);
		}
	}

	public void click_item_icon(GameObject obj)
	{
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)m_stone_id);
		cmessage_center._instance.add_message(message);
	}

	void jl_guids()
	{
		m_guids.Clear();
		for(int i = 0; i < sys._instance.m_self.m_t_player.treasures.Count;++i)
		{
			dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_guid(sys._instance.m_self.m_t_player.treasures[i]);
			if(_treasure.role_guid !=0 || _treasure.locked == 1 || _treasure.jilian != 0 || _treasure.enhance != 0 || m_treasure.guid  == sys._instance.m_self.m_t_player.treasures[i]
			   || _treasure.star > 0 || _treasure.star_exp > 0)
			{
				continue;
			}
			if(_treasure.template_id == m_treasure.template_id)
			{
				m_guids.Add(sys._instance.m_self.m_t_player.treasures[i]);
			}
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
            _msg.m_ints.Add(5);
			_msg.m_long.Add(m_treasure.guid);
			cmessage_center._instance.add_message(_msg);
		}
		if(obj.name == "jinglian")
		{
			if(flag == 1)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("treasure_jinglian_gui.cs_276_59"));//该饰品已精炼至满阶
				return;
			}
			if(flag == 2)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
				return;
			}
			if(flag == 4)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("treasure_jinglian_gui.cs_286_59"));//饰品精炼需要的材料不足
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
			s_t_baowu _treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
			s_t_baowu_jl _baowu_jl= game_data._instance.get_t_baowu_jl(m_treasure.jilian+1);
			protocol.game.cmsg_treasure_jinlian _msg = new protocol.game.cmsg_treasure_jinlian ();
			for(int i = 0 ; i < _baowu_jl.num;++i)
			{
				_msg.treasure_guid.Add(m_guids[i]);
			}
			_msg.jinlian_guid = m_treasure.guid;
			net_http._instance.send_msg<protocol.game.cmsg_treasure_jinlian> (opclient_t.CMSG_TREASURE_JINLIAN, _msg);
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
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		for(int i = 0 ; i < game_data._instance.m_dbc_equip_skill.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_equip_skill.get(0,i));
			s_t_equip_skill t_equip_skill = game_data._instance.get_t_equip_skill (id);
			if(t_equip_skill.part == t_treasure.type + 4)
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
			active.transform.GetComponent<equip_skill_sub>().jinlian = m_treasure.jilian;
			active.transform.GetComponent<equip_skill_sub>().reset();
			sys._instance.add_pos_anim(active,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(active,0.3f, 0, 1.0f, i * 0.05f);
		}
		m_jinglian.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_jl_gui.cs_345_59") , m_treasure.jilian );//当前精炼阶段数:{0}阶
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_TREASURE_JINLIAN)
		{
			protocol.game.smsg_treasure_jinlian _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_jinlian> (message.m_byte);
			s_t_baowu _treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
			s_t_baowu_jl _baowu_jl= game_data._instance.get_t_baowu_jl(m_treasure.jilian+1);
			sys._instance.m_self.sub_att(e_player_attr.player_gold, _baowu_jl.cost,game_data._instance.get_t_language ("treasure_jinglian_gui.cs_393_74"));//宝物精炼消耗
            sys._instance.m_self.remove_item(m_stone_id, _baowu_jl.stone, game_data._instance.get_t_language ("treasure_jinglian_gui.cs_393_74"));//宝物精炼消耗
			sys._instance.m_self.m_t_player.bwjl_task_num ++;
			for(int i = 0 ; i < _baowu_jl.num;++i)
			{
				sys._instance.m_self.remove_treasure(m_guids[i]);
			}
			m_treasure.jilian +=1;
			if(m_treasure.role_guid != 0)
			{
				ccard m_card = sys._instance.m_self.get_card_guid(m_treasure.role_guid);
				string s = sys._instance.m_self.gongzhengs_ex(m_card,m_gongzhens);
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
			
			_main.transform.Find("jinglian").GetComponent<UIButton>().isEnabled = false;
			if(_treasure.font_color == 5)
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
		s_t_baowu t_treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		for(int i = 0 ; i < game_data._instance.m_dbc_equip_skill.get_y();++i)
		{
			int id = int.Parse(game_data._instance.m_dbc_equip_skill.get(0,i));
			s_t_equip_skill t_equip_skill = game_data._instance.get_t_equip_skill (id);
			if(t_equip_skill.part == t_treasure.type + 4)
			{
				equip_skills.Add(t_equip_skill);
			}
		}
		for(int i = 0; i < equip_skills.Count;++i)
		{
			if(m_treasure.jilian == equip_skills[i].jinglian)
			{ 
				flag = true;
				text = string.Format(game_data._instance.get_t_language ("equip_jl_gui.cs_417_25") ,equip_skills[i].name );//[ffffff]圣器技能[-][f9d420]{0}[-][ffffff]开启[-]
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


	}
	// Update is called once per frame
	void Update () {
		if (m_play && isplay() )
		{
			m_play = false;
			sys._instance.remove_child(m_icon);
			reset(m_treasure, m_out_message);
			
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
		}
		
	}
}
