using UnityEngine;

public class juntuan_main : MonoBehaviour ,IMessage{
	public GameObject m_juntuan;
	public GameObject m_juntuan_number;
	public GameObject m_juntuan_paiming;
    public GameObject m_apply_gui;
    public GameObject m_view;
	public UIToggle m_juntuan_button;
	public UILabel m_juntuan_label;
	public UILabel m_chengyuan_label;
	public UILabel m_huodong_label;
	public UILabel m_paiming_label;
	public UILabel m_juntuan_label1;
	public UILabel m_chengyuan_label1;
	public UILabel m_huodong_label1;
	public UILabel m_paiming_label1;
	public UILabel m_wodegongxian;
    public ulong guid;

	void Start()
	{
        cmessage_center._instance.add_handle(this);
	}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void  IMessage.message(s_message message)
    {
 
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_GUILD_APPLY)
        {
           
        }
        else if(message.m_opcode == opclient_t.CMSG_GUILD_AGREE)
        {
            for (int i = 0; i < juntuan_gui._instance.m_guild_t.apply_guids.Count; i++)
            {
                if (guid == juntuan_gui._instance.m_guild_t.apply_guids[i])
                {
                    juntuan_gui._instance.m_guild_t.apply_guids.Remove(guid);
                    juntuan_gui._instance.m_guild_t.apply_achieve.RemoveAt(i);
                    juntuan_gui._instance.m_guild_t.apply_bf.RemoveAt(i);
                    juntuan_gui._instance.m_guild_t.apply_level.RemoveAt(i);
                    juntuan_gui._instance.m_guild_t.apply_names.RemoveAt(i);
                    juntuan_gui._instance.m_guild_t.apply_vip.RemoveAt(i);
                    juntuan_gui._instance.m_guild_t.apply_template.RemoveAt(i);
                    juntuan_gui._instance.m_guild_t.member_guids.Add(guid);
                    break;
                }
            }
            reset_apply();

        }


    }
	public void refresh(string type)
	{
		switch(type)
		{
			case "juntuan":
                if (!m_juntuan.activeSelf)
                {
                    m_juntuan.SetActive(true);
                }
				
				m_juntuan_number.SetActive (false);
				m_juntuan_paiming.SetActive (false);
				m_juntuan.GetComponent<juntuan_info>().refresh ();
				break;
			case "juntuan_number":
				target_active ("members");
			break;
		}

	}
	public void target_active(string t)
	{
		m_juntuan.SetActive (false);
		m_juntuan_number.SetActive (false);
		m_juntuan_paiming.SetActive (false);
		switch (t)
		{
		case "paiming":
			m_juntuan_paiming.SetActive (true);
			break;
		case "juntuan":
			m_juntuan.SetActive (true);
			break;
		case "huodong":
			break;
		case "members":
			m_juntuan_number.SetActive (true);
			break;
		}

	}
	public  void click(GameObject obj)
	{
		switch (obj.name)
		{
		case "juntuan":
			target_active ("juntuan");
				break;
		case "members":
			target_active ("members");
			break;
		case "huodong":
			target_active ("huodong");
			break;
		case "paiming":
			target_active ("paiming");
			break;
        case "apply":
            reset_apply();
            break;
		case "close":
			m_juntuan.SetActive (false);
			m_juntuan_number.SetActive (false);
			m_juntuan_paiming.SetActive (false);
			this.GetComponent<ui_show_anim>().hide_ui ();
			m_juntuan_button.value = true;
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message (_message);
			juntuan_gui._instance.gameObject.SetActive (false);
			break;
		}
	}
    void reset_apply()
    {
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        dhc.guild_t guild =  juntuan_gui._instance.m_guild_t;
        for (int i = 0; i < juntuan_gui._instance.m_guild_t.apply_guids.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/apply_item");
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(0,112 - i * 116,0);
            obj.transform.localScale = Vector3.one;
            GameObject _obj1 = icon_manager._instance.create_player_icon((int)guild.apply_template[i], guild.apply_achieve[i], guild.apply_vip[i],guild.apply_nalfags[i]);
            _obj1.transform.parent = obj.transform.Find("icon").transform;
            _obj1.transform.localScale = new Vector3(1, 1, 1);
            _obj1.transform.localPosition = new Vector3(0, 0, 0);
            obj.transform.Find("icon").GetComponent<UISprite>().spriteName = game_data._instance.get_t_class(guild.apply_template[i]).icon;
            obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(guild.apply_achieve[i]) + guild.apply_names[i];
            obj.transform.Find("level").GetComponent<UILabel>().text = guild.apply_level[i] + "";
            obj.transform.Find("bf").GetComponent<UILabel>().text = sys._instance.value_to_wan(guild.apply_bf[i]) + "";
            UIEventListener.Get(obj.transform.Find("agree").gameObject).onClick = appy_click;
            UIEventListener.Get(obj.transform.Find("jujue").gameObject).onClick = appy_click;
            obj.name = guild.apply_guids[i] + "";
        }
        m_apply_gui.SetActive(true);
 
    }
    public void appy_click(GameObject obj)
    {
        if (obj.name == "agree")
        {
            if (juntuan_gui._instance.m_guild_t.member_guids.Count >= game_data._instance.get_guild(juntuan_gui._instance.m_guild_t.level).number_count)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("juntuan_main.cs_169_71"));//军团人数已达上限
                return;
            }
            protocol.game.cmsg_guild_agree agree = new protocol.game.cmsg_guild_agree();
            agree.player_guid = ulong.Parse(obj.transform.parent.name);
            agree.argree = true;
            guid = agree.player_guid;
            net_http._instance.send_msg<protocol.game.cmsg_guild_agree>(opclient_t.CMSG_GUILD_AGREE, agree);

 
        }
        else if (obj.name == "jujue")
        {
            protocol.game.cmsg_guild_agree agree = new protocol.game.cmsg_guild_agree();
            agree.player_guid = ulong.Parse(obj.transform.parent.name); ;
            agree.argree = false;
            guid = ulong.Parse(obj.transform.parent.name); ;
            net_http._instance.send_msg<protocol.game.cmsg_guild_agree>(opclient_t.CMSG_GUILD_AGREE, agree);
        }
 
    }
}
