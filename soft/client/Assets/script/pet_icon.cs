
using UnityEngine;
using System.Collections;

public class pet_icon : MonoBehaviour {

	public ulong m_guid_id;
	public pet m_pet;
	public int m_star = 0;
	public int m_jlevel = 0;
	public int m_level = 0;
	public s_t_pet m_t_pet;
	public string m_out_message;


	// Use this for initialization
	void Start () {
	
	}

	public void init()
	{
		m_guid_id = 0;
		m_pet = null;
		m_star = 0;
		m_jlevel = 0;
		m_level = 0;
		m_pet = null;
		
		UIDragScrollView uisv = this.GetComponent<UIDragScrollView>();
		if (uisv != null)
		{
			Object.Destroy(uisv);
		}
		this.GetComponent<UISprite>().alpha = 1.0f;
		this.transform.GetComponent<BoxCollider>().enabled = true;
		this.transform.localEulerAngles = new Vector3(0,0,0);
	}

	public void reset(int type = 0)
	{
		if(m_guid_id > 0)
		{
			m_pet = sys._instance.m_self.get_pet_guid (m_guid_id);
		}
		
		if(m_pet != null)
		{
			m_t_pet = m_pet.m_t_pet;
			if(type == 0 )
			{
				m_star = m_pet.get_star();
				m_jlevel = m_pet.get_jlevel();
				m_level = m_pet.get_level();
			}
			else
			{
				m_star = 0;
				m_jlevel = 0;
				m_level = 1;
			}
			
			m_guid_id = m_pet.get_guid();
		}
		
		if(m_t_pet == null)
		{
			return;
		}

		this.transform.GetComponent<UISprite>().spriteName = m_t_pet.icon;
		
		int color = 0;
		string s = "";
		color = m_t_pet.color;
		
		if (color == 1)
		{
			s = "xtbk_lvpt001";
		}
		else if (color == 2)
		{
			s = "xtbk_lanpt001";
		}
		else if (color == 3)
		{
			s = "xtbk_zipt001";
		}
		else if (color == 4)
		{
			s = "xtbk_chpt001";
		}
		else if (color == 5)
		{
			s = "xtbk_hopt001";
		}
		else
		{
			s = "xtbk_jinpt001";
		}
		
		this.transform.Find("bg").GetComponent<UISprite>().spriteName = s;
		
		if (m_level == 0)
		{
			this.transform.Find("lv").gameObject.SetActive(false);
			this.transform.Find("jlevel").gameObject.SetActive(false);
		}
		else
		{
			this.transform.Find("lv").gameObject.SetActive(true);
			this.transform.Find("jlevel").gameObject.SetActive(true);
			this.transform.Find("lv").GetComponent<UILabel>().text = m_level.ToString();

			s_t_pet_jinjie t_jinjie = game_data._instance.get_t_pet_jinjie(m_jlevel);
			this.transform.Find("jlevel").GetComponent<UISprite>().spriteName = t_jinjie.icon;
			this.transform.Find("jlevel").GetComponent<UISprite>().MakePixelPerfect();
		}
		
		this.transform.Find("s1").gameObject.SetActive(true);
		this.transform.Find("s2").gameObject.SetActive(true);
		this.transform.Find("s3").gameObject.SetActive(true);
		this.transform.Find("s4").gameObject.SetActive(true);
		this.transform.Find("s5").gameObject.SetActive(true);
		
		color = 0;
		
		if(m_pet != null)
		{
			color = m_pet.get_star();
		}
		
		else if (m_star >= 0)
		{
			color = m_star;
		}
		
		for(int i = 1; i <= 5;i ++)
		{
			if(i <= color)
			{
				this.transform.Find("s" + i.ToString()).gameObject.SetActive(true);
			}
			else
			{
				this.transform.Find("s" + i.ToString()).gameObject.SetActive(false);
			}
		}
		
		int num_ = color;
		if(num_ > 5)
		{
			num_ = 5;
		}
		this.transform.Find("s1").localPosition = new Vector3(-6 * (num_ - 1), -40, 0);

	}

	public void click()
	{
		if(m_out_message.Length == 0)
		{
			if(m_pet != null && ( sys._instance.m_self.is_gaurd(m_pet.get_guid())))
			{
				root_gui._instance.show_pet_detail(m_pet.get_pet(), 0);
				return;
			}
			s_message _message = new s_message ();
			_message.m_type = "pet_dialog_box";
			_message.m_ints.Add (m_t_pet.id);
			_message.m_ints.Add (1);
			_message.m_object.Add (m_pet);
			
			cmessage_center._instance.add_message (_message);
		}
	}
}
