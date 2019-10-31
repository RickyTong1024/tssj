using System.Collections.Generic;
using UnityEngine;

public class guild_judian_gui : MonoBehaviour {
    protocol.game.msg_guild_fight_info m_guild_fightinfo;

    public UILabel m_name;
    public UIProgressBar m_hp;
    public UILabel m_hp_text;
    public UILabel m_guild_zhanji;
    public UILabel m_geren_all_zhanji;
    public UILabel m_geren_zhanji;
    public UILabel m_num;
    public UILabel m_time;
    public List<GameObject> m_judians;
    public GameObject m_main;
    public void reset(protocol.game.msg_guild_fight_info guildinfo)
    {
        m_guild_fightinfo = guildinfo;
        m_name.text = m_guild_fightinfo.guild_name;
        int count_value = 0;
        foreach (int id in game_data._instance.m_dbc_guildfight.m_index.Keys)
        {
            s_t_guildfight fight = game_data._instance.get_guild_fight(id);
            if (fight != null)
            {
                count_value += fight.chengfangvalue;
            }
        }
        int leave = 0;
        for (int i = 0; i < m_guild_fightinfo.guard_points.Count; i++)
        {
            leave += m_guild_fightinfo.guard_points[i];
        }
        m_hp.value = (float)(count_value - leave) / count_value;
        m_hp_text.text = count_value - leave + "/" + count_value;
        protocol.game.msg_guild_fight _fight = juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().look_msg.fight;
        m_guild_zhanji.text = _fight.guild_zhanji + "";
        m_geren_all_zhanji.text = _fight.total_zhanji + "";
        m_geren_zhanji.text = _fight.zhanji + "";
        CancelInvoke();
        InvokeRepeating("time", 0, 1);

        for (int i = 0; i < m_judians.Count; i++)
        {
             s_t_guildfight fight = game_data._instance.get_guild_fight(i);
             int num = 0; 
            for (int j = i * 7; j < i * 7 + 7 && j < m_guild_fightinfo.target_defense_nums.Count; j++)
             {
                 if (m_guild_fightinfo.target_defense_nums[j] >= fight.defendnum)
                 {
                     num += 1;
                 }  
             }
            if (m_guild_fightinfo.guard_gongpo[i] == 1)
            {
                m_judians[i].transform.Find(i + "").gameObject.SetActive(false);
                m_judians[i].transform.Find("yigongqu").gameObject.SetActive(true);


            }
            else
            {
                m_judians[i].transform.Find(i + "").gameObject.SetActive(true);
                m_judians[i].transform.Find("yigongqu").gameObject.SetActive(false);
                m_judians[i].transform.Find(i + "/num").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("guild_judian_gui.cs_67_106"), fight.defendrolenum - num, fight.defendrolenum);//{0}/{1}人
                m_judians[i].transform.Find( i + "/hp").GetComponent<UIProgressBar>().value = (float)(fight.chengfangvalue - m_guild_fightinfo.guard_points[i]) / fight.chengfangvalue;
                m_judians[i].transform.Find(i + "/hp/hp").GetComponent<UILabel>().text = (fight.chengfangvalue - m_guild_fightinfo.guard_points[i]) + "/" + fight.chengfangvalue;
 

            }
            

        }
    }
    public void reset()
    {
        reset(m_guild_fightinfo);
    }
    void time()
    {
        long time = timer.get_time_cuo(24) - (long)timer.now();
        m_time.text = string.Format(game_data._instance.get_t_language ("guild_fight_gui.cs_92_36"), (int)timer.dtnow().DayOfWeek - 1, timer.get_time_show_rob(time));//军团战第{0}轮进行中，{1}结束
    }
    public void click(GameObject obj)
    {

        int index = int.Parse(obj.name);
        if (index == 3)
        {
            bool flag = false;
            for (int i = 0; i < 3; i++)
            {
                if (m_guild_fightinfo.guard_gongpo[i] == 1)
                {
                    flag = true;
                }
            }
            if (!flag)
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_judian_gui.cs_102_58"));//[ffc884]请先攻破其它3个据点
                return;
            }
        }

        if (m_guild_fightinfo.guard_gongpo[int.Parse(obj.name)] > 0)
        {
            root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_pvp_gui.cs_171_54"));//[ffc884]该建筑已被攻破，请攻打其他建筑
        }
        else
        {
                this.transform.parent.gameObject.SetActive(false);
                guild_pvp_gui.m_guild_fightinfo = m_guild_fightinfo;
                guild_pvp_gui.m_id = int.Parse(obj.name);
                sys._instance.m_game_state = "guild_fight_pvp";
                sys._instance.load_scene("ts_game_guildwar");
        }
           
    }
        
}
