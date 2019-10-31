using System.Collections.Generic;
using UnityEngine;

public class juntuan_paiming_info : MonoBehaviour,IMessage {
	public List<dhc.guild_t> m_guilds;
	public GameObject m_guilds_items;
	public GameObject m_tip;
    protocol.game.smsg_guild_mission_ranking _msg;
    public protocol.game.smsg_rank_view m_msg;
	
    public UIToggle m_rank;
    public GameObject top1;
    public GameObject top2;
	public UILabel m_jun1;
	public UILabel m_jun2;
	public UILabel m_jun3;
	public UILabel m_jun4;
	public UILabel m_jun5;
	public UILabel m_fuhe;

	public e_guild_rank_type m_rank_type;
	public GameObject m_other_rank_gui;
	public GameObject m_rank_scro;
	public GameObject m_geren_scro;
	public GameObject m_rank_me;
	public GameObject m_guild_rank_gui;
	public GameObject m_geren_rank_gui;
//	public UIToggle m_first_toggle;
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void OnEnable()
	{
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_RANKING, _msg);
		m_guilds_items.GetComponent<UIScrollView>().ResetPosition ();
	}
	void IMessage.message (s_message message)
	{
		
	}
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GUILD_RANKING)
		{
			protocol.game.smsg_guild_ranking _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_ranking> (message.m_byte);
			List<dhc.guild_t> guilds = new List<dhc.guild_t>();
			for (int i = 0; i < _msg.guild_names.Count; i++)
			{
				dhc.guild_t guild = new dhc.guild_t ();
				guild.icon = _msg.guild_icons [i];
				guild.level = _msg.guild_levels [i];
				guild.name = _msg.guild_names [i];
				for (int j = 0; j < _msg.guild_members[i]; j++)
				{
					guild.member_guids.Add (0);
				}
				guilds.Add (guild);
			}
			m_guilds = guilds;
			m_other_rank_gui.SetActive(true);
			refresh (m_guilds);
		}
		else if (message.m_opcode == opclient_t.CMSG_GUILD_MISSION_RANKING)
		{
			_msg = net_http._instance.parse_packet<protocol.game.smsg_guild_mission_ranking> (message.m_byte);
			m_other_rank_gui.SetActive(true);
			refresh_mission ();
 
		} 
		else if (message.m_opcode == opclient_t.CMSG_PVP_RANK && gameObject.activeSelf)
		{
			if (m_rank_type == e_guild_rank_type.guild_rank)
			{
				m_msg = net_http._instance.parse_packet<protocol.game.smsg_rank_view> (message.m_byte);
				m_other_rank_gui.SetActive(false);
				m_guild_rank_gui.SetActive (true);
				m_geren_rank_gui.SetActive (false);
				top1.SetActive(false);
				top2.SetActive(false);
				reset_guildrank ();
			}
			else
			{
				m_msg = net_http._instance.parse_packet<protocol.game.smsg_rank_view> (message.m_byte);
				m_other_rank_gui.SetActive(false);
				m_guild_rank_gui.SetActive (false);
				m_geren_rank_gui.SetActive (true);
				top1.SetActive(false);
				top2.SetActive(false);
				reset_gerenrank ();
				
			}
		}
	}
	public void reset_guildrank()
	{
		if (m_rank_scro.GetComponent<SpringPanel>() != null)
		{
			m_rank_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_rank_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_rank_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_rank_scro);
		for (int i = 0; i < m_msg.rank_list.player_guid.Count; i++)
		{
			GameObject target = game_data._instance.ins_object_res("ui/juntuan/guilditem");
			
			target.transform.parent = m_rank_scro.transform;
			target.transform.localPosition = new Vector3(0, 140 - i * 112, 0);
			target.transform.localScale = Vector3.one;
			target.transform.Find("name").GetComponent<UILabel>().text = m_msg.rank_list.player_name[i];
			target.transform.Find("rank").GetComponent<UILabel>().text = i + 1 + "";
			target.transform.Find("icon").GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon(m_msg.rank_list.player_template[i]).icon;
			target.transform.Find("num").GetComponent<UILabel>().text = sys._instance.get_server(m_msg.rank_list.player_level[i]).m_name;
			target.transform.Find("blood").GetComponent<UILabel>().text = "[00ff00]" + m_msg.rank_list.value[i] + "";
			
		}
		
	}
	void reset_gerenrank()
	{
		if (m_geren_scro.GetComponent<SpringPanel>() != null)
		{
			m_geren_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_geren_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_geren_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_geren_scro);
		for (int i = 0; i < m_msg.rank_list.player_level.Count; i++)
		{
			GameObject _obj = game_data._instance.ins_object_res("ui/juntuan/guild_fight_sub");
			_obj.transform.name = i.ToString();
			_obj.transform.parent = m_geren_scro.transform;
			_obj.transform.localScale = new Vector3(1, 1, 1);
			_obj.transform.localPosition = new Vector3(0, 110 - i * 68, 1);
			
			int _rank = i + 1;
			string _rank_show = _rank.ToString();
			GameObject _rank_obj = _obj.transform.Find("rank").gameObject;
			UIEventListener.Get(_obj.transform.Find("look").gameObject).onClick = click;
			
			if (_rank == 1)
			{
				_rank_show = game_data._instance.get_t_language(game_data._instance.get_t_language ("look_jl_gui.cs_99_18"));//第1名
			}
			else if (_rank == 2)
			{
				_rank_show = game_data._instance.get_t_language(game_data._instance.get_t_language ("look_jl_gui.cs_104_18"));//第2名
			}
			else if (_rank == 3)
			{
				_rank_show = game_data._instance.get_t_language(game_data._instance.get_t_language ("look_jl_gui.cs_109_18"));//第3名
			}
			_rank_obj.GetComponent<UILabel>().text = _rank_show;
			_obj.transform.Find("look").GetComponent<UIButtonMessage>().target = this.gameObject;
			_obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_msg.rank_list.player_achieve[i]) + m_msg.rank_list.player_name[i].ToString();
			_obj.transform.Find("level").GetComponent<UILabel>().text = "[00ff00]" + sys._instance.get_server(m_msg.rank_list.player_level[i]).m_name;
			_obj.transform.Find("hurt").GetComponent<UILabel>().text = m_msg.rank_list.value[i].ToString();
			sys._instance.get_chenghao(m_msg.rank_list.player_chenghao[i], _obj.transform.Find("chenhao").gameObject);
			
			GameObject m_icon = _obj.transform.Find("icon").gameObject;
			sys._instance.remove_child(m_icon);
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_msg.rank_list.player_template[i], m_msg.rank_list.player_achieve[i], m_msg.rank_list.player_vip[i],m_msg.rank_list.player_nalflag[i]);
			
			_obj1.transform.parent = m_icon.transform;
			_obj1.transform.localScale = new Vector3(1, 1, 1);
			_obj1.transform.localPosition = new Vector3(0, 0, 0);
		}
		m_rank_me.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color
			(sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name.ToString();
		m_rank_me.transform.Find("level").GetComponent<UILabel>().text = "[00ff00]" + sys._instance.get_server(sys._instance.m_self.m_t_player.level).m_name; ;
		
		if (juntuan_gui._instance.m_guild_kuafu .GetComponent<juntuan_kuafu_control>().look_msg.fight != null)
		{
			m_rank_me.transform.Find("hurt").GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_kuafu .GetComponent<juntuan_kuafu_control>().look_msg.fight.zhanji + "";
		}
		else if (juntuan_gui._instance.m_guild_kuafu .GetComponent<juntuan_kuafu_control>().look_msg.zhanji != null)
		{
			m_rank_me.transform.Find("hurt").GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_kuafu .GetComponent<juntuan_kuafu_control>().look_msg.zhanji.zhanji + "";
		}
		GameObject _icon = m_rank_me.transform.Find("icon").gameObject;
		sys._instance.remove_child(_icon);
		GameObject _obj2 = icon_manager._instance.create_player_icon
			((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count, sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
		
		_obj2.transform.parent = _icon.transform;
		_obj2.transform.localScale = new Vector3(1, 1, 1);
		_obj2.transform.localPosition = new Vector3(0, 0, 0);
		sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on, m_rank_me.transform.Find("chenhao").gameObject);
		int rank = get_rank();
		if (rank != -1)
		{
			m_rank_me.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), (rank + 1));//第{0}名
			
		}
		else
		{
			m_rank_me.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81"));//未上榜
			
			
		}
		
		
		
	}
	int get_rank()
	{
		for (int i = 0; i < m_msg.rank_list.player_guid.Count; i++)
		{
			if (m_msg.rank_list.player_guid[i] == sys._instance.m_self.m_guid)
			{
				return i;
			}
		}
		return -1;
	}

    void click(GameObject obj)
    {
        if (obj.name == "rank_gongxian")
        {
			top1.SetActive(true);
			top2.SetActive(false);
			m_other_rank_gui.SetActive(true);
			m_guild_rank_gui.SetActive (false);
			m_geren_rank_gui.SetActive (false);
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_RANKING, _msg);
            m_guilds_items.GetComponent<UIScrollView>().ResetPosition();
 
        }
        else if (obj.name == "rank_fuben")
        {
			top1.SetActive(false);
			top2.SetActive(true);
			m_other_rank_gui.SetActive(true);
			m_guild_rank_gui.SetActive (false);
			m_geren_rank_gui.SetActive (false);
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_MISSION_RANKING, _msg);
            m_guilds_items.GetComponent<UIScrollView>().ResetPosition();

 
        }
		else if (obj.name == "guild_rank")
		{
			protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view();
			_msg.type = (int)e_guild_rank_type.guild_rank;
			m_rank_type = e_guild_rank_type.guild_rank;
			net_http._instance.send_msg<protocol.game.cmsg_rank_view>(opclient_t.CMSG_PVP_RANK, _msg, true);
			
		}
		else if (obj.name == "geren_rank")
		{
			protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view();
			_msg.type = 201;
			m_rank_type = e_guild_rank_type.geren_rank;
			net_http._instance.send_msg<protocol.game.cmsg_rank_view>(opclient_t.CMSG_PVP_RANK, _msg, true);
		}
    }
	public static int comp(dhc.guild_t guild1, dhc.guild_t guild2)
	{
		if (guild1.level > guild2.level)
		{
			return -1;
		}
		else if (guild1.level < guild2.level)
		{
			return 1;
		}

		if (guild1.exp > guild2.exp)
		{
			return -1;
		}
		else if (guild1.exp < guild2.exp)
		{
			return 1;
		}
		return 0;
	}
    public void refresh_mission()
    {
        top1.SetActive(false);
        top2.SetActive(true);
		m_guild_rank_gui.SetActive (false);
		m_geren_rank_gui.SetActive (false);
		m_guilds_items.transform.localPosition = new Vector3(0, 0, 0);
        m_guilds_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_guilds_items);
        if (_msg.guild_name.Count == 0)
        {
            m_tip.SetActive(true);
        }
        else
        {
            m_tip.SetActive(false);
        }
        for (int i = 0; i < _msg.guild_name.Count; i++)
        {
            GameObject target = game_data._instance.ins_object_res("ui/juntuan/guilditem");
            target.transform.parent = m_guilds_items.transform;
            target.transform.localPosition = new Vector3(0, 140 - i * 112, 0);
            target.transform.localScale = Vector3.one;
            target.transform.Find("name").GetComponent<UILabel>().text = _msg.guild_name[i];
            target.transform.Find("rank").GetComponent<UILabel>().text = i + 1 + "";
            target.transform.Find("icon").GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon(_msg.icon[i]).icon;
			target.transform.Find("icon/guildicon").GetComponent<UISprite>().spriteName = juntuan_gui._instance.setKuang(_msg.guild_level[i]);
            target.transform.Find("num").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("boss.cs_234_75"), _msg.ceng[i]);//第{0}关
            target.transform.Find("blood").GetComponent<UILabel>().text = "[00ff00]" + (int)((_msg.max_hp[i] - (float)_msg.hp[i])/_msg.max_hp[i] * 100) + "%";

        }
 
    }
	public void refresh(List<dhc.guild_t> t)
	{
        top1.SetActive(true);
        top2.SetActive(false);
		m_guild_rank_gui.SetActive (false);
		m_geren_rank_gui.SetActive (false);
		t.Sort (comp);
		m_guilds_items.transform.localPosition = new Vector3(0, 0, 0);
		m_guilds_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_guilds_items);
		if(t.Count == 0)
		{
			m_tip.SetActive(true);
		}
		else
		{
			m_tip.SetActive(false);
		}
		for(int i = 0;i < t.Count;i++)
		{
            GameObject target = game_data._instance.ins_object_res("ui/juntuan/guilditem");

            target.transform.parent = m_guilds_items.transform;
            target.transform.localPosition = new Vector3(0, 140 - i * 112, 0);
            target.transform.localScale = Vector3.one;
            target.transform.Find("name").GetComponent<UILabel>().text = t[i].name;
            target.transform.Find("rank").GetComponent<UILabel>().text = i + 1 + "";
            target.transform.Find("icon").GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon(t[i].icon).icon;
			target.transform.Find("icon/guildicon").GetComponent<UISprite>().spriteName = juntuan_gui._instance.setKuang(t[i].level);
            target.transform.Find("num").GetComponent<UILabel>().text = t[i].level + "";
            target.transform.Find("blood").GetComponent<UILabel>().text = "[00ff00]" + t[i].member_guids.Count.ToString() + "/" + game_data._instance.get_guild(t[i].level).number_count.ToString();
		}
	}
}
