using System.Collections.Generic;
using UnityEngine;

public class bingyuan_battle : MonoBehaviour, IMessage
{
    public protocol.team.smsg_fight_team m_fight;
    public List<protocol.team.team_player> m_team;
    public Transform[] m_icons;
    public UILabel[] m_names;
    public UILabel[] m_bfs;
    public UILabel[] m_chenhaos;
    public UILabel m_huihe;
    public UISlider[] m_hps;
    public GameObject m_left_view;
    public GameObject m_right_view;
    public List<GameObject> m_left;
    public List<GameObject> m_right;
    public int huihe = 1;
    public float m_tiem = 0;
    public List<skill_ex> m_skills;
    public float m_wait = 0;
    public int m_skill_index = 0;
    public int m_huihe_num = 0;
    public int m_hp_num = 0;
    private int m_left_num;
    private int m_right_num;
    public bool m_battle_fight = false;
    public bool m_end_huihe = false;
    public string m_des_string;
    public int m_des_index;
    public ulong m_team_id;
    public UISprite m_vs;
    public static bingyuan_battle _instance;
    public protocol.team.team_player current_left_player;
    public protocol.team.team_player current_right_player;

    void Start()
    {
        _instance = this;
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void createitem()
    {
        sys._instance.remove_child(m_left_view);
        sys._instance.remove_child(m_right_view);
        m_left = new List<GameObject>();
        m_right = new List<GameObject>();
        for (int i = 0; i < bingyuan_gui._instance.m_team.players.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/bingyuan_item_fan");

            obj.transform.Find("main/bf").GetComponent<UILabel>().text = sys._instance.value_to_wan(bingyuan_gui._instance.m_team.players[i].bf);
            s_t_bingyuan_chenhao _chnhao = game_data._instance.get_t_bingyuan_chenghao(bingyuan_gui._instance.m_team.players[i].chenhao);
            if (_chnhao != null)
            {
                obj.transform.Find("main/chenhao").GetComponent<UILabel>().text = game_data._instance.get_t_chenhao(_chnhao.chenhaoid).name;
            }
            else
            {
                obj.transform.Find("main/chenhao").GetComponent<UILabel>().text = "";
            }
            obj.transform.Find("main/level").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("bingyuan_battle.cs_93_95"), bingyuan_gui._instance.m_team.players[i].level);//等级： {0}
            obj.transform.Find("name").GetComponent<UILabel>().text =
                game_data._instance.get_name_color(bingyuan_gui._instance.m_team.players[i].achieve) + bingyuan_gui._instance.m_team.players[i].name;

            GameObject icon = icon_manager._instance.create_player_icon(bingyuan_gui._instance.m_team.players[i].id, bingyuan_gui._instance.m_team.players[i].achieve, bingyuan_gui._instance.m_team.players[i].vip, bingyuan_gui._instance.m_team.players[i].nalflag);
            icon.transform.parent = obj.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            obj.transform.parent = m_left_view.transform;
            obj.transform.localPosition = new Vector3(-36, 250 - i * 121, 0);
            obj.transform.localScale = new Vector3(-1, 1, 1);
            obj.transform.Find("jidao").GetComponent<UISprite>().spriteName = "";
            obj.name = i + "";
            UIEventListener.Get(obj).onClick = left_click;
            m_left.Add(obj);
        }
        for (int i = 0; i < m_fight.oteam.players.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/bingyuan_sub");

            obj.transform.Find("main/bf").GetComponent<UILabel>().text = sys._instance.value_to_wan(m_fight.oteam.players[i].bf);
            s_t_bingyuan_chenhao _chnhao = game_data._instance.get_t_bingyuan_chenghao(m_fight.oteam.players[i].chenhao);
            if (_chnhao != null)
            {
                obj.transform.Find("main/chenhao").GetComponent<UILabel>().text = game_data._instance.get_t_chenhao(_chnhao.chenhaoid).name;
            }
            obj.transform.Find("main/level").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("bingyuan_battle.cs_93_95"), m_fight.oteam.players[i].level);//等级： {0}
            obj.transform.Find("name").GetComponent<UILabel>().text =
                game_data._instance.get_name_color(m_fight.oteam.players[i].achieve) + m_fight.oteam.players[i].name;

            GameObject icon = icon_manager._instance.create_player_icon(m_fight.oteam.players[i].id, m_fight.oteam.players[i].achieve, m_fight.oteam.players[i].vip, m_fight.oteam.players[i].nalflag);
            icon.transform.parent = obj.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = new Vector3(1, 1, 1);
            obj.transform.parent = m_right_view.transform;
            obj.transform.localPosition = new Vector3(36, 250 - i * 121, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("jidao").GetComponent<UISprite>().spriteName = "";
            obj.name = i + "";
            UIEventListener.Get(obj).onClick = right_click;
            m_right.Add(obj);
        }
    }

