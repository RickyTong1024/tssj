using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bag_gui : MonoBehaviour, IMessage
{

    public GameObject m_bag_grid;
    public GameObject m_use_button;
    public GameObject m_sell_button;
    public GameObject m_tuse_button;
    public GameObject m_qiangwang_button;
    public GameObject m_pet_button;
    public UISprite m_sell_icon;
    public GameObject m_equiphecheng_button;
    public GameObject m_jiyinhechneg_button;
    public GameObject m_property_root;
    public GameObject m_property_icon;
    public GameObject m_property_name;
    public GameObject m_property_des;
    public GameObject m_property_price;
    public GameObject m_empty;
    public GameObject m_level;
    public GameObject m_price_panel;
    public GameObject m_use_items_gui;
    public GameObject m_icon_root;
    public GameObject m_tabs_panel;
    public GameObject m_duihuan_panel;
    public GameObject m_scro;
    public UIToggle m_jiyin_toogle;
    public UIToggle m_item_toogle;
    public GameObject m_hc_effect;
    public UIToggle m_jiyin;
    public GameObject m_toggle_panel;
    public UIToggle m_toggle_blue;
    public UIToggle m_toggle_red;
    public UIToggle m_toggle_zi;
    public List<int> m_color_ids = new List<int>();
    public GameObject m_sell_yijian;
    public UIPanel m_sell_scrollow;
    public UIToggle m_red_equip_toggle;
    public GameObject m_guanghuan;
    public GameObject m_pet_duihuan;
    public UIToggle m_pet_sp_toggle;
    public int m_color_price = 0;
    private s_t_item m_t_item;
    private int m_item_num;
    private dhc.equip_t m_t_equip;
    private dhc.treasure_t m_t_treasure;
    private bool uflag = false;
    private int num = 0;
    public GameObject m_select;
    private int m_add_gold;
    private int m_add_mw;
    private s_t_itemhecheng t_itemhecheng;
    bool m_flag = false;
    List<GameObject> m_icons = new List<GameObject>();
    List<GameObject> m_equip_icons = new List<GameObject>();
    List<GameObject> m_treasure_icons = new List<GameObject>();
    public GameObject m_effect_sui;
    public GameObject m_effect_ji;
    public GameObject m_effect_pet;
    public GameObject m_bag_buy_gui;
    public UIToggle m_equip_toggle;
    public UIToggle m_baowu_toggle;
    public GameObject m_red_equip_effect;
    public GameObject m_guanghuan_effect;
    public GameObject m_pet_effect;
    int type = 0;
    int select = 0;
    public List<UIToggle> m_toggles;
    public UILabel m_label_type3;
    public UILabel m_label_type4;
    public GameObject m_label_type5;
    public UISprite m_buy_icon;
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
        type = 0;
    }
    public void reset()
    {
        m_t_item = null;
        m_t_equip = null;
        m_t_treasure = null;
        m_tabs_panel.SetActive(true);
        m_duihuan_panel.SetActive(false);
        update_ui(true);
    }

    public static bool is_effect()
    {
        return can_jiyin() || can_suipian() || can_pet() || effect();
    }

    public static bool can_jiyin()
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
            if (_item == null)
            {
                continue;
            }
            int _num = sys._instance.m_self.get_item_num((uint)_item.id);
            if (_item.type == 3001)
            {
                if (_num >= _item.def_2)
                {
                    if (sys._instance.m_self.get_card_id(_item.def_1) == null)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public static bool can_suipian()
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
            if (_item == null)
            {
                continue;
            }
            int _num = sys._instance.m_self.get_item_num((uint)_item.id);
            if (_item.type == 7001)
            {
                if (_num >= _item.def_2)
                {

                    return true;
                }
            }
        }
        return false;
    }

    public static bool can_pet()
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
            if (_item == null)
            {
                continue;
            }
            int _num = sys._instance.m_self.get_item_num((uint)_item.id);
            if (_item.type == 10001)
            {
                if (_num >= _item.def_2)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_ITEM_SELL && this.gameObject.activeSelf)
        {
            int _num = m_item_num;
            sys._instance.m_self.remove_item((uint)m_t_item.id, _num, m_t_item.name + game_data._instance.get_t_language("bag_gui.cs_183_75"));//物品使用消耗

            if (m_t_item.type == 3001)
            {
                sys._instance.m_self.add_att(e_player_attr.player_jjc_point, _num * m_t_item.gold);
                if (sys._instance.m_self.get_item_num((uint)m_t_item.id) == 0)
                {
                    m_t_item = null;

                }
                get_jiyin();

            }
            else if (m_t_item.type == 9001)
            {
                sys._instance.m_self.add_att(e_player_attr.player_huiyi_point, _num * m_t_item.gold);
                if (sys._instance.m_self.get_item_num((uint)m_t_item.id) == 0)
                {
                    m_t_item = null;

                }
                get_huiyi();
            }

            else if (m_t_item.type == 7001)
            {
                sys._instance.m_self.add_att(e_player_attr.player_hj, _num * m_t_item.gold);
                if (sys._instance.m_self.get_item_num((uint)m_t_item.id) == 0)
                {
                    m_t_item = null;

                }
                get_equip_sp();
            }
            else
            {
                sys._instance.m_self.add_att(e_player_attr.player_gold, _num * m_t_item.gold, game_data._instance.get_t_language("bag_gui.cs_220_93"));//背包出售物品获得
                if (sys._instance.m_self.get_item_num((uint)m_t_item.id) == 0)
                {
                    m_t_item = null;

                }
                update_ui(false);

            }

        }
        if (message.m_opcode == opclient_t.CMSG_ROLE_DUIHUAN && this.gameObject.activeSelf)
        {
            if (m_flag)
            {
                protocol.game.smsg_role_duihuan _msg = net_http._instance.parse_packet<protocol.game.smsg_role_duihuan>(message.m_byte);
                if (sys._instance.m_self.has_card(_msg.role.template_id))
                {
                    return;
                }
                sys._instance.m_self.remove_item((uint)m_t_item.id, m_t_item.def_2, m_t_item.name + game_data._instance.get_t_language("bag_gui.cs_241_88"));//物兑换消耗
                sys._instance.m_self.add_card(_msg.role, false);
                int _num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
                if (_num <= 0)
                {
                    m_t_item = null;
                }
                get_jiyin();
                m_jiyin_toogle.value = true;
                this.gameObject.SetActive(false);
                s_message _mes1 = new s_message();
                _mes1.m_type = "show_bag_gui";

                ccard _card = sys._instance.m_self.get_card_guid(_msg.role.guid);
                s_message _message = new s_message();
                _message.m_type = "show_zhaomu_shuxing";
                _message.m_object.Add(_card);
                _message.m_object.Add(_mes1);
                cmessage_center._instance.add_message(_message);
                m_flag = false;

            }
        }
        if (message.m_opcode == opclient_t.CMSG_PET_DUIHUAN && this.gameObject.activeSelf)
        {
            protocol.game.smsg_pet_duihuan _msg = net_http._instance.parse_packet<protocol.game.smsg_pet_duihuan>(message.m_byte);
            sys._instance.m_self.remove_item((uint)m_t_item.id, m_t_item.def_2, m_t_item.name + game_data._instance.get_t_language("bag_gui.cs_268_87"));//宠物兑换消耗
            sys._instance.m_self.add_pet(_msg.pet, true);
            int _num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
            if (_num <= 0)
            {
                m_t_item = null;
            }
            get_pet_sp();
            m_pet_sp_toggle.value = true;
        }
        if (message.m_opcode == opclient_t.CMSG_ITEM_APPLY && this.gameObject.activeSelf)
        {
            if (m_t_item.use == 2)
            {
                return;
            }
            protocol.game.smsg_item_apply _msg = net_http._instance.parse_packet<protocol.game.smsg_item_apply>(message.m_byte);

            if (!uflag)
            {
                if (m_t_item.def_2 != 2)
                {
                    sys._instance.m_self.m_is_chou = true;
                }
            }
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i], uflag);
            }

            int j = 0;
            for (int c = 0; c < _msg.types.Count; c++)
            {
                sys._instance.m_self.add_reward(_msg.types[c], _msg.value1s[c], _msg.value2s[c], _msg.value3s[c], uflag, m_t_item.name + game_data._instance.get_t_language("bag_gui.cs_301_124"));//物品使用获得
            }
            for (int i = 0; i < _msg.equips.Count; ++i)
            {
                sys._instance.m_self.add_equip(_msg.equips[i], uflag);
            }
            for (int i = 0; i < _msg.roles.Count; ++i)
            {
                sys._instance.m_self.add_card(_msg.roles[i], uflag);
            }
            for (int i = 0; i < _msg.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_msg.pets[i]);
            }
            sys._instance.m_self.remove_item((uint)m_t_item.id, num, m_t_item.name + game_data._instance.get_t_language("bag_gui.cs_183_75"));//物品使用消耗
            if (_msg.role_ids.Count == 1)
            {
                int sid = _msg.role_ids[0];
                bool flag = false;
                if (_msg.roles.Count == 0)
                {
                    sys._instance.m_self.add_item((uint)ccard.get_fragment_id(sid), 10, false, m_t_item.name + game_data._instance.get_t_language("bag_gui.cs_301_124"));//物品使用获得
                    flag = true;
                }

                s_message _mes2 = new s_message();
                _mes2.m_type = "hide_bag";
                cmessage_center._instance.add_message(_mes2);

                s_message _mes1 = new s_message();
                _mes1.m_type = "show_bag";

                ccard _card = sys._instance.m_self.get_card_id(sid);
                s_message _message = new s_message();
                _message.m_type = "show_zhaomu_shuxing";
                _message.m_object.Add(_card);
                _message.m_object.Add(_mes1);
                _message.m_bools.Add(flag);
                cmessage_center._instance.add_message(_message);
                this.transform.Find("frame_big").GetComponent<frame>().hide();
            }

            int _num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
            if (!uflag)
            {
                if (m_t_item.def_2 != 2)
                {
                    m_use_items_gui.GetComponent<use_items_gui>().init();
                    for (j = 0; j < _msg.types.Count; ++j)
                    {
                        m_use_items_gui.GetComponent<use_items_gui>().types.Add(_msg.types[j]);
                        m_use_items_gui.GetComponent<use_items_gui>().values1.Add(_msg.value1s[j]);
                        m_use_items_gui.GetComponent<use_items_gui>().values2.Add(_msg.value2s[j]);
                        m_use_items_gui.GetComponent<use_items_gui>().values3.Add(_msg.value3s[j]);
                    }
                    m_use_items_gui.SetActive(true);

                }
            }
            if (_num <= 0)
            {
                m_t_item = null;
            }
            update_ui(false);
            sys._instance.remove_child(m_icon_root);
        }
        else if (message.m_opcode == opclient_t.CMSG_EQUIP_SUIPIAN)
        {
            sys._instance.m_self.remove_item((uint)m_t_item.id, m_t_item.def_2, m_t_item.name + game_data._instance.get_t_language("bag_gui.cs_374_86"));//兑换装备消耗
            protocol.game.smsg_equip_suipian _msg = net_http._instance.parse_packet<protocol.game.smsg_equip_suipian>(message.m_byte);
            sys._instance.m_self.add_equip(_msg.equip);
            int _num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
            if (_num <= 0)
            {
                m_t_item = null;
            }
            get_equip_sp();
        }
        else if (message.m_opcode == opclient_t.CMSG_ITEM_SELL_ALL)
        {
            for (int i = 0; i < m_color_ids.Count; i++)
            {
                sys._instance.m_self.remove_item((uint)m_color_ids[i], sys._instance.m_self.get_item_num((uint)m_color_ids[i]), game_data._instance.get_item(m_color_ids[i]).name + game_data._instance.get_t_language("bag_gui.cs_388_178"));//物品出售消耗
            }

            if (m_t_item.type == 3001)
            {
                if (sys._instance.m_self.get_item_num((uint)m_t_item.id) == 0)
                {
                    m_t_item = null;
                }
                sys._instance.m_self.add_att(e_player_attr.player_jjc_point, m_color_price);
                get_jiyin();
            }
            else if (m_t_item.type == 7001)
            {
                if (sys._instance.m_self.get_item_num((uint)m_t_item.id) == 0)
                {
                    m_t_item = null;
                }
                sys._instance.m_self.add_att(e_player_attr.player_hj, m_color_price);
                get_equip_sp();
            }
            else if (m_t_item.type == 9001)
            {
                if (sys._instance.m_self.get_item_num((uint)m_t_item.id) == 0)
                {
                    m_t_item = null;
                }
                sys._instance.m_self.add_att(e_player_attr.player_huiyi_point, m_color_price);
                get_huiyi();
            }
        }
        else if (message.m_opcode == opclient_t.CMSG_ITEM_HECHENG && t_itemhecheng != null)
        {
            protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success>(message.m_byte);
            if (_msg.success)
            {
                sys._instance.m_self.add_reward(t_itemhecheng.type, t_itemhecheng.item_id, t_itemhecheng.item_num, 0, game_data._instance.get_t_language("bag_gui.cs_424_102"));//物品合成
                for (int i = 0; i < t_itemhecheng.cl_type.Count; ++i)
                {
                    sys._instance.m_self.remove_item((uint)t_itemhecheng.cl_id[i], t_itemhecheng.cl_num[i], game_data._instance.get_t_language("bag_gui.cs_427_92"));//物品合成消耗
                }
                s_message _message = new s_message();
                _message.m_type = "refresh_bag_duihuan_gui";
                cmessage_center._instance.add_message(_message);
                t_itemhecheng = null;
            }
        }
    }
    void IMessage.message(s_message message)
    {
        if (message.m_type == "show_bag_item_property")
        {
            m_t_item = game_data._instance.get_item((int)message.m_ints[0]);
            m_select.transform.parent = ((GameObject)message.m_object[0]).transform;
            m_select.transform.localPosition = Vector3.zero;
            show_item_property();
        }
        else if (message.m_type == "sui_pian_he_cheng")
        {

            protocol.game.cmsg_role_suipian _msg = new protocol.game.cmsg_role_suipian();
            _msg.item_id = (uint)m_t_item.id;
            m_flag = true;
            net_http._instance.send_msg<protocol.game.cmsg_role_suipian>(opclient_t.CMSG_ROLE_DUIHUAN, _msg);
        }
        else if (message.m_type == "select_item_gui")
        {
            protocol.game.cmsg_item_apply msg = new protocol.game.cmsg_item_apply();
            msg.item_id = (uint)m_t_item.id;
            msg.item_index = (int)message.m_ints[0];
            msg.item_count = (int)message.m_ints[1];
            num = msg.item_count;
            uflag = true;
            net_http._instance.send_msg<protocol.game.cmsg_item_apply>(opclient_t.CMSG_ITEM_APPLY, msg);
        }
        else if (message.m_type == "pet_he_cheng")
        {
            protocol.game.cmsg_pet_duihuan _msg = new protocol.game.cmsg_pet_duihuan();
            _msg.item_id = m_t_item.id;
            net_http._instance.send_msg<protocol.game.cmsg_pet_duihuan>(opclient_t.CMSG_PET_DUIHUAN, _msg);
        }
        else if (message.m_type == "show_bag_select")
        {
            m_select.transform.parent = ((GameObject)message.m_object[0]).transform;
            m_t_equip = sys._instance.m_self.get_equip_guid((ulong)message.m_ints[0]);
            GameObject _icon = icon_manager._instance.create_equip_icon(m_t_equip.template_id);

            sys._instance.remove_child(m_property_icon);
            _icon.transform.parent = m_property_icon.transform;
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);

            m_property_name.GetComponent<UILabel>().text = equip.get_equip_name(m_t_equip);
            m_property_des.GetComponent<UILabel>().text = equip.get_equip_value(m_t_equip);
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(true);
            m_sell_button.name = "peiyang";
            m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = game_data._instance.get_t_language("bag_gui.cs_485_89");//培养
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_select.transform.localPosition = Vector3.zero;
        }
        else if (message.m_type == "show_baowu_select")
        {
            m_select.transform.parent = ((GameObject)message.m_object[0]).transform;
            m_t_treasure = sys._instance.m_self.get_treasure_guid((ulong)message.m_ints[0]);
            GameObject _icon = icon_manager._instance.create_treasure_icon(m_t_treasure.template_id);

            sys._instance.remove_child(m_property_icon);
            _icon.transform.parent = m_property_icon.transform;
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);

            m_property_name.GetComponent<UILabel>().text = treasure.get_treasure_name(m_t_treasure);
            m_property_des.GetComponent<UILabel>().text = treasure.get_treasure_value(m_t_treasure.template_id, m_t_treasure.enhance, m_t_treasure.jilian);
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(true);
            m_sell_button.name = "peiyang";
            m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = game_data._instance.get_t_language("bag_gui.cs_485_89");//培养
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_select.transform.localPosition = Vector3.zero;
            show_treasure_property();
        }
        if (message.m_type == "bag_sell_item")
        {
            if ((int)message.m_ints[1] > 0)
            {
                protocol.game.cmsg_item_sell _msg = new protocol.game.cmsg_item_sell();
                _msg.item_id = (uint)m_t_item.id;
                _msg.item_num = (int)message.m_ints[1];
                m_item_num = _msg.item_num;
                net_http._instance.send_msg<protocol.game.cmsg_item_sell>(opclient_t.CMSG_ITEM_SELL, _msg);
            }
        }
        else if (message.m_type == "equipsp_he_cheng")
        {
            protocol.game.cmsg_equip_suipian _msg = new protocol.game.cmsg_equip_suipian();
            _msg.item_id = (uint)m_t_item.id;

            net_http._instance.send_msg<protocol.game.cmsg_equip_suipian>(opclient_t.CMSG_EQUIP_SUIPIAN, _msg);
        }
        else if (message.m_type == "refresh_bag_gui1")
        {
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            int _num1 = sys._instance.m_self.get_item_num((uint)m_t_item.id);
            if (_num1 <= 0)
            {
                m_t_item = null;
            }
            update_ui(false);
            sys._instance.remove_child(m_icon_root);
        }
        else if (message.m_type == "refresh_bag_duihuan_gui")
        {
            if (!this.gameObject.activeSelf)
            {
                return;
            }
            exchange_gui();
        }
        else if (message.m_type == "sell_item_all")
        {
            if (m_color_ids.Count > 0)
            {
                protocol.game.cmsg_item_sell_all _msg = new protocol.game.cmsg_item_sell_all();
                for (int i = 0; i < m_color_ids.Count; i++)
                {
                    _msg.item_ids.Add((uint)m_color_ids[i]);
                }
                net_http._instance.send_msg<protocol.game.cmsg_item_sell_all>(opclient_t.CMSG_ITEM_SELL_ALL, _msg);
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language("bag_gui.cs_570_71"));//请选择要出售的物品
            }

        }
        else if (message.m_type == "buy_guanghuan_gui")
        {
            t_itemhecheng = message.m_object[0] as s_t_itemhecheng;
            protocol.game.cmsg_item_apply _net_msg = new protocol.game.cmsg_item_apply();
            _net_msg.item_id = (uint)t_itemhecheng.id;
            _net_msg.item_count = 1;
            net_http._instance.send_msg<protocol.game.cmsg_item_apply>(opclient_t.CMSG_ITEM_HECHENG, _net_msg);
        }
    }

    public static int comp(ccard x, ccard y)
    {
        //上阵 颜色 阶级 等级
        bool fx = false;
        for (int j = 0; j < sys._instance.m_self.m_t_player.zhenxing.Count; ++j)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[j] == x.get_guid())
            {
                fx = true;
                break;
            }
        }
        bool fy = false;
        for (int j = 0; j < sys._instance.m_self.m_t_player.zhenxing.Count; ++j)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[j] == y.get_guid())
            {
                fy = true;
                break;
            }
        }
        if (fx == true && fy == false)
        {
            return -1;
        }
        else if (fx == false && fy == true)
        {
            return 1;
        }
        else
        {
            s_t_class _class_x = x.m_t_class;
            s_t_class _class_y = y.m_t_class;
            if (_class_x.color > _class_y.color)
            {
                return -1;
            }
            else if (_class_x.color < _class_y.color)
            {
                return 1;
            }
            else if (x.get_pinzhi() > y.get_pinzhi())
            {
                return -1;
            }
            else if (x.get_pinzhi() < y.get_pinzhi())
            {
                return 1;
            }
            else if (x.get_glevel() > y.get_glevel())
            {
                return -1;
            }
            else if (x.get_glevel() < y.get_glevel())
            {
                return 1;
            }
            else if (x.get_jlevel() > y.get_jlevel())
            {
                return -1;
            }
            else if (x.get_jlevel() < y.get_jlevel())
            {
                return 1;
            }
            else if (x.get_level() > y.get_level())
            {
                return -1;
            }
            else if (x.get_level() < y.get_level())
            {
                return 1;
            }
            else if (x.get_template_id() < y.get_template_id())
            {
                return -1;
            }
            else if (x.get_template_id() > y.get_template_id())
            {
                return 1;
            }
        }
        return 0;
    }

    void get_equip()
    {
        update_effect();
        m_select.transform.parent = m_bag_grid.transform.parent;

        if (m_bag_grid.transform.parent.GetComponent<SpringPanel>() != null)
        {
            m_bag_grid.transform.parent.GetComponent<SpringPanel>().enabled = false;
        }
        m_bag_grid.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        m_bag_grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_bag_grid);

        m_sell_scrollow.SetRect(0, 0, 378, 490);
        m_sell_scrollow.transform.parent.localPosition = new Vector3(0, 0, 0);
        m_sell_yijian.gameObject.SetActive(false);
        m_equip_icons.Clear();
        List<dhc.equip_t> m_equips = new List<dhc.equip_t>();
        m_equips.Clear();
        for (int i = 0; i < sys._instance.m_self.get_equip_num(); i++)
        {
            dhc.equip_t _equip = sys._instance.m_self.get_equip_index(i);
            s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
            m_equips.Add(_equip);
        }
        m_equips.Sort(equip.comp);
        int c = 0;
        for (int i = 0; i < m_equips.Count; i++, c++)
        {
            GameObject _icon = icon_manager._instance.create_equip_icon_ex(m_equips[i], true);
            _icon.transform.parent = m_bag_grid.transform;
            _icon.transform.localPosition = new Vector3((i % 4) * 95 - 142,
                                                        -i / 4 * 95 + 193, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.AddComponent<UIDragScrollView>().scrollView = m_bag_grid.GetComponent<UIScrollView>();
            _icon.GetComponent<equip_icon>().m_out_message = "show_bag_select";
            m_equip_icons.Add(_icon);
        }
        if (c > 0)
        {
            if (m_t_equip == null)
            {
                m_t_equip = m_equips[0];
            }
            for (int i = 0; i < m_equip_icons.Count; i++)
            {
                if (m_t_equip.guid == m_equip_icons[i].GetComponent<equip_icon>().m_equip.guid)
                {
                    m_select.SetActive(true);
                    m_select.GetComponent<UISprite>().enabled = true;
                    m_select.transform.parent = m_equip_icons[i].transform;
                    m_select.transform.localPosition = Vector3.zero;
                    break;
                }
            }
            m_sell_button.GetComponent<BoxCollider>().enabled = true;
            m_use_button.GetComponent<BoxCollider>().enabled = true;
            show_equip_property();
        }
        else
        {
            sys._instance.remove_child(m_property_icon);

            m_sell_button.GetComponent<BoxCollider>().enabled = false;
            m_use_button.GetComponent<BoxCollider>().enabled = false;
            m_tuse_button.SetActive(false);
            m_property_name.GetComponent<UILabel>().text = "----";
            m_property_price.GetComponent<UILabel>().text = "----";
            m_property_des.GetComponent<UILabel>().text = "";
            m_level.GetComponent<UILabel>().text = "";
        }
        if (m_equips.Count > 0)
        {
            m_empty.gameObject.SetActive(false);

        }
        else
        {
            m_empty.gameObject.SetActive(true);
            m_select.SetActive(false);
        }

    }

    void get_treasure()
    {
        update_effect();
        m_select.transform.parent = m_bag_grid.transform.parent;

        m_sell_scrollow.SetRect(0, 0, 378, 490);
        m_sell_scrollow.transform.parent.localPosition = new Vector3(0, 0, 0);

        if (m_bag_grid.transform.parent.GetComponent<SpringPanel>() != null)
        {
            m_bag_grid.transform.parent.GetComponent<SpringPanel>().enabled = false;
        }
        m_bag_grid.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        m_bag_grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_bag_grid);
        m_sell_yijian.gameObject.SetActive(false);

        m_treasure_icons.Clear();
        List<dhc.treasure_t> m_tresures = new List<dhc.treasure_t>();
        m_tresures.Clear();
        for (int i = 0; i < sys._instance.m_self.get_treasure_num(); i++)
        {
            dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(i);
            s_t_baowu t_treasure = game_data._instance.get_t_baowu(_treasure.template_id);
            m_tresures.Add(_treasure);
        }
        m_tresures.Sort(treasure.comp);
        int c = 0;
        for (int i = 0; i < m_tresures.Count; i++, c++)
        {
            GameObject _icon = icon_manager._instance.create_treasure_icon_ex(m_tresures[i], true);
            _icon.transform.parent = m_bag_grid.transform;
            _icon.transform.localPosition = new Vector3((i % 4) * 95 - 142,
                                                        -i / 4 * 95 + 193, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.AddComponent<UIDragScrollView>().scrollView = m_bag_grid.GetComponent<UIScrollView>();
            _icon.GetComponent<treasure_icon>().m_out_message = "show_baowu_select";
            m_treasure_icons.Add(_icon);

        }
        if (c > 0)
        {
            if (m_t_treasure == null)
            {
                m_t_treasure = m_tresures[0];
            }
            for (int i = 0; i < m_treasure_icons.Count; i++)
            {
                if (m_t_treasure.guid == m_treasure_icons[i].GetComponent<treasure_icon>().m_treasure.guid)
                {
                    m_select.SetActive(true);
                    m_select.GetComponent<UISprite>().enabled = true;
                    m_select.transform.parent = m_treasure_icons[i].transform;
                    m_select.transform.localPosition = Vector3.zero;
                    break;
                }
            }
            m_sell_button.GetComponent<BoxCollider>().enabled = true;
            m_use_button.GetComponent<BoxCollider>().enabled = true;
            show_treasure_property();
        }
        else
        {
            sys._instance.remove_child(m_property_icon);

            m_sell_button.GetComponent<BoxCollider>().enabled = false;
            m_use_button.GetComponent<BoxCollider>().enabled = false;
            m_tuse_button.SetActive(false);
            m_property_name.GetComponent<UILabel>().text = "----";
            m_property_price.GetComponent<UILabel>().text = "----";
            m_property_des.GetComponent<UILabel>().text = "";
            m_level.GetComponent<UILabel>().text = "";
        }
        if (m_tresures.Count > 0)
        {
            m_empty.gameObject.SetActive(false);

        }
        else
        {
            m_empty.gameObject.SetActive(true);
            m_select.SetActive(false);
        }
    }

    int sort(int x, int y)
    {
        s_t_item _x = game_data._instance.get_item(x);
        s_t_item _y = game_data._instance.get_item(y);
        int x_num = sys._instance.m_self.get_item_num((uint)x);
        int y_num = sys._instance.m_self.get_item_num((uint)y);
        s_t_equip _x_equip = game_data._instance.get_t_equip(_x.def_1);
        s_t_equip _y_equip = game_data._instance.get_t_equip(_y.def_1);
        if (x_num >= _x.def_2 && y_num < _y.def_2)
        {
            return -1;
        }
        else if (x_num < _x.def_2 && y_num >= _y.def_2)
        {
            return 1;
        }
        else if (_x.font_color != _y.font_color)
        {
            return -(_x.font_color - _y.font_color);
        }
        else
        {
            return (_x_equip.type - _y_equip.type);
        }
    }

    int sort1(int x, int y)
    {
        s_t_item _x = game_data._instance.get_item(x);
        s_t_item _y = game_data._instance.get_item(y);
        int x_num = sys._instance.m_self.get_item_num((uint)x);
        int y_num = sys._instance.m_self.get_item_num((uint)y);
        s_t_baowu _x_baowu = game_data._instance.get_t_baowu(_x.def_1);
        s_t_baowu _y_baowu = game_data._instance.get_t_baowu(_y.def_1);
        if (x_num >= _x.def_2 && y_num < _y.def_2)
        {
            return -1;
        }
        else if (x_num < _x.def_2 && y_num >= _y.def_2)
        {
            return 1;
        }
        else if (_x.font_color != _y.font_color)
        {
            return -(_x.font_color - _y.font_color);
        }
        else
        {
            return (_x_baowu.type - _y_baowu.type);
        }
    }

    void get_equip_sp()
    {
        type = 4;
        update_effect();
        m_select.transform.parent = m_bag_grid.transform.parent;
        if (m_bag_grid.transform.parent.GetComponent<SpringPanel>() != null)
        {
            m_bag_grid.transform.parent.GetComponent<SpringPanel>().enabled = false;
        }
        m_bag_grid.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        m_bag_grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_bag_grid);

        m_sell_scrollow.SetRect(0, 0, 378, 400);
        m_sell_scrollow.transform.parent.localPosition = new Vector3(0, 44, 0);

        m_sell_yijian.gameObject.SetActive(true);
        List<int> _ids = new List<int>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
            if (_item == null)
            {
                continue;
            }
            if (_item.type == 7001)
            {
                _ids.Add(_item.id);
            }

        }
        int c = 0;
        _ids.Sort(sort);
        m_icons.Clear();
        for (int i = 0; i < _ids.Count; i++, c++)
        {
            s_t_item _item = game_data._instance.get_item(_ids[i]);
            GameObject _icon = icon_manager._instance.create_item_icon_ex(_ids[i], sys._instance.m_self.get_item_num((uint)_ids[i]), _item.def_2);
            _icon.transform.parent = m_bag_grid.transform;
            _icon.transform.localPosition = new Vector3((i % 4) * 95 - 142,
                                                        -i / 4 * 95 + 152, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.AddComponent<UIDragScrollView>().scrollView = m_bag_grid.GetComponent<UIScrollView>();
            _icon.GetComponent<item_icon>().m_out_message = "show_bag_item_property";
            m_icons.Add(_icon);
        }
        if (c > 0)
        {
            if (m_t_item == null)
            {
                m_t_item = game_data._instance.get_item(_ids[0]);
            }
            for (int i = 0; i < m_icons.Count; i++)
            {
                if (m_t_item.id == m_icons[i].GetComponent<item_icon>().m_item_id)
                {
                    m_select.SetActive(true);
                    m_select.GetComponent<UISprite>().enabled = true;
                    m_select.transform.parent = m_icons[i].transform;
                    m_select.transform.localPosition = Vector3.zero;
                    break;
                }
            }
            m_sell_button.GetComponent<BoxCollider>().enabled = true;
            m_use_button.GetComponent<BoxCollider>().enabled = true;
            show_item_property();
        }
        else
        {
            sys._instance.remove_child(m_property_icon);
            m_sell_button.GetComponent<BoxCollider>().enabled = false;
            m_use_button.GetComponent<BoxCollider>().enabled = false;
            m_sell_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_property_name.GetComponent<UILabel>().text = "----";
            m_property_price.GetComponent<UILabel>().text = "----";
            m_property_des.GetComponent<UILabel>().text = "";
            m_level.GetComponent<UILabel>().text = "";
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_price_panel.SetActive(false);
        }
        if (_ids.Count > 0)
        {
            m_empty.gameObject.SetActive(false);
        }
        else
        {
            m_empty.gameObject.SetActive(true);
            m_select.SetActive(false);
        }

    }

    void get_treasure_sp()
    {
        update_effect();
        m_select.transform.parent = m_bag_grid.transform.parent;
        if (m_bag_grid.transform.parent.GetComponent<SpringPanel>() != null)
        {
            m_bag_grid.transform.parent.GetComponent<SpringPanel>().enabled = false;
        }
        m_bag_grid.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        m_bag_grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_bag_grid);
        m_sell_scrollow.SetRect(0, 0, 378, 490);
        m_sell_scrollow.transform.parent.localPosition = new Vector3(0, 0, 0);

        m_sell_yijian.gameObject.SetActive(false);
        List<int> _ids = new List<int>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
            if (_item == null)
            {
                continue;
            }
            if (_item.type == 10001)
            {
                _ids.Add(_item.id);
            }

        }
        int c = 0;
        _ids.Sort(sort1);
        m_icons.Clear();
        for (int i = 0; i < _ids.Count; i++, c++)
        {
            s_t_item _item = game_data._instance.get_item(_ids[i]);
            GameObject _icon = icon_manager._instance.create_item_icon_ex(_ids[i], sys._instance.m_self.get_item_num((uint)_ids[i]), 0);
            _icon.transform.parent = m_bag_grid.transform;
            _icon.transform.localPosition = new Vector3((i % 4) * 95 - 142,
                                                        -i / 4 * 95 + 193, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.AddComponent<UIDragScrollView>().scrollView = m_bag_grid.GetComponent<UIScrollView>();
            _icon.GetComponent<item_icon>().m_out_message = "show_bag_item_property";
            m_icons.Add(_icon);
        }
        if (c > 0)
        {
            if (m_t_item == null)
            {
                m_t_item = game_data._instance.get_item(_ids[0]);
            }
            for (int i = 0; i < m_icons.Count; i++)
            {
                if (m_t_item.id == m_icons[i].GetComponent<item_icon>().m_item_id)
                {
                    m_select.SetActive(true);
                    m_select.GetComponent<UISprite>().enabled = true;
                    m_select.transform.parent = m_icons[i].transform;
                    m_select.transform.localPosition = Vector3.zero;
                    break;
                }
            }
            m_sell_button.GetComponent<BoxCollider>().enabled = true;
            m_use_button.GetComponent<BoxCollider>().enabled = true;
            show_item_property();
        }
        else
        {
            sys._instance.remove_child(m_property_icon);
            m_sell_button.GetComponent<BoxCollider>().enabled = false;
            m_use_button.GetComponent<BoxCollider>().enabled = false;
            m_sell_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_property_name.GetComponent<UILabel>().text = "----";
            m_property_price.GetComponent<UILabel>().text = "----";
            m_property_des.GetComponent<UILabel>().text = "";
            m_level.GetComponent<UILabel>().text = "";
        }
        if (_ids.Count > 0)
        {
            m_empty.gameObject.SetActive(false);
        }
        else
        {
            m_empty.gameObject.SetActive(true);
            m_select.SetActive(false);
        }

    }

    int comp_huiyi(int x, int y)
    {
        int x_x = sys._instance.m_self.get_huiyi_xuyao(x);
        int y_y = sys._instance.m_self.get_huiyi_xuyao(y);
        if (x_x != y_y)
        {
            return -(x_x - y_y);
        }
        else
        {
            return -(game_data._instance.get_item(x).font_color - game_data._instance.get_item(y).font_color);
        }
    }

    int comp(int x, int y)
    {
        s_t_item x_i = game_data._instance.get_item(x);
        s_t_item y_i = game_data._instance.get_item(y);
        ccard x_c = sys._instance.m_self.get_card_id(x_i.def_1);
        ccard y_c = sys._instance.m_self.get_card_id(y_i.def_1);
        if ((x_c == null && sys._instance.m_self.get_item_num((uint)x) >= x_i.def_2) && !(y_c == null && sys._instance.m_self.get_item_num((uint)y) >= y_i.def_2))
        {
            return -1;
        }
        else if (!(x_c == null && sys._instance.m_self.get_item_num((uint)x) >= x_i.def_2) && (y_c == null && sys._instance.m_self.get_item_num((uint)y) >= y_i.def_2))
        {
            return 1;
        }
        else
        {
            if (x_c == null && y_c != null)
            {
                return 1;
            }
            else if (x_c != null && y_c == null)
            {
                return -1;
            }
            else
            {
                return -(x_i.font_color - y_i.font_color);
            }
        }
    }

    void get_jiyin()
    {
        type = 3;
        update_effect();
        m_select.transform.parent = m_bag_grid.transform.parent;
        if (m_bag_grid.transform.parent.GetComponent<SpringPanel>() != null)
        {
            m_bag_grid.transform.parent.GetComponent<SpringPanel>().enabled = false;
        }
        m_bag_grid.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        m_bag_grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_bag_grid);

        m_sell_scrollow.SetRect(0, 0, 378, 400);
        m_sell_scrollow.transform.parent.localPosition = new Vector3(0, 44, 0);
        m_sell_yijian.gameObject.SetActive(true);

        m_icons.Clear();
        List<int> _ids = new List<int>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
            if (_item == null)
            {
                continue;
            }
            if (_item.type == 3001)
            {
                _ids.Add(_item.id);
            }

        }
        _ids.Sort(comp);
        int c = 0;
        for (int i = 0; i < _ids.Count; i++, c++)
        {
            s_t_item _item = game_data._instance.get_item(_ids[i]);
            bool tp = false;
            if (_item.type == 3001)
            {
                if (sys._instance.m_self.get_card_id(_item.def_1) != null)
                {
                    tp = true;
                }
            }
            GameObject _icon = icon_manager._instance.create_item_icon_ex(_ids[i], sys._instance.m_self.get_item_num((uint)_ids[i]), _item.def_2, tp);
            _icon.transform.parent = m_bag_grid.transform;
            _icon.transform.localPosition = new Vector3((i % 4) * 95 - 142,
                                                        -i / 4 * 95 + 152, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.AddComponent<UIDragScrollView>().scrollView = m_bag_grid.GetComponent<UIScrollView>();
            _icon.GetComponent<item_icon>().m_out_message = "show_bag_item_property";
            m_icons.Add(_icon);

        }
        if (c > 0)
        {
            if (m_t_item == null)
            {
                m_t_item = game_data._instance.get_item(_ids[0]);
            }
            for (int i = 0; i < m_icons.Count; i++)
            {
                if (m_t_item.id == m_icons[i].GetComponent<item_icon>().m_item_id)
                {
                    m_select.SetActive(true);
                    m_select.GetComponent<UISprite>().enabled = true;
                    m_select.transform.parent = m_icons[i].transform;
                    m_select.transform.localPosition = Vector3.zero;
                    break;
                }
            }
            m_sell_button.GetComponent<BoxCollider>().enabled = true;
            m_use_button.GetComponent<BoxCollider>().enabled = true;
            m_sell_button.GetComponent<UISprite>().set_enable(true);
            show_item_property();
        }
        else
        {
            sys._instance.remove_child(m_property_icon);
            m_sell_button.GetComponent<BoxCollider>().enabled = false;
            m_use_button.GetComponent<BoxCollider>().enabled = false;
            m_tuse_button.SetActive(false);
            m_property_name.GetComponent<UILabel>().text = "----";
            m_property_price.GetComponent<UILabel>().text = "----";
            m_property_des.GetComponent<UILabel>().text = "";
            m_level.GetComponent<UILabel>().text = "";
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_price_panel.SetActive(false);
        }
        if (_ids.Count > 0)
        {
            m_empty.gameObject.SetActive(false);
        }
        else
        {
            m_empty.gameObject.SetActive(true);
            m_select.SetActive(false);
        }
    }

    void get_item(bool rf)
    {
        update_effect();
        m_select.transform.parent = m_bag_grid.transform.parent;
        if (m_bag_grid.transform.parent.GetComponent<SpringPanel>() != null)
        {
            m_bag_grid.transform.parent.GetComponent<SpringPanel>().enabled = false;
        }
        m_bag_grid.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        m_bag_grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_bag_grid);
        m_sell_scrollow.SetRect(0, 0, 378, 490);
        m_sell_scrollow.transform.parent.localPosition = new Vector3(0, 0, 0);
        m_sell_yijian.gameObject.SetActive(false);
        m_select.SetActive(true);
        m_select.GetComponent<UISprite>().enabled = true;
        List<int> items = new List<int>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; ++i)
        {
            int _item_id = (int)sys._instance.m_self.m_t_player.item_ids[i];
            items.Add(_item_id);
        }
        items.Sort();
        int c = 0;
        m_icons.Clear();
        for (int i = 0; i < items.Count; i++)
        {
            int _item_id = items[i];
            s_t_item _item = game_data._instance.get_item(_item_id);
            if (_item == null)
            {
                continue;
            }
            if (_item.type == 3001 || _item.type == 3002 || _item.type == 7001
                || _item.type == 10001 || _item.type == 11001 || _item.type == 6001 || _item.type == 9001)
            {
                continue;
            }

            int _num = sys._instance.m_self.get_item_num((uint)_item_id);
            GameObject _icon = icon_manager._instance.create_item_icon_ex(_item_id, _num);
            _icon.transform.parent = m_bag_grid.transform;
            _icon.transform.localPosition = new Vector3((c % 4) * 95 - 142,
                                                        -c / 4 * 95 + 193, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.AddComponent<UIDragScrollView>().scrollView = m_bag_grid.GetComponent<UIScrollView>();
            _icon.GetComponent<item_icon>().m_out_message = "show_bag_item_property";
            m_icons.Add(_icon);
            c++;
        }

        if (c > 0)
        {
            if (m_t_item == null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    int _item_id = items[i];
                    s_t_item _item = game_data._instance.get_item(_item_id);
                    if (_item.type != 3001 && _item.type != 3002 && _item.type != 7001 && _item.type != 10001 && _item.type != 11001 && _item.type != 6001)
                    {
                        m_t_item = game_data._instance.get_item(items[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < m_icons.Count; i++)
            {
                if (m_t_item.id == m_icons[i].GetComponent<item_icon>().m_item_id)
                {
                    m_select.SetActive(true);
                    m_select.GetComponent<UISprite>().enabled = true;
                    m_select.transform.parent = m_icons[i].transform;
                    m_select.transform.localPosition = Vector3.zero;
                    break;
                }
            }
            m_sell_button.GetComponent<BoxCollider>().enabled = true;
            m_sell_button.GetComponent<UISprite>().set_enable(true);
            m_use_button.GetComponent<BoxCollider>().enabled = true;
            show_item_property();
        }
        else
        {
            sys._instance.remove_child(m_property_icon);

            m_sell_button.GetComponent<BoxCollider>().enabled = false;
            m_sell_button.SetActive(false);
            m_use_button.GetComponent<BoxCollider>().enabled = false;
            m_tuse_button.SetActive(false);
            m_property_name.GetComponent<UILabel>().text = "----";
            m_property_price.GetComponent<UILabel>().text = "----";
            m_property_des.GetComponent<UILabel>().text = "";
            m_price_panel.SetActive(false);
            m_level.GetComponent<UILabel>().text = "";
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_price_panel.SetActive(false);
        }

        if (c > 0)
        {
            m_empty.gameObject.SetActive(false);
        }
        else
        {
            m_empty.gameObject.SetActive(true);
            m_select.SetActive(false);
        }
    }

    void get_pet_sp()
    {
        update_effect();
        m_select.transform.parent = m_bag_grid.transform.parent;
        if (m_bag_grid.transform.parent.GetComponent<SpringPanel>() != null)
        {
            m_bag_grid.transform.parent.GetComponent<SpringPanel>().enabled = false;
        }
        m_bag_grid.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        m_bag_grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_bag_grid);
        m_sell_scrollow.SetRect(0, 0, 378, 490);
        m_sell_scrollow.transform.parent.localPosition = new Vector3(0, 0, 0);
        m_sell_yijian.gameObject.SetActive(false);
        m_select.SetActive(true);
        m_select.GetComponent<UISprite>().enabled = true;
        m_icons.Clear();
        List<int> _ids = new List<int>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
            if (_item == null)
            {
                continue;
            }
            if (_item.type == 10001)
            {
                _ids.Add(_item.id);
            }

        }
        _ids.Sort(pet_comp);
        int c = 0;
        for (int i = 0; i < _ids.Count; i++, c++)
        {
            s_t_item _item = game_data._instance.get_item(_ids[i]);
            bool tp = false;
            if (_item.type == 10001)
            {
                if (sys._instance.m_self.get_card_id(_item.def_1) != null)
                {
                    tp = true;
                }
            }
            GameObject _icon = icon_manager._instance.create_item_icon_ex(_ids[i], sys._instance.m_self.get_item_num((uint)_ids[i]), _item.def_2, tp);
            _icon.transform.parent = m_bag_grid.transform;
            _icon.transform.localPosition = new Vector3((i % 4) * 95 - 142,
                                                        -i / 4 * 95 + 193, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.AddComponent<UIDragScrollView>().scrollView = m_bag_grid.GetComponent<UIScrollView>();
            _icon.GetComponent<item_icon>().m_out_message = "show_bag_item_property";
            m_icons.Add(_icon);

        }
        if (c > 0)
        {
            if (m_t_item == null)
            {
                m_t_item = game_data._instance.get_item(_ids[0]);
            }
            for (int i = 0; i < m_icons.Count; i++)
            {
                if (m_t_item.id == m_icons[i].GetComponent<item_icon>().m_item_id)
                {
                    m_select.SetActive(true);
                    m_select.GetComponent<UISprite>().enabled = true;
                    m_select.transform.parent = m_icons[i].transform;
                    m_select.transform.localPosition = Vector3.zero;
                    break;
                }
            }
            m_sell_button.GetComponent<BoxCollider>().enabled = true;
            m_use_button.GetComponent<BoxCollider>().enabled = true;
            m_sell_button.GetComponent<UISprite>().set_enable(true);
            show_item_property();
        }
        else
        {
            sys._instance.remove_child(m_property_icon);
            m_sell_button.GetComponent<BoxCollider>().enabled = false;
            m_use_button.GetComponent<BoxCollider>().enabled = false;
            m_tuse_button.SetActive(false);
            m_property_name.GetComponent<UILabel>().text = "----";
            m_property_price.GetComponent<UILabel>().text = "----";
            m_property_des.GetComponent<UILabel>().text = "";
            m_level.GetComponent<UILabel>().text = "";
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_price_panel.SetActive(false);
        }
        if (_ids.Count > 0)
        {
            m_empty.gameObject.SetActive(false);
        }
        else
        {
            m_empty.gameObject.SetActive(true);
            m_select.SetActive(false);
        }
    }

    int pet_comp(int x, int y)
    {
        s_t_item x_i = game_data._instance.get_item(x);
        s_t_item y_i = game_data._instance.get_item(y);
        if ((sys._instance.m_self.get_item_num((uint)x) >= x_i.def_2) && !(sys._instance.m_self.get_item_num((uint)y) >= y_i.def_2))
        {
            return -1;
        }
        else if (!(sys._instance.m_self.get_item_num((uint)x) >= x_i.def_2) && (sys._instance.m_self.get_item_num((uint)y) >= y_i.def_2))
        {
            return 1;
        }
        return -(x_i.font_color - y_i.font_color);
    }

    void get_huiyi()
    {
        type = 6;
        update_effect();
        m_select.transform.parent = m_bag_grid.transform.parent;

        if (m_bag_grid.transform.parent.GetComponent<SpringPanel>() != null)
        {
            m_bag_grid.transform.parent.GetComponent<SpringPanel>().enabled = false;
        }
        m_bag_grid.transform.parent.transform.localPosition = new Vector3(0, 0, 0);
        m_bag_grid.transform.parent.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_bag_grid);

        m_sell_scrollow.SetRect(0, 0, 378, 400);
        m_sell_scrollow.transform.parent.localPosition = new Vector3(0, 44, 0);
        m_sell_yijian.gameObject.SetActive(true);


        m_icons.Clear();
        List<int> _ids = new List<int>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
        {
            s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
            if (_item == null)
            {
                continue;
            }
            if (_item.type == 9001)
            {
                _ids.Add(_item.id);
            }

        }
        _ids.Sort(comp_huiyi);
        int c = 0;
        for (int i = 0; i < _ids.Count; i++, c++)
        {
            s_t_item _item = game_data._instance.get_item(_ids[i]);
            GameObject _icon = icon_manager._instance.create_item_icon_ex(_ids[i], sys._instance.m_self.get_item_num((uint)_ids[i]), _item.def_2);
            _icon.transform.parent = m_bag_grid.transform;
            _icon.transform.localPosition = new Vector3((i % 4) * 95 - 142,
                                                        -i / 4 * 95 + 152, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.AddComponent<UIDragScrollView>().scrollView = m_bag_grid.GetComponent<UIScrollView>();
            _icon.GetComponent<item_icon>().m_out_message = "show_bag_item_property";
            m_icons.Add(_icon);
        }
        if (c > 0)
        {
            if (m_t_item == null)
            {
                m_t_item = game_data._instance.get_item(_ids[0]);
            }
            for (int i = 0; i < m_icons.Count; i++)
            {
                if (m_t_item.id == m_icons[i].GetComponent<item_icon>().m_item_id)
                {
                    m_select.SetActive(true);
                    m_select.GetComponent<UISprite>().enabled = true;
                    m_select.transform.parent = m_icons[i].transform;
                    m_select.transform.localPosition = Vector3.zero;
                    break;
                }
            }
            m_sell_button.GetComponent<BoxCollider>().enabled = true;
            m_use_button.GetComponent<BoxCollider>().enabled = true;
            m_sell_button.GetComponent<UISprite>().set_enable(true);
            show_item_property();
        }
        else
        {
            sys._instance.remove_child(m_property_icon);
            m_sell_button.GetComponent<BoxCollider>().enabled = false;
            m_use_button.GetComponent<BoxCollider>().enabled = false;
            m_tuse_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_property_name.GetComponent<UILabel>().text = "----";
            m_property_price.GetComponent<UILabel>().text = "----";
            m_property_des.GetComponent<UILabel>().text = "";
            m_level.GetComponent<UILabel>().text = "";
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_price_panel.SetActive(false);
        }
        if (_ids.Count > 0)
        {
            m_empty.gameObject.SetActive(false);
        }
        else
        {
            m_empty.gameObject.SetActive(true);
            m_select.SetActive(false);
        }

    }

    void show_equip_property()
    {
        if (m_t_equip == null)
        {
            return;
        }
        GameObject _icon = icon_manager._instance.create_equip_icon(m_t_equip.template_id);

        sys._instance.remove_child(m_property_icon);
        _icon.transform.parent = m_property_icon.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
        m_price_panel.SetActive(false);
        m_property_name.GetComponent<UILabel>().text = equip.get_equip_name(m_t_equip);
        m_property_des.GetComponent<UILabel>().text = equip.get_equip_value(m_t_equip);
        m_price_panel.SetActive(false);
        m_level.SetActive(false);
        m_sell_button.SetActive(true);
        m_sell_button.GetComponent<UISprite>().set_enable(true);
        m_qiangwang_button.SetActive(false);
        m_sell_button.name = "peiyang";
        m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = game_data._instance.get_t_language("bag_gui.cs_485_89");//培养
        m_use_button.SetActive(false);
        m_tuse_button.SetActive(false);
        m_select.transform.localPosition = Vector3.zero;
    }

    void show_item_property()
    {
        m_sell_button.name = "sell";
        m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = game_data._instance.get_t_language("bag_gui.cs_1598_79");//出售
        m_sell_button.GetComponent<UISprite>().set_enable(true);
        m_sell_button.GetComponent<BoxCollider>().enabled = true;
        m_sell_button.SetActive(true);
        m_sell_button.GetComponent<UISprite>().set_enable(true);
        m_price_panel.SetActive(true);
        m_price_panel.transform.Find("icon").GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(1);
        m_buy_icon.spriteName = sys._instance.get_res_samall_icon(1);
        m_qiangwang_button.SetActive(false);
        m_jiyinhechneg_button.SetActive(false);
        m_pet_button.SetActive(false);
        m_equiphecheng_button.SetActive(false);
        if (m_t_item == null)
        {
            return;
        }

        m_property_root.SetActive(true);

        int _num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
        GameObject _icon = icon_manager._instance.create_item_icon(m_t_item.id, _num);

        sys._instance.remove_child(m_property_icon);
        _icon.transform.parent = m_property_icon.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);

        m_property_name.GetComponent<UILabel>().text = ccard.get_color_name(m_t_item.name + " × " + _num, m_t_item.font_color);
        m_property_des.GetComponent<UILabel>().text = m_t_item.desc;
        if (m_t_item.need_level > 0)
        {
            int nlevel = m_t_item.need_level;
            if (nlevel > sys._instance.m_self.m_t_player.level)
            {
                m_level.SetActive(true);
                m_level.GetComponent<UILabel>().text = "[ff0000]" + string.Format(game_data._instance.get_t_language("bag_gui.cs_1633_70"), nlevel.ToString());//{0}级可使用
            }
            else
            {
                m_level.SetActive(true);
                m_level.GetComponent<UILabel>().text = "[00ff00]" + string.Format(game_data._instance.get_t_language("bag_gui.cs_1633_70"), nlevel.ToString());//{0}级可使用
            }
        }
        else
        {
            m_level.GetComponent<UILabel>().text = "";
        }
        string color = sys._instance.get_res_color(1);
        if (m_t_item.type == 3001)
        {
            color = sys._instance.get_res_color(5);
        }
        if (m_t_item.type == 7001)
        {
            color = sys._instance.get_res_color(6);
        }
        m_property_price.GetComponent<UILabel>().text = color + (m_t_item.gold * _num).ToString();
        m_bag_buy_gui.GetComponent<bag_buy_num_gui>().m_color = color;
        if (m_t_item.use != 1)
        {
            if (m_t_item.type == 3001)
            {
                int m_sp_num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
                ccard _card = sys._instance.m_self.get_card_id(m_t_item.def_1);
                if (m_sp_num >= m_t_item.def_2 && _card == null)
                {
                    m_jiyinhechneg_button.SetActive(true);
                    m_sell_button.SetActive(false);
                    m_qiangwang_button.SetActive(false);
                    m_equiphecheng_button.SetActive(false);
                    m_sell_button.GetComponent<UISprite>().set_enable(true);
                    m_sell_button.GetComponent<BoxCollider>().enabled = true;
                    m_price_panel.SetActive(true);
                    m_price_panel.transform.Find("icon").GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(5);
                    m_buy_icon.spriteName = sys._instance.get_res_samall_icon(5);
                }
                else
                {
                    m_qiangwang_button.SetActive(true);
                    m_sell_button.SetActive(false);
                    m_price_panel.SetActive(true);
                    m_price_panel.transform.Find("icon").GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(5);
                    m_buy_icon.spriteName = sys._instance.get_res_samall_icon(5);
                }

            }
            if (m_t_item.type == 9001)
            {

                {
                    m_property_price.GetComponent<UILabel>().text = sys._instance.get_res_color(21) + (m_t_item.gold * _num).ToString();
                    m_bag_buy_gui.GetComponent<bag_buy_num_gui>().m_color = sys._instance.get_res_color(21);
                    m_qiangwang_button.SetActive(true);
                    m_sell_button.SetActive(false);
                    m_price_panel.SetActive(true);
                    m_price_panel.transform.Find("icon").GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(21);
                    m_buy_icon.spriteName = sys._instance.get_res_samall_icon(21);
                }

            }
            if (m_t_item.type == 7001)
            {
                int m_sp_num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
                if (m_sp_num >= m_t_item.def_2)
                {
                    m_equiphecheng_button.SetActive(true);
                    m_jiyinhechneg_button.SetActive(false);
                    m_sell_button.SetActive(false);
                    m_price_panel.SetActive(true);
                    m_price_panel.transform.Find("icon").GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(6);
                    m_buy_icon.spriteName = sys._instance.get_res_samall_icon(6);
                }
                else
                {
                    m_qiangwang_button.SetActive(true);
                    m_sell_button.SetActive(false);
                    m_price_panel.SetActive(true);
                    m_price_panel.transform.Find("icon").GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(6);
                    m_buy_icon.spriteName = sys._instance.get_res_samall_icon(6);
                }
            }
            if (m_t_item.type == 10001)
            {
                int m_sp_num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
                if (m_sp_num < m_t_item.def_2)
                {
                    m_pet_button.SetActive(false);
                    m_sell_button.name = "qianwang";
                    m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = game_data._instance.get_t_language("bag_gui.cs_1734_82");//获取
                    m_sell_button.SetActive(true);
                    m_price_panel.SetActive(false);
                    m_sell_button.GetComponent<UISprite>().set_enable(true);
                }
                else
                {
                    m_pet_button.SetActive(true);
                    m_sell_button.SetActive(false);
                    m_price_panel.SetActive(false);
                }
            }
            m_use_button.SetActive(false);
            m_sell_button.transform.localPosition = new Vector3(0, -254.0f, 0);
            m_tuse_button.SetActive(false);
            if (m_t_item.use == 2)
            {
                m_sell_button.name = "use";
                m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = game_data._instance.get_t_language("bag_gui.cs_1753_93");//使用
                m_sell_button.SetActive(true);
                m_sell_button.GetComponent<UISprite>().set_enable(true);
                m_use_button.SetActive(false);
                m_tuse_button.SetActive(false);
                m_qiangwang_button.SetActive(false);
                m_price_panel.SetActive(false);
            }
        }
        else
        {
            m_sell_button.SetActive(false);
            m_price_panel.SetActive(false);
            int num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
            if (m_t_item.def_2 == 0 || m_t_item.def_2 == 2)
            {
                m_sell_button.name = "use";
                m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = game_data._instance.get_t_language("bag_gui.cs_1753_93");//使用
                m_sell_button.SetActive(true);
                m_sell_button.GetComponent<UISprite>().set_enable(true);
                m_use_button.SetActive(false);
                m_tuse_button.SetActive(false);
                m_qiangwang_button.SetActive(false);
            }
            else
            {
                m_use_button.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("bag_gui.cs_1780_91"), 1);//开{0}个
                if (num > 1)
                {
                    m_tuse_button.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("bag_gui.cs_1780_91"), num.ToString());//开{0}个
                    if (num > 10)
                    {
                        m_tuse_button.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("bag_gui.cs_1780_91"), 10);//开{0}个
                    }
                    if (m_t_item.id == 10010062)
                    {
                        int tre_num = (sys._instance.m_self.get_max_treasure_num() - sys._instance.m_self.get_un_treasure_num()) / 5;
                        if (tre_num > num)
                        {
                            tre_num = num;
                        }
                        if (tre_num > 10)
                        {
                            tre_num = 10;
                        }
                        if (tre_num <= 0)
                        {
                            tre_num = 1;
                        }
                        if (tre_num <= 10)
                        {
                            m_tuse_button.transform.Find("Label").GetComponent<UILabel>().text =
                                string.Format(game_data._instance.get_t_language("bag_gui.cs_1780_91"), tre_num);//开{0}个
                        }
                    }
                    m_tuse_button.SetActive(true);
                    m_use_button.transform.localPosition = new Vector3(-72, -254, 0);
                    m_sell_button.SetActive(false);
                    m_use_button.SetActive(true);
                    m_qiangwang_button.SetActive(false);
                }
                else
                {
                    m_tuse_button.SetActive(false);
                    m_sell_button.GetComponent<UISprite>().set_enable(true);
                    m_use_button.SetActive(false);
                    m_sell_button.name = "use";
                    m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("bag_gui.cs_1780_91"), 1);//开{0}个
                    m_sell_button.SetActive(true);
                    m_qiangwang_button.SetActive(false);
                }
            }
        }
    }

    public void show_gui(int type, bool flag = false)
    {
        m_tabs_panel.SetActive(true);
        m_duihuan_panel.SetActive(false);
        if (type == 3)
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_jiyin();
        }
        else if (type == 2)
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_equip();
        }
        else if (type == 5)
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_treasure();
        }
        else if (type == 6)
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            m_item_toogle.value = true;
            m_pet_duihuan.GetComponent<UIToggle>().value = true;
            m_tabs_panel.SetActive(false);
            m_duihuan_panel.SetActive(true);
            select = 3;
            exchange_gui();
        }
        else if (type == 7)
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            m_item_toogle.value = true;
            m_guanghuan.GetComponent<UIToggle>().value = true;
            m_tabs_panel.SetActive(false);
            m_duihuan_panel.SetActive(true);
            select = 2;
            exchange_gui();
        }
    }

    IEnumerator set_toggle()
    {
        yield return new WaitForSeconds(0.4f);
        m_jiyin.value = true;
    }
    void show_treasure_property()
    {
        if (m_t_treasure == null)
        {
            return;
        }
        GameObject _icon = icon_manager._instance.create_treasure_icon(m_t_treasure.template_id);
        m_qiangwang_button.SetActive(false);
        m_equiphecheng_button.SetActive(false);
        m_jiyinhechneg_button.SetActive(false);
        m_pet_button.SetActive(false);
        sys._instance.remove_child(m_property_icon);
        _icon.transform.parent = m_property_icon.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
        m_price_panel.SetActive(false);
        m_property_name.GetComponent<UILabel>().text = treasure.get_treasure_name(m_t_treasure);
        m_property_des.GetComponent<UILabel>().text = treasure.get_treasure_value(m_t_treasure.template_id, m_t_treasure.enhance, m_t_treasure.jilian);
        m_price_panel.SetActive(false);
        m_qiangwang_button.SetActive(false);
        m_equiphecheng_button.SetActive(false);
        m_jiyinhechneg_button.SetActive(false);
        m_sell_button.GetComponent<UISprite>().set_enable(true);
        m_level.SetActive(false);
        m_sell_button.SetActive(true);
        m_sell_button.name = "peiyang";
        m_sell_button.transform.Find("choushou").GetComponent<UILabel>().text = game_data._instance.get_t_language("bag_gui.cs_485_89");//培养
        m_use_button.SetActive(false);
        m_tuse_button.SetActive(false);
        m_select.transform.localPosition = Vector3.zero;
        if (m_t_treasure.template_id == 12001 || m_t_treasure.template_id == 13001)
        {
            m_sell_button.GetComponent<UISprite>().set_enable(false);
        }
        else
        {
            m_sell_button.GetComponent<UISprite>().set_enable(true);
        }
    }

    public void click(GameObject obj)
    {
        if (obj.transform.name == "peiyang")
        {
            if (m_t_equip != null && m_t_equip.guid != 0)
            {
                root_gui._instance.show_equip_detail(m_t_equip, 3, this.gameObject, false);
                List<dhc.equip_t> _equips = new List<dhc.equip_t>();
                _equips.Add(m_t_equip);
                root_gui._instance.add_equip(_equips, true);
            }
            if (m_t_treasure != null && m_t_treasure.guid != 0)
            {
                root_gui._instance.show_treasure_detail(m_t_treasure, 3, this.gameObject, false);
                List<dhc.treasure_t> _treasures = new List<dhc.treasure_t>();
                _treasures.Add(m_t_treasure);
                root_gui._instance.add_treasure(_treasures, true);
            }
        }
        else if (obj.name == "qianwang")
        {
            s_message message = new s_message();
            message.m_type = "show_cl_gui";
            message.m_ints.Add(m_t_item.id);
            cmessage_center._instance.add_message(message);
            sys._instance.m_message_type.Add("show_bag");
        }
        else if (obj.transform.name == "sell")
        {
            s_message _message = new s_message();
            _message.m_type = "bag_sell_item";
            _message.m_ints.Add(m_t_item.id);
            m_bag_buy_gui.SetActive(true);
            m_bag_buy_gui.GetComponent<bag_buy_num_gui>().reset(m_t_item.id, _message);
        }
        else if (obj.transform.name == "dh")
        {
            s_message _message = new s_message();
            _message.m_type = "sui_pian_he_cheng";
            cmessage_center._instance.add_message(_message);
            return;
        }
        else if (obj.name == "dh_equip")
        {
            if (sys._instance.m_self.bag_full())
            {
                return;
            }
            s_message _message = new s_message();
            _message.m_type = "equipsp_he_cheng";
            _message.m_ints.Add((int)m_t_item.id);
            cmessage_center._instance.add_message(_message);
            return;
        }

        else if (obj.transform.name == "dh_pet")
        {
            s_message _message = new s_message();
            _message.m_type = "pet_he_cheng";
            cmessage_center._instance.add_message(_message);
            return;
        }

        if (obj.transform.name == "use")
        {
            if (m_t_item.use == 2)
            {
                int num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
                if (num > 0)
                {
                    root_gui._instance.show_tili_dialog_box(m_t_item.id);
                    return;
                }
            }
            if (m_t_item.def_2 != 2)
            {
                if (m_t_item.need_level > sys._instance.m_self.m_t_player.level)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bag_gui.cs_2039_75"));//等级不足
                    return;
                }
                uflag = false;
                if (m_t_item.def_1 <= 9)
                {
                    uflag = true;
                }
                if (m_t_item.def_2 == 3 || m_t_item.def_2 == 4)
                {
                    uflag = true;
                }
                num = 1;
                if (m_t_item.id == 10010062)
                {
                    int tre_num = (sys._instance.m_self.get_max_treasure_num() - sys._instance.m_self.get_un_treasure_num()) / 5;

                    if (tre_num <= 0)
                    {
                        string tishi = game_data._instance.get_t_language("arena.cs_104_45");//提示
                        string _des = game_data._instance.get_t_language("bag_gui.cs_2059_38");//您的饰品携带数量已达上限，是否前往进行饰品强化或者精炼
                        s_message _message = new s_message();
                        _message.m_type = "show_buzheng";
                        root_gui._instance.show_select_dialog_box(tishi, _des, _message);
                        return;
                    }
                }
                protocol.game.cmsg_item_apply _msg = new protocol.game.cmsg_item_apply();
                _msg.item_id = (uint)m_t_item.id;
                _msg.item_count = num;
                net_http._instance.send_msg<protocol.game.cmsg_item_apply>(opclient_t.CMSG_ITEM_APPLY, _msg);
            }
            else
            {
                if (m_t_item.need_level > sys._instance.m_self.m_t_player.level)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bag_gui.cs_2039_75"));//等级不足
                    return;
                }
                root_gui._instance.show_select_item_gui(m_t_item.id, game_data._instance.get_t_itemstore(m_t_item.def_1).rewards, "select_item_gui");
            }

        }

        if (obj.transform.name == "tuse")
        {
            if (m_t_item.need_level > sys._instance.m_self.m_t_player.level)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bag_gui.cs_2039_75"));//等级不足
                return;
            }
            uflag = false;
            if (m_t_item.def_2 == 3 || m_t_item.def_2 == 4)
            {
                uflag = true;
            }
            num = sys._instance.m_self.get_item_num((uint)m_t_item.id);
            protocol.game.cmsg_item_apply _msg = new protocol.game.cmsg_item_apply();
            _msg.item_id = (uint)m_t_item.id;
            if (num >= 10)
            {
                num = 10;
                _msg.item_count = num;
            }
            else
            {
                _msg.item_count = num;
            }
            if (m_t_item.id == 10010062)
            {
                int tre_num = (sys._instance.m_self.get_max_treasure_num() - sys._instance.m_self.get_un_treasure_num()) / 5;
                if (tre_num <= 0)
                {
                    string tishi = game_data._instance.get_t_language("arena.cs_104_45");//提示
                    string _des = game_data._instance.get_t_language("bag_gui.cs_2059_38");//您的饰品携带数量已达上限，是否前往进行饰品强化或者精炼
                    s_message _message = new s_message();
                    _message.m_type = "show_buzheng";
                    root_gui._instance.show_select_dialog_box(tishi, _des, _message);
                    return;
                }
                else
                {
                    if (num > tre_num)
                    {
                        num = tre_num;
                        _msg.item_count = tre_num;
                    }
                }
            }
            net_http._instance.send_msg<protocol.game.cmsg_item_apply>(opclient_t.CMSG_ITEM_APPLY, _msg);
        }
        if (obj.transform.name == "close")
        {
            m_item_toogle.value = true;
            m_red_equip_toggle.value = true;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            transform.Find("frame_big").GetComponent<frame>().hide();
            s_message _message = new s_message();
            _message.m_type = "show_main_gui";
            cmessage_center._instance.add_message(_message);
        }
        if (obj.transform.name == "hide")
        {
            m_use_items_gui.SetActive(false);
        }
        if (obj.name == "wupin")
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_item(true);
        }
        else if (obj.name == "suipian")
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_pet_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_equip_sp();

        }
        else if (obj.name == "jiyin")
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_jiyin();
        }
        else if (obj.name == "equip")
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_equip();
        }
        else if (obj.name == "baowu")
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_treasure();
        }
        else if (obj.name == "baowu_suipian")
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_treasure_sp();
        }
        else if (obj.name == "huiyi")
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_huiyi();
        }
        else if (obj.name == "pet_sp")
        {
            m_price_panel.SetActive(false);
            m_sell_button.SetActive(false);
            m_use_button.SetActive(false);
            m_tuse_button.SetActive(false);
            m_qiangwang_button.SetActive(false);
            m_equiphecheng_button.SetActive(false);
            m_jiyinhechneg_button.SetActive(false);
            m_select.transform.parent = m_bag_grid.transform.parent;
            m_t_item = null;
            m_t_equip = null;
            m_t_treasure = null;
            get_pet_sp();
        }
        else if (obj.name == "exchange")
        {
            m_item_toogle.value = true;
            m_red_equip_toggle.value = true;
            m_tabs_panel.SetActive(false);
            m_duihuan_panel.SetActive(true);
            select = 1;
            exchange_gui();
        }
        else if (obj.name == "back")
        {
            m_item_toogle.value = true;
            m_red_equip_toggle.value = true;
            m_tabs_panel.SetActive(true);
            m_duihuan_panel.SetActive(false);
            reset();
        }
        else if (obj.name == "red_equip")
        {
            select = 1;
            exchange_gui();
        }
        else if (obj.name == "guanghuan")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_guanghuan)
            {
                m_guanghuan.GetComponent<UIToggle>().enabled = false;
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("bag_gui.cs_2299_73"), (int)e_open_level.el_guanghuan));//光环合成{0}级开启
                return;
            }
            m_guanghuan.GetComponent<UIToggle>().enabled = true;
            select = 2;
            exchange_gui();
        }
        else if (obj.name == "pet")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pet)
            {
                m_pet_duihuan.GetComponent<UIToggle>().enabled = false;
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("bag_gui.cs_2299_73"), (int)e_open_level.el_pet));//光环合成{0}级开启
                return;
            }
            m_pet_duihuan.GetComponent<UIToggle>().enabled = true;
            select = 3;
            exchange_gui();
        }
        else if (obj.name == "sell_yijian")
        {
            m_color_ids.Clear();
            m_color_price = 0;
            if (m_t_item == null)
            {
                if (type == 6)
                {
                    root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2326_62"));//[ffc884]主人，您没有不需要的回忆！

                }
                else if (type == 3)
                {
                    root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2331_62"));//[ffc884]主人，您当前没有基因！

                }
                else if (type == 4)
                {
                    root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2336_62"));//[ffc884]主人，您当前没有碎片！

                }
                return;
            }
            if (m_t_item.type != 9001)
            {
                m_toggle_panel.SetActive(true);
                m_toggle_blue.value = false;
                m_toggle_red.value = false;
                m_toggle_zi.value = false;
                if (m_t_item.type == 3001)
                {
                    m_label_type3.text = game_data._instance.get_t_language("bag_gui.cs_2349_41");//一键出售蓝色基因
                    m_label_type4.text = game_data._instance.get_t_language("bag_gui.cs_2350_41");//一键出售紫色基因
                    m_label_type5.SetActive(false);
                    m_label_type4.transform.parent.localPosition = new Vector3(0, -15, 0);
                }
                else if (m_t_item.type == 7001)
                {
                    m_label_type3.text = game_data._instance.get_t_language("bag_gui.cs_2357_41");//一键出售绿色装备碎片
                    m_label_type4.text = game_data._instance.get_t_language("bag_gui.cs_2358_41");//一键出售蓝色装备碎片
                    m_label_type5.SetActive(true);
                    m_label_type4.transform.parent.localPosition = new Vector3(0, 0, 0);
                }
            }
            else
            {
                bool flag = false;
                for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
                    if (_item.type == 9001)
                    {
                        if (0 == sys._instance.m_self.get_huiyi_xuyao(_item.id))
                        {
                            m_color_ids.Add(_item.id);
                            m_color_price += _item.gold * sys._instance.m_self.get_item_num((uint)_item.id); ;
                            flag = true;
                        }
                    }
                }
                if (flag)
                {
                    s_message _mes = new s_message();
                    _mes.m_type = "sell_item_all";
                    string s = "";
                    int temp = 0;
                    for (int i = 0; i < m_color_ids.Count; i++)
                    {
                        temp += sys._instance.m_self.get_item_num((uint)m_color_ids[i]);
                    }
                    if (m_t_item.type == 9001)
                    {
                        s = string.Format(game_data._instance.get_t_language("bag_gui.cs_2395_46"), temp, m_color_price);//你已经选择了[00ffff]{0}[-]个回忆，{nn}总计可获得[00ffff]{1}[-]回忆结晶
                    }
                    root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2398_62"), s, _mes);//一键出售
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2326_62"));//[ffc884]主人，您没有不需要的回忆！
                }

            }
        }
        else if (obj.name == "cancel")
        {
            m_toggle_panel.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "toggle_blue")
        {
            bool flag = false;
            if (m_toggle_blue.value)
            {
                for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
                    if (_item.type == 3001)
                    {
                        if ((_item.font_color == 2)
                      && _item.type == m_t_item.type)//蓝色
                        {
                            m_color_ids.Add(_item.id);
                            flag = true;
                        }

                    }
                    else if (_item.type == 7001)
                    {
                        if ((_item.font_color == 1)
                     && _item.type == m_t_item.type)//蓝色
                        {
                            m_color_ids.Add(_item.id);
                            flag = true;
                        }
                    }

                }
                m_color_price = 0;
                for (int i = 0; i < m_color_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item(m_color_ids[i]);
                    if (_item.type == m_t_item.type)
                    {
                        m_color_price += _item.gold * sys._instance.m_self.get_item_num((uint)_item.id);
                    }
                }
                if (!flag)
                {

                    m_toggle_blue.value = false;
                    if (m_t_item.type == 3001)
                    {
                        root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2459_48"));//[ffc884]主人，您没有该品质的基因！
                    }
                    else if (m_t_item.type == 7001)
                    {
                        root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2463_48"));//[ffc884]主人，您没有该品质的装备碎片！
                    }
                }

            }
            else
            {
                if (m_t_item == null)
                {
                    return;
                }
                for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
                    if (m_t_item.type == 3001)
                    {
                        if ((_item.font_color == 2)
                      && _item.type == m_t_item.type)//蓝色
                        {
                            m_color_ids.Remove(_item.id);
                            flag = true;
                        }
                    }
                    else if (m_t_item.type == 7001)
                    {
                        if ((_item.font_color == 1)
                                            && _item.type == m_t_item.type)//蓝色
                        {
                            m_color_ids.Remove(_item.id);
                            flag = true;
                        }
                    }
                }
                m_color_price = 0;
                for (int i = 0; i < m_color_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item(m_color_ids[i]);
                    if (_item.type == m_t_item.type)
                    {
                        m_color_price += _item.gold * sys._instance.m_self.get_item_num((uint)_item.id);
                    }
                }
            }
        }
        else if (obj.name == "toggle_red")
        {
            bool flag = false;
            if (m_toggle_red.value)
            {
                for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
                    if (_item.type == 3001)
                    {
                        if ((_item.font_color == 3) && _item.type == m_t_item.type)//蓝色
                        {
                            m_color_ids.Add(_item.id);
                            flag = true;
                        }
                    }
                    else if (_item.type == 7001)
                    {
                        if ((_item.font_color == 2) && _item.type == m_t_item.type)//蓝色
                        {
                            m_color_ids.Add(_item.id);
                            flag = true;
                        }
                    }
                }
                m_color_price = 0;
                for (int i = 0; i < m_color_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item(m_color_ids[i]);
                    if (_item.type == m_t_item.type)
                    {
                        m_color_price += _item.gold * sys._instance.m_self.get_item_num((uint)_item.id);
                    }
                }
                if (!flag)
                {
                    m_toggle_red.value = false;
                    if (m_t_item.type == 3001)
                    {
                        root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2459_48"));//[ffc884]主人，您没有该品质的基因！
                    }
                    else if (m_t_item.type == 7001)
                    {
                        root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2463_48"));//[ffc884]主人，您没有该品质的装备碎片！
                    }
                }
            }
            else
            {
                if (m_t_item == null)
                {
                    return;
                }

                for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
                    if (m_t_item.type == 3001)
                    {
                        if ((_item.font_color == 3)
                      && _item.type == m_t_item.type)//蓝色
                        {
                            m_color_ids.Remove(_item.id);
                            flag = true;
                        }
                    }
                    else if (m_t_item.type == 7001)
                    {
                        if ((_item.font_color == 2)
                                            && _item.type == m_t_item.type)//蓝色
                        {
                            m_color_ids.Remove(_item.id);
                            flag = true;
                        }
                    }
                }
                m_color_price = 0;
                for (int i = 0; i < m_color_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item(m_color_ids[i]);
                    if (_item.type == m_t_item.type)
                    {
                        m_color_price += _item.gold * sys._instance.m_self.get_item_num((uint)_item.id);
                    }
                }
            }
        }
        else if (obj.name == "toggle_zi")
        {
            bool flag = false;
            if (m_toggle_zi.value)
            {
                for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);

                    if (_item.type == 7001)
                    {
                        if ((_item.font_color == 3)
                     && _item.type == m_t_item.type)//紫色
                        {
                            m_color_ids.Add(_item.id);
                            flag = true;
                        }

                    }
                }
                m_color_price = 0;
                for (int i = 0; i < m_color_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item(m_color_ids[i]);
                    if (_item.type == m_t_item.type)
                    {
                        m_color_price += _item.gold * sys._instance.m_self.get_item_num((uint)_item.id);
                    }

                }
                if (!flag)
                {
                    m_toggle_zi.value = false;
                    if (m_t_item.type == 7001)
                    {
                        root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2463_48"));//[ffc884]主人，您没有该品质的装备碎片！
                    }
                }
            }
            else
            {
                if (m_t_item == null)
                {
                    return;
                }
                for (int i = 0; i < sys._instance.m_self.m_t_player.item_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item((int)sys._instance.m_self.m_t_player.item_ids[i]);
                    if (m_t_item.type == 7001)
                    {
                        if ((_item.font_color == 3)
                                            && _item.type == m_t_item.type)//紫色
                        {
                            m_color_ids.Remove(_item.id);
                            flag = true;
                        }
                    }
                }
                m_color_price = 0;
                for (int i = 0; i < m_color_ids.Count; i++)
                {
                    s_t_item _item = game_data._instance.get_item(m_color_ids[i]);
                    if (_item.type == m_t_item.type)
                    {
                        m_color_price += _item.gold * sys._instance.m_self.get_item_num((uint)_item.id);
                    }
                }
            }
        }
        else if (obj.name == "queding")
        {
            s_message _mes = new s_message();
            _mes.m_type = "sell_item_all";
            string s = "";
            int temp = 0;
            for (int i = 0; i < m_color_ids.Count; i++)
            {
                temp += sys._instance.m_self.get_item_num((uint)m_color_ids[i]);
            }
            if (m_t_item.type == 3001)
            {
                s = string.Format(game_data._instance.get_t_language("bag_gui.cs_2690_34"), temp, m_color_price);//你已经选择了[00ffff]{0}[-]基因，{nn}总计可获得[00ffff]{1}[-]战魂
            }
            else
            {
                s = string.Format(game_data._instance.get_t_language("bag_gui.cs_2694_34"), temp, m_color_price);//你已经选择了[00ffff]{0}[-]装备碎片，{nn}总计可获得[00ffff]{1}[-]合金
            }
            if (m_color_ids.Count > 0)
            {
                root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2398_62"), s, _mes);//一键出售
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("bag_gui.cs_2704_58"));//[ffc884]主人，您还没有选择物品！
            }
            m_toggle_panel.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "close_toggle")
        {
            m_toggle_panel.transform.Find("frame_big").GetComponent<frame>().hide();
        }
    }

    public void set_toogle_blue()
    {
        m_toggle_blue.value = false;
    }

    public void set_toogle_red()
    {
        m_toggle_red.value = false;
    }

    public void close()
    {
        m_item_toogle.value = true;
        m_red_equip_toggle.value = true;
        m_t_item = null;
        m_t_equip = null;
        m_t_treasure = null;
        transform.Find("frame_big").GetComponent<frame>().hide();
    }

    public void exchange_gui()
    {
        duihuan_effect();
        if (sys._instance.m_self.m_t_player.level < (int)e_open_see.es_guanghuan)
        {
            m_guanghuan.SetActive(false);
        }
        else
        {
            m_guanghuan.SetActive(true);
        }
        if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pet)
        {
            m_pet_duihuan.SetActive(false);
        }
        else
        {
            m_pet_duihuan.SetActive(true);
        }
        m_scro.SetActive(true);
        if (m_scro.GetComponent<SpringPanel>() != null)
        {
            m_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_scro);
        List<s_t_itemhecheng> itemhechengs = new List<s_t_itemhecheng>();
        for (int i = 0; i < game_data._instance.m_dbc_itemhecheng.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_itemhecheng.get(0, i));
            s_t_itemhecheng t_itemhecheng = game_data._instance.get_t_itemhecheng(id);
            if (t_itemhecheng.fenye == select)
            {
                itemhechengs.Add(t_itemhecheng);
            }
        }
        itemhechengs.Sort(red_equip_comp);
        if (select != 3)
        {
            for (int i = 0; i < itemhechengs.Count; ++i)
            {
                GameObject obj = game_data._instance.ins_object_res("ui/dui_huan_sub");
                obj.transform.parent = m_scro.transform;
                obj.transform.localPosition = new Vector3(0, 165 - 156 * i, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.transform.GetComponent<dui_huan_sub>().m_scro = m_scro;
                obj.transform.GetComponent<dui_huan_sub>().t_itemhecheng = itemhechengs[i];
                obj.transform.GetComponent<dui_huan_sub>().reset();
                sys._instance.add_pos_anim(obj, 0.3f, new Vector3(-300, 0, 0), i * 0.1f);
                sys._instance.add_alpha_anim(obj, 0.3f, 0, 1.0f, i * 0.1f);
            }
        }
        else
        {
            int num = 0;
            for (int i = 0; i < itemhechengs.Count; ++i)
            {
                if (itemhechengs[i].cl_type.Count < 3)
                {
                    GameObject obj = game_data._instance.ins_object_res("ui/dui_huan_sub");
                    obj.transform.parent = m_scro.transform;
                    obj.transform.localPosition = new Vector3(0, 165 - 156 * num, 0);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.transform.GetComponent<dui_huan_sub>().m_scro = m_scro;
                    obj.transform.GetComponent<dui_huan_sub>().t_itemhecheng = itemhechengs[num];
                    obj.transform.GetComponent<dui_huan_sub>().reset();
                    sys._instance.add_pos_anim(obj, 0.3f, new Vector3(-300, 0, 0), num * 0.1f);
                    sys._instance.add_alpha_anim(obj, 0.3f, 0, 1.0f, num * 0.1f);
                }
                else
                {
                    GameObject obj = game_data._instance.ins_object_res("ui/dui_huan_pet_sub");
                    obj.transform.parent = m_scro.transform;
                    obj.transform.localPosition = new Vector3(0, 165 - 156 * num, 0);
                    obj.transform.localScale = new Vector3(1, 1, 1);
                    obj.transform.GetComponent<dui_huan_pet_sub>().m_scro = m_scro;
                    obj.transform.GetComponent<dui_huan_pet_sub>().t_itemhecheng = itemhechengs[num];
                    obj.transform.GetComponent<dui_huan_pet_sub>().reset();
                    sys._instance.add_pos_anim(obj, 0.3f, new Vector3(-300, 0, 0), num * 0.1f);
                    sys._instance.add_alpha_anim(obj, 0.3f, 0, 1.0f, num * 0.1f);
                }
                num++;
            }
        }
    }

    public static bool effect()
    {
        for (int i = 0; i < game_data._instance.m_dbc_itemhecheng.get_y(); ++i)
        {
            bool flag = true;
            int id = int.Parse(game_data._instance.m_dbc_itemhecheng.get(0, i));
            s_t_itemhecheng t_itemhecheng = game_data._instance.get_t_itemhecheng(id);
            for (int j = 0; j < t_itemhecheng.cl_type.Count; ++j)
            {
                int cl_num = sys._instance.m_self.get_item_num((uint)t_itemhecheng.cl_id[j]);
                if (cl_num < t_itemhecheng.cl_num[j])
                {
                    flag = false;
                    break;
                }
            }
            if (t_itemhecheng.type == 7)
            {
                if (sys._instance.m_self.m_t_player.guanghuan.Contains(t_itemhecheng.item_id))
                {
                    flag = false;
                }
            }
            if (flag)
            {
                return true;
            }
        }
        return false;
    }

    void duihuan_effect()
    {
        m_guanghuan_effect.SetActive(false);
        m_red_equip_effect.SetActive(false);
        m_pet_effect.SetActive(false);
        for (int i = 0; i < game_data._instance.m_dbc_itemhecheng.get_y(); ++i)
        {
            bool flag = true;
            int id = int.Parse(game_data._instance.m_dbc_itemhecheng.get(0, i));
            s_t_itemhecheng t_itemhecheng = game_data._instance.get_t_itemhecheng(id);
            for (int j = 0; j < t_itemhecheng.cl_type.Count; ++j)
            {
                int cl_num = sys._instance.m_self.get_item_num((uint)t_itemhecheng.cl_id[j]);
                if (cl_num < t_itemhecheng.cl_num[j])
                {
                    flag = false;
                    break;
                }
            }
            if (t_itemhecheng.type == 7)
            {
                if (!sys._instance.m_self.m_t_player.guanghuan.Contains(t_itemhecheng.item_id) && flag)
                {
                    m_guanghuan_effect.SetActive(true);
                }
            }
            if (flag && t_itemhecheng.fenye == 1)
            {
                m_red_equip_effect.SetActive(true);
            }
            else if (flag && t_itemhecheng.fenye == 2)
            {
                m_guanghuan_effect.SetActive(true);
            }
            else if (flag && t_itemhecheng.fenye == 3)
            {
                m_pet_effect.SetActive(true);
            }
        }
    }

    public int red_equip_comp(s_t_itemhecheng t_itemhecheng1, s_t_itemhecheng t_itemhecheng2)
    {
        bool flag = false;
        if (t_itemhecheng1.type == 7)
        {
            if (sys._instance.m_self.m_t_player.guanghuan.Contains(t_itemhecheng1.item_id))
            {
                flag = true;
            }
        }
        else
        {
            for (int i = 0; i < t_itemhecheng1.cl_type.Count; ++i)
            {
                int cl_num = sys._instance.m_self.get_item_num((uint)t_itemhecheng1.cl_id[i]);
                if (cl_num < t_itemhecheng1.cl_num[i])
                {
                    flag = true;
                    break;
                }
            }
        }
        bool _flag = false;
        if (t_itemhecheng2.type == 7)
        {
            if (sys._instance.m_self.m_t_player.guanghuan.Contains(t_itemhecheng2.item_id))
            {
                _flag = true;
            }
        }
        else
        {
            for (int i = 0; i < t_itemhecheng2.cl_type.Count; ++i)
            {
                int cl_num = sys._instance.m_self.get_item_num((uint)t_itemhecheng2.cl_id[i]);
                if (cl_num < t_itemhecheng2.cl_num[i])
                {
                    _flag = true;
                    break;
                }
            }
        }
        if (!flag && _flag)
        {
            return -1;
        }
        else if (flag && !_flag)
        {
            return 1;
        }
        else
        {
            return t_itemhecheng1.id - t_itemhecheng2.id;
        }
    }

    public void show_jiyin()
    {
        m_price_panel.SetActive(false);
        m_sell_button.SetActive(false);
        m_use_button.SetActive(false);
        m_tuse_button.SetActive(false);
        m_qiangwang_button.SetActive(false);
        m_equiphecheng_button.SetActive(false);
        m_jiyinhechneg_button.SetActive(false);
        m_select.transform.parent = m_bag_grid.transform.parent;
        get_jiyin();
    }

    public void update_ui(bool rf, int x = 1)
    {
        m_flag = false;
        m_select.transform.parent = m_bag_grid.transform.parent;
        if (x == 1)
        {
            get_item(rf);
        }
        else if (x == 3)
        {
            get_equip();
        }
        else if (x == 4)
        {
            get_treasure();
        }
        else if (x == 2)
        {
            get_jiyin();
        }
        else if (x == 5)
        {
            get_treasure_sp();
        }
        update_effect();
    }

    void update_effect()
    {
        if (can_jiyin())
        {
            m_effect_ji.SetActive(true);
        }
        else
        {
            m_effect_ji.SetActive(false);
        }
        if (can_suipian())
        {
            m_effect_sui.SetActive(true);
        }
        else
        {
            m_effect_sui.SetActive(false);
        }
        if (can_pet())
        {
            m_effect_pet.SetActive(true);
        }
        else
        {
            m_effect_pet.SetActive(false);
        }
        if (effect())
        {
            m_hc_effect.SetActive(true);
        }
        else
        {
            m_hc_effect.SetActive(false);
        }
    }
}
