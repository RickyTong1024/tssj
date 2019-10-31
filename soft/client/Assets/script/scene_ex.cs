using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class scene_ex : MonoBehaviour,IMessage {

	public bool m_Fog;
	public Color m_Fog_Color;
	public FogMode m_Fog_Mode;
	public float m_FogDensity;
	public float m_linear_fog_start = 0;
	public float m_linear_fog_end = 0;
	public Color m_Ambient_Light;
	public Material m_skybox;
	public bool m_add_scene = true;
	private string m_director;

	public List<GameObject> m_directs = new List<GameObject>();
	// Use this for initialization
	void Start () {
	
		if(sys._instance == null)
		{
			RenderSeting();
		}
		else if(m_add_scene)
		{
			RenderSeting();
		}

		cmessage_center._instance.add_handle (this);
	}
	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}
	public void OnEnable()
	{
		RenderSeting();
	}

	void RenderSeting()
	{
		RenderSettings.fog = m_Fog;
		RenderSettings.fogColor = m_Fog_Color;
		RenderSettings.fogMode = m_Fog_Mode;
		RenderSettings.fogDensity = m_FogDensity;
		RenderSettings.fogStartDistance = m_linear_fog_start;
		RenderSettings.fogEndDistance = m_linear_fog_end;
		RenderSettings.ambientLight = m_Ambient_Light;
		RenderSettings.skybox = m_skybox;
	}
	void IMessage.net_message(s_net_message message)
	{
		
	}
	
	void IMessage.message(s_message message)
	{
		if(message.m_type == "show_director")
		{
			show_direct((string)message.m_string[0]);
		}
		if(message.m_type == "hide_director")
		{
			hide_direct();
		}
	}

	void show_direct(string name)
	{
		for(int i = 0;i < m_directs.Count;i ++)
		{
			if(m_directs[i].transform.name == name)
			{
				m_directs[i].SetActive(true);
				sys._instance.m_is_director = true;
				sys._instance.m_buttle_cam.SetActive(false);
				root_gui._instance.hide_battle_gui();
				sys._instance.m_root_unit.SetActive(false);
				root_gui._instance.m_ui_bottomleft.SetActive(false);
				root_gui._instance.m_ui_bottomleft_1.SetActive(false);
				root_gui._instance.show_mask();
				m_director = name;
			}
		}
	}
	void hide_direct()
	{
		for(int i = 0;i < m_directs.Count;i ++)
		{
			if(m_directs[i].transform.name == m_director)
			{
				m_directs[i].SetActive(false);
				sys._instance.m_is_director = false;
				sys._instance.m_buttle_cam.SetActive(true);
				root_gui._instance.show_battle_gui();
				sys._instance.m_root_unit.SetActive(true);
				root_gui._instance.m_ui_bottomleft.SetActive(true);
				root_gui._instance.m_ui_bottomleft_1.SetActive(true);
				root_gui._instance.show_mask();
			}
		}
	}
}