    void left_click(GameObject obj)
    {
        int index = int.Parse(obj.name);
        protocol.team.cmsg_player_look _msg = new protocol.team.cmsg_player_look();
        _msg.guid = m_team[index].guid;
        net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_player_look>(opclient_t.CMSG_VIEW_TEAM_MEMBER, _msg);
    }

    void right_click(GameObject obj)
    {
        int index = int.Parse(obj.name);
        protocol.team.cmsg_player_look _msg = new protocol.team.cmsg_player_look();
        _msg.guid = m_fight.oteam.players[index].guid;
        net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_player_look>(opclient_t.CMSG_VIEW_TEAM_MEMBER, _msg);
    }

    protocol.team.team_player clone(protocol.team.team_player player)
    {
        protocol.team.team_player _player = new protocol.team.team_player();
        _player.achieve = player.achieve;
        _player.bf = player.bf;
        _player.chenhao = player.chenhao;
        _player.dress = player.dress;
        _player.guanghuan = player.guanghuan;
        _player.guid = player.guid;
        _player.id = player.id;
        _player.is_npc = player.is_npc;
        _player.kill = player.kill;
        _player.leader = player.leader;
        _player.level = player.level;
        _player.prepare = player.prepare;
        _player.reward_num = player.reward_num;
        _player.serverid = player.serverid;
        _player.vip = player.vip;
        _player.name = player.name;
        return _player;
    }

    public void reset(protocol.team.smsg_fight_team fight)
    {
        m_fight = fight;
        m_huihe_num = 0;
        m_hp_num = -1;
        m_left_num = 0;
        m_right_num = 0;
        m_team = new List<protocol.team.team_player>();
        for (int i = 0; i < bingyuan_gui._instance.m_team.players.Count; i++)
        {
            protocol.team.team_player _player = new protocol.team.team_player();
            _player = clone(bingyuan_gui._instance.m_team.players[i]);
            m_team.Add(_player);
        }
        for (int i = 0; i < m_fight.oteam.players.Count; i++)
        {
            m_fight.oteam.players[i].kill = 0;
        }
        createitem();
    }

    public void start()
    {
        parse_text();
        m_battle_fight = true;
        s_message _msg = new s_message();
        _msg.m_type = "end_layout";
        _msg.m_ints.Add(0);
        _msg.time = 0.1f;
        cmessage_center._instance.add_message(_msg);
    }


