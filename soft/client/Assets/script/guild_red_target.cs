
using UnityEngine;
using System.Collections;

public class guild_red_target : MonoBehaviour {

    public int m_active_id;
    public int reward = 0;
    public GameObject m_icon;
    public int player_level = 0;
	// Use this for initialization
	void Start () {
	
	}

    public void reset(int id)
    {
        m_active_id = id;
        s_t_hongbao_target t_boss_active = game_data._instance.get_t_hongbao_target(m_active_id);
        transform.Find("ylq").gameObject.SetActive(false);
        transform.Find("wcd").gameObject.SetActive(true);
        transform.Find("desc").GetComponent<UILabel>().text = t_boss_active.desc;
        int jewel = 0;
        if(t_boss_active.type == 1)
        {
            jewel = sys._instance.m_self.m_t_player.guild_deliver_jewel;
        }
        else
        {
            jewel = sys._instance.m_self.m_t_player.guild_rob_jewel;
        }
        if (sys._instance.m_self.get_hongbao_target_state(m_active_id) == 3)
        {
            transform.Find("wcd").GetComponent<UILabel>().text = "[ffffff]" + game_data._instance.get_t_language ("active.cs_19_75");//目标达成
            transform.Find("get").GetComponent<UISprite>().set_enable(true);
            transform.Find("get").gameObject.SetActive(true);
            transform.Find("ylq").gameObject.SetActive(false);

        }
        else if (sys._instance.m_self.get_hongbao_target_state(m_active_id) == 1)
        {
            transform.Find("wcd").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guild_red_target.cs_45_70") + "  "+ "[24d0fd]" + jewel + "/" + t_boss_active.tiaojian;//进度：
            transform.Find("ylq").gameObject.SetActive(true);
            transform.Find("wcd").gameObject.SetActive(false);
            transform.Find("get").gameObject.SetActive(false);

        }
        else
        {
			transform.Find("wcd").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guild_red_target.cs_45_70") + "  " + "[24d0fd]" + jewel + "/" + t_boss_active.tiaojian;//进度：
            transform.Find("get").GetComponent<UISprite>().set_enable(false);
            transform.Find("get").gameObject.SetActive(true);
            transform.Find("ylq").gameObject.SetActive(false);

        }
        sys._instance.remove_child(m_icon);
        for (int i = 0; i < t_boss_active.rewrds.Count; i++)
        {
            GameObject obj = icon_manager._instance.create_reward_icon(t_boss_active.rewrds[i].type, t_boss_active.rewrds[i].value1,
               t_boss_active.rewrds[i].value2, t_boss_active.rewrds[i].value3);
            obj.transform.parent = m_icon.transform;
            obj.transform.localPosition = new Vector3(i * 100,0,0);
            obj.transform.localScale = Vector3.one;

        }
      
    }
   
    void click(GameObject obj)
    {
        if (obj.name == "get")
        {
            s_message _mes = new s_message();
            _mes.m_type = "guild_red_lingqu";
            _mes.m_ints.Add(m_active_id);
            cmessage_center._instance.add_message(_mes);
        }
    }
}
