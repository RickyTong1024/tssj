
using UnityEngine;
using System.Collections;

public class guanghuan_icon : MonoBehaviour {

	public s_t_guanghuan m_guanghuan;
	public string m_out_message;
	public GameObject m_level;
	public int level = 0;
	// Use this for initialization
	void Start () {
	
	}

	public void init()
	{
		level = 0;
		this.GetComponent<UISprite>().alpha = 1.0f;
		this.transform.GetComponent<BoxCollider>().enabled = true;
		this.transform.localEulerAngles = new Vector3(0,0,0);
		UIDragScrollView uisv = this.GetComponent<UIDragScrollView>();
		if (uisv != null)
		{
			Object.Destroy(uisv);
		}
	}

	public void reset()
	{
		this.transform.GetComponent<UISprite>().spriteName = m_guanghuan.icon;
		string s = "";
		if (m_guanghuan.color == 1)
		{
			s = "xtbk_lvpt001";
		}
		else if (m_guanghuan.color == 2)
		{
			s = "xtbk_lanpt001";
		}
		else if (m_guanghuan.color == 3)
		{
			s = "xtbk_zipt001";
		}
		else if (m_guanghuan.color == 4)
		{
			s = "xtbk_chpt001";
		}
		else if (m_guanghuan.color == 5)
		{
			s = "xtbk_hopt001";
		}
		else if (m_guanghuan.color == 6)
		{
			s = "xtbk_jinpt001";
		}
		this.transform.Find("bg").GetComponent<UISprite>().spriteName = s;
		if(level == 0)
		{
			m_level.SetActive(false);
		}
		else
		{
			m_level.SetActive(true);
			m_level.GetComponent<UILabel>().text = "+" + level;
		}
	}

	public void click()
	{
		if(m_out_message.Length == 0)
		{
			s_message _message = new s_message ();
			
			_message.m_type = "guanghuan_dialog_box";
			_message.m_ints.Add (m_guanghuan.id);
			
			cmessage_center._instance.add_message (_message);
			return;
		}
		
	}
}

