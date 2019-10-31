using UnityEngine;

public class bag_buy_num_gui : MonoBehaviour
{
    public GameObject m_icon;
    public UILabel m_num;
    public UILabel m_text;
    public int num = 1;
    public s_t_item item;
    public int id = 50080001;
    public s_message m_mes;
    public UILabel m_name;
    public UILabel m_sell_num;
    public string m_color;

    public void reset(int id, s_message m_mes)
    {
        this.id = id;
        this.m_mes = m_mes;
        sys._instance.remove_child(m_icon);
        item = game_data._instance.get_item(id);
        GameObject obj = icon_manager._instance.create_item_icon(id, sys._instance.m_self.get_item_num((uint)id));
        obj.transform.parent = m_icon.transform;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        m_name.text = sys._instance.get_res_info(2, id, 0, 0);
        m_num.text = game_data._instance.get_t_language("bag_buy_num_gui.cs_32_22") + sys._instance.m_self.get_item_num((uint)id);//数量 x 
        if (num > sys._instance.m_self.get_item_num((uint)item.id))
        {
            num = sys._instance.m_self.get_item_num((uint)item.id);

        }
        m_text.text = num + "";
        m_sell_num.text = m_color + num * item.gold + "";
    }

    public void click(GameObject obj)
    {
        if (obj.name == "add")
        {
            if (num + 1 <= sys._instance.m_self.get_item_num((uint)id))
            {
                num++;
            }

        }
        else if (obj.name == "sub")
        {
            if (num - 1 >= 1)
            {
                num--;
            }

        }
        else if (obj.name == "add10")
        {

            num += 10;
        }
        else if (obj.name == "sub10")
        {
            if (num - 10 >= 1)
            {
                num -= 10;
            }
            else
            {
                num = 1;
            }
        }
        else if (obj.name == "queding")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            m_mes.m_ints.Add(num);
            cmessage_center._instance.add_message(m_mes);
        }
        else if (obj.name == "cancel")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        reset(id, m_mes);

    }

    void OnDisable()
    {
        num = 1;
    }
}
