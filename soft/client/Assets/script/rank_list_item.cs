
using UnityEngine;
using System.Collections;

public class rank_list_item : MonoBehaviour {

	public ulong m_guid;
	public int m_is_npc;
	public int m_rank;
	//public GameObject m_zh;
	//public GameObject m_jewel;
	// Use this for initialization
	public UILabel m_look_Label;
	public UILabel m_level_Label;
	public UILabel m_zl_Label;
	void Start () {

	}

	public void reset()
	{
		for(int i = 0;i < game_data._instance.m_dbc_sport_rank.get_y();i ++)
		{
			int _min = int.Parse(game_data._instance.m_dbc_sport_rank.get(0,i));
			int _max = int.Parse(game_data._instance.m_dbc_sport_rank.get(1,i));
			
			/*if(m_rank >= _min && m_rank <= _max)
			{
				m_zh.GetComponent<UILabel>().text = game_data._instance.m_dbc_sport_rank.get(3,i);
				m_jewel.GetComponent<UILabel>().text = game_data._instance.m_dbc_sport_rank.get(2,i);
				break;
			}*/
		}
	}

	public void click(GameObject obj) {
		if (obj.name == "look")
		{
			if (m_is_npc == 1)
			{
				string s = game_data._instance.get_t_language ("rank_list_item.cs_40_15");//[ffc882]我是NPC，别点我
				root_gui._instance.show_prompt_dialog_box(s);
				return;
			}
			protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look ();
			_msg.target_guid = m_guid;
			net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
