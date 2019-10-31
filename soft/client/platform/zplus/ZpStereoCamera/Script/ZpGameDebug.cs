#if !UNITY_IOS
using UnityEngine;
using System.Collections;

public class ZpGameDebug : MonoBehaviour {
    private int KeyTime = 0;
    private bool IsKeyDown = false;
    private KeyCode DownKey = KeyCode.Space;
    private int Select = 0;
    private float Timer = 0;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        if (ZpGameStatic.DebugType == "debug")
        {
            GUIStyle fontStyle = new GUIStyle();
            fontStyle.fontSize = 30;
            fontStyle.normal.textColor = new Color(1, 1, 1);

            GUIStyle fontStyle1 = new GUIStyle();
            fontStyle1.fontSize = 30;
            fontStyle1.normal.textColor = new Color(1, 0, 0);

            if (TouchKey(KeyCode.JoystickButton7) || TouchKey(KeyCode.JoystickButton4))
            {
                Select++;
            }

            for (int index = 0; index < ZpGameSceneSet.Instance.GetCurrentCC(); index++)
            {
                SPitem tmp = ZpGameSceneSet.Instance.GetCameraSP(index);
                Select = Select % ZpGameSceneSet.Instance.GetCurrentCC();

                if(index != Select)
                {
                    if (tmp.CameraCtlType == ZpGameContant.StereoCameraType.Static)
                    {
                        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                        GUI.Label(new Rect(Screen.width / 6, index*25, 500, 100), "OUT SID= " + tmp.SPUid + "  PD= " + tmp.Pupil.ToString("F2") + "  FD= " + tmp.Focal.ToString("F2"), fontStyle);
                        GUI.EndGroup();
                    }
                    else if (tmp.CameraCtlType == ZpGameContant.StereoCameraType.Dynamic)
                    {
                        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                        GUI.Label(new Rect(Screen.width / 6, index*25, 500, 100), "OUT SID= " + tmp.SPUid + "  OR= " + tmp.ToFocal.ToString("F2") + "  DTT= " + tmp.ToPupil.ToString("F2"), fontStyle);
                        GUI.EndGroup();
                    }
                }
                else
                {

                    if (tmp.CameraCtlType == ZpGameContant.StereoCameraType.Static)
                    {
                        if (TouchKey(KeyCode.U) || TouchKey(KeyCode.JoystickButton0))
                            tmp.Focal += ZpGameStatic.DebugInterval;
                        if (TouchKey(KeyCode.I) || TouchKey(KeyCode.JoystickButton1))
                            if (tmp.Focal - ZpGameStatic.DebugInterval > ZpGameStatic.DebugInterval)
                                tmp.Focal -= ZpGameStatic.DebugInterval;
                        if (TouchKey(KeyCode.O) || TouchKey(KeyCode.JoystickButton2))
                            tmp.Pupil += ZpGameStatic.DebugInterval;
                        if (TouchKey(KeyCode.P) || TouchKey(KeyCode.JoystickButton3))
                            if (tmp.Pupil - ZpGameStatic.DebugInterval > ZpGameStatic.DebugInterval)
                                tmp.Pupil -= ZpGameStatic.DebugInterval;


                        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                        GUI.Label(new Rect(Screen.width / 6, index * 25, 500, 100), "OUT SID= " + tmp.SPUid + "  PD= " + tmp.Pupil.ToString("F2") + "  FD= " + tmp.Focal.ToString("F2"), fontStyle1);
                        GUI.EndGroup();
                    }
                    else if (tmp.CameraCtlType == ZpGameContant.StereoCameraType.Dynamic)
                    {
                        if (TouchKey(KeyCode.U) || TouchKey(KeyCode.JoystickButton0))
                            tmp.ToPupil += ZpGameStatic.DebugInterval;
                        if (TouchKey(KeyCode.I) || TouchKey(KeyCode.JoystickButton1))
                            if (tmp.ToPupil - ZpGameStatic.DebugInterval > ZpGameStatic.DebugInterval)
                                tmp.ToPupil -= ZpGameStatic.DebugInterval;
                        if (TouchKey(KeyCode.O) || TouchKey(KeyCode.JoystickButton2))
                            tmp.ToFocal += ZpGameStatic.DebugInterval;
                        if (TouchKey(KeyCode.P) || TouchKey(KeyCode.JoystickButton3))
                            if (tmp.ToFocal - ZpGameStatic.DebugInterval > ZpGameStatic.DebugInterval)
                                tmp.ToFocal -= ZpGameStatic.DebugInterval;


                        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                        GUI.Label(new Rect(Screen.width / 6, index * 25, 500, 100), "OUT SID= " + tmp.SPUid + "  DTT= " + tmp.ToFocal.ToString("F2") + "  OR= " + tmp.ToPupil.ToString("F2"), fontStyle1);
                        GUI.EndGroup();
                    }
                    ZpGameSceneSet.Instance.reload();

                    if (ZpGameDevelopApi.getScreenType() == ZpGameContant.AEffects.SlantWeaved)
                    {
                        float[] ShaderPara = EyeTrack.Instance.getEyetrackData();

                        GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                        GUI.Label(new Rect(Screen.width / 6, 100, 500, 100), "OUT ctview= " + ShaderPara[3] + "  SLANT= " + ShaderPara[2].ToString("F2") + " PITCH= " + ShaderPara[1], fontStyle1);
                        GUI.EndGroup();
                    }
                }
            }

            if (Input.GetKey(KeyCode.F10) || Input.GetKey(KeyCode.JoystickButton6))
            {
                GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
                GUI.Label(new Rect(Screen.width / 6, ZpGameSceneSet.Instance.GetCurrentCC()*25, 500, 100), "SAVE", fontStyle);
                GUI.EndGroup();
                ZpGameSceneSet.Instance.saveToCfgFile();
            }

            if (TouchKey(KeyCode.F12) || TouchKey(KeyCode.JoystickButton7))
            {
                KeyTime++;
                //_WeavedMaterial.SetInt("_RB", KeyTime % 2);
            }
            //if (Input.anyKeyDown)
            //{
            //    foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            //    {
            //        if (TouchKeyDown(keyCode))
            //        {
            //            ZpGameLog.Log(keyCode.ToString());
            //        }
            //    }
            //}
        }
    }
    bool TouchKey(KeyCode tmp)
    {
        if (Input.GetKeyDown(tmp) && !IsKeyDown)
        {
            IsKeyDown = true;
            DownKey = tmp;
            Timer = Time.time;
        }
        else if (Input.GetKeyUp(tmp) && IsKeyDown)
        {
                IsKeyDown = false;
                return true;
        }
        else if (Input.GetKey(tmp) && IsKeyDown)
        {
            if (Input.GetKey(DownKey))
            {
                if (Time.time - Timer > 0.3)
                    return true;
            }
        }
        return false;
    }
}

