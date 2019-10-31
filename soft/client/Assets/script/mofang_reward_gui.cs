
using UnityEngine;
using System.Collections;

public class mofang_reward_gui : MonoBehaviour{
    public GameObject m_view1;
    public GameObject m_view2;
    public GameObject m_view3;
	void Start () 
    {
	
	}
    void OnEnable()
    {
        reset();
    }
    void OnDestroy()
    {

    }
    public void reset()
    {
        reset_view(m_view1);
        reset_view(m_view2);
        reset_view(m_view3);
        int type_1 = -1;
        int type_2 = -1;
        int type_3 = -1;
        int type = 0;
        GameObject view = m_view1;    
        foreach (int id in game_data._instance.m_mofangs.Keys)
        {
            s_t_mofang mofang = game_data._instance.m_mofangs[id];
            if (mofang.leixing == 1)
            {
                type_1++;
                type = type_1;
                view = m_view1;
            }
            else if (mofang.leixing == 2)
            {
                type_2++;
                type = type_2;
                view = m_view2;
            }
            else
            {
                type_3++;
                type = type_3;
                view = m_view3;
            }

            GameObject item = game_data._instance.ins_object_res("ui/mofang_sub");
            item.transform.parent = view.transform;
            item.transform.localPosition = new Vector3(0, 121 - 83 * type, 0);
            item.transform.localScale = new Vector3(1, 1, 1);

            item.transform.Find("name").GetComponent<UILabel>().text = sys._instance.m_self.get_name(mofang.type, mofang.value1, mofang.value2, mofang.value3);
            GameObject icon = item.transform.Find("icon").gameObject;
            sys._instance.remove_child(icon);
            GameObject obj = icon_manager._instance.create_reward_icon(mofang.type, mofang.value1, mofang.value2, mofang.value3);
            obj.transform.parent = icon.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.localScale = new Vector3(1, 1, 1);

        }

    }
    public void click(GameObject obj)
    {
 

    }
    void reset_view(GameObject m_view)
    {
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
 
    }
}
