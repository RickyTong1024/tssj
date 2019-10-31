//
// Source code recreated from a .class file by IntelliJ IDEA
// (powered by Fernflower decompiler)
//

package com.wjhe.tsjh24.alipay;

import android.annotation.SuppressLint;
import android.os.Handler;
import android.os.Message;
import android.text.TextUtils;
import android.util.Log;
import com.alipay.sdk.app.PayTask;
import com.unity3d.player.UnityPlayer;
import com.wjhe.tsjh24.MainActivity;
import net.sourceforge.simcpux.Util;


import java.util.Map;

public class Alipay {
    private MainActivity m_act;
    String m_appid = "";
    static Map<String, String> params = null;
    String orderParam = "";

    @SuppressLint({"HandlerLeak"})
    private Handler mHandler;

    public void init(MainActivity act, String app_id) {
        m_act = act;
        m_appid = app_id;
        this.mHandler = new Handler(UnityPlayer.currentActivity.getMainLooper()) {
            public void handleMessage(Message msg) {
                switch(msg.what) {
                    case 1:
                        AuthResult authResult = new AuthResult((Map)msg.obj, true);
                        String resultStatus = authResult.getResultStatus();
                        Log.e("unity", "handleMessage: "+resultStatus );
                        if(TextUtils.equals(resultStatus, "9000")) {
                            m_act.pay_done("");
                        } else {
                            m_act.pay_cancel("");
                        }
                        break;
                    default:
                        break;
                }
            }
        };
    }

    public void do_alipay(String body, String subject, String price, String sign_url,String notify_url)
    {
        params = OrderInfoUtil2_0.buildOrderParamMap(m_appid, body, subject, price,notify_url);
        Log.d("unity", "do_alipay_params: "+params);
        this.orderParam = OrderInfoUtil2_0.buildOrderParam(params, false);
        Log.d("unity", "do_alipay_oderParam: "+orderParam);
        do_alisign(this.orderParam, sign_url);
    }

    public void do_alisign(String param, String recharge_url)
    {
        String sign = Util.httpPost(recharge_url, param);
        if (sign==null||sign.equals(""))
        {
            m_act.toastMes("请求失败");
            m_act.pay_cancel("");
            return;
        }
        Log.d("unity", "sign:"+sign);
        do_alipay1(sign);
    }

    public void do_alipay1(final String sign) {
        Runnable payRunnable = new Runnable() {
            public void run() {
                PayTask pay_task = new PayTask(UnityPlayer.currentActivity);
                Alipay.this.orderParam = OrderInfoUtil2_0.buildOrderParam(Alipay.params, true);
                String orderInfo = Alipay.this.orderParam + "&sign=" + sign;
                Log.d("unity", "orderInfo:"+orderInfo);
                Map<String, String> result = pay_task.payV2(orderInfo, true);
                Log.d("unity", "result:"+result);
                Message msg = new Message();
                msg.what = 1;
                msg.obj = result;
                Alipay.this.mHandler.sendMessage(msg);
            }
        };
        Thread payThread = new Thread(payRunnable);
        payThread.start();
    }
}
