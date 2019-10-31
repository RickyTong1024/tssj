
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class jc_huodong_sub : MonoBehaviour {

	public int m_id;
	public int m_huodong_id = 0;
	public GameObject m_name;
	public GameObject m_text;
	public GameObject m_time;
	public GameObject m_effect;
	public GameObject m_jc_huodong_gui;
	private s_t_jc_huodong m_t_jc_huodong;
	public ulong m_end_time = 0;
	public string huodong_name = "";
	// Use this for initialization
	void Start () {

	}

	void OnEnable()
	{
		InvokeRepeating ("time", 0.0f, 1.0f);
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}

	public void reset()
	{
		m_t_jc_huodong = game_data._instance.get_t_jc_huodong(m_id);
		m_name.GetComponent<UILabel>().text = huodong_name;
		if(m_id == 2 || m_id == 9)
		{
			m_name.GetComponent<UILabel>().text = m_t_jc_huodong.name;
		}
		string[] s = m_t_jc_huodong.name_color.Split (' ');
		m_name.GetComponent<UILabel>().color = new Color (int.Parse(s[0]) / 255.0f, int.Parse(s[1]) / 255.0f, int.Parse(s[2]) / 255.0f);
		s = m_t_jc_huodong.name_mb.Split (' ');
		m_name.GetComponent<UILabel>().effectColor = new Color (int.Parse(s[0]) / 255.0f, int.Parse(s[1]) / 255.0f, int.Parse(s[2]) / 255.0f);
		m_text.GetComponent<UILabel>().text = m_t_jc_huodong.text;
		s = m_t_jc_huodong.text_color.Split (' ');
		m_text.GetComponent<UILabel>().color = new Color (int.Parse(s[0]) / 255.0f, int.Parse(s[1]) / 255.0f, int.Parse(s[2]) / 255.0f);
		s = m_t_jc_huodong.text_mb.Split (' ');
		m_text.GetComponent<UILabel>().effectColor = new Color (int.Parse(s[0]) / 255.0f, int.Parse(s[1]) / 255.0f, int.Parse(s[2]) / 255.0f);
		s = m_t_jc_huodong.time_color.Split (' ');
		m_time.GetComponent<UILabel>().color = new Color (int.Parse(s[0]) / 255.0f, int.Parse(s[1]) / 255.0f, int.Parse(s[2]) / 255.0f);
		s = m_t_jc_huodong.time_mb.Split (' ');
		m_time.GetComponent<UILabel>().effectColor = new Color (int.Parse(s[0]) / 255.0f, int.Parse(s[1]) / 255.0f, int.Parse(s[2]) / 255.0f);
		this.GetComponent<UISprite>().spriteName = m_t_jc_huodong.res;
		m_effect.SetActive (m_jc_huodong_gui.GetComponent<jc_huodong_gui>().is_effect(m_id));
	}

	void time()
	{
		if(m_end_time == 0)
		{
			m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jc_huodong_sub.cs_61_41");//永久活动
			if(m_id == 13)
			{
				long _time = 0;
				int run_day = timer.run_day (timer.start_time_) + 1;
				if (run_day <= 7)
				{
					_time = (7 - run_day) * 86400000 + timer.last_time_today();
				}
				string _text = timer.get_time_show_ex(_time);
				m_time.GetComponent<UILabel>().text = _text;
				if(_time <= 0)
				{
					m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jc_huodong_sub.cs_74_43");//活动已经结束
				}
			}
		}
		else
		{
			if(m_id == 15 && m_end_time < timer.now())
			{

				long _time1 = (long)(m_end_time + (ulong)(25*24*60*60)*(1000) - timer.now());
				m_time.GetComponent<UILabel>().text = timer.get_time_show_ex(_time1);
			}
			else
			{
			    long _time = (long)(m_end_time - timer.now());
			    m_time.GetComponent<UILabel>().text = timer.get_time_show_ex(_time);
			}
		}
	}

	public void click(GameObject obj)
	{

		if (m_t_jc_huodong.message != "0")
		{
			s_message mes = new s_message();
			mes.m_type = m_t_jc_huodong.message;
			mes.m_ints.Add(m_huodong_id);
            mes.m_string.Add(m_name.GetComponent<UILabel>().text);
            mes.m_ints.Add(m_id);
            mes.m_bools.Add(false);
			cmessage_center._instance.add_message(mes);

            //jc_huodong_gui._instance.m_select_p.transform.localPosition = this.transform.localPosition;
            //jc_huodong_gui._instance.m_select_p.SetActive(true);
		}
		else
		{
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
		}
	}

	// Update is called once per frame
	void Update () 
	{

	}
}
