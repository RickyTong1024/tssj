using System.Collections.Generic;
using UnityEngine;

public class boss_saodang : MonoBehaviour
{

    public GameObject m_view;
    public GameObject m_close;
    public GameObject m_close1;
    private float m_time;
    private int m_index;
    private int m_len;
    List<int> xunzhnags;
    List<long> hits;
    List<int> levels;
    public UILabel m_desc;
    public UILabel m_num;
    int num;

    void OnFinished()
    {

    }

    public void init(List<int> xunzhnags, List<long> hits, List<int> levels, int num1)//List<int> golds,List<int> powders,List<int> suipians
    {
        this.num = num1;
        if (num < 0)
        {
            num = 0;
        }
        this.xunzhnags = xunzhnags;
        this.hits = hits;
        this.levels = levels;
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

        m_time = 0.5f;
        m_index = -1;
        m_len = 0;
        m_desc.text = game_data._instance.get_t_language("boss_saodang.cs_51_22");//消耗：
        m_desc.transform.parent.gameObject.SetActive(false);
    }

    public void click(GameObject obj)
    {
        if (obj.name == "close" || obj.name == "hide")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
    }

    void deal()
    {
        GameObject obj = game_data._instance.ins_object_res("ui/boss_saodang_sub");
        obj.transform.parent = m_view.transform;
        obj.transform.localScale = Vector3.one;
        obj.transform.localPosition = new Vector3(0, 93 - m_index * 158, 0);
        obj.GetComponent<boss_saodang_sub>().init(xunzhnags[m_index], hits[m_index], levels[m_index], m_index + 1);

        m_len += 158;
        int y = 372 - m_len;
        if (y < 0)
        {
            SpringPanel.Begin(m_view.GetComponent<UIScrollView>().panel.cachedGameObject,
                              new Vector3(0, -y, 0), 8.0f).onFinished = OnFinished;
        }
        m_time = 0.5f;
    }

    void end()
    {
        m_close.GetComponent<BoxCollider>().enabled = true;
        m_close.GetComponent<UISprite>().alpha = 1.0f;
        m_close1.GetComponent<BoxCollider>().enabled = true;
        m_close1.GetComponent<UISprite>().alpha = 1.0f;
        if (num <= 0)
        {
            m_num.gameObject.SetActive(false);
        }
        else
        {
            m_num.text = "x" + num + "";
            m_num.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        if (m_index >= hits.Count - 1)
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
                if (m_index >= hits.Count - 1)
                {
                    end();
                    return;
                }
            }
        }
    }
}
