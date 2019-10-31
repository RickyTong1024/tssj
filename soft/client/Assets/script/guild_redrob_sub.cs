
using UnityEngine;
using System.Collections;

public class guild_redrob_sub : MonoBehaviour {
    dhc.guild_red_t m_red;
    ulong guid;
    public UILabel m_name;
    public UILabel m_jewel;
    public UILabel m_num;
    public UILabel m_time;
    public Transform m_icon;
    public UISprite m_button_qiang;
    public void reset(dhc.guild_red_t red,ulong guid)
    {
        m_red = red;
        this.guid = guid;
        GameObject icon = icon_manager._instance.create_player_icon
            (m_red.create_id, m_red.create_achieve, m_red.create_vip,m_red.nalflag);
        icon.transform.parent = m_icon;
        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.one;
        this.name  = guid + "";
        m_name.text = game_data._instance.get_t_language ("guild_redrob_sub.cs_23_22") +//发放团员：
            game_data._instance.get_name_color(m_red.create_achieve) + m_red.create_name;
        m_jewel.text = game_data._instance.get_item(m_red.template_id).jewel + "";
        m_num.text = game_data._instance.get_t_language ("guild_redrob_sub.cs_26_21") + (20 - m_red.player_ids.Count) + "/20";//剩余数量：

        m_button_qiang.set_enable(!is_yilingqu());
        long nowtime = (long)timer.now();
        long time = (long)m_red.time + 24 * 60 * 60 * 1000 - nowtime;
        CancelInvoke();
        if (time < 0)
        {
            m_time.text = game_data._instance.get_t_language ("guild_redrob_sub.cs_34_26");//[ff0000]已过期
        }
        else
        {
            InvokeRepeating("set_time", 0, 1);
        }
      
            
 
    }
    bool is_yilingqu()
    {
        bool guoqi = false;
        long nowtime = (long)timer.now();
        long time = (long)m_red.time + 24 * 60 * 60 * 1000 - nowtime;
        if (time < 0)
        {
            guoqi = true;
        }
        if (m_red.player_guid.Contains(sys._instance.m_self.m_guid) || guoqi || m_red.player_ids.Count >= 20)
        {
            return true;
        }
        return false;
    }
    void set_time()
    {
        long nowtime = (long)timer.now();
        long time = (long)m_red.time + 24 * 60 * 60 * 1000 - nowtime;
        if (time > 0)
        {
            m_time.text = string.Format(game_data._instance.get_t_language ("guild_redrob_sub.cs_65_40") , timer.get_time_show_rob((long)time));//[00ff00]{0}[-]后过期
        }
        else
        {
            m_time.text = game_data._instance.get_t_language ("guild_redrob_sub.cs_34_26");//[ff0000]已过期
            s_message _mes = new s_message();
            _mes.m_type = "refresh_red_qiang";
            cmessage_center._instance.add_message(_mes);
            CancelInvoke();
        }
       
    }
    void click(GameObject obj)
    {
        if (obj.name == "rob")
        {
            s_message _mes = new s_message();
            _mes.m_type = "red_rob";
            _mes.m_long.Add(guid);
            cmessage_center._instance.add_message(_mes);

 
        }
        else if (obj.name == "detail")
        {
            s_message _mes = new s_message();
            _mes.m_type = "guild_red_detail";
            _mes.m_object.Add(m_red);
            cmessage_center._instance.add_message(_mes);
 
        }

    }
    void OnDisable()
    {
        CancelInvoke();
    }
	
	
}
