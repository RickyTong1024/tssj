using System.Collections.Generic;
using UnityEngine;

public class guild_shop : MonoBehaviour,IMessage {
	public GameObject shop_items;
	public GameObject m_time;
    public int m_reward_id;
	public UILabel m_juntuanshangdian;
	public UILabel m_juntuanshnagdiandesc;
	public UILabel m_wodegongxian;
    public UIToggle m_shop;
    public GameObject m_effect;
	public GameObject m_hb_power;
	public List<uint> guild_shop_ids = new List<uint>();
	public List<int> guild_shop_sell = new List<int>();
	private int m_guild_xs_id;
    private int m_guild_guanghuan_id;
	private int gezi;
	private uint m_role_power = 50110001;

	bool m_need_update = false;
	// Use this for initialization
	void Start ()
	{
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnEnable()
	{
		m_need_update = false;
		m_time.SetActive (false);
		this.InvokeRepeating ("time", 0.0f, 1.0f);
		reset ();
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}

	void time()
	{
		ulong time = (ulong)((timer.dtnow ().Hour * 60 + timer.dtnow ().Minute) * 60 + timer.dtnow ().Second) * 1000;
		long _time = 0;
		if(time <= 9*3600*1000)
		{
			_time = (long)(9*3600*1000 - time);
		}
		else if(time <= 12*3600*1000)
		{
			_time = (long)(12*3600*1000 - time);
		}
		else if(time <= 18*3600*1000)
		{
			_time = (long)(18*3600*1000 - time);
		}
		else if(time <= 21*3600*1000)
		{
			_time = (long)(21*3600*1000 - time);
		}
		else
		{
			_time = (long)((24*3600*1000 - time) + 9*3600*1000);
		}
		m_time.GetComponent<UILabel>().text =  game_data._instance.get_t_language ("guild_shop.cs_71_42") + timer.get_time_show (_time);//刷新倒计时: 
	}


    public void click(GameObject obj)
    {
        if (obj.name == "reward")
        {
            m_need_update = false;
            m_time.SetActive(false);
            reset();
        }
        else if (obj.name == "shangpin")
        {
            m_need_update = false;
            m_time.SetActive(false);
            reset_reward();
        }
        else if (obj.name == "xianshi")
        {
            m_need_update = true;
            m_time.SetActive(true);
            protocol.game.cmsg_common _net_msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_SHOP_CHECK, _net_msg);
        }
        else if (obj.name == "guanghuan")
        {
            m_need_update = false;
            m_time.SetActive(false);
            reset_guanghuan();
        }
 
    }
    int compare(int x,int y)
    {
        bool _x = guild_canbuy(x);
        bool _y = guild_canbuy(y);
        if (_x && !_y)
        {
            return 1;
        }
        else if (!_x && _y)
        {
            return -1;
        }
        else
        {
            return x - y;
        }
    }
    public static bool guild_canbuy(int id)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.guild_shop_rewards.Count; i++)
        {
            if (sys._instance.m_self.m_t_player.guild_shop_rewards[i] == id)
            {
                return true;
            }
        }
        return false;

    }
    public static bool can_shop()
    {
        for (int i = 0; i < game_data._instance.m_dbc_guild_mubiao.get_y(); ++i)
        {
            int id = int.Parse(game_data._instance.m_dbc_guild_mubiao.get(0, i));
            if (canbuy2(id) && !sys._instance.m_self.m_t_player.guild_shop_rewards.Contains(id))
            {
                return true;
            }
        }
        return false;
 
    }
	

    static bool canbuy2(int id)
    {
        s_t_guild_mubiao _mubiao = game_data._instance.get_guild_mubiao(id);
        if (_mubiao.level <= juntuan_gui._instance.m_guild_t.level && sys._instance.m_self.m_t_player.contribution >= _mubiao.price)
        {
            return true;
        }
        return false;
    }

    void reset_reward()
    {
        int k = 0;
		m_hb_power.transform.parent.gameObject.SetActive(false);
        sys._instance.remove_child(shop_items);
        shop_items.transform.localPosition = new Vector3(0, 0, 0);
        shop_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (shop_items.GetComponent<SpringPanel>() != null)
        {
            shop_items.GetComponent<SpringPanel>().enabled = false;
        }
        List<int> ids = new List<int>();
        for (int i = 0; i < game_data._instance.m_dbc_guild_mubiao.get_y(); ++i)
        {
            ids.Add(int.Parse(game_data._instance.m_dbc_guild_mubiao.get(0, i)));
        }
        ids.Sort(compare);
        for (int i = 0; i < ids.Count; ++i)
        {
			int id = ids[i];
           	s_t_guild_mubiao _shop_mubiao = game_data._instance.get_guild_mubiao(id);
            GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");

            _card.transform.parent = shop_items.transform;
            _card.GetComponent<temaihui_card>().m_shop_id = id;
            _card.GetComponent<temaihui_card>().type = 7;
            _card.GetComponent<temaihui_card>().updata_ui();
           
            if (k % 2 == 0)
            {
                _card.transform.localPosition = new Vector3(-201, 135 - k / 2 * 137, 0);
            }
            if (k % 2 == 1)
            {
                _card.transform.localPosition = new Vector3(201, 135 - k / 2 * 137, 0);
            }
            _card.transform.localScale = new Vector3(1, 1, 1);
          
            _card.SetActive(true);
            k++;
        }
        m_effect.SetActive(can_shop());
    }
    void reset_guanghuan()
    {
        int k = 0;
        m_hb_power.transform.parent.gameObject.SetActive(false);
        sys._instance.remove_child(shop_items);
        shop_items.transform.localPosition = new Vector3(0, 0, 0);
        shop_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (shop_items.GetComponent<SpringPanel>() != null)
        {
            shop_items.GetComponent<SpringPanel>().enabled = false;
        }
        List<int> ids = new List<int>();
        foreach(int id in game_data._instance.m_dbc_guild_guanghuan.m_index.Keys)
        {
            ids.Add(id);
        }
       // ids.Sort(compare);
        for (int i = 0; i < ids.Count; ++i)
        {
            int id = ids[i];
            s_t_guild_shop_ex _shop_mubiao = game_data._instance.get_guild_shop_ex(id);
            GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");

            _card.transform.parent = shop_items.transform;
            _card.GetComponent<temaihui_card>().m_shop_id = id;
            if (sys._instance.m_self.m_t_player.guild_shop_ex_rewards.Contains(id))
            {
                _card.GetComponent<temaihui_card>().m_shell = 0;

            }
            else
            {
                _card.GetComponent<temaihui_card>().m_shell = 1;

            }
            _card.GetComponent<temaihui_card>().type = 18;
            _card.GetComponent<temaihui_card>().updata_ui();

            if (k % 2 == 0)
            {
                _card.transform.localPosition = new Vector3(-201, 135 - k / 2 * 137, 0);
            }
            if (k % 2 == 1)
            {
                _card.transform.localPosition = new Vector3(201, 135 - k / 2 * 137, 0);
            }
            _card.transform.localScale = new Vector3(1, 1, 1);

            _card.SetActive(true);
            k++;
        }
    }
	void reset()
	{
		if(sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_pvp)
		{
			m_hb_power.transform.parent.gameObject.SetActive(true);
			int num = sys._instance.m_self.get_item_num(m_role_power);
			m_hb_power.transform.GetComponent<UILabel>().text = "[FF8585]"  + num.ToString();
		}
		else
		{
			m_hb_power.transform.parent.gameObject.SetActive(false);
		}
		sys._instance.remove_child (shop_items);
		shop_items.transform.localPosition = new Vector3(0, 0, 0);
		shop_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
        if (shop_items.GetComponent<SpringPanel>() != null)
        {
            shop_items.GetComponent<SpringPanel>().enabled = false;
        }
		for (int i = 0; i < game_data._instance.m_dbc_guild_shop.get_y(); i++) 
		{
			int id = int.Parse(game_data._instance.m_dbc_guild_shop.get(0,i));
			GameObject temp = game_data._instance.ins_object_res("ui/temaihui_card");

			temp.transform.parent = shop_items.transform;
			temp.GetComponent<temaihui_card>().m_shop_id = id;
			temp.GetComponent<temaihui_card>().m_shell = buy_shop_num(id);
			temp.GetComponent<temaihui_card>().type = 10;
			temp.GetComponent<temaihui_card>().updata_ui();
			temp.transform.localScale = new Vector3(1,1,1);
            if (i % 2 == 0)
            {
                temp.transform.localPosition = new Vector3(-201, 135 - i / 2 * 137, 0);
            }
            if (i % 2 == 1)
            {
                temp.transform.localPosition = new Vector3(201, 135 - i / 2 * 137, 0);
            }
		}
        m_effect.SetActive(can_shop());
	}

	public int buy_shop_num(int id)
	{
		s_t_guild_shop t_guild_shop = game_data._instance.get_guild_shop (id);
		int num = t_guild_shop.num;
		for(int i = 0; i < sys._instance.m_self.m_t_player.shop3_ids.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.shop3_ids[i] == (uint)id)
			{
				num = t_guild_shop.num - sys._instance.m_self.m_t_player.shop3_sell[i];
				break;
			}
		}
		return num;
	}

	void reset_xianshi()
	{
		int k = 0;
		m_hb_power.transform.parent.gameObject.SetActive(false);
		sys._instance.remove_child(shop_items);
		shop_items.transform.localPosition = new Vector3(0, 0, 0);
		shop_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		if (shop_items.GetComponent<SpringPanel>() != null)
		{
			shop_items.GetComponent<SpringPanel>().enabled = false;
		}
		for(int y = 0;y < 3;y ++)
		{
			for(int x = 0;x < 2;x ++)
			{				
				GameObject _card = game_data._instance.ins_object_res("ui/temaihui_card");
				
				_card.transform.parent = shop_items.transform;
				_card.transform.name = k.ToString();
				_card.transform.localPosition = new Vector3(401 * x - 200, -138 * y + 135,0);
				_card.transform.localScale = new Vector3(1,1,1);
				_card.GetComponent<temaihui_card>().m_item_shop_gui = this.gameObject;
				_card.GetComponent<temaihui_card>().m_shop_id = (int)guild_shop_ids[k];
				_card.GetComponent<temaihui_card>().m_shell = (int)can_buy_xs(guild_shop_ids[k]);
				_card.GetComponent<temaihui_card>().type = 11; 
				_card.GetComponent<temaihui_card>().updata_ui();
				_card.SetActive (true);
				
				sys._instance.add_pos_anim(_card,0.3f, new Vector3(0, 60, 0), k * 0.05f);
				sys._instance.add_alpha_anim(_card,0.3f, 0, 1.0f, k * 0.05f);
				k ++;
			}
		}

		m_effect.SetActive(can_shop());
	}

	public int can_buy_xs(uint id)
	{
		s_t_guild_shop_xs t_guild_shop_xs = game_data._instance.get_t_guild_shop_xs ((int)id);
		for(int i = 0; i < guild_shop_ids.Count;++i)
		{
			if(guild_shop_ids[i] == id)
			{
				return t_guild_shop_xs.num - guild_shop_sell[i];
			}
		}
		return 0;
	}
	
	void IMessage.message (s_message message)
	{
        if (message.m_type == "buy_guild_shop_item")
        {
            s_message _message = new s_message();
            _message.m_type = "buy_num_gui";
            _message.m_ints.Add((int)message.m_ints[0]);
            _message.m_ints.Add(5);
            _message.m_ints.Add(2);
            cmessage_center._instance.add_message(_message);
        }

        else if (message.m_type == "buy_guild_reward")
        {
            m_reward_id = (int)message.m_ints[0];
            protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
            _net_msg.item_id = m_reward_id;
            _net_msg.num = 1;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_GUILD_MUBIAO, _net_msg);
        }
        else if (message.m_type == "guild_guanghuan_item")
        {
            protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
            _net_msg.item_id = m_guild_guanghuan_id;
            _net_msg.num = 1;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_GUILD_SHOP_EX_BUY, _net_msg);
        }
        else if (message.m_type == "buy_guild_shop_guanghuan")
        {
            m_guild_guanghuan_id = (int)message.m_ints[0];
            s_t_guild_shop_ex t_guild_shop_guanghuan = game_data._instance.get_guild_shop_ex(m_guild_guanghuan_id);
            string item_num = sys._instance.m_self.get_item_num
                (t_guild_shop_guanghuan.type, t_guild_shop_guanghuan.value1, t_guild_shop_guanghuan.value2, t_guild_shop_guanghuan.value3);            
            string _des = string.Format(game_data._instance.get_t_language ("guild_shop.cs_397_40") , t_guild_shop_guanghuan.jewel ,game_data._instance.get_t_resource(10).namecolor , t_guild_shop_guanghuan.gongxian ,//是否花费[00ffff]{0}钻石[-]和{1}{2}贡献[-]购买{3}[-]?
                sys._instance.get_res_info(t_guild_shop_guanghuan.type, t_guild_shop_guanghuan.value1, t_guild_shop_guanghuan.value2, t_guild_shop_guanghuan.value3, 1)
                    );
            if (item_num != "")
            {
				_des += "\n" + game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ item_num;//当前拥有：
            }
            s_message _msg = new s_message();

            _msg.m_type = "guild_guanghuan_item";

            root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"), _des, _msg);//提示
        }
        else if (message.m_type == "refresh_guild_shop_gui")
        {
            reset();
        }
        else if (message.m_type == "buy_guild_xg_item")
        {
            m_guild_xs_id = (int)message.m_ints[0];
            gezi = (int)message.m_ints[1];
            s_t_guild_shop_xs t_guild_shop_xs = game_data._instance.get_t_guild_shop_xs(m_guild_xs_id);
            string item_num = sys._instance.m_self.get_item_num(t_guild_shop_xs.type, t_guild_shop_xs.value1, t_guild_shop_xs.value2, t_guild_shop_xs.value3);
            string _des = string.Format(game_data._instance.get_t_language ("guild_shop.cs_424_40") , t_guild_shop_xs.jewel ,//是否花费[00ffff]{0}钻石[-]购买{1}[-]?
                 sys._instance.get_res_info(t_guild_shop_xs.type, t_guild_shop_xs.value1, t_guild_shop_xs.value2, t_guild_shop_xs.value3, 1)
                   );
            if (item_num != "")
            {
				_des += "\n" + game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ item_num;//当前拥有：
            }
            s_message _msg = new s_message();

            _msg.m_type = "guild_xs_buy_item";

            root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"), _des, _msg);//提示

        }
		if(message.m_type == "guild_xs_buy_item")
		{
			protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
			_net_msg.item_id = m_guild_xs_id;
			_net_msg.gezi = gezi;
			net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_GUILD_TIME_BUY, _net_msg);
		}
	}

	void check()
	{
		bool _check = false;
		
		ulong now = timer.now();
		if (timer.trigger_time(sys._instance.m_self.guild_shop_xs_time, 21, 0))
		{
			_check = true;
		}
		else if (timer.trigger_time(sys._instance.m_self.guild_shop_xs_time, 18, 0))
		{
			_check = true;
		}
		else if (timer.trigger_time(sys._instance.m_self.guild_shop_xs_time, 12, 0))
		{
			_check = true;
		}
		else if (timer.trigger_time(sys._instance.m_self.guild_shop_xs_time, 9, 0))
		{
			_check = true;
		}
		
		if(_check)
		{
			sys._instance.m_self.guild_shop_xs_time = now;
			protocol.game.cmsg_common _net_msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SHOP_CHECK, _net_msg);
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GUILD_MUBIAO)
        {
			protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy> (message.m_byte);
			
			for(int i = 0;i < _msg.equips.Count;i ++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i],true);
			}
			for(int i = 0;i < _msg.treasures.Count;i ++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i],true);
			}
			for(int i = 0; i < _msg.types.Count;++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("guild_shop.cs_493_101"));//军团boss任务获得
			}
			s_t_guild_mubiao m_t_shop = game_data._instance.get_guild_mubiao(m_reward_id);
            sys._instance.m_self.sub_res(14, m_t_shop.price);
            sys._instance.m_self.m_t_player.guild_shop_rewards.Add(m_t_shop.id);
            reset_reward();
        }
		if(message.m_opcode == opclient_t.CMSG_SHOP_CHECK)
		{
			protocol.game.smsg_shop_refresh _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_refresh> (message.m_byte);
			
			sys._instance.m_self.m_t_player.shop1_ids.Clear();
			sys._instance.m_self.m_t_player.shop1_sell.Clear();
			
			for(int i = 0;i < _msg.shop1_ids.Count;i ++)
			{
				sys._instance.m_self.m_t_player.shop1_ids.Add(_msg.shop1_ids[i]);
				sys._instance.m_self.m_t_player.shop1_sell.Add(_msg.shop1_sell[i]);
			}

			guild_shop_ids.Clear();
			guild_shop_sell.Clear();
			
			for(int i = 0;i < _msg.guild_shop_ids.Count;i ++)
			{
				guild_shop_ids.Add(_msg.guild_shop_ids[i]);
				guild_shop_sell.Add(_msg.guild_shop_sell[i]);
			}
			reset_xianshi();
		}
        if (message.m_opcode == opclient_t.CMSG_GUILD_TIME_BUY)
        {
            protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy>(message.m_byte);

            for (int i = 0; i < _msg.equips.Count; i++)
            {
                sys._instance.m_self.add_equip(_msg.equips[i], true);
            }
            for (int i = 0; i < _msg.treasures.Count; i++)
            {
                sys._instance.m_self.add_treasure(_msg.treasures[i], true);
            }
            for (int i = 0; i < _msg.roles.Count; ++i)
            {
                sys._instance.m_self.add_card(_msg.roles[i], true);
            }
            for (int i = 0; i < _msg.types.Count; ++i)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("guild_shop.cs_541_113"));//军团商店购买获得
            }
            s_t_guild_shop_xs t_guild_shop_xs = game_data._instance.get_t_guild_shop_xs(m_guild_xs_id);
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, t_guild_shop_xs.jewel,game_data._instance.get_t_language ("equip_buy_gui.cs_227_102"));//军团商店购买消耗
            sys._instance.m_self.m_t_player.shop1_ids.Add((uint)m_guild_xs_id);
            sys._instance.m_self.m_t_player.shop1_sell.Add(1);
            bool flag = false;
            for (int i = 0; i < guild_shop_ids.Count; ++i)
            {
                if (guild_shop_ids[i] == (uint)m_guild_xs_id)
                {
                    flag = true;
                    guild_shop_sell[i] += 1;
                    break;
                }
            }
            if (!flag)
            {
                guild_shop_ids.Add((uint)m_guild_xs_id);
                guild_shop_sell.Add(1);
            }
            reset_xianshi();
        }
        else if (message.m_opcode == opclient_t.CMSG_GUILD_SHOP_EX_BUY)
        {
            sys._instance.m_self.m_t_player.guild_shop_ex_rewards.Add(m_guild_guanghuan_id);
            protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy>(message.m_byte);
            for (int i = 0; i < _msg.types.Count; ++i)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("equip_buy_gui.cs_221_101"));//军团商店购买
            }
            s_t_guild_shop_ex _ex = game_data._instance.get_guild_shop_ex(m_guild_guanghuan_id);
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, _ex.jewel, game_data._instance.get_t_language ("equip_buy_gui.cs_227_102"));//军团商店购买消耗
            sys._instance.m_self.sub_att(e_player_attr.player_contribution, _ex.gongxian);
            reset_guanghuan();
        }
	}

	void Update () {
		if(m_need_update)
		{
			m_need_update = false;
			check();
		}
		
	}
}
