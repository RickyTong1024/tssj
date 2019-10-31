
using UnityEngine;
using System.Collections;

public class item_icon : MonoBehaviour {
	
	public int m_item_id;
	public int m_max = 0;
	public int m_item_num = 0;
	public string m_out_message;
	public bool flag = false;
	public bool type = false;
	private s_t_item m_t_item;
	public UIAtlas m_atlas;
	public UIAtlas m_atlas1;
	public UIAtlas m_atlas2;
	public UIAtlas m_atlas3;
	public UIAtlas m_atlas4;
	public UIAtlas m_atlas5;
	// Use this for initialization
	void Start () {
	}
	
	public void click()
	{
		if(m_out_message.Length == 0)
		{
			if(m_item_id == 1)
			{
				return;
			}
			if(m_t_item.type != 3001 && m_t_item.type != 7001 && m_t_item.type != 6001&& m_t_item.type != 9001 && m_t_item.type != 10001)
			{
				s_message _message = new s_message ();
				
				_message.m_type = "item_dialog_box";
				_message.m_ints.Add (m_t_item.id);
				_message.m_ints.Add (2);
				cmessage_center._instance.add_message (_message);
			}
			else if(m_t_item.type == 7001)
			{
				s_message _message = new s_message ();
				
				_message.m_type = "equip_dialog_box";
				_message.m_string.Add (ccard.get_color_name(m_t_item.name, m_t_item.font_color));
				_message.m_ints.Add (m_t_item.id);
				_message.m_ints.Add (0);
				_message.m_object.Add (null);
				cmessage_center._instance.add_message (_message);
			}
			else if(m_t_item.type == 6001)
			{
				s_message _message = new s_message ();
				
				_message.m_type = "treasure_dialog_box";
				_message.m_string.Add (ccard.get_color_name(m_t_item.name, m_t_item.font_color));
				_message.m_ints.Add (m_t_item.id);
				_message.m_ints.Add (0);
				_message.m_object.Add (null);
				cmessage_center._instance.add_message (_message);
			}
			else if(m_t_item.type == 3001)
			{
				s_message _message = new s_message ();
				
				_message.m_type = "card_dialog_box";
				_message.m_string.Add (ccard.get_color_name(m_t_item.name, m_t_item.font_color));
				_message.m_ints.Add (m_t_item.id);
				_message.m_ints.Add (0);
				_message.m_object.Add (null);
				cmessage_center._instance.add_message (_message);
			}
			else if(m_t_item.type == 9001)
			{
				s_message _message = new s_message ();
				
				_message.m_type = "huiyi_dialog_box";
				_message.m_string.Add (ccard.get_color_name(m_t_item.name, m_t_item.font_color));
				_message.m_ints.Add (m_t_item.id);
				cmessage_center._instance.add_message (_message);
			}
			else if(m_t_item.type == 10001)
			{
				s_message _message = new s_message ();
				
				_message.m_type = "pet_dialog_box";
				_message.m_string.Add (pet.get_color_name(m_t_item.name, m_t_item.font_color));
				_message.m_ints.Add (m_t_item.id);
				_message.m_ints.Add (0);
				_message.m_object.Add (null);
				cmessage_center._instance.add_message (_message);
			}
			else
			{
				Camera gc = NGUITools.FindCameraForLayer (this.gameObject.layer);
				Vector3 pos = gc.WorldToScreenPoint (this.transform.position);
				
				s_message _message = new s_message ();
				
				_message.m_type = "min_dialog_box";
				_message.m_string.Add (ccard.get_color_name(m_t_item.name, m_t_item.font_color));
				string text = m_t_item.desc;
				if(m_t_item.type == 7001)
				{
					text = text + "\n" + equip.get_equip_value(m_t_item.def_1,0,0);
				}
				if(m_item_id == (int)e_huodong_item_id.ei_huodong_item1)
				{
					text = sys._instance.m_self.m_jieri_icon_desc;
				}
				if(m_item_id == (int)e_huodong_item_id.ei_huodong_item2)
				{
					text = sys._instance.m_self.m_jieri_icon_desc1;
				}
				_message.m_string.Add (text);
				_message.m_object.Add (pos);
				_message.m_ints.Add (2);
				_message.m_ints.Add (m_t_item.id);
				_message.m_ints.Add (0);
				_message.m_ints.Add (0);
				
				if (m_t_item.need_level > 0)
				{
					_message.m_ints.Add(m_t_item.need_level);
				}
				if(m_t_item.type == 3001)
				{
					s_t_class _t_class = game_data._instance.get_t_class(m_t_item.def_1);
					string text1 = game_data._instance.get_t_language ("item_icon.cs_130_20") + _t_class.pz;//品质
					_message.m_string.Add (ccard.get_color_name(text1, m_t_item.font_color));
				}
				cmessage_center._instance.add_message (_message);
			}
			return;
		}

		s_message m_message = new s_message ();
		
		m_message.m_type = m_out_message;
		m_message.m_ints.Add (m_item_id);
		m_message.m_object.Add (gameObject);
		m_message.m_string.Add (m_t_item.name);
		cmessage_center._instance.add_message (m_message);

	}

