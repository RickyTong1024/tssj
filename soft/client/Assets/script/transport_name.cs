
using UnityEngine;
using System.Collections;

public class transport_name : MonoBehaviour {

	public ulong m_guid;
	// Use this for initialization
	void Start () {
	
	}

	public void click()
	{
		s_message _msg = new s_message ();

		_msg.m_type = "transport_ship";
		_msg.m_long.Add (m_guid);

		cmessage_center._instance.add_message (_msg);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
