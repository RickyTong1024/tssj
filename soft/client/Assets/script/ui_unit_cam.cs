
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ui_unit_cam : MonoBehaviour,IMessage {
    
    public GameObject m_cam;
	public GameObject m_obj;
	public GameObject m_pet_obj;
	private GameObject m_unit;
	private GameObject m_pet_unit;
	private int m_end = 0;
	private RaycastHit m_rayhit;
	private Vector3 m_old_pos = new Vector3();
	private float m_rot = 0;
    private bool m_can_rot = true;
	private GameObject m_mm;
	private List<GameObject> m_units = new List<GameObject>();
    void Start () {
		cmessage_center._instance.add_handle (this);
		
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	private GameObject add_unit(ccard _card)
	{
		for (int i = 0; i < m_units.Count;) 
		{
			if(m_units[i] != null && m_units[i].GetComponent<unit>() != null)
			{
				if(m_units[i].GetComponent<unit>().m_card.get_template_id() == _card.get_template_id())
				{
					return m_units[i];
				}
				i ++;
			}
			else
			{
				m_units.RemoveAt(i);
			}
		}

        GameObject _unit = sys._instance.create_class(_card, 0, m_obj);
        m_units.Add(_unit);
		return _unit;
	}

    private void clear_unit()
    {
        for (int i = 0; i < m_units.Count; ++i)
        {
            Destroy(m_units[i]);
        }
        if (m_mm != null)
        {
            Destroy(m_mm);
            m_mm = null;
        }
        m_units.Clear();
    }

	void IMessage.net_message(s_net_message message)
	{
		
	}
	
	void IMessage.message(s_message message)
	{
		if (message.m_type == "guide_end") 
		{
            clear_unit();
            sys._instance.remove_child(m_obj);
            sys._instance.remove_child(m_pet_obj);
        }
        if (message.m_type == "show_ui_unit_cam_guide")
        {
            m_can_rot = false;
            m_rot = 0;
            m_cam.SetActive(true);
            sys._instance.remove_child(m_pet_obj);

            Vector3 v = (Vector3)message.m_object[0];
            Vector3 r = (Vector3)message.m_object[1];

            if (m_unit != null)
            {
                m_unit.SetActive(false);
            }

            if (message.m_object[2] == null)
            {
                if (m_mm != null)
                {
                    m_unit = m_mm;
                }
                else
                {
                    m_unit = sys._instance.create_mm(m_obj);
                    m_unit.GetComponent<NavMeshAgent>().enabled = false;
                    m_unit.GetComponent<face>().enabled = false;
                    m_mm = m_unit;
                }
            }
            else
            {
                ccard _card = (ccard)message.m_object[2];
                m_unit = add_unit(_card);
            }

            m_unit.SetActive(true);

            m_cam.transform.localPosition = v;
            m_cam.transform.localEulerAngles = r;

            s_message mes = new s_message();
            mes.m_type = "update_ui_unit_cam";
            mes.time = 0.3f;
            cmessage_center._instance.add_message(mes);
        }
        if (message.m_type == "show_ui_unit_cam")
		{
            m_can_rot = true;
            m_rot = 0;
            m_cam.SetActive(true);
			sys._instance.remove_child(m_obj);
			sys._instance.remove_child(m_pet_obj);

            if (message.m_bools.Count > 0)
            {
                m_can_rot = message.m_bools[0];
            }
            Vector3 v = (Vector3)message.m_object[0];
            Vector3 r = (Vector3)message.m_object[1];

            ccard _card = (ccard)message.m_object[2];
        	m_unit = sys._instance.create_class(_card, 0, m_obj);
			if(message.m_object.Count > 3 &&(pet)message.m_object[3] != null)
			{
				m_pet_unit = sys._instance.create_pet((pet)message.m_object[3], 1, m_pet_obj);
                m_unit.transform.localPosition = new Vector3(-0.3f, 0, 0);
                m_pet_unit.transform.localPosition = new Vector3(-0.4f, 0, 0.24f);
            }

			m_cam.transform.localPosition = v;
			m_cam.transform.localEulerAngles = r;
			
			s_message mes = new s_message ();
			mes.m_type = "update_ui_unit_cam";
			mes.time = 0.3f;
			cmessage_center._instance.add_message (mes);
		}
		if (message.m_type == "show_ui_pet_cam")
		{
            m_can_rot = true;
            m_rot = 0;
            m_cam.SetActive(true);
			sys._instance.remove_child(m_obj);
			sys._instance.remove_child(m_pet_obj);

            Vector3 v = (Vector3)message.m_object[0];
            Vector3 r = (Vector3)message.m_object[1];

            pet _pet = (pet)message.m_object[2];
			m_unit = sys._instance.create_pet(_pet, 0, m_obj);

            m_cam.transform.localPosition = v;
            m_cam.transform.localEulerAngles = r;

            s_message mes = new s_message ();
			mes.m_type = "update_ui_pet_cam";
			mes.time = 0.3f;
			cmessage_center._instance.add_message (mes);
		}
		if (message.m_type == "update_ui_unit_cam")
		{
			if (m_unit == null)
			{
				return;
			}
			foreach(Transform tran in m_unit.GetComponentsInChildren<Transform>())
			{
				tran.gameObject.layer = 11;
			}
			float h = m_unit.GetComponent<unit>().m_name_height;
            if (h > 2.5f)
            {
                h = 2.5f / h;
            }
            else
            {
                h = 1;
            }
            m_unit.transform.localScale = new Vector3(h, h, h);
			if(m_pet_unit != null)
			{
				foreach(Transform tran in m_pet_unit.GetComponentsInChildren<Transform>())
				{
					tran.gameObject.layer = 11;
				}
                m_pet_unit.transform.localScale = new Vector3(h, h, h);
			}
		}
		if (message.m_type == "update_ui_pet_cam")
		{
			if (m_unit == null)
			{
				return;
			}
			foreach(Transform tran in m_unit.GetComponentsInChildren<Transform>())
			{
				tran.gameObject.layer = 11;
			}
		}
		if (message.m_type == "hide_ui_unit_cam")
		{
            m_cam.SetActive(false);
		}
		if (message.m_type == "action_ui_unit_cam")
		{
			if (m_unit == null)
			{
				return;
			}
            m_unit.GetComponent<unit>().m_target_pos = Vector3.zero;
            m_unit.GetComponent<unit>().action("attack");
			if (m_pet_unit != null)
			{
				m_pet_unit.GetComponent<unit>().action("win");
			}
		}
	}

	void Update () {
		if (m_unit == null)
		{
			return;
		}
        if (!m_can_rot)
        {
            return;
        }
		if (sys._instance.get_mouse_button(0))
		{
			Vector3 _move_pos = sys._instance.get_mouse_position();
			
			if(m_end == 0)
			{
				if (_move_pos.y < Screen.height / 2)
				{
					m_end = 1;
				}
				else
				{
					m_end = 2;
				}
				m_old_pos.Set(_move_pos.x,_move_pos.y,_move_pos.z);
			}
			
			Vector3 _des_pos = _move_pos - m_old_pos;
			m_old_pos.Set(_move_pos.x,_move_pos.y,_move_pos.z);

			m_rot -= _des_pos.x * 0.2f;
			m_unit.transform.localEulerAngles = new Vector3(0, m_rot, 0);
			if(m_pet_unit != null)
			{
				m_pet_unit.transform.localEulerAngles = new Vector3(0, m_rot, 0);
			}
		}
		else
		{
			m_end = 0;
		}
	}
}
