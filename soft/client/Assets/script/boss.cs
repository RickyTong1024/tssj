using UnityEngine;

public class boss : MonoBehaviour, IMessage
{
    public GameObject m_boss_hp;
    public GameObject m_time;
    public GameObject m_look_jl_gui;
    public GameObject m_shop_gui;
    public GameObject m_task_gui;
    public GameObject m_task_effect;
    public GameObject m_hp_num;
    public GameObject m_boss_levl;
    public int count;
    public int m_guwu_type = 0;
    public GameObject m_ltime;
    private long m_boss_max_hp = 0;
    public GameObject m_num_buy;
    int m_yijian_num;
    bool m_yijian_use_item;
    public GameObject m_saodang;

    public GameObject m_buttle;
    public GameObject m_yijian_buttle;

    public UILabel m_time1;
    public UILabel m_time2;
    public UILabel m_lagenaluo;

    public UILabel m_num_mowang;
    public UILabel m_max_hit;
    public UILabel m_max_allhit;
    public UILabel m_attack_num;
    public protocol.game.smsg_boss_look m_msg = new protocol.game.smsg_boss_look();
    protocol.game.smsg_boss_rank m_rank_msg;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void OnEnable()
    {
        InvokeRepeating("check", 20.0f, 20.0f);
        boss_look();
        Time.timeScale = 1;
        if (boss_task_gui.effect())
        {
            m_task_effect.SetActive(true);
        }
        else
        {
            m_task_effect.SetActive(false);
        }
    }

    void OnDisable()
    {
        CancelInvoke("check");
        CancelInvoke("showtime");
    }

