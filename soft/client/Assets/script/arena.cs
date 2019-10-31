using System.Collections.Generic;
using UnityEngine;

public class arena : MonoBehaviour,IMessage {

	public GameObject m_result;
	public GameObject m_scro;
	public GameObject m_ranking;
	public GameObject m_jj_reward;
	public GameObject m_zs_reward;

	public GameObject m_ranking_list;
	public GameObject m_reward;
	public GameObject m_tishi;
	public GameObject m_paiming_jl;
	public GameObject m_my_rank;
	public GameObject m_my_name;
	public GameObject m_my_level;
	public GameObject m_my_bf;
	public GameObject m_my_icon;

	public GameObject m_view;
	public GameObject m_reward_view;
	public GameObject m_reward_time;
	public GameObject m_reward_ranking;
	public GameObject m_reward_icon_0;
	public GameObject m_reward_icon_1;
	public GameObject m_reward_button;
	public GameObject m_reward_tishi;
    public GameObject m_chenhao;
    int m_saodnag_num;

	public GameObject m_reward_point;
	public GameObject m_shop_point;
	private int m_jjd_value = 0;
	private int m_index = 0;
	private int m_reward_id ;
	private int m_last_rank = 0;
	private int m_current_rank = 0;
	private bool m_can_reward = false;
	private s_t_sport_rank m_rank = new s_t_sport_rank();
	private bool m_need_update = false;
	private float m_click_time;
    private static bool first_pk = false;
	public UILabel m_shangci_paiming_reward_label;
	public GameObject m_saodang;

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
        if (first_pk)
        {
            first_pk = false;
            root_gui._instance.action_guide("first_pk_end");
        }
		InvokeRepeating("time", 0.0f,2.0f);
        protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_SPORT_LOOK, _msg);
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}

	public void select(GameObject obj)
	{
		if (m_click_time > 0)
		{
			return;
		}
		m_click_time = 1.0f;

		if (obj.transform.Find("name").GetComponent<UILabel>().text == sys._instance.m_self.m_t_player.name)
		{
			root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("arena.cs_110_58"));//无法攻击自己
			return;
		}

		if (sys._instance.m_self.m_t_player.energy < 2)
		{
			int item_id = 10010007;
			int num = sys._instance.m_self.get_item_num((uint)item_id);
			if(num > 0)
			{
				root_gui._instance.show_tili_dialog_box(item_id);
				return;
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "buy_num_gui";
				_message.m_ints.Add(100300);
				_message.m_ints.Add(2);
				cmessage_center._instance.add_message(_message);
				return;
			}
		}

		protocol.game.cmsg_sport_fight_end _msg = new protocol.game.cmsg_sport_fight_end ();
		_msg.index = int.Parse (obj.transform.name);
		net_http._instance.send_msg<protocol.game.cmsg_sport_fight_end> (opclient_t.CMSG_SPORT_FIGHT_END, _msg);

		m_index = int.Parse (obj.transform.name);
	}

	public void select_reward(GameObject obj)
	{
		m_reward_id = int.Parse(obj.transform.name);
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SPORT_REWARD, _msg);
	}
	public void show_reward()
	{
		m_reward.SetActive(true);
		if(m_reward_view.GetComponent<SpringPanel>() != null)
		{
			m_reward_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_reward_view.transform.localPosition = new Vector3(0, 0, 0);
		m_reward_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_reward_view);
		for(int i = 0; i < game_data._instance.m_dbc_sport_rank.get_y(); i ++)
		{
			int rank1 = int.Parse(game_data._instance.m_dbc_sport_rank.get(0, i));
			if(rank1 >= 1000)
			{
				break;
			}

            GameObject _obj = game_data._instance.ins_object_res("ui/arena_reward_item");
			_obj.transform.parent = m_reward_view.transform;
			_obj.transform.localScale = new Vector3(1, 1, 1);
			_obj.transform.localPosition = new Vector3(0, 112 - i * 117, 0);
            GameObject icon1 = _obj.transform.Find("icon1").gameObject;
			sys._instance.remove_child (icon1);
			GameObject icon2 = _obj.transform.Find("icon2").gameObject;
			sys._instance.remove_child (icon2);
            _obj.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_179_81"), game_data._instance.m_dbc_sport_rank.get(0, i));//第{0}名
			if(int.Parse(game_data._instance.m_dbc_sport_rank.get(0, i)) != int.Parse(game_data._instance.m_dbc_sport_rank.get(1, i)))
			{
                _obj.transform.Find("name").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_182_82"), game_data._instance.m_dbc_sport_rank.get(0, i),//第{0}-{1}名
                    game_data._instance.m_dbc_sport_rank.get(1, i));
			}
			if(i < 3)
			{
				_obj.transform.Find("name").localPosition = new Vector3(-159,0,0);
				_obj.transform.Find("icon").gameObject.SetActive(true);
                _obj.transform.Find("icon").GetComponent<UISprite>().spriteName = "crow_xx0" + (i + 1).ToString();
			}
			else
			{
                _obj.transform.Find("name").localPosition = new Vector3(-205, 0, 0);
                _obj.transform.Find("icon").gameObject.SetActive(false);
			}

            GameObject iicon1 = icon_manager._instance.create_reward_icon(1, 2, int.Parse(game_data._instance.m_dbc_sport_rank.get(3, i)), 0);
			iicon1.transform.parent =  icon1.transform;
			iicon1.transform.localPosition = new Vector3(0,0,0);
			iicon1.transform.localScale = new Vector3(1,1,1);

            GameObject iicon2 = icon_manager._instance.create_reward_icon(1, 14, int.Parse(game_data._instance.m_dbc_sport_rank.get(2, i)), 0);
			iicon2.transform.parent = icon2.transform;
            iicon2.transform.localPosition = new Vector3(0, 0, 0);
            iicon2.transform.localScale = new Vector3(1, 1, 1);
		}
		if(m_last_rank > 1000)
		{
			m_reward_icon_0.SetActive(false);
			m_reward_icon_1.SetActive(false);
			m_reward_time.SetActive(false);
			m_shangci_paiming_reward_label.gameObject.SetActive(false);
			m_reward_tishi.SetActive(true);
            m_reward_tishi.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_214_63"), 1000);//{0}名以内可获得每日排名奖励
		}
		else
		{
			m_reward_icon_0.SetActive(true);
			m_reward_icon_1.SetActive(true);
			m_reward_time.SetActive(true);
			m_shangci_paiming_reward_label.gameObject.SetActive(true);
			m_reward_tishi.SetActive(false);
		}
        for (int i = 0; i < game_data._instance.m_dbc_sport_rank.get_y(); i++)
        {
            int _min = int.Parse(game_data._instance.m_dbc_sport_rank.get(0, i));
            int _max = int.Parse(game_data._instance.m_dbc_sport_rank.get(1, i));

            if (m_last_rank >= _min && m_last_rank <= _max && m_last_rank <= 1000)
            {
                sys._instance.remove_child(m_reward_icon_0.gameObject);
                GameObject icon1 = icon_manager._instance.create_reward_icon(1, 2, int.Parse(game_data._instance.m_dbc_sport_rank.get(3, i)), 0);
                icon1.transform.parent = m_reward_icon_0.transform;
                icon1.transform.localPosition = new Vector3(0, 0, 0);
                icon1.transform.localScale = new Vector3(1, 1, 1);
                sys._instance.remove_child(m_reward_icon_1.gameObject);

                GameObject icon2 = icon_manager._instance.create_reward_icon(1, 14, int.Parse(game_data._instance.m_dbc_sport_rank.get(2, i)), 0);
                icon2.transform.parent = m_reward_icon_1.transform;
                icon2.transform.localPosition = new Vector3(0, 0, 0);
                icon2.transform.localScale = new Vector3(1, 1, 1);
                m_rank.jj_point = int.Parse(game_data._instance.m_dbc_sport_rank.get(2, i));
                m_rank.zs_point = int.Parse(game_data._instance.m_dbc_sport_rank.get(3, i));
            }
        }
		m_reward_ranking.GetComponent<UILabel>().text = m_last_rank.ToString();
		if (m_last_rank == 0)
		{
			m_reward_ranking.GetComponent<UILabel>().text = game_data._instance.get_t_language ("arena.cs_250_51");//无排名
		}
		if(m_can_reward == false)
		{
			m_reward_button.GetComponent<UISprite>().set_enable(false);
			m_reward_button.GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			m_reward_button.GetComponent<UISprite>().set_enable(true);
			m_reward_button.GetComponent<BoxCollider>().enabled = true;
		}
	}
	public void click(GameObject obj)
	{
		if(obj.transform.name == "close_reward")
		{
			m_reward.transform.Find("frame_big").GetComponent<frame>().hide();
			reward_point();
		}
		if(obj.transform.name == "close_ranking_list")
		{
			m_ranking_list.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "shop")
		{
			this.transform.GetComponent<ui_show_anim>().hide_ui();
			s_message _message = new s_message();
			_message.m_ints.Add(m_current_rank);
			_message.m_type = "show_honor_shop";
			cmessage_center._instance.add_message(_message);
		}
		if(obj.transform.name == "accept")
		{
			m_reward_id = 0;
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();		
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SPORT_REWARD, _msg);
		}
		if(obj.transform.name == "close")
		{
			s_message _message = new s_message();
			_message.m_type = "show_huo_dong";
			_message.m_ints.Add(6);
			_message.m_bools.Add(false);
			cmessage_center._instance.add_message(_message);
			Object.Destroy(this.gameObject);
		}
		if(obj.transform.name == "ranking_list")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SPORT_TOP, _msg);
			m_ranking_list.SetActive(true);
		}
		if(obj.transform.name == "reward")
		{
			show_reward();
		}
		if(obj.transform.name == "buy_count")
		{
			int item_id = 10010007;
			int num = sys._instance.m_self.get_item_num((uint)item_id);
			if(num > 0)
			{
				root_gui._instance.show_tili_dialog_box(item_id);
				return;
			}
			else
			{
				s_message _message = new s_message();
				_message.m_type = "buy_num_gui";
				_message.m_ints.Add(100300);
				_message.m_ints.Add(2);
				cmessage_center._instance.add_message(_message);
				return;
			}
		}
		else if(obj.transform.name == "duixing")
		{
			s_message _message = new s_message();
			_message.m_type = "show_duixing_gui";
			_message.m_bools.Add(true);
			cmessage_center._instance.add_message(_message);
		}
	}

    void IMessage.net_message(s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_SPORT_REWARD)
        {
            sys._instance.m_self.add_att(e_player_attr.player_treasure_powder, m_rank.jj_point);
            sys._instance.m_self.add_att(e_player_attr.player_jewel, m_rank.zs_point, game_data._instance.get_t_language("arena.cs_345_76"));//竞技场商店购买
            m_can_reward = false;
            huo_dong_gui.m_jjc_effect = 0;
            show_reward();
        }

        if (message.m_opcode == opclient_t.CMSG_SPORT_TOP)
        {
            protocol.game.smsg_sport_top _msg = net_http._instance.parse_packet<protocol.game.smsg_sport_top>(message.m_byte);
            if (m_view.GetComponent<SpringPanel>() != null)
            {
                m_view.GetComponent<SpringPanel>().enabled = false;
            }
            m_view.transform.localPosition = new Vector3(0, 0, 0);
            m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
            sys._instance.remove_child(m_view);
            for (int i = 0; i < _msg.players.Count; i++)
            {
                GameObject _obj = game_data._instance.ins_object_res("ui/ranking_list_item");
                _obj.transform.parent = m_view.transform;
                _obj.transform.localScale = new Vector3(1, 1, 1);
                _obj.transform.localPosition = new Vector3(0, 109 - i * 68, 1);
                GameObject m_icon = _obj.transform.Find("head").gameObject;
                sys._instance.remove_child(m_icon);

                GameObject _obj1 = icon_manager._instance.create_player_icon(_msg.players[i].player_template, _msg.players[i].player_achieve, _msg.players[i].player_vip, _msg.players[i].player_nalflag);
                _obj1.transform.parent = m_icon.transform;
                _obj1.transform.localScale = new Vector3(1, 1, 1);
                _obj1.transform.localPosition = new Vector3(0, 0, 0);
                _obj.transform.Find("rank").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_179_81"), _msg.players[i].player_rank.ToString());//第{0}名
                _obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(_msg.players[i].player_achieve) + System.Text.Encoding.UTF8.GetString(_msg.players[i].player_name, 0, _msg.players[i].player_name.Length);
                _obj.transform.Find("level").GetComponent<UILabel>().text = _msg.players[i].player_level.ToString();
                _obj.transform.Find("fighting").GetComponent<UILabel>().text = _msg.players[i].player_bat_eff.ToString();
                _obj.transform.Find("fighting").GetComponent<UILabel>().text = sys._instance.value_to_wan((long)_msg.players[i].player_bat_eff);
                sys._instance.get_chenghao(_msg.players[i].player_chenghao, _obj.transform.Find("chenhao").gameObject);
                _obj.GetComponent<UIDragScrollView>().scrollView = m_view.GetComponent<UIScrollView>();
                _obj.GetComponent<rank_list_item>().m_guid = _msg.players[i].player_guid;
                _obj.GetComponent<rank_list_item>().m_is_npc = _msg.players[i].player_isnpc;
                _obj.GetComponent<rank_list_item>().m_rank = _msg.players[i].player_rank;
                _obj.GetComponent<rank_list_item>().reset();
            }

            GameObject _obj2 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count, sys._instance.m_self.m_t_player.vip, sys._instance.m_self.m_t_player.nalflag);
            _obj2.transform.parent = m_my_icon.transform;
            _obj2.transform.localScale = new Vector3(1, 1, 1);
            _obj2.transform.localPosition = new Vector3(0, 0, 0);
            m_my_rank.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_179_81"), m_current_rank.ToString());//第{0}名
            m_my_level.GetComponent<UILabel>().text = sys._instance.m_self.m_t_player.level.ToString();
            m_my_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name;
            m_my_bf.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)sys._instance.m_self.m_t_player.bf);
            sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on, m_chenhao);
        }
        if (message.m_opcode == opclient_t.CMSG_SPORT_FIGHT_END)
        {
            protocol.game.smsg_sport_fight_end _msg = net_http._instance.parse_packet<protocol.game.smsg_sport_fight_end>(message.m_byte);
            battle_logic_ex._instance.set_sport_fight_end(_msg);

            s_message _out_msg = new s_message();
            _out_msg.m_type = "sport_fight";
            cmessage_center._instance.add_message(_out_msg);

            sys._instance.load_scene_ex("ts_fight_jjc");
            sys._instance.m_game_state = "buttle";
        }
        if (message.m_opcode == opclient_t.CMSG_SPORT_SAODANG)
        {
            protocol.game.smsg_sport_saodang _msg = net_http._instance.parse_packet<protocol.game.smsg_sport_saodang>(message.m_byte);
            if (_msg.saodangs.Count * 2 <= sys._instance.m_self.m_t_player.energy)
            {
                sys._instance.m_self.sub_att(e_player_attr.player_treasure_energy, _msg.saodangs.Count * 2);
            }
            else
            {
                int outenergy = m_saodnag_num * 2 - sys._instance.m_self.m_t_player.energy;
                sys._instance.m_self.m_t_player.energy = 0;
                int item = outenergy / 10;
                int energy = outenergy % 10;

                if (energy > 0)
                {
                    sys._instance.m_self.remove_item(10010007, item + 1, game_data._instance.get_t_language("arena.cs_430_73"));//竞技场扫荡消耗
                    sys._instance.m_self.add_att(e_player_attr.player_treasure_energy, 10 - energy, false);
                }
                else
                {
                    sys._instance.m_self.remove_item(10010007, item, game_data._instance.get_t_language("arena.cs_430_73"));//竞技场扫荡消耗
                }

            }
            for (int i = 0; i < _msg.saodangs.Count; i++)
            {
                for (int j = 0; j < _msg.saodangs[i].types.Count; j++)
                {
                    sys._instance.m_self.add_reward(_msg.saodangs[i].types[j], _msg.saodangs[i].value1s[j], _msg.saodangs[i].value2s[j], _msg.saodangs[i].value3s[j], false, game_data._instance.get_t_language("arena.cs_421_156"));//竞技场扫荡
                }
                if (_msg.saodangs[i].pgold > 0)
                {
                    sys._instance.m_self.add_att(e_player_attr.player_gold, _msg.saodangs[i].pgold, false, game_data._instance.get_t_language("arena.cs_425_107"));//竞技场扫荡获得
                }
                else if (_msg.saodangs[i].result > 0)
                {
                    s_t_sport_card card = game_data._instance.get_t_sport_card(_msg.saodangs[i].cid);
                    if (card != null)
                    {
                        sys._instance.m_self.add_reward(card.type, card.value1, card.value2, card.value3, false, game_data._instance.get_t_language("arena.cs_425_107"));//竞技场扫荡获得
                    }
                }
                if (_msg.saodangs[i].result > 0)
                {
                    sys._instance.m_self.zs_skill_target(3, 1);
                }
                sys._instance.m_self.m_t_player.jj_task_num++;
                sys._instance.m_self.add_active(900, 1);
                sys._instance.m_self.check_target_done();
            }

            m_saodang.SetActive(true);
            m_saodang.GetComponent<arena_saodang>().init(_msg.saodangs);
        }
        if (message.m_opcode == opclient_t.CMSG_SPORT_LOOK)
        {
            if (this.gameObject.activeSelf == false)
            {
                this.gameObject.SetActive(true);
            }
            protocol.game.smsg_sport_look _msg = net_http._instance.parse_packet<protocol.game.smsg_sport_look>(message.m_byte);
            if (_msg.can_get == 1)
            {
                m_can_reward = true;
            }
            m_ranking.GetComponent<UILabel>().text = _msg.rank.ToString();
            m_last_rank = _msg.last_rank;
            m_current_rank = _msg.rank;
            for (int i = 0; i < 3; i++)
            {
                Transform _root = m_result.transform.Find(i.ToString());
                _root.gameObject.SetActive(false);
            }
            if (m_current_rank > 5000)
            {
                m_paiming_jl.SetActive(false);
                m_tishi.SetActive(true);
                m_tishi.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_471_57"), 5000);//{0}名以内刷新最高排名，可获取大量钻石
            }
            else
            {
                m_paiming_jl.SetActive(true);
                m_tishi.SetActive(false);
                if (m_current_rank >= 1001)
                {
                    m_paiming_jl.SetActive(false);
                    m_tishi.SetActive(true);
                    m_tishi.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language("arena.cs_214_63"), 1000);//{0}名以内可获得每日排名奖励
                }
                else
                {
                    m_paiming_jl.GetComponent<UILabel>().text = game_data._instance.get_t_language("arena.cs_485_49");//排名奖励
                }
                m_jj_reward.GetComponent<UILabel>().text = "x0";
                m_zs_reward.GetComponent<UILabel>().text = "x0";
                for (int i = 0; i < game_data._instance.m_dbc_sport_rank.get_y(); i++)
                {
                    int _min = int.Parse(game_data._instance.m_dbc_sport_rank.get(0, i));
                    int _max = int.Parse(game_data._instance.m_dbc_sport_rank.get(1, i));

                    if (m_current_rank >= _min && m_current_rank <= _max && m_current_rank <= 1000)
                    {
                        m_jj_reward.GetComponent<UILabel>().text = "x" + int.Parse(game_data._instance.m_dbc_sport_rank.get(2, i));
                        m_zs_reward.GetComponent<UILabel>().text = "x" + int.Parse(game_data._instance.m_dbc_sport_rank.get(3, i));
                        break;
                    }
                }
            }
            for (int i = _msg.sports.Count - 1; i >= 0; --i)
            {
                dhc.sport_t _sport = _msg.sports[i];
                Transform _root = m_result.transform.Find(i.ToString());
                _root.gameObject.SetActive(true);

                Transform _label = _root.Find("Label").transform;
                string _text = "";
                ulong _now = timer.now();
                long dtime = (long)_now - (long)_sport.time;
                if (dtime < 60 * 1000)
                {
                    _text += "[0aabff]" + game_data._instance.get_t_language("arena.cs_520_27");//刚刚
                }
                else if (dtime < 60 * 1000 * 60)
                {
                    long _time = dtime / (60 * 1000);
                    _text += "[0aabff]" + string.Format(game_data._instance.get_t_language("arena.cs_525_41"), _time.ToString());//{0}分钟前
                }
                else if (dtime < 60 * 1000 * 60 * 24)
                {
                    long _time = dtime / (60 * 1000 * 60);
                    _text += "[0aabff]" + string.Format(game_data._instance.get_t_language("arena.cs_530_41"), _time.ToString());//{0}小时前
                }
                else if (dtime >= 60 * 1000 * 60 * 24)
                {
                    long _time = dtime / (60 * 1000 * 60 * 24);
                    _text += "[0aabff]" + string.Format(game_data._instance.get_t_language("arena.cs_535_41"), _time.ToString());//{0}天前
                }

                string other_name = System.Text.Encoding.UTF8.GetString(_sport.other_name, 0, _sport.other_name.Length).TrimEnd('\0');
                if (_sport.win == 0)
                {
                    if (_sport.type == 0)
                    {
                        if (_sport.rank == 0)
                        {
                            _text = "[0aabff]" + _text + string.Format(game_data._instance.get_t_language("arena.cs_545_71"), other_name); //你挑战了[-][ffffff]{0}[-][0aabff],你获胜了！排名保持不变

                            _root.Find("star2").gameObject.SetActive(false);
                            _root.Find("star1").gameObject.SetActive(false);
                        }
                        else
                        {
                            _text = "[0aabff]" + _text;
                            _text += string.Format(game_data._instance.get_t_language("arena.cs_553_30"), other_name, _sport.rank);//你挑战了[ffffff]{0}[-][0aabff],你获胜了！排名升至第{1}名
                            _root.Find("star2").gameObject.SetActive(true);
                            _root.Find("star1").gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        _text += "[-][ffffff]" + string.Format(game_data._instance.get_t_language("arena.cs_560_45"), other_name);//{0}[-][0aabff]挑战了你,你获胜了！排名保持不变[-]
                        _root.Find("star2").gameObject.SetActive(false);
                        _root.Find("star1").gameObject.SetActive(false);
                    }
                }
                else
                {
                    if (_sport.type == 0)
                    {
                        _text = "[0aabff]" + _text;
                        _text += string.Format(game_data._instance.get_t_language("arena.cs_571_29"), other_name);//你挑战了[ffffff]{0}[-][0aabff],你战败了！排名保持不变
                        _root.Find("star2").gameObject.SetActive(false);
                        _root.Find("star1").gameObject.SetActive(false);
                    }
                    else
                    {
                        if (_sport.rank == 0)
                        {
                            _text += string.Format(game_data._instance.get_t_language("arena.cs_579_30"), other_name);//[ffffff]{0}[-][a0abff]挑战了你,你战败了！排名保持不变
                            _root.Find("star2").gameObject.SetActive(false);
                            _root.Find("star1").gameObject.SetActive(false);
                        }
                        else
                        {
                            _text = "[0aabff]" + _text;
                            _text += string.Format(game_data._instance.get_t_language("arena.cs_586_30"), other_name, _sport.rank);//[ffffff]{0}[-],[0aabff]挑战了你,你战败了！排名降至第{1}名
                            _root.Find("star2").gameObject.SetActive(false);
                            _root.Find("star1").gameObject.SetActive(true);
                        }
                    }
                }
                _label.GetComponent<UILabel>().text = _text;
            }
            if (m_scro.GetComponent<SpringPanel>() != null)
            {
                m_scro.GetComponent<SpringPanel>().enabled = false;
            }
            m_scro.transform.localPosition = new Vector3(0, 0, 0);
            m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
            sys._instance.remove_child(m_scro);
            protocol.game.msg_sport_player m_player = new protocol.game.msg_sport_player();
            m_player.player_bat_eff = sys._instance.m_self.m_t_player.bf;
            m_player.player_level = sys._instance.m_self.m_t_player.level;
            m_player.player_name = System.Text.Encoding.UTF8.GetBytes(sys._instance.m_self.m_t_player.name);
            m_player.player_rank = m_current_rank;
            m_player.player_template = (int)sys._instance.m_self.m_t_player.template_id;
            m_player.player_isnpc = 0;
            m_player.player_vip = sys._instance.m_self.m_t_player.vip;
            m_player.player_achieve = sys._instance.m_self.m_t_player.dress_achieves.Count;
            m_player.player_nalflag = sys._instance.m_self.m_t_player.nalflag;
            bool flag = false;
            int select_id = _msg.players.Count - 1;
            for (int i = 0; i < _msg.players.Count; i++)
            {
                if (_msg.players[i].player_rank > m_current_rank)
                {
                    flag = true;
                    _msg.players.Insert(i, m_player);
                    break;
                }
            }
            if (!flag)
            {
                _msg.players.Add(m_player);
            }
            int _id = my_position(_msg.players) - _msg.players.Count;
            int num = -132;
            if (my_position(_msg.players) <= 4)
            {
                num = 162;
                _id = -(_msg.players.Count - 1);
            }
            if (my_position(_msg.players) == 11)
            {
                num = -230;
                _id = 0;
            }
            bool gao = false;
            for (int i = _msg.players.Count - 1; i >= 0; i--)
            {
                GameObject temp = game_data._instance.ins_object_res("ui/arena_item");
                temp.transform.parent = m_scro.transform;
                temp.transform.localPosition = new Vector3(-152, num + 98 * _id, 0);
                temp.transform.localScale = new Vector3(1, 1, 1);
                if (is_min(_msg.players[i]))
                {
                    temp.GetComponent<arena_item>().m_id = 11;
                    gao = true;
                }
                else
                {
                    temp.GetComponent<arena_item>().m_id = select_id;
                    select_id--;
                }
                temp.GetComponent<arena_item>()._player = _msg.players[i];
                temp.GetComponent<arena_item>().reset(gao);
                _id++;
            }
            reward_point();
            sys._instance.m_self.m_t_player.max_rank = _msg.max_rank;
            m_need_update = true;
        }
    }

    public static bool is_min(protocol.game.msg_sport_player _player)
    {
        string name = System.Text.Encoding.UTF8.GetString(_player.player_name, 0, _player.player_name.Length);
        if (name == sys._instance.m_self.m_t_player.name)
        {
            return true;
        }
        return false;
    }

    public int my_position(List<protocol.game.msg_sport_player> players)
    {
        for (int i = 0; i < players.Count; ++i)
        {
            string name = System.Text.Encoding.UTF8.GetString(players[i].player_name, 0, players[i].player_name.Length);
            if (name == sys._instance.m_self.m_t_player.name)
            {
                return i + 1;
            }
        }
        return 0;
    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "update_player_att")
        {
            m_need_update = true;
        }
        if (message.m_type == "update_arena_gui")
        {
            m_need_update = true;
        }
        if (message.m_type == "select_arena_item")
        {
            if (m_click_time > 0)
            {
                return;
            }
            m_click_time = 1.0f;

            if ((int)message.m_ints[0] == 11)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language("arena.cs_110_58"));//无法攻击自己
                return;
            }

            if (sys._instance.m_self.m_t_player.energy < 2)
            {
                int item_id = 10010007;
                int num = sys._instance.m_self.get_item_num((uint)item_id);
                if (num > 0)
                {
                    root_gui._instance.show_tili_dialog_box(item_id);
                    return;
                }
                else
                {
                    s_message _message = new s_message();
                    _message.m_type = "buy_num_gui";
                    _message.m_ints.Add(100300);
                    _message.m_ints.Add(2);
                    cmessage_center._instance.add_message(_message);
                    return;
                }
            }

            protocol.game.cmsg_sport_fight_end _msg = new protocol.game.cmsg_sport_fight_end();
            _msg.index = (int)message.m_ints[0];
            net_http._instance.send_msg<protocol.game.cmsg_sport_fight_end>(opclient_t.CMSG_SPORT_FIGHT_END, _msg);

            m_index = (int)message.m_ints[0];
        }
        if (message.m_type == "select_arena_item1")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_sport_zhan5ci && sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_sport_zhan5ci)
            {

                root_gui._instance.show_prompt_dialog_box
                    (string.Format(game_data._instance.get_t_language("arena.cs_747_35"), (int)e_open_vip.ev_sport_zhan5ci, (int)e_open_level.el_sport_zhan5ci));//[ffc882]该功能VIP{0}或者达到{1}级开启
                return;
            }

            if (m_click_time > 0)
            {
                return;
            }
            m_click_time = 1.0f;
            root_gui._instance.show_select_num_gui(1);
            m_index = (int)message.m_ints[0];
        }
        if (message.m_type == "yijian_arena")
        {
            m_saodnag_num = (int)message.m_ints[0];
            protocol.game.cmsg_sport_saodang _msg = new protocol.game.cmsg_sport_saodang();
            _msg.index = m_index;
            _msg.num = m_saodnag_num;
            _msg.use_item = (bool)message.m_bools[0];
            net_http._instance.send_msg<protocol.game.cmsg_sport_saodang>(opclient_t.CMSG_SPORT_SAODANG, _msg);
        }
        if (message.m_type == "arena_look")
        {
            reset();
        }
        if (message.m_type == "first_pk")
        {
            first_pk = true;
        }
    }

	public void reset()
	{
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_SPORT_LOOK, _msg);
	}

	void time()
	{
		long _time = timer.last_time_today() - 3600 * 1000 * 3;
		if (_time < 0)
		{
			_time = _time + 3600 * 1000 * 24;
		}
		string _text = timer.get_time_show(_time);
		m_reward_time.GetComponent<UILabel>().text = _text;
		
	}

	void reward_point()
	{
		bool flag = false;
		if (m_can_reward)
		{
			flag  = true;
		}
		if (flag)
		{
			m_reward_point.SetActive(true);
		}
		else
		{
			m_reward_point.SetActive(false);
		}
	}

	void update_ui()
	{
		int _num = sys._instance.m_self.m_t_player.energy;
		if(honor_shop_gui.is_effect())
		{
			m_shop_point.SetActive(true);
		}
		else
		{
			m_shop_point.SetActive(false);
		}
	}

	void Update () {
		if (m_need_update)
		{
			update_ui ();
			m_need_update = false;
		}

		if (m_click_time > 0)
		{
			m_click_time -= Time.deltaTime;
		}
		if(m_jjd_value != sys._instance.m_self.get_att(e_player_attr.player_treasure_powder))
		{
			m_jjd_value = sys._instance.m_self.get_att(e_player_attr.player_treasure_powder);
		}
	}
}
