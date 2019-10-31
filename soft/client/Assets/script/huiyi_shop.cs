
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class huiyi_shop : MonoBehaviour,IMessage {

    public GameObject m_view;
    private s_t_huiyi_shop m_t_shop;
    int m_gezi;
    public UILabel m_lost_time;
    public int hour;
	// Use this for initialization
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	
	}
	
	// Update is called once per frame
	void Update () 
    {
	
	}
    void OnEnable()
    {
        reset();
    }
    void IMessage.message(s_message message)
    {
        if (message.m_type == "huiyi_shop_buy")
        {
            m_gezi = (int)message.m_ints[1];
            m_t_shop = game_data._instance.get_t_huiyi_shop((int)message.m_ints[0]);
            string item_num = sys._instance.m_self.get_item_num(m_t_shop.reward.type,
                m_t_shop.reward.value1, m_t_shop.reward.value2, m_t_shop.reward.value3);
            if (m_t_shop.huobi_type == 2)
            {
                string _des = string.Format(game_data._instance.get_t_language ("huiyi_shop.cs_41_44"), m_t_shop.huobi_value//是否花费[00ffff]{0}钻石[-]购买{1}
                    , sys._instance.get_res_info(m_t_shop.reward.type,
                    m_t_shop.reward.value1, m_t_shop.reward.value2, m_t_shop.reward.value3, 1)) + "[-]?";
                if (item_num != "")
                {
					_des += "\n" + game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ item_num;//当前拥有：
                }
                s_message _msg = new s_message();

                _msg.m_type = "select_buy_huiyi_item";

                root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"), _des, _msg);//提示
            }
            else
            {
                protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                _net_msg.type = 4;
                _net_msg.gezi = m_gezi + 1;
                _net_msg.num = 1;

                net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_HUIYI_SHOP, _net_msg);
            }
        }
        if (message.m_type == "select_buy_huiyi_item")
        {
            protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
            _net_msg.type = 4;
            _net_msg.gezi = m_gezi + 1;
            _net_msg.num = 1;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_HUIYI_SHOP, _net_msg);
        }
        if (message.m_type == "refresh_huiyi_shop")
        {
            if (this.gameObject.activeSelf)
            {
                reset();
            }
        }
    }
    void time()
    {
      //  int hour = timer.time2dt(timer.now()).Hour;
        int hour = timer.time2dt(timer.now()).Hour;
        int lasthour = 0;
        int _hour = 0;
        if(hour >= 21 || hour < 9)
        {
            _hour = 12;
            lasthour = 21;
        }
        else if (hour >= 12 && hour < 18)
        {
            _hour = 6;
            lasthour = 12;
        }
        else if(hour >= 9 && hour < 12)
        {
            _hour = 3;
            lasthour = 9;
        }
        else if (hour >= 18 && hour < 21)
        {
            _hour = 3;
            lasthour = 18;
        }
        long _time = 0;
        if (hour >= 0 && hour < 9)
        {
            _time = _hour * 60 * 60 * 1000 - (int)(timer.now() - (ulong)timer.get_time_cuo(lasthour,-1));//6min
        }
        else
        {
            _time = _hour * 60 * 60 * 1000 - (int)(timer.now() - (ulong)timer.get_time_cuo(lasthour));//6min
        }
        if (_time > 0)
        {
            m_lost_time.GetComponent<UILabel>().text =game_data._instance.get_t_language ("huiyi_shop.cs_117_54") + " " + timer.get_time_show(_time);//刷新倒计时：
        }
 
    }
    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_HUIYI_SHOP)
        {
            protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy>(message.m_byte);
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i], true);
            }

            for (int i = 0; i < _msg.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_msg.equips[i], true);
            }
            for (int i = 0; i < _msg.types.Count; ++i)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("huiyi_shop.cs_137_113"));//回忆商店购买
            }
            sys._instance.m_self.sub_res(m_t_shop.huobi_type, m_t_shop.huobi_value,game_data._instance.get_t_language ("huiyi_shop.cs_139_83"));//回忆商店消耗


            sys._instance.m_self.m_t_player.shop4_sell[m_gezi] = 1;
            reset();
 
        }
        else if (message.m_opcode == opclient_t.CMSG_SHOP_CHECK)
        {
            if (this.gameObject.activeSelf)
            {
                protocol.game.smsg_shop_refresh _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_refresh>(message.m_byte);
                sys._instance.m_self.m_t_player.shop4_ids.Clear();
                sys._instance.m_self.m_t_player.shop4_sell.Clear();

                for (int i = 0; i < _msg.shop4_ids.Count; i++)
                {
                    sys._instance.m_self.m_t_player.shop4_ids.Add(_msg.shop4_ids[i]);
                    sys._instance.m_self.m_t_player.shop4_sell.Add(_msg.shop4_sell[i]);

                }
                sys._instance.m_self.m_t_player.huiyi_shop_last_time = timer.now();
                reset();
 
            }
           
        }

    }
   public  void reset()
    {
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        this.transform.parent.GetComponent<memory_gui>().flag = false;
        s_message mes = new s_message();
        mes.m_type = "hide_memory_scene";
        cmessage_center._instance.add_message(mes);
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        sys._instance.remove_child(m_view);
      
       // sys._instance.m_self.m_t_player.shop4_ids.Sort(compare);
        int _id = 0;
        for (int i = 0; i < sys._instance.m_self.m_t_player.shop4_ids.Count; i++)
        {
            int row = i / 2;
            int lie = i % 2;
            s_t_huiyi_shop _t_shop_xg = game_data._instance.get_t_huiyi_shop((int)sys._instance.m_self.m_t_player.shop4_ids[i]);

            GameObject temp = game_data._instance.ins_object_res("ui/temaihui_card");
            temp.transform.parent = m_view.transform;
            temp.name = i + "";
            temp.transform.localPosition = new Vector3(401 * lie - 164, -138 * row + 106, 0);
            temp.transform.localScale = new Vector3(1, 1, 1);
            temp.GetComponent<temaihui_card>().m_item_shop_gui = null;
            temp.GetComponent<temaihui_card>().m_shop_id = _t_shop_xg.id;
            temp.GetComponent<temaihui_card>().m_shell = (int)sys._instance.m_self.m_t_player.shop4_sell[i];
            temp.GetComponent<temaihui_card>().type = 14;
            temp.GetComponent<temaihui_card>().updata_ui();
            sys._instance.add_pos_anim(temp, 0.3f, new Vector3(0, 60, 0), _id * 0.05f);
            sys._instance.add_alpha_anim(temp, 0.3f, 0, 1.0f, _id * 0.05f);
            _id++;
        }
        InvokeRepeating("time", 0, 1);
 
    }
    int compare(uint x, uint y)
    {
        bool flag_x = sys._instance.m_self.is_huiyi_shop_buy((int)x);
        bool flag_y = sys._instance.m_self.is_huiyi_shop_buy((int)y);
        if (flag_x && !flag_y)
        {
            return 1;
        }
        else if (!flag_x && flag_y)
        {
            return -1;
        }
        else
        {
            return (int)(x - y);
        }
    }
    void OnDisable()
    {
        TweenAlpha _alp = this.GetComponent<TweenAlpha>();
        if (_alp != null)
        {
            Destroy(_alp);
        }
        CancelInvoke("time");
    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
}
