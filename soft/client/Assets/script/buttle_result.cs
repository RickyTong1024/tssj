using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class buttle_result : MonoBehaviour, IMessage
{
    public GameObject[] m_star;
    public List<GameObject> m_stra;
    public GameObject m_gold;
    public GameObject m_yuanli;
    public GameObject m_exp;
    public GameObject m_exp_bar;
    public GameObject m_fail_show;
    public float m_target_exp = 0;
    public float m_cur_exp = 0;
    public float m_max_exp = 100;
    public UILabel m_des;
    public bool win;
    public bool m_can_close = false;
    public float m_time = 0;
    public s_message m_message;
    public List<GameObject> m_objs;
    public GameObject m_win_show;
    public GameObject m_shell;
    public GameObject m_reward_0;
    public GameObject m_reward_1;
    public GameObject m_cs;
    public GameObject m_tip;
    public UILabel m_tip_d;
    List<s_t_reward_ex> rewards = new List<s_t_reward_ex>();
    private int m_reward_num = 0;
    private float m_shake = 0;
    private int m_add_exp = 0;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    public void set_reward_num(int num)
    {
        sys._instance.remove_child(m_reward_0);
        sys._instance.remove_child(m_reward_1);

        m_reward_num = num;
        m_tip.SetActive(true);
    }

    public void add_gold(int gold)
    {
        m_gold.GetComponent<ui_num_anim>().m_cur = 0;
        m_gold.GetComponent<ui_num_anim>().m_cur = gold;
    }

    public void add_exp(int exp)
    {
        m_add_exp = exp;
        m_exp.GetComponent<ui_num_anim>().m_cur = 0;
        m_exp.GetComponent<ui_num_anim>().m_cur = exp;
    }

    public void add_yuanli(int yuanli)
    {
        m_yuanli.GetComponent<ui_num_anim>().m_cur = 0;
        m_yuanli.GetComponent<ui_num_anim>().m_cur = yuanli;
    }

    private GameObject get_reward_root(bool ex)
    {
        GameObject _reward = m_reward_0;

        if (ex)
        {
            _reward = m_reward_1;
        }
        return _reward;
    }

    public void set_tip(string s)
    {
        m_des.gameObject.SetActive(true);
        m_des.text = s;
    }

    public void add_tip(string s)
    {
        m_tip_d.text = s;
    }

    public void set_tip(int type, int value)
    {
        switch (type)
        {
            case 0:
                set_tip(game_data._instance.get_t_language("buttle_result.cs_95_24"));//我方全灭
                break;
            case 1:
                set_tip(string.Format(game_data._instance.get_t_language("buttle_result.cs_98_38"), value));//战斗超过{0}回合
                break;
            case 2:
                set_tip(string.Format(game_data._instance.get_t_language("buttle_result.cs_101_38"), value));//我方死亡人数超过{0}人
                break;
            case 3:
                set_tip(string.Format(game_data._instance.get_t_language("buttle_result.cs_104_38"), value));//我方剩余血量低于{0}%
                break;
            case 4:
                set_tip(string.Format(game_data._instance.get_t_language("buttle_result.cs_98_38"), value));//战斗超过{0}回合
                break;
        }
    }

    int sort(s_t_reward_ex x, s_t_reward_ex y)
    {

        if (x.value1 == 4 && y.value1 != 4)
        {
            return -1;
        }
        else if (x.value1 != 4 && y.value1 == 4)
        {
            return 1;
        }
        else
        {
            if (x.value1 == 1 && y.value1 != 1)
            {
                return -1;
            }
            else if (x.value1 != 1 && y.value1 == 1)
            {
                return 1;
            }
            else
            {
                return x.value1 - y.value1;
            }
        }
    }

    void create_icon()
    {
        rewards.Sort(sort);
        for (int i = 0; i < rewards.Count; i++)
        {
            if (rewards[i].value1 == 19 && rewards[i].type == 1)
            {
                continue;
            }
            GameObject obj = game_data._instance.ins_object_res("ui/win_reward_desc");
            obj.transform.parent = m_reward_0.transform;
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
            obj.transform.Find("icon").GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(rewards[i].value1);
            obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)rewards[i].value2;
            if (rewards[i].yichu)
            {
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)rewards[i].value4;
            }
            obj.transform.Find("value").GetComponent<ui_num_anim>().m_is_yichu = rewards[i].yichu;
            if (rewards[i].value1 == 13 || rewards[i].value1 == 1 || rewards[i].value1 == 6)
            {
                string s = "";
                if (rewards[i].value3 == 1)
                {
                    s = "[2fa4ff](" + game_data._instance.get_t_language("buttle_result.cs_166_38") + ")";//暴击
                }
                else if (rewards[i].value3 == 2)
                {
                    s = "[ff00ff](" + game_data._instance.get_t_language("buttle_result.cs_170_38") + ")";//大暴击
                }
                else if (rewards[i].value3 == 3)
                {
                    s = "[ff6100](" + game_data._instance.get_t_language("buttle_result.cs_174_38") + ")";//幸运暴击
                }
                obj.transform.Find("bj").GetComponent<UILabel>().text = s;
            }
            else if (rewards[i].value1 == 24)
            {
                long jiebn = rewards[i].value2;
                long jiacheng = rewards[i].value2 * rewards[i].value3 / 100;
                obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)jiebn;
                obj.transform.Find("bj").GetComponent<UILabel>().text = "[ff6100]+" + jiacheng + game_data._instance.get_t_language("buttle_result.cs_184_102") + rewards[i].value3 + "%)";// (奖励加成：
                if (rewards[i].value3 == 0)
                {
                    rewards[i].value3 = 1;
                }
            }
            else if (rewards[i].value1 == 25)
            {
                long jiebn = rewards[i].value2;
                long jiacheng = rewards[i].value2 * rewards[i].value3 / 100;
                obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)jiebn;
                obj.transform.Find("bj").GetComponent<UILabel>().text = "[ff6100]+" + jiacheng + game_data._instance.get_t_language("buttle_result.cs_184_102") + rewards[i].value3 + "%)";// (奖励加成：
                if (rewards[i].value3 == 0)
                {
                    rewards[i].value3 = 1;
                }
            }
            else if (rewards[i].value1 == 200)
            {
                obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)rewards[i].value2;
            }
            else if (rewards[i].value1 == 103)
            {
                obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)rewards[i].value2;
                obj.transform.Find("bj").GetComponent<UILabel>().text = "[ff6100](" + string.Format(game_data._instance.get_t_language("buttle_result.cs_212_105"), rewards[i].value3) + ")";//上升{0}名
            }
            else if (rewards[i].value1 == 104)
            {
                obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)rewards[i].value2;
                obj.transform.Find("bj").GetComponent<UILabel>().text = "[ff6100](" + string.Format(game_data._instance.get_t_language("buttle_result.cs_212_105"), rewards[i].value3) + ")";//上升{0}名
            }
            else if (rewards[i].value1 == 106)
            {
                obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)rewards[i].value2;
                if (rewards[i].value3 > 0)
                {
                    obj.transform.Find("bj").GetComponent<UILabel>().text = "[ff6100](" + string.Format(game_data._instance.get_t_language("buttle_result.cs_227_109"), rewards[i].value3) + ")";//上升{0}积分
                }
                else if (rewards[i].value3 < 0)
                {
                    obj.transform.Find("bj").GetComponent<UILabel>().text = "[ff6100](" + string.Format(game_data._instance.get_t_language("buttle_result.cs_231_109"), -rewards[i].value3) + ")";//下降{0}积分
                }
                else
                {
                    if (win)
                    {
                        obj.transform.Find("bj").GetComponent<UILabel>().text = "[ff6100](" + string.Format(game_data._instance.get_t_language("buttle_result.cs_227_109"), rewards[i].value3) + ")";//上升{0}积分
                    }
                    else
                    {
                        obj.transform.Find("bj").GetComponent<UILabel>().text = "[ff6100](" + string.Format(game_data._instance.get_t_language("buttle_result.cs_231_109"), rewards[i].value3) + ")";//下降{0}积分
                    }
                }
            }
            else if (rewards[i].value1 == 107)
            {
                obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)rewards[i].value2;
            }
            else if (rewards[i].value1 == 108)
            {
                obj.GetComponent<UILabel>().text = sys._instance.get_res_color_name(rewards[i].value1) + "：";
                obj.transform.Find("value").GetComponent<ui_num_anim>().m_target = (float)rewards[i].value2;
            }
        }
    }

    public void arrange()
    {
        create_icon();
        int y = 0;
        int k = 0;
        if (m_des.gameObject.activeSelf)
        {
            k = 0;
        }
        bool flag = false;
        GameObject flag_obj = null;
        for (int i = 0; i < m_reward_0.transform.childCount; i++)
        {
            GameObject _icon = m_reward_0.transform.GetChild(i).gameObject;
            Vector3 _pos = _icon.transform.localPosition;

            _pos.x = 10;
            _pos.y = -i * 40 - k;
            y = -(i + 1) * 40 - 44;
            _icon.transform.localPosition = _pos;
            _icon.SetActive(false);
            if (!m_objs.Contains(_icon))
            {
                m_objs.Add(_icon);

            }
            GameObject obj = _icon;
            if (!flag)
            {
                if (rewards[i].value3 > 0)
                {
                    flag = true;
                    int value_w = obj.transform.Find("value").GetComponent<UILabel>().width;
                    int bj_w = obj.transform.Find("bj").GetComponent<UILabel>().width;
                    int obj_w = obj.GetComponent<UILabel>().width;
                    int icon_w = obj.transform.Find("icon").GetComponent<UISprite>().width;
                    obj.transform.localPosition = new Vector3(-((value_w + bj_w + obj_w + icon_w + 34) / 2 - obj_w)
                        , obj.transform.localPosition.y, obj.transform.localPosition.z);
                    flag_obj = obj;
                }
            }
        }
        if (flag)
        {
            for (int i = 0; i < m_objs.Count; i++)
            {
                m_objs[i].transform.localPosition = new Vector3(flag_obj.transform.localPosition.x, m_objs[i].transform.localPosition.y, m_objs[i].transform.localPosition.z);
            }
        }

        Vector3 _reward_pos = m_reward_0.transform.localPosition;
        m_reward_0.transform.localPosition = _reward_pos;

        for (int i = 0; i < m_reward_1.transform.childCount; i++)
        {
            GameObject _icon = m_reward_1.transform.GetChild(i).gameObject;
            Vector3 _pos = _icon.transform.localPosition;

            _pos.x = 100 * i;
            _pos.y = 0;

            _icon.transform.localPosition = _pos;
        }
        _reward_pos = m_reward_1.transform.localPosition;
        _reward_pos.y = y;
        _reward_pos.x = -50 * (m_reward_1.transform.childCount - 1);

        m_reward_1.transform.localPosition = _reward_pos;
        m_reward_1.SetActive(false);
        m_objs.Add(m_reward_1);
        for (int i = 0; i < m_objs.Count; i++)
        {
            if (i == 0)
            {
                m_objs[i].SetActive(true);
            }
            if ((i + 1) < m_objs.Count)
            {
                if (m_objs[i].transform.Find("value") != null)
                {
                    m_objs[i].transform.Find("value").GetComponent<ui_num_anim>().m_obj = m_objs[i + 1];
                }
            }
        }
    }

    IEnumerator fade()
    {
        for (int i = 0; i < m_objs.Count; i++)
        {
            m_objs[i].SetActive(true);
            yield return null;
        }
    }

    public void set_win(bool ex)
    {
        win = ex;
        if (ex)
        {
            m_win_show.SetActive(true);
            m_fail_show.SetActive(false);
        }
        else
        {
            m_win_show.SetActive(false);
            m_fail_show.SetActive(true);
            game_data._instance.m_guaji = 0;
        }
    }

    public void add_reward(int type, int value1, long value2, int value3, bool ex = false)
    {
        if (type == 1)
        {

            if (value1 == 19)
            {
                return;
            }
            if (value2 > 0 || value1 == 24 || value1 == 25 || value1 == 200 || value1 == 106)
            {
                s_t_reward_ex reward = new s_t_reward_ex();
                reward.type = type;
                reward.value1 = value1;
                reward.value2 = value2;
                if (value2 > int.MaxValue)
                {
                    reward.value4 = value2;
                    reward.yichu = true;
                }
                else
                {
                    reward.yichu = false;
                }
                reward.value3 = value3;
                rewards.Add(reward);
                if (value1 == 4)
                {
                    uplevel_player((int)value2);
                }
            }
        }
        else
        {
            if (value1 > 0)
            {
                if (type == 3)
                {
                    if (sys._instance.m_self.get_card_id(value1) != null)
                    {
                        return;
                    }
                }
                GameObject _icon = icon_manager._instance.create_reward_icon(type, value1, (int)value2, value3);
                _icon.transform.parent = m_reward_1.transform;
                _icon.transform.localPosition = new Vector3(0, 0, 0);
                _icon.transform.localScale = new Vector3(1, 1, 1);
                _icon.SetActive(true);
            }
        }
    }
    public void add_reward(dhc.role_t role, bool ex = false)
    {
        GameObject _icon = icon_manager._instance.create_card_icon(role.guid);
        _icon.transform.parent = m_reward_1.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
    }

    public void add_reward(dhc.equip_t equip, bool ex = false)
    {
        GameObject _icon = icon_manager._instance.create_equip_icon(equip.guid);
        _icon.transform.parent = m_reward_1.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
    }

    public void uplevel_player(int add_exp)
    {
        player _player = sys._instance.m_self;
        m_exp_bar.SetActive(true);
        m_exp_bar.GetComponent<ui_exp_bar_anim>().m_speed = 0.5f;

        if (_player.m_t_player.exp >= add_exp)
        {
            m_exp_bar.GetComponent<ui_exp_bar_anim>().set_property(0, 99, _player.m_t_player.level, _player.m_t_player.exp - add_exp, add_exp);
        }
        else
        {
            s_t_exp t_exp = game_data._instance.get_t_exp(_player.m_t_player.level);
            if (t_exp != null)
            {
                m_exp_bar.GetComponent<ui_exp_bar_anim>().set_property(0, 99, _player.m_t_player.level - 1, _player.m_t_player.exp + t_exp.exp - add_exp, add_exp);
            }
            else
            {
                m_exp_bar.GetComponent<ui_exp_bar_anim>().set_property(0, 99, _player.m_t_player.level, _player.m_t_player.exp, add_exp);
            }
        }
    }

    public void set_star(int num)
    {
        for (int i = 0; i < m_stra.Count; i++)
        {
            m_stra[i].SetActive(true);
        }
        int _num = num;
        float _delay = 0.6f;
        for (int i = 0; i < m_star.Length; i++)
        {
            m_star[i].SetActive(true);
            m_star[i].GetComponent<UISprite>().alpha = 0.0f;
            sys._instance.add_alpha_anim(m_star[i], 0.1f, 0, 1, _delay + i * 0.2f);

            if (i >= _num)
            {
                m_star[i].GetComponent<UISprite>().spriteName = "jsdh_star02";
                m_star[i].transform.localScale = new Vector3(0, 0, 0);
                sys._instance.add_scale_anim(m_star[i], 0.2f, 0, 1, _delay + i * 0.2f);
                m_star[i].GetComponent<TweenScale>().enabled = false;
            }
            else
            {
                m_star[i].GetComponent<UISprite>().spriteName = "jsdh_star01";
                m_star[i].transform.localScale = new Vector3(7, 7, 7);
                sys._instance.add_scale_anim(m_star[i], 0.1f, 7, 1, _delay + i * 0.2f);

                TweenScale _scale = m_star[i].GetComponent<TweenScale>();

                EventDelegate.Add(_scale.onFinished, delegate ()
                                  {
                                      m_shake = 6;

                                      sys._instance.play_sound("sound/mcx20070511");

                                  });

                m_star[i].GetComponent<TweenScale>().enabled = true;
            }
        }

        m_shell.SetActive(true);
        m_shell.transform.localScale = new Vector3(0, 0, 0);
        m_cs.GetComponent<UISprite>().alpha = 0.0f;
        sys._instance.add_scale_anim(m_shell, 0.2f, 0, 1, _delay + 3 * 0.2f);

        TweenScale _shell_scale = m_shell.GetComponent<TweenScale>();
        EventDelegate.Add(_shell_scale.onFinished, delegate ()
                          {
                              m_can_close = true;
                              m_time = 0.5f;
                          });

        if (_num > 2)
        {
            sys._instance.add_alpha_anim(m_cs, 0.1f, 0, 1, _delay + 4 * 0.2f);
            sys._instance.add_scale_anim(m_cs, 0.1f, 7, 1, _delay + 4 * 0.2f);

            TweenScale _cscale = m_cs.GetComponent<TweenScale>();
            EventDelegate.Add(_cscale.onFinished, delegate ()
                              {
                                  m_shake = 10;

                                  sys._instance.play_sound("sound/mcx20070511");
                              });
        }
    }

    public void OnEnable()
    {
        m_can_close = false;

        if (m_gold == null && m_shell != null)
        {
            m_shell.SetActive(true);
        }
        arrange();
    }

    public void OnDisable()
    {
        for (int i = 0; i < m_star.Length; i++)
        {
            m_star[i].SetActive(false);
        }
        for (int i = 0; i < m_stra.Count; i++)
        {
            m_stra[i].SetActive(false);
        }
        if (m_shell != null)
        {
            m_shell.SetActive(false);
        }

        sys._instance.remove_child(m_reward_0);
        sys._instance.remove_child(m_reward_1);
        m_exp_bar.SetActive(false);
        m_des.gameObject.SetActive(false);
        m_des.text = "";
        m_objs.Clear();
        rewards.Clear();
    }

    void IMessage.net_message(s_net_message message)
    {

    }

    public void click(GameObject obj)
    {
        if (obj.name == "defeat")
        {
            if (m_message == null)
            {
                sys._instance.m_game_state = "hall";
                s_message msg = new s_message();
                msg.m_type = "load_scene";
                msg.m_string.Add((string)sys._instance.m_hall_name);
                cmessage_center._instance.add_message(msg);
            }
            else
            {
                if (m_message.m_next == null)
                {
                    m_message.m_next = new s_message();
                }
                cmessage_center._instance.add_message(m_message);
                m_message = null;
            }
            return;
        }
        else if (obj.name == "defupgradehero")
        {
            //跳转到布阵 充能界面
            if (sys._instance.m_game_state != "hall")
            {
                sys._instance.m_game_state = "hall";
                root_gui._instance.m_default_active = "upgrade";
                sys._instance.load_scene(sys._instance.m_hall_name);
            }
            s_message _mes = new s_message();
            _mes.m_type = "return_to_main";
            cmessage_center._instance.add_message(_mes);
            return;

        }
        else if (obj.name == "defsummonhero")
        {
            //跳转到 伙伴招募
            if (sys._instance.m_game_state != "hall")
            {
                sys._instance.m_game_state = "hall";
                root_gui._instance.m_default_active = "summon";
                sys._instance.load_scene(sys._instance.m_hall_name);
            }
            s_message _mes = new s_message();
            _mes.m_type = "return_to_main";
            cmessage_center._instance.add_message(_mes);
            return;
        }
        else if (obj.name == "defforg")
        {
            //跳转到强化装备
            if (sys._instance.m_game_state != "hall")
            {
                sys._instance.m_game_state = "hall";
                root_gui._instance.m_default_active = "forg";
                sys._instance.load_scene(sys._instance.m_hall_name);
            }
            s_message _mes = new s_message();
            _mes.m_type = "return_to_main";
            cmessage_center._instance.add_message(_mes);
            return;
        }
        if (m_reward_1.activeSelf)
        {
            if (m_message == null)
            {
                sys._instance.m_game_state = "hall";
                s_message msg = new s_message();
                msg.m_type = "load_scene";
                msg.m_string.Add((string)sys._instance.m_hall_name);
                cmessage_center._instance.add_message(msg);
            }
            else
            {
                if (m_message.m_next == null)
                {
                    m_message.m_next = new s_message();
                }
                cmessage_center._instance.add_message(m_message);
                m_message = null;
            }
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "loaded" && sys._instance.m_game_state == "buttle")
        {
            sys._instance.remove_child(m_reward_0);
            sys._instance.remove_child(m_reward_1);
        }
        if (message.m_type == "battle_scene_show" && this.gameObject.activeSelf == true)
        {
            m_reward_0.transform.parent.gameObject.SetActive(true);
            m_reward_1.transform.parent.gameObject.SetActive(false);
        }
        if (message.m_type == "battle_reward_show2" && this.gameObject.activeSelf == true)
        {
            m_reward_0.transform.parent.gameObject.SetActive(false);
            m_reward_1.transform.parent.gameObject.SetActive(true);
            battle._instance.remove_all();
            root_gui._instance.m_ui_bottomleft.transform.gameObject.SetActive(true);
        }
        if (message.m_type == "box_end" && this.gameObject.activeSelf == true)
        {
            s_message _message = new s_message();
            _message.m_type = "battle_reward_show";
            cmessage_center._instance.add_message(_message);

            _message = new s_message();
            _message.time = 0.3f;
            _message.m_type = "battle_reward_show2";
            cmessage_center._instance.add_message(_message);
        }
    }

    void Update()
    {
        if (m_shake > 0)
        {
            float _addx = Random.Range(-m_shake, m_shake);
            float _addy = Random.Range(-m_shake, m_shake);
            float _addz = Random.Range(-m_shake, m_shake);

            this.transform.localPosition = new Vector3(_addx, _addy, _addz);

            m_shake -= Time.deltaTime * m_shake * 10.0f;

            if (m_shake <= 0)
            {
                this.transform.localPosition = new Vector3(0, 0, 0);
            }
        }
        if (m_time > 0)
        {
            m_time -= Time.deltaTime;
        }
    }
}
