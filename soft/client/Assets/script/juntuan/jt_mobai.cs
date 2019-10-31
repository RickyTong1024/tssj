using System.Collections.Generic;
using UnityEngine;

public class jt_mobai : MonoBehaviour,IMessage {
    public protocol.game.smsg_guild_activity m_msg;
    int rewardid; 
    public UILabel m_value1;
    public UILabel m_value2;
    public UILabel m_value3;
    public UILabel m_value4;
    public UISlider m_slider;
    public GameObject m_hor_reward;
    public GameObject m_icon1;
    public GameObject m_icon2;
   // public UILabel m_lba;
    public GameObject m_horlingqu_button;
    public List<UISprite> m_sprites;
    public List<GameObject> m_mobais;
    public int sign_flag;
    public GameObject m_mobai_obj;
    public GameObject m_obj;
    public GameObject m_desc;
    public UILabel m_cj_hr;
    public UILabel m_cj_gx;
    public UILabel m_cj_hb;
    public UILabel m_cj_name;
    public UILabel m_cj_exp;

    public UILabel m_zj_hr;
    public UILabel m_zj_gx;
    public UILabel m_zj_hb;
    public UILabel m_zj_name;
    public UILabel m_zj_exp;

    public UILabel m_today_hor;
    public UILabel m_num_hor;
    public UILabel m_gj_hr;
    public UILabel m_gj_gx;
    public UILabel m_gj_hb;
    public UILabel m_gj_name;
    public UILabel m_gj_exp;

    public List<GameObject> m_effects;
    public int m_type;
	// Use this for initialization

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void OnEnable()
    {
        cmessage_center._instance.add_handle(this);
    }
    void OnDisable()
    {
        cmessage_center._instance.remove_handle(this);
    }

