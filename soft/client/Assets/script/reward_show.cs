
using UnityEngine;
using System.Collections;

public class reward_show : MonoBehaviour,IMessage {

	public GameObject m_unit_root;
	public GameObject m_unit;
	public GameObject m_shell_0;
	public GameObject m_shell_1;
	private UITexture m_mask;
    public Camera m_cam;
	// Use this for initialization

	void Start()
	{
		cmessage_center._instance.add_handle (this);
        platform._instance.deal_cam("reward_show", m_cam);
    }

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		RenderSettings.fog = false;
		sys._instance.remove_child(m_unit_root);
		m_mask = root_gui._instance.m_mask.transform.Find("Texture").GetComponent<UITexture>();
		m_mask.gameObject.transform.parent.gameObject.SetActive (true);
		m_mask.alpha = 1.0f;
	}

	void IMessage.net_message(s_net_message message)
	{
		
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "reward_show_unit")
		{
			sys._instance.remove_child(m_unit_root);

			ccard _card = sys._instance.m_self.get_card_guid((ulong)message.m_long[0]);
			m_unit = sys._instance.create_class(_card, 0, m_unit_root);
			m_unit.GetComponent<unit>().m_alpha = 0.0f;

			Vector3 _pos = m_unit_root.transform.localPosition;
			float _height = m_unit.GetComponent<unit>().m_name_height;

			Transform _object = m_unit.transform.Find("xuankuang");

			if(_object != null)
			{
				_pos.z = - 9.0f + _object.transform.localScale.y * m_unit.transform.localScale.y * 2.0f;
				_pos.y = - _object.transform.localPosition.y * m_unit.transform.localScale.y;
			}

			m_unit_root.transform.localPosition = _pos;

			sys._instance.play_sound_ex("sound/huanhu");
		}
	}
	public void show()
	{
		if(m_unit != null)
		{
			m_unit.GetComponent<unit>().action("win");
		}
	}
	// Update is called once per frame
	void Update () {
	
		if(m_mask.alpha > 0)
		{
			m_mask.alpha -= Time.deltaTime;

			if(m_mask.alpha < 0)
			{
				m_mask.alpha = 0;
				m_mask.gameObject.transform.parent.gameObject.SetActive (false);
			}
		}

		Vector3 _angle_0 = m_shell_0.transform.localEulerAngles;

		_angle_0.z += Time.deltaTime * 10;
		m_shell_0.transform.localEulerAngles = _angle_0;

		Vector3 _angle_1 = m_shell_1.transform.localEulerAngles;
		_angle_1.z -= Time.deltaTime * 10;
		m_shell_1.transform.localEulerAngles = _angle_1;
	}
}
