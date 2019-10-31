
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mi_jing_jl : MonoBehaviour {
	
	public GameObject m_cur;
	public GameObject m_desc;
	public List<GameObject> m_icons;
	public GameObject m_close;
	public GameObject m_lq;



	void Start () {
	
	
	}

	public void OnEnable()
	{
		reset ();
	}

	public void reset()
	{
		int index = sys._instance.m_self.m_t_player.ttt_cur_stars.Count;
		if (sys._instance.m_self.m_t_player.ttt_can_reward == 1)
		{
			index -= 1;
		}
		int d1 = index / 3 * 3 + 1;
		int d2 = d1 + 2;
		m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("mi_jing_jl.cs_36_55"),d1.ToString (),d2.ToString ());//在{0}~{1}层中获得星数达到以下数量时可获得
		int star = 0;
		for (int i = d1 - 1; i < d1 + 2; ++i)
		{
			if (i < sys._instance.m_self.m_t_player.ttt_cur_stars.Count)
			{
				star += sys._instance.m_self.m_t_player.ttt_cur_stars[i];
			}
		}
		m_cur.GetComponent<UILabel>().text = star.ToString();
		int reward_index = index / 3 + 1;
		s_t_ttt_reward t_ttt_reward = game_data._instance.get_t_ttt_reward (reward_index);
		for (int i = 0; i < m_icons.Count; ++i)
		{
			GameObject icon = m_icons[i];
			sys._instance.remove_child(icon);
			int t1 = i / 3;
			int t2 = i % 3;
			if (t_ttt_reward.rewardss[t1][t2].type == 0 || sys._instance.is_hide_reward(t_ttt_reward.rewardss[t1][t2].type,t_ttt_reward.rewardss[t1][t2].value1))
			{
				icon.SetActive(false);
				continue;
			}
			icon.SetActive(true);
			GameObject _icon = icon_manager._instance.create_reward_icon (t_ttt_reward.rewardss[t1][t2].type, t_ttt_reward.rewardss[t1][t2].value1,
			                                                              t_ttt_reward.rewardss[t1][t2].value2, t_ttt_reward.rewardss[t1][t2].value3);
			_icon.transform.parent = icon.transform;
			_icon.transform.localPosition = new Vector3 (0, 0, 0);
			_icon.transform.localScale = new Vector3 (1, 1, 1);
		}

		if (sys._instance.m_self.m_t_player.ttt_can_reward == 0)
		{
            m_close.transform.parent.gameObject.SetActive(true);
			m_close.GetComponent<UISprite>().alpha = 1.0f;
			m_close.GetComponent<BoxCollider>().enabled = true;
			m_lq.GetComponent<UISprite>().set_enable(false);
			m_lq.GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
            m_close.transform.parent.gameObject.SetActive(false);
			m_close.GetComponent<UISprite>().alpha = 1.0f;
			m_close.GetComponent<BoxCollider>().enabled = false;
			m_lq.GetComponent<UISprite>().set_enable(true);
			m_lq.GetComponent<BoxCollider>().enabled = true;
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "close2")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if (obj.name == "lq")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();

			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();	
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_TTT_REWARD, _msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
