
using UnityEngine;
using System.Collections;

public class ui_bar_anim : MonoBehaviour {
	
	public GameObject m_bar;
	
	private double m_target_val = 0.0f;
	private double m_cur_val = 0.0f;
	public double m_speed = 1.0f;
	// Use this for initialization
	void Start () {

	}
	public void set_val(double target,double max)
	{
		m_target_val = target / max;
	}
	double abs(double val)
	{
		if(val < 0)
		{
			val = -val;
		}

		return val;
	}
	// Update is called once per frame
	void Update () 
	{
		double _add = Time.deltaTime * m_speed;

		if(abs(m_cur_val - m_target_val) > _add)
		{
			if(m_cur_val < m_target_val)
			{
				m_cur_val += _add;
			}
			else
			{
				m_cur_val -= _add;
			}
		}
		else
		{
			m_cur_val = m_target_val;
		}

		m_bar.GetComponent<UISprite>().fillAmount = (float)m_cur_val;
	}
}
