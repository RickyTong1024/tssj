
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class first_recharge_gui : MonoBehaviour, IMessage {
    
	public GameObject m_recharge;
	public GameObject m_effect;


	public UILabel m_yijianlingqu;
	public UILabel m_desc1;
	public UILabel m_desc2;
	public UILabel m_desc3;
	public UILabel m_desc4;

	public UILabel m_1;
	public UILabel m_2;
	public UILabel m_3;
	public UILabel m_4;
	public UILabel m_5;
	public UILabel m_6;
	public UILabel m_7;
	public UILabel m_8;
	public UILabel m_9;
	public UILabel m_10;
	// Use this for initialization
	void Start ()
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void init()
	{
        if (platform_config_common.game_model == 2)
        {
            this.transform.Find("main/GameObject/ww").gameObject.GetComponent<UILabel>().text = "699999";
            this.transform.Find("main/GameObject/Texture").gameObject.GetComponent<UITexture>().mainTexture = sys._instance.get_image_half("bsx_sqks");
        }
		for (int i = 0; i < 6; ++i)
		{
			s_t_reward t_reward = new s_t_reward();
			t_reward.type = int.Parse(game_data._instance.m_dbc_first_recharge.get (0, i));
			t_reward.value1 = int.Parse(game_data._instance.m_dbc_first_recharge.get (1, i));
			t_reward.value2 = int.Parse(game_data._instance.m_dbc_first_recharge.get (2, i));
			t_reward.value3 = int.Parse(game_data._instance.m_dbc_first_recharge.get (3, i));

            GameObject icon = icon_manager._instance.create_reward_icon(t_reward.type, t_reward.value1, t_reward.value2, t_reward.value3);
            icon.transform.parent = this.transform.Find("main/GameObject/icon" + (i + 1).ToString());
            icon.transform.localPosition = new Vector3(0, 0, 0);
            icon.transform.localScale = new Vector3(1, 1, 1);
        }
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "hide")
		{
			transform.GetComponent<ui_show_anim>().hide_ui();
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.transform.name == "recharge")
		{
			if (sys._instance.m_self.m_t_player.first_reward == 0)
			{
				transform.GetComponent<ui_show_anim>().hide_ui();
				s_message _msg = new s_message();
				_msg.m_type = "show_recharge";
				cmessage_center._instance.add_message(_msg);
				s_message _message = new s_message();
				_message.m_type = "show_main_gui";
				cmessage_center._instance.add_message(_message);
			}
			else
			{
				protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
				net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_FIRST_RECHARGE, _msg);
			}
			
		}
	}

	void IMessage.message (s_message message)
	{
        if (message.m_type == "hide_first_recharge")
        {
            transform.GetComponent<ui_show_anim>().hide_ui();
        }
		
	}
	
	void IMessage.net_message (s_net_message message)
	{

	}
}
