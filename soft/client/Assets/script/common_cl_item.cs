
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class common_cl_item : MonoBehaviour {

	public int m_mission_id;
	public int m_rate;
	public GameObject m_button;
	public GameObject m_lock;

	// Use this for initialization
	void Start () {


		init ();
	}

	public void init()
	{
		s_t_mission t_mission = game_data._instance.get_t_mission (m_mission_id);
		string s = "(" + game_data._instance.get_t_language ("common_cl_item.cs_22_19") + ")";//掉率:极高
		if (m_rate <= 5)
		{
			s = "("+game_data._instance.get_t_language ("common_cl_item.cs_25_11") + ")";//掉率：极低
		}
		else if (m_rate <= 10)
		{
			s = "("+game_data._instance.get_t_language ("common_cl_item.cs_29_11")+")";//掉率：低
		}
		else if (m_rate <= 20)
		{
			s = "("+game_data._instance.get_t_language ("common_cl_item.cs_33_11")+")";//掉率：中
		}
		else if (m_rate <= 40)
		{
			s = "("+game_data._instance.get_t_language ("common_cl_item.cs_37_11")+")";//掉率：高
		}
		this.transform.Find("dl").GetComponent<UILabel>().text = s;

		s_t_mission _mission = game_data._instance.get_t_mission (m_mission_id);
		
		if(sys._instance.m_self.m_t_player.mission < _mission.lock_id 
		   || sys._instance.m_self.m_t_player.mission_jy < _mission.jylock_id
		   || sys._instance.m_self.m_t_player.level < game_data._instance.get_t_map(_mission.map_id).level)
		{
			m_button.SetActive(false);
			m_lock.SetActive(true);
			this.transform.Find("name").GetComponent<UILabel>().text = t_mission.name + " ("  + string.Format(game_data._instance.get_t_language ("common_cl_item.cs_49_106"),t_mission.day_num);//0/{0}次)
		}
		else
		{
			int ci = t_mission.day_num - sys._instance.m_self.get_mission_cishu(t_mission.id);
			this.transform.Find("name").GetComponent<UILabel>().text = t_mission.name + " " + string.Format(game_data._instance.get_t_language ("common_cl_item.cs_54_104"), ci, t_mission.day_num);//({0}/{1}次)
			m_button.SetActive(true);
			m_lock.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void click(GameObject obj)
	{
		//this.gameObject.SetActive (false);

		s_message _message = new s_message ();
		_message.m_type = "show_fu_ben";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_tupo_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_buzheng";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_jinjie_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_huoban";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_skill_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_equip_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_cl_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message();
		_message.m_type = "hide_unit_show";
		cmessage_center._instance.add_message(_message);

		_message = new s_message ();
		_message.m_type = "hide_show_unit";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "scene_show";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_bag_gui";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_dui_xing";
		cmessage_center._instance.add_message (_message);

		_message = new s_message ();
		_message.m_type = "hide_pet_jj_gui";
		cmessage_center._instance.add_message (_message);

		s_t_mission _mission = game_data._instance.get_t_mission (m_mission_id);

		_message = new s_message ();
		_message.time = 0.1f;
		_message.m_type = "select_map";
		_message.m_ints.Add (_mission.map_id);
		_message.m_ints.Add (m_mission_id);
		cmessage_center._instance.add_message (_message);
	}
}
