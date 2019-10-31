//
// Source code recreated from a .class file by IntelliJ IDEA
// (powered by Fernflower decompiler)
//

package com.wjhe.tsjh24.alipay;

import android.text.TextUtils;
import java.util.Iterator;
import java.util.Map;

public class AuthResult {
    private String resultStatus;
    private String result;
    private String memo;
    private String resultCode;
    private String authCode;
    private String alipayOpenId;

    public AuthResult(Map<String, String> rawResult, boolean removeBrackets) {
        if(rawResult != null) {
            Iterator var4 = rawResult.keySet().iterator();

            while(var4.hasNext()) {
                String key = (String)var4.next();
                if(TextUtils.equals(key, "resultStatus")) {
                    this.resultStatus = (String)rawResult.get(key);
                } else if(TextUtils.equals(key, "result")) {
                    this.result = (String)rawResult.get(key);
                } else if(TextUtils.equals(key, "memo")) {
                    this.memo = (String)rawResult.get(key);
                }
            }

            String[] resultValue = this.result.split("&");
            String[] var7 = resultValue;
            int var6 = resultValue.length;

            for(int var5 = 0; var5 < var6; ++var5) {
                String value = var7[var5];
                if(value.startsWith("alipay_open_id")) {
                    this.alipayOpenId = this.removeBrackets(this.getValue("alipay_open_id=", value), removeBrackets);
                } else if(value.startsWith("auth_code")) {
                    this.authCode = this.removeBrackets(this.getValue("auth_code=", value), removeBrackets);
                } else if(value.startsWith("result_code")) {
                    this.resultCode = this.removeBrackets(this.getValue("result_code=", value), removeBrackets);
                }
            }

        }
    }

    private String removeBrackets(String str, boolean remove) {
        if(remove && !TextUtils.isEmpty(str)) {
            if(str.startsWith("\"")) {
                str = str.replaceFirst("\"", "");
            }

            if(str.endsWith("\"")) {
                str = str.substring(0, str.length() - 1);
            }
        }

        return str;
    }

    public String toString() {
        return "resultStatus={" + this.resultStatus + "};memo={" + this.memo + "};result={" + this.result + "}";
    }

    private String getValue(String header, String data) {
        return data.substring(header.length(), data.length());
    }

    public String getResultStatus() {
        return this.resultStatus;
    }

    public String getMemo() {
        return this.memo;
    }

    public String getResult() {
        return this.result;
    }

    public String getResultCode() {
        return this.resultCode;
    }

    public String getAuthCode() {
        return this.authCode;
    }

    public String getAlipayOpenId() {
        return this.alipayOpenId;
    }
}