    int get_index(int site, ulong guid)
    {
        if (site == 0)
        {
            for (int i = 0; i < m_team.Count; i++)
            {
                if (guid == m_team[i].guid)
                {
                    return i;
                }
            }
        }
        else if (site == 1)
        {
            for (int i = 0; i < m_fight.oteam.players.Count; i++)
            {
                if (guid == m_fight.oteam.players[i].guid)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    void refresh_player(protocol.team.team_player player_left, protocol.team.team_player player_right)
    {
        List<protocol.team.team_player> m_players = new List<protocol.team.team_player>();
        m_players.Add(player_left);
        m_players.Add(player_right);
        protocol.team.bingyuan_huihe huihe = m_fight.text.huihes[m_huihe_num];
        for (int i = 0; i < m_players.Count; i++)
        {
            sys._instance.remove_child(m_icons[i].gameObject);
            GameObject obj = icon_manager._instance.create_player_icon(m_players[i].id, m_players[i].achieve, m_players[i].vip, m_players[i].nalflag);
            obj.transform.parent = m_icons[i];
            obj.transform.localScale = Vector3.one;
            obj.transform.localPosition = Vector3.zero;
            m_names[i].text = m_players[i].name;
            m_bfs[i].text = sys._instance.value_to_wan(m_players[i].bf);
            s_t_bingyuan_chenhao _chnhao = game_data._instance.get_t_bingyuan_chenghao(m_players[i].chenhao);
            if (_chnhao != null)
            {
                m_chenhaos[i].text = game_data._instance.get_t_chenhao(_chnhao.chenhaoid).name;
            }
            else
            {
                m_chenhaos[i].text = game_data._instance.get_t_chenhao(game_data._instance.get_t_bingyuan_chenghao(7).chenhaoid).name;
            }

        }
        current_left_player = player_left;
        current_right_player = player_right;
        refresh_hp(0);
        bingyuan_fight._instance.vs_show(current_right_player, current_left_player);
        m_huihe.text = game_data._instance.get_t_language("bingyuan_battle.cs_261_23") + "\n" + (m_huihe_num + 1);//回   合
        top_res.add_scale_anim(m_huihe.gameObject);
    }

    void refresh_hp(int type)
    {
        protocol.team.bingyuan_huihe huihe = m_fight.text.huihes[m_huihe_num];
        double hp0 = 0;
        double hp1 = 0;
        if (type == 0)
        {
            hp0 = huihe.hps[get_team_index(0, huihe)];
            hp1 = huihe.hps[get_team_index(1, huihe)];
        }
        else
        {
            if (3 + m_hp_num * 2 < huihe.hps.Count)
            {
                hp0 = huihe.hps[get_team_index(0, huihe) + 2 + m_hp_num * 2];
                hp1 = huihe.hps[get_team_index(1, huihe) + 2 + m_hp_num * 2];
            }
            else
            {
                hp0 = huihe.hps[huihe.hps.Count - 2];
                hp1 = huihe.hps[huihe.hps.Count - 1];
            }
        }

        double m_max_hp_0 = huihe.max_hps[get_team_index(0, huihe)];
        double m_max_hp_1 = huihe.max_hps[get_team_index(1, huihe)];
        if (bingyuan_fight._instance.m_hps[0])
        {
            bingyuan_fight._instance.m_hps[0].transform.Find("hp").GetComponent<UISlider>().value = (float)(hp0 / m_max_hp_0);
        }
        if (bingyuan_fight._instance.m_hps[1])
        {
            bingyuan_fight._instance.m_hps[1].transform.Find("hp").GetComponent<UISlider>().value = (float)(hp1 / m_max_hp_1);
        }
        if (type == 1)
        {
            if (huihe.win == huihe.team_guids[get_team_index(0, huihe)])
            {
                if (bingyuan_fight._instance.m_hps[1])
                {
                    bingyuan_fight._instance.m_hps[1].transform.Find("hp").GetComponent<UISlider>().value = 0;
                }

            }
            else
            {
                if (bingyuan_fight._instance.m_hps[0])
                {
                    bingyuan_fight._instance.m_hps[0].transform.Find("hp").GetComponent<UISlider>().value = 0;
                }
            }
        }
    }

    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "bingyuan_fight_next")
        {
            parse_text();
        }
        else if (mes.m_type == "attack_hp")
        {
            attack_hp();
        }
    }

    void IMessage.net_message(s_net_message mes)
    {

    }

    void parse_text()
    {
        if (m_huihe_num >= m_fight.text.huihes.Count)
        {
            s_message mes = new s_message();
            mes.m_type = "bingyuan_fight_end";
            cmessage_center._instance.add_message(mes);
            end();
            return;
        }
        m_hp_num++;
        protocol.team.bingyuan_huihe huihe = m_fight.text.huihes[m_huihe_num];

        if (m_hp_num == 0)
        {
            protocol.team.team_player player_left = m_team[get_index(0, huihe.team_guids[get_team_index(0, huihe)])];
            s_message mes = new s_message();
            mes.m_type = "bingyuan_fight_unit";
            mes.m_ints.Add(0);
            mes.m_object.Add(player_left);
            cmessage_center._instance.add_message(mes);

            protocol.team.team_player player_right = m_fight.oteam.players[get_index(1, huihe.team_guids[get_team_index(1, huihe)])];
            mes = new s_message();
            mes.m_type = "bingyuan_fight_unit";
            mes.m_ints.Add(1);
            mes.m_object.Add(player_right);
            cmessage_center._instance.add_message(mes);

            refresh_player(player_left, player_right);
            for (int i = 0; i < m_left.Count; i++)
            {
                m_left[i].transform.Find("state").GetComponent<UILabel>().text = "";
            }
            for (int i = 0; i < m_right.Count; i++)
            {
                m_right[i].transform.Find("state").GetComponent<UILabel>().text = "";
            }
            m_left[get_index(0, huihe.team_guids[get_team_index(0, huihe)])].transform.Find("state").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_battle.cs_384_136");//出战中
            m_right[get_index(1, huihe.team_guids[get_team_index(1, huihe)])].transform.Find("state").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_battle.cs_384_136");//出战中
        }
        if (m_hp_num < 3)
        {
            m_hp_num = 2;
            for (int num = 0; num < 3; num++)
            {
                double hp0_0 = huihe.hps[get_team_index(0, huihe) + 2 + num * 2];
                double hp1_0 = huihe.hps[get_team_index(1, huihe) + 2 + num * 2];
                if (hp0_0 == 0 || hp1_0 == 0)
                {
                    m_hp_num = num;
                }
            }
            m_end_huihe = true;
            bingyuan_fight._instance.action();
        }
        else
        {
            m_hp_num = -1;
            m_huihe_num++;
            if (huihe.win == huihe.team_guids[get_team_index(0, huihe)])
            {
                s_message mes = new s_message();
                mes.m_type = "bingyuan_fight_unit_die";
                mes.m_ints.Add(1);
                cmessage_center._instance.add_message(mes);
                int num = ++m_team[get_index(0, huihe.team_guids[get_team_index(0, huihe)])].kill;
                m_des_index = 1;
                m_des_string = "\n";
                if (num == 3)
                {
                    m_des_string += string.Format(game_data._instance.get_t_language("bingyuan_battle.cs_436_51"), game_data._instance.get_name_color(current_left_player.achieve), current_left_player.name);//{0}{1}[-][ffff00]已打满三场
                    m_left[m_left_num].transform.Find("jidao").GetComponent<UISprite>().spriteName = "bj_msc";
                    mes = new s_message();
                    mes.m_type = "bingyuan_fight_unit_die";
                    mes.m_ints.Add(0);
                    cmessage_center._instance.add_message(mes);
                    m_left_num++;

                }
                m_right[m_right_num].transform.Find("jidao").GetComponent<UISprite>().spriteName = "bj_yjd";
                m_right_num++;
            }
            else if (huihe.win == huihe.team_guids[get_team_index(1, huihe)])
            {
                s_message mes = new s_message();
                mes.m_type = "bingyuan_fight_unit_die";
                mes.m_ints.Add(0);

                cmessage_center._instance.add_message(mes);
                m_left[m_left_num].transform.Find("jidao").GetComponent<UISprite>().spriteName = "bj_yjd";
                m_left_num++;

                int num = ++m_fight.oteam.players[get_index(1, huihe.team_guids[get_team_index(1, huihe)])].kill;

                m_des_index = 0;
                m_des_string = "\n";
                if (num == 3)
                {
                    m_des_string += string.Format(game_data._instance.get_t_language("bingyuan_battle.cs_436_51"), game_data._instance.get_name_color(current_right_player.achieve), current_right_player.name);//{0}{1}[-][ffff00]已打满三场
                    m_right[m_right_num].transform.Find("jidao").GetComponent<UISprite>().spriteName = "bj_msc";
                    mes = new s_message();
                    mes.m_type = "bingyuan_fight_unit_die";
                    mes.m_ints.Add(1);
                    cmessage_center._instance.add_message(mes);
                    m_right_num++;
                }
            }
            else
            {
                s_message mes = new s_message();
                mes.m_type = "bingyuan_fight_unit_die";
                mes.m_ints.Add(0);
                cmessage_center._instance.add_message(mes);
                m_left_num++;
                mes = new s_message();
                mes.m_type = "bingyuan_fight_unit_die";
                mes.m_ints.Add(1);
                cmessage_center._instance.add_message(mes);
                m_right_num++;
                m_des_index = 1;
                m_des_string = "";
            }
        }
    }

    public int get_team_index(int x, protocol.team.bingyuan_huihe huihe)
    {
        int index = 0;
        if (x == 0)
        {
            for (int i = 0; i < huihe.team_ids.Count; i++)
            {
                if (m_team_id == huihe.team_ids[i])
                {
                    index = i;
                }
            }
        }
        else
        {
            for (int i = 0; i < huihe.team_ids.Count; i++)
            {
                if (bingyuan_battle._instance.m_fight.oteam.team_id == huihe.team_ids[i])
                {
                    index = i;
                }
            }
        }
        return index;
    }

    public void set_des_t()
    {
        set_des(m_des_index, m_des_string);
    }

    void set_des(int index, string des = "")
    {
        string s = "";
        if (index == 0)
        {
            int kill = bingyuan_battle._instance.current_right_player.kill;
            if (kill == 1)
            {
                s += "[ff0000]" + game_data._instance.get_t_language("bingyuan_battle.cs_529_34") + "[-]\n";//第一滴血
                s += game_data._instance.get_name_color(bingyuan_battle._instance.current_right_player.achieve)
                    + bingyuan_battle._instance.current_right_player.name + "[-]" + game_data._instance.get_t_language("bingyuan_battle.cs_531_84")// 击倒了 
                    + game_data._instance.get_name_color(bingyuan_battle._instance.current_left_player.achieve) +
                          bingyuan_battle._instance.current_left_player.name;
            }
            else if (kill == 2)
            {
                s += "[ff0000]" + game_data._instance.get_t_language("bingyuan_battle.cs_537_34") + "[-]\n";//双杀
                s += game_data._instance.get_name_color(bingyuan_battle._instance.current_right_player.achieve)
                    + bingyuan_battle._instance.current_right_player.name + "[-]" + game_data._instance.get_t_language("bingyuan_battle.cs_531_84")// 击倒了 
                    + game_data._instance.get_name_color(bingyuan_battle._instance.current_left_player.achieve) +
                          bingyuan_battle._instance.current_left_player.name;
            }
            else if (kill == 3)
            {
                s += "[ff0000]" + game_data._instance.get_t_language("bingyuan_battle.cs_545_34") + "[-]\n";//三杀
                s += game_data._instance.get_name_color(bingyuan_battle._instance.current_right_player.achieve)
                    + bingyuan_battle._instance.current_right_player.name + "[-]" + game_data._instance.get_t_language("bingyuan_battle.cs_531_84")// 击倒了 
                    + game_data._instance.get_name_color(bingyuan_battle._instance.current_left_player.achieve) +
                          bingyuan_battle._instance.current_left_player.name;
            }
        }
        else
        {
            int kill = bingyuan_battle._instance.current_left_player.kill;
            if (kill == 1)
            {
                s += "[ff0000]" + game_data._instance.get_t_language("bingyuan_battle.cs_529_34") + "[-]\n";//第一滴血
                s += game_data._instance.get_name_color(bingyuan_battle._instance.current_left_player.achieve)
                    + bingyuan_battle._instance.current_left_player.name + "[-]" + game_data._instance.get_t_language("bingyuan_battle.cs_531_84")// 击倒了 
                    + game_data._instance.get_name_color(bingyuan_battle._instance.current_right_player.achieve) +
                          bingyuan_battle._instance.current_right_player.name;
            }
            else if (kill == 2)
            {
                s += "[ff0000]" + game_data._instance.get_t_language("bingyuan_battle.cs_537_34") + "[-]\n";//双杀
                s += game_data._instance.get_name_color(bingyuan_battle._instance.current_left_player.achieve)
                    + bingyuan_battle._instance.current_left_player.name + "[-]" + game_data._instance.get_t_language("bingyuan_battle.cs_531_84")// 击倒了 
                    + game_data._instance.get_name_color(bingyuan_battle._instance.current_right_player.achieve) +
                          bingyuan_battle._instance.current_right_player.name;
            }
            else if (kill == 3)
            {
                s += "[ff0000]" + game_data._instance.get_t_language("bingyuan_battle.cs_545_34") + "[-]\n";//三杀
                s += game_data._instance.get_name_color(bingyuan_battle._instance.current_left_player.achieve)
                    + bingyuan_battle._instance.current_left_player.name + "[-]" + game_data._instance.get_t_language("bingyuan_battle.cs_531_84")// 击倒了 
                    + game_data._instance.get_name_color(bingyuan_battle._instance.current_right_player.achieve) +
                          bingyuan_battle._instance.current_right_player.name;
            }
        }
        bingyuan_gui._instance.set_des(s + des);
    }

    void attack_hp()
    {
        protocol.team.bingyuan_huihe huihe = m_fight.text.huihes[m_huihe_num];
        double hp0 = 0;
        double hp1 = 0;
        if (3 + m_hp_num * 2 < huihe.hps.Count)
        {

            hp0 = huihe.hps[get_team_index(0, huihe) + 2 + m_hp_num * 2];

            hp1 = huihe.hps[get_team_index(1, huihe) + 2 + m_hp_num * 2];
        }
        else
        {
            hp0 = huihe.hps[huihe.hps.Count - 2];
            hp1 = huihe.hps[huihe.hps.Count - 1];
        }

        double attack_hp_0 = 0;
        double attack_hp_1 = 0;
        double m_max_hp_0 = huihe.max_hps[get_team_index(0, huihe)];
        double m_max_hp_1 = huihe.max_hps[get_team_index(1, huihe)];

        attack_hp_0 = m_max_hp_0 - hp0;
        attack_hp_1 = m_max_hp_1 - hp1;

        s_message _mes = new s_message();
        _mes.m_type = "show_attack_num_bingyuan";
        _mes.m_floats.Add(attack_hp_0);
        _mes.m_floats.Add(attack_hp_1);
        if (hp0 == 0 && hp1 != 0)
        {
            _mes.m_ints.Add(1);
        }
        else if (hp0 != 0 && hp1 == 0)
        {
            _mes.m_ints.Add(0);
        }
        else
        {
            _mes.m_ints.Add(-1);
        }

        cmessage_center._instance.add_message(_mes);
        refresh_hp(1);
        double hp0_0 = huihe.hps[get_team_index(0, huihe) + 2 + m_hp_num * 2];
        double hp1_0 = huihe.hps[get_team_index(1, huihe) + 2 + m_hp_num * 2];
        if (hp0_0 == 0 || hp1_0 == 0)
        {
            m_hp_num = 3;
        }
    }

    void end()
    {
        s_message _mes = new s_message();
        _mes.m_type = "bingyuan_battle_end";
        cmessage_center._instance.add_message(_mes);
        refresh_item();
    }

    public void refresh_left()
    {
        if (m_left_view.transform.parent.localPosition.x == 15)
        {
            for (int i = 0; i < m_left.Count; i++)
            {
                m_left[i].transform.Find("main").gameObject.SetActive(false);
                m_left[i].transform.Find("name").localPosition = new Vector3(-90, -60, 0);
            }
        }
        else
        {
            for (int i = 0; i < m_left.Count; i++)
            {
                m_left[i].transform.Find("main").gameObject.SetActive(true);
                m_left[i].transform.Find("name").localPosition = new Vector3(0, -60, 0);
            }
        }
    }

    public void refresh_right()
    {
        if (m_right_view.transform.parent.localPosition.x == -15)
        {
            for (int i = 0; i < m_left.Count; i++)
            {
                m_right[i].transform.Find("main").gameObject.SetActive(false);
                m_right[i].transform.Find("name").localPosition = new Vector3(-90, -60, 0);
            }
        }
        else
        {
            for (int i = 0; i < m_left.Count; i++)
            {
                m_right[i].transform.Find("main").gameObject.SetActive(true);
                m_right[i].transform.Find("name").localPosition = new Vector3(0, -60, 0);
            }
        }
    }

    void refresh_item()
    {
        if (m_right_view.transform.parent.localPosition.x != -15)
        {
            m_right_view.transform.parent.GetComponent<TweenPosition>().Toggle();
        }
        if (m_left_view.transform.parent.localPosition.x != 15)
        {
            m_left_view.transform.parent.GetComponent<TweenPosition>().Toggle();
        }
    }

    void OnDisable()
    {
        s_message mes = new s_message();
        mes.m_type = "bingyuan_fight_end";
        cmessage_center._instance.add_message(mes);
    }
}
