using System.Collections.Generic;
using UnityEngine;

public class catch_card_show : MonoBehaviour, IMessage
{
    public protocol.game.smsg_chouka m_msg;
    public GameObject m_icon_root;
    public GameObject m_close_panel;
    private int m_type;
    private int m_num;
    private float m_time;
    private List<s_t_reward> m_rewards;
    private List<int> m_rewards_pz;
    public List<Vector3> m_v = new List<Vector3>();
    public GameObject m_label;
    public GameObject m_shell;
    private List<GameObject> m_objs;
    public GameObject m_again;
    private bool m_start;
    public string m_message;
    bool flag = false;

    private bool m_show_role = false;
    private s_t_reward m_show;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void OnEnable()
    {
        m_start = false;
        flag = false;
        m_close_panel.SetActive(false);
        sys._instance.remove_child(m_icon_root);
    }

    public void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            this.GetComponent<ui_show_anim>().hide_ui();

            s_message _message = new s_message();
            _message.m_type = m_message;
            cmessage_center._instance.add_message(_message);
        }
        if (obj.name == "again")
        {
            if (m_msg.type == 1)
            {
                if (sys._instance.m_self.get_item_num(80010001) < 1)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("buy_num_gui_ex.cs_224_71"));//道具不足
                    return;
                }
            }
            else if (m_msg.type == 10)
            {
                if (sys._instance.m_self.get_item_num(80010001) < 10)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("buy_num_gui_ex.cs_224_71"));//道具不足
                    return;
                }
            }
            else if (m_msg.type == 2)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA) && sys._instance.m_self.get_item_num(80010002) < 1)
                {
                    root_gui._instance.show_recharge_dialog_box(delegate ()
                    {
                        this.GetComponent<ui_show_anim>().hide_ui();
                    });
                    return;
                }
            }
            else if (m_msg.type == 20)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN) && sys._instance.m_self.get_item_num(80010002) < 10)
                {
                    root_gui._instance.show_recharge_dialog_box(delegate ()
                    {
                        this.GetComponent<ui_show_anim>().hide_ui();
                    });
                    return;
                }
            }
            else if (m_msg.type == 3)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 10)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("catch_card_gui.cs_337_56"));//道具不足
                    return;
                }
            }
            else if (m_msg.type == 30)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 100)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("catch_card_gui.cs_337_56"));//道具不足
                    return;
                }
            }

            this.GetComponent<ui_show_anim>().hide_ui();
            if (m_message == "show_catch_gui")
            {
                s_message message = new s_message();
                message.m_type = "again_catch_card_show";
                message.m_ints.Add(m_msg.type);
                cmessage_center._instance.add_message(message);
            }
        }
    }

    void end()
    {
        int num = m_msg.item_ids.Count;
        m_rewards = new List<s_t_reward>();
        m_rewards_pz = new List<int>();
        for (int i = 0; i < num; ++i)
        {
            int a = Random.Range(0, num - i);
            int pz = 0;
            if (m_msg.item_ids[a] != 0 && m_msg.item_nums[a] != 0)
            {
                s_t_item t_item = game_data._instance.get_item(m_msg.item_ids[a]);
                if (t_item.font_color >= 4)
                {
                    pz = 1;
                }

                s_t_reward t_reward = new s_t_reward();
                t_reward.type = 2;
                t_reward.value1 = m_msg.item_ids[a];
                t_reward.value2 = m_msg.item_nums[a];
                t_reward.value3 = 0;
                m_rewards.Add(t_reward);
                m_rewards_pz.Add(pz);
            }
            if (m_msg.item_ids[a] != 0 && m_msg.item_nums[a] == 0)
            {
                s_t_item t_item = game_data._instance.get_item(m_msg.item_ids[a]);
                s_t_class t_class = game_data._instance.get_t_class(t_item.def_1);
                if (t_class.color >= 4)
                {
                    pz = 1;
                }

                s_t_reward t_reward = new s_t_reward();
                t_reward.type = 3;
                t_reward.value1 = t_class.id;
                t_reward.value2 = 0;
                t_reward.value3 = 0;
                m_rewards.Add(t_reward);
                m_rewards_pz.Add(pz);
            }
            m_msg.item_ids.RemoveAt(a);
            m_msg.item_nums.RemoveAt(a);

        }
        if (num == 1)
        {
            m_type = 0;

            m_time = 0.4f;
        }
        else
        {
            m_type = 1;
            m_time = 3.1f;
        }

        m_num = 0;
        m_objs = new List<GameObject>();
        m_start = true;
    }

    public void update_ui()
    {
        int num = sys._instance.m_self.get_item_num(80010001);
        int num2 = sys._instance.m_self.get_item_num(80010002);
        if (m_msg.type == 2)
        {
            if (num2 > 0)
            {
                m_again.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_gojzmx";

                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[41b2ff]x1";

            }
            else
            {
                m_again.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA))
                {
                    m_again.transform.Find("jewel").GetComponent<UILabel>().text = string.Format("[ff0000]x{0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA));
                }
                else
                {
                    m_again.transform.Find("jewel").GetComponent<UILabel>().text = string.Format("[41b2ff]x{0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA));
                }
            }
        }
        else if (m_msg.type == 20)
        {
            if (num2 >= 10)
            {
                m_again.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_gojzmx";

                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[41b2ff]x10";

            }
            else
            {
                m_again.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN))
                {
                    m_again.transform.Find("jewel").GetComponent<UILabel>().text = string.Format("[ff0000]x{0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN));
                }
                else
                {
                    m_again.transform.Find("jewel").GetComponent<UILabel>().text = string.Format("[41b2ff]x{0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN));
                }
            }
        }
        else if (m_msg.type == 3)
        {
            m_again.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_yqzomx";
            if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 10)
            {
                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[ff0000]x10";
            }
            else
            {
                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[41b2ff]x10";
            }
        }
        else if (m_msg.type == 30)
        {
            m_again.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_yqzomx";
            if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 100)
            {
                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[ff0000]x100";
            }
            else
            {
                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[41b2ff]x100";
            }
        }
        else if (m_msg.type == 10)
        {
            m_again.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_jin";
            if (num < 10)
            {
                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[ff0000]x 10";
            }
            else
            {
                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[f6d446]x 10";
            }
        }
        else
        {
            m_again.transform.Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_jin";
            if (num < 1)
            {
                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[ff0000]x 1";
            }
            else
            {
                m_again.transform.Find("jewel").GetComponent<UILabel>().text = "[f6d446]x 1";
            }
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "update_catch_card_show")
        {
            update_ui();
        }
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_ROLE_DUIHUAN)
        {
            protocol.game.smsg_role_duihuan _msg = net_http._instance.parse_packet<protocol.game.smsg_role_duihuan>(message.m_byte);

            if (sys._instance.m_self.has_card(_msg.role.template_id))
            {
                return;
            }

            sys._instance.m_self.add_card(_msg.role, false);

            if (m_show_role)
            {
                m_show_role = false;
                s_message _mes2 = new s_message();
                _mes2.m_type = "hide_catch_card";
                cmessage_center._instance.add_message(_mes2);

                s_message _mes1 = new s_message();
                _mes1.m_type = "show_catch_card";

                ccard _card = sys._instance.m_self.get_card_guid(_msg.role.guid);

                s_message _message = new s_message();
                _message.m_type = "show_zhaomu_shuxing";
                _message.m_object.Add(_card);
                _message.m_object.Add(_mes1);
                cmessage_center._instance.add_message(_message);
            }
        }
    }

    void dui_huan(s_t_reward t_reward)
    {
        if (t_reward.type == 3 && m_show_role)
        {
            sys._instance.m_self.add_card(m_msg.roles[0], false);
            flag = true;
            if (m_show_role)
            {
                m_show_role = false;
                s_message _mes2 = new s_message();
                _mes2.m_type = "hide_catch_card";
                cmessage_center._instance.add_message(_mes2);

                s_message _mes1 = new s_message();
                _mes1.m_type = "show_catch_card";

                ccard _card = sys._instance.m_self.get_card_guid(m_msg.roles[0].guid);

                s_message _message = new s_message();
                _message.m_type = "show_zhaomu_shuxing";
                _message.m_object.Add(_card);
                _message.m_object.Add(_mes1);
                cmessage_center._instance.add_message(_message);
            }
        }
        else if (t_reward.type == 3 && !m_show_role)
        {
            for (int i = 0; i < m_msg.roles.Count; ++i)
            {
                if (m_msg.roles[i].template_id == t_reward.value1)
                {
                    sys._instance.m_self.add_card(m_msg.roles[i], false);
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (!m_start)
        {
            return;
        }
        for (int i = 0; i < m_objs.Count; ++i)
        {
            m_objs[i].transform.localEulerAngles = new Vector3(0, 0, m_objs[i].transform.localEulerAngles.z + Time.deltaTime * 720);
        }

        if (m_time > 0)
        {
            m_time -= Time.deltaTime;

            if (m_type == 0)
            {
                if (m_num == 0 && m_time <= 0.3f)
                {
                    GameObject icon = null;
                    m_show = m_rewards[0];
                    icon = icon_manager._instance.create_reward_icon_ex(m_rewards[0].type, m_rewards[0].value1, m_rewards[0].value2, m_rewards[0].value3);
                    icon.transform.parent = m_icon_root.transform;
                    icon.transform.localPosition = new Vector3(0, -20, 0);
                    icon.transform.localScale = new Vector3(1, 1, 1);
                    m_objs.Add(icon);
                    TweenPosition effect = sys._instance.add_pos_anim(icon, 0.3f, new Vector3(0, 230, 0), 0);
                    EventDelegate.Add(effect.onFinished, delegate ()
                    {
                        string _name = "";
                        if (m_rewards[0].type == 2)
                        {
                            s_t_item item = game_data._instance.get_item(m_rewards[0].value1);
                            _name = item.name;
                        }
                        else if (m_rewards[0].type == 3)
                        {
                            s_t_class _class = game_data._instance.get_t_class(m_rewards[0].value1);
                            _name = _class.name;
                        }
                        m_show_role = true;
                        dui_huan(m_show);
                        icon.transform.localEulerAngles = new Vector3(0, 0, 0);

                        GameObject obj = (GameObject)Instantiate(m_label);
                        obj.transform.parent = icon.transform;
                        obj.transform.localPosition = new Vector3(0, 0, 0);
                        obj.transform.localScale = new Vector3(1, 1, 1);

                        card_icon _icon = effect.gameObject.GetComponent<card_icon>();

                        if (_icon != null)
                        {
                            _name = _icon.m_class.name;
                        }
                        if (m_rewards[0].type == 2)
                        {
                            s_t_item item = game_data._instance.get_item(m_rewards[0].value1);
                            obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(_name, item.font_color);
                        }
                        else if (m_rewards[0].type == 3)
                        {
                            s_t_class _class = game_data._instance.get_t_class(m_rewards[0].value1);
                            obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(_name, _class.color);
                        }
                        obj.SetActive(true);

                        if (m_rewards_pz[0] == 1)
                        {
                            obj = (GameObject)Instantiate(m_shell);
                            obj.transform.parent = icon.transform;
                            obj.transform.localPosition = new Vector3(0, 0, 0);
                            obj.transform.localScale = new Vector3(1, 1, 1);
                            obj.SetActive(true);
                        }
                        m_objs.Remove(icon);
                    });
                    m_num = 1;
                }
            }
            else
            {
                for (int i = 0; i < 10; ++i)
                {
                    if (m_num == i && m_time <= 3.0f - i * 0.3f)
                    {
                        GameObject icon = null;

                        if (m_rewards[i].value3 > 0)
                        {
                            icon = icon_manager._instance.create_card_icon_ex(m_rewards[i].value3, 0, 0, 0);
                        }
                        else
                        {
                            icon = icon_manager._instance.create_reward_icon_ex(m_rewards[i].type, m_rewards[i].value1, m_rewards[i].value2, m_rewards[i].value3);
                        }
                        dui_huan(m_rewards[i]);
                        icon.transform.parent = m_icon_root.transform;
                        icon.transform.localPosition = m_v[i];
                        icon.transform.localScale = new Vector3(1, 1, 1);

                        m_objs.Add(icon);

                        Vector3 v = new Vector3(-m_v[i].x, -m_v[i].y + 210, -m_v[i].z);

                        TweenPosition effect = sys._instance.add_pos_anim(icon, 0.3f, v, 0);
                        int num = i;
                        EventDelegate.Add(effect.onFinished, delegate ()
                                          {

                                              icon.transform.localEulerAngles = new Vector3(0, 0, 0);

                                              GameObject obj = (GameObject)Instantiate(m_label);
                                              obj.transform.parent = icon.transform;
                                              obj.transform.localPosition = new Vector3(0, 0, 0);
                                              obj.transform.localScale = new Vector3(1, 1, 1);
                                              string _name = "";
                                              if (m_rewards[num].type == 2)
                                              {
                                                  s_t_item item = game_data._instance.get_item(m_rewards[num].value1);
                                                  _name = item.name;
                                              }
                                              else if (m_rewards[num].type == 3)
                                              {
                                                  s_t_class _class = game_data._instance.get_t_class(m_rewards[num].value1);
                                                  _name = _class.name;
                                              }

                                              card_icon _icon = effect.gameObject.GetComponent<card_icon>();

                                              if (_icon != null)
                                              {
                                                  _name = _icon.m_class.name;
                                              }

                                              if (m_rewards[num].type == 2)
                                              {
                                                  s_t_item item = game_data._instance.get_item(m_rewards[num].value1);
                                                  obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(_name, item.font_color);
                                              }
                                              else if (m_rewards[num].type == 3)
                                              {
                                                  s_t_class _class = game_data._instance.get_t_class(m_rewards[num].value1);
                                                  obj.transform.Find("Label").GetComponent<UILabel>().text = ccard.get_color_name(_name, _class.color);
                                              }
                                              obj.SetActive(true);

                                              if (m_rewards_pz[num] == 1)
                                              {
                                                  obj = (GameObject)Instantiate(m_shell);
                                                  obj.transform.parent = icon.transform;
                                                  obj.transform.localPosition = new Vector3(0, 0, 0);
                                                  obj.transform.localScale = new Vector3(1, 1, 1);
                                                  obj.SetActive(true);
                                              }

                                              m_objs.Remove(icon);
                                          });

                        m_num = i + 1;
                    }
                }
            }
            if (m_time <= 0)
            {
                if (flag)
                {
                    m_close_panel.SetActive(false);
                }
                else
                {
                    m_close_panel.SetActive(true);
                }
                flag = false;
                update_ui();
            }
        }
    }
}
