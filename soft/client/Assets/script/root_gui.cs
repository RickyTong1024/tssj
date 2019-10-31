
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class root_gui : MonoBehaviour,IMessage {
	public static root_gui _instance;
	[System.NonSerialized]
	public string m_hall_guide = "";
	[System.NonSerialized]
	public GameObject m_battle_gui;
    [System.NonSerialized]
    public GameObject m_hall_gui;
	[System.NonSerialized]
	public GameObject m_bw_gui;
	private GameObject m_login_gui;
	private GameObject m_boss_gui;
	private GameObject m_pvp_gui;
	private GameObject m_bingyuan_gui;
    private GameObject m_master_gui;
	private GameObject m_bf_gui;
	private GameObject m_guild_boss_gui;
	private GameObject m_memory_gui;
	private GameObject m_yj_gui;
	private GameObject m_ts_gui;
	private GameObject m_mj_gui;
	[System.NonSerialized]
	public GameObject m_tanbao_gui;
	private GameObject m_vr_gui;
	private GameObject m_transport_gui;
    private GameObject m_guild_pvp_gui;
    private GameObject m_guild_fight_gui;
	[System.NonSerialized]
	public GameObject m_talk;
	[System.NonSerialized]
	public GameObject m_mask;
	[System.NonSerialized]
	public GameObject m_wait;
    private GameObject m_chat_gui;

	private GameObject m_min_dialog_box;
	private GameObject m_item_dialog_box;
	private GameObject m_chneghao_dialog_box;
	private GameObject m_equip_dialog_box;
	private GameObject m_guanghuan_dialog_box;
	private GameObject m_card_dialog_box;
	private GameObject m_pet_dialog_box;
	private GameObject m_huiyi_dialog_box;
	private GameObject m_treasure_dialog_box;
	private GameObject m_select_dialog_box;
	private GameObject m_single_dialog_box;
	private GameObject m_recharge_dialog_box;
	private GameObject m_selcet_mianyi_box;
	private GameObject m_tili_dialog_box;
	private GameObject m_cishu_dialog_box;
	private GameObject m_jd_dialog_box;
	private GameObject m_other_dialog_box;
	private GameObject m_duixing_gui;
	[System.NonSerialized]
	public GameObject m_buy_num_gui;
    [System.NonSerialized]
    public GameObject m_select_num_gui;
	[System.NonSerialized]
	public GameObject m_common_card_page_gui;
	[System.NonSerialized]
	public GameObject m_common_equip_page_gui;
	[System.NonSerialized]
	public GameObject m_common_treasure_page_gui;
	[System.NonSerialized]
	public GameObject m_equip_detail;
	[System.NonSerialized]
	public GameObject m_common_pet_page_gui;
	[System.NonSerialized]
	public GameObject m_common_item_page_gui;
	[System.NonSerialized]
	private GameObject m_pet_detail;
    [System.NonSerialized]
    public GameObject m_treasure_detail;
	private GameObject m_dress_detail;
	private GameObject m_common_player_panel;
    private GameObject m_player_info_gui;
	private GameObject m_common_cl_panel;
	[System.NonSerialized]
	public GameObject m_guide_gui;
	private GameObject m_unit_show;
	private GameObject m_level_gui;
	private GameObject m_target_finish_gui;
	private GameObject m_jd_gui;
	private GameObject m_recharge_gui;
	private GameObject m_dress_target_finsh_gui;
	private GameObject m_card_info;
	private GameObject m_hb_dress_gui;
	[System.NonSerialized]
	public GameObject m_dress_gui;
	private GameObject m_arena_max;
	private GameObject m_zhaomu_shuxing;
	private GameObject m_ui_mask;
	private GameObject m_other_reward;
	[System.NonSerialized]
	public GameObject m_danmu_panel;
	private GameObject m_bu_zheng_help_gui;
	private GameObject m_loading_ex;
	[System.NonSerialized]
	public GameObject m_loading;
	private GameObject m_arena_end;
    private GameObject m_master_levelup;
	private GameObject m_select_item_gui;
	
	public GameObject m_ui_bottomleft;
	public GameObject m_ui_bottomleft_1;
	
	public GameObject m_ui_cam;
	private float m_ui_mask_time = 0.0f;
	private bool m_wait_active = false;
	[System.NonSerialized]
	public int m_mi_jing_shop = 0;
	[System.NonSerialized]
	public int m_shop = 0;
    [System.NonSerialized]
    public string m_default_active = "";

	void Awake()
	{
		_instance = this;

		float _height = (float)Screen.height;
		float _width = (float)Screen.width;
		
		if(_height > 800)
		{
			_width = _width * (800.0f / _height);
			_height = 800.0f;
		}
		
		if(_width / _height < 1.5f)
		{
			if(_height > 800)
			{
				_height = 800;
			}
			
			m_ui_cam.transform.parent.GetComponent<UIRoot>().maximumHeight = (int)_height;
			m_ui_cam.transform.parent.GetComponent<UIRoot>().minimumHeight = (int)_height;
		}
	}

	void Start ()
	{
		cmessage_center._instance.add_handle (this);
		init_comm_ui();
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}


	public void show_battle_gui()
	{
        if (m_battle_gui == null)
        {
            m_battle_gui = game_data._instance.ins_object_res("ui/battle_gui");
            m_battle_gui.transform.parent = m_ui_cam.transform;
            m_battle_gui.transform.localPosition = new Vector3(0, 0, 0);
            m_battle_gui.transform.localScale = new Vector3(1, 1, 1);
            m_battle_gui.AddComponent<gui_remove>();
 
        }
		m_battle_gui.SetActive (true);
	}
	
	public void hide_battle_gui()
	{
		if(m_battle_gui != null)
		{
			m_battle_gui.SetActive(false);
		}
	}

	public void show_recharge_gui(int type, int vip = -1)
	{
		if(m_recharge_gui == null)
		{
			m_recharge_gui = game_data._instance.ins_object_res("ui/recharge_gui");
			m_recharge_gui.transform.parent = m_ui_cam.transform;
			m_recharge_gui.transform.localPosition = new Vector3(0,0,0);
			m_recharge_gui.transform.localScale = Vector3.one;
		}
		
		m_recharge_gui.transform.GetComponent<recharge_gui>().m_type = type;
		m_recharge_gui.transform.GetComponent<recharge_gui>().m_vip = vip;
		m_recharge_gui.SetActive(true);
	}
	
	void show_jd_gui()
	{
		if(m_jd_gui != null)
		{
			return;
		}

		m_jd_gui = game_data._instance.ins_object_res("ui/jd_gui");
		m_jd_gui.transform.parent = m_ui_cam.transform;
		m_jd_gui.transform.localPosition = new Vector3(0,0,0);
		m_jd_gui.transform.localScale = Vector3.one;
		m_jd_gui.AddComponent<gui_remove>();
		m_jd_gui.SetActive(true);
	}
	
	void show_bz_help_gui()
	{
		if(m_bu_zheng_help_gui != null)
		{
			return;
		}

		m_bu_zheng_help_gui = game_data._instance.ins_object_res("ui/bu_zheng_help_gui");
		m_bu_zheng_help_gui.transform.parent = m_ui_cam.transform;
		m_bu_zheng_help_gui.transform.localPosition = new Vector3(0,0,0);
		m_bu_zheng_help_gui.transform.localScale = Vector3.one;
		m_bu_zheng_help_gui.AddComponent<gui_remove>();
		m_bu_zheng_help_gui.SetActive(true);
	}
	
	void hide_bz_help_gui()
	{
		if(m_bu_zheng_help_gui != null)
		{
			m_bu_zheng_help_gui.SetActive(false);
		}
	}
	
	public void show_login_gui()
	{
		m_login_gui = game_data._instance.ins_object_res("ui/login_gui");
		m_login_gui.transform.parent = m_ui_cam.transform;
		m_login_gui.transform.localPosition = new Vector3(0,0,0);
		m_login_gui.transform.localScale = new Vector3(1,1,1);
		m_login_gui.AddComponent<gui_remove>();
		m_login_gui.SetActive (true);
	}
	
	public void hide_login_gui()
	{
		if(m_login_gui != null)
		{
			m_login_gui.SetActive(false);
		}
	}
	
	void show_boss_gui()
	{
		if (m_boss_gui == null)
		{
			m_boss_gui = game_data._instance.ins_object_res("ui/boss_gui");
			m_boss_gui.transform.parent = m_ui_cam.transform;
			m_boss_gui.transform.localPosition = new Vector3(0,0,0);
			m_boss_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_boss_gui.SetActive (true);
	}
	
	void show_pvp_gui()
	{
		if (m_pvp_gui == null)
		{
			m_pvp_gui = game_data._instance.ins_object_res("ui/pvp_gui");
			m_pvp_gui.transform.parent = m_ui_cam.transform;
			m_pvp_gui.transform.localPosition = new Vector3(0, 0, 0);
			m_pvp_gui.transform.localScale = new Vector3(1, 1, 1);
            m_pvp_gui.GetComponent<pvp_gui>().add_handle();
		}
		m_pvp_gui.SetActive(true);
	}
	
	void show_bingyuan_gui()
	{
        if (m_bingyuan_gui == null)
        {
            m_bingyuan_gui = game_data._instance.ins_object_res("ui/bingyuan_gui");
            m_bingyuan_gui.transform.parent = m_ui_cam.transform;
            m_bingyuan_gui.transform.localPosition = new Vector3(0, 0, 0);
            m_bingyuan_gui.transform.localScale = new Vector3(1, 1, 1);
        }
        m_bingyuan_gui.GetComponent<bingyuan_gui>().add_handle();
        m_bingyuan_gui.SetActive(true);
		
	}
    void show_master_gui()
    {
        if (m_master_gui == null)
        {
            m_master_gui = game_data._instance.ins_object_res("ui/masterLeague_gui");
            m_master_gui.transform.parent = m_ui_cam.transform;
            m_master_gui.transform.localPosition = new Vector3(0, 0, 0);
            m_master_gui.transform.localScale = new Vector3(1, 1, 1);
        }
        m_master_gui.SetActive(true);
 
    }
	void show_guild_boss_gui()
	{
		if (m_guild_boss_gui == null)
		{
			m_guild_boss_gui = game_data._instance.ins_object_res("ui/juntuan/guild_boss_ex");
			m_guild_boss_gui.transform.parent = m_ui_cam.transform;
			m_guild_boss_gui.transform.localPosition = new Vector3(0, 0, 0);
			m_guild_boss_gui.transform.localScale = new Vector3(1, 1, 1);
		}
		m_guild_boss_gui.SetActive(true);
		
	}
	void show_memory_gui()
	{
		if (m_memory_gui == null)
		{
			m_memory_gui = game_data._instance.ins_object_res("ui/memory_gui");
			m_memory_gui.transform.parent = m_ui_cam.transform;
			m_memory_gui.transform.localPosition = new Vector3(0, 0, 0);
			m_memory_gui.transform.localScale = new Vector3(1, 1, 1);
            m_memory_gui.GetComponent<memory_gui>().add_handle();
			
		}
		m_memory_gui.SetActive(true);
	}
	public void hide_boss_gui(bool is_del)
	{
		if(m_boss_gui != null)
		{
			m_boss_gui.SetActive (false);
			if (is_del)
			{
				Object.Destroy (m_boss_gui);
			}
		}
	}
    void show_guild_pvp_gui()
    {
        if (m_guild_pvp_gui == null)
        {
            m_guild_pvp_gui = game_data._instance.ins_object_res("ui/guild_pvp_gui");
            m_guild_pvp_gui.transform.parent = m_ui_cam.transform;
            m_guild_pvp_gui.transform.localPosition = new Vector3(0, 0, 0);
            m_guild_pvp_gui.transform.localScale = new Vector3(1, 1, 1);
        }
        m_guild_pvp_gui.SetActive(true);
        m_guild_pvp_gui.GetComponent<guild_pvp_gui>().reset();


    }
    void show_guild_fight_gui()
    {
        if (m_guild_fight_gui == null)
        {
            m_guild_fight_gui = game_data._instance.ins_object_res("ui/juntuan/guild_fight_gui");
            m_guild_fight_gui.transform.parent = m_ui_cam.transform;
            m_guild_fight_gui.transform.localPosition = new Vector3(0, 0, 0);
            m_guild_fight_gui.transform.localScale = new Vector3(1, 1, 1);
        }
        m_guild_fight_gui.SetActive(true);
        m_guild_fight_gui.GetComponent<guild_fight_gui>().reset();
   
    }
    
	void show_transport_gui()
	{
		if(m_transport_gui == null)
		{
			m_transport_gui = game_data._instance.ins_object_res("ui/transport_gui");
			m_transport_gui.transform.parent = m_ui_cam.transform;
			m_transport_gui.transform.localPosition = new Vector3(0,0,0);
			m_transport_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_transport_gui.SetActive(true);
	}
	
	void hide_transport_gui(bool is_del)
	{
		if(m_transport_gui != null)
		{
			m_transport_gui.SetActive (false);
			if (is_del)
			{
				Object.Destroy (m_transport_gui);
			}
		}
	}
	
	void show_unit_show()
	{
		if(m_unit_show == null)
		{
			m_unit_show = game_data._instance.ins_object_res("ui/unit_show");
			m_unit_show.transform.parent = sys._instance.m_map_root.transform;
			m_unit_show.transform.localPosition = new Vector3(-100,0,0);
			m_unit_show.transform.localScale = new Vector3(1,1,1);
		}
		m_unit_show.SetActive (true);
		
		show_mask ();
	}
	
	void hide_unit_show()
	{
		if(m_unit_show != null)
		{
			GameObject.Destroy(m_unit_show);
			show_mask ();
		}
	}
	
	public void show_bao_wu_gui()
	{
		if (m_bw_gui == null)
		{
			m_bw_gui = game_data._instance.ins_object_res("ui/baowu_hecheng_gui_ex");
			m_bw_gui.transform.parent = m_ui_cam.transform;
			m_bw_gui.transform.localPosition = new Vector3(0,0,0);
			m_bw_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_bw_gui.SetActive (true);
	}
	
	void hide_bao_wu_gui(bool is_del)
	{
		if(m_bw_gui != null)
		{
			m_bw_gui.SetActive (false);
			if (is_del)
			{
				Object.Destroy (m_bw_gui);
			}
		}
	}
	
	void hide_pvp_gui(bool is_del)
	{
		if(m_pvp_gui != null)
		{
			m_pvp_gui.SetActive (false);
			if (is_del)
			{
				Object.Destroy (m_pvp_gui);
			}
		}
	}
	
	void show_ying_jiu_gui()
	{
		if (m_yj_gui == null)
		{
			m_yj_gui = game_data._instance.ins_object_res("ui/ying_jiu_gui");
			m_yj_gui.transform.parent = m_ui_cam.transform;
			m_yj_gui.transform.localPosition = new Vector3(0,0,0);
			m_yj_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_yj_gui.SetActive (true);
	}
	
	void hide_ying_jiu_gui(bool is_del)
	{
		if(m_yj_gui != null)
		{
			m_yj_gui.SetActive (false);
			if (is_del)
			{
				Object.Destroy (m_yj_gui);
			}
		}
	}
	
	public void show_ts_gui()
	{
		if (m_ts_gui == null)
		{
			m_ts_gui = game_data._instance.ins_object_res("ui/explore_gui");
			m_ts_gui.transform.parent = m_ui_cam.transform;
			m_ts_gui.transform.localPosition = new Vector3(0,0,0);
			m_ts_gui.transform.localScale = new Vector3(1,1,1);
		}
		else
		{
			m_ts_gui.GetComponent<explore_gui>().reset(true);
		}
		m_ts_gui.SetActive (true);
	}
	
	public void hide_ts_gui(bool is_del)
	{
		if(m_ts_gui != null)
		{
			m_ts_gui.SetActive (false);
			if (is_del)
			{
				Object.Destroy (m_ts_gui);
			}
		}
	}
	
	public void show_vr()
	{
		if(m_vr_gui == null)
		{
			m_vr_gui = game_data._instance.ins_object_res("ui/vr_gui");
			m_vr_gui.transform.parent = m_ui_cam.transform;
			m_vr_gui.transform.localPosition = new Vector3(0,0,0);
			m_vr_gui.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void hide_vr()
	{
		if(m_vr_gui != null)
		{
			Object.Destroy (m_vr_gui);
		}
	}
	
	public void show_mi_jing_gui()
	{
		if (m_mj_gui == null)
		{
			m_mj_gui = game_data._instance.ins_object_res("ui/mi_jing_gui");
			m_mj_gui.transform.parent = m_ui_cam.transform;
			m_mj_gui.transform.localPosition = new Vector3(0,0,0);
			m_mj_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_mj_gui.SetActive (true);
	}
	
	public void show_tanbao_gui()
	{
		if (m_tanbao_gui == null)
		{
			m_tanbao_gui = game_data._instance.ins_object_res("ui/tanbao_gui");
			m_tanbao_gui.transform.parent = m_ui_cam.transform;
			m_tanbao_gui.transform.localPosition = new Vector3(0,0,0);
			m_tanbao_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_tanbao_gui.SetActive (true);
	}
	
	public void hide_tanbao_gui(bool is_del)
	{
		if(m_tanbao_gui != null)
		{
			m_tanbao_gui.SetActive (false);
			if (is_del)
			{
				Object.Destroy (m_tanbao_gui);
			}
		}
	}
	
	public void hide_mi_jing_gui(bool is_del)
	{
		if(m_mj_gui != null)
		{
			m_mj_gui.SetActive (false);
			if (is_del)
			{
				Object.Destroy (m_mj_gui);
			}
		}
	}

	public void show_min_dialog_box()
	{
		if (m_min_dialog_box == null)
		{
			m_min_dialog_box = game_data._instance.ins_object_res("ui/min_dialog_box");
			m_min_dialog_box.AddComponent<gui_remove>();
			m_min_dialog_box.transform.parent = m_ui_bottomleft.transform;
			m_min_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_min_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_item_dialog_box()
	{
		if (m_item_dialog_box == null)
		{
			m_item_dialog_box = game_data._instance.ins_object_res("ui/item_dialog_box");
			m_item_dialog_box.AddComponent<gui_remove>();
			m_item_dialog_box.transform.parent = m_ui_cam.transform;
			m_item_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_item_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}
	public void show_chenghao_dialog_box()
	{
		if (m_chneghao_dialog_box == null)
		{
			m_chneghao_dialog_box = game_data._instance.ins_object_res("ui/chenhao_dialog_box");
			m_chneghao_dialog_box.AddComponent<gui_remove>();
			m_chneghao_dialog_box.transform.parent = m_ui_cam.transform;
			m_chneghao_dialog_box.transform.localPosition = new Vector3(0, 0, 0);
			m_chneghao_dialog_box.transform.localScale = new Vector3(1, 1, 1);
		}
		
	}
	
	
	public void show_equip_dialog_box()
	{
		if (m_equip_dialog_box == null)
		{
			m_equip_dialog_box = game_data._instance.ins_object_res("ui/equip_dialog_box");
			m_equip_dialog_box.AddComponent<gui_remove>();
			m_equip_dialog_box.transform.parent = m_ui_cam.transform;
			m_equip_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_equip_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_guanghuan_dialog_box()
	{
		if (m_guanghuan_dialog_box == null)
		{
			m_guanghuan_dialog_box = game_data._instance.ins_object_res("ui/guanghuan_dialog_box");
			m_guanghuan_dialog_box.AddComponent<gui_remove>();
			m_guanghuan_dialog_box.transform.parent = m_ui_cam.transform;
			m_guanghuan_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_guanghuan_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_card_dialog_box()
	{
		if (m_card_dialog_box == null)
		{
			m_card_dialog_box = game_data._instance.ins_object_res("ui/card_dialog_box");
			m_card_dialog_box.AddComponent<gui_remove>();
			m_card_dialog_box.transform.parent = m_ui_cam.transform;
			m_card_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_card_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void show_pet_dialog_box()
	{
		if (m_pet_dialog_box == null)
		{
			m_pet_dialog_box = game_data._instance.ins_object_res("ui/pet_info_ex");
			m_pet_dialog_box.transform.parent = m_ui_cam.transform;
			m_pet_dialog_box.AddComponent<gui_remove>();
			m_pet_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_pet_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
		cmessage_center._instance.add_handle (m_pet_dialog_box.GetComponent<pet_info>());
	}

	public void show_huiyi_dialog_box()
	{
		if (m_huiyi_dialog_box == null)
		{
			m_huiyi_dialog_box = game_data._instance.ins_object_res("ui/huiyi_dialog_box");
			m_huiyi_dialog_box.AddComponent<gui_remove>();
			m_huiyi_dialog_box.transform.parent = m_ui_cam.transform;
			m_huiyi_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_huiyi_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_treasure_dialog_box()
	{
		if (m_treasure_dialog_box == null)
		{
			m_treasure_dialog_box = game_data._instance.ins_object_res("ui/treasure_dialog_box");
			m_treasure_dialog_box.AddComponent<gui_remove>();
			m_treasure_dialog_box.transform.parent = m_ui_cam.transform;
			m_treasure_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_treasure_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_loading_gui()
	{
		if (m_loading == null)
		{
			m_loading = game_data._instance.ins_object_res("ui/loading");
			m_loading.transform.parent = m_ui_cam.transform;
			m_loading.transform.localPosition = new Vector3(0,0,0);
			m_loading.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void hide_loading_gui()
	{
		Object.Destroy (m_loading);
	}

	public void show_loading_gui_ex()
	{
		if (m_loading_ex == null)
		{
			m_loading_ex = game_data._instance.ins_object_res("ui/loading_ex");
			m_loading_ex.transform.parent = m_ui_cam.transform;
			m_loading_ex.transform.localPosition = new Vector3(0,0,0);
			m_loading_ex.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void hide_loading_gui_ex()
	{
		Object.Destroy (m_loading_ex);
	}
	
	public void show_single_dialog_box()
	{
		if (m_single_dialog_box == null)
		{
			m_single_dialog_box = game_data._instance.ins_object_res("ui/single_dialog_box");
			m_single_dialog_box.AddComponent<gui_remove>();
			m_single_dialog_box.transform.parent = m_ui_cam.transform;
			m_single_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_single_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void show_single_dialog_box(string name,string des,s_message message)
	{
		s_message _msg = new s_message();
		
		_msg.m_type = "single_dialog_box";
		_msg.m_string.Add (name);
		_msg.m_string.Add (des);
		
		_msg.m_object.Add(message);
		
		cmessage_center._instance.add_message(_msg);
	}
	
	public void show_common_card_page_gui()
	{
		if (m_common_card_page_gui == null)
		{
			m_common_card_page_gui = game_data._instance.ins_object_res("ui/common_card_page_gui");
			m_common_card_page_gui.transform.parent = m_ui_cam.transform;
			m_common_card_page_gui.AddComponent<gui_remove>();
			m_common_card_page_gui.transform.localPosition = new Vector3(0,0,0);
			m_common_card_page_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_common_card_page_gui.GetComponent<common_card_page_gui>().init ();
	}

	public void show_common_equip_page_gui()
	{
		if (m_common_equip_page_gui == null)
		{
			m_common_equip_page_gui = game_data._instance.ins_object_res("ui/common_equip_page_gui");
			m_common_equip_page_gui.transform.parent = m_ui_cam.transform;
			m_common_equip_page_gui.AddComponent<gui_remove>();
			m_common_equip_page_gui.transform.localPosition = new Vector3(0,0,0);
			m_common_equip_page_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_common_equip_page_gui.GetComponent<common_equip_page_gui>().init ();
	}
	
	public void show_common_treasure_page_gui()
	{
		if (m_common_treasure_page_gui == null)
		{
			m_common_treasure_page_gui = game_data._instance.ins_object_res("ui/common_treasure_page_gui");
			m_common_treasure_page_gui.transform.parent = m_ui_cam.transform;
			m_common_treasure_page_gui.AddComponent<gui_remove>();
			m_common_treasure_page_gui.transform.localPosition = new Vector3(0,0,0);
			m_common_treasure_page_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_common_treasure_page_gui.GetComponent<common_treasure_page_gui>().init ();
	}
	
	public void show_card_info()
	{
		if (m_card_info == null)
		{
			m_card_info = game_data._instance.ins_object_res("ui/card_info_ex");
			m_card_info.transform.parent = m_ui_cam.transform;
			m_card_info.AddComponent<gui_remove>();
			m_card_info.transform.localPosition = new Vector3(0,0,0);
			m_card_info.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_equip_detail()
	{
		if (m_equip_detail == null)
		{
			m_equip_detail = game_data._instance.ins_object_res("ui/equip_detail");
			m_equip_detail.transform.parent = m_ui_cam.transform;
			m_equip_detail.AddComponent<gui_remove>();
			m_equip_detail.transform.localPosition = new Vector3(0,0,0);
			m_equip_detail.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void show_pet_detail()
	{
		if (m_pet_detail == null)
		{
			m_pet_detail = game_data._instance.ins_object_res("ui/pet_detail");
			m_pet_detail.transform.parent = m_ui_cam.transform;
			m_pet_detail.AddComponent<gui_remove>();
			m_pet_detail.transform.localPosition = new Vector3(0,0,0);
			m_pet_detail.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_treasure_detail()
	{
		if (m_treasure_detail == null)
		{
			m_treasure_detail = game_data._instance.ins_object_res("ui/treasure_detail");
			m_treasure_detail.transform.parent = m_ui_cam.transform;
			m_treasure_detail.AddComponent<gui_remove>();
			m_treasure_detail.transform.localPosition = new Vector3(0,0,0);
			m_treasure_detail.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_common_cl_panel()
	{
		if (m_common_cl_panel == null)
		{
			m_common_cl_panel = game_data._instance.ins_object_res("ui/common_cl_panel");
			m_common_cl_panel.transform.parent = m_ui_cam.transform;
			m_common_cl_panel.AddComponent<gui_remove>();
			m_common_cl_panel.transform.localPosition = new Vector3(0,0,0);
			m_common_cl_panel.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_dress_detail()
	{
		if (m_dress_detail == null)
		{
			m_dress_detail = game_data._instance.ins_object_res("ui/dress_detail");
			m_dress_detail.transform.parent = m_ui_cam.transform;
			m_dress_detail.AddComponent<gui_remove>();
			m_dress_detail.transform.localPosition = new Vector3(0,0,0);
			m_dress_detail.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_level_gui()
	{
		if (m_level_gui == null)
		{
			m_level_gui = game_data._instance.ins_object_res("ui/levelup");
			m_level_gui.transform.parent = m_ui_cam.transform;
			m_level_gui.AddComponent<gui_remove>();
			m_level_gui.transform.localPosition = new Vector3(0,0,0);
			m_level_gui.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_target_finish_gui()
	{
		if (m_target_finish_gui == null)
		{
			m_target_finish_gui = game_data._instance.ins_object_res("ui/target_finish");
			m_target_finish_gui.transform.parent = m_ui_cam.transform;
			m_target_finish_gui.AddComponent<gui_remove>();
			m_target_finish_gui.transform.localPosition = new Vector3(0,0,0);
			m_target_finish_gui.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_arena_max()
	{
		if (m_arena_max == null)
		{
			m_arena_max = game_data._instance.ins_object_res("ui/arena_max");
			m_arena_max.transform.parent = m_ui_cam.transform;
			m_arena_max.AddComponent<gui_remove>();
			m_arena_max.transform.localPosition = new Vector3(0,0,0);
			m_arena_max.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_zhaomu_shuxing()
	{
		if (m_zhaomu_shuxing == null)
		{
			m_zhaomu_shuxing = game_data._instance.ins_object_res("ui/zhaomu_shuxing");
			m_zhaomu_shuxing.transform.parent = m_ui_cam.transform;
			m_zhaomu_shuxing.AddComponent<gui_remove>();
			m_zhaomu_shuxing.transform.localPosition = new Vector3(0,0,0);
			m_zhaomu_shuxing.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_tili_dialog_box()
	{
		if (m_tili_dialog_box == null)
		{
			m_tili_dialog_box = game_data._instance.ins_object_res("ui/tili_dialog_box");
			m_tili_dialog_box.transform.parent = m_ui_cam.transform;
			m_tili_dialog_box.AddComponent<gui_remove>();
			m_tili_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_tili_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_cishu_dialog_bos()
	{
		if (m_cishu_dialog_box == null)
		{
			m_cishu_dialog_box = game_data._instance.ins_object_res("ui/cishu_dialog_box");
			m_cishu_dialog_box.transform.parent = m_ui_cam.transform;
			m_cishu_dialog_box.AddComponent<gui_remove>();
			m_cishu_dialog_box.transform.localPosition = new Vector3(0, 0, 0);
			m_cishu_dialog_box.transform.localScale = new Vector3(1, 1, 1);
		}
	}
	
	public void show_recharge_dialog_box()
	{
		if (m_recharge_dialog_box == null)
		{
			m_recharge_dialog_box = game_data._instance.ins_object_res("ui/recharge_dialog_box");
			m_recharge_dialog_box.transform.parent = m_ui_cam.transform;
			m_recharge_dialog_box.AddComponent<gui_remove>();
			m_recharge_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_recharge_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_buy_num_gui()
	{
		if (m_buy_num_gui == null)
		{
			m_buy_num_gui = game_data._instance.ins_object_res("ui/buy_num_gui");
			m_buy_num_gui.transform.parent = m_ui_cam.transform;
			m_buy_num_gui.AddComponent<gui_remove>();
			m_buy_num_gui.transform.localPosition = new Vector3(0,0,0);
			m_buy_num_gui.transform.localScale = new Vector3(1,1,1);
		}
	}
    public void show_select_num_gui(int type)
    {
        if (m_select_num_gui == null)
        {
            m_select_num_gui = game_data._instance.ins_object_res("ui/select_cishu_box");
            m_select_num_gui.transform.parent = m_ui_cam.transform;
            m_select_num_gui.AddComponent<gui_remove>();
            m_select_num_gui.transform.localPosition = new Vector3(0, 0, 0);
            m_select_num_gui.transform.localScale = new Vector3(1, 1, 1);
            m_select_num_gui.GetComponent<select_cishu_box>().init(type);
            m_select_num_gui.SetActive(true);
        }
 
    }
	
	public void show_duixing_gui()
	{
		if (m_duixing_gui == null)
		{
			m_duixing_gui = game_data._instance.ins_object_res("ui/duixing_gui_ex");
			m_duixing_gui.transform.parent = m_ui_cam.transform;
			m_duixing_gui.AddComponent<gui_remove>();
			m_duixing_gui.transform.localPosition = new Vector3(0,0,0);
			m_duixing_gui.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_hb_dress_gui()
	{
		if (m_hb_dress_gui == null)
		{
			m_hb_dress_gui = game_data._instance.ins_object_res("ui/hb_dress_gui_ex");
			m_hb_dress_gui.transform.parent = m_ui_cam.transform;
			m_hb_dress_gui.AddComponent<gui_remove>();
			m_hb_dress_gui.transform.localPosition = new Vector3(0,0,0);
			m_hb_dress_gui.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	public void show_dress_gui()
	{
		if (m_dress_gui == null)
		{
			m_dress_gui = game_data._instance.ins_object_res("ui/dress_gui");
			m_dress_gui.transform.parent = m_ui_cam.transform;
			m_dress_gui.AddComponent<gui_remove>();
			m_dress_gui.transform.localPosition = new Vector3(0,0,0);
			m_dress_gui.transform.localScale = new Vector3(1,1,1);
		}
	}
	
	void init_comm_ui()
	{
		if (m_ui_mask == null)
		{
			m_ui_mask = game_data._instance.ins_object_res("ui/ui_mask");
			m_ui_mask.transform.parent = m_ui_cam.transform;
			m_ui_mask.transform.localPosition = new Vector3(0,0,0);
			m_ui_mask.transform.localScale = new Vector3(1,1,1);
		}

		if (m_mask == null)
		{
			m_mask = game_data._instance.ins_object_res("ui/mask");
			m_mask.transform.parent = m_ui_cam.transform;
			m_mask.transform.localPosition = new Vector3(0,0,0);
			m_mask.transform.localScale = new Vector3(1,1,1);
		}

		if (m_wait == null)
		{
			m_wait = game_data._instance.ins_object_res("ui/wait");
			m_wait.transform.parent = m_ui_cam.transform;
			m_wait.transform.localPosition = new Vector3(0,0,0);
			m_wait.transform.localScale = new Vector3(1,1,1);
		}
		
		if (m_danmu_panel == null)
		{
			m_danmu_panel = game_data._instance.ins_object_res("ui/danmu_panel");
			m_danmu_panel.transform.parent = m_ui_cam.transform;
			m_danmu_panel.transform.localPosition = new Vector3(0,0,0);
			m_danmu_panel.transform.localScale = new Vector3(1,1,1);
		}

		if (m_talk == null)
		{
			m_talk = game_data._instance.ins_object_res("ui/talk");
			m_talk.transform.parent = m_ui_bottomleft.transform;
			m_talk.transform.localPosition = new Vector3(0,0,0);
			m_talk.transform.localScale = new Vector3(1,1,1);
		}

		if (m_hall_gui == null)
		{
			m_hall_gui = game_data._instance.ins_object_res("ui/hall_gui");
			m_hall_gui.transform.parent = m_ui_cam.transform;
			m_hall_gui.transform.localPosition = new Vector3(0,0,0);
			m_hall_gui.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void action_guide(string action)
	{
        if(game_data._instance.m_guaji == 0)
        {
            if (m_guide_gui == null)
            {
                m_guide_gui = game_data._instance.ins_object_res("ui/guide");
                m_guide_gui.transform.parent = m_ui_cam.transform;
                m_guide_gui.transform.localPosition = new Vector3(0, 0, 0);
                m_guide_gui.transform.localScale = new Vector3(1, 1, 1);
                m_guide_gui.AddComponent<gui_remove>();
            }
            m_guide_gui.GetComponent<guide_gui>().got_main_gui();
            m_guide_gui.GetComponent<guide_gui>().action_guide(action);
        }
		
	}

	public void show_select_dialog_box()
	{
		if (m_select_dialog_box == null)
		{
			m_select_dialog_box = game_data._instance.ins_object_res("ui/select_dialog_box");
			m_select_dialog_box.AddComponent<gui_remove>();
			m_select_dialog_box.transform.parent = m_ui_cam.transform;
			m_select_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_select_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
	}

	public void show_select_dialog_box(string name,string des,s_message ok_msg)
	{
		s_message _msg = new s_message();
		
		_msg.m_type = "select_dialog_box";
		_msg.m_string.Add (name);
		_msg.m_string.Add (des);
		
		_msg.m_object.Add(ok_msg);
		
		cmessage_center._instance.add_message(_msg);
	}
	public void show_cishu_dialog_box(int num1, int num2,int buy_num,int type, s_message ok_mes)
	{
		s_message _msg = new s_message();
		_msg.m_type = "cishu_dialog_box";
		_msg.m_ints.Add(num1);
		_msg.m_ints.Add(num2);
		_msg.m_ints.Add(buy_num);
		_msg.m_ints.Add(type);
		_msg.m_object.Add(ok_mes);
		cmessage_center._instance.add_message(_msg);
		
	}
	
	public void show_tili_dialog_box(int id,int type = 0)
	{
		s_message _msg = new s_message();
		
		_msg.m_type = "tili_dialog_box";
		_msg.m_ints.Add (id);
		_msg.m_ints.Add (type);
		cmessage_center._instance.add_message(_msg);
	}
	
	public void show_select_dialog_box(string name,string des,s_message ok_msg,s_message cancel_msg,bool flag = false,bool m_login = false)
	{
		s_message _msg = new s_message();
		
		_msg.m_type = "select_dialog_box";
		_msg.m_string.Add (name);
		_msg.m_string.Add (des);
		_msg.m_bools.Add (flag);
		_msg.m_bools.Add(m_login);
		_msg.m_object.Add(ok_msg);
		_msg.m_object.Add (cancel_msg);
		
		cmessage_center._instance.add_message(_msg);
	}
	
	
	public void show_recharge_dialog_box(recharge_dialog_box.rdb_method method)
	{
		show_recharge_dialog_box ();
		m_recharge_dialog_box.GetComponent<recharge_dialog_box>().set_func(method);
		m_recharge_dialog_box.SetActive (true);
	}
	
	public void show_other_dialog_box(other_dialog_box.rdb_method method,string name,string des,s_message ok_msg,s_message mess)
	{
		s_message _msg = new s_message();
		
		_msg.m_type = "other_dialog_box";
		_msg.m_string.Add (name);
		_msg.m_string.Add (des);
		
		_msg.m_object.Add(ok_msg);
		_msg.m_object.Add(mess);
		m_other_dialog_box.GetComponent<other_dialog_box>().set_func(method);
		
		cmessage_center._instance.add_message(_msg);
	}
	
	public void show_jd_dialog_box()
	{
		if (m_jd_dialog_box == null)
		{
			m_jd_dialog_box = game_data._instance.ins_object_res("ui/jd_dialog_box");
			m_jd_dialog_box.transform.parent = m_ui_cam.transform;
			m_jd_dialog_box.AddComponent<gui_remove>();
			m_jd_dialog_box.transform.localPosition = new Vector3(0,0,0);
			m_jd_dialog_box.transform.localScale = new Vector3(1,1,1);
		}
		m_jd_dialog_box.SetActive (true);
	}
	
	public void show_mianyi_dialog_box(select_mianyi_box.rdb_method method,string des,s_message ok_msg)
	{
		if (m_selcet_mianyi_box == null)
		{
			m_selcet_mianyi_box = game_data._instance.ins_object_res("ui/select_mianyi_box");
			m_selcet_mianyi_box.transform.parent = m_ui_cam.transform;
			m_selcet_mianyi_box.AddComponent<gui_remove>();
			m_selcet_mianyi_box.transform.localPosition = new Vector3(0,0,0);
			m_selcet_mianyi_box.transform.localScale = new Vector3(1,1,1);
		}
		m_selcet_mianyi_box.GetComponent<select_mianyi_box>().set_des(method,des,ok_msg);
		m_selcet_mianyi_box.SetActive (true);
	}
	
	public void show_prompt_dialog_box(string des)
	{
		show_prompt_dialog_box (des, 0, "", "");
	}
	public void show_prompt_dialog_box(string des, int sset, string icon, string des1)
	{
		s_message _msg = new s_message();
		
		_msg.m_type = "show_prompt_dialog_box";
		_msg.m_string.Add (des);
		_msg.m_ints.Add (sset);
		_msg.m_string.Add (icon);
		_msg.m_string.Add (des1);
		
		cmessage_center._instance.add_message(_msg);
	}
	public void show_common_card_panel (string des, bool show_remove, int min_color, List<ulong> hide_guids, string out_message, bool see_ok,GameObject next)
	{
		show_common_card_page_gui ();
		m_common_card_page_gui.GetComponent<common_card_page_gui>().is_zhanhun = false;
		m_common_card_page_gui.GetComponent<common_card_page_gui>().reset ();
		card_page_gui _card_page_gui = m_common_card_page_gui.GetComponent<common_card_page_gui>().m_card_page_gui.GetComponent<card_page_gui>();
		
		_card_page_gui.init ();
		_card_page_gui.m_show_remove = show_remove;
		_card_page_gui.m_min_color = min_color;
		_card_page_gui.set_text(des);
		_card_page_gui.m_hide_guids = hide_guids;
		_card_page_gui.m_out_message = out_message;
        int num = 0;
        for (int i = 0; i < sys._instance.m_self.m_t_player.zhenxing.Count; ++i)
        {
            if (sys._instance.m_self.m_t_player.zhenxing[i] != 0)
            {
                num++;
            }
        }
        if (num > 3)
        {
            m_common_card_page_gui.GetComponent<common_card_page_gui>().m_remove.SetActive(show_remove);
        }
        else
        {
            m_common_card_page_gui.GetComponent<common_card_page_gui>().m_remove.SetActive(false);

        }
		_card_page_gui.card_reset ();
		m_common_card_page_gui.GetComponent<common_card_page_gui>().m_ok.SetActive(see_ok);
		m_common_card_page_gui.GetComponent<common_card_page_gui>().m_next = next;
		m_common_card_page_gui.SetActive(true);
	}
	public void show_common_jiyin_panel (string des, bool show_remove, int min_color, List<int> hide_sps, string out_message, bool see_ok,GameObject next)
	{
		show_common_card_page_gui ();
		card_page_gui _card_page_gui = m_common_card_page_gui.GetComponent<common_card_page_gui>().m_card_page_gui.GetComponent<card_page_gui>();
		
		_card_page_gui.init ();
		_card_page_gui.m_show_remove = show_remove;
		_card_page_gui.m_min_color = min_color;
		_card_page_gui.set_text(des);
		_card_page_gui.m_hide_sps = hide_sps;
		_card_page_gui.m_out_message = out_message;
		_card_page_gui.sp_reset ();
		
		m_common_card_page_gui.GetComponent<common_card_page_gui>().m_remove.SetActive(show_remove);
		m_common_card_page_gui.GetComponent<common_card_page_gui>().m_ok.SetActive(see_ok);
		m_common_card_page_gui.GetComponent<common_card_page_gui>().m_next = next;
		m_common_card_page_gui.SetActive(true);
	}
	public void show_common_equip_panel(string des, bool show_remove, bool show_lock, int type, List<ulong> hide_guids, string out_message, bool see_ok,int show,GameObject next)
	{
		show_common_equip_page_gui ();
		equip_page_gui _equip_page_gui = m_common_equip_page_gui.GetComponent<common_equip_page_gui>().m_equip_page_gui.GetComponent<equip_page_gui>();
		
		_equip_page_gui.init ();
		_equip_page_gui.m_show_remove = show_remove;
		_equip_page_gui.m_show_lock = show_lock;
		_equip_page_gui.m_show_type = type;
		_equip_page_gui.set_text(des);
		_equip_page_gui.m_hide_guids = hide_guids;
		_equip_page_gui.m_out_message = out_message;
		_equip_page_gui.m_show = show;
		_equip_page_gui.equip_reset ();
		
		m_common_equip_page_gui.GetComponent<common_equip_page_gui>().m_ok.SetActive(see_ok);
		m_common_equip_page_gui.GetComponent<common_equip_page_gui>().m_next = next;
		m_common_equip_page_gui.SetActive(true);
	}
	
	public void show_common_treasure_panel(string des, bool show_remove, bool show_lock, int type, List<ulong> hide_guids, string out_message, bool see_ok,int show,GameObject next,int count = 0)
	{
		show_common_treasure_page_gui ();
		treasure_page_gui _treasure_page_gui = m_common_treasure_page_gui.GetComponent<common_treasure_page_gui>().m_treasure_page_gui.GetComponent<treasure_page_gui>();
		
		_treasure_page_gui.init ();
		_treasure_page_gui.m_show_remove = show_remove;
		_treasure_page_gui.m_show_lock = show_lock;
		_treasure_page_gui.m_show_type = type;
		_treasure_page_gui.set_text(des);
		_treasure_page_gui.m_hide_guids = hide_guids;
		_treasure_page_gui.m_out_message = out_message;
		_treasure_page_gui.m_show = show;
		_treasure_page_gui.m_count = count;
		_treasure_page_gui.treasure_reset ();
		
		m_common_treasure_page_gui.GetComponent<common_treasure_page_gui>().m_ok.SetActive(see_ok);
		m_common_treasure_page_gui.GetComponent<common_treasure_page_gui>().m_next = next;
		m_common_treasure_page_gui.SetActive(true);
	}

	public void show_equip_detail(dhc.equip_t _equip, int type,GameObject next,bool flag)
	{
		show_equip_detail ();
		m_equip_detail.GetComponent<equip_detail>().m_flag = flag;
		m_equip_detail.GetComponent<equip_detail>().reset (_equip, type);
		m_equip_detail.SetActive (true);
	}

	public void add_equip(List<dhc.equip_t> equips,bool flag)
	{
		show_equip_detail ();
		m_equip_detail.GetComponent<equip_detail>().m_t_equips.Clear();
		for(int i =0 ;i < equips.Count;++i)
		{
			m_equip_detail.GetComponent<equip_detail>().m_t_equips.Add(equips[i]);
			m_equip_detail.GetComponent<equip_detail>().m_flag = flag;
		}
	}

	public void add_treasure(List<dhc.treasure_t> treasures,bool flag)
	{
		show_treasure_detail ();
		m_treasure_detail.GetComponent<treasure_detail>().m_t_treasures.Clear();
		for(int i =0 ;i < treasures.Count;++i)
		{
			m_treasure_detail.GetComponent<treasure_detail>().m_t_treasures.Add(treasures[i]);
			m_treasure_detail.GetComponent<treasure_detail>().m_flag = flag;
		}
	}

	public void show_treasure_detail(dhc.treasure_t _treasure, int type,GameObject next,bool flag)
	{
		show_treasure_detail ();
		m_treasure_detail.GetComponent<treasure_detail>().m_next = next;
		m_treasure_detail.GetComponent<treasure_detail>().m_flag = flag;
		m_treasure_detail.GetComponent<treasure_detail>().reset (_treasure, type);
		m_treasure_detail.SetActive (true);
	}
	public void show_dress_detail(s_t_dress _dress, int type)
	{
		show_dress_detail ();
		m_dress_detail.GetComponent<dress_detail>().reset (_dress, type);
		m_dress_detail.SetActive (true);
	}
    public void show_player_info_gui(protocol.game.smsg_player_look msg)
    {
        if (m_player_info_gui == null)
        {
            m_player_info_gui = game_data._instance.ins_object_res("ui/player_info_gui");
            m_player_info_gui.AddComponent<gui_remove>();
            m_player_info_gui.transform.parent = m_ui_cam.transform;
            m_player_info_gui.transform.localPosition = new Vector3(0, 0, 0);
            m_player_info_gui.transform.localScale = new Vector3(1, 1, 1);
        }
        m_player_info_gui.GetComponent<player_info_gui>().reset(msg);
        m_player_info_gui.SetActive(true);
    }
	public void show_common_player(protocol.game.smsg_player_look msg)
	{
		if (m_common_player_panel == null)
		{
			m_common_player_panel = game_data._instance.ins_object_res("ui/common_player_panel_ex");
			m_common_player_panel.AddComponent<gui_remove>();
			m_common_player_panel.transform.parent = m_ui_cam.transform;
			m_common_player_panel.transform.localPosition = new Vector3(0,0,0);
			m_common_player_panel.transform.localScale = new Vector3(1,1,1);
		}
		m_common_player_panel.GetComponent<common_player_panel>().m_msg = msg;
		m_common_player_panel.SetActive (true);
	}
	public void show_arena_max(int rank1, int rank2)
	{
		show_arena_max ();
		m_arena_max.GetComponent<arena_max>().m_rank1 = rank1;
		m_arena_max.GetComponent<arena_max>().m_rank2 = rank2;
		m_arena_max.SetActive (true);
	}
	
	public void show_arena_end(List<int> _rewards,int index,int gold,string scence)
	{
		if (m_arena_end == null)
		{
			m_arena_end = game_data._instance.ins_object_res("ui/arena_end");
			m_arena_end.transform.parent = m_ui_cam.transform;
			m_arena_end.transform.localPosition = new Vector3(0,0,0);
			m_arena_end.transform.localScale = new Vector3(1,1,1);
			arena_end_gui _arena =  m_arena_end.GetComponent<arena_end_gui>();
			_arena.m_scence = scence;
			_arena.gold = gold;
			_arena.m_rewards.Clear();
			for(int i = 0;i <_rewards.Count;i++ )
			{
				_arena.m_rewards.Add(_rewards[i]);
			}
			_arena.m_index = index;
			m_arena_end.SetActive(true);
		}
	}
    public void show_master_levelup(int leftid, int rightid,s_message m_msg)
    {
        if (m_master_levelup == null)
        {
            m_master_levelup = game_data._instance.ins_object_res("ui/master_duanweiup_gui");
            m_master_levelup.transform.parent = m_ui_cam.transform;
            m_master_levelup.transform.localPosition = new Vector3(0, 0, 0);
            m_master_levelup.transform.localScale = new Vector3(1, 1, 1);
            
        }
        master_duanwei_levelup levelup = m_master_levelup.GetComponent<master_duanwei_levelup>();
        levelup.m_msg = m_msg;
        levelup.reset(leftid, rightid);
        m_master_levelup.SetActive(true);

    }

	public void show_pet_detail(dhc.pet_t _pet, int type)
	{
		show_pet_detail ();
		m_pet_detail.GetComponent<pet_detail>().reset (_pet, type);
		m_pet_detail.SetActive (true);
	}

	public void show_select_item_gui(int id, List<s_t_reward> rewards, string mes,int type = 0,int num = 0)
	{
		if (m_select_item_gui == null)
		{
			m_select_item_gui = game_data._instance.ins_object_res("ui/select_item_gui");
			m_select_item_gui.transform.parent = m_ui_cam.transform;
			m_select_item_gui.AddComponent<gui_remove>();
			m_select_item_gui.transform.localPosition = new Vector3(0, 0, 0);
			m_select_item_gui.transform.localScale = new Vector3(1, 1, 1);
		}
		select_item_gui _item_gui = m_select_item_gui.GetComponent<select_item_gui>();
		_item_gui.m_id = id;
		_item_gui.type = type;
		_item_gui.m_total_num = num;
		_item_gui.rewards = rewards;
		_item_gui.m_mes = new s_message();
		_item_gui.m_mes.m_type = mes;
		_item_gui.reset();
		_item_gui.gameObject.SetActive(true);
	}

	public void show_other_reward(string name, string desc, List<s_t_reward> rewards, bool flag, s_message msg)
	{
		if (m_other_reward == null)
		{
			m_other_reward = game_data._instance.ins_object_res("ui/other_reward_gui");
			m_other_reward.transform.parent = m_ui_cam.transform;
			m_other_reward.AddComponent<gui_remove>();
			m_other_reward.transform.localPosition = new Vector3(0,0,0);
			m_other_reward.transform.localScale = new Vector3(1,1,1);
		}
		
		m_other_reward.GetComponent<other_reward_gui>().m_out_message = msg;
		m_other_reward.GetComponent<other_reward_gui>().m_flag = flag;
		m_other_reward.GetComponent<other_reward_gui>().m_name = name;
		m_other_reward.GetComponent<other_reward_gui>().m_desc = desc;
		m_other_reward.GetComponent<other_reward_gui>().m_rewards = rewards;
		m_other_reward.GetComponent<other_reward_gui>().update_ui ();
		m_other_reward.SetActive(true);
	}

	public void show_common_pet_panel (string des, bool show_remove, int min_color, List<ulong> hide_guids, string out_message, bool see_ok,GameObject next,int m_show)
	{
		show_common_pet_page_gui ();
		pet_page_gui _pet_page_gui = m_common_pet_page_gui.GetComponent<common_pet_page_gui>().m_pet_page_gui.GetComponent<pet_page_gui>();
		
		_pet_page_gui.init ();
		_pet_page_gui.m_show_remove = show_remove;
		_pet_page_gui.m_show = m_show;
		_pet_page_gui.m_min_color = min_color;
		_pet_page_gui.set_text(des);
		_pet_page_gui.m_hide_guids = hide_guids;
		_pet_page_gui.m_out_message = out_message;
		_pet_page_gui.pet_reset ();
		
		m_common_pet_page_gui.GetComponent<common_pet_page_gui>().m_remove.SetActive(show_remove);
		m_common_pet_page_gui.GetComponent<common_pet_page_gui>().m_ok.SetActive(see_ok);
		m_common_pet_page_gui.GetComponent<common_pet_page_gui>().m_next = next;
		m_common_pet_page_gui.SetActive(true);
	}

	public void show_common_pet_page_gui()
	{
		if (m_common_pet_page_gui == null)
		{
			m_common_pet_page_gui = game_data._instance.ins_object_res("ui/common_pet_page_gui");
			m_common_pet_page_gui.transform.parent = m_ui_cam.transform;
			m_common_pet_page_gui.AddComponent<gui_remove>();
			m_common_pet_page_gui.transform.localPosition = new Vector3(0,0,0);
			m_common_pet_page_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_common_pet_page_gui.GetComponent<common_pet_page_gui>().init ();
	}

	public void show_common_item_panel (string des, bool show_remove,  List<int> show_ids, List<int> hide_ids, string out_message, bool see_ok,GameObject next,int count = 0)
	{
		show_common_item_page_gui ();
		item_page_gui _item_page_gui = m_common_item_page_gui.GetComponent<common_item_page_gui>().m_item_page_gui.GetComponent<item_page_gui>();
		
		_item_page_gui.init ();
		_item_page_gui.m_show_remove = show_remove;
		_item_page_gui.m_ids = show_ids;
		_item_page_gui.set_text(des);
		_item_page_gui.m_hide_ids = hide_ids;
		_item_page_gui.m_out_message = out_message;
		_item_page_gui.m_count = count;
		_item_page_gui.item_reset ();
		
		m_common_item_page_gui.GetComponent<common_item_page_gui>().m_remove.SetActive(show_remove);
		m_common_item_page_gui.GetComponent<common_item_page_gui>().m_ok.SetActive(see_ok);
		m_common_item_page_gui.GetComponent<common_item_page_gui>().m_next = next;
		m_common_item_page_gui.SetActive(true);
	}
	
	public void show_common_item_page_gui()
	{
		if (m_common_item_page_gui == null)
		{
			m_common_item_page_gui = game_data._instance.ins_object_res("ui/common_item_page_gui");
			m_common_item_page_gui.transform.parent = m_ui_cam.transform;
			m_common_item_page_gui.AddComponent<gui_remove>();
			m_common_item_page_gui.transform.localPosition = new Vector3(0,0,0);
			m_common_item_page_gui.transform.localScale = new Vector3(1,1,1);
		}
		m_common_item_page_gui.GetComponent<common_item_page_gui>().init ();
	}

	public bool is_wait_active()
	{
		return m_wait_active;
	}

	public void wait(bool active)
	{
		m_wait_active = active;
		if (active)
		{
			m_wait.SetActive(true);
		}
		else
		{
			m_wait.SetActive (false);
		}
	}

	public void show_mask()
	{
		m_mask.GetComponent<UIPanel>().alpha = 1.0f;
		m_mask.SetActive (true);
	}
	public void do_mask(float time)
	{
		if (m_ui_mask == null)
		{
			return;
		}
		m_ui_mask.SetActive (true);
		if (m_ui_mask_time < time)
		{
			m_ui_mask_time = time;
		}
	}

	public void do_bf(int num)
	{
		m_bf_gui = game_data._instance.ins_object_res("ui/bf_gui");
		m_bf_gui.transform.parent = m_ui_cam.transform;
		m_bf_gui.transform.localPosition = Vector3.zero;
		m_bf_gui.AddComponent<gui_remove>();
		m_bf_gui.transform.GetComponent<bf_gui>().m_num = num;
		m_bf_gui.SetActive(true);
	}

	public Camera get_ui_cam()
	{
		return m_ui_cam.GetComponent<Camera>();
	}

	public Vector3 get_ui_pos(Transform t)
	{
		Vector3 v = Vector3.zero;
		while (this.transform != t) 
		{
			v = v + t.localPosition;
			t = t.parent;
		}
		return v;
	}

    public Vector3 get_ui_adapt_pos(Transform t, float size)
    {
        Vector3 v = Vector3.zero;
        while (this.transform != t)
        {
            v = v + new Vector3( t.localPosition.x * size, t.localPosition.y * size, t.localPosition.z * size);
            t = t.parent;
        }
        return v;
    }
	void IMessage.net_message(s_net_message message)
	{
        if (message.m_opcode == opclient_t.CMSG_PLAYER_LOOK || message.m_opcode == opclient_t.SMSG_VIEW_TEAM_MEMBER) {
			protocol.game.smsg_player_look _msg = net_http._instance.parse_packet<protocol.game.smsg_player_look> (message.m_byte);
			if (_msg.guid != sys._instance.m_self.m_t_player.guid && _msg.serverid == sys._instance.m_sid && sys._instance.m_game_state != "bingyuan" && sys._instance.m_game_state != "bingyuan_buttle") {
				show_player_info_gui (_msg);
			} else {
				root_gui._instance.show_common_player (_msg);
			}
		}
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "hide_dui_xing")
		{
			if(m_duixing_gui != null && m_duixing_gui.activeSelf)
			{
				m_duixing_gui.transform.Find("frame_big").GetComponent<frame>().hide();
			}
		}
		else if(message.m_type == "localorder_recharge")
		{
			protocol.game.cmsg_common_ex _msg = new protocol.game.cmsg_common_ex ();
			net_http._instance.send_msg_ex<protocol.game.cmsg_common_ex> (opclient_t.CMSG_RECHARGE_CHECK_EX, _msg);
		}
		else if(message.m_type == "show_jd_gui")
		{
			show_jd_gui();
			sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm09");
		}
       
        else if (message.m_type == "show_recharge")
        {
            if (message.m_ints.Count == 0)
            {
                show_recharge_gui(0);
            }
            else if (message.m_ints.Count == 1)
            {
                show_recharge_gui((int)message.m_ints[0]);
            }
            else if (message.m_ints.Count == 2)
            {
                show_recharge_gui((int)message.m_ints[0], (int)message.m_ints[1]);
            }

            sys._instance.play_sound_ex("sound/ts_rwpy/tspy_jm07");
        }
        else if (message.m_type == "hide_mijing_gui")
        {
            hide_mi_jing_gui(true);
        }
        else if (message.m_type == "show_unit_show")
        {
            show_unit_show();
        }
        else if (message.m_type == "hide_unit_show")
        {
            hide_unit_show();
        }
        else if (message.m_type == "show_bz_help_gui")
        {
            show_bz_help_gui();
        }
        else if (message.m_type == "hide_bz_help_gui")
        {
            hide_bz_help_gui();
        }
        else if (message.m_type == "loaded")
        {
            if (sys._instance.m_game_state == "hall")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(true);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                hide_login_gui();
                hide_boss_gui(true);
                hide_ts_gui(true);
                hide_ying_jiu_gui(true);
                hide_mi_jing_gui(true);
                hide_transport_gui(true);
                hide_bao_wu_gui(true);
                hide_pvp_gui(false);
                hide_vr();
                hide_tanbao_gui(true);
                sys._instance.m_map_root.SetActive(true);

                if (m_hall_guide.Length > 0)
                {
                    action_guide(m_hall_guide);
                    m_hall_guide = "";

                    if (sys._instance.m_map.activeSelf == false)
                    {
                        s_message _message = new s_message();
                        _message.time = 0.1f;
                        _message.m_type = "show_main_gui";
                        cmessage_center._instance.add_message(_message);
                    }
                }
            }
            else if (sys._instance.m_game_state == "login")
            {
                wait(false);

                show_login_gui();

                sys._instance.m_buttle_cam.SetActive(false);

                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                sys._instance.m_map_root.SetActive(true);
            }
            else if (sys._instance.m_game_state == "buttle")
            {
                show_battle_gui();
                m_hall_gui.SetActive(false);

                sys._instance.m_buttle_cam.SetActive(true);
                hide_login_gui();
                hide_boss_gui(false);
                hide_ts_gui(false);
                hide_ying_jiu_gui(false);
                hide_mi_jing_gui(false);
                hide_transport_gui(false);
                hide_bao_wu_gui(false);
                if(m_master_gui != null)
                {
                    m_master_gui.SetActive(false);
                }
                if(m_bingyuan_gui != null)
                {
                    m_bingyuan_gui.SetActive(false);
                }
                for (int i = 0; i < sys._instance.m_root_unit.transform.childCount; i++)
                {
                    Destroy(sys._instance.m_root_unit.transform.GetChild(i).gameObject);
                }
                    sys._instance.m_map_root.SetActive(false);
            }
            else if (sys._instance.m_game_state == "bingyuan_buttle")
            {
                hide_battle_gui();
                m_hall_gui.SetActive(false);

                sys._instance.m_buttle_cam.SetActive(true);
                hide_login_gui();
                hide_boss_gui(false);
                hide_ts_gui(false);
                hide_ying_jiu_gui(false);
                hide_mi_jing_gui(false);
                hide_transport_gui(false);
                hide_bao_wu_gui(false);
                s_message mes = new s_message();
                mes.m_type = "bingyuan_fight_end";
                cmessage_center._instance.add_message(mes);
                sys._instance.m_map_root.SetActive(false);
                if (bingyuan_gui._instance.m_win_result.activeSelf)
                {
                    bingyuan_gui._instance.end();
                    bingyuan_gui._instance.bingyuan_battle.m_battle_fight = false;
                }

                if (bingyuan_gui._instance.bingyuan_battle.gameObject.activeSelf)
                {
                    s_message _message = new s_message();
                    _message.m_type = "bingyuan_fight_start";
                    cmessage_center._instance.add_message(_message);
                    for (int i = 0; i < bingyuan_gui._instance.m_team.players.Count; i++)
                    {
                        if (!bingyuan_gui._instance.m_team.players[i].leader && !bingyuan_gui._instance.m_team.players[i].is_npc)
                        {
                            bingyuan_gui._instance.m_team.players[i].prepare = false;
                        }
                    }
                }

            }
            else if (sys._instance.m_game_state == "boss")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_boss_gui();
                if (m_shop == 1)
                {
                    s_message _message = new s_message();
                    _message.m_type = "show_mowang_shop";
                    cmessage_center._instance.add_message(_message);
                }
            }
            else if (sys._instance.m_game_state == "pvp")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_pvp_gui();
                if (m_shop == 1)
                {
                    s_message _message = new s_message();
                    _message.m_type = "show_pvp_shop_gui";
                    cmessage_center._instance.add_message(_message);
                }
               
            }
            else if (sys._instance.m_game_state == "master")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_master_gui();
                if (sys._instance.m_state_flag == 1)
                {
                    
                }
                for (int i = 0; i < sys._instance.m_root_unit.transform.childCount; i++)
                {
                    sys._instance.m_root_unit.transform.GetChild(i).gameObject.SetActive(true);
                }
 
            }
            else if (sys._instance.m_game_state == "bingyuan")
            {
                
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                s_message mes = new s_message();
                mes.m_type = "bingyuan_fight_end";
                cmessage_center._instance.add_message(mes);
                if (m_bingyuan_gui == null)
                {
                    show_bingyuan_gui();
                }
                else
                {
                    m_bingyuan_gui.SetActive(true);
                }
            }
            else if(sys._instance.m_game_state == "guild_pvp")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_guild_pvp_gui();
            }
            else if (sys._instance.m_game_state == "transport")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_transport_gui();
            }
            else if (sys._instance.m_game_state == "mi_jing")
            {
                
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(true);
                show_mi_jing_gui();
                if (m_mi_jing_shop == 1)
                {
                    s_message _message = new s_message();
                    _message.m_type = "show_equip_mj_shop";
                    cmessage_center._instance.add_message(_message);
                }
                else if (m_mi_jing_shop == 2)
                {
                    s_message _message = new s_message();
                    _message = new s_message();
                    _message.m_type = "show_equip_mj_reward_shop";
                    cmessage_center._instance.add_message(_message);
                }
            }
            else if (sys._instance.m_game_state == "tan_suo")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_ts_gui();
            }
            else if (sys._instance.m_game_state == "ying_jiu")
            {
               
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_ying_jiu_gui();
            }
            else if (sys._instance.m_game_state == "vr")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_vr();
            }
            else if (sys._instance.m_game_state == "bao_wu")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
                show_bao_wu_gui();
            }
            else if (sys._instance.m_game_state == "guild_boss_ex")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(true);
                show_guild_boss_gui();
            }
            else if (sys._instance.m_game_state == "guild_fight_pvp")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(true);
                show_guild_pvp_gui();
            }
            else if (sys._instance.m_game_state == "explore_ex")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(true);
                show_ts_gui();
            }
            else if (sys._instance.m_game_state == "memory")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(true);
                show_memory_gui();

            }
            else if (sys._instance.m_game_state == "director")
            {
                sys._instance.m_map_root.SetActive(false);
            }
            else if (sys._instance.m_game_state == "tanbao")
            {
                Time.timeScale = 1;
                m_hall_gui.SetActive(false);
                hide_battle_gui();
                sys._instance.m_buttle_cam.SetActive(false);
            }
            platform._instance.scene_loaded();
        }
        else if (message.m_type == "min_dialog_box")
        {
            show_min_dialog_box();
            m_min_dialog_box.GetComponent<min_dialog_box>().m_name = (string)message.m_string[0];
            m_min_dialog_box.GetComponent<min_dialog_box>().m_des = (string)message.m_string[1];
            if (message.m_string.Count >= 3)
            {
                m_min_dialog_box.GetComponent<min_dialog_box>().m_pz = (string)message.m_string[2];
            }
            else
            {
                m_min_dialog_box.GetComponent<min_dialog_box>().m_pz = "";
            }
            m_min_dialog_box.GetComponent<min_dialog_box>().m_ints = message.m_ints;

            Vector3 _pos = sys._instance.get_mouse_position();
            _pos.y += 10;

            m_min_dialog_box.transform.localPosition = _pos;
            m_min_dialog_box.transform.localScale = new Vector3(1, 1, 1);
            m_min_dialog_box.SetActive(true);
        }
        else if (message.m_type == "hide_min_dialog_box")
        {
            if (m_min_dialog_box != null)
            {
                m_min_dialog_box.SetActive(false);
            }
        }
        else if (message.m_type == "item_dialog_box")
        {
            show_item_dialog_box();
            m_item_dialog_box.GetComponent<item_dialog_box>().item_id = (int)message.m_ints[0];
            m_item_dialog_box.GetComponent<item_dialog_box>().m_type = (int)message.m_ints[1];
            if (message.m_string.Count > 0)
            {
                m_item_dialog_box.GetComponent<item_dialog_box>().desc = (string)message.m_string[0];
            }
            m_item_dialog_box.SetActive(true);
        }
        else if (message.m_type == "chenhao_dialog_box")
        {
            show_chenghao_dialog_box();
            m_chneghao_dialog_box.GetComponent<chenghao_dialog_box>().reset((int)message.m_ints[0]);
            m_chneghao_dialog_box.SetActive(true);

        }
        else if (message.m_type == "equip_dialog_box")
        {
            show_equip_dialog_box();
            m_equip_dialog_box.GetComponent<equip_dialog_box>().item_id = (int)message.m_ints[0];
            m_equip_dialog_box.GetComponent<equip_dialog_box>().type = (int)message.m_ints[1];
            if (message.m_object.Count > 0)
            {
                m_equip_dialog_box.GetComponent<equip_dialog_box>().m_equip = (dhc.equip_t)message.m_object[0];
            }
            m_equip_dialog_box.SetActive(true);
        }

        else if (message.m_type == "guanghuan_dialog_box")
        {
            show_guanghuan_dialog_box();
            m_guanghuan_dialog_box.GetComponent<guanghuan_dialog_box>().item_id = (int)message.m_ints[0];
            m_guanghuan_dialog_box.SetActive(true);
        }

        else if (message.m_type == "card_dialog_box")
        {
            show_card_dialog_box();
            m_card_dialog_box.GetComponent<card_dialog_box>().item_id = (int)message.m_ints[0];
            m_card_dialog_box.GetComponent<card_dialog_box>().type = (int)message.m_ints[1];
            if (message.m_object.Count > 0)
            {
                m_card_dialog_box.GetComponent<card_dialog_box>().m_card = (ccard)message.m_object[0];
            }
            m_card_dialog_box.SetActive(true);
        }
        else if (message.m_type == "treasure_dialog_box")
        {
            show_treasure_dialog_box();
            m_treasure_dialog_box.GetComponent<treasure_dialog_box>().item_id = (int)message.m_ints[0];
            m_treasure_dialog_box.GetComponent<treasure_dialog_box>().type = (int)message.m_ints[1];
            if (message.m_object.Count > 0)
            {
                m_treasure_dialog_box.GetComponent<treasure_dialog_box>().m_treasure = (dhc.treasure_t)message.m_object[0];
            }
            m_treasure_dialog_box.SetActive(true);
        }
        else if (message.m_type == "huiyi_dialog_box")
        {
            show_huiyi_dialog_box();
            m_huiyi_dialog_box.GetComponent<huiyi_dialog_box>().item_id = (int)message.m_ints[0];
            m_huiyi_dialog_box.SetActive(true);
        }
        else if (message.m_type == "select_dialog_box")
        {
            show_select_dialog_box();
            if (message.m_bools.Count > 0)
            {
                m_select_dialog_box.GetComponent<select_dialog_box>().flag = (bool)message.m_bools[0];
                m_select_dialog_box.GetComponent<select_dialog_box>().m_login = (bool)message.m_bools[1];

            }
            else
            {
                m_select_dialog_box.GetComponent<select_dialog_box>().m_login = false;
                m_select_dialog_box.GetComponent<select_dialog_box>().flag = false;
            }
            m_select_dialog_box.GetComponent<select_dialog_box>().set_des(message);
            m_select_dialog_box.SetActive(true);
        }
		else if (message.m_type == "pet_dialog_box")
		{
			show_pet_dialog_box();
			m_pet_dialog_box .GetComponent<pet_info>().item_id = (int)message.m_ints[0];
			m_pet_dialog_box.GetComponent<pet_info>().type = (int)message.m_ints[1];
			if (message.m_object.Count > 0)
			{
				m_pet_dialog_box.GetComponent<pet_info>().m_pet = (pet)message.m_object[0];
			}
			m_pet_dialog_box.SetActive(true);
		}
        else if (message.m_type == "tili_dialog_box")
        {
            show_tili_dialog_box();
            m_tili_dialog_box.GetComponent<tili_dialog_box>().set_des(message);

            m_tili_dialog_box.SetActive(true);
        }
        else if (message.m_type == "cishu_dialog_box")
        {
            show_cishu_dialog_bos();
            m_cishu_dialog_box.GetComponent<cishu_dialog_box>().num1 = (int)message.m_ints[0];
            m_cishu_dialog_box.GetComponent<cishu_dialog_box>().num2 = (int)message.m_ints[1];
            m_cishu_dialog_box.GetComponent<cishu_dialog_box>().buy_num = (int)message.m_ints[2];
            m_cishu_dialog_box.GetComponent<cishu_dialog_box>().type = (int)message.m_ints[3];
            m_cishu_dialog_box.GetComponent<cishu_dialog_box>().m_mes = (s_message)message.m_object[0];
            m_cishu_dialog_box.GetComponent<cishu_dialog_box>().updata_ui();
            m_cishu_dialog_box.SetActive(true);
        }
        else if (message.m_type == "hide_cishu_dialog_box")
        {
            if (m_cishu_dialog_box != null && m_cishu_dialog_box.activeSelf)
            {
                m_cishu_dialog_box.transform.Find("frame_big").GetComponent<frame>().hide();
            }

        }
        else if (message.m_type == "single_dialog_box")
        {
            show_single_dialog_box();
            m_single_dialog_box.GetComponent<select_dialog_box>().set_des(message);
            m_single_dialog_box.SetActive(true);
        }

        else if (message.m_type == "hide_cl_gui")
        {
            m_common_cl_panel.transform.Find("frame_big").GetComponent<frame>().hide();
        }
        else if (message.m_type == "hide_common_player_panel")
        {
            if (m_common_player_panel != null && m_common_player_panel.activeSelf)
            {
                m_common_player_panel.GetComponent<common_player_panel>().close();
            }

        }
        else if (message.m_type == "hide_player_info_panel")
        {
            if (m_player_info_gui != null && m_player_info_gui.activeSelf)
            {
                m_player_info_gui.transform.Find("frame_big").GetComponent<frame>().hide();
            }
        }
        else if (message.m_type == "other_dialog_box")
        {
            if (m_other_dialog_box == null)
            {
                m_other_dialog_box = game_data._instance.ins_object_res("ui/other_dialog_box");
                m_other_dialog_box.transform.parent = m_ui_cam.transform;
                m_other_dialog_box.AddComponent<gui_remove>();
                m_other_dialog_box.transform.localPosition = new Vector3(0, 0, 0);
                m_other_dialog_box.transform.localScale = new Vector3(1, 1, 1);
            }
            m_other_dialog_box.GetComponent<other_dialog_box>().set_des(message);
            m_other_dialog_box.SetActive(true);
        }

        else if (message.m_type == "show_duixing_gui")
        {
            show_duixing_gui();
            m_duixing_gui.GetComponent<duixing_gui>().up_flag = (bool)message.m_bools[0];
            m_duixing_gui.GetComponent<duixing_gui>().reset(0);
            m_duixing_gui.SetActive(true);
        }
        else if (message.m_type == "show_guild_fight_gui")
        {
            show_guild_fight_gui();
        }
        else if (message.m_type == "buy_num_gui")
        {
            show_buy_num_gui();
            m_buy_num_gui.SetActive(true);
            if ((int)message.m_ints[1] == 7 && (int)message.m_ints[0] == 0)
            {
                m_buy_num_gui.GetComponent<equip_buy_gui>().t_itemhecheng = message.m_object[0] as s_t_itemhecheng;
                m_buy_num_gui.GetComponent<equip_buy_gui>().m_type = (int)message.m_ints[1];
            }
            else
            {
                m_buy_num_gui.GetComponent<equip_buy_gui>().m_id = (int)message.m_ints[0];
                m_buy_num_gui.GetComponent<equip_buy_gui>().m_type = (int)message.m_ints[1];
                if (message.m_ints.Count > 2)
                {
                    m_buy_num_gui.GetComponent<equip_buy_gui>().shop_num = (int)message.m_ints[2];
                }
            }
            m_buy_num_gui.GetComponent<equip_buy_gui>().updata_ui();
        }
        else if (message.m_type == "show_select_num_gui")
        {
            show_select_num_gui(0);
            m_select_num_gui.SetActive(true);
        }
        else if (message.m_type == "hide_battle_gui")
        {
            hide_battle_gui();
        }
        else if (message.m_type == "show_cl_gui")
        {
            show_common_cl_panel();
            m_common_cl_panel.GetComponent<common_cl_panel>().m_id = (int)message.m_ints[0];
            if (message.m_ints.Count > 1)
            {
                m_common_cl_panel.GetComponent<common_cl_panel>().m_cid = (int)message.m_ints[1];
            }
            else
            {
                m_common_cl_panel.GetComponent<common_cl_panel>().m_cid = 0;
            }
            m_common_cl_panel.GetComponent<common_cl_panel>().init();
            m_common_cl_panel.SetActive(true);
        }
        else if (message.m_type == "show_levelup")
        {
            if (sys._instance.m_self.m_t_player.level >= 15)
            {
                m_hall_gui.GetComponent<hall_gui>().m_main_gui.gameObject.GetComponent<main_gui>().m_finger_pointer.SetActive(false);
            }
            if (game_data._instance.m_guaji > 0)
            {
                return;
            }
            show_level_gui();
            m_level_gui.SetActive(true);
            m_level_gui.GetComponent<levelup>().reset((int)message.m_ints[0], (int)message.m_ints[1]);
        }
        else if (message.m_type == "show_target_finish")
        {
            show_target_finish_gui();
            m_target_finish_gui.GetComponent<target_finish>().m_id = (int)message.m_ints[0];
            m_target_finish_gui.GetComponent<target_finish>().m_type = (int)message.m_ints[1];
            m_target_finish_gui.GetComponent<target_finish>().reset();
        }
        else if (message.m_type == "show_zhaomu_shuxing")
        {
            bool flag = false;
            show_zhaomu_shuxing();
            if (message.m_bools.Count > 0)
            {
                flag = (bool)message.m_bools[0];
            }
            m_zhaomu_shuxing.GetComponent<zhaomu_shuxing>().init((ccard)message.m_object[0], (s_message)message.m_object[1], flag);
            m_zhaomu_shuxing.SetActive(true);
        }
        else if (message.m_type == "show_hb_dress_gui")
        {
            show_hb_dress_gui();
            ulong guid = (ulong)message.m_long[0];
            m_hb_dress_gui.GetComponent<hb_dress_gui_ex>().init();
            m_hb_dress_gui.GetComponent<hb_dress_gui_ex>().reset(guid);
            m_hb_dress_gui.GetComponent<hb_dress_gui_ex>().m_message = (string)message.m_string[0];
            m_hb_dress_gui.SetActive(true);
        }
        else if (message.m_type == "hide_hb_dress_gui")
        {
            m_vr_gui.SetActive(true);
            m_hb_dress_gui.GetComponent<ui_show_anim>().hide_ui();
        }

        else if (message.m_type == "show_dress_gui")
        {
            show_dress_gui();
            m_dress_gui.GetComponent<dress_gui>().init();
            m_dress_gui.GetComponent<dress_gui>().reset();
            m_dress_gui.GetComponent<dress_gui>().m_message = (string)message.m_string[0];
            m_dress_gui.SetActive(true);
        }
        else if (message.m_type == "hide_dress_gui")
        {
            m_vr_gui.SetActive(true);
            //m_dress_gui.GetComponent<ui_show_anim>().hide_ui();
        }
        else if (message.m_type == "check_dress_target_done")
        {
            show_dress_gui();
            int tmp = m_dress_gui.GetComponent<dress_gui>().m_dress_chengjiu_gui.GetComponent<chengjiu_gui>().check_dress_target_done();
        }
        else if (message.m_type == "card_info_class")
        {
            show_card_info();
            m_card_info.GetComponent<card_info>().m_class = game_data._instance.get_t_class((int)message.m_ints[0]);
            m_card_info.GetComponent<card_info>().m_card = ccard.get_new_card(game_data._instance.get_t_class((int)message.m_ints[0]).id);
            m_card_info.GetComponent<card_info>().show_info();

            m_card_info.SetActive(true);
        }

        else if (message.m_type == "card_info_card" || message.m_type == "py_show_zl")
        {
            show_card_info();
            m_card_info.GetComponent<card_info>().m_card = sys._instance.m_self.get_card_guid((ulong)message.m_long[0]);
            m_card_info.GetComponent<card_info>().m_class = sys._instance.m_self.get_card_guid((ulong)message.m_long[0]).m_t_class;
            m_card_info.GetComponent<card_info>().show_info_t();

            m_card_info.SetActive(true);
        }
        else if (message.m_type == "clickliaotian")
        {
            show_chat_gui();
            m_chat_gui.GetComponent<chat_gui>().m_type = 0;
            m_chat_gui.transform.parent.gameObject.SetActive(true);
            m_chat_gui.SetActive(true);
            TweenPosition.Begin(m_chat_gui, 0.3f, new Vector3(0, 0, 0));
        }
        else if (message.m_type == "siliao")
        {
            show_chat_gui();
            m_chat_gui.GetComponent<chat_gui>().m_type = 2;
            m_chat_gui.GetComponent<chat_gui>().m_oppanel.SetActive(false);
            m_chat_gui.GetComponent<chat_gui>().m_tname.GetComponent<UIInput>().value = (string)message.m_string[0];
            m_chat_gui.GetComponent<chat_gui>().reset();
            m_chat_gui.transform.parent.gameObject.SetActive(true);
            m_chat_gui.SetActive(true);
            m_chat_gui.GetComponent<chat_gui>().m_shijie.GetComponent<UIToggle>().value = false;
            m_chat_gui.GetComponent<chat_gui>().m_siliao.GetComponent<UIToggle>().value = true;
            TweenPosition.Begin(m_chat_gui, 0.3f, new Vector3(0, 0, 0));

        }
        else if (message.m_type == "show_master_gui")
        {
            sys._instance.m_game_state = "master";
            sys._instance.load_scene("ts_game_master");
        }
	}
    void show_chat_gui()
    {
        //聊天
        if (m_chat_gui == null)
        {
            m_chat_gui = game_data._instance.ins_object_res("ui/chat_gui");
            m_chat_gui.transform.parent = m_ui_cam.transform;
            m_chat_gui.AddComponent<gui_remove>();
            m_chat_gui.transform.localScale = Vector3.one;
            m_chat_gui.transform.localPosition = new Vector3(m_chat_gui.transform.localPosition.x, m_chat_gui.transform.localPosition.y,0);
            m_chat_gui = m_chat_gui.transform.Find("chat_gui").gameObject;
            m_chat_gui.GetComponent<chat_gui>().init();
            m_chat_gui.transform.localPosition = new Vector3(-800, 0, 0);
        }
        
    }

	void Update()
	{
		if(m_mask != null 
		   && m_mask.activeSelf 
		   && sys._instance.m_is_loading == false)
		{
			if(m_mask.GetComponent<UIPanel>().alpha > 0)
			{
				m_mask.GetComponent<UIPanel>().alpha -= Time.deltaTime;
			}
			else
			{
				m_mask.SetActive(false);
			}
		}
		
		if (m_ui_mask != null && m_ui_mask.activeSelf == true)
		{
			m_ui_mask_time -= Time.deltaTime;
			if (m_ui_mask_time <= 0)
			{
				m_ui_mask.SetActive(false);
			}
		}
	}
}
