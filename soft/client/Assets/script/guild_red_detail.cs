
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class guild_red_detail : MonoBehaviour,IMessage {
    public UILabel m_name;
    public Transform m_icon;
    public UILabel m_jiyu;
    public GameObject m_view;
    private dhc.guild_red_t m_red_detail;

    void IMessage.message(s_message mes)
    {

    }
    void IMessage.net_message(s_net_message mes)
    {
 
    }
    int compare(int x, int y)
    {
        int x_jewel = m_red_detail.player_jewel[x];
        int y_jewel = m_red_detail.player_jewel[y];
        return y_jewel - x_jewel;
    }
    public void reset(dhc.guild_red_t red_detail)
    {
        m_red_detail = red_detail;
        m_name.text = game_data._instance.get_name_color(red_detail.create_achieve) + red_detail.create_name;
        m_jiyu.text = red_detail.text;
        sys._instance.remove_child(m_icon.gameObject);
        GameObject icon = icon_manager._instance.create_player_icon
            (m_red_detail.create_id, m_red_detail.create_achieve, m_red_detail.create_vip,m_red_detail.nalflag);
        icon.transform.parent = m_icon;
        icon.transform.localPosition = Vector3.zero;
        icon.transform.localScale = Vector3.one;
        List<int> ids = new List<int>();
        for (int i = 0; i < m_red_detail.player_ids.Count; i++)
        {
            ids.Add(i);
        }
        ids.Sort(compare);
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
        for (int j = 0; j < ids.Count; j++)
        {
            int i = ids[j];
            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guild_red_detail_sub");
            if (m_red_detail.player_guid[i] == sys._instance.m_self.m_guid)
            {
                obj.GetComponent<UISprite>().spriteName = "nnssl_hbxq_bg";
            }
            else
            {
                obj.GetComponent<UISprite>().spriteName = "jtlt_xdk001";
 
            }
            obj.transform.parent = m_view.transform;
            obj.transform.localPosition = new Vector3(1,96 - j * 84,0);
            obj.transform.localScale = Vector3.one;
            GameObject m_icon1 = obj.transform.Find("icon").gameObject;
            sys._instance.remove_child(m_icon1);
            GameObject icon1 = icon_manager._instance.create_player_icon
                (m_red_detail.player_ids[i], m_red_detail.player_achieve[i], m_red_detail.player_vip[i],m_red_detail.player_nalflag[i]).gameObject;
            icon1.transform.parent = m_icon1.transform;
            icon1.transform.localPosition = Vector3.zero;
            icon1.transform.localScale = Vector3.one;
            obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_red_detail.player_achieve[i]) + m_red_detail.player_names[i];
            obj.transform.Find("jewel").GetComponent<UILabel>().text =
                sys._instance.get_res_color(2) +  m_red_detail.player_jewel[i] + "";
            if (m_red_detail.player_guid[i] == sys._instance.m_self.m_t_player.guid)
            {
 
            }
        }
        
    }
	
}
