
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class other_reward_gui : MonoBehaviour {

	public List<s_t_reward> m_rewards;
	public s_message m_out_message;
	public string m_name;
	public string m_desc;
	public bool m_flag;

	public GameObject m_icon1;
	public GameObject m_icon2;
	public GameObject m_icon3;
	public GameObject m_ok;
	public GameObject m_gname;
	public GameObject m_gdesc;

	public UILabel m_get_Label;
	// Use this for initialization
	void Start () {

	}

	public void hide()
	{
		transform.GetComponent<ui_show_anim>().hide_ui();
	}
	
	public void update_ui()
	{
		m_gname.GetComponent<UILabel>().text = m_name;
		m_gdesc.GetComponent<UILabel>().text = m_desc;
		if (m_flag)
		{
			m_ok.GetComponent<UISprite>().set_enable(true);
			m_ok.GetComponent<BoxCollider>().enabled = true;
		}
		else
		{
			m_ok.GetComponent<UISprite>().set_enable(false);
			m_ok.GetComponent<BoxCollider>().enabled = false;
		}
		{
			sys._instance.remove_child (m_icon1);
			GameObject icon1 = icon_manager._instance.create_reward_icon (m_rewards[0].type, m_rewards[0].value1, m_rewards[0].value2, m_rewards[0].value3);
			icon1.transform.parent = m_icon1.transform;
			icon1.transform.localPosition = new Vector3(0,0,0);
			icon1.transform.localScale = new Vector3(1,1,1);
		}
		{
			sys._instance.remove_child (m_icon2);
			GameObject icon2 = icon_manager._instance.create_reward_icon (m_rewards[1].type, m_rewards[1].value1, m_rewards[1].value2, m_rewards[1].value3);
			icon2.transform.parent = m_icon2.transform;
			icon2.transform.localPosition = new Vector3(0,0,0);
			icon2.transform.localScale = new Vector3(1,1,1);
		}
		{
			sys._instance.remove_child (m_icon3);
			GameObject icon3 = icon_manager._instance.create_reward_icon (m_rewards[2].type, m_rewards[2].value1, m_rewards[2].value2, m_rewards[2].value3);
			icon3.transform.parent = m_icon3.transform;
			icon3.transform.localPosition = new Vector3(0,0,0);
			icon3.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			cmessage_center._instance.add_message(m_out_message);
			this.GetComponent<ui_show_anim>().hide_ui ();

		}
		else if (obj.name == "hide")
		{
			this.GetComponent<ui_show_anim>().hide_ui ();
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
