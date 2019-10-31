
using UnityEngine;
using System.Collections;

public class guanghuan_up_item : MonoBehaviour {

	public s_t_guanghuan_skill t_guanghuan_skill;
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
		if(t_guanghuan_skill.enhance > guanghuan_gui.guanghuan_skill_level(t_guanghuan_skill))
		{
			m_lock.SetActive(true);
		}
		else
		{
			m_lock.SetActive(false);
		}
		m_name.GetComponent<UILabel>().text = s + t_guanghuan_skill.name;
		s = "[0af6ff]";
		string desc = t_guanghuan_skill.desc;
		if(t_guanghuan_skill.type == 1)
		{
			desc = game_data._instance.get_value_string(t_guanghuan_skill.def1,t_guanghuan_skill.def2,1);
		}
		if(t_guanghuan_skill.type == 2 || t_guanghuan_skill.type == 3)
		{
			desc = t_guanghuan_skill.desc;
			desc = desc.Replace("{{n1}}",t_guanghuan_skill.def1.ToString());
		}
		m_desc.GetComponent<UILabel>().text = s + desc;
		s = "[0aabff]";
		m_kaiqi.GetComponent<UILabel>().text = s + string.Format(game_data._instance.get_t_language ("guanghuan_up_item.cs_41_60") , t_guanghuan_skill.enhance);//强化到{0}开启
		int width = m_desc.GetComponent<UILabel>().width;
		if(width >= 378)
		{
			m_desc.GetComponent<UILabel>().spacingX = -1;
		}
		else
		{
			m_desc.GetComponent<UILabel>().spacingX = 0;
		}
	}
}
