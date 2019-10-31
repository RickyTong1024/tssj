
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mi_jing_gui : MonoBehaviour, IMessage {
	
	// Use this for initialization
	public static mi_jing_gui _instance;
	public GameObject m_tj;
	public GameObject m_last_star;
	public GameObject m_cur_star;
	public GameObject m_shuxing;
	public GameObject m_guan;
	public GameObject m_stars;
	public GameObject m_ysx;
	public GameObject m_t1;
	public GameObject m_t2;
	public GameObject m_t3;
	public GameObject m_jl;
	public GameObject m_ph;
	public GameObject m_nd;
	public GameObject m_jc;
	public GameObject m_down;
	public GameObject m_discout;
	public GameObject m_mibao_discout;
	private GameObject m_unit;
	public int m_jc_index;
	public GameObject m_dlg;
	public bool m_daily_refresh;
	public GameObject m_tg;
	public GameObject m_shop_gui;
	public GameObject m_mowang_have;
	public GameObject m_mibao;
	public GameObject m_mibao_gui;
	private s_t_ttt_mibao _t_ttt_mibao;
	public GameObject m_star;
	public GameObject m_icon;
	public GameObject m_name;
	public GameObject m_old_price;
	public GameObject m_now_price;
	public GameObject m_icon1;
    public GameObject m_saodang;
	public GameObject m_effect;
	public GameObject m_ronglian_gui;
	public GameObject m_shenlian;



	void Awake()
	{
		_instance = this;
	}

	void Start () {
	
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void IMessage.message (s_message message)
	{
		if (message.m_type == "mi_jing_cz")
		{
			int _count = (int)message.m_ints[0];
			if (_count > sys._instance.m_self.get_att(e_player_attr.player_jewel))
			{
				string s = game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59");//钻石不足
				root_gui._instance.show_prompt_dialog_box("[ffc882]"+s);
				return;
			}
			
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_TTT_CZ, _msg);
		}
		if (message.m_type == "daily_refresh")
		{
			if (this.gameObject.activeSelf)
			{
				reset ();
			}
			else
			{
				m_daily_refresh = true;
			}
		}
		if(message.m_type == "show_equip_mj_shop")
		{
			if(m_mibao_gui.activeSelf)
			{
				m_mibao_gui.SetActive(false);
			}
			m_shop_gui.SetActive(true);
			m_shop_gui.GetComponent<mijing_shop_gui>().equip_shop();
		}
		if (message.m_type == "show_equip_mj_reward_shop") 
		{
			if(m_mibao_gui.activeSelf)
			{
				m_mibao_gui.SetActive(false);
			}
			m_shop_gui.SetActive(true);
			m_shop_gui.GetComponent<mijing_shop_gui>().reward_shop();
		}
		if (message.m_type == "reset_mijing_gui")
		{
			reset ();
		}
		if(message.m_type == "buy_mibao")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_TTT_BAOZANG, _msg);
		}
		if(message.m_type == "refresh_mijing_gui")
		{
			if(mijing_shop_gui.is_effect())
			{
				m_effect.SetActive(true);
			}
			else
			{
				m_effect.SetActive(false);
			}
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_TTT_FIGHT_END)
		{
			protocol.game.smsg_ttt_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_ttt_fight_end> (message.m_byte);

			sys._instance.m_game_state = "buttle";
			
			battle_logic_ex._instance.set_ttt_fight_end(_msg);
			
			sys._instance.load_scene_ex("ts_fight_mijing");

			int index = sys._instance.m_self.m_t_player.ttt_cur_stars.Count;
			if (_msg.result != 1)
			{
				sys._instance.m_self.m_t_player.ttt_dead = 1;
			}
			else
			{
				sys._instance.m_self.m_t_player.ttt_cur_stars.Add(_msg.nd);
				sys._instance.m_self.m_t_player.ttt_star += _msg.nd;
				sys._instance.m_self.zs_skill_target(5,_msg.nd);
				if (index >= sys._instance.m_self.m_t_player.ttt_last_stars.Count)
				{
					sys._instance.m_self.m_t_player.ttt_last_stars.Add(_msg.nd);
				}
				else if (_msg.nd > sys._instance.m_self.m_t_player.ttt_last_stars[index])
				{
					sys._instance.m_self.m_t_player.ttt_last_stars[index] = _msg.nd;
				}
				s_t_ttt t_ttt = game_data._instance.get_t_ttt(index + 1);
				int reward_mod = (index + 1) % 3;
				if (reward_mod == 0)
				{
					sys._instance.m_self.m_t_player.ttt_can_reward = 1;
				}
			}
			sys._instance.m_self.m_t_player.ttt_mibao = _msg.mibao;
			if(_msg.mibao != 0)
			{
				_t_ttt_mibao = game_data._instance.get_t_ttt_mibao(_msg.mibao);
				sys._instance.remove_child(m_icon1);
				GameObject _icon = icon_manager._instance.create_reward_icon(_t_ttt_mibao.type ,_t_ttt_mibao.value1 ,_t_ttt_mibao.value2 ,_t_ttt_mibao.value3);
				_icon.transform.parent = m_icon1.transform;
				_icon.transform.localPosition = new Vector3(0,0,0);
				_icon.transform.localScale = new Vector3(1,1,1);
				UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
				meses[0].target = this.gameObject;
				meses[0].functionName = "select";
				meses[1].target = this.gameObject;
				meses[1].functionName = "";
				meses[2].target = null;
				meses[2].functionName = "";
				float discount = (float) _t_ttt_mibao.now_price /(float) _t_ttt_mibao.old_price;
				discount = (float)decimal.Round ((decimal)discount, 2) *10;
				if(discount > 0 && discount < 10)
				{
					m_discout.transform.parent.gameObject.SetActive(true);
                    m_discout.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"), sys._instance.discountChange(discount));//{0}折扣
				}
				else
				{
					m_discout.transform.parent.gameObject.SetActive(false);
				}
				m_mibao.SetActive(true);
                select();
			}
			sys._instance.m_self.m_t_player.ttt_task_num++;
			sys._instance.m_self.add_active(1200, 1);
			sys._instance.m_self.check_target_done();
		}
		if (message.m_opcode == opclient_t.CMSG_TTT_VALUE_LOOK)
		{
			protocol.game.smsg_ttt_value_look _msg = net_http._instance.parse_packet<protocol.game.smsg_ttt_value_look> (message.m_byte);
			sys._instance.m_self.m_t_player.ttt_cur_reward_ids.Clear();
			for (int i = 0; i < _msg.ids.Count; ++i)
			{
				sys._instance.m_self.m_t_player.ttt_cur_reward_ids.Add(_msg.ids[i]);
			}
			m_jc.SetActive(true);
		}
		if (message.m_opcode == opclient_t.CMSG_TTT_VALUE)
		{
			int star = (m_jc_index + 1) * 3;
			sys._instance.m_self.m_t_player.ttt_star -= star;
			sys._instance.m_self.m_t_player.ttt_reward_ids.Add(sys._instance.m_self.m_t_player.ttt_cur_reward_ids[m_jc_index]);
			sys._instance.m_self.m_t_player.ttt_cur_reward_ids.Clear();
			m_shuxing.GetComponent<UILabel>().text = get_jc_string();
			if(get_jc_count() >=6)
			{
				m_down.SetActive(true);
			}
			else
			{
				m_down.SetActive(false);
			}
		}

		if (message.m_opcode == opclient_t.CMSG_TTT_CZ)
		{
			int num = sys._instance.m_self.m_t_player.ttt_cz_num;
			s_t_price _price = game_data._instance.get_t_price(num + 1);
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, _price.ttt_cz,game_data._instance.get_t_language ("mi_jing_gui.cs_233_74"));//秘境充值消耗
			sys._instance.m_self.m_t_player.ttt_dead = 0;
			sys._instance.m_self.m_t_player.ttt_star = 0;
            sys._instance.m_self.m_t_player.ttt_mibao = 0;
			sys._instance.m_self.m_t_player.ttt_reward_ids.Clear ();
			sys._instance.m_self.m_t_player.ttt_cur_stars.Clear ();
			sys._instance.m_self.m_t_player.ttt_cur_reward_ids.Clear ();
			sys._instance.m_self.m_t_player.ttt_cz_num++;
			m_mibao.SetActive(false);
			reset();
		}
		if (message.m_opcode == opclient_t.CMSG_TTT_REWARD)
		{
			int index = sys._instance.m_self.m_t_player.ttt_cur_stars.Count;
			int reward_index = (index - 1) / 3 + 1;
			s_t_ttt_reward t_ttt_reward = game_data._instance.get_t_ttt_reward(reward_index);
			int star = 0;
			for (int i = 1; i <= 3; ++i)
			{
				star += sys._instance.m_self.m_t_player.ttt_cur_stars[index - i];
			}
			star = star / 3 - 1;
			for (int i = 0; i < t_ttt_reward.rewardss[star].Count; ++i)
			{
				if (sys._instance.is_hide_reward(t_ttt_reward.rewardss[star][i].type,t_ttt_reward.rewardss[star][i].value1))
				{
					continue;
				}
				sys._instance.m_self.add_reward(t_ttt_reward.rewardss[star][i].type, t_ttt_reward.rewardss[star][i].value1,
				                                t_ttt_reward.rewardss[star][i].value2, t_ttt_reward.rewardss[star][i].value3,game_data._instance.get_t_language ("mi_jing_gui.cs_262_113"));//秘境奖励
			}
			sys._instance.m_self.m_t_player.ttt_can_reward = 0;
			if (index / 3 > sys._instance.m_self.m_t_player.ttt_reward_ids.Count)
			{
				reset_value();
			}
		}
		if(message.m_opcode == opclient_t.CMSG_TTT_BAOZANG)
		{
			protocol.game.smsg_huodong_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_reward> (message.m_byte);
			for(int i = 0;i < _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i]);
			}
			for(int i = 0;i < _msg.equips.Count;i++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i]);
			}
			for(int i = 0;i < _msg.roles.Count;i++)
			{
				sys._instance.m_self.add_card(_msg.roles[i]);
			}
            for (int i = 0; i < _msg.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_msg.pets[i]);
            }
			for(int i = 0;i < _msg.types.Count;i++)
			{
				sys._instance.m_self.add_reward(_msg.types[i],_msg.value1s[i],
				                               _msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("mi_jing_gui.cs_292_67"));//秘境扫荡
			}
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, _t_ttt_mibao.now_price, game_data._instance.get_t_language ("mi_jing_gui.cs_294_93"));//秘境扫荡消耗
			sys._instance.m_self.m_t_player.ttt_mibao = 0;
			m_mibao_gui.transform.Find("frame_big").GetComponent<frame>().hide();
			m_mibao.SetActive(false);
		}
        if (message.m_opcode == opclient_t.CMSG_TTT_SANXING)
        {
            protocol.game.smsg_ttt_sanxing _msg = net_http._instance.parse_packet<protocol.game.smsg_ttt_sanxing>(message.m_byte);
            for (int i = 0; i < _msg.subs.Count; i++)
            {
                if (i < _msg.subs.Count - 1)
                {
                    sys._instance.m_self.m_t_player.ttt_cur_stars.Add(_msg.subs[i].nd);
                    sys._instance.m_self.m_t_player.ttt_star += _msg.subs[i].nd;
					sys._instance.m_self.add_active(1200, 1);
					sys._instance.m_self.m_t_player.ttt_task_num++;
					sys._instance.m_self.zs_skill_target(5,3);
                }
               
                for(int j = 0;j < _msg.subs[i].types.Count; j ++)
                {
                    sys._instance.m_self.add_reward(_msg.subs[i].types[j], _msg.subs[i].value1s[j], _msg.subs[i].value2s[j], _msg.subs[i].value3s[j],false,game_data._instance.get_t_language ("mi_jing_gui.cs_315_155"));//秘境三星获得
                }

            }
            sys._instance.m_self.m_t_player.ttt_can_reward = 0;
            m_saodang.GetComponent<ttt_saodang>().init(_msg.subs, 0);
            m_saodang.SetActive(true); 
        }
		if (message.m_opcode == opclient_t.CMSG_TTT_SAODANG)
		{
			protocol.game.smsg_ttt_sanxing _msg = net_http._instance.parse_packet<protocol.game.smsg_ttt_sanxing>(message.m_byte);
			for (int i = 0; i < _msg.subs.Count; i++)
			{
				
				{
                    if (_msg.subs[i].index != 0)
                    {
                        sys._instance.m_self.m_t_player.ttt_cur_stars.Add(_msg.subs[i].nd);
                        sys._instance.m_self.m_t_player.ttt_star += _msg.subs[i].nd;
                        sys._instance.m_self.add_active(1200, 1);
                        sys._instance.m_self.m_t_player.ttt_task_num++;
						sys._instance.m_self.zs_skill_target(5,3);
                    }

					
				}
				for(int j = 0;j < _msg.subs[i].types.Count; j ++)
				{
					sys._instance.m_self.add_reward(_msg.subs[i].types[j], _msg.subs[i].value1s[j], _msg.subs[i].value2s[j], _msg.subs[i].value3s[j],false,game_data._instance.get_t_language ("mi_jing_gui.cs_343_140"));//秘境扫荡获得
				}
				if (_msg.subs[i].index == 0)
				{
					s_t_ttt_value t_ttt_value = game_data._instance.get_t_ttt_value(_msg.subs[i].nd);
					sys._instance.m_self.m_t_player.ttt_reward_ids.Add(t_ttt_value.id);
					sys._instance.m_self.m_t_player.ttt_star -= t_ttt_value.xh;
				}
			}
			sys._instance.m_self.m_t_player.ttt_can_reward = 0;
			m_saodang.GetComponent<ttt_saodang>().init(_msg.subs, 1);
			m_saodang.SetActive(true); 

			m_shuxing.GetComponent<UILabel>().text = get_jc_string();
			if(get_jc_count() >=6)
			{
				m_down.SetActive(true);
			}
			else
			{
				m_down.SetActive(false);
			}
		}
	}

	public void OnEnable()
	{
		reset ();
	}

	public void reset()
	{
		m_daily_refresh = false;
		sys._instance.play_mus ("music/login_music");
		update_bottom ();
		int index = sys._instance.m_self.m_t_player.ttt_cur_stars.Count;
		s_t_ttt t_ttt = game_data._instance.get_t_ttt (index + 1);
		if (t_ttt == null)
		{
			m_tj.GetComponent<UILabel>().text = "--";
			m_guan.GetComponent<UILabel>().text = "--";
			m_tg.SetActive(true);
		}
		else
		{
			m_tj.GetComponent<UILabel>().text = t_ttt.tj;
			m_guan.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("boss.cs_234_75"),(index + 1).ToString()) ;//第{0}关
			m_tg.SetActive(false);
		}
		int star = 0;
		for (int i = 0; i < sys._instance.m_self.m_t_player.ttt_cur_stars.Count; ++i)
		{
			star += sys._instance.m_self.m_t_player.ttt_cur_stars[i];
		}
		m_cur_star.GetComponent<UILabel>().text = star.ToString();
		star = 0;
		for (int i = 0; i < sys._instance.m_self.m_t_player.ttt_last_stars.Count; ++i)
		{
			star += sys._instance.m_self.m_t_player.ttt_last_stars[i];
		}
		m_last_star.GetComponent<UILabel>().text = star.ToString();
		star = 0;
        for (int i = index; i < sys._instance.m_self.m_t_player.ttt_last_stars.Count && i < index + 3; i++)
        {
            star += sys._instance.m_self.m_t_player.ttt_last_stars[i];
 
        }
		//m_stars.GetComponent<UISprite>().width = star * 32;
		if (star != 9)
		{
            m_ysx.GetComponent<UIButton>().isEnabled = true;
		}
		else
		{
            m_ysx.GetComponent<UIButton>().isEnabled = true;
		}
		m_shuxing.GetComponent<UILabel>().text = get_jc_string();
		if(get_jc_count() >=6)
		{
			m_down.SetActive(true);
		}
		else
		{
			m_down.SetActive(false);
		}
		if (t_ttt == null)
		{
			m_dlg.GetComponent<UILabel>().text = "";

			if(m_unit != null)
			{
				GameObject.Destroy(m_unit);
			}
		}
		else
		{
			int reward_index = sys._instance.m_self.m_t_player.ttt_reward_ids.Count;
			if (sys._instance.m_self.m_t_player.ttt_can_reward == 1)
			{
				m_jl.SetActive(true);
			}
			else if (index / 3 > reward_index)
			{
				reset_value();
			}
			m_dlg.GetComponent<UILabel>().text = t_ttt.desc;

			List<int> _ids = new List<int>();

			_ids.Add(index);

			if (sys._instance.m_self.m_t_player.ttt_cur_stars.Count > index)
			{
				_ids.Add(sys._instance.m_self.m_t_player.ttt_cur_stars[index]);
			}
			else
			{
				_ids.Add(0);
			}

			_ids.Add((index % 3) + 1);

			for(int i = index / 3 * 3;i < index / 3 * 3 + 3;i ++)
			{
				if(i != index)
				{
					_ids.Add(i);
					if (sys._instance.m_self.m_t_player.ttt_cur_stars.Count > i)
					{
						_ids.Add(sys._instance.m_self.m_t_player.ttt_cur_stars[i]);
					}
					else
					{
						_ids.Add(0);
					}

					_ids.Add((i % 3) + 1);
				}
			}
				
			s_message _msg = new s_message();

			_msg.m_type = "mj_show_unit";
			_msg.time = 0.5f;

			for(int i = 0;i < _ids.Count;i += 3)
			{
				s_t_ttt _t_ttt = game_data._instance.get_t_ttt (_ids[i] + 1);

				if(_t_ttt != null)
				{
					s_t_ttt_guai _t_ttt_guai = _t_ttt.guais[0];

					s_t_monster _monster = game_data._instance.get_t_monster (_t_ttt_guai.id1);

					_msg.m_ints.Add(_monster.class_id);
					_msg.m_ints.Add(_ids[i + 1]);
					_msg.m_ints.Add(_ids[i + 2]);
				}
			}

			cmessage_center._instance.add_message(_msg);
		}
		if(sys._instance.m_self.m_t_player.ttt_mibao!=0)
		{
			m_mibao.SetActive(true);
			_t_ttt_mibao = game_data._instance.get_t_ttt_mibao(sys._instance.m_self.m_t_player.ttt_mibao);
			sys._instance.remove_child(m_icon1);
			GameObject _icon = icon_manager._instance.create_reward_icon(_t_ttt_mibao.type ,_t_ttt_mibao.value1 ,_t_ttt_mibao.value2 ,_t_ttt_mibao.value3);
			_icon.transform.parent = m_icon1.transform;
			_icon.transform.localPosition = new Vector3(0,0,0);
			_icon.transform.localScale = new Vector3(1,1,1);
			UIButtonMessage[] meses = _icon.transform.GetComponents<UIButtonMessage>();
			meses[0].target = this.gameObject;
			meses[0].functionName = "select";
			meses[1].target = this.gameObject;
			meses[1].functionName = "";
			meses[2].target = null;
			meses[2].functionName = "";
			float discount = (float) _t_ttt_mibao.now_price /(float) _t_ttt_mibao.old_price;
			discount = (float)decimal.Round ((decimal)discount, 2) *10;
			if(discount > 0 && discount < 10)
			{
				m_discout.transform.parent.gameObject.SetActive(true);
                m_discout.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"), sys._instance.discountChange(discount));//{0}折扣
			}
			else
			{
				m_discout.transform.parent.gameObject.SetActive(false);
			}
            select();
		}
		else
		{
			m_mibao.SetActive(false);
		}
		if(mijing_shop_gui.is_effect())
		{
			m_effect.SetActive(true);
		}
		else
		{
			m_effect.SetActive(false);
		}
		if(sys._instance.m_self.m_t_player.level >= (int)e_open_see.es_equip_shenglian)
		{
			m_shenlian.SetActive(true);
		}
		else
		{
			m_shenlian.SetActive(false);
		}
	}

	void update_bottom()
	{
		int index = sys._instance.m_self.m_t_player.ttt_cur_stars.Count;
		s_t_ttt t_ttt = game_data._instance.get_t_ttt (index + 1);
		if (t_ttt == null)
		{
			m_t1.SetActive(false);
			m_t2.SetActive(true);
			m_t3.SetActive(false);
		}
		else if (sys._instance.m_self.m_t_player.ttt_dead == 0)
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_see.es_mijing_sanxin_sao && sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_mijing_sanxin_sao)
			{
				m_t1.SetActive(true);
				m_t2.SetActive(false);
				m_t3.SetActive(false);
			}
			else
			{
				m_t1.SetActive(false);
				m_t2.SetActive(false);
				m_t3.SetActive(true);
			}
		}
		else
		{
			m_t1.SetActive(false);
			m_t2.SetActive(true);
			m_t3.SetActive(false);
		}
	}

	void reset_value()
	{
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_TTT_VALUE_LOOK, _msg);
	}

	public void click(GameObject obj)
	{
		if(obj.name == "close")
		{
			if(m_shop_gui.activeSelf)
			{
				m_shop_gui.GetComponent<mijing_shop_gui>().m_shop.GetComponent<UIToggle>().value = true;
				m_shop_gui.transform.Find("frame_big").GetComponent<frame>().hide();
				root_gui._instance.m_mi_jing_shop = 0;
				if (sys._instance.m_message_type.Count != 0)
				{
					s_message m_message = new s_message();
					m_message.m_type = "show_huo_dong";
					m_message.m_ints.Add(5);
					m_message.m_bools.Add(false);
					cmessage_center._instance.add_message(m_message);
					sys._instance.m_game_state = "hall";
					sys._instance.load_scene(sys._instance.m_hall_name);
					s_message _message1 = new s_message();
					_message1.m_type = "show_main_gui";
					cmessage_center._instance.add_message(_message1);
				}
				return;
			}
			s_message _message = new s_message();
			_message.m_type = "show_huo_dong";
			_message.m_ints.Add(5);
			_message.m_bools.Add(false);
			cmessage_center._instance.add_message(_message);

			_message = new s_message ();
			_message.m_type = "end_battle_close_up";
			cmessage_center._instance.add_message (_message);
			
			sys._instance.m_game_state = "hall";
			sys._instance.load_scene(sys._instance.m_hall_name);
			
			this.gameObject.SetActive(false);

			GameObject.Destroy(m_unit);
		}
		if (obj.name == "jl")
		{
			m_jl.SetActive(true);
		}
		if (obj.name == "ph")
		{
			m_ph.SetActive(true);
		}
		if (obj.name == "zd")
		{
			m_nd.SetActive(true);
		}
		if (obj.name == "yj")
		{
            int index = sys._instance.m_self.m_t_player.ttt_cur_stars.Count;
            bool flag = true;
            for (int i = index;  i < index + 3; i++)
            {
                if(i > sys._instance.m_self.m_t_player.ttt_last_stars.Count - 1)
                {
                    flag = false;
                    break;
                }
                if (sys._instance.m_self.m_t_player.ttt_last_stars[i] != 3)
                {
                    flag = false;
                }
                if ((i + 1) % 3 == 0)
                {
                    break;
                }

            }
            if (flag)
            {
				if (sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_mj_yj && sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mj_yj)
                {
                    root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("arena.cs_747_35") ,(int)e_open_vip.ev_mj_yj , (int)e_open_level.el_mj_yj ));//[ffc882]该功能VIP{0}或者达到{1}级开启
                    return;
                }
                protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
                net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_TTT_SANXING, _msg);

            }
            else
            {
            	root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("mi_jing_gui.cs_686_55"));//[ffc882]主人，前方敌人过于危险，需要手动前进
            }
			
		}
		if (obj.name == "sxs")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mijing_sanxin_sao && sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_mijing_sanxin_sao)
			{
				
                root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("arena.cs_747_35"), (int)e_open_vip.ev_mijing_sanxin_sao, (int)e_open_level.el_mijing_sanxin_sao));//[ffc882]该功能VIP{0}或者达到{1}级开启

				return;
			}
			int index = sys._instance.m_self.m_t_player.ttt_cur_stars.Count;
			int index1 = sys._instance.m_self.m_t_player.ttt_last_stars.Count;
			for (int i = index;  i < sys._instance.m_self.m_t_player.ttt_last_stars.Count; i++)
			{
				if (sys._instance.m_self.m_t_player.ttt_last_stars[i] != 3)
				{
					index1 = i;
					break;
				}				
			}
			if (index == index1)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("mi_jing_gui.cs_686_55"));//[ffc882]主人，前方敌人过于危险，需要手动前进
				return;
			}
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
			net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_TTT_SAODANG, _msg);
		}
		if (obj.name == "cz")
		{
			s_t_vip t_vip = game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip);
			int num = sys._instance.m_self.m_t_player.ttt_cz_num;
			if (t_vip.ttt_cz_num <= num)
			{
				string s = game_data._instance.get_t_language ("mi_jing_gui.cs_723_15");//今日重置次数已经用完，提升vip等级可增加重置次数
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + s);
				return;
			}
			string tishi1 = game_data._instance.get_t_language ("arena.cs_104_45");//提示
			s_t_price _price = game_data._instance.get_t_price(num + 1);
			string  _des = string.Format(game_data._instance.get_t_language ("mi_jing_gui.cs_729_32") , _price.ttt_cz , ((int)t_vip.ttt_cz_num - num));//是否花费[00ffff]{0}钻石[-]重置星河秘境，您今日还可重置{1}次
			s_message _message = new s_message();
			_message.m_type = "mi_jing_cz";
			_message.m_ints.Add(_price.ttt_cz);
			root_gui._instance.show_select_dialog_box(tishi1,_des,_message);
		}
		if(obj.transform.name == "shop")
		{
			if(m_mibao_gui.activeSelf)
			{
				m_mibao_gui.SetActive(false);
			}
			m_shop_gui.SetActive(true);
		}
		if(obj.transform.name == "mibao")
		{
			m_mibao_gui.SetActive(true);
		}
		if(obj.transform.name == "mibao_close")
		{
			m_mibao_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "mibao_buy")
		{ 
			//m_mibao_gui.transform.Find("frame_big").GetComponent<frame>().hide();
			if(sys._instance.m_self.get_att(e_player_attr.player_jewel) < _t_ttt_mibao.now_price)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
				return ;
			}
			string item_num = sys._instance.m_self.get_item_num(_t_ttt_mibao.type,_t_ttt_mibao.value1,_t_ttt_mibao.value2,_t_ttt_mibao.value3);
			string _des = string.Format(game_data._instance.get_t_language ("item_shop_gui.cs_117_31") , _t_ttt_mibao.now_price//是否花费[00ffff]{0}钻石[-]购买[00ff00][{1}][-]?
				,sys._instance.get_res_info(_t_ttt_mibao.type,_t_ttt_mibao.value1,_t_ttt_mibao.value2,_t_ttt_mibao.value3));
			if(item_num != "")
			{
				_des += "\n"+ game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ item_num;//当前拥有：
			}
			s_message _msg = new s_message();
			_msg.m_type = "buy_mibao";
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_msg);//提示
		}
		if(obj.name == "shenglian")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_equip_shenglian)
			{
				root_gui._instance.show_prompt_dialog_box(string.Format(game_data._instance.get_t_language ("mi_jing_gui.cs_774_60") ,(int)e_open_level.el_equip_shenglian ));//[ffc882]装备圣炼{0}级开启
				return;
			}
			m_ronglian_gui.GetComponent<equip_ronglian_gui>().m_id = 0;
			m_ronglian_gui.GetComponent<equip_ronglian_gui>().m_guid = 0;
			m_ronglian_gui.GetComponent<equip_ronglian_gui>().reset(0);
			m_ronglian_gui.SetActive(true);
		}
	}

	public void select()
	{
		m_mibao_gui.SetActive(true);
		mi_bao ();
	}

	public void mi_bao()
	{
		int all_star = 0;
		for(int i = 0; i < sys._instance.m_self.m_t_player.ttt_cur_stars.Count;++i)
		{
			all_star += sys._instance.m_self.m_t_player.ttt_cur_stars[i];
		}
		m_star.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("mi_jing_gui.cs_797_55"), all_star.ToString ());//恭喜您本次达到{0}星并发现了特别的秘宝
		sys._instance.remove_child (m_icon);
		GameObject _obj = icon_manager._instance.create_reward_icon(_t_ttt_mibao.type ,_t_ttt_mibao.value1 ,_t_ttt_mibao.value2 ,_t_ttt_mibao.value3);
		_obj.transform.parent = m_icon.transform;
		_obj.transform.localScale = new Vector3(1,1,1);
		_obj.transform.localPosition = new Vector3(0,0,0);
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_t_ttt_mibao.type ,_t_ttt_mibao.value1 ,_t_ttt_mibao.value2 ,_t_ttt_mibao.value3);
		m_old_price.GetComponent<UILabel>().text = _t_ttt_mibao.old_price.ToString();
		m_now_price.GetComponent<UILabel>().text = _t_ttt_mibao.now_price.ToString();
		float discount = (float) _t_ttt_mibao.now_price /(float) _t_ttt_mibao.old_price;
		discount = (float)decimal.Round ((decimal)discount, 2) * 10;
		if(discount > 0 && discount < 10)
		{
            m_mibao_discout.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("kaifu_sub_ex.cs_65_56"), sys._instance.discountChange(discount));//{0}折扣
			m_mibao_discout.transform.parent.gameObject.SetActive(true);
		}
		else
		{
			m_mibao_discout.transform.parent.gameObject.SetActive(false);
		}

	}
	public static string get_jc_string()
	{
		string s = "";
		Dictionary<int, int> dic = new Dictionary<int, int>();
		for (int i = 0; i < sys._instance.m_self.m_t_player.ttt_reward_ids.Count; ++i)
		{
			int id = sys._instance.m_self.m_t_player.ttt_reward_ids[i];
			s_t_ttt_value t_ttt_value = game_data._instance.get_t_ttt_value(id);
			if (dic.ContainsKey(t_ttt_value.sxtype))
			{
				dic[t_ttt_value.sxtype] += t_ttt_value.sxvalue;
			}
			else
			{
				dic.Add(t_ttt_value.sxtype, t_ttt_value.sxvalue);
			}

		}
		int l = 0;
		foreach (KeyValuePair<int, int> pair in dic)
		{
			if (l == 0)
			{
				s += game_data._instance.get_value_string(pair.Key, pair.Value);
			}
			else
			{
				s += "\n" + game_data._instance.get_value_string(pair.Key, pair.Value);
			}
			l++;
		}
		return s;
	}

	public static int get_jc_count()
	{
		Dictionary<int, int> dic = new Dictionary<int, int>();
		for (int i = 0; i < sys._instance.m_self.m_t_player.ttt_reward_ids.Count; ++i)
		{
			int id = sys._instance.m_self.m_t_player.ttt_reward_ids[i];
			s_t_ttt_value t_ttt_value = game_data._instance.get_t_ttt_value(id);
			if (dic.ContainsKey(t_ttt_value.sxtype))
			{
				dic[t_ttt_value.sxtype] += t_ttt_value.sxvalue;
			}
			else
			{
				dic.Add(t_ttt_value.sxtype, t_ttt_value.sxvalue);
			}
			
		}
		return dic.Count;
	}

	// Update is called once per frame
	void Update () {
       
	}
}
