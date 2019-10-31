
using UnityEngine;
using System.Collections;

public class post_text : MonoBehaviour {

	public dhc.post_t m_post;
	public bool m_is_zd;
	public int m_zd_index;
	public GameObject m_text;
	public GameObject m_get;
	public UILabel m_kefu;
	public Transform m_reward;
	public UILabel m_name_Label;
	// Use this for initialization
	void Start () 
	{
	}

	public void reset()
	{

		m_text.transform.parent.localPosition = Vector3.zero;
		m_text.transform.parent.GetComponent<UIPanel>().clipOffset = Vector3.zero;
		m_reward.Find("Label").gameObject.SetActive (false);

		if (!m_is_zd)
		{

			m_get.GetComponent<UILabel>().text =  game_data._instance.get_t_language ("jt_mobai.cs_97_91");//领取
			transform.Find("back").Find("title").GetComponent<UILabel>().text = m_post.title;
			m_text.GetComponent<UILabel>().text = m_post.text.Replace("{nn}","\n");
			transform.Find("back").Find("sender").GetComponent<UILabel>().text = m_post.sender_name;

			Transform picon1 = m_reward.Find("icon1");
			sys._instance.remove_child (picon1.gameObject);
			if (m_post.type.Count >= 1)
			{
				GameObject icon1 = icon_manager._instance.create_reward_icon(m_post.type[0], m_post.value1[0], m_post.value2[0], m_post.value3[0]);
				icon1.transform.parent = picon1;
				icon1.transform.localPosition = new Vector3(0,0,0);
				icon1.transform.localScale = new Vector3(1,1,1);
				picon1.gameObject.SetActive(true);
				m_reward.Find("Label").gameObject.SetActive (true);
			}
			else
			{
				picon1.gameObject.SetActive(false);
			}
			
			Transform picon2 = m_reward.Find("icon2");
			sys._instance.remove_child (picon2.gameObject);
			if (m_post.type.Count >= 2)
			{
				GameObject icon2 = icon_manager._instance.create_reward_icon(m_post.type[1], m_post.value1[1], m_post.value2[1], m_post.value3[1]);
				icon2.transform.parent = picon2;
				icon2.transform.localPosition = new Vector3(0,0,0);
				icon2.transform.localScale = new Vector3(1,1,1);
				picon2.gameObject.SetActive(true);
			}
			else
			{
				picon2.gameObject.SetActive(false);
			}

			Transform picon3 = m_reward.Find("icon3");
			sys._instance.remove_child (picon3.gameObject);
			if (m_post.type.Count >= 3)
			{
				GameObject icon3 = icon_manager._instance.create_reward_icon(m_post.type[2], m_post.value1[2], m_post.value2[2], m_post.value3[2]);
				icon3.transform.parent = picon3;
				icon3.transform.localPosition = new Vector3(0,0,0);
				icon3.transform.localScale = new Vector3(1,1,1);
				picon3.gameObject.SetActive(true);
			}
			else
			{
				picon3.gameObject.SetActive(false);
			}

			if (m_post.type.Count == 0)
			{
				m_get.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jt_mobai.cs_217_99");//关闭
				m_reward.gameObject.SetActive(true);
			}
			else
			{
				m_reward.gameObject.SetActive(true);
			}
		}
		else
		{
			if(m_post.type.Count  == 0)
			{
				m_reward.gameObject.SetActive(true);
			}
			else
			{
				m_reward.gameObject.SetActive(true);
			}
			m_get.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jt_mobai.cs_217_99");//关闭
			transform.Find("back").Find("title").GetComponent<UILabel>().text = game_data._instance.m_postzd_list[m_zd_index].title;
			m_text.GetComponent<UILabel>().text = game_data._instance.m_postzd_list[m_zd_index].text.Replace("{nn}","\n");
			transform.Find("back").Find("sender").GetComponent<UILabel>().text = game_data._instance.get_t_language ("post.cs_41_26");//斗人少尉
			Transform picon1 = m_reward.Find("icon1");
			sys._instance.remove_child (picon1.gameObject);
			picon1.gameObject.SetActive(true);
			Transform picon2 = m_reward.Find("icon2");
			sys._instance.remove_child (picon2.gameObject);
			picon2.gameObject.SetActive(true);
			Transform picon3 = m_reward.Find("icon3");
			sys._instance.remove_child (picon3.gameObject);
			picon3.gameObject.SetActive(true);
		}
		m_text.transform.parent.GetComponent<UIScrollView>().ResetPosition();

	}
	
	// Update is called once per frame
	void Update () {

	}
}
