
using UnityEngine;
using System.Collections;

public class zkfs_gui : MonoBehaviour,IMessage {

	public GameObject m_view;
	public GameObject m_time;
	public GameObject m_title;
	public GameObject m_title1;
	public GameObject m_buy_num_gui;
	public GameObject m_input;
	public GameObject m_desc;
	public GameObject m_num;
	public GameObject m_name;
	public GameObject m_zongjia;
	public GameObject m_icon;
	public int m_huodong_id;
	private int m_id;
	private int m_index;
	private bool flag = false;
	public static ulong m_end_time;
	protocol.game.smsg_huodong_reward_view _msg;
	private int total_num = 0;
	private int m_input_num = 0;
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
        net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_EXCHANGE_VIEW, _msg);
	}
    public void init(bool resert)
    {
        if (resert || this.gameObject.activeSelf)
        {
            protocol.game.cmsg_huodong_reward_view _msg = new protocol.game.cmsg_huodong_reward_view();
            _msg.id = m_huodong_id;
            net_http._instance.send_msg<protocol.game.cmsg_huodong_reward_view>(opclient_t.CMSG_HUODONG_EXCHANGE_VIEW, _msg);
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
			int a = 0; 
			for(a = 0; a < _msg.subs.Count; a++)
			{
				if(_msg.subs[a].id == m_id)
				{
					break;
				}
			}
			if(sys._instance.m_self.m_t_player.jewel < _msg.subs[a].arg1*m_input_num)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide();
			protocol.game.cmsg_huodong_reward _msg1 = new protocol.game.cmsg_huodong_reward ();
			_msg1.id = m_id;
			_msg1.huodong = m_huodong_id;
			_msg1.index = m_index;
			_msg1.num = m_input_num;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_EXCHANGE, _msg1);
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
		m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (1,2,0,0);
		m_desc.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("jieri_duihuan_gui.cs_300_26")," "+sys._instance.m_self.get_item_num (1,2,1,0));//[0af6ff]当前拥有:[-]{0}
		m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),total_num.ToString () );//{0}次
		string color = sys._instance.get_res_color (2);
		if(sys._instance.m_self.m_t_player.jewel < _msg.subs[a].arg1*m_input_num)
		{
			color = "[ff0000]";
		}
		m_zongjia.GetComponent<UILabel>().text = color + "X" +(_msg.subs [a].arg1 * m_input_num).ToString ();
		sys._instance.remove_child (m_icon);
		GameObject _obj1 = icon_manager._instance.create_reward_icon(1,2,_msg.subs[a].arg1*m_input_num,0);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3 (0, 0, 0);
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
			GameObject _obj = game_data._instance.ins_object_res("ui/zkfs_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,111 - i * 134,1);
			_obj.GetComponent<zkfs_sub>().m_id = _msg.subs[i].id;
			_obj.GetComponent<zkfs_sub>().price = _msg.subs[i].arg1;
			_obj.GetComponent<zkfs_sub>().m_count = _msg.subs[i].arg2;
			_obj.GetComponent<zkfs_sub>().m_toltal_count = _msg.subs[i].arg3;
			_obj.GetComponent<zkfs_sub>().is_end = flag;
			_obj.GetComponent<zkfs_sub>().flag = _msg.subs[i].arg4;
			_obj.GetComponent<zkfs_sub>().discount = _msg.subs[i].arg5;
			_obj.GetComponent<zkfs_sub>().m_scro = m_view; 
			_obj.GetComponent<zkfs_sub>().rewards.Clear();
			for(int j =0; j < _msg.subs[i].types.Count; ++j )
			{
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = _msg.subs[i].types[j];
				t_reward.value1 = _msg.subs[i].value1s[j];
				t_reward.value2 = _msg.subs[i].value2s[j];
				t_reward.value3 = _msg.subs[i].value3s[j];
				_obj.GetComponent<zkfs_sub>().rewards.Add(t_reward);
			}
			_obj.GetComponent<zkfs_sub>().reset();
			
			sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, i * 0.05f);
		}
	}
   

	public int comp(protocol.game.huodong_reward_sub sub1,protocol.game.huodong_reward_sub sub2)
	{
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
		int jewel = sys._instance.m_self.m_t_player.jewel;
		if(sub1.arg2 < sub1.arg3 && sub2.arg2  >= sub2.arg3)
		{
			return -1;
		}
		else if(sub1.arg2 >= sub1.arg3 && sub2.arg2  < sub2.arg3)
		{
			return 1;
		}
		else if(sub1.arg1 <= jewel && sub2.arg1 > jewel)
		{
			return -1;
		}
		else if(sub1.arg1 > jewel && sub2.arg1 <= jewel)
		{
			return 1;
		}
		else if(id1 < id2)
		{
			return -1;
		}
		else if(id1 > id2)
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
		return 0;
	}
	
	void IMessage.message (s_message message)
	{
		
		if(message.m_type == "select_zkfs")
		{
			m_id = (int)message.m_ints[1];
			int a = 0; 
			for(a = 0; a < _msg.subs.Count; a++)
			{
				if(_msg.subs[a].id == m_id)
				{
					break;
				}
			}
			m_index = (int)message.m_ints[0];
			if(_msg.subs[a].arg4 == 1)
			{
				m_input_num = (int)message.m_ints[2];
				m_id = (int)message.m_ints[1];
				protocol.game.cmsg_huodong_reward _msg1 = new protocol.game.cmsg_huodong_reward ();
				_msg1.id = m_id;
				_msg1.huodong = m_huodong_id;
				_msg1.index = m_index;
				_msg1.num = m_input_num;
				net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_EXCHANGE, _msg1);
				return;
			}
			total_num = (int)message.m_ints[2];
			if(total_num > 1000)
			{
				total_num = 1000;
			}
			m_input_num = 1;
			buy_num();
			m_buy_num_gui.SetActive(true);
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_EXCHANGE && this.transform.gameObject.activeSelf)
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
					//_msg.subs[a].count +=1;
					break;
				}
			}
			_msg.subs[a].arg2 += m_input_num;
            string des = "";
			if(_msg.subs[a].arg4 == 1)
			{
				sys._instance.m_self.add_reward(_msg.subs[a].types[m_index],_msg.subs[a].value1s[m_index]
			                                ,_msg.subs[a].value2s[m_index]*m_input_num, _msg.subs[a].value3s[m_index],_msg.name + game_data._instance.get_t_language ("zkfs_gui.cs_348_121"));//活动获得
                des += sys._instance.m_self.get_name(_msg.subs[a].types[m_index], _msg.subs[a].value1s[m_index]
                                                , _msg.subs[a].value2s[m_index] * m_input_num, _msg.subs[a].value3s[m_index]);
            }
			else
			{
				for(int i = 0;i <_msg.subs[a].types.Count;i ++)
				{
					sys._instance.m_self.add_reward(_msg.subs[a].types[i],_msg.subs[a].value1s[i]
					                                ,_msg.subs[a].value2s[i]*m_input_num, _msg.subs[a].value3s[i],_msg.name + game_data._instance.get_t_language ("bingyuan_gui.cs_776_19"));//获得
                    des += sys._instance.m_self.get_name(_msg.subs[a].types[i], _msg.subs[a].value1s[i]
                                                    , _msg.subs[a].value2s[i] * m_input_num, _msg.subs[a].value3s[i]);
				}
			}
          
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, _msg.subs[a].arg1 * m_input_num, _msg.name +  game_data._instance.get_t_language(game_data._instance.get_t_language ("jieri_duihuan_gui.cs_343_109")) + ":" + des);//消耗
           
			reset();
		}
		if(message.m_opcode == opclient_t.CMSG_HUODONG_EXCHANGE_VIEW)
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
