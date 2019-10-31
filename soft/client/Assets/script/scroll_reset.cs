
using UnityEngine;
using System.Collections;

public class scroll_reset : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	public void OnEnable()
	{
		this.GetComponent<UIScrollView>().ResetPosition ();
	}
	// Update is called once per frame
	void Update () {
	
	}
}
