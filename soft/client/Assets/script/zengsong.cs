
using UnityEngine;
using System.Collections;

public class zengsong : MonoBehaviour {

	public dhc.social_t m_social;
	public GameObject m_touxiang;
	public GameObject m_level;
	public GameObject m_name;
	public GameObject m_bf;

	public UILabel m_zl_Label;
	public UILabel m_tili_Label;
	// Use this for initialization
	void Start () {
	}

	public void reset()
	{	
		sys._instance.remove_child (m_touxiang);
        GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_social.template_id, m_social.achieve, m_social.vip, m_social.nalflag);
		
		_obj1.transform.parent = m_touxiang.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		m_level.GetComponent<UILabel>().text = m_social.level.ToString();
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(m_social.achieve) +  m_social.name;
		m_bf.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)m_social.bf);
	}

	public void click(GameObject obj)
	{
		if (obj.name == "look") 
		{
			protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look ();
			_msg.target_guid = m_social.target_guid;
			net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
