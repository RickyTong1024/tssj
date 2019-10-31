
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class common_cl_panel : MonoBehaviour {

	public int m_id;
	public GameObject m_view;
	public GameObject m_num;
	public GameObject m_cc;
	public int m_cid;


	// Use this for initialization
	void Start () 
	{

	}

	public bool can_item_other(string[] temp, string s)
	{
		for(int i = 0;i < temp.Length;i ++)
		{
			if(temp[i] == s)
			{
				return true;
			}
		}
		return false;
	}

	public void init()
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view.gameObject);
		s_t_guanghuan t_guanghuan = game_data._instance.get_t_guanghuan (m_id);
		if(t_guanghuan != null)
		{
			m_cc.SetActive(false);
			m_num.gameObject.SetActive(false);
			this.transform.Find("back").Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(t_guanghuan.color) + 
				t_guanghuan.name;
			sys._instance.remove_child (this.transform.Find("back").Find("icon").gameObject);
			GameObject icon = icon_manager._instance.create_guanghuan_icon (m_id,0);
			icon.transform.parent = this.transform.Find("back").Find("icon");
			icon.transform.localPosition = new Vector3 (0, 0, 0);
			icon.transform.localScale = new Vector3 (1, 1, 1);
			icon.transform.GetComponent<BoxCollider>().enabled = false;
			if(t_guanghuan.color == 4 || t_guanghuan.color == 5)
			{
				GameObject target = game_data._instance.ins_object_res("ui/common_guanghuan_other");
				target.transform.parent = m_view.transform;
				target.transform.localPosition = new Vector3(0, 41,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.transform.Find("go").GetComponent<UIButtonMessage>().target = this.gameObject;
				target.SetActive(true);
				sys._instance.add_pos_anim(target,0.3f, new Vector3(-200, 0, 0), 0);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, 0);


                GameObject target2 = game_data._instance.ins_object_res("ui/common_cl_item_other");
                target2.transform.parent = m_view.transform;
                target2.transform.localPosition = new Vector3(0, -69, 0);
                target2.transform.localScale = new Vector3(1, 1, 1);
                target2.transform.GetComponent<common_cl_item_other>().m_id = m_id;
                target2.transform.GetComponent<common_cl_item_other>().type = 1001;
                target2.transform.GetComponent<common_cl_item_other>().init();
                target2.SetActive(true);
                sys._instance.add_pos_anim(target2, 0.3f, new Vector3(-200, 0, 0), 0);
                sys._instance.add_alpha_anim(target2, 0.3f, 0, 1.0f, 0);
			}
			else
			{
				GameObject target = game_data._instance.ins_object_res("ui/common_cl_item_other");
				target.transform.parent = m_view.transform;
				target.transform.localPosition = new Vector3(0, 41,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.transform.GetComponent<common_cl_item_other>().m_id = m_id;
				target.transform.GetComponent<common_cl_item_other>().type = 3;
				target.transform.GetComponent<common_cl_item_other>().init();
				target.SetActive(true);
				sys._instance.add_pos_anim(target,0.3f, new Vector3(-200, 0, 0), 0);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, 0);
			}
			return;
		}
		m_num.gameObject.SetActive(true);
		dbc m_dbc_mission = game_data._instance.m_dbc_mission;
		int num = 0;
		s_t_item t_item = game_data._instance.get_item (m_id);
		string[] temp = t_item.out_put.Split ('|');
		if(!can_item_other(temp,"0"))
		{
			for(int i = 0;i < temp.Length;i ++)
			{
				if(temp[i] == "12")
				{
					continue;
				}
				GameObject target = game_data._instance.ins_object_res("ui/common_cl_item_other");
				target.transform.parent = m_view.transform;
				target.transform.localPosition = new Vector3(0, 41 - num * 110,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.transform.GetComponent<common_cl_item_other>().m_id = m_id;
				target.transform.GetComponent<common_cl_item_other>().type = int.Parse(temp[i]);
				target.transform.GetComponent<common_cl_item_other>().init();
				target.SetActive(true);
				sys._instance.add_pos_anim(target,0.3f, new Vector3(-200, 0, 0), num * 0.05f);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, num * 0.05f);
				
				num++;
			}
		}
		if(can_item_other(temp,"12"))
		{
			for (int i = 0; i < m_dbc_mission.get_y(); i ++)
			{
				s_t_mission t_mission = game_data._instance.get_t_mission(int.Parse(game_data._instance.m_dbc_mission.get(0,i)));
				if (t_mission.type > 3)
				{
					continue;
				}
				bool flag = false;
				int rate = 0;
				for (int j = 0; j < t_mission.items.Count; ++j)
				{
					if (t_mission.items[j].reward.type == 2 && t_mission.items[j].reward.value1 == m_id)
					{
						flag = true;
						rate = t_mission.items[j].rate;
						break;
					}
				}
				if (!flag)
				{
					continue;
				}

				GameObject target = game_data._instance.ins_object_res("ui/common_cl_item");
				target.transform.parent = m_view.transform;
				target.transform.localPosition = new Vector3(0, 41 - num * 110,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.transform.GetComponent<common_cl_item>().m_mission_id = t_mission.id;
				target.transform.GetComponent<common_cl_item>().m_rate = rate;
				target.SetActive(true);

				sys._instance.add_pos_anim(target,0.3f, new Vector3(-200, 0, 0), num * 0.05f);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, num * 0.05f);

				num++;
			}
		}

		this.transform.Find("back").Find("name").GetComponent<UILabel>().text = t_item.name;
		if (m_cid > 0)
		{
			s_t_class t_class = game_data._instance.get_t_class (m_cid);
			this.transform.Find("back").Find("name").GetComponent<UILabel>().text = ccard.get_color_name(t_class.name, t_class.color);
		}
		m_num.GetComponent<UILabel>().text =  string.Format(game_data._instance.get_t_language ("common_cl_panel.cs_153_54"), sys._instance.m_self.get_item_num((uint)t_item.id).ToString());//[0af6ff]已收集[-][0aff16]{0}个[-]

		sys._instance.remove_child (this.transform.Find("back").Find("icon").gameObject);
		GameObject _icon = icon_manager._instance.create_item_icon (m_id);
		_icon.transform.parent = this.transform.Find("back").Find("icon");
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.transform.GetComponent<BoxCollider>().enabled = false;

		if (num == 0)
		{
			m_cc.SetActive(true);
		}
		else
		{
			m_cc.SetActive(false);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void select(GameObject obj)
	{
		root_gui._instance.show_prompt_dialog_box (game_data._instance.get_t_language ("common_cl_panel.cs_179_45"));//[ffc882]敬请关注近期活动
		sys._instance.message_clear ();
	}

	public void click(GameObject obj)
	{
		transform.Find("frame_big").GetComponent<frame>().hide();
		sys._instance.message_clear ();
	}
}
