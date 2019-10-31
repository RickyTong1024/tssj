#if !UNITY_IOS
using UnityEngine;
using System.Collections;
using System.IO;
using System.Xml;
using System;
using System.Runtime.InteropServices;
using ZpGame;
public class ZpGameSceneSet : ZpUnitySingleton<ZpGameSceneSet>
{
    private string ResFile = "ZpGameCameraSet";
    private string XmlPath = Application.persistentDataPath + "/ZpGameCameraSet.xml";

    private object obj = new object();    

    private ArrayList _StereoParaList;
    private ArrayList _BindCameraList;

    private bool _IsTargetSet = false;
    private Transform _Target;

    #region RenderTextureSet
    private bool IsCreateTexture = false;
    private const int EyeTextureCount = 2;
    private int EyeTextureWidth = Screen.width/2;
    private int EyeTextureHeight = Screen.height;
    private int EyeTextureDepth;
    private RenderTexture[] EyeTextures = new RenderTexture[EyeTextureCount];
    private int[] EyeTextureIds = new int[EyeTextureCount];
    public enum SEyes
    {
        Left = 0,
        Right = 1
    }
#endregion

    private bool _IsTexMode = false;
    private Texture2D _PreTex;

    private bool _Float3DTrig = true;
    private float _Percent = 1.0f;
    private ZpGamePostRender _PostRender;

    public bool IsFloat3DTrig
    {
        get { return _Float3DTrig; }
    }

    public void Begin()
    {
        ZpGameLog.Log("ZpGameSceneSet init");
    }

    public ZpGameSceneSet() {

        _StereoParaList = new ArrayList();
        _BindCameraList = new ArrayList();

        Debugger.Log(XmlPath);

        //_PostRender = UnityEngine.Object.FindObjectOfType<ZpGamePostRender>();
        //if (_PostRender == null)
        //{
        //    GameObject _PostRenderobj = new GameObject("ZpGamePostRender", typeof(ZpGamePostRender));
        //    _PostRender = _PostRenderobj.GetComponent<ZpGamePostRender>();
        //    UnityEngine.Object.DontDestroyOnLoad(_PostRender);
        //}
        //_PostRender.enabled = false;

        if (!File.Exists(XmlPath))
        {
            try
            {
                string Data = Resources.Load(ResFile).ToString();
                XmlDocument Doc = new XmlDocument();
                Doc.LoadXml(Data);
                Doc.Save(XmlPath);
            }
            catch
            {
            }
        }

#if UNITY_STANDALONE_WIN
        if (File.Exists("ZpGameCameraSet.xml"))
        {
            XmlDocument Doc = new XmlDocument();
            Doc.Load("ZpGameCameraSet.xml");
            Doc.Save(XmlPath);
        }
#endif
        loadxml(null);
    }

    #region EYE_TEXTURE
    public void CreateEyeTexture()
    {
        lock (obj)
        {
            if (IsCreateTexture == false)
            {
				EyeTextureWidth = (ZpGameStatic.EyeTextureRect == ZpGameStatic.RenderTextureRect.Half_screen )? Screen.width / 2:Screen.width;
                EyeTextureHeight = Screen.height;
				EyeTextureDepth = (int)ZpGameStatic.EyeTextureDepth;

                for (int index = 0; index < EyeTextureCount; index++)
                {
                    EyeTextures[index] = new RenderTexture(EyeTextureWidth, EyeTextureHeight, EyeTextureDepth, RenderTextureFormat.ARGB32);
                    EyeTextureIds[index] = EyeTextures[index].GetNativeTexturePtr().ToInt32();

                    EyeTextures[index].filterMode = FilterMode.Bilinear;
                    EyeTextures[index].antiAliasing = 2;
                    EyeTextures[index].mipMapBias = 1;
                    EyeTextures[index].useMipMap = false;
                    EyeTextures[index].Create();
                }
                IsCreateTexture = true;
            }
        }
    }

    public RenderTexture GetEyeTexture(SEyes eye)
    {
        return EyeTextures[(int)eye];
    }

    public int GetEyeTextureID(SEyes eye)
    {
        return EyeTextureIds[(int)eye];
    }

    public bool IsTextureCreate()
    {
        return IsCreateTexture;
    }

    #endregion

    public int GetCurrentCC()
    {
        return _BindCameraList.Count;
    }

