
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mi_jing_ph : MonoBehaviour, IMessage {

	protocol.game.smsg_rank_view m_msg;
	public GameObject m_view;
	public GameObject m_pw;
	public GameObject m_name;
	public GameObject m_bf;
	public GameObject m_star;
	public GameObject m_icon;
    public GameObject m_chenhao;
	public UILabel m_name_Label;
	public UILabel m_ph_name_Label;
	public UILabel m_ph_Label;
	public UILabel m_ph_star_Label;
	public UILabel m_ph_fight_Label;

	void Start () {
		
		cmessage_center._instance.add_handle (this);
	}
	
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	public void OnEnable()
	{
		view ();
	}

	void IMessage.message (s_message message)
	{
	}

	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_RANK_TTT_VIEW)
		{
			m_msg = net_http._instance.parse_packet<protocol.game.smsg_rank_view> (message.m_byte);
			reset ();
		}
	}

	void view()
	{
		protocol.game.cmsg_rank_view _msg = new protocol.game.cmsg_rank_view ();
		_msg.type = 5;
		net_http._instance.send_msg<protocol.game.cmsg_rank_view> (opclient_t.CMSG_RANK_TTT_VIEW, _msg);
	}

	void reset()
	{
		int max_num = 20;
		dhc.rank_t rank_t = m_msg.rank_list;
		if (rank_t.player_guid.Count < max_num)
		{
			max_num = rank_t.player_guid.Count;
		}
		bool flag = false;
		m_view.transform.localPosition = new Vector3(0, 0, 0);
		m_view.GetComponent<UIPanel>().clipOffset = new Vector2(0, 0);
		sys._instance.remove_child (m_view);
		for (int i = 0; i < max_num; ++i)
		{
			GameObject rank_list = game_data._instance.ins_object_res("ui/mjrank");
			rank_list.transform.parent = m_view.transform;
			rank_list.transform.localPosition = new Vector3(0, 117 - i * 68,0);
			rank_list.transform.localScale = new Vector3(1,1,1);
			rank_list.transform.GetComponent<mjrank>().reset(i + 1, rank_t.player_name[i], rank_t.player_bf[i], rank_t.value[i],rank_t.player_template[i]
                                                             , rank_t.player_achieve[i], rank_t.player_vip[i], rank_t.player_chenghao[i],rank_t.player_nalflag[i]);
			rank_list.transform.GetComponent<mjrank>().m_guid = rank_t.player_guid[i];
			if (sys._instance.m_self.m_guid == rank_t.player_guid[i])
			{
				flag = true;
				m_pw.GetComponent<UILabel>().text = (i + 1).ToString();
			}
		}
		if (!flag)
		{
			m_pw.GetComponent<UILabel>().text = game_data._instance.get_t_language ("bingyuan_rank_gui.cs_83_81");//未上榜
		}
		m_name.GetComponent<UILabel>().text = game_data._instance.get_name_color(sys._instance.m_self.m_t_player.dress_achieves.Count) + sys._instance.m_self.m_t_player.name;
		m_bf.GetComponent<UILabel>().text = sys._instance.value_to_wan((long)sys._instance.m_self.get_fighting());
        sys._instance.get_chenghao(sys._instance.m_self.m_t_player.chenghao_on, m_chenhao);
        sys._instance.remove_child (m_icon);
		GameObject _obj1 = icon_manager._instance.create_player_icon((int)sys._instance.m_self.m_t_player.template_id,sys._instance.m_self.m_t_player.dress_achieves.Count
		                                                             ,sys._instance.m_self.m_t_player.vip,sys._instance.m_self.m_t_player.nalflag);
		
		_obj1.transform.parent = m_icon.transform;
		_obj1.transform.localScale = new Vector3(1,1,1);
		_obj1.transform.localPosition = new Vector3(0,0,0);
		int star = 0;
		for (int i = 0; i < sys._instance.m_self.m_t_player.ttt_last_stars.Count; ++i)
		{
			star += sys._instance.m_self.m_t_player.ttt_last_stars[i];
		}
		m_star.GetComponent<UILabel>().text = star.ToString();
	}

	public void click(GameObject obj)
	{
		if (obj.name == "close2")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
