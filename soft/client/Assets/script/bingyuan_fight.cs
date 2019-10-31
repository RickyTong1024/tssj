using System.Collections.Generic;
using UnityEngine;

public class bingyuan_fight : MonoBehaviour, IMessage
{
    public static bingyuan_fight _instance;
    public List<GameObject> m_by;
    private List<GameObject> m_units = new List<GameObject>();
    public List<GameObject> m_hps = new List<GameObject>();
    private List<GameObject> m_biaoqings = new List<GameObject>();
    private float m_action_time = 0;
    private float m_next_time = 0;
    private float m_des_time = 0;
    private float m_lv_time = 0;
    private int hp_0 = 0;
    private int hp_1 = 0;
    private int success_index;
    private float m_vs_time = 0;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        cmessage_center._instance.add_handle(this);
        m_units.Add(null);
        m_units.Add(null);
        m_hps.Add(null);
        m_hps.Add(null);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    public void vs_show(protocol.team.team_player player_0, protocol.team.team_player player_1)
    {
        bingyuan_gui._instance.m_buttle_vs.transform.Find("head_0").Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(player_0.achieve) + player_0.name;
        bingyuan_gui._instance.m_buttle_vs.transform.Find("head_0").Find("logo").GetComponent<UISprite>().spriteName = player.get_touxiang((int)player_0.id);
        bingyuan_gui._instance.m_buttle_vs.transform.Find("head_0").Find("level").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_fight.cs_43_118") + sys._instance.value_to_wan(player_0.bf);//战力 

        bingyuan_gui._instance.m_buttle_vs.transform.Find("head_1").Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(player_1.achieve) + player_1.name;
        bingyuan_gui._instance.m_buttle_vs.transform.Find("head_1").Find("logo").GetComponent<UISprite>().spriteName = player.get_touxiang((int)player_1.id);
        bingyuan_gui._instance.m_buttle_vs.transform.Find("head_1").Find("level").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_fight.cs_43_118") + sys._instance.value_to_wan(player_1.bf);//战力 

        bingyuan_gui._instance.m_buttle_vs.SetActive(true);
        bingyuan_gui._instance.m_buttle_vs.GetComponent<hide_time>().m_time = 1f;
        m_vs_time = 1f;
    }

