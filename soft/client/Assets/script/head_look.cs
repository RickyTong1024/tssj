
using UnityEngine;
using System.Collections;

public class head_look : MonoBehaviour {

    private GameObject m_head_bone;

	public Vector3 m_xiuzheng;
    private Quaternion m_q = new Quaternion();
	private Quaternion m_out_q = new Quaternion();

	private float m_max_head_x = 30f;
	private float m_max_head_y = 80f;

	private float m_head_angle_spead = 5f;

    private GameObject m_head_look;

	public GameObject m_look_obj;
	private float m_head_angle_t = 0.0f;
	private float m_head_angle_max = 0.6f;
    // Use this for initialization
	
	void Start()
	{
		Transform t = get_child_name (this.transform, "Bip01 Head");
		if (t != null)
		{
			m_head_bone = t.gameObject;
			m_head_look = new GameObject ();
			m_head_look.transform.parent = m_head_bone.transform.parent;
			m_head_look.transform.localPosition = Vector3.zero;
		}
		else
		{
			m_head_bone = null;
		}
	}

	public void look_obj(GameObject obj)
	{
		m_look_obj = obj;
		m_head_angle_t = 0;
	}

	public void look_empty()
	{
		m_look_obj = null;
		m_head_angle_t = 0;
	}

	void do_head()
    {
        if (m_head_bone == null)
        {
            return;
        }
		float x = m_head_look.transform.eulerAngles.x - this.transform.eulerAngles.x;
		float y = m_head_look.transform.eulerAngles.y - this.transform.eulerAngles.y;
		if (x > 180)
		{
			x = x - (int)(x / 360) - 360;
		}
		if (x < -180)
		{
			x = x - (int)(x / 360) + 360;
		}
		if (x > m_max_head_x)
		{
			x = m_max_head_x;
		}
		else if (x < -m_max_head_x)
		{
			x = -m_max_head_x;
		}
		if (y > 180)
		{
			y = y - (int)(y / 360) - 360;
		}
		if (y < -180)
		{
			y = y - (int)(y / 360) + 360;
		}
		if (y > m_max_head_y)
		{
			y = m_max_head_y;
		}
		else if (y < -m_max_head_y)
		{
			y = -m_max_head_y;
		}
		m_head_look.transform.eulerAngles = new Vector3(x + this.transform.eulerAngles.x, y + this.transform.eulerAngles.y, m_head_look.transform.eulerAngles.z);
		m_q.eulerAngles = new Vector3(m_xiuzheng.x, m_xiuzheng.y, m_xiuzheng.z);
		m_out_q = m_head_look.transform.localRotation * m_q;
		if (m_head_angle_t < m_head_angle_max)
		{
			m_head_angle_t += Time.deltaTime * m_head_angle_spead;
		}
		m_head_bone.transform.localRotation = Quaternion.Lerp(m_head_bone.transform.localRotation,m_out_q,m_head_angle_t);
    }

	Transform get_child_name(Transform t, string name)
	{
		for (int i = 0; i < t.childCount; ++i)
		{
			Transform tt = t.GetChild(i);
			if (tt.gameObject.name == name)
			{
				return tt;
			}
			Transform ttt = get_child_name(tt, name);
			if (ttt != null)
			{
				return ttt;
			}
		}
		return null;
	}

	void LateUpdate()
	{
		if (m_head_bone == null)
		{
			return;
		}
		if (m_look_obj != null)
		{
			Vector3 v = m_look_obj.transform.position;
			Transform head = get_child_name (m_look_obj.transform, "Bip01 Head");
			Transform spine = get_child_name (m_look_obj.transform, "Bip01 Spine1");
			if (head != null && spine != null)
			{
				v = (head.position + spine.position) / 2;
			}
			m_head_look.transform.LookAt(v, new Vector3(0,1,0));
			do_head();
		}
	}
}
