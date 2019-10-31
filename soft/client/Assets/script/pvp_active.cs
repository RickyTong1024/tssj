
using UnityEngine;
using System.Collections;

public class pvp_active : MonoBehaviour {

    public int m_active_id;
    public int reward = 0;
    public GameObject m_icon;
    // Use this for initialization
    void Start()
    {

    }

    public void reset()
    {
        s_t_pvp_active t_boss_active = game_data._instance.get_t_pvp_active(m_active_id);
        transform.Find("yi").gameObject.SetActive(false);
        transform.Find("wcd").gameObject.SetActive(true);
        if (pvp_task_gui.get_state(m_active_id) == 3)
        {
            transform.Find("wcd").GetComponent<UILabel>().text = "[ffffff]" + game_data._instance.get_t_language ("active.cs_19_75");//目标达成
            transform.Find("gou").gameObject.SetActive(true);
            transform.Find("main").gameObject.SetActive(false);
            transform.Find("main1").gameObject.SetActive(true);
        }
        else if (pvp_task_gui.get_state(m_active_id) == 1)
        {
            transform.Find("wcd").GetComponent<UILabel>().text = game_data._instance.get_t_language ("boss_active.cs_35_63") + sys._instance.m_self.m_t_player.pvp_hit + "/" + t_boss_active.neednum;//进度：[24d0fd]
            transform.Find("gou").gameObject.SetActive(false);
            transform.Find("main").gameObject.SetActive(true);
            transform.Find("main1").gameObject.SetActive(false);
            transform.Find("yi").gameObject.SetActive(true);
            transform.Find("wcd").gameObject.SetActive(false);
 
        }
        else
        {
            transform.Find("wcd").GetComponent<UILabel>().text = game_data._instance.get_t_language ("boss_active.cs_35_63") + sys._instance.m_self.m_t_player.pvp_hit + "/" + t_boss_active.neednum;//进度：[24d0fd]
            transform.Find("gou").gameObject.SetActive(false);
            transform.Find("main").gameObject.SetActive(true);
            transform.Find("main1").gameObject.SetActive(false);
        }
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(1, 22, t_boss_active.lieb, 0);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        string text = "[0aabff]" + game_data._instance.get_t_language ("active.cs_45_16") + ":[-] ";//奖励
        text += sys._instance.get_res_info(1, 22, t_boss_active.lieb, 0);
        //string text1 = t_boss_active.desc.Replace("{n1}", c_damage);
        transform.Find("desc").GetComponent<UILabel>().text = t_boss_active.name;
        transform.Find("reward").GetComponent<UILabel>().text = text;
    }
}
