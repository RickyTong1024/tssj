using UnityEngine;

public class active : MonoBehaviour {

	public int m_active_id;
	public int m_num;
	public void reset()
	{
		s_t_active t_active = game_data._instance.get_t_active(m_active_id);
		if (mubiao_active_panel.active_done(t_active))
		{
			transform.Find("wcd").GetComponent<UILabel>().text ="[ffffff]" + game_data._instance.get_t_language ("active.cs_19_75");//目标达成
			transform.Find("wcd").transform.localPosition = new Vector3(282,32, 0);
			transform.Find("gou").gameObject.SetActive(true);
			transform.Find("main").gameObject.SetActive(false);
			transform.Find("main1").gameObject.SetActive(true);
			transform.Find("qianwang").gameObject.SetActive(false);
		}
		else
		{
			transform.Find("wcd").GetComponent<UILabel>().text = "[24d0fd]" + m_num.ToString() + "/" + t_active.num.ToString();
			transform.Find("wcd").transform.localPosition = new Vector3(138,-22, 0);
			transform.Find("qianwang").gameObject.SetActive(true);
			if (t_active.id == 100 || t_active.id == 101)
			{
				transform.Find("wcd").GetComponent<UILabel>().text = "";
				transform.Find("qianwang").gameObject.SetActive(false);
			}

			transform.Find("gou").gameObject.SetActive(false);
			transform.Find("main").gameObject.SetActive(true);
			transform.Find("main1").gameObject.SetActive(false);
		}
		string text = game_data._instance.get_t_language ("active.cs_45_16") + ": ";//奖励
		text += sys._instance.get_res_info (t_active.reward.type, t_active.reward.value1, t_active.reward.value2, t_active.reward.value3);
		transform.Find("reward").GetComponent<UILabel>().text = text;
		if (t_active.score != 0) 
		{
			transform.Find("desc").GetComponent<UILabel>().text = t_active.desc + "(" + game_data._instance.get_t_language ("active.cs_50_86") + " x" + t_active.score.ToString () + ")";//toupiao_gui_jifen_1//积分
		}
		else
		{
			transform.Find("desc").GetComponent<UILabel>().text = t_active.desc;
		}
		transform.Find("icon").GetComponent<UISprite>().spriteName = t_active.icon;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public 	void click(GameObject obj)
	{
		if(obj.transform.name == "qianwang")
		{
			int m_select_card_id = 0;
			if (sys._instance.m_self.m_t_player.zhenxing[m_select_card_id] == 0)
			{
				for(int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; i ++)
				{
					if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
					{
						m_select_card_id = i;
						break;
					}
				}
			}
			s_t_active t_active = game_data._instance.get_t_active(m_active_id);
			s_message _mes = new s_message();
			s_message _mes1 = new s_message();
			s_message _mes2 = new s_message();
			switch (t_active.id)
			{
			case  3:
				_mes.m_type = "show_catch_gui";
				cmessage_center._instance.add_message(_mes);

				_mes1.m_type = "hall_anim";
				_mes1.m_string.Add("1");
				cmessage_center._instance.add_message(_mes1);
				
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				break;
			case  200:
				_mes.m_type = "show_recharge";
				cmessage_center._instance.add_message(_mes);
				
				_mes1.m_type = "show_main_gui";
				cmessage_center._instance.add_message(_mes1);
				
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				break;
			case  300:
				_mes.m_type = "show_recharge";
				cmessage_center._instance.add_message(_mes);

				_mes1.m_type = "show_main_gui";
				cmessage_center._instance.add_message(_mes1);

				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				break;
			case  400:
				
				_mes.m_type = "show_fu_ben";
				cmessage_center._instance.add_message (_mes);

				_mes1.time = 0.1f;
				_mes1.m_type = "select_map";
				int map_id1 = 0;
				for(int i = 0;i < sys._instance.m_self.m_t_player.map_ids.Count;i++)
				{
					if (sys._instance.m_self.m_t_player.map_ids[i] < 10000 && sys._instance.m_self.m_t_player.map_ids[i] > map_id1)
					{
						map_id1 = sys._instance.m_self.m_t_player.map_ids[i];
					}
				}
				if(map_id1 == 0)
				{
					map_id1 = 1;
				}
				_mes1.m_ints.Add (map_id1);
				cmessage_center._instance.add_message (_mes1);

				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				break;
			case  500:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jy)
				{
                    root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("active.cs_144_76"),(int)e_open_level.el_jy));//[ffc882]精英副本{0}级开启
					return;
				}

				_mes.m_type = "show_fu_ben";
				cmessage_center._instance.add_message (_mes);
				
				_mes1.time = 0.1f;
				_mes1.m_type = "select_map";
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
				_mes1.m_ints.Add (map_id);
				cmessage_center._instance.add_message (_mes1);
				
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				break;
			case  550:
				_mes.m_type = "show_catch_gui";
				cmessage_center._instance.add_message(_mes);
				
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				break;
			case  600:
				_mes.m_type = "show_buzheng";
				cmessage_center._instance.add_message(_mes);
				
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				break;
			case 700:
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);

