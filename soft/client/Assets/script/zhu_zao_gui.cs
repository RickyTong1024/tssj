
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class zhu_zao_gui : MonoBehaviour,IMessage {

	dhc.treasure_t m_treasure;
	private string m_out_message;
	public GameObject m_old_icon;
	public GameObject m_new_icon;
	public GameObject m_old_qattr;
	public GameObject m_old_qattr_num;
	public GameObject m_old_jattr;
	public GameObject m_old_jattr_num;
	public GameObject m_new_qattr;
	public GameObject m_new_qattr_num;
	public GameObject m_new_jattr;
	public GameObject m_new_jattr_num;
	public GameObject m_jewel;
	public GameObject m_old_name;
	public GameObject m_new_name;
	public GameObject m_jt;
	public GameObject m_zhuzao;
	private string error = "";

	float m_jt_x = 0;
	int toltal_exp= 0;

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void reset(dhc.treasure_t t_treasure, string out_message,bool flag = true) 
	{
		error = "";
		m_treasure = t_treasure;
		m_out_message = out_message;
		if (m_treasure.role_guid != 0) 
		{
			sys._instance.m_sxs.Clear ();
			ccard m_card = sys._instance.m_self.get_card_guid (m_treasure.role_guid);
			sys._instance.m_sxs = sys._instance.m_self.get_gongzhen(m_card);
			sys._instance.m_card = m_card;
		}
		s_t_baowu _treasure = game_data._instance.get_t_baowu (m_treasure.template_id);
		toltal_exp = game_data._instance.get_total_treasure_enhance (m_treasure.enhance,_treasure.font_color)+ m_treasure.enhance_exp;
		sys._instance.remove_child (m_old_icon);
		GameObject _icon = icon_manager._instance.create_treasure_icon(m_treasure);
		_icon.transform.parent = m_old_icon.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		_icon.transform.GetComponent<BoxCollider>().enabled = false;
		m_old_name.GetComponent<UILabel>().text = treasure.get_treasure_real_name (m_treasure.template_id);
		List<string> text = new List<string>();
		text = treasure.get_treasure_attr (m_treasure.template_id, m_treasure.enhance);
		string s = "";
		s += game_data._instance.get_t_language ("zhu_zao_gui.cs_62_7");//强化等级
		for(int i = 0;i < text.Count;++i)
		{
			s += "\n" + text[i].Split('+')[0]; 
		}
		m_old_qattr.GetComponent<UILabel>().text = s;
		s = "";
		s += m_treasure.enhance.ToString ();
		for(int i = 0;i < text.Count;++i)
		{
			s += "\n" + "[0aff16]+"+ text[i].Split('+')[1]; 
		}
		m_old_qattr_num.GetComponent<UILabel>().text = s;
		s = "";
		s += game_data._instance.get_t_language ("zhu_zao_gui.cs_76_7");//精炼阶数
		for(int i = 0; i < 3;++i)
		{
			if(treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian,i) !="")
			{
				s += "\n" + treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian,i).Split('+')[0];
			}
		}
		m_old_jattr.GetComponent<UILabel>().text = s;
		s = "";
		s += string.Format(game_data._instance.get_t_language ("zhu_zao_gui.cs_86_21"),m_treasure.jilian.ToString ());//{0}阶
		for(int i = 0; i < 3;++i)
		{
			if(treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian,i) !="")
			{
				s += "\n" + "[0aff16]+" + treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian,i).Split('+')[1];
			}
		}
		m_old_jattr_num.GetComponent<UILabel>().text = s;
		if(flag)
		{
			dhc.treasure_t new_treasure = new dhc.treasure_t();
			new_treasure.enhance_counts = m_treasure.enhance_counts;
			new_treasure.guid = m_treasure.guid;
			new_treasure.jilian = m_treasure.jilian;
			new_treasure.locked = m_treasure.locked;
			new_treasure.player_guid = m_treasure.player_guid;
			new_treasure.role_guid = m_treasure.role_guid;
			new_treasure.template_id = m_treasure.template_id + 1000;
			level_num (new_treasure);
			_treasure = game_data._instance.get_t_baowu (new_treasure.template_id);
			sys._instance.remove_child (m_new_icon);
			GameObject _icon1 = icon_manager._instance.create_treasure_icon(new_treasure);
			_icon1.transform.parent = m_new_icon.transform;
			_icon1.transform.localPosition = new Vector3(0,0,0);
			_icon1.transform.localScale = new Vector3(1,1,1);
			_icon1.transform.GetComponent<BoxCollider>().enabled = false;
			m_new_name.GetComponent<UILabel>().text = treasure.get_treasure_real_name (new_treasure.template_id);
			text.Clear ();
			text = treasure.get_treasure_attr (new_treasure.template_id, new_treasure.enhance);
			s = "";
			s += game_data._instance.get_t_language ("zhu_zao_gui.cs_62_7");//强化等级
			for(int i = 0;i < text.Count;++i)
			{
				s += "\n" + text[i].Split('+')[0]; 
			}
			m_new_qattr.GetComponent<UILabel>().text = s;
			s = "";
			s += new_treasure.enhance.ToString ();
			for(int i = 0;i < text.Count;++i)
			{
				s += "\n" + "[0aff16]+" + text[i].Split('+')[1]; 
			}
			m_new_qattr_num.GetComponent<UILabel>().text = s;
			s = "";
			s += game_data._instance.get_t_language ("zhu_zao_gui.cs_76_7");//精炼阶数
			for(int i = 0; i < 3;++i)
			{
				if(treasure.get_treasure_jl_text(new_treasure,new_treasure.jilian,i) != "")
				{
					s += "\n" + treasure.get_treasure_jl_text(new_treasure,new_treasure.jilian,i).Split('+')[0];
				}
			}
			m_new_jattr.GetComponent<UILabel>().text = s;
			s = "";
			s += string.Format(game_data._instance.get_t_language ("zhu_zao_gui.cs_86_21"), new_treasure.jilian);//{0}阶
			for(int i = 0; i < 3;++i)
			{
				if(treasure.get_treasure_jl_text(new_treasure,new_treasure.jilian,i) != "")
				{
					s += "\n" + "[0aff16]+" + treasure.get_treasure_jl_text(new_treasure,new_treasure.jilian,i).Split('+')[1];
				}
			}
			m_new_jattr_num.GetComponent<UILabel>().text = s;
			int treasure_num = 0;
			for(int i = 0; i <= m_treasure.jilian;++i)
			{
				s_t_baowu_jl _t_baowu_jl = game_data._instance.get_t_baowu_jl(i);
				if(_t_baowu_jl != null)
				{
					treasure_num += _t_baowu_jl.num;
				}
			}
			m_jewel.GetComponent<UILabel>().text = sys._instance.get_res_color (2) + (550 + 550 * treasure_num).ToString();
			if(sys._instance.m_self.m_t_player.jewel < 550 + 550 *treasure_num)
			{
				error = game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				m_jewel.GetComponent<UILabel>().text = "[ff0000]" + (550 + 550 * treasure_num).ToString();
			}
		}
		m_zhuzao.GetComponent<UIButton>().isEnabled = flag;
	}

	void level_num(dhc.treasure_t m_t_treasure)
	{
		int num = 0;
		int value = 1;
		s_t_baowu _treasure = game_data._instance.get_t_baowu (m_t_treasure.template_id);
		int exp = game_data._instance.get_treasure_enhance(value,_treasure.font_color);
		int max_enchance = Mathf.Min(int.Parse(game_data._instance.m_dbc_enhance.get_index(0, game_data._instance.m_dbc_enhance.get_y() - 1)),sys._instance.m_self.m_t_player.level);
		while(toltal_exp >= exp)
		{
			value = value + 1;
			if(num >= max_enchance)
			{
				m_t_treasure.enhance_exp = 0;
				break;
			}
			num ++;
			toltal_exp -=  exp;
			exp = game_data._instance.get_treasure_enhance(value,_treasure.font_color);
		}
		m_t_treasure.enhance = num;
		m_t_treasure.enhance_exp = toltal_exp;
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_TREASURE_ZHUZAO)
		{
			protocol.game.smsg_treasure_zhuzao _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_zhuzao> (message.m_byte);
			int treasure_num = 0;
			for(int i = 0; i <= m_treasure.jilian;++i)
			{
				s_t_baowu_jl _t_baowu_jl = game_data._instance.get_t_baowu_jl(i);
				if(_t_baowu_jl != null)
				{
					treasure_num += _t_baowu_jl.num;
				}
			}
			sys._instance.m_self.sub_att(e_player_attr.player_jewel,550 + 550 * treasure_num,game_data._instance.get_t_language ("zhu_zao_gui.cs_207_84"));//宝物铸造消耗
			m_treasure.template_id += 1000;
			m_treasure.enhance = _msg.level;
			m_treasure.enhance_exp = _msg.exp;
			reset(m_treasure,m_out_message,false);
			m_jewel.GetComponent<UILabel>().text = "--";
			s_message _message3 = new s_message();
			_message3.m_type = "check_bf";
			cmessage_center._instance.add_message(_message3);
		}
	}

	void IMessage.message(s_message message)
	{
	}

	public void click(GameObject obj)
	{
		if (obj.name == "close")
		{
			s_message _msg = new s_message();
			_msg.m_type = m_out_message;
			cmessage_center._instance.add_message(_msg);
			Object.Destroy(this.gameObject);
		}
		if(obj.name == "zhuzao")
		{
			if(error!= "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]"+ error);
				return;
			}
			protocol.game.cmsg_treasure_zhuzao _msg = new protocol.game.cmsg_treasure_zhuzao ();
			_msg.treasure_guid = m_treasure.guid;
			net_http._instance.send_msg<protocol.game.cmsg_treasure_zhuzao> (opclient_t.CMSG_TREASURE_ZHUZAO, _msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
		m_jt_x += Time.deltaTime * 100;
		if (m_jt_x > 60)
		{
			m_jt_x -= (int)m_jt_x / 60 * 60;
		}
		m_jt.transform.localPosition = new Vector3 (m_jt_x, 0, 0);
	}
}
