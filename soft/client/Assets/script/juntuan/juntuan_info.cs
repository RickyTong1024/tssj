using UnityEngine;

public class juntuan_info : MonoBehaviour,IMessage {
	public GameObject m_juntuan_icon;
	public GameObject m_juntuan_name;
	public GameObject m_juntuan_jifen;
	public GameObject m_juntuan_tuanzhang;
	public GameObject m_juntuan_level;
	public GameObject m_juntuan_number;
	public GameObject m_juntuan_log;
	public GameObject m_juntuan_jiesan;
	public GameObject m_juntuan_tuichu;
	public GameObject m_juntuan_edit_jiarutiaojian;
	public GameObject m_juntuan_gonggao;
	public GameObject m_juntuan_gonggao_edit;
	public GameObject m_juntuan_gonggao_edit_panel;
    public GameObject m_log_scrollview;
	public UILabel m_show_gonggao;
	public GameObject m_juntuan_edit_icon;
    public GameObject m_apply;
	public UIToggle m_mem_button;
    public GameObject m_bianggeng_gui;
    public GameObject m_change_name_gui;
    public GameObject m_edit;
    public UILabel m_changge_name;
    public UILabel m_change_guild_jewel;

	public UILabel m_juntuanxinxi;
	public UILabel m_juntuanzhang;
	public UILabel m_juntuandengji;
	public UILabel m_juntuanrenshu;
	public UILabel m_juntuanjingyan;
	public UILabel m_jiarutiaojian;
	public UILabel m_biangeng;
	public UILabel m_juntuanrizhi;
	public UILabel m_juntuangonggao;
	public UILabel m_bianji;
	public UILabel m_jiesan;
	public UILabel m_tuichu;
	public UILabel m_fabugonggao;
	public UILabel m_queding;

