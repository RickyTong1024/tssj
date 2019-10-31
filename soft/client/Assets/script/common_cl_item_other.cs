

using UnityEngine;
using System.Collections;

public class common_cl_item_other : MonoBehaviour {	

	public int m_id;
	public int type;
	public GameObject m_button;
	public UILabel m_lock;
	public UILabel  m_name;
	public UILabel m_desc;
	public UISprite m_icon;
	s_t_role_shop t_shop;



	void Start()
	{

	}
	// Use this for initialization
	public void init () 
	{

		m_button.SetActive(true);
		m_lock.gameObject.SetActive (false);
		m_desc.text = "";
		s_t_item t_item = game_data._instance.get_item (m_id);
		if(type == 1)//1 伙伴商店 2 援救伙伴 3 军团商店 4 装备商店 5 装备箱子 6 竞技场 7 夺宝奇兵  8 宝物箱子 9 商城
            //10 魔王商店 11秘境商店 12 副本 13 全线副本 15猎人商店 16命运指针 17命运占卜 18回忆商店 19幸运商店 20精英副本
		{
			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_29_17");//伙伴商店购得
			m_icon.spriteName = "small_hbpy001";
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shop)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 2)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_39_17");//援救伙伴产出
			m_icon.spriteName = "icon_hd_yjhb";
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_hhb)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 3)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_49_17");//军团商店购得
			m_icon.spriteName = "small_jtsd001";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_guild_boss_kaiqi)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
 
            }
			
		}
		else if(type == 4)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_61_17");//秘境商店购得
			m_icon.spriteName = "icon_hd_xhmj";
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mijing)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 5)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_71_17");//商城-装备箱
			m_desc.text = "（" + game_data._instance.get_t_language ("common_cl_item_other.cs_72_23") + "）";//有几率获得
			m_icon.spriteName = "icon_zbbx_jin";
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shop)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 6)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_82_17");//荣誉商店购得
			m_icon.spriteName = "icon_hd_jjc";
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jjc)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 7)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_92_17");//夺宝奇兵中抢夺碎片
			m_icon.spriteName = "icon_bwqd";
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasue_qh)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 8 || type == 9)
		{

			
			if(type == 8)
			{

                m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_105_30");//商城-饰品箱
                m_icon.spriteName = "icon_clbx_jin01";
				m_desc.text =  "（" + game_data._instance.get_t_language ("common_cl_item_other.cs_72_23") + "）";//有几率获得
			}
			if(type == 9)
			{

                m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_111_30");//商城可购得
				m_icon.spriteName = "small_sc001";
			}
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shop)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 10)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_122_17");//魔王商店购得
			m_icon.spriteName = "icon_jt_boss";
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mowang)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 11)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_132_17");//星河秘境掉落
			m_icon.spriteName = "icon_hd_xhmj";
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mijing)
			{

				m_button.SetActive(false);
				m_lock.gameObject.SetActive(true);
			}
		}
		else if(type == 13)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_142_17");//所有主线副本中均掉落
			m_icon.spriteName = "icon_hd_ptfb";
			m_button.SetActive(false);
			m_lock.gameObject.SetActive(false);
		}
        else if (type == 14)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_149_26");//星际宝藏可获得
            m_icon.spriteName = "icon_hd_arfkq";
            m_button.SetActive(true);
            m_lock.gameObject.SetActive(false);
        }
        else if (type == 15)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_156_26");//猎人商店可购得
            m_icon.spriteName = "small_lr001";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pvp)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
        }
        else if (type == 16)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_166_26");//命运指针可获得
            m_icon.spriteName = "small_zbgz001";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
        }
        else if (type == 17)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_176_26");//命运占卜可获得
            m_icon.spriteName = "icon_hd_jyfb";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
        }
        else if (type == 18)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_186_26");//回忆商店可购得
            m_icon.spriteName = "small_hy001";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
        }
        else if (type == 19)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_196_26");//幸运商店可购得
            m_icon.spriteName = "small_xy001";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
        }
        else if (type == 20)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_206_26");//精英副本掉落
            m_icon.spriteName = "icon_hd_jyjyfb";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jy)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
        }
        else if (type == 21)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_216_26");//冰原商店购得
            m_icon.spriteName = "small_jzby";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_bingyuan)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
        }
        else if (type == 22)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_226_26");//宠物商店购得
            m_icon.spriteName = "small_sc001";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_master)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
 
        }
        else if (type == 23)
        {

            m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_237_26");//大师联赛可获得
            m_icon.spriteName = "small_chongwu001";
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_master)
            {

                m_button.SetActive(false);
                m_lock.gameObject.SetActive(true);
            }
 
        }
        else if (type == 24)
        {

            m_name.text = game_data._instance.get_t_language ("bag_gui.cs_424_102");//物品合成
            m_icon.spriteName = "small_zbgz001";
 
        }
		else if (type == 25)
		{

			m_name.text = game_data._instance.get_t_language ("common_cl_item_other.cs_254_17");//跨服军团战获得
			m_icon.spriteName = "jthzn_001";
			
		}
        else if (type == 1001) //物品合成 - 光环合成
        {
            m_name.text = game_data._instance.get_t_language("bag_gui.cs_424_102");//物品合成
            m_icon.spriteName = "small_zbgz001";
        }
		
	}
	
	// Update is called once per frame
	public void click(GameObject obj)
	{

		s_message _message = new s_message ();
		switch(type)
		{
			case 1:
			_message.m_type = "show_shop_other";
			break;
			case 2:
			_message.m_type = "show_huo_dong";
			_message.m_ints.Add(3);
			break;
            case 14:
            _message.m_type = "show_huo_dong";
            _message.m_ints.Add(4);
            break;
            case 3:
            if (sys._instance.m_self.m_t_player.guild == 0)
            {

                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("common_cl_item_other.cs_275_71"));//您还没有加入军团
                return;
            }
            _message.m_type = "show_guild_shop";
            break;
			case 4:
			_message.m_type = "show_shop_equip";
			break;
			case 11:
			_message.m_type = "show_shop_equip_reward";
				break;
			case 5:
			case 8:
			case 9:
			_message.m_type = "show_baoshi_shop";
			break;
			case 6:
			_message.m_type = "show_arena_shop";
			break;
			case 7:
			_message.m_type = "show_huo_dong";
			_message.m_ints.Add(9);
			break;
			case 10:
			_message.m_type = "show_mo_wang_shop";
			break;
            case 15:
            _message.m_type = "show_pvp_shop";
            break;
            case 16:
            _message.m_type = "show_mingyun_zhizhen";
            break;
            case 17:
            _message.m_type = "show_mingyun_zhanbu";
            break;
            case 18:
            _message.m_type = "show_huiyi_shop";
            break;
            case 19:
            _message.m_type = "show_luck_shop";
            break;
            case 20:
			_message.m_type = "show_fu_ben";
            break;
            case 21:
            _message.m_type = "show_bingyuan";
			break;
            case 22:
            _message.m_type = "show_master_gui";
            sys._instance.m_state_flag = 1;
            break;
            case 23:
                _message.m_type = "show_huo_dong";
			_message.m_ints.Add(12);
            break;
            case 24:
            _message.m_type = "show_bag1";
            _message.m_ints.Add(6);
            break;
			case 25:
				if (sys._instance.m_self.m_t_player.guild == 0)
				{

					root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("common_cl_item_other.cs_275_71"));//您还没有加入军团
					return;
				}
				_message.m_type = "show_juntuan_kuafu";
				break;
            case 1001:
                _message.m_type = "show_bag1";
                _message.m_ints.Add(7);
                break;
		}
		cmessage_center._instance.add_message (_message);
		_message = new s_message ();
		_message.m_type = "hide_tupo_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_buzheng";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_huoban";
		cmessage_center._instance.add_message (_message);
		
		_message = new s_message ();
		_message.m_type = "hide_skill_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_jinjie_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_equip_gui";
		cmessage_center._instance.add_message (_message);
		
		_message = new s_message ();
		_message.m_type = "hide_cl_gui";
		cmessage_center._instance.add_message (_message);
		
		_message = new s_message ();
		_message.m_type = "hide_show_unit";
		cmessage_center._instance.add_message (_message);
		
		_message = new s_message ();
		_message.m_type = "scene_show";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_treasure_gui";
		cmessage_center._instance.add_message (_message);

		if(type != 24 && type != 1001)
		{

	        _message = new s_message();
	        _message.m_type = "hide_bag_gui";
	        cmessage_center._instance.add_message(_message);
		}

		_message = new s_message ();
		_message.m_type = "hide_main_gui";
		cmessage_center._instance.add_message (_message);

        _message = new s_message();
        _message.m_type = "hide_buzheng";
        cmessage_center._instance.add_message(_message);

		if(type != 20)
		{

			_message = new s_message ();
			_message.m_type = "hide_map_gui";
			cmessage_center._instance.add_message (_message);
		}

		_message = new s_message ();
		_message.m_type = "hide_equip_jl";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_treasure_jl";
		cmessage_center._instance.add_message (_message);
	
		_message = new s_message ();
		_message.m_type = "hide_dui_xing";
		cmessage_center._instance.add_message (_message);
	
		_message = new s_message ();
		_message.m_type = "hide_mijing_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_equip_sx";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_pet_jj_gui";
		cmessage_center._instance.add_message (_message);
		
		_message = new s_message ();
		_message.m_type = "hide_pet_sx_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_pet_gui";
		cmessage_center._instance.add_message (_message);

		if(type == 20)
		{

			int map_id = 0;
			for(int i = 0;i < sys._instance.m_self.m_t_player.map_ids.Count;i++)
			{

				if (sys._instance.m_self.m_t_player.map_ids[i] > 10000)
				{

					map_id = sys._instance.m_self.m_t_player.map_ids[i];
					
				}
			}
			if(map_id == 0)
			{

				map_id = 10001;
			}
			_message = new s_message ();
			_message.time = 0.1f;
			_message.m_type = "select_map";
			_message.m_ints.Add (map_id);
			cmessage_center._instance.add_message (_message);
		}
	}
}
