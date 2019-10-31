
using UnityEngine;
using System.Collections;

public class temaihui_card : MonoBehaviour {

	public GameObject m_item_shop_gui;
	public GameObject m_image;
	public GameObject m_name;
	public GameObject m_gold;
	public GameObject m_icon;
	public GameObject m_money_image;
	public GameObject m_money_image1;
	public GameObject m_numbers;
	public GameObject m_remai;
	public GameObject m_discount;
	public GameObject m_old_price;
	public GameObject m_star;
	public GameObject m_tuijian;
	public GameObject m_jifen;
	public int m_shop_id;
	public int m_item_num;
	public int m_shell = 0;
	public int type = 0;
	public int currency = 0;
	private s_t_shop_xg m_t_shop;
	private s_t_ttt_shop _t_ttt_shop;
	private s_t_ttt_mubiao _t_ttt_mubiao;
	private s_t_guild_mubiao _t_guid_mubiao;
    private s_t_bingyuan_mubiao _t_bingyuan_mubiao;
	private s_t_boss_shop _t_boss_shop;
    private s_t_huiyi_luckshop _t_luck_shop;
    private s_t_pvp_shop _t_pvp_shop;
	private s_t_sport_mubiao _t_honor_shop;
	private s_t_role_shop m_t_role_shop;
    private s_t_chongwu_shop m_t_chongwu_shop;
    private s_t_huiyi_shop m_t_huiyi_shop;
	private s_t_sport_shop m_t_sport_shop;
	private s_t_item m_t_item;
	private s_t_guild_shop m_t_guild_shop;
    private s_t_mofang_shop m_mofang_shop;
    private s_t_guild_shop_ex m_t_guild_shop_guang;
	private s_t_guild_shop_xs m_t_guild_shop_xs;
	public  s_t_tanbao_shop m_t_tanbao_shop;
    private s_t_bingyuan_shop m_t_bingyuan_shop;
	private s_t_chongzhifanpai_shop m_t_chongzhifanpai_shop;
	private string m_ds;
	private bool flag = false;

	private uint red_role_power = 50110001;
	public UILabel m_buy_Label;
    public UILabel m_xg_text;
	// Use this for initialization
	void Start () {


	}
	
