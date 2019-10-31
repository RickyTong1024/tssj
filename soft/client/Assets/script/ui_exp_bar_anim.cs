
using UnityEngine;
using System.Collections;

public class ui_exp_bar_anim : MonoBehaviour,IMessage {
	
	public GameObject m_bar;

	private int m_type;
	private int m_level;
	private int m_max_level;
	private float m_cur_exp;
	private float m_tar_exp;
	private float m_max_exp;
	private float m_targer_exp;
	public float m_speed = 10.0f;
	public float m_wait = 0.0f;
	public GameObject m_label;
	public GameObject m_uplevel_effect;

	public delegate void levelup ();

	public levelup m_levelup;

	public ccard m_card;
	// Use this for initialization
	void Start () {

		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.net_message(s_net_message message)
	{
		
	}
	
	void IMessage.message(s_message message)
	{
	
	}

	public void set_property(int type,int max_level,int level,int cur_exp,int add_exp)
	{
		m_type = type;
		m_level = level;
		m_max_level = max_level;
		m_cur_exp = cur_exp;
		m_tar_exp = m_cur_exp + add_exp;

		if(m_label != null)
		{
            m_label.GetComponent<UILabel>().text = "Lv " + m_level;
		}

		s_t_exp _exp = game_data._instance.get_t_exp (level + 1);
		
		if(_exp == null)
		{
			return;
		}
		
		if(type == 0)
		{
			m_max_exp = _exp.exp;
		}
		else
		{
			m_max_exp = _exp.role_exp;
		}

		bar ();
	}
	void bar()
	{
		float _value = m_cur_exp / m_max_exp;

		/*
		if(m_cur_exp < 1.0f)
		{
			if(m_bar.GetComponent<Renderer>().enabled == true)
			{
				m_bar.GetComponent<Renderer>().enabled = false;
			}

			return ;
		}
		else if(m_bar.GetComponent<Renderer>().enabled == false)
		{
			m_bar.GetComponent<Renderer>().enabled = true;
		}
		*/

		UIProgressBar _bar = m_bar.GetComponent<UIProgressBar>();
		
		if(_bar != null)
		{
			_bar.value = m_cur_exp / m_max_exp;

			if(m_cur_exp < 1.0f)
			{
				_bar.alpha = 0.0f;
			}
			else
			{
				_bar.alpha = 1.0f;
			}
		}
		else 
		{
			m_bar.GetComponent<UISprite>().fillAmount = m_cur_exp / m_max_exp;

			if(m_cur_exp < 1.0f)
			{
				m_bar.GetComponent<UISprite>().alpha = 0.0f;
			}
			else
			{
				m_bar.GetComponent<UISprite>().alpha = 1.0f;
			}
		}
	}
	// Update is called once per frame
	void Update () 
	{
		if(m_wait > 0)
		{
			m_wait -= Time.deltaTime;
			return;
		}

		if(m_level < m_max_level)
		{
			if(m_cur_exp != m_tar_exp)
			{ 
				float _add = Time.deltaTime * m_max_exp * m_speed;

				if(Mathf.Abs(m_cur_exp - m_tar_exp) > _add)
				{
					if(m_cur_exp > m_tar_exp)
					{
						m_cur_exp -= _add;
					}
					else if(m_cur_exp < m_tar_exp)
					{
						m_cur_exp += _add;
					}
				}
				else
				{
					m_cur_exp = m_tar_exp;
				}

				bar ();
			}
			
			if(m_cur_exp >= m_max_exp)
			{
				m_cur_exp = 0;
				
				m_tar_exp = m_tar_exp - m_max_exp;
				
				m_level ++;

				if(m_level < m_max_level)
				{
					set_property((int)m_type,m_max_level,(int)m_level,(int)m_cur_exp,(int)m_tar_exp);

					if(m_uplevel_effect != null)
					{
						m_uplevel_effect.SetActive(true);
					}

					if(m_levelup != null)
					{
						m_levelup();
					}
				}
				else
				{
					m_tar_exp = 0;
					m_cur_exp = 0;

					set_property((int)m_type,m_max_level,(int)m_level,(int)m_cur_exp,(int)m_tar_exp);
				}
			}
		}

		UIProgressBar _bar = m_bar.GetComponent<UIProgressBar>();

		if(_bar != null)
		{
			if(m_cur_exp < 1.0f && _bar.alpha != 0.0f)
			{
				_bar.alpha = 0.0f;
			}
			else if(m_cur_exp > 0.0f && _bar.alpha != 1.0f)
			{
				_bar.alpha = 1.0f;
			}
		}
		else
		{
			if(m_cur_exp < 1.0f && m_bar.GetComponent<UISprite>().alpha != 0.0f)
			{
				m_bar.GetComponent<UISprite>().alpha = 0.0f;
			}
			else if(m_cur_exp > 0.0f && m_bar.GetComponent<UISprite>().alpha != 1.0f)
			{
				m_bar.GetComponent<UISprite>().alpha = 1.0f;
			}
		}

	}
}
