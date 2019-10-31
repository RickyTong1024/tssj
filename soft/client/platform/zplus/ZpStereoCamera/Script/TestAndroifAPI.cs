using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZpGame;

public class TestAndroifAPI : MonoBehaviour
{
    private string androidapiclass = "com.kdx.game.z_plus_3d.api.K3DX2Android";
    private static string message;
    // Use this for initialization
    public  void Onk3dxGameSdkCallback(string message)
    {
        TestAndroifAPI.message = message;
        Debugger.Log(this.gameObject.name + "stereoCamera Onk3dxGameSdkCallback: " + message);
        print(message);
    }
    void Start()
    {
        ZpGameAndroidApi.registerGameId(this.gameObject.name);
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("f发起支付", GUILayout.Width(200), GUILayout.Height(100)))
        {
            showpay("");
        }
        if (GUILayout.Button("登录", GUILayout.Width(200), GUILayout.Height(100)))
        {
            login();

        }
        if (GUILayout.Button("切换账号", GUILayout.Width(200), GUILayout.Height(100)))
        {
            switchCount();

        }
        if (GUILayout.Button("角色信息", GUILayout.Width(200), GUILayout.Height(100)))
        {
            setRole("");

        }
        if (GUILayout.Button("获取3D效果参数与场景参数", GUILayout.Width(200), GUILayout.Height(100)))
        {
            game3dParams(1);

        }
        if (GUILayout.Button("记录用户调节后的3D效果参数", GUILayout.Width(200), GUILayout.Height(100)))
        {
            gameParamRecord();

        }
        //if (GUILayout.Button("播放", GUILayout.Width(200), GUILayout.Height(200)))
        //{
        //    AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        //    o.CallStatic("playVideo","");

        //}
        if (GUILayout.Button("退出", GUILayout.Width(200), GUILayout.Height(100)))
        {
            clearData();
            Application.Quit();

        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("初始化", GUILayout.Width(200), GUILayout.Height(100)))
        {
            initSDK();

        }
		if (GUILayout.Button("getScreentype", GUILayout.Width(200), GUILayout.Height(100)))
		{
            int type = ZpGameAndroidApi.getScreenType();
			Debugger.Log("getScreenType=================="+type);

		}
        GUILayout.EndHorizontal();
       
        if (message != null)
        {
            GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
            myButtonStyle.fontSize = 50;
            GUILayout.TextField(message, myButtonStyle, GUILayout.Height(150));
        }
    }
    public void initSDK()
    {
       // AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        ZpGameAndroidApi.init(true, true, true);
    }
    public void showpay(string payInfo)
    {
        JsonData jd = new JsonData();
        jd["cp_order_id"] = "2016102811111";
        jd["product_price"] = 0.01f;
        jd["product_count"] = 1;
        jd["product_id"] = "1";
        jd["product_name"] = "金币";
        jd["product_desc"] = "qew";
        jd["exchange_rate"] = 1;
        jd["currency_name"] = "qwe";
        jd["ext"] = "qwe";


        JsonData roleInfo = new JsonData();
        roleInfo["role_type"] = 1;
        roleInfo["server_id"] = "123.23.23";
        roleInfo["server_name"] = "123";
        roleInfo["party_name"] = "partyname";

        roleInfo["role_id"] = "Server_id";
        roleInfo["role_level"] = "111";
        roleInfo["role_vip"] = "222";
        roleInfo["role_balence"] = 12f;
        roleInfo["role_name"] = "qwe";
        System.DateTime currentTime = new System.DateTime();
        roleInfo["rolelevel_ctime"] = "" + currentTime.Millisecond / 1000;
        roleInfo["rolelevel_mtime"] = "" + currentTime.Millisecond / 1000;
        jd["roleinfo"] = roleInfo;

        //AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        //o.CallStatic("showPay", jd.ToJson());
        ZpGameAndroidApi.showpay(jd.ToJson());
    }
    public void clearData()
    {
        //AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        //o.CallStatic("exitApplication");
        ZpGameAndroidApi.exit();
    }
    public void login()
    {
        //AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        //o.CallStatic("login");
        ZpGameAndroidApi.login();
    }
    public void switchCount()
    {
        //AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        //o.CallStatic("switchAccount");
        ZpGameAndroidApi.switchAccount();
    }
    public void game3dParams(int gameID)
    {
        //AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        //o.CallStatic("game3dParams", gameID);
    }
    public void gameParamRecord()
    {
        //AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        //JsonData record = new JsonData();
        //record["game_id"] = 1;
        //record["parameter"] = 10;

        //o.CallStatic("gameParamRecord", record.ToJson());
    }
    public void setRole(string rolerInfo)
    {
        JsonData roleInfo = new JsonData();
        roleInfo["role_type"] = 1;
        roleInfo["server_id"] = "123.23.23";
        roleInfo["server_name"] = "123";
        roleInfo["party_name"] = "partyname";

        roleInfo["role_id"] = "Server_id";
        roleInfo["role_level"] = "111";
        roleInfo["role_vip"] = "222";
        roleInfo["role_balence"] = 12f;
        roleInfo["role_name"] = "qwe";
        System.DateTime currentTime = new System.DateTime();
        roleInfo["rolelevel_ctime"] = "" + currentTime.Millisecond / 1000;
        roleInfo["rolelevel_mtime"] = "" + currentTime.Millisecond / 1000;
        //AndroidJavaObject o = new AndroidJavaObject(androidapiclass);
        //o.CallStatic("setRoleInfo", roleInfo.ToJson());
        ZpGameAndroidApi.setRole(roleInfo.ToJson());
    }
}
