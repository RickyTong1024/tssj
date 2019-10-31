using System.Collections.Generic;
using UnityEngine;

public class bf_gui : MonoBehaviour
{

    public GameObject m_panel;
    public GameObject[] m_nums;
    public GameObject[] m_nums1;
    public GameObject m_bf;
    public GameObject m_ss;
    public int m_num;
    private List<int> m_num_list = new List<int>();
    private List<int> m_num_list1 = new List<int>();
    private List<int> m_speed = new List<int>();
    private int m_index;
    private int m_cishu;
    private bool m_finish = false;
    private float m_time;

    void Start()
    {

        if (game_data._instance.m_language == e_language.English)
        {
            m_bf.GetComponent<UISprite>().spriteName = "fight1_word";
        }
        else if (game_data._instance.m_language == e_language.Simplified)
        {
            m_bf.GetComponent<UISprite>().spriteName = "fight1_word_cnj";
        }
        else
        {
            m_bf.GetComponent<UISprite>().spriteName = "fight1_word_cn";
        }
    }

    void OnEnable()
    {
        reset(m_num);
    }

    public void reset(int num)
    {
        m_num_list.Clear();
        m_num_list1.Clear();
        m_speed.Clear();
        m_num = num;
        m_index = 0;
        m_cishu = 0;
        m_finish = false;
        m_time = 0;
        while (num > 0 && m_num_list.Count < m_nums.Length)
        {
            m_num_list.Add(num % 10);
            num = num / 10;
        }
        for (int i = 0; i < m_num_list.Count; ++i)
        {
            m_num_list1.Add(Random.Range(0, 10));
            m_speed.Add(Random.Range(600, 1000));
        }
        int w = m_num_list.Count * 34;
        for (int i = 0; i < m_nums.Length; ++i)
        {
            if (i < m_num_list.Count)
            {
                m_nums[i].SetActive(true);
                m_nums[i].transform.localPosition = new Vector3(w / 2 - i * 34 - 18, 0, 0);
            }
            else
            {
                m_nums[i].SetActive(false);
            }
        }
        m_panel.GetComponent<UIPanel>().baseClipRegion = new Vector4(0, 0, w, 50);
        m_bf.transform.localPosition = new Vector3(-w / 2 - 2, 4, 0);
        m_ss.transform.localPosition = new Vector3(w / 2 + 2, 6, 0);

        for (int i = 0; i < m_num_list.Count; ++i)
        {
            set_num(m_nums[i], m_num_list1[i]);
            set_num(m_nums[i], m_num_list1[i] + 1);
        }
    }

    void set_num(GameObject obj, int num)
    {
        obj.GetComponent<UILabel>().text = (num % 10).ToString();
    }

    void finish()
    {
        this.GetComponent<ui_show_anim>().hide_ui();
    }

    void Update()
    {
        for (int i = m_index; i < m_num_list.Count; ++i)
        {
            Vector3 v = m_nums[i].transform.localPosition;
            v.y += Time.deltaTime * m_speed[i];
            while (v.y > 44)
            {
                v.y -= 44;
                m_num_list1[i]++;
                if (m_num_list1[i] == 10)
                {
                    m_num_list1[i] = 0;
                }
                set_num(m_nums[i], m_num_list1[i]);
                set_num(m_nums1[i], m_num_list1[i] + 1);
                if (i == m_index)
                {
                    m_cishu++;
                    int cs = 2;
                    if (i == 0)
                    {
                        cs = 8;
                    }
                    if (m_cishu > cs && m_num_list1[i] == m_num_list[i])
                    {
                        m_index++;
                        m_cishu = 0;
                        v.y = 0;
                    }
                }
            }
            m_nums[i].transform.localPosition = v;
        }
        if (m_index >= m_num_list.Count)
        {
            m_time += Time.deltaTime;
            if (!m_finish && m_time > 1)
            {
                m_finish = true;
                finish();
            }
        }
    }
}
