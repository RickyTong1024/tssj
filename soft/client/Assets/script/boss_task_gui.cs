using UnityEngine;

public class boss_task_gui : MonoBehaviour, IMessage
{
    public GameObject m_view;
    public GameObject m_yq_button;
    public GameObject m_sh;
    public GameObject m_qf;
    public GameObject m_sh_point;
    public GameObject m_qf_point;
    int click_id = 0;
    int m_select = 1;

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
        protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_BOSS_ACTIVE_LOOK, _msg);
    }

    public void reset(int anim)
    {
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }

        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        sys._instance.remove_child(m_view);

        int tnum = 0;
        for (int i = 0; i < game_data._instance.m_dbc_boss_active.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_boss_active.get(0, i));
            s_t_boss_active t_boss_active = game_data._instance.get_t_boss_active(id);
            if (!active_vis(t_boss_active))
            {
                continue;
            }
            if (t_boss_active.task_type != m_select)
            {
                continue;
            }
            if (!active_done(t_boss_active))
            {
                continue;
            }
            if (sys._instance.is_hide_reward(t_boss_active.type, t_boss_active.value1))
            {
                continue;
            }
            long num = get_active_num(t_boss_active);

            GameObject active = game_data._instance.ins_object_res("ui/boss_task");
            active.transform.parent = m_view.transform;
            active.transform.localPosition = new Vector3(0, 117 - tnum * 110, 0);
            active.transform.localScale = new Vector3(1, 1, 1);
            active.transform.GetComponent<boss_active>().m_active_id = t_boss_active.id;
            active.transform.GetComponent<boss_active>().m_damage = num;
            active.transform.GetComponent<boss_active>().task_damage = get_damage(t_boss_active);
            active.transform.GetComponent<boss_active>().reward = get_reward(t_boss_active);
            active.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
            active.transform.GetComponent<boss_active>().reset();

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
        for (int i = 0; i < game_data._instance.m_dbc_boss_active.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_boss_active.get(0, i));
            s_t_boss_active t_boss_active = game_data._instance.get_t_boss_active(id);
            if (!active_vis(t_boss_active))
            {
                continue;
            }
            if (t_boss_active.task_type != m_select)
            {
                continue;
            }
            if (active_done(t_boss_active))
            {
                continue;
            }
            if (sys._instance.is_hide_reward(t_boss_active.type, t_boss_active.value1))
            {
                continue;
            }
            long num = get_active_num(t_boss_active);

            GameObject active = game_data._instance.ins_object_res("ui/boss_task");
            active.transform.parent = m_view.transform;
            active.transform.localPosition = new Vector3(0, 117 - tnum * 110, 0);
            active.transform.localScale = new Vector3(1, 1, 1);
            active.transform.GetComponent<boss_active>().m_active_id = t_boss_active.id;
            active.transform.GetComponent<boss_active>().m_damage = num;
            active.transform.GetComponent<boss_active>().task_damage = get_damage(t_boss_active);
            active.transform.GetComponent<boss_active>().reward = get_reward(t_boss_active);
            active.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
            active.transform.GetComponent<boss_active>().reset();

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
        if (point_effect(m_select))
        {
            m_yq_button.GetComponent<BoxCollider>().enabled = true;
            m_yq_button.GetComponent<UISprite>().set_enable(true);
        }
        else
        {
            m_yq_button.GetComponent<BoxCollider>().enabled = false;
            m_yq_button.GetComponent<UISprite>().set_enable(false);
        }
        if (point_effect(1))
        {
            m_sh_point.SetActive(true);
        }
        else
        {
            m_sh_point.SetActive(false);
        }
        if (point_effect(2))
        {
            m_qf_point.SetActive(true);
        }
        else
        {
            m_qf_point.SetActive(false);
        }
    }

    public long get_active_num(s_t_boss_active t_boss_active)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.boss_active_ids.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.boss_active_ids[i] == t_boss_active.id)
            {
                return sys._instance.m_self.m_t_player.boss_active_nums[i];
            }
        }
        return 0;
    }

    static public bool active_vis(s_t_boss_active t_active)
    {
        if (sys._instance.m_self.m_t_player.boss_active_rewards.Count == 0)
        {
            return true;
        }
        for (int i = 0; i < sys._instance.m_self.m_t_player.boss_active_ids.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.boss_active_ids[i] == t_active.id)
            {
                if (sys._instance.m_self.m_t_player.boss_active_rewards[i] == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return true;
    }

    static public bool active_done(s_t_boss_active t_boss_active)
    {
        long damage = 0;

        for (int i = 0; i < sys._instance.m_self.m_t_player.boss_active_ids.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.boss_active_ids[i] == t_boss_active.id)
            {
                damage = sys._instance.m_self.m_t_player.boss_active_nums[i];
                break;
            }
        }

        if (damage >= get_damage(t_boss_active))
        {
            return true;
        }
        return false;
    }

    static public int get_damage(s_t_boss_active t_boss_active)
    {
        int damage = 0;
        if (t_boss_active.task_type == 1)
        {
            for (int i = 0; i < game_data._instance.m_dbc_boss_dw.get_y(); ++i)
            {
                int id = int.Parse(game_data._instance.m_dbc_boss_dw.get(0, i));
                s_t_boss_dw _t_boss_dw = game_data._instance.get_t_boss_dw(id);
                if (sys._instance.m_self.m_t_player.boss_player_level <= _t_boss_dw.level2)
                {
                    damage = t_boss_active.count * _t_boss_dw.base_hurt;
                    break;
                }

            }
        }
        else if (t_boss_active.task_type == 2)
        {
            damage = t_boss_active.count;
        }
        return damage;
    }

    public int get_reward(s_t_boss_active t_boss_active)
    {
        int damage = 0;
        foreach (int id in game_data._instance.m_dbc_boss_dw.m_index.Keys)
        {
            s_t_boss_dw _t_boss_dw = game_data._instance.get_t_boss_dw(id);
            if (sys._instance.m_self.m_t_player.boss_player_level <= _t_boss_dw.level2)
            {
                if (t_boss_active.task_type == 1)
                {
                    damage = t_boss_active.value2 + (_t_boss_dw.dw * t_boss_active.ex_add);
                }
                else
                {
                    damage = t_boss_active.value2;
                }
                break;
            }
        }
        return damage;
    }

    public bool point_effect(int type)
    {
        for (int i = 0; i < game_data._instance.m_dbc_boss_active.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_boss_active.get(0, i));
            s_t_boss_active t_boss_active = game_data._instance.get_t_boss_active(id);
            if (t_boss_active.task_type != type)
            {
                continue;
            }
            if (active_done(t_boss_active) && active_vis(t_boss_active))
            {
                return true;
            }
        }
        return false;
    }

    public static bool effect()
    {
        foreach (int id in game_data._instance.m_dbc_boss_active.m_index.Keys)
        {
            s_t_boss_active t_boss_active = game_data._instance.get_t_boss_active(id);
            if (active_done(t_boss_active) && active_vis(t_boss_active))
            {
                return true;
            }
        }
        return false;
    }

    public void click(GameObject obj)
    {
        if (obj.name == "boss_task(Clone)")
        {
            click_id = obj.transform.GetComponent<boss_active>().m_active_id;
            s_t_boss_active t_boss_active = game_data._instance.get_t_boss_active(click_id);
            if (active_done(t_boss_active))
            {
                protocol.game.cmsg_active_reward _msg = new protocol.game.cmsg_active_reward();
                _msg.id = click_id;
                net_http._instance.send_msg<protocol.game.cmsg_active_reward>(opclient_t.CMSG_BOSS_ACTIVE_REWARD, _msg);
            }
        }
        if (obj.name == "close")
        {
            m_sh.GetComponent<UIToggle>().value = true;
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        if (obj.name == "yq")
        {
            click_id = 0;
            protocol.game.cmsg_active_reward _msg = new protocol.game.cmsg_active_reward();
            _msg.id = m_select;
            net_http._instance.send_msg<protocol.game.cmsg_active_reward>(opclient_t.CMSG_BOSS_ACTIVE_REWARD, _msg);
        }
        if (obj.transform.name == "sh")
        {
            m_select = 1;
            m_sh.GetComponent<UIToggle>().value = true;
            reset(1);
        }
        if (obj.transform.name == "qf")
        {
            m_select = 2;
            m_qf.GetComponent<UIToggle>().value = true;
            reset(1);
        }
    }

    void IMessage.message(s_message message)
    {

    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_BOSS_ACTIVE_REWARD)
        {
            protocol.game.smsg_active_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_active_reward>(message.m_byte);
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i], true);
            }
            if (click_id != 0)
            {
                s_t_boss_active t_boss_active = game_data._instance.get_t_boss_active(click_id);
                for (int i = 0; i < sys._instance.m_self.m_t_player.boss_active_ids.Count; ++i)
                {
                    if (sys._instance.m_self.m_t_player.boss_active_ids[i] == click_id)
                    {
                        sys._instance.m_self.m_t_player.boss_active_rewards[i] = 1;
                    }
                }
                for (int i = 0; i < _msg.types.Count; ++i)
                {
                    sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i], game_data._instance.get_t_language("boss_task_gui.cs_390_102"));//boss战任务获得
                }
            }
            else
            {
                for (int i = 0; i < game_data._instance.m_dbc_boss_active.get_y(); ++i)
                {
                    int id = int.Parse(game_data._instance.m_dbc_boss_active.get(0, i));
                    s_t_boss_active t_boss_active = game_data._instance.get_t_boss_active(id);
                    if (active_done(t_boss_active) && active_vis(t_boss_active) && t_boss_active.task_type == m_select)
                    {
                        for (int j = 0; j < sys._instance.m_self.m_t_player.boss_active_ids.Count; ++j)
                        {
                            if (sys._instance.m_self.m_t_player.boss_active_ids[j] == id)
                            {
                                sys._instance.m_self.m_t_player.boss_active_rewards[j] = 1;
                            }
                        }
                    }
                }
                for (int i = 0; i < _msg.types.Count; ++i)
                {
                    sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i], game_data._instance.get_t_language("boss_task_gui.cs_390_102"));//boss战任务获得
                }
            }
            s_message _mes = new s_message();
            _mes.m_type = "show_boss_task_effect";
            cmessage_center._instance.add_message(_mes);
            reset(0);
        }
        if (message.m_opcode == opclient_t.CMSG_BOSS_ACTIVE_LOOK)
        {
            protocol.game.smsg_boss_active_look _msg = net_http._instance.parse_packet<protocol.game.smsg_boss_active_look>(message.m_byte);
            sys._instance.m_self.m_t_player.boss_active_ids.Clear();
            sys._instance.m_self.m_t_player.boss_active_nums.Clear();
            sys._instance.m_self.m_t_player.boss_active_rewards.Clear();
            for (int i = 0; i < _msg.ids.Count; ++i)
            {
                sys._instance.m_self.m_t_player.boss_active_ids.Add(_msg.ids[i]);
            }
            if (_msg.nums.Count != _msg.ids.Count)
            {
                for (int i = 0; i < _msg.ids.Count; ++i)
                {
                    sys._instance.m_self.m_t_player.boss_active_nums.Add(0);
                }
            }
            else
            {
                for (int i = 0; i < _msg.nums.Count; ++i)
                {
                    sys._instance.m_self.m_t_player.boss_active_nums.Add(_msg.nums[i]);
                }
            }
            if (_msg.rewards.Count != _msg.ids.Count)
            {
                for (int i = 0; i < _msg.ids.Count; ++i)
                {
                    sys._instance.m_self.m_t_player.boss_active_rewards.Add(0);
                }
            }
            else
            {
                for (int i = 0; i < _msg.rewards.Count; ++i)
                {
                    sys._instance.m_self.m_t_player.boss_active_rewards.Add(_msg.rewards[i]);
                }
            }
            sys._instance.m_self.m_t_player.boss_player_level = _msg.level;
            m_select = 1;
            reset(1);
        }
    }
}
