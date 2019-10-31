using System.Collections.Generic;
using UnityEngine;

public class bu_zheng_panel : MonoBehaviour, IMessage
{
    private int m_select_card_id = 0;
    private int m_preselect_card_id = 0;
    private int m_select_equip_id = 0;
    private int m_select_treasure_id = 0;
    public GameObject m_select;
    public GameObject m_info;
    public GameObject m_des;
    public GameObject m_tihuan;
    public GameObject m_jibanex_des;
    public GameObject m_zuhe_view;
    public GameObject m_card_view;
    public GameObject m_view;
    public GameObject m_role;
    public GameObject m_chongneng;
    public GameObject m_jingjie;
    public GameObject m_tupo;
    public GameObject m_shengpin;
    public GameObject m_skill_gui;
    public GameObject m_name;
    public GameObject m_zb;
    public GameObject m_shizhuang;
    public GameObject m_shizhuang1;
    public GameObject m_show_Label;
    public GameObject m_down;
    public GameObject m_gongzheng_mubiao_gui;
    public GameObject m_duixing_effect;
    public int m_info_id = 0;
    public ccard m_card;
    public GameObject[] effect;
    public GameObject[] effect1;
    public GameObject[] effect2;
    public GameObject m_pet_effect;
    public List<GameObject> tx = new List<GameObject>();
    public GameObject m_xiangxi_panel;
    public GameObject m_peiyang_panel;
    public GameObject m_peiyang_button;
    public GameObject m_xiangxi_button;
    public GameObject m_look;
    public List<GameObject> m_cards = new List<GameObject>();
    public List<GameObject> m_equips = new List<GameObject>();
    public List<GameObject> m_treasures = new List<GameObject>();
    public List<GameObject> m_adds = new List<GameObject>();
    public List<GameObject> m_bw_adds = new List<GameObject>();
    public List<GameObject> level = new List<GameObject>();
    public List<GameObject> name = new List<GameObject>();
    public List<ccard> my_cards = new List<ccard>();
    public List<dhc.equip_t> m_old_equips = new List<dhc.equip_t>();
    public List<dhc.treasure_t> m_old_treasures = new List<dhc.treasure_t>();
    public GameObject m_xq;
    public GameObject m_yenhance;
    public GameObject m_gongzhen_button;
    public GameObject m_dacheng_des;
    public GameObject m_houyuan_gui;
    public GameObject m_bu_zheng_panel;
    public GameObject m_shenping_panel;
    public GameObject m_juexing_level;
    public GameObject m_top_res;
    public GameObject m_guanghuan_gui;
    public GameObject m_sx_panel;

    //光环
    public GameObject m_guanghuan_type;
    public GameObject m_guanghuan_sx;
    public GameObject m_guanghuan_jn;
    public GameObject m_guanghuan_name;
    public GameObject m_guanghuan_icon;
    public GameObject m_guanghuan_panel;
    public GameObject m_up_gui;
    public GameObject m_tj_gui;
    private int m_guanghuan_id;

    //宠物
    public GameObject m_pet_tihuan;
    public GameObject m_pet_jj;
    public GameObject m_pet_guard_gui;
    public GameObject m_pet_guard_scro;
    public GameObject m_pet_guard_detail;
    public GameObject m_pet_xiangxi_panel;
    public GameObject m_pet_peiyang_panel;
    public GameObject m_pet_icon;
    public GameObject m_pet_panel;
    public GameObject m_pet_peiyang_button;
    public GameObject m_pet_xiangxi_button;
    public GameObject m_pet_down;
    public UILabel m_pet_name;
    public UILabel m_pet_hp;
    public UILabel m_pet_attack;
    public UILabel m_pet_mf;
    public UILabel m_pet_wf;
    public UILabel m_pet_skill;
    public UILabel m_pet_desc;
    public GameObject m_none;
    public GameObject m_guard_icon;
    public UILabel m_guard_name;
    public UILabel m_guard_hp;
    public UILabel m_guard_attack;
    public UILabel m_guard_mf;
    public UILabel m_guard_wf;
    private ulong m_pet_guard_id = 0;
    private ulong change_pet_id = 0;
    public GameObject m_fight;
    public GameObject m_pet_weiyang_effect;
    public GameObject m_pet_jingjie_effect;
    public GameObject m_pet_shengxing_effect;
    public UILabel m_extra;

    List<dhc.equip_t> m_t_equips = new List<dhc.equip_t>();
    List<dhc.treasure_t> m_t_treasures = new List<dhc.treasure_t>();
    private string error = "";
    private ulong change_id = 0;
    private ulong card_id = 0;

