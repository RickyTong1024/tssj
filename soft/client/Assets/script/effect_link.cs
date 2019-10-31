
using UnityEngine;
using System.Collections;

public class effect_link : MonoBehaviour {

	public float m_length = 1.0f;
	public GameObject m_target;
	public GameObject m_src;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (m_target == null || m_src == null)
						return;

		transform.position = m_src.transform.position;
		transform.LookAt (m_target.transform.position);

		Vector3 _v3 = m_target.transform.position - transform.position;
		float _lenght = _v3.magnitude / m_length;
		transform.localScale = new Vector3 (transform.localScale.x,transform.localScale.y,_lenght);
	}
}
