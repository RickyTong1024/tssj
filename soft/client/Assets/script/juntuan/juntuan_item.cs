using UnityEngine;

public class juntuan_item : MonoBehaviour,IMessage {
	public GameObject m_tubiao;
	public GameObject m_mingchen;
	public GameObject m_level;
	public GameObject m_number;
	public GameObject m_join;
    public GameObject m_apply;
	public ulong m_guild;
	public GameObject m_jifen;
    public int rank = 0;
    dhc.guild_t m_guildt;
	public dhc.guild_t m_guilt
	{
		set
		{
            m_guildt = value;
			m_guild = value.guid;
			m_jifen.GetComponent<UILabel>().text = value.honor.ToString();
			this.transform.Find("guildicon").GetComponent<UISprite>().spriteName = juntuan_gui._instance.setKuang(value.level);
			m_tubiao.GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon(value.icon).icon;
			m_mingchen.GetComponent<UILabel>().text = value.name;
			m_level.GetComponent<UILabel>().text = value.level.ToString();
            m_number.GetComponent<UILabel>().text = value.member_guids.Count.ToString() + "/" + game_data._instance.get_guild(value.level).number_count.ToString();
		}
	}
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
		UIEventListener.Get (m_join).onClick = ButtonClick;
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.message (s_message message)
	{
		
	}
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GUILD_JOIN) 
		{
			protocol.game.smsg_guild_data _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_data> (message.m_byte);
			juntuan_gui._instance.m_guild_t = _msg.guild;
			juntuan_gui._instance.m_zhiwu_t = _msg.zhiwu;
			juntuan_gui._instance.m_juntuan_open.SetActive (false);
			juntuan_gui._instance.m_juntuan_main.SetActive (true);
			juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().refresh ("juntuan");
		}
       

	}
	
	void ButtonClick(GameObject obj)
	{
		switch(obj.name)
		{
			case "join":
                if (m_guildt.member_guids.Count >= game_data._instance.get_guild(m_guildt.level).number_count)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("juntuan_item.cs_63_75"));//该军团人数已达上限
                    return;
                }
                s_message _mes = new s_message();
                _mes.m_type = "guild_apply";
                _mes.m_object.Add(m_guild);
                cmessage_center._instance.add_message(_mes);
                
           
			break;
		}
	}
}