    bool tx_effect = false;
    bool m_need_update = false;
    public bool m_start = false;
    bool is_pet = false;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void set_select_id()
    {
        if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[m_select_card_id] == 0)
            {
                for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i++)
                {
                    if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
                    {
                        m_select_card_id = i;
                        break;
                    }
                }
            }
        }
    }

    public void OnEnable()
    {

        set_select_id();
        if (m_xiangxi_panel.activeSelf)
        {
            m_peiyang_panel.SetActive(false);
            m_down.SetActive(true);
            m_xiangxi_panel.SetActive(true);
        }
        else if (m_peiyang_panel.activeSelf)
        {
            m_peiyang_panel.SetActive(true);
            m_down.SetActive(false);
            m_xiangxi_panel.SetActive(false);
        }
        m_show_Label.SetActive(false);
        update_ui();
        if (m_start)
        {
            select_info(m_select_card_id, false);
        }
        else
        {
            select_info(m_select_card_id, true);
        }
        for (int i = 0; i < tx.Count; ++i)
        {
            tx[i].SetActive(false);
        }
        m_start = false;
    }

    void show_upgrade()
    {
        root_gui._instance.m_default_active = "";
        m_down.SetActive(false);
        m_peiyang_panel.SetActive(true);
        m_xiangxi_panel.SetActive(false);
        m_peiyang_button.GetComponent<UIToggle>().value = true;

        root_gui._instance.m_default_active = "";
        s_message _message = new s_message();
        _message.m_type = "show_cn_gui";
        if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            _message.m_long.Add(sys._instance.m_self.m_t_player.zhenxing[m_select_card_id]);
        }
        else
        {
            _message.m_long.Add(my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid());
        }
        cmessage_center._instance.add_message(_message);
    }

    public void show_peiyang()
    {
        m_peiyang_panel.SetActive(true);
        m_xiangxi_panel.SetActive(false);
        m_peiyang_button.GetComponent<UIToggle>().value = true;
    }

    public void click(GameObject obj)
    {
        if (obj.transform.name == "pei_yang")
        {
            m_down.SetActive(false);
            m_peiyang_panel.SetActive(true);
            m_xiangxi_panel.SetActive(false);
            m_peiyang_button.GetComponent<UIToggle>().value = true;
        }
        else if (obj.transform.name == "xiang_xi")
        {
            m_peiyang_panel.SetActive(false);
            m_down.SetActive(true);
            m_xiangxi_panel.SetActive(true);
            m_xiangxi_button.GetComponent<UIToggle>().value = true;
        }
        else if (obj.transform.name == "sm")
        {
            s_message _msg = new s_message();
            _msg.m_type = "show_bz_help_gui";
            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.transform.name == "close")
        {
            if (m_shenping_panel.activeSelf)
            {
                m_bu_zheng_panel.SetActive(true);
                m_shenping_panel.SetActive(false);
                m_top_res.GetComponent<top_res>().types[3] = 7;
                update_ui();
                select_info(m_select_card_id);
            }
            else if (m_guanghuan_gui.activeSelf)
            {
                m_bu_zheng_panel.SetActive(true);
                m_guanghuan_gui.SetActive(false);
                update_ui();
                select_info(m_select_card_id);
            }
            else
            {
                this.GetComponent<ui_show_anim>().hide_ui();
                m_card = null;
                m_start = false;
                s_message _message = new s_message();
                _message.m_type = "hide_show_unit";
                cmessage_center._instance.add_message(_message);

                _message = new s_message();
                _message.m_type = "show_main_gui";
                cmessage_center._instance.add_message(_message);

                _message = new s_message();
                _message.m_type = "hide_unit_show";
                cmessage_center._instance.add_message(_message);
                this.GetComponent<gui_remove>().m_remove = true;
            }
        }
        else if (obj.transform.name == "xiangqing")
        {
            s_message _msg = new s_message();

            _msg.m_type = "py_show_zl";
            _msg.m_long.Add(m_card.get_guid());

            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.transform.name == "shenping")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shengpin)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("bu_zheng_panel.cs_269_73"), (int)e_open_level.el_shengpin));//觉醒{0}级开启
                return;
            }
            if (m_card.m_t_class.color < 4)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_274_59"));//该伙伴无法觉醒
                return;
            }
            m_bu_zheng_panel.SetActive(false);
            m_shenping_panel.GetComponent<shengping_gui>().m_card = m_card;
            m_shenping_panel.SetActive(true);
            m_top_res.GetComponent<top_res>().types[3] = 5;
        }
        else if (obj.transform.name == "tihuan")
        {
            m_preselect_card_id = m_select_card_id;
            List<ulong> self = new List<ulong>();
            for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
                {
                    self.Add(sys._instance.m_self.m_t_player.zhenxing[i]);
                }
            }
            for (int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.houyuan[i] != 0)
                {
                    self.Add(sys._instance.m_self.m_t_player.houyuan[i]);
                }
            }
            root_gui._instance.show_common_card_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_300_46"), true, 1, self, "common_select_card", false, this.gameObject);//请选择需要上阵的伙伴
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        else if (obj.transform.name == "shangzhen")
        {
            for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.zhenxing[i] == 0 && is_open(i))
                {
                    m_preselect_card_id = i;
                    break;
                }
            }
            s_message _msg = new s_message();

            _msg.m_type = "common_select_card";
            _msg.m_long.Add(m_card.get_guid());

            cmessage_center._instance.add_message(_msg);
        }
        else if (obj.transform.name == "tu_po")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_tupo)
            {
                string str = game_data._instance.get_t_language("bu_zheng_panel.cs_325_17");//突破{0}级开启
                str = string.Format(str, ((int)e_open_level.el_tupo).ToString());
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
                return;
            }
            s_message _message = new s_message();
            _message.m_type = "show_tu_po_gui";
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                _message.m_long.Add(sys._instance.m_self.m_t_player.zhenxing[m_select_card_id]);
            }
            else
            {
                _message.m_long.Add(my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid());
            }
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.transform.name == "jinjie")
        {
            s_message _message = new s_message();
            _message.m_type = "show_jj_gui";
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                _message.m_long.Add(sys._instance.m_self.m_t_player.zhenxing[m_select_card_id]);
            }
            else
            {
                _message.m_long.Add(my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid());
            }
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.transform.name == "chong_neng")
        {
            s_message _message = new s_message();
            _message.m_type = "show_cn_gui";
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                _message.m_long.Add(sys._instance.m_self.m_t_player.zhenxing[m_select_card_id]);
            }
            else
            {
                _message.m_long.Add(my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid());
            }
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.transform.name == "ji_neng")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_skill)
            {
                string str = game_data._instance.get_t_language("bu_zheng_panel.cs_375_17");//技能{0}级开启
                str = string.Format(str, ((int)e_open_level.el_skill).ToString());
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
                return;
            }
            s_message _message = new s_message();
            _message.m_type = "show_jn_gui";
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                _message.m_long.Add(sys._instance.m_self.m_t_player.zhenxing[m_select_card_id]);
            }
            else
            {
                _message.m_long.Add(my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid());
            }
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.transform.name == "chong_sheng")
        {
            s_message _message = new s_message();
            _message.m_type = "show_cs_gui";
            _message.m_long.Add(sys._instance.m_self.m_t_player.zhenxing[m_select_card_id]);
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.transform.name == "look")
        {
            this.transform.GetComponent<ui_show_anim>().hide_ui();

            ulong _gui = (ulong)sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];

            s_message _message = new s_message();
            _message.m_type = "show_unit";
            _message.m_long.Add((ulong)_gui);
            _message.m_string.Add("show_buzheng2");
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.transform.name == "shi_zhuang")
        {
            if (error != "")
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + error);
                return;
            }
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
            ulong _gui = 0;
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                _gui = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
            }
            else
            {
                _gui = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
            }
            s_message _message = new s_message();
            _message.m_type = "show_hb_dress_gui";
            _message.m_long.Add((ulong)_gui);
            _message.m_string.Add("show_bu_zheng");
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.transform.name == "yremove")
        {
            List<int> index = new List<int>();
            List<ulong> yequip_guid = new List<ulong>();
            ulong guid = 0;
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
            }
            else
            {
                guid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
            }
            ccard card = sys._instance.m_self.get_card_guid(guid);
            for (int i = 0; i < 4; i++)
            {
                dhc.equip_t equip = card.m_equip[i];
                if (equip != null)
                {
                    index.Add(i);
                    yequip_guid.Add(equip.guid);
                    equip.role_guid = 0;
                    card.get_role().zhuangbeis[i] = 0;
                }
            }
            card.update_role_attr();
            select_info(m_select_card_id);
            show_equip(m_select_card_id);
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
            protocol.game.cmsg_role_equip _msg = new protocol.game.cmsg_role_equip();
            _msg.role_guid = card.get_guid();
            for (int i = 0; i < index.Count; ++i)
            {
                _msg.index.Add(index[i]);
                _msg.equip_guid.Add(0);
            }

            net_http._instance.send_msg<protocol.game.cmsg_role_equip>(opclient_t.CMSG_ROLE_EQUIP, _msg);
        }
        else if (obj.transform.name == "change_dress")
        {
            ulong guid = 0;
            int count = 0;
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
            }
            else
            {
                guid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
            }
            ccard card = sys._instance.m_self.get_card_guid(guid);
            if (!sys._instance.m_self.is_zheng(card.get_guid()))
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_491_59"));//伙伴未上阵
                return;
            }
            List<dhc.equip_t> m_equips = new List<dhc.equip_t>();
            List<dhc.treasure_t> m_treasures = new List<dhc.treasure_t>();
            List<int> index = new List<int>();
            List<int> treasure_index = new List<int>();
            List<ulong> equip_guid = new List<ulong>();
            List<ulong> treasure_guid = new List<ulong>();
            for (int i = 1; i <= 2; ++i)
            {
                if (is_treasure_open(i))
                {
                    count++;
                }
            }
            for (int m_select_equip_id = 0; m_select_equip_id < 4; m_select_equip_id++)
            {
                m_equips.Clear();
                for (int i = 0; i < sys._instance.m_self.get_equip_num(); i++)
                {
                    dhc.equip_t _equip = sys._instance.m_self.get_equip_index(i);
                    s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
                    if (t_equip.type != m_select_equip_id + 1 || _equip.role_guid > 0)
                    {
                        continue;
                    }
                    m_equips.Add(_equip);
                }
                bool flag = false;
                dhc.equip_t temp = card.m_equip[m_select_equip_id];
                if (m_equips.Count != 0)
                {
                    if (temp == null)
                    {
                        flag = true;
                        temp = m_equips[0];
                    }
                    else
                    {
                        if (get_better(temp, m_equips[0]))
                        {
                            flag = true;
                            temp = m_equips[0];
                        }
                    }
                    for (int j = 1; j < m_equips.Count; j++)
                    {
                        if (get_better(temp, m_equips[j]))
                        {
                            flag = true;
                            temp = m_equips[j];
                        }
                    }
                }
                if (temp != null && flag)
                {
                    index.Add(m_select_equip_id);
                    equip_guid.Add(temp.guid);

                    dhc.equip_t temp1 = card.m_equip[m_select_equip_id];
                    if (temp1 != null)
                    {
                        temp1.role_guid = 0;
                    }
                    temp.role_guid = card.get_guid();
                    card.get_role().zhuangbeis[m_select_equip_id] = temp.guid;
                    card.update_role_attr();
                }
            }
            for (int m_select_treasure_id = 0; m_select_treasure_id < count; m_select_treasure_id++)
            {
                m_treasures.Clear();
                for (int i = 0; i < sys._instance.m_self.get_treasure_num(); i++)
                {
                    dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(i);
                    s_t_baowu t_treasure = game_data._instance.get_t_baowu(_treasure.template_id);
                    if (t_treasure.type != m_select_treasure_id + 1 || _treasure.role_guid > 0)
                    {
                        continue;
                    }
                    m_treasures.Add(_treasure);
                }
                bool flag = false;
                dhc.treasure_t temp = card.m_treasure[m_select_treasure_id];
                if (m_treasures.Count != 0)
                {
                    if (temp == null)
                    {
                        flag = true;
                        temp = m_treasures[0];
                    }
                    else
                    {
                        if (get_better_treasure(temp, m_treasures[0]))
                        {
                            flag = true;
                            temp = m_treasures[0];
                        }
                    }
                    for (int j = 1; j < m_treasures.Count; j++)
                    {
                        if (get_better_treasure(temp, m_treasures[j]))
                        {
                            flag = true;
                            temp = m_treasures[j];
                        }
                    }
                }
                if (temp != null && flag)
                {
                    treasure_index.Add(m_select_treasure_id);
                    treasure_guid.Add(temp.guid);

                    dhc.treasure_t temp1 = card.m_treasure[m_select_treasure_id];
                    if (temp1 != null)
                    {
                        temp1.role_guid = 0;
                    }
                    temp.role_guid = card.get_guid();
                    card.get_role().treasures[m_select_treasure_id] = temp.guid;
                    card.update_role_attr();
                }
            }
            if (index.Count > 0 || treasure_index.Count > 0)
            {
                card.update_role_attr_ex();
                sys._instance.m_card = card;
                select_info(m_select_card_id);
                show_equip(m_select_card_id);
                s_message _message2 = new s_message();
                _message2.m_type = "check_bf";
                cmessage_center._instance.add_message(_message2);
                protocol.game.cmsg_role_all_equip _msg = new protocol.game.cmsg_role_all_equip();
                _msg.role_guid = card.get_guid();
                for (int i = 0; i < index.Count; ++i)
                {
                    _msg.equip_index.Add(index[i]);
                    _msg.equip_guid.Add(equip_guid[i]);
                }
                for (int i = 0; i < treasure_index.Count; ++i)
                {
                    _msg.treasure_index.Add(treasure_index[i]);
                    _msg.treasure_guid.Add(treasure_guid[i]);
                }

                net_http._instance.send_msg<protocol.game.cmsg_role_all_equip>(opclient_t.CMSG_ROLE_ALL_EQUIP, _msg);
            }
        }
        else if (obj.transform.name == "yenhance")
        {
            if (!sys._instance.m_self.is_zheng(m_card.get_guid()))
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_491_59"));//伙伴未上阵
                return;
            }
            bool flag = false;
            for (int i = 0; i < 4; ++i)
            {
                dhc.equip_t t_equip = m_card.m_equip[i];
                if (t_equip != null && equip.is_enhance(t_equip))
                {
                    flag = true;
                    break;
                }
            }
            if (!flag)
            {
                bool flat = false;
                for (int j = 0; j < 4; ++j)
                {
                    dhc.equip_t t_equip = m_card.m_equip[j];
                    if (t_equip != null && equip.gold_enough(t_equip))
                    {
                        flat = true;
                        break;
                    }
                }
                if (flat)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_671_60"));//金币不足
                    return;
                }
                return;
            }
            y_enhace();
            m_card.update_role_attr();
            sys._instance.m_card = m_card;
            select_info(m_select_card_id);
            show_equip(m_select_card_id);

            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);

        }
        else if (obj.transform.name == "gongzheng")
        {
            ulong _guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
            ccard m_card = sys._instance.m_self.get_card_guid(_guid);
            bool flag = false;
            for (int i = 0; i < 4; ++i)
            {
                dhc.equip_t t_equip = m_card.m_equip[i];
                if (t_equip == null)
                {
                    flag = true;
                    break;
                }
            }
            if (flag)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_703_59")); //主人,穿齐4件装备才能开启属性共振
                return;
            }
            m_gongzheng_mubiao_gui.GetComponent<gongzheng_mubiao_gui>().m_card = m_card;
            m_gongzheng_mubiao_gui.SetActive(true);
        }
        else if (obj.transform.name == "houyuan")
        {
            m_houyuan_gui.GetComponent<houyuan_gui>().m_buzheng = this.gameObject;
            m_houyuan_gui.GetComponent<houyuan_gui>().m_select_card_id = 0;
            m_houyuan_gui.GetComponent<houyuan_gui>().card_id = 0;
            m_houyuan_gui.GetComponent<houyuan_gui>().m_select = 0;
            m_houyuan_gui.SetActive(true);
        }
        else if (obj.transform.name == "duixing")
        {
            s_message _message = new s_message();
            _message.m_type = "show_duixing_gui";
            _message.m_bools.Add(false);
            cmessage_center._instance.add_message(_message);
        }
        else if (obj.transform.name == "guanghuan")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_guanghuan)
            {
                string str = game_data._instance.get_t_language("bu_zheng_panel.cs_728_17");//光环{0}级开启
                str = string.Format(str, ((int)e_open_level.el_guanghuan).ToString());
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
                return;
            }
            m_bu_zheng_panel.SetActive(false);
            m_guanghuan_gui.GetComponent<guanghuan_gui>().m_card = m_card;
            m_guanghuan_gui.SetActive(true);
        }
        else if (obj.transform.name == "qh")
        {
            m_up_gui.GetComponent<guanghuan_up_gui>().t_guanghuan = game_data._instance.get_t_guanghuan(sys._instance.m_self.m_t_player.guanghuan_id);
            m_up_gui.GetComponent<guanghuan_up_gui>().reset_skill();
            m_up_gui.SetActive(true);
        }
        else if (obj.transform.name == "cs")
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_745_58"));//装备中的光环不可重生
            return;
        }
        else if (obj.transform.name == "tujian")
        {
            m_tj_gui.GetComponent<guanghuan_tj_gui>().reset_tj();
            m_tj_gui.SetActive(true);
        }
        else if (obj.transform.name == "huwei")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pet_guard)
            {
                string str = game_data._instance.get_t_language("bu_zheng_panel.cs_757_17");//护卫宠物{0}级开启
                str = string.Format(str, ((int)e_open_level.el_pet_guard).ToString());
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
                return;
            }
            if (!sys._instance.m_self.is_zheng(m_card.get_guid()))
            {
                string str = game_data._instance.get_t_language("bu_zheng_panel.cs_491_59");//伙伴未上阵
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
                return;
            }
            if (m_card.get_role().pet != 0)
            {
                pet_guard_detail();
                m_pet_guard_detail.SetActive(true);
            }
            else
            {
                pet_guard();
                m_pet_guard_gui.SetActive(true);
            }
        }
        else if (obj.transform.name == "guard_close")
        {
            m_pet_guard_detail.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (obj.transform.name == "guard_xiexia")
        {
            m_pet_guard_id = m_card.get_role().pet;
            protocol.game.cmsg_pet_guard _msg = new protocol.game.cmsg_pet_guard();
            _msg.pet_guid = m_pet_guard_id;
            _msg.role_guid = m_card.get_role().guid;
            net_http._instance.send_msg<protocol.game.cmsg_pet_guard>(opclient_t.CMSG_PET_GUARD, _msg);
        }
        else if (obj.transform.name == "guard_tihuan")
        {
            m_pet_guard_detail.transform.Find("frame_big").GetComponent<frame>().hide();
            pet_guard();
            m_pet_guard_gui.SetActive(true);
        }
        else if (obj.transform.name == "pet_tihuan")
        {
            m_preselect_card_id = 10000;
            List<ulong> self = new List<ulong>();
            for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
            {
                ccard _card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[i]);
                if (_card == null)
                {
                    continue;
                }
                if (_card.get_role().pet > 0)
                {
                    self.Add(_card.get_role().pet);
                }
            }
            self.Add(sys._instance.m_self.m_t_player.pet_on);
            root_gui._instance.show_common_pet_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_814_45"), true, 1, self, "common_select_pet", false, this.gameObject, 1);//请选择需要上阵的宠物
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        else if (obj.transform.name == "pet_pei_yang")
        {
            m_pet_down.SetActive(false);
            m_pet_peiyang_panel.SetActive(true);
            m_pet_xiangxi_panel.SetActive(false);
            m_pet_peiyang_button.GetComponent<UIToggle>().value = true;
        }
        else if (obj.transform.name == "pet_xiang_xi")
        {
            m_pet_down.SetActive(true);
            m_pet_peiyang_panel.SetActive(false);
            m_pet_xiangxi_panel.SetActive(true);
            m_pet_xiangxi_button.GetComponent<UIToggle>().value = true;
        }
        else if (obj.transform.name == "close_guard")
        {
            m_pet_guard_gui.transform.Find("frame_big").GetComponent<frame>().hide();
        }
    }

    void pet_guard_detail()
    {
        pet m_pet = sys._instance.m_self.get_pet_guid(m_card.get_role().pet);
        m_guard_name.text = m_pet.get_color_name();
        m_guard_hp.text = m_pet.get_guard_attr(1).ToString("f0");
        m_guard_attack.text = m_pet.get_guard_attr(2).ToString("f0");
        m_guard_wf.text = m_pet.get_guard_attr(3).ToString("f0");
        m_guard_mf.text = m_pet.get_guard_attr(4).ToString("f0");
        sys._instance.remove_child(m_guard_icon);
        GameObject _icon = icon_manager._instance.create_pet_icon(m_pet.get_guid());
        _icon.transform.parent = m_guard_icon.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
    }

    void pet_guard()
    {
        if (m_pet_guard_scro.GetComponent<SpringPanel>() != null)
        {
            m_pet_guard_scro.GetComponent<SpringPanel>().enabled = false;
        }
        m_pet_guard_scro.transform.localPosition = Vector3.zero;
        m_pet_guard_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_pet_guard_scro);
        List<pet> m_pets = new List<pet>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.pets.Count; ++i)
        {
            pet _pet = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pets[i]);
            if (_pet.get_pet().role_guid > 0 || sys._instance.m_self.m_t_player.pets[i] == sys._instance.m_self.m_t_player.pet_on)
            {
                continue;
            }
            pet m_pet = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pets[i]);
            m_pets.Add(m_pet);
        }
        if (m_pets.Count == 0)
        {
            m_none.SetActive(true);
        }
        else
        {
            m_none.SetActive(false);
        }
        m_pets.Sort(pet.guard_com);
        for (int i = 0; i < m_pets.Count; ++i)
        {
            GameObject _pet = game_data._instance.ins_object_res("ui/pet_guard_item");

            _pet.transform.parent = m_pet_guard_scro.transform;
            _pet.transform.localPosition = new Vector3(0, -135 * i + 122, 0);
            _pet.transform.localScale = new Vector3(1, 1, 1);
            _pet.GetComponent<pet_guard_item>().m_pet = m_pets[i];
            _pet.GetComponent<pet_guard_item>().update_ui();
            _pet.SetActive(true);
        }
    }

    bool can_enhance()
    {
        bool flag = false;
        for (int i = 0; i < 4; ++i)
        {
            dhc.equip_t t_equip = m_card.m_equip[i];
            if (t_equip != null && equip.is_enhance(t_equip))
            {
                flag = true;
                break;
            }
        }
        if (!flag)
        {
            bool flat = false;
            for (int j = 0; j < 4; ++j)
            {
                dhc.equip_t t_equip = m_card.m_equip[j];
                if (t_equip != null && equip.gold_enough(t_equip))
                {
                    flat = true;
                    break;
                }
            }
            if (flat)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    void y_enhace()
    {
        List<dhc.equip_t> equips = new List<dhc.equip_t>();
        Dictionary<ulong, int> _equips = new Dictionary<ulong, int>();
        for (int i = 0; i < 4; ++i)
        {
            dhc.equip_t equip = m_card.m_equip[i];
            if (equip != null)
            {
                equips.Add(equip);
            }
        }
        equips.Sort(comp);
        for (int i = 0; i < equips.Count; ++i)
        {
            _equips.Add(equips[i].guid, equips[i].enhance);
        }
        bool is_enhance = true;
        while (is_enhance)
        {
            bool has_enhance = false;
            int s = second_num(equips);
            for (int i = 0; i < equips.Count; ++i)
            {
                int enhance = s - equips[i].enhance - 1;
                if (enhance < 0)
                {
                    continue;
                }
                s_t_equip _equip = game_data._instance.get_t_equip(equips[i].template_id);
                int gold = game_data._instance.get_enhance(equips[i].enhance + 1, _equip.font_color);
                if (gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
                {
                    continue;
                }

                if (equips[i].enhance >= (sys._instance.m_self.m_t_player.level + 10) / 10 * 10)
                {
                    continue;
                }
                has_enhance = true;
                enhance--;
                sys._instance.m_self.sub_att(e_player_attr.player_gold, gold, game_data._instance.get_t_language("bu_zheng_panel.cs_975_65"));//装备强化消耗
                equips[i].enhance++;
            }
            equips.Sort(comp);
            if (!is_enchance(equips) || !has_enhance)
            {
                is_enhance = false;
            }
        }
        bool flat_ = false;
        is_enhance = true;
        for (int i = 0; i < 4; ++i)
        {
            dhc.equip_t t_equip = m_card.m_equip[i];
            if (t_equip != null && equip.is_enhance(t_equip))
            {
                flat_ = true;
                break;
            }
        }
        if (!flat_)
        {
            is_enhance = false;
        }
        while (is_enhance)
        {
            bool has_enhance = false;
            for (int i = 0; i < equips.Count; ++i)
            {
                s_t_equip _equip = game_data._instance.get_t_equip(equips[i].template_id);
                int gold = game_data._instance.get_enhance(equips[i].enhance + 1, _equip.font_color);
                if (gold > sys._instance.m_self.get_att(e_player_attr.player_gold))
                {
                    continue;
                }
                int max_enchance = Mathf.Min(int.Parse(game_data._instance.m_dbc_enhance.get_index(0, game_data._instance.m_dbc_enhance.get_y() - 1)), sys._instance.m_self.m_t_player.level);
                if (equips[i].enhance >= max_enchance)
                {
                    continue;
                }
                has_enhance = true;
                sys._instance.m_self.sub_att(e_player_attr.player_gold, gold, game_data._instance.get_t_language("bu_zheng_panel.cs_975_65"));//装备强化消耗
                equips[i].enhance++;
            }
            bool flat = false;
            for (int j = 0; j < 4; ++j)
            {
                dhc.equip_t t_equip = m_card.m_equip[j];
                if (t_equip != null && equip.is_enhance(t_equip))
                {
                    flat = true;
                    break;
                }
            }
            if (!flat || !has_enhance)
            {
                is_enhance = false;
            }
        }
        protocol.game.cmsg_equip_auto_enhance _msg = new protocol.game.cmsg_equip_auto_enhance();
        for (int i = 0; i < equips.Count; ++i)
        {
            _msg.equip_guid.Add(equips[i].guid);
            foreach (ulong id in _equips.Keys)
            {
                if (id == equips[i].guid)
                {
                    int num = equips[i].enhance - _equips[equips[i].guid];
                    _msg.enhance_num.Add(num);
                    break;
                }
            }

        }
        net_http._instance.send_msg<protocol.game.cmsg_equip_auto_enhance>(opclient_t.CMSG_EQUIP_AUTO_ENHANCE, _msg);
        for (int i = 0; i < equips.Count; ++i)
        {
            foreach (ulong id in _equips.Keys)
            {
                if (id == equips[i].guid)
                {
                    int num = equips[i].enhance - _equips[equips[i].guid];
                    s_t_equip t_equip = game_data._instance.get_t_equip(equips[i].template_id);
                    if (num > 0)
                    {
                        tx[t_equip.type - 1].GetComponent<UISpriteAnimation>().ResetToBeginning();
                        tx[t_equip.type - 1].SetActive(true);
                        tx_effect = true;
                    }
                    sys._instance.m_self.m_t_player.qh_task_num += num;
                    sys._instance.m_self.add_active(600, num);
                    sys._instance.m_self.check_target_done();
                    break;
                }
            }
        }
    }

    bool is_enchance(List<dhc.equip_t> equips)
    {
        int num = 0;
        for (int i = 0; i < equips.Count; ++i)
        {
            if (!equip.is_enhance(equips[i]))
            {
                num++;
            }
        }
        if (num == equips.Count)
        {
            return false;
        }
        num = 0;
        for (int i = 0; i < equips.Count; ++i)
        {
            if (equips[0].enhance == equips[i].enhance)
            {
                num++;
            }
        }
        if (num == equips.Count)
        {
            return false;
        }
        return true;
    }

    int second_num(List<dhc.equip_t> equips)
    {
        List<dhc.equip_t> t_equips = new List<dhc.equip_t>();
        for (int i = 0; i < equips.Count; ++i)
        {
            t_equips.Add(equips[i]);
        }
        t_equips.Sort(comp);
        int m_min = 0;
        int s_min = 1;
        if (t_equips.Count <= 2)
        {
            return t_equips[t_equips.Count - 1].enhance;
        }
        if (t_equips[1].enhance < t_equips[0].enhance)
        {
            m_min = 1;
            s_min = 0;
        }
        for (int i = 2; i < t_equips.Count; i++)
        {
            if (t_equips[i].enhance < t_equips[s_min].enhance)
            {
                if (t_equips[i].enhance < t_equips[m_min].enhance)
                {
                    s_min = m_min;
                    m_min = i;
                }
                else
                {
                    s_min = i;
                }
            }
            if (t_equips[i].enhance > t_equips[m_min].enhance)
            {
                if (t_equips[m_min].enhance == t_equips[s_min].enhance)
                {
                    m_min = s_min;
                    s_min = i;
                }
            }
        }
        int num = 0;
        for (int i = 0; i < t_equips.Count; i++)
        {
            num++;
            if (t_equips[i].enhance != t_equips[0].enhance)
            {
                break;
            }
        }
        if (num == t_equips.Count)
        {
            return t_equips[t_equips.Count - 1].enhance;
        }
        return t_equips[s_min].enhance;
    }

    public int comp(dhc.equip_t x, dhc.equip_t y)
    {
        s_t_equip t_equip_x = game_data._instance.get_t_equip(x.template_id);
        s_t_equip t_equip_y = game_data._instance.get_t_equip(y.template_id);
        if (x.enhance < y.enhance)
        {
            return -1;
        }
        else if (x.enhance > y.enhance)
        {
            return 1;
        }
        else if (t_equip_x.type < t_equip_y.type)
        {
            return -1;
        }
        else if (t_equip_x.type > t_equip_y.type)
        {
            return 1;
        }

        return 0;
    }

    bool get_better(dhc.equip_t x, dhc.equip_t y)
    {
        if (game_data._instance.get_t_equip(x.template_id).font_color < game_data._instance.get_t_equip(y.template_id).font_color)
        {
            return true;
        }
        else if (game_data._instance.get_t_equip(x.template_id).font_color == game_data._instance.get_t_equip(y.template_id).font_color)
        {
            if (x.enhance < y.enhance)
            {
                return true;
            }
        }
        return false;
    }

    bool get_better_treasure(dhc.treasure_t x, dhc.treasure_t y)
    {
        if (game_data._instance.get_t_baowu(x.template_id).font_color < game_data._instance.get_t_baowu(y.template_id).font_color)
        {
            return true;
        }
        else if (game_data._instance.get_t_baowu(x.template_id).font_color == game_data._instance.get_t_baowu(y.template_id).font_color)
        {
            if (x.jilian < y.jilian)
            {
                return true;
            }
            else if (x.jilian == y.jilian)
            {
                if (x.enhance < y.enhance)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void role_effect()
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            effect1[i].SetActive(false);
        }
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[i] == 0)
            {
                effect1[i].SetActive(false);
            }
            else
            {
                if (can_effect(i))
                {
                    effect1[i].SetActive(true);
                }
                else
                {
                    effect1[i].SetActive(false);
                }
            }
        }
    }

    public static bool bu_zheng_effect()
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[i] == 0)
            {
                continue;
            }
            else
            {
                if (can_effect(i))
                {
                    return true;
                }
            }
        }
        pet m_pet = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pet_on);
        if (m_pet != null)
        {
            if (pet_weiyang_gui.is_weiyang(m_pet))
            {
                return true;
            }
            if (pet_jingjie_gui.is_jingjie(m_pet))
            {
                return true;
            }
            if (pet_shengxing_gui.is_shengxing(m_pet))
            {
                return true;
            }
        }
        return false;
    }

    public static bool can_effect(int id)
    {
        ulong _guid = sys._instance.m_self.m_t_player.zhenxing[id];
        ccard card = sys._instance.m_self.get_card_guid(_guid);
        if (jinjie_gui.is_jinjie(card))
        {
            return true;
        }
        if (tu_po_gui.is_tupo(card))
        {
            return true;
        }
        if (chong_neng_gui.is_chongneng(card))
        {
            return true;
        }
        if (skill_gui.is_skill(card) || skill_gui.is_zs_skill(card))
        {
            return true;
        }
        if (shengping_gui.is_shengping(card))
        {
            return true;
        }
        for (int i = 0; i < card.m_equip.Count; ++i)
        {
            dhc.equip_t temp = card.m_equip[i];
            bool flag = false;
            for (int j = 0; j < sys._instance.m_self.get_equip_num(); ++j)
            {
                dhc.equip_t _equip = sys._instance.m_self.get_equip_index(j);
                if (_equip.role_guid == 0)
                {
                    s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
                    if (t_equip.type == i + 1)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (temp == null)
            {
                if (flag)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (equip.is_enhance(temp))
                {
                    return true;
                }
            }
        }
        for (int i = 0; i < card.m_treasure.Count && i < 2; ++i)
        {
            dhc.treasure_t temp = card.m_treasure[i];
            bool flag = false;
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasue_qh)
            {
                return false;
            }
            for (int j = 0; j < sys._instance.m_self.get_treasure_num(); ++j)
            {
                dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(j);
                if (_treasure.role_guid == 0)
                {
                    s_t_baowu t_treasure = game_data._instance.get_t_baowu(_treasure.template_id);
                    if (t_treasure.type == i + 1)
                    {
                        flag = true;
                        break;
                    }
                }
            }
            if (temp == null)
            {
                if (flag)
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                if (treasure.is_enhance(temp))
                {
                    return true;
                }
            }
        }
        return false;
    }

    void is_effect(int id)
    {
        bool flag = false;
        ulong _guid = 0;
        if (id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            _guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
        }
        else
        {
            _guid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
        }
        ccard card = sys._instance.m_self.get_card_guid(_guid);

        if (chong_neng_gui.is_chongneng(card))
        {
            flag = true;
            m_chongneng.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_chongneng.transform.Find("effect").gameObject.SetActive(false);
        }

        if (jinjie_gui.is_jinjie(card))
        {
            flag = true;
            m_jingjie.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_jingjie.transform.Find("effect").gameObject.SetActive(false);
        }

        if (tu_po_gui.is_tupo(card))
        {
            flag = true;
            m_tupo.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_tupo.transform.Find("effect").gameObject.SetActive(false);
        }
        if (skill_gui.is_skill(card) || skill_gui.is_zs_skill(card))
        {
            flag = true;
            m_skill_gui.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_skill_gui.transform.Find("effect").gameObject.SetActive(false);
        }
        if (shengping_gui.is_shengping(card))
        {
            flag = true;
            m_shengpin.transform.Find("effect").gameObject.SetActive(true);
        }
        else
        {
            m_shengpin.transform.Find("effect").gameObject.SetActive(false);
        }

        if (flag)
        {
            m_peiyang_button.transform.Find("effect1").gameObject.SetActive(true);
        }
        else
        {
            m_peiyang_button.transform.Find("effect1").gameObject.SetActive(false);
        }
        flag = false;
        for (int i = 0; i < card.m_equip.Count; ++i)
        {
            dhc.equip_t temp = card.m_equip[i];
            if (temp == null)
            {
                effect[i].SetActive(false);
            }
            else
            {
                if (equip.is_enhance(temp))
                {
                    effect[i].SetActive(true);
                }
                else
                {
                    effect[i].SetActive(false);
                }
            }
        }
        for (int i = 0; i < m_card.m_treasure.Count && i < 2; ++i)
        {
            if (!is_treasure_open(i + 1))
            {
                effect2[i].SetActive(false);
                continue;
            }
            dhc.treasure_t temp = card.m_treasure[i];
            if (temp == null)
            {
                effect2[i].SetActive(false);
            }
            else
            {
                if (treasure.is_enhance(temp))
                {
                    effect2[i].SetActive(true);
                }
                else
                {
                    effect2[i].SetActive(false);
                }
            }
        }
    }

    public void pet_effect()
    {
        m_pet_weiyang_effect.SetActive(false);
        m_pet_jingjie_effect.SetActive(false);
        m_pet_shengxing_effect.SetActive(false);
        pet m_pet = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pet_on);
        if (m_pet == null || sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pet)
        {
            m_pet_effect.SetActive(false);
        }
        else
        {
            bool pet_flag = false;
            if (pet_weiyang_gui.is_weiyang(m_pet))
            {
                m_pet_weiyang_effect.SetActive(true);
                pet_flag = true;
            }
            if (pet_jingjie_gui.is_jingjie(m_pet))
            {
                m_pet_jingjie_effect.SetActive(true);
                pet_flag = true;
            }
            if (pet_shengxing_gui.is_shengxing(m_pet))
            {
                m_pet_shengxing_effect.SetActive(true);
                pet_flag = true;
            }
            m_pet_effect.SetActive(pet_flag);
            m_pet_peiyang_button.transform.Find("effect1").gameObject.SetActive(pet_flag);
        }
    }

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_ZHENXING)
        {
            List<ulong> _zhengxings = new List<ulong>();
            List<int> _duixings = new List<int>();

            for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i++)
            {
                _zhengxings.Add(sys._instance.m_self.m_t_player.zhenxing[i]);
            }
            _zhengxings[m_preselect_card_id] = change_id;
            ccard m_card = null;
            card_id = change_id;
            if (change_id == 0)
            {
                m_card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[m_preselect_card_id]);
            }
            else
            {
                m_card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[m_preselect_card_id]);
                ccard _card = sys._instance.m_self.get_card_guid(change_id);
                if (m_card != null)
                {
                    for (int i = 0; i < m_card.m_treasure.Count; ++i)
                    {
                        if (m_card.m_treasure[i] != null)
                        {
                            m_card.m_treasure[i].role_guid = change_id;
                            _card.get_role().treasures[i] = m_card.get_role().treasures[i];
                            m_card.get_role().treasures[i] = 0;
                        }
                    }
                    for (int i = 0; i < m_card.m_equip.Count; ++i)
                    {
                        if (m_card.m_equip[i] != null)
                        {
                            m_card.m_equip[i].role_guid = change_id;
                            _card.get_role().zhuangbeis[i] = m_card.get_role().zhuangbeis[i];
                            m_card.get_role().zhuangbeis[i] = 0;
                        }
                    }
                    if (m_card.get_role().pet > 0)
                    {
                        pet m_pet = sys._instance.m_self.get_pet_guid(m_card.get_role().pet);
                        m_pet.get_pet().role_guid = change_id;
                        _card.get_role().pet = m_card.get_role().pet;
                        m_card.get_role().pet = 0;
                    }
                }
                for (int i = 0; i < _zhengxings.Count; i++)
                {
                    sys._instance.m_self.m_t_player.zhenxing[i] = _zhengxings[i];
                }
                for (int i = 0; i < _duixings.Count; i++)
                {
                    sys._instance.m_self.m_t_player.duixing[i] = _duixings[i];
                }
                m_select_card_id = m_preselect_card_id;
            }

            if (m_view.GetComponent<SpringPanel>() != null)
            {
                m_view.GetComponent<SpringPanel>().enabled = false;
            }
            ccard card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[m_preselect_card_id]);
            if (m_card != null)
            {
                m_card.update_role_attr_ex();
            }
            else
            {
                sys._instance.m_gongzhengs.Clear();
                for (int i = 0; i < 5; ++i)
                {
                    sys._instance.m_gongzhengs.Add(0);
                }
                m_old_equips.Clear();
                m_old_treasures.Clear();
                sys._instance.m_sxs.Clear();
                sys._instance.m_sxs = sys._instance.m_self.get_gongzhen(m_card);
            }
            sys._instance.m_self.get_fighting();
            m_view.transform.localPosition = new Vector3(0, 0, 0);
            m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
            m_select.transform.localPosition = m_cards[m_select_card_id].transform.localPosition;
            if (change_id != 0)
            {
                card.update_role_attr_ex();
                sys._instance.m_card = card;
            }
            else
            {
                sys._instance.m_card = null;
                m_card.update_role_attr_ex();
                string s = "";
                m_dacheng_des.SetActive(false);
                hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();
                if (_hide_Label != null)
                {
                    Destroy(_hide_Label);
                }
                sys._instance.m_gongzhengs.Clear();
                for (int i = 0; i < 5; ++i)
                {
                    sys._instance.m_gongzhengs.Add(0);
                }
                m_old_equips.Clear();
                m_old_treasures.Clear();
                sys._instance.m_sxs.Clear();
                sys._instance.m_sxs = sys._instance.m_self.get_gongzhen(null);
                s += sys._instance.m_self.check_gongzhen(m_card, sys._instance.m_gongzhengs, m_old_equips, m_old_treasures, sys._instance.m_sxs, m_card.get_guid());
                s = s.Replace(game_data._instance.get_t_language("player.cs_3500_31"), game_data._instance.get_t_language("bu_zheng_panel.cs_1672_36"));//达成//失效
                s = s.Replace("[ff0000]", "@color");
                s = s.Replace("[00ff00]", "[ff0000]");
                s = s.Replace("@color", "[00ff00]");
                System.Text.StringBuilder temp = new System.Text.StringBuilder();
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == '+')
                    {
                        temp.Append('-');

                    }
                    else if (s[i] == '-' && i > 0 && s[i - 1] != '[')
                    {
                        temp.Append('+');
                    }
                    else
                    {
                        temp.Append(s[i]);
                    }
                }
                m_dacheng_des.GetComponent<UILabel>().text = "";
                m_dacheng_des.GetComponent<UILabel>().text = temp.ToString();
                dacheng();

                for (int i = 0; i < m_card.m_treasure.Count; ++i)
                {
                    if (m_card.m_treasure[i] != null)
                    {
                        m_card.m_treasure[i].role_guid = 0;
                        m_card.get_role().treasures[i] = 0;
                    }
                }
                for (int i = 0; i < m_card.m_equip.Count; ++i)
                {
                    if (m_card.m_equip[i] != null)
                    {
                        m_card.m_equip[i].role_guid = 0;
                        m_card.get_role().zhuangbeis[i] = 0;
                    }
                }
                for (int i = 0; i < _zhengxings.Count; i++)
                {
                    sys._instance.m_self.m_t_player.zhenxing[i] = _zhengxings[i];
                }
                for (int i = 0; i < _duixings.Count; i++)
                {
                    sys._instance.m_self.m_t_player.duixing[i] = _duixings[i];
                }
                m_card.get_role().pet = 0;
                for (int i = 0; i < _zhengxings.Count; i++)
                {
                    if (_zhengxings[i] != 0)
                    {
                        m_select_card_id = i;
                        break;
                    }
                }
                sys._instance.m_card = null;
                card_id = 0;

            }

            m_need_update = true;
            select_info(m_select_card_id);
            show_equip(m_select_card_id);
            show_treasure(m_select_card_id);
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
        }
        if (message.m_opcode == opclient_t.CMSG_PET_GUARD)
        {
            protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success>(message.m_byte);
            if (_msg.success)
            {
                pet m_pet = sys._instance.m_self.get_pet_guid(m_pet_guard_id);
                if (m_card.get_role().pet == m_pet_guard_id)
                {
                    m_card.get_role().pet = 0;
                    m_pet.get_pet().role_guid = 0;
                }
                else
                {
                    pet _pet = sys._instance.m_self.get_pet_guid(m_card.get_role().pet);
                    if (_pet != null)
                    {
                        _pet.get_pet().role_guid = 0;
                    }
                    m_card.get_role().pet = m_pet_guard_id;
                    m_pet.get_pet().role_guid = m_card.get_guid();
                }
                s_message _message = new s_message();
                _message.m_type = "show_pet";
                _message.time = 0.1f;
                _message.m_object.Add(sys._instance.m_self.get_pet_guid(m_card.get_role().pet));
                _message.m_ints.Add((int)1);
                _message.m_string.Add("select_show");
                cmessage_center._instance.add_message(_message);
                select_info(m_select_card_id);
                if (m_pet_guard_gui.activeSelf)
                {
                    m_pet_guard_gui.transform.Find("frame_big").GetComponent<frame>().hide();
                }
                if (m_pet_guard_detail.activeSelf)
                {
                    m_pet_guard_detail.transform.Find("frame_big").GetComponent<frame>().hide();
                }
                s_message _message2 = new s_message();
                _message2.m_type = "check_bf";
                cmessage_center._instance.add_message(_message2);
            }
        }
        if (message.m_opcode == opclient_t.CMSG_PET_ON)
        {
            protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success>(message.m_byte);
            if (_msg.success)
            {
                is_pet = true;
                m_select_card_id = 10000;
                sys._instance.m_self.m_t_player.pet_on = change_pet_id;
                set_pet();
                select_info(m_select_card_id);
            }
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "common_select_card")
        {
            change_id = (ulong)message.m_long[0];
            List<ulong> _zhengxings = new List<ulong>();
            List<int> _duixings = new List<int>();

            for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i++)
            {
                _zhengxings.Add(sys._instance.m_self.m_t_player.zhenxing[i]);
            }
            _zhengxings[m_preselect_card_id] = change_id;
            int _zheng = 0;
            for (int i = 0; i < _zhengxings.Count; i++)
            {
                if (_zhengxings[i] != 0)
                {
                    _zheng++;
                }
            }
            if (_zheng == 0)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_1765_59"));//至少一个伙伴在阵前
                return;
            }

            protocol.game.cmsg_zhenxing _msg = new protocol.game.cmsg_zhenxing();

            if (change_id == 0)
            {
                _msg.index = -1;
                _msg.role_guid = sys._instance.m_self.m_t_player.zhenxing[m_preselect_card_id];
            }
            else
            {
                _msg.index = m_preselect_card_id;
                _msg.role_guid = change_id;
            }

            net_http._instance.send_msg<protocol.game.cmsg_zhenxing>(opclient_t.CMSG_ZHENXING, _msg);
        }

        if (message.m_type == "common_select_equip")
        {
            ulong equip_guid = (ulong)message.m_long[0];
            dhc.equip_t temp = sys._instance.m_self.get_equip_guid(equip_guid);
            ulong guid = 0;
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
            }
            else
            {
                guid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
            }
            ccard card = sys._instance.m_self.get_card_guid(guid);
            dhc.equip_t temp1 = card.m_equip[m_select_equip_id];
            if (temp1 != null)
            {
                temp1.role_guid = 0;
                card.get_role().zhuangbeis[m_select_equip_id] = 0;
            }

            if (temp != null)
            {
                temp.role_guid = card.get_guid();
                card.get_role().zhuangbeis[m_select_equip_id] = temp.guid;
            }
            card.update_role_attr();
            sys._instance.m_card = card;
            select_info(m_select_card_id);
            show_equip(m_select_card_id);
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
            protocol.game.cmsg_role_equip _msg = new protocol.game.cmsg_role_equip();
            _msg.role_guid = card.get_guid();
            _msg.index.Add(m_select_equip_id);
            _msg.equip_guid.Add(equip_guid);
            net_http._instance.send_msg<protocol.game.cmsg_role_equip>(opclient_t.CMSG_ROLE_EQUIP, _msg);
        }
        if (message.m_type == "common_equip_tihuan" && this.gameObject.activeSelf)
        {
            root_gui._instance.show_common_equip_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_1826_47"), true, true, m_select_equip_id + 1, new List<ulong>(), "common_select1_equip", false, 1, this.gameObject);//请选择装备
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        if (message.m_type == "common_treasure_tihuan" && this.gameObject.activeSelf)
        {
            root_gui._instance.show_common_treasure_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_1833_50"), true, true, m_select_treasure_id + 1, new List<ulong>(), "common_select1_treasure", false, 1, this.gameObject);//请选择需要的饰品
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        if (message.m_type == "common_select_treasure")
        {
            ulong treasure_guid = (ulong)message.m_long[0];
            dhc.treasure_t temp = sys._instance.m_self.get_treasure_guid(treasure_guid);
            ulong guid = 0;
            if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
            {
                guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
            }
            else
            {
                guid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
            }
            ccard card = sys._instance.m_self.get_card_guid(guid);
            dhc.treasure_t temp1 = card.m_treasure[m_select_treasure_id];
            if (temp1 != null)
            {
                temp1.role_guid = 0;
                card.get_role().treasures[m_select_treasure_id] = 0;
            }

            if (temp != null)
            {
                temp.role_guid = card.get_guid();
                card.get_role().treasures[m_select_treasure_id] = temp.guid;
            }
            card.update_role_attr();
            sys._instance.m_card = card;
            select_info(m_select_card_id);
            show_treasure(m_select_card_id);
            s_message _message2 = new s_message();
            _message2.m_type = "check_bf";
            cmessage_center._instance.add_message(_message2);
            protocol.game.cmsg_treasure_equip _msg = new protocol.game.cmsg_treasure_equip();
            _msg.role_guid = card.get_guid();
            _msg.index.Add(m_select_treasure_id);
            _msg.treasure_guid.Add(treasure_guid);
            net_http._instance.send_msg<protocol.game.cmsg_treasure_equip>(opclient_t.CMSG_TREASURE_EQUICP, _msg);
        }
        if (message.m_type == "update_bu_zheng")
        {
            ulong guid = 0;
            bool flag = false;
            if (message.m_long.Count > 0)
            {
                card_id = (ulong)message.m_long[0];
            }
            if (message.m_bools.Count > 0)
            {
                flag = (bool)message.m_bools[0];
            }
            if (m_select_card_id >= sys._instance.m_self.m_t_player.zhenxing.Count
               && m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count < my_cards.Count)
            {
                guid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
            }
            update_ui();
            if (m_select_card_id >= sys._instance.m_self.m_t_player.zhenxing.Count
               && m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count < my_cards.Count)
            {
                for (int i = 0; i < my_cards.Count; ++i)
                {
                    if (guid == my_cards[i].get_guid())
                    {
                        m_select_card_id = sys._instance.m_self.m_t_player.zhenxing.Count + i;
                        break;
                    }
                }
                if (my_cards.Count <= 0)
                {
                    m_select_card_id = 0;
                    guid = sys._instance.m_self.m_t_player.zhenxing[0];
                }
                else if (m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count >= my_cards.Count)
                {
                    m_select_card_id = sys._instance.m_self.m_t_player.zhenxing.Count + my_cards.Count - 1;
                    int row = (m_select_card_id) / 2;
                    int lie = (m_select_card_id) % 2;
                    m_select.transform.localPosition = new Vector3(75 + lie * 100, -305 - row * 100, 0);
                }
            }
            if (flag)
            {
                m_card.update_role_attr_ex();
                sys._instance.m_card = m_card;
                select_info(m_select_card_id);
            }
            else
            {
                string s = "";
                sys._instance.m_card = null;
                select_info(m_select_card_id);
                if (sys._instance.m_houyuans.Count > 0)
                {
                    m_dacheng_des.SetActive(false);
                    hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();
                    if (_hide_Label != null)
                    {
                        Destroy(_hide_Label);
                    }
                    s += sys._instance.m_self.check_houyuan(sys._instance.m_houyuans, sys._instance.m_houyuan_sxs, card_id);
                    m_dacheng_des.GetComponent<UILabel>().text = "";
                    m_dacheng_des.GetComponent<UILabel>().text = s;
                    dacheng();
                    card_id = 0;
                }
            }
        }
        if (message.m_type == "show_tp_label")
        {
            m_show_Label.SetActive(true);
            m_show_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language("xtp_sjzt_001");
            m_show_Label.GetComponent<UILabel>().gradientTop = new Color(240f / 255f, 1.0f, 180f / 255f);
            m_show_Label.GetComponent<UILabel>().gradientBottom = new Color(1.0f, 1.0f, 0.0f);

            Label_time();
            is_effect(m_select_card_id);
        }
        if (message.m_type == "show_jj_label")
        {
            s_t_jinjie t_jinjie = game_data._instance.get_jinjie(m_card.get_jlevel());
            if (t_jinjie.point != 0)
            {
                m_dacheng_des.SetActive(false);
                hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();
                if (_hide_Label != null)
                {
                    Destroy(_hide_Label);
                }
                m_dacheng_des.GetComponent<UILabel>().text = "";
                m_dacheng_des.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("bu_zheng_panel.cs_1964_63"), t_jinjie.point);//全属性[00ff00] +{0}%
                dacheng();
            }
            m_show_Label.SetActive(true);
            m_show_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language("xtp_sjzt_004");
            m_show_Label.GetComponent<UILabel>().gradientTop = new Color(250f / 255f, 125f / 255f, 250f / 255f);
            m_show_Label.GetComponent<UILabel>().gradientBottom = new Color(250f / 255f, 0.0f, 250f / 255f);
            Label_time();
            is_effect(m_select_card_id);
        }
        if (message.m_type == "show_skill_label")
        {
            m_show_Label.SetActive(true);

            m_show_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language("xtp_sjzt_003");
            m_show_Label.GetComponent<UILabel>().gradientTop = new Color(45f / 255f, 1.0f, 1.0f);
            m_show_Label.GetComponent<UILabel>().gradientBottom = new Color(0.0f, 100f / 255f, 1.0f);
            Label_time();
        }
        if (message.m_type == "show_cn_label")
        {
            m_show_Label.SetActive(true);
            m_show_Label.GetComponent<UILabel>().text = game_data._instance.get_t_language("xtp_sjzt_002");
            m_show_Label.GetComponent<UILabel>().gradientTop = new Color(174f / 255f, 1.0f, 92f / 255f);
            m_show_Label.GetComponent<UILabel>().gradientBottom = new Color(34f / 255f, 1.0f, 0.0f);
            Label_time();
            is_effect(m_select_card_id);
        }
        if (message.m_type == "show_gongzheng_skill_gui")
        {
            m_gongzheng_mubiao_gui.GetComponent<gongzheng_mubiao_gui>().skill();
            m_gongzheng_mubiao_gui.SetActive(true);
        }
        if (message.m_type == "update_duixing_effect")
        {
            m_duixing_effect.SetActive(duixing_gui.up_duixing());
        }
        if (message.m_type == "updte_guanghuan_gui")
        {
            m_guanghuan_id = (int)message.m_ints[0];
            update_guanghuan();
        }
        if (message.m_type == "select_pet_guard")
        {
            m_pet_guard_id = (ulong)message.m_long[0];
            protocol.game.cmsg_pet_guard _msg = new protocol.game.cmsg_pet_guard();
            _msg.pet_guid = m_pet_guard_id;
            _msg.role_guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
            net_http._instance.send_msg<protocol.game.cmsg_pet_guard>(opclient_t.CMSG_PET_GUARD, _msg);
        }
        if (message.m_type == "common_select_pet")
        {
            change_pet_id = (ulong)message.m_long[0];
            protocol.game.cmsg_pet_on _msg = new protocol.game.cmsg_pet_on();

            if (change_pet_id == 0)
            {
                _msg.guid = sys._instance.m_self.m_t_player.pet_on;
            }
            else
            {
                _msg.guid = change_pet_id;
            }

            net_http._instance.send_msg<protocol.game.cmsg_pet_on>(opclient_t.CMSG_PET_ON, _msg);
        }
        if (message.m_type == "pet_guard_tihuan")
        {
            m_pet_guard_detail.transform.Find("frame_big").GetComponent<frame>().hide();
            pet_guard();
            m_pet_guard_gui.SetActive(true);
        }
        if (message.m_type == "updte_bz_guanghuan_gui")
        {
            set_guanghuan();
            update_guanghuan();
        }
    }

    void Label_time()
    {
        hide_time _hide = m_show_Label.GetComponent<hide_time>();

        if (_hide == null)
        {
            _hide = m_show_Label.AddComponent<hide_time>();
        }

        _hide.m_time = 0.3f;
    }

    void show_equip(int id)
    {
        ulong guid = 0;
        if (id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            guid = sys._instance.m_self.m_t_player.zhenxing[id];
        }
        else
        {
            guid = my_cards[id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
        }
        ccard card = sys._instance.m_self.get_card_guid(guid);
        for (int i = 0; i < 4; i++)
        {
            set_equip(i, card.m_equip[i]);
        }
    }

    void show_treasure(int id)
    {
        ulong guid = 0;
        if (id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            guid = sys._instance.m_self.m_t_player.zhenxing[id];
        }
        else
        {
            guid = my_cards[id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
        }
        ccard card = sys._instance.m_self.get_card_guid(guid);
        for (int i = 0; i < 2; i++)
        {
            set_treasure(i, card.m_treasure[i]);
        }
    }

    bool is_open(int id)
    {
        if (id == 1 && sys._instance.m_self.m_t_player.level < (int)e_open_level.el_second_role)
        {
            return false;
        }
        if (id == 2 && sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_third_role)
        {
            return false;
        }

        if (id == 3 && sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_fourth_role)
        {
            return false;
        }

        if (id == 4 && sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_fifth_role)
        {
            return false;
        }

        if (id == 5 && sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_sixth_role)
        {
            return false;
        }

        if (id == 6 && sys._instance.m_self.get_att(e_player_attr.player_level) < 46)
        {
            return false;
        }

        if (id == 7 && sys._instance.m_self.get_att(e_player_attr.player_level) < 53)
        {
            return false;
        }

        if (id == 8 && sys._instance.m_self.get_att(e_player_attr.player_level) < 63)
        {
            return false;
        }
        return true;
    }

    public static bool is_treasure_open(int id)
    {
        if (id == 1 && sys._instance.m_self.get_att(e_player_attr.player_level) < 15)
        {
            return false;
        }

        if (id == 2 && sys._instance.m_self.get_att(e_player_attr.player_level) < 15)
        {
            return false;
        }
        return true;
    }

    void set_card_guid(int id, ulong guid)
    {
        if (id >= 6)
        {
            return;
        }
        Transform _add_tag = m_cards[id].transform.Find("null");
        Transform tf = m_cards[id].transform.Find("clabel");
        if (tf)
        {
            tf.gameObject.SetActive(true);
        }
        if (!is_open(id))
        {
            _add_tag.gameObject.SetActive(false);
            if (tf)
            {
                tf.gameObject.SetActive(false);
            }
            return;
        }
        else if (guid <= 0)
        {
            sys._instance.remove_child(m_cards[id], id.ToString());
            sys._instance.remove_child(m_cards[id], "label");
            _add_tag.gameObject.SetActive(true);
            return;
        }

        sys._instance.remove_child(m_cards[id], id.ToString());
        sys._instance.remove_child(m_cards[id], "label");
        _add_tag.gameObject.SetActive(true);

        GameObject _icon = icon_manager._instance.create_card_icon_ex(guid);
        _icon.transform.name = id.ToString();
        _icon.transform.parent = m_cards[id].transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.AddComponent<UIDragScrollView>().scrollView = m_cards[id].transform.parent.GetComponent<UIScrollView>();
        UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
        meses[0].target = this.gameObject;
        meses[0].functionName = "click_card_icon";
        meses[1].target = null;
        meses[1].functionName = "";
        meses[2].target = null;
        meses[2].functionName = "";

        sys._instance.remove_child(m_card_view);
        my_cards.Clear();
        foreach (int class_id in game_data._instance.m_dbc_class.m_index.Keys)
        {
            s_t_class t_class = game_data._instance.get_t_class(class_id);
            ccard c_card = sys._instance.m_self.get_card_id(class_id);
            if (sys._instance.m_self.has_card(class_id) && is_zhengxing(c_card.get_guid()))
            {
                my_cards.Add(c_card);
            }
        }
        my_cards.Sort(compare);
        int card_id = sys._instance.m_self.m_t_player.zhenxing.Count;
        for (int i = 0; i < my_cards.Count; ++i)
        {
            int row = i / 2;
            int lie = i % 2;
            GameObject icon = icon_manager._instance.create_card_icon_ex(my_cards[i]);
            icon.transform.name = card_id.ToString();
            icon.transform.parent = m_card_view.transform;
            icon.transform.localPosition = new Vector3(75 + lie * 100, -305 - row * 100, 0);
            icon.transform.localScale = Vector3.one;
            icon.AddComponent<UIDragScrollView>();
            UIButtonMessage[] message = icon.transform.GetComponents<UIButtonMessage>();
            message[0].target = this.gameObject;
            message[0].functionName = "click_card_icon_ex";
            message[1].target = null;
            message[1].functionName = "";
            message[2].target = null;
            message[2].functionName = "";

            card_id++;
        }
    }

    void set_guanghuan()
    {
        Transform _add_tag = m_guanghuan_icon.transform.Find("null");
        Transform tf = m_guanghuan_icon.transform.Find("clabel");
        if (tf)
        {
            tf.gameObject.SetActive(true);
        }
        if (sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_guanghuan)
        {
            _add_tag.gameObject.SetActive(false);
            if (tf)
            {
                tf.gameObject.SetActive(false);
            }
            return;
        }
        else if (sys._instance.m_self.m_t_player.guanghuan_id <= 0)
        {
            sys._instance.remove_child(m_guanghuan_icon, "guanghuan_icon");
            sys._instance.remove_child(m_guanghuan_icon, "label");
            _add_tag.gameObject.SetActive(true);
            return;
        }

        sys._instance.remove_child(m_guanghuan_icon, "guanghuan_icon");
        sys._instance.remove_child(m_guanghuan_icon, "label");
        _add_tag.gameObject.SetActive(true);
        s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan(sys._instance.m_self.m_t_player.guanghuan_id);
        GameObject _icon = icon_manager._instance.create_guanghuan_icon(sys._instance.m_self.m_t_player.guanghuan_id, guanghuan_gui.guanghuan_level(t_guanghuan));
        _icon.transform.name = "guanghuan_icon";
        _icon.transform.parent = m_guanghuan_icon.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.AddComponent<UIDragScrollView>().scrollView = m_guanghuan_icon.transform.parent.GetComponent<UIScrollView>();
        UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
        meses[0].target = this.gameObject;
        meses[0].functionName = "click_guanghuan_icon";
        meses[1].target = null;
        meses[1].functionName = "";
        meses[2].target = null;
        meses[2].functionName = "";
    }

    void set_pet()
    {
        Transform _add_tag = m_pet_icon.transform.Find("null");
        Transform tf = m_pet_icon.transform.Find("clabel");
        if (tf)
        {
            tf.gameObject.SetActive(true);
        }
        if (sys._instance.m_self.get_att(e_player_attr.player_level) < (int)e_open_level.el_pet)
        {
            _add_tag.gameObject.SetActive(false);
            if (tf)
            {
                tf.gameObject.SetActive(false);
            }
            return;
        }
        else if (sys._instance.m_self.m_t_player.pet_on <= 0)
        {
            sys._instance.remove_child(m_pet_icon, "pet_icon");
            sys._instance.remove_child(m_pet_icon, "label");
            _add_tag.gameObject.SetActive(true);
            return;
        }

        sys._instance.remove_child(m_pet_icon, "pet_icon");
        sys._instance.remove_child(m_pet_icon, "label");
        _add_tag.gameObject.SetActive(true);

        GameObject _icon = icon_manager._instance.create_pet_icon(sys._instance.m_self.m_t_player.pet_on);
        _icon.transform.name = "pet_icon";
        _icon.transform.parent = m_pet_icon.transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.AddComponent<UIDragScrollView>().scrollView = m_guanghuan_icon.transform.parent.GetComponent<UIScrollView>();
        UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
        meses[0].target = this.gameObject;
        meses[0].functionName = "click_pet_icon";
        meses[1].target = null;
        meses[1].functionName = "";
        meses[2].target = null;
        meses[2].functionName = "";
    }

    public void dacheng()
    {
        if (m_dacheng_des.GetComponent<UILabel>().text != "")
        {
            m_dacheng_des.SetActive(true);
            TweenScale _scale = sys._instance.add_scale_anim(m_dacheng_des.gameObject, 0.2f, 0.5f, 1.2f, 0);
            EventDelegate.Add(_scale.onFinished, delegate ()
                              {
                                  hide();
                              }, true);
        }
    }

    public bool is_zhengxing(ulong guid)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i++)
        {
            if (guid == sys._instance.m_self.m_t_player.zhenxing[i])
            {
                return false;
            }
        }
        for (int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count; i++)
        {
            if (guid == sys._instance.m_self.m_t_player.houyuan[i])
            {
                return false;
            }
        }
        return true;
    }

    public static int compare(ccard x, ccard y)
    {
        if (x.m_t_class.color > y.m_t_class.color)
        {
            return -1;
        }
        else if (x.m_t_class.color < y.m_t_class.color)
        {
            return 1;
        }
        else if (x.get_pinzhi() > y.get_pinzhi())
        {
            return -1;
        }
        else if (x.get_pinzhi() < y.get_pinzhi())
        {
            return 1;
        }
        else if (x.get_glevel() > y.get_glevel())
        {
            return -1;
        }
        else if (x.get_glevel() < y.get_glevel())
        {
            return 1;
        }
        else if (x.get_jlevel() > y.get_jlevel())
        {
            return -1;
        }
        else if (x.get_jlevel() < y.get_jlevel())
        {
            return 1;
        }
        else if (x.get_level() > y.get_level())
        {
            return -1;
        }
        else if (x.get_level() < y.get_level())
        {
            return 1;
        }
        else if (x.m_t_class.id > y.m_t_class.id)
        {
            return -1;
        }
        else if (x.m_t_class.id < y.m_t_class.id)
        {
            return 1;
        }
        return 0;
    }


    void set_equip(int id, dhc.equip_t equip)
    {
        sys._instance.remove_child(m_equips[id]);

        bool flag = false;
        for (int i = 0; i < sys._instance.m_self.get_equip_num(); ++i)
        {
            dhc.equip_t _equip = sys._instance.m_self.get_equip_index(i);
            if (_equip.role_guid == 0)
            {
                s_t_equip t_equip = game_data._instance.get_t_equip(_equip.template_id);
                if (t_equip.type == id + 1)
                {
                    flag = true;
                    break;
                }
            }
        }
        if (sys._instance.m_self.is_zheng(m_card.get_guid()))
        {
            m_adds[id].gameObject.SetActive(flag);
        }
        else
        {
            m_adds[id].gameObject.SetActive(false);
        }

        if (equip == null)
        {
            return;
        }

        GameObject _icon = icon_manager._instance.create_equip_icon(equip);
        _icon.transform.GetComponent<BoxCollider>().enabled = false;

        _icon.transform.name = "icon";
        _icon.transform.parent = m_equips[id].transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
    }

    void set_treasure(int id, dhc.treasure_t treasure)
    {
        sys._instance.remove_child(m_treasures[id]);

        bool flag = false;
        for (int i = 0; i < sys._instance.m_self.get_treasure_num(); ++i)
        {
            dhc.treasure_t _treasure = sys._instance.m_self.get_treasure_index(i);
            if (_treasure.role_guid == 0)
            {
                s_t_baowu t_baowu = game_data._instance.get_t_baowu(_treasure.template_id);
                if (t_baowu.type == id + 1 && is_treasure_open(id + 1))
                {
                    flag = true;
                    break;
                }
            }
        }
        if (sys._instance.m_self.is_zheng(m_card.get_guid()))
        {
            m_bw_adds[id].gameObject.SetActive(flag);
        }
        else
        {
            m_bw_adds[id].gameObject.SetActive(false);
        }

        for (int i = 0; i < 2; ++i)
        {
            if (is_treasure_open(i + 1))
            {
                level[i].SetActive(false);
                name[i].SetActive(true);
            }
            else
            {
                level[i].SetActive(true);
                name[i].SetActive(false);
            }
        }

        if (treasure == null)
        {
            return;
        }

        GameObject _icon = icon_manager._instance.create_treasure_icon(treasure);
        _icon.transform.GetComponent<BoxCollider>().enabled = false;

        _icon.transform.name = "icon";
        _icon.transform.parent = m_treasures[id].transform;
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        _icon.transform.localScale = new Vector3(1, 1, 1);
    }

    public void select_equip(GameObject obj)
    {
        if (m_select_card_id >= sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_491_59"));//伙伴未上阵
            return;
        }
        m_select_equip_id = int.Parse(obj.transform.name);
        ulong guid = 0;
        if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
        }
        else
        {
            guid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
        }
        ccard card = sys._instance.m_self.get_card_guid(guid);
        m_t_equips.Clear();
        for (int i = 0; i < 4; ++i)
        {
            dhc.equip_t equip = card.m_equip[i];
            if (equip != null)
            {
                m_t_equips.Add(equip);
            }
        }
        if (card.m_equip[m_select_equip_id] == null)
        {
            root_gui._instance.show_common_equip_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_1826_47"), false, true, m_select_equip_id + 1, new List<ulong>(), "common_select1_equip", false, 1, this.gameObject);//请选择装备
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        else
        {
            dhc.equip_t equip = card.m_equip[m_select_equip_id];
            root_gui._instance.add_equip(m_t_equips, true);
            root_gui._instance.show_equip_detail(equip, 1, this.gameObject, true);
        }
    }

    public void select_treasure(GameObject obj)
    {
        if (m_select_card_id >= sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_491_59"));//伙伴未上阵
            return;
        }
        m_select_treasure_id = int.Parse(obj.transform.name) - 4;
        if (!is_treasure_open(m_select_treasure_id + 1))
        {
            if (m_select_treasure_id == 0 || m_select_treasure_id == 1)
            {
                string str = game_data._instance.get_t_language("bu_zheng_panel.cs_2560_29");//饰品{0}级开启
                str = string.Format(str, "15");
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
            }
            return;
        }
        ulong guid = 0;
        if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            guid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
        }
        else
        {
            guid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
        }
        ccard card = sys._instance.m_self.get_card_guid(guid);
        m_t_treasures.Clear();
        for (int i = 0; i < 2; ++i)
        {
            dhc.treasure_t treasure = card.m_treasure[i];
            if (treasure != null)
            {
                m_t_treasures.Add(treasure);
            }

        }
        if (card.m_treasure[m_select_treasure_id] == null)
        {
            root_gui._instance.show_common_treasure_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_1833_50"), false, true, m_select_treasure_id + 1, new List<ulong>(), "common_select1_treasure", false, 1, this.gameObject);//请选择需要的饰品
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        else
        {
            dhc.treasure_t treasure = card.m_treasure[m_select_treasure_id];
            root_gui._instance.add_treasure(m_t_treasures, true);
            root_gui._instance.show_treasure_detail(treasure, 1, this.gameObject, true);
        }
    }

    public void click_card_icon(GameObject obj)
    {
        m_guanghuan_panel.SetActive(false);
        m_sx_panel.SetActive(true);
        m_select_card_id = int.Parse(obj.transform.name);
        m_tihuan.SetActive(true);
        m_tihuan.name = "tihuan";
        m_tihuan.transform.Find("change_role").GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_2608_77");//点击更换伙伴
        select_info(m_select_card_id);
        show_equip(m_select_card_id);
        show_treasure(m_select_card_id);
    }

    public void click_card_icon_ex(GameObject obj)
    {
        m_guanghuan_panel.SetActive(false);
        m_sx_panel.SetActive(true);
        m_select_card_id = int.Parse(obj.transform.name);
        m_tihuan.name = "shangzhen";
        bool flag = false;
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[i] == 0 && is_open(i))
            {
                flag = true;
                break;
            }
        }
        if (flag)
        {
            m_tihuan.SetActive(true);
        }
        else
        {
            m_tihuan.SetActive(false);
        }
        m_tihuan.transform.Find("change_role").GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_2637_78");//点击伙伴上阵
        select_info(m_select_card_id);
        show_equip(m_select_card_id);
        show_treasure(m_select_card_id);
    }

    public void click_guanghuan_icon(GameObject obj)
    {
        m_select_card_id = 999;
        m_select.transform.localPosition = m_guanghuan_icon.transform.localPosition;
        m_tihuan.SetActive(false);
        update_guanghuan();
        select_info(m_select_card_id);
    }


    public void click_pet_icon(GameObject obj)
    {
        is_pet = true;
        m_select_card_id = 10000;
        m_select.transform.localPosition = m_pet_icon.transform.localPosition;
        update_pet();
        select_info(m_select_card_id);
    }

    public void update_guanghuan()
    {
        s_t_guanghuan t_guanghuan;
        if (m_guanghuan_id == 0)
        {
            m_guanghuan_id = sys._instance.m_self.m_t_player.guanghuan_id;
        }
        t_guanghuan = game_data._instance.get_t_guanghuan(m_guanghuan_id);
        m_guanghuan_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(t_guanghuan.color) + t_guanghuan.name;
        if (t_guanghuan.attr1 == 1)
        {
            m_guanghuan_type.GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_2668_51");//生命型
        }
        else if (t_guanghuan.attr1 == 2)
        {
            m_guanghuan_type.GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_2672_51");//攻击型
        }
        int level = guanghuan_gui.guanghuan_level(t_guanghuan);
        string text = "";
        string[] _text = guanghuan_gui.get_attr(m_guanghuan_id, level, 0).Split('+');
        text += (_text[0] + "   +" + _text[1]);
        if (t_guanghuan.attr2 > 0)
        {
            _text = guanghuan_gui.get_attr(m_guanghuan_id, level, 1).Split('+');
            text += "\n" + (_text[0] + "   +" + _text[1]);
        }
        m_guanghuan_sx.GetComponent<UILabel>().text = text;
        List<s_t_guanghuan_skill> t_guanghuan_skills = new List<s_t_guanghuan_skill>();
        foreach (int _id in game_data._instance.m_dbc_guanghuan_skill.m_index.Keys)
        {
            s_t_guanghuan_skill t_guanghuan_skill = game_data._instance.get_t_guanghuan_skill(_id);
            if (t_guanghuan_skill.wing_id == t_guanghuan.id)
            {
                t_guanghuan_skills.Add(t_guanghuan_skill);
            }
        }
        string zh = "";
        for (int i = 0; i < t_guanghuan_skills.Count; ++i)
        {
            zh += m_guanghuan_gui.GetComponent<guanghuan_gui>().get_skill_des(t_guanghuan_skills[i], zh == "");
        }
        if (zh == "")
        {
            m_guanghuan_jn.GetComponent<UILabel>().text = "[FFFF00]" + game_data._instance.get_t_language("bu_zheng_panel.cs_2702_63");//无技能
        }
        else
        {
            m_guanghuan_jn.GetComponent<UILabel>().text = zh;
        }
    }

    public void update_pet()
    {
        pet m_pet = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pet_on);
        m_pet_name.text = m_pet.get_color_name();
        m_pet_hp.text = ((int)m_pet.get_attr(1)).ToString();
        m_pet_attack.text = ((int)m_pet.get_attr(2)).ToString();
        m_pet_wf.text = ((int)m_pet.get_attr(3)).ToString();
        m_pet_mf.text = ((int)m_pet.get_attr(4)).ToString();
        string text = "";
        int id = m_pet.m_t_pet.skills[0];
        if (id > 0)
        {
            s_t_pet_skill t_pet_skill = game_data._instance.get_t_pet_skill(id);
            text += "[ffff00]" + t_pet_skill.name;
            if (t_pet_skill.attack_type == 1)
            {
                text += " [ff0000][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2726_26") + "]\n";//物理
            }
            else if (t_pet_skill.attack_type == 2)
            {
                text += " [7fa0ff][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
            }
            text += "[0aabff]" + t_pet_skill.des;
        }
        id = m_pet.m_t_pet.skills[1];
        if (id > 0)
        {
            s_t_pet_skill t_pet_skill = game_data._instance.get_t_pet_skill(id + m_pet.get_star());
            text += "\n\n";
            text += "[ffff00]" + t_pet_skill.name;
            if (t_pet_skill.attack_type == 1)
            {
                text += " [ff0000][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2726_26") + "]\n";//物理
            }
            else if (t_pet_skill.attack_type == 2)
            {
                text += " [7fa0ff][" + game_data._instance.get_t_language("bu_zheng_panel.cs_2730_26") + "]\n";//魔法
            }
            else
            {
                text += "\n";
            }
            text += "[0aabff]" + t_pet_skill.des;
        }
        m_pet_skill.text = text;
        m_pet_desc.text = m_pet.m_t_pet.desc;
        m_extra.text = string.Format(game_data._instance.get_t_language("bu_zheng_panel.cs_2756_32"), m_pet.m_t_pet.sz_sx_add * 100);//上阵为全队提供自身基本属性*{0}%的属性加成
        s_t_pet_jinjie t_pet_jinjie = game_data._instance.get_t_pet_jinjie(m_pet.get_jlevel());
        m_pet_jj.GetComponent<UISprite>().spriteName = t_pet_jinjie.icon;
        m_pet_jj.GetComponent<UISprite>().MakePixelPerfect();
    }

    public void select_card(GameObject obj)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[i] == 0)
            {
                m_preselect_card_id = i;
                break;
            }
        }

        if (!is_open(m_preselect_card_id))
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("bu_zheng_panel.cs_2775_58"));//尚未开启
            return;
        }

        List<ulong> self = new List<ulong>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
            {
                self.Add(sys._instance.m_self.m_t_player.zhenxing[i]);
            }
        }
        for (int i = 0; i < sys._instance.m_self.m_t_player.houyuan.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.houyuan[i] != 0)
            {
                self.Add(sys._instance.m_self.m_t_player.houyuan[i]);
            }
        }
        bool flag = true;
        if (sys._instance.m_self.m_t_player.zhenxing[int.Parse(obj.name)] == 0)
        {
            flag = false;
        }
        root_gui._instance.show_common_card_panel(game_data._instance.get_t_language(game_data._instance.get_t_language("bu_zheng_panel.cs_300_46")), flag, 1, self, "common_select_card", false, this.gameObject);//请选择需要上阵的伙伴
        this.GetComponent<ui_show_anim>().hide_ui();
        this.GetComponent<gui_remove>().m_remove = false;
    }

    public void select_guanghuan(GameObject obj)
    {
        if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_guanghuan)
        {
            string str = game_data._instance.get_t_language("bu_zheng_panel.cs_728_17");//光环{0}级开启
            str = string.Format(str, ((int)e_open_level.el_guanghuan).ToString());
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
            return;
        }
        m_select_card_id = 999;
        m_bu_zheng_panel.SetActive(false);
        m_guanghuan_gui.GetComponent<guanghuan_gui>().m_card = m_card;
        m_guanghuan_gui.SetActive(true);

    }

    public void select_pet(GameObject obj)
    {
        if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pet)
        {
            string str = game_data._instance.get_t_language("bu_zheng_panel.cs_2819_16");//宠物{0}级开启
            str = string.Format(str, ((int)e_open_level.el_pet).ToString());
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
            return;
        }
        List<ulong> self = new List<ulong>();
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            ccard _card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[i]);
            if (_card == null)
            {
                continue;
            }
            if (_card.get_role().pet > 0)
            {
                self.Add(_card.get_role().pet);
            }
        }
        self.Add(sys._instance.m_self.m_t_player.pet_on);
        root_gui._instance.show_common_pet_panel(game_data._instance.get_t_language("bu_zheng_panel.cs_814_45"), false, 0, self, "common_select_pet", false, this.gameObject, 1);//请选择需要上阵的宠物
        this.GetComponent<ui_show_anim>().hide_ui();
        this.GetComponent<gui_remove>().m_remove = false;
    }


    public void click_info(GameObject obj)
    {
        m_select_card_id = int.Parse(obj.transform.name);
        select_info(m_select_card_id);
    }

    public string get_jb_text(int jb_id, bool first)
    {
        ulong _jguid = 0;
        if (m_select_card_id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            _jguid = sys._instance.m_self.m_t_player.zhenxing[m_select_card_id];
        }
        else
        {
            _jguid = my_cards[m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
        }
        ccard _jcard = sys._instance.m_self.get_card_guid(_jguid);
        string _text = "";
        string color = "";
        s_t_ji_ban _jb = game_data._instance.get_t_ji_ban(jb_id);
        if (_jb.type == 1)
        {
            bool flag = true;
            for (int i = 0; i < _jb.tids.Count; ++i)
            {
                if (i != 0)
                {
                    _text += "、";
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
                    color = "\n\n" + "[FFFF00]";
                }
                else
                {
                    color = "[FFFF00]";
                }
            }
            else
            {
                if (!first)
                {
                    color = "\n\n" + "[777777]";
                }
                else
                {
                    color = "[777777]";
                }
            }
            string s = "";
            if (_jb.attr1 > 0)
            {
                if (_jb.attr2 > 0)
                {
                    s += game_data._instance.get_value_string(_jb.attr1, _jb.value1) + "、";
                }
                else
                {
                    s += game_data._instance.get_value_string(_jb.attr1, _jb.value1);
                }
            }
            if (_jb.attr2 > 0)
            {
                s += game_data._instance.get_value_string(_jb.attr2, _jb.value2);
            }
            _text = color + string.Format(game_data._instance.get_t_language("bu_zheng_panel.cs_2922_32"), _jb.name, _text, s);//◆【{0}】 与{1}一起上阵，{2}
        }
        else if (_jb.type == 2)
        {
            bool flag = false;
            for (int j = 0; j < _jb.tids.Count; ++j)
            {
                flag = false;
                for (int i = 0; i < _jcard.m_equip.Count; i++)
                {
                    if (_jcard.m_equip[i] == null)
                    {
                        continue;
                    }

                    if (_jb.tids[j] == _jcard.m_equip[i].template_id)
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
                for (int i = 0; i < _jcard.m_treasure.Count; i++)
                {
                    if (_jcard.m_treasure[i] == null)
                    {
                        continue;
                    }

                    if (_jb.tids[j] == _jcard.m_treasure[i].template_id)
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

    public void select_info(int id, bool pz = false)
    {
        role_effect();
        pet_effect();
        if (id == 999)
        {
            m_xq.SetActive(false);
            m_fight.SetActive(false);
            if (sys._instance.m_self.m_t_player.guanghuan_id <= 0)
            {
                for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i++)
                {
                    if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
                    {
                        m_select_card_id = i;
                        id = i;
                        break;
                    }
                }
            }
            else
            {
                ccard card = sys._instance.m_self.get_card_guid(sys._instance.m_self.m_t_player.zhenxing[0]);
                m_pet_tihuan.name = "guanghuan";
                m_pet_tihuan.transform.Find("tihuan").GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_3064_78");//更换
                m_juexing_level.SetActive(false);
                m_sx_panel.SetActive(false);
                m_guanghuan_panel.SetActive(true);
                m_zb.SetActive(false);
                m_tihuan.SetActive(false);
                m_gongzhen_button.SetActive(false);
                m_pet_panel.SetActive(false);
                m_select.transform.localPosition = m_guanghuan_icon.transform.localPosition;
                update_guanghuan();
                if (is_pet)
                {
                    m_card = card;
                    s_message _message = new s_message();
                    _message.m_type = "show_unit";
                    _message.time = 0.1f;
                    _message.m_object.Add(card);
                    _message.m_ints.Add((int)1);
                    _message.m_string.Add("select_show");
                    cmessage_center._instance.add_message(_message);
                }
                is_pet = false;
                return;
            }
        }
        if (id == 10000)
        {
            if (sys._instance.m_self.m_t_player.pet_on <= 0)
            {
                for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i++)
                {
                    if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
                    {
                        m_select_card_id = i;
                        id = i;
                        break;
                    }
                }
            }
            else
            {
                m_pet_tihuan.name = "pet_tihuan";
                m_pet_tihuan.transform.Find("tihuan").GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_3064_78");//更换
                m_juexing_level.SetActive(false);
                pet m_pet = sys._instance.m_self.get_pet_guid(sys._instance.m_self.m_t_player.pet_on);
                m_xq.SetActive(false);
                m_fight.SetActive(true);
                m_fight.transform.GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_3104_54") + sys._instance.value_to_wan((long)m_pet.get_fight());//战斗力  
                m_sx_panel.SetActive(false);
                m_pet_panel.SetActive(true);
                m_tihuan.SetActive(false);
                m_guanghuan_panel.SetActive(false);
                m_zb.SetActive(false);
                m_gongzhen_button.SetActive(false);
                m_select.transform.localPosition = m_pet_icon.transform.localPosition;
                if (m_pet_xiangxi_panel.activeSelf)
                {
                    m_pet_down.SetActive(true);
                }
                else
                {
                    m_pet_down.SetActive(false);
                }
                update_pet();
                s_message _message = new s_message();
                _message.m_type = "show_pet";
                _message.time = 0.1f;
                _message.m_object.Add(m_pet);
                _message.m_ints.Add((int)0);
                _message.m_string.Add("select_show");
                cmessage_center._instance.add_message(_message);
                return;
            }
        }
        m_pet_tihuan.name = "huwei";
        m_pet_tihuan.transform.Find("tihuan").GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_3132_76");//宠物
        m_juexing_level.SetActive(true);
        m_sx_panel.SetActive(true);
        m_guanghuan_panel.SetActive(false);
        m_pet_panel.SetActive(false);
        m_zb.SetActive(true);
        m_xq.SetActive(true);
        m_fight.SetActive(false);
        m_zuhe_view.GetComponent<UIScrollView>().ResetPosition();
        ulong _guid = 0;
        GameObject _sx = m_info;
        if (id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            m_info_id = id;
            int _id = id;
            _guid = sys._instance.m_self.m_t_player.zhenxing[_id];
        }
        else
        {
            _guid = my_cards[id - sys._instance.m_self.m_t_player.zhenxing.Count].get_guid();
        }
        ccard _card = sys._instance.m_self.get_card_guid(_guid);
        if (m_card != _card || is_pet)
        {
            m_card = _card;
            if (m_card.m_t_class.sound != "0")
            {
                if (!pz)
                {
                    sys._instance.play_sound_ex("sound/ts_rwpy/" + m_card.m_t_class.sound);
                }
            }
            pz = true;

            s_message mes = new s_message();
            mes.m_type = "action_ex";
            mes.m_string.Add("win");
            mes.m_long.Add(m_card.get_guid());
            mes.m_long.Add(m_card.get_role().pet);
            mes.time = 0.5f;
            cmessage_center._instance.add_message(mes);
        }
        if (pz)
        {
            s_message _message = new s_message();
            _message.m_type = "show_unit";
            _message.time = 0.1f;
            _message.m_object.Add(_card);
            _message.m_ints.Add((int)1);
            _message.m_string.Add("select_show");
            cmessage_center._instance.add_message(_message);
        }
        is_pet = false;
        s_t_jinjie t_jinjie = game_data._instance.get_jinjie(m_card.get_jlevel());
        m_look.GetComponent<UISprite>().spriteName = t_jinjie.icon;
        m_look.GetComponent<UISprite>().MakePixelPerfect();
        _card.update_role_attr_ex();
        string s = "";
        if ((sys._instance.m_card != null && sys._instance.m_card == m_card))
        {
            m_dacheng_des.SetActive(false);
            hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();
            if (_hide_Label != null)
            {
                Destroy(_hide_Label);
            }
            s += sys._instance.m_self.check_gongzhen(m_card, sys._instance.m_gongzhengs, m_old_equips, m_old_treasures, sys._instance.m_sxs, card_id);
            m_dacheng_des.GetComponent<UILabel>().text = "";
            m_dacheng_des.GetComponent<UILabel>().text = s;
            dacheng();
            sys._instance.m_card = null;
            card_id = 0;
        }

        int _hp = (int)_card.get_attr(1);
        int _damage = (int)_card.get_attr(2);
        int _pd = (int)_card.get_attr(3);
        int _md = (int)_card.get_attr(4);
        int _speed = (int)_card.get_attr(5);

        _sx.transform.Find("hp").GetComponent<UILabel>().text = _hp.ToString();
        _sx.transform.Find("attack").GetComponent<UILabel>().text = _damage.ToString();
        _sx.transform.Find("pd").GetComponent<UILabel>().text = _pd.ToString();
        _sx.transform.Find("md").GetComponent<UILabel>().text = _md.ToString();
        _sx.transform.Find("speed").GetComponent<UILabel>().text = _speed.ToString();

        if (id < sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            m_select.transform.localPosition = m_cards[id].transform.localPosition;
        }
        else
        {
            int row = (m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count) / 2;
            int lie = (m_select_card_id - sys._instance.m_self.m_t_player.zhenxing.Count) % 2;
            m_select.transform.localPosition = new Vector3(75 + lie * 100, -305 - row * 100, 0);
        }
        show_equip(id);
        show_treasure(id);

        string _zh = "";

        if (_card.m_t_class.jbs[0] > 0)
        {
            _zh += get_jb_text(_card.m_t_class.jbs[0], _zh == "");
        }
        if (_card.m_t_class.jbs[1] > 0)
        {
            _zh += get_jb_text(_card.m_t_class.jbs[1], _zh == "");
        }
        if (_card.m_t_class.jbs[2] > 0)
        {
            _zh += get_jb_text(_card.m_t_class.jbs[2], _zh == "");
        }
        if (_card.m_t_class.jbs[3] > 0)
        {
            _zh += get_jb_text(_card.m_t_class.jbs[3], _zh == "");
        }
        if (_card.m_t_class.jbs[4] > 0)
        {
            _zh += get_jb_text(_card.m_t_class.jbs[4], _zh == "");
        }
        if (_card.m_t_class.jbs[5] > 0)
        {
            _zh += get_jb_text(_card.m_t_class.jbs[5], _zh == "");
        }
        if (_card.m_t_class.jbs[6] > 0)
        {
            _zh += get_jb_text(_card.m_t_class.jbs[6], _zh == "");
        }
        if (_card.m_t_class.jbs[7] > 0)
        {
            _zh += get_jb_text(_card.m_t_class.jbs[7], _zh == "");
        }
        if (_zh == "")
        {
            _zh = game_data._instance.get_t_language("bu_zheng_panel.cs_3270_10");//该伙伴没有组合属性
        }
        m_des.transform.GetComponent<UILabel>().text = _zh;

        m_jibanex_des.transform.GetComponent<UILabel>().text = m_card.m_t_class.card;

        m_name.GetComponent<UILabel>().text = _card.m_t_class.name;
        is_effect(id);
        m_duixing_effect.SetActive(duixing_gui.up_duixing());
        error = "";
        dbc m_dbc_role_dress = game_data._instance.get_dbc_role_dress();
        List<s_t_role_dress> m_dresss_role = new List<s_t_role_dress>();
        for (int i = 0; i < m_dbc_role_dress.get_y(); ++i)
        {
            int role_dress_id = int.Parse(m_dbc_role_dress.get(0, i));
            s_t_role_dress _t_role_dress = game_data._instance.get_t_role_dress(role_dress_id);
            if (_t_role_dress.role == _card.get_template_id() && _t_role_dress.hq_condition != 3)
            {
                m_dresss_role.Add(_t_role_dress);
            }
        }
        if (m_dresss_role.Count == 0)
        {
            m_shizhuang.SetActive(false);
            m_shizhuang1.SetActive(true);
        }
        else
        {
            m_shizhuang.SetActive(true);
            m_shizhuang1.SetActive(false);
        }
        if (m_card != null && m_card.get_color() >= 4 && m_card.get_guid() > 0 && m_card.get_role() != null && m_card.get_role().xq != 3)
        {
            m_xq.SetActive(true);
            m_xq.GetComponent<UISprite>().spriteName = xq_change_item.xq_icon(m_card.get_role().xq);
            m_xq.transform.Find("xq").GetComponent<UILabel>().text = xq_change_item.xq_Label(m_card.get_role().xq);
        }
        else
        {
            m_xq.SetActive(false);
        }
        if (can_enhance())
        {
            m_yenhance.GetComponent<UISprite>().spriteName = "bz_ironanniu01b";
            m_yenhance.transform.Find("yijianqh").gameObject.SetActive(true);
            m_yenhance.transform.Find("yijianqh_1").gameObject.SetActive(false);
            m_yenhance.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            m_yenhance.GetComponent<UISprite>().spriteName = "bz_ironanniu01_ft";
            m_yenhance.transform.Find("yijianqh").gameObject.SetActive(false);
            m_yenhance.transform.Find("yijianqh_1").gameObject.SetActive(true);
            m_yenhance.GetComponent<BoxCollider>().enabled = false;
        }
        if (m_select_card_id >= sys._instance.m_self.m_t_player.zhenxing.Count)
        {
            m_gongzhen_button.SetActive(false);
            m_tihuan.name = "shangzhen";
            m_tihuan.transform.Find("change_role").GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_2637_78");//点击伙伴上阵
        }
        else
        {
            m_gongzhen_button.SetActive(true);
            m_tihuan.SetActive(true);
            m_tihuan.name = "tihuan";
            m_tihuan.transform.Find("change_role").GetComponent<UILabel>().text = game_data._instance.get_t_language("bu_zheng_panel.cs_2608_77");//点击更换伙伴
        }
        sys._instance.m_gongzhengs.Clear();
        sys._instance.m_gongzhengs = gongzheng_mubiao_gui.gongzhen_cur(m_card);
        m_old_equips.Clear();
        m_old_treasures.Clear();
        for (int i = 0; i < m_card.m_equip.Count; ++i)
        {
            dhc.equip_t equip = m_card.m_equip[i];
            if (equip != null)
            {
                m_old_equips.Add(equip);
            }
        }
        for (int i = 0; i < m_card.m_treasure.Count; ++i)
        {
            dhc.treasure_t treasure = m_card.m_treasure[i];
            if (treasure != null)
            {
                m_old_treasures.Add(treasure);
            }
        }
        sys._instance.m_sxs.Clear();
        sys._instance.m_sxs = sys._instance.m_self.get_gongzhen(m_card);
        if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shengpin)
        {
            m_juexing_level.SetActive(false);
        }
        else
        {
            s_t_role_shengpin t_role_shengpin = game_data._instance.get_t_role_shengpin(m_card.get_pinzhi());
            m_juexing_level.GetComponent<UILabel>().text = game_data._instance.get_name_color(t_role_shengpin.color) + t_role_shengpin.name;
            m_juexing_level.SetActive(true);
        }
    }

    public void update_ui()
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i++)
        {
            set_card_guid(i, sys._instance.m_self.m_t_player.zhenxing[i]);
        }
        set_guanghuan();
        set_pet();
    }

    public void hide()
    {
        hide_Label _hide_Label = m_dacheng_des.GetComponent<hide_Label>();

        if (_hide_Label == null)
        {
            _hide_Label = m_dacheng_des.AddComponent<hide_Label>();
        }
        _hide_Label.m_time = 1.6f;
    }

    public void pet_click(GameObject obj)
    {
        if (obj.transform.name == "chong_neng")
        {
            s_message _message = new s_message();
            _message.m_type = "show_pet_cn_gui";
            _message.m_long.Add(sys._instance.m_self.m_t_player.pet_on);
            _message.m_string.Add("show_bu_zheng");
            cmessage_center._instance.add_message(_message);
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        else if (obj.transform.name == "shengxing")
        {
            s_message _message = new s_message();
            _message.m_type = "show_pet_sx_gui";
            _message.m_long.Add(sys._instance.m_self.m_t_player.pet_on);
            _message.m_string.Add("show_bu_zheng");
            cmessage_center._instance.add_message(_message);
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        else if (obj.transform.name == "jinjie")
        {
            s_message _message = new s_message();
            _message.m_type = "show_pet_jj_gui";
            _message.m_long.Add(sys._instance.m_self.m_t_player.pet_on);
            _message.m_string.Add("show_bu_zheng");
            cmessage_center._instance.add_message(_message);
            this.GetComponent<ui_show_anim>().hide_ui();
            this.GetComponent<gui_remove>().m_remove = false;
        }
        else if (obj.transform.name == "tj")
        {
            s_message _message = new s_message();
            _message.m_type = "show_pet_tj_gui";
            _message.m_long.Add(sys._instance.m_self.m_t_player.pet_on);
            cmessage_center._instance.add_message(_message);
        }
    }

    void Update()
    {
        if (m_need_update == true)
        {
            m_need_update = false;
            update_ui();
        }
        if (tx_effect)
        {
            int num = 0;
            for (int i = 0; i < tx.Count; ++i)
            {
                if (!tx[i].GetComponent<UISpriteAnimation>().isPlaying)
                {
                    tx[i].SetActive(false);
                }
                if (!tx[i].activeSelf)
                {
                    num++;
                }
            }
            if (num == m_card.m_equip.Count)
            {
                tx_effect = false;
            }
        }
    }
}	
