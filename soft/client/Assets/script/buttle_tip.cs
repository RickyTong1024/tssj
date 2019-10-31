using System.Collections.Generic;
using UnityEngine;

public class buttle_tip : MonoBehaviour, IMessage
{
    public int m_mission_id = 1001;
    s_t_mission m_mission;
    public GameObject m_des;
    public GameObject m_name;
    public GameObject m_free;
    public GameObject m_num;
    public GameObject m_sd;
    public GameObject m_sdtan;
    public GameObject m_start;
    public GameObject m_tili;
    public GameObject m_gold;
    public GameObject m_yuanli;
    public GameObject m_exp;
    public GameObject m_main_root;
    public GameObject m_star;
    public GameObject m_boss;
    public GameObject[] m_rewards;
    public GameObject m_dress;
    public GameObject m_saodang;
    private int m_pnum;
    private int m_sd_num;
    public GameObject m_ex_tip;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    public void click(GameObject obj)
    {
        if (obj.name == "hide")
        {
            return;
        }
        if (sys._instance.m_self.bag_full())
        {
            m_saodang.transform.Find("frame_big").GetComponent<frame>().hide();
            return;
        }
        if (obj.transform.name == "dui_xing")
        {
            s_message _message = new s_message();
            _message.m_type = "show_duixing_gui";
            _message.m_bools.Add(true);
            cmessage_center._instance.add_message(_message);
            return;
        }
        m_pnum = 1;
        if (obj.transform.name == "shao_dang_10")
        {
            m_pnum = m_sd_num;
        }
        int ci = sys._instance.m_self.get_mission_cishu(m_mission.id);
        if (m_mission.day_num != 0 && ci >= m_mission.day_num)
        {
            int num = sys._instance.m_self.get_mission_goumai(m_mission.id);
            s_t_price _price = game_data._instance.get_t_price(num + 1);
            s_t_vip _vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
            s_t_exp _exp = game_data._instance.get_t_exp(sys._instance.m_self.m_t_player.level);
            if (num + 1 > _vip.jy_buy_num)
            {
                string s = "[ffc882]" + game_data._instance.get_t_language("buttle_tip.cs_82_40");//今日购买次数已经用完，提升vip等级可增加购买次数
                root_gui._instance.show_prompt_dialog_box(s);
                return;
            }
            string s1 = string.Format(game_data._instance.get_t_language("buttle_tip.cs_86_38"), _price.jy, ((int)_vip.jy_buy_num - num));//是否花费[00ffff]{0}钻石[-]购买关卡次数，您今日还可购买{1}次

            string s4 = game_data._instance.get_t_language("arena.cs_104_45");//提示
            string _des = s1;
            s_message _message = new s_message();
            _message.m_type = "buy_jy";
            _message.m_ints.Add(_price.jy);
            root_gui._instance.show_select_dialog_box(s4, _des, _message);
            return;
        }
        int tili = sys._instance.m_self.get_att(e_player_attr.player_tili);
        if (tili < m_mission.tili * m_pnum)
        {
            int item_id = 10010002;
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
                _message.m_ints.Add(100200);
                _message.m_ints.Add(2);
                cmessage_center._instance.add_message(_message);
            }
            return;
        }

