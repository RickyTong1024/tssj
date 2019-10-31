using UnityEngine;

public class chengjiu_item : MonoBehaviour {

	public s_t_dress_target m_dress;
	public GameObject name;
	public GameObject tiaojian;
	public GameObject attr;
	public GameObject isfinshed;
	public GameObject progress;
	public UISprite[] needdress;
	public bool tarrget_finish=false;
	public string target_value;
	public void reset()
	{
		name.GetComponent<UILabel>().text = m_dress.name;
		tiaojian.GetComponent<UILabel>().text =  m_dress.desc.ToString();

        progress.GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_detail_gui_soft10") + ": " + target_value;//已收集
		progress.SetActive (false);
	
		for(int i=0;i<needdress.Length;i++)
		{
			needdress[i].gameObject.SetActive(false);
		}
		if (tarrget_finish)
		{
			if(m_dress.value2 != 0)
			{
				attr.GetComponent<UILabel>().text =  "[00ffff]" + string.Format (game_data._instance.get_value_string (m_dress.attr1, m_dress.value1,1)) 
					+ "[-]\n[ffff00]" + string.Format (game_data._instance.get_value_string (m_dress.attr2, m_dress.value2,1));
			}
			else
			{
				attr.GetComponent<UILabel>().text =  "[00ffff]" + string.Format (game_data._instance.get_value_string (m_dress.attr1, m_dress.value1,1)) ;

			}
			if(m_dress.type == 2)
			{
				for(int i = 0;i < m_dress.defs.Count;i++)
				{
					needdress[i].spriteName = game_data._instance.get_t_dress (m_dress.defs[i]).icon;
					needdress[i].GetComponent<dress_icon>().m_t_dress = game_data._instance.get_t_dress (m_dress.defs[i]);
					needdress[i].gameObject.SetActive (true);
					needdress[i].transform.Find("icon").GetComponent<UISprite>().spriteName = icon_name(game_data._instance.get_t_dress (m_dress.defs[i]));
				}
			}
			isfinshed.SetActive (true);
			progress.SetActive (false);
		}
		else
		{
			if(m_dress.value2 != 0)
			{
				attr.GetComponent<UILabel>().text =  "[00ffff]" + string.Format (game_data._instance.get_value_string (m_dress.attr1, m_dress.value1,1)) 
					+ "[-]\n[ffff00]" + string.Format (game_data._instance.get_value_string (m_dress.attr2, m_dress.value2,1));
			}
			else
			{
				attr.GetComponent<UILabel>().text =  "[00ffff]" + string.Format (game_data._instance.get_value_string (m_dress.attr1, m_dress.value1,1)) ;
				
			}

		
			if(m_dress.type == 2)
			{
				for(int i = 0;i < m_dress.defs.Count;i++)
				{
					needdress[i].spriteName = game_data._instance.get_t_dress (m_dress.defs[i]).icon;
					needdress[i].GetComponent<dress_icon>().m_t_dress = game_data._instance.get_t_dress (m_dress.defs[i]);
					needdress[i].gameObject.SetActive (true);
					needdress[i].transform.Find("icon").GetComponent<UISprite>().spriteName = icon_name(game_data._instance.get_t_dress (m_dress.defs[i]));
				}
			}
			progress.SetActive (true);
			isfinshed.SetActive (false);
		}
	}

	public static string icon_name(s_t_dress t_dress)
	{
		if(t_dress.color == 2)
		{
			return "nszjm_szblz04";
		}
		if(t_dress.color == 3)
		{
			return "nszjm_szblz06";
		}
		if(t_dress.color == 4)
		{
			return "nszjm_szblz05";
		}
		return "";
	}
}
