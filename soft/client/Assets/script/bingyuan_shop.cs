using System.Collections.Generic;
using UnityEngine;

public class bingyuan_shop : MonoBehaviour, IMessage
{
    public GameObject m_view;
    private int m_reward_id;
    public GameObject m_effect;
    public UIToggle m_button_shangpin;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "bingyuan_shop")
        {
            s_message _message = new s_message();
            _message.m_type = "buy_num_gui";
            _message.m_ints.Add((int)message.m_ints[0]);
            _message.m_ints.Add(16);
            _message.m_ints.Add(2);
            cmessage_center._instance.add_message(_message);
        }
        else if (message.m_type == "bingyuan_reward")
        {
            m_reward_id = (int)message.m_ints[0];
            protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
            _net_msg.item_id = m_reward_id;
            _net_msg.num = 1;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_BY_MUBIAO, _net_msg);
        }
        else if (message.m_type == "refresh_bingyuan_shop_gui")
        {
            reset();
        }
    }

    void click(GameObject obj)
    {
        if (obj.name == "shangpin")
        {
            reset();
        }
        else if (obj.name == "reward")
        {
            reset_reward();
        }
    }

    public static bool effect()
    {
        foreach (int id in game_data._instance.m_dbc_bingyuan_mubiao.m_index.Keys)
        {
            if (sys._instance.m_self.m_t_player.by_rewards.Contains(id))
            {
                continue;
            }
            s_t_bingyuan_mubiao _reward = game_data._instance.get_t_bingyuan_mubiao(id);
            if (sys._instance.m_self.m_t_player.by_point >= _reward.jifen)
            {
                return true;
            }
        }
        return false;
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_BY_MUBIAO)
        {
            protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy>(message.m_byte);

            for (int i = 0; i < _msg.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_msg.equips[i], true);
            }
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i], true);
            }
            for (int i = 0; i < _msg.types.Count; ++i)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i], game_data._instance.get_t_language("bingyuan_shop.cs_91_113"));//冰原商店购买
            }
            s_t_bingyuan_mubiao m_t_shop = game_data._instance.get_t_bingyuan_mubiao(m_reward_id);
            sys._instance.m_self.sub_res(24, m_t_shop.price);
            sys._instance.m_self.m_t_player.by_rewards.Add(m_t_shop.id);
            reset_reward();
        }
    }

    int compare(int x, int y)
    {
        int temp = get_reward_state(x) - get_reward_state(y);
        if (temp != 0)
        {
            return temp;
        }
        else
        {
            return x - y;
        }
    }

    int get_reward_state(int id)
    {
        if (sys._instance.m_self.m_t_player.by_rewards.Contains(id))
        {
            return 3;
        }
        else
        {
            s_t_bingyuan_mubiao _mubiao = game_data._instance.get_t_bingyuan_mubiao(id);
            if (sys._instance.m_self.m_t_player.by_point >= _mubiao.jifen)
            {
                return 1;
            }
            else return 2;
        }
    }

    void reset_reward()
    {
        int k = 0;
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        List<int> ids = new List<int>();
        for (int i = 0; i < game_data._instance.m_dbc_bingyuan_mubiao.get_y(); ++i)
        {
            ids.Add(int.Parse(game_data._instance.m_dbc_bingyuan_mubiao.get(0, i)));
        }
        ids.Sort(compare);
        for (int i = 0; i < ids.Count; ++i)
        {
            int id = ids[i];
            s_t_bingyuan_mubiao _shop_mubiao = game_data._instance.get_t_bingyuan_mubiao(id);
            GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");

            _card.transform.parent = m_view.transform;
            _card.GetComponent<temaihui_card>().m_shop_id = id;
            _card.GetComponent<temaihui_card>().type = 17;
            _card.GetComponent<temaihui_card>().updata_ui();

            if (k % 2 == 0)
            {
                _card.transform.localPosition = new Vector3(-201, 135 - k / 2 * 137, 0);
            }
            if (k % 2 == 1)
            {
                _card.transform.localPosition = new Vector3(201, 135 - k / 2 * 137, 0);
            }
            _card.transform.localScale = new Vector3(1, 1, 1);
            _card.SetActive(true);
            k++;
        }
        if (effect())
        {
            m_effect.SetActive(true);
        }
        else
        {
            m_effect.SetActive(false);
        }
        bingyuan_gui._instance.bingyuan_shop_effect();
    }

    public void hide()
    {
        m_button_shangpin.value = true;
        this.transform.Find("frame_big").GetComponent<frame>().hide();
    }

    public void reset()
    {
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        int k = 0;
        for (int i = 0; i < game_data._instance.m_dbc_bingyuan_shop.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_bingyuan_shop.get(0, i));

            if (game_data._instance.get_t_bingyuan_shop(id).level > sys._instance.m_self.m_t_player.level)
            {
                continue;
            }
            GameObject temp = game_data._instance.ins_object_res("ui/temaihui_card");
            temp.transform.parent = m_view.transform;
            temp.GetComponent<temaihui_card>().m_shop_id = id;
            temp.GetComponent<temaihui_card>().m_shell = buy_shop_num(id);
            temp.GetComponent<temaihui_card>().type = 16;
            temp.GetComponent<temaihui_card>().updata_ui();
            temp.transform.localScale = new Vector3(1, 1, 1);
            if (k % 2 == 0)
            {
                temp.transform.localPosition = new Vector3(-201, 135 - i / 2 * 137, 0);
            }
            if (k % 2 == 1)
            {
                temp.transform.localPosition = new Vector3(201, 135 - i / 2 * 137, 0);
            }
            k++;
        }
        if (effect())
        {
            m_effect.SetActive(true);
        }
        else
        {
            m_effect.SetActive(false);
        }
        bingyuan_gui._instance.bingyuan_shop_effect();
    }

    public int buy_shop_num(int id)
    {
        s_t_bingyuan_shop t_bingyuan_shop = game_data._instance.get_t_bingyuan_shop(id);
        int num = t_bingyuan_shop.buy_count;
        for (int i = 0; i < sys._instance.m_self.m_t_player.by_shops.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.by_shops[i] == (uint)id)
            {
                num = t_bingyuan_shop.buy_count - sys._instance.m_self.m_t_player.by_nums[i];
                if (t_bingyuan_shop.buy_count == 0)
                {
                    num = 1000;
                }
                break;
            }
        }
        return num;
    }
}
