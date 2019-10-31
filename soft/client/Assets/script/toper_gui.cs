
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class toper_gui : MonoBehaviour,IMessage {
	
	// Use this for initialization
	public GameObject m_view;
	private int m_type = 0;
	List<bool> m_flag = new List<bool>();
	List<protocol.game.smsg_rank_view> m_ranks = new List<protocol.game.smsg_rank_view>();
	public GameObject m_lv;
	public GameObject m_zhanli;
	public GameObject m_bf;
	public GameObject m_star;
	public GameObject m_fight;
	public GameObject m_icon;
	public GameObject m_chenghao;
	public GameObject m_ph_item;
	static string s;//战力
	static string s1;//关卡
	static string s2;//星数
	public static string[] m_desc = {s, s, s1, s1, s2};
	private int m_cur_page = 0;
	private int m_max_page = 0;
	public GameObject m_left;
	public GameObject m_right;
	public UILabel m_page;
	public UILabel m_name_Label;
	public UILabel m_dj_Label1;
	public UILabel m_dj_Label2;
	public UILabel m_zl_Label1;
	public UILabel m_zl_Label2;
	public UILabel m_gk_Label1;
	public UILabel m_gk_Label2;
	public UILabel m_jy_Label1;
	public UILabel m_jy_Label2;
	public UILabel m_star_label1;
	public UILabel m_star_label2;
	public UILabel m_rank_Label;
	public UILabel m_rank_role_Label;
	public UILabel m_rank_Level_Label;
	public UILabel m_rank_zl_Label;

	public void start () {

		cmessage_center._instance.add_handle (this);
        s = game_data._instance.get_t_language("master_rank_gui.cs_65_26");//战力
        s1 = game_data._instance.get_t_language("toper_gui.cs_21_20");//关卡
        s2 = game_data._instance.get_t_language("toper_gui.cs_22_20");//星数
        m_desc[0] = s;
        m_desc[1] = s;
        m_desc[2] = s1;
        m_desc[3] = s1;
        m_desc[4] = s2;
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void onEnable () {
		m_flag.Clear ();
		m_ranks.Clear ();
		for (int i = 0; i < 5; ++i)
		{
			m_flag.Add(false);
			m_ranks.Add(null);
		}
		m_lv.GetComponent<UIToggle>().value = true;
		reset(0);
	}

	public void bf_toper()
	{
		m_bf.GetComponent<UIToggle>().value = true;
		reset (1);
	}

	void reset(int type)
	{
		m_type = type;
		if (m_flag[m_type] == false)
		{
			protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view ();
			_msg.type = m_type;
			net_http._instance.send_msg<protocol.game.cmsg_rank_view> (opclient_t.CMSG_RANK_VIEW, _msg);
			return;
		}
		int max_num = 50;
		dhc.rank_t rank_t = m_ranks[m_type].rank_list;
		if (rank_t.player_guid.Count < max_num)
		{
			max_num = rank_t.player_guid.Count;
		}
		int num = rank_t.player_guid.Count;
		if (num > 50)
		{
			num = 50;
		}
		m_max_page = (num + 4) / 4;
		if(num % 4 == 0)
		{
			m_max_page = num / 4;
		}
		m_cur_page = 0;
		if(m_max_page < 1)
		{
			m_max_page = 1;
		}
		string s = sys._instance.value_to_wan(sys._instance.m_self.m_t_player.bf);
		int rank = rank_t.player_guid.IndexOf(sys._instance.m_self.m_t_player.guid);
	 	if (type == 4)
		{
			int value = 0;
			for(int i = 0; i < sys._instance.m_self.m_t_player.map_star.Count;++i)
			{
				value += sys._instance.m_self.m_t_player.map_star[i];
			}
			s = value.ToString();
		}
		else if (type == 2)
		{
			s = game_data._instance.get_t_language ("toper_gui.cs_128_7");//未知
			int mission_id = 0;
			for(int i = 0;i < sys._instance.m_self.m_t_player.mission_ids.Count;i++)
			{
				s_t_mission t_mission = game_data._instance.get_t_mission(sys._instance.m_self.m_t_player.mission_ids[i]);
				if(t_mission == null)
				{
					continue;
				}
				if (t_mission.type == 1 && sys._instance.m_self.m_t_player.mission_ids[i] > mission_id)
				{
					mission_id = sys._instance.m_self.m_t_player.mission_ids[i];
				}
			}
			s_t_mission _mission = game_data._instance.get_t_mission(mission_id);
			if(_mission != null)
			{
				string[] text = _mission.name.Split(' ');
				s = text[0];
				if(text.Length >= 2)
				{
					s = "[ " + text[0] + " ]" + "\n" + text[1];
				}
			}
		}
		else if (type == 3)
		{
			s = game_data._instance.get_t_language ("toper_gui.cs_128_7");//未知
			int mission_id = 0;
			for(int i = 0;i < sys._instance.m_self.m_t_player.mission_ids.Count;i++)
			{
				s_t_mission t_mission = game_data._instance.get_t_mission(sys._instance.m_self.m_t_player.mission_ids[i]);
				if(t_mission == null)
				{
					continue;
				}
				if (t_mission.type == 2 && sys._instance.m_self.m_t_player.mission_ids[i] > mission_id)
				{
					mission_id = sys._instance.m_self.m_t_player.mission_ids[i];
				}
			}
			s_t_mission _mission = game_data._instance.get_t_mission(mission_id);
			if(_mission != null)
			{
				string[] text = _mission.name.Split(' ');
				s = text[0];
				if(text.Length >= 2)
				{
					s = "[ " + text[0] + " ]" + "\n" + text[1];
				}
			}
		}
		UILabel s1 = m_fight.transform.GetComponent<UILabel>();
		m_fight.SetActive (true);
		m_star.SetActive(false);
		m_ph_item.transform.Find("star_num").gameObject.SetActive (false);
		m_ph_item.transform.Find("num").gameObject.SetActive (false);
		switch(type)
		{
		case 0:
			s1.text =  s;
			break;
		case 1:
			s1.text =  s;
			break;
		case 2:
			m_star.SetActive(false);
			m_fight.SetActive(false);
			m_ph_item.transform.Find("num").gameObject.SetActive (true);
			m_ph_item.transform.Find("num").GetComponent<UILabel>().text = "[0aabff]" + s;
			break;
		case 3:
			m_star.SetActive(false);
			m_fight.SetActive(false);
			m_ph_item.transform.Find("num").gameObject.SetActive (true);
			m_ph_item.transform.Find("num").GetComponent<UILabel>().text = "[0aabff]" + s;
			break;
		case 4:
			m_star.SetActive(true);
			m_fight.SetActive(false);
			m_ph_item.transform.Find("star_num").gameObject.SetActive (true);
			m_ph_item.transform.Find("star_num").GetComponent<UILabel>().text = "[f8cf40]" + s;
			break;
			
		}
		if(rank >= 0 && rank < 50)
		{
			m_ph_item.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), (rank+1).ToString());//第{0}名
		}
		else
		{
			m_ph_item.transform.Find("rank").GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81");//未上榜
		}
		m_ph_item.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) 
			+ sys._instance.m_self.m_t_player.name;
		m_ph_item.transform.Find("level").GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.level.ToString();
		sys._instance.remove_child (m_icon);
		GameObject _obj1 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,
		                                                             sys._instance.m_self.m_t_player.dress_achieves.Count,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		sys._instance.get_chenghao (sys._instance.m_self.m_t_player.chenghao_on, m_chenghao);
		set_top_page (0);
	
	}
	void set_top_page(int id)
	{
		if(id < 0)
		{
			id = 0;
		}
		
		m_right.SetActive(true);
		m_left.SetActive (true);
		m_page.text = m_cur_page + 1 + "/" + m_max_page;
		if(id + 1 < m_max_page)
		{
			m_cur_page = id;
			m_right.GetComponent<UISprite>().alpha = 1.0f;
			m_right.GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			m_right.GetComponent<UISprite>().alpha = 0.5f;
			m_right.GetComponent<BoxCollider>().enabled = false;
		}
		
		if(id == 0)
		{
			m_left.GetComponent<UISprite>().alpha = 0.5f;
			m_left.GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			m_left.GetComponent<UISprite>().alpha = 1.0f;
			m_left.GetComponent<BoxCollider>().enabled = true;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		dhc.rank_t rank_t = m_ranks[m_type].rank_list;
		for (int i = 0; i < 4; ++i)
		{
			if(id * 4 + i < rank_t.player_guid.Count && id * 4 + i < 50)
			{
				GameObject rank_list = game_data._instance.ins_object_res("ui/toper_list_item");
				rank_list.transform.parent = m_view.transform;
				rank_list.transform.localPosition = new Vector3(0, 73 - i *69 ,0);
				rank_list.transform.localScale = new Vector3(1,1,1);
				rank_list.transform.GetComponent<toper_list_item>().reset(m_type, id * 4 + i + 1,
				                                                     rank_t.player_guid[id * 4 + i], rank_t.player_name[id * 4 + i], rank_t.player_level[id * 4 + i],
				                                                       rank_t.player_bf[id * 4 + i], rank_t.value[id * 4 + i],rank_t.player_template[id * 4 + i]
				                                                          ,rank_t.player_achieve[id*4 + i],rank_t.player_vip[id*4 + i],rank_t.player_chenghao[id*4 + i],rank_t.player_nalflag[id *4 +i]);
			}    
			
		}
		
		m_zhanli.GetComponent<UILabel>().text = m_desc[m_type];
	}
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_RANK_VIEW)
		{
			protocol.game.smsg_rank_view _msg = net_http._instance.parse_packet<protocol.game.smsg_rank_view> (message.m_byte);
			m_flag[m_type] = true;
			m_ranks[m_type] = _msg;
			reset (m_type);
		}
	}

	void IMessage.message(s_message message)
	{

	}

	public void click(GameObject obj) {
		if(obj.transform.name == "close")
		{
			m_lv.GetComponent<UIToggle>().value = true;
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
            Destroy(this.gameObject);
		}
		if (obj.name == "lvtoper")
		{
			reset (0);
		}
		if (obj.name == "bftoper")
		{
			reset (1);
		}
		if (obj.name == "pttoper")
		{
			reset (2);
		}
		if (obj.name == "jytoper")
		{
			reset (3);
		}
		if (obj.name == "xstoper")
		{
			reset (4);
		}
		if(obj.transform.name == "left")
		{
			m_cur_page --;
			set_top_page(m_cur_page);
		}
		
		if(obj.transform.name == "right")
		{
			m_cur_page ++;
			set_top_page(m_cur_page);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
