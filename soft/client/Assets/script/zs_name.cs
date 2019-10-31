
using UnityEngine;
using System.Collections;

public class zs_name : MonoBehaviour,IMessage {

	public GameObject m_name;
	public GameObject m_text;

	string[] m_name_lists;
	public int m_type = 0;
    
	void Start () 
	{
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void reset(int type)
	{
		m_type = type;
		if (m_type == 0)
		{
			m_text.GetComponent<UILabel>().text = game_data._instance.get_t_language ("zs_name.cs_33_42");//我叫...
		}
		else
		{
			m_name_lists = new string[6];
			m_name_lists [0] = game_data._instance.get_t_language ("zs_name.cs_16_21");//艾莉萨
			m_name_lists [1] = game_data._instance.get_t_language ("zs_name.cs_17_21");//艾丽斯
			m_name_lists [2] = game_data._instance.get_t_language ("zs_name.cs_18_21");//安吉莉娜
			m_name_lists [3] = game_data._instance.get_t_language ("zs_name.cs_19_21");//凯丽
			m_name_lists [4] = game_data._instance.get_t_language ("zs_name.cs_20_21");//达芙妮
			m_name_lists [5] = game_data._instance.get_t_language ("zs_name.cs_21_21");//安吉莉亚
			m_text.GetComponent<UILabel>().text = game_data._instance.get_t_language ("zs_name.cs_37_42");//给您的助手起个名字吧
		}
		rand_name ();
	}
    
	public string rand()
	{
		int _max = game_data._instance.m_dbc_name.get_y ();

       
		int w1 = 0;
		for (int i = 0; i < _max; ++i)
		{
            if (game_data._instance.m_dbc_name.get(((int)game_data._instance.m_language) * 2, i) == "0")
			{
				break;
			}
			else
			{
				w1 = i;
			}
		}
		int w2 = 0;
		for (int i = 0; i < _max; ++i)
		{
            if (game_data._instance.m_dbc_name.get((((int)game_data._instance.m_language) * 2) + 1, i) == "0")
			{
				break;
			}
			else
			{
				w2 = i;
			}
		}

        string _one_name = game_data._instance.m_dbc_name.get(((int)game_data._instance.m_language) * 2, Random.Range(0, w1));
        string _two_name = game_data._instance.m_dbc_name.get((((int)game_data._instance.m_language) * 2) + 1, Random.Range(0, w2));
		
		return _one_name + _two_name;
	}

	void rand_name()
	{
		if (m_type == 0)
		{
			m_name.GetComponent<UILabel>().text = rand();
		}
		else
		{
			m_name.GetComponent<UILabel>().text = m_name_lists[Random.Range(0,m_name_lists.Length)];
		}
	}

	public void click(GameObject obj)
	{
		if(obj.transform.name == "rand")
		{
			rand_name();
		}

		if(obj.transform.name == "ok")
		{
			if (m_type == 0)
			{
                string text = m_name.GetComponent<UILabel>().text;
                if (game_data._instance.m_dfa.fei_fa(text) || text.IndexOf(" ") >= 0)
                {
                    root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("info.cs_277_58"));//[ffc882]非法名字
                    return;
                }
                protocol.game.cmsg_player_name _msg = new protocol.game.cmsg_player_name ();				
				_msg.name = m_name.GetComponent<UILabel>().text;
				net_http._instance.send_msg<protocol.game.cmsg_player_name> (opclient_t.CMSG_PLAYER_NAME, _msg);
			}
			else
			{
				protocol.game.cmsg_player_zsname _msg = new protocol.game.cmsg_player_zsname ();				
				_msg.zsname = m_name.GetComponent<UILabel>().text;				
				net_http._instance.send_msg<protocol.game.cmsg_player_zsname> (opclient_t.CMSG_PLAYER_ZSNAME, _msg);
			}
		}
	}

	void IMessage.net_message(s_net_message message)
	{
		if (message.m_opcode == opclient_t.CMSG_PLAYER_ZSNAME && m_type == 1)
		{
			sys._instance.m_self.m_t_player.zsname = m_name.GetComponent<UILabel>().text;

			this.GetComponent<ui_show_anim>().hide_ui();
            root_gui._instance.action_guide("zs_name_end");


        }
        if (message.m_opcode == opclient_t.CMSG_PLAYER_NAME && m_type == 0)
		{
            protocol.game.smsg_player_name state = net_http._instance.parse_packet<protocol.game.smsg_player_name>(message.m_byte);
            if (state.code_stat == 0 || state.code_stat == 1 || state.code_stat == 2)
            {
                sys._instance.m_self.m_t_player.name = m_name.GetComponent<UILabel>().text;
                this.GetComponent<ui_show_anim>().hide_ui();

                if (platform_config_common.m_nationality == 0)
                {
                    root_gui._instance.action_guide("player_name_end");
                }
                else
                {
                    s_message _mes_nationality = new s_message();
                    _mes_nationality.m_type = "show_nationality";
                    _mes_nationality.m_ints.Add(1);
                    cmessage_center._instance.add_message(_mes_nationality);
                }

                s_message _mes = new s_message();
                _mes.m_type = "update_player_att";
                cmessage_center._instance.add_message(_mes);
                if (state.code_stat == 1)
                {
                    sys._instance.m_self.add_att(e_player_attr.player_jewel, 100,game_data._instance.get_t_language ("zs_name.cs_138_81"));//邀请进入游戏获得
                }

            }
            else
            {
                root_gui._instance.show_prompt_dialog_box("[ffc884]" + game_data._instance.get_t_language ("zs_name.cs_144_71"));//没有该邀请码，请确认后重试
            }
			
		}
	}
	void IMessage.message(s_message message)
	{

	}
	// Update is called once per frame
	void Update () {
	
	}
}
