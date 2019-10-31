
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class manzhan_gui : MonoBehaviour,IMessage {

	// Use this for initialization
	private int m_id;
	private s_t_item m_item;

	private List<int> m_item_ids = new List<int>();
	public UILabel m_jewel;
	public UILabel m_mianzhanbaohu;
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
		reset ();
		
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void OnEnable()
	{
		reset ();
	}

	void reset()
	{

		Transform _root_0 = this.transform.Find("back").Find("0");
		Transform _root_1 = this.transform.Find("back").Find("1");

		if(m_item_ids.Count == 0)
		{
			m_item_ids.Add (50050001);
			m_item_ids.Add (50050002);

			GameObject _icon = icon_manager._instance.create_item_icon(50050001);

			_icon.transform.parent = _root_0.Find("icon");
			_icon.transform.localPosition = Vector3.zero;
			_icon.transform.localScale = new Vector3(1,1,1);

			_icon = icon_manager._instance.create_item_icon(50050002);
			
			_icon.transform.parent = _root_1.Find("icon");
			_icon.transform.localPosition = Vector3.zero;
			_icon.transform.localScale = new Vector3(1,1,1);
		}

		s_t_item _item_0 = game_data._instance.get_item (m_item_ids[0]);
		s_t_item _item_1 = game_data._instance.get_item (m_item_ids[1]);

		int _item_num_0 = sys._instance.m_self.get_item_num ((uint)_item_0.id);
		int _item_num_1 = sys._instance.m_self.get_item_num ((uint)_item_1.id);



		_root_0.Find("time").Find("time").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("manzhan_gui.cs_61_91"), _item_0.def_1);//增加{0}小时
		_root_0.Find("time1").Find("time").GetComponent<UILabel>().text =  string .Format(game_data._instance.get_t_language ("manzhan_gui.cs_62_94"),_item_0.def_2);//{0}小时
		_root_0.Find("num").Find("num").GetComponent<UILabel>().text = _item_num_0.ToString();
		_root_0.Find("jewel").Find("num").GetComponent<UILabel>().text =  sys._instance.get_res_color(2) + _item_0.def_3.ToString();

		if(_item_num_0 > 0)
		{
			_root_0.Find("use").gameObject.SetActive(true);
			_root_0.Find("buy").gameObject.SetActive(false);
		}
		else
		{
			_root_0.Find("use").gameObject.SetActive(false);
			_root_0.Find("buy").gameObject.SetActive(true);
		}

        _root_1.Find("time").Find("time").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("manzhan_gui.cs_61_91"), _item_1.def_1);//增加{0}小时
        _root_1.Find("time1").Find("time").GetComponent<UILabel>().text = string.Format(game_data._instance.get_t_language ("manzhan_gui.cs_62_94"), _item_1.def_2);//{0}小时
		_root_1.Find("num").Find("num").GetComponent<UILabel>().text = _item_num_1.ToString();
		_root_1.Find("jewel").Find("num").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + _item_1.def_3.ToString();

		if(_item_num_1 > 0)
		{
			_root_1.Find("use").gameObject.SetActive(true);
			_root_1.Find("buy").gameObject.SetActive(false);
		}
		else
		{
			_root_1.Find("use").gameObject.SetActive(false);
			_root_1.Find("buy").gameObject.SetActive(true);
		}
		m_jewel.text = sys._instance.m_self.m_t_player.jewel + "";

	}

	void click(GameObject obj)
	{
		if(obj.transform.parent.name == "0")
		{
			m_id = m_item_ids[0];
		}

		if(obj.transform.parent.name == "1")
		{
			m_id = m_item_ids[1];
		}

		m_item = game_data._instance.get_item (m_id);


		if(obj.transform.name == "buy")
		{
			s_message _mes = new s_message();
			_mes.m_type = "buy_mianzhanpai";
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("baowu_mianzhan.cs_51_17"),string.Format(game_data._instance.get_t_language ("manzhan_gui.cs_115_67") , m_item.def_3 , sys._instance.get_res_info(2,m_item.id,0,0)),_mes);//购买免战牌//你确定要花费[00ffff]{0}钻石购买[{1}][-]吗？
		}

		if(obj.transform.name == "use")
		{
			protocol.game.cmsg_treasure_protect _msg = new protocol.game.cmsg_treasure_protect();
			if(m_id == m_item_ids[0])
			{
				_msg.type = 0;
			}
			else
			{
				_msg.type = 1;
			}
			net_http._instance.send_msg<protocol.game.cmsg_treasure_protect> (opclient_t.CMSG_TREASURE_PROTECT, _msg);
		}

		if(obj.name == "close")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_TREASURE_PROTECT) 
		{
			protocol.game.smsg_treasure_protect _msg = net_http._instance.parse_packet<protocol.game.smsg_treasure_protect> (message.m_byte);
			sys._instance.m_self.m_t_player.treasure_protect_cd_time = _msg.cd_time;
			sys._instance.m_self.m_t_player.treasure_protect_next_time = _msg.next_time;
			sys._instance.m_self.remove_item((uint)m_id,1,game_data._instance.get_t_language ("manzhan_gui.cs_145_49"));//免战保护消耗
			this.transform.Find("frame_big").GetComponent<frame>().hide();
			this.transform.parent.GetComponent<baowu_hecheng>().refresh_time();
		}
		else if(message.m_opcode == opclient_t.CMSG_TREASURE_BUY)
		{
			sys._instance.m_self.add_item((uint)m_id,1,game_data._instance.get_t_language ("manzhan_gui.cs_151_46"));//免战购买
            sys._instance.m_self.sub_att(e_player_attr.player_jewel, m_item.def_3, game_data._instance.get_t_language ("manzhan_gui.cs_152_83"));//免战购买消耗
			reset();
		}
			
	}
	void IMessage.message(s_message message)
	{
		if(message.m_type == "buy_mianzhanpai")
		{

			if(sys._instance.m_self.m_t_player.jewel < m_item.def_3)
			{
				root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language ("explore_gui.cs_118_47"));//[ffc882]钻石不足
				return;
			}
			
			protocol.game.cmsg_treasure_buy _msg = new protocol.game.cmsg_treasure_buy();
			if(m_id == m_item_ids[0])
			{
				_msg.type = 0;
			}
			else
			{
				_msg.type = 1;
			}
			net_http._instance.send_msg<protocol.game.cmsg_treasure_buy> (opclient_t.CMSG_TREASURE_BUY, _msg);
			
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
