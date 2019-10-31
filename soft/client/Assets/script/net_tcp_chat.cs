
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Runtime.InteropServices;


public class net_tcp_chat : net_tcp {

    public static net_tcp _instance;

    void Awake()
    {
        _instance = this;
    }
    protected override void connect_callback(IAsyncResult ar)
    {
        try
        {
            base.connect_callback(ar);
            net_tcp_chat._instance.send_msg_null(opclient_t.CMSG_CHAT_PLAYER);
           
        }
        catch (Exception)
        {
            if (m_start)
            {
                connect();
            }
            else
            {
                disconnect();
            }
        }
    }

}
