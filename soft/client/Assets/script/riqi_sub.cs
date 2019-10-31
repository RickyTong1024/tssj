
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class riqi_sub : MonoBehaviour {

	public int m_id;
	public int m_is_chose;
	public int m_month = 0;
	public int is_land = 0;
	public int m_can_get = 0;
	public int m_day = 0;
	public bool is_end = false;
	public GameObject m_get;
	public GameObject m_ylq;
	public GameObject m_desc;
	public GameObject m_view;
	public GameObject m_scro;
	public GameObject m_wdc;
	public List<s_t_reward> rewards = new List<s_t_reward>();
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		m_desc.GetComponent<UILabel>().text =  m_month.ToString () + "." + m_day.ToString() + game_data._instance.get_t_language ("riqi_sub.cs_28_89");//登录可领取
		for (int i = 0; i < rewards.Count; ++i)
		{
			GameObject _obj = icon_manager._instance.create_reward_icon(rewards[i].type ,rewards[i].value1 ,rewards[i].value2 ,rewards[i].value3);
			if(m_is_chose == 1)
			{
				_obj.AddComponent<UIDragScrollView>().scrollView = m_view.GetComponent<UIScrollView>();
			}
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(-252 + i * 99,-17,0);
			_obj.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>(); 
		}
		m_wdc.SetActive (false);
		if(m_can_get == 1)
		{
			m_get.SetActive(false);
			m_ylq.SetActive(true);
		}
		else
		{
			if(is_land == 1 && m_can_get == 0)
			{
				m_get.SetActive(true);
				m_get.GetComponent<BoxCollider>().enabled = true;
				m_get.GetComponent<UISprite>().set_enable(true);
				m_ylq.SetActive(false);
			}
			else
			{
				m_get.SetActive(true);
				m_get.GetComponent<BoxCollider>().enabled = false;
				m_get.GetComponent<UISprite>().set_enable(false);
				m_ylq.SetActive(false);
			}
			if(is_end)
			{
				m_get.SetActive(false);
				m_ylq.SetActive(false);
				m_wdc.SetActive(true);
			}
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			if(m_is_chose == 1)
			{
				root_gui._instance.show_select_item_gui(m_id,rewards,"huodong_riqi_lj",1);
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "huodong_riqi_lj";
				_message.m_ints.Add (0);
				_message.m_ints.Add (m_id);
				cmessage_center._instance.add_message(_message);
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
