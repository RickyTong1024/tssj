
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class jieri_duihuan_gui : MonoBehaviour,IMessage {

	public GameObject m_view;
	public GameObject m_time;
	public GameObject m_title;
	public GameObject m_title1;
	public GameObject m_buy_num_gui;
	public GameObject m_name;
	public GameObject m_desc;
	public GameObject m_num;
	public GameObject m_input;
	public GameObject m_icon;
	public int m_huodong_id;
	private int m_id;
	public static ulong m_end_time;
	private int total_num = 0;
	private int m_input_num = 0;
	private bool flag = false;
	protocol.game.smsg_huodong_reward_view _msg;
	
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	void OnEnable()
	{
		InvokeRepeating ("time", 0.0f, 1.0f);
		protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view ();
		_msg.id = m_huodong_id;
		net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view> (opclient_t.CMSG_HUODONG_DJDH_VIEW, _msg);
	}
	
	void OnDisable()
	{
		CancelInvoke ("time");
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
		long _time = (long)(m_end_time - timer.now());
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
		if (obj.name == "add") 
		{
			if(m_input_num + 1 <= total_num)
			{
				m_input_num += 1;
				buy_num();
			}
		}
		else if(obj.name == "sub")
		{
			if (m_input_num > 1)
			{
				m_input_num--;
				buy_num();
			}
		}
		else if(obj.name == "add10")
		{
			if(m_input_num + 10 <= total_num)
			{
				m_input_num += 10;
			}
			else
			{
				m_input_num = total_num;
			}
			buy_num();
		}
		else if(obj.name == "sub10")
		{
			if (m_input_num - 10 >= 1) 
			{
				m_input_num -= 10;
			}
			else
			{
				m_input_num = 1;
			}
			buy_num();
		}
		else if(obj.name == "queding")
		{
			m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide();
			protocol.game.cmsg_huodong_reward _msg = new protocol.game.cmsg_huodong_reward ();
			_msg.id = m_id;
			_msg.huodong = m_huodong_id;
			_msg.index = m_input_num;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_DJDH, _msg);
		}
		else if(obj.name == "hide")
		{
			m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if(obj.name == "cancle")
		{
			m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide();
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
			GameObject _obj = game_data._instance.ins_object_res("ui/jieri_duihuan_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,131 - i * 134,1);
			_obj.GetComponent<jieri_duihuan_sub>().m_id = _msg.subs[i].id;
			_obj.GetComponent<jieri_duihuan_sub>().m_count = _msg.subs[i].arg1;
			_obj.GetComponent<jieri_duihuan_sub>().m_toltal_count = _msg.subs[i].arg2;
			_obj.GetComponent<jieri_duihuan_sub>().is_end = flag;
			_obj.GetComponent<jieri_duihuan_sub>().m_scro = m_view; 
			_obj.GetComponent<jieri_duihuan_sub>().rewards.Clear();
			_obj.GetComponent<jieri_duihuan_sub>().reward1s.Clear();
			for(int j =0; j < _msg.subs[i].types.Count; ++j )
			{
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = _msg.subs[i].types[j];
				t_reward.value1 = _msg.subs[i].value1s[j];
				t_reward.value2 = _msg.subs[i].value2s[j];
				t_reward.value3 = _msg.subs[i].value3s[j];
				_obj.GetComponent<jieri_duihuan_sub>().reward1s.Add(t_reward);
			}
			for(int j =0; j <  _msg.subs[i].arg6.Count; ++j )
			{
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = _msg.subs[i].arg6[j];
				t_reward.value1 = _msg.subs[i].arg7[j];
				t_reward.value2 = _msg.subs[i].arg8[j];
				t_reward.value3 = 0;
				_obj.GetComponent<jieri_duihuan_sub>().rewards.Add(t_reward);
			}
			_obj.GetComponent<jieri_duihuan_sub>().reset();
			
			sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, i * 0.05f);
		}
	}
	
	public int comp(protocol.game.huodong_reward_sub sub1,protocol.game.huodong_reward_sub sub2)
	{
		int num = 0;
		int num1 = 0;
		int max_num = 9999;
		int max_num1 = 9999;
		for (int i = 0; i < sub1.arg6.Count; ++i)
		{
			num = sys._instance.m_self.get_item_num((uint)sub1.arg7[i])/ sub1.arg8[i];
			if(num < max_num)
			{
				max_num = num;
			}
		}
		for (int i = 0; i < sub2.arg6.Count; ++i)
		{
			num1 = sys._instance.m_self.get_item_num((uint)sub2.arg7[i])/ sub2.arg8[i];
			if(num1 < max_num1)
			{
				max_num1 = num1;
			}
		}
		int id1 = 0;
		int id2 = 0;
		for(int i = 0;i < _msg.subs.Count ; ++i )
		{
			if(_msg.subs[i].id == sub1.id)
			{
				id1 = i;
				break;
			}
		}
		for(int i = 0;i < _msg.subs.Count ; ++i )
		{
			if(_msg.subs[i].id == sub2.id)
			{
				id2 = i;
				break;
			}
		}
		if(sub1.arg1 < sub1.arg2 && sub2.arg1  >= sub2.arg2)
		{
			return -1;
		}
		else if(sub1.arg1 >= sub1.arg2 && sub2.arg1  < sub2.arg2)
		{
			return 1;
		}
		else if(max_num != 0 && max_num1 == 0)
		{
			return -1;
		}
		else if(max_num == 0 && max_num1 != 0)
		{
			return 1;
		}
		else
		{
			return id1 - id2;
		}
	}
	
	void IMessage.message (s_message message)
	{
		if(message.m_type == "select_jieri_duihuan")
		{
			m_id = (int)message.m_ints[0];
			total_num = 1000;
			m_input_num = 1;
			int num = 0; 
			int a = 0; 
			for(a = 0; a < _msg.subs.Count; a++)
			{
				if(_msg.subs[a].id == m_id)
				{
					break;
				}
			}
			for (int i = 0; i < _msg.subs[a].arg6.Count; ++i)
			{
				num = sys._instance.m_self.get_item_num((uint)_msg.subs[a].arg7[i])/_msg.subs[a].arg8[i];
				if(num < total_num)
				{
					total_num = num;
				}
			}
			if(total_num >  _msg.subs[a].arg2 -  _msg.subs[a].arg1)
			{
				total_num =  _msg.subs[a].arg2 -  _msg.subs[a].arg1;
			}
			buy_num();
			m_buy_num_gui.SetActive(true);
		}
	}

	public void buy_num()
	{
		int a = 0; 
		for(a = 0; a < _msg.subs.Count; a++)
		{
			if(_msg.subs[a].id == m_id)
			{
				break;
			}
		}
		m_input.GetComponent<UILabel>().text = m_input_num.ToString ();
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (_msg.subs[a].types[0],_msg.subs[a].value1s[0],_msg.subs[a].value2s[0],_msg.subs[a].value3s[0]);
		m_desc.GetComponent<UILabel>().text = 
			string.Format(game_data._instance.get_t_language ("jieri_duihuan_gui.cs_300_26"), " "+sys._instance.m_self.get_item_num (_msg.subs[a].types[0],_msg.subs[a].value1s[0],_msg.subs[a].value2s[0],_msg.subs[a].value3s[0]));//[0af6ff]当前拥有:[-]{0}
		m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),total_num.ToString ());//{0}次
		sys._instance.remove_child (m_icon);
		GameObject _obj1 = icon_manager._instance.create_reward_icon(_msg.subs[a].types[0],_msg.subs[a].value1s[0],_msg.subs[a].value2s[0],_msg.subs[a].value3s[0]);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3 (0, 0, 0);
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_DJDH && this.transform.gameObject.activeSelf)
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
			_msg.subs[a].arg1 += m_input_num;
			for(int i = 0;i < _msg.subs[a].types.Count;++i)
			{
				sys._instance.m_self.add_reward(_msg.subs[a].types[i],_msg.subs[a].value1s[i]
					                                ,_msg.subs[a].value2s[i]*m_input_num, _msg.subs[a].value3s[i],_msg.name + game_data._instance.get_t_language ("bingyuan_gui.cs_776_19"));//获得
			}
			for(int i = 0;i <_msg.subs[a].arg6.Count;i ++)
			{
				sys._instance.m_self.remove_item((uint)_msg.subs[a].arg7[i],_msg.subs[a].arg8[i]*m_input_num,_msg.name + game_data._instance.get_t_language ("jieri_duihuan_gui.cs_343_109"));//消耗
			}
			reset();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_DJDH_VIEW)
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
