
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class huo_yue_sub : MonoBehaviour {

	public int m_type;
	public int m_id;
	public int m_need_jewel = 0;
	public int m_self_num = 0;
	public int m_can_get = 0;
	public int m_toltal_count = 0;
	public bool is_end = false;
	public GameObject m_get;
	public GameObject m_ylq;
	public GameObject m_qianwang;
	public GameObject m_desc;
	public GameObject m_num;
	public GameObject m_wdc;
	public List<GameObject> m_icons;
	public List<int> rtype = new List<int>();
	public List<int> rvalue1 = new List<int>();
	public List<int> rvalue2 = new List<int>();
	public List<int> rvalue3 = new List<int>();

	// Use this for initialization
	void Start () {
		
	}
	
	public void reset()
	{
		m_num.GetComponent<UILabel>().text = m_self_num.ToString() + "/" + m_toltal_count.ToString ();
		for (int i = 0; i < rtype.Count; ++i)
		{
			m_icons[i].SetActive(true);
			GameObject _obj = icon_manager._instance.create_reward_icon(rtype[i] ,rvalue1[i] ,rvalue2[i] ,rvalue3[i]);
			
			_obj.transform.parent = m_icons[i].transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,0,0);
		}
		for (int i = rtype.Count; i < 4; ++i)
		{
			m_icons[i].SetActive(false);
		}
		m_wdc.SetActive (false);
		if(m_can_get == 1 )
		{
			m_get.SetActive(false);
			m_ylq.SetActive(true);
			m_qianwang.SetActive(false);
		}
		else
		{
			if(m_self_num < m_toltal_count)
			{
				m_get.SetActive(false);
				m_ylq.SetActive(false);
				m_qianwang.SetActive(true);
			}
			else
			{
				m_get.SetActive(true);
				m_ylq.SetActive(false);
				m_qianwang.SetActive(false);
			}
			if(is_end)
			{
				m_get.SetActive(false);
				m_ylq.SetActive(false);
				m_qianwang.SetActive(false);
				m_wdc.SetActive(true);
			}
		}
		switch(m_type)
		{
			case 100:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_79_55") , m_toltal_count.ToString());//普通副本胜利{0}次
				break;
			case 101:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_82_55") , m_toltal_count.ToString());//挑战魔王{0}次
				break;
			case 102:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_85_55") , m_toltal_count.ToString());//竞技场胜利{0}次
				break;
			case 103:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_88_55") , m_toltal_count.ToString());//进行抢夺{0}次
				break;
			case 104:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_91_55") , m_toltal_count.ToString());//命运指针抽取回忆{0}次
				break;
			case 105:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("幸运探宝积分达到{0}次") , m_toltal_count.ToString());
				break;
			case 106:
			m_desc.GetComponent<UILabel>().text =string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_97_54") , m_toltal_count.ToString());//精英副本胜利{0}次
				break;
			case 107:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_100_55") , m_toltal_count.ToString());//军团副本挑战{0}次
				break;
			case 108:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_103_55") , m_toltal_count.ToString());//猎人大会挑战{0}次
				break;
			case 109:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_106_55") , m_toltal_count.ToString());//决战冰原获得奖励{0}次
				break;
			case 110:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("银河转盘积分达到{0}次") , m_toltal_count.ToString());
				break;
			case 111:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("太空漫步积分达到{0}分") , m_toltal_count.ToString());
				break;
			case 112:
			m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_115_55") , m_toltal_count.ToString());//九宫魔方积分达到{0}分
				break;
            case 113:
                m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_118_68"), m_toltal_count.ToString());//太空运输拦截飞船{0}次
                break;
            case 114:
                m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_121_68"), m_toltal_count.ToString());//太空运输护送企业号{0}次
                break;
            case 115:
                m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_124_68"), m_toltal_count.ToString());//合成橙色饰品{0}个
                break;
            case 116:
                m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_127_68"), m_toltal_count.ToString());//合成红色饰品{0}个
                break;
            case 117:
                m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_130_68"), m_toltal_count.ToString());//刷新伙伴商店{0}次
                break;
            case 118:
                m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_133_68"), m_toltal_count.ToString());//重置主线副本{0}次
                break;


		}
	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "get")
		{
			s_message _message = new s_message();
			_message.m_type = "huodong_huoyue_lj";
			_message.m_ints.Add (m_id);
			cmessage_center._instance.add_message(_message);
		}
		else if (obj.name == "qianwang")
		{
			s_message _message = new s_message ();
			s_message _message1 = new s_message();
			s_message _mess = new s_message();
			s_message _mess1 = new s_message();
			switch(m_type)
			{
				case 100:
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);

				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);

				_message.m_type = "show_fu_ben";
				cmessage_center._instance.add_message (_message);
				
				_message1.time = 0.1f;
				_message1.m_type = "select_map";
				int map_id1 = 0;
				for(int i = 0;i < sys._instance.m_self.m_t_player.map_ids.Count;i++)
				{
					if (sys._instance.m_self.m_t_player.map_ids[i] < 10000)
					{
						map_id1 = sys._instance.m_self.m_t_player.map_ids[i];
						
					}
				}
				if(map_id1 == 0)
				{
					map_id1 = 1;
				}
				_message1.m_ints.Add (map_id1);
				cmessage_center._instance.add_message (_message);
				cmessage_center._instance.add_message (_message1);
					break;
				case 101:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mowang)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]"+ string.Format(game_data._instance.get_t_language ("active.cs_250_73") , (int)e_open_level.el_mowang));//魔王讨伐{0}级开启
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);

				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);

				_message.m_type = "show_huo_dong";
				_message.m_ints.Add(1);
				cmessage_center._instance.add_message(_message);
					break;
				case 102:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jjc)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_185_74") , (int)e_open_level.el_jjc ));//竞技场{0级开启}
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);

				_message.m_type = "show_huo_dong";
				_message.m_ints.Add(6);
				cmessage_center._instance.add_message(_message);
					break;
				case 103:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_qu)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_340_74") , (int)e_open_level.el_treasure_qu  ));//夺宝奇兵{0}级开启
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);

				_message.m_type = "show_huo_dong";
				_message.m_ints.Add(9);
				cmessage_center._instance.add_message(_message);
					break;
				case 104:
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_mingyun_zhizhen";
				cmessage_center._instance.add_message(_message);
					break;
				case 105:
				if(sys._instance.m_self.m_huodong_xhqd == 0 || (sys._instance.m_self.m_huodong_xhqd & 1) == 0)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("huo_yue_sub.cs_227_47"));//[ffc882]活动还未开放，敬请关注
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_tanbao";
				cmessage_center._instance.add_message(_message);
				break;
				case 106:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jy)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_242_74"), (int)e_open_level.el_jy ));//精英副本{0}级开启
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_fu_ben";
				cmessage_center._instance.add_message (_message);
				
				_message1.time = 0.1f;
				_message1.m_type = "select_map";
				int map_id = 0;
				for(int i = 0;i < sys._instance.m_self.m_t_player.map_ids.Count;i++)
				{
					if (sys._instance.m_self.m_t_player.map_ids[i] > 10000 && sys._instance.m_self.m_t_player.map_ids[i] > map_id)
					{
						map_id = sys._instance.m_self.m_t_player.map_ids[i];
					}
				}
				if(map_id == 0)
				{
					map_id = 10001;
				}
				_message1.m_ints.Add (map_id);
				cmessage_center._instance.add_message (_message);
				cmessage_center._instance.add_message (_message1);
				break;
				case 107:
				if (sys._instance.m_self.m_t_player.guild == 0)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("common_cl_item_other.cs_275_71"));//您还没有加入军团
					return;
				}
				/*
				if (juntuan_gui._instance.m_guild_t.level < (int)e_open_level.el_guild_boss_kaiqi)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_280_74") , (int)e_open_level.el_guild_boss_kaiqi ));//军团副本需军团{0}级开启
					return;
				}
				 */
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_juntuan_map";
				cmessage_center._instance.add_message(_message);
					break;
				case 108:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pvp)
				{
					root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_295_61") , (int)e_open_level.el_pvp ));//[ffc882]猎人大会{0}级开启
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_huo_dong";
				_message.m_ints.Add(10);
				cmessage_center._instance.add_message(_message);
				break;
				case 109:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_bingyuan)
				{
					root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_311_61") , (int)e_open_level.el_bingyuan));//[ffc882]决战冰原{0}级开启
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_huo_dong";
				_message.m_ints.Add(11);
				cmessage_center._instance.add_message(_message);
				break;
				case 110:
				if(sys._instance.m_self.m_huodong_xhqd == 0 || (sys._instance.m_self.m_huodong_xhqd & 4) == 0)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("huo_yue_sub.cs_227_47"));//[ffc882]活动还未开放，敬请关注
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_czfp";
				cmessage_center._instance.add_message(_message);
				break;
				case 111:
				if(sys._instance.m_self.m_huodong_xhqd == 0 || (sys._instance.m_self.m_huodong_xhqd & 8) == 0)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("huo_yue_sub.cs_227_47"));//[ffc882]活动还未开放，敬请关注
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_explore_gui";
				cmessage_center._instance.add_message(_message);
				break;
				case 112:
				if(sys._instance.m_self.m_huodong_xhqd == 0 || (sys._instance.m_self.m_huodong_xhqd & 16) == 0)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("huo_yue_sub.cs_227_47"));//[ffc882]活动还未开放，敬请关注
					return;
				}
				_mess.m_type = "hide_jc_huodong";
				cmessage_center._instance.add_message(_mess);
				
				_mess1.m_type = "hide_huoyue_huodong";
				cmessage_center._instance.add_message(_mess1);
				
				_message.m_type = "show_mofang_gui";
				cmessage_center._instance.add_message(_message);
				break;
                case 113:
                if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_transport_ship)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_289_74"), (int)e_open_level.el_transport_ship));//太空运输{0}级开启
                    return;
                }
                _mess.m_type = "hide_jc_huodong";
                cmessage_center._instance.add_message(_mess);

                _mess1.m_type = "hide_huoyue_huodong";
                cmessage_center._instance.add_message(_mess1);

                _message.m_type = "show_huo_dong";
                _message.m_ints.Add(2);
                cmessage_center._instance.add_message(_message);
                break;
                case 114:
                if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_transport_ship)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_289_74"), (int)e_open_level.el_transport_ship));//太空运输{0}级开启
                    return;
                }
                _mess.m_type = "hide_jc_huodong";
                cmessage_center._instance.add_message(_mess);

                _mess1.m_type = "hide_huoyue_huodong";
                cmessage_center._instance.add_message(_mess1);

                _message.m_type = "show_huo_dong";
                _message.m_ints.Add(2);
                cmessage_center._instance.add_message(_message);
                break;
                case 115:
                if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_qu)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_340_74"), (int)e_open_level.el_treasure_qu));//夺宝奇兵{0}级开启
                    return;
                }
                _mess.m_type = "hide_jc_huodong";
                cmessage_center._instance.add_message(_mess);

                _mess1.m_type = "hide_huoyue_huodong";
                cmessage_center._instance.add_message(_mess1);

                _message.m_type = "show_huo_dong";
                _message.m_ints.Add(9);
                cmessage_center._instance.add_message(_message);
                break;
                case 116:
                if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_qu)
                {
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_340_74"), (int)e_open_level.el_treasure_qu));//夺宝奇兵{0}级开启
                    return;
                }
                _mess.m_type = "hide_jc_huodong";
                cmessage_center._instance.add_message(_mess);

                _mess1.m_type = "hide_huoyue_huodong";
                cmessage_center._instance.add_message(_mess1);

                _message.m_type = "show_huo_dong";
                _message.m_ints.Add(9);
                cmessage_center._instance.add_message(_message);
                break;
                case 117:
                      _mess.m_type = "hide_jc_huodong";
                cmessage_center._instance.add_message(_mess);

                _mess1.m_type = "hide_huoyue_huodong";
                cmessage_center._instance.add_message(_mess1);

                _message.m_type = "show_shop_other";
                cmessage_center._instance.add_message(_message);
                    
                break;
                case 118:
                _mess.m_type = "hide_jc_huodong";
                cmessage_center._instance.add_message(_mess);

                _mess1.m_type = "hide_huoyue_huodong";
                cmessage_center._instance.add_message(_mess1);

                _message.m_type = "show_fu_ben";
				cmessage_center._instance.add_message (_message);

                break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
