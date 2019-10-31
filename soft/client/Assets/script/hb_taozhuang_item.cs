
using UnityEngine;
using System.Collections;

public class hb_taozhuang_item : MonoBehaviour {

	public s_t_role_dresstarget m_dress;
	public GameObject name;
	public GameObject tiaojian;
	public GameObject attr;
	public GameObject isfinshed;
	public GameObject progress;
	public UISprite[] needdress;
	public bool tarrget_finish=false;
	public string target_value;
	public UIAtlas m_atlas;
	public UIAtlas m_atlas1;

	public void reset()
	{
		name.GetComponent<UILabel>().text = m_dress.name;
		tiaojian.GetComponent<UILabel>().text =  m_dress.desc.ToString();
		
		progress.GetComponent<UILabel>().text = game_data._instance.get_t_language ("guanghuan_taozhuang_item.cs_49_44") + ": " + target_value;//已收集
		progress.SetActive (false);
		
		for(int i=0;i<needdress.Length;i++)
		{
			needdress[i].gameObject.SetActive(false);
		}
		if (tarrget_finish)
		{
			string s = "";
			for(int i = 0;i < m_dress.attrs.Count;i++)
			{
				if(i == m_dress.attrs.Count -1)
				{
					s  += game_data._instance.get_value_string(m_dress.attrs[i].attr,m_dress.attrs[i].value,1);
				}
				else
				{
					s  += game_data._instance.get_value_string(m_dress.attrs[i].attr,m_dress.attrs[i].value,1) + "\n";
				}
			}
			attr.GetComponent<UILabel>().text = s;
			for(int i = 0;i < m_dress.ids.Count;i++)
			{
				if(m_atlas.GetListOfSprites().Contains(game_data._instance.get_t_role_dress (m_dress.ids[i]).icon))
				{
					needdress[i].transform.GetComponent<UISprite>().atlas = m_atlas;
				}
				else
				{
					needdress[i].transform.GetComponent<UISprite>().atlas = m_atlas1;
				}
				needdress[i].spriteName = game_data._instance.get_t_role_dress (m_dress.ids[i]).icon;
				needdress[i].GetComponent<dressrole_icon>().m_id = game_data._instance.get_t_role_dress (m_dress.ids[i]).id;
				needdress[i].gameObject.SetActive (true);
			}
			isfinshed.SetActive (true);
			progress.SetActive (false);
		}
		else
		{
			string s = "";
			for(int i = 0;i < m_dress.attrs.Count;i++)
			{
				if(i == m_dress.attrs.Count -1)
				{
					s  += game_data._instance.get_value_string(m_dress.attrs[i].attr,m_dress.attrs[i].value,1);
				}
				else
				{
					s  += game_data._instance.get_value_string(m_dress.attrs[i].attr,m_dress.attrs[i].value,1) + "\n";
				}
			}
			attr.GetComponent<UILabel>().text = s;
			for(int i = 0;i < m_dress.ids.Count;i++)
			{
				if(m_atlas.GetListOfSprites().Contains(game_data._instance.get_t_role_dress (m_dress.ids[i]).icon))
				{
					needdress[i].transform.GetComponent<UISprite>().atlas = m_atlas;
				}
				else
				{
					needdress[i].transform.GetComponent<UISprite>().atlas = m_atlas1;
				}
				needdress[i].spriteName = game_data._instance.get_t_role_dress (m_dress.ids[i]).icon;
				needdress[i].GetComponent<dressrole_icon>().m_id = game_data._instance.get_t_role_dress (m_dress.ids[i]).id;
				needdress[i].gameObject.SetActive (true);
			}
			progress.SetActive (true);
			isfinshed.SetActive (false);
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