	void Start () 
	{
		cmessage_center._instance.add_handle (this);
		UIEventListener.Get (m_juntuan_gonggao_edit).onClick = ButtonClick;
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void ButtonClick(GameObject obj)
	{
		s_message message = new s_message();
		switch (obj.name)
		{
		case "juntuan_edit":
                if (platform_config_common.game_model == 2)
                {
                    m_juntuan_gonggao_edit_panel.SetActive(false);
                }
                else
                {
                    m_juntuan_gonggao_edit_panel.SetActive(true);
                }
                m_juntuan_gonggao.GetComponent<UIInput>().value = juntuan_gui._instance.set_juntuan_notice();
                break;
		case "juntuan_edit_sure":
			if( m_juntuan_gonggao.GetComponent<UIInput>().label.text == "")
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("juntuan_info.cs_64_59")+"。");//名字不能为空
				return;
			}
			protocol.game.cmsg_guild_modify_bulletin _msg = new protocol.game.cmsg_guild_modify_bulletin();
			_msg.bulletin = m_juntuan_gonggao.GetComponent<UIInput>().label.text;
			net_http._instance.send_msg<protocol.game.cmsg_guild_modify_bulletin> (opclient_t.CMSG_GUILD_MODIFY_BULLETIN, _msg);
			break;
		}

	}
   
	void IMessage.message (s_message message)
	{
		if (message.m_type == "jiesan")
		{
            if (juntuan_gui._instance.m_guild_member_t.Count < 10)
            {
                protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
                net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_DISMISS, _msg);
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("juntuan_info.cs_86_71"));//军团成员数量少于10人才可以解散军团
 
            }
		
		}
		else if (message.m_type == "tuichu") 
		{
			if(juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_leader)
			{
				m_mem_button.value = true;
				juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().target_active ("members");
				root_gui._instance.show_prompt_dialog_box ("[ffc882]" + game_data._instance.get_t_language ("juntuan_info.cs_88_60"));//请先任免总军团长
				return;
			}
			else
			{
				protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
				net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GUILD_LEAVE, _msg);
			}
		}
        else if (message.m_type == "tanheleader")
        {
 
        }
        
	}
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GUILD_DISMISS)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			sys._instance.m_self.m_t_player.guild = 0;
			juntuan_gui._instance.m_juntuan_main.gameObject.SetActive (false);
			juntuan_gui._instance.m_juntuan_main.transform.parent.gameObject.SetActive (false);
			s_message _message = new s_message ();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message (_message);

		}
		if (message.m_opcode == opclient_t.CMSG_GUILD_MODIFY_BULLETIN)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if (_msg.success)
			{
			    juntuan_gui._instance.m_guild_t.gonggao = m_juntuan_gonggao.GetComponent<UIInput>().label.text;
				refresh();
			}

		}
		if (message.m_opcode == opclient_t.CMSG_GUILD_LEAVE) 
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if(_msg.success)
			{
				sys._instance.m_self.m_t_player.guild = 0;
				juntuan_gui._instance.m_juntuan_main.gameObject.SetActive (false);
				juntuan_gui._instance.m_juntuan_main.transform.parent.gameObject.SetActive (false);
				s_message _message = new s_message ();
				_message.m_type = "show_main_gui";
				cmessage_center._instance.add_message (_message);
			}
			
		}
        else if (message.m_opcode == opclient_t.CMSG_GUILD_MODIFY_GUILD_NAME)
        {
            juntuan_gui._instance.m_guild_t.name = m_changge_name.text;
            sys._instance.m_self.sub_att(e_player_attr.player_jewel,game_data._instance.get_t_price
                (++juntuan_gui._instance.m_guild_t.change_name).change_name,game_data._instance.get_t_language ("juntuan_info.cs_144_76"));//军团改名字消耗

            refresh();
 
        }
	}
	public void set_number_count()
	{
		m_juntuan_number.GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_t.member_guids.Count - 1 + "/"+ game_data._instance.get_guild(juntuan_gui._instance.m_guild_t.level).number_count;
	}
	public void refresh()
	{
		m_juntuan_icon.GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon (juntuan_gui._instance.m_guild_t.icon).icon;
		this.gameObject.transform.Find("back/info1/catch").GetComponent<UISprite>().spriteName = juntuan_gui._instance.setKuang (juntuan_gui._instance.m_guild_t.level);
		m_juntuan_name.GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_t.name;
		m_juntuan_level.GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_t.level.ToString();
		m_juntuan_number.GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_t.member_guids.Count + "/" + game_data._instance.get_guild(juntuan_gui._instance.m_guild_t.level).number_count;
        //m_juntuan_jifen.GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_t.honor.ToString();
        
        m_show_gonggao.text = juntuan_gui._instance.set_juntuan_notice();

        m_juntuan_tuanzhang.GetComponent<UILabel>().text = juntuan_gui._instance.m_guild_t.leader_name;
        sys._instance.remove_child(m_log_scrollview);
        GameObject obj = null;
		for (int i = juntuan_gui._instance.m_guild_log.Count - 1; i >= 0; i--) 
		{
            dhc.guild_event_t temp = juntuan_gui._instance.m_guild_log[i];
            GameObject log = game_data._instance.ins_object_res("ui/juntuan/guild_log");
            log.transform.parent = m_log_scrollview.transform;
           
            log.transform.localScale = Vector3.one;
            string s = "[" + timer.time2dt(temp.time).ToString("MM-dd HH:mm") + "]";
            log.GetComponent<UILabel>().text = s;
            string value = "";
			if (temp.type == 0)//加入军团
			{
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_178_26"), temp.player_name );//成员[00ff00]{0}[-]加入了军团
			}
			else if (temp.type == 1)//初级签到
			{
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_182_26"), temp.player_name , temp.value );//成员[00ff00]{0}[-]进行了[0BBBF5]初级膜拜[-]为军团增加了[ffA000]{1}[-]点经验
			}
			else if (temp.type == 2)//中级签到
			{
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_186_26") , temp.player_name , temp.value );//成员[00ff00]{0}[-]进行了[F8891F]中级膜拜[-]为军团增加了[ffA000]{1}[-]点经验
			}
			else if (temp.type == 3)//高级签到
			{
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_190_26"), temp.player_name , temp.value);//成员[00ff00]{0}[-]进行了[F7D31E]高级膜拜[-]为军团增加了[ffA000]{1}[-]点经验
			}
			else if (temp.type == 4)//退出军团
			{
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_194_26"), temp.player_name );//成员[00ff00]{0}[-]退出了军团
			}
			else if (temp.type == 5)//踢出军团
			{
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_198_26"),temp.player_name);//成员[00ff00]{0}[-]被移出了军团
			}
			else if (temp.type == 6)//任命副军团长
			{
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_202_26") , temp.create_name , temp.player_name );//军团长[00ff00]{0}[-]任命了[00ff00]{1}[-]为副军团长
			}
            else if (temp.type == 7)//解除副军团长
            {
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_206_26"), temp.create_name , temp.player_name );//军团长[00ff00]{0}[-]解除了[00ff00][-]{1}副军团长职务
            }
            else if (temp.type == 8)//职务转让
            {
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_210_26") , temp.create_name , temp.player_name );//军团长[00ff00]{0}[-]把职务让位给了[00ff00]{1}[-]，让我们欢迎新的军团长吧
            }
            else if (temp.type == 9)//军团升级
            {
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_214_26") , temp.value );//可喜可贺，军团升到了[ffA000]{0}[-]级
            }
            else if (temp.type == 10)//击杀守卫
            {
				value = string.Format(game_data._instance.get_t_language ("juntuan_info.cs_218_26") , get_name(temp.value) , temp.value1 );//众队员齐心协力，击杀了[00ff00]{0}[-]为军团增加了[ffA000]{1}[-]点经验
            }
            else if (temp.type == 11)//弹劾军团长
            {
				value = get_zhiwu(temp.value) + string.Format(game_data._instance.get_t_language ("juntuan_info.cs_222_50") , temp.player_name , temp.create_name );//[00ff00]{0}[-]弹劾了许久未上线的军团长[00ff00]{1}[-]成为了新的军团长！
            }
            log.transform.Find("Label").GetComponent<UILabel>().text = value + "";
			log.transform.Find("Label").GetComponent<UILabel>().ProcessText();
            if (obj != null)
            {
                log.transform.localPosition = new Vector3(-184, obj.transform.localPosition.y - obj.transform.Find("Label").GetComponent<UILabel>().height - 10f, 0);
            }
            else
            {
                log.transform.localPosition = new Vector3(-184, 130 - 42f * (juntuan_gui._instance.m_guild_log.Count - i - 1), 0);
            }
            obj = log;
		}
       
        m_juntuan_gonggao_edit.SetActive(bt_setting());
		if (juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_leader)
		{
            m_juntuan_gonggao_edit.SetActive(bt_setting());
            m_juntuan_jiesan.SetActive (true);
			m_juntuan_tuichu.SetActive (true);
            m_apply.SetActive(true);

			m_juntuan_tuichu.transform.localPosition = new Vector3(130f,-215,0);
            m_juntuan_tuichu.GetComponent<UISprite>().width = 127;

            m_juntuan_jiesan.transform.localPosition = new Vector3(-130f, -215, 0);
            m_juntuan_jiesan.GetComponent<UISprite>().width = 127;

            m_apply.transform.localPosition = new Vector3(0, -215, 0);
            m_apply.GetComponent<UISprite>().width = 127;
		}
		else if(juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_common)
		{
            m_juntuan_gonggao_edit.SetActive(false);
            m_juntuan_jiesan.SetActive (false);
			m_juntuan_tuichu.SetActive (true);
            m_apply.SetActive(false);
            m_juntuan_tuichu.transform.localPosition = new Vector3(0, -215, 0);
            m_juntuan_tuichu.GetComponent<UISprite>().width = 388;
		}
		else if(juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_senator)
		{
            m_juntuan_gonggao_edit.SetActive(bt_setting());
            m_juntuan_jiesan.SetActive (false);
			m_juntuan_tuichu.SetActive (true);
            m_apply.SetActive(true);
            m_juntuan_tuichu.transform.localPosition = new Vector3(97, -215, 0);
            m_juntuan_tuichu.GetComponent<UISprite>().width = 190;

            m_apply.transform.localPosition = new Vector3(-97, -214, 0);
            m_apply.GetComponent<UISprite>().width = 190;

		}
        if (juntuan_gui._instance.m_guild_t.apply_guids.Count != 0)
        {
            m_apply.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_apply.transform.Find("effect").gameObject.SetActive(false);
 
        }
		
	}
    string get_zhiwu(int id)
    {
        if (id == 0)
        {
            return game_data._instance.get_t_language ("juntuan_info.cs_291_19");//军团长
        }
        else if (id == 1)
        {
            return game_data._instance.get_t_language ("juntuan_info.cs_295_19");//副军团长
        }
        else
        {
            return game_data._instance.get_t_language ("juntuan_info.cs_299_19");//成员
        }
    }
    string get_name(int m_id)
    {
        string s = "";
        switch (m_id)
        {
            case 1:
                s  = game_data._instance.get_t_language ("guildboss_task_gui.cs_286_10");//坚苍守卫
                break;
            case 2:
                s = game_data._instance.get_t_language ("guildboss_task_gui.cs_289_10");//韧战守卫
                break;
            case 3:
                s = game_data._instance.get_t_language ("guildboss_task_gui.cs_292_10");//迅霆守卫
                break;
            case 4:
                s = game_data._instance.get_t_language ("guildboss_task_gui.cs_295_10");//布洛守卫
                break;
        }
        return s;
    }

    bool bt_setting()
    {
        if (platform_config_common.game_model == 2)
        {
            return false;
        }
        else
        {

            return true;
        }
    }
	public void click(GameObject obj)
	{
		s_message message = new s_message();

		switch (obj.name) 
		{
		case "changeguildicon":
            if (juntuan_gui._instance.m_zhiwu_t != (int)e_guild_member_type.e_member_type_leader)
			{
               
				return;
			}
            m_bianggeng_gui.transform.Find("frame_big").GetComponent<frame>().hide();
			m_juntuan_edit_icon.SetActive (true);
			break;
		case "jiesan":
			message = new s_message();
			message.m_type = "jiesan";
			root_gui._instance.show_select_dialog_box (game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("juntuan_info.cs_340_52"), message);//提示//您将解散军团，是否继续?

			break;
		case "tuichu":
		    message = new s_message();
			message.m_type = "tuichu";
			root_gui._instance.show_select_dialog_box (game_data._instance.get_t_language ("arena.cs_104_45"), game_data._instance.get_t_language ("juntuan_info.cs_346_52"), message);//提示//您将退出军团，是否继续?
			break;
		case "juntuan_biangengtiaojian":
			m_juntuan_edit_jiarutiaojian.SetActive(true);
			break;
		case "juntuan_gonggao":

			break;
		case "juntuan_edit_sure":
			protocol.game.cmsg_guild_modify_bulletin _msg = new protocol.game.cmsg_guild_modify_bulletin();
			_msg.bulletin = m_juntuan_gonggao.GetComponent<UIInput>().label.text;
			net_http._instance.send_msg<protocol.game.cmsg_guild_modify_bulletin> (opclient_t.CMSG_GUILD_MODIFY_BULLETIN, _msg);
            m_juntuan_gonggao_edit_panel.transform.Find("frame_big").GetComponent<frame>().hide();
			break;
		case "icon_juntuan":
			break;
		case "hide":
			m_juntuan_gonggao_edit_panel.transform.Find("frame_big").GetComponent<frame>().hide();

			break;
        case "change":
            m_bianggeng_gui.SetActive(true);
            break;
        case "changeguildname":
            m_bianggeng_gui.transform.Find("frame_big").GetComponent<frame>().hide();
            m_change_name_gui.SetActive(true);
            m_change_guild_jewel.text = "x" + game_data._instance.get_t_price(juntuan_gui._instance.m_guild_t.change_name + 1).change_name + "";
            m_changge_name.GetComponent<UIInput>().value = "";
            break;
            case "change_guildname":
            if (m_changge_name.GetComponent<UIInput>().value == "")
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("juntuan_info.cs_64_59"));//名字不能为空
                return;
            }
            
            if (sys._instance.m_self.get_att(e_player_attr.player_jewel) >= game_data._instance.get_t_price(juntuan_gui._instance.m_guild_t.change_name + 1).change_name)
            {
              
                {
                    protocol.game.cmsg_guild_modify_bulletin _msg1 = new protocol.game.cmsg_guild_modify_bulletin();
                    _msg1.bulletin = m_changge_name.text;
                    net_http._instance.send_msg<protocol.game.cmsg_guild_modify_bulletin>(opclient_t.CMSG_GUILD_MODIFY_GUILD_NAME, _msg1);
                    m_change_name_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                }
                

            }
            else
            {
                root_gui._instance.show_recharge_dialog_box(delegate() { });
 
            }
               
            break;

		}

	}
}