    public SPitem GetCameraSP(Camera Cur)
    {
        Cameraitem tmp = GetCameraList(Cur);
        if (tmp != null)
            return tmp._CurrentScene;

        return null;
    }

    public SPitem GetCameraSP(int index)
    {
        return ((Cameraitem)_BindCameraList[index])._CurrentScene;
    }


    #region TARGET
    public void SetTarget(Transform target)
    {
        _Target = target;
        if (target != null)
            _IsTargetSet = true;
        else
            _IsTargetSet = false;
    }

    public bool IsTargetSet()
    {
        return _IsTargetSet;
    }

    public Transform GetTarget()
    {
        return _Target;
    }
    #endregion

    #region PRE_TEXTURE
    public void SetTex(Texture2D pretex)
    {
        _PreTex = pretex;
        if (pretex != null)
            _IsTexMode = true;
        else
            _IsTexMode = false;
    }

    public bool IsPreTex()
    {
        return _IsTexMode;
    }

    public Texture2D GetPreTex()
    {
        return _PreTex;
    }
    #endregion

    #region PERCENT
    public void SetPercent(float percent)
    {
        _Percent = percent;
        if (_Percent - 0.00000001 < 0)
            _Float3DTrig = false;
        else
            _Float3DTrig = true;
    }

    public float GetPercent()
    {
        return _Percent;
    }

    #endregion

    public SPitem CheckExist(string SPUid)
    {

        foreach(SPitem tmp in _StereoParaList)
        {
            if (tmp.SPUid == SPUid)
            {
                return tmp;
            }
        }

        return null;
    }

	public void CheckExistAndRep(SPitem NScene)
	{
		for(int index = 0; index < _StereoParaList.Count; index++)
		{
			if (_StereoParaList [index] is SPitem ? true : false) {
				if (((SPitem)_StereoParaList [index]).SPUid == NScene.SPUid) {
					_StereoParaList.RemoveAt (index);
					_StereoParaList.Add (NScene);
					return;
				}
			}
		}

		_StereoParaList.Add (NScene);
	}

    public void AddSScene(SPitem NScene)
    {
		    CheckExistAndRep(NScene);
            CheckCameraList(NScene);
    }

    public void CheckCameraList(Camera Cur, SPitem BindSP)
    {
        Cameraitem tmp = new Cameraitem(Cur, BindSP);

        if (_BindCameraList == null)
        {
            _BindCameraList.Add(tmp);
            return;
        }

        for (int index = 0; index < _BindCameraList.Count; index++)
        {
            if (((Cameraitem)_BindCameraList[index])._CurrentCamera.GetInstanceID() == Cur.GetInstanceID())
            {
                _BindCameraList.RemoveAt(index);
                _BindCameraList.Add(tmp);
                return;
            }
        }

        _BindCameraList.Add(tmp);
    }

    public void CheckCameraList(SPitem BindSP)
    {
        for (int index = 0; index < _BindCameraList.Count; index++)
        {
            if (((Cameraitem)_BindCameraList[index])._CurrentScene.SPUid == BindSP.SPUid)
            {
                ((Cameraitem)_BindCameraList[index])._CurrentScene = BindSP;
            }
        }
    }

    public void RemoveCameraList(Camera Cur)
    {
        for (int index = 0; index < _BindCameraList.Count; index++)
        {
            if (((Cameraitem)_BindCameraList[index])._CurrentCamera.GetInstanceID() == Cur.GetInstanceID())
            {
                _BindCameraList.RemoveAt(index);
            }
        }
    }

    public Cameraitem GetCameraList(Camera Cur)
    {
        for (int index = 0; index < _BindCameraList.Count; index++)
        {
            if (((Cameraitem)_BindCameraList[index])._CurrentCamera.GetInstanceID() == Cur.GetInstanceID())
            {
                return (Cameraitem)_BindCameraList[index];
            }
        }

        return null;
    }

    public Cameraitem GetCameraList()
    {
        if (_BindCameraList.Count != 1)
            ZpGameLog.LogWarning("Current Activity Camera " + _BindCameraList.Count);

        return  (Cameraitem)_BindCameraList[0];
    }

