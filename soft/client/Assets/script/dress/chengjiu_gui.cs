using System.Collections.Generic;
using UnityEngine;

public class chengjiu_gui : MonoBehaviour{
	public GameObject m_view;
	public List<s_t_dress_target> data_chengjiu = new List<s_t_dress_target>();
	public List<s_t_dress_target> data_taozhuang = new List<s_t_dress_target>();
	public List<s_t_dress_target> dress_target_done = new List<s_t_dress_target>();
	public bool flag = false;
	void Start ()
	{

	}
	
	public void reset(int type)
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		data_chengjiu.Clear ();
		data_taozhuang.Clear ();
		dbc m_dress_target = game_data._instance.m_dbc_dress_target;
		for (int i = 0; i < m_dress_target.get_y(); ++i)
		{
			s_t_dress_target temp = game_data._instance.get_t_dress_target(int.Parse(m_dress_target.get(0,i)));
			if(temp.type == 1)
			{
				data_chengjiu.Add (temp);
			}
			else if(temp.type == 2)
			{
				data_taozhuang.Add(temp);
			}

		}
		data_chengjiu.Sort (comp);
		data_taozhuang.Sort (comp);
		if(type == 1)
		{
			for (int i = 0; i < data_chengjiu.Count; i++)
			{
				GameObject target = game_data._instance.ins_object_res("ui/dress_taozhuang_item");
				target.transform.parent = m_view.transform;
				target.transform.localPosition = new Vector3(-20, 127 - i* 133,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.transform.GetComponent<chengjiu_item>().m_dress = data_chengjiu[i];
				
				if(check_dress_target_done (data_chengjiu[i]))
				{
					target.transform.GetComponent<chengjiu_item>().tarrget_finish = true;
				}
				else
				{
					target.transform.GetComponent<chengjiu_item>().tarrget_finish = false;
				}
				target.transform.GetComponent<chengjiu_item>().target_value = check_chengjiudone(data_chengjiu[i]);
				sys._instance.add_pos_anim (target,0.3f, new Vector3( -300, 0, 0), i * 0.05f);
				//sys._instance.add_alpha_anim (target,0.3f, 0, 1.0f, i * 0.05f);
				target.transform.GetComponent<chengjiu_item>().reset ();
			}

		}
		else if(type == 2)
		{
			for (int i = 0; i < data_taozhuang.Count; i++)
			{
				GameObject target = game_data._instance.ins_object_res("ui/dress_item");
				target.transform.parent = m_view.transform;
				target.transform.localPosition = new Vector3(-20, 142 - i* 160,0);
				target.transform.localScale = new Vector3(1,1,1);
				target.transform.GetComponent<chengjiu_item>().m_dress = data_taozhuang[i];
				
				if(check_dress_target_done (data_taozhuang[i]))
				{
					target.transform.GetComponent<chengjiu_item>().tarrget_finish = true;
				}
				else
				{
					target.transform.GetComponent<chengjiu_item>().tarrget_finish = false;
				}
				target.transform.GetComponent<chengjiu_item>().target_value = check_chengjiudone(data_taozhuang[i]);
				sys._instance.add_pos_anim (target,0.3f, new Vector3( -300, 0, 0), i * 0.05f);
				//sys._instance.add_alpha_anim (target,0.3f, 0, 1.0f, i * 0.05f);
				target.transform.GetComponent<chengjiu_item>().reset ();
			}
		}

	}
	public int comp(s_t_dress_target x,s_t_dress_target y)
	{
		bool x_b = check_dress_target_done (x);
		bool y_b = check_dress_target_done (y);
		if (x_b && !y_b)
			return 1;
		if (!x_b && y_b)
			return -1;
		return x.id - y.id;

	}

	public string check_chengjiudone(s_t_dress_target t)
	{
		if (t.type == 1) 
		{
			return sys._instance.m_self.m_t_player.dress_ids.Count + "/"+t.defs[0];
		}
		else if(t.type == 2)
		{
			int count = 0;
			List<int> temp = sys._instance.m_self.m_t_player.dress_ids;
			for(int i = 0;i < temp.Count;i++)
			{
				for(int j = 0;j < t.defs.Count;j++)
				{
					if (temp[i] == t.defs[j])
						count++;
				}
				
			}
			return count + "/" + t.defs.Count;
		}
		return "";
	}
	public bool check_dress_target_done (s_t_dress_target _dress_target)
	{
		if (_dress_target.type == 1) 
		{
			for( int i = 0; i <sys._instance.m_self.m_t_player.dress_achieves.Count; ++i)
			{
				if(_dress_target.id == sys._instance.m_self.m_t_player.dress_achieves[i] )
				{
					return true;
				}
			}
		}
		if (_dress_target.type == 2)
		{
			int count = 0;
			List<int> temp = sys._instance.m_self.m_t_player.dress_ids;
			for(int i = 0;i < temp.Count;i++)
			{
				for(int j = 0;j < _dress_target.defs.Count;j++)
				{
					if (temp[i] == _dress_target.defs[j])
						count++;
				}

			}
			if (count == _dress_target.defs.Count)
			{
				return true;
			}
		}
		return false;

	}
	public int check_dress_target_done ()
	{
		List<s_t_dress_target> temp_dress = new List<s_t_dress_target>();
		dbc m_dress_target = game_data._instance.m_dbc_dress_target;
		for (int i = 0; i < m_dress_target.get_y(); ++i)
		{
			s_t_dress_target temp = game_data._instance.get_t_dress_target (int.Parse (m_dress_target.get (0, i)));

			if (check_dress_target_done (temp))
			{
				temp_dress.Add (temp);
			}

		}
		if (flag == false)
		{
			flag = true;
			dress_target_done = temp_dress;
			return 0;
		}
		else
		{
			for(int i = 0;i < temp_dress.Count;i++)
			{
				if (!ison_dresstarget(temp_dress[i].id))
				{
					dress_target_done = temp_dress;
					return temp_dress[i].id;
				}
			}
		}
		return 0;
	}
	bool ison_dresstarget(int id)
	{
		for (int i = 0; i<dress_target_done.Count; i++)
		{
			if(id == dress_target_done[i].id)
			{
				return true; 
			}
		}
		return false;
	}
}
