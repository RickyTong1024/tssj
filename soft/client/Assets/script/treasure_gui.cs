
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_gui : MonoBehaviour ,IMessage{

	public GameObject m_treasure_page_gui;
	public GameObject m_kc;
	public GameObject m_effect;
	public GameObject m_detail_gui;
	private List<ulong> m_shells = new List<ulong>();
	private int m_select = 9;
	private int m_item_id;
	public GameObject m_name;
	public GameObject m_dec;
	public GameObject m_icon;

	public UILabel m_baowu_Label1;
	public UILabel m_baowu_Label2;
	public UILabel m_suipian_Label1;
	public UILabel m_suipian_Label2;
	public UILabel m_kc_Label;
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
		if (m_select == 0)
		{
			m_kc.SetActive(true);
		}
	}


	public void init()
	{
		m_treasure_page_gui = game_data._instance.ins_object_res("ui/treasure_page_gui");
		m_treasure_page_gui.transform.parent = this.transform;
		m_treasure_page_gui.transform.localPosition = new Vector3(0,0,0);
		m_treasure_page_gui.transform.localScale = new Vector3(1,1,1);
		m_treasure_page_gui.SetActive (true);
	}
	
	public void reset()
	{
		baowu ();
	}
	
	public void hide_page()
	{
		m_detail_gui.SetActive(false);
		Transform _obj = this.transform.Find("main_button");
		_obj.Find("bao_wu").GetComponent<UIToggle>().value = true;
	}
	
	public void click(GameObject obj)
	{
		if(obj.transform.name == "hide_treasure")
		{
			hide_page();
			transform.GetComponent<ui_show_anim>().hide_ui();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.transform.name == "bao_wu")
		{
			baowu();
		}
		else if (obj.transform.name == "sui_pian")
		{
			suipian();
		}
		else if (obj.transform.name == "kc")
		{
			if (sys._instance.m_self.m_t_player.treasure_kc_num >= 5)
			{
				string s = game_data._instance.get_t_language ("treasure_gui.cs_86_15");//[ffc882]扩充次数已满
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			s_t_price _price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.treasure_kc_num + 1);
			string _des = string.Format(game_data._instance.get_t_language ("treasure_gui.cs_94_31") , _price.kc );//是否花费[00ffff]{0}钻石[-]扩充10格饰品格
			s_message _message = new s_message();
			_message.m_type = "treasure_kc";
			root_gui._instance.show_select_dialog_box(tishi,_des,_message);
		}
		else if (obj.transform.name == "hide")
		{
			m_detail_gui.GetComponent<ui_bloom_anim>().hide_ui();
		}
	}
	
	public void baowu()
	{		
		m_kc.SetActive(true);
		
		hide_page ();
		m_select = 0;
		
		m_treasure_page_gui.SetActive(true);
		m_treasure_page_gui.GetComponent<treasure_page_gui>().init();
		m_treasure_page_gui.GetComponent<treasure_page_gui>().set_text(sys._instance.m_self.get_un_treasure_num() + " / " + sys._instance.m_self.get_max_treasure_num().ToString());
		m_treasure_page_gui.GetComponent<treasure_page_gui>().m_out_message = "click_treasure";
		m_treasure_page_gui.GetComponent<treasure_page_gui>().treasure_reset();
	}
	
	public void suipian()
	{		
		m_kc.SetActive (false);
		
		hide_page ();
		m_select = 1;
		
		m_treasure_page_gui.SetActive(true);
		m_treasure_page_gui.GetComponent<treasure_page_gui>().init();
		m_treasure_page_gui.GetComponent<treasure_page_gui>().sp_reset();
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_TREASURE_EXPAND)
		{
			s_t_price _price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.treasure_kc_num + 1);
			sys._instance.m_self.m_t_player.treasure_kc_num++;
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, _price.kc);
			baowu();
		}
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "treasure_kc")
		{
			s_t_price _price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.treasure_kc_num + 1);
			if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < _price.kc)
			{
				root_gui._instance.show_recharge_dialog_box(delegate() {
					this.GetComponent<ui_show_anim>().hide_ui();
				});
				return;
			}
			
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_TREASURE_EXPAND, _msg);
		}
		else if(message.m_type == "click_treasure")
		{
			if(m_select == 0)
			{
				List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
				for(int i =0; i< m_treasure_page_gui.GetComponent<treasure_page_gui>().m_treasures.Count;++i)
				{
					int id = m_treasure_page_gui.GetComponent<treasure_page_gui>().m_treasures[i].template_id;
					s_t_baowu _treasure = game_data._instance.get_t_baowu(id);
					if(_treasure.type == 5)
					{
						continue;
					}
					m_treasures.Add(m_treasure_page_gui.GetComponent<treasure_page_gui>().m_treasures[i]);
				}
				ulong _gui = (ulong)message.m_long[0];
				GameObject obj = (GameObject)message.m_object[1];
				root_gui._instance.add_treasure(m_treasures,false);
				dhc.treasure_t t_treasure = sys._instance.m_self.get_treasure_guid(_gui);
				s_t_baowu _baowu = game_data._instance.get_t_baowu(t_treasure.template_id);
				if(_baowu.type == 5)
				{
					sys._instance.remove_child (m_icon);
					GameObject iicon = icon_manager._instance.create_treasure_icon(t_treasure.template_id);
					iicon.transform.parent = m_icon.transform;
					iicon.transform.localPosition = new Vector3(0,0,0);
					iicon.transform.localScale = new Vector3(1,1,1);
					m_name.GetComponent<UILabel>().text = treasure.get_treasure_real_name(t_treasure.template_id);
					m_dec.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("treasure.cs_46_41"),_baowu.exp.ToString());//饰品强化时提供{0}点饰品经验
					m_detail_gui.SetActive(true);
				}
				else
				{
					root_gui._instance.show_treasure_detail(t_treasure, 0,null,false);
				}
			}
		}
	}
}
