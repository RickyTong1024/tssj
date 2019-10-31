using UnityEngine;

public class box_dance : MonoBehaviour
{
    private float m_shake = 0.0f;

    public void shake(float shake)
    {
        m_shake = shake;
    }

    public void OnEnable()
    {
        this.GetComponent<BoxCollider>().enabled = true;
    }

    public void open()
    {
        this.GetComponent<Animator>().Play("box_open");
        this.GetComponent<BoxCollider>().enabled = false;
    }

    public void end()
    {
        s_message _msg = new s_message();
        _msg.m_type = "box_end";
        cmessage_center._instance.add_message(_msg);
    }

    void Update()
    {
        if (m_shake > 0)
        {
            m_shake -= Time.deltaTime * 10;

            if (m_shake <= 0)
            {
                m_shake = 0;
            }

            Vector3 _pos = this.transform.localPosition;
            _pos.x = Random.Range(-m_shake, m_shake);
            _pos.y = Random.Range(-m_shake, m_shake);
            this.transform.localPosition = _pos;
        }
    }
}
