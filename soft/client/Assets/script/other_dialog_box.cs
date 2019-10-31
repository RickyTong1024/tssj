
using UnityEngine;
using System.Collections;

public class other_dialog_box : MonoBehaviour {

	public delegate void rdb_method();
	public rdb_method m_method;
	public s_message m_out_message;
	public GameObject m_des;

	public UILabel m_name;
	public UILabel m_close_Label;
	public UILabel m_recharge_Label;
	// Use this for initialization
	void Start () {

	}

	public void set_func(rdb_method method)
	{
		m_method = method;
	}

	public void set_des(s_message msg)
	{
		m_name.GetComponent<UILabel>().text = (string)msg.m_string [0];
		m_des.GetComponent<UILabel>().text = (string)msg.m_string [1];
		
		m_out_message = msg;
	}
	
	public void hide()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();
	}
	
	public void ok()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();
		cmessage_center._instance.add_message((s_message)m_out_message.m_object [0]);
		if(m_out_message.m_object.Count > 1)
		{
			cmessage_center._instance.add_message((s_message)m_out_message.m_object [1]);
		}
		
		m_method.Invoke ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
