using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.Purchasing;
using System;

public class platform_recharge :platform_recharge_common ,IStoreListener {

    private IStoreController controller;
    private IExtensionProvider extensions;
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

    public override void init()
    {
        if (Application.isEditor)
        {
            //return;
        }
        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        dbc m_dbc_recharge = game_data._instance.m_dbc_recharge;
        List<string> vs = new List<string>();
        for (int i = 0; i < m_dbc_recharge.get_y(); ++i)
        {
            int id = int.Parse(m_dbc_recharge.get(0, i));
            s_t_recharge t_recharge = game_data._instance.get_t_recharge(id);

            if(!vs.Contains(t_recharge.ios_id.ToString()))
            {
                vs.Add(t_recharge.ios_id.ToString());
                builder.AddProduct(t_recharge.ios_id.ToString(), ProductType.Consumable, new IDs{{t_recharge.ios_id.ToString(), AppleAppStore.Name}});
            }

        }
        
        UnityPurchasing.Initialize(this, builder);
    }

    protected override void buy(int id, int huodongMid = 0, int huodongEid = 0)
    {
        m_ios_hd_mid = huodongMid;
        m_ios_hd_eid = huodongEid;
        s_t_recharge t_recharge = game_data._instance.get_t_recharge(id);
        root_gui._instance.wait(true);
        controller.InitiatePurchase(t_recharge.ios_id.ToString());
    }

    IEnumerator check_recharge()
    {
        WWWForm _form = new WWWForm();

        _form.AddField("username", game_data._instance.m_player_data.m_token);
        _form.AddField("serverid", sys._instance.m_sid);
        _form.AddField("rid", m_id);
        _form.AddField("code", m_code);
        _form.AddField("huodong_id", m_ios_hd_mid);
        _form.AddField("entry_id", m_ios_hd_eid);
        WWW _www = new WWW(platform_config_common.get_pay_url() + "notifyapple", _form);

        yield return _www;
        if (_www.error == null)
        {
            Debug.Log(_www.text);
            root_gui._instance.wait(false);
            if (_www.text.Contains("success"))
            {
                root_gui._instance.wait(true);
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
            root_gui._instance.wait(false);
        }
    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("OnInitialized");
        this.controller = controller;
        this.extensions = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("OnInitializeFailed" + error.ToString());
    }

    public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
    {
        Debug.Log(i.ToString());
        Debug.Log("PurchaseFailureReason :" + p.ToString());
        root_gui._instance.wait(false);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("ProcessPurchase");
        if (m_code == extensions.GetExtension<IAppleExtensions>().GetTransactionReceiptForProduct(e.purchasedProduct))
        {
            return PurchaseProcessingResult.Complete;
        }
        m_code = extensions.GetExtension<IAppleExtensions>().GetTransactionReceiptForProduct(e.purchasedProduct);
        platform_recharge_object._instance.StartCoroutine(check_recharge());
        return PurchaseProcessingResult.Complete;
    }
}
