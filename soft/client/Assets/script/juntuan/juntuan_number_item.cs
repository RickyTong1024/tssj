using UnityEngine;

public class juntuan_number_item : MonoBehaviour,IMessage {
	public GameObject juntuan_number_icon;
	public GameObject juntuan_number_zhiwei;
	public GameObject juntuan_number_mingchen;
	public GameObject juntuan_number_level;
	public GameObject juntuan_number_zhandouli;
	public GameObject juntuan_number_qiangdao;
	public GameObject juntuan_number_chakan;
	public GameObject member_detail;
    public GameObject m_tanhe;
	public GameObject m_zaixian;
	public UILabel m_gongxian;
	public int member_index;

	public UILabel m_zhandouli;
	public UILabel m_benzhougongxian;
	public UILabel m_chakan;
   
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public dhc.guild_member_t member
	{
		set
		{
			if (value.player_guid == sys._instance.m_self.m_guid)
			{
				this.transform.GetComponent<UISprite>().spriteName = "jjc_ybxdb001a";
			}
            GameObject _obj1 = icon_manager._instance.create_player_icon((int)value.player_iocn_id, value.player_achieve, value.player_vip,value.nalflag);
            _obj1.transform.parent = juntuan_number_icon.transform;
            _obj1.transform.localScale = new Vector3(1, 1, 1);
            _obj1.transform.localPosition = new Vector3(0, 0, 0);
			juntuan_number_icon.GetComponent<UISprite>().spriteName = player.get_touxiang (value.player_iocn_id);
            juntuan_number_mingchen.GetComponent<UILabel>().text = game_data._instance.get_name_color(value.player_achieve) + value.player_name;
			juntuan_number_level.GetComponent<UILabel>().text = "[ffffff]" + value.player_level.ToString () + "[-]";

            juntuan_number_zhandouli.GetComponent<UILabel>().text = sys._instance.value_to_wan(value.bf).ToString();
			m_gongxian.text = "  " + value.contribution.ToString();
			int day = timer.run_day(value.last_sign_time);
			if(value.last_sign_time == 0)
			{
				juntuan_number_qiangdao.GetComponent<UILabel>().text = "[ff5000]" + game_data._instance.get_t_language ("juntuan_number_item.cs_51_72");//今天未膜拜
			}
			else
			{
				if (day > 10)
				{
					juntuan_number_qiangdao.GetComponent<UILabel>().text = "[ff5000]" + game_data._instance.get_t_language ("juntuan_number_item.cs_57_73");//很久未膜拜
				}

				else if (day > 1)
				{
					juntuan_number_qiangdao.GetComponent<UILabel>().text = "[ff5000]" + string.Format(game_data._instance.get_t_language ("juntuan_number_item.cs_62_87"),day.ToString());//{0}天未膜拜
				}

				else if (day == 1)
				{
					juntuan_number_qiangdao.GetComponent<UILabel>().text = "[ff5000]" + game_data._instance.get_t_language ("juntuan_number_item.cs_51_72");//今天未膜拜
				}
		    	else
				{
					if (value.sign_flag == 1)
					{
						juntuan_number_qiangdao.GetComponent<UILabel>().text = game_data._instance.get_t_language ("juntuan_number_item.cs_73_61");//[0BBBF5]初级膜拜
					}
					else if (value.sign_flag == 2)
					{
						juntuan_number_qiangdao.GetComponent<UILabel>().text = game_data._instance.get_t_language ("juntuan_number_item.cs_77_61");//[F8891F]中级膜拜
					}
					else
					{
						juntuan_number_qiangdao.GetComponent<UILabel>().text = game_data._instance.get_t_language ("juntuan_number_item.cs_81_61");//[F7D31E]高级膜拜
					}
				}

			}
			if (value.zhiwu == (int)e_guild_member_type.e_member_type_leader)
			{
				juntuan_number_zhiwei.GetComponent<UILabel>().text = game_data._instance.get_t_language ("juntuan_info.cs_291_19");//军团长
			}
			else if (value.zhiwu == (int)e_guild_member_type.e_member_type_senator)
			{
				juntuan_number_zhiwei.GetComponent<UILabel>().text = game_data._instance.get_t_language ("juntuan_info.cs_295_19");//副军团长
			}
			else
			{
				juntuan_number_zhiwei.GetComponent<UILabel>().text = game_data._instance.get_t_language ("juntuan_info.cs_299_19");//成员
			}

			if (timer.now() < value.offline_time + 300000)
			{
				m_zaixian.GetComponent<UILabel>().text = game_data._instance.get_t_language ("haoyou.cs_47_53");//[ffc864]在线
			}
			else
			{
				m_zaixian.GetComponent<UILabel>().text = "[ff5000]" + timer.get_guild_show(value.offline_time);
			}
		}
	}
	void IMessage.message (s_message message)
	{

		
	}
	void IMessage.net_message(s_net_message message)
	{

	}

	public void click(GameObject obj)
	{
		switch (obj.name)
        {
	        case "chakan":
		        protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look();
		        _msg.target_guid = juntuan_gui._instance.m_guild_member_t[member_index].player_guid;
		        net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);	
		        break;
	        case "juntuan_number_item(Clone)":
                if (juntuan_gui._instance.m_guild_member_t[member_index].zhiwu == 0 && juntuan_gui._instance.m_zhiwu_t != 0)
                {
                    m_tanhe.SetActive(true);
                    return;
                }
		        if(juntuan_gui._instance.m_guild_member_t[member_index].zhiwu != 0 && juntuan_gui._instance.m_zhiwu_t != 2 && juntuan_gui._instance.m_zhiwu_t < juntuan_gui._instance.m_guild_member_t[member_index].zhiwu)
		        {
			        member_detail.SetActive(true);
		            member_detail.GetComponent<juntuan_member_details>().index = member_index;
		        }
		        break;
           
		}
	}
}
