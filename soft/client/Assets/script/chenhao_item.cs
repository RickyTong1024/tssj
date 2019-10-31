
using UnityEngine;
using System.Collections;

public class chenhao_item : MonoBehaviour {

	public int m_id = 0;
	public GameObject m_name;
	public GameObject m_icon;
	public GameObject m_desc;
	public GameObject m_sx;
	public GameObject m_time;
	public GameObject m_pd;
	public GameObject m_pd_button;
	public GameObject m_hq_button;
	public GameObject m_jh_button;

	// Use this for initialization
	void Start () {

	}


	public void reset()
	{
		m_pd.SetActive (false);
		m_hq_button.SetActive (false);
		m_pd_button.SetActive (false);
		m_hq_button.SetActive (false);
		m_jh_button.SetActive (false);
		s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao (m_id);
		string name = t_chenghao.name.Remove(0,8);
		string color = "";
		string text = "";
		for(int i = 0;i < t_chenghao.attr.Count;++i)
		{
			if(i == t_chenghao.attr.Count - 1)
			{
				text += game_data._instance.get_value_string(t_chenghao.attr[i].attr,t_chenghao.attr[i].value);
			}
			else
			{
				text += game_data._instance.get_value_string(t_chenghao.attr[i].attr,t_chenghao.attr[i].value) + " ";
			}
		}
		if(text == "")
		{
			text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_187_79");//无属性加成
		}
		m_desc.GetComponent<UILabel>().text = t_chenghao.condition;
		if(!sys._instance.m_self.m_t_player.chenghao.Contains(t_chenghao.id))
		{
			color = "[777777]";
			m_name.GetComponent<UILabel>().effectColor = new Color(0,0,0);
			m_name.GetComponent<UILabel>().text = color + name;
			m_icon.GetComponent<UISprite>().spriteName = t_chenghao.icon2;
			m_time.SetActive(false);
			m_hq_button.SetActive(true);
		}
		else
		{
			m_name.GetComponent<UILabel>().effectColor = sys._instance.get_chenghao_color(t_chenghao.id);
			m_name.GetComponent<UILabel>().text = t_chenghao.name;
			m_icon.GetComponent<UISprite>().spriteName = t_chenghao.icon1;
			if(t_chenghao.time > 0)
			{
				long time = 0;
				for(int i = 0; i < sys._instance.m_self.m_t_player.chenghao.Count;++i)
				{
					if(t_chenghao.id == sys._instance.m_self.m_t_player.chenghao[i])
					{
						time = (long)sys._instance.m_self.m_t_player.chengchao_time[i] - (long)timer.now();
						break;
					}
				}
				int tt = (int)(time / 1000);
				int day = tt / 3600 / 24;
				m_time.SetActive(true);
				m_time.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("chenhao_item.cs_78_56") , day );//剩余时间:{0}天
			}
			else
			{
				m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chenhao_item.cs_82_42");//永久称号
			}
			if(sys._instance.m_self.m_t_player.chenghao_on == t_chenghao.id)
			{
				m_pd.SetActive(true);
			}
			else
			{
				m_pd_button.SetActive(true);
			}
			s_t_chenghao _t_chenghao = game_data._instance.get_t_chenhao (sys._instance.m_self.m_t_player.chenghao_on);
			/*if(_t_chenghao != null && t_chenghao.id > sys._instance.m_self.m_t_player.chenghao_on && t_chenghao.type == _t_chenghao.type)
			{
				text = game_data._instance.get_t_language ("chenhao_item.cs_95_11");//已激活更高级别称号的加成效果
			}*/
		}
		m_sx.GetComponent<UILabel>().text = text;
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "jh")
		{

		}
		if(obj.transform.name == "hq")
		{
			s_t_chenghao t_chenghao = game_data._instance.get_t_chenhao (m_id);
			int type = t_chenghao.type;
			s_message _message = new s_message ();
			s_message _message1 = new s_message ();
			switch(type)
			{
			case 2:
                   _message1.m_type = "hide_chenghao_gui";
				cmessage_center._instance.add_message(_message1);

				_message.m_type = "show_guild_hongbao";
				_message.m_ints.Add(1);
				cmessage_center._instance.add_message(_message);
				break;
			case 3:
                _message1.m_type = "hide_chenghao_gui";
				cmessage_center._instance.add_message(_message1);

				_message.m_type = "show_guild_hongbao";
				_message.m_ints.Add(0);
				cmessage_center._instance.add_message(_message);
				break;
			case 4:
				if(sys._instance.m_self.m_t_player.level < (int)e_open_level.el_pvp)
				{
					root_gui._instance.show_prompt_dialog_box("[ffc882]" + string.Format(game_data._instance.get_t_language ("chenhao_gui.cs_182_74") , (int)e_open_level.el_pvp ));//猎人大会{0}级开启
					return;
				}
				_message1.m_type = "hide_chenghao_gui";
				cmessage_center._instance.add_message(_message1);

				_message.m_type = "show_huo_dong";
				_message.m_ints.Add(10);
				cmessage_center._instance.add_message(_message);
				break;
			}
		}
		if(obj.transform.name == "pd")
		{
			s_message _message2 = new s_message();
			_message2.m_type = "refresh_chenghao";
			_message2.m_ints.Add(m_id);
			cmessage_center._instance.add_message(_message2);
		}
	}
	

	public void select(GameObject obj)
	{
		s_message _message2 = new s_message();
		_message2.m_type = "click_chenghao_item";
		_message2.m_ints.Add(m_id);
		cmessage_center._instance.add_message(_message2);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
