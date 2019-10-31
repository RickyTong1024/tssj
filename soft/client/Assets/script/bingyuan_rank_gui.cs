using System.Collections.Generic;
using UnityEngine;

public class bingyuan_rank_gui : MonoBehaviour, IMessage
{
    public GameObject m_rank_panel;
    public GameObject m_rank_scro;
    public GameObject m_chenhao_panel;
    public GameObject m_chenhao_scro;
    public GameObject m_mychenghao;
    public GameObject m_reward_panel;
    public GameObject m_reward_scro;
    public GameObject m_rank_me;
    protocol.game.smsg_rank_view m_rank_msg;
    public UIToggle m_button_rank;

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
        net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_VIEW_CHENGHAO_RANK);
    }

    void IMessage.message(s_message message)
    {

    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.SMSG_VIEW_CHENGHAO_RANK)
        {
            m_rank_msg = net_tcp_bingyua._instance.parse_packet<protocol.game.smsg_rank_view>(message.m_byte);
            reset_rank();
        }
    }

    void reset_rank()
    {
        m_rank_panel.SetActive(true);
        m_chenhao_panel.SetActive(false);
        m_reward_panel.SetActive(false);
        if (m_rank_scro.GetComponent<SpringPanel>() != null)
        {
            m_rank_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_rank_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_rank_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_rank_scro);

        for (int i = 0; i < m_rank_msg.rank_list.player_guid.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/bingyuan_list_sub");

            obj.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_179_81"), (i + 1));//第{0}名
            obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color
                (m_rank_msg.rank_list.player_achieve[i]) + m_rank_msg.rank_list.player_name[i].ToString();
            obj.transform.Find("level").GetComponent<UILabel>().text = sys._instance.get_server(m_rank_msg.rank_list.player_level[i]).m_name.ToString(); ;
            obj.transform.Find("hurt").GetComponent<UILabel>().text = m_rank_msg.rank_list.value[i] + "";
            obj.name = i + "";
            UIEventListener.Get(obj.transform.Find("look").gameObject).onClick = look;
            GameObject _icon = obj.transform.Find("icon").gameObject;
            sys._instance.remove_child(_icon);
            GameObject _obj = icon_manager._instance.create_player_icon((int)m_rank_msg.rank_list.player_template[i], m_rank_msg.rank_list.player_achieve[i], m_rank_msg.rank_list.player_vip[i], m_rank_msg.rank_list.player_nalflag[i]);

            _obj.transform.parent = _icon.transform;
            _obj.transform.localScale = new Vector3(1, 1, 1);
            _obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.parent = m_rank_scro.transform;
            obj.transform.localPosition = new Vector3(0, 143 - i * 70, 0);
            obj.transform.localScale = Vector3.one;
        }
        if (bingyuan_gui._instance.m_world.rank != -1)
        {
            m_rank_me.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_179_81"), bingyuan_gui._instance.m_world.rank);//第{0}名

        }
        else
        {
            m_rank_me.transform.Find("rank").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_rank_gui.cs_83_81");//未上榜

        }
        m_rank_me.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color
            (sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name.ToString();
        for (int i = 0; i < game_data._instance.m_server_list.Count; i++)
        {
            if (game_data._instance.m_server_list[i].m_id == game_data._instance.m_player_data.m_id.ToString())
            {
                m_rank_me.transform.Find("level").GetComponent<UILabel>().text = game_data._instance.m_server_list[i].m_name.ToString(); ;
            }
        }
        m_rank_me.transform.Find("hurt").GetComponent<UILabel>().text = bingyuan_gui._instance.m_world.point + "";
        GameObject m_icon = m_rank_me.transform.Find("icon").gameObject;
        sys._instance.remove_child(m_icon);
        GameObject _obj1 = icon_manager._instance.create_player_icon
            ((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count, sys._instance.m_self.m_t_player.vip, sys._instance.m_self.m_t_player.nalflag);
        _obj1.transform.parent = m_icon.transform;
        _obj1.transform.localScale = new Vector3(1, 1, 1);
        _obj1.transform.localPosition = new Vector3(0, 0, 0);
    }

    void look(GameObject obj)
    {
        int index = int.Parse(obj.transform.parent.name);
        protocol.team.cmsg_player_look _msg = new protocol.team.cmsg_player_look();
        _msg.guid = m_rank_msg.rank_list.player_guid[index];
        net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_player_look>(opclient_t.CMSG_VIEW_TEAM_MEMBER, _msg);
    }

    server_list get_index(int s)
    {
        List<server_list> serverlist = game_data._instance.m_server_list;
        for (int i = 0; i < serverlist.Count; i++)
        {
            if (serverlist[i].m_id == s.ToString())
            {
                return serverlist[i];
            }
        }
        return serverlist[0];
    }

    void reset_reward()
    {
        m_rank_panel.SetActive(false);
        m_chenhao_panel.SetActive(false);
        m_reward_panel.SetActive(true);
        int k = 0;
        if (m_reward_scro.GetComponent<SpringPanel>() != null)
        {
            m_reward_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_reward_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_reward_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_reward_scro);
        foreach (int id in game_data._instance.m_dbc_bingyuan_reward.m_index.Keys)
        {
            s_t_bingyuan_reward _reard = game_data._instance.get_t_bingyuan_reward(id);
            GameObject obj = game_data._instance.ins_object_res("ui/boss_ph_item");
            obj.transform.parent = m_reward_scro.transform;
            obj.transform.localPosition = new Vector3(0, 145 - k * 120, 0);
            obj.transform.localScale = Vector3.one;
            if (_reard.rank1 == _reard.rank2)
            {
                obj.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_179_81"), _reard.rank1);//第{0}名
            }
            else
            {
                obj.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_182_82"), _reard.rank1, _reard.rank2);//第{0}-{1}名
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
        m_chenhao_panel.SetActive(true);
        m_reward_panel.SetActive(false);
        int k = 0;
        if (m_chenhao_scro.GetComponent<SpringPanel>() != null)
        {
            m_chenhao_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_chenhao_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_chenhao_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_chenhao_scro);
        foreach (int id in game_data._instance.m_dbc_bingyuan_chenhao.m_index.Keys)
        {
            s_t_bingyuan_chenhao _bingyuanchenhao = game_data._instance.get_t_bingyuan_chenghao(id);
            s_t_chenghao _chenhao = game_data._instance.get_t_chenhao(_bingyuanchenhao.chenhaoid);
            GameObject obj = game_data._instance.ins_object_res("ui/bingyuan_chenhao_item");
            obj.transform.parent = m_chenhao_scro.transform;
            obj.transform.localPosition = new Vector3(0, 103 - k * 84, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("name").GetComponent<UILabel>().text = _chenhao.name;
            obj.transform.Find("desc").GetComponent<UILabel>().text = "";
            if (_chenhao.attr.Count == 0)
            {
                obj.transform.Find("desc").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_rank_gui.cs_187_79");//无属性加成
            }
            else
            {
                for (int i = 0; i < _chenhao.attr.Count; i++)
                {
                    obj.transform.Find("desc").GetComponent<UILabel>().text += obj.transform.Find("desc").GetComponent<UILabel>().text =
                        game_data._instance.get_value_string(_chenhao.attr[i].attr, _chenhao.attr[i].value) + " ";
                }
            }

            obj.transform.Find("needpaiming").GetComponent<UILabel>().text = _bingyuanchenhao.rank + "";
            obj.transform.Find("needjifen").GetComponent<UILabel>().text = _bingyuanchenhao.jifen + "";
            k++;
        }
        int chen_hao_id = bingyuan_gui._instance.m_world.chenghao;// get_chenhao_id();
        s_t_bingyuan_chenhao _chenhao1 = game_data._instance.get_t_bingyuan_chenghao(chen_hao_id);
        if (_chenhao1 == null)
        {
            _chenhao1 = game_data._instance.get_t_bingyuan_chenghao(7);
        }
        s_t_chenghao _chenhao2 = game_data._instance.get_t_chenhao(_chenhao1.chenhaoid);
        m_mychenghao.transform.Find("name").GetComponent<UILabel>().text = _chenhao2.name;
        m_mychenghao.transform.Find("desc").GetComponent<UILabel>().text = "";
        if (_chenhao2.attr.Count == 0)
        {
            m_mychenghao.transform.Find("desc").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_rank_gui.cs_187_79");//无属性加成
        }
        else
        {
            for (int i = 0; i < _chenhao2.attr.Count; i++)
            {
                m_mychenghao.transform.Find("desc").GetComponent<UILabel>().text += m_mychenghao.transform.Find("desc").GetComponent<UILabel>().text =
                    game_data._instance.get_value_string(_chenhao2.attr[i].attr, _chenhao2.attr[i].value) + " ";
            }
        }

        m_mychenghao.transform.Find("needpaiming").GetComponent<UILabel>().text = _chenhao1.rank + "";
        m_mychenghao.transform.Find("needjifen").GetComponent<UILabel>().text = _chenhao1.jifen + "";
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
        bingyuan_gui._instance.m_can_move = true;
    }

    void click(GameObject obj)
    {
        if (obj.name == "rank")
        {
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_VIEW_CHENGHAO_RANK);
        }
        else if (obj.name == "reward")
        {
            reset_reward();
        }
        else if (obj.name == "chenghao")
        {
            reset_chenhao();
        }
        else if (obj.name == "close")
        {
            close();
        }
    }
}
