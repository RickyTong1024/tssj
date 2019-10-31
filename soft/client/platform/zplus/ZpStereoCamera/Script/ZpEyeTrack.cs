#if !UNITY_IOS
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZpGame;

public class EyeTrack
{
#if !UNITY_EDITOR && UNITY_ANDROID
    public static ZpEyeTrackAndroid Instance
    {
        get{
            return ZpEyeTrackAndroid.Instance;
        }
    }
#else
    public static ZpEyeTrackWindows Instance
    {
        get
        {
            return ZpEyeTrackWindows.Instance;
        }
    }
#endif
}
#endif