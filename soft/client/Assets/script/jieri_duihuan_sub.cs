
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class jieri_duihuan_sub : MonoBehaviour {
	
	public int m_id;
	public int price = 0;
	public int discount = 0;
	public int m_count = 0;
	public int m_toltal_count = 0;
	public bool is_end = false;
	public GameObject m_get;
	public GameObject m_ylq;
	public GameObject m_num;
	public GameObject m_icon;
	public GameObject m_view;
	public GameObject m_scro;
	public GameObject m_wdc;
	public List<s_t_reward> reward1s = new List<s_t_reward>();
	public List<s_t_reward> rewards = new List<s_t_reward>();
	// Use this for initialization
	void Start () {
		
	}
	
	public void reset()
	{
		List<int> nums = new List<int>();
		int num = 0;
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		for (int i = 0; i < rewards.Count; ++i)
		{
            //if (rewards[i].type == 1)
            //{
            //  num = sys._instance.m_self.get_resources_num(rewards[i].value1);
            //}
            //else
            //{
			    num = sys._instance.m_self.get_item_num((uint)rewards[i].value1);
            //}
			GameObject _obj = icon_manager._instance.create_reward_icon(rewards[i].type ,rewards[i].value1 ,rewards[i].value2 ,rewards[i].value3,num);
			if(rewards.Count >= 4)
			{
				_obj.AddComponent<UIDragScrollView>().scrollView = m_view.GetComponent<UIScrollView>(); 
			}
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(-106 + i * 99,0,0);
			_obj.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>();
			num = num / rewards[i].value2;
			nums.Add(num);
		}
		
		sys._instance.remove_child (m_icon);
		GameObject _obj1 = icon_manager._instance.create_reward_icon(reward1s[0].type ,reward1s[0].value1 ,reward1s[0].value2 ,reward1s[0].value3);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3 (0, 0, 0);
		m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jieri_duihuan_sub.cs_60_40") + (m_toltal_count - m_count) + "/" + m_toltal_count;//剩余次数 
		bool flag = false;
		m_wdc.SetActive (false);
		for(int i = 0; i< nums.Count;++i)
		{
			if(nums[i] == 0)
			{
				flag = true;
				break;
			}
		}
		if(m_count < m_toltal_count)
		{
			m_get.SetActive(true);
			m_ylq.SetActive(false);
			if(flag)
			{
				m_get.GetComponent<BoxCollider>().enabled = false;
				m_get.GetComponent<UISprite>().set_enable(false);
			}
			else
			{
				m_get.GetComponent<BoxCollider>().enabled = true;
				m_get.GetComponent<UISprite>().set_enable(true);
			}

		}
		else
		{
			m_get.SetActive(true);
			m_ylq.SetActive(false);
			m_get.GetComponent<BoxCollider>().enabled = false;
			m_get.GetComponent<UISprite>().set_enable(false);
		}
		if(is_end)
		{
			m_get.SetActive(false);
			m_ylq.SetActive(false);
			m_wdc.SetActive(true);
		}
	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			s_message _message = new s_message();
			_message.m_type = "select_jieri_duihuan";
			_message.m_ints.Add (m_id);
			cmessage_center._instance.add_message(_message);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
