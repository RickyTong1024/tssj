#if !UNITY_IOS
using UnityEngine;
using System.Collections;
using System;
using ZpGame;
public class ZpGameCallback : MonoBehaviour
{
    public delegate void CallBack(int code, JsonData json);
    public static CallBack OnLoginCallBack;
    public void Onk3dxGameSdkCallback(string jsonstr)
    {
        Debugger.LogWarning(this.gameObject.name + "stereoCamera Onk3dxGameSdkCallback: " + jsonstr);
        JsonData json = JsonMapper.ToObject(jsonstr);
        string callbackType = (string)json["callbackType"];
        int code = (int)json["code"];
        switch (callbackType)
        {
            case ZpGameContant.CALLBACKTYPE_Init:
                DisposeInit(code, json);
                break;
            case ZpGameContant.CALLBACKTYPE_CameraParameter:
                DisposeCameraP(code, json);
                break;
            case ZpGameContant.CALLBACKTYPE_deviceCheck:
                DisposeCheckDevice(code, json);
                //UnityEngine.Object.FindObjectOfType<EyeTrack>().SendMessage("updateState");
                UnityEngine.Object.FindObjectOfType<ZpGameStereoCamera>().SendMessage("updateState");
                break;
            case ZpGameContant.CALLBACKTYPE_DownResult:
                DisposeDownResult(code, json);
                break;
            case ZpGameContant.CALLBACKTYPE_SetRole:
                break;
            case ZpGameContant.CALLBACKTYPE_Pay:
                break;
            case ZpGameContant.CALLBACKTYPE_Login:
                if (OnLoginCallBack != null)
                {
                    OnLoginCallBack(code, json);
                    OnLoginCallBack = null;
                }
                break;
        }
    }

    // Use this for initialization
    private void Awake()
    {
        ZpGameAndroidApi.registerGameId(this.gameObject.name);
    }

    void OnDestroy()
    {
        Debug.Log("game  callback OnDestroy");
        ZpGameAndroidApi.exit();
    }
    public void DisposeInit(int code, JsonData json)
    {
        if (code == ZpGameContant.CODE_SUCCESS)
        {
            Debug.Log("DisposeInit SUCCESS " + code);
        }
        else
        {
            Debug.Log("DisposeInit FAIL " + code);
        }
    }
    //public void DisposeInit(int code, JsonData json)
    //{
    //    if (code == ZpGameContant.CODE_SUCCESS)
    //    {
    //        ZpGameStatic.IsInit = true;
    //        JsonData CameraData = json["data"];
    //        if (CameraData.Count > 0)
    //        {
    //            try
    //            {
    //                if (((IDictionary)CameraData).Contains(ZpGameContant.DATA_DebugType))
    //                    ZpGameStatic.DebugType = (string)CameraData[ZpGameContant.DATA_DebugType];

    //                if (((IDictionary)CameraData).Contains(ZpGameContant.DATA_DebugInterval))
    //                {
    //                    JsonData DebugInterval = CameraData[ZpGameContant.DATA_DebugInterval];
    //                    ZpGameStatic.DebugInterval = GetJsonFloat(DebugInterval);
    //                }

    //                if (((IDictionary)CameraData).Contains(ZpGameContant.DATA_Config))
    //                {
    //                    string CameraConfig = (string)CameraData[ZpGameContant.DATA_Config];
    //                    if (CameraConfig != null)
    //                    {
    //                        ZpGameSceneSet.Instance.loadxml(CameraConfig);
    //                    }
    //                }
    //                ZpGameSceneSet.Instance.reload();

