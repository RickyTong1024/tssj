
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_detail : MonoBehaviour,IMessage {

	public List<dhc.treasure_t> m_t_treasures = new List<dhc.treasure_t>();
	private dhc.treasure_t m_treasure;
	private int m_type;
	public GameObject m_name;
	public GameObject m_qname;
	public GameObject m_Lv;
	public GameObject m_jl;
	public GameObject m_attr;
	public GameObject m_attr1;
	public GameObject m_part;
	public List<GameObject> m_jl_attrs = new List<GameObject>();
	public GameObject m_locked;
	public GameObject m_qianghua;
	public GameObject m_locked1;
	public GameObject m_icon;
	public GameObject m_tihuan;
	public GameObject m_jinglian;
	public GameObject m_lock;
	public GameObject m_next;
	public GameObject m_remove;
	public GameObject effect;
	public GameObject m_jl_effect;
	public GameObject[] star;
	public GameObject m_frame_big;
	public GameObject m_equip_skill_sprie;
	public GameObject m_sx_sprite;
	public GameObject m_desc;
	public GameObject m_down;
	public GameObject m_scro;
	public GameObject m_zhuzao;
	public GameObject m_star1;
	public GameObject m_star2;
	public GameObject m_sx_attr;
	public bool m_flag = false;

	public UILabel m_qsx_Label;
	public UILabel m_sjc_Label;
	public UILabel m_enhance_Label;
	public UILabel m_tihuan_Label;
	public UILabel m_title;

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void reset(dhc.treasure_t _treasure, int type)
	{
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0); 
		m_type = type;
		m_treasure = _treasure;
		bool flag = false;
		if(m_flag)
		{
			m_remove.SetActive(true);
		}
		else
		{
			m_remove.SetActive(false);
		}
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
		m_name.GetComponent<UILabel>().text = treasure.get_treasure_real_name (m_treasure.template_id);
		int enhance_up = game_data._instance.m_dbc_treasure_enhance.get_y() - 1;
		int jl_up = game_data._instance.m_dbc_baowu_jl.get_y ();
		m_Lv.GetComponent<UILabel>().text =  m_treasure.enhance.ToString() + "/" + enhance_up;
		m_jl.GetComponent<UILabel>().text =  (m_treasure.jilian).ToString() + "/"+ jl_up;
		m_part.GetComponent<UILabel>().text = treasure.get_treasure_part (m_treasure.template_id);
		m_attr.GetComponent<UILabel>().text = treasure.get_treasure_value1(m_treasure.template_id,m_treasure.enhance,m_treasure.jilian);
		m_attr1.GetComponent<UILabel>().text = treasure.get_treasure_value2(m_treasure.template_id, m_treasure.enhance,m_treasure.jilian);
		for(int i = 0; i < m_jl_attrs.Count;++i)
		{
			m_jl_attrs[i].SetActive(true);
			m_jl_attrs[i].GetComponent<UILabel>().text = treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian,i);
			if(treasure.get_treasure_jl_text(m_treasure,m_treasure.jilian,i) == "")
			{
				flag = true;
				m_jl_attrs[i].SetActive(false);
			}
		}
		if (m_treasure.locked == 1)
		{
			m_locked1.GetComponent<UILabel>().text = game_data._instance.get_t_language ("treasure_detail.cs_98_45");//解锁
		}
		else
		{
			m_locked1.GetComponent<UILabel>().text = game_data._instance.get_t_language ("treasure_detail.cs_102_45");//锁定
		}
		sys._instance.remove_child (m_icon);
		GameObject iicon = icon_manager._instance.create_treasure_icon(m_treasure.guid);
		iicon.transform.parent = m_icon.transform;
		iicon.transform.localPosition = new Vector3(0,0,0);
		iicon.transform.localScale = new Vector3(1.15f,1.15f,1.15f);
		iicon.transform.GetComponent<BoxCollider>().enabled = false;
		if (m_type == 0)
		{
			m_locked.SetActive(true);
			m_tihuan.gameObject.SetActive(false);
		}
		else if(m_type ==3)
		{
			m_locked.SetActive(false);
			m_tihuan.gameObject.SetActive(false);
			m_frame_big.GetComponent<UISprite>().height = 515;
			m_frame_big.GetComponent<frame>().m_height = 515;
			m_frame_big.transform.localPosition = new Vector3(0,-1,0);
		}
		else
		{
			m_tihuan.gameObject.SetActive(true);
			m_locked.gameObject.SetActive(false);
			m_frame_big.GetComponent<UISprite>().height = 566;
			m_frame_big.GetComponent<frame>().m_height = 566;
			m_frame_big.transform.localPosition = new Vector3(0,-27,0);
		}

		if (m_treasure.locked == 1)
		{
			m_lock.SetActive(true);
		}
		else
		{
			m_lock.SetActive(false);
		}
		if(treasure.is_enhance(m_treasure))
		{
			effect.SetActive(true);
		}
		else
		{
			effect.SetActive(false);
		}
		if(treasure.is_jinglian(m_treasure))
		{
			m_jl_effect.SetActive(true);
		}
		else
		{
			m_jl_effect.SetActive(false);
		}
		if((sys._instance.m_self.m_t_player.level < (int)e_open_see.es_baowu_sx) || (t_treasure.font_color != 4 && t_treasure.font_color != 5))
		{
			m_sx_sprite.SetActive(false);
			if(flag)
			{
				m_sx_sprite.transform.localPosition = new Vector3(0,-165,0);
			}
			else
			{
				m_sx_sprite.transform.localPosition = new Vector3(0,-199,0);
			}
		}
		else
		{
			m_sx_sprite.SetActive(true);
			if(flag)
			{
				m_sx_sprite.transform.localPosition = new Vector3(0,-308,0);
			}
			else
			{
				m_sx_sprite.transform.localPosition = new Vector3(0,-342,0);
			}
		}
		if(t_treasure.font_color == 5)
		{
			m_desc.GetComponent<UILabel>().text = equip.equip_skill(t_treasure.type + 4,m_treasure.jilian);
			m_equip_skill_sprie.SetActive(true);
			m_down.SetActive(true);
		}
		else
		{
			m_equip_skill_sprie.SetActive(false);
			m_down.SetActive(false);
		}
		if(t_treasure.font_color == 4 && sys._instance.m_self.m_t_player.level >= (int)e_open_see.es_treasure_zhuzao)
		{
			m_zhuzao.SetActive(true);
		}
		else
		{
			m_zhuzao.SetActive(false);
		}
		int sx_num = 0;
		for(int j = 0;j < game_data._instance.m_dbc_baowu_sx.get_y();++j)
		{
			if(int.Parse (game_data._instance.m_dbc_baowu_sx.get (1, j)) == t_treasure.font_color)
			{
				sx_num++;
			}
		}
		if(sx_num > 0)
		{
			m_star1.GetComponent<UISprite>().width = 37*sx_num;
			m_star2.GetComponent<UISprite>().width = 18*m_treasure.star;
			m_sx_attr.GetComponent<UILabel>().text = treasure.get_baowu_sx_text(m_treasure,m_treasure.star);
		}
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_TREASURE_LOCK)
		{
			m_treasure.locked = 1 - m_treasure.locked;
			reset(m_treasure, m_type);
			
			/*s_message msg = new s_message();
			msg.m_type = "show_treasure_gui2";
			msg.m_long.Add(m_treasure.guid);
			cmessage_center._instance.add_message(msg);*/
		}
	}
	void IMessage.message(s_message message)
	{
		
	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "locked")
		{
			protocol.game.cmsg_treasure_lock _msg = new protocol.game.cmsg_treasure_lock ();
			_msg.treasure_guid = m_treasure.guid;
			if(m_locked1.GetComponent<UILabel>().text == game_data._instance.get_t_language ("treasure_detail.cs_102_45"))//锁定
			{
				_msg.locked = true;
			}
			else
			{
				_msg.locked = false;
			}
			net_http._instance.send_msg<protocol.game.cmsg_treasure_lock> (opclient_t.CMSG_TREASURE_LOCK, _msg);
		}
		else if (obj.name == "ok")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "remove")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _msg = new s_message();
			_msg.m_type = "common_select_treasure";
			_msg.m_long.Add ((ulong)0);
			cmessage_center._instance.add_message(_msg);
			if(m_next != null)
			{
				m_next.SetActive(true);
			}
		}
		else if (obj.name == "qianghua")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasue_qh)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("treasure_detail.cs_270_73") , (int)e_open_level.el_treasue_qh ));//饰品强化{0}级开启
				return;
			}
			s_message msg = new s_message();
			msg.m_type = "show_treasure_qianghua_gui";
			msg.m_long.Add(m_treasure.guid);
			for(int i =0 ; i < m_t_treasures.Count;++i)
			{
				msg.m_long.Add(m_t_treasures[i].guid);
			}
			if (m_type == 0)
			{
				msg.m_string.Add("show_treasure_gui2");
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
				msg1.m_type = "hide_treasure_gui";
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
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_jl)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("treasure_detail.cs_319_73"), (int)e_open_level.el_treasure_jl ));//饰品精炼{0}级开启
				return;
			}
			s_t_baowu_jl _treasure_jl_next = game_data._instance.get_t_baowu_jl (m_treasure.jilian +1);
			if(_treasure_jl_next == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_detail.cs_325_46"));//[ffc882]该饰品已精炼至满阶
				return;
			}
			s_message msg = new s_message();
			msg.m_type = "show_jianglian_gui";
			msg.m_long.Add(m_treasure.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_treasure_gui2");
			}
			else if (m_type == 1)
			{
				msg.m_string.Add("show_buzheng2");
			}
			else if (m_type == 3)
			{
                msg.m_string.Add("show_bag1");
			}
			cmessage_center._instance.add_message(msg);
			
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_treasure_gui";
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
		else if (obj.name == "cast")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_zhuzao)
			{
				root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("treasure_detail.cs_370_60") , (int)e_open_level.el_treasure_zhuzao ));//[ffc882]饰品铸造{0}级开启
				return;
			}
			if(m_treasure.star_exp > 0 || m_treasure.star > 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_detail.cs_375_46"));//[ffc882]升过星的橙色饰品不能铸造
				return;
			}
			s_message msg = new s_message();
			msg.m_type = "show_zhuzao_gui";
			msg.m_long.Add(m_treasure.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_treasure_gui2");
			}
			else if (m_type == 1)
			{
				msg.m_string.Add("show_buzheng2");
			}
			else if (m_type == 3)
			{
				msg.m_string.Add("show_bag_gui_treasure");
			}
			cmessage_center._instance.add_message(msg);
			
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_treasure_gui";
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
		else if(obj.name == "sx")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_baowu_sx)
			{
				root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("treasure_detail.cs_420_60") , (int)e_open_level.el_baowu_sx ));//[ffc882]饰品升星{0}级开启
				return;
			}
			s_t_baowu t_treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
			s_t_baowu_sx _treasure_sx_next = game_data._instance.get_t_baowu_sx (m_treasure.star+1,t_treasure.font_color);
			if(_treasure_sx_next == null)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_detail.cs_427_46"));//[ffc882]该饰品已升至满星
				return;
			}
			s_message msg = new s_message();
			msg.m_type = "show_treasure_sx_gui";
			msg.m_long.Add(m_treasure.guid);
			if (m_type == 0)
			{
				msg.m_string.Add("show_treasure_gui2");
			}
			else if (m_type == 1)
			{
				msg.m_string.Add("show_buzheng2");
			}
			else if (m_type == 3)
			{
                msg.m_string.Add("show_bag1");
			}
			cmessage_center._instance.add_message(msg);
			
			if (m_type == 0)
			{
				s_message msg1 = new s_message();
				msg1.m_type = "hide_treasure_gui";
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
			msg1.m_type = "common_treasure_tihuan";
			cmessage_center._instance.add_message(msg1);
		}
	}
}
