#if !UNITY_IOS
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ZpGameManage))]
public class GUIZpGameManageInspector : Editor
{

    GUIStyle paddingStyle1;
    ZpGameManage t;
    //public bool debug;

    public bool WidgetTrigger;
    [Range(0, 1)]
    public float Widget_x = 0;
    [Range(0, 1)]
    public float Widget_y = 0;

    public bool StereoTrigger = true;

    public int EyeTextureWidth ;
    public int EyeTextureHeight;
	public ZpGameStatic.RenderTextureDepth EyeTextureDepth ;
	public ZpGameStatic.RenderTextureRect EyeTextureRect;


    void OnEnable()
    {
        paddingStyle1 = new GUIStyle();
        paddingStyle1.padding = new RectOffset(15, 0, 0, 0);
        t = (ZpGameManage)target;

        Widget_x = t.Widget_x;
        Widget_y = t.Widget_y;
        //debug = t._Debug;
        StereoTrigger = t.StereoTrigger;
        EyeTextureDepth = t.EyeTextureDepth;
        EyeTextureRect = t.EyeTextureRect;

    }

    public override void OnInspectorGUI()
    {
        t._Debug = EditorGUILayout.Toggle("Debug", t._Debug);

        t.StereoTrigger = EditorGUILayout.Toggle("StereoTrigger", t.StereoTrigger);
		if (t.StereoTrigger == true) {
			t.Widget_x = EditorGUILayout.FloatField ("Widget_x", t.Widget_x);
			t.Widget_y = EditorGUILayout.FloatField ("Widget_y", t.Widget_y);

			t.EyeTextureDepth = (ZpGameStatic.RenderTextureDepth)EditorGUILayout.EnumPopup ("Texture Depth", t.EyeTextureDepth);
			ZpGameStatic.EyeTextureDepth = t.EyeTextureDepth;
			t.EyeTextureRect = (ZpGameStatic.RenderTextureRect)EditorGUILayout.EnumPopup ("Texture Rect", t.EyeTextureRect);
			ZpGameStatic.EyeTextureRect = t.EyeTextureRect;        

			t.screenType = (ZpGameContant.AEffects)EditorGUILayout.EnumPopup ("Screen Type", t.screenType);
		} else {
			t.screenType = ZpGameContant.AEffects.Original;
		}
    }
}
#else

#endif