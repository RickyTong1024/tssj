using System.Collections.Generic;
using UnityEngine;

public class baowu_ronglian_gui : MonoBehaviour, IMessage
{
    public List<GameObject> m_icons = new List<GameObject>();
    List<s_t_baowu> baowus = new List<s_t_baowu>();
    public GameObject m_new_icon;
    public GameObject m_name;
    public GameObject m_attr;
    public GameObject m_ricon;
    public GameObject m_sp_icon;
    public GameObject m_hc_name;
    public GameObject m_jewel;
    public GameObject m_select;
    public GameObject m_sp_show;
    public int m_id = 0;
    public ulong m_guid;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    public void reset(int _id)
    {
        baowus.Clear();
        for (int i = 0; i < game_data._instance.m_dbc_baowu.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_baowu.get(0, i));
            s_t_baowu t_baowu = game_data._instance.get_t_baowu(id);
            if (t_baowu.font_color == 5)
            {
                baowus.Add(t_baowu);
            }
        }
        for (int i = 0; i < baowus.Count && i < m_icons.Count; ++i)
        {
            sys._instance.remove_child(m_icons[i]);
            GameObject _icon = icon_manager._instance.create_item_icon_ex(baowus[i].fragments[0]);
            _icon.transform.name = i.ToString();
            _icon.transform.parent = m_icons[i].transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            _icon.transform.GetComponent<BoxCollider>().enabled = true;
            UIButtonMessage[] message = _icon.transform.GetComponents<UIButtonMessage>();
            message[0].target = this.gameObject;
            message[0].functionName = "click_sp_icon";
            message[1].target = null;
            message[1].functionName = "";
            message[2].target = null;
            message[2].functionName = "";
        }
        GameObject temp1 = Instantiate(m_select) as GameObject;
        temp1.transform.parent = m_icons[_id].transform;
        temp1.transform.localScale = Vector3.one;
        temp1.transform.localPosition = new Vector3(0, 0, 0);
        temp1.SetActive(true);
        sys._instance.remove_child(m_new_icon);
        GameObject _icon1 = icon_manager._instance.create_treasure_icon(baowus[_id].id);
        _icon1.transform.parent = m_new_icon.transform;
        _icon1.transform.localScale = new Vector3(1, 1, 1);
        _icon1.transform.localPosition = new Vector3(0, 0, 0);
        m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2, baowus[_id].fragments[0], 1, 0);
        m_hc_name.GetComponent<UILabel>().text = baowus[_id].name;
        m_attr.GetComponent<UILabel>().text = game_data._instance.get_float_string(baowus[_id].attr1, baowus[_id].value1) + "  "
            + game_data._instance.get_float_string(baowus[_id].attr2, baowus[_id].value2);
        sys._instance.remove_child(m_sp_icon);
        GameObject _icon2 = icon_manager._instance.create_item_icon(baowus[_id].fragments[0]);
        _icon2.transform.parent = m_sp_icon.transform;
        _icon2.transform.localScale = new Vector3(1, 1, 1);
        _icon2.transform.localPosition = new Vector3(0, 0, 0);
        sys._instance.remove_child(m_ricon);
        if (m_guid != 0)
        {
            GameObject _icon = icon_manager._instance.create_treasure_icon(m_guid);
            _icon.transform.GetComponent<BoxCollider>().enabled = false;
            _icon.transform.parent = m_ricon.transform;
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            _icon.transform.localScale = new Vector3(1, 1, 1);
        }

        if (sys._instance.m_self.m_t_player.jewel < 250)
        {
            m_jewel.GetComponent<UILabel>().text = "[ff0000]x250";
        }
        else
        {
            m_jewel.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + "x250";
        }
    }

    public void select_treasure(GameObject obj)
    {
        if (m_guid == 0)
        {
            List<ulong> m_hide_guids = new List<ulong>();
            root_gui._instance.show_common_treasure_panel(game_data._instance.get_t_language("baowu_ronglian_gui.cs_103_50"), false, false, 0, m_hide_guids, "common_ronglian_treasure", false, 3, root_gui._instance.m_bw_gui);//请选择需要圣炼的饰品
            root_gui._instance.m_bw_gui.SetActive(false);
        }
        else
        {
            sys._instance.remove_child(m_ricon);
            m_guid = 0;
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "common_ronglian1_treasure")
        {
            m_guid = (ulong)message.m_long[0];
            reset(m_id);
        }
    }

    public void click_sp_icon(GameObject obj)
    {
        m_id = int.Parse(obj.transform.name);
        reset(m_id);
    }

    public void click(GameObject obj)
    {
        if (obj.transform.name == "sl")
        {
            if (m_guid == 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_ronglian_gui.cs_136_59"));//未选择未强化未精炼的橙色饰品
                return;
            }
            if (sys._instance.m_self.m_t_player.jewel < 250)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_ronglian_gui.cs_141_59"));//钻石不足
                return;
            }
            if (m_guid != 0)
            {
                protocol.game.cmsg_treasure_ronglian _msg = new protocol.game.cmsg_treasure_ronglian();
                _msg.suipian_id = baowus[m_id].fragments[0];
                _msg.treasure_guid = m_guid;
                net_http._instance.send_msg<protocol.game.cmsg_treasure_ronglian>(opclient_t.CMSG_TREASURE_RONGLIAN, _msg);
            }
        }
        else if (obj.transform.name == "auto")
        {
            if (m_guid != 0)
            {
                return;
            }
            auto_treasure();
            reset(m_id);
        }
        else if (obj.name == "close_sp")
        {
            m_sp_show.GetComponent<ui_show_anim>().hide_ui();
            reset(m_id);
        }
        else if (obj.name == "close")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
    }

    void show_item()
    {
        m_sp_show.SetActive(true);
        UILabel _label = m_sp_show.transform.Find("info").GetComponent<UILabel>();
        GameObject _icon = m_sp_show.transform.Find("icon").gameObject;
        _label.text = sys._instance.get_res_info(2, baowus[m_id].fragments[0], 0, 0);
        sys._instance.remove_child(_icon);
        GameObject temp = icon_manager._instance.create_item_icon(baowus[m_id].fragments[0]);
        temp.transform.parent = _icon.transform;
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
    }

    public void auto_treasure()
    {
        m_guid = 0;
        List<ulong> m_treasures_guid = new List<ulong>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.treasures.Count; ++i)
        {
            dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_guid(sys._instance.m_self.m_t_player.treasures[i]);
            s_t_baowu t_treasure = game_data._instance.get_t_baowu(_treasure.template_id);
            if (_treasure.role_guid != 0 || _treasure.locked == 1 || _treasure.jilian != 0 || _treasure.enhance != 0 || t_treasure.font_color != 4)
            {
                continue;
            }
            m_treasures_guid.Add(sys._instance.m_self.m_t_player.treasures[i]);
        }
        if (m_treasures_guid.Count <= 0)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_ronglian_gui.cs_202_58"));//您没有闲置的未强化未精炼的橙色饰品
            return;
        }
        m_treasures_guid.Sort(comp);
        m_guid = m_treasures_guid[0];
    }

    public int comp(ulong guid1, ulong guid2)
    {
        dhc.treasure_t t_treasure1 = sys._instance.m_self.get_treasure_guid(guid1);
        dhc.treasure_t t_treasure2 = sys._instance.m_self.get_treasure_guid(guid2);
        if (t_treasure1.template_id < t_treasure2.template_id)
        {
            return -1;
        }
        else if (t_treasure2.template_id < t_treasure1.template_id)
        {
            return 1;
        }
        return 0;
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_TREASURE_RONGLIAN)
        {
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, 250, game_data._instance.get_t_language("baowu_ronglian_gui.cs_228_74"));//宝物熔炼消耗
            sys._instance.m_self.remove_treasure(m_guid);
            sys._instance.m_self.add_item((uint)baowus[m_id].fragments[0], 1, false, game_data._instance.get_t_language("baowu_ronglian_gui.cs_230_73"));//宝物熔炼获得
            sys._instance.remove_child(m_ricon);
            m_guid = 0;
            show_item();
            s_message _msg = new s_message();
            _msg.m_type = "refresh_bw_gui";
            cmessage_center._instance.add_message(_msg);
        }
    }
}
