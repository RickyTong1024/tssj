
using UnityEngine;
using System.Collections;

public class effect_sound : MonoBehaviour {

	public string m_sound_name;
	public float m_delay = 0.0f;
	private float m_time = 0.0f;

	// Use this for initialization
	void Start () {

		if(m_delay <= 0)
		{
			m_time = m_delay;
			play_sound ();
		}

	}

	public void OnEnable()
	{
		if(m_delay > 0)
		{
			m_time = m_delay;
		}
		else
		{
			play_sound ();
		}
	}

	void play_sound()
	{
		if(m_time <= 0)
		{
			sys._instance.play_sound(m_sound_name);
			//GameObject.Destroy(this);
		}
	}
	// Update is called once per frame
	void Update () {
	
		if(m_time > 0)
		{
			m_time -= Time.deltaTime;

			play_sound();
		}
	}
}
