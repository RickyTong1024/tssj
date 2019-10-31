using System.Collections.Generic;
using UnityEngine;

public class guild_hongbao_gui : MonoBehaviour,IMessage {

	// Use this for initialization
    public GameObject m_qiang_gui;
    public GameObject m_fa_gui;
    public GameObject m_caishen_target_gui;
    public GameObject m_hongbao;
    public GameObject m_hongbao_parent;
    public GameObject m_red_detail;
    public GameObject m_qiang_info;
    public int m_id;
    public UIToggle m_button_fa;
    public UIToggle m_buttonfa1;
    public UILabel m_red_num;
    public UILabel m_rob_num;
    public GameObject m_rob_view;
    public GameObject m_caishen_view;
    public GameObject m_caishen_effect;
    public GameObject m_shouqi_effect;
    public GameObject m_rob_effect;
    int index;
    ulong m_rob_guid;
    protocol.game.smsg_guild_red_view m_redview_msg;
    int m_target_id;
     void IMessage.message(s_message mes)
    {
        if (mes.m_type == "guild_red_detail")
        {
            m_red_detail.SetActive(true);
            m_red_detail.GetComponent<guild_red_detail>().reset((dhc.guild_red_t)mes.m_object[0]);
 
        }
        else if (mes.m_type == "refresh_red_qiang")
        {
            send_view_red();
        }
        else if (mes.m_type == "red_rob")
        {
            if (sys._instance.m_self.m_t_player.guild_red_num1 >= 5)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_hongbao_gui.cs_44_58"));//[ffc884]次数不足
                return;
            }
            protocol.game.cmsg_guild_red_rob msg = new protocol.game.cmsg_guild_red_rob();
            msg.guid = (ulong)mes.m_long[0];
            m_rob_guid = msg.guid;
            net_http._instance.send_msg<protocol.game.cmsg_guild_red_rob>(opclient_t.CMSG_GUILD_RED_ROB, msg);
        }
        else if (mes.m_type == "guild_red_lingqu")
        {
            int id = (int)mes.m_ints[0];
            m_target_id = id;
            protocol.game.cmsg_guild_red_target tar = new protocol.game.cmsg_guild_red_target();
            tar.id = id;
            net_http._instance.send_msg<protocol.game.cmsg_guild_red_target>(opclient_t.CMSG_GUILD_RED_TARGET,tar);
        }

 
    }
     void Start()
     {
         cmessage_center._instance.add_handle(this);
     }
     void OnEnable()
     {
         send_view_red();
     }
     void OnDestroy()
     {
         cmessage_center._instance.remove_handle(this);
 
     }
     void IMessage.net_message(s_net_message mes)
    {
        if (mes.m_opcode == opclient_t.CMSG_GUILD_RED_DELIVER)
        {
            sys._instance.m_self.m_t_player.guild_red_num++;
            sys._instance.m_self.m_t_player.guild_deliver_jewel += game_data._instance.get_item(m_id).jewel;
            protocol.game.smsg_guild_red_deliver deliver = net_http._instance.parse_packet<protocol.game.smsg_guild_red_deliver>(mes.m_byte);
            m_redview_msg.guild_reds.Add(deliver.guild_red);
            sys._instance.m_self.remove_item((uint)m_id,1,game_data._instance.get_t_language ("guild_hongbao_gui.cs_84_58"));//军团红包消耗
            s_message _mes = new s_message();
            _mes.m_type = "guild_red_detail";
            _mes.m_object.Add(deliver.guild_red);
            cmessage_center._instance.add_message(_mes);
            reset_fa_gui();
        }
        else if (mes.m_opcode == opclient_t.CMSG_GUILD_RED_ROB)
        {
            protocol.game.smsg_guild_red_rob _rob = net_http._instance.parse_packet<protocol.game.smsg_guild_red_rob>(mes.m_byte);
            sys._instance.m_self.add_att(e_player_attr.player_jewel, _rob.jewel,true,game_data._instance.get_t_language ("guild_hongbao_gui.cs_94_85"));//军团红包抢到
            sys._instance.m_self.m_t_player.guild_red_num1++;
            sys._instance.m_self.m_t_player.guild_rob_jewel += _rob.jewel;
            for (int i = 0; i < m_redview_msg.guild_reds.Count; i++)
            {
                if (m_redview_msg.guild_reds[i].guid == m_rob_guid)
                {
                    m_redview_msg.guild_reds[i].player_achieve.Add(sys._instance.m_self.m_t_player.dress_achieves.Count);

                    m_redview_msg.guild_reds[i].player_guid.Add(sys._instance.m_self.m_t_player.guid);
                    m_redview_msg.guild_reds[i].player_ids.Add((int)sys._instance.m_self.m_t_player.template_id);
                    m_redview_msg.guild_reds[i].player_jewel.Add(_rob.jewel);
                    m_redview_msg.guild_reds[i].player_names.Add(sys._instance.m_self.m_t_player.name);
                    m_redview_msg.guild_reds[i].player_vip.Add(sys._instance.m_self.m_t_player.vip);
                    break;
                }
            }
         
            reset_rob_gui();
          
        }
        else if (mes.m_opcode == opclient_t.CMSG_GUILD_RED_VIEW)
        {
            m_redview_msg = net_http._instance.parse_packet<protocol.game.smsg_guild_red_view>(mes.m_byte);
            if (index == 0)
            {
                m_qiang_gui.SetActive(true);
                m_fa_gui.SetActive(false);
                m_caishen_target_gui.SetActive(false);
                reset_rob_gui();
 
            }
            else if (index == 1)
            {
                index = 0;
                m_fa_gui.SetActive(true);
               
                m_qiang_gui.SetActive(false);
                m_caishen_target_gui.SetActive(false);
                reset_fa_gui();
            }

          
        }
        else if (mes.m_opcode == opclient_t.CMSG_GUILD_RED_TARGET)
        {
            s_t_hongbao_target target = game_data._instance.get_t_hongbao_target(m_target_id);
            for(int i = 0;i < target.rewrds.Count;i ++)
            {
                sys._instance.m_self.add_reward(target.rewrds[i].type, target.rewrds[i].value1
                    , target.rewrds[i].value2, target.rewrds[i].value3,true,game_data._instance.get_t_language ("guild_hongbao_gui.cs_144_76"));//军团红包任务获得
            }
            sys._instance.m_self.m_t_player.guild_red_rewards.Add(target.id);
            reset_caishen_gui(target.type);
        }
 
    }
     public void send_view_red()
     {
         protocol.game.cmsg_common msg = new protocol.game.cmsg_common();
         net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_GUILD_RED_VIEW, msg);
     }
     void OnDisable()
     {
         index = 0;
 
     }
     public static bool target_effect(int type)
     {
         foreach (int id in game_data._instance.m_dbc_hongbao_target.m_index.Keys)
         {
             if (game_data._instance.get_t_hongbao_target(id).type == type)
             {
                 if (sys._instance.m_self.get_hongbao_target_state(id) == 3)
                 {
                     return true;
                 }
 
             }
             
         }
         return false;
     }
     public static bool hongbao_effect()
     {
         if (juntuan_gui._instance.m_msg.has_hongbao || target_effect(1) || target_effect(2))
         {
             return true;
         }
         return false;
     }
     public void close()
     {
         m_hongbao.transform.parent.gameObject.SetActive(false);
     }
    public void click(GameObject obj)
    {
        if(obj.name == "close")
        {
            close();

        }
        else if (obj.name == "toggleqiang")
        {
            send_view_red();

        }
         
        else if (obj.name == "togglefa")
        {
            m_fa_gui.SetActive(true);
            m_qiang_gui.SetActive(false);
            m_caishen_target_gui.SetActive(false);

            reset_fa_gui();
        }
        else if (obj.name == "togglecaishen")
        {
            m_caishen_target_gui.SetActive(true);
            m_fa_gui.SetActive(false);
            m_qiang_gui.SetActive(false);
            reset_caishen_gui(1);

        }
        else if (obj.name == "toggleshouqi")
        {
            m_caishen_target_gui.SetActive(true);
            m_fa_gui.SetActive(false);
            m_qiang_gui.SetActive(false);
            reset_caishen_gui(2);
        }
        else if (obj.name == "fa")
        {
            send_red();
        }
 
    }
    void send_red()
    {
        if (sys._instance.m_self.m_t_player.guild_red_num >= 1)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("guild_hongbao_gui.cs_235_67"));//次数不足
            return;
        }
        protocol.game.cmsg_guild_red_deliver msg = new protocol.game.cmsg_guild_red_deliver();
        msg.id = m_id;
        string text = m_hongbao.transform.Find("input").GetComponent<UIInput>().value.Replace("\n", "");
        text = game_data._instance.m_dfa.search(text);
        msg.text = text;
        net_http._instance.send_msg<protocol.game.cmsg_guild_red_deliver>(opclient_t.CMSG_GUILD_RED_DELIVER, msg);
 
    }
    void click_hongbao(GameObject obj)
    {
        int index = int.Parse(obj.name);
        m_id = 80040001 + index;
        s_t_item item = game_data._instance.get_item(m_id);
        if (sys._instance.m_self.get_item_num((uint)m_id) > 0)
        {
            if (sys._instance.m_self.m_t_player.guild_red_num >= 1)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("guild_hongbao_gui.cs_235_67"));//次数不足
                return;
            }
            m_hongbao.transform.parent.gameObject.SetActive(true);
            sys._instance.remove_child(m_hongbao.transform.Find("icon").gameObject);
            GameObject icon = icon_manager._instance.create_item_icon(m_id);
            icon.transform.parent = m_hongbao.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            m_hongbao.transform.Find("name").GetComponent<UILabel>().text = item.name;
            m_hongbao.transform.localPosition = new Vector3(0,25,0);
            m_hongbao.transform.Find("input").GetComponent<UIInput>().value = game_data._instance.get_t_language ("guild_hongbao_gui.cs_266_83");//军团是我家，繁荣看大家
        }
        else 
        {
            if (sys._instance.m_self.get_item_num((uint)m_id) <= 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("guild_hongbao_gui.cs_272_71"));//红包可以从官方活动【充值翻牌】中的红包商店获得，敬请关注
            }
            
 
        }

    }
    void set_red_num()
    {
        m_red_num.text = game_data._instance.get_t_language ("guild_hongbao_gui.cs_281_25") + sys._instance.m_self.m_t_player.guild_red_num + "/1";//可发放次数：
    }
    void set_rob_num()
    {
        m_rob_num.text = string.Format(game_data._instance.get_t_language ("guild_hongbao_gui.cs_285_39"), sys._instance.m_self.m_t_player.guild_red_num1 );//可抢次数：{0}/5
    }
    void reset_fa_gui()
    {
        m_hongbao.transform.parent.gameObject.SetActive(false);
        
        sys._instance.remove_child(m_hongbao_parent);
        for (int i = 0; i < 3; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/hongbao_item");
            obj.transform.parent = m_hongbao_parent.transform;
            obj.transform.localPosition = new Vector3(-240 + i * 240,20,0);
            obj.transform.localScale = Vector3.one;
            GameObject icon = icon_manager._instance.create_item_icon(80040001 + i);
            sys._instance.remove_child(obj.transform.Find("icon").gameObject);
            icon.transform.parent = obj.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            UIEventListener.Get(obj).onClick = click_hongbao;
            obj.name = i + "";
            string s = "";
            if (i == 0)
            {
                obj.GetComponent<UISprite>().spriteName = "nnssl_hb_g";
                obj.transform.Find("back").GetComponent<UISprite>().spriteName = "nnssl_hb_gk";
                s = "[00ff00]";
            }
            else if (i == 1)
            {
                obj.GetComponent<UISprite>().spriteName = "nnssl_hb_b";
                obj.transform.Find("back").GetComponent<UISprite>().spriteName = "nszjm_szblz04";
                s = "[009dff]";

            }
            else if (i == 2)
            {
                obj.GetComponent<UISprite>().spriteName = "nnssl_hb_r";
                obj.transform.Find("back").GetComponent<UISprite>().spriteName = "nszjm_szblz06";
                s = "[ff00ff]";

            }
            obj.transform.Find("jewel").GetComponent<UILabel>().text = s + game_data._instance.get_item(80040001 + i).jewel +" "+ game_data._instance.get_t_language ("czfp_gui.cs_360_108");//钻石
            obj.transform.Find("num").GetComponent<UILabel>().text = s + game_data._instance.get_t_language ("catch_card_gui.cs_208_136") + "：" + sys._instance.m_self.get_item_num((uint)80040001 + (uint)i);//拥有

        }
        set_red_num();
        m_caishen_effect.SetActive(target_effect(1));
        m_shouqi_effect.SetActive(target_effect(2));
        m_rob_effect.SetActive(is_rob() && sys._instance.m_self.m_t_player.guild_red_num1 < 5);


    }
    void reset_rob_gui()
    {
        if (m_rob_view.GetComponent<SpringPanel>() != null)
        {
            m_rob_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_rob_view.transform.localPosition = new Vector3(0, 0, 0);
        m_rob_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_rob_view);
        m_shouqi_effect.SetActive(target_effect(2));
        m_caishen_effect.SetActive(target_effect(1));
        set_rob_num();
        m_rob_effect.SetActive(is_rob() && sys._instance.m_self.m_t_player.guild_red_num1 < 5);
        if (m_redview_msg.guild_reds.Count == 0)
        {
            m_qiang_info.SetActive(true);
            return;
        }
        m_qiang_info.SetActive(false);
        m_redview_msg.guild_reds.Sort(compare);
        for (int i = 0; i < m_redview_msg.guild_reds.Count; i++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_redrob_sub");
            obj.transform.parent = m_rob_view.transform;
            obj.transform.localPosition = new Vector3(0,110 - i * 142,0);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<guild_redrob_sub>().reset(m_redview_msg.guild_reds[i], m_redview_msg.guild_reds[i].guid);
        }
    
     
 
    }
    void reset_caishen_gui(int type)
    {
        if (m_caishen_view.GetComponent<SpringPanel>() != null)
        {
            m_caishen_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_caishen_view.transform.localPosition = new Vector3(0, 0, 0);
        m_caishen_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_caishen_view);
        List<int> ids = new List<int>();
        foreach (int id in game_data._instance.m_dbc_hongbao_target.m_index.Keys)
        {
            s_t_hongbao_target target = game_data._instance.get_t_hongbao_target(id);
            if (target.type == type)
            {
                ids.Add(target.id);
            }
        }
        ids.Sort(compare);
        for(int i  = 0;i < ids.Count;i ++)
        {
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_red_target");
            obj.transform.parent = m_caishen_view.transform;
            obj.transform.localPosition = new Vector3(0, 146 - i * 143, 0);
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<guild_red_target>().reset(ids[i]);
        }
        if (type == 1)
        {
            m_caishen_effect.SetActive(target_effect(1));
        }
        else
        {
            m_shouqi_effect.SetActive(target_effect(2));
        }
        m_rob_effect.SetActive(is_rob() && sys._instance.m_self.m_t_player.guild_red_num1 < 5);
 
    }
    bool is_rob()
    {
        for (int i = 0; i < m_redview_msg.guild_reds.Count; i++)
        {
            if (!is_rob_id(m_redview_msg.guild_reds[i]))
            {
                return true;
            }
        }
       
        return false;
       
    }
    int compare(int x, int y)
    {
        int x_state = sys._instance.m_self.get_hongbao_target_state(x);
        int y_state = sys._instance.m_self.get_hongbao_target_state(y);
        if(x_state != y_state)
        {
            return y_state - x_state;

        }
        return x - y;
    }
    int compare(dhc.guild_red_t x,dhc.guild_red_t y)
    {
        bool x_ = false;
        bool y_ = false;
        x_ = is_rob_id(x);
        y_ = is_rob_id(y);
        if (x_ && !y_)
        {
            return 1;
        }
        else if (!x_ && y_)
        {
            return -1;
        }
        else
        {
            

            return (int)(y.time - x.time);
        }
 
    }
    bool is_rob_id(dhc.guild_red_t x)
    {
        if (timer.now() > x.time + 24 * 60 * 60 * 1000 || x.player_guid.Contains(sys._instance.m_self.m_guid) || x.player_ids.Count >= 20)
        {
            return  true;
        }
        return false;
    }
    public void upde_ui(int id)
    {
        if(id == 1)
        {
            index = id;
        }
    }
	
}
