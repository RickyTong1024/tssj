package com.yymoon.xzn.official;

import android.content.Intent;
import android.graphics.Color;

import com.snowfish.cn.ganga.helper.SFOnlineSplashActivity;

public class SplashActivity extends SFOnlineSplashActivity{
	@Override
	public int getBackgroundColor() {
		// TODO Auto-generated method stub
		return Color.WHITE;
	}

	@Override
	public void onSplashStop() {
		// TODO Auto-generated method stub
		Intent intent = new Intent(this,MainActivity.class);
		startActivity(intent);
		this.finish();
	}
}
