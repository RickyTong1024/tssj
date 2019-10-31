#if !UNITY_IOS
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ZpGameStereoCamera))]
public class GUIZpGameStereoCameraInspector : Editor
{

    GUIStyle paddingStyle1;

    ZpGameContant.StereoCameraType Type;
    Transform Target;
    float startPD;
    float startFD;
    float startIO;
    float startDTT;
    ZpGameStereoCamera t;

    void OnEnable()
    {
        paddingStyle1 = new GUIStyle();
        paddingStyle1.padding = new RectOffset(15, 0, 0, 0);
        t = (ZpGameStereoCamera)target;
        startPD = t._Pupil;
        startFD = t._Focal;
        startIO = t._ToPupil;
        startDTT = t._ToFocal;
        Type = t._SCameraCtlType;
        Target = t._Target;
    }

    public override void OnInspectorGUI()
    {
        t._Debug = EditorGUILayout.Toggle("Debug", t._Debug);
        ZpGameStatic.DebugType = (t._Debug == true) ? "debug" : "release";

		t.IsCullCtr = EditorGUILayout.Toggle("CullMask", t.IsCullCtr);
		if (t.IsCullCtr)
		{
			t._DefaultCullingMaskOff = EditorGUILayout.LayerField("Main Camera CullMask", t._DefaultCullingMaskOff);
			t.DefaultCullingMask = 1 << t._DefaultCullingMaskOff;
		}
		else
		{
			t._DefaultCullingMaskOff = 0;
			t.DefaultCullingMask = 0;
		}

		t._IsSupportCopy = EditorGUILayout.Toggle("CopyComponet", t._IsSupportCopy);


        t._SPUid = EditorGUILayout.TextField("SPUid", t._SPUid);
        t._SCameraCtlType = (ZpGameContant.StereoCameraType)EditorGUILayout.EnumPopup("CameraType", t._SCameraCtlType);
        if (t._SCameraCtlType == ZpGameContant.StereoCameraType.Static)
        {
            t._Pupil = EditorGUILayout.FloatField("pupil", t._Pupil);
            t._Focal = EditorGUILayout.FloatField("focal", t._Focal);
        }
        else if (t._SCameraCtlType == ZpGameContant.StereoCameraType.Dynamic)
        {
            t._ToFocal = EditorGUILayout.FloatField("ToFocal", t._ToFocal);
            t._ToPupil = EditorGUILayout.FloatField("ToPupil", t._ToPupil);
            t._Target = (Transform)EditorGUILayout.ObjectField("target", t._Target, typeof(Transform), true);
        }

        if (GUI.changed)
        {
            SPitem tmp = ZpGameSceneSet.Instance.GetCameraSP(t.GetComponent<Camera>());

            if (t._SCameraCtlType != Type)
            {
                tmp.CameraCtlType = t._SCameraCtlType;
                Type = t._SCameraCtlType;
            }
            if (t._Target != Target)
            {
                ZpGameSceneSet.Instance.SetTarget(t._Target);
                Target = t._Target;
            }
            if (t._Pupil != startPD)
            { 
                tmp.Pupil = t._Pupil;
                startPD = t._Pupil;
            }
            if (t._Focal != startFD)
            {
                tmp.Focal = t._Focal;
                startFD = t._Focal;
            }
            if (t._ToPupil != startIO)
            {
                tmp.ToPupil = t._ToPupil;
                startIO = t._ToPupil;
            }
            if (t._ToFocal != startDTT)
            {
                tmp.ToFocal = t._ToFocal;
                startDTT = t._ToFocal;
            }

        }
        
    }
}
#else

#endif