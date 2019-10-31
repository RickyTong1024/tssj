
using UnityEngine;
using System.Collections;

public class effect_cast : MonoBehaviour {

	public GameObject m_target_pos;
	public GameObject m_target_unit;
	public float m_speed = 60.0f;
	public string m_effect = "";
	public string m_action = "";
	public float m_remove_time;
	public float m_shake;
	public float m_remove_time2;
	// Use this for initialization
	void Start () {
	
	}
	void OnDestroy()
	{

	}
	// Update is called once per frame
	void Update () {

		if (m_target_pos == null)
		{
			Object.Destroy(transform.gameObject);
			return;	
		}
		
		Vector3 _add = m_target_pos.transform.position - transform.position;
		float _dis = _add.magnitude;

		_add.Normalize ();
		_add *= m_speed * Time.deltaTime;

		if(_dis > _add.magnitude)
		{
			transform.position = transform.position + _add;

			transform.LookAt (m_target_pos.transform.position);
		}
		else
		{

			if(m_effect.Length > 0)
			{
				GameObject _ins = game_data._instance.ins_object_res(m_effect);
				if(_ins != null)
				{					
					_ins.transform.localPosition = new Vector3(0,0,0);
					_ins.transform.localEulerAngles = new Vector3(0,0,0);
					_ins.transform.position = transform.position;
					Object.Destroy(_ins,m_remove_time2);
				}

				m_effect = "";
			}
			
			if(m_effect.Length > 0 && m_target_unit != null)
			{
				//m_target_unit.GetComponent<unit>().action(m_action);
			}
			
			if(m_shake > 0)
			{
				sys._instance.shake_cam(m_shake);
			}

			Object.Destroy(transform.gameObject,m_remove_time);
		}

	}
}
