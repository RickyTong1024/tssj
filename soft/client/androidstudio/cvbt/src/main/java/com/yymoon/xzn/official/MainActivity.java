package com.yymoon.xzn.official;


import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.res.Configuration;
import android.os.Bundle;

import android.util.Log;
import android.view.KeyEvent;
import com.sy.framework.SPluginWrapper;
import com.sy.framework.SYSDK;
import com.sy.framework.SYSDKListener;
import com.sy.framework.platform.SYSDKPlatform;
import com.unity3d.player.UnityPlayer;
import com.yymoon.game.GameActivity;
import java.util.HashMap;
import java.util.Map;


public class MainActivity extends GameActivity{
    public static String TAG = "Unity";
	String role_id;
	String role_level;
	String role_name;
	String role_server_id;
	String role_server_name;
    String cur_userId = null;
	@Override
	protected void onCreate(Bundle savedInstanceState)
    {
		super.onCreate(savedInstanceState);
        Sdkinit();
	}
	private void Sdkinit()
    {

        SYSDKPlatform.getInstance().setListener(new SYSDKListener()
        {
            @Override
            public void onCallBack(int action, Map<String, String> result)
            {
                String msg = null;
                switch (action)
                {
                    case SYSDKPlatform.ACTION_INIT_SUCCESS:
                        msg = "初始化成功回调";
                        break;
                    case SYSDKPlatform.ACTION_INIT_FAILURE:
                        msg = "初始化失败回调";
                        break;
                    case SYSDKPlatform.ACTION_LOGIN_SUCCESS:
                        msg = "登录成功回调";
                        if (cur_userId == null)
                        {
                            cur_userId = result.get("userId");
                            UnityPlayer.UnitySendMessage("game_node", "platform_login_success", result.get("userId") + " " + result.get("token") + " " +"0");
                        }
                        else{
                            cur_userId = result.get("userId");
                            UnityPlayer.UnitySendMessage("game_node", "platform_login_success", result.get("userId") + " " + result.get("token") + " " +"1");
                        }
                        SYSDKPlatform.getInstance().doAntiAddictionQuery();
                        break;
                    case SYSDKPlatform.ACTION_LOGIN_FAILURE:
                        msg = "登录失败回调";
                        UnityPlayer.UnitySendMessage("game_node", "platform_login_fail","");
                        break;
                    case SYSDKPlatform.ACTION_ACCOUNTSWITCH_LOGOUT_SUCCESS:
                        msg = "账号注销成功回调";
                        cur_userId = null;
                        UnityPlayer.UnitySendMessage("game_node","platform_logout","");
                        break;
                    case SYSDKPlatform.ACTION_ACCOUNTSWITCH_FAILURE:
                        msg = "帐号切换失败回调";
                        break;
                    case SYSDKPlatform.ACTION_PAY_SUCCESS:
                        msg = "支付成功回调";
                        UnityPlayer.UnitySendMessage("game_node","recharge_done","");
                        break;
                    case SYSDKPlatform.ACTION_PAY_FAILURE:
                        msg = "支付失败回调";
                        UnityPlayer.UnitySendMessage("game_node","recharge_cancel","");
                        break;
                    case SYSDKPlatform.ACTION_EXIT_FROM_PLATFORM:
                        msg = "第三方平台退出，请直接退出游戏";
                        exitGame();
                        break;
                    case SYSDKPlatform.ACTION_EXIT_FROM_GAME:
                        msg = "游戏自己退出，请调起自己的退出框";
                        show_exit_dialog();
                        break;
                    case SYSDKPlatform.ACTION_ANTI_ADDICTION_QUERY_SUCCESS:
                        msg = "防成谜查询成功回调";
                        break;
                    case SYSDKPlatform.ACTION_ANTI_ADDICTION_QUERY_FAILURE:
                        msg = "防成谜查询失败回调";
                        break;
                    default:
                        break;
                }
                Log.d(TAG, "msg:" + msg + "\t result:" + (null != result ? result.toString() : null));
            }
        });

        Map<String, String> appInfo = new HashMap<String, String>();
        appInfo.put("name", "x战娘");
        appInfo.put("shortName", "xzn");
        appInfo.put("direction", "0");//0 横屏 1 竖屏

        SYSDK.getInstance().setDebug(false); //测试模式
        SYSDK.getInstance().init(this, appInfo);
	}

