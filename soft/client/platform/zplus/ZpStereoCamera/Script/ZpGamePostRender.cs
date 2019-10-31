#if !UNITY_IOS
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ZpGame;

[RequireComponent (typeof(Camera))]
public class ZpGamePostRender : MonoBehaviour
{

#region Import_lib

#if UNITY_EDITOR
	[DllImport ("wzweaving")]
#elif UNITY_ANDROID
        [DllImport("wzweavingarm32")]
#else
        [DllImport("wzweaving")]
#endif
    private static extern System.IntPtr GetRenderEventFunc ();

#if UNITY_EDITOR
	[DllImport ("wzweaving")]
#elif UNITY_ANDROID
        [DllImport("wzweavingarm32")]
#else
        [DllImport("wzweaving")]
#endif
    private static extern void Weave2 (System.IntPtr srcTexture, System.IntPtr srcTexture2, float[] paras);

#if UNITY_EDITOR
	[DllImport ("wzweaving")]
#elif UNITY_ANDROID
        [DllImport("wzweavingarm32")]
#else
        [DllImport("wzweaving")]
#endif
    private static extern void Weave (System.IntPtr srcTexture, float[] paras);

#endregion


	public Material _WeavedMaterial;
	private ZpGameContant.AEffects _ScreenType;
	private Shader _Weaved;
	private RenderTexture[] _LRRenderTextures = new RenderTexture[2];

#region EYE_TRACK

    float[] ShaderPara;

#endregion


	void Awake ()
	{
		awake ();
	}

	private void awake ()
	{
		//ZpStereoManage.Instance.log();
		_ScreenType = (ZpGameContant.AEffects)ZpGameDevelopApi.getScreenType ();
		if (_ScreenType == ZpGameContant.AEffects.Original) {
			Destroy (this);
			return;
		}

		switch (_ScreenType) {
		case ZpGameContant.AEffects.LRWeaved:
		case ZpGameContant.AEffects.RLWeaved:
			_Weaved = Shader.Find ("ZpGame/Weaved") as Shader;
			break;
		case ZpGameContant.AEffects.SlantWeaved:
			_Weaved = Shader.Find ("ZpGame/Weaved") as Shader;
			break;
		case ZpGameContant.AEffects.LRSBS:
			_Weaved = Shader.Find ("ZpGame/WeavedSBS") as Shader;
			break;
		default:
			break;
		}

		if (_Weaved != null)
			_WeavedMaterial = new Material (_Weaved);

		if (_ScreenType == ZpGameContant.AEffects.SlantWeaved) {
			ShaderPara = EyeTrack.Instance.getEyetrackData ();
		}
	}

	// Use this for initialization
	void Start ()
	{
		start ();
	}

	void start ()
	{

		transform.GetComponent<Camera> ().clearFlags = CameraClearFlags.Depth;
		transform.GetComponent<Camera> ().cullingMask = 0;
		if (ZpGameSceneSet.Instance.IsTextureCreate () == false) {
			ZpGameSceneSet.Instance.CreateEyeTexture ();
		}

		//_LRCameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
		//_LRCameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1);
		_LRRenderTextures [0] = ZpGameSceneSet.Instance.GetEyeTexture (ZpGameSceneSet.SEyes.Left);
		_LRRenderTextures [1] = ZpGameSceneSet.Instance.GetEyeTexture (ZpGameSceneSet.SEyes.Right);
		_WeavedMaterial.SetTexture ("_LTexture", _LRRenderTextures [0]);
		_WeavedMaterial.SetTexture ("_RTexture", _LRRenderTextures [1]);

		Debugger.Log ("screen type " + _ScreenType);
		if (_ScreenType == ZpGameContant.AEffects.RLWeaved)
			_WeavedMaterial.SetInt ("_SwitchLR", 1);
		else if (_ScreenType == ZpGameContant.AEffects.LRWeaved)
			_WeavedMaterial.SetInt ("_SwitchLR", 0);
	}

