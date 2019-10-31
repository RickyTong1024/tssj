
using UnityEngine;
using System.Collections;

public class unit_show : MonoBehaviour,IMessage {

	public GameObject m_cam;
	public GameObject m_unit_root;
	public GameObject m_pet_root;
	public GameObject m_effect_0;
	public GameObject m_effect_1;

	private ccard m_show_card;
	private GameObject m_show_unit;
	private GameObject m_show_pet;
	private pet m_pet;
	private int m_big = 0;
	private Vector3 m_cam_target = new Vector3();
	private bool flag = false;
	private bool is_pet = false;

	private float m_mouse_pos_x = 0f;
	private bool m_mouse_button = false;
	private RaycastHit m_rayhit;

	// Use this for initialization
	void Start () 
	{
		cmessage_center._instance.add_handle (this);

	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	void IMessage.net_message(s_net_message message)
	{
		if((message.m_opcode == opclient_t.CMSG_ROLE_UPGRADE && chong_neng_gui.is_texiao)||
		   message.m_opcode == opclient_t.CMSG_ROLE_JINJIE ||
		   (message.m_opcode == opclient_t.CMSG_ROLE_TUPO&& tu_po_gui.is_texiao) ||  message.m_opcode == opclient_t.CMSG_ROLE_SHENGPIN)
		{
			GameObject _effect = (GameObject)Instantiate(m_effect_1,m_effect_1.transform.position,m_effect_1.transform.rotation);

			_effect.SetActive(true);

			GameObject.Destroy(_effect,5f);

			_effect = (GameObject)Instantiate(m_effect_0);
			
			_effect.SetActive(true);

			Vector3 _pos = m_show_unit.transform.position;

			_pos.y += m_show_unit.GetComponent<unit>().m_name_height * 0.5f;

			_effect.transform.position = _pos;
			
			GameObject.Destroy(_effect,5f);
		}
		if(message.m_opcode == opclient_t.CMSG_PET_LEVEL || message.m_opcode == opclient_t.CMSG_PET_STAR ||
		   (message.m_opcode == opclient_t.CMSG_PET_JINJIE&& pet_jingjie_gui.is_jj))
		{
			GameObject _effect = (GameObject)Instantiate(m_effect_1,m_effect_1.transform.position,m_effect_1.transform.rotation);
			
			_effect.SetActive(true);
			
			GameObject.Destroy(_effect,5f);
			
			_effect = (GameObject)Instantiate(m_effect_0);
			
			_effect.SetActive(true);
			
			Vector3 _pos = m_show_pet.transform.position;
			
			_pos.y += m_show_pet.GetComponent<unit>().m_name_height * 0.5f;
			
			_effect.transform.position = _pos;
			
			GameObject.Destroy(_effect,5f);
		}
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "show_unit")
		{
			if(message.m_long.Count > 0)
			{
				ulong guid = (ulong)message.m_long[0];
				m_show_card = sys._instance.m_self.get_card_guid(guid);
			}
			else if(message.m_object.Count > 0)
			{
				m_show_card = (ccard)message.m_object[0];
			}
			if(message.m_bools.Count > 0)
			{
				flag = (bool)message.m_bools[0];
			}
			else
			{
				flag = false;
			}
			show_object();
			is_pet = false;
			int big = 1;
			m_pet = sys._instance.m_self.get_pet_guid(m_show_card.get_role().pet);
			if(m_pet != null)
			{
				show_pet_object(big);
			}
			else
			{
				sys._instance.remove_child(m_pet_root);
			}
		}

		if(message.m_type == "show_pet")
		{
			int big = 0;
			if(message.m_object.Count > 0)
			{
				m_pet = (pet)message.m_object[0];
			}
			if(message.m_ints.Count > 0)
			{
				big = (int)message.m_ints[0];
			}
			if(message.m_bools.Count > 0)
			{
				flag = (bool)message.m_bools[0];
			}
			else
			{
				flag = false;
			}
			if(big == 0)
			{
				sys._instance.remove_child(m_unit_root);
				is_pet = true;
				show_pet_object(big);
			}
			else
			{
				is_pet = false;
				m_big = 1;
				m_pet = sys._instance.m_self.get_pet_guid(m_show_card.get_role().pet);
				if(m_pet != null)
				{
					show_pet_object(big);
				}
				else
				{
					sys._instance.remove_child(m_pet_root);
				}
			}
		}

		if(message.m_type == "show_dress")
		{
			if(m_show_unit != null)
			{
				dhc.role_t _role = message.m_object[0] as dhc.role_t;
				
				if(m_show_unit.GetComponent<unit>().m_card.get_role() == _role)
				{
					m_show_card = m_show_unit.GetComponent<unit>().m_card;
					m_show_unit = null;
					show_object();
				}
			}
		}

		if(message.m_type == "hide_show_unit")
		{
			sys._instance.remove_child(m_unit_root);

			m_show_unit = null;
			m_show_pet = null;
		}
		if(message.m_type == "hide_show_pet")
		{
			sys._instance.remove_child(m_pet_root);
			
			m_show_pet = null;
		}
	}

