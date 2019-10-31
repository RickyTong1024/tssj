
using UnityEngine;
using System.Collections;

public class mubiao : MonoBehaviour {

	public int m_mubiao_id;
	public int m_num;
	public UILabel m_lq_Label;
	// Use this for initialization
	void Start () {

	}

	public void reset()
	{
		s_t_target t_target = game_data._instance.get_t_target(m_mubiao_id);
		string s1 = "";
		if (m_num >= t_target.tjnum)
		{
			s1 = game_data._instance.get_t_language ("active.cs_19_75");//目标达成
			transform.Find("wcd").GetComponent<UILabel>().text = "[ffffff]" + s1;
			transform.Find("wcd").transform.localPosition = new Vector3(334,32, 0);
			transform.Find("gou").gameObject.SetActive(true);
			transform.Find("main").gameObject.SetActive(false);
			transform.Find("main1").gameObject.SetActive(true);
		}
		else
		{
			transform.Find("wcd").GetComponent<UILabel>().text = "[24d0fd]" + m_num.ToString() + "/" + t_target.tjnum.ToString();
			transform.Find("wcd").transform.localPosition = new Vector3(334,-24, 0);
			transform.Find("gou").gameObject.SetActive(false);
			transform.Find("main").gameObject.SetActive(true);
			transform.Find("main1").gameObject.SetActive(false);
		}
		string text1 = game_data._instance.get_t_language ("active.cs_45_16")+ ": ";//奖励
		string text = text1 + sys._instance.get_res_info (t_target.reward.type, t_target.reward.value1, t_target.reward.value2, t_target.reward.value3);
		transform.Find("reward").GetComponent<UILabel>().text = text;
		transform.Find("desc").GetComponent<UILabel>().text = t_target.desc;
		transform.Find("icon").GetComponent<UISprite>().spriteName = t_target.icon;
	}

	// Update is called once per frame
	void Update () {
	
	}
   


}
