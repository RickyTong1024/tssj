using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TeamStat
{
    TS_CREATE,
    TS_PREPARE,
    TS_MATCH,
};

public class bingyuan_gui : MonoBehaviour, IMessage
{
    bool m_join_server = false;
    public protocol.team.team m_team = null;
    public ulong m_team_id;
    public static bingyuan_gui _instance;
    public GameObject m_start_panel;
    public GameObject m_pipei_panel;
    public GameObject m_teams;
    protocol.team.smsg_fight_team m_fight;
    protocol.game.smsg_bingyuan_fight_end m_bingyuan_fight_end;
    public GameObject m_join_team;
    public GameObject m_create_team;
    public GameObject m_start_me;
    public bingyuan_battle bingyuan_battle;
    public GameObject m_wait_pipei;
    public GameObject m_b_fight;
    public GameObject m_b_prepare;
    public GameObject m_b_prepare_cancel;
    public GameObject m_l_kaiqi;
    public GameObject m_op;
    private int _index;
    private int _num;
    private bool m_result = false;
    private int m_fight_time = 30;
    public GameObject m_effect_shop;
    public GameObject m_close_pipei;
    public GameObject m_prepare_tishi;
    public GameObject m_prepare_tishi1;
    public GameObject m_button_shop;
    public GameObject m_yaoqing_panel;
    public GameObject m_yaoqing_scro;
    public GameObject m_duiwu_error;
    public GameObject m_duiwu_error1;
    public GameObject m_bingyuan_shop;
    public GameObject m_duiwu_panel;
    public GameObject m_duiwu_scro;
    public GameObject m_button_zudui;
    public GameObject m_rank_panel;
    public GameObject m_wait_zidong_pipei;
    public GameObject m_move_desc;
    protocol.game.smsg_team_friend_view m_view;
    public GameObject m_yaoqing_error;
    public protocol.team.smsg_enter_world m_world;
    public UILabel m_jifen;
    public UILabel m_rank;
    public UILabel m_reward_num;
    public UILabel m_reward_num1;
    public ulong m_time;
    public List<GameObject> m_uis;
    public int m_reward_jiachneg = 0;
    public int m_reward_jiacheng_result = 0;
    public UILabel m_jiachneg;
    public bingyuan_tip m_tip;
    public GameObject m_info;
    public UIToggle m_toggle_open;
    public GameObject m_info_panel;
    public GameObject m_wofang;
    public GameObject m_difang;
    public GameObject m_win_result;
    public GameObject m_button_cuicu;
    public GameObject m_chat;
    public GameObject m_button_yaoqing;
    public bool m_can_move;
    public GameObject m_tj_panel;
    public GameObject m_buttle_vs;
    public GameObject m_hp_panel;
    public GameObject m_ui_ficht;
    public GameObject m_ui_jibai;
    public List<UILabel> m_end_names;
    public List<UILabel> m_end_chenhaos;
    public List<UILabel> m_end_kills;
    public GameObject m_wait;
    bool m_flag = false;

    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject icon = game_data._instance.ins_object_res("ui/bingyuan_item");
            icon.transform.parent = m_teams.transform;
            icon.transform.localPosition = new Vector3(188 * i, 0, 0);
            icon.transform.localScale = Vector3.one;
            icon.name = i + "";
            UIEventListener.Get(icon).onClick = click;
            UIEventListener.Get(icon.transform.Find("add").gameObject).onClick = click_add;
            m_uis.Add(icon);
        }
    }

    public void add_handle()
    {
        _instance = this;
        cmessage_center._instance.add_handle(this);
        protocol.game.cmsg_common msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_TEAM_ENTER, msg);
        m_join_server = false;
    }

    void OnDestroy()
    {
        net_tcp_bingyua._instance.disconnect();
        print(game_data._instance.get_t_language("bingyuan_gui.cs_123_14"));//主动断开连接
        cmessage_center._instance.remove_handle(this);
    }

    void create_class()
    {
        m_jifen.text = m_world.point + "";
        if (m_world.rank != -1)
        {
            m_rank.gameObject.SetActive(true);
            m_rank.text = m_world.rank + "";
            m_rank.transform.parent.transform.Find("wei").gameObject.SetActive(false);
        }
        else
        {
            m_rank.gameObject.SetActive(false);
            m_rank.transform.parent.transform.Find("wei").gameObject.SetActive(true);
        }
        m_reward_num.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
        m_reward_num1.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
        m_l_kaiqi.SetActive(false);
        s_message _mes = new s_message();
        _mes.m_type = "bingyuan_start";
        cmessage_center._instance.add_message(_mes);
        sys._instance.remove_child(m_start_me);
        create_team(m_start_me.transform);
    }

    GameObject create_team(Transform parent)
    {
        GameObject icon = game_data._instance.ins_object_res("ui/bingyuan_item");
        icon.transform.Find("prepare").gameObject.SetActive(false);
        icon.transform.Find("leader").gameObject.SetActive(false);
        icon.transform.Find("main/name").GetComponent<UILabel>().text = game_data._instance.get_name_color
            (sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name;
        icon.transform.Find("main/attack").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_fight.cs_43_118") +//战力 
            sys._instance.value_to_wan(sys._instance.m_self.m_t_player.bf);
        icon.transform.Find("add").gameObject.SetActive(false);
        icon.transform.Find("kuang").gameObject.SetActive(false);
        icon.transform.Find("main").localPosition = new Vector3(0, 66, 0);

        if (platform_config_common.m_nationality > 0)
        {
            if (sys._instance.m_self.m_t_player.nalflag != 0)
            {
                icon.transform.Find("main/gq").GetComponent<UISprite>().spriteName = sys._instance.returnfirstSpritename(sys._instance.m_self.m_t_player.nalflag);
                icon.transform.Find("main/gq").gameObject.SetActive(true);
            }
            else
            {
                icon.transform.Find("main/gq").gameObject.SetActive(false);
            }
        }
        else
        {
            icon.transform.Find("main/gq").gameObject.SetActive(false);
        }

        s_t_bingyuan_chenhao _chenhao = game_data._instance.get_t_bingyuan_chenghao(m_world.chenghao);
        if (_chenhao != null)
        {
            icon.transform.Find("main/chenhao").GetComponent<UILabel>().text = game_data._instance.get_t_chenhao(_chenhao.chenhaoid).name;
        }
        else
        {
            _chenhao = game_data._instance.get_t_bingyuan_chenghao(7);
            icon.transform.Find("main/chenhao").GetComponent<UILabel>().text = game_data._instance.get_t_chenhao(_chenhao.chenhaoid).name;
        }
        icon.transform.parent = parent;
        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.one;
        return icon;
    }

    void reset()
    {
        create_class();
        if (m_world.invites.Count > 0)
        {
            m_button_zudui.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_button_zudui.transform.Find("effect").gameObject.SetActive(false);
        }

        m_can_move = false;
        if (!m_win_result.activeSelf && !bingyuan_battle.gameObject.activeSelf && !m_info_panel.activeSelf)
        {
            m_button_shop.SetActive(true);
            m_start_panel.SetActive(true);
            m_info.SetActive(true);
        }
        m_team = null;
        m_win_result.SetActive(false);
        m_pipei_panel.SetActive(false);
        m_wait_pipei.SetActive(false);

        m_jiachneg.gameObject.SetActive(false);
        m_wait_zidong_pipei.SetActive(false);
        bingyuan_shop_effect();
    }

    public void bingyuan_shop_effect()
    {
        if (bingyuan_shop.effect())
        {
            m_effect_shop.SetActive(true);
        }
        else
        {
            m_effect_shop.SetActive(false);
        }
    }


    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "bingyuan_login_check")
        {
            protocol.team.cmsg_enter_world msg = new protocol.team.cmsg_enter_world();
            msg.guid = sys._instance.m_self.m_t_player.guid;
            msg.sig = sys._instance.m_self.m_sig;
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_enter_world>(opclient_t.CMSG_ENTER_TEAM_SERVER, msg);
        }
        else if (mes.m_type == "kick_member")
        {
            protocol.team.cmsg_team_kick _mes = new protocol.team.cmsg_team_kick();
            _mes.guid = m_team.players[_index].guid;
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_team_kick>(opclient_t.CMSG_KICK_TEAM, _mes);
        }
        else if (mes.m_type == "buy_bingyuan_reward_num")
        {
            protocol.game.cmsg_shop_buy _msg = new protocol.game.cmsg_shop_buy();
            _msg.num = (int)mes.m_ints[0];
            _num = _msg.num;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_BINGYUAN_BUY_REWARD, _msg);
        }
        else if (mes.m_type == "change_pos")
        {
            int x = (int)mes.m_ints[0];
            int y = (int)mes.m_ints[1];
            protocol.team.cmsg_team_move _move = new protocol.team.cmsg_team_move();
            _move.guid = m_team.players[x].guid;
            _move.index = y;
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_team_move>(opclient_t.CMSG_MOVE_TEAM, _move);
        }
        else if (mes.m_type == "bingyuan_repare")
        {
            prepare();
        }
        else if (mes.m_type == "bingyuan_fight")
        {
            fight();
        }
        else if (mes.m_type == "bingyuan_fight_start")
        {
            bingyuan_battle.start();
        }
        else if (mes.m_type == "show_bingyuanshop")
        {
            m_bingyuan_shop.SetActive(true);
            m_flag = true;
            m_bingyuan_shop.GetComponent<bingyuan_shop>().reset();
        }
        else if (mes.m_type == "bingyuan_battle_end")
        {
            battle_end();
        }
        else if (mes.m_type == "bingyuan_canmove")
        {
            if (!m_rank_panel.activeSelf)
            {
                m_can_move = true;
            }
        }
        else if (mes.m_type == "bingyuan_move")
        {
            m_can_move = true;
        }
    }

    void succes_SMSG_ENTER_TEAM(s_net_message mes)
    {
        protocol.team.smsg_team_enter _player = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_team_enter>(mes.m_byte);
        m_reward_jiachneg = _player.jiacheng;
        m_jiachneg.text = string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_296_40"), m_reward_jiachneg);//组队奖励加成: {0}%
        int index = -1;
        if (m_team != null && _player != null)
        {
            for (int i = 0; i < m_team.players.Count; i++)
            {
                if (m_team.players[i].guid == 0)
                {
                    m_team.players[i] = _player.member;
                    index = i;
                    break;
                }
            }
        }
        enter_team(index);
    }

    void enter_team(int index)
    {
        int leave_id = index;
        GameObject icon = m_uis[leave_id];
        if (m_team.players[leave_id].guid != 0)
        {
            icon.transform.Find("main").gameObject.SetActive(true);
            icon.transform.Find("main/name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_team.players[leave_id].achieve) + m_team.players[leave_id].name;
            icon.transform.Find("main/attack").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_fight.cs_43_118") + sys._instance.value_to_wan(m_team.players[leave_id].bf);//战力 
            icon.transform.Find("add").gameObject.SetActive(false);
            icon.transform.Find("kuang").gameObject.SetActive(false);
            icon.transform.Find("main").localPosition = new Vector3(0, 115, 0);

            if (platform_config_common.m_nationality > 0)
            {
                if (m_team.players[leave_id].nalflag != 0)
                {
                    icon.transform.Find("main/gq").GetComponent<UISprite>().spriteName = sys._instance.returnfirstSpritename(m_team.players[leave_id].nalflag);
                    icon.transform.Find("main/gq").gameObject.SetActive(true);
                }
                else
                {
                    icon.transform.Find("main/gq").gameObject.SetActive(false);
                }
            }
            else
            {
                icon.transform.Find("main/gq").gameObject.SetActive(false);
            }

            s_t_bingyuan_chenhao _chenghao = game_data._instance.get_t_bingyuan_chenghao(m_team.players[leave_id].chenhao);
            if (_chenghao != null)
            {
                icon.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                    game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
            }
            else
            {
                _chenghao = game_data._instance.get_t_bingyuan_chenghao(7);
                icon.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                    game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
            }
            icon.transform.Find("leader").gameObject.SetActive(false);

            if (m_team.players[leave_id].prepare)
            {
                icon.transform.Find("prepare").gameObject.SetActive(true);
            }
            else
            {
                icon.transform.Find("prepare").gameObject.SetActive(false);
            }
            if (m_team.players[leave_id].leader)
            {
                icon.transform.Find("leader").gameObject.SetActive(true);
                icon.transform.Find("prepare").gameObject.SetActive(false);
            }
        }
        else
        {
            icon.transform.Find("main").gameObject.SetActive(false);
            icon.transform.Find("add").gameObject.SetActive(true);
            icon.transform.Find("leader").gameObject.SetActive(false);
            icon.transform.Find("prepare").gameObject.SetActive(false);
            icon.transform.Find("kuang").gameObject.SetActive(true);
            icon.transform.Find("main").localPosition = new Vector3(0, 115, 0);
        }
        if (m_pipei_panel.activeSelf)
        {
            bingyuan_scence._instance.refresh_player_create(leave_id);
        }
    }

    void success_SMSG_CREATE_TEAM(s_net_message mes)
    {
        m_wait_pipei.SetActive(false);
        StopAllCoroutines();
        protocol.team.smsg_team_create _create = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_team_create>(mes.m_byte);
        m_team = _create.team_info;
        m_reward_jiachneg = _create.team_info.jiacheng;
        m_jiachneg.text = string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_296_40"), m_reward_jiachneg);//组队奖励加成: {0}%
        m_can_move = true;
        if (m_duiwu_panel.activeSelf)
        {
            m_duiwu_panel.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        reset_pipei(true);
    }

    void success_SMSG_MOVE_TEAM(s_net_message mes)
    {
        protocol.team.smsg_team_move _mes = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_team_move>(mes.m_byte);
        int index1 = 0;
        int index2 = _mes.index;
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == _mes.guid)
            {
                index1 = i;
                break;
            }
        }
        if (index1 != index2)
        {
            protocol.team.team_player temp = m_team.players[index1];
            m_team.players[index1] = m_team.players[index2];
            m_team.players[index2] = temp;
            refresh_player(index1, index2);
        }
    }

    void refresh_player(int index1, int index2)
    {
        m_uis[index1].name = index2 + "";
        m_uis[index2].name = index1 + "";

        m_uis[index1].transform.localPosition = new Vector3(188 * index2, 0, 0);
        m_uis[index2].transform.localPosition = new Vector3(188 * index1, 0, 0);

        GameObject obj = m_uis[index1];
        m_uis[index1] = m_uis[index2];
        m_uis[index2] = obj;

        s_message _mes = new s_message();
        _mes.m_type = "bingyuan_refresh_player";
        _mes.m_ints.Add(index1);
        _mes.m_ints.Add(index2);
        cmessage_center._instance.add_message(_mes);
    }

    void refresh_player(int index1)
    {
        GameObject icon = m_uis[index1];
        if (m_team.players[index1].guid != 0)
        {
            icon.transform.Find("main/name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_team.players[index1].achieve) + m_team.players[index1].name;
            icon.transform.Find("main/attack").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_fight.cs_43_118") + sys._instance.value_to_wan(m_team.players[index1].bf);//战力 
            icon.transform.Find("add").gameObject.SetActive(false);
            icon.transform.Find("kuang").gameObject.SetActive(false);
            s_t_bingyuan_chenhao _chenghao = game_data._instance.get_t_bingyuan_chenghao(m_team.players[index1].chenhao);
            if (_chenghao != null)
            {
                icon.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                    game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
            }
            else
            {
                _chenghao = game_data._instance.get_t_bingyuan_chenghao(7);
                icon.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                    game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
            }
            icon.transform.Find("leader").gameObject.SetActive(false);

            if (m_team.players[index1].prepare)
            {
                icon.transform.Find("prepare").gameObject.SetActive(true);
            }
            else
            {
                icon.transform.Find("prepare").gameObject.SetActive(false);
            }

            if (m_team.players[index1].leader)
            {
                icon.transform.Find("leader").gameObject.SetActive(true);
                icon.transform.Find("prepare").gameObject.SetActive(false);
            }
        }
        else
        {
            icon.transform.Find("main").gameObject.SetActive(false);
            icon.transform.Find("add").gameObject.SetActive(true);
            icon.transform.Find("leader").gameObject.SetActive(false);
            icon.transform.Find("prepare").gameObject.SetActive(false);
            icon.transform.Find("kuang").gameObject.SetActive(true);
        }
    }

    void success_SMSG_BINGYUAN_FIGHT_END(s_net_message mes)
    {
        StopAllCoroutines();
        m_wait_zidong_pipei.transform.Find("frame_big").GetComponent<frame>().hide();
        m_pipei_panel.SetActive(false);
        m_info.SetActive(false);
        m_fight = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_fight_team>(mes.m_byte);
        protocol.game.cmsg_bingyuan_fight _f = new protocol.game.cmsg_bingyuan_fight();
        _f.id = m_fight.id;
        net_http._instance.send_msg<protocol.game.cmsg_bingyuan_fight>(opclient_t.CMSG_BINGYUAN_FIGHT_END, _f);
        bingyuan_battle.m_team_id = m_team.team_id;
        m_team_id = m_team.team_id;
        bingyuan_battle.reset(m_fight);
        m_l_kaiqi.SetActive(false);
        StopAllCoroutines();
        m_reward_jiacheng_result = m_reward_jiachneg;
        sys._instance.m_game_state = "bingyuan_buttle";
        sys._instance.load_scene("ts_chapter09_01");
        s_message _mes = new s_message();
        _mes.m_type = "remove_unit";
        cmessage_center._instance.add_message(_mes);
        bingyuan_battle.gameObject.SetActive(true);
    }

    void success_CMSG_BINGYUAN_FIGHT_END(s_net_message mes)
    {
        m_bingyuan_fight_end = net_http._instance.parse_packet<protocol.game.smsg_bingyuan_fight_end>(mes.m_byte);
    }

    void success_SMSG_LEAVE_TEAM(s_net_message mes)
    {
        if (m_team == null)
        {
            return;
        }
        protocol.team.smsg_team_leave pl = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_team_leave>(mes.m_byte);
        m_reward_jiachneg = pl.jiacheng;
        m_jiachneg.text = string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_296_40"), m_reward_jiachneg);//组队奖励加成: {0}%
        int leave_id = -1;
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == pl.guid)
            {
                m_team.players[i].guid = 0;
                m_team.players[i].prepare = false;
                leave_id = i;
            }
            if (pl.leader != 0)
            {
                if (m_team.players[i].guid == pl.leader)
                {
                    m_team.players[i].leader = true;
                }
                else
                {
                    m_team.players[i].leader = false;
                }
            }
        }

        if (!is_leave_duiwu())
        {
            leave_team(leave_id);

            {
                leave_reset();
            }
        }
        else
        {
            if (!bingyuan_battle.gameObject.activeSelf)
            {
                reset();
            }
        }
        m_op.SetActive(false);
        s_message _ms = new s_message();
        _ms.m_type = "hide_player_info_panel";
        cmessage_center._instance.add_message(_ms);
    }

    void leave_team(int leave_id)
    {
        Destroy(m_uis[leave_id]);
        m_uis[leave_id] = null;
        if (bingyuan_scence._instance.gameObj)
        {
            if (leave_id.ToString() == bingyuan_scence._instance.gameObj.name)
            {
                bingyuan_scence._instance.gameObj = null;
            }
        }

        GameObject icon = game_data._instance.ins_object_res("ui/bingyuan_item");
        icon.transform.parent = m_teams.transform;
        icon.transform.localPosition = new Vector3(188 * leave_id, 0, 0);
        icon.transform.localScale = Vector3.one;
        icon.name = leave_id + "";
        UIEventListener.Get(icon).onClick = click;
        UIEventListener.Get(icon.transform.Find("add").gameObject).onClick = click_add;
        m_uis[leave_id] = icon;

        if (m_team.players[leave_id].guid != 0)
        {
            icon.transform.Find("main").gameObject.SetActive(true);
            icon.transform.Find("main/name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_team.players[leave_id].achieve) + m_team.players[leave_id].name;
            icon.transform.Find("main/attack").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_fight.cs_43_118") + sys._instance.value_to_wan(m_team.players[leave_id].bf);//战力 
            icon.transform.Find("add").gameObject.SetActive(false);
            icon.transform.Find("kuang").gameObject.SetActive(false);
            icon.transform.Find("main").localPosition = new Vector3(0, 115, 0);

            if (platform_config_common.m_nationality > 0)
            {
                if (m_team.players[leave_id].nalflag != 0)
                {
                    icon.transform.Find("main/gq").GetComponent<UISprite>().spriteName = sys._instance.returnfirstSpritename(m_team.players[leave_id].nalflag);
                    icon.transform.Find("main/gq").gameObject.SetActive(true);
                }
                else
                {
                    icon.transform.Find("main/gq").gameObject.SetActive(false);
                }
            }
            else
            {
                icon.transform.Find("main/gq").gameObject.SetActive(false);
            }
            s_t_bingyuan_chenhao _chenghao = game_data._instance.get_t_bingyuan_chenghao(m_team.players[leave_id].chenhao);
            if (_chenghao != null)
            {
                icon.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                    game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
            }
            else
            {
                _chenghao = game_data._instance.get_t_bingyuan_chenghao(7);
                icon.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                    game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
            }
            icon.transform.Find("leader").gameObject.SetActive(false);

            if (m_team.players[leave_id].prepare)
            {
                icon.transform.Find("prepare").gameObject.SetActive(true);
            }
            else
            {
                icon.transform.Find("prepare").gameObject.SetActive(false);
            }
            if (m_team.players[leave_id].leader)
            {
                icon.transform.Find("leader").gameObject.SetActive(true);
                icon.transform.Find("prepare").gameObject.SetActive(false);
            }
        }
        else
        {
            icon.transform.Find("main").gameObject.SetActive(false);
            icon.transform.Find("add").gameObject.SetActive(true);
            icon.transform.Find("leader").gameObject.SetActive(false);
            icon.transform.Find("prepare").gameObject.SetActive(false);
            icon.transform.Find("kuang").gameObject.SetActive(true);
            icon.transform.Find("main").localPosition = new Vector3(0, 115, 0);
        }
        s_message _mes = new s_message();
        _mes.m_type = "bingyuan_pipei";
        _mes.m_bools.Add(is_leader());
        _mes.m_bools.Add(false);
        cmessage_center._instance.add_message(_mes);
        m_can_move = true;
    }

    void success_SMSG_CHANGE_REWARD_NUM(s_net_message mes)
    {
        protocol.team.smsg_team_player_reward_change _change = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_team_player_reward_change>(mes.m_byte);

        if (sys._instance.m_self.m_guid == _change.guid)
        {
            m_world.chenghao = _change.chenghao;
            m_world.point = _change.point;
            m_world.rank = _change.rank;
            sys._instance.m_self.m_t_player.by_reward_num = _change.num;
            m_reward_num.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
            m_reward_num1.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
        }

        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == _change.guid)
            {
                m_team.players[i].reward_num = _change.num;
                m_team.players[i].chenhao = _change.chenghao;
                m_team.players[i].reward_num = _change.num;
                break;
            }
        }

    }

    void success_CMSG_TEAM_INVITE(s_net_message mes)
    {
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == sys._instance.m_self.m_guid)
            {
                m_team.players[i].invites.Add(m_view.guid[_index]);
            }
        }
        reset_yaoqing(1);
    }

    void success_SMSG_INVITE_ADD(s_net_message mes)
    {
        protocol.team.smsg_invite_add _add = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_invite_add>(mes.m_byte);
        m_world.invites.Add(_add.invite);
        if (m_world.invites.Count > 0)
        {
            m_button_zudui.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_button_zudui.transform.Find("effect").gameObject.SetActive(false);
        }
    }

    void success_SMSG_PREPARE_TEAM(s_net_message mes)
    {
        protocol.team.smsg_team_prepare _mes = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_team_prepare>(mes.m_byte);
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (_mes.guid == m_team.players[i].guid)
            {
                m_team.players[i].prepare = _mes.prepare;
                break;
            }
        }
        StopAllCoroutines();
        m_l_kaiqi.SetActive(false);
        refresh_ui_obj();
    }

    void success_SMSG_INVITE_REMOVE(s_net_message mes)
    {
        protocol.team.smsg_invite_remove _mes = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_invite_remove>(mes.m_byte);
        _index = -1;
        for (int i = 0; i < m_world.invites.Count; i++)
        {
            if (m_world.invites[i].invite_id == _mes.invite_id)
            {
                _index = i;
            }
        }
        if (_index != -1)
        {
            if (_index >= 0 && _index < m_world.invites.Count)
            {
                m_world.invites.RemoveAt(_index);
            }
        }
        reset_duiwu(2);
    }

    void success_SMSG_INVITE_REMOVE_SOCIAL(s_net_message mes)
    {
        protocol.team.smsg_invite_remove_social _soc = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_invite_remove_social>(mes.m_byte);
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (sys._instance.m_self.m_guid == m_team.players[i].guid)
            {
                m_team.players[i].invites.Remove(_soc.guid);
                break;
            }
        }
    }

    void success_SMSG_VIEW_TEAM_MEMBER(s_net_message mes)
    {
        m_op.SetActive(false);
    }

    void success_CMSG_BINGYUAN_BUY_REWARD(s_net_message mes)
    {
        int jewel = 0;

        for (int i = 0; i < _num; i++)
        {
            jewel += game_data._instance.get_t_price(sys._instance.m_self.m_t_player.by_reward_buy + 1 + i).bingyuan_reward;
        }
        sys._instance.m_self.sub_att(e_player_attr.player_jewel, jewel, game_data._instance.get_t_language("bingyuan_gui.cs_775_71"));//购买冰原奖励次数
        string s = game_data._instance.get_t_language("bingyuan_gui.cs_776_19");//获得
        root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, "", "[ffffc0]" + "奖励次数" + "[ffd000] + " + _num.ToString());
        sys._instance.m_self.m_t_player.by_reward_buy += _num;
        m_reward_num.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
        m_reward_num1.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == sys._instance.m_self.m_t_player.guid)
            {
                m_team.players[i].reward_num = sys._instance.m_self.m_t_player.by_reward_num;
            }
        }
        m_can_move = true;
    }

    void success_SMSG_TEAM_URGE(s_net_message mes)
    {
        protocol.team.smsg_hanhua msg = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_hanhua>(mes.m_byte);

        int index = 0;
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == msg.guid)
            {
                index = i;
            }
        }
        m_chat.SetActive(false);
        string s = game_data._instance.get_t_language("bingyuan_chat_" + msg.content);
        if (index != 4)
        {
            m_prepare_tishi.SetActive(true);
            m_prepare_tishi1.SetActive(false);
            m_prepare_tishi.transform.position = m_uis[index].transform.position;
            m_prepare_tishi.GetComponent<hide_time>().m_time = 2;
            m_prepare_tishi.transform.Find("text").GetComponent<UILabel>().text = s;
        }
        else
        {
            m_prepare_tishi.SetActive(false);
            m_prepare_tishi1.SetActive(true);
            m_prepare_tishi1.GetComponent<hide_time>().m_time = 2;
            m_prepare_tishi1.transform.Find("text").GetComponent<UILabel>().text = s;
        }
    }

    void IMessage.net_message(s_net_message mes)
    {
        if (mes.m_opcode == opclient_t.CMSG_TEAM_ENTER)
        {
            net_tcp_bingyua._instance.connect();
        }
        else if (mes.m_opcode == opclient_t.SMSG_ENTER_TEAM_SERVER)
        {
            m_world = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_enter_world>(mes.m_byte);
            reset();
        }
        else if (mes.m_opcode == opclient_t.SMSG_ENTER_TEAM)
        {
            succes_SMSG_ENTER_TEAM(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_CREATE_TEAM)
        {
            success_SMSG_CREATE_TEAM(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_MATCH_TEAM)
        {
            m_wait_pipei.SetActive(true);
            StartCoroutine("time");
        }
        else if (mes.m_opcode == opclient_t.SMSG_MOVE_TEAM)
        {
            success_SMSG_MOVE_TEAM(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_MATCH_TEAM_TIMEOUT)
        {
            StopAllCoroutines();
            m_wait_pipei.SetActive(false);
            root_gui._instance.show_prompt_dialog_box("[ffc880]" + game_data._instance.get_t_language("bingyuan_gui.cs_856_67")); //当前没有队伍,试着创建一个
        }
        else if (mes.m_opcode == opclient_t.SMSG_END_MATCH_TEAM)
        {
            StopAllCoroutines();
            m_wait_pipei.SetActive(false);
        }
        else if (mes.m_opcode == opclient_t.SMSG_STOP_FIGHT_TEAM)
        {
            StopAllCoroutines();
            StartCoroutine("kaiqi_time");
            m_wait_zidong_pipei.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (mes.m_opcode == opclient_t.SMSG_BINGYUAN_FIGHT_END)
        {
            success_SMSG_BINGYUAN_FIGHT_END(mes);
        }
        else if (mes.m_opcode == opclient_t.CMSG_BINGYUAN_FIGHT_END)
        {
            success_CMSG_BINGYUAN_FIGHT_END(mes);
        }
        else if (mes.m_opcode == opclient_t.CMSG_TEAM_INVITE_LOOK)
        {
            m_view = net_http._instance.parse_packet<protocol.game.smsg_team_friend_view>(mes.m_byte);
            reset_yaoqing(1);
        }
        else if (mes.m_opcode == opclient_t.SMSG_LEAVE_TEAM)
        {
            success_SMSG_LEAVE_TEAM(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_CHANGE_REWARD_NUM)
        {
            success_SMSG_CHANGE_REWARD_NUM(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_OPEN_TEAM)
        {
            m_team.open = !m_team.open;
            set_toggle();
        }
        else if (mes.m_opcode == opclient_t.SMSG_INVITE_ADD)
        {
            success_SMSG_INVITE_ADD(mes);
        }
        else if (mes.m_opcode == opclient_t.CMSG_TEAM_INVITE)
        {
            success_CMSG_TEAM_INVITE(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_PREPARE_TEAM)
        {
            success_SMSG_PREPARE_TEAM(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_INVITE_REMOVE)
        {
            success_SMSG_INVITE_REMOVE(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_INVITE_REMOVE_SOCIAL)
        {
            success_SMSG_INVITE_REMOVE_SOCIAL(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_VIEW_TEAM_MEMBER)
        {
            success_SMSG_VIEW_TEAM_MEMBER(mes);
        }

        else if (mes.m_opcode == opclient_t.CMSG_BINGYUAN_BUY_REWARD)
        {
            success_CMSG_BINGYUAN_BUY_REWARD(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_TEAM_NOT_EXIST)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language("bingyuan_gui.cs_934_67"));//该队伍已不存在
        }
        else if (mes.m_opcode == opclient_t.SMSG_TEAM_FULL)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language("bingyuan_gui.cs_939_67"));//该队伍已满员
        }
        else if (mes.m_opcode == opclient_t.SMSG_INVITE_ALL)
        {
            protocol.team.smsg_invite_all _all = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_invite_all>(mes.m_byte);
            m_time = _all.next_time;
        }
        else if (mes.m_opcode == opclient_t.SMSG_TEAM_URGE)
        {
            success_SMSG_TEAM_URGE(mes);
        }
        else if (mes.m_opcode == opclient_t.SMSG_CHANGE_TEAM_STAT)
        {
            success_SMSG_CHANGE_TEAM_STAT(mes);
        }
    }

    void success_SMSG_CHANGE_TEAM_STAT(s_net_message mes)
    {
        protocol.team.smsg_change_team_stat _state = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_change_team_stat>(mes.m_byte);
        if (_state.stat == (int)TeamStat.TS_CREATE)
        {
            StopAllCoroutines();
            m_l_kaiqi.SetActive(false);
            if (m_wait_zidong_pipei.activeSelf)
            {
                m_wait_zidong_pipei.transform.Find("frame_big").GetComponent<frame>().hide();
            }
        }
        else if (_state.stat == (int)TeamStat.TS_MATCH)
        {
            //寻找队伍
            m_l_kaiqi.SetActive(false);
            m_wait_zidong_pipei.SetActive(true);
            if (m_yaoqing_panel.activeSelf)
            {
                m_yaoqing_panel.transform.Find("frame_big").GetComponent<frame>().hide();
            }
            if (m_rank_panel.activeSelf)
            {
                m_rank_panel.transform.Find("frame_big").GetComponent<frame>().hide();
            }
            if (m_duiwu_panel.activeSelf)
            {
                m_duiwu_panel.transform.Find("frame_big").GetComponent<frame>().hide();
            }
            if (m_bingyuan_shop.activeSelf)
            {
                m_bingyuan_shop.GetComponent<bingyuan_shop>().hide();
                sys._instance.message_clear();
            }
            s_message _mes = new s_message();
            _mes.m_type = "hide_dui_xing";
            cmessage_center._instance.add_message(_mes);

            s_message _ms = new s_message();
            _ms.m_type = "hide_common_player_panel";
            cmessage_center._instance.add_message(_ms);
            _mes = new s_message();
            _mes.m_type = "hide_cishu_dialog_box";
            cmessage_center._instance.add_message(_mes);
            if (is_leader())
            {
                m_close_pipei.SetActive(true);
            }
            else
            {
                m_close_pipei.SetActive(false);
            }
            StartCoroutine("time1");
        }
        else if (_state.stat == (int)TeamStat.TS_PREPARE)
        {
            StopAllCoroutines();
            if (m_wait_zidong_pipei.activeSelf)
            {
                m_wait_zidong_pipei.transform.Find("frame_big").GetComponent<frame>().hide();
            }
            m_can_move = true;
            //30秒倒计时
            m_l_kaiqi.SetActive(true);
            m_fight_time = 30;
            StartCoroutine(kaiqi_time());
        }

    }

    void set_toggle()
    {
        m_toggle_open.value = m_team.open;
    }

    void reset_duiwu(int type)
    {

        if (m_duiwu_scro.GetComponent<SpringPanel>() != null)
        {
            m_duiwu_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_duiwu_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_duiwu_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_duiwu_scro);
        List<protocol.team.team_invite> temp = new List<protocol.team.team_invite>();
        for (int i = 0; i < m_world.invites.Count; i++)
        {
            bool flag = false;
            for (int j = 0; j < temp.Count; j++)
            {
                if (m_world.invites[i].guid == temp[j].guid)
                {
                    if (m_world.invites[i].invite_id > temp[j].invite_id)
                    {
                        temp[j] = m_world.invites[i];
                        flag = true;
                        break;
                    }
                }
            }
            if (!flag)
            {
                temp.Add(m_world.invites[i]);
            }
        }
        m_world.invites.Clear();
        for (int i = 0; i < temp.Count; i++)
        {
            m_world.invites.Add(temp[i]);

        }
        for (int i = 0; i < temp.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/bingyuan_duiwu_sub");
            GameObject icon = icon_manager._instance.create_player_icon(temp[i].id, temp[i].achieve, temp[i].vip, temp[i].nalflag);
            icon.transform.parent = obj.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            obj.transform.parent = m_duiwu_scro.transform;
            obj.transform.localScale = Vector3.one;
            s_t_bingyuan_chenhao _chenhao = game_data._instance.get_t_bingyuan_chenghao(temp[i].chenghao);
            if (_chenhao != null)
            {
                obj.transform.Find("chenhao").GetComponent<UILabel>().text = game_data._instance.get_t_chenhao(_chenhao.chenhaoid).name;
            }
            else
            {
                _chenhao = game_data._instance.get_t_bingyuan_chenghao(7);
                obj.transform.Find("chenhao").GetComponent<UILabel>().text = game_data._instance.get_t_chenhao(_chenhao.chenhaoid).name; ;
            }
            obj.transform.Find("name").GetComponent<UILabel>().text = temp[i].name;
            if (temp[i].guild != "")
            {
                obj.transform.Find("juntuan").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_gui.cs_1095_86") + temp[i].guild;//军团 
            }
            else
            {
                obj.transform.Find("juntuan").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_gui.cs_1100_86");//军团 暂无
            }
            obj.transform.Find("level").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_gui.cs_1105_80") + temp[i].level;//等级 Lv 
            obj.transform.Find("bf").GetComponent<UILabel>().text = sys._instance.value_to_wan(temp[i].bf);
            obj.name = temp[i].invite_id + "";
            obj.transform.localPosition = new Vector3(0, 107 - i * 150, 0);

            if (type == 1)
            {
                UIEventListener.Get(obj.transform.Find("yaoqing").gameObject).onClick = click_yaoqing;
                obj.transform.Find("yaoqing").gameObject.SetActive(true);
                obj.transform.Find("agree").gameObject.SetActive(false);
                obj.transform.Find("jujue").gameObject.SetActive(false);
            }
            else
            {

                obj.transform.Find("yaoqing").gameObject.SetActive(false);
                obj.transform.Find("agree").gameObject.SetActive(true);
                obj.transform.Find("jujue").gameObject.SetActive(true);
                UIEventListener.Get(obj.transform.Find("agree").gameObject).onClick = click_yaoqing;
                UIEventListener.Get(obj.transform.Find("jujue").gameObject).onClick = click_yaoqing;
            }
        }
        if (m_world.invites.Count == 0)
        {
            m_duiwu_error.SetActive(true);
            m_duiwu_error1.SetActive(false);
        }
        else
        {
            m_duiwu_error.SetActive(false);
            m_duiwu_error1.SetActive(true);
        }
        if (m_world.invites.Count > 0)
        {
            m_button_zudui.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_button_zudui.transform.Find("effect").gameObject.SetActive(false);
        }
    }

    bool is_induiwu(ulong guid)
    {
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == guid)
            {
                return true;
            }
        }
        return false;
    }

    void reset_yaoqing(int type)
    {
        m_yaoqing_panel.SetActive(true);
        if (m_yaoqing_scro.GetComponent<SpringPanel>() != null)
        {
            m_yaoqing_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_yaoqing_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_yaoqing_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_yaoqing_scro);

        for (int i = 0; i < m_view.guid.Count; i++)
        {

            GameObject obj = game_data._instance.ins_object_res("ui/bingyuan_duiwu_sub");
            GameObject icon = icon_manager._instance.create_player_icon(m_view.id[i], m_view.achieve[i], m_view.vip[i], m_view.nalflag[i]);
            icon.transform.parent = obj.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            obj.transform.parent = m_yaoqing_scro.transform;
            obj.transform.localScale = Vector3.one;
            s_t_bingyuan_chenhao _chenhao = game_data._instance.get_t_bingyuan_chenghao(m_view.chenghao[i]);

            if (_chenhao != null)
            {
                obj.transform.Find("chenhao").GetComponent<UILabel>().text = game_data._instance.get_t_chenhao(_chenhao.chenhaoid).name;
            }
            else
            {
                _chenhao = game_data._instance.get_t_bingyuan_chenghao(7);
                obj.transform.Find("chenhao").GetComponent<UILabel>().text = game_data._instance.get_t_chenhao(_chenhao.chenhaoid).name;
            }
            obj.transform.Find("name").GetComponent<UILabel>().text = m_view.name[i];
            if (m_view.guild[i] != "")
            {
                obj.transform.Find("juntuan").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_gui.cs_1095_86") + m_view.guild[i];//军团 
            }
            else
            {
                obj.transform.Find("juntuan").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_gui.cs_1095_86") + game_data._instance.get_t_language("bingyuan_gui.cs_1203_90");//军团 //暂无
            }


            obj.transform.Find("level").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_gui.cs_1105_80") + m_view.level[i];//等级 Lv 
            obj.transform.Find("bf").GetComponent<UILabel>().text = sys._instance.value_to_wan(m_view.bf[i]);
            obj.name = i + "";
            obj.transform.localPosition = new Vector3(0, 156 - i * 150, 0);
            UIEventListener.Get(obj.transform.Find("yaoqing").gameObject).onClick = click_yaoqing;
            if (type == 1)
            {
                obj.transform.Find("yaoqing").gameObject.SetActive(true);
                obj.transform.Find("agree").gameObject.SetActive(false);
                obj.transform.Find("jujue").gameObject.SetActive(false);
            }
            else
            {
                obj.transform.Find("yaoqing").gameObject.SetActive(false);
                obj.transform.Find("agree").gameObject.SetActive(true);
                obj.transform.Find("jujue").gameObject.SetActive(true);
            }
            if (get_yaoqing_state(m_view.guid[i]))
            {
                obj.transform.Find("yaoqing").GetComponent<UISprite>().set_enable(false);
            }
            else
            {
                obj.transform.Find("yaoqing").GetComponent<UISprite>().set_enable(true);
            }
        }
        if (m_view.guid.Count == 0)
        {
            m_yaoqing_error.SetActive(true);
        }
        else
        {
            m_yaoqing_error.SetActive(false);
        }
    }

    bool get_yaoqing_state(ulong guid)
    {
        int index = -1;
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (sys._instance.m_self.m_guid == m_team.players[i].guid)
            {
                index = i;
            }
        }
        if (m_team.players[index].invites.Contains(guid))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void click_yaoqing(GameObject obj)
    {
        if (obj.name == "yaoqing")
        {

            int index = int.Parse(obj.transform.parent.name);
            if (is_induiwu(m_view.guid[index]))
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language("bingyuan_gui.cs_1276_71"));//该好友已在队伍中
                return;
            }
            protocol.game.cmsg_team_invite_friend _fiend = new protocol.game.cmsg_team_invite_friend();
            _fiend.friends = m_view.guid[index];
            _index = index;
            net_http._instance.send_msg<protocol.game.cmsg_team_invite_friend>(opclient_t.CMSG_TEAM_INVITE, _fiend);
        }
        else if (obj.name == "agree")
        {
            ulong inviate = ulong.Parse(obj.transform.parent.name);
            protocol.team.cmsg_invite_agree _agree = new protocol.team.cmsg_invite_agree();
            _agree.agree = true;
            _agree.invite_id = (ulong)inviate;
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_invite_agree>(opclient_t.CMSG_SOCIAL_INVITE, _agree);
        }
        else if (obj.name == "jujue")
        {
            ulong inviate = ulong.Parse(obj.transform.parent.name);
            protocol.team.cmsg_invite_agree _agree = new protocol.team.cmsg_invite_agree();
            _agree.agree = false;
            _agree.invite_id = (ulong)inviate;
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_invite_agree>(opclient_t.CMSG_SOCIAL_INVITE, _agree);
        }
    }

    bool is_leave_duiwu()
    {
        bool _b = true;
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == sys._instance.m_self.m_guid)
            {
                _b = false;
            }
        }
        return _b;
    }

    void click_add(GameObject obj)
    {
        m_can_move = false;
        int index = int.Parse(obj.transform.parent.name);
        protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_TEAM_INVITE_LOOK, _msg);
    }

    void refresh_ui_obj()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject icon = m_uis[i];
            if (m_team.players[i].guid != 0)
            {
                icon.transform.Find("main").gameObject.SetActive(true);
                icon.transform.Find("main/name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_team.players[i].achieve) + m_team.players[i].name;
                icon.transform.Find("main/attack").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_fight.cs_43_118") + sys._instance.value_to_wan(m_team.players[i].bf);//战力 
                icon.transform.Find("add").gameObject.SetActive(false);
                icon.transform.Find("kuang").gameObject.SetActive(false);
                icon.transform.Find("main").localPosition = new Vector3(0, 115, 0);

                if (platform_config_common.m_nationality > 0)
                {
                    if (m_team.players[i].nalflag != 0)
                    {

                        icon.transform.Find("main/gq").GetComponent<UISprite>().spriteName = sys._instance.returnfirstSpritename(m_team.players[i].nalflag);
                        icon.transform.Find("main/gq").gameObject.SetActive(true);
                    }
                    else
                    {
                        icon.transform.Find("main/gq").gameObject.SetActive(false);
                    }
                }
                else
                {
                    icon.transform.Find("main/gq").gameObject.SetActive(false);
                }

                s_t_bingyuan_chenhao _chenghao = game_data._instance.get_t_bingyuan_chenghao(m_team.players[i].chenhao);
                if (_chenghao != null)
                {
                    icon.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                        game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
                }
                else
                {
                    _chenghao = game_data._instance.get_t_bingyuan_chenghao(7);
                    icon.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                        game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
                }
                icon.transform.Find("leader").gameObject.SetActive(false);

                if (m_team.players[i].prepare)
                {
                    icon.transform.Find("prepare").gameObject.SetActive(true);
                }
                else
                {
                    icon.transform.Find("prepare").gameObject.SetActive(false);
                }

                if (m_team.players[i].leader)
                {
                    icon.transform.Find("leader").gameObject.SetActive(true);
                    icon.transform.Find("prepare").gameObject.SetActive(false);
                }
            }
            else
            {
                icon.transform.Find("main").gameObject.SetActive(false);
                icon.transform.Find("add").gameObject.SetActive(true);
                icon.transform.Find("leader").gameObject.SetActive(false);
                icon.transform.Find("prepare").gameObject.SetActive(false);
                icon.transform.Find("kuang").gameObject.SetActive(true);
                icon.transform.Find("main").localPosition = new Vector3(0, 115, 0);
            }
        }
        for (int i = 0; i < 5; i++)
        {
            if (sys._instance.m_self.m_guid == m_team.players[i].guid)
            {
                if (m_team.players[i].leader)
                {
                    m_b_fight.SetActive(true);

                    m_b_prepare_cancel.SetActive(false);
                    m_b_prepare.SetActive(false);
                }
                else
                {
                    if (m_team.players[i].prepare)
                    {
                        m_b_prepare_cancel.SetActive(true);
                        m_b_prepare.SetActive(false);
                    }
                    else
                    {
                        m_b_prepare_cancel.SetActive(false);
                        m_b_prepare.SetActive(true);
                    }
                    m_b_fight.SetActive(false);
                    m_l_kaiqi.SetActive(false);
                }
            }
        }
    }
    void leave_reset()
    {
        m_button_shop.SetActive(false);
        m_start_panel.SetActive(false);
        if (!m_win_result.activeSelf && !bingyuan_battle.gameObject.activeSelf && !m_info_panel.activeSelf)
        {
            m_pipei_panel.SetActive(true);
            m_info.SetActive(true);
        }

        m_jifen.text = m_world.point + "";
        if (is_leader())
        {
            m_move_desc.SetActive(true);
        }
        else
        {
            m_move_desc.SetActive(false);
        }
        for (int i = 0; i < 5; i++)
        {
            if (sys._instance.m_self.m_guid == m_team.players[i].guid)
            {
                if (m_team.players[i].leader)
                {
                    m_b_fight.SetActive(true);
                    m_b_prepare_cancel.SetActive(false);
                    m_b_prepare.SetActive(false);
                }
                else
                {
                    if (m_team.players[i].prepare)
                    {
                        m_b_prepare_cancel.SetActive(true);
                        m_b_prepare.SetActive(false);
                    }
                    else
                    {
                        m_b_prepare_cancel.SetActive(false);
                        m_b_prepare.SetActive(true);
                    }

                    m_b_fight.SetActive(false);
                    m_l_kaiqi.SetActive(false);
                }
            }
        }
        if (!is_leader())
        {
            m_toggle_open.gameObject.SetActive(false);
            m_button_cuicu.SetActive(true);
            m_button_yaoqing.SetActive(false);
        }
        else
        {
            m_toggle_open.gameObject.SetActive(true);
            m_button_cuicu.SetActive(true);
            m_button_yaoqing.SetActive(true);
        }
        refresh_ui_obj();
        m_jiachneg.gameObject.SetActive(true);
        m_jiachneg.text = string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_296_40"), m_reward_jiachneg);//组队奖励加成: {0}%
        set_toggle();
        m_reward_num.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
        m_reward_num1.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
    }

    void reset_pipei(bool create = false)
    {
        m_can_move = true;
        m_button_shop.SetActive(false);
        bingyuan_scence._instance.gameObj = null;
        m_start_panel.SetActive(false);
        if (!m_win_result.activeSelf && !bingyuan_battle.gameObject.activeSelf && !m_info_panel.activeSelf)
        {
            m_pipei_panel.SetActive(true);
            m_info.SetActive(true);
        }

        m_jifen.text = m_world.point + "";
        if (is_leader())
        {
            m_move_desc.SetActive(true);
        }
        else
        {
            m_move_desc.SetActive(false);
        }
        if (create)
        {
            sys._instance.remove_child(m_teams.gameObject);
            m_uis.Clear();
            for (int i = 0; i < 5; i++)
            {
                GameObject icon = game_data._instance.ins_object_res("ui/bingyuan_item");
                icon.transform.parent = m_teams.transform;
                icon.transform.localPosition = new Vector3(188 * i, 0, 0);
                icon.transform.localScale = Vector3.one;
                icon.name = i + "";
                UIEventListener.Get(icon).onClick = click;
                UIEventListener.Get(icon.transform.Find("add").gameObject).onClick = click_add;
                m_uis.Add(icon);
            }
        }
        refresh_ui_obj();
        for (int i = 0; i < 5; i++)
        {
            if (sys._instance.m_self.m_guid == m_team.players[i].guid)
            {
                if (m_team.players[i].leader)
                {
                    m_b_fight.SetActive(true);

                    m_b_prepare_cancel.SetActive(false);
                    m_b_prepare.SetActive(false);
                }
                else
                {
                    if (m_team.players[i].prepare)
                    {
                        m_b_prepare_cancel.SetActive(true);
                        m_b_prepare.SetActive(false);
                    }
                    else
                    {
                        m_b_prepare_cancel.SetActive(false);
                        m_b_prepare.SetActive(true);
                    }

                    m_b_fight.SetActive(false);
                    m_l_kaiqi.SetActive(false);
                }
            }
        }
        if (!is_leader())
        {
            m_toggle_open.gameObject.SetActive(false);
            m_button_cuicu.SetActive(true);
            m_button_yaoqing.SetActive(false);
        }
        else
        {
            m_toggle_open.gameObject.SetActive(true);
            m_button_cuicu.SetActive(true);
            m_button_yaoqing.SetActive(true);
        }
        m_jiachneg.gameObject.SetActive(true);
        m_jiachneg.text = string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_296_40"), m_reward_jiachneg);//组队奖励加成: {0}%
        set_toggle();
        m_reward_num.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
        m_reward_num1.text = game_data._instance.get_t_language("bingyuan_gui.cs_141_28") + sys._instance.m_self.m_t_player.by_reward_num;//剩余奖励次数：
        s_message _mes = new s_message();
        _mes.m_type = "bingyuan_pipei";
        _mes.m_bools.Add(is_leader());
        _mes.m_bools.Add(create);
        cmessage_center._instance.add_message(_mes);
    }

    IEnumerator kaiqi_time()
    {
        while (true)
        {
            m_l_kaiqi.GetComponent<UILabel>().text
            = string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_1587_28"), m_fight_time);//请队长在{0}秒内开启战斗

            if (m_fight_time <= 0)
            {
                StopAllCoroutines();
            }
            m_fight_time--;
            if (m_fight_time < 0)
            {
                m_fight_time = 0;
            }
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator time()
    {
        int time = 0;
        while (true)
        {
            m_wait_pipei.transform.Find("time").GetComponent<UILabel>().text
            = string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_1609_28"), time);//已用时：[00ff00]{0}秒
            time++;
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator time1()
    {
        int time = 0;
        string s = "";
        string s1 = "";
        for (int i = 0; i < m_team.players.Count; i++)
        {
            s += game_data._instance.get_name_color(m_team.players[i].achieve) + m_team.players[i].name + "[-]\n";
            s1 += m_team.players[i].reward_num + "\n";
        }
        m_wait_zidong_pipei.transform.Find("back/desc").GetComponent<UILabel>().text = s;
        m_wait_zidong_pipei.transform.Find("back/desc1").GetComponent<UILabel>().text = s1;

        while (true)
        {
            m_wait_zidong_pipei.transform.Find("back/time").GetComponent<UILabel>().text
            = string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_1609_28"), time);//已用时：[00ff00]{0}秒
            time++;
            yield return new WaitForSeconds(1);
        }
    }

    void prepare()
    {
        net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_PREPARE_TEAM);
    }

    void fight()
    {
        net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_FIGHT_TEAM);
    }

    public void click(GameObject obj)
    {
        m_can_move = false;
        if (obj.name == "close")
        {
            m_can_move = true;
            if (m_tj_panel.activeSelf)
            {
                m_tj_panel.transform.Find("frame_big").GetComponent<frame>().hide();
                return;
            }
            if (m_bingyuan_shop.activeSelf)
            {
                m_bingyuan_shop.GetComponent<bingyuan_shop>().hide();
                sys._instance.message_clear();
                return;
            }
            if (m_duiwu_panel.activeSelf)
            {
                m_duiwu_panel.transform.Find("frame_big").GetComponent<frame>().hide();
                return;
            }
            if (m_yaoqing_panel.activeSelf)
            {
                m_yaoqing_panel.transform.Find("frame_big").GetComponent<frame>().hide();
                return;
            }
            if (m_pipei_panel.activeSelf)
            {
                net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_LEAVE_TEAM);
                s_message _mes = new s_message();
                _mes.m_type = "remove_unit";
                cmessage_center._instance.add_message(_mes);
                reset();
                return;
            }
            s_message _message = new s_message();
            _message.m_type = "show_huo_dong";
            _message.m_ints.Add(11);
            _message.m_bools.Add(m_flag);
            m_flag = false;
            cmessage_center._instance.add_message(_message);
            sys._instance.m_game_state = "hall";
            net_tcp_bingyua._instance.disconnect();
            print(game_data._instance.get_t_language("bingyuan_gui.cs_123_14"));//主动断开连接
            sys._instance.load_scene(sys._instance.m_hall_name);
            Destroy(this.gameObject);
        }
        else if (obj.name == "cuicuzhunbei")
        {
            m_can_move = true;
            m_chat.SetActive(!m_chat.activeSelf);
        }
        else if (obj.name == "close_zidong")
        {
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_STOP_FIGHT_TEAM);
        }
        else if (obj.name == "dis_connect")
        {
            net_tcp_bingyua._instance.disconnect();
            print(game_data._instance.get_t_language("bingyuan_gui.cs_123_14"));//主动断开连接
        }
        else if (obj.name == "yijianyaoqing")
        {
            m_can_move = true;
            if (timer.now() >= m_time)
            {
                net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_INVITE_ALL);
            }
            else
            {
                ulong time = m_time - timer.now();
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_1728_85"), time / 1000));//使用一键邀请太过频繁，可以在{0}秒后再次使用
            }
        }
        else if (obj.name == "toggle_red")
        {
            m_can_move = true;
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_OPEN_TEAM);
        }
        else if (obj.name == "join_team")
        {
            if (!m_join_server)
            {

            }
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_MATCH_TEAM);
        }
        else if (obj.name == "create_team")
        {
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_CREATE_TEAM);
            _index = -1;
        }
        else if (obj.name == "cancel_pipei")
        {
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_END_MATCH_TEAM);
        }
        else if (obj.name == "dui_xing")
        {
            s_message _message = new s_message();
            _message.m_type = "show_duixing_gui";
            _message.m_bools.Add(true);
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.name == "chakan")
        {
            bingyuan_scence._instance.gameObj = null;
            protocol.team.cmsg_player_look _msg = new protocol.team.cmsg_player_look();
            _msg.guid = m_team.players[_index].guid;
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_player_look>(opclient_t.CMSG_VIEW_TEAM_MEMBER, _msg);
        }
        else if (obj.name == "rank")
        {
            m_rank_panel.SetActive(true);
        }
        else if (obj.name == "qingchu")
        {
            bingyuan_scence._instance.gameObj = null;
            s_message _mes = new s_message();
            _mes.m_type = "kick_member";
            root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language("arena.cs_104_45"), string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_1781_74"), game_data._instance.get_name_color(m_team.players[_index].achieve), m_team.players[_index].name), _mes);//提示//是否要踢出{0}{1}
        }
        else if (obj.name == "prepare")
        {
            m_can_move = true;
            if (sys._instance.m_self.m_t_player.by_reward_num > 0)
            {
                net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_PREPARE_TEAM);
            }
            else
            {
                s_message _mes = new s_message();
                _mes.m_type = "bingyuan_repare";
                m_tip.m_mes = _mes;
                m_tip.reset(2);
            }
        }
        else if (obj.name == "prepare_cancel")
        {
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_PREPARE_TEAM);
        }
        else if (obj.name == "zuidui")
        {
            reset_duiwu(2);
            m_duiwu_panel.SetActive(true);
        }
        else if (obj.name == "shop")
        {
            m_bingyuan_shop.SetActive(true);
            m_bingyuan_shop.GetComponent<bingyuan_shop>().reset();
        }
        else if (obj.name == "fight")
        {
            int temp = can_fight();
            if (temp == 1)
            {
                m_can_move = true;
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language("bingyuan_gui.cs_1820_71"));//人数不足
                return;
            }
            else if (temp == 2)
            {
                m_can_move = true;
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language("bingyuan_gui.cs_1827_71"));//有队员未准备
                return;
            }
            if (sys._instance.m_self.m_t_player.by_reward_num <= 0)
            {
                s_message _mes = new s_message();
                _mes.m_type = "bingyuan_fight";
                m_tip.m_mes = _mes;
                m_tip.reset(1);
                return;
            }
            if (is_leader() && temp == 3)
            {
                fight();
            }
        }
        else if (obj.name == "result_click")
        {
            if (m_result)
            {
                result();
            }
        }
        else if (obj.name == "close_qingchu")
        {
            m_op.transform.Find("frame_big").GetComponent<frame>().hide();
            m_can_move = true;
        }
        else if (obj.name == "tj")
        {
            m_tj_panel.SetActive(true);
        }
        else if (obj.name == "buy")
        {
            s_t_price t_price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.by_reward_buy + 1);

            if (sys._instance.m_self.m_t_player.by_reward_buy >= 9)
            {
                m_can_move = true;
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bingyuan_gui.cs_1872_71"));//，提升vip等级可增加每日购买次数");//今日购买次数不足
                return;
            }
            s_message _mes = new s_message();
            _mes.m_type = "buy_bingyuan_reward_num";
            root_gui._instance.show_cishu_dialog_box(sys._instance.m_self.m_t_player.by_reward_num, 9,
                sys._instance.m_self.m_t_player.by_reward_buy, 3, _mes);
        }
        else
        {
            try
            {
                _index = int.Parse(obj.name);
                if (is_leader())
                {
                    if (m_team.players[_index].guid == sys._instance.m_self.m_guid)
                    {
                        m_can_move = true;
                        return;
                    }
                    if (m_team.players[_index].guid == 0)
                    {
                        m_can_move = true;
                        return;
                    }
                    m_op.SetActive(true);
                    return;
                }
                else
                {
                    if (m_team.players[_index].guid == sys._instance.m_self.m_guid)
                    {
                        return;
                    }
                    if (m_team.players[_index].guid == 0)
                    {
                        return;
                    }
                    protocol.team.cmsg_player_look _msg = new protocol.team.cmsg_player_look();
                    _msg.guid = m_team.players[_index].guid;
                    net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_player_look>(opclient_t.CMSG_VIEW_TEAM_MEMBER, _msg);
                    return;
                }
            }
            catch
            {
                print("error");
            }
        }
    }

    int can_fight()
    {
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].guid == 0)
            {
                return 1;//玩家不足
            }
            if (!m_team.players[i].prepare && !m_team.players[i].leader)
            {
                return 2;//没有准备
            }
        }
        return 3;//可以战斗
    }

    bool is_leader()
    {
        for (int i = 0; i < m_team.players.Count; i++)
        {
            if (m_team.players[i].leader && m_team.players[i].guid == sys._instance.m_self.m_guid)
            {
                return true;
            }
        }
        return false;
    }

    public void battle_end()
    {
        s_message _ms = new s_message();
        _ms.m_type = "hide_common_player_panel";
        cmessage_center._instance.add_message(_ms);

        _ms = new s_message();
        _ms.m_type = "hide_player_info_panel";
        cmessage_center._instance.add_message(_ms);
        m_info_panel.SetActive(true);
        bingyuan_battle.gameObject.SetActive(false);
        if (m_fight.win == m_team_id)
        {
            m_wofang.transform.Find("success").GetComponent<UISprite>().spriteName = "bj_sl";
            m_difang.transform.Find("success").GetComponent<UISprite>().spriteName = "bj_sb";
        }
        else
        {
            m_wofang.transform.Find("success").GetComponent<UISprite>().spriteName = "bj_sb";
            m_difang.transform.Find("success").GetComponent<UISprite>().spriteName = "bj_sl";
        }
        m_result = false;
        float speed = 0.3f;
        float dela = 0.1f;
        for (int i = 0; i < bingyuan_battle._instance.m_team.Count; i++)
        {
            s_t_bingyuan_chenhao _b = game_data._instance.get_t_bingyuan_chenghao(bingyuan_battle._instance.m_team[i].chenhao);
            s_t_chenghao _chenhao = null;
            if (_b != null)
            {
                _chenhao = game_data._instance.get_t_chenhao(_b.chenhaoid);
            }
            else
            {
                _chenhao = game_data._instance.get_t_chenhao(game_data._instance.get_t_bingyuan_chenghao(7).chenhaoid);
            }

            m_end_names[i].text = game_data._instance.get_name_color(bingyuan_battle._instance.m_team[i].achieve) +
                bingyuan_battle._instance.m_team[i].name + "";
            m_end_chenhaos[i].text = _chenhao.name + "";
            if (bingyuan_battle._instance.m_team[i].kill > 0)
            {
                m_end_kills[i].text = "[00ff00]" + bingyuan_battle._instance.m_team[i].kill + "[-]";
            }
            else
            {
                m_end_kills[i].text = "[ff0000]" + bingyuan_battle._instance.m_team[i].kill + "[-]";
            }
            sys._instance.add_pos_anim(m_end_names[i].gameObject, speed, new Vector3(-1000, 0, 0), i * dela);
            sys._instance.add_pos_anim(m_end_chenhaos[i].gameObject, speed, new Vector3(-1000, 0, 0), i * dela);
            sys._instance.add_pos_anim(m_end_kills[i].gameObject, speed, new Vector3(-1000, 0, 0), i * dela);
        }

        for (int i = 0; i < m_fight.oteam.players.Count; i++)
        {
            s_t_bingyuan_chenhao _b = game_data._instance.get_t_bingyuan_chenghao(m_fight.oteam.players[i].chenhao);
            s_t_chenghao _chenhao = null;
            if (_b != null)
            {
                _chenhao = game_data._instance.get_t_chenhao(_b.chenhaoid);
            }
            else
            {
                _chenhao = game_data._instance.get_t_chenhao(game_data._instance.get_t_bingyuan_chenghao(7).chenhaoid);
            }

            m_end_names[i + 5].text = game_data._instance.get_name_color(m_fight.oteam.players[i].achieve) +
                m_fight.oteam.players[i].name + "";
            m_end_chenhaos[i + 5].text = _chenhao.name + "";
            if (m_fight.oteam.players[i].kill > 0)
            {
                m_end_kills[i + 5].text = "[00ff00]" + m_fight.oteam.players[i].kill + "[-]";
            }
            else
            {
                m_end_kills[i + 5].text = "[ff0000]" + m_fight.oteam.players[i].kill + "[-]";
            }
            sys._instance.add_pos_anim(m_end_names[i + 5].gameObject, speed, new Vector3(-1000, 0, 0), (i + 5) * dela);
            sys._instance.add_pos_anim(m_end_chenhaos[i + 5].gameObject, speed, new Vector3(-1000, 0, 0), (i + 5) * dela);
            sys._instance.add_pos_anim(m_end_kills[i + 5].gameObject, speed, new Vector3(-1000, 0, 0), (i + 5) * dela);
        }

        sys._instance.add_scale_anim(m_wofang.transform.Find("success").gameObject, speed, 3, 1, 10 * dela);
        sys._instance.add_scale_anim(m_difang.transform.Find("success").gameObject, speed, 3, 1, 10 * dela);
        sys._instance.add_pos_anim(m_wofang.transform.Find("success").gameObject, speed, new Vector3(0, 1000, 0), 10 * dela);
        sys._instance.add_pos_anim(m_difang.transform.Find("success").gameObject, speed, new Vector3(0, 1000, 0), 10 * dela).AddOnFinished(set_result);
    }

    void set_result()
    {
        m_result = true;
    }

    void result()
    {
        Time.timeScale = 1;
        m_pipei_panel.SetActive(false);
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        bool win = false;
        m_info_panel.SetActive(false);
        bingyuan_battle.gameObject.SetActive(false);
        m_info_panel.SetActive(false);
        m_pipei_panel.SetActive(false);

        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add((string)"ts_game_bingyuan");

        _result.add_tip("[ffb391]" + string.Format(game_data._instance.get_t_language("bingyuan_gui.cs_2072_51"), sys._instance.m_self.m_t_player.by_reward_num));//[ffb391]//今日剩余奖励次数： [-]{0}次
        sys._instance.m_self.add_reward(1, 24, m_bingyuan_fight_end.bingjing, 0, false, game_data._instance.get_t_language("bingyuan_gui.cs_2073_87"));//冰原战斗获得
        sys._instance.m_self.m_t_player.by_point += m_bingyuan_fight_end.point;
        if (m_fight.win == m_team_id)
        {
            win = true;
        }
        int bingjin = 0;
        int point = 0;
        if (m_bingyuan_fight_end.point != 0)
        {
            if (win)
            {
                bingjin = game_data._instance.get_t_exp(sys._instance.m_self.m_t_player.level).suc_bingjin;
                if ((timer.dtnow().Hour >= 12 && timer.dtnow().Hour < 14) || (timer.dtnow().Hour >= 18 && timer.dtnow().Hour < 22))
                {
                    point = 200;
                }
                else
                {
                    point = 100;
                }
            }
            else
            {
                bingjin = game_data._instance.get_t_exp(sys._instance.m_self.m_t_player.level).fail_bingjin;
                if ((timer.dtnow().Hour >= 12 && timer.dtnow().Hour < 14) || (timer.dtnow().Hour >= 18 && timer.dtnow().Hour < 22))
                {
                    point = 100;
                }
                else
                {
                    point = 50;
                }
            }
        }
        _result.add_reward(1, 24, bingjin, m_reward_jiacheng_result);
        _result.add_reward(1, 25, point, m_reward_jiacheng_result);
        _result.set_win(win);

        if (win == false)
        {
            m_win_result.SetActive(true);
            return;
        }
        _result.set_win(win);
        m_win_result.SetActive(true);
    }

    public void end()
    {
        m_info_panel.SetActive(false);
        m_win_result.SetActive(false);
        if (m_team == null)
        {
            reset();
            return;
        }
        if (is_leader())
        {
            net_tcp_bingyua._instance.send_msg_null(opclient_t.CMSG_LEADER_PREPARE);
        }
        m_pipei_panel.SetActive(true);
        if (!is_leave_duiwu())
        {
            reset_pipei();
        }
        else
        {
            reset();
        }
    }

    public static bool is_effect()
    {
        if (sys._instance.m_self.is_bingyuan_effect > 0 || bingyuan_shop.effect())
        {
            return true;
        }
        return false;
    }

    public void dacheng()
    {
        if (m_ui_jibai.GetComponent<UILabel>().text != "")
        {
            m_ui_jibai.gameObject.SetActive(true);
            TweenScale _scale = sys._instance.add_scale_anim(m_ui_jibai.gameObject, 0.2f, 0.5f, 1.2f, 0);
            EventDelegate.Add(_scale.onFinished, delegate ()
            {
                hide();
            }, true);
        }
    }

    public void hide()
    {
        hide_Label _hide_Label = m_ui_jibai.GetComponent<hide_Label>();

        if (_hide_Label == null)
        {
            _hide_Label = m_ui_jibai.gameObject.AddComponent<hide_Label>();
        }
        _hide_Label.m_time = 1f;
    }

    public void set_des(string s)
    {
        m_ui_jibai.GetComponent<UILabel>().text = s;
        dacheng();
    }
}
