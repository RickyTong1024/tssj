using UnityEngine;
using System.Collections;

public static class ZpGameStatic {

    public static string Version = "1.0.17";

    public static bool IsInit = false;
    public static bool IsEnableStereo = true;
    public static string DebugType = "release";
    public static bool Show_EyeTrace_Log = false;
    public static float DebugInterval = 0.01f;


	public enum RenderTextureRect
	{
		Half_screen,
		Full_screen,
	}

	public enum RenderTextureDepth
	{
		Depth_16 = 16,
		Depth_24 = 24,
		Depth_32 = 32,
	}
	public static RenderTextureRect EyeTextureRect = RenderTextureRect.Half_screen;
	public static RenderTextureDepth EyeTextureDepth = RenderTextureDepth.Depth_24;

#if UNITY_EDITOR
	public static ZpGameContant.AEffects Global_ScrType = ZpGameContant.AEffects.Original;
#elif UNITY_ANDROID
	public static ZpGameContant.AEffects Global_ScrType = ZpGameContant.AEffects.Original;
#elif UNITY_STANDALONE_WIN
    public static ZpGameContant.AEffects Global_ScrType = ZpGameContant.AEffects.SlantWeaved;
#endif
}