    //            }
    //            catch (Exception e)
    //            {
    //                Debugger.Log("INIT " + e);
    //            }
    //        }
    //        else
    //        {
    //            Debugger.Log("INIT DATA FAIL");
    //        }
    //    }
    //    else
    //    {
    //        log("DISPOSE FAIL");
    //    }
    //}
    public void DisposeDownResult(int code, JsonData json)
    {
        if (code == ZpGameContant.CODE_SUCCESS)
        {
            JsonData CameraData = json["data"];
            if (CameraData.Count > 0)
            {
                try
                {
                    if (((IDictionary)CameraData).Contains(ZpGameContant.DATA_Config))
                    {
                        string CameraConfig = (string)CameraData[ZpGameContant.DATA_Config];
                        if (CameraConfig != null)
                        {
                            ZpGameSceneSet.Instance.loadxml(CameraConfig);
                        }
                    }
                    ZpGameSceneSet.Instance.reload();
                    Debug.LogWarning("DisposeDownResult  " + code);
                }
                catch (Exception e)
                {
                    Debug.Log("DisposeDownResult " + e);
                }
            }
        }
        else
        {
            Debug.Log("DisposeDownResult FAIL " + code);
        }
    }

    public void DisposeCheckDevice(int code, JsonData json)
    {
        if (code == ZpGameContant.CODE_SUCCESS || code == ZpGameContant.CODE_CD_FAIL_BUT_HAVE_LOCAL)
        {
            ZpGameStatic.IsInit = true;
            JsonData CameraData = json["data"];
            if (CameraData.Count > 0)
            {
                try
                {
                    if (((IDictionary)CameraData).Contains(ZpGameContant.DATA_DebugType))
                        ZpGameStatic.DebugType = (string)CameraData[ZpGameContant.DATA_DebugType];

                    if (((IDictionary)CameraData).Contains(ZpGameContant.DATA_DebugInterval))
                    {
                        JsonData DebugInterval = CameraData[ZpGameContant.DATA_DebugInterval];
                        ZpGameStatic.DebugInterval = GetJsonFloat(DebugInterval);
                    }

                    if (((IDictionary)CameraData).Contains(ZpGameContant.DATA_Config))
                    {
                        string CameraConfig = (string)CameraData[ZpGameContant.DATA_Config];
                        if (CameraConfig != null)
                        {
                            ZpGameSceneSet.Instance.loadxml(CameraConfig);
                        }
                    }
                    //ZpGameSceneSet.Instance.reload();

                    Debug.LogWarning("CheckDevice  " + code);

                }
                catch (Exception e)
                {
                    Debug.Log("CheckDevice " + e);
                }
            }
            else
            {
                Debug.Log("CheckDevice FAIL " + code);
            }
        }
        else
        {
            log("CheckDevice FAIL");
        }
    }
    public void DisposeCameraP(int code, JsonData json)
    {
        JsonData Camerap;

        if (code == ZpGameContant.CODE_SUCCESS)
        {

            JsonData CameraData = json["data"];

            Camerap = (JsonData)CameraData[ZpGameContant.DATA_percent];
            ZpGameSceneSet.Instance.SetPercent(GetJsonFloat(Camerap));
        }
        else
        {
            log("DisposeCameraP fail");
        }
    }

    public float GetJsonFloat(JsonData json)
    {
        float ret = 0.0f;

        switch (json.GetJsonType())
        {
            case JsonType.Int:
                ret = (float)(int)json;
                break;
            case JsonType.Double:
                ret = (float)(double)json;
                break;
            case JsonType.String:
                try
                {

                    ret = (float)Convert.ToDouble((string)json);
                }
                catch (Exception e)
                {
                    Debugger.Log("is not a valid number " + e);
                }
                break;
            default:
                Debugger.Log("is not a valid json number");
                break;
        }

        return ret;
    }

    private void FixedUpdate()
    {
        if (ZpGameStatic.DebugType == "debug")
        {
            ZpGameDebug Debug = gameObject.GetComponent<ZpGameDebug>();
            if (Debug == null)
                this.gameObject.AddComponent<ZpGameDebug>();
        }
    }

    void log(string msg) { }

}
#else
using UnityEngine;
public class ZpGameCallback : MonoBehaviour {
}
#endif