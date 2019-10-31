
using UnityEngine;
using System.Collections;

public class mm_show_gui : MonoBehaviour {
	
	public static bool m_new = false;
	public GameObject m_vr;
	public GameObject m_cam;
	// Use this for initialization
	void Start () {
	}

	public void OnEnable()
	{
		m_new = false;
		s_message _message = new s_message();
		_message.m_type = "show_mm_show_gui";
		cmessage_center._instance.add_message(_message);
	}
	
	public void OnDisable()
	{
		s_message _message = new s_message();
		_message.m_type = "hide_mm_show_gui";
		cmessage_center._instance.add_message(_message);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			//transform.GetComponent<ui_title_anim>().hide_ui();
			this.gameObject.SetActive(false);

			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);

			_message = new s_message();
			_message.m_type = "hide_mm_show";
			cmessage_center._instance.add_message(_message);

		}

		if(obj.transform.name == "sz")
		{
			s_message _out = new s_message();
			
			_out.m_type = "show_help_gui";

			cmessage_center._instance.add_message(_out);

			this.gameObject.SetActive(false);
		}

		if(obj.transform.name == "vr")
		{
			m_vr.SetActive(false);
			m_cam.SetActive(true);

			s_message _out = new s_message();
			
			_out.m_type = "hide_vr";
			
			cmessage_center._instance.add_message(_out);
		}
	
		if(obj.transform.name == "show_vr")
		{
			m_vr.SetActive(true);
			m_cam.SetActive(false);

			s_message _out = new s_message();
			
			_out.m_type = "show_vr";
			
			cmessage_center._instance.add_message(_out);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
