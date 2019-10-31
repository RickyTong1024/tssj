package com.yymoon.xzn.official;

import android.Manifest;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageInfo;
import android.content.pm.PackageManager;
import android.content.res.Configuration;
import android.os.Build;
import android.os.Bundle;
import android.support.v4.content.PermissionChecker;
import android.util.Log;
import android.view.KeyEvent;
import com.snowfish.cn.ganga.base.IUtils;
import com.snowfish.cn.ganga.helper.SFOnlineExitListener;
import com.snowfish.cn.ganga.helper.SFOnlineHelper;
import com.snowfish.cn.ganga.helper.SFOnlineLoginListener;
import com.snowfish.cn.ganga.helper.SFOnlinePayResultListener;
import com.snowfish.cn.ganga.helper.SFOnlineUser;
import com.unity3d.player.UnityPlayer;
import com.yymoon.game.GameActivity;
import org.json.JSONException;
import org.json.JSONObject;

public class MainActivity extends GameActivity  {
	static final String Y_TAG="sfwarning";
	static final int REQUEST_READ_PHONE_STATE=1;
	String role_id;
	String role_level;
	String role_name;
	String role_server_id;
	String role_server_name;
	String cur_channelId=null;
	String cur_userId=null;

	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		cur_channelId=get_platform_id();
		premission_prepared();
		Log.e(Y_TAG, "MainActivity onCreate");
		this.Sdkinit();
	}

	private void premission_prepared(){ //一些渠道需要动态获取一些权限
		if(cur_channelId.equals("A15DC579667D6DA6")){  //百度渠道要动态获取访问手机信息权限
			Log.e(Y_TAG, "premission_prepared: "+"百度渠道");
			requestPremission(this, Manifest.permission.READ_PHONE_STATE);
		} else if(cur_channelId.equals("C6B5708195B3725C")){  //聚乐渠道要动态获取访问手机信息权限
			Log.e(Y_TAG, "premission_prepared: "+"聚乐渠道");
			requestPremission(this, Manifest.permission.READ_PHONE_STATE);
		}else if(cur_channelId.equals("07441320EB983FC6")){  //聚乐渠道要动态获取访问手机信息权限
			Log.e(Y_TAG, "premission_prepared: "+"夜神渠道");
			requestPremission(this, Manifest.permission.READ_PHONE_STATE);
		}else if(cur_channelId.equals("6EDC1D7C654416C7")){  //游戏fan渠道要动态获取访问手机信息权限
			Log.e(Y_TAG, "premission_prepared: "+"游戏fan渠道");
			requestPremission(this, Manifest.permission.READ_PHONE_STATE);
		}

	}

	private  void requestPremission(Context context, String permission){
		boolean result = true;
		Log.e(Y_TAG, "Build.VERSION.SDK_INT: "+ Build.VERSION.SDK_INT);
		if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
			Log.e(Y_TAG, "getTargetSdkVersion: "+getTargetSdkVersion(context));
			if (getTargetSdkVersion(context) >= Build.VERSION_CODES.M) {
				Log.e(Y_TAG, "context.checkSelfPermission ");
				result = context.checkSelfPermission(permission) == PackageManager.PERMISSION_GRANTED;
			} else {
				Log.e(Y_TAG, "PermissionChecker.checkSelfPermission");
				result = PermissionChecker.checkSelfPermission(context, permission) == PermissionChecker.PERMISSION_GRANTED;
			}
			Log.e(Y_TAG, "permission: "+permission+"---"+result);
			if (!result){
				this.requestPermissions(new String[]{permission}, REQUEST_READ_PHONE_STATE);
			}
		}
	}

	private int getTargetSdkVersion(Context context) {
		int version = 0;
		try {
			final PackageInfo info = context.getPackageManager().getPackageInfo(
					context.getPackageName(), 0);
			version = info.applicationInfo.targetSdkVersion;
		} catch (PackageManager.NameNotFoundException e) {
			e.printStackTrace();
		}
		return version;
	}

	private void Sdkinit() {
		SFOnlineHelper.onCreate(this);
		SFOnlineHelper.setLoginListener(this , new SFOnlineLoginListener() {
			@Override
			public void onLoginSuccess(SFOnlineUser user, Object customParams)
			{
				if (cur_userId==null){
					cur_userId=user.getChannelUserId();
					UnityPlayer.UnitySendMessage("game_node", "platform_login_success", cur_channelId + "_"+ user.getChannelUserId() + " " + user.getToken()+" "+"0");
					Log.e(Y_TAG, "登入成功,内容是：  "+cur_channelId + "_"+cur_userId + " " + user.getToken()+" "+"平台之前未登入");
				}
				else{
					cur_userId=user.getChannelUserId();
					UnityPlayer.UnitySendMessage("game_node", "platform_login_success", cur_channelId + "_"+ user.getChannelUserId() + " " + user.getToken()+" "+"1");
					Log.e(Y_TAG, "登入成功,内容是：  "+cur_channelId + "_"+cur_userId + " " + user.getToken()+" "+"平台之前已登入");
				}

			}
			@Override
			public void onLoginFailed(String reason, Object customParams) {
				UnityPlayer.UnitySendMessage("game_node", "platform_login_fail","");
				//登陆失败回调
				Log.e(Y_TAG, "onLoginFailed: "+"登入失败" );
			}
			@Override
			public void onLogout(Object customParams) {
				//登出回调
				UnityPlayer.UnitySendMessage("game_node","platform_logout","");
				cur_userId=null;
				Log.e(Y_TAG, "onLogout: "+"平台登出游戏" );
			}
		});
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data) {
		// TODO Auto-generated method stub
		super.onActivityResult(requestCode, resultCode, data);
		SFOnlineHelper.onActivityResult(this, requestCode, resultCode, data);
	}

	@Override
	public void game_pay(String body) {
		Log.e(Y_TAG, "body: "+body);
		String[] bodys = body.split("\\|",-1);
		String player_id = bodys[0];
		String server_id = bodys[1];
		String recharge_id = bodys[2];
		String ios_id= bodys[3];
		String huodong_id = bodys[4];
		String entry_id = bodys[5];
		String recharge_price = bodys[6];
		String recharge_name = bodys[7];
		String recharge_desc=bodys[8];
		String pay_cpUserInfo = player_id + "_" + server_id + "_" + recharge_id + "_" + huodong_id + "_" + entry_id;
		Log.e(Y_TAG, "pay_cpUserInfo: "+pay_cpUserInfo);
		int p=Integer.parseInt(recharge_price);
		String rechName=p/10+"diamonds";
		SFOnlineHelper.pay(MainActivity.this ,p, rechName, 1, pay_cpUserInfo,"http://47.75.230.46:10002/notifyyijie",
				new SFOnlinePayResultListener()
				{
					@Override
					public void onFailed(String remain)
					{
						Log.e(Y_TAG,"onFailed");
						UnityPlayer.UnitySendMessage("game_node","recharge_cancel","");
					}
					@Override
					public void onSuccess(String remain)
					{
						Log.e(Y_TAG,"onSuccess");
						UnityPlayer.UnitySendMessage("game_node","recharge_done","");
					}
					@Override
					public void onOderNo(String remain)
					{
						Log.e(Y_TAG,"onOderNo");
						UnityPlayer.UnitySendMessage("game_node","recharge_onOderNo","");
					}
				});
	}

	@Override
	public void game_logout() {
		logout_prepared();
		game_logout_yijie();
	}

	private void logout_prepared() {  //一些渠道有登出登陆的一些准备
		switch (cur_channelId)
		{
			case "E7FDED8015C8FD56":   //360渠道
				Log.e(Y_TAG, "logout_prepared: 360渠道登出setData");
				JSONObject shareinfo = new JSONObject();
				try {
					shareinfo.put("gamemoneyname", "Diamonds");
				} catch (JSONException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
				SFOnlineHelper.setData(this,"exitServer",shareinfo.toString());
			default:
				break;
		}
	}

	private void game_logout_yijie(){
		Log.e(Y_TAG, "game_logout: "+"游戏账号登出");
		SFOnlineHelper.logout(this, "LoginOut");
	}

	@Override
	public void game_login() {
		game_login_yijie();
	}

	private void game_login_yijie(){
		Log.e(Y_TAG, "game_login: "+"游戏账号登入");
		SFOnlineHelper.login(this, "Login");
	}

	@Override
	public  void on_game_login(String parma, int role_new) {
		String[] params = parma.split("_");
		role_id = params[0];
		role_name = params[1];
		role_level = params[2];
		role_server_id = params[3];
		role_server_name = params[4];
		JSONObject roleInfo = get_roleInfo(role_level);
		if(role_new == 1)
		{
			Log.e(Y_TAG, "是新玩家!!");
			SFOnlineHelper.setData(MainActivity.this,"createrole",roleInfo.toString());
		}
		Log.e(Y_TAG, "上传玩家数据!!");
		SFOnlineHelper.setRoleData(this, role_id, role_name , role_level, role_server_id, role_server_name);
		SFOnlineHelper.setData(MainActivity.this,"enterServer",roleInfo.toString());
	}

	@Override
	public String get_platform_id() {
		String channelId = IUtils.getChannelId(this);
		Log.e(Y_TAG,"当前渠道ID: "+channelId);
		return channelId;
	}

	public String get_platform_isbn()
	{
		String str = "游戏名称：女神星球" + "  " + "著作权人：上海银月网络科技有限公司\n" + "文网游备字[2016]M-3075号" + " "
				+ "游戏版号：ISBN 978-7-7979-5341-2" + " " + "审批文号：新广出审[2017]677号\n" + "出版方:华东师范大学电子音像出版社有限公司"
				+" " + "运营商：上海银月科技有限公司";
		return  str;
	}

	@Override
	public  void on_game_user_upgrade(int level) {

		JSONObject roleInfo =get_roleInfo(String.valueOf(level));
		SFOnlineHelper.setData(this,"levelup",roleInfo.toString());
	}

	//易接扩展接口上报角色信息内容
	private JSONObject get_roleInfo(String level) {
		JSONObject roleInfo = new JSONObject();
		try {
			roleInfo.put("roleId", role_id); //当前登录的玩家角色 ID，必须为数字
			roleInfo.put("roleName", role_name); //当前登录的玩家角色名，不能为空，不能为 null
			roleInfo.put("roleLevel", level); //当前登录的玩家角色等级，必须为数字，且不能为 0，若无，传入 1
			roleInfo.put("zoneId", role_server_id); //当前登录的游戏区服 ID，必须为数字，且不能为 0，若无，传入 1
			roleInfo.put("zoneName", role_server_name); //当前登录的游戏区服名称，不能为空，不能为null
			roleInfo.put("balance", "0"); //用户游戏币余额，必须为数字，若无，传入 0
			roleInfo.put("vip", "1"); //当前用户 VIP 等级，必须为数字，若无，传入 1
			roleInfo.put("partyName", "无帮派"); //当前角色所属帮派，不能为空，不能为 null，若无，传入“无帮派”
			roleInfo.put("roleCTime", "21322222");  //单位为秒，创建角色的时间
			roleInfo.put("roleLevelMTime", "54456556");  //单位为秒，角色等级变化时间
		} catch (JSONException e) {
			e.printStackTrace();
		}
		return  roleInfo;
	}

	@Override
	public boolean onKeyDown(int var1, KeyEvent var2) {
		if (var1 == KeyEvent.KEYCODE_BACK) {
			Log.e(Y_TAG, "onBackPressed: 点了退出键");

			SFOnlineHelper.exit (this, new SFOnlineExitListener()
			{
				@Override
				public void onSDKExit(boolean bool)
				{
					Log.e(Y_TAG, "onBackPressed: SDK有退出小界面");
					if (bool)
					{
						Log.e(Y_TAG, "onBackPressed: SDK已退出,回调游戏退出");
						finish();
						exit_extra();
					}
				}
				@Override
				public void onNoExiterProvide()
				{
					Log.e(Y_TAG, "onBackPressed: SDK没有退出小界面");
				}
			});
		}
		return this.mUnityPlayer.injectEvent(var2);
	}

	private void exit_extra() {    //一些渠道在结束游戏时有一些额外操作
		if(cur_channelId.equals("4CB4C42B71641CB3")){   //4399渠道要完全释放资源
			android.os.Process.killProcess(android.os.Process.myPid());
		}
	}

	@Override
	protected void onStop() {
		super.onStop();
		SFOnlineHelper.onStop(this);
	}

	@Override
	protected void onRestart() {
		super.onRestart();
		SFOnlineHelper.onRestart ( this );
	}

	@Override
	protected void onPause() {
		super.onPause();
		SFOnlineHelper.onPause ( this );
	}

	@Override
	protected void onResume() {
		super.onResume();
		SFOnlineHelper.onResume( this );
	}

	@Override
	protected void onDestroy() {
		super.onDestroy();
		SFOnlineHelper.onDestroy( this );
	}

	@Override
	protected void onNewIntent(Intent intent) {
		// TODO Auto-generated method stub
		super.onNewIntent(intent);
		SFOnlineHelper.onNewIntent(this, intent);
	}

	@Override
	public void onConfigurationChanged(Configuration newConfig) {
		// TODO Auto-generated method stub
		super.onConfigurationChanged(newConfig);
		//SFOnlineHelper.onConfigurationChanged(this, newConfig);
	}

	@Override
	public void onRequestPermissionsResult(int requestCode,String[] permissions, int[] grantResults) {
		// TODO Auto-generated method stub
		Log.d(Y_TAG, "onRequestPermissionsResult");
		super.onRequestPermissionsResult(requestCode, permissions, grantResults);
		//SFOnlineHelper.onRequestPermissionsResult(this, requestCode, permissions, grantResults);
		switch (requestCode) {
			case REQUEST_READ_PHONE_STATE:
				if ((grantResults.length > 0) && (grantResults[0] == PackageManager.PERMISSION_GRANTED)) {
					Log.e(Y_TAG, "onRequestPermissionsResult: "+"获取读取手机信息权限" );
				}
				break;
			default:
				break;
		}
	}

	public void tip_unity(String ss)
	{
		Log.d(Y_TAG, "tip_unity: "+ss);
	}
}
