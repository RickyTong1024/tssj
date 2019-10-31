using System.Collections.Generic;
using UnityEngine;

public class skill_target_ex
{
	public int m_site;
	public int m_type;
	public int m_mian_y;
	public int m_san_b;
	public int m_bao_j;
	public int m_ge_d;
	public double m_value;
	public double m_hp;
	public int m_mp;
	public int m_mp_bh;
	public int m_jght;
	public int m_wshj;
	public int m_dlhj;
	public int m_shuang_b;
	public int m_yl;
	public double m_yl_value;
    public double m_hd;
}

public class buffer_target_ex
{
	public int m_site;
	public int m_id;
	public int m_col;
}

public class skill_ex
{

    public int m_index = 0;
    public int m_site;
    public s_t_skill m_t_skill;
    public int m_xx;
    public double m_xx_value;
    public double m_hp;
    public int m_mp;
    public int m_mp_ch;
    public int m_ft;
    public double m_ft_value;

    public List<skill_target_ex> m_skill_target_exs = new List<skill_target_ex>();
    public List<buffer_target_ex> m_buffer_target_exs = new List<buffer_target_ex>();
    public List<buffer_target_ex> m_buffer_ends = new List<buffer_target_ex>();

    public int do_zz;
    public int do_zz_site;
    public List<s_buttle_message> m_message = new List<s_buttle_message>();
}


public class battle_logic_ex : MonoBehaviour, IMessage
{

    public static battle_logic_ex _instance;

    [System.NonSerialized]
    public List<s_buttle_message> m_buttle_message = new List<s_buttle_message>();
    [System.NonSerialized]
    public protocol.game.msg_fight_text m_msg_fight_text;
    private protocol.game.msg_fight_bo m_msg_fight_bo;
    [System.NonSerialized]
    public int m_max_battle_num = 20;
    private int m_cur_battle_num = 0;
    [System.NonSerialized]
    public int m_max_round = 0;
    private int m_cur_round = 0;

    private protocol.game.smsg_mission_fight_end m_smsg_mission_fight_end;
    private protocol.game.smsg_qiecuo m_qiecuo_fight_end;
    private protocol.game.smsg_treasure_fight_end m_smsg_treasure_fight_end;
    private protocol.game.smsg_guild_mission_fight_end m_msg_guild_boss_fight_end;
    private protocol.game.smsg_pvp_fight_end m_msg_pvp_fight_end;
    private protocol.team.smsg_fight_ds m_msg_ds_fight_end;

    private protocol.game.smsg_sport_fight_end m_smsg_sport_fight_end;
    private protocol.game.smsg_boss_fight_end m_smsg_boss_fight_end;
    private protocol.game.smsg_qiyu_fight_end m_smsg_qiyu_fight_end;
    private protocol.game.smsg_yb_ybq_fight_end m_smsg_yb_ybq_fight_end;
    private protocol.game.smsg_ttt_fight_end m_smsg_ttt_fight_end;
    private protocol.game.smsg_hbb_fight_end m_smsg_hbb_fight_end;
    private protocol.game.smsg_ore_fight_end m_smsg_ore_fight_end;

    private protocol.game.smsg_huodong_tansuo_event m_msg_explore_fight;
    private protocol.game.smsg_guild_fight m_msg_guildpvp_fight;

    private string m_battle_type;
    private bool m_win = false;
    private int m_fight_type = 1;
    private bool m_skip = false;
    private bool is_exchange = false;

    private void set_win(int res)
    {
        if (res >= 1)
        {
            m_win = true;
        }
        else
        {
            m_win = false;
        }
    }

    public void start_logic()
    {
        battle._instance.set_start(true);
        m_skip = false;
        is_exchange = false;
        battle_ex();
    }

    public void set_ore_fight_end(protocol.game.smsg_ore_fight_end msg)
    {
        m_smsg_ore_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "ore_result";
    }

    public void set_ttt_fight_end(protocol.game.smsg_ttt_fight_end msg)
    {
        m_smsg_ttt_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "ttt_result";
    }

    public void set_transport_end(protocol.game.smsg_yb_ybq_fight_end msg)
    {
        m_smsg_yb_ybq_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "transport_result";
    }

