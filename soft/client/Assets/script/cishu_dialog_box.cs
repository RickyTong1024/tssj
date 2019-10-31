
using UnityEngine;
using System.Collections;

public class cishu_dialog_box : MonoBehaviour,IMessage {

    public GameObject m_name;
    public GameObject m_input;
    public UILabel m_num1;
    public UILabel m_num2;
    public int num1;
    public int num2;
    public int buy_num;
    private int m_input_num = 1;
    public s_message m_mes;
    public UILabel m_jewel;
    public int type = 1;//juntuan
    // Use this for initialization
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
        m_input_num = 1;
    }
    public void hide()
    {
        this.transform.Find("frame_big").GetComponent<frame>().hide();
        m_input_num = 1;
        s_message mes = new s_message();
        mes.m_type = "guildfightpvp_flag";
        cmessage_center._instance.add_message(mes);
    }
    int get_jewel()
    {
        int jewel = 0;
        for (int i = buy_num + 1; i <= buy_num + m_input_num; i++)
        {
            s_t_price t_price = game_data._instance.get_t_price(i);
            if (type == 1)
            {
                jewel += t_price.guild_attack_buy;
            }
            else if (type == 2)
            {
                jewel += t_price.hunter_assembly;

            }
            else if (type == 3)
            {
                jewel += t_price.bingyuan_reward;
            }
            else if (type == 4)
            {
                jewel += t_price.master;
            }
            else if (type == 5)
            {
                jewel += t_price.guildpvpbuy;
            }
            
        }
        return jewel;
 
    }
    public void updata_ui()
    {
        m_num1.text = game_data._instance.get_t_language ("cishu_dialog_box.cs_67_22") +" " + num1 + "";//当前次数：
        m_num2.text = game_data._instance.get_t_language ("cishu_dialog_box.cs_68_22") +" " + (num2 - buy_num) + "";//还可购买：
        if (type == 1)
        {
            m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language ("cishu_dialog_box.cs_71_50");//军团副本挑战次数

        }
        if (type == 3)
        {
            m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language ("cishu_dialog_box.cs_76_50");//冰原奖励次数

        }
            
        else if(type == 2)
        {
            m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language ("cishu_dialog_box.cs_82_50");//猎人大会挑战次数

        }
        else if (type == 4)
        {
            m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language ("cishu_dialog_box.cs_87_50");//大师联赛奖励次数

        }
        else if (type == 5)
        {
            m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language ("cishu_dialog_box.cs_99_50");//跨服战攻打次数
        }
        m_jewel.text = get_jewel() + "";
        m_input.GetComponent<UILabel>().text = (m_input_num).ToString();
       
    }

    public void click(GameObject obj)
    {
        if (obj.name == "hide")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            s_message _mes = new s_message();
            _mes.m_type = "bingyuan_move";
            cmessage_center._instance.add_message(_mes);
            s_message mes = new s_message();
            mes.m_type = "hide_cishudialog_box";
            cmessage_center._instance.add_message(mes);
            return;
        }
        int toltle_num = num2 - buy_num;
        if (obj.name == "add")
        {
            if (m_input_num + 1 <= toltle_num)
            {
                m_input_num += 1;
            }
        }
        else if (obj.name == "sub")
        {
            if (m_input_num > 1)
            {
                m_input_num--;
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
        }
        else if (obj.name == "queding")
        {
            ok();
            hide();

        }
        updata_ui();
    }

    public void ok()
    {
        if (sys._instance.m_self.m_t_player.jewel >= get_jewel())
        {
            m_mes.m_ints.Add(m_input_num);
            cmessage_center._instance.add_message(m_mes);
        }
        else
        {
            root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
        }
        
    }

    void IMessage.net_message(s_net_message message)
    {
       
    }

    void IMessage.message(s_message message)
    {

    }
}
