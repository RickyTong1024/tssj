using UnityEngine;

enum e_guild_member_type
{
	e_member_type_leader	= 0,	// 军长
	e_member_type_senator	= 1,	// 副军长
	e_member_type_common	= 2		// 普通成员
};
public class juntuan_member_details : MonoBehaviour,IMessage {

	public GameObject m_guild_shengzhi;
	public GameObject m_guild_jiangzhi;
	public GameObject m_guild_qingchu;
    public int index;
	int duty = 3;


	public UILabel m_shengzhi;
	public UILabel m_jiangzhi;
	public UILabel m_qingchu;
	void Start () 
	{
		m_shengzhi.text = game_data._instance.get_t_language ("juntuan_member_details.cs_23_20");//升职
		m_jiangzhi.text = game_data._instance.get_t_language ("juntuan_member_details.cs_24_20");//降职
		m_qingchu.text = game_data._instance.get_t_language ("juntuan_member_details.cs_25_19");//请出
        cmessage_center._instance.add_handle(this);
	}
	void OnEnable()
	{
		if (juntuan_gui._instance.m_zhiwu_t == 0) 
		{

			m_guild_shengzhi.GetComponent<UISprite>().set_enable(true);
			m_guild_shengzhi.GetComponent<BoxCollider>().enabled = true;
			m_guild_jiangzhi.GetComponent<UISprite>().set_enable(true);
			m_guild_jiangzhi .GetComponent<BoxCollider>().enabled = true;
			m_guild_qingchu.GetComponent<UISprite>().set_enable(true);
			m_guild_qingchu.GetComponent<BoxCollider>().enabled = true;
		} 
		else
		{
			m_guild_shengzhi.GetComponent<UISprite>().set_enable(false);
			m_guild_shengzhi.GetComponent<BoxCollider>().enabled = false;
			m_guild_jiangzhi.GetComponent<UISprite>().set_enable(false);
			m_guild_jiangzhi .GetComponent<BoxCollider>().enabled = false;
			m_guild_qingchu.GetComponent<UISprite>().set_enable(true);
			m_guild_qingchu.GetComponent<BoxCollider>().enabled = true;
		}

						
	}
	public void click (GameObject obj)
	{
		switch (obj.name) 
		{
		case "guild_shengzhi":
			s_message message = new s_message ();
			message.m_type = "guild_shengzhi";
			if(juntuan_gui._instance.m_guild_member_t[index].zhiwu - 1 == 0)
			{
				root_gui._instance.show_select_dialog_box (game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("juntuan_member_details.cs_61_53"), message);//提示//你将升他为总军团长（你将降为副军团长），是否继续?
			}
			else
			{
				root_gui._instance.show_select_dialog_box (game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("juntuan_member_details.cs_65_53"), message);//提示//您将升他的职位，是否继续?
			}

			break;
		case "guild_jiangzhi":
			s_message message1 = new s_message ();
			message1.m_type = "guild_jiangzhi";
			root_gui._instance.show_select_dialog_box (game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("juntuan_member_details.cs_72_52"), message1);//提示//您将降他的职位，是否继续?
			break;
		case "guild_qingchu":
			s_message message2 = new s_message ();
			message2.m_type = "guild_qingchu";
			root_gui._instance.show_select_dialog_box (game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("juntuan_member_details.cs_77_52"), message2);//提示//您将把他请出军团，是否继续?
			break;
		}

	}

    void IMessage.message(s_message message)
    {
		protocol.game.cmsg_guild_change_member_duty _msg = new protocol.game.cmsg_guild_change_member_duty();

		switch (message.m_type) 
		{
		case "guild_shengzhi":
			_msg.member_guid = juntuan_gui._instance.m_guild_member_t[index].player_guid;
			_msg.new_duty = juntuan_gui._instance.m_guild_member_t[index].zhiwu - 1;
			net_http._instance.send_msg<protocol.game.cmsg_guild_change_member_duty> (opclient_t.CMSG_GUILD_CHANGE_MEMBER_DUTY, _msg);
			if(_msg.new_duty == 0)
			{
				duty = 100;
			}
			else
			{
				duty = 0;
			}
			break;
		case "guild_jiangzhi":
			if (juntuan_gui._instance.m_guild_member_t[index].zhiwu != 2 )
			{
				_msg.member_guid = juntuan_gui._instance.m_guild_member_t[index].player_guid;
				_msg.new_duty = juntuan_gui._instance.m_guild_member_t[index].zhiwu + 1;
				net_http._instance.send_msg<protocol.game.cmsg_guild_change_member_duty> (opclient_t.CMSG_GUILD_CHANGE_MEMBER_DUTY, _msg);
			}
			else
			{
				root_gui._instance.show_prompt_dialog_box ("[ffc882]" + game_data._instance.get_t_language ("juntuan_member_details.cs_111_60"));//已是普通成员不能降职
			}
			break;
		case "guild_qingchu":
			protocol.game.cmsg_guild_kick_member _msg1 = new protocol.game.cmsg_guild_kick_member();
			_msg1.member_guid = juntuan_gui._instance.m_guild_member_t[index].player_guid;
			net_http._instance.send_msg<protocol.game.cmsg_guild_kick_member> (opclient_t.CMSG_GUILD_KICK_MEMBER, _msg1);
			break;
		}

    }
    void IMessage.net_message(s_net_message message)
    {
		if (message.m_opcode == opclient_t.CMSG_GUILD_CHANGE_MEMBER_DUTY)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if (_msg.success) 
			{
				if (duty == 100)
				{
					juntuan_gui._instance.m_zhiwu_t = 1;
					juntuan_gui._instance.m_guild_t.leader_name = juntuan_gui._instance.m_guild_member_t[index].player_name;
				}
				protocol.game.cmsg_common _msg1 = new protocol.game.cmsg_common ();
				net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_MEMBER_VIEW, _msg1);
		        juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().m_juntuan.GetComponent<juntuan_info>().refresh ();
		        this.transform.Find("frame_big").GetComponent<frame>().hide ();
			}
		}
		if (message.m_opcode == opclient_t.CMSG_GUILD_KICK_MEMBER)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if (_msg.success)
			{
				protocol.game.cmsg_common _msg1 = new protocol.game.cmsg_common ();
				net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_MEMBER_VIEW, _msg1);
				juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().m_juntuan.GetComponent<juntuan_info>().set_number_count ();
                this.transform.Find("frame_big").GetComponent<frame>().hide();
			}
		}

    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
}
