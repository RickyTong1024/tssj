
using UnityEngine;
using System.Collections;

public class effect_base : MonoBehaviour {

	public float m_destroy = 0.0f;
	public string m_effect;
	public string m_skill;
	// Use this for initialization
	void Start () {
	
		if(m_destroy > 0.0f)
		{
			Object.Destroy(transform.gameObject,m_destroy);
		}
	}
	void OnDestroy()
	{
		if(m_effect.Length > 0)
		{
			GameObject _ins = game_data._instance.ins_object_res(m_effect);			
			_ins.transform.localPosition = new Vector3(0,0,0);
			_ins.transform.localEulerAngles = new Vector3(0,0,0);
			_ins.transform.position = transform.position;
		}
		
		if(m_skill.Length > 0)
		{
			
		}

	}	
	// Update is called once per frame
	void Update () {
	
	}
}
