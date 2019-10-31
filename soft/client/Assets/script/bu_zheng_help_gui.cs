using UnityEngine;

public class bu_zheng_help_gui : MonoBehaviour
{
    public void click(GameObject obj)
    {
        if (obj.transform.name == "help_close")
        {
            s_message _msg = new s_message();
            _msg.m_type = "hide_bz_help_gui";
            cmessage_center._instance.add_message(_msg);
        }
    }
}
