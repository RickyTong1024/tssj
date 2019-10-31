
using UnityEngine;
using System.Collections;

public class toper_list_item : MonoBehaviour {

	private ulong m_guid;
	public UILabel m_look_Label;
	public GameObject m_star;
	public GameObject m_fight;
	public GameObject m_icon;
	public GameObject m_chenghao;
	// Use this for initialization
	void Start () {
	}

	public void reset(int type, int rank, ulong guid, string name, int level, int bf, int value,int template,int count ,int vip,int chenghao_id,int gq)
	{
		m_guid = guid;
		string s = sys._instance.value_to_wan(bf);
		if (type == 4)
		{
			s = value.ToString();
		}
		else if (type == 2)
		{
			s = game_data._instance.get_t_language ("toper_gui.cs_128_7");//未知
			for (int i = 0; i < game_data._instance.m_dbc_mission.get_y(); ++i)
			{
				s_t_mission t_mission = game_data._instance.get_t_mission(int.Parse(game_data._instance.m_dbc_mission.get(0,i)));
				if (t_mission.index_id == value)
				{
					string[] text = t_mission.name.Split(' ');
					s = text[0];
					if(text.Length >= 2)
					{
						s = "[ " + text[0] + " ]" + "\n" + text[1];
					}
					break;
				}
			}
		}
		else if (type == 3)
		{
			s = game_data._instance.get_t_language ("toper_gui.cs_128_7");//未知
			for (int i = 0; i < game_data._instance.m_dbc_mission.get_y(); ++i)
			{
				s_t_mission t_mission = game_data._instance.get_t_mission(int.Parse(game_data._instance.m_dbc_mission.get(0,i)));
				if (t_mission.jyindex_id == value)
				{
					string[] text = t_mission.name.Split(' ');
					s = text[0];
					if(text.Length >= 2)
					{
						s = "[ " + text[0] + " ]" + "\n" + text[1];
					}
					break;
				}
			}
		}
		UILabel s1 = m_fight.transform.GetComponent<UILabel>();
		m_fight.SetActive (true);
		m_star.SetActive(false);
		this.transform.Find("star_num").gameObject.SetActive (false);
		this.transform.Find("num").gameObject.SetActive (false);
		switch(type)
		{

			case 0:
			s1.text =  s;
				break;
			case 1:
			s1.text =  s;
				break;
			case 2:
			m_star.SetActive(false);
			m_fight.SetActive(false);
			this.transform.Find("num").gameObject.SetActive (true);
			this.transform.Find("num").GetComponent<UILabel>().text = "[0aabff]" + s;
				break;
			case 3:
			m_star.SetActive(false);
			m_fight.SetActive(false);
			this.transform.Find("num").gameObject.SetActive (true);
			this.transform.Find("num").GetComponent<UILabel>().text = "[0aabff]" + s;
				break;
			case 4:
			m_star.SetActive(true);
			m_fight.SetActive(false);
			this.transform.Find("star_num").gameObject.SetActive (true);
			this.transform.Find("star_num").GetComponent<UILabel>().text = "[f8cf40]" + s;
				break;

		}
		this.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), rank.ToString()//第{0}名
			);
		this.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(count) + name;
		this.transform.Find("level").GetComponent<UILabel>().text = level.ToString();
		sys._instance.remove_child (m_icon);
        GameObject _obj1 = icon_manager._instance.create_player_icon(template, count, vip, gq);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		sys._instance.get_chenghao (chenghao_id, m_chenghao);
	}

	public void click(GameObject obj)
	{
		if (obj.name == "look")
		{
			protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look ();
			_msg.target_guid = m_guid;
			net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
