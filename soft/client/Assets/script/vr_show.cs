
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class vr_show : MonoBehaviour,IMessage {

	public GameObject m_vr;
	public GameObject m_cam;
	private GameObject m_unit;
	private Vector3 m_mouse_pos = new Vector3();
	private bool m_press = false;
	private RaycastHit m_rayhit;
	private ulong m_guid;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);

		if(m_unit != null)
		{
			GameObject.Destroy(m_unit);
		}
	}

	void OnEnable()
	{
		if(m_unit != null)
		{
			s_message _new_msg = new s_message();
			
			_new_msg.m_type = "vr_show_unit";
			_new_msg.m_long.Add(m_guid);
			
			cmessage_center._instance.add_message(_new_msg);
		}
	}

	void OnDisable()
	{
		if(m_unit != null)
		{
			m_unit.SetActive(false);
		}
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "show_vr")
		{
			m_cam.SetActive(false);
			m_vr.SetActive(true);
		}
		
		if(message.m_type == "hide_vr")
		{
			m_cam.SetActive(true);
			m_vr.SetActive(false);
		}

		if(message.m_type == "vr_show_unit")
		{
			if(m_unit != null)
			{
				GameObject.Destroy(m_unit);
			}

			m_guid = (ulong)message.m_long[0];
			if (m_guid == (ulong)99)
			{
                m_unit = sys._instance.create_mm();
                m_unit.transform.localPosition = new Vector3(0, 0.05f, 0);
                m_unit.transform.localEulerAngles = new Vector3(0, 180, 0);
                m_unit.GetComponent<face>().m_target_rot = 180;
				m_unit.GetComponent<face>().m_free = true;

				s_message _msg = new s_message();
				_msg.m_type = "show_wd";
				cmessage_center._instance.add_message(_msg);
			}
			else
			{
				ccard card = sys._instance.m_self.get_card_guid(m_guid);
				m_unit = sys._instance.create_class(card, 0);
				m_unit.transform.localEulerAngles = new Vector3(0,180,0);

				s_message _msg = new s_message();
				_msg.m_type = "hide_wd";
				cmessage_center._instance.add_message(_msg);
			}
		}
	}
	
	void IMessage.net_message(s_net_message message)
	{
		
	}

	// Update is called once per frame
	void Update () {
	
		Ray _ui_ray = root_gui._instance.get_ui_cam().ScreenPointToRay(Input.mousePosition);
		
		if(sys._instance.get_mouse_button(0) && Physics.Raycast(_ui_ray, out m_rayhit) == false)
		{
			if(m_press == false)
			{
				m_press = true;
			}
			else
			{
				
				Vector3 _angle = m_cam.transform.eulerAngles;
				
				_angle.y -= (m_mouse_pos.x - sys._instance.get_mouse_position().x) * 0.2f;
				_angle.x -= (m_mouse_pos.y - sys._instance.get_mouse_position().y) * 0.2f;
				
				//if(_angle.x < 110f && _angle.x > 60f)
				{
					m_cam.transform.eulerAngles = _angle;
				}
			}
			
			m_mouse_pos =  sys._instance.get_mouse_position();
		}
		else
		{
			m_press = false;
		}
	}
}
