using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_mission_type
{
	buttle_mission = 0,
	buttle_mission_bz = 1,
	buttle_mission_guild = 2,
}

class s_reserves
{
	public Vector3 positon;
	public float time;
	public int index;
	public long guid;
	public List<long> reserves = new List<long>();
}

public class s_buttle_message
{
	public string type;
	public ulong guid;
	
	public int m_site;
	public List<string> strings = new List<string>();
	public List<int> ints = new List<int>();
	public List<bool> bools = new List<bool>();
	public List<double> doubles = new List<double>();
	public List<float> floats = new List<float>();
	public ArrayList values = new ArrayList ();
}

public class s_battle_step
{
	public int type;
	public int value_0;
	public int value_1;
	public int value_2;
	public List<int> values = new List<int>();
	public List<int> targets = new List<int>();
	public List<int> bj = new List<int>();
	public List<int> gd = new List<int>();
	public List<int> xx = new List<int>();
}

public class battle : MonoBehaviour, IMessage
{
    public static battle _instance;
    [System.NonSerialized]
    public List<GameObject> m_units = new List<GameObject>();

    private float m_wait_round = 0.0f;
    private float m_ficht_time = 0.0f;
    private float m_vs_time = 0.0f;
    private float m_result_time = 0.0f;

    private GameObject m_ui_ficht;
    private GameObject m_ui_round;
    private GameObject m_ui_buttle;
    private GameObject m_win_result;
    private GameObject m_win_gui;
    private GameObject m_win_result_2;
    private GameObject m_battle_main;
    private GameObject m_defeated_result_0;
    private GameObject m_defeated_result_1;
    private GameObject m_buttle_vs;
    private GameObject m_ui_hit;

    private s_message m_boss_message;
    private s_message m_result_message = new s_message();
    private s_message m_out_message = new s_message();
    private string m_result_state;

    private s_t_mission m_t_mission;
    public protocol.game.msg_guild_fight_info m_guildfightinfo;
    public int m_guildfightindex;
    private protocol.game.smsg_ds_fight_end m_ds_fight_end;
    private int cur_duanwei;
    private int pre_duanwei;
    private s_t_ore m_ore;

    private string m_last_state = "";
    private string m_last_scence = "";
    private List<player> m_players = new List<player>();
    private List<int> m_reward_type = new List<int>();
    private List<object> m_reward_object = new List<object>();

    private s_buttle_message m_buttle_result = null;
    private bool m_win = true;
    private GameObject m_jy_unit;
    private e_mission_type m_mission_type = e_mission_type.buttle_mission;
    private string m_play_move;
    private bool m_round_end = false;
    private bool m_start = false;
    public ulong m_timer_cd;
    public void set_start(bool start)
    {
        m_start = start;
    }

