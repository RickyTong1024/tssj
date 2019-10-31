
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mubiao_mubiao_panel : MonoBehaviour, IMessage {

	public GameObject m_mubiao_panel;
	public GameObject m_view;
	int m_num;
	int m_type = 0;
	int m_click_id = 0;
	static List<int> m_done_ids = new List<int>();
    private Dictionary<int, bool> m_target_vis_dict = new Dictionary<int, bool>();

	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public static int check_target_done()
	{
		int idd = 0;
		dbc m_dbc_target = game_data._instance.m_dbc_target;
		for (int i = 0; i < m_dbc_target.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_target.get(0, i));
			s_t_target t_target = game_data._instance.get_t_target(id);
			if (target_done(t_target))
			{
				bool flag = false;
				for (int j = 0; j < m_done_ids.Count; ++j)
				{
					if (id == m_done_ids[j])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					m_done_ids.Add(id);
					idd = id;
				}
			}
		}
		return idd;
	}

	public int check_target_num()
	{
		int num = 0;
		dbc m_dbc_target = game_data._instance.m_dbc_target;
		for (int i = 0; i < m_dbc_target.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_target.get(0, i));
			s_t_target t_target = game_data._instance.get_t_target(id);
			if (!target_vis(t_target))
			{
				continue;
			}
			if (target_done(t_target))
			{
				num++;
			}
		}
		return num;
	}

	static int get_target_num(s_t_target t_target)
	{
		if (t_target.tjtype == 1)
		{
			return sys._instance.m_self.m_t_player.level;
		}
		else if (t_target.tjtype == 3)
		{
			int num = 0;
			for (int i = 0; i < sys._instance.m_self.m_t_player.role_template_ids.Count; ++i)
			{
				if (sys._instance.m_self.m_t_player.role_template_ids.Count >= t_target.tjdef2)
				{
					num++;
				}
			}
			return num;
		}
		else if (t_target.tjtype == 5)
		{
			if (sys._instance.m_self.m_t_player.mission >= t_target.tjdef1)
			{
				return 1;
			}
			return 0;
		}
		else if (t_target.tjtype == 6)
		{
			return sys._instance.m_self.m_t_player.pt_task_num;
		}
		else if (t_target.tjtype == 7)
		{
			return sys._instance.m_self.m_t_player.sj_task_num;
		}
		else if (t_target.tjtype == 8)
		{
			return sys._instance.m_self.m_t_player.jjie_task_num;
		}
		else if (t_target.tjtype == 9)
		{
			return sys._instance.m_self.m_t_player.qh_task_num;
		}
		else if (t_target.tjtype == 10)
		{
			return sys._instance.m_self.m_t_player.jj_task_num;
		}
		else if (t_target.tjtype == 11)
		{
			return sys._instance.m_self.m_t_player.boss_task_num;
		}
		else if (t_target.tjtype == 12)
		{
			return sys._instance.m_self.m_t_player.jy_task_num;
		}
		else if (t_target.tjtype == 13)
		{
			return sys._instance.m_self.m_t_player.hs_task_num;
		}
		else if (t_target.tjtype == 14)
		{
			return sys._instance.m_self.m_t_player.ttt_task_num;
		}
		else if (t_target.tjtype == 15)
		{
			if (sys._instance.m_self.m_t_player.mission_jy >= t_target.tjdef1)
			{
				return 1;
			}
			return 0;
		}
		else if (t_target.tjtype == 16)
		{
			int num = 0;
			for (int i =0;i < sys._instance.m_self.m_t_player.map_ids.Count;++i)
			{
				if( sys._instance.m_self.m_t_player.map_ids[i] < 10000)
				{
					num += sys._instance.m_self.m_t_player.map_star[i];
				}
			}
			return num;
		}
		else if (t_target.tjtype == 17)
		{
			int num = 0;
			for (int i =0;i < sys._instance.m_self.m_t_player.map_ids.Count;++i)
			{
				if( sys._instance.m_self.m_t_player.map_ids[i] >= 10000)
				{
					num += sys._instance.m_self.m_t_player.map_star[i];
				}
			}
			return num;
		}
		else if (t_target.tjtype == 18)
		{
			int num = sys._instance.m_self.m_t_player.bf;
			return num;
		}
		else if (t_target.tjtype == 19)
		{
			int num = 0;
			for (int i =0;i < sys._instance.m_self.m_t_player.ttt_last_stars.Count;++i)
			{
				num += sys._instance.m_self.m_t_player.ttt_last_stars[i];
			}
			return num;
		}
		return 0;
	}

	static bool target_done(s_t_target t_target)
	{
		if (get_target_num(t_target) >= t_target.tjnum)
		{
			return true;
		}
		return false;
	}

	static bool target_vis(s_t_target t_target)
	{

        if (sys._instance.m_self.m_t_player.finished_tasks.Contains((uint)t_target.id))
        {
            return false;
        }
		if (t_target.pid != 0)
		{
			bool flag = false;
            if (sys._instance.m_self.m_t_player.finished_tasks.Contains((uint)t_target.pid))
            {
                flag = true;
            }

			if (!flag)
			{
				return false;
			}
		}
		return true;
	}

	public static bool is_effect()
	{
		if (is_effect(1) || is_effect(2))
		{
			return true;
		}
		return false;
	}

	public static bool is_effect(int type)
	{
		dbc m_dbc_target = game_data._instance.m_dbc_target;
        foreach (int id in m_dbc_target.m_index.Keys)
		{
			s_t_target t_target = game_data._instance.get_t_target(id);
			if (t_target.type != type)
			{
				continue;
			}
			if (!target_vis(t_target))
			{
				continue;
			}
			if (target_done(t_target))
			{
				return true;
			}
		}
		
		return false;
	}

	public void reset(int type, int anim)
	{
		m_type = type;

		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}

		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);

		dbc m_dbc_target = game_data._instance.m_dbc_target;
		int tnum = 0;
		for (int i = 0; i < m_dbc_target.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_target.get(0, i));
			s_t_target t_target = game_data._instance.get_t_target(id);
			if (t_target.type != m_type)
			{
				continue;
			}
			if (!target_vis(t_target))
			{
				continue;
			}
			if (!target_done(t_target))
			{
				continue;
			}
			int num = get_target_num(t_target);

			GameObject target = game_data._instance.ins_object_res("ui/mubiao");
			target.transform.parent = m_view.transform;
			target.transform.localPosition = new Vector3(0, 109 - tnum * 109,0);
			target.transform.localScale = new Vector3(1,1,1);
			target.transform.GetComponent<mubiao>().m_mubiao_id = t_target.id;
			target.transform.GetComponent<mubiao>().m_num = num;
			target.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			target.transform.GetComponent<mubiao>().reset();

			if (anim == 1)
			{
				sys._instance.add_pos_anim(target,0.3f, new Vector3(-300, 0, 0), tnum * 0.05f);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, tnum * 0.05f);
			}
			else if (anim == 2)
			{
				sys._instance.add_pos_anim(target,0.3f, new Vector3(-300, 0, 0), tnum * 0.05f + 0.45f);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, tnum * 0.05f + 0.5f);
			}
			else
			{
				if (target.GetComponent<TweenPosition>() != null)
				{
					Object.Destroy(target.GetComponent<TweenPosition>());
				}
				if (target.GetComponent<TweenAlpha>() != null)
				{
					Object.Destroy(target.GetComponent<TweenAlpha>());
				}
				target.GetComponent<UIWidget>().alpha = 1.0f;
			}

			tnum++;
		}
		for (int i = 0; i < m_dbc_target.get_y(); ++i)
		{
			int id = int.Parse(m_dbc_target.get(0, i));
			s_t_target t_target = game_data._instance.get_t_target(id);
			if (t_target.type != m_type)
			{
				continue;
			}
			if (!target_vis(t_target))
			{
				continue;
			}
			if (target_done(t_target))
			{
				continue;
			}
			int num = get_target_num(t_target);

			GameObject target = game_data._instance.ins_object_res("ui/mubiao");
			target.transform.parent = m_view.transform;
			target.transform.localPosition = new Vector3(0, 109 - tnum * 109,0);
			target.transform.localScale = new Vector3(1,1,1);
			target.transform.GetComponent<mubiao>().m_mubiao_id = t_target.id;
			target.transform.GetComponent<mubiao>().m_num = num;
			target.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			target.transform.GetComponent<mubiao>().reset();

			if (anim == 1)
			{
				sys._instance.add_pos_anim(target,0.3f, new Vector3(-300, 0, 0), tnum * 0.05f);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, tnum * 0.05f);
			}
			else if (anim == 2)
			{
				sys._instance.add_pos_anim(target,0.3f, new Vector3(-300, 0, 0), tnum * 0.05f + 0.45f);
				sys._instance.add_alpha_anim(target,0.3f, 0, 1.0f, tnum * 0.05f + 0.5f);
			}
			else
			{
				if (target.GetComponent<TweenPosition>() != null)
				{
					Object.Destroy(target.GetComponent<TweenPosition>());
				}
				if (target.GetComponent<TweenAlpha>() != null)
				{
					Object.Destroy(target.GetComponent<TweenAlpha>());
				}
				target.GetComponent<UIWidget>().alpha = 1.0f;
			}

			tnum++;
		}
		m_mubiao_panel.GetComponent<mubiao_gui>().reset_button ();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void click(GameObject obj)
	{
		if (obj.name == "mubiao(Clone)")
		{
			m_click_id = obj.transform.GetComponent<mubiao>().m_mubiao_id;
			int num = obj.transform.GetComponent<mubiao>().m_num;
			s_t_target t_target = game_data._instance.get_t_target(m_click_id);
			if (num >= t_target.tjnum)
			{
				protocol.game.cmsg_player_task _msg = new protocol.game.cmsg_player_task ();
				_msg.task_id = (uint)m_click_id;
				net_http._instance.send_msg<protocol.game.cmsg_player_task> (opclient_t.CMSG_PLAYER_TASK, _msg);
			}
		}
	}

	void IMessage.message (s_message message)
	{
		
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_PLAYER_TASK)
		{
			sys._instance.m_self.m_t_player.finished_tasks.Add((uint)m_click_id);
			s_t_target t_target = game_data._instance.get_t_target(m_click_id);
			sys._instance.m_self.add_reward(t_target.reward.type, t_target.reward.value1, t_target.reward.value2, t_target.reward.value3,game_data._instance.get_t_language ("mubiao_mubiao_panel.cs_403_128"));//目标任务获得
			reset(m_type, 0);
		}
	}
}
