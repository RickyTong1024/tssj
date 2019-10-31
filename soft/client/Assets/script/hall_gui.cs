
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hall_gui : MonoBehaviour,IMessage {

	public GameObject m_main_gui;
	public GameObject m_shop_panel;
    public GameObject m_jiban_panel;
    private GameObject m_huodong_gui;
	private GameObject m_ccl_gui;
	private GameObject m_recycle_gui;
	private GameObject m_jieri_duihuan_gui;
	private GameObject m_mubiao_gui;
	private GameObject m_juntuan_gui;
	private GameObject m_post_gui;
	private GameObject m_first_recharge_gui;
	private GameObject m_online_reward_gui;
	private GameObject m_daily_sign_gui;
	private GameObject m_arena_gui;
	private GameObject m_item_shop_gui;
	private GameObject m_bu_zheng_gui;
	private GameObject m_pet_gui;
	private GameObject m_chongsheng_gui;
	private GameObject m_vip_toupiao_gui;
	private GameObject m_partner_gui;
	private GameObject m_bag_gui;
	private GameObject m_catch_gui;
	private GameObject m_catch_card_show;
	private GameObject m_map_gui;
	private GameObject m_info_gui;
    private GameObject m_notice_gui;
	private GameObject m_chong_neng_gui;
	private GameObject m_equip_qianghua_gui;
	private GameObject m_treasure_qianghua_gui;
	private GameObject m_equip_gaizao_gui;
	private GameObject m_treasure_jinglian_gui;
	private GameObject m_treasure_sx_gui;
	private GameObject m_treasure_zhuzao_gui;
	private GameObject m_help_gui;
	private GameObject m_toper_gui;
	private GameObject m_friend_gui;
	private GameObject m_partner_shop_gui;
	private GameObject m_jiyin_gaizao_gui;
	public GameObject m_more_panel;
	public GameObject m_last_panel;
	public GameObject m_more_button;
	public GameObject m_last_button;
	private GameObject m_pet_weiyang_gui;
	private GameObject m_pet_shengxing_gui;
	private GameObject m_pet_jj_gui;
	private GameObject m_pet_tj_gui;
	private GameObject m_five_stars;
	private GameObject m_tu_po_gui;

	private GameObject m_jinjie_gui;
	private GameObject m_skill_gui;
	private GameObject m_wuping_gui;
	private GameObject m_equip_sx_gui;
	private GameObject m_xq_change_gui;
	private GameObject m_honor_shop_gui;
	private GameObject m_equip_jl_gui;

	private GameObject m_kaifu_gui;
	private GameObject m_kaifu_gui_ex;
	private GameObject m_jieri_huodong_gui;
	private GameObject m_mrfl_gui;
	private GameObject m_jc_huodong_gui;
	private GameObject m_czjh_gui;
	private GameObject m_ore_gui;
	private GameObject m_xq_gui;
	private GameObject m_czfp_gui;
	private GameObject m_zhuanpan_gui;
    private GameObject m_mofang_gui;
	private GameObject m_chenghao;
	private GameObject m_huigui_panel;
	private GameObject m_download_gui;
    private GameObject m_nationality_gui;
	private ulong m_check_time;
	private bool m_need_deal = false;
	private float m_deal_time = 0;
	private int m_quick_item = 0;
    private List<int> m_end_rewards = new List<int>();
    private int m_end_index = 0;
    private string m_scence = "";
    private int m_gold = 0;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
		s_message _mes = new s_message();
		_mes.m_type = "hall_anim";
		_mes.m_string.Add("0");
		_mes.time = 0.1f;
		cmessage_center._instance.add_message(_mes);

		m_main_gui.GetComponent<main_gui>().m_show = true;
		m_main_gui.SetActive (true);
        
        m_jiban_panel.GetComponent<ui_show_anim>().hide_ui();
		m_shop_panel.GetComponent<ui_show_anim>().hide_ui ();
		sys._instance.player_check();

		sys._instance.m_xq_check = true;
		m_check_time = timer.now ();
		mubiao_mubiao_panel.check_target_done();
		///触发第一次战斗引导
		if(sys._instance.m_self.m_t_player.level == 1 && sys._instance.m_self.m_t_player.exp == 0)
		{
            root_gui._instance.action_guide("game_start");

        }
		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_2x && game_data._instance.m_player_data.m_speed != 1)
		{
			game_data._instance.m_player_data.m_speed = 1;
			game_data._instance.save();
		}

        if (sys._instance.m_self.m_t_player.level >= 10)
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_VIEW, _msg);
        }

		if(sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_jiasu && game_data._instance.m_player_data.m_speed == 3)
		{
			game_data._instance.m_player_data.m_speed = 1;
			game_data._instance.save();
		}

		if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_auto_battle 
		   && game_data._instance.m_player_data.m_auto_skill != 0)
		{
			game_data._instance.m_player_data.m_auto_skill = 0;
			game_data._instance.save();
		}
		m_more_panel.SetActive(false);
		m_last_panel.SetActive (true);
		m_last_button.GetComponent<UIPanel>().alpha = 0;
		m_more_button.GetComponent<UIPanel>().alpha = 1;
	}
    void OnDisable()
    {
        root_gui._instance.m_talk.SetActive(false);
    }
	void load_gui(ref GameObject obj, string dir, string name)
	{
        if (this.transform.Find(name) == null && this.transform.Find(name + "Clone") == null)
		{
			obj = game_data._instance.ins_object_res(dir + name);
			obj.transform.parent = this.transform;
			obj.transform.localPosition = new Vector3(0,0,0);
			obj.transform.localScale = Vector3.one;
		}
		else
		{
			obj = transform.Find(name).gameObject;
		}
	}

	void IMessage.net_message(s_net_message message)
	{

		if (message.m_opcode == opclient_t.CMSG_PLAYER_CHECK)
		{
			protocol.game.smsg_player_check _msg = net_http._instance.parse_packet<protocol.game.smsg_player_check> (message.m_byte);
			sys._instance.m_self.is_post = _msg.post;
			sys._instance.m_self.is_friend_apply = _msg.friend_apply;
			sys._instance.m_self.is_friend_tili = _msg.friend_tili;
			sys._instance.m_self.m_can_kaifu = _msg.kaifu;
			sys._instance.m_self.m_can_pttq = _msg.pttq;
            sys._instance.m_self.m_can_guild = _msg.guild;
			sys._instance.m_self.m_exist_huodong = _msg.jieri_chanchu;
			sys._instance.m_self.m_item_shop_effect = _msg.shop_refresh;
			sys._instance.m_self.m_jieri_huodong = _msg.huodong;
			sys._instance.m_self.m_can_jieri_huodong = _msg.jieri_point;
			sys._instance.m_self.m_huodong_xhqd  = _msg.xingheqidian;
            sys._instance.m_self.is_bingyuan_effect = _msg.bingyuan;
			if(_msg.huodong == "")
			{
				int num = sys._instance.m_self.get_item_num((uint)e_huodong_item_id.ei_huodong_item1);
				sys._instance.m_self.remove_item((uint)e_huodong_item_id.ei_huodong_item1,num);
				num = sys._instance.m_self.get_item_num((uint)e_huodong_item_id.ei_huodong_item2);
				sys._instance.m_self.remove_item((uint)e_huodong_item_id.ei_huodong_item2,num);
			}
			sys._instance.m_self.m_jieri_icon_name = _msg.huodong_item1;
			sys._instance.m_self.m_jieri_icon_name1 = _msg.huodong_item2;
			sys._instance.m_self.m_jieri_icon_desc = _msg.huodong_des1;
			sys._instance.m_self.m_jieri_icon_desc1 = _msg.huodong_des2;
			sys._instance.m_self.m_t_player.ds_duanwei = _msg.duanwei;
			huo_dong_gui.m_yb_effect = _msg.yb;
			huo_dong_gui.m_qd_effect = _msg.qd;
			huo_dong_gui.m_jjc_effect = _msg.jjc;
			m_check_time = timer.now ();
			m_need_deal = true;
		}
		if (message.m_opcode == opclient_t.CMSG_RANDOM_EVENT_LOOK)
		{
			protocol.game.cmsg_random_event_get _msg = new protocol.game.cmsg_random_event_get ();
			_msg.index = 0;
			net_http._instance.send_msg<protocol.game.cmsg_random_event_get> (opclient_t.CMSG_RANDOM_EVENT_GET, _msg);
		}
		if(message.m_opcode == opclient_t.CMSG_ROLE_YH_LOOK)
		{
			load_gui (ref m_xq_gui, "ui/", "xq_gui");
			m_xq_gui.AddComponent<gui_remove>();
			
			protocol.game.smsg_role_xq_look _msg = net_http._instance.parse_packet<protocol.game.smsg_role_xq_look> (message.m_byte);
			m_xq_gui.transform.GetComponent<xq_gui>().guids = _msg.guid;
			m_xq_gui.transform.GetComponent<xq_gui>().xqs = _msg.xq;
			m_xq_gui.transform.GetComponent<xq_gui>().reset();
			m_xq_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_VIEW)
		{
			protocol.game.smsg_huodong_view _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_view> (message.m_byte);
			{
                
				sys._instance.m_self.m_end_time.Clear();
				sys._instance.m_self.m_huodong_name.Clear();
				sys._instance.m_self.m_huodong_ids.Clear();
                sys._instance.m_self.m_huodong_czjh_count = _msg.czjh_count;
				for(int i = 0 ; i < _msg.huodong_ids.Count; ++i)
				{
					sys._instance.m_self.m_end_time.Add(_msg.huodong_times[i]);
					sys._instance.m_self.m_huodong_name.Add(_msg.huodong_names[i]);
					sys._instance.m_self.m_huodong_ids.Add(_msg.huodong_ids[i]);
				}
				if(m_jc_huodong_gui == null)
				{
					load_gui (ref m_jc_huodong_gui, "ui/", "event_gui");
				}
				m_jc_huodong_gui.AddComponent<gui_remove>();
				m_jc_huodong_gui.SetActive(true);
				m_main_gui.GetComponent<main_gui>().hide();

			}
		}
		if(message.m_opcode == opclient_t.CMSG_ROLE_XQ_LOOK)
		{
			protocol.game.smsg_role_xq_look _msg = net_http._instance.parse_packet<protocol.game.smsg_role_xq_look> (message.m_byte);
			if(_msg.guid.Count > 0)
			{
				show_xq_chang_gui(_msg.guid,_msg.xq);
			}
			else
			{
				sys._instance.m_xq_check = false;
			}
			sys._instance.m_self.set_att(e_player_attr.player_jewel,_msg.zs,false);
			sys._instance.m_self.set_att(e_player_attr.player_gold,_msg.jb,false);

		}
	}

	void show_xq_chang_gui(List<ulong> _guids,List<int> _xqs)
	{
		load_gui (ref m_xq_change_gui, "ui/", "xq_change");
		m_xq_change_gui.AddComponent<gui_remove>();
		m_xq_change_gui.GetComponent<xq_change>().guids = _guids;
		m_xq_change_gui.GetComponent<xq_change>().xqs = _xqs;
		int _id = 0;
		for(int i = 0; i < _guids.Count;++i)
		{
			ccard card = sys._instance.m_self.get_card_guid(_guids[i]);
			if(card.get_role().xq == 0)
			{
				card.get_role().xq = 3;
			}
			if(card.get_role().xq == _xqs[i])
			{
				continue;
			}
			_id ++;
		}
		m_xq_change_gui.GetComponent<xq_change>().reset();
		if(_id > 0)
		{
			m_xq_change_gui.SetActive(true);
		}
		else
		{
			m_xq_change_gui.SetActive(false);
		}
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "showupgrade")
		{
			bu_zheng();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm03");
		}
		if (message.m_type == "show_first_reward") {
			if (sys._instance.m_self.m_t_player.first_reward == 0) {
				show_first_recharge_gui ();
				m_main_gui.GetComponent<main_gui>().hide ();
			}
		}
		if (message.m_type == "hide_bag_gui") {
			if (m_bag_gui != null) {
				m_bag_gui.SetActive (false);
			}
		}
		if (message.m_type == "hide_equip_jl") {
			if (m_equip_jl_gui != null) {
				m_equip_jl_gui.SetActive (false);
			}
		}
		if (message.m_type == "hide_treasure_jl") {
			if (m_treasure_jinglian_gui != null) {
				m_treasure_jinglian_gui.SetActive (false);
			}
		}
		if (message.m_type == "hide_equip_sx") {
			if (m_equip_sx_gui != null) {
				m_equip_sx_gui.SetActive (false);
			}
		}
		if (message.m_type == "hide_main_gui") {
			if (m_main_gui != null) {
				m_main_gui.SetActive (false);
				root_gui._instance.m_talk.SetActive (false);
			}
		}
		if (message.m_type == "mm_talk") {
			if (m_main_gui != null && m_main_gui.activeSelf && this.gameObject.activeSelf) {
				root_gui._instance.m_talk.GetComponent<hide_time>().m_time = 10f;
				root_gui._instance.m_talk.SetActive (true);
			}
		}
		if (message.m_type == "show_vip_toupiao") {
			load_gui (ref m_vip_toupiao_gui, "ui/", "vip_toupiao_gui");
			m_vip_toupiao_gui.SetActive (true);
			m_main_gui.GetComponent<main_gui>().hide ();
		}
		if (message.m_type == "show_ore_gui") {
			show_ore_gui ();
		}

		if (message.m_type == "touch_mm") {
			s_message _out = new s_message ();
			
			_out.m_type = "show_dress_gui";
			_out.m_string.Add ("show_main_gui");
			
			cmessage_center._instance.add_message (_out);
			m_main_gui.GetComponent<main_gui>().hide ();
			m_main_gui.SetActive (false);
		}

		if (message.m_type == "show_jing_ji_chang") {
			show_arena_gui ();
			sys._instance.play_sound_ex ("sound/ts_rwpy/tspy_jm05");
		}

		if (message.m_type == "show_jing_ji_chang") {
			show_arena_gui ();
			sys._instance.play_sound_ex ("sound/ts_rwpy/tspy_jm05");
		}
		if (message.m_type == "show_arena_shop") {
			load_gui (ref m_honor_shop_gui, "ui/", "honor_shop_gui");
			m_honor_shop_gui.AddComponent<gui_remove>();
			m_honor_shop_gui.SetActive (true);
			//m_honor_shop_gui.GetComponent<honor_shop_gui>().rewrd_shop();
		}
		if (message.m_type == "hide_bag") {
			m_bag_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
		}
		if (message.m_type == "show_bag_gui1") {
			show_bag_gui_ex ();
			m_bag_gui.SetActive (true);
			m_bag_gui.GetComponent<bag_gui>().update_ui (true, 3);
		}
		if (message.m_type == "show_bag_gui_treasure") {
			show_bag_gui_ex ();
			m_bag_gui.SetActive (true);
			m_bag_gui.GetComponent<bag_gui>().update_ui (true, 4);
		}
		if (message.m_type == "show_bu_zheng_gui") {
			show_buzheng_gui ();
			m_bu_zheng_gui.SetActive (true);

			s_message _mes = new s_message ();
			_mes.m_type = "show_unit_show";
			cmessage_center._instance.add_message (_mes);
			
			m_main_gui.GetComponent<main_gui>().hide ();
		}

		if (message.m_type == "show_sheng_pin_gui") {
			show_buzheng_gui ();
			m_bu_zheng_gui.SetActive (true);
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_bu_zheng_panel.SetActive (false);
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_shenping_panel.SetActive (true);
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().show_peiyang ();
			s_message _mes = new s_message ();
			_mes.m_type = "show_unit_show";
			cmessage_center._instance.add_message (_mes);
			
			m_main_gui.GetComponent<main_gui>().hide ();
		}

		if (message.m_type == "show_sheng_pin_gui_ex" && m_bu_zheng_gui != null) {
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_bu_zheng_panel.SetActive (false);
			ccard m_card = sys._instance.m_self.get_card_guid ((ulong)message.m_long [0]);
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_shenping_panel.GetComponent<shengping_gui>().m_card = m_card;
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_shenping_panel.SetActive (true);
		}

		if (message.m_type == "show_guanghuan_gui") {
			show_buzheng_gui ();
			m_bu_zheng_gui.SetActive (true);
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_bu_zheng_panel.SetActive (false);
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_shenping_panel.SetActive (false);
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_guanghuan_gui.SetActive (true);
			s_message _mes = new s_message ();
			_mes.m_type = "show_unit_show";
			cmessage_center._instance.add_message (_mes);
			
			m_main_gui.GetComponent<main_gui>().hide ();
		}
		if (message.m_type == "hide_recycle_gui") {
			if (m_recycle_gui != null && m_recycle_gui.activeSelf) {
				m_recycle_gui.SetActive (false);
			}
		}

		if (message.m_type == "show_juntuan_kuafu")
		{
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_juntuan)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_juntuan));//该功能{0}级开启
				return;
			}
			if (m_juntuan_gui == null)
			{
				load_gui (ref m_juntuan_gui, "ui/juntuan/", "juntuan_gui");
			}
			m_juntuan_gui.SetActive(true);
			m_juntuan_gui.GetComponent<juntuan_gui>().reset();
			m_main_gui.GetComponent<main_gui>().hide();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm13");

			
			this.Invoke("open_kuafu",0.5f);
		}

		if (message.m_type == "show_huo_dong") {
			show_huodong_gui ();
			if (message.m_ints.Count > 0) {
				if (message.m_bools.Count > 0) {
					m_huodong_gui.GetComponent<huo_dong_gui>().m_select = (int)message.m_ints [0];
					m_huodong_gui.GetComponent<huo_dong_gui>().flag = (bool)message.m_bools [0];
					m_huodong_gui.GetComponent<huo_dong_gui>().update_ui ();
				} else {
					m_huodong_gui.GetComponent<huo_dong_gui>().m_select = (int)message.m_ints [0];
					m_huodong_gui.GetComponent<huo_dong_gui>().flag = true;
					m_huodong_gui.GetComponent<huo_dong_gui>().update_ui ();
				}
			}
			sys._instance.play_sound_ex ("sound/ts_rwpy/tspy_jm10");
		}
		if (message.m_type == "show_catch_gui") {
			if (m_catch_gui == null) {
				load_gui (ref m_catch_gui, "ui/", "catch_card_gui");
			}
			m_catch_gui.GetComponent<catch_card_gui>().reset ();
			m_catch_gui.SetActive (true);
		}
		if (message.m_type == "show_catch_gui2") {
			if (m_catch_gui != null) {
				m_catch_gui.SetActive (true);
			}
		} else if (message.m_type == "show_catch_card_show") {

			load_gui (ref m_catch_card_show, "ui/", "catch_card_show");
			m_catch_card_show.AddComponent<gui_remove>();

			protocol.game.smsg_chouka _msg = (protocol.game.smsg_chouka)message.m_object [0];
			m_catch_card_show.GetComponent<catch_card_show>().m_msg = _msg;
			m_catch_card_show.GetComponent<catch_card_show>().m_message = (string)message.m_string [0];

			m_catch_card_show.SetActive (true);
		} else if (message.m_type == "show_main_gui") {
		
			m_main_gui.GetComponent<main_gui>().m_show = true;
			if (sys._instance.m_message_type.Count == 0) {
				m_main_gui.SetActive (true);
			}
			m_shop_panel.GetComponent<ui_show_anim>().hide_ui ();
			m_jiban_panel.GetComponent<ui_show_anim>().hide_ui ();
			m_more_panel.SetActive(false);
			m_last_panel.SetActive (true);
			m_last_button.GetComponent<UIPanel>().alpha = 0;
			m_more_button.GetComponent<UIPanel>().alpha = 1;
			s_message _mes = new s_message ();
			_mes.m_type = "hall_anim";
			_mes.m_string.Add ("0");
			cmessage_center._instance.add_message (_mes);
			s_message _message = new s_message ();
			_message = new s_message ();
			_message.m_type = "hide_unit_show";
			cmessage_center._instance.add_message (_message);

			_message.m_type = "hide_show_unit";
			cmessage_center._instance.add_message (_message);

			if (sys._instance.m_message_type.Count == 0) {
				/// 刷新图标
				m_need_deal = true;

				sys._instance.m_xq_check = true;
				/// 查看邮件
				if (timer.now () - m_check_time >= 120000) {
					sys._instance.player_need_check ();
				}
			}

			if (m_quick_item > 0) {
				load_gui (ref m_wuping_gui, "ui/", "wuping_gui");
				m_wuping_gui.AddComponent<gui_remove>();
				m_wuping_gui.GetComponent<wuping_gui>().m_item_id = m_quick_item;
				m_wuping_gui.SetActive (true);
				m_wuping_gui.GetComponent<wuping_gui>().reset ();
				m_quick_item = 0;
			}

			hide_huodong_gui ();
			if (sys._instance.m_message_type.Count != 0) {
				for (int i = 0; i < sys._instance.m_message_type.Count; ++i) {
					s_message _mess = new s_message ();
					_mess.m_type = sys._instance.m_message_type [i];
					if (sys._instance.m_message_long.Count != 0) {
						for (int j = 0; j < sys._instance.m_message_long.Count; ++j) {
							_mess.m_long.Add (sys._instance.m_message_long [j]);
						}
					}
					cmessage_center._instance.add_message (_mess);
				}
			}
			sys._instance.message_clear ();

			if(root_gui._instance.m_default_active == "upgrade")
			{
				root_gui._instance.m_default_active ="";
				bu_zheng();
			}
			else if(root_gui._instance.m_default_active == "summon")
			{
				if (m_catch_gui == null)
				{
					load_gui (ref m_catch_gui, "ui/", "catch_card_gui");
				}
				m_catch_gui.GetComponent<catch_card_gui>().reset();
				m_catch_gui.SetActive(true);
				
				s_message _mes_summon = new s_message();
				
				_mes_summon.m_type = "hall_anim";
				_mes_summon.m_string.Add("1");
				
				cmessage_center._instance.add_message(_mes_summon);
				
				m_main_gui.GetComponent<main_gui>().hide();
				sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm02");

				root_gui._instance.m_default_active ="";
			}
			else if(root_gui._instance.m_default_active == "forg")
			{
				root_gui._instance.m_default_active ="";
				bu_zheng();
			}

		} else if (message.m_type == "deal_main_gui") {
			m_need_deal = true;
		} else if (message.m_type == "show_fu_ben") {
			fu_ben ();
		} else if (message.m_type == "hide_huoban" && m_partner_gui != null) {
			m_partner_gui.GetComponent<ui_show_anim>().hide_ui ();
		} else if (message.m_type == "show_huoban2") {
			show_partner_gui ();
		} else if (message.m_type == "hide_bag" && m_bag_gui != null) {
			m_bag_gui.SetActive (false);
		} else if (message.m_type == "show_bag") {
			show_bag_gui ();
			m_bag_gui.SetActive (true);
			m_main_gui.GetComponent<main_gui>().hide ();
		} else if (message.m_type == "show_bag1") {
			show_bag_gui_ex ();
			if (m_bag_gui.activeSelf) {
				m_bag_gui.GetComponent<bag_gui>().show_gui ((int)message.m_ints [0]);
				m_bag_gui.GetComponent<bag_gui>().m_jiyin.value = true;
               
			} else {
				m_bag_gui.SetActive (true);
				m_bag_gui.transform.Find("frame_big").GetComponent<frame>().handle = delegate() {
					if ((int)message.m_ints [0] == 3) {
						m_bag_gui.GetComponent<bag_gui>().m_jiyin.value = true;
						m_bag_gui.GetComponent<bag_gui>().m_jiyin.startsActive = true;
						m_bag_gui.GetComponent<bag_gui>().m_item_toogle.startsActive = false;
 
					} else if ((int)message.m_ints [0] == 2) {
						m_bag_gui.GetComponent<bag_gui>().m_equip_toggle.value = true;
						m_bag_gui.GetComponent<bag_gui>().m_equip_toggle.startsActive = true;
						m_bag_gui.GetComponent<bag_gui>().m_item_toogle.startsActive = false;

					} else if ((int)message.m_ints [0] == 5) {
						m_bag_gui.GetComponent<bag_gui>().m_baowu_toggle.value = true;
						m_bag_gui.GetComponent<bag_gui>().m_baowu_toggle.startsActive = true;
						m_bag_gui.GetComponent<bag_gui>().m_item_toogle.startsActive = false;

					} else if ((int)message.m_ints [0] == 6) {
						m_bag_gui.GetComponent<bag_gui>().m_red_equip_toggle.value = false;
						m_bag_gui.GetComponent<bag_gui>().m_baowu_toggle.startsActive = false;
						m_bag_gui.GetComponent<bag_gui>().m_pet_duihuan.GetComponent<UIToggle>().value = true;
						
					}

				};
				m_bag_gui.GetComponent<bag_gui>().show_gui ((int)message.m_ints [0]);
			}
			m_main_gui.GetComponent<main_gui>().hide ();
		} else if (message.m_type == "show_huoban3") {
			if (message.m_ints.Count > 0) {
				huo_ban (true);
			} else if (message.m_long.Count > 0) {
				ulong guid = (ulong)message.m_long [0];

				if (m_partner_gui != null) {
					m_partner_gui.GetComponent<partner_gui>().check (guid);
				}

				huo_ban (false);
			}
		} else if (message.m_type == "show_buzheng2") {
			show_buzheng_gui ();
			m_bu_zheng_gui.SetActive (true);
			s_message _mes = new s_message ();
			_mes.m_type = "show_unit_show";
			cmessage_center._instance.add_message (_mes);
			
			m_main_gui.GetComponent<main_gui>().hide ();
		} else if (message.m_type == "show_juntuan_gui") {
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_juntuan) {
				root_gui._instance.show_prompt_dialog_box ("[ffc882]" + string.Format (game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_juntuan));//该功能{0}级开启
				return;
			}
			if (m_juntuan_gui == null) {
				load_gui (ref m_juntuan_gui, "ui/juntuan/", "juntuan_gui");
			}
			m_juntuan_gui.SetActive (true);
			m_juntuan_gui.GetComponent<juntuan_gui>().reset ();
			m_main_gui.GetComponent<main_gui>().hide ();
           
			sys._instance.play_sound_ex ("sound/ts_rwpy/tspy_jm13");
 
		} else if (message.m_type == "show_cs_gui") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();

				ulong guid = (ulong)message.m_long [0];
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "chongsheng";
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_card (sys._instance.m_self.get_card_guid (guid));
				m_chongsheng_gui.SetActive (true);
			}
		} else if (message.m_type == "show_jiegu_gui") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();

				ulong guid = (ulong)message.m_long [0];
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "jiegu";
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_jiegu_card (sys._instance.m_self.get_card_guid (guid));
				m_chongsheng_gui.SetActive (true);
			}
		} else if (message.m_type == "show_treasure_cs") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();

				ulong guid = (ulong)message.m_long [0];
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "treasure_chongsheng";
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_baowu (sys._instance.m_self.get_treasure_guid (guid));
				m_chongsheng_gui.SetActive (true);
 
			}
           
		} else if (message.m_type == "show_equip_cs") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();

				ulong guid = (ulong)message.m_long [0];
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "equip_chongsheng";
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_object.Add (guid);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_equip_chongsheng (sys._instance.m_self.get_equip_guid (guid));
				m_chongsheng_gui.SetActive (true);
			}
		} else if (message.m_type == "show_guanghuan_cs") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();
				int id = (int)message.m_ints [0];
				s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan (id);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "guanghuan_chongsheng";
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_guanghuan_chongsheng (t_guanghuan);
				m_chongsheng_gui.SetActive (true);
			}
		} else if (message.m_type == "show_pet_cs_gui") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();
				
				ulong guid = (ulong)message.m_long [0];
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "pet_chongsheng";
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_jiegu_pet (sys._instance.m_self.get_pet_guid (guid));
				m_chongsheng_gui.SetActive (true);
			}
		} else if (message.m_type == "show_pet_fenjie_gui") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();
				
				ulong guid = (ulong)message.m_long [0];
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "pet_fenjie";
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_fenjie_pet (sys._instance.m_self.get_pet_guid (guid));
				m_chongsheng_gui.SetActive (true);
				
			}
		} else if (message.m_type == "show_equip_fenjie") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();

				ulong guid = (ulong)message.m_long [0];
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "equip_fenjie";
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_object.Add (guid);

				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_equip_fenjie (sys._instance.m_self.get_equip_guid (guid));
				m_chongsheng_gui.SetActive (true);
			}
		} else if (message.m_type == "role_yijian_jiegu") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();

				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "jiegu";
				List<ccard> cards = new List<ccard>();
				for (int i = 0; i < message.m_long.Count; i++) {
					ulong guid = (ulong)message.m_long [i];
					m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
					cards.Add (sys._instance.m_self.get_card_guid (guid));
				}
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_role_yijian_fenjie (cards);
				m_chongsheng_gui.SetActive (true);
			}
 
		} else if (message.m_type == "equip_yijian_fenjie") {
			if (m_chongsheng_gui == null) {
				load_gui (ref m_chongsheng_gui, "ui/", "common_chongsheng_gui");
				m_chongsheng_gui.AddComponent<gui_remove>();

				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().flag = false;
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_type = "equip_fenjie";
				List<dhc.equip_t> cards = new List<dhc.equip_t>();
				for (int i = 0; i < message.m_long.Count; i++) {
					ulong guid = (ulong)message.m_long [i];
					m_chongsheng_gui.GetComponent<common_chongsheng_gui>().m_msg.m_long.Add (guid);
					cards.Add (sys._instance.m_self.get_equip_guid (guid));
				}
				m_chongsheng_gui.GetComponent<common_chongsheng_gui>().reset_equip_yijian_fenjie (cards);
				m_chongsheng_gui.SetActive (true);
			}

		} else if (message.m_type == "hide_cs_gui" && m_chongsheng_gui != null) {
			m_chongsheng_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
		} else if (message.m_type == "show_buzheng") {
			bu_zheng ();
			m_main_gui.GetComponent<main_gui>().hide ();
			if (m_huodong_gui != null) {
				m_huodong_gui.SetActive (false);
 
			}
			if (m_catch_gui != null) {
				m_catch_gui.SetActive (false);
 
			}
			if (m_bag_gui != null && m_bag_gui.activeSelf) {
				m_bag_gui.GetComponent<bag_gui>().close ();
			}
            
		} else if (message.m_type == "hide_buzheng" && m_bu_zheng_gui != null) {
			s_message _message = new s_message ();
			_message = new s_message ();
			_message.m_type = "hide_unit_show";
			cmessage_center._instance.add_message (_message);
			bool flag = false;
			if (m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_guanghuan_gui.activeSelf 
				|| m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_up_gui.activeSelf) {
				flag = true;
			}
			m_bu_zheng_gui.GetComponent<ui_show_anim>().hide_ui ();
			if (!flag) {
				m_bu_zheng_gui.GetComponent<gui_remove>().m_remove = false;
			} else {
				m_bu_zheng_gui.GetComponent<gui_remove>().m_remove = true;
			}
		} else if (message.m_type == "hide_buzheng_ex" && m_bu_zheng_gui != null) {
			m_bu_zheng_gui.GetComponent<ui_show_anim>().hide_ui ();
			m_bu_zheng_gui.GetComponent<gui_remove>().m_remove = false;
		} else if (message.m_type == "hide_pet_buzheng" && m_bu_zheng_gui != null) {
			m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_pet_guard_detail.SetActive (false);
			m_bu_zheng_gui.GetComponent<ui_show_anim>().hide_ui ();
			m_bu_zheng_gui.GetComponent<gui_remove>().m_remove = false;
		} else if (message.m_type == "show_sport_shop") {
			show_item_shop_gui ();
		} else if (message.m_type == "show_honor_shop") {
			load_gui (ref m_honor_shop_gui, "ui/", "honor_shop_gui");
			m_honor_shop_gui.AddComponent<gui_remove>();
			m_honor_shop_gui.SetActive (true);
		} else if (message.m_type == "treasure_full_buzheng") {
			sys._instance.m_game_state = "hall";
			sys._instance.load_scene (sys._instance.m_hall_name);
            
			s_message _msg1 = new s_message ();
			_msg1.m_type = "show_buzheng";
			cmessage_center._instance.add_message (_msg1);         
		} else if (message.m_type == "show_baowu_hecheng_gui") {
			sys._instance.m_game_state = "bao_wu";
			sys._instance.load_scene ("ts_game_duobao");
		} else if (message.m_type == "show_shop_other") {
			if (m_partner_shop_gui == null) {
				load_gui (ref m_partner_shop_gui, "ui/", "partner_shop_gui");
			}
			//m_partner_shop_gui.AddComponent<gui_remove>();
			m_partner_shop_gui.SetActive (true);

			m_main_gui.GetComponent<main_gui>().hide ();
			m_shop_panel.GetComponent<ui_show_anim>().hide_ui ();
		} else if (message.m_type == "show_baoshi_shop") {
			show_item_shop_gui ();
			if (sys._instance.m_game_state == "mi_jing") {
				sys._instance.m_game_state = "hall";
				sys._instance.load_scene (sys._instance.m_hall_name);
				hide_huodong_gui ();
			}
		} else if (message.m_type == "show_shop") {
			show_item_shop_gui ();

		} else if (message.m_type == "show_shop_equip") {
			root_gui._instance.m_mi_jing_shop = 1;
			sys._instance.m_game_state = "mi_jing";
			sys._instance.load_scene ("ts_game_mijing");
		} else if (message.m_type == "show_guild_shop") {
			show_guild_shop ();
		} else if (message.m_type == "show_guild_hongbao") {
			show_guild_hongbao ((int)message.m_ints [0]);
		} else if (message.m_type == "show_juntuan_map") {
			show_guild_map ();
		} else if (message.m_type == "show_shop_equip_reward") {
			sys._instance.m_game_state = "mi_jing";
			sys._instance.load_scene ("ts_game_mijing");
		} else if (message.m_type == "show_mo_wang_shop") {
			root_gui._instance.m_shop = 1;
			sys._instance.m_game_state = "boss";
			sys._instance.load_scene ("ts_game_boss");
		}

		if (message.m_type == "show_catch_card") {
			this.gameObject.SetActive (true);
			//m_catch_card_show.gameObject.SetActive(true);
			m_catch_gui.gameObject.SetActive (true);
		}
        if (message.m_type == "hide_catch_card") {
            this.gameObject.SetActive(false);
            //m_catch_card_show.gameObject.SetActive(false);
            //m_catch_gui.gameObject.SetActive(false);
        } else if (message.m_type == "bag_recycle") {
            show_recycle((int)message.m_ints[0], (GameObject)message.m_object[0]);

        } else if (message.m_type == "show_bagfull_gui") {
            if (m_huodong_gui != null) {
                m_huodong_gui.SetActive(false);
            }

            m_main_gui.SetActive(false);

            if (m_catch_gui != null) {
                m_catch_gui.SetActive(false);
            }

            s_message _mes = new s_message();
            _mes.m_type = "hall_anim";
            _mes.m_string.Add("0");
            cmessage_center._instance.add_message(_mes);

            if (m_map_gui != null) {
                m_map_gui.SetActive(false);
                if (sys._instance.m_map.activeSelf == true) {
                    root_gui._instance.show_mask();
                    sys._instance.m_hall_root.SetActive(true);
                    sys._instance.m_map.SetActive(false);
                }
            }
            if (m_bag_gui != null && m_bag_gui.activeSelf) {
                m_bag_gui.GetComponent<bag_gui>().m_item_toogle.value = true;
                m_bag_gui.SetActive(false);

            }
            show_recycle_gui();
            m_recycle_gui.SetActive(true);
            m_main_gui.GetComponent<main_gui>().hide();
            m_recycle_gui.GetComponent<recycle_gui>().equip_fenjie();
            m_recycle_gui.GetComponent<recycle_gui>().m_equip_huishou.GetComponent<UIToggle>().value = true;
            sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm04");

        } else if (message.m_type == "buzheng_show") {
            if (m_map_gui != null) {
                m_map_gui.SetActive(false);
            }

            if (m_huodong_gui != null) {
                m_huodong_gui.SetActive(false);
            }

            m_main_gui.SetActive(false);

            if (m_catch_gui != null) {
                m_catch_gui.SetActive(false);
            }

            show_buzheng_gui();
            m_bu_zheng_gui.SetActive(true);
            if (m_map_gui != null) {
                this.gameObject.SetActive(true);
                m_bu_zheng_gui.GetComponent<bu_zheng_panel>().show_peiyang();
            }

            if (m_huodong_gui != null) {
                this.gameObject.SetActive(true);
                m_bu_zheng_gui.GetComponent<bu_zheng_panel>().show_peiyang();
            }
            s_message _mes = new s_message();
            _mes.m_type = "show_unit_show";
            cmessage_center._instance.add_message(_mes);

            m_main_gui.GetComponent<main_gui>().hide();

        } else if (message.m_type == "card_show") {
            if (m_map_gui != null) {
                m_map_gui.SetActive(false);
            }

            if (m_huodong_gui != null) {
                m_huodong_gui.SetActive(false);
            }

            m_main_gui.SetActive(false);

            if (m_catch_gui != null) {
                m_catch_gui.SetActive(false);
            }

            s_message _mes = new s_message();
            _mes.m_type = "hall_anim";
            _mes.m_string.Add("4");
            cmessage_center._instance.add_message(_mes);

            m_main_gui.GetComponent<main_gui>().hide();

            show_partner_gui();

        } else if (message.m_type == "check_target_done") {
            int tmp = mubiao_mubiao_panel.check_target_done();
            bool flag = true;
            for (int j = 0; j < sys._instance.m_self.m_t_player.finished_tasks.Count; ++j) {
                if (sys._instance.m_self.m_t_player.finished_tasks[j] == tmp) {
                    flag = false;
                }
            }
            if (flag) {
                if (tmp != 0) {
                    s_message _mes = new s_message();
                    _mes.m_type = "show_target_finish";
                    _mes.m_ints.Add(tmp);
                    _mes.m_ints.Add(0);
                    cmessage_center._instance.add_message(_mes);
                }
            }


        } else if (message.m_type == "return_to_main") {
            for (int i = 0; i < this.transform.childCount; ++i) {
                GameObject obj = this.transform.GetChild(i).gameObject;
                if (obj.name != "main_gui") {
                    obj.SetActive(false);
                }
            }

            sys._instance.m_map.SetActive(false);
            sys._instance.m_hall_root.SetActive(true);
            sys._instance.message_clear();

            s_message _mes = new s_message();
            _mes.m_type = "show_main_gui";
            cmessage_center._instance.add_message(_mes);
        } else if (message.m_type == "return_to_main_ex") {
            sys._instance.m_map.SetActive(false);
            sys._instance.m_hall_root.SetActive(true);
        } else if (message.m_type == "check_bf") {
            sys._instance.m_check_bf = true;
        } else if (message.m_type == "check_bf2") {

        } else if (message.m_type == "show_cn_gui") {
            ulong guid = (ulong)message.m_long[0];
            load_gui(ref m_chong_neng_gui, "ui/", "chong_neng_gui_ex");
            m_chong_neng_gui.AddComponent<gui_remove>();
            m_chong_neng_gui.GetComponent<chong_neng_gui>().reset(guid);
            m_chong_neng_gui.transform.gameObject.SetActive(true);
        } else if (message.m_type == "hide_cn_gui" && m_chong_neng_gui != null) {
            m_chong_neng_gui.transform.Find("frame_big").GetComponent<frame>().hide();
        } else if (message.m_type == "show_tu_po_gui") {
            ulong guid = (ulong)message.m_long[0];
            ccard m_card = sys._instance.m_self.get_card_guid(guid);
            load_gui(ref m_tu_po_gui, "ui/", "tu_po_gui_ex");
            m_tu_po_gui.AddComponent<gui_remove>();
            m_tu_po_gui.GetComponent<tu_po_gui>().resert(guid);
            m_tu_po_gui.SetActive(true);
        } else if (message.m_type == "hide_tupo_gui" && m_tu_po_gui != null) {
            m_tu_po_gui.transform.Find("frame_big").GetComponent<frame>().hide();
        } else if (message.m_type == "show_jj_gui") {
            ulong guid = (ulong)message.m_long[0];
            load_gui(ref m_jinjie_gui, "ui/", "jing_jie_gui_ex");
            m_jinjie_gui.AddComponent<gui_remove>();
            m_jinjie_gui.GetComponent<jinjie_gui>().reset(guid);
            m_jinjie_gui.gameObject.SetActive(true);
        } else if (message.m_type == "hide_jinjie_gui" && m_jinjie_gui != null) {
            m_jinjie_gui.transform.Find("frame_big").GetComponent<frame>().hide();
        } else if (message.m_type == "show_first_recharge_gui") {
            show_first_recharge_gui();
            m_main_gui.GetComponent<main_gui>().hide();
        } else if (message.m_type == "show_jn_gui") {
            ulong guid = (ulong)message.m_long[0];
            load_gui(ref m_skill_gui, "ui/", "skill_gui_ex");
            m_skill_gui.AddComponent<gui_remove>();
            m_skill_gui.GetComponent<skill_gui>().reset(guid);
            m_skill_gui.SetActive(true);
        } else if (message.m_type == "show_jn_gui_ex") {
            ulong guid = (ulong)message.m_long[0];
            load_gui(ref m_skill_gui, "ui/", "skill_gui_ex");
            m_skill_gui.AddComponent<gui_remove>();
            m_skill_gui.GetComponent<skill_gui>().reset(guid, 1);
            m_skill_gui.SetActive(true);
            m_skill_gui.GetComponent<skill_gui>().zs_skill();
        } else if (message.m_type == "hide_skill_gui" && m_skill_gui != null) {
            m_skill_gui.transform.Find("frame_big").GetComponent<frame>().hide();
        } else if (message.m_type == "show_qianghua_gui") {
            load_gui(ref m_equip_qianghua_gui, "ui/", "equip_qianghua_gui");
            m_equip_qianghua_gui.AddComponent<gui_remove>();

            ulong guid = (ulong)message.m_long[0];
            dhc.equip_t equip = sys._instance.m_self.get_equip_guid(guid);
            string out_message = (string)message.m_string[0];
            m_equip_qianghua_gui.GetComponent<equip_qianghua_panel>().reset(equip, out_message);
            message.m_long.RemoveAt(0);
            m_equip_qianghua_gui.GetComponent<equip_qianghua_panel>().m_guids.Clear();
            for (int i = 0; i < message.m_long.Count; ++i) {
                m_equip_qianghua_gui.GetComponent<equip_qianghua_panel>().m_guids.Add((ulong)message.m_long[i]);
            }
            m_equip_qianghua_gui.SetActive(true);
        } else if (message.m_type == "show_treasure_qianghua_gui") {
            if (m_treasure_qianghua_gui == null) {
                load_gui(ref m_treasure_qianghua_gui, "ui/", "treasure_qianghua_gui_ex");
            }
            m_treasure_qianghua_gui.GetComponent<treasure_qianghua_gui>().m_treasure_guids.Clear();
            ulong guid = (ulong)message.m_long[0];
            dhc.treasure_t treasure = sys._instance.m_self.get_treasure_guid(guid);
            string out_message = (string)message.m_string[0];
            m_treasure_qianghua_gui.GetComponent<treasure_qianghua_gui>().reset(treasure, out_message);
            message.m_long.RemoveAt(0);
            m_treasure_qianghua_gui.GetComponent<treasure_qianghua_gui>().m_guids.Clear();
            for (int i = 0; i < message.m_long.Count; ++i) {
                m_treasure_qianghua_gui.GetComponent<treasure_qianghua_gui>().m_guids.Add((ulong)message.m_long[i]);
            }
            m_treasure_qianghua_gui.SetActive(true);
        } else if (message.m_type == "show_gaizao_gui") {
            ulong guid = (ulong)message.m_long[0];
            dhc.equip_t equip = sys._instance.m_self.get_equip_guid(guid);
            string out_message = (string)message.m_string[0];
            load_gui(ref m_equip_gaizao_gui, "ui/", "equip_gaizao_gui");
            m_equip_gaizao_gui.AddComponent<gui_remove>();
            m_equip_gaizao_gui.GetComponent<equip_gaizao_panel>().reset(equip, out_message, true);
            m_equip_gaizao_gui.SetActive(true);
        } else if (message.m_type == "show_jianglian_gui") {
            ulong guid = (ulong)message.m_long[0];
            dhc.treasure_t treasure = sys._instance.m_self.get_treasure_guid(guid);
            string out_message = (string)message.m_string[0];

            load_gui(ref m_treasure_jinglian_gui, "ui/", "treasure_jinglian_gui");
            m_treasure_jinglian_gui.GetComponent<treasure_jinglian_gui>().reset(treasure, out_message);
            m_treasure_jinglian_gui.AddComponent<gui_remove>();
            m_treasure_jinglian_gui.SetActive(true);
        } else if (message.m_type == "show_treasure_sx_gui") {
            ulong guid = (ulong)message.m_long[0];
            dhc.treasure_t treasure = sys._instance.m_self.get_treasure_guid(guid);
            string out_message = (string)message.m_string[0];

            load_gui(ref m_treasure_sx_gui, "ui/", "treasure_sx_gui");
            m_treasure_sx_gui.GetComponent<treasure_sx_gui>().m_treasure = treasure;
            m_treasure_sx_gui.GetComponent<treasure_sx_gui>().m_out_message = out_message;
            m_treasure_sx_gui.AddComponent<gui_remove>();
            m_treasure_sx_gui.SetActive(true);
        } else if (message.m_type == "show_zhuzao_gui") {
            ulong guid = (ulong)message.m_long[0];
            dhc.treasure_t treasure = sys._instance.m_self.get_treasure_guid(guid);
            string out_message = (string)message.m_string[0];

            load_gui(ref m_treasure_zhuzao_gui, "ui/", "zhu_zao_gui");
            m_treasure_zhuzao_gui.GetComponent<zhu_zao_gui>().reset(treasure, out_message);
            m_treasure_zhuzao_gui.AddComponent<gui_remove>();
            m_treasure_zhuzao_gui.SetActive(true);
        } else if (message.m_type == "set_arena_end_gui") {
            m_end_rewards.Clear();
            for (int i = 0; i < 3; i++) {
                m_end_rewards.Add((int)message.m_ints[i]);
            }
            m_end_index = (int)message.m_ints[3];
            m_scence = (string)message.m_string[0];
            m_gold = (int)message.m_ints[4];
        } else if (message.m_type == "show_arena_end_gui") {
            s_message _msg = new s_message();
            _msg.m_type = "hide_battle_gui";
            cmessage_center._instance.add_message(_msg);
            if (m_end_index >= 0) {
                root_gui._instance.show_arena_end(m_end_rewards, m_end_index, m_gold, m_scence);
            }

        } else if (message.m_type == "show_master_levelup_gui") {
            root_gui._instance.show_master_levelup((int)message.m_ints[0], (int)message.m_ints[1], message.m_next);

        } else if (message.m_type == "hide_arena_gui") {

            Object.Destroy(m_arena_gui.gameObject);
        } else if (message.m_type == "hide_equip_jl_gui") {
            s_message _message = new s_message();
            _message.m_type = "show_main_gui";
            cmessage_center._instance.add_message(_message);

            m_equip_jl_gui.SetActive(false);
        } else if (message.m_type == "show_hall") {
            this.gameObject.SetActive(true);
        } else if (message.m_type == "hide_hall") {
            this.gameObject.SetActive(false);
        } else if (message.m_type == "show_first_recharge2") {
            show_first_recharge_gui();
        } else if (message.m_type == "hide_first_recharge" && m_first_recharge_gui != null) {
            m_first_recharge_gui.SetActive(false);
        } else if (message.m_type == "show_map_gui2") {
            show_map_gui();
        } else if (message.m_type == "hide_map_gui" && m_map_gui != null) {
            m_map_gui.SetActive(false);
            if (sys._instance.m_hall_root != null) {
                sys._instance.m_hall_root.SetActive(true);
            }
            sys._instance.m_map.SetActive(false);
        } else if (message.m_type == "catch_item") {
            m_quick_item = (int)message.m_ints[0];
        } else if (message.m_type == "show_equip_sx_gui") {
            ulong guid = (ulong)message.m_long[0];
            dhc.equip_t _equip = sys._instance.m_self.get_equip_guid(guid);
            string out_message = (string)message.m_string[0];

            load_gui(ref m_equip_sx_gui, "ui/", "equip_sx_gui");
            m_equip_sx_gui.GetComponent<equip_sx_gui>().reset(_equip, out_message);
            m_equip_sx_gui.AddComponent<gui_remove>();
            m_equip_sx_gui.SetActive(true);
        } else if (message.m_type == "show_equip_jinglian_gui") {
            ulong guid = (ulong)message.m_long[0];
            dhc.equip_t _equip = sys._instance.m_self.get_equip_guid(guid);
            string out_message = (string)message.m_string[0];

            load_gui(ref m_equip_jl_gui, "ui/", "equip_jl_gui");
            m_equip_jl_gui.GetComponent<equip_jl_gui>().reset(_equip, out_message);
            m_equip_jl_gui.AddComponent<gui_remove>();
            m_equip_jl_gui.SetActive(true);
        } else if (message.m_type == "hide_mubiao_gui") {
            m_mubiao_gui.GetComponent<mubiao_gui>().m_richang.GetComponent<UIToggle>().value = true;
            m_mubiao_gui.transform.Find("frame_big").GetComponent<frame>().hide();
        } else if (message.m_type == "show_haoyou") {
            m_main_gui.GetComponent<main_gui>().hide();

            load_gui(ref m_friend_gui, "ui/", "friend_gui");
            m_friend_gui.SetActive(true);
            m_friend_gui.GetComponent<friend_gui>().reset();
        } else if (message.m_type == "show_mrfl") {
            load_gui(ref m_mrfl_gui, "ui/", "day_reward_gui");
            m_mrfl_gui.AddComponent<gui_remove>();
            m_mrfl_gui.SetActive(true);
        } else if (message.m_type == "show_bag_gui") {
            show_bag_gui_ex();
            m_bag_gui.SetActive(true);
            m_bag_gui.GetComponent<bag_gui>().update_ui(true, 2);
        } else if (message.m_type == "show_jc_huodong") {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG_VIEW, _msg);
        } else if (message.m_type == "show_ccl") {
            load_gui(ref m_ccl_gui, "ui/", "ccl_gui");
            m_ccl_gui.AddComponent<gui_remove>();
            m_ccl_gui.SetActive(true);
        } else if (message.m_type == "show_jrdh") {
            load_gui(ref m_jieri_duihuan_gui, "ui/", "jieri_duihuan_gui");
            m_jieri_duihuan_gui.AddComponent<gui_remove>();
            m_jieri_duihuan_gui.GetComponent<jieri_duihuan_gui>().m_huodong_id = (int)message.m_ints[0];
            m_jieri_duihuan_gui.SetActive(true);
        } else if (message.m_type == "show_jd") {
            s_message _mes = new s_message();
            _mes.m_type = "show_jd_gui";
            cmessage_center._instance.add_message(_mes);
        } else if (message.m_type == "show_toper_bf") {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_ph) {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_ph));//该功能{0}级开启
                return;
            }
            load_gui(ref m_toper_gui, "ui/", "toper_gui");
            m_toper_gui.SetActive(true);
            m_toper_gui.GetComponent<toper_gui>().start();
            m_toper_gui.GetComponent<toper_gui>().onEnable();
            m_main_gui.GetComponent<main_gui>().hide();
            m_toper_gui.GetComponent<toper_gui>().bf_toper();
        } else if (message.m_type == "hide_jc_huodong" && m_jc_huodong_gui != null) {
            m_jc_huodong_gui.SetActive(false);
        } else if (message.m_type == "show_pvp_shop") {
            root_gui._instance.m_shop = 1;
            sys._instance.m_game_state = "pvp";
            sys._instance.load_scene("ts_game_lieren");
        } else if (message.m_type == "show_mingyun_zhizhen") {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory) {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_memory));//该功能{0}级开启
                return;
            }
            if (sys._instance.m_game_state != "memory") {
                show_memory_gui();
            }
            root_gui._instance.m_default_active = "show_zhizhen";

        } else if (message.m_type == "show_memory_jihuo") {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory) {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_memory));//该功能{0}级开启
                return;
            }
            if (sys._instance.m_game_state != "memory") {
                show_memory_gui();
            }
            root_gui._instance.m_default_active = "show_huiyilu";
        } else if (message.m_type == "show_mingyun_zhanbu") {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory) {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_memory));//该功能{0}级开启
                return;
            }
            if (sys._instance.m_game_state != "memory") {
                show_memory_gui();
            }
            root_gui._instance.m_default_active = "show_zhanbu";

        } else if (message.m_type == "show_huiyi_shop") {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory) {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_memory));//该功能{0}级开启
                return;
            }
            if (sys._instance.m_game_state != "memory") {
                show_memory_gui();
            }
            root_gui._instance.m_default_active = "show_huiyishop";


        } else if (message.m_type == "show_luck_shop") {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory) {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language("battle_2x.cs_31_72"), (int)e_open_level.el_memory));//该功能{0}级开启
                return;
            }
            if (sys._instance.m_game_state != "memory") {
                show_memory_gui();
            }
            root_gui._instance.m_default_active = "show_luckshop";

        } else if (message.m_type == "show_tanbao") {
            sys._instance.m_game_state = "tanbao";
            sys._instance.load_scene("ts_game_tanbao");
        } else if (message.m_type == "show_explore_gui") {
            sys._instance.m_game_state = "tan_suo";
            sys._instance.load_scene("ts_game_explore");
        } else if (message.m_type == "show_czfp") {
            load_gui(ref m_czfp_gui, "ui/", "czfp_gui");
            m_czfp_gui.AddComponent<gui_remove>();

            m_czfp_gui.SetActive(true);
            m_main_gui.GetComponent<main_gui>().hide();
        } else if (message.m_type == "show_bu_zheng") {
            show_buzheng_gui();
            m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_start = true;
            m_bu_zheng_gui.SetActive(true);
        } else if (message.m_type == "show_bingyuan") {
            sys._instance.m_game_state = "bingyuan";
            sys._instance.load_scene("ts_game_bingyuan");
            s_message _mes = new s_message();
            _mes.m_type = "show_bingyuanshop";
            cmessage_center._instance.add_message(_mes);
        } else if (message.m_type == "show_pet_gui") {
            show_pet_gui();
            m_pet_gui.SetActive(true);
        } else if (message.m_type == "show_pet_cn_gui") {
            ulong guid = (ulong)message.m_long[0];
            if (m_pet_weiyang_gui == null) {
                load_gui(ref m_pet_weiyang_gui, "ui/", "pet_weiyang_gui");
                m_pet_weiyang_gui.AddComponent<gui_remove>();
            }
            m_pet_weiyang_gui.GetComponent<pet_weiyang_gui>().m_message = (string)message.m_string[0];
            m_pet_weiyang_gui.GetComponent<pet_weiyang_gui>().reset(guid);
            m_pet_weiyang_gui.transform.gameObject.SetActive(true);
        } else if (message.m_type == "hide_pet_cn_gui" && m_pet_weiyang_gui != null) {
            m_pet_weiyang_gui.GetComponent<ui_show_anim>().hide_ui();
        } else if (message.m_type == "show_pet_sx_gui") {
            ulong guid = (ulong)message.m_long[0];
            if (m_pet_shengxing_gui == null) {
                load_gui(ref m_pet_shengxing_gui, "ui/", "pet_shengxing_gui");
                m_pet_shengxing_gui.AddComponent<gui_remove>();
            }
            m_pet_shengxing_gui.GetComponent<pet_shengxing_gui>().m_message = (string)message.m_string[0];
            m_pet_shengxing_gui.GetComponent<pet_shengxing_gui>().reset(guid);
            m_pet_shengxing_gui.transform.gameObject.SetActive(true);
        } else if (message.m_type == "hide_pet_sx_gui" && m_pet_shengxing_gui != null) {
            m_pet_shengxing_gui.GetComponent<ui_show_anim>().hide_ui();
        } else if (message.m_type == "show_pet_jj_gui") {
            ulong guid = (ulong)message.m_long[0];
            if (m_pet_jj_gui == null) {
                load_gui(ref m_pet_jj_gui, "ui/", "pet_jingjie_gui");
                m_pet_jj_gui.AddComponent<gui_remove>();
            }
            m_pet_jj_gui.GetComponent<pet_jingjie_gui>().m_message = (string)message.m_string[0];
            m_pet_jj_gui.GetComponent<pet_jingjie_gui>().reset(guid);
            m_pet_jj_gui.transform.gameObject.SetActive(true);
        } else if (message.m_type == "hide_pet_jj_gui" && m_pet_jj_gui != null) {
            m_pet_jj_gui.GetComponent<ui_show_anim>().hide_ui();
        } else if (message.m_type == "show_pet_tj_gui") {
            if (m_pet_jj_gui == null) {
                load_gui(ref m_pet_tj_gui, "ui/", "pet_tj_gui");
                m_pet_tj_gui.AddComponent<gui_remove>();
            }
            m_pet_tj_gui.GetComponent<pet_tj_gui>().reset_tj();
            m_pet_tj_gui.transform.gameObject.SetActive(true);
        } else if (message.m_type == "hide_pet_gui" && m_pet_gui != null) {
            s_message _message = new s_message();
            _message = new s_message();
            _message.m_type = "hide_unit_show";
            cmessage_center._instance.add_message(_message);
            m_pet_gui.GetComponent<ui_show_anim>().hide_ui();
        }
        else if (message.m_type == "five_stars") {
            if (platform_config_common.m_five_star == 1)
            {
                load_gui(ref m_five_stars, "ui/", "five_stars");
                m_five_stars.SetActive(true);
            }
		}
        else if(message.m_type == "show_nationality")
        {
            load_gui(ref m_nationality_gui, "ui/", "nationality_gui");
            m_nationality_gui.GetComponent<select_nationality>().type = (int)message.m_ints[0];
            m_nationality_gui.SetActive(true);
        }

	}
    void show_memory_gui()
    {
        if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72") , (int)e_open_level.el_memory));//该功能{0}级开启
            return;
        }
        sys._instance.m_game_state = "memory";
        sys._instance.load_scene("ts_game_memory");
        m_jiban_panel.GetComponent<ui_show_anim>().hide_ui();
 
    }

	void show_guild_shop()
    {
        if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_juntuan)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72") , (int)e_open_level.el_juntuan ));//该功能{0}级开启
            return;
        }
        if (m_juntuan_gui == null)
        {
            load_gui(ref m_juntuan_gui, "ui/juntuan/", "juntuan_gui");
        }
        m_juntuan_gui.SetActive(true);
        m_juntuan_gui.GetComponent<juntuan_gui>().reset();
        if (sys._instance.m_self.m_t_player.guild > 0)
        {
            m_juntuan_gui.GetComponent<juntuan_gui>().m_guild_shop.gameObject.SetActive(true);
        }
   
        m_main_gui.GetComponent<main_gui>().hide();
        sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm13");
    }
    void show_guild_hongbao(int id)
    {
        if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_juntuan)
        {
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72") , (int)e_open_level.el_juntuan ));//该功能{0}级开启
            return;
        }
        if (m_juntuan_gui == null)
        {
            load_gui(ref m_juntuan_gui, "ui/juntuan/", "juntuan_gui");
        }
        m_juntuan_gui.SetActive(true);
        m_juntuan_gui.GetComponent<juntuan_gui>().reset();
        if (sys._instance.m_self.m_t_player.guild > 0)
        {
            m_juntuan_gui.GetComponent<juntuan_gui>().m_guild_hongbao.SetActive(true);
            m_juntuan_gui.GetComponent<juntuan_gui>().m_guild_hongbao.GetComponent<guild_hongbao_gui>().upde_ui(id);
            if (id == 1)
            {
                m_juntuan_gui.GetComponent<juntuan_gui>().m_guild_hongbao.transform.Find("frame_big").GetComponent<frame>().handle = delegate()
                {
                    m_juntuan_gui.GetComponent<juntuan_gui>().m_guild_hongbao.GetComponent<guild_hongbao_gui>().m_buttonfa1.startsActive = true;
                    m_juntuan_gui.GetComponent<juntuan_gui>().m_guild_hongbao.GetComponent<guild_hongbao_gui>().m_button_fa.startsActive = false;
                    m_juntuan_gui.GetComponent<juntuan_gui>().m_guild_hongbao.GetComponent<guild_hongbao_gui>().m_buttonfa1.value = true;
                   

                };
 
            }
           
        }

        m_main_gui.GetComponent<main_gui>().hide();
        sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm13");
 
    }

	void show_guild_map()
	{
		if (m_juntuan_gui == null)
		{
			load_gui(ref m_juntuan_gui, "ui/juntuan/", "juntuan_gui");
		}
        m_juntuan_gui.SetActive(true);
        m_juntuan_gui.GetComponent<juntuan_gui>().reset();
		if (sys._instance.m_self.m_t_player.guild > 0)
		{
			StartCoroutine(wait_guid_t_retur());

		}

		m_main_gui.GetComponent<main_gui>().hide();
		sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm13");

	}
	IEnumerator wait_guid_t_retur()
	{
		while(juntuan_gui._instance.m_guild_t.level == 0)
		{
			yield return 0;
		}
		m_juntuan_gui.GetComponent<juntuan_gui>().juntuan_map();


	}
	public void show_recycle(int type ,GameObject obj)
	{
		show_recycle_gui ();
		m_recycle_gui.GetComponent<recycle_gui>().m_obj = obj;
		if (type == 1)
        {
            m_recycle_gui.SetActive(true);
            m_main_gui.GetComponent<main_gui>().hide();
            sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm04");
        }
        else if(type == 0)
        {
            m_recycle_gui.SetActive(true);
            m_main_gui.GetComponent<main_gui>().hide();
            m_recycle_gui.GetComponent<recycle_gui>().equip_fenjie();
            m_recycle_gui.GetComponent<recycle_gui>().m_equip_huishou.GetComponent<UIToggle>().value = true;
 
        }
    }
	void bu_zheng()
	{
		show_buzheng_gui ();
		sys._instance.m_card = null;
		m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_dacheng_des.SetActive (false);
		m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_bu_zheng_panel.SetActive (true);
		m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_shenping_panel.SetActive (false);
		m_bu_zheng_gui.GetComponent<bu_zheng_panel>().m_guanghuan_gui.SetActive (false);
		m_bu_zheng_gui.SetActive(true);

		s_message _mes = new s_message();
		_mes.m_type = "show_unit_show";
		cmessage_center._instance.add_message(_mes);
		
		m_main_gui.GetComponent<main_gui>().hide();
	}

	void chongwu_gui()
	{
		show_pet_gui ();

		m_pet_gui.SetActive(true);
		
		s_message _mes = new s_message();
		_mes.m_type = "show_unit_show";
		cmessage_center._instance.add_message(_mes);
		
		m_main_gui.GetComponent<main_gui>().hide();
	}

	void huo_ban(bool init = true)
	{
		show_partner_gui ();

		if(init == true)
		{
			m_partner_gui.transform.GetComponent<partner_gui>().reset ();
		}

		s_message _mes = new s_message();
		_mes.m_type = "hall_anim";
		_mes.m_string.Add("4");
		cmessage_center._instance.add_message(_mes);
		
		m_main_gui.GetComponent<main_gui>().hide();
	}
	void show_arena_gui()
	{
		if (m_arena_gui == null)
		{
			load_gui (ref m_arena_gui, "ui/", "arena_gui");
		}
		m_arena_gui.SetActive(true);
	}

	void show_huodong_gui()
	{
		if(m_huodong_gui == null)
		{
			load_gui (ref m_huodong_gui, "ui/", "huo_dong_gui");
		}

		//m_huodong_gui.AddComponent<gui_remove>();
		m_huodong_gui.SetActive(true);
	}

	void hide_huodong_gui()
	{
		if(m_huodong_gui != null)
		{
			GameObject.Destroy(m_huodong_gui);
		}
	}

	void show_ore_gui()
	{
		if(m_ore_gui == null)
		{
			load_gui (ref m_ore_gui, "ui/", "ore_gui");
			m_ore_gui.SetActive(true);
		}

		m_ore_gui.SetActive (true);
	}
	
	void show_partner_gui()
	{
		if(m_partner_gui == null)
		{
			load_gui (ref m_partner_gui, "ui/", "partner_gui");
			m_partner_gui.GetComponent<partner_gui>().init ();
			m_partner_gui.transform.GetComponent<partner_gui>().reset();
		}

		m_partner_gui.SetActive(true);
	}
	void show_item_shop_gui(int shop_type = 1)
	{
		load_gui (ref m_item_shop_gui, "ui/", "item_shop_gui");
        m_item_shop_gui.GetComponent<item_shop_gui>().m_shop_tyep = shop_type;
		m_item_shop_gui.AddComponent<gui_remove>();
		m_item_shop_gui.SetActive(true);
	}

	void open_kuafu()
	{
		s_message _message = new s_message ();
		_message.m_type = "open_kuafu_reward";
		cmessage_center._instance.add_message (_message);	
		root_gui._instance.m_default_active = "show_rank";
	}
	void show_first_recharge_gui()
	{
		load_gui (ref m_first_recharge_gui, "ui/", "first_recharge_gui");
		m_first_recharge_gui.GetComponent<first_recharge_gui>().init();
		m_first_recharge_gui.AddComponent<gui_remove>();
		m_first_recharge_gui.SetActive(true);
	}

	void show_bag_gui()
	{
		if(m_bag_gui == null)
		{
			load_gui (ref m_bag_gui, "ui/", "bag_gui");
			m_bag_gui.AddComponent<gui_remove>();
		}

		m_bag_gui.SetActive(true);
		m_bag_gui.GetComponent<bag_gui>().reset ();
	}
	void show_map_gui()
	{
		if(m_map_gui == null)
		{
			load_gui (ref m_map_gui, "ui/", "map_gui");
			//m_map_gui.AddComponent<gui_remove>();
		}
		m_map_gui.SetActive(true);
	}
	void fu_ben()
	{
		m_main_gui.GetComponent<main_gui>().hide();
		m_main_gui.SetActive (false);

		show_map_gui ();
		root_gui._instance.m_mask.GetComponent<UIPanel>().alpha = 1.0f;
		root_gui._instance.m_mask.SetActive (true);
	}

	void show_buzheng_gui()
	{
		//布阵
		if(m_bu_zheng_gui == null)
		{
			load_gui (ref m_bu_zheng_gui, "ui/", "bu_zheng_gui_ex");
			m_bu_zheng_gui.AddComponent<gui_remove>();
		}
	}

	void show_pet_gui()
	{
		if(m_pet_gui == null)
		{
			load_gui (ref m_pet_gui, "ui/", "pet_gui");
			m_pet_gui.AddComponent<gui_remove>();
		}
	}

	void show_recycle_gui()
	{
		if(m_recycle_gui == null)
		{
			load_gui(ref m_recycle_gui, "ui/", "recycle_gui");
			m_recycle_gui.GetComponent<recycle_gui>().init();
			m_recycle_gui.AddComponent<gui_remove>();
		}
	}

	void show_bag_gui_ex()
	{
		if(m_bag_gui == null)
		{
			load_gui (ref m_bag_gui, "ui/", "bag_gui");
			m_bag_gui.AddComponent<gui_remove>();
		}
	}

	void show_renweu_gui()
	{
		//目标
		if(m_mubiao_gui == null)
		{
			load_gui (ref m_mubiao_gui, "ui/", "renwu_gui");
			mubiao_mubiao_panel.check_target_done ();
			m_mubiao_gui.AddComponent<gui_remove>();
		}
	}

	
	public void click(GameObject obj)
	{
		if(obj.transform.name == "more")
		{
			m_last_panel.SetActive(false);
            m_more_panel.SetActive(true);
			m_last_button.GetComponent<UIPanel>().alpha = 1;
			m_more_button.GetComponent<UIPanel>().alpha = 0;
            m_deal_time = 4.9f;
		}
		if(obj.transform.name == "last")
		{
			m_last_panel.SetActive(true);
            m_more_panel.SetActive(false);
            m_last_button.GetComponent<UIPanel>().alpha = 0;
			m_more_button.GetComponent<UIPanel>().alpha = 1;
            m_deal_time = 4.9f;
        }
		if(obj.transform.name == "buy_tili")
		{
			s_message message1 = new s_message ();
			message1.m_type = "show_cl_gui";
			message1.m_ints.Add (10010002);
			cmessage_center._instance.add_message(message1);
		}
		if(obj.name == "bao_wu")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_baowu_gui";
			cmessage_center._instance.add_message(_mes);
		}
		if(obj.name == "bao_wu1")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_baowu_gui";
			cmessage_center._instance.add_message(_mes);
		}
		if(obj.transform.name == "liao_tian")
		{
            s_message mes = new s_message();
            mes.m_type = "clickliaotian";
            cmessage_center._instance.add_message(mes);
		
		}
		if(obj.name == "vip_toupiao")
		{
			load_gui (ref m_vip_toupiao_gui, "ui/", "vip_toupiao_gui");
			m_vip_toupiao_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "bu_zheng_last")
		{
			bu_zheng();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm03");
		}
		if(obj.transform.name == "tanbao")
		{
			sys._instance.m_game_state = "tanbao";
			sys._instance.load_scene("ts_game_tanbao");
		}
		if(obj.transform.name == "pet")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pet)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("bu_zheng_panel.cs_2819_16"),(int)e_open_level.el_pet));//宠物{0}级开启
				return;
			}
			if(sys._instance.m_self.m_t_player.pets.Count <= 0)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("hall_gui.cs_2134_46"));//[ffc882]您还尚未拥有宠物
				return;
			}
			chongwu_gui();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm03");
		}
		if(obj.transform.name == "tansuo")
		{
			sys._instance.m_game_state = "tan_suo";
			sys._instance.load_scene("ts_game_explore");
		}
		if(obj.transform.name == "nv_tuan")
		{
			s_message _message = new s_message();
			_message.m_type = "show_nvtuan";
			cmessage_center._instance.add_message(_message);
		}
		if(obj.transform.name == "huo_dong_last")
		{
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mx)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72") , (int)e_open_level.el_mx));//该功能{0}级开启
				return;
			}

			show_huodong_gui();
			m_huodong_gui.GetComponent<huo_dong_gui>().m_select = 6;
			m_huodong_gui.GetComponent<huo_dong_gui>().flag = true;
			m_huodong_gui.GetComponent<huo_dong_gui>().offset = 0;
			m_huodong_gui.GetComponent<huo_dong_gui>().update_ui();

			m_main_gui.GetComponent<main_gui>().hide();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm10");
		}
		if(obj.transform.name == "VR")
		{
			root_gui._instance.show_mask();
			sys._instance.m_game_state = "vr";
			sys._instance.load_scene("ts_game_vr");

		}
		if(obj.transform.name == "kai_fu_huodong")
		{
			load_gui (ref m_kaifu_gui, "ui/", "kaifu_gui");
			m_kaifu_gui.AddComponent<gui_remove>();
			
			m_kaifu_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "kai_fu_huodong_ex")
		{
			load_gui (ref m_kaifu_gui_ex, "ui/", "kaifu_gui_ex");
			m_kaifu_gui_ex.AddComponent<gui_remove>();
			
			m_kaifu_gui_ex.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "jingcai_huo_dong")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_VIEW, _msg);
		}
		if(obj.transform.name == "lian_jing")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_jd_gui";
			cmessage_center._instance.add_message(_mes);
		}

		if(obj.transform.name == "sign")
		{
			load_gui (ref m_daily_sign_gui, "ui/", "dsign_gui");

			m_daily_sign_gui.AddComponent<gui_remove>();
			m_daily_sign_gui.SetActive(true);

			m_main_gui.GetComponent<main_gui>().hide();
		}
        if (obj.transform.name == "vip")
        {
            s_message _mes = new s_message();
            _mes.m_type = "show_recharge";
            _mes.m_ints.Add(1);
            _mes.m_ints.Add(sys._instance.m_self.m_t_player.vip);
            cmessage_center._instance.add_message(_mes);
        }
        if (obj.transform.name == "shouchong")
        {
            show_first_recharge_gui();
            m_main_gui.GetComponent<main_gui>().hide();
        }
        if (obj.transform.name == "chong_zhi")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);
		}
		if(obj.transform.name == "fu_ben")
		{
			fu_ben();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm06");
		}
		if(obj.transform.name == "huo_ban")
		{
			huo_ban();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm01");
		}
		if(obj.transform.name == "recycle_last")
		{
			show_recycle_gui();
			m_recycle_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm04");
		}
		if(obj.transform.name == "mu_biao_last")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_richang)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" +  string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72") , (int)e_open_level.el_richang));//该功能{0}级开启
				return;
			}
			show_renweu_gui();
			m_mubiao_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm13");
		}

		if (obj.transform.name == "jun_tuan")
		{
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_juntuan)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_juntuan));//该功能{0}级开启
				return;
			}
			if (m_juntuan_gui == null)
			{
				load_gui (ref m_juntuan_gui, "ui/juntuan/", "juntuan_gui");
			}
			m_juntuan_gui.SetActive(true);
			m_juntuan_gui.GetComponent<juntuan_gui>().reset();
			m_main_gui.GetComponent<main_gui>().hide();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm13");
		}

		if(obj.transform.name == "bei_bao_last")
		{
			show_bag_gui();

			m_main_gui.GetComponent<main_gui>().hide();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm12");
		}

		if(obj.transform.name == "zhao_mu_last")
		{
			if (m_catch_gui == null)
			{
				load_gui (ref m_catch_gui, "ui/", "catch_card_gui");
			}
			m_catch_gui.GetComponent<catch_card_gui>().reset();
			m_catch_gui.SetActive(true);
			
			s_message _mes = new s_message();
			
			_mes.m_type = "hall_anim";
			_mes.m_string.Add("1");
			
			cmessage_center._instance.add_message(_mes);
			
			m_main_gui.GetComponent<main_gui>().hide();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm02");
		}
		if(obj.transform.name == "hei_shi")
		{
			show_item_shop_gui();
			m_main_gui.GetComponent<main_gui>().hide();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm11");
			m_shop_panel.GetComponent<ui_show_anim>().hide_ui();
		}
		if(obj.transform.name == "you_jian")
		{
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_post)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_post));//该功能{0}级开启
				return;
			}
			load_gui (ref m_post_gui, "ui/", "post_gui");
			m_post_gui.AddComponent<gui_remove>();
			m_post_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "online_reward")
		{
			load_gui (ref m_online_reward_gui, "ui/", "online_reward_gui");
			m_online_reward_gui.AddComponent<gui_remove>();
			m_online_reward_gui.SetActive(true);
		}
		if (obj.transform.name == "shui_ji")
		{
			s_message _mes = new s_message();
			_mes.m_type = "show_talk";
			cmessage_center._instance.add_message(_mes);
		}
       
		if (obj.transform.name == "xin_qing")
		{
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_yuehui)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_yuehui));//该功能{0}级开启
				return;
			}
            m_jiban_panel.GetComponent<ui_show_anim>().hide_ui();
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_ROLE_YH_LOOK, _msg);
		}
        if (obj.name == "memory")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_memory));//该功能{0}级开启
                return;
            }
            sys._instance.m_game_state = "memory";
            sys._instance.load_scene("ts_game_memory");
            m_jiban_panel.GetComponent<ui_show_anim>().hide_ui();
        }
		if(obj.transform.name == "self")
		{
			load_gui (ref m_info_gui, "ui/", "info_gui");
			m_info_gui.AddComponent<gui_remove>();
			m_info_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "pai_hang")
		{
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_ph)
			{
				root_gui._instance.show_prompt_dialog_box(string.Format("[ffc882]" + game_data._instance.get_t_language ("battle_2x.cs_31_72"),(int)e_open_level.el_ph));//该功能{0}级开启
				return;
			}
			load_gui (ref m_toper_gui, "ui/", "toper_gui");
			m_toper_gui.SetActive(true);
            m_toper_gui.GetComponent<toper_gui>().start();
            m_toper_gui.GetComponent<toper_gui>().onEnable();
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "hb_shop")
		{
            if (m_partner_shop_gui == null)
            {
                load_gui(ref m_partner_shop_gui, "ui/", "partner_shop_gui");
            }
			m_partner_shop_gui.SetActive(true);
			
			m_main_gui.GetComponent<main_gui>().hide();
			m_shop_panel.GetComponent<ui_show_anim>().hide_ui();
		}
		if(obj.transform.name == "shop_last")
		{
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_shop)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_shop));//该功能{0}级开启
				return;
			}
			m_shop_panel.SetActive(true);
            m_deal_time = 4.9f;
		}
        if (obj.name == "ji_ban_last")
        {
            m_jiban_panel.SetActive(true);
        }
		if(obj.transform.name == "jiban_back")
		{
            m_jiban_panel.GetComponent<ui_show_anim>().hide_ui();
		}
		if(obj.transform.name == "shop_back")
		{
			m_shop_panel.GetComponent<ui_show_anim>().hide_ui();
		}
		if(obj.transform.name == "hao_you")
		{
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_friend)
			{
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_friend));//该功能{0}级开启
				return;
			}
			m_main_gui.GetComponent<main_gui>().hide();

			load_gui (ref m_friend_gui, "ui/", "friend_gui");
			m_friend_gui.SetActive(true);
            m_friend_gui.GetComponent<friend_gui>().reset();
		}
		if (obj.transform.name == "zhu_shou")
		{
			s_message _out = new s_message();
			_out.m_type = "show_mm_show";
			cmessage_center._instance.add_message(_out);
		}
		if(obj.transform.name == "jieri_huodong")
		{
			load_gui (ref m_jieri_huodong_gui, "ui/", "jieri_huodong_gui");
			m_jieri_huodong_gui.AddComponent<gui_remove>();
			
			m_jieri_huodong_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "gaizao")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jiyingaizao)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("hall_gui.cs_2521_73"), (int)e_open_level.el_jiyingaizao ));//基因改造{0}级开启
				return;
			}
			if(m_jiyin_gaizao_gui == null)
			{
				load_gui(ref m_jiyin_gaizao_gui, "ui/", "jiyin_gaizao_gui");
				m_jiyin_gaizao_gui.AddComponent<gui_remove>();
			}
			m_jiyin_gaizao_gui.GetComponent<jiyin_gaizao_gui>().select_id = 0;
			m_jiyin_gaizao_gui.GetComponent<jiyin_gaizao_gui>().select_num = 0;
			m_jiyin_gaizao_gui.GetComponent<jiyin_gaizao_gui>().hc_id = 0;
			m_jiyin_gaizao_gui.GetComponent<jiyin_gaizao_gui>().hc_num =0;
			m_jiyin_gaizao_gui.GetComponent<jiyin_gaizao_gui>().reset();
			m_jiyin_gaizao_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "czfp")
		{
			load_gui (ref m_czfp_gui, "ui/", "czfp_gui");
			m_czfp_gui.AddComponent<gui_remove>();
			
			m_czfp_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
		if(obj.transform.name == "zhuanpan")
		{
			load_gui (ref m_zhuanpan_gui, "ui/", "rotary_table_gui");
			m_zhuanpan_gui.AddComponent<gui_remove>();
			
			m_zhuanpan_gui.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
        if (obj.name == "mofang")
        {
            load_gui(ref m_mofang_gui, "ui/", "jiugong_mofang_gui");
            m_mofang_gui.AddComponent<gui_remove>();
            m_mofang_gui.SetActive(true);
            m_main_gui.GetComponent<main_gui>().hide();
        }
		if(obj.transform.name == "chenghao")
		{
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_chenghao)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("hall_gui.cs_2564_73") , (int)e_open_level.el_chenghao));//称号{0}级开启
				return;
			}
			load_gui (ref m_chenghao, "ui/", "chenhao_gui");
			m_chenghao.AddComponent<gui_remove>();
			
			m_chenghao.SetActive(true);
			m_main_gui.GetComponent<main_gui>().hide();
		}
        if (obj.name == "huiyi_shop")
        {
            if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
            {
                root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("battle_2x.cs_31_72"), (int)e_open_level.el_memory));//该功能{0}级开启
                return;
            }
            if (sys._instance.m_game_state != "memory")
            {
                show_memory_gui();
            }
            root_gui._instance.m_default_active = "show_huiyishop";
            m_shop_panel.GetComponent<ui_show_anim>().hide_ui();
        }
        if (obj.name == "cw_shop")
        {
            s_message _message = new s_message();
            _message.m_type = "show_master_gui";
            sys._instance.m_state_flag = 1;
            cmessage_center._instance.add_message(_message);
            sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm03");
            m_shop_panel.GetComponent<ui_show_anim>().hide_ui();
        }
		if (obj.transform.name == "huigui") 
		{
			load_gui (ref m_huigui_panel, "ui/", "hero_comeback_gui");
            m_huigui_panel.SetActive(true);
            m_main_gui.GetComponent<main_gui>().hide();
			
		}
		if (obj.transform.name == "download") 
		{
			load_gui(ref m_download_gui, "ui/", "download_gui");
			root_gui._instance.wait(true);
		}
        if (obj.transform.name == "infinite_shop")
        {
            show_item_shop_gui(platform_config_common.game_model);
        }
        if (obj.transform.name == "gua_ji")
        {
            game_data._instance.m_guaji = 1;
            fu_ben();
            sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm06");

        }
    }

	void Update () {
		m_deal_time += Time.deltaTime;
		if (m_need_deal || m_deal_time > 5 && m_main_gui.activeSelf)
		{
			deal_main_gui();
			m_need_deal = false;
			m_deal_time = 0;
			m_main_gui.GetComponent<main_gui>().m_need_update = true;
		}
		if(sys._instance.m_xq_check)
		{
			sys._instance.xq_check();
		}
		if (sys._instance.m_need_check)
		{
			sys._instance.player_check();
		}

	}


	void deal_main_gui() {

		if(sys._instance.m_self.m_t_player == null)
		{
			return;
		}

		bool last_effect = false;
		bool more_effect = false;
        // 在线领奖
		s_t_online_reward tor = game_data._instance.get_t_online_reward (sys._instance.m_self.m_t_player.online_reward_index);
		if (tor != null)
		{
			ulong dm = 0;
			ulong now = timer.now ();
			if (now <= sys._instance.m_self.m_t_player.online_reward_time)
			{
				dm = sys._instance.m_self.m_t_player.online_reward_time - now;
			}
			if (dm == 0)
			{
				s_message _msg = new s_message();
				_msg.m_type = "show_main_icon_effect";
				_msg.m_string.Add("online_reward");
				cmessage_center._instance.add_message(_msg);
			}
			else
			{
				s_message _msg = new s_message();
				_msg.m_type = "hide_main_icon_effect";
				_msg.m_string.Add("online_reward");
				_msg.m_long.Add(dm);
				cmessage_center._instance.add_message(_msg);
			}
		}

        // 日常
		if (mubiao_gui.is_effect())
		{
			last_effect = true;
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("mu_biao_last");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("mu_biao_last");
			cmessage_center._instance.add_message(_msg);
		}

        // 背包
		if (bag_gui.is_effect() )
		{
			last_effect = true;
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("bei_bao_last");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("bei_bao_last");
			cmessage_center._instance.add_message(_msg);
		}
        
        // 阵容
		if (bu_zheng_panel.bu_zheng_effect() || duixing_gui.up_duixing())
		{
			last_effect = true;
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("bu_zheng_last");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("bu_zheng_last");
			cmessage_center._instance.add_message(_msg);
		}

        // 招募
		if (catch_card_gui.can_zhaomu())
		{
			last_effect = true;
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("zhao_mu_last");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("zhao_mu_last");
			cmessage_center._instance.add_message(_msg);
		}

        // 商店
		if (partner_shop_gui.effect())
		{
			last_effect = true;
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("shop_last");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("shop_last");
			cmessage_center._instance.add_message(_msg);
		}

        // 伙伴商店
		if (partner_shop_gui.effect())
		{
			last_effect = true;
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("hb_shop");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("hb_shop");
			cmessage_center._instance.add_message(_msg);
        }

        // 冒险
        if (huo_dong_gui.can_effect())
        {
            last_effect = true;
            s_message _msg = new s_message();
            _msg.m_type = "show_main_icon_effect";
            _msg.m_string.Add("huo_dong_last");
            cmessage_center._instance.add_message(_msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "hide_main_icon_effect";
            _msg.m_string.Add("huo_dong_last");
            cmessage_center._instance.add_message(_msg);
        }

        // 精彩活动
        if (jc_huodong_gui.is_effect() || sys._instance.m_self.m_jc_huodong_effect == 1)
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("jingcai_huo_dong");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("jingcai_huo_dong");
			cmessage_center._instance.add_message(_msg);
		}

        // 不知道
		if (sys._instance.m_self.m_t_player.vip_reward_ids.Count < sys._instance.m_self.m_t_player.vip)
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("vb");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("vb");
			cmessage_center._instance.add_message(_msg);
		}

        // 邮件
		if (post_gui.can_post())
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("you_jian");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("you_jian");
			cmessage_center._instance.add_message(_msg);
		}

		// 好友
		if ((sys._instance.m_self.is_friend_apply > 0 || sys._instance.m_self.is_friend_tili > 0) &&( sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_friend))
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("hao_you");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("hao_you");
			cmessage_center._instance.add_message(_msg);
		}
        // 军团
        if (sys._instance.m_self.m_t_player.guild > 0 && (sys._instance.m_self.m_can_guild > 0) && sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_juntuan)
        {
            s_message _msg = new s_message();
            _msg.m_type = "show_main_icon_effect";
            _msg.m_string.Add("jun_tuan");
            cmessage_center._instance.add_message(_msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "hide_main_icon_effect";
            _msg.m_string.Add("jun_tuan");
            cmessage_center._instance.add_message(_msg);
        }
        // 心情
        bool flag = false;
        if (xq_gui.effect())
        {
            flag = true;
            s_message _msg = new s_message();
            _msg.m_type = "show_main_icon_effect";
            _msg.m_string.Add("xin_qing");
            cmessage_center._instance.add_message(_msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "hide_main_icon_effect";
            _msg.m_string.Add("xin_qing");
            cmessage_center._instance.add_message(_msg);
        }
        // 回忆录
        if (((1 - sys._instance.m_self.m_t_player.huiyi_chou_num) > 0 ||
            sys._instance.m_self.m_item_shop_effect > 0 ||
            huiyilu_gui.is_jihuo().Count > 0 ||
            (game_data._instance.get_t_vip(sys._instance.m_self.m_t_player.vip).huiyi_zhanbu_num - sys._instance.m_self.m_t_player.huiyi_zhanpu_num) > 0)
            && sys._instance.m_self.m_t_player.level >= (int)e_open_level.el_memory)
        {
            flag = true;
            s_message _msg = new s_message();
            _msg.m_type = "show_main_icon_effect";
            _msg.m_string.Add("memory");
            cmessage_center._instance.add_message(_msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "hide_main_icon_effect";
            _msg.m_string.Add("memory");
            cmessage_center._instance.add_message(_msg);
        }
        // 羁绊
        if (flag)
        {
			more_effect = true;
            s_message _msg = new s_message();
            _msg.m_type = "show_main_icon_effect";
            _msg.m_string.Add("ji_ban_last");
            cmessage_center._instance.add_message(_msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "hide_main_icon_effect";
            _msg.m_string.Add("ji_ban_last");
            cmessage_center._instance.add_message(_msg);
        }
        // 7天活动
		if (sys._instance.m_self.m_can_kaifu == 1)
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("kai_fu_huodong");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("kai_fu_huodong");
			cmessage_center._instance.add_message(_msg);
		}
        // 14天活动
		if (timer.run_day(sys._instance.m_self.m_t_player.birth_time) < 15 && timer.run_day(sys._instance.m_self.m_t_player.birth_time) >= 7)
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("kai_fu_huodong_ex");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("kai_fu_huodong_ex");
			cmessage_center._instance.add_message(_msg);
		}
        // 节日活动
		if (sys._instance.m_self.m_can_jieri_huodong == 1)
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("jieri_huodong");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("jieri_huodong");
			cmessage_center._instance.add_message(_msg);
		}
        // 签到
		if (sign_gui.effect())
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("sign");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("sign");
			cmessage_center._instance.add_message(_msg);
		}
		
        // 首充
        if (sys._instance.m_self.m_t_player.first_reward == 1)
        {
            s_message _msg = new s_message();
            _msg.m_type = "show_main_icon_effect";
            _msg.m_string.Add("shouchong");
            cmessage_center._instance.add_message(_msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "hide_main_icon_effect";
            _msg.m_string.Add("shouchong");
            cmessage_center._instance.add_message(_msg);
        }

        // vr
		if (vr_gui.effect() || dress_gui.is_effect())
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("VR");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("VR");
			cmessage_center._instance.add_message(_msg);
		}

        // 关卡
		if (map_gui.map_effect() || qiyu_gui.qiyu_effect())
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("fu_ben");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("fu_ben");
			cmessage_center._instance.add_message(_msg);
		}
        // 称号
		if(sys._instance.chenghao_effect > 0 && sys._instance.m_self.m_t_player.level > (int)e_open_level.el_chenghao)
		{
			more_effect = true;
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("chenghao");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("chenghao");
			cmessage_center._instance.add_message(_msg);
		}
        // 宠物
		if(pet_gui.is_effect() && sys._instance.m_self.m_t_player.level > (int)e_open_level.el_pet)
		{
			more_effect = true;
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("pet");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("pet");
			cmessage_center._instance.add_message(_msg);
        }
        // last more
        if (last_effect)
        {
            s_message _msg = new s_message();
            _msg.m_type = "show_main_icon_effect";
            _msg.m_string.Add("last");
            cmessage_center._instance.add_message(_msg);
        }
        else
        {
            s_message _msg = new s_message();
            _msg.m_type = "hide_main_icon_effect";
            _msg.m_string.Add("last");
            cmessage_center._instance.add_message(_msg);
        }
        if (more_effect)
		{
			s_message _msg = new s_message();
			_msg.m_type = "show_main_icon_effect";
			_msg.m_string.Add("more");
			cmessage_center._instance.add_message(_msg);
		}
		else
		{
			s_message _msg = new s_message();
			_msg.m_type = "hide_main_icon_effect";
			_msg.m_string.Add("more");
			cmessage_center._instance.add_message(_msg);
		}

	}
}