        if (obj.transform.name == "start")
        {
            protocol.game.cmsg_mission_fight_end _msg = new protocol.game.cmsg_mission_fight_end();
            _msg.mission_id = m_mission.id;
            net_http._instance.send_msg<protocol.game.cmsg_mission_fight_end>(opclient_t.CMSG_MISSION_FIGHT_END, _msg);
        }
        else if (obj.transform.name == "shao_dang" || obj.transform.name == "shao_dang_10")
        {
            if (obj.transform.name == "shao_dang_10")
            {
                if (sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_saodang10 && sys._instance.m_self.m_t_player.level < (int)e_open_level.el_saodang10)
                {
                    string s = string.Format(game_data._instance.get_t_language("baowu_hecheng.cs_555_48"), (int)e_open_vip.ev_saodang10, (int)e_open_level.el_saodang10);//该功能VIP{0}或者达到{1}级开启

                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
                    return;
                }
            }
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_saodang)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("duixing_gui.cs_431_71"), (int)e_open_level.el_saodang));//{0}级开启
                return;
            }

            s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
            int total_num = sys._instance.m_self.m_t_player.tili / m_mission.tili;
            if (total_num <= 0)
            {
                string tishi = game_data._instance.get_t_language("arena.cs_104_45");//提示
                string s = string.Format(game_data._instance.get_t_language("buttle_tip.cs_148_29"), m_pnum.ToString(), m_pnum.ToString());//是否花费[00ffff]{0}钻石[-]扫荡{1}次
                s_message msg = new s_message();
                msg.m_type = "saodang";
                root_gui._instance.show_select_dialog_box(tishi, s, msg);
            }
            else
            {
                saodang();
            }
        }
    }

    void saodang()
    {
        s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
        int total_num = sys._instance.m_self.m_t_player.tili / m_mission.tili;
        if (total_num <= 0)
        {
            int item_id = 10010002;
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
                _message.m_ints.Add(100200);
                _message.m_ints.Add(2);
                cmessage_center._instance.add_message(_message);
                return;
            }
        }
        this.gameObject.SetActive(false);
        protocol.game.cmsg_mission_saodang _msg = new protocol.game.cmsg_mission_saodang();
        _msg.mission_id = m_mission.id;
        _msg.num = m_pnum;
        net_http._instance.send_msg<protocol.game.cmsg_mission_saodang>(opclient_t.CMSG_MISSION_SAODANG, _msg);
    }

    public void close()
    {
        this.transform.Find("frame_big").GetComponent<frame>().hide();
    }

    public void init()
    {
        m_mission = game_data._instance.get_t_mission(m_mission_id);
        Transform _up = this.transform.Find("back");
        m_ex_tip.SetActive(false);
        if (m_mission.type == 3)
        {
            m_ex_tip.SetActive(true);
        }
        m_name.GetComponent<UILabel>().text = m_mission.name;
        m_des.GetComponent<UILabel>().text = m_mission.des;
        m_star.GetComponent<UIProgressBar>().value = 0.3333f * sys._instance.m_self.get_mission_star(m_mission_id);
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            sys._instance.remove_child(m_rewards[i]);
            m_rewards[i].gameObject.SetActive(false);
        }
        m_dress.gameObject.SetActive(false);
        for (int i = 0; i < m_mission.items.Count; i++)
        {
            if (m_mission.items[i].reward.type == 7)
            {
                s_t_dress dress = game_data._instance.get_t_dress(m_mission.items[i].reward.value1);
                m_dress.gameObject.SetActive(true);
                m_dress.GetComponent<UISprite>().spriteName = dress.icon;
                continue;
            }
            if (sys._instance.is_hide_reward(m_mission.items[i].reward.type, m_mission.items[i].reward.value1))
            {
                continue;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_mission.items[i].reward.type, m_mission.items[i].reward.value1, m_mission.items[i].reward.value2, m_mission.items[i].reward.value3);

            if (_icon != null)
            {
                m_rewards[count].gameObject.SetActive(true);
                _icon.transform.parent = m_rewards[count].transform;
                _icon.transform.localPosition = new Vector3(0, 0, 0);
                _icon.transform.localScale = new Vector3(1, 1, 1);
            }
            count++;
        }

        int _id = get_monster_id(m_mission);
        if (_id > 2)
        {
            ccard _card = new ccard();
            _card.set_monster(_id);
            m_boss.GetComponent<UISprite>().spriteName = _card.m_t_class.icon;
            if (m_mission.jytype == 0)
            {
                m_boss.transform.Find("icon").GetComponent<UISprite>().spriteName = "xtbk_lvpt001";
            }
            else if (m_mission.jytype == 1)
            {
                m_boss.transform.Find("icon").GetComponent<UISprite>().spriteName = "xtbk_zipt001";
            }
            else if (m_mission.jytype == 2)
            {
                m_boss.transform.Find("icon").GetComponent<UISprite>().spriteName = "xtbk_chpt001";
            }
        }

        int num;
        s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
        int total_num = sys._instance.m_self.m_t_player.tili / m_mission.tili;
        int day_num = m_mission.day_num - sys._instance.m_self.get_mission_cishu(m_mission.id);
        int max_num = m_mission.day_num;
        if (max_num > 10)
        {
            max_num = 10;
        }
        num = total_num;
        if (num > max_num)
        {
            num = max_num;
        }
        if (num > day_num)
        {
            num = day_num;
        }
        if (num == 0)
        {
            if (total_num == 0 && day_num == 0)
            {
                num = max_num;
            }
            else if (total_num == 0)
            {
                num = max_num;
                if (day_num < num)
                {
                    num = day_num;
                }
            }
            else
            {
                num = max_num;
                if (total_num < num)
                {
                    num = total_num;
                }
            }
        }
        m_free.GetComponent<UILabel>().text = day_num + "/" + m_mission.day_num;
        string sd = string.Format(game_data._instance.get_t_language("buttle_tip.cs_303_28"), num.ToString());//扫荡{0}次
        m_num.transform.GetComponent<UILabel>().text = sd;
        m_sd_num = num;

        if (num == 0)
        {
            m_sd.transform.GetComponent<UISprite>().set_enable(false);
            m_sd.transform.GetComponent<BoxCollider>().enabled = false;
            m_sdtan.transform.GetComponent<UISprite>().set_enable(false);
            m_sdtan.transform.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            if (sys._instance.m_self.get_mission_star(m_mission.id) != 3)
            {
                m_sd.transform.GetComponent<UISprite>().set_enable(false);
                m_sd.transform.GetComponent<BoxCollider>().enabled = false;
                m_sdtan.transform.GetComponent<UISprite>().set_enable(false);
                m_sdtan.transform.GetComponent<BoxCollider>().enabled = false;
            }
            else
            {
                m_sd.transform.GetComponent<UISprite>().set_enable(true);
                m_sd.transform.GetComponent<BoxCollider>().enabled = true;
                m_sdtan.transform.GetComponent<UISprite>().set_enable(true);
                m_sdtan.transform.GetComponent<BoxCollider>().enabled = true;
            }
        }

        m_tili.transform.GetComponent<UILabel>().text = sys._instance.get_res_color(3) + m_mission.tili.ToString();
        m_gold.transform.GetComponent<UILabel>().text = sys._instance.get_res_color(1) + (m_mission.tili * sys._instance.m_self.m_t_player.level * 10 + 50).ToString();
        m_exp.transform.GetComponent<UILabel>().text = sys._instance.get_res_color(4) + (m_mission.tili * 5).ToString();
        m_yuanli.transform.GetComponent<UILabel>().text = sys._instance.get_res_color(7) + (m_mission.tili * 250).ToString();

        if (game_data._instance.m_guaji > 0 && sys._instance.m_self.m_t_player.tili > m_mission.tili)
        {
            protocol.game.cmsg_mission_fight_end _msg = new protocol.game.cmsg_mission_fight_end();
            _msg.mission_id = m_mission.id;
            net_http._instance.send_msg<protocol.game.cmsg_mission_fight_end>(opclient_t.CMSG_MISSION_FIGHT_END, _msg);
        }
        else if (game_data._instance.m_guaji > 0)
        {
            game_data._instance.m_guaji = 0;
            root_gui._instance.show_tili_dialog_box(10010002);
            return;
        }
    }

    public static int get_monster_id(s_t_mission t_mission)
    {
        int _id = t_mission.monsters[0];

        if (t_mission.monsters[5] > 0)
        {
            _id = t_mission.monsters[5];
        }
        return _id;
    }

    public void fini_saodang(List<protocol.game.smsg_mission_fight_end> saodangs)
    {
        for (int k = 0; k < saodangs.Count; ++k)
        {
            protocol.game.smsg_mission_fight_end sd = saodangs[k];
            for (int i = 0; i < sd.types.Count; i++)
            {
                sys._instance.m_self.add_reward(sd.types[i], sd.value1s[i], sd.value2s[i], sd.value3s[i], false, game_data._instance.get_t_language("buttle_tip.cs_358_100"));//关卡扫荡获得
            }
            for (int i = 0; i < sd.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(sd.equips[i], false);
            }
            sys._instance.m_self.sub_att(e_player_attr.player_tili, m_mission.tili);
            // 副本次数
            if (m_mission.day_num > 0)
            {
                sys._instance.m_self.add_mission_cishu(m_mission.id, 1);
            }
            // 目标，日常
            if (m_mission.type == 1)
            {
                sys._instance.m_self.m_t_player.pt_task_num = sys._instance.m_self.m_t_player.pt_task_num + 1;
                sys._instance.m_self.add_active(400, 1);
                sys._instance.m_self.check_target_done();
                sys._instance.m_self.zs_skill_target(1, 1);
            }
            else if (m_mission.type == 2)
            {
                sys._instance.m_self.m_t_player.jy_task_num = sys._instance.m_self.m_t_player.jy_task_num + 1;
                sys._instance.m_self.add_active(500, 1);
                sys._instance.m_self.check_target_done();
                sys._instance.m_self.zs_skill_target(2, 1);
            }
            else if (m_mission.type == 4)
            {
                sys._instance.m_self.add_active(1400, 1);
            }
            else if (m_mission.type == 5)
            {
                sys._instance.m_self.add_active(1500, 1);
            }
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
        }
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_MISSION_FIGHT_END)
        {
            protocol.game.smsg_mission_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_mission_fight_end>(message.m_byte);
            battle_logic_ex._instance.set_mission_fight_end(_msg);
            this.gameObject.SetActive(false);
            sys._instance.load_scene_ex(m_mission.map_name);
            s_message _new_msg = new s_message();
            _new_msg.m_type = "mission";
            _new_msg.m_string.Add(m_mission_id.ToString());
            cmessage_center._instance.add_message(_new_msg);

            sys._instance.m_game_state = "buttle";

            if ((m_mission.type == 1 && sys._instance.m_self.m_t_player.mission == m_mission.lock_id) || (m_mission.type == 2 && sys._instance.m_self.m_t_player.mission_jy == m_mission.jylock_id))
            {
                s_message _out_msg = new s_message();
                _out_msg.m_type = "mission_lock";
                _out_msg.m_ints.Add(0);
                cmessage_center._instance.add_message(_out_msg);
            }
            else
            {
                s_message _out_msg = new s_message();
                _out_msg.m_type = "mission_lock";
                _out_msg.m_ints.Add(m_mission_id);
                cmessage_center._instance.add_message(_out_msg);
            }
        }
        if (message.m_opcode == opclient_t.CMSG_MISSION_GOUMAI)
        {
            int num = sys._instance.m_self.get_mission_goumai(m_mission.id);
            s_t_price _price = game_data._instance.get_t_price(num + 1);

            sys._instance.m_self.sub_att(e_player_attr.player_jewel, _price.jy, game_data._instance.get_t_language("buttle_tip.cs_447_79"));//关卡次数购买消耗

            bool flag = false;
            for (int i = 0; i < sys._instance.m_self.m_t_player.mission_goumai_ids.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.mission_goumai_ids[i] == m_mission.id)
                {
                    flag = true;
                    sys._instance.m_self.m_t_player.mission_goumai[i]++;
                    break;
                }
            }
            if (!flag)
            {
                sys._instance.m_self.m_t_player.mission_goumai_ids.Add(m_mission.id);
                sys._instance.m_self.m_t_player.mission_goumai.Add(1);
            }

            for (int i = 0; i < sys._instance.m_self.m_t_player.mission_cishu_ids.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.mission_cishu_ids[i] == m_mission.id)
                {
                    sys._instance.m_self.m_t_player.mission_cishu[i] = 0;
                    break;
                }
            }
            init();
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "saodang")
        {
            saodang();
        }
        if (message.m_type == "buy_jy")
        {
            int _count = (int)message.m_ints[0];
            if (_count > sys._instance.m_self.get_att(e_player_attr.player_jewel))
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_ronglian_gui.cs_141_59"));//钻石不足
                return;
            }
            protocol.game.cmsg_mission_goumai _msg = new protocol.game.cmsg_mission_goumai();
            _msg.mission_id = m_mission.id;
            net_http._instance.send_msg<protocol.game.cmsg_mission_goumai>(opclient_t.CMSG_MISSION_GOUMAI, _msg);
        }
        if (message.m_type == "refresh_buttle_gui")
        {
            init();
        }
    }
}
