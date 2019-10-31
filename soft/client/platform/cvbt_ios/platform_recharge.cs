using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;


public class platform_recharge :platform_recharge_common {

    [DllImport("__Internal")]
    private static extern void platform_buy(string body);

    private static platform_recharge _platform_recharge;
    string m_code;
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
        m_ios_hd_mid = huodongMid;
        m_ios_hd_eid = huodongEid;
        s_t_recharge t_recharge = game_data._instance.get_t_recharge(id);
        root_gui._instance.wait(true);
        string desc = "";
        if (t_recharge.desc == null)
        {
            desc = "null";
        }
        else
        {
            desc = t_recharge.desc;
        }
        string body = game_data._instance.m_player_data.m_token + "|" + sys._instance.m_self.m_t_player.serverid + "|" + t_recharge.id + "|" + t_recharge.ios_id + "|" + huodongMid + "|" + huodongEid + "|" + platform_czbl(t_recharge.vippt) + "|" + t_recharge.name + "|" + desc;
        platform_buy(body);
    }
	
	int platform_czbl(int jewel)
    {
        return jewel / game_data._instance.get_const_vale((int)opclient_t.CONST_CZJH);
    }
	
    public override void recharge_done(string s)
    {
        root_gui._instance.wait(true);
        do_check();
    }

    public override void recharge_cancel(string s)
    {
        root_gui._instance.wait(false);
    }

    public override void recharge_onOderNo(string s)
    {
        root_gui._instance.wait(false);
    }
}
