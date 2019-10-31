
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class pttq_gui : MonoBehaviour, IMessage{
	
	public GameObject m_vip_list;
	public GameObject m_reward_list;

	public UILabel m_time_Label;
	public UILabel m_time;
	public UILabel m_content_Label;
	public UILabel m_content;
	public UILabel m_tiltle1;
	public UILabel m_tiltle2;
	public UILabel m_vip_Label;

	int m_select = 1;
	dhc.global_t m_global;
	s_t_huodong_pttq m_huodong;
	s_t_huodong_pttq_sub m_huodong_sub;

	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG4_VIEW)
		{
			protocol.game.smsg_huodong_pttq_view _msg = net_http._instance.parse_packet<protocol.game.smsg_huodong_pttq_view> (message.m_byte);
			m_global = _msg.data;
			update_ui ();
		} 
	    if (message.m_opcode == opclient_t.CMSG_HUODONG4_PTTQ) 
		{
			protocol.game.smsg_huodong_reward  _treasures = net_http._instance.parse_packet<protocol.game.smsg_huodong_reward>(message.m_byte);
			for(int i = 0;i < _treasures.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_treasures.treasures[i]);
			}
            for (int i = 0; i < _treasures.pets.Count; i++)
            {
                sys._instance.m_self.add_pet(_treasures.pets[i]);
            }
			if (m_huodong == null || m_huodong_sub == null)
			{
				return;
			}
			sys._instance.m_self.m_t_player.huodong_pttq_reward.Add(m_huodong.id * 100 + m_huodong_sub.vip);
			sys._instance.m_self.add_reward(m_huodong_sub.reward.type, m_huodong_sub.reward.value1,
			                                    m_huodong_sub.reward.value2, m_huodong_sub.reward.value3,game_data._instance.get_t_language ("pttq_gui.cs_56_96"));//普天同庆获得

			update_ui();
		}
	}

	void IMessage.message(s_message messag)
	{

	}
    public void init(bool resert)
    {
        if (resert || this.gameObject.activeSelf)
        {
            protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
            net_http._instance.send_msg<protocol.game.cmsg_common>(opclient_t.CMSG_HUODONG4_VIEW, _msg);
        }
    }
	public void OnEnable()
	{
		m_select = 1;
		protocol.game.cmsg_common _msg = new protocol.game.cmsg_common();
		net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_HUODONG4_VIEW, _msg);
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			//this.transform.GetComponent<ui_title_anim>().hide_ui();
			//this.gameObject.SetActive(false);
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			s_message _message = new s_message();
			_message.m_type = "show_jc_huodong";
			cmessage_center._instance.add_message(_message);
		}
	}

	public void select(GameObject obj)
	{
		select_vip(int.Parse(obj.transform.name));

		for (int i = 0; i < m_vip_list.transform.childCount; ++i)
		{
			GameObject child = m_vip_list.transform.GetChild(i).gameObject;
			if (child.transform.name == obj.transform.name)
			{
				child.GetComponent<UISprite>().spriteName = "pttq_zcan01b";
			}
			else
			{
				child.GetComponent<UISprite>().spriteName = "pttq_zcan01a";
			}
		}
	}

	public void select_vip(int id)
	{
		m_select = id;

		if(m_reward_list.GetComponent<SpringPanel>() != null)
		{
			m_reward_list.GetComponent<SpringPanel>().enabled = false;
		}
		m_reward_list.transform.localPosition = new Vector3(0, 0, 0);
		m_reward_list.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_reward_list);

		s_t_huodong_pttq _huodong = game_data._instance.get_t_huodong_pttq(m_select);
		if (_huodong == null) 
		{
			return;	
		}
		m_huodong = _huodong;

		if (m_global == null) 
		{
			return;
		}
		bool has_complete = m_global.pttq_vip_id.Exists (delegate(int val) {
						return val == m_huodong.id;
				});
		int a = 0;
		for (int i = 0; i < _huodong.sub.Count; ++i) 
		{
			int index_id = _huodong.id * 100 + _huodong.sub[i].vip;
			if(is_lq(index_id))
			{
				continue;
			}
			create_pttq_reward(_huodong, _huodong.sub[i], a, has_complete);
			a ++;
		}
		for (int i = 0; i < _huodong.sub.Count; ++i) 
		{
			int index_id = _huodong.id * 100 + _huodong.sub[i].vip;
			if(!is_lq(index_id))
			{
				continue;
			}
			create_pttq_reward(_huodong, _huodong.sub[i], a, has_complete);
			a ++;
		}

	}

	public bool is_lq(int id)
	{
		for(int i = 0; i < sys._instance.m_self.m_t_player.huodong_pttq_reward.Count;++i)
		{
			if(sys._instance.m_self.m_t_player.huodong_pttq_reward[i] == id)
			{
				return true;
			}
		}
		return false;
	}

	public void select_reward(GameObject obj)
	{
		int _id = int.Parse(obj.transform.parent.name);
		s_t_huodong_pttq_sub sub = m_huodong.sub.Find(
			delegate(s_t_huodong_pttq_sub p) { return p.vip == _id; });
		if (sub != null) 
		{
			m_huodong_sub = sub;
			if (m_huodong == null)
			{
				return;
			}
			protocol.game.cmsg_huodong_pttq _msg = new protocol.game.cmsg_huodong_pttq();
			_msg.id = m_huodong.id;
			_msg.vip = _id;
			net_http._instance.send_msg<protocol.game.cmsg_huodong_pttq> (opclient_t.CMSG_HUODONG4_PTTQ,_msg);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void update_ui()
	{
		if(m_vip_list.GetComponent<SpringPanel>() != null)
		{
			m_vip_list.GetComponent<SpringPanel>().enabled = false;
		}
		m_vip_list.transform.localPosition = new Vector3(64, -133, 0);
		m_vip_list.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_vip_list);

		sys._instance.m_self.m_can_pttq = 0;

		for (int i = 0; i < game_data._instance.m_dbc_huodong_pttq.get_y(); i ++) 
		{
			int _id = int.Parse (game_data._instance.m_dbc_huodong_pttq.get (0, i));
			s_t_huodong_pttq _huodong = game_data._instance.get_t_huodong_pttq (_id);
			if (_huodong == null) 
			{
				continue;
			}
			create_pttq_vip(_huodong, i);
		}
		select_vip(m_select);
	}
	
	private void create_pttq_vip(s_t_huodong_pttq huodong, int index)
	{
		if (m_global == null) 
		{
			return;
		}
		string name = "";
		bool has_reward = false;
		for (int j = 0; j < m_global.pttq_vip_id.Count; ++j)
		{
			if (m_global.pttq_vip_id[j] == huodong.id)
			{
				name = m_global.pttq_player_name[j];
				break;
			}
		}

		if (name != "") 
		{
			for (int k = 0; k < huodong.sub.Count; ++k)
			{
				if (huodong.sub[k].vip <= sys._instance.m_self.m_t_player.vip)
				{
					int index_id = huodong.id * 100 + huodong.sub[k].vip;
					if (!sys._instance.m_self.m_t_player.huodong_pttq_reward.Exists(
						delegate(int p) {return p == index_id; }))
					{
						has_reward = true;
						break;
					}
				}
			}
			name = game_data._instance.get_t_language ("pttq_gui.cs_248_10") + name;//达标者:
		}
		else
		{
			name = game_data._instance.get_t_language ("pttq_gui.cs_252_10");//没有达标者
		}
		
		GameObject _item = game_data._instance.ins_object_res ("ui/pttq_vip_gui");
		if (_item == null) 
		{
			return;
		}
		_item.transform.name = huodong.id.ToString ();
		_item.transform.parent = m_vip_list.transform;
		_item.transform.localPosition = new Vector3(225 * index  -194,  0, 0);
		_item.transform.localScale = new Vector3(1,1,1);
		_item.transform.Find("name").GetComponent<UILabel>().text = name;
		_item.transform.Find("des").GetComponent<UILabel>().text = game_data._instance.get_t_language ("pttq_gui.cs_265_68");//达标奖励
		_item.transform.Find("vip").GetComponent<UILabel>().text = "v" + huodong.vip.ToString ();
		_item.transform.Find("point").gameObject.SetActive(has_reward);
		if (huodong.id == m_select) 
		{
			_item.GetComponent<UISprite>().spriteName = "pttq_zcan01b";
		}
		_item.GetComponent<UIButtonMessage>().target = this.gameObject;
		_item.SetActive (true);

		if (has_reward) 
		{
			sys._instance.m_self.m_can_pttq = 1;
		}
	}

	private void create_pttq_reward(s_t_huodong_pttq huodong, s_t_huodong_pttq_sub sub, int index, bool complete)
	{
		GameObject _item = game_data._instance.ins_object_res("ui/pttq_reward_gui");
		if (_item == null)
		{
			return;
		}
		_item.transform.name = sub.vip.ToString();
		_item.transform.parent = m_reward_list.transform;
		_item.transform.localPosition = new Vector3(0,  -111 * index + 64,0);
		_item.transform.localScale = new Vector3(1,1,1);
		_item.transform.Find("des").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("pttq_gui.cs_292_80"), huodong.vip.ToString());//VIP{0}返利礼盒
		_item.transform.Find("con").GetComponent<UILabel>().text = game_data._instance.get_t_language ("pttq_gui.cs_293_66");//领取条件
		_item.transform.Find("vip").GetComponent<UILabel>().text = "v" + sub.vip.ToString();

		GameObject _icon = icon_manager._instance.create_reward_icon (sub.reward.type, sub.reward.value1, sub.reward.value2, sub.reward.value3);
		_icon.transform.parent = _item.transform.Find("icon_kuang").gameObject.transform;
		_icon.transform.localPosition = new Vector3(0,0,0);
		_icon.transform.localScale = new Vector3(1,1,1);
        _item.transform.Find("reward").GetComponent<UIButtonMessage>().target = this.gameObject;
		_item.transform.Find("reward1").GetComponent<UISprite>().enabled = false;
		_item.transform.Find("reward/ling").GetComponent<UILabel>().text = game_data._instance.get_t_language ("jt_mobai.cs_97_91");//领取

		if (complete && (sub.vip <= sys._instance.m_self.m_t_player.vip)) 
		{
			GameObject reward = _item.transform.Find("reward").gameObject;
			int index_id = huodong.id * 100 + sub.vip;
			if (sys._instance.m_self.m_t_player.huodong_pttq_reward.Exists(
				delegate(int p) {return p == index_id; }))
			{
				reward.GetComponent<BoxCollider>().enabled = false;
				reward.GetComponent<UISprite>().enabled = false;
				_item.transform.Find("reward1").GetComponent<UISprite>().enabled = true;
				reward.transform.Find("ling").GetComponent<UILabel>().enabled = false;
			}
			else
			{

				reward.transform.Find("ling").GetComponent<UILabel>().text = game_data._instance.get_t_language ("jt_mobai.cs_97_91");//领取
			}
		} 
		else 
		{
			_item.transform.Find("reward").GetComponent<BoxCollider>().enabled = false;
			_item.transform.Find("reward").GetComponent<UISprite>().set_enable(false);	
		}
		sys._instance.add_pos_anim(_item,0.3f, new Vector3(-300, 0, 0), index * 0.05f);
		sys._instance.add_alpha_anim(_item,0.3f, 0, 1.0f, index * 0.05f);
		_item.SetActive (true);
	}
}
