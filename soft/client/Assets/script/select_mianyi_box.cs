
using UnityEngine;
using System.Collections;

public class select_mianyi_box : MonoBehaviour {

	//public GameObject m_name;
	public GameObject m_des;
	public s_message m_out_message;
	public delegate void rdb_method();
	public rdb_method m_method;

	public UILabel m_ok_Label;
	public UILabel m_close_Label;
	// Use this for initialization
	void Start () {

	}
	public void set_des(rdb_method method,string des,s_message ok_msg)
	{
		//m_name.GetComponent<UILabel>().text = (string)msg.m_string [0];
		m_des.GetComponent<UILabel>().text = des;
		m_out_message = ok_msg;
		m_method = method;
	}
	public void change()
	{
		transform.GetComponent<ui_show_anim>().hide_ui();
		
		s_message _message = new s_message();
		_message.m_type = "show_bu_zheng_gui";
		cmessage_center._instance.add_message(_message);
		
		m_method.Invoke ();

	}
	public void fight()
	{
		transform.GetComponent<ui_show_anim>().hide_ui();
		cmessage_center._instance.add_message(m_out_message);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
