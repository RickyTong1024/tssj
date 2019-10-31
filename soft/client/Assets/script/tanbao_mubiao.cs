
using UnityEngine;
using System.Collections;

public class tanbao_mubiao : MonoBehaviour {

	public int tanbao_mubiao_id;
	public GameObject m_icon;
	public GameObject m_task_name;
	public GameObject m_reward;
	public GameObject m_main;
	public GameObject m_main1;
	public GameObject m_ylq;
	public GameObject m_wcd;
	public GameObject m_gou;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		s_t_tanbao_mubiao t_tanbao_mubiao = game_data._instance.get_t_tanbao_mubiao (tanbao_mubiao_id);
		if(tanbao_reward_gui.active_done(t_tanbao_mubiao))
		{
			m_wcd.SetActive(false);
			m_gou.SetActive(false);
			m_main.SetActive(true);
			m_main1.SetActive(false);
			m_ylq.SetActive(true);
		}
		else if (tanbao_reward_gui.active_vis(t_tanbao_mubiao))
		{
			m_wcd.GetComponent<UILabel>().text ="[ffffff]" + game_data._instance.get_t_language ("active.cs_19_75");//目标达成
			m_gou.SetActive(true);
			m_main.SetActive(false);
			m_main1.SetActive(true);
			m_ylq.SetActive(false);
		}
		else
		{
			m_wcd.GetComponent<UILabel>().text = game_data._instance.get_t_language ("boss_active.cs_35_63") + tanbao_gui._instance.qidian_num + "/" + t_tanbao_mubiao.task_num;//进度：[24d0fd]
			m_gou.SetActive(false);
			m_main.SetActive(true);
			m_main1.SetActive(false);
			m_ylq.SetActive(false);
		}
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(t_tanbao_mubiao.type,t_tanbao_mubiao.value1,t_tanbao_mubiao.value2,t_tanbao_mubiao.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		string text = "[0aabff]" + game_data._instance.get_t_language ("active.cs_45_16") + ":[-]";//奖励
		text += sys._instance.get_res_info (t_tanbao_mubiao.type,t_tanbao_mubiao.value1,t_tanbao_mubiao.value2,t_tanbao_mubiao.value3);
		string text1 = string.Format(game_data._instance.get_t_language ("tanbao_mubiao.cs_59_31") , t_tanbao_mubiao.task_num);//经过起点{0}次
		m_task_name.GetComponent<UILabel>().text = text1;
		m_reward.GetComponent<UILabel>().text = text;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
