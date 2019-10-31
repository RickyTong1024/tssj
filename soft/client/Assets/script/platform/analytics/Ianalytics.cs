using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Ianalytics{

    void goldGet(string info, int gold);

    void goldConsume(string info, int gold);

    void jewelGet(string info, int jewel);

    void jewelConsume(string info, int jewel);
    
    void itemGet(string info, string item, int num);

    void ChargeRequst(string orderId, string iapId, double currencyAmount, double virtualCurrencyAmount);

    void ChargeSuccess(string orderId);

    void onGameRegister();

    void onGameLogin(int is_new);

    void onGameUserUpgrade(int level);

    void onGameRecharge(string scene, int num, int getgold, int holdgold);

    void itemConsume(string info, string item, int num);

    void OnClick_ads();

    void OnOpen_event();

    void OnBuy_event();
    
}
