using UnityEngine;

public class arena_saodang_sub : MonoBehaviour{
	public int m_num;
	public protocol.game.smsg_sport_fight_end reward;

	public GameObject m_gold_obj;
	public GameObject m_exp_obj;
	public GameObject m_icon_obj;
	public GameObject m_power_obj;
	public GameObject m_sb;

    public void init()
    {
        int gold = 0;
        int exp = 0;
        int power = 0;
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
            if (reward.value1s[i] == 14)
            {
                power = reward.value2s[i];
            }
        }
        this.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena_saodang_sub.cs_35_81"), m_num.ToString());//第{0}战
        m_gold_obj.GetComponent<UILabel>().text = game_data._instance.get_t_resource(1).namecolor + " " + gold.ToString();
        m_exp_obj.GetComponent<UILabel>().text = game_data._instance.get_t_resource(4).namecolor + " " + exp.ToString();
        m_power_obj.GetComponent<UILabel>().text = game_data._instance.get_t_resource(14).namecolor + " " + power.ToString();
        if (reward.result != 1)
        {
            m_icon_obj.SetActive(false);
            m_sb.SetActive(true);
        }
        else
        {
            if (reward.pgold > 0)
            {
                GameObject icon = icon_manager._instance.create_resource_icon(1, reward.pgold);
                icon.transform.parent = m_icon_obj.transform;
                icon.transform.localScale = Vector3.one;
                icon.transform.localPosition = Vector3.zero;
            }
            else if (reward.result > 0)
            {
                s_t_sport_card card = game_data._instance.get_t_sport_card(reward.cid);
                GameObject icon = icon_manager._instance.create_reward_icon(card.type, card.value1, card.value2, card.value3);
                icon.transform.parent = m_icon_obj.transform;
                icon.transform.localScale = Vector3.one;
                icon.transform.localPosition = Vector3.zero;
            }
        }
    }
}
