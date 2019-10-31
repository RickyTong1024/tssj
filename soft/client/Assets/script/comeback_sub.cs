
using UnityEngine;
using System.Collections.Generic;

public class comeback_sub : MonoBehaviour {

	public int m_id;
	public int m_num;
	public GameObject m_get;
	public GameObject m_recharge;
	public GameObject m_ylq;
	public GameObject m_name;
	public GameObject m_main;
	public GameObject m_main1;
	public UILabel m_lq;
	public UILabel m_cz;
	public List<GameObject> m_icons;
	s_t_comeback t_comeback;

	public void reset(int type)
	{
		m_ylq.SetActive(false);
		m_get.SetActive(true);
		m_get.GetComponent<UISprite>().set_enable (false);
		m_get.GetComponent<BoxCollider>().enabled = false;
		m_recharge.SetActive(false);
		m_main.SetActive(true);
		m_main1.SetActive(false);
		t_comeback = game_data._instance.get_t_comeback (m_id);
		m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language(t_comeback.design);

		if (t_comeback.type == 3)
        {
			m_name.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language(t_comeback.design), sys._instance.get_czbl(t_comeback.def1)) + "(" + sys._instance.get_czbl(m_num).ToString() + "/" + sys._instance.get_czbl(t_comeback.def1).ToString() + ")";
        }
        else
        {
			m_name.GetComponent<UILabel>().text = game_data._instance.get_t_language (t_comeback.design);
		}


		for (int i = 0; i < t_comeback.rewards.Count; ++i)
		{

			GameObject _obj = icon_manager._instance.create_reward_icon(t_comeback.rewards[i].type, t_comeback.rewards[i].value1, t_comeback.rewards[i].value2, t_comeback.rewards[i].value3);
			_obj.transform.parent = m_icons[i].transform;
			_obj.transform.localScale = new Vector3(1, 1, 1);
			_obj.transform.localPosition = new Vector3(0, 0, 0);
		}
		for (int i = t_comeback.rewards.Count; i < 4; ++i)
		{
			 m_icons[i].SetActive(false);
		}
		if (type == 0 && t_comeback.type == 1) //可以领取 未领取状态
        { 
			m_get.GetComponent<UISprite>().set_enable (true);
			m_get.GetComponent<BoxCollider>().enabled = true;
		}
        else if (type == 1 && t_comeback.type == 1)//不可领取
        {  
			m_get.GetComponent<UISprite>().set_enable (false);
			m_get.GetComponent<BoxCollider>().enabled = false;
		}
        else if (type == 2 && t_comeback.type == 1)//可领取 已经领取完毕
        { 
			m_ylq.SetActive (true);
			m_get.SetActive (false);	
		}
        else if (type == 0 && t_comeback.type == 3)//可以领取 未领取状态      type 1   回归补助奖励    3 回归豪礼
        {
			m_get.GetComponent<UISprite>().set_enable (true);
			m_get.GetComponent<BoxCollider>().enabled = true;
		}
        else if (type == 1 && t_comeback.type == 3)//不可领取
        {  
			m_get.SetActive (false);
			m_get.GetComponent<BoxCollider>().enabled = false;
			m_recharge.SetActive (true);
		}
        else if (type == 2 && t_comeback.type == 3)//可领取 已经领取完毕
        { 
			m_ylq.SetActive (true);
			m_get.SetActive (false);	
		}
	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "get_fuli")
		{
			s_message _message = new s_message();
			_message.m_type = "Huigui";
			_message.m_ints.Add(t_comeback.id);
			_message.m_ints.Add(t_comeback.type);
			_message.m_ints.Add(1);
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.name == "recharge")
		{
			s_message _message = new s_message ();
			_message.m_type = "show_recharge";
			cmessage_center._instance.add_message (_message);
		}
	}
}