				_mes.m_type = "show_buzheng";
				cmessage_center._instance.add_message(_mes);

				break;
			case 750:
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				
				_mes.m_type = "show_buzheng";
				cmessage_center._instance.add_message(_mes);
				
				break;
			case  800:
				if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_tupo)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]"+ string.Format(game_data._instance.get_t_language ("active.cs_204_73"),(int)e_open_level.el_tupo));//伙伴培养{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				
				_mes.m_type = "show_buzheng";
				cmessage_center._instance.add_message(_mes);
				
				break;
			case  830:
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				
				_mes.m_type = "show_buzheng";
				cmessage_center._instance.add_message(_mes);
				
				break;
			case  850:
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				
				_mes.m_type = "show_jd";
				cmessage_center._instance.add_message(_mes);

				_mes1.m_type = "show_main_gui";
				cmessage_center._instance.add_message(_mes1);
				
				break;
			case  900:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jjc)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_236_74"),(int)e_open_level.el_jjc));//竞技场{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				
				_mes.m_type = "show_huo_dong";
				_mes.m_ints.Add(6);
				cmessage_center._instance.add_message(_mes);

				break;
			case  1000:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mowang)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]"+ string.Format(game_data._instance.get_t_language ("active.cs_250_73"),(int)e_open_level.el_mowang));//魔王讨伐{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);

				_mes.m_type = "show_huo_dong";
				_mes.m_ints.Add(1);
				cmessage_center._instance.add_message(_mes);
				break;
			case  1100:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shop)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_263_74") ,(int)e_open_level.el_shop));//商店{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);

				_mes.m_type = "show_shop";
				cmessage_center._instance.add_message(_mes);
				break;
			case  1200:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mijing)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]"+ string.Format(game_data._instance.get_t_language ("active.cs_275_73"),(int)e_open_level.el_mijing));//星河秘境{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);

				_mes.m_type = "show_huo_dong";
				_mes.m_ints.Add(5);
				cmessage_center._instance.add_message(_mes);

				break;
			case  1300:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_transport_ship)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_289_74"),(int)e_open_level.el_transport_ship));//太空运输{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);

				_mes.m_type = "show_huo_dong";
				_mes.m_ints.Add(2);
				cmessage_center._instance.add_message(_mes);

				break;
			case  1500:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_baozang)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_303_74") + (int)e_open_level.el_baozang));//星际宝藏{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);

				_mes.m_type = "show_huo_dong";
				_mes.m_ints.Add(4);
				cmessage_center._instance.add_message(_mes);

				break;
			case  1600:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_hhb)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_317_74"),(int)e_open_level.el_hhb));//援救伙伴{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);

				_mes.m_type = "show_huo_dong";
				_mes.m_ints.Add(3);
				cmessage_center._instance.add_message(_mes);

				break;

			case  1700:
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				
				_mes.m_type = "show_buzheng";
				cmessage_center._instance.add_message(_mes);
				
				break;
			case  1800:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_qu)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_340_74"),(int)e_open_level.el_treasure_qu));//夺宝奇兵{0}级开启
					return;
				}
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				
				_mes.m_type = "show_huo_dong";
				_mes.m_ints.Add(9);
				cmessage_center._instance.add_message(_mes);
				
				break;
			case  1900:
			case  2000:
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shop)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_263_74") ,(int)e_open_level.el_shop));//商店{0}级开启
					return;
				}
				_mes.m_type = "show_baoshi_shop";
				cmessage_center._instance.add_message(_mes);
				
				break;
			case  2100:
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_friend)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_369_74"),(int)e_open_level.el_friend));//好友{0}级开启
					return;
				}
				_mes.m_type = "show_haoyou";
				cmessage_center._instance.add_message(_mes);
				
				break;
			case  2200:
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
                if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
				{
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_381_89"),(int)e_open_level.el_memory));//回忆{0}级开启
					return;
				}
                _mes.m_type = "show_mingyun_zhizhen";
				cmessage_center._instance.add_message(_mes);
				
				break;
            case 2300:
               _mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
				{
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_381_89"),(int)e_open_level.el_memory));//回忆{0}级开启
					return;
				}
                _mes.m_type = "show_mingyun_zhanbu";
				cmessage_center._instance.add_message(_mes);
                break;
			case 2400:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jy)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_402_74") ,(int)e_open_level.el_jy));//奇遇挑战{0}级开启
					return;
				}
				
				_mes.m_type = "show_fu_ben";
				cmessage_center._instance.add_message (_mes);
				
				_mes1.time = 0.1f;
				_mes1.m_type = "select_map";
				map_id = 0;
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
				_mes1.m_ints.Add (map_id);
				cmessage_center._instance.add_message (_mes1);
				
				_mes2.m_type = "hide_mubiao_gui";
				cmessage_center._instance.add_message(_mes2);
				break;
			default:
				break;
			}
		}
	}
		
}
