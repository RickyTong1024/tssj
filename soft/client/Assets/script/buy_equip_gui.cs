using UnityEngine;

public class buy_equip_gui : MonoBehaviour
{
    public int m_id;
    public GameObject m_zprice;
    public GameObject m_icon;
    public GameObject m_name;
    public GameObject m_input;
    private s_t_ttt_shop _t_ttt_shop;

    private int m_input_num = 1;
    private int m_input_price = 0;

    public void updata_ui()
    {
        _t_ttt_shop = game_data._instance.get_t_ttt_shop(m_id);
        if (_t_ttt_shop == null)
        {
            return;
        }
        GameObject _icon = icon_manager._instance.create_reward_icon(_t_ttt_shop.type, _t_ttt_shop.value1, _t_ttt_shop.value2, _t_ttt_shop.value3);
        sys._instance.remove_child(m_icon);
        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        m_input_num = 1;
        reset_price();
    }

    public void reset_price()
    {
        int toltle_num = sys._instance.m_self.get_att(e_player_attr.player_hj) / _t_ttt_shop.price;
        m_input_price = m_input_num * _t_ttt_shop.price;
        string s = "[35dbea]";
        if (m_input_price > sys._instance.m_self.get_att(e_player_attr.player_hj))
        {
            s = "[ff0000]";
        }
        m_input.GetComponent<UILabel>().text = (m_input_num).ToString();
        m_zprice.GetComponent<UILabel>().text = s + (m_input_price).ToString();
        m_name.GetComponent<UILabel>().text = _t_ttt_shop.name;
    }


    public void click(GameObject obj)
    {
        int toltle_num = sys._instance.m_self.get_att(e_player_attr.player_hj) / _t_ttt_shop.price;
        if (toltle_num >= 100)
        {
            toltle_num = 100;
        }
        if (obj.name == "add")
        {
            if (m_input_num + 1 <= toltle_num)
            {
                m_input_num += 1;
                reset_price();
            }
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
            if (m_input_num + 10 <= toltle_num)
            {
                m_input_num += 10;
            }
            else
            {
                m_input_num = toltle_num;
            }
            reset_price();
        }
        else if (obj.name == "sub10")
        {
            if (m_input_num - 10 >= 1)
            {
                m_input_num -= 10;
            }
            else
            {
                m_input_num = 1;
            }
            reset_price();
        }
        else if (obj.name == "queding")
        {
            s_message _msg = new s_message();
            _msg.m_type = "select_buy_equip";
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
