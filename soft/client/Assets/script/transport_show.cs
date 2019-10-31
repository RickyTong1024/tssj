
using UnityEngine;
using System.Collections;

public class transport_show : MonoBehaviour {

	public GameObject m_cam;
	
	private Vector3 m_old_pos = new Vector3();

	private bool m_press = false;
	// Use this for initialization
	void Start () {
	
	}

	public bool get_mouse_button(int id)
	{
		if(Input.touchCount == 1)
		{
			return true;
		}
		else 
		{
			return Input.GetMouseButton (0);
		}
	}

	public Vector3 get_mouse_position()
	{
		Vector3 pos = new Vector3 ();
		if(Input.touchCount > 0)
		{
			pos = Input.GetTouch(0).position;
		}
		else
		{
			pos = Input.mousePosition;
		}

		return pos;
	}

	// Update is called once per frame
	void Update () {
	
		if(get_mouse_button(0))
		{
			Vector3 _pos = get_mouse_position ();

			if(m_press == false)
			{
				m_press = true;
				
				m_old_pos.x = _pos.x;
				m_old_pos.y = _pos.y;
				m_old_pos.z = _pos.z;	
			}

			Vector3 _cam_pos = m_cam.transform.localPosition;

			_cam_pos.x -= (_pos.x - m_old_pos.x) * 0.2f;

			if(_cam_pos.x > 100)
			{
				_cam_pos.x = 100;
			}

			if(_cam_pos.x < 0)
			{
				_cam_pos.x = 0;
			}

			m_cam.transform.localPosition = _cam_pos;

			m_old_pos.x = _pos.x;
			m_old_pos.y = _pos.y;
			m_old_pos.z = _pos.z;
		}
		else
		{
			m_press = false;
		}
	}
}
