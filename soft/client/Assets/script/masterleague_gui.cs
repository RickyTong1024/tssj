
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class masterleague_gui : MonoBehaviour,IMessage {

    public protocol.team.smsg_enter_world m_world;
    public UILabel m_saijileave;
    public UILabel m_danfurank;
    public UILabel m_allrank;
    public UILabel m_player_name;
    public UILabel m_chenhao;
    public UILabel m_bf;
    public UILabel m_reward_count;
    public UILabel m_reset_label;
    public GameObject m_duanwei;
    public  int _num;
    public GameObject _unit;
    public GameObject m_loopunit;
    public GameObject m_mask;
    public bool flag = false;
    public GameObject m_reward_gui;
    public GameObject m_rank_gui;
    public GameObject m_shop_gui;
    public GameObject m_duanwei_reset_gui;
    public GameObject m_target_effect;
    public GameObject m_wait_pipei;
    public GameObject m_right_info;
    public GameObject m_master_info;
    public static masterleague_gui _instance;
    public bool m_flag;
	public GameObject m_defeat_time;
	public GameObject m_defeat_huafei;
	public GameObject m_defeat_clear_plane;
	private int m_jewlt;
	void Start () 
    {
        cmessage_center._instance.add_handle(this);
	
	}
    void OnEnable()
    {
        
        if (flag)
        {
            reset();
            return;
        }
        flag = true;
        _instance = this;
        protocol.game.cmsg_common msg = new protocol.game.cmsg_common();
        net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_TEAM_ENTER, msg);
    }
    
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }
    public void click(GameObject obj)
    {
        if (obj.name == "close") {
			if (m_master_info.activeSelf) {
				m_master_info.transform.Find("frame_big").GetComponent<frame>().hide ();
				return;
			}
			if (m_reward_gui.activeSelf) {
				m_reward_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
				return;
			}
            
			if (m_rank_gui.activeSelf) {
				m_rank_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
				return;
			}
			if (m_shop_gui.activeSelf) {
				m_shop_gui.transform.Find("frame_big").GetComponent<frame>().hide ();
				return;
			}
			s_message _message = new s_message ();
			_message.m_type = "show_huo_dong";
			_message.m_ints.Add (12);
			_message.m_bools.Add (m_flag);
			m_flag = false;
			cmessage_center._instance.add_message (_message);
			sys._instance.m_game_state = "hall";
			sys._instance.load_scene (sys._instance.m_hall_name);
			net_tcp_bingyua._instance.disconnect ();
			if (_unit != null) {
				Destroy (_unit);
			}
			if (m_loopunit != null) {
				Destroy (m_loopunit);
 
			}
			Destroy (this.gameObject); 
		} else if (obj.name == "add") {
 
		} else if (obj.name == "buy") {
			s_t_price t_price = game_data._instance.get_t_price (sys._instance.m_self.m_t_player.ds_reward_buy + 1);
			s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
			if (sys._instance.m_self.m_t_player.ds_reward_buy >= t_vip.master_buy_num) {
				root_gui._instance.show_prompt_dialog_box ("[ffc882]" + game_data._instance.get_t_language ("masterleague_gui.cs_110_71"));//今日购买次数不足，提升vip等级可增加每日购买次数
				return;
			}
			s_message _mes = new s_message ();
			_mes.m_type = "buy_master_reward_num";
			root_gui._instance.show_cishu_dialog_box (sys._instance.m_self.m_t_player.ds_reward_num, t_vip.master_buy_num,
                sys._instance.m_self.m_t_player.ds_reward_buy, 4, _mes);


		} else if (obj.name == "pipei") {
			if (timer.now () > m_world.ds_cdtime) {
				net_tcp_bingyua._instance.send_msg_null (opclient_t.CMSG_DS_MATCH);
			} else {
				m_defeat_clear_plane.SetActive(true);

			}
           
		} else if (obj.name == "day_reward") {
			m_reward_gui.SetActive (true);
			m_reward_gui.GetComponent<master_reward_gui>().reset (1);
		} else if (obj.name == "rank") {
			m_rank_gui.SetActive (true);

		} else if (obj.name == "shop") {
			m_shop_gui.SetActive (true);
		} else if (obj.name == "mask") {
			m_duanwei_reset_gui.SetActive (false);
		} else if (obj.name == "tj") {
			m_master_info.SetActive (true);
		} else if (obj.name == "cancel_pipei") {
			net_tcp_bingyua._instance.send_msg_null (opclient_t.CMSG_DS_MATCH_STOP);
		} else if (obj.name == "yes") {
			if(sys._instance.m_self.m_t_player.jewel >= m_jewlt)
			{
			protocol.game.cmsg_common meg= new protocol.game.cmsg_common();

			net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_DS_TIME_BUY,meg);
			}
			else
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("baowu_ronglian_gui.cs_141_59"));//钻石不足
				return;
			}

		} else if (obj.name == "no") {
			m_defeat_clear_plane.SetActive(false);
		}
 

    }
    public static bool is_effect()
    {
        if (sys._instance.m_self.m_t_player.ds_reward_num > 0 || master_reward_gui.is_effect())
        {
            return true;
        }
        return false;
    }
	public void message (s_message message)
    {
        if (message.m_type == "bingyuan_login_check")
        {
            protocol.team.cmsg_enter_world msg = new protocol.team.cmsg_enter_world();
            msg.guid = sys._instance.m_self.m_t_player.guid;
            msg.sig = sys._instance.m_self.m_sig;
            msg.type = 1;
            net_tcp_bingyua._instance.send_msg<protocol.team.cmsg_enter_world>(opclient_t.CMSG_ENTER_TEAM_SERVER, msg);

        }
        else if (message.m_type == "buy_master_reward_num")
        {
            protocol.game.cmsg_shop_buy _msg = new protocol.game.cmsg_shop_buy();
            _msg.num = (int)message.m_ints[0];
            _num = _msg.num;
            net_http._instance.send_msg<protocol.game.cmsg_shop_buy>(opclient_t.CMSG_DS_FIGHT_BUY, _msg);
        }
        else if (message.m_type == "refresh_master")
        {
            reset();
 
        }
        else if (message.m_type == "daily_refresh")
        {
           
        }

    }
	public void net_message (s_net_message message)
    {
        if (message.m_opcode == opclient_t.CMSG_TEAM_ENTER) {
			//protocol.team.smsg_ds_cdtime cdtimer = net_http._instance.parse_packet<protocol.team.smsg_ds_cdtime>(message.m_byte);
			net_tcp_bingyua._instance.connect ();
		} else if (message.m_opcode == opclient_t.SMSG_ENTER_TEAM_SERVER) {
			m_world = net_tcp_bingyua._instance.parse_packet<protocol.team.smsg_enter_world> (message.m_byte);

			reset ();
			if (sys._instance.m_state_flag == 1) {
				sys._instance.m_state_flag = 0;
				m_flag = true;
				m_shop_gui.SetActive (true);
			}
		} else if (message.m_opcode == opclient_t.CMSG_DS_FIGHT_BUY) {
			int jewel = 0;

			for (int i = 0; i < _num; i++) {
				jewel += game_data._instance.get_t_price (sys._instance.m_self.m_t_player.ds_reward_buy + 1 + i).master;
			}
			sys._instance.m_self.sub_att (e_player_attr.player_jewel, jewel, game_data._instance.get_t_language ("masterleague_gui.cs_219_75"));//大师联赛购买奖励次数消耗
			string s = game_data._instance.get_t_language ("bingyuan_gui.cs_776_19");//获得
			root_gui._instance.show_prompt_dialog_box ("[00ff00]" + s, 0, "", "[ffffc0]" + "大师联赛奖励次数" + "[ffd000] + " + _num.ToString ());
			sys._instance.m_self.m_t_player.ds_reward_buy += _num;
			sys._instance.m_self.m_t_player.ds_reward_num += _num;
			reset ();
            
		} else if (message.m_opcode == opclient_t.SMSG_DS_MATCH) {
			m_duanwei.SetActive (false);
			m_wait_pipei.SetActive (true);
			StartCoroutine (pipei_time ());
 
		} else if (message.m_opcode == opclient_t.SMSG_DS_FIGHT_END) {
			protocol.team.smsg_fight_ds fight = net_http._instance.parse_packet<protocol.team.smsg_fight_ds> (message.m_byte);
			m_world.ds_cdtime =fight.cd_time;
			create_right (fight);
			m_mask.SetActive (true);
			Invoke ("start_fight", 2);
			StopAllCoroutines ();
			m_wait_pipei.SetActive (false);
			protocol.game.cmsg_ds_fight _f = new protocol.game.cmsg_ds_fight ();
			_f.id = fight.id;
			net_http._instance.send_msg<protocol.game.cmsg_ds_fight> (opclient_t.CMSG_DS_FIGHT_END, _f);
			battle_logic_ex._instance.set_ds_fight_end (fight);
 
		} else if (message.m_opcode == opclient_t.CMSG_DS_FIGHT_END) {
			protocol.game.smsg_ds_fight_end fightend = net_http._instance.parse_packet<protocol.game.smsg_ds_fight_end> (message.m_byte);
			m_world.ds_cdtime = fightend.cd_time;
			sys._instance.m_self.m_t_player.ds_hit++;
            
			{
				sys._instance.m_self.m_t_player.ds_point += fightend.point;
				m_world.spoint += fightend.point;
			}
			if (sys._instance.m_self.m_t_player.ds_point < 0) {
				sys._instance.m_self.m_t_player.ds_point = 0;
			}
			if (m_world.spoint < 0) {
				m_world.spoint = 0;
			}
			m_world.srank = fightend.srank;
			m_world.sgrank = fightend.grank;
			if (fightend.ciliao > 0) {
				sys._instance.m_self.add_item (110020001, fightend.ciliao, false, game_data._instance.get_t_language ("masterleague_gui.cs_270_79"));//大师联赛获得
			}
           
			sys._instance.m_self.add_att ((e_player_attr)27, fightend.xinpian, false);
			if (--sys._instance.m_self.m_t_player.ds_reward_num < 0) {
				sys._instance.m_self.m_t_player.ds_reward_num = 0;
			}
			{
				s_message mes = new s_message ();
				mes.m_type = "set_ds_fight_end";
				mes.m_object.Add (fightend);
				mes.m_ints.Add (m_world.sduanwei);
				mes.m_ints.Add (fightend.duanwei);
				cmessage_center._instance.add_message (mes);
 
			}
          
			m_world.sduanwei = fightend.duanwei;
			sys._instance.m_self.m_t_player.ds_duanwei = fightend.duanwei;
		} else if (message.m_opcode == opclient_t.SMSG_DS_MATCH_TIMEOUT) {
			m_wait_pipei.SetActive (false);
			StopAllCoroutines ();
			m_duanwei.SetActive (true);
			root_gui._instance.show_prompt_dialog_box ("[ffc882]" + game_data._instance.get_t_language ("masterleague_gui.cs_296_67"));//暂时匹配不到对手，请稍后再试！
		} else if (message.m_opcode == opclient_t.SMSG_DS_MATCH_STOP) {
			m_wait_pipei.SetActive (false);
			StopAllCoroutines ();
			m_duanwei.SetActive (true);
 
		} else if (message.m_opcode == opclient_t.SMSG_DS_MATCH_CD) {
			root_gui._instance.show_prompt_dialog_box ("[ffc882]" + game_data._instance.get_t_language ("masterleague_gui.cs_282_59"));//您匹配的太频繁了，请稍后再来！
		} else if (message.m_opcode == opclient_t.CMSG_DS_TIME_BUY) {
			protocol.game.smsg_huodong_fanpai fp= net_http._instance.parse_packet<protocol.game.smsg_huodong_fanpai> (message.m_byte);
			m_world.ds_cdtime = 0;
			battle._instance.m_timer_cd = 0;
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, fp.id,game_data._instance.get_t_language ("masterleague_gui.cs_287_66"));	//大师段位重置战败冷却消耗
			m_defeat_clear_plane.SetActive(false);

		}

    }
    void start_fight()
    {
        sys._instance.m_game_state = "buttle";
        sys._instance.load_scene("ts_fight_jjc");
        this.gameObject.SetActive(false);
        Destroy(m_loopunit);
        Destroy(_unit);
 
    }
    void create_right(protocol.team.smsg_fight_ds fight)
    {
        m_right_info.SetActive(true);
        m_loopunit = sys._instance.create_class
           ((int)fight.template_id,fight.achieve, fight.guanghuan);
        m_loopunit.transform.localPosition = new Vector3(1.2f, -0.1f, -6.5f);
        m_loopunit.transform.localEulerAngles = new Vector3(0, 180, 0);
        m_loopunit.transform.localScale = Vector3.one;
        if ((int)fight.template_id == 27)
        {
            m_loopunit.transform.localScale = new Vector3(0.7f,0.7f,0.7f);
        }

        m_right_info.transform.Find("bf").GetComponent<UILabel>().text = game_data._instance.get_t_language ("masterleague_gui.cs_329_78") + sys._instance.value_to_wan(fight.bf);//战斗力：
        s_t_master_duanwei duanwei = game_data._instance.get_t_master_duanwei(fight.duanwei);
        m_right_info.transform.Find("name").GetComponent<UILabel>().text = 
             game_data._instance.get_name_color(fight.achieve) + fight.name;
        m_right_info.transform.Find("chenhao").GetComponent<UILabel>().text = duanwei.duanwei;
        m_right_info.transform.Find("chenhao").GetComponent<UILabel>().gradientTop = sys._instance.string_to_color(duanwei.topcolor);
        m_right_info.transform.Find("chenhao").GetComponent<UILabel>().gradientBottom = sys._instance.string_to_color(duanwei.bottomcolor);
        
 
    }
    IEnumerator pipei_time()
    {
        int time = 0;
        while (true)
        {
            m_wait_pipei.transform.Find("time").GetComponent<UILabel>().text
            = string.Format(game_data._instance.get_t_language ("bingyuan_gui.cs_1609_28"), time);//已用时：[00ff00]{0}秒
            time++;
            yield return new WaitForSeconds(1);
			if(time >= 10)
			{
				m_wait_pipei.SetActive(false);
				StopAllCoroutines();
				m_duanwei.SetActive(true);
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("masterleague_gui.cs_296_67"));//暂时匹配不到对手，请稍后再试！
				break;

			}

        }

    }
    public void reset()
    {
        
        CancelInvoke();
        InvokeRepeating("time",0,1);
        m_right_info.SetActive(false);
        m_mask.SetActive(false);
        if (m_world.srank != -1)
        {
            (m_danfurank.transform.Find("rank").GetComponent("UILabel") as UILabel).text = m_world.srank + "";
             m_danfurank.transform.Find("wei").gameObject.SetActive(false);
            
        }
        else
        {
            (m_danfurank.transform.Find("rank").GetComponent("UILabel") as UILabel).text = "";
            m_danfurank.transform.Find("wei").gameObject.SetActive(true);
        }
        if (m_world.sgrank != -1)
        {
            m_allrank.transform.Find("rank").GetComponent<UILabel>().text = m_world.sgrank + "";
            m_allrank.transform.Find("wei").gameObject.SetActive(false);
        }
        else
        {
            m_allrank.transform.Find("rank").GetComponent<UILabel>().text = "";  
            m_allrank.transform.Find("wei").gameObject.SetActive(true);
        }
        m_duanwei.SetActive(true);
        m_player_name.text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) +  sys._instance.m_self.m_t_player.name;
        m_bf.text = game_data._instance.get_t_language ("masterleague_gui.cs_329_78") + sys._instance.value_to_wan(sys._instance.m_self.m_t_player.bf);//战斗力：
        m_reward_count.text = string.Format(game_data._instance.get_t_language ("masterleague_gui.cs_392_44"), sys._instance.m_self.m_t_player.ds_reward_num);//今日奖励次数：{0}
        if (_unit == null)
        {
            _unit = sys._instance.create_class
            ((int)sys._instance.m_self.m_t_player.template_id, sys._instance.m_self.m_t_player.dress_achieves.Count, sys._instance.m_self.m_t_player.guanghuan_id);
            _unit.transform.localPosition = new Vector3(-1.2f, -0.1f, -6.5f);
            _unit.transform.localEulerAngles = new Vector3(0, 180, 0);
            _unit.transform.localScale = Vector3.one;
            if ((int)sys._instance.m_self.m_t_player.template_id == 27)
            {
                _unit.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
            }

        }
        s_t_master_duanwei duanwei = game_data._instance.get_t_master_duanwei(m_world.sduanwei);
        s_t_master_duanwei nextduanwei = game_data._instance.get_t_master_duanwei(m_world.sduanwei + 1);
        if (duanwei != null)
        {
            m_chenhao.text = duanwei.duanwei;
            m_duanwei.transform.Find("name").GetComponent<UILabel>().text = duanwei.duanwei;

        }
        else
        {
            m_chenhao.text = game_data._instance.get_t_language ("masterleague_gui.cs_416_29");//青铜
            m_duanwei.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_t_language ("masterleague_gui.cs_416_29");//青铜
            return;
        }
       
        
        m_duanwei.transform.Find("name").GetComponent<UILabel>().text = duanwei.duanwei;
        m_duanwei.transform.Find("name").GetComponent<UILabel>().gradientTop = sys._instance.string_to_color(duanwei.topcolor);
        m_duanwei.transform.Find("name").GetComponent<UILabel>().gradientBottom = sys._instance.string_to_color(duanwei.bottomcolor);
        m_duanwei.transform.Find("duanwei").GetComponent<UISprite>().spriteName = duanwei.icon;
        m_duanwei.transform.Find("kuang").GetComponent<UISprite>().spriteName = duanwei.kuang;
        for (int i = 1; i <= 3; i++)
        {
            if (i <= duanwei.starcount)
            {
                m_duanwei.transform.Find("" + i).GetComponent<UISprite>().spriteName = duanwei.staricon;

                m_duanwei.transform.Find("" + i).gameObject.SetActive(true);
            }
            else
            {
                m_duanwei.transform.Find("" + i).gameObject.SetActive(false);
            }
 
        }
        m_duanwei.transform.Find("tiaojian").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("masterleague_gui.cs_441_95"), m_world.spoint);//积分：{0}
        if (nextduanwei != null)
        {
            m_duanwei.transform.Find("num").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("masterleague_gui.cs_444_94"), nextduanwei.duanwei, nextduanwei.need_jifen);//【{0}:积分{1}】

        }
        else
        {
            m_duanwei.transform.Find("num").GetComponent<UILabel>().text = "";
 
        }
        if (m_world.sreset)
        {
            m_duanwei_reset_gui.SetActive(true);
            GameObject preobj = m_duanwei_reset_gui.transform.Find("pre").gameObject;
            GameObject cur = m_duanwei_reset_gui.transform.Find("current").gameObject;
            s_t_master_duanwei pre_duanwei = game_data._instance.get_t_master_duanwei(m_world.soduanwei);
            s_t_master_duanwei cur_duanwei = game_data._instance.get_t_master_duanwei(m_world.sduanwei);
            set_duanwei(pre_duanwei, preobj);
            set_duanwei(cur_duanwei, cur);
            m_world.sreset = false;
            m_reset_label.text = string.Format(game_data._instance.get_t_language ("masterleague_gui.cs_462_47"),game_data._instance.get_t_master_duanwei(m_world.sduanwei).duanwei);//根据您上赛季的表现，这赛季您将从【{0}】开始联赛。
        }
        if (master_reward_gui.is_effect())
        {
            m_target_effect.SetActive(true);

        }
        else
        {
            m_target_effect.SetActive(false);
 
        }

    }
    void set_duanwei(s_t_master_duanwei duanwei,GameObject obj)
    {
        obj.transform.Find("name").GetComponent<UILabel>().text = duanwei.duanwei;
        obj.transform.Find("name").GetComponent<UILabel>().gradientTop = sys._instance.string_to_color(duanwei.topcolor);
        obj.transform.Find("name").GetComponent<UILabel>().gradientBottom = sys._instance.string_to_color(duanwei.bottomcolor);
        obj.GetComponent<UISprite>().spriteName = duanwei.icon;
        obj.transform.Find("kuang").GetComponent<UISprite>().spriteName = duanwei.kuang;
        for (int i = 1; i <= 3; i++)
        {
            if (i <= duanwei.starcount)
            {
                obj.transform.Find("" + i).GetComponent<UISprite>().spriteName = duanwei.staricon;

                obj.transform.Find("" + i).gameObject.SetActive(true);
            }
            else
            {
                obj.transform.Find("" + i).gameObject.SetActive(false);
            }

        }

 
    }
    void time()
    {
        long netmonday = timer.get_next_mondy();
        long now = (long)timer.now();
        long time = netmonday + 7 * 24 * 60 * 60 * 1000 - (long)timer.now();
        m_saijileave.text = timer.get_time_show_ex(time);
		if (m_world != null) {
			//if(battle._instance.)
			if(m_world.ds_cdtime == 0)
			{
			m_world.ds_cdtime = battle._instance.m_timer_cd;
			}
			if (timer.now () < m_world.ds_cdtime) {
				long defeat_timer = (long)(m_world.ds_cdtime - timer.now ());
				long residue_timer = defeat_timer / 1000;
				m_jewlt = (int)residue_timer / 5 + 1;
				m_defeat_time.SetActive(true);
				m_defeat_time.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("masterleague_gui.cs_503_65"), timer.get_time_show_mAnds (defeat_timer)); // 战败冷却：{0}
				m_defeat_huafei.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("masterleague_gui.cs_504_67"), "[2FD4FF]"+m_jewlt.ToString()+"[FFFFFF]");//是否花费{0}钻石清除冷却时间
			}else
			{
				m_defeat_clear_plane.SetActive(false);
				m_defeat_time.SetActive(false);
			}
		}

    }

	void Update () {
	
	}
}
