
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Runtime.InteropServices;

public struct packet
{
	public byte m_compress;
	public short m_opcode;
	public int m_hid;
	public int m_size;
	public long m_guid;
}

public class net_tcp : MonoBehaviour {
	
	public static net_tcp _instance;

	private int m_header_size = 24;
	private packet m_packet = new packet();
	private bool m_is_head = true;
	private Socket m_client;
	[System.NonSerialized]
	public string m_ip;
	[System.NonSerialized]
	public int m_port;
	protected bool m_start = false;
	[System.NonSerialized]
    public bool m_connect = false;

	void Awake()
	{
		_instance = this;
	}

	void Start () {
		m_header_size = 24;
	}

	public void connect()
	{
		m_start = true;
		if (m_client != null)
		{
			disconnect();
		}
        m_connect = true;
		m_client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		m_client.ReceiveTimeout = 10;
		m_client.BeginConnect (m_ip, m_port, new AsyncCallback(connect_callback), m_client);
	}

	protected virtual void connect_callback(IAsyncResult ar)
	{
   
        Socket client = (Socket)ar.AsyncState;
        client.EndConnect(ar);
        m_connect = false;
	}

	public void disconnect()
	{
		if (m_client == null)
		{
			return;
		}
		m_client.Close ();
		m_client = null;
		m_is_head = true;
	}

	bool is_online()
	{
		if (m_client == null)
		{
			return false;
		}
		return !((m_client.Poll(100, SelectMode.SelectRead) && (m_client.Available == 0)) || !m_client.Connected);
	}

	public byte[] Struct2Bytes<T>(T obj)
	{
		int _size = Marshal.SizeOf(obj);
		byte[] bytes = new byte[_size];        
		IntPtr arrPtr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
		Marshal.StructureToPtr(obj, arrPtr, true);       
		return bytes;
	}

	public T Bytes2Struct<T>(byte[] bytes)
	{        
		IntPtr arrPtr = Marshal.UnsafeAddrOfPinnedArrayElement(bytes, 0);
		return (T)Marshal.PtrToStructure(arrPtr, typeof(T));    
	}
	
	public T parse_packet<T>(byte[] bytes)
	{
		System.IO.MemoryStream _ms = new System.IO.MemoryStream(bytes);
		object _msg = new object();
		_msg = ProtoBuf.Serializer.Deserialize<T> (_ms); 
		
		return (T)_msg;  
	}

	public void compress_data(byte[] inData, out byte[] outData)
	{
		MemoryStream outMemoryStream = new MemoryStream ();
		zlib.ZOutputStream outZStream = new zlib.ZOutputStream (outMemoryStream, zlib.zlibConst.Z_DEFAULT_COMPRESSION);
		Stream inMemoryStream = new MemoryStream (inData);
		
		CopyStream(inMemoryStream, outZStream);
		outZStream.finish();
		outData = outMemoryStream.ToArray();
		
	}
	
	public void decompress_data(byte[] inData, out byte[] outData)
	{
		MemoryStream outMemoryStream = new MemoryStream ();
		zlib.ZOutputStream outZStream = new zlib.ZOutputStream (outMemoryStream);
		Stream inMemoryStream = new MemoryStream (inData);
		
		CopyStream(inMemoryStream, outZStream);
		outZStream.finish();
		outData = outMemoryStream.ToArray();
	}
	
	public void CopyStream(System.IO.Stream input, System.IO.Stream output)
	{
		byte[] buffer = new byte[2000];
		int len;
		while ((len = input.Read(buffer, 0, 2000)) > 0)
		{
			output.Write(buffer, 0, len);
		}
		output.Flush();
	}   
	
	public void onRecMsg(byte[] msg)
	{
		s_net_message _message = new s_net_message ();
		
		if (m_packet.m_compress > 0) 
		{
			byte[] _out = new byte[m_packet.m_size];
			MemoryStream outMemoryStream = new MemoryStream ();
			zlib.ZOutputStream outZStream = new zlib.ZOutputStream (outMemoryStream);
			Stream inMemoryStream = new MemoryStream (msg);
			
			CopyStream(inMemoryStream, outZStream);
			outZStream.finish();
			_out = outMemoryStream.ToArray();
			
			_message.m_byte = _out;
		}
		else
		{
			_message.m_byte = msg;
		}
		_message.m_opcode = (opclient_t)m_packet.m_opcode;
		cmessage_center._instance.add_net_message (_message);
	}