    public float GetCameraDepth()
    {
        float Max = 0;

        if (_BindCameraList.Count > 0)
            Max = ((Cameraitem)_BindCameraList[0])._CurrentCamera.depth;

        for (int index = 0; index < _BindCameraList.Count; index++)
        {
            if (((Cameraitem)_BindCameraList[index])._CurrentCamera.depth > Max)
            {
                Max = ((Cameraitem)_BindCameraList[index])._CurrentCamera.depth;
                return Max;
            }
        }

        return Max;
    }

    public void EnableSP(string SPUid, ZpGameContant.StereoCameraType CameraCtlType, float Pupil, float Intersection, Transform Target, Camera Cur = null)
    {
        SPitem tmp = CheckExist(SPUid);

        if (tmp == null)
        {
            tmp = new SPitem(SPUid, CameraCtlType, Pupil, Intersection);
            _StereoParaList.Add(tmp);
            if (CameraCtlType == ZpGameContant.StereoCameraType.Dynamic && Target != null)
                SetTarget(Target);
        }

        if (_PostRender != null)
        {
            _PostRender.enabled = true;
        }
        else
        {
            _PostRender = UnityEngine.Object.FindObjectOfType<ZpGamePostRender>();
            if (_PostRender != null)
                _PostRender.enabled = true;
        }

        if (Cur != null)
        {
            CheckCameraList(Cur, tmp);
            Cur.gameObject.SendMessage("StereoReset");
        }
        else
        {
            Cameraitem item = GetCameraList();
            if (item != null)
            {
                CheckCameraList(item._CurrentCamera, tmp);
                item._CurrentCamera.gameObject.SendMessage("StereoReset");
            }
        }

        if (CameraCtlType == ZpGameContant.StereoCameraType.Dynamic && Target != null)
            SetTarget(Target);
    }
		
    public void DisableSP(Camera Current)
    {
        RemoveCameraList(Current);
        if (_BindCameraList.Count < 1)
        {
            if (_PostRender != null)
            {
                _PostRender.enabled = false;
            }
            else
            {
                _PostRender = UnityEngine.Object.FindObjectOfType<ZpGamePostRender>();
                if(_PostRender !=null)
                    _PostRender.enabled = false;
            }
        }
    }

	public void reload()
	{
		for (int index = 0; index < _BindCameraList.Count; index++) {
			((Cameraitem)_BindCameraList [index])._CurrentCamera.gameObject.SendMessage ("StereoReset");
		}
		
	}
    public void ZpGamePostRenderReload()
    {
        UnityEngine.Object.FindObjectOfType<ZpGamePostRender>().SendMessage("StereoReset");
    }
    #region XML_OPERATION
    public void saveToCfgFile()
    {
        //新建xml对象  
        XmlDocument xml = new XmlDocument();
        //加入声明  
        xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-8", null));
        //加入根元素  
        xml.AppendChild(xml.CreateElement("ZpGameCfg"));

        //获取根节点  
        XmlNode root = xml.SelectSingleNode("ZpGameCfg");

        foreach (SPitem tmp in _StereoParaList)
        {
            //添加元素  
            XmlElement element = xml.CreateElement("Scene");
            element.SetAttribute("SPUid", tmp.SPUid);
            element.SetAttribute("CameraCtlType", tmp.CameraCtlType.ToString());
            element.SetAttribute("Pupil", tmp.Pupil.ToString());
            element.SetAttribute("Intersection", tmp.Focal.ToString());
            element.SetAttribute("Ipoverride", tmp.ToPupil.ToString());
            element.SetAttribute("DistanceToTarget", tmp.ToFocal.ToString());

            root.AppendChild(element);
        }
        xml.Save(XmlPath);
#if UNITY_EDITOR
        xml.Save("Assets/Resources/ZpGameCameraSet.xml");
#endif
        return;

    }

