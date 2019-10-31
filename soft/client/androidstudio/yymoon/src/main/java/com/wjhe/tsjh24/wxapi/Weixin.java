package com.wjhe.tsjh24.wxapi;

import android.util.Log;
import com.tencent.mm.sdk.modelpay.PayReq;
import com.tencent.mm.sdk.openapi.IWXAPI;
import com.tencent.mm.sdk.openapi.WXAPIFactory;
import com.unity3d.player.UnityPlayer;

import net.sourceforge.simcpux.MD5;
import net.sourceforge.simcpux.Util;

import org.w3c.dom.Document;
import org.w3c.dom.Node;
import org.w3c.dom.NodeList;
import org.xml.sax.SAXException;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.UnsupportedEncodingException;
import java.net.Inet6Address;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.net.SocketException;
import java.net.URLEncoder;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Enumeration;
import java.util.HashMap;
import java.util.Iterator;
import java.util.Map;
import java.util.Random;
import java.util.Set;
import java.util.SortedMap;
import java.util.TreeMap;

import javax.xml.parsers.DocumentBuilder;
import javax.xml.parsers.DocumentBuilderFactory;
import javax.xml.parsers.ParserConfigurationException;
import com.wjhe.tsjh24.MainActivity;

public class Weixin {

    private IWXAPI m_wx;
    public static MainActivity m_act;
    public static String m_app_id;
    public static String m_partner_id;

    public void init(MainActivity act, String app_id, String partner_id)
    {
        m_app_id = app_id;
        m_partner_id = partner_id;
        m_act = act;
        m_wx = WXAPIFactory.createWXAPI(m_act, m_app_id, false);
        m_wx.registerApp(m_app_id);
    }

    public void do_weixinpay(String body,String subject,String price,String pay_url)
    {
        if(m_wx.isWXAppInstalled())
        {
            int value  = (int)Float.parseFloat(price);
            String p=String.valueOf(value*100);
            String url = "https://api.mch.weixin.qq.com/pay/unifiedorder";
            SortedMap<String, String> req = new TreeMap<String, String>();
            req.put("notify_url", pay_url+"notifywechat");
            req.put("appid", m_app_id);
            req.put("device_info", "WEB");
            req.put("attach", body);
            req.put("body", subject);
            req.put("mch_id", m_partner_id);
            req.put("nonce_str", getRandomStringByLength(32));
            req.put("out_trade_no", getCurrTime());
            req.put("total_fee", p);
            req.put("spbill_create_ip", getHostIP());
            req.put("trade_type", "APP");
            String xmlParams = parseString2Xml(req,getSign(req,false));
            try {
                xmlParams =   new String(xmlParams.toString().getBytes("utf-8"), "iso8859-1");
            } catch (UnsupportedEncodingException e2) {
                // TODO Auto-generated catch block
                e2.printStackTrace();
            }
            Log.d("unity", "xmlParams:" + xmlParams);
            String jsonStr = "";
            try {
                jsonStr = Util.httpPost(url, xmlParams);
                if (jsonStr==null||jsonStr.equals(""))
                {
                    m_act.toastMes("请求失败");
                    m_act.pay_cancel("");
                    return;
                }
                Log.d("unity", "jsonStr:" + jsonStr);
            } catch (Exception e) {
                e.printStackTrace();
            }
            Map<String,String> resultMap = new HashMap();
            try {
                resultMap = getMapFromXML(jsonStr);
            } catch (ParserConfigurationException e1) {
                // TODO Auto-generated catch block
                e1.printStackTrace();
            } catch (IOException e1) {
                // TODO Auto-generated catch block
                e1.printStackTrace();
            } catch (SAXException e1) {
                // TODO Auto-generated catch block
                e1.printStackTrace();
            }
            if (jsonStr.indexOf("FAIL") == -1 && jsonStr.trim().length() > 0)
            {
                Log.d("unity", "startpay");
                String time = new Date().getTime() + "";
                String noncetr = getRandomStringByLength(32);
                PayReq request = new PayReq();
                request.appId = m_app_id;
                request.partnerId = m_partner_id;
                request.prepayId= resultMap.get("prepay_id");
                request.nonceStr= noncetr;
                request.timeStamp= time;
                request.packageValue = "Sign=WXPay";
                //Log.d("unity", request.checkArgs() + "");
                SortedMap<String, String> requestMap = new TreeMap<String, String>();
                requestMap.put("appid", m_app_id);
                requestMap.put("partnerid", m_partner_id);
                requestMap.put("prepayid", resultMap.get("prepay_id"));
                requestMap.put("package", "Sign=WXPay");
                requestMap.put("noncestr", noncetr);
                requestMap.put("timestamp", time);
                request.sign= getSign(requestMap,false);
                Log.d("unity", "sign"+ request.sign);
                Log.d("unity", "request:"+request.checkArgs() );
                m_wx.sendReq(request);
            }
            else
            {
                Log.d("unity", "startpayerror");
            }
        }
        else
        {
            UnityPlayer.UnitySendMessage("node(Clone)",
                    "recharge_cancel", "");
        }

    }

