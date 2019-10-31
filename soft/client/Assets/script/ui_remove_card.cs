
using UnityEngine;
using System.Collections;

public class ui_remove_card : MonoBehaviour {

	public string m_out_message = "click_card";
	void Start () {

	}
	public void click()
	{
		s_message _msg = new s_message();
		
		_msg.m_type = m_out_message;
		_msg.m_long.Add ((ulong)0);
		cmessage_center._instance.add_message(_msg);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
