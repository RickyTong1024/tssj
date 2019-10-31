using System.Collections.Generic;
using UnityEngine;

public class card_info : MonoBehaviour
{
    public s_t_class m_class;
    public ccard m_card;

    public GameObject m_icon;
    public GameObject m_name;
    public GameObject m_xb;
    public GameObject m_zy;
    public GameObject m_xj;

    public GameObject m_ji_nen;
    public GameObject m_des;
    public GameObject m_sx;
    public GameObject m_g_sx;
    public GameObject m_ji_ban;
    public GameObject m_cheng_zhang;
    public GameObject m_scroll_view;

    public string get_jb_text(ccard card, int jb_id, bool first)
    {
        s_t_ji_ban _jb = game_data._instance.get_t_ji_ban(jb_id);
        string _text = "";
        if (_jb.type == 1)
        {
            bool flag = true;
            for (int i = 0; i < _jb.tids.Count; ++i)
            {
                if (i != 0)
                {
                    _text = string.Format("{0}{1}", _text, "、");
                }
                if (sys._instance.m_self.is_zheng(_jb.tids[i]) || sys._instance.m_self.is_houyuan(_jb.tids[i]))
                {
                    _text += game_data._instance.get_t_class(_jb.tids[i]).name;
                }
                else
                {
                    flag = false;
                    _text += game_data._instance.get_t_class(_jb.tids[i]).name;
                }
            }
            if (flag)
            {
                if (!first)
                {

                    _text = string.Format("\n\n[FFFF00]◆【{0}】{1}", _jb.name, game_data._instance.get_t_language("card_dialog_box.cs_272_54") + _text);// 与
                                                                                                                                                      //_text = "\n\n" +"[FFFF00]" + "◆【" + _jb.name + "】"+ game_data._instance.get_t_language ("card_dialog_box.cs_272_54") + _text;// 与
                }
                else
                {
                    _text = string.Format("[FFFF00]◆【{0}】{1}", _jb.name, game_data._instance.get_t_language("card_dialog_box.cs_272_54") + _text);// 与
                                                                                                                                                  //_text = "[FFFF00]" + "◆【" + _jb.name + "】"+ game_data._instance.get_t_language ("card_dialog_box.cs_272_54") + _text;// 与
                }
            }
            else
            {
                if (!first)
                {
                    _text = string.Format("\n\n[777777]◆【{0}】{1}", _jb.name, game_data._instance.get_t_language("card_dialog_box.cs_272_54") + _text);// 与
                                                                                                                                                      //_text = "\n\n" +"[777777]" + "◆【" + _jb.name + "】"+ game_data._instance.get_t_language ("card_dialog_box.cs_272_54") + _text;// 与
                }
                else
                {
                    _text = string.Format("[777777]◆【{0}】{1}", _jb.name, game_data._instance.get_t_language("card_dialog_box.cs_272_54") + _text);// 与
                                                                                                                                                  //_text = "[777777]" + "◆【" + _jb.name + "】"+ game_data._instance.get_t_language ("card_dialog_box.cs_272_54") + _text;// 与
                }
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
            bool flag = false;
            for (int j = 0; j < _jb.tids.Count; ++j)
            {
                flag = false;
                for (int i = 0; i < card.m_equip.Count; i++)
                {
                    if (card.m_equip[i] == null)
                    {
                        continue;
                    }
                    if (_jb.tids[j] == card.m_equip[i].template_id)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!first)
            {
                _text += "\n\n";
            }
            if (flag)
            {
                _text += "[FFFF00]";
            }
            else
            {
                _text += "[777777]";
            }
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
            bool flag = false;
            for (int j = 0; j < _jb.tids.Count; ++j)
            {
                flag = false;
                for (int i = 0; i < card.m_treasure.Count; i++)
                {
                    if (card.m_treasure[i] == null)
                    {
                        continue;
                    }
                    if (_jb.tids[j] == card.m_treasure[i].template_id)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (!first)
            {
                _text += "\n\n";
            }
            if (flag)
            {
                _text += "[FFFF00]";
            }
            else
            {
                _text += "[777777]";
            }
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

    public string get_jbex_text(int jbex_id, bool first)
    {
        string _text = "";
        s_t_ji_banex _jbex = game_data._instance.get_t_ji_banex(jbex_id);
        if (!first)
        {
            _text += "\n\n";
        }
        _text += "◆【" + _jbex.name + "】" + game_data._instance.get_t_language("card_info.cs_238_37");//收集到
        bool flag = true;
        for (int i = 0; i < _jbex.tids.Count; ++i)
        {
            if (i != 0)
            {
                _text += "、";
            }
            if (sys._instance.m_self.has_card(_jbex.tids[i]))
            {
                _text += "[FFFF00]" + game_data._instance.get_t_class(_jbex.tids[i]).name + "[-]";
            }
            else
            {
                flag = false;
                _text += "[777777]" + game_data._instance.get_t_class(_jbex.tids[i]).name + "[-]";
            }
        }
        if (flag)
        {
            _text = "[FFFF00]" + _text;
        }
        else
        {
            _text = "[777777]" + _text;
        }
        _text += game_data._instance.get_t_language("card_info.cs_264_11") + "，";//时
        _text += game_data._instance.get_value_string(_jbex.attr, _jbex.value);

        return _text;
    }

    public string get_profession()
    {
        if (m_class.job == 1)
        {
            return game_data._instance.get_t_language("card_dialog_box.cs_372_10");//防
        }
        else if (m_class.job == 2)
        {
            return game_data._instance.get_t_language("card_dialog_box.cs_376_10");//物
        }

        return game_data._instance.get_t_language("card_dialog_box.cs_379_9");//魔
    }

    string get_jj(int level, int type = 0)
    {
        if (type == 0)
        {
            s_t_jinjie t_jinjie = game_data._instance.get_jinjie(level);
            return t_jinjie.name;
        }
        else
        {
            if (level == 1)
            {
                return string.Format(game_data._instance.get_t_language("card_info.cs_295_25"), "D");//{0}级猎人
            }
            else if (level == 3)
            {
                return string.Format(game_data._instance.get_t_language("card_info.cs_295_25"), "C");//{0}级猎人
            }
            else if (level == 6)
            {
                return string.Format(game_data._instance.get_t_language("card_info.cs_295_25"), "B");//{0}级猎人
            }
            else if (level == 9)
            {
                return string.Format(game_data._instance.get_t_language("card_info.cs_295_25"), "A");//{0}级猎人
            }
            else if (level == 12)
            {
                return string.Format(game_data._instance.get_t_language("card_info.cs_295_25"), "S");//{0}级猎人
            }
            else
            {
                return game_data._instance.get_t_language("card_info.cs_315_11");//王牌猎人
            }
        }
    }

    public void show_info()
    {
        m_scroll_view.GetComponent<UIScrollView>().ResetPosition();
        GameObject icon = icon_manager._instance.create_card_icon_ex(m_class.id, 0, 0, 0);
        sys._instance.remove_child(m_icon);
        icon.transform.parent = m_icon.transform;
        icon.transform.localPosition = new Vector3(0, 0, 0);
        icon.transform.localScale = new Vector3(1, 1, 1);
        icon.GetComponent<BoxCollider>().enabled = false;
        m_name.GetComponent<UILabel>().text = m_class.name;

        if (m_class.gender == 2)
        {
            m_xb.GetComponent<UILabel>().text = game_data._instance.get_t_language("card_info.cs_338_40");//女
        }
        else
        {
            m_xb.GetComponent<UILabel>().text = game_data._instance.get_t_language("card_info.cs_342_40");//男
        }

        m_zy.GetComponent<UILabel>().text = get_profession();
        m_xj.GetComponent<UILabel>().text = m_class.pz.ToString();
        m_des.GetComponent<UILabel>().text = "";

        TypewriterEffect _effect = m_des.GetComponent<TypewriterEffect>();

        if (_effect != null)
        {
            Object.Destroy(_effect);
        }

        _effect = m_des.AddComponent<TypewriterEffect>();

        _effect.charsPerSecond = 40;
        _effect.mFullText = m_class.card;
        if (m_card.m_player == null)
        {
            m_card.m_player = sys._instance.m_self;
        }
        m_card.update_role_attr();
        List<double> attrs = ccard.get_role_org_attr(m_card.get_template_id(), m_card.get_level(), m_card.get_jlevel(), m_card.get_glevel(), m_card.get_pinzhi());
        int _hp = (int)attrs[1];
        int _damage = (int)attrs[2];
        int _pd = (int)attrs[3];
        int _md = (int)attrs[4];
        int _speed = (int)attrs[5];
        double _bj = attrs[11];
        double _kb = attrs[18];
        double _mz = attrs[16];
        double _sb = attrs[17];
        double _ct = attrs[19];
        double _gd = attrs[12];
        double _wm = attrs[14];
        double _mm = attrs[15];
        double _zs = attrs[20];
        double _js = attrs[21];
        double _pvpzs = attrs[35];
        double _pvpjs = attrs[36];
        double miefang = attrs[23];
        double miewu = attrs[24];
        double miemo = attrs[25];
        double kf = attrs[26];
        double kw = attrs[27];
        double km = attrs[28];

        m_sx.transform.Find("hp/hp").GetComponent<UILabel>().text = _hp.ToString();
        m_sx.transform.Find("attack/attack").GetComponent<UILabel>().text = _damage.ToString();
        m_sx.transform.Find("pd/pd").GetComponent<UILabel>().text = _pd.ToString();
        m_sx.transform.Find("md/md").GetComponent<UILabel>().text = _md.ToString();
        m_sx.transform.Find("speed/speed").GetComponent<UILabel>().text = _speed.ToString();

        string text = game_data._instance.get_float_string(11, (float)_bj).Split('+')[1];
        m_g_sx.transform.Find("bj/bj").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(18, (float)_kb).Split('+')[1];
        m_g_sx.transform.Find("kb/kb").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(16, (float)_mz).Split('+')[1];
        m_g_sx.transform.Find("mz/mz").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(17, (float)_sb).Split('+')[1];
        m_g_sx.transform.Find("sb/sb").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(19, (float)_ct).Split('+')[1];
        m_g_sx.transform.Find("ct/ct").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(12, (float)_gd).Split('+')[1];
        m_g_sx.transform.Find("gd/gd").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(14, (float)_wm).Split('+')[1];
        m_g_sx.transform.Find("wm/wm").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(15, (float)_mm).Split('+')[1];
        m_g_sx.transform.Find("mm/mm").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(20, (float)_zs).Split('+')[1];
        m_g_sx.transform.Find("zs/zs").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(21, (float)_js).Split('+')[1];
        m_g_sx.transform.Find("js/js").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(35, (float)_pvpzs).Split('+')[1];
        m_g_sx.transform.Find("pvpzs/pvpzs").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(36, (float)_pvpjs).Split('+')[1];
        m_g_sx.transform.Find("pvpjs/pvpjs").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(23, (float)miefang).Split('+')[1];
        m_g_sx.transform.Find("miefang/miefang").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(24, (float)miewu).Split('+')[1];
        m_g_sx.transform.Find("miewu/miewu").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(25, (float)miemo).Split('+')[1];
        m_g_sx.transform.Find("miemo/miemo").GetComponent<UILabel>().text = text;
        if (game_data._instance.get_float_string(26, (float)kf).Contains("+"))
        {
            text = game_data._instance.get_float_string(26, (float)kf).Split('+')[1];
        }
        else
        {
            text = game_data._instance.get_float_string(26, (float)kf).Split('-')[1];
        }

        m_g_sx.transform.Find("kf/kf").GetComponent<UILabel>().text = text;
        if (game_data._instance.get_float_string(27, (float)kw).Contains("+"))
        {
            text = game_data._instance.get_float_string(27, (float)kw).Split('+')[1];
        }
        else
        {
            text = game_data._instance.get_float_string(27, (float)kw).Split('-')[1];
        }

        m_g_sx.transform.Find("kw/kw").GetComponent<UILabel>().text = text;
        if (game_data._instance.get_float_string(28, (float)km).Contains("+"))
        {
            text = game_data._instance.get_float_string(28, (float)km).Split('+')[1];
        }
        else
        {
            text = game_data._instance.get_float_string(28, (float)km).Split('-')[1];
        }

        m_g_sx.transform.Find("km/km").GetComponent<UILabel>().text = text;

        UILabel _skill_label = m_ji_nen.transform.Find("des").GetComponent<UILabel>();
        string _skill_des = "[ffff00]" + game_data._instance.get_t_language("card_dialog_box.cs_144_36");//普通攻击
        role_skill _skill = m_card.m_skills[0];
        if (_skill.m_t_skill.attack_type == 1)
        {
            _skill_des += "[ff0000][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2726_26") + "]\n";//物理
        }
        else if (_skill.m_t_skill.attack_type == 2)
        {
            _skill_des += "[7fa0ff][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
        }
        _skill_des += "[0aabff]" + _skill.get_des();

        _skill = m_card.m_skills[1];
        if (_skill != null)
        {
            _skill_des += "\n\n";
            _skill_des += "[ffff00]" + _skill.m_t_skill.name;
            if (_skill.m_t_skill.attack_type == 1)
            {
                _skill_des += "[ff0000][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2726_26") + "]\n";//物理
            }
            else if (_skill.m_t_skill.attack_type == 2)
            {
                _skill_des += "[7fa0ff][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
            }
            else
            {
                _skill_des += "\n";
            }
            _skill_des += "[0aabff]" + _skill.get_des();
            _skill_des += "\n\n";
        }
        s_t_role_skillunlock t_role_skilunlock = game_data._instance.get_t_role_skillunlock(m_card.get_template_id(), m_card.get_role().bskill_level + 1);
        if (t_role_skilunlock != null)
        {
            int index = 0;
            for (int i = 0; i < m_card.m_skills.Count; ++i)
            {
                if (m_card.m_skills[i].m_t_skill.id == t_role_skilunlock.id)
                {
                    break;
                }
                index++;
            }
            _skill_des += "[ffff00]" + t_role_skilunlock.name + " (" + string.Format(game_data._instance.get_t_language("card_info.cs_479_85"), (int)e_open_level.el_skill_zs) + ")" + "\n";//{0}级激活后开启
            _skill_des += "[0aabff]" + m_card.m_skills[index].get_des(0);
            _skill_des += "\n\n";
        }

        int _tp_level = 1;

        for (int i = (int)e_skill_type.skill_type_glevel_1; i < (int)e_skill_type.skill_end; ++i)
        {
            _skill = m_card.m_skills[i];
            if (_skill != null)
            {
                _skill_des += "[ffff00]" + _skill.m_t_skill.name + " " + "(+" + string.Format(game_data._instance.get_t_language("card_dialog_box.cs_184_83"), _tp_level.ToString()) + ")\n";//突破{0}开启
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
                _skill_des += "[ffff00]" + _skill.m_t_skill.name + " " + "(" + get_jj(num) + game_data._instance.get_t_language("card_dialog_box.cs_205_84") + ")\n";//开启
                _skill_des += "[0aabff]" + _skill.get_des();
                _skill_des += "\n\n";
                _tp_level++;
            }

        }

        _skill_label.GetComponent<UILabel>().text = _skill_des;

        ///成长属性
        Transform _obj = m_cheng_zhang.transform.Find("hp/hp");
        double _f = m_class.cz[0] + m_class.czcz[0] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

        _obj = m_cheng_zhang.transform.Find("pd/pd");
        _f = m_class.cz[2] + m_class.czcz[2] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

        _obj = m_cheng_zhang.transform.Find("md/md");
        _f = m_class.cz[3] + m_class.czcz[3] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

        _obj = m_cheng_zhang.transform.Find("attack/attack");
        _f = m_class.cz[1] + m_class.czcz[1] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

        _obj = m_cheng_zhang.transform.Find("speed/speed");
        _f = m_class.cz[4] + m_class.czcz[4] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

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
        m_ji_ban.transform.GetComponent<UILabel>().text = _zh;
        s_message mes = new s_message();
        mes.m_type = "show_ui_unit_cam";
        mes.m_object.Add(new Vector3(0, 1f, 7.5f));
        mes.m_object.Add(new Vector3(0, 180, 0));
        mes.m_object.Add(m_card);
        cmessage_center._instance.add_message(mes);
    }

    public void show_info_t()
    {
        m_scroll_view.GetComponent<UIScrollView>().ResetPosition();
        GameObject icon = icon_manager._instance.create_card_icon_ex(m_card);

        sys._instance.remove_child(m_icon);
        icon.transform.parent = m_icon.transform;
        icon.transform.localPosition = new Vector3(0, 0, 0);
        icon.transform.localScale = new Vector3(1, 1, 1);
        icon.transform.GetComponent<BoxCollider>().enabled = false;
        m_name.GetComponent<UILabel>().text = m_class.name;

        if (m_class.gender == 2)
        {
            m_xb.GetComponent<UILabel>().text = game_data._instance.get_t_language("card_info.cs_338_40");//女
        }
        else
        {
            m_xb.GetComponent<UILabel>().text = game_data._instance.get_t_language("card_info.cs_342_40");//男
        }

        m_zy.GetComponent<UILabel>().text = get_profession();
        m_xj.GetComponent<UILabel>().text = m_class.pz.ToString();
        m_des.GetComponent<UILabel>().text = "";

        TypewriterEffect _effect = m_des.GetComponent<TypewriterEffect>();

        if (_effect != null)
        {
            Object.Destroy(_effect);
        }

        _effect = m_des.AddComponent<TypewriterEffect>();

        _effect.charsPerSecond = 40;
        _effect.mFullText = m_class.card;
        m_card.update_role_attr();
        int _hp = (int)m_card.get_attr(1);
        int _damage = (int)m_card.get_attr(2);
        int _pd = (int)m_card.get_attr(3);
        int _md = (int)m_card.get_attr(4);
        int _speed = (int)m_card.get_attr(5);

        double _bj = m_card.get_attr(11);
        double _kb = m_card.get_attr(18);
        double _mz = m_card.get_attr(16);
        double _sb = m_card.get_attr(17);
        double _ct = m_card.get_attr(19);
        double _gd = m_card.get_attr(12);
        double _wm = m_card.get_attr(14);
        double _mm = m_card.get_attr(15);
        double _zs = m_card.get_attr(20);
        double _js = m_card.get_attr(21);
        double _pvpzs = m_card.get_attr(35);
        double _pvpjs = m_card.get_attr(36);
        double miefang = m_card.get_attr(23);
        double miewu = m_card.get_attr(24);
        double miemo = m_card.get_attr(25);
        double kf = m_card.get_attr(26);
        double kw = m_card.get_attr(27);
        double km = m_card.get_attr(28);

        m_sx.transform.Find("hp/hp").GetComponent<UILabel>().text = _hp.ToString();
        m_sx.transform.Find("attack/attack").GetComponent<UILabel>().text = _damage.ToString();
        m_sx.transform.Find("pd/pd").GetComponent<UILabel>().text = _pd.ToString();
        m_sx.transform.Find("md/md").GetComponent<UILabel>().text = _md.ToString();
        m_sx.transform.Find("speed/speed").GetComponent<UILabel>().text = _speed.ToString();

        string text = game_data._instance.get_float_string(11, (float)_bj).Split('+')[1];
        m_g_sx.transform.Find("bj/bj").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(18, (float)_kb).Split('+')[1];
        m_g_sx.transform.Find("kb/kb").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(16, (float)_mz).Split('+')[1];
        m_g_sx.transform.Find("mz/mz").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(17, (float)_sb).Split('+')[1];
        m_g_sx.transform.Find("sb/sb").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(19, (float)_ct).Split('+')[1];
        m_g_sx.transform.Find("ct/ct").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(12, (float)_gd).Split('+')[1];
        m_g_sx.transform.Find("gd/gd").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(14, (float)_wm).Split('+')[1];
        m_g_sx.transform.Find("wm/wm").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(15, (float)_mm).Split('+')[1];
        m_g_sx.transform.Find("mm/mm").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(20, (float)_zs).Split('+')[1];
        m_g_sx.transform.Find("zs/zs").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(21, (float)_js).Split('+')[1];
        m_g_sx.transform.Find("js/js").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(35, (float)_pvpzs).Split('+')[1];
        m_g_sx.transform.Find("pvpzs/pvpzs").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(36, (float)_pvpjs).Split('+')[1];
        m_g_sx.transform.Find("pvpjs/pvpjs").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(23, (float)miefang).Split('+')[1];
        m_g_sx.transform.Find("miefang/miefang").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(24, (float)miewu).Split('+')[1];
        m_g_sx.transform.Find("miewu/miewu").GetComponent<UILabel>().text = text;
        text = game_data._instance.get_float_string(25, (float)miemo).Split('+')[1];
        m_g_sx.transform.Find("miemo/miemo").GetComponent<UILabel>().text = text;
        if (game_data._instance.get_float_string(26, (float)kf).Contains("+"))
        {
            text = game_data._instance.get_float_string(26, (float)kf).Split('+')[1];
        }
        else
        {
            text = game_data._instance.get_float_string(26, (float)kf).Split('-')[1];
        }

        m_g_sx.transform.Find("kf/kf").GetComponent<UILabel>().text = text;
        if (game_data._instance.get_float_string(27, (float)kw).Contains("+"))
        {
            text = game_data._instance.get_float_string(27, (float)kw).Split('+')[1];
        }
        else
        {
            text = game_data._instance.get_float_string(27, (float)kw).Split('-')[1];
        }

        m_g_sx.transform.Find("kw/kw").GetComponent<UILabel>().text = text;
        if (game_data._instance.get_float_string(28, (float)km).Contains("+"))
        {
            text = game_data._instance.get_float_string(28, (float)km).Split('+')[1];
        }
        else
        {
            text = game_data._instance.get_float_string(28, (float)km).Split('-')[1];
        }

        m_g_sx.transform.Find("km/km").GetComponent<UILabel>().text = text;


        UILabel _skill_label = m_ji_nen.transform.Find("des").GetComponent<UILabel>();
        string _skill_des = "[ffff00]" + game_data._instance.get_t_language("card_dialog_box.cs_144_36");//普通攻击
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

        s_t_role_skillunlock t_role_skilunlock = game_data._instance.get_t_role_skillunlock(m_card.get_template_id(), m_card.get_role().bskill_level + 1);
        if (t_role_skilunlock != null)
        {
            int index = 0;
            for (int i = 0; i < m_card.m_skills.Count; ++i)
            {
                if (m_card.m_skills[i].m_t_skill.id == t_role_skilunlock.id)
                {
                    break;
                }
                index++;
            }
            if (m_card.get_role().level >= (int)e_open_level.el_skill_zs)
            {
                _skill_des += "[ffff00]" + t_role_skilunlock.name + " (" + string.Format(game_data._instance.get_t_language("{0}级激活后开启){nn}"), (int)e_open_level.el_skill_zs);
                if (m_card.get_role().bskill_level == 0)
                {
                    _skill_des += "[0aabff]" + m_card.m_skills[index].get_des(m_card.get_role().bskill_level);
                }
                else
                {
                    _skill_des += "[0aabff]" + m_card.m_skills[index].get_des(m_card.get_role().bskill_level - 1);
                }
            }
            else
            {
                _skill_des += "[999999]" + t_role_skilunlock.name + " (" + string.Format(game_data._instance.get_t_language("{0}级激活后开启){nn}"), (int)e_open_level.el_skill_zs);
                if (m_card.get_role().bskill_level == 0)
                {
                    _skill_des += "[999999]" + m_card.m_skills[index].get_des_ex(m_card.get_role().bskill_level);
                }
                else
                {
                    _skill_des += "[999999]" + m_card.m_skills[index].get_des(m_card.get_role().bskill_level - 1);
                }
            }
            _skill_des += "\n\n";
        }

        int _tp_level = 1;

        for (int i = (int)e_skill_type.skill_type_glevel_1; i < (int)e_skill_type.skill_end; ++i)
        {
            _skill = m_card.m_skills[i];
            if (_skill != null)
            {
                if (m_card.is_skill((e_skill_type)i))
                {
                    _skill_des += "[ffff00]" + _skill.m_t_skill.name + "\n";
                    _skill_des += "[0aabff]" + _skill.get_des();
                }
                else
                {
                    _skill_des += "[999999]" + _skill.m_t_skill.name + " " + "(" + string.Format(game_data._instance.get_t_language("card_info.cs_792_81"), _tp_level.ToString()) + ")\n";//突破+{0}开启
                    _skill_des += "[999999]" + _skill.get_des_ex();
                }
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
                if (m_card.is_skill((e_skill_type)i))
                {
                    _skill_des += "[ffff00]" + _skill.m_t_skill.name + "\n";
                    _skill_des += "[0aabff]" + m_card.m_skills[i].get_des();
                }
                else
                {
                    int num = _tp_level * 3;
                    if (_tp_level == 0)
                    {
                        num = 1;
                    }
                    _skill_des += "[999999]" + _skill.m_t_skill.name + " " + "(" + get_jj(num, 1) + game_data._instance.get_t_language("card_dialog_box.cs_205_84") + ")\n";//开启
                    _skill_des += "[999999]" + m_card.m_skills[i].get_des_ex();
                }
                _tp_level++;
                _skill_des += "\n\n";
            }

        }

        _skill_label.GetComponent<UILabel>().text = _skill_des;

        Transform _obj = m_cheng_zhang.transform.Find("hp/hp");
        double _f = m_class.cz[0] + m_class.czcz[0] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

        _obj = m_cheng_zhang.transform.Find("pd/pd");
        _f = m_class.cz[2] + m_class.czcz[2] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

        _obj = m_cheng_zhang.transform.Find("md/md");
        _f = m_class.cz[3] + m_class.czcz[3] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

        _obj = m_cheng_zhang.transform.Find("attack/attack");
        _f = m_class.cz[1] + m_class.czcz[1] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

        _obj = m_cheng_zhang.transform.Find("speed/speed");
        _f = m_class.cz[4] + m_class.czcz[4] * m_card.get_glevel();

        _obj.GetComponent<UILabel>().text = " " + _f.ToString("f2");

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
        m_ji_ban.transform.GetComponent<UILabel>().text = _zh;
        s_message mes = new s_message();
        mes.m_type = "show_ui_unit_cam";
        mes.m_object.Add(new Vector3(0, 1f, 7.5f));
        mes.m_object.Add(new Vector3(0, 180, 0));
        mes.m_object.Add(m_card);
        cmessage_center._instance.add_message(mes);
    }

    public void click(GameObject obj)
    {
        if (obj.name == "role")
        {
            s_message mes = new s_message();
            mes.m_type = "action_ui_unit_cam";
            cmessage_center._instance.add_message(mes);
        }
        if (obj.name == "close")
        {
            s_message mes = new s_message();
            mes.m_type = "hide_ui_unit_cam";
            cmessage_center._instance.add_message(mes);
            this.gameObject.SetActive(false);
        }
    }
}