	public void ReceiveMsg()
	{
		if (!is_online())
		{
            print("socket disconnect");
			disconnect();
            m_connect = true;
            s_message _mes = new s_message();
            _mes.m_type = "tcp_reconnet";
            cmessage_center._instance.add_message(_mes);
            connect();
			return;
		}
		if(m_is_head)
		{
			if(m_client.Available < m_header_size)
			{
				return;
			}
			
			byte[] _lenbyte = new byte[m_header_size];
			m_client.Receive(_lenbyte,0,m_header_size, SocketFlags.None);
			
			m_packet.m_compress = _lenbyte[0];
			m_packet.m_opcode = System.BitConverter.ToInt16(_lenbyte,2);
			m_packet.m_hid = System.BitConverter.ToInt32(_lenbyte,4);
			m_packet.m_size = System.BitConverter.ToInt32(_lenbyte,8);
			m_packet.m_guid = System.BitConverter.ToInt64(_lenbyte,16);
			m_is_head = false;
		}
		
		if(!m_is_head)
		{
			byte[] _lenbyte = new byte[m_packet.m_size];
			
			if(m_packet.m_size > 0)
			{
                int flag = m_client.Receive(_lenbyte, 0, m_packet.m_size, SocketFlags.None);
                while (flag != m_packet.m_size)
                {
                    flag += m_client.Receive(_lenbyte, flag, m_packet.m_size - flag, SocketFlags.None);
                }  
			}
			
			m_is_head = true;
			
			onRecMsg(_lenbyte);
		}
	}

	public void send_msg_null(opclient_t opcode)
	{
		if (m_client == null || !m_client.Connected)
		{
			return;
		}

		byte[] _data = new byte[m_header_size];

		packet _packer = new packet ();
		
		_packer.m_compress = 0;
		_packer.m_opcode = (short)opcode;
		_packer.m_hid = 0;
		_packer.m_guid = (long)sys._instance.m_self.m_guid;
		_packer.m_size = 0;

		byte _compress = 0;
		System.BitConverter.GetBytes(_compress).CopyTo(_data,0);
		System.BitConverter.GetBytes((short)opcode).CopyTo(_data,2);
		System.BitConverter.GetBytes(0).CopyTo(_data,4);
		System.BitConverter.GetBytes(0).CopyTo(_data,8);
		System.BitConverter.GetBytes(sys._instance.m_self.m_guid).CopyTo(_data,16);
        m_client.Send(_data);
    }
    void OnDisable()
    {
      
    }
	public void send_msg<T>(opclient_t opcode,T obj)
	{
		if (m_client == null || !m_client.Connected)
		{
            print(game_data._instance.get_t_language ("net_tcp.cs_255_18"));//连接已断开
			return;
		}

		System.IO.MemoryStream _memStream = new System.IO.MemoryStream ();
		ProtoBuf.Serializer.Serialize(_memStream,obj);
		
		byte[] msg = _memStream.ToArray ();
		
		byte[] _data = new byte[m_header_size + msg.Length];
		
		packet _packer = new packet ();
		
		_packer.m_compress = 0;
		_packer.m_opcode = (short)opcode;
		_packer.m_hid = 0;
		_packer.m_guid = (long)sys._instance.m_self.m_guid;
		_packer.m_size = msg.Length;
		byte _compress = 0;
		System.BitConverter.GetBytes(_compress).CopyTo(_data,0);
		System.BitConverter.GetBytes((short)opcode).CopyTo(_data,2);
		System.BitConverter.GetBytes(0).CopyTo(_data,4);
		System.BitConverter.GetBytes(msg.Length).CopyTo(_data,8);
		System.BitConverter.GetBytes(sys._instance.m_self.m_guid).CopyTo(_data,16);
		msg.CopyTo (_data, 24);
		m_client.Send(_data);
	}
	// Update is called once per frame
	public void Update () {
		if (m_client == null || m_connect)
		{
			return;
		}
		ReceiveMsg ();
	}
}
