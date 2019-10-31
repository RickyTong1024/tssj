
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;
using ZpGame;
#if !UNITY_IOS
[RequireComponent(typeof(Camera))]
public class ZpGameStereoCamera : MonoBehaviour {

#region Value
#region Public member
    public bool _Debug = false;
    public string _SPUid;
    public ZpGameContant.AEffects _ScreenType;
    public ZpGameContant.StereoCameraType _SCameraCtlType = ZpGameContant.StereoCameraType.Static;
    public float _Pupil = 0.065f;
    [Range(0.01f,Mathf.Infinity)]
    public float _Focal = 3f;

    public bool _IsTargetMode = false;
    public Transform _Target;
    public float _ToPupil = 50.0f;
    public float _ToFocal = 1.0f;

    public bool IsCullCtr = false;
    public int _DefaultCullingMaskOff = 0;
    public int _DefaultCullingMask = 0;
#endregion

#region SCRIPT_STATE
	private SInit _Init = SInit.initable;
	private enum SInit
	{
		UNinitable = 0,
		initable = 1,
		initing = 2,
		inited = 3,
	}
#endregion

#region EYEINTERVAL
    private float _ApplyPupil = 0.04f;
    private float _Percent = 1.0f;
#endregion

#region CAMERA_RELATION
    private Camera _MainCamera;
	private int _MainCullingMask;
    private CameraClearFlags _MainClearFlag;
    private CameraClearFlags _DefaultClearFlag = CameraClearFlags.Nothing;
    private float _MainNear;
    private Vector3 _BestNearPlan;
    private GameObject[] _LRCameras = new GameObject[2];
	private  Camera[] _LRCameraComponents = new Camera[2];
    private RenderTexture[] _LRRenderTextures = new RenderTexture[2]; 
    private int _Antialiasing = 2;
	private ZpGamePostRender _PostRender;
    private bool _Is3DTrig = true;

    private StereoCameraState _CurState = StereoCameraState.Disable;
    enum StereoCameraState
    {
        Disable,
        Enable
    }
#endregion

#region EXTENSION_FUNC_CONTROL
    private bool _IsSupportOutSrc = false;
	public bool _IsSupportCopy = false;
#endregion

#region Getter & Setter
    /* GETTER || SETTER */
    public int AntiAliasing
    {
        set { _Antialiasing = Mathf.Clamp(Mathf.ClosestPowerOfTwo(value), 0, 8); }
        get { return _Antialiasing; }
    }

    public float Percent
    {
        set { _Percent = value < 0 ? 0.0f : value; }
        get { return _Percent; }
    }

	public int MainCullingMask
	{
		set { _MainCullingMask = value; }
		get { return _MainCullingMask; }
	}

    public int DefaultCullingMask
    {
        set
        {
            _DefaultCullingMask = value;
            //if(_MainCamera!=null)
            //    _MainCamera.cullingMask = value;
        }
        get { return _DefaultCullingMask; }
    }

#endregion
#endregion

#region Function
#region MONO_FUNC
    void Awake()
    {
        _MainCamera = transform.GetComponent<Camera>();      
    }

    void ToEnable()
    {
        if (_CurState != StereoCameraState.Enable)
        {
            Enable();
            _CurState = StereoCameraState.Enable;
        }
    }

    void ToDisable()
    {
        if (_CurState != StereoCameraState.Disable)
        {
            Disable();
            _CurState = StereoCameraState.Disable;
        }
    }

    void Enable()
    {
        SwitchStereoPara();
    }

    void Disable()
    {
        ZpGameDevelopApi.DisableStereoParameter(_MainCamera);
    }

    void OnEnable(){
        ToEnable();
    }

    private void OnDisable()
    {
		Debugger.Log ("OnDisable remove camera");
        ToDisable();
    }

    public void SwitchStereoPara()
    {
        if (_SCameraCtlType == ZpGameContant.StereoCameraType.Static)
            ZpGameDevelopApi.SwitchStereoPara(_SPUid, _Pupil, _Focal, _MainCamera);
        else if (_SCameraCtlType == ZpGameContant.StereoCameraType.Dynamic)
            ZpGameDevelopApi.SwitchStereoPara(_SPUid, _ToPupil, _ToFocal, _Target, _MainCamera);
    }