	public void init()
	{
		m_item_id = 0;
		m_max = 0;
		m_item_num = 0;
		flag = false;
		m_out_message = "";
		type = false;
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
		if(m_item_id == 0)
		{
			return;
		}

		m_t_item = game_data._instance.get_item (m_item_id);

		if(m_t_item == null && m_item_id != 1)
		{
			return;
		}
		if(m_item_id == 1)
		{
			this.transform.GetComponent<UISprite>().atlas = m_atlas4;
			this.transform.GetComponent<UISprite>().spriteName = "small_xytz";
			this.transform.Find("type").gameObject.SetActive(false);
			this.transform.Find("sp").gameObject.SetActive(false);
			this.transform.Find("s1").gameObject.SetActive(false);
			this.transform.Find("s2").gameObject.SetActive(false);
			this.transform.Find("s3").gameObject.SetActive(false);
			this.transform.Find("s4").gameObject.SetActive(false);
			this.transform.Find("s5").gameObject.SetActive(false);
			this.transform.Find("s6").gameObject.SetActive(false);
			string text_ = "x" + m_item_num.ToString ();
			
			if (m_item_num <= 0)
			{
				text_ = "";
			}
			this.transform.Find("num").GetComponent<UILabel>().text = text_;
			this.transform.Find("bg").GetComponent<UISprite>().spriteName = "xtbk_chpt001";
			return;
		}
		if(m_atlas1.GetListOfSprites().Contains(m_t_item.icon))
		{
			this.transform.GetComponent<UISprite>().atlas = m_atlas1;
		}
		else
		{
			this.transform.GetComponent<UISprite>().atlas = m_atlas5;
		}
		if(m_item_id == 50010002 || m_t_item.type == 5003)
		{
			this.transform.GetComponent<UISprite>().atlas = m_atlas3;
		}
		this.transform.GetComponent<UISprite>().spriteName = m_t_item.icon;
		if(m_item_id == (int)e_huodong_item_id.ei_huodong_item1)
		{
			this.transform.GetComponent<UISprite>().atlas = m_atlas1;
			this.transform.GetComponent<UISprite>().spriteName = sys._instance.m_self.m_jieri_icon_name;
		}
		if(m_item_id == (int)e_huodong_item_id.ei_huodong_item2)
		{
			this.transform.GetComponent<UISprite>().atlas = m_atlas1;
			this.transform.GetComponent<UISprite>().spriteName = sys._instance.m_self.m_jieri_icon_name1;
		}
		if (m_t_item.type == 9001)
		{
			if (!type)
			{
				this.transform.Find("type").gameObject.SetActive(true);
				xuyao();
			}
			else
			{
				this.transform.Find("type").gameObject.SetActive(false);
			}
		}
		else if(type)
		{
			this.transform.Find("type").gameObject.SetActive(true);
		}
		else
		{
			this.transform.Find("type").gameObject.SetActive(false);
		}
        this.transform.Find("sp").gameObject.SetActive(false);
		this.transform.Find("s1").gameObject.SetActive(false);
		this.transform.Find("s2").gameObject.SetActive(false);
		this.transform.Find("s3").gameObject.SetActive(false);
		this.transform.Find("s4").gameObject.SetActive(false);
		this.transform.Find("s5").gameObject.SetActive(false);
		this.transform.Find("s6").gameObject.SetActive(false);

		if (m_t_item.type == 3001 || m_t_item.type == 9001)
		{
			s_t_class t_class = game_data._instance.get_t_class(m_t_item.def_1);
			this.transform.GetComponent<UISprite>().atlas = m_atlas;
			this.transform.GetComponent<UISprite>().spriteName = t_class.icon;
		}
		else if(m_t_item.type == 10001)
		{
			s_t_pet t_pet = game_data._instance.get_t_pet(m_t_item.def_1);
			this.transform.GetComponent<UISprite>().atlas = m_atlas;
			this.transform.GetComponent<UISprite>().spriteName = t_pet.icon;
		}
		else if (m_t_item.type == 6001)
		{
			s_t_baowu t_baowu = game_data._instance.get_t_baowu(m_t_item.def_1);
			this.transform.GetComponent<UISprite>().atlas = m_atlas4;
			this.transform.GetComponent<UISprite>().spriteName = t_baowu.icon;
			this.transform.Find("sp").gameObject.SetActive (true);
			string ss1 = "bwspzz_",ss2 = "00";
			switch(t_baowu.count)
			{
			case 3:
				ss1 += "ls";
				break;
			case 4:
				ss1 += "zs";
				break;
			case 5:
				ss1 += "js";
				break;
            case 6:
                ss1 += "hs";
				break;
			}
			string ss3 = m_t_item.id.ToString();
			ss2 += ss3[ss3.Length-1];
			
			this.transform.Find("sp").GetComponent<UISprite>().spriteName = ss1 + ss2;
		}
		else if (m_t_item.type == 7001)
		{
			s_t_equip t_equip = game_data._instance.get_t_equip(m_t_item.def_1);
			this.transform.GetComponent<UISprite>().atlas = m_atlas2;
			this.transform.GetComponent<UISprite>().spriteName = t_equip.icon;
		}

		string _text = "x" + m_item_num.ToString ();

		if (m_item_num <= 0)
		{
			_text = "";
		}

		if(m_max > 0)
		{
			if(m_item_num >= m_max)
			{
				_text = "[FFFF00]" +  m_item_num.ToString() + "[-]/" + m_max.ToString(); 
			}
			else
			{
				_text = "[FF0000]" +  m_item_num.ToString() + "[-]/" + m_max.ToString(); 
			}
		}
		else if(m_max == -1)
		{
			_text = "[FFFF00]" +  m_item_num.ToString() + "[-]/--"; 
		}

		this.transform.Find("num").GetComponent<UILabel>().text = _text;

		string s = "xtbk_lvpt001";
		//if (m_t_item.font_color == 1)


		if(m_t_item.type == 3001 || m_t_item.type == 7001 || m_t_item.type == 10001)
		{
			if (m_t_item.font_color == 1)
			{
				s = "xtbk_lvsp001";
				//s1 = "";
			}
			else if (m_t_item.font_color == 2)
			{
				s = "xtbk_lansp001";
				//s1 = "";
			}
			else if (m_t_item.font_color == 3)
			{
				s = "xtbk_zisp001";
				//s1 = "";
			}
			else if (m_t_item.font_color == 4)
			{
				s = "xtbk_chsp001";
				//s1 = "txkt_gl00";
			}
			else if (m_t_item.font_color == 5)
			{
				s = "xtbk_hosp001";
				//s1 = "txkt_gh00";
			}
			else if (m_t_item.font_color == 6)
			{
				s = "xtbk_jinsp001";
				//s1 = "txkt_rb00";
			}
		}
		else if (m_t_item.type == 9001)
		{
			if (m_t_item.font_color == 3)
			{
				s = "xtbk_zhy";
				//s1 = "";
			}
			else if (m_t_item.font_color == 4)
			{
				s = "xtbk_chy";
				//s1 = "txkt_gl00";
			}
			else if (m_t_item.font_color == 5)
			{
				s = "xtbk_hhy";
				//s1 = "txkt_gh00";
			}
		}
		else
		{
			if (m_t_item.font_color == 0)
			{
				s = "xtbk_fupt001";
				//s1 = "";
			}
			else if (m_t_item.font_color == 1)
			{
				s = "xtbk_lvpt001";
				//s1 = "";
			}
			else if (m_t_item.font_color == 2)
			{
				s = "xtbk_lanpt001";
				//s1 = "";
			}
			else if (m_t_item.font_color == 3)
			{
				s = "xtbk_zipt001";
				//s1 = "";
			}
			else if (m_t_item.font_color == 4)
			{
				s = "xtbk_chpt001";
				//s1 = "txkt_gl00";
			}
			else if (m_t_item.font_color == 5)
			{
				s = "xtbk_hopt001";
				//s1 = "txkt_gh00";
			}
			else if (m_t_item.font_color == 6)
			{
				s = "xtbk_jinpt001";
				//s1 = "txkt_rb00";
			}
		}
       // time();
		this.transform.Find("bg").GetComponent<UISprite>().spriteName = s;
	}

	void xuyao()
	{
		int xuyao = sys._instance.m_self.get_huiyi_xuyao(m_item_id);
		if (xuyao == 2)
		{
			this.transform.Find("type").GetComponent<UILabel>().text = game_data._instance.get_t_language ("item_icon.cs_428_68");//[ff0000]急需!
		}
		else if (xuyao == 1)
		{
			this.transform.Find("type").GetComponent<UILabel>().text = game_data._instance.get_t_language ("item_icon.cs_432_68");//[ffff00]需要
		}
		else
		{
			this.transform.Find("type").GetComponent<UILabel>().text = "";
		}
	}
}
