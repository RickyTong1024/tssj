
using UnityEngine;
using System.Collections;

public class vr : MonoBehaviour,IMessage {

	public GameObject m_cam;
	public GameObject m_look_at;
	public GameObject m_mm;
	public GameObject m_base;
	public GameObject m_wu;
	private RaycastHit[] m_rayhit;
	private Ray m_ray;
	private Vector3 m_point = new Vector3(0.5f,0.5f,0);
	private float m_speed = 0.25f;
	private Transform m_select;
	private Color m_color_0 = new Color(1,1,1);
	private Color m_color_1 = new Color(0,1,0);

	private float m_time = 0;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
		
	}
    void OnEnable()
    {
        if (m_mm == null)
        {
            m_mm = GameObject.Find("mm");
        }
        if (m_mm != null)
        {
            Animation an = m_mm.GetComponent<Animation>();
            if (!an.IsPlaying("wd1"))
            {
                m_wu.SetActive(true);
 
            }

        }
 
    }
	void IMessage.message(s_message message)
	{
		if(message.m_type == "show_wd")
		{
            m_wu.SetActive(true);
		}
		else if(message.m_type == "hide_wd")
		{
			m_wu.SetActive(false);
		}
        else if (message.m_type == "test")
        {
            s_message _msg = new s_message();

            _msg.m_type = "mm_action";
            _msg.m_string.Add("wu");

            cmessage_center._instance.add_message(_msg);

            m_wu.SetActive(false);
 
        }
	}
	
	void IMessage.net_message(s_net_message message)
	{
		
	}
	// Update is called once per frame
	void Update () {
	
		//Ray _ray = m_cam.GetComponent<Camera>().ViewportPointToRay (m_point);
		m_ray.origin = m_cam.transform.position;
		m_ray.direction = m_look_at.transform.position - m_cam.transform.position;

		m_rayhit = Physics.RaycastAll(m_ray);

		if(m_rayhit.Length == 0 && m_select != null)
		{
			m_select.GetComponent<Renderer>().material.SetColor("_TintColor",m_color_0);
			m_time = 0.2f;
		}

		if(m_rayhit.Length > 0 && m_time > 0f)
		{
			m_time -= Time.deltaTime;

			return;
		}


		for(int i = 0;i < m_rayhit.Length;i ++)
		{
			if(m_rayhit[i].collider.gameObject.tag == "vr")
			{
				if(m_rayhit[i].collider.gameObject.name == "up" && m_base.transform.localPosition.y < 2f)
				{
					Vector3 _pos =  m_base.transform.localPosition;

					_pos.y += Time.deltaTime * m_speed;

					m_base.transform.localPosition = _pos;
				}

				if(m_rayhit[i].collider.gameObject.name == "down" && m_base.transform.localPosition.y > 0.2f)
				{
					Vector3 _pos =  m_base.transform.localPosition;
					
					_pos.y -= Time.deltaTime * m_speed;
					
					m_base.transform.localPosition = _pos;
				}

				if(m_rayhit[i].collider.gameObject.name == "+" && m_base.transform.localPosition.z > - 2f)
				{
					Vector3 _pos =  m_base.transform.localPosition;
					
					_pos.z -= Time.deltaTime * m_speed;
					
					m_base.transform.localPosition = _pos;
				}

				if(m_rayhit[i].collider.gameObject.name == "-" && m_base.transform.localPosition.z < - 0.2f)
				{
					Vector3 _pos =  m_base.transform.localPosition;
					
					_pos.z += Time.deltaTime * m_speed;
					
					m_base.transform.localPosition = _pos;
				}

				if(m_rayhit[i].collider.gameObject.name == "l")
				{
					Vector3 _angle =  this.transform.localEulerAngles;

					_angle.y += Time.deltaTime * 30f;

					this.transform.localEulerAngles = _angle;
				}
				
				if(m_rayhit[i].collider.gameObject.name == "r")
				{
					Vector3 _angle =  this.transform.localEulerAngles;
					
					_angle.y -= Time.deltaTime * 30f;

					this.transform.localEulerAngles = _angle;
				}

				if(m_rayhit[i].collider.gameObject.name == "wd")
				{
					s_message _msg = new s_message();
					
					_msg.m_type = "mm_action";
					_msg.m_string.Add("wu");
					
					cmessage_center._instance.add_message(_msg);
					
					m_wu.SetActive(false);
				}

				if(m_select != null && m_select != m_rayhit[i].collider.transform)
				{
					m_select.GetComponent<Renderer>().material.SetColor("_TintColor",m_color_0);
					m_select = m_rayhit[i].collider.transform;
					m_time = 0.2f;
				}
				else if(m_select == null)
				{
					m_select = m_rayhit[i].collider.transform;
				}


				m_rayhit[i].collider.transform.GetComponent<Renderer>().material.SetColor("_TintColor",m_color_1);
			}
		}
	}
}
