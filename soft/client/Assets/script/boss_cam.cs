using UnityEngine;

public class boss_cam : MonoBehaviour, IMessage
{
    public GameObject m_cam;
    public GameObject m_boss;
    private float m_shake = 0;
    public string[] m_actions;
    private float m_rand_time = 1.0f;

    void Start()
    {
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    void IMessage.net_message(s_net_message message)
    {

    }

    void IMessage.message(s_message message)
    {

        if (message.m_type == "shake_cam")
        {
            message.m_remove = true;
            m_shake = (float)message.m_floats[0];
        }
    }

    void Update()
    {

        Vector3 _pos = transform.position;
        if (m_shake > 0)
        {
            float _addx = Random.Range(-m_shake, m_shake);
            float _addy = Random.Range(-m_shake, m_shake);
            float _addz = Random.Range(-m_shake, m_shake);
            m_cam.transform.position = new Vector3(_pos.x + _addx, _pos.y + _addy, _pos.z + _addz);
            m_shake -= Time.deltaTime * 3.0f;
        }
        else
        {
            m_cam.transform.position = _pos;
        }

        if (m_rand_time > 0)
        {
            m_rand_time -= Time.deltaTime;

            if (m_rand_time <= 0)
            {
                m_boss.GetComponent<unit>().action("aa");
                m_boss.GetComponent<unit>().action(m_actions[Random.Range(0, m_actions.Length)]);
                m_rand_time = Random.Range(5.0f, 9.0f);
            }
        }
    }
}