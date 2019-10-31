
using UnityEngine;
using System.Collections;
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
    private static extern void returnPrice(string sku);

    [DllImport("__Internal")]
    private static extern void game_open_store();

    [DllImport("__Internal")]
    private static extern string platform_id();

    private static platform _platform;

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


    public override void game_update()
    {
        if (Application.isEditor)
        {
            return;
        }
        game_open_store();
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
        analytics_management.m_onGameLogin(is_new);
    }
}
