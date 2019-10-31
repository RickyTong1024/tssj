
using UnityEngine;
using System.Collections;

public class map_end : MonoBehaviour {

	public GameObject m_text;
	public UILabel m_xrxz_Label;
	// Use this for initialization
	void Start () {


	}

	void OnEnable()
	{
		m_text.GetComponent<UISprite>().spriteName = "tg_w03";
		m_text.GetComponent<UISpriteAnimation>().enabled = false;
		this.GetComponent<UIButtonMessage>().functionName = "";
	}

	public void action_end()
	{
		m_text.GetComponent<UISpriteAnimation>().ResetToBeginning ();
		m_text.GetComponent<UISpriteAnimation>().enabled = true;
	}

	public void action_end1()
	{
		this.GetComponent<UIButtonMessage>().functionName = "click";
	}
}
