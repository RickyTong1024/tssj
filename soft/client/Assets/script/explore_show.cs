
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explore_show : MonoBehaviour,IMessage {
	
	public GameObject m_cam;
	public GameObject m_title;
	private GameObject m_unit = null;
	public GameObject m_root;
	private List<GameObject> m_floors = new List<GameObject>();
	private List <GameObject> m_targets = new List<GameObject>();
	
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	void create_floor(bool start = false)
	{
		int id = 0;
		for(int i = 0; i < m_floors.Count;++i)
		{
			Vector3 _dis = m_floors[i].transform.position - m_unit.transform.position;
			if(m_floors[i].transform.name == "start")
			{
				m_floors[i].transform.name = "rewarded";
			}
			if(_dis.magnitude <= 2)
			{
				id = int.Parse(m_floors[i].transform.GetChild(0).name);
				break;
			}
		}

		for(int i = 0; i < m_floors.Count;)
		{
			Vector3 _dis = m_floors[i].transform.position - m_unit.transform.position;
			if(_dis.magnitude > 2 && id >= int.Parse(m_floors[i].transform.GetChild(0).name))
			{
				m_floors[i].GetComponent<ts_tile>().m_time = Random.Range(0.6f,1.0f);
				m_floors[i].GetComponent<ts_tile>().m_target_alpha = - 0.1f;
				if(m_floors[i].GetComponent<ts_tile>().m_effect != null)
				{
					m_floors[i].GetComponent<ts_tile>().m_effect.SetActive(false);
				}
				m_floors.RemoveAt(i);
				continue;
			}
			else if(_dis.magnitude <= 2)
			{
				m_floors[i].transform.name = "start";
				m_floors[i].transform.GetChild(0).name = "0";
				m_floors[i].GetComponent<ts_tile>().m_target_alpha = 1f;
				m_floors[i].GetComponent<ts_tile>().m_effect.SetActive(false);
			}
			i++;
		}
		if(start)
		{
			GameObject _ins = (GameObject)Object.Instantiate(m_title);
			_ins.transform.position = new Vector3(0,0,0);
			_ins.transform.name = "start";
			_ins.transform.GetChild(0).name = "0";
			_ins.GetComponent<ts_tile>().m_time = 0;
			_ins.GetComponent<ts_tile>().m_target_alpha = 1;
			_ins.SetActive (true);
			m_floors.Add (_ins);
		}
		id = 0;
		for(int i = 0; i < m_floors.Count;++i)
		{
			if(m_floors[i].transform.name == "start")
			{
				id = i;
				break;
			}
		}
		if(start)
		{
			sys._instance.remove_child (m_root);
			ccard card =  sys._instance.m_self.get_card_id(100);
			m_unit = sys._instance.create_class
				(100, card.get_role().dress_on_id, sys._instance.m_self.m_t_player.guanghuan_id, m_root);
		}
		m_unit.GetComponent<unit>().action("ready");
		m_unit.transform.position = m_floors [id].transform.position;
		m_unit.transform.localEulerAngles = new Vector3(0, 0, 0);
		if(m_unit.GetComponent<CapsuleCollider>() == null)
		{
			m_unit.AddComponent<CapsuleCollider>();
			m_unit.transform.GetComponent<CapsuleCollider>().center = new Vector3 (0.14f,1.08f,0.06f);
			m_unit.transform.GetComponent<CapsuleCollider>().radius = 0.65f;
			m_unit.transform.GetComponent<CapsuleCollider>().height = 2.75f;
			m_unit.transform.tag = "unit";
			m_unit.AddComponent<Rigidbody>();
			m_unit.GetComponent<Rigidbody>().useGravity = false;
            m_unit.GetComponent<Rigidbody>().isKinematic = true;
		}

		tile ("c",m_floors[id].transform.position + new Vector3(0,0,3),false,1);
		tile ("f",m_floors[id].transform.position + new Vector3(0,0,6),true,3);
		tile ("l",m_floors[id].transform.position + new Vector3(-3,0,3),true,2);
		tile ("r",m_floors[id].transform.position + new Vector3(3,0,3),true,2);
	}

	GameObject get_tile(Vector3 pos)
	{
		for(int i = 0;i < m_floors.Count;i++)
		{
			Vector3 _dis = m_floors[i].transform.position - pos;
			
			if(_dis.magnitude < 1)
			{
				return  m_floors[i];
			}
		}
		return null;
	}

	void tile(string name,Vector3 _pos,bool reward,int children_name)
	{
		GameObject _ins = get_tile (_pos);
		
		if(_ins == null)
		{
			_ins = (GameObject)Object.Instantiate(m_title);
			_ins.transform.position = _pos;
			if(reward == true)
			{
				_ins.GetComponent<ts_tile>().reward(name);
			}
			
			_ins.SetActive (true);
			m_floors.Add (_ins);
		}
		else
		{
			_ins.GetComponent<ts_tile>().m_target_alpha = 1f;
		}
		_ins.transform.name = "reward" +name;
		_ins.transform.GetChild (0).name = children_name.ToString ();
			
	}

	void move()
	{
		m_unit.transform.LookAt (m_targets[0].transform.position);
		TweenPosition pos = TweenPosition.Begin (m_unit,0.8f,m_unit.transform.position);
		pos.to = m_targets [0].transform.position;
		EventDelegate.Add (pos.onFinished, delegate() {
			m_targets.RemoveAt(0);
			if(m_targets.Count > 0)
			{
				move();
			}
			else
			{
				create_floor();
				//explore_gui._instance.is_press = true;
				explore_gui._instance.move_finish();
			}
		});
		m_unit.GetComponent<unit>().action("run");
	}

	void IMessage.message (s_message message)
	{
		if(message.m_type == "tansuo_move")
		{
			explore_gui._instance.is_press = false;
			explore_gui._instance.button(false);
			int num = (int)message.m_ints[0];
			if(num >= 2)
			{
				num = 2;
			}
			string s = "";
			if(num == 0)
			{
				s = "l";
			}
			else if(num == 1)
			{
				s = "r";
			}
			else if(num == 2)
			{
				s = "f";
			}
			m_targets.Clear();
			for(int i = 0; i < m_floors.Count;++i)
			{
				if(m_floors[i].name == "rewardc")
				{
					m_targets.Add(m_floors[i]);
				}
			}
			for(int i = 0; i < m_floors.Count;++i)
			{
				if(m_floors[i].name == "reward" + s)
				{
					m_targets.Add(m_floors[i]);
				}
			}
			move();
		}
		if(message.m_type == "explore_create_floor")
		{
			create_floor (true);
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		
	}
	// Update is called once per frame
	void Update ()
    {
        if (m_cam != null && m_unit != null)
        {
            m_cam.transform.position = m_unit.transform.position + new Vector3(6, 6, -1);
        }
        if (Input.GetMouseButton(0) && explore_gui._instance.have_huodong && explore_gui._instance.is_press)
		{
			Ray ray = m_cam.transform.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
			RaycastHit[] _hits = Physics.RaycastAll(ray);
			for(int i = 0; i < _hits.Length;++i)
			{
				int num = 0;
				if(explore_gui._instance.m_num >= 5)
				{
					s_t_price t_price = game_data._instance.get_t_price((explore_gui._instance.m_num -5) +1);
					if(sys._instance.m_self.m_t_player.jewel < t_price.manyou)
					{
						root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
						return;
					}
				}
				if(_hits[i].transform.name == "f")
				{
					num = 2;
					explore_gui._instance.m_select = 0;
					s_message _message = new s_message();
					_message.m_type = "tansuo_move";
					_message.m_ints.Add(num);
					cmessage_center._instance.add_message(_message);
				}
				else if(_hits[i].transform.name == "l")
				{
					num = 0;
					explore_gui._instance.m_select = 0;
					s_message _message = new s_message();
					_message.m_type = "tansuo_move";
					_message.m_ints.Add(num);
					cmessage_center._instance.add_message(_message);
				}
				else if(_hits[i].transform.name == "r")
				{
					num = 1;
					explore_gui._instance.m_select = 0;
					s_message _message = new s_message();
					_message.m_type = "tansuo_move";
					_message.m_ints.Add(num);
					cmessage_center._instance.add_message(_message);
				}
			}
		}
	}
}
