
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class rotary_table_rank_gui : MonoBehaviour,IMessage {

	public GameObject m_scro;
	public GameObject m_view;
	public GameObject m_top;
	public GameObject m_top1;
	public int score = 0;
	private int m_select = 1;
	List<protocol.game.smsg_rank_view> m_ranks = new List<protocol.game.smsg_rank_view>();
	private List<ulong> m_guids = new List<ulong>();
	public GameObject m_icon;
	public GameObject m_ph;
	public GameObject m_name;
	public GameObject m_chenghao;
	public GameObject m_score;
	public GameObject m_norank;
	public UIToggle m_pt_reward_toggle;
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
		m_ranks.Clear ();
		for (int i = 0; i < 2; ++i)
		{
			m_ranks.Add(null);
		}
		m_select = 1;
		reset_yulan ();
	}
	
	public void click(GameObject obj)
	{
		if(obj.transform.name == "pt_reward")
		{
			m_select = 1;
			reset_yulan();
		}
		if(obj.transform.name == "jy_reward")
		{
			m_select = 2;
			reset_yulan();
		}
		if(obj.transform.name == "pt_ph")
		{
			m_select = 13;
			protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view ();
			_msg.type = m_select;
			net_http._instance.send_msg<protocol.game.cmsg_rank_view> (opclient_t.CMSG_RANK_VIEW, _msg);
		}
		if(obj.transform.name == "jy_ph")
		{
			m_select = 14;
			protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view ();
			_msg.type = m_select;
			net_http._instance.send_msg<protocol.game.cmsg_rank_view> (opclient_t.CMSG_RANK_VIEW, _msg);
		}
		if(obj.transform.name == "close")
		{
			m_pt_reward_toggle.value = true;
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		if(obj.transform.name == "look")
		{
			int id = int.Parse(obj.transform.parent.name);
			protocol.game.cmsg_player_look _msg = new protocol.game.cmsg_player_look ();
			_msg.target_guid = m_guids[id];
			net_http._instance.send_msg<protocol.game.cmsg_player_look> (opclient_t.CMSG_PLAYER_LOOK, _msg);
		}
	}
	
	public void reset_yulan()
	{
		m_top.SetActive(true);
		m_top1.SetActive(false);
		if(m_view.GetComponent<SpringPanel>() != null)
		{
			m_view.GetComponent<SpringPanel>().enabled = false;
		}
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_view);
		List<s_t_zhuanpan_reward> zhuanpan_rewards = new List<s_t_zhuanpan_reward>();
		for(int i = 0;i < game_data._instance.m_dbc_zhuanpan_reward.get_y();i ++)
		{
			int _id = int.Parse(game_data._instance.m_dbc_zhuanpan_reward.get(0,i));
			s_t_zhuanpan_reward t_zhuanpan_reward = game_data._instance.get_t_zhuanpan_reward(_id,m_select);
			if(zhuanpan_rewards.Contains(t_zhuanpan_reward))
			{
				continue;
			}
			zhuanpan_rewards.Add(t_zhuanpan_reward);
			GameObject temp = game_data._instance.ins_object_res ("ui/rotary_ph_item");
			if (t_zhuanpan_reward.rank1 == t_zhuanpan_reward.rank2)
			{
				temp.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"),//第{0}名
					t_zhuanpan_reward.rank1.ToString());
				
			}
			else
			{
				temp.transform.Find("Label").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("arena.cs_182_82")//第{0}-{1}名
					,t_zhuanpan_reward.rank1.ToString() , t_zhuanpan_reward.rank2 );
				
			}
			
			temp.transform.parent = m_view.transform;
			temp.transform.localScale = new Vector3(1,1,1);
			temp.transform.localPosition = new Vector3(0,- i * 119 + 149,0);
			
			sys._instance.add_pos_anim(temp,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(temp,0.3f, 0, 1.0f, i * 0.05f);
			for(int c = 0;c < t_zhuanpan_reward.rewards.Count;c ++)
			{
				GameObject _icon = icon_manager._instance.create_reward_icon(t_zhuanpan_reward.rewards[c].type,t_zhuanpan_reward.rewards[c].value1,t_zhuanpan_reward.rewards[c].value2,t_zhuanpan_reward.rewards[c].value3);
				_icon.transform.parent = temp.transform.Find(c.ToString());
				_icon.transform.localScale = new Vector3(1,1,1);
				_icon.transform.localPosition = new Vector3(0,0,0);
				temp.transform.Find(c.ToString()).gameObject.SetActive(true);
			}
			for(int j = t_zhuanpan_reward.rewards.Count; j < 3;++j)
			{
				temp.transform.Find(j.ToString()).gameObject.SetActive(false);
			}
		}
	}
	
	public void reset_ph()
	{
		m_top.SetActive(false);
		m_top1.SetActive(true);
		m_guids.Clear ();
		if(m_scro.GetComponent<SpringPanel>() != null)
		{
			m_scro.GetComponent<SpringPanel>().enabled = false;
		}
		m_scro.transform.localPosition = new Vector3(0, 0, 0);
		m_scro.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child(m_scro);
		for (int i = 0; i < m_ranks[m_select - 13].rank_list.player_guid.Count && i < 20; i++)
		{
			GameObject _obj = game_data._instance.ins_object_res("ui/rotary_rank_sub");
			_obj.transform.name = i.ToString();
			_obj.transform.parent = m_scro.transform;
			_obj.transform.localScale = new Vector3(1, 1, 1);
			_obj.transform.localPosition = new Vector3(0, 141 - i * 68, 1);
			
			sys._instance.add_pos_anim(_obj,0.3f, new Vector3(-300, 0, 0), i * 0.05f);
			sys._instance.add_alpha_anim(_obj,0.3f, 0, 1.0f, i * 0.05f);
			int _rank = i + 1;
			string _rank_show = string.Format(game_data._instance.get_t_language ("arena.cs_179_81"), _rank.ToString());//第{0}名
			GameObject _rank_obj = _obj.transform.Find("rank").gameObject;
			_rank_obj.GetComponent<UILabel>().text = _rank_show;
			_obj.transform.Find("look").GetComponent<UIButtonMessage>().target = this.gameObject;
			_obj.transform.Find("name").GetComponent<UILabel>().text = game_data._instance.get_name_color(m_ranks[m_select - 13].rank_list.player_achieve[i]) + m_ranks[m_select - 13].rank_list.player_name[i].ToString();
			_obj.transform.Find("score").GetComponent<UILabel>().text = m_ranks[m_select - 13].rank_list.value[i].ToString();
			GameObject m_icon1 =  _obj.transform.Find("icon").gameObject;
			sys._instance.remove_child (m_icon1);
			GameObject _obj1 = icon_manager._instance.create_player_icon((int)m_ranks[m_select - 13].rank_list.player_template[i],m_ranks[m_select - 13].rank_list.player_achieve[i],m_ranks[m_select - 13].rank_list.player_vip[i],m_ranks[m_select -13].rank_list.player_nalflag[i]);
			
			_obj1.transform.parent = m_icon1.transform;
			_obj1.transform.localScale = new Vector3(1,1,1);
			_obj1.transform.localPosition = new Vector3(0,0,0);
			m_guids.Add(m_ranks[m_select - 13].rank_list.player_guid[i]);
			GameObject chenghao = _obj.transform.Find("chenghao").gameObject;
			sys._instance.get_chenghao((int)m_ranks[m_select - 13].rank_list.player_chenghao[i],chenghao);
		}
		sys._instance.remove_child (m_icon);
		GameObject _obj2 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count,
		                                                             sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
		
		_obj2.transform.parent = m_icon.transform;
		_obj2.transform.localScale = new Vector3(1,1,1);
		_obj2.transform.localPosition = new Vector3(0,0,0);
		bool flag = false;
		for(int i = 0; i < m_ranks[m_select - 13].rank_list.player_guid.Count;++i)
		{
			if(m_ranks[m_select - 13].rank_list.player_guid[i] == sys._instance.m_self.m_t_player.guid)
			{
				m_ph.GetComponent<UILabel>().text =  string.Format(game_data._instance.get_t_language ("arena.cs_179_81") ,(i +1).ToString());//第{0}名
				flag = true;
				break;
			}
		}
		if(!flag)
		{
			m_ph.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81");//未上榜
			if(m_select == 14)
			{
				m_norank.SetActive(true);
			}
			else
			{
				m_norank.SetActive(false);
			}
		}
		else
		{
			m_norank.SetActive(false);
		}
		m_score.transform.GetComponent<UILabel>().text = score.ToString();
		m_name.transform.GetComponent<UILabel>().text = game_data._instance.get_name_color (sys._instance.m_self.m_t_player.dress_achieves.Count)
			+ sys._instance.m_self.m_t_player.name;
		sys._instance.get_chenghao (sys._instance.m_self.m_t_player.chenghao_on,m_chenghao);
	}
	
	void IMessage.message(s_message message)
	{

	}
	
	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_RANK_VIEW)
		{
			protocol.game.smsg_rank_view _msg = net_http._instance.parse_packet<protocol.game.smsg_rank_view> (message.m_byte);
			m_ranks[m_select - 13] = _msg;
			reset_ph();
		}
	}
}
