
using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System;
using System.IO;
using System.Runtime.InteropServices;

public class net_tcp_bingyua : net_tcp,IMessage
{
    public static net_tcp _instance;
    public static string ip = "";
    public static int port = 8001;
    float m_wait = -0.1f;
    bool m_tcp_reconnet;
    void Awake()
    {
        _instance = this;
        m_ip = ip;
        m_port = port;
        cmessage_center._instance.add_handle(this);
    }

	protected override void connect_callback(IAsyncResult ar)
	{
        try
        {
            base.connect_callback(ar);
            print("socket connect success");
            s_message mes = new s_message();
            mes.m_type = "bingyuan_login_check";
            cmessage_center._instance.add_message(mes);
            s_message _mes = new s_message();
            _mes.m_type = "socket_success";
            cmessage_center._instance.add_message(_mes);

        }
        catch (Exception e)
        {
            print(e);
            if (m_start)
            {
                if (m_tcp_reconnet)
                {
                    connect();
                }
            }
            else
            {
                disconnect();
            }
        }
	}
    void IMessage.message(s_message mes)
    {
        if (mes.m_type == "net_restart_bingyuan")
        {
            m_tcp_reconnet = true;
            m_wait = 10;
            root_gui._instance.wait(true);
            connect();
 
        }
        else if (mes.m_type == "tcp_reconnet")
        {
            m_tcp_reconnet = true;
            if (this.gameObject.activeSelf)
            {
                m_wait = 10;
                root_gui._instance.wait(true);
            }
           
 
        }
        else if (mes.m_type == "socket_success")
        {
            root_gui._instance.wait(false);
            m_tcp_reconnet = false;
            m_wait = -0.1f;
 
        }
 
    }
    void IMessage.net_message(s_net_message mes)
    {
 
    }
    void Update()
    {

        if (m_wait > 0 && m_tcp_reconnet)
        {
            m_wait -= Time.deltaTime;

            if (m_wait <= 0)
            {
                string tishi = game_data._instance.get_t_language ("arena.cs_104_45");//提示
                string text = game_data._instance.get_t_language ("net_http.cs_76_28");//网络异常，建议检查网络通畅后重试
                this.StopAllCoroutines();
                root_gui._instance.wait(false);

                s_message ok_message = new s_message();
                ok_message.m_type = "net_restart_bingyuan";

                s_message cancel_message = new s_message();
                cancel_message.m_type = "re_login";
                m_tcp_reconnet = false;
                root_gui._instance.show_select_dialog_box(tishi, text, ok_message, cancel_message, false,true);
            }
        }
        base.Update();

    }
    void OnDestroy()
    {
        cmessage_center._instance.remove_handle(this);
    }    
}
