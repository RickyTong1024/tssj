using UnityEngine;

public class att_modify_effect : MonoBehaviour
{

    public GameObject m_sprite;
    public float m_add;
    public Vector3 m_position;
    public float m_height = 100.0f;
    // Use this for initialization
    void Start()
    {
        m_height = 100 * m_add;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _pos = sys._instance.WorldToScreenPoint(m_position);
        this.transform.localPosition = _pos;
    }
}
