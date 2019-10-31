using System.Collections.Generic;
using UnityEngine;

public class baowu_yjsaodang : MonoBehaviour
{
    public GameObject m_view;
    List<protocol.game.smsg_treasure_fight_end> m_rewards;
    public GameObject m_close;
    public GameObject m_close1;
    private float m_time;
    private int m_index;
    private int m_len;
    private int m_nengliang;
    public GameObject m_nl_num;
    private int m_bid;
    private bool m_suc;

    void OnFinished()
    {

    }

    public void init(int bid, bool suc, List<protocol.game.smsg_treasure_fight_end> rewards)//List<int> golds,List<int> powders,List<int> suipians
    {
        m_rewards = rewards;
        m_bid = bid;
        m_suc = suc;
        m_nengliang = sys._instance.m_self.m_t_player.energy;
        m_close.GetComponent<BoxCollider>().enabled = false;
        m_close.GetComponent<UISprite>().alpha = 0.5f;
        m_close1.GetComponent<BoxCollider>().enabled = false;
        m_close1.GetComponent<UISprite>().alpha = 0.5f;

        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
        sys._instance.m_self.zs_skill_target(4, rewards.Count);
        m_time = 0.5f;
        m_index = -1;
        m_len = 0;
        m_nl_num.GetComponent<UILabel>().text = sys._instance.m_self.get_item_num(10010007).ToString();
    }

    public void click(GameObject obj)
    {
        if (obj.name == "close" || obj.name == "hide")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            s_message _mes = new s_message();
            _mes.m_type = "refresh_bw_gui";
            cmessage_center._instance.add_message(_mes);
        }
    }

    void deal()
    {
        for (int j = 0; j < m_rewards[m_index].types.Count; j++)
        {
            sys._instance.m_self.add_reward(m_rewards[m_index].types[j], m_rewards[m_index].value1s[j], m_rewards[m_index].value2s[j], m_rewards[m_index].value3s[j], false);
        }
        if (m_rewards[m_index].suipian_id != 0)
        {
            sys._instance.m_self.add_item((uint)m_rewards[m_index].suipian_id, 1, false, game_data._instance.get_t_language("baowu_yjsaodang.cs_69_89"));//夺宝奇兵一键扫荡获得
        }
        if (m_rewards[m_index].pgold > 0)
        {
            sys._instance.m_self.add_att(e_player_attr.player_gold, m_rewards[m_index].pgold, false, game_data._instance.get_t_language("baowu_yjsaodang.cs_69_89"));//夺宝奇兵一键扫荡获得
        }
        else
        {
            s_t_sport_card card = game_data._instance.get_t_sport_card(m_rewards[m_index].card);
            sys._instance.m_self.add_reward(card.type, card.value1, card.value2, card.value3, false, game_data._instance.get_t_language("baowu_yjsaodang.cs_69_89"));//夺宝奇兵一键扫荡获得
        }

        m_nengliang -= 2;
        if (m_nengliang < 0)
        {
            m_nengliang += 10;
            sys._instance.m_self.remove_item(10010007, 1, game_data._instance.get_t_language("baowu_yjsaodang.cs_85_58"));//夺宝奇兵一键扫荡消耗
            GameObject obj1 = game_data._instance.ins_object_res("ui/baowu_yjsaodang_sub1");
            obj1.transform.parent = m_view.transform;
            obj1.transform.localScale = Vector3.one;
            obj1.transform.localPosition = new Vector3(0, 163 - m_len, 0);
            m_len += 42;
        }
        sys._instance.m_self.set_att(e_player_attr.player_treasure_energy, m_nengliang, false);
        GameObject obj = game_data._instance.ins_object_res("ui/qiangduo_saodang_sub");
        obj.transform.parent = m_view.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(0, 93 - m_len, 0);
        obj.GetComponent<qiangduo_saodang_sub>().reward = m_rewards[m_index];
        obj.GetComponent<qiangduo_saodang_sub>().m_num = m_index + 1;
        obj.GetComponent<qiangduo_saodang_sub>().init();

        m_len += 158;

        if (m_index >= m_rewards.Count - 1)
        {
            GameObject obj1 = game_data._instance.ins_object_res("ui/baowu_yjsaodang_sub");
            obj1.transform.parent = m_view.transform;
            obj1.transform.localScale = Vector3.one;
            obj1.transform.localPosition = new Vector3(0, 160 - m_len, 0);
            if (!m_suc)
            {
                obj1.transform.Find("name").GetComponent<UILabel>().text = "[ff0000]" + game_data._instance.get_t_language("baowu_yjsaodang.cs_111_81");//能量不足，抢夺失败。
            }
            else
            {
                obj1.transform.Find("name").GetComponent<UILabel>().text =
                    "[ffff00]" + string.Format(game_data._instance.get_t_language("baowu_yjsaodang.cs_116_47"), sys._instance.get_res_info(6, m_bid, 0, 0));//恭喜你获得了{0}的全部碎片
            }
            m_len += 50;
        }

        int y = 372 - m_len;
        if (y < 0)
        {
            SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
                              new Vector3(0, -y, 0), 8.0f).onFinished = OnFinished;
        }
        m_nl_num.GetComponent<UILabel>().text = sys._instance.m_self.get_item_num(10010007).ToString();
        m_time = 0.5f;
    }

    void end()
    {
        m_close.GetComponent<BoxCollider>().enabled = true;
        m_close.GetComponent<UISprite>().alpha = 1.0f;
        m_close1.GetComponent<BoxCollider>().enabled = true;
        m_close1.GetComponent<UISprite>().alpha = 1.0f;
    }

    void Update()
    {
        if (m_index >= m_rewards.Count - 1)
        {
            return;
        }
        if (m_time > 0)
        {
            m_time -= Time.deltaTime;
            if (m_time <= 0)
            {
                m_index++;
                deal();
                if (m_index >= m_rewards.Count - 1)
                {
                    end();
                    return;
                }
            }
        }
    }
}
