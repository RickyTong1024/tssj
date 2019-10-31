
using UnityEngine;
using System.Collections;

public class equip_buy_gui : MonoBehaviour,IMessage {

	public int m_id;
	public int shop_id;
	public GameObject m_zprice;
	public GameObject m_icon;
	public GameObject m_name;
	public GameObject m_input;
	public GameObject m_item_num;
	public GameObject m_currentjewel;
	public GameObject m_money_image;
	public GameObject m_power_image;
	public GameObject m_zpower;
	public GameObject jr_num;
	public int m_type = 0;
	public int shop_num = 0;
	int total_num  = 0;
	private s_t_item m_t_item;
	s_t_shop_xg _t_shop_xg;
	private s_t_sport_shop m_t_sport_shop;
	private s_t_boss_shop m_t_boss_shop;
    private s_t_guild_shop m_t_guild_shop;
    private s_t_mofang_shop m_t_mofang_shop;
    private s_t_bingyuan_shop m_t_bingyuan_shop;
	private s_t_tanbao_shop m_t_tanbao_shop;
    private s_t_pvp_shop m_t_pvp_shop;
	private s_t_ttt_shop m_t_ttt_shop;
    private s_t_huiyi_luckshop m_luck_shop;
	public s_t_itemhecheng t_itemhecheng;
	private s_t_chongzhifanpai_shop m_t_chongzhifanpai_shop;

	private uint m_role_power = 50110001;
	private int m_input_num = 1;
	private int m_input_price = 10;

