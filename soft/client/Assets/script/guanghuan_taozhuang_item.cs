
using UnityEngine;
using System.Collections;

public class guanghuan_taozhuang_item : MonoBehaviour {

	public UILabel m_name;
	public GameObject m_icon_parent;
	public UILabel m_desc;
	public GameObject isfinshed;
	public GameObject progress;
	public GameObject m_mask;
	public int m_id;
	// Use this for initialization
	void Start () {
	
	}

	public void reset()
	{
		s_t_guanghuan_target _target = game_data._instance.get_t_guanghuan_target(m_id);
		m_name.text = _target.name;
		string s = "";
		for (int i = 0; i < _target.attrs.Count; i++)
		{
			if (i != 0)
			{
				s += "\n";
			}
			s += game_data._instance.get_value_string(_target.attrs[i].attr,_target.attrs[i].value,1);
		}
		m_desc.text = s;
		int num = 0;
		for(int i = 0; i < _target.ids.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.guanghuan.Contains(_target.ids[i]))
			{
				num ++;
			}
		}
		bool temp = sys._instance.m_self.is_guanghuan_finish(m_id);
		if(temp)
		{
			progress.SetActive(false);
			isfinshed.SetActive(true);
		}
		else
		{
			progress.SetActive(true);
			progress.GetComponent<UILabel>().text = game_data._instance.get_t_language ("guanghuan_taozhuang_item.cs_49_44") + ": " + num//已收集
				+ "/" + _target.ids.Count;
			isfinshed.SetActive(false);
		}
		sys._instance.remove_child(m_icon_parent);
		for (int i = 0; i < _target.ids.Count; i++)
		{
			GameObject obj = icon_manager._instance.create_guanghuan_icon(_target.ids[i],0);
			obj.transform.parent = m_icon_parent.transform;
			obj.transform.localPosition = new Vector3(-147 + 107*i, 20, 0);
			obj.transform.localScale = Vector3.one;
			if(!sys._instance.m_self.m_t_player.guanghuan.Contains(_target.ids[i]))
			{
				GameObject obj1 = (GameObject)Instantiate(m_mask);
				obj1.transform.parent = m_icon_parent.transform;
				obj1.transform.localPosition = obj.transform.localPosition;
				obj1.transform.localScale = Vector3.one;
				obj1.SetActive(true);
			}
		}

	}

}
