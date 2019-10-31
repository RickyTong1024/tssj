
using UnityEngine;
using System;
using ZpGame;
#if !UNITY_IOS
public class ZpGameAndroidApi : MonoBehaviour
{

    private const string SDK_JAVA_CLASS = "com.kdx.game.z_plus_3d.api.K3DX2Android";
    private const string SDK_ET_JAVA_CLASS = "com.kdx.game.eyetracksdk.EyeTrackUtil";

    public static bool checkeEeserverInstalled()
    {
        try
        {
            return callSdkApiBool("checkeEeserverInstalled");
        }
        catch (Exception e)
        {
            Debugger.Log("checkeEeserverInstalled " + e);
            return false;
        }
    }
    public static void startEeserver()
    {
        try
        {
            callSdkApi("startEeserver");
        }
        catch (Exception e)
        {
            Debugger.Log("startEeserver " + e);
        }
    }
    public static bool isServiceRunning()
    {
        try
        {
            return callSdkApiBool("isServiceRunning");
        }
        catch (Exception e)
        {
            Debugger.Log("isServiceRunning " + e);
            return false;
        }
    }

    public static void stopEeserver()
    {
        try
        {
            callSdkApi("stopEeserver");
        }
        catch (Exception e)
        {
            Debugger.Log("stopEeserver " + e);
        }
    }

    public static void checkVersion()
    {
        try
        {
            callSdkApi("checkVersion");
        }
        catch (Exception e)
        {
            ZpGameLog.Log("checkVersion " + e);
        }
    }

    public static void init(bool _IsDebug, bool _IsInitSDK = true, bool _IsFloat = false)
    {
        try
        {
            callSdkApi("init", _IsDebug, _IsInitSDK, _IsFloat);
        }
        catch (Exception e)
        {
            ZpGameLog.Log("init " + e);
        }
    }

    public static void init(bool _IsDebug, string APPID, string CLIENTID, string CLIENTKEY)
    {
        try
        {
            callSdkApi("init", _IsDebug, APPID, CLIENTID, CLIENTKEY);
        }
        catch (Exception e)
        {
            ZpGameLog.Log("init " + e);
        }
    }

    public static void registerGameId(string Gameid)
    {
        try
        {
            callSdkApi("registerGameId", Gameid);
        }
        catch (Exception e)
        {
            ZpGameLog.Log("registerGameID " + e);
        }
    }

    public static int getScreenType()
    {
        int ret = 0;
        try
        {
            ret = callSdkApiInt("getScreenType");
            if (ret == -1)
                return (int)ZpGameStatic.Global_ScrType;
            else
                return ret;
        }
        catch
        {
            //ZpGameLog.Log("Non Android call , return " + ZpGameStatic.Global_ScrType);
            return (int)ZpGameStatic.Global_ScrType;
        }
    }

    public static void openWidget(int w = 0, int h = 0)
    {
        try
        {
            callSdkApi("openWidget", w, h);
        }
        catch
        {
            return;
        }
    }

    public static void enterSScene(string sceneID)
    {
        try
        {
            callSdkApi("enterSScene", sceneID);
        }
        catch
        {
        }
    }

    public static void login()
    {
        try
        {
            callSdkApi("login");
        }
        catch
        {
        }
    }
    public static void switchAccount()
    {
        try
        {
            callSdkApi("switchAccount");
        }
        catch
        {
        }
    }
    public static void setRole(string rolerInfo)
    {
        try
        {
            callSdkApi("setRole", rolerInfo);
        }
        catch
        {
        }
    }
    public static void showpay(string payInfo)
    {
        try
        {
            callSdkApi("showpay", payInfo);
        }
        catch
        {
        }
    }
    public static void exit()
    {
        try
        {
            callSdkApi("exitApplication");
        }
        catch
        {
        }
    }

    public static void EyeTrackInit()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            callETSdkApi("init", context);
        }
        catch (Exception e)
        {
            Debug.Log("EyeTrackInit " + e);
        }
    }

    public static void onResume()
    {
        try
        {
            callETSdkApi("onResume");
        }
        catch (Exception e)
        {
            Debug.Log("onResume " + e);
        }
    }

    public static void onPause()
    {
        try
        {
            callETSdkApi("onPause");
        }
        catch (Exception e)
        {
            Debug.Log("onPause " + e);
        }
    }

    public static void EyeTrackDestroy()
    {
        try
        {
            callETSdkApi("onDestroy");
        }
        catch (Exception e)
        {
            Debug.Log("onDestroy " + e);
        }
    }

    public static float[] requestEyeTrackResult()
    {
        try
        {
             return callETSdkApiobj("requestEyeTrackResult");
        }
        catch (Exception e)
        {
            Debug.Log("requestEyeTrackResult " + e);
            return null;
        }
    }

    public static void enableDebug(bool enable)
    {
        try
        {
            callETSdkApi("enAbleDebug", enable);
        }
        catch (Exception e)
        {
            Debug.Log("enableDebug " + e);
        }
    }

    public static void callSdkApi(string apiName, params object[] args)
    {
        //ZpGameLog.Log("Unity3D " + apiName + " calling...");
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic(apiName, args);
        }
#endif
    }
    public static int callSdkApiInt(string apiName, params object[] args)
    {
        //ZpGameLog.Log("Unity3D " + apiName + " calling...");
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<int>(apiName, args);
        }
#endif
        return -1;
    }

    public static bool callSdkApiBool(string apiName, params object[] args)
    {
        //ZpGameLog.Log("Unity3D " + apiName + " calling...");
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<bool>(apiName, args);
        }
#endif
        return true;
    }

    public static void callETSdkApi(string apiName, params object[] args)
    {
#if UNITY_ANDROID
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_ET_JAVA_CLASS))
        {
            cls.CallStatic(apiName, args);
        }
#endif
    }

    static AndroidJavaObject rev;
    static float[] result = new float[20];
    public static float[] callETSdkApiobj(string apiName, params object[] args)
    {
        //#if UNITY_ANDROID
        
        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_ET_JAVA_CLASS))
        {
            rev = cls.CallStatic<AndroidJavaObject>(apiName, args);
            if (rev == null)
            {
                Debug.Log("eye track data null");
                return null;
            }
            result[0] = rev.Get<float>("SHADERPARCNT");

            result[1] = rev.Get<float>("PITCH");

            result[2] = rev.Get<float>("SLANT");

            result[3] = rev.Get<float>("CTVIEW");

            result[4] = rev.Get<float>("CAMERAX");

            result[5] = rev.Get<float>("CAMERAY");

            result[6] = (float)rev.Get<int>("RGB_MODE");

            for (int i = 1; i < result[0] - 5 ; i++)
            {
                result[i + 6] = (float)rev.Get<float>("SHADEREXT"+ i);
            }

            if (result[1] < 0)
                return null;

            return result;
        }
//#endif
    }
}
#else
public class ZpGameAndroidApi : MonoBehaviour
{
}
#endif