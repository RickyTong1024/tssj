
using UnityEngine;
using System.Collections;

public class ui_hall_rb_button : MonoBehaviour {

	public GameObject m_bottom;
	public GameObject m_icons_1;
	// Use this for initialization
	void Start () {
	
		on ();
	}
	public void click(GameObject obj)
	{
		if(obj.transform.name == "switch")
		{
			if(obj.transform.GetComponent<UIToggle>().value)
			{
				on ();
			}
			else
			{
				off ();
			}
		}
	}
	public void OnEnable()
	{
		//on ();
		//show ();
	}

	void show()
	{
		transform.localPosition = new Vector3 (0,0,0);

		TweenPosition _pos_effect = TweenPosition.Begin (transform.gameObject,0.6f,new Vector3(-94,90,0));
		_pos_effect.method = UITweener.Method.Linear;
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 1.0f), new Keyframe(0.8f, 0.95f), new Keyframe(1f, 1f));
	}

	void hide()
	{
		transform.localPosition = new Vector3(-94,90,0);

		TweenPosition.Begin (transform.gameObject,0.6f,new Vector3(0,0,0));
	}

	public void off()
	{
		TweenPosition _pos_effect = TweenPosition.Begin (m_bottom,0.3f,new Vector3(650,0,0));
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 0.0f), new Keyframe(1f, 1f));

		TweenAlpha _effect = TweenAlpha.Begin (m_icons_1, 0.05f, 0);
		_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 0.0f), new Keyframe(1f, 1f));
	}

	public void on()
	{
		TweenPosition _pos_effect = TweenPosition.Begin (m_bottom,0.3f,new Vector3(0,0,0));
		_pos_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 1.02f), new Keyframe(0.8f, 0.99f), new Keyframe(1f, 1f));

		TweenAlpha _effect = TweenAlpha.Begin (m_icons_1, 0.3f, 1);
		_effect.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 0.0f), new Keyframe(1f, 1f));
	}

	// Update is called once per frame
	void Update () {
	
	}
}
