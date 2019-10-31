
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tanbao_reward_gui : MonoBehaviour ,IMessage{

	public GameObject m_view;
	public GameObject m_num;
	int click_id = 0;
	
	// Use this for initialization
	void Start () {
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	
	public void OnEnable()
	{
		reset ();
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
		List<s_t_tanbao_mubiao> t_tanbao_mubiaos = new List<s_t_tanbao_mubiao>();
		foreach (int id in game_data._instance.m_dbc_tanbao_mubiao.m_index.Keys)
		{
			s_t_tanbao_mubiao t_tanbao_mubiao = game_data._instance.get_t_tanbao_mubiao(id);
			t_tanbao_mubiaos.Add(t_tanbao_mubiao);
		}
		t_tanbao_mubiaos.Sort (comp);
		for(int i = 0; i < t_tanbao_mubiaos.Count;++i)
		{
			if(sys._instance.is_hide_reward(t_tanbao_mubiaos[i].type,t_tanbao_mubiaos[i].value1))
			{
				continue;
			}
			
			GameObject active = game_data._instance.ins_object_res("ui/tanbao_task");
			active.transform.parent = m_view.transform;
			active.transform.localPosition = new Vector3(0, 105 - i * 110,0);
			active.transform.localScale = new Vector3(1,1,1);
			active.transform.GetComponent<tanbao_mubiao>().tanbao_mubiao_id = t_tanbao_mubiaos[i].id;
			active.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			active.transform.GetComponent<tanbao_mubiao>().reset();
			

			sys._instance.add_pos_anim(active,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(active,0.3f, 0, 1.0f, i * 0.05f);
		}
		m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("tanbao_reward_gui.cs_62_53"), tanbao_gui._instance.qidian_num );//已经过起点:{0}次
	}

	int comp(s_t_tanbao_mubiao t_tanbao_mubiao1,s_t_tanbao_mubiao t_tanbao_mubiao2)
	{
		if(!active_done(t_tanbao_mubiao1) && active_done(t_tanbao_mubiao2))
		{
			return -1;
		}
		else if(active_done(t_tanbao_mubiao1) && !active_done(t_tanbao_mubiao2))
		{
			return 1;
		}
		else if(active_vis(t_tanbao_mubiao1) && !active_vis(t_tanbao_mubiao2))
		{
			return -1;
		}
		else if(!active_vis(t_tanbao_mubiao1) && active_vis(t_tanbao_mubiao2))
		{
			return 1;
		}
		return t_tanbao_mubiao1.id - t_tanbao_mubiao2.id;
	}

	static public bool active_vis(s_t_tanbao_mubiao t_tanbao_mubiao)
	{
		if(t_tanbao_mubiao.task_num > tanbao_gui._instance.qidian_num)
		{
			return false;
		}
		return true;
	}
	
	static public bool active_done(s_t_tanbao_mubiao t_tanbao_mubiao)
	{
		for (int i = 0; i < tanbao_gui._instance.rewards.Count; ++i)
		{
			if (tanbao_gui._instance.rewards[i] == t_tanbao_mubiao.id)
			{
				return true;
			}
		}
		return false;
	}

	public static bool effect()
	{
		foreach (int id in game_data._instance.m_dbc_tanbao_mubiao.m_index.Keys)
		{
			s_t_tanbao_mubiao t_tanbao_mubiao = game_data._instance.get_t_tanbao_mubiao(id);
			if (!active_done(t_tanbao_mubiao) && active_vis(t_tanbao_mubiao))
			{
				return true;
			}
		}
		return false;
	}

	public void click(GameObject obj)
	{
		if (obj.name == "tanbao_task(Clone)") 
		{
			click_id = obj.transform.GetComponent<tanbao_mubiao>().tanbao_mubiao_id;
			s_t_tanbao_mubiao t_tanbao_mubiao = game_data._instance.get_t_tanbao_mubiao (click_id);
			if (active_vis (t_tanbao_mubiao)) 
			{
				protocol.game.cmsg_tanbao_active _msg = new protocol.game.cmsg_tanbao_active ();
				_msg.id = click_id;
				net_http._instance.send_msg<protocol.game.cmsg_tanbao_active> (opclient_t.CMSG_HUODONG_TANBAO_MUBIAO, _msg);
			}
		}
		if(obj.name == "close")
		{
			tanbao_gui.press = false;
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}
	
	void IMessage.message (s_message message)
	{
		
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_TANBAO_MUBIAO)
		{
			protocol.game.smsg_shop_buy _msg = net_http._instance.parse_packet<protocol.game.smsg_shop_buy> (message.m_byte);
			for(int i =0 ;i< _msg.treasures.Count;i++)
			{
				sys._instance.m_self.add_treasure(_msg.treasures[i],true);
			}
			for(int i =0 ;i< _msg.equips.Count;i++)
			{
				sys._instance.m_self.add_equip(_msg.equips[i],true);
			}
			for(int i = 0; i < _msg.types.Count;++i)
			{
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("tanbao_reward_gui.cs_160_101"));//探宝任务奖励获得
			}
			tanbao_gui._instance.rewards.Add(click_id);
			s_message _mes = new s_message();
			_mes.m_type = "show_tanbao_task_effect";
			cmessage_center._instance.add_message(_mes);
			reset();
		}
		if(message.m_opcode == opclient_t.CMSG_BOSS_ACTIVE_LOOK)
		{
			protocol.game.smsg_boss_active_look _msg = net_http._instance.parse_packet<protocol.game.smsg_boss_active_look> (message.m_byte);
			sys._instance.m_self.m_t_player.boss_active_ids.Clear();
			sys._instance.m_self.m_t_player.boss_active_nums.Clear();
			sys._instance.m_self.m_t_player.boss_active_rewards.Clear();
			for(int i = 0; i < _msg.ids.Count;++i)
			{
				sys._instance.m_self.m_t_player.boss_active_ids.Add(_msg.ids[i]);
			}
			if(_msg.nums.Count != _msg.ids.Count )
			{
				for(int i = 0; i < _msg.ids.Count;++i)
				{
					sys._instance.m_self.m_t_player.boss_active_nums.Add(0);
				}
			}
			else
			{
				for(int i = 0; i < _msg.nums.Count;++i)
				{
					sys._instance.m_self.m_t_player.boss_active_nums.Add(_msg.nums[i]);
				}
			}
			if(_msg.rewards.Count != _msg.ids.Count  )
			{
				for(int i = 0; i < _msg.ids.Count;++i)
				{
					sys._instance.m_self.m_t_player.boss_active_rewards.Add(0);
				}
			}
			else
			{
				for(int i = 0; i < _msg.rewards.Count;++i)
				{
					sys._instance.m_self.m_t_player.boss_active_rewards.Add(_msg.rewards[i]);
				}
			}
			sys._instance.m_self.m_t_player.boss_player_level = _msg.level;
			reset ();
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
