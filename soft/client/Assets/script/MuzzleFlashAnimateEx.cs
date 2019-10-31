
using UnityEngine;
using System.Collections;

public class MuzzleFlashAnimateEx : MonoBehaviour {

	public GameObject[] m_bullet;
	public GameObject m_light;
	// Use this for initialization
	void Start () {
	
		InvokeRepeating ("flash", 0.0f, 0.05f);

		InvokeRepeating ("bullet", 0.0f,0.1f);

	}
	public void bullet()
	{
		for(int i = 0;i < m_bullet.Length;i ++)
		{
			m_bullet[i].SetActive(false);
		}
		
		GameObject _bullet = m_bullet[Random.Range(0,m_bullet.Length)];
		
		_bullet.SetActive (true);
	}
	public void flash()
	{
		if(m_light == null)
		{
			return;
		}

		if(this.gameObject.activeSelf == false)
		{
			m_light.SetActive(false);

			return;
		}

		if(m_light.activeSelf)
		{
			this.GetComponent<Light>().range = 0;
			m_light.SetActive(false);
		}
		else
		{
			this.GetComponent<Light>().range = 10;
			m_light.SetActive(true);
		}

	}
	// Update is called once per frame
	void Update () {

		/*
		transform.localScale = Vector3.one * Random.Range(0.5f,1.5f);

		Vector3 _angle = transform.localEulerAngles;

		_angle.z = Random.Range(0,90.0f);

		transform.localEulerAngles = _angle;
		*/

	}
}
