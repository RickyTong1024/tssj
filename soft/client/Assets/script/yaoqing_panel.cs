
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class yaoqing_panel : MonoBehaviour,IMessage {

    public GameObject m_view;
    public UILabel m_code;
    public UILabel m_code1;
    public UILabel m_yaonum;
    public GameObject m_share_panel;
    public GameObject m_friend_gui;
    public UILabel m_text;
    public UISprite m_texture;
	public GameObject m_logo;
    public UILabel m_save;
    public int id;
    protocol.game.smsg_social_look m_msg;
    int get_num(int id)
    {
        s_t_target t_target = game_data._instance.get_t_target(id);
        dhc.social_t temp = null;
        for (int i = 0; i < m_msg.social.Count; i++)
        {
            if (m_msg.social[i].template_id == -1)
            {
                temp = m_msg.social[i];

            }
        }
        int num = 0;
        if (temp != null)
        {
            for (int i = 0; i < temp.invite_players.Count; i++)
            {
                if ((int)temp.invite_levels[i] >= t_target.tjdef1)
                {
                    num++;

                }
            }
        }
        return num;
    }
    int get_state(int id)
    {
        s_t_target t_target = game_data._instance.get_t_target(id);
        dhc.social_t temp = null;
        for (int i = 0; i < m_msg.social.Count; i++)
        {
            if (m_msg.social[i].template_id == -1)
            {
                temp = m_msg.social[i];
 
            }
        }
        int num = 0;
        if (temp != null)
        {
            for (int i = 0; i < temp.invite_players.Count; i++)
            {
                if ((int)temp.invite_levels[i] >= t_target.tjdef1)
                {
                    num++;
 
                }
            }
        }
        if (sys._instance.m_self.m_t_player.finished_tasks.Contains((uint)id))
        {
            return 3;//已领取
        }
        else
        {
            if (num >= t_target.tjnum)
            {
                return 2;//可领取

            }
            else
            {
                return 1;//未完成
            }
        }
 
    }
    int get_invitenum()
    {
        dhc.social_t temp = null;
        for (int i = 0; i < m_msg.social.Count; i++)
        {
            if (m_msg.social[i].template_id == -1)
            {
                temp = m_msg.social[i];

            }
        }
        if (temp != null)
        {
            return temp.invite_players.Count;
        }
        else
        {
            return 0;
        }
 
    }
    string get_code()
    {
        dhc.social_t temp = null;
        for (int i = 0; i < m_msg.social.Count; i++)
        {
            if (m_msg.social[i].template_id == -1)
            {
                temp = m_msg.social[i];

            }
        }
        if (temp != null)
        {
            return temp.name;
        }
        else
        {
            return "";
        }
 
    }
    int compare(int x, int y)
    {
        int x_state = get_state(x);
        int y_state = get_state(y);
        if (x_state != y_state)
        {
            return -get_state(x)  + get_state(y);

        }
        else
        {
            return x - y;
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
    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "save_suc")
        {
            root_gui._instance.show_prompt_dialog_box("[00ff00]" + game_data._instance.get_t_language ("yaoqing_panel.cs_154_67"));//保存成功！
        }
 
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_PLAYER_TASK)
        {
            sys._instance.m_self.m_t_player.finished_tasks.Add((uint)id);
            s_t_target t_target = game_data._instance.get_t_target(id);
            sys._instance.m_self.add_reward(t_target.reward.type, t_target.reward.value1, t_target.reward.value2, t_target.reward.value3,game_data._instance.get_t_language ("yaoqing_panel.cs_164_137"));//好友邀请任务奖励
            reset(m_msg);
        }
 
    }
    public void reset(protocol.game.smsg_social_look m_msg)
    {
        this.m_msg = m_msg;
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }

        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
        if (get_code() == "")
        {
            m_code.text = game_data._instance.get_t_language ("yaoqing_panel.cs_182_26");//30级可生成邀请码
        }
        else
        {
            m_code.text = game_data._instance.get_t_language ("yaoqing_panel.cs_186_26") + get_code();//我的邀请码：
            m_code1.text = get_code();
        }
        m_yaonum.text = game_data._instance.get_t_language ("yaoqing_panel.cs_189_24") + get_invitenum();//已邀请人数：
        m_save.text = game_data._instance.get_t_language ("yaoqing_panel.cs_191_22");//保存到手机
        List<int> ids = new List<int>();
       foreach(int id in game_data._instance.m_dbc_target.m_index.Keys)
       {
            s_t_target t_target = game_data._instance.get_t_target(id);
            if (t_target == null)
            {
                continue;
            }
            if (t_target.type == 4)
            {

                ids.Add(id);
 
            }
       }
       ids.Sort(compare);
       int n = 0;
       List<int> qianzhi = new List<int>();
       foreach (int id in ids)
       {
           
           s_t_target t_target = game_data._instance.get_t_target(id);
           
           int state = get_state(id);
           if (state != 3)
           {
               if (qianzhi.Contains(t_target.tjnum))
               {
                   continue;
               }
               else
               {
                   qianzhi.Add(t_target.tjnum);
               }
               GameObject target = game_data._instance.ins_object_res("ui/yaoqing_sub");
               UIEventListener.Get(target).onClick = click;
               target.name = t_target.id + "";
               target.transform.parent = m_view.transform;
               target.transform.localPosition = new Vector3(0, 118 - n * 110 , 0);
               target.transform.localScale = new Vector3(1, 1, 1);
               target.transform.Find("desc").GetComponent<UILabel>().text = t_target.desc;
               target.transform.Find("wcd").GetComponent<UILabel>().text = get_num(id) + "/" + t_target.tjnum;
               string text1 = game_data._instance.get_t_language ("active.cs_45_16")+ ":";//奖励
		       string text = text1 + sys._instance.get_res_info 
                   (t_target.reward.type, t_target.reward.value1, t_target.reward.value2, t_target.reward.value3);
              target.transform.Find("reward").GetComponent<UILabel>().text = text;
              target.transform.Find("gou").gameObject.SetActive(false);
              target.transform.Find("icon").GetComponent<UISprite>().spriteName = t_target.icon;
               if (state == 2)
               {
                   target.transform.Find("gou").gameObject.SetActive(true);
                   target.transform.Find("main1").gameObject.SetActive(true);
                   target.transform.Find("main").gameObject.SetActive(false);
                   
 
               }
               else if (state == 1)
               {
                   target.transform.Find("main1").gameObject.SetActive(false);
                   target.transform.Find("main").gameObject.SetActive(true);
 
               }
               n ++;
           }

       }
       m_friend_gui.GetComponent<friend_gui>().update_button();

    }
    void click(GameObject obj)
    {
        if (obj.name == "share")
        {
            m_texture.spriteName = "yqhd_sjtp01";//yqhd_sjtp01
            m_text.gameObject.SetActive(true);
            m_logo.SetActive(true);
#if UNITY_IPHONE
            m_texture.gameObject.SetActive(true);
#endif
            m_share_panel.SetActive(true);
        }
        else if (obj.name == "close_share")
        {
            m_share_panel.transform.Find("frame_big").GetComponent<frame>().hide();
 
        }
        else if (obj.name == "save")
        {
            platform._instance.copy(get_code());
        }
        else
        {
            id = int.Parse(obj.name);
            if (get_state(id) == 2)
            {
                s_t_target t_target = game_data._instance.get_t_target(id);
                protocol.game.cmsg_player_task _msg = new protocol.game.cmsg_player_task();
                _msg.task_id = (uint)id;
                net_http._instance.send_msg<protocol.game.cmsg_player_task>(opclient_t.CMSG_PLAYER_TASK, _msg);
            }
        }
    }
}
