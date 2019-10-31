
using UnityEngine;
using System.Collections;

public class common_item_page_gui : MonoBehaviour,IMessage {

	public GameObject m_ok;
	public GameObject m_remove;
	public GameObject m_item_page_gui;
	public GameObject m_next;
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
		if(m_item_page_gui == null)
		{
			m_item_page_gui = game_data._instance.ins_object_res("ui/item_page_gui");
			m_item_page_gui.transform.parent = this.transform;
			m_item_page_gui.transform.localPosition = new Vector3(0,0,0);
			m_item_page_gui.transform.localScale = new Vector3(1,1,1);
			m_item_page_gui.SetActive (true);
		}
	}
	
	void IMessage.net_message(s_net_message message)
	{
		
	}
	
	void IMessage.message(s_message message)
	{

	}
	
	public void OnDisable()
	{
		m_ok.SetActive (false);
		m_remove.SetActive (false);
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
			_msg.m_type = "common_select_item1";
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
