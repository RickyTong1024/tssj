
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class map_show : MonoBehaviour,IMessage {

    public static map_show _instance;
    private List<GameObject> m_star_level = new List<GameObject>();
	public Camera m_cam;
	public Camera m_unit_cam;
    public GameObject m_root;
    // Use this for initialization
    void Awake()
    {
        _instance = this;
    }
    void Start () {
		cmessage_center._instance.add_handle (this);
        platform._instance.deal_cam("map_show", m_cam);

        bool _end = false;
		int _i = 1;
		while(_end == false)
		{
			string _name = "a_part" + _i;

			Transform _obj = this.transform.Find("xq").Find(_name);

			if(_obj != null)
			{
				Vector3 _pos = _obj.transform.localPosition;

				_pos.x = 100 - _i * 20;

				_obj.transform.localPosition = _pos;

				m_star_level.Add(_obj.gameObject);
			} 
			else
			{
				_end = true;
			}

			_i ++;
		}
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.net_message(s_net_message message)
	{
	}

	void IMessage.message(s_message message)
	{
		if(message.m_type == "select_star_level")
		{
            sys._instance.remove_child(m_root);
            int _id = (int)message.m_ints[0];

			Vector3 _from = m_cam.transform.position;
			Vector3 _to = new Vector3( m_star_level[_id].transform.position.x,_from.y,_from.z);
			
			TweenPosition _effect = TweenPosition.Begin(m_cam.gameObject, 0.5f,_to);
			
			_effect.method = UITweener.Method.EaseInOut;
			_effect.from = _from;
			_effect.to = _to;

			s_message _msg = new s_message();
			_msg.time = 0.3f;
			_msg.m_type = "cam_finished";
			cmessage_center._instance.add_message(_msg);
		}
        else if (message.m_type == "show_map_unit")
        {
            sys._instance.remove_child(m_root);

			Vector3 _angle = m_unit_cam.transform.localEulerAngles;
			_angle.y = (((float)Screen.width / (float)Screen.height) - 1f)  * 10f * 1.4f + 9f;
			m_unit_cam.transform.localEulerAngles = _angle;

            ccard _card = (ccard)message.m_object[0];
            GameObject _unit = sys._instance.create_class(_card, 0, m_root);
            float nt = _unit.GetComponent<unit>().m_name_height;
            if (nt > 2.3f)
            {
                _unit.transform.localPosition = new Vector3(0, 2.3f - nt, 0);
            }
            else
            {
                _unit.transform.localPosition = Vector3.zero;
            }

			_unit.GetComponent<unit>().set_layer(m_root.layer);
			_unit.transform.localEulerAngles = new Vector3(0, 170, 0);
        }

    }
}
