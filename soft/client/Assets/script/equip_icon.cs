
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class equip_icon : MonoBehaviour {

	public ulong m_equip_guid;
	public int m_equip_id;
	public int m_enhance;
	public int m_jl;
	public dhc.equip_t m_equip;
	private s_t_equip m_t_equip;
	public int m_max = 0;
	public int m_num = 0;
	public bool m_show_enhance;
	public GameObject m_lock;
	public string m_out_message;
	public GameObject m_type;
	public bool flag = false;
	// Use this for initialization
	void Start () {
		
	}

	public void click()
	{
		if(m_out_message.Length == 0)
		{
			s_message _message = new s_message ();
			
			_message.m_type = "equip_dialog_box";
			_message.m_string.Add (equip.get_equip_name(m_equip));
			_message.m_ints.Add (m_t_equip.id);
			_message.m_ints.Add (1);
			_message.m_object.Add (m_equip);
			
			cmessage_center._instance.add_message (_message);
			return;
		}
		
		s_message m_message = new s_message ();
		
		m_message.m_type = m_out_message;
		m_message.m_ints.Add (m_equip.guid);
		m_message.m_object.Add (gameObject);
		
		cmessage_center._instance.add_message (m_message);
		
	}
	public void release()
	{
		s_message _message = new s_message ();
	
		_message.m_type = "hide_min_dialog_box";
		
		cmessage_center._instance.add_message (_message);
	}

	public void init()
	{
		m_equip_guid = 0;
		m_equip_id = 0;
		m_equip = null;
		m_max = 0;
		m_num = 0;
		m_enhance = 0;
		m_jl = 0;
		m_show_enhance = true;
		m_type.SetActive (false);
		UIDragScrollView uisv = this.GetComponent<UIDragScrollView>();
		if (uisv != null)
		{
			Object.Destroy(uisv);
		}
		this.GetComponent<UISprite>().alpha = 1.0f;
		this.transform.GetComponent<BoxCollider>().enabled = true;
		this.transform.localEulerAngles = new Vector3(0,0,0);
	}

	public void reset()
	{
		if (m_equip != null)
		{
			m_equip_id = m_equip.template_id;
			m_t_equip = game_data._instance.get_t_equip (m_equip_id);
		}
		else if (m_equip_guid > 0)
		{
			m_equip = sys._instance.m_self.get_equip_guid(m_equip_guid);
			m_equip_id = m_equip.template_id;
			m_t_equip = game_data._instance.get_t_equip (m_equip_id);
		}
		else
		{
			m_equip = new dhc.equip_t();
			m_equip.template_id = m_equip_id;
			m_equip.enhance = m_enhance;
			m_equip.star = m_jl;
			m_t_equip = game_data._instance.get_t_equip (m_equip_id);
		}
		
		this.transform.GetComponent<UISprite>().spriteName = m_t_equip.icon;

		if (m_max > 0)
		{
			string s = "";
			if (m_num < m_max)
			{
				s = "[ff0000]";
			}
			s = s + m_num.ToString() + "[ffffff]/" + m_max.ToString();
			this.transform.Find("num").GetComponent<UILabel>().text = s;
		}
		else
		{
			if(m_num != 0)
			{
				this.transform.Find("num").GetComponent<UILabel>().text = m_num.ToString();
			}
			else
			{
				this.transform.Find("num").GetComponent<UILabel>().text = "";
			}

		}
		if(m_equip != null && flag)
		{
			if(m_equip.role_guid != 0)
			{
				m_type.SetActive(true);
			}
			else
			{
				m_type.SetActive(false);
			}
		}

		//string ss = "zbkt_l001";
		string ss = "xtbk_lvpt001";

		if (m_t_equip.font_color == 2)
		{
			ss = "xtbk_lanpt001";
			//s1 = "";
		}
		else if (m_t_equip.font_color == 3)
		{
			ss = "xtbk_zipt001";
			//s1 = "";
		}
		else if (m_t_equip.font_color == 4)
		{
			ss = "xtbk_chpt001";
			//s1 = "txkt_gl00";
		}
		else if (m_t_equip.font_color == 5)
		{
			ss = "xtbk_hopt001";
			//s1 = "txkt_gl00";
		}
		else if (m_t_equip.font_color == 6)
		{
			ss = "xtbk_jinpt001";
			//s1 = "txkt_gl00";
		}

		this.transform.Find("bg").GetComponent<UISprite>().spriteName = ss;

		if (m_show_enhance && m_equip.enhance > 0)
		{
			this.transform.Find("enhance").gameObject.SetActive(true);
			this.transform.Find("enhance").GetComponent<UILabel>().text = "+" + m_equip.enhance.ToString();
		}
		else
		{
			this.transform.Find("enhance").gameObject.SetActive(false);
		}
		if (m_equip.locked == 1)
		{
			m_lock.SetActive(true);
		}
		else
		{
			m_lock.SetActive(false);
		}

		if (!m_show_enhance)
		{
			this.transform.Find("r0").gameObject.SetActive(false);
			this.transform.Find("r1").gameObject.SetActive(false);
			this.transform.Find("r2").gameObject.SetActive(false);

			this.transform.Find("s1").gameObject.SetActive(false);
			this.transform.Find("s2").gameObject.SetActive(false);
			this.transform.Find("s3").gameObject.SetActive(false);
			this.transform.Find("s4").gameObject.SetActive(false);
			this.transform.Find("s5").gameObject.SetActive(false);
		}
		else
		{
			this.transform.Find("r0").gameObject.SetActive(true);
			this.transform.Find("r1").gameObject.SetActive(true);
			this.transform.Find("r2").gameObject.SetActive(true);

			for(int i = 1; i <= 5;i ++)
			{
				if(i <= m_equip.star)
				{
					this.transform.Find("s" + i.ToString()).gameObject.SetActive(true);
				}
				else
				{
					this.transform.Find("s" + i.ToString()).gameObject.SetActive(false);
				}

			}
			int num_ = m_equip.star;
			this.transform.Find("s1").localPosition = new Vector3(-6 * (num_ - 1), -35, 0);
			for (int i = 0; i < m_equip.stone.Count; ++i)
			{
				this.transform.Find("r" + (i).ToString()).gameObject.SetActive(true);
				this.transform.Find("r" + (i).ToString()).localPosition = new Vector3(i * 20 - (m_equip.stone.Count - 1) * 10, -17, 0);
				if (m_equip.stone[i] == 0)
				{
					this.transform.Find("r" + (i).ToString()).GetComponent<UISprite>().spriteName = "zbzs_xdb";
				}
				else
				{
					s_t_item t_item = game_data._instance.get_item(m_equip.stone[i]);
					this.transform.Find("r" + (i).ToString()).GetComponent<UISprite>().spriteName = "zbzs_xzs00" + t_item.def_1.ToString();
				}
			}
			for (int i = m_equip.stone.Count; i < 3; ++i)
			{
				this.transform.Find("r" + (i).ToString()).gameObject.SetActive(false);
			}
		}

		this.transform.Find("r0").gameObject.SetActive(false);
		this.transform.Find("r1").gameObject.SetActive(false);
		this.transform.Find("r2").gameObject.SetActive(false);

	}
	// Update is called once per frame
	void Update () 
	{

	}
}
