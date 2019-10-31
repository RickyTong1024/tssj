
using UnityEngine;
using System.Collections;

public class hide_Label : MonoBehaviour {

	public GameObject m_hide_root;
	public float m_time = 0;
	private bool m_hide = false;

	// Use this for initialization
	void Start () {
	
	}

	void hide()
	{
		if(m_hide_root != null)
		{
			m_hide_root.SetActive(false);
		}
		m_hide = false;
	}

	public void OnEnable()
	{
		if(m_hide_root == null)
		{
			m_hide_root = this.gameObject;
		}
		m_hide = true;
	}
	// Update is called once per frame
	void Update () {
		if(m_hide)
		{
			if(m_time > 0.0f)
			{
				m_time -= Time.deltaTime;
				
				if(m_time < 0.0f)
				{
					TweenScale _scale = sys._instance.add_scale_anim(m_hide_root.gameObject,0.2f,1.2f,0.0f,0);
					EventDelegate.Add(_scale.onFinished, delegate() 
					                  {
						hide();
					},true);
				}
			}
		}
	
	}
}
