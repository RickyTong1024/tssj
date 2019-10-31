using UnityEngine;
using System.Collections;

public class ZpGameContant {

    #region Debug
    public const string DATA_DebugType = "DebugType"; // debug or release
    public const string DATA_Show_EyeTrace_Log = "EyeTraceLog"; // 打开人眼跟踪日志
    public const string DATA_DebugInterval = "DebugInterval"; // 0.001f
    public const string DATA_Config = "ConfigPath"; // config
    #endregion

    public const string DATA_SceneID = "sceneID";
    public const string DATA_CameraCtlType = "CameraCtlType";

    public const string DATA_percent = "percent";
    public const string DATA_ScreenType = "ScreenType";

    public const string CALLBACKTYPE_Init = "init";
    public const string CALLBACKTYPE_CameraParameter = "stereocamerap";
    public const string CALLBACKTYPE_Pay = "pay";
    public const string CALLBACKTYPE_Login = "login";
    public const string CALLBACKTYPE_SwitchAccount = "switchAccount";
    public const string CALLBACKTYPE_SetRole = "setRole";
    public const string CALLBACKTYPE_exit = "exit";
	public const string CALLBACKTYPE_deviceCheck = "deviceCheck";
    public const string CALLBACKTYPE_DownResult = "downResult";

    public const int CODE_SUCCESS = 1;                   //成功
    public const int CODE_FAIL = -1;                     //未知失败
    public const int CODE_LOGIN_USER_ACCOUNT_FAIL = -2;  //登陆失败
    public const int CODE_PAY_FAIL = -4;                 //支付失败
    public const int CODE_NETWORK_FAIL = -8;             //网络失败
    public const int CODE_TIME_OUT_FAIL = -16;           //网络超时失败
    public const int CODE_CD_FAIL = -32;                 //检查设备失败
    public const int CODE_CD_FAIL_BUT_HAVE_LOCAL = -64;  //检查设备失败但是有本地数据
    public const int CODE_NONE_FAIL = -128;              //未知错误



    public enum AEffects
    {
        Original = 0,
        LRWeaved = 1,
        RLWeaved = 2,
        SlantWeaved = 3,
        TwoDWithZ = 4,
        LRSBS = 5,
    }

    public enum StereoCameraType
    {
        None = 0,
        Static = 1,
        Dynamic = 2,
    }


}
