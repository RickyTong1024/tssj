
using UnityEngine;

public class ZpGameDevelopApi
{
#if !UNITY_IOS
	/// <summary>
	/// Attachs the stereo to camera.
	/// </summary>
	/// <param name="Cam">Cam.</param>
	/// <param name="id">Identifier.</param>
	/// <param name="LayerMask">Layer mask.</param>
	/// <param name="copyComponent">If set to <c>true</c> copy component.</param>
	public static void AttachStereoToCamera(Camera Cam, string id, int LayerMask = 0, bool copyComponent = false)
    {
        GameObject camera = Cam.gameObject;

        ZpGameStereoCamera tmp = camera.GetComponentInParent(typeof(ZpGameStereoCamera)) as ZpGameStereoCamera;
        if (tmp == null)
        {
            tmp = camera.AddComponent<ZpGameStereoCamera>();
            if (LayerMask != 0)
            {
                tmp._DefaultCullingMask = 1 << LayerMask;
            }
            if (copyComponent)
                tmp._IsSupportCopy = copyComponent;
            SwitchStereoPara(id, Cam);
        }
        else
        {
            SwitchStereoPara(id, Cam);
        }
    }

    /// <summary>
    /// Switchs the stereo para.
    /// </summary>
    /// <param name="id">Identifier.</param>
    /// <param name="BindCamera">Bind camera.</param>
    public static void SwitchStereoPara(string id, Camera BindCamera = null)
    {
        if (id == null)
            return;
        ZpGameSceneSet.Instance.EnableSP(id, ZpGameContant.StereoCameraType.Static, 0.01f, 1f, null, BindCamera);
        ZpGameAndroidApi.enterSScene(id);
    }

	/// <summary>
	/// Switchs the stereo para.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="pupil">Pupil.</param>
	/// <param name="focal">Focal.</param>
	/// <param name="BindCamera">Bind camera.</param>
    public static void SwitchStereoPara(string id, float pupil, float focal, Camera BindCamera = null)
    {
        if (id == null)
            return;

        if (pupil == 0 || focal == 0)
        {
            pupil = 0.01f;
            focal = 1.0f;
        }
        ZpGameSceneSet.Instance.EnableSP(id, ZpGameContant.StereoCameraType.Static, pupil, focal, null, BindCamera);
        ZpGameAndroidApi.enterSScene(id);
    }

	/// <summary>
	/// Switchs the stereo para.
	/// </summary>
	/// <param name="id">Identifier.</param>
	/// <param name="toPupil">To pupil.</param>
	/// <param name="toTarget">To target.</param>
	/// <param name="Target">Target.</param>
	/// <param name="BindCamera">Bind camera.</param>
    public static void SwitchStereoPara(string id, float toPupil, float toTarget, Transform Target, Camera BindCamera = null)
    {
        if (id == null)
            return;

        if (toPupil == 0 || toTarget == 0)
        {
            toPupil = 50;
            toTarget = 1;
        }

        ZpGameSceneSet.Instance.EnableSP(id, ZpGameContant.StereoCameraType.Dynamic, toPupil, toTarget, Target, BindCamera);
        ZpGameAndroidApi.enterSScene(id);
    }

	/// <summary>
	/// Disables the stereo parameter.
	/// </summary>
	/// <param name="cam">Cam.</param>
    public static void DisableStereoParameter(Camera cam)
    {
        ZpGameSceneSet.Instance.DisableSP(cam);
    }

	/// <summary>
	/// Sets the target.
	/// </summary>
	/// <param name="Target">Target.</param>
    public static void SetTarget(Transform Target)
    {
        ZpGameSceneSet.Instance.SetTarget(Target);
    }

	/// <summary>
	/// Gets the type of the screen.
	/// </summary>
	/// <returns>The screen type.</returns>
    public static ZpGameContant.AEffects getScreenType()
    {
#if UNITY_EDITOR
        return (ZpGameContant.AEffects)ZpGameAndroidApi.getScreenType();
#else

        if (EyeTrack.Instance.IsSupportEyeTrack)
        {
            return ZpGameContant.AEffects.SlantWeaved;
        }
        else
        {
            return (ZpGameContant.AEffects)ZpGameAndroidApi.getScreenType();
        }
#endif
    }
#else
    public static void AttachStereoToCamera(Camera Cam, string id)
    { }

    public static void SwitchStereoPara(string id)
    { }

    public static void SwitchStereoPara(string id, float pupil, float focal)
    { }

    public static void SwitchStereoPara(string id, float toPupil, float toTarget, Transform Target)
    { }

    public static void SetTarget(Transform Target)
    { }

    public static void CaptureScreen(string path)
    {
        Application.CaptureScreenshot(path, 0);
    }
#endif
}
