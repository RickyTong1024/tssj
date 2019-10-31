
using UnityEngine;
using System.Collections;

public class czjh_sub : MonoBehaviour {

	public int m_id;
	public GameObject m_name;
	public GameObject m_reward;
	public GameObject m_level;
	public GameObject m_get;
	public GameObject m_ylq;

	// Use this for initialization
	void Start () {
	}

	public void reset()
	{
		s_t_huodong_czjh t_huodong_czjh = game_data._instance.get_t_huodong_czjh (m_id);
		m_name.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("czjh_sub.cs_20_55"),t_huodong_czjh.level.ToString());//{0}级成长基金
		m_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("czjh_sub.cs_21_55"),t_huodong_czjh.level.ToString());//{0}级
		m_reward.GetComponent<UILabel>().text = t_huodong_czjh.jewel.ToString ();
		if(is_has())
		{
			if(sys._instance.m_self.m_t_player.level >= t_huodong_czjh.level && sys._instance.m_self.m_t_player.huodong_czjh_index == m_id -1)
			{
				m_get.SetActive(true);
				m_get.GetComponent<UISprite>().set_enable(true);
				m_get.GetComponent<BoxCollider>().enabled = true;
			}
			else
			{
				m_get.SetActive(true);
				m_get.GetComponent<UISprite>().set_enable(false);
				m_get.GetComponent<BoxCollider>().enabled = false;
			}
		}
		else
		{
			m_get.SetActive(true);
			m_get.GetComponent<UISprite>().set_enable(false);
			m_get.GetComponent<BoxCollider>().enabled = false;
		}
		if(m_id <= sys._instance.m_self.m_t_player.huodong_czjh_index)
		{
			m_get.SetActive(false);
			m_ylq.SetActive(true);
		}
	
	}

	public static bool is_has()
	{
		if(sys._instance.m_self.get_vip() >= 2 && sys._instance.m_self.m_t_player.huodong_czjh_buy ==1)
		{
			return true;
		}
		return false;
	}

	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			s_message _message = new s_message();
			_message.m_type = "huodong_czjh_lj";
			_message.m_ints.Add (m_id);
			cmessage_center._instance.add_message(_message);
		}
	}


	// Update is called once per frame
	void Update () {
	
	}
}
