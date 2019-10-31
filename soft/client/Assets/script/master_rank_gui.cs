
using UnityEngine;
using System.Collections;

public class master_rank_gui : MonoBehaviour,IMessage{

    public GameObject m_rank_panel;
    public GameObject m_rank_scro;

    public GameObject m_duanwei_panel;
    public GameObject m_duanwei_scro;
    public GameObject m_mychenghao;
    public GameObject m_reward_panel;
    public GameObject m_reward_scro;
    public GameObject m_rank_me;
    protocol.game.smsg_rank_view m_rank_msg;
    protocol.game.smsg_rank_view m_rank_msg_quanfu;
    public UIToggle m_button_rank;
    public UILabel m_text;
    public int type;
    // Use this for initialization
    void Awake()
    {
        cmessage_center._instance.add_handle(this);
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void OnEnable()
    {
        protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view();
        _msg.type = 17;
        net_http._instance.send_msg<protocol.game.cmsg_rank_view>(opclient_t.CMSG_RANK_VIEW, _msg);
    }
    void IMessage.message(s_message message)
    {

    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_RANK_VIEW)
        {
            m_rank_msg = net_tcp_bingyua._instance.parse_packet<protocol.game.smsg_rank_view>(message.m_byte);
            reset_rank(1);
        }
        else if (message.m_opcode == opclient_t.SMSG_VIEW_DS_RANK)
        {
            m_rank_msg = net_tcp_bingyua._instance.parse_packet<protocol.game.smsg_rank_view>(message.m_byte);
            reset_rank(2);
 
        }
    }
    void reset_rank(int type)
    {
        m_rank_panel.SetActive(true);
        m_reward_panel.SetActive(false);
        m_duanwei_panel.SetActive(false);
        if (m_rank_scro.GetComponent<SpringPanel>() != null)
        {
            m_rank_scro.GetComponent<SpringPanel>().enabled = false;
        }
        this.type = type;
        if (type == 1)
        {
            m_text.text = game_data._instance.get_t_language ("master_rank_gui.cs_65_26");//战力
        }
        else
        {
            m_text.text = game_data._instance.get_t_language ("master_rank_gui.cs_69_26");//区服
        }
        m_rank_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_rank_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_rank_scro);

