using UnityEngine;

public class bingyuan_tip : MonoBehaviour
{
    public s_message m_mes;
    public UILabel m_jifen_success;
    public UILabel m_jifen_fail;
    public UILabel m_desc;
    public UILabel m_su_bingjin;
    public UILabel m_fail_bingjin;

    public void reset(int type)
    {
        this.gameObject.SetActive(true);
        int jifen = 100;
        int rate = 1;
        if ((timer.dtnow().Hour >= 12 && timer.dtnow().Hour < 14) || (timer.dtnow().Hour >= 18 && timer.dtnow().Hour < 22))
        {
            rate = 2;
        }
        m_jifen_success.text = string.Format(game_data._instance.get_t_language("bingyuan_tip.cs_20_45"), (jifen + jifen * bingyuan_gui._instance.m_reward_jiachneg / 100) * rate);//冰原积分：{0}
        m_jifen_fail.text = game_data._instance.get_t_language("bingyuan_tip.cs_21_28") + (jifen / 2 + jifen / 2 * bingyuan_gui._instance.m_reward_jiachneg / 100) * rate + "";//冰原积分：
        if (type == 1)
        {
            m_desc.text = game_data._instance.get_t_language("bingyuan_tip.cs_24_26");//继续开启
        }
        else if (type == 2)
        {
            m_desc.text = game_data._instance.get_t_language("bingyuan_tip.cs_28_26");//继续准备
        }
        m_su_bingjin.text = game_data._instance.get_t_resource(24).namecolor + game_data._instance.get_t_exp(sys._instance.m_self.m_t_player.level).suc_bingjin;
        m_fail_bingjin.text = game_data._instance.get_t_resource(24).namecolor + game_data._instance.get_t_exp(sys._instance.m_self.m_t_player.level).fail_bingjin;
    }

    void click(GameObject obj)
    {
        if (obj.name == "jixu")
        {
            close();
            if (bingyuan_gui._instance.m_start_panel.activeSelf)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language("bingyuan_tip.cs_42_71"));//不在房间中
            }
            else
            {
                cmessage_center._instance.add_message(m_mes);
            }
        }
        else if (obj.name == "buy")
        {
            s_t_price t_price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.by_reward_buy + 1);

            if (sys._instance.m_self.m_t_player.by_reward_buy >= 9)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bingyuan_gui.cs_1872_71"));//，提升vip等级可增加每日购买次数");//今日购买次数不足
                return;
            }
            close();
            s_message _mes = new s_message();
            _mes.m_type = "buy_bingyuan_reward_num";
            root_gui._instance.show_cishu_dialog_box(sys._instance.m_self.m_t_player.by_reward_num, 9,
                sys._instance.m_self.m_t_player.by_reward_buy, 3, _mes);
        }
        else if (obj.name == "hide")
        {
            close();
            bingyuan_gui._instance.m_can_move = true;
        }
    }

    void close()
    {
        this.transform.Find("frame_big").GetComponent<frame>().hide();
    }
}
