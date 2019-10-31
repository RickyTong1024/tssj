
using UnityEngine;
using System.Collections;

public class gonggao : MonoBehaviour {
	
	public GameObject m_notice_text;
	public UILabel m_name;
	public UILabel m_close_notice_Label;
	// Use this for initialization
	void Start () {
		m_notice_text.GetComponent<UILabel>().text = platform_config_common.m_gonggao;
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.gameObject.SetActive(false);


		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
