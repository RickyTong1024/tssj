
using UnityEngine;
using System.Collections;

public class dj_gui : MonoBehaviour, IMessage {

	public GameObject m_djtan;
	public GameObject m_dj;


	public UILabel m_lianjin;
	public UILabel m_desc1;
	public UILabel m_desc2;
	public UILabel m_jinrishenyu;
	public UILabel m_lianjin_label;
	public UILabel m_lianjin10ci;
	public UILabel m_huafei;
	// Use this for initialization
	void Start () 
	{
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
		if (sys._instance.m_self.m_t_player.vip < (int)e_open_vip.ev_lianjin10)
		{
			m_djtan.SetActive(false);
            m_dj.transform.localPosition = new Vector3(0, -147.0f, 0);
		}
		else
		{
			m_djtan.SetActive(true);
            m_dj.transform.localPosition = new Vector3(-110, -147.0f, 0);
            m_djtan.transform.localPosition = new Vector3(110, -147.0f, 0);
		}
		GameObject back = this.transform.Find("back").gameObject;

		m_dj.GetComponent<BoxCollider>().enabled = true;
		m_dj.GetComponent<UISprite>().set_enable(true);
		m_djtan.GetComponent<BoxCollider>().enabled = true;
		m_djtan.GetComponent<UISprite>().set_enable(true);
		int num = sys._instance.m_self.m_t_player.dj_num;
		s_t_price t_price = game_data._instance.get_t_price (num + 1);
		s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
		back.transform.Find("renum").GetComponent<UILabel>().text = (t_vip.dj_num - num).ToString() + "/" + t_vip.dj_num.ToString();
		if (t_vip.dj_num <= num)
		{
			m_dj.GetComponent<BoxCollider>().enabled = false;
			m_dj.GetComponent<UISprite>().set_enable(false);
			m_djtan.GetComponent<BoxCollider>().enabled = false;
			m_djtan.GetComponent<UISprite>().set_enable(false);
		}
		string jewel = t_price.dj.ToString();
		if (sys._instance.m_self.get_att(e_player_attr.player_jewel) < t_price.dj)
		{
			jewel = "[ff0000]" + jewel;
			m_dj.GetComponent<BoxCollider>().enabled = false;
			m_dj.GetComponent<UISprite>().set_enable(false);
			m_djtan.GetComponent<BoxCollider>().enabled = false;
			m_djtan.GetComponent<UISprite>().set_enable(false);
		}
		back.transform.Find("jewel").GetComponent<UILabel>().text = sys._instance.get_res_color(2) + jewel;
		int gold = (int)(40000 * (1 + 0.008 * sys._instance.m_self.get_att (e_player_attr.player_level)));
		back.transform.Find("gold").GetComponent<UILabel>().text = sys._instance.get_res_color(1) + gold.ToString();
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "hide")
		{
			this.transform.Find("frame_big").GetComponent<frame>().hide();
		}
		else if (obj.transform.name == "dj")
		{
			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_DJ, _msg);
		}

		else if (obj.transform.name == "djten")
		{
			int num = sys._instance.m_self.m_t_player.dj_num;
			s_t_price t_price = game_data._instance.get_t_price (num + 1);
			s_t_vip t_vip = game_data._instance.get_t_vip (sys._instance.m_self.m_t_player.vip);
			int dnum = 10;
			if (t_vip.dj_num - num < 10)
			{
				dnum = t_vip.dj_num - num;
			}
			int jewel = 0;
			for (int i = num + 1; i <= num + dnum; ++i)
			{
				t_price = game_data._instance.get_t_price (i);
				jewel += t_price.dj;
			}

			string _des = string.Format(game_data._instance.get_t_language ("dj_gui.cs_112_31"),jewel.ToString(), dnum.ToString());//是否花费[00ffff]{0}钻石[-]炼金[00ff00]{1}[-]次
			s_message _message = new s_message();
			_message.m_type = "djten";
			_message.m_ints.Add(jewel);
			root_gui._instance.show_select_dialog_box(game_data._instance.get_t_language ("arena.cs_104_45"),_des,_message);//提示
		}
	}

	void IMessage.message (s_message message)
	{
		if (message.m_type == "djten")
		{
			int _count = (int)message.m_ints[0];
			if (_count > sys._instance.m_self.get_att(e_player_attr.player_jewel))
			{
				root_gui._instance.show_recharge_dialog_box(delegate() {
					this.transform.Find("frame_big").GetComponent<frame>().hide();
				});
				return;
			}

			protocol.game.cmsg_common _msg = new protocol.game.cmsg_common ();
			net_http._instance.send_msg<protocol.game.cmsg_common> (opclient_t.CMSG_DJTEN, _msg);
		}
	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_DJTEN) 
		{
			protocol.game.smsg_dj_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_dj_reward> (message.m_byte);
			
			int num = sys._instance.m_self.m_t_player.dj_num;
			int jewel = 0;
			for (int i = num + 1; i <= num + _msg.bs; ++i)
			{
				s_t_price t_price = game_data._instance.get_t_price (i);
				jewel += t_price.dj;
			}

			sys._instance.m_self.sub_att(e_player_attr.player_jewel, jewel,game_data._instance.get_t_language ("dj_gui.cs_152_66"));//10次炼金消耗
			sys._instance.m_self.m_t_player.dj_num += _msg.bs;

			sys._instance.m_self.add_att(e_player_attr.player_gold, _msg.gold,game_data._instance.get_t_language ("dj_gui.cs_155_69"));//10次炼金获得
			sys._instance.m_self.add_active(850, _msg.bs);
			
			reset ();
		}
		else if (message.m_opcode == opclient_t.CMSG_DJ) 
		{
			protocol.game.smsg_dj_reward _msg = net_http._instance.parse_packet<protocol.game.smsg_dj_reward> (message.m_byte);
			
			int num = sys._instance.m_self.m_t_player.dj_num;
			s_t_price t_price = game_data._instance.get_t_price (num + 1);
			
			sys._instance.m_self.sub_att(e_player_attr.player_jewel, t_price.dj,game_data._instance.get_t_language ("dj_gui.cs_167_71"));//炼金消耗
			sys._instance.m_self.m_t_player.dj_num++;
			
			if (_msg.bs == 2)
			{
				root_gui._instance.show_prompt_dialog_box("[7777ff]" + game_data._instance.get_t_language ("dj_gui.cs_172_59"));//双倍暴击
			}
			else if (_msg.bs == 5)
			{
				root_gui._instance.show_prompt_dialog_box("[ffff77]" + game_data._instance.get_t_language ("dj_gui.cs_176_59"));//五倍暴击
			}
			else if (_msg.bs == 10)
			{
				root_gui._instance.show_prompt_dialog_box("[ff7777]" + game_data._instance.get_t_language ("dj_gui.cs_180_59"));//十倍暴击
			}
			sys._instance.m_self.add_att(e_player_attr.player_gold, _msg.gold,game_data._instance.get_t_language ("dj_gui.cs_182_69"));//炼金获得
			sys._instance.m_self.add_active(850, 1);
			
			reset ();
		}
	}
	// Update is called once per frame
	void Update () {
	
	}

}
