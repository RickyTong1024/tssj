
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class online_reward_gui : MonoBehaviour, IMessage {

	public GameObject m_icon1;
	public GameObject m_icon2;
	public GameObject m_icon3;
	public GameObject m_num;
	public GameObject m_time;
	public GameObject m_name;
	public GameObject m_empty;

	public UILabel m_name_Label;
	public UILabel m_time_Label;
	public UILabel m_get_Label;
	private bool m_flag = false;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		m_flag = false;
		reset ();
	}

	public void reset()
	{
		m_num.transform.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.online_reward_index.ToString() + "/5";


		GameObject back = this.transform.Find("back").gameObject;

		s_t_online_reward t_or = game_data._instance.get_t_online_reward (sys._instance.m_self.m_t_player.online_reward_index);

		if (t_or == null)
		{
			m_empty.SetActive(true);
			m_name.SetActive(false);
			back.transform.Find("get").GetComponent<BoxCollider>().enabled = false;
			back.transform.Find("get").GetComponent<UISprite>().set_enable(false);
			back.transform.Find("watch").GetComponent<BoxCollider>().enabled = false;
			back.transform.Find("watch").GetComponent<UISprite>().set_enable(false);
			return;
		}
		else
		{
			m_empty.SetActive(false);
			m_name.SetActive(true);
		}

		sys._instance.remove_child (m_icon1.gameObject);
		if (t_or.rewards.Count >= 1)
		{
			GameObject icon1 = icon_manager._instance.create_reward_icon(t_or.rewards[0].type, t_or.rewards[0].value1, t_or.rewards[0].value2, t_or.rewards[0].value3);
			icon1.transform.parent = m_icon1.transform;
			icon1.transform.localPosition = new Vector3(0,0,0);
			icon1.transform.localScale = new Vector3(1,1,1);
			m_icon1.gameObject.SetActive(true);
		}
		else
		{
			m_icon1.gameObject.SetActive(false);
		}

		sys._instance.remove_child (m_icon2.gameObject);
		if (t_or.rewards.Count >= 2)
		{
			GameObject icon2 = icon_manager._instance.create_reward_icon(t_or.rewards[1].type, t_or.rewards[1].value1, t_or.rewards[1].value2, t_or.rewards[1].value3);
			icon2.transform.parent = m_icon2.transform;
			icon2.transform.localPosition = new Vector3(0,0,0);
			icon2.transform.localScale = new Vector3(1,1,1);
			m_icon2.gameObject.SetActive(true);
		}
		else
		{
			m_icon2.gameObject.SetActive(false);
		}

		sys._instance.remove_child (m_icon3.gameObject);
		if (t_or.rewards.Count >= 3)
		{
			GameObject icon3 = icon_manager._instance.create_reward_icon(t_or.rewards[2].type, t_or.rewards[2].value1, t_or.rewards[2].value2, t_or.rewards[2].value3);
			icon3.transform.parent = m_icon3.transform;
			icon3.transform.localPosition = new Vector3(0,0,0);
			icon3.transform.localScale = new Vector3(1,1,1);
			m_icon3.gameObject.SetActive(true);
		}
		else
		{
			m_icon3.gameObject.SetActive(false);
		}

		do_time ();
		if (sys._instance.m_self.m_t_player.online_reward_time > timer.now())
        {
            if (platform_config_common.m_ads == 1)
            {
                back.transform.Find("watch").GetComponent<BoxCollider>().enabled = false;
                back.transform.Find("watch").GetComponent<UISprite>().set_enable(false);
                if (back.transform.Find("get").gameObject.activeSelf)
                {
                    back.transform.Find("get").gameObject.SetActive(false);
                }
            }
            else
            {
                if (back.transform.Find("watch").gameObject.activeSelf)
                {
                    back.transform.Find("watch").gameObject.SetActive(false);
                }
                back.transform.Find("get").GetComponent<BoxCollider>().enabled = false;
                back.transform.Find("get").GetComponent<UISprite>().set_enable(false);
            }
		}
		else
        {
            if (platform_config_common.m_ads == 1)
            {
                back.transform.Find("watch").GetComponent<BoxCollider>().enabled = true;
                back.transform.Find("watch").GetComponent<UISprite>().set_enable(true);

                if (back.transform.Find("get").gameObject.activeSelf)
                {
                    back.transform.Find("get").gameObject.SetActive(false);
                }
            }
            else
            {
                if (back.transform.Find("watch").gameObject.activeSelf)
                {
                    back.transform.Find("watch").gameObject.SetActive(false);
                }
                back.transform.Find("get").GetComponent<BoxCollider>().enabled = true;
                back.transform.Find("get").GetComponent<UISprite>().set_enable(true);
            }
		}
	}

    public void handleFinished()
	{	
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_ONLINE_REWARD, _msg);
	}
    public void handleSkipped()
    {
        string s = game_data._instance.get_t_language ("online_reward_gui.cs_161_123");//跳过广告，无法获得奖励
        root_gui._instance.show_prompt_dialog_box("[ffd000]" + s);
    }
	public void click(GameObject obj)
	{
		if (obj.transform.name == "hide") 
		{
			transform.GetComponent<ui_show_anim>().hide_ui ();
		} 
		else if (obj.transform.name == "get") { 
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_ONLINE_REWARD, _msg);
		} 
		else if (obj.transform.name == "watch") 
		{
			if(UnityAdsHelper.isInitialized)//get vedio download ?
			{
                if (UnityAdsHelper.IsReady("rewardedVideo"))
                {
                    UnityAdsHelper.ShowAd("rewardedVideo", handleFinished, handleSkipped, handleFinished);

                }
                else
                {
                    protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
                    net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_ONLINE_REWARD, _msg);
                }
            }
			else
			{
                protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
                net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_ONLINE_REWARD, _msg);
			}
		}
	}

	void IMessage.message (s_message message)
	{

	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_ONLINE_REWARD)
		{
			protocol.game.smsg_online_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_online_reward> (message.m_byte);
			for(int i =0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			s_t_online_reward t_or = game_data._instance.get_t_online_reward (sys._instance.m_self.m_t_player.online_reward_index);
			for (int i = 0; i < _msg.types.Count; ++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg.value1s[i],_msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("online_reward_gui.cs_155_101"));//在线奖励领取
			}
			sys._instance.m_self.m_t_player.online_reward_index++;
			t_or = game_data._instance.get_t_online_reward (sys._instance.m_self.m_t_player.online_reward_index);
			if (t_or == null)
			{

			}
			else
			{
				sys._instance.m_self.m_t_player.online_reward_time = timer.now() + (ulong)t_or.time;
			}
            reset();

			s_message msg = new s_message();
			msg.m_type = "deal_main_gui";
			cmessage_center._instance.add_message(msg);
		}
	}

	void do_time()
	{
		ulong dm = 0;
		ulong now = timer.now ();
		if (now <= sys._instance.m_self.m_t_player.online_reward_time)
		{
			dm = sys._instance.m_self.m_t_player.online_reward_time - now;
		}
		if (!m_flag && dm <= 0)
		{
			m_flag = true;
			reset();
		}
		string _show = timer.get_time_show ((long)dm);
		m_time.transform.GetComponent<UILabel>().text = _show;
	}

	// Update is called once per frame
	void Update () {
		do_time ();
	}

}
