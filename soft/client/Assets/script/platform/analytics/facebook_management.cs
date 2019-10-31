using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if FACEBOOK
using Facebook.Unity;
public class facebook_management : MonoBehaviour,Ianalytics {

    public static facebook_management _instance;

    void Awake()
    {
        _instance = this;
       
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
        else
        {
            //Handle FB.Init
            FB.Init(() => {
                FB.ActivateApp();
            });
        }
    }

    void OnApplicationPause(bool pauseStatus)
    {
        // Check the pauseStatus to see if we are in the foreground
        // or background
        if (!pauseStatus)
        {
            //app resume
            if (FB.IsInitialized)
            {
                FB.ActivateApp();
            }
            else
            {
                //Handle FB.Init
                FB.Init(() => {
                    FB.ActivateApp();
                });
            }
        }
    }

    public void ChargeRequst(string orderId, string iapId, double currencyAmount, double virtualCurrencyAmount)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["currencyAmount"] = currencyAmount;
        parameters["orderId"] = orderId;
        parameters["jewel"] = virtualCurrencyAmount;
        float valueToSum = 0;
        FB.LogAppEvent(
            "Recharge",
            valueToSum,
            parameters
        );
    }

    public void ChargeSuccess(string orderId)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["orderId"] = orderId;
        float valueToSum = 0;
        FB.LogAppEvent(
            "RechargeSuccess",
            valueToSum,
            parameters
        );
    }

    public void goldConsume(string info, int gold)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["info"] = info;
        parameters["gold"] = gold;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();        
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        float valueToSum = 0;
        FB.LogAppEvent(
            "goldConsume",
            valueToSum,
            parameters
        );
    }

    public void goldGet(string info, int gold)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["info"] = info;
        parameters["gold"] = gold;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();       
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        float valueToSum = 0;
        FB.LogAppEvent(
            "goldGet",
            valueToSum,
            parameters
        );
    }

    public void itemGet(string info, string item, int num)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["info"] = info;
        parameters["item"] = item;
        parameters["num"] = num;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        float valueToSum = 0;
        FB.LogAppEvent(
            "itemGet",
            valueToSum,
            parameters
        );
    }

    public void itemConsume(string info, string item, int num)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["info"] = info;
        parameters["item"] = item;
        parameters["num"] = num;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        float valueToSum = 0;
        FB.LogAppEvent(
            "itemConsume",
            valueToSum,
            parameters
        );
    }

    public void jewelConsume(string info, int jewel)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["info"] = info;
        parameters["jewel"] = jewel;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        float valueToSum = 0;
        FB.LogAppEvent(
            "jewelConsume",
            valueToSum,
            parameters
        );
    }

    public void jewelGet(string info, int jewel)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["info"] = info;
        parameters["jewel"] = jewel;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        float valueToSum = 0;
        FB.LogAppEvent(
            "jewelGet",
            valueToSum,
            parameters
        );
    }

    

    public void onGameLogin(int is_new)
    {
        
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["isnew 1or0 1new"] = is_new;
        parameters["server_name"] = sys._instance.m_sname;
        parameters["player_name"] = sys._instance.m_self.m_t_player.name + "_" + game_data._instance.m_oldusername;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        float valueToSum = 0;
        FB.LogAppEvent(
            "onGameLogin",
            valueToSum,
            parameters
        );
    }

    public void onGameRecharge(string scene, int num, int getgold, int holdgold)
    {

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["scene"] = scene;
        parameters["server_name"] = sys._instance.m_sname;
        parameters["player_name"] = sys._instance.m_self.m_t_player.name + "_" + game_data._instance.m_oldusername;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        parameters["num"] = num;
        parameters["getgold"] = getgold;
        parameters["holdgold"] = holdgold;

        float valueToSum = 0;
        FB.LogAppEvent(
            "onGameRecharge",
            valueToSum,
            parameters
        );
    }

    public void onGameRegister()
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["name"] = game_data._instance.m_player_data.m_name;
        float valueToSum = 1;
        FB.LogAppEvent(
            "onGameRegister",
            valueToSum,
            parameters
        );
    }

    public void onGameUserUpgrade(int level)
    {
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters["level"] = level;
        parameters["guid"] = sys._instance.m_self.m_guid.ToString();
        parameters["server_id"] = sys._instance.m_sid;
        parameters["player_level"] = sys._instance.m_self.m_t_player.level;
        float valueToSum = 0;
        FB.LogAppEvent(
            "onGameUserUpgrade",
            valueToSum,
            parameters
        );
    }

    public void OnOpen_event()
    {
        
    }
    public void OnBuy_event()
    {

    }

    public void OnClick_ads()
    {

    }
}
#endif