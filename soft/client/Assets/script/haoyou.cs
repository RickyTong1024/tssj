
using UnityEngine;
using System.Collections;

public class haoyou : MonoBehaviour {

	public dhc.social_t m_social;
	public GameObject m_touxiang;
	public GameObject m_level;
	public GameObject m_name;
	public GameObject m_bf;
	public GameObject m_song;
	public GameObject m_ysong;
    public UILabel m_zaixian;
	public UILabel m_zhanli;
	public UILabel m_zengsongtili;
	// Use this for initialization
	void Start () 
	{
		
	}

	
	public void reset()
	{		
		sys._instance.remove_child (m_touxiang);
		GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_social.template_id,m_social.achieve,m_social.vip,m_social.nalflag);
		
		_obj1.transform.parent = m_touxiang.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		m_level.GetComponent<UILabel>().text = m_social.level.ToString();
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(m_social.achieve) +  m_social.name;
		m_bf.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)m_social.bf);

		if (timer.trigger_time(m_social.last_song_time, 0, 0))
		{
			m_song.SetActive(true);
			m_ysong.SetActive(false);
		}
		else
		{
			m_song.SetActive(false);
			m_ysong.SetActive(true);
		}
        if (timer.now() < m_social.offline_time + 300000)
        {
            m_zaixian.GetComponent<UILabel>().text = game_data._instance.get_t_language ("haoyou.cs_47_53");//[ffc864]在线
        }
        else
        {
            m_zaixian.GetComponent<UILabel>().text = "[ff5000]" + timer.get_guild_show(m_social.offline_time);
        }
	}

	public void click(GameObject obj)
	{
		if (obj.name == "look")
		{
			protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look ();
			_msg.target_guid = m_social.target_guid;
			net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);
		}
		else if (obj.name == "delete")
		{
			s_message mes = new s_message();
			mes.m_type = "delete_haoyou";
			mes.m_long.Add(m_social.guid);

			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("haoyou.cs_69_51") + "?", mes);//提示//您将删除好友，是否继续
		}
		else if (obj.name == "song")
		{
			protocol.game.cmsg_social_song _msg = new protocol.game.cmsg_social_song ();
			_msg.social_guid = m_social.guid;
			net_http._instance.send_msg<protocol.game.cmsg_social_song> (opclient_t.CMSG_SOCIAL_SONG, _msg);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
