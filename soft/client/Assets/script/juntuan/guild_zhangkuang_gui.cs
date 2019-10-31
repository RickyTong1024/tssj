using System.Collections.Generic;
using UnityEngine;

public class guild_zhangkuang_gui : MonoBehaviour,IMessage {

    public GameObject m_attack_panel;
    public GameObject m_defend_panel;
    public GameObject m_rank_panel;
    public GameObject m_rank_view;
    public UILabel m_attack_zhanji;
    public protocol.game.smsg_guild_fight_pvp_look look_msg;
    protocol.game.smsg_guild_look_zhankuang m_zhankuang;
    protocol.game.smsg_guild_look_zhanji m_zhani;
    public List<GameObject> m_attack_zhankuangs;
    public List<GameObject> m_defend_zhankuangs;
    public UIToggle m_first_toggle;
    public GameObject m_rankme;
    int type = 0;
    void IMessage.message(s_message mes)
    {

 
    }
    void IMessage.net_message(s_net_message msg)
    {
        if (msg.m_opcode == opclient_t.CMSG_PVP_GUILD_ZHANGKUANG_LOOK)
        {
            m_zhankuang = net_http._instance.parse_packet<protocol.game.smsg_guild_look_zhankuang>(msg.m_byte);
            resetzhankuang(type);
 
        }
        else if (msg.m_opcode == opclient_t.CMSG_PVP_GUILD_ZHANJI_LOOK)
        {
            m_zhani = net_http._instance.parse_packet<protocol.game.smsg_guild_look_zhanji>(msg.m_byte);
            resetzhanji();
        }
 
    }
    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void OnEnable()
    {
        look_msg = juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().look_msg;
        protocol.game.cmsg_guild_look_zhankuang _msg = new protocol.game.cmsg_guild_look_zhankuang();
        _msg.guild = look_msg.fight.fights[3].guild;
        _msg.type = 1;
        type = 1;
        net_http._instance.send_msg<protocol.game.cmsg_guild_look_zhankuang>(opclient_t.CMSG_PVP_GUILD_ZHANGKUANG_LOOK, _msg, true);
 
    }
    void resetzhankuang(int type)
    {
        if (type == 1)
        {
            m_attack_panel.SetActive(true);
            m_defend_panel.SetActive(false);
            m_rank_panel.SetActive(false);
            m_attack_zhanji.text = string.Format(game_data._instance.get_t_language ("guild_zhangkuang_gui.cs_64_49"), m_zhankuang.guild_zhanji);//[0090ff]今日攻击战绩：[00ff00]{0}

            for (int i = 0; i < m_zhankuang.guild_icon.Count; i++)
            {
                m_attack_zhankuangs[i].transform.Find("name").GetComponent<UILabel>().text = m_zhankuang.guild_name[i];
                m_attack_zhankuangs[i].transform.Find("catch").GetComponent<UISprite>().spriteName = juntuan_gui._instance.setKuang(m_zhankuang.guild_level[i]);
                if (game_data._instance.get_guild_icon(m_zhankuang.guild_icon[i]) != null)
                {
                    m_attack_zhankuangs[i].transform.Find("catch/icon").GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon(m_zhankuang.guild_icon[i]).icon;
                }
                for (int j = 0; j < 4; j++)
                {
                    s_t_guildfight fight = game_data._instance.get_guild_fight(j);
                    GameObject obj = m_attack_zhankuangs[i].transform.Find("desc").GetChild(j).gameObject;
                    obj.transform.Find("name").GetComponent<UILabel>().text = fight.name;
                    obj.transform.Find("num").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("guild_judian_gui.cs_67_106"),m_zhankuang.guild_player_nums[i * 4 + j], fight.defendrolenum);//{0}/{1}人
                    obj.transform.Find("bar").GetComponent<UIProgressBar>().value = (float)(m_zhankuang.guild_val_nums[i * 4 + j]) / fight.chengfangvalue;
                    obj.transform.Find("bar/text").GetComponent<UILabel>().text = string.Format("{0}/{1}", m_zhankuang.guild_val_nums[i * 4 + j], fight.chengfangvalue);
                }
            }
        }
        else
        {
            m_attack_panel.SetActive(false);
            m_defend_panel.SetActive(true);
            m_rank_panel.SetActive(false);
            m_attack_zhanji.text = string.Format(game_data._instance.get_t_language ("guild_zhangkuang_gui.cs_90_49"), m_zhankuang.guild_zhanji);//[0090ff]今日防守战绩：[00ff00]{0}

            for (int i = 0; i < m_zhankuang.guild_icon.Count; i++)
            {
                m_defend_zhankuangs[i].transform.Find("name").GetComponent<UILabel>().text = m_zhankuang.guild_name[i];
                m_defend_zhankuangs[i].transform.Find("catch").GetComponent<UISprite>().spriteName = juntuan_gui._instance.setKuang(m_zhankuang.guild_level[i]);
                m_defend_zhankuangs[i].transform.Find("catch/icon").GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon(m_zhankuang.guild_icon[i]).icon;
                for (int j = 0; j < 4; j++)
                {
                    s_t_guildfight fight = game_data._instance.get_guild_fight(j);
                    GameObject obj = m_defend_zhankuangs[i].transform.Find("desc").GetChild(j).gameObject;
                    obj.transform.Find("name").GetComponent<UILabel>().text = fight.name;
                    obj.transform.Find("rolenum").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("guild_judian_gui.cs_67_106"), m_zhankuang.guild_player_nums[i * 4 + j], fight.defendrolenum);//{0}/{1}人
                    obj.transform.Find("fangshounum").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("guild_zhangkuang_gui.cs_103_104"), m_zhankuang.guild_val_nums[ i * 4 + j]);//剩余{0}次防守
              
                }
            }

 
        }

 
    }
    void resetzhanji()
    {
        m_attack_panel.SetActive(false);
        m_defend_panel.SetActive(false);
        m_rank_panel.SetActive(true);
        sys._instance.remove_child(m_rank_view);
        for (int i = 0; i < m_zhani.player_guids.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_zhangkuang_sub");
            GameObject iconp = obj.transform.Find("icon").gameObject;
            sys._instance.remove_child(iconp);
            GameObject _obj1 = icon_manager._instance.create_player_icon(m_zhani.player_template[i], m_zhani.player_achieve[i], m_zhani.player_vip[i],m_zhani.player_nalflags[i]);
            _obj1.transform.parent = iconp.transform;
            _obj1.transform.localPosition = Vector3.zero;
            _obj1.transform.localScale = Vector3.one;
            obj.transform.parent = m_rank_view.transform;
            obj.transform.localPosition = new Vector3(0, 86 - i * 70, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("name").GetComponent<UILabel>().text = m_zhani.player_names[i];
            obj.transform.Find("dayzhanji").GetComponent<UILabel>().text = m_zhani.player_zhanjis[i] + "";
            obj.transform.Find("allzhanji").GetComponent<UILabel>().text = m_zhani.player_total_zhanjis[i] + "";
        }

        GameObject iconpa = m_rankme.transform.Find("icon").gameObject;
        sys._instance.remove_child(iconpa);
        GameObject _icon = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count,
            sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
        _icon.transform.parent = iconpa.transform;
        _icon.transform.localPosition = Vector3.zero;
        _icon.transform.localScale = Vector3.one;
        m_rankme.transform.Find("name").GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.name;
        m_rankme.transform.Find("dayzhanji").GetComponent<UILabel>().text = look_msg.fight.zhanji + "";
        m_rankme.transform.Find("allzhanji").GetComponent<UILabel>().text = look_msg.fight.total_zhanji + "";
    }
    public void click(GameObject obj)
    {
        if (obj.name == "attack")
        {
            protocol.game.cmsg_guild_look_zhankuang _msg = new protocol.game.cmsg_guild_look_zhankuang();
            _msg.guild = look_msg.fight.fights[3].guild;
            _msg.type = 1;
            type = 1;
            net_http._instance.send_msg<protocol.game.cmsg_guild_look_zhankuang>(opclient_t.CMSG_PVP_GUILD_ZHANGKUANG_LOOK, _msg, true);
 
        }
        else if (obj.name == "defend")
        {
            protocol.game.cmsg_guild_look_zhankuang _msg = new protocol.game.cmsg_guild_look_zhankuang();
            _msg.guild = look_msg.fight.fights[3].guild;
            _msg.type = 2;
            type = 2;
            net_http._instance.send_msg<protocol.game.cmsg_guild_look_zhankuang>(opclient_t.CMSG_PVP_GUILD_ZHANGKUANG_LOOK, _msg, true);
 
        }
        else if (obj.name == "rank")
        {
            protocol.game.cmsg_guild_look_zhanji _msg = new protocol.game.cmsg_guild_look_zhanji();
            _msg.guild = look_msg.fight.fights[3].guild;
            net_http._instance.send_msg<protocol.game.cmsg_guild_look_zhanji>(opclient_t.CMSG_PVP_GUILD_ZHANJI_LOOK, _msg, true);
        }
    }
}
