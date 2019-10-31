using System.Collections.Generic;
using UnityEngine;

public class catch_card_gui : MonoBehaviour, IMessage
{
    public GameObject m_free_time_1;
    public GameObject m_free_image_1;
    public GameObject m_tj_panel;
    public GameObject m_yisuoji_panel;
    public GameObject m_weisuoji_panel;
    public GameObject m_effect;
    public bool flat = false;
    private Dictionary<int, GameObject> m_icons = new Dictionary<int, GameObject>();
    int m_type;
    public UILabel m_yishouji;
    public UILabel m_weishouji;
    public List<GameObject> effects = new List<GameObject>();
    private float life_time = 0.01f;
    private int num = 0;
    public GameObject m_frame_big;
    public GameObject m_yisuoji_Button;
    public GameObject m_weisuoji_Button;
    public GameObject m_tishi_box;

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
        InvokeRepeating("time", 0.0f, 1.0f);
        create_panel();
        num = 0;
        m_effect.GetComponent<UIPanel>().alpha = 0;
        m_effect.GetComponent<Animator>().enabled = false;
        m_weisuoji_panel.SetActive(false);
    }

    public void reset()
    {
        num = 0;
        life_time = 0.01f;
        if (m_tj_panel.activeSelf)
        {
            m_tj_panel.GetComponent<ui_bloom_anim>().hide_ui();
            m_tj_panel.SetActive(false);
        }
        update_ui();
    }

    void OnDisable()
    {
        CancelInvoke("time");
    }

    void IMessage.net_message(s_net_message message)
    {

        if (message.m_opcode == opclient_t.CMSG_CHOUKA)
        {
            protocol.game.smsg_chouka _msg = net_http._instance.parse_packet<protocol.game.smsg_chouka>(message.m_byte);
            sys._instance.m_self.set_att(e_player_attr.player_jewel, _msg.jewel);

            if (m_type == 1)
            {
                sys._instance.m_self.remove_item(80010001, 1, game_data._instance.get_t_language("catch_card_gui.cs_82_50"));//抽卡消耗
            }
            else if (m_type == 10)
            {
                sys._instance.m_self.remove_item(80010001, 10, game_data._instance.get_t_language("catch_card_gui.cs_82_50"));//抽卡消耗
            }
            else if (m_type == 2)
            {
                long _time = (long)sys._instance.m_self.m_t_player.ck2_free_time - (long)timer.now();
                if (_time > 0)
                {
                    if (sys._instance.m_self.get_item_num(80010002) > 0)
                    {
                        sys._instance.m_self.remove_item(80010002, 1, game_data._instance.get_t_language("catch_card_gui.cs_82_50"));//抽卡消耗
                    }
                }
                sys._instance.m_self.add_active(550, 1);
            }
            else if (m_type == 20)
            {
                if (sys._instance.m_self.get_item_num(80010002) >= 10)
                {
                    sys._instance.m_self.remove_item(80010002, 10, game_data._instance.get_t_language("catch_card_gui.cs_82_50"));//抽卡消耗
                }
                sys._instance.m_self.add_active(550, 1);
            }
            else if (m_type == 3)
            {
                sys._instance.m_self.set_att(e_player_attr.player_friend_point, sys._instance.m_self.get_att(e_player_attr.player_friend_point) - 10);
            }
            else if (m_type == 30)
            {
                sys._instance.m_self.set_att(e_player_attr.player_friend_point, sys._instance.m_self.get_att(e_player_attr.player_friend_point) - 100);
            }
            sys._instance.m_self.m_t_player.ck2_free_time = _msg.ck2_free_time;
            update_ui();

            s_t_reward t_reward = new s_t_reward();
            t_reward.type = 2;
            t_reward.value1 = _msg.item_ids[0];
            t_reward.value2 = _msg.item_nums[0];
            t_reward.value3 = 0;

            if (flat)
            {
                s_message _message = new s_message();
                _message.m_type = "update_catch_card_show";
                cmessage_center._instance.add_message(_message);
            }
            this.GetComponent<ui_show_anim>().hide_ui();
        }
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "hide_catch_card_gui")
        {
            this.GetComponent<ui_show_anim>().hide_ui();
        }
        if (message.m_type == "again_catch_card_show")
        {
            flat = true;
            protocol.game.cmsg_chouka _msg = new protocol.game.cmsg_chouka();
            _msg.type = (int)message.m_ints[0];
            net_http._instance.send_msg<protocol.game.cmsg_chouka>(opclient_t.CMSG_CHOUKA, _msg);
        }
    }
    public void click(GameObject obj)
    {
        if (obj.name == "close")
        {
            if (m_tj_panel.activeSelf)
            {
                m_frame_big.GetComponent<frame>().hide();
                return;
            }
            this.GetComponent<ui_show_anim>().hide_ui();
            s_message _message = new s_message();
            _message.m_type = "show_main_gui";
            cmessage_center._instance.add_message(_message);
            Object.Destroy(this.gameObject);
            return;
        }
        if (obj.name == "yisuoji")
        {
            m_yisuoji_panel.SetActive(true);
            m_weisuoji_panel.SetActive(false);
            return;
        }
        if (obj.name == "weisuoji")
        {
            m_weisuoji_panel.SetActive(true);
            m_yisuoji_panel.SetActive(false);
            return;
        }
        if (obj.name == "tj")
        {
            m_tj_panel.SetActive(true);
            return;
        }
        if (obj.name == "help")
        {
            m_tishi_box.SetActive(true);
            return;
        }
        m_type = 1;
        if (obj.name == "catch_0")
        {
            m_type = 1;
            if (sys._instance.m_self.get_item_num(80010001) < 1)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("buy_num_gui_ex.cs_224_71"));//招募装置不足
                return;
            }
        }
        else if (obj.name == "catch_1")
        {
            m_type = 2;
            long _time = (long)(sys._instance.m_self.m_t_player.ck2_free_time) - (long)(timer.now());
            if (_time > 0 && sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA))
            {
                root_gui._instance.show_recharge_dialog_box(delegate ()
                {
                    this.GetComponent<ui_show_anim>().hide_ui();
                });
                return;
            }
        }
        else if (obj.name == "catch_2")
        {
            m_type = 3;
            if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN))
            {
                root_gui._instance.show_recharge_dialog_box(delegate ()
                {
                    this.GetComponent<ui_show_anim>().hide_ui();
                });
                return;
            }
        }
        else if (obj.name == "buleone")
        {
            m_type = 1;
            if (sys._instance.m_self.get_item_num(80010001) < 1)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("buy_num_gui_ex.cs_224_71"));//道具不足
                return;
            }
        }
        else if (obj.name == "buleten")
        {
            m_type = 10;
            if (sys._instance.m_self.get_item_num(80010001) < 10)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("buy_num_gui_ex.cs_224_71"));//道具不足
                return;
            }
        }
        else if (obj.name == "yellowone")
        {
            m_type = 2;
            long _time = (long)(sys._instance.m_self.m_t_player.ck2_free_time) - (long)(timer.now());
            if (_time > 0 && sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA) && sys._instance.m_self.get_item_num(80010002) < 1)
            {
                root_gui._instance.show_recharge_dialog_box(delegate ()
                {
                    this.GetComponent<ui_show_anim>().hide_ui();
                });
                return;
            }
        }
        else if (obj.name == "yellowten")
        {
            m_type = 20;
            long _time = (long)(sys._instance.m_self.m_t_player.ck2_free_time) - (long)(timer.now());
            if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN) && sys._instance.m_self.get_item_num(80010002) < 10)
            {
                root_gui._instance.show_recharge_dialog_box(delegate ()
                {
                    this.GetComponent<ui_show_anim>().hide_ui();
                });
                return;
            }
        }
        else if (obj.name == "purpleone")
        {
            m_type = 3;
            if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 10)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("catch_card_gui.cs_337_56"));//	友情点不足
                return;
            }
        }
        else if (obj.name == "purpleten")
        {
            m_type = 30;
            if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 100)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("catch_card_gui.cs_337_56"));//	友情点不足
                return;
            }
        }
        protocol.game.cmsg_chouka _msg = new protocol.game.cmsg_chouka();
        _msg.type = m_type;
        net_http._instance.send_msg<protocol.game.cmsg_chouka>(opclient_t.CMSG_CHOUKA, _msg);
    }

    public void pclick(GameObject obj)
    {
        if (obj.name == "close")
        {
            m_tj_panel.GetComponent<ui_bloom_anim>().hide_ui();
        }
    }

    public void end()
    {
        num = 1;
    }

    void catch_effects()
    {
        int num = sys._instance.m_self.get_item_num(80010001);
        if (num < 1)
        {
            this.transform.Find("center").Find("catch_0").Find("buleone").Find("effect").gameObject.SetActive(false);
            this.transform.Find("center").Find("catch_0").Find("buleten").Find("effect").gameObject.SetActive(false);
        }
        else if (num < 10)
        {
            this.transform.Find("center").Find("catch_0").Find("buleone").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_0").Find("buleten").Find("effect").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("center").Find("catch_0").Find("buleone").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_0").Find("buleten").Find("effect").gameObject.SetActive(true);
        }

        int num2 = sys._instance.m_self.get_item_num(80010002);
        if (num2 < 1 && sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA))
        {

            this.transform.Find("center").Find("catch_1").Find("yellowone").Find("effect").gameObject.SetActive(false);
            this.transform.Find("center").Find("catch_1").Find("yellowten").Find("effect").gameObject.SetActive(false);
        }

        if (num2 > 1 || sys._instance.m_self.get_att(e_player_attr.player_jewel) > game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA)
            && num2 < 10 || sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN))
        {
            this.transform.Find("center").Find("catch_1").Find("yellowone").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_1").Find("yellowten").Find("effect").gameObject.SetActive(false);
        }

        if (num2 >= 10 || sys._instance.m_self.get_att(e_player_attr.player_jewel) >= game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN))
        {
            this.transform.Find("center").Find("catch_1").Find("yellowone").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_1").Find("yellowten").Find("effect").gameObject.SetActive(true);
        }

        if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 10)
        {
            this.transform.Find("center").Find("catch_2").Find("purpleone").Find("effect").gameObject.SetActive(false);
            this.transform.Find("center").Find("catch_2").Find("purpleten").Find("effect").gameObject.SetActive(false);
        }
        else if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 100)
        {
            this.transform.Find("center").Find("catch_2").Find("purpleone").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_2").Find("purpleten").Find("effect").gameObject.SetActive(false);
        }
        else
        {
            this.transform.Find("center").Find("catch_2").Find("purpleone").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_2").Find("purpleten").Find("effect").gameObject.SetActive(true);
        }
    }

    void update_ui()
    {
        int num = sys._instance.m_self.get_item_num(80010001);
        int num2 = sys._instance.m_self.get_item_num(80010002);
        long _time = (long)(sys._instance.m_self.m_t_player.ck2_free_time) - (long)(timer.now());
        catch_effects();
        if (num < 1)
        {
            this.transform.Find("center").Find("catch_0").Find("buleshowone").GetComponent<UILabel>().text = "[ff0000]x 1";
            this.transform.Find("center").Find("catch_0").Find("buleshowten").GetComponent<UILabel>().text = "[ff0000]x 10";
        }
        else if (num < 10)
        {
            this.transform.Find("center").Find("catch_0").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_0").Find("buleshowone").GetComponent<UILabel>().text = "x 1";
            this.transform.Find("center").Find("catch_0").Find("buleshowten").GetComponent<UILabel>().text = "[ff0000]x 10";
        }
        else
        {
            this.transform.Find("center").Find("catch_0").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_0").Find("buleshowone").GetComponent<UILabel>().text = "x 1";
            this.transform.Find("center").Find("catch_0").Find("buleshowten").GetComponent<UILabel>().text = "x 10";
        }

        if (num2 < 1)
        {
            if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA))
            {
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").GetComponent<UILabel>().text = string.Format("[ff0000]x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA));
                this.transform.Find("center").Find("catch_1").Find("yellowshowten").Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                this.transform.Find("center").Find("catch_1").Find("yellowshowten").GetComponent<UILabel>().text = string.Format("[ff0000]x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN));
            }
            else if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN))
            {
                this.transform.Find("center").Find("catch_1").Find("effect").gameObject.SetActive(true);
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").GetComponent<UILabel>().text = string.Format("x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA));
                this.transform.Find("center").Find("catch_1").Find("yellowshowten").Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                this.transform.Find("center").Find("catch_1").Find("yellowshowten").GetComponent<UILabel>().text = string.Format("[ff0000]x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN));
            }
            else
            {
                this.transform.Find("center").Find("catch_1").Find("effect").gameObject.SetActive(true);
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").GetComponent<UILabel>().text = string.Format("x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA));
                this.transform.Find("center").Find("catch_1").Find("yellowshowten").Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                this.transform.Find("center").Find("catch_1").Find("yellowshowten").GetComponent<UILabel>().text = string.Format("x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN));
            }
        }
        else
        {
            if (num2 < 10)
            {
                this.transform.Find("center").Find("catch_1").Find("effect").gameObject.SetActive(true);
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_gojzmx";
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").GetComponent<UILabel>().text = "x 1";

                if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN))
                {
                    this.transform.Find("center").Find("catch_1").Find("yellowshowten").Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                    this.transform.Find("center").Find("catch_1").Find("yellowshowten").GetComponent<UILabel>().text = string.Format("[ff0000]x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN));
                }
                else
                {
                    this.transform.Find("center").Find("catch_1").Find("yellowshowten").Find("Sprite").GetComponent<UISprite>().spriteName = "zs";
                    this.transform.Find("center").Find("catch_1").Find("yellowshowten").GetComponent<UILabel>().text = string.Format("x {0}", game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA_TEN));
                }
            }
            else
            {
                this.transform.Find("center").Find("catch_1").Find("effect").gameObject.SetActive(true);
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_gojzmx";
                this.transform.Find("center").Find("catch_1").Find("yellowshowone").GetComponent<UILabel>().text = "x 1";
                this.transform.Find("center").Find("catch_1").Find("yellowshowten").Find("Sprite").GetComponent<UISprite>().spriteName = "ckdj_gojzmx";
                this.transform.Find("center").Find("catch_1").Find("yellowshowten").GetComponent<UILabel>().text = "x 10";
            }
        }

        if ((long)(sys._instance.m_self.m_t_player.ck2_free_time) - (long)(timer.now()) < 0)
        {
            this.transform.Find("center").Find("catch_1").Find("yellowshowone").Find("Sprite").GetComponent<UISprite>().spriteName = "gx_tb03";
            this.transform.Find("center").Find("catch_1").Find("yellowshowone").GetComponent<UILabel>().text = game_data._instance.get_t_language("catch_card_gui.cs_368_120");//免费
        }

        if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 10)
        {
            this.transform.Find("center").Find("catch_2").Find("purpleshowone").GetComponent<UILabel>().text = "[ff0000]x 10";
            this.transform.Find("center").Find("catch_2").Find("purpleshowten").GetComponent<UILabel>().text = "[ff0000]x 100";
        }
        else if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) < 100)
        {
            this.transform.Find("center").Find("catch_2").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_2").Find("purpleshowone").GetComponent<UILabel>().text = "x 10";
            this.transform.Find("center").Find("catch_2").Find("purpleshowten").GetComponent<UILabel>().text = "[ff0000]x 100";
        }
        else
        {
            this.transform.Find("center").Find("catch_2").Find("effect").gameObject.SetActive(true);
            this.transform.Find("center").Find("catch_2").Find("purpleshowone").GetComponent<UILabel>().text = "x 10";
            this.transform.Find("center").Find("catch_2").Find("purpleshowten").GetComponent<UILabel>().text = "x 100";
        }
    }

    void time()
    {
        long _time = (long)(sys._instance.m_self.m_t_player.ck2_free_time) - (long)(timer.now());
        int num2 = sys._instance.m_self.get_item_num(80010002);
        if (_time > 0)
        {
            m_free_time_1.SetActive(true);
            m_free_time_1.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("catch_card_gui.cs_245_64"), timer.get_time_show(_time));//{0}后免费
            m_free_image_1.SetActive(false);
            if (num2 < 1 && sys._instance.m_self.m_t_player.jewel < game_data._instance.get_const_vale((int)opclient_t.CONST_CHOUKA))
            {
                this.transform.Find("center").Find("catch_1").Find("effect").gameObject.SetActive(false);
            }
        }
        else
        {
            m_free_time_1.SetActive(false);
            m_free_image_1.SetActive(true);
            this.transform.Find("center").Find("catch_1").Find("effect").gameObject.SetActive(true);
        }
    }
    int compare(s_t_class x, s_t_class y)
    {
        if (y.color == x.color)
        {
            return y.pz - x.pz;
        }
        return y.color - x.color;
    }
    void create_panel()
    {
        List<s_t_class> my_classes = new List<s_t_class>();
        List<s_t_class> unmy_classes = new List<s_t_class>();

        for (int i = 0; i < game_data._instance.m_dbc_class.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_class.get(0, i));
            s_t_class t_class = game_data._instance.get_t_class(id);
            if (t_class.color >= 2 && t_class.ycang == 0)
            {
                if (sys._instance.m_self.has_card(id))
                {
                    my_classes.Add(t_class);
                }
                else
                {
                    unmy_classes.Add(t_class);
                }
            }

        }
        my_classes.Sort(compare);
        unmy_classes.Sort(compare);

        if (m_yisuoji_panel.GetComponent<SpringPanel>() != null)
        {
            m_yisuoji_panel.GetComponent<SpringPanel>().enabled = false;
        }
        sys._instance.remove_child(m_yisuoji_panel);

        float y = m_yishouji.transform.localPosition.y - 80;
        for (int i = 0; i < my_classes.Count; i++)
        {
            int row = i / 2;
            int lie = i % 2;
            GameObject citem = game_data._instance.ins_object_res("ui/catch_card_item_ex");
            citem.transform.parent = m_yisuoji_panel.transform;
            citem.transform.localPosition = new Vector3(-186 + lie * 358, y - 119 * row, 0);
            citem.transform.localScale = new Vector3(1, 1, 1);
            citem.transform.GetComponent<catch_card_item>().id = my_classes[i].id;
            GameObject icon = icon_manager._instance.create_card_icon_ex(my_classes[i].id, 0, 0, 0);
            icon.GetComponent<Collider>().enabled = false;
            icon.transform.parent = citem.transform.Find("icon");
            icon.transform.localPosition = new Vector3(0, 0, 0);
            icon.transform.localScale = new Vector3(1, 1, 1);
            m_icons[my_classes[i].id] = icon;
            if (i == my_classes.Count - 1)
            {
                y = citem.transform.localPosition.y - 80;
            }
        }
        sys._instance.remove_child(m_weisuoji_panel);

        y = m_weishouji.transform.localPosition.y - 80;
        for (int i = 0; i < unmy_classes.Count; i++)
        {
            int row = i / 2;
            int lie = i % 2;
            GameObject citem = game_data._instance.ins_object_res("ui/catch_card_item_ex");
            citem.transform.parent = m_weisuoji_panel.transform;
            citem.transform.localPosition = new Vector3(-186 + lie * 358, y - 119 * row, 0);
            citem.transform.localScale = new Vector3(1, 1, 1);
            citem.transform.GetComponent<catch_card_item>().id = unmy_classes[i].id;
            GameObject icon = icon_manager._instance.create_card_icon_ex(unmy_classes[i].id, 0, 0, 0);
            icon.GetComponent<Collider>().enabled = false;
            icon.transform.parent = citem.transform.Find("icon");
            icon.transform.localPosition = new Vector3(0, 0, 0);
            icon.transform.localScale = new Vector3(1, 1, 1);
            m_icons[unmy_classes[i].id] = icon;
        }
    }

    public static bool can_zhaomu()
    {
        if (timer.now() > sys._instance.m_self.m_t_player.ck2_free_time)
        {
            return true;
        }
        if (sys._instance.m_self.get_item_num(80010001) > 0)
        {
            return true;
        }
        if (sys._instance.m_self.get_item_num(80010002) > 0)
        {
            return true;
        }
        if (sys._instance.m_self.get_att(e_player_attr.player_friend_point) >= 10)
        {
            return true;
        }
        return false;
    }

    void Update()
    {
        if (num == 1)
        {
            life_time -= Time.deltaTime;
            if (life_time < 0)
            {
                life_time = 0.01f;
                m_effect.GetComponent<UIPanel>().alpha += 0.1f;

            }
            if (m_effect.GetComponent<UIPanel>().alpha == 1)
            {
                num = 0;
                m_effect.GetComponent<Animator>().enabled = true;
            }
        }
    }
}
