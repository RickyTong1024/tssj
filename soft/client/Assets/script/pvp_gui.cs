
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class pvp_gui : MonoBehaviour,IMessage {

    public GameObject m_pvp_shop;
    public GameObject m_pvp_task;
    public GameObject m_pvp_rank;
    public GameObject m_task_button;
    public UILabel m_cur_jifen;
    public UILabel m_rank;
    public GameObject m_refresh_num;
    public UILabel m_attack_num;
    public List<GameObject> m_lierens;
    protocol.game.smsg_pvp_view m_msg;
    public GameObject m_objs; 

    public List<GameObject> m_units = new List<GameObject>();
    List<int> m_ids = new List<int>();
    private GameObject m_cam;
    int num = 0;
    public GameObject m_rank_qiansan;
    public  List<GameObject> m_qiansan;
    public GameObject m_bottom;

    public List<GameObject> m_click_objs;
	// Use this for initialization

    public void add_handle()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnEnable()
    {
        protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_PVP_VIEW, _msg);
    }

    void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            if (m_pvp_shop.activeSelf)
            {
                m_pvp_shop.SetActive(false);
                return;
            }
            if (m_pvp_rank.activeSelf)
            {
                m_pvp_rank.SetActive(false);
                return;
            }
            if (m_pvp_task.activeSelf)
            {
                m_pvp_task.SetActive(false);
                return;
            }
            s_message _message = new s_message();
            _message.m_type = "show_huo_dong";
            _message.m_ints.Add(10);
			_message.m_bools.Add(false);
            cmessage_center._instance.add_message(_message);
            sys._instance.m_game_state = "hall";
            sys._instance.load_scene(sys._instance.m_hall_name);
            this.gameObject.SetActive(false);
        }
        else if (obj.name == "shop")
        {
            m_pvp_shop.SetActive(true);
        }
        else if (obj.name == "rank")
        {
            protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view();
            _msg.type = 0;
            net_http._instance.send_msg<protocol.game.cmsg_rank_view>(opclient_t.CMSG_PVP_RANK, _msg, true);
        }
        else if (obj.name == "task")
        {
            m_pvp_task.SetActive(true);
            m_pvp_task.GetComponent<pvp_task_gui>().reset(0);
        }
        else if (obj.name == "refresh")
        {
            if (sys._instance.m_self.m_t_player.pvp_refresh_num < 10)
            {
                protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
                net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_PVP_REFRESH, _msg);
            }
            else
            {
                if (sys._instance.m_self.m_t_player.jewel >= 10)
                {
                    protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
                    net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_PVP_REFRESH, _msg);
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
                }
            }
        }
        else if (obj.name == "buy")
        {
            s_t_price t_price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.pvp_buy_num + 1);
            s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
            if (sys._instance.m_self.m_t_player.pvp_buy_num >= t_vip.hunter_assembly)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_boss_gui_ex.cs_537_58"));//[ffc882]今日购买次数不足，提升VIP等级可增加每日购买次数
                return;
            }
            s_message _mes = new s_message();
            _mes.m_type = "buy_pvp_attack_num";
            root_gui._instance.show_cishu_dialog_box(sys._instance.m_self.m_t_player.pvp_num, t_vip.hunter_assembly,
                sys._instance.m_self.m_t_player.pvp_buy_num,2, _mes);
        }
        else
        {
            if (m_msg.player_templates.Count >= int.Parse(obj.name) + 1)
            {
                for (int i = 0; i < m_msg.player_wins.Count; i++)
                {
                    if (int.Parse(obj.name) == i && m_msg.player_wins[i] == 1)
                    {
                        return;
                    }
                }
                if (!is_lieren())
                {
                    return;
                }
                if (m_msg.player_guids.Count <= int.Parse(obj.name))
                {
                    return;
                }
                if (sys._instance.m_self.m_t_player.pvp_num > 0)
                {
                    try
                    {
                        GameObject gameObj = m_units[int.Parse(obj.name)];
                        TweenScale _scale = TweenScale.Begin(gameObj, 0.35f, new Vector3(1.1f, 1.1f, 1.1f));
                        _scale.updateTable = true;
                        _scale.method = UITweener.Method.Linear;
                        _scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));
                        protocol.game.cmsg_pvp_fight_end _msg = new protocol.game.cmsg_pvp_fight_end();
                        _msg.index = int.Parse(obj.name);
                        net_http._instance.send_msg<protocol.game.cmsg_pvp_fight_end>(opclient_t.CMSG_PVP_FIGHT_END, _msg);
                    }
                    catch (UnityException)
                    {
 
                    }
                }
                else
                {
                    s_t_price t_price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.pvp_buy_num + 1);
                    s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
                    if (sys._instance.m_self.m_t_player.pvp_buy_num >= t_vip.hunter_assembly)
                    {
                        root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("pvp_gui.cs_192_66"));//[ffc882]挑战次数不足，提升VIP等级可以增加可购买的挑战次数
                        return;
                    }
                    s_message _mes = new s_message();
                    _mes.m_type = "buy_pvp_attack_num";
                    root_gui._instance.show_cishu_dialog_box(sys._instance.m_self.m_t_player.pvp_num, t_vip.hunter_assembly,
                        sys._instance.m_self.m_t_player.pvp_buy_num, 2, _mes);

                } 
            }
        }
    }
    void refresh_pvp()
    {
     
        if (m_msg.player_guids.Count != 0)
        {
            m_attack_num.text = game_data._instance.get_t_language ("pvp_gui.cs_214_32") + sys._instance.m_self.m_t_player.pvp_num + "";//今日挑战次数：
            m_attack_num.transform.Find("buy").gameObject.SetActive(true);
        }
        else
        {
            m_attack_num.text = game_data._instance.get_t_language ("huo_dong_gui.cs_735_31");//[ff0000]未开启
            m_attack_num.transform.Find("buy").gameObject.SetActive(false);
            m_attack_num.transform.localPosition = new Vector3(30,-290,0);
        }
        m_attack_num.gameObject.SetActive(true);
        m_refresh_num.gameObject.SetActive(true);
        m_cur_jifen.text = sys._instance.m_self.m_t_player.pvp_total + "";
        if (10 - sys._instance.m_self.m_t_player.pvp_refresh_num > 0)
        {
            m_refresh_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pvp_gui.cs_231_57");//免费刷新：
            m_refresh_num.transform.Find("Label").gameObject.SetActive(true);
            m_refresh_num.transform.Find("jewel").gameObject.SetActive(false);
            m_refresh_num.transform.Find("Label").GetComponent<UILabel>().text = 10 - sys._instance.m_self.m_t_player.pvp_refresh_num + "";
        }
        else
        {
            m_refresh_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pvp_gui.cs_238_57");//钻石刷新：
            m_refresh_num.transform.Find("Label").gameObject.SetActive(false);
            m_refresh_num.transform.Find("jewel").gameObject.SetActive(true);
            m_refresh_num.transform.Find("jewel").GetComponent<UILabel>().text = 10 + ""; ;
        }
        pvp_reward_gui _rank = m_pvp_rank.GetComponent<pvp_reward_gui>();
        if (_rank._msg.rank_list == null)
        {
            m_rank.transform.Find("wei").gameObject.SetActive(true);
            m_rank.transform.Find("rank").gameObject.SetActive(false);
        }
        else if (_rank.get_rank() != -1)
        {
            m_rank.transform.Find("wei").gameObject.SetActive(false);
            m_rank.transform.Find("rank").gameObject.SetActive(true);
            m_rank.transform.Find("rank").GetComponent<UILabel>().text = _rank.get_rank() + 1 + "";
        }
        else
        {
            m_rank.transform.Find("wei").gameObject.SetActive(true);
            m_rank.transform.Find("rank").gameObject.SetActive(false);
        }
        if (pvp_task_gui.is_effect())
        {
            m_task_button.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_task_button.transform.Find("effect").gameObject.SetActive(false);
        }
        if (is_lieren())
        {
            GameObject tx = GameObject.Find("ts_kfuz001");
            if (tx != null)
            {
                tx.SetActive(true);
            }
            m_bottom.SetActive(true);
        }
        else
        {
            GameObject tx = GameObject.Find("ts_kfuz001");
            if (tx != null)
            {
                tx.SetActive(false);
            }
            m_bottom.SetActive(false);
        }
    }
   
    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "buy_pvp_attack_num")
        {
            protocol.game.cmsg_shop_buy _msg = new protocol.game.cmsg_shop_buy();
            _msg.num = (int)mes.m_ints[0];
            num = _msg.num;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_PVP_BUY, _msg);
        }
        else if (mes.m_type == "refresh_pvp")
        {
            refresh_pvp();
        }
		else if(mes.m_type == "show_pvp_shop_gui")
		{
			m_pvp_shop.SetActive(true);
		}
    }
    void IMessage.net_message(s_net_message mes) 
    {
        if (mes.m_opcode == opclient_t.CMSG_PVP_VIEW)
        {
            if (is_lieren())
            {
                this.gameObject.SetActive(true);
                m_msg = net_http._instance.parse_packet<protocol.game.smsg_pvp_view>(mes.m_byte);
                show_huoban();
                m_rank_qiansan.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(true);
                m_msg = net_http._instance.parse_packet<protocol.game.smsg_pvp_view>(mes.m_byte);
                show_qiansan();
            }
           
        }
        else if (mes.m_opcode == opclient_t.CMSG_PVP_REFRESH)
        {
            if (sys._instance.m_self.m_t_player.pvp_refresh_num < 10)
            {
                sys._instance.m_self.m_t_player.pvp_refresh_num++;
            }
            else
            {
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, 10,game_data._instance.get_t_language ("pvp_gui.cs_348_76"));//猎人大会刷新消耗
            }
            m_msg = net_http._instance.parse_packet<protocol.game.smsg_pvp_view>(mes.m_byte);
            show_huoban();
 
        }
        else if (mes.m_opcode == opclient_t.CMSG_PVP_FIGHT_END)
        {
            protocol.game.smsg_pvp_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_pvp_fight_end>(mes.m_byte);
            battle_logic_ex._instance.set_pvp_fight_end(_msg);
            this.gameObject.SetActive(false);
            sys._instance.m_game_state = "buttle";
			sys._instance.load_scene_ex("ts_chapter01");
            sys._instance.m_self.m_t_player.pvp_num--;
            remove_all();
        }
        else if (mes.m_opcode == opclient_t.CMSG_PVP_BUY)
        {
            int jewel = 0;
            for (int i = sys._instance.m_self.m_t_player.pvp_buy_num + 1; i <= sys._instance.m_self.m_t_player.pvp_buy_num + num; i++)
            {
                s_t_price t_price = game_data._instance.get_t_price(i);
                jewel += t_price.hunter_assembly;
            }
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, jewel,game_data._instance.get_t_language ("pvp_gui.cs_373_75"));//购买猎人大会挑战次数消耗
            sys._instance.m_self.m_t_player.pvp_num += num;
            sys._instance.m_self.m_t_player.pvp_buy_num += num;
            string s = game_data._instance.get_t_language ("bingyuan_gui.cs_776_19");//获得
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, "", "[ffffc0]" + "挑战次数" + "[ffd000] + " + num.ToString());
            m_attack_num.text = game_data._instance.get_t_language ("pvp_gui.cs_378_32") + "：" + sys._instance.m_self.m_t_player.pvp_num + "";//今日挑战次数
        }
        else if (mes.m_opcode == opclient_t.CMSG_PVP_RANK)
        {
            m_pvp_rank.GetComponent<pvp_reward_gui>()._msg = net_http._instance.parse_packet<protocol.game.smsg_rank_view>(mes.m_byte);
            m_pvp_rank.GetComponent<pvp_reward_gui>().reset_hurt(1);
            m_pvp_rank.SetActive(true);
            pvp_reward_gui _rank = m_pvp_rank.GetComponent<pvp_reward_gui>();
            if (_rank._msg.rank_list == null)
            {
                m_rank.transform.Find("wei").gameObject.SetActive(true);
                m_rank.transform.Find("rank").gameObject.SetActive(false);
            }
            else if (_rank.get_rank() != -1)
            {
                m_rank.transform.Find("wei").gameObject.SetActive(false);
                m_rank.transform.Find("rank").gameObject.SetActive(true);
                m_rank.transform.Find("rank").GetComponent<UILabel>().text = _rank.get_rank() + 1 + "";
            }
            else
            {
                m_rank.transform.Find("wei").gameObject.SetActive(true);
                m_rank.transform.Find("rank").gameObject.SetActive(false);
            }
        }
    }
    void show_qiansan()
    {
        for (int i = 0; i < m_lierens.Count; i++)
        {
            m_lierens[i].SetActive(false);
        }
        m_attack_num.gameObject.SetActive(false);
        m_refresh_num.gameObject.SetActive(false);
        m_rank_qiansan.SetActive(true);
        for (int i = 0; i < m_qiansan.Count; i++)
        {
            sys._instance.remove_child(m_qiansan[i].transform.Find("icon").gameObject);
            m_qiansan[i].transform.Find("qu").GetComponent<UILabel>().text = "";
            m_qiansan[i].transform.Find("name").GetComponent<UILabel>().text = "";
            m_qiansan[i].transform.Find("Sprite").gameObject.SetActive(false);
            sys._instance.get_chenghao(0, m_qiansan[i].transform.Find("chenhao").gameObject);
        }
        if (pvp_task_gui.is_effect())
        {
            m_task_button.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_task_button.transform.Find("effect").gameObject.SetActive(false);
        }
        for (int i = 0; i < m_msg.player_guids.Count; i++)
        {
            GameObject _obj = m_qiansan[i];
            _obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_msg.player_achieves[i]) + m_msg.player_names[i].ToString();
            GameObject m_icon = _obj.transform.Find("icon").gameObject;
            sys._instance.remove_child(m_icon);
            GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_msg.player_templates[i], m_msg.player_achieves[i], m_msg.player_vips[i],m_msg.player_nalflags[i]);
            _obj1.transform.parent = m_icon.transform;
            _obj1.transform.localScale = new Vector3(1, 1, 1);
            _obj1.transform.localPosition = new Vector3(0, 0, 0);
            _obj.transform.Find("qu").GetComponent<UILabel>().text = "";
            sys._instance.get_chenghao(m_msg.player_chenghaos[i], _obj.transform.Find("chenhao").gameObject);

            m_qiansan[i].transform.Find("Sprite").gameObject.SetActive(true);
        }
        m_cur_jifen.text = sys._instance.m_self.m_t_player.pvp_total + "";

        pvp_reward_gui _rank = m_pvp_rank.GetComponent<pvp_reward_gui>();
        if (_rank._msg.rank_list == null)
        {
            m_rank.transform.Find("wei").gameObject.SetActive(true);
            m_rank.transform.Find("rank").gameObject.SetActive(false);
        }
        else if (_rank.get_rank() != -1)
        {
            m_rank.transform.Find("wei").gameObject.SetActive(false);
            m_rank.transform.Find("rank").gameObject.SetActive(true);
            m_rank.transform.Find("rank").GetComponent<UILabel>().text = _rank.get_rank() + 1 + "";
            sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on, m_rank.transform.Find("chenhao").gameObject);
        }
        else
        {
            m_rank.transform.Find("wei").gameObject.SetActive(true);
            m_rank.transform.Find("rank").gameObject.SetActive(false);
            sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on, m_rank.transform.Find("chenhao").gameObject);
        }
    }

    void show_huoban()
    {
        remove_all();
        for (int i = 0; i < m_msg.player_templates.Count; i++)
        {
            add_card((int)m_msg.player_templates[i], m_msg.player_dress[i], m_msg.player_guanghuans[i], i);
            m_units[i].GetComponent<unit>().set_bh(m_msg.player_wins[int.Parse(m_units[i].name)] * 5);
        }
        if (m_msg.player_guids.Count != 0)
        {
            cam();
        }
        refresh_pvp();
    }

    public static bool is_lieren()
    {
        int weekday = System.Convert.ToInt32(timer.dtnow().DayOfWeek);
        if (weekday >= 1 && weekday <= 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool is_effect()
    {
        if ((pvp_task_gui.is_effect() || (is_lieren() && sys._instance.m_self.m_t_player.pvp_num > 0)) 
            && game_data._instance.get_t_huodong_sub(100001).level <= sys._instance.m_self.m_t_player.level
        )
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void remove_all()
    {
        for (int i = 0; i < m_units.Count; i++)
        {
            GameObject.Destroy(m_units[i]);
        }

        m_units.Clear();
    }

    void cam()
    {
        m_cam = GameObject.Find("Camera");

        if (m_cam == null)
        {
            return;
        }

        float _max_height = 0;
        GameObject _max_object = null;

        for (int i = 0; i < m_units.Count; i++)
        {
            GameObject _unit = m_units[i];

            if (_unit.GetComponent<unit>().m_name_height + 0.3f > _max_height)
            {
                _max_height = _unit.GetComponent<unit>().m_name_height + 0.3f;
                _max_object = _unit;
            }
        }

        List<GameObject> _units = new List<GameObject>();

        for (int i = 0; i < m_units.Count; i++)
        {
            GameObject _unit = m_units[i];

            if (_unit != _max_object)
            {
                _units.Add(_unit);
            }
        }
        if (_max_object != null)
        {
            _max_object.transform.localPosition = new Vector3(0, 0, -5.78f);
            _max_object.transform.localEulerAngles = new Vector3(0, 180, 0);
        }
      
        try
        {

            if (m_units.Count > 1)
            {
                _units[0].transform.localPosition = new Vector3(-2.35f, 0, -5);
                _units[0].transform.localEulerAngles = new Vector3(0, 150, 0);

            }
            if (m_units.Count > 2)
            {
                _units[1].transform.localPosition = new Vector3(2.35f, 0, -5);
                _units[1].transform.localEulerAngles = new Vector3(0, 210, 0);
            }
        }
        catch (UnityException)
        {
 
        }
        for (int i = 0; i < m_lierens.Count; i++)
        {
            m_lierens[i].SetActive(false);
        }

        for (int i = 0; i < m_units.Count; i++)
        {
            if(m_units[i].transform.localPosition.x == 0)
            {
                int temp = int.Parse(m_units[i].name);
                m_lierens[temp].SetActive(true);
                m_click_objs[1].name = m_units[i].name;
                string s2 = game_data._instance.get_name_color(m_msg.player_achieves[temp]) + m_msg.player_names[temp];
                s2 = s2.Replace("\0", "");
                string s1 = s2;
                m_click_objs[1].transform.Find("info").transform.Find("name").GetComponent<UILabel>().text = s1;
                m_click_objs[1].name = m_units[i].name;
                m_click_objs[1].transform.Find("info").transform.Find("attack").GetComponent<UILabel>().text = game_data._instance.get_t_language ("arena_item.cs_42_42") + "：" +//战斗力
                   sys._instance.value_to_wan(m_msg.player_bfs[i]);
                m_click_objs[1].transform.Find("info").Find("liren").Find("Sprite").GetComponent<UISprite>().spriteName = game_data._instance.get_t_resource(22).smallicon;
                m_click_objs[1].transform.Find("info").Find("liren").GetComponent<UILabel>().text = game_data._instance.get_t_resource(22).namecolor + m_msg.player_points[temp] + "";
            }
            else if (m_units[i].transform.localPosition.x < 0)
            {
                int temp = int.Parse(m_units[i].name);
                m_lierens[temp].SetActive(true);
                m_click_objs[0].name = m_units[i].name;
                string s2 = game_data._instance.get_name_color(m_msg.player_achieves[temp]) + m_msg.player_names[temp];
                s2 = s2.Replace("\0", "");
                string s1 = s2;
                m_click_objs[0].transform.Find("info").transform.Find("name").GetComponent<UILabel>().text = s1;
                m_click_objs[0].name = m_units[i].name;
                m_click_objs[0].transform.Find("info").transform.Find("attack").GetComponent<UILabel>().text = game_data._instance.get_t_language ("arena_item.cs_42_42") + "：" +//战斗力
                   sys._instance.value_to_wan(m_msg.player_bfs[temp]);
                m_click_objs[0].transform.Find("info").Find("liren").Find("Sprite").GetComponent<UISprite>().spriteName = game_data._instance.get_t_resource(22).smallicon;
                m_click_objs[0].transform.Find("info").Find("liren").GetComponent<UILabel>().text = game_data._instance.get_t_resource(22).namecolor + m_msg.player_points[temp] + "";
            }
            else if (m_units[i].transform.localPosition.x > 0)
            {
                int temp = int.Parse(m_units[i].name);
                m_lierens[temp].SetActive(true);
                m_click_objs[2].name = m_units[i].name;
                string s2 = game_data._instance.get_name_color(m_msg.player_achieves[temp]) + m_msg.player_names[temp];
                s2 = s2.Replace("\0", "");
                string s1 = s2;
                 m_click_objs[2].transform.Find("info").transform.Find("name").GetComponent<UILabel>().text = s1;
                 m_click_objs[2].name = m_units[i].name;
                 m_click_objs[2].transform.Find("info").transform.Find("attack").GetComponent<UILabel>().text = game_data._instance.get_t_language ("arena_item.cs_42_42") + "：" +//战斗力
                    sys._instance.value_to_wan(m_msg.player_bfs[temp]);
                 m_click_objs[2].transform.Find("info").Find("liren").Find("Sprite").GetComponent<UISprite>().spriteName = game_data._instance.get_t_resource(22).smallicon;
                 m_click_objs[2].transform.Find("info").Find("liren").GetComponent<UILabel>().text = game_data._instance.get_t_resource(22).namecolor + m_msg.player_points[temp] + "";
            }
        }
        Vector3 _pos = m_cam.transform.localPosition;

        _pos.y = _max_height * 0.55f;
        _pos.z = _max_height * -1.4f;

        m_cam.transform.localPosition = _pos;
    }

    void add_card(int id, int dress, int guanghuan, int pos)
    {
        GameObject _unit = sys._instance.create_class(id, dress, guanghuan);
        _unit.name = pos + "";
        m_units.Add(_unit);
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void OnDisable()
    {
        remove_all();
        Destroy(this.gameObject);
    }
}
