using System.Collections.Generic;
using UnityEngine;

public class card_dialog_box : MonoBehaviour
{
    public GameObject m_title;
    public GameObject m_name;
    public GameObject m_desc;
    public GameObject m_profession;
    public GameObject m_pinzhi;
    public GameObject m_icon;
    public UILabel m_hp;
    public UILabel m_attack;
    public UILabel m_wf;
    public UILabel m_mf;
    public UILabel m_speed;
    public UILabel m_skill_desc;
    public UILabel m_hp_up;
    public UILabel m_attack_up;
    public UILabel m_wf_up;
    public UILabel m_mf_up;
    public UILabel m_speed_up;
    public UILabel m_zuhe;
    public GameObject m_sp_toggle;
    public GameObject m_role_toggle;
    public GameObject m_scro;
    public GameObject m_role_panel;
    public GameObject m_sp_panel;
    public int item_id;
    public int type;
    public ccard m_card = null;

    public void OnEnable()
    {
        reset(type);
    }

    public void reset(int select)
    {
        if (m_scro.GetComponent<SpringPanel>() != null)
        {
            m_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_scro.transform.localPosition = new Vector3(0, 0, 0);
        m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (select == 0)
        {
            m_pinzhi.SetActive(false);
            m_name.transform.localPosition = new Vector3(-5, 182, 0);
            m_profession.transform.localPosition = new Vector3(-5, 149, 0);
            m_role_toggle.SetActive(false);
            m_sp_toggle.SetActive(true);
            int sp_id = item_id;
            s_t_class t_class = game_data._instance.get_t_class(item_id);
            if (t_class != null)
            {
                for (int i = 0; i < game_data._instance.m_dbc_item.get_y(); ++i)
                {
                    int id = int.Parse(game_data._instance.m_dbc_item.get(0, i));
                    s_t_item _t_item = game_data._instance.get_item(id);
                    if (_t_item.type == 3001 && _t_item.def_1 == t_class.id)
                    {
                        sp_id = id;
                        break;
                    }
                }
            }
            m_role_panel.SetActive(false);
            m_sp_panel.SetActive(true);
            m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language("card_dialog_box.cs_76_42");//基因
            s_t_item t_item = game_data._instance.get_item(sp_id);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2, sp_id, 0, 0);
            m_desc.GetComponent<UILabel>().text = "[fff300]" + t_item.desc;
            int num = sys._instance.m_self.get_item_num((uint)sp_id);
            m_profession.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("card_dialog_box.cs_81_62"), num.ToString());//当前拥有：{0}
            sys._instance.remove_child(m_icon);
            GameObject icon = icon_manager._instance.create_item_icon(sp_id);
            Transform iicon = m_icon.transform;
            icon.transform.parent = iicon;
            icon.transform.localPosition = new Vector3(0, 0, 0);
            icon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            icon.GetComponent<BoxCollider>().enabled = false;
        }
        if (select == 1)
        {
            m_role_toggle.SetActive(true);
            m_sp_toggle.SetActive(false);
            m_role_panel.SetActive(true);
            m_sp_panel.SetActive(false);
            m_name.transform.localPosition = new Vector3(-5, 188, 0);
            m_profession.transform.localPosition = new Vector3(-5, 164, 0);
            m_pinzhi.SetActive(true);
            m_title.GetComponent<UILabel>().text = game_data._instance.get_t_language("card_dialog_box.cs_99_42");//伙伴
            int role_id = item_id;
            s_t_item t_item = game_data._instance.get_item(item_id);
            if (t_item != null)
            {
                role_id = t_item.def_1;
            }
            s_t_class t_class = game_data._instance.get_t_class(role_id);
            m_profession.GetComponent<UILabel>().text = get_profession(t_class);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(3, role_id, 0, 0);
            s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(t_class.pz * 100);
            m_pinzhi.GetComponent<UILabel>().text = t_role_shengpin.name;
            if (m_card == null)
            {
                m_card = ccard.get_new_card(t_class.id);
            }
            sys._instance.remove_child(m_icon);
            GameObject icon = icon_manager._instance.create_card_icon(role_id, 0, 0, 0);
            Transform iicon = m_icon.transform;
            icon.transform.parent = iicon;
            icon.transform.localPosition = new Vector3(0, 0, 0);
            icon.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            icon.GetComponent<BoxCollider>().enabled = false;
            List<double> attrs = ccard.get_role_org_attr(role_id, m_card.get_level(), m_card.get_jlevel(), m_card.get_glevel(), m_card.get_pinzhi());
            int value = (int)attrs[1];
            m_hp.text = value.ToString();
            value = (int)attrs[2];
            m_attack.text = value.ToString();
            value = (int)attrs[3];
            m_wf.text = value.ToString();
            value = (int)attrs[4];
            m_mf.text = value.ToString();
            value = (int)attrs[5];
            m_speed.text = value.ToString();
            double _f = t_class.cz[0] + t_class.czcz[0] * m_card.get_glevel();
            m_hp_up.text = _f.ToString("f2");
            _f = t_class.cz[1] + t_class.czcz[1] * m_card.get_glevel();
            m_attack_up.text = _f.ToString("f2");
            _f = t_class.cz[2] + t_class.czcz[2] * m_card.get_glevel();
            m_wf_up.text = _f.ToString("f2");
            _f = t_class.cz[3] + t_class.czcz[3] * m_card.get_glevel();
            m_mf_up.text = _f.ToString("f2");
            _f = t_class.cz[4] + t_class.czcz[4] * m_card.get_glevel();
            m_speed_up.text = _f.ToString("f2");

            string _skill_des = " [ffff00]" + game_data._instance.get_t_language("card_dialog_box.cs_144_36");//普通攻击
            role_skill _skill = m_card.m_skills[0];
            if (_skill.m_t_skill.attack_type == 1)
            {
                _skill_des += " [ff0000][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2726_26") + "]\n";//物理
            }
            else if (_skill.m_t_skill.attack_type == 2)
            {
                _skill_des += " [7fa0ff][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
            }
            _skill_des += "[0aabff]" + _skill.get_des();