    public void click(GameObject obj)
    {
        if (obj.transform.name == "close")
        {
            s_message _message = new s_message();
            _message.m_type = "show_huo_dong";
            _message.m_ints.Add(1);
            _message.m_bools.Add(false);
            cmessage_center._instance.add_message(_message);
            sys._instance.m_game_state = "hall";
            sys._instance.load_scene(sys._instance.m_hall_name);
        }
        else if (obj.transform.name == "buttle")
        {
            buttle();
        }
        else if (obj.name == "yijian_buttle")
        {
            if (timer.dtnow().Hour >= 12 && timer.dtnow().Hour <= 24)
            {
                s_message mes = new s_message();
                mes.m_type = "show_select_num_gui";
                cmessage_center._instance.add_message(mes);
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("boss.cs_144_67"));//魔王未开启
            }
        }
        else if (obj.transform.name == "ph")
        {
            protocol.game.cmsg_common msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_BOSS_RANK, msg);
        }
        else if (obj.transform.name == "shop")
        {
            m_shop_gui.SetActive(true);
        }
        else if (obj.transform.name == "task")
        {
            m_task_gui.SetActive(true);
        }
        else if (obj.transform.name == "buy_yaoqing")
        {
            int item_id = 10010009;
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
                _message.m_ints.Add(100600);
                _message.m_ints.Add(2);
                cmessage_center._instance.add_message(_message);
            }
        }
        else if (obj.transform.name == "duixing")
        {
            s_message _message = new s_message();
            _message.m_type = "show_duixing_gui";
            _message.m_bools.Add(true);
            cmessage_center._instance.add_message(_message);
        }
    }

    void buttle()
    {
        if (timer.dtnow().Hour >= 12 && timer.dtnow().Hour <= 24)
        {
            if (sys._instance.m_self.m_t_player.boss_num <= 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("boss.cs_135_71"));//挑战次数不足
                return;
            }
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_BOSS_FIGHT_END, _msg);
        }
        else
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("boss.cs_144_67"));//魔王未开启
        }
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_BOSS_FIGHT_END)
        {
            protocol.game.smsg_boss_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_boss_fight_end>(message.m_byte);
            sys._instance.m_self.m_t_player.boss_num--;
            battle_logic_ex._instance.set_boss_fight_end(_msg);

            sys._instance.load_scene_ex("ts_fight_boss");
            sys._instance.m_game_state = "buttle";

        }
        if (message.m_opcode == opclient_t.CMSG_BOSS_LOOK || message.m_opcode == opclient_t.CMSG_BOSS_LOOK_EX)
        {
            m_msg = net_http._instance.parse_packet<protocol.game.smsg_boss_look>(message.m_byte);

            m_boss_max_hp = m_msg.max_hp;
            m_boss_hp.GetComponent<UIProgressBar>().value = (float)m_msg.cur_hp / (float)m_msg.max_hp;
            m_boss_hp.GetComponent<UIProgressBar>().ForceUpdate();
            m_hp_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("boss.cs_174_57"), sys._instance.value_to_wan(m_msg.cur_hp) + "/" + sys._instance.value_to_wan(m_msg.max_hp));//生命值 {0}
            m_lagenaluo.text = game_data._instance.get_t_language("boss.cs_175_31");//拉格纳罗斯分身
            m_boss_levl.GetComponent<UILabel>().text = "Lv" + m_msg.level;
            m_look_jl_gui.GetComponent<look_jl_gui>()._msg = m_rank_msg;
            if (m_msg.toprank > 0)
            {
                if (m_msg.myhit > 0)
                {
                    m_max_hit.text = sys._instance.value_to_wan(m_msg.tophit) + " - " + string.Format(game_data._instance.get_t_language("arena.cs_179_81"), m_msg.toprank);//第{0}名
                }
                else
                {
                    m_max_hit.text = string.Format(game_data._instance.get_t_language("boss.cs_195_51"), m_msg.toprank);//无 - 第{0}名
                }
            }
            else
            {
                if (m_msg.tophit > 0)
                {
                    m_max_hit.text = sys._instance.value_to_wan(m_msg.tophit) + " - " + game_data._instance.get_t_language("bingyuan_rank_gui.cs_83_81");//未上榜
                }
                else
                {
                    m_max_hit.text = game_data._instance.get_t_language("boss.cs_206_37");//无 - 未上榜
                }
            }
            m_num_mowang.GetComponent<UILabel>().text = "x" + sys._instance.m_self.get_att(e_player_attr.player_hj);
            if (m_msg.rank > 0)
            {
                if (m_msg.myhit > 0)
                {
                    m_max_allhit.text = sys._instance.value_to_wan(m_msg.myhit) + " - " + string.Format(game_data._instance.get_t_language("arena.cs_179_81"), m_msg.rank);//第{0}名
                }
                else
                {
                    m_max_allhit.text = game_data._instance.get_t_language("boss.cs_218_40") + " - " + string.Format(game_data._instance.get_t_language("arena.cs_179_81"), m_msg.rank);//无//第{0}名
                }
            }
            else
            {
                if (m_msg.myhit > 0)
                {
                    m_max_allhit.text = sys._instance.value_to_wan(m_msg.myhit) + " - " + game_data._instance.get_t_language("bingyuan_rank_gui.cs_83_81");//未上榜
                }
                else
                {
                    m_max_allhit.text = game_data._instance.get_t_language("boss.cs_206_37");//无 - 未上榜
                }
            }
            m_time.GetComponent<UILabel>().text = "";
            m_attack_num.text = sys._instance.m_self.m_t_player.boss_num + "/10";
            m_ltime.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("boss.cs_234_75"), m_msg.level);//第{0}关
            if (m_msg.reward == 1)
            {
                m_task_effect.SetActive(true);
            }
            else
            {
                m_task_effect.SetActive(false);
            }
            InvokeRepeating("showtime", 0.0f, 0.1f);

            if (sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_yijian_boss)
            {
                m_buttle.SetActive(false);
                m_yijian_buttle.SetActive(true);
            }
            else
            {
                m_buttle.SetActive(true);
                m_yijian_buttle.SetActive(false);
            }
        }
        else if (message.m_opcode == opclient_t.CMSG_BOSS_RANK)
        {
            m_rank_msg = net_http._instance.parse_packet<protocol.game.smsg_boss_rank>(message.m_byte);

            m_look_jl_gui.SetActive(true);
            m_look_jl_gui.GetComponent<look_jl_gui>()._msg = m_rank_msg;
            m_look_jl_gui.GetComponent<look_jl_gui>().m_msg = m_msg;
            m_look_jl_gui.GetComponent<look_jl_gui>().reset_hurt(1);
        }
        else if (message.m_opcode == opclient_t.CMSG_BOSS_SAODANG)
        {
            protocol.game.smsg_boss_saodang saodangs = net_http._instance.parse_packet<protocol.game.smsg_boss_saodang>(message.m_byte);
            m_saodang.SetActive(true);
            m_saodang.GetComponent<boss_saodang>().init(saodangs.medal, saodangs.hit, saodangs.level, m_yijian_num - sys._instance.m_self.m_t_player.boss_num);
            boss_look();
            int num = 0;
            sys._instance.m_self.add_active(1000, m_yijian_num);
            sys._instance.m_self.m_t_player.boss_task_num += m_yijian_num;
            sys._instance.m_self.check_target_done();
            for (int i = 0; i < saodangs.medal.Count; i++)
            {
                num += saodangs.medal[i];
            }
            sys._instance.m_self.add_att(e_player_attr.player_medal_point, num, false, game_data._instance.get_t_language("boss.cs_317_84"));//魔王扫荡
            if (sys._instance.m_self.m_t_player.boss_num >= m_yijian_num)
            {
                sys._instance.m_self.m_t_player.boss_num = sys._instance.m_self.m_t_player.boss_num - m_yijian_num;
            }
            else
            {
                sys._instance.m_self.remove_item(10010009, m_yijian_num - sys._instance.m_self.m_t_player.boss_num, game_data._instance.get_t_language("boss.cs_317_84"));//魔王扫荡
                sys._instance.m_self.m_t_player.boss_num = 0;
            }
        }
    }

    long reutenTime(int h)
    {
        long now_timer = timer.datetimeconvertcuo(timer.dtnow().Date);
        long star_fighttimer = now_timer + h * 60 * 60 * 1000;
        return star_fighttimer;
    }

    void showtime()
    {
        string s = "";
        if (timer.dtnow().Hour < 12)
        {
            s = "";
            m_time1.text = string.Format("{0}  {1}", s + timer.get_time_show_ex(reutenTime(12) - (long)timer.now()), game_data._instance.get_t_language("chat_gui.cs_44_98"));//开
        }
        else if (timer.dtnow().Hour >= 12 && timer.dtnow().Hour < 14)
        {
            s = "[ff0000]";
            m_time1.text = string.Format("{0}  {1}", s + timer.get_time_show_ex(reutenTime(14) - (long)timer.now()), game_data._instance.get_t_language("chat_gui.cs_45_99"));//关

        }
        else if (timer.dtnow().Hour >= 14 && timer.dtnow().Hour < 18)
        {
            s = "";
            m_time1.text = string.Format("{0}  {1}", s + timer.get_time_show_ex(reutenTime(18) - (long)timer.now()), game_data._instance.get_t_language("chat_gui.cs_44_98"));//开
        }
        else if (timer.dtnow().Hour >= 18 && timer.dtnow().Hour < 20)
        {
            s = "[ff0000]";
            m_time1.text = string.Format("{0}  {1}", s + timer.get_time_show_ex(reutenTime(20) - (long)timer.now()), game_data._instance.get_t_language("chat_gui.cs_45_99"));//关
        }
        else
        {
            s = "";
            m_time1.text = string.Format("{0}  {1}", s + timer.get_time_show_ex(reutenTime(36) - (long)timer.now()), game_data._instance.get_t_language("chat_gui.cs_44_98"));//开
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "show_boss_task_effect")
        {
            if (boss_task_gui.effect())
            {
                m_task_effect.SetActive(true);
            }
            else
            {
                m_task_effect.SetActive(false);
            }
        }
        else if (message.m_type == "refresh_mowang_gui")
        {
            m_attack_num.text = sys._instance.m_self.m_t_player.boss_num + "/10";
        }
        else if (message.m_type == "show_mowang_shop")
        {
            m_shop_gui.SetActive(true);
        }
        else if (message.m_type == "yijian_mowang")
        {
            m_yijian_num = (int)message.m_ints[0];
            m_yijian_use_item = (bool)message.m_bools[0];
            protocol.game.cmsg_boss_saodang _msg = new protocol.game.cmsg_boss_saodang();
            _msg.num = m_yijian_num;
            _msg.use_item = m_yijian_use_item;
            net_http._instance.send_msg<protocol.game.cmsg_boss_saodang>(opclient_t.CMSG_BOSS_SAODANG, _msg);
        }
    }

    public void boss_look()
    {
        protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_BOSS_LOOK, _msg);
    }

    public void boss_look_ex()
    {
        protocol.game.cmsg_common_ex _msg = new protocol.game.cmsg_common_ex();
        net_http._instance.send_msg_ex<protocol.game.cmsg_common_ex>(opclient_t.CMSG_BOSS_LOOK_EX, _msg);
    }

    void check()
    {
        if (sys._instance.m_game_state == "boss")
        {
            CancelInvoke("showtime");
            boss_look_ex();
        }
    }
}