    public void set_hbb_fight_end(protocol.game.smsg_hbb_fight_end msg)
    {
        m_smsg_hbb_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "hbb_result";
    }
    public void set_boss_fight_end(protocol.game.smsg_boss_fight_end msg)
    {
        m_smsg_boss_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "boss_result";
    }
    public void set_qiyu_fight_end(protocol.game.smsg_qiyu_fight_end msg)
    {
        m_smsg_qiyu_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "qiyu_result";
    }

    public void set_sport_fight_end(protocol.game.smsg_sport_fight_end msg)
    {
        m_smsg_sport_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "sport_result";
    }

    public void set_mission_fight_end(protocol.game.smsg_mission_fight_end msg)
    {
        m_smsg_mission_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        m_fight_type = _text.fight_type;
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "mission_result";
    }

    public void set_qiecuo_fight_end(protocol.game.smsg_qiecuo msg)
    {
        m_qiecuo_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        m_fight_type = _text.fight_type;
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "qiecuo_result";
    }

    public void set_treasure_fight_end(protocol.game.smsg_treasure_fight_end msg)
    {
        m_smsg_treasure_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        m_fight_type = _text.fight_type;
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "treasure_result";
    }

    public void set_guild_boss_fight_end(protocol.game.smsg_guild_mission_fight_end msg)
    {
        m_msg_guild_boss_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        m_fight_type = _text.fight_type;
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "guild_boss_result";
    }

    public void set_guild_pvp_fight_end(protocol.game.smsg_guild_fight msg)
    {
        m_msg_guildpvp_fight = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "guildfight_result";
    }

    public void set_explore_fight_end(protocol.game.smsg_huodong_tansuo_event msg)
    {
        m_msg_explore_fight = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        m_fight_type = _text.fight_type;
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "explore_result";
    }

    public void set_pvp_fight_end(protocol.game.smsg_pvp_fight_end msg)
    {
        m_msg_pvp_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        m_fight_type = _text.fight_type;
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "pvp_result";
    }

    public void set_ds_fight_end(protocol.team.smsg_fight_ds msg)
    {
        m_msg_ds_fight_end = msg;
        protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
        m_fight_type = _text.fight_type;
        set_fight_text(_text);
        set_win(msg.result);
        m_battle_type = "ds_result";
    }

    public void skip()
    {
        m_buttle_message.Clear();
        for (int i = 0; i < m_msg_fight_text.bos.Count; i++)
        {
            protocol.game.msg_fight_bo _bo = m_msg_fight_text.bos[i];

            for (int c = 0; c < _bo.ticks.Count; c++)
            {
                if (_bo.ticks[c].type == 4)
                {
                    m_cur_battle_num++;
                }
            }
        }

        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "set_buttle_index";
            _message.ints.Add(m_cur_battle_num);
            _message.ints.Add(m_max_battle_num);
            m_buttle_message.Add(_message);
        }

        protocol.game.msg_fight_state _end = m_msg_fight_text.end_state;
        for (int i = 0; i < 15; i++)
        {
            bool _die = true;

            for (int c = 0; c < _end.roles.Count; c++)
            {
                protocol.game.msg_fight_role _role = _end.roles[c];

                if (_role.site == i)
                {
                    _die = false;
                    break;
                }
            }

            if (_die == true)
            {
                s_buttle_message _message = new s_buttle_message();
                _message.type = "unit_die";
                _message.m_site = i;
                m_buttle_message.Add(_message);
            }
        }

