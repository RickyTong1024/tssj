
using UnityEngine;
using System.Collections;

public class main_icon : MonoBehaviour,IMessage {

	public GameObject m_effect;
	public GameObject m_time_show;
	public ulong m_time;
	public bool m_show_effect ;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
		if(m_effect != null)
		{
			m_effect.SetActive(false);
			m_show_effect = false;
		}
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void OnEnable()
	{
		if(m_time_show != null)
		{
			InvokeRepeating("time", 0.0f, 1.0f);
		}
	}
	
	void OnDisable()
	{
		if(m_time_show != null)
		{
			CancelInvoke ("time");
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "show_main_icon_effect" && this.transform.name == (string)message.m_string[0])
		{
			m_time = timer.now();
			m_effect.SetActive(true);
		}
		else if(message.m_type == "hide_main_icon_effect" && this.transform.name == (string)message.m_string[0])
		{
			ulong _time = 0;
			if (message.m_long.Count > 0)
			{
				_time = (ulong)message.m_long[0];
			}

			m_show_effect = true;
			m_time = timer.now() + _time;
			m_effect.SetActive(false);
		}
	}
	void time()
	{
		if(m_show_effect == false)
		{
			return ;
		}

		long _time = (long)m_time - (long)timer.now();

		if(_time > 0)
		{
			string _show = timer.get_time_show ((long)_time);
			m_time_show.GetComponent<UILabel>().text = _show;
		}
		else
		{
			string _show = timer.get_time_show (0);
			m_time_show.GetComponent<UILabel>().text = _show;
			m_effect.SetActive(true);
			m_show_effect = false;
		}
	}
}
