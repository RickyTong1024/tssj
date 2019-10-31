
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ui_3dto2d : MonoBehaviour {

	public GameObject obj;
	public Vector3 m_pos;
	// Use this for initialization
	void Start () {

		//m_pos = new Vector3 (0f, 100f, 0f);
	}
	// Update is called once per frame
	void Update () {

		if(obj == null)
		{
			GameObject.Destroy(this.gameObject);
			return;
		}
        transform.localPosition = sys._instance.WorldToScreenPoint(obj.transform.position + m_pos);
	}
}