            _skill = m_card.m_skills[1];
            if (_skill != null)
            {
                _skill_des += "\n\n";
                _skill_des += "[ffff00]" + _skill.m_t_skill.name;
                if (_skill.m_t_skill.attack_type == 1)
                {
                    _skill_des += " [ff0000][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2726_26") + "]\n";//物理
                }
                else if (_skill.m_t_skill.attack_type == 2)
                {
                    _skill_des += " [7fa0ff][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
                }
                else
                {
                    _skill_des += "\n";
                }
                _skill_des += "[0aabff]" + _skill.get_des();
                _skill_des += "\n\n";
            }

            int _tp_level = 1;

            for (int i = (int)e_skill_type.skill_type_glevel_1; i < (int)e_skill_type.skill_end; ++i)
            {
                _skill = m_card.m_skills[i];
                if (_skill != null)
                {
                    _skill_des += "[ffff00]" + _skill.m_t_skill.name + " " + "(+" + string.Format(game_data._instance.get_t_language("card_dialog_box.cs_184_83"), _tp_level.ToString()) + "" + ")\n";//突破{0}开启
                    _skill_des += "[0aabff]" + _skill.get_des();
                    _skill_des += "\n\n";
                }
                _tp_level++;
            }

            _tp_level = 0;

            for (int i = (int)e_skill_type.skill_type_jlevel_1; i < (int)e_skill_type.skill_type_glevel_1; ++i)
            {
                _skill = m_card.m_skills[i];
                if (_skill != null)
                {
                    int num = _tp_level * 3;
                    if (_tp_level == 0)
                    {
                        num = 1;
                    }
                    s_t_jinjie t_jinjie = game_data._instance.get_jinjie(num);
                    _skill_des += "[ffff00]" + _skill.m_t_skill.name + " " + "(" + t_jinjie.name + game_data._instance.get_t_language("card_dialog_box.cs_205_84") + ")\n";//开启
                    _skill_des += "[0aabff]" + _skill.get_des();
                    _skill_des += "\n\n";
                    _tp_level++;
                }
            }
            string s = _skill_des.Trim('\n').Trim('\n');
            m_skill_desc.text = s;

            string _zh = "";

            if (m_card.m_t_class.jbs[0] > 0)
            {
                _zh += get_jb_text(m_card, m_card.m_t_class.jbs[0], _zh == "");
            }
            if (m_card.m_t_class.jbs[1] > 0)
            {
                _zh += get_jb_text(m_card, m_card.m_t_class.jbs[1], _zh == "");
            }
            if (m_card.m_t_class.jbs[2] > 0)
            {
                _zh += get_jb_text(m_card, m_card.m_t_class.jbs[2], _zh == "");
            }
            if (m_card.m_t_class.jbs[3] > 0)
            {
                _zh += get_jb_text(m_card, m_card.m_t_class.jbs[3], _zh == "");
            }
            if (m_card.m_t_class.jbs[4] > 0)
            {
                _zh += get_jb_text(m_card, m_card.m_t_class.jbs[4], _zh == "");
            }
            if (m_card.m_t_class.jbs[5] > 0)
            {
                _zh += get_jb_text(m_card, m_card.m_t_class.jbs[5], _zh == "");
            }
            if (m_card.m_t_class.jbs[6] > 0)
            {
                _zh += get_jb_text(m_card, m_card.m_t_class.jbs[6], _zh == "");
            }
            if (m_card.m_t_class.jbs[7] > 0)
            {
                _zh += get_jb_text(m_card, m_card.m_t_class.jbs[7], _zh == "");
            }
            if (_zh == "")
            {
                _zh = game_data._instance.get_t_language("bu_zheng_panel.cs_3270_10");//该伙伴没有组合属性
            }
            m_zuhe.text = _zh;
        }
    }

    public string get_jb_text(ccard card, int jb_id, bool first)
    {
        s_t_ji_ban _jb = game_data._instance.get_t_ji_ban(jb_id);
        string _text = "";
        if (_jb.type == 1)
        {
            for (int i = 0; i < _jb.tids.Count; ++i)
            {
                if (i != 0)
                {
                    _text += "、";
                }
                _text += game_data._instance.get_t_class(_jb.tids[i]).name;
            }
            if (!first)
            {
                _text = "\n" + "[FFFF00]" + "◆【" + _jb.name + "】" + game_data._instance.get_t_language("card_dialog_box.cs_272_54") + _text;// 与
            }
            else
            {
                _text = "[FFFF00]" + "◆【" + _jb.name + "】" + game_data._instance.get_t_language("card_dialog_box.cs_272_54") + _text;// 与
            }
            _text += game_data._instance.get_t_language("card_dialog_box.cs_278_12") + "，";//一起上阵
            if (_jb.attr1 > 0)
            {
                if (_jb.attr2 > 0)
                {
                    _text += game_data._instance.get_value_string(_jb.attr1, _jb.value1) + "、";
                }
                else
                {
                    _text += game_data._instance.get_value_string(_jb.attr1, _jb.value1);
                }
            }
            if (_jb.attr2 > 0)
            {
                _text += game_data._instance.get_value_string(_jb.attr2, _jb.value2);
            }
        }
        else if (_jb.type == 2)
        {
            if (!first)
            {
                _text += "\n";
            }
            _text += "[FFFF00]";
            _text += "◆【" + _jb.name + "】" + game_data._instance.get_t_language("bu_zheng_panel.cs_2956_35");//装备
            for (int j = 0; j < _jb.tids.Count; ++j)
            {
                if (j == _jb.tids.Count - 1)
                {
                    _text += game_data._instance.get_t_equip(_jb.tids[j]).name + ",";
                }
                else
                {
                    _text += game_data._instance.get_t_equip(_jb.tids[j]).name + "、";
                }
            }
            if (_jb.attr1 > 0)
            {
                if (_jb.attr2 > 0)
                {
                    _text += game_data._instance.get_value_string(_jb.attr1, _jb.value1) + "、";
                }
                else
                {
                    _text += game_data._instance.get_value_string(_jb.attr1, _jb.value1);
                }
            }
            if (_jb.attr2 > 0)
            {
                _text += game_data._instance.get_value_string(_jb.attr2, _jb.value2);
            }
        }
        else if (_jb.type == 3)
        {
            if (!first)
            {
                _text += "\n";
            }
            _text += "[FFFF00]";
            _text += "◆【" + _jb.name + "】" + game_data._instance.get_t_language("bu_zheng_panel.cs_2956_35");//装备
            for (int j = 0; j < _jb.tids.Count; ++j)
            {
                if (j == _jb.tids.Count - 1)
                {
                    _text += game_data._instance.get_t_baowu(_jb.tids[j]).name + ",";
                }
                else
                {
                    _text += game_data._instance.get_t_baowu(_jb.tids[j]).name + "、";
                }
            }
            if (_jb.attr1 > 0)
            {
                if (_jb.attr2 > 0)
                {
                    _text += game_data._instance.get_value_string(_jb.attr1, _jb.value1) + "、";
                }
                else
                {
                    _text += game_data._instance.get_value_string(_jb.attr1, _jb.value1);
                }
            }
            if (_jb.attr2 > 0)
            {
                _text += game_data._instance.get_value_string(_jb.attr2, _jb.value2);
            }
        }
        return _text;
    }

    public string get_profession(s_t_class m_t_class)
    {
        if (m_t_class.job == 1)
        {
            return game_data._instance.get_t_language("card_dialog_box.cs_372_10");//防
        }
        else if (m_t_class.job == 2)
        {
            return game_data._instance.get_t_language("card_dialog_box.cs_376_10");//物
        }
        return game_data._instance.get_t_language("card_dialog_box.cs_379_9");//魔
    }

    public void click(GameObject obj)
    {
        if (obj.transform.name == "ok")
        {
            this.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        if (obj.transform.name == "role")
        {
            type = 1;
            reset(type);
        }
        if (obj.transform.name == "sp")
        {
            type = 0;
            reset(type);
        }
    }
}