	void show_object()
	{
		if (m_show_card != null)
		{
			GameObject _object = null;
			ccard _card = m_show_card;
			
			if(m_show_unit != null && m_show_unit.GetComponent<unit>().m_card.get_guid() == m_show_card.get_guid() && !flag)
			{
				_object = m_show_unit;
			}
			else
			{
				sys._instance.remove_child(m_unit_root);
				_object = sys._instance.create_class(_card, sys._instance.m_self.m_t_player.guanghuan_id, m_unit_root);
				m_unit_root.transform.localEulerAngles = new Vector3(0,0,0);
				_object.GetComponent<unit>().m_alpha = 0.0f;

				m_cam_target.y = _object.GetComponent<unit>().m_name_height * 0.4f;
				m_cam_target.z = _object.GetComponent<unit>().m_name_height * 1.8f;
			}
			
			m_show_unit = _object;
		}

	}

	void show_pet_object(int big)
	{
		if (m_pet != null)
		{
			GameObject _object = null;
			if(big == 0)
			{
				m_pet_root.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
				m_pet_root.transform.localPosition = new Vector3(0,0,0);
			}
			else
			{
				m_pet_root.transform.localScale =  Vector3.one;
				m_pet_root.transform.localPosition = new Vector3(0.8f,0,0.24f);
			}
			flag = false;
			if(big != m_big)
			{
				flag = true;
				m_big = big;
			}
			if(m_show_pet != null && m_show_pet.GetComponent<unit>().m_pet.get_guid() == m_pet.get_guid() && !flag)
			{
				m_pet_root.transform.localEulerAngles = new Vector3(0,0,0);
				_object = m_show_pet;
			}
			else
			{
				sys._instance.remove_child(m_pet_root);
				_object = sys._instance.create_pet(m_pet, m_big, m_pet_root);
				m_pet_root.transform.localEulerAngles = new Vector3(0,0,0);
				_object.GetComponent<unit>().m_alpha = 0.0f;
				if(is_pet)
				{
					m_cam_target.y = _object.GetComponent<unit>().m_name_height * 0.4f;
					m_cam_target.z = _object.GetComponent<unit>().m_name_height * 1.8f;
				}
			}
		
			m_show_pet = _object;
		}
		
	}

	// Update is called once per frame
	void Update () {

		if(m_show_unit == null && m_show_pet == null)
		{
			return;
		}

		m_cam.transform.localPosition += (m_cam_target - m_cam.transform.localPosition) * Time.deltaTime;

		if(sys._instance.get_mouse_button(0) && !is_pet)
		{
			Ray _ui_ray = root_gui._instance.get_ui_cam().ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast(_ui_ray, out m_rayhit) == true)
			{
				m_mouse_button = false;
				return ;
			}
			
			if(m_mouse_button == false)
			{
				m_mouse_pos_x = sys._instance.get_mouse_position ().x;
				m_mouse_button = true;
			}
			
			float _mouse_pos_x = sys._instance.get_mouse_position ().x;
			
			Vector3 _angle = m_unit_root.transform.localEulerAngles;
			
			_angle.y += m_mouse_pos_x - _mouse_pos_x;
			
			m_unit_root.transform.localEulerAngles = _angle;
			m_pet_root.transform.localEulerAngles = _angle;
			
			m_mouse_pos_x = sys._instance.get_mouse_position ().x;
		}
		else if(sys._instance.get_mouse_button(0) && is_pet)
		{
			Ray _ui_ray = root_gui._instance.get_ui_cam().ScreenPointToRay(Input.mousePosition);
			
			if(Physics.Raycast(_ui_ray, out m_rayhit) == true)
			{
				m_mouse_button = false;
				return ;
			}
			
			if(m_mouse_button == false)
			{
				m_mouse_pos_x = sys._instance.get_mouse_position ().x;
				m_mouse_button = true;
			}
			
			float _mouse_pos_x = sys._instance.get_mouse_position ().x;
			
			Vector3 _angle = m_pet_root.transform.localEulerAngles;
			
			_angle.y += m_mouse_pos_x - _mouse_pos_x;
		
			m_pet_root.transform.localEulerAngles = _angle;
			
			m_mouse_pos_x = sys._instance.get_mouse_position ().x;
		}
		else if(m_mouse_button == true)
		{
			m_mouse_button = false;
		}
	}
}
