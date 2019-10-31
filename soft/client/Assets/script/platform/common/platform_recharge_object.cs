using UnityEngine;

public class platform_recharge_object : MonoBehaviour ,IMessage{	

	public static platform_recharge_object _instance;
    private string order = "";
    public bool m_is_wait = false;

    void Awake()
	{
		_instance = this;
	}
	
	void Start () {
        cmessage_center._instance.add_handle(this);
        platform_recharge._instance.init();
        InvokeRepeating("time", 2.0f, 2.0f);
    }

    void OnDestroy()
    {
        CancelInvoke("time");
        cmessage_center._instance.remove_handle(this);
    }

    void Update ()
    {
        platform_recharge._instance.update();
    }

    void time()
    {
        platform_recharge._instance.time();
    }

    void recharge_done(string s)
    {
        platform_recharge._instance.recharge_done(s);
    }

    void recharge_cancel(string s)
    {
        platform_recharge._instance.recharge_cancel(s);
    }

    void recharge_onOderNo(string s)
    {
        platform_recharge._instance.recharge_onOderNo(s);
    }

    void recharge_product(string s)
    {
        platform_recharge._instance.recharge_product(s);
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_RECHARGE)
        {
            root_gui._instance.wait(true);
            platform_recharge._instance.do_check();
        }
        if (message.m_opcode == opclient_t.CMSG_RECHARGE_CHECK_EX)
        {
            protocol.game.smsg_recharge_check_ex _msg = net_http._instance.parse_packet<protocol.game.smsg_recharge_check_ex>(message.m_byte);
            order = timer.now() + "" + sys._instance.m_self.m_guid + "";
            s_t_recharge t_recharge = game_data._instance.get_t_recharge(platform_recharge._instance.m_id);
            for (int i = 0; i < _msg.type_judge.Count; ++i)
            {
                if (_msg.type_judge[i] == 0)
                {
                    end_pay(_msg.rdz_ids[i], 0);
                    if (sys._instance.m_self.m_t_player.huodong_yxhg_tap == 1)
                    {
                        sys._instance.m_self.m_t_player.huodong_yxhg_rmb += (int)t_recharge.vippt;
                    }

                }
                else if (_msg.type_judge[i] == 1)
                {
                    sys._instance.m_self.m_t_player.total_recharge += t_recharge.vippt;
                    for (int f = 0; f < _msg.treasures.Count; f++)
                    {
                        sys._instance.m_self.add_treasure(_msg.treasures[f]);
                    }
                    for (int f = 0; f < _msg.equips.Count; f++)
                    {
                        sys._instance.m_self.add_equip(_msg.equips[f]);
                    }
                    for (int f = 0; f < _msg.pets.Count; f++)
                    {
                        sys._instance.m_self.add_pet(_msg.pets[f]);
                    }
                    for (int f = 0; f < _msg.types.Count; f++)
                    {
                        sys._instance.m_self.add_reward(_msg.types[f], _msg.value1s[f]
                                               , _msg.value2s[f], _msg.value3s[f], "recharge" + game_data._instance.get_t_language("bingyuan_gui.cs_776_19"));//获得
                    }
                    if (sys._instance.m_self.m_t_player.first_reward == 0)
                    {
                        sys._instance.m_self.m_t_player.first_reward = 1;
                    }
                    string body = game_data._instance.m_player_data.m_token + "_" + sys._instance.m_self.m_t_player.serverid + "_" + t_recharge.id;
                    if (platform_config_common.m_test != 1)
                    {
                        platform._instance.on_charge_request(order, body + t_recharge.name, t_recharge.vippt / 10, t_recharge.jewel);
                        platform._instance.on_charge_success(order);
                    }

                    s_message _msg_ = new s_message();
                    _msg_.m_type = "refresh_czfp";
                    cmessage_center._instance.add_message(_msg_);

                    s_message _message1 = new s_message();
                    _message1.m_type = "resert_jc_huodong_pag";
                    cmessage_center._instance.add_message(_message1);


                    s_message _msg_1 = new s_message();
                    _msg_1.m_type = "refresh_explore_chongzhi";
                    _msg_1.m_ints.Add(t_recharge.ios_id);
                    cmessage_center._instance.add_message(_msg_1);
                }
            }
            if (_msg.rdz_ids.Count > 0 && m_is_wait)
            {
                root_gui._instance.wait(false);
                m_is_wait = false;
            }
            platform_recharge._instance.m_need_check = false;
        }
    }

    void IMessage.message(s_message message)
    {

    }

    void end_pay(int id, int count)
    {
        s_t_recharge t_recharge = game_data._instance.get_t_recharge(id);
        int jewel = 0;
        int ve = 0;
		int rmb = 0;
        if (count > 0 && count != t_recharge.vippt / 10)
        {
            Debug.Log("CCCCCCCOOOOUUUTTT::" + count.ToString());
            jewel = count * 10;
            Debug.Log("JWWWWWWWWWWWWWW::" + jewel.ToString());
            ve = count * 10;
			rmb = count;
        }
        else
        {
            ulong now = timer.now();
            if (t_recharge.type == 3)
            {
                bool flag = false;
                for (int i = 0; i < sys._instance.m_self.m_t_player.recharge_ids.Count; ++i)
                {
                    if (sys._instance.m_self.m_t_player.recharge_ids[i] == t_recharge.id)
                    {
                        flag = true;
                        break;
                    }
                }
                if (flag)
                {
                    t_recharge = game_data._instance.get_t_recharge(t_recharge.pid);
                }
            }

            if (t_recharge.type == 1)
            {
                if (now < sys._instance.m_self.m_t_player.zhouka_time)
                {
                    sys._instance.m_self.m_t_player.zhouka_time = sys._instance.m_self.m_t_player.zhouka_time + (ulong)86400000 * 30;
                }
                else
                {
                    sys._instance.m_self.m_t_player.zhouka_time = now + (ulong)86400000 * 30;
                }
                sys._instance.m_self.add_active(200, 1);
            }
            else if (t_recharge.type == 2)
            {
                if (now < sys._instance.m_self.m_t_player.yueka_time)
                {
                    sys._instance.m_self.m_t_player.yueka_time = sys._instance.m_self.m_t_player.yueka_time + (ulong)86400000 * 30;
                }
                else
                {
                    sys._instance.m_self.m_t_player.yueka_time = now + (ulong)86400000 * 30;
                }
                sys._instance.m_self.add_active(300, 1);
            }
            else if (t_recharge.type == 3)
            {
                sys._instance.m_self.m_t_player.recharge_ids.Add(t_recharge.id);
            }
            jewel = t_recharge.jewel;
            ve = t_recharge.vippt;
            rmb = t_recharge.vippt;
        }
        sys._instance.m_self.add_att(e_player_attr.player_jewel, jewel, "");
		sys._instance.m_self.m_t_player.total_recharge += rmb;

        if (jewel >= 198 || sys._instance.m_self.m_t_player.total_recharge >= 1000)
        {
            sys._instance.m_self.m_t_player.guest = 1;
        }
        sys._instance.m_self.add_att(e_player_attr.player_vip_exp, ve);

        if (sys._instance.m_self.m_t_player.first_reward == 0)
        {
            sys._instance.m_self.m_t_player.first_reward = 1;
        }

        string body = game_data._instance.m_player_data.m_token + "_" + sys._instance.m_self.m_t_player.serverid + "_" + t_recharge.id;
        if (platform_config_common.m_test != 1)
        {
            platform._instance.on_charge_request(order, body + t_recharge.name, t_recharge.vippt / 10, t_recharge.jewel);
            platform._instance.on_charge_success(order);
        }
        s_message _msg = new s_message();
        _msg.m_type = "refresh_czfp";
        cmessage_center._instance.add_message(_msg);

        s_message _message1 = new s_message();
        _message1.m_type = "resert_jc_huodong_pag";
        cmessage_center._instance.add_message(_message1);


        s_message msg = new s_message();
        msg.m_type = "refresh_explore_chongzhi";
        msg.m_ints.Add(t_recharge.ios_id);
        cmessage_center._instance.add_message(msg);
    }
}