    public void loadxml(string ExFilePath)
    {
        XmlDocument doc = new XmlDocument();

        try
        {
            if (ExFilePath == null)
                doc.Load(XmlPath);
            else
                doc.Load(ExFilePath);
        }
        catch {
            ZpGameLog.LogWarning("FILE MISS");
            return;
        }
        XmlNode root = doc.SelectSingleNode("ZpGameCfg");

        try
        {
            if (((XmlElement)root).HasAttribute(ZpGameContant.DATA_DebugType))
            {
                string Dtype = ((XmlElement)root).GetAttribute(ZpGameContant.DATA_DebugType);
                if (Dtype != null)
                {
                    if (Dtype == "debug")
                    {
                        ZpGameStatic.DebugType = "debug";
                        Debugger.EnableLog = true;
                    }
                }
            }
            if (((XmlElement)root).HasAttribute(ZpGameContant.DATA_Show_EyeTrace_Log))
            {
                Debug.Log("DATA_Show_EyeTrace_Log true");
                ZpGameStatic.Show_EyeTrace_Log = true;
                ZpGameAndroidApi.enableDebug(true);
            }
            else
            {
                Debug.Log("DATA_Show_EyeTrace_Log false");
                ZpGameStatic.Show_EyeTrace_Log = false;
                ZpGameAndroidApi.enableDebug(false);
            }

            if (((XmlElement)root).HasAttribute(ZpGameContant.DATA_DebugInterval))
            {
                string Dinterval = ((XmlElement)root).GetAttribute(ZpGameContant.DATA_DebugInterval);
                if (Dinterval != null)
                {
                    ZpGameStatic.DebugInterval = float.Parse(Dinterval);
                }
            }
        }
        catch
        {
            ZpGameLog.Log("CONFIG FILE DO'T HAVE THE DEBUG MESSAGE");
        }

        lock (obj)
        {
            foreach (XmlNode sceneNode in root)
            {
                XmlElement Spinfo = (XmlElement)sceneNode;

                SPitem node = new SPitem();
                node.SPUid = Spinfo.GetAttribute("SPUid");
                node.CameraCtlType = (ZpGameContant.StereoCameraType)System.Enum.Parse(typeof(ZpGameContant.StereoCameraType), Spinfo.GetAttribute("CameraCtlType"));
                node.Pupil = float.Parse(Spinfo.GetAttribute("Pupil"));
                node.Focal = float.Parse(Spinfo.GetAttribute("Intersection"));
                node.ToPupil = float.Parse(Spinfo.GetAttribute("Ipoverride"));
                node.ToFocal = float.Parse(Spinfo.GetAttribute("DistanceToTarget"));
                AddSScene(node);
            }
        }
    }
#endregion
}

public class SPitem
{
    private string _SPUid;
    private ZpGameContant.StereoCameraType _SCameraCtlType;
    private float _Pupil;
    private float _Focal;
    private float _ToPupil;
    private float _ToFocal;

    public SPitem(string id, ZpGameContant.StereoCameraType cct, float p, float i)
    {
        _SPUid = id;
        _SCameraCtlType = cct;
        if (cct == ZpGameContant.StereoCameraType.Static)
        {
            _Pupil = p;

            if (i != 0)
                _Focal = i;
            else
                _Focal = 0.01f;

            _ToPupil = 0;
            _ToFocal = 0;
        }
        else if (cct == ZpGameContant.StereoCameraType.Dynamic)
        {
            _Pupil = 0;
            _Focal = 0;
            _ToPupil = p;
            _ToFocal = i;
        }
    }
    public SPitem()
    {

    }

#region Getter & Setter
    /* GETTER || SETTER */
	public string SPUid
    {
        set { _SPUid = value; }
        get { return _SPUid; }
    }

    public float Pupil
    {
        set { _Pupil = value < 0 ? 0.0f : value; }
        get { return _Pupil; }
    }
    public float Focal
    {
        set { _Focal = value < 0 ? 0.0f : value; }
        get { return _Focal; }
    }

    public float ToPupil
    {
        set { _ToPupil = value < 0 ? 0.0f : value; }
        get { return _ToPupil; }
    }

    public float ToFocal
    {
        set { _ToFocal = value < 0 ? 0.0f : value; }
        get { return _ToFocal; }
    }

    public ZpGameContant.StereoCameraType CameraCtlType
    {
        set { _SCameraCtlType = value; }
        get { return _SCameraCtlType; }
    }
#endregion

    public void print()
    {
        ZpGameLog.Log("_SPUid " + _SPUid + " _SCameraCtlType= " + _SCameraCtlType + " _Pupil= " + _Pupil + " _Focal= "+ _Focal);
    }

}

public class Cameraitem
{
    public Camera _CurrentCamera;
    public SPitem _CurrentScene;

    public Cameraitem(Camera cur, SPitem csp)
    {
        _CurrentCamera = cur;
        _CurrentScene = csp;
    }

    public Cameraitem()
    { }
}

#endif