    public void get_battle_gui(bool flag)
    {
        if (flag)
        {
            m_ui_ficht = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_ui_ficht;
            m_ui_round = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_ui_round;
            m_ui_buttle = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_ui_buttle;
            m_win_result = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_win_result;
            m_win_gui = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_win_gui;
            m_win_result_2 = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_win_result_2;
            m_battle_main = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_battle_main;
            m_defeated_result_0 = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_defeated_result_0;
            m_defeated_result_1 = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_defeated_result_1;
            m_buttle_vs = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_buttle_vs;
            m_ui_hit = root_gui._instance.m_battle_gui.GetComponent<battle_gui>().m_ui_hit;
        }
        else
        {
            m_ui_ficht = null;
            m_ui_round = null;
            m_ui_buttle = null;
            m_win_result = null;
            m_win_gui = null;
            m_win_result_2 = null;
            m_battle_main = null;
            m_defeated_result_0 = null;
            m_defeated_result_1 = null;
            m_buttle_vs = null;
            m_ui_hit = null;
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

    void Awake()
    {
        _instance = this;
    }

    public void result_click(GameObject obj)
    {
        if (m_result_message.m_next == null)
        {
            m_result_message.m_next = new s_message();
        }
        cmessage_center._instance.add_message(m_result_message);
        m_result_message = new s_message();

        if (m_result_state != "")
        {
            sys._instance.m_game_state = m_result_state;
            m_result_state = "";
        }
    }

    void vs_show(player player_0, player player_1)
    {
        m_buttle_vs.transform.Find("head_0").Find("Label").GetComponent<UILabel>().text = player_0.m_t_player.name;
        m_buttle_vs.transform.Find("head_0").Find("logo").GetComponent<UISprite>().spriteName = player.get_touxiang((int)player_0.m_t_player.template_id);

        m_buttle_vs.transform.Find("head_1").Find("Label").GetComponent<UILabel>().text = player_1.m_t_player.name;
        m_buttle_vs.transform.Find("head_1").Find("logo").GetComponent<UISprite>().spriteName = player.get_touxiang((int)player_1.m_t_player.template_id);

        m_buttle_vs.SetActive(true);
        m_buttle_vs.GetComponent<hide_time>().m_time = 3.0f;

    }

    public void remove_all()
    {
        for (int i = 0; i < m_units.Count; i++)
        {
            GameObject.Destroy(m_units[i]);
        }
        m_units.Clear();
    }

    void IMessage.net_message(s_net_message message)
    {

    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "battle_scene_show")
        {
            m_battle_main.SetActive(true);
            sys._instance.m_reward_show.SetActive(false);
            sys._instance.m_buttle_cam.SetActive(true);
        }

        if (message.m_type == "battle_reward_show")
        {
            sys._instance.m_reward_show.SetActive(true);
            m_battle_main.SetActive(false);
            sys._instance.m_buttle_cam.SetActive(false);
        }

        if (message.m_type == "mission")
        {
            m_t_mission = game_data._instance.get_t_mission(int.Parse((string)message.m_string[0]));
        }
        else if (message.m_type == "guildfight_info")
        {
            m_guildfightinfo = (protocol.game.msg_guild_fight_info)message.m_object[0];
            m_guildfightindex = (int)message.m_ints[0];

        }
        if (message.m_type == "set_ds_fight_end")
        {
            m_ds_fight_end = (protocol.game.smsg_ds_fight_end)message.m_object[0];
            pre_duanwei = (int)message.m_ints[0];
            cur_duanwei = (int)message.m_ints[1];

        }
        if (message.m_type == "save_state")
        {
            m_last_state = message.m_string[0] as string;
            m_last_scence = message.m_string[1] as string;
        }
        if (message.m_type == "ore_index")
        {
            int index = (int)message.m_ints[0];
            m_ore = game_data._instance.get_t_ore(index);
        }
        if (message.m_type == "add_reward_show")
        {
            m_reward_type.Clear();
            m_reward_object.Clear();

            for (int i = 0; i < message.m_ints.Count; i++)
            {
                m_reward_type.Add((int)message.m_ints[i]);
            }

            for (int i = 0; i < message.m_object.Count; i++)
            {
                m_reward_object.Add((int)message.m_object[i]);
            }
        }
        if (message.m_type == "loaded")
        {
            if (sys._instance.m_game_state == "hall"
               || sys._instance.m_game_state == "boss"
               || sys._instance.m_game_state == "mi_jing"
               || sys._instance.m_game_state == "ying_jiu"
               || sys._instance.m_game_state == "tan_suo"
               || sys._instance.m_game_state == "guild_boss_ex"
               || sys._instance.m_game_state == "guild_fight_pvp"
               || sys._instance.m_game_state == "transport"
               || sys._instance.m_game_state == "bao_wu"
               || sys._instance.m_game_state == "ore"
                || sys._instance.m_game_state == "pvp"
                || sys._instance.m_game_state == "memory"
                || sys._instance.m_game_state == "bingyuan"
                || sys._instance.m_game_state == "master"
               || sys._instance.m_game_state == "explore_ex")
            {
                remove_all();

                m_t_mission = null;

                s_message _msg = new s_message();

                _msg.m_type = "restart_player_mp";
                cmessage_center._instance.add_message(_msg);
                battle_logic_ex._instance.m_buttle_message.Clear();
                m_wait_round = 0.0f;
                m_ficht_time = 0.0f;
                m_vs_time = 0.0f;
                m_result_time = 0.0f;
                set_round_index(1, 1);
                sys._instance.m_self.set_att(e_player_attr.player_mp, 0);
                root_gui._instance.m_ui_bottomleft.transform.gameObject.SetActive(true);
            }
            else if (sys._instance.m_game_state == "buttle")
            {
                if (m_t_mission != null && m_t_mission.lock_id == sys._instance.m_self.m_t_player.mission)
                {
                    string _br = "start_battle_" + sys._instance.m_self.m_t_player.mission;
                    root_gui._instance.action_guide(_br);
                }
            }
        }
        if (message.m_type == "defeated_result")
        {
            m_defeated_result_0.SetActive(true);

            cmessage_center._instance.add_message(m_out_message);
            m_out_message = new s_message();
        }
        if (message.m_type == "show_result_win")
        {
            buttle_result _result = m_win_gui.GetComponent<buttle_result>();

            _result.set_reward_num(m_reward_type.Count / 4 + m_reward_object.Count);

            for (int i = 0; i < m_reward_type.Count; i += 4)
            {
                _result.add_reward(m_reward_type[i], m_reward_type[i + 1], m_reward_type[i + 2], m_reward_type[i + 3]);
            }

            for (int i = 0; i < m_reward_object.Count; i++)
            {
                _result.add_reward((dhc.role_t)m_reward_object[i]);
            }

            m_reward_type.Clear();
            m_reward_object.Clear();

            m_win_gui.SetActive(true);
            cmessage_center._instance.add_message(m_out_message);
            m_out_message = new s_message();
        }
        if (message.m_type == "show_result_def")
        {
            m_defeated_result_0.SetActive(true);
            cmessage_center._instance.add_message(m_out_message);
            m_out_message = new s_message();
        }
        if (message.m_type == "skill_hit")
        {
            m_ui_hit.SetActive(true);
            m_ui_hit.transform.Find("Label").GetComponent<UILabel>().text = (string)message.m_string[0];
            m_ui_hit.transform.GetComponent<Animator>().Play("scale_anim_1", 0, 0);
            m_ui_hit.GetComponent<hide_time>().m_time = 2.0f;
        }
        else if (message.m_type == "hide_pet")
        {
            for (int i = 0; i < m_units.Count; i++)
            {
                if (m_units[i].GetComponent<unit>().m_site == 100 || m_units[i].GetComponent<unit>().m_site == 101)
                {
                    m_units[i].GetComponent<unit>().show_pet = 0;
                }
                else
                {
                    m_units[i].SetActive(true);
                    m_units[i].GetComponent<unit>().show_pet = 0;
                }
            }

        }
        if (message.m_type == "battle_result_0")
        {
            show_win(m_buttle_result.bools[0]);
        }
        if (message.m_type == "battle_result_1")
        {
            player_result_sound(m_buttle_result.bools[0]);
            if (m_buttle_result.type == "sport_result")
            {
                sport_result(m_buttle_result.bools[0]);
            }
            if (m_buttle_result.type == "treasure_result")
            {
                treasure_result(m_buttle_result.bools[0]);
            }
            if (m_buttle_result.type == "boss_result")
            {
                boss_result(m_buttle_result.bools[0], m_buttle_result.ints[0]);
            }

            if (m_buttle_result.type == "guild_boss_result")
            {
                guild_boss_result(m_buttle_result.bools[0], m_buttle_result.ints[0]);
            }

            if (m_buttle_result.type == "explore_result")
            {
                explore_result(m_buttle_result.bools[0], m_buttle_result.ints[0]);
            }

            if (m_buttle_result.type == "yingjiu_result")
            {
                yingjiu_result(m_buttle_result.bools[0]);
            }

            if (m_buttle_result.type == "mission_result")
            {
                mission_result(m_buttle_result.bools[0], (s_t_mission)m_buttle_result.values[0], m_buttle_result.ints[0]);
            }
            if (m_buttle_result.type == "tu_result")
            {
                tu_result(m_buttle_result.bools[0]);
            }
            if (m_buttle_result.type == "ore_result")
            {
                ore_result(m_buttle_result.bools[0], m_buttle_result.ints[0]);
            }
            if (m_buttle_result.type == "ttt_result")
            {
                ttt_result(m_buttle_result.bools[0]);
            }
            if (m_buttle_result.type == "guild_boss_ex_result")
            {
                guild_boss_ex_result(m_buttle_result.bools[0]);
            }
        }

        if (message.m_type == "result_show")
        {
            result_show(message.m_bools[0]);
        }
        else if (message.m_type == "ttt_result")
        {
            ttt_result_ex((protocol.game.smsg_ttt_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "transport_result")
        {
            transport_result_ex((protocol.game.smsg_yb_ybq_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "boss_result")
        {
            boss_result_ex((protocol.game.smsg_boss_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "qiyu_result")
        {
            qiyu_result_ex((protocol.game.smsg_qiyu_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "mission_result")
        {
            mission_result_ex((protocol.game.smsg_mission_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "qiecuo_result")
        {
            qiecuo_result_ex((protocol.game.smsg_qiecuo)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "sport_result")
        {
            sport_result_ex((protocol.game.smsg_sport_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "treasure_result")
        {
            treasure_result_ex((protocol.game.smsg_treasure_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "guild_boss_result")
        {
            guild_boss_result_ex((protocol.game.smsg_guild_mission_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "guildfight_result")
        {
            guild_pvp_fight_ex((protocol.game.smsg_guild_fight)message.m_object[0], message.m_bools[0]);

        }
        else if (message.m_type == "explore_result")
        {
            explore_result_ex((protocol.game.smsg_huodong_tansuo_event)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "pvp_result")
        {
            pvp_result_ex((protocol.game.smsg_pvp_fight_end)message.m_object[0], message.m_bools[0]);

        }
        else if (message.m_type == "ds_result")
        {
            ds_result_ex((protocol.team.smsg_fight_ds)message.m_object[0], message.m_bools[0]);

        }
        else if (message.m_type == "hbb_result")
        {
            yingjiu_result_ex((protocol.game.smsg_hbb_fight_end)message.m_object[0], message.m_bools[0]);
        }
        else if (message.m_type == "ore_result")
        {
            ore_result_ex((protocol.game.smsg_ore_fight_end)message.m_object[0], message.m_bools[0]);
        }
    }
    void show_result(bool win)
    {
        s_message _mes = new s_message();

        if (win)
        {
            _mes.m_type = "show_result_win";
        }
        else
        {
            _mes.m_type = "show_result_def";
        }
        _mes.time = 1;
        cmessage_center._instance.add_message(_mes);
    }

    void ore_result(bool win, int hp)
    {
        m_win = win;

        m_result_state = "hall";

        s_message _msg = new s_message();

        _msg.m_type = "ore_fight_end";
        _msg.time = 0.1f;

        if (win == true)
        {
            _msg.m_ints.Add((int)0);
        }
        else
        {
            _msg.m_ints.Add((int)1);
        }
        _msg.m_ints.Add(hp);

        cmessage_center._instance.add_message(_msg);

        _msg = new s_message();

        _msg.m_type = "load_scene";
        _msg.m_string.Add((string)sys._instance.m_hall_name);

        Time.timeScale = 1;
        m_result_message = _msg;
        m_wait_round = 0.0f;
    }

    void tu_result(bool win)
    {
        show_result(win);

        m_result_state = "tan_suo";

        m_out_message = new s_message();
        m_out_message.m_type = "ts_fight_end";
        m_out_message.time = 0.1f;

        if (win == true)
        {
            m_out_message.m_ints.Add((int)0);
        }
        else
        {
            m_out_message.m_ints.Add((int)1);
        }
        s_message _msg = new s_message();
        _msg.m_type = "load_scene";
        _msg.m_string.Add((string)"ts_game_explore");
        Time.timeScale = 1;
        m_result_message = _msg;

        m_wait_round = 0.0f;
        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
    }

    void guild_boss_result(bool win, int hp)
    {
        show_result(win);

        m_mission_type = e_mission_type.buttle_mission;
        s_message _message = new s_message();
        _message.m_type = "guild_boss_battle_end";

        if (win)
        {
            _message.m_ints.Add(0);
        }
        else
        {
            _message.m_ints.Add(1);
        }

        _message.m_ints.Add(hp);

        m_result_message = _message;

        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
    }

    void explore_result(bool win, int hp)
    {
        show_result(win);

        m_mission_type = e_mission_type.buttle_mission;

        s_message _message = new s_message();
        _message.m_type = "explore_battle_end";

        if (win)
        {
            _message.m_ints.Add(0);
        }
        else
        {
            _message.m_ints.Add(1);
        }

        _message.m_ints.Add(hp);
        m_result_message = _message;
        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
    }

    void boss_result(bool win, int hp)
    {

        m_win = win;
        m_result_state = "boss";
        s_message _msg = new s_message();
        _msg.m_type = "boss_fight_end";
        _msg.time = 0.1f;

        if (win == true)
        {
            _msg.m_ints.Add((int)0);
        }
        else
        {
            _msg.m_ints.Add((int)1);
        }

        _msg.m_ints.Add(hp);
        cmessage_center._instance.add_message(_msg);
        _msg = new s_message();
        _msg.m_type = "load_scene";
        _msg.m_string.Add((string)"ts_game_boss");
        Time.timeScale = 1;
        m_result_message = _msg;

        m_wait_round = 0.0f;
        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
    }

    void ttt_result(bool win)
    {
        m_win = win;

        m_result_state = "mi_jing";

        s_message _msg = new s_message();
        _msg.m_type = "ttt_fight_end";
        _msg.time = 0.1f;

        if (win == true)
        {
            _msg.m_ints.Add((int)0);
        }
        else
        {
            _msg.m_ints.Add((int)1);
        }

        cmessage_center._instance.add_message(_msg);
        sys._instance.m_game_state = "mi_jing";
        s_message _message = new s_message();

        _message = new s_message();
        _message.m_type = "loaded";
        m_result_message = _message;

        m_wait_round = 0.0f;
        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
        m_defeated_result_0.GetComponent<defeated_Result>().m_des.SetActive(true);
    }

    void guild_boss_ex_result(bool win)
    {
        m_win = win;

        m_result_state = "guild_boss_ex";

        s_message _msg = new s_message();
        _msg.m_type = "guild_boss_fight_ex_end";
        _msg.time = 0.1f;

        if (win == true)
        {
            _msg.m_ints.Add((int)0);
        }
        else
        {
            _msg.m_ints.Add((int)1);
        }
        cmessage_center._instance.add_message(_msg);
        sys._instance.m_game_state = "guild_boss_ex";
        s_message _message = new s_message();
        _message = new s_message();
        _message.m_type = "loaded";
        m_result_message = _message;

        m_wait_round = 0.0f;
        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
        m_defeated_result_0.GetComponent<defeated_Result>().m_des.SetActive(true);
    }

    void treasure_result(bool win)
    {
        m_win = win;

        {
            protocol.game.cmsg_treasure_fight_end _net_msg = new protocol.game.cmsg_treasure_fight_end();
            net_http._instance.send_msg<protocol.game.cmsg_treasure_fight_end>(opclient_t.CMSG_TREASURE_FIGHT_END, _net_msg);
        }

        m_result_state = "bao_wu";

        s_message _msg = new s_message();
        _msg.m_type = "load_scene";
        _msg.m_string.Add("ts_game_duobao");
        Time.timeScale = 1;
        m_result_message = _msg;

        m_wait_round = 0.0f;
        m_reward_type.Clear();
        m_reward_object.Clear();

        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
    }

    void player_result_sound(bool win)
    {
        if (win)
        {
            sys._instance.play_sound("sound/gksl01");
        }
        else
        {
            sys._instance.play_sound("sound/gksb01");
        }
    }

    void sport_result(bool win)
    {
        m_win = win;

        m_result_state = "hall";

        s_message _msg = new s_message();
        _msg.m_type = "sport_fight_end";

        if (win == true)
        {
            _msg.m_ints.Add((int)0);
        }
        else
        {
            _msg.m_ints.Add((int)1);
        }
        cmessage_center._instance.add_message(_msg);

        if (win == true)
        {
            _msg = new s_message();
            _msg.m_type = "load_scene";
            _msg.m_string.Add((string)sys._instance.m_hall_name);
            Time.timeScale = 1;
            s_message _msg1 = new s_message();
            _msg1.m_type = "arena_look";
            _msg.m_next = _msg1;
            m_result_message = _msg;
        }
        else
        {
            _msg = new s_message();
            _msg.m_type = "load_scene";
            _msg.m_string.Add((string)sys._instance.m_hall_name);
            Time.timeScale = 1;
            s_message _msg1 = new s_message();
            _msg1.m_type = "arena_look";
            _msg.m_next = _msg1;
            m_result_message = _msg;
        }
        m_wait_round = 0.0f;
        m_reward_type.Clear();
        m_reward_object.Clear();

        buttle_result _result = m_win_gui.GetComponent<buttle_result>();
        _result.set_reward_num(0);
        _result = m_win_result.GetComponent<buttle_result>();
        _result.set_reward_num(0);
        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
    }

    void yingjiu_result(bool win)
    {
        m_win = win;
        m_result_state = "ying_jiu";

        s_message _msg = new s_message();
        _msg.m_type = "yingjiu_fight_end";
        _msg.time = 0.1f;

        if (win == true)
        {
            _msg.m_ints.Add((int)0);
        }
        else
        {
            _msg.m_ints.Add((int)1);
        }
        cmessage_center._instance.add_message(_msg);

        _msg = new s_message();
        _msg.m_type = "load_scene";
        _msg.m_string.Add((string)"ts_game_hhb");
        Time.timeScale = 1;
        m_result_message = _msg;

        m_wait_round = 0.0f;



        m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
    }

    void ore_result_ex(protocol.game.smsg_ore_fight_end msg, bool win)
    {
        Time.timeScale = 1;
        sys._instance.m_self.m_t_player.ore_last_time = timer.now();
        sys._instance.m_self.m_t_player.ore_finish_num++;
        if (win && m_ore.index == sys._instance.m_self.m_t_player.ore_nindex)
        {
            sys._instance.m_self.m_t_player.ore_nindex++;

        }
        sys._instance.m_self.add_active(1500, 1);
        sys._instance.m_self.sub_att(e_player_attr.player_tili, m_ore.tili);
        Time.timeScale = 1;
        m_wait_round = 0.0f;

        m_win_result.GetComponent<buttle_result>().set_win(win);
        sys._instance.m_game_state = "hall";
        m_win_result.GetComponent<buttle_result>().m_message = new s_message();
        m_win_result.GetComponent<buttle_result>().m_message.m_type = "load_scene";
        m_win_result.GetComponent<buttle_result>().m_message.m_string.Add((string)sys._instance.m_hall_name);
        for (int i = 0; i < msg.types.Count; i++)
        {

            sys._instance.m_self.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i], false, game_data._instance.get_t_language("battle.cs_889_108"));//金币本战斗获得
            m_win_result.GetComponent<buttle_result>().add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i]);

        }

        for (int i = 0; i < msg.equips.Count; i++)
        {
            sys._instance.m_self.add_equip(msg.equips[i], false);
        }
        for (int i = 0; i < msg.treasures.Count; i++)
        {
            sys._instance.m_self.add_treasure(msg.treasures[i], false);
        }
        for (int i = 0; i < msg.roles.Count; i++)
        {
            sys._instance.m_self.add_card(msg.roles[i], false);
        }
        m_win_result.SetActive(true);
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);

            m_win_result.GetComponent<buttle_result>().set_tip(_text.fail_type, _text.fail_param);

            return;
        }
    }

    void yingjiu_result_ex(protocol.game.smsg_hbb_fight_end msg, bool win)
    {
        sys._instance.m_game_state = "ying_jiu";
        Time.timeScale = 1;
        m_wait_round = 0.0f;

        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        sys._instance.m_self.m_t_player.hbb_finish_num++;
        sys._instance.m_self.m_t_player.hbb_refresh_num = 0;
        for (int i = 0; i < msg.types.Count; ++i)
        {
            sys._instance.m_self.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i], false, game_data._instance.get_t_language("battle.cs_928_112"));//营救伙伴战斗获得
            _result.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i]);
        }
        _result.set_win(win);
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);

            m_win_result.GetComponent<buttle_result>().set_tip(_text.fail_type, _text.fail_param);
            _result.m_message = new s_message();
            _result.m_message.m_type = "load_scene";
            _result.m_message.m_string.Add((string)"ts_game_hhb");

            m_win_result.SetActive(true);
            return;
        }

        sys._instance.m_self.add_active(1600, 1);
        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add((string)"ts_game_hhb");
        m_win_result.SetActive(true);
    }

    void ttt_result_ex(protocol.game.smsg_ttt_fight_end msg, bool win)
    {
        sys._instance.m_game_state = "mi_jing";

        Time.timeScale = 1;

        m_wait_round = 0.0f;

        m_win_result.GetComponent<buttle_result>().set_win(win);

        for (int i = 0; i < msg.types.Count; ++i)
        {
            sys._instance.m_self.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i], false, game_data._instance.get_t_language("battle.cs_965_112"));//秘境战斗获得
            if ((msg.types[i] == 1 && msg.value1s[i] == 1))
            {
                m_win_result.GetComponent<buttle_result>().add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.baoji1);

            }
            else if ((msg.types[i] == 1 && msg.value1s[i] == 6))
            {
                m_win_result.GetComponent<buttle_result>().add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.baoji2);

            }
            else
            {
                m_win_result.GetComponent<buttle_result>().add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i]);

            }

        }
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);

            m_win_result.GetComponent<buttle_result>().set_tip(_text.fail_type, _text.fail_param);
            m_win_result.GetComponent<buttle_result>().m_message = new s_message();
            m_win_result.GetComponent<buttle_result>().m_message.m_type = "load_scene";
            m_win_result.GetComponent<buttle_result>().m_message.m_string.Add((string)"ts_game_mijing");
            m_win_result.SetActive(true);
            return;
        }

