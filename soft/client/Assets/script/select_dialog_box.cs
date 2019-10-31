
using UnityEngine;
using System.Collections;

public class select_dialog_box : MonoBehaviour {

	public GameObject m_name;
	public GameObject m_des;
	public s_message m_out_message;

	public bool flag = false;
    public bool m_login = false;
	public UILabel m_ok_Label;
	public UILabel m_close_Label;
	// Use this for initialization
	void Start () {

	}
	public void set_des(s_message msg)
	{
		m_name.GetComponent<UILabel>().text = (string)msg.m_string [0];
		m_des.GetComponent<UILabel>().text = (string)msg.m_string [1];
        if (m_login)
        {
            m_close_Label.text = game_data._instance.get_t_language ("select_dialog_box.cs_24_33");//返回登录
            m_ok_Label.text = game_data._instance.get_t_language ("select_dialog_box.cs_25_30");//尝试重连
        }
        else
        {
            if (m_close_Label != null)
            {
                m_close_Label.text = game_data._instance.get_t_language ("select_dialog_box.cs_31_37");//取消
 
            }
            m_ok_Label.text = game_data._instance.get_t_language ("select_dialog_box.cs_34_30");//确定

 
        }
		m_out_message = msg;
	}
	public void hide()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();

		if(m_out_message.m_object.Count > 1 && !flag)
		{
			cmessage_center._instance.add_message((s_message)m_out_message.m_object [1]);
		}

	}
	public void ok()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();
		cmessage_center._instance.add_message((s_message)m_out_message.m_object [0]);
		if(m_out_message.m_object.Count > 1 && flag)
		{
			cmessage_center._instance.add_message((s_message)m_out_message.m_object [1]);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
