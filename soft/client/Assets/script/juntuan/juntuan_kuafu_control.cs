
using System.Collections.Generic;
using UnityEngine;

public class juntuan_kuafu_control : MonoBehaviour,IMessage {

	

	public GameObject m_guild_kuafu_shuoming;
	public GameObject m_guild_kuafu_tishi;
	
	public GameObject m_guild_kuafu_left;
	public GameObject m_guild_kuafu_right;
	public GameObject m_guild_kuafu_main;
	public GameObject m_guild_kuafu_command;
	public GameObject m_today_zhanji;
	public GameObject m_button_shuoming;
	public GameObject m_button_baoming;
	public GameObject m_button_tishi_quxiao;
	public GameObject m_button_tishi_queding;
//	public GameObject m_button_top_shuoming;
	public GameObject m_button_l;
	public GameObject m_button_r;
	public GameObject m_button_m;
	public GameObject m_button_c;
	public GameObject start_timer;
	public List<GameObject> show_jt_jincheng =new List<GameObject>();
	public UILabel m_biaoti;
	public UILabel m_neirong;
	public GameObject m_guild_kuafu_set;
	public GameObject m_tishi_save;
    public GameObject m_guild_fight_gui;
	public protocol.game.smsg_guild_fight_pvp_look look_msg;  //军团战查看
	public string m_text;
	public int m_gamestat;
	public GameObject m_rank_panel;
	public GameObject m_gongporeward;
	int m_N;
	void Start()
	{
		if (root_gui._instance.m_default_active == "show_rank")
		{

			m_rank_panel.SetActive(true);
			m_guild_kuafu_tishi.transform.Find("frame_big").GetComponent<frame>().hide ();

		}
		Invoke("yanshi",0.5f);
	}
	void yanshi()
	{
		root_gui._instance.m_default_active ="";
	}
	void OnEnable()
	{
		InvokeRepeating ("m_time",0.0f,0.1f);

	}
	void Awake () 
	{
		cmessage_center._instance.add_handle (this);

	}
	void OnDisable()
	{
		CancelInvoke ("m_time");
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
		CancelInvoke ("m_time");
		// m_guild_hongbao.GetComponent<guild_hongbao_gui>().Destroy();
	}

