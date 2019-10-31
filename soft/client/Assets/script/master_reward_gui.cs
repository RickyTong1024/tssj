
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class master_reward_gui : MonoBehaviour,IMessage{

    public GameObject m_view;
    int click_id = 0;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	
	}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    public void reset(int anim)
    {
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        List<int> master_ids = new List<int>();
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);

        int tnum = 0;
        foreach(int id in game_data._instance.m_master_targets.Keys)
        {
            master_ids.Add(id);
        }
        master_ids.Sort(com);
        for (int i = 0; i < master_ids.Count; i++)
        {
            int id = master_ids[i];
            s_t_master_target t_pvp_active = game_data._instance.get_t_master_target(id);
            GameObject active = game_data._instance.ins_object_res("ui/master_task");
            active.transform.parent = m_view.transform;
            active.transform.localPosition = new Vector3(0, 178 - tnum * 110, 0);
            active.transform.localScale = new Vector3(1, 1, 1);
            active.transform.GetComponent<master_sub>().m_active_id = t_pvp_active.id;
            active.transform.GetComponent<master_sub>().reset();
            active.GetComponent<UIButtonMessage>().target = this.gameObject;
            active.GetComponent<UIButtonMessage>().functionName = "click";

            if (anim == 1)
            {
                sys._instance.add_pos_anim(active, 0.3f, new Vector3(-300, 0, 0), tnum * 0.05f);
                sys._instance.add_alpha_anim(active, 0.3f, 0, 1.0f, tnum * 0.05f);
            }
            else if (anim == 2)
            {
                sys._instance.add_pos_anim(active, 0.3f, new Vector3(-300, 0, 0), tnum * 0.05f + 0.45f);
                sys._instance.add_alpha_anim(active, 0.3f, 0, 1.0f, tnum * 0.05f + 0.5f);
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

            tnum++;
        }

    }
    static int com(int x, int y)
    {
        if (get_state(x) != get_state(y))
        {
            return get_state(y) - get_state(x);

        }
        else
        {
            return x - y;
        }

    }
    public static int get_state(int id)
    {
        s_t_master_target pvp_active = game_data._instance.get_t_master_target(id);
        if (sys._instance.m_self.m_t_player.ds_hit < pvp_active.count)
        {
            return 2;//未完成
        }
        else
        {
            if (is_lingqu(id))
            {
                return 1;//已领取
            }
            else
            {
                return 3;//未领取
            }
        }
    }
    public static bool is_effect()
    {
        foreach(int id in game_data._instance.m_master_targets.Keys)
        {
            if (get_state(id) == 3)
            {
                return true;
            }
        }
        return false;

    }
    static bool is_lingqu(int id)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.ds_hit_ids.Count; i++)
        {
            if (id == sys._instance.m_self.m_t_player.ds_hit_ids[i])
            {
                return true;
            }
        }
        return false;
    }
    public void click(GameObject obj)
    {
        if (obj.name == "master_task(Clone)")
        {
            click_id = obj.GetComponent<master_sub>().m_active_id;
            if (get_state(click_id) == 3)
            {
                protocol.game.cmsg_active_reward _msg = new protocol.game.cmsg_active_reward();
                _msg.id = click_id;
                net_http._instance.send_msg<protocol.game.cmsg_active_reward>(opclient_t.CMSG_DS_ACTIVE, _msg);
            }
        }
        if (obj.name == "close")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }

    }
    void IMessage.message(s_message mes)
    {
 
    }
    void IMessage.net_message(s_net_message msg)
    {
        if (msg.m_opcode == opclient_t.CMSG_DS_ACTIVE)
        {
            sys._instance.m_self.m_t_player.ds_hit_ids.Add(click_id);
            s_t_master_target _active = game_data._instance.get_t_master_target(click_id);
            sys._instance.m_self.add_reward(_active.type,_active.value1,_active.value2,_active.value3,_active.name + game_data._instance.get_t_language ("master_reward_gui.cs_158_117"));//大师联赛任务获得
            s_message _mes = new s_message();
            _mes.m_type = "refresh_master";
            cmessage_center._instance.add_message(_mes);
            reset(1);
        }
 
    }
	
}