	// Use this for initialization
	void Start ()
	{
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.net_message(s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_ITEM_DIRECT_BUY)
		{
			sys._instance.m_self.sub_att(e_player_attr.player_jewel,m_input_price);	
			sys._instance.m_self.add_item((uint)m_id,m_input_num,"");
			s_message _mes = new s_message();
			_mes.m_type = "refresh_gaizhaoshi";
			cmessage_center._instance.add_message(_mes);

		}

		if(message.m_opcode == opclient_t.CMSG_SHOP_XG && this.gameObject.activeSelf)
		{
			protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy> (message.m_byte);
			for(int i = 0; i < _msg.types.Count;++i)
			{
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i], game_data._instance.get_t_language ("equip_buy_gui.cs_65_114"));//主界面商城获得
			}
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, m_input_price,game_data._instance.get_t_language ("equip_buy_gui.cs_67_74"));//主界面商城消耗
			if(_t_shop_xg.xg_num != 0)
			{
				bool flag = false;
				for(int j = 0 ;j < sys._instance.m_self.m_t_player.shop_xg_ids.Count ; ++j)
				{
					if(sys._instance.m_self.m_t_player.shop_xg_ids[j] == _t_shop_xg.id)
					{
						sys._instance.m_self.m_t_player.shop_xg_nums[j] = sys._instance.m_self.m_t_player.shop_xg_nums[j] + m_input_num;
						flag = true;
					}
				}
				if(!flag)
				{
					sys._instance.m_self.m_t_player.shop_xg_ids.Add (_t_shop_xg.id);
					sys._instance.m_self.m_t_player.shop_xg_nums.Add(m_input_num);
				}
			}
			for(int i = 0;i < _msg.equips.Count;i ++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i],true);
			}
			for(int i = 0;i < _msg.treasures.Count;i ++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i],true);
			}
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			sys._instance.m_self.m_t_player.hs_task_num += m_input_num;//shnagcheng
			if(_t_shop_xg.id == 100200)
			{
				sys._instance.m_self.add_active(1900, m_input_num);
			}
			if(_t_shop_xg.id == 100300)
			{
				sys._instance.m_self.add_active(2000, m_input_num);
			}
			s_message _message = new s_message();
			_message.m_type = "refresh_item_shop_gui";
			cmessage_center._instance.add_message(_message);

			s_message _message1 = new s_message();
			_message1.m_type = "update_cn_gui";
			cmessage_center._instance.add_message(_message1);

            s_message _message2 = new s_message();
            _message2.m_type = "refresh_gaizhaoshi";

            s_message _message5 = new s_message();
            _message5.m_type = "refresh_select_num";
            cmessage_center._instance.add_message(_message5);
            cmessage_center._instance.add_message(_message2);


			sys._instance.m_self.check_target_done();
		}
		if(message.m_opcode == opclient_t.CMSG_SPORT_SHOP_BUY)
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
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("arena.cs_345_76"));//竞技场商店购买
			}
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			sys._instance.m_self.sub_att(e_player_attr.player_treasure_powder,m_input_price);
			if(m_t_sport_shop.hb_power > 0)
			{
                sys._instance.m_self.remove_item(m_role_power, m_t_sport_shop.hb_power * m_input_num, game_data._instance.get_t_language ("equip_buy_gui.cs_138_102"));//竞技场商店购买消耗
			}
			s_message _message = new s_message();
			_message.m_type = "refresh_honor_shop_gui";
			cmessage_center._instance.add_message(_message);
		}

		if(message.m_opcode == opclient_t.CMSG_BOSS_SHOP_BUY)
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
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("equip_buy_gui.cs_159_101"));//魔王商店购买
			}
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			sys._instance.m_self.sub_att(e_player_attr.player_medal_point,m_input_price);
			if(m_t_boss_shop.hb_power > 0)
			{
                sys._instance.m_self.remove_item(m_role_power, m_t_boss_shop.hb_power * m_input_num, game_data._instance.get_t_language ("equip_buy_gui.cs_165_101"));//魔王商店购买消耗
			}
			s_message _message = new s_message();
			_message.m_type = "refresh_mowang_shop_gui";
			cmessage_center._instance.add_message(_message);
		}
        else if (message.m_opcode == opclient_t.CMSG_LIEREN_SHOP)
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
            for (int i = 0; i < _msg.types.Count; ++i)
            {
                sys._instance.m_self.add_reward(_msg.types[i], _msg.value1s[i], _msg.value2s[i], _msg.value3s[i],game_data._instance.get_t_language ("equip_buy_gui.cs_185_113"));//猎人商店购买
            }
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            if (m_t_pvp_shop.liebi > 0)
            {
                sys._instance.m_self.sub_att(e_player_attr.player_pvp_jz, m_input_price);
            }
            if (m_t_pvp_shop.redequippower > 0)
            {
                sys._instance.m_self.remove_item(50120001, m_input_price, game_data._instance.get_t_language ("equip_buy_gui.cs_194_74"));//猎人商店购买消耗
            }
            if (m_t_pvp_shop.redrolepower > 0)
            {
                sys._instance.m_self.remove_item(50110001, m_input_price, game_data._instance.get_t_language ("equip_buy_gui.cs_194_74"));//猎人商店购买消耗
            }
          //  sys._instance.m_self.m_t_player.hs_task_num++;
         //   sys._instance.m_self.check_target_done();
            s_message _message = new s_message();
            _message.m_type = "refresh_pvp_shop_gui";
            cmessage_center._instance.add_message(_message);
 
        }
        if (message.m_opcode == opclient_t.CMSG_GUILD_SHOP_BUY)
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
			for(int i = 0; i < _msg.types.Count;++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("equip_buy_gui.cs_221_101"));//军团商店购买
			}
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            sys._instance.m_self.sub_att(e_player_attr.player_contribution,m_input_price);
			if(m_t_guild_shop.hb_power > 0)
			{
                sys._instance.m_self.remove_item(m_role_power, m_t_guild_shop.hb_power * m_input_num, game_data._instance.get_t_language ("equip_buy_gui.cs_227_102"));//军团商店购买消耗
			}
			bool flag = false;
			for(int i = 0; i < sys._instance.m_self.m_t_player.shop3_ids.Count;++i)
			{
				if(sys._instance.m_self.m_t_player.shop3_ids[i] == (uint)m_t_guild_shop.id)
				{
					flag = true;
					sys._instance.m_self.m_t_player.shop3_sell[i] += m_input_num;
					break;
				}
			}
			if(!flag)
			{
				sys._instance.m_self.m_t_player.shop3_ids.Add((uint)m_t_guild_shop.id);
				sys._instance.m_self.m_t_player.shop3_sell.Add(m_input_num);
			}
            s_message _message = new s_message();
            _message.m_type = "refresh_guild_shop_gui";
            cmessage_center._instance.add_message(_message);
        }
		if(message.m_opcode == opclient_t.CMSG_TTT_SHOP_BUY)
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
			sys._instance.m_self.add_reward(m_t_ttt_shop.type, m_t_ttt_shop.value1, m_t_ttt_shop.value2*m_input_num, m_t_ttt_shop.value3,game_data._instance.get_t_language ("equip_buy_gui.cs_261_128"));//秘境商店购买
			sys._instance.m_self.sub_res(6, m_input_price);
			this.transform.Find("frame_big").GetComponent<frame>().hide();

			s_message _message = new s_message();
			_message.m_type = "refresh_mj_shop_gui";
			cmessage_center._instance.add_message(_message);
		}
        if (message.m_opcode == opclient_t.CMSG_BY_SHOP_BUY)
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
            bool flag = false;
            for (int i = 0; i < sys._instance.m_self.m_t_player.by_shops.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.by_shops[i] == (uint)m_t_bingyuan_shop.id)
                {
                    flag = true;
                    sys._instance.m_self.m_t_player.by_nums[i] += m_input_num;
                    break;
                }
            }
            if (!flag)
            {
                sys._instance.m_self.m_t_player.by_shops.Add(m_t_bingyuan_shop.id);
                sys._instance.m_self.m_t_player.by_nums.Add(m_input_num);
            }
            sys._instance.m_self.add_reward(m_t_bingyuan_shop.type, m_t_bingyuan_shop.value1, m_t_bingyuan_shop.value2 * m_input_num, m_t_bingyuan_shop.value3,game_data._instance.get_t_language ("bingyuan_shop.cs_91_113"));//冰原商店购买
            sys._instance.m_self.sub_res(24, m_input_price);
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            s_message _message = new s_message();
            _message.m_type = "refresh_bingyuan_shop_gui";
            cmessage_center._instance.add_message(_message);
        }
		if(message.m_opcode == opclient_t.CMSG_ITEM_HECHENG && t_itemhecheng != null)
		{
			protocol.game.smsg_success _msg = net_http._instance.parse_packet<protocol.game.smsg_success> (message.m_byte);
			if(_msg.success)
			{
				sys._instance.m_self.add_reward(t_itemhecheng.type,t_itemhecheng.item_id,t_itemhecheng.item_num*m_input_num,0,game_data._instance.get_t_language ("equip_buy_gui.cs_309_114"));//物品合成获得
				for(int i =0 ; i < t_itemhecheng.cl_type.Count;++i)
				{
                    sys._instance.m_self.remove_item((uint)t_itemhecheng.cl_id[i], t_itemhecheng.cl_num[i] * m_input_num, game_data._instance.get_t_language ("bag_gui.cs_427_92"));//物品合成消耗
				}
				this.transform.Find("frame_big").GetComponent<frame>().hide();
				s_message _message = new s_message();
				_message.m_type = "refresh_bag_duihuan_gui";
				cmessage_center._instance.add_message(_message);
				t_itemhecheng = null;
			}
		}
        else if (message.m_opcode == opclient_t.CMSG_HUIYI_LUCK_SHOP)
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
            sys._instance.m_self.add_reward(m_luck_shop.reward.type, m_luck_shop.reward.value1, m_luck_shop.reward.value2 * m_input_num, m_luck_shop.reward.value3,game_data._instance.get_t_language ("equip_buy_gui.cs_333_163"));//回忆幸运商店购买
            sys._instance.m_self.sub_att(e_player_attr.player_luck_point, m_input_price);
            this.transform.Find("frame_big").GetComponent<frame>().hide();
            bool flag = false;
            for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_shop_ids.Count; i++)
            {
                if (m_luck_shop.id == sys._instance.m_self.m_t_player.huiyi_shop_ids[i])
                {
                    sys._instance.m_self.m_t_player.huiyi_shop_nums[i] += m_input_num;
                    flag = true;
                }
            }
            if (!flag)
            {
                sys._instance.m_self.m_t_player.huiyi_shop_ids.Add(m_luck_shop.id);
                sys._instance.m_self.m_t_player.huiyi_shop_nums.Add(m_input_num);
            }

            s_message _message = new s_message();
            _message.m_type = "refresh_luck_shop_gui";
            cmessage_center._instance.add_message(_message);
 
        }

	}
	void IMessage.message(s_message message)
	{

	}
	public void updata_ui()
	{
		total_num = 0;
        if (m_type == 1)
        {
            jr_num.transform.parent.gameObject.SetActive(false);
            total_num = 1000;
            m_t_item = game_data._instance.get_item(m_id);
            GameObject _icon = icon_manager._instance.create_item_icon(m_id);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_name.GetComponent<UILabel>().text = m_t_item.name;
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_id).ToString();//当前拥有：
            m_input_num = 1;
            m_input_price = m_t_item.jewel;
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(2);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
        }
        else if (m_type == 2)
        {
            jr_num.transform.parent.gameObject.SetActive(true);
            jr_num.transform.parent.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_386_67");//今日还可以购买:
            _t_shop_xg = game_data._instance.get_shop_xg(m_id);
            int xg_num = 0;
            for (int i = 0; i < sys._instance.m_self.m_t_player.shop_xg_ids.Count; ++i)
            {
                if (_t_shop_xg.id == sys._instance.m_self.m_t_player.shop_xg_ids[i])
                {
                    xg_num = sys._instance.m_self.m_t_player.shop_xg_nums[i];
                    break;
                }
            }
            if (_t_shop_xg.xg_type == 2)
            {
                total_num = _t_shop_xg.vip_type[sys._instance.m_self.m_t_player.vip] - xg_num;
            }
            if (_t_shop_xg.xg_type == 1)
            {
                total_num = _t_shop_xg.vip_type[0] - xg_num;
            }
            if (_t_shop_xg.xg_type == 0)
            {
                total_num = 100;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(_t_shop_xg.type, _t_shop_xg.vlaue1, _t_shop_xg.vlaue2, _t_shop_xg.vlaue3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(_t_shop_xg.type, _t_shop_xg.vlaue1, _t_shop_xg.vlaue2, _t_shop_xg.vlaue3);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)_t_shop_xg.vlaue1);//当前拥有：
            if (m_id == 100100)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_att(e_player_attr.player_yuanli);//当前拥有：
            }
            if (_t_shop_xg.type == 6)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_treasure_num(_t_shop_xg.vlaue1);//当前拥有：
            }
            m_input_num = 1;
            if (total_num == 0)
            {
                m_input_num = 0;
            }
            m_input_price = sys._instance.m_self.get_xg_shop_price(m_id, m_input_num);
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),total_num.ToString());//{0}次
            if (_t_shop_xg.xg_type == 0)
            {
                jr_num.transform.parent.gameObject.SetActive(false);
            }
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(2);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
        }
        else if (m_type == 3)
        {
            jr_num.transform.parent.gameObject.SetActive(false);
            m_t_sport_shop = game_data._instance.get_t_sport_shop(m_id);
            total_num = sys._instance.m_self.get_att(e_player_attr.player_treasure_powder) / m_t_sport_shop.price;
            if (m_t_sport_shop.hb_power > 0)
            {
                if (total_num > sys._instance.m_self.get_item_num(m_role_power) / m_t_sport_shop.hb_power)
                {
                    total_num = sys._instance.m_self.get_item_num(m_role_power) / m_t_sport_shop.hb_power;
                }
            }
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_sport_shop.type, m_t_sport_shop.value1, m_t_sport_shop.value2, m_t_sport_shop.value3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_sport_shop.value1);//当前拥有：
            if (m_t_sport_shop.type == 6)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_treasure_num(m_t_sport_shop.value1);//当前拥有：
            }
            m_input_num = 1;
            m_input_price = m_t_sport_shop.price;
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),total_num.ToString() );//{0}次
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(14);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_sport_shop.type, m_t_sport_shop.value1, m_t_sport_shop.value2, m_t_sport_shop.value3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
            if (m_t_sport_shop.hb_power > 0)
            {
                m_power_image.SetActive(true);
                m_zpower.SetActive(true);
            }
        }
        else if (m_type == 4)
        {
            jr_num.transform.parent.gameObject.SetActive(false);
            m_t_boss_shop = game_data._instance.get_t_boss_shop(m_id);
            total_num = sys._instance.m_self.get_att(e_player_attr.player_medal_point) / m_t_boss_shop.price;
            if (m_t_boss_shop.hb_power > 0)
            {
                if (total_num > sys._instance.m_self.get_item_num(m_role_power) / m_t_boss_shop.hb_power)
                {
                    total_num = sys._instance.m_self.get_item_num(m_role_power) / m_t_boss_shop.hb_power;
                }
            }
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_boss_shop.type, m_t_boss_shop.value1, m_t_boss_shop.value2, m_t_boss_shop.value3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_boss_shop.value1);//当前拥有：
            if (m_t_boss_shop.type == 6)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_treasure_num(m_t_boss_shop.value1);//当前拥有：
            }
            m_input_num = 1;
            m_input_price = m_t_boss_shop.price;
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),total_num.ToString() );//{0}次
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(13);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_boss_shop.type, m_t_boss_shop.value1, m_t_boss_shop.value2, m_t_boss_shop.value3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
            if (m_t_boss_shop.hb_power > 0)
            {
                m_power_image.SetActive(true);
                m_zpower.SetActive(true);
            }
        }
        else if (m_type == 5)
        {
            jr_num.transform.parent.gameObject.SetActive(true);
            jr_num.transform.parent.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_386_67");//今日还可以购买:
            m_t_guild_shop = game_data._instance.get_guild_shop(m_id);
            total_num = sys._instance.m_self.get_att(e_player_attr.player_contribution) / m_t_guild_shop.gx;
            int num = 0;
            for (int i = 0; i < sys._instance.m_self.m_t_player.shop3_ids.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.shop3_ids[i] == (uint)m_t_guild_shop.id)
                {
                    num = sys._instance.m_self.m_t_player.shop3_sell[i];
                    break;
                }
            }
            if (total_num >= m_t_guild_shop.num - num)
            {
                total_num = m_t_guild_shop.num - num;
            }
            if (m_t_guild_shop.hb_power > 0)
            {
                if (total_num > sys._instance.m_self.get_item_num(m_role_power) / m_t_guild_shop.hb_power)
                {
                    total_num = sys._instance.m_self.get_item_num(m_role_power) / m_t_guild_shop.hb_power;
                }
            }
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_guild_shop.reward.type, m_t_guild_shop.reward.value1, m_t_guild_shop.reward.value2, m_t_guild_shop.reward.value3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_guild_shop.reward.value1);//当前拥有：
            if (m_t_guild_shop.reward.type == 6)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_treasure_num(m_t_guild_shop.reward.value1);//当前拥有：
            }
            m_input_num = 1;
            m_input_price = m_t_guild_shop.gx;
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(10);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_guild_shop.reward.type, m_t_guild_shop.reward.value1, m_t_guild_shop.reward.value2, m_t_guild_shop.reward.value3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
            if (m_t_guild_shop.hb_power > 0)
            {
                m_power_image.SetActive(true);
                m_zpower.SetActive(true);
            }
        }
        else if (m_type == 21)
        {
            jr_num.transform.parent.gameObject.SetActive(true);
            m_t_mofang_shop = game_data._instance.get_t_mofang_shop(m_id);
            if (m_t_mofang_shop.buycount == 0)
            {
                total_num = sys._instance.m_self.get_att(e_player_attr.player_mofang_jifen) / m_t_mofang_shop.price;
                jr_num.transform.parent.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_577_71");//可以购买:

                jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次

            }
            else
            {
                total_num = shop_num;
                jr_num.transform.parent.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_386_67");//今日还可以购买:

                jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
            }
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
           

            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_mofang_shop.type, m_t_mofang_shop.value1, m_t_mofang_shop.value2, m_t_mofang_shop.value3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            if (m_t_mofang_shop.type == 1)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.value_to_wan(sys._instance.m_self.get_res_num(m_t_mofang_shop.value1));//当前拥有：
            }
            else
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_mofang_shop.value1);//当前拥有：
            }
            m_input_num = 1;
            m_input_price = m_t_mofang_shop.price;
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(28);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_mofang_shop.type, m_t_mofang_shop.value1, m_t_mofang_shop.value2, m_t_mofang_shop.value3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
 
        }
        else if (m_type == 16)
        {
            jr_num.transform.parent.gameObject.SetActive(true);
            jr_num.transform.parent.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_386_67");//今日还可以购买:
            m_t_bingyuan_shop = game_data._instance.get_t_bingyuan_shop(m_id);
            total_num = sys._instance.m_self.get_att(e_player_attr.player_bingyuan_point) / m_t_bingyuan_shop.binjin;
            int num = 0;
            for (int i = 0; i < sys._instance.m_self.m_t_player.by_shops.Count; ++i)
            {
                if (sys._instance.m_self.m_t_player.by_shops[i] == (uint)m_t_bingyuan_shop.id)
                {
                    num = sys._instance.m_self.m_t_player.by_nums[i];
                    break;
                }
            }
            if (total_num >= m_t_bingyuan_shop.buy_count - num)
            {
                total_num = m_t_bingyuan_shop.buy_count - num;
            }
            if (total_num >= 1000 || m_t_bingyuan_shop.buy_count == 0)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_bingyuan_shop.type, m_t_bingyuan_shop.value1, m_t_bingyuan_shop.value2, m_t_bingyuan_shop.value3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_bingyuan_shop.value1);//当前拥有：
            if (m_t_bingyuan_shop.type == 6)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_treasure_num(m_t_bingyuan_shop.value1);//当前拥有：
            }
            if (m_t_bingyuan_shop.type == 1)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " +//当前拥有：
                    sys._instance.value_to_wan(sys._instance.m_self.get_att((e_player_attr)m_t_bingyuan_shop.value1));
            }
            m_input_num = 1;
            m_input_price = m_t_bingyuan_shop.binjin;
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(24);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_bingyuan_shop.type, m_t_bingyuan_shop.value1, m_t_bingyuan_shop.value2, m_t_bingyuan_shop.value3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
        }
        else if (m_type == 12)
        {
            jr_num.transform.parent.gameObject.SetActive(false);
            m_t_pvp_shop = game_data._instance.get_t_pvp_shop(m_id);
            if (m_t_pvp_shop.liebi > 0)
            {
                total_num = sys._instance.m_self.get_att(e_player_attr.player_pvp_jz) / m_t_pvp_shop.liebi;
                m_input_price = m_t_pvp_shop.liebi;
                m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(22);
            }
            else if (m_t_pvp_shop.redequippower > 0)
            {
                total_num = sys._instance.m_self.get_item_num(50120001) / m_t_pvp_shop.redequippower;
                m_input_price = m_t_pvp_shop.redequippower;
                m_money_image.GetComponent<UISprite>().spriteName = "hszbzlx_icon";
            }
            else
            {
                total_num = sys._instance.m_self.get_item_num(50110001) / m_t_pvp_shop.redrolepower;
                m_input_price = m_t_pvp_shop.redrolepower;
                m_money_image.GetComponent<UISprite>().spriteName = "hshbzlx_icon";
            }
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_pvp_shop.type, m_t_pvp_shop.value1, m_t_pvp_shop.value2, m_t_pvp_shop.value3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_pvp_shop.value1);//当前拥有：
            if (m_t_pvp_shop.type == 6)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_treasure_num(m_t_pvp_shop.value1);//当前拥有：
            }
            m_input_num = 1;

            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次

            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_pvp_shop.type, m_t_pvp_shop.value1, m_t_pvp_shop.value2, m_t_pvp_shop.value3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);

        }
        else if (m_type == 6)
        {
            jr_num.transform.parent.gameObject.SetActive(false);
            m_t_ttt_shop = game_data._instance.get_t_ttt_shop(m_id);
            total_num = sys._instance.m_self.get_att(e_player_attr.player_hj) / m_t_ttt_shop.price;
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_ttt_shop.type, m_t_ttt_shop.value1, m_t_ttt_shop.value2, m_t_ttt_shop.value3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_ttt_shop.value1);//当前拥有：
            if (m_t_ttt_shop.type == 6)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_treasure_num(m_t_ttt_shop.value1);//当前拥有：
            }
            m_input_num = 1;
            m_input_price = m_t_ttt_shop.price;
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(6);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_ttt_shop.type, m_t_ttt_shop.value1, m_t_ttt_shop.value2, m_t_ttt_shop.value3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
        }
        else if (m_type == 7)
        {
            jr_num.transform.parent.gameObject.SetActive(true);
            int value = 1000;
            for (int i = 0; i < t_itemhecheng.cl_type.Count; ++i)
            {
                int num = sys._instance.m_self.get_item_num((uint)t_itemhecheng.cl_id[i]) / t_itemhecheng.cl_num[i];
                if (num < value)
                {
                    value = num;
                }
            }
            total_num = value;
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_item_icon(t_itemhecheng.item_id);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)t_itemhecheng.item_id);//当前拥有：
            m_input_num = 1;
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
            jr_num.transform.parent.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_759_67");//可合成:
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(t_itemhecheng.type, t_itemhecheng.item_id, 1, 0);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
        }
        else if (m_type == 13)
        {
            jr_num.transform.parent.gameObject.SetActive(false);

            m_luck_shop = game_data._instance.get_t_huiyi_luckshop(m_id);
            total_num = m_luck_shop.day_num - luck_shop(m_id);
            GameObject _icon = icon_manager._instance.create_item_icon(m_luck_shop.reward.value1);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            m_name.GetComponent<UILabel>().text = sys._instance.get_res_info(m_luck_shop.reward.type, m_luck_shop.reward.value1, m_luck_shop.reward.value2, m_luck_shop.reward.value3);
            m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_luck_shop.reward.value1).ToString();//当前拥有：
            m_input_num = 1;
            m_input_price = m_luck_shop.luck_point;
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(20);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);

        }
        else if (m_type == 15)
        {
            jr_num.transform.parent.gameObject.SetActive(true);
            jr_num.transform.parent.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_386_67");//今日还可以购买:
            m_t_tanbao_shop = game_data._instance.get_t_tanbao_shop(m_id);
            total_num = shop_num;
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_tanbao_shop.rtype, m_t_tanbao_shop.rvalue1, m_t_tanbao_shop.rvalue2, m_t_tanbao_shop.rvalue3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            if (m_t_tanbao_shop.rtype == 1)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.value_to_wan(sys._instance.m_self.get_res_num(m_t_tanbao_shop.rvalue1));//当前拥有：
            }
            else
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_tanbao_shop.rvalue1);//当前拥有：
            }
            m_input_num = 1;
            m_input_price = m_t_tanbao_shop.price;
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(2);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_tanbao_shop.rtype, m_t_tanbao_shop.rvalue1, m_t_tanbao_shop.rvalue2, m_t_tanbao_shop.rvalue3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
        }
        else if (m_type == 17)
        {

        }
        else if (m_type == 19)
        {
            jr_num.transform.parent.gameObject.SetActive(true);
            jr_num.transform.parent.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_577_71");//可以购买:
            m_t_chongzhifanpai_shop = game_data._instance.get_t_chongzhifanpai_shop(m_id);
            total_num = shop_num;
            if (total_num >= 1000)
            {
                total_num = 1000;
            }
            GameObject _icon = icon_manager._instance.create_reward_icon(m_t_chongzhifanpai_shop.type, m_t_chongzhifanpai_shop.value1, m_t_chongzhifanpai_shop.value2, m_t_chongzhifanpai_shop.value3);
            sys._instance.remove_child(m_icon);
            _icon.transform.parent = m_icon.transform;
            _icon.transform.localScale = new Vector3(1, 1, 1);
            _icon.transform.localPosition = new Vector3(0, 0, 0);
            if (m_t_chongzhifanpai_shop.type == 1)
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.value_to_wan(sys._instance.m_self.get_res_num(m_t_chongzhifanpai_shop.value1));//当前拥有：
            }
            else
            {
                m_item_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") + " " + sys._instance.m_self.get_item_num((uint)m_t_chongzhifanpai_shop.value1);//当前拥有：
            }
            m_input_num = 1;
            m_input_price = m_t_chongzhifanpai_shop.price;
            jr_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
            m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(26);
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_chongzhifanpai_shop.type, m_t_chongzhifanpai_shop.value1, m_t_chongzhifanpai_shop.value2, m_t_chongzhifanpai_shop.value3);
            m_power_image.SetActive(false);
            m_zpower.SetActive(false);
        }
		reset();

	}
    int luck_shop(int id)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.huiyi_shop_ids.Count; i++)
        {
            if (id == sys._instance.m_self.m_t_player.huiyi_shop_ids[i])
            {
                return sys._instance.m_self.m_t_player.huiyi_shop_nums[i];

            }
        }
        return 0;
    }
	public void reset()
	{
		m_zprice.SetActive(true);
		m_money_image.SetActive(true);
        if (m_type == 1)
        {
            m_input_price = m_input_num * m_t_item.jewel;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + m_input_price + "";
        }
        else if (m_type == 13)
        {
            m_input_price = m_input_num * m_luck_shop.luck_point;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(20) + m_input_price + "";

        }
        else if (m_type == 2)
        {
            m_input_price = sys._instance.m_self.get_xg_shop_price(m_id, m_input_num);
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + m_input_price + "";
            if (m_input_price > sys._instance.m_self.m_t_player.jewel)
            {
                m_zprice.GetComponent<UILabel>().text = "[ff0000]" + m_input_price + "";
            }
        }
        else if (m_type == 3)
        {
            m_input_price = m_t_sport_shop.price * m_input_num;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(14) + m_input_price + "";
            if (m_t_sport_shop.hb_power > 0)
            {
                m_zpower.GetComponent<UILabel>().text = (m_t_sport_shop.hb_power * m_input_num).ToString();
            }
        }
        else if (m_type == 4)
        {
            m_input_price = m_t_boss_shop.price * m_input_num;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(13) + m_input_price + "";
            if (m_t_boss_shop.hb_power > 0)
            {
                m_zpower.GetComponent<UILabel>().text = (m_t_boss_shop.hb_power * m_input_num).ToString();
            }
        }
        else if (m_type == 5)
        {
            m_input_price = m_t_guild_shop.gx * m_input_num;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(10) + m_input_price + "";
            if (m_t_guild_shop.hb_power > 0)
            {
                m_zpower.GetComponent<UILabel>().text = (m_t_guild_shop.hb_power * m_input_num).ToString();
            }
        }
        else if (m_type == 6)
        {
            m_input_price = m_t_ttt_shop.price * m_input_num;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(6) + m_input_price + "";
        }
        else if (m_type == 15)
        {
            m_input_price = m_t_tanbao_shop.price * m_input_num;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(2) + m_input_price + "";
        }
        else if (m_type == 16)
        {
            m_input_price = m_t_bingyuan_shop.binjin * m_input_num;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(24) + m_input_price + "";
        }
        else if (m_type == 7)
        {
            m_zprice.SetActive(false);
            m_money_image.SetActive(false);
        }
        else if (m_type == 12)
        {
            if (m_t_pvp_shop.liebi > 0)
            {
                m_input_price = m_t_pvp_shop.liebi * m_input_num;
                m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(22) + m_input_price + "";
            }
            else if (m_t_pvp_shop.redequippower > 0)
            {
                m_input_price = m_t_pvp_shop.redequippower * m_input_num;
                m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(22) + m_input_price + "";

            }
            else if (m_t_pvp_shop.redrolepower > 0)
            {
                m_input_price = m_t_pvp_shop.redrolepower * m_input_num;
                m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(22) + m_input_price + "";
            }

        }
        else if (m_type == 19)
        {
            m_input_price = m_t_chongzhifanpai_shop.price * m_input_num;
            m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(26) + m_input_price + "";
        }
        else if (m_type == 21)
        {
            m_input_price = m_t_mofang_shop.price * m_input_num;
            if (sys._instance.m_self.get_att(e_player_attr.player_mofang_jifen) < m_input_price)
            {
                m_zprice.GetComponent<UILabel>().text = "[ff0000]" + m_input_price + "";

            }
            else
            {
                m_zprice.GetComponent<UILabel>().text = sys._instance.get_res_color(28) + m_input_price + "";
 
            }

 
        }
		m_input.GetComponent<UILabel>().text = m_input_num.ToString ();

	}

	
	public void click(GameObject obj)
	{
		if (obj.name == "add") 
		{
			if(m_input_num == 0 && m_type != 7)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("equip_buy_gui.cs_988_59"));//VIP等级不足，请提升VIP等级后再次尝试
                return;
			}

			if(m_input_num + 1 <= total_num  )
			{
                if (m_type == 16)
                {
                    if(m_input_num + 1 <= sys._instance.m_self.get_att(e_player_attr.player_bingyuan_point) / m_t_bingyuan_shop.binjin)
                    {
                         m_input_num++;
                    }

                }
                else
                {
                    m_input_num++;
                }
			}
			else
			{
				m_input_num = total_num;
			}
            if (m_input_num == 0)
            {
                m_input_num = 1;
            }

		}
		else if(obj.name == "sub")
		{
			if(m_input_num == 0)
			{
				return;
			}
			if(m_input_num - 1 > 0)
			{
				m_input_num--;
			}

		}
		else if(obj.name == "add10")
		{
			if(m_input_num == 0 && m_type != 7)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("equip_buy_gui.cs_988_59"));//VIP等级不足，请提升VIP等级后再次尝试
                return;
			}
			if(m_input_num + 10 <= total_num  )
			{
                if (m_type == 16)
                {
                    if (m_input_num + 10 <= sys._instance.m_self.get_att(e_player_attr.player_bingyuan_point) / m_t_bingyuan_shop.binjin)
                    {
                        m_input_num += 10;
                    }

                }
                else
                {
                    m_input_num += 10;
                }
			}
			else
			{
				m_input_num = total_num;
			}
            if (m_input_num == 0)
            {
                m_input_num = 1;
            }
		}
		else if(obj.name == "sub10")
		{
			if(m_input_num == 0)
			{
				return;
			}
			if(m_input_num - 10 > 0)
			{
				m_input_num -= 10;
			}
			else
			{
				m_input_num = 1;
			}

		}
		else if(obj.name == "queding")
		{
            if (m_type == 2)
            {
                if (sys._instance.m_self.m_t_player.jewel >= m_input_price)
                {
                    if (m_input_num == 0)
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("equip_buy_gui.cs_988_59"));//VIP等级不足，请提升VIP等级后再次尝试
                        return;
                    }
                    protocol.game.cmsg_shop_xg _net_msg = new protocol.game.cmsg_shop_xg();
                    _net_msg.shop_id = _t_shop_xg.id;
                    _net_msg.shop_num = m_input_num;
                    net_http._instance.send_msg<protocol.game.cmsg_shop_xg>(opclient_t.CMSG_SHOP_XG, _net_msg);
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
                }
            }
            else if (m_type == 3)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_treasure_powder) < m_input_price)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1101_75"));//荣誉不足
                    return;
                }
                if (m_t_sport_shop.hb_power > 0)
                {
                    if (sys._instance.m_self.get_item_num(m_role_power) < m_t_sport_shop.hb_power * m_input_num)
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1108_79"));//红色伙伴之力不足
                        return;
                    }
                }
                protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                _net_msg.item_id = m_t_sport_shop.id;
                _net_msg.num = m_input_num;
                net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_SPORT_SHOP_BUY, _net_msg);
            }
            else if (m_type == 4)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_medal_point) < m_input_price)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1121_75"));//魔王勋章不足
                    return;
                }
                if (m_t_boss_shop.hb_power > 0)
                {
                    if (sys._instance.m_self.get_item_num(m_role_power) < m_t_boss_shop.hb_power * m_input_num)
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1108_79"));//红色伙伴之力不足
                        return;
                    }
                }
                protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                _net_msg.item_id = m_t_boss_shop.id;
                _net_msg.num = m_input_num;
                net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_BOSS_SHOP_BUY, _net_msg);
            }
            else if (m_type == 12)
            {
                if (m_t_pvp_shop.liebi > 0)
                {
                    if (sys._instance.m_self.get_att(e_player_attr.player_pvp_jz) >= m_input_price)
                    {
                        protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                        _net_msg.item_id = m_t_pvp_shop.id;
                        _net_msg.num = m_input_num;
                        net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_LIEREN_SHOP, _net_msg);
                    }
                    else
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1150_79"));//猎人奖章不足
                    }

                }
                if (m_t_pvp_shop.redequippower > 0)
                {
                    if (sys._instance.m_self.get_item_num(50120001) >= m_input_price)
                    {
                        protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                        _net_msg.item_id = m_t_pvp_shop.id;
                        _net_msg.num = m_input_num;
                        net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_LIEREN_SHOP, _net_msg);
                    }
                    else
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1165_79"));//红色装备之力不足
                    }

                }
                if (m_t_pvp_shop.redrolepower > 0)
                {
                    if (sys._instance.m_self.get_item_num(50110001) >= m_input_price)
                    {
                        protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                        _net_msg.item_id = m_t_pvp_shop.id;
                        _net_msg.num = m_input_num;
                        net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_LIEREN_SHOP, _net_msg);
                    }
                    else
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1108_79"));//红色伙伴之力不足
                    }

                }

            }
            else if (m_type == 5)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_contribution) < m_input_price)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1190_75"));//军团贡献不足
                    return;
                }
                if (m_t_guild_shop.hb_power > 0)
                {
                    if (sys._instance.m_self.get_item_num(m_role_power) < m_t_guild_shop.hb_power * m_input_num)
                    {
                        root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1108_79"));//红色伙伴之力不足
                        return;
                    }
                }
                protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                _net_msg.item_id = m_t_guild_shop.id;
                _net_msg.num = m_input_num;
                net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_GUILD_SHOP_BUY, _net_msg);
            }
            else if (m_type == 16)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_bingyuan_point) < m_input_price)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1210_75"));//冰晶不足
                    return;
                }
                protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                _net_msg.item_id = m_t_bingyuan_shop.id;
                _net_msg.num = m_input_num;
                net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_BY_SHOP_BUY, _net_msg);
            }
            else if (m_type == 6)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_hj) >= m_input_price)
                {
                    protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                    _net_msg.item_id = m_t_ttt_shop.id;
                    _net_msg.num = m_input_num;
                    net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_TTT_SHOP_BUY, _net_msg);
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75"));//合金不足
                }
            }
            else if (m_type == 7)
            {
                protocol.game.cmsg_item_apply _net_msg = new protocol.game.cmsg_item_apply();
                _net_msg.item_id = (uint)t_itemhecheng.id;
                _net_msg.item_count = m_input_num;
                net_http._instance.send_msg<protocol.game.cmsg_item_apply>(opclient_t.CMSG_ITEM_HECHENG, _net_msg);
            }
            else if (m_type == 13)
            {
                if (sys._instance.m_self.get_att(e_player_attr.player_luck_point) >= m_input_price)
                {
                    protocol.game.cmsg_shop_buy _net_msg = new protocol.game.cmsg_shop_buy();
                    _net_msg.item_id = m_luck_shop.id;
                    _net_msg.num = m_input_num;
                    net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_HUIYI_LUCK_SHOP, _net_msg);
                }
                else
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1250_75"));//幸运点不足
                }

            }
            else if (m_type == 15)
            {
                s_message _message = new s_message();
                _message.m_type = "tanbao_shop_buy";
                _message.m_ints.Add(m_input_num);
                cmessage_center._instance.add_message(_message);
            }
            else if (m_type == 21)
            {
                if (m_input_price > sys._instance.m_self.get_att(e_player_attr.player_mofang_jifen))
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1265_75"));//魔方积分不足
                }
                else
                {
                    s_message _message = new s_message();
                    _message.m_type = "mofang_shop_buy";
                    _message.m_ints.Add(m_input_num);
                    cmessage_center._instance.add_message(_message);
                    this.transform.Find("frame_big").GetComponent<frame>().hide();
                }
              
 
            }
            else if (m_type == 19)
            {
                s_message _message = new s_message();
                _message.m_type = "chongzhifanpai_shop_buy";
                _message.m_ints.Add(m_input_num);
                cmessage_center._instance.add_message(_message);
            }
		}
		else if(obj.name == "hide")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.name == "cancle")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		reset ();
	}
	// Update is called once per frame
	void Update () {
		
	}
}
