package com.wjhe.tsjh24;
import com.yymoon.game.GameActivity;

import com.unity3d.player.UnityPlayer;
import com.wjhe.tsjh24.alipay.Alipay;
import com.wjhe.tsjh24.wxapi.Weixin;

import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

public class MainActivity extends GameActivity
{
	private Alipay m_alipay;
	private Weixin m_weixin;
	private String m_pay_param = "";

	@Override
	protected void onCreate(Bundle savedInstanceState)
	{
		super.onCreate(savedInstanceState);
		m_alipay = new Alipay();
		m_alipay.init(this, "2016090501851430");
		m_weixin = new Weixin();
		m_weixin.init(this, "wx2045be7b1f45fdf5", "1446277302");
	}

	@Override
	public void game_pay(String param)
	{
		m_pay_param = param;
		Intent intent = new Intent(MainActivity.this, SelectActivity.class);
		startActivityForResult(intent, 11);
	}

	@Override
	protected void onActivityResult(int requestCode, int resultCode, Intent data)
	{
		super.onActivityResult(requestCode, resultCode, data);
		String select = data.getStringExtra("select");
		if (select.equals("0"))
		{
			do_alipay(m_pay_param);
		}
		else if (select.equals("1"))
		{
			do_weixin(m_pay_param);
		}
		else if(select.equals("-1"))
		{
			pay_cancel("");
		}
	}

	public void do_alipay(String param)
	{
		Log.d("unity","param:"+param);
		String[] s = param.split(" ");
		String body = s[0];
		String subject = s[1];
		String price = s[2];
		String pay_url = s[3];
		String sign_url = pay_url+"notifysign";
		String notify_url = pay_url+"notifyali";
		String rechName = get_rechName(price);
		m_alipay.do_alipay(body, rechName, price, sign_url, notify_url);
	}

	public void do_weixin(String param)
	{
		String[] s = param.split(" ");
		String body = s[0];
		String subject = s[1];
		String price = s[2];
		String pay_url = s[3];
		String rechName = get_rechName(price);
		m_weixin.do_weixinpay(body, rechName, price, pay_url);
	}

	private String get_rechName(String price)
	{
		int p=Integer.parseInt(price);
		String rechName = p*10 +"diamonds";
		return  rechName;
	}

	public void pay_done(String message)
	{
		UnityPlayer.UnitySendMessage("game_node","recharge_done", message);
	}

	public void pay_cancel(String message)
	{
		UnityPlayer.UnitySendMessage("game_node","recharge_cancel", message);
	}

	public void toastMes(String mes)
	{
		showToast(mes);
	}

	@Override
	public String get_platform_id()
	{
		return "yymoon";
	}
}
