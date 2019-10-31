
using UnityEngine;
using System.Collections;

public class ts_tile : MonoBehaviour {

	private float m_alpha = 0f;
	public float m_target_alpha = 0f;
	public float m_time = 0f;
	public GameObject m_effect;
	// Use this for initialization
	void Start () {
	

	}

	public void reward(string pos)
	{
		m_effect = game_data._instance.ins_object_res("effect/ef_cj_wh01");
		m_effect.transform.name = pos;
		m_effect.transform.parent = get_child().transform;
		m_effect.transform.localPosition = new Vector3(0,0,0);
	}

	public void OnEnable()
	{
		get_child().GetComponent<Renderer>().material.SetFloat ("_alpha", 0);
		m_target_alpha = 1;

		Vector3 _rot = transform.localEulerAngles;

		_rot.y = (float)Random.Range (0, 4) * 90.0f;

		transform.localEulerAngles = _rot;
		//update_ui ();
	}

	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "unit")
		{
			get_child().GetComponent<Animator>().Play("floor");
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "unit")
		{
			get_child().GetComponent<Animator>().Play("floor_end");
		}
	}
	private GameObject get_child()
	{
		return this.transform.GetChild (0).gameObject;
	}
	// Update is called once per frame
	void Update () {
	
		if(m_alpha < m_target_alpha)
		{
			m_alpha += Time.deltaTime * 5;

			if(m_alpha > m_target_alpha)
			{
				m_alpha = m_target_alpha;
			}
			get_child().GetComponent<Renderer>().material.SetFloat ("_alpha", m_alpha);
		}

		if(m_time > 0)
		{
			m_time -= Time.deltaTime;

			return;
		}

		if(m_alpha > m_target_alpha)
		{
			Vector3 _pos = this.transform.localPosition;

			_pos.y  -= Time.deltaTime * 8;

			this.transform.localPosition = _pos;

			m_alpha -= Time.deltaTime;
			
			if(m_alpha <= 0.0f)
			{
				GameObject.Destroy(this.gameObject,1.0f);
			}
			get_child().GetComponent<Renderer>().material.SetFloat ("_alpha", m_alpha);
		}
	}
}
