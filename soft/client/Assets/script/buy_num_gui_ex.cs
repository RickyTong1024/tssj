using UnityEngine;

public class buy_num_gui_ex : MonoBehaviour, IMessage
{

    public GameObject m_icon;
    public UILabel m_num;
    public UILabel m_text;
    public int num = 0;
    public s_t_item item;
    const int id = 50080001;
    string color = "[000000]";

    public void reset()
    {
        sys._instance.remove_child(m_icon);
        item = game_data._instance.get_item(id);
        GameObject obj = icon_manager._instance.create_item_icon(id, sys._instance.m_self.get_item_num((uint)id));
        obj.transform.parent = m_icon.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        m_num.text = item.name + " x " + sys._instance.m_self.get_item_num((uint)id);
        if (num > sys._instance.m_self.get_item_num((uint)item.id))
        {
            color = "[ff0000]";

        }
        else
        {
            color = "[ffffff]";
        }
        m_text.text = color + num + "";
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_BOSS_ITEM_APPLY)
        {
            protocol.game.smsg_item_apply _msg = net_http._instance.parse_packet<protocol.game.smsg_item_apply>(message.m_byte);
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i], true);
            }
            for (int c = 0; c < _msg.types.Count; c++)
            {
                sys._instance.m_self.add_reward(_msg.types[c], _msg.value1s[c], _msg.value2s[c], _msg.value3s[c], true, "bossitemapply");
            }
            for (int i = 0; i < _msg.equips.Count; ++i)
            {
                sys._instance.m_self.add_equip(_msg.equips[i], true);
            }
            for (int i = 0; i < _msg.roles.Count; ++i)
            {
                sys._instance.m_self.add_card(_msg.roles[i], true);
            }
            sys._instance.m_self.remove_item((uint)item.id, num, game_data._instance.get_t_language("buy_num_gui_ex.cs_57_65"));//bossitemapply消耗
            num = 0;
            reset();
        }
    }

    void IMessage.message(s_message message)
    {

    }

    public void click(GameObject obj)
    {
        if (obj.name == "add")
        {
            {
                num++;
            }
        }
        else if (obj.name == "sub")
        {
            if (num - 1 >= 0)
            {
                num--;
            }

        }
        else if (obj.name == "add10")
        {
            {
                num += 10;
            }
        }
        else if (obj.name == "sub10")
        {
            if (num - 10 >= 0)
            {
                num -= 10;
            }
        }
        else if (obj.name == "queding")
        {
            if (num <= sys._instance.m_self.get_item_num((uint)id))
            {
                if (num == 0)
                {
                    return;
                }
                protocol.game.cmsg_item_apply _msg = new protocol.game.cmsg_item_apply();
                _msg.item_id = (uint)id;
                _msg.item_count = num;
                net_http._instance.send_msg_ex<protocol.game.cmsg_item_apply>(opclient_t.CMSG_BOSS_ITEM_APPLY, _msg);
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("buy_num_gui_ex.cs_122_71"));//道具不足
            }
        }
        else if (obj.name == "cancel")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        reset();
    }
}
