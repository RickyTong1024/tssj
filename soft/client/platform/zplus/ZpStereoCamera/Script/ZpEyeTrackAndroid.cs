#if !UNITY_IOS
using System;
using System.Collections;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class ZpEyeTrackAndroid : ZpMonoUnitySingleton<ZpEyeTrackAndroid>
{
    bool SupportETAndroid = false;
    bool _3DOn = false;
    float[] EyeTraceResult;
    bool Exist = true;

    public bool IsSupportEyeTrack
    {
        get
        {
            return SupportETAndroid;
        }
    }

    public bool Is3DOn
    {
        get
        {
            return _3DOn;
        }
    }

    void Awake()
    {
        if (!CheckAndroidSupportET())
        {
            Debug.Log("ZpEyeTrackAndroid false");
            Exist = false;
            return;
        }
        ZpGameAndroidApi.EyeTrackInit();
        Exist = true;
    }

    void OnApplicationPause(bool pause)
    {
        if (Exist)
        {
            if (pause)
                ZpGameAndroidApi.onPause();
            else
                ZpGameAndroidApi.onResume();
        }
    }

    void Update()
    {
        if (Exist)
        {
            EyeTraceResult = ZpGameAndroidApi.requestEyeTrackResult();
            if (EyeTraceResult == null)
                _3DOn = false;
            else
                _3DOn = true;
        }
    }

    void OnDestory()
    {
        if (Exist)
        {
            ZpGameAndroidApi.EyeTrackDestroy();
        }
    }

    private bool CheckAndroidSupportET()
    {
        SupportETAndroid = ZpGameAndroidApi.checkeEeserverInstalled();
        return SupportETAndroid;
    }

    public void EyeTraceResume()
    {

    }
    public void EyeTracePause()
    {

    }

    public void FocusOn()
    {
    }

    public void FocusOut()
    {
    }

    public float[] getEyetrackData()
    {
        return EyeTraceResult;
    }

    public void Begin()
    {
        Debug.Log("android eyetrack begin");
    }
}
#else
#endif