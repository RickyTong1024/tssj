
using UnityEngine;
using System.Collections;

public class explore_task_item : MonoBehaviour {

	public int explore_mubiao_id;
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
		s_t_manyou_mubiao t_manyou_mubiao = game_data._instance.get_t_manyou_mubiao (explore_mubiao_id);
		if(explore_task_gui.active_done(t_manyou_mubiao))
		{
			m_wcd.SetActive(false);
			m_gou.SetActive(false);
			m_main.SetActive(true);
			m_main1.SetActive(false);
			m_ylq.SetActive(true);
		}
		else if (explore_task_gui.active_vis(t_manyou_mubiao))
		{
			m_wcd.GetComponent<UILabel>().text ="[ffffff]" + game_data._instance.get_t_language ("active.cs_19_75");//目标达成
			m_gou.SetActive(true);
			m_main.SetActive(false);
			m_main1.SetActive(true);
			m_ylq.SetActive(false);
		}
		else
		{
			m_wcd.GetComponent<UILabel>().text = game_data._instance.get_t_language ("explore_task_item.cs_45_41") + "：[24d0fd]" + explore_gui._instance.score + "/" + t_manyou_mubiao.score;//进度
			m_gou.SetActive(false);
			m_main.SetActive(true);
			m_main1.SetActive(false);
			m_ylq.SetActive(false);
		}
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(t_manyou_mubiao.reward.type,t_manyou_mubiao.reward.value1,t_manyou_mubiao.reward.value2,t_manyou_mubiao.reward.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		string text = "[0aabff]" + game_data._instance.get_t_language ("active.cs_45_16") + ":[-]";//奖励
		text += sys._instance.get_res_info (t_manyou_mubiao.reward.type,t_manyou_mubiao.reward.value1,t_manyou_mubiao.reward.value2,t_manyou_mubiao.reward.value3);
		string text1 = string.Format(game_data._instance.get_t_language ("explore_task_item.cs_59_31"), t_manyou_mubiao.score);//积分达到{0}分
		m_task_name.GetComponent<UILabel>().text = text1;
		m_reward.GetComponent<UILabel>().text = text;
	}
}
