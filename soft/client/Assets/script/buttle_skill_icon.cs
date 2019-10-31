using UnityEngine;

public class buttle_skill_icon : MonoBehaviour
{
    public GameObject m_icon;
    public GameObject m_anim;
    public GameObject m_unit;
    public int m_id = 0;
    public bool m_can_release = true;

    void Start()
    {
        m_icon.SetActive(false);
        m_anim.SetActive(false);
    }

    public void set_unit(GameObject obj)
    {
        m_unit = obj;
        m_icon.GetComponent<UISprite>().spriteName = m_unit.GetComponent<unit>().m_t_class.icon;
        m_icon.GetComponent<UISprite>().color = new Color(1, 1, 1, 0.5f);
    }
    void button()
    {
        if (m_unit == null)
        {
            return;
        }
        if (m_unit.GetComponent<unit>().m_release_skill == true)
        {
            return;
        }
        if (cost_mp(false) == false)
        {
            return;
        }
        m_unit.GetComponent<unit>().release_skill();
    }

    bool cost_mp(bool check)
    {
        float _need_mp = 100;

        if (m_unit.GetComponent<unit>().m_t_class.job == 3)
        {
            _need_mp = 75;
        }
        float _val = sys._instance.m_self.get_att(e_player_attr.player_mp);
        if (_val > _need_mp)
        {
            if (check == false)
            {
                sys._instance.m_self.set_att(e_player_attr.player_mp, (int)(_val - _need_mp));
            }
            return true;
        }
        return false;
    }

    public void update_ui()
    {
        if (m_unit == null)
        {
            return;
        }
        if (cost_mp(true) && m_unit.GetComponent<unit>().m_release_skill == false)
        {
            m_icon.GetComponent<UISprite>().color = new Color(1, 1, 1, 1);
            m_anim.SetActive(true);
        }
        else if (m_anim.activeSelf == true)
        {
            m_icon.GetComponent<UISprite>().color = new Color(1, 1, 1, 0.5f);
            m_anim.SetActive(false);
        }
    }

    void Update()
    {
        if (m_unit == null)
        {
            m_icon.SetActive(false);
            m_anim.SetActive(false);
            return;
        }
        else if (m_icon.activeSelf == false)
        {
            m_icon.SetActive(true);
        }
        update_ui();
    }
}

