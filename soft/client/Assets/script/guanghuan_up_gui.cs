
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class guanghuan_up_gui : MonoBehaviour,IMessage{

	public s_t_guanghuan t_guanghuan = null;
	public GameObject m_sj;
	public GameObject m_skill_scro;
	public GameObject m_guanghuan_now_level;
	public GameObject m_guanghuan_next_level;
	public GameObject m_skill_name;
	public GameObject m_skill_desc;
	public GameObject m_gold;
	public GameObject m_cl_icon;
	public GameObject m_skill_attr1;
	public GameObject m_skill_attr1_up;

	private string error = "";
	private uint cl_id = 50150001;

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}


	public void click(GameObject obj)
	{
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
			
			protocol.game.cmsg_guanghuan_level _msg = new protocol.game.cmsg_guanghuan_level ();
			_msg.id = t_guanghuan.id;
			net_http._instance.send_msg<protocol.game.cmsg_guanghuan_level> (opclient_t.CMSG_GUANGHUAN_LEVEL, _msg);
		}
		if(obj.transform.name == "up_close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	public void click_item_icon(GameObject obj)
	{
		s_message message = new s_message ();
		message.m_type = "show_cl_gui";
		message.m_ints.Add ((int)cl_id);
		cmessage_center._instance.add_message(message);
	}

	public void reset_skill()
	{
		error = "";
		if(sys._instance.m_self.m_t_player.guanghuan.Contains(t_guanghuan.id))
		{
			m_sj.GetComponent<BoxCollider>().enabled = true;
			m_sj.GetComponent<UISprite>().set_enable(true);
		}
		else
		{
			m_sj.GetComponent<BoxCollider>().enabled = false;
			m_sj.GetComponent<UISprite>().set_enable(false);
		}
		if(m_skill_scro.GetComponent<SpringPanel>() != null)
		{
			m_skill_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_skill_scro.GetComponent<UIPanel>().clipOffset = new Vector2 (0, 0);
		m_skill_scro.transform.localPosition = new Vector2 (0, 0);
		sys._instance.remove_child(m_skill_scro);
		List<s_t_guanghuan_skill> guanghuan_skills = new List<s_t_guanghuan_skill>();
		foreach(int id in game_data._instance.m_dbc_guanghuan_skill.m_index.Keys)
		{
			s_t_guanghuan_skill t_guanghuan_skill = game_data._instance.get_t_guanghuan_skill(id);
			if(t_guanghuan_skill.wing_id == t_guanghuan.id)
			{
				guanghuan_skills.Add(t_guanghuan_skill);
			}
		}
		for(int i = 0;i < guanghuan_skills.Count;++i)
		{
			GameObject obj = game_data._instance.ins_object_res("ui/guanghuan_up_item");
			obj.transform.parent = m_skill_scro.transform;
			obj.transform.localPosition = new Vector3(-202, 112 - 130*i,0);
			obj.transform.localScale = new Vector3(1,1,1);
			obj.transform.GetComponent<guanghuan_up_item>().t_guanghuan_skill = guanghuan_skills[i];
			obj.transform.GetComponent<guanghuan_up_item>().reset();
		}
		m_guanghuan_now_level.GetComponent<UILabel>().text = guanghuan_gui.guanghuan_level(t_guanghuan).ToString();
		m_guanghuan_next_level.GetComponent<UILabel>().text = "----";
		m_skill_name.GetComponent<UILabel>().text = "----";
		m_skill_desc.GetComponent<UILabel>().text = game_data._instance.get_t_language ("guanghuan_up_gui.cs_110_46");//已开启全部光环技能
		if(guanghuan_skills.Count == 0)
		{
			m_skill_name.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bu_zheng_panel.cs_2702_63");//无技能
			m_skill_desc.GetComponent<UILabel>().text = "";
		}
		m_gold.GetComponent<UILabel>().text = "--";
		sys._instance.remove_child (m_cl_icon);
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
		for(int i = 0;i < guanghuan_skills.Count;++i)
		{
			s_t_guanghuan_skill t_guanghuan_skill = guanghuan_skills[i];
			if(guanghuan_gui.guanghuan_level(t_guanghuan) < t_guanghuan_skill.enhance)
			{
				m_skill_name.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("guanghuan_up_gui.cs_135_62") ,t_guanghuan_skill.name,t_guanghuan_skill.enhance);//{0}[0aabff]({1}级开启)[-]
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
				m_skill_desc.GetComponent<UILabel>().text = desc;
				break;
			}
		}
		int level = guanghuan_gui.guanghuan_level (t_guanghuan);
		s_t_guanghuan_enhance t_guanghuan_enhance = game_data._instance.get_t_guanghuan_enhance(level+1);
		m_skill_attr1.GetComponent<UILabel>().text = guanghuan_gui.get_attr (t_guanghuan.id, guanghuan_gui.guanghuan_level (t_guanghuan), 0);
		m_skill_attr1_up.GetComponent<UILabel>().text = "----";
		if(t_guanghuan_enhance != null)
		{
			m_guanghuan_next_level.GetComponent<UILabel>().text = (guanghuan_gui.guanghuan_level(t_guanghuan) +1).ToString();
			int gold = t_guanghuan_enhance.golds[t_guanghuan.color -2];
			m_gold.GetComponent<UILabel>().text = sys._instance.get_res_color(1) +gold;
			if(sys._instance.m_self.m_t_player.gold <gold)
			{
				m_gold.GetComponent<UILabel>().text = "[ff0000]" + gold;
				error = game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
			}
			int cl = t_guanghuan_enhance.yuansus[t_guanghuan.color -2];
			if(num < cl)
			{
				error = "0";
			}
			sys._instance.remove_child(m_cl_icon);
			GameObject iicon = icon_manager._instance.create_item_icon_ex((int)cl_id,num,cl);
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
			m_skill_attr1_up.GetComponent<UILabel>().text = guanghuan_gui.get_attr (t_guanghuan.id,guanghuan_gui.guanghuan_level (t_guanghuan) +1, 0).Split('+')[1];
		}
		else
		{
			error = game_data._instance.get_t_language ("guanghuan_up_gui.cs_185_11");//光环已升至满级
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_GUANGHUAN_LEVEL)
		{
			s_t_guanghuan_enhance t_guanghuan_enhance = game_data._instance.get_t_guanghuan_enhance(guanghuan_gui.guanghuan_level(t_guanghuan)+1);
			int cl = t_guanghuan_enhance.yuansus[t_guanghuan.color -2];
			sys._instance.m_self.remove_item(cl_id,cl,game_data._instance.get_t_language ("guanghuan_up_gui.cs_195_45"));//光环升级消耗
			int gold = t_guanghuan_enhance.golds[t_guanghuan.color -2];
			sys._instance.m_self.sub_att(e_player_attr.player_gold,gold,game_data._instance.get_t_language ("guanghuan_up_gui.cs_195_45"));//光环升级消耗
			for(int i = 0; i < sys._instance.m_self.m_t_player.guanghuan.Count;++i)
			{
				if(sys._instance.m_self.m_t_player.guanghuan[i] == t_guanghuan.id)
				{
					sys._instance.m_self.m_t_player.guanghuan_level[i] += 1;
					break;
				}
			}
			s_message _message2 = new s_message();
			_message2.m_type = "check_bf";
			cmessage_center._instance.add_message(_message2);
			
			sys._instance.play_sound_ex("sound/skill_up");
			List<s_t_guanghuan_skill> guanghuan_skills = new List<s_t_guanghuan_skill>();
			foreach(int id in game_data._instance.m_dbc_guanghuan_skill.m_index.Keys)
			{
				s_t_guanghuan_skill t_guanghuan_skill = game_data._instance.get_t_guanghuan_skill(id);
				if(t_guanghuan_skill.wing_id == t_guanghuan.id)
				{
					guanghuan_skills.Add(t_guanghuan_skill);
				}
			}
			for(int i = 0;i < guanghuan_skills.Count;++i)
			{
				s_t_guanghuan_skill t_guanghuan_skill = guanghuan_skills[i];
				if(t_guanghuan_skill.enhance == guanghuan_gui.guanghuan_level(t_guanghuan))
				{
					root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("duixing_gui.cs_543_61"),t_guanghuan_skill.name));//开启[00ff00]{0}[-]技能
					break;
				}
			}
			guanghuan_gui.sxs = guanghuan_gui.sx ();
			reset_skill();
			s_message _message = new s_message();
			_message.m_type = "updte_guanghuan_gui";
		    _message.m_ints.Add(t_guanghuan.id);
			cmessage_center._instance.add_message(_message);

			s_message _message1 = new s_message();
			_message1.m_type = "updte_bz_guanghuan_gui";
			cmessage_center._instance.add_message(_message1);
		}

	}
	
	void IMessage.message(s_message message)
	{

	}


	// Update is called once per frame
	void Update () {
	
	}
}
