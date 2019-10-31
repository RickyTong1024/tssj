using UnityEngine;

public class baowu_mianzhan : MonoBehaviour, IMessage
{

    public GameObject m_buy;
    public GameObject m_use;
    public GameObject m_item_num;
    public UILabel m_time;
    public GameObject m_icon;
    public GameObject m_baowu_hecheng;
    public UILabel m_jewel_label;
    public int m_id;
    s_t_item _item;

    void Start()
    {
        cmessage_center._instance.add_handle(this);

    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    public void reset()
    {
        sys._instance.remove_child(m_icon);
        GameObject temp = icon_manager._instance.create_item_icon(m_id);
        temp.transform.parent = m_icon.transform;
        temp.transform.localPosition = Vector3.zero;
        temp.transform.localScale = Vector3.one;
        m_item_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("baowu_mianzhan.cs_32_59"), sys._instance.m_self.get_item_num((uint)m_id));//当前拥有 : {0}
        _item = game_data._instance.get_item((int)m_id);

        m_time.text = string.Format(game_data._instance.get_t_language("baowu_mianzhan.cs_35_30"), _item.def_1);//免战时间增加{0}小时
        m_jewel_label.text = _item.def_3.ToString();
    }

    void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            this.GetComponent<ui_bloom_anim>().hide_ui();
        }
        else if (obj.name == "buy")
        {
            s_message _mes = new s_message();
            _mes.m_type = "buy_mianzhanpai";
            root_gui._instance.show_select_dialog_box
                (game_data._instance.get_t_language("baowu_mianzhan.cs_51_17"), string.Format(game_data._instance.get_t_language("baowu_mianzhan.cs_51_40"), _item.def_3, sys._instance.get_res_info(2, _item.id, 0, 0)), _mes);//购买免战牌//你确定要花费[00ffff]{0}[-]钻石购买[{1}]吗？
        }
        else if (obj.name == "shiyong")
        {
            if (timer.now() > sys._instance.m_self.m_t_player.treasure_protect_cd_time)
            {
                if (sys._instance.m_self.get_item_num((uint)m_id) != 0)
                {
                    protocol.game.cmsg_treasure_protect _msg = new protocol.game.cmsg_treasure_protect();
                    if (m_id == 50050001)
                    {
                        _msg.type = 0;
                    }
                    else
                    {
                        _msg.type = 1;
                    }
                    net_http._instance.send_msg<protocol.game.cmsg_treasure_protect>(opclient_t.CMSG_TREASURE_PROTECT, _msg);
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("baowu_mianzhan.cs_73_60"));//道具不足请购买
                }
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]");
            }
        }
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_TREASURE_PROTECT)
        {
            protocol.game.smsg_treasure_protect _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_protect>(message.m_byte);
            sys._instance.m_self.m_t_player.treasure_protect_cd_time = _msg.cd_time;
            sys._instance.m_self.m_t_player.treasure_protect_next_time = _msg.next_time;
            sys._instance.m_self.remove_item((uint)m_id, 1, game_data._instance.get_t_language("baowu_mianzhan.cs_92_49"));//夺宝免战消耗
            this.GetComponent<ui_bloom_anim>().hide_ui();
            m_baowu_hecheng.GetComponent<baowu_hecheng>().refresh_time();
        }
        else if (message.m_opcode == opclient_t.CMSG_TREASURE_BUY)
        {
            sys._instance.m_self.add_item((uint)m_id, 1, game_data._instance.get_t_language("baowu_mianzhan.cs_98_47") + game_data._instance.get_item(m_id).name);//夺宝奇兵免战购买
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, _item.def_3, game_data._instance.get_t_language("baowu_mianzhan.cs_99_82"));//夺宝奇兵免战购买消耗
            reset();
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "buy_mianzhanpai")
        {
            if (sys._instance.m_self.m_t_player.jewel < _item.def_3)
            {
                root_gui._instance.show_recharge_dialog_box(delegate ()
                {
                    this.GetComponent<ui_bloom_anim>().hide_ui();
                    m_baowu_hecheng.GetComponent<ui_title_anim>().hide_ui();
                });
                return;
            }
            protocol.game.cmsg_treasure_buy _msg = new protocol.game.cmsg_treasure_buy();
            if (m_id == 50050001)
            {
                _msg.type = 0;
            }
            else
            {
                _msg.type = 1;
            }
            net_http._instance.send_msg<protocol.game.cmsg_treasure_buy>(opclient_t.CMSG_TREASURE_BUY, _msg);
        }
    }
}
