
using UnityEngine;
using System.Collections;

public class map_jiesuo : MonoBehaviour {

	public GameObject m_jiesuo;
	public GameObject m_map;
	float m_time;
	// Use this for initialization
	void Start () {
	

	}

	void OnEnable()
	{
		this.GetComponent<UIPanel>().alpha = 1.0f;
		m_jiesuo.GetComponent<UISprite>().spriteName = "lock_mo01";
		m_jiesuo.GetComponent<UISpriteAnimation>().ResetToBeginning ();
		m_time = 2.0f;
		sys._instance.add_alpha_anim (this.gameObject, 1.0f, 1.0f, 0.0f, 1.2f);
	}
	
	// Update is called once per frame
	void Update () {
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_time <= 0)
			{
				this.gameObject.SetActive(false);
				m_map.GetComponent<map_gui>().map_jiesuo_end();
			}
		}
	}
}
