
var EffectPath = "hit_shield";

function Start () {

}

function Update () {
	if(Input.GetMouseButtonDown(0)){
		var ray = Camera.main.ScreenPointToRay (Input.mousePosition);    
 		var hit : RaycastHit;    
		if (Physics.Raycast (ray, hit))
        {
#if  UNITY_EDITOR && !UNITY_WEBPLAYER
	 		var strPrefabName = "Assets/Effect/"+EffectPath+".prefab";
            var Effect = UnityEditor.AssetDatabase.LoadAssetAtPath(strPrefabName, typeof(Object));
	 		Instantiate(Effect, hit.point,hit.transform.rotation);
#endif
		}
	}
}