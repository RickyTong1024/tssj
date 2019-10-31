using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class platform_recharge : platform_recharge_common
{
    private static platform_recharge _platform_recharge;
    int sku = 0;
    public static platform_recharge _instance
    {
        get
        {
            if (_platform_recharge == null)
            {
                _platform_recharge = new platform_recharge();
            }
            return _platform_recharge;
        }
    }

    protected override void buy(int id, int huodongMid = 0, int huodongEid = 0)
    {
        s_t_recharge t_recharge = game_data._instance.get_t_recharge(m_id);
        sku = t_recharge.ios_id;
        root_gui._instance.wait(true);
        string body= game_data._instance.m_player_data.m_token + "_" + sys._instance.m_self.m_t_player.serverid + "_" + t_recharge.id + "_" + sku + "_" + huodongMid + "_" + huodongEid;
        string price = (t_recharge.vippt / 10).ToString();
        string subject = t_recharge.name;
        string pay_url = platform_config_common.get_pay_url();
        string param = body + " " + subject +" "+ price + " " + pay_url;
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("game_pay", param);
    }

    public override void recharge_done(string s)
    {
        do_check();
    }

    public override void recharge_cancel(string s)
    {
        root_gui._instance.wait(false);
    }
}