public enum ZpGameLogLevel  //Log等级
{
    Everything,
    Normal,
    Important,
    Emergy
}

public static class ZpGameLog
{
	public static ZpGameLogLevel curMsgType = ZpGameLogLevel.Normal; //当前允许的打印等级

    public static void Log(string msg, ZpGameLogLevel type = ZpGameLogLevel.Normal)
    {
        if (type < curMsgType || ZpGameStatic.DebugType == "release")  //只允许打印比curMsgType等级高的Log,方便Log等级管理
        {
            return;
        }
		Debug.LogWarning(msg);
    }

    public static void Log(string msg, GameObject go, ZpGameLogLevel type = ZpGameLogLevel.Normal)
    {
        if (type < curMsgType || ZpGameStatic.DebugType == "release")
        {
            return;
        }
		Debug.LogWarning(msg, go);
    }

    public static void LogWarning(string msg, ZpGameLogLevel type = ZpGameLogLevel.Normal)
    {
        if (type < curMsgType || ZpGameStatic.DebugType == "release")
        {
            return;
        }
        Debug.LogWarning(msg);
    }
}

#region Gizmos
public static class StereoCameraGizmos
{
    public static void DrawFrustumGizmos(Camera cam)
    {
        // Frustum
        Matrix4x4 tmp = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(cam.transform.position, cam.transform.rotation, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawFrustum(Vector3.zero, cam.fieldOfView, cam.farClipPlane, cam.nearClipPlane, cam.aspect);
        Gizmos.matrix = tmp;
    }

    public static void DrawScreenPlane(Camera cam, float focalDis)
    {
        Matrix4x4 tmp = Gizmos.matrix;
        Vector3 screenPlaneData = GetScreenPlane(focalDis, cam.fieldOfView, cam.aspect);
        Gizmos.color = new Color(0, 0, 1, 0.2F); // blue
        Vector3 spCenter = cam.transform.position + (cam.transform.TransformDirection(Vector3.forward) * focalDis);
        Gizmos.matrix = cam.transform.localToWorldMatrix;
        Gizmos.matrix = Matrix4x4.TRS(spCenter, cam.transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, new Vector3(screenPlaneData.x, screenPlaneData.y, 0.05f));
        Gizmos.matrix = tmp;
    }

    public static void DrawNearPlane(Camera cam, float focalDis, float stereoBasis)
    {
        Matrix4x4 tmp = Gizmos.matrix;
        Vector3 nearPlaneData = GetNearPlane(stereoBasis, focalDis, cam.fieldOfView, cam.aspect);
        Gizmos.color = new Color(0, 1, 0, 0.2F); // green
        Vector3 npCenter = cam.transform.position + (cam.transform.TransformDirection(Vector3.forward) * nearPlaneData.z);
        Gizmos.matrix = cam.transform.localToWorldMatrix;
        Gizmos.matrix = Matrix4x4.TRS(npCenter, cam.transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, new Vector3(nearPlaneData.x, nearPlaneData.y, 0.05f));
        Gizmos.matrix = tmp;

    }

    public static void DrawFarPlane(Camera cam, float focalDis, float stereoBasis)
    {
        Matrix4x4 tmp = Gizmos.matrix;
        Vector3 farPlaneData = GetFarPlane(stereoBasis, focalDis, cam.fieldOfView, cam.aspect);
        Gizmos.color = new Color(1, 0, 0, 0.2F); // red
        Vector3 fpCenter = cam.transform.position + (cam.transform.TransformDirection(Vector3.forward) * farPlaneData.z);
        Gizmos.matrix = cam.transform.localToWorldMatrix;
        Gizmos.matrix = Matrix4x4.TRS(fpCenter, cam.transform.rotation, Vector3.one);
        Gizmos.DrawCube(Vector3.zero, new Vector3(farPlaneData.x, farPlaneData.y, 0.05f));
        Gizmos.matrix = tmp;
    }
    //to do: if any object of interest runs across the boarder, notify ui for some action: eg, hide/pop out
    public static Vector3 GetNearPlane(float stereoBasis, float focalDis, float fov, float aspect)
    {
        float D_ = -WeaveConfig.safeDisparity;//safe disparity range on screen in mm
        float W_ = WeaveConfig.windowWidth;//screen width in mm
        float w = ((Mathf.Tan((fov / 2) * Mathf.Deg2Rad) * focalDis)) * aspect;
        //z=ef/(e-dis); dis=D'/W'2tan(fov/2)*f*aspect; z=f/(1-2*D'/W'*f/e*tan(fov/2)*aspect)
        float z = focalDis / (1 - 2 * D_ / W_ * w / stereoBasis);
        //z=-k2x+d; where k2=f/(e+w) d=fw/(e+w); set the axis base at the center of zero parallax plane
        float n = (focalDis - z) / focalDis * 10;   //measured with 1/10f
        float width = ((10 - n) * w / 10 - stereoBasis / 2 * n / 10) * 2;
        float height = width / aspect;
        return new Vector3(width, height, z);
    }

    public static Vector3 GetScreenPlane(float focalDis, float fov, float aspect)
    {
        float H = focalDis;
        float width = ((Mathf.Tan((fov / 2) * Mathf.Deg2Rad) * (H)) * 2) * aspect;
        float height = width / aspect;
        //ZpGameLog.Log("aspect: "+aspect+" w "+width+" h "+height);
        return new Vector3(width, height, focalDis);
    }

    public static Vector3 GetFarPlane(float stereoBasis, float focalDis, float fov, float aspect)
    {
        float D_ = WeaveConfig.safeDisparity;
        float W_ = WeaveConfig.windowWidth;
        float w = ((Mathf.Tan((fov / 2) * Mathf.Deg2Rad) * focalDis)) * aspect;
        float temp = 1 - 2 * D_ / W_ * w / stereoBasis;
        float z = focalDis / (temp < 0 ? 0.001f : temp);
        float n = (focalDis - z) / focalDis * 10;
        float width = ((10 - n) * w / 10 - stereoBasis / 2 * n / 10) * 2;
        float height = width / aspect;
        return new Vector3(width, height, z);
    }
}

public static class WeaveConfig
{
    /*
     * Default Config...
     */
#region Values
    public static float StereoBasisAdjustValue = 0.01f;
    public static float FocalDistanceAdjustValue = 0.2f;
    public static float ToeinAdjustValue = 0.02f;
    public static float MaxStereoBasis = 20.0f;
    public static float MaxFocalDistance = 100.0f;
    public static float MaxToein = 1.0f; //degree
    public static bool enableDebugging = false;
    public static float safeDisparity = 2.52f; // mm
    public static float windowWidth = 152.0f; //mm
#endregion
}
#endregion
#endif