	public void click(GameObject obj)
	{
		if(obj.name == "buy" )
		{
			if(m_shell == 0 && (type == 1 || type == 11))
			{
				string s = game_data._instance.get_t_language ("item_shop_card.cs_32_58");//此商品已售罄
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
				return;
			}
			if (m_ds != "")
			{
				if (m_ds == "0")
				{
					root_gui._instance.show_recharge_dialog_box(delegate() {
                        if (m_item_shop_gui != null)
                        {
                            m_item_shop_gui.transform.Find("frame_big").GetComponent<frame>().hide();
 
                        }
					});
				}
				else
				{
					root_gui._instance.show_prompt_dialog_box(m_ds);
				}
				return;
			}
            if (type == 1)
            {
                if (m_t_shop.jewel > sys._instance.m_self.m_t_player.total_recharge)
                {
                    string s = string.Format(game_data._instance.get_t_language("temaihui_card.cs_402_65"), sys._instance.get_czbl(m_t_shop.jewel));//充值{0}元可以购买
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
                    return;
                }
                s_message _msg = new s_message();
                _msg.m_type = "buy_xg_item";
                _msg.m_ints.Add(m_shop_id);
                _msg.m_bools.Add(flag);
                _msg.m_ints.Add(m_shell);
                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 2)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_equip";
                _msg.m_ints.Add(m_shop_id);

                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 3)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_honor_shop";
                _msg.m_ints.Add(m_shop_id);

                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 4)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_mijing_equip";
                _msg.m_ints.Add(m_shop_id);

                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 5)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_mowang_shop";
                _msg.m_ints.Add(m_shop_id);

                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 7)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_guild_reward";
                _msg.m_ints.Add(m_shop_id);

                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 8)
            {
                int id = int.Parse(obj.transform.parent.name);
                s_message _msg = new s_message();

                _msg.m_type = "buy_item";
                _msg.m_ints.Add(m_shop_id);
                _msg.m_ints.Add(id);
                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 9)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_honor_item";
                _msg.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 10)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_guild_shop_item";
                _msg.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 18)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_guild_shop_guanghuan";
                _msg.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 11)
            {
                int id = int.Parse(obj.transform.parent.name) + 1;
                s_message _msg = new s_message();
                _msg.m_type = "buy_guild_xg_item";
                _msg.m_ints.Add(m_shop_id);
                _msg.m_ints.Add(id);
                cmessage_center._instance.add_message(_msg);
            }
            else if (type == 12)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_pvp_shop_item";
                _msg.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_msg);

            }
            else if (type == 13)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_luck_shop_item";
                _msg.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_msg);

            }
            else if (type == 14)
            {
                int id = int.Parse(obj.transform.parent.name);
                s_message _mes = new s_message();
                _mes.m_type = "huiyi_shop_buy";
                _mes.m_ints.Add(m_shop_id);
                _mes.m_ints.Add(id);
                cmessage_center._instance.add_message(_mes);
            }
            else if (type == 15)
            {
                s_message _mes = new s_message();
                _mes.m_type = "tanbao_shop_ex";
                _mes.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_mes);
            }
            else if (type == 16)
            {
                s_message _mes = new s_message();
                _mes.m_type = "bingyuan_shop";
                _mes.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_mes);
            }
            else if (type == 17)
            {
                s_message _mes = new s_message();
                _mes.m_type = "bingyuan_reward";
                _mes.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_mes);
            }
			else if (type == 19)
			{
				s_message _mes = new s_message();
				_mes.m_type = "czfp_shop";
				_mes.m_ints.Add(m_shop_id);
				cmessage_center._instance.add_message(_mes);
			}
            else if (type == 20)
            {
                int id = int.Parse(obj.transform.parent.name);
                s_message _msg = new s_message();

                _msg.m_type = "chongwu_shop_buy";
                _msg.m_ints.Add(m_shop_id);
                _msg.m_ints.Add(id);
                cmessage_center._instance.add_message(_msg);
 
            }
            else if (type == 21)
            {
                s_message _msg = new s_message();

                _msg.m_type = "buy_mofang_shop_item";
                _msg.m_ints.Add(m_shop_id);
                cmessage_center._instance.add_message(_msg);
            }
		}

	}
	public void updata_ui()
	{
        if (type == 1)
        {
            teimaihui();
        }
        else if (type == 2)
        {
            equip_shop();
        }
        else if (type == 3)
        {
            honor_shop();
        }
        else if (type == 4)
        {
            mi_jing_shop_gui();
        }
        else if (type == 5)
        {
            mowang_shop();
        }
        else if (type == 7)
        {
            guild_shop_gui();
        }
        else if (type == 8)
        {
            partner_shop_gui();
        }
        else if (type == 9)
        {
            honor_item_shop();
        }
        else if (type == 10)
        {
            guild_item_shop();
        }
        else if (type == 11)
        {
            guild_xs_shop();
        }
        else if (type == 12)
        {
            pvp_shop();
        }
        else if (type == 13)
        {
            luck_shop();
        }
        else if (type == 14)
        {
            huiyi_shop();
        }
        else if (type == 15)
        {
            tanbao_shop();
        }
        else if (type == 16)
        {
            bingyuan_shop();
        }
        else if (type == 17)
        {
            bingyuan_mubiao_gui();
        }
        else if(type == 18)
        {
            guild_shop_guanghuan();
        }
		else if(type == 19)
		{
			chongzhifanpai_shop();
		}
        else if (type == 20)
        {
            chongwu_shop_gui();
 
        }
        else if (type == 21)
        {
            mofang_shop();
 
        }
	}
	
	public static int get_price(int num, int price_type)
	{
		s_t_price _price = game_data._instance.get_t_price(num);
		int count = 0;
		for(int i = 0; i < game_data._instance.m_dbc_price.get_y();++i)
		{
			count ++;
		}
		if(_price == null)
		{
			_price = game_data._instance.get_t_price(count);
		}
		if(price_type == 1)
		{
			return _price.yuanli_potion;
		}
		if(price_type == 2)
		{
			return _price.tili_potion;
		}
		if(price_type == 3)
		{
			return _price.energy_potion;
		}
		if(price_type == 4)
		{
			return _price.jinxiang_equip;
		}
		if(price_type == 5)
		{
			return _price.jinxiang_treasure;
		}
		if(price_type == 6)
		{
			return _price.mowang_invitation;
		}
		return 0;
	}

	void teimaihui()
	{
        
		this.transform.Find("buy").gameObject.SetActive(true);
		m_old_price.transform.parent.gameObject.SetActive(false);
		m_tuijian.SetActive (false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		int i = 0;
		float discount = 0;
		m_t_shop = game_data._instance.get_shop_xg (m_shop_id);
		m_t_item = game_data._instance.get_item (m_t_shop.vlaue1);

        if (m_t_shop.level > sys._instance.m_self.m_t_player.level)
        {
            m_xg_text.text = string.Format(game_data._instance.get_t_language("temaihui_card.cs_414_65"), m_t_shop.level);
            this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(false);
            m_xg_text.gameObject.SetActive(true);
        }
        else if (platform_config_common.game_model == 2 && m_t_shop.jewel > 0 && m_t_shop.jewel > sys._instance.m_self.m_t_player.total_recharge)
        {
            m_xg_text.text = string.Format(game_data._instance.get_t_language("temaihui_card.cs_402_65"), sys._instance.get_czbl(m_t_shop.jewel));
            this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(false);
            m_xg_text.gameObject.SetActive(true);
        }

        s_t_vip _vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_shop.type,m_t_shop.vlaue1,m_t_shop.vlaue2,m_t_shop.vlaue3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(2);
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(2);
		m_ds = "";
		string s = sys._instance.get_res_color(2);
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_shop.type,m_t_shop.vlaue1,m_t_shop.vlaue2,m_t_shop.vlaue3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		dbc m_dbc_shop_xg = game_data._instance.get_dbc_shop_xg();
		if(m_t_shop.xg_num != 0)
		{
			s = sys._instance.get_res_color(2);
			m_ds = "";
			if(m_t_shop.xg_type == 2)
			{
				int xg_num = 0;
				m_shell = m_t_shop.vip_type[sys._instance.m_self.m_t_player.vip] - xg_num;
				for(i = 0;i < sys._instance.m_self.m_t_player.shop_xg_ids.Count;++i )
				{
					if(m_t_shop.id == sys._instance.m_self.m_t_player.shop_xg_ids[i])
					{
						xg_num = sys._instance.m_self.m_t_player.shop_xg_nums[i];
						m_shell = m_t_shop.vip_type[sys._instance.m_self.m_t_player.vip] - xg_num;
						break;
					}
				}
				if (sys._instance.m_self.get_att(e_player_attr.player_jewel) <  get_price(xg_num+1,m_t_shop.price_type))
				{
					s = "[ff0000]";
					m_ds = "0";
				}
				else
				{
					s = sys._instance.get_res_color(2);
				}
				int jewel = 0;
			
				jewel = m_t_shop.price;
	
				discount = ((float)get_price(xg_num+1,m_t_shop.price_type) / (float)jewel)*10;
				m_gold.GetComponent<UILabel>().text = s + get_price(xg_num+1,m_t_shop.price_type);
			}
			else if(m_t_shop.xg_type == 1)
			{
				s = sys._instance.get_res_color(2);
				m_ds = "";
				int xg_num = 0;
				m_shell = m_t_shop.vip_type[0] - xg_num;
				for(i = 0;i < sys._instance.m_self.m_t_player.shop_xg_ids.Count;++i )
				{
					if(m_t_shop.id == sys._instance.m_self.m_t_player.shop_xg_ids[i])
					{
						xg_num = sys._instance.m_self.m_t_player.shop_xg_nums[i];
						m_shell = m_t_shop.vip_type[0] - xg_num;
						break;
					}
				}
				if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < m_t_shop.price)
				{
					s = "[ff0000]";
					m_ds = "0";
				}
				else
				{
					s = sys._instance.get_res_color(2);
				}
				int jewel = 0;
				jewel = m_t_shop.price;
				discount = ((float)m_t_shop.price /(float)jewel)*10;
				m_gold.GetComponent<UILabel>().text = s + m_t_shop.price;
			}
			m_numbers.SetActive(true);
			m_numbers.GetComponent<UILabel>().text = "[b3fe13]" + string.Format(game_data._instance.get_t_language ("temaihui_card.cs_475_71") , m_shell );//今日可买{0}次[-][0aff16][-]
		}
		else
		{
			s = sys._instance.get_res_color(2);
			m_ds = "";
			m_shell = 1;
			if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < m_t_shop.price)
			{
				s = "[ff0000]";
				m_ds = "0";
			}
			int jewel = 0;
			jewel = m_t_shop.price;
			discount = ((float)m_t_shop.price / (float)jewel)*10;
			m_gold.GetComponent<UILabel>().text = s + m_t_shop.price.ToString();
			m_numbers.SetActive(false);
		}
		if(m_shell == 0)
		{
			m_numbers.SetActive(false);
			this.transform.Find("kuang").gameObject.SetActive (true);
			this.transform.Find("buy").gameObject.SetActive(false);
		}
		if(discount >= 10.0f || discount == 0)
		{
			m_remai.SetActive(false);
		}
		else
		{
			m_remai.SetActive(true);
			discount = (float)decimal.Round ((decimal)discount, 1);
			m_discount.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"),sys._instance.discountChange(discount));//{0}折
		}
		m_star.SetActive (false);
		m_jifen.SetActive (false);
	}

	void equip_shop()
	{
		int money_type = 0;
		int price = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		_t_ttt_shop =  game_data._instance.get_t_ttt_shop (m_shop_id);
		m_tuijian.SetActive (false);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(_t_ttt_shop.type,_t_ttt_shop.value1,_t_ttt_shop.value2,_t_ttt_shop.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);

		money_type = 6;
        price = sys._instance.m_self.get_att(e_player_attr.player_hj);
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
		m_ds = "";
		string s = "";
		if (price < _t_ttt_shop.price)
		{
			s = "[ff0000]";
			if(type == 2)
			{
				m_ds = "[ffc882]" +  game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
			}
			else if(type == 5)
			{
				m_ds = "[ffc882]" +  game_data._instance.get_t_language ("equip_buy_gui.cs_1121_75");//魔王勋章不足
			}
		}
		else
		{
			s = sys._instance.get_res_color(money_type);
		}
		
		m_gold.GetComponent<UILabel>().text = s + _t_ttt_shop.price.ToString();
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_t_ttt_shop.type,_t_ttt_shop.value1,_t_ttt_shop.value2,_t_ttt_shop.value3);
		if(_t_ttt_shop.type == 2)
		{
			s_t_item _item = game_data._instance.get_item(_t_ttt_shop.value1);
			int num = 0;
			num = sys._instance.m_self.get_item_num((uint)_t_ttt_shop.value1);
			if(_item.type == 7001)
			{
				m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_t_ttt_shop.type,_t_ttt_shop.value1,_t_ttt_shop.value2,_t_ttt_shop.value3)
					+"[5CB7F2]"+"(" + num.ToString()+ "/" + _item.def_2.ToString()+ ")" + "[-]";
			}
		}

		this.transform.Find("kuang").gameObject.SetActive (false);
		m_remai.SetActive(false);
		m_numbers.SetActive(false);
		m_old_price.transform.parent.gameObject.SetActive(false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		m_star.SetActive (false);
		m_jifen.SetActive (false);
	}

	void mowang_shop()
	{
		int money_type = 0;
		int price = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_tuijian.SetActive (false);
		_t_boss_shop =  game_data._instance.get_t_boss_shop (m_shop_id);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(_t_boss_shop.type,_t_boss_shop.value1,_t_boss_shop.value2,_t_boss_shop.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		money_type = 13;
		price = sys._instance.m_self.get_att(e_player_attr.player_medal_point);
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
		m_ds = "";
		string s = "";
		if (price < _t_boss_shop.price)
		{
			s = "[ff0000]";
			if(type == 2)
			{
				m_ds = "[ffc882]" +  game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
			}
			else if(type == 5)
			{
				m_ds = "[ffc882]" +  game_data._instance.get_t_language ("equip_buy_gui.cs_1121_75");//魔王勋章不足
			}
		}
		else
		{
			s = sys._instance.get_res_color(money_type);
		}
		
		m_gold.GetComponent<UILabel>().text = s + _t_boss_shop.price.ToString();
		int num = sys._instance.m_self.get_item_num (red_role_power);
		if(_t_boss_shop.hb_power > 0)
		{
			m_old_price.transform.parent.gameObject.SetActive(true);
			price = _t_boss_shop.hb_power;
			if(num < price)
			{
				s = "[ff0000]";
				m_ds = game_data._instance.get_t_language ("temaihui_card.cs_617_11");//[ffc882]红色伙伴之力不足
			}
			else
			{
				s = "[FF8585]";
			}
			m_old_price.transform.GetComponent<UILabel>().text = s + _t_boss_shop.hb_power.ToString();
			m_money_image.transform.Find("hua").gameObject.SetActive(false);
		}
		else
		{
			m_old_price.transform.parent.gameObject.SetActive(false);
		}
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_t_boss_shop.type,_t_boss_shop.value1,_t_boss_shop.value2,_t_boss_shop.value3);
		if(_t_boss_shop.type == 2)
		{
			s_t_item _item = game_data._instance.get_item(_t_boss_shop.value1);
			num = 0;
			num = sys._instance.m_self.get_item_num((uint)_t_boss_shop.value1);
			if(_item.type == 7001)
			{
				m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_t_boss_shop.type,_t_boss_shop.value1,_t_boss_shop.value2,_t_boss_shop.value3)
					+"[5CB7F2]"+"(" + num.ToString()+ "/" + _item.def_2.ToString()+ ")" + "[-]";
			}
		}
		
		this.transform.Find("kuang").gameObject.SetActive (false);
		m_remai.SetActive(true);
		if(_t_boss_shop.discount == 0)
		{
			m_remai.SetActive(false);
		}
        m_discount.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"), sys._instance.discountChange(_t_boss_shop.discount)); //{0}折
		m_numbers.SetActive(false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		if(_t_boss_shop.hb_power > 0)
		{
			m_old_price.transform.parent.localPosition = new Vector3(-60f,3f,0);
            m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
			m_gold.transform.parent.localPosition = new Vector3 (-60f,-28f,0);
		}
		m_star.SetActive (false);
		m_jifen.SetActive (false);
	}
    void luck_shop()
    {
        int money_type = 0;
        int price = 0;
        this.transform.Find("buy").gameObject.SetActive(true);
        m_tuijian.SetActive(false);
        _t_luck_shop = game_data._instance.get_t_huiyi_luckshop(m_shop_id);
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(_t_luck_shop.reward.type, _t_luck_shop.reward.value1, _t_luck_shop.reward.value2, _t_luck_shop.reward.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        money_type = 20;
        price = sys._instance.m_self.get_att(e_player_attr.player_luck_point);
        m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
        m_ds = "";
        string s = "";
        if (price < _t_luck_shop.luck_point)
        {
            s = "[ff0000]";
            if (type == 2)
            {
                m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
            }
            else if (type == 5)
            {
                m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1121_75");//魔王勋章不足
            }
        }
        else
        {
            s = sys._instance.get_res_color(money_type);
        }

        m_gold.GetComponent<UILabel>().text = s + _t_luck_shop.luck_point.ToString();
        m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(_t_luck_shop.reward.type, _t_luck_shop.reward.value1, _t_luck_shop.reward.value2, _t_luck_shop.reward.value3);
        if (_t_luck_shop.reward.type == 2)
        {
            s_t_item _item = game_data._instance.get_item(_t_luck_shop.reward.value1);
            int num = 0;
            num = sys._instance.m_self.get_item_num((uint)_t_luck_shop.reward.value1);
            if (_item.type == 7001)
            {
                m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(_t_luck_shop.reward.type, _t_luck_shop.reward.value1, _t_luck_shop.reward.value2, _t_boss_shop.value3)
                    + "[5CB7F2]" + "(" + num.ToString() + "/" + _item.def_2.ToString() + ")" + "[-]";
            }
        }
        if (_t_luck_shop.day_num - luck_shop(_t_luck_shop.id) == 0)
        {
            m_numbers.SetActive(false);
            this.transform.Find("kuang").gameObject.SetActive(true);
            this.transform.Find("buy").gameObject.SetActive(false);
        }
        else
        {
            m_numbers.SetActive(true);
            this.transform.Find("kuang").gameObject.SetActive(false);
            this.transform.Find("buy").gameObject.SetActive(true);
 
        }
      //  this.transform.Find("kuang").gameObject.SetActive(false);
        m_remai.SetActive(false);
        m_numbers.GetComponent<UILabel>().text = "[b3fe13]" + string.Format(game_data._instance.get_t_language ("temaihui_card.cs_725_76"),(_t_luck_shop.day_num - luck_shop(_t_luck_shop.id)) );// "[b3fe13]" + text +"[-]" + "[0aff16]"+m_shell + text1+"[-]";//今日还可购买 :[-][0aff16]{0}次
        m_old_price.transform.parent.gameObject.SetActive(false);
        m_gold.transform.parent.localPosition = new Vector3(-60f, -20f, 0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
        m_star.SetActive(false);
		m_jifen.SetActive (false);
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
    void pvp_shop()
    {
        int price = 0;
        this.transform.Find("buy").gameObject.SetActive(true);
        m_tuijian.SetActive(false);
        _t_pvp_shop = game_data._instance.get_t_pvp_shop(m_shop_id);
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(_t_pvp_shop.type, _t_pvp_shop.value1, _t_pvp_shop.value2, _t_pvp_shop.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);
        m_remai.SetActive(false);
        if (_t_pvp_shop.liebi > 0)
        {
            price = sys._instance.m_self.get_att(e_player_attr.player_pvp_jz);
            m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(22);
            m_ds = "";
            string s = "";
            if (price < _t_pvp_shop.liebi)
            {
                s = "[ff0000]";
                if (type == 2)
                {
                    m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
                }
                else if (type == 5)
                {
                    m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1121_75");//魔王勋章不足
                }
            }
            else
            {
                s = sys._instance.get_res_color(22);
            }

            m_gold.GetComponent<UILabel>().text = s + _t_pvp_shop.liebi.ToString();
 
        }
        else if (_t_pvp_shop.redequippower > 0)
        {
            m_ds = "";
            string s = "";
            price = sys._instance.m_self.get_item_num(50120001);
            m_money_image1.GetComponent<UISprite>().spriteName = "hszbzlx_icon";

            if (price < _t_pvp_shop.redequippower)
            {
                s = "[ff0000]";
                if (type == 2)
                {
                    m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
                }
                else if (type == 5)
                {
                    m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1121_75");//魔王勋章不足
                }
            }
            else
            {
                s = "[ff8585]";
            }

            m_gold.GetComponent<UILabel>().text = s + _t_pvp_shop.redequippower.ToString();

        }
        else if (_t_pvp_shop.redrolepower > 0)
        {
            m_ds = "";
            string s = "";
            price = sys._instance.m_self.get_item_num(50110001);
            m_money_image1.GetComponent<UISprite>().spriteName = "hshbzlx_icon";

            if (price < _t_pvp_shop.redrolepower)
            {
                s = "[ff0000]";
                if (type == 2)
                {
                    m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
                }
                else if (type == 5)
                {
                    m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1121_75");//魔王勋章不足
                }
            }
            else
            {
                s = "[ff8585]";
            }

            m_gold.GetComponent<UILabel>().text = s + _t_pvp_shop.redrolepower.ToString();

        }
        m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(_t_pvp_shop.type, _t_pvp_shop.value1, _t_pvp_shop.value2, _t_pvp_shop.value3);
        if (_t_pvp_shop.type == 2)
        {
            s_t_item _item = game_data._instance.get_item(_t_pvp_shop.value1);
            int num = 0;
            num = sys._instance.m_self.get_item_num((uint)_t_pvp_shop.value1);
            if (_item.type == 7001)
            {
                m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(_t_pvp_shop.type, _t_pvp_shop.value1, _t_pvp_shop.value2, _t_pvp_shop.value3)
                    + "[5CB7F2]" + "(" + num.ToString() + "/" + _item.def_2.ToString() + ")" + "[-]";
            }
        }

        this.transform.Find("kuang").gameObject.SetActive(false);
        m_numbers.SetActive(false);
        m_old_price.transform.parent.gameObject.SetActive(false);
        m_gold.transform.parent.localPosition = new Vector3(-60f, -20f, 0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
        m_star.SetActive(false);
		m_jifen.SetActive (false);
    }
	void honor_shop()
	{
		this.transform.Find("buy").gameObject.SetActive(true);
		_t_honor_shop =  game_data._instance.get_t_sport_mubiao (m_shop_id);
		m_tuijian.SetActive (false);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(_t_honor_shop.type,_t_honor_shop.value1,_t_honor_shop.value2,_t_honor_shop.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(14);
		m_ds = "";
		string s = "";
		if (sys._instance.m_self.get_att(e_player_attr.player_treasure_powder) < _t_honor_shop.price)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]" +  game_data._instance.get_t_language ("equip_buy_gui.cs_1101_75");//荣誉不足
		}
		m_gold.GetComponent<UILabel>().text = s + _t_honor_shop.price.ToString();
		if(sys._instance.m_self.m_t_player.max_rank > _t_honor_shop.rank)
		{
			this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = false;
			this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = true;
			this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(true);
		}
		m_remai.SetActive(true);
		m_remai.transform.localPosition = new Vector3 (-167,41,0);
		m_discount.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"),  sys._instance.discountChange(_t_honor_shop.discount));//{0}折
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_t_honor_shop.type ,_t_honor_shop.value1,_t_honor_shop.value2,_t_honor_shop.value3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		if(_t_honor_shop.discount == 0)
		{
			m_remai.SetActive(false);
		}

		m_old_price.transform.parent.gameObject.SetActive(false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		m_numbers.SetActive(true);
		m_numbers.GetComponent<UILabel>().text = "[b3fe13]" + string.Format(game_data._instance.get_t_language ("temaihui_card.cs_912_71"),_t_honor_shop.rank.ToString () //达到[-]{0}名[-][b3fe13]可购买[-]
			);
		if(honor_shop_gui.can_buy(m_shop_id))
		{
			this.transform.Find("kuang").gameObject.SetActive (true);
			this.transform.Find("buy").gameObject.SetActive(false);
		}
		m_star.SetActive (false);
		m_jifen.SetActive (false);
	}

	void mi_jing_shop_gui()
	{
		int money_type = 0;
		int price = 0;
		int buy_qualification = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_tuijian.SetActive (false);
		_t_ttt_mubiao =  game_data._instance.get_t_ttt_mubiao (m_shop_id);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(_t_ttt_mubiao.type,_t_ttt_mubiao.value1,_t_ttt_mubiao.value2,_t_ttt_mubiao.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);

	
		money_type = 6;
        price = sys._instance.m_self.get_att(e_player_attr.player_hj);
		for (int i = 0; i < sys._instance.m_self.m_t_player.ttt_last_stars.Count; ++i)
		{
			buy_qualification += sys._instance.m_self.m_t_player.ttt_last_stars[i];
		}
		

		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
		m_ds = "";
		string s = "";
		if ( price < _t_ttt_mubiao.price)
		{
			s = "[ff0000]";

            m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足

		}
		else
		{
			s = sys._instance.get_res_color(money_type);
		}
		m_gold.GetComponent<UILabel>().text = s + _t_ttt_mubiao.price.ToString();
		s = "[1EFD72]";
		if(buy_qualification < _t_ttt_mubiao.star)
		{
			s = "[ff0000]";
			this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = false;
			this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			s = "[1EFD72]";
			this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = true;
			this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(true);
		}
		m_remai.SetActive(true);
		m_remai.transform.localPosition = new Vector3 (-167,41,0);
		m_discount.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"), sys._instance.discountChange(_t_ttt_mubiao.discount));//{0}折
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_t_ttt_mubiao.type ,_t_ttt_mubiao.value1,_t_ttt_mubiao.value2,_t_ttt_mubiao.value3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		if(_t_ttt_mubiao.discount == 0)
		{
			m_remai.SetActive(false);
		}

		m_old_price.transform.parent.gameObject.SetActive(false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);

		
		m_numbers.SetActive(false);
		m_star.SetActive (true);
		m_star.transform.Find("dd").GetComponent<UILabel>().text = string.Format(s + game_data._instance.get_t_language ("temaihui_card.cs_992_84"), _t_ttt_mubiao.star.ToString());//达到{0}星可购买[-]
		if(mijing_shop_gui.can_buy(m_shop_id))
		{
			this.transform.Find("kuang").gameObject.SetActive (true);
			this.transform.Find("buy").gameObject.SetActive(false);
		}
		m_jifen.SetActive (false);
	}

	void guild_shop_gui()
	{
		int money_type = 0;
		int price = 0;
		int buy_qualification = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_tuijian.SetActive (false);
		_t_guid_mubiao =  game_data._instance.get_guild_mubiao (m_shop_id);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(_t_guid_mubiao.type,_t_guid_mubiao.value1,_t_guid_mubiao.value2,_t_guid_mubiao.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);

		money_type = 10;
		price = sys._instance.m_self.get_att(e_player_attr.player_contribution);
		buy_qualification = juntuan_gui._instance.m_guild_t.level;
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
		m_ds = "";
        s_t_resource res = game_data._instance.get_t_resource(10);
        string s = res.namecolor;
		if ( price < _t_guid_mubiao.price)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]" + game_data._instance.get_t_language ("temaihui_card.cs_1026_23");//贡献不足
		}
		else
		{
            s = res.namecolor;
		}
		m_gold.GetComponent<UILabel>().text = s + _t_guid_mubiao.price.ToString();
		s = "[1EFD72]";
		if(buy_qualification < _t_guid_mubiao.level)
		{
			s = "[ff0000]";
			this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = false;
			this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			s = "[1EFD72]";
			this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = true;
			this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(true);
		}
		m_remai.SetActive(true);
		m_remai.transform.localPosition = new Vector3 (-167,41,0);
		m_discount.GetComponent<UILabel>().text =  sys._instance.discountChange(_t_guid_mubiao.discount) + game_data._instance.get_t_language ("dsign_week_sub.cs_52_80");//折
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_t_guid_mubiao.type ,_t_guid_mubiao.value1,_t_guid_mubiao.value2,_t_guid_mubiao.value3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		if(_t_guid_mubiao.discount == 0)
		{
			m_remai.SetActive(false);
		}
		
		m_old_price.transform.parent.gameObject.SetActive(false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);

		m_star.SetActive(false);
		if (guild_canbuy(m_shop_id))
		{
			this.transform.Find("kuang").gameObject.SetActive(true);
			this.transform.Find("buy").gameObject.SetActive(false);
		}
        if (juntuan_gui._instance.m_guild_t.level >= _t_guid_mubiao.level)
        {
			m_numbers.SetActive(false);
        }
        else
        {
            m_numbers.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1072_67"), _t_guid_mubiao.level );//[ff0000]军团{0}级开放
			m_numbers.SetActive(true);
		}
		m_jifen.SetActive (false);
	}
    void bingyuan_mubiao_gui()
    {
        int money_type = 24;
        int price = 0;
        int buy_qualification = 0;
        this.transform.Find("buy").gameObject.SetActive(true);
        m_tuijian.SetActive(false);
        _t_bingyuan_mubiao = game_data._instance.get_t_bingyuan_mubiao(m_shop_id);
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(_t_bingyuan_mubiao.type, _t_bingyuan_mubiao.value1, _t_bingyuan_mubiao.value2, _t_bingyuan_mubiao.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);

        price = sys._instance.m_self.get_att(e_player_attr.player_bingyuan_point);
        buy_qualification =  bingyuan_gui._instance.m_world.point;
        m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
        m_ds = "";
        s_t_resource res = game_data._instance.get_t_resource(money_type);
        string s = res.namecolor;
        if (price < _t_bingyuan_mubiao.price)
        {
            s = "[ff0000]";
            m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1210_75");//冰晶不足
        }
        else
        {
            s = res.namecolor;
        }
        m_gold.GetComponent<UILabel>().text = s + _t_bingyuan_mubiao.price.ToString();
        s = "[1EFD72]";
        if (buy_qualification < _t_bingyuan_mubiao.jifen)
        {
            s = "[ff0000]";
            this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = false;
            this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(false);
        }
        else
        {
            s = "[1EFD72]";
            this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = true;
            this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(true);
        }
        m_remai.SetActive(true);
        m_remai.transform.localPosition = new Vector3(-167, 41, 0);
		m_discount.GetComponent<UILabel>().text =   sys._instance.discountChange(_t_bingyuan_mubiao.discount) + game_data._instance.get_t_language ("dsign_week_sub.cs_52_80");//折
        m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(_t_bingyuan_mubiao.type, _t_bingyuan_mubiao.value1, _t_bingyuan_mubiao.value2, _t_bingyuan_mubiao.value3);
        this.transform.Find("kuang").gameObject.SetActive(false);
        if (_t_bingyuan_mubiao.discount == 0)
        {
            m_remai.SetActive(false);
        }

        m_old_price.transform.parent.gameObject.SetActive(false);
        m_gold.transform.parent.localPosition = new Vector3(-60f, -20f, 0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);

        m_star.SetActive(false);
        if (bingyuan_canbuy(m_shop_id))
        {
            this.transform.Find("kuang").gameObject.SetActive(true);
            this.transform.Find("buy").gameObject.SetActive(false);
        }
        if (bingyuan_gui._instance.m_world.point >= _t_bingyuan_mubiao.jifen)
        {
            m_numbers.SetActive(false);
        }
        else
        {
            m_numbers.GetComponent<UILabel>().text = game_data._instance.get_t_language ("temaihui_card.cs_1147_53") + _t_bingyuan_mubiao.jifen + "";//[ff0000]积分达到
            m_numbers.SetActive(true);
        }
        m_jifen.SetActive(false);
    }
    public void partner_shop_gui()
	{
		int money_type = 0;
		int price = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_tuijian.SetActive (false);
		//this.transform.Find("buy").localPosition = new Vector3(120,-17,0);
		m_t_role_shop =  game_data._instance.get_role_shop (m_shop_id);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_role_shop.sell_type,m_t_role_shop.sell_value_0,m_t_role_shop.sell_value_1,m_t_role_shop.sell_value_2);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		
		money_type = m_t_role_shop.money_type;
		if(money_type == 2)
		{
			price = sys._instance.m_self.get_att (e_player_attr.player_jewel);
		}
		else if(money_type == 1)
		{
			price = sys._instance.m_self.get_att (e_player_attr.player_gold);
		}
		else if(money_type == 6)
		{
            price = sys._instance.m_self.get_att(e_player_attr.player_hj);
		}
		else if(money_type == 5)
		{
			price = sys._instance.m_self.get_att (e_player_attr.player_jjc_point);
		}
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
		m_ds = "";
		string s = "";
		if (price < m_t_role_shop.price)
		{
			s = "[ff0000]";
			if(money_type == 2)
			{
				m_ds = "0";
			}
			else if(money_type == 1)
			{
				m_ds = "[ffc882]" +  game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
			}
			else if(money_type == 6)
			{
				m_ds = "[ffc882]" +  game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
			}
			else if(money_type == 5)
			{
				m_ds = "[ffc882]" +  game_data._instance.get_t_language ("item_shop_card.cs_84_24");//战魂不足
			}
		}
		else
		{
			s = sys._instance.get_res_color(money_type);
		}
		
		m_gold.GetComponent<UILabel>().text = s + m_t_role_shop.price.ToString();
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (m_t_role_shop.sell_type,m_t_role_shop.sell_value_0,m_t_role_shop.sell_value_1,m_t_role_shop.sell_value_2);
		this.transform.Find("kuang").gameObject.SetActive (false);
		this.transform.Find("buy").gameObject.SetActive(true);
		m_remai.SetActive(false);
		m_numbers.SetActive(false);
		m_old_price.transform.parent.gameObject.SetActive(false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		m_star.SetActive (false);
		if(m_shell != 0)
		{
			this.transform.Find("kuang").gameObject.SetActive (true);
			this.transform.Find("buy").gameObject.SetActive(false);
		}
		if(m_t_role_shop.rec == 1)
		{
			m_tuijian.SetActive(true);
			m_tuijian.GetComponent<UISprite>().spriteName = "tj_jiaobiao";
            m_tuijian.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("temaihui_card.cs_1232_263");//推荐
		}
		if(m_t_role_shop.sell_type == 2)
		{
			s_t_item _t_item = game_data._instance.get_item(m_t_role_shop.sell_value_0);
			if(_t_item.type == 3001)
			{
				for(int i = 0;i < sys._instance.m_self.m_t_player.zhenxing.Count;i ++)
				{
					ulong guid = sys._instance.m_self.m_t_player.zhenxing[i];
					ccard m_card = sys._instance.m_self.get_card_guid(guid);
					if(m_card == null)
					{
						continue;
					}
					if(m_card.get_template_id() == _t_item.def_1)
					{
						m_tuijian.SetActive(true);
						m_tuijian.GetComponent<UISprite>().spriteName = "ysz_jiaobiao";
                        m_tuijian.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language("temaihui_card.cs_1246_263");//已拥有
						break;
					}
				}
			}
		}
		m_jifen.SetActive (false);
	}
    public void chongwu_shop_gui()
    {
        int money_type = 0;
        int price = 0;
        this.transform.Find("buy").gameObject.SetActive(true);
        m_tuijian.SetActive(false);
        //this.transform.Find("buy").localPosition = new Vector3(120,-17,0);
        m_t_chongwu_shop = game_data._instance.get_t_chongwu_shop(m_shop_id);
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(m_t_chongwu_shop.type, m_t_chongwu_shop.value1, m_t_chongwu_shop.value2, m_t_chongwu_shop.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);

        money_type = m_t_chongwu_shop.huobitype;
        price = sys._instance.m_self.get_att((e_player_attr)money_type);
        
        m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
        m_ds = "";
        string s = "";
        if (price < m_t_chongwu_shop.huobi)
        {
            s = "[ff0000]";
            if (money_type == 2)
            {
                m_ds = "0";
            }
            else if (money_type == 1)
            {
                m_ds = "[ffc882]" + game_data._instance.get_t_language ("bu_zheng_panel.cs_671_60");//金币不足
            }
            else if (money_type == 6)
            {
                m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1229_75");//合金不足
            }
            else if (money_type == 5)
            {
                m_ds = "[ffc882]" + game_data._instance.get_t_language ("item_shop_card.cs_84_24");//战魂不足
            }
            else if (money_type == 27)
            {
                m_ds = "[ffc882]" + game_data._instance.get_t_language ("temaihui_card.cs_1298_36");//数码芯片不足
            }
        }
        else
        {
            s = sys._instance.get_res_color(money_type);
        }

        m_gold.GetComponent<UILabel>().text = s + m_t_chongwu_shop.huobi.ToString();
        m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_chongwu_shop.type, m_t_chongwu_shop.value1, m_t_chongwu_shop.value2, m_t_chongwu_shop.value3);
        this.transform.Find("kuang").gameObject.SetActive(false);
        this.transform.Find("buy").gameObject.SetActive(true);
        m_remai.SetActive(false);
        m_numbers.SetActive(false);
        m_old_price.transform.parent.gameObject.SetActive(false);
        m_gold.transform.parent.localPosition = new Vector3(-60f, -20f, 0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
        m_star.SetActive(false);
        if (m_shell != 0)
        {
            this.transform.Find("kuang").gameObject.SetActive(true);
            this.transform.Find("buy").gameObject.SetActive(false);
        }
        if (m_t_chongwu_shop.tuijian == 1)
        {
            m_tuijian.SetActive(true);
            m_tuijian.GetComponent<UISprite>().spriteName = "tj_jiaobiao";
            m_tuijian.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("temaihui_card.cs_1232_263");//推荐
        }
        if (m_t_chongwu_shop.type == 2)
        {
            s_t_item _t_item = game_data._instance.get_item(m_t_chongwu_shop.value1);
            if (_t_item.type == 3001)
            {
                for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i++)
                {
                    ulong guid = sys._instance.m_self.m_t_player.zhenxing[i];
                    ccard m_card = sys._instance.m_self.get_card_guid(guid);
                    if (m_card == null)
                    {
                        continue;
                    }
                    if (m_card.get_template_id() == _t_item.def_1)
                    {
                        m_tuijian.SetActive(true);
                        m_tuijian.GetComponent<UISprite>().spriteName = "ysz_jiaobiao";
                        m_tuijian.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language("temaihui_card.cs_1246_263");//已拥有
                        break;
                    }
                }
            }
        }
        m_jifen.SetActive(false);
 
    }
    void huiyi_shop()
    {
        int money_type = 0;
        int price = 0;
        this.transform.Find("buy").gameObject.SetActive(true);
        m_tuijian.SetActive(false);
		m_jifen.SetActive (false);
        //this.transform.Find("buy").localPosition = new Vector3(120,-17,0);
        m_t_huiyi_shop = game_data._instance.get_t_huiyi_shop(m_shop_id);
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(m_t_huiyi_shop.reward.type,
            m_t_huiyi_shop.reward.value1, m_t_huiyi_shop.reward.value2, m_t_huiyi_shop.reward.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);

        money_type = m_t_huiyi_shop.huobi_type;
        price = sys._instance.m_self.get_att((e_player_attr)m_t_huiyi_shop.huobi_type);
        
        m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
        m_ds = "";
        string s = "";
        if (price < m_t_huiyi_shop.huobi_value)
        {
            s = "[ff0000]";
            if (money_type == 2)
            {
                m_ds = "0";
            }
            else if (money_type == 21)
            {
                m_ds = "[ffc882]" + game_data._instance.get_t_language ("temaihui_card.cs_1383_36");//回忆结晶不足
            }
        }
        else
        {
            s = sys._instance.get_res_color(money_type);
        }

        m_gold.GetComponent<UILabel>().text = s + m_t_huiyi_shop.huobi_value.ToString();
        m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_huiyi_shop.reward.type,
            m_t_huiyi_shop.reward.value1, m_t_huiyi_shop.reward.value2, m_t_huiyi_shop.reward.value3);
        this.transform.Find("kuang").gameObject.SetActive(false);
        this.transform.Find("buy").gameObject.SetActive(true);
        m_remai.SetActive(false);
        m_numbers.SetActive(false);
        m_old_price.transform.parent.gameObject.SetActive(false);
        m_gold.transform.parent.localPosition = new Vector3(-60f, -20f, 0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
        m_star.SetActive(false);
        if (m_shell != 0)
        {
            this.transform.Find("kuang").gameObject.SetActive(true);
            this.transform.Find("buy").gameObject.SetActive(false);
        }
        //if (m_t_role_shop.rec == 1)
        //{
        //    m_tuijian.SetActive(true);
        //    m_tuijian.GetComponent<UISprite>().spriteName = "tj_jiaobiao";
        //}
        
        }
 
	public void honor_item_shop()
	{
		int price = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_t_sport_shop = game_data._instance.get_t_sport_shop (m_shop_id);
		m_tuijian.SetActive (false);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_sport_shop.type,m_t_sport_shop.value1,m_t_sport_shop.value2,m_t_sport_shop.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		
		price = sys._instance.m_self.get_att (e_player_attr.player_treasure_powder);
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(14);
		m_ds = "";
		string s = "";
		if (price < m_t_sport_shop.price)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]" +  game_data._instance.get_t_language ("equip_buy_gui.cs_1101_75");//荣誉不足
		}
		else
		{
			s = sys._instance.get_res_color(14);
		}
		
		m_gold.GetComponent<UILabel>().text = s + m_t_sport_shop.price.ToString();
		int num = sys._instance.m_self.get_item_num (red_role_power);
		if(m_t_sport_shop.hb_power > 0)
		{
			m_old_price.transform.parent.gameObject.SetActive(true);
			price = m_t_sport_shop.hb_power;
			if(num < price)
			{
				s = "[ff0000]";
				m_ds = game_data._instance.get_t_language ("temaihui_card.cs_617_11");//[ffc882]红色伙伴之力不足
			}
			else
			{
				s = "[FF8585]";
			}
			m_old_price.transform.GetComponent<UILabel>().text = s + m_t_sport_shop.hb_power.ToString();
			m_money_image.transform.Find("hua").gameObject.SetActive(false);
		}
		else
		{
			m_old_price.transform.parent.gameObject.SetActive(false);
		}
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (m_t_sport_shop.type,m_t_sport_shop.value1,m_t_sport_shop.value2,m_t_sport_shop.value3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		m_remai.SetActive(false);
		m_numbers.SetActive(false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		if(m_t_sport_shop.hb_power > 0)
		{
			m_old_price.transform.parent.localPosition = new Vector3(-60f,3f,0);
            m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
			m_gold.transform.parent.localPosition = new Vector3 (-60f,-28f,0);
		}
		m_star.SetActive (false);
		m_jifen.SetActive (false);
	}
    void guild_shop_guanghuan()
    {
        int price = 0;
        int buy_qualification = 0;
        this.transform.Find("buy").gameObject.SetActive(true);
        m_tuijian.SetActive(false);
        m_t_guild_shop_guang = game_data._instance.get_guild_shop_ex(m_shop_id);
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(m_t_guild_shop_guang.type, m_t_guild_shop_guang.value1,
            m_t_guild_shop_guang.value2, m_t_guild_shop_guang.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);

        price = sys._instance.m_self.get_att(e_player_attr.player_contribution);
        buy_qualification = juntuan_gui._instance.m_guild_t.level;
        m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(2);
        m_money_image.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(10);
        m_money_image.transform.Find("hua").gameObject.SetActive(false);

        m_ds = "";
        s_t_resource res = game_data._instance.get_t_resource(2);

        s_t_resource res1 = game_data._instance.get_t_resource(10);
        string s = res.namecolor;
        string s1 = res1.namecolor;
        if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < m_t_guild_shop_guang.jewel)
        {
            s = "[ff0000]";
            m_ds = "[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
        }

        if (price < m_t_guild_shop_guang.gongxian)
        {
            s1 = "[ff0000]";
            m_ds = "[ffc882]" + game_data._instance.get_t_language ("temaihui_card.cs_1026_23");//贡献不足
        }
       
        m_gold.GetComponent<UILabel>().text = s + m_t_guild_shop_guang.jewel.ToString();
        m_money_image.transform.Find("num").GetComponent<UILabel>().text = s1 + m_t_guild_shop_guang.gongxian;

        s = "[1EFD72]";
        if (buy_qualification < m_t_guild_shop_guang.level)
        {
            s = "[ff0000]";
            this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = false;
            this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(false);
        }
        else
        {
            s = "[1EFD72]";
            this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = true;
            this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(true);
        }
        m_remai.SetActive(false);
        if (m_t_guild_shop_guang.type == 7)
        {
            m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_t_guild_shop_guang.type, m_t_guild_shop_guang.value1, m_t_guild_shop_guang.value2, m_t_guild_shop_guang.value3);

        }
        else
        {
            m_name.GetComponent<UILabel>().text = "[f98c20]" + m_t_guild_shop_guang.name;
        }
        this.transform.Find("kuang").gameObject.SetActive(false);
        m_star.SetActive(false);
        if (juntuan_gui._instance.m_guild_t.level >= m_t_guild_shop_guang.level)
        {
            m_numbers.SetActive(false);
        }
        else
        {
            m_numbers.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1072_67"), m_t_guild_shop_guang.level );//[ff0000]军团{0}级开放
            m_numbers.SetActive(true);
        }
        if (m_shell <= 0)
        {
            m_numbers.SetActive(false);
            this.transform.Find("kuang").gameObject.SetActive(true);
            this.transform.Find("buy").gameObject.SetActive(false);
        }
        m_jifen.SetActive(false);
    }
    void mofang_shop()
    {
        int money_type = 28;
        int price = 0;
        this.transform.Find("buy").gameObject.SetActive(true);
        m_tuijian.SetActive(false);
        m_mofang_shop = game_data._instance.get_t_mofang_shop(m_shop_id);
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(m_mofang_shop.type, m_mofang_shop.value1, m_mofang_shop.value2, m_mofang_shop.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);

        price = sys._instance.m_self.get_att(e_player_attr.player_mofang_jifen);
        m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
        m_ds = "";
        s_t_resource res = game_data._instance.get_t_resource(money_type);
        string s = res.namecolor;
        if (price < m_mofang_shop.price)
        {
            s = "[ff0000]";
            m_ds = "[ffc882]" + game_data._instance.get_t_language ("czfp_gui.cs_934_59");//积分不足
        }
        else
        {
            s = res.namecolor;
        }
        m_gold.GetComponent<UILabel>().text = s + m_mofang_shop.price.ToString();
        m_remai.SetActive(false);
        m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name(m_mofang_shop.type, m_mofang_shop.value1, m_mofang_shop.value2, m_mofang_shop.value3);
        this.transform.Find("kuang").gameObject.SetActive(false);
        m_gold.transform.parent.localPosition = new Vector3(-60f, -20f, 0);
        m_name.transform.localPosition = new Vector3(-73f, 25f, 0);
        m_star.SetActive(false);
        if (m_mofang_shop.buycount != 0)
        {
            m_numbers.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1603_67"), m_shell);//[b3fe13]今日可买[-][0aff16]{0}次[-]
            m_numbers.SetActive(true);
        }
        else
        {
            m_numbers.SetActive(false);
        }
      
        if (m_shell <= 0 && m_mofang_shop.buycount != 0)
        {
            m_numbers.SetActive(false);
            this.transform.Find("kuang").gameObject.SetActive(true);
            this.transform.Find("buy").gameObject.SetActive(false);
        }
        m_jifen.SetActive(false);
        m_old_price.transform.parent.gameObject.SetActive(false);

 
    }
	void guild_item_shop()
	{
		int money_type = 0;
		int price = 0;
		int buy_qualification = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_tuijian.SetActive (false);
		m_t_guild_shop =  game_data._instance.get_guild_shop (m_shop_id);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_guild_shop.reward.type,m_t_guild_shop.reward.value1,m_t_guild_shop.reward.value2,m_t_guild_shop.reward.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		
		money_type = 10;
		price = sys._instance.m_self.get_att(e_player_attr.player_contribution);
		buy_qualification = juntuan_gui._instance.m_guild_t.level;
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
		m_ds = "";
		s_t_resource res = game_data._instance.get_t_resource(10);
		string s = res.namecolor;
		if ( price < m_t_guild_shop.gx)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]" + game_data._instance.get_t_language ("temaihui_card.cs_1026_23");//贡献不足
		}
		else
		{
			s = res.namecolor;
		}
		m_gold.GetComponent<UILabel>().text = s + m_t_guild_shop.gx.ToString();
		int num = sys._instance.m_self.get_item_num (red_role_power);
		if(m_t_guild_shop.hb_power > 0)
		{
			m_old_price.transform.parent.gameObject.SetActive(true);
			price = m_t_guild_shop.hb_power;
			if(num < price)
			{
				s = "[ff0000]";
				m_ds = game_data._instance.get_t_language ("temaihui_card.cs_617_11");//[ffc882]红色伙伴之力不足
			}
			else
			{
				s = "[FF8585]";
			}
			m_old_price.transform.GetComponent<UILabel>().text = s + m_t_guild_shop.hb_power.ToString();
			m_money_image.transform.Find("hua").gameObject.SetActive(false);
		}
		else
		{
			m_old_price.transform.parent.gameObject.SetActive(false);
		}
		s = "[1EFD72]";
		if(buy_qualification < m_t_guild_shop.level)
		{
			s = "[ff0000]";
			this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = false;
			this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(false);
		}
		else
		{
			s = "[1EFD72]";
			this.transform.Find("buy").gameObject.GetComponent<BoxCollider>().enabled = true;
			this.transform.Find("buy").gameObject.GetComponent<UISprite>().set_enable(true);
		}
		m_remai.SetActive(false);
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (m_t_guild_shop.reward.type,m_t_guild_shop.reward.value1,m_t_guild_shop.reward.value2,m_t_guild_shop.reward.value3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		if(m_t_guild_shop.hb_power > 0)
		{
			m_old_price.transform.parent.localPosition = new Vector3(-60f,3f,0);
            m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
			m_gold.transform.parent.localPosition = new Vector3 (-60f,-28f,0);
		}
		m_star.SetActive(false);
		if (juntuan_gui._instance.m_guild_t.level >= m_t_guild_shop.level)
		{
			m_numbers.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1603_67") , m_shell );//[b3fe13]今日可买[-][0aff16]{0}次[-]
			m_numbers.SetActive(true);
		}
		else
		{
			m_numbers.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1072_67") , m_t_guild_shop.level);//[ff0000]军团{0}级开放
			m_numbers.SetActive(true);
		}
		if (m_shell <= 0)
		{
			m_numbers.SetActive(false);
			this.transform.Find("kuang").gameObject.SetActive(true);
			this.transform.Find("buy").gameObject.SetActive(false);
		}
		m_jifen.SetActive (false);
	}

	public void guild_xs_shop()
	{
		int money_type = 2;
		int price = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_tuijian.SetActive (false);
		m_t_guild_shop_xs =  game_data._instance.get_t_guild_shop_xs (m_shop_id);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_guild_shop_xs.type,m_t_guild_shop_xs.value1,m_t_guild_shop_xs.value2,m_t_guild_shop_xs.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		price = sys._instance.m_self.get_att (e_player_attr.player_jewel);
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
		m_ds = "";
		string s = "";
		if (price < m_t_guild_shop_xs.jewel)
		{
			s = "[ff0000]";
			m_ds = "0";
		}
		else
		{
			s = sys._instance.get_res_color(money_type);
		}
		
		m_gold.GetComponent<UILabel>().text = s + m_t_guild_shop_xs.jewel.ToString();
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (m_t_guild_shop_xs.type,m_t_guild_shop_xs.value1,m_t_guild_shop_xs.value2,m_t_guild_shop_xs.value3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		m_remai.SetActive(false);
		m_numbers.SetActive(true);
		m_numbers.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1751_58"), m_shell );//[b3fe13]军团剩余[-][0aff16]{0}[-][b3fe13]件[-]
		m_old_price.transform.parent.gameObject.SetActive(false);
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		m_star.SetActive (false);
		bool flag = false;
		for(int i = 0 ; i < sys._instance.m_self.m_t_player.shop1_ids.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.shop1_ids[i] == m_shop_id)
			{
				flag = true;
				break;
			}
		}
		if(flag)
		{
			this.transform.Find("buy").GetComponent<UISprite>().set_enable(false);
			this.transform.Find("buy").GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			this.transform.Find("buy").GetComponent<UISprite>().set_enable(true);
			this.transform.Find("buy").GetComponent<BoxCollider>().enabled = true;
		}
		if(m_t_guild_shop_xs.discount >= 10 || m_t_guild_shop_xs.discount <= 0)
		{
			m_remai.SetActive(false);
		}
		else
		{
			m_remai.SetActive(true);
			m_discount.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"),  sys._instance.discountChange(m_t_guild_shop_xs.discount));//{0}折
		}
		if(m_shell <= 0)
		{
			this.transform.Find("buy").gameObject.SetActive(false);
			this.transform.Find("kuang").gameObject.SetActive (true);
		}
		else
		{
			this.transform.Find("buy").gameObject.SetActive(true);
		}
		m_jifen.SetActive (false);
	}

	void tanbao_shop()
	{
		int price = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_tuijian.SetActive (false);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_tanbao_shop.rtype,m_t_tanbao_shop.rvalue1,m_t_tanbao_shop.rvalue2,m_t_tanbao_shop.rvalue3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);

		price = sys._instance.m_self.get_att (e_player_attr.player_jewel);
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(2);
		m_ds = "";
		string s = "";
		if (price < m_t_tanbao_shop.price)
		{
			s = "[ff0000]";
			m_ds = "[ffc882]" +  game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
		}
		else
		{
			s = sys._instance.get_res_color(2);
		}
		
		m_gold.GetComponent<UILabel>().text = s + m_t_tanbao_shop.price.ToString();
		m_old_price.transform.parent.gameObject.SetActive(false);
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (m_t_tanbao_shop.rtype,m_t_tanbao_shop.rvalue1,m_t_tanbao_shop.rvalue2,m_t_tanbao_shop.rvalue3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		m_remai.SetActive(false);
		m_numbers.SetActive(true);
		m_numbers.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1830_58"), m_shell );//[b3fe13]还可购买:[-][0aff16]{0}[-][b3fe13]次[-]
		if(m_shell <= 0)
		{
			this.transform.Find("buy").gameObject.SetActive(false);
			m_numbers.SetActive(false);
			this.transform.Find("kuang").gameObject.SetActive (true);
		}
		m_gold.transform.parent.localPosition = new Vector3 (-80f,3f,0);
        m_name.transform.localPosition = new Vector3(-89f, m_name.transform.localPosition.y, 0);
		m_jifen.transform.localPosition = new Vector3 (-89,-28,0);
		m_jifen.SetActive (true);
		m_jifen.transform.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1841_66") , m_t_tanbao_shop.score );//[0af6ff]可获得积分:{0}[-]
		float discount = (float)m_t_tanbao_shop.discount / 10;
		if(discount >= 10.0f || discount == 0)
		{
			m_remai.SetActive(false);
		}
		else
		{
			m_remai.SetActive(true);
			discount = (float)decimal.Round ((decimal)discount, 1);
            m_discount.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"), sys._instance.discountChange(discount));//{0}折
		}
		m_star.SetActive (false); 
	}
    void bingyuan_shop()
    {
        int money_type = 24;
        int price = 0;
        this.transform.Find("buy").gameObject.SetActive(true);
        m_t_bingyuan_shop = game_data._instance.get_t_bingyuan_shop(m_shop_id);
        m_tuijian.SetActive(false);
        sys._instance.remove_child(m_icon);
        GameObject _icon = icon_manager._instance.create_reward_icon(m_t_bingyuan_shop.type, m_t_bingyuan_shop.value1, m_t_bingyuan_shop.value2, m_t_bingyuan_shop.value3);

        _icon.transform.parent = m_icon.transform;
        _icon.transform.localScale = new Vector3(1, 1, 1);
        _icon.transform.localPosition = new Vector3(0, 0, 0);

        price = sys._instance.m_self.get_att(e_player_attr.player_bingyuan_point);
        m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
        m_ds = "";
        string s = "";
        if (price < m_t_bingyuan_shop.binjin)
        {
            s = "[ff0000]";
            m_ds = "[ffc882]" + game_data._instance.get_t_language ("equip_buy_gui.cs_1210_75");//冰晶不足
        }
        else
        {
            s = sys._instance.get_res_color(money_type);
        }

        m_gold.GetComponent<UILabel>().text = s + m_t_bingyuan_shop.binjin.ToString();
        m_old_price.transform.parent.gameObject.SetActive(false);
        m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name
            (m_t_bingyuan_shop.type, m_t_bingyuan_shop.value1, m_t_bingyuan_shop.value2, m_t_bingyuan_shop.value3);
        this.transform.Find("kuang").gameObject.SetActive(false);
        m_remai.SetActive(false);
        m_numbers.SetActive(true);
        if (m_t_bingyuan_shop.buy_count != 0)
        {
            m_numbers.SetActive(true);
            m_numbers.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("temaihui_card.cs_1893_67") ,m_shell );//[b3fe13]今日可买:[-][0aff16]{0}[-][b3fe13]次[-]
        }
        else
        {
            m_numbers.SetActive(false);
        }
        if (m_shell <= 0 && m_t_bingyuan_shop.buy_count != 0)
        {
            this.transform.Find("buy").gameObject.SetActive(false);
            m_numbers.SetActive(false);
            this.transform.Find("kuang").gameObject.SetActive(true);
        }
        m_jifen.SetActive(false);
     
        m_star.SetActive(false);
    }
    void bingyuan_reward()
    {

    }
	void chongzhifanpai_shop()
	{
		int money_type = 26;
		int price = 0;
		this.transform.Find("buy").gameObject.SetActive(true);
		m_t_chongzhifanpai_shop = game_data._instance.get_t_chongzhifanpai_shop (m_shop_id);
		m_tuijian.SetActive (false);
		sys._instance.remove_child (m_icon);
		GameObject _icon = icon_manager._instance.create_reward_icon(m_t_chongzhifanpai_shop.type,m_t_chongzhifanpai_shop.value1,m_t_chongzhifanpai_shop.value2,m_t_chongzhifanpai_shop.value3);
		
		_icon.transform.parent = m_icon.transform;
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		
		price = currency;
		m_money_image1.GetComponent<UISprite>().spriteName = sys._instance.get_res_samall_icon(money_type);
		m_ds = "";
		string s = "";
		if (price < m_t_chongzhifanpai_shop.price)
		{
			s = "[ff0000]";
			m_ds = game_data._instance.get_t_language ("temaihui_card.cs_1934_10");//[ffc882]积分不足
		}
		else
		{
			s = sys._instance.get_res_color(money_type);
		}
		m_gold.transform.parent.localPosition = new Vector3 (-60f,-20f,0);
        m_name.transform.localPosition = new Vector3(-73f, m_name.transform.localPosition.y, 0);
		m_gold.GetComponent<UILabel>().text = s + m_t_chongzhifanpai_shop.price.ToString();
		m_old_price.transform.parent.gameObject.SetActive(false);
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (m_t_chongzhifanpai_shop.type,m_t_chongzhifanpai_shop.value1,m_t_chongzhifanpai_shop.value2,m_t_chongzhifanpai_shop.value3);
		this.transform.Find("kuang").gameObject.SetActive (false);
		m_remai.SetActive(false);
		m_numbers.SetActive(false);
		m_jifen.SetActive (false);
		m_remai.SetActive(false);
		m_star.SetActive (false); 
	}

    bool guild_canbuy(int  id)
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
    bool bingyuan_canbuy(int id)
    {
        for (int i = 0; i < sys._instance.m_self.m_t_player.by_rewards.Count; i++)
        {
            if (sys._instance.m_self.m_t_player.by_rewards[i] == id)
            {
                return true;
            }
        }
        return false;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
