
using UnityEngine;
using System.Collections;

public class lookat : MonoBehaviour {

	public GameObject m_cam_pos;
	public GameObject m_lookat;
	public GameObject m_cam;
	public bool m_target = false;
	// Use this for initialization
	void Start () {
	
	}

	public void look()
	{
		if(m_cam_pos != null)
		{
			m_cam.transform.position = m_cam_pos.transform.position;
		}
		m_cam.transform.LookAt (m_lookat.transform.position);
	}
	// Update is called once per frame
	public void Update () {
	
		if(m_cam == null)
		{
			m_cam = this.transform.gameObject;
		}

		if(!m_target)
		{
			look();
		}
	}
	
}
