
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hb_dress_gui_ex : MonoBehaviour,IMessage {


	public ulong m_guid;
	private ccard m_card;
	public GameObject m_taozhuang_gui;
	public GameObject m_hb_dress_page_gui;
	public GameObject m_hb_dress_detail;
	public GameObject m_icon;
	public GameObject m_attr1;
	public GameObject m_name;
	public UIToggle m_shizhuang_toggle;
	private int m_dir = 1;
	private List<GameObject> m_hb_dress_item = new List<GameObject>();
	List<s_t_role_dress> m_role_dresss = new List<s_t_role_dress>();
	private Vector3 m_mouse_pos;
	private s_t_role_dress role_dress;
	private bool m_need = false;
	public string m_message = "";


	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}


	public void shizhuang()
	{
		init ();
		update_ui ();
	}

	public void taozhaung()
	{
		m_hb_dress_page_gui.SetActive (false);
		m_taozhuang_gui.SetActive (true);
		m_taozhuang_gui.GetComponent<hb_taozhuang_gui>().reset ();
	}

	public void init()
	{
		m_hb_dress_page_gui.SetActive (true);
		m_taozhuang_gui.SetActive (false);
		m_role_dresss.Clear ();
		
		if(m_hb_dress_item.Count == 0)
		{
			for(int y = 0;y < 2;y ++)
			{
				for(int x = 0;x < 4;x ++)
				{
					GameObject _item = game_data._instance.ins_object_res("ui/hb_dress_page_item");
					_item.name = (y * 4 + x).ToString();
					_item.transform.parent = m_hb_dress_page_gui.transform;
					_item.transform.localPosition = new Vector3(x * 210f - 310f,- y * 240f + 110f,0);
					_item.transform.localScale = new Vector3(1,1,1);
					_item.SetActive(true);
					
					m_hb_dress_item.Add(_item);
				}
			}
		}
	}

	public static int comp(s_t_role_dress x,s_t_role_dress y)
	{
		int x_need = 0;
		int y_need = 0;
		ccard _card1 = sys._instance.m_self.get_card_id (x.role);
		ccard _card2 = sys._instance.m_self.get_card_id (y.role);
		
		if (sys._instance.m_self.has_role_dress (_card1.get_guid(), x.id)) 
		{	
			x_need = 1;
		}
		if (sys._instance.m_self.has_role_dress (_card2.get_guid(),y.id))
		{
			y_need = 1;		
		}
		if (sys._instance.m_self.has_role_dress_on (_card1.get_guid(),x.id))
		{
			x_need = 2;
		}	
		if (sys._instance.m_self.has_role_dress_on (_card2.get_guid(),y.id))
		{
			y_need = 2;		
		}
		if (x_need != y_need) 
		{
			return  y_need - x_need;
		}
		return x.id - y.id;
	}
	
	
	public void reset(ulong guid)
	{
		m_guid = guid;
		update_ui ();
	}

	public void update_ui()
	{
		for(int i = 0;i < 8;i ++)
		{
			hb_dress_page_item _item = m_hb_dress_item[i].GetComponent<hb_dress_page_item>();
			_item.m_card = m_card;
			_item.set_hb_dress (-1,m_dir);
		}
		
		m_role_dresss.Clear ();
		dbc m_dbc_role_dress = game_data._instance.get_dbc_role_dress();
		m_card = sys._instance.m_self.get_card_guid (m_guid);
		for(int i = 0;i < m_dbc_role_dress.get_y();i ++)
		{
			int id = int.Parse( game_data._instance.m_dbc_role_dress.get(0,i));
			s_t_role_dress _dress = game_data._instance.get_t_role_dress(id);
			if(_dress.role == m_card.get_template_id())
			{
				m_role_dresss.Add(_dress);
			}
		}
		
		m_role_dresss.Sort(comp);
		set_dress_page (0);
	}

	public void set_dress_page(int id)
	{
		if(id < 0)
		{
			id = 0;
		}
	
		for(int i = 0;i < 8;i ++)
		{
			int _id = id * 8 + i;
			hb_dress_page_item _item = m_hb_dress_item[i].GetComponent<hb_dress_page_item>();
			
			if(_id < m_role_dresss.Count)
			{
				_item.m_t_role_dress = m_role_dresss[_id];
				_item.m_card = m_card;
				_item.set_hb_dress(m_role_dresss[_id].id, m_dir);
			}
			else
			{
				_item.m_card = m_card;
				_item.set_hb_dress(-1,m_dir);
			}
			
		}
	}

	public void click(GameObject obj)
	{
		if(obj.name == "close")
		{
			m_shizhuang_toggle.value = true;
			this.transform.GetComponent<ui_show_anim>().hide_ui();
			s_message _out_msg = new s_message();
			_out_msg.m_type = m_message;
			cmessage_center._instance.add_message(_out_msg);
		}
		if(obj.name == "ok")
		{
			m_hb_dress_detail.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.name == "shi_zhuang")
		{
			shizhuang();
		}
		if(obj.name == "tao_zhuang")
		{
			taozhaung();
		}
	}

	public void reset_dress(s_t_role_dress t_role_dress)
	{
		if(t_role_dress.mai_desc != "0")
		{
			m_attr1.SetActive(true);
			m_attr1.GetComponent<UILabel>().text = t_role_dress.mai_desc;
		}
		else
		{
			m_attr1.SetActive(false);
		}
		m_icon.GetComponent<UISprite>().spriteName = t_role_dress.icon;
		m_name.GetComponent<UILabel>().text = t_role_dress.name;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(m_need)
		{
			m_need = false;
			update_ui();
		}
	}
	
	void IMessage.message(s_message message)
	{
		if(message.m_type == "role_dress_on")
		{
			role_dress = message.m_object[0] as s_t_role_dress;
			protocol.game.cmsg_role_dress_on _msg = new protocol.game.cmsg_role_dress_on ();
			if(role_dress.hq_condition == 3)
			{
				_msg.dress_id = 0;
			}
			else
			{
				_msg.dress_id = role_dress.id;
			}
			_msg.role_guid = m_card.get_guid();
			net_http._instance.send_msg<protocol.game.cmsg_role_dress_on> (opclient_t.CMSG_ROLE_DRESS_ON, _msg);
		}
		if(message.m_type == "show_hb_dress_detail")
		{
			s_t_role_dress t_role_dress = message.m_object[0] as s_t_role_dress;
			reset_dress(t_role_dress);
			m_hb_dress_detail.SetActive(true);
		}
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ROLE_DRESS_ON)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			dhc.role_t m_role = m_card.get_role();
			if(role_dress.hq_condition == 3)
			{
				m_role.dress_on_id = 0;
			}
			else
			{
				m_role.dress_on_id = role_dress.id;
			}
			s_message _out_msg = new s_message();
			_out_msg.m_type = "show_dress";
			_out_msg.m_object.Add(m_role);
			cmessage_center._instance.add_message(_out_msg);
			this.transform.GetComponent<ui_show_anim>().hide_ui();
			s_message _msg1 = new s_message();
			_msg1.m_type = m_message;
			cmessage_center._instance.add_message(_msg1);
			m_need = true;
		}
	}

}
