using System.Collections.Generic;
using UnityEngine;

public class attack_num_show : MonoBehaviour
{

    public Vector3 m_position;
    public float m_y = 0;
    public float m_depth = 0.0f;
    public GameObject m_root;
    public GameObject m_s;

    public void reset(List<string> ss)
    {
        for (int i = 0; i < ss.Count; ++i)
        {
            string s = ss[i];
            GameObject _item = (GameObject)Instantiate(m_s);
            _item.transform.parent = m_root.transform;
            _item.transform.localPosition = new Vector3(0, -i * 32 + (ss.Count - 1) * 16, 0);
            _item.transform.localScale = new Vector3(1, 1, 1);
            _item.SetActive(true);
            _item.GetComponent<UILabel>().gradientBottom = set_bottom_color(s);
            _item.GetComponent<UILabel>().gradientTop = set_top_color(s);
            _item.GetComponent<UILabel>().text = game_data._instance.get_t_language(s);
            _item.GetComponent<UILabel>().MakePixelPerfect();
        }
    }

    Color set_top_color(string word_id)
    {
        Color _color = new Color(254.0f / 255.0f, 242.0f / 255.0f, 180.0f / 255.0f);
        switch (word_id)
        {
            case "pojia_word":
                _color = new Color(86.0f / 255.0f, 166.0f / 255.0f, 255.0f / 255.0f);
                break;
            case "gd_zdword":
                _color = new Color(255.0f / 255.0f, 254.0f / 255.0f, 240.0f / 255.0f);
                break;
            case "shubei_word":
                _color = new Color(255.0f / 255.0f, 253.0f / 255.0f, 162.0f / 255.0f);
                break;
            case "bj_zdword":
                _color = new Color(255.0f / 255.0f, 248.0f / 255.0f, 198.0f / 255.0f);
                break;
            case "shanbi_zdword":
                _color = new Color(255.0f / 255.0f, 240.0f / 255.0f, 208.0f / 255.0f);
                break;
            case "mfmy_word":
                _color = new Color(228.0f / 255.0f, 144.0f / 255.0f, 255.0f / 255.0f);
                break;
            case "wlmy_word":
                _color = new Color(254.0f / 255.0f, 189.0f / 255.0f, 119.0f / 255.0f);
                break;
        }
        return _color;
    }

    Color set_bottom_color(string word_id)
    {
        Color _color = new Color(181.0f / 255.0f, 142.0f / 255.0f, 21.0f / 255.0f);
        switch (word_id)
        {
            case "pojia_word":
                _color = new Color(144.0f / 255.0f, 48.0f / 255.0f, 250.0f / 255.0f);
                break;
            case "gd_zdword":
                _color = new Color(144.0f / 255.0f, 48.0f / 255.0f, 250.0f / 255.0f);
                break;
            case "shubei_word":
                _color = new Color(255.0f / 255.0f, 211.0f / 255.0f, 47.0f / 255.0f);
                break;
            case "bj_zdword":
                _color = new Color(253.0f / 255.0f, 88.0f / 255.0f, 0.0f / 255.0f);
                break;
            case "shanbi_zdword":
                _color = new Color(255.0f / 255.0f, 120.0f / 255.0f, 0.0f / 255.0f);
                break;
            case "mfmy_word":
                _color = new Color(171.0f / 255.0f, 344.0f / 255.0f, 254.0f / 255.0f);
                break;
            case "wlmy_word":
                _color = new Color(254.0f / 255.0f, 87.0f / 255.0f, 49.0f / 255.0f);
                break;
        }
        return _color;
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 _pos = sys._instance.WorldToScreenPoint(m_position);
        _pos.z = m_depth;
        _pos.y += m_y;
        this.transform.localPosition = _pos;
    }
}
