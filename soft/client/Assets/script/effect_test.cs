
using UnityEngine;
using System.Collections;

public class effect_test : MonoBehaviour, IMessage {

	public float m_speed = 1.0f;
	public Camera m_cam;
	private float m_shake = 0;
	private Vector3 m_cur_cam_pos;
	// Use this for initialization
	void Start () {
	
		cmessage_center._instance.add_handle (this);
		m_cur_cam_pos = m_cam.transform.position;
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.message (s_message message)
	{
		if(message.m_type == "shake_cam")
		{
			message.m_remove = true;
			
			m_shake = (float)message.m_floats[0];
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{

	}
	// Update is called once per frame
	void Update () {

		if (m_shake > 0)
		{
			float _addx = Random.Range(- m_shake,m_shake);
			float _addy = Random.Range(- m_shake,m_shake);
			float _addz = Random.Range(- m_shake,m_shake);
			
			m_cam.transform.position = new Vector3(m_cur_cam_pos.x + _addx,m_cur_cam_pos.y + _addy,m_cur_cam_pos.z + _addz);
			
			m_shake -= Time.deltaTime * 3.0f;
		}

		if(sys._instance.m_game_speed < 1.0f)
		{
			sys._instance.m_game_speed += Time.deltaTime / sys._instance.m_game_speed;
			
			if(sys._instance.m_game_speed > 1.0f)
			{
				sys._instance.m_game_speed = 1.0f;
			}

			Time.timeScale = m_speed * sys._instance.m_game_speed;
		}
		else
		{
			Time.timeScale = m_speed;
		}
	}
}
