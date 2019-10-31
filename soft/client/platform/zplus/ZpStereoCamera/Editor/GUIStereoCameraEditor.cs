#if !UNITY_IOS
using UnityEditor;
using UnityEngine;

public class GUIStereoCameraEditor : EditorWindow
{
    public static GUIStereoCameraEditor window;
    public GameObject camera;
    public string SceneID;
    public ZpGameContant.StereoCameraType Type = ZpGameContant.StereoCameraType.None;
    public float pupil = 0.065f;
    public float focal = 2.6f;
    public float ToFocal = 1.0f;
    public float ToPupil = 50.0f;
    public Transform target;
    public int layerpostion = 0;
    public int layerinterval = 20;


    [MenuItem("Z+Game/Add StereoCamera")]
    public static void OpenWindow()
    {
        window = (GUIStereoCameraEditor)EditorWindow.GetWindow(typeof(GUIStereoCameraEditor));
    }

    void OnGUI()
    {

        if (window == null)
        {
            OpenWindow();
        }

        GUILayout.Label("StereCamera Setup", EditorStyles.boldLabel);
        camera = (GameObject)EditorGUI.ObjectField(new Rect(5, 20, position.width - 10, 16), "Main Camera", camera, typeof(GameObject), true);

        if (GUI.Button(new Rect(5, 50, position.width - 10, 24), "About"))
        {
            About();
        }

        if (GUI.Button(new Rect(5, 80, position.width - 10, 24), "Ok"))
        {
            Ok();
        }
    }

    void About()
    {
        EditorUtility.DisplayDialog("Specify Parameters", "We need Add main camera", "Ok");
        return;
    }

    void Ok()
    {
        if (camera == null)
        {
            EditorUtility.DisplayDialog("Specify Parameters", "Please specify the required parameters camera", "Ok");
            return;
        }

        ZpGameStereoCamera tmp_2 = camera.GetComponentInParent(typeof(ZpGameStereoCamera)) as ZpGameStereoCamera;
        if (tmp_2 == null)
        {
            tmp_2 = camera.AddComponent<ZpGameStereoCamera>();
            tmp_2._SPUid = SceneID;
            if (Type == ZpGameContant.StereoCameraType.Static)
            {
                tmp_2._SCameraCtlType = ZpGameContant.StereoCameraType.Static;
                tmp_2._Pupil = pupil;
                tmp_2._Focal = focal;
            }
            else if (Type == ZpGameContant.StereoCameraType.Dynamic)
            { 
                tmp_2._SCameraCtlType = ZpGameContant.StereoCameraType.Dynamic;
                tmp_2._IsTargetMode = true;
                tmp_2._ToFocal = ToFocal;
                tmp_2._ToPupil = ToPupil;
                tmp_2._Target = target;
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Specify Parameters", "Double use", "Ok");
            return;
        }
    }
}
#else

#endif