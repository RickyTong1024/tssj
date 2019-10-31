
using UnityEngine;
using System.Collections;

public class mofang_shop_gui : MonoBehaviour,IMessage{
    public GameObject m_view;
    public int m_buy_num;
    public int m_shop_id;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	
	}
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    public void click(GameObject obj)
    {
 

    }
    void IMessage.message(s_message mes)
    {
        if(mes.m_type == "buy_mofang_shop_item")
        {
            s_message _message = new s_message();
            _message.m_type = "buy_num_gui";
            _message.m_ints.Add((int)mes.m_ints[0]);
            _message.m_ints.Add(21);
            _message.m_ints.Add(buy_shop_num((int)mes.m_ints[0]));
            cmessage_center._instance.add_message(_message);
            m_shop_id = (int)mes.m_ints[0];
        }
        else if (mes.m_type == "mofang_shop_buy")
        {
            protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
            _net_msg.item_id = m_shop_id;
            m_buy_num = (int)mes.m_ints[0];
            _net_msg.num = m_buy_num;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_MOFANG_BUY, _net_msg);
 
        }
        else if (mes.m_type == "daily_refresh")
        {
            reset();
        }
    }
    void IMessage.net_message(s_net_message msg)
    {
        if (msg.m_opcode == opclient_t.CMSG_MOFANG_BUY)
        {
            s_t_mofang_shop shop = game_data._instance.get_t_mofang_shop(m_shop_id);
            sys._instance.m_self.sub_att(e_player_attr.player_mofang_jifen, m_buy_num * shop.price);
            sys._instance.m_self.add_reward(shop.type,shop.value1,shop.value2 *m_buy_num,shop.value3 ,game_data._instance.get_t_language ("mofang_shop_gui.cs_54_102"));//魔方商店购买
            if (shop.buycount != 0)
            {
                bool flag = false;
                for (int i = 0; i < mofang_gui._instance.m_view_data.shop_ids.Count; i++)
                {
                    if (m_shop_id == mofang_gui._instance.m_view_data.shop_ids[i])
                    {
                        mofang_gui._instance.m_view_data.shop_nums[i] += m_buy_num;
                        flag = true;
                    }


                }
                if (!flag)
                {
                    mofang_gui._instance.m_view_data.shop_ids.Add(m_shop_id);
                    mofang_gui._instance.m_view_data.shop_nums.Add(m_buy_num);
 
                }
 
            }
            
            reset();
        }
 
    }
    void OnEnable()
    {
        reset();
    }
    void reset()
    {
        sys._instance.remove_child(m_view);
        m_view.transform.localPosition = new Vector3(0, 0, 0);
        m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (m_view.GetComponent<SpringPanel>() != null)
        {
            m_view.GetComponent<SpringPanel>().enabled = false;
        }
        int i = 0;
        foreach(int id in game_data._instance.m_mofang_shops.Keys)
        {
            GameObject temp = game_data._instance.ins_object_res("ui/temaihui_card");
            temp.transform.parent = m_view.transform;
            temp.GetComponent<temaihui_card>().m_shop_id = id;
            temp.GetComponent<temaihui_card>().m_shell = buy_shop_num(id);
            temp.GetComponent<temaihui_card>().type = 21;
            temp.GetComponent<temaihui_card>().updata_ui();
            temp.transform.localScale = new Vector3(1, 1, 1);
            if (i % 2 == 0)
            {
                temp.transform.localPosition = new Vector3(-201, 154 - i / 2 * 137, 0);
            }
            if (i % 2 == 1)
            {
                temp.transform.localPosition = new Vector3(201, 154 - i / 2 * 137, 0);
            }
            i++;
        }
    }
    public int buy_shop_num(int id)
    {
        s_t_mofang_shop t_shop = game_data._instance.get_t_mofang_shop(id);
        int num = t_shop.buycount;
        for (int i = 0; i < mofang_gui._instance.m_view_data.shop_ids .Count; ++i)
        {
            if (mofang_gui._instance.m_view_data.shop_ids[i] == (uint)id)
            {
                num = t_shop.buycount - mofang_gui._instance.m_view_data.shop_nums[i];
                break;
            }
        }
        return num;
    }

	
}
