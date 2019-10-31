using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if APPSFLYER
public class Appsflyer_management : MonoBehaviour,Ianalytics {

    public static Appsflyer_management _instance;

    public string APP_ID = "1342549729";
    public string APPSFLYER_DEV_KEY = "ryDMeYCsxuVUT654DVrnnD";

    public void ChargeRequst(string orderId, string iapId, double currencyAmount, double virtualCurrencyAmount)
    {
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("currencyAmount", currencyAmount.ToString());
        purchaseEvent.Add("orderId", orderId);
        purchaseEvent.Add("jewel", virtualCurrencyAmount.ToString());
        AppsFlyer.trackRichEvent("Recharge", purchaseEvent);

        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        eventValue.Add("af_revenue", currencyAmount.ToString());
        eventValue.Add("orderId", orderId);
        eventValue.Add("jewel", virtualCurrencyAmount.ToString());
        eventValue.Add("af_currency", "CNY");
        AppsFlyer.trackRichEvent(AFInAppEvents.PURCHASE, eventValue);
    }

    public void ChargeSuccess(string orderId)
    {
        
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("orderId", orderId);
        AppsFlyer.trackRichEvent("RechargeSuccess", purchaseEvent);
    }

    public void goldConsume(string info, int gold)
    {
       
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("info", info);
        purchaseEvent.Add("gold", gold.ToString());
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        AppsFlyer.trackRichEvent("goldConsume", purchaseEvent);
    }

    public void goldGet(string info, int gold)
    {
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("info", info);
        purchaseEvent.Add("gold", gold.ToString());
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        AppsFlyer.trackRichEvent("goldGet", purchaseEvent);
    }

    public void itemConsume(string info, string item, int num)
    {

        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("info", info);
        purchaseEvent.Add("item", item);
        purchaseEvent.Add("num", num.ToString());
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        AppsFlyer.trackRichEvent("itemConsume", purchaseEvent);
    }

    public void itemGet(string info, string item, int num)
    {
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("info", info);
        purchaseEvent.Add("item", item);
        purchaseEvent.Add("num", num.ToString());
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        AppsFlyer.trackRichEvent("itemGet", purchaseEvent);
    }

    public void jewelConsume(string info, int jewel)
    {
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("info", info);
        purchaseEvent.Add("jewel", jewel.ToString());
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        AppsFlyer.trackRichEvent("jewelConsume", purchaseEvent);
    }

    public void jewelGet(string info, int jewel)
    {
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("info", info);
        purchaseEvent.Add("jewel", jewel.ToString());
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        AppsFlyer.trackRichEvent("jewelGet", purchaseEvent);
    }

    public void OnBuy_event()
    {
       
    }

    public void OnClick_ads()
    {
        
    }

    public void onGameLogin(int is_new)
    {
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("isnew 1or0 1new", is_new.ToString());
        purchaseEvent.Add("server_name", sys._instance.m_sname);
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("player_name", sys._instance.m_self.m_t_player.name + "_" + game_data._instance.m_player_data.m_token);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        AppsFlyer.trackRichEvent("onGameLogin", purchaseEvent);
    }

    public void onGameRecharge(string scene, int num, int getgold, int holdgold)
    {
       
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("scene", scene);
        purchaseEvent.Add("server_name", sys._instance.m_sname);
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("player_name", sys._instance.m_self.m_t_player.name + "_" + game_data._instance.m_player_data.m_token);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        purchaseEvent.Add("num", num.ToString());
        purchaseEvent.Add("getgold", getgold.ToString());
        purchaseEvent.Add("holdgold", holdgold.ToString());
        AppsFlyer.trackRichEvent("onGameRecharge", purchaseEvent);
    }

    public void onGameRegister()
    {
       

        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("name", game_data._instance.m_player_data.m_token);
        AppsFlyer.trackRichEvent("onGameRegister", purchaseEvent);
    }

    public void onGameUserUpgrade(int level)
    {
        
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add("level", level.ToString());
        purchaseEvent.Add("guid", sys._instance.m_self.m_guid.ToString());
        purchaseEvent.Add("server_id", sys._instance.m_sid);
        purchaseEvent.Add("player_level", sys._instance.m_self.m_t_player.level.ToString());
        AppsFlyer.trackRichEvent("onGameUserUpgrade", purchaseEvent);

    }

    public void OnOpen_event()
    {
        
    }
    void Awake()
    {
        _instance = this;
    }
    // Use this for initialization
    void Start () {

#if UNITY_IPHONE
       AppsFlyer.setAppsFlyerKey(APPSFLYER_DEV_KEY);
       AppsFlyer.setAppID (APP_ID);
       AppsFlyer.trackAppLaunch ();
#elif UNITY_ANDROID
       AppsFlyer.setAppID (Application.identifier);
       AppsFlyer.init (APPSFLYER_DEV_KEY, "AppsFlyerTrackerCallbacks");
#endif
    }

    // Update is called once per frame
    void Update () {
		
	}
}
#endif
