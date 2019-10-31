
using UnityEngine;
using System.Collections;

public class sousuo : MonoBehaviour {

	public protocol.game.msg_social_player m_social_player;
	public dhc.social_t m_social;
	public bool is_add = false;
	public GameObject m_add;
	public GameObject m_jsq;
	public GameObject m_touxiang;
	public GameObject m_level;
	public GameObject m_name;
	public GameObject m_bf;

	public UILabel m_zl_Label;
	public UILabel m_add_Label;
	public UILabel m_ysq_Lable;
    public UILabel m_zaixian;
	// Use this for initialization
	void Start () {

	}

	public void reset()
	{
		if (is_add)
		{
			m_add.SetActive(false);
			m_jsq.SetActive(true);
		}
		else
		{
			m_add.SetActive(true);
			m_jsq.SetActive(false);
		}
	
		sys._instance.remove_child (m_touxiang);
		GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_social_player.player_template,m_social_player.player_achieve,m_social_player.player_vip,m_social_player.nalflag);
		
		_obj1.transform.parent = m_touxiang.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		m_level.GetComponent<UILabel>().text = m_social_player.player_level.ToString();
        m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(m_social_player.player_achieve) + m_social_player.player_name;
		m_bf.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)m_social_player.player_bf);
        if (timer.now() < m_social_player.offline_time + 300000)
        {
            m_zaixian.GetComponent<UILabel>().text = game_data._instance.get_t_language ("haoyou.cs_47_53");//[ffc864]在线
        }
        else
        {
            m_zaixian.GetComponent<UILabel>().text = "[ff5000]" + timer.get_guild_show(m_social_player.offline_time);
        }
       
	}

	public void click(GameObject obj)
	{
		if (obj.name == "add")
		{
			protocol.game.cmsg_social_add _msg = new protocol.game.cmsg_social_add ();
			_msg.player_guid = m_social_player.player_guid;
			net_http._instance.send_msg<protocol.game.cmsg_social_add> (opclient_t.CMSG_SOCIAL_ADD, _msg);
		}
		if (obj.name == "look")
		{
			protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look ();
			_msg.target_guid = m_social_player.player_guid;
			net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
