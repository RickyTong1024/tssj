
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class guanghuan_tj_gui : MonoBehaviour {

	public GameObject m_tj_scro;

	// Use this for initialization
	void Start () {
	
	}

	public void reset_tj()
	{
		sys._instance.remove_child(m_tj_scro);
		m_tj_scro.transform.localPosition = new Vector3(0, 0, 0);
		if (m_tj_scro.GetComponent<SpringPanel>() != null)
		{
			m_tj_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_tj_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		List<int> _ids = new List<int>();
		foreach (int id in game_data._instance.m_dbc_guanghuan_target.m_index.Keys)
		{
			_ids.Add(id);
		}
		_ids.Sort(compare);
		
		for (int i = 0; i < _ids.Count; i++)
		{
			GameObject obj = game_data._instance.ins_object_res("ui/guanghuan_taozhuang_item");
			obj.transform.parent = m_tj_scro.transform;
			obj.transform.localPosition = new Vector3(0, 102 - 160 * i, 0);
			obj.transform.localScale = Vector3.one;
			obj.GetComponent<guanghuan_taozhuang_item>().m_id = _ids[i];
			obj.GetComponent<guanghuan_taozhuang_item>().reset();
		}
	}

	int compare(int x, int y)
	{
		bool  flag_x = sys._instance.m_self.is_guanghuan_finish(x);
		bool flag_y = sys._instance.m_self.is_guanghuan_finish(y);
		
		if (!flag_x && flag_y)
		{
			return -1;
		}
		else if(flag_x && !flag_y)
		{
			return 1;
		}
		else
		{
			return x - y;
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "tj_close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
