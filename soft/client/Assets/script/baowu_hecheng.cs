using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class baowu_hecheng : MonoBehaviour, IMessage
{
    List<int> m_baowu_suipian = new List<int>();
    List<int> m_baowu = new List<int>();
    List<int> m_baowu_all = new List<int>();
    public int current_select = 0;
    public int m_baowu_id;
    public int type = 1;
    bool m_first = true;
    bool m_hecheng;
    List<GameObject> m_baowu_objs = new List<GameObject>();
    public GameObject m_baowu_obj;
    public List<GameObject> m_suipians;
    public GameObject m_select;
    public GameObject m_mianzhan_label;
    public GameObject m_mian_zhan_button;

    public GameObject m_kuang;
    public GameObject m_view;
    public GameObject m_effect;
    private static bool m_first_qiang = false;
    private int m_suipian_id;
    public GameObject m_qing_duo_gui;
    public GameObject m_mian_zhan_gui;
    public GameObject m_scroview_text;
    public GameObject m_item_show;
    public GameObject m_ronglian_gui;
    public GameObject m_shenlian;
    List<dhc.treasure_report_t> m_reports;
    public GameObject m_yijian;
    public GameObject m_yjsaodang;
    public GameObject m_select_yj;

    private bool first = true;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void OnDisable()
    {
        CancelInvoke("time");
        CancelInvoke("time1");
        CancelInvoke("refresh_report");
    }
    void refresh_report()
    {
        protocol.game.cmsg_treasure_report_ex _msg = new protocol.game.cmsg_treasure_report_ex();
        net_http._instance.send_msg_ex<protocol.game.cmsg_treasure_report_ex>(opclient_t.CMSG_TREASURE_REPORT_EX, _msg);
    }
    public void OnEnable()
    {
        InvokeRepeating("refresh_report", 10.0f, 10.0f);

        if (m_first_qiang)
        {
            m_first_qiang = false;
            root_gui._instance.action_guide("first_qiang_end");
        }
        huo_dong_gui.m_qd_effect = 0;

        if (first)
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_TREASURE_REPORT, _msg);
        }
        else
        {
            first = true;
        }
    }

    public void fresh_gui()
    {
        OnEnable();
    }

    public void reset()
    {
        if (sys._instance.m_self.m_t_player.level >= (int)e_open_see.es_treasure_shenglian)
        {
            m_shenlian.SetActive(true);
        }
        else
        {
            m_shenlian.SetActive(false);
        }
        if (sys._instance.m_self.m_t_player.level < (int)e_open_see.es_baowu_yj_duobao && sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_baowu_yj_duobao)
        {
            m_yijian.SetActive(false);
        }
        else
        {
            m_yijian.SetActive(true);
        }
        m_baowu.Clear();
        m_baowu_all.Clear();
        m_baowu_suipian.Clear();
        if (m_view.transform.childCount == 2)
        {
            for (int i = 0; i < game_data._instance.m_dbc_baowu.get_y(); i++)
            {
                GameObject temp = Instantiate(m_kuang) as GameObject;
                temp.transform.parent = m_view.transform;
                temp.transform.localScale = Vector3.one;
                temp.transform.localPosition = new Vector3(0, 145 - 100 * i, 0);
                temp.SetActive(true);
                temp.name = i.ToString(); ;
                m_baowu_objs.Add(temp);
            }
        }
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            if (game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]).type == 6001)
            {
                m_baowu_suipian.Add((int)sys._instance.m_self.m_t_player.item_ids[i]);
            }
        }
        s_t_baowu _baowu;
        for (int i = 0; i < game_data._instance.m_dbc_baowu.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_baowu.get(0, i));
            _baowu = game_data._instance.get_t_baowu(id);
            bool flag = false;
            for (int j = 0; j < _baowu.fragments.Count; j++)
            {
                for (int k = 0; k < m_baowu_suipian.Count; k++)
                {
                    if (_baowu.fragments[j] == m_baowu_suipian[k])
                    {
                        flag = true;
                        break;
                    }
                    if (flag)
                    {
                        break;
                    }
                }
                if (flag)
                {
                    m_baowu_all.Add(_baowu.id);
                    break;
                }
            }
        }
        m_select.transform.parent = m_view.transform;
        for (int i = 0; i < m_baowu_objs.Count; i++)
        {
            if (m_baowu_objs[i].transform.Find("icon").childCount != 0)
            {
                sys._instance.remove_child(m_baowu_objs[i].transform.Find("icon").gameObject);
            }
        }
        s_t_huodong_sub sub = game_data._instance.get_t_huodong_sub(90001);
        if (sys._instance.m_self.m_t_player.level >= sub.level)
        {
            if (!m_baowu_all.Contains(2001))
            {
                m_baowu_all.Add(2001);
            }
        }

        {
            if (!m_baowu_all.Contains(2002))
            {
                m_baowu_all.Add(2002);
            }
        }

        {
            if (!m_baowu_all.Contains(2003))
            {
                m_baowu_all.Add(2003);
            }
        }

        {
            if (!m_baowu_all.Contains(2004))
            {
                m_baowu_all.Add(2004);
            }
        }

        m_baowu = m_baowu_all;
        m_baowu.Sort(Comparer);
        if (m_first)
        {
            m_first = false;
            current_select = 0;
            m_baowu_id = m_baowu[0];
        }
        else
        {
            bool flag = false;
            for (int i = 0; i < m_baowu.Count; i++)
            {
                if (m_baowu[i] == m_baowu_id)
                {
                    current_select = i;
                    flag = true;
                }
            }
            if (!flag)
            {
                current_select = 0;
                m_baowu_id = m_baowu[0];
            }
        }
        create_baowu();
        create_suipian();
        hecheng();
        show_effect();
        if (sys._instance.m_self.m_t_player.treasure_protect_next_time > timer.now())
        {
            InvokeRepeating("time", 0, 1f);
            m_mian_zhan_button.SetActive(false);
            m_mianzhan_label.SetActive(true);
        }
        else if (sys._instance.m_self.m_t_player.treasure_protect_cd_time > timer.now())
        {
            InvokeRepeating("time1", 0, 1f);
            m_mian_zhan_button.SetActive(false);
            m_mianzhan_label.SetActive(true);
        }
        else
        {
            m_mianzhan_label.SetActive(false);
            m_mian_zhan_button.SetActive(true);
        }
    }

    void show_effect()
    {
        for (int i = 0; i < m_baowu_objs.Count; i++)
        {
            m_baowu_objs[i].transform.Find("effect").gameObject.SetActive(false);
        }
        for (int k = 0; k < m_baowu.Count; k++)
        {
            s_t_baowu _baowu = game_data._instance.get_t_baowu(m_baowu[k]);
            int num = 0;
            for (int i = 0; i < _baowu.count; i++)
            {

                for (int j = 0; j < sys._instance.m_self.m_t_player.item_ids.Count; j++)
                {
                    if (sys._instance.m_self.m_t_player.item_ids[j] == _baowu.fragments[i])
                    {
                        num++;
                    }
                }
            }
            if (num == _baowu.count)
            {
                m_baowu_objs[k].transform.Find("effect").gameObject.SetActive(true);
            }
            else
            {
                m_baowu_objs[k].transform.Find("effect").gameObject.SetActive(false);
            }
        }
    }

    public static int Comparer(int x, int y)
    {
        s_t_baowu x_baowu = game_data._instance.get_t_baowu(x);
        s_t_baowu y_baowu = game_data._instance.get_t_baowu(y);
        if (x_baowu.type == 5 && y_baowu.type != 5)
        {
            return -1;
        }
        else if (x_baowu.type != 5 && y_baowu.type == 5)
        {
            return 1;
        }
        else if (x_baowu.type == 5 && y_baowu.type == 5)
        {
            return y_baowu.font_color - x_baowu.font_color;
        }
        else if (x_baowu.font_color != y_baowu.font_color)
        {
            return y_baowu.font_color - x_baowu.font_color;
        }
        else
        {
            return x - y;
        }
    }

    void time()
    {
        long _time = (long)(sys._instance.m_self.m_t_player.treasure_protect_next_time - timer.now());
        if (_time > 0)
        {
            m_mianzhan_label.SetActive(true);
            m_mianzhan_label.GetComponent<UILabel>().text = game_data._instance.get_t_language("baowu_hecheng.cs_302_52") + timer.get_time_show(_time);//免战剩余时间：
        }
        else
        {
            CancelInvoke();
            InvokeRepeating("time1", 0, 1f);
        }
    }

    void time1()
    {
        long _time = (long)(sys._instance.m_self.m_t_player.treasure_protect_cd_time - timer.now());
        if (_time > 0)
        {
            m_mianzhan_label.SetActive(true);
            m_mianzhan_label.GetComponent<UILabel>().text = game_data._instance.get_t_language("baowu_hecheng.cs_317_52") + timer.get_time_show(_time);//免战冷却时间：

        }
        else
        {
            CancelInvoke();
            m_mian_zhan_button.SetActive(true);
            m_mianzhan_label.SetActive(false);
        }
    }

    void select_type()
    {
        m_baowu.Clear();
        for (int i = 0; i < m_baowu_all.Count; i++)
        {
            if (game_data._instance.get_t_baowu(m_baowu_all[i]).type == type)
            {
                m_baowu.Add(m_baowu_all[i]);
            }
        }
    }

    void create_baowu()
    {
        for (int i = 0; i < m_baowu_objs.Count; i++)
        {
            m_baowu_objs[i].SetActive(false);
        }
        if (m_baowu.Count != 0)
        {
            for (int i = 0; i < 4; i++)
            {
                m_baowu_objs[i].SetActive(true);
            }
            for (int i = 0; i < m_baowu.Count; i++)
            {
                GameObject temp = icon_manager._instance.create_treasure_icon_ex(m_baowu[i]);
                m_baowu_objs[i].SetActive(true);
                temp.transform.parent = m_baowu_objs[i].transform.Find("icon");
                temp.transform.localPosition = Vector3.zero;
                temp.transform.localScale = Vector3.one;
                temp.GetComponent<BoxCollider>().enabled = false;

            }
            m_select.transform.parent = m_baowu_objs[current_select].transform;
            m_select.transform.localPosition = Vector3.zero;
        }
    }

    public void refresh_time()
    {
        if (sys._instance.m_self.m_t_player.treasure_protect_next_time > timer.now())
        {
            InvokeRepeating("time", 0, 1f);
            m_mian_zhan_button.SetActive(false);
        }
        else if (sys._instance.m_self.m_t_player.treasure_protect_cd_time > timer.now())
        {
            InvokeRepeating("time1", 0, 1f);
            m_mian_zhan_button.SetActive(false);
        }
        else
        {
            m_mianzhan_label.SetActive(false);
            m_mian_zhan_button.SetActive(true);
        }
    }

    void create_suipian()
    {
        if (m_baowu.Count != 0)
        {
            s_message _msg = new s_message();

            _msg.m_type = "bw_show";
            _msg.m_ints.Add(m_baowu[current_select]);

            cmessage_center._instance.add_message(_msg);
        }
    }

    void hecheng()
    {
        s_t_baowu _baowu = game_data._instance.get_t_baowu(m_baowu[current_select]);
        int num = 0;
        for (int i = 0; i < _baowu.count; i++)
        {

            for (int j = 0; j < sys._instance.m_self.m_t_player.item_ids.Count; j++)
            {
                if (sys._instance.m_self.m_t_player.item_ids[j] == _baowu.fragments[i])
                {
                    num++;
                }
            }
        }
        if (num == _baowu.count)
        {
            m_hecheng = true;
        }
        else
        {
            m_hecheng = false;
        }
    }

    public static bool is_canhecheng()
    {
        for (int i = 0; i < game_data._instance.m_dbc_baowu.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_baowu.get(0, i));
            s_t_baowu _bw = game_data._instance.get_t_baowu(id);
            int temp = 0;
            for (int j = 0; j < _bw.fragments.Count; j++)
            {
                if (sys._instance.m_self.get_item_num((uint)_bw.fragments[j]) > 0)
                {
                    temp++;
                }
            }
            if (temp == _bw.fragments.Count)
            {
                return true;
            }
        }
        return false;
    }

    void qianduo(GameObject obj)
    {
        int _id = obj.GetComponent<item_icon>().m_item_id;

        if (!sys._instance.m_self.m_t_player.item_ids.Contains((uint)_id))
        {
            {
                this.GetComponent<ui_title_anim>().hide_ui();
                s_message _mes = new s_message();
                _mes.m_type = "show_baowu_qiangduo_gui";
                _mes.m_ints.Add(_id);
                cmessage_center._instance.add_message(_mes);
            }
        }
    }

    void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            s_message _message = new s_message();
            _message.m_type = "show_huo_dong";
            _message.m_ints.Add(9);
            _message.m_bools.Add(false);
            cmessage_center._instance.add_message(_message);

            sys._instance.m_game_state = "hall";
            sys._instance.load_scene(sys._instance.m_hall_name);
            m_first = true;
            Object.Destroy(this.gameObject);
        }
        else if (obj.name == "close_itemshow")
        {
            m_item_show.GetComponent<ui_show_anim>().hide_ui();
            reset();
        }
        else if (obj.transform.name == "add_jl")
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
        else if (obj.name == "shenglian")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_shenglian)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("baowu_hecheng.cs_501_73"), (int)e_open_level.el_treasure_shenglian));//饰品圣炼{0}级开启
                return;
            }
            first = false;
            m_ronglian_gui.GetComponent<baowu_ronglian_gui>().m_id = 0;
            m_ronglian_gui.GetComponent<baowu_ronglian_gui>().m_guid = 0;
            m_ronglian_gui.GetComponent<baowu_ronglian_gui>().reset(0);
            m_ronglian_gui.SetActive(true);
        }
        else if (obj.name == "mianzhan")
        {
            m_mian_zhan_gui.SetActive(true);
        }
        else if (obj.name == "zhan_bao")
        {
            s_message _mes = new s_message();
            _mes.m_type = "show_baowu_zhanbao_gui";
            cmessage_center._instance.add_message(_mes);
            this.GetComponent<ui_title_anim>().hide_ui();
        }
        else if (obj.name == "he_cheng")
        {
            if (m_hecheng)
            {
                if (sys._instance.m_self.get_un_treasure_num() < sys._instance.m_self.get_max_treasure_num())
                {
                    protocol.game.cmsg_treasure_hecheng _msg = new protocol.game.cmsg_treasure_hecheng();
                    _msg.id = m_baowu[current_select];
                    net_http._instance.send_msg<protocol.game.cmsg_treasure_hecheng>(opclient_t.CMSG_TREASURE_HECHENG, _msg);
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_hecheng.cs_533_60"));//饰品格子不足
                }
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_hecheng.cs_539_59"));//材料不足
            }
        }
        else if (obj.name == "yijian")
        {
            if (m_hecheng)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_hecheng.cs_546_59"));//当前饰品材料已经收集齐，可以合成饰品啦！
                return;
            }
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_baowu_yj_duobao && sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_baowu_yj_duobao)
            {
                root_gui._instance.show_prompt_dialog_box
                    ("[ffc882]" + string.Format(game_data._instance.get_t_language("baowu_hecheng.cs_555_48"), (int)e_open_vip.ev_baowu_yj_duobao, (int)e_open_level.el_baowu_yj_duobao));//该功能VIP{0}或者达到{1}级开启
                return;
            }
            if (sys._instance.m_self.m_t_player.energy < 2)
            {
                int item_id = 10010007;
                int num = sys._instance.m_self.get_item_num((uint)item_id);
                if (num > 0)
                {
                    m_select_yj.SetActive(true);
                    m_select_yj.GetComponent<baowu_yjsaodang_select>().init(m_baowu_id);
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
            else
            {
                m_select_yj.SetActive(true);
                m_select_yj.GetComponent<baowu_yjsaodang_select>().init(m_baowu_id);
            }
        }
        else
        {
            int num = int.Parse(obj.name);

            if (m_baowu.Count > num)
            {
                current_select = num;
                m_baowu_id = m_baowu[current_select];
                m_select.transform.parent = m_baowu_objs[current_select].transform;
                m_select.transform.localPosition = Vector3.zero;
                hecheng();
                create_suipian();
            }
        }
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_TREASURE_HECHENG)
        {
            protocol.game.smsg_treasure_hecheng _treasures = net_http._instance.parse_packet<protocol.game.smsg_treasure_hecheng>(message.m_byte);
            sys._instance.m_self.add_treasure(_treasures.treasure, false);
            s_t_baowu _baowu = game_data._instance.get_t_baowu(m_baowu[current_select]);
            for (int i = 0; i < _baowu.fragments.Count; i++)
            {
                sys._instance.m_self.remove_item((uint)_baowu.fragments[i], 1, game_data._instance.get_t_language("baowu_hecheng.cs_609_65"));//宝物合成消耗
            }
            m_suipian_id = m_baowu[current_select];
            sys._instance.m_self.add_active(1800, 1);
            this.Invoke("show_item", 2.5f);
        }
        else if (message.m_opcode == opclient_t.CMSG_TREASURE_REPORT)
        {
            protocol.game.smsg_treasure_report _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_report>(message.m_byte);
            m_reports = _msg.reports;
            if (_msg.suipian_ids.Count != 0)
            {
                for (int i = 0; i < _msg.suipian_ids.Count; i++)
                {
                    sys._instance.m_self.remove_item((uint)_msg.suipian_ids[i], 1, game_data._instance.get_t_language("baowu_hecheng.cs_623_66"));//夺宝战报消耗
                }
            }
            show_reports(_msg.reports);
            reset();
        }
        else if (message.m_opcode == opclient_t.CMSG_TREASURE_REPORT_EX)
        {
            protocol.game.smsg_treasure_report _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_report>(message.m_byte);
            if (_msg.suipian_ids.Count != 0)
            {
                for (int i = 0; i < _msg.suipian_ids.Count; i++)
                {
                    sys._instance.m_self.remove_item((uint)_msg.suipian_ids[i], 1, game_data._instance.get_t_language("baowu_hecheng.cs_623_66"));//夺宝战报消耗
                }
                reset();
            }
            int count = m_reports.Count;

            for (int i = 0; i < _msg.reports.Count; i++)
            {
                if (!is_inreport(_msg.reports[i].guid))
                {

                    if (m_reports.Count == 10)
                    {
                        m_reports.RemoveAt(0);
                    }
                    m_reports.Add(_msg.reports[i]);
                }
            }
            show_reports(m_reports);
        }
        else if (message.m_opcode == opclient_t.CMSG_TREASURE_YIJIAN_SAODANG)
        {
            protocol.game.smsg_treasure_yijian_saodang _msg1 = net_http._instance.parse_packet<protocol.game.smsg_treasure_yijian_saodang>(message.m_byte);

            m_yjsaodang.GetComponent<baowu_yjsaodang>().init(m_baowu_id, _msg1.success, _msg1.rewards);
            m_yjsaodang.SetActive(true);
        }
    }

    bool is_inreport(ulong guid)
    {
        bool flag = false;
        for (int i = 0; i < m_reports.Count; i++)
        {
            if (m_reports[i].guid == guid)
            {
                flag = true;
                break;
            }
        }
        return flag;
    }

    void show_item()
    {
        m_item_show.SetActive(true);

        UILabel _label = m_item_show.transform.Find("info").GetComponent<UILabel>();
        GameObject _icon = m_item_show.transform.Find("icon").gameObject;
        _label.text = sys._instance.get_res_info(6, m_suipian_id, 0, 0);
        sys._instance.remove_child(_icon);
        GameObject temp = icon_manager._instance.create_treasure_icon(m_suipian_id);
        temp.transform.parent = _icon.transform;
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
    }

    void show_reports(List<dhc.treasure_report_t> _inforeport)
    {
        string s = "";
        for (int i = 0; i < _inforeport.Count; i++)
        {
            dhc.treasure_report_t temp = _inforeport[i];
            switch (temp.win)
            {
                case 0:
                    s += "\n" + "[ff0000]" + string.Format(game_data._instance.get_t_language("baowu_hecheng.cs_703_36"),
                    timer.get_show_time((long)(timer.now() - (ulong)temp.time)), Encoding.UTF8.GetString(temp.other_name), sys._instance.get_res_info(2, temp.suipian_id, 0, 0));//胜利！[-]{0},[00ff00]{1}[-]试图抢夺你的[{2}]被你赶走了。[-]
                    break;
                case 1:
                    s += "\n" + "[ff0000]" + string.Format(game_data._instance.get_t_language("baowu_hecheng.cs_707_35"),
                        timer.get_show_time((long)(timer.now() - (ulong)temp.time)), Encoding.UTF8.GetString(temp.other_name), sys._instance.get_res_info(2, temp.suipian_id, 0, 0));//失败！[-]{0},你被[00ff00]{1}[-]打败，你带着[{2}]灰溜溜地逃走了
                    break;
                case 2:
                    s += "\n" + "[ff0000]" + string.Format(game_data._instance.get_t_language(" baowu_hecheng.cs_711_35"),
                        timer.get_show_time((long)(timer.now() - (ulong)temp.time)), Encoding.UTF8.GetString(temp.other_name), sys._instance.get_res_info(2, temp.suipian_id, 0, 0));//失败！[-]{0},你被[00ff00]{1}[-]打败，你的[{2}]]被抢走了。
                    break;
                case 3:
                    s += "\n" + "[ff0000]" + string.Format(game_data._instance.get_t_language("baowu_hecheng.cs_715_34"),
                        timer.get_show_time((long)(timer.now() - (ulong)temp.time)), Encoding.UTF8.GetString(temp.other_name), sys._instance.get_res_info(2, temp.suipian_id, 0, 0));//失败！[-]{0},你被[00ff00]{1}[-]连续抢夺，你的[{2}被抢走了。
                    break;
                case 4:
                    s += "\n" + "[ff0000]" + string.Format(game_data._instance.get_t_language("baowu_hecheng.cs_719_34"),
                        timer.get_show_time((long)(timer.now() - (ulong)temp.time)) + "" + Encoding.UTF8.GetString(temp.other_name) + "" + "[" + sys._instance.get_res_info(2, temp.suipian_id, 0, 0));//失败！[-]{0},[00ff00]{1}[-]试图连续抢夺你，你带着[{2}]逃走了。
                    break;
            }
        }
        m_scroview_text.GetComponent<UILabel>().text = s;
        m_scroview_text.GetComponent<UIDragScrollView>().scrollView.Scroll(-1000f);
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "qiangduo_1_next" || message.m_type == "show_baowu_qiangduo_gui")
        {
            m_qing_duo_gui.GetComponent<baowu_qiangduo>().m_suipian_id = (int)message.m_ints[0];
            m_qing_duo_gui.GetComponent<baowu_qiangduo>().init();
            m_qing_duo_gui.SetActive(true);
        }
        else if (message.m_type == "first_qiang")
        {
            m_first_qiang = true;
        }
        else if (message.m_type == "refresh_treasure_effect")
        {
            m_effect.SetActive(false);
        }
        else if (message.m_type == "refresh_bw_gui")
        {
            reset();
        }
        else if (message.m_type == "show_baowu_hecheng_gui1")
        {
            reset();
        }
        else if (message.m_type == "baowu_hecheng")
        {
            if (sys._instance.m_self.get_un_treasure_num() >= sys._instance.m_self.get_max_treasure_num())
            {
                s_message mes = new s_message();
                mes.m_type = "treasure_full_buzheng";
                string tishi = game_data._instance.get_t_language("arena.cs_104_45");//提示
                string _des = game_data._instance.get_t_language("bag_gui.cs_2059_38");//您的饰品携带数量已达上限，是否前往进行饰品强化或者精炼
                root_gui._instance.show_select_dialog_box(tishi, _des, mes);

                return;
            }
            if (sys._instance.m_self.get_un_treasure_num() < sys._instance.m_self.get_max_treasure_num())
            {
                protocol.game.cmsg_treasure_hecheng _msg = new protocol.game.cmsg_treasure_hecheng();
                _msg.id = (int)message.m_ints[0];
                net_http._instance.send_msg<protocol.game.cmsg_treasure_hecheng>(opclient_t.CMSG_TREASURE_HECHENG, _msg);
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_hecheng.cs_533_60"));//饰品格子不足
            }
        }
    }
}
