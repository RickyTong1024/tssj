
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_page_item : MonoBehaviour {

	public GameObject m_icon_root;
	public GameObject m_name;
	public GameObject m_dh;
	public GameObject m_pro;
	private GameObject m_icon;
	public GameObject m_select;
	public string m_out_message;
	private float m_rot;
	private dhc.equip_t m_equip;

	private int m_sp_id;
	private s_t_item m_t_item;
	private int m_sp_num;
	// Use this for initialization
	void Start () {
		m_rot = Random.Range (0f, 180f);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "dh")
		{
			if (m_sp_num < m_t_item.def_2)
			{
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language ("card_page_item.cs_48_59"));//碎片不足
				return ;
			}
			
			s_message _message = new s_message ();
			_message.m_type = "equipsp_he_cheng";
			_message.m_ints.Add ((int)m_sp_id);

			cmessage_center._instance.add_message (_message);
			return;
		}

		if (m_equip != null)
		{
			s_message _msg = new s_message();
			
			_msg.m_type = m_out_message;
			_msg.m_long.Add(m_equip.guid);
			_msg.m_object.Add (m_icon);
			_msg.m_object.Add (this.gameObject);
			
			cmessage_center._instance.add_message (_msg);
			
			if(m_out_message == "select_equip")
			{
				m_select.SetActive(!m_select.activeSelf);
				sys._instance.select_equips (m_equip.guid);
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
		m_dh.SetActive (false);
		m_equip = null;
		m_sp_id = 0;
		//m_pro.GetComponent<UILabel>().text = "";
		m_name.GetComponent<UILabel>().text = "";

		if(id == 0)
		{
			return;
		}

		m_sp_id = id;

		m_t_item = game_data._instance.get_item ((int)m_sp_id);
		m_name.GetComponent<UILabel>().text = ccard.get_color_name(m_t_item.name, m_t_item.font_color);
		
		m_sp_num = sys._instance.m_self.get_item_num ((uint)m_sp_id);		
		if (m_sp_num >= m_t_item.def_2)
		{
			m_dh.SetActive(true);
		}
		else
		{
			m_dh.SetActive(false);
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

		GameObject _icon = icon_manager._instance.create_item_icon ((int)m_sp_id, m_sp_num, m_t_item.def_2);
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

		m_pro.GetComponent<UISprite>().spriteName = "szzjsj_001";
	}
	public void set_equip(ulong guid,int dir)
	{
		GetComponent<BoxCollider>().enabled = true;
		m_select.SetActive (false);
		m_dh.SetActive (false);
		m_equip = null;
		m_sp_id = 0;
		remove_icon (dir);
		m_pro.GetComponent<UISprite>().spriteName = "";
		m_name.GetComponent<UILabel>().text = "";

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
		
		GameObject _icon = icon_manager._instance.create_equip_icon_ex (guid);
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

		m_equip = sys._instance.m_self.get_equip_guid (guid);
		s_t_equip t_equip = game_data._instance.get_t_equip (m_equip.template_id);
		m_pro.GetComponent<UISprite>().spriteName = equip.get_equip_type(t_equip.id);
		m_name.GetComponent<UILabel>().text = equip.get_equip_real_name (t_equip.id);

		m_dh.SetActive (false);

		if(sys._instance.is_select_equip(guid))
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
