
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class explore_task_gui : MonoBehaviour,IMessage{

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
		List<s_t_manyou_mubiao> t_explore_mubiaos = new List<s_t_manyou_mubiao>();
		foreach (int id in game_data._instance.m_dbc_manyou_mubiao.m_index.Keys)
		{
			s_t_manyou_mubiao t_manyou_mubiao = game_data._instance.get_t_manyou_mubiao(id);
			t_explore_mubiaos.Add(t_manyou_mubiao);
		}
		t_explore_mubiaos.Sort (comp);
		for(int i = 0; i < t_explore_mubiaos.Count;++i)
		{
			if(sys._instance.is_hide_reward(t_explore_mubiaos[i].reward.type,t_explore_mubiaos[i].reward.value1))
			{
				continue;
			}
			
			GameObject active = game_data._instance.ins_object_res("ui/explore_task");
			active.transform.parent = m_view.transform;
			active.transform.localPosition = new Vector3(0, 105 - i * 110,0);
			active.transform.localScale = new Vector3(1,1,1);
			active.transform.GetComponent<explore_task_item>().explore_mubiao_id = t_explore_mubiaos[i].id;
			active.transform.GetComponent<UIButtonMessage>().target = this.gameObject;
			active.transform.GetComponent<explore_task_item>().reset();
			
			
			sys._instance.add_pos_anim(active,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(active,0.3f, 0, 1.0f, i * 0.05f);
		}
		m_num.GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("explore_task_gui.cs_62_53"), explore_gui._instance.score);//当前积分:{0}分
	}
	
	int comp(s_t_manyou_mubiao t_manyou_mubiao1,s_t_manyou_mubiao t_manyou_mubiao2)
	{
		if(!active_done(t_manyou_mubiao1) && active_done(t_manyou_mubiao2))
		{
			return -1;
		}
		else if(active_done(t_manyou_mubiao1) && !active_done(t_manyou_mubiao2))
		{
			return 1;
		}
		else if(active_vis(t_manyou_mubiao1) && !active_vis(t_manyou_mubiao2))
		{
			return -1;
		}
		else if(!active_vis(t_manyou_mubiao1) && active_vis(t_manyou_mubiao2))
		{
			return 1;
		}
		return t_manyou_mubiao1.id - t_manyou_mubiao2.id;
	}
	
	static public bool active_vis(s_t_manyou_mubiao t_manyou_mubiao)
	{
		if(t_manyou_mubiao.score > explore_gui._instance.score)
		{
			return false;
		}
		return true;
	}
	
	static public bool active_done(s_t_manyou_mubiao t_manyou_mubiao)
	{
		for (int i = 0; i < explore_gui._instance.rewards.Count; ++i)
		{
			if (explore_gui._instance.rewards[i] == t_manyou_mubiao.id)
			{
				return true;
			}
		}
		return false;
	}
	
	public static bool effect()
	{
		foreach (int id in game_data._instance.m_dbc_manyou_mubiao.m_index.Keys)
		{
			s_t_manyou_mubiao t_manyou_mubiao = game_data._instance.get_t_manyou_mubiao(id);
			if (!active_done(t_manyou_mubiao) && active_vis(t_manyou_mubiao))
			{
				return true;
			}
		}
		return false;
	}
	
	public void click(GameObject obj)
	{
		if (obj.name == "explore_task(Clone)") 
		{
			click_id = obj.transform.GetComponent<explore_task_item>().explore_mubiao_id;
			s_t_manyou_mubiao t_manyou_mubiao = game_data._instance.get_t_manyou_mubiao (click_id);
			if (active_vis (t_manyou_mubiao)) 
			{
				protocol.game.cmsg_huodong_tansuo_mubiao _msg = new protocol.game.cmsg_huodong_tansuo_mubiao ();
				_msg.id = click_id;
				net_http._instance.send_msg<protocol.game.cmsg_huodong_tansuo_mubiao> (opclient_t.CMSG_HUODONG_MANYOU_MUBIAO, _msg);
			}
		}
		if(obj.name == "close")
		{
			explore_gui._instance.is_press = true;
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}
	
	void IMessage.message (s_message message)
	{
		
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_HUODONG_MANYOU_MUBIAO)
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
				sys._instance.m_self.add_reward( _msg.types[i], _msg. value1s[i],_msg.value2s[i],_msg.value3s[i],game_data._instance.get_t_language ("explore_task_gui.cs_160_101"));//太空漫游任务获得
			}
			explore_gui._instance.rewards.Add(click_id);
			s_message _mes = new s_message();
			_mes.m_type = "show_manyou_task_effect";
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
