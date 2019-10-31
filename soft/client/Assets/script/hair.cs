
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hair : MonoBehaviour {

	// Use this for initialization
	private List<GameObject> m_joint = new List<GameObject>();
	public GameObject m_hair_root;
	public float m_limit = 2.0f;
	public float m_mass = 0.1f;
	public float m_spring = 500;
	public float m_damper = 0;
	public float m_depth = 2.5f;
	public bool m_drift = false;

	void Start () {
	
		//m_hair_root = new GameObject ();

		//m_hair_root.transform.name = "hair";
		//m_hair_root.transform.position = new Vector3(0,0,0);

		//transform.parent = null;
		List<GameObject> _list = new List<GameObject> ();

		for(int i = 0;i < transform.childCount;i ++)
		{
			add_joint(transform.GetChild(i).gameObject,transform.gameObject);
			//m_joint.Add(transform.GetChild(i).gameObject);
		}

		for(int i = 0;i < m_joint.Count;i ++ )
		{
			//m_joint[i].transform.parent = m_hair_root.transform;
		}

	}

	float get_joint_depth(GameObject obj)
	{
		float _num = 1.5f;

		GameObject _obj = obj;

		while(_obj.transform.parent.GetComponent<ConfigurableJoint>() != null)
		{
			_obj = _obj.transform.parent.gameObject;
			_num += 1;
		}

		return _num;
	}
	void OnDestroy()
	{
		//GameObject.Destroy(m_hair_root);
	}
	void add_joint(GameObject obj,GameObject root)
	{
		if (obj.GetComponent<Renderer> () != null) {
			obj.GetComponent<Renderer> ().enabled = false;
		}

		for(int i = 0;i < obj.transform.childCount;i ++)
		{
			add_joint(obj.transform.GetChild(i).gameObject,obj);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
