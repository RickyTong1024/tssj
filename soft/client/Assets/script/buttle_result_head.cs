using UnityEngine;

public class buttle_result_head : MonoBehaviour
{
    public GameObject m_icon;
    public GameObject m_bar;
    public GameObject m_levelup;
    public GameObject m_name;
    public GameObject m_exp;
    public double m_target_exp = 0;
    public double m_cur_exp = 0;
    public int m_add_exp;
    public int m_id;
    public int m_level;
    public int m_max_level;

    public void OnEnable()
    {
        s_t_class _class = game_data._instance.get_t_class(m_id);

        if (_class == null)
        {
            this.gameObject.SetActive(false);
            return;
        }

        m_name.GetComponent<UILabel>().text = _class.name;
        m_exp.GetComponent<UILabel>().text = "EXP + " + m_add_exp.ToString();
        m_icon.GetComponent<UISprite>().spriteName = _class.icon;
        this.transform.GetComponent<ui_exp_bar_anim>().m_speed = 0.5f;
        this.transform.GetComponent<ui_exp_bar_anim>().m_levelup = onlevelup;
        this.transform.GetComponent<ui_exp_bar_anim>().set_property(1, m_max_level, m_level, (int)m_cur_exp, m_add_exp);
    }

    public void onlevelup()
    {
        m_levelup.SetActive(true);
    }
}
