
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class vip_panel : MonoBehaviour, IMessage {
	
	int m_page = 0;
	public GameObject m_desc;
	public List<GameObject> m_descs;
	public List<GameObject> m_icons;
	public GameObject m_now_price;
	public GameObject m_old_price;
	public GameObject m_scr;
	public GameObject m_gq;
    
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void reset(int page)
	{
		transform.Find("reward").gameObject.SetActive(true);
		m_gq.SetActive (false);
		if (page == 0)
		{
			page = 1;
		}
		m_page = page;
		m_scr.transform.localPosition = Vector3.zero;
		m_scr.GetComponent<UIPanel>().clipOffset = Vector2.zero;
		s_t_vip t_vip = game_data._instance.get_t_vip (m_page);
		m_now_price.GetComponent<UILabel>().text = t_vip.jewel.ToString();
		m_old_price.GetComponent<UILabel>().text = t_vip.yjewel.ToString();
		m_desc.GetComponent<UILabel>().text = t_vip.recharge + "";
        if (game_data._instance.m_language != e_language.English)
        {
			m_scr.transform.Find("Label").GetComponent<UILabel>().text = (t_vip.desc1).Replace("|","\n");
        }
        else
        {
            m_scr.transform.Find("Label").GetComponent<UILabel>().text = t_vip.desc1;
        }
		
		transform.Find("vip_num2").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("vip_panel.cs_76_81"), t_vip.desc);//VIP{0}特权介绍
		transform.Find("vip_num1").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("vip_panel.cs_77_81"), t_vip.desc);//达到VIP{0}即可购买
		transform.Find("page").GetComponent<UILabel>().text 
			= m_page.ToString() + "/" + (game_data._instance.m_dbc_vip.get_y() - 1).ToString();

		bool vis = true;
		if (sys._instance.m_self.m_t_player.vip < m_page)
		{
			vis = false;
		}
		for (int i = 0; i < sys._instance.m_self.m_t_player.vip_reward_ids.Count; ++i)
		{
			if (sys._instance.m_self.m_t_player.vip_reward_ids[i] == m_page)
			{
				transform.Find("reward").gameObject.SetActive(false);
				m_gq.SetActive (true);
			}
		}
		transform.Find("reward").GetComponent<BoxCollider>().enabled = vis;
		if (vis)
		{
			transform.Find("reward").GetComponent<UISprite>().set_enable(true);
		}
		else
		{
			transform.Find("reward").GetComponent<UISprite>().set_enable(false);
		}

		for (int i = 0; i < m_icons.Count && i < t_vip.rewards.Count ; ++i)
		{
			sys._instance.remove_child (m_icons[i].gameObject);
			if (t_vip.rewards[i].type > 0)
			{
				GameObject icon1 = icon_manager._instance.create_reward_icon(t_vip.rewards[i].type, t_vip.rewards[i].value1, t_vip.rewards[i].value2, t_vip.rewards[i].value3);
				icon1.transform.parent = m_icons[i].transform;
				icon1.transform.localPosition = new Vector3(0,0,0);
				icon1.transform.localScale = new Vector3(1,1,1);
				m_icons[i].gameObject.SetActive(true);
			}
			else
			{
				m_icons[i].gameObject.SetActive(false);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void click(GameObject obj)
	{
		if (obj.name == "left")
		{
			m_page--;
			if (m_page <= 0)
			{
				m_page = game_data._instance.m_dbc_vip.get_y() - 1;
			}
			reset(m_page);
		}
		else if (obj.name == "right")
		{
			m_page++;
			if (m_page >= game_data._instance.m_dbc_vip.get_y())
			{
				m_page = 1;
			}
			reset(m_page);
		}
		else if (obj.name == "reward")
		{
			s_t_vip t_vip = game_data._instance.get_t_vip (m_page);
			if (t_vip.jewel > sys._instance.m_self.get_att(e_player_attr.player_jewel))
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
				return;
			}
			string _des = string.Format(game_data._instance.get_t_language ("vip_panel.cs_166_31") , t_vip.jewel//是否花费[00ffff]{0}钻石[-]购买VIP{1}超值礼包[-]?
				, m_page.ToString());
			s_message _msg = new s_message();
			_msg.m_type = "buy_vip_libao";
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
		}
	}

	void IMessage.message (s_message message)
	{
		if(message.m_type == "buy_vip_libao")
		{
			protocol.game.cmsg_vip_reward _msg = new protocol.game.cmsg_vip_reward ();
			_msg.vip = m_page;
			net_http._instance.send_msg<protocol.game.cmsg_vip_reward> (opclient_t.CMSG_VIP_REWARD, _msg);
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_VIP_REWARD)
		{
			s_t_vip t_vip = game_data._instance.get_t_vip(m_page);
			sys._instance.m_self.sub_att(e_player_attr.player_jewel,t_vip.jewel,game_data._instance.get_t_language ("vip_panel.cs_189_71"));//购买vip礼包消耗

			protocol.game.smsg_vip_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_vip_reward> (message.m_byte);
			for (int i = 0; i < _msg.types.Count; ++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg.value1s[i],_msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("vip_panel.cs_194_101"));//vip奖励获得
			}
			for(int i = 0; i < _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for (int i = 0; i < _msg.equips.Count; ++i)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			for (int i = 0; i < _msg.roles.Count; ++i)
			{
				sys._instance.m_self.add_card(_msg.roles[i]);
			}
			sys._instance.m_self.m_t_player.vip_reward_ids.Add(m_page);
			reset(m_page);
		}
	}
}
