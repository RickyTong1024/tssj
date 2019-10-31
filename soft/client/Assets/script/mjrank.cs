
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mjrank : MonoBehaviour {

	public GameObject m_rank;
	public GameObject m_name;
	public GameObject m_bf;
	public GameObject m_star;
	public GameObject m_icon;
	public ulong m_guid;
	void Start () {
	
	}

    public void reset(int rank, string name, int bf, int value, int template_id, int achieve, int vip, int chenhao, int nationality)
	{
		m_rank.GetComponent<UILabel>().text = rank.ToString();
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(achieve) + name;
		m_bf.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)bf);
		m_star.GetComponent<UILabel>().text = value.ToString();
		sys._instance.remove_child (m_icon);
        GameObject _obj1 = icon_manager._instance.create_player_icon((int)template_id, achieve, vip, nationality);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
        sys._instance.get_chenghao(chenhao, this.transform.Find("chenhao").gameObject);

	}

	public void click(GameObject obj)
	{
		protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look ();
		_msg.target_guid = m_guid;
		net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
