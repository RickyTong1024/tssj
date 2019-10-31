using UnityEngine;

public class guildboss_active : MonoBehaviour {

    public int m_num;
    public GameObject m_icon;

    public s_t_guild_mission m_mission;
    public int id;
    public protocol.game.smsg_guild_mission_look m_msg;
    public int flag;
    void Start()
    {

    }
    
    public void reset()
    {
        if (flag == 3)
        {
            transform.Find("wcd").GetComponent<UILabel>().text = "[ffffff]" + game_data._instance.get_t_language ("active.cs_19_75");//目标达成
            transform.Find("gou").gameObject.SetActive(true);
            transform.Find("main").gameObject.SetActive(false);
            transform.Find("main1").gameObject.SetActive(true);
            transform.Find("yi").gameObject.SetActive(false);
            
        }
        else if (flag == 2)
        {
            transform.Find("wcd").GetComponent<UILabel>().text = "[24d0fd]" + game_data._instance.get_t_language ("guildboss_active.cs_36_83");//未达成
            transform.Find("gou").gameObject.SetActive(false);
            transform.Find("main").gameObject.SetActive(true);
            transform.Find("main1").gameObject.SetActive(false);
            transform.Find("yi").gameObject.SetActive(false);

        }
        else
        {
            transform.Find("wcd").GetComponent<UILabel>().text = "[24d0fd]" + "";
            transform.Find("gou").gameObject.SetActive(false);
            transform.Find("main").gameObject.SetActive(true);
            transform.Find("main1").gameObject.SetActive(false);
            transform.Find("yi").gameObject.SetActive(true);
 
        }
       
    }


    // Update is called once per frame
    void Update()
    {

    }
    string get_name(int id)
    {
        string s = "";
        switch (id)
        {
            case 1:
                s = game_data._instance.get_t_language ("guildboss_active.cs_67_20");//击败坚苍守卫
                break;
            case 2:
                s = game_data._instance.get_t_language ("guildboss_active.cs_70_20");//击败韧战守卫
                break;
            case 3:
                s = game_data._instance.get_t_language ("guildboss_active.cs_73_20");//击败迅霆守卫
                break;
            case 4:
                s = game_data._instance.get_t_language ("guildboss_active.cs_76_20");//击败布洛守卫
                break;
        }
        return s;
    }
}
