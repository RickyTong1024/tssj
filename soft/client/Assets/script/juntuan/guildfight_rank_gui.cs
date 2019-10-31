using UnityEngine;

public enum e_guild_rank_type
{
    guild_rank = 200,
    geren_rank = 201
}
public class guildfight_rank_gui : MonoBehaviour ,IMessage{
    public GameObject m_reward_scro;
    public GameObject m_rank_scro;
    public protocol.game.smsg_rank_view m_msg;
    public e_guild_rank_type m_rank_type;
    public GameObject m_geren_scro;
    public GameObject m_rank_me;
    public GameObject m_guild_rank_gui;
    public GameObject m_geren_rank_gui;
    public GameObject m_reward_gui;
    public UIToggle m_first_toggle;
    void OnEnable()
    {
		if (root_gui._instance.m_default_active == "show_rank")
		{
			m_guild_rank_gui.SetActive (false);
			m_geren_rank_gui.SetActive (false);
			m_reward_gui.SetActive (true);
			this.transform.Find("back/geren_reward").GetComponent<UIToggle>().Set (true);
			this.transform.Find("back/guild_rank").GetComponent<UIToggle>().Set (false);
			this.transform.Find("back/guild_reward").GetComponent<UIToggle>().Set (false);
			this.transform.Find("back/geren_rank").GetComponent<UIToggle>().Set (false);
			reset_guildreward (2);
			root_gui._instance.m_default_active = "";
			
			return;
			
		}
		else
		{
			protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view ();
			_msg.type = (int)e_guild_rank_type.guild_rank;
			m_rank_type = e_guild_rank_type.guild_rank;
			net_http._instance.send_msg<protocol.game.cmsg_rank_view> (opclient_t.CMSG_PVP_RANK, _msg, true);
		}
        
    }
    void Start()
    {

        cmessage_center._instance.add_handle(this);
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void IMessage.message (s_message message)
	{
		
	}
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_PVP_RANK && gameObject.activeSelf )
        {
            if (m_rank_type == e_guild_rank_type.guild_rank)
            {
                m_msg = net_http._instance.parse_packet<protocol.game.smsg_rank_view>(message.m_byte);
               			
				m_guild_rank_gui.SetActive(true);
				m_geren_rank_gui.SetActive(false);
				m_reward_gui.SetActive(false);
				reset_guildrank();
			}
			else
            {
                m_msg = net_http._instance.parse_packet<protocol.game.smsg_rank_view>(message.m_byte);
                m_guild_rank_gui.SetActive(false);
                m_geren_rank_gui.SetActive(true);
                m_reward_gui.SetActive(false);
                reset_gerenrank();
 
            }
          
        }
    }
    
    void reset_guildreward(int type)
    {
        if (m_reward_scro.GetComponent<SpringPanel>() != null)
        {
            m_reward_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_reward_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_reward_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_reward_scro);
        int k = 0;
        for (int j = 0; j < game_data._instance.m_dbc_guildfight_reward.get_y(); j++)
        {
            int _rank1 = int.Parse(game_data._instance.m_dbc_guildfight_reward.get(0, j));
            int _type = int.Parse(game_data._instance.m_dbc_guildfight_reward.get(2, j));
            if (type != _type)
            {
                continue;
            }
            s_t_guildfight_reward _reward = game_data._instance.get_t_guildfight_reward(_rank1, _type);
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_fight_reward_sub");
            obj.transform.parent = m_reward_scro.transform;
            obj.transform.localPosition = new Vector3(0, 138 - k * 122, 0);
            obj.transform.localScale = Vector3.one;
            if (_reward.rank1 == _reward.rank2)
            {
                obj.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), _reward.rank1);//第{0}名
            }
            else
            {
                obj.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_182_82"), _reward.rank1, _reward.rank2);//第{0}-{1}名
            }
            for (int i = 0; i < 4; i++)
            {
                Transform parent = obj.transform.Find(i + "");
                parent.gameObject.SetActive(false);
            }
            for (int i = 0; i < _reward.rewards.Count; i++)
            {
                Transform parent = obj.transform.Find(i + "");
                parent.gameObject.SetActive(true);
                sys._instance.remove_child(parent.gameObject);
                GameObject icon = icon_manager._instance.create_reward_icon(_reward.rewards[i].type, _reward.rewards[i].value1, _reward.rewards[i].value2,
                    _reward.rewards[i].value3);
                icon.transform.parent = parent;
                icon.transform.localPosition = Vector3.zero;
                icon.transform.localScale = Vector3.one;
            }
            k++;

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
        m_rank_me.transform.Find("level").GetComponent<UILabel>().text = "[00ff00]" + sys._instance.get_server(int.Parse(sys._instance.m_sid)).m_name; ;

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
            ((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count, sys._instance.m_self.m_t_player.vip, sys._instance.m_self.m_t_player.nalflag);

        _obj2.transform.parent = _icon.transform;
        _obj2.transform.localScale = new Vector3(1, 1, 1);
        _obj2.transform.localPosition = new Vector3(0, 0, 0);
        sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on, m_rank_me.transform.Find("chenhao").gameObject);
        int rank = get_rank();
        if (rank != -1)
        {
            m_rank_me.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language(game_data._instance.get_t_language ("arena.cs_179_81")), (rank + 1));//第{0}名

        }
        else
        {
            m_rank_me.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language(game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81")));//未上榜


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
        if (obj.name == "guild_rank")
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
        else if (obj.name == "guild_reward")
        {
            m_guild_rank_gui.SetActive(false);
            m_geren_rank_gui.SetActive(false);
            m_reward_gui.SetActive(true);
            reset_guildreward(1);
 
        }
        else if (obj.name == "geren_reward")
        {
            m_guild_rank_gui.SetActive(false);
            m_geren_rank_gui.SetActive(false);
            m_reward_gui.SetActive(true);
            reset_guildreward(2);
 
        }
        
    }
}