	private void StereoReset ()
	{
	//Debugger.Log ("ZpGamePostRender Reset ------");
		awake ();
		start ();
	}
	// Update is called once per frame
	void Update ()
	{
		transform.GetComponent<Camera> ().depth = ZpGameSceneSet.Instance.GetCameraDepth ();
	}

	void OnApplicationFocus (bool hasFocus)
	{
		//ZpGameLog.Log("OnApplicationFocus " + hasFocus);
		if (_ScreenType == ZpGameContant.AEffects.SlantWeaved) {
			if (hasFocus == true)
				EyeTrack.Instance.FocusOn ();
			else
				EyeTrack.Instance.FocusOut ();
		}
	}

	private void OnRenderImage (RenderTexture src, RenderTexture dst)
	{
		if (ZpGameSceneSet.Instance.IsFloat3DTrig == true) {
			switch (_ScreenType) {
			case ZpGameContant.AEffects.LRWeaved:
			case ZpGameContant.AEffects.RLWeaved:
				WeaveStraight (src, dst);
				break;
			case ZpGameContant.AEffects.SlantWeaved:
				WeaveSlant (src, dst);
				break;
			case ZpGameContant.AEffects.LRSBS:
				WeaveSBS (src, dst);
				break;
			case ZpGameContant.AEffects.Original:
			case ZpGameContant.AEffects.TwoDWithZ:
				break;
			}
		}

	}

	private void WeaveSBS (RenderTexture src, RenderTexture dst)
	{
		_WeavedMaterial.SetTexture ("_LTexture", _LRRenderTextures [0]);
		_WeavedMaterial.SetTexture ("_RTexture", _LRRenderTextures [1]);

		Graphics.Blit (_LRRenderTextures [0], dst, _WeavedMaterial);
	}

	private void WeaveStraight (RenderTexture src, RenderTexture dst)
	{
		_WeavedMaterial.SetTexture ("_LTexture", _LRRenderTextures [0]);
		_WeavedMaterial.SetTexture ("_RTexture", _LRRenderTextures [1]);

		Graphics.Blit (_LRRenderTextures [0], dst, _WeavedMaterial);
	}

	float[] paras = new float[20];

	private void WeaveSlant (RenderTexture src, RenderTexture dst)
	{
		if (EyeTrack.Instance.Is3DOn == true) {
			ShaderPara = EyeTrack.Instance.getEyetrackData ();

			Graphics.SetRenderTarget (dst);

            if (ShaderPara[0] == 6)
            {
                if (ShaderPara[1] != 0 || ShaderPara[2] != 0)
                {
                    paras[0] = ShaderPara[1];
                    paras[1] = ShaderPara[2];
                    paras[2] = ShaderPara[3];
                    paras[3] = ShaderPara[4];
                    paras[4] = ShaderPara[5];
                    paras[5] = Screen.width;
                    paras[6] = Screen.height;
                    paras[7] = ShaderPara[6];
                }
            }

            if (ShaderPara[0] > 6)
            {
                paras[0] = 200.0f;
                paras[1] = ShaderPara[0];

                paras[2] = ShaderPara[1];
                paras[3] = ShaderPara[2];
                paras[4] = ShaderPara[3];
                paras[5] = ShaderPara[4];
                paras[6] = ShaderPara[5];
                paras[7] = Screen.width > Screen.height?Screen.width:Screen.height;
                paras[8] = Screen.height > Screen.width ? Screen.width : Screen.height;
                paras[9] = ShaderPara[6];

                for (int i = 0; i < ShaderPara[0] - 6; i++)
                {
                    paras[10 + i] = ShaderPara[7 + i];
                }
            }

			Weave2 (_LRRenderTextures [0].GetNativeTexturePtr (), _LRRenderTextures [1].GetNativeTexturePtr (), paras);
#if UNITY_5_3_OR_NEWER
			GL.IssuePluginEvent (GetRenderEventFunc (), 1);
#else
            GL.IssuePluginEvent(1);
#endif
		}
	}
}
#endif