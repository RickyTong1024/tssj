
using UnityEngine;
using System.Collections;

public class chongzhifali_gui : MonoBehaviour, IMessage {

	public GameObject m_view;
	public GameObject m_num;
	public GameObject m_time;
	public GameObject m_title;
	public GameObject m_title1;
	public int m_huodong_id;
	public static chongzhifali_gui _instance;
	private int m_id;
	public static ulong m_end_time;
	protocol.game.smsg_huodong_reward_view _msg ;
	// Use this for initialization


	void Awake () {

		_instance = this;
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void OnEnable()
	{

		InvokeRepeating ("time", 0.0f, 1.0f);
        protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view();
        _msg.id = m_huodong_id;
        net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_LJCZ_VIEW, _msg);
		
	}
    public void init(bool resert)
    {
        if (resert || this.gameObject.activeSelf)
        {
            protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view();
            _msg.id = m_huodong_id;
            net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_LJCZ_VIEW, _msg);
        }
       
    }
	void OnDisable()
	{
		CancelInvoke ("time");
       
        sys._instance.remove_child(m_view);
    
	}

	public void click(GameObject obj)
	{
		if (obj.transform.name == "close") 
		{
			
			s_message _message = new s_message ();
			_message.m_type = "show_jc_huodong";
			cmessage_center._instance.add_message (_message);

		}
	}

	void time()
	{
		m_end_time = 0;
		for(int i = 0; i < sys._instance.m_self.m_huodong_ids.Count;++i)
		{
			if(m_huodong_id == sys._instance.m_self.m_huodong_ids[i])
			{
				m_end_time = sys._instance.m_self.m_end_time[i];
				break;
			}
		}
		long _time = (long)(m_end_time - timer.now());
		if(_time <= 0)
		{
			m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
		}
		else
		{
			m_time.GetComponent<UILabel>().text = timer.get_time_show_ex(_time);
		}
	}
    
	public void reset()
	{
		int cz_jewel = 0;
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		_msg.subs.Sort (comp);
		for(int i = 0;i < _msg.subs.Count;++i )
		{
			cz_jewel = _msg.subs[i].arg3;
			
			GameObject _obj = game_data._instance.ins_object_res("ui/chongzhifali_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,96 - i * 143,1);
			_obj.GetComponent< chongzhifali_sub>().m_id = _msg.subs[i].id;
			_obj.GetComponent<chongzhifali_sub>().m_need_jewel = _msg.subs[i].arg1;
			_obj.GetComponent<chongzhifali_sub>().m_can_get = _msg.subs[i].arg2;
			_obj.GetComponent<chongzhifali_sub>().m_have_jewel = _msg.subs[i].arg3;
			_obj.GetComponent<chongzhifali_sub>().rtype.Clear();
			_obj.GetComponent<chongzhifali_sub>().rvalue1.Clear();
			_obj.GetComponent<chongzhifali_sub>().rvalue2.Clear();
			_obj.GetComponent<chongzhifali_sub>().rvalue3.Clear();
			for(int j =0; j < _msg.subs[i].types.Count; ++j )
			{
				_obj.GetComponent<chongzhifali_sub>().rtype.Add(_msg.subs[i].types[j]);
			}
			for(int j =0; j < _msg.subs[i].value1s.Count; ++j )
			{
				_obj.GetComponent<chongzhifali_sub>().rvalue1.Add(_msg.subs[i].value1s[j]);
			}
			for(int j =0; j < _msg.subs[i].value2s.Count; ++j )
			{
				_obj.GetComponent<chongzhifali_sub>().rvalue2.Add(_msg.subs[i].value2s[j]);
			}
			for(int j =0; j < _msg.subs[i].value3s.Count; ++j )
			{
				_obj.GetComponent<chongzhifali_sub>().rvalue3.Add(_msg.subs[i].value3s[j]);
			}

			_obj.GetComponent<chongzhifali_sub>().reset();

			sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, i * 0.05f);
			
		}
		m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("chongzhifali_gui.cs_127_54"),cz_jewel.ToString ());//{0}元
       
	}

	public int comp(protocol.game.huodong_reward_sub sub1,protocol.game.huodong_reward_sub sub2)
	{
		if(sub1.arg2 != 1 && sub2.arg2 == 1)
		{
			return -1;
		}
		else if(sub1.arg2 == 1 && sub2.arg2 != 1)
		{
			return 1;
		}
		else if(sub1.arg1 <= sub1.arg3 && sub2.arg1 > sub2.arg3)
		{
			return -1;
		}
		else if(sub1.arg1 > sub1.arg3 && sub2.arg1 <= sub2.arg3)
		{
			return 1;
		}
		else if(sub1.arg1 < sub2.arg1)
		{
			return -1;
		}
		else if(sub2.arg1 < sub1.arg1)
		{
			return 1;
		}
		return 0;
	}

	void IMessage.message (s_message message)
	{
		if(message.m_type == "huodong_czfl_lj")
		{
			m_id = (int)message.m_ints[0];
			protocol.game.cmsg_huodong_reward _msg = new protocol.game.cmsg_huodong_reward ();
			_msg.id = m_id;
			_msg.huodong = m_huodong_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_LJCZ, _msg);
		}
		else if (message.m_type == "huodong_czfl_recharge")
		{
			
			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);

			s_message _mes1 = new s_message();
			_mes1.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_mes1);
		}
		
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_LJCZ)
		{
			protocol.game.smsg_huodong_reward  _treasures= net_http._instance.parse_packet<protocol.game.smsg_huodong_reward>(message.m_byte);
			for(int i = 0;i < _treasures.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_treasures.treasures[i]);
			}
			for(int i = 0;i < _treasures.equips.Count;i++)
			{
				sys._instance.m_self.add_equip(_treasures.equips[i]);
			}
            for (int i = 0; i < _treasures.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_treasures.pets[i]);
            }
			//s_t_huodong_czfl t_huodong_czfl = game_data._instance.get_t_huodong_czfl (m_id);
			int a = 0; 
			for(a = 0; a < _msg.subs.Count; a++)
			{
				if(_msg.subs[a].id == m_id)
				{
					break;
				}
			}
			_msg.subs[a].arg2 = 1;
			for(int i = 0;i <_msg.subs[a].types.Count;i ++)
			{
				sys._instance.m_self.add_reward(_msg.subs[a].types[i],_msg.subs[a].value1s[i]
				                                ,_msg.subs[a].value2s[i], _msg.subs[a].value3s[i],game_data._instance.get_t_language ("chongzhifali_gui.cs_213_86"));//充值返利获得
			}

			reset ();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_LJCZ_VIEW)
		{
			_msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_reward_view> (message.m_byte);
			m_title.GetComponent<UILabel>().text = _msg.name;
			m_title1.GetComponent<UILabel>().text = _msg.name;
			reset ();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
