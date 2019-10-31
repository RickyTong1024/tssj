
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_qianghua_effect : MonoBehaviour {

	// Use this for initialization
	public GameObject m_eff1;
	public GameObject m_eff2;
	public GameObject m_eff3;
	public GameObject m_eff4;
	public GameObject m_eff5;
	public GameObject m_eff6;

	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnEnable() {
		m_eff1.GetComponent<UISpriteAnimation>().ResetToBeginning ();
		m_eff2.GetComponent<UISpriteAnimation>().ResetToBeginning ();
		m_eff3.GetComponent<UISpriteAnimation>().ResetToBeginning ();
		m_eff4.GetComponent<UISpriteAnimation>().ResetToBeginning ();
		m_eff5.GetComponent<UISpriteAnimation>().ResetToBeginning ();
		m_eff6.GetComponent<UISpriteAnimation>().ResetToBeginning ();
	}

	public void effect_end()
	{
		s_message mes = new s_message ();
		mes.m_type = "treasure_qianghua_effect_end";
		cmessage_center._instance.add_message (mes);
	}
}
