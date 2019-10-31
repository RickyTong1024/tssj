
using UnityEngine;
using System.Collections;

public class post_gui : MonoBehaviour {

	public GameObject post_panel_;
	public UILabel m_name;
	public UILabel m_no_post;

	// Use this for initialization
	void Start () {

	}

	public void OnEnable()
	{
		sys._instance.m_self.is_postzd = false;
		post_panel_.GetComponent<post_post_panel>().clear ();

		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_POST_LOOK, _msg);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public static bool can_post()
	{
		if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_post)
		{
			return false;
		}
		if (sys._instance.m_self.is_post > 0)
		{
			return true;
		}
		if (sys._instance.m_self.is_postzd && sys._instance.m_self.m_t_player.level >= 10)
		{
			return true;
		}
		return false;
	}

}