    public void click(GameObject obj)
    {
        if (obj.name == "hide")
        {
            m_mobai_obj.SetActive(false);
           
        }
        else if (obj.name == "horer_lingqu")
        {
            protocol.game.cmsg_guild_sign_reward _msg = new protocol.game.cmsg_guild_sign_reward();
            _msg.id = rewardid;
            net_http._instance.send_msg<protocol.game.cmsg_guild_sign_reward>(opclient_t.CMSG_GUILD_SIGN_REWARD, _msg);
        }
        else if (obj.name == "guild_hor_reward")
        {
            s_t_guild_horreward _hor1 = game_data._instance.get_t_guild_horreward(1);
            s_t_guild_horreward _hor2 = game_data._instance.get_t_guild_horreward(2);
            s_t_guild_horreward _hor3 = game_data._instance.get_t_guild_horreward(3);
            s_t_guild_horreward _hor4 = game_data._instance.get_t_guild_horreward(4);
            m_hor_reward.SetActive(true);
            m_horlingqu_button.name = "horer_lingqu";
            m_horlingqu_button.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("jt_mobai.cs_97_91");//领取
            sys._instance.remove_child(m_icon1);
            sys._instance.remove_child(m_icon2);
         
        }
        else if (obj.name == "get")
        {
            s_message msg = new s_message();

            switch(sign_flag)
            {
                case 0:
                      if (sys._instance.m_self.m_t_player.gold < game_data._instance.get_guild_sign(0).coin)
                        {
                            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
                            return;
                        }
           
                    msg.m_type = "chujiqiandao";
                    cmessage_center._instance.add_message(msg);
                    break;
                case 1:
                     if (sys._instance.m_self.m_t_player.gold < game_data._instance.get_guild_sign(1).coin)
                        {
                            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
                            return;
                        }
           
                    msg.m_type = "zhongjiqiandao";
                    cmessage_center._instance.add_message(msg);
                    break;
                case 2:
                     if (sys._instance.m_self.m_t_player.gold < game_data._instance.get_guild_sign(2).coin)
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
                        return;
                    }
          
                    msg.m_type = "gaojiqiandao";
                    cmessage_center._instance.add_message(msg);

                    break;
            }
        }
        else if (obj.name == "cj_button")
        {
            if (m_msg.sign_flag > 0)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("jt_mobai.cs_147_58"));//[ffc882]今天已经膜拜过了
                return;
            }
            sign_flag = 0;
            s_t_guild_sign sign = game_data._instance.get_guild_sign(sign_flag);
            sys._instance.remove_child(m_obj);
            GameObject obj1 = icon_manager._instance.create_resource_icon(10, sign.gongxian);
            obj1.transform.parent = m_obj.transform;
            obj1.transform.localPosition = Vector3.zero;
            obj1.transform.localScale = Vector3.one;
            m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("jt_mobai.cs_157_64"),sign.exp);//您的膜拜可以为军团增加{0}经验
            m_mobai_obj.SetActive(true);
          
        }
        else if (obj.name == "zj_button")
        {
            if (m_msg.sign_flag > 0)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("jt_mobai.cs_147_58"));//[ffc882]今天已经膜拜过了
                return;
            }
            sign_flag = 1;
            s_t_guild_sign sign = game_data._instance.get_guild_sign(sign_flag);
            sys._instance.remove_child(m_obj);
            GameObject obj1 = icon_manager._instance.create_resource_icon(10, sign.gongxian);
            obj1.transform.parent = m_obj.transform;
            obj1.transform.localPosition = Vector3.zero;
            obj1.transform.localScale = Vector3.one;
            m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("jt_mobai.cs_157_64"), sign.exp );//您的膜拜可以为军团增加{0}经验
            m_mobai_obj.SetActive(true);
        }
        else if (obj.name == "close_reward")
        {
            m_hor_reward.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.name == "gj_button")
        {
            if (m_msg.sign_flag > 0)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("jt_mobai.cs_147_58"));//[ffc882]今天已经膜拜过了
                return;
            }
            sign_flag = 2;
            s_t_guild_sign sign = game_data._instance.get_guild_sign(sign_flag);
            sys._instance.remove_child(m_obj);
            GameObject obj1 = icon_manager._instance.create_resource_icon(10,sign.gongxian);
            obj1.transform.parent = m_obj.transform;
            obj1.transform.localPosition = Vector3.zero;
            obj1.transform.localScale = Vector3.one;
            m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("jt_mobai.cs_157_64"), sign.exp );//您的膜拜可以为军团增加{0}经验
            m_mobai_obj.SetActive(true);
        }
        else
        {
            try
            {
                rewardid = int.Parse(obj.name);
                m_hor_reward.SetActive(true);
                if (can_reward(rewardid))
                {
                    m_horlingqu_button.name = "horer_lingqu";
                    m_horlingqu_button.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("jt_mobai.cs_97_91");//领取
                }
                else
                {
                    m_horlingqu_button.name = "close_reward";
                    m_horlingqu_button.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("jt_mobai.cs_217_99");//关闭
                }
                sys._instance.remove_child(m_icon1);
                sys._instance.remove_child(m_icon2);
                s_t_guild_horreward reward = game_data._instance.get_t_guild_horreward(rewardid);
                if (reward == null)
                {
                    return;
                }
                GameObject _icon1 = icon_manager._instance.create_reward_icon(reward.reward.type, reward.reward.value1, reward.reward.value2 + (juntuan_gui._instance.m_guild_t.last_level - 1) * reward.value2_add, reward.reward.value3);
                _icon1.transform.parent = m_icon1.transform;
                _icon1.transform.localPosition = Vector3.zero;
                _icon1.transform.localScale = Vector3.one;
            }
            catch (UnityException)
            {

            }
        }
    }

    bool can_reward(int id)
    {
        bool flag = true;
        s_t_guild_horreward _hor = game_data._instance.get_t_guild_horreward(id);
        for (int i = 0; i < m_msg.mobai_ids.Count; i++)
        {
            if (id == m_msg.mobai_ids[i])
            {
                flag =  false;
            }
        }
        if (flag && juntuan_gui._instance.m_guild_t.honor >= _hor.value + _hor.speed_add * (juntuan_gui._instance.m_guild_t.last_level - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public  void message(s_message message)
    {
       s_t_guild_sign t_guild_sign;
       if (message.m_type == "zhongjiqiandao")
       {
            t_guild_sign = game_data._instance.get_guild_sign(1);
           if(sys._instance.m_self.m_t_player.gold < t_guild_sign.coin)
           {
               root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
               return;
           }
           if (sys._instance.m_self.m_t_player.jewel < t_guild_sign.zuanshi)
           {
               root_gui._instance.show_recharge_dialog_box(delegate()
               {
                  //juntuan_gui._instance.transform.Find("frame_big").GetComponent<>
               });
               return;
           }
           protocol.game.cmsg_guild_sign_in _msg = new protocol.game.cmsg_guild_sign_in();
           _msg.sign_in_type = 1;
           m_type = 1;
           net_http._instance.send_msg<protocol.game.cmsg_guild_sign_in>(opclient_t.CMSG_GUILD_SIGN_IN, _msg);
       }
       if (message.m_type == "gaojiqiandao")
       {
           t_guild_sign = game_data._instance.get_guild_sign(2);
           if (sys._instance.m_self.m_t_player.gold < t_guild_sign.coin)
           {
               root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
               return;
           }
           if (sys._instance.m_self.m_t_player.jewel < t_guild_sign.zuanshi)
           {
               root_gui._instance.show_recharge_dialog_box(delegate()
               {
                   //juntuan_gui._instance.transform.Find("frame_big").GetComponent<>
               });
               return;
           }
           protocol.game.cmsg_guild_sign_in _msg = new protocol.game.cmsg_guild_sign_in();
           _msg.sign_in_type = 2;
           m_type = 2;
           net_http._instance.send_msg<protocol.game.cmsg_guild_sign_in>(opclient_t.CMSG_GUILD_SIGN_IN, _msg);
       }
       if (message.m_type == "chujiqiandao")
       {
           t_guild_sign = game_data._instance.get_guild_sign(0);
           if (sys._instance.m_self.m_t_player.gold < t_guild_sign.coin)
           {
               root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60"));//金币不足
               return;
           }
           if (sys._instance.m_self.m_t_player.jewel < t_guild_sign.zuanshi)
           {
               root_gui._instance.show_recharge_dialog_box(delegate()
               {
                   //juntuan_gui._instance.transform.Find("frame_big").GetComponent<>
               });
               return;
           }
           protocol.game.cmsg_guild_sign_in _msg = new protocol.game.cmsg_guild_sign_in();
           _msg.sign_in_type = 0;
           m_type = 0;
           net_http._instance.send_msg<protocol.game.cmsg_guild_sign_in>(opclient_t.CMSG_GUILD_SIGN_IN, _msg);
       }
   }
    public void net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_GUILD_SIGN_IN)
        {
            m_mobai_obj.SetActive(false);
            protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success>(message.m_byte);
            if (_msg.success)
            {
                s_t_guild_sign t_guild_sign = game_data._instance.get_guild_sign(m_type);
                sys._instance.m_self.sub_att(e_player_attr.player_gold, t_guild_sign.coin,game_data._instance.get_t_language ("jt_mobai.cs_336_90"));//军团膜拜消耗
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, t_guild_sign.zuanshi,game_data._instance.get_t_language ("jt_mobai.cs_336_90"));//军团膜拜消耗
                sys._instance.m_self.add_att(e_player_attr.player_guild_hor, t_guild_sign.hor);
                sys._instance.m_self.add_att(e_player_attr.player_contribution, t_guild_sign.gongxian);
                sys._instance.m_self.add_att(e_player_attr.player_guild_exp, t_guild_sign.exp);
                for (int i = 0; i < juntuan_gui._instance.m_guild_member_t.Count; i++)
                {
                    if (sys._instance.m_self.m_t_player.guid == juntuan_gui._instance.m_guild_member_t[i].player_guid)
                    {
                        juntuan_gui._instance.m_guild_member_t[i].last_sign_time = timer.now();
                       juntuan_gui._instance.m_juntuan_main.GetComponent<juntuan_main>().m_juntuan_number.GetComponent<juntuan_number_info>().update_ui();
                    }
                }
                m_msg.sign_flag = m_type + 1;
                
                juntuan_gui._instance.m_guild_t.mobai_num++;
                updateui();
            } 
        }
        else if (message.m_opcode == opclient_t.CMSG_GUILD_SIGN_REWARD)
        {
            s_t_guild_horreward _hor1 =game_data._instance.get_t_guild_horreward(rewardid);
            sys._instance.m_self.add_reward(_hor1.reward.type, _hor1.reward.value1, _hor1.reward.value2 + (juntuan_gui._instance.m_guild_t.last_level - 1) * _hor1.value2_add, _hor1.reward.value3,game_data._instance.get_t_language ("jt_mobai.cs_358_195"));//军团签到获得
            m_hor_reward.transform.Find("frame_big").GetComponent<frame>().hide();
            m_msg.mobai_ids.Add(rewardid);
            updateui();
          
        }
       
    }
   public void updateui()
    {
        int add_value = (juntuan_gui._instance.m_guild_t.last_level - 1);
        s_t_guild_horreward _hor1 =game_data._instance.get_t_guild_horreward(1);
        s_t_guild_horreward _hor2 = game_data._instance.get_t_guild_horreward(2);
        s_t_guild_horreward _hor3 = game_data._instance.get_t_guild_horreward(3);
        s_t_guild_horreward _hor4 = game_data._instance.get_t_guild_horreward(4);
        s_t_guild_sign _sign1 = game_data._instance.get_guild_sign(0);
        s_t_guild_sign _sign2 = game_data._instance.get_guild_sign(1);
        s_t_guild_sign _sign3 = game_data._instance.get_guild_sign(2);
        m_value1.text = _hor1.value + add_value * _hor1.speed_add + "";
        m_value2.text = _hor2.value + add_value * _hor2.speed_add + "";
        m_value3.text = _hor3.value + add_value * _hor3.speed_add + "";
        m_value4.text = _hor4.value + add_value * _hor4.speed_add + "";
        for (int i = 0; i < m_effects.Count; i++)
        {
            s_t_guild_horreward h =game_data._instance.get_t_guild_horreward(i + 1);
            float rate = (float)(h.value + add_value * h.speed_add) / (_hor4.value + add_value * _hor4.speed_add);
            m_effects[i].transform.parent.localPosition = new Vector3(-338 + m_slider.GetComponent<UISprite>().width * rate, 27, 0);
        }
            for (int i = 0; i < m_effects.Count; i++)
            {
                if (can_reward(i + 1))
                {
                    m_effects[i].SetActive(true);
                }
                else
                {
                    m_effects[i].SetActive(false);
                }
            }
            for (int i = 0; i < m_mobais.Count; i++)
            {
                m_mobais[i].SetActive(false);
            }
        if (m_msg.sign_flag > 0)
        {
            m_mobais[m_msg.sign_flag - 1].SetActive(true);
        }
        for (int i = 0; i < m_sprites.Count; i++)
        {
            s_t_guild_horreward _hor = game_data._instance.get_t_guild_horreward(i + 1);
            set_sprite(_hor,m_sprites[i]);
            
        }
        m_slider.value = (float)juntuan_gui._instance.m_guild_t.honor / (_hor4.value + add_value * _hor4.speed_add);
        m_num_hor.text = game_data._instance.get_t_language ("jt_mobai.cs_412_25") + juntuan_gui._instance.m_guild_t.mobai_num + "/" + game_data._instance.get_guild(juntuan_gui._instance.m_guild_t.level).number_count;//今日膜拜人数：
        m_today_hor.text = game_data._instance.get_t_language ("jt_mobai.cs_413_27") + "  "+ juntuan_gui._instance.m_guild_t.honor + ""; //今日军团荣誉：
        m_cj_name.text =  _sign1.name;
        m_cj_exp.text = "[00ffff]" + game_data._instance.get_t_language ("guild_skill.cs_324_33") + " +[00ff00]" + _sign1.exp + "";//军团经验
        m_zj_exp.text = "[00ffff]" + game_data._instance.get_t_language ("jt_mobai.cs_416_37") + " +[00ff00]" + _sign2.exp + "";//军团经验 
        m_gj_exp.text = "[00ffff]" + game_data._instance.get_t_language ("guild_skill.cs_324_33") + " +[00ff00]"  + _sign3.exp + "";//军团经验
        m_zj_name.text = _sign2.name;
        m_gj_name.text = _sign3.name;
        m_cj_gx.text = "[00ffff]" + game_data._instance.get_t_language ("guild_skill.cs_383_33") + " +[00ff00]" + _sign1.gongxian + "";//军团贡献
        m_zj_gx.text = "[00ffff]" + game_data._instance.get_t_language ("guild_skill.cs_383_33") + " +[00ff00]" + _sign2.gongxian + "";//军团贡献
        m_gj_gx.text = "[00ffff]" + game_data._instance.get_t_language ("guild_skill.cs_383_33") + " +[00ff00]" + _sign3.gongxian + "";//军团贡献

        m_cj_hr.text = "[00ffff]" + game_data._instance.get_t_language ("jt_mobai.cs_424_36") + " +[00ff00]" + _sign1.hor + "";//军团荣誉
        m_zj_hr.text = "[00ffff]" + game_data._instance.get_t_language ("jt_mobai.cs_424_36") + " +[00ff00]" + _sign2.hor + "";//军团荣誉
        m_gj_hr.text = "[00ffff]" + game_data._instance.get_t_language ("jt_mobai.cs_424_36") + " +[00ff00]" + _sign3.hor + "";//军团荣誉

        m_cj_hb.text = game_data._instance.get_t_resource(1).namecolor + "x" + _sign1.coin + "";
        m_zj_hb.text = game_data._instance.get_t_resource(2).namecolor + "x" + _sign2.zuanshi + "";
        m_gj_hb.text = game_data._instance.get_t_resource(2).namecolor + "x" + _sign3.zuanshi + "";
    }
   void set_sprite(s_t_guild_horreward _hor ,UISprite sprite)
   {
       string name = "";
       if (_hor.id == 1)
       {
           name = "green_box_b";
       }
       else if (_hor.id == 2)
       {
           name = "blue_box_b";
       }
       else if (_hor.id == 3)
       {
           name = "purple_box_b";
       }
       else if (_hor.id == 4)
       {
           name = "gold_box_b";
       }
       sprite.spriteName = name + "1";
       if ((float)juntuan_gui._instance.m_guild_t.honor >= _hor.value + _hor.speed_add * (juntuan_gui._instance.m_guild_t.last_level - 1))
       {
           sprite.spriteName = sprite.spriteName = name + "04";
       }
       for (int i = 0; i < m_msg.mobai_ids.Count; i++)
       {
           if (m_msg.mobai_ids[i] == _hor.id)
           {
               sprite.spriteName = sprite.spriteName = name + "5";

           }
       }
 
   }
}
