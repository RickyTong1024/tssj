
using UnityEngine;
using System.Collections;

public class gui_remove : MonoBehaviour {

	public bool m_remove = true;
	// Use this for initialization
	void Start () {
	
	}
	public void OnDisable()
	{
		if(!m_remove)
		{
			return;
		}
		Object.Destroy (this.gameObject);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
