
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class huiyi_dialog_box : MonoBehaviour {

	public GameObject m_name_obj;
	public GameObject m_des_obj;
	public GameObject m_icon;
	public GameObject m_num;
	public GameObject m_jihuo_num;
	public GameObject m_huiyi;
	public GameObject m_huiyi_panel;

	public int item_id;

	public void OnEnable()
	{
		s_t_item t_item = game_data._instance.get_item (item_id);
		m_name_obj.GetComponent<UILabel>().text = sys._instance.m_self.get_name(2,item_id,0,0);
		m_des_obj.GetComponent<UILabel>().text = "[fff300]" + t_item.desc;
		int num = sys._instance.m_self.get_item_num ((uint)item_id);
		m_num.GetComponent<UILabel>().text = game_data._instance.get_t_language ("equip_buy_gui.cs_376_54") +" "+ num.ToString();//当前拥有：
		sys._instance.remove_child (m_icon);
		GameObject icon = icon_manager._instance.create_item_icon(item_id,true);
		Transform iicon = m_icon.transform;
		icon.transform.parent = iicon;
		icon.transform.localPosition = new Vector3(0,0,0);
		icon.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
		icon.GetComponent<BoxCollider>().enabled = false;
		List<int> _ids = new List<int>();
		
		for (int i = 0; i < game_data._instance.m_dbc_huiyi_sub.get_y(); i++)
		{
			int _id = int.Parse(game_data._instance.m_dbc_huiyi_sub.get(0, i));
			s_t_huiyi_sub huiyi_sub = game_data._instance.get_t_huiyi_sub(_id);
			for(int j = 0; j < huiyi_sub.huiyis.Count;++j)
			{
				if(huiyi_sub.huiyis[j] == item_id)
				{
					_ids.Add(_id);
					break;
				}
			}
		}
		int jihuo_num = 0;
		int huiyi_num = 0;
		int height = 24;
		string color = "";
		sys._instance.remove_child (m_huiyi_panel);
		GameObject item = Object.Instantiate (m_huiyi) as GameObject;
		item.transform.parent = m_huiyi_panel.transform;
		
        item.transform.localScale = new Vector3(1, 1, 1);
        item.transform.localPosition = new Vector3(-220 + 92 * 0, height, 0);
		for(int i = 0; i < _ids.Count;++i)
		{
			s_t_huiyi_sub t_huiyi_sub = game_data._instance.get_t_huiyi_sub(_ids[i]);
			if(sys._instance.m_self.is_huiyi_finish(_ids[i]) == 4 || sys._instance.m_self.is_huiyi_finish(_ids[i]) == 2|| sys._instance.m_self.is_huiyi_finish(_ids[i]) == 5)
			{
				color = "[FFFF00]";
				jihuo_num ++;
			}
			else
			{
				color = "[777777]";
			}
            item.GetComponent<UILabel>().text += color + t_huiyi_sub.name + "    ";
			huiyi_num++;
			if(huiyi_num == 5)
			{
				height -= 33;
				huiyi_num = 0;
			}
			item.SetActive(true);
		}
		m_jihuo_num.GetComponent<UILabel>().text = jihuo_num.ToString () + "/" + _ids.Count;
	}

	public void click(GameObject obj)
	{
		this.transform.GetComponent<ui_show_anim>().hide_ui ();
	}
}
