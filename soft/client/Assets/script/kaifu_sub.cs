
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class kaifu_sub : MonoBehaviour {
	// Use this for initialization
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

	void Start () {
	}

	public void reset()
	{
		m_ylq.SetActive(false);
		m_get.SetActive(true);
		m_get.GetComponent<UISprite>().set_enable (false);
		m_get.GetComponent<BoxCollider>().enabled = false;
		m_recharge.SetActive(false);
		m_main.SetActive(true);
		m_main1.SetActive(false);
		s_t_kaifu_mubiao t_kaifu_mubiao = game_data._instance.get_t_kaifu_mubiao (m_id);
        if (t_kaifu_mubiao.type == 2 || t_kaifu_mubiao.type == 55)
        {
            m_name.GetComponent<UILabel>().text = string.Format(t_kaifu_mubiao.desc, sys._instance.get_czbl(t_kaifu_mubiao.ck)) + "(" + sys._instance.get_czbl(m_num).ToString() + "/" + sys._instance.get_czbl(t_kaifu_mubiao.ck).ToString() + ")";
        }
        else
        {
            m_name.GetComponent<UILabel>().text = t_kaifu_mubiao.desc + "(" + m_num.ToString() + "/" + t_kaifu_mubiao.ck.ToString() + ")";
        }

		for (int i = 0; i < t_kaifu_mubiao.rewards.Count; ++i)
		{
			GameObject _obj = icon_manager._instance.create_reward_icon(t_kaifu_mubiao.rewards[i].type
			                                                            ,t_kaifu_mubiao.rewards[i].value1
			                                                            ,t_kaifu_mubiao.rewards[i].value2
			                                                            ,t_kaifu_mubiao.rewards[i].value3);
			
			_obj.transform.parent = m_icons[i].transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,0,0);
		}
		for (int i = t_kaifu_mubiao.rewards.Count; i < 4; ++i)
		{
			m_icons[i].SetActive(false);
		}

		bool has = false;
		for (int i = 0; i < sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids.Count; ++i)
		{
			if (m_id == sys._instance.m_self.m_t_player.huodong_kaifu_finish_ids[i])
			{
				has = true;
				break;
			}
		}

		if (has)
		{
			m_ylq.SetActive(true);
			m_main.SetActive(true);
			m_main1.SetActive(false);
			m_get.SetActive(false);
		}
		else if (m_num >= t_kaifu_mubiao.ck)
		{
			m_get.GetComponent<UISprite>().set_enable (true);
			m_get.GetComponent<BoxCollider>().enabled = true;
			m_main1.SetActive(true);
			m_main.SetActive(false);
		}
		else if (t_kaifu_mubiao.type == 2)
		{
			m_recharge.SetActive(true);
			m_main.SetActive(true);
			m_main1.SetActive(false);
			m_get.SetActive(false);
		}
	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			s_t_kaifu_mubiao t_kaifu_mubiao = game_data._instance.get_t_kaifu_mubiao (m_id);
			List<s_t_reward> rewards = new List<s_t_reward>();
			if(t_kaifu_mubiao.def3 == 1)
			{
				root_gui._instance.show_select_item_gui(m_id,t_kaifu_mubiao.rewards,"huodong_kaifu_lj",1);
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "kaifu_lj";
				_message.m_ints.Add (m_id);
				cmessage_center._instance.add_message(_message);
			}
		}
		else if (obj.name == "recharge")
		{
			s_message _message = new s_message();
			_message.m_type = "kaifu_recharge";
			_message.m_ints.Add (m_id);
			cmessage_center._instance.add_message(_message);
		}
	}

	// Update is called once per frame
	void Update () {

	}

}
