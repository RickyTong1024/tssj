
using UnityEngine;
using System.Collections;

public class resource_icon : MonoBehaviour {

	public int m_type;
	public int m_num;
	public int m_max;
	public string m_out_message;
	string desc;
    s_t_resource res;
	// Use this for initialization
	void Start () {

	}
	public void press()
	{

	}
	public void release()
	{
		s_message _message = new s_message ();
		
		_message.m_type = "hide_min_dialog_box";
		
		cmessage_center._instance.add_message (_message);
	}

	public void click()
	{
		if (m_out_message.Length == 0)
		{
			s_message _message = new s_message ();
			
			_message.m_type = "item_dialog_box";
			_message.m_ints.Add (m_type);
			_message.m_ints.Add (1);
			_message.m_string.Add (desc);
			cmessage_center._instance.add_message (_message);
			return;
		}
	}

	public void init()
	{
		m_type = 0;
		m_num = 0;
		m_max = 0;
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
		string s = m_num.ToString ();
		if (m_num == 0)
		{
			s = game_data._instance.get_t_language ("resource_icon.cs_65_7");//一定
		}
         res = game_data._instance.get_t_resource(m_type);
		desc = res.desc;
		transform.GetComponent<UISprite>().spriteName = res.icon;
        if (res.color == 1)
        {
            s = "xtbk_lvpt001";
        }
        else if (res.color == 2)
        {
            s = "xtbk_lanpt001";
        }
        else if (res.color == 3)
        {
            s = "xtbk_zipt001";
        }
        else if (res.color == 4)
        {
            s = "xtbk_chpt001";
        }
        else if (res.color == 5)
        {
            s = "xtbk_jinpt001";
        }
        else if (res.color == 6)
        {
            s = "xtbk_hopt001";
        }
        transform.Find("kuang").GetComponent<UISprite>().spriteName = s;
        string _text = "x" + sys._instance.value_to_wan(m_num);
		if (m_num <= 0)
		{
			_text = "";
		}
		if(m_max > 0)
		{
			if(m_num >= m_max)
			{
                _text = "[FFFF00]" + sys._instance.value_to_wan(m_num) + "[-]/" + sys._instance.value_to_wan(m_max); 
			}
			else
			{
                _text = "[FF0000]" + sys._instance.value_to_wan(m_num) + "[-]/" + sys._instance.value_to_wan(m_max); 
			}
		}
		transform.Find("num").GetComponent<UILabel>().text = _text;
	}
}
