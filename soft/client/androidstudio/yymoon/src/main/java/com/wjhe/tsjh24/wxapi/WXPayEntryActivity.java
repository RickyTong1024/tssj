package com.wjhe.tsjh24.wxapi;


import com.tencent.mm.sdk.modelbase.BaseReq;
import com.tencent.mm.sdk.modelbase.BaseResp;
import com.tencent.mm.sdk.openapi.IWXAPI;
import com.tencent.mm.sdk.openapi.IWXAPIEventHandler;
import com.tencent.mm.sdk.openapi.WXAPIFactory;
import android.app.Activity;
import android.content.Intent;
import android.os.Bundle;
import android.util.Log;

public class WXPayEntryActivity extends Activity implements IWXAPIEventHandler{

	private static final String TAG = "unity";

	private IWXAPI api;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		api = WXAPIFactory.createWXAPI(this, Weixin.m_app_id);
		api.handleIntent(getIntent(), this);

	}

	@Override
	protected void onNewIntent(Intent intent) {
		super.onNewIntent(intent);
		setIntent(intent);
		api.handleIntent(intent, this);
	}

	@Override
	public void onReq(BaseReq req) {

	}

	@Override
	public void onResp(BaseResp resp) {
		Log.d(TAG, "onPayFinish, errCode = " + resp.errCode);

		if (resp.errCode == 0)
		{
			Weixin.m_act.pay_done("");
		}
		else if(resp.errCode == -1)
		{
			Weixin.m_act.pay_cancel("");
		}
		else
		{
			Weixin.m_act.pay_cancel("");
		}
		finish();
	}
}
