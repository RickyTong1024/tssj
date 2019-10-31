
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class common_treasure_page_gui : MonoBehaviour,IMessage {

	public GameObject m_ok;
	public GameObject m_treasure_page_gui;
	public GameObject m_next;
	public dhc.treasure_t m_treasure;
	private dhc.treasure_t m_treasure1;
	private ulong _gui;



	// Use this for initialization
	void Start () {

		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void init()
	{
		if(m_treasure_page_gui == null)
		{
			m_treasure_page_gui = game_data._instance.ins_object_res("ui/treasure_page_gui");
			m_treasure_page_gui.transform.parent = this.transform;
			m_treasure_page_gui.transform.localPosition = new Vector3(0,0,0);
			m_treasure_page_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_treasure_page_gui.SetActive (true);
	}
	
	void IMessage.net_message(s_net_message message)
	{
		
	}
	
	void IMessage.message(s_message message)
	{
		if(message.m_type == "common_select1_treasure")
		{
			if(m_treasure != null)
			{
				_gui = (ulong)message.m_long[0];
				this.gameObject.GetComponent<ui_show_anim>().hide_ui ();
				s_message _msg = new s_message();
				_msg.m_type = "common_select_treasure";
				_msg.m_long.Add (_gui);
				cmessage_center._instance.add_message(_msg);
				if(m_next != null)
				{
					m_next.SetActive(true);
				}
			}
			else
			{
				this.gameObject.GetComponent<ui_show_anim>().hide_ui ();
				_gui = (ulong)message.m_long[0];
				s_message _msg = new s_message();
				_msg.m_type = "common_select_treasure";
				_msg.m_long.Add (_gui);
				cmessage_center._instance.add_message(_msg);
				if(m_next != null)
				{
					m_next.SetActive(true);
				}
			}
		}
		if(message.m_type == "common_ronglian_treasure")
		{
			this.gameObject.GetComponent<ui_show_anim>().hide_ui ();
			_gui = (ulong)message.m_long[0];
			s_message _msg = new s_message();
			_msg.m_type = "common_ronglian1_treasure";
			_msg.m_long.Add (_gui);
			cmessage_center._instance.add_message(_msg);
			if(m_next != null)
			{
				m_next.SetActive(true);
			}
		}
	}
	public void OnDisable()
	{
		m_ok.SetActive (false);
	}
	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.gameObject.GetComponent<ui_show_anim>().hide_ui ();
			if(m_next != null)
			{
				m_next.SetActive(true);
			}
		}
		else if(obj.transform.name == "ok")
		{
			this.gameObject.GetComponent<ui_show_anim>().hide_ui ();
			s_message _msg = new s_message();
			_msg.m_type = "common_devour1_treasure";
			cmessage_center._instance.add_message(_msg);
			if(m_next != null)
			{
				m_next.SetActive(true);
			}
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
