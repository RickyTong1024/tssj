
using UnityEngine;
using System.Collections;

public class huo_yue_gui : MonoBehaviour ,IMessage{

	public GameObject m_view;
	public GameObject m_time;
	public int m_huodong_id;
	private int m_id;
	private bool flag = false;
	public static ulong m_end_time;
	protocol.game.smsg_huodong_reward_view _msg;
	
	public UILabel m_huodongshijian;
	public UILabel m_huodongjieshao;
	
	
	// Use this for initialization
	void Awake () {
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
        net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_HYHD_VIEW, _msg);
		
	}
    public void init(bool resert)
    {
        if (resert || this.gameObject.activeSelf)
        {
            protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view();
            _msg.id = m_huodong_id;
            net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_HYHD_VIEW, _msg);
        }
       
    }
	
	void OnDisable()
	{
		CancelInvoke ("time");
        sys._instance.remove_child(m_view);
	}

	void time()
	{
		flag = false;
		m_end_time = 0;
		for(int i = 0; i < sys._instance.m_self.m_huodong_ids.Count;++i)
		{
			if(m_huodong_id == sys._instance.m_self.m_huodong_ids[i])
			{
				m_end_time = sys._instance.m_self.m_end_time[i];
				break;
			}
		}
		long _time = (long)(m_end_time - timer.now ());
		if(_time <= 0)
		{
			flag = true;
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
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
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
			GameObject _obj = game_data._instance.ins_object_res("ui/huo_yue_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,106 - i * 154,1);
			_obj.GetComponent<huo_yue_sub>().m_id = _msg.subs[i].id;
			_obj.GetComponent<huo_yue_sub>().m_type = _msg.subs[i].arg1;
			_obj.GetComponent<huo_yue_sub>().m_self_num = _msg.subs[i].arg2;
			_obj.GetComponent<huo_yue_sub>().m_can_get = _msg.subs[i].arg3;
			_obj.GetComponent<huo_yue_sub>().m_toltal_count = _msg.subs[i].arg4;
			_obj.GetComponent<huo_yue_sub>().is_end = flag;
			_obj.GetComponent<huo_yue_sub>().rtype.Clear();
			_obj.GetComponent<huo_yue_sub>().rvalue1.Clear();
			_obj.GetComponent<huo_yue_sub>().rvalue2.Clear();
			_obj.GetComponent<huo_yue_sub>().rvalue3.Clear();
			for(int j =0; j < _msg.subs[i].types.Count; ++j )
			{
				_obj.GetComponent<huo_yue_sub>().rtype.Add(_msg.subs[i].types[j]);
			}
			for(int j =0; j < _msg.subs[i].value1s.Count; ++j )
			{
				_obj.GetComponent<huo_yue_sub>().rvalue1.Add(_msg.subs[i].value1s[j]);
			}
			for(int j =0; j < _msg.subs[i].value2s.Count; ++j )
			{
				_obj.GetComponent<huo_yue_sub>().rvalue2.Add(_msg.subs[i].value2s[j]);
			}
			for(int j =0; j < _msg.subs[i].value3s.Count; ++j )
			{
				_obj.GetComponent<huo_yue_sub>().rvalue3.Add(_msg.subs[i].value3s[j]);
			}
			_obj.GetComponent<huo_yue_sub>().reset();
			
			sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, i * 0.05f);
		}
	}

  	static bool can_duihuan(protocol.game.huodong_reward_sub sub)
	{
		if(sub.arg3 == 0 && sub.arg2 >= sub.arg4)
		{
			return true;
		}
		return false;
	}

	public static int comp(protocol.game.huodong_reward_sub sub1,protocol.game.huodong_reward_sub sub2)
	{
		if(can_duihuan(sub1) && !can_duihuan(sub2))
		{
			return -1;
		}
		else if(!can_duihuan(sub1) && can_duihuan(sub2))
		{
			return 1;
		}
		else if(sub1.arg3 == 0 && sub2.arg3 == 1)
		{
			return -1;
		}
		else if(sub1.arg3 == 1 && sub2.arg3 == 0)
		{
			return 1;
		}
		else if(sub1.arg1 > sub2.arg1)
		{
			return 1;
		}
		else if(sub1.arg1 < sub2.arg1)
		{
			return -1;
		}
		else if(sub1.arg1 > sub2.arg1)
		{
			return 1;
		}
		else if(sub1.arg4 < sub2.arg4)
		{
			return -1;
		}
		else if(sub2.arg4 < sub1.arg4)
		{
			return 1;
		}
		return 0;
	}
    
	void IMessage.message (s_message message)
	{
		if(message.m_type == "huodong_huoyue_lj")
		{
			m_id = (int)message.m_ints[0];
			protocol.game.cmsg_huodong_reward _msg = new protocol.game.cmsg_huodong_reward ();
			_msg.id = m_id;
			_msg.huodong = m_huodong_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_HYHD, _msg);
		}
		if(message.m_type == "hide_huoyue_huodong")
		{
			
		}
		
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_HYHD && this.transform.gameObject.activeSelf)
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
			_msg.subs[a].arg3 = 1;
			for(int i = 0;i <_msg.subs[a].types.Count;i ++)
			{
				sys._instance.m_self.add_reward(_msg.subs[a].types[i],_msg.subs[a].value1s[i]
				                                ,_msg.subs[a].value2s[i], _msg.subs[a].value3s[i],_msg.name + game_data._instance.get_t_language ("bingyuan_gui.cs_776_19"));//获得
			}
			reset();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_HYHD_VIEW)
		{
			_msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_reward_view> (message.m_byte);
            jc_huodong_gui._instance.top_lable_front.text = _msg.name;
            jc_huodong_gui._instance.top_title_lable.text = _msg.name;
			reset ();
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