	@Override
	public void game_pay(String body)
    {
        String[] bodys = body.split("\\|",-1);
        String player_id = bodys[0];
        String server_id = bodys[1];
        String recharge_id = bodys[2];
        String ios_id = bodys[3];
        String huodong_id = bodys[4];
        String entry_id = bodys[5];
        String recharge_price = bodys[6];
        String recharge_name = bodys[7];
        String recharge_desc = bodys[8];
        Log.d(TAG, recharge_name.split("\\+")[0]);
        String pay_cpUserInfo = player_id + "_" + server_id + "_" + recharge_id + "_" + huodong_id + "_" + entry_id;
        Map<String, String> payInfo = new HashMap<String, String>();
        payInfo.put("productId", ios_id);
        payInfo.put("productName", recharge_name.split("\\+")[0]);
        payInfo.put("productDesc", recharge_desc);
        payInfo.put("productPrice", recharge_price);
        payInfo.put("productCount", "1");
        payInfo.put("productType", "0");
        payInfo.put("coinName", "钻⽯");
        payInfo.put("coinRate", "500");
        payInfo.put("extendInfo", pay_cpUserInfo);
        payInfo.put("roleId", player_id);
        payInfo.put("roleName", role_name);
        payInfo.put("zoneId", server_id);
        payInfo.put("zoneName", role_server_name);
        payInfo.put("partyName", "无");
        payInfo.put("roleLevel", role_level);
        payInfo.put("roleVipLevel", "0");
        payInfo.put("balance", "0");
        SYSDKPlatform.getInstance().doPay(payInfo);
	}
	@Override
	public void game_logout()
    {
        SYSDKPlatform.getInstance().doAccountSwitch();
	}

	@Override
	public void game_login()
    {
        SYSDKPlatform.getInstance().doLogin();
	}

	@Override
	public  void on_game_login(String parma, int role_new)
    {
		String[] params = parma.split("_");
		role_id = params[0];
		role_name = params[1];
		role_level = params[2];
		role_server_id = params[3];
		role_server_name = params[4];
        Map<String, String> roleInfo = new HashMap<String, String>();
        roleInfo.put("roleId", role_id);
        roleInfo.put("roleName", role_name);
        roleInfo.put("zoneId", role_server_id);
        roleInfo.put("zoneName", role_server_name);
        roleInfo.put("partyName", "无");
        roleInfo.put("roleLevel", role_level);
        roleInfo.put("roleVipLevel", "16");
        roleInfo.put("balance", "0");
		if(role_new == 1)
		{
            roleInfo.put("isNewRole", "1");
		}
		else
        {
            roleInfo.put("isNewRole", "0");
        }
        SYSDKPlatform.getInstance().setRoleInfo(roleInfo);
	}

	@Override
	public  void on_game_user_upgrade(int level)
    {
        role_level = level + "";
        SYSDKPlatform.getInstance().onRoleLevelUpgrade(level);
	}

    @Override
    public boolean onKeyDown(int keyCode, KeyEvent event)
    {
        // TODO Auto-generated method stub
        if (KeyEvent.KEYCODE_BACK == keyCode)
        {
            SYSDKPlatform.getInstance().doExit();
            return true;
        }
        return super.onKeyDown(keyCode, event);
    }
    private void show_exit_dialog() {
        final AlertDialog.Builder normalDialog = new AlertDialog.Builder(this);
        normalDialog.setTitle("提示");
        normalDialog.setMessage("是否退出游戏！");
        normalDialog.setPositiveButton("确定",
                new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                        exitGame();
                    }
                });
        normalDialog.setNegativeButton("取消",
                new DialogInterface.OnClickListener() {
                    @Override
                    public void onClick(DialogInterface dialog, int which) {
                    }
                });
        normalDialog.show();
    }

    private void exitGame()
    {
        SYSDK.getInstance().release();
        this.finish();
        System.exit(0);
    }

    @Override
    protected void onStart()
    {
        super.onStart();
        SPluginWrapper.onStart();
    }

    @Override
    protected void onPause()
    {
        super.onPause();
        SPluginWrapper.onPause();
    }

    @Override
    protected void onResume()
    {
        super.onResume();
        SPluginWrapper.onResume();
    }

    @Override
    protected void onStop()
    {
        super.onStop();
        SPluginWrapper.onStop();
    }

    @Override
    protected void onRestart()
    {
        super.onRestart();
        SPluginWrapper.onRestart();
    }

    @Override
    protected void onDestroy()
    {
        super.onDestroy();
        SPluginWrapper.onDestroy();
    }

    @Override
    protected void onNewIntent(Intent intent)
    {
        super.onNewIntent(intent);
        SPluginWrapper.onNewIntent(intent);
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data)
    {
        super.onActivityResult(requestCode, resultCode, data);
        SPluginWrapper.onActivityResult(requestCode, resultCode, data);
    }

    @Override
    public void onConfigurationChanged(Configuration newConfig)
    {
        super.onConfigurationChanged(newConfig);
        SPluginWrapper.onConfigurationChanged(newConfig);
    }

    @Override
    public void onWindowFocusChanged(boolean hasFocus)
    {
        super.onWindowFocusChanged(hasFocus);
        SPluginWrapper.onWindowFocusChanged(hasFocus);
    }

    @Override
    protected void onSaveInstanceState(Bundle outState)
    {
        super.onSaveInstanceState(outState);
        SPluginWrapper.onSaveInstanceState(outState);
    }

    @Override
    public void onBackPressed()
    {
        super.onBackPressed();
        SPluginWrapper.onBackPressed();
    }
}