    private void StereoReset()
    {
        SPitem tmp = ZpGameSceneSet.Instance.GetCameraSP(_MainCamera);
        if (tmp != null)
        {
            _SPUid = tmp.SPUid;
            _SCameraCtlType = tmp.CameraCtlType;
            _Pupil = tmp.Pupil;
            _Focal = tmp.Focal;
            _ToPupil = tmp.ToPupil;
            _ToFocal = tmp.ToFocal;
        }
        else
        {
            ZpGameLog.LogWarning("reset get nul");
        }
		if (ZpGameStatic.IsInit == true) {
			CheckScreenType ();
			Init ();
			CopyComponent ();
		}
    }

    // Use this for initialization
    void Start () {

		if (ZpGameStatic.IsInit == true) {
			CheckScreenType ();
			Init ();
			CopyComponent ();
		}
    }

	void CheckScreenType()
	{
		if (_Init <= SInit.initable ) {
			_ScreenType = ZpGameDevelopApi.getScreenType ();
			if (_ScreenType == ZpGameContant.AEffects.Original) {
				_Init = SInit.UNinitable;
			} else {
				_Init = SInit.initable;
			}
		}
	}

	void Init()
	{
		if (_Init == SInit.initable) {
			_Init = SInit.initing;
			for (int i = 0; i < _LRCameras.Length; i++) {
				_LRCameras [i] = new GameObject ("LRCamera_" + (i + 1), typeof(Camera));
				_LRCameraComponents [i] = _LRCameras [i].GetComponent<Camera> ();
				_LRCameras [i].GetComponent<Camera> ().CopyFrom (_MainCamera);
				_LRCameras [i].transform.parent = _MainCamera.transform;
				_LRCameras [i].GetComponent<Camera> ().clearFlags = _MainCamera.clearFlags;
			
			}
		
			if (ZpGameSceneSet.Instance.IsTextureCreate () == false) {
				ZpGameSceneSet.Instance.CreateEyeTexture ();
			}
			_LRRenderTextures [0] = ZpGameSceneSet.Instance.GetEyeTexture (ZpGameSceneSet.SEyes.Left);
			_LRRenderTextures [1] = ZpGameSceneSet.Instance.GetEyeTexture (ZpGameSceneSet.SEyes.Right);
			_LRCameraComponents[0].targetTexture = _LRRenderTextures [0];
			_LRCameraComponents[1].targetTexture = _LRRenderTextures [1];
		
			_MainCullingMask = _MainCamera.cullingMask;
			_MainCamera.cullingMask = _DefaultCullingMask;
			_MainClearFlag = _MainCamera.clearFlags;
			_MainCamera.clearFlags = _DefaultClearFlag;
			_MainNear = _MainCamera.nearClipPlane;

            _PostRender = UnityEngine.Object.FindObjectOfType<ZpGamePostRender>();
            if (_PostRender == null)
            {
                GameObject _PostRenderobj = new GameObject("ZpGamePostRender", typeof(ZpGamePostRender));
                _PostRender = _PostRenderobj.GetComponent<ZpGamePostRender>();
                UnityEngine.Object.DontDestroyOnLoad(_PostRender);
            }

            _Init = SInit.inited;
		}
	}

	void CopyComponent()
	{
		if (_Init == SInit.inited) {
			if (_IsSupportCopy) {
				MonoBehaviour[] coms;
				coms = _MainCamera.GetComponents<MonoBehaviour> ();

				foreach (MonoBehaviour tmp in coms) {
					if (tmp.enabled) {
						CopyComponent (tmp, _LRCameras [0]);
						CopyComponent (tmp, _LRCameras [1]);
					}
				}
                _IsSupportCopy = false;

            }
		}
	}

	public void updateState ()
	{
		_ScreenType = ZpGameDevelopApi.getScreenType ();
	}
    // Update is called once per frame
    void Update () {
		if (_Init == SInit.inited) {
			UpdateStereoCameraParameters ();
			ComputeFinalParameters ();
			UpdateCameraSettings ();
			SelectShaderPass ();
		}

    }

