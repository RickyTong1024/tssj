using System.Collections.Generic;
using UnityEngine;

public class guildfight_target_gui : MonoBehaviour,IMessage {
    public GameObject m_view;
    public protocol.game.smsg_guild_fight_pvp_look look_msg;
    int click_id = 0;




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

    }
    void IMessage.net_message(s_net_message msg)
    {
        if (msg.m_opcode == opclient_t.CMSG_GUILD_JT_REWARD && gameObject.activeSelf)
        {
            look_msg.fight.reward_ids.Add(click_id);
            s_t_guildfight_target _active = game_data._instance.get_t_guildfight_target(click_id);
            sys._instance.m_self.add_reward(_active.reward.type, _active.reward.value1, _active.reward.value2, _active.reward.value3, _active.name + game_data._instance.get_t_language ("guildfight_target_gui.cs_30_149"));//跨服军团战任务获得
            reset();
 
        }
    }
    public void reset()
    {
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        look_msg = juntuan_gui._instance.m_guild_kuafu.GetComponent<juntuan_kuafu_control>().look_msg;
        sys._instance.remove_child(m_view);
        List<int> ids = new List<int>();
        foreach (int key in game_data._instance.m_dbc_guildfight_target.m_index.Keys)
        {
            ids.Add(key);
        }
        ids.Sort(compare);
        int anim = 1;
        sys._instance.remove_child(m_view);
        for (int i = 0; i < ids.Count; i++)
        {
            int id = ids[i];
            s_t_guildfight_target target = game_data._instance.get_t_guildfight_target(id);
            GameObject active = game_data._instance.ins_object_res("ui/juntuan/guild_gongpo_sub");
            active.transform.parent = m_view.transform;
            active.transform.localPosition = new Vector3(0, 154 - i * 110, 0);
            active.transform.localScale = new Vector3(1, 1, 1);
            active.transform.Find("reward").GetComponent<UIButtonMessage>().target = this.gameObject;
            active.transform.Find("reward").GetComponent<UIButtonMessage>().functionName = "click";
            active.name = id + "";
            int state = get_state(id);
            active.transform.Find("name").GetComponent<UILabel>().text = target.name;
            active.transform.Find("des").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guildfight_target_gui.cs_66_77") + sys._instance.m_self.get_name(target.reward.type,target.reward.value1,target.reward.value2,target.reward.value3);//奖励：
            GameObject icon = icon_manager._instance.create_reward_icon(target.reward.type,target.reward.value1,target.reward.value2,target.reward.value3);
            icon.transform.parent = active.transform.Find("icon");
            icon.transform.localPosition = Vector3.zero;
            icon.transform.localScale = Vector3.one;
            if (state == 3)
            {
                active.transform.Find("reward").gameObject.SetActive(true);
                active.transform.Find("yilingqu").gameObject.SetActive(false);
                active.transform.Find("num").gameObject.SetActive(false);

 
            }
            else if (state == 2)
            {
                active.transform.Find("reward").gameObject.SetActive(false);
                active.transform.Find("yilingqu").gameObject.SetActive(false);
                active.transform.Find("num").gameObject.SetActive(true);
                if(target.type == 1)
                {
                    active.transform.Find("num").GetComponent<UILabel>().text = look_msg.fight.judian + "/" + target.num;

                }
                else if (target.type == 2)
                {
                    active.transform.Find("num").GetComponent<UILabel>().text = look_msg.fight.jidi + "/" + target.num;

                }
                else if (target.type == 3)
                {
                    active.transform.Find("num").GetComponent<UILabel>().text = look_msg.fight.perfect + "/" + target.num;
                }

            }
            else
            {
                active.transform.Find("reward").gameObject.SetActive(false);
                active.transform.Find("yilingqu").gameObject.SetActive(true);
                active.transform.Find("num").gameObject.SetActive(false);
 
            }

            if (anim == 1)
            {
                sys._instance.add_pos_anim(active, 0.3f, new Vector3(-300, 0, 0), i * 0.05f);
                sys._instance.add_alpha_anim(active, 0.3f, 0, 1.0f, i * 0.05f);
            }
            else if (anim == 2)
            {
                sys._instance.add_pos_anim(active, 0.3f, new Vector3(-300, 0, 0), i * 0.05f + 0.45f);
                sys._instance.add_alpha_anim(active, 0.3f, 0, 1.0f, i * 0.05f + 0.5f);
            }
            else
            {
                if (active.GetComponent<TweenPosition>() != null)
                {
                    Object.Destroy(active.GetComponent<TweenPosition>());
                }
                if (active.GetComponent<TweenAlpha>() != null)
                {
                    Object.Destroy(active.GetComponent<TweenAlpha>());
                }
                active.GetComponent<UIWidget>().alpha = 1.0f;
            }

        }

    }
    void click(GameObject obj)
    {
        int id = int.Parse(obj.transform.parent.name);
        protocol.game.cmsg_active_reward _msg = new protocol.game.cmsg_active_reward();
        _msg.id = id;
        click_id = id;
        net_http._instance.send_msg<protocol.game.cmsg_active_reward>(opclient_t.CMSG_GUILD_JT_REWARD, _msg);
        
    }
   static int get_state(int id)
    {
        s_t_guildfight_target x_target = game_data._instance.get_t_guildfight_target(id);
        if (x_target.type == 1)
        {
            if (x_target.num <= juntuan_gui._instance.look_msg.fight.judian)
            {
                if (juntuan_gui._instance.look_msg.fight.reward_ids.Contains(id))
                {
                    return 1;//已领取
                }
                else
                {
                    return 3;//可领取
  
                }
            }
        }
        else if (x_target.type == 2)
        {
            if (x_target.num <= juntuan_gui._instance.look_msg.fight.jidi)
            {
                if (juntuan_gui._instance.look_msg.fight.reward_ids.Contains(id))
                {
                    return 1;
                }
                else
                {
                    return 3;

                }

            }

        }
        else if (x_target.type == 3)
        {
            if (x_target.num <= juntuan_gui._instance.look_msg.fight.perfect)
            {
                if (juntuan_gui._instance.look_msg.fight.reward_ids.Contains(id))
                {
                    return 1;
                }
                else
                {
                    return 3;

                }

            }

        }
        return 2;//未完成
    }
    int compare(int x,int y)
    {
        int x_state = get_state(x);
        int y_state = get_state(y);
        if (x_state != y_state)
        {
            return -(x_state - y_state);
        }
        else
        {
            return x - y;
        }
 
    }
    public static bool can_effect()
    {
        foreach (var key in game_data._instance.m_dbc_guildfight_target.m_index.Keys)
        {
            if (get_state((int)key) == 3)
            {
                return true;
            }
 
        }
        return false;

    }

}