        buttle_result _result = m_win_result.GetComponent<buttle_result>();

        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add((string)"ts_game_mijing");
        m_win_result.SetActive(true);

    }

    void transport_result_ex(protocol.game.smsg_yb_ybq_fight_end msg, bool win)
    {
        sys._instance.m_game_state = "transport";

        Time.timeScale = 1;

        m_wait_round = 0.0f;

        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        m_win_result.GetComponent<buttle_result>().set_win(win);
        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add((string)"ts_game_transport");
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);

            m_win_result.GetComponent<buttle_result>().set_tip(_text.fail_type, _text.fail_param);

            m_win_result.SetActive(true);
            sys._instance.m_self.m_t_player.ybq_finish_num++;
            sys._instance.m_self.m_t_player.ybq_last_time = timer.now();
            return;
        }
        protocol.game.smsg_yb_ybq_fight_end _msg = msg;
        for (int i = 0; i < _msg.types.Count; i++)
        {
            _result.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i], win);

            sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i], false, game_data._instance.get_t_language("battle.cs_1034_116"));//运输货船

        }
        _result.set_win(win);
        m_win_result.SetActive(true);

        sys._instance.m_self.m_t_player.ybq_finish_num++;
        sys._instance.m_self.m_t_player.ybq_last_time = timer.now();
    }

    void qiyu_result_ex(protocol.game.smsg_qiyu_fight_end msg, bool win)
    {
        Time.timeScale = 1;
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);

        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            m_win_result.GetComponent<buttle_result>().set_tip(_text.fail_type, _text.fail_param);
            m_win_result.SetActive(true);
            return;
        }

        for (int i = 0; i < msg.types.Count; i++)
        {
            _result.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i]);
            sys._instance.m_self.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i], false, game_data._instance.get_t_language("battle.cs_1060_108"));//奇遇挑战
        }
        for (int i = 0; i < msg.equips.Count; i++)
        {
            sys._instance.m_self.add_equip(msg.equips[i], false);
        }
        for (int i = 0; i < msg.roles.Count; i++)
        {
            sys._instance.m_self.m_roles.Add(msg.roles[i]);
        }
        for (int i = 0; i < msg.treasures.Count; i++)
        {
            sys._instance.m_self.add_treasure(msg.treasures[i], false);
        }
        _result.set_win(win);
        m_win_result.SetActive(true);
    }

    void boss_result_ex(protocol.game.smsg_boss_fight_end msg, bool win)
    {
        sys._instance.m_game_state = "boss";

        Time.timeScale = 1;
        m_wait_round = 0.0f;
        sys._instance.m_self.add_att(e_player_attr.player_medal_point, msg.medal + msg.hit_medal, false);
        protocol.game.smsg_boss_fight_end _msg = msg;
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);

        if (win)
        {
            _result.set_reward_num(0);

            _result.add_reward(1, 13, msg.medal, msg.baoji);
            _result.add_reward(1, 100, _msg.hit, 0);
            _result.add_reward(1, 102, msg.hit_medal, 0);
            if (msg.bmax_rank != msg.amax_rank)
            {
                int m = msg.bmax_rank;
                if (m == 0)
                {
                    m = 201;
                }
                m = m - msg.amax_rank;
                _result.add_reward(1, 103, msg.amax_rank, m);
            }
            if (msg.btop_rank != msg.atop_rank)
            {
                int m = msg.btop_rank;
                if (m == 0)
                {
                    m = 201;
                }
                m = m - msg.atop_rank;
                _result.add_reward(1, 104, msg.atop_rank, m);
            }
            m_win_result.SetActive(true);
        }
        else
        {
            _result.set_reward_num(0);
            _result.add_reward(1, 100, msg.hit, 0);
            _result.add_reward(1, 13, msg.medal, msg.baoji);
            if (msg.bmax_rank != msg.amax_rank)
            {
                int m = msg.bmax_rank;
                if (m == 0)
                {
                    m = 201;
                }
                m = m - msg.amax_rank;
                _result.add_reward(1, 103, msg.amax_rank, m);
            }
            if (msg.btop_rank != msg.atop_rank)
            {
                int m = msg.btop_rank;
                if (m == 0)
                {
                    m = 201;
                }
                m = m - msg.atop_rank;
                _result.add_reward(1, 104, msg.atop_rank, m);
            }
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            _result.set_win(win);
            m_win_result.SetActive(true);
        }

        sys._instance.m_self.add_active(1000, 1);
        sys._instance.m_self.m_t_player.boss_task_num++;
        sys._instance.m_self.check_target_done();
        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add((string)"ts_game_boss");
    }

    void sport_result_ex(protocol.game.smsg_sport_fight_end msg, bool win)
    {
        m_result_state = "hall";
        Time.timeScale = 1;
        protocol.game.smsg_sport_fight_end _msg_sport = msg;
        sys._instance.m_self.add_att(e_player_attr.player_gold, _msg_sport.pgold, false, game_data._instance.get_t_language("battle.cs_1167_87"));//竞技场战斗后获得
        sys._instance.m_self.sub_att(e_player_attr.player_treasure_energy, 2);
        for (int i = 0; i < _msg_sport.types.Count; i++)
        {
            sys._instance.m_self.add_reward(_msg_sport.types[i], _msg_sport.value1s[i], _msg_sport.value2s[i], _msg_sport.value3s[i], false, game_data._instance.get_t_language("battle.cs_1167_87"));//竞技场战斗后获得
        }
        for (int i = 0; i < msg.equips.Count; i++)
        {
            sys._instance.m_self.add_equip(msg.equips[i], false);
        }
        for (int i = 0; i < msg.roles.Count; i++)
        {
            sys._instance.m_self.add_card(msg.roles[i], false);
        }
        sys._instance.m_self.m_t_player.jj_task_num++;
        sys._instance.m_self.add_active(900, 1);
        sys._instance.m_self.check_target_done();
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            for (int i = 0; i < _msg_sport.types.Count; i++)
            {
                _result.add_reward(_msg_sport.types[i], _msg_sport.value1s[i], _msg_sport.value2s[i], _msg_sport.value3s[i], win);
            }
            m_win_result.SetActive(true);
            return;
        }
        sys._instance.m_self.zs_skill_target(3, 1);

        if (_msg_sport.max_rank < sys._instance.m_self.m_t_player.max_rank)
        {
            root_gui._instance.show_arena_max(_msg_sport.max_rank, sys._instance.m_self.m_t_player.max_rank);
        }
        sys._instance.m_self.m_t_player.max_rank = _msg_sport.max_rank;
        if (_msg_sport.result > 0)
        {
            s_t_sport_card _card = game_data._instance.get_t_sport_card(_msg_sport.cid);
            if (_msg_sport.pgold <= 0)
            {
                sys._instance.m_self.add_reward(_card.type, _card.value1, _card.value2, _card.value3, false, game_data._instance.get_t_language("battle.cs_1167_87"));//竞技场战斗后获得
            }
            s_message _msg_arena = new s_message();
            _msg_arena.m_type = "set_arena_end_gui";
            List<int> ids = new List<int>();
            ids.Add(_msg_sport.cid);
            List<int> ids1 = new List<int>();
            for (int i = 0; i < game_data._instance.m_dbc_sport_card.get_y(); i++)
            {
                int id = int.Parse(game_data._instance.m_dbc_sport_card.get(0, i));
                s_t_sport_card card = game_data._instance.get_t_sport_card(id);
                if (sys._instance.m_self.m_t_player.level >= card.level1 && sys._instance.m_self.m_t_player.level <= card.level2)
                {

                    if (sys._instance.is_hide_reward(card.type, card.value1))
                    {
                        continue;
                    }
                    ids1.Add(id);

                }
            }

            int id2 = Random.Range(0, ids1.Count);
            ids.Add(ids1[id2]);
            ids1.Remove(ids1[id2]);
            id2 = Random.Range(0, ids1.Count);
            ids.Add(ids1[id2]);
            for (int i = 0; i < ids.Count; i++)
            {
                _msg_arena.m_ints.Add(ids[i]);
            }
            _msg_arena.m_ints.Add(0);
            _msg_arena.m_ints.Add(_msg_sport.pgold);
            _msg_arena.m_string.Add(sys._instance.m_hall_name);
            _result.m_message = new s_message();
            _result.m_message.m_type = "show_arena_end_gui";
            cmessage_center._instance.add_message(_msg_arena);
        }

        for (int i = 0; i < _msg_sport.types.Count; i++)
        {
            _result.add_reward(_msg_sport.types[i], _msg_sport.value1s[i], _msg_sport.value2s[i], _msg_sport.value3s[i], win);
        }
        m_win_result.SetActive(true);
    }

    void pvp_result_ex(protocol.game.smsg_pvp_fight_end msg, bool win)
    {
        sys._instance.m_game_state = "pvp";
        Time.timeScale = 1;
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);
        if (win)
        {
            sys._instance.m_self.m_t_player.pvp_hit++;
        }
        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add((string)"ts_game_lieren");

        sys._instance.m_self.m_t_player.pvp_total += msg.pvp_point;

        for (int i = 0; i < msg.types.Count; i++)
        {
            _result.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i], win);
        }
        _result.add_reward(1, 23, msg.pvp_point, 0, win);
        for (int i = 0; i < msg.types.Count; i++)
        {
            sys._instance.m_self.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i], false, game_data._instance.get_t_language("battle.cs_1283_112"));//猎人大会战斗
        }
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            m_win_result.SetActive(true);
            return;
        }

        Time.timeScale = 1;
        m_wait_round = 0.0f;
        m_reward_type.Clear();
        m_reward_object.Clear();

        /// 结算图标
        m_win_result.SetActive(true);
    }

    void ds_result_ex(protocol.team.smsg_fight_ds msg, bool win)
    {
        m_timer_cd = msg.cd_time;
        sys._instance.m_game_state = "master";
        Time.timeScale = 1;
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);
        if (pre_duanwei != cur_duanwei)
        {
            _result.m_message = new s_message();
            _result.m_message.m_type = "show_master_levelup_gui";
            _result.m_message.m_ints.Add(pre_duanwei);
            _result.m_message.m_ints.Add(cur_duanwei);

            s_message nextmsg = new s_message();
            nextmsg.m_type = "load_scene";
            nextmsg.m_string.Add((string)"ts_game_master");
            _result.m_message.m_next = nextmsg;
        }
        else
        {
            _result.m_message = new s_message();
            _result.m_message.m_type = "load_scene";
            _result.m_message.m_string.Add((string)"ts_game_master");

        }
        _result.add_reward(1, 27, m_ds_fight_end.xinpian, 0, win);
        if (m_ds_fight_end.ciliao > 0)
        {
            _result.add_reward(2, 110020001, m_ds_fight_end.ciliao, 0);
        }

        {
            _result.add_reward(1, 106, sys._instance.m_self.m_t_player.ds_point, m_ds_fight_end.point, win);

        }
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            m_win_result.SetActive(true);
            return;
        }

        Time.timeScale = 1;

        m_wait_round = 0.0f;
        m_reward_type.Clear();
        m_reward_object.Clear();

        /// 结算图标
        m_win_result.SetActive(true);

    }

    void guild_pvp_fight_ex(protocol.game.smsg_guild_fight msg, bool win)
    {
        sys._instance.m_game_state = "guild_fight_pvp";
        Time.timeScale = 1;
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);
        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add((string)"ts_game_guildwar");
        _result.add_reward(1, 10, msg.gongxian, 0);
        _result.add_reward(1, 107, msg.zhanji, 0);
        _result.add_reward(1, 108, msg.guard_point, 0);
        sys._instance.m_self.add_reward(1, 10, msg.gongxian, 0, false, game_data._instance.get_t_language("battle.cs_1390_66"));//跨服军团战战斗获得
        sys._instance.m_self.m_t_player.guild_pvp_num--;
        protocol.game.smsg_guild_fight_pvp_look look_msg = juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().look_msg;
        look_msg.fight.zhanji += msg.zhanji;
        m_guildfightinfo.guard_points[m_guildfightindex / 7] += msg.guard_point;
        look_msg.fight.jidi = msg.jidi;
        look_msg.fight.judian = msg.judian;
        look_msg.fight.perfect = msg.perfect;
        if (msg.result >= 2)
        {
            m_guildfightinfo.guard_gongpo[m_guildfightindex / 7] = 1;
        }
        if (msg.result > 0)
        {
            m_guildfightinfo.target_defense_nums[m_guildfightindex]++;
        }
        if (msg.result <= 0)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            _result.set_win(false);
            m_win_result.SetActive(true);
            return;
        }
        _result.set_win(true);

        Time.timeScale = 1;
        m_wait_round = 0.0f;
        m_reward_type.Clear();
        m_reward_object.Clear();

        /// 结算图标
        s_message _mes = new s_message();
        _mes.m_type = "hide_guild_guttle_gui";
        cmessage_center._instance.add_message(_mes);
        m_win_result.SetActive(true);

    }

    void guild_boss_result_ex(protocol.game.smsg_guild_mission_fight_end msg, bool win)
    {
        sys._instance.m_game_state = "guild_boss_ex";
        Time.timeScale = 1;
        sys._instance.m_self.m_t_player.contribution += msg.contri + msg.hit_contri;
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);
        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add((string)"ts_game_guildfb");
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            _result.add_reward(1, 10, msg.contri, 0);
            _result.add_reward(1, 100, (long)msg.hit, 0);
            m_win_result.SetActive(true);
            return;
        }
        Time.timeScale = 1;

        m_wait_round = 0.0f;
        m_reward_type.Clear();
        m_reward_object.Clear();

        /// 结算图标
        _result.add_reward(1, 10, msg.contri, 0);
        _result.add_reward(1, 100, (long)msg.hit, 0);
        _result.add_reward(1, 105, msg.hit_contri, 0);
        s_message _mes = new s_message();
        _mes.m_type = "hide_guild_guttle_gui";
        cmessage_center._instance.add_message(_mes);
        m_win_result.SetActive(true);
    }

    void explore_result_ex(protocol.game.smsg_huodong_tansuo_event msg, bool win)
    {
        sys._instance.m_game_state = "explore_ex";
        Time.timeScale = 1;
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);
        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene2";
        _result.m_message.m_string.Add((string)"ts_game_explore");
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            //_result.add_reward(msg.types[0],msg.value1s[0],msg.value2s[0],msg.value3s[0]);
            m_win_result.SetActive(true);
            return;
        }
        Time.timeScale = 1;
        m_wait_round = 0.0f;
        m_reward_type.Clear();
        m_reward_object.Clear();

        /// 结算图标
        _result.add_reward(msg.types[0], msg.value1s[0], msg.value2s[0], msg.value3s[0]);
        s_message _message = new s_message();
        _message.m_type = "explore_create_floor2";
        cmessage_center._instance.add_message(_message);
        m_win_result.SetActive(true);
    }

    void treasure_result_ex(protocol.game.smsg_treasure_fight_end msg, bool win)
    {
        m_result_state = "bao_wu";

        Time.timeScale = 1;

        m_wait_round = 0.0f;

        m_reward_type.Clear();
        m_reward_object.Clear();
        sys._instance.m_self.sub_att(e_player_attr.player_treasure_energy, 2);
        sys._instance.m_self.add_att(e_player_attr.player_gold, msg.pgold, false, game_data._instance.get_t_language("battle.cs_1447_80"));//夺宝奇兵战斗后获得
        sys._instance.m_self.zs_skill_target(4, 1);
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        for (int i = 0; i < msg.types.Count; i++)
        {
            sys._instance.m_self.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i], false, game_data._instance.get_t_language("battle.cs_1447_80"));//夺宝奇兵战斗后获得
        }
        for (int i = 0; i < msg.equips.Count; i++)
        {
            sys._instance.m_self.add_equip(msg.equips[i], false);
        }
        for (int i = 0; i < msg.roles.Count; i++)
        {
            sys._instance.m_self.add_card(msg.roles[i], false);
        }
        if (msg.suipian_id > 0)
        {
            sys._instance.m_self.add_item((uint)msg.suipian_id, 1, false, game_data._instance.get_t_language("battle.cs_1447_80"));//夺宝奇兵战斗后获得
        }

        /// 结算图标
        {
            _result.set_reward_num(0);
            for (int i = 0; i < msg.types.Count; i++)
            {
                _result.add_reward(msg.types[i], msg.value1s[i], msg.value2s[i], msg.value3s[i], win);
            }
            _result.add_reward(2, msg.suipian_id, 1, 0);
            _result.set_win(win);
            if (win)
            {

            }
            else
            {
                protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
                _result.set_tip(_text.fail_type, _text.fail_param);
            }
            m_win_result.SetActive(true);
        }

        s_t_sport_card _card = game_data._instance.get_t_sport_card(msg.card);
        if (msg.pgold <= 0)
        {
            sys._instance.m_self.add_reward(_card.type, _card.value1, _card.value2, _card.value3, false, game_data._instance.get_t_language("battle.cs_1447_80"));//夺宝奇兵战斗后获得
        }

        s_message _msg_arena = new s_message();
        _msg_arena.m_type = "set_arena_end_gui";
        List<int> ids = new List<int>();
        ids.Add(msg.card);
        List<int> ids1 = new List<int>();
        for (int i = 0; i < game_data._instance.m_dbc_sport_card.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_sport_card.get(0, i));
            s_t_sport_card card = game_data._instance.get_t_sport_card(id);
            if (sys._instance.m_self.m_t_player.level >= card.level1 && sys._instance.m_self.m_t_player.level <= card.level2)
            {
                if (sys._instance.is_hide_reward(card.type, card.value1))
                {
                    continue;
                }
                ids1.Add(id);
            }
        }
        int id2 = Random.Range(0, ids1.Count);
        ids.Add(ids1[id2]);
        ids1.Remove(ids1[id2]);
        id2 = Random.Range(0, ids1.Count);
        ids.Add(ids1[id2]);
        for (int i = 0; i < ids.Count; i++)
        {
            _msg_arena.m_ints.Add(ids[i]);
        }
        _msg_arena.m_ints.Add(0);
        _msg_arena.m_ints.Add(msg.pgold);
        _msg_arena.m_string.Add("ts_game_duobao");
        _result.m_message = new s_message();
        _result.m_message.m_type = "show_arena_end_gui";
        s_message _msg = new s_message();
        _msg.m_type = "treasure_result1";
        _msg.m_object.Add(msg);
        _result.m_message = new s_message();
        _result.m_message.m_type = "show_arena_end_gui";
        cmessage_center._instance.add_message(_msg);
        cmessage_center._instance.add_message(_msg_arena);
    }

    void mission_result_ex(protocol.game.smsg_mission_fight_end msg, bool win)
    {
        Time.timeScale = 1;
        m_result_state = "hall";
        m_result_message = new s_message();
        m_result_message.m_next = new s_message();
        buttle_result _result = m_win_result_2.GetComponent<buttle_result>();
        _result.set_win(win);
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            m_win_result_2.SetActive(true);
            return;
        }
        protocol.game.smsg_mission_fight_end _msg = msg;
        int _num = 0;
        // 解锁id
        if (m_t_mission.index_id > sys._instance.m_self.m_t_player.mission)
        {
            sys._instance.m_self.m_t_player.mission = m_t_mission.index_id;
        }
        if (m_t_mission.jyindex_id > sys._instance.m_self.m_t_player.mission_jy)
        {
            sys._instance.m_self.m_t_player.mission_jy = m_t_mission.jyindex_id;
        }
        sys._instance.m_self.sub_att(e_player_attr.player_tili, m_t_mission.tili);
        for (int i = 0; i < _msg.types.Count; i++)
        {
            sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i], false, m_t_mission.name + game_data._instance.get_t_language("battle.cs_1567_126"));//关卡战斗获得
            _num++;
        }
        for (int i = 0; i < _msg.equips.Count; i++)
        {
            sys._instance.m_self.add_equip(_msg.equips[i], false);
        }
        for (int i = 0; i < _msg.roles.Count; i++)
        {
            sys._instance.m_self.add_card(_msg.roles[i], false);
        }
        _result.set_reward_num(_num);
        for (int i = 0; i < _msg.types.Count; i++)
        {
            _result.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i], win);
        }
        _result.set_star(_msg.star);

        // 副本次数
        if (m_t_mission.day_num > 0)
        {
            sys._instance.m_self.add_mission_cishu(m_t_mission.id, 1);
        }

        // 副本星星
        int star_add = sys._instance.m_self.add_mission_star(m_t_mission.id, _msg.star);

        // 地图星星
        if (star_add > 0)
        {
            if (m_t_mission.type != 3)
            {
                sys._instance.m_self.add_map_star(m_t_mission.map_id, star_add);
            }
        }

        // 目标，日常
        if (m_t_mission.type == 1)
        {
            sys._instance.m_self.m_t_player.pt_task_num++;
            sys._instance.m_self.add_active(400, 1);
            sys._instance.m_self.check_target_done();
            sys._instance.m_self.zs_skill_target(1, 1);
        }
        else if (m_t_mission.type == 2)
        {
            sys._instance.m_self.m_t_player.jy_task_num++;
            sys._instance.m_self.add_active(500, 1);
            sys._instance.m_self.check_target_done();
            sys._instance.m_self.zs_skill_target(2, 1);
        }

        Time.timeScale = 1;
        m_win_result_2.SetActive(true);

        if (game_data._instance.m_guaji > 0)
        {
            sys._instance.m_game_state = "hall";
            s_message msg1 = new s_message();
            msg1.m_type = "load_scene";
            msg1.m_string.Add((string)sys._instance.m_hall_name);
            cmessage_center._instance.add_message(msg1);
        }
    }

    void qiecuo_result_ex(protocol.game.smsg_qiecuo msg, bool win)
    {
        Time.timeScale = 1;
        m_result_state = m_last_state;
        sys._instance.m_game_state = m_last_state;
        buttle_result _result = m_win_result.GetComponent<buttle_result>();
        _result.set_win(win);
        _result.m_message = new s_message();
        _result.m_message.m_type = "load_scene";
        _result.m_message.m_string.Add(m_last_scence);
        if (win == false)
        {
            protocol.game.msg_fight_text _text = net_http._instance.parse_packet<protocol.game.msg_fight_text>(msg.text);
            _result.set_tip(_text.fail_type, _text.fail_param);
            m_win_result.SetActive(true);
            return;
        }
        m_win_result.SetActive(true);

    }

    void result_show(bool win)
    {
        int _camp = 1;

        if (win)
        {
            _camp = 0;
            sys._instance.play_sound("sound/gksl01");
        }
        else
        {
            sys._instance.play_sound("sound/gksb01");
        }

        for (int i = 0; i < m_units.Count; i++)
        {
            if (m_units[i] != null && m_units[i].GetComponent<unit>().m_camp == _camp)
            {
                m_units[i].GetComponent<unit>().action("win");
            }
        }
    }

    void mission_result(bool win, s_t_mission mission, int star)
    {
        Time.timeScale = 1;

        if (m_mission_type == e_mission_type.buttle_mission_bz)
        {
            show_result(win);
            m_mission_type = e_mission_type.buttle_mission;
            s_message _message = new s_message();
            _message.m_type = "bz_battle_end";
            m_result_message = _message;
            m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
            return;
        }
        else if (m_mission_type == e_mission_type.buttle_mission_guild)
        {
            show_result(win);
            m_mission_type = e_mission_type.buttle_mission;
            s_message _message = new s_message();
            _message.m_type = "guild_battle_end";
            if (win)
            {
                _message.m_ints.Add(0);
            }
            else
            {
                _message.m_ints.Add(1);
            }
            m_result_message = _message;
            m_defeated_result_0.GetComponent<defeated_Result>().set_tip(false);
            return;
        }

        m_result_state = "hall";
        m_result_message = new s_message();
        m_result_message.m_type = "load_scene";
        m_result_message.m_string.Add((string)sys._instance.m_hall_name);
        m_result_message.m_next = new s_message();

        m_wait_round = 0.0f;

        if (star < 1)
        {
            star = 1;
        }

        if (win == true)
        {
            m_t_mission = mission;

            sys._instance.play_sound("sound/gksl01");

        }
        else
        {
            s_message _msg = new s_message();

            _msg.m_type = "defeated_result";
            _msg.time = 1.0f;
            cmessage_center._instance.add_message(_msg);

            sys._instance.play_sound("sound/gksb01");

            m_defeated_result_0.GetComponent<defeated_Result>().set_tip(true);
            m_defeated_result_0.GetComponent<defeated_Result>().m_des.SetActive(false);
        }
    }

    public void set_battle_index(int index, int max)
    {
        if (m_t_mission != null)
        {
            string _bb = "bb_" + sys._instance.m_self.m_t_player.mission + "_" + index;

            root_gui._instance.action_guide(_bb);

            _bb = "mbb_" + m_t_mission.id + "_" + index;
            root_gui._instance.action_guide(_bb);
        }

        m_ui_buttle.transform.Find("Label").GetComponent<UILabel>().text = index + "/" + max.ToString();
        m_ui_buttle.transform.GetComponent<Animator>().Play("scale_anim_0", 0, 0);
    }

    public void set_round_index(int index, int max)
    {
        if (battle_logic_ex._instance.m_buttle_message.Count > 0 && index > 0)
        {
            if (index == 1)
            {
                m_ficht_time = 1f;
            }
            else
            {
                m_ficht_time = 0.2f;
            }

            UILabel _lv = m_ui_ficht.transform.Find("lv").GetComponent<UILabel>();

            if (index == 1)
            {
                _lv.text = "LV1 START";
            }
            else if (index == 2)
            {
                _lv.text = "LV2 START";

            }
            else if (index == 3)
            {
                _lv.text = "LV3 START";

            }
        }
        else
        {
            index = 1;
        }

        if (m_t_mission != null && m_t_mission.lock_id == sys._instance.m_self.m_t_player.mission)
        {
            string _br = "br_" + sys._instance.m_self.m_t_player.mission + "_" + index;
            root_gui._instance.action_guide(_br);

            _br = "ccr_" + sys._instance.m_self.m_t_player.mission + "_" + index;
            root_gui._instance.action_guide(_br);
        }

        if (m_t_mission != null)
        {
            string _br = "mbr_" + m_t_mission.id + "_" + index;
            root_gui._instance.action_guide(_br);
        }

        if (m_ui_round != null)
        {
            m_ui_round.transform.Find("Label").GetComponent<UILabel>().text = index + "/" + max;
            m_ui_round.transform.GetComponent<Animator>().Play("scale_anim_0", 0, 0);
        }
    }
    public GameObject add_unit(int id, int duiwei, int site, double max_hp, double cur_hp, int cur_mp, int glevel, int jlevel, int pinzhi, int dress, int guanghuan, Vector3 pos, int flag)
    {
        GameObject _unit = get_unit_site(site);

        if (_unit != null)
        {
            if (site != 100 && site != 101 && _unit.GetComponent<unit>().m_t_class.id != id)
            {
                remove_unit(site);
            }
            else if (site == 100 && site == 101 && _unit.GetComponent<unit>().m_t_pet.id != id)
            {
                remove_unit(site);
            }
            else
            {
                _unit.transform.localPosition = pos;
                _unit.GetComponent<unit>().m_start_pos = pos;
                _unit.GetComponent<unit>().m_max_hp = max_hp;
                _unit.GetComponent<unit>().m_cur_hp = cur_hp;
                _unit.GetComponent<unit>().m_cur_mp = cur_mp;

                return _unit;
            }
        }

        int _camp = 1;
        if (site <= 5 || site == 100)
        {
            _camp = 0;
        }

        GameObject _ins = null;
        if (site != 100 && site != 101)
        {
            _ins = sys._instance.create_class(id, dress, guanghuan);
        }
        else
        {
            _ins = sys._instance.create_pet(id, 0);
        }
        _ins.transform.GetComponent<unit>().m_site = site;
        _ins.transform.GetComponent<unit>().m_camp = _camp;
        _ins.transform.GetComponent<unit>().show_pet = flag;
        _ins.transform.GetComponent<unit>().m_end_battle = false;
        _ins.transform.GetComponent<unit>().m_battle_end_delay = 0.0f;
        _ins.transform.GetComponent<unit>().m_max_hp = max_hp;
        _ins.transform.GetComponent<unit>().m_cur_hp = cur_hp;
        _ins.transform.GetComponent<unit>().m_cur_mp = cur_mp;
        _ins.transform.GetComponent<unit>().m_glevel = glevel;
        _ins.transform.GetComponent<unit>().m_jlevel = jlevel;
        _ins.transform.GetComponent<unit>().m_pinzhi = pinzhi;
        _ins.transform.localPosition = pos;
        _ins.GetComponent<unit>().m_start_pos = pos;
        if (id == 999)
        {
            Vector3 _scale = _ins.transform.localScale * 1.35f;
            _ins.transform.localScale = _scale;
            _ins.transform.GetComponent<unit>().m_scale = 1.3f;
        }

        if (id == 998)
        {
            Vector3 _scale = _ins.transform.localScale * 1.35f;
            _ins.transform.localScale = _scale;
            _ins.transform.GetComponent<unit>().m_scale = 1.3f;
        }
        if (site != 100 && site != 101)
        {
            _ins.transform.GetComponent<unit>().create_mini_pro();
        }
        _ins.SetActive(true);
        if (_camp > 0)
        {
            _ins.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            _ins.transform.localEulerAngles = new Vector3(0, 0, 0);
        }
        m_units.Add(_ins);
        return _ins;
    }
    public void remove_unit(int site)
    {
        for (int i = 0; i < m_units.Count; i++)
        {
            if (m_units[i].GetComponent<unit>().m_site == site)
            {
                m_units[i].GetComponent<unit>().m_die = true;
                m_units.RemoveAt(i);
                i = 0;
            }
        }
    }
    public GameObject get_unit_site(int site)
    {
        for (int i = 0; i < m_units.Count; i++)
        {
            if (m_units[i].GetComponent<unit>().m_site == site)
            {
                return m_units[i];
            }
        }

        return null;
    }
    void set_unit_pos(int site, Vector3 pos)
    {
        GameObject _obj = get_unit_site(site);

        _obj.transform.localPosition = pos;
        _obj.GetComponent<unit>().m_start_pos = pos;
    }

    void set_unit_rot(int site, Vector3 rot)
    {
        GameObject _obj = get_unit_site(site);
        _obj.transform.localEulerAngles = rot;
    }

    void show_win(bool win)
    {
        int _camp = 1;
        if (win)
        {
            _camp = 0;
        }

        for (int i = 0; i < m_units.Count; i++)
        {
            if (m_units[i] != null && m_units[i].GetComponent<unit>().m_camp == _camp)
            {
                m_units[i].GetComponent<unit>().action("win");
            }
        }
    }

    bool is_buttle_action_end()
    {
        for (int i = 0; i < m_units.Count; i++)
        {
            GameObject _object = m_units[i];

            if (_object.GetComponent<unit>().m_battle_end_delay > 0)
            {
                return false;
            }
        }
        return true;
    }


    void start_battle_logic_ex()
    {
        s_message _msg = new s_message();

        _msg = new s_message();
        _msg.m_type = "battle_logic";
        cmessage_center._instance.add_message(_msg);

    }

    void battle_message()
    {
        string _message_type = "";

        while (battle_logic_ex._instance.m_buttle_message.Count > 0)
        {
            s_buttle_message _message = battle_logic_ex._instance.m_buttle_message[0];

            if (_message_type == "")
            {
                _message_type = _message.type;
            }
            else if (_message_type != _message.type)
            {
                _message_type = _message.type;

                if (m_wait_round > 0.0f)
                {
                    break;
                }
            }

            if (_message.type == "play_move")
            {
                m_play_move = (string)_message.strings[0];
            }
            else if (_message.type == "round_end")
            {

            }
            else if (_message.type == "fu_chou")
            {
                GameObject _unit_obj = get_unit_site(_message.m_site);

                _unit_obj.GetComponent<unit>().show_att_modify_effect("fc_zdword", 1, "ex");
                sys._instance.play_sound("sound/attr_up");

                m_wait_round = 1.0f;
            }
            else if (_message.type == "pr_guide")
            {
                m_round_end = true;
                m_wait_round = 1.0f;
            }
            else if (_message.type == "st_guide")
            {
                root_gui._instance.action_guide(_message.strings[0]);
            }
            else if (_message.type == "xi_sheng")
            {
                for (int i = 0; i < _message.ints.Count; i++)
                {
                    int _site = _message.ints[i];
                    double _add = _message.doubles[i * 2];
                    double _cur = _message.doubles[i * 2 + 1];

                    GameObject _unit_obj = get_unit_site(_site);
                    _unit_obj.GetComponent<unit>().show_attack_num(1, _add, game_data._instance.get_t_language("xs_zdword"));//爆炸 Explosion
                    _unit_obj.GetComponent<unit>().show_att_modify_effect("xs_zdword", 0, "ex");
                    _unit_obj.GetComponent<unit>().m_cur_hp = _cur;
                    sys._instance.show_effect(_unit_obj.GetComponent<unit>().get_accept_pos(), "effect/ef_zzsj01", 2);
                    sys._instance.play_sound("sound/blood");
                }
                m_wait_round = 0.2f;
            }
            else if (_message.type == "fen_xie")
            {
                int sid = _message.ints[0];
                for (int i = 0; i < _message.ints.Count - 1; i++)
                {
                    int _site = _message.ints[i + 1];
                    double _add = _message.doubles[i * 2];
                    double _cur = _message.doubles[i * 2 + 1];

                    GameObject _unit_obj = get_unit_site(_site);
                    _unit_obj.GetComponent<unit>().show_attack_num(3, _add, "fx");
                    _unit_obj.GetComponent<unit>().m_cur_hp = _cur;
                    _unit_obj.GetComponent<unit>().show_att_modify_effect("fx_zdword", 1, "ex");
                    sys._instance.play_sound("sound/heal");

                    if (sid > 0)
                    {
                        s_t_skill t_skill = game_data._instance.get_t_skill(sid);
                        _unit_obj.GetComponent<unit>().show_zskill(t_skill.name, t_skill.icon);
                    }
                }

                m_wait_round = 0.2f;
            }
            else if (_message.type == "round_end_add_hp")
            {
                GameObject _unit_obj = get_unit_site(_message.m_site);

                for (int i = 0; i < _message.ints.Count; i++)
                {
                    int _site = _message.ints[i];
                    double _add = _message.doubles[i * 2];
                    double _cur = _message.doubles[i * 2 + 1];

                    _unit_obj = get_unit_site(_site);
                    _unit_obj.GetComponent<unit>().show_attack_num(3, _add, "");
                    _unit_obj.GetComponent<unit>().m_cur_hp = _cur;
                    sys._instance.play_sound("sound/heal");
                }

                m_wait_round = 0.2f;
            }
            else if (_message.type == "round_end_add_nl")
            {
                GameObject _unit_obj = get_unit_site(_message.m_site);

                for (int i = 0; i < _message.ints.Count / 3; i++)
                {
                    int _site = _message.ints[i * 3];
                    int _add = _message.ints[i * 3 + 1];
                    int _cur = _message.ints[i * 3 + 2];

                    _unit_obj = get_unit_site(_site);
                    _unit_obj.GetComponent<unit>().show_att_modify_effect("nl_word", 1);
                    _unit_obj.GetComponent<unit>().m_cur_mp = _cur;
                    sys._instance.play_sound("sound/attr_up");
                }

                m_wait_round = 0.2f;
            }
            else if (_message.type == "chufa_zskill")
            {
                GameObject _unit_obj = get_unit_site(_message.m_site);
                if (_unit_obj != null)
                {
                    s_t_skill t_skill = game_data._instance.get_t_skill(_message.ints[0]);
                    _unit_obj.GetComponent<unit>().show_zskill(t_skill.name, t_skill.icon);
                }
            }
            else if (_message.type == "hide_pet")
            {
                for (int i = 0; i < m_units.Count; i++)
                {
                    if (m_units[i].GetComponent<unit>().m_site == 100 || m_units[i].GetComponent<unit>().m_site == 101)
                    {
                        m_units[i].GetComponent<unit>().show_pet = 0;
                        m_units[i].GetComponent<unit>().battle_start = (int)_message.ints[0];
                    }
                    else
                    {
                        m_units[i].SetActive(true);
                        m_units[i].GetComponent<unit>().show_pet = 0;
                        m_units[i].GetComponent<unit>().battle_start = (int)_message.ints[0];
                    }
                }
            }
            else if (_message.type == "hide_role")
            {
                if (_message.m_site == 100)
                {
                    for (int i = 0; i < m_units.Count; i++)
                    {
                        m_units[i].GetComponent<unit>().show_pet = 1;
                        m_units[i].GetComponent<unit>().battle_start = 1;
                        int site = m_units[i].GetComponent<unit>().m_site;
                        if (site >= 6)
                        {
                            m_units[i].SetActive(true);
                        }
                    }
                }
                if (_message.m_site == 101)
                {
                    for (int i = 0; i < m_units.Count; i++)
                    {
                        m_units[i].GetComponent<unit>().show_pet = 2;
                        m_units[i].GetComponent<unit>().battle_start = 1;
                        int site = m_units[i].GetComponent<unit>().m_site;
                        if (site < 6)
                        {
                            m_units[i].SetActive(true);
                        }
                    }
                }
            }

            else if (_message.type == "vs_show")
            {
                m_players.Clear();
                m_players.Add((player)_message.values[0]);
                m_players.Add((player)_message.values[1]);

                m_vs_time = 2;
            }
            else if (_message.type == "unit_die")
            {
                remove_unit(_message.m_site);
                m_wait_round = 1.0f;
                m_round_end = true;
            }
            else if (_message.type == "buffer_end")
            {
                GameObject _object = get_unit_site(_message.m_site);

                if (_object != null)
                {
                    _object.GetComponent<unit>().buffer(_message.ints[0], _message.ints[1], false);
                }
            }
            else if (_message.type == "set_buttle_index")
            {
                set_battle_index(_message.ints[0], _message.ints[1]);

                if (sys._instance.m_pause == true)
                {
                    battle_logic_ex._instance.m_buttle_message.RemoveAt(0);
                    return;
                }
            }
            else if (_message.type == "set_round_index")
            {
                int _index = (int)_message.ints[0];

                set_round_index(_message.ints[0], _message.ints[1]);

                s_message _msg = new s_message();
                _msg.m_type = "cam_pos";

                _msg.m_floats.Add(0.0f);
                _msg.m_floats.Add(30.0f);

                cmessage_center._instance.add_message(_msg);
            }
            else if (_message.type == "wait_round")
            {
                m_wait_round = 1.0f;
            }
            else if (_message.type == "add_unit")
            {
                int max_row = 0;
                int count_i = 9;
                int count_d = 2;
                for (int i = 0; i < (int)_message.doubles.Count / count_d; i++)
                {
                    int _index_i = i * count_i;
                    int _index_d = i * count_d;
                    int duiwei = _message.ints[_index_i + 1];
                    int site = _message.ints[_index_i + 2];
                    if (site < 6)
                    {
                        if (duiwei > 11)
                        {
                            max_row = 2;
                        }
                        else if (duiwei > 5)
                        {
                            max_row = 1;
                        }
                    }
                    add_unit(_message.ints[_index_i]
                             , _message.ints[_index_i + 1]
                             , _message.ints[_index_i + 2]
                             , _message.doubles[_index_d]
                             , _message.doubles[_index_d + 1]
                             , _message.ints[_index_i + 3]
                             , _message.ints[_index_i + 4]
                             , _message.ints[_index_i + 5]
                             , _message.ints[_index_i + 6]
                             , _message.ints[_index_i + 7]
                             , _message.ints[_index_i + 8]
                             , (Vector3)_message.values[i], 0);
                }
                if (sys._instance.m_game_state == "buttle")
                {
                    if (m_t_mission != null && m_t_mission.lock_id == sys._instance.m_self.m_t_player.mission)
                    {
                        string _br = "unlock_mission_" + sys._instance.m_self.m_t_player.mission;
                        root_gui._instance.action_guide(_br);
                    }
                }

                s_message _msg = new s_message();
                _msg.m_type = "end_layout";
                _msg.m_ints.Add(max_row);
                _msg.time = 0.1f;
                cmessage_center._instance.add_message(_msg);
            }
            else if (_message.type == "buffer_out")
            {
                GameObject _object = get_unit_site(_message.m_site);

                if (_message.ints[0] == 1)
                {
                    _object.GetComponent<unit>().show_attack_num(_message.ints[0], _message.doubles[0], game_data._instance.get_t_language("bleed_word"), true); //流血
                }
                else if (_message.ints[0] == 2)
                {
                    _object.GetComponent<unit>().show_attack_num(_message.ints[0], _message.doubles[0], game_data._instance.get_t_language("curse_word"), true); //诅咒
                }
                else
                {
                    _object.GetComponent<unit>().show_attack_num(_message.ints[0], _message.doubles[0], "", true);
                }

                _object.GetComponent<unit>().m_cur_hp = _message.doubles[1];

                m_wait_round = 0.5f;
            }
            else if (_message.type == "double_attack")
            {
                m_wait_round = 0.1f;
                m_round_end = true;

                GameObject _object = get_unit_site(_message.m_site);
                unit _unit = _object.GetComponent<unit>();
                List<string> ss = new List<string>();
                ss.Add("jf_zdword");
                sys._instance.show_skill_lable(_unit.get_accept_pos(), ss);
            }
            else if (_message.type == "release_skill")
            {
                s_message _out_message = new s_message();

                _out_message.m_site = _message.m_site;
                _out_message.m_type = "release_skill";
                _out_message.m_object.Add(_message.values[0]);

                GameObject _object = get_unit_site(_message.m_site);
                unit _unit = _object.GetComponent<unit>();

                _unit.m_release_skill = true;

                skill_ex _skill = (skill_ex)_message.values[0];

                _unit.m_cur_mp = _skill.m_mp;

                if (_skill.m_t_skill.action != "attack")
                {
                    Vector3 _pos = _unit.get_accept_pos();
                    s_message _new_msg = new s_message();

                    _new_msg.m_type = "skill_name";

                    _new_msg.m_string.Add(_skill.m_t_skill.name);
                    _new_msg.m_floats.Add(_pos.x);
                    if (_message.m_site == 100 || _message.m_site == 101)
                    {
                        _new_msg.m_floats.Add(_pos.y * 1.5f);
                    }
                    else
                    {
                        _new_msg.m_floats.Add(_pos.y);
                    }
                    _new_msg.m_floats.Add(_pos.z);

                    cmessage_center._instance.add_message(_new_msg);
                }

                _object.GetComponent<unit>().m_battle_end_delay = 0.0f;
                cmessage_center._instance.add_message(_out_message);
                m_wait_round = 0.1f;
            }
            else if (_message.type == "buttle_fu_huo_0")
            {
                m_wait_round = 1.0f;
                m_round_end = true;

                GameObject _object = get_unit_site(_message.m_site);
                _object.GetComponent<unit>().m_fuhuo = true;
            }
            else if (_message.type == "buttle_fu_huo_1")
            {
                m_wait_round = 1.0f;
                m_round_end = true;

                GameObject _object = get_unit_site(_message.m_site);
                unit _unit = _object.GetComponent<unit>();
                _unit.m_target_alpha = 1;
                _unit.m_fuhuo1 = true;
                _unit.m_cur_hp = _message.doubles[0];
                _unit.m_cur_mp = _message.ints[0];
                List<string> ss = new List<string>();
                ss.Add("fh_zdword");
                sys._instance.show_skill_lable(_unit.get_accept_pos(), ss);
                sys._instance.show_effect(_unit.m_start_pos, "effect/npc/pt_ef_fh01", 2.0f);
            }
            else if (_message.type == "buttle_wait")
            {
                m_wait_round = _message.floats[0] * 0.5f;
            }
            else if (_message.type == "sport_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "treasure_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "tu_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "boss_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "qiyu_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "guild_boss_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "guildfight_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "explore_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "pvp_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;

            }
            else if (_message.type == "ore_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "yingjiu_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "mission_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "ttt_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "guild_boss_ex_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "transport_result")
            {
                m_buttle_result = _message;
                m_result_time = 1.0f;
            }
            else if (_message.type == "add_player_mp")
            {
                int _val = sys._instance.m_self.get_att(e_player_attr.player_mp) + _message.ints[0];

                sys._instance.m_self.set_att(e_player_attr.player_mp, _val);
            }
            battle_logic_ex._instance.m_buttle_message.RemoveAt(0);
        }
    }

    void Update()
    {
        if (sys._instance.m_pause == true || sys._instance.m_is_director == true || m_start == false)
        {
            return;
        }

        if (m_vs_time > 0.0f)
        {
            m_vs_time -= Time.deltaTime;

            if (m_vs_time < 0.0f)
            {
                vs_show(m_players[0], m_players[1]);
            }
            return;
        }
        else if (m_ficht_time > 0.0f)
        {
            m_ficht_time -= Time.deltaTime;

            if (m_ficht_time < 0.0f)
            {
                m_ui_ficht.SetActive(true);
                m_ui_ficht.GetComponent<hide_time>().m_time = 2.0f;
                Time.timeScale = sys._instance.get_game_speed();
                m_wait_round = 1.5f;
            }
            return;
        }

        if (m_wait_round > 0.0f)
        {
            m_wait_round -= Time.deltaTime;
            return;
        }

        if (m_result_time > 0 && battle_logic_ex._instance.m_buttle_message.Count == 0 && m_round_end == false)
        {
            if (is_buttle_action_end())
            {
                if (m_play_move != null && m_play_move.Length > 0)
                {
                    root_gui._instance.action_guide(m_play_move);
                    m_play_move = "";
                }
                m_result_time -= Time.deltaTime;
            }

            if (m_result_time <= 0)
            {
                if (m_buttle_result != null)
                {
                    sys._instance.play_mus("");
                    s_message _msg = new s_message();
                    _msg.m_type = "battle_result_0";
                    cmessage_center._instance.add_message(_msg);
                    _msg = new s_message();
                    _msg.m_type = "battle_result_1";
                    _msg.time = 0.5f;
                    cmessage_center._instance.add_message(_msg);
                }
            }
            return;
        }

        if (battle_logic_ex._instance.m_buttle_message.Count > 0 && m_units.Count == 0)
        {
            s_message _msg = new s_message();
            _msg.m_type = "cam_pos";
            _msg.m_floats.Add(0.0f);
            _msg.m_floats.Add(30.0f);
            cmessage_center._instance.add_message(_msg);
        }

        if (battle_logic_ex._instance.m_buttle_message.Count > 0 && m_round_end == false)
        {
            battle_message();
        }

        if (m_wait_round <= 0 && m_round_end == true)
        {
            bool _battle = true;

            for (int i = 0; i < m_units.Count; i++)
            {
                GameObject _object = m_units[i];

                if (_object.GetComponent<unit>().m_battle_end_delay > 0.0f
                   || _object.GetComponent<unit>().m_die == true
                   || _object.GetComponent<unit>().m_fuhuo == true
                   || _object.GetComponent<unit>().m_die_wait > 0
                   || _object.GetComponent<unit>().m_fuhuo_wait > 0
                   || _object.GetComponent<unit>().m_first_wait > 0)
                {
                    _battle = false;
                }
            }

            if (_battle == true)
            {
                m_round_end = false;
            }
        }

        if (battle_logic_ex._instance.m_buttle_message.Count == 0
           && m_wait_round <= 0)
        {
            bool _battle = true;

            for (int i = 0; i < m_units.Count; i++)
            {
                GameObject _object = m_units[i];

                if (_object.GetComponent<unit>().m_battle_end_delay > 0.0f
                   || _object.GetComponent<unit>().m_die == true
                   || _object.GetComponent<unit>().m_fuhuo == true
                   || _object.GetComponent<unit>().m_release_skill == true
                   || _object.GetComponent<unit>().m_first == true)
                {
                    _battle = false;
                }
            }

            if (_battle)
            {
                m_ui_hit.SetActive(false);
                start_battle_logic_ex();
            }
        }
    }
}
