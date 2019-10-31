
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hb_taozhuang_gui : MonoBehaviour {

	public GameObject m_view;
	public List<s_t_role_dresstarget> data_taozhuang = new List<s_t_role_dresstarget>();
	public List<s_t_role_dresstarget> dress_target_done = new List<s_t_role_dresstarget>();
	public bool flag = false;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		data_taozhuang.Clear ();
		dbc m_dress_target = game_data._instance.m_dbc_role_dresstarget;
		foreach(int id in m_dress_target.m_index.Keys)
		{
			s_t_role_dresstarget temp = game_data._instance.get_t_role_dresstarget(id);
			data_taozhuang.Add(temp);
		}
		data_taozhuang.Sort (comp);
		for (int i = 0; i < data_taozhuang.Count; i++)
		{
			GameObject target = game_data._instance.ins_object_res("ui/hb_taozhuang_item");
			target.transform.parent = m_view.transform;
			target.transform.localPosition = new Vector3(-20, 142 - i* 160,0);
			target.transform.localScale = new Vector3(1,1,1);
			target.transform.GetComponent<hb_taozhuang_item>().m_dress = data_taozhuang[i];
			
			if(check_dress_target_done (data_taozhuang[i]))
			{
				target.transform.GetComponent<hb_taozhuang_item>().tarrget_finish = true;
			}
			else
			{
				target.transform.GetComponent<hb_taozhuang_item>().tarrget_finish = false;
			}
			target.transform.GetComponent<hb_taozhuang_item>().target_value = check_chengjiudone(data_taozhuang[i]);
			sys._instance.add_pos_anim (target,0.3f, new Vector3( -300, 0, 0), i * 0.05f);
			target.transform.GetComponent<hb_taozhuang_item>().reset ();
		}
		
	}

	public int comp(s_t_role_dresstarget x,s_t_role_dresstarget y)
	{
		bool x_b = check_dress_target_done (x);
		bool y_b = check_dress_target_done (y);
		if (x_b && !y_b)
			return 1;
		if (!x_b && y_b)
			return -1;
		return x.id - y.id;
		
	}

	public bool check_dress_target_done (s_t_role_dresstarget _dress_target)
	{
		int count = 0;
		List<int> temp = new List<int>();
		for(int i = 0; i < sys._instance.m_self.m_t_player.roles.Count;++i)
		{
			ulong guid = sys._instance.m_self.m_t_player.roles[i];
			ccard m_card = sys._instance.m_self.get_card_guid(guid);
			if(m_card != null)
			{
				for(int j = 0; j <  m_card.get_role().dress_ids.Count;++j)
				{
					temp.Add(m_card.get_role().dress_ids[j]);
				}
			}
		}
		for(int i = 0;i < _dress_target.ids.Count;i++)
		{
			s_t_role_dress t_role_dress = game_data._instance.get_t_role_dress(_dress_target.ids[i]);
			if (temp.Contains(_dress_target.ids[i]) || t_role_dress.hq_condition == 3)
				count++;
		}
		if (count == _dress_target.ids.Count)
		{
			return true;
		}
		return false;
		
	}

	public string check_chengjiudone(s_t_role_dresstarget t)
	{
		int count = 0;
		List<int> temp = new List<int>();
		for(int i = 0; i < sys._instance.m_self.m_t_player.roles.Count;++i)
		{
			ulong guid = sys._instance.m_self.m_t_player.roles[i];
			ccard m_card = sys._instance.m_self.get_card_guid(guid);
			if(m_card != null)
			{
				for(int j = 0; j < m_card.get_role().dress_ids.Count;++j)
				{
					temp.Add( m_card.get_role().dress_ids[j]);
				}
			}
		}
		for(int i = 0;i < t.ids.Count;i++)
		{
			s_t_role_dress t_role_dress = game_data._instance.get_t_role_dress(t.ids[i]);
			if (temp.Contains(t.ids[i]) || t_role_dress.hq_condition == 3)
				count++;
		}
		return count + "/" + t.ids.Count;
	}

	
	// Update is called once per frame
	void Update () {
	
	}
}
