
using UnityEngine;
using System.Collections;

public class treasure_icon : MonoBehaviour {

	
	public ulong m_treasure_guid;
	public int m_treasure_id;
	public int m_enhance;
	public int m_jl;
	public dhc.treasure_t m_treasure;
	private s_t_baowu m_t_treasure;
	public int m_max = 0;
	public int m_num = 0;
	public bool m_show_enhance;
	public bool m_show_sx;
	public GameObject m_lock;
	public GameObject m_type;
	public string m_out_message;
	public bool flag = false;
	// Use this for initialization
	void Start () {
		m_type.GetComponent<UILabel>().text = game_data._instance.get_t_language ("treasure_icon.cs_23_41");//已装备
	}

	public void press()
	{
		if(m_out_message.Length > 0)
		{
			return;
		}

	}

	public void release()
	{
		s_message _message = new s_message ();
		
		_message.m_type = "hide_min_dialog_box";
		
		cmessage_center._instance.add_message (_message);
	}
	public void init()
	{
		m_treasure_guid = 0;
		m_treasure_id = 0;
		m_treasure = null;
		m_max = 0;
		m_num = 0;
		m_enhance = 0;
		m_jl = 0;
		m_show_enhance = true;
		m_show_sx = true;
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

	public void click()
	{
		if(m_out_message.Length == 0)
		{
			s_message _message = new s_message ();
			
			_message.m_type = "treasure_dialog_box";
			_message.m_ints.Add (m_t_treasure.id);
			if(m_t_treasure.type != 5)
			{
				_message.m_ints.Add (1);
			}
			else
			{
				_message.m_ints.Add (2);
			}
			_message.m_object.Add (m_treasure);
			
			cmessage_center._instance.add_message (_message);
			return;
		}
		
		s_message m_message = new s_message ();
		
		m_message.m_type = m_out_message;
		m_message.m_ints.Add (m_treasure.guid);
		m_message.m_object.Add (gameObject);
		
		cmessage_center._instance.add_message (m_message);
	}
	
	public void reset()
	{
		if (m_treasure != null)
		{
			m_treasure_id = m_treasure.template_id;
			m_t_treasure = game_data._instance.get_t_baowu(m_treasure_id);
		}
		else if (m_treasure_guid > 0)
		{
			m_treasure = sys._instance.m_self.get_treasure_guid(m_treasure_guid);
			m_treasure_id = m_treasure.template_id;
			m_t_treasure = game_data._instance.get_t_baowu (m_treasure_id);
		}
		else
		{
			m_treasure = new dhc.treasure_t();
			m_treasure.template_id = m_treasure_id;
			m_treasure.enhance = m_enhance;
			m_treasure.jilian = m_jl;
			m_t_treasure = game_data._instance.get_t_baowu (m_treasure_id);
		}
		
		this.transform.GetComponent<UISprite>().spriteName = m_t_treasure.icon;
		
		if (m_max > 0)
		{
			string s = "[ffff00]";
			if (m_num < m_max)
			{
				s = "[ff0000]";
			}
			s = s + m_num.ToString() + "[ffffff]/" + m_max.ToString();
			this.transform.Find("num").GetComponent<UILabel>().text = s;
		}
		else if(m_max == -1)
		{
			this.transform.Find("num").GetComponent<UILabel>().text = "[FFFF00]" +  m_num.ToString() + "[-]/--"; 
		}
		else
		{
			this.transform.Find("num").GetComponent<UILabel>().text = "";
		}
		string ss = "zbkt_l001";
		string ss1 = "";
		if (m_t_treasure.font_color == 2)
		{
			ss = "zbkt_b001";
			ss1 = "zbkt_b00";
		}
		else if (m_t_treasure.font_color == 3)
		{
			ss = "zbkt_ph001";
			ss1 = "zbkt_ph00";
		}
		else if (m_t_treasure.font_color == 4)
		{
			ss = "zbkt_gh001";
			ss1 = "zbkt_gh00";
		}
		if (ss == "")
		{
			this.transform.Find("kuang").gameObject.SetActive(false);
		}
		else
		{
			this.transform.Find("kuang").gameObject.SetActive(true);
			this.transform.Find("kuang").GetComponent<UISprite>().spriteName = ss;
		}
		if (ss1 == "")
		{
			this.transform.Find("kuang").GetComponent<UISpriteAnimation>().enabled = false;
		}
		else
		{
			this.transform.Find("kuang").GetComponent<UISpriteAnimation>().enabled = true;
			this.transform.Find("kuang").GetComponent<UISpriteAnimation>().namePrefix = ss1;
		}
		string bg_type = "xtbk_lvpt001";
		if (m_t_treasure.font_color == 2)
		{
			bg_type = "xtbk_lanpt001";
		}
		if (m_t_treasure.font_color == 3)
		{
			bg_type = "xtbk_zipt001";
		}
		if (m_t_treasure.font_color == 4)
		{
			bg_type = "xtbk_chpt001";
		}
		if (m_t_treasure.font_color == 5)
		{
			bg_type = "xtbk_hopt001";
		}
		if (m_t_treasure.font_color == 6)
		{
			bg_type = "xtbk_jinpt001";
		}
		this.transform.Find("bg").GetComponent<UISprite>().spriteName = bg_type;
		if (m_show_enhance && m_treasure.enhance > 0)
		{
			this.transform.Find("enhance").gameObject.SetActive(true);
			this.transform.Find("enhance").GetComponent<UILabel>().text = "+" + m_treasure.enhance.ToString();
		}
		else
		{
			this.transform.Find("enhance").gameObject.SetActive(false);
		}
		if (!m_show_sx)
		{
			this.transform.Find("s1").gameObject.SetActive(false);
			this.transform.Find("s2").gameObject.SetActive(false);
			this.transform.Find("s3").gameObject.SetActive(false);
			this.transform.Find("s4").gameObject.SetActive(false);
			this.transform.Find("s5").gameObject.SetActive(false);
		}
		else
		{
			for(int i = 1; i <= 5;i ++)
			{
				if(i <= m_treasure.star)
				{
					this.transform.Find("s" + i.ToString()).gameObject.SetActive(true);
				}
				else
				{
					this.transform.Find("s" + i.ToString()).gameObject.SetActive(false);
				}
				
			}
			int num_ = m_treasure.star;
			this.transform.Find("s1").localPosition = new Vector3(-6 * (num_ - 1), -35, 0);
		}
		if (m_treasure.locked == 1)
		{
			m_lock.SetActive(true);
		}
		else
		{
			m_lock.SetActive(false);
		}
		if(m_treasure != null && flag)
		{
			if(m_treasure.role_guid != 0)
			{
				m_type.SetActive(true);
			}
			else
			{
				m_type.SetActive(false);
			}
		}
	}

	// Update is called once per frame
	void Update () 
	{
		
	}
}
