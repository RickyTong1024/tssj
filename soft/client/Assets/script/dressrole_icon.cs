
using UnityEngine;
using System.Collections;

public class dressrole_icon : MonoBehaviour {

	public int m_id;
	string name;
	string desc;
	public UIAtlas m_atlas;
	public UIAtlas m_atlas1;
	public string m_out_message ="";
	// Use this for initialization
	void Start () {

	}

	public void init()
	{
		UIDragScrollView uisv = this.GetComponent<UIDragScrollView>();
		if (uisv != null)
		{
			Object.Destroy(uisv);
		}
		this.GetComponent<UISprite>().alpha = 1.0f;
		this.transform.GetComponent<BoxCollider>().enabled = true;
		this.transform.localEulerAngles = new Vector3(0,0,0);
	}

	public void reset()
	{
		s_t_role_dress t_role_dress = game_data._instance.get_t_role_dress (m_id);
		name = t_role_dress.name;
		if(m_atlas.GetListOfSprites().Contains(t_role_dress.icon))
		{
			this.transform.Find("bg").GetComponent<UISprite>().atlas = m_atlas;
		}
		else
		{
			this.transform.Find("bg").GetComponent<UISprite>().atlas = m_atlas1;
		}
		/*if(t_role_dress.value1 != 0)
		{
			desc = game_data._instance.get_value_string (t_role_dress.attr1, t_role_dress.value1) + "\n";
		}
		if(t_role_dress.value2 != 0)
		{
			desc += game_data._instance.get_value_string (t_role_dress.attr2, t_role_dress.value2) + "\n";
		}*/
		//desc += t_role_dress.des;
		transform.Find("bg").GetComponent<UISprite>().spriteName = t_role_dress.icon;
	}

	public void click()
	{
		if(m_out_message.Length == 0)
		{
			s_message _message = new s_message ();
			
			_message.m_type = "item_dialog_box";
			_message.m_ints.Add (m_id);
			_message.m_ints.Add (5);
			cmessage_center._instance.add_message (_message);
			return;
		}
		
		s_message m_message = new s_message ();
		
		m_message.m_type = m_out_message;
		m_message.m_ints.Add (m_id);
		
		cmessage_center._instance.add_message (m_message);
	}

	// Update is called once per frame
	void Update () 
	{
	
	}
}
