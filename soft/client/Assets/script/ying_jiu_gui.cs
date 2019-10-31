
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ying_jiu_gui : MonoBehaviour,IMessage {

	public GameObject m_card_root;
	public GameObject m_num;

	public List<GameObject> m_units = new List<GameObject>();
	
	private GameObject m_cam;

	public UILabel m_num_Label;
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
		update_ui ();
	}

	public void update_ui()
	{
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();	
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HBB_LOOK, _msg);
	}

	public void click(GameObject obj)
	{
		if(obj.name == "restart")
		{
			string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			int num = sys._instance.m_self.m_t_player.hbb_refresh_num;
			s_t_price t_price = game_data._instance.get_t_price(num + 1);
			s_message _message = new s_message();
			_message.m_type = "buy_hbb_refresh";
			_message.m_ints.Add(t_price.hbb_refresh);
			string  _des = string.Format(game_data._instance.get_t_language ("ying_jiu_gui.cs_47_32") , t_price.hbb_refresh );				//本次需要花费[00ffff]{0}钻石[-]，是否确定刷新?
			root_gui._instance.show_select_dialog_box(tishi,_des,_message);
		}


		if(obj.name == "attack")
		{
			if (sys._instance.m_self.m_t_player.hbb_finish_num >= sys._instance.m_self.m_t_player.hbb_num)
			{
				string s = game_data._instance.get_t_language ("huo_dong_gui.cs_904_15");//[ffc882]今日次数已用完
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			
			if(sys._instance.m_self.get_un_equip_num() >= sys._instance.m_self.get_max_equip_num())
			{

				string  _des = game_data._instance.get_t_language ("ying_jiu_gui.cs_64_19");//您的装备携带数量已达上限，请先进行清理
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + _des);

				return ;
			}
			if(sys._instance.m_self.get_un_treasure_num() >= sys._instance.m_self.get_max_treasure_num())
			{
				
				string  _des = game_data._instance.get_t_language ("ying_jiu_gui.cs_72_19");//您的饰品携带数量已达上限，请先进行清理
				root_gui._instance.show_prompt_dialog_box("[ffc882]" +_des);
				
				return ;
			}

			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();		
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HBB_FIGHT_END, _msg);
		}


		if(obj.name == "close")
		{
			remove_all();
			
			s_message _message = new s_message();
			_message.m_type = "show_huo_dong";
			_message.m_ints.Add(3);
			_message.m_bools.Add(false);
			cmessage_center._instance.add_message(_message);

			sys._instance.m_game_state = "hall";
			sys._instance.load_scene(sys._instance.m_hall_name);
			
			this.gameObject.SetActive(false);
		}
	}
	void remove_all()
	{
		for(int i = 0;i < m_units.Count;i ++)
		{
			GameObject.Destroy(m_units[i]);
		}
		
		m_units.Clear ();
	}
	void cam()
	{
		m_cam = GameObject.Find("tt_cam");

		if(m_cam == null)
		{
			return;
		}

		float _max_height = 0;
		GameObject _max_object = null;

		for(int i = 0;i < m_units.Count;i ++)
		{
			GameObject _unit = m_units[i];

			if(_unit.GetComponent<unit>().m_name_height + 0.3f > _max_height)
			{
				_max_height = _unit.GetComponent<unit>().m_name_height + 0.3f;
				_max_object = _unit;
			}
		}

		List<GameObject> _units = new List<GameObject>();

		for(int i = 0;i < m_units.Count;i ++)
		{
			GameObject _unit = m_units[i];

			if(_unit != _max_object)
			{
				_units.Add(_unit);
			}
		}

		_max_object.transform.localPosition = new Vector3 (0,0,0);
		_max_object.transform.localEulerAngles = new Vector3 (0,180,0);

		_units[0].transform.localPosition = new Vector3 (- 1.57f,0,0);
		_units[0].transform.localEulerAngles = new Vector3 (0,150,0);
		_units[1].transform.localPosition = new Vector3 (1.57f,0,0);
		_units[1].transform.localEulerAngles = new Vector3 (0,210,0);

		Vector3 _pos = m_cam.transform.localPosition;

		_pos.y = _max_height * 0.55f;
		_pos.z = _max_height * - 1.4f;

		m_cam.transform.localPosition = _pos;
	}

	void add_card(int id,int pos)
	{
		ccard _card = ccard.get_new_card ((int)id);

		GameObject _unit = sys._instance.create_class (_card, 0);
		_unit.GetComponent<unit>().create_name (_card.get_color_name(),_card.get_color());

		m_units.Add (_unit);
	}

	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_HBB_FIGHT_END)
		{
			protocol.game.smsg_hbb_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_hbb_fight_end> (message.m_byte);

			battle_logic_ex._instance.set_hbb_fight_end(_msg);

			sys._instance.load_scene_ex("ts_fight_jjc");
			sys._instance.m_game_state = "buttle";
			
			remove_all();

			sys._instance.remove_child(m_card_root);
		}

		if (message.m_opcode == opclient_t.CMSG_HBB_LOOK 
		    || message.m_opcode == opclient_t.CMSG_HBB_REFRESH)
		{
			protocol.game.smsg_hbb_look _msg = net_http._instance.parse_packet<protocol.game.smsg_hbb_look> (message.m_byte);
			if(message.m_opcode == opclient_t.CMSG_HBB_REFRESH)
			{
				int num = sys._instance.m_self.m_t_player.hbb_refresh_num;
				s_t_price t_price = game_data._instance.get_t_price(num + 1);
				sys._instance.m_self.sub_att(e_player_attr.player_jewel,t_price.hbb_refresh,game_data._instance.get_t_language ("ying_jiu_gui.cs_193_80"));//营救伙伴刷新

				sys._instance.m_self.m_t_player.hbb_refresh_num ++;
				int t_num = 0;
				for(int i = 0;i < _msg.class_ids.Count;i ++)
				{

					if(game_data._instance.get_t_class((int)_msg.class_ids[i]).color == 4)
					{
						t_num++;

					}
				}
			}


			sys._instance.remove_child(m_card_root);

			remove_all();
			for(int i = 0;i < _msg.class_ids.Count;i ++)
			{
				add_card((int)_msg.class_ids[i],i);
			}
			cam();

			m_num.GetComponent<UILabel>().text = (sys._instance.m_self.m_t_player.hbb_num - sys._instance.m_self.m_t_player.hbb_finish_num) + "/" + sys._instance.m_self.m_t_player.hbb_num;
		}

		//m_jewel.text = sys._instance.m_self.m_t_player.jewel + "";
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "show_unit" )
		{
			ui_show_anim _ui = this.transform.GetComponent<ui_show_anim>();

			if(_ui != null)
			{
				this.transform.GetComponent<ui_show_anim>().hide_ui();
			}
		}

		if(message.m_type == "buy_hbb_count")
		{
			/*
			int _count = (int)message.m_ints[0];
			if (_count > sys._instance.m_self.get_att(e_player_attr.player_jewel))
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();		
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HBB_ADD, _msg);
			*/
		}
		if(message.m_type == "buy_hbb_refresh")
		{
			int _count = (int)message.m_ints[0];
			if (_count > sys._instance.m_self.get_att(e_player_attr.player_jewel))
			{
				string s = "[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}

			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();	
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HBB_REFRESH, _msg);
		}
		/*
		if(message.m_type == "yingjiu_fight_end")
		{
			int _win = (int)message.m_ints[0];
			
			protocol.game.cmsg_hbb_fight_end _net_msg = new protocol.game.cmsg_hbb_fight_end ();
			_net_msg.win = _win;
			net_http._instance.send_msg<protocol.game.cmsg_hbb_fight_end> (opclient_t.CMSG_HBB_FIGHT_END, _net_msg);
		}	
		*/
	}
	// Update is called once per frame
	void Update () {
	
	}
}
