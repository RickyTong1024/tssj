using System.Collections.Generic;
using UnityEngine;

public class role_skill
{
    public int m_index;
    public s_t_skill _t_skill;
    public s_t_skill m_t_skill
    {
        get
        {
            if (m_index == 1)
            {
                s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(m_role.pinzhi);
                return game_data._instance.get_t_skill(_t_skill.id + t_role_shengpin.zdjnjc);
            }
            return _t_skill;
        }
        set { _t_skill = value; }
    }
    public s_t_skill m_t_org_skill;
    public s_t_monster m_t_monster;
    public dhc.role_t m_role;

    public int level()
    {
        int _level = 1;

        if (m_role != null)
        {
            if (m_index == 1)
            {
                _level = m_role.jskill_level[0];
            }
            else if (m_index == 2)
            {
                _level = m_role.bskill_level;
            }
            else if (m_index >= (int)e_skill_type.skill_type_jlevel_1 && m_index < (int)e_skill_type.skill_type_glevel_1)
            {
                _level = m_role.jskill_level[m_index - (int)e_skill_type.skill_type_jlevel_1 + 1];
            }
        }
        else if (m_t_monster != null)
        {
            _level = m_t_monster.level;
        }

        if (_level < 1)
        {
            _level = 1;
        }

        return _level;
    }

    public string get_des()
    {
        return get_des(level());
    }

    public string get_des(int level)
    {
        string s = "[0aabff]" + m_t_skill.des;

        s = s.Replace("{{n1}}", "[-][0aff16]" + (get_attack_pe(level) * 100).ToString() + "[-][0aabff]");
        s = s.Replace("{{n2}}", "[-][0aff16]" + (get_buffer_attack_pe(level, 0) * 100).ToString() + "[-][0aabff]");
        s = s.Replace("{{n3}}", "[-][0aff16]" + (Mathf.Abs(get_buffer_modify_att_val(level, 0))).ToString("f2") + "[-][0aabff]");
        s = s.Replace("{{n7}}", "[-][0aff16]" + (get_buffer_attack_pe(level, 1) * 100).ToString() + "[-][0aabff]");
        s = s.Replace("{{n8}}", "[-][0aff16]" + (Mathf.Abs(get_buffer_modify_att_val(level, 1))).ToString("f2") + "[-][0aabff]");
        s = s.Replace("{{n4}}", "[-][0aff16]" + (Mathf.Abs(ex_type_val_0(level))).ToString() + "[-][0aabff]");
        s = s.Replace("{{n5}}", "[-][0aff16]" + (Mathf.Abs(ex_type_val_1(level))).ToString() + "[-][0aabff]");
        s = s.Replace("{{n6}}", "[-][0aff16]" + (Mathf.Abs(ex_type_val_2(level))).ToString() + "[-][0aabff]");
        if (m_t_skill.passive_modify_att_val > 0)
        {
            s = get_bd_des(m_role.pinzhi);
        }
        return s;
    }

    public string get_bd_des(int pingzhi, int type = 0)
    {
        s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(pingzhi);
        string color = "[0aff16]";
        if (type == 1)
        {
            color = "[0aabff]";
        }
        string s = "[0aabff]" + m_t_skill.des;
        s = s.Replace("{{n1}}", "[-]" + color + (m_t_skill.passive_modify_att_val + m_t_skill.passive_modify_att_val_add * t_role_shengpin.bdjnjc).ToString() + "[-][0aabff]");
        return s;
    }

    public string get_des_ex()
    {
        return get_des_ex(level());
    }

    public string get_des_ex(int level)
    {
        string s = m_t_skill.des;

        s = s.Replace("{{n1}}", (get_attack_pe(level) * 100).ToString());
        s = s.Replace("{{n2}}", (get_buffer_attack_pe(level, 0) * 100).ToString());
        s = s.Replace("{{n3}}", (Mathf.Abs(get_buffer_modify_att_val(level, 0))).ToString("f2"));
        s = s.Replace("{{n7}}", (get_buffer_attack_pe(level, 1) * 100).ToString());
        s = s.Replace("{{n8}}", (Mathf.Abs(get_buffer_modify_att_val(level, 1))).ToString("f2"));
        s = s.Replace("{{n4}}", (Mathf.Abs(ex_type_val_0(level))).ToString());
        s = s.Replace("{{n5}}", (Mathf.Abs(ex_type_val_1(level))).ToString());
        s = s.Replace("{{n6}}", (Mathf.Abs(ex_type_val_2(level))).ToString());
        if (m_t_skill.passive_modify_att_val > 0)
        {
            s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(m_role.pinzhi);
            s = m_t_skill.des;
            s = s.Replace("{{n1}}", (m_t_skill.passive_modify_att_val + m_t_skill.passive_modify_att_val_add * t_role_shengpin.bdjnjc).ToString());
        }
        return s;
    }

    public float ex_type_val_0(int level)
    {
        return m_t_skill.base_ex_type_val_0;
    }

    public float ex_type_val_1(int level)
    {
        return m_t_skill.base_ex_type_val_1 + m_t_skill.add_ex_type_val_1 * level;
    }

    public float ex_type_val_2(int level)
    {
        return m_t_skill.base_ex_type_val_2 + m_t_skill.add_ex_type_val_2 * level;
    }

    public float get_attack_pe(int level)
    {
        return m_t_skill.attack_pe + m_t_skill.attack_pe_add * level;
    }
    public float get_buffer_attack_pe(int level, int col)
    {
        return m_t_skill.buffer_attack_pes[col] + m_t_skill.buffer_attack_pe_adds[col] * level;
    }
    public float get_buffer_modify_att_val(int level, int col)
    {
        return m_t_skill.buffer_modify_att_vals[col] + m_t_skill.buffer_modify_att_val_adds[col] * level;
    }

};

public class ccard
{
    private static Color32[] s_t_color_array = new Color32[7]{new Color32(255, 0, 0, 255), new Color32(255,128,0,255), new Color32(255, 255,0, 255),
        new Color32(0,255, 0, 255), new Color32(0, 255, 255, 255), new Color32(0,0, 255, 255), new Color32(128, 0, 255, 255)};
    public s_t_class m_t_class;
    public s_t_monster m_t_monster;
    private dhc.role_t m_role;
    private List<double> m_attrs = new List<double>();
    public List<role_skill> m_skills = new List<role_skill>();
    public List<dhc.equip_t> m_equip = new List<dhc.equip_t>();
    public List<dhc.treasure_t> m_treasure = new List<dhc.treasure_t>();

    public player m_player;
    private bool m_need_update_attr = true;
    public static int m_attr_num = 50;

    public double get_attr(int type)
    {
        if (m_need_update_attr)
        {
            update_role_attr_ex();
        }
        return m_attrs[type];
    }

    void update_equip()
    {
        m_equip.Clear();

        if (m_player == null)
        {
            return;
        }

        int _pos = get_zhengxin_pos() * 4;

        for (int i = 0; i < m_role.zhuangbeis.Count; i++)
        {
            dhc.equip_t _equip = m_player.get_equip_guid(m_role.zhuangbeis[i]);
            if (_equip != null && _equip.template_id == 0)
            {
                m_equip.Add(null);
            }
            else
            {
                m_equip.Add(_equip);
            }
        }
    }

    void update_treasure()
    {
        m_treasure.Clear();

        if (m_player == null)
        {
            return;
        }

        int _pos = get_zhengxin_pos() * 4;

        for (int i = 0; i < m_role.treasures.Count; i++)
        {
            dhc.treasure_t _treasure = m_player.get_treasure_guid(m_role.treasures[i]);
            if (_treasure != null && _treasure.template_id == 0)
            {
                m_treasure.Add(null);
            }
            else
            {
                m_treasure.Add(_treasure);
            }
        }
    }

