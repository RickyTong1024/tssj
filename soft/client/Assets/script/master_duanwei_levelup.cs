
using UnityEngine;
using System.Collections;

public class master_duanwei_levelup : MonoBehaviour{

    public GameObject m_left;
    public GameObject m_right;
    public s_message m_msg;
    public UILabel m_text;
	void Start () 
    {
        
	
	}
    void OnDestroy()
    {
        
    }
    public void reset(int leftid, int rightid)
    {
        if (leftid < rightid)
        {
            m_text.text = game_data._instance.get_t_language ("master_duanwei_levelup.cs_23_26");//段位升级
            m_right.transform.Find("down").gameObject.SetActive(false);
            m_right.transform.Find("up").gameObject.SetActive(true);

        }
        else 
        {
            m_text.text = game_data._instance.get_t_language ("master_duanwei_levelup.cs_30_26");//段位降级
            m_right.transform.Find("down").gameObject.SetActive(true);
            m_right.transform.Find("up").gameObject.SetActive(false);
        }
        set_duanwei(leftid, m_left);
        set_duanwei(rightid, m_right);
 
    }
    public void click(GameObject obj)
    {
        cmessage_center._instance.add_message(m_msg);
        Destroy(this.gameObject);
 
    }
    void set_duanwei(int id,GameObject m_duanwei)
    {
        s_t_master_duanwei duanwei = game_data._instance.get_t_master_duanwei(id);
        if (duanwei != null)
        {
            m_duanwei.transform.Find("name").GetComponent<UILabel>().text = duanwei.duanwei;

        }
        else
        {
            m_duanwei.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language ("masterleague_gui.cs_416_29");//青铜
            return;
        }
        m_duanwei.transform.Find("name").GetComponent<UILabel>().text = duanwei.duanwei;
        m_duanwei.transform.Find("name").GetComponent<UILabel>().gradientTop = sys._instance.string_to_color(duanwei.topcolor);
        m_duanwei.transform.Find("name").GetComponent<UILabel>().gradientBottom = sys._instance.string_to_color(duanwei.bottomcolor);
        m_duanwei.transform.Find("duanwei").GetComponent<UISprite>().spriteName = duanwei.icon;
        m_duanwei.transform.Find("kuang").GetComponent<UISprite>().spriteName = duanwei.kuang;
        for (int i = 1; i <= 3; i++)
        {
            if (i <= duanwei.starcount)
            {
                m_duanwei.transform.Find("" + i).GetComponent<UISprite>().spriteName = duanwei.staricon;

                m_duanwei.transform.Find("" + i).gameObject.SetActive(true);
            }
            else
            {
                m_duanwei.transform.Find("" + i).gameObject.SetActive(false);
            }

        }
        string des = game_data._instance.get_value_string(duanwei.attr1, (float) duanwei.value1, 1);
        des = des.Replace("Team ", "");
        des = des.Replace("(PvP)", "");
        m_duanwei.transform.Find("attr1").GetComponent<UILabel>().text = des;
        des = game_data._instance.get_value_string(duanwei.attr2, (float) duanwei.value2, 1);
        des = des.Replace("Team ", "");
        des = des.Replace("(PvP)", "");
        m_duanwei.transform.Find("attr2").GetComponent<UILabel>().text = des;
       
    }
	
}
