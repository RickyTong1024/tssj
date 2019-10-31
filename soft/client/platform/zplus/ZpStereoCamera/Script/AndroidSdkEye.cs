using System;
using UnityEngine;

public class AndroidSdkEye : MonoBehaviour
{

    private const string SDK_JAVA_CLASS = "com.k3dx.het.sdk.HumanEyeTrackingMain";
    private static string message;
    private static int HEIGHT = 300;
    private static int WIDTH = 450;
    void Start()
    {
        //startOrbindEEServerService();
    }
    private void OnGUI()
    {
        GUIStyle myButtonStyle = new GUIStyle(GUI.skin.button);
        myButtonStyle.fontSize = 30;
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("startEeserver", myButtonStyle, GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
        {
            startEeserver();
        }
        if (GUILayout.Button("stopEeserver", myButtonStyle, GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
        {
            stopEeserver();
        }
        if (GUILayout.Button("isEEServerServiceRunningAndBinding", myButtonStyle, GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
        {
            message = "isEEServerServiceRunningAndBinding:" + isEEServerServiceRunningAndBinding();
        }

        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("bindEEServerService", myButtonStyle, GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
        {
            bindEEServerService();
        }
        if (GUILayout.Button("startOrbindEEServerService", myButtonStyle,GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
        {
            startOrbindEEServerService();
        }
        if (GUILayout.Button("rebindOrRestartEEServerService", myButtonStyle, GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
        {
            rebindOrRestartEEServerService();
        }
        if (GUILayout.Button("unBindEEServerService", myButtonStyle, GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
        {
            unBindEEServerService();
        }


        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("getServiceTCPStatus", myButtonStyle, GUILayout.Width(WIDTH), GUILayout.Height(HEIGHT)))
        {
            message = "getServiceTCPStatus:" + getServiceTCPStatus();
        }
        GUILayout.EndHorizontal();
        if (message != null)
        {

            GUIStyle myTextStyle = new GUIStyle(GUI.skin.label);
            myTextStyle.fontSize = 50;
            GUILayout.TextField(message, myTextStyle);
        }
    }
    public static void startEeserver()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            callSdkApi("startEEServerService", context);
        }
        catch (Exception e)
        {
            Debug.Log("startEeserver " + e);
        }
    }
    public static bool isEEServerServiceRunningAndBinding()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            return callSdkApiBool("isEEServerServiceRunningAndBinding", context);
        }
        catch (Exception e)
        {
            Debug.Log("isServiceRunning " + e);
            return false;
        }
    }
    public static void bindEEServerService()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            callSdkApi("bindEEServerService", context);
        }
        catch (Exception e)
        {
            Debug.Log("startEeserver " + e);
        }
    }
    public static void startOrbindEEServerService()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            callSdkApi("startOrbindEEServerService", context);
        }
        catch (Exception e)
        {
            Debug.Log("startEeserver " + e);
        }
    }
    public static void rebindOrRestartEEServerService()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            callSdkApi("rebindOrRestartEEServerService", context);
        }
        catch (Exception e)
        {
            Debug.Log("startEeserver " + e);
        }
    }
    public static void unBindEEServerService()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            callSdkApi("unBindEEServerService", context);
        }
        catch (Exception e)
        {
            Debug.Log("startEeserver " + e);
        }
    }
    public static int getServiceTCPStatus()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            return callSdkApiInt("getServiceTCPStatus", context);
        }
        catch (Exception e)
        {
            Debug.Log("startEeserver " + e);
            return -1;
        }
    }
    public static int getTrackingCameraState()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            return callSdkApiInt("getTrackingCameraState", context);
        }
        catch (Exception e)
        {
            Debug.Log("getTrackingCameraState " + e);
            return -1;
        }
    }
    public static int getDeviceProtectorState()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            return callSdkApiInt("getDeviceProtectorState", context);
        }
        catch (Exception e)
        {
            Debug.Log("getDeviceProtectorState " + e);
            return -1;
        }
    }
    public static void stopEeserver()
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject context = jc.GetStatic<AndroidJavaObject>("currentActivity");
            callSdkApi("stopEEServerService", context);
        }
        catch (Exception e)
        {
            Debug.Log("stopEeserver " + e);
        }
    }
    //public static void restartEeserverService()
    //{
    //    try
    //    {
    //        callSdkApi("stopEEServerService");
    //    }
    //    catch (Exception e)
    //    {
    //        Debug.Log("stopEeserver " + e);
    //    }
    //}
    public static void callSdkApi(string apiName, params object[] args)
    {
        //ZpGameLog.Log("Unity3D " + apiName + " calling...");

        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            cls.CallStatic(apiName, args);
        }
    }
    public static int callSdkApiInt(string apiName, params object[] args)
    {
        //ZpGameLog.Log("Unity3D " + apiName + " calling...");

        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<int>(apiName, args);
        }
    }

    public static bool callSdkApiBool(string apiName, params object[] args)
    {
        //ZpGameLog.Log("Unity3D " + apiName + " calling...");

        using (AndroidJavaClass cls = new AndroidJavaClass(SDK_JAVA_CLASS))
        {
            return cls.CallStatic<bool>(apiName, args);
        }
    }
}
