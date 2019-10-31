using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class analytics_management  {
    public delegate void DelegategoldGet(string info, int gold);
    public static DelegategoldGet m_goldGet;

    public delegate void DelegategoldConsume(string info, int gold);
    public static DelegategoldConsume m_goldConsume;

    public delegate void DelegatejewelGet(string info, int jewel);
    public static DelegatejewelGet m_jewelGet;

    public delegate void DelegatejewelConsume(string info, int jewel);
    public static DelegatejewelConsume m_jewelConsume;

    public delegate void DelegateitemGet(string info, string item, int num);
    public static DelegateitemGet m_itemGet;

    public delegate void DelegateChargeRequst(string orderId, string iapId, double currencyAmount, double virtualCurrencyAmount);
    public static DelegateChargeRequst m_chargeRequst;

    public delegate void DelegateChargeSuccess(string orderId);
    public static DelegateChargeSuccess m_chargeSuccess;

    public delegate void DelegateonGameRegister();
    public static DelegateonGameRegister m_onGameRegister;

    public delegate void DelegateonGameLogin(int is_new);
    public static DelegateonGameLogin m_onGameLogin;

    public delegate void DelegateonGameUserUpgrade(int level);
    public static DelegateonGameUserUpgrade m_onGameUserUpgrade;

    public delegate void DelegateonGameRecharge(string scene, int num, int getgold, int holdgold);
    public static DelegateonGameRecharge m_onGameRecharge;

    public delegate void DelegateitemConsume(string info, string item, int num);
    public static DelegateitemConsume m_itemConsume;

    public delegate void DelegateOnClick_ads();
    public static DelegateOnClick_ads m_OnClick_ads;

    public delegate void DelegateOnOpen_event();
    public static DelegateOnOpen_event m_OnOpen_event;

    public delegate void DelegateOnBuy_event();
    public static DelegateOnBuy_event m_OnBuy_event;
    
    public static void analytics_config()
    {
#if APPSFLYER
        m_goldGet += Appsflyer_management._instance.goldGet;
        m_goldConsume += Appsflyer_management._instance.goldConsume;
        m_jewelGet += Appsflyer_management._instance.jewelGet;
        m_jewelConsume += Appsflyer_management._instance.jewelConsume;  
        m_itemGet += Appsflyer_management._instance.itemGet;    
        m_chargeRequst += Appsflyer_management._instance.ChargeRequst;
        m_chargeSuccess += Appsflyer_management._instance.ChargeSuccess;
        m_onGameRegister += Appsflyer_management._instance.onGameRegister;
        m_onGameLogin += Appsflyer_management._instance.onGameLogin;
        m_onGameUserUpgrade += Appsflyer_management._instance.onGameUserUpgrade;
        m_onGameRecharge += Appsflyer_management._instance.onGameRecharge;
        m_itemConsume += Appsflyer_management._instance.itemConsume;
        m_OnClick_ads += Appsflyer_management._instance.OnClick_ads;
        m_OnOpen_event += Appsflyer_management._instance.OnOpen_event;
        m_OnBuy_event += Appsflyer_management._instance.OnBuy_event;
#endif

#if FACEBOOK
        m_goldGet += facebook_management._instance.goldGet;
        m_goldConsume += facebook_management._instance.goldConsume;
        m_jewelGet += facebook_management._instance.jewelGet;
        m_jewelConsume += facebook_management._instance.jewelConsume;  
        m_itemGet += facebook_management._instance.itemGet;    
        m_chargeRequst += facebook_management._instance.ChargeRequst;
        m_chargeSuccess += facebook_management._instance.ChargeSuccess;
        m_onGameRegister += facebook_management._instance.onGameRegister;
        m_onGameLogin += facebook_management._instance.onGameLogin;
        m_onGameUserUpgrade += facebook_management._instance.onGameUserUpgrade;
        m_onGameRecharge += facebook_management._instance.onGameRecharge;
        m_itemConsume += facebook_management._instance.itemConsume;
        m_OnClick_ads += facebook_management._instance.OnClick_ads;
        m_OnOpen_event += facebook_management._instance.OnOpen_event;
        m_OnBuy_event += facebook_management._instance.OnBuy_event;

#endif
    }   
}
