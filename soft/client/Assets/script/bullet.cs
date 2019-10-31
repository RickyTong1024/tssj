using UnityEngine;

public class bullet : MonoBehaviour
{
    public GameObject m_bullet;

    public void OnEnable()
    {
        Vector3 _pos = m_bullet.transform.localPosition;
        _pos.x = 0;
        _pos.y = 0;
        _pos.z = 0;
        m_bullet.transform.localPosition = _pos;

        Vector3 _angle = transform.localEulerAngles;
        _angle.x = Random.Range(-5, 5.0f);
        _angle.y = Random.Range(-5, 5.0f);
        _angle.z = Random.Range(-5, 5.0f);
        transform.localEulerAngles = _angle;
    }

    void Update()
    {
        Vector3 _pos = m_bullet.transform.localPosition;
        _pos.z += Time.deltaTime * 60;
        m_bullet.transform.localPosition = _pos;
    }
}
