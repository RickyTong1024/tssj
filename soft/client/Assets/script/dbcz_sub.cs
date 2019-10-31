
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class dbcz_sub : MonoBehaviour {

	public int m_id;
	public int m_need_jewel = 0;
	public int m_cz_num = 0;
	public int m_count = 0;
	public int m_toltal_count = 0;
	public bool is_end = false;
	public GameObject m_get;
	public GameObject m_ylq;
	public GameObject m_recharge;
	public GameObject m_desc;
	public GameObject m_num;
	public GameObject m_wdc;
	public List<GameObject> m_icons;
	public List<int> rtype = new List<int>();
	public List<int> rvalue1 = new List<int>();
	public List<int> rvalue2 = new List<int>();
	public List<int> rvalue3 = new List<int>();

	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
        
        m_desc.GetComponent<UILabel>().text =  string.Format(game_data._instance.get_t_language ("dbcz_sub.cs_32_56"), sys._instance.get_czbl(m_need_jewel).ToString () );//单笔充值{0}元可以领取以下奖励
		m_num.GetComponent<UILabel>().text = (m_toltal_count - m_count).ToString() + "/" + m_toltal_count.ToString ();
		for (int i = 0; i < rtype.Count; ++i)
		{
			m_icons[i].SetActive(true);
			GameObject _obj = icon_manager._instance.create_reward_icon(rtype[i] ,rvalue1[i] ,rvalue2[i] ,rvalue3[i]);
			
			_obj.transform.parent = m_icons[i].transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,0,0);
		}
		for (int i = rtype.Count; i < 4; ++i)
		{
			m_icons[i].SetActive(false);
		}
		m_wdc.SetActive (false);
		if(m_cz_num <= m_count )
		{
			if(m_count < m_toltal_count)
			{
				m_get.SetActive(false);
				m_ylq.SetActive(false);
				m_recharge.SetActive(true);
				if(is_end)
				{
					m_get.SetActive(false);
					m_ylq.SetActive(false);
					m_recharge.SetActive(false);
					m_wdc.SetActive(true);
				}
			}
			else
			{
				m_get.SetActive(false);
				m_ylq.SetActive(true);
				m_recharge.SetActive(false);
			}
		}
		else
		{
			if(m_count < m_toltal_count)
			{
				m_get.SetActive(true);
				m_ylq.SetActive(false);
				m_recharge.SetActive(false);
			}
			else
			{
				m_get.SetActive(false);
				m_ylq.SetActive(true);
				m_recharge.SetActive(false);
			}
			if(is_end)
			{
				m_get.SetActive(false);
				m_ylq.SetActive(false);
				m_recharge.SetActive(false);
				m_wdc.SetActive(true);
			}
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			s_message _message = new s_message();
			_message.m_type = "huodong_dbcz_lj";
			_message.m_ints.Add (m_id);
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.name == "recharge")
		{
			s_message _message = new s_message();
			_message.m_type = "huodong_dbcz_recharge";
			cmessage_center._instance.add_message(_message);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
