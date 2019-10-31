#if !UNITY_IOS
using UnityEditor;
using UnityEngine;

public class GUIManageEditor : EditorWindow
{
	public static GUIManageEditor window;
	public GameObject Manage;
    public string SceneID;
    public int layerpostion = 0;
    public int layerinterval = 20;


    [MenuItem("Z+Game/Add init Script")]
    public static void OpenWindow()
    {
		window = (GUIManageEditor)EditorWindow.GetWindow(typeof(GUIManageEditor));
    }

    void OnGUI()
    {

        if (window == null)
        {
            OpenWindow();
        }
			
        GUILayout.Label("Setup", EditorStyles.boldLabel);
		Manage = (GameObject)EditorGUI.ObjectField(new Rect(5, 20, position.width - 10, 16), "Mount Point", Manage, typeof(GameObject), true);

        if (GUI.Button(new Rect(5, 50, position.width - 10, 24), "Ok"))
        {
            Ok();
        }
    }

    void About()
    {
        return;
    }

    void Ok()
    {
		if (Manage == null)
        {
            EditorUtility.DisplayDialog("Specify Parameters", "Please specify the required parameters Mount Point", "Ok");
            return;
        }

		if (Manage.GetComponent<ZpGameManage> () == null) {
			Manage.AddComponent<ZpGameManage> ();
		}
			
    }
}
#else

#endif