
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class chongzhifali_sub : MonoBehaviour {

	public int m_id;
	public GameObject m_get;
	public GameObject m_recharge;
	public GameObject m_ylq;
	public GameObject m_name;
	public List<GameObject> m_icons;
	public int m_need_jewel = 0;
	public int m_have_jewel = 0;
	public int m_can_get = 0;
	public List<int> rtype = new List<int>();
	public List<int> rvalue1 = new List<int>();
	public List<int> rvalue2 = new List<int>();
	public List<int> rvalue3 = new List<int>();


	// Use this for initialization
	void Start ()
	{
		
	}

    
	public void reset()
	{
		m_name.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("chongzhifali_sub.cs_29_55"), sys._instance.get_czbl(m_need_jewel).ToString () + "")//累计充值{0}元可以领取以下奖励
							+ "(" + sys._instance.get_czbl(m_have_jewel) + "/" + sys._instance.get_czbl(m_need_jewel).ToString() + ")";
		for (int i = 0; i < rtype.Count; ++i)
		{
			GameObject _obj = icon_manager._instance.create_reward_icon(rtype[i] ,rvalue1[i] ,rvalue2[i] ,rvalue3[i]);
			
			_obj.transform.parent = m_icons[i].transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,0,0);
		}
		for (int i = rtype.Count; i < 4; ++i)
		{
			m_icons[i].SetActive(false);
		}

		if (m_can_get == 1)
		{
			m_ylq.SetActive(true);
			m_recharge.SetActive(false);
			m_get.SetActive(false);
		}
		else if(m_have_jewel < m_need_jewel)
		{
			m_ylq.SetActive(false);
			m_recharge.SetActive(true);
			m_get.SetActive(false);
		}
		else
		{
			m_ylq.SetActive(false);
			m_get.SetActive(true);
			m_recharge.SetActive(false);
		}
	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			s_message _message = new s_message();
			_message.m_type = "huodong_czfl_lj";
			_message.m_ints.Add (m_id);
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.name == "recharge")
		{
			s_message _message = new s_message();
			_message.m_type = "huodong_czfl_recharge";
			_message.m_ints.Add (m_id);
			cmessage_center._instance.add_message(_message);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