        for (int i = 0; i < m_rank_msg.rank_list.player_guid.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/master_rank_sub");

            obj.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), (i + 1));//第{0}名
            obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color
                (m_rank_msg.rank_list.player_achieve[i]) + m_rank_msg.rank_list.player_name[i].ToString();

            if (type != 1)
            {
                obj.transform.Find("level").GetComponent<UILabel>().text = "[00ff00]" + sys._instance.get_server(m_rank_msg.rank_list.player_level[i]).m_name.ToString(); ;
            }
            else
            {
                obj.transform.Find("level").GetComponent<UILabel>().text = "[F06F28]" + sys._instance.value_to_wan(m_rank_msg.rank_list.player_bf[i]);
            }
            obj.transform.Find("hurt").GetComponent<UILabel>().text = m_rank_msg.rank_list.value[i] + "";
            obj.name = i + "";
            UIEventListener.Get(obj.transform.Find("look").gameObject).onClick = look;
            GameObject _icon = obj.transform.Find("icon").gameObject;
            sys._instance.remove_child(_icon);
            GameObject _obj = icon_manager._instance.create_player_icon((int)m_rank_msg.rank_list.player_template[i], m_rank_msg.rank_list.player_achieve[i], m_rank_msg.rank_list.player_vip[i],m_rank_msg.rank_list.player_nalflag[i]);

            _obj.transform.parent = _icon.transform;
            _obj.transform.localScale = new Vector3(1, 1, 1);
            _obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.parent = m_rank_scro.transform;
            obj.transform.localPosition = new Vector3(0, 143 - i * 70, 0);
            obj.transform.localScale = Vector3.one;
        }
        m_rank_me.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color
            (sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name.ToString();
        if (type == 1)
        {
            m_rank_me.transform.Find("level").GetComponent<UILabel>().text = "[F06F28]" + sys._instance.value_to_wan(sys._instance.m_self.m_t_player.bf);
        }
        else
        {
			for (int i = 0; i < game_data._instance.m_server_list.Count; i++) 
			{
				if(game_data._instance.m_server_list[i].m_id == game_data._instance.m_player_data.m_id.ToString())
				{	
					m_rank_me.transform.Find("level").GetComponent<UILabel>().text = "[00ff00]" + game_data._instance.m_server_list[i].m_name.ToString(); ;
				}
			}
           
        }
     
        m_rank_me.transform.Find("hurt").GetComponent<UILabel>().text = masterleague_gui._instance.m_world.spoint + "";

        GameObject m_icon = m_rank_me.transform.Find("icon").gameObject;
        sys._instance.remove_child(m_icon);
        GameObject _obj1 = icon_manager._instance.create_player_icon
            ((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count, sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);

        _obj1.transform.parent = m_icon.transform;
        _obj1.transform.localScale = new Vector3(1, 1, 1);
        _obj1.transform.localPosition = new Vector3(0, 0, 0);
		int rank = get_rank ();
		if (rank != -1) {
			m_rank_me.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), (rank + 1));//第{0}名

				} else {
			m_rank_me.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81"));//未上榜
		

		}

    }
	int get_rank()
	{
		if (m_rank_msg != null) 
		{
			for(int i = 0;i < m_rank_msg.rank_list.player_guid.Count;i ++)
			{

				if(sys._instance.m_self.m_guid == m_rank_msg.rank_list.player_guid[i])
				{
					return i;

				}
			}


		}
		return -1;

	}
    void look(GameObject obj)
    {
        int index = int.Parse(obj.transform.parent.name);
       
        if (type == 1)
        {
            protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look();
            _msg.target_guid = m_rank_msg.rank_list.player_guid[index];
            net_http._instance.send_msg<protocol.game.cmsg_player_look>(opclient_t.CMSG_PLAYER_LOOK, _msg);

        }
        else
        {
            protocol.team.cmsg_player_look _msg = new protocol.team.cmsg_player_look();
            _msg.guid = m_rank_msg.rank_list.player_guid[index];
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_player_look>(opclient_t.CMSG_VIEW_TEAM_MEMBER, _msg);
 
        }
    }

    void reset_reward()
    {
        m_rank_panel.SetActive(false);
        m_duanwei_panel.SetActive(false);
        m_reward_panel.SetActive(true);
        int k = 0;
        if (m_reward_scro.GetComponent<SpringPanel>() != null)
        {
            m_reward_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_reward_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_reward_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_reward_scro);
        foreach (int id in game_data._instance.m_master_rewards.Keys)
        {
            s_t_master_reward _reard = game_data._instance.get_t_master_reward(id);
            GameObject obj = game_data._instance.ins_object_res("ui/boss_ph_item");
            obj.transform.parent = m_reward_scro.transform;
            obj.transform.localPosition = new Vector3(0, 145 - k * 120, 0);
            obj.transform.localScale = Vector3.one;
            if (_reard.rank1 == _reard.rank2)
            {
                obj.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), _reard.rank1);//第{0}名
            }
            else
            {
                obj.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_182_82"), _reard.rank1, _reard.rank2);//第{0}-{1}名
            }
            for (int i = 0; i < _reard.rewards.Count; i++)
            {
                Transform parent = obj.transform.Find(i + "");
                GameObject icon = icon_manager._instance.create_reward_icon(_reard.rewards[i].type, _reard.rewards[i].value1, _reard.rewards[i].value2,
                    _reard.rewards[i].value3);
                icon.transform.parent = parent;
                icon.transform.localPosition = Vector3.zero;
                icon.transform.localScale = Vector3.one;
            }
            k++;

        }

    }
    void reset_chenhao()
    {
        m_rank_panel.SetActive(false);
        m_duanwei_panel.SetActive(true);
        m_reward_panel.SetActive(false);
        int k = 0;
        if (m_duanwei_scro.GetComponent<SpringPanel>() != null)
        {
            m_duanwei_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_duanwei_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_duanwei_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_duanwei_scro);
        foreach (int id in game_data._instance.m_master_duanweis.Keys)
        {
            s_t_master_duanwei _duanwei = game_data._instance.get_t_master_duanwei(id);
            GameObject obj = game_data._instance.ins_object_res("ui/bingyuan_chenhao_item");
            obj.transform.parent = m_duanwei_scro.transform;
            obj.transform.localPosition = new Vector3(0, 103 - k * 84, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("name").GetComponent<UILabel>().text = _duanwei.duanwei;
            obj.transform.Find("name").GetComponent<UILabel>().gradientTop = sys._instance.string_to_color(_duanwei.topcolor);
            obj.transform.Find("name").GetComponent<UILabel>().gradientBottom = sys._instance.string_to_color(_duanwei.bottomcolor);
            obj.transform.Find("desc").GetComponent<UILabel>().text = "";
            {
     
                {
                    if (_duanwei.value1 == 0 && _duanwei.value2 == 0)
                    {
                        obj.transform.Find("desc").GetComponent<UILabel>().text += game_data._instance.get_t_language ("bingyuan_rank_gui.cs_187_79");//无属性加成
                    }
                    else
                    {
                        obj.transform.Find("desc").GetComponent<UILabel>().text +=
                       game_data._instance.get_value_string(_duanwei.attr1, (float)_duanwei.value1) + " ";
                        obj.transform.Find("desc").GetComponent<UILabel>().text +=
                           game_data._instance.get_value_string(_duanwei.attr2, (float)_duanwei.value2) + " ";
 
                    }
                   
                }

            }


            obj.transform.Find("needpaiming").GetComponent<UILabel>().text = _duanwei.need_rank + "";
            obj.transform.Find("needjifen").GetComponent<UILabel>().text = _duanwei.need_jifen + "";
            k++;
        }
        int duanwei = masterleague_gui._instance.m_world.sduanwei;// get_chenhao_id();
        s_t_master_duanwei _myduanwei = game_data._instance.get_t_master_duanwei(duanwei);
        if (_myduanwei == null)
        {
            _myduanwei = game_data._instance.get_t_master_duanwei(1);
        }
        m_mychenghao.transform.Find("name").GetComponent<UILabel>().text = _myduanwei.duanwei;
        m_mychenghao.transform.Find("name").GetComponent<UILabel>().gradientTop = sys._instance.string_to_color(_myduanwei.topcolor);
        m_mychenghao.transform.Find("name").GetComponent<UILabel>().gradientBottom = sys._instance.string_to_color(_myduanwei.bottomcolor);
        m_mychenghao.transform.Find("desc").GetComponent<UILabel>().text = "";

        if (_myduanwei.value1 == 0 && _myduanwei.value2 == 0)
        {
            m_mychenghao.transform.Find("desc").GetComponent<UILabel>().text += game_data._instance.get_t_language ("bingyuan_rank_gui.cs_187_79");//无属性加成
        }
        else
        {
            m_mychenghao.transform.Find("desc").GetComponent<UILabel>().text +=
                      game_data._instance.get_value_string(_myduanwei.attr1, (float)_myduanwei.value1) + " ";
            m_mychenghao.transform.Find("desc").GetComponent<UILabel>().text +=
               game_data._instance.get_value_string(_myduanwei.attr2, (float)_myduanwei.value2) + " ";
 
        }
      

        m_mychenghao.transform.Find("needpaiming").GetComponent<UILabel>().text = _myduanwei.need_rank + "";
        m_mychenghao.transform.Find("needjifen").GetComponent<UILabel>().text = _myduanwei.need_jifen + "";



    }
    int get_chenhao_id()
    {
        int rank = bingyuan_gui._instance.m_world.rank + 1;
        int _id = 1007;
        for (int i = 0; i < game_data._instance.m_dbc_bingyuan_chenhao.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_bingyuan_chenhao.get(0, i));
            s_t_bingyuan_chenhao _chenhao = game_data._instance.get_t_bingyuan_chenghao(id);
            if (rank <= _chenhao.rank && rank != 0)
            {
                _id = id;
                break;
            }

        }

        return _id;

    }
    void close()
    {
        m_button_rank.value = true;
        this.transform.Find("frame_big").GetComponent<frame>().hide();
    }
    void click(GameObject obj)
    {
        if (obj.name == "danfu_rank")
        {
            protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view();
            _msg.type = 17;
            net_http._instance.send_msg<protocol.game.cmsg_rank_view>(opclient_t.CMSG_RANK_VIEW, _msg);
        }
        else if (obj.name == "quanfu_rank")
        {
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_VIEW_DS_RANK);

        }
        else if (obj.name == "chenghao")
        {
            reset_chenhao();
        }
        else if (obj.name == "close")
        {
            close();
        }
        else if (obj.name == "reward")
        {
            reset_reward();
 
        }
        else if (obj.name == "duanweijiacheng")
        {
            reset_chenhao();
        }
    }
	
}
