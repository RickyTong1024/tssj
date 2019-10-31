
using UnityEngine;
using System.Collections;

public class chenghao_dialog_box : MonoBehaviour {
    public GameObject m_info_name;
    public GameObject m_info_desc;
    public GameObject m_info_sx;
    public GameObject m_info_time;
    public void reset(int m_id)
    {
		s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao (m_id);
        sys._instance.get_chenghao(m_id,m_info_name);
		string text = "";
		for(int i = 0;i < t_chenghao.attr.Count;++i)
		{
			if(i == t_chenghao.attr.Count - 1)
			{
				text += game_data._instance.get_value_string(t_chenghao.attr[i].attr,t_chenghao.attr[i].value);
			}
			else
			{
				text += game_data._instance.get_value_string(t_chenghao.attr[i].attr,t_chenghao.attr[i].value) + " ";
			}
		}
		if(text == "")
		{
			text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_187_79");//无属性加成
		}
		m_info_desc.GetComponent<UILabel>().text = t_chenghao.desc;
        if (!sys._instance.m_self.m_t_player.chenghao.Contains(t_chenghao.id))
        {
            m_info_time.SetActive(false);
        }
        else
        {
           
        }
        if (t_chenghao.time > 0)
        {
            m_info_time.SetActive(true);
            m_info_time.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("chenghao_dialog_box.cs_41_69"),t_chenghao.time);//可获得称号{0}天
        }
        else
        {
            m_info_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chenghao_dialog_box.cs_45_55");//可获得永久称号
        }
		m_info_sx.GetComponent<UILabel>().text = text;
	
    }
    void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            this.gameObject.SetActive(false);
        }
    }
}