	void IMessage.message (s_message message)
	{
		
		
		
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_GIILD_JT_APPLY)
		{
			juntuan_gui._instance.m_guild_member_t.Clear();
			protocol.game.smsg_guild_fight_pvp_look b_msg =net_http._instance.parse_packet<protocol.game.smsg_guild_fight_pvp_look>(message.m_byte);
			m_gamestat = b_msg.stat;
			for (int i = 0; i < b_msg.member.player_guids.Count; i++)
			{
				dhc.guild_member_t temp = new dhc.guild_member_t();
				temp.bf = b_msg.member.player_bat_eff[i];   //战斗力
				temp.player_guid = b_msg.member.player_guids[i];
				temp.player_iocn_id = b_msg.member.player_template[i];
				temp.player_vip = b_msg.member.player_vip[i];
				temp.player_name = b_msg.member.player_names[i];
				temp.player_level = b_msg.member.player_level[i];
				temp.join_time =b_msg.member.player_join_time[i];
				juntuan_gui._instance.m_guild_member_t.Add(temp);
			}

			if(juntuan_gui._instance.m_button_top_shuoming.activeSelf)
			{
				juntuan_gui._instance.m_guild_member_t.Sort (compare);
				
				for (int i = 0; i < juntuan_gui._instance.m_guild_member_t.Count; i++)
				{
					if (juntuan_gui._instance.m_guild_member_t [i].join_time + 24 * 60 * 60 * 1000   >= timer.now())
					{
						juntuan_gui._instance.m_defend_type.Add (5);
					} 
					else
					{
						juntuan_gui._instance.m_defend_type.Add (0); ////  type 0 为为布防，1  左据点 2右  3 主  4 司令。 5加入军团不满24小时
					}
				}
			}

			juntuan_gui._instance.m_defend_type_temp.Clear();
			for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++) 
			{
				juntuan_gui._instance.m_defend_type_temp.Add(juntuan_gui._instance.m_defend_type[i]);
			}
			reset();
		}
		else if(message.m_opcode == opclient_t.CMSG_GUILD_JT_BUSHU)
		{
			for (int i = 0; i < juntuan_gui._instance.m_defend_type_temp.Count; i++) {

				juntuan_gui._instance.m_defend_type_temp[i]=juntuan_gui._instance.m_defend_type[i];
			}
			m_guild_kuafu_set.transform.Find("frame_big").GetComponent<frame>().hide ();

			m_guild_kuafu_left.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (1), game_data._instance.get_guild_fight (0).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_right.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (2), game_data._instance.get_guild_fight (1).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_main.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (3), game_data._instance.get_guild_fight (2).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_command.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (4), game_data._instance.get_guild_fight (3).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			//城防只显示
			m_guild_kuafu_left.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (0).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_right.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (1).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_main.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (2).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_command.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (3).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}

			
			
		}
	}
	public void reset()
	{

		if (look_msg.fight != null)
		{
			this.transform.Find("gongporeward").gameObject.SetActive(true);
		}

		if (m_gamestat == 4)
		{
			m_guild_kuafu_tishi.SetActive (false);
			if (juntuan_gui._instance.m_zhiwu_t == 2)
			{
				m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_left.transform.Find("player_name").transform.Find("add_l").gameObject.SetActive (false);
				m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_right.transform.Find("player_name").transform.Find("add_r").gameObject.SetActive (false);
				m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_main.transform.Find("player_name").transform.Find("add_m").gameObject.SetActive (false);
				m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_command.transform.Find("player_name").transform.Find("add_c").gameObject.SetActive (false);

				juntuan_gui._instance.m_guild_kuafu.SetActive (true);
			}
			else
			{ 
				m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_left.transform.Find("player_name").transform.Find("add_l").gameObject.SetActive (true);
				m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_right.transform.Find("player_name").transform.Find("add_r").gameObject.SetActive (true);
				m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_main.transform.Find("player_name").transform.Find("add_m").gameObject.SetActive (true);
				m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_command.transform.Find("player_name").transform.Find("add_c").gameObject.SetActive (true);
				
				juntuan_gui._instance.m_guild_kuafu.SetActive (true);
			}

			m_guild_kuafu_left.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (1), game_data._instance.get_guild_fight (0).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_right.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (2), game_data._instance.get_guild_fight (1).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_main.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (3), game_data._instance.get_guild_fight (2).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_command.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (4), game_data._instance.get_guild_fight (3).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			//城防只显示
			m_guild_kuafu_left.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (0).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_right.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (1).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_main.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (2).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_command.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (3).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
		} 
		else if (m_gamestat == 0)
		{//预留 符合军团战报名时间和条件时；
			if (juntuan_gui._instance.m_zhiwu_t == 2)
			{ //职位为普通成员在报名阶段只看得到据点，无提示框
				m_guild_kuafu_tishi.SetActive (false);
				juntuan_gui._instance.m_guild_kuafu.SetActive (true);
			} else
			{
				m_guild_kuafu_tishi.SetActive (true);
				m_button_baoming.SetActive (true);
				m_button_shuoming.SetActive (false);
				
				m_biaoti.text = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_215_20");//军团战报名
				m_neirong.alignment = NGUIText.Alignment.Center;
				m_neirong.text = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_217_21");//[0AABFF]报名时间: [0AFF16]周一0:00~周二10:00{nn}{nn}[B3FE13](军团长和副军团长可以报名)
				juntuan_gui._instance.m_guild_kuafu.SetActive (true);
			}

			m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (false);
			juntuan_gui._instance.m_guild_kuafu.SetActive (true);
		} 
		else if (m_gamestat == 1)
		{//预留 军团战进行 未报名
			m_guild_kuafu_tishi.SetActive(true);
			m_button_baoming.SetActive (false);
			m_button_shuoming.SetActive (true);
			m_biaoti.text = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_236_19");//军团战正在进行
			m_neirong.alignment = NGUIText.Alignment.Left;
            m_neirong.text = game_data._instance.get_t_language("juntuan_kuafu_control.cs_238_20");//本轮军团战已经开启,你所在的军团{nn}缺席了本次军团战,请在下次军团战{nn}开启时及时报名,如有疑问可查看军{nn}团战说明
			
			m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (false);
			
			juntuan_gui._instance.m_guild_kuafu.SetActive (true);
		} 

		else if (m_gamestat == 5)
		{
            this.gameObject.SetActive(false);
            s_message _mes = new s_message();
            _mes.m_type = "show_guild_fight_gui";
            cmessage_center._instance.add_message(_mes);
 
		}
		else if (look_msg.zhanji.guild_total_zhanji == 0 && m_gamestat == 3)
		{//预留 军团战结束 、未报名
			m_guild_kuafu_tishi.SetActive(true);
			
			m_button_baoming.SetActive (false);
			m_button_shuoming.SetActive (true);
			m_biaoti.text = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_266_19");//本轮军团战结束
			m_neirong.alignment = NGUIText.Alignment.Left;
            m_neirong.text = game_data._instance.get_t_language("juntuan_kuafu_control.cs_238_20");//本轮军团战已经开启,你所在的军团{nn}缺席了本次军团战,请在下次军团战{nn}开启时及时报名,如有疑问可查看军{nn}团战说明
			m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (false);
			juntuan_gui._instance.m_guild_kuafu.SetActive (true);
		} 
		else if (m_gamestat == 3 && look_msg.zhanji.guild_total_zhanji != 0)  // 休战周， 军团上周参见了军团战。
		{
			show_ui();
			m_today_zhanji.SetActive(true);
			m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_left.transform.Find("player_name").transform.Find("add_l").gameObject.SetActive (false);
			m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_right.transform.Find("player_name").transform.Find("add_r").gameObject.SetActive (false);
			m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_main.transform.Find("player_name").transform.Find("add_m").gameObject.SetActive (false);
			m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = false;
			m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (false);
			m_guild_kuafu_command.transform.Find("player_name").transform.Find("add_c").gameObject.SetActive (false);
			
			juntuan_gui._instance.m_guild_kuafu.SetActive (true);
//			if (juntuan_gui._instance.m_zhiwu_t == 2)
//			{
//				m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = false;
//				m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (false);
//				m_guild_kuafu_left.transform.Find("player_name").transform.Find("add_l").gameObject.SetActive (false);
//				m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = false;
//				m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (false);
//				m_guild_kuafu_right.transform.Find("player_name").transform.Find("add_r").gameObject.SetActive (false);
//				m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = false;
//				m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (false);
//				m_guild_kuafu_main.transform.Find("player_name").transform.Find("add_m").gameObject.SetActive (false);
//				m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = false;
//				m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (false);
//				m_guild_kuafu_command.transform.Find("player_name").transform.Find("add_c").gameObject.SetActive (false);
//				
//				juntuan_gui._instance.m_guild_kuafu.SetActive (true);
//			}
//			else
//			{ 
//				m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = false;
//				m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (false);
//				m_guild_kuafu_left.transform.Find("player_name").transform.Find("add_l").gameObject.SetActive (false);
//				m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = false;
//				m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (false);
//				m_guild_kuafu_right.transform.Find("player_name").transform.Find("add_r").gameObject.SetActive (false);
//				m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = false;
//				m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (false);
//				m_guild_kuafu_main.transform.Find("player_name").transform.Find("add_m").gameObject.SetActive (false);
//				m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = false;
//				m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (false);
//				m_guild_kuafu_command.transform.Find("player_name").transform.Find("add_c").gameObject.SetActive (false);
//				
//				juntuan_gui._instance.m_guild_kuafu.SetActive (true);
//			}
		}
		else if (m_gamestat == 6) //  开战周，  处于 周3 到周日 的  0： 00 ： 10: 00 之间   可以参看前一日战绩
		{

			show_ui();
			m_today_zhanji.SetActive(true);
			if (juntuan_gui._instance.m_zhiwu_t == 2)
			{
				m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_left.transform.Find("player_name").transform.Find("add_l").gameObject.SetActive (false);
				m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_right.transform.Find("player_name").transform.Find("add_r").gameObject.SetActive (false);
				m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_main.transform.Find("player_name").transform.Find("add_m").gameObject.SetActive (false);
				m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_command.transform.Find("player_name").transform.Find("add_c").gameObject.SetActive (false);
				
				juntuan_gui._instance.m_guild_kuafu.SetActive (true);
			}
			else
			{ 
				m_guild_kuafu_left.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_left.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_left.transform.Find("player_name").transform.Find("add_l").gameObject.SetActive (true);
				m_guild_kuafu_right.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_right.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_right.transform.Find("player_name").transform.Find("add_r").gameObject.SetActive (true);
				m_guild_kuafu_main.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_main.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_main.transform.Find("player_name").transform.Find("add_m").gameObject.SetActive (true);
				m_guild_kuafu_command.GetComponent<BoxCollider>().enabled = true;
				m_guild_kuafu_command.transform.Find("player_name").gameObject.SetActive (true);
				m_guild_kuafu_command.transform.Find("player_name").transform.Find("add_c").gameObject.SetActive (true);
				
				juntuan_gui._instance.m_guild_kuafu.SetActive (true);
			}
			
			m_guild_kuafu_left.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (1), game_data._instance.get_guild_fight (0).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_right.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (2), game_data._instance.get_guild_fight (1).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_main.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (3), game_data._instance.get_guild_fight (2).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			m_guild_kuafu_command.transform.Find("player_name/num").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_133_120"), ReturnTypeNum (4), game_data._instance.get_guild_fight (3).defendrolenum);//[0AFEFF]守军 [0AFF16]{0}/{1}
			//城防只显示
			m_guild_kuafu_left.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (0).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_right.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (1).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_main.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (2).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
			m_guild_kuafu_command.transform.Find("player_name/chengfang").gameObject.GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_138_126"), game_data._instance.get_guild_fight (3).chengfangvalue);//[0AFEFF]城防 [0AFF16]{0}
		}
	}

	void show_ui()
	{
		m_today_zhanji.transform.Find("back/back_1/juntuan_fight/todayzhan").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_386_121"),look_msg.zhanji.guild_zhanji);//[0AABFF]本轮军团战绩：[0AFF16]{0}
		m_today_zhanji.transform.Find("back/back_1/juntuan_fight/totalzhan").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_387_121"),look_msg.zhanji.guild_total_zhanji);//[0AABFF]军团总战绩：[0AFF16]{0}
		m_today_zhanji.transform.Find("back/back_1/juntuan_fight/getExp").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_388_118"),look_msg.zhanji.guild_exp);//[0AF6FF]军团在本轮获得军团经验：[0AFF16]{0}
		m_today_zhanji.transform.Find("back/back_1/player_fight/name").GetComponent<UILabel>().text = string.Format("{0}", game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) 
		                                                                                                                 + sys._instance.m_self.m_t_player.name);
		m_today_zhanji.transform.Find("back/back_1/player_fight/todayzhan").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_391_120"),look_msg.zhanji.zhanji);//[0AABFF]本轮个人战绩：[0AFF16]{0}
		m_today_zhanji.transform.Find("back/back_1/player_fight/totalzhan").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_392_120"),look_msg.zhanji.total_zhanji);//[0AABFF]个人总战绩：[0AFF16]{0}

		//军团头像
		m_today_zhanji.transform.Find("back/back_1/icon_kuang/icon").GetComponent<UISprite>().spriteName = game_data._instance.get_guild_icon (juntuan_gui._instance.m_guild_t.icon).icon;
		m_today_zhanji.transform.Find("back/back_1/icon_kuang").GetComponent<UISprite>().spriteName = juntuan_gui._instance.setKuang (juntuan_gui._instance.m_guild_t.level);
		//玩家头像
		GameObject _obj1 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count
		                                                             ,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
		_obj1.transform.parent = m_today_zhanji.transform.Find("back/back_1/icon").transform;
		_obj1.transform.localScale = new Vector3(1, 1, 1);
		_obj1.transform.localPosition = new Vector3(0, 0, 0);
		if (m_gamestat == 6)
		{
			m_today_zhanji.transform.Find("back/xiayichang").GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_405_105"), ReturnNextFight ());//[B3FE13]下一轮军团战时间: {0} 10:00 - 24:00
			m_today_zhanji.transform.Find("back/dijichang").GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("[0AF6FF]第{0}轮军团战已结束,战绩如下"), m_N);
		} 
		else if(m_gamestat == 3)
		{
			string t = ReturnNextFight ();
			m_today_zhanji.transform.Find("back/xiayichang").GetComponent<UILabel>().text = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_411_90");//[B3FE13]本轮军团战已全部结束
			m_today_zhanji.transform.Find("back/dijichang").GetComponent<UILabel>().text = string.Format (game_data._instance.get_t_language ("[0AF6FF]第{0}轮军团战已结束,战绩如下"), m_N);
		}

	}

	string ReturnNextFight()
	{
		string s = "";
		System.DateTime date_1 = timer.dtnow ();
		int temp = (int)date_1.DayOfWeek ;
		if (m_gamestat == 3)
		{
			m_N = 6;
			s ="";
			return s;
		}
		switch(temp)
		{
			case 3:
				s = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_431_8");//周三
				m_N = 1;
				break;
			case 4:
				s = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_435_8");//周四
				m_N = 2;
				break;
			case 5:
				s = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_439_8");//周五
				m_N = 3;
				break;
			case 6:
				s = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_443_8");//周六
				m_N = 4;
				break;
			case 7:
				m_N = 5;
				s = game_data._instance.get_t_language ("juntuan_kuafu_control.cs_448_8");//周日
				break;
			default:
				m_N = 6;
				s ="";
				break;

		}
		return s;
	}
	public void click(GameObject obj)
	{
		if (obj.name == "add_l" || obj.name == "left")
		{
			
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_index = 1;
//			m_button_c.GetComponent<UIToggle>().value = false;
//			m_button_l.GetComponent<UIToggle>().value = true;
//			m_button_m.GetComponent<UIToggle>().value = false;
//			m_button_r.GetComponent<UIToggle>().value = false;

			m_button_c.GetComponent<UIToggle>().Set(false);
			m_button_l.GetComponent<UIToggle>().Set(true);
			m_button_m.GetComponent<UIToggle>().Set(false);
			m_button_r.GetComponent<UIToggle>().Set(false);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems();
			
			m_guild_kuafu_set.SetActive (true);
			
		} 
		else if (obj.name == "add_r" || obj.name == "right")
		{
			
			
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_index = 2;
//			m_button_c.GetComponent<UIToggle>().value = false;
//			
//			m_button_l.GetComponent<UIToggle>().value = false;
//			
//			m_button_m.GetComponent<UIToggle>().value = false;
//			
//			m_button_r.GetComponent<UIToggle>().value = true;

			m_button_c.GetComponent<UIToggle>().Set(false);
			m_button_l.GetComponent<UIToggle>().Set(false);
			m_button_m.GetComponent<UIToggle>().Set(false);
			m_button_r.GetComponent<UIToggle>().Set(true);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems();
			
			m_guild_kuafu_set.SetActive (true);
			
			
			
		} 
		else if (obj.name == "add_m" || obj.name == "main")
		{
			
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_index = 3;
//			m_button_c.GetComponent<UIToggle>().value = false;
//
//			m_button_l.GetComponent<UIToggle>().value = false;
//			
//			m_button_m.GetComponent<UIToggle>().value = true;
//
//			m_button_r.GetComponent<UIToggle>().value = false;

			m_button_c.GetComponent<UIToggle>().Set(false);
			m_button_l.GetComponent<UIToggle>().Set(false);
			m_button_m.GetComponent<UIToggle>().Set(true);
			m_button_r.GetComponent<UIToggle>().Set(false);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems();
			m_guild_kuafu_set.SetActive (true);
			
			
		} 
		else if (obj.name == "add_c" || obj.name == "command")
		{
			
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_index = 4;

//			m_button_c.GetComponent<UIToggle>().value = true;
//
//			m_button_l.GetComponent<UIToggle>().value = false;		
//			
//			m_button_m.GetComponent<UIToggle>().value = false;
//
//			m_button_r.GetComponent<UIToggle>().value = false;	

			m_button_c.GetComponent<UIToggle>().Set(true);
			m_button_l.GetComponent<UIToggle>().Set(false);
			m_button_m.GetComponent<UIToggle>().Set(false);
			m_button_r.GetComponent<UIToggle>().Set(false);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems();
			m_guild_kuafu_set.SetActive (true);
			
		} 
		else if (obj.name == "yjpeizhi")
		{
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().yijianDef ();
		} 
		else if (obj.name == "tj")
		{
			m_guild_kuafu_shuoming.SetActive (true);
		} 
		else if (obj.name == "left_2")
		{

			m_button_c.GetComponent<UIToggle>().Set(false);
			m_button_l.GetComponent<UIToggle>().Set(true);
			m_button_m.GetComponent<UIToggle>().Set(false);
			m_button_r.GetComponent<UIToggle>().Set(false);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_index = 1;
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems ();
		} 
		else if (obj.name == "right_2")
		{
		
			m_button_c.GetComponent<UIToggle>().Set(false);
			m_button_l.GetComponent<UIToggle>().Set(false);
			m_button_m.GetComponent<UIToggle>().Set(false);
			m_button_r.GetComponent<UIToggle>().Set(true);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_index = 2;
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems ();
		} 
		else if (obj.name == "main_2")
		{

			m_button_c.GetComponent<UIToggle>().Set(false);
			m_button_l.GetComponent<UIToggle>().Set(false);
			m_button_m.GetComponent<UIToggle>().Set(true);
			m_button_r.GetComponent<UIToggle>().Set(false);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_index = 3;
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems ();
		} 
		else if (obj.name == "command_2")
		{
			m_button_c.GetComponent<UIToggle>().Set(true);
			m_button_l.GetComponent<UIToggle>().Set(false);
			m_button_m.GetComponent<UIToggle>().Set(false);
			m_button_r.GetComponent<UIToggle>().Set(false);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().m_index = 4;
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().juntuan_number_items.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
			m_guild_kuafu_set.GetComponent<juntuan_kuafu_info>().initItems ();
		} 
		else if (obj.name == "close_s")
		{
			m_guild_kuafu_shuoming.transform.Find("frame_big").GetComponent<frame>().hide ();
			if (juntuan_gui._instance.m_zhiwu_t != 2 && (m_gamestat != 4 && m_gamestat != 5 && m_gamestat != 6 && m_gamestat != 3))
			{
				m_guild_kuafu_tishi.SetActive (true);
			}
			return;
		}
		else if (obj.name == "close_p")
		{
			m_tishi_save.SetActive (true);
		} 
		else if(obj.name == "rankreward")
		{
			m_rank_panel.SetActive(true);
		}
		else if (obj.name == "close_rank")
		{
			m_rank_panel.transform.Find("frame_big").GetComponent<frame>().hide();
			m_rank_panel.GetComponent<guildfight_rank_gui>().m_first_toggle.value = true;




		}
		else if (obj.name == "close_reward")
		{
			m_gongporeward.transform.Find("frame_big").GetComponent<frame>().hide();     
		}
		else if (obj.name == "gongporeward")
		{
			m_gongporeward.SetActive(true);
			m_gongporeward.GetComponent<guildfight_target_gui>().reset();
			
		}
		else if (obj.name == "queding")
		{
			for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++) {
				juntuan_gui._instance.m_defend_type[i] = juntuan_gui._instance.m_defend_type_temp[i];
			}
			m_button_c.GetComponent<UIToggle>().startsActive = false;
			m_button_l.GetComponent<UIToggle>().startsActive = false;
			m_button_m.GetComponent<UIToggle>().startsActive = false;
			m_button_r.GetComponent<UIToggle>().startsActive = false;

			m_tishi_save.SetActive (false);
			m_guild_kuafu_set.transform.Find("frame_big").GetComponent<frame>().hide ();
			
			
			
		} 
		else if (obj.name == "quxiao")
		{
			
			m_tishi_save.transform.Find("frame_big").GetComponent<frame>().hide ();
			
		} 
		else if (obj.name == "off")
		{	
			m_button_c.GetComponent<UIToggle>().startsActive = false;
			m_button_l.GetComponent<UIToggle>().startsActive = false;
			m_button_m.GetComponent<UIToggle>().startsActive = false;
			m_button_r.GetComponent<UIToggle>().startsActive = false;
			m_guild_kuafu_set.transform.Find("frame_big").GetComponent<frame>().hide ();
			
		}
		else if (obj.name == "save")
		{
			protocol.game.cmsg_guild_pvp_bushu _msg = new protocol.game.cmsg_guild_pvp_bushu ();
			int temp = 0;
			List<ulong> msg_send = new List<ulong>();
			for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++)
			{
				if(juntuan_gui._instance.m_defend_type[i] == 0 ||juntuan_gui._instance. m_defend_type[i] == 5)
				{
					continue;
				}
				else if(juntuan_gui._instance.m_defend_type[i] == 1 )
				{
					temp++;
					_msg.player_guids.Add(juntuan_gui._instance.m_guild_member_t[i].player_guid);
				}
			}

			for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++)
			{
				if(juntuan_gui._instance.m_defend_type[i] == 0 ||juntuan_gui._instance. m_defend_type[i] == 5)
				{
					continue;
				}
				else if(juntuan_gui._instance.m_defend_type[i] == 2 )
				{
					temp++;
					_msg.player_guids.Add(juntuan_gui._instance.m_guild_member_t[i].player_guid);
				}
			}

			for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++)
			{
				if(juntuan_gui._instance.m_defend_type[i] == 0 ||juntuan_gui._instance. m_defend_type[i] == 5)
				{
					continue;
				}
				else if(juntuan_gui._instance.m_defend_type[i] == 3 )
				{
					temp++;
					_msg.player_guids.Add(juntuan_gui._instance.m_guild_member_t[i].player_guid);
				}
			}
			for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++)
			{
				if(juntuan_gui._instance.m_defend_type[i] == 0 ||juntuan_gui._instance. m_defend_type[i] == 5)
				{
					continue;
				}
				else if(juntuan_gui._instance.m_defend_type[i] == 4 )
				{
					temp++;
					_msg.player_guids.Add(juntuan_gui._instance.m_guild_member_t[i].player_guid);
				}
			}

			if(temp >= 22)
			{
				net_http._instance.send_msg<protocol.game.cmsg_guild_pvp_bushu> (opclient_t.CMSG_GUILD_JT_BUSHU, _msg);
			}
			else
			{
				root_gui._instance.show_prompt_dialog_box (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_732_47"));//[ffc882]必须完成所有据点的勾选才可以保存阵容
				return;
			}
		}
		else if (obj.name == "shuoming") 
		{
			m_guild_kuafu_tishi.transform.Find("frame_big").GetComponent<frame>().hide ();
			m_guild_kuafu_shuoming.SetActive (true);
		} 
		else if (obj.name == "baoming") 
		{

			
			
			if(juntuan_gui._instance. m_guild_t.level < 5)
			{
				root_gui._instance.show_prompt_dialog_box (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_748_47"));//[ffc882]军团需要达到5级才可以报名军团战
				return;
			}
//			else if(!sys._instance.is_can_kuafu_p())
//			{
//				root_gui._instance.show_prompt_dialog_box (game_data._instance.get_t_language ("juntuan_kuafu_control.cs_753_49"));//[ffc882]军团人数需要大于等于22人，且成员中不能包括加入军团未满24小时的人
//				return;
//			}
            if (!Application.isEditor)
            {
                System.DateTime date_1 = timer.dtnow();
                if (date_1.DayOfWeek == System.DayOfWeek.Tuesday)
                {
                    long now_timer3 = timer.datetimeconvertcuo(timer.dtnow().Date);
                    long star_fighttimer3 = now_timer3 + 10 * 60 * 60 * 1000;
                    if (star_fighttimer3 < (long)timer.now())
                    {
                        root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("juntuan_kuafu_control.cs_764_48"));//[ffc882]报名时间已结束，请重新进入军团战界面
                        return;
                    }
                }
            }
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_GIILD_JT_APPLY, _msg);
			
			
		}
		else if(obj.name == "guanbi")
		{
			m_today_zhanji.transform.Find("frame_big").GetComponent<frame>().hide();
		}

	}

	int ReturnTypeNum(int m_type)
	{
		int temp_num = 0;
		for (int i = 0; i < juntuan_gui._instance.m_defend_type.Count; i++)
		{
			if(juntuan_gui._instance.m_defend_type[i] == m_type)
			{
				temp_num++;
			}
		}
		return temp_num;
	}

	int compare(dhc.guild_member_t x, dhc.guild_member_t y)
	{
		int mX = x.bf;
		int mY = y.bf;
		if (mX > mY) 
		{
			return -1;
		}
		else
		{
			return 1;
		}
	}
	public void m_time()
	{
		System.DateTime date_1 = timer.dtnow ();
		switch (date_1.DayOfWeek)
		{
			case System.DayOfWeek.Monday: 
				
				long now_timer = timer.datetimeconvertcuo(timer.dtnow().Date);
				long star_fighttimer =now_timer + 34 * 60 * 60 * 1000;
				string strTimer = timer.get_time_show_color_ex(star_fighttimer -(long)timer.now(),"[FF160A]","[FF160A]");
				if(m_gamestat != 2 && m_gamestat != 3)
				{
					start_timer.GetComponent<UILabel>().text  = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_819_63"),strTimer);//[B3FE13]距离开战：{0}
				}
				else
				{
					start_timer.GetComponent<UILabel>().text  = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_823_63"));//[B3FE13]本周为休战周，下次军团战开启时间为下周二
				}
				break;

			default:
				long now_timer3 = timer.datetimeconvertcuo(timer.dtnow().Date);
				long star_fighttimer3 =now_timer3 + 10 * 60 * 60 * 1000;
				string strTimer3 = timer.get_time_show_color_ex(star_fighttimer3 -(long)timer.now(),"[FF160A]","[FF160A]");
				if(m_gamestat != 2 && m_gamestat != 3)
				{
					if(star_fighttimer3 <(long)timer.now())
					{
						start_timer.GetComponent<UILabel>().text  = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_835_64"));//[B3FE13]军团战进行中
					}
					else
					{
						start_timer.GetComponent<UILabel>().text  = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_819_63"),strTimer3);//[B3FE13]距离开战：{0}
					}
				}
				else
				{
					start_timer.GetComponent<UILabel>().text  = string.Format(game_data._instance.get_t_language ("juntuan_kuafu_control.cs_823_63"));//[B3FE13]本周为休战周，下次军团战开启时间为下周二
				}
				for (int i =0 ; i < 6; i++) 
				{
					int temp = (int)date_1.DayOfWeek - 2;
					if(temp < -1)
					{
						temp = 5; 
					}
					
					if(i < temp)
					{
						show_jt_jincheng[i].transform.Find("back_guo").gameObject.SetActive(true);
						show_jt_jincheng[i].transform.Find("back_now").gameObject.SetActive(false);
						show_jt_jincheng[i].transform.Find("back_wei").gameObject.SetActive(false);
					}
					else if(i > temp)
					{
						show_jt_jincheng[i].transform.Find("back_guo").gameObject.SetActive(false);
						show_jt_jincheng[i].transform.Find("back_now").gameObject.SetActive(false);
						show_jt_jincheng[i].transform.Find("back_wei").gameObject.SetActive(true);
					}
					else 
					{
						show_jt_jincheng[i].transform.Find("back_guo").gameObject.SetActive(false);
						show_jt_jincheng[i].transform.Find("back_now").gameObject.SetActive(true);
						show_jt_jincheng[i].transform.Find("back_wei").gameObject.SetActive(false);
					}
				}
				break;
		}
	}

}
