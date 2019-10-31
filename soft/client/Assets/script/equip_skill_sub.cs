
using UnityEngine;
using System.Collections;

public class equip_skill_sub : MonoBehaviour {

	public s_t_equip_skill t_equip_skill;
	public int jinlian;
	public GameObject m_skill_name;
	public GameObject m_skill_desc;
	public GameObject m_skill_kq;
	public GameObject m_lock;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		string s = "";
		if(jinlian < t_equip_skill.jinglian)
		{
			m_lock.SetActive(true);
		}
		else
		{
			m_lock.SetActive(false);
		}
		m_skill_name.GetComponent<UILabel>().text = s + t_equip_skill.name;
		s = "";
		if(t_equip_skill.type == 1)
		{
			string[] text = game_data._instance.get_value_string(t_equip_skill.def1,t_equip_skill.def2).Split('+');
			s += "[0aabff]" + text[0] + "[-]" + "[0aff16]+" +text[1] + "[-]";
		}
		else if(t_equip_skill.type == 2)
		{
			t_equip_skill.desc = t_equip_skill.desc.Replace("%","");
			s +=   "[0aabff]" + t_equip_skill.desc.Replace("{{n1}}", "[-][0aff16]" + t_equip_skill.def1.ToString() + "%[-][0aabff]");
		}
		else if(t_equip_skill.type == 3 || t_equip_skill.type == 4 || t_equip_skill.type == 5 
		        || t_equip_skill.type == 6 || t_equip_skill.type == 7)
		{
			string text = "";
			t_equip_skill.desc = t_equip_skill.desc.Replace("%","");
			text =  "[0aabff]" + t_equip_skill.desc.Replace("{{n1}}",  "[-][0aff16]" + t_equip_skill.def1.ToString() + "%[-][0aabff]");
			text =  "[0aabff]" + text.Replace("{{n2}}", "[-][0aff16]" + t_equip_skill.def2.ToString() + "%[-][0aabff]");
			s += text;
		}
		m_skill_desc.GetComponent<UILabel>().text = s;
		m_skill_kq.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_skill_sub.cs_50_59"), t_equip_skill.jinglian);//[ffde00]精炼至[-][0aff16]{0}阶[-][ffde00]开启[-]
	}
	

	// Update is called once per frame
	void Update () {
	
	}
}
