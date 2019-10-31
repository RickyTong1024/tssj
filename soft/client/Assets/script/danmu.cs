
using UnityEngine;
using System.Collections;

public class danmu : MonoBehaviour {
	
	// Use this for initialization
	private float m_speed;
	private float m_x;
	private float m_y;
	public bool m_left;

	void Start () {

	}

	public void init()
	{
		m_speed = Random.Range (80, 300);
		m_x = Screen.width / 2;
		m_y = Random.Range(-100, 270);
		m_left = true;
	}

	// Update is called once per frame
	void FixedUpdate () {
		m_x -= m_speed * Time.fixedDeltaTime;
		this.transform.localPosition = new Vector3 (m_x, m_y, 0);
		if (m_x < -Screen.width / 2 - this.GetComponent<UILabel>().width)
		{
			m_left = false;
		}
	}
}
