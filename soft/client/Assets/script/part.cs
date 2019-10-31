
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class part : MonoBehaviour {

	public Transform m_target;
	//public Transform m_sorce;

	// Use this for initialization
	void Start () {

		//GameObject go = (GameObject)Object.Instantiate(gameObjectRequest.asset);

		GameObject _obj = game_data._instance.ins_object_res("Pal_m_050a04");
		_obj = _obj.transform.Find("Pal_m_050a04_Bod").gameObject;

		SkinnedMeshRenderer smr = _obj.transform.GetComponent<SkinnedMeshRenderer>();

		if (smr == null) {
			Debug.Log(game_data._instance.get_t_language ("part.cs_21_13"));//空值
		}

		//Transform _bone = smr.bones[0];

		//Debug.Log(_bone.name);

		Transform[] m_hips = m_target.GetComponentsInChildren<Transform>();

		List<Transform> bones = new List<Transform>();

		foreach(Transform bone in smr.bones)
		{
			foreach(Transform hip in m_hips)
			{		
				if(hip.name != bone.name) continue;		
				bones.Add(hip);	
				Debug.Log(bone.name);
				break;	
			}	
		}

		GameObject partObj = new GameObject();
		partObj.name = "test_part";
		partObj.transform.parent = m_target;
		SkinnedMeshRenderer _target = partObj.AddComponent<SkinnedMeshRenderer>();

		_target.sharedMesh = smr.sharedMesh;
		_target.bones = bones.ToArray();
		_target.materials = smr.materials;

		Object.Destroy (_obj);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
