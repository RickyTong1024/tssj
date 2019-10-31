using UnityEngine;

public class chat : MonoBehaviour
{
    public protocol.game.smsg_chat m_scs;

    public void init()
    {
        System.DateTime dt = timer.time2dt(timer.chatChangeTime(m_scs.time));
        this.transform.Find("time1").GetComponent<UILabel>().text = dt.ToString("yyyy-MM-dd");
        this.transform.Find("time").GetComponent<UILabel>().text = dt.ToString("HH:mm:ss");
        this.transform.Find("name").GetComponent<UILabel>().text = "[00ff00]Lv." + m_scs.level.ToString() + " " + m_scs.player_name;
        this.transform.Find("text").GetComponent<UILabel>().text = m_scs.color + m_scs.text;
        this.transform.Find("icon").GetComponent<UISprite>().spriteName = player.get_touxiang((int)m_scs.player_template);
        if (m_scs.vip == 0)
        {
            this.transform.Find("vip").gameObject.SetActive(false);
        }
        else
        {
            
            s_t_vip t_vip = game_data._instance.get_t_vip(m_scs.vip);
            if (t_vip != null)
            {
                this.transform.Find("vip").GetComponent<UILabel>().text = t_vip.desc;
                this.transform.Find("vip").gameObject.SetActive(true);
            }
            else
            {
                this.transform.Find("vip").gameObject.SetActive(false);
            }
        }
        if (platform_config_common.m_vip == 0)
        {
            this.transform.Find("vip").gameObject.SetActive(false);
        }
        if(platform_config_common.m_nationality > 0)
        {
            this.transform.Find("icon/gq").gameObject.SetActive(true);
        }

        if (m_scs.type == 2 && m_scs.player_guid == sys._instance.m_self.m_guid)
        {
            this.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("chat.cs_38_83"), m_scs.target_name);//发送给{0} [00ff00]
        }
        if (m_scs.nalflag != 0)
        {
            if (platform_config_common.m_nationality == 0)
            {
                this.transform.Find("icon/gq").gameObject.SetActive(false);
            }
            else
            {
                this.transform.Find("icon/gq").gameObject.SetActive(true);
                this.transform.Find("icon/gq").GetComponent<UISprite>().spriteName = sys._instance.returnfirstSpritename(m_scs.nalflag);
            }
        }
        else
        {
            this.transform.Find("icon/gq").gameObject.SetActive(false);
        }
    }
}
