
using UnityEngine;
using System.Collections;

public class jd_dialog_box : MonoBehaviour {

	public delegate void rdb_method();
	public rdb_method m_method;
	public UILabel m_name;
	public UILabel m_desc;
	public UILabel m_close_Label;
	public UILabel m_ok_Label;
	// Use this for initialization
	void Start () {

	}

	public void hide()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();
	}
	
	public void ok()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();
		
		s_message _message = new s_message();
		_message.m_type = "show_jd_gui";
		cmessage_center._instance.add_message(_message);

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
