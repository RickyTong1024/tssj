using UnityEngine;

public class catch_box : MonoBehaviour
{
    public s_t_reward m_reward;
    public s_message m_out_message;
    public string m_name;
    public float m_time;

    public void hide()
    {
        transform.GetComponent<ui_bloom_anim>().hide_ui();
    }

    public void ok()
    {
        if (m_time > 0)
        {
            return;
        }
        hide();
        cmessage_center._instance.add_message(m_out_message);
    }

    public void update_ui()
    {
        this.transform.Find("back").Find("name").GetComponent<UILabel>().text = m_name;
        sys._instance.remove_child(this.transform.Find("back").Find("icon").gameObject);
        GameObject icon1 = icon_manager._instance.create_reward_icon(m_reward.type, m_reward.value1, m_reward.value2, m_reward.value3);
        icon1.transform.parent = this.transform.Find("back").Find("icon");
        icon1.transform.localPosition = new Vector3(0, 0, 0);
        icon1.transform.localScale = new Vector3(1, 1, 1);
        m_time = 0.5f;
    }

    void Update()
    {
        if (m_time > 0)
        {
            m_time -= Time.deltaTime;
        }
    }
}
