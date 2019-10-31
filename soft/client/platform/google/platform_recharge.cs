using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class platform_recharge : platform_recharge_common
{
    private static platform_recharge _platform_recharge;
    string m_google_token = "";
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

    protected override void buy(int id, int huodongMid, int huodongEid)
    {
        s_t_recharge t_recharge = game_data._instance.get_t_recharge(m_id);
        sku = t_recharge.ios_id;
        root_gui._instance.wait(true);
        string param = game_data._instance.m_player_data.m_token + "_" + sys._instance.m_self.m_t_player.serverid + "_" + t_recharge.id + "_" + sku + "_" + huodongMid + "_" + huodongEid;
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        jo.Call("game_pay", param);
    }

    public override void recharge_done(string s)
    {
        if (m_google_token == s)
        {
            return;
        }
        Debug.Log("recharge_done google: " + s);
        m_google_token = s;
        do_check_recharge();
    }

    private void do_check_recharge()
    {
        platform_recharge_object._instance.StopAllCoroutines();
        platform_recharge_object._instance.StartCoroutine(check_recharge());
    }

    IEnumerator check_recharge()
    {
        WWWForm _form = new WWWForm();
        _form.AddField("packageName", Application.identifier);
        _form.AddField("productId", sku);
        _form.AddField("purchase_token", m_google_token);
        
        WWW _www = new WWW(platform_config_common.get_pay_url() + "notifygoogle", _form);
        yield return _www;

        if (_www.error == null)
        {
            Debug.Log(_www.text);
            root_gui._instance.wait(false);
            if (_www.text.Contains("success"))
            {
                do_check();
            }
            else
            {
                root_gui._instance.show_prompt_dialog_box(game_data._instance.get_t_language("recharge_iphone_yymoon.cs_168_47"));//[ffc882]支付失败，请尝试重新登录或者联系客服
            }

        }
        else
        {
            Debug.Log(_www.error);
            do_check_recharge();
        }
    }

    public override void recharge_cancel(string s)
    {
        Debug.Log("recharge_cancel : " + s);
        root_gui._instance.wait(false);
    }
}
