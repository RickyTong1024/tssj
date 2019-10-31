
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class transport_enemy_item : MonoBehaviour {

	public ulong m_guid;
	public GameObject m_name;

	void Start () {
	}

	void click(GameObject obj)
	{
		if(obj.name == "enemy_fight")
		{
			s_message _message = new s_message();
			_message.m_type = "transport_enemy_item";
			_message.m_long.Add(m_guid);
			cmessage_center._instance.add_message(_message);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
