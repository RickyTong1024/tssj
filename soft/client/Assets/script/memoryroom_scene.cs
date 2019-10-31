using UnityEngine;
using System.Collections;

public class memoryroom_scene : MonoBehaviour,IMessage {

	private GameObject m_mm;
	public GameObject m_sn;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
		s_message _message = new s_message();
		_message.m_type = "hide_mm_show_gui";
		cmessage_center._instance.add_message(_message);

		_message = new s_message();
		_message.m_type = "hide_main_gui_0";
		cmessage_center._instance.add_message(_message);
		
		if(m_mm != null)
		{
			for(int i = 0;i < sys._instance.m_self.m_t_player.dress_on_ids.Count;i ++)
			{
				s_t_dress _dress = game_data._instance.get_t_dress((int)sys._instance.m_self.m_t_player.dress_on_ids[i]);
				
				if(_dress.type == 2)
				{
					m_mm.GetComponent<unit>().change_part(_dress.res,0);
				}
			}
			
			m_mm.GetComponent<face>().enabled = false;
			return;
		}

        m_mm = sys._instance.create_mm(m_sn);
		m_mm.GetComponent<face>().enabled = false;
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
		s_message _message = new s_message();
		_message.m_type = "show_mm_show_gui";
		cmessage_center._instance.add_message(_message);

		_message = new s_message();
		_message.m_type = "show_main_gui_0";
		cmessage_center._instance.add_message(_message);
	}
	
	void IMessage.net_message(s_net_message message)
	{
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "show_memory_scene")
		{
			m_sn.SetActive(true);
		}
		if (message.m_type == "hide_memory_scene")
		{
			m_sn.SetActive(false);
		}
	}
}
