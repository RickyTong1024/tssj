
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class duixing_item : MonoBehaviour {

	public GameObject m_use;
	public GameObject m_name;
	public GameObject m_lock;
	public GameObject m_level;
	public List<GameObject> m_items = new List<GameObject>();
	public s_t_duixng t_duixing;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		m_use.SetActive (false);
		m_lock.SetActive (false);
		m_level.SetActive (false);
		for(int i = 0 ; i < m_items.Count;++i)
		{
			m_items[i].SetActive(false);
		}
		for(int i = 0;i < t_duixing.zhenwei.Count;++i)
		{
			m_items[t_duixing.zhenwei[i]].SetActive(true);
		}
		if(t_duixing.id == sys._instance.m_self.m_t_player.duixing_id)
		{
			m_use.SetActive(true);
		}
		else
		{
			m_use.SetActive(false);
		}
		if(t_duixing.level > sys._instance.m_self.m_t_player.level)
		{
			m_lock.SetActive(true);
			m_level.SetActive(true);
			m_level.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("duixing_gui.cs_431_71"),t_duixing.level.ToString());//{0}级开启
		}
		else
		{
			m_lock.SetActive(false);
			m_level.SetActive(false);
		}
		m_name.GetComponent<UILabel>().text = t_duixing.name;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
