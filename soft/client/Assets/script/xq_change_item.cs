
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class xq_change_item : MonoBehaviour {

	public ulong guid;
	public int xq;
	public GameObject m_icon;
	public GameObject m_des;
	public GameObject m_up;
	public GameObject m_down;
	public GameObject m_no;
	public GameObject m_now;
	public GameObject m_change;

	// Use this for initialization
	void Start () {
	
	}

	public void updata_ui()
	{
		m_up.SetActive (false);
		m_down.SetActive (false);
		m_no.SetActive (false);
		ccard card = sys._instance.m_self.get_card_guid(guid);
		m_now.GetComponent<UISprite>().spriteName = xq_icon (card.get_role().xq);
		m_change.GetComponent<UISprite>().spriteName = xq_icon (xq);
		if(xq > card.get_role().xq)
		{
			m_up.SetActive(true);
            m_des.GetComponent<UILabel>().text = "[00ff00]" + game_data._instance.get_t_language ("xq_change_item.cs_33_53"); //她的心情变得更好了,战斗力更高了呢
		}
		else
		{
			m_down.SetActive(true);
			m_des.GetComponent<UILabel>().text = "[ff0000]" + game_data._instance.get_t_language ("xq_change_item.cs_38_53");//她的心情变差了，赶紧去邀请她约会吧
		}
		card.get_role ().xq = xq;
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_card_icon_ex(guid);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		_icon.transform.GetComponent<BoxCollider>().enabled = false;

	}

	public static string xq_icon(int xq)
	{
		string s = "";
		if(xq == 1)
		{
			s = "hbxq_cha";
		}
		else if(xq == 2)
		{
			s = "hbxq_js";
		}
		else if(xq == 3)
		{
			s = "hbxq_pt";
		}
		else if(xq == 4)
		{
			s = "hbxq_kx";
		}
		else if(xq == 5)
		{
			s = "hbxq_hao";
		}
		else
		{
			s = "hbxq_pt";
		}
		return s;
	}

	public static string xq_Label(int xq)
	{
		string s = "";
		string s1 = "";
		s_t_xinqing t_xinqing = game_data._instance.get_t_xinqing(xq);
		if(xq <= 0)
		{
			t_xinqing = game_data._instance.get_t_xinqing(3);
		}
		if(t_xinqing.sx_per == 0)
		{
			s1 = game_data._instance.get_t_language ("xq_change_item.cs_92_8");//属性不变
		}
		else if(t_xinqing.sx_per > 0)
		{
			s1 = game_data._instance.get_t_language ("xq_change_item.cs_96_8") + "+" + t_xinqing.sx_per + "%";//最终伤害
		}
		else
		{
			s1 = game_data._instance.get_t_language ("xq_change_item.cs_96_8") + t_xinqing.sx_per + "%";//最终伤害
		}

		if(xq == 1)
		{
			s = "[ff0000]" + s1;
		}
		else if(xq == 2)
		{
			s = "[00ff00]" + s1;
		}
		else if(xq == 3)
		{
			s = "[00ffff]" + s1;
		}
		else if(xq == 4)
		{
			s = "[ff00ff]" + s1;
		}
		else if(xq == 5)
		{
			s = "[ff8a00]" + s1;
		}
		else
		{
			s = "[00ffff]" + s1;
		}
		return s;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