    int get_chong_sheng_yl()
    {
        int gold = 0;
        for (int i = 1; i < this.get_level(); i++)
        {
            s_t_exp _exp = game_data._instance.get_t_exp(i + 1);
            if (_exp != null)
            {
                gold += get_role_pz_exp(_exp);
            }
        }
        return gold;
    }
    List<s_t_reward> get_chong_sheng_item()
    {
        List<s_t_reward> rewards = new List<s_t_reward>();

        if (this.get_glevel() != 0)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = 2;
            reward.value1 = get_fragment_id();
            reward.value3 = 0;
            for (int i = 1; i <= this.get_glevel(); i++)
            {
                s_t_tupo t_tupo = game_data._instance.get_tupo(i);
                reward.value2 += t_tupo.sps[get_color() - 2];
            }
            rewards.Add(reward);
        }
        s_t_reward reward_skill = new s_t_reward();
        reward_skill.type = 2;
        reward_skill.value1 = 50090001;
        reward_skill.value3 = 0;
        for (int i = 0; i < this.get_role().jskill_level.Count; i++)
        {
            for (int j = 1; j < this.get_role().jskill_level[i]; j++)
            {
                s_t_skillup skill = game_data._instance.get_skillup(j + 1);
                reward_skill.value2 += skill.skillstone;
            }
        }
        for (int i = 0; i <= this.get_role().bskill_level; i++)
        {
            s_t_role_spskillup t_role_spskillup = game_data._instance.get_t_role_spskillup(i);
            if (t_role_spskillup == null)
            {
                continue;
            }
            reward_skill.value2 += t_role_spskillup.stone;
        }
        if (reward_skill.value2 > 0)
        {
            rewards.Add(reward_skill);
        }
        for (int i = 1; i <= this.get_jlevel(); i++)
        {
            s_t_jinjie t_jinjie = game_data._instance.get_jinjie(i);
            s_t_reward _reward = new s_t_reward();
            _reward.type = 2;
            _reward.value1 = t_jinjie.clty;
            _reward.value2 = t_jinjie.clty_num;
            s_t_reward _reward1 = new s_t_reward();
            _reward1.type = 2;
            _reward1.value1 = t_jinjie.clfy;
            if (this.get_prof() == 1)
            {
                _reward1.value2 = t_jinjie.clfy_num1;
            }
            else
            {
                _reward1.value2 = t_jinjie.clfy_num;
            }
            s_t_reward _reward11 = new s_t_reward();
            _reward11.type = 2;
            _reward11.value1 = t_jinjie.clgj;

            if (get_prof() == 2)
            {
                _reward11.value2 = t_jinjie.clgj_num1;
            }
            else
            {
                _reward11.value2 = t_jinjie.clgj_num;
            }

            s_t_reward _reward111 = new s_t_reward();
            _reward111.type = 2;
            _reward111.value1 = t_jinjie.clmf;
            if (get_prof() == 3)
            {
                _reward111.value2 = t_jinjie.clmf_num1;
            }
            else
            {
                _reward111.value2 = t_jinjie.clmf_num;
            }
            rewards.Add(_reward);
            rewards.Add(_reward1);
            rewards.Add(_reward11);
            rewards.Add(_reward111);
        }
        int temp_suipian = 0;
        int temp_red_power = 0;
        int temp_stone = 0;
        if (this.m_t_class.color >= 4)
        {
            int temp = m_t_class.pz * 100;
            while (temp != this.get_role().pinzhi)
            {
                s_t_role_shengpin _sheng = game_data._instance.get_t_role_shengpin(temp);
                s_t_role_shengpin _sheng_next = game_data._instance.get_t_role_shengpin(_sheng.next_pinzhi);
                temp_suipian += _sheng_next.suipian;
                temp_red_power += _sheng_next.hongsehuobanzhili;
                temp_stone += _sheng_next.shengpinshi;
                temp = _sheng.next_pinzhi;
            }

        }
        if (temp_suipian > 0)
        {
            s_t_reward _suipian = new s_t_reward();
            _suipian.type = 2;
            _suipian.value1 = get_fragment_id();
            _suipian.value2 = temp_suipian;
            _suipian.value3 = 0;
            rewards.Add(_suipian);

        }
        if (temp_red_power > 0)
        {
            s_t_reward _red = new s_t_reward();
            _red.type = 2;
            _red.value1 = 50110001;
            _red.value2 = temp_red_power;
            _red.value3 = 0;
            rewards.Add(_red);
        }
        if (temp_stone > 0)
        {
            s_t_reward _red = new s_t_reward();
            _red.type = 2;
            _red.value1 = 50130001;
            _red.value2 = temp_stone;
            _red.value3 = 0;
            rewards.Add(_red);
        }
        return rewards;
    }

    public int get_sp_pz()
    {
        int pz = this.get_pz();
        if (pz >= 4 && pz <= 6)
        {
            return 0;
        }
        if (pz >= 7 && pz <= 9)
        {
            return 1;
        }
        if (pz >= 10 && pz <= 12)
        {
            return 2;
        }
        return 0;
    }

    int get_chong_sheng_gold()
    {
        //等级
        int gold = 0;
        for (int i = 1; i < this.get_level(); i++)
        {
            s_t_exp _exp = game_data._instance.get_t_exp(i + 1);
            if (_exp != null)
            {
                gold += get_role_pz_exp(_exp);
            }
        }
        for (int i = 0; i < this.get_jlevel(); i++)
        {
            s_t_jinjie t_jinjie = game_data._instance.get_jinjie(i + 1);
            if (t_jinjie != null)
            {
                gold += t_jinjie.gold;

            }
            else
            {
                Debug.LogError(game_data._instance.get_t_language("ccard.cs_429_31"));//进阶表找不到
            }
        }
        for (int i = 0; i < this.get_glevel(); i++)
        {
            s_t_tupo t_tupo = game_data._instance.get_tupo(i + 1);
            if (t_tupo != null)
            {
                gold += t_tupo.cl_gold;

            }
            else
            {
                Debug.LogError(game_data._instance.get_t_language("ccard.cs_429_31"));//进阶表找不到
            }
        }
        for (int i = 0; i < this.get_role().jskill_level.Count; i++)
        {
            for (int j = 1; j < this.get_role().jskill_level[i]; j++)
            {
                s_t_skillup skill = game_data._instance.get_skillup(j + 1);
                gold += skill.gold;
            }
        }
        for (int i = 0; i <= this.get_role().bskill_level; i++)
        {
            s_t_role_spskillup t_role_spskillup = game_data._instance.get_t_role_spskillup(i);
            if (t_role_spskillup == null)
            {
                continue;
            }
            gold += t_role_spskillup.gold;
        }
        if (this.m_t_class.color >= 4)
        {
            int temp = m_t_class.pz * 100;
            while (temp != this.get_role().pinzhi)
            {
                s_t_role_shengpin _sheng = game_data._instance.get_t_role_shengpin(temp);
                s_t_role_shengpin _sheng_next = game_data._instance.get_t_role_shengpin(_sheng.next_pinzhi);
                gold += _sheng_next.gold;
                temp = _sheng.next_pinzhi;
            }

        }

        return gold;
    }
    int get_chongsheng_zhanhun()
    {
        int zhanhun = 0;
        if (this.m_t_class.color >= 4)
        {
            int temp = m_t_class.pz * 100;
            while (temp != this.get_role().pinzhi)
            {
                s_t_role_shengpin _sheng = game_data._instance.get_t_role_shengpin(temp);
                s_t_role_shengpin _sheng_next = game_data._instance.get_t_role_shengpin(_sheng.next_pinzhi);
                zhanhun += _sheng_next.zhanhun;
                temp = _sheng.next_pinzhi;
            }

        }
        return zhanhun;

    }

    public List<s_t_reward> get_chongsheng_reward()
    {
        List<s_t_reward> rewards = new List<s_t_reward>();
        if (get_chong_sheng_gold() > 0)
        {
            s_t_reward _reward1 = new s_t_reward();
            _reward1.type = 1;
            _reward1.value1 = 1;
            _reward1.value2 = get_chong_sheng_gold();
            _reward1.value3 = 0;
            rewards.Add(_reward1);
        }

        if (get_chong_sheng_yl() > 0)
        {
            s_t_reward _reward2 = new s_t_reward();
            _reward2.type = 1;
            _reward2.value1 = 7;
            _reward2.value2 = get_chong_sheng_yl();
            _reward2.value3 = 0;
            rewards.Add(_reward2);

        }
        if (get_chongsheng_zhanhun() > 0)
        {
            s_t_reward _reward3 = new s_t_reward();
            _reward3.type = 1;
            _reward3.value1 = 5;
            _reward3.value2 = get_chongsheng_zhanhun();
            _reward3.value3 = 0;
            rewards.Add(_reward3);

        }
        List<s_t_reward> rewards3 = get_chong_sheng_item();
        for (int i = 0; i < rewards3.Count; i++)
        {
            rewards.Add(rewards3[i]);
        }

        s_t_class _role = game_data._instance.get_t_class(this.get_template_id());
        return rewards;
    }


    public List<s_t_reward> get_jiegu_reward()
    {
        List<s_t_reward> rewards = get_chongsheng_reward();
        if (game_data._instance.get_t_class(get_template_id()).exp > 0)
        {
            s_t_reward reward = new s_t_reward();
            reward.type = 1;
            reward.value1 = 5;
            reward.value2 = game_data._instance.get_t_class(get_template_id()).exp;
            rewards.Add(reward);

        }
        return rewards;
    }

    public int get_zhengxin_pos()
    {
        for (int i = 0; i < m_player.m_t_player.zhenxing.Count; i++)
        {
            if (m_player.m_t_player.zhenxing[i] == get_guid())
            {
                return i;
            }
        }
        return -1;
    }

    public bool is_zheng()
    {
        if (m_player == null)
        {
            return false;
        }
        for (int j = 0; j < m_player.m_t_player.zhenxing.Count; ++j)
        {
            if (m_player.m_t_player.zhenxing[j] == get_guid())
            {
                return true;
            }
        }
        return false;
    }

    public ulong get_guid()
    {
        if (m_role != null)
        {
            return m_role.guid;
        }
        return 0;
    }
    public int get_template_id()
    {
        return m_t_class.id;
    }

    public int get_jlevel_color_id()
    {
        int _id = 0;

        if (m_role.jlevel >= 3 && m_role.jlevel <= 5)
        {
            _id = 1;
        }

        if (m_role.jlevel >= 6 && m_role.jlevel <= 8)
        {
            _id = 2;
        }

        if (m_role.jlevel >= 9 && m_role.jlevel <= 11)
        {
            _id = 3;
        }

        if (m_role.jlevel >= 12)
        {
            _id = 4;
        }
        return _id;
    }

    public int get_glevel()
    {
        if (m_t_monster != null)
        {
            return m_t_monster.glevel;
        }

        return m_role.glevel;
    }

    public int get_jlevel()
    {
        if (m_t_monster != null)
        {
            return m_t_monster.jlevel;
        }
        return m_role.jlevel;
    }

    public int get_pinzhi()
    {
        if (m_t_monster != null)
        {
            return 100;
        }
        return m_role.pinzhi;
    }

    public int get_color()
    {
        return m_t_class.color;
    }

    public int get_true_color()
    {
        if (m_t_monster != null)
        {
            return m_t_class.color + m_t_monster.pinzhi_skill;
        }
        s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(m_role.pinzhi);
        return t_role_shengpin.color;
    }

    public int get_level()
    {
        if (m_t_monster != null)
        {
            return m_t_monster.level;
        }
        return m_role.level;
    }

    public int get_prof()
    {
        return m_t_class.job;
    }

    public int get_pz()
    {
        return m_t_class.pz;
    }

    public string get_profession()
    {
        if (m_t_class.job == 1)
        {
            return "szsxtb_003";
        }
        else if (m_t_class.job == 2)
        {
            return "szsxtb_002";
        }

        return "szsxtb_001";
    }

    public dhc.role_t get_role()
    {
        return m_role;
    }

    public int get_jy_id()
    {
        return get_jy_id(m_t_class.id);
    }

    public static int get_jy_id(int id)
    {
        for (int i = 0; i < game_data._instance.m_dbc_item.get_y(); i++)
        {
            int _type = int.Parse(game_data._instance.m_dbc_item.get(4, i));

            if (_type == 3002)
            {
                int _id = int.Parse(game_data._instance.m_dbc_item.get(8, i));

                if (_id == id)
                {
                    return int.Parse(game_data._instance.m_dbc_item.get(0, i));
                }
            }
        }
        return 0;
    }

    public int get_fragment_id()
    {
        return get_fragment_id(m_t_class.id);
    }

    public static int get_fragment_id(int id)
    {
        for (int i = 0; i < game_data._instance.m_dbc_item.get_y(); i++)
        {
            int _type = int.Parse(game_data._instance.m_dbc_item.get(4, i));

            if (_type == 3001)
            {
                int _id = int.Parse(game_data._instance.m_dbc_item.get(9, i));

                if (_id == id)
                {
                    return int.Parse(game_data._instance.m_dbc_item.get(0, i));
                }
            }
        }
        return 0;
    }

    public int get_fragment_num()
    {
        for (int i = 0; i < game_data._instance.m_dbc_item.get_y(); i++)
        {
            int _type = int.Parse(game_data._instance.m_dbc_item.get(4, i));

            if (_type == 3001)
            {
                int _id = int.Parse(game_data._instance.m_dbc_item.get(9, i));

                if (_id == get_template_id())
                {
                    int id = int.Parse(game_data._instance.m_dbc_item.get(0, i));
                    return game_data._instance.get_item(id).def_2;
                }
            }
        }
        return 0;
    }

    public string get_color_name()
    {
        return get_color_name(m_t_class.name, get_true_color());
    }

    public static string get_color_name(string name, int color)
    {
        string _name = game_data._instance.get_name_color(color) + name + "[-]";
        return _name;
    }

    public static string get_static_color_name(string name)
    {
        int step = 0;
        string _name = "";
        Color32 _color;
        for (int i = 0; i < name.Length; ++i)
        {
            _color = Color32.Lerp(s_t_color_array[step % s_t_color_array.Length],
                                  s_t_color_array[(step + 1) % s_t_color_array.Length],
                                  0.5f);
            _name = _name + "[" + _color.r.ToString("x2") + _color.g.ToString("x2") +
                _color.b.ToString("x2") + "]" + name[i] + "[-]";

            step = step + 2;
        }
        return _name;
    }

    public void set_role(dhc.role_t role)
    {
        m_role = role;

        set_class(role.template_id);
    }

    public void set_monster(int id)
    {
        m_t_monster = game_data._instance.get_t_monster(id);

        if (m_t_monster == null)
        {
            Debug.Log(game_data._instance.get_t_language("ccard.cs_824_13") + id);//找不到单位id
        }
        set_class(m_t_monster.class_id);
    }

    void set_class(int id)
    {
        m_t_class = game_data._instance.get_t_class(id);

        if (m_t_class == null)
        {
            Debug.Log("null class" + id);
            return;
        }

        for (int i = 0; i < m_t_class.skills.Count; ++i)
        {
            s_t_skill _t_skill = game_data._instance.get_t_skill(m_t_class.skills[i]);

            if (_t_skill != null)
            {
                role_skill _skill = new role_skill();

                _skill.m_index = i;
                _skill.m_role = m_role;
                _skill.m_t_monster = m_t_monster;
                _skill.m_t_skill = game_data._instance.get_t_skill(m_t_class.skills[i]);

                m_skills.Add(_skill);
            }
            else
            {
                m_skills.Add(null);
            }
        }
    }

    public int get_skill_gl(e_skill_type type)
    {
        if (type >= e_skill_type.skill_type_glevel_1 && type < e_skill_type.skill_end)
        {
            return (int)type - 1;
        }

        return 0;
    }

    public bool is_skill(e_skill_type type)
    {
        if (type == e_skill_type.skill_type_active)
        {
            return true;
        }

        if (type >= e_skill_type.skill_type_glevel_1 && type < e_skill_type.skill_end)
        {
            int glevel = get_glevel();
            int s = (int)type - (int)e_skill_type.skill_type_glevel_1 + 1;
            return glevel >= s;
        }
        else if (type >= e_skill_type.skill_type_jlevel_1 && type < e_skill_type.skill_end)
        {
            int jlevel = get_jlevel();

            if (type == e_skill_type.skill_type_jlevel_1 && jlevel >= 1)
            {
                return true;
            }
            else if (type == e_skill_type.skill_type_jlevel_2 && jlevel >= 3)
            {
                return true;
            }
            else if (type == e_skill_type.skill_type_jlevel_3 && jlevel >= 6)
            {
                return true;
            }
            else if (type == e_skill_type.skill_type_jlevel_4 && jlevel >= 9)
            {
                return true;
            }
            else if (type == e_skill_type.skill_type_jlevel_5 && jlevel >= 12)
            {
                return true;
            }
        }

        return false;
    }

    public bool is_skill_ex(e_skill_type_ex type)
    {
        if (type == e_skill_type_ex.skill_type_active)
        {
            return true;
        }
        if (type >= e_skill_type_ex.skill_type_jlevel_1 && type < e_skill_type_ex.skill_end)
        {
            int jlevel = get_jlevel();

            if (type == e_skill_type_ex.skill_type_jlevel_1 && jlevel >= 1)
            {
                return true;
            }
            else if (type == e_skill_type_ex.skill_type_jlevel_2 && jlevel >= 3)
            {
                return true;
            }
            else if (type == e_skill_type_ex.skill_type_jlevel_3 && jlevel >= 6)
            {
                return true;
            }
            else if (type == e_skill_type_ex.skill_type_jlevel_4 && jlevel >= 9)
            {
                return true;
            }
            else if (type == e_skill_type_ex.skill_type_jlevel_5 && jlevel >= 12)
            {
                return true;
            }
        }

        return false;
    }

    public string is_guanghuan(e_skill_type type)
    {
        string s = "";
        int glevel = get_glevel();
        if (type == e_skill_type.skill_type_glevel_1)
        {
            if (glevel >= 1)
            {
                s = "[00ff00]" + game_data._instance.get_t_language("ccard.cs_956_21");//绿色光环开启
                return s;
            }
        }
        else if (type == e_skill_type.skill_type_glevel_3)
        {
            if (glevel >= 3)
            {
                s = "[00abff]" + game_data._instance.get_t_language("ccard.cs_964_21");//蓝色光环开启
                return s;
            }
        }
        else if (type == e_skill_type.skill_type_glevel_5)
        {
            if (glevel >= 5)
            {
                s = "[ff00ed]" + game_data._instance.get_t_language("ccard.cs_972_21");//紫色光环开启
                return s;
            }
        }
        return s;
    }

    public int get_role_pz_exp(s_t_exp t_exp)
    {
        int color = get_color();
        if (color == 2)
        {
            return t_exp.role_exp;
        }
        if (color == 3)
        {
            return t_exp.role_zi_exp;
        }
        if (color == 4)
        {
            return t_exp.role_jin_exp;
        }
        if (color == 5)
        {
            return t_exp.role_hong_exp;
        }
        return 999999;
    }

    public double get_fighting()
    {
        double force = 0;
        if (m_need_update_attr)
        {
            update_role_attr_ex();
        }

        /// 总计
        int _skill_level = 0;
        for (int i = 0; i < m_role.jskill_level.Count; i++)
        {
            _skill_level += m_role.jskill_level[i];
        }

        force += (m_attrs[1] * 0.25 + m_attrs[2] * 2 + m_attrs[3] * 5 + m_attrs[4] * 5 + m_attrs[5] * 5)
            * (1 + m_attrs[11] * 0.003f + m_attrs[12] * 0.003f + m_attrs[14] * 0.003f + m_attrs[15] * 0.003f
               + m_attrs[16] * 0.003f + m_attrs[17] * 0.003f + m_attrs[18] * 0.003f + m_attrs[19] * 0.003f
               + m_attrs[20] * 0.003f + m_attrs[21] * 0.003f + m_attrs[22] * 0.05f + m_attrs[23] * 0.0003f
               + m_attrs[24] * 0.0003f + m_attrs[25] * 0.0003f + m_attrs[26] * 0.0003f + m_attrs[27] * 0.0003f
               + m_attrs[28] * 0.0003f) * (1 + _skill_level * 0.002f);

        return force;
    }

    public void update_role_attr()
    {
        m_need_update_attr = true;
    }

    public void update_role_attr_ex()
    {
        m_need_update_attr = false;
        update_equip();
        update_treasure();
        m_attrs.Clear();
        for (int i = 0; i < m_attr_num; ++i)
        {
            m_attrs.Add(0);
        }

        get_player_dress_attrs();
        get_role_org_attr();
        get_role_jinjie_attr();
        get_role_equip_attr();
        get_role_equip_tz_attr();
        get_role_treasure_attr();
        get_role_skill_attr();
        get_role_tdskill_attr();
        get_role_jb_attr();
        get_role_jbex_attr();
        get_role_gongzhens_attr();
        get_role_guild_skill_attr();
        get_role_huiyi_attr();
        get_role_huiyi_chengjiu_attr();
        get_role_duixing_attr();
        get_role_duixing_skill_attr();
        get_role_guanghuan_attr();
        get_role_guanghuan_skill();
        get_role_guanghuan_target();
        get_role_chenghao();
        get_role_master_attr();
        get_role_duanwei_attr();
        get_pet_on_attr();
        get_role_pet_guard_attr();
        get_role_pet_target();

        m_attrs[3] += m_attrs[29];
        m_attrs[4] += m_attrs[29];
        m_attrs[1] += m_attrs[37];
        m_attrs[2] += m_attrs[37];
        m_attrs[3] += m_attrs[37];
        m_attrs[4] += m_attrs[37];
        m_attrs[5] += m_attrs[37];

        m_attrs[8] += m_attrs[30];
        m_attrs[9] += m_attrs[30];
        m_attrs[6] += m_attrs[38];
        m_attrs[7] += m_attrs[38];
        m_attrs[8] += m_attrs[38];
        m_attrs[9] += m_attrs[38];
        m_attrs[10] += m_attrs[38];

        if (m_t_class.skills[0] <= 9)
        {
            m_attrs[2] += m_attrs[31];
            m_attrs[7] += m_attrs[33];
        }
        else
        {
            m_attrs[2] += m_attrs[32];
            m_attrs[7] += m_attrs[34];
        }
        for (int j = 1; j <= 5; ++j)
        {
            m_attrs[j] = m_attrs[j] * (1 + m_attrs[j + 5] * 0.01f);
        }
    }

    void get_player_dress_attrs()
    {
        for (int i = 0; i < m_player.m_t_player.dress_ids.Count; ++i)
        {
            int dress_id = m_player.m_t_player.dress_ids[i];
            s_t_dress t_dress = game_data._instance.get_t_dress(dress_id);
            if (t_dress == null)
            {
                continue;
            }
            for (int j = 0; j < t_dress.attrs.Count; ++j)
            {
                m_attrs[t_dress.attrs[j].attr] += t_dress.attrs[j].value;
            }
        }
        for (int i = 0; i < game_data._instance.m_dbc_dress_target.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_dress_target.get(0, i));
            s_t_dress_target t_dress_target = game_data._instance.get_t_dress_target(id);

            if (t_dress_target.type == 2)
            {
                bool flag = true;
                for (int j = 0; j < t_dress_target.defs.Count; ++j)
                {
                    if (!m_player.has_dress(t_dress_target.defs[j]))
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    m_attrs[t_dress_target.attr1] += t_dress_target.value1;
                    m_attrs[t_dress_target.attr2] += t_dress_target.value2;
                    m_attrs[t_dress_target.attr3] += t_dress_target.value3;
                    m_attrs[t_dress_target.attr4] += t_dress_target.value4;
                }
            }
        }
        for (int i = 0; i < sys._instance.m_self.m_t_player.dress_achieves.Count; ++i)
        {
            int id = sys._instance.m_self.m_t_player.dress_achieves[i];
            s_t_dress_target t_dress_target = game_data._instance.get_t_dress_target(id);
            if (t_dress_target != null)
            {
                m_attrs[t_dress_target.attr1] += t_dress_target.value1;
                m_attrs[t_dress_target.attr2] += t_dress_target.value2;
                m_attrs[t_dress_target.attr3] += t_dress_target.value3;
                m_attrs[t_dress_target.attr4] += t_dress_target.value4;
            }
        }
    }

    void get_role_org_attr()
    {
        s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(get_pinzhi());
        if (t_role_shengpin == null)
        {
            return;
        }
        for (int j = 0; j < 5; ++j)
        {
            /// 伙伴基本属性
            double _sxper = 0;
            for (int i = 0; i <= m_role.jlevel; i++)
            {
                s_t_jinjie _jinjie = game_data._instance.get_jinjie(i);
                if (_jinjie == null)
                {
                    continue;
                }
                _sxper += _jinjie.sxper;
            }

            double cz = m_t_class.cz[j] * t_role_shengpin.cz[j] + m_t_class.czcz[j] * t_role_shengpin.cz[j] * m_role.glevel;
            double cs = m_t_class.cs[j] * t_role_shengpin.cs[j] + m_t_class.cscz[j] * t_role_shengpin.cs[j] * _sxper;
            m_attrs[j + 1] += cs + cz * (m_role.level - 1);
        }
        /// 伙伴原始属性
        if (m_t_class.job == 1)
        {
            m_attrs[12] += 10;
        }
        else if (m_t_class.job == 2)
        {
            m_attrs[11] += 10;
        }
        else if (m_t_class.job == 3)
        {
            m_attrs[17] += 10;
        }
    }

    void get_role_jinjie_attr()
    {
        float val = game_data._instance.get_jinjie_point(m_role.jlevel);
        m_attrs[6] += val;
        m_attrs[7] += val;
        m_attrs[8] += val;
        m_attrs[9] += val;
        m_attrs[10] += val;
    }

    public static List<double> get_role_org_attr(int id, int level, int jlevel, int glevel, int pinzhi)
    {
        List<double> attrs = new List<double>();
        for (int i = 0; i < m_attr_num; ++i)
        {
            attrs.Add(0);
        }
        s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(pinzhi);
        if (t_role_shengpin == null)
        {
            return attrs;
        }
        s_t_class t_class = game_data._instance.get_t_class(id);
        if (t_class == null)
        {
            return attrs;
        }
        for (int j = 0; j < 5; ++j)
        {
            /// 伙伴基本属性
            double _sxper = 0;
            for (int i = 0; i <= jlevel; i++)
            {
                s_t_jinjie _jinjie = game_data._instance.get_jinjie(i);
                if (_jinjie == null)
                {
                    continue;
                }
                _sxper += _jinjie.sxper;
            }

            double cz = t_class.cz[j] * t_role_shengpin.cz[j] + t_class.czcz[j] * t_role_shengpin.cz[j] * glevel;
            double cs = t_class.cs[j] * t_role_shengpin.cs[j] + t_class.cscz[j] * t_role_shengpin.cs[j] * _sxper;
            attrs[j + 1] += cs + cz * (level - 1);
        }
        /// 伙伴原始属性
        if (t_class.job == 1)
        {
            attrs[12] = 10;
        }
        else if (t_class.job == 2)
        {
            attrs[11] = 10;
        }
        else if (t_class.job == 3)
        {
            attrs[17] = 10;
        }
        attrs[3] += attrs[29];
        attrs[4] += attrs[29];
        attrs[8] += attrs[30];
        attrs[9] += attrs[30];
        if (t_class.skills[0] <= 9)
        {
            attrs[2] += attrs[30];
            attrs[7] += attrs[32];
        }
        else
        {
            attrs[2] += attrs[31];
            attrs[7] += attrs[33];
        }
        return attrs;
    }

    void get_role_equip_attr()
    {
        for (int j = 0; j < m_equip.Count; ++j)
        {
            dhc.equip_t equip = m_equip[j];
            if (equip == null)
            {
                continue;
            }
            s_t_equip t_equip = game_data._instance.get_t_equip(equip.template_id);
            if (t_equip == null)
            {
                continue;
            }
            s_t_equip_sx t_equip_sx = game_data._instance.get_t_equip_sx(t_equip.font_color, equip.star);
            if (t_equip_sx == null)
            {
                continue;
            }
            m_attrs[t_equip.eattr.attr] += t_equip.eattr.value + t_equip.eattr.value * t_equip_sx.enhance_rate * equip.enhance;
            for (int k = 0; k < t_equip.ejlattr.Count; ++k)
            {
                m_attrs[t_equip.ejlattr[k].attr] += t_equip.ejlattr[k].value * equip.jilian;
            }
            for (int k = 0; k < equip.rand_ids.Count; ++k)
            {
                m_attrs[equip.rand_ids[k]] += equip.rand_values[k];
            }
            for (int k = 0; k < equip.stone.Count; ++k)
            {
                s_t_item t_item = game_data._instance.get_item(equip.stone[k]);
                if (t_item == null)
                {
                    continue;
                }
                m_attrs[t_item.def_2] += t_item.def_3;
            }
            if (t_equip.font_color == 5)
            {
                for (int i = 0; i < game_data._instance.m_dbc_equip_skill.get_y(); ++i)
                {
                    int id = int.Parse(game_data._instance.m_dbc_equip_skill.get(0, i));
                    s_t_equip_skill t_equip_skill = game_data._instance.get_t_equip_skill(id);
                    if (t_equip_skill.type == 1 && t_equip_skill.part == t_equip.type && t_equip_skill.jinglian <= equip.jilian)
                    {
                        m_attrs[t_equip_skill.def1] += t_equip_skill.def2;
                    }
                }
            }
        }
    }

    void get_role_equip_tz_attr()
    {
        HashSet<ulong> equip_set = new HashSet<ulong>();
        for (int j = 0; j < m_equip.Count; ++j)
        {
            if (m_equip[j] == null)
            {
                continue;
            }
            dhc.equip_t equip = m_equip[j];
            if (equip == null)
            {
                continue;
            }
            ulong equip_guid = equip.guid;
            if (equip_set.Contains(equip_guid))
            {
                continue;
            }
            equip_set.Add(equip_guid);
            s_t_equip_tz t_equip_tz = game_data._instance.get_t_equip_tz(equip.template_id);
            if (t_equip_tz == null)
            {
                continue;
            }
            int num = 1;
            for (int k = j + 1; k < m_equip.Count; ++k)
            {
                if (m_equip[k] == null)
                {
                    continue;
                }
                dhc.equip_t equip1 = m_equip[k];
                if (equip == null)
                {
                    continue;
                }
                ulong equip_guid1 = equip1.guid;
                if (equip_set.Contains(equip_guid1))
                {
                    continue;
                }
                if (!t_equip_tz.has_equip_id(equip1.template_id))
                {
                    continue;
                }
                num++;
                equip_set.Add(equip_guid1);
            }
            if (num >= 2)
            {
                m_attrs[t_equip_tz.attr1] += t_equip_tz.value1;
            }
            if (num >= 3)
            {
                m_attrs[t_equip_tz.attr2] += t_equip_tz.value2;
            }
            if (num >= 4)
            {
                m_attrs[t_equip_tz.attr3] += t_equip_tz.value3;
                m_attrs[t_equip_tz.attr4] += t_equip_tz.value4;
            }
        }
    }

    void get_role_treasure_attr()
    {
        for (int j = 0; j < m_treasure.Count; ++j)
        {
            dhc.treasure_t treasure = m_treasure[j];
            if (treasure == null)
            {
                continue;
            }
            s_t_baowu t_treasure = game_data._instance.get_t_baowu(treasure.template_id);
            if (t_treasure == null)
            {
                continue;
            }
            m_attrs[t_treasure.attr1] += t_treasure.value1 + t_treasure.value1 * treasure.enhance;
            m_attrs[t_treasure.attr2] += t_treasure.value2 + t_treasure.value2 * treasure.enhance;
            m_attrs[t_treasure.jl_type_0] += t_treasure.jl_value_0 * treasure.jilian;
            m_attrs[t_treasure.jl_type_1] += t_treasure.jl_value_1 * treasure.jilian;
            m_attrs[t_treasure.jl_type_2] += t_treasure.jl_value_2 * treasure.jilian;

            if (t_treasure.font_color == 5)
            {
                for (int i = 0; i < game_data._instance.m_dbc_equip_skill.get_y(); ++i)
                {
                    int id = int.Parse(game_data._instance.m_dbc_equip_skill.get(0, i));
                    s_t_equip_skill t_equip_skill = game_data._instance.get_t_equip_skill(id);
                    if (t_equip_skill.type == 1 && t_equip_skill.part == t_treasure.type + 4 && t_equip_skill.jinglian <= treasure.jilian)
                    {
                        m_attrs[t_equip_skill.def1] += t_equip_skill.def2;
                    }
                }
            }

            {
                double val = 0;
                s_t_baowu_sx t_treasure_sx = game_data._instance.get_t_baowu_sx(treasure.star, t_treasure.font_color);
                if (t_treasure_sx != null)
                {
                    val = t_treasure_sx.valuemax;
                }
                s_t_baowu_sx t_treasure_sx1 = game_data._instance.get_t_baowu_sx(treasure.star + 1, t_treasure.font_color);
                if (t_treasure_sx1 != null)
                {
                    val += t_treasure_sx1.value1 * treasure.star_exp;
                }
                if (t_treasure.type == 1)
                {
                    m_attrs[2] += val;
                }
                else
                {
                    m_attrs[29] += val;
                }
            }
        }
    }

    void get_role_skill_attr()
    {
        for (int j = (int)e_skill_type.skill_type_glevel_1; j < (int)e_skill_type.skill_end; ++j)
        {
            if (m_skills[j] == null)
            {
                continue;
            }
            s_t_skill t_skill = game_data._instance.get_t_skill(m_skills[j].m_t_skill.id);
            if (t_skill == null)
            {
                continue;
            }
            s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(get_pinzhi());
            if (t_role_shengpin == null)
            {
                continue;
            }
            if (t_skill.type != 2 || t_skill.passive_type != 1)
            {
                continue;
            }
            if (j - (int)e_skill_type.skill_type_glevel_1 >= m_role.glevel)
            {
                break;
            }
            m_attrs[t_skill.passive_modify_att_type] += t_skill.passive_modify_att_val + t_skill.passive_modify_att_val_add * t_role_shengpin.bdjnjc;
        }
    }

    void get_role_tdskill_attr()
    {
        for (int i = 0; i < m_player.m_t_player.zhenxing.Count; ++i)
        {
            ccard card = m_player.get_card_guid(m_player.m_t_player.zhenxing[i]);
            if (card == null)
            {
                continue;
            }
            for (int j = (int)e_skill_type.skill_type_glevel_1; j < (int)e_skill_type.skill_end; ++j)
            {
                if (card.m_skills[j] == null)
                {
                    continue;
                }
                s_t_skill t_skill = game_data._instance.get_t_skill(card.m_skills[j].m_t_skill.id);
                if (t_skill == null)
                {
                    continue;
                }
                s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(card.get_pinzhi());
                if (t_role_shengpin == null)
                {
                    continue;
                }
                if (t_skill.type != 2 || t_skill.passive_type != 2)
                {
                    continue;
                }
                if (j - (int)e_skill_type.skill_type_glevel_1 >= card.get_glevel())
                {
                    break;
                }
                m_attrs[t_skill.passive_modify_att_type] += t_skill.passive_modify_att_val + t_skill.passive_modify_att_val_add * t_role_shengpin.bdjnjc;
            }
        }
    }

    void get_role_jb_attr()
    {
        if (!is_zheng())
        {
            return;
        }
        HashSet<int> role_ids = new HashSet<int>();
        for (int i = 0; i < m_player.m_t_player.zhenxing.Count; ++i)
        {
            ccard card = m_player.get_card_guid(m_player.m_t_player.zhenxing[i]);
            if (card == null)
            {
                continue;
            }
            role_ids.Add(card.get_template_id());
        }
        for (int i = 0; i < m_player.m_t_player.houyuan.Count; ++i)
        {
            ccard card = m_player.get_card_guid(m_player.m_t_player.houyuan[i]);
            if (card == null)
            {
                continue;
            }
            role_ids.Add(card.get_template_id());
        }
        HashSet<int> equip_ids = new HashSet<int>();
        for (int i = 0; i < m_equip.Count; ++i)
        {
            dhc.equip_t equip = m_equip[i];
            if (equip == null)
            {
                continue;
            }
            equip_ids.Add(equip.template_id);
        }
        HashSet<int> treasure_ids = new HashSet<int>();
        for (int i = 0; i < m_treasure.Count; ++i)
        {
            dhc.treasure_t treasure = m_treasure[i];
            if (treasure == null)
            {
                continue;
            }
            treasure_ids.Add(treasure.template_id);
        }

        for (int j = 0; j < m_t_class.jbs.Count; ++j)
        {
            s_t_ji_ban t_jiban = game_data._instance.get_t_ji_ban(m_t_class.jbs[j]);
            if (t_jiban == null)
            {
                continue;
            }
            bool flag = true;
            if (t_jiban.type == 1)
            {
                for (int k = 0; k < t_jiban.tids.Count; ++k)
                {
                    if (!role_ids.Contains(t_jiban.tids[k]))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else if (t_jiban.type == 2)
            {
                for (int k = 0; k < t_jiban.tids.Count; ++k)
                {
                    if (!equip_ids.Contains(t_jiban.tids[k]))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            else if (t_jiban.type == 3)
            {
                for (int k = 0; k < t_jiban.tids.Count; ++k)
                {
                    if (!treasure_ids.Contains(t_jiban.tids[k]))
                    {
                        flag = false;
                        break;
                    }
                }
            }
            if (flag)
            {
                m_attrs[t_jiban.attr1] += t_jiban.value1;
                m_attrs[t_jiban.attr2] += t_jiban.value2;
            }
        }
    }

    void get_role_jbex_attr()
    {
        Dictionary<int, ulong> player_roles = new Dictionary<int, ulong>();
        for (int i = 0; i < m_player.m_t_player.roles.Count; ++i)
        {
            ccard card = m_player.get_card_guid(m_player.m_t_player.roles[i]);
            if (card == null)
            {
                continue;
            }
            player_roles[card.get_template_id()] = card.get_guid();
        }
        for (int j = 0; j < m_t_class.jbexs.Count; ++j)
        {
            s_t_ji_banex t_jibanex = game_data._instance.get_t_ji_banex(m_t_class.jbexs[j]);
            if (t_jibanex == null)
            {
                continue;
            }
            bool flag = true;
            for (int k = 0; k < t_jibanex.tids.Count; ++k)
            {
                if (!player_roles.ContainsKey(t_jibanex.tids[k]))
                {
                    flag = false;
                    break;
                }
            }
            if (flag)
            {
                m_attrs[t_jibanex.attr] += t_jibanex.value;
            }
        }
    }

    void get_role_gongzhens_attr()
    {
        s_t_gongzhen t_gongzheng = null;
        int min_enhanc_level = 99999;
        int min_jl_level = 99999;

        bool has_equip = (m_equip.Count == 4) ? true : false;
        dhc.equip_t equip = null;
        for (int i = 0; i < m_equip.Count; ++i)
        {
            equip = m_equip[i];
            if (equip == null)
            {
                has_equip = false;
                break;
            }
            if (equip.enhance < min_enhanc_level)
            {
                min_enhanc_level = equip.enhance;
            }
            if (equip.jilian < min_jl_level)
            {
                min_jl_level = equip.jilian;
            }
        }
        if (has_equip)
        {
            t_gongzheng = game_data._instance.get_t_gongzhen(1, min_enhanc_level);
            if (t_gongzheng != null)
            {
                for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
                {
                    m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
                }
            }
            t_gongzheng = game_data._instance.get_t_gongzhen(3, min_jl_level);
            if (t_gongzheng != null)
            {
                for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
                {
                    m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
                }
            }
        }

        min_enhanc_level = 99999;
        min_jl_level = 99999;
        bool has_treasure = (m_treasure.Count == 2) ? true : false;
        dhc.treasure_t treasure = null;
        for (int i = 0; i < m_treasure.Count; ++i)
        {
            treasure = m_treasure[i];
            if (treasure == null)
            {
                has_treasure = false;
                break;
            }
            if (treasure.enhance < min_enhanc_level)
            {
                min_enhanc_level = treasure.enhance;
            }
            if (treasure.jilian < min_jl_level)
            {
                min_jl_level = treasure.jilian;
            }
        }
        if (has_treasure)
        {
            t_gongzheng = game_data._instance.get_t_gongzhen(2, min_enhanc_level);
            if (t_gongzheng != null)
            {
                for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
                {
                    m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
                }
            }
            t_gongzheng = game_data._instance.get_t_gongzhen(4, min_jl_level);
            if (t_gongzheng != null)
            {
                for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
                {
                    m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
                }
            }
        }

        int skill_level = m_role.jskill_level[0];
        for (int i = (int)e_skill_type_ex.skill_type_jlevel_1; i < (int)e_skill_type_ex.skill_end; ++i)
        {
            int index = i - (int)e_skill_type_ex.skill_type_jlevel_1 + 1;
            if (is_skill_ex((e_skill_type_ex)i))
            {
                skill_level += m_role.jskill_level[index];
            }
        }
        t_gongzheng = game_data._instance.get_t_gongzhen(5, skill_level);
        if (t_gongzheng != null)
        {
            for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
            {
                m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
            }
        }

        min_enhanc_level = 99999;
        min_jl_level = 99999;
        bool has_houyuan = (m_player.m_t_player.houyuan.Count == 6) ? true : false;
        for (int i = 0; i < m_player.m_t_player.houyuan.Count; ++i)
        {
            ccard card = m_player.get_card_guid(m_player.m_t_player.houyuan[i]);
            if (card == null)
            {
                has_houyuan = false;
                break;
            }
            if (card.get_level() < min_enhanc_level)
            {
                min_enhanc_level = card.get_level();
            }
            if (card.get_glevel() < min_jl_level)
            {
                min_jl_level = card.get_glevel();
            }
        }
        if (has_houyuan)
        {
            t_gongzheng = game_data._instance.get_t_gongzhen(6, min_enhanc_level);
            if (t_gongzheng != null)
            {
                for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
                {
                    m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
                }
            }
            t_gongzheng = game_data._instance.get_t_gongzhen(7, min_jl_level);
            if (t_gongzheng != null)
            {
                for (int j = 0; j < t_gongzheng.attrs.Count; ++j)
                {
                    m_attrs[t_gongzheng.attrs[j]] += t_gongzheng.value1[j];
                }
            }
        }
    }

    void get_role_guild_skill_attr()
    {
        if (sys._instance.m_self.m_t_player.guild != 0)
        {
            for (int i = 0; i < sys._instance.m_self.m_t_player.guild_skill_ids.Count; i++)
            {
                s_t_guild_keji _keiji = game_data._instance.get_t_guild_keji(sys._instance.m_self.m_t_player.guild_skill_ids[i]);
                if (_keiji != null)
                {
                    m_attrs[_keiji.sx] += _keiji.sx_value * sys._instance.m_self.m_t_player.guild_skill_levels[i];
                }
            }
        }
    }

    void get_role_huiyi_attr()
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count; i++)
        {
            s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(sys._instance.m_self.m_t_player.huiyi_jihuos[i]);
            if (_sub != null)
            {
                for (int j = 0; j < _sub.attrs.Count; j++)
                {
                    m_attrs[_sub.attrs[j]] += _sub.values[j];
                    m_attrs[_sub.attrs[j]] += _sub.values2[j] * sys._instance.m_self.m_t_player.huiyi_jihuo_starts[i];
                }
            }
        }
    }

    void get_role_master_attr()
    {
        s_t_master_duanwei duanwei = game_data._instance.get_t_master_duanwei(sys._instance.m_self.m_t_player.ds_duanwei);
        if (duanwei != null)
        {
            m_attrs[duanwei.attr1] += duanwei.value1;
            m_attrs[duanwei.attr2] += duanwei.value2;
        }
    }

    void get_role_huiyi_chengjiu_attr()
    {
        for (int i = 0; i < game_data._instance.m_dbc_huiyi_chengjiu.get_y(); i++)
        {
            int _id = int.Parse(game_data._instance.m_dbc_huiyi_chengjiu.get(0, i));
            s_t_huiyi_chengjiu _chengjiu = game_data._instance.get_t_huiyi_chengjiu(_id);
            if (huiyilu_gui.is_finish_chengjiu(_id))
            {
                m_attrs[_chengjiu.attr] += _chengjiu.value;
            }
            else
            {
                break;
            }
        }
    }

    void get_role_duixing_attr()
    {
        int id = m_t_class.id;
        int index = -1;
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[i] == get_guid())
            {
                index = i;
                break;
            }
        }
        if (index == -1)
        {
            return;
        }
        int site = sys._instance.m_self.m_t_player.duixing[index];
        s_t_duixng t_duixing = game_data._instance.get_t_duixing(sys._instance.m_self.m_t_player.duixing_id);
        if (t_duixing == null)
        {
            return;
        }
        int duiwei = t_duixing.zhenwei[site];
        if (duiwei < 6)
        {
            m_attrs[t_duixing.q_attr] += t_duixing.q_value + t_duixing.q_cz * sys._instance.m_self.m_t_player.duixing_level;
            m_attrs[t_duixing.q_attr1] += t_duixing.q_value1 + t_duixing.q_cz1 * sys._instance.m_self.m_t_player.duixing_level;
        }
        else if (duiwei < 12)
        {
            m_attrs[t_duixing.z_attr] += t_duixing.z_value + t_duixing.z_cz * sys._instance.m_self.m_t_player.duixing_level;
            m_attrs[t_duixing.z_attr1] += t_duixing.z_value1 + t_duixing.z_cz1 * sys._instance.m_self.m_t_player.duixing_level;
        }
        else
        {
            m_attrs[t_duixing.h_attr] += t_duixing.h_value + t_duixing.h_cz * sys._instance.m_self.m_t_player.duixing_level;
            m_attrs[t_duixing.h_attr1] += t_duixing.h_value1 + t_duixing.h_cz1 * sys._instance.m_self.m_t_player.duixing_level;
        }
    }

    void get_role_duixing_skill_attr()
    {
        if (!is_zheng())
        {
            return;
        }
        for (int i = 0; i < game_data._instance.m_dbc_duixing_skill.get_y(); i++)
        {
            int _id = int.Parse(game_data._instance.m_dbc_duixing_skill.get(0, i));
            s_t_duixng_skill _duixing_skill = game_data._instance.get_t_duixing_skill(_id);
            if (sys._instance.m_self.m_t_player.duixing_level >= _duixing_skill.level)
            {
                for (int j = 0; j < _duixing_skill.attrs.Count; ++j)
                {
                    m_attrs[_duixing_skill.attrs[j].attr] += _duixing_skill.attrs[j].value;
                }
            }
            else
            {
                break;
            }
        }
    }


    void get_role_guanghuan_attr()
    {
        if (sys._instance.m_self.m_t_player.guanghuan_id > 0)
        {
            s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan(sys._instance.m_self.m_t_player.guanghuan_id);
            if (t_guanghuan != null)
            {
                int level = 0;
                for (int i = 0; i < sys._instance.m_self.m_t_player.guanghuan.Count; ++i)
                {
                    if (sys._instance.m_self.m_t_player.guanghuan[i] == sys._instance.m_self.m_t_player.guanghuan_id)
                    {
                        level = sys._instance.m_self.m_t_player.guanghuan_level[i];
                        break;
                    }
                }
                if (t_guanghuan.attr1 > 0)
                {
                    m_attrs[t_guanghuan.attr1] += t_guanghuan.value1 + t_guanghuan.value1 * 0.025f * level;
                }
                if (t_guanghuan.attr2 > 0)
                {
                    m_attrs[t_guanghuan.attr2] += t_guanghuan.value2;
                }
            }
        }
    }

    void get_role_guanghuan_skill()
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.guanghuan.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.guanghuan[i] == sys._instance.m_self.m_t_player.guanghuan_id)
            {
                int id = sys._instance.m_self.m_t_player.guanghuan[i];
                if (game_data._instance.m_guanghuan_skill_ids.ContainsKey(id))
                {
                    List<int> ids = game_data._instance.m_guanghuan_skill_ids[id];
                    for (int j = 0; j < ids.Count; ++j)
                    {
                        s_t_guanghuan_skill t_guanghuan_skill = game_data._instance.get_t_guanghuan_skill(ids[j]);
                        if (sys._instance.m_self.m_t_player.guanghuan_level[i] >= t_guanghuan_skill.enhance)
                        {
                            if (t_guanghuan_skill.type == 1)
                            {
                                m_attrs[t_guanghuan_skill.def1] += t_guanghuan_skill.def2;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
        }
    }

    void get_role_guanghuan_target()
    {
        foreach (s_t_guanghuan_target t_guanghuan_target in game_data._instance.m_guanghuan_targets.Values)
        {
            bool com = true;
            for (int i = 0; i < t_guanghuan_target.ids.Count; ++i)
            {
                bool has = false;
                for (int j = 0; j < sys._instance.m_self.m_t_player.guanghuan.Count; ++j)
                {
                    if (sys._instance.m_self.m_t_player.guanghuan[j] == t_guanghuan_target.ids[i])
                    {
                        has = true;
                        break;
                    }
                }
                if (!has)
                {
                    com = false;
                    break;
                }
            }
            if (com)
            {
                for (int i = 0; i < t_guanghuan_target.attrs.Count; ++i)
                {
                    m_attrs[t_guanghuan_target.attrs[i].attr] += t_guanghuan_target.attrs[i].value;
                }
            }
        }
    }

    void get_role_chenghao()
    {
        Dictionary<int, int> chs = new Dictionary<int, int>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.chenghao.Count; i++)
        {
            s_t_chenghao t_chenhao = game_data._instance.get_t_chenhao(sys._instance.m_self.m_t_player.chenghao[i]);
            if (t_chenhao != null)
            {
                int type = t_chenhao.type;
                if (chs.ContainsKey(type))
                {
                    if (chs[type] > t_chenhao.id)
                    {
                        chs[type] = t_chenhao.id;
                    }
                }
                else
                {
                    chs.Add(type, t_chenhao.id);
                }
            }
        }
        foreach (int id in chs.Values)
        {
            s_t_chenghao t_chenhao = game_data._instance.get_t_chenhao(id);
            if (t_chenhao != null)
            {
                for (int i = 0; i < t_chenhao.attr.Count; ++i)
                {
                    m_attrs[t_chenhao.attr[i].attr] += t_chenhao.attr[i].value;
                }
            }
        }
    }

    void get_role_duanwei_attr()
    {
        s_t_master_duanwei t_duanwei = game_data._instance.get_t_master_duanwei(sys._instance.m_self.m_t_player.ds_duanwei);
        if (t_duanwei != null)
        {
            m_attrs[t_duanwei.attr1] += t_duanwei.value1;
            m_attrs[t_duanwei.attr2] += t_duanwei.value2;
        }
    }

    void get_pet_attr(pet pt, Dictionary<int, double> pet_attrs)
    {
        dhc.pet_t pet = pt.get_pet();
        if (pet == null)
        {
            return;
        }
        for (int i = 0; i < pt.m_t_pet.cs.Count; i++)
        {
            pet_attrs.Add(i + 1, 0);
            pet_attrs[i + 1] = pt.m_t_pet.cs[i] + (pt.m_t_pet.cscz[i] + pt.m_t_pet.shengxing_cz[i] * pt.get_star()) * pt.get_level() +
                pt.m_t_pet.jinjie_sxcz[i] * pt.get_jlevel() + pt.m_t_pet.shengxing_sxcz[i] * pt.get_star();
        }

        s_t_pet_jinjieitem t_jinjie_item;
        for (int i = 0; i < pet.jinjie_slot.Count; i++)
        {
            if (pet.jinjie_slot[i] != 0)
            {
                t_jinjie_item = game_data._instance.get_t_pet_jinjieitem(pet.jinjie_slot[i]);
                if (t_jinjie_item != null)
                {
                    for (int j = 0; j < t_jinjie_item.attrs.Count; j++)
                    {
                        if (pet_attrs.ContainsKey(t_jinjie_item.attrs[j].attr))
                        {
                            pet_attrs[t_jinjie_item.attrs[j].attr] += t_jinjie_item.attrs[j].value;
                        }
                        else
                        {
                            pet_attrs.Add(t_jinjie_item.attrs[j].attr, t_jinjie_item.attrs[j].value);
                        }
                    }
                }
            }
        }

        int quan = 0;
        int[] extra = { 0, 0 };
        for (int i = 0; i <= pt.get_jlevel(); i++)
        {
            s_t_pet_jinjie t_jinjie = game_data._instance.get_t_pet_jinjie(i);
            if (t_jinjie != null)
            {
                if (i != pt.get_jlevel())
                {
                    for (int j = 0; j < t_jinjie.cls.Count; ++j)
                    {
                        t_jinjie_item = game_data._instance.get_t_pet_jinjieitem(t_jinjie.cls[j]);
                        if (t_jinjie_item != null)
                        {
                            for (int k = 0; k < t_jinjie_item.attrs.Count; ++k)
                            {
                                if (pet_attrs.ContainsKey(t_jinjie_item.attrs[k].attr))
                                {
                                    pet_attrs[t_jinjie_item.attrs[k].attr] += t_jinjie_item.attrs[k].value;
                                }
                                else
                                {
                                    pet_attrs.Add(t_jinjie_item.attrs[k].attr, t_jinjie_item.attrs[k].value);
                                }
                            }
                        }
                    }
                }
                quan += t_jinjie.qsx_add;
                if (t_jinjie.esx_add == 1)
                {
                    extra[0] += 1;
                }
                else if (t_jinjie.esx_add == 2)
                {
                    extra[1] += 1;
                }
            }
        }

        for (int i = 0; i < 2; ++i)
        {
            if (i == 0 && extra[i] > 0)
            {
                if (pet_attrs.ContainsKey(pt.m_t_pet.jinjie_add_sx[i].attr))
                {
                    pet_attrs[pt.m_t_pet.jinjie_add_sx[i].attr] += pt.m_t_pet.jinjie_add_sx[i].value * extra[i];
                }
                else
                {
                    pet_attrs.Add(pt.m_t_pet.jinjie_add_sx[i].attr, pt.m_t_pet.jinjie_add_sx[i].value * extra[i]);
                }
            }
        }

        if (quan > 0)
        {
            double jiancheng = (double)quan / 100.0;
            for (int i = 0; i < 4; i++)
            {
                pet_attrs[i + 1] *= (1 + jiancheng);
            }
        }
    }

    void get_pet_on_attr()
    {
        if (sys._instance.m_self.m_t_player.pet_on != 0)
        {
            pet pt = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pet_on);
            if (pt == null)
            {
                return;
            }
            Dictionary<int, double> pet_attrs = new Dictionary<int, double>();
            get_pet_attr(pt, pet_attrs);
            foreach (var item in pet_attrs)
            {
                m_attrs[item.Key] += item.Value * pt.m_t_pet.sz_sx_add;
            }
        }
    }

    void get_role_pet_guard_attr()
    {
        if (m_role.pet != 0)
        {
            pet pt = sys._instance.m_self.get_pet_guid(m_role.pet);
            if (pt == null)
            {
                return;
            }
            Dictionary<int, double> pet_attrs = new Dictionary<int, double>();
            get_pet_attr(pt, pet_attrs);
            foreach (var item in pet_attrs)
            {
                m_attrs[item.Key] += item.Value * pt.m_t_pet.sx_add;
            }
        }
    }

    void get_role_pet_target()
    {
        s_t_pet_target t_pet_target;
        foreach (int id in game_data._instance.m_dbc_chongwu_target.m_index.Keys)
        {
            t_pet_target = game_data._instance.get_t_pet_target(id);
            if (t_pet_target != null)
            {
                int has_pet = 0;
                for (int i = 0; i < t_pet_target.target_ids.Count; i++)
                {
                    for (int j = 0; j < sys._instance.m_self.m_t_player.pets.Count; ++j)
                    {
                        pet m_pet = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pets[j]);
                        if (m_pet != null && m_pet.m_t_pet.id == t_pet_target.target_ids[i])
                        {
                            has_pet++;
                            break;
                        }
                    }
                }
                if (has_pet >= t_pet_target.target_ids.Count)
                {
                    for (int k = 0; k < t_pet_target.attrs.Count; k++)
                    {
                        m_attrs[t_pet_target.attrs[k].attr] += t_pet_target.attrs[k].value;
                    }
                }
            }
        }
    }

    public double get_xq()
    {
        double _val = 1.0;

        if (m_role == null)
        {
            return _val;
        }
        if (m_role.xq == 1)
        {
            _val = 0.8;
        }
        if (m_role.xq == 2)
        {
            _val = 0.9;
        }
        if (m_role.xq == 3)
        {
            _val = 1.0;
        }
        if (m_role.xq == 4)
        {
            _val = 1.1;
        }
        if (m_role.xq == 5)
        {
            _val = 1.2;
        }
        return _val;
    }
    public double get_jj_sxper(int jj)
    {
        float _sxper = 0;
        for (int i = 0; i <= jj; i++)
        {
            s_t_jinjie _jinjie = game_data._instance.get_jinjie(i);
            if (_jinjie != null)
            {
                _sxper += _jinjie.sxper;
            }
        }

        return _sxper;
    }

    public static ccard get_new_card(int id)
    {
        ccard _card = new ccard();
        dhc.role_t _role = new dhc.role_t();

        _role.guid = 0;
        _role.level = 1;
        _role.player_guid = 0;
        _role.template_id = id;
        _role.bskill_level = 0;

        for (int i = 0; i < _role.bskill_counts.Count; ++i)
        {
            _role.bskill_counts[i] = 0;
        }

        for (int i = 0; i < 7; i++)
        {
            _role.jskill_level.Add(1);
        }

        _role.glevel = 0;
        _role.jlevel = 0;
        s_t_class t_class = game_data._instance.get_t_class(id);
        _role.pinzhi = t_class.pz * 100;

        for (int j = 0; j < 4; ++j)
        {
            _role.zhuangbeis.Add(0);
        }

        for (int j = 0; j < 2; ++j)
        {
            _role.treasures.Add(0);
        }

        _card.set_role(_role);

        return _card;
    }

    public void role_dress(ccard m_card)
    {
        dbc m_dbc_role_dress = game_data._instance.get_dbc_role_dress();
        for (int i = 0; i < m_dbc_role_dress.get_y(); i++)
        {
            int id = int.Parse(game_data._instance.m_dbc_role_dress.get(0, i));
            s_t_role_dress _dress = game_data._instance.get_t_role_dress(id);
            if (_dress.role == m_card.get_template_id() && _dress.hq_condition == 1 && m_card.get_role().glevel >= _dress.hq_Level)
            {
                m_card.get_role().dress_ids.Add(_dress.id);
            }
            if (_dress.role == m_card.get_template_id() && _dress.hq_condition == 2 && m_card.get_role().jlevel >= _dress.hq_Level)
            {
                m_card.get_role().dress_ids.Add(_dress.id);
            }
        }
    }
}
