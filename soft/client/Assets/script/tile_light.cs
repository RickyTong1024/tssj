
using UnityEngine;
using System.Collections;

public class tile_light : MonoBehaviour {

	private float m_value = 0;
	// Use this for initialization
	void Start () {
		m_value = Random.Range (0.0f, 1000.0f);
	}
	
	// Update is called once per frame
	void Update () {
	
		m_value += Time.deltaTime * 0.8f;

		float _light = 1.3f + Mathf.Sin (m_value) * 0.3f;

		this.GetComponent<Renderer>().material.SetFloat ("_light", _light); 
	}
}
