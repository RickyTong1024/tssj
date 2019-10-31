
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class min_dialog_box : MonoBehaviour {

	public GameObject m_image_obj;
	public GameObject m_name_obj;
	public GameObject m_des_obj;
	public GameObject m_level;

	public string m_name;
	public string m_des;
	public string m_pz;
	public ArrayList m_ints;

	// Use this for initialization
	void Start () {

	}
	public void OnEnable()
	{
		m_name_obj.GetComponent<UILabel>().text = m_name;
		if(m_ints.Count >= 4 && (int)m_ints[0] == 8)
		{
			m_des_obj.GetComponent<UILabel>().text = m_des;
		}
		else
		{
			m_des_obj.GetComponent<UILabel>().text = "[fff300]" + m_des;
		}
		if (m_ints.Count >= 5 && (int)m_ints[0] != 3)
		{
			int nlevel = (int)m_ints[4];
			if (nlevel > sys._instance.m_self.m_t_player.level)
			{
				m_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("min_dialog_box.cs_38_57") , nlevel.ToString());//[ff0000]{0}级可使用
			}
			else
			{
				m_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("min_dialog_box.cs_42_57") , nlevel.ToString());//[00ff00]{0}级可使用
			}
		}
		else if(m_pz != "")
		{
			m_level.GetComponent<UILabel>().text = m_pz;
		}
		else
		{
			m_level.GetComponent<UILabel>().text = "";
		}
		sys._instance.remove_child (this.transform.Find("image").Find("icon").gameObject);
		GameObject icon = null;
		if ((int)m_ints[0] == 0)
		{
			icon = icon_manager._instance.create_dress_icon_ex((int)m_ints[1],1);
		}
		else if((int)m_ints[0] == 3)
		{
			icon = icon_manager._instance.create_card_icon((int)m_ints[1],(int)m_ints[3],(int)m_ints[4],(int)m_ints[2]);
		}
		else
		{
			icon = icon_manager._instance.create_reward_icon((int)m_ints[0], (int)m_ints[1], (int)m_ints[2], (int)m_ints[3]);
		}
		Transform iicon = this.transform.Find("image").Find("icon");
		if (icon == null)
		{
			iicon.gameObject.SetActive(false);
		}
		else
		{
			iicon.gameObject.SetActive(true);
			icon.transform.parent = iicon;
			icon.transform.localPosition = new Vector3(0,0,0);
			icon.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
			icon.GetComponent<BoxCollider>().enabled = false;
		}
	}

	// Update is called once per frame
	void Update () {

	}
}