    void LateUpdate()
    {
		if (_Init == SInit.inited) {
			UpdateView ();
		}
    }

    /// <summary>
    /// OnDrawGizmos FUNC draw the gizmos in the unity editor for telling developer the best scope in 3D space on stereocopic
    /// </summary>
    void OnDrawGizmos()
    {
        if (_MainCamera != null)
        {
            StereoCameraGizmos.DrawFrustumGizmos(_MainCamera);
            StereoCameraGizmos.DrawNearPlane(_MainCamera, _Focal, _ApplyPupil);
            StereoCameraGizmos.DrawScreenPlane(_MainCamera, _Focal);
            StereoCameraGizmos.DrawFarPlane(_MainCamera, _Focal, _ApplyPupil);
        }
    }
#endregion

#region OTHER_FUNC
    private void UpdateStereoCameraParameters()
    {
        _IsTargetMode = ZpGameSceneSet.Instance.IsTargetSet();
        if (_IsTargetMode == true)
            _Target = ZpGameSceneSet.Instance.GetTarget();
        else
            _Target = null;

        _Percent = ZpGameSceneSet.Instance.GetPercent();
    }

    private void ComputeFinalParameters()
    {
        if (_SCameraCtlType == ZpGameContant.StereoCameraType.Static)
        {
            _ApplyPupil = _Pupil * _Percent;
            if (_Focal <= 0)
                _Focal = 0.01f;
        }

        if (_SCameraCtlType == ZpGameContant.StereoCameraType.Dynamic)
        {
            if (_Target!= null)
            {
                _Focal = _ToFocal * Vector3.Distance(_MainCamera.transform.position, _Target.position);
                _Pupil = _Focal / _ToPupil;
                _ApplyPupil = _Pupil * _Percent;
            }
            else
            {

                _Focal = 0.01f;
                _Pupil = 0f;
                _ApplyPupil = 0;
            }
        }

        if (_ApplyPupil - 0.000000001 < 0)
        {
            _Is3DTrig = false;
        }
        else
        {
            _Is3DTrig = true;
        }

        if (_IsSupportOutSrc)
        {
            if (_ApplyPupil - 0.00001f > 0)
            {
                Shader.SetGlobalVector("_GlobalStereo", new Vector4(_ApplyPupil * 0.5f, _Focal, _BestNearPlan.z, 1.0f));
            }
            else
            {
                Shader.SetGlobalVector("_GlobalStereo", new Vector4(_ApplyPupil * 0.5f, _Focal, _BestNearPlan.z, 0.0f));
            }
        }

        _BestNearPlan = StereoCameraGizmos.GetNearPlane(_ApplyPupil, _Focal, _MainCamera.fieldOfView, _MainCamera.aspect);

    }

    void UpdateCameraSettings()
    {

        if (_MainCamera.cullingMask != _DefaultCullingMask)
        {
            //_MainCullingMask = _MainCamera.cullingMask;
            _MainCamera.cullingMask = _DefaultCullingMask;
        }

        if (_MainCamera.clearFlags != _DefaultClearFlag)
        {
            _MainClearFlag = _MainCamera.clearFlags;
            _MainCamera.clearFlags = _DefaultClearFlag;
        }

        for (int i = 0; i < _LRCameras.Length; i++)
        {
			_LRCameraComponents[i].backgroundColor = _MainCamera.backgroundColor;
			_LRCameraComponents[i].clearFlags = _MainClearFlag;
			_LRCameraComponents[i].cullingMask = _MainCullingMask;
        }

        if (_ScreenType == ZpGameContant.AEffects.SlantWeaved)
        {
            if (_Is3DTrig == false || _MainCamera.isActiveAndEnabled == false || EyeTrack.Instance.Is3DOn == false|| _MainCamera.orthographic == true)
            {
                close3DRender();
            }
            else
            {
                ToEnable();
            }
        }
        else
        {
            if (_Is3DTrig == false || _MainCamera.isActiveAndEnabled == false || _MainCamera.orthographic == true)
            {
                close3DRender();
            }
            else
            {
                ToEnable();
            }
        }
    }
    void close3DRender()
    {
        try
        {
            _LRCameraComponents[0].clearFlags = CameraClearFlags.Nothing;
            _LRCameraComponents[1].clearFlags = CameraClearFlags.Nothing;
            _LRCameraComponents[0].cullingMask = 0;
            _LRCameraComponents[1].cullingMask = 0;
            _MainCamera.cullingMask = _MainCullingMask;
            _MainCamera.clearFlags = _MainClearFlag;
            ToDisable();
        }
        catch { }
       
    }
    void SelectShaderPass()
    {
		_LRRenderTextures[0] = ZpGameSceneSet.Instance.GetEyeTexture(ZpGameSceneSet.SEyes.Left);
		_LRRenderTextures[1] = ZpGameSceneSet.Instance.GetEyeTexture(ZpGameSceneSet.SEyes.Right);
		_LRCameraComponents[0].targetTexture = _LRRenderTextures [0];
		_LRCameraComponents[1].targetTexture = _LRRenderTextures [1];

        if (_ScreenType == ZpGameContant.AEffects.SlantWeaved)
        {

            if ( _Is3DTrig == true)
            {
                EyeTrack.Instance.EyeTraceResume();
            }
            else
            {
                EyeTrack.Instance.EyeTracePause();
            }
        }
    }

