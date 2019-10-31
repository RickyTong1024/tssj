
using UnityEngine;
using System.Collections;

public class play_audio : MonoBehaviour {

	public float m_time = 0.0f;
	public AudioClip m_clip;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(m_time > 0)
		{
			m_time -= Time.deltaTime;
			return;
		}
		
		AudioSource _source = transform.gameObject.AddComponent<AudioSource>();
		
		_source.clip = m_clip;
		_source.Play();
		
		Object.Destroy(_source,m_clip.length);
		Object.Destroy(this);
	}
}
