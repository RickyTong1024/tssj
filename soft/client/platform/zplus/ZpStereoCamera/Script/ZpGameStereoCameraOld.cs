#if !UNITY_IOS
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class ZpGameStereoCameraOld : MonoBehaviour {

 #region Import_lib
    #if UNITY_EDITOR
        [DllImport("wzweaving")]
    #elif UNITY_ANDROID
        [DllImport("wzweavingarm32")]
    #else
        [DllImport("wzweaving")]
    #endif
        private static extern System.IntPtr GetRenderEventFunc();

    #if UNITY_EDITOR
        [DllImport("wzweaving")]
    #elif UNITY_ANDROID
        [DllImport("wzweavingarm32")]
    #else
        [DllImport("wzweaving")]
    #endif
        private static extern void Weave2(System.IntPtr srcTexture, System.IntPtr srcTexture2, float[] paras);

    #if UNITY_EDITOR
        [DllImport("wzweaving")]
    #elif UNITY_ANDROID
        [DllImport("wzweavingarm32")]
    #else
        [DllImport("wzweaving")]
    #endif
        private static extern void Weave(System.IntPtr srcTexture, float[] paras);

 #endregion

 #region Value
    #region Public member
    public bool _Debug = false;
    public string _SPUid;
    public ZpGameContant.AEffects _ScreenType;
    public Material _WeavedMaterial;

    public ZpGameContant.StereoCameraType _SCameraCtlType = ZpGameContant.StereoCameraType.None;
    public float _Pupil = 0.065f;
    [Range(0.01f,Mathf.Infinity)]
    public float _Focal = 3f;

    public bool _IsTargetMode = false;
    public Transform _Target;
    public float _ToPupil = 50.0f;
    public float _ToFocal = 1.0f;
    #endregion

    private float _ApplyPupil = 0.04f;
    private float _Percent = 1.0f;

    #region CAMERA_RELATION
    private Camera _MainCamera;
    private int _MainCullingMask;
    private int _DefaultCullingMask = 0;
    private CameraClearFlags _MainClearFlag;
    private CameraClearFlags _DefaultClearFlag = CameraClearFlags.Nothing;
    private float _MainNear;
    private Vector3 _BestNearPlan;
    private GameObject[] _LRCameras = new GameObject[2];
    private RenderTexture[] _LRRenderTextures = new RenderTexture[2]; 
    private int _Antialiasing = 2;
    #endregion

    #region SHADER_RELATION
    private Shader _Weaved;
    //private int _CurrentPass = 0; //this is the shader pass we choice currently
    #endregion

    #region EYE_TRACK
    float[] ShaderPara;
    #endregion

    #region EXTENSION_FUNC_CONTROL
    private bool _IsSupportClip = false;
    private float _ClipDistance = 1.0f;

    private bool _IsSupportOutSrc = false;
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

    #endregion
    #endregion

#region Function
    #region MONO_FUNC
    void Awake()
    {
        _ScreenType = (ZpGameContant.AEffects)ZpGameDevelopApi.getScreenType();
        ZpGameLog.LogWarning("screen Type " + _ScreenType);
        if (_ScreenType == ZpGameContant.AEffects.Original)
        {
            Destroy(this);
            return;
        }

        switch (_ScreenType)
        {
            case ZpGameContant.AEffects.LRWeaved:
            case ZpGameContant.AEffects.RLWeaved:
                _Weaved = Resources.Load("Material/Weaved", typeof(Shader)) as Shader;
                break;
            case ZpGameContant.AEffects.SlantWeaved:
                _Weaved = Resources.Load("Material/Weaved", typeof(Shader)) as Shader;
                break;
            default:
                break;
        } 

        if (_Weaved != null)
            _WeavedMaterial = new Material(_Weaved);
    }

    void OnEnable(){
        if (_SCameraCtlType == ZpGameContant.StereoCameraType.Static)
            ZpGameDevelopApi.SwitchStereoPara(_SPUid, _Pupil, _Focal);
        else if (_SCameraCtlType == ZpGameContant.StereoCameraType.Dynamic)
            ZpGameDevelopApi.SwitchStereoPara(_SPUid, _ToPupil, _ToFocal, _Target);

        if (_ScreenType == ZpGameContant.AEffects.SlantWeaved)
        {
            ShaderPara = EyeTrack.Instance.getEyetrackData();
        }
    }

    // Use this for initialization
    void Start () {
  

        _MainCamera = transform.GetComponent<Camera>();

        //MonoBehaviour[] coms;
        //coms = _MainCamera.GetComponents<MonoBehaviour>();

        for (int i = 0; i < _LRCameras.Length; i++)
        {
            _LRCameras[i] = new GameObject("LRCamera_" + (i + 1), typeof(Camera));
            _LRCameras[i].GetComponent<Camera>().CopyFrom(_MainCamera);
            _LRCameras[i].transform.parent = _MainCamera.transform;
            _LRCameras[i].GetComponent<Camera>().clearFlags = _MainCamera.clearFlags;

            //_LRRenderTextures[i] = new RenderTexture((int)_MainCamera.pixelWidth / 2, (int)_MainCamera.pixelHeight, 16, RenderTextureFormat.ARGB32);
            //_LRRenderTextures[i].filterMode = FilterMode.Bilinear;
            //_LRRenderTextures[i].antiAliasing = _Antialiasing;
            //_LRCameras[i].GetComponent<Camera>().targetTexture =  GetEyeTexture[i];

            //foreach (MonoBehaviour tmp in coms)
            //{
            //    if (tmp.enabled)
            //        CopyComponent(tmp, _LRCameras[i]);
            //}
        }

        
        _LRRenderTextures[0] = ZpGameSceneSet.Instance.GetEyeTexture(ZpGameSceneSet.SEyes.Left);
        _LRRenderTextures[1] = ZpGameSceneSet.Instance.GetEyeTexture(ZpGameSceneSet.SEyes.Right);
        _LRCameras[0].GetComponent<Camera>().targetTexture = _LRRenderTextures[0];
        _LRCameras[1].GetComponent<Camera>().targetTexture = _LRRenderTextures[1];
        //_LRCameras[0].GetComponent<Camera>().rect = new Rect(0, 0, 0.5f, 1);
        //_LRCameras[1].GetComponent<Camera>().rect = new Rect(0.5f, 0, 0.5f, 1);

        _WeavedMaterial.SetTexture("_LTexture", _LRRenderTextures[0]);
        _WeavedMaterial.SetTexture("_RTexture", _LRRenderTextures[1]);

        if (_ScreenType == ZpGameContant.AEffects.RLWeaved)
            _WeavedMaterial.SetInt("_SwitchLR",1);
        else if(_ScreenType == ZpGameContant.AEffects.LRWeaved)
            _WeavedMaterial.SetInt("_SwitchLR", 0);

        UpdateView();

        _MainCullingMask = _MainCamera.cullingMask;
        _MainCamera.cullingMask = _DefaultCullingMask;
        _MainClearFlag = _MainCamera.clearFlags;
        _MainCamera.clearFlags = _DefaultClearFlag;
        _MainNear = _MainCamera.nearClipPlane;
    }

    // Update is called once per frame
    void Update () {

        UpdateStereoCameraParameters();
        ComputeFinalParameters();
        UpdateCameraSettings();
        SelectShaderPass();

    }

    void LateUpdate()
    {
        UpdateView();
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

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        switch (_ScreenType)
        {
            case ZpGameContant.AEffects.LRWeaved:
            case ZpGameContant.AEffects.RLWeaved:
                WeaveStraight(src, dst);
                break;
            case ZpGameContant.AEffects.SlantWeaved:
                WeaveSlant(src, dst);
                break;
            case ZpGameContant.AEffects.Original:
            case ZpGameContant.AEffects.TwoDWithZ:
                break;
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        //ZpGameLog.Log("OnApplicationFocus " + hasFocus);
        if (_ScreenType == ZpGameContant.AEffects.SlantWeaved)
        {
            if (hasFocus == true)
                EyeTrack.Instance.FocusOn();
            else
                EyeTrack.Instance.FocusOut();
        }
    }

    #endregion

    #region OTHER_FUNC

    private void WeaveStraight(RenderTexture src, RenderTexture dst)
    {
        _WeavedMaterial.SetTexture("_LTexture", _LRRenderTextures[0]);
        _WeavedMaterial.SetTexture("_RTexture", _LRRenderTextures[1]);

        if (ZpGameSceneSet.Instance.IsFloat3DTrig  == true)
        {
            Graphics.Blit(src, dst, _WeavedMaterial);
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }
    /*
    private void WeaveStraightBg(RenderTexture src, RenderTexture dst)
    {
        _WeavedMaterial.SetTexture("_LTexture", _LRRenderTextures[0]);
        _WeavedMaterial.SetTexture("_RTexture", _LRRenderTextures[1]);

        if (_EnableWeave == true)
        {
            _PreTex.mipMapBias = 0;
            _WeavedMaterial.SetTexture("_LRTexture", _PreTex);
            _WeavedMaterial.SetInt("_CloseWeave", 0);
            Graphics.Blit(src, dst, _WeavedMaterial);
        }
        else
        {
            _WeavedMaterial.SetTexture("_LRTexture", _OriTex);
            _WeavedMaterial.SetInt("_CloseWeave", 1);
            Graphics.Blit(src, dst, _WeavedMaterial);
        }
    }
    */
    float[] paras = new float[8];
    private void WeaveSlant(RenderTexture src, RenderTexture dst)
    {
        if (ZpGameSceneSet.Instance.IsFloat3DTrig && EyeTrack.Instance.Is3DOn)
        {
            Graphics.SetRenderTarget(dst);
            paras[5] = Screen.width;
            paras[6] = Screen.height;

            if (ShaderPara[0] != 0 || ShaderPara[1] != 0)
            {
                paras[0] =  ShaderPara[0];
                paras[1] =  ShaderPara[1] ;
                paras[2] = ShaderPara[2];
                paras[3] = ShaderPara[3];
                paras[4] =  ShaderPara[4];
                paras[5] = Screen.width;
                paras[6] = Screen.height;
                paras[7] = 0;
            }
            else
            {
                paras[0] = 4.999f;
                paras[1] = -0.4f;
            }
            Weave2(_LRRenderTextures[0].GetNativeTexturePtr(), _LRRenderTextures[1].GetNativeTexturePtr(), paras);
#if UNITY_5_3
            GL.IssuePluginEvent(GetRenderEventFunc(), 1);
#endif
        }
        else
        {
            Graphics.Blit(src, dst);
        }
    }

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
                _Pupil = _ToFocal * Vector3.Distance(_MainCamera.transform.position, _Target.position) / _ToPupil;
                _ApplyPupil = _Pupil * _Percent;
            }
            else
            {

                _Focal = 0.01f;
                _Pupil = 0f;
                _ApplyPupil = 0;
            }
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
        if (_MainCamera.cullingMask != _DefaultCullingMask )
        {
            _MainCullingMask = _MainCamera.cullingMask;
            _MainCamera.cullingMask = _DefaultCullingMask;
        }
        if (_MainCamera.clearFlags != _DefaultClearFlag)
        {
            _MainClearFlag = _MainCamera.clearFlags;
            _MainCamera.clearFlags = _DefaultClearFlag;
        }

        for (int i = 0; i < _LRCameras.Length; i++)
        {
            _LRCameras[i].GetComponent<Camera>().backgroundColor = _MainCamera.backgroundColor;
            _LRCameras[i].GetComponent<Camera>().clearFlags = _MainClearFlag;
            _LRCameras[i].GetComponent<Camera>().cullingMask = _MainCullingMask;
        }


        if (ZpGameSceneSet.Instance.IsFloat3DTrig == false)
        {
            _LRCameras[0].GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
            _LRCameras[1].GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
            _LRCameras[0].GetComponent<Camera>().cullingMask = 0;
            _LRCameras[1].GetComponent<Camera>().cullingMask = 0;
            _MainCamera.cullingMask = _MainCullingMask;
            _MainCamera.clearFlags = _MainClearFlag;
        }

        if (_ScreenType == ZpGameContant.AEffects.SlantWeaved)
        {
            if(EyeTrack.Instance.Is3DOn == false)
            {
                _LRCameras[0].GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
                _LRCameras[1].GetComponent<Camera>().clearFlags = CameraClearFlags.Nothing;
                _LRCameras[0].GetComponent<Camera>().cullingMask = 0;
                _LRCameras[1].GetComponent<Camera>().cullingMask = 0;
                _MainCamera.cullingMask = _MainCullingMask;
                _MainCamera.clearFlags = _MainClearFlag;
            }
        }


    }

    void SelectShaderPass()
    {
        if (_ScreenType == ZpGameContant.AEffects.LRWeaved || _ScreenType == ZpGameContant.AEffects.RLWeaved)
        {
            _WeavedMaterial.SetTexture("_LTexture", _LRRenderTextures[0]);
            _WeavedMaterial.SetTexture("_RTexture", _LRRenderTextures[1]);
        }
        if (_ScreenType == ZpGameContant.AEffects.SlantWeaved)
        {

            if (ZpGameSceneSet.Instance.IsFloat3DTrig == true)
            {
                EyeTrack.Instance.EyeTraceResume();
            }
            else
            {
                EyeTrack.Instance.EyeTracePause();
            }

            ShaderPara = EyeTrack.Instance.getEyetrackData();
            //if ((ShaderPara.gSlant > -0.01f && ShaderPara.gSlant < 0.01f) || ShaderPara._newPitch < 0.01)
            //{
            //    ShaderPara._newPitch = 4.91147f;
            //    ShaderPara._leftTopViewNo = 13f;
            //    ShaderPara.gSlant = -0.26796f;
            //    ShaderPara.gCameraX = Screen.width / 2; ;
            //    ShaderPara.gCameraY = -50;
            //}
            //_WeavedMaterial.SetFloat("_NewPitch", ShaderPara._newPitch);
            //_WeavedMaterial.SetFloat("_LeftTopViewNo", ShaderPara._leftTopViewNo);
            //_WeavedMaterial.SetFloat("_mSlant", ShaderPara.gSlant);
            //_WeavedMaterial.SetFloat("_cameraX", ShaderPara.gCameraX);
            //_WeavedMaterial.SetFloat("_cameraY", ShaderPara.gCameraY);
            //_WeavedMaterial.SetFloat("screenWidth", Screen.width);
            //_WeavedMaterial.SetFloat("screenHeight", Screen.height);
            //_WeavedMaterial.SetFloat("_BR", 0);

        }
    }

    private void UpdateView()
    {
        for (int i = 0; i < _LRCameras.Length; i++)
        {
            _LRCameras[i].GetComponent<Camera>().depth = _MainCamera.depth;
            _LRCameras[i].transform.position = _MainCamera.transform.position + _MainCamera.transform.TransformDirection((i == 0 ? -1 : 1) * 0.5f * _ApplyPupil, 0.0f, 0.0f);
            _LRCameras[i].transform.rotation = _MainCamera.transform.rotation;
        }
        _LRCameras[0].GetComponent<Camera>().projectionMatrix = FrustumShift(0);
        _LRCameras[1].GetComponent<Camera>().projectionMatrix = FrustumShift(1);
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