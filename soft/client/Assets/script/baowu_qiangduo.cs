using System.Collections.Generic;
using UnityEngine;

public class baowu_qiangduo : MonoBehaviour, IMessage
{
    public int m_suipian_id;
    protocol.game.smsg_treasure_rob_view _msg;
    int refresh_number = 0;
    int m_index;
    public GameObject m_view;
    List<qiangduo_sub> m_qiangduo_subs = new List<qiangduo_sub>();
    ulong m_time = 0;
    public UILabel m_time_obj;
    public GameObject m_saodang;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    public void init()
    {
        protocol.game.cmsg_treasure_rob_view _msg = new protocol.game.cmsg_treasure_rob_view();
        _msg.treasure_suipian = m_suipian_id;
        net_http._instance.send_msg<protocol.game.cmsg_treasure_rob_view>(opclient_t.CMSG_TREASURE_VIEW, _msg);
    }

    void click(GameObject obj)
    {
        s_message _mes = new s_message();

        if (obj.name == "close")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            _mes = new s_message();
            _mes.m_type = "show_baowu_hecheng_gui1";
            cmessage_center._instance.add_message(_mes);
        }
        else if (obj.name == "refresh")
        {
            if (m_time < timer.now())
            {
                m_time = timer.now() + 5 * 1000;
                InvokeRepeating("time", 0, 1f);
                refresh_number++;
                if (refresh_number >= (_msg.player_guids.Count / 3))
                {
                    refresh_number = 0;
                }
                reset();
            }
        }
    }

    void reset()
    {
        sys._instance.remove_child(m_view);
        m_qiangduo_subs.Clear();
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        List<int> rl = new List<int>();
        for (int i = refresh_number * 3; i < refresh_number * 3 + 3 && i < _msg.player_guids.Count; i++)
        {
            if (_msg.player_npcs[i])
            {
                rl.Add(i);
            }
            else
            {
                rl.Insert(0, i);
            }
        }

        for (int j = 0; j < rl.Count; ++j)
        {
            int i = rl[j];
            GameObject ts = game_data._instance.ins_object_res("ui/qiangduo_sub");
            qiangduo_sub _qiangduo = ts.GetComponent<qiangduo_sub>();
            if (i == refresh_number * 3)
            {
                _qiangduo.m_button_1.name = "button_qiangduo";
            }
            _qiangduo.m_name = new System.Text.UTF8Encoding(true).GetString(_msg.player_names[i]);
            _qiangduo.m_level = _msg.player_levels[i];
            _qiangduo.m_rate = _msg.player_rates[i];
            _qiangduo.m_bf = _msg.player_bfs[i];
            _qiangduo.m_guid = _msg.player_guids[i];
            _qiangduo.is_npc = _msg.player_npcs[i];
            if ((sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_treasure_qiangduo5 || sys._instance.m_self.m_t_player.vip >= (int)e_open_vip.ev_treasure_qiangduo5)
                && _msg.player_npcs[i])
            {
                _qiangduo.m_qiangduo_5 = true;
            }
            else
            {
                _qiangduo.m_qiangduo_5 = false;
            }
            _qiangduo.m_id = (ulong)_msg.player_templates[i];
            _qiangduo.m_achieve = _msg.player_achieves[i];
            _qiangduo.m_vip = _msg.player_vips[i];
            _qiangduo.m_index = i - refresh_number * 3;
            _qiangduo.gq = _msg.player_nalflags[i];
            _qiangduo.reset();
            m_qiangduo_subs.Add(_qiangduo);
            ts.transform.parent = m_view.transform;
            ts.transform.localPosition = new Vector3(52, 127 - 126 * (i % 3), 0);
            ts.transform.localScale = new Vector3(1, 1, 1);
            sys._instance.add_pos_anim(ts, 0.3f, new Vector3(-300, 0, 0), i % 3 * 0.05f);
        }
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_TREASURE_VIEW)
        {
            _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_rob_view>(message.m_byte);
            refresh_number = 0;
            reset();
        }
        else if (message.m_opcode == opclient_t.CMSG_TREASURE_FIGHT_END)
        {
            if (!_msg.player_npcs[m_index + refresh_number * 3])
            {
                sys._instance.m_self.m_t_player.treasure_protect_next_time = 0;
            }
            s_message _out_msg = new s_message();
            _out_msg.m_type = "treasure_fight";
            protocol.game.smsg_treasure_fight_end m_msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_fight_end>(message.m_byte);

            battle_logic_ex._instance.set_treasure_fight_end(m_msg);
            cmessage_center._instance.add_message(_out_msg);
            sys._instance.load_scene_ex("ts_fight_jjc");
            sys._instance.m_game_state = "buttle";
            s_message _msg1 = new s_message();
            _msg1.m_type = "hide_hecheng";
            cmessage_center._instance.add_message(_msg1);
        }
        else if (message.m_opcode == opclient_t.CMSG_TREASURE_SAODANG)
        {
            if (!_msg.player_npcs[m_index + refresh_number * 3])
            {
                sys._instance.m_self.m_t_player.treasure_protect_next_time = 0;
            }
            protocol.game.smsg_treasure_saodang _msg1 = net_http._instance.parse_packet<protocol.game.smsg_treasure_saodang>(message.m_byte);
            sys._instance.m_self.sub_att(e_player_attr.player_treasure_energy, _msg1.rewards.Count * 2);
            for (int i = 0; i < _msg1.rewards.Count; i++)
            {
                for (int j = 0; j < _msg1.rewards[i].types.Count; j++)
                {
                    sys._instance.m_self.add_reward(_msg1.rewards[i].types[j], _msg1.rewards[i].value1s[j], _msg1.rewards[i].value2s[j], _msg1.rewards[i].value3s[j], false, game_data._instance.get_t_language("baowu_qiangduo.cs_171_171"));//夺宝奇兵扫荡
                }
                if (_msg1.rewards[i].suipian_id != 0)
                {
                    sys._instance.m_self.add_item((uint)_msg1.rewards[i].suipian_id, 1, false, game_data._instance.get_t_language("baowu_qiangduo.cs_171_171"));//夺宝奇兵扫荡

                    this.transform.Find("frame_big").GetComponent<frame>().hide();
                    s_message _mes = new s_message();
                    _mes.m_type = "refresh_bw_gui";
                    cmessage_center._instance.add_message(_mes);
                }
                if (_msg1.rewards[i].pgold > 0)
                {
                    sys._instance.m_self.add_att(e_player_attr.player_gold, _msg1.rewards[i].pgold, false, game_data._instance.get_t_language("baowu_qiangduo.cs_171_171"));//夺宝奇兵扫荡
                }
                else
                {
                    s_t_sport_card card = game_data._instance.get_t_sport_card(_msg1.rewards[i].card);
                    sys._instance.m_self.add_reward(card.type, card.value1, card.value2, card.value3, false, game_data._instance.get_t_language("baowu_qiangduo.cs_171_171"));//夺宝奇兵扫荡
                }
            }
            sys._instance.m_self.zs_skill_target(4, 5);
            m_saodang.GetComponent<qiang_duo_saodang>().init(_msg1.rewards);
            m_saodang.SetActive(true);
        }
    }

    void time()
    {
        long _time = (long)(m_time - timer.now());
        if (_time >= 0)
        {
            m_time_obj.text = timer.get_time_show((long)_time);
        }
        else
        {
            m_time_obj.text = game_data._instance.get_t_language("baowu_qiangduo.cs_208_21");//换一换
            CancelInvoke();
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "qiangduo_1")
        {
            m_index = (int)message.m_ints[0];
            if (sys._instance.m_self.m_t_player.energy >= 2)
            {
                if (sys._instance.m_self.m_t_player.treasure_protect_next_time < timer.now())
                {
                    protocol.game.cmsg_treasure_fight_end _msg1 = new protocol.game.cmsg_treasure_fight_end();

                    _msg1.player_guid = _msg.player_guids[m_index + refresh_number * 3];
                    net_http._instance.send_msg<protocol.game.cmsg_treasure_fight_end>(opclient_t.CMSG_TREASURE_FIGHT_END, _msg1);
                }
                else
                {
                    if (_msg.player_npcs[m_index + refresh_number * 3])
                    {
                        protocol.game.cmsg_treasure_fight_end _msg1 = new protocol.game.cmsg_treasure_fight_end();
                        _msg1.player_guid = _msg.player_guids[m_index + refresh_number * 3];
                        net_http._instance.send_msg<protocol.game.cmsg_treasure_fight_end>(opclient_t.CMSG_TREASURE_FIGHT_END, _msg1);
                    }
                    else
                    {
                        m_index = (int)message.m_ints[0];
                        s_message _mes = new s_message();
                        _mes.m_type = "qiangduo_10_next";
                        root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language("arena.cs_104_45"), game_data._instance.get_t_language("主动攻击玩家会丢失保护状态,抢npc不会失去保护"), _mes);//提示
                    }
                }
            }
            else
            {
                int item_id = 10010007;
                int num = sys._instance.m_self.get_item_num((uint)item_id);
                if (num > 0)
                {
                    root_gui._instance.show_tili_dialog_box(item_id);
                    return;
                }
                else
                {
                    s_message _message = new s_message();
                    _message.m_type = "buy_num_gui";
                    _message.m_ints.Add(100300);
                    _message.m_ints.Add(2);
                    cmessage_center._instance.add_message(_message);
                    return;
                }
            }
        }
        else if (message.m_type == "qiangduo_5")
        {
            m_index = (int)message.m_ints[0];
            if (sys._instance.m_self.m_t_player.energy >= 2)
            {
                if (sys._instance.m_self.m_t_player.treasure_protect_next_time < timer.now())
                {
                    protocol.game.cmsg_treasure_saodang _msg1 = new protocol.game.cmsg_treasure_saodang();
                    _msg1.player_guid = _msg.player_guids[m_index + refresh_number * 3];
                    net_http._instance.send_msg<protocol.game.cmsg_treasure_saodang>(opclient_t.CMSG_TREASURE_SAODANG, _msg1);
                }
                else
                {
                    if (_msg.player_npcs[m_index + refresh_number * 3])
                    {
                        protocol.game.cmsg_treasure_saodang _msg1 = new protocol.game.cmsg_treasure_saodang();
                        _msg1.player_guid = _msg.player_guids[m_index + refresh_number * 3];
                        net_http._instance.send_msg<protocol.game.cmsg_treasure_saodang>(opclient_t.CMSG_TREASURE_SAODANG, _msg1);
                    }
                    else
                    {
                        m_index = (int)message.m_ints[0];
                        s_message _mes = new s_message();
                        _mes.m_type = "qiangduo_50_next";
                        root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language("arena.cs_104_45"), game_data._instance.get_t_language("主动攻击玩家会丢失保护状态,抢npc不会失去保护"), _mes);//提示
                    }
                }
            }
            else
            {
                int item_id = 10010007;
                int num = sys._instance.m_self.get_item_num((uint)item_id);
                if (num > 0)
                {
                    root_gui._instance.show_tili_dialog_box(item_id);
                    return;
                }
                else
                {
                    s_message _message = new s_message();
                    _message.m_type = "buy_num_gui";
                    _message.m_ints.Add(100300);
                    _message.m_ints.Add(2);
                    cmessage_center._instance.add_message(_message);
                    return;
                }
            }
        }
        else if (message.m_type == "qiangduo_10_next")
        {
            protocol.game.cmsg_treasure_fight_end _msg1 = new protocol.game.cmsg_treasure_fight_end();
            _msg1.player_guid = _msg.player_guids[m_index + refresh_number * 3];
            net_http._instance.send_msg<protocol.game.cmsg_treasure_fight_end>(opclient_t.CMSG_TREASURE_FIGHT_END, _msg1);
        }
        else if (message.m_type == "qiangduo_50_next")
        {
            protocol.game.cmsg_treasure_saodang _msg1 = new protocol.game.cmsg_treasure_saodang();
            _msg1.player_guid = _msg.player_guids[m_index + refresh_number * 3];
            net_http._instance.send_msg<protocol.game.cmsg_treasure_saodang>(opclient_t.CMSG_TREASURE_SAODANG, _msg1);
        }
        else if (message.m_type == "treasure_result1")
        {
            protocol.game.smsg_treasure_fight_end msg = (protocol.game.smsg_treasure_fight_end)message.m_object[0];
            if (msg.suipian_id != 0)
            {
                this.transform.Find("frame_big").GetComponent<frame>().hide();
            }
            else
            {
                if (msg.result == 1)
                {
                    m_qiangduo_subs[m_index].reset();
                }
            }
        }
    }
}
