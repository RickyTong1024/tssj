
using UnityEngine;
using System.Collections;

public class tanbao_floor : MonoBehaviour {

	private float m_alpha = 0f;
	public float m_target_alpha = 1.0f;
	public float m_down_time = 0f;
	public float m_up_time = 0f;
	
	// Use this for initialization
	void Start () {
		
		
	}
	
	public void OnEnable()
	{
		get_child().GetComponent<Renderer>().material.SetFloat ("_alpha", 1);
		m_target_alpha = 1;
		m_alpha = 1;
		//update_ui ();
	}
	
	public void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "unit")
		{
			m_down_time = 0.6f;
			m_alpha = 2.0f;
		}
	}
	
	public void OnTriggerExit(Collider other)
	{
		if(other.gameObject.tag == "unit")
		{
			m_up_time = 0.6f;
			m_alpha = 1.0f;
		}
	}
	private GameObject get_child()
	{
		return this.transform.GetChild (0).GetChild(0).GetChild(0).gameObject;
	}
	// Update is called once per frame
	void Update () {
		
		if(m_down_time > 0)
		{
			m_down_time -= Time.deltaTime;
			Vector3 pos = this.transform.position - new Vector3(0,0.06f,0);
			if(pos.y < -0.36f)
			{
				pos.y  = -0.36f;
			}
			TweenPosition position = TweenPosition.Begin(this.gameObject,0.1f,pos);
		}
		if(m_up_time > 0)
		{
			m_up_time -= Time.deltaTime;
			Vector3 pos = this.transform.position + new Vector3(0,0.06f,0);
			if(pos.y >= 0)
			{
				pos.y = 0;
			}
			TweenPosition position = TweenPosition.Begin(this.gameObject,0.1f,pos);
		}
		if(m_alpha < m_target_alpha)
		{
			m_target_alpha -= Time.deltaTime * 5;
			
			if(m_alpha > m_target_alpha)
			{
				m_target_alpha = m_alpha;
			}
			get_child().GetComponent<Renderer>().material.SetFloat ("_alpha", m_target_alpha);
		}
		
		if(m_alpha > m_target_alpha)
		{
			m_target_alpha += Time.deltaTime *5;
			if(m_alpha < m_target_alpha)
			{
				m_target_alpha = m_alpha;
			}
			get_child().GetComponent<Renderer>().material.SetFloat ("_alpha", m_target_alpha);
		}
	}
}
