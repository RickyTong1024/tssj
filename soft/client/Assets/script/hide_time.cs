
using UnityEngine;
using System.Collections;

public class hide_time : MonoBehaviour {

	public float m_time = 0;

	void Update () {
	
		if(m_time > 0.0f)
		{
			m_time -= Time.deltaTime;

			if(m_time < 0.0f)
			{
				this.transform.gameObject.SetActive(false);
			}
		}
	}
}
