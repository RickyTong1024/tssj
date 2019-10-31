
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mi_jing_jc : MonoBehaviour {
	
	public GameObject m_shuxing;
	public List<GameObject> m_sxs;
	public GameObject m_star;
	public GameObject m_mi_jing_gui;
	public GameObject m_view;

	public UILabel m_name;
	public UILabel m_cur_Label;
	public UILabel m_sx_add;
	public UILabel m_t1_star;
	public UILabel m_t2_star;
	public UILabel m_t3_star;

	void Start () {

	}

	public void OnEnable()
	{
		reset ();
	}

	public void reset()
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_view);
		string[] s  = mi_jing_gui.get_jc_string().Split('\n');
		for(int i =0; i < s.Length;++i)
		{
			if(s[i] != "")
			{
				GameObject temp = Instantiate(m_shuxing) as GameObject;
				temp.transform.parent = m_view.transform;
				temp.transform.localScale = Vector3.one;
				temp.transform.localPosition = new Vector3(0,159 - 47 * i,0);
				temp.SetActive(true);
				temp.name = "shuxing" + (i).ToString();
				temp.transform.Find("shuxing").GetComponent<UILabel>().text = s[i];
			}
		}
		m_star.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.ttt_star.ToString ();
		for (int i = 0; i < sys._instance.m_self.m_t_player.ttt_cur_reward_ids.Count; ++i)
		{
			int id = sys._instance.m_self.m_t_player.ttt_cur_reward_ids[i];
			s_t_ttt_value t_ttt_value = game_data._instance.get_t_ttt_value(id);
			m_sxs[i].GetComponent<UILabel>().text = game_data._instance.get_value_string (t_ttt_value.sxtype, t_ttt_value.sxvalue);
		}
	}

	public void click(GameObject obj)
	{
		int index = 0;
		if (obj.name == "t1")
		{
			if (sys._instance.m_self.m_t_player.ttt_star < 3)
			{
				string s = game_data._instance.get_t_language ("mi_jing_jc.cs_68_15");//[ffc882]星数不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			index = 0;
		}
		if (obj.name == "t2")
		{
			if (sys._instance.m_self.m_t_player.ttt_star < 6)
			{
				string s = game_data._instance.get_t_language ("mi_jing_jc.cs_68_15");//[ffc882]星数不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			index = 1;
		}
		if (obj.name == "t3")
		{
			if (sys._instance.m_self.m_t_player.ttt_star < 9)
			{
				string s = game_data._instance.get_t_language ("mi_jing_jc.cs_68_15");//[ffc882]星数不足
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			index = 2;
		}

		m_mi_jing_gui.GetComponent<mi_jing_gui>().m_jc_index = index;

		protocol.game.cmsg_ttt_value _msg = new protocol.game.cmsg_ttt_value ();
		_msg.index = index;
		net_http._instance.send_msg<protocol.game.cmsg_ttt_value> (opclient_t.CMSG_TTT_VALUE, _msg);

		this.transform.Find("frame_big").GetComponent<frame>().hide();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
