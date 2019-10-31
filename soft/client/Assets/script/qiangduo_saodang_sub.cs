
using UnityEngine;
using System.Collections;
using System;

public class qiangduo_saodang_sub : MonoBehaviour{
	public int m_num;
	// Use this for initialization
    public protocol.game.smsg_treasure_fight_end reward;

	public GameObject m_gold_obj;
	public GameObject m_icon_obj;
	public GameObject m_label_obj;
	public GameObject m_power_obj;

		
	public void init() {
        int gold = 0;
        int exp = 0;
        for (int i = 0; i < reward.types.Count; i++)
        {
            if (reward.value1s[i] == 1)
            {
                gold = reward.value2s[i];
            }
            if (reward.value1s[i] == 4)
            {
                exp = reward.value2s[i];
            }
        }

		this.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena_saodang_sub.cs_35_81"),m_num.ToString() );//第{0}战
        m_gold_obj.GetComponent<UILabel>().text = game_data._instance.get_t_resource(1).namecolor + " " + gold.ToString();
        m_power_obj.GetComponent<UILabel>().text = game_data._instance.get_t_resource(4).namecolor + " " + exp;
		if(reward.suipian_id != 0)
		{
            s_t_item _item = game_data._instance.get_item(reward.suipian_id);

            m_label_obj.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("qiangduo_saodang_sub.cs_49_69"),sys._instance.get_res_info(2, reward.suipian_id, 0, 0));//[ffff00]恭喜您![-]成功获得[{0}][-]
		}
		else
		{
			string s2 = game_data._instance.get_t_language ("qiangduo_saodang_sub.cs_53_15");//夺宝失败，请再接再厉。
			m_label_obj.GetComponent<UILabel>().text = game_data._instance.get_t_language ("qiangduo_saodang_sub.cs_54_46");//[ff0000]夺宝失败，请再接再厉。
		}
        if (reward.pgold > 0)
        {
            GameObject icon = icon_manager._instance.create_resource_icon(1,reward.pgold);
            icon.transform.parent = this.transform.Find("icon");
            icon.transform.localScale = Vector3.one;
            icon.transform.localPosition = Vector3.zero;
        }
        else
        {
            s_t_sport_card card = game_data._instance.get_t_sport_card(reward.card);
            GameObject icon = icon_manager._instance.create_reward_icon(card.type,card.value1,card.value2,card.value3);
            icon.transform.parent = this.transform.Find("icon");
            icon.transform.localScale = Vector3.one;
            icon.transform.localPosition = Vector3.zero;
        }
	}
}
