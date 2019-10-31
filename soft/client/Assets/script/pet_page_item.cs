
using UnityEngine;
using System.Collections;

public class pet_page_item : MonoBehaviour {
	public GameObject m_icon_root;
	public GameObject m_name;
	public GameObject m_sz;
	public GameObject m_dh;
	public GameObject m_hc;
	public GameObject m_sell;
	public GameObject m_pro;
	public GameObject m_zhanli;
	private GameObject m_icon;
	public GameObject m_select;
	public string m_out_message;
	private float m_rot;
	private pet m_pet;
	
	private int m_sp_id;
	private s_t_item m_t_item;
	private int m_sp_num;
	// Use this for initialization
	void Start () {
	
		m_rot = Random.Range (0f, 180f);
	}
	
	void hide()
	{
		m_dh.SetActive (false);
		m_hc.SetActive (false);
		m_sell.SetActive (false);
		m_sz.SetActive (false);
	}
	
	public void click(GameObject obj)
	{
		if (m_pet != null)
		{
			s_message _msg = new s_message();
			
			_msg.m_type = m_out_message;
			_msg.m_long.Add(m_pet.get_guid());
			_msg.m_object.Add (m_icon);
			
			cmessage_center._instance.add_message (_msg);
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
	public void set_sp(int id,int dir,bool flag = false)
	{
		GetComponent<BoxCollider>().enabled = true;
		
		remove_icon (dir);
		
		hide ();
		m_pet = null;
		m_sp_id = 0;
		m_pro.GetComponent<UILabel>().text = "";
		m_name.GetComponent<UILabel>().text = "";
		m_select.SetActive (false);
		if(id == 0)
		{
			return;
		}
		
		m_sp_id = id;
		
		m_t_item = game_data._instance.get_item ((int)m_sp_id);
		m_name.GetComponent<UILabel>().text = pet.get_color_name(m_t_item.name, m_t_item.font_color);
		
		m_sp_num = sys._instance.m_self.get_item_num ((uint)m_sp_id);
		
		if (m_sp_num >= m_t_item.def_2)
		{
			//m_dh.SetActive(true);
		}
		else
		{
			m_dh.SetActive(false);
		}
		
		GameObject _root = game_data._instance.ins_object_res("ui/pet_page_icon");
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
		if(flag)
		{
			GameObject _icon = icon_manager._instance.create_item_icon ((int)m_sp_id,m_sp_num);
			_icon.transform.parent = _root.transform;
			_icon.transform.localPosition = new Vector3 (0, 0, 0);
			_icon.transform.localScale = new Vector3 (1, 1, 1);
			_icon.GetComponent<BoxCollider>().enabled = false;
		}
		else
		{
			GameObject _icon = icon_manager._instance.create_item_icon ((int)m_sp_id,0);
			_icon.transform.parent = _root.transform;
			_icon.transform.localPosition = new Vector3 (0, 0, 0);
			_icon.transform.localScale = new Vector3 (1, 1, 1);
			_icon.GetComponent<BoxCollider>().enabled = false;
		}
		
		m_icon = _root;
		
		if (dir != 0)
		{
			sys._instance.add_alpha_anim (m_icon,0.5f,0f,1f,0);
			sys._instance.add_scale_anim (m_icon,0.5f,0.2f,1.0f,0);
			TweenPosition _ceffect = TweenPosition.Begin(m_icon,0.5f,new Vector3(0,0,0));
			_ceffect.method = UITweener.Method.EaseInOut;
		}
		if(recycle_gui.is_injiyins((int)m_sp_id))
		{
			m_select.SetActive(true);
		}
		else
		{
			m_select.SetActive(false);
		}
        //m_pro.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_page_item.cs_154_39");//宠
	}
	public void set_pet(ulong guid,int dir)
	{
		GetComponent<BoxCollider>().enabled = true;
		hide ();
		m_pet = null;
		m_sp_id = 0;
		m_pro.GetComponent<UILabel>().text = "";
		m_name.GetComponent<UILabel>().text = "";
		m_zhanli.GetComponent<UILabel>().text = "";
		m_select.SetActive (false);
		m_hc.SetActive (false);
		remove_icon (dir);
		
		if(guid == 0)
		{
			return;
		}
		
		m_pet = sys._instance.m_self.get_pet_guid (guid);
		
		if(m_pet == null)
		{
			return;
		}
		
		GameObject _root = game_data._instance.ins_object_res("ui/pet_page_icon");
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
		
		GameObject _icon = icon_manager._instance.create_pet_icon_ex (guid);
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

		m_name.GetComponent<UILabel>().text = m_pet.get_color_name();
		m_sz.SetActive(false);
		m_select.SetActive(false);
		m_dh.SetActive (false);
        //m_pro.GetComponent<UILabel>().text = game_data._instance.get_t_language ("pet_page_item.cs_154_39");//宠
		m_zhanli.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_fight.cs_43_118") + sys._instance.value_to_wan((long)m_pet.get_fight ());//战力 
	}
	

	// Update is called once per frame
	void Update () {
		m_rot += Time.deltaTime * 2f;
		
		Vector3 _pos = m_icon_root.transform.localPosition;
		
		_pos.y = Mathf.Sin (m_rot) * 5f;
		
		m_icon_root.transform.localPosition = _pos;
	}
}
