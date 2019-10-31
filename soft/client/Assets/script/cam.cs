using UnityEngine;

public class cam : MonoBehaviour, IMessage
{
    public GameObject m_cam;
    public GameObject m_target;
    private float m_speed = 3.0f;
    private Vector3 m_end_cam_pos;
    private Vector3 m_end_target_pos;

    private Vector3 m_cur_cam_pos;
    private Vector3 m_cur_target_pos;

    private float m_dis = 11;
    private Vector3 m_rot = new Vector3();
    private float m_height = 1.0f;
    private Vector3 m_old_pos = new Vector3();
    private bool m_end = false;

    private float m_max_rot = 30.0f;

    private float m_shake = 0.0f;

    private int m_cam_type = 0;

    private GameObject m_Close_up;
    private bool m_is_close_up;

    private Vector3 m_close_up_pos;
    private Vector3 m_close_up_lookat = new Vector3();

    private RaycastHit m_rayhit;
    private float m_fDistance = 20f;
    public static cam _instance;

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        m_rot.y = 20;
        cmessage_center._instance.add_handle(this);
    }

    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }

    private double PointToAngle(Vector3 AOrigin, Vector3 APoint, float AEccentricity)
    {
        if (APoint.x == AOrigin.x)
        {
            if (APoint.z > AOrigin.z)
            {
                return Mathf.PI * 0.5;
            }
            else
            {
                return Mathf.PI * 1.5;
            }
        }
        else if (APoint.z == AOrigin.z)
        {
            if (APoint.x > AOrigin.x)
            {
                return 0;
            }
            else
            {
                return Mathf.PI;
            }
        }
        else
        {
            float Result = Mathf.Atan((AOrigin.z - APoint.z) / (AOrigin.x - APoint.x) * AEccentricity);

            if ((APoint.x < AOrigin.x) && (APoint.z > AOrigin.z))
            {
                return Result + Mathf.PI;
            }
            else if ((APoint.x < AOrigin.x) && (APoint.z < AOrigin.z))
            {
                return Result + Mathf.PI;
            }
            else if ((APoint.x > AOrigin.x) && (APoint.z < AOrigin.z))
            {
                return Result + 2 * Mathf.PI;
            }
            else
            {
                return Result;
            }
        }
    }

    float get_height()
    {
        float _height = 1.0f;
        for (int i = 0; i < sys._instance.m_root_unit.transform.childCount; i++)
        {
            Transform _unit = sys._instance.m_root_unit.transform.GetChild(i);
            if (_unit.GetComponent<unit>().m_site == 100 || _unit.GetComponent<unit>().m_site == 101)
            {
                continue;
            }

            Transform _box = _unit.Find("xuankuang");

            if (_box != null && _box.localScale.y > _height)
            {
                _height = _box.localScale.y;
            }
        }
        return _height;
    }

    void IMessage.net_message(s_net_message message)
    {

    }

    void IMessage.message(s_message message)
    {
        if (message.m_type == "loaded")
        {
            Camera _cam = sys._instance.get_cam();
            if (_cam != null)
            {
                _cam.backgroundColor = new Color(0, 0, 0);
            }
        }

        if (message.m_type == "end_layout")
        {
            int max_row = (int)message.m_ints[0];
            float _height = get_height();

            m_end = true;
            m_height = _height * 0.4f;
            m_rot.x = m_height + 4.2f;
            m_dis = 10 + _height * 0.7f + max_row * 1.5f;

            if (m_cam_type > 0)
            {
                m_cam_type = -1;
                m_rot.y = -10;
            }
            else
            {
                m_cam_type = 1;
                m_rot.y = 10;
            }

            if (m_dis < 11f + max_row * 1.5f)
            {
                m_dis = 10f + max_row * 1.5f;
            }
            else if (m_dis > 13f + max_row * 1.5f)
            {
                m_dis = 13f + +max_row * 1.5f;
            }
            if (_height > 8)
            {
                m_rot.x = 3.0f;
            }
            set_cam();
        }

        if (message.m_type == "cam_src")
        {
            m_end_cam_pos.x = (float)message.m_floats[0];
            m_end_cam_pos.y = (float)message.m_floats[1];
            m_end_cam_pos.z = (float)message.m_floats[2];

            m_end_target_pos.x = (float)message.m_floats[3];
            m_end_target_pos.y = (float)message.m_floats[4];
            m_end_target_pos.z = (float)message.m_floats[5];

            Camera _cam = sys._instance.get_cam();

            if ((int)message.m_ints[0] == 1)
            {
                m_cur_target_pos = m_end_target_pos;
                m_cur_cam_pos = m_end_cam_pos;
            }

        }

        if (message.m_type == "cam_pos2")
        {
            m_rot.y = (float)message.m_floats[0];
            m_rot.x = (float)message.m_floats[1];

            if (message.m_floats.Count > 2)
            {
                m_dis = (float)message.m_floats[2];
            }
            set_cam();
        }


        if (message.m_type == "shake_cam")
        {
            message.m_remove = true;
            m_shake = (float)message.m_floats[0];
        }

        if (message.m_type == "end_battle_close_up")
        {
            m_is_close_up = false;
        }

        if (message.m_type == "battle_close_up")
        {
            m_is_close_up = true;
            m_Close_up = (GameObject)message.m_object[0];
        }
    }

    void set_cam()
    {
        if (m_rot.y > m_max_rot) m_rot.y = m_max_rot;
        if (m_rot.y < -m_max_rot) m_rot.y = -m_max_rot;

        if (m_rot.x > m_height + 5) m_rot.x = m_height + 5;
        if (m_rot.x < 1) m_rot.x = 1;


        float _x = Mathf.Sin(m_rot.y * (3.14f / 180.0f)) * -m_dis;
        float _y = Mathf.Cos(m_rot.y * (3.14f / 180.0f)) * -m_dis;

        m_end_cam_pos = new Vector3(_x, m_rot.x, _y);
        m_end_target_pos = new Vector3(0, m_height, 0);

    }

    public void set_cam_cur(Vector3 src, Vector3 target)
    {
        m_cur_cam_pos = src;
        m_cur_target_pos = target;
    }

    void Update()
    {

        if (sys._instance.get_mouse_button(0))
        {
            Ray _ui_ray = root_gui._instance.get_ui_cam().ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ui_ray, out m_rayhit, m_fDistance) == false)
            {
                Vector3 _move_pos = sys._instance.get_mouse_position();

                if (m_end == true)
                {
                    m_end = false;
                    m_old_pos.Set(_move_pos.x, _move_pos.y, _move_pos.z);
                }
                Vector3 _des_pos = _move_pos - m_old_pos;
                m_old_pos.Set(_move_pos.x, _move_pos.y, _move_pos.z);

                m_rot.x -= _des_pos.y * 0.1f;
                m_rot.y += _des_pos.x * 0.1f;
                set_cam();
            }
        }
        else
        {
            m_end = true;
        }

        Vector3 _out_pos = m_cur_cam_pos;
        Vector3 _out_lookat = m_cur_target_pos;
        Vector3 _end_out_pos = m_end_cam_pos;
        Vector3 _end_out_lookat = m_end_target_pos;

        if (m_is_close_up)
        {
            if (m_Close_up != null)
            {
                unit _unit = m_Close_up.GetComponent<unit>();

                float _height = _unit.m_name_height * 0.6f;

                m_close_up_pos = new Vector3(m_Close_up.transform.position.x, _height, m_Close_up.transform.position.z - _height * 2.5f);

                m_close_up_lookat.x = _unit.transform.position.x;
                m_close_up_lookat.y = _unit.m_name_height * 0.5f;
                m_close_up_lookat.z = _unit.transform.position.z;

                m_Close_up = null;
            }

            _end_out_pos = m_close_up_pos;
            _end_out_lookat = m_close_up_lookat;
        }

        m_cur_target_pos = (_end_out_lookat - m_cur_target_pos) * Time.smoothDeltaTime * m_speed + m_cur_target_pos;
        m_cur_cam_pos = (_end_out_pos - m_cur_cam_pos) * Time.smoothDeltaTime * m_speed + m_cur_cam_pos;
        _out_lookat = m_cur_target_pos;

        if (m_shake > 0)
        {
            float _addx = Random.Range(-m_shake, m_shake);
            float _addy = Random.Range(-m_shake, m_shake);
            float _addz = Random.Range(-m_shake, m_shake);

            _out_pos = new Vector3(m_cur_cam_pos.x + _addx, m_cur_cam_pos.y + _addy, m_cur_cam_pos.z + _addz);
            _out_lookat = new Vector3(m_cur_target_pos.x + _addx, m_cur_target_pos.y + _addy, m_cur_target_pos.z + _addz);
            m_shake -= Time.deltaTime * 3.0f;
        }
        Camera _cam = null;

        if (sys._instance != null)
        {
            _cam = sys._instance.get_cam();
        }
        else
        {
            _cam = m_cam.GetComponent<Camera>();
        }

        if (_cam != null)
        {
            _cam.gameObject.transform.position = _out_pos;
            _cam.gameObject.transform.LookAt(_out_lookat);
        }
    }
}
