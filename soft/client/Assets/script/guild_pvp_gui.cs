
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class guild_pvp_gui : MonoBehaviour ,IMessage
{
    public UIProgressBar m_chengfangvalue;
    public UILabel m_sucreward;
    public UILabel m_succondition;
    public UILabel m_value;
    public UILabel m_attack_num;
    public UILabel m_desc;
    public UILabel m_name;
    public static int m_id;
    public int num;
    public List<GameObject> m_units;
    public GameObject m_guild_info;
    public static  protocol.game.msg_guild_fight_info m_guild_fightinfo;
    public int m_index;
    bool m_flght_flag = false;

    bool m_addflag;
    void Start()
    {
        addHandle();
    }
    public void addHandle()
    {
        cmessage_center._instance.add_handle(this);
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "buy_guild_fight_num")
        {
            if (this.gameObject.activeSelf)
            {
                protocol.game.cmsg_shop_buy _msg = new protocol.game.cmsg_shop_buy();
                _msg.num = (int)mes.m_ints[0];
                num = _msg.num;
                net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_GUILD_JT_BUY, _msg);
 
            }
         
        }
        else if (mes.m_type == "guildfightpvp_flag")
        {
            m_addflag = false;
        }
        else if (mes.m_type == "resert_comeback_gui")
        {
            m_addflag = false;
        }
        else if (mes.m_type == "hide_cishudialog_box")
        {
            m_addflag = false;
        }
       
 
    }
    void IMessage.net_message(s_net_message msg)
    {
        if (msg.m_opcode == opclient_t.CMSG_PVP_GUILD_FIGHT)
        {
            protocol.game.smsg_guild_fight _msg = net_http._instance.parse_packet<protocol.game.smsg_guild_fight>(msg.m_byte);
            battle_logic_ex._instance.set_guild_pvp_fight_end(_msg);
            sys._instance.m_game_state = "buttle";
            sys._instance.load_scene_ex("ts_chapter01");
            this.gameObject.SetActive(false);
            s_message _new_msg = new s_message();
            _new_msg.m_type = "guildfight_info";
            _new_msg.m_object.Add(m_guild_fightinfo);
            _new_msg.m_ints.Add(m_index);
            cmessage_center._instance.add_message(_new_msg);
            m_flght_flag = false;
            remove_all();
        }
        else if(msg.m_opcode == opclient_t.CMSG_GUILD_JT_BUY)
        {
            if (this.gameObject.activeSelf)
            {
                int jewel = 0;
                for (int i = sys._instance.m_self.m_t_player.guild_pvp_buy_num + 1; i <= sys._instance.m_self.m_t_player.guild_pvp_buy_num + num; i++)
                {
                    s_t_price t_price = game_data._instance.get_t_price(i);
                    jewel += t_price.guildpvpbuy;

                }
                sys._instance.m_self.sub_att(e_player_attr.player_jewel, jewel, game_data._instance.get_t_language ("guild_pvp_gui.cs_91_80"));//跨服军团战攻打次数购买
                sys._instance.m_self.m_t_player.guild_pvp_buy_num += num;
                sys._instance.m_self.m_t_player.guild_pvp_num += num;
                string s = game_data._instance.get_t_language(game_data._instance.get_t_language ("bingyuan_gui.cs_776_19"));//获得
                root_gui._instance.show_prompt_dialog_box("[00ff00]" + s, 0, "", "[ffffc0]" + "攻打次数" + "[ffd000] + " + num.ToString());
                reset(1);
 
            }
            
        }
 
    }
    void buy_guild_fight_num()
    {
        s_t_price t_price = game_data._instance.get_t_price(sys._instance.m_self.m_t_player.guild_pvp_buy_num + 1);
        s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
        if (sys._instance.m_self.m_t_player.guild_pvp_buy_num >= t_vip.guildpvpbuy_num)
        {
            root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language(game_data._instance.get_t_language ("guild_buttle_gui.cs_73_47")));//[ffc882]今日购买次数已经用完，提升vip等级可增加购买次数
            return;
        }
        m_addflag = true;
        if (sys._instance.m_self.m_t_player.jewel < t_price.guildpvpbuy)
        {
            root_gui._instance.show_recharge_dialog_box(
                    delegate()
                    {

                    }
                );
            return;
        }
        s_message _mes = new s_message();
        _mes.m_type = "buy_guild_fight_num";
        root_gui._instance.show_cishu_dialog_box(sys._instance.m_self.m_t_player.guild_pvp_num, t_vip.guildpvpbuy_num, sys._instance.m_self.m_t_player.guild_pvp_buy_num, 5, _mes);
    }
    public void click(GameObject obj)
    {
        if (obj.name == "add")
        {
            buy_guild_fight_num();

        }
        else if (obj.name == "close")
        {
            sys._instance.m_game_state = "hall";
            sys._instance.load_scene(sys._instance.m_hall_name);
            s_message _mes = new s_message();
            _mes.m_type = "show_guild_fight_gui";
            cmessage_center._instance.add_message(_mes);
            remove_all();  
            this.gameObject.SetActive(false);
 
        }
       
        else
        {
            if (sys._instance.m_self.m_t_player.guild_pvp_num > 0)
            {
                int index = int.Parse(obj.name);
                if (m_guild_fightinfo.target_defense_nums[index] < game_data._instance.get_guild_fight(m_id).defendnum)
                {
                    fight(int.Parse(obj.name));
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_pvp_gui.cs_157_62"));//[ffc884]敌方防守次数不足
                }

            }
            else
            {
                buy_guild_fight_num();
            }
        }
    }
    void fight(int index)
    {
        if (m_guild_fightinfo.guard_gongpo[m_id] > 0)
        {
            root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("guild_pvp_gui.cs_171_54"));//[ffc884]该建筑已被攻破，请攻打其他建筑
        }
        else
        {
            protocol.game.cmsg_guild_fight _msg = new protocol.game.cmsg_guild_fight();
            _msg.target_index = index;
            _msg.target_guild = m_guild_fightinfo.guild;
            m_index = index;
            net_http._instance.send_msg<protocol.game.cmsg_guild_fight>(opclient_t.CMSG_PVP_GUILD_FIGHT, _msg);
            m_flght_flag = true;
        }
       
    }
    public void reset(int type = 0)
    {
        if (type == 0)
        {
            create_huoban();
        }
        m_flght_flag = false;
        s_t_guildfight fight = game_data._instance.get_guild_fight(m_id);
        m_sucreward.text = game_data._instance.get_t_language ("guild_pvp_gui.cs_192_27") + "\n" + string.Format(game_data._instance.get_t_language ("guild_pvp_gui.cs_192_57"),fight.exp);//攻破奖励//获得{0}军团经验
        m_succondition.text = game_data._instance.get_t_language ("guild_pvp_gui.cs_193_30") + "\n" + game_data._instance.get_t_language ("guild_pvp_gui.cs_193_46");//攻破条件//城防值降为0
        m_chengfangvalue.value = (float)(fight.chengfangvalue - m_guild_fightinfo.guard_points[m_id]) / fight.chengfangvalue;
        m_value.text = (fight.chengfangvalue - m_guild_fightinfo.guard_points[m_id]) + " /" + fight.chengfangvalue;
        m_name.text = fight.name + game_data._instance.get_t_language ("guild_pvp_gui.cs_196_35");//城防值
        m_attack_num.text = game_data._instance.get_t_language ("guild_pvp_gui.cs_197_28") + "：" + sys._instance.m_self.m_t_player.guild_pvp_num + "";//攻打次数
        m_desc.text = string.Format(game_data._instance.get_t_language ("guild_pvp_gui.cs_198_36"), fight.suc_chengfangvalue, fight.suc_con);//挑战胜利可减少{0}点城防值，获得{1}军团贡献，挑战的敌人战力越高，获得的个人战绩越高
 
    }
    void remove_all()
    {
        for (int i = 0; i < m_units.Count; i++)
        {
            GameObject.Destroy(m_units[i]);
        }

        m_units.Clear();
    }
    Vector3 get_tran(int id)
    {
        if (m_id == 3)
        {
            return new Vector3(0, -0.05f, -0.45f);
        }
        else
        {
            return new Vector3(3.3f - id % 7 * 1.1f, -0.05f, -0.45f);
        }
 
    }
    IEnumerator addcolider(GameObject unit)
    {
        yield return new WaitForSeconds(1f);
        CapsuleCollider cap = unit.AddComponent<CapsuleCollider>();
        cap.center = new Vector3(0, 1, 0);
        cap.height = 2;
    }
    void create_huoban()
    {
        remove_all();
        int i = 0;
        int count = 0;
        if (m_id == 3)
        {
            i = m_guild_fightinfo.target_templates.Count - 1;
            count = m_guild_fightinfo.target_templates.Count;
        }
        else
        {
            i = 7 * m_id;
            count = 7 * m_id + 7;
        }
        s_t_guildfight fight = game_data._instance.get_guild_fight(m_id);
        for (; i < count; i++)
        {
            ccard _card = new ccard();
            
            GameObject _unit = sys._instance.create_class
            ((int)m_guild_fightinfo.target_templates[i], m_guild_fightinfo.target_achieves[i], 0); ;
            _unit.name = i + "";
            _unit.tag = "guild_fb";
            _unit.transform.localPosition = get_tran(i);
            StartCoroutine(addcolider(_unit));
            if (m_guild_fightinfo.target_templates[i] == 27)
            {
                _unit.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }
            if (m_guild_fightinfo.target_defense_nums[i] >= fight.defendnum)
            {
                _unit.GetComponent<unit>().set_bh(5);
            }
            else
            {
                _unit.GetComponent<unit>().set_bh(0);
 
            }
            m_units.Add(_unit);
        }
        GameObject camera = GameObject.Find("ts_byzd002/Camera_byx02");
        sys._instance.remove_child(m_guild_info);
        for (int j = 0; j < m_units.Count; j++)
        {
            Vector3 pos;
            if (j % 2 == 0)
            {
                Vector3 temp = m_units[j].transform.position;
                pos = UICamera.currentCamera.ScreenToWorldPoint(camera.GetComponent<Camera>().WorldToScreenPoint(new Vector3(temp.x,temp.y + m_units[j].GetComponent<unit>().m_name_height)));
            }
            else
            {
                pos = UICamera.currentCamera.ScreenToWorldPoint(camera.GetComponent<Camera>().WorldToScreenPoint(m_units[j].transform.position));
            }

            GameObject obj = game_data._instance.ins_object_res("ui/juntuan/guildfight_info");
           
            obj.transform.parent = m_guild_info.transform;
            obj.transform.localScale = Vector3.one;
            obj.transform.position = new Vector3(pos.x,pos.y,0);
            if (j % 2 == 0)
            {
                if (m_units[j].GetComponent<unit>().m_t_class.id == 27)
                {
                    obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y - 127, 0);
                }
                else
                {
                    obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y - 7, 0);
                }


            }
            else
            {
                obj.transform.localPosition = new Vector3(obj.transform.localPosition.x, obj.transform.localPosition.y, 0);
 
            }
            if (j == 0 && m_units.Count > 1)
            {
                obj.transform.localPosition = new Vector3(-380f, 94f, 0);
            }
            else if(j == 0)
            {
                obj.transform.localPosition = new Vector3(0, 94f, 0);
 
            }
            obj.transform.Find("name").GetComponent<UILabel>().text = m_guild_fightinfo.target_names[int.Parse(m_units[j].name)];
            obj.transform.Find("num").GetComponent<UILabel>().text = game_data._instance.get_t_language ("guild_pvp_gui.cs_318_74") + (fight.defendnum - m_guild_fightinfo.target_defense_nums[int.Parse(m_units[j].name)]) + "";//防守次数：
            obj.transform.Find("bf").GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_detail.cs_55_17") + sys._instance.value_to_wan(m_guild_fightinfo.target_bat_effs[int.Parse(m_units[j].name)]) + "";//战斗力 

        }

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Camera camera = GameObject.Find("ts_byzd002/Camera_byx02").GetComponent<Camera>();
            if (camera != null)
            {
                Ray ray = camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    GameObject gameObj = hitInfo.collider.gameObject;
                    string name = gameObj.name;
                    if (gameObj.transform.tag == "guild_fb")
                    {
                        if (m_flght_flag)
                        {
                            return;
                        }
                        TweenScale _scale = null;
                        if (gameObj.GetComponent<unit>().m_t_class.id != 27)
                        {
                            gameObj.transform.localScale = Vector3.one;
                            _scale = TweenScale.Begin(gameObj, 0.35f, new Vector3(1.1f, 1.1f, 1.1f));

                        }
                        else
                        {
                            gameObj.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f); ;
                            _scale = TweenScale.Begin(gameObj, 0.35f, new Vector3(0.8f, 0.8f, 0.8f));
                            _scale.from = new Vector3(0.7f, 0.7f, 0.7f);
                        }
                        _scale.updateTable = false;
                        _scale.method = UITweener.Method.Linear;
                        _scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));

                        if (!m_addflag)
                        {
                            click(gameObj);
                        }
                    }
                }
 
            }
           
        }
    }

}
