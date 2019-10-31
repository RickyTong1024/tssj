
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class danmu_panel : MonoBehaviour, IMessage {

	private List<GameObject> m_danmus = new List<GameObject>();

	void Start () {
		cmessage_center._instance.add_handle (this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void IMessage.message (s_message message)
	{

	}
	
	void IMessage.net_message (s_net_message message)
	{
		if (game_data._instance.m_player_data.m_danmu == 0)
		{
			return;
		}
		if (m_danmus.Count > 15)
		{
			return;
		}
		if (message.m_opcode == opclient_t.SMSG_GUNDONG)
		{
			protocol.game.smsg_gundong _msg = net_http._instance.parse_packet<protocol.game.smsg_gundong> (message.m_byte);
			GameObject target = game_data._instance.ins_object_res("ui/danmu");
			target.transform.parent = this.transform;
			target.transform.localPosition = new Vector3(-5000, 0, 0);
			target.transform.localScale = new Vector3(1,1,1);
            target.transform.GetComponent<UILabel>().text = _msg.text;
			target.transform.GetComponent<danmu>().init();
			m_danmus.Add(target);
		}
		if (message.m_opcode == opclient_t.SMSG_CHAT)
		{
			protocol.game.smsg_chat _msg = net_http._instance.parse_packet<protocol.game.smsg_chat> (message.m_byte);
			if (_msg.type != 0)
			{
				return;
			}
			if(_msg.is_danmu == 0)
			{
				return;
			}
			GameObject target = game_data._instance.ins_object_res("ui/danmu");
			target.transform.parent = this.transform;
			target.transform.localPosition = new Vector3(-5000, 0, 0);
			target.transform.localScale = new Vector3(1,1,1);
			target.transform.GetComponent<UILabel>().text = _msg.color + _msg.player_name + "：" + _msg.text;
			target.transform.GetComponent<danmu>().init();
			m_danmus.Add(target);
		}
        if (message.m_opcode == opclient_t.SMSG_GUNDOG_SERVER)
        {
            protocol.game.smsg_gundong_server _msg = net_http._instance.parse_packet<protocol.game.smsg_gundong_server>(message.m_byte);

            GameObject target = game_data._instance.ins_object_res("ui/danmu");
            target.transform.parent = this.transform;
            target.transform.localPosition = new Vector3(-5000, 0, 0);
            target.transform.localScale = new Vector3(1, 1, 1);
        
            if (_msg.gundong_Pars.Count > 2)
            {
                target.transform.GetComponent<UILabel>().text = returnLanguage(_msg.text, _msg.gundong_Pars[0], _msg.gundong_Pars[1], _msg.gundong_Pars[2]);
            }
            else if (_msg.gundong_Pars.Count > 1)
            {
                target.transform.GetComponent<UILabel>().text = returnLanguage(_msg.text, _msg.gundong_Pars[0], _msg.gundong_Pars[1]);
            }
            else if (_msg.gundong_Pars.Count > 0)
            {
                target.transform.GetComponent<UILabel>().text = returnLanguage(_msg.text, _msg.gundong_Pars[0]);
            }
            else
            {
                target.transform.GetComponent<UILabel>().text = returnLanguage(_msg.text);
            }
           
            target.transform.GetComponent<danmu>().init();
            m_danmus.Add(target);

        }
	}
  
    string returnLanguage(string key, string value1 = null, string value2 = null, string value3 = null, string value4 = null, string value5 = null)
    {
        string text = "";

        if (!object.ReferenceEquals(value2, null))
        {
            string[] value2_split = value2.Split(' ');
            string temp = "";
            for (int i = 0; i < value2_split.Length; i++)
            {
                string[] value2_split_ = value2_split[i].Split(']');
               
                if (value2_split_.Length > 1)
                {
                    string temp1 = "";
                    temp1 = string.Format("{0}{1}{2}{3}", value2_split_[0], "]",game_data._instance.get_t_language(value2_split_[1]),"[-]");                  
                   

                    temp += " " + temp1;
                }
                else
                {

                    if (!key.Equals("t_server_language_text_levelup"))
                    {
                        value2 = game_data._instance.get_t_language (value2).Equals("") ? value2 : game_data._instance.get_t_language(value2);
                        break;
                    }
                    break;
                }

                value2 = temp;

            }
            

           /* if (value2_split.Length > 1)
            {
                string temp = "";
                temp = string.Format("{0}{1}", value2_split[0], "]");

                for (int i = 0; i < value2_split[1].Split(' ').Length; i++)
                {
                    temp = string.Format("{0}{1}", temp, game_data._instance.get_t_language(value2_split[1].Split(' ')[i]) + " ");
                }

                temp += "[-]";

                value2 = temp;
            }
            else
            {
                
            }*/
        }
        
        if (!object.ReferenceEquals(value5, null))
        {
            text = string.Format(game_data._instance.get_t_language(key),value1,value2,value3,value4,value5);
        }
        else if (!object.ReferenceEquals(value4, null))
        {
            text = string.Format(game_data._instance.get_t_language(key), value1, value2, value3, value4);
        }
        else if (!object.ReferenceEquals(value3, null))
        {
             text = string.Format(game_data._instance.get_t_language(key),value1,value2,value3);
        }
        else if (!object.ReferenceEquals(value2, null))
        {
            text = string.Format(game_data._instance.get_t_language(key),value1,value2);
        }
        else if (!object.ReferenceEquals(value1, null))
        {
             text = string.Format(game_data._instance.get_t_language(key),value1);
        }
        else
        {
             text = game_data._instance.get_t_language(key);
        }


        return text;
    }
    public void remove_all()
	{
		for (int i = 0; i < m_danmus.Count; ++i)
		{
			Object.Destroy(m_danmus[i]);
		}
		m_danmus.Clear ();
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < m_danmus.Count;)
		{
			if (m_danmus[i].GetComponent<danmu>().m_left == false)
			{
				Object.Destroy(m_danmus[i]);
				m_danmus.Remove(m_danmus[i]);
				continue;
			}
			i++;
		}
	}

}
