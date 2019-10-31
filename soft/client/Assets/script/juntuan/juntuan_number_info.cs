using UnityEngine;

public class juntuan_number_info : MonoBehaviour,IMessage {
	public GameObject juntuan_number_items;
    public GameObject member_details;
    public GameObject m_tanhe;
    public int m_index;


	public UILabel m_juntuanchengyuan;
	void Start () 
	{
		m_juntuanchengyuan.text = game_data._instance.get_t_language ("juntuan_number_info.cs_14_28");//军团成员列表
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void OnEnable()
	{
		juntuan_number_items.GetComponent<UIScrollView>().ResetPosition ();
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_MEMBER_VIEW, _msg);
	}

	void IMessage.message (s_message message)
	{
        if (message.m_type == "tanheleader")
        {
            if (juntuan_gui._instance.m_zhiwu_t == 1)
            {
                if (sys._instance.m_self.m_t_player.jewel < 200)
                {
                    root_gui._instance.show_recharge_dialog_box(
                        delegate()
                        {
                            m_tanhe.transform.Find("frame_big").GetComponent<frame>().hide();
                        }
                        );
                            return;
                }
            }
            else if (juntuan_gui._instance.m_zhiwu_t == 2)
            {
                if (sys._instance.m_self.m_t_player.jewel < 2000)
                {
                    root_gui._instance.show_recharge_dialog_box(
                        delegate()
                        {
                            m_tanhe.transform.Find("frame_big").GetComponent<frame>().hide();
                        }
                        );
                    return;
                }
 
            }
            protocol.game.cmsg_guild_kick_member _msg = new protocol.game.cmsg_guild_kick_member();
            _msg.member_guid = juntuan_gui._instance.m_guild_member_t[0].player_guid;
            net_http._instance.send_msg<protocol.game.cmsg_guild_kick_member>(opclient_t.CMSG_GUILD_TANHE, _msg);
        }
      
	}
	void IMessage.net_message(s_net_message message)
	{
        if (message.m_opcode == opclient_t.CMSG_GUILD_MEMBER_VIEW)
        {
            protocol.game.smsg_guild_member_view _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_member_view>(message.m_byte);
            juntuan_gui._instance.m_guild_member_t = _msg.guild_members;
            update_ui();
        }
        else if (message.m_opcode == opclient_t.CMSG_GUILD_TANHE)
        {
            if (juntuan_gui._instance.m_zhiwu_t == 1)
            {
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, 200,game_data._instance.get_t_language ("juntuan_number_info.cs_77_77"));//弹劾军团长
            }
            else if (juntuan_gui._instance.m_zhiwu_t == 2)
            {
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, 2000, game_data._instance.get_t_language ("juntuan_number_info.cs_77_77"));//弹劾军团长
 
            }
            m_tanhe.transform.Find("frame_big").GetComponent<frame>().hide();
            juntuan_gui._instance.m_zhiwu_t = 0;
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_MEMBER_VIEW, _msg);
        }
      

	}
	public void update_ui()
	{
		sys._instance.remove_child (juntuan_number_items);
		juntuan_number_items.transform.localPosition = new Vector3(0, 0, 0);
		juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		for (int i = 0; i < juntuan_gui._instance.m_guild_member_t.Count; i++)
		{
			GameObject target = game_data._instance.ins_object_res("ui/juntuan/juntuan_number_item");
            target.GetComponent<juntuan_number_item>().member_index = i;
			target.GetComponent<juntuan_number_item>().member = juntuan_gui._instance.m_guild_member_t[i];
            target.GetComponent<juntuan_number_item>().m_tanhe = m_tanhe;
            target.GetComponent<juntuan_number_item>().member_detail = member_details;
			target.transform.parent = juntuan_number_items.transform;
			target.transform.localPosition = new Vector3(0,150 - i * 120,0);
			target.transform.localScale = Vector3.one;
		}

	}
	public void click(GameObject obj)
	{
		switch (obj.name)
		{
			
		case "icon":
			break;
		case "jiesan":

			break;
        case "guild_tanhe":
            ulong time = juntuan_gui._instance.m_guild_member_t[0].offline_time;
            if ((timer.dtnow() - timer.time2dt(time)).TotalDays < 7)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("juntuan_number_info.cs_124_71"));//军团长连续7天未上线后，军团成员可以弹劾
                return;

            }
            else
            {
                if (juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_senator)
                {
                    s_message _mes = new s_message();
                    _mes.m_type = "tanheleader";
                    root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("juntuan_number_info.cs_134_68"), _mes);//提示// 军团长已超过7天未上线，是否花费200钻石弹劾他使自己成为新的军团长
                }
                else if (juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_common)
                {
                    s_message _mes = new s_message();
                    _mes.m_type = "tanheleader";
					root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("juntuan_number_info.cs_140_53"), _mes);//提示// 军团长已超过7天未上线，是否花费2000钻石弹劾他使自己成为新的军团长
                }

            }
            break;
		}
		
	}
}
