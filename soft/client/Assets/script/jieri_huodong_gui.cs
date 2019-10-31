
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class jieri_huodong_gui : MonoBehaviour,IMessage {

	public GameObject m_view;
	public GameObject m_time;
	public GameObject m_title;
	public GameObject m_title1;
	public List<GameObject> m_names = new List<GameObject>();
	public List<GameObject> m_name1s = new List<GameObject>();
	public static int m_huodong_id;
	private int m_id;
	private int m_index;
	public ulong m_end_time;
	public GameObject m_buy_num_gui;
	public GameObject m_name;
	public GameObject m_desc;
	public GameObject m_num;
	public GameObject m_input;
	public GameObject m_icon;
	public GameObject m_effect1;
	public GameObject m_effect2;
	public GameObject m_effect3;
	public GameObject m_effect4;
	public GameObject m_money_type;
	public GameObject m_zongjia;
	public bool flag = false;
	private int total_num = 0;
	private int m_input_num = 0;
	protocol.game.smsg_huodong_jiri_view _msg;
	protocol.game.huodong_reward_sub sub;  
	private int m_select = 0;
	private int type = 0;
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void OnDisable()
	{
		CancelInvoke ("time");
	}

	void OnEnable()
	{
		m_select = 1;
		InvokeRepeating ("time", 0.0f, 1.0f);
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_JIERI_VIEW, _msg);
	}

	void time()
	{
		long _time = (long)(m_end_time - timer.now());
		m_time.GetComponent<UILabel>().text = timer.get_time_show_ex(_time);
		flag = false;
		if(_time <= 0)
		{
			flag = true;
			m_time.GetComponent<UILabel>().text = game_data._instance.get_t_language ("chongzhifali_gui.cs_68_42");//活动已结束
		}
	}
	
	public void reset(int id)
	{
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		if(_msg.huodongs.Count == 0)
		{
			return;
		}
		m_end_time = _msg.huodongs[id -1].time;
		time ();
		sys._instance.remove_child (m_view);
		if(id == 1)
		{
			int a = 0;
			List<protocol.game.huodong_reward_sub> m_huodong_subs = new List<protocol.game.huodong_reward_sub>();
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 9)
				{
					for(int j = 0; j < _msg.huodongs[i].reward.subs.Count;++j)
					{
						if(_msg.huodongs[i].reward.subs[j].arg4 == 0 && _msg.huodongs[i].reward.subs[j].arg3 == 1)
						{
							m_huodong_subs.Add(_msg.huodongs[i].reward.subs[j]);
						}
					}
				}
			}
			m_huodong_subs.Sort(comp);
			riqi(m_huodong_subs,a);
			a += m_huodong_subs.Count;
			m_huodong_subs.Clear();
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 5)
				{
					for(int j = 0; j < _msg.huodongs[i].reward.subs.Count;++j)
					{
						if(_msg.huodongs[i].reward.subs[j].arg3 < _msg.huodongs[i].reward.subs[j].arg4 && _msg.huodongs[i].reward.subs[j].arg2 > _msg.huodongs[i].reward.subs[j].arg3 )
						{
							m_huodong_subs.Add(_msg.huodongs[i].reward.subs[j]);
						}
					}
				}
			}
			m_huodong_subs.Sort(dbcz_gui.comp);
			dbcz(m_huodong_subs,a);
			a += m_huodong_subs.Count;
			m_huodong_subs.Clear();
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 9)
				{
					for(int j = 0; j < _msg.huodongs[i].reward.subs.Count;++j)
					{
						if(_msg.huodongs[i].reward.subs[j].arg4 == 0 && _msg.huodongs[i].reward.subs[j].arg3 == 0)
						{
							m_huodong_subs.Add(_msg.huodongs[i].reward.subs[j]);
						}
					}
				}
			}
			m_huodong_subs.Sort(comp);
			riqi(m_huodong_subs,a);
			a += m_huodong_subs.Count;
			m_huodong_subs.Clear();
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 5)
				{
					for(int j = 0; j < _msg.huodongs[i].reward.subs.Count;++j)
					{
						if(_msg.huodongs[i].reward.subs[j].arg3 < _msg.huodongs[i].reward.subs[j].arg4 && _msg.huodongs[i].reward.subs[j].arg2 <= _msg.huodongs[i].reward.subs[j].arg3)
						{
							m_huodong_subs.Add(_msg.huodongs[i].reward.subs[j]);
						}
					}
				}
			}
			m_huodong_subs.Sort(dbcz_gui.comp);
			dbcz(m_huodong_subs,a);
			a += m_huodong_subs.Count;
			m_huodong_subs.Clear();
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 9)
				{
					for(int j = 0; j < _msg.huodongs[i].reward.subs.Count;++j)
					{
						if(_msg.huodongs[i].reward.subs[j].arg4 == 1 && _msg.huodongs[i].reward.subs[j].arg3 == 1)
						{
							m_huodong_subs.Add(_msg.huodongs[i].reward.subs[j]);
						}
					}
				}
			}
			m_huodong_subs.Sort(comp);
			riqi(m_huodong_subs,a);
			a += m_huodong_subs.Count;
			m_huodong_subs.Clear();
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 5)
				{
					for(int j = 0; j < _msg.huodongs[i].reward.subs.Count;++j)
					{
						if(_msg.huodongs[i].reward.subs[j].arg3 >= _msg.huodongs[i].reward.subs[j].arg4)
						{
							m_huodong_subs.Add(_msg.huodongs[i].reward.subs[j]);
						}
					}
				}
			}
			m_huodong_subs.Sort(dbcz_gui.comp);
			dbcz(m_huodong_subs,a);
			a += m_huodong_subs.Count;
			m_huodong_subs.Clear();
		}
		if(id == 2)
		{
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 4)
				{
					_msg.huodongs[i].reward.subs.Sort(huo_yue_gui.comp);
					for(int j = 0;j <_msg.huodongs[i].reward.subs.Count ; ++j )
					{
						GameObject _obj = game_data._instance.ins_object_res("ui/huo_yue_sub");
						
						_obj.transform.parent = m_view.transform;
						_obj.transform.localScale = new Vector3(1,1,1);
						_obj.transform.localPosition = new Vector3(0,111 - j * 153,1);
						_obj.GetComponent<huo_yue_sub>().m_id = _msg.huodongs[i].reward.subs[j].id;
						_obj.GetComponent<huo_yue_sub>().m_type =_msg.huodongs[i].reward.subs[j].arg1;
						_obj.GetComponent<huo_yue_sub>().m_self_num = _msg.huodongs[i].reward.subs[j].arg2;
						_obj.GetComponent<huo_yue_sub>().m_can_get = _msg.huodongs[i].reward.subs[j].arg3;
						_obj.GetComponent<huo_yue_sub>().m_toltal_count = _msg.huodongs[i].reward.subs[j].arg4;
						_obj.GetComponent<huo_yue_sub>().is_end = flag; 
						_obj.GetComponent<huo_yue_sub>().rtype.Clear();
						_obj.GetComponent<huo_yue_sub>().rvalue1.Clear();
						_obj.GetComponent<huo_yue_sub>().rvalue2.Clear();
						_obj.GetComponent<huo_yue_sub>().rvalue3.Clear();
						for(int k =0; k < _msg.huodongs[i].reward.subs[j].types.Count; ++k )
						{
							_obj.GetComponent<huo_yue_sub>().rtype.Add(_msg.huodongs[i].reward.subs[j].types[k]);
						}
						for(int k =0; k <  _msg.huodongs[i].reward.subs[j].value1s.Count; ++k )
						{
							_obj.GetComponent<huo_yue_sub>().rvalue1.Add( _msg.huodongs[i].reward.subs[j].value1s[k]);
						}
						for(int k =0; k <_msg.huodongs[i].reward.subs[j].value2s.Count; ++k )
						{
							_obj.GetComponent<huo_yue_sub>().rvalue2.Add(_msg.huodongs[i].reward.subs[j].value2s[k]);
						}
						for(int k =0; k < _msg.huodongs[i].reward.subs[j].value3s.Count; ++k )
						{
							_obj.GetComponent<huo_yue_sub>().rvalue3.Add(_msg.huodongs[i].reward.subs[j].value3s[k]);
						}
						_obj.GetComponent<huo_yue_sub>().reset();
						
						sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), j * 0.05f);
						sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, j * 0.05f);
					}
					break;
				}
			}
		}
		if(id == 3)
		{
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 7)
				{
					_msg.huodongs[i].reward.subs.Sort(zkfs_comp);
					for(int j = 0;j <_msg.huodongs[i].reward.subs.Count ; ++j )
					{
						GameObject _obj = game_data._instance.ins_object_res("ui/zkfs_sub");
						
						_obj.transform.parent = m_view.transform;
						_obj.transform.localScale = new Vector3(1,1,1);
						_obj.transform.localPosition = new Vector3(0,119 - j * 134,1);
						_obj.GetComponent<zkfs_sub>().m_id = _msg.huodongs[i].reward.subs[j].id;
						_obj.GetComponent<zkfs_sub>().price = _msg.huodongs[i].reward.subs[j].arg1;
						_obj.GetComponent<zkfs_sub>().m_count = _msg.huodongs[i].reward.subs[j].arg2;
						_obj.GetComponent<zkfs_sub>().m_toltal_count = _msg.huodongs[i].reward.subs[j].arg3;
						_obj.GetComponent<zkfs_sub>().flag = _msg.huodongs[i].reward.subs[j].arg4;
						_obj.GetComponent<zkfs_sub>().discount = _msg.huodongs[i].reward.subs[j].arg5;
						_obj.GetComponent<zkfs_sub>().is_end = flag;
						_obj.GetComponent<zkfs_sub>().m_scro = m_view; 
						_obj.GetComponent<zkfs_sub>().rewards.Clear();
						for(int k =0; k < _msg.huodongs[i].reward.subs[j].types.Count; ++k )
						{
							s_t_reward t_reward = new s_t_reward();
							t_reward.type = _msg.huodongs[i].reward.subs[j].types[k];
							t_reward.value1 = _msg.huodongs[i].reward.subs[j].value1s[k];
							t_reward.value2 = _msg.huodongs[i].reward.subs[j].value2s[k];
							t_reward.value3 = _msg.huodongs[i].reward.subs[j].value3s[k];
							_obj.GetComponent<zkfs_sub>().rewards.Add(t_reward);
						}
						_obj.GetComponent<zkfs_sub>().reset();
						
						sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), j * 0.05f);
						sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, j * 0.05f);
					}
					break;
				}
			}
		}
		if(id == 4)
		{
			for(int i = 0; i < _msg.huodongs.Count;++i)
			{
				if(_msg.huodongs[i].type == 8)
				{
					_msg.huodongs[i].reward.subs.Sort(duihuan_comp);
					for(int j = 0;j < _msg.huodongs[i].reward.subs.Count ; ++j )
					{
						GameObject _obj = game_data._instance.ins_object_res("ui/jieri_duihuan_sub");
						
						_obj.transform.parent = m_view.transform;
						_obj.transform.localScale = new Vector3(1,1,1);
						_obj.transform.localPosition = new Vector3(0,119 - j * 134,1);
						_obj.GetComponent<jieri_duihuan_sub>().m_id = _msg.huodongs[i].reward.subs[j].id;
						_obj.GetComponent<jieri_duihuan_sub>().m_count = _msg.huodongs[i].reward.subs[j].arg1;
						_obj.GetComponent<jieri_duihuan_sub>().m_toltal_count = _msg.huodongs[i].reward.subs[j].arg2;
						_obj.GetComponent<jieri_duihuan_sub>().m_scro = m_view; 
						_obj.GetComponent<jieri_duihuan_sub>().rewards.Clear();
						_obj.GetComponent<jieri_duihuan_sub>().reward1s.Clear();
						_obj.GetComponent<jieri_duihuan_sub>().is_end = flag;
						for(int k =0; k < _msg.huodongs[i].reward.subs[j].types.Count; ++k )
						{
							s_t_reward t_reward = new s_t_reward();
							t_reward.type = _msg.huodongs[i].reward.subs[j].types[k];
							t_reward.value1 = _msg.huodongs[i].reward.subs[j].value1s[k];
							t_reward.value2 = _msg.huodongs[i].reward.subs[j].value2s[k];
							t_reward.value3 = _msg.huodongs[i].reward.subs[j].value3s[k];
							_obj.GetComponent<jieri_duihuan_sub>().reward1s.Add(t_reward);
						}
						for(int k =0; k <  _msg.huodongs[i].reward.subs[j].arg6.Count; ++k )
						{
							s_t_reward t_reward = new s_t_reward();
							t_reward.type = _msg.huodongs[i].reward.subs[j].arg6[k];
							t_reward.value1 = _msg.huodongs[i].reward.subs[j].arg7[k];
							t_reward.value2 = _msg.huodongs[i].reward.subs[j].arg8[k];
							t_reward.value3 = 0;
							_obj.GetComponent<jieri_duihuan_sub>().rewards.Add(t_reward);
						}
						_obj.GetComponent<jieri_duihuan_sub>().reset();
						
						sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), j * 0.05f);
						sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, j * 0.05f);
					}
					break;
				}
			}
		}
		effect ();
	}

	public int duihuan_comp(protocol.game.huodong_reward_sub sub1,protocol.game.huodong_reward_sub sub2)
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
		protocol.game.smsg_huodong_reward_view _msg_huodong = new protocol.game.smsg_huodong_reward_view();
		for(int i = 0; i < _msg.huodongs.Count;++i)
		{
			if(_msg.huodongs[i].type == 7)
			{
				_msg_huodong = _msg.huodongs[i].reward;
				break;
			}
		}
		int id1 = 0;
		int id2 = 0;
		for(int i = 0;i < _msg_huodong.subs.Count ; ++i )
		{
			if(_msg_huodong.subs[i].id == sub1.id)
			{
				id1 = i;
				break;
			}
		}
		for(int i = 0;i < _msg_huodong.subs.Count ; ++i )
		{
			if(_msg_huodong.subs[i].id == sub2.id)
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
	public int zkfs_comp(protocol.game.huodong_reward_sub sub1,protocol.game.huodong_reward_sub sub2)
	{
		int id1 = 0;
		int id2 = 0;
		protocol.game.smsg_huodong_reward_view _msg_huodong = new protocol.game.smsg_huodong_reward_view();
		for(int i = 0; i < _msg.huodongs.Count;++i)
		{
			if(_msg.huodongs[i].type == 7)
			{
				_msg_huodong = _msg.huodongs[i].reward;
				break;
			}
		}
		for(int i = 0;i < _msg_huodong.subs.Count ; ++i )
		{
			if(_msg_huodong.subs[i].id == sub1.id)
			{
				id1 = i;
				break;
			}
		}
		for(int i = 0;i < _msg_huodong.subs.Count ; ++i )
		{
			if(_msg_huodong.subs[i].id == sub2.id)
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

	public void dbcz(List<protocol.game.huodong_reward_sub> m_huodong_subs, int num)
	{
		for (int i = 0; i < m_huodong_subs.Count; ++i) 
		{
			GameObject _obj = game_data._instance.ins_object_res ("ui/dbcz_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3 (1, 1, 1);
			_obj.transform.localPosition = new Vector3 (0, 111 - num * 153, 1);
			_obj.GetComponent<dbcz_sub>().m_id = m_huodong_subs [i].id;
			_obj.GetComponent<dbcz_sub>().m_need_jewel = m_huodong_subs [i].arg5;
			_obj.GetComponent<dbcz_sub>().m_cz_num = m_huodong_subs [i].arg2;
			_obj.GetComponent<dbcz_sub>().m_count = m_huodong_subs [i].arg3;
			_obj.GetComponent<dbcz_sub>().m_toltal_count = m_huodong_subs [i].arg4;
			_obj.GetComponent<dbcz_sub>().is_end = flag;
			_obj.GetComponent<dbcz_sub>().rtype.Clear ();
			_obj.GetComponent<dbcz_sub>().rvalue1.Clear ();
			_obj.GetComponent<dbcz_sub>().rvalue2.Clear ();
			_obj.GetComponent<dbcz_sub>().rvalue3.Clear ();
			for (int k =0; k < m_huodong_subs[i].types.Count; ++k)
			{
				_obj.GetComponent<dbcz_sub>().rtype.Add (m_huodong_subs [i].types [k]);
			}
			for (int k =0; k <  m_huodong_subs[i].value1s.Count; ++k) 
			{
				_obj.GetComponent<dbcz_sub>().rvalue1.Add (m_huodong_subs [i].value1s [k]);
			}
			for (int k =0; k <  m_huodong_subs[i].value2s.Count; ++k) 
			{
				_obj.GetComponent<dbcz_sub>().rvalue2.Add (m_huodong_subs [i].value2s [k]);
			}
			for (int k =0; k <  m_huodong_subs[i].value3s.Count; ++k) 
			{
				_obj.GetComponent<dbcz_sub>().rvalue3.Add (m_huodong_subs [i].value3s [k]);
			}
			_obj.GetComponent<dbcz_sub>().reset ();
			
			sys._instance.add_pos_anim (_obj, 0.3f, new Vector3 (-300, 0, 0), num * 0.05f);
			sys._instance.add_alpha_anim (_obj, 0.3f, 0, 1.0f, num * 0.05f);
			num++;
		}
	}

	public void riqi(List<protocol.game.huodong_reward_sub> m_huodong_subs, int num)
	{
		for(int i = 0;i < m_huodong_subs.Count ; ++i )
		{
			GameObject _obj = game_data._instance.ins_object_res("ui/riqi_sub");
			
			_obj.transform.parent = m_view.transform;
			_obj.transform.localScale = new Vector3(1,1,1);
			_obj.transform.localPosition = new Vector3(0,111 - num * 153,1);
			_obj.GetComponent<riqi_sub>().m_id = m_huodong_subs[i].id;
			_obj.GetComponent<riqi_sub>().m_month = m_huodong_subs[i].arg1;
			_obj.GetComponent<riqi_sub>().m_day = m_huodong_subs[i].arg2;
			_obj.GetComponent<riqi_sub>().is_land = m_huodong_subs[i].arg3;
			_obj.GetComponent<riqi_sub>().m_can_get = m_huodong_subs[i].arg4;
			_obj.GetComponent<riqi_sub>().m_is_chose = m_huodong_subs[i].arg5;
			_obj.GetComponent<riqi_sub>().m_scro = m_view;
			_obj.GetComponent<riqi_sub>().is_end = flag;
			_obj.GetComponent<riqi_sub>().rewards.Clear();
			for(int k =0; k < m_huodong_subs[i].types.Count; ++k )
			{
				s_t_reward t_reward = new s_t_reward();
				t_reward.type = m_huodong_subs[i].types[k];
				t_reward.value1 = m_huodong_subs[i].value1s[k];
				t_reward.value2 = m_huodong_subs[i].value2s[k];
				t_reward.value3 = m_huodong_subs[i].value3s[k];
				_obj.GetComponent<riqi_sub>().rewards.Add(t_reward);
			}
			_obj.GetComponent<riqi_sub>().reset();
			
			sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), num * 0.05f);
			sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, num * 0.05f);
			num++;
		}
	}

	public void effect()
	{
		bool flag = false;
		int jieri_flag = 0;
		for(int i = 0; i < _msg.huodongs.Count;++i)
		{
			if(_msg.huodongs[i].type == 5)
			{
				for(int j = 0; j < _msg.huodongs[i].reward.subs.Count;++j)
				{
					if(_msg.huodongs[i].reward.subs[j].arg3 < _msg.huodongs[i].reward.subs[j].arg4 && _msg.huodongs[i].reward.subs[j].arg2 > _msg.huodongs[i].reward.subs[j].arg3)
					{
						flag = true;
						break;
					}
				}
				long _time = (long)(_msg.huodongs[i].time - timer.now());
				if(_time <= 0)
				{
					flag = false;
				}
				break;
			}
		}
		for(int i = 0; i < _msg.huodongs.Count;++i)
		{
			if(_msg.huodongs[i].type == 9)
			{
				for(int j = 0; j < _msg.huodongs[i].reward.subs.Count;++j)
				{
					if(_msg.huodongs[i].reward.subs[j].arg4 == 0 && _msg.huodongs[i].reward.subs[j].arg3 == 1)
					{
						flag = true;
						break;
					}
				}
				long _time = (long)(_msg.huodongs[i].time - timer.now());
				if(_time <= 0)
				{
					flag = false;
				}
				break;
			}
		}
		if(flag)
		{
			jieri_flag = 1;
			m_effect1.SetActive(true);
		}
		else
		{
			m_effect1.SetActive(false);
		}
		flag = false;
		for(int i = 0; i < _msg.huodongs.Count;++i)
		{
			if(_msg.huodongs[i].type == 4)
			{
				for(int j = 0;j <_msg.huodongs[i].reward.subs.Count ; ++j )
				{
					if(_msg.huodongs[i].reward.subs[j].arg2 >= _msg.huodongs[i].reward.subs[j].arg4 && _msg.huodongs[i].reward.subs[j].arg3 == 0)
					{
						flag = true;
						break;
					}
				}
				long _time = (long)(_msg.huodongs[i].time - timer.now());
				if(_time <= 0)
				{
					flag = false;
				}
				break;
			}
		}
		if(flag)
		{
			jieri_flag = 1;
			m_effect2.SetActive(true);
		}
		else
		{
			m_effect2.SetActive(false);
		}
		/*flag = false;
		for(int i = 0; i < _msg.huodongs.Count;++i)
		{
			if(_msg.huodongs[i].type == 7)
			{
				for(int j = 0;j <_msg.huodongs[i].reward.subs.Count ; ++j )
				{
					if(_msg.huodongs[i].reward.subs[j].arg2 < _msg.huodongs[i].reward.subs[j].arg3)
					{
						flag = true;
						break;
					}
				}
				break;
			}
		}
		if(flag)
		{
			m_effect3.SetActive(true);
		}
		else
		{
			m_effect3.SetActive(false);
		}*/
		flag = false;
		for(int i = 0; i < _msg.huodongs.Count;++i)
		{
			if(_msg.huodongs[i].type == 8)
			{
				for(int j = 0;j <_msg.huodongs[i].reward.subs.Count ; ++j )
				{
					bool u_flag = false;
					for (int k = 0; k < _msg.huodongs[i].reward.subs[j].arg6.Count; ++k)
					{
						int num = sys._instance.m_self.get_item_num((uint)_msg.huodongs[i].reward.subs[j].arg7[k])/_msg.huodongs[i].reward.subs[j].arg8[k];
						if(num <= 0)
						{
							u_flag = true;
							break;
						}
					}
					if(_msg.huodongs[i].reward.subs[j].arg2 - _msg.huodongs[i].reward.subs[j].arg1 > 0 &&!u_flag)
					{
						flag = true;
						break;
					}
					long _time = (long)(_msg.huodongs[i].time - timer.now());
					if(_time <= 0)
					{
						flag = false;
					}
				}
				break;
			}
		}
		if(flag)
		{
			jieri_flag = 1;
			m_effect4.SetActive(true);
		}
		else
		{
			m_effect4.SetActive(false);
		}
		sys._instance.m_self.m_can_jieri_huodong = jieri_flag;
	}
	
	public int comp(protocol.game.huodong_reward_sub sub1,protocol.game.huodong_reward_sub sub2)
	{
		if(sub1.arg4 == 0 && sub2.arg4 != 0)
		{
			return -1;
		}
		else if(sub1.arg4 != 0 &&  sub2.arg4 == 0)
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
		else if(sub1.arg2 < sub2.arg2)
		{
			return -1;
		}
		else if(sub1.arg2 > sub2.arg2)
		{
			return 1;
		}
		return 0;
	}
	
	public void click(GameObject obj)
	{
		if (obj.transform.name == "close") 
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
			s_message _message = new s_message();
			_message.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_message);
			
		}
		if (obj.transform.name == "t1") 
		{
			m_select = 1;
			reset(m_select);
		}
		if (obj.transform.name == "t2") 
		{
			m_select = 2;
			reset(m_select);
		}
		if (obj.transform.name == "t3") 
		{
			m_select = 3;
			reset(m_select);
		}
		if (obj.transform.name == "t4") 
		{
			m_select = 4;
			reset(m_select);
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
			if(type == 0)
			{
				m_buy_num_gui.transform.Find("frame_big").GetComponent<frame>().hide();
				for(int a = 0; a < _msg.huodongs.Count; a++)
				{
					if(_msg.huodongs[a].type == 8)
					{
						m_huodong_id = _msg.huodongs[a].id;
						break;
					}
				}
				protocol.game.cmsg_huodong_reward msg = new protocol.game.cmsg_huodong_reward ();
				msg.id = m_id;
				msg.huodong = m_huodong_id;
				msg.index = m_input_num;
				net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_DJDH, msg);
			}
			else if(type == 1)
			{
				if(sys._instance.m_self.m_t_player.jewel < sub.arg1*m_input_num)
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

	void IMessage.message (s_message message)
	{
		if (message.m_type == "daily_refresh") 
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG_JIERI_VIEW, _msg);
		}
		if(message.m_type == "huodong_riqi_lj")
		{
			m_index = (int)message.m_ints[0];
			m_id = (int)message.m_ints[1];
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 9)
				{
					m_huodong_id = _msg.huodongs[a].id;
					break;
				}
			}
			protocol.game.cmsg_huodong_reward msg = new protocol.game.cmsg_huodong_reward ();
			msg.id = m_id;
			msg.index = m_index;
			msg.huodong = m_huodong_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_RQDL, msg);
		}
		if(message.m_type == "huodong_huoyue_lj")
		{
			m_id = (int)message.m_ints[0];
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 4)
				{
					m_huodong_id = _msg.huodongs[a].id;
					break;
				}
			}
			protocol.game.cmsg_huodong_reward msg = new protocol.game.cmsg_huodong_reward ();
			msg.id = m_id;
			msg.huodong = m_huodong_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_HYHD, msg);
		}
		if(message.m_type == "hide_huoyue_huodong")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(message.m_type == "select_zkfs")
		{
			type = 1;
			m_id = (int)message.m_ints[1];
			m_index = (int)message.m_ints[0];
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 7)
				{
					m_huodong_id = _msg.huodongs[a].id;
					break;
				}
			}
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 7)
				{
					for(int i = 0; i < _msg.huodongs[a].reward.subs.Count; i++)
					{
						if(_msg.huodongs[a].reward.subs[i].id == m_id)
						{
							sub = _msg.huodongs[a].reward.subs[i];
						}
					}
					break;
				}
			}
			if(sub.arg4 == 1)
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
		if(message.m_type == "huodong_dbcz_lj")
		{
			m_id = (int)message.m_ints[0];
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 5)
				{
					m_huodong_id = _msg.huodongs[a].id;
					break;
				}
			}
			protocol.game.cmsg_huodong_reward msg = new protocol.game.cmsg_huodong_reward ();
			msg.id = m_id;
			msg.huodong = m_huodong_id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_reward> (opclient_t.CMSG_HUODONG_DBCZ, msg);
		}
		if (message.m_type == "huodong_dbcz_recharge")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			
			s_message _mes = new s_message();
			_mes.m_type = "show_recharge";
			cmessage_center._instance.add_message(_mes);
			
			s_message _mes1 = new s_message();
			_mes1.m_type = "show_main_gui";
			cmessage_center._instance.add_message(_mes1);
		}
		if(message.m_type == "select_jieri_duihuan")
		{
			type = 0;
			m_id = (int)message.m_ints[0];
			total_num = 1000;
			m_input_num = 1;
			int num = 0; 
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 8)
				{
					for(int i = 0; i < _msg.huodongs[a].reward.subs.Count; i++)
					{
						if(_msg.huodongs[a].reward.subs[i].id == m_id)
						{
							sub = _msg.huodongs[a].reward.subs[i];
						}
					}
					break;
				}
			}
			for (int i = 0; i < sub.arg6.Count; ++i)
			{
				num = sys._instance.m_self.get_item_num((uint)sub.arg7[i])/sub.arg8[i];
				if(num < total_num)
				{
					total_num = num;
				}
			}
			if(total_num > sub.arg2 - sub.arg1)
			{
				total_num = sub.arg2 - sub.arg1;
			}
			buy_num();
			m_buy_num_gui.SetActive(true);
		}
	}

	public void buy_num()
	{
		if(type == 0)
		{
			m_money_type.SetActive(false);
			m_zongjia.SetActive(false);
			m_input.GetComponent<UILabel>().text = m_input_num.ToString ();
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (sub.types[0],sub.value1s[0],sub.value2s[0],sub.value3s[0]);
			m_desc.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jieri_huodong_gui.cs_1005_42") +" "+ sys._instance.m_self.get_item_num (sub.types[0],sub.value1s[0],sub.value2s[0],sub.value3s[0]);//[0af6ff]当前拥有:[-]
			m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"),total_num.ToString ());//{0}次
			sys._instance.remove_child (m_icon);
			GameObject _obj1 = icon_manager._instance.create_reward_icon(sub.types[0],sub.value1s[0],sub.value2s[0],sub.value3s[0]);
			
			_obj1.transform.parent = m_icon.transform;
			_obj1.transform.localScale = new Vector3(1,1,1);
			_obj1.transform.localPosition = new Vector3 (0, 0, 0);
		}
		else if(type == 1)
		{
			m_money_type.SetActive(true);
			m_zongjia.SetActive(true);
			m_input.GetComponent<UILabel>().text = m_input_num.ToString ();
			m_name.GetComponent<UILabel>().text = sys._instance.m_self.get_name (1,2,0,0);
			m_desc.GetComponent<UILabel>().text = game_data._instance.get_t_language ("jieri_huodong_gui.cs_1005_42") +" "+ sys._instance.m_self.get_item_num (1,2,1,0);//[0af6ff]当前拥有:[-]
            m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("equip_buy_gui.cs_430_64"), total_num.ToString());//{0}次
			string color = sys._instance.get_res_color (2);
			if(sys._instance.m_self.m_t_player.jewel < sub.arg1*m_input_num)
			{
				color = "[ff0000]";
			}
			m_zongjia.GetComponent<UILabel>().text = color + "X" +(sub.arg1 * m_input_num).ToString ();
			sys._instance.remove_child (m_icon);
			GameObject _obj1 = icon_manager._instance.create_reward_icon(1,2,sub.arg1 * m_input_num,0);
			
			_obj1.transform.parent = m_icon.transform;
			_obj1.transform.localScale = new Vector3(1,1,1);
			_obj1.transform.localPosition = new Vector3 (0, 0, 0);
		}
	}

	void IMessage.net_message (s_net_message message)
	{
		if(message.m_opcode == opclient_t.CMSG_HUODONG_JIERI_VIEW)
		{
			_msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_jiri_view> (message.m_byte);
			m_title.GetComponent<UILabel>().text = sys._instance.m_self.m_jieri_huodong;
			m_title1.GetComponent<UILabel>().text = sys._instance.m_self.m_jieri_huodong;
			if(_msg.huodongs.Count == 0)
			{
				return;
			}
			m_end_time = _msg.huodongs[0].time;
			for(int i = 0; i < _msg.huodongs.Count && i < m_names.Count;++i)
			{
				if(_msg.huodongs[i].type == 5 || _msg.huodongs[i].type == 9)
				{
					m_names[0].GetComponent<UILabel>().text = _msg.huodongs[i].reward.name;
					m_name1s[0].GetComponent<UILabel>().text = _msg.huodongs[i].reward.name;
				}
				if(_msg.huodongs[i].type == 4 )
				{
					m_names[1].GetComponent<UILabel>().text = _msg.huodongs[i].reward.name;
					m_name1s[1].GetComponent<UILabel>().text = _msg.huodongs[i].reward.name;
				}
				if(_msg.huodongs[i].type == 7)
				{
					m_names[2].GetComponent<UILabel>().text = _msg.huodongs[i].reward.name;
					m_name1s[2].GetComponent<UILabel>().text = _msg.huodongs[i].reward.name;
				}
				if(_msg.huodongs[i].type == 8)
				{
					m_names[3].GetComponent<UILabel>().text = _msg.huodongs[i].reward.name;
					m_name1s[3].GetComponent<UILabel>().text = _msg.huodongs[i].reward.name;
				}
			}
			reset (m_select);
		}
		if (message.m_opcode == opclient_t.CMSG_HUODONG_RQDL)
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
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 9)
				{
					for(int i = 0; i < _msg.huodongs[a].reward.subs.Count; i++)
					{
						if(_msg.huodongs[a].reward.subs[i].id == m_id)
						{
							sub = _msg.huodongs[a].reward.subs[i];
						}
					}
					break;
				}
			}
			sub.arg4 = 1;
			if(sub.arg5 == 1)
			{
				sys._instance.m_self.add_reward(sub.types[m_index],sub.value1s[m_index]
                                                , sub.value2s[m_index], sub.value3s[m_index], sys._instance.m_self.m_jieri_huodong + game_data._instance.get_t_language ("jieri_huodong_gui.cs_1107_133"));//节日活动获得
			}
			else
			{
				for(int i = 0;i <sub.types.Count;i ++)
				{
					sys._instance.m_self.add_reward(sub.types[i],sub.value1s[i]
                                                    , sub.value2s[i], sub.value3s[i], sys._instance.m_self.m_jieri_huodong + game_data._instance.get_t_language ("jieri_huodong_gui.cs_1107_133"));//节日活动获得
				}
			}
			reset(m_select);
		}
		if (message.m_opcode == opclient_t.CMSG_HUODONG_DBCZ && this.transform.gameObject.activeSelf)
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
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 5)
				{
					for(int i = 0; i < _msg.huodongs[a].reward.subs.Count; i++)
					{
						if(_msg.huodongs[a].reward.subs[i].id == m_id)
						{
							sub = _msg.huodongs[a].reward.subs[i];
						}
					}
					break;
				}
			}
			sub.arg3 += 1;
			for(int i = 0;i <sub.types.Count;i ++)
			{
				sys._instance.m_self.add_reward(sub.types[i],sub.value1s[i]
				                                ,sub.value2s[i],sub.value3s[i],sys._instance.m_self.m_jieri_huodong + game_data._instance.get_t_language ("jieri_huodong_gui.cs_1107_133"));//节日活动获得
			}
			reset(m_select);
		}
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
			sub.arg2 += m_input_num;
			if(sub.arg4 == 1)
			{
				sys._instance.m_self.add_reward(sub.types[m_index],sub.value1s[m_index]
                                                , sub.value2s[m_index] * m_input_num, sub.value3s[m_index], sys._instance.m_self.m_jieri_huodong + game_data._instance.get_t_language ("jieri_huodong_gui.cs_1175_147"));//节日活动兑换
			}
			else
			{
				for(int i = 0;i <sub.types.Count;i ++)
				{
					sys._instance.m_self.add_reward(sub.types[i],sub.value1s[i]
                                                    , sub.value2s[i] * m_input_num, sub.value3s[i], sys._instance.m_self.m_jieri_huodong + game_data._instance.get_t_language ("jieri_huodong_gui.cs_1175_147"));//节日活动兑换
				}
			}
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, sub.arg1 * m_input_num, game_data._instance.get_t_language ("jieri_huodong_gui.cs_1185_93"));//节日活动兑换消耗
			reset(m_select);
		}
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
			sub.arg1 += m_input_num;

			for(int i = 0;i < sub.types.Count;++i)
			{
				sys._instance.m_self.add_reward(sub.types[i],sub.value1s[i]
				                                ,sub.value2s[i]*m_input_num, sub.value3s[i],sys._instance.m_self.m_jieri_huodong + game_data._instance.get_t_language ("jieri_huodong_gui.cs_1107_133"));//节日活动获得
			}
			for(int i = 0;i <sub.arg6.Count;i ++)
			{
                sys._instance.m_self.remove_item((uint)sub.arg7[i], sub.arg8[i] * m_input_num, sys._instance.m_self.m_jieri_huodong + game_data._instance.get_t_language ("jieri_huodong_gui.cs_1212_134"));//节日活动消耗
			}
			reset(m_select);
		}
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
			for(int a = 0; a < _msg.huodongs.Count; a++)
			{
				if(_msg.huodongs[a].type == 4)
				{
					for(int i = 0; i < _msg.huodongs[a].reward.subs.Count; i++)
					{
						if(_msg.huodongs[a].reward.subs[i].id == m_id)
						{
							sub = _msg.huodongs[a].reward.subs[i];
						}
					}
					break;
				}
			}
			sub.arg3 = 1;
			for(int i = 0;i <sub.types.Count;i ++)
			{
				sys._instance.m_self.add_reward(sub.types[i],sub.value1s[i]
                                                , sub.value2s[i], sub.value3s[i], sys._instance.m_self.m_jieri_huodong + game_data._instance.get_t_language ("jieri_huodong_gui.cs_1107_133"));//节日活动获得
			}
			reset(m_select);
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
