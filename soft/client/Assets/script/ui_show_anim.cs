
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ui_show_anim : MonoBehaviour {

	public GameObject[] m_alpha_anim;
	public GameObject[] m_scale_anim;

	public bool m_active = false;

	private float m_scale_speed = 0.1f;
	private float m_alpha_speed = 0.1f;

	private static List<ui_show_anim> m_objs = new List<ui_show_anim>();
	// Use this for initialization
	void Start () {
		for(int i = 0;i < m_alpha_anim.Length;i ++)
		{
			UIPanel _panel = m_alpha_anim[i].GetComponent<UIPanel>();
			
			if(_panel != null)
			{
				_panel.alpha = 0;
			}
			
			UISprite _sprite = m_alpha_anim[i].GetComponent<UISprite>();
			
			if(_sprite != null)
			{
				_sprite.alpha = 0;
			}
		}
	
	}

	public void OnEnable()
	{
		show_ui ();
	}
	public void OnDisable()
	{

	}
	public void onFinished()
	{
		this.gameObject.SetActive(m_active);
	}

	public void show_ui()
	{
		//this.gameObject.SetActive(true);
		//m_cur_val = 0.0f;
		//m_target_val = 1.0f;
		//Update ();
		if (m_active)
		{
			return;
		}
		m_active = true;

		for(int i = 0;i < m_alpha_anim.Length;i ++)
		{
			UIPanel _panel = m_alpha_anim[i].GetComponent<UIPanel>();
			
			if(_panel != null)
			{
				_panel.alpha = 0;
			}
			
			UISprite _sprite = m_alpha_anim[i].GetComponent<UISprite>();
			
			if(_sprite != null)
			{
				_sprite.alpha = 0;
			}
		}

		for(int i = 0;i < m_scale_anim.Length;i ++)
		{
			Vector3 _scale = m_scale_anim[i].transform.localScale;
			
			_scale.x = 0;
			_scale.y = 0;
			_scale.z = 0;
			
			m_scale_anim[i].transform.localScale = _scale;
			
		}

		for(int i = 0;i < m_scale_anim.Length;i ++)
		{
			m_scale_anim[i].transform.localScale = new Vector3(0,0,0);

			TweenScale _scale = TweenScale.Begin (m_scale_anim[i].gameObject,m_scale_speed,new Vector3(1,1,1));

			_scale.updateTable = true;

			_scale.method = UITweener.Method.Linear;

			_scale.animationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(0.7f, 1.05f), new Keyframe(0.8f, 0.97f), new Keyframe(1f, 1f));
		}

		for(int i = 0;i < m_alpha_anim.Length;i ++)
		{
			TweenAlpha _alpha = TweenAlpha.Begin(m_alpha_anim[i].gameObject,m_alpha_speed,1.0f);
		}

		root_gui._instance.do_mask (m_alpha_speed);
	}
	public void hide_ui()
	{
		if (!m_active)
		{
			return;
		}
		m_active = false;

		for(int i = 0;i < m_scale_anim.Length;i ++)
		{
			TweenScale _scale = TweenScale.Begin (m_scale_anim[i].gameObject,m_scale_speed,new Vector3(0,0,0));

			_scale.method = UITweener.Method.EaseInOut;

			EventDelegate.Add (_scale.onFinished, onFinished);
		}

		for(int i = 0;i < m_alpha_anim.Length;i ++)
		{
			TweenAlpha _alpha = TweenAlpha.Begin(m_alpha_anim[i].gameObject,m_alpha_speed,0.0f);

			EventDelegate.Add (_alpha.onFinished, onFinished);
		}

		root_gui._instance.do_mask (m_alpha_speed);
		//m_cur_val = 1.0f;
		//m_target_val = 0.0f;
		//Update ();
	}
	// Update is called once per frame
	void Update () {

		}
}
