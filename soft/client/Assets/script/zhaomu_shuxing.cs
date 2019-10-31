
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class zhaomu_shuxing : MonoBehaviour {

	public GameObject m_star;
	public GameObject m_name;
	public GameObject m_has;
	public GameObject m_back;
	public GameObject m_back1;
	bool m_flag;
	float m_shake;
	s_message m_mes;
	float m_time;
	ccard m_card;

	void Start () {
		m_has.GetComponent<UILabel>().text = game_data._instance.get_t_language ("zhaomu_shuxing.cs_19_40");//您已经拥有该伙伴，将转化为6个基因
	}

	public void init(ccard card, s_message mes, bool flag)
	{
		m_flag = flag;
		m_card = card;
		m_mes = mes;
		m_name.transform.GetComponent<UILabel>().text = card.get_color_name();
		set_star (card.get_color());

		s_message _message = new s_message();
		_message.m_type = "reward_show";
		_message.m_long.Add(m_card.get_guid());
		cmessage_center._instance.add_message (_message);
	}

	public void set_star(int num)
	{
		int _num = num;
		string text = "";
		if(_num == 1)
		{
			text = game_data._instance.get_t_language ("zhaomu_shuxing.cs_42_10");//绿色伙伴
		}
		else if(_num == 2)
		{
			text = game_data._instance.get_t_language ("zhaomu_shuxing.cs_46_10");//蓝色伙伴
		}
		else if(_num == 3)
		{
			text = game_data._instance.get_t_language ("zhaomu_shuxing.cs_50_10");//紫色伙伴
		}
		else if(_num == 4)
		{
			text = game_data._instance.get_t_language ("zhaomu_shuxing.cs_54_10");//橙色伙伴
		}
		else if(_num == 5)
		{
			text = game_data._instance.get_t_language ("zhaomu_shuxing.cs_58_10");//红色伙伴
		}
		else if(_num == 6)
		{
			text = game_data._instance.get_t_language ("zhaomu_shuxing.cs_62_10");//金色伙伴
		}
		float _delay = 1.5f;
		m_time = _delay + 4 * 0.2f + 0.5f;
		m_back.GetComponent<UISprite>().alpha = 0.0f;
		sys._instance.add_alpha_anim(m_back, 0.1f, 0, 1, _delay);
		/*for(int i = 0;i < text.Length;i ++)
		{
			if(i >= _num)
			{
				m_star[i].SetActive(false);
			}
			else
			{
				m_star[i].SetActive(true);

				m_star[i].transform.localPosition = new Vector3(-50 * (num - 1) + 100 * i, 115, 0);

				m_star[i].GetComponent<UISprite>().alpha = 0.0f;
				sys._instance.add_alpha_anim(m_star[i], 0.1f, 0, 1, _delay + i * 0.2f);
				
				m_star[i].transform.localScale = new Vector3(7,7,7);
				sys._instance.add_scale_anim(m_star[i], 0.1f, 7, 1, _delay + i * 0.2f);
				
				TweenScale _scale = m_star[i].GetComponent<TweenScale>();
				
				EventDelegate.Add(_scale.onFinished, delegate() 
				                  {
					m_shake = 6;
					sys._instance.play_sound("sound/mcx20070511");
				});
				
				m_star[i].GetComponent<TweenScale>().enabled = true;
			}
			string  s = "";
			s = game_data._instance.get_name_color(num) + text[i];
			m_star[i].GetComponent<UILabel>().text = s;
			m_star[i].GetComponent<UILabel>().alpha = 0.0f;
			sys._instance.add_alpha_anim(m_star[i], 0.1f, 0, 1, _delay + i * 0.2f);
			
			m_star[i].transform.localScale = new Vector3(7,7,7);
			sys._instance.add_scale_anim(m_star[i], 0.1f, 7, 1, _delay + i * 0.2f);
			
			TweenScale _scale = m_star[i].GetComponent<TweenScale>();
			
			EventDelegate.Add(_scale.onFinished, delegate() 
			                  {
				m_shake = 6;
				sys._instance.play_sound("sound/mcx20070511");
			});
			
			m_star[i].GetComponent<TweenScale>().enabled = true;

		}*/
		string  s = "";
		s = game_data._instance.get_name_color(num) + text;
		m_star.GetComponent<UILabel>().text = s;
		m_star.GetComponent<UILabel>().alpha = 0.0f;
		sys._instance.add_alpha_anim(m_star, 0.1f, 0, 1, _delay);
		
		m_star.transform.localScale = new Vector3(7,7,7);
		sys._instance.add_scale_anim(m_star, 0.1f, 7, 1, _delay);
		
		TweenScale _scale = m_star.GetComponent<TweenScale>();
		
		EventDelegate.Add(_scale.onFinished, delegate() 
		                  {
			m_shake = 6;
			sys._instance.play_sound("sound/mcx20070511");
		});
		m_star.GetComponent<TweenScale>().enabled = true;
		m_back1.GetComponent<UISprite>().alpha = 0.0f;
		sys._instance.add_alpha_anim(m_back1, 0.1f, 0, 1, _delay + (text.Length -1) * 0.2f);
		m_name.transform.localPosition = new Vector3(0, 30, 0);
		m_name.GetComponent<UILabel>().alpha = 0.0f;
		sys._instance.add_alpha_anim(m_name, 0.1f, 0, 1, _delay + num * 0.2f + 0.3f);
		
		m_name.transform.localScale = new Vector3(7,7,7);
		sys._instance.add_scale_anim(m_name, 0.1f, 7, 1, _delay + num * 0.2f + 0.3f);
		
		TweenScale _scale1 = m_name.GetComponent<TweenScale>();
		EventDelegate.Add(_scale1.onFinished, delegate() 
		                  {
			m_shake = 10;
			sys._instance.play_sound("sound/mcx20070511");
		});

		if (m_flag)
		{
			m_has.SetActive(true);
			m_has.transform.localPosition = new Vector3(0, 200, 0);
			m_has.GetComponent<UILabel>().alpha = 0.0f;
			sys._instance.add_alpha_anim(m_has, 0.1f, 0, 1, _delay + num * 0.2f + 0.6f);
			
			m_has.transform.localScale = new Vector3(7,7,7);
			sys._instance.add_scale_anim(m_has, 0.1f, 7, 1, _delay + num * 0.2f + 0.6f);
			
			TweenScale _scale2 = m_has.GetComponent<TweenScale>();
			EventDelegate.Add(_scale2.onFinished, delegate() 
			                  {
				m_shake = 10;
				sys._instance.play_sound("sound/mcx20070511");
			});
		}
		else
		{
			m_has.SetActive(false);
		}
	}

	public void click(GameObject obj)
	{
		if (obj.name == "main")
		{
			if (m_time > 0)
			{
				return;
			}
			this.gameObject.SetActive(false);

			s_message _message = new s_message();
			_message.m_type = "scene_show";
			cmessage_center._instance.add_message(_message);

			cmessage_center._instance.add_message(m_mes);
		}
	}

	void Update() {
		if (m_shake > 0)
		{
			float _addx = Random.Range(-m_shake, m_shake);
			float _addy = Random.Range(-m_shake, m_shake);
			float _addz = Random.Range(-m_shake, m_shake);
			
			this.transform.localPosition = new Vector3(_addx, _addy, _addz);
			
			m_shake -= Time.deltaTime * m_shake * 10.0f;

			if (m_shake <= 0)
			{
				this.transform.localPosition = new Vector3(0, 0, 0);
			}
		}
		if (m_time > 0)
		{
			m_time -= Time.deltaTime;
			if (m_time <= 0 && m_card.m_t_class.sound != "0")
			{
				sys._instance.play_sound_ex("sound/ts_rwpy/" + m_card.m_t_class.sound);
			}
		}
	}
}
