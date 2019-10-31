using UnityEngine;

public class boss_active : MonoBehaviour
{
    public int m_active_id;
    public long m_damage = 0;
    public int task_damage = 0;
    public int reward = 0;
    public GameObject m_icon;
    public int player_level = 0;

    public void reset()
    {
        s_t_boss_active t_boss_active = game_data._instance.get_t_boss_active(m_active_id);
        string damage = sys._instance.value_to_wan(m_damage);
        string c_damage = sys._instance.value_to_wan(task_damage);
        if (boss_task_gui.active_done(t_boss_active))
        {
            transform.Find("wcd").GetComponent<UILabel>().text = "[ffffff]" + game_data._instance.get_t_language("active.cs_19_75");//目标达成
            transform.Find("gou").gameObject.SetActive(true);
            transform.Find("main").gameObject.SetActive(false);
            transform.Find("main1").gameObject.SetActive(true);
        }
        else
        {
            transform.Find("wcd").GetComponent<UILabel>().text = game_data._instance.get_t_language("boss_active.cs_35_63") + damage + "/" + c_damage;//进度：[24d0fd]
            transform.Find("gou").gameObject.SetActive(false);
            transform.Find("main").gameObject.SetActive(true);
            transform.Find("main1").gameObject.SetActive(false);
        }
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(t_boss_active.type, t_boss_active.value1, reward, t_boss_active.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        string text = "[0aabff]" + game_data._instance.get_t_language("active.cs_45_16") + ":[-] ";//奖励
        text += sys._instance.get_res_info(t_boss_active.type, t_boss_active.value1, reward, t_boss_active.value3);
        string text1 = t_boss_active.desc.Replace("{n1}", c_damage);
        transform.Find("desc").GetComponent<UILabel>().text = text1;
        transform.Find("reward").GetComponent<UILabel>().text = text;
    }
}
