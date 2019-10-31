using UnityEngine;

public class dress_detail : MonoBehaviour,IMessage {

	private s_t_dress m_dress;
	private int m_type;
	public GameObject m_name;
	public GameObject m_attr;
	public GameObject m_attr1;
	public GameObject m_icon;
	public GameObject m_buy;
	public GameObject m_dress_on;
	public GameObject is_need;
	public GameObject m_lock;
	public GameObject m_tishi;
	public GameObject m_tex;

	public UILabel m_shuxing;
	public UILabel m_shuoming;
	public UILabel m_genghuan;
	public UILabel m_goumai;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	public void reset(s_t_dress _dress, int type)
	{
		m_buy.SetActive (false);
		m_dress_on.SetActive(false);
		m_tishi.SetActive(false);
		m_type = type;
		m_dress = _dress;
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(_dress.color) +  m_dress.name + "";
		m_attr.GetComponent<UILabel>().text = "";
		bool flag = false;
		string sx = "";
		for(int i = 0; i < m_dress.attrs.Count;i++)
		{
			if (m_dress.attrs[i].attr > 0)
			{
				if(flag)
				{
					sx += "\n" +game_data._instance.get_value_string (m_dress.attrs[i].attr, m_dress.attrs[i].value,1);
				}
				else
				{
					sx += game_data._instance.get_value_string (m_dress.attrs[i].attr, m_dress.attrs[i].value,1);
				}
				flag = true;
			}
		}
		m_attr.GetComponent<UILabel>().text = "[0aabff]" + sx;
		if(!flag)
		{
            m_attr.GetComponent<UILabel>().text = "[0aabff]" + game_data._instance.get_t_language ("dress_detail_gui_soft14");//该时装无属性加成
		}
		m_icon.GetComponent<UISprite>().spriteName = m_dress.icon.ToString ();
		is_need.transform.GetComponent<UISprite>().spriteName = "ycd_word03";
		if (sys._instance.m_self.has_dress(m_dress.id))
		{
			is_need.transform.GetComponent<UISprite>().spriteName = "ycd_word02";
		} 
		 if (sys._instance.m_self.has_dress_on(m_dress.id))
		{
			is_need.transform.GetComponent<UISprite>().spriteName = "ycd_word01";
		}

		if (!sys._instance.m_self.has_dress(m_dress.id)) 
		{
			//m_dress_on.SetActive(false);
			m_lock.SetActive(true);
			m_dress_on.SetActive (false);
			m_buy.SetActive (true);
			m_buy.GetComponent<BoxCollider>().enabled = false;
			m_buy.GetComponent<UISprite>().set_enable(false);
			s_t_dress_unlock _t_dress_unlock  = null;
			for(int i = 0; i < game_data._instance.m_dbc_dress_unlock.get_y();++i)
			{
				int id = int.Parse(game_data._instance.m_dbc_dress_unlock.get(0,i));
				_t_dress_unlock = game_data._instance.get_t_dress_unlock(id);
				if(m_dress.id == _t_dress_unlock.sz_id)
				{
					break;
				}
			}
			if(sys._instance.m_self.m_t_player.dress_tuzhi >= _t_dress_unlock.tz_num)
			{
				m_buy.GetComponent<BoxCollider>().enabled = true;
				m_buy.GetComponent<UISprite>().set_enable(true);
			}
            m_attr1.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("dress_detail_gui_soft16"), _t_dress_unlock.tz_num);	//解锁需要时装设计图{0}个	
		} 
		else
		{
			m_buy.SetActive (false);
			m_lock.SetActive(false);
			m_dress_on.SetActive (true);
            m_dress_on.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_detail_gui_soft11"); //更换时装
		}

		if(sys._instance.m_self.has_dress(m_dress.id))
		{
			if ( !sys._instance.m_self.has_dress_on(m_dress.id))
			{
				if(is_type_dress(m_dress))
				{
					m_dress_on.name = "dress_on";
                    m_dress_on.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_detail_gui_soft11"); //更换时装
					m_dress_on.SetActive (true);
					m_dress_on.GetComponent<BoxCollider>().enabled = true;
					m_dress_on.GetComponent<UISprite>().set_enable(true);
				}
				else
				{
					m_dress_on.name = "dress_on";
                    m_dress_on.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_detail_gui_soft12");//装备时装
					m_dress_on.SetActive (true);
					m_dress_on.GetComponent<BoxCollider>().enabled = true;
					m_dress_on.GetComponent<UISprite>().set_enable(true);
				}
						
			}
			else
			{
				if(m_dress.type > 2)
				{
					m_dress_on.name = "dress_off";
                    m_dress_on.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_detail_gui_soft13"); //卸下时装
					m_dress_on.SetActive (true);
					m_dress_on.GetComponent<BoxCollider>().enabled = true;
					m_dress_on.GetComponent<UISprite>().set_enable(true);
				}
				else
				{
					m_dress_on.SetActive (true);
                    m_dress_on.transform.Find("Label").GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_detail_gui_soft12");//装备时装
					m_dress_on.GetComponent<BoxCollider>().enabled = false;
					m_dress_on.GetComponent<UISprite>().set_enable(false);	
				}
			
			}
            m_attr1.GetComponent<UILabel>().text = game_data._instance.get_t_language ("dress_detail_gui_soft17");//该时装已解锁
		}
		m_tex.GetComponent<UISprite>().spriteName = chengjiu_item.icon_name (_dress);
	}
	public bool is_type_dress(s_t_dress t )
	{
		for(int i = 0;i < sys._instance.m_self.m_t_player.dress_on_ids.Count; i++)
		{
			s_t_dress s = game_data._instance.get_t_dress(sys._instance.m_self.m_t_player.dress_on_ids[i]);
			if(t.type == s.type)
			{
				return true;
			}
		}
		return false;
	}
	void Update () 
	{
		
	}
	
	void IMessage.net_message(s_net_message message)
	{

	}
	void IMessage.message(s_message message)
	{
		
	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "ok")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.name == "goumai")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message msg1 = new s_message();
			msg1.m_type = "dress_buy";
			msg1.m_object.Add (m_dress);
			cmessage_center._instance.add_message(msg1);
		}
		else if (obj.name == "dress_on")
		{
			s_message _msg = new s_message();
			_msg.m_type = "dress_on";
			_msg.m_object.Add (m_dress);
			cmessage_center._instance.add_message(_msg);
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.transform.name == "dress_off")
		{
			s_message _msg = new s_message();
			_msg.m_type = "dress_off";
			_msg.m_object.Add (m_dress);
			cmessage_center._instance.add_message(_msg);
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}


	}
}
