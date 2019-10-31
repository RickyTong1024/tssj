
using UnityEngine;
using System.Collections;

public class post : MonoBehaviour {

	public dhc.post_t m_post = null;
	public bool m_is_zd;
	public int m_zd_index;
	public GameObject m_delete;
	public GameObject m_icon;
	// Use this for initialization
	void Start () {
		init ();
	}


	public void init()
	{
		if(m_is_zd)
		{
			transform.Find("pt_yj").gameObject .SetActive(false);
			transform.Find("zd_yj").gameObject .SetActive(true);
			m_icon.GetComponent<UISprite>().spriteName = "xyj_bztb002";
		}
		else
		{
			m_icon.GetComponent<UISprite>().spriteName = "xyj_bztb001";
			if(m_post.type.Count == 0)
			{
				m_delete.SetActive(true);
			}
			else
			{
				m_delete.SetActive(false);
			}
			transform.Find("pt_yj").gameObject .SetActive(true);
			transform.Find("zd_yj").gameObject .SetActive(false);
		}
		if(m_is_zd)
		{
			string s = "[18c8be]" +game_data._instance.get_t_language ("post.cs_41_26");//斗人少尉
			transform.Find("name").GetComponent<UILabel>().text = "[18c8be]" + game_data._instance.m_postzd_list[m_zd_index].title;
			transform.Find("time").GetComponent<UILabel>().text = "";
			transform.Find("sender").GetComponent<UILabel>().text =  s;
		}
		else
		{
			transform.Find("name").GetComponent<UILabel>().text = "[1ca2ff]" + m_post.title;
			System.DateTime bdate = timer.time2dt (m_post.sender_date);
			transform.Find("time").GetComponent<UILabel>().text = "[1ca2ff]" + bdate.ToString("yyyy-MM-dd HH:mm:ss");
			transform.Find("sender").GetComponent<UILabel>().text =  "[1ca2ff]" + m_post.sender_name;
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "delete")
		{
			s_message _mes = new s_message();
			_mes.m_type = "delete_post";
			_mes.m_long.Add(m_post.guid);
			cmessage_center._instance.add_message(_mes);

		}
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
