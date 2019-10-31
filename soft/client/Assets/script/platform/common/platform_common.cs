
using System;
using System.Text;
using UnityEngine;

public abstract class platform_common
{
    public float m_photo_time = 0.1f;
    public virtual void init(GameObject obj)
    {
        cancel_notify();
        int _second = timer.dtnow().Second;
        int _minute = timer.dtnow().Minute;
        int _hour = timer.dtnow().Hour;
        if (_hour < 12)
        {

            int time = (12 - _hour) * 60 - _minute;
            time = time * 60 - _second;
            create_notify(game_data._instance.get_t_language("MtaU3D.cs_94_17"), game_data._instance.get_t_language("MtaU3D.cs_94_17到了，赶快上线领取50体力吧"), game_data._instance.get_t_language("MtaU3D.cs_94_17到了，赶快上线领取50体力吧"), time);//午餐补给//MtaU3D.cs_94_25//午餐补给到了，赶快上线领取50体力吧
        }
        if (_hour < 13)
        {

            int time = (13 - _hour) * 60 - _minute;
            time = time * 60 - _second;
            create_notify(game_data._instance.get_t_language("MtaU3D.cs_100_17"), game_data._instance.get_t_language("MtaU3D.cs_100_26"), game_data._instance.get_t_language("MtaU3D.cs_100_26"), time);//魔王讨伐战//MtaU3D.cs_100_26//星际BOSS出现了，赶快上线打败它吧
        }
        if (_hour < 18)
        {

            int time = (18 - _hour) * 60 - _minute;
            time = time * 60 - _second;
            create_notify(game_data._instance.get_t_language("MtaU3D.cs_106_17"), game_data._instance.get_t_language("MtaU3D.cs_106_17到了，赶快上线领取50体力吧"), game_data._instance.get_t_language("MtaU3D.cs_106_17到了，赶快上线领取50体力吧"), time);//晚餐补给//MtaU3D.cs_106_25//晚餐补给到了，赶快上线领取50体力吧
        }
        {
            int time = (12 + 24 - _hour) * 60 - _minute;
            time = time * 60 - _second;
            create_notify(game_data._instance.get_t_language("MtaU3D.cs_94_17"), game_data._instance.get_t_language("MtaU3D.cs_94_17到了，赶快上线领取50体力吧"), game_data._instance.get_t_language("MtaU3D.cs_94_17到了，赶快上线领取50体力吧"), time);//午餐补给//MtaU3D.cs_94_25//午餐补给到了，赶快上线领取50体力吧
        }
        {
            int time = (13 + 24 - _hour) * 60 - _minute;
            time = time * 60 - _second;
            create_notify(game_data._instance.get_t_language("MtaU3D.cs_100_17"), game_data._instance.get_t_language("MtaU3D.cs_100_26"), game_data._instance.get_t_language("MtaU3D.cs_100_26"), time);//魔王讨伐战//MtaU3D.cs_100_26//星际BOSS出现了，赶快上线打败它吧
        }
        {
            int time = (18 + 24 - _hour) * 60 - _minute;
            time = time * 60 - _second;
            create_notify(game_data._instance.get_t_language("MtaU3D.cs_106_17"), game_data._instance.get_t_language("MtaU3D.cs_106_17到了，赶快上线领取50体力吧"), game_data._instance.get_t_language("MtaU3D.cs_106_17到了，赶快上线领取50体力吧"), time);//晚餐补给//MtaU3D.cs_106_25//晚餐补给到了，赶快上线领取50体力吧
        }
    }

    public virtual void create_notify(string title, string text, string ticker, int secondsFromNow)
    {
        if (Application.isEditor)
        {
            return;
        }
        AndroidJavaClass javaClass = new AndroidJavaClass("com.yymoon.tool.AlarmReceiver");
        javaClass.CallStatic("startAlarm", new object[4] { title, text, ticker, secondsFromNow });
    }

    public virtual void cancel_notify()
    {
        if (Application.isEditor)
        {
            return;
        }
        AndroidJavaClass javaClass = new AndroidJavaClass("com.yymoon.tool.AlarmReceiver");
        javaClass.CallStatic("clearNotification");
    }

    public virtual void copy(string text)
    {
        if (Application.isEditor)
        {
            return;
        }
        AndroidJavaClass javaClass = new AndroidJavaClass("com.yymoon.tool.ClipboardTools");
        javaClass.CallStatic("copyTextToClipboard");
    }

    public virtual void do_save_photo_platform(string name, string message)
    {
        if (message != "")
        {
            s_message msg = new s_message();
            msg.m_type = message;
            cmessage_center._instance.add_message(msg);
        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////

    public virtual void game_login()
    {
        if (Application.isEditor)
        {
            return;
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("game_login");
    }

    public virtual void platform_login_success(string s)
    {

    }

    public virtual void game_logout()
    {
        if (Application.isEditor)
        {
            return;
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("game_logout");
    }

    public virtual void platfrom_logout_success(string s)
    {
        sys._instance.game_logout();
    }

    public virtual void on_game_login(int is_new)
    {
        if (Application.isEditor)
        {
            return;
        }
        string param = sys._instance.m_self.m_t_player.guid + "_" + sys._instance.m_self.m_t_player.name + "_" + sys._instance.m_self.m_t_player.level + "_" + sys._instance.m_self.m_t_player.serverid + "_" + sys._instance.m_sname;
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("on_game_login", param, is_new);
        if(analytics_management.m_onGameLogin != null)
        {
            analytics_management.m_onGameLogin(is_new);
        }
        
    }

    public virtual void on_game_user_upgrade(int level)
    {
        if (Application.isEditor)
        {
            return;
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("on_game_user_upgrade", level);
    }

    public virtual void on_charge_request(string orderId, string iapId, double currencyAmount, double virtualCurrencyAmount)
    {
        if (Application.isEditor)
        {
            return;
        }
        if (analytics_management.m_chargeRequst != null)
        {
            analytics_management.m_chargeRequst(orderId, iapId, currencyAmount, virtualCurrencyAmount);
        }
        
    }

    public virtual void on_charge_success(string orderId)
    {
        if (Application.isEditor)
        {
            return;
        }
        if(analytics_management.m_chargeSuccess != null)
        {
            analytics_management.m_chargeSuccess(orderId);
        }
        
    }

    public virtual void game_update()
    {
        if (Application.isEditor)
        {
            return;
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("game_update", platform_config_common.m_open_market, platform_config_common.m_open_url);
    }

    public virtual int get_language()
    {
        if (Application.isEditor)
        {
            return 0;
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        return jo.Call<int>("get_language");
    }

    public virtual string get_bundle_name()
    {
        if (Application.isEditor)
        {
            return "";
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        return jo.Call<string>("get_bundle_name");
    }

    public virtual string get_platform_id()
    {
        if (Application.isEditor)
        {
            return "";
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        return jo.Call<string>("get_platform_id");
    }

    public virtual string get_platform_isbn()
    {
        return "游戏名称：女神星球  著作权人：上海银月网络科技有限公司\n文网游备字[2016]M-3075号 游戏版号：ISBN 978-7-7979-5341-2 审批文号：新广出审[2017]8325号\n出版方:上海银月科技有限公司 运营商：上海银月科技有限公司";
    }

    /// <summary>
    /// 特殊用
    /// </summary>

    public virtual void scene_loaded()
    {

    }

    public virtual void deal_cam(string name, Camera cam)
    {

    }
}
