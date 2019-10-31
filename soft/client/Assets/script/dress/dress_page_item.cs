using UnityEngine;

public class dress_page_item : MonoBehaviour {

	public GameObject m_name;
	public GameObject m_pro;
	public GameObject m_icon_root;
	private GameObject m_icon;
	public GameObject m_tex;
	public string m_out_message;
	private float m_rot;
	public  s_t_dress m_dress;
	public GameObject mylock;
	public GameObject m_select;
	public GameObject des;
	void Start () {
		m_rot = Random.Range (0f, 180f);
	}
	
	public void click(GameObject obj)
	{
		root_gui._instance.show_dress_detail (m_dress,1);

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

	public void set_dress(int id,int dir)
	{
		GetComponent<BoxCollider>().enabled = true;
		mylock.SetActive(false);
		remove_icon (dir);
		m_pro.GetComponent<UISprite>().spriteName = "";
		m_name.GetComponent<UILabel>().text = "";
		des.GetComponent<UILabel>().text = "";
		m_tex.GetComponent<UISprite>().spriteName = "nszjm_szblz04";
		m_select.SetActive (false);
		if(id == 0)
		{
			GetComponent<BoxCollider>().enabled = false;
			return;
		}

		GameObject _icon = game_data._instance.ins_object_res("ui/dress_item_icon");
		_icon.transform.parent = m_icon_root.transform;
		_icon.GetComponent<UISprite>().spriteName = m_dress.icon;

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
		if (m_dress.type == 1) 
		{
			m_pro.GetComponent<UISprite>().spriteName = "szsjbi_003";
		}
		else if(m_dress.type == 2)
		{
			m_pro.GetComponent<UISprite>().spriteName = "szsjbi_001";
		}
		else if(m_dress.type == 3)
		{
			m_pro.GetComponent<UISprite>().spriteName = "szsjbi_002";
		}
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(m_dress.color) + m_dress.name;
		/*if(m_dress.value1 != 0)
		{
			des.GetComponent<UILabel>().text = string.Format (game_data._instance.get_value_string (m_dress.attr1, m_dress.value1,1)) 
				+ "\n";
		}*/
		m_tex.GetComponent<UISprite>().spriteName = chengjiu_item.icon_name (m_dress);
		mylock.SetActive(true);

		if (sys._instance.m_self.has_dress (m_dress.id))
		{
			mylock.SetActive(false);
				
		} 
		 if (sys._instance.m_self.has_dress_on (m_dress.id))
		{
			m_select.SetActive (true);
		}
	}

}
