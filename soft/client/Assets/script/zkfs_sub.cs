
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class zkfs_sub : MonoBehaviour {

	public int m_id;
	public int price = 0;
	public int flag = 0;
	public int discount = 0;
	public int m_count = 0;
	public int m_toltal_count = 0;
	public bool is_end = false;
	public GameObject m_get;
	public GameObject m_ylq;
	public GameObject m_desc;
	public GameObject m_num;
	public GameObject m_icon;
	public GameObject m_view;
	public GameObject m_scro;
	public GameObject m_wdc;
	public List<s_t_reward> rewards = new List<s_t_reward>();
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		for (int i = 0; i < rewards.Count; ++i)
		{
			GameObject _obj = icon_manager._instance.create_reward_icon(rewards[i].type ,rewards[i].value1 ,rewards[i].value2 ,rewards[i].value3);
			_obj.AddComponent<UIDragScrollView>().scrollView = m_view.GetComponent<UIScrollView>(); 
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(-106 + i * 99,0,0);
			_obj.AddComponent<UIDragScrollView>().scrollView = m_scro.GetComponent<UIScrollView>(); 
		}

		sys._instance.remove_child (m_icon);
		GameObject _obj1 = icon_manager._instance.create_reward_icon(1 ,2 ,price ,0);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		float num = (float)discount / 10;
		num = (float)decimal.Round ((decimal)num, 2);
		if(discount/10 >= 10 || discount <= 0)
		{
			m_desc.transform.parent.gameObject.SetActive(false);
		}
		else
		{
			m_desc.transform.parent.gameObject.SetActive(true);

            m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"), sys._instance.discountChange(num));//{0}折
			
		}
		m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jieri_duihuan_sub.cs_60_40") + (m_toltal_count - m_count) + "/" + m_toltal_count;//剩余次数 
		m_wdc.SetActive (false);
		if(m_count < m_toltal_count)
		{
			m_get.SetActive(true);
			m_ylq.SetActive(false);
			if(sys._instance.m_self.m_t_player.jewel < price)
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
			if(flag == 1)
			{
				int total_num = (sys._instance.m_self.m_t_player.jewel/price);
				total_num = Mathf.Min(total_num,(m_toltal_count - m_count));
				root_gui._instance.show_select_item_gui(m_id,rewards,"select_zkfs",2,total_num);
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "select_zkfs";
				_message.m_ints.Add (0);
				_message.m_ints.Add (m_id);
				_message.m_ints.Add (m_toltal_count - m_count);
				cmessage_center._instance.add_message(_message);
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
