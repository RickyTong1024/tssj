
using UnityEngine;
using System.Collections;

public class zs_skill_sub : MonoBehaviour {

	public s_t_role_skillunlock_task t_role_skillunlock_task;
	public ccard m_card;
	public UILabel m_task;
	public UILabel m_jindu;
	public GameObject m_dacheng;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		string renwu = "";
		int num = 0;
		int achieve_num = 0;
		switch(t_role_skillunlock_task.task_type)
		{
		case 1:
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_24_25"),t_role_skillunlock_task.def1.ToString());//上阵并获得{0}次普通副本胜利
			num = t_role_skillunlock_task.def1;
			break;
		case 2:
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_28_25"),t_role_skillunlock_task.def1.ToString());//上阵并获得{0}次精英副本胜利
			num = t_role_skillunlock_task.def1;
			break;
		case 3:
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_32_25"),t_role_skillunlock_task.def1.ToString());//上阵并获得{0}次竞技场胜利
			num = t_role_skillunlock_task.def1;
			break;
		case 4:
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_36_25"),t_role_skillunlock_task.def1.ToString());;//上阵并夺宝{0}次
			num = t_role_skillunlock_task.def1;
			break;
		case 5:
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_40_25"),t_role_skillunlock_task.def1.ToString());//上阵并在星河秘境获得{0}星
			num = t_role_skillunlock_task.def1;
			break;
		case 6:
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_44_25"),t_role_skillunlock_task.def1.ToString());//等级达到{0}级
			num = t_role_skillunlock_task.def1;
			achieve_num = m_card.get_level();
			break;
		case 7:
			s_t_jinjie t_jinjie = game_data._instance.get_jinjie(t_role_skillunlock_task.def1);
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_50_25"),t_jinjie.name);//阶级达到{0}
			num = t_role_skillunlock_task.def1;
			achieve_num = m_card.get_jlevel();
			break;
		case 8:
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_55_25"),t_role_skillunlock_task.def1.ToString());//突破等级达到{0}级
			num = t_role_skillunlock_task.def1;
			achieve_num = m_card.get_glevel();
			break;
		case 9:
			s_t_role_shengpin t_shengpin = game_data._instance.get_t_role_shengpin(t_role_skillunlock_task.def1);
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_61_25"),t_shengpin.name);//品级达到{0}
			num = 1;
			if(m_card.get_pinzhi() >= t_role_skillunlock_task.def1)
			{
				achieve_num = 1;
			}
			break;
		case 10:
			string name = "";
			if(t_role_skillunlock_task.def1 == 0)
			{
				name = m_card.m_skills[t_role_skillunlock_task.def1+1].m_t_skill.name;
			}
			else
			{
				name = m_card.m_skills[t_role_skillunlock_task.def1+2].m_t_skill.name;
			}
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_78_25"),name,t_role_skillunlock_task.def2.ToString());//{0}技能达到{1}级 
			num = t_role_skillunlock_task.def2;
			achieve_num = m_card.get_role().jskill_level[t_role_skillunlock_task.def1];
			break;
		case 11:
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_83_25"),t_role_skillunlock_task.def1.ToString());//激活{0}条相关的回忆录 
			num = t_role_skillunlock_task.def1;
			int item_id = 0;
			foreach(int id in game_data._instance.m_dbc_item.m_index.Keys)
			{
				s_t_item t_item = game_data._instance.get_item(id);
				if(t_item.type == 9001 && t_item.def_1 == m_card.m_t_class.id)
				{
					item_id=t_item.id;
					break;
				}
			}
			for(int i = 0; i < sys._instance.m_self.m_t_player.huiyi_jihuos.Count;++i)
			{
				s_t_huiyi_sub _sub = game_data._instance.get_t_huiyi_sub(sys._instance.m_self.m_t_player.huiyi_jihuos[i]);
				if(_sub.huiyis.Contains(item_id))
				{
					achieve_num++;
				}
			}
			break;
		case 12:
			s_t_role_dress t_role_dress = game_data._instance.get_t_role_dress(t_role_skillunlock_task.def1);
			renwu = string.Format(game_data._instance.get_t_language ("zs_skill_sub.cs_106_25"),t_role_dress.name);//拥有{0}时装
			num = 1;
			achieve_num = 0;
			for(int i = 0; i < m_card.get_role().dress_ids.Count;++i)
			{
				if(m_card.get_role().dress_ids[i] == t_role_skillunlock_task.def1)
				{
					achieve_num = 1;
					break;
				}
			}
			break;
		}
		m_task.text = renwu;
		if(t_role_skillunlock_task.task_type <= 5)
		{
			achieve_num = m_card.get_role().bskill_counts[t_role_skillunlock_task.task_type -1];
		}
		m_jindu.text = achieve_num.ToString () + "/" + num.ToString ();
		if(achieve_num >= num)
		{
			m_dacheng.GetComponent<UILabel>().text = game_data._instance.get_t_language ("zs_skill_sub.cs_127_44");//[0af6ff]已完成[-]
			m_dacheng.GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			m_dacheng.GetComponent<UILabel>().text = game_data._instance.get_t_language ("zs_skill_sub.cs_132_44");//[f8cf40]前往[-]
			m_dacheng.GetComponent<BoxCollider>().enabled = true;
		}
	}

	public void click(GameObject obj)
	{
		s_message _mes = new s_message();
		s_message _mes1 = new s_message();
		s_message _mes2 = new s_message();
		s_message _mes3 = new s_message();
		switch(t_role_skillunlock_task.task_type)
		{
		case 1:
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
			
			_mes2.m_type = "hide_buzheng";
			cmessage_center._instance.add_message(_mes2);

			_mes3.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes3);
			break;
		case 2:
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jy)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("huo_yue_sub.cs_242_74"),(int)e_open_level.el_jy ));//精英副本{0}级开启
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
			
			_mes2.m_type = "hide_buzheng";
			cmessage_center._instance.add_message(_mes2);

			_mes3.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes3);
			break;
		case 3:
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_jjc)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_236_74"), (int)e_open_level.el_jjc));//竞技场{0}级开启
				return;
			}
			_mes2.m_type = "hide_buzheng";
			cmessage_center._instance.add_message(_mes2);

			_mes3.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes3);
			_mes.m_type = "show_huo_dong";
			_mes.m_ints.Add(6);
			cmessage_center._instance.add_message(_mes);

			break;
		case 4:
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_treasure_qu)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_340_74") , (int)e_open_level.el_treasure_qu  ));//夺宝奇兵{0}级开启
				return;
			}
			_mes2.m_type = "hide_buzheng";
			cmessage_center._instance.add_message(_mes2);
			_mes3.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes3);
			_mes.m_type = "show_huo_dong";
			_mes.m_ints.Add(9);
			cmessage_center._instance.add_message(_mes);
			break;
		case 5:
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_mijing)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]"+ string.Format(game_data._instance.get_t_language ("active.cs_275_73") , (int)e_open_level.el_mijing ));//星河秘境{0}级开启
				return;
			}
			_mes2.m_type = "hide_buzheng";
			cmessage_center._instance.add_message(_mes2);
			_mes3.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes3);
			_mes.m_type = "show_huo_dong";
			_mes.m_ints.Add(5);
			cmessage_center._instance.add_message(_mes);
			break;
		case 6:
			_mes2.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes2);
			
			_mes.m_type = "show_cn_gui";
			_mes.m_long.Add(m_card.get_guid());
			cmessage_center._instance.add_message(_mes);
			
			break;
		case 7:
			_mes2.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes2);
			
			_mes.m_type = "show_jj_gui";
			_mes.m_long.Add(m_card.get_guid());
			cmessage_center._instance.add_message(_mes);
			break;
		case 8:
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_tupo)
			{
				string str = game_data._instance.get_t_language ("bu_zheng_panel.cs_325_17");//突破{0}级开启
				str = string.Format(str,((int)e_open_level.el_tupo).ToString());
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
				return;
			}
			_mes2.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes2);
			
			_mes.m_type = "show_tu_po_gui";
			_mes.m_long.Add(m_card.get_guid());
			cmessage_center._instance.add_message(_mes);
			break;
		case 9:
			_mes2.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes2);
			
			_mes.m_type = "show_sheng_pin_gui_ex";
			_mes.m_long.Add(m_card.get_guid());
			cmessage_center._instance.add_message(_mes);
			break;
		case 10:
			if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_skill)
			{
				string str = game_data._instance.get_t_language ("bu_zheng_panel.cs_375_17");//技能{0}级开启
				str = string.Format(str,((int)e_open_level.el_tupo).ToString());
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + str);
				return;
			}
			_mes2.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes2);
			
			_mes.m_type = "show_jn_gui";
			_mes.m_long.Add(m_card.get_guid());
			cmessage_center._instance.add_message(_mes);
			break;
		case 11:
			if (sys._instance.m_self.m_t_player.level < (int)e_open_level.el_memory)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("active.cs_381_89"),(int)e_open_level.el_memory));//回忆{0}级开启
				return;
			}
			_mes2.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes2);

			_mes3.m_type = "hide_buzheng";
			cmessage_center._instance.add_message(_mes3);
			_mes.m_type = "show_memory_jihuo";
			cmessage_center._instance.add_message(_mes);
			break;
		case 12:
			_mes2.m_type = "hide_skill_gui";
			cmessage_center._instance.add_message(_mes2);
			_mes3.m_type = "hide_buzheng_ex";
			cmessage_center._instance.add_message(_mes3);
			_mes.m_type = "show_hb_dress_gui";
			_mes.m_long.Add(m_card.get_guid());
			_mes.m_string.Add("show_bu_zheng");
			cmessage_center._instance.add_message(_mes);
			break;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
