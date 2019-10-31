
using UnityEngine;
using System.Collections;

public class effect_particle_set : MonoBehaviour {

	public float m_speed = 1;
	// Use this for initialization
	void Start () {
	
		this.GetComponent<ParticleSystem>().playbackSpeed = m_speed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
