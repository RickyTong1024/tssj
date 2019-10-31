using UnityEngine;

public class guild_skill : MonoBehaviour,IMessage {

    public UILabel m_guild_level;
    public UILabel m_guild_exp;
    public UILabel m_cur_level;
    public UILabel m_cur_value;
    public UILabel m_next_level;
    public UILabel m_next_value;
    public UILabel m_level_consume;
    public UILabel m_conname;
    public UILabel m_skill_name;
    public GameObject m_view; 
    public GameObject m_button;
    public UIToggle m_button_stu;
    public UIToggle m_button_kaiqi;
    public UIToggle m_button_desc;
    public UILabel m_con_name;
    public GameObject m_select;
    public GameObject m_main;
    public GameObject m_desc;
    public int type;
    public int inde;
	// Use this for initialization
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	
	}
    void IMessage.message(s_message mes)
    {

 
    }
    void IMessage.net_message(s_net_message mes)
    {
        s_t_guild_keji gu_keji = game_data._instance.get_t_guild_keji(inde);
        if (mes.m_opcode == opclient_t.CMSG_GUILD_KEJI_OPEN)
        {
            juntuan_gui._instance.m_guild_t.exp -= gu_keji.exp + (get_skilllevel_guild(inde)) * gu_keji.expadd;
            juntuan_gui._instance.m_guild_t.skill_ids.Add(inde);
            juntuan_gui._instance.m_guild_t.skill_levels.Add(1);
			sys._instance.play_sound_ex("sound/skill_up");
        }
        else if (mes.m_opcode == opclient_t.CMSG_GUILD_KEJI_UPLEVEL)
        {
            juntuan_gui._instance.m_guild_t.exp -= gu_keji.exp + (get_skilllevel_guild(inde)) * gu_keji.expadd;
            for (int i = 0; i < juntuan_gui._instance.m_guild_t.skill_ids.Count; i ++)
            {
                if (juntuan_gui._instance.m_guild_t.skill_ids[i] == inde)
                {
                    juntuan_gui._instance.m_guild_t.skill_levels[i]++;

                }
 
            }
			sys._instance.play_sound_ex("sound/skill_up");
        }
        else if (mes.m_opcode == opclient_t.CMSG_GUILD_KEJI_STUDY)
        {
            sys._instance.m_self.m_t_player.contribution -= gu_keji.con + (get_skilllevel(inde)) * gu_keji.conadd;
            sys._instance.m_self.m_t_player.guild_skill_ids.Add(inde);
            sys._instance.m_self.m_t_player.guild_skill_levels.Add(1);
			sys._instance.play_sound_ex("sound/skill_up");
        }
		else if (mes.m_opcode == opclient_t.CMSG_GUILD_KEJI_SKILLUP)
        {
            sys._instance.m_self.m_t_player.contribution -= gu_keji.con + (get_skilllevel(inde)) * gu_keji.conadd;
            for (int i = 0; i < sys._instance.m_self.m_t_player.guild_skill_ids.Count; i++)
            {
                if (sys._instance.m_self.m_t_player.guild_skill_ids[i] == inde)
                {
                    sys._instance.m_self.m_t_player.guild_skill_levels[i] ++;
 
                }
            }
			sys._instance.play_sound_ex("sound/skill_up");
        }
        reset(type);
 
    }
	
    void click(GameObject obj)
    {
        s_t_guild_keji guild_keji = game_data._instance.get_t_guild_keji(inde);
        if (obj.name == "togglestudy")
        {
            type = 0;
            reset(type);

        }
        else if(obj.name == "togglekaiqi")
        {
            type = 1;
            reset(type);
        }
        else if (obj.name == "toggledesc")
        {
            m_main.SetActive(false);
            m_desc.SetActive(true);
        }
        else if (obj.name == "guildkaiqi")
        {
            if ((juntuan_gui._instance.m_guild_t.exp - guild_keji.exp - (get_skilllevel_guild(inde)) * guild_keji.expadd) < 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_skill.cs_108_71"));//军团经验不足
                return;

            }
            if (juntuan_gui._instance.m_guild_t.level < guild_keji.guildlevel)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_skill.cs_114_71"));//军团等级不足
                return;

            }
            protocol.game.cmsg_guild_keji keji = new protocol.game.cmsg_guild_keji();
            keji.id = inde;
            net_http._instance.send_msg<protocol.game.cmsg_guild_keji>(opclient_t.CMSG_GUILD_KEJI_OPEN, keji);

        }
        else if (obj.name == "guildlevelup")
        {
            if ((juntuan_gui._instance.m_guild_t.exp - guild_keji.exp - (get_skilllevel_guild(inde)) * guild_keji.expadd) < 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_skill.cs_108_71"));//军团经验不足
                return;

            }
            int max_level = (juntuan_gui._instance.m_guild_t.level - guild_keji.guildlevel + 1) * guild_keji.level;
            if (get_skilllevel_guild(inde) >= max_level)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_skill.cs_134_71"));//已达上限
                return;
            }
            protocol.game.cmsg_guild_keji keji = new protocol.game.cmsg_guild_keji();
            keji.id = inde;
            net_http._instance.send_msg<protocol.game.cmsg_guild_keji>(opclient_t.CMSG_GUILD_KEJI_UPLEVEL, keji);
        }
        else if (obj.name == "study")
        {
            if ((sys._instance.m_self.m_t_player.contribution - guild_keji.con - (get_skilllevel(inde)) * guild_keji.conadd) < 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1190_75"));//军团贡献不足
                return;
            }
            if (get_skilllevel(inde) >= get_skilllevel_guild(inde))
            {
                if (get_skilllevel_guild(inde) == 0)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_skill.cs_152_75"));//未开启该技能
                    return;
                }
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_skill.cs_134_71"));//已达上限
                return;
            }
            protocol.game.cmsg_guild_keji keji = new protocol.game.cmsg_guild_keji();
            keji.id = inde;
            net_http._instance.send_msg<protocol.game.cmsg_guild_keji>(opclient_t.CMSG_GUILD_KEJI_STUDY, keji);
        }
        else if (obj.name == "levelup")
        {
            if ((sys._instance.m_self.m_t_player.contribution - guild_keji.con - (get_skilllevel(inde)) * guild_keji.conadd) < 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1190_75"));//军团贡献不足
                return;
            }
            if (get_skilllevel(inde) >= get_skilllevel_guild(inde))
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("guild_skill.cs_134_71"));//已达上限
                return;
            }
            protocol.game.cmsg_guild_keji keji = new protocol.game.cmsg_guild_keji();
            keji.id = inde;
			net_http._instance.send_msg<protocol.game.cmsg_guild_keji>(opclient_t.CMSG_GUILD_KEJI_SKILLUP, keji);
        }
 
    }
    public void reset(int type)
    {
        m_main.SetActive(true);
        m_desc.SetActive(false);
        if (juntuan_gui._instance.m_zhiwu_t == (int)e_guild_member_type.e_member_type_common)
        {
            m_button_kaiqi.gameObject.SetActive(false);
            m_button_desc.gameObject.transform.localPosition = m_button_kaiqi.transform.localPosition;
        }
        else
        {
            m_button_desc.gameObject.transform.localPosition = new Vector3(-80.79f,241.4f,0);
            m_button_kaiqi.gameObject.SetActive(true);
        }
        if (type == 1)
        {
            
            m_con_name.text = game_data._instance.get_t_language ("guild_skill.cs_197_30");//开启消耗
        }
        else
        {
            m_con_name.text = game_data._instance.get_t_language ("guild_skill.cs_201_30");//提升消耗
 
        }
        m_select.transform.parent = m_view.transform.parent;
        sys._instance.remove_child(m_view);
        m_select.transform.parent = m_view.transform;
        for (int i = 0; i < game_data._instance.m_dbc_guild_keji.get_y(); i++)
        {
            int index = int.Parse(game_data._instance.m_dbc_guild_keji.get(0, i));
            s_t_guild_keji keji = game_data._instance.get_t_guild_keji(index);
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_skill_sub");
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(0, 153 - 86 * i, 0);
            obj.transform.localScale = Vector3.one;
            obj.transform.Find("name").GetComponent<UILabel>().text = keji.name;
            obj.transform.Find("icon").GetComponent<UISprite>().spriteName = keji.icon;
            if (type == 1)
            {
                if (get_skilllevel_guild(index) == 0)
                {
                    obj.transform.Find("desc").GetComponent<UILabel>().text = game_data._instance.get_t_language ("huo_dong_gui.cs_643_86");//未开启
                }
                else
                {
                    obj.transform.Find("desc").GetComponent<UILabel>().text = get_skilllevel_guild(index) + "/" + (juntuan_gui._instance.m_guild_t.level - keji.guildlevel + 1) * keji.level;

                }

            }
            else
            {
                if (get_skilllevel(index) == 0)
                {
                    obj.transform.Find("desc").GetComponent<UILabel>().text = get_skilllevel(index) + "/" + get_skilllevel_guild(index);
                }
                else
                {
                    obj.transform.Find("desc").GetComponent<UILabel>().text = get_skilllevel(index) + "/" + get_skilllevel_guild(index);

                }
 
            }
           
            obj.name = index + "";
            UIEventListener.Get(obj).onClick = selectskill;
            if (inde == index)
            {
                m_select.transform.localPosition = obj.transform.localPosition;
                resetright(index, type);
            }
           
        }
        s_message _message2 = new s_message();
        _message2.m_type = "check_bf";
        cmessage_center._instance.add_message(_message2);
 
    }
    int get_skilllevel(int id)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.guild_skill_ids.Count; i++)
        {
            if (sys._instance.m_self.m_t_player.guild_skill_ids[i] == id)
            {
                return sys._instance.m_self.m_t_player.guild_skill_levels[i];
            }
        }
        return 0;
    }
    int get_skilllevel_guild(int id)
    {
        for (int i = 0; i <juntuan_gui._instance.m_guild_t.skill_levels.Count; i++)
        {
            if (juntuan_gui._instance.m_guild_t.skill_ids[i] == id)
            {
                return juntuan_gui._instance.m_guild_t.skill_levels[i];
            }
        }
        return 0;
    }
    void resetright(int id,int type)
    {
        s_t_guild_keji _keji = game_data._instance.get_t_guild_keji(id);
        m_guild_level.text = "[0aabff]" + game_data._instance.get_t_language ("guild_skill.cs_283_42") + "  [-][6cff00]" + juntuan_gui._instance.m_guild_t.level + "";//军团等级
        m_guild_exp.text = game_data._instance.get_t_language ("guild_skill.cs_284_27") + sys._instance.value_to_wan(juntuan_gui._instance.m_guild_t.exp);//[0aabff]军团经验  [-][6cff00]
        if (type == 1)
        {
            m_con_name.text = game_data._instance.get_t_language ("guild_skill.cs_197_30");//开启消耗
        }
        else
        {
            m_con_name.text = game_data._instance.get_t_language ("guild_skill.cs_296_30");//升级消耗

        }
       
        m_skill_name.text = _keji.name;
        if (type == 1)//军团
        {
            if (get_skilllevel_guild(inde) == 0)
            {
                m_con_name.text = game_data._instance.get_t_language ("guild_skill.cs_197_30");//开启消耗
                m_cur_level.text = game_data._instance.get_t_language ("huo_dong_gui.cs_643_86");//未开启
                m_cur_value.text = game_data._instance.get_t_language ("boss.cs_218_40");//无
                m_next_level.text = (get_skilllevel_guild(inde) + 1) + "";
                m_next_value.text = game_data._instance.get_t_value(_keji.sx).des.Replace("{n}", _keji.sx_value * (get_skilllevel_guild(inde) + 1) + "");

            }
            else
            {
                m_con_name.text = game_data._instance.get_t_language ("guild_skill.cs_201_30");//提升消耗
                m_cur_level.text = get_skilllevel_guild(_keji.id) + "";
                m_cur_value.text = game_data._instance.get_t_value(_keji.sx).des.Replace("{n}", _keji.sx_value * get_skilllevel_guild(inde) + "");
                m_next_level.text = (get_skilllevel_guild(inde) + 1) + "";
                m_next_value.text = game_data._instance.get_t_value(_keji.sx).des.Replace("{n}", _keji.sx_value * (get_skilllevel_guild(inde) + 1) + "");
 
            }
            if (juntuan_gui._instance.m_guild_t.exp >= (_keji.exp + _keji.expadd * get_skilllevel_guild(inde)))
            {
                m_level_consume.text = " [6cff00] x" + (_keji.exp + _keji.expadd * get_skilllevel_guild(inde)) + "";
                m_conname.text = game_data._instance.get_t_language ("guild_skill.cs_324_33");//军团经验

            }
            else
            {
                m_level_consume.text = " [ff0000] x" + (_keji.exp + _keji.expadd * get_skilllevel_guild(inde)) + "";
                m_conname.text = game_data._instance.get_t_language ("guild_skill.cs_324_33");//军团经验
 
            }

            if (get_skilllevel_guild(inde) == 0)
            {
                m_button.name = "guildkaiqi";
                m_button.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("card_dialog_box.cs_205_84");//开启

            }
            else
            {
                m_button.name = "guildlevelup";
                m_button.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guild_skill.cs_343_85");//提升
            }


        }
        else if (type == 0)//个人
        {
            if (get_skilllevel(inde) == 0)
            {
                m_cur_level.text = game_data._instance.get_t_language ("guild_skill.cs_352_35");//未学习
                m_cur_value.text = game_data._instance.get_t_language ("boss.cs_218_40");//无
                m_con_name.text = game_data._instance.get_t_language ("guild_skill.cs_354_34");//学习消耗
                m_next_level.text = (get_skilllevel(inde) + 1) + "";
                m_next_value.text = game_data._instance.get_t_value(_keji.sx).des.Replace("{n}", _keji.sx_value * (get_skilllevel(inde) + 1) + "");

            }
            else
            {
                m_con_name.text = game_data._instance.get_t_language ("guild_skill.cs_201_30");//提升消耗
                m_cur_level.text = get_skilllevel(_keji.id) + "";
                m_cur_value.text = game_data._instance.get_t_value(_keji.sx).des.Replace("{n}", _keji.sx_value * get_skilllevel(inde) + "");
                m_next_level.text = (get_skilllevel(inde) + 1) + "";
                m_next_value.text = game_data._instance.get_t_value(_keji.sx).des.Replace("{n}", _keji.sx_value * (get_skilllevel(inde) + 1) + "");

            }
            if (get_skilllevel(inde) == 0)
            {
                m_button.name = "study";
                m_button.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guild_skill.cs_371_85");//学习

            }
            else
            {
                m_button.name = "levelup";
                m_button.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guild_skill.cs_343_85");//提升
 
            }
            if (sys._instance.m_self.m_t_player.contribution > (_keji.con + _keji.conadd * get_skilllevel(inde)))
            {
                m_level_consume.text = "[00ff00] x" + (_keji.con + _keji.conadd * get_skilllevel(inde)) + "";
                m_conname.text = game_data._instance.get_t_language ("guild_skill.cs_383_33");//军团贡献

            }
            else
            {
                m_level_consume.text = "[ff0000] x" + (_keji.con + _keji.conadd * get_skilllevel(inde)) + "";
                m_conname.text = game_data._instance.get_t_language ("guild_skill.cs_383_33");//军团贡献
 
            }
            
            
        }
        
    }
    void selectskill(GameObject obj)
    {
        int index = int.Parse(obj.name);
        inde = index;
        m_select.transform.localPosition = obj.transform.localPosition;
        resetright(index,type);
 
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
}
