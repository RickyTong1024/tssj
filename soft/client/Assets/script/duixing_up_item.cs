
using UnityEngine;
using System.Collections;

public class duixing_up_item : MonoBehaviour {

	public s_t_duixng_skill t_duixing_skill;
	public GameObject m_name;
	public GameObject m_desc;
	public GameObject m_kaiqi;
	public GameObject m_lock;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		string s = "";
		if(t_duixing_skill.level > sys._instance.m_self.m_t_player.duixing_level)
		{
			m_lock.SetActive(true);
		}
		else
		{
			m_lock.SetActive(false);
		}
		m_name.GetComponent<UILabel>().text = s + t_duixing_skill.name;
		s = "[0af6ff]";
		m_desc.GetComponent<UILabel>().text = s + t_duixing_skill.desc;
		s = "[0aabff]";
		m_kaiqi.GetComponent<UILabel>().text = s + string.Format(game_data._instance.get_t_language ("duixing_up_item.cs_31_60"),t_duixing_skill.level );//升至{0}开启
	}
	// Update is called once per frame
	void Update () {
	
	}
}
