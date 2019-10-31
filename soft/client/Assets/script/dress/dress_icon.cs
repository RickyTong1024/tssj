using UnityEngine;

public class dress_icon : MonoBehaviour {

	public string m_out_message;
	public  int m_id;
	public s_t_dress m_t_dress;
	// Use this for initialization
	void Start () {

	}

	public void init()
	{
		m_out_message = "";

		this.GetComponent<UISprite>().alpha = 1.0f;
		this.transform.GetComponent<BoxCollider>().enabled = true;
		this.transform.localEulerAngles = new Vector3(0,0,0);
	}

	public void reset()
	{
		m_t_dress = game_data._instance.get_t_dress (m_id);

		if(m_t_dress == null)
		{
			return ;
		}

		this.GetComponent<UISprite>().spriteName = m_t_dress.icon;
	}

	public void click()
	{
		if(m_out_message.Length == 0)
		{
			s_message _message = new s_message ();
			
			_message.m_type = "item_dialog_box";
			_message.m_ints.Add (m_t_dress.id);
			_message.m_ints.Add (3);
			cmessage_center._instance.add_message (_message);
			return;
		}
		
		s_message m_message = new s_message ();
		
		m_message.m_type = m_out_message;
		m_message.m_ints.Add (m_t_dress.id);
		
		cmessage_center._instance.add_message (m_message);
	}
}
