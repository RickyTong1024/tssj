
using UnityEngine;
using System.Collections;

public class player_info_gui : MonoBehaviour,IMessage{
    protocol.game.smsg_player_look m_msg;
    public UILabel m_name;
    public UILabel m_name1;
    public UILabel m_guildname;
    public UILabel m_bf;
    public UILabel m_level;
    public GameObject m_icon;
    public UILabel m_vip;
    public GameObject m_chenghao;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
        
    }
    public void reset(protocol.game.smsg_player_look msg)
    {
        this.m_msg = msg;
        m_name.text = game_data._instance.get_t_language ("player_info_gui.cs_26_22");//玩家信息
        m_name1.text = msg.name;
        m_level.text = string.Format("Lv {0}", msg.level);
        s_t_vip t_vip = game_data._instance.get_t_vip(msg.vip);
        m_vip.text = t_vip.desc;

        if (platform_config_common.m_vip == 0)
        {
        m_vip.gameObject.SetActive(false);
        }
        else
        {
        m_vip.gameObject.SetActive(true);
        }
        if (msg.guild != "")
        {
            m_guildname.text = msg.guild;
        }
        else
        {
            m_guildname.text = game_data._instance.get_t_language ("bingyuan_gui.cs_1203_90");//暂无
        }
        m_bf.text = string.Format(game_data._instance.get_t_language ("player_info_gui.cs_38_34"), sys._instance.value_to_wan(msg.bf));//战斗力 {0}
        sys._instance.get_chenghao(msg.chenghao,m_chenghao);
        sys._instance.remove_child(m_icon);
        GameObject _obj1 = icon_manager._instance.create_player_icon((int)msg.template_id, msg.achieves, msg.vip,msg.nalflag);
        _obj1.transform.parent = m_icon.transform;
        _obj1.transform.localScale = new Vector3(1, 1, 1);
        _obj1.transform.localPosition = new Vector3(0, 0, 0);
    }
    public void click(GameObject obj)
    {
        if (obj.name == "tianjia")
        {
            protocol.game.cmsg_social_add _msg = new protocol.game.cmsg_social_add();
            _msg.player_guid = m_msg.guid;
            net_http._instance.send_msg<protocol.game.cmsg_social_add>(opclient_t.CMSG_SOCIAL_ADD, _msg);
 
        }
        else if (obj.name == "zhenrong")
        {
            root_gui._instance.show_common_player(m_msg);
 
        }
        else if (obj.name == "siliao")
        {
            s_message mes = new s_message();
            mes.m_type = "siliao";
            mes.m_string.Add(m_msg.name);
            cmessage_center._instance.add_message(mes);
            this.transform.Find("frame_big").GetComponent<frame>().hide();
 
        }
        else if (obj.name == "qiechuo")
        {
            protocol.game.cmsg_qiecuo msg = new protocol.game.cmsg_qiecuo();
            msg.target = this.m_msg.guid;
            net_http._instance.send_msg<protocol.game.cmsg_qiecuo>(opclient_t.CMSG_PLAYER_QIECUO, msg);
            //sys._instance.load_scene_ex("ts_fight_jjc");
           // sys._instance.m_game_state = "buttle";
 
        }
        else if (obj.name == "hide")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
 

    }
     void IMessage.message(s_message mes)
    {
 
    }
     void IMessage.net_message(s_net_message mes)
    {
        if (mes.m_opcode == opclient_t.CMSG_PLAYER_QIECUO)
        {
            protocol.game.smsg_qiecuo m_qiecuo = net_http._instance.parse_packet<protocol.game.smsg_qiecuo>(mes.m_byte);
            battle_logic_ex._instance.set_qiecuo_fight_end(m_qiecuo);
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            s_message msg = new s_message();
            msg.m_type = "save_state";
            msg.m_string.Add(sys._instance.m_game_state);
            msg.m_string.Add(sys._instance.m_load_name);
            cmessage_center._instance.add_message(msg);
            sys._instance.load_scene_ex("ts_fight_jjc");
            sys._instance.m_game_state = "buttle";
        }
    }
	
}
