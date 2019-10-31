
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class treasure_page_item : MonoBehaviour {

	public GameObject m_icon_root;
	public GameObject m_name;
	public GameObject m_pro;
	private GameObject m_icon;
	public GameObject m_select;
	public string m_out_message;
	private float m_rot;
	public GameObject m_jinyan;
	private dhc.treasure_t m_treasure;
	
	private int m_sp_id;
	private s_t_item m_t_item;
	private int m_sp_num;

	// Use this for initialization
	void Start () {
		m_rot = Random.Range (0f, 180f);
	}

	public void click(GameObject obj)
	{	
		if (m_treasure != null)
		{
			s_message _msg = new s_message();
			
			_msg.m_type = m_out_message;
			_msg.m_long.Add(m_treasure.guid);
			_msg.m_object.Add (m_icon);
			_msg.m_object.Add (this.gameObject);
			
			cmessage_center._instance.add_message (_msg);
			
			if(m_out_message == "common_devour_treasure")
			{
				bool flag = false;
				for(int i = 0; i < sys._instance.m_select_treasures.Count;++i)
				{
					if(sys._instance.m_select_treasures[i] == 0)
					{
						flag = true;
						break;
					}
				}
				if(!flag && !m_select.activeSelf)
				{
					root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("treasure_page_item.cs_52_47"));//[ffc882]选择饰品数量不能超过5个
					return;
				}
				m_select.SetActive(!m_select.activeSelf);
				sys._instance.select_treasures(m_treasure.guid);
			}
		}
		
		if (m_sp_id != 0)
		{
			s_message _msg = new s_message ();
			_msg.m_type = "show_cl_gui";
			_msg.m_ints.Add ((int)m_sp_id);
			cmessage_center._instance.add_message(_msg);
		}
	}
	
	void remove_icon(int dir)
	{
		if(m_icon != null)
		{
			if (dir == 0)
			{
				GameObject.Destroy(m_icon);
				return;
			}
			TweenAlpha.Begin (m_icon, 0.5f, 0f);
			TweenScale.Begin (m_icon, 0.5f, new Vector3(0.2f,0.2f,0.2f));
			
			TweenAlpha _alpha = m_icon.GetComponent<TweenAlpha>();
			
			EventDelegate.Add(_alpha.onFinished, delegate() 
			                  {
				GameObject.Destroy(_alpha.gameObject);
			});
			
			TweenPosition _effect = TweenPosition.Begin(m_icon,0.5f,new Vector3(-100f * dir,0,0));
			_effect.method = UITweener.Method.EaseInOut;
		}
	}
	public void set_sp(int id,int dir)
	{
		GetComponent<BoxCollider>().enabled = true;
		
		remove_icon (dir);
		
		m_select.SetActive (false);
		m_treasure = null;
		m_sp_id = 0;
		m_pro.GetComponent<UISprite>().spriteName = "";
		m_name.GetComponent<UILabel>().text = "";
		
		if(id == 0)
		{
			return;
		}
		
		m_sp_id = id;
		
		m_t_item = game_data._instance.get_item ((int)m_sp_id);
		m_name.GetComponent<UILabel>().text = ccard.get_color_name(m_t_item.name, m_t_item.font_color);
		
		m_sp_num = sys._instance.m_self.get_item_num ((uint)m_sp_id);		
		GameObject _root = game_data._instance.ins_object_res("ui/card_page_icon");
		_root.transform.parent = m_icon_root.transform;
		if (dir == 0)
		{
			_root.transform.localPosition = new Vector3(100 * 0,0,0);
			_root.transform.localScale = new Vector3(1,1,1);
		}
		else
		{
			_root.transform.localPosition = new Vector3(100 * dir,0,0);
			_root.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			_root.GetComponent<UISprite>().alpha = 0.0f;
		}
		_root.SetActive(true);
		
		GameObject _icon = icon_manager._instance.create_item_icon ((int)m_sp_id, m_sp_num, 0);
		_icon.transform.parent = _root.transform;
		_icon.transform.localPosition = new Vector3 (0, 0, 0);
		_icon.transform.localScale = new Vector3 (1, 1, 1);
		_icon.GetComponent<BoxCollider>().enabled = false;
		
		m_icon = _root;
		
		if (dir != 0)
		{
			sys._instance.add_alpha_anim (m_icon,0.5f,0f,1f,0);
			sys._instance.add_scale_anim (m_icon,0.5f,0.2f,1.0f,0);
			TweenPosition _ceffect = TweenPosition.Begin(m_icon,0.5f,new Vector3(0,0,0));
			_ceffect.method = UITweener.Method.EaseInOut;
		}
		
		m_pro.GetComponent<UISprite>().spriteName = "zsdtb_002";
	}

	public void set_treasure(ulong guid,int dir)
	{
		GetComponent<BoxCollider>().enabled = true;
		m_select.SetActive (false);
		m_treasure = null;
		m_sp_id = 0;
		remove_icon (dir);
		m_pro.GetComponent<UISprite>().spriteName = "";
		m_name.GetComponent<UILabel>().text = "";
		m_jinyan.GetComponent<UILabel>().text = "";
		if(guid == 0)
		{
			return;
		}
		
		GameObject _root = game_data._instance.ins_object_res("ui/card_page_icon");
		_root.transform.parent = m_icon_root.transform;
		if (dir == 0)
		{
			_root.transform.localPosition = new Vector3(100 * 0,0,0);
			_root.transform.localScale = new Vector3(1,1,1);
		}
		else
		{
			_root.transform.localPosition = new Vector3(100 * dir,0,0);
			_root.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			_root.GetComponent<UISprite>().alpha = 0.0f;
		}
		_root.SetActive(true);
		
		GameObject _icon = icon_manager._instance.create_treasure_icon_ex (guid);
		_icon.transform.parent = _root.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
		_icon.SetActive(true);
		_icon.GetComponent<BoxCollider>().enabled = false;
		
		m_icon = _root;
		
		if (dir != 0)
		{
			sys._instance.add_alpha_anim (m_icon,0.5f,0f,1f,0);
			sys._instance.add_scale_anim (m_icon,0.5f,0.2f,1.0f,0);
			TweenPosition _ceffect = TweenPosition.Begin(m_icon,0.5f,new Vector3(0,0,0));
			_ceffect.method = UITweener.Method.EaseInOut;
		}
		
		m_treasure = sys._instance.m_self.get_treasure_guid (guid);
		s_t_baowu t_treasure = game_data._instance.get_t_baowu(m_treasure.template_id);
		int treasure_exp = 0;
		treasure_exp += t_treasure.exp;
		treasure_exp +=  m_treasure.enhance_exp;
		treasure_exp += game_data._instance.get_total_treasure_enhance(m_treasure.enhance, t_treasure.font_color);
		if(m_treasure.jilian > 0)
		{
			treasure_exp +=  t_treasure.exp * m_treasure.jilian;
		}
		m_pro.GetComponent<UISprite>().spriteName = treasure.get_treasure_type_e(t_treasure.id);
		m_name.GetComponent<UILabel>().text = treasure.get_treasure_real_name (t_treasure.id);
		m_jinyan.GetComponent<UILabel>().text = game_data._instance.get_t_language ("player.cs_1606_16") + "+"+ treasure_exp.ToString();//经验
		if(sys._instance.is_select_treasure(guid))
		{
			m_select.SetActive (true);
		}
	}
	
	// Update is called once per frame
	void Update () {
		m_rot += Time.deltaTime * 2f;
		
		Vector3 _pos = m_icon_root.transform.localPosition;
		
		_pos.y = Mathf.Sin (m_rot) * 5f;
		
		m_icon_root.transform.localPosition = _pos;
	}
}
