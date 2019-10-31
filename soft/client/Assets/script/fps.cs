
using UnityEngine;
using System.Collections;

public class fps : MonoBehaviour {
	public float updateInterval = 0.5F;
	private float lastInterval;
	public float m_old_time = 0;
	public int m_show_fps = 0;
	private int frames = 0;
	private float cfps;
	void Start() {
		lastInterval = Time.realtimeSinceStartup;
		frames = 0;
	}
	void OnGUI() {
		GUILayout.Label("" + m_show_fps);
	}
	void Update() {
		++frames;
		float timeNow = Time.time;
		if (timeNow - m_old_time > 1.0f) {

			m_old_time = timeNow;

			m_show_fps = frames;
			frames = 0;
		}
	}
}
