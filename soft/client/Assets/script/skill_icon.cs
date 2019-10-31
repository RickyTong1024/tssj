
using UnityEngine;
using System.Collections;

public class skill_icon : MonoBehaviour {

	public s_t_skill m_skill;
	public role_skill m_role_skill;
	// Use this for initialization
	void Start () {
	
	}

	public void init()
	{
		this.GetComponent<UISprite>().alpha = 1.0f;
		this.transform.GetComponent<BoxCollider>().enabled = true;
		this.transform.localEulerAngles = new Vector3(0,0,0);
	}
	
	public void reset()
	{
		this.transform.GetComponent<UISprite>().spriteName = m_skill.icon;
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	public void press()
	{
		s_message _message = new s_message ();
		
		_message.m_type = "item_dialog_box";
		_message.m_ints.Add (m_skill.id);
		_message.m_ints.Add (4);
		if(m_skill.type == 4)
		{
			_message.m_string.Add (m_role_skill.get_des(m_role_skill.level() -1));
		}
		else
		{
			_message.m_string.Add (m_role_skill.get_des());
		}
		cmessage_center._instance.add_message (_message);
	}

	public void release()
	{
		s_message _message = new s_message ();
		
		_message.m_type = "hide_min_dialog_box";
		
		cmessage_center._instance.add_message (_message);
	}
}
