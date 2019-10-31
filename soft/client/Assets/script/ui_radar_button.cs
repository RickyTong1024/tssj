
using UnityEngine;
using System.Collections;

public class ui_radar_button : MonoBehaviour {

	public GameObject m_part_1;
	public GameObject m_part_2;
	public GameObject m_part_3;
	public GameObject m_light;
	public GameObject m_cur;
	// Use this for initialization
	void Start () {
	
		TweenAlpha _alpha = TweenAlpha.Begin (m_light, 0.5f, 0);
		_alpha.style = UITweener.Style.PingPong;

	}
	public void OnEnable()
	{
		show ();
	}
	void show()
	{
		m_part_1.transform.localPosition = new Vector3 (0,59,0);
		m_part_2.transform.localPosition = new Vector3 (61,0,0);
		m_part_3.transform.localPosition = new Vector3 (150,150,0);

		TweenPosition _pos_effect = TweenPosition.Begin (m_part_1,0.6f,new Vector3(0,0,0));
		_pos_effect.method = UITweener.Method.Linear;
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 1.1f), new Keyframe(0.8f, 0.95f), new Keyframe(1f, 1f));

		_pos_effect = TweenPosition.Begin (m_part_2,0.6f,new Vector3(0,0,0));
		_pos_effect.method = UITweener.Method.Linear;
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 1.1f), new Keyframe(0.8f, 0.95f), new Keyframe(1f, 1f));

		_pos_effect = TweenPosition.Begin (m_part_3,0.6f,new Vector3(0,0,0));
		_pos_effect.method = UITweener.Method.Linear;
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f),new Keyframe(0.5f, 0f), new Keyframe(0.7f, 1.05f), new Keyframe(0.8f, 0.98f), new Keyframe(1f, 1f));
	}
	void hide()
	{
		m_part_1.transform.localPosition = new Vector3 (0,0,0);
		m_part_2.transform.localPosition = new Vector3 (0,0,0);
		m_part_3.transform.localPosition = new Vector3 (0,0,0);
		
		TweenPosition _pos_effect = TweenPosition.Begin (m_part_1,0.6f,new Vector3(0,59,0));
		_pos_effect.method = UITweener.Method.Linear;
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f),new Keyframe(0.5f, 0f), new Keyframe(0.7f, 1.1f), new Keyframe(0.8f, 0.95f), new Keyframe(1f, 1f));
		
		_pos_effect = TweenPosition.Begin (m_part_2,0.6f,new Vector3(61,0,0));
		_pos_effect.method = UITweener.Method.Linear;
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f),new Keyframe(0.5f, 0f),new Keyframe(0.5f, 0f), new Keyframe(0.7f, 1.1f), new Keyframe(0.8f, 0.95f), new Keyframe(1f, 1f));
		
		_pos_effect = TweenPosition.Begin (m_part_3,0.35f,new Vector3(150,150,0));
		_pos_effect.method = UITweener.Method.Linear;
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 1.05f), new Keyframe(0.8f, 0.98f), new Keyframe(1f, 1f));

	}
	// Update is called once per frame
	void Update () {
	
		Vector3 _rot = m_cur.transform.localEulerAngles;

		_rot.z -= Time.smoothDeltaTime * 180;

		m_cur.transform.localEulerAngles = _rot;
	}
}
