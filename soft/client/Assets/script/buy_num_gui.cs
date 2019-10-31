using UnityEngine;

public class buy_num_gui : MonoBehaviour
{
    public int m_id;
    public int m_num;
    public GameObject m_zprice;
    public GameObject m_icon;
    public GameObject m_name;
    public GameObject m_input;
    public GameObject m_cishu;
    private s_t_shop_xg m_t_shop;

    private int m_input_num = 1;
    private int m_input_price = 0;

    public void updata_ui()
    {
        m_t_shop = game_data._instance.get_shop_xg(m_id);
        GameObject _icon = icon_manager._instance.create_reward_icon(m_t_shop.type, m_t_shop.vlaue1, m_t_shop.vlaue2, m_t_shop.vlaue3);
        sys._instance.remove_child(m_icon);
        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        m_name.GetComponent<UILabel>().text = m_t_shop.name;

        m_input_num = 1;
        reset_price();

        if (m_t_shop.xg_type != 0)
        {
            m_cishu.SetActive(true);
            m_cishu.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("buy_num_gui.cs_43_56"), m_num.ToString());//今日可购买{0}：
            if (m_t_shop.xg_type == 3)
            {
                m_cishu.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("buy_num_gui.cs_46_57"), m_num.ToString());//本周还可购买{0}：
            }
        }
        else
        {
            m_cishu.SetActive(false);
        }
    }

    public void reset_price()
    {
        m_input_price = m_input_num * m_t_shop.price;
        if (m_t_shop.xg_type == 2)
        {
            m_input_price = 0;
            int xg_buy_num = 0;

            for (int i = 0; i < sys._instance.m_self.m_t_player.shop_xg_ids.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.shop_xg_ids[i] == m_id)
                {
                    xg_buy_num = sys._instance.m_self.m_t_player.shop_xg_nums[i];
                    break;
                }
            }

            for (int j = 0; j < m_input_num; ++j)
            {
                int xg_num = xg_buy_num + j + 1;
                m_input_price += temaihui_card.get_price(xg_num, m_t_shop.price_type);
            }
        }
        string s = "[35dbea]";
        if (m_input_price > sys._instance.m_self.get_att(e_player_attr.player_jewel))
        {
            s = "[ff0000]";
        }
        m_input.GetComponent<UILabel>().text = (m_input_num).ToString();
        m_zprice.GetComponent<UILabel>().text = s + (m_input_price).ToString();
    }

    public int get_inc_input_num(int input_num)
    {
        if (m_t_shop.xg_type == 0)
        {
            return input_num;
        }

        int xg_buy_num = 0;
        int total_num = m_t_shop.xg_num;
        if (m_t_shop.xg_type == 2)
        {
            total_num = m_t_shop.vip_type[sys._instance.m_self.m_t_player.vip];
        }
        if (m_t_shop.xg_type == 1)
        {
            total_num = m_t_shop.vip_type[0];
        }
        if (m_t_shop.xg_type == 0)
        {
            total_num = 100;
        }

        for (int i = 0; i < sys._instance.m_self.m_t_player.shop_xg_ids.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.shop_xg_ids[i] == m_id)
            {
                xg_buy_num = sys._instance.m_self.m_t_player.shop_xg_nums[i];
                break;
            }
        }

        if (xg_buy_num >= total_num)
        {
            return 0;
        }

        int left_num = total_num - xg_buy_num;
        if (m_input_num >= left_num)
        {
            return 0;
        }
        if (left_num >= input_num)
        {
            if (m_input_num + input_num > left_num)
            {
                return left_num - m_input_num;
            }
            else
            {
                return input_num;
            }
        }
        else
        {
            return left_num - m_input_num;
        }
    }

    public void click(GameObject obj)
    {
        if (obj.name == "add")
        {
            m_input_num += get_inc_input_num(1);
            reset_price();
        }
        else if (obj.name == "sub")
        {
            if (m_input_num > 1)
            {
                m_input_num--;
                reset_price();
            }
        }
        else if (obj.name == "add10")
        {
            m_input_num += get_inc_input_num(10);
            reset_price();
        }
        else if (obj.name == "sub10")
        {
            if (m_input_num <= 10)
            {
                m_input_num = 1;
                reset_price();
            }
            else
            {
                m_input_num -= 10;
                reset_price();
            }
        }
        else if (obj.name == "queding")
        {
            s_message _msg = new s_message();

            _msg.m_type = "buy_temaihui_item";
            _msg.m_ints.Add(m_input_num);
            _msg.m_ints.Add(m_input_price);
            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.name == "hide")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "cancle")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
    }
}