    public void set_round(int index)
    {
        if (index == 1)
        {
            m_lv_time = 1.5f;
        }
        else
        {
            m_lv_time = 1.5f;
        }

        UILabel _lv = bingyuan_gui._instance.m_ui_ficht.transform.Find("lv").GetComponent<UILabel>();

        {
            _lv.text = string.Format(game_data._instance.get_t_language("bingyuan_fight.cs_68_37"));//第1回合
        }

        bingyuan_gui._instance.m_ui_ficht.SetActive(true);
        bingyuan_gui._instance.m_ui_ficht.GetComponent<hide_time>().m_time = 2.0f;
    }

    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "bingyuan_fight_unit")
        {
            int index = (int)mes.m_ints[0];
            protocol.team.team_player _player = (protocol.team.team_player)mes.m_object[0];

            if (m_units[index] != null)
            {
                return;
            }
            GameObject _unit = sys._instance.create_class(_player.id, _player.dress, _player.guanghuan, m_by[index]);
            if (index == 1)
            {
                _unit.transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            m_units[index] = _unit;
            if (index == 0)
            {
                _unit.GetComponent<unit>().m_camp = -1;
            }
            else
            {
                _unit.GetComponent<unit>().m_camp = 1;
            }
            GameObject obj = _unit.GetComponent<unit>().create_mini_pro_player(_player);
            obj.name = index + "";
            m_hps[index] = obj;
        }
        if (mes.m_type == "bingyuan_fight_unit_die")
        {
            int index = (int)mes.m_ints[0];
            if (m_units[index] != null)
            {
                m_units[index].GetComponent<unit>().m_die = true;
                m_units[index] = null;
                Destroy(m_hps[index]);
                m_hps[index] = null;
            }
            m_des_time = 1;
        }
        if (mes.m_type == "bingyuan_fight_end")
        {
            for (int i = 0; i < m_units.Count; ++i)
            {
                sys._instance.remove_child(m_by[i]);
                m_units[i] = null;
                if (m_hps[i] != null)
                {
                    Destroy(m_hps[i]);
                }
                m_hps[i] = null;
            }
        }
        if (mes.m_type == "show_attack_num_bingyuan")
        {
            for (int i = 0; i < m_units.Count; i++)
            {
                if (m_units[i] != null)
                {
                    m_units[i].GetComponent<unit>().show_attack_num(1, (int)(double)mes.m_floats[i], "", false);
                }
            }

            hp_0 = (int)(double)mes.m_floats[0];
            hp_1 = (int)(double)mes.m_floats[1];
            success_index = (int)mes.m_ints[0];
            Invoke("show_biaoqing", 0.5f);
        }
    }

    GameObject create_min_hp(protocol.team.team_player player)
    {
        GameObject min_hp = game_data._instance.ins_object_res("ui/player_hp_pro");
        min_hp.transform.Find("main/name").GetComponent<UILabel>().text = game_data._instance.get_name_color(player.achieve) + player.name;
        min_hp.transform.Find("main/attack").GetComponent<UILabel>().text = game_data._instance.get_t_language("bingyuan_fight.cs_43_118") + sys._instance.value_to_wan(player.bf);//战力 
        s_t_bingyuan_chenhao _chenghao = game_data._instance.get_t_bingyuan_chenghao(player.chenhao);
        if (_chenghao != null)
        {
            min_hp.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
        }
        else
        {
            _chenghao = game_data._instance.get_t_bingyuan_chenghao(7);
            min_hp.transform.Find("main/chenhao").GetComponent<UILabel>().text =
                game_data._instance.get_t_chenhao(_chenghao.chenhaoid).name;
        }
        return min_hp;
    }

    void show_biaoqing()
    {
        GameObject obj = null;
        m_biaoqings.Clear();
        float _r = 0.3f;
        if (hp_0 > hp_1)
        {
            obj = game_data._instance.ins_object_res("ui/biaoqing");

            {
                obj.GetComponent<UISprite>().spriteName = "png-0425";
            }
            obj.transform.parent = root_gui._instance.m_ui_bottomleft.transform;

            Vector3 _pos = m_units[0].transform.position;
            obj.transform.position = UICamera.mainCamera.ScreenToWorldPoint
                (sys._instance.get_cam().WorldToScreenPoint
                (new Vector3(_pos.x + m_units[0].GetComponent<unit>().m_name_height * _r, _pos.y + m_units[0].GetComponent<unit>().m_name_height, _pos.z)));
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            obj.transform.localScale = Vector3.one;
            m_biaoqings.Add(obj);
            Destroy(obj, 1f);

            obj = game_data._instance.ins_object_res("ui/biaoqing");
            obj.GetComponent<UISprite>().spriteName = "png-0430";
            obj.transform.parent = root_gui._instance.m_ui_bottomleft.transform;

            _pos = m_units[1].transform.position;
            obj.transform.position = UICamera.mainCamera.ScreenToWorldPoint
                (sys._instance.get_cam().WorldToScreenPoint
                (new Vector3(_pos.x + m_units[1].GetComponent<unit>().m_name_height * _r, _pos.y + m_units[1].GetComponent<unit>().m_name_height, _pos.z)));
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            obj.transform.localScale = Vector3.one;
            m_biaoqings.Add(obj);
            Destroy(obj, 1f);
        }
        else if (hp_0 < hp_1)
        {
            obj = game_data._instance.ins_object_res("ui/biaoqing");

            {
                obj.GetComponent<UISprite>().spriteName = "png-0430";
            }
            obj.transform.parent = root_gui._instance.m_ui_bottomleft.transform;

            Vector3 _pos = m_units[0].transform.position;
            obj.transform.position = UICamera.mainCamera.ScreenToWorldPoint
                (sys._instance.get_cam().WorldToScreenPoint
                (new Vector3(_pos.x + m_units[0].GetComponent<unit>().m_name_height * _r, _pos.y + m_units[0].GetComponent<unit>().m_name_height, _pos.z)));
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            m_biaoqings.Add(obj);
            obj.transform.localScale = Vector3.one;
            Destroy(obj, 1f);

            obj = game_data._instance.ins_object_res("ui/biaoqing");
            obj.GetComponent<UISprite>().spriteName = "png-0425";
            obj.transform.parent = root_gui._instance.m_ui_bottomleft.transform;

            _pos = m_units[1].transform.position;
            obj.transform.position = UICamera.mainCamera.ScreenToWorldPoint
                (sys._instance.get_cam().WorldToScreenPoint
                (new Vector3(_pos.x + m_units[1].GetComponent<unit>().m_name_height * _r, _pos.y + m_units[1].GetComponent<unit>().m_name_height, _pos.z)));
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            m_biaoqings.Add(obj);
            obj.transform.localScale = Vector3.one;
            Destroy(obj, 1f);
        }
        else
        {
            obj = game_data._instance.ins_object_res("ui/biaoqing");
            obj.GetComponent<UISprite>().spriteName = "png-0425";
            obj.transform.parent = root_gui._instance.m_ui_bottomleft.transform;

            Vector3 _pos = m_units[0].transform.position;
            obj.transform.position = UICamera.mainCamera.ScreenToWorldPoint
                (sys._instance.get_cam().WorldToScreenPoint
                (new Vector3(_pos.x + m_units[0].GetComponent<unit>().m_name_height * _r, _pos.y + m_units[0].GetComponent<unit>().m_name_height, _pos.z)));
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            obj.transform.localScale = Vector3.one;
            m_biaoqings.Add(obj);
            Destroy(obj, 1f);

            obj = game_data._instance.ins_object_res("ui/biaoqing");
            obj.GetComponent<UISprite>().spriteName = "png-0430";
            obj.transform.parent = root_gui._instance.m_ui_bottomleft.transform;
            _pos = m_units[1].transform.position;
            obj.transform.position = UICamera.mainCamera.ScreenToWorldPoint
                (sys._instance.get_cam().WorldToScreenPoint
                (new Vector3(_pos.x + m_units[1].GetComponent<unit>().m_name_height * _r, _pos.y + m_units[1].GetComponent<unit>().m_name_height, _pos.z)));
            obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
            m_biaoqings.Add(obj);
            obj.transform.localScale = Vector3.one;
            Destroy(obj, 1f);
        }

        if (success_index == 1)
        {
            m_biaoqings[0].GetComponent<UISprite>().spriteName = "png-0436";
            m_biaoqings[1].GetComponent<UISprite>().spriteName = "png-0406";
        }
        else if (success_index == 0)
        {
            m_biaoqings[1].GetComponent<UISprite>().spriteName = "png-0436";
            m_biaoqings[0].GetComponent<UISprite>().spriteName = "png-0406";
        }

    }
    void IMessage.net_message(s_net_message mes)
    {

    }

    public void action()
    {
        m_action_time = 0.2f;
    }

    void effect()
    {
        if (bingyuan_gui._instance == null)
        {
            return;
        }
        if (bingyuan_battle._instance == null || !bingyuan_battle._instance.m_battle_fight)
        {
            return;
        }
        sys._instance.show_effect(transform.position + new Vector3(0, 1.5f, 0), "effect/ef_zjtx01", 1.0f);
        sys._instance.play_sound("sound/hudun01");
    }

    void end1()
    {
        m_next_time = 1;
        s_message _mes = new s_message();
        _mes.m_type = "attack_hp";
        cmessage_center._instance.add_message(_mes);
        for (int i = 0; i < m_hps.Count; i++)
        {
            m_hps[i].SetActive(true);
        }
    }

    void next()
    {
        Time.timeScale = 1f;
        s_message mes = new s_message();
        mes.m_type = "bingyuan_fight_next";
        cmessage_center._instance.add_message(mes);
    }

    void Update()
    {
        if (bingyuan_battle._instance == null)
        {
            return;
        }
        if (!bingyuan_battle._instance.m_battle_fight)
        {
            return;
        }
        if (m_des_time > 0)
        {
            m_des_time -= Time.deltaTime;
            if (m_des_time < 0)
            {
                bingyuan_battle._instance.set_des_t();
                m_next_time = 1;
            }
        }
        if (m_lv_time > 0)
        {
            m_lv_time -= Time.deltaTime;
            return;
        }
        if (m_vs_time > 0)
        {
            m_vs_time -= Time.deltaTime;
            if (m_vs_time < 0)
            {
                set_round(bingyuan_battle._instance.m_hp_num + 1);
            }
            return;
        }
        for (int i = 0; i < m_units.Count; i++)
        {
            if (m_units[i] != null)
            {
                GameObject _unit = m_units[i];
                if (i < m_hps.Count)
                {
                    GameObject obj = m_hps[i];
                    if (obj)
                    {
                        obj.transform.position = get_position(_unit.transform.position + new Vector3(0, _unit.GetComponent<unit>().m_name_height * 1.5f, 0));
                        obj.transform.position = new Vector3(obj.transform.position.x, obj.transform.position.y, 0);
                    }
                }

                if (i < m_biaoqings.Count)
                {
                    if (m_biaoqings[i] != null)
                    {
                        Vector3 _pos = m_units[i].transform.position;
                        m_biaoqings[i].transform.position = UICamera.mainCamera.ScreenToWorldPoint
                            (sys._instance.get_cam().WorldToScreenPoint
                            (new Vector3(_pos.x + m_units[i].GetComponent<unit>().m_name_height * 0.3f, _pos.y + m_units[i].GetComponent<unit>().m_name_height, _pos.z)));
                        m_biaoqings[i].transform.position = new Vector3(m_biaoqings[i].transform.position.x, m_biaoqings[i].transform.position.y, 0);
                    }
                }

            }
        }

        if (m_action_time > 0)
        {
            m_action_time -= Time.deltaTime;
            if (m_action_time <= 0)
            {
                this.GetComponent<Animator>().Play("bingyuan_fight");
                string s = "";
                if (bingyuan_battle._instance.m_end_huihe)
                {
                    s = "effect/ef_zjtx_tw02";
                }
                else
                {
                    s = "effect/ef_zjtx_tw01";

                }
                for (int i = 0; i < m_units.Count; i++)
                {
                    GameObject obj = sys._instance.show_effect(m_units[i].transform.position + new Vector3(0, 0f, 0), s, 1.0f);
                    obj.transform.parent = m_units[i].GetComponent<unit>().m_accept.transform;
                    obj.transform.localPosition = Vector3.zero;
                    obj.transform.localScale = Vector3.one;
                    m_hps[i].SetActive(false);
                    if (i < m_biaoqings.Count)
                    {
                        if (m_biaoqings[i])
                        {
                            m_biaoqings[i].SetActive(false);
                        }
                    }
                }
                Time.timeScale = 1f;
                Invoke("end1", 1);
            }
        }

        if (m_next_time > 0)
        {
            m_next_time -= Time.deltaTime;
            if (m_next_time <= 0)
            {
                next();
            }
        }
    }

    Vector3 get_position(Vector3 pos)
    {
        Camera _cam = sys._instance.get_cam();
        if (_cam == null)
        {
            return pos;
        }
        return UICamera.mainCamera.ScreenToWorldPoint(_cam.WorldToScreenPoint(pos));
    }
}
