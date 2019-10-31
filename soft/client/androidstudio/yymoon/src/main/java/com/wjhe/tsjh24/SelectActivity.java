package com.wjhe.tsjh24;

import android.os.Bundle;
import android.app.Activity;
import android.view.View;
import android.view.View.*;
import android.widget.*;
import android.content.Intent;

public class SelectActivity extends Activity
{
    @Override
    protected void onCreate(Bundle savedInstanceState)
    {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_select);

        ImageButton btn_alipay = findViewById(R.id.Alipay);
        btn_alipay.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                end("0");
            }
        });

        ImageButton btn_weixin = findViewById(R.id.Weixin);
        btn_weixin.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                end("1");
            }
        });

        ImageButton btn_close = findViewById(R.id.Close);
        btn_close.setOnClickListener(new OnClickListener() {
            @Override
            public void onClick(View v) {
                end("-1");
            }
        });
    }

    private void end(String select)
    {
        Intent intent = new Intent();
        intent.putExtra("select", select);
        setResult(3, intent);
        finish();
    }
}
