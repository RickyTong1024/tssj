using UnityEngine;

public class guild_chat : MonoBehaviour {
    public protocol.game.smsg_guild_message_view m_msg;
    public GameObject m_scrollow;
    public UIInput m_input;
    public GameObject m_chat_change;
    int id;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void reset()
    {
        if (platform_config_common.game_model == 2)
        {
            m_input.gameObject.SetActive(false);
            sys._instance.remove_child(m_scrollow);
            m_scrollow.transform.localPosition = new Vector3(0, 0, 0);
            m_scrollow.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_chat");
            obj.GetComponent<UILabel>().text = game_data._instance.get_t_language("guild_chat.cs_31_41");//(军团长)
            obj.transform.Find("desc").GetComponent<UILabel>().text = "[00A6FF]" + game_data._instance.get_t_language("juntuan_gui.cs_549_85") + "\n" + "[FFFFFF]";
            obj.transform.Find("time").gameObject.SetActive(false);
            obj.transform.Find("Label").gameObject.SetActive(false);
            obj.transform.parent = m_scrollow.transform;
            obj.transform.localPosition = new Vector3(-169, 198, 0);
            obj.transform.localScale = Vector3.one;
        }
        else
        {
            m_input.gameObject.SetActive(true);
            sys._instance.remove_child(m_scrollow);
            m_scrollow.transform.localPosition = new Vector3(0, 0, 0);
            m_scrollow.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
            for (int i = 0; i < m_msg.msgs.Count; i++)
            {
                string s = "";
                if (m_msg.msgs[i].zhiwu == 0)
                {
                    s = m_msg.msgs[i].name + game_data._instance.get_t_language("guild_chat.cs_31_41");//(军团长)
                }
                else if (m_msg.msgs[i].zhiwu == 1)
                {
                    s = m_msg.msgs[i].name + game_data._instance.get_t_language("guild_chat.cs_35_41");//(副军团长)
                }
                else if (m_msg.msgs[i].zhiwu == 2)
                {
                    s = m_msg.msgs[i].name + game_data._instance.get_t_language("guild_chat.cs_39_41");//(普通成员)
                }
                GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_chat");


                obj.GetComponent<UILabel>().text = s;
                obj.transform.Find("desc").GetComponent<UILabel>().text = "[00A6FF]" + m_msg.msgs[i].text + "\n" + "[FFFFFF]";
                obj.transform.Find("time").GetComponent<UILabel>().text = timer.get_show(m_msg.msgs[i].time).ToString();
                if (m_msg.msgs[i].otype == 1)
                {
                    obj.transform.Find("Label").gameObject.SetActive(true);
                }
                else
                {
                    obj.transform.Find("Label").gameObject.SetActive(false);
                }
                obj.transform.parent = m_scrollow.transform;
                obj.GetComponent<UIButtonMessage>().target = this.gameObject;
                obj.name = i + "";
                obj.transform.localPosition = new Vector3(-169, 202 - i * 79, 0);
                obj.transform.localScale = Vector3.one;
            }
        }
       
    }
    public int comp(dhc.guild_message_t x, dhc.guild_message_t y)
    {
        if (x.otype != y.otype)
        {
            return -(x.otype - y.otype);
        }
        else
        {
            return -(int)(x.time - y.time);
        }
 
    }
    public void click(GameObject obj)
    {
       
        if (obj.name == "send")
        {
            if (m_input.value == "")
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("guild_chat.cs_81_71"));//内容不能为空
                return;
            }
            protocol.game.cmsg_guild_message_add _msg = new protocol.game.cmsg_guild_message_add();
            _msg.text = m_input.value;
            m_input.value = "";
            net_http._instance.send_msg<protocol.game.cmsg_guild_message_add>(opclient_t.CMSG_GUILD_MESSAGE_ADD, _msg);
        }
        else  if(obj.name == "top")
        {
            protocol.game.cmsg_guild_message_top _msg = new protocol.game.cmsg_guild_message_top();
            _msg.msg_guid = m_msg.msgs[id].guid;
            net_http._instance.send_msg<protocol.game.cmsg_guild_message_top>(opclient_t.CMSG_GUILD_MESSAGE_TOP, _msg);
            m_chat_change.transform.Find("frame_big").GetComponent<frame>().hide();
            
        }
        else if (obj.name == "delete")
        {
            protocol.game.cmsg_guild_message_delete _msg = new protocol.game.cmsg_guild_message_delete();
            _msg.msg_guid = m_msg.msgs[id].guid;
            net_http._instance.send_msg<protocol.game.cmsg_guild_message_delete>(opclient_t.CMSG_GUILD_MESSAGE_DELETE, _msg);
            m_chat_change.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "hide")
        {
            m_chat_change.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "close")
        {
            //this.gameObject.SetActive(false);
        }
        else
        {
            try
            {
                id = int.Parse(obj.name);
                if (juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_common)
                {
                    return;
                }
                m_chat_change.SetActive(true);
            }
            catch (UnityException)
            {
                return;
            }
        }

    }
}
