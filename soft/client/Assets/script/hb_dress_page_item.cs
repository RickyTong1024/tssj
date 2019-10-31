
using UnityEngine;
using System.Collections;

public class hb_dress_page_item : MonoBehaviour {
	
	public int m_role_dress_id;
	public ccard m_card;
	public GameObject m_name;
	public GameObject m_pro;
	public s_t_role_dress m_t_role_dress;
	private int m_dress_id;
	private GameObject m_icon;
	public GameObject m_ycd;
	public GameObject m_cd;
	public GameObject m_wjs;
	public GameObject m_lock;
	public GameObject m_icon_root;
	public GameObject m_button;
	public UIAtlas m_atlas;
	public UIAtlas m_atlas1;

	public UILabel m_wjs_Label;
	public UILabel m_ycd_Label;
	public UILabel m_cd_Label;

	// Use this for initialization
	void Start () {

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

	public void set_hb_dress(int id,int dir)
	{
		GetComponent<BoxCollider>().enabled = true;
		remove_icon (dir);
		m_lock.SetActive(false);
		m_button.SetActive (false);
		m_pro.GetComponent<UISprite>().spriteName = "";
		m_name.GetComponent<UILabel>().text = "";
		if(id == -1)
		{
			GetComponent<BoxCollider>().enabled = false;
			return;
		}
		
		GameObject _icon = game_data._instance.ins_object_res("ui/hb_dress_item_icon");
		_icon.transform.parent = m_icon_root.transform;
		if(m_atlas.GetListOfSprites().Contains(m_t_role_dress.icon))
		{
			_icon.transform.GetComponent<UISprite>().atlas = m_atlas;
		}
		else
		{
			_icon.transform.GetComponent<UISprite>().atlas = m_atlas1;
		}
		_icon.GetComponent<UISprite>().spriteName = m_t_role_dress.icon;
		
		GameObject _root = _icon.gameObject;
		if (dir == 0)
		{
			_root.transform.localPosition = new Vector3(100 * 0,-10,0);
			_root.transform.localScale = new Vector3(1,1,1);
		}
		else
		{
			_root.transform.localPosition = new Vector3(100 * dir,-10,0);
			_root.transform.localScale = new Vector3(0.2f,0.2f,0.2f);
			_root.GetComponent<UISprite>().alpha = 0.0f;
		}
		_root.SetActive(true);
		
		m_icon = _root;
		
		if (dir != 0)
		{
			sys._instance.add_alpha_anim (m_icon,0.5f,0f,1f,0);
			sys._instance.add_scale_anim (m_icon,0.5f,0.2f,1.0f,0);
			TweenPosition _ceffect = TweenPosition.Begin(m_icon,0.5f,new Vector3(0,-10,0));
			_ceffect.method = UITweener.Method.EaseInOut;
		}
		m_name.GetComponent<UILabel>().text = m_t_role_dress.name;
		m_pro.GetComponent<UISprite>().spriteName = "szsjbi_003";
		m_lock.SetActive(true);
		m_button.SetActive (true);
		ccard _card = sys._instance.m_self.get_card_guid(m_card.get_guid());
		dhc.role_t m_role = _card.get_role ();
		if (!has_dress(m_t_role_dress.id) && m_t_role_dress.hq_condition != 3)
		{
			m_ycd.SetActive(false);
			m_cd.SetActive(false);
			m_wjs.SetActive(true);
			m_lock.SetActive(true);
			if(m_t_role_dress.mai_desc != "0")
			{
				m_lock.transform.Find("condition").gameObject.SetActive(true);
				m_lock.transform.Find("condition").GetComponent<UILabel>().text = m_t_role_dress.mai_desc;
			}
			else
			{
				m_lock.transform.Find("condition").gameObject.SetActive(false);
			}
		}
		else
		{
			if(m_t_role_dress.hq_condition == 3)
			{
				if(sys._instance.m_self.has_role_dress_on (m_card.get_guid(),0))
				{
					m_ycd.SetActive(true);
					m_cd.SetActive(false);
					m_wjs.SetActive(false);
					m_lock.SetActive(false);
				}
				else
				{
					m_ycd.SetActive(false);
					m_cd.SetActive(true);
					m_wjs.SetActive(false);
					m_lock.SetActive(false);
				}
			}
			else
			{
				if (sys._instance.m_self.has_role_dress_on (m_card.get_guid(),m_t_role_dress.id))
				{
					m_ycd.SetActive(true);
					m_cd.SetActive(false);
					m_wjs.SetActive(false);
					m_lock.SetActive(false);
				}
				else
				{
					m_ycd.SetActive(false);
					m_cd.SetActive(true);
					m_wjs.SetActive(false);
					m_lock.SetActive(false);
				}
			}
		}
	}

	bool has_dress(int id)
	{
		for(int i = 0; i < m_card.get_role().dress_ids.Count;++i)
		{
			if(id ==  m_card.get_role().dress_ids[i])
			{
				return true;
			}
		}
		return false;
	}

	void click(GameObject obj)
	{
		if(obj.name  == "cd")
		{
			s_message _msg = new s_message();
			_msg.m_type = "role_dress_on";
			_msg.m_object.Add (m_t_role_dress);
			cmessage_center._instance.add_message(_msg);
		}
		if(obj.name == "wjs")
		{
			root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("hb_dress_page_item.cs_188_58"));//未解锁
			return;
		}
	}

	/*public void select(GameObject obj)
	{
		s_message _msg = new s_message();
		_msg.m_type = "show_hb_dress_detail";
		_msg.m_object.Add (m_t_role_dress);
		cmessage_center._instance.add_message(_msg);
		
	}*/

	// Update is called once per frame
	void Update () {
	
	}
}
