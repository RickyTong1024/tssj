
using UnityEngine;
using System.Collections;

public class land_gift_gui : MonoBehaviour ,IMessage{

	public GameObject m_view;
	public GameObject m_time;
	public GameObject m_title;
	public GameObject m_title1;
	public int m_huodong_id;
	private int m_id;
	private int m_index;
	public static ulong m_end_time;
	protocol.game.smsg_huodong_reward_view _msg;
	// Use this for initialization
	void Awake ()
    {
		cmessage_center._instance.add_handle (this);
	}

	void OnEnable()
	{
		InvokeRepeating ("time", 0.0f, 1.0f);
        protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view();
        _msg.id = m_huodong_id;
        net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_DLSL_VIEW, _msg);
		
	}
    public void init(bool resert)
    {
        if (resert || this.gameObject.activeSelf)
        {
            protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view();
            _msg.id = m_huodong_id;
            net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_DLSL_VIEW, _msg);

        }
    }

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnDisable()
	{
		CancelInvoke ("time");
        sys._instance.remove_child(m_view);
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

	public void click(GameObject obj)
	{
		if (obj.transform.name == "close") 
		{
			s_message _message = new s_message ();
			_message.m_type = "show_jc_huodong";
			cmessage_center._instance.add_message (_message);
			
		}
	}
	
	public void reset()
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		_msg.subs.Sort (comp);
		for(int i = 0;i < _msg.subs.Count ; ++i )
		{
			GameObject _obj = game_data._instance.ins_object_res("ui/land_gif_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,106 - i * 154,1);
			_obj.GetComponent<land_gift_sub>().m_id = _msg.subs[i].id;
			_obj.GetComponent<land_gift_sub>().m_need_day = _msg.subs[i].arg1;
			_obj.GetComponent<land_gift_sub>().m_can_get = _msg.subs[i].arg2;
			_obj.GetComponent<land_gift_sub>().m_land_day = _msg.subs[i].arg3;
			_obj.GetComponent<land_gift_sub>().m_is_chose = _msg.subs[i].arg4;
			_obj.GetComponent<land_gift_sub>().m_scro = m_view; 
			_obj.GetComponent<land_gift_sub>().rewards.Clear();
			for(int j =0; j < _msg.subs[i].types.Count; ++j )
			{
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = _msg.subs[i].types[j];
				t_reward.value1 = _msg.subs[i].value1s[j];
				t_reward.value2 = _msg.subs[i].value2s[j];
				t_reward.value3 = _msg.subs[i].value3s[j];
				_obj.GetComponent<land_gift_sub>().rewards.Add(t_reward);
			}
			_obj.GetComponent<land_gift_sub>().reset();
			
			sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, i * 0.05f);
		}
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
		if(message.m_type == "huodong_land_lj")
		{
			m_index = (int)message.m_ints[0];
			m_id = (int)message.m_ints[1];
			protocol.game.cmsg_huodong_reward _msg = new protocol.game.cmsg_huodong_reward ();
			_msg.id = m_id;
			_msg.index = m_index;
			_msg.huodong = m_huodong_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_DLSL, _msg);
		}

		
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_DLSL)
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
			int a = 0; 
			for(a = 0; a < _msg.subs.Count; a++)
			{
				if(_msg.subs[a].id == m_id)
				{
					break;
				}
			}
			_msg.subs[a].arg2 = 1;
			if(_msg.subs[a].arg4 == 1)
			{
				sys._instance.m_self.add_reward(_msg.subs[a].types[m_index],_msg.subs[a].value1s[m_index]
				                                ,_msg.subs[a].value2s[m_index], _msg.subs[a].value3s[m_index],_msg.name + game_data._instance.get_t_language ("bingyuan_gui.cs_776_19"));//获得
			}
			else
			{
				for(int i = 0;i <_msg.subs[a].types.Count;i ++)
				{
					sys._instance.m_self.add_reward(_msg.subs[a].types[i],_msg.subs[a].value1s[i]
					                                ,_msg.subs[a].value2s[i], _msg.subs[a].value3s[i],_msg.name + game_data._instance.get_t_language ("bingyuan_gui.cs_776_19"));//获得
				}
			}
			reset();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_DLSL_VIEW)
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
