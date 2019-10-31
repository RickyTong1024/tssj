using UnityEngine;
using System.Runtime.InteropServices;

public class platform : platform_common
{
    [DllImport("__Internal")]
    private static extern void initNotify();

    [DllImport("__Internal")]
    private static extern void createNotify(string text, int secondsFromNow);

    [DllImport("__Internal")]
    private static extern void cancelNotify();

    [DllImport("__Internal")]
    private static extern void copyTextToClipboard(string text);

    [DllImport("__Internal")]
    private static extern int getlanguage();

    [DllImport("__Internal")]
    private static extern void _save_photo(string readaddr);

    [DllImport("__Internal")]
    private static extern void returnPrice(string sku);

    [DllImport("__Internal")]
    private static extern void gameu_open_store();

    [DllImport("__Internal")]
    private static extern string platform_id();

    [DllImport("__Internal")]
    private static extern void platform_game_user_upgrade(int level);

    [DllImport("__Internal")]
    private static extern void platform_game_login();

    [DllImport("__Internal")]
    private static extern void platform_on_game_login(string param);

    [DllImport("__Internal")]
    private static extern void platform_game_logout();

    [DllImport("__Internal")]
    private static extern void platform_game_init();

    private static platform _platform;
    private static bool _isHadLogined;  //是否第三方已登陆
    private string cur_token = null;
    private string cur_pass = null;

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

    public override void init(GameObject obj)
    {
        m_photo_time = 1.2f;
        if (Application.isEditor)
        {
            return;
        }
        platform_game_init();
        initNotify();
        base.init(obj);
    }

    public override void create_notify(string title, string text, string ticker, int secondsFromNow)
    {
        if (Application.isEditor)
        {
            return;
        }
		createNotify(text, secondsFromNow);
    }

    public override void cancel_notify()
    {
        if (Application.isEditor)
        {
            return;
        }
        cancelNotify();
    }

    public override void copy(string text)
    {
        if (Application.isEditor)
        {
            return;
        }
        copyTextToClipboard(text);
    }

    public override void game_login()
    {
        if (!_isHadLogined)
        {
            if (Application.isEditor)
            {
                return;
            }
            platform_game_login();
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
        string token = mess[0];
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

    private void login_confirm()  //登录验证
    {
        if (cur_token == null || cur_pass == null)
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

    public override void do_save_photo_platform(string name, string message)
    {
        string path_read = Application.persistentDataPath + "/" + name;
        _save_photo(path_read);

        if (message != "")
        {
            s_message msg = new s_message();
            msg.m_type = message;
            cmessage_center._instance.add_message(msg);
        }
    }

    public override void game_logout()
    {
        if (Application.isEditor)
        {
            return;
        }
        platform_game_logout();
    }

    public override void game_update()
    {
        if (Application.isEditor)
        {
            return;
        }
        gameu_open_store();
    }

    public override string get_platform_id()
    {
        if (Application.isEditor)
        {
            return "";
        }
        return platform_id();
    }

    public override void on_game_user_upgrade(int level)
    {
        if (Application.isEditor)
        {
            return;
        }
        platform_game_user_upgrade(level);
    }

    public override int get_language()
    {
        if (Application.isEditor)
        {
            return 0;
        }
        return getlanguage();
    }

    public override void on_game_login(int is_new)
    {
        if (Application.isEditor)
        {
            return;
        }
        string param = sys._instance.m_self.m_t_player.guid + "_" + sys._instance.m_self.m_t_player.name + "_" + sys._instance.m_self.m_t_player.level + "_" + sys._instance.m_self.m_t_player.serverid + "_" + sys._instance.m_sname + "_" + is_new;
        platform_on_game_login(param);
    }
}
