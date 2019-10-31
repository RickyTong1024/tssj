using System.Collections.Generic;
using UnityEngine;

public class guild_fb_scene : MonoBehaviour, IMessage {

	public List<GameObject> m_s;
	public GameObject m_sj;
	public List<GameObject> m_sjs;
	public List<GameObject> m_bjs;
	public Camera m_cam;
	
	public static guild_fb_scene _instance;

	void Awake()
	{
		_instance = this;
	}

	void Start()
	{
		cmessage_center._instance.add_handle(this);
	}

	void OnDestroy()
	{
		cmessage_center._instance.remove_handle (this);
	}

	void IMessage.message(s_message message)
	{
		if (message.m_type == "guild_fb_change_scene")
		{
			for (int i = 0; i < m_bjs.Count; ++i)
			{
				m_bjs[i].SetActive(false);
			}
			int index = (int)message.m_ints[0];
            index = index % 4;
			m_bjs[index].SetActive(true);
            
			m_sj = m_sjs[index];
		}
	}

	void IMessage.net_message(s_net_message message)
	{

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
		{
			Ray ray = root_gui._instance.get_ui_cam().ScreenPointToRay(Input.mousePosition);
			RaycastHit hitInfo;
			float dis = 20f;
			if (Physics.Raycast(ray, out hitInfo, dis))
			{
				return ;
			}
			ray = m_cam.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray,out hitInfo))
			{
				GameObject gameObj = hitInfo.transform.gameObject;
				string name = gameObj.name;
				if(gameObj.transform.tag == "guild_fb")
				{
					if (name == "sj")
					{
						gameObj = m_sj;
					}
					else
					{
						int index = int.Parse(name.Substring(1));
						gameObj = m_s[index - 1];
					}
					gameObj.transform.localScale = Vector3.one;
					TweenScale _scale = TweenScale.Begin (gameObj, 0.35f, new Vector3(1.1f,1.1f,1.1f));
					_scale.updateTable = true;
					_scale.method = UITweener.Method.Linear;
					_scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f));

					s_message msg = new s_message();
					msg.m_type = "guild_fb_click";
					msg.m_string.Add(name);
					cmessage_center._instance.add_message(msg);
				}
			}
		}
	}
}