    public static Map<String,String> getMapFromXML(String xmlString) throws ParserConfigurationException, IOException, SAXException {
        //这里用Dom的方式解析回包的最主要目的是防止API新增回包字段
        DocumentBuilderFactory factory = DocumentBuilderFactory.newInstance();
        DocumentBuilder builder = factory.newDocumentBuilder();
        InputStream is   =   new ByteArrayInputStream(xmlString.getBytes());
        Document document = builder.parse(is);
        //获取到document里面的全部结点
        NodeList allNodes = document.getFirstChild().getChildNodes();
        Node node;
        Map<String, String> map = new HashMap<String, String>();
        int i=0;
        while (i < allNodes.getLength()) {
            node = allNodes.item(i);
            {
                map.put(node.getNodeName(),node.getTextContent());
            }
            i++;
        }
        return map;
    }

    public static String parseString2Xml(SortedMap<String, String> map,String sign){
        StringBuffer sb = new StringBuffer();
        sb.append("<xml>\n");
        Set es = map.entrySet();
        Iterator iterator = es.iterator();
        while(iterator.hasNext()){
            Map.Entry entry = (Map.Entry)iterator.next();
            String k = (String)entry.getKey();
            String v = entry.getValue().toString();
            try {
                //v = URLEncoder.encode((String)entry.getValue(), "UTF-8");
            } catch (Exception e) {
                // TODO Auto-generated catch block
                Log.d("unity", "utf8error:" + (String)entry.getValue());
            }
            sb.append("<"+k+">"+v+"</"+k+">\n");
        }
        sb.append("<sign>"+sign+"</sign>\n");
        sb.append("</xml>");
        return sb.toString();
    }

    public static String getRandomStringByLength(int length) {
        String base = "abcdefghijklmnopqrstuvwxyz0123456789";
        Random random = new Random();
        StringBuffer sb = new StringBuffer();
        for (int i = 0; i < length; i++) {
            int number = random.nextInt(base.length());
            sb.append(base.charAt(number));
        }
        return sb.toString();
    }

    public static String getCurrTime() {
        Date now = new Date();
        SimpleDateFormat outFormat = new SimpleDateFormat("yyyyMMddHHmmss");
        String s = outFormat.format(now);
        return s;
    }

    public static String getHostIP() {

        String hostIp = null;
        try {
            Enumeration nis = NetworkInterface.getNetworkInterfaces();
            InetAddress ia = null;
            while (nis.hasMoreElements()) {
                NetworkInterface ni = (NetworkInterface) nis.nextElement();
                Enumeration<InetAddress> ias = ni.getInetAddresses();
                while (ias.hasMoreElements()) {
                    ia = ias.nextElement();
                    if (ia instanceof Inet6Address) {
                        continue;// skip ipv6
                    }
                    String ip = ia.getHostAddress();
                    if (!"127.0.0.1".equals(ip)) {
                        hostIp = ia.getHostAddress();
                        break;
                    }
                }
            }
        } catch (SocketException e) {
            Log.i("unity", "SocketException");
            e.printStackTrace();
        }
        return hostIp;
    }

    public static String getSign(SortedMap<String, String> params,boolean utf8){
        String sign = null;
        StringBuffer sb = new StringBuffer();
        Set es = params.entrySet();
        Iterator iterator = es.iterator();
        while(iterator.hasNext()){
            Map.Entry entry = (Map.Entry)iterator.next();
            String k = (String)entry.getKey();
            String v;
            try {
                if(utf8)
                {
                    v = URLEncoder.encode((String)entry.getValue(), "UTF-8");
                }
                else
                {
                    v = entry.getValue().toString();
                }

            } catch (UnsupportedEncodingException e) {
                // TODO Auto-generated catch block
                Log.d("unity", "utf8error:" + (String)entry.getValue());
                v = (String)entry.getValue();
            }
            if (null != v && !"".equals(v) && !"sign".equals(k)&& !"key".equals(k)) {
                sb.append(k+"="+v+"&");
            }
        }
        sb.append("key=" + "yinyuewangluo123YYMOONWANGLUO123");
        Log.d("unity", sb.toString());
        sign = MD5.getMessageDigest(sb.toString().getBytes()).toUpperCase();
        return sign;
    }
}
