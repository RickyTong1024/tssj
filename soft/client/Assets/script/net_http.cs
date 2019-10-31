
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class net_pck
{
	public opclient_t opcode;
	public object obj;
	public float wait;
};
public class net_http : MonoBehaviour,IMessage {
	
	public static net_http _instance;
	[System.NonSerialized]
	public string m_ip;
	[System.NonSerialized]
    public string m_pvp_ip;
    bool is_pvp = false;
	private bool m_exdo = false;
	
	private List<net_pck> m_pcks = new List<net_pck>();
	
	private float m_wait = 0;
	private WWW m_www;
	private WWW m_www_ex;
	
	void Awake()
	{
		_instance = this;
	}
	
	void Start ()
    {
		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public  static int bytesToInt(byte[] bytes) {
		
		int addr = bytes[0];
		
		addr |= ((bytes[1] << 8));
		
		addr |= ((bytes[2] << 16));
		
		addr |= ((bytes[3] << 24));
		
		return addr;
	}
	
	private void onRecMsg(opclient_t opcode, byte[] msg)
	{
		s_net_message _message = new s_net_message ();
		_message.m_byte = msg;
		_message.m_opcode = opcode;
		cmessage_center._instance.add_net_message (_message);
	}
	
	private bool do_www(WWW _www, opclient_t opcode, byte[] msg, bool flag)
	{
        if (!string.IsNullOrEmpty(_www.error))
        {           
            string text = game_data._instance.get_t_language("net_http.cs_110_18");//系统错误
            root_gui._instance.show_prompt_dialog_box("[ffc882]" + text + _www.error);
            Debug.Log("http error  :" + _www.error);
            return false;
        }
        else
		{
			if (flag)
			{
				root_gui._instance.wait(false);
				m_wait = 0;
			}
			int _length = sizeof(int);
			byte[] _lenbyte = new byte[_www.bytes.Length - _length];
            Buffer.BlockCopy(_www.bytes, _length, _lenbyte, 0,_lenbyte.Length);
			int _err = bytesToInt(_www.bytes);
			if(_err == 0)
			{
				if (flag)
				{
					sys._instance.m_self.m_pck_id++;
				}
				onRecMsg(opcode, _lenbyte);
			}
			else if (_err < 0)
			{
                if (opcode != opclient_t.CMSG_RECHARGE_CHECK_EX)
                {
                    string text = game_data._instance.get_t_language("net_http.cs_110_18");//系统错误
                    string s = System.Text.Encoding.UTF8.GetString(_lenbyte, 0, _lenbyte.Length);
                    Debug.Log("unknow err: " + s);
                    root_gui._instance.show_prompt_dialog_box("[ffc882]" + text + s);
                }
			}
			else
			{
				Debug.Log("www err: " + game_data._instance.get_t_language(game_data._instance.m_dbc_error.get_index(2, _err)));

				if(_err == 20010 || _err == 20100 || _err == 20128)
				{
					string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
					s_message _msg = new s_message();

					_msg.m_type = "re_login";

					root_gui._instance.show_single_dialog_box(tishi,game_data._instance.get_t_language(game_data._instance.m_dbc_error.get_index(2, _err)),_msg);
				}
				root_gui._instance.show_prompt_dialog_box("[ffc882]" + game_data._instance.get_t_language(game_data._instance.m_dbc_error.get_index(2, _err)));
			}
			return true;
		}
	}

    public void send_msg<T>(opclient_t opcode, T obj,bool pvp = false, float wait = 600)
	{
		net_pck np = new net_pck();
		np.opcode = opcode;
		np.obj = obj;
		np.wait = wait;
		m_pcks.Add(np);
        is_pvp = pvp;
		if (m_pcks.Count > 1)
		{
			return;
		}
		
		net_start();
	}
	
	public void send_msg_ex<T>(opclient_t opcode, T obj)
	{
		Type t = obj.GetType ();
		if (t.GetProperty ("comm") != null)
		{
			protocol.game.msg_common_ex comm = new protocol.game.msg_common_ex();
			comm.guid = sys._instance.m_self.m_guid;
			comm.sig = sys._instance.m_self.m_sig;
			t.GetProperty ("comm").SetValue(obj, comm, null);
		}

		System.IO.MemoryStream _memStream = new System.IO.MemoryStream ();  
		ProtoBuf.Serializer.Serialize(_memStream,obj);
		
		//StopAllCoroutines ();
		if (m_exdo)
		{
			return;
		}
		m_exdo = true;
		StartCoroutine(httpex(opcode, encrypt_des.encode(_memStream.ToArray ())));
	}
	
	public T parse_packet<T>(byte[] bytes)
	{
		System.IO.MemoryStream _ms = new System.IO.MemoryStream(bytes);
		object _msg = new object();
		_msg = ProtoBuf.Serializer.Deserialize<T> (_ms); 
		
		return (T)_msg;  
	}
	
	public void net_start()
	{
		m_wait = m_pcks[0].wait;
		root_gui._instance.wait(true);
		
		Type t = m_pcks [0].obj.GetType ();
		if (t.GetProperty ("comm") != null)
		{
			protocol.game.msg_common comm = new protocol.game.msg_common();
			comm.guid = sys._instance.m_self.m_guid;
			comm.sig = sys._instance.m_self.m_sig;
			comm.pck_id = sys._instance.m_self.m_pck_id;
			comm.pck_gold = sys._instance.m_self.m_t_player.gold;
			comm.pck_jewel = sys._instance.m_self.m_t_player.jewel;
			t.GetProperty ("comm").SetValue(m_pcks [0].obj, comm, null);
		}
		
		System.IO.MemoryStream _memStream = new System.IO.MemoryStream ();  
		ProtoBuf.Serializer.Serialize(_memStream,m_pcks[0].obj);
		
		StartCoroutine(http(m_pcks[0].opcode, encrypt_des.encode(_memStream.ToArray ())));
	}
	
	void IMessage.net_message(s_net_message message)
	{
		
	}
	
	void IMessage.message(s_message message)
	{
		if(message.m_type == "net_restart")
		{
			net_start();
		}
        if (message.m_type == "do_set_storage")
        {
            StartCoroutine(do_set_storage());
        }
	}
	void Update () {
		
		if(m_wait > 0)
		{
			m_wait -= Time.deltaTime;
			
            if(m_wait <= 0)
            {
               string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
                string text = game_data._instance.get_t_language ("net_http.cs_76_28");//网络异常，建议检查网络通畅后重试
                this.StopAllCoroutines ();
               root_gui._instance.wait(false);
				
                s_message ok_message = new s_message();
                ok_message.m_type = "net_restart";

                s_message cancel_message = new s_message();
                cancel_message.m_type = "re_login";

                root_gui._instance.show_select_dialog_box(tishi, text, ok_message,cancel_message,false,true);
            }
		}
	}
	
	IEnumerator http(opclient_t opcode, byte[] msg)
	{
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/octet-stream");
        if (!is_pvp)
        {
            m_www = new WWW(m_ip + ((int)opcode).ToString(), msg, headers);
        }
        else
        {
            m_www = new WWW(m_pvp_ip + ((int)opcode).ToString(), msg, headers);
 
        }
		
		while(!m_www.isDone)
		{
			yield return new WaitForSeconds(0.1f);
		}
		
		bool res = do_www (m_www, opcode, msg, true);
		if (res)
		{
			m_pcks.RemoveAt (0);
			if (m_pcks.Count > 0)
			{
				net_start();
			}
		}
	}
	
	IEnumerator httpex(opclient_t opcode, byte[] msg)
	{
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Content-Type", "application/octet-stream");
        m_www_ex = new WWW (m_ip + ((int)opcode).ToString(), msg, headers);
		while(!m_www_ex.isDone)
		{
			yield return new WaitForSeconds(0.1f);
		}
		do_www(m_www_ex, opcode, msg, false);
		m_exdo = false;
	}


    IEnumerator do_set_storage()
    {
        WWWForm _form = new WWWForm();
        _form.AddField("token", game_data._instance.m_player_data.m_token);
        _form.AddField("serverid", sys._instance.m_self.m_t_player.serverid);
        _form.AddField("level", sys._instance.m_self.m_t_player.level);
        WWW _www = new WWW(platform_config_common.get_storage_url() + "set", _form);
        yield return _www;
    }
}
