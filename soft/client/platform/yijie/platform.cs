
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class platform : platform_common
{
    private static platform _platform;
    private static bool _isHadLogined;  //是否第三方已登陆
    private string cur_token=null;
    private string cur_pass=null;
    public static platform _instance
    {
        get
        {
            if (_platform == null)
            {
                _platform = new platform();
            }
            return _platform;
        }
    }

    public override void game_login()
    {
        if (!_isHadLogined)
        {
            if (Application.isEditor)
            {
                return;
            }
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
            jo.Call("game_login");
        }
        else
        {
            //验证登陆
            login_confirm();
        }
    }

    public override void platform_login_success(string s)
    {
        string[] mess = s.Split(' ');
        string token= mess[0];
        string pass = mess[1];
        setConfirmData(token, pass);
        string logined = mess[2];
        if (logined == "0")
        {
            login_confirm();
        }
        else if (logined == "1")
        {
            sys._instance.game_logout();
        }
        _isHadLogined = true;
    }

    public override void platfrom_logout_success(string s)
    {
        _isHadLogined = false;
        sys._instance.game_logout();
    }

    public override string get_platform_isbn()
    {
        if (Application.isEditor)
        {
            return "";
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        string isbn = jo.Call<string>("get_platform_isbn");
        return isbn;
    }

    private void login_confirm()  //登录验证
    {
        if (cur_token==null || cur_pass==null)
        {
            return;
        }
        game_data._instance.m_player_data.m_token = cur_token;
        game_data._instance.m_player_data.m_pass = cur_pass;
        s_message _msg = new s_message();
        _msg.m_type = "game_user_success_login_game";
        cmessage_center._instance.add_message(_msg);
    }

    private void setConfirmData(string token, string pass)
    {
        cur_token = token;
        cur_pass = pass;
    }
}