        add_unit(_end.roles);
        m_msg_fight_bo = null;
        m_msg_fight_text.bos.Clear();
    }

    public void set_fight_text(protocol.game.msg_fight_text msg)
    {
        m_cur_battle_num = 0;
        m_msg_fight_text = msg;
        m_fight_type = m_msg_fight_text.fight_type;
        m_max_round = m_msg_fight_text.bos.Count;
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }


    void IMessage.net_message(s_net_message message)
    {

    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "skip_battle")
        {
            m_skip = true;
        }
        else if (message.m_type == "ore_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "tu_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "boss_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "guild_boss_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "sport_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "treasure_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "transport_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "ttt_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "guild_boss_ex_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "yingjiu_fight")
        {
            battle_ex();
        }
        else if (message.m_type == "battle_logic")
        {
            if (m_skip == true)
            {
                m_skip = false;
                skip();
            }
            else
            {
                battle_ex();
            }
        }
    }

    void result()
    {
        m_cur_round = 0;
        s_message _message = new s_message();
        _message.m_type = "result_show";
        _message.m_bools.Add(m_win);
        cmessage_center._instance.add_message(_message);

        if (m_battle_type == "mission_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_mission_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "qiecuo_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_qiecuo_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "ttt_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_ttt_fight_end);
            _message.m_bools.Add(m_win);

            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "qiangduo_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_treasure_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "transport_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_yb_ybq_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "boss_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_boss_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "qiyu_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_qiyu_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "sport_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_sport_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "treasure_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_treasure_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "guild_boss_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_msg_guild_boss_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "guildfight_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_msg_guildpvp_fight);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "explore_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_msg_explore_fight);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "pvp_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_msg_pvp_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "ds_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_msg_ds_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "hbb_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_hbb_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }
        else if (m_battle_type == "ore_result")
        {
            _message = new s_message();
            _message.time = 1.5f;
            _message.m_type = m_battle_type;
            _message.m_object.Add(m_smsg_ore_fight_end);
            _message.m_bools.Add(m_win);
            cmessage_center._instance.add_message(_message);
        }

        battle._instance.set_start(false);
    }
    private Vector3 get_pos_index(int min_dw, int max_dw, int duiwei, int camp)
    {
        Vector3 _pos = new Vector3(0, 0, 0);
        int x = duiwei % 6;
        int y = duiwei / 6;
        int dt = max_dw - min_dw;
        float xx = (x - min_dw - dt / 2.0f) * 1.5f;
        float yy = -5 - y * 1.5f;
        if (camp == 1)
        {
            xx = -xx * 1.5f * 1.2f;
            yy = 3 + y * 1.5f * 2f;
        }
        if (dt == 1)
        {
            xx *= 1.5f;
        }
        else if (dt == 2)
        {
            xx *= 1.3f;
        }
        else if (dt == 3)
        {
            xx *= 1.18f;
        }
        else if (dt == 4)
        {
            xx *= 1.06f;
        }
        _pos.x = xx;
        _pos.z = yy;
        return _pos;
    }

    public void add_battle_wait(float time)
    {
        s_buttle_message _buttle_message = new s_buttle_message();
        _buttle_message.type = "buttle_wait";
        _buttle_message.floats.Add(time);
        m_buttle_message.Add(_buttle_message);
    }

    private void battle_ex()
    {
        if (m_msg_fight_bo != null && m_msg_fight_bo.ticks.Count == 0)
        {
            m_msg_fight_text.bos.RemoveAt(0);
            m_msg_fight_bo = null;
        }

        if (m_msg_fight_bo == null && m_msg_fight_text.bos.Count > 0)
        {

            m_msg_fight_bo = m_msg_fight_text.bos[0];
            add_unit(m_msg_fight_bo);
            s_buttle_message _message = new s_buttle_message();
            _message.type = "set_round_index";
            m_cur_round++;
            _message.ints.Add(m_cur_round);
            _message.ints.Add(m_max_round);
            m_buttle_message.Add(_message);
            return;
        }

        if (m_msg_fight_bo == null)
        {
            s_message m_message = new s_message();
            m_message.m_type = "hide_pet";
            cmessage_center._instance.add_message(m_message);
            result();
            return;
        }

        protocol.game.msg_fight_tick _tick = m_msg_fight_bo.ticks[0];

        if (_tick.type == 1)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "buffer_out";
            _message.m_site = _tick.values[0];
            _message.ints.Add((int)3);
            _message.doubles.Add((int)_tick.dvalues[0]);
            _message.doubles.Add((int)_tick.dvalues[1]);
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 2)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "buffer_out";
            _message.m_site = _tick.values[0];
            _message.ints.Add((int)_tick.values[1]);
            _message.doubles.Add((int)_tick.dvalues[0]);
            _message.doubles.Add((int)_tick.dvalues[1]);
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 3 || _tick.type == 18)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "buffer_end";
            _message.m_site = _tick.values[0];
            _message.ints.Add((int)_tick.values[1]);
            _message.ints.Add((int)_tick.values[2]);
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 4)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "set_buttle_index";
            m_cur_battle_num++;
            _message.ints.Add(m_cur_battle_num);
            _message.ints.Add(m_max_battle_num);
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 5)
        {
            if (is_exchange)
            {
                s_buttle_message m_message = new s_buttle_message();
                m_message.m_site = _tick.values[0];
                m_message.type = "hide_pet";
                m_message.ints.Add(1);
                m_buttle_message.Add(m_message);
                is_exchange = false;
                add_battle_wait(1.0f);
            }
            s_buttle_message _message = new s_buttle_message();
            _message.type = "buttle_fu_huo_0";
            _message.m_site = _tick.values[0];
            m_buttle_message.Add(_message);
            _message = new s_buttle_message();
            _message.type = "buttle_fu_huo_1";
            _message.m_site = _tick.values[0];
            _message.ints.Add(_tick.values[1]);
            _message.doubles.Add(_tick.dvalues[0]);
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 6)
        {
            if (is_exchange)
            {
                s_buttle_message m_message = new s_buttle_message();
                m_message.m_site = _tick.values[0];
                m_message.type = "hide_pet";
                m_message.ints.Add(1);
                m_buttle_message.Add(m_message);
                is_exchange = false;
                add_battle_wait(1.0f);
            }

            s_buttle_message _message = new s_buttle_message();
            _message.type = "unit_die";
            _message.m_site = _tick.values[0];
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 7)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "fu_chou";
            _message.m_site = _tick.values[0];
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 8)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "fen_xie";
            _message.m_site = _tick.values[0];
            _message.ints.Add(_tick.values[1]);
            m_msg_fight_bo.ticks.RemoveAt(0);
            while (m_msg_fight_bo.ticks.Count > 0)
            {
                if (m_msg_fight_bo.ticks[0].type == 9)
                {
                    _tick = m_msg_fight_bo.ticks[0];
                    _message.ints.Add(_tick.values[0]);
                    _message.doubles.Add(_tick.dvalues[0]);
                    _message.doubles.Add(_tick.dvalues[1]);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
            m_buttle_message.Add(_message);
        }
        else if (_tick.type == 10)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "xi_sheng";
            _message.m_site = _tick.values[0];
            m_msg_fight_bo.ticks.RemoveAt(0);
            while (m_msg_fight_bo.ticks.Count > 0 && m_msg_fight_bo.ticks[0].type == 11)
            {
                _tick = m_msg_fight_bo.ticks[0];
                _message.ints.Add(_tick.values[0]);
                _message.doubles.Add(_tick.dvalues[0]);
                _message.doubles.Add(_tick.dvalues[1]);
                m_msg_fight_bo.ticks.RemoveAt(0);
            }
            m_buttle_message.Add(_message);
        }
        else if (_tick.type == 12)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "round_end_add_hp";
            _message.m_site = _tick.values[0];
            m_msg_fight_bo.ticks.RemoveAt(0);
            while (m_msg_fight_bo.ticks.Count > 0 && m_msg_fight_bo.ticks[0].type == 13)
            {
                _tick = m_msg_fight_bo.ticks[0];
                _message.ints.Add(_tick.values[0]);
                _message.doubles.Add(_tick.dvalues[0]);
                _message.doubles.Add(_tick.dvalues[1]);
                m_msg_fight_bo.ticks.RemoveAt(0);
            }
            m_buttle_message.Add(_message);
        }
        else if (_tick.type == 23)
        {
            #region
            protocol.game.msg_fight_tick tick = _tick;
            m_msg_fight_bo.ticks.RemoveAt(0);
            _tick = m_msg_fight_bo.ticks[0];
            skill_ex _skill = new skill_ex();
            while (_tick.type != 14)
            {
                if (_tick.type == 15)
                {
                    skill_target_ex _target = new skill_target_ex();
                    _target.m_site = _tick.values[0];
                    _target.m_type = _tick.values[1];
                    if (_target.m_type == 4)
                    {
                        _target.m_mp = _tick.values[2];
                        _target.m_mp_bh = _tick.values[3];
                        _target.m_hp = _tick.dvalues[0];
                    }
                    else if (_target.m_type == 3)
                    {
                        _target.m_mp = _tick.values[2];
                        _target.m_mp_bh = _tick.values[3];
                        _target.m_value = _tick.dvalues[0];
                        _target.m_hp = _tick.dvalues[1];

                    }
                    else
                    {
                        _target.m_mp = _tick.values[2];
                        _target.m_mian_y = _tick.values[3];
                        _target.m_san_b = _tick.values[4];
                        _target.m_bao_j = _tick.values[5];
                        _target.m_ge_d = _tick.values[6];
                        _target.m_mp_bh = _tick.values[7];
                        _target.m_jght = _tick.values[8];
                        _target.m_wshj = _tick.values[9];
                        _target.m_dlhj = _tick.values[10];
                        _target.m_shuang_b = _tick.values[11];
                        _target.m_yl = _tick.values[12];
                        _target.m_value = _tick.dvalues[0];
                        _target.m_hp = _tick.dvalues[1];
                        _target.m_yl_value = _tick.dvalues[2];
                        _target.m_hd = _tick.dvalues[3];
                    }
                    _skill.m_skill_target_exs.Add(_target);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 16)
                {
                    buffer_target_ex _buffer = new buffer_target_ex();
                    _buffer.m_site = _tick.values[0];
                    _buffer.m_id = _tick.values[1];
                    _buffer.m_col = _tick.values[2];
                    _skill.m_buffer_target_exs.Add(_buffer);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 18)
                {
                    buffer_target_ex _buffer = new buffer_target_ex();
                    _buffer.m_site = _tick.values[0];
                    _buffer.m_id = _tick.values[1];
                    _buffer.m_col = _tick.values[2];
                    _skill.m_buffer_ends.Add(_buffer);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 19)
                {
                    s_buttle_message _message = new s_buttle_message();
                    _message.type = "round_end_add_nl";
                    _message.m_site = _tick.values[0];
                    m_msg_fight_bo.ticks.RemoveAt(0);
                    while (m_msg_fight_bo.ticks[0].type == 20)
                    {
                        _tick = m_msg_fight_bo.ticks[0];
                        _message.ints.Add(_tick.values[0]);
                        _message.ints.Add(_tick.values[1]);
                        _message.ints.Add(_tick.values[2]);
                        m_msg_fight_bo.ticks.RemoveAt(0);
                    }
                    m_buttle_message.Add(_message);
                }
                else if (_tick.type == 21)
                {
                    s_buttle_message _message = new s_buttle_message();
                    _message.type = "chufa_zskill";
                    _message.m_site = _tick.values[0];
                    _message.ints.Add(_tick.values[1]);
                    m_buttle_message.Add(_message);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 22)
                {
                    _skill.do_zz = _tick.values[1];
                    _skill.do_zz_site = _tick.values[0];
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 25)
                {
                    print(game_data._instance.get_t_language("battle_logic_ex.cs_1017_26"));//触发克制技能
                    skill_target_ex _target = new skill_target_ex();
                    _target.m_type = 25;
                    _target.m_site = _tick.values[0];
                    _skill.m_skill_target_exs.Add(_target);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 26)
                {
                    skill_target_ex _target = new skill_target_ex();
                    _target.m_type = 26;
                    _target.m_site = _tick.values[0];
                    _target.m_yl = _tick.values[1];
                    if (_target.m_yl == 1)
                    {
                        _target.m_yl_value = _tick.dvalues[0];
                        print(_target.m_yl + ":" + _target.m_yl_value);

                    }
                    else
                    {
                        _target.m_yl_value = battle._instance.get_unit_site(_target.m_site).GetComponent<unit>().m_max_hd;
                    }
                    _skill.m_skill_target_exs.Add(_target);
                    m_msg_fight_bo.ticks.RemoveAt(0);

                }
                else if (_tick.type == 12)
                {
                    s_buttle_message _message = new s_buttle_message();
                    _message.type = "round_end_add_hp";
                    _message.m_site = _tick.values[0];
                    m_msg_fight_bo.ticks.RemoveAt(0);
                    while (m_msg_fight_bo.ticks.Count > 0 && m_msg_fight_bo.ticks[0].type == 13)
                    {
                        _tick = m_msg_fight_bo.ticks[0];
                        _message.ints.Add(_tick.values[0]);
                        _message.doubles.Add(_tick.dvalues[0]);
                        _message.doubles.Add(_tick.dvalues[1]);
                        m_msg_fight_bo.ticks.RemoveAt(0);
                    }
                    _skill.m_message.Add(_message);
                }
                else
                {
                    m_msg_fight_bo.ticks.RemoveAt(0);
                    Debug.LogError(game_data._instance.get_t_language("battle_logic_ex.cs_1072_35") + _tick.type);//丢失的tick:
                }
                _tick = m_msg_fight_bo.ticks[0];
            }

            if (is_exchange)
            {
                s_buttle_message m_message = new s_buttle_message();
                m_message.m_site = _tick.values[0];
                m_message.type = "hide_pet";
                m_message.ints.Add(1);
                m_buttle_message.Add(m_message);
                is_exchange = false;
                add_battle_wait(2.5f);
            }

            _skill.m_site = _tick.values[0];
            _skill.m_t_skill = game_data._instance.get_t_skill(_tick.values[1]);
            _skill.m_mp = _tick.values[2];
            _skill.m_xx = _tick.values[3];
            _skill.m_ft = _tick.values[4];
            _skill.m_mp_ch = _tick.values[5];
            _skill.m_xx_value = _tick.dvalues[0];
            _skill.m_hp = _tick.dvalues[1];
            _skill.m_ft_value = _tick.dvalues[2];
            s_buttle_message _buttle_message = new s_buttle_message();
            _buttle_message.m_site = _skill.m_site;
            _buttle_message.type = "release_skill";
            _buttle_message.values.Add(_skill);
            m_buttle_message.Add(_buttle_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
            #endregion
        }
        else if (_tick.type == 24)
        {
            #region
            is_exchange = true;
            s_buttle_message m_message = new s_buttle_message();
            m_message.m_site = _tick.values[0];
            m_message.type = "hide_role";
            m_buttle_message.Add(m_message);
            add_battle_wait(2.5f);
            m_msg_fight_bo.ticks.RemoveAt(0);
            _tick = m_msg_fight_bo.ticks[0];
            skill_ex _skill = new skill_ex();

            while (_tick.type != 14)
            {
                if (_tick.type == 15)
                {
                    skill_target_ex _target = new skill_target_ex();
                    _target.m_site = _tick.values[0];
                    _target.m_type = _tick.values[1];
                    if (_target.m_type == 4)
                    {
                        _target.m_mp = _tick.values[2];
                        _target.m_mp_bh = _tick.values[3];
                        _target.m_hp = _tick.dvalues[0];
                    }
                    else if (_target.m_type == 3)
                    {
                        _target.m_mp = _tick.values[2];
                        _target.m_mp_bh = _tick.values[3];
                        _target.m_value = _tick.dvalues[0];
                        _target.m_hp = _tick.dvalues[1];
                    }
                    else
                    {
                        _target.m_mp = _tick.values[2];
                        _target.m_mian_y = _tick.values[3];
                        _target.m_san_b = _tick.values[4];
                        _target.m_bao_j = _tick.values[5];
                        _target.m_ge_d = _tick.values[6];
                        _target.m_mp_bh = _tick.values[7];
                        _target.m_jght = _tick.values[8];
                        _target.m_wshj = _tick.values[9];
                        _target.m_dlhj = _tick.values[10];
                        _target.m_shuang_b = _tick.values[11];
                        _target.m_yl = _tick.values[12];
                        _target.m_value = _tick.dvalues[0];
                        _target.m_hp = _tick.dvalues[1];
                        _target.m_yl_value = _tick.dvalues[2];
                    }
                    _skill.m_skill_target_exs.Add(_target);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 16)
                {
                    buffer_target_ex _buffer = new buffer_target_ex();
                    _buffer.m_site = _tick.values[0];
                    _buffer.m_id = _tick.values[1];
                    _buffer.m_col = _tick.values[2];
                    _skill.m_buffer_target_exs.Add(_buffer);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 18)
                {
                    buffer_target_ex _buffer = new buffer_target_ex();
                    _buffer.m_site = _tick.values[0];
                    _buffer.m_id = _tick.values[1];
                    _buffer.m_col = _tick.values[2];
                    _skill.m_buffer_ends.Add(_buffer);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 21)
                {
                    s_buttle_message _message = new s_buttle_message();
                    _message.type = "chufa_zskill";
                    _message.m_site = _tick.values[0];
                    _message.ints.Add(_tick.values[1]);
                    m_buttle_message.Add(_message);
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 22)
                {
                    _skill.do_zz = _tick.values[1];
                    _skill.do_zz_site = _tick.values[0];
                    m_msg_fight_bo.ticks.RemoveAt(0);
                }
                else if (_tick.type == 12)
                {
                    s_buttle_message _message = new s_buttle_message();
                    _message.type = "round_end_add_hp";
                    _message.m_site = _tick.values[0];
                    m_msg_fight_bo.ticks.RemoveAt(0);
                    while (m_msg_fight_bo.ticks.Count > 0 && m_msg_fight_bo.ticks[0].type == 13)
                    {
                        _tick = m_msg_fight_bo.ticks[0];
                        _message.ints.Add(_tick.values[0]);
                        _message.doubles.Add(_tick.dvalues[0]);
                        _message.doubles.Add(_tick.dvalues[1]);
                        m_msg_fight_bo.ticks.RemoveAt(0);
                    }
                    _skill.m_message.Add(_message);
                }
                else
                {
                    m_msg_fight_bo.ticks.RemoveAt(0);
                    Debug.LogError(game_data._instance.get_t_language("battle_logic_ex.cs_1072_35") + _tick.type);//丢失的tick:
                }
                _tick = m_msg_fight_bo.ticks[0];
            }

            _skill.m_site = _tick.values[0];
            _skill.m_t_skill = game_data._instance.get_t_skill(_tick.values[1]);
            _skill.m_mp = _tick.values[2];
            _skill.m_xx = _tick.values[3];
            _skill.m_ft = _tick.values[4];
            _skill.m_mp_ch = _tick.values[5];
            _skill.m_xx_value = _tick.dvalues[0];
            _skill.m_hp = _tick.dvalues[1];
            _skill.m_ft_value = _tick.dvalues[2];
            s_buttle_message _buttle_message = new s_buttle_message();
            _buttle_message.m_site = _skill.m_site;
            _buttle_message.type = "release_skill";
            _buttle_message.values.Add(_skill);
            m_buttle_message.Add(_buttle_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
            #endregion
        }
        else if (_tick.type == 17)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "double_attack";
            _message.m_site = _tick.values[0];
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 19)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "round_end_add_nl";
            _message.m_site = _tick.values[0];
            m_msg_fight_bo.ticks.RemoveAt(0);

            while (m_msg_fight_bo.ticks[0].type == 20)
            {
                _tick = m_msg_fight_bo.ticks[0];
                _message.ints.Add(_tick.values[0]);
                _message.ints.Add(_tick.values[1]);
                _message.ints.Add(_tick.values[2]);
                m_msg_fight_bo.ticks.RemoveAt(0);
            }
            m_buttle_message.Add(_message);
        }

        else if (_tick.type == 21)
        {
            s_buttle_message _message = new s_buttle_message();
            _message.type = "chufa_zskill";
            _message.m_site = _tick.values[0];
            _message.ints.Add(_tick.values[1]);
            m_buttle_message.Add(_message);
            m_msg_fight_bo.ticks.RemoveAt(0);
        }
        else if (_tick.type == 25)
        {


        }
        else if (_tick.type == 99)
        {
            m_msg_fight_bo.ticks.RemoveAt(0);
        }

    }

    private void add_unit(List<protocol.game.msg_fight_role> roles)
    {
        List<protocol.game.msg_fight_role> _role_0s = new List<protocol.game.msg_fight_role>();
        List<protocol.game.msg_fight_role> _role_1s = new List<protocol.game.msg_fight_role>();
        protocol.game.msg_fight_role _pet_0 = null;
        protocol.game.msg_fight_role _pet_1 = null;

        for (int i = 0; i < roles.Count; i++)
        {
            protocol.game.msg_fight_role _role = roles[i];

            if (_role.site <= 5)
            {
                _role_0s.Add(_role);
            }
            else if (_role.site == 100)
            {
                _pet_0 = _role;
            }
            else if (_role.site == 101)
            {
                _pet_1 = _role;
            }
            else if (m_fight_type == 0)
            {
                _role_1s.Add(_role);
            }
        }

        for (int i = 0; i < roles.Count && m_fight_type == 1; i++)
        {
            protocol.game.msg_fight_role _role = roles[i];
            s_t_class _class = game_data._instance.get_t_class(_role.id);

            if (_role.site > 5 && _role.site != 101 && _role.site != 100)
            {
                _role_1s.Add(_role);
            }
        }

        s_buttle_message _message = new s_buttle_message();
        _message.type = "add_unit";
        int max_wei = -1;
        int min_wei = 999;
        for (int i = 0; i < _role_0s.Count; i++)
        {
            protocol.game.msg_fight_role _role = _role_0s[i];
            int wei = _role.duiwei % 6;
            if (wei < min_wei)
            {
                min_wei = wei;
            }
            if (wei > max_wei)
            {
                max_wei = wei;
            }
        }
        for (int i = 0; i < _role_0s.Count; i++)
        {
            protocol.game.msg_fight_role _role = _role_0s[i];

            _message.doubles.Add(_role.max_hp);
            _message.doubles.Add(_role.cur_hp);
            _message.ints.Add(_role.id);
            _message.ints.Add(_role.duiwei);
            _message.ints.Add(_role.site);
            _message.ints.Add(_role.nengliang);
            _message.ints.Add(_role.glevel);
            _message.ints.Add(_role.jlevel);
            _message.ints.Add(_role.pinzhi);
            _message.ints.Add(_role.dress_id);
            _message.ints.Add(_role.guanghuan_id);
            _message.values.Add(get_pos_index(min_wei, max_wei, _role.duiwei, 0));
        }

        if (_pet_0 != null)
        {
            is_exchange = true;
            protocol.game.msg_fight_role _role = _pet_0;

            _message.doubles.Add(_role.max_hp);
            _message.doubles.Add(_role.cur_hp);
            _message.ints.Add(_role.id);
            _message.ints.Add(_role.duiwei);
            _message.ints.Add(_role.site);
            _message.ints.Add(_role.nengliang);
            _message.ints.Add(_role.glevel);
            _message.ints.Add(_role.jlevel);
            _message.ints.Add(_role.pinzhi);
            _message.ints.Add(_role.dress_id);
            _message.ints.Add(_role.guanghuan_id);
            _message.values.Add(new Vector3(0, 0, -5));
        }

        max_wei = -1;
        min_wei = 999;
        for (int i = 0; i < _role_1s.Count; i++)
        {
            protocol.game.msg_fight_role _role = _role_1s[i];
            int wei = _role.duiwei % 6;
            if (wei < min_wei)
            {
                min_wei = wei;
            }
            if (wei > max_wei)
            {
                max_wei = wei;
            }
        }
        for (int i = 0; i < _role_1s.Count; i++)
        {
            protocol.game.msg_fight_role _role = _role_1s[i];
            _message.doubles.Add(_role.max_hp);
            _message.doubles.Add(_role.cur_hp);
            _message.ints.Add(_role.id);
            _message.ints.Add(_role.duiwei);
            _message.ints.Add(_role.site);
            _message.ints.Add(_role.nengliang);
            _message.ints.Add(_role.glevel);
            _message.ints.Add(_role.jlevel);
            _message.ints.Add(_role.pinzhi);
            _message.ints.Add(_role.dress_id);
            _message.ints.Add(_role.guanghuan_id);
            _message.values.Add(get_pos_index(min_wei, max_wei, _role.duiwei, 1));
        }
        if (_pet_1 != null)
        {
            is_exchange = true;
            protocol.game.msg_fight_role _role = _pet_1;
            _message.doubles.Add(_role.max_hp);
            _message.doubles.Add(_role.cur_hp);
            _message.ints.Add(_role.id);
            _message.ints.Add(_role.duiwei);
            _message.ints.Add(_role.site);
            _message.ints.Add(_role.nengliang);
            _message.ints.Add(_role.glevel);
            _message.ints.Add(_role.jlevel);
            _message.ints.Add(_role.pinzhi);
            _message.ints.Add(_role.dress_id);
            _message.ints.Add(_role.guanghuan_id);
            _message.values.Add(new Vector3(0, 0, 3));
        }
        m_buttle_message.Add(_message);
    }

    private void add_unit(protocol.game.msg_fight_bo bo_state)
    {
        add_unit(bo_state.bo_state.roles);
    }

    void Update()
    {

        if (sys._instance.m_pause == true || sys._instance.m_is_director == true)
        {
            return;
        }

    }
}