    private void UpdateView()
    {
        for (int i = 0; i < _LRCameras.Length; i++)
        {
			_LRCameraComponents[i].depth = _MainCamera.depth;
            _LRCameras[i].transform.position = _MainCamera.transform.position + _MainCamera.transform.TransformDirection((i == 0 ? -1 : 1) * 0.5f * _ApplyPupil, 0.0f, 0.0f);
            _LRCameras[i].transform.rotation = _MainCamera.transform.rotation;
        }
		_LRCameraComponents[0].projectionMatrix = FrustumShift(0);
		_LRCameraComponents[1].projectionMatrix = FrustumShift(1);
    }

    private Matrix4x4 PerspectiveOffCenter(float left, float right, float bottom, float top, float near, float far)
    {
        float x = (2.0f * near) / (right - left);
        float y = (2.0f * near) / (top - bottom);
        float a = (right + left) / (right - left);
        float b = (top + bottom) / (top - bottom);
        float c = -(far + near) / (far - near);
        float d = -(2.0f * far * near) / (far - near);
        float e = -1.0f;

        Matrix4x4 m = new Matrix4x4();
        m[0, 0] = x; m[0, 1] = 0f; m[0, 2] = a; m[0, 3] = 0f;
        m[1, 0] = 0f; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0f;
        m[2, 0] = 0f; m[2, 1] = 0f; m[2, 2] = c; m[2, 3] = d;
        m[3, 0] = 0f; m[3, 1] = 0f; m[3, 2] = e; m[3, 3] = 0f;

        return m;
    }

    private Matrix4x4 FrustumShift(int cam)
    {
        float left;
        float right;
        float a;
        float b;
        float fov;

        fov = _MainCamera.fieldOfView / 180.0f * Mathf.PI;
        float aspect = _MainCamera.aspect;

        a = _MainCamera.nearClipPlane * Mathf.Tan(fov * 0.5f);
        b = _MainCamera.nearClipPlane / _Focal;

        left = -aspect * a + 0.0f * b;
        right = aspect * a + 0.0f * b;


        if (cam == 0)  // left cam
        {
            left = -aspect * a + _ApplyPupil * b / 2.0f;
            right = aspect * a + _ApplyPupil * b / 2.0f;
        }
        else if (cam == 1)  // right cam
        {
            left = -aspect * a - _ApplyPupil * b / 2.0f;
            right = aspect * a - _ApplyPupil * b / 2.0f;
        }

       
        return PerspectiveOffCenter(left, right, -a, a, _MainCamera.nearClipPlane, _MainCamera.farClipPlane);
    }

    T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();

        if (type != this.GetType())
        {
            Component copy = destination.AddComponent(type);
            System.Reflection.FieldInfo[] fields = type.GetFields();
            foreach (System.Reflection.FieldInfo field in fields)
            {
                field.SetValue(copy, field.GetValue(original));
            }
            return copy as T;
        }
        return null;
    }
#endregion
}
#endregion

#else
public class ZpGameStereoCamera : MonoBehaviour
{
}
#endif