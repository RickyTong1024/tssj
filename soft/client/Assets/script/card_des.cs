using System.Collections.Generic;
using UnityEngine;

public class card_des : MonoBehaviour, IMessage
{
    public ccard m_card;
    public s_message m_out_message;
    public List<GameObject> m_equips = new List<GameObject>();
    public List<GameObject> m_treasures = new List<GameObject>();
    public List<GameObject> m_adds = new List<GameObject>();
    public List<GameObject> m_bw_adds = new List<GameObject>();
    public List<GameObject> level = new List<GameObject>();
    public List<GameObject> name = new List<GameObject>();
    List<dhc.equip_t> m_t_equips = new List<dhc.equip_t>();
    List<dhc.treasure_t> m_t_treasures = new List<dhc.treasure_t>();
    public GameObject[] effect;
    public GameObject[] effect2;
    public GameObject equip_effect;
    public GameObject treasure_effect;
    public GameObject m_bw;
    public GameObject m_zb;

    private int m_select_equip_id = 0;
    private int m_select_treasure_id = 0;

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
        if (obj.transform.name == "close")
        {
            m_out_message.m_long.Add(m_card.get_guid());
            cmessage_center._instance.add_message(m_out_message);

            s_message _message = new s_message();
            _message.m_type = "hide_show_unit";
            cmessage_center._instance.add_message(_message);

            this.GetComponent<ui_show_anim>().hide_ui();
        }
        else if (obj.transform.name == "equip_button")
        {
            m_zb.SetActive(true);
            m_bw.SetActive(false);
            show_equip();
            is_effect();
        }
        else if (obj.transform.name == "treasure_button")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_treasure));//该功能{0}级开启
                return;
            }
            m_zb.SetActive(false);
            m_bw.SetActive(true);
            show_treasure();
            is_effect();
        }
        else if (obj.transform.name == "change_treasure")
        {
            int count = 0;
            ccard card = m_card;
            List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
            List<int> index = new List<int>();
            List<ulong> treasure_guid = new List<ulong>();
            for (int i = 1; i <= 4; ++i)
            {
                if (is_treasure_open(i))
                {
                    count++;
                }
            }
            for (int m_select_treasure_id = 0; m_select_treasure_id < count; m_select_treasure_id++)
            {
                m_treasures.Clear();
                for (int i = 0; i < sys._instance.m_self.get_treasure_num(); i++)
                {
                    dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(i);
                    s_t_baowu t_treasure = game_data._instance.get_t_baowu(_treasure.template_id);
                    if (t_treasure.type != m_select_treasure_id + 1 || _treasure.role_guid > 0)
                    {
                        continue;
                    }
                    m_treasures.Add(_treasure);
                }
                bool flag = false;
                dhc.treasure_t temp = card.m_treasure[m_select_treasure_id];
                if (m_treasures.Count != 0)
                {
                    if (temp == null)
                    {
                        flag = true;
                        temp = m_treasures[0];
                    }
                    else
                    {
                        if (get_better_treasure(temp, m_treasures[0]))
                        {
                            flag = true;
                            temp = m_treasures[0];
                        }
                    }
                    for (int j = 1; j < m_treasures.Count; j++)
                    {
                        if (get_better_treasure(temp, m_treasures[j]))
                        {
                            flag = true;
                            temp = m_treasures[j];
                        }
                    }
                }
                if (temp != null && flag)
                {
                    index.Add(m_select_treasure_id);
                    treasure_guid.Add(temp.guid);

                    dhc.treasure_t temp1 = card.m_treasure[m_select_treasure_id];
                    if (temp1 != null)
                    {
                        temp1.role_guid = 0;
                    }
                    temp.role_guid = card.get_guid();
                    card.get_role().treasures[m_select_treasure_id] = temp.guid;
                    card.update_role_attr();
                }
            }
            if (index.Count > 0)
            {
                show_treasure();
                s_message _message2 = new s_message();
                _message2.m_type = "check_bf";
                cmessage_center._instance.add_message(_message2);

                protocol.game.cmsg_treasure_equip _msg = new protocol.game.cmsg_treasure_equip();
                _msg.role_guid = card.get_guid();
                for (int i = 0; i < index.Count; ++i)
                {
                    _msg.index.Add(index[i]);
                    _msg.treasure_guid.Add(treasure_guid[i]);
                }

                net_http._instance.send_msg<protocol.game.cmsg_treasure_equip>(opclient_t.CMSG_TREASURE_EQUICP, _msg);
            }
            is_effect();
        }
        else if (obj.transform.name == "yremove_treasure")
        {
            List<int> index = new List<int>();
            List<ulong> ytreasure_guid = new List<ulong>();
            int count = 0;
            ccard card = m_card;
            for (int j = 1; j <= 4; ++j)
            {
                if (is_treasure_open(j))
                {
                    count++;
                }
            }
            for (int i = 0; i < count; i++)
            {
                dhc.treasure_t treasure = card.m_treasure[i];
                if (treasure != null)
                {
                    index.Add(i);
                    ytreasure_guid.Add(treasure.guid);
                    treasure.role_guid = 0;
                    card.get_role().treasures[i] = 0;
                }
            }
            card.update_role_attr();

            show_treasure();
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
            protocol.game.cmsg_treasure_equip _msg = new protocol.game.cmsg_treasure_equip();
            _msg.role_guid = card.get_guid();
            for (int i = 0; i < index.Count; ++i)
            {
                _msg.index.Add(index[i]);
                _msg.treasure_guid.Add(0);
            }

            net_http._instance.send_msg<protocol.game.cmsg_treasure_equip>(opclient_t.CMSG_TREASURE_EQUICP, _msg);
            is_effect();
        }
        else if (obj.transform.name == "yremove")
        {
            List<int> index = new List<int>();
            List<ulong> yequip_guid = new List<ulong>();
            ccard card = m_card;
            for (int i = 0; i < 4; i++)
            {
                dhc.equip_t equip = card.m_equip[i];
                if (equip != null)
                {
                    index.Add(i);
                    yequip_guid.Add(equip.guid);
                    equip.role_guid = 0;
                    card.get_role().zhuangbeis[i] = 0;
                }
            }
            card.update_role_attr();

            show_equip();
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
            protocol.game.cmsg_role_equip _msg = new protocol.game.cmsg_role_equip();
            _msg.role_guid = card.get_guid();
            for (int i = 0; i < index.Count; ++i)
            {
                _msg.index.Add(index[i]);
                _msg.equip_guid.Add(0);
            }

            net_http._instance.send_msg<protocol.game.cmsg_role_equip>(opclient_t.CMSG_ROLE_EQUIP, _msg);
            is_effect();
        }
        else if (obj.transform.name == "change_dress")
        {
            ccard card = m_card;
            List<dhc.equip_t> m_equips = new List<dhc.equip_t>();
            List<int> index = new List<int>();
            List<ulong> equip_guid = new List<ulong>();
            for (int m_select_equip_id = 0; m_select_equip_id < 4; m_select_equip_id++)
            {
                m_equips.Clear();
                for (int i = 0; i < sys._instance.m_self.get_equip_num(); i++)
                {
                    dhc.equip_t _equip = sys._instance.m_self.get_equip_index(i);
                    s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
                    if (t_equip.type != m_select_equip_id + 1 || _equip.role_guid > 0)
                    {
                        continue;
                    }
                    m_equips.Add(_equip);
                }
                bool flag = false;
                dhc.equip_t temp = card.m_equip[m_select_equip_id];
                if (m_equips.Count != 0)
                {
                    if (temp == null)
                    {
                        flag = true;
                        temp = m_equips[0];
                    }
                    else
                    {
                        if (get_better(temp, m_equips[0]))
                        {
                            flag = true;
                            temp = m_equips[0];
                        }
                    }
                    for (int j = 1; j < m_equips.Count; j++)
                    {
                        if (get_better(temp, m_equips[j]))
                        {
                            flag = true;
                            temp = m_equips[j];
                        }
                    }
                }
                if (temp != null && flag)
                {
                    index.Add(m_select_equip_id);
                    equip_guid.Add(temp.guid);

                    dhc.equip_t temp1 = card.m_equip[m_select_equip_id];
                    if (temp1 != null)
                    {
                        temp1.role_guid = 0;
                    }
                    temp.role_guid = card.get_guid();
                    card.get_role().zhuangbeis[m_select_equip_id] = temp.guid;
                    card.update_role_attr();
                }
            }
            if (index.Count > 0)
            {
                show_equip();
                s_message _message2 = new s_message();
                _message2.m_type = "check_bf";
                cmessage_center._instance.add_message(_message2);

                protocol.game.cmsg_role_equip _msg = new protocol.game.cmsg_role_equip();
                _msg.role_guid = card.get_guid();
                for (int i = 0; i < index.Count; ++i)
                {
                    _msg.index.Add(index[i]);
                    _msg.equip_guid.Add(equip_guid[i]);
                }

                net_http._instance.send_msg<protocol.game.cmsg_role_equip>(opclient_t.CMSG_ROLE_EQUIP, _msg);
            }
            is_effect();
        }
    }

    bool get_better(dhc.equip_t x, dhc.equip_t y)
    {
        if (game_data._instance.get_t_equip(x.template_id).font_color < game_data._instance.get_t_equip(y.template_id).font_color)
        {
            return true;
        }
        else if (game_data._instance.get_t_equip(x.template_id).font_color == game_data._instance.get_t_equip(y.template_id).font_color)
        {
            if (x.enhance < y.enhance)
            {
                return true;
            }
        }
        return false;
    }

    bool get_better_treasure(dhc.treasure_t x, dhc.treasure_t y)
    {
        if (game_data._instance.get_t_baowu(x.template_id).font_color < game_data._instance.get_t_baowu(y.template_id).font_color)
        {
            return true;
        }
        else if (game_data._instance.get_t_baowu(x.template_id).font_color == game_data._instance.get_t_baowu(y.template_id).font_color)
        {
            if (x.jilian < y.jilian)
            {
                return true;
            }
            else if (x.jilian == y.jilian)
            {
                if (x.enhance < y.enhance)
                {
                    return true;
                }
            }
        }
        return false;
    }

    void is_effect()
    {
        bool flag = false;
        ccard card = m_card;
        flag = false;
        for (int i = 0; i < 4; ++i)
        {
            dhc.equip_t temp = card.m_equip[i];
            for (int j = 0; j < sys._instance.m_self.get_equip_num(); ++j)
            {
                dhc.equip_t _equip = sys._instance.m_self.get_equip_index(j);
                if (_equip.role_guid == 0)
                {
                    s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
                    if (t_equip.type == i + 1 && temp == null)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (temp == null)
            {
                effect[i].SetActive(false);
            }
            else
            {
                if (equip.is_enhance(temp))
                {
                    flag = true;
                    effect[i].SetActive(true);
                }
                else
                {
                    effect[i].SetActive(false);
                }
            }
        }
        if (flag)
        {
            equip_effect.SetActive(true);
        }
        else
        {
            equip_effect.SetActive(false);
        }
        flag = false;
        for (int i = 0; i < 4; ++i)
        {
            if (!is_treasure_open(i + 1))
            {
                effect2[i].SetActive(false);
                continue;
            }
            dhc.treasure_t temp = card.m_treasure[i];
            treasure_effect.SetActive(false);
            for (int j = 0; j < sys._instance.m_self.get_treasure_num(); ++j)
            {
                dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(j);
                if (_treasure.role_guid == 0)
                {
                    s_t_baowu t_treasure = game_data._instance.get_t_baowu(_treasure.template_id);
                    if (t_treasure.type == i + 1 && temp == null)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (temp == null)
            {
                effect2[i].SetActive(false);
            }
            else
            {
                if (treasure.is_enhance(temp))
                {
                    flag = true;
                    effect2[i].SetActive(true);
                }
                else
                {
                    effect2[i].SetActive(false);
                }
            }
        }
        if (flag)
        {
            treasure_effect.SetActive(true);
        }
        else
        {
            treasure_effect.SetActive(false);
        }
    }

    public void select_equip(GameObject obj)
    {
        m_select_equip_id = int.Parse(obj.transform.name);
        m_t_equips.Clear();
        for (int i = 0; i < 4; ++i)
        {
            dhc.equip_t equip = m_card.m_equip[i];
            if (equip != null)
            {
                m_t_equips.Add(equip);
            }
        }
        if (m_card.m_equip[m_select_equip_id] == null)
        {
            root_gui._instance.show_common_equip_panel(game_data._instance.get_t_language("card_des.cs_462_47"), false, true, m_select_equip_id + 1, new List<ulong>(), "common_select1_equip", false, 1, this.gameObject);//请选择需要的装备
            this.GetComponent<ui_show_anim>().hide_ui();
        }
        else
        {
            dhc.equip_t equip = m_card.m_equip[m_select_equip_id];
            root_gui._instance.add_equip(m_t_equips, true);
            root_gui._instance.show_equip_detail(equip, 2, this.gameObject, true);
        }
    }

    public void select_treasure(GameObject obj)
    {
        m_select_treasure_id = int.Parse(obj.transform.name);
        if (!is_treasure_open(m_select_treasure_id + 1))
        {
            if (m_select_treasure_id == 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("card_des.cs_480_59"));//25级开启饰品
            }
            if (m_select_treasure_id == 1)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("card_des.cs_484_59"));//28级开启饰品
            }
            if (m_select_treasure_id == 2)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("card_des.cs_488_59"));//31级开启饰品
            }
            if (m_select_treasure_id == 3)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("card_des.cs_492_59"));//34级开启饰品
            }
            return;
        }
        m_t_treasures.Clear();
        for (int i = 0; i < 4; ++i)
        {
            dhc.treasure_t treasure = m_card.m_treasure[i];
            if (treasure != null)
            {
                m_t_treasures.Add(treasure);
            }
        }
        if (m_card.m_treasure[m_select_treasure_id] == null)
        {
            root_gui._instance.show_common_treasure_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_1833_50"), false, true, m_select_treasure_id + 1, new List<ulong>(), "common_select1_treasure", false, 1, this.gameObject);//请选择需要的饰品
            this.GetComponent<ui_show_anim>().hide_ui();
        }
        else
        {
            dhc.treasure_t treasure = m_card.m_treasure[m_select_treasure_id];
            root_gui._instance.add_treasure(m_t_treasures, true);
            root_gui._instance.show_treasure_detail(treasure, 2, this.gameObject, true);
        }
    }


    public void reset()
    {
        if (m_zb.activeSelf)
        {
            m_zb.SetActive(true);
            m_bw.SetActive(false);
            show_equip();
        }
        if (m_bw.activeSelf)
        {
            m_zb.SetActive(false);
            m_bw.SetActive(true);
            show_treasure();
        }
        is_effect();
    }

    void set_equip(int id, dhc.equip_t equip)
    {
        sys._instance.remove_child(m_equips[id]);

        bool flag = false;
        for (int i = 0; i < sys._instance.m_self.get_equip_num(); ++i)
        {
            dhc.equip_t _equip = sys._instance.m_self.get_equip_index(i);
            if (_equip.role_guid == 0)
            {
                s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
                if (t_equip.type == id + 1)
                {
                    flag = true;
                    break;
                }
            }
        }
        m_adds[id].gameObject.SetActive(flag);

        if (equip == null)
        {
            return;
        }

        GameObject _icon = icon_manager._instance.create_equip_icon(equip);
        _icon.transform.GetComponent<BoxCollider>().enabled = false;

        _icon.transform.name = "icon";
        _icon.transform.parent = m_equips[id].transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
    }

    void show_equip()
    {
        for (int i = 0; i < 4; i++)
        {
            set_equip(i, m_card.m_equip[i]);
        }
    }
    void show_treasure()
    {
        for (int i = 0; i < 4; i++)
        {
            set_treasure(i, m_card.m_treasure[i]);
        }
    }

    void set_treasure(int id, dhc.treasure_t treasure)
    {
        sys._instance.remove_child(m_treasures[id]);

        bool flag = false;
        for (int i = 0; i < sys._instance.m_self.get_treasure_num(); ++i)
        {
            dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(i);
            if (_treasure.role_guid == 0)
            {
                s_t_baowu t_baowu = game_data._instance.get_t_baowu(_treasure.template_id);
                if (t_baowu.type == id + 1 && is_treasure_open(id + 1))
                {
                    flag = true;
                    break;
                }
            }
        }
        m_bw_adds[id].gameObject.SetActive(flag);
        for (int i = 0; i < 4; ++i)
        {
            if (is_treasure_open(i + 1))
            {
                level[i].SetActive(false);
                name[i].SetActive(true);
            }
            else
            {
                level[i].SetActive(true);
                name[i].SetActive(false);
            }
        }
        if (treasure == null)
        {
            return;
        }

        GameObject _icon = icon_manager._instance.create_treasure_icon(treasure);
        _icon.transform.GetComponent<BoxCollider>().enabled = false;
        _icon.transform.name = "icon";
        _icon.transform.parent = m_treasures[id].transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
    }

    public static bool is_treasure_open(int id)
    {
        if (id == 1 && sys._instance.m_self.get_att(e_player_attr.player_level) < 25)
        {
            return false;
        }
        if (id == 2 && sys._instance.m_self.get_att(e_player_attr.player_level) < 28)
        {
            return false;
        }

        if (id == 3 && sys._instance.m_self.get_att(e_player_attr.player_level) < 31)
        {
            return false;
        }
        if (id == 4 && sys._instance.m_self.get_att(e_player_attr.player_level) < 34)
        {
            return false;
        }
        return true;
    }

    void IMessage.net_message(s_net_message message)
    {

    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "common_select_equip")
        {
            ulong equip_guid = (ulong)message.m_long[0];
            dhc.equip_t temp = sys._instance.m_self.get_equip_guid(equip_guid);

            dhc.equip_t temp1 = m_card.m_equip[m_select_equip_id];
            if (temp1 != null)
            {
                temp1.role_guid = 0;
                m_card.get_role().zhuangbeis[m_select_equip_id] = 0;
            }
            if (temp != null)
            {
                temp.role_guid = m_card.get_guid();
                m_card.get_role().zhuangbeis[m_select_equip_id] = temp.guid;
            }
            m_card.update_role_attr();

            show_equip();
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
            protocol.game.cmsg_role_equip _msg = new protocol.game.cmsg_role_equip();
            _msg.role_guid = m_card.get_guid();
            _msg.index.Add(m_select_equip_id);
            _msg.equip_guid.Add(equip_guid);
            net_http._instance.send_msg<protocol.game.cmsg_role_equip>(opclient_t.CMSG_ROLE_EQUIP, _msg);
            reset();
        }
        if (message.m_type == "common_equip_tihuan" && this.gameObject.activeSelf)
        {
            root_gui._instance.show_common_equip_panel(game_data._instance.get_t_language("card_des.cs_462_47"), true, true, m_select_equip_id + 1, new List<ulong>(), "common_select1_equip", false, 1, this.gameObject);//请选择需要的装备
                                                                                                                                                                                                                          //root_gui._instance.add_tz_equip(m_t_equips);
            this.GetComponent<ui_show_anim>().hide_ui();
        }
        if (message.m_type == "common_treasure_tihuan" && this.gameObject.activeSelf)
        {
            root_gui._instance.show_common_treasure_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_1833_50"), true, true, m_select_treasure_id + 1, new List<ulong>(), "common_select1_treasure", false, 1, this.gameObject);//请选择需要的饰品
            this.GetComponent<ui_show_anim>().hide_ui();
            reset();
        }
        if (message.m_type == "common_select_treasure"
           && (root_gui._instance.m_common_equip_page_gui.GetComponent<common_treasure_page_gui>().m_next == this.gameObject
             || root_gui._instance.m_treasure_detail.GetComponent<treasure_detail>().m_next == this.gameObject))
        {
            ulong treasure_guid = (ulong)message.m_long[0];
            dhc.treasure_t temp = sys._instance.m_self.get_treasure_guid(treasure_guid);
            ccard card = m_card;
            dhc.treasure_t temp1 = card.m_treasure[m_select_treasure_id];
            if (temp1 != null)
            {
                temp1.role_guid = 0;
                card.get_role().treasures[m_select_treasure_id] = 0;
            }

            if (temp != null)
            {
                temp.role_guid = card.get_guid();
                card.get_role().treasures[m_select_treasure_id] = temp.guid;
            }
            card.update_role_attr();

            show_treasure();
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
            protocol.game.cmsg_treasure_equip _msg = new protocol.game.cmsg_treasure_equip();
            _msg.role_guid = card.get_guid();
            _msg.index.Add(m_select_treasure_id);
            _msg.treasure_guid.Add(treasure_guid);
            net_http._instance.send_msg<protocol.game.cmsg_treasure_equip>(opclient_t.CMSG_TREASURE_EQUICP, _msg);
            reset();
        }
    }
}
