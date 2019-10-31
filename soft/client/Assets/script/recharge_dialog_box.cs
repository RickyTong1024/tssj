
using UnityEngine;
using System.Collections;

public class recharge_dialog_box : MonoBehaviour {

	public delegate void rdb_method();
	public rdb_method m_method;
	public UILabel m_name;
	public UILabel m_desc;
	public UILabel m_close_Label;
	public UILabel m_recharge_Label;
	// Use this for initialization
	void Start () {

	}

	public void set_func(rdb_method method)
	{
		m_method = method;
	}

	public void hide()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();
      	s_message _message = new s_message();
        _message.m_type = "guildfightpvp_flag";
		cmessage_center._instance.add_message(_message);
        
	}

	public void ok()
	{
		this.transform.Find("frame_big").GetComponent<frame>().hide();

		s_message _message = new s_message();
		_message.m_type = "show_recharge";
		cmessage_center._instance.add_message(_message);

        s_message _mes1 = new s_message();
        _mes1.m_type = "show_main_gui";
        cmessage_center._instance.add_message(_mes1);

        s_message _mes = new s_message();
        _mes.m_type = "hide_recycle_gui";
        cmessage_center._instance.add_message(_mes);

		m_method.Invoke ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
