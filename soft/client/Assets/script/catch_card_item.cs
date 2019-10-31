using UnityEngine;

public class catch_card_item : MonoBehaviour
{
    public int id;
    private s_t_class t_class;

    void Start()
    {
        reset();
    }

    void reset()
    {
        t_class = game_data._instance.get_t_class(id);
        string s = "";
        if (t_class.job == 1)
        {
            s = "[0BBBF5]" + game_data._instance.get_t_language("catch_card_item.cs_20_8");//防卫
        }
        else if (t_class.job == 2)
        {
            s = "[F98C20]" + game_data._instance.get_t_language("bu_zheng_panel.cs_2726_26");//物理
        }
        else if (t_class.job == 3)
        {
            s = "[E928B8]" + game_data._instance.get_t_language("bu_zheng_panel.cs_2730_26");//魔法
        }
        this.transform.Find("name").GetComponent<UILabel>().text = t_class.name;
        this.transform.Find("zy").GetComponent<UILabel>().text = s;
        this.transform.Find("pz").GetComponent<UILabel>().text = t_class.pz.ToString();
    }

    public void click(GameObject obj)
    {
        s_message _message = new s_message();
        _message.m_type = "card_info_class";
        _message.m_ints.Add(t_class.id);
        cmessage_center._instance.add_message(_message);
    }
}
