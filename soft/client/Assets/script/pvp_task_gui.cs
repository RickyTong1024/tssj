
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class pvp_task_gui : MonoBehaviour,IMessage {

    public GameObject m_view;
    public GameObject m_yq_button;
    int click_id = 0;
    int m_select = 1;

    // Use this for initialization
    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    public void OnEnable()
    {
       
    }

    public void reset(int anim)
    {
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        List<int> pvp_ids = new List<int>();
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);

        int tnum = 0;
        for (int i = 0; i < game_data._instance.m_dbc_pvp_active.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_pvp_active.get(0, i));
            pvp_ids.Add(id);
        }
        pvp_ids.Sort(com);
        for(int i = 0 ;i < pvp_ids.Count;i ++)
        {
            int id = pvp_ids[i];
            s_t_pvp_active t_pvp_active = game_data._instance.get_t_pvp_active(id);
            GameObject active = game_data._instance.ins_object_res("ui/pvp_task");
            active.transform.parent = m_view.transform;
            active.transform.localPosition = new Vector3(0, 117 - tnum * 110, 0);
            active.transform.localScale = new Vector3(1, 1, 1);
            active.transform.GetComponent<pvp_active>().m_active_id = t_pvp_active.id;
            active.transform.GetComponent<pvp_active>().reset();
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
        s_t_pvp_active pvp_active = game_data._instance.get_t_pvp_active(id);
        if (sys._instance.m_self.m_t_player.pvp_hit < pvp_active.neednum)
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
        for (int i = 0; i < game_data._instance.m_dbc_pvp_active.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_pvp_active.get(0, i));

            if (get_state(id) == 3)
            {
                return true;
            }
        }
        return false;
 
    }
    static bool is_lingqu(int id)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.pvp_hit_ids.Count; i++)
        {
            if (id == sys._instance.m_self.m_t_player.pvp_hit_ids[i])
            {
                return true;
            }
        }
        return false;
    }
    public void click(GameObject obj)
    {
        if (obj.name == "pvp_task(Clone)")
        {
            click_id = obj.GetComponent<pvp_active>().m_active_id;
           if(get_state(click_id) == 3)
            {
                protocol.game.cmsg_active_reward _msg = new protocol.game.cmsg_active_reward();
                _msg.id = click_id;
                net_http._instance.send_msg<protocol.game.cmsg_active_reward>(opclient_t.CMSG_PVP_ACTIVE, _msg);
            }
        }
        if (obj.name == "close")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        if (obj.name == "yq")
        {
            click_id = 0;
            protocol.game.cmsg_active_reward _msg = new protocol.game.cmsg_active_reward();
            _msg.id = m_select;
            net_http._instance.send_msg<protocol.game.cmsg_active_reward>(opclient_t.CMSG_BOSS_ACTIVE_REWARD, _msg);
        }

    }

    void IMessage.message(s_message message)
    {

    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_PVP_ACTIVE)
        {
            sys._instance.m_self.m_t_player.pvp_hit_ids.Add(click_id);
            s_t_pvp_active _active = game_data._instance.get_t_pvp_active(click_id);
            sys._instance.m_self.add_att(e_player_attr.player_pvp_jz, _active.lieb);
            s_message _mes = new s_message();
            _mes.m_type = "refresh_pvp";
            cmessage_center._instance.add_message(_mes);
            reset(0);
        }
        
    }
    // Update is called once per frame
    void Update()
    {

    }
}
