
using UnityEngine;
using System.Collections;

public class scene_anim_action : MonoBehaviour {

	public string m_load_name;
	// Use this for initialization
	void Start () {
	
	}
	public void click(GameObject obj)
	{
		if(obj.transform.name == "close")
		{
			sys._instance.m_game_state = "login";
			sys._instance.load_scene(m_load_name);
		}
	}
	public void load_scene(string name)
	{
		sys._instance.m_game_state = "login";
		sys._instance.load_scene(name);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
