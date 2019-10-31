using UnityEngine;

public class attack_line : MonoBehaviour
{
    public Vector3 m_start_pos;
    public Vector3 m_end_pos;
    public GameObject m_bar_res;
    public GameObject m_bar = null;
    void bar()
    {
        m_bar = (GameObject)Instantiate(m_bar_res);
        m_bar.transform.position = m_start_pos;
    }
    void OnDestroy()
    {
        if (m_bar != null)
        {
            GameObject.Destroy(m_bar);
        }
    }
    public void OnEnable()
    {
        if (m_bar == null)
        {
            bar();
        }
        m_bar.transform.position = m_start_pos;
    }

    // Update is called once per frame
    public void Update()
    {
        Vector3 _v3 = m_end_pos - m_bar.transform.position;
        float _add = Time.deltaTime * 20f;

        if (_v3.magnitude > _add)
        {
            _v3.Normalize();

            m_bar.transform.position += _v3 * _add;
        }
        else
        {
            GameObject.Destroy(m_bar);
            bar();
        }

        this.GetComponent<LineRenderer>().SetPosition(0, m_start_pos);
        this.GetComponent<LineRenderer>().SetPosition(1, m_end_pos);
    }
}
