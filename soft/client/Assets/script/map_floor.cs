
using UnityEngine;
using System.Collections;

public class map_floor : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		Vector3 _size = transform.GetComponent<BoxCollider>().size;

		_size.x = Screen.width;
		_size.y = Screen.height;

		transform.GetComponent<BoxCollider>().size = _size;
	}
}
