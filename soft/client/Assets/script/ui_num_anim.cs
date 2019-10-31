
using UnityEngine;
using System.Collections;

public class ui_num_anim : MonoBehaviour {

	public float m_target;
	public float m_cur;
	public float m_speed = 10.0f;
	public GameObject m_label;
    public GameObject m_obj = null;
	public bool m_is_yichu = false;

    void FixedUpdate()
    {
        m_cur = Mathf.Lerp(m_cur, m_target, 0.2f);
        if (m_target < 1000000)
        {
            if (Mathf.Abs(m_cur - m_target) < 1)
            {
                if (m_obj != null)
                {
                    m_obj.SetActive(true);
                }


                m_cur = m_target;
            }
        }
        else
        {
            if (Mathf.Abs(m_cur - m_target) < 10000)
            {
                if (m_obj != null)
                {
                    m_obj.SetActive(true);
                }
                
                m_cur = m_target;
            }
 
        }
       
        m_label.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)m_cur);
		if (m_is_yichu) 
		{
			if (m_obj != null)
			{
				m_obj.SetActive(true);
			}
		}
		if (m_cur == 0)
		{
			m_label.GetComponent<UILabel>().text = "0";
        } 
	}
    public void up()
    {
        float _speed = m_target / m_speed;

        float _add = Time.deltaTime * _speed;

        if (Mathf.Abs(m_cur - m_target) > _add)
        {
            if (m_cur > m_target)
            {
                m_cur -= _add;
            }
            else if (m_cur < m_target)
            {
                m_cur += _add;
            }

            if (Mathf.Abs(m_cur - m_target) < _add)
            {
                m_cur = m_target;

                m_label.transform.GetComponent<Animator>().Play("scale_anim_0", 0, 0);
            }
        }

        m_label.GetComponent<UILabel>().text = ((int)m_cur).ToString();
 
    }
}
