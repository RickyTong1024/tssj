
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mi_jing_nd : MonoBehaviour {

	public GameObject m_guan;
	public GameObject m_tj;
	public List<GameObject> m_golds;
	public List<GameObject> m_bfs;
	public List<GameObject> m_hjs;
	private int m_nd = 0;
	void Start () {

	}

	public void OnEnable()
	{
		reset ();
	}

	public void reset()
	{
		int index = sys._instance.m_self.m_t_player.ttt_cur_stars.Count;
		s_t_ttt t_ttt = game_data._instance.get_t_ttt(index + 1);
		m_guan.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("boss.cs_234_75"),t_ttt.index.ToString()) ;//第{0}关
		m_tj.GetComponent<UILabel>().text = t_ttt.tj;
		for (int i = 0; i < 3; ++i)
		{
			m_golds[i].GetComponent<UILabel>().text = sys._instance.get_res_color(1) + sys._instance.value_to_wan((long)t_ttt.guais[i].gold);
			m_hjs[i].GetComponent<UILabel>().text = sys._instance.get_res_color(6) +sys._instance.value_to_wan((long)t_ttt.guais[i].hj);
			long bf = long.Parse(t_ttt.guais[i].bf);
			m_bfs[i].GetComponent<UILabel>().text = sys._instance.value_to_wan(bf);
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "duixing")
		{
			s_message _message = new s_message();
			_message.m_type = "show_duixing_gui";
			_message.m_bools.Add(true);
			cmessage_center._instance.add_message(_message);
			return;
		}
		if (obj.name == "close2")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			return;
		}
		if (obj.name == "pt")
		{
			m_nd = 1;
			int fight_id = sys._instance.m_self.m_t_player.ttt_cur_stars.Count + 1;
			fight(fight_id);
		}
		if (obj.name == "kn")
		{
			m_nd = 2;
			int fight_id = sys._instance.m_self.m_t_player.ttt_cur_stars.Count + 1;
			fight(fight_id);
		}
		if (obj.name == "dy")
		{
			m_nd = 3;
			int fight_id = sys._instance.m_self.m_t_player.ttt_cur_stars.Count + 1;
			fight(fight_id);
		}
		this.transform.Find("frame_big").GetComponent<frame>().hide();
		this.gameObject.SetActive (false);
	}

	void fight(int id)
	{
		sys._instance.m_game_state = "buttle";
		protocol.game.cmsg_ttt_fight_end _msg = new protocol.game.cmsg_ttt_fight_end ();
		_msg.nd = m_nd;
		net_http._instance.send_msg<protocol.game.cmsg_ttt_fight_end> (opclient_t.CMSG_TTT_FIGHT_END, _msg);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
