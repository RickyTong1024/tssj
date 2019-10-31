using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZpGame;
public class ZpGameManage : ZpMonoUnitySingleton<ZpGameManage>
{
#if !UNITY_IOS
    /// <summary>
    /// App_id, Client_id and Client_key: The keys connect with the server of SDK
    /// </summary>
    //public string App_id = "362639";
    //public string Client_id = "300";
    //public string Client_key = "c537272e1591ef5a876ffa9053fa038d";
    public bool _Debug = false;

    /// <summary>
    ///  Widget_x and Widget_y: The position about Float widget of sdk  
    /// </summary>
    [Range(0, 1)]
    public float Widget_x = 0;
    [Range(0, 1)]
    public float Widget_y = 0;

    /// <summary>
    /// StereoTrigger: The Trigger for Stereo on or off
    /// </summary>
    public bool StereoTrigger = true;


    /// <summary>
    /// EyeTextureDepth: The parameters for the RenderTexture
    /// </summary>

	public ZpGameStatic.RenderTextureDepth EyeTextureDepth = ZpGameStatic.RenderTextureDepth.Depth_24;
	public ZpGameStatic.RenderTextureRect EyeTextureRect = ZpGameStatic.RenderTextureRect.Half_screen;

	//// <summary>
	/// The type of the screen, Editor in the Editor Mode
	/// </summary>
	public ZpGameContant.AEffects screenType = ZpGameContant.AEffects.RLWeaved;
#endif
    void Awake()
    {
#if !UNITY_IOS
        Instance.log();
        //ZpGameSceneSet.Instance.Begin();
        ZpGameAndroidApi.checkVersion();

        ZpGameStatic.IsEnableStereo = StereoTrigger;
		ZpGameStatic.EyeTextureRect = EyeTextureRect;
		ZpGameStatic.EyeTextureDepth = EyeTextureDepth;
		ZpGameSceneSet.Instance.CreateEyeTexture();

        EyeTrack.Instance.Begin();

		ZpGameStatic.Global_ScrType = screenType;
#endif
    }

    // Use this for initialization
    void Start()
    {
#if !UNITY_IOS
        ZpGameAndroidApi.init(true);
		if(StereoTrigger)
			ZpGameAndroidApi.openWidget((int)(Screen.width * Widget_x), (int)(Screen.height * Widget_y));

		ZpGameStatic.IsInit = true;
		ZpGameSceneSet.Instance.reload();

        if (_Debug)
            ZpGameStatic.DebugType = "debug";
#endif
    }

    public void log()
    {
#if !UNITY_IOS
        Debugger.LogWarning("Welcome start stereo world " + ZpGameStatic.Version);
#endif
    }
}